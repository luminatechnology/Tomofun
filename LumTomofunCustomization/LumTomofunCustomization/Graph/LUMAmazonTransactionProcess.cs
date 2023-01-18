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
        public SelectFrom<LUMAmazonFulfillmentTransData>.View FulfillmentTransactions;

        public LUMAmazonTransactionProcess()
        {
            AmazonTransaction.Cache.AllowUpdate = true;
            AmazonTransaction.SetProcessDelegate(delegate (List<LUMAmazonTransData> list)
            {
                GoProcessing(list);
            });
        }

        #region Event

        public virtual void _(Events.RowSelected<LUMAmazonTransData> e)
        {
            var row = e.Row;
            if (row != null && !row.Amount.HasValue && !string.IsNullOrEmpty(row.TransJson))
            {
                try
                {
                    var sourceData = JsonConvert.DeserializeObject<LumTomofunCustomization.API_Entity.AmazonOrder.Order>(row.TransJson);
                    row.Amount = (decimal?)sourceData.Amount;
                }
                catch (Exception) { }
            }
        }

        #endregion

        #region Method

        /// <summary> 執行Process </summary>
        public static void GoProcessing(List<LUMAmazonTransData> list)
        {
            var graph = CreateInstance<LUMAmazonTransactionProcess>();
            graph.CreateSalesOrder(graph, list);
        }

        /// <summary> Create Sales Order </summary>
        public virtual void CreateSalesOrder(LUMAmazonTransactionProcess baseGraph, List<LUMAmazonTransData> amazonList)
        {
            PXUIFieldAttribute.SetEnabled<LUMAmazonTransData.isProcessed>(AmazonTransaction.Cache, null, true);
            DateTime? amzFulfillmentDate = null;
            foreach (var row in amazonList)
            {
                decimal? systemTax = (decimal)0;
                var invType = string.Empty;
                var invNbr = string.Empty;
                PXProcessing.SetCurrentItem(row);
                try
                {
                    using (PXTransactionScope sc = new PXTransactionScope())
                    {
                        var soGraph = PXGraph.CreateInstance<SOOrderEntry>();
                        // validation
                        Validation(row);
                        // Marketplace tax calculation
                        var isTaxCalculate = GetMarketplaceTaxCalculation(row.Marketplace);
                        // Amazon Order Object
                        var amzOrder = JsonConvert.DeserializeObject<LumTomofunCustomization.API_Entity.AmazonOrder.Order>(row.TransJson);
                        var marketplacePreference = SelectFrom<LUMMarketplacePreference>
                            .Where<LUMMarketplacePreference.marketplace.IsEqual<P.AsString>>
                            .View.Select(baseGraph, row.Marketplace).TopFirst;
                        // CreatedDatetime 和 LatestShipDate 取最早的
                        try
                        {
                            amzFulfillmentDate = amzOrder?.LatestShipDate == null || row.CreatedDateTime < CalculateAmazonDateTime(amzOrder?.LatestShipDate) ? row.CreatedDateTime : CalculateAmazonDateTime(amzOrder?.LatestShipDate);
                        }
                        catch (Exception)
                        {
                            amzFulfillmentDate = row.CreatedDateTime;
                        }
                        // Fulfillment date < 2022/07/01
                        if (amzFulfillmentDate < new DateTime(2022, 06, 01))
                            throw new Exception("Legacy Order");

                        // Amazon Total Tax Amount
                        var amzTotalTax = (decimal?)amzOrder.Items.Sum(x => x.ItemTaxAmount - x.PromotionDiscountTaxAmount + x.GiftWrapTaxAmount + (x.ShippingPriceAmount - x.ShippingDiscountAmount == 0 ? 0 : x.ShippingTaxAmount));

                        var oldsoOrder = SelectFrom<SOOrder>
                                         .Where<SOOrder.orderType.IsEqual<P.AsString>
                                            .And<SOOrder.customerOrderNbr.IsEqual<P.AsString>>>
                                         .View.Select(baseGraph, "FA", amzOrder.OrderId).TopFirst;
                        // 如果之前沒有Create 過SOOrder 則Create
                        if (oldsoOrder == null || row.TransactionType == "RE-Amazon Orders")
                        {
                            #region Create Sales Order Header(Header/Contact/Address/Tax)
                            SOOrder order = soGraph.Document.Cache.CreateInstance() as SOOrder;
                            order.OrderType = "FA";
                            order.OrderDate = CalculateAmazonDateTime(amzOrder.PurchaseDate).AddHours(marketplacePreference?.TimeZone ?? 0);
                            order.RequestDate = amzFulfillmentDate ?? CalculateAmazonDateTime(amzOrder.LatestShipDate);
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

                            var defaultAddress = SelectFrom<PX.Objects.CR.Address>
                                                 .Where<PX.Objects.CR.Address.bAccountID.IsEqual<P.AsInt>>
                                                 .View.SelectSingleBound(baseGraph, null, order.CustomerID).TopFirst;

                            soAddress.OverrideAddress = true;
                            soAddress.PostalCode = amzOrder.PostalCode;
                            soAddress.CountryID = string.IsNullOrEmpty(amzOrder.CountryCode) ? defaultAddress?.CountryID : amzOrder.CountryCode; ;
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
                                CreateSOLine(soGraph, amzOrder, item, row);

                            #endregion

                            #region Update Tax
                            // Setting SO Tax
                            if (!isTaxCalculate)
                            {
                                systemTax = soGraph.Taxes.Current?.CuryTaxAmt ?? 0;
                                soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, row.Marketplace + "EC");
                                soGraph.Taxes.Cache.SetValueExt<SOTaxTran.curyTaxAmt>(soGraph.Taxes.Current, amzTotalTax);

                                soGraph.Document.Cache.SetValueExt<SOOrder.curyTaxTotal>(soGraph.Document.Current, amzTotalTax);
                                soGraph.Document.Cache.SetValueExt<SOOrder.curyOrderTotal>(soGraph.Document.Current, (soGraph.Document.Current?.CuryOrderTotal ?? 0) + amzTotalTax - systemTax);
                            }
                            #endregion

                            // Write json into note
                            PXNoteAttribute.SetNote(soGraph.Document.Cache, soGraph.Document.Current, row.TransJson);
                            // Sales Order Save
                            soGraph.Save.Press();
                        }
                        // 如果已經存在 則更改Request date + 刪除所有SOLine 並重新建立
                        else
                        {
                            soGraph.Document.Current = oldsoOrder;
                            // 執行同一張Report 但是還沒出貨，避免override request date
                            if (amzFulfillmentDate.HasValue)
                                soGraph.Document.Cache.SetValueExt<SOOrder.requestDate>(soGraph.Document.Current, amzFulfillmentDate);

                            // 刪除所有SOLine 
                            foreach (SOLine oldLine in soGraph.Transactions.Select().RowCast<SOLine>())
                                soGraph.Transactions.Delete(oldLine);

                            // Save SalesOrder
                            soGraph.Save.Press();

                            // 重新建立SOLine
                            #region Create Sales Line

                            foreach (var item in amzOrder.Items)
                                CreateSOLine(soGraph, amzOrder, item, row);

                            #endregion

                            #region Update Tax
                            // Setting SO Tax
                            if (!isTaxCalculate)
                            {
                                systemTax = soGraph.Taxes.Current?.CuryTaxAmt ?? 0;
                                soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, row.Marketplace + "EC");
                                soGraph.Taxes.Cache.SetValueExt<SOTaxTran.curyTaxAmt>(soGraph.Taxes.Current, amzTotalTax);

                                soGraph.Document.Cache.SetValueExt<SOOrder.curyTaxTotal>(soGraph.Document.Current, amzTotalTax);
                                soGraph.Document.Cache.SetValueExt<SOOrder.curyOrderTotal>(soGraph.Document.Current, (soGraph.Document.Current?.CuryOrderTotal ?? 0) + amzTotalTax - systemTax);
                            }
                            #endregion

                            // Save SalesOrder
                            soGraph.Save.Press();
                        }
                        // 有Fulfillment date 才Prepare invoice
                        if (row.OrderStatus?.ToUpper() == "SHIPPED")
                        {
                            // Prepare Invoice
                            try
                            {
                                soGraph.prepareInvoice.Press();
                            }
                            // Prepare Invoice Success
                            catch (PXRedirectRequiredException ex)
                            {
                                #region Override Invoice Tax
                                // Invoice Graph
                                SOInvoiceEntry invoiceGraph = ex.Graph as SOInvoiceEntry;
                                invType = invoiceGraph.Document.Current?.DocType;
                                invNbr = invoiceGraph.Document.Current?.RefNbr;
                                var soTax = SelectFrom<SOTaxTran>
                                            .Where<SOTaxTran.orderNbr.IsEqual<P.AsString>
                                                 .And<SOTaxTran.orderType.IsEqual<P.AsString>>>
                                            .View.SelectSingleBound(this, null, soGraph.Document.Current.OrderNbr, soGraph.Document.Current.OrderType)
                                            .TopFirst;
                                // update invoice Date
                                invoiceGraph.Document.SetValueExt<ARInvoice.invoiceDate>(invoiceGraph.Document.Current, amzFulfillmentDate);
                                // update invoice tax
                                if (soTax != null && !isTaxCalculate)
                                {
                                    // setting Tax
                                    invoiceGraph.Taxes.Current = invoiceGraph.Taxes.Select();
                                    invoiceGraph.Taxes.SetValueExt<ARTaxTran.curyTaxAmt>(invoiceGraph.Taxes.Current, soTax.CuryTaxAmt);
                                    invoiceGraph.Taxes.Cache.MarkUpdated(invoiceGraph.Taxes.Current);
                                    // setting Document
                                    invoiceGraph.Document.SetValueExt<ARInvoice.curyTaxTotal>(invoiceGraph.Document.Current, soTax.CuryTaxAmt);
                                    invoiceGraph.Document.SetValueExt<ARInvoice.curyDocBal>(invoiceGraph.Document.Current, invoiceGraph.Document.Current.CuryDocBal + (soTax.CuryTaxAmt ?? 0) - systemTax);
                                    invoiceGraph.Document.SetValueExt<ARInvoice.curyOrigDocAmt>(invoiceGraph.Document.Current, invoiceGraph.Document.Current.CuryOrigDocAmt + (soTax.CuryTaxAmt ?? 0) - systemTax);
                                    invoiceGraph.Document.Update(invoiceGraph.Document.Current);
                                }
                                // Save
                                invoiceGraph.Save.Press();
                                // Release Invoice
                                invoiceGraph.releaseFromCreditHold.Press();
                                // Release 失敗不Rollback
                                //invoiceGraph.release.Press();
                                #endregion
                            }
                        }

                        row.IsProcessed = true;
                        row.ErrorMessage = amzFulfillmentDate.HasValue ? string.Empty : "Can not find Fulfillment report";

                        // Update Fulfilment report Processed
                        PXDatabase.Update<LUMAmazonFulfillmentTransData>(
                            new PXDataFieldAssign<LUMAmazonFulfillmentTransData.isProcessed>(true),
                            new PXDataFieldRestrict<LUMAmazonFulfillmentTransData.amazonOrderID>(amzOrder.OrderId));
                        sc.Complete();
                    }
                }
                catch (PXOuterException ex)
                {
                    row.ErrorMessage = ex.InnerMessages[0];
                    row.IsProcessed = false;
                }
                catch (Exception ex)
                {
                    row.ErrorMessage = ex.Message;
                    row.IsProcessed = false;
                }
                finally
                {
                    if (!string.IsNullOrEmpty(row.ErrorMessage) && row.ErrorMessage != "Can not find Fulfillment report")
                        PXProcessing.SetError(row.ErrorMessage);
                    baseGraph.AmazonTransaction.Update(row);
                    // Save
                    baseGraph.Actions.PressSave();
                }
                // 建立Invoice後，在Release。即使失敗也照常產生
                if (amzFulfillmentDate.HasValue && (row?.IsProcessed ?? false))
                {
                    try
                    {
                        var invGraph = PXGraph.CreateInstance<SOInvoiceEntry>();
                        invGraph.Document.Current = invGraph.Document.Search<ARInvoice.docType, ARInvoice.refNbr>(invType, invNbr);
                        if (invGraph.Document.Current != null)
                            invGraph.release.Press();
                    }
                    catch (Exception)
                    {
                        // Do nothing
                    }
                }
            }
        }

        public void CreateSOLine(SOOrderEntry soGraph, LumTomofunCustomization.API_Entity.AmazonOrder.Order amzOrder, LumTomofunCustomization.API_Entity.AmazonOrder.Item item, LUMAmazonTransData row)
        {
            // Sales Order Line
            var line = soGraph.Transactions.Cache.CreateInstance() as SOLine;
            line.InventoryID = GetInvetoryitemID(soGraph, item.SellerSKU);
            if (line.InventoryID == null)
                throw new Exception($"can not find Inventory item ID ({item.SellerSKU})");
            line.ManualPrice = true;
            line.OrderQty = amzOrder.OrderStatus == "Shipped" ? item?.QuantityShipped : item?.QuantityOrdered;
            if ((item?.QuantityShipped ?? 0) == 0)
                line.CuryUnitPrice =
                    (row.Marketplace == "US" || row.Marketplace == "CA" || row.Marketplace == "MX") ?
                    (decimal?)item.ItemPriceAmount / item.QuantityOrdered :
                    (decimal?)((item.ItemPriceAmount - item.ItemTaxAmount) / item.QuantityOrdered);
            else
                line.CuryUnitPrice =
                     (row.Marketplace == "US" || row.Marketplace == "CA" || row.Marketplace == "MX") ?
                     (decimal?)item.ItemPriceAmount / item.QuantityShipped :
                     (decimal?)((item.ItemPriceAmount - item.ItemTaxAmount) / item.QuantityShipped);
            line.CuryDiscAmt =
                (row.Marketplace == "US" || row.Marketplace == "CA" || row.Marketplace == "MX") ?
                (decimal?)item.PromotionDiscountAmount :
                row.Marketplace == "JP" ? (decimal)item?.PromotionDiscountAmount - (decimal)item?.PromotionDiscountTaxAmount :
                (decimal?)(item.PromotionDiscountAmount + item.PromotionDiscountTaxAmount);
            soGraph.Transactions.Insert(line);
            // Non-stock Item(Shipping) 
            if (item.ShippingPriceAmount - item.ShippingDiscountAmount != 0)
            {
                var soShipLine = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                soShipLine.InventoryID = GetFeeNonStockItem("Shipping");
                soShipLine.OrderQty = 1;
                soShipLine.CuryUnitPrice =
                    (row.Marketplace == "US" || row.Marketplace == "CA" || row.Marketplace == "MX") ?
                    (decimal?)(item.ShippingPriceAmount - item.ShippingDiscountAmount) :
                    (decimal?)(item.ShippingPriceAmount - item.ShippingDiscountAmount - (item.ShippingPriceAmount - item.ShippingDiscountAmount == 0 ? 0 : item.ShippingTaxAmount));
                soGraph.Transactions.Insert(soShipLine);
            }

            // Non-stock Item(Giftwrap)
            if (item.GiftWrapPriceAmount != 0)
            {
                var soGiftLine = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                soGiftLine.InventoryID = GetFeeNonStockItem("Giftwrap");
                soGiftLine.OrderQty = 1;
                soGiftLine.CuryUnitPrice =
                    (row.Marketplace == "US" || row.Marketplace == "CA" || row.Marketplace == "MX") ?
                    (decimal?)item.GiftWrapPriceAmount :
                    (decimal?)(item.GiftWrapPriceAmount - item.GiftWrapTaxAmount);
                soGraph.Transactions.Insert(soGiftLine);
            }
        }

        /// <summary> 邏輯檢核 </summary>
        public void Validation(LUMAmazonTransData row)
        {
            // Valid OrderStatus
            if (row.OrderStatus.ToLower() == "canceled")
                throw new Exception("Order Status can not equal Canceled");
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

        /// <summary> 取Marketplace 對應 Tax Calculation </summary>
        public bool GetMarketplaceTaxCalculation(string marketPlace)
            => SelectFrom<LUMMarketplacePreference>
               .Where<LUMMarketplacePreference.marketplace.IsEqual<P.AsString>>
               .View.Select(this, marketPlace).TopFirst?.IsTaxCalculation ?? false;

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

        public DateTime? GetFulfillmentDate(LUMAmazonTransactionProcess baseGraph, string amazonOrderId, LUMMarketplacePreference marketplacePreference)
        {
            var data = SelectFrom<LUMAmazonFulfillmentTransData>
                       .Where<LUMAmazonFulfillmentTransData.amazonOrderID.IsEqual<P.AsString>>
                       .View.Select(baseGraph, amazonOrderId).TopFirst;
            return data == null ? null : data.ShipmentDate?.AddHours(marketplacePreference?.TimeZone ?? 0);
        }
        #endregion
    }
}
