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
        public PXSave<UploadForecastFilter> Save;
        public PXCancel<UploadForecastFilter> Cancel;

        public PXFilter<UploadForecastFilter> Filter;

        [PXImport(typeof(UploadForecastFilter))]
        public SelectFrom<LUMForecastUpload>.View Transaction;

        public IEnumerable transaction()
        {
            var filter = this.Filter.Current;
            var newBql = new SelectFrom<LUMForecastUpload>
                             .InnerJoin<NoteDoc>.On<LUMForecastUpload.noteid.IsEqual<NoteDoc.noteID>>.View(this);
            PXView select = (filter.WithAttachment ?? false) ?
                            new PXView(this, false, newBql.View.BqlSelect) :
                            new PXView(this, false, Transaction.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;
            return select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters, ref startrow, PXView.MaximumRows, ref totalrow);
        }

        public bool PrepareImportRow(string viewName, IDictionary keys, IDictionary values)
            => true;

        public void PrepareItems(string viewName, IEnumerable items) { }

        public bool RowImported(string viewName, object row, object oldRow)
            => ((row as LUMForecastUpload)?.Qty ?? 0) == 0 ? false : true;

        public bool RowImporting(string viewName, object row)
            => true;
    }

    [Serializable]
    public class UploadForecastFilter : IBqlTable
    {
        [PXBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "With Attachment")]
        public virtual bool? WithAttachment { get; set; }
        public abstract class withAttachment : PX.Data.BQL.BqlBool.Field<withAttachment> { }
    }
}
