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
    public class LUMMarketplaceFeePreferenceMaint : PXGraph<LUMMarketplaceFeePreferenceMaint>
    {
        public PXSave<LUMMarketplaceFeePreference> Save;
        public PXCancel<LUMMarketplaceFeePreference> Cancel;
        public SelectFrom<LUMMarketplaceFeePreference>.View Transactions;
    }
}
