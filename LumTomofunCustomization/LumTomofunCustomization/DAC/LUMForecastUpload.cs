using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;

namespace LumTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMForecastUpload")]
    public class LUMForecastUpload : IBqlTable
    {
        #region Mrptype
        [PXDBString(20, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "MRP Type")]
        public virtual string Mrptype { get; set; }
        public abstract class mrptype : PX.Data.BQL.BqlString.Field<mrptype> { }
        #endregion

        #region Revision
        [PXDBString(10, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Revision")]
        public virtual string Revision { get; set; }
        public abstract class revision : PX.Data.BQL.BqlString.Field<revision> { }
        #endregion

        #region Sku
        [PXDBString(100, InputMask = "", IsUnicode = true, IsKey = true)]
        //[StockItem]
        //[PXSelector(
        //    typeof(SearchFor<InventoryItem.inventoryID>),
        //    DescriptionField = typeof(InventoryItem.inventoryCD),
        //    SubstituteKey = typeof(InventoryItem.inventoryCD))]
        [PXUIField(DisplayName = "SKU")]
        public virtual string Sku { get; set; }
        public abstract class sku : PX.Data.BQL.BqlString.Field<sku> { }
        #endregion

        #region Company
        [PXDBString(5, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Company")]
        public virtual string Company { get; set; }
        public abstract class company : PX.Data.BQL.BqlString.Field<company> { }
        #endregion

        #region Warehouse
        [PXDBString(30,IsUnicode = true, InputMask = "",IsKey = true)]
        //[PXSelector(
        //    typeof(SearchFor<INSite.siteID>),
        //    SubstituteKey = typeof(INSite.siteCD))]
        [PXUIField(DisplayName = "Warehouse")]
        public virtual string Warehouse { get; set; }
        public abstract class warehouse : PX.Data.BQL.BqlString.Field<warehouse> { }
        #endregion

        #region Date
        [PXDBDate(IsKey = true)]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
        #endregion

        #region Qty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty")]
        public virtual Decimal? Qty { get; set; }
        public abstract class qty : PX.Data.BQL.BqlDecimal.Field<qty> { }
        #endregion

        #region Week
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Week")]
        public virtual string Week { get; set; }
        public abstract class week : PX.Data.BQL.BqlString.Field<week> { }
        #endregion

        #region Qoh
        [PXDBDecimal()]
        [PXUIField(DisplayName = "QOH")]
        public virtual Decimal? Qoh { get; set; }
        public abstract class qoh : PX.Data.BQL.BqlDecimal.Field<qoh> { }
        #endregion

        #region Noteid
        [PXNote()]
        public virtual Guid? Noteid { get; set; }
        public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        [PXUIField(Enabled = false)]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        [PXUIField(Enabled = false)]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        [PXUIField(Enabled = false)]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }
}