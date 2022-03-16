using LumTomofunCustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.Graph
{
    public class LumMRPDataUploadProcess : PXGraph<LumMRPDataUploadProcess>, PXImportAttribute.IPXPrepareItems
    {

        public PXSave<LumMRPData> Save;
        public PXCancel<LumMRPData> Cancel;

        [PXImport(typeof(LumMRPData))]
        public SelectFrom<LumMRPData>.View Transaction;

        public bool PrepareImportRow(string viewName, IDictionary keys, IDictionary values)
            => true;

        public void PrepareItems(string viewName, IEnumerable items) { }

        public bool RowImported(string viewName, object row, object oldRow)
            => true;

        public bool RowImporting(string viewName, object row)
            => true;
    }
}
