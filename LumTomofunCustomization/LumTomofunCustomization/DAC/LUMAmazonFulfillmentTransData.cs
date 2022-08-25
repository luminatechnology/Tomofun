using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMAmazonFulfillmentTransData")]
    public class LUMAmazonFulfillmentTransData : IBqlTable
    {
        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region AmazonOrderID
        [PXDBString(20, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Amazon Order ID")]
        public virtual string AmazonOrderID { get; set; }
        public abstract class amazonOrderID : PX.Data.BQL.BqlString.Field<amazonOrderID> { }
        #endregion

        #region MarketPlace
        [PXDBString(20, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Market Place")]
        public virtual string MarketPlace { get; set; }
        public abstract class marketPlace : PX.Data.BQL.BqlString.Field<marketPlace> { }
        #endregion

        #region ShipmentDate
        [PXDBDateAndTime(InputMask = "g", UseTimeZone = false)]
        [PXUIField(DisplayName = "Shipment Date (GMT+?)")]
        public virtual DateTime? ShipmentDate { get; set; }
        public abstract class shipmentDate : PX.Data.BQL.BqlGuid.Field<shipmentDate> { }
        #endregion

        #region ReportID
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Amazon Fulfillment ReportID")]
        public virtual string ReportID { get; set; }
        public abstract class reportID : PX.Data.BQL.BqlString.Field<reportID> { }
        #endregion

        #region IsProcessed
        [PXDBBool()]
        [PXUIField(DisplayName = "Is Processed")]
        public virtual bool? IsProcessed { get; set; }
        public abstract class isProcessed : PX.Data.BQL.BqlBool.Field<isProcessed> { }
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
}