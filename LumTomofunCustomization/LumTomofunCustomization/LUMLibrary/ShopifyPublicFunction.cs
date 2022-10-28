using LumTomofunCustomization.DAC;
using LUMTomofunCustomization.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.IN;

namespace LumTomofunCustomization.LUMLibrary
{
    public static class ShopifyPublicFunction
    {
        /// <summary> 取Marketplace 對應 Customer ID </summary>
        public static int? GetMarketplaceCustomer(string marketPlace)
            => SelectFrom<LUMShopifyMarketplacePreference>
               .Where<LUMShopifyMarketplacePreference.marketplace.IsEqual<P.AsString>>
               .View.Select(new PXGraph(), marketPlace).TopFirst?.BAccountID;

        public static int? GetMarketplaceTimeZone(string marketplace)
            => SelectFrom<LUMShopifyMarketplacePreference>
               .Where<LUMShopifyMarketplacePreference.marketplace.IsEqual<P.AsString>>
               .View.Select(new PXGraph(), marketplace).TopFirst?.TimeZone ?? 0;

        /// <summary> 取Marketplace 對應 Tax Calculation </summary>
        public static bool GetMarketplaceTaxCalculation(string marketPlace)
            => SelectFrom<LUMShopifyMarketplacePreference>
               .Where<LUMShopifyMarketplacePreference.marketplace.IsEqual<P.AsString>>
               .View.Select(new PXGraph(), marketPlace).TopFirst?.IsTaxCalculation ?? false;

        /// <summary> 取Fee 對應 Non-Stock item ID </summary>
        public static int? GetFeeNonStockItem(string fee)
            => SelectFrom<LUMMarketplaceFeePreference>
               .Where<LUMMarketplaceFeePreference.fee.IsEqual<P.AsString>>
               .View.Select(new PXGraph(), fee).TopFirst?.InventoryID;

        /// <summary> 取Inventory Item ID </summary>
        public static int? GetInvetoryitemID(PXGraph graph, string sku)
            => InventoryItem.UK.Find(graph, sku)?.InventoryID ??
               SelectFrom<INItemXRef>
               .Where<INItemXRef.alternateID.IsEqual<P.AsString>>
               .View.SelectSingleBound(graph, null, sku).TopFirst?.InventoryID;

        public static int? GetSalesAcctID(PXGraph graph, string inventoryName, int? inventoryID, PX.Objects.SO.SOOrder mapShopifyOrder, int? customerID)
        {
            if (inventoryName.ToUpper().Contains("REFUND") && mapShopifyOrder.Status == PX.Objects.SO.SOOrderStatus.Open)
                return PX.Objects.AR.Customer.PK.Find(graph, customerID)?.PrepaymentAcctID;
            else if (inventoryName.ToUpper().Contains("REFUND") && mapShopifyOrder.Status != PX.Objects.SO.SOOrderStatus.Open)
                return null;
            else if (inventoryName.ToUpper().Contains("EC-WHTAX") && mapShopifyOrder.Status == PX.Objects.SO.SOOrderStatus.Open)
                return PX.Objects.AR.Customer.PK.Find(graph, customerID)?.PrepaymentAcctID;
            else if (inventoryName.ToUpper().Contains("EC-WHTAX") && mapShopifyOrder.Status != PX.Objects.SO.SOOrderStatus.Open)
                return InventoryItem.PK.Find(graph, inventoryID)?.SalesAcctID;
            else
                return InventoryItem.PK.Find(graph, inventoryID)?.SalesAcctID;
        }

        public static int? GetSalesSubAcctID(PXGraph graph, string inventoryName, int? inventoryID, PX.Objects.SO.SOOrder mapShopifyOrder, int? customerID)
        {

            if (inventoryName.ToUpper().Contains("REFUND") && mapShopifyOrder.Status == PX.Objects.SO.SOOrderStatus.Open)
                return PX.Objects.AR.Customer.PK.Find(graph, customerID)?.PrepaymentSubID;
            else if (inventoryName.ToUpper().Contains("REFUND") && mapShopifyOrder.Status != PX.Objects.SO.SOOrderStatus.Open)
                return null;
            else if (inventoryName.ToUpper().Contains("EC-WHTAX") && mapShopifyOrder.Status == PX.Objects.SO.SOOrderStatus.Open)
                return PX.Objects.AR.Customer.PK.Find(graph, customerID)?.PrepaymentSubID;
            else if (inventoryName.ToUpper().Contains("EC-WHTAX") && mapShopifyOrder.Status != PX.Objects.SO.SOOrderStatus.Open)
                return InventoryItem.PK.Find(graph, inventoryID)?.SalesSubID;
            else
                return InventoryItem.PK.Find(graph, inventoryID)?.SalesSubID;
        }
    }
}
