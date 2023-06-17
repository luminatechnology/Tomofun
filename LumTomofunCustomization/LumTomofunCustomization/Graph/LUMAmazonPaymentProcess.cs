using LUMTomofunCustomization.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.SO;
using System.Collections;
using PX.Objects.CA;
using PX.Objects.AR;
using PX.Objects.CM;
using LumTomofunCustomization.LUMLibrary;
using PX.Objects.SO.GraphExtensions.SOOrderEntryExt;

namespace LumTomofunCustomization.Graph
{
    public class LUMAmazonPaymentProcess : PXGraph<LUMAmazonPaymentProcess>, PXImportAttribute.IPXPrepareItems
    {
        public PXSave<LUMAmazonPaymentTransData> Save;
        public PXCancel<LUMAmazonPaymentTransData> Cancel;

        [PXImport(typeof(LUMAmazonPaymentTransData))]
        public PXProcessing<LUMAmazonPaymentTransData> PaymentTransactions;

        public LUMAmazonPaymentProcess()
        {
            this.PaymentTransactions.Cache.AllowInsert = this.PaymentTransactions.Cache.AllowUpdate = this.PaymentTransactions.Cache.AllowDelete = true;
            #region Set Field Enable
            PXUIFieldAttribute.SetEnabled<LUMAmazonPaymentTransData.transSequenceNumber>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonPaymentTransData.marketplace>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonPaymentTransData.transactionPostedDate>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonPaymentTransData.transactionType>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonPaymentTransData.amazonTransactionId>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonPaymentTransData.amazonOrderReferenceId>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonPaymentTransData.sellerOrderId>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonPaymentTransData.transactionAmount>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonPaymentTransData.totalTransactionFee>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonPaymentTransData.netTransactionAmount>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonPaymentTransData.settlementId>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonPaymentTransData.currencyCode>(PaymentTransactions.Cache, null, true);
            #endregion
            this.PaymentTransactions.SetProcessDelegate(delegate (List<LUMAmazonPaymentTransData> list)
            {
                GoProcessing(list);
            });
        }

        #region Event

        public virtual void _(Events.RowInserted<LUMAmazonPaymentTransData> e)
        {
            var row = e.Row as LUMAmazonPaymentTransData;
            if (row.SellerOrderId.IndexOf("#") != -1)
                row.OrderID = row.SellerOrderId.Substring(row.SellerOrderId.IndexOf("#") + 1);
            // 新的Payment method 所提供的Excel SellerOrderID 不會有'#' 所以需要放入'OrderID'
            else
                row.OrderID = row.SellerOrderId;
        }

        #endregion

        #region Action
        public PXAction<LUMAmazonPaymentTransData> deleteRecord;
        [PXButton]
        [PXUIField(DisplayName = "Delete All Payment", MapEnableRights = PXCacheRights.Delete, MapViewRights = PXCacheRights.Delete)]
        public virtual IEnumerable DeleteRecord(PXAdapter adapter)
        {
            WebDialogResult result = this.PaymentTransactions.Ask(ActionsMessages.Warning, PXMessages.LocalizeFormatNoPrefix("Do you want to delete data?"),
                MessageButtons.OKCancel, MessageIcon.Warning, true);
            if (result != WebDialogResult.OK)
                return adapter.Get();

            PXDatabase.Delete<LUMAmazonPaymentTransData>();
            this.PaymentTransactions.Cache.Clear();
            return adapter.Get();
        }

        #endregion

        #region Method

        public static void GoProcessing(List<LUMAmazonPaymentTransData> list)
        {
            var baseGraph = CreateInstance<LUMAmazonPaymentProcess>();
            baseGraph.CreateShopifyPayment(list, baseGraph);
        }

        /// <summary> Create Shopify Payment </summary>
        public virtual void CreateShopifyPayment(List<LUMAmazonPaymentTransData> list, LUMAmazonPaymentProcess baseGraph)
        {
            foreach (var row in list)
            {
                // Setting marketplace
                row.Marketplace = row.CurrencyCode?.ToUpper() == "USD" ? "US" :
                                  row.CurrencyCode?.ToUpper() == "JPY" ? "JP" : "";
                // clean error message
                row.ErrorMessage = string.Empty;
                PXLongOperation.SetCurrentItem(row);
                try
                {
                    using (PXTransactionScope sc = new PXTransactionScope())
                    {
                        // SOLine SalesAccount
                        int? newSalesAcctID = null;
                        // SOLine SalesSubAccount
                        int? newSalesSubAcctID = null;
                        // 以建立的Shopify Sales Order
                        var oldShopifySOOrder = SelectFrom<SOOrder>
                                               .Where<SOOrder.orderType.IsEqual<P.AsString>
                                                 .And<SOOrder.customerOrderNbr.IsEqual<P.AsString>.Or<SOOrder.customerRefNbr.IsEqual<P.AsString>>>>
                                               .View.SelectSingleBound(baseGraph, null, "SP", row.OrderID, row.OrderID).TopFirst;
                        // Shopify Cash account
                        var spCashAccount = SelectFrom<CashAccount>
                                            .Where<CashAccount.cashAccountCD.IsEqual<P.AsString>>
                                            .View.SelectSingleBound(baseGraph, null, $"{row.Marketplace}SPFAMZ").TopFirst;
                        switch (row.TransactionType.ToUpper())
                        {
                            case "CAPTURE":
                                #region TransactionType: CAPTURE
                                if (oldShopifySOOrder == null)
                                    throw new PXException("Cannot find Sales Order");
                                if (spCashAccount == null)
                                    throw new PXException($"Can not find Cash Account ({row.CurrencyCode}SPFAMZ)");
                                var arGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                                #region Header(Document)
                                var arDoc = arGraph.Document.Cache.CreateInstance() as ARPayment;
                                arDoc.DocType = oldShopifySOOrder == null ? "PMT" :
                                                oldShopifySOOrder.Status == "N" ? "PPM" : "PMT";
                                arDoc.AdjDate = row.TransactionPostedDate;
                                arDoc.ExtRefNbr = row.SettlementId;
                                arDoc.CustomerID = ShopifyPublicFunction.GetMarketplaceCustomer(row.Marketplace);
                                arDoc.CashAccountID = spCashAccount.CashAccountID;
                                arDoc.DocDesc = $"Amazon Payment Gateway {row.OrderID}";
                                arDoc.DepositAfter = row.TransactionPostedDate;
                                arDoc.CuryOrigDocAmt = row.TransactionAmount;
                                #region User-Defiend
                                // UserDefined - ECNETPAY
                                arGraph.Document.Cache.SetValueExt(arDoc, PX.Objects.CS.Messages.Attribute + "ECNETPAY", row.NetTransactionAmount);
                                #endregion

                                arGraph.Document.Insert(arDoc);
                                #endregion

                                #region Document to Apply / Sales order
                                if (arDoc.DocType == "PMT")
                                {
                                    #region Adjustments
                                    var adjTrans = arGraph.Adjustments.Cache.CreateInstance() as ARAdjust;
                                    adjTrans.AdjdDocType = "INV";
                                    adjTrans.AdjdRefNbr = SelectFrom<ARInvoice>
                                                          .InnerJoin<ARTran>.On<ARInvoice.docType.IsEqual<ARTran.tranType>
                                                                .And<ARInvoice.refNbr.IsEqual<ARTran.refNbr>>>
                                                          .InnerJoin<SOOrder>.On<ARTran.sOOrderNbr.IsEqual<SOOrder.orderNbr>>
                                                          .Where<SOOrder.orderNbr.IsEqual<P.AsString>
                                                            .And<SOOrder.orderType.IsEqual<P.AsString>>>
                                                          .View.SelectSingleBound(baseGraph, null, oldShopifySOOrder.OrderNbr, oldShopifySOOrder.OrderType).TopFirst?.RefNbr;
                                    adjTrans.CuryAdjgAmt = row.TransactionAmount;
                                    arGraph.Adjustments.Insert(adjTrans);
                                    #endregion
                                }
                                else
                                {
                                    #region SOAdjust
                                    var adjSOTrans = arGraph.SOAdjustments.Cache.CreateInstance() as SOAdjust;
                                    adjSOTrans.AdjdOrderType = oldShopifySOOrder.OrderType;
                                    adjSOTrans.AdjdOrderNbr = oldShopifySOOrder.OrderNbr;
                                    adjSOTrans.CuryAdjgAmt = row.TransactionAmount;
                                    arGraph.SOAdjustments.Insert(adjSOTrans);
                                    #endregion
                                }
                                #endregion

                                #region CHARGS
                                var chargeTrans = arGraph.PaymentCharges.Cache.CreateInstance() as ARPaymentChargeTran;
                                chargeTrans.EntryTypeID = "COMMISSION";
                                chargeTrans.CuryTranAmt = (row.TotalTransactionFee ?? 0) * -1;
                                arGraph.PaymentCharges.Insert(chargeTrans);
                                #endregion

                                // set payment amount to apply amount
                                if (arDoc.DocType == "PMT")
                                    arGraph.Document.SetValueExt<ARPayment.curyOrigDocAmt>(arGraph.Document.Current, arGraph.Document.Current.CuryApplAmt);
                                else if (arDoc.DocType == "PPM")
                                    arGraph.Document.SetValueExt<ARPayment.curyOrigDocAmt>(arGraph.Document.Current, arGraph.Document.Current.CurySOApplAmt);
                                // Save Payment
                                arGraph.Actions.PressSave();
                                arGraph.releaseFromHold.Press();
                                arGraph.release.Press();
                                #endregion
                                break;
                            case "REFUND":
                            case "CHARGEBACK":
                            case "A TO Z GUARANTEE CLAIM":
                                #region TransactionType: REFUND/CHARGEBACK/A TO Z GUARANTEE CLAIM
                                if (oldShopifySOOrder == null && row.TransactionType.ToUpper() == "REFUND")
                                    throw new PXException($"Can not find SalesOrder");
                                var soGraph = PXGraph.CreateInstance<SOOrderEntry>();

                                #region Header
                                var soDoc = soGraph.Document.Cache.CreateInstance() as SOOrder;
                                soDoc.OrderType = "CM";
                                soDoc.CustomerOrderNbr = row.OrderID;
                                soDoc.OrderDate = row.TransactionPostedDate;
                                soDoc.RequestDate = row.TransactionPostedDate;
                                soDoc.CustomerID = ShopifyPublicFunction.GetMarketplaceCustomer(row.Marketplace);
                                soDoc.OrderDesc = $"Amazon Payment Gateway {row.TransactionType} {row.OrderID}";
                                #endregion

                                #region User-Defined
                                // UserDefined - ORDERTYPE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERTYPE", $"Amazon Gateway Refund");
                                // UserDefined - MKTPLACE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "MKTPLACE", row.Marketplace);
                                // UserDefined - ORDERAMT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERAMT", Math.Abs(row.NetTransactionAmount ?? 0));
                                // UserDefined - ORDTAAMT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDTAXAMT", 0);
                                // UserDefined - TAXCOLLECT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "TAXCOLLECT", 0);
                                #endregion

                                // Insert SOOrder
                                soGraph.Document.Insert(soDoc);

                                #region Set Currency
                                CurrencyInfo info = CurrencyInfoAttribute.SetDefaults<SOOrder.curyInfoID>(soGraph.Document.Cache, soGraph.Document.Current);
                                if (info != null)
                                    soGraph.Document.Cache.SetValueExt<SOOrder.curyID>(soGraph.Document.Current, info.CuryID);
                                #endregion

                                #region Address
                                var soGraph_SP = PXGraph.CreateInstance<SOOrderEntry>();
                                soGraph_SP.Document.Current = oldShopifySOOrder;
                                if (soGraph_SP.Document.Current != null)
                                {
                                    // Setting Shipping_Address
                                    var soAddress = soGraph.Shipping_Address.Current;
                                    soGraph_SP.Shipping_Address.Current = soGraph_SP.Shipping_Address.Select();
                                    soAddress.OverrideAddress = true;
                                    soAddress.PostalCode = soGraph_SP.Shipping_Address.Current?.PostalCode;
                                    soAddress.CountryID = soGraph_SP.Shipping_Address.Current?.CountryID;
                                    soAddress.State = soGraph_SP.Shipping_Address.Current?.State;
                                    soAddress.City = soGraph_SP.Shipping_Address.Current?.City;
                                    soAddress.RevisionID = 1;
                                    // Setting Shipping_Contact
                                    var soContact = soGraph.Shipping_Contact.Current;
                                    soGraph_SP.Shipping_Contact.Current = soGraph_SP.Shipping_Contact.Select();
                                    soContact.OverrideContact = true;
                                    soContact.Email = soGraph_SP.Shipping_Contact.Current?.Email;
                                    soContact.RevisionID = 1;
                                }
                                #endregion

                                #region SOLine
                                var ECWHTAXAmount = (decimal?)0;
                                // SOLine-Tax
                                if (row.TransactionType.ToUpper() == "REFUND")
                                {
                                    ECWHTAXAmount = row.TransactionAmount * -1 == oldShopifySOOrder?.CuryOrderTotal ? oldShopifySOOrder?.CuryTaxTotal : oldShopifySOOrder?.CuryTaxTotal / oldShopifySOOrder?.CuryOrderTotal * row.TransactionAmount * -1;
                                    var itemCD = $"EC-WHTAX-{row.Marketplace}";
                                    var ecSOTran = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                    ecSOTran.InventoryID = ShopifyPublicFunction.GetInvetoryitemID(soGraph, itemCD);
                                    ecSOTran.OrderQty = 1;
                                    ecSOTran.CuryUnitPrice = ECWHTAXAmount;
                                    newSalesAcctID = ShopifyPublicFunction.GetSalesAcctID(soGraph, itemCD, ecSOTran.InventoryID, oldShopifySOOrder, soDoc.CustomerID);
                                    newSalesSubAcctID = ShopifyPublicFunction.GetSalesSubAcctID(soGraph, itemCD, ecSOTran.InventoryID, oldShopifySOOrder, soDoc.CustomerID);
                                    if (newSalesAcctID.HasValue)
                                        ecSOTran.SalesAcctID = newSalesAcctID;
                                    if (newSalesSubAcctID.HasValue)
                                        ecSOTran.SalesSubID = newSalesSubAcctID;
                                    soGraph.Transactions.Insert(ecSOTran);
                                }

                                // Amount
                                var soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                if (row.TransactionType.Length > 6 && row.TransactionType.ToUpper().Substring(0, 6) == "A TO Z")
                                {
                                    soTrans.InventoryID = ShopifyPublicFunction.GetInvetoryitemID(soGraph, "GuaranteeClaim");
                                    newSalesAcctID = ShopifyPublicFunction.GetSalesAcctID(soGraph, "GuaranteeClaim", soTrans.InventoryID, oldShopifySOOrder, soDoc.CustomerID);
                                    newSalesSubAcctID = ShopifyPublicFunction.GetSalesSubAcctID(soGraph, "GuaranteeClaim", soTrans.InventoryID, oldShopifySOOrder, soDoc.CustomerID);
                                    // If Inventory ID != ‘Refund’ 
                                    soTrans.TaxCategoryID = "NONTAXABLE";
                                }
                                else
                                {
                                    var refundItemCD = row.TransactionType.ToUpper() == "REFUND" && row.Marketplace.ToUpper() == "TW" ? $"Refund-{row.Marketplace}" : row.TransactionType;
                                    soTrans.InventoryID = ShopifyPublicFunction.GetInvetoryitemID(soGraph, refundItemCD);
                                    newSalesAcctID = ShopifyPublicFunction.GetSalesAcctID(soGraph, refundItemCD, soTrans.InventoryID, oldShopifySOOrder, soDoc.CustomerID);
                                    newSalesSubAcctID = ShopifyPublicFunction.GetSalesSubAcctID(soGraph, refundItemCD, soTrans.InventoryID, oldShopifySOOrder, soDoc.CustomerID);
                                    // If Inventory ID != ‘Refund’ 
                                    if (row.TransactionType?.ToUpper() != "REFUND")
                                        soTrans.TaxCategoryID = "NONTAXABLE";
                                }
                                soTrans.OrderQty = 1;
                                soTrans.CuryUnitPrice = (row.TransactionAmount ?? 0) * -1 - ECWHTAXAmount;
                                if (newSalesAcctID.HasValue)
                                    soTrans.SalesAcctID = newSalesAcctID;
                                if (newSalesSubAcctID.HasValue)
                                    soTrans.SalesSubID = newSalesSubAcctID;
                                soGraph.Transactions.Insert(soTrans);

                                // Fee
                                if ((row?.TotalTransactionFee ?? 0) != 0)
                                {
                                    soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                    soTrans.InventoryID = ShopifyPublicFunction.GetInvetoryitemID(soGraph, "EC-COMMISSION");
                                    soTrans.OrderQty = 1;
                                    soTrans.CuryUnitPrice = (row.TotalTransactionFee ?? 0) * -1;
                                    newSalesAcctID = ShopifyPublicFunction.GetSalesAcctID(soGraph, "EC-COMMISSION", soTrans.InventoryID, oldShopifySOOrder, soDoc.CustomerID);
                                    newSalesSubAcctID = ShopifyPublicFunction.GetSalesSubAcctID(soGraph, "EC-COMMISSION", soTrans.InventoryID, oldShopifySOOrder, soDoc.CustomerID);
                                    if (newSalesAcctID.HasValue)
                                        soTrans.SalesAcctID = newSalesAcctID;
                                    if (newSalesSubAcctID.HasValue)
                                        soTrans.SalesSubID = newSalesSubAcctID;
                                    // If Inventory ID != ‘Refund’ 
                                    soTrans.TaxCategoryID = "NONTAXABLE";
                                    soGraph.Transactions.Insert(soTrans);
                                }
                                #endregion

                                #region Update Tax
                                // Setting SO Tax
                                soGraph.Taxes.Current = soGraph.Taxes.Current ?? soGraph.Taxes.Insert(soGraph.Taxes.Cache.CreateInstance() as SOTaxTran);
                                soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, row.Marketplace + "EC");
                                soGraph.Taxes.Update(soGraph.Taxes.Current);
                                #endregion

                                // Sales Order Save
                                soGraph.Save.Press();

                                #region Create PaymentRefund
                                var paymentExt = soGraph.GetExtension<CreatePaymentExt>();
                                paymentExt.SetDefaultValues(paymentExt.QuickPayment.Current, soGraph.Document.Current);
                                paymentExt.QuickPayment.Current.CuryRefundAmt = row.NetTransactionAmount * -1;
                                paymentExt.QuickPayment.Current.CashAccountID = spCashAccount.CashAccountID;
                                paymentExt.QuickPayment.Current.ExtRefNbr = row.SettlementId;
                                ARPaymentEntry paymentEntry = paymentExt.CreatePayment(paymentExt.QuickPayment.Current, soGraph.Document.Current, ARPaymentType.Refund);
                                paymentEntry.Document.Cache.SetValueExt<ARPayment.adjDate>(paymentEntry.Document.Current, row.TransactionPostedDate);
                                paymentEntry.Save.Press();
                                paymentEntry.releaseFromHold.Press();
                                paymentEntry.release.Press();
                                #endregion

                                // Prepare Invoice
                                PrepareInvoiceAndOverrideTax(soGraph, soDoc);
                                #endregion
                                break;
                            case "DEBT":
                                #region TransactionType: DEBT
                                soGraph = PXGraph.CreateInstance<SOOrderEntry>();

                                #region Header
                                soDoc = soGraph.Document.Cache.CreateInstance() as SOOrder;
                                soDoc.OrderType = "IN";
                                soDoc.CustomerOrderNbr = row.OrderID;
                                soDoc.OrderDate = row.TransactionPostedDate;
                                soDoc.RequestDate = row.TransactionPostedDate;
                                soDoc.CustomerID = ShopifyPublicFunction.GetMarketplaceCustomer(row.Marketplace);
                                soDoc.OrderDesc = $"Amazon Payment Gateway {row.TransactionType} {row.SettlementId}";
                                #endregion

                                #region User-Defined
                                // UserDefined - ORDERTYPE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERTYPE", $"Amazon Gateway {row.TransactionType}");
                                // UserDefined - MKTPLACE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "MKTPLACE", row.Marketplace);
                                // UserDefined - ORDERAMT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERAMT", Math.Abs(row.NetTransactionAmount ?? 0));
                                // UserDefined - ORDTAAMT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDTAXAMT", 0);
                                // UserDefined - TAXCOLLECT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "TAXCOLLECT", 0);
                                #endregion

                                // Insert SOOrder
                                soGraph.Document.Insert(soDoc);

                                #region Set Currency
                                info = CurrencyInfoAttribute.SetDefaults<SOOrder.curyInfoID>(soGraph.Document.Cache, soGraph.Document.Current);
                                if (info != null)
                                    soGraph.Document.Cache.SetValueExt<SOOrder.curyID>(soGraph.Document.Current, info.CuryID);
                                #endregion

                                #region SOLine
                                // Net
                                soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                soTrans.InventoryID = ShopifyPublicFunction.GetInvetoryitemID(soGraph, "EC-COMMISSION");
                                soTrans.OrderQty = 1;
                                soTrans.CuryUnitPrice = row.NetTransactionAmount;
                                soGraph.Transactions.Insert(soTrans);
                                #endregion

                                #region Update Tax
                                // Setting SO Tax
                                soGraph.Taxes.Current = soGraph.Taxes.Current ?? soGraph.Taxes.Insert(soGraph.Taxes.Cache.CreateInstance() as SOTaxTran);
                                soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, row.Marketplace + "EC");
                                soGraph.Taxes.Update(soGraph.Taxes.Current);
                                #endregion

                                // Sales Order Save
                                soGraph.Save.Press();

                                #region Create Payment
                                paymentExt = soGraph.GetExtension<CreatePaymentExt>();
                                paymentExt.SetDefaultValues(paymentExt.QuickPayment.Current, soGraph.Document.Current);
                                paymentExt.QuickPayment.Current.CuryOrigDocAmt = row.NetTransactionAmount;
                                paymentExt.QuickPayment.Current.CashAccountID = spCashAccount.CashAccountID;
                                paymentExt.QuickPayment.Current.ExtRefNbr = row.SettlementId;
                                paymentEntry = paymentExt.CreatePayment(paymentExt.QuickPayment.Current, soGraph.Document.Current, ARPaymentType.Payment);
                                paymentEntry.Document.Cache.SetValueExt<ARPayment.adjDate>(paymentEntry.Document.Current, row.TransactionPostedDate);
                                paymentEntry.Save.Press();
                                paymentEntry.releaseFromHold.Press();
                                paymentEntry.release.Press();
                                #endregion

                                // Prepare Invoice
                                PrepareInvoiceAndOverrideTax(soGraph, soDoc);
                                #endregion
                                break;
                            default:
                                break;
                        }
                        sc.Complete();
                    }
                }
                catch (PXOuterException ex)
                {
                    row.ErrorMessage = ex.InnerMessages[0];
                }
                catch (Exception ex)
                {
                    row.ErrorMessage = ex.Message;
                }
                finally
                {
                    row.IsProcessed = string.IsNullOrEmpty(row.ErrorMessage);
                    if (!row.IsProcessed.Value)
                        PXProcessing.SetError(row.ErrorMessage);
                    baseGraph.PaymentTransactions.Update(row);
                }
                baseGraph.Actions.PressSave();
            }
        }

        /// <summary> Sales Order Prepare Invoice and Override Tax </summary>
        public virtual void PrepareInvoiceAndOverrideTax(SOOrderEntry soGraph, SOOrder soDoc)
        {
            // Prepare Invoice
            try
            {
                soGraph.releaseFromHold.Press();
                soGraph.prepareInvoice.Press();
            }
            // Prepare Invoice Success
            catch (PXRedirectRequiredException ex)
            {
                #region Override Invoice Tax
                // Invoice Graph
                SOInvoiceEntry invoiceGraph = ex.Graph as SOInvoiceEntry;
                // update docDate
                invoiceGraph.Document.SetValueExt<ARInvoice.docDate>(invoiceGraph.Document.Current, soDoc.RequestDate);
                // Save
                invoiceGraph.Save.Press();
                // Release Invoice
                invoiceGraph.releaseFromHold.Press();
                invoiceGraph.releaseFromCreditHold.Press();
                invoiceGraph.release.Press();
                #endregion
            }
        }

        #endregion

        public bool PrepareImportRow(string viewName, IDictionary keys, IDictionary values)
        {
            try
            {
                var type = values["TransactionType"] as string;
                return type?.ToUpper() != "TRANSFER";
            }
            catch
            {
                return false;
            }
        }

        public void PrepareItems(string viewName, IEnumerable items) { }

        public bool RowImported(string viewName, object row, object oldRow)
        {
            try
            {
                return ((LUMAmazonPaymentTransData)row).TransactionType?.ToUpper() != "TRANSFER";
            }
            catch
            {
                return false;
            }
        }

        public bool RowImporting(string viewName, object row)
            => true;

    }
}
