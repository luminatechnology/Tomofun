using LUMTomofunCustomization.DAC;
using Newtonsoft.Json;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.IN;
using PX.Objects.AR;
using PX.Objects.CM;

namespace LumTomofunCustomization.Graph
{
    public class LUMAmazonTransactionProcess : PXGraph<LUMAmazonTransactionProcess>
    {
        public PXSave<LUMAmazonTransData> Save;
        public PXCancel<LUMAmazonTransData> Cancel;
        public PXProcessing<LUMAmazonTransData> AmazonTransaction;

        public LUMAmazonTransactionProcess()
        {
            AmazonTransaction.Cache.AllowUpdate = true;
            AmazonTransaction.SetProcessDelegate(delegate (List<LUMAmazonTransData> list)
            {
                GoProcessing(list);
            });
        }

        #region Method

        /// <summary> 執行Process </summary>
        public static void GoProcessing(List<LUMAmazonTransData> list)
        {
            var graph = CreateInstance<LUMAmazonTransactionProcess>();
            graph.CreateSalesOrder(list);
        }

        /// <summary> Create Sales Order </summary>
        public virtual void CreateSalesOrder(List<LUMAmazonTransData> amazonList)
        {
            PXUIFieldAttribute.SetEnabled<LUMAmazonTransData.isProcessed>(AmazonTransaction.Cache, null, true);
            foreach (var row in amazonList)
            {
                try
                {
                    using (PXTransactionScope sc = new PXTransactionScope())
                    {
                        var soGraph = PXGraph.CreateInstance<SOOrderEntry>();
                        // validation
                        Validation(row);
                        // Amazon Order Object
                        var amzOrder = JsonConvert.DeserializeObject<LumTomofunCustomization.API_Entity.AmazonOrder.Order>(row.TransJson);
                        // Amazon Total Tax Amount
                        var amzTotalTax = (decimal?)amzOrder.Items.Sum(x => x.ItemTaxAmount - x.PromotionDiscountTaxAmount + x.GiftWrapTaxAmount + (x.ShippingPriceAmount - x.ShippingDiscountAmount == 0 ? 0 : x.ShippingTaxAmount));

                        #region Create Sales Order Header(Header/Contact/Address/Tax)

                        SOOrder order = soGraph.Document.Cache.CreateInstance() as SOOrder;
                        order.OrderType = "FA";
                        order.OrderDate = CalculateAmazonDateTime(amzOrder.PurchaseDate);
                        order.RequestDate = CalculateAmazonDateTime(amzOrder.LatestShipDate);
                        order.CustomerID = GetMarketplaceCustomer(row.Marketplace);
                        order.OrderDesc = $"Amazon Order ID: {amzOrder.OrderId}";
                        order.CustomerOrderNbr = amzOrder.OrderId;
                        // Testing(?)
                        order.TermsID = "0000";
                        #region User-Defined
                        // UserDefined - ORDERTYPE
                        soGraph.Document.Cache.SetValueExt(order, PX.Objects.CS.Messages.Attribute + "ORDERTYPE", amzOrder.OrderType);
                        // UserDefined - MKTPLACE
                        soGraph.Document.Cache.SetValueExt(order, PX.Objects.CS.Messages.Attribute + "MKTPLACE", amzOrder.SalesChannel);
                        // UserDefined - ORDERAMT
                        soGraph.Document.Cache.SetValueExt(order, PX.Objects.CS.Messages.Attribute + "ORDERAMT", amzOrder.Amount);
                        // UserDefined - ORDTAAMT
                        soGraph.Document.Cache.SetValueExt(order, PX.Objects.CS.Messages.Attribute + "ORDTAXAMT", amzTotalTax);
                        // UserDefined - TAXCOLLECT
                        soGraph.Document.Cache.SetValueExt(order, PX.Objects.CS.Messages.Attribute + "TAXCOLLECT", row.Marketplace == "US" ? amzTotalTax : 0);
                        #endregion
                        // Insert SOOrder
                        soGraph.Document.Insert(order);
                        // Setting Shipping_Address
                        var soAddress = soGraph.Shipping_Address.Current;
                        soAddress.OverrideAddress = true;
                        soAddress.PostalCode = amzOrder.PostalCode;
                        soAddress.CountryID = amzOrder.CountryCode;
                        soAddress.State = amzOrder.StateOrRegion;
                        soAddress.City = amzOrder.City;
                        soAddress.RevisionID = 1;
                        // Setting Shipping_Contact
                        var soContact = soGraph.Shipping_Contact.Current;
                        soContact.OverrideContact = true;
                        soContact.Email = amzOrder.BuyerEmail;
                        soContact.RevisionID = 1;
                        #endregion

                        #region Set Currency
                        CurrencyInfo info = CurrencyInfoAttribute.SetDefaults<SOOrder.curyInfoID>(soGraph.Document.Cache, soGraph.Document.Current);
                        if (info != null)
                            soGraph.Document.Cache.SetValueExt<SOOrder.curyID>(soGraph.Document.Current, info.CuryID);
                        #endregion

                        #region Create Sales Line

                        foreach (var item in amzOrder.Items)
                        {
                            // Sales Order Line
                            var line = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                            line.InventoryID = GetInvetoryitemID(soGraph, item.SellerSKU);
                            if (line.InventoryID == null)
                                throw new Exception("can not find Inventory item ID");
                            line.ManualPrice = true;
                            line.OrderQty = item.QuantityShipped;
                            line.CuryUnitPrice =
                                (row.Marketplace == "US" || row.Marketplace == "CA") ?
                                 (decimal?)(item.ItemPriceAmount - item.PromotionDiscountAmount) / item.QuantityShipped :
                                 (decimal?)(item.ItemPriceAmount - item.PromotionDiscountAmount - item.ItemTaxAmount - item.GiftWrapTaxAmount + item.PromotionDiscountTaxAmount - (item.ShippingPriceAmount - item.ShippingDiscountAmount == 0 ? 0 : item.ShippingTaxAmount / item.QuantityShipped));
                            soGraph.Transactions.Insert(line);
                            // Non-stock Item(Shipping) 
                            if (item.ShippingPriceAmount - item.ShippingDiscountAmount != 0)
                            {
                                var soShipLine = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                soShipLine.InventoryID = GetFeeNonStockItem("Shipping");
                                soShipLine.OrderQty = 1;
                                soShipLine.CuryUnitPrice = (decimal?)(item.ShippingPriceAmount - item.ShippingDiscountAmount);
                                soGraph.Transactions.Insert(soShipLine);
                            }

                            // Non-stock Item(Giftwrap)
                            if (item.GiftWrapPriceAmount != 0)
                            {
                                var soGiftLine = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                soGiftLine.InventoryID = GetFeeNonStockItem("Giftwrap");
                                soGiftLine.OrderQty = 1;
                                soGiftLine.CuryUnitPrice = (decimal?)item.GiftWrapPriceAmount;
                                soGraph.Transactions.Insert(soGiftLine);
                            }
                        }

                        #endregion

                        #region Update Tax
                        // Setting SO Tax
                        soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, row.Marketplace + "EC");
                        soGraph.Taxes.Cache.SetValueExt<SOTaxTran.curyTaxAmt>(soGraph.Taxes.Current, row.Marketplace == "US" ? 0 : amzTotalTax);

                        soGraph.Document.Cache.SetValueExt<SOOrder.curyTaxTotal>(soGraph.Document.Current, row.Marketplace == "US" ? 0 : amzTotalTax);
                        soGraph.Document.Cache.SetValueExt<SOOrder.curyOrderTotal>(soGraph.Document.Current, (soGraph.Document.Current?.CuryOrderTotal ?? 0) + (row.Marketplace == "US" ? 0 : amzTotalTax));
                        #endregion

                        soGraph.Save.Press();

                        // Prepare Invoice
                        try
                        {
                            var newAdapter = new PXAdapter(soGraph.Document)
                            {
                                Searches = new Object[]
                                {
                                    soGraph.Document.Current.OrderType,
                                    soGraph.Document.Current.OrderNbr
                                }
                            };
                            soGraph.PrepareInvoice(newAdapter);
                        }
                        // Prepare Invoice Success
                        catch (PXRedirectRequiredException ex)
                        {
                            #region Override Invoice Tax
                            // Invoice Graph
                            SOInvoiceEntry invoiceGraph = ex.Graph as SOInvoiceEntry;
                            var soTax = SelectFrom<SOTaxTran>
                                        .Where<SOTaxTran.orderNbr.IsEqual<P.AsString>
                                             .And<SOTaxTran.orderType.IsEqual<P.AsString>>>
                                        .View.SelectSingleBound(this, null, soGraph.Document.Current.OrderNbr, soGraph.Document.Current.OrderType)
                                        .TopFirst;
                            // update docDate
                            invoiceGraph.Document.SetValueExt<ARInvoice.docDate>(invoiceGraph.Document.Current, order.RequestDate);
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
                                // Save
                                invoiceGraph.Save.Press();
                                // Release Invoice
                                invoiceGraph.releaseFromCreditHold.Press();
                                // invoiceGraph.release.Press();
                            }
                            #endregion
                        }
                        row.IsProcessed = true;
                        row.ErrorMessage = string.Empty;
                        this.AmazonTransaction.Update(row);
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
                    if (!string.IsNullOrEmpty(row.ErrorMessage))
                        PXProcessing.SetError(row.ErrorMessage);
                    this.AmazonTransaction.Update(row);
                }
            }
            this.Actions.PressSave();
        }

        /// <summary> 邏輯檢核 </summary>
        public void Validation(LUMAmazonTransData row)
        {
            // Valid Order Stauts
            if (row.OrderStatus != "Shipped")
                throw new Exception("Order Stauts is not equal Shipped!!");
            // Valid SalesChannel
            if (!row.SalesChannel.ToLower().StartsWith("amazon"))
                throw new Exception("SalesChannel must starts with 'Amazon'!");
        }

        /// <summary> 轉換Amazon時間格式 </summary>
        public DateTime CalculateAmazonDateTime(int? amazonDate)
            => DateTime.FromOADate(((amazonDate.Value + 8 * 3600) / 86400 + 70 * 365 + 19));

        /// <summary> 取Marketplace 對應 Customer ID </summary>
        public int? GetMarketplaceCustomer(string marketPlace)
            => SelectFrom<LUMMarketplacePreference>
               .Where<LUMMarketplacePreference.marketplace.IsEqual<P.AsString>>
               .View.Select(this, marketPlace).TopFirst?.BAccountID;

        /// <summary> 取Fee 對應 Non-Stock item ID </summary>
        public int? GetFeeNonStockItem(string fee)
            => SelectFrom<LUMMarketplaceFeePreference>
               .Where<LUMMarketplaceFeePreference.fee.IsEqual<P.AsString>>
               .View.Select(this, fee).TopFirst?.InventoryID;

        /// <summary> 取Inventory Item ID </summary>
        public int? GetInvetoryitemID(PXGraph graph, string sku)
            => InventoryItem.UK.Find(graph, sku)?.InventoryID ??
               SelectFrom<INItemXRef>
               .Where<INItemXRef.alternateID.IsEqual<P.AsString>>
               .View.SelectSingleBound(graph, null, sku).TopFirst?.InventoryID;

        #endregion
    }
}
