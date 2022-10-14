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
               .Where<v_GlobalINItemSiteHistDay.sDate.IsLessEqual<DailyInventoryFilter.sDate.FromCurrent>
                    .And<v_GlobalINItemSiteHistDay.inventoryID.IsEqual<DailyInventoryFilter.inventoryID.FromCurrent>.Or<DailyInventoryFilter.inventoryID.FromCurrent.IsNull>>>
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
                                       .View.Select(this).RowCast<vGlobalINReconciliation>();
            foreach (var inventoryGroup in result.GroupBy(x => new { ((v_GlobalINItemSiteHistDay)x).InventoryID, ((v_GlobalINItemSiteHistDay)x).Siteid, ((v_GlobalINItemSiteHistDay)x).LocationID }))
            {
                // Calculate VarQty
                v_GlobalINItemSiteHistDay currentRow = inventoryGroup.OrderByDescending(x => ((v_GlobalINItemSiteHistDay)x).SDate).FirstOrDefault() as v_GlobalINItemSiteHistDay;
                if (currentRow != null)
                {
                    var mappingRow = vINReconciliationData.Where(x => x.SiteCD?.Trim() == currentRow?.SiteCD?.Trim() && 
                                                                      x.LocationCD?.Trim() == currentRow?.LocationCD?.Trim() && 
                                                                      x.ERPSku?.Trim() == currentRow?.InventoryCD?.Trim() && 
                                                                      x.INDate?.Date == filter?.SDate?.Date);
                    currentRow.WarehouseQty = mappingRow.Sum(x => x.Qty ?? 0);
                    currentRow.VarQty = currentRow.WarehouseQty - (currentRow?.EndQty ?? 0);
                }
                yield return currentRow;
            }
        }

        public class DailyInventoryFilter : IBqlTable
        {
            [StockItem()]
            [PXDefault()]
            [PXUIField(DisplayName = "InventoryID")]
            public virtual int? InventoryID { get; set; }
            public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }

            [PXDBDate]
            [PXDefault(typeof(AccessInfo.businessDate))]
            [PXUIField(DisplayName = "Date")]
            public virtual DateTime? SDate { get; set; }
            public abstract class sDate : PX.Data.BQL.BqlDateTime.Field<sDate> { }
        }
    }
}
