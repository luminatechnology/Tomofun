using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LUMLocalization.DAC;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.Update;
using PX.Objects.CR;
using PX.Objects.IN;
using PX.Objects.PO;
using PX.Objects.SO;
using static PX.Data.PXAccess;
using BAccount = PX.Objects.CR.BAccount;

namespace LUMLocalization.Graph
{
    public class LumPOCreateIntertenantSO : PXGraph<LumPOCreateIntertenantSO>
    {

		public PXCancel<LumSOForPurchaseReceiptFilter> Cancel;
		public PXFilter<LumSOForPurchaseReceiptFilter> Filter;
		public PXSetup<POSetup> POSetup;
		public PXSetup<INSetup> INSetup;
		public PXSelectReadonly<SOOrder, Where<True, Equal<False>>> Order;

		[PXFilterable]
		[PXVirtualDAC]
		public PXFilteredProcessingOrderBy<v_LumICFromPO, LumSOForPurchaseReceiptFilter, OrderBy<Asc<v_LumICFromPO.sOShipmentShipmentNbr>>> Documents;

		public LumPOCreateIntertenantSO()
		{
			POSetup posetup = POSetup.Current;
			INSetup insetup = INSetup.Current;

			Documents.SetSelected<SOShipment.selected>();
			Documents.SetProcessCaption("Process");
			Documents.SetProcessAllCaption("Process All");

			PXUIFieldAttribute.SetDisplayName<v_LumICFromPO.pobranchid>(Documents.Cache, "Purchasing Company");
			PXUIFieldAttribute.SetDisplayName<v_LumICFromPO.sobranchid>(Order.Cache, "Selling Company");
			PXUIFieldAttribute.SetDisplayName<v_LumICFromPO.pOOrderNbr>(Order.Cache, "Intercompany PO Nbr");

			PXUIFieldAttribute.SetVisible<v_LumICFromPO.sOShipmentWorkgroupID>(Documents.Cache, null, false);
			PXUIFieldAttribute.SetVisible<v_LumICFromPO.shipmentWeight>(Documents.Cache, null, false);
			PXUIFieldAttribute.SetVisible<v_LumICFromPO.shipmentVolume>(Documents.Cache, null, false);
			PXUIFieldAttribute.SetVisible<v_LumICFromPO.sOShipmentPackageCount>(Documents.Cache, null, false);
			PXUIFieldAttribute.SetVisible<v_LumICFromPO.sOShipmentPackageWeight>(Documents.Cache, null, false);
			PXUIFieldAttribute.SetVisible<v_LumICFromPO.sOShipmentStatus>(Documents.Cache, null, false);
		}

		protected virtual IEnumerable documents()
		{
			List<PXResult<v_LumICFromPO>> list = new List<PXResult<v_LumICFromPO>>();

			if (Filter.Current != null)
			{
				using (new PXReadBranchRestrictedScope())
				{
					PXResultset<v_LumICFromPO> shipments;
					if (Filter.Current.DocDate != null)
                    {
						if (Filter.Current.SellingCompany != null)
                        {
                            shipments = SelectFrom<v_LumICFromPO>.
                                Where<v_LumICFromPO.sOShipmentShipDate.IsLessEqual<LumSOForPurchaseReceiptFilter.docDate.FromCurrent>.
                                And<v_LumICFromPO.sOCompanyID.IsEqual<@P.AsInt>>>.
                                View.Select(this, Filter.Current.SellingCompany);
                        }
                        else
                        {
							shipments = SelectFrom<v_LumICFromPO>.
								Where<v_LumICFromPO.sOShipmentShipDate.IsLessEqual<LumSOForPurchaseReceiptFilter.docDate.FromCurrent>>.
								View.Select(this);
						}
					}
                    else
                    {
						shipments = SelectFrom<v_LumICFromPO>.
								Where<v_LumICFromPO.sOShipmentShipDate.IsNull>.
								View.Select(this);
					}

					foreach (PXResult<v_LumICFromPO> shipment in shipments)
					{
						list.Add(shipment);
					}
				}
			}

			return list;
		}

        #region Event Handlers

        public virtual void _(Events.RowSelected<LumSOForPurchaseReceiptFilter> e)
        {
            LumSOForPurchaseReceiptFilter filter = e.Row;
            Documents.SetProcessDelegate(itemsList => GeneratePurchaseReceipt(itemsList, filter));
        }

        public virtual void _(Events.RowUpdated<LumSOForPurchaseReceiptFilter> e)
		{
			LumSOForPurchaseReceiptFilter row = e.Row;
			LumSOForPurchaseReceiptFilter oldRow = e.OldRow;
			if (row != null && oldRow != null && !Filter.Cache.ObjectsEqual<LumSOForPurchaseReceiptFilter.docDate, LumSOForPurchaseReceiptFilter.purchasingCompany, LumSOForPurchaseReceiptFilter.sellingCompany>(row, oldRow))
			{
				Documents.Cache.Clear();
				Documents.Cache.ClearQueryCache();
			}
		}
		#endregion

		#region Processing
		public static void GeneratePurchaseReceipt(List<v_LumICFromPO> itemsList, LumSOForPurchaseReceiptFilter filter)
		{
			if (filter == null)
				return;

			POOrderEntry pOOrderEntry = PXGraph.CreateInstance<POOrderEntry>();
			var curUserName = PXLogin.ExtractUsername(PXContext.PXIdentity.IdentityName);

			//show message if there is only a single selected line and is not created PO receipt
			if (itemsList.Count == 1 && itemsList[0].ReceiptNbr != null && itemsList[0].ReceiptDate != null) throw new PXException("The PO Receipt is created");

			foreach (v_LumICFromPO line in itemsList)
			{
				if (line.ReceiptNbr == null && line.ReceiptDate == null)
				{
					var curPOOrder = SelectFrom<POOrder>.Where<POOrder.orderNbr.IsEqual<@P.AsString>.And<POOrder.orderType.IsEqual<@P.AsString>>>.View.Select(pOOrderEntry, line.POOrderNbr, line.POOrderType)?.TopFirst;
					var curLMICVendor = PXSelect<LMICVendor, Where<LMICVendor.vendorid, Equal<Required<LMICVendor.vendorid>>>>.Select(pOOrderEntry, curPOOrder.VendorID).TopFirst;

					//get intertenant SOShipment line items' shippedQty
					Dictionary<string, decimal?> dicInterTenantShipmentLine = new Dictionary<string, decimal?>();
					if (curLMICVendor != null)
					{
						try
						{
							using (PXLoginScope pXLoginScope = new PXLoginScope($"{curUserName}@{curLMICVendor.LoginName}"))
							{
								SOShipmentEntry sOShipmentEntry = PXGraph.CreateInstance<SOShipmentEntry>();
								//SOShipment sOShipment = SelectFrom<SOShipment>.Where<SOShipment.shipmentNbr.IsEqual<@P.AsString>>.View.Select(sOShipmentEntry, line.SOShipmentShipmentNbr)?.TopFirst;
								var sOShiplines = SelectFrom<SOShipLine>.Where<SOShipLine.shipmentNbr.IsEqual<@P.AsString>>.View.Select(sOShipmentEntry, line.SOShipmentShipmentNbr);

								if (sOShiplines == null) throw new PXException("Shipment Has No Items To Receive");

								foreach (SOShipLine sOShipLine in sOShiplines)
								{
									dicInterTenantShipmentLine.Add(PXSelect<InventoryItem, Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>.Select(sOShipmentEntry, sOShipLine.InventoryID).TopFirst?.InventoryCD, sOShipLine.ShippedQty);
								}
							}
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

					//Remove Hold
					pOOrderEntry.Document.Current = curPOOrder;
					curPOOrder.Status = POOrderStatus.Open;
					curPOOrder = pOOrderEntry.Document.Update(curPOOrder);
					pOOrderEntry.Actions.PressSave();
					//Create PO Reciept

					POOrder order = (POOrder)curPOOrder;

					POReceiptEntry pOReceiptEntry = PXGraph.CreateInstance<POReceiptEntry>();
					POReceipt receipt = pOReceiptEntry.Document.Insert();
					receipt.ReceiptType = POReceiptType.POReceipt;
					receipt.BranchID = order.BranchID;
					receipt.VendorID = order.VendorID;
					receipt.VendorLocationID = order.VendorLocationID;
					receipt.ProjectID = order.ProjectID;
					receipt.CuryID = order.CuryID;
					receipt = pOReceiptEntry.Document.Update(receipt);

					pOReceiptEntry.AddPurchaseOrder(order);
					pOReceiptEntry.Actions.PressSave();

					foreach (POReceiptLine pOReceiptLine in pOReceiptEntry.transactions.Select())
					{
						var inventoryCD = PXSelect<InventoryItem, Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>.Select(pOReceiptEntry, pOReceiptLine.InventoryID).TopFirst?.InventoryCD;
						if (dicInterTenantShipmentLine.ContainsKey(inventoryCD))
						{
							pOReceiptLine.ReceiptQty = dicInterTenantShipmentLine[inventoryCD].Value;
							pOReceiptEntry.transactions.Update(pOReceiptLine);

						}
						else
						{
							pOReceiptEntry.transactions.Delete(pOReceiptLine);
						}
					}
					pOReceiptEntry.Actions.PressSave();
				}
			}
		}
        #endregion

        public PXAction<LumSOForPurchaseReceiptFilter> viewPOOrder;

		[PXUIField(DisplayName = "", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select, Visible = false)]
		[PXEditDetailButton(ImageKey = PX.Web.UI.Sprite.Main.DataEntry)]
		public virtual IEnumerable ViewPOOrder(PXAdapter adapter)
		{
			v_LumICFromPO doc = Documents.Current;
			if (doc != null)
			{
				POOrderEntry pOOrderEntry = PXGraph.CreateInstance<POOrderEntry>();
				pOOrderEntry.Document.Current = POOrder.PK.Find(pOOrderEntry, doc.POOrderType, doc.POOrderNbr);
				PXRedirectHelper.TryRedirect(pOOrderEntry, PXRedirectHelper.WindowMode.NewWindow);
			}
			return Filter.Select();
		}

		public PXAction<LumSOForPurchaseReceiptFilter> viewPOReceipt;

		[PXUIField(DisplayName = "", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select, Visible = false)]
		[PXEditDetailButton(ImageKey = PX.Web.UI.Sprite.Main.DataEntry)]
		public virtual IEnumerable ViewPOReceipt(PXAdapter adapter)
		{
			v_LumICFromPO doc = Documents.Current;
			if (doc != null)
			{
				POReceiptEntry pOReceiptEntry = PXGraph.CreateInstance<POReceiptEntry>();
				pOReceiptEntry.Document.Current = POReceipt.PK.Find(pOReceiptEntry, doc.ReceiptNbr);
				PXRedirectHelper.TryRedirect(pOReceiptEntry, PXRedirectHelper.WindowMode.NewWindow);
			}
			return Filter.Select();
		}


		public override bool IsDirty { get { return false; } }

	}
}