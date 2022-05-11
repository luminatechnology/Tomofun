using System;
using LUMLocalization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.Update;
using PX.Objects.AP;
using PX.SM;

namespace LUMLocalization.Graph
{
    public class LMICVendorMaint : PXGraph<LMICVendorMaint>
    {

        public PXSave<LMICVendor> Save;
        public PXCancel<LMICVendor> Cancel;

        public SelectFrom<LMICVendor>.View DetailsView;

        #region Event Handlers

        #region Auto-fill LoginName
        protected void LMICVendor_TenantID_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
            var row = (LMICVendor)e.Row;
            if (row == null || row.TenantID == null) return;

            foreach (UPCompany info in PXCompanyHelper.SelectCompanies())
            {
                if (info.CompanyID == row.TenantID)
                {
                    row.LoginName = info.LoginName;
                    break;
                }
            }
        }
        #endregion

        #region Auto-fill VendorName
        protected void LMICVendor_VendorID_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
            if (e.Row == null) return;

            var row = (LMICVendor)e.Row;
            var vendor = PXSelect<Vendor, Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>.Select(this, row.VendorID);
            row.VendorName = vendor.TopFirst.AcctName;
        }
        #endregion

        #endregion

    }
}