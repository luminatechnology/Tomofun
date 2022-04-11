using LumTomofunCustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.Graph
{
    public class LUMShopifyMarketplacePreferenceMaint : PXGraph<LUMShopifyMarketplacePreferenceMaint>
    {
        public PXSave<LUMShopifyMarketplacePreference> Save;
        public PXCancel<LUMShopifyMarketplacePreference> Cancel;
        public SelectFrom<LUMShopifyMarketplacePreference>.View Transactions;
    }
}
