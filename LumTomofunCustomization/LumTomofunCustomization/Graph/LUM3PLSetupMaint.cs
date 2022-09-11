using PX.Data;
using LUMTomofunCustomization.DAC;

namespace LUMTomofunCustomization.Graph
{
    public class LUM3PLSetupMaint : PXGraph<LUM3PLSetupMaint>
    {
        public PXSave<LUM3PLSetup> Save;
        public PXCancel<LUM3PLSetup> Cancel;
        public PXSelect<LUM3PLSetup> Setup;
    }
}