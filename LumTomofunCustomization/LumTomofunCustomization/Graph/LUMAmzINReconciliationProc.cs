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
        public Dictionary<string, string> Markerplaces = new Dictionary<string, string>()
        {
            { "US", "ATVPDKIKX0DER" },
            { "CA", "ATVPDKIKX0DER" },
            { "MX", "ATVPDKIKX0DER" },
            { "AU", "A39IBJ37TRP1C6" },      
            { "UK", "A13V1IB3VIYZZH" },
            { "FR", "A13V1IB3VIYZZH" },
            { "DE", "A13V1IB3VIYZZH" },
            { "IT", "A13V1IB3VIYZZH" },
            { "ES", "A13V1IB3VIYZZH" },
            { "NL", "A13V1IB3VIYZZH" },
            { "SE", "A13V1IB3VIYZZH" },
            { "JP", "A1VC38T7YXB528" },
            { "SG", "A19VAU5U5O7RUS" } 
        };

        public Dictionary<string, string> RefreshTokens = new Dictionary<string, string>()
        {
            // 美洲都用這個
            { "ATVPDKIKX0DER" ,"Atzr|IwEBIIp0ZezYucHWyk0OLYR0bgWdNSgw7zbyhB1_zwKpEoc9VBK4RRe2mXr3VQlnv7nO8237Q5fwIhiCxdUkdtKePVcNsxglJobyJa5Fo7KbebCHis5PF2rvcQd6rMfEbVFkD2R2hI4CW-_8dGFcvtriScAdhMcUzDh3jh6UZ_QOLP8X9_af2qzfkwrUHQfxNECLjWl5QjL6jOm8_7Mthi2yOFf0LKPtIcSD87Z6McQWAia0zETu4pBxwVZv_783BULKT42JbGO9KnLbCtGPVvMXSmHy-mD8GIYmlgsRs9cpM8ch3R410E1LB2kzqVtm_TdMMXM" },
            // EU都用這個
            { "A13V1IB3VIYZZH", "Atzr|IwEBIDT3Gt6rpMY0XFBy5Y8yqVYTT5l-EYa7xagDwlDYf3LPCsqKy80PaTMDsSZwpp6Tq6tK9BSSPBjS9ca8xfhR_KPiV528-WeifurxrHvG2rF0VDWto1yMZ0VVrB_4GKrM0jsJ-CBzgPqG8ukeXb1iLsF6X9StcAsxa6ZS7R6zxQy2fNaZ8lmitVMQ49Lzyl0Oj2glMH8QbPy5cDhzbrPt80VV1KPymSB-9EPvfl7FKmYZ6H9lYbIAJPAQ1u9PMmlS5mGBJOB-XipoTogvW4i1IzLrJz-2kPE9K2ukVB1oZUIpGe3287ubwQq95yhFYE9-oGnGqZO0dMa7HMS-O19VADGz" },
            // MarketPlace.Japan
            { "A1VC38T7YXB528", "Atzr|IwEBIKaD4GZIyIrsPeq_eGShNISwCUWrph77-5wFIMR-8aZmv47EMDgJhR4as4WU6j4qymsdX2q8HU5aiW1SOL1XcxzUIYJrlv2w3ei2pDqsMEcYmGiiq09JoN6AwVYSXeOSHIWZ4WS23FzP6hiPVtxjmRiXmsS9POOzzM5KxF5hhCBnkRD6Kx_te_ycO0yhsHu8tMH-qjBBgDkdg_id17E-B4snDU3AFg5oZhaFzr_BsUaI6riDb2wMIu3koT4oHk-8YgdXMRe2TTRGu82n63BjT4LINXsVHB70tg4OYsM1fvzVJWjdOPgNCbrU46GwOcM2ALU" },
            // MarketPlace.Australia
            { "A39IBJ37TRP1C6", "Atzr|IwEBIMYdx7q_sBo_a1jlsshAUd6cBnCEIClRzQVaNZS5y7tE7nGV6qjp2a0RgYHKduOhee-Xl54q-gi2S17S5om7GwGj6vCc44enCQP6FK0ZN5DjXbjd-_cq0vMQwqP5e1HSRDJ6Oc8y32QRN7_QPow_PCm6hglLUsLvg4KW7OXXowRswkNBYMUA-3KnGCTpotrbV0u2IhOZnl0bP3vxmZ8nul3iw9htYhhhDw-Xb-FMmTfPQGkC3TPbDeyC6tDej3GANGuiWfRxqZTJBo3RDR2u0RDu0-WtFf3RqHQLUza38gmtMJt2Wj_PtKC83ugS6kFfwVA" },
            // MarketPlace.Singapore
            { "A19VAU5U5O7RUS", "Atzr|IwEBIA7GJ4sFj4Qt8snK0uEJRmzx1fEnUtsGTFmERMR1aIeD_lVf2kH2sOIfDZ3X-HUwm0c9q5DP0VESE59wzn-h2o28m5Uds-LhvBN9GDk-CJS_T92THt89qXOv-S01HlalsoYDpX5yvNROnGpHXgYAIPuXZCRobHCZWndo2PAf-2ZlC1KX3KLymsmDG2_fvB9wgL0fzxHbapgv-Lly5R2N79wz2Bfc0X59pfcJ3zRTbgStT_VHiIAXOOr__RXdoATJZ40-oCSKdUaX8XFbg9DohLMMQ2ttsM-akf8q0Hy3CxOTQA83c3XgKlUOp2aofmF9Nz8" }
        };

        public virtual AmazonConnection GetAmazonConnObject(LUMMWSPreference preference, string marketPlace, bool IsSingapore)
        {
            RefreshTokens.TryGetValue(marketPlace, out string refeshToken);

            return new AmazonConnection(new AmazonCredential()
            {
                AccessKey    = IsSingapore == false ? preference.AccessKey    : preference.SGAccessKey,
                SecretKey    = IsSingapore == false ? preference.SecretKey    : preference.SGSecretKey,
                RoleArn      = IsSingapore == false ? preference.RoleArn      : preference.SGRoleArn,
                ClientId     = IsSingapore == false ? preference.ClientID     : preference.SGClientID,
                ClientSecret = IsSingapore == false ? preference.ClientSecret : preference.SGClientSecret,
                RefreshToken = refeshToken,
                MarketPlace  = MarketPlace.GetMarketPlaceByID(marketPlace),
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
                
                string dicValue = null;
                foreach (LUMMarketplacePreference mfPref in SelectFrom<LUMMarketplacePreference>.View.Select(this))
                {
                    Markerplaces.TryGetValue(mfPref.Marketplace, out dicValue);

                    AmazonConnection amzConnection = GetAmazonConnObject(preference, dicValue, mfPref.Marketplace == "SG");

                    var reports = GetFulfillmentInventoryReports(amzConnection, Filter.Current.FromDate, dicValue);

                    reports.RemoveAll(r => r.ReportDocumentId == null);

                    List<string> lines = new List<string>();

                    for (int i = 0; i < reports.Count; i++)
                    {
                        var reportData = amzConnection.Reports.GetReportFile(reports[i].ReportDocumentId);

                        DeleteSameOrEmptyData(reports[i].ReportId);

                        int dataCount = 1;
                        using (StreamReader sr = new StreamReader(reportData))
                        {
                            var data = sr.ReadToEnd().Split('\n').ToArray();

                            while (data.Length > dataCount)
                            {
                                lines = data[dataCount].Split('\t').ToList();

                                CreateAmzINReconciliation(lines, reports[i].ReportId);

                                dataCount++;
                            }
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

            if (string.IsNullOrEmpty(country) ) { return; }

            LUMAmzINReconcilition reconcilition = new LUMAmzINReconcilition()
            {
                SnapshotDate = DateTime.Parse(list[0]),
                Sku = InventoryItem.UK.Find(this, list[2])?.InventoryID,
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
                    InventoryID = aggrList[i].Sku,
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
            PXDatabase.Delete<LUMAmzINReconcilition>(new PXDataFieldRestrict<LUMAmzINReconcilition.reportID>(string.Empty),
                                                     new PXDataFieldRestrict<LUMAmzINReconcilition.isProcesses>(false));

            PXDatabase.Delete<LUMAmzINReconcilition>(new PXDataFieldRestrict<LUMAmzINReconcilition.reportID>(reportID),
                                                     new PXDataFieldRestrict<LUMAmzINReconcilition.isProcesses>(false));
        }
        #endregion
    }
}