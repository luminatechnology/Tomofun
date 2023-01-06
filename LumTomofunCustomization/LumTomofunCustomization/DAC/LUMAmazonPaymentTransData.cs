using System;
using PX.Data;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMAmazonPaymentTransData")]
    public class LUMAmazonPaymentTransData : IBqlTable
    {

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

        #region Marketplace
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Marketplace")]
        public virtual string Marketplace { get; set; }
        public abstract class marketplace : PX.Data.BQL.BqlString.Field<marketplace> { }
        #endregion

        #region TransactionPostedDate
        [PXDBDateAndTime(UseTimeZone = true, PreserveTime = true)]
        [PXUIField(DisplayName = "Transaction Posted Date")]
        public virtual DateTime? TransactionPostedDate { get; set; }
        public abstract class transactionPostedDate : PX.Data.BQL.BqlDateTime.Field<transactionPostedDate> { }
        #endregion

        #region TransactionType
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction Type")]
        public virtual string TransactionType { get; set; }
        public abstract class transactionType : PX.Data.BQL.BqlString.Field<transactionType> { }
        #endregion

        #region AmazonTransactionId
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Amazon Transaction Id")]
        public virtual string AmazonTransactionId { get; set; }
        public abstract class amazonTransactionId : PX.Data.BQL.BqlString.Field<amazonTransactionId> { }
        #endregion

        #region AmazonOrderReferenceId
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Amazon Order Reference Id")]
        public virtual string AmazonOrderReferenceId { get; set; }
        public abstract class amazonOrderReferenceId : PX.Data.BQL.BqlString.Field<amazonOrderReferenceId> { }
        #endregion

        #region SellerOrderId
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Seller Order Id")]
        public virtual string SellerOrderId { get; set; }
        public abstract class sellerOrderId : PX.Data.BQL.BqlString.Field<sellerOrderId> { }
        #endregion

        #region OrderID
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "OrderID")]
        [PXDefault]
        public virtual string OrderID { get; set; }
        public abstract class orderID : PX.Data.BQL.BqlString.Field<orderID> { }
        #endregion

        #region TransactionAmount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Transaction Amount")]
        public virtual Decimal? TransactionAmount { get; set; }
        public abstract class transactionAmount : PX.Data.BQL.BqlDecimal.Field<transactionAmount> { }
        #endregion

        #region TotalTransactionFee
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Total Transaction Fee")]
        public virtual Decimal? TotalTransactionFee { get; set; }
        public abstract class totalTransactionFee : PX.Data.BQL.BqlDecimal.Field<totalTransactionFee> { }
        #endregion

        #region NetTransactionAmount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Net Transaction Amount")]
        public virtual Decimal? NetTransactionAmount { get; set; }
        public abstract class netTransactionAmount : PX.Data.BQL.BqlDecimal.Field<netTransactionAmount> { }
        #endregion

        #region SettlementId
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Settlement Id")]
        public virtual string SettlementId { get; set; }
        public abstract class settlementId : PX.Data.BQL.BqlString.Field<settlementId> { }
        #endregion

        #region CurrencyCode
        [PXDBString(3, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Currency Code")]
        public virtual string CurrencyCode { get; set; }
        public abstract class currencyCode : PX.Data.BQL.BqlString.Field<currencyCode> { }
        #endregion

        #region IsProcessed
        [PXDBBool()]
        [PXUIField(DisplayName = "Is Processed")]
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