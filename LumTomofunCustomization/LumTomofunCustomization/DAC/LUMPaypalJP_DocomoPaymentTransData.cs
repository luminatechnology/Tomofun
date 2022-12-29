using System;
using PX.Data;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMPaypalJP_DocomoPaymentTransData")]
    public class LUMPaypalJP_DocomoPaymentTransData : IBqlTable
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

        #region TransactionDate
        [PXDBDate(UseTimeZone = false)]
        [PXUIField(DisplayName = "日付")]
        public virtual DateTime? TransactionDate { get; set; }
        public abstract class transactionDate : PX.Data.BQL.BqlDateTime.Field<transactionDate> { }
        #endregion

        #region TransactionType
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction Type")]
        public virtual string TransactionType { get; set; }
        public abstract class transactionType : PX.Data.BQL.BqlString.Field<transactionType> { }
        #endregion

        #region OrderID
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Order ID")]
        public virtual string OrderID { get; set; }
        public abstract class orderID : PX.Data.BQL.BqlString.Field<orderID> { }
        #endregion

        #region OrigOrderID
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "プロダクト")]
        public virtual string OrigOrderID { get; set; }
        public abstract class origOrderID : PX.Data.BQL.BqlString.Field<origOrderID> { }
        #endregion

        #region Gross
        [PXDBDecimal()]
        [PXUIField(DisplayName = "価格")]
        public virtual Decimal? Gross { get; set; }
        public abstract class gross : PX.Data.BQL.BqlDecimal.Field<gross> { }
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

        #region Description
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region Currency
        [PXDBString(3, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Currency")]
        public virtual string Currency { get; set; }
        public abstract class currency : PX.Data.BQL.BqlString.Field<currency> { }
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