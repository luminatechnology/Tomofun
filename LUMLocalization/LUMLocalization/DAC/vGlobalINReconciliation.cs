using System;
using PX.Data;
using PX.Objects.IN;

namespace LUMLocalization.DAC
{
    [Serializable]
    [PXCacheName("view_GlobalINReconciliation")]
    public class vGlobalINReconciliation : IBqlTable
    {
        #region CompanyCD
        [PXDBString(128, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Company")]
        public virtual string CompanyCD { get; set; }
        public abstract class companyCD : PX.Data.BQL.BqlString.Field<companyCD> { }
        #endregion

        #region SnapshotDate
        [PXDBDateAndTime(PreserveTime = true, UseTimeZone = false)]
        [PXUIField(DisplayName = "Tran Date")]
        public virtual DateTime? SnapshotDate { get; set; }
        public abstract class snapshotDate : PX.Data.BQL.BqlDateTime.Field<snapshotDate> { }
        #endregion

        #region INDate
        [PXDBDate(IsKey = true)]
        [PXUIField(DisplayName = "IN Date")]
        public virtual DateTime? INDate { get; set; }
        public abstract class iNDate : PX.Data.BQL.BqlDateTime.Field<iNDate> { }
        #endregion

        #region Sku
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Sku")]
        public virtual string Sku { get; set; }
        public abstract class sku : PX.Data.BQL.BqlString.Field<sku> { }
        #endregion

        #region FBACenterID
        [PXDBString(10, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Fulfillment Center ID")]
        public virtual string FBACenterID { get; set; }
        public abstract class fBACenterID : PX.Data.BQL.BqlString.Field<fBACenterID> { }
        #endregion

        #region InventoryID
        [PXDBInt()]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region InventoryCD
        [PXDBString(50, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "ERP Sku")]
        public virtual string InventoryCD { get; set; }
        public abstract class inventoryCD : PX.Data.BQL.BqlString.Field<inventoryCD> { }
        #endregion

        #region ProductName
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Product Name")]
        public virtual string ProductName { get; set; }
        public abstract class productName : PX.Data.BQL.BqlString.Field<productName> { }
        #endregion

        #region DetailedDesc
        [PXDBString(100, IsUnicode = true, InputMask = "",IsKey = true)]
        [PXUIField(DisplayName = "Detailed Desc")]
        public virtual string DetailedDesc { get; set; }
        public abstract class detailedDesc : PX.Data.BQL.BqlString.Field<detailedDesc> { }
        #endregion

        #region Qty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty")]
        public virtual decimal? Qty { get; set; }
        public abstract class qty : PX.Data.BQL.BqlDecimal.Field<qty> { }
        #endregion

        #region CountryID
        [PXDBString(2, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Country")]
        public virtual string CountryID { get; set; }
        public abstract class countryID : PX.Data.BQL.BqlString.Field<countryID> { }
        #endregion

        #region SiteID
        [PXDBInt()]
        public virtual int? SiteID { get; set; }
        public abstract class siteID : PX.Data.BQL.BqlInt.Field<siteID> { }
        #endregion

        #region SiteCD
        [PXDBString(30, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Warehouse")]
        public virtual string SiteCD { get; set; }
        public abstract class siteCD : PX.Data.BQL.BqlString.Field<siteCD> { }
        #endregion

        #region LocationID
        [PXDBInt()]
        public virtual int? LocationID { get; set; }
        public abstract class locationID : PX.Data.BQL.BqlInt.Field<locationID> { }
        #endregion

        #region LocationCD
        [PXDBString(30, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Location")]
        public virtual string LocationCD { get; set; }
        public abstract class locationCD : PX.Data.BQL.BqlString.Field<locationCD> { }
        #endregion

        #region Source
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Source")]
        public virtual string Source { get; set; }
        public abstract class source : PX.Data.BQL.BqlString.Field<source> { }
        #endregion
    }
}