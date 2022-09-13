using System;
using PX.Data;
using PX.Objects.GL;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMAmazonSettlementTransData")]
    public class LUMAmazonSettlementTransData : IBqlTable
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

        #region BranchID
        [Branch()]
        [PXDefault(typeof(AccessInfo.branchID), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region Marketplace
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Marketplace(Report Country)")]
        public virtual string Marketplace { get; set; }
        public abstract class marketplace : PX.Data.BQL.BqlString.Field<marketplace> { }
        #endregion

        #region SettlementID
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Settlement ID")]
        public virtual string SettlementID { get; set; }
        public abstract class settlementID : PX.Data.BQL.BqlString.Field<settlementID> { }
        #endregion

        #region SettlementStartDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Settlement Start Date")]
        public virtual DateTime? SettlementStartDate { get; set; }
        public abstract class settlementStartDate : PX.Data.BQL.BqlDateTime.Field<settlementStartDate> { }
        #endregion

        #region SettlementEndDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Settlement End Date")]
        public virtual DateTime? SettlementEndDate { get; set; }
        public abstract class settlementEndDate : PX.Data.BQL.BqlDateTime.Field<settlementEndDate> { }
        #endregion

        #region DepositDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Deposit Date")]
        public virtual DateTime? DepositDate { get; set; }
        public abstract class depositDate : PX.Data.BQL.BqlDateTime.Field<depositDate> { }
        #endregion

        #region TotalAmount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Total Amount")]
        public virtual Decimal? TotalAmount { get; set; }
        public abstract class totalAmount : PX.Data.BQL.BqlDecimal.Field<totalAmount> { }
        #endregion

        #region DepositCurrency
        [PXDBString(3, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Deposit Currency")]
        public virtual string DepositCurrency { get; set; }
        public abstract class depositCurrency : PX.Data.BQL.BqlString.Field<depositCurrency> { }
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

        #region AmountType
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Amount Type")]
        public virtual string AmountType { get; set; }
        public abstract class amountType : PX.Data.BQL.BqlString.Field<amountType> { }
        #endregion

        #region AmountDescription
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Amount Description")]
        public virtual string AmountDescription { get; set; }
        public abstract class amountDescription : PX.Data.BQL.BqlString.Field<amountDescription> { }
        #endregion

        #region Amount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Amount")]
        public virtual Decimal? Amount { get; set; }
        public abstract class amount : PX.Data.BQL.BqlDecimal.Field<amount> { }
        #endregion

        #region PostedDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Posted Date")]
        public virtual DateTime? PostedDate { get; set; }
        public abstract class postedDate : PX.Data.BQL.BqlDateTime.Field<postedDate> { }
        #endregion

        #region MarketPlaceName
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Market Place Name")]
        public virtual string MarketPlaceName { get; set; }
        public abstract class marketPlaceName : PX.Data.BQL.BqlString.Field<marketPlaceName> { }
        #endregion

        #region MerchantOrderID
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Merchant Order ID")]
        public virtual string MerchantOrderID { get; set; }
        public abstract class merchantOrderID : PX.Data.BQL.BqlString.Field<merchantOrderID> { }
        #endregion

        #region MerchantOrderItemID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Merchant Order Item ID")]
        public virtual string MerchantOrderItemID { get; set; }
        public abstract class merchantOrderItemID : PX.Data.BQL.BqlString.Field<merchantOrderItemID> { }
        #endregion

        #region Sku
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Sku")]
        public virtual string Sku { get; set; }
        public abstract class sku : PX.Data.BQL.BqlString.Field<sku> { }
        #endregion

        #region QuantityPurchased
        [PXDBInt()]
        [PXUIField(DisplayName = "Quantity Purchased")]
        public virtual int? QuantityPurchased { get; set; }
        public abstract class quantityPurchased : PX.Data.BQL.BqlInt.Field<quantityPurchased> { }
        #endregion

        #region IsProcessed
        [PXDBBool]
        [PXUIField(DisplayName = "IsProcessed", Enabled = false)]
        public virtual bool? IsProcessed { get; set; }
        public abstract class isProcessed : PX.Data.BQL.BqlBool.Field<isProcessed> { }
        #endregion

        #region ErrorMessage
        [PXDBString(500, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Error Message")]
        public virtual string ErrorMessage { get; set; }
        public abstract class errorMessage : PX.Data.BQL.BqlString.Field<errorMessage> { }
        #endregion

        #region CurrentMarketplace
        [PXDBString(20, IsUnicode = true)]
        [PXUIField(DisplayName = "Current Marketplace(Process)", Visible = false)]
        public virtual string CurrentMarketplace { get; set; }
        public abstract class currentMarketplace : PX.Data.BQL.BqlString.Field<currentMarketplace> { }
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