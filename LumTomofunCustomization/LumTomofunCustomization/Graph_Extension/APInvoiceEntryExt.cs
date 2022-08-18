using PX.Data;
using PX.Objects.AP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.Graph_Extension
{
    public class APInvoiceEntryExt : PXGraphExtension<APInvoiceEntry>
    {
        public PXAction<APInvoice> printAPForm;
        [PXButton(CommitChanges = true, DisplayOnMainToolbar = false, Category = "Reports"), PXUIField(DisplayName = "AP Bill Application Form", MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable PrintAPForm(PXAdapter adapter)
        {
            if(Base.Document.Current != null)
            {
                var _reportID = "LM610500";
                var doc = Base.Document.Current;
                object masterPeriodID;
                string docMasterPeriod = ((masterPeriodID = Base.Caches[typeof(APRegister)].GetValueExt<APRegister.tranPeriodID>(doc)) is PXFieldState) ? ((string)((PXFieldState)masterPeriodID).Value) : ((string)masterPeriodID);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["PeriodFrom"] = docMasterPeriod;
                parameters["PeriodTo"] = docMasterPeriod;
                parameters["OrgBAccountID"] = PXAccess.GetBranchCD(doc.BranchID);
                parameters["DocType"] = doc.DocType;
                parameters["RefNbr"] = doc.RefNbr;
                throw new PXReportRequiredException(parameters, _reportID, $"Report {_reportID}") { Mode = PXBaseRedirectException.WindowMode.New };
            }
            return adapter.Get();
        }

        [PXButton(CommitChanges = true), PXUIField(DisplayName = "AP Bill Application Form (Release)", MapEnableRights = PXCacheRights.Select)]
        public virtual IEnumerable PrintAPRegister(PXAdapter adapter, string reportID = null) => Base.PrintAPRegister(adapter,reportID);
    }
}
