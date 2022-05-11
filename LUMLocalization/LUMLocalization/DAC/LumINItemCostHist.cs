using PX.Data;
using PX.Objects.IN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUMLocalization.DAC
{
    [Serializable]
    [PXCacheName("LumINItemCostHist")]
    public class LumINItemCostHist : IBqlTable
    {
        #region Id
        [PXDBIdentity]
        public virtual int? Id { get; set; }
        public abstract class id : PX.Data.BQL.BqlInt.Field<id> { }
        #endregion

        #region InventoryID
        [Inventory(Filterable = true, DirtyRead = true, Enabled = false, DisplayName = "Inventory ID")]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region InventoryCD
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Inventory CD")]
        public virtual string InventoryCD { get; set; }
        public abstract class inventoryCD : PX.Data.BQL.BqlString.Field<inventoryCD> { }
        #endregion

        #region ItemDescr
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Item Description")]
        public virtual string ItemDescr { get; set; }
        public abstract class itemDescr : PX.Data.BQL.BqlString.Field<itemDescr> { }
        #endregion

        #region ItemClassID
        [PXDBInt()]
        [PXUIField(DisplayName = "Item Class ID")]
        public virtual int? ItemClassID { get; set; }
        public abstract class itemClassID : PX.Data.BQL.BqlInt.Field<itemClassID> { }
        #endregion

        #region ItemClassCD
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Item Class")]
        public virtual string ItemClassCD { get; set; }
        public abstract class itemClassCD : PX.Data.BQL.BqlString.Field<itemClassCD> { }
        #endregion

        #region ItemClassDescr
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Item Class Description")]
        public virtual string ItemClassDescr { get; set; }
        public abstract class itemClassDescr : PX.Data.BQL.BqlString.Field<itemClassDescr> { }
        #endregion
        
        #region EndingQty_FinYtdQty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Ending Qty")]
        public virtual Decimal? EndingQty_FinYtdQty { get; set; }
        public abstract class endingQty_FinYtdQty : PX.Data.BQL.BqlDecimal.Field<endingQty_FinYtdQty> { }
        #endregion

        #region EndingCost_FinYtdCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Ending Cost")]
        public virtual Decimal? EndingCost_FinYtdCost { get; set; }
        public abstract class endingCost_FinYtdCost : PX.Data.BQL.BqlDecimal.Field<endingCost_FinYtdCost> { }
        #endregion

        #region PeriodQtyWithin30D
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty 0 - 30 Days")]
        public virtual Decimal? PeriodQtyWithin30D { get; set; }
        public abstract class periodQtyWithin30D : PX.Data.BQL.BqlDecimal.Field<periodQtyWithin30D> { }
        #endregion

        #region PeriodQtyFrom30Dto60D
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty 30 - 60 Days")]
        public virtual Decimal? PeriodQtyFrom30Dto60D { get; set; }
        public abstract class periodQtyFrom30Dto60D : PX.Data.BQL.BqlDecimal.Field<periodQtyFrom30Dto60D> { }
        #endregion

        #region PeriodQtyFrom60Dto90D
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty 60 - 90 Days")]
        public virtual Decimal? PeriodQtyFrom60Dto90D { get; set; }
        public abstract class periodQtyFrom60Dto90D : PX.Data.BQL.BqlDecimal.Field<periodQtyFrom60Dto90D> { }
        #endregion

        #region PeriodQtyFrom4Mto6M
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty 4M - 6M")]
        public virtual Decimal? PeriodQtyFrom4Mto6M { get; set; }
        public abstract class periodQtyFrom4Mto6M : PX.Data.BQL.BqlDecimal.Field<periodQtyFrom4Mto6M> { }
        #endregion

        #region PeriodQtyFrom7Mto12M
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty 7M - 12M")]
        public virtual Decimal? PeriodQtyFrom7Mto12M { get; set; }
        public abstract class periodQtyFrom7Mto12M : PX.Data.BQL.BqlDecimal.Field<periodQtyFrom7Mto12M> { }
        #endregion

        #region PeriodQtyOver1Y
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty > 1 Year")]
        public virtual Decimal? PeriodQtyOver1Y { get; set; }
        public abstract class periodQtyOver1Y : PX.Data.BQL.BqlDecimal.Field<periodQtyOver1Y> { }
        #endregion

        #region PeriodCostWithin30D
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Cost 0 - 30 Days")]
        public virtual Decimal? PeriodCostWithin30D { get; set; }
        public abstract class periodCostWithin30D : PX.Data.BQL.BqlDecimal.Field<periodCostWithin30D> { }
        #endregion

        #region PeriodCostFrom30Dto60D
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Cost 30 - 60 Days")]
        public virtual Decimal? PeriodCostFrom30Dto60D { get; set; }
        public abstract class periodCostFrom30Dto60D : PX.Data.BQL.BqlDecimal.Field<periodCostFrom30Dto60D> { }
        #endregion

        #region PeriodCostFrom60Dto90D
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Cost 60 - 90 Days")]
        public virtual Decimal? PeriodCostFrom60Dto90D { get; set; }
        public abstract class periodCostFrom60Dto90D : PX.Data.BQL.BqlDecimal.Field<periodCostFrom60Dto90D> { }
        #endregion

        #region PeriodCostFrom4Mto6M
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Cost 4M - 6M")]
        public virtual Decimal? PeriodCostFrom4Mto6M { get; set; }
        public abstract class periodCostFrom4Mto6M : PX.Data.BQL.BqlDecimal.Field<periodCostFrom4Mto6M> { }
        #endregion

        #region PeriodCostFrom7Mto12M
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Cost 7M - 12M")]
        public virtual Decimal? PeriodCostFrom7Mto12M { get; set; }
        public abstract class periodCostFrom7Mto12M : PX.Data.BQL.BqlDecimal.Field<periodCostFrom7Mto12M> { }
        #endregion

        #region PeriodCostOver1Y
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Cost > 1 Year")]
        public virtual Decimal? PeriodCostOver1Y { get; set; }
        public abstract class periodCostOver1Y : PX.Data.BQL.BqlDecimal.Field<periodCostOver1Y> { }
        #endregion

        #region LastActivityPeriod
        [PXDBString(6, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Last Activity Period")]
        public virtual string LastActivityPeriod { get; set; }
        public abstract class lastActivityPeriod : PX.Data.BQL.BqlString.Field<lastActivityPeriod> { }
        #endregion

        #region ConditionPeriod
        [PXDBString(6, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Condition Period")]
        public virtual string ConditionPeriod { get; set; }
        public abstract class conditionPeriod : PX.Data.BQL.BqlString.Field<conditionPeriod> { }
        #endregion

        #region LastSalesDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Last Sales Date")]
        public virtual DateTime? LastSalesDate { get; set; }
        public abstract class lastSalesDate : PX.Data.BQL.BqlDateTime.Field<lastSalesDate> { }
        #endregion

        #region LastSalesDoc
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Last Sales Document")]
        public virtual string LastSalesDoc { get; set; }
        public abstract class lastSalesDoc : PX.Data.BQL.BqlString.Field<lastSalesDoc> { }
        #endregion

        #region LastReceiptDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Last Receipt Date")]
        public virtual DateTime? LastReceiptDate { get; set; }
        public abstract class lastReceiptDate : PX.Data.BQL.BqlDateTime.Field<lastReceiptDate> { }
        #endregion

        #region LastReceiptDoc
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Last Receipt Document")]
        public virtual string LastReceiptDoc { get; set; }
        public abstract class lastReceiptDoc : PX.Data.BQL.BqlString.Field<lastReceiptDoc> { }
        #endregion
    }
}
