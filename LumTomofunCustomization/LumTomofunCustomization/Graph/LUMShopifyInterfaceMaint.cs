using LUMTomofunCustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PX.Objects.SO;
using PX.Data.BQL;

namespace LumTomofunCustomization.Graph
{
    public class LUMShopifyInterfaceMaint : PXGraph<LUMShopifyInterfaceMaint>
    {
        public PXSave<LUMShopifySourceData> Save;
        public PXCancel<LUMShopifySourceData> Cancel;
        public PXProcessing<LUMShopifySourceData> ShopifySourceData;
        public SelectFrom<LUMShopifySourceData>
               .Where<LUMShopifySourceData.sequenceNumber.IsEqual<LUMShopifySourceData.sequenceNumber.FromCurrent>>
               .View JsonViewer;

        public LUMShopifyInterfaceMaint()
        {
            ShopifySourceData.Cache.AllowInsert = ShopifySourceData.Cache.AllowUpdate = ShopifySourceData.Cache.AllowDelete = true;

            PXUIFieldAttribute.SetEnabled<LUMShopifySourceData.branchID>(ShopifySourceData.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMShopifySourceData.aPIType>(ShopifySourceData.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMShopifySourceData.transactionType>(ShopifySourceData.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMShopifySourceData.marketplace>(ShopifySourceData.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMShopifySourceData.jsonSource>(ShopifySourceData.Cache, null, true);

            ShopifySourceData.SetProcessDelegate(
               delegate (List<LUMShopifySourceData> list)
               {
                   GoProcessing(list);
               });
        }

        #region Action

        public PXAction<LUMShopifySourceData> ViewJson;
        [PXButton]
        [PXUIField(DisplayName = "View Json", MapEnableRights = PXCacheRights.Select)]
        protected void viewJson()
        {
            if (JsonViewer.AskExt(true) != WebDialogResult.OK) return;
        }

        #endregion

        #region Method

        public static void GoProcessing(List<LUMShopifySourceData> list)
        {
            var graph = CreateInstance<LUMShopifyInterfaceMaint>();
            graph.AnalyzeJsonData(list);
        }

        /// <summary> 解析Json </summary>
        public virtual void AnalyzeJsonData(List<LUMShopifySourceData> dataSource)
        {
            var graph = PXGraph.CreateInstance<LUMShopifyTransactionProcess>();
            foreach (var data in dataSource)
            {
                var isAllSkipped = true;
                try
                {
                    switch (data.TransactionType)
                    {
                        case "Shopify Orders":
                            // 逐筆解析Json + 新增資料
                            var order = JsonConvert.DeserializeObject<API_Entity.ShopifyOrder.ShopifyOrderEntity>(data.JsonSource);
                            if (order.financial_status.ToLower() != "paid")
                                continue;
                            // Shopify Order
                            var shopifySOOrder = SelectFrom<SOOrder>
                                                 .Where<SOOrder.customerOrderNbr.IsEqual<P.AsString>
                                                    .Or<SOOrder.customerRefNbr.IsEqual<P.AsString>>>
                                                 .View.Select(this, order.id, order.id).TopFirst;
                            if(shopifySOOrder != null && order.financial_status?.ToUpper() == "PAID" && string.IsNullOrEmpty(order.fulfillment_status))
                            { 
                                data.SkipReason = $"Sales Order is Exsits : {shopifySOOrder.OrderNbr}";
                                continue;
                            }
                            else if(shopifySOOrder != null && shopifySOOrder.Status != "N" && order.fulfillment_status?.ToUpper() == "FULFILLED")
                            {
                                data.SkipReason = $"Sales Order Status is not equeal OPEN : {shopifySOOrder.OrderNbr}";
                                continue;
                            }
                            isAllSkipped = false;
                            var orderTrans = graph.ShopifyTransaction.Insert((LUMShopifyTransData)graph.ShopifyTransaction.Cache.CreateInstance());
                            orderTrans.BranchID = data.BranchID;
                            orderTrans.Apitype = data.APIType;
                            orderTrans.TransactionType = data.TransactionType;
                            orderTrans.Marketplace = data.Marketplace;
                            orderTrans.SequenceNumber = data.SequenceNumber;
                            orderTrans.OrderID = order.id.ToString();
                            orderTrans.FullfillmentStatus = order.fulfillment_status ?? string.Empty;
                            orderTrans.FinancialStatus = order.financial_status;
                            orderTrans.TransJson = JsonConvert.SerializeObject(order);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    PXProcessing.SetError(ex.Message);
                }
                finally
                {
                    data.IsProcessed = !isAllSkipped;
                    data.IsSkippedProcess = isAllSkipped;
                    this.ShopifySourceData.Update(data);
                    graph.Actions.PressSave();
                }
            }
            this.Actions.PressSave();
        }

        #endregion

    }
}
