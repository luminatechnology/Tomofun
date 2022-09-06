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
    }
}
