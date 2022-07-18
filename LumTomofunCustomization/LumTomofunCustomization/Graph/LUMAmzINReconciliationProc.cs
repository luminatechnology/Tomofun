using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using LUMTomofunCustomization.DAC;
using LumTomofunCustomization.Graph;
using FikaAmazonAPI;
using FikaAmazonAPI.Parameter.Report;
using FikaAmazonAPI.Utils;
using static FikaAmazonAPI.Utils.Constants;

namespace LUMTomofunCustomization.Graph
{
    public class LUMAmzINReconciliationProc : PXGraph<LUMAmzINReconciliationProc>
    {
        #region Features
        public PXCancel<SettlementFilter> Cancel;

        public PXFilter<SettlementFilter> Filter;
        public PXFilteredProcessing<LUMAmzINReconcilition, SettlementFilter/*, Where<LUMAmzINReconcilition.isProcesses, Equal<False>>*/> Reconcilition;
        //public PXSelectReadonly<LUMAmzINReconcilition, Where<LUMAmzINReconcilition.isProcesses, Equal<True>>> PrcoessedReconcil;

        public PXSetup<LUMMWSPreference> Setup;
        #endregion

        #region Ctor
        public LUMAmzINReconciliationProc()
        {
            if (Reconcilition.Select().Count == 0) { InsertInitializedData(); }

            Actions.Move(nameof(Cancel), nameof(massDeletion), true);
            Actions.Move(nameof(massDeletion), nameof(importFBAIN), true);
            Actions.Move(nameof(importFBAIN), nameof(createAdjustment), true);

            Reconcilition.SetProcessVisible(false);
            Reconcilition.SetProcessAllCaption("Import & Create");
            Reconcilition.SetProcessDelegate(delegate (List<LUMAmzINReconcilition> lists)
            {
                ImportRecords(lists);
            });
        }
        #endregion

        #region Actions
        public PXAction<SettlementFilter> massDeletion;
        [PXButton(CommitChanges = true, ImageKey = PX.Web.UI.Sprite.Main.RecordDel), PXUIField(DisplayName = "Mass Delete")]
        protected virtual IEnumerable MassDeletion(PXAdapter adapter)
        {
            foreach (LUMAmzINReconcilition row in Reconcilition.Cache.Updated)
            {
                if (row.Selected == true && row.IsProcesses == false)
                {
                    Reconcilition.Delete(row);
                }
            }

            Actions.PressSave();

            return adapter.Get();
        }

        public PXAction<SettlementFilter> importFBAIN;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Import FBA IN")]
        protected virtual IEnumerable ImportFBAIN(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, delegate ()
            {
                ImportAmzRecords();
            });
            
            return adapter.Get();
        }

        public PXAction<SettlementFilter> createAdjustment;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Create In Adjustment")]
        protected virtual IEnumerable CreateAdjustment(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, delegate ()
            {
                CreateInvAdjustment(Reconcilition.Cache.Updated.RowCast<LUMAmzINReconcilition>().ToList());

                foreach (LUMAmzINReconcilition row in Reconcilition.Cache.Updated)
                {
                    Reconcilition.Cache.SetValue<LUMAmzINReconcilition.isProcesses>(row, true);
                    Reconcilition.Update(row);
                }

                this.Actions.PressSave();
            });

            return adapter.Get();
        }
        #endregion

        #region Cache Attached
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIField(DisplayName = "Date")]
        [PXDefault(typeof(AccessInfo.businessDate))]
        protected virtual void _(Events.CacheAttached<SettlementFilter.fromDate> e) { }
        #endregion

        #region Static Methods
        public static void ImportRecords(List<LUMAmzINReconcilition> lists)
        {
            LUMAmzINReconciliationProc graph = CreateInstance<LUMAmzINReconciliationProc>();

            graph.ImportAmzRecords();
            graph.CreateInvAdjustment(lists);
        }
        #endregion

        #region Methods
        public virtual (string marketPlaceID, string refreshToken) GetAmzCredentialInfo(LUMMWSPreference preference, string marketPlace)
        {
            switch (marketPlace)
            {
                case "US":
                case "CA":
                case "MX":
                    return (preference.USMarketplaceID, preference.USRefreshToken);
                case "AU":
                    return (preference.AUMarketplaceID, preference.AURefreshToken);
                case "JP":
                    return (preference.JPMarketplaceID, preference.JPRefreshToken);
                case "SG":
                    return (preference.SGMarketplaceID, preference.SGRefreshToken);
                default :
                    return (preference.EUMarketplaceID, preference.EURefreshToken);
            }
        }

        public virtual AmazonConnection GetAmazonConnObject(LUMMWSPreference preference, string marketPlace, bool IsSingapore, out string mpID)
        {
            (string marketPlaceID, string refreshToken) = GetAmzCredentialInfo(preference, marketPlace);

            mpID = marketPlaceID;

            return new AmazonConnection(new AmazonCredential()
            {
                AccessKey    = IsSingapore == false ? preference.AccessKey    : preference.SGAccessKey,
                SecretKey    = IsSingapore == false ? preference.SecretKey    : preference.SGSecretKey,
                RoleArn      = IsSingapore == false ? preference.RoleArn      : preference.SGRoleArn,
                ClientId     = IsSingapore == false ? preference.ClientID     : preference.SGClientID,
                ClientSecret = IsSingapore == false ? preference.ClientSecret : preference.SGClientSecret,
                RefreshToken = refreshToken,
                MarketPlace  = MarketPlace.GetMarketPlaceByID(marketPlaceID),
            });
        }

        public virtual List<FikaAmazonAPI.AmazonSpApiSDK.Models.Reports.Report> GetFulfillmentInventoryReports(AmazonConnection amzConnection, DateTime? filterDate, string marketPlace)
        {
            var parameters = new ParameterReportList
            {
                pageSize = 100,
                reportTypes = new List<ReportTypes>()
            };

            parameters.reportTypes.Add(ReportTypes.GET_FBA_FULFILLMENT_CURRENT_INVENTORY_DATA);
            parameters.marketplaceIds = new List<string>
            {
                marketPlace
            };
            parameters.createdSince = filterDate;

            return amzConnection.Reports.GetReports(parameters);
        }

        public virtual void ImportAmzRecords()
        {
            try
            {
                LUMMWSPreference preference = PXSelect<LUMMWSPreference>.SelectSingleBound(this, null);

                Dictionary<string, string> dicRpt = new Dictionary<string, string>();
                Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();

                string mpID = null;
                foreach (LUMMarketplacePreference mfPref in SelectFrom<LUMMarketplacePreference>.View.Select(this))
                {
                    AmazonConnection amzConnection = GetAmazonConnObject(preference, mfPref.Marketplace, mfPref.Marketplace == "SG", out mpID);

                    if (string.IsNullOrEmpty(mpID)) 
                    {
                        string MarketplaceNull = $"No Marketplace {mfPref.Marketplace} Token Is Defined.";

                        throw new PXException(MarketplaceNull); 
                    }

                    var reports = GetFulfillmentInventoryReports(amzConnection, Filter.Current.FromDate, mpID);

                    reports.RemoveAll(r => r.ReportDocumentId == null);

                    List<string> lines = new List<string>();

                    for (int i = 0; i < reports.Count; i++)
                    {
                        DeleteSameOrEmptyData(reports[i].ReportId);

                        var reportData = amzConnection.Reports.GetReportFile(reports[i].ReportDocumentId);

                        int dataCount = 1;
                        using (StreamReader sr = new StreamReader(reportData))
                        {
                            var data = sr.ReadToEnd().Split('\n').ToArray();

                            while (data.Length > dataCount)
                            {
                                lines = data[dataCount++].Split('\t').ToList();
                                
                                string key = $"{lines[0]}-{lines[2]}-{lines[5]}-{lines[6]}";

                                if (dic.ContainsKey(key) == false)
                                {
                                    dic.Add(key, lines);
                                    dicRpt.Add(key, reports[i].ReportId);
                                }
                            }
                        }
                    }
                }

                var dicList = dic.Values.ToList();
                
                for (int i = 0; i < dicList.Count; i++)
                {
                    dicRpt.TryGetValue(dic.Keys.ToList()[i], out string reportID);

                    CreateAmzINReconciliation(dicList[i], reportID);
                }

                this.Actions.PressSave();

                DeleteSameOrEmptyData(string.Empty);
            }
            catch (Exception e)
            {
                PXProcessing.SetError<LUMAmzINReconcilition>(e.Message);
                throw;
            }
        }

        public virtual void CreateAmzINReconciliation(List<string> list, string reportID)
        {
            string country = list[7].Replace("\r", "");

            //if (string.IsNullOrEmpty(country) ) { return; }

            LUMAmzINReconcilition reconcilition = new LUMAmzINReconcilition()
            {
                SnapshotDate = DateTime.Parse(list[0]),
                FNSku = list[1],
                Sku = list[2],
                ProductName = list[3],
                Qty = Convert.ToDecimal(list[4]),
                FBACenterID = list[5],
                DetailedDesc = list[6],
                CountryID = country,
                Warehouse = INSite.UK.Find(this, $"AMZ{country}00")?.SiteID,
                ReportID = reportID
            };

            //reconcilition.FNSku    = GetStockItemByCrossRefer(list[1], reconcilition.Sku);
            reconcilition.Location = SelectFrom<INLocation>.Where<INLocation.siteID.IsEqual<@P.AsInt>
                                                                  .And<INLocation.locationCD.IsEqual<@P.AsString>>>.View
                                                           .Select(this, reconcilition.Warehouse, list[6].ToUpper() == "SELLABLE" ? "601" : "602").TopFirst?.LocationID;
            Reconcilition.Insert(reconcilition);
        }

        public virtual void CreateInvAdjustment(List<LUMAmzINReconcilition> lists)
        {
            if (lists.Count == 0) 
            {
                const string NoSelectedRec = "Please Tick At Least One Record.";

                throw new PXException(NoSelectedRec);
            }

            INAdjustmentEntry adjustEntry = CreateInstance<INAdjustmentEntry>();

            adjustEntry.CurrentDocument.Insert(new INRegister()
            {
                DocType = INDocType.Adjustment,
                TranDate = lists[0].SnapshotDate,
                TranDesc = "FBA IN Reconciliation"
            });

            var aggrList = lists.GroupBy(g => new { g.Sku, g.Warehouse, g.Location }).Select(v => new
            {
                Sku = v.Key.Sku,
                Warehouse = v.Key.Warehouse,
                Location = v.Key.Location,
                Qty = v.Sum(s => s.Qty)
            }).ToList();

            for (int i = 0; i < aggrList.Count; i++)
            {
                INTran tran = new INTran()
                {
                    InventoryID = InventoryItem.UK.Find(adjustEntry, aggrList[i].Sku).InventoryID,
                    SiteID = aggrList[i].Warehouse,
                    LocationID = aggrList[i].Location
                };

                tran.Qty        = aggrList[i].Qty - (GetINLocationQtyAvail(tran.InventoryID, tran.SiteID, tran.LocationID) ?? 0m);
                tran.ReasonCode = "INRECONCILE";

                adjustEntry.transactions.Insert(tran);
            }

            adjustEntry.Save.Press();
        }

        /// <summary>
        /// 1. search in cross reference, 如果搜出來超過1個stock item -> 報錯
        /// 2. cross reference 只有mapp到一個 stock item -> OK 
        /// 3. cross reference 找不到，則用SKU 去找stock item，找得到 OK, 找不到 報錯這樣
        /// </summary>
        private int? GetStockItemByCrossRefer(string fNSku, int? sku)
        {
            const string MultipleStockItems  = "There Are Multiple Stock Items Found.";
            const string NoCorrespondingItem = "There Is No Corresponding Stock Item In System.";

            var xRefs = SelectFrom<INItemXRef>.Where<INItemXRef.alternateID.IsEqual<@P.AsString>>.View.Select(this, fNSku).ToList();
            
            if (xRefs.Count > 1)
            {
                throw new PXException(MultipleStockItems);
            }
            if (xRefs.Count <= 0 && sku == null)
            {
                throw new PXException(NoCorrespondingItem);
            }

            return xRefs.Count == 1 ? xRefs.FirstOrDefault().Record?.InventoryID : sku;
        }

        private decimal? GetINLocationQtyAvail(int? inventoryID, int? siteID, int? locationID)
        {
            return SelectFrom<INLocationStatus>.Where<INLocationStatus.inventoryID.IsEqual<@P.AsInt>
                                                      .And<INLocationStatus.siteID.IsEqual<@P.AsInt>
                                                           .And<INLocationStatus.locationID.IsEqual<@P.AsInt>>>>.View
                                               .SelectSingleBound(this, null, inventoryID, siteID, locationID).TopFirst?.QtyAvail;
        }

        /// <summary>
        /// Since the standard process must have at least one record, a temporary record is inserted.
        /// </summary>
        private void InsertInitializedData()
        {
            string screenIDWODot = this.Accessinfo.ScreenID.ToString().Replace(".", "");

            PXDatabase.Insert<LUMAmzINReconcilition>(new PXDataFieldAssign<LUMAmzINReconcilition.createdByID>(this.Accessinfo.UserID),
                                                     new PXDataFieldAssign<LUMAmzINReconcilition.createdByScreenID>(screenIDWODot),
                                                     new PXDataFieldAssign<LUMAmzINReconcilition.createdDateTime>(this.Accessinfo.BusinessDate),
                                                     new PXDataFieldAssign<LUMAmzINReconcilition.lastModifiedByID>(this.Accessinfo.UserID),
                                                     new PXDataFieldAssign<LUMAmzINReconcilition.lastModifiedByScreenID>(screenIDWODot),
                                                     new PXDataFieldAssign<LUMAmzINReconcilition.lastModifiedDateTime>(this.Accessinfo.BusinessDate),
                                                     new PXDataFieldAssign<LUMAmzINReconcilition.noteID>(Guid.NewGuid()),
                                                     new PXDataFieldAssign<LUMAmzINReconcilition.isProcesses>(false),
                                                     new PXDataFieldAssign<LUMAmzINReconcilition.reportID>(string.Empty) );
        }

        private void DeleteSameOrEmptyData(string reportID)
        {
            if (reportID == string.Empty)
            {
                PXDatabase.Delete<LUMAmzINReconcilition>(new PXDataFieldRestrict<LUMAmzINReconcilition.reportID>(string.Empty),
                                                         new PXDataFieldRestrict<LUMAmzINReconcilition.isProcesses>(false));
            }

            PXDatabase.Delete<LUMAmzINReconcilition>(new PXDataFieldRestrict<LUMAmzINReconcilition.reportID>(reportID),
                                                     new PXDataFieldRestrict<LUMAmzINReconcilition.isProcesses>(false));
        }
        #endregion
    }
}