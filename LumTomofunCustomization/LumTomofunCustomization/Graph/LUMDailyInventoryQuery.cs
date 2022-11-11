using LUMLocalization.DAC;
using LUMTomofunCustomization.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LumTomofunCustomization.Graph
{
    public class LUMDailyInventoryQuery : PXGraph<LUMDailyInventoryQuery>
    {
        public const string NotFoundInCurTenant = "{0} [{1}] Cannot Be Found In {2} Tenant.";

        /// <remarks> This variable is only used to call delegate data view in CreateGlobalINAdjustments() to retrieve screen redenering results. </remarks>
        public bool Recall = false;

        #region Selects & Filter
        public PXFilter<DailyInventoryFilter> Filter;
        public SelectFrom<v_GlobalINItemSiteHistDay>.Where<v_GlobalINItemSiteHistDay.sDate.IsLessEqual<DailyInventoryFilter.sDate.FromCurrent>>.View Transaction;
        public SelectFrom<vGlobalINReconciliation>.Where<vGlobalINReconciliation.iNDate.IsLessEqual<DailyInventoryFilter.sDate.FromCurrent>>
                                                  .AggregateTo<GroupBy<vGlobalINReconciliation.siteCD,
                                                                       GroupBy<vGlobalINReconciliation.locationCD,
                                                                               GroupBy<vGlobalINReconciliation.inventoryCD,
                                                                                       GroupBy<vGlobalINReconciliation.companyCD,
                                                                                               GroupBy<vGlobalINReconciliation.iNDate,
                                                                                                       Sum<vGlobalINReconciliation.qty>>>>>>>.View Transaction2;
        #endregion

        #region Delegate Data View
        public IEnumerable transaction()
        {
            var filter = this.Filter.Current;

            int totalrow = 0, startrow = PXView.StartRow;
            List<object> result = new PXView(this, true, Transaction.View.BqlSelect).Select(PXView.Currents, PXView.Parameters,
                                                                                            PXView.Searches, PXView.SortColumns, PXView.Descendings,
                                                                                            //PXView.Filters,
                                                                                            Transaction.View.GetExternalFilters(),
                                                                                            ref startrow, 1000000, ref totalrow);
            if (Recall == false)
            {
                PXView.StartRow = 0;
            }

            var histData = new List<v_GlobalINItemSiteHistDay>();
            foreach (var inventoryGroup in result.GroupBy(x => new { ((v_GlobalINItemSiteHistDay)x).InventoryID, ((v_GlobalINItemSiteHistDay)x).Siteid, ((v_GlobalINItemSiteHistDay)x).LocationID }))
            {
                // Calculate VarQty
                v_GlobalINItemSiteHistDay currentRow = inventoryGroup.OrderByDescending(x => ((v_GlobalINItemSiteHistDay)x).SDate).FirstOrDefault() as v_GlobalINItemSiteHistDay;
                histData.Add(currentRow);
            }

            var selects = new PXView(this, true, Transaction2.View.BqlSelect).Select(PXView.Currents, PXView.Parameters,
                                                                                     PXView.Searches, PXView.SortColumns, PXView.Descendings,
                                                                                     Transaction.View.GetExternalFilters(), ref startrow, 1000000, ref totalrow);

            var vINReconciliationData = new List<vGlobalINReconciliation>();
            foreach (vGlobalINReconciliation row in selects.Where(w => (w as vGlobalINReconciliation).INDate == filter.SDate) )
            {
                vINReconciliationData.Add(row);
            }

            #region Generate Data From v_GlobalINItemSiteHistDay
            var leftResult = from hist in histData
                             join rec in vINReconciliationData on new { A = hist.SiteCD?.Trim(), B = hist?.LocationCD?.Trim(), C = hist?.InventoryCD?.Trim(), D = hist?.SDate?.Date } equals
                                                                  new { A = rec.SiteCD?.Trim(), B = rec?.LocationCD?.Trim(), C = rec?.InventoryCD?.Trim(), D = rec?.INDate?.Date } into temp
                             from rec in temp.DefaultIfEmpty()
                             select new v_GlobalINItemSiteHistDay()
                             {
                                 CompanyCD = hist?.CompanyCD,
                                 InventoryID = hist?.InventoryID,
                                 InventoryCD = hist?.InventoryCD?.Trim(),
                                 EndQty = hist?.EndQty,
                                 Siteid = hist?.Siteid,
                                 SiteCD = hist?.SiteCD?.Trim(),
                                 LocationID = hist?.LocationID,
                                 LocationCD = hist?.LocationCD?.Trim(),
                                 WarehouseQty = rec?.Qty ?? 0,
                                 InventoryITemDescr = hist?.InventoryITemDescr,
                                 VarQty = (rec?.Qty ?? 0) - (hist?.EndQty ?? 0)
                             };
            #endregion

            #region Generate Data From vGlobalINReconciliation
            var rightResult = from rec in vINReconciliationData
                              join hist in histData on new { A = rec.SiteCD?.Trim(), B = rec?.LocationCD?.Trim(), C = rec?.InventoryCD?.Trim(), D = rec?.INDate?.Date } equals
                                                       new { A = hist.SiteCD?.Trim(), B = hist?.LocationCD?.Trim(), C = hist?.InventoryCD?.Trim(), D = hist?.SDate?.Date } into temp
                              from hist in temp.DefaultIfEmpty()
                              select new v_GlobalINItemSiteHistDay()
                              {
                                  CompanyCD = rec?.CompanyCD,
                                  InventoryID = rec?.InventoryID,
                                  InventoryCD = rec?.InventoryCD?.Trim(),
                                  EndQty = hist?.EndQty ?? 0,
                                  Siteid = rec?.SiteID,
                                  SiteCD = rec?.SiteCD?.Trim(),
                                  LocationID = rec?.LocationID,
                                  LocationCD = rec?.LocationCD?.Trim(),
                                  WarehouseQty = rec?.Qty ?? 0,
                                  VarQty = (rec?.Qty ?? 0) - (hist?.EndQty ?? 0)
                              };
            #endregion

            return leftResult.Union(rightResult.Where(x => x.EndQty == 0));
            //return leftResult.Union(rightResult).Distinct().GroupBy(x => new { x.InventoryCD ,x.EndQty ,x.SiteCD ,x.LocationCD ,x.WarehouseQty ,x.VarQty }).Select(g => g.FirstOrDefault());
        }
        #endregion

        #region Actions
        public PXAction<DailyInventoryFilter> createINAdjust;
        [PXProcessButton(CommitChanges = true), PXUIField(DisplayName = "Create IN Adjustment", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable CreateINAdjust(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                CreateGlobalINAdjustments(Filter.Current.SDate);
            });

            return adapter.Get();
        }
        #endregion

        #region Methods
        protected virtual void CreateGlobalINAdjustments(DateTime? filterDate)
        {
            Recall = true;
            IEnumerable<v_GlobalINItemSiteHistDay> result = (IEnumerable<v_GlobalINItemSiteHistDay>)transaction();

            foreach (PX.SM.UPCompany row in PX.Data.Update.PXCompanyHelper.SelectCompanies() )
            {
                var resKey = result.Where(w => w.CompanyCD == row.CompanyCD).ToList();

                if (resKey.Count > 0)
                {
                    using (new PXLoginScope($"{Accessinfo.UserName}@{row.CompanyCD}"))
                    {
                        INAdjustmentEntry adjustEntry = CreateInstance<INAdjustmentEntry>();

                        adjustEntry.CurrentDocument.Insert(new INRegister()
                        {
                            DocType  = INDocType.Adjustment,
                            TranDate = filterDate,
                            TranDesc = "IN Reconciliation"
                        });

                        for (int i = 0; i < resKey.Count; i++)
                        {
                            INTran tran = new INTran();

                            tran.InventoryID = InventoryItem.UK.Find(adjustEntry, resKey[i].InventoryCD)?.InventoryID;
                            if (tran.InventoryID == null) 
                            {
                                throw new PXException(string.Format(NotFoundInCurTenant, PXUIFieldAttribute.GetDisplayName<INTran.inventoryID>(adjustEntry.transactions.Cache), resKey[i].InventoryCD, row.CompanyCD));
                            }
                            tran.SiteID      = INSite.UK.Find(adjustEntry, resKey[i].SiteCD).SiteID;
                            if (tran.SiteID == null) 
                            {
                                throw new PXException(string.Format(NotFoundInCurTenant, PXUIFieldAttribute.GetDisplayName<INTran.siteID>(adjustEntry.transactions.Cache), resKey[i].SiteCD, row.CompanyCD));
                            }
                            tran.LocationID  = SelectFrom<INLocation>.Where<INLocation.locationCD.IsEqual<@P.AsString>
                                                                            .And<INLocation.siteID.IsEqual<@P.AsInt>>>.View.SelectSingleBound(adjustEntry, null, resKey[i].LocationCD, tran.SiteID).TopFirst?.LocationID;
                            if (tran.LocationID == null) 
                            {
                                throw new PXException(string.Format(NotFoundInCurTenant, PXUIFieldAttribute.GetDisplayName<INTran.locationID>(adjustEntry.transactions.Cache), resKey[i].LocationCD, row.CompanyCD));
                            }
                            tran.Qty         = resKey[i].VarQty;
                            tran.ReasonCode  = "INRECONCILE";

                            adjustEntry.transactions.Insert(tran);
                        }

                        adjustEntry.Save.Press();
                    }
                }
            }   
        }
        #endregion

        #region Unbound DAC
        public class DailyInventoryFilter : IBqlTable
        {
            [PXDBDate]
            [PXDefault(typeof(AccessInfo.businessDate))]
            [PXUIField(DisplayName = "Date")]
            public virtual DateTime? SDate { get; set; }
            public abstract class sDate : PX.Data.BQL.BqlDateTime.Field<sDate> { }
        }
        #endregion
    }
}
