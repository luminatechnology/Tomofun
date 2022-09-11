using PX.Data;
using LUMTomofunCustomization.DAC;

namespace LUMTomofunCustomization.Graph
{
    public class LUM3PLWHMappingMaint : PXGraph<LUM3PLWHMappingMaint>
    {
        public PXSave<LUM3PLWarehouseMapping> Save;
        public PXCancel<LUM3PLWarehouseMapping> Cancel;

        [PXImport(typeof(LUM3PLWarehouseMapping))]
        public PXSelect<LUM3PLWarehouseMapping> ThirdPLWHMap;
    }
}