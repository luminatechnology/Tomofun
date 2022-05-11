using System;
using LUMLocalization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace LUMLocalization.Graph
{
    public class LMTFInvEntry : PXGraph<LMTFInvEntry>
    {
        public PXSave<LMTFInvQty> Save;
        public PXCancel<LMTFInvQty> Cancel;
        
        [PXImport(typeof(LMTFInvQty))] 
        public SelectFrom<LMTFInvQty>.View DetailsView;

    }
}