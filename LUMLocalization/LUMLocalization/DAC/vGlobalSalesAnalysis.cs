using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AR;
using PX.Objects.CM;
using PX.Objects.CR;
using PX.Objects.GL;
using PX.Objects.IN;

namespace LUMLocalization.DAC
{
    [Serializable]
    [PXCacheName("vGlobalSalesAnalysis")]
    public class vGlobalSalesAnalysis : IBqlTable
    {
        #region CompanyCD
        [PXDBString(128, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Company CD")]
        public virtual string CompanyCD { get; set; }
        public abstract class companyCD : PX.Data.BQL.BqlString.Field<companyCD> { }
        #endregion

        #region BranchID
        [Branch(typeof(ARRegister.branchID))]
        [PXUIField(DisplayName = "Branch ID")]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region TranDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Tran Date")]
        public virtual DateTime? TranDate { get; set; }
        public abstract class tranDate : PX.Data.BQL.BqlDateTime.Field<tranDate> { }
        #endregion

        #region Drcr
        [PXDBString(1, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Drcr")]
        public virtual string Drcr { get; set; }
        public abstract class drcr : PX.Data.BQL.BqlString.Field<drcr> { }
        #endregion

        #region TranAmt
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Tran Amt")]
        public virtual Decimal? TranAmt { get; set; }
        public abstract class tranAmt : PX.Data.BQL.BqlDecimal.Field<tranAmt> { }
        #endregion

        #region TranCost
        [PXDBBaseCury()]
        [PXUIField(DisplayName = "Tran Cost")]
        public virtual Decimal? TranCost { get; set; }
        public abstract class tranCost : PX.Data.BQL.BqlDecimal.Field<tranCost> { }
        #endregion

        #region IsTranCostFinal
        [PXDBBool()]
        [PXUIField(DisplayName = "Is Tran Cost Final")]
        public virtual bool? IsTranCostFinal { get; set; }
        public abstract class isTranCostFinal : PX.Data.BQL.BqlBool.Field<isTranCostFinal> { }
        #endregion

        #region SOOrderNbr
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "SOOrder Nbr")]
        public virtual string SOOrderNbr { get; set; }
        public abstract class sOOrderNbr : PX.Data.BQL.BqlString.Field<sOOrderNbr> { }
        #endregion

        #region SOOrderType
        [PXDBString(2, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "SOOrder Type")]
        public virtual string SOOrderType { get; set; }
        public abstract class sOOrderType : PX.Data.BQL.BqlString.Field<sOOrderType> { }
        #endregion

        #region TranType
        [PXDBString(3, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Tran Type")]
        public virtual string TranType { get; set; }
        public abstract class tranType : PX.Data.BQL.BqlString.Field<tranType> { }
        #endregion

        #region ItemClassID
        [PXDBInt()]
        [PXUIField(DisplayName = "Item Class ID")]
        public virtual int? ItemClassID { get; set; }
        public abstract class itemClassID : PX.Data.BQL.BqlInt.Field<itemClassID> { }
        #endregion

        #region ItemClassDescr
        [PXDBString()]
        [PXUIField(DisplayName = "Item Class Descr")]
        public virtual string ItemClassDescr { get; set; }
        public abstract class itemClassDescr : PX.Data.BQL.BqlString.Field<itemClassDescr> { }
        #endregion

        #region InventoryID
        [ARTranInventoryItem(Filterable = true)]
        [PXForeignReference(typeof(Field<inventoryID>.IsRelatedTo<PX.Objects.IN.InventoryItem.inventoryID>))]
        [PXUIField(DisplayName = "Inventory ID")]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region InventoryCD
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "InventoryCD")]
        public virtual string InventoryCD { get; set; }
        public abstract class inventoryCD : PX.Data.BQL.BqlString.Field<inventoryCD> { }
        #endregion

        #region InventoryDescr
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Inventory Descr")]
        public virtual string InventoryDescr { get; set; }
        public abstract class inventoryDescr : PX.Data.BQL.BqlString.Field<inventoryDescr> { }
        #endregion

        #region CustomerClassID
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Customer Class ID")]
        public virtual string CustomerClassID { get; set; }
        public abstract class customerClassID : PX.Data.BQL.BqlString.Field<customerClassID> { }
        #endregion

        #region CustomerClassDescr
        [PXDBString(60, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Customer Class Descr")]
        public virtual string CustomerClassDescr { get; set; }
        public abstract class customerClassDescr : PX.Data.BQL.BqlString.Field<customerClassDescr> { }
        #endregion

        #region CustomerID
        [PXDBInt()]
        [PXUIField(DisplayName = "Customer ID")]
        public virtual int? CustomerID { get; set; }
        public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
        #endregion

        #region CustomerName
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Customer Name")]
        public virtual string CustomerName { get; set; }
        public abstract class customerName : PX.Data.BQL.BqlString.Field<customerName> { }
        #endregion

        #region SalesPersonID
        //[PXDBInt()]
        [SalesPerson()]
        [PXUIField(DisplayName = "Sales Person ID")]
        public virtual int? SalesPersonID { get; set; }
        public abstract class salesPersonID : PX.Data.BQL.BqlInt.Field<salesPersonID> { }
        #endregion

        #region Released
        [PXDBBool()]
        [PXUIField(DisplayName = "Released")]
        public virtual bool? Released { get; set; }
        public abstract class released : PX.Data.BQL.BqlBool.Field<released> { }
        #endregion

        #region Qty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty")]
        public virtual Decimal? Qty { get; set; }
        public abstract class qty : PX.Data.BQL.BqlDecimal.Field<qty> { }
        #endregion

        #region CountryID
        [PXDBString(2, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Country ID")]
        [Country]
        public virtual string CountryID { get; set; }
        public abstract class countryID : PX.Data.BQL.BqlString.Field<countryID> { }
        #endregion

        #region InvoiceBranchID
        [PXDBInt()]
        [PXUIField(DisplayName = "Invoice Branch ID")]
        public virtual int? InvoiceBranchID { get; set; }
        public abstract class invoiceBranchID : PX.Data.BQL.BqlInt.Field<invoiceBranchID> { }
        #endregion

        #region InvoiceBranchCD
        [PXDBString(128, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Invoice Branch CD")]
        public virtual string InvoiceBranchCD { get; set; }
        public abstract class invoiceBranchCD : PX.Data.BQL.BqlString.Field<invoiceBranchCD> { }
        #endregion

        #region BaseCuryID
        [PXDBString(5, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Base Cury ID")]
        [PXSelector(typeof(Search<CurrencyList.curyID>))]
        public virtual string BaseCuryID { get; set; }
        public abstract class baseCuryID : PX.Data.BQL.BqlString.Field<baseCuryID> { }
        #endregion

        #region CuryRate
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Currency Rate")]
        public virtual Decimal? CuryRate { get; set; }
        public abstract class curyRate : PX.Data.BQL.BqlDecimal.Field<curyRate> { }
        #endregion

        #region Uom
        [INUnit(typeof(vGlobalSalesAnalysis.inventoryID))]
        [PXUIField(DisplayName = "UOM")]
        public virtual string Uom { get; set; }
        public abstract class uom : PX.Data.BQL.BqlString.Field<uom> { }
        #endregion
    }
}