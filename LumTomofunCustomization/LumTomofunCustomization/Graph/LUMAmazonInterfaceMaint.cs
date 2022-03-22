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
    public class LUMAmazonInterfaceMaint : PXGraph<LUMAmazonInterfaceMaint>
    {
        public PXSave<LUMAmazonSourceData> Save;
        public PXCancel<LUMAmazonSourceData> Cancel;

        public SelectFrom<LUMAmazonSourceData>.View AmazonSourceData;
    }
}
