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
using LumTomofunCustomization.DAC;

namespace LumTomofunCustomization.Graph
{
    public class LUMPaypalPaymentProcess : PXGraph<LUMPaypalPaymentProcess>
    {
        public PXSave<LUMPaypalPaymentTransData> Save;
        public PXCancel<LUMPaypalPaymentTransData> Cancel;

        [PXImport(typeof(LUMPaypalPaymentTransData))]
        public PXProcessing<LUMPaypalPaymentTransData> PaymentTransactions;

        public LUMPaypalPaymentProcess()
        {
            this.PaymentTransactions.Cache.AllowInsert = this.PaymentTransactions.Cache.AllowUpdate = this.PaymentTransactions.Cache.AllowDelete = true;
            #region Set Field Enable
            PXUIFieldAttribute.SetEnabled<LUMPaypalPaymentTransData.transSequenceNumber>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMPaypalPaymentTransData.marketplace>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMPaypalPaymentTransData.transactionDate>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMPaypalPaymentTransData.transactionType>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMPaypalPaymentTransData.invoiceID>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMPaypalPaymentTransData.orderID>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMPaypalPaymentTransData.gross>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMPaypalPaymentTransData.fee>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMPaypalPaymentTransData.net>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMPaypalPaymentTransData.description>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMPaypalPaymentTransData.marketplace2>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMPaypalPaymentTransData.currency>(PaymentTransactions.Cache, null, true);
            #endregion
            this.PaymentTransactions.SetProcessDelegate(delegate (List<LUMPaypalPaymentTransData> list)
            {
                GoProcessing(list);
            });
        }

        #region Event

        public virtual void _(Events.RowInserted<LUMPaypalPaymentTransData> e)
        {
            var row = e.Row as LUMPaypalPaymentTransData;
            // OrderID/Type
            if (string.IsNullOrEmpty(row.InvoiceID) || !row.InvoiceID.StartsWith("c"))
            {
                row.OrderID = string.Empty;
                row.TransactionType = "Skip";
            }
            else if (row.InvoiceID.IndexOf('.') != -1)
            {
                row.OrderID = row.InvoiceID.Substring(1, row.InvoiceID.IndexOf('.') - 1);
                row.TransactionType = row.Description == "Express Checkout Payment" ? "Order" :
                                      row.Description == "Payment Refund" ? "Refund" :
                                      row.Description == "Cancellation of Hold for Dispute Resolution" ? "DisputePay" :
                                      row.Description == "Dispute Fee" ? "DisputeCharge" :
                                      row.Description == "Chargeback" ? "Chargeback" :
                                      row.Description == "Hold on Balance for Dispute Investigation" ? "DisputeCharge" :
                                      row.Description == "Payment Reversal" ? "Reversal" : string.Empty;
            }
            // Marketplace2
            if (!string.IsNullOrEmpty(row.Marketplace))
                row.Marketplace2 = row.Marketplace;
            else
                row.Marketplace2 = SelectFrom<SOOrder>
                                    .InnerJoin<LUMShopifyMarketplacePreference>.On<SOOrder.customerID.IsEqual<LUMShopifyMarketplacePreference.bAccountID>>
                                    .Where<SOOrder.customerOrderNbr.IsEqual<P.AsString>.Or<SOOrder.customerRefNbr.IsEqual<P.AsString>>>
                                    .View.SelectSingleBound(this, null, row.OrderID, row.OrderID).RowCast<LUMShopifyMarketplacePreference>().FirstOrDefault()?.Marketplace;
        }

        #endregion

        #region Method

        public static void GoProcessing(List<LUMPaypalPaymentTransData> list)
        {
            var baseGraph = CreateInstance<LUMPaypalPaymentProcess>();
            baseGraph.CreateShopifyPayment(list, baseGraph);
        }

        /// <summary> Create Shopify Payment </summary>
        public virtual void CreateShopifyPayment(List<LUMPaypalPaymentTransData> list, LUMPaypalPaymentProcess baseGraph)
        {
            foreach (var row in list)
            {
                // clean error message
                row.ErrorMessage = string.Empty;
                PXLongOperation.SetCurrentItem(row);
                try
                {
                    using (PXTransactionScope sc = new PXTransactionScope())
                    {
                        // 以建立的Shopify Sales Order
                        var shopifySOOrder = SelectFrom<SOOrder>
                         .Where<SOOrder.orderType.IsEqual<P.AsString>
                           .And<SOOrder.customerOrderNbr.IsEqual<P.AsString>.Or<SOOrder.customerRefNbr.IsEqual<P.AsString>>>>
                         .View.SelectSingleBound(baseGraph, null, "SP", row.OrderID, row.OrderID).TopFirst;
                        // Shopify Cash account
                        var spCashAccount = SelectFrom<CashAccount>
                                            .Where<CashAccount.cashAccountCD.IsEqual<P.AsString>>
                                            .View.SelectSingleBound(baseGraph, null, $"{row.Currency}SPFPAL").TopFirst;
                        switch (row.TransactionType.ToUpper())
                        {
                            case "ORDER":
                                #region TransactionType: ORDER
                                if (spCashAccount == null)
                                    throw new PXException($"Can not find Cash Account ({row.Currency}SPFPAL)");
                                var arGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                                #region Header(Document)
                                var arDoc = arGraph.Document.Cache.CreateInstance() as ARPayment;
                                arDoc.DocType = shopifySOOrder == null ? "PMT" :
                                                shopifySOOrder.Status == "N" ? "PPM" : "PMT";
                                arDoc.AdjDate = row.TransactionDate;
                                arDoc.ExtRefNbr = row.OrderID;
                                arDoc.CustomerID = ShopifyPublicFunction.GetMarketplaceCustomer(row.Marketplace);
                                arDoc.CashAccountID = spCashAccount.CashAccountID;
                                arDoc.DocDesc = $"PayPal Payment Gateway {row.OrderID}";
                                arDoc.DepositAfter = row.TransactionDate;
                                arDoc.CuryOrigDocAmt = row.Gross;
                                #region User-Defiend
                                // UserDefined - ECNETPAY
                                arGraph.Document.Cache.SetValueExt(arDoc, PX.Objects.CS.Messages.Attribute + "ECNETPAY", row.Net);
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
                                                          .View.SelectSingleBound(baseGraph, null, shopifySOOrder.OrderNbr, shopifySOOrder.OrderType).TopFirst?.RefNbr;
                                    adjTrans.CuryAdjgAmt = row.Gross;
                                    arGraph.Adjustments.Insert(adjTrans);
                                    #endregion
                                }
                                else
                                {
                                    #region SOAdjust
                                    var adjSOTrans = arGraph.SOAdjustments.Cache.CreateInstance() as SOAdjust;
                                    adjSOTrans.AdjdOrderType = shopifySOOrder.OrderType;
                                    adjSOTrans.AdjdOrderNbr = shopifySOOrder.OrderNbr;
                                    adjSOTrans.CuryAdjgAmt = row.Gross;
                                    arGraph.SOAdjustments.Insert(adjSOTrans);
                                    #endregion
                                }
                                #endregion

                                #region CHARGS
                                var chargeTrans = arGraph.PaymentCharges.Cache.CreateInstance() as ARPaymentChargeTran;
                                chargeTrans.EntryTypeID = "COMMISSION";
                                chargeTrans.CuryTranAmt = (row.Fee ?? 0) * -1;
                                arGraph.PaymentCharges.Insert(chargeTrans);
                                #endregion

                                // set payment amount to apply amount
                                if (arDoc.DocType == "PMT")
                                    arGraph.Document.SetValueExt<ARPayment.curyOrigDocAmt>(arGraph.Document.Current, arGraph.Document.Current.CuryApplAmt);
                                else if (arDoc.DocType == "PPM")
                                    arGraph.Document.SetValueExt<ARPayment.curyOrigDocAmt>(arGraph.Document.Current, arGraph.Document.Current.CurySOApplAmt);
                                // Save Payment
                                arGraph.Actions.PressSave();

                                #endregion
                                break;
                            case "REFUND":
                            case "CHARGEBACK":
                            case "DISPUTECHARGE":
                            case "REVERSAL":
                                #region TransactionType: REFUND/CHARGEBACK/DISPUTECHARGE/REVERSAL
                                var soGraph = PXGraph.CreateInstance<SOOrderEntry>();

                                #region Header
                                var soDoc = soGraph.Document.Cache.CreateInstance() as SOOrder;
                                soDoc.OrderType = "CM";
                                soDoc.CustomerOrderNbr = row.OrderID;
                                soDoc.OrderDate = row.TransactionDate;
                                soDoc.RequestDate = Accessinfo.BusinessDate;
                                soDoc.CustomerID = ShopifyPublicFunction.GetMarketplaceCustomer(row.Marketplace);
                                soDoc.OrderDesc = $"Paypal Payment Gateway {row.Description} {row.OrderID}";
                                #endregion

                                #region User-Defined
                                // UserDefined - ORDERTYPE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERTYPE", $"Paypal Gateway {row.Description}");
                                // UserDefined - MKTPLACE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "MKTPLACE", row.Marketplace2);
                                // UserDefined - ORDERAMT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERAMT", Math.Abs(row.Net ?? 0));
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
                                soGraph_SP.Document.Current = shopifySOOrder;
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
                                // Amount
                                var soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                soTrans.InventoryID = ShopifyPublicFunction.GetInvetoryitemID(soGraph, row.TransactionType);
                                soTrans.OrderQty = 1;
                                soTrans.CuryUnitPrice = (row.Gross ?? 0) * -1;
                                soGraph.Transactions.Insert(soTrans);
                                // Fee
                                soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                soTrans.InventoryID = ShopifyPublicFunction.GetInvetoryitemID(soGraph, "EC-COMMISSION");
                                soTrans.OrderQty = 1;
                                soTrans.CuryUnitPrice = (row.Fee ?? 0) * -1;
                                soGraph.Transactions.Insert(soTrans);
                                #endregion

                                #region Update Tax
                                // Setting SO Tax
                                soGraph.Taxes.Current = soGraph.Taxes.Current ?? soGraph.Taxes.Insert(soGraph.Taxes.Cache.CreateInstance() as SOTaxTran);
                                soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, row.Marketplace2 + "EC");
                                soGraph.Taxes.Update(soGraph.Taxes.Current);
                                #endregion

                                // Sales Order Save
                                soGraph.Save.Press();

                                #region Create PaymentRefund
                                var paymentExt = soGraph.GetExtension<CreatePaymentExt>();
                                paymentExt.SetDefaultValues(paymentExt.QuickPayment.Current, soGraph.Document.Current);
                                paymentExt.QuickPayment.Current.CuryRefundAmt = row.Net * -1;
                                paymentExt.QuickPayment.Current.CashAccountID = spCashAccount.CashAccountID;
                                paymentExt.QuickPayment.Current.ExtRefNbr = row.OrderID;
                                ARPaymentEntry paymentEntry = paymentExt.CreatePayment(paymentExt.QuickPayment.Current, soGraph.Document.Current, ARPaymentType.Refund);
                                paymentEntry.releaseFromHold.Press();
                                paymentEntry.Save.Press();
                                #endregion

                                // Prepare Invoice
                                PrepareInvoiceAndOverrideTax(soGraph, soDoc);
                                #endregion
                                break;
                            case "DISPUTEPAY":
                                #region TransactionType: DISPUTEPAY
                                soGraph = PXGraph.CreateInstance<SOOrderEntry>();

                                #region Header
                                soDoc = soGraph.Document.Cache.CreateInstance() as SOOrder;
                                soDoc.OrderType = "IN";
                                soDoc.CustomerOrderNbr = row.OrderID;
                                soDoc.OrderDate = row.TransactionDate;
                                soDoc.RequestDate = Accessinfo.BusinessDate;
                                soDoc.CustomerID = ShopifyPublicFunction.GetMarketplaceCustomer(row.Marketplace);
                                soDoc.OrderDesc = $"Paypal Payment Gateway {row.Description} {row.OrderID}";
                                #endregion

                                #region User-Defined
                                // UserDefined - ORDERTYPE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERTYPE", $"Paypal Gateway {row.Description}");
                                // UserDefined - MKTPLACE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "MKTPLACE", row.Marketplace2);
                                // UserDefined - ORDERAMT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERAMT", Math.Abs(row.Net ?? 0));
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

                                #region Address
                                soGraph_SP = PXGraph.CreateInstance<SOOrderEntry>();
                                soGraph_SP.Document.Current = shopifySOOrder;
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
                                // Amount
                                soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                soTrans.InventoryID = ShopifyPublicFunction.GetInvetoryitemID(soGraph, row.TransactionType);
                                soTrans.OrderQty = 1;
                                soTrans.CuryUnitPrice = (row.Gross ?? 0);
                                soGraph.Transactions.Insert(soTrans);
                                // Fee
                                soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                soTrans.InventoryID = ShopifyPublicFunction.GetInvetoryitemID(soGraph, "EC-COMMISSION");
                                soTrans.OrderQty = 1;
                                soTrans.CuryUnitPrice = (row.Fee ?? 0);
                                soGraph.Transactions.Insert(soTrans);
                                #endregion

                                #region Update Tax
                                // Setting SO Tax
                                soGraph.Taxes.Current = soGraph.Taxes.Current ?? soGraph.Taxes.Insert(soGraph.Taxes.Cache.CreateInstance() as SOTaxTran);
                                soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, row.Marketplace2 + "EC");
                                soGraph.Taxes.Update(soGraph.Taxes.Current);
                                #endregion

                                // Sales Order Save
                                soGraph.Save.Press();

                                #region Create Payment
                                paymentExt = soGraph.GetExtension<CreatePaymentExt>();
                                paymentExt.SetDefaultValues(paymentExt.QuickPayment.Current, soGraph.Document.Current);
                                paymentExt.QuickPayment.Current.CuryOrigDocAmt = row.Net;
                                paymentExt.QuickPayment.Current.CashAccountID = spCashAccount.CashAccountID;
                                paymentExt.QuickPayment.Current.ExtRefNbr = row.OrderID;
                                paymentEntry = paymentExt.CreatePayment(paymentExt.QuickPayment.Current, soGraph.Document.Current, ARPaymentType.Payment);
                                paymentEntry.releaseFromHold.Press();
                                paymentEntry.Save.Press();
                                #endregion

                                // Prepare Invoice
                                PrepareInvoiceAndOverrideTax(soGraph, soDoc);
                                #endregion
                                break;
                            default:
                                throw new PXException("Transaction Type is not valid");
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
                invoiceGraph.releaseFromCreditHold.Press();
                // invoiceGraph.release.Press();
                #endregion
            }
        }

        #endregion

    }
}
