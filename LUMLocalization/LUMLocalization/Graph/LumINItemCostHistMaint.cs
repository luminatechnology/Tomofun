using System;
using System.Collections;
using System.Collections.Generic;
using LUMLocalization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.GL;
using PX.Objects.IN;


namespace LUMLocalization.Graph
{
    public class LumINItemCostHistMaint : PXGraph<LumINItemCostHistMaint>
    {
        public PXFilter<LumINItemCostFilter> MasterViewFilter;

        public SelectFrom<LumINItemCostHist>.Where<LumINItemCostHist.conditionPeriod.IsEqual<LumINItemCostFilter.finPeriodID.FromCurrent>>.View DetailsView;

        #region Base Table Function Control
        public LumINItemCostHistMaint()
        {
            DetailsView.AllowInsert = DetailsView.AllowUpdate = DetailsView.AllowDelete = false;
        }
        #endregion

        #region LumINItemCost Filter
        [Serializable]
        [PXCacheName("Lum INItemCostHist Filter")]
        public class LumINItemCostFilter : IBqlTable
        {
            #region FinPeriodID
            [PXDBString(6, InputMask = "")]
            [FinPeriodID()]
            [PXUIField(DisplayName = "Period ID", Visibility = PXUIVisibility.SelectorVisible)]
            [PXSelector(typeof(Search4<INItemCostHist.finPeriodID, Where<INItemCostHist.finPeriodID.IsEqual<INItemCostHist.finPeriodID>>, Aggregate<GroupBy<INItemCostHist.finPeriodID>>, OrderBy<Desc<INItemCostHist.finPeriodID>>>),
                        typeof(INItemCostHist.finPeriodID))]
            public virtual string FinPeriodID { get; set; }
            public abstract class finPeriodID : PX.Data.BQL.BqlString.Field<finPeriodID> { }
            #endregion
        }
        #endregion

        #region Delegate DataView
        public IEnumerable detailsView()
        {
            List<object> result = new List<object>();

            var currentSearchPeriod = ((LumINItemCostFilter)this.Caches[typeof(LumINItemCostFilter)].Current)?.FinPeriodID;

            if (currentSearchPeriod != null)
            {
                var pars = new List<PXSPParameter>();
                PXSPParameter period = new PXSPInParameter("@period", PXDbType.Int, currentSearchPeriod);
                PXSPParameter companyID = new PXSPInParameter("@companyID", PXDbType.Int, PX.Data.Update.PXInstanceHelper.CurrentCompany);
                pars.Add(period);
                pars.Add(companyID);

                using (new PXConnectionScope())
                {
                    using (PXTransactionScope ts = new PXTransactionScope())
                    {
                        PXDatabase.Execute("SP_GenerateLumINItemCostHist", pars.ToArray());
                        ts.Complete();
                    }
                }

                PXView select = new PXView(this, true, DetailsView.View.BqlSelect);
                int totalrow = 0;
                int startrow = PXView.StartRow;
                result = select.Select(PXView.Currents, PXView.Parameters, PXView.Searches, PXView.SortColumns, PXView.Descendings, PXView.Filters, ref startrow, PXView.MaximumRows, ref totalrow);
                PXView.StartRow = 0;
                return result;

            }

            return result;
        }
        #endregion
    }
}