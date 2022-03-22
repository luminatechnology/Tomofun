using LUMTomofunCustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.Graph
{
    public class LUMShopifyInterfaceMaint : PXGraph<LUMShopifyInterfaceMaint>
    {
        public PXSave<LUMShopifySourceData> Save;
        public PXCancel<LUMShopifySourceData> Cancel;
        public SelectFrom<LUMShopifySourceData>.View ShopifySourceData;
    }
}
