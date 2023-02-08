using System;
using PX.Data;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMAmazonCAPaymentReport")]
    public class LUMAmazonCAPaymentReport : IBqlTable
    {
        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region SequenceNumber
        [PXDBIdentity(IsKey = true)]
        [PXUIField(DisplayName = "Sequence Number", Visible = false)]
        public virtual int? SequenceNumber { get; set; }
        public abstract class sequenceNumber : PX.Data.BQL.BqlInt.Field<sequenceNumber> { }
        #endregion

        #region ReportDateTime
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "date/time")]
        public virtual string ReportDateTime { get; set; }
        public abstract class reportDateTime : PX.Data.BQL.BqlString.Field<reportDateTime> { }
        #endregion

        #region Settlementid
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "settlement id")]
        public virtual string Settlementid { get; set; }
        public abstract class settlementid : PX.Data.BQL.BqlString.Field<settlementid> { }
        #endregion

        #region ReportType
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "type")]
        public virtual string ReportType { get; set; }
        public abstract class reportType : PX.Data.BQL.BqlString.Field<reportType> { }
        #endregion

        #region OrderID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "order id")]
        public virtual string OrderID { get; set; }
        public abstract class orderID : PX.Data.BQL.BqlString.Field<orderID> { }
        #endregion

        #region Sku
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "sku")]
        public virtual string Sku { get; set; }
        public abstract class sku : PX.Data.BQL.BqlString.Field<sku> { }
        #endregion

        #region Description
        [PXDBString(300, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region ProductSales
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "product sales")]
        public virtual string ProductSales { get; set; }
        public abstract class productSales : PX.Data.BQL.BqlString.Field<productSales> { }
        #endregion

        #region ProductSalesTax
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "product sales tax")]
        public virtual string ProductSalesTax { get; set; }
        public abstract class productSalesTax : PX.Data.BQL.BqlString.Field<productSalesTax> { }
        #endregion

        #region ShippingCredits
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "shipping credits")]
        public virtual string ShippingCredits { get; set; }
        public abstract class shippingCredits : PX.Data.BQL.BqlString.Field<shippingCredits> { }
        #endregion

        #region ShippingCreditsTax
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "shipping credits tax")]
        public virtual string ShippingCreditsTax { get; set; }
        public abstract class shippingCreditsTax : PX.Data.BQL.BqlString.Field<shippingCreditsTax> { }
        #endregion

        #region GiftWrapCredits
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "gift wrap credits")]
        public virtual string GiftWrapCredits { get; set; }
        public abstract class giftWrapCredits : PX.Data.BQL.BqlString.Field<giftWrapCredits> { }
        #endregion

        #region GiftWrapCreditsTax
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "giftwrap credits tax")]
        public virtual string GiftWrapCreditsTax { get; set; }
        public abstract class giftWrapCreditsTax : PX.Data.BQL.BqlString.Field<giftWrapCreditsTax> { }
        #endregion

        #region RegulatoryFee
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Regulatory fee")]
        public virtual string RegulatoryFee { get; set; }
        public abstract class regulatoryFee : PX.Data.BQL.BqlString.Field<regulatoryFee> { }
        #endregion

        #region TaxOnRegulatoryFee
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Tax on regulatory fee")]
        public virtual string TaxOnRegulatoryFee { get; set; }
        public abstract class taxOnRegulatoryFee : PX.Data.BQL.BqlString.Field<taxOnRegulatoryFee> { }
        #endregion

        #region PromotionalRebates
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "promotional rebates")]
        public virtual string PromotionalRebates { get; set; }
        public abstract class promotionalRebates : PX.Data.BQL.BqlString.Field<promotionalRebates> { }
        #endregion

        #region PromotionalRebatesTax
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "promotional rebates tax")]
        public virtual string PromotionalRebatesTax { get; set; }
        public abstract class promotionalRebatesTax : PX.Data.BQL.BqlString.Field<promotionalRebatesTax> { }
        #endregion

        #region MarketplaceWithheldTax
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "marketplace withheld tax")]
        public virtual string MarketplaceWithheldTax { get; set; }
        public abstract class marketplaceWithheldTax : PX.Data.BQL.BqlString.Field<marketplaceWithheldTax> { }
        #endregion

        #region SellingFees
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "selling fees")]
        public virtual string SellingFees { get; set; }
        public abstract class sellingFees : PX.Data.BQL.BqlString.Field<sellingFees> { }
        #endregion

        #region Fbafees
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "fba fees")]
        public virtual string Fbafees { get; set; }
        public abstract class fbafees : PX.Data.BQL.BqlString.Field<fbafees> { }
        #endregion

        #region OtherTransactionFee
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "other transaction fees")]
        public virtual string OtherTransactionFee { get; set; }
        public abstract class otherTransactionFee : PX.Data.BQL.BqlString.Field<otherTransactionFee> { }
        #endregion

        #region OtherFee
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "other")]
        public virtual string OtherFee { get; set; }
        public abstract class otherFee : PX.Data.BQL.BqlString.Field<otherFee> { }
        #endregion

        #region Total
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "total")]
        public virtual string Total { get; set; }
        public abstract class total : PX.Data.BQL.BqlString.Field<total> { }
        #endregion

        #region API_Marketplace
        [PXDBString(2, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "API_ Marketplace")]
        public virtual string API_Marketplace { get; set; }
        public abstract class aPI_Marketplace : PX.Data.BQL.BqlString.Field<aPI_Marketplace> { }
        #endregion

        #region Api_date_1
        [PXDBDate(UseTimeZone = false, PreserveTime = true, DisplayMask = "g")]
        [PXUIField(DisplayName = "Api_date_1")]
        public virtual DateTime? Api_date_1 { get; set; }
        public abstract class api_date_1 : PX.Data.BQL.BqlDateTime.Field<api_date_1> { }
        #endregion

        #region Api_date
        [PXDBDate(UseTimeZone = false, PreserveTime = true, DisplayMask = "g")]
        [PXUIField(DisplayName = "Api_date")]
        public virtual DateTime? Api_date { get; set; }
        public abstract class api_date : PX.Data.BQL.BqlDateTime.Field<api_date> { }
        #endregion

        #region Api_settlementid
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Api_settlementid")]
        public virtual string Api_settlementid { get; set; }
        public abstract class api_settlementid : PX.Data.BQL.BqlString.Field<api_settlementid> { }
        #endregion

        #region Api_trantype
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Api_trantype")]
        public virtual string Api_trantype { get; set; }
        public abstract class api_trantype : PX.Data.BQL.BqlString.Field<api_trantype> { }
        #endregion

        #region Api_orderid
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Api_orderid")]
        public virtual string Api_orderid { get; set; }
        public abstract class api_orderid : PX.Data.BQL.BqlString.Field<api_orderid> { }
        #endregion

        #region Api_sku
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Api_sku")]
        public virtual string Api_sku { get; set; }
        public abstract class api_sku : PX.Data.BQL.BqlString.Field<api_sku> { }
        #endregion

        #region Api_description
        [PXDBString(300, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Api_description")]
        public virtual string Api_description { get; set; }
        public abstract class api_description : PX.Data.BQL.BqlString.Field<api_description> { }
        #endregion

        #region Api_productsales
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_productsales")]
        public virtual Decimal? Api_productsales { get; set; }
        public abstract class api_productsales : PX.Data.BQL.BqlDecimal.Field<api_productsales> { }
        #endregion

        #region Api_producttax
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_producttax")]
        public virtual Decimal? Api_producttax { get; set; }
        public abstract class api_producttax : PX.Data.BQL.BqlDecimal.Field<api_producttax> { }
        #endregion

        #region Api_shipping
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_shipping")]
        public virtual Decimal? Api_shipping { get; set; }
        public abstract class api_shipping : PX.Data.BQL.BqlDecimal.Field<api_shipping> { }
        #endregion

        #region Api_shippingtax
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_shippingtax")]
        public virtual Decimal? Api_shippingtax { get; set; }
        public abstract class api_shippingtax : PX.Data.BQL.BqlDecimal.Field<api_shippingtax> { }
        #endregion

        #region Api_giftwrap
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_giftwrap")]
        public virtual Decimal? Api_giftwrap { get; set; }
        public abstract class api_giftwrap : PX.Data.BQL.BqlDecimal.Field<api_giftwrap> { }
        #endregion

        #region Api_giftwraptax
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_giftwraptax")]
        public virtual Decimal? Api_giftwraptax { get; set; }
        public abstract class api_giftwraptax : PX.Data.BQL.BqlDecimal.Field<api_giftwraptax> { }
        #endregion

        #region Api_regulatoryfee
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_regulatoryfee")]
        public virtual Decimal? Api_regulatoryfee { get; set; }
        public abstract class api_regulatoryfee : PX.Data.BQL.BqlDecimal.Field<api_regulatoryfee> { }
        #endregion

        #region Api_taxonregulatoryfee
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_taxonregulatoryfee")]
        public virtual Decimal? Api_taxonregulatoryfee { get; set; }
        public abstract class api_taxonregulatoryfee : PX.Data.BQL.BqlDecimal.Field<api_taxonregulatoryfee> { }
        #endregion

        #region Api_promotion
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_promotion")]
        public virtual Decimal? Api_promotion { get; set; }
        public abstract class api_promotion : PX.Data.BQL.BqlDecimal.Field<api_promotion> { }
        #endregion

        #region Api_promotiontax
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_promotiontax")]
        public virtual Decimal? Api_promotiontax { get; set; }
        public abstract class api_promotiontax : PX.Data.BQL.BqlDecimal.Field<api_promotiontax> { }
        #endregion

        #region Api_whtax
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_whtax")]
        public virtual Decimal? Api_whtax { get; set; }
        public abstract class api_whtax : PX.Data.BQL.BqlDecimal.Field<api_whtax> { }
        #endregion

        #region Api_sellingfee
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_sellingfee")]
        public virtual Decimal? Api_sellingfee { get; set; }
        public abstract class api_sellingfee : PX.Data.BQL.BqlDecimal.Field<api_sellingfee> { }
        #endregion

        #region Api_fbafee
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_fbafee")]
        public virtual Decimal? Api_fbafee { get; set; }
        public abstract class api_fbafee : PX.Data.BQL.BqlDecimal.Field<api_fbafee> { }
        #endregion

        #region Api_othertranfee
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_othertranfee")]
        public virtual Decimal? Api_othertranfee { get; set; }
        public abstract class api_othertranfee : PX.Data.BQL.BqlDecimal.Field<api_othertranfee> { }
        #endregion

        #region Api_otherfee
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_otherfee")]
        public virtual Decimal? Api_otherfee { get; set; }
        public abstract class api_otherfee : PX.Data.BQL.BqlDecimal.Field<api_otherfee> { }
        #endregion

        #region Api_total
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_total")]
        public virtual Decimal? Api_total { get; set; }
        public abstract class api_total : PX.Data.BQL.BqlDecimal.Field<api_total> { }
        #endregion

        #region Api_cod
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_cod")]
        public virtual Decimal? Api_cod { get; set; }
        public abstract class api_cod : PX.Data.BQL.BqlDecimal.Field<api_cod> { }
        #endregion

        #region Api_codfee
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_codfee")]
        public virtual Decimal? Api_codfee { get; set; }
        public abstract class api_codfee : PX.Data.BQL.BqlDecimal.Field<api_codfee> { }
        #endregion

        #region Api_coditemcharge
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_coditemcharge")]
        public virtual Decimal? Api_coditemcharge { get; set; }
        public abstract class api_coditemcharge : PX.Data.BQL.BqlDecimal.Field<api_coditemcharge> { }
        #endregion

        #region Api_points
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Api_points")]
        public virtual Decimal? Api_points { get; set; }
        public abstract class api_points : PX.Data.BQL.BqlDecimal.Field<api_points> { }
        #endregion

        #region IsProcessed
        [PXDBBool()]
        [PXUIField(DisplayName = "Is Processed")]
        public virtual bool? IsProcessed { get; set; }
        public abstract class isProcessed : PX.Data.BQL.BqlBool.Field<isProcessed> { }
        #endregion

        #region ErrorMessage
        [PXDBString(2048, IsUnicode = true, InputMask = "")]
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