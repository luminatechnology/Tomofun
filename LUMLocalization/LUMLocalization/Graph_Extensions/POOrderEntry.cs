using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LUMLocalization.DAC;
using PX.Common;
using PX.Data;
using PX.Data.Update;
using PX.Objects.AR;
using PX.Objects.IN;
using PX.Objects.SO;

namespace PX.Objects.PO
{
    public class POOrderEntry_Extension : PXGraphExtension<POOrderEntry>
    {
        public override void Initialize()
        {
            base.Initialize();
            Base.action.AddMenuAction(CreateICSOAction);
        }

        #region Material Issues Action
        public PXAction<POOrder> CreateICSOAction;
        [PXButton]
        [PXUIField(DisplayName = "Create IC SO")]
        [Obsolete]
        protected void createICSOAction()
        {
            Base.Save.Press();

            var curCompanyID = PXInstanceHelper.CurrentCompany;
            var curUserName = PXLogin.ExtractUsername(PXContext.PXIdentity.IdentityName);

            //A.By using curLMICVendor table to identify the destination tenant of SO need to be created
            var curPOOrder = (POOrder)Base.Caches[typeof(POOrder)].Current;
            //var curLMICCustomer = PXSelect<LMICCustomer, Where<LMICCustomer.customerID, Equal<Required<LMICCustomer.customerID>>>>.Select(Base, curPOOrder.CustomerID).TopFirst;
            var curLMICVendor = PXSelect<LMICVendor, Where<LMICVendor.vendorid, Equal<Required<LMICVendor.vendorid>>>>.Select(Base, curPOOrder.VendorID).TopFirst;

            //B.Go to the destination tenant, and ensure vendor is defined in LMICVendor.If not, throw error message “The vendor is not defined in destination tenant”.
            var curCompanyName = PXLogin.ExtractCompany(PXContext.PXIdentity.IdentityName);
            var curLMICCustomer = new LMICCustomer();
            try
            {
                using (PXLoginScope pXLoginScope = new PXLoginScope($"{curUserName}@{curLMICVendor.LoginName}"))
                {
                    curLMICCustomer = PXSelect<LMICCustomer, Where<LMICCustomer.tenantID, Equal<Required<LMICCustomer.tenantID>>>>.Select(Base, curCompanyID).TopFirst;
                }
            }
            catch (Exception)
            {
                throw new PXException("Please check IC Customer setting.");
            }

            /*Get InventoryCD in each line*/
            var inventoryCDList = new List<string>();
            foreach (POLine line in Base.Transactions.Cache.Cached)
            {
                inventoryCDList.Add(PXSelect<InventoryItem, Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>.Select(Base, line.InventoryID).TopFirst?.InventoryCD);
            }

            if (curLMICVendor != null)
            {
                if (curLMICCustomer != null)
                {
                    try
                    {
                        PXLongOperation.StartOperation(Base, () =>
                        {
                            var POOrderVendorRefNbr = string.Empty;
                            
                            //C.Create a purchase order in destination tenant by uing the following mapping.
                            using (PXLoginScope pXLoginScope = new PXLoginScope($"{curUserName}@{curLMICVendor.LoginName}"))
                            {
                                SOOrderEntry sOOrderEntry = PXGraph.CreateInstance<SOOrderEntry>();

                                /* SO Header */
                                SOOrder sOOrder = sOOrderEntry.Document.Insert();

                                sOOrder.BranchID = curLMICCustomer.BranchID;
                                sOOrder.CustomerID = curLMICCustomer.CustomerID;
                                sOOrderEntry.Document.Cache.RaiseFieldUpdated<SOOrder.customerID>(sOOrder, null);
                                
                                sOOrder.CustomerOrderNbr = curPOOrder.OrderNbr;
                                sOOrder.OrderDate = curPOOrder.OrderDate;
                                sOOrder.OrderDesc = "IC SO | " + curCompanyName + " | " + curPOOrder.OrderNbr;

                                // Get Employee ID (BAccountID)
                                //var curEmployee = PXSelect<EPEmployee, Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>.Select(Base).TopFirst;
                                //pOOrder.EmployeeID = curEmployee.BAccountID;

                                int checkSeq = 0;
                                foreach (POLine line in Base.Transactions.Cache.Cached)
                                {
                                    var targetInventoryID = PXSelect<InventoryItem, Where<InventoryItem.inventoryCD, Equal<Required<InventoryItem.inventoryCD>>>>.Select(sOOrderEntry, inventoryCDList[checkSeq])?.TopFirst?.InventoryID;
                                    if (targetInventoryID == null) targetInventoryID = PXSelect<INItemXRef, Where<INItemXRef.alternateID, Equal<Required<INItemXRef.alternateID>>>>.Select(sOOrderEntry, inventoryCDList[checkSeq])?.TopFirst?.InventoryID;
                                    if (targetInventoryID == null) throw new PXException("The inventory Item cannot be found.");
                                    checkSeq++;
                                }


                                sOOrderEntry.Actions.PressSave();

                                /* SO Line */
                                int i = 0;
                                foreach (POLine line in Base.Transactions.Cache.Cached)
                                {
                                    SOLine sOLine = new SOLine();
                                    var targetInventoryID = PXSelect<InventoryItem, Where<InventoryItem.inventoryCD, Equal<Required<InventoryItem.inventoryCD>>>>.Select(sOOrderEntry, inventoryCDList[i])?.TopFirst?.InventoryID;
                                    if (targetInventoryID == null) targetInventoryID = PXSelect<INItemXRef, Where<INItemXRef.alternateID, Equal<Required<INItemXRef.alternateID>>>>.Select(sOOrderEntry, inventoryCDList[i])?.TopFirst?.InventoryID;
                                    if (targetInventoryID == null) throw new PXException("The inventory Item cannot be found.");
                                    
                                    sOLine.InventoryID = targetInventoryID;
                                    sOLine.OrderQty = line.OrderQty;
                                    sOLine.CuryUnitPrice = line.CuryUnitCost;

                                    sOLine = sOOrderEntry.Transactions.Insert(sOLine);
                                    sOOrderEntry.Actions.PressSave();

                                    i++;
                                }

                                //D.If the SO is saved successfully, please update the following fields
                                // - SOOrder, ICPOCreated = true
                                SOOrderExt sOOrderExt = sOOrder.GetExtension<SOOrderExt>();
                                sOOrderExt.UsrICPOCreated = true;
                                //add for intercompany
                                sOOrderExt.UsrICPONoteID = curPOOrder.NoteID;
                                sOOrder = sOOrderEntry.Document.Update(sOOrder);
                                sOOrderEntry.Actions.PressSave();

                                POOrderVendorRefNbr = sOOrder.OrderNbr;
                            }

                            //D.If the SO is saved successfully, please update the following fields
                            // - POOrder, ICSOCreated = true; Customer Order Nbr = SOOrder.OrderNbr
                            POOrderExt pOOrderExt = curPOOrder.GetExtension<POOrderExt>();
                            pOOrderExt.UsrICSOCreated = true;
                            curPOOrder.VendorRefNbr = POOrderVendorRefNbr;
                            Base.Document.Update(curPOOrder);
                            Base.Save.Press();
                        });
                    }
                    catch (Exception)
                    {
                        throw new PXException("The vendor is not defined in destination tenant");
                    }
                }
                else
                {
                    throw new PXException("Please try again.");
                }
            }

        }
        #endregion

        #region Event Handlers
        protected virtual void POOrder_RowSelected(PXCache cache, PXRowSelectedEventArgs e, PXRowSelected InvokeBaseHandler)
        {
            InvokeBaseHandler?.Invoke(cache, e);
            var row = e.Row as POOrder;
            
            // check the ICSOCreated checkbox
            if (row.GetExtension<POOrderExt>()?.UsrICSOCreated == true)
            {
                Base.Document.AllowDelete = false;
                CreateICSOAction.SetEnabled(false);
            }
            else
            {
                Base.Document.AllowDelete = true;
                CreateICSOAction.SetEnabled(true);
            }
        }

        protected virtual void POOrder_RowDeleting(PXCache cache, PXRowDeletingEventArgs e, PXRowDeleting InvokeBaseHandler)
        {
            InvokeBaseHandler?.Invoke(cache, e);
            var row = e.Row as POOrder;
            if (row == null) return;

            // check the ICSOCreated checkbox
            if (row.GetExtension<POOrderExt>()?.UsrICSOCreated == true) throw new PXException("You cannot delete this PO because ICSOCreated is clicked.");
        }
        #endregion
    }
}
