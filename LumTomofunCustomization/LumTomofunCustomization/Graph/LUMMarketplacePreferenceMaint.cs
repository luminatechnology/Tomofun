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
    public class LUMMarketplacePreferenceMaint : PXGraph<LUMMarketplacePreferenceMaint>
    {
        public PXSave<LUMMarketplacePreference> Save;
        public PXCancel<LUMMarketplacePreference> Cancel;
        public SelectFrom<LUMMarketplacePreference>.View Transactions;
    }
}
