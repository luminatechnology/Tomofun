using System;
using PX.Data;
using PX.Objects.GL;
using PX.Objects.IN;

namespace LUMLocalization.DAC
{
    [Serializable]
    [PXCacheName("LCMValuation")]
    public class LCMValuation : IBqlTable
    {
        #region Id
        [PXDBIdentity(IsKey = true)]
        public virtual int? Id { get; set; }
        public abstract class id : PX.Data.BQL.BqlInt.Field<id> { }
        #endregion

        #region CompanyID
        [PXDBInt]
        public virtual int? CompanyID { get; set; }
        public abstract class companyID : PX.Data.BQL.BqlInt.Field<companyID> { }
        #endregion

        #region InventoryID
        [Inventory(Filterable = true, DirtyRead = true, Enabled = false, DisplayName = "Inventory ID")]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region FinPeriodID
        [PXDBString(6, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Fin Period ID", Visible = false)]
        public virtual string FinPeriodID { get; set; }
        public abstract class finPeriodID : PX.Data.BQL.BqlString.Field<finPeriodID> { }
        #endregion

        #region ConditionPeriod
        [PXDBString(6, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Condition Period", Visible = false)]
        public virtual string ConditionPeriod { get; set; }
        public abstract class conditionPeriod : PX.Data.BQL.BqlString.Field<conditionPeriod> { }
        #endregion

        #region FinYtdCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Fin Ytd Cost")]
        public virtual Decimal? FinYtdCost { get; set; }
        public abstract class finYtdCost : PX.Data.BQL.BqlDecimal.Field<finYtdCost> { }
        #endregion

        #region FinYtdQty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Fin Ytd Qty")]
        public virtual Decimal? FinYtdQty { get; set; }
        public abstract class finYtdQty : PX.Data.BQL.BqlDecimal.Field<finYtdQty> { }
        #endregion

        #region UnitCost
        [PXDBDecimal(4)]
        [PXUIField(DisplayName = "Unit Cost")]
        public virtual Decimal? UnitCost { get; set; }
        public abstract class unitCost : PX.Data.BQL.BqlDecimal.Field<unitCost> { }
        #endregion

        #region LastSalesPrice
        [PXDBDecimal(4)]
        [PXUIField(DisplayName = "Last Sales Price")]
        public virtual Decimal? LastSalesPrice { get; set; }
        public abstract class lastSalesPrice : PX.Data.BQL.BqlDecimal.Field<lastSalesPrice> { }
        #endregion

        #region LastSalesDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Last Sales Date")]
        public virtual DateTime? LastSalesDate { get; set; }
        public abstract class lastSalesDate : PX.Data.BQL.BqlDateTime.Field<lastSalesDate> { }
        #endregion

        #region IsValuationLoss
        [PXDBInt()]
        [PXUIField(DisplayName = "Is Valuation Loss")]
        public virtual int? IsValuationLoss { get; set; }
        public abstract class isValuationLoss : PX.Data.BQL.BqlInt.Field<isValuationLoss> { }
        #endregion

        #region ValuationLoss
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Valuation Loss")]
        public virtual Decimal? ValuationLoss { get; set; }
        public abstract class valuationLoss : PX.Data.BQL.BqlDecimal.Field<valuationLoss> { }
        #endregion

        #region LastReceiptPrice
        [PXDBDecimal(4)]
        [PXUIField(DisplayName = "Last Receipt Price")]
        public virtual Decimal? LastReceiptPrice { get; set; }
        public abstract class lastReceiptPrice : PX.Data.BQL.BqlDecimal.Field<lastReceiptPrice> { }
        #endregion

        #region LastReceiptDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Last Receipt Date")]
        public virtual DateTime? LastReceiptDate { get; set; }
        public abstract class lastReceiptDate : PX.Data.BQL.BqlDateTime.Field<lastReceiptDate> { }
        #endregion
    }
}