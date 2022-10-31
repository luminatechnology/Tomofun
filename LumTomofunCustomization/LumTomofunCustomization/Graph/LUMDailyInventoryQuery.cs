﻿using LUMLocalization.DAC;
using LUMTomofunCustomization.DAC;
using PX.Data;
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
        public PXFilter<DailyInventoryFilter> Filter;
        public SelectFrom<v_GlobalINItemSiteHistDay>
               .Where<v_GlobalINItemSiteHistDay.sDate.IsLessEqual<DailyInventoryFilter.sDate.FromCurrent>>
               .View Transaction;

        #region Delegate Data View
        public IEnumerable transaction()
        {
            var filter = this.Filter.Current;

            PXView select = new PXView(this, true, Transaction.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;
            List<object> result = select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters,
                   ref startrow, 1000000, ref totalrow);
            PXView.StartRow = 0;
            var vINReconciliationData = SelectFrom<vGlobalINReconciliation>
                                       .Where<vGlobalINReconciliation.iNDate.IsNotNull>
                                       .View.Select(this).RowCast<vGlobalINReconciliation>()
                                       .Where(x => x.INDate?.Date == filter.SDate?.Date)
                                       .GroupBy(x => new { x.SiteCD, x.LocationCD, x.ERPSku, INDate = x.INDate?.Date, CompanyCD = x.CompanyCD })
                                       .Select(x => new v_GlobalINItemSiteHistDay()
                                       {
                                           InventoryCD = x.Key.ERPSku,
                                           SiteCD = x.Key.SiteCD,
                                           LocationCD = x.Key.LocationCD,
                                           EndQty = 0,
                                           WarehouseQty = x.Sum(y => y.Qty ?? 0),
                                           SDate = x.Key.INDate,
                                           CompanyCD = x.Key.CompanyCD
                                       });
            var histData = new List<v_GlobalINItemSiteHistDay>();
            foreach (var inventoryGroup in result.GroupBy(x => new { ((v_GlobalINItemSiteHistDay)x).InventoryID, ((v_GlobalINItemSiteHistDay)x).Siteid, ((v_GlobalINItemSiteHistDay)x).LocationID }))
            {
                // Calculate VarQty
                v_GlobalINItemSiteHistDay currentRow = inventoryGroup.OrderByDescending(x => ((v_GlobalINItemSiteHistDay)x).SDate).FirstOrDefault() as v_GlobalINItemSiteHistDay;
                histData.Add(currentRow);
            }

            var leftResult = from hist in histData
                             join rec in vINReconciliationData on new { A = hist.SiteCD?.Trim(), B = hist?.LocationCD?.Trim(), C = hist?.InventoryCD?.Trim(), D = hist?.SDate?.Date } equals
                                                                  new { A = rec.SiteCD?.Trim(), B = rec?.LocationCD?.Trim(), C = rec?.InventoryCD?.Trim(), D = rec?.SDate?.Date } into temp
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
                                 WarehouseQty = rec?.WarehouseQty ?? 0,
                                 InventoryITemDescr = hist?.InventoryITemDescr,
                                 VarQty = (rec?.WarehouseQty ?? 0) - (hist?.EndQty ?? 0)
                             };
            var rightResult = from rec in vINReconciliationData
                              join hist in histData on new { A = rec.SiteCD?.Trim(), B = rec?.LocationCD?.Trim(), C = rec?.InventoryCD?.Trim(), D = rec?.SDate?.Date } equals
                                                       new { A = hist.SiteCD?.Trim(), B = hist?.LocationCD?.Trim(), C = hist?.InventoryCD?.Trim(), D = hist?.SDate?.Date } into temp
                              from hist in temp.DefaultIfEmpty()
                              select new v_GlobalINItemSiteHistDay()
                              {
                                  CompanyCD = rec?.CompanyCD,
                                  InventoryID = rec?.InventoryID,
                                  InventoryCD = rec?.InventoryCD?.Trim(),
                                  EndQty = hist?.EndQty ?? 0,
                                  Siteid = rec?.Siteid,
                                  SiteCD = rec?.SiteCD?.Trim(),
                                  LocationID = rec?.LocationID,
                                  LocationCD = rec?.LocationCD?.Trim(),
                                  WarehouseQty = rec?.WarehouseQty ?? 0,
                                  VarQty = (rec?.WarehouseQty ?? 0) - (hist?.EndQty ?? 0)
                              };

            ///<remarks> Makes the cache must have records that calculated unbound fields.</remarks>
            foreach (v_GlobalINItemSiteHistDay row in leftResult.Union(rightResult.Where(x => x.EndQty == 0)))
            {
                Transaction.Cache.Insert(row);

                yield return row;
            }
            //return leftResult.Union(rightResult.Where(x => x.EndQty == 0));
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
            PXView select = new PXView(this, true, Transaction.View.BqlSelect);

            int totalrow = 0, startrow = PXView.StartRow;

            foreach (PX.SM.UPCompany row in PX.Data.Update.PXCompanyHelper.SelectCompanies() )
            {
                // Get no manual filtering of cached records on the grid.
                List<v_GlobalINItemSiteHistDay> curCompanyData = Transaction.Cache.Cached.RowCast<v_GlobalINItemSiteHistDay>().Where(w => w.VarQty != 0m && w.CompanyCD == row.CompanyCD).ToList();

                // Get manual filtering of cache records and re-aggregate and filter.
                var result = select.Select(PXView.Currents, PXView.Parameters, PXView.Searches, PXView.SortColumns, PXView.Descendings,
                                           Transaction.View.GetExternalFilters(), ref startrow, PXView.MaximumRows, ref totalrow)
                                   .OrderByDescending(o => (o as v_GlobalINItemSiteHistDay).SDate)
                                   .GroupBy(g => new
                                                 {
                                                     (g as v_GlobalINItemSiteHistDay).CompanyCD,
                                                     (g as v_GlobalINItemSiteHistDay).InventoryID,
                                                     (g as v_GlobalINItemSiteHistDay).Siteid,
                                                     (g as v_GlobalINItemSiteHistDay).LocationID
                                                 })
                                   .Where(w => w.Key.CompanyCD == row.CompanyCD).ToList();

                if (result.Count > 0)
                {
                    using (new PXLoginScope($"{Accessinfo.UserName}@{row.CompanyCD}"))
                    {
                        INAdjustmentEntry adjustEntry = CreateInstance<INAdjustmentEntry>();

                        adjustEntry.CurrentDocument.Insert(new INRegister()
                        {
                            DocType = INDocType.Adjustment,
                            TranDate = filterDate,
                            TranDesc = "IN Reconciliation"
                        });

                        var resKey = result.ToList();

                        for (int i = 0; i < result.Count; i++)
                        {
                            INTran tran = new INTran()
                            {
                                InventoryID = resKey[i].Key.InventoryID,
                                SiteID      = resKey[i].Key.Siteid,
                                LocationID  = resKey[i].Key.LocationID
                            };

                            tran.Qty = curCompanyData.Find(f => f.InventoryID == tran.InventoryID && f.Siteid == tran.SiteID && f.LocationID == tran.LocationID)?.VarQty;
                            //tran.ReasonCode = "INRECONCILE";

                            adjustEntry.transactions.Insert(tran);
                        }

                        adjustEntry.Save.Press();
                    }
                }
            }   
        }
        #endregion

        public class DailyInventoryFilter : IBqlTable
        {
            [PXDBDate]
            [PXDefault(typeof(AccessInfo.businessDate))]
            [PXUIField(DisplayName = "Date")]
            public virtual DateTime? SDate { get; set; }
            public abstract class sDate : PX.Data.BQL.BqlDateTime.Field<sDate> { }
        }
    }
}
