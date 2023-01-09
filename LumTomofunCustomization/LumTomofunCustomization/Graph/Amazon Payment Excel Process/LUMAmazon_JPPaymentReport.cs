using LumTomofunCustomization.LUMLibrary;
using LUMTomofunCustomization.DAC;
using PX.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.Graph
{
    public class LUMAmazon_JPPaymentUploadProcess : PXGraph<LUMAmazon_JPPaymentUploadProcess>, PXImportAttribute.IPXPrepareItems
    {
        public PXSave<LUMAmazonJPPaymentReport> Save;
        public PXCancel<LUMAmazonJPPaymentReport> Cancel;

        public PXFilter<AmazonPaymentUploadFileter> Filter;
        [PXImport(typeof(LUMAmazonJPPaymentReport))]
        public PXProcessing<LUMAmazonJPPaymentReport> PaymentTransactions;

        protected virtual IEnumerable filter()
        {
            AmazonPaymentUploadFileter filter = Filter.Current;
            int startRow = 0;
            int totalRows = 0;
            if (filter != null)
            {
                filter.ApiTotal = 0; //reset total to zero 
                var cmd = PaymentTransactions.View.BqlSelect.AggregateNew<Aggregate<Sum<LUMAmazonJPPaymentReport.api_total>>>();// add aggregation to the existing BQL
                foreach (LUMAmazonJPPaymentReport row in
                        new PXView(this, false, cmd).Select(null, //pass filter context to the grid view delegate
                        null,
                        null,
                        null,
                        null,
                        PaymentTransactions.View.GetExternalFilters(), //Get grid user filters
                        ref startRow,
                        0, //get all records without regard to paging
                        ref totalRows))
                {
                    filter.ApiTotal += row.Api_total ?? 0;
                }
            }
            yield return filter;
        }

        public LUMAmazon_JPPaymentUploadProcess()
        {
            this.PaymentTransactions.Cache.AllowInsert = this.PaymentTransactions.Cache.AllowUpdate = this.PaymentTransactions.Cache.AllowDelete = true;
            #region Set Field Enable
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.reportDateTime>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.settlementid>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.reportType>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.orderID>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.sku>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.description>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.productSales>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.productSalesTax>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.shippingCredits>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.shippingCreditsTax>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.giftWrapCredits>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.giftWrapCreditsTax>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.promotionalRebates>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.promotionalRebatesTax>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.marketplaceWithheldTax>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.sellingFees>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.fbafees>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.otherTransactionFee>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.otherFee>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.total>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.cOD>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.cODFee>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.cODItemCharge>(PaymentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonJPPaymentReport.points>(PaymentTransactions.Cache, null, true);
            #endregion
            this.PaymentTransactions.SetProcessDelegate(delegate (List<LUMAmazonJPPaymentReport> list)
            {
                GoProcessing(list);
            });
        }

        #region Action
        public PXAction<LUMAmazonJPPaymentReport> deleteRecord;
        [PXButton]
        [PXUIField(DisplayName = "Delete All Payment", MapEnableRights = PXCacheRights.Delete, MapViewRights = PXCacheRights.Delete)]
        public virtual IEnumerable DeleteRecord(PXAdapter adapter)
        {
            WebDialogResult result = this.PaymentTransactions.Ask(ActionsMessages.Warning, PXMessages.LocalizeFormatNoPrefix("Do you want to delete data?"),
                MessageButtons.OKCancel, MessageIcon.Warning, true);
            if (result != WebDialogResult.OK)
                return adapter.Get();

            PXDatabase.Delete<LUMAmazonJPPaymentReport>();
            this.PaymentTransactions.Cache.Clear();
            return adapter.Get();
        }

        #endregion

        #region Event

        public virtual void _(Events.RowInserted<LUMAmazonJPPaymentReport> e)
        {
            var row = e.Row as LUMAmazonJPPaymentReport;
            #region API Field Binding
            row.API_Marketplace = "JP";
            var CultureName = "ja-JP";
            row.Api_date_1 = AmazonPublicFunction.DatetimeParseWithCulture(CultureName, row.ReportDateTime);
            row.Api_date = AmazonPublicFunction.DatetimeParseWithCulture(CultureName, row.ReportDateTime);
            row.Api_settlementid = row?.Settlementid;
            row.Api_trantype = AmazonPublicFunction.AmazonOrderTypeTreanslate(row.API_Marketplace, row?.ReportType);
            row.Api_orderid = row?.OrderID;
            row.Api_sku = row?.Sku;
            row.Api_description = row?.Description;
            row.Api_productsales = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.ProductSales) ? "0" : row.ProductSales);
            row.Api_producttax = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.ProductSalesTax) ? "0" : row?.ProductSalesTax);
            row.Api_shipping = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.ShippingCredits) ? "0" : row?.ShippingCredits);
            row.Api_shippingtax = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.ShippingCreditsTax) ? "0" : row?.ShippingCreditsTax);
            row.Api_giftwrap = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.GiftWrapCredits) ? "0" : row?.GiftWrapCredits);
            row.Api_giftwraptax = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.GiftWrapCreditsTax) ? "0" : row?.GiftWrapCreditsTax);
            row.Api_regulatoryfee = 0;
            row.Api_taxonregulatoryfee = 0;
            row.Api_promotion = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.PromotionalRebates) ? "0" : row?.PromotionalRebates);
            row.Api_promotiontax = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.PromotionalRebatesTax) ? "0" : row?.PromotionalRebatesTax);
            row.Api_whtax = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.MarketplaceWithheldTax) ? "0" : row?.MarketplaceWithheldTax);
            row.Api_sellingfee = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.SellingFees) ? "0" : row?.SellingFees);
            row.Api_fbafee = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.Fbafees) ? "0" : row?.Fbafees);
            row.Api_othertranfee = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.OtherTransactionFee) ? "0" : row?.OtherTransactionFee);
            row.Api_otherfee = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.OtherFee) ? "0" : row?.OtherFee);
            row.Api_total = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.Total) ? "0" : row?.Total);
            row.Api_cod = 0;
            row.Api_codfee = 0;
            if ((row?.Api_orderid?.StartsWith("S") ?? false) && row?.Api_total == row?.Api_otherfee && (Math.Abs(row.Api_total ?? 0) >= 10000 && Math.Abs(row.Api_total ?? 0) <= 90000))
                row.Api_coditemcharge = row.Api_total;
            else
                row.Api_coditemcharge = 0;
            row.Api_points = AmazonPublicFunction.CurrencyConvertWithCulture(CultureName, string.IsNullOrEmpty(row?.Points) ? "0" : row?.Points);

            #endregion
        }

        #endregion

        #region Method
        public static void GoProcessing(List<LUMAmazonJPPaymentReport> list)
        {
            var baseGraph = CreateInstance<LUMAmazon_JPPaymentUploadProcess>();
            baseGraph.CreatePaymentByOrder(baseGraph, list);
        }

        /// <summary> 執行 Process Amazon payment </summary>
        public virtual void CreatePaymentByOrder(LUMAmazon_JPPaymentUploadProcess baseGraph, List<LUMAmazonJPPaymentReport> selectedList)
        {
            foreach (var selectedItem in selectedList)
            {
                // Initial variable
                string errorMessge = string.Empty;
                // Setting Process Current item
                PXProcessing.SetCurrentItem(selectedItem);
                try
                {
                    // Skip 
                    if (selectedItem.IsProcessed ?? false)
                        continue;
                    using (PXTransactionScope sc = new PXTransactionScope())
                    {
                        AmazonToAcumaticaCore<LUMAmazonJPPaymentReport>.CreatePayment(selectedItem, selectedItem.API_Marketplace, baseGraph);
                        sc.Complete();
                    }
                }
                catch (PXOuterException outerException)
                {
                    for (int i = 0; i < outerException.InnerFields.Length; i++)
                        errorMessge += $"{outerException.InnerFields[i]} - {outerException.InnerMessages[i]} \r\n";
                }
                catch (Exception ex)
                {
                    errorMessge = ex.Message;
                }
                finally
                {
                    // Setting record information
                    selectedItem.ErrorMessage = errorMessge.Length > 2048 ? errorMessge.Substring(0, 2048) : errorMessge; ;
                    selectedItem.IsProcessed = string.IsNullOrEmpty(errorMessge);
                    baseGraph.PaymentTransactions.Update(selectedItem);
                    baseGraph.Actions.PressSave();
                    // Setting Process information
                    if (!string.IsNullOrEmpty(errorMessge))
                        PXProcessing.SetError(errorMessge);
                    else
                        PXProcessing.SetProcessed();
                }
            }
        }

        #endregion

        public bool PrepareImportRow(string viewName, IDictionary keys, IDictionary values)
            => true;

        public void PrepareItems(string viewName, IEnumerable items) { }

        public bool RowImported(string viewName, object row, object oldRow)
            => true;

        public bool RowImporting(string viewName, object row)
            => true;
    }
}
