using LumTomofunCustomization.DAC;
using LUMTomofunCustomization.DAC;
using Newtonsoft.Json;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using PX.Objects.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.CM;
using PX.Objects.AR;
using LumTomofunCustomization.LUMLibrary;
using PX.Objects.SO.GraphExtensions.SOOrderEntryExt;
using PX.Objects.CA;

namespace LumTomofunCustomization.Graph
{
    public class LUMShopifyTransactionProcess : PXGraph<LUMShopifyTransactionProcess>
    {
        public PXSave<LUMShopifyTransData> Save;
        public PXCancel<LUMShopifyTransData> Cancel;
        public PXProcessing<LUMShopifyTransData> ShopifyTransaction;

        public LUMShopifyTransactionProcess()
        {
            ShopifyTransaction.Cache.AllowUpdate = true;
            ShopifyTransaction.SetProcessDelegate(delegate (List<LUMShopifyTransData> list)
            {
                GoProcessing(list);
            });
        }

        #region Method

        /// <summary> 執行Process </summary>
        public static void GoProcessing(List<LUMShopifyTransData> list)
        {
            var graph = CreateInstance<LUMShopifyTransactionProcess>();
            graph.CreateSalesOrder(graph, list);
        }

        /// <summary> Create Sales Order </summary>
        public virtual void CreateSalesOrder(LUMShopifyTransactionProcess baseGraph, List<LUMShopifyTransData> shopifyList)
        {
            PXUIFieldAttribute.SetEnabled<LUMShopifyTransData.isProcessed>(ShopifyTransaction.Cache, null, true);
            foreach (var row in shopifyList)
            {
                row.ErrorMessage = string.Empty;
                PXProcessing.SetCurrentItem(row);
                try
                {
                    using (PXTransactionScope sc = new PXTransactionScope())
                    {
                        // Marketplace tax calculation
                        var isTaxCalculate = ShopifyPublicFunction.GetMarketplaceTaxCalculation(row.Marketplace);
                        // Create Sales Order Graph
                        var soGraph = PXGraph.CreateInstance<SOOrderEntry>();
                        var spOrder = JsonConvert.DeserializeObject<API_Entity.ShopifyOrder.ShopifyOrderEntity>(row.TransJson);
                        // validation
                        Validation(row);
                        // 判斷是否需要建立Invoice
                        var GoPrepareInvoice = row.FullfillmentStatus?.ToLower() == "fulfilled";
                        // Shopify Order
                        var shopifySOOrder = SelectFrom<SOOrder>
                                             .Where<SOOrder.customerOrderNbr.IsEqual<P.AsString>
                                                .Or<SOOrder.customerRefNbr.IsEqual<P.AsString>>>
                                             .View.Select(new PXGraph(), spOrder.id, spOrder.id).TopFirst;
                        // Create Sales Order
                        if (shopifySOOrder == null && row.FinancialStatus == "paid")
                        {
                            #region Create Sales Order Header
                            shopifySOOrder = soGraph.Document.Cache.CreateInstance() as SOOrder;
                            shopifySOOrder.OrderType = "SP";
                            shopifySOOrder.CustomerOrderNbr = spOrder.checkout_id.ToString();
                            shopifySOOrder.CustomerRefNbr = spOrder.id.ToString();
                            shopifySOOrder.OrderDesc = $"Shopify Order #{spOrder.order_number}";
                            shopifySOOrder.OrderDate = spOrder.created_at;
                            shopifySOOrder.RequestDate = spOrder.created_at;
                            shopifySOOrder.CustomerID = ShopifyPublicFunction.GetMarketplaceCustomer(row.Marketplace);
                            shopifySOOrder.TermsID = "0000";
                            #region User-Defined
                            // UserDefined - ORDERTYPE
                            soGraph.Document.Cache.SetValueExt(shopifySOOrder, PX.Objects.CS.Messages.Attribute + "ORDERTYPE", spOrder.gateway);
                            // UserDefined - MKTPLACE
                            soGraph.Document.Cache.SetValueExt(shopifySOOrder, PX.Objects.CS.Messages.Attribute + "MKTPLACE", $"Shopify.{row.Marketplace}");
                            // UserDefined - ORDERAMT
                            soGraph.Document.Cache.SetValueExt(shopifySOOrder, PX.Objects.CS.Messages.Attribute + "ORDERAMT", spOrder.current_total_price);
                            // UserDefined - ORDTAAMT
                            soGraph.Document.Cache.SetValueExt(shopifySOOrder, PX.Objects.CS.Messages.Attribute + "ORDTAXAMT", spOrder.current_total_tax);
                            // UserDefined - TAXCOLLECT
                            soGraph.Document.Cache.SetValueExt(shopifySOOrder, PX.Objects.CS.Messages.Attribute + "TAXCOLLECT", 0);
                            // UserDefined -  ORDERTAGS
                            soGraph.Document.Cache.SetValueExt(shopifySOOrder, PX.Objects.CS.Messages.Attribute + "ORDERTAGS", spOrder.tags);
                            #endregion
                            // Insert Sales Order
                            soGraph.Document.Insert(shopifySOOrder);
                            // Setting Shipping_Address
                            var soAddress = soGraph.Shipping_Address.Current;
                            soAddress.OverrideAddress = true;
                            soAddress.PostalCode = spOrder.shipping_address?.zip;
                            soAddress.CountryID = spOrder.shipping_address?.country_code;
                            soAddress.State = spOrder.shipping_address?.province;
                            soAddress.City = spOrder.shipping_address?.city;
                            soAddress.RevisionID = 1;
                            // Setting Shipping_Contact
                            var soContact = soGraph.Shipping_Contact.Current;
                            soContact.OverrideContact = true;
                            soContact.RevisionID = 1;
                            #endregion

                            #region Set Currency
                            CurrencyInfo info = CurrencyInfoAttribute.SetDefaults<SOOrder.curyInfoID>(soGraph.Document.Cache, soGraph.Document.Current);
                            if (info != null)
                                soGraph.Document.Cache.SetValueExt<SOOrder.curyID>(soGraph.Document.Current, info.CuryID);
                            #endregion

                            #region Create Sales Order Line
                            foreach (var item in spOrder.line_items)
                            {
                                // requires_shipping <> True (Do not import this item)
                                if (!item.requires_shipping)
                                    continue;
                                var line = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                line.InventoryID = AmazonPublicFunction.GetInvetoryitemID(soGraph, item.sku);
                                if (line.InventoryID == null)
                                    throw new Exception($"can not find Inventory item ID({item.sku})");
                                line.ManualPrice = true;
                                line.OrderQty = item.quantity;
                                line.CuryUnitPrice = decimal.Parse(item.pre_tax_price) / item.quantity;
                                soGraph.Transactions.Insert(line);
                            }
                            // IF SOLine is empty, do not create Sales Order
                            if (soGraph.Transactions.Cache.Inserted.RowCast<SOLine>().Count() == 0)
                                throw new Exception("can not find andy SOLine Item");
                            #endregion

                            #region Create Slaes Order Line for Shipping
                            if (spOrder.shipping_lines.Any(x => decimal.Parse(x.price) > 0))
                            {
                                var soShipLine = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                soShipLine.InventoryID = ShopifyPublicFunction.GetFeeNonStockItem("Shipping");
                                soShipLine.OrderQty = 1;
                                soShipLine.CuryUnitPrice =
                                    (row.Marketplace == "US" || row.Marketplace == "CA") ?
                                    (decimal?)spOrder.shipping_lines.Sum(x => decimal.Parse(x.price)) :
                                    (decimal?)spOrder.shipping_lines.Sum(x => decimal.Parse(x.price) - x.tax_lines.Sum(y => decimal.Parse(y.price)));
                                soGraph.Transactions.Insert(soShipLine);
                            }
                            #endregion

                            #region Update Tax
                            // Setting SO Tax
                            if (!isTaxCalculate)
                            {
                                soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, row.Marketplace + "EC");
                                soGraph.Taxes.Cache.SetValueExt<SOTaxTran.curyTaxAmt>(soGraph.Taxes.Current, spOrder.current_total_tax);

                                soGraph.Document.Cache.SetValueExt<SOOrder.curyTaxTotal>(soGraph.Document.Current, spOrder.current_total_tax);
                                soGraph.Document.Cache.SetValueExt<SOOrder.curyOrderTotal>(soGraph.Document.Current, (soGraph.Document.Current?.CuryOrderTotal ?? 0) + decimal.Parse(spOrder.current_total_tax));
                            }
                            #endregion

                            #region Create Payment
                            if ((spOrder.gateway ?? string.Empty).ToUpper().Contains("HITRUSTPAY"))
                            {
                                var spCashAccount = SelectFrom<CashAccount>
                                            .Where<CashAccount.cashAccountCD.IsEqual<P.AsString>>
                                            .View.SelectSingleBound(baseGraph, null, $"TWDHITRUST").TopFirst;
                                var paymentExt = soGraph.GetExtension<CreatePaymentExt>();
                                paymentExt.SetDefaultValues(paymentExt.QuickPayment.Current, soGraph.Document.Current);
                                paymentExt.QuickPayment.Current.CashAccountID = spCashAccount.CashAccountID;
                                paymentExt.QuickPayment.Current.ExtRefNbr = row.OrderID;
                                var paymentEntry = paymentExt.CreatePayment(paymentExt.QuickPayment.Current, soGraph.Document.Current, ARPaymentType.Payment);
                                paymentEntry.Save.Press();
                                paymentEntry.releaseFromHold.Press();
                                paymentEntry.release.Press();
                            }
                            #endregion

                            // Write json into note
                            PXNoteAttribute.SetNote(soGraph.Document.Cache, soGraph.Document.Current, row.TransJson);
                            // Sales Order Save
                            soGraph.Save.Press();
                        }
                        // Assign Document Current
                        else if (GoPrepareInvoice && shopifySOOrder != null && shopifySOOrder.Status == "N")
                            soGraph.Document.Current = shopifySOOrder;
                        // Do nothing
                        else
                            throw new Exception($"FinancialStatus or FullfillmentStatus is not correct or Sales Order is already Invoiced");
                        // Prepare Invocie
                        try
                        {
                            // 判斷是否需要Create Invoice
                            var tagConditions = new string[] { "KOL", "REPLACE", "FAAS" };

                            // JSON\Tags is not Empty and Upper(JSON\Tags) NOT INCLUDES ‘KOL’ or ‘REPLACE’ or ‘FAAS
                            if (!string.IsNullOrEmpty(spOrder.tags) && Array.IndexOf(tagConditions, spOrder.tags?.ToUpper()) == -1)
                            { 
                                GoPrepareInvoice = false;
                                row.ErrorMessage = @"JSON\Tags is not Empty and Upper(JSON\Tags) NOT INCLUDES ‘KOL’ or ‘REPLACE’ or ‘FAAS";
                            }
                            // SO Order.CuryOrderTotal is 0 and Upper(JSON\Tags) DOEST NOT INCLUDE ‘KOL’ or ‘REPLACE’ or ‘FAAS’
                            else if (soGraph.Document.Current.CuryOrderTotal == 0 && !string.IsNullOrEmpty(spOrder.tags) && Array.IndexOf(tagConditions, spOrder.tags?.ToUpper()) == -1)
                            { 
                                GoPrepareInvoice = false;
                                row.ErrorMessage = @"SO Order.CuryOrderTotal is 0 and Upper(JSON\Tags) DOEST NOT INCLUDE ‘KOL’ or ‘REPLACE’ or ‘FAAS’";
                            }
                            // Shoipify Market Preference ‘Tax Calculation’ is SELECTED  AND ([SOOrder.AttributeORDERAMT] <> ( [SOOrder.CuryOrderTotal] - [SOOrder.CuryTaxTotal] ))
                            else if (isTaxCalculate && decimal.Parse(spOrder.current_total_price) != soGraph.Document.Current.CuryOrderTotal - soGraph.Document.Current.CuryTaxTotal)
                            {
                                GoPrepareInvoice = false;
                                row.ErrorMessage = @"Shoipify Market Preference ‘Tax Calculation’ is SELECTED  AND ([SOOrder.AttributeORDERAMT] <> ( [SOOrder.CuryOrderTotal] - [SOOrder.CuryTaxTotal] ))";
                            }
                            // Shopify Market Preference ‘Tax Calculation’ is NOT SELECTED AND ([SOOrder.AttributeORDERAMT] - [SOOrder.AttributeTAXCOLLECT] ) <> [SOOrder.CuryOrderTotal]
                            else if (!isTaxCalculate && decimal.Parse(spOrder.current_total_price) - 0 != soGraph.Document.Current.CuryOrderTotal)
                            { 
                                GoPrepareInvoice = false;
                                row.ErrorMessage = @"Shopify Market Preference ‘Tax Calculation’ is NOT SELECTED AND ([SOOrder.AttributeORDERAMT] - [SOOrder.AttributeTAXCOLLECT] ) <> [SOOrder.CuryOrderTotal]";
                            }
                            if (GoPrepareInvoice)
                            {
                                soGraph.releaseFromHold.Press();
                                soGraph.prepareInvoice.Press();
                            }
                        }
                        // Prepare invoice Success
                        catch (PXRedirectRequiredException ex)
                        {
                            SOInvoiceEntry invoiceGraph = ex.Graph as SOInvoiceEntry;
                            // Update docDate
                            invoiceGraph.Document.SetValueExt<ARInvoice.docDate>(invoiceGraph.Document.Current, shopifySOOrder.RequestDate);
                            var soTax = SelectFrom<SOTaxTran>
                                        .Where<SOTaxTran.orderNbr.IsEqual<P.AsString>
                                             .And<SOTaxTran.orderType.IsEqual<P.AsString>>>
                                        .View.SelectSingleBound(this, null, soGraph.Document.Current.OrderNbr, soGraph.Document.Current.OrderType)
                                        .TopFirst;
                            if (soTax != null)
                            {
                                // setting Tax
                                invoiceGraph.Taxes.Current = invoiceGraph.Taxes.Select();
                                invoiceGraph.Taxes.SetValueExt<ARTaxTran.curyTaxAmt>(invoiceGraph.Taxes.Current, soTax.CuryTaxAmt);
                                invoiceGraph.Taxes.Cache.MarkUpdated(invoiceGraph.Taxes.Current);
                                // setting Document
                                invoiceGraph.Document.SetValueExt<ARInvoice.curyTaxTotal>(invoiceGraph.Document.Current, soTax.CuryTaxAmt);
                                invoiceGraph.Document.SetValueExt<ARInvoice.curyDocBal>(invoiceGraph.Document.Current, invoiceGraph.Document.Current.CuryDocBal + (soTax.CuryTaxAmt ?? 0));
                                invoiceGraph.Document.SetValueExt<ARInvoice.curyOrigDocAmt>(invoiceGraph.Document.Current, invoiceGraph.Document.Current.CuryOrigDocAmt + (soTax.CuryTaxAmt ?? 0));
                                invoiceGraph.Document.Update(invoiceGraph.Document.Current);
                            }
                            // Save
                            invoiceGraph.Save.Press();
                            // Release Invoice
                            invoiceGraph.releaseFromCreditHold.Press();
                            invoiceGraph.release.Press();
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
                    if (!string.IsNullOrEmpty(row.ErrorMessage))
                        PXProcessing.SetError(row.ErrorMessage);
                    baseGraph.ShopifyTransaction.Update(row);
                    // Save 
                    baseGraph.Actions.PressSave();
                }
            }
        }

        /// <summary> 邏輯檢核 </summary>
        public void Validation(LUMShopifyTransData row)
        {
            // Valid Financial Stauts
            if (row.FinancialStatus.ToLower() != "paid")
                throw new Exception("Financial Stauts is not equal Paid!!");
        }

        #endregion
    }
}
