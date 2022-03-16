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
    public class LUMMRPPreferenceMaint : PXGraph<LUMMRPPreferenceMaint>
    {
        public PXSave<LUMMRPPreference> Save;
        public PXCancel<LUMMRPPreference> Cancel;
        public SelectFrom<LUMMRPPreference>.View Preference;
    }
}
