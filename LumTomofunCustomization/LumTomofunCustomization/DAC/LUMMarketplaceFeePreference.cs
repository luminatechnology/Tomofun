using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using static PX.Objects.IN.INItemTypes;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMMarketplaceFeePreference")]
    public class LUMMarketplaceFeePreference : IBqlTable
    {
        #region Fee
        [PXDBString(100, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Fee")]
        public virtual string Fee { get; set; }
        public abstract class fee : PX.Data.BQL.BqlString.Field<fee> { }
        #endregion

        #region InventoryID
        [PXDBInt()]
        [PXSelector(typeof(SelectFrom<InventoryItem>.Where<InventoryItem.itemType.IsEqual<NonStockTypeAttr>>.SearchFor<InventoryItem.inventoryID>),
            typeof(InventoryItem.inventoryCD),
            typeof(InventoryItem.descr),
            DescriptionField = typeof(InventoryItem.descr),
            SubstituteKey = typeof(InventoryItem.inventoryCD))]
        [PXUIField(DisplayName = "Non-Stock Item")]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region Noteid
        [PXNote()]
        public virtual Guid? Noteid { get; set; }
        public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
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

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }

    public class NonStockTypeAttr : PX.Data.BQL.BqlString.Constant<NonStockTypeAttr>
    {
        public NonStockTypeAttr() : base("N") { }
    }
}