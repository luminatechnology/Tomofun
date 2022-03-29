using LUMTomofunCustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LumTomofunCustomization.API_Entity;

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

            PXUIFieldAttribute.SetEnabled<LUMShopifySourceData.branchID>(ShopifySourceData.Cache,null,true);
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
                try
                {
                    switch (data.TransactionType)
                    {
                        case "Shopify Orders":
                            // 逐筆解析Json + 新增資料
                            foreach (var item in JsonConvert.DeserializeObject<ShopifyOrderEntity>(data.JsonSource).orders)
                            {
                                var trans = graph.ShopifyTransaction.Insert((LUMShopifyTransData)graph.ShopifyTransaction.Cache.CreateInstance());
                                trans.BranchID = data.BranchID;
                                trans.Apitype = data.APIType;
                                trans.TransactionType = data.TransactionType;
                                trans.Marketplace = data.Marketplace;
                                trans.SequenceNumber = data.SequenceNumber;
                                trans.OrderID = item.id.ToString();
                                trans.TransJson = JsonConvert.SerializeObject(item);
                            }
                            break;
                        case "Shopify Payment":
                            // 逐筆解析Json + 新增資料
                            foreach (var item in JsonConvert.DeserializeObject<List<API_Entity.ShopifyPayment.ShopifyPaymentEntity>>(data.JsonSource))
                            {
                                var trans = graph.ShopifyTransaction.Insert((LUMShopifyTransData)graph.ShopifyTransaction.Cache.CreateInstance());
                                trans.BranchID = data.BranchID;
                                trans.Apitype = data.APIType;
                                trans.TransactionType = data.TransactionType;
                                trans.Marketplace = data.Marketplace;
                                trans.SequenceNumber = data.SequenceNumber;
                                trans.OrderID = item.id.ToString();
                                trans.TransJson = JsonConvert.SerializeObject(item);
                            }
                            break;
                    }
                    data.IsProcessed = true;
                    this.ShopifySourceData.Update(data);
                    graph.Actions.PressSave();
                }
                catch (Exception ex)
                {
                    PXProcessing.SetError(ex.Message);
                }
            }
            this.Actions.PressSave();
        }

        #endregion

    }
}
