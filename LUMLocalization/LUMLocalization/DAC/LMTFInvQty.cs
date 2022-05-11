using System;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.IN;

namespace LUMLocalization.DAC
{
    [Serializable]
    [PXCacheName("LMTFInvQty")]
    public class LMTFInvQty : IBqlTable
    {
        #region InventoryID
        [PXDefault(typeof(LMTFInvQty.inventoryID), PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "SKU")]
        [PXSelector(typeof(Search<InventoryItem.inventoryID>), SubstituteKey = typeof(InventoryItem.inventoryCD))]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion
        
        #region Siteid
        [PXDBInt()]
        [PXUIField(DisplayName = "Warehouse")]
        [PXSelector(typeof(Search<INSite.siteID>), SubstituteKey = typeof(INSite.siteCD))]
        public virtual int? Siteid { get; set; }
        public abstract class siteid : PX.Data.BQL.BqlInt.Field<siteid> { }
        #endregion

        #region INLocationID
        [PXDBInt()]
        [PXUIField(DisplayName = "Location")]
        [PXSelector(typeof(Search<INLocation.locationID>), SubstituteKey = typeof(INLocation.locationCD))]
        public virtual int? INLocationID { get; set; }
        public abstract class iNLocationID : PX.Data.BQL.BqlInt.Field<iNLocationID> { }
        #endregion
        
        #region Qty
        [PXDBInt()]
        [PXUIField(DisplayName = "Qty")]
        public virtual int? Qty { get; set; }
        public abstract class qty : PX.Data.BQL.BqlInt.Field<qty> { }
        #endregion

        #region ExportedDate
        [PXDefault(typeof(LMTFInvQty.exportedDate), PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXDBDate(IsKey = true)]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? ExportedDate { get; set; }
        public abstract class exportedDate : PX.Data.BQL.BqlDateTime.Field<exportedDate> { }
        #endregion

        #region FulfillmentCenterID
        [PXDefault(typeof(LMTFInvQty.fulfillmentCenterID), PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXDBString(10, IsKey = true)]
        [PXUIField(DisplayName = "Fulfillment Center ID")]
        public virtual string FulfillmentCenterID { get; set; }
        public abstract class fulfillmentCenterID : PX.Data.BQL.BqlString.Field<fulfillmentCenterID> { }
        #endregion

        #region CountryID
        [PXDefault(typeof(LMTFInvQty.countryID), PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXDBString(5, IsKey = true)]
        [PXUIField(DisplayName = "Country")]
        [PXSelector(typeof(Country.countryID), CacheGlobal = true, DescriptionField = typeof(Country.description))]
        public virtual string CountryID { get; set; }
        public abstract class countryID : PX.Data.BQL.BqlString.Field<countryID> { }
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
    }
}