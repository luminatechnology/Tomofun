using System;
using System.Collections.Generic;

namespace LumTomofunCustomization.API_Entity
{
    public class ReturnHelperEntity
    {
        public class CountryRoot
        {
            public string correlationId { get; set; }
            public Meta meta { get; set; }
            public List<Country> countries { get; set; }
        }

        public class WarehouseRoot
        {
            public string correlationId { get; set; }
            public int totalNumberOfRecords { get; set; }
            public Meta meta { get; set; }
            public List<Warehouse> warehouses { get; set; }
        }

        public class ReturnInvtRoot
        {
            public string correlationId { get; set; }
            public int totalNumberOfRecords { get; set; }
            public Meta meta { get; set; }
            public List<ReturnInventoryList> returnInventoryList { get; set; }
        }

        public class Data { }

        public class Error { }

        public class Meta
        {
            public int status { get; set; }
            public Data data { get; set; }
            public object errorCode { get; set; }
            public Error error { get; set; }
        }

        public class Country
        {
            public string countryCode { get; set; }
            public string countryName { get; set; }
        }

        public class Warehouse
        {
            public object correlationId { get; set; }
            public object meta { get; set; }
            public int warehouseId { get; set; }
            public string countryCode { get; set; }
            public string contactName { get; set; }
            public string companyName { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public object fax { get; set; }
            public string street1 { get; set; }
            public object street2 { get; set; }
            public object street3 { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string postalCode { get; set; }
            public string addressType { get; set; }
            public string description { get; set; }
        }

        public class ReturnInventoryList
        {
            public string apiName { get; set; }
            public string returnRequestNumber { get; set; }
            public string shipmentNumber { get; set; }
            public int vasCount { get; set; }
            public string returnRequestRemarks { get; set; }
            public int shipmentId { get; set; }
            public double actualWeight { get; set; }
            public double cbm { get; set; }
            public bool isFraudulent { get; set; }
            public string fraudReasonCode { get; set; }
            public int returnInventoryId { get; set; }
            public int warehouseId { get; set; }
            public double stockAge { get; set; }
            public int returnRequestLineItemId { get; set; }
            public int returnRequestId { get; set; }
            public string returnRequestLineItemNumber { get; set; }
            public string description { get; set; }
            public double weight { get; set; }
            public string weightUom { get; set; }
            public string valueCurrencyCode { get; set; }
            public double value { get; set; }
            public string handlingCode { get; set; }
            public string handlingStatusCode { get; set; }
            public object returnRequestLineItemImages { get; set; }
            public object returnRequestLineItemVasList { get; set; }
            public object returnRequestLineItemSupplement { get; set; }
            public object requestLineItemPayload { get; set; }
            public string warehouseRemarks { get; set; }
            public string handlingUpdatedOnStr { get; set; }
            public string sku { get; set; }
            public string itemRma { get; set; }
            public DateTime modifyOn { get; set; }
            public string modifyBy { get; set; }
            public string modifyOnStr { get; set; }
            public DateTime createOn { get; set; }
            public string createBy { get; set; }
            public string createOnStr { get; set; }
        }
    }
}
