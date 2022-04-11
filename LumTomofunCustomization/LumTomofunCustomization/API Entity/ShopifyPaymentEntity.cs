using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.API_Entity.ShopifyPayment
{
    public class ShopMoney
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class PresentmentMoney
    {
        public string amount { get; set; }
        public string currency_code { get; set; }
    }

    public class CurrentSubtotalPriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class CurrentTotalDiscountsSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class CurrentTotalPriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class CurrentTotalTaxSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class DiscountCode
    {
        public string code { get; set; }
        public string amount { get; set; }
        public string type { get; set; }
    }

    public class SubtotalPriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class PriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class TaxLine
    {
        public string price { get; set; }
        public double rate { get; set; }
        public string title { get; set; }
        public PriceSet price_set { get; set; }
        public bool? channel_liable { get; set; }
    }

    public class TotalDiscountsSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class TotalLineItemsPriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class TotalPriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class TotalShippingPriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class TotalTaxSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class BillingAddress
    {
        public string first_name { get; set; }
        public string address1 { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string last_name { get; set; }
        public string address2 { get; set; }
        public string company { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
    }

    public class DefaultAddress
    {
        public object id { get; set; }
        public object customer_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public string province_code { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public bool @default { get; set; }
    }

    public class Customer
    {
        public object id { get; set; }
        public string email { get; set; }
        public bool accepts_marketing { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string orders_count { get; set; }
        public string state { get; set; }
        public string total_spent { get; set; }
        public object last_order_id { get; set; }
        public object note { get; set; }
        public bool verified_email { get; set; }
        public object multipass_identifier { get; set; }
        public bool tax_exempt { get; set; }
        public object phone { get; set; }
        public string tags { get; set; }
        public string last_order_name { get; set; }
        public string currency { get; set; }
        public string accepts_marketing_updated_at { get; set; }
        public string marketing_opt_in_level { get; set; }
        public List<object> tax_exemptions { get; set; }
        public object sms_marketing_consent { get; set; }
        public string admin_graphql_api_id { get; set; }
        public DefaultAddress default_address { get; set; }
    }

    public class DiscountApplication
    {
        public string target_type { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string value_type { get; set; }
        public string allocation_method { get; set; }
        public string target_selection { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

    public class OriginAddress
    {
    }

    public class Receipt
    {
        public string response_status { get; set; }
        public string response_comment { get; set; }
        public string id { get; set; }
        public string amount { get; set; }
        public BalanceTransaction balance_transaction { get; set; }
        public Charge charge { get; set; }
        public string @object { get; set; }
        public object reason { get; set; }
        public string status { get; set; }
        public string created { get; set; }
        public string currency { get; set; }
        public Metadata metadata { get; set; }
        public PaymentMethodDetails payment_method_details { get; set; }
        public MitParams mit_params { get; set; }
        public DateTime? timestamp { get; set; }
        public string ack { get; set; }
        public string correlation_id { get; set; }
        public string version { get; set; }
        public string build { get; set; }
        public string refund_transaction_id { get; set; }
        public string net_refund_amount { get; set; }
        public string net_refund_amount_currency_id { get; set; }
        public string fee_refund_amount { get; set; }
        public string fee_refund_amount_currency_id { get; set; }
        public string gross_refund_amount { get; set; }
        public string gross_refund_amount_currency_id { get; set; }
        public string total_refunded_amount { get; set; }
        public string total_refunded_amount_currency_id { get; set; }
        public string refund_status { get; set; }
        public string pending_reason { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Ack { get; set; }
        public string CorrelationID { get; set; }
        public string Version { get; set; }
        public string Build { get; set; }
        public string RefundTransactionID { get; set; }
        public string NetRefundAmount { get; set; }
        public string FeeRefundAmount { get; set; }
        public string GrossRefundAmount { get; set; }
        public string TotalRefundedAmount { get; set; }
        public RefundInfo RefundInfo { get; set; }
        public string amount_capturable { get; set; }
        public string amount_received { get; set; }
        public object canceled_at { get; set; }
        public object cancellation_reason { get; set; }
        public string capture_method { get; set; }
        public Charges charges { get; set; }
        public string confirmation_method { get; set; }
        public object last_payment_error { get; set; }
        public bool livemode { get; set; }
        public object next_action { get; set; }
        public string payment_method { get; set; }
        public List<string> payment_method_types { get; set; }
        public object source { get; set; }
        public Error error { get; set; }
        public string error_code { get; set; }
        public object authorization_expiration { get; set; }
        public TransactionEvent transaction_event { get; set; }
        public string bin { get; set; }
        public AuthorizeResult AuthorizeResult { get; set; }
        public ResponseMetadata ResponseMetadata { get; set; }
        public string token { get; set; }
        public string transaction_id { get; set; }
        public object parent_transaction_id { get; set; }
        public object receipt_id { get; set; }
        public string transaction_type { get; set; }
        public string payment_type { get; set; }
        public DateTime? payment_date { get; set; }
        public string gross_amount { get; set; }
        public string gross_amount_currency_id { get; set; }
        public string fee_amount { get; set; }
        public string fee_amount_currency_id { get; set; }
        public string tax_amount { get; set; }
        public string tax_amount_currency_id { get; set; }
        public object exchange_rate { get; set; }
        public string payment_status { get; set; }
        public string reason_code { get; set; }
        public string protection_eligibility { get; set; }
        public string protection_eligibility_type { get; set; }
        public string pay_pal_account_id { get; set; }
        public string secure_merchant_account_id { get; set; }
        public string success_page_redirect_requested { get; set; }
        public object coupled_payment_info { get; set; }
        public string Token { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
        public string SuccessPageRedirectRequested { get; set; }
        public object CoupledPaymentInfo { get; set; }
    }

    public class PreTaxPriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class TotalDiscountSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class AmountSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class DiscountAllocation
    {
        public string amount { get; set; }
        public AmountSet amount_set { get; set; }
        public string discount_application_index { get; set; }
    }

    public class OriginLocation
    {
        public string id { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
    }

    public class DestinationLocation
    {
        public object id { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
    }

    public class LineItem
    {
        public object id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public string fulfillable_quantity { get; set; }
        public string fulfillment_service { get; set; }
        public string fulfillment_status { get; set; }
        public bool gift_card { get; set; }
        public string grams { get; set; }
        public string name { get; set; }
        public string pre_tax_price { get; set; }
        public PreTaxPriceSet pre_tax_price_set { get; set; }
        public string price { get; set; }
        public PriceSet price_set { get; set; }
        public bool product_exists { get; set; }
        public object product_id { get; set; }
        public List<object> properties { get; set; }
        public string quantity { get; set; }
        public bool requires_shipping { get; set; }
        public string sku { get; set; }
        public bool taxable { get; set; }
        public string title { get; set; }
        public string total_discount { get; set; }
        public TotalDiscountSet total_discount_set { get; set; }
        public object variant_id { get; set; }
        public string variant_inventory_management { get; set; }
        public string variant_title { get; set; }
        public string vendor { get; set; }
        public List<TaxLine> tax_lines { get; set; }
        public List<object> duties { get; set; }
        public List<DiscountAllocation> discount_allocations { get; set; }
        public OriginLocation origin_location { get; set; }
        public DestinationLocation destination_location { get; set; }
    }

    public class Fulfillment
    {
        public object id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public string created_at { get; set; }
        public object location_id { get; set; }
        public string name { get; set; }
        public object order_id { get; set; }
        public OriginAddress origin_address { get; set; }
        public Receipt receipt { get; set; }
        public string service { get; set; }
        public string shipment_status { get; set; }
        public string status { get; set; }
        public string tracking_company { get; set; }
        public string tracking_number { get; set; }
        public List<string> tracking_numbers { get; set; }
        public string tracking_url { get; set; }
        public List<string> tracking_urls { get; set; }
        public string updated_at { get; set; }
        public List<LineItem> line_items { get; set; }
    }

    public class TotalDutiesSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class PaymentsRefundAttributes
    {
        public string status { get; set; }
        public string acquirer_reference_number { get; set; }
    }

    public class BalanceTransaction
    {
        public string id { get; set; }
        public string @object { get; set; }
        public object exchange_rate { get; set; }
    }

    public class FraudDetails
    {
    }

    public class Metadata
    {
        public string shop_id { get; set; }
        public string shop_name { get; set; }
        public string transaction_fee_total_amount { get; set; }
        public string transaction_fee_tax_amount { get; set; }
        public string payments_charge_id { get; set; }
        public string order_transaction_id { get; set; }
        public string manual_entry { get; set; }
        public string order_id { get; set; }
        public string email { get; set; }
        public string payments_refund_id { get; set; }
    }

    public class Outcome
    {
        public string network_status { get; set; }
        public object reason { get; set; }
        public string risk_level { get; set; }
        public string seller_message { get; set; }
        public string type { get; set; }
        public int? risk_score { get; set; }
    }

    public class Checks
    {
        public string address_line1_check { get; set; }
        public string address_postal_code_check { get; set; }
        public object cvc_check { get; set; }
    }

    public class ApplePay
    {
    }

    public class Wallet
    {
        public ApplePay apple_pay { get; set; }
        public string dynamic_last4 { get; set; }
        public string type { get; set; }
    }

    public class Card
    {
        public int? amount_authorized { get; set; }
        public string brand { get; set; }
        public Checks checks { get; set; }
        public string country { get; set; }
        public string description { get; set; }
        public object ds_transaction_id { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public string fingerprstring { get; set; }
        public string funding { get; set; }
        public string iin { get; set; }
        public object installments { get; set; }
        public string issuer { get; set; }
        public string last4 { get; set; }
        public object moto { get; set; }
        public string network { get; set; }
        public object network_token { get; set; }
        public string network_transaction_id { get; set; }
        public object three_d_secure { get; set; }
        public Wallet wallet { get; set; }
        public object acquirer_reference_number { get; set; }
        public string acquirer_reference_number_status { get; set; }
    }

    public class PaymentMethodDetails
    {
        public Card card { get; set; }
        public string type { get; set; }
    }

    public class MitParams
    {
        public string network_transaction_id { get; set; }
    }

    public class Charge
    {
        public string id { get; set; }
        public string @object { get; set; }
        public string amount { get; set; }
        public string application_fee { get; set; }
        public string balance_transaction { get; set; }
        public bool captured { get; set; }
        public string created { get; set; }
        public string currency { get; set; }
        public object failure_code { get; set; }
        public object failure_message { get; set; }
        public FraudDetails fraud_details { get; set; }
        public bool livemode { get; set; }
        public Metadata metadata { get; set; }
        public Outcome outcome { get; set; }
        public bool paid { get; set; }
        public string payment_intent { get; set; }
        public string payment_method { get; set; }
        public PaymentMethodDetails payment_method_details { get; set; }
        public bool refunded { get; set; }
        public object source { get; set; }
        public string status { get; set; }
        public MitParams mit_params { get; set; }
    }

    public class RefundInfo
    {
        public string RefundStatus { get; set; }
        public string PendingReason { get; set; }
    }

    public class PaymentDetails
    {
        public string credit_card_bin { get; set; }
        public string avs_result_code { get; set; }
        public object cvv_result_code { get; set; }
        public string credit_card_number { get; set; }
        public string credit_card_company { get; set; }
    }

    public class Transaction
    {
        public object id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public string amount { get; set; }
        public string authorization { get; set; }
        public string created_at { get; set; }
        public string currency { get; set; }
        public object device_id { get; set; }
        public object error_code { get; set; }
        public string gateway { get; set; }
        public string kind { get; set; }
        public object location_id { get; set; }
        public string message { get; set; }
        public object order_id { get; set; }
        public object parent_id { get; set; }
        public PaymentsRefundAttributes payments_refund_attributes { get; set; }
        public string processed_at { get; set; }
        public Receipt receipt { get; set; }
        public string source_name { get; set; }
        public string status { get; set; }
        public bool test { get; set; }
        public object user_id { get; set; }
        public PaymentDetails payment_details { get; set; }
    }

    public class SubtotalSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class LineItem3
    {
        public object id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public string fulfillable_quantity { get; set; }
        public string fulfillment_service { get; set; }
        public string fulfillment_status { get; set; }
        public bool gift_card { get; set; }
        public string grams { get; set; }
        public string name { get; set; }
        public OriginLocation origin_location { get; set; }
        public string pre_tax_price { get; set; }
        public PreTaxPriceSet pre_tax_price_set { get; set; }
        public string price { get; set; }
        public PriceSet price_set { get; set; }
        public bool product_exists { get; set; }
        public object product_id { get; set; }
        public List<object> properties { get; set; }
        public string quantity { get; set; }
        public bool requires_shipping { get; set; }
        public string sku { get; set; }
        public bool taxable { get; set; }
        public string title { get; set; }
        public string total_discount { get; set; }
        public TotalDiscountSet total_discount_set { get; set; }
        public object variant_id { get; set; }
        public string variant_inventory_management { get; set; }
        public string variant_title { get; set; }
        public string vendor { get; set; }
        public List<TaxLine> tax_lines { get; set; }
        public List<object> duties { get; set; }
        public List<DiscountAllocation> discount_allocations { get; set; }
        public DestinationLocation destination_location { get; set; }
    }

    public class RefundLineItem
    {
        public object id { get; set; }
        public object line_item_id { get; set; }
        public object location_id { get; set; }
        public string quantity { get; set; }
        public string restock_type { get; set; }
        public double subtotal { get; set; }
        public SubtotalSet subtotal_set { get; set; }
        public double total_tax { get; set; }
        public TotalTaxSet total_tax_set { get; set; }
        public LineItem line_item { get; set; }
    }

    public class Refund
    {
        public object id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public string created_at { get; set; }
        public string note { get; set; }
        public object order_id { get; set; }
        public string processed_at { get; set; }
        public bool restock { get; set; }
        public TotalDutiesSet total_duties_set { get; set; }
        public object user_id { get; set; }
        public List<object> order_adjustments { get; set; }
        public List<Transaction> transactions { get; set; }
        public List<RefundLineItem> refund_line_items { get; set; }
        public List<object> duties { get; set; }
    }

    public class ShippingAddress
    {
        public string first_name { get; set; }
        public string address1 { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string last_name { get; set; }
        public string address2 { get; set; }
        public string company { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string province_code { get; set; }
    }

    public class DiscountedPriceSet
    {
        public ShopMoney shop_money { get; set; }
        public PresentmentMoney presentment_money { get; set; }
    }

    public class ShippingLine
    {
        public object id { get; set; }
        public object carrier_identifier { get; set; }
        public string code { get; set; }
        public object delivery_category { get; set; }
        public string discounted_price { get; set; }
        public DiscountedPriceSet discounted_price_set { get; set; }
        public object phone { get; set; }
        public string price { get; set; }
        public PriceSet price_set { get; set; }
        public object requested_fulfillment_service_id { get; set; }
        public string source { get; set; }
        public string title { get; set; }
        public List<TaxLine> tax_lines { get; set; }
        public List<object> discount_allocations { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string @object { get; set; }
        public string amount { get; set; }
        public string application_fee { get; set; }
        public BalanceTransaction balance_transaction { get; set; }
        public bool captured { get; set; }
        public string created { get; set; }
        public string currency { get; set; }
        public object failure_code { get; set; }
        public object failure_message { get; set; }
        public FraudDetails fraud_details { get; set; }
        public bool livemode { get; set; }
        public Metadata metadata { get; set; }
        public Outcome outcome { get; set; }
        public bool paid { get; set; }
        public string payment_intent { get; set; }
        public string payment_method { get; set; }
        public PaymentMethodDetails payment_method_details { get; set; }
        public bool refunded { get; set; }
        public object source { get; set; }
        public string status { get; set; }
        public MitParams mit_params { get; set; }
    }

    public class Charges
    {
        public string @object { get; set; }
        public List<Datum> data { get; set; }
        public bool has_more { get; set; }
        public string total_count { get; set; }
        public string url { get; set; }
    }

    public class Error
    {
        public string charge { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string param { get; set; }
        public string type { get; set; }
        public string decline_code { get; set; }
    }

    public class TransactionEvent
    {
        public string created { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string fee { get; set; }
    }

    public class AuthorizationAmount
    {
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class CapturedAmount
    {
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class IdList
    {
        public string member { get; set; }
    }

    public class AuthorizationStatus
    {
        public string LastUpdateTimestamp { get; set; }
        public string ReasonCode { get; set; }
        public string State { get; set; }
    }

    public class AuthorizationFee
    {
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class AuthorizationDetails
    {
        public AuthorizationAmount AuthorizationAmount { get; set; }
        public CapturedAmount CapturedAmount { get; set; }
        public string SoftDescriptor { get; set; }
        public string ExpirationTimestamp { get; set; }
        public IdList IdList { get; set; }
        public string SoftDecline { get; set; }
        public AuthorizationStatus AuthorizationStatus { get; set; }
        public AuthorizationFee AuthorizationFee { get; set; }
        public string CaptureNow { get; set; }
        public object SellerAuthorizationNote { get; set; }
        public string CreationTimestamp { get; set; }
        public string AmazonAuthorizationId { get; set; }
        public string AuthorizationReferenceId { get; set; }
    }

    public class AuthorizeResult
    {
        public AuthorizationDetails AuthorizationDetails { get; set; }
    }

    public class ResponseMetadata
    {
        public string RequestId { get; set; }
    }

    public class SellerDetails
    {
        public string PayPalAccountID { get; set; }
        public string SecureMerchantAccountID { get; set; }
    }

    public class PaymentInfo
    {
        public string TransactionID { get; set; }
        public object ParentTransactionID { get; set; }
        public object ReceiptID { get; set; }
        public string TransactionType { get; set; }
        public string PaymentType { get; set; }
        public string PaymentDate { get; set; }
        public string GrossAmount { get; set; }
        public string FeeAmount { get; set; }
        public string TaxAmount { get; set; }
        public object ExchangeRate { get; set; }
        public string PaymentStatus { get; set; }
        public string PendingReason { get; set; }
        public string ReasonCode { get; set; }
        public string ProtectionEligibility { get; set; }
        public string ProtectionEligibilityType { get; set; }
        public SellerDetails SellerDetails { get; set; }
    }

    public class ClientDetails
    {
        public string accept_language { get; set; }
        public int? browser_height { get; set; }
        public string browser_ip { get; set; }
        public int? browser_width { get; set; }
        public object session_hash { get; set; }
        public string user_agent { get; set; }
    }

    public class ShopifyPaymentEntity
    {
        public object id { get; set; }
        public string admin_graphql_api_id { get; set; }
        public string app_id { get; set; }
        public string browser_ip { get; set; }
        public bool buyer_accepts_marketing { get; set; }
        public object cancel_reason { get; set; }
        public object cancelled_at { get; set; }
        public string cart_token { get; set; }
        public long? checkout_id { get; set; }
        public string checkout_token { get; set; }
        public string closed_at { get; set; }
        public bool confirmed { get; set; }
        public string contact_email { get; set; }
        public string created_at { get; set; }
        public string currency { get; set; }
        public string current_subtotal_price { get; set; }
        public CurrentSubtotalPriceSet current_subtotal_price_set { get; set; }
        public string current_total_discounts { get; set; }
        public CurrentTotalDiscountsSet current_total_discounts_set { get; set; }
        public object current_total_duties_set { get; set; }
        public string current_total_price { get; set; }
        public CurrentTotalPriceSet current_total_price_set { get; set; }
        public string current_total_tax { get; set; }
        public CurrentTotalTaxSet current_total_tax_set { get; set; }
        public string customer_locale { get; set; }
        public object device_id { get; set; }
        public List<DiscountCode> discount_codes { get; set; }
        public string email { get; set; }
        public bool estimated_taxes { get; set; }
        public string financial_status { get; set; }
        public string fulfillment_status { get; set; }
        public string gateway { get; set; }
        public string landing_site { get; set; }
        public object landing_site_ref { get; set; }
        public object location_id { get; set; }
        public string name { get; set; }
        public object note { get; set; }
        public List<object> note_attributes { get; set; }
        public string number { get; set; }
        public string order_number { get; set; }
        public string order_status_url { get; set; }
        public object original_total_duties_set { get; set; }
        public List<string> payment_gateway_names { get; set; }
        public string phone { get; set; }
        public string presentment_currency { get; set; }
        public string processed_at { get; set; }
        public string processing_method { get; set; }
        public object reference { get; set; }
        public string referring_site { get; set; }
        public object source_identifier { get; set; }
        public string source_name { get; set; }
        public object source_url { get; set; }
        public string subtotal_price { get; set; }
        public SubtotalPriceSet subtotal_price_set { get; set; }
        public string tags { get; set; }
        public List<TaxLine> tax_lines { get; set; }
        public bool taxes_included { get; set; }
        public bool test { get; set; }
        public string token { get; set; }
        public string total_discounts { get; set; }
        public TotalDiscountsSet total_discounts_set { get; set; }
        public string total_line_items_price { get; set; }
        public TotalLineItemsPriceSet total_line_items_price_set { get; set; }
        public string total_outstanding { get; set; }
        public string total_price { get; set; }
        public TotalPriceSet total_price_set { get; set; }
        public string total_price_usd { get; set; }
        public TotalShippingPriceSet total_shipping_price_set { get; set; }
        public string total_tax { get; set; }
        public TotalTaxSet total_tax_set { get; set; }
        public string total_tip_received { get; set; }
        public string total_weight { get; set; }
        public string updated_at { get; set; }
        public long? user_id { get; set; }
        public BillingAddress billing_address { get; set; }
        public Customer customer { get; set; }
        public List<DiscountApplication> discount_applications { get; set; }
        public List<Fulfillment> fulfillments { get; set; }
        public List<LineItem> line_items { get; set; }
        public object payment_terms { get; set; }
        public List<Refund> refunds { get; set; }
        public ShippingAddress shipping_address { get; set; }
        public List<ShippingLine> shipping_lines { get; set; }
        public List<Transaction> transactions { get; set; }
        public ClientDetails client_details { get; set; }
        public PaymentDetails payment_details { get; set; }
    }

}
