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
    public class LUMMWSSetup : PXGraph<LUMMWSSetup>
    {
        public PXCancel<LUMMWSPreference> Cancel;
        public PXSave<LUMMWSPreference> Save;
        public SelectFrom<LUMMWSPreference>.View Setup;
    }
}
