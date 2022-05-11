using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LUMLocalization.DAC;
using PX.Common;
using PX.Data;
using PX.Data.Update;
using PX.Objects.AP;
using PX.Objects.EP;
using PX.Objects.IN;
using PX.Objects.PO;

namespace PX.Objects.SO
{
    public class SOOrderEntry_Extension : PXGraphExtension<SOOrderEntry>
    {
        #region Event Handlers
        [Obsolete]
        protected virtual void SOOrder_RowDeleted(PXCache cache, PXRowDeletedEventArgs e, PXRowDeleted InvokeBaseHandler)
        //protected void _(Events.RowDeleted<SOOrder> e)
        {
            InvokeBaseHandler?.Invoke(cache, e);
            var row = e.Row as SOOrder;
            if (row == null) return;

            var curSOOrder = (SOOrder)Base.Caches[typeof(SOOrder)].Current;
            SOOrderExt sOOrderExt = curSOOrder.GetExtension<SOOrderExt>();

            if ((bool)sOOrderExt.UsrICPOCreated)
            {
                var curSOLines = (SOLine)Base.Caches[typeof(SOLine)].Current;
                var curUserName = PXLogin.ExtractUsername(PXContext.PXIdentity.IdentityName);
                //var curLMICVendor = PXSelect<LMICVendor, Where<LMICVendor.vendorid, Equal<Required<LMICVendor.vendorid>>>>.Select(Base, curPOOrder.VendorID)?.TopFirst;
                var curLMICCustomer = PXSelect<LMICCustomer, Where<LMICCustomer.customerID, Equal<Required<LMICCustomer.customerID>>>>.Select(Base, curSOOrder.CustomerID).TopFirst;

                if (curLMICCustomer != null)
                {
                    using (PXLoginScope pXLoginScope = new PXLoginScope($"{curUserName}@{curLMICCustomer.LoginName}"))
                    {
                        // set SOorder.ICPOCreated = False, SOOrder.Customerordernbr = Blank
                        POOrderEntry pOOrderEntry = PXGraph.CreateInstance<POOrderEntry>();
                        POOrder pOOrder = PXSelect<POOrder, Where<POOrder.orderNbr, Equal<Required<POOrder.orderNbr>>>>.Select(pOOrderEntry, row.CustomerOrderNbr)?.TopFirst;
                        pOOrder.VendorRefNbr = "IC SO had been deleted";
                        POOrderExt pOOrderExt = pOOrder.GetExtension<POOrderExt>();
                        pOOrderExt.UsrICSOCreated = false;
                        pOOrder = pOOrderEntry.Document.Update(pOOrder);
                        pOOrderEntry.Actions.PressSave();
                    }
                    Base.Document.Delete(curSOOrder);
                    Base.Transactions.Delete(curSOLines);
                    Base.Actions.PressSave();
                }
            }
        }
        #endregion
    }
}
