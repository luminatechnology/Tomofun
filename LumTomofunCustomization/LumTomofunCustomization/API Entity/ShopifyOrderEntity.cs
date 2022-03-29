using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.API_Entity
{
    public class ShopifyOrderEntity
    {
        public List<Order> orders;
    }

    public class ShopMoney
    {
        public string amount;
        public string currency_code;
    }

    public class PresentmentMoney
    {
        public string amount;
        public string currency_code;
    }

    public class CurrentSubtotalPriceSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class CurrentTotalDiscountsSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class CurrentTotalPriceSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class CurrentTotalTaxSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class SubtotalPriceSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class TotalDiscountsSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class TotalLineItemsPriceSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class TotalPriceSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class TotalShippingPriceSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class TotalTaxSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class BillingAddress
    {
        public string first_name;
        public string address1;
        public string phone;
        public string city;
        public string zip;
        public string province;
        public string country;
        public string last_name;
        public object address2;
        public object company;
        public string latitude;
        public string longitude;
        public string name;
        public string country_code;
        public string province_code;
    }

    public class DefaultAddress
    {
        public long id;
        public long customer_id;
        public string first_name;
        public string last_name;
        public object company;
        public string address1;
        public object address2;
        public string city;
        public string province;
        public string country;
        public string zip;
        public string phone;
        public string name;
        public string province_code;
        public string country_code;
        public string country_name;
        public bool @default;
    }

    public class Customer
    {
        public long id;
        public string email;
        public bool accepts_marketing;
        public string created_at;
        public string updated_at;
        public string first_name;
        public string last_name;
        public string orders_count;
        public string state;
        public string total_spent;
        public long last_order_id;
        public object note;
        public bool verified_email;
        public object multipass_identifier;
        public bool tax_exempt;
        public object phone;
        public string tags;
        public string last_order_name;
        public string currency;
        public string accepts_marketing_updated_at;
        public object marketing_opt_in_level;
        public List<object> tax_exemptions;
        public object sms_marketing_consent;
        public string admin_graphql_api_id;
        public DefaultAddress default_address;
    }

    public class OriginAddress
    {
    }

    public class Receipt
    {
    }

    public class PreTaxPriceSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class PriceSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class TotalDiscountSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class LineItem
    {
        public long id;
        public string admin_graphql_api_id;
        public string fulfillable_quantity;
        public string fulfillment_service;
        public string fulfillment_status;
        public bool gift_card;
        public string grams;
        public string name;
        public string pre_tax_price;
        public PreTaxPriceSet pre_tax_price_set;
        public string price;
        public PriceSet price_set;
        public bool product_exists;
        public long product_id;
        public List<object> properties;
        public string quantity;
        public bool requires_shipping;
        public string sku;
        public bool taxable;
        public string title;
        public string total_discount;
        public TotalDiscountSet total_discount_set;
        public long variant_id;
        public string variant_inventory_management;
        public string variant_title;
        public string vendor;
        public List<object> tax_lines;
        public List<object> duties;
        public List<object> discount_allocations;
    }

    public class Fulfillment
    {
        public long id;
        public string admin_graphql_api_id;
        public string created_at;
        public string location_id;
        public string name;
        public long order_id;
        public OriginAddress origin_address;
        public Receipt receipt;
        public string service;
        public object shipment_status;
        public string status;
        public string tracking_company;
        public string tracking_number;
        public List<string> tracking_numbers;
        public string tracking_url;
        public List<string> tracking_urls;
        public string updated_at;
        public List<LineItem> line_items;
    }

    public class ShippingAddress
    {
        public string first_name;
        public string address1;
        public string phone;
        public string city;
        public string zip;
        public string province;
        public string country;
        public string last_name;
        public object address2;
        public object company;
        public string latitude;
        public string longitude;
        public string name;
        public string country_code;
        public string province_code;
    }

    public class DiscountedPriceSet
    {
        public ShopMoney shop_money;
        public PresentmentMoney presentment_money;
    }

    public class ShippingLine
    {
        public long id;
        public object carrier_identifier;
        public string code;
        public object delivery_category;
        public string discounted_price;
        public DiscountedPriceSet discounted_price_set;
        public object phone;
        public string price;
        public PriceSet price_set;
        public object requested_fulfillment_service_id;
        public string source;
        public string title;
        public List<object> tax_lines;
        public List<object> discount_allocations;
    }

    public class Order
    {
        public long id;
        public string admin_graphql_api_id;
        public string app_id;
        public object browser_ip;
        public bool buyer_accepts_marketing;
        public object cancel_reason;
        public object cancelled_at;
        public object cart_token;
        public object checkout_id;
        public object checkout_token;
        public string closed_at;
        public bool confirmed;
        public string contact_email;
        public string created_at;
        public string currency;
        public string current_subtotal_price;
        public CurrentSubtotalPriceSet current_subtotal_price_set;
        public string current_total_discounts;
        public CurrentTotalDiscountsSet current_total_discounts_set;
        public object current_total_duties_set;
        public string current_total_price;
        public CurrentTotalPriceSet current_total_price_set;
        public string current_total_tax;
        public CurrentTotalTaxSet current_total_tax_set;
        public object customer_locale;
        public object device_id;
        public List<object> discount_codes;
        public string email;
        public bool estimated_taxes;
        public string financial_status;
        public string fulfillment_status;
        public string gateway;
        public object landing_site;
        public object landing_site_ref;
        public object location_id;
        public string name;
        public object note;
        public List<object> note_attributes;
        public string number;
        public string order_number;
        public string order_status_url;
        public object original_total_duties_set;
        public List<object> payment_gateway_names;
        public object phone;
        public string presentment_currency;
        public string processed_at;
        public string processing_method;
        public object reference;
        public object referring_site;
        public object source_identifier;
        public string source_name;
        public object source_url;
        public string subtotal_price;
        public SubtotalPriceSet subtotal_price_set;
        public string tags;
        public List<object> tax_lines;
        public bool taxes_included;
        public bool test;
        public string token;
        public string total_discounts;
        public TotalDiscountsSet total_discounts_set;
        public string total_line_items_price;
        public TotalLineItemsPriceSet total_line_items_price_set;
        public string total_outstanding;
        public string total_price;
        public TotalPriceSet total_price_set;
        public string total_price_usd;
        public TotalShippingPriceSet total_shipping_price_set;
        public string total_tax;
        public TotalTaxSet total_tax_set;
        public string total_tip_received;
        public string total_weight;
        public string updated_at;
        public object user_id;
        public BillingAddress billing_address;
        public Customer customer;
        public List<object> discount_applications;
        public List<Fulfillment> fulfillments;
        public List<LineItem> line_items;
        public object payment_terms;
        public List<object> refunds;
        public ShippingAddress shipping_address;
        public List<ShippingLine> shipping_lines;
    }
}
