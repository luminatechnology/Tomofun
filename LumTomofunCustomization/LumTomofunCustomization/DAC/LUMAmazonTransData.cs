using System;
using PX.Data;
using PX.Objects.GL;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMAmazonTransData")]
    public class LUMAmazonTransData : IBqlTable
    {
        public class FK
        {
            public class AmazonSourceData : LUMAmazonSourceData.PK.ForeignKeyOf<LUMAmazonSourceData>.By<sequenceNumber> { }
        }

        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region TransSequenceNumber
        [PXDBIdentity(IsKey = true)]
        public virtual int? TransSequenceNumber { get; set; }
        public abstract class transSequenceNumber : PX.Data.BQL.BqlInt.Field<transSequenceNumber> { }
        #endregion

        #region SequenceNumber
        [PXDBInt()]
        [PXUIField(DisplayName = "Sequence Number")]
        public virtual int? SequenceNumber { get; set; }
        public abstract class sequenceNumber : PX.Data.BQL.BqlInt.Field<sequenceNumber> { }
        #endregion

        #region BranchID
        [Branch()]
        [PXDefault(typeof(AccessInfo.branchID), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region Apitype
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Apitype")]
        public virtual string Apitype { get; set; }
        public abstract class apitype : PX.Data.BQL.BqlString.Field<apitype> { }
        #endregion

        #region TransactionType
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction Type")]
        public virtual string TransactionType { get; set; }
        public abstract class transactionType : PX.Data.BQL.BqlString.Field<transactionType> { }
        #endregion

        #region Marketplace
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Marketplace")]
        public virtual string Marketplace { get; set; }
        public abstract class marketplace : PX.Data.BQL.BqlString.Field<marketplace> { }
        #endregion

        #region OrderID
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Amazon Order ID")]
        public virtual string OrderID { get; set; }
        public abstract class orderID : PX.Data.BQL.BqlString.Field<orderID> { }
        #endregion

        #region SalesChannel
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Sales Channel")]
        public string SalesChannel { get; set; }
        public abstract class salesChannel : PX.Data.BQL.BqlString.Field<salesChannel> { }
        #endregion

        #region OrderStatus
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Order Status")]
        public string OrderStatus { get; set; }
        public abstract class orderStatus : PX.Data.BQL.BqlString.Field<orderStatus> { }
        #endregion

        #region Amount
        [PXDBDecimal]
        [PXUIField(DisplayName = "Amount")]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion

        #region TransJson
        [PXDBString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction Json")]
        public virtual string TransJson { get; set; }
        public abstract class transJson : PX.Data.BQL.BqlString.Field<transJson> { }
        #endregion

        #region IsProcessed
        [PXDBBool]
        [PXUIField(DisplayName = "IsProcessed")]
        public virtual bool? IsProcessed { get; set; }
        public abstract class isProcessed : PX.Data.BQL.BqlBool.Field<isProcessed> { }
        #endregion

        #region ErrorMessage
        [PXDBString(500, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Error Message")]
        public virtual string ErrorMessage { get; set; }
        public abstract class errorMessage : PX.Data.BQL.BqlString.Field<errorMessage> { }
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