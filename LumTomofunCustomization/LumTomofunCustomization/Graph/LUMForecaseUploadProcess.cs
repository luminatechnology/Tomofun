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
    public class LUMForecaseUploadProcess : PXGraph<LUMForecaseUploadProcess>, PXImportAttribute.IPXPrepareItems
    {

        public PXSave<LUMForecaseUpload> Save;
        public PXCancel<LUMForecaseUpload> Cancel;

        [PXImport(typeof(LUMForecaseUpload))]
        public SelectFrom<LUMForecaseUpload>.View Transaction;

        public bool PrepareImportRow(string viewName, IDictionary keys, IDictionary values)
            => true;

        public void PrepareItems(string viewName, IEnumerable items) { }

        public bool RowImported(string viewName, object row, object oldRow)
            => ((row as LUMForecaseUpload)?.Qty ?? 0) == 0 ? false : true;

        public bool RowImporting(string viewName, object row)
            => true;
    }
}
