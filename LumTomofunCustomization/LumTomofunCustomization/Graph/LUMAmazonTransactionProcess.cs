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
    public class LUMAmazonTransactionProcess : PXGraph<LUMAmazonTransactionProcess>
    {
        public PXSave<LUMAmazonTransData> Save;
        public PXCancel<LUMAmazonTransData> Cancel;
        public SelectFrom<LUMAmazonTransData>.View AmazonTransaction;
    }
}
