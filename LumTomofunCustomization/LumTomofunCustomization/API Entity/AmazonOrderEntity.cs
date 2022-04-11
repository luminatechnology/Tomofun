using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.API_Entity.AmazonOrder
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Item
    {
        public int? QuantityShipped { get; set; }
        public string ShippingPriceCurrencyCode { get; set; }
        public double ShippingDiscountAmount { get; set; }
        public string Title { get; set; }
        public double ShippingPriceAmount { get; set; }
        public string Country { get; set; }
        public string ShippingTaxCurrencyCode { get; set; }
        public string ItemPriceCurrencyCode { get; set; }
        public string OrderId { get; set; }
        public string AmazonOrder_id { get; set; }
        public double ShippingTaxAmount { get; set; }
        public double ItemPriceAmount { get; set; }
        public double GiftWrapPriceAmount { get; set; }
        public string PromotionDiscountCurrencyCode { get; set; }
        public string ItemTaxCurrencyCode { get; set; }
        public string GiftWrapPriceCurrencyCode { get; set; }
        public string ItemId { get; set; }
        public double PromotionDiscountAmount { get; set; }
        public double PromotionDiscountTaxAmount { get; set; }
        public double ItemTaxAmount { get; set; }
        public double GiftWrapTaxAmount { get; set; }
        public int? QuantityOrdered { get; set; }
        public string ASIN { get; set; }
        public string ShippingDiscountCurrencyCode { get; set; }
        public string GiftWrapTaxCurrencyCode { get; set; }
        public string SellerSKU { get; set; }
    }

    public class Order
    {
        public int? PurchaseDate { get; set; }
        public int? NumberOfItemsUnshipped { get; set; }
        public string FulfillmentChannel { get; set; }
        public int? IsPrime { get; set; }
        public string BuyerEmail { get; set; }
        public object BuyerName { get; set; }
        public string PaymentMethod { get; set; }
        public string ShipmentServiceLevelCategory { get; set; }
        public int? LastUpdateDate { get; set; }
        public string CurrencyCode { get; set; }
        public string StateOrRegion { get; set; }
        public string SellerOrderId { get; set; }
        public string ShipServiceLevel { get; set; }
        public double? Amount { get; set; }
        public string City { get; set; }
        public int? IsFilled { get; set; }
        public string OrderId { get; set; }
        public int? NumberOfItemsShipped { get; set; }
        public int? IsPremiumOrder { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public int? LatestShipDate { get; set; }
        public string OrderStatus { get; set; }
        public int? EarliestShipDate { get; set; }
        public string PostalCode { get; set; }
        public string OrderType { get; set; }
        public string SalesChannel { get; set; }
        public string MarketplaceId { get; set; }
        public object Name { get; set; }
        public int? IsBusinessOrder { get; set; }
        public object AddressLine1 { get; set; }
        public List<Item> Items { get; set; }
    }

    public class AmazonOrderEntity
    {
        public List<Order> Orders { get; set; }
    }


}
