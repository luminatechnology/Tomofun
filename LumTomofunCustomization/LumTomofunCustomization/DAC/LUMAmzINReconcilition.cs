using System;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.IN;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("Amazon IN Reconcilition")]
    public class LUMAmzINReconcilition : IBqlTable
    {
        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region IdentityID
        [PXDBIdentity(IsKey = true)]
        public virtual int? IdentityID { get; set; }
        public abstract class identityID : PX.Data.BQL.BqlInt.Field<identityID> { }
        #endregion
    
        #region SnapshotDate
        [PXDBDateAndTime(PreserveTime = true, UseTimeZone = false)]
        [PXUIField(DisplayName = "Snapshot Date")]
        public virtual DateTime? SnapshotDate { get; set; }
        public abstract class snapshotDate : PX.Data.BQL.BqlDateTime.Field<snapshotDate> { }
        #endregion

        #region FNSku
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "FN Sku")]
        public virtual string FNSku { get; set; }
        public abstract class fNSku : PX.Data.BQL.BqlString.Field<fNSku> { }
        #endregion

        #region Sku
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "Sku")]
        public virtual string Sku { get; set; }
        public abstract class sku : PX.Data.BQL.BqlString.Field<sku> { }
        #endregion
    
        #region ProductName
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = "Product Name")]
        public virtual string ProductName { get; set; }
        public abstract class productName : PX.Data.BQL.BqlString.Field<productName> { }
        #endregion
    
        #region Qty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Quantity")]
        public virtual decimal? Qty { get; set; }
        public abstract class qty : PX.Data.BQL.BqlDecimal.Field<qty> { }
        #endregion
    
        #region FBACenterID
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Fulfillment Center ID")]
        public virtual string FBACenterID { get; set; }
        public abstract class fBACenterID : PX.Data.BQL.BqlString.Field<fBACenterID> { }
        #endregion
    
        #region DetailedDesc
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = "Detailed Description")]
        public virtual string DetailedDesc { get; set; }
        public abstract class detailedDesc : PX.Data.BQL.BqlString.Field<detailedDesc> { }
        #endregion
    
        #region CountryID
        [PXDBString(2, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Country")]
        public virtual string CountryID { get; set; }
        public abstract class countryID : PX.Data.BQL.BqlString.Field<countryID> { }
        #endregion
    
        #region Warehouse
        [Site(DisplayName = "Warehouse")]
        public virtual int? Warehouse { get; set; }
        public abstract class warehouse : PX.Data.BQL.BqlInt.Field<warehouse> { }
        #endregion
    
        #region Location
        [Location(typeof(warehouse))]
        public virtual int? Location { get; set; }
        public abstract class location : PX.Data.BQL.BqlInt.Field<location> { }
        #endregion
    
        #region IsProcesses
        [PXDBBool()]
        [PXUIField(DisplayName = "Is Processes")]
        [PXDefault(false)]
        public virtual bool? IsProcesses { get; set; }
        public abstract class isProcesses : PX.Data.BQL.BqlBool.Field<isProcesses> { }
        #endregion

        #region ReportID
        [PXDBString(20, IsUnicode = true)]
        [PXUIField(DisplayName = "Report ID")]
        public virtual string ReportID { get; set; }
        public abstract class reportID : PX.Data.BQL.BqlString.Field<reportID> { }
        #endregion

        #region ERPSku
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "ERP Sku")]
        public virtual string ERPSku { get; set; }
        public abstract class eRPSku : PX.Data.BQL.BqlString.Field<eRPSku> { }
        #endregion

        #region INDate
        [PXDBDate(UseTimeZone = false)]
        [PXUIField(DisplayName = "IN Date")]
        public virtual DateTime? INDate { get; set; }
        public abstract class iNDate : PX.Data.BQL.BqlDateTime.Field<iNDate> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
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
        [PXUIField(DisplayName = "Created On")]
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
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion
    
        #region NoteID
        [PXNote()]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
    
        #region Tstamp
        [PXDBTimestamp()]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }
}