using System;
using PX.Data;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMShopifySettlementTransData")]
    public class LUMShopifySettlementTransData : IBqlTable
    {
        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region TransSequenceNumber
        [PXDBIdentity(IsKey = true)]
        [PXUIField(DisplayName = "Trans Sequence Number")]
        public virtual int? TransSequenceNumber { get; set; }
        public abstract class transSequenceNumber : PX.Data.BQL.BqlInt.Field<transSequenceNumber> { }
        #endregion

        #region Marketplace
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Marketplace")]
        public virtual string Marketplace { get; set; }
        public abstract class marketplace : PX.Data.BQL.BqlString.Field<marketplace> { }
        #endregion

        #region TransactionDate
        [PXDBDate(IsKey = true, UseTimeZone = false)]
        [PXUIField(DisplayName = "Transaction Date")]
        public virtual DateTime? TransactionDate { get; set; }
        public abstract class transactionDate : PX.Data.BQL.BqlDateTime.Field<transactionDate> { }
        #endregion

        #region TransactionType
        [PXDBString(100, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Type")]
        public virtual string TransactionType { get; set; }
        public abstract class transactionType : PX.Data.BQL.BqlString.Field<transactionType> { }
        #endregion

        #region OrderID
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Order")]
        public virtual string OrderID { get; set; }
        public abstract class orderID : PX.Data.BQL.BqlString.Field<orderID> { }
        #endregion

        #region PayoutDate
        [PXDBDate(IsKey = true,UseTimeZone = false)]
        [PXUIField(DisplayName = "Payout Date")]
        public virtual DateTime? PayoutDate { get; set; }
        public abstract class payoutDate : PX.Data.BQL.BqlDateTime.Field<payoutDate> { }
        #endregion

        #region Amount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Amount")]
        public virtual Decimal? Amount { get; set; }
        public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion

        #region Fee
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Fee")]
        public virtual Decimal? Fee { get; set; }
        public abstract class fee : PX.Data.BQL.BqlDecimal.Field<fee> { }
        #endregion

        #region Net
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Net")]
        public virtual Decimal? Net { get; set; }
        public abstract class net : PX.Data.BQL.BqlDecimal.Field<net> { }
        #endregion

        #region Checkout
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Checkout")]
        [PXDefault]
        public virtual string Checkout { get; set; }
        public abstract class checkout : PX.Data.BQL.BqlString.Field<checkout> { }
        #endregion

        #region PaymentMethodName
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Payment Method Name")]
        [PXDefault]
        public virtual string PaymentMethodName { get; set; }
        public abstract class paymentMethodName : PX.Data.BQL.BqlString.Field<paymentMethodName> { }
        #endregion

        #region Currency
        [PXDBString(3, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Currency")]
        public virtual string Currency { get; set; }
        public abstract class currency : PX.Data.BQL.BqlString.Field<currency> { }
        #endregion

        #region PresentmentAmount
        [PXDBDecimal]
        [PXUIField(DisplayName = "Presentment Amount")]
        public virtual decimal? PresentmentAmount { get; set; }
        public abstract class presentmentAmount : PX.Data.BQL.BqlDecimal.Field<presentmentAmount> { }
        #endregion

        #region PresentmentCurrency
        [PXDBString(3)]
        [PXUIField(DisplayName = "Presentment Currency")]
        public virtual string PresentmentCurrency { get; set; }
        public abstract class presentmentCurrency : PX.Data.BQL.BqlString.Field<presentmentCurrency> { }
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