using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FikaAmazonAPI;
using FikaAmazonAPI.Utils;
using LUMTomofunCustomization.DAC;
using LumTomofunCustomization.Graph;
using LumTomofunCustomization.API_Entity;
using static FikaAmazonAPI.Utils.Constants;

namespace LUMTomofunCustomization.Graph
{
    public class LUMAmzINReconciliationProc : PXGraph<LUMAmzINReconciliationProc>
    {
        public const string SpcialLabel_NoItem = "*****";

        #region Features
        public PXCancel<SettlementFilter> Cancel;
        public PXFilter<SettlementFilter> Filter;
        public PXFilteredProcessing<LUMAmzINReconcilition, SettlementFilter, Where<LUMAmzINReconcilition.reportID, IsNotNull,
                                                                                   Or<LUMAmzINReconcilition.iNDate, Equal<Current<SettlementFilter.fromDate>>>>,
                                                                             OrderBy<Desc<LUMAmzINReconcilition.iNDate>>> Reconcilition;
        public PXSetup<LUMMWSPreference> Setup;
        #endregion

        #region Ctor
        public LUMAmzINReconciliationProc()
        {
            if (Reconcilition.Select().Count == 0) { InsertInitializedData(); }

            Actions.Move(nameof(Cancel), nameof(massDeletion), true);
            //Actions.Move(nameof(massDeletion), nameof(importFBAIN), true);
            //Actions.Move("ProcessAll", nameof(createAdjustment), true);

            SettlementFilter filter = Filter.Current;

            Reconcilition.SetProcessVisible(false);
            Reconcilition.SetProcessAllCaption("Import FBA IN");
            Reconcilition.SetProcessDelegate(reconciliations => ImportRecords(reconciliations, filter));
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

        //public PXAction<SettlementFilter> importFBAIN;
        //[PXButton(CommitChanges = true), PXUIField(DisplayName = "Import FBA IN", Visible = false)]
        //protected virtual IEnumerable ImportFBAIN(PXAdapter adapter)
        //{
        //    PXLongOperation.StartOperation(this, delegate ()
        //    {
        //        ImportAmzRecords();
        //    });

        //    return adapter.Get();
        //}

        //public PXAction<SettlementFilter> createAdjustment;
        //[PXButton(CommitChanges = true), PXUIField(DisplayName = "Create In Adjustment")]
        //protected virtual IEnumerable CreateAdjustment(PXAdapter adapter)
        //{
        //    PXLongOperation.StartOperation(this, delegate ()
        //    {
        //        CreateInvAdjustment(Reconcilition.Cache.Updated.RowCast<LUMAmzINReconcilition>().ToList());

        //        foreach (LUMAmzINReconcilition row in Reconcilition.Cache.Updated)
        //        {
        //            Reconcilition.Cache.SetValue<LUMAmzINReconcilition.isProcesses>(row, true);
        //            Reconcilition.Update(row);
        //        }

        //        this.Actions.PressSave();
        //    });

        //    return adapter.Get();
        //}
        #endregion

        #region Cache Attached
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIField(DisplayName = "Date")]
        [PXDefault(typeof(AccessInfo.businessDate))]
        protected virtual void _(Events.CacheAttached<SettlementFilter.fromDate> e) { }
        #endregion

        #region Static Methods
        public static void ImportRecords(List<LUMAmzINReconcilition> lists, SettlementFilter filter)
        {
            LUMAmzINReconciliationProc graph = CreateInstance<LUMAmzINReconciliationProc>();

            graph.ImportAmzRecords(filter.FromDate);
            //graph.CreateInvAdjustment(lists);
        }
        #endregion

        #region Methods
        public virtual (string marketPlaceID, string refreshToken) GetAmzCredentialInfo(LUMMWSPreference preference, string marketPlace)
        {
            switch (marketPlace)
            {
                case "US":
                case "CA":
                    return (preference.USMarketplaceID, preference.USRefreshToken);
                case "MX":
                    return (preference.MXMarketplaceID, preference.MXRefreshToken);
                case "AU":
                    return (preference.AUMarketplaceID, preference.AURefreshToken);
                case "JP":
                    return (preference.JPMarketplaceID, preference.JPRefreshToken);
                case "SG":
                    return (preference.SGMarketplaceID, preference.SGRefreshToken);
                default:
                    return (preference.EUMarketplaceID, preference.EURefreshToken);
            }
        }

        public virtual AmazonConnection GetAmazonConnObject(LUMMWSPreference preference, string marketPlace, bool isSingapore, bool isMexico, out string mpID)
        {
            (string marketPlaceID, string refreshToken) = GetAmzCredentialInfo(preference, marketPlace);

            mpID = marketPlaceID;

            return new AmazonConnection(new AmazonCredential()
            {
                AccessKey = isSingapore == false ? preference.AccessKey : preference.SGAccessKey,
                SecretKey = isSingapore == false ? preference.SecretKey : preference.SGSecretKey,
                RoleArn = isSingapore == false ? preference.RoleArn : preference.SGRoleArn,
                ClientId = isSingapore == false ? isMexico == true ? preference.MXClientID : preference.ClientID : preference.SGClientID,
                ClientSecret = isSingapore == false ? isMexico == true ? preference.MXClientSecret : preference.ClientSecret : preference.SGClientSecret,
                RefreshToken = refreshToken,
                MarketPlace = MarketPlace.GetMarketPlaceByID(marketPlaceID),
            });
        }

        // Old Amazon API is expired on 2023/01/31
        //public virtual List<FikaAmazonAPI.AmazonSpApiSDK.Models.Reports.Report> GetFulfillmentInventoryReports(AmazonConnection amzConnection, DateTime? filterDate, string marketPlace)
        //{
        //    var parameters = new ParameterReportList
        //    {
        //        //pageSize = 100, // Roy says it's optional, so it doesn't need to be specified.
        //        reportTypes = new List<ReportTypes>()
        //    };

        //    parameters.reportTypes.Add(ReportTypes.GET_FBA_FULFILLMENT_CURRENT_INVENTORY_DATA);
        //    parameters.marketplaceIds = new List<string>
        //    {
        //        marketPlace
        //    };
        //    parameters.createdSince = filterDate;
        //    // Add the following parameter to make report data a little more compact.
        //    parameters.createdUntil = filterDate.Value.AddDays(1);

        //    return amzConnection.Reports.GetReports(parameters);
        //}

        private async Task<List<LedgerSummaryReportRow>> GetLedgerSummaryReport(AmazonConnection amazonConnection, DateTime fromDate, DateTime toDate, bool mpIsJP)
        {
            List<LedgerSummaryReportRow> list = new List<LedgerSummaryReportRow>();

            var reportOptions = new FikaAmazonAPI.AmazonSpApiSDK.Models.Reports.ReportOptions();

            reportOptions.Add("aggregatedByTimePeriod", "DAILY");

            var path = await amazonConnection.Reports.CreateReportAndDownloadFileAsync(ReportTypes.GET_LEDGER_SUMMARY_VIEW_DATA, fromDate, toDate, reportOptions);
       
            LedgerSummaryReport report = new LedgerSummaryReport(path, amazonConnection.RefNumber, mpIsJP);

            list.AddRange(report.Data);

            return list; 
        }

        public virtual void ImportAmzRecords(DateTime? endDate)
        {
            try
            {
                LUMMWSPreference preference = PXSelect<LUMMWSPreference>.SelectSingleBound(this, null);

                string mpID = null, mP_EU_CA = null;
                foreach (LUMMarketplacePreference mfPref in SelectFrom<LUMMarketplacePreference>.View.Select(this))
                {
                    AmazonConnection amzConnection = GetAmazonConnObject(preference, mfPref.Marketplace, mfPref.Marketplace == "SG", mfPref.Marketplace == "MX", out mpID);

                    if (string.IsNullOrEmpty(mpID))
                    {
                        string MarketplaceNull = $"No Marketplace {mfPref.Marketplace} Token Is Defined.";

                        throw new PXException(MarketplaceNull);
                    }

                    if (mP_EU_CA == mpID)
                    {
                        continue;
                    }
                    else
                    {
                        mP_EU_CA = mpID;

                        var reports = GetLedgerSummaryReport(amzConnection, endDate.Value, endDate.Value, mfPref.Marketplace == "JP").Result;

                        for (int i = 0; i < reports.Count; i++)
                        {
                            DeleteSameOrEmptyData(string.Empty);

                            List<string> strList = new List<string>
                            {
                                reports[i].Date.ToString(),
                                reports[i].FNSKU,
                                reports[i].MSKU,
                                reports[i].Title,
                                reports[i].EndingWarehouseBalance.ToString(),
                                string.Empty,
                                reports[i].Disposition,
                                reports[i].Location
                            };

                            CreateAmzINReconciliation(strList, null);
                        }
                    }
                }

                this.Actions.PressSave();
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

            ///<remarks> Country GB = UK, Warehouse ID = AMZUK (這個較特殊)</remarks>
            if (country == "GB") { country = "UK"; }

            LUMAmzINReconcilition reconcilition = new LUMAmzINReconcilition()
            {
                SnapshotDate = DateTimeOffset.Parse(list[0]).DateTime,
                FNSku = list[1],
                Sku = list[2],
                ProductName = list[3],
                Qty = Convert.ToDecimal(list[4]),
                FBACenterID = list[5],
                DetailedDesc = list[6],
                CountryID = country,
                Warehouse = INSite.UK.Find(this, list[5].Contains("*XFR") ? "FBAINTR" : $"AMZ{country}00")?.SiteID,
                ReportID = reportID
            };

            reconcilition.ERPSku = GetStockItemOrCrossRef(reconcilition.Sku);
            reconcilition.Location = GetLocationIDByWarehouse(reconcilition.Warehouse, list[6].ToUpper());
            // FBA publish IN report after 12:00 am, so the snapshot date actually is one day before.
            // Since Amazon has deployed a new report to present new information, that doesn't need to subtract a day.
            reconcilition.INDate = reconcilition.SnapshotDate.Value/*.AddDays(-1)*/.Date;

            if (Reconcilition.Cache.Inserted.RowCast<LUMAmzINReconcilition>().Where(w => w.INDate == reconcilition.INDate && w.Sku == reconcilition.Sku && w.FBACenterID == reconcilition.FBACenterID &&
                                                                                         w.Warehouse == reconcilition.Warehouse && w.Location == reconcilition.Location).Count() <= 0)
            {
                Reconcilition.Insert(reconcilition);
            }

            DeleteSameOrEmptyData(null, reconcilition.INDate, reconcilition.Sku, reconcilition.Warehouse.Value, reconcilition.Location.Value, reconcilition.FBACenterID);
        }

        /// <summary>
        /// Date = IIF( [snapshot-date] = End of Month , [snapshot-date], SKIP the Record ), 僅針對月底(End of Month) 產生 IN ADJ
        /// </summary>
        public virtual void CreateInvAdjustment(List<LUMAmzINReconcilition> lists)
        {
            if (lists.Count == 0)
            {
                const string NoSelectedRec = "Please Tick At Least One Record.";

                throw new PXException(NoSelectedRec);
            }

            lists.RemoveAll(r => r.INDate != new DateTime(Accessinfo.BusinessDate.Value.Year, Accessinfo.BusinessDate.Value.Month, DateTime.DaysInMonth(Accessinfo.BusinessDate.Value.Year, Accessinfo.BusinessDate.Value.Month)));

            if (lists.Count <= 0) { return; }

            INAdjustmentEntry adjustEntry = CreateInstance<INAdjustmentEntry>();

            adjustEntry.CurrentDocument.Insert(new INRegister()
            {
                DocType = INDocType.Adjustment,
                TranDate = lists[0].SnapshotDate,
                TranDesc = "FBA IN Reconciliation"
            });

            var aggrList = lists.GroupBy(g => new { g.ERPSku, g.Warehouse, g.Location }).Select(v => new
            {
                ERPSku = v.Key.ERPSku,
                Warehouse = v.Key.Warehouse,
                Location = v.Key.Location,
                Qty = v.Sum(s => s.Qty)
            }).ToList();

            for (int i = 0; i < aggrList.Count; i++)
            {
                INTran tran = new INTran()
                {
                    InventoryID = InventoryItem.UK.Find(adjustEntry, aggrList[i].ERPSku)?.InventoryID,
                    SiteID = aggrList[i].Warehouse,
                    LocationID = aggrList[i].Location
                };

                tran.Qty = (aggrList[i].Qty ?? 0m) - (GetINFinYtdQtyAvail(tran.InventoryID, tran.SiteID, tran.LocationID) ?? 0m);
                tran.ReasonCode = "INRECONCILE";

                adjustEntry.transactions.Insert(tran);
            }

            adjustEntry.Save.Press();
        }

        /// <summary>
        /// Search ERP Stock Item & Inventory Cross Reference (Global Type).If Not Found then ERP SKU = ‘*****’
        /// </summary>
        public virtual string GetStockItemOrCrossRef(string sku)
        {
            return InventoryItem.UK.Find(this, sku)?.InventoryCD ??
                   InventoryItem.PK.Find(this, SelectFrom<INItemXRef>.Where<INItemXRef.alternateID.IsEqual<@P.AsString>
                                                                           .And<INItemXRef.alternateType.IsEqual<INAlternateType.global>>>
                                                                     .View.Select(this, sku).TopFirst?.InventoryID)?.InventoryCD ??
                   SpcialLabel_NoItem;
        }

        public virtual int? GetLocationIDByWarehouse(int? warehouse, string locationDescr)
        {
            return SelectFrom<INLocation>.Where<INLocation.siteID.IsEqual<@P.AsInt>.And<INLocation.locationCD.IsEqual<@P.AsString>>>.View
                                         .Select(this, warehouse, locationDescr == "SELLABLE" ? "601" : "602").TopFirst?.LocationID;
        }

        /// <summary>
        /// Quantity = discrepancy qty between Acumatica and FBA [FBA IN Quantity(group by SKU + WH + Location)] – [Acumatica Hist Period End Quantity] where same SKU+WH+Location
        /// </summary>
        private decimal? GetINFinYtdQtyAvail(int? inventoryID, int? siteID, int? locationID)
        {
            return SelectFrom<INItemSiteHist>.Where<INItemSiteHist.inventoryID.IsEqual<@P.AsInt>
                                                    .And<INItemSiteHist.siteID.IsEqual<@P.AsInt>
                                                         .And<INItemSiteHist.locationID.IsEqual<@P.AsInt>>>>.View
                                             .SelectSingleBound(this, null, inventoryID, siteID, locationID).TopFirst?.FinYtdQty;
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
                                                     new PXDataFieldAssign<LUMAmzINReconcilition.reportID>(string.Empty));
        }

        private void DeleteSameOrEmptyData(string reportID, DateTime? iNDate = null, string sku = null, int warehouse = 0, int location = 0, string fBACenterID = null)
        {
            if (!string.IsNullOrEmpty(sku))
            {
                // Delete same records.
                PXDatabase.Delete<LUMAmzINReconcilition>(new PXDataFieldRestrict<LUMAmzINReconcilition.iNDate>(PXDbType.DateTime, 8, iNDate, PXComp.EQ),
                                                         new PXDataFieldRestrict<LUMAmzINReconcilition.sku>(sku),
                                                         new PXDataFieldRestrict<LUMAmzINReconcilition.warehouse>(warehouse),
                                                         new PXDataFieldRestrict<LUMAmzINReconcilition.location>(location),
                                                         new PXDataFieldRestrict<LUMAmzINReconcilition.fBACenterID>(fBACenterID),
                                                         new PXDataFieldRestrict<LUMAmzINReconcilition.isProcesses>(false));
            }

            // Delete initial temporary record.
            PXDatabase.Delete<LUMAmzINReconcilition>(new PXDataFieldRestrict<LUMAmzINReconcilition.reportID>(reportID),
                                                     new PXDataFieldRestrict<LUMAmzINReconcilition.isProcesses>(false));
        }
        #endregion
    }
}