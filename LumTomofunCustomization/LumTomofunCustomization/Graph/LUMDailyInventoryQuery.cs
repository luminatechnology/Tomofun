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
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.Graph
{
    public class LUMDailyInventoryQuery : PXGraph<LUMDailyInventoryQuery>
    {
        public PXFilter<DailyInventoryFilter> Filter;
        public SelectFrom<v_GlobalINItemSiteHistDay>
               .Where<v_GlobalINItemSiteHistDay.sDate.IsLessEqual<DailyInventoryFilter.sDate.FromCurrent>>
               .View Transaction;

        public IEnumerable transaction()
        {
            var filter = this.Filter.Current;
            PXView select = new PXView(this, true, Transaction.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;
            List<object> result = select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters, ref startrow, 1000000, ref totalrow);
            PXView.StartRow = 0;
            var vINReconciliationData = SelectFrom<vGlobalINReconciliation>
                                       .Where<vGlobalINReconciliation.iNDate.IsNotNull>
                                       .View.Select(this).RowCast<vGlobalINReconciliation>()
                                       .Where(x => x.INDate?.Date == filter.SDate?.Date)
                                       .GroupBy(x => new { x.SiteCD, x.LocationCD, x.ERPSku, INDate = x.INDate?.Date })
                                       .Select(x => new v_GlobalINItemSiteHistDay()
                                       {
                                           InventoryCD = x.Key.ERPSku,
                                           SiteCD = x.Key.SiteCD,
                                           LocationCD = x.Key.LocationCD,
                                           EndQty = 0,
                                           WarehouseQty = x.Sum(y => y.Qty ?? 0),
                                           SDate = x.Key.INDate
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
                                 InventoryCD = hist?.InventoryCD?.Trim(),
                                 EndQty = hist?.EndQty,
                                 SiteCD = hist?.SiteCD?.Trim(),
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
                                  InventoryCD = rec?.InventoryCD?.Trim(),
                                  EndQty = hist?.EndQty ?? 0,
                                  SiteCD = rec?.SiteCD?.Trim(),
                                  LocationCD = rec?.LocationCD?.Trim(),
                                  WarehouseQty = rec?.WarehouseQty ?? 0,
                                  VarQty = (rec?.WarehouseQty ?? 0) - (hist?.EndQty ?? 0)
                              };
            return leftResult.Union(rightResult.Where(x => x.EndQty == 0));
            //return leftResult.Union(rightResult).Distinct().GroupBy(x => new { x.InventoryCD ,x.EndQty ,x.SiteCD ,x.LocationCD ,x.WarehouseQty ,x.VarQty }).Select(g => g.FirstOrDefault());
        }

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
