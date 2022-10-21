using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Objects.CA
{
    public class CATranEnqExt : PXGraphExtension<CATranEnq>
    {

        public virtual void _(Events.RowSelected<CATranEnq.CATranExt> e, PXRowSelected baseHandler)
        {
            baseHandler?.Invoke(e.Cache, e.Args);
            var row = e.Row;
            if (row != null && string.IsNullOrEmpty(row.ExtRefNbr))
            {
                var mappingData = SelectFrom<CATranEnq.CATranExt>
                                 .Where<CATranEnq.CATranExt.origRefNbr.IsEqual<P.AsString>
                                   .And<CATranEnq.CATranExt.batchNbr.IsEqual<P.AsString>>
                                   .And<CATranEnq.CATranExt.extRefNbr.IsNotNull>>
                                 .View.Select(Base, row.OrigRefNbr, row.BatchNbr).TopFirst;
                if (mappingData != null)
                    row.ExtRefNbr = mappingData?.ExtRefNbr;
            }
        }

    }
}
