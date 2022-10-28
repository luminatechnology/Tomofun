using System;
using PX.Data;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("v_GlobalINItemSiteHistDay")]
    public class v_GlobalINItemSiteHistDay : IBqlTable
    {
        #region CompanyCD
        [PXDBString(128, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Company CD",Visible = false)]
        public virtual string CompanyCD { get; set; }
        public abstract class companyCD : PX.Data.BQL.BqlString.Field<companyCD> { }
        #endregion

        #region SDate
        [PXDBDate(IsKey = true)]
        [PXUIField(DisplayName = "Start Date", Visible = false)]
        public virtual DateTime? SDate { get; set; }
        public abstract class sDate : PX.Data.BQL.BqlDateTime.Field<sDate> { }
        #endregion

        #region InventoryID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Inventory ID", Visible = false)]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region InventoryCD
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "SKU")]
        public virtual string InventoryCD { get; set; }
        public abstract class inventoryCD : PX.Data.BQL.BqlString.Field<inventoryCD> { }
        #endregion

        #region InventoryITemDescr
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "SKU Descr.")]
        public virtual string InventoryITemDescr { get; set; }
        public abstract class inventoryITemDescr : PX.Data.BQL.BqlString.Field<inventoryITemDescr> { }
        #endregion

        #region EndQty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "End Qty.")]
        public virtual Decimal? EndQty { get; set; }
        public abstract class endQty : PX.Data.BQL.BqlDecimal.Field<endQty> { }
        #endregion

        #region Siteid
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Site ID", Visible = false)]
        public virtual int? Siteid { get; set; }
        public abstract class siteid : PX.Data.BQL.BqlInt.Field<siteid> { }
        #endregion

        #region SiteCD
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Warehouse")]
        public virtual string SiteCD { get; set; }
        public abstract class siteCD : PX.Data.BQL.BqlString.Field<siteCD> { }
        #endregion

        #region SiteDescr
        [PXDBString(60, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Site Descr.", Visible = false)]
        public virtual string SiteDescr { get; set; }
        public abstract class siteDescr : PX.Data.BQL.BqlString.Field<siteDescr> { }
        #endregion

        #region LocationID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Location ID", Visible = false)]
        public virtual int? LocationID { get; set; }
        public abstract class locationID : PX.Data.BQL.BqlInt.Field<locationID> { }
        #endregion

        #region LocationCD
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Location")]
        public virtual string LocationCD { get; set; }
        public abstract class locationCD : PX.Data.BQL.BqlString.Field<locationCD> { }
        #endregion

        #region LocationDescr
        [PXDBString(60, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Location Descr", Visible = false)]
        public virtual string LocationDescr { get; set; }
        public abstract class locationDescr : PX.Data.BQL.BqlString.Field<locationDescr> { }
        #endregion

        #region WarehouseQty
        [PXDecimal()]
        [PXUIField(DisplayName = "Warehouse Qty")]
        public virtual Decimal? WarehouseQty { get; set; }
        public abstract class warehouseQty : PX.Data.BQL.BqlDecimal.Field<warehouseQty> { }
        #endregion

        #region VarQty
        [PXDecimal()]
        [PXUIField(DisplayName = "Variant Qty.")]
        public virtual Decimal? VarQty { get; set; }
        public abstract class varQty : PX.Data.BQL.BqlDecimal.Field<varQty> { }
        #endregion
    }
}