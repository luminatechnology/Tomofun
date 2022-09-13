using System;
using PX.Data;

namespace LUMTomofunCustomization
{
    [Serializable]
    [PXCacheName("v_AmazonSettlementReportQuery")]
    public class v_AmazonSettlementReportQuery : IBqlTable
    {
        #region SettlementID
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Settlement ID")]
        public virtual string SettlementID { get; set; }
        public abstract class settlementID : PX.Data.BQL.BqlString.Field<settlementID> { }
        #endregion

        #region PostedDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Posted Date")]
        public virtual DateTime? PostedDate { get; set; }
        public abstract class postedDate : PX.Data.BQL.BqlDateTime.Field<postedDate> { }
        #endregion

        #region OrderID
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Order ID")]
        public virtual string OrderID { get; set; }
        public abstract class orderID : PX.Data.BQL.BqlString.Field<orderID> { }
        #endregion

        #region TransactionType
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction Type")]
        public virtual string TransactionType { get; set; }
        public abstract class transactionType : PX.Data.BQL.BqlString.Field<transactionType> { }
        #endregion

        #region Amount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Amount")]
        public virtual Decimal? Amount { get; set; }
        public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion

        #region AmountDescriptionSummary
        [PXDBString(1000, IsUnicode = true)]
        [PXUIField(DisplayName = "Amount Description Summary")]
        public virtual string AmountDescriptionSummary { get; set; }
        public abstract class amountDescriptionSummary : PX.Data.BQL.BqlString.Field<amountDescriptionSummary> { }
        #endregion

        #region ErrorMessage
        [PXDBString(500, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Error Message")]
        public virtual string ErrorMessage { get; set; }
        public abstract class errorMessage : PX.Data.BQL.BqlString.Field<errorMessage> { }
        #endregion
    }
}