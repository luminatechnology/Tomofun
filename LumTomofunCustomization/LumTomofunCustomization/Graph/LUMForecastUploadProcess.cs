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
    public class LUMForecastUploadProcess : PXGraph<LUMForecastUploadProcess>, PXImportAttribute.IPXPrepareItems
    {

        public PXSave<LUMForecastUpload> Save;
        public PXCancel<LUMForecastUpload> Cancel;

        [PXImport(typeof(LUMForecastUpload))]
        public SelectFrom<LUMForecastUpload>.View Transaction;

        public bool PrepareImportRow(string viewName, IDictionary keys, IDictionary values)
            => true;

        public void PrepareItems(string viewName, IEnumerable items) { }

        public bool RowImported(string viewName, object row, object oldRow)
            => ((row as LUMForecastUpload)?.Qty ?? 0) == 0 ? false : true;

        public bool RowImporting(string viewName, object row)
            => true;
    }
}
