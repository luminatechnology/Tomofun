using FikaAmazonAPI;
using FikaAmazonAPI.ReportGeneration;
using FikaAmazonAPI.Utils;
using LumTomofunCustomization.LUMLibrary;
using LUMTomofunCustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.SO;
using PX.Objects.CM;
using PX.Objects.CR;
using PX.Data.BQL;
using PX.Objects.SO.GraphExtensions.SOOrderEntryExt;
using PX.Objects.AR.GraphExtensions;

namespace LumTomofunCustomization.Graph
{
    public class LUMAmazonSettlementTransactionProcess : PXGraph<LUMAmazonSettlementTransactionProcess>
    {
        public PXSave<SettlementFilter> Save;
        public PXCancel<SettlementFilter> Cancel;
        public PXFilter<SettlementFilter> Filter;
        public PXFilteredProcessing<LUMAmazonSettlementTransData, SettlementFilter> SettlementTransaction;
        public SelectFrom<LUMMWSPreference>.View Setup;

        [InjectDependency]
        private ILegacyCompanyService _legacyCompanyService { get; set; }

        public LUMAmazonSettlementTransactionProcess()
        {
            this.SettlementTransaction.AllowUpdate = true;
            var filter = this.Filter.Current;
            SettlementTransaction.SetProcessDelegate(delegate (List<LUMAmazonSettlementTransData> list)
            {
                GoProcessing(list, filter);
            });
            // Initial Data
            if (this.SettlementTransaction.Select().Count == 0)
                InitialData();
        }

        #region Method

        /// <summary> 執行Process </summary>
        public static void GoProcessing(List<LUMAmazonSettlementTransData> list, SettlementFilter filter)
        {
            var baseGraph = CreateInstance<LUMAmazonSettlementTransactionProcess>();
            if (String.IsNullOrEmpty(filter.ProcessType))
                throw new Exception("Process Type can not be empty!");
            baseGraph.DeleteDefaultData();
            switch (filter.ProcessType)
            {
                case "Prepare Data":
                    baseGraph.PreparePaymentData(baseGraph, filter);
                    break;
                case "Process Payment":
                    baseGraph.CreatePaymentByOrder(baseGraph, list, filter);
                    break;
            }
        }

        /// <summary> 執行 Get Amazon Payment Data </summary>
        public virtual void PreparePaymentData(LUMAmazonSettlementTransactionProcess baseGraph, SettlementFilter filter)
        {
            try
            {
                PXUIFieldAttribute.SetEnabled<LUMAmazonSettlementTransData.isProcessed>(baseGraph.SettlementTransaction.Cache, null, true);
                PXUIFieldAttribute.SetEnabled<LUMAmazonSettlementTransData.marketPlaceName>(baseGraph.SettlementTransaction.Cache, null, true);
                var actCompanyName = _legacyCompanyService.ExtractCompany(PX.Common.PXContext.PXIdentity.IdentityName);
                Dictionary<string, AmazonConnection> amzConnObjs = new Dictionary<string, AmazonConnection>();
                // TW Tenant要執行兩次
                if (actCompanyName == "TW")
                {
                    amzConnObjs.Add("EU", GetAmazonConnObject("EU"));
                    amzConnObjs.Add("AU", GetAmazonConnObject("AU"));
                    amzConnObjs.Add("SG", GetAmazonConnObject("SG"));
                }
                else if (actCompanyName == "US")
                {
                    amzConnObjs.Add("US", GetAmazonConnObject("US"));
                    amzConnObjs.Add("MX", GetAmazonConnObject("MX"));
                }
                else
                    amzConnObjs.Add(actCompanyName, GetAmazonConnObject(actCompanyName));
                foreach (var dic in amzConnObjs)
                {
                    ReportManager reportManager = new ReportManager(dic.Value);
                    var SettlementOrders = reportManager.GetSettlementOrderAsync(filter.FromDate == null ? DateTime.Now.AddDays(-1).Date : filter.FromDate.Value.Date,
                                                                               filter.ToDate == null ? DateTime.Now.Date : filter.ToDate.Value.Date).Result;
                    foreach (var item in SettlementOrders)
                    {
                        var trans = baseGraph.SettlementTransaction.Cache.CreateInstance() as LUMAmazonSettlementTransData;
                        trans.Marketplace = dic.Key;
                        trans.SettlementID = item.SettlementId;
                        trans.SettlementStartDate = item.SettlementStartDate;
                        trans.SettlementEndDate = item.SettlementEndDate;
                        trans.DepositDate = item.DepositDate;
                        trans.TotalAmount = item.TotalAmount;
                        trans.DepositDate = item.DepositDate;
                        trans.OrderID = item.OrderId;
                        trans.TransactionType = item.TransactionType;
                        trans.AmountType = item.AmountType;
                        trans.AmountDescription = item.AmountDescription;
                        trans.Amount = item.Amount;
                        trans.PostedDate = item?.PostedDateTime;
                        trans.MarketPlaceName = item.MarketplaceName;
                        trans.MerchantOrderID = item.MerchantOrderId;
                        trans.MerchantOrderItemID = item.MerchantOrderItemId;
                        trans.Sku = item.SKU;
                        trans.QuantityPurchased = item.QuantityPurchased;
                        baseGraph.SettlementTransaction.Insert(trans);
                    }
                }
                baseGraph.Actions.PressSave();
            }
            catch (PXOuterException ex)
            {
                throw new Exception(ex.InnerMessages[0]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary> 執行 Process Amazon payment </summary>
        public virtual void CreatePaymentByOrder(LUMAmazonSettlementTransactionProcess baseGraph, List<LUMAmazonSettlementTransData> amazonList, SettlementFilter filter)
        {
            // 先找DB內所有Settlement id 符合條件的MarketplaceName
            var AllMarketplaceNameList = SelectFrom<LUMAmazonSettlementTransData>
                                        .View.Select(baseGraph)
                                        .RowCast<LUMAmazonSettlementTransData>()
                                        .Where(x => (x?.MarketPlaceName ?? string.Empty).ToUpper().StartsWith("AMAZON")).Select(x => new { x.SettlementID, x.MarketPlaceName }).Distinct();
            // 取Marketplace Preference Data
            var marketplacePreference = SelectFrom<LUMMarketplacePreference>.View.Select(baseGraph).RowCast<LUMMarketplacePreference>();
            // 相同OrderID只會Create一張Payment
            foreach (var amzGroupOrderData in amazonList.GroupBy(x => new { x.Marketplace, x.SettlementID, x.TransactionType, x.OrderID, x.PostedDate, x.MerchantOrderID }))
            {
                PXLongOperation.SetCurrentItem(amzGroupOrderData.FirstOrDefault());
                string errorMsg = string.Empty;
                string DisplayGroupKey = $"{amzGroupOrderData.Key.SettlementID},{amzGroupOrderData.Key.TransactionType},{amzGroupOrderData.Key.OrderID}";
                try
                {
                    #region Setting Marketplace
                    // 找DB相同Settlement ID的MarketPlaceName資料(非non-Amazon)
                    var _marketplace = AllMarketplaceNameList.FirstOrDefault(x => x.SettlementID == amzGroupOrderData.Key.SettlementID)?.MarketPlaceName;
                    if (string.IsNullOrEmpty(_marketplace) || _marketplace?.ToUpper() == "NON-AMAZON")
                    {
                        var refOrderView = new SelectFrom<SOOrder>
                                       .Where<SOOrder.customerOrderNbr.IsEqual<P.AsString>>
                                       .View(baseGraph);
                        var refOrder = refOrderView.Select(amzGroupOrderData.Key.OrderID).TopFirst;
                        if (refOrder == null)
                            throw new Exception($"Settlement Market Place Not Found in Sales order({amzGroupOrderData.Key.OrderID})");
                        _marketplace = GetMarketplaceName((string)((PXFieldState)refOrderView.Cache.GetValueExt(refOrder, PX.Objects.CS.Messages.Attribute + "MKTPLACE")).Value);
                    }
                    // 如果 Maketplace name = SI CA Prod Marketplace -> CA
                    else if (amazonList.FirstOrDefault(x => x.SettlementID == amzGroupOrderData.Key.SettlementID && x.MarketPlaceName == "SI CA Prod Marketplace") != null)
                        _marketplace = "CA";
                    else
                        // 如果是MX但是Marketplace name = us 
                        _marketplace = amzGroupOrderData.Key.Marketplace == "MX" && GetMarketplaceName(_marketplace) == "US" ? "MX" : GetMarketplaceName(_marketplace);

                    if (string.IsNullOrEmpty(_marketplace))
                        throw new Exception("Settlement Market Place Not Found");
                    #endregion

                    var isTaxCalculate = AmazonPublicFunction.GetMarketplaceTaxCalculation(_marketplace);
                    // Recalculete Amount(處理歐洲美些國家金額放大一百倍問題)(CouponRedemptionFee 除外)
                    foreach (var item in amzGroupOrderData)
                    {
                        // 如果CurrentMarketplace is not null 代表該筆資料已經重新計算過Amount, 無須再重新計算
                        if (!string.IsNullOrEmpty(item.CurrentMarketplace))
                            continue;
                        item.CurrentMarketplace = _marketplace;
                        if (item.AmountType != "CouponRedemptionFee")
                            item.Amount /= (marketplacePreference.FirstOrDefault(x => x.Marketplace == _marketplace)?.PaymentFormat ?? 1);
                    }
                    var amzTotalTax = amzGroupOrderData.Where(x => x.AmountDescription == "Tax" || x.AmountDescription == "ShippingTax" || x.AmountDescription == "TaxDiscount" || x.AmountType == "ItemWithheldTax").Sum(x => (x.Amount ?? 0) * -1);

                    using (PXTransactionScope sc = new PXTransactionScope())
                    {
                        switch (amzGroupOrderData.Key.TransactionType.ToUpper())
                        {
                            case "REFUND":
                                #region Transaction Type: Refund

                                var soGraph = PXGraph.CreateInstance<SOOrderEntry>();

                                #region Header
                                var soDoc = soGraph.Document.Cache.CreateInstance() as SOOrder;
                                soDoc.OrderType = "RT";
                                soDoc.CustomerOrderNbr = amzGroupOrderData.Key.OrderID;
                                soDoc.OrderDate = amzGroupOrderData.Key.PostedDate;
                                soDoc.RequestDate = amzGroupOrderData.Key.PostedDate;
                                soDoc.CustomerID = AmazonPublicFunction.GetMarketplaceCustomer(_marketplace);
                                soDoc.OrderDesc = $"Amazon ({amzGroupOrderData.Key.TransactionType}) {amzGroupOrderData.Key.OrderID}";
                                #endregion

                                #region User-Defined
                                // UserDefined - ORDERTYPE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERTYPE", $"Amazon {amzGroupOrderData.Key.TransactionType}");
                                // UserDefined - MKTPLACE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "MKTPLACE", _marketplace);
                                // UserDefined - ORDERAMT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERAMT", amzGroupOrderData.Sum(x => (x.Amount ?? 0) * -1));
                                // UserDefined - ORDTAAMT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDTAXAMT", amzTotalTax);
                                // UserDefined - TAXCOLLECT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "TAXCOLLECT", _marketplace == "US" ? amzTotalTax : 0);
                                #endregion

                                // Insert SOOrder
                                soGraph.Document.Insert(soDoc);

                                #region Set Currency
                                CurrencyInfo info = CurrencyInfoAttribute.SetDefaults<SOOrder.curyInfoID>(soGraph.Document.Cache, soGraph.Document.Current);
                                if (info != null)
                                    soGraph.Document.Cache.SetValueExt<SOOrder.curyID>(soGraph.Document.Current, info.CuryID);
                                #endregion

                                #region Address
                                var soGraph_FA = PXGraph.CreateInstance<SOOrderEntry>();
                                var soOrder_FAInfo = SelectFrom<SOOrder>
                                                 .Where<SOOrder.orderType.IsEqual<P.AsString>
                                                   .And<SOOrder.customerOrderNbr.IsEqual<P.AsString>>>
                                                 .View.SelectSingleBound(soGraph_FA, null, "FA", amzGroupOrderData.Key.OrderID).TopFirst;
                                soGraph_FA.Document.Current = soOrder_FAInfo;
                                if (soGraph_FA.Document.Current != null)
                                {
                                    // Setting Shipping_Address
                                    var soAddress = soGraph.Shipping_Address.Current;
                                    soGraph_FA.Shipping_Address.Current = soGraph_FA.Shipping_Address.Select();
                                    soAddress.OverrideAddress = true;
                                    soAddress.PostalCode = soGraph_FA.Shipping_Address.Current?.PostalCode;
                                    soAddress.CountryID = soGraph_FA.Shipping_Address.Current?.CountryID;
                                    soAddress.State = soGraph_FA.Shipping_Address.Current?.State;
                                    soAddress.City = soGraph_FA.Shipping_Address.Current?.City;
                                    soAddress.RevisionID = 1;
                                    // Setting Shipping_Contact
                                    var soContact = soGraph.Shipping_Contact.Current;
                                    soGraph_FA.Shipping_Contact.Current = soGraph_FA.Shipping_Contact.Select();
                                    soContact.OverrideContact = true;
                                    soContact.Email = soGraph_FA.Shipping_Contact.Current?.Email;
                                    soContact.RevisionID = 1;
                                }
                                #endregion

                                var customerDefWarehouse = SelectFrom<Location>
                                                           .Where<Location.bAccountID.IsEqual<P.AsInt>>
                                                           .View.SelectSingleBound(baseGraph, null, soDoc.CustomerID)
                                                           .TopFirst?.CSiteID;
                                #region SOLine
                                foreach (var row in amzGroupOrderData.OrderBy(x => x.AmountType))
                                {
                                    PXLongOperation.SetCurrentItem(row);
                                    var soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                    if (row.AmountDescription == "Commission")
                                    {
                                        soTrans.InventoryID = AmazonPublicFunction.GetFeeNonStockItem(row.AmountDescription);
                                        soTrans.OrderQty = 1;
                                        soTrans.CuryUnitPrice = (row.Amount ?? 0) * -1;
                                    }
                                    else if (row.AmountDescription == "RefundCommission")
                                    {
                                        soTrans.InventoryID = AmazonPublicFunction.GetFeeNonStockItem(row.AmountDescription);
                                        soTrans.OrderQty = 1;
                                        soTrans.CuryUnitPrice = (row.Amount ?? 0) * -1;
                                    }
                                    else if (row.AmountDescription == "Principal" && row.Amount < 0)
                                    {
                                        soTrans.InventoryID = AmazonPublicFunction.GetInvetoryitemID(soGraph, row.Sku);
                                        soTrans.LocationID = AmazonPublicFunction.GetLocationID(customerDefWarehouse);
                                        soTrans.OrderQty = 1;
                                        soTrans.CuryUnitPrice = (row.Amount ?? 0) * -1;
                                    }
                                    else if (row.AmountDescription == "Principal" && row.Amount > 0)
                                    {
                                        // 找Principal條件的Line
                                        var PrincipalLine = soGraph.Transactions.Cache.Cached.RowCast<SOLine>().FirstOrDefault(x => x.InventoryID == AmazonPublicFunction.GetInvetoryitemID(soGraph, row.Sku));
                                        if (PrincipalLine != null)
                                        {
                                            PrincipalLine.ManualDisc = true;
                                            PrincipalLine.CuryDiscAmt += row.Amount;
                                            soGraph.Transactions.Update(PrincipalLine);
                                        }
                                        continue;
                                    }
                                    else if (row.AmountDescription == "Goodwill")
                                    {
                                        soTrans.InventoryID = AmazonPublicFunction.GetFeeNonStockItem(row.AmountDescription);
                                        soTrans.OrderQty = 1;
                                        soTrans.CuryUnitPrice = (row.Amount ?? 0) * -1;
                                    }
                                    else if (row.AmountDescription == "RestockingFee")
                                    {
                                        soTrans.InventoryID = AmazonPublicFunction.GetFeeNonStockItem(row.AmountDescription);
                                        soTrans.OrderQty = 1;
                                        soTrans.CuryUnitPrice = (row.Amount ?? 0) * -1;
                                    }
                                    else if (row.AmountDescription == "Shipping")
                                    {
                                        soTrans.InventoryID = AmazonPublicFunction.GetFeeNonStockItem(row.AmountDescription);
                                        soTrans.OrderQty = 1;
                                        soTrans.CuryUnitPrice = (row.Amount ?? 0) * -1;
                                    }
                                    else if (row.AmountDescription == "ShippingChargeback")
                                    {
                                        soTrans.InventoryID = AmazonPublicFunction.GetFeeNonStockItem(row.AmountDescription);
                                        soTrans.OrderQty = 1;
                                        soTrans.CuryUnitPrice = (row.Amount ?? 0) * -1;
                                    }
                                    else if (row.AmountDescription == "ShippingHB")
                                    {
                                        soTrans.InventoryID = AmazonPublicFunction.GetFeeNonStockItem(row.AmountDescription);
                                        soTrans.OrderQty = 1;
                                        soTrans.CuryUnitPrice = (row.Amount ?? 0) * -1;
                                    }
                                    else if (row.AmountDescription == "PointsReturned" || row.AmountDescription == "PointsFee")
                                    {
                                        soTrans.InventoryID = AmazonPublicFunction.GetFeeNonStockItem(row.AmountDescription);
                                        soTrans.OrderQty = 1;
                                        soTrans.CuryUnitPrice = (row.Amount ?? 0) * -1;
                                    }
                                    else
                                        continue;
                                    if (soTrans.InventoryID == null)
                                        throw new PXException($"Can not find SOLine InventoryID (OrderType: {amzGroupOrderData.Key.TransactionType}, Amount Descr:{row.AmountDescription})");
                                    // isTaxCalculate = true then NONTAXABLE
                                    if (isTaxCalculate)
                                        soTrans.TaxCategoryID = "NONTAXABLE";
                                    soGraph.Transactions.Insert(soTrans);
                                }

                                #endregion

                                #region Update Tax
                                // Setting SO Tax
                                if (!isTaxCalculate)
                                {
                                    soGraph.Taxes.Current = soGraph.Taxes.Current ?? soGraph.Taxes.Insert(soGraph.Taxes.Cache.CreateInstance() as SOTaxTran);
                                    soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, _marketplace + "EC");
                                    soGraph.Taxes.Cache.SetValueExt<SOTaxTran.curyTaxAmt>(soGraph.Taxes.Current, _marketplace == "US" ? 0 : amzTotalTax);

                                    soGraph.Document.Cache.SetValueExt<SOOrder.curyTaxTotal>(soGraph.Document.Current, _marketplace == "US" ? 0 : amzTotalTax);
                                    soGraph.Document.Cache.SetValueExt<SOOrder.curyOrderTotal>(soGraph.Document.Current, (soGraph.Document.Current?.CuryOrderTotal ?? 0) + (_marketplace == "US" ? 0 : amzTotalTax));
                                }
                                #endregion

                                // Sales Order Save
                                soGraph.Save.Press();

                                #region Create PaymentRefund
                                var paymentExt = soGraph.GetExtension<CreatePaymentExt>();
                                paymentExt.SetDefaultValues(paymentExt.QuickPayment.Current, soGraph.Document.Current);
                                paymentExt.QuickPayment.Current.ExtRefNbr = amzGroupOrderData.Key.SettlementID;
                                ARPaymentEntry paymentEntry = paymentExt.CreatePayment(paymentExt.QuickPayment.Current, soGraph.Document.Current, ARPaymentType.Refund);
                                paymentEntry.Document.Cache.SetValueExt<ARPayment.adjDate>(paymentEntry.Document.Current, amzGroupOrderData.Key.PostedDate);
                                paymentEntry.Save.Press();
                                paymentEntry.releaseFromHold.Press();
                                paymentEntry.release.Press();
                                #endregion

                                // Prepare Invoice
                                PrepareInvoiceAndOverrideTax(soGraph, soDoc);
                                #endregion
                                break;
                            case "ORDER":
                                #region Transaction Type: Order
                                #region Create Payment
                                if (!(amzGroupOrderData.Key?.OrderID?.ToUpper().StartsWith("S") ?? false))
                                {
                                    var arGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                                    #region Header(Document)
                                    var arDoc = arGraph.Document.Cache.CreateInstance() as ARPayment;
                                    arDoc.DocType = "PMT";
                                    arDoc.AdjDate = amzGroupOrderData.Key.PostedDate;
                                    arDoc.ExtRefNbr = amzGroupOrderData.Key.SettlementID;
                                    arDoc.CustomerID = AmazonPublicFunction.GetMarketplaceCustomer(_marketplace);
                                    arDoc.DocDesc = $"Amazon ({amzGroupOrderData.Key.TransactionType}) {amzGroupOrderData.Key.OrderID}";
                                    arDoc.DepositDate = GetDepositDate(amzGroupOrderData.Key.SettlementID) ?? DateTime.Now;
                                    if (arDoc.DepositDate == null)
                                        throw new Exception($"can not find Deposit Date({amzGroupOrderData.Key.SettlementID})");

                                    #region User-Defiend

                                    // UserDefined - ECNETPAY
                                    arGraph.Document.Cache.SetValueExt(arDoc, PX.Objects.CS.Messages.Attribute + "ECNETPAY", amzGroupOrderData.Sum(x => x.Amount ?? 0));

                                    #endregion

                                    arGraph.Document.Insert(arDoc);
                                    #endregion

                                    #region Adjustments
                                    var AmzPaymentAmount = amzGroupOrderData.Where(x => x.AmountType == "ItemPrice" || x.AmountType == "Promotion").Sum(x => x.Amount);
                                    var mapInvoice = SelectFrom<ARInvoice>
                                                          .InnerJoin<ARTran>.On<ARInvoice.docType.IsEqual<ARTran.tranType>
                                                                .And<ARInvoice.refNbr.IsEqual<ARTran.refNbr>>>
                                                          .InnerJoin<SOOrder>.On<ARTran.sOOrderNbr.IsEqual<SOOrder.orderNbr>>
                                                          .Where<ARInvoice.invoiceNbr.IsEqual<P.AsString>
                                                            .And<SOOrder.orderType.IsEqual<P.AsString>>>
                                                          .View.SelectSingleBound(baseGraph, null, amzGroupOrderData.Key.OrderID, "FA").TopFirst;
                                    if (mapInvoice == null)
                                        throw new Exception($"Can not Find Invoice (OrderID: {amzGroupOrderData.Key.OrderID})");
                                    var adjTrans = arGraph.Adjustments.Cache.CreateInstance() as ARAdjust;
                                    adjTrans.AdjdDocType = "INV";
                                    adjTrans.AdjdRefNbr = mapInvoice?.RefNbr;
                                    adjTrans.CuryAdjgAmt = AmzPaymentAmount ?? 0;
                                    arGraph.Adjustments.Insert(adjTrans);
                                    #endregion

                                    #region CHARGS
                                    foreach (var item in amzGroupOrderData)
                                    {
                                        PXLongOperation.SetCurrentItem(item);
                                        var chargeTrans = arGraph.PaymentCharges.Cache.CreateInstance() as ARPaymentChargeTran;
                                        if ((item.AmountType?.ToUpper() == "ITEMFEES" && item.AmountDescription?.ToUpper() != "CODFEE") || (item.AmountType?.ToUpper() == "POINTS" && item.AmountDescription?.ToUpper() != "CODITEMCHARGE") || item.AmountType?.ToUpper() == "SHIPMENTFEES")
                                        {
                                            chargeTrans.EntryTypeID = item.AmountDescription.Length >= 10 ? item.AmountDescription.Substring(0, 10) : item.AmountDescription;
                                            chargeTrans.CuryTranAmt = item?.Amount * -1;
                                        }
                                        else if (item.AmountDescription?.ToUpper() == "CODFEE")
                                        {
                                            chargeTrans.EntryTypeID = "CODFEE";
                                            chargeTrans.CuryTranAmt = (decimal)amzGroupOrderData.Where(x => x.AmountDescription.ToUpper().StartsWith("COD")).Sum(y => (y.Amount ?? 0)) * -1;
                                            if (chargeTrans.CuryTranAmt == 0 || !chargeTrans.CuryTranAmt.HasValue)
                                                continue;
                                        }
                                        else if ((item?.Amount ?? 0) != 0 && (item.AmountDescription?.ToUpper() == "MARKETPLACEFACILITATORVAT-PRINCIPAL" || item.AmountDescription?.ToUpper() == "MARKETPLACEFACILITATORTAX-PRINCIPAL" || item.AmountDescription?.ToUpper() == "MARKETPLACEFACILITATORVAT-SHIPPING" || item.AmountDescription?.ToUpper() == "MARKETPLACEFACILITATORTAX-OTHER" || item.AmountDescription?.ToUpper() == "MARKETPLACEFACILITATORTAX-SHIPPING"))
                                        {
                                            chargeTrans.EntryTypeID = "WHTAX" + _marketplace;
                                            chargeTrans.CuryTranAmt = item.Amount * -1;
                                        }
                                        else
                                            continue;
                                        arGraph.PaymentCharges.Insert(chargeTrans);
                                    }

                                    #endregion
                                    // set payment amount to apply amount
                                    arGraph.Document.SetValueExt<ARPayment.curyOrigDocAmt>(arGraph.Document.Current, arGraph.Document.Current.CuryApplAmt);
                                    // Save Payment
                                    arGraph.Actions.PressSave();
                                    // Release Payment
                                    arGraph.releaseFromHold.Press();
                                    arGraph.release.Press();
                                }
                                #endregion
                                #region Create Sales Order MCF
                                else
                                {
                                    soGraph = PXGraph.CreateInstance<SOOrderEntry>();

                                    #region Header
                                    soDoc = soGraph.Document.Cache.CreateInstance() as SOOrder;
                                    soDoc.OrderType = "CM";
                                    soDoc.CustomerOrderNbr = amzGroupOrderData.Key.OrderID;
                                    soDoc.CustomerRefNbr = amzGroupOrderData.Key.MerchantOrderID;
                                    soDoc.OrderDate = amzGroupOrderData.Key.PostedDate;
                                    soDoc.RequestDate = amzGroupOrderData.Key.PostedDate;
                                    soDoc.CustomerID = AmazonPublicFunction.GetMarketplaceCustomer(_marketplace);
                                    soDoc.OrderDesc = $"Amazon ({amzGroupOrderData.Key.TransactionType}) {amzGroupOrderData.Key.OrderID}";
                                    #endregion

                                    #region User-Defined
                                    // UserDefined - ORDERTYPE
                                    soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERTYPE", $"Amazon MCF");
                                    // UserDefined - MKTPLACE
                                    soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "MKTPLACE", _marketplace);
                                    // UserDefined - ORDERAMT
                                    soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERAMT", amzGroupOrderData.Sum(x => (x.Amount ?? 0) * -1));
                                    #endregion

                                    // Insert SOOrder
                                    soGraph.Document.Insert(soDoc);

                                    #region Set Currency
                                    info = CurrencyInfoAttribute.SetDefaults<SOOrder.curyInfoID>(soGraph.Document.Cache, soGraph.Document.Current);
                                    if (info != null)
                                        soGraph.Document.Cache.SetValueExt<SOOrder.curyID>(soGraph.Document.Current, info.CuryID);
                                    #endregion

                                    #region Address
                                    soGraph_FA = PXGraph.CreateInstance<SOOrderEntry>();
                                    soOrder_FAInfo = SelectFrom<SOOrder>
                                                     .Where<SOOrder.orderType.IsEqual<P.AsString>
                                                       .And<SOOrder.customerOrderNbr.IsEqual<P.AsString>>>
                                                     .View.SelectSingleBound(soGraph_FA, null, "FA", amzGroupOrderData.Key.OrderID).TopFirst;
                                    soGraph_FA.Document.Current = soOrder_FAInfo;
                                    if (soGraph_FA.Document.Current != null)
                                    {
                                        // Setting Shipping_Address
                                        var soAddress = soGraph.Shipping_Address.Current;
                                        soGraph_FA.Shipping_Address.Current = soGraph_FA.Shipping_Address.Select();
                                        soAddress.OverrideAddress = true;
                                        soAddress.PostalCode = soGraph_FA.Shipping_Address.Current?.PostalCode;
                                        soAddress.CountryID = soGraph_FA.Shipping_Address.Current?.CountryID;
                                        soAddress.State = soGraph_FA.Shipping_Address.Current?.State;
                                        soAddress.City = soGraph_FA.Shipping_Address.Current?.City;
                                        soAddress.RevisionID = 1;
                                        // Setting Shipping_Contact
                                        var soContact = soGraph.Shipping_Contact.Current;
                                        soGraph_FA.Shipping_Contact.Current = soGraph_FA.Shipping_Contact.Select();
                                        soContact.OverrideContact = true;
                                        soContact.Email = soGraph_FA.Shipping_Contact.Current?.Email;
                                        soContact.RevisionID = 1;
                                    }
                                    #endregion

                                    #region SOLine
                                    foreach (var row in amzGroupOrderData)
                                    {
                                        PXLongOperation.SetCurrentItem(row);
                                        var soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                        if ((row.Amount ?? 0) == 0)
                                            continue;
                                        soTrans.InventoryID = AmazonPublicFunction.GetInvetoryitemID(baseGraph, "EC-SHIPPING");
                                        soTrans.OrderQty = 1;
                                        soTrans.TranDesc = row.AmountDescription;
                                        soTrans.CuryUnitPrice = (row.Amount ?? 0) * -1;
                                        if (soTrans.InventoryID == null)
                                            throw new PXException($"Can not find SOLine InventoryID (OrderType: {amzGroupOrderData.Key.TransactionType}, Amount Descr:EC-SHIPPING)");
                                        // isTaxCalculate = true then NONTAXABLE
                                        if (isTaxCalculate)
                                            soTrans.TaxCategoryID = "NONTAXABLE";
                                        soGraph.Transactions.Insert(soTrans);
                                    }

                                    #endregion

                                    #region Update Tax
                                    // Setting SO Tax
                                    if (!isTaxCalculate)
                                    {
                                        soGraph.Taxes.Current = soGraph.Taxes.Current ?? soGraph.Taxes.Insert(soGraph.Taxes.Cache.CreateInstance() as SOTaxTran);
                                        soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, _marketplace + "EC");
                                    }
                                    #endregion

                                    // Sales Order Save
                                    soGraph.Save.Press();

                                    #region Create PaymentRefund
                                    paymentExt = soGraph.GetExtension<CreatePaymentExt>();
                                    paymentExt.SetDefaultValues(paymentExt.QuickPayment.Current, soGraph.Document.Current);
                                    paymentExt.QuickPayment.Current.ExtRefNbr = amzGroupOrderData.Key.SettlementID;
                                    paymentEntry = paymentExt.CreatePayment(paymentExt.QuickPayment.Current, soGraph.Document.Current, ARPaymentType.Refund);
                                    paymentEntry.Document.Cache.SetValueExt<ARPayment.adjDate>(paymentEntry.Document.Current, amzGroupOrderData.Key.PostedDate);
                                    paymentEntry.Save.Press();
                                    paymentEntry.releaseFromHold.Press();
                                    paymentEntry.release.Press();
                                    #endregion

                                    // Prepare Invoice
                                    PrepareInvoiceAndOverrideTax(soGraph, soDoc);
                                }
                                #endregion
                                #endregion
                                break;
                            case "REFUND_RETROCHARGE":
                                #region Transaction Type: Refund_Retrocharge

                                soGraph = PXGraph.CreateInstance<SOOrderEntry>();

                                #region Header
                                soDoc = soGraph.Document.Cache.CreateInstance() as SOOrder;
                                soDoc.OrderType = "CM";
                                soDoc.CustomerOrderNbr = amzGroupOrderData.Key.OrderID;
                                soDoc.OrderDate = amzGroupOrderData.Key.PostedDate;
                                soDoc.RequestDate = amzGroupOrderData.Key.PostedDate;
                                soDoc.CustomerID = AmazonPublicFunction.GetMarketplaceCustomer(_marketplace);
                                soDoc.OrderDesc = $"Amazon ({amzGroupOrderData.Key.TransactionType}) {amzGroupOrderData.Key.OrderID}";
                                #endregion

                                #region User-Defined
                                // UserDefined - ORDERTYPE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERTYPE", $"Amazon {amzGroupOrderData.Key.TransactionType}");
                                // UserDefined - MKTPLACE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "MKTPLACE", _marketplace);
                                // UserDefined - ORDERAMT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERAMT", amzGroupOrderData.Sum(x => (x.Amount ?? 0) * -1));
                                #endregion

                                // Insert SOOrder
                                soGraph.Document.Insert(soDoc);

                                #region Set Currency
                                info = CurrencyInfoAttribute.SetDefaults<SOOrder.curyInfoID>(soGraph.Document.Cache, soGraph.Document.Current);
                                if (info != null)
                                    soGraph.Document.Cache.SetValueExt<SOOrder.curyID>(soGraph.Document.Current, info.CuryID);
                                #endregion

                                #region Address
                                soGraph_FA = PXGraph.CreateInstance<SOOrderEntry>();
                                soOrder_FAInfo = SelectFrom<SOOrder>
                                                 .Where<SOOrder.orderType.IsEqual<P.AsString>
                                                   .And<SOOrder.customerOrderNbr.IsEqual<P.AsString>>>
                                                 .View.SelectSingleBound(soGraph_FA, null, "FA", amzGroupOrderData.Key.OrderID).TopFirst;
                                soGraph_FA.Document.Current = soOrder_FAInfo;
                                if (soGraph_FA.Document.Current != null)
                                {
                                    // Setting Shipping_Address
                                    var soAddress = soGraph.Shipping_Address.Current;
                                    soGraph_FA.Shipping_Address.Current = soGraph_FA.Shipping_Address.Select();
                                    soAddress.OverrideAddress = true;
                                    soAddress.PostalCode = soGraph_FA.Shipping_Address.Current?.PostalCode;
                                    soAddress.CountryID = soGraph_FA.Shipping_Address.Current?.CountryID;
                                    soAddress.State = soGraph_FA.Shipping_Address.Current?.State;
                                    soAddress.City = soGraph_FA.Shipping_Address.Current?.City;
                                    soAddress.RevisionID = 1;
                                    // Setting Shipping_Contact
                                    var soContact = soGraph.Shipping_Contact.Current;
                                    soGraph_FA.Shipping_Contact.Current = soGraph_FA.Shipping_Contact.Select();
                                    soContact.OverrideContact = true;
                                    soContact.Email = soGraph_FA.Shipping_Contact.Current?.Email;
                                    soContact.RevisionID = 1;
                                }
                                #endregion

                                #region SOLine
                                foreach (var row in amzGroupOrderData)
                                {
                                    PXLongOperation.SetCurrentItem(row);
                                    var soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                    soTrans.InventoryID = AmazonPublicFunction.GetFeeNonStockItem(row.AmountDescription);
                                    soTrans.OrderQty = 1;
                                    soTrans.CuryUnitPrice = (row.Amount ?? 0) * -1;
                                    if (soTrans.InventoryID == null)
                                        throw new PXException($"Can not find SOLine InventoryID (OrderType: {amzGroupOrderData.Key.TransactionType}, Amount Descr:{row.AmountDescription})");
                                    // isTaxCalculate = true then NONTAXABLE
                                    if (isTaxCalculate)
                                        soTrans.TaxCategoryID = "NONTAXABLE";
                                    soGraph.Transactions.Insert(soTrans);
                                }

                                #endregion

                                #region Update Tax
                                // Setting SO Tax
                                if (!isTaxCalculate)
                                {
                                    soGraph.Taxes.Current = soGraph.Taxes.Current ?? soGraph.Taxes.Insert(soGraph.Taxes.Cache.CreateInstance() as SOTaxTran);
                                    soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, _marketplace + "EC");
                                }
                                #endregion

                                // Sales Order Save
                                soGraph.Save.Press();

                                #region Create PaymentRefund
                                paymentExt = soGraph.GetExtension<CreatePaymentExt>();
                                paymentExt.SetDefaultValues(paymentExt.QuickPayment.Current, soGraph.Document.Current);
                                paymentExt.QuickPayment.Current.ExtRefNbr = amzGroupOrderData.Key.SettlementID;
                                paymentEntry = paymentExt.CreatePayment(paymentExt.QuickPayment.Current, soGraph.Document.Current, ARPaymentType.Refund);
                                paymentEntry.Document.Cache.SetValueExt<ARPayment.adjDate>(paymentEntry.Document.Current, amzGroupOrderData.Key.PostedDate);
                                paymentEntry.Save.Press();
                                paymentEntry.releaseFromHold.Press();
                                paymentEntry.release.Press();
                                #endregion

                                // Prepare Invoice
                                PrepareInvoiceAndOverrideTax(soGraph, soDoc);
                                #endregion
                                break;
                            case "COUPONREDEMPTIONFEE":
                                #region Transaction Type: CouponRedemptionFee

                                soGraph = PXGraph.CreateInstance<SOOrderEntry>();

                                #region Header
                                soDoc = soGraph.Document.Cache.CreateInstance() as SOOrder;
                                soDoc.OrderType = "CM";
                                soDoc.CustomerOrderNbr = amzGroupOrderData.Key.OrderID;
                                soDoc.OrderDate = amzGroupOrderData.Key.PostedDate;
                                soDoc.RequestDate = amzGroupOrderData.Key.PostedDate;
                                soDoc.CustomerID = AmazonPublicFunction.GetMarketplaceCustomer(_marketplace);
                                soDoc.OrderDesc = $"Amazon ({amzGroupOrderData.Key.TransactionType}) {amzGroupOrderData.Key.OrderID}";
                                #endregion

                                #region User-Defined
                                // UserDefined - ORDERTYPE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERTYPE", $"Amazon {amzGroupOrderData.Key.TransactionType}");
                                // UserDefined - MKTPLACE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "MKTPLACE", _marketplace);
                                // UserDefined - ORDERAMT
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERAMT", amzGroupOrderData.Sum(x => (x.Amount ?? 0) * -1));
                                #endregion

                                // Insert SOOrder
                                soGraph.Document.Insert(soDoc);

                                #region Set Currency
                                info = CurrencyInfoAttribute.SetDefaults<SOOrder.curyInfoID>(soGraph.Document.Cache, soGraph.Document.Current);
                                if (info != null)
                                    soGraph.Document.Cache.SetValueExt<SOOrder.curyID>(soGraph.Document.Current, info.CuryID);
                                #endregion

                                #region Address
                                soGraph_FA = PXGraph.CreateInstance<SOOrderEntry>();
                                soOrder_FAInfo = SelectFrom<SOOrder>
                                                 .Where<SOOrder.orderType.IsEqual<P.AsString>
                                                   .And<SOOrder.customerOrderNbr.IsEqual<P.AsString>>>
                                                 .View.SelectSingleBound(soGraph_FA, null, "FA", amzGroupOrderData.Key.OrderID).TopFirst;
                                soGraph_FA.Document.Current = soOrder_FAInfo;
                                if (soGraph_FA.Document.Current != null)
                                {
                                    // Setting Shipping_Address
                                    var soAddress = soGraph.Shipping_Address.Current;
                                    soGraph_FA.Shipping_Address.Current = soGraph_FA.Shipping_Address.Select();
                                    soAddress.OverrideAddress = true;
                                    soAddress.PostalCode = soGraph_FA.Shipping_Address.Current?.PostalCode;
                                    soAddress.CountryID = soGraph_FA.Shipping_Address.Current?.CountryID;
                                    soAddress.State = soGraph_FA.Shipping_Address.Current?.State;
                                    soAddress.City = soGraph_FA.Shipping_Address.Current?.City;
                                    soAddress.RevisionID = 1;
                                    // Setting Shipping_Contact
                                    var soContact = soGraph.Shipping_Contact.Current;
                                    soGraph_FA.Shipping_Contact.Current = soGraph_FA.Shipping_Contact.Select();
                                    soContact.OverrideContact = true;
                                    soContact.Email = soGraph_FA.Shipping_Contact.Current?.Email;
                                    soContact.RevisionID = 1;
                                }
                                #endregion

                                #region SOLine
                                foreach (var row in amzGroupOrderData)
                                {
                                    PXLongOperation.SetCurrentItem(row);
                                    var soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                    soTrans.InventoryID = AmazonPublicFunction.GetInvetoryitemID(soGraph, "EC-COUPONREDEMPTION");
                                    soTrans.TranDesc = row.AmountDescription;
                                    soTrans.OrderQty = 1;
                                    soTrans.CuryUnitPrice = (row.Amount ?? 0) * -1;
                                    if (soTrans.InventoryID == null)
                                        throw new PXException($"Can not find SOLine InventoryID (OrderType: {amzGroupOrderData.Key.TransactionType}, Amount Descr:{row.AmountDescription})");
                                    // isTaxCalculate = true then NONTAXABLE
                                    if (isTaxCalculate)
                                        soTrans.TaxCategoryID = "NONTAXABLE";
                                    soGraph.Transactions.Insert(soTrans);
                                }

                                #endregion

                                #region Update Tax
                                // Setting SO Tax
                                if (!isTaxCalculate)
                                {
                                    soGraph.Taxes.Current = soGraph.Taxes.Current ?? soGraph.Taxes.Insert(soGraph.Taxes.Cache.CreateInstance() as SOTaxTran);
                                    soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, _marketplace + "EC");
                                }
                                #endregion

                                // Sales Order Save
                                soGraph.Save.Press();

                                #region Create PaymentRefund
                                paymentExt = soGraph.GetExtension<CreatePaymentExt>();
                                paymentExt.SetDefaultValues(paymentExt.QuickPayment.Current, soGraph.Document.Current);
                                paymentExt.QuickPayment.Current.ExtRefNbr = amzGroupOrderData.Key.SettlementID;
                                paymentEntry = paymentExt.CreatePayment(paymentExt.QuickPayment.Current, soGraph.Document.Current, ARPaymentType.Refund);
                                paymentEntry.Document.Cache.SetValueExt<ARPayment.adjDate>(paymentEntry.Document.Current, amzGroupOrderData.Key.PostedDate);
                                paymentEntry.Save.Press();
                                paymentEntry.releaseFromHold.Press();
                                paymentEntry.release.Press();
                                #endregion

                                // Prepare Invoice
                                PrepareInvoiceAndOverrideTax(soGraph, soDoc);
                                #endregion
                                break;
                            case "OTHER-TRANSACTION":
                                #region Transaction Type: OTHER-TRANSACTION

                                soGraph = PXGraph.CreateInstance<SOOrderEntry>();

                                #region Header
                                soDoc = soGraph.Document.Cache.CreateInstance() as SOOrder;
                                soDoc.OrderType = amzGroupOrderData.Where(x => x.AmountDescription != "Current Reserve Amount" &&
                                                                               x.AmountDescription != "Previous Reserve Amount Balance")
                                                                   .Sum(x => x.Amount ?? 0) > 0 ? "IN" : "CM";
                                soDoc.CustomerOrderNbr = amzGroupOrderData.Key.OrderID;
                                soDoc.OrderDate = amzGroupOrderData.Key.PostedDate;
                                soDoc.RequestDate = amzGroupOrderData.Key.PostedDate;
                                soDoc.CustomerID = AmazonPublicFunction.GetMarketplaceCustomer(_marketplace);
                                soDoc.OrderDesc = $"Amazon ({amzGroupOrderData.Key.TransactionType}) {amzGroupOrderData.Key.OrderID}";
                                #endregion

                                #region User-Defined
                                // UserDefined - ORDERTYPE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERTYPE", $"Amazon {amzGroupOrderData.Key.TransactionType}");
                                // UserDefined - MKTPLACE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "MKTPLACE", _marketplace);
                                // UserDefined - ORDERAMT (CM: Sum Amount * -1)
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERAMT", soDoc.OrderType == "CM" ?
                                    amzGroupOrderData.Where(x => x.AmountDescription != "Current Reserve Amount" &&
                                                                 x.AmountDescription != "Previous Reserve Amount Balance" &&
                                                                 x.AmountDescription != "Payable to Amazon")
                                                     .Sum(x => x.Amount ?? 0) * -1 :
                                    amzGroupOrderData.Where(x => x.AmountDescription != "Current Reserve Amount" &&
                                                                 x.AmountDescription != "Previous Reserve Amount Balance" &&
                                                                 x.AmountDescription != "Payable to Amazon")
                                                     .Sum(x => x.Amount ?? 0));
                                #endregion

                                // Insert SOOrder
                                soGraph.Document.Insert(soDoc);

                                #region Set Currency
                                info = CurrencyInfoAttribute.SetDefaults<SOOrder.curyInfoID>(soGraph.Document.Cache, soGraph.Document.Current);
                                if (info != null)
                                    soGraph.Document.Cache.SetValueExt<SOOrder.curyID>(soGraph.Document.Current, info.CuryID);
                                #endregion

                                #region Address
                                soGraph_FA = PXGraph.CreateInstance<SOOrderEntry>();
                                soOrder_FAInfo = SelectFrom<SOOrder>
                                                 .Where<SOOrder.orderType.IsEqual<P.AsString>
                                                   .And<SOOrder.customerOrderNbr.IsEqual<P.AsString>>>
                                                 .View.SelectSingleBound(soGraph_FA, null, "FA", amzGroupOrderData.Key.OrderID).TopFirst;
                                soGraph_FA.Document.Current = soOrder_FAInfo;
                                if (soGraph_FA.Document.Current != null)
                                {
                                    // Setting Shipping_Address
                                    var soAddress = soGraph.Shipping_Address.Current;
                                    soGraph_FA.Shipping_Address.Current = soGraph_FA.Shipping_Address.Select();
                                    soAddress.OverrideAddress = true;
                                    soAddress.PostalCode = soGraph_FA.Shipping_Address.Current?.PostalCode;
                                    soAddress.CountryID = soGraph_FA.Shipping_Address.Current?.CountryID;
                                    soAddress.State = soGraph_FA.Shipping_Address.Current?.State;
                                    soAddress.City = soGraph_FA.Shipping_Address.Current?.City;
                                    soAddress.RevisionID = 1;
                                    // Setting Shipping_Contact
                                    var soContact = soGraph.Shipping_Contact.Current;
                                    soGraph_FA.Shipping_Contact.Current = soGraph_FA.Shipping_Contact.Select();
                                    soContact.OverrideContact = true;
                                    soContact.Email = soGraph_FA.Shipping_Contact.Current?.Email;
                                    soContact.RevisionID = 1;
                                }
                                #endregion

                                #region SOLine
                                foreach (var row in amzGroupOrderData.Where(x => x.AmountDescription != "Current Reserve Amount" && x.AmountDescription != "Previous Reserve Amount Balance"))
                                {
                                    PXLongOperation.SetCurrentItem(row);
                                    var soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                    soTrans.InventoryID = AmazonPublicFunction.GetFeeNonStockItem(row.AmountDescription);
                                    soTrans.OrderQty = 1;
                                    soTrans.CuryUnitPrice = soDoc.OrderType == "CM" ? (row.Amount ?? 0) * -1 : (row.Amount ?? 0);
                                    if (soTrans.InventoryID == null)
                                        throw new PXException($"Can not find SOLine InventoryID (OrderType: {amzGroupOrderData.Key.TransactionType}, Amount Descr:{row.AmountDescription})");
                                    // isTaxCalculate = true then NONTAXABLE
                                    if (isTaxCalculate)
                                        soTrans.TaxCategoryID = "NONTAXABLE";
                                    soGraph.Transactions.Insert(soTrans);
                                }

                                #endregion

                                #region Update Tax
                                // Setting SO Tax
                                if (!isTaxCalculate)
                                {
                                    soGraph.Taxes.Current = soGraph.Taxes.Current ?? soGraph.Taxes.Insert(soGraph.Taxes.Cache.CreateInstance() as SOTaxTran);
                                    soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, _marketplace + "EC");
                                }
                                #endregion

                                // Sales Order Save
                                soGraph.Save.Press();

                                #region Create PaymentRefund
                                paymentExt = soGraph.GetExtension<CreatePaymentExt>();
                                paymentExt.SetDefaultValues(paymentExt.QuickPayment.Current, soGraph.Document.Current);
                                paymentExt.QuickPayment.Current.ExtRefNbr = amzGroupOrderData.Key.SettlementID;
                                paymentEntry = paymentExt.CreatePayment(paymentExt.QuickPayment.Current, soGraph.Document.Current, soDoc.OrderType == "CM" ? ARPaymentType.Refund : ARPaymentType.Payment);
                                paymentEntry.Document.Cache.SetValueExt<ARPayment.adjDate>(paymentEntry.Document.Current, amzGroupOrderData.Key.PostedDate);
                                paymentEntry.Save.Press();
                                paymentEntry.releaseFromHold.Press();
                                paymentEntry.release.Press();
                                #endregion

                                // Prepare Invoice
                                PrepareInvoiceAndOverrideTax(soGraph, soDoc);
                                #endregion
                                break;
                            default:
                                #region Transaction Type: Undefined Transactions

                                soGraph = PXGraph.CreateInstance<SOOrderEntry>();

                                #region Header
                                soDoc = soGraph.Document.Cache.CreateInstance() as SOOrder;
                                soDoc.OrderType = amzGroupOrderData.Sum(x => x.Amount ?? 0) > 0 ? "IN" : "CM";
                                soDoc.CustomerOrderNbr = amzGroupOrderData.Key.OrderID;
                                soDoc.OrderDate = amzGroupOrderData.Key.PostedDate;
                                soDoc.RequestDate = amzGroupOrderData.Key.PostedDate;
                                soDoc.CustomerID = AmazonPublicFunction.GetMarketplaceCustomer(_marketplace);
                                soDoc.OrderDesc = $"Amazon ({amzGroupOrderData.Key.TransactionType}) {amzGroupOrderData.Key.OrderID}";
                                #endregion

                                #region User-Defined
                                // UserDefined - ORDERTYPE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERTYPE", $"Amazon Undefined Transactions");
                                // UserDefined - MKTPLACE
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "MKTPLACE", _marketplace);
                                // UserDefined - ORDERAMT (CM: Sum Amount * -1)
                                soGraph.Document.Cache.SetValueExt(soDoc, PX.Objects.CS.Messages.Attribute + "ORDERAMT", soDoc.OrderType == "CM" ?
                                    amzGroupOrderData.Sum(x => x.Amount ?? 0) * -1 :
                                    amzGroupOrderData.Sum(x => x.Amount ?? 0));
                                #endregion

                                // Insert SOOrder
                                soGraph.Document.Insert(soDoc);

                                #region Set Currency
                                info = CurrencyInfoAttribute.SetDefaults<SOOrder.curyInfoID>(soGraph.Document.Cache, soGraph.Document.Current);
                                if (info != null)
                                    soGraph.Document.Cache.SetValueExt<SOOrder.curyID>(soGraph.Document.Current, info.CuryID);
                                #endregion

                                #region Address
                                soGraph_FA = PXGraph.CreateInstance<SOOrderEntry>();
                                soOrder_FAInfo = SelectFrom<SOOrder>
                                                 .Where<SOOrder.orderType.IsEqual<P.AsString>
                                                   .And<SOOrder.customerOrderNbr.IsEqual<P.AsString>>>
                                                 .View.SelectSingleBound(soGraph_FA, null, "FA", amzGroupOrderData.Key.OrderID).TopFirst;
                                soGraph_FA.Document.Current = soOrder_FAInfo;
                                if (soGraph_FA.Document.Current != null)
                                {
                                    // Setting Shipping_Address
                                    var soAddress = soGraph.Shipping_Address.Current;
                                    soGraph_FA.Shipping_Address.Current = soGraph_FA.Shipping_Address.Select();
                                    soAddress.OverrideAddress = true;
                                    soAddress.PostalCode = soGraph_FA.Shipping_Address.Current?.PostalCode;
                                    soAddress.CountryID = soGraph_FA.Shipping_Address.Current?.CountryID;
                                    soAddress.State = soGraph_FA.Shipping_Address.Current?.State;
                                    soAddress.City = soGraph_FA.Shipping_Address.Current?.City;
                                    soAddress.RevisionID = 1;
                                    // Setting Shipping_Contact
                                    var soContact = soGraph.Shipping_Contact.Current;
                                    soGraph_FA.Shipping_Contact.Current = soGraph_FA.Shipping_Contact.Select();
                                    soContact.OverrideContact = true;
                                    soContact.Email = soGraph_FA.Shipping_Contact.Current?.Email;
                                    soContact.RevisionID = 1;
                                }
                                #endregion

                                #region SOLine
                                foreach (var row in amzGroupOrderData)
                                {
                                    PXLongOperation.SetCurrentItem(row);
                                    var soTrans = soGraph.Transactions.Cache.CreateInstance() as SOLine;
                                    soTrans.InventoryID = AmazonPublicFunction.GetInvetoryitemID(soGraph, "EC-COUPONREDEMPTION");
                                    soTrans.TranDesc = row.AmountDescription;
                                    soTrans.OrderQty = 1;
                                    soTrans.CuryUnitPrice = soDoc.OrderType == "CM" ? (row.Amount ?? 0) * -1 : (row.Amount ?? 0);
                                    if (soTrans.InventoryID == null)
                                        throw new PXException($"Can not find SOLine InventoryID (OrderType: Undefined Transactions, Amount Descr:{row.AmountDescription})");
                                    // isTaxCalculate = true then NONTAXABLE
                                    if (isTaxCalculate)
                                        soTrans.TaxCategoryID = "NONTAXABLE";
                                    soGraph.Transactions.Insert(soTrans);
                                }

                                #endregion

                                #region Update Tax
                                // Setting SO Tax
                                if (!isTaxCalculate)
                                {
                                    soGraph.Taxes.Current = soGraph.Taxes.Current ?? soGraph.Taxes.Insert(soGraph.Taxes.Cache.CreateInstance() as SOTaxTran);
                                    soGraph.Taxes.Cache.SetValueExt<SOTaxTran.taxID>(soGraph.Taxes.Current, _marketplace + "EC");
                                }
                                #endregion

                                // Sales Order Save
                                soGraph.Save.Press();

                                #region Create PaymentRefund
                                paymentExt = soGraph.GetExtension<CreatePaymentExt>();
                                paymentExt.SetDefaultValues(paymentExt.QuickPayment.Current, soGraph.Document.Current);
                                paymentExt.QuickPayment.Current.ExtRefNbr = amzGroupOrderData.Key.SettlementID;
                                paymentEntry = paymentExt.CreatePayment(paymentExt.QuickPayment.Current, soGraph.Document.Current, soDoc.OrderType == "CM" ? ARPaymentType.Refund : ARPaymentType.Payment);
                                paymentEntry.Document.Cache.SetValueExt<ARPayment.adjDate>(paymentEntry.Document.Current, amzGroupOrderData.Key.PostedDate);
                                paymentEntry.Save.Press();
                                paymentEntry.releaseFromHold.Press();
                                paymentEntry.release.Press();
                                #endregion

                                // Prepare Invoice
                                PrepareInvoiceAndOverrideTax(soGraph, soDoc);
                                #endregion
                                break;
                        }
                        sc.Complete();
                    }
                }
                catch (PXOuterException ex)
                {
                    errorMsg = $"Key:{DisplayGroupKey} Error: {ex.InnerMessages[0]}";
                    var errorItem = PXLongOperation.GetCurrentItem() as LUMAmazonSettlementTransData;
                    errorItem.ErrorMessage = errorMsg;
                }
                catch (Exception ex)
                {
                    errorMsg = $"Key:{DisplayGroupKey} Error: {ex.Message}";
                    var errorItem = PXLongOperation.GetCurrentItem() as LUMAmazonSettlementTransData;
                    errorItem.ErrorMessage = errorMsg;
                }
                finally
                {
                    // 有錯誤訊息
                    if (!string.IsNullOrEmpty(errorMsg))
                        PXProcessing.SetError<LUMAmazonSettlementTransData>(errorMsg);
                    amzGroupOrderData.ToList().ForEach(x =>
                    {
                        x.IsProcessed = string.IsNullOrEmpty(errorMsg);
                        x.ErrorMessage = (x.IsProcessed ?? false) ? string.Empty : x.ErrorMessage;
                        baseGraph.SettlementTransaction.Update(x);
                    });
                    // Save
                    baseGraph.Actions.PressSave();
                }
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
                var soTax = SelectFrom<SOTaxTran>
                            .Where<SOTaxTran.orderNbr.IsEqual<P.AsString>
                                 .And<SOTaxTran.orderType.IsEqual<P.AsString>>>
                            .View.SelectSingleBound(this, null, soGraph.Document.Current.OrderNbr, soGraph.Document.Current.OrderType)
                            .TopFirst;
                // update docDate
                invoiceGraph.Document.SetValueExt<ARInvoice.docDate>(invoiceGraph.Document.Current, soDoc.RequestDate);
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
                invoiceGraph.releaseFromHold.Press();
                invoiceGraph.releaseFromCreditHold.Press();
                invoiceGraph.release.Press();
                #endregion
            }
        }

        /// <summary> Get Amazon Connection Object </summary>
        public virtual AmazonConnection GetAmazonConnObject(string _marketPlace)
        {
            var setup = this.Setup.Select().TopFirst;
            if (setup == null)
                throw new Exception("MWS Preference is null");
            return new AmazonConnection(new AmazonCredential()
            {
                AccessKey = _marketPlace == "SG" ? setup.SGAccessKey : setup.AccessKey,
                SecretKey = _marketPlace == "SG" ? setup.SGSecretKey : setup.SecretKey,
                RoleArn = _marketPlace == "SG" ? setup.SGRoleArn : setup.RoleArn,
                ClientId = _marketPlace == "SG" ? setup.SGClientID :
                           _marketPlace == "MX" ? setup.MXClientID : setup.ClientID,
                ClientSecret = _marketPlace == "SG" ? setup.SGClientSecret :
                               _marketPlace == "MX" ? setup.MXClientSecret : setup.ClientSecret,
                MarketPlace = _marketPlace == "SG" ? MarketPlace.GetMarketPlaceByID(setup.SGMarketplaceID) :
                              _marketPlace == "US" ? MarketPlace.GetMarketPlaceByID(setup.USMarketplaceID) :
                              _marketPlace == "MX" ? MarketPlace.GetMarketPlaceByID(setup.MXMarketplaceID) :
                              _marketPlace == "EU" ? MarketPlace.GetMarketPlaceByID(setup.EUMarketplaceID) :
                              _marketPlace == "JP" ? MarketPlace.GetMarketPlaceByID(setup.JPMarketplaceID) : MarketPlace.GetMarketPlaceByID(setup.AUMarketplaceID),
                RefreshToken = _marketPlace == "SG" ? setup.SGRefreshToken :
                               _marketPlace == "US" ? setup.USRefreshToken :
                               _marketPlace == "MX" ? setup.MXRefreshToken :
                               _marketPlace == "EU" ? setup.EURefreshToken :
                               _marketPlace == "JP" ? setup.JPRefreshToken : setup.AURefreshToken
            });
        }

        /// <summary> 產生一筆固定資料 </summary>
        public virtual void InitialData()
        {
            string screenIDWODot = this.Accessinfo.ScreenID.ToString().Replace(".", "");

            PXDatabase.Insert<LUMAmazonSettlementTransData>(
                                 new PXDataFieldAssign<LUMAmazonSettlementTransData.marketplace>("Default"),
                                 new PXDataFieldAssign<LUMAmazonSettlementTransData.createdByID>(this.Accessinfo.UserID),
                                 new PXDataFieldAssign<LUMAmazonSettlementTransData.createdByScreenID>(screenIDWODot),
                                 new PXDataFieldAssign<LUMAmazonSettlementTransData.createdDateTime>(this.Accessinfo.BusinessDate),
                                 new PXDataFieldAssign<LUMAmazonSettlementTransData.lastModifiedByID>(this.Accessinfo.UserID),
                                 new PXDataFieldAssign<LUMAmazonSettlementTransData.lastModifiedByScreenID>(screenIDWODot),
                                 new PXDataFieldAssign<LUMAmazonSettlementTransData.lastModifiedDateTime>(this.Accessinfo.BusinessDate));
        }

        /// <summary> 刪除固定資料 </summary>
        public virtual void DeleteDefaultData()
            => PXDatabase.Delete<LUMAmazonSettlementTransData>(
                   new PXDataFieldRestrict<LUMAmazonSettlementTransData.marketplace>("Default"));

        public virtual DateTime? GetDepositDate(string settlementid)
            => SelectFrom<LUMAmazonSettlementTransData>
               .Where<LUMAmazonSettlementTransData.settlementID.IsEqual<P.AsString>
                 .And<LUMAmazonSettlementTransData.isProcessed.IsEqual<False>.Or<LUMAmazonSettlementTransData.isProcessed.IsNull>>
                 .And<LUMAmazonSettlementTransData.depositDate.IsNotNull>>
               .View.SelectSingleBound(this, null, settlementid).TopFirst?.DepositDate;

        public virtual string GetMarketplaceName(string saleschannel)
        {
            var splitIdx = saleschannel.LastIndexOf('.');
            if (splitIdx >= 0)
            {
                saleschannel = saleschannel.Substring(splitIdx + 1);
                saleschannel = saleschannel == "com" ? "US" : saleschannel.ToUpper();
            }
            else
                throw new Exception("is not match marketplace rule");
            return saleschannel;
        }

        #endregion

    }

    [Serializable]
    public class SettlementFilter : IBqlTable
    {
        [PXDBDate]
        [PXUIField(DisplayName = "From Date")]
        public virtual DateTime? FromDate { get; set; }
        public abstract class fromDate : PX.Data.BQL.BqlDateTime.Field<fromDate> { }

        [PXDBDate]
        [PXUIField(DisplayName = "To Date")]
        public virtual DateTime? ToDate { get; set; }
        public abstract class toDate : PX.Data.BQL.BqlDateTime.Field<toDate> { }

        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXDefault("")]
        [PXUIField(DisplayName = "Process type")]
        [PXStringList(new string[] { "Prepare Data", "Process Payment" }, new string[] { "Prepare Data", "Process Payment" })]
        public virtual string ProcessType { get; set; }
        public abstract class processType : PX.Data.BQL.BqlString.Field<processType> { }
    }
}
