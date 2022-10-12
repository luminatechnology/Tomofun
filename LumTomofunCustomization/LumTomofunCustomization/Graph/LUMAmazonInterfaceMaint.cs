using LUMTomofunCustomization.DAC;
using Newtonsoft.Json;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.Graph
{
    public class LUMAmazonInterfaceMaint : PXGraph<LUMAmazonInterfaceMaint>
    {
        public PXSave<LUMAmazonSourceData> Save;
        public PXCancel<LUMAmazonSourceData> Cancel;

        public PXProcessing<LUMAmazonSourceData> AmazonSourceData;
        public SelectFrom<LUMAmazonSourceData>
               .Where<LUMAmazonSourceData.sequenceNumber.IsEqual<LUMAmazonSourceData.sequenceNumber.FromCurrent>>
               .View JsonViewer;

        public LUMAmazonInterfaceMaint()
        {
            AmazonSourceData.Cache.AllowInsert = AmazonSourceData.Cache.AllowUpdate = AmazonSourceData.Cache.AllowDelete = true;

            PXUIFieldAttribute.SetEnabled<LUMAmazonSourceData.branchID>(AmazonSourceData.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonSourceData.aPIType>(AmazonSourceData.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonSourceData.transactionType>(AmazonSourceData.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonSourceData.marketplace>(AmazonSourceData.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonSourceData.jsonSource>(AmazonSourceData.Cache, null, true);

            AmazonSourceData.SetProcessDelegate(
               delegate (List<LUMAmazonSourceData> list)
               {
                   GoProcessing(list);
               });
        }

        #region Action

        public PXAction<LUMAmazonSourceData> ViewJson;
        [PXButton]
        [PXUIField(DisplayName = "View Json", MapEnableRights = PXCacheRights.Select)]
        protected void viewJson()
        {
            if (JsonViewer.AskExt(true) != WebDialogResult.OK) return;
        }

        #endregion

        #region Method

        public static void GoProcessing(List<LUMAmazonSourceData> list)
        {
            var graph = CreateInstance<LUMAmazonInterfaceMaint>();
            graph.AnalyzeJsonData(list);
        }

        /// <summary> 解析Json </summary>
        public virtual void AnalyzeJsonData(List<LUMAmazonSourceData> dataSource)
        {
            var graph = PXGraph.CreateInstance<LUMAmazonTransactionProcess>();
            foreach (var data in dataSource)
            {
                try
                {
                    var isAllSkipped = true;
                    switch (data.TransactionType)
                    {
                        case "Amazon Orders":
                            // 逐筆解析Json + 新增資料
                            foreach (var item in JsonConvert.DeserializeObject<API_Entity.AmazonOrder.AmazonOrderEntity>(data.JsonSource).Orders)
                            {
                                if (!item.SalesChannel.ToUpper().StartsWith("AMAZON"))
                                    continue;
                                isAllSkipped = false;
                                var trans = graph.AmazonTransaction.Insert((LUMAmazonTransData)graph.AmazonTransaction.Cache.CreateInstance());
                                trans.BranchID = data.BranchID;
                                trans.Apitype = data.APIType;
                                trans.TransactionType = data.TransactionType;
                                trans.Marketplace = data.Marketplace;
                                trans.SequenceNumber = data.SequenceNumber;
                                trans.SalesChannel = item.SalesChannel;
                                trans.OrderStatus = item.OrderStatus;
                                trans.OrderID = item.OrderId.ToString();
                                trans.Amount = (decimal?)item.Amount;
                                trans.TransJson = JsonConvert.SerializeObject(item);
                            }
                            break;
                    }
                    data.IsProcessed = !isAllSkipped;
                    data.IsSkippedProcess = isAllSkipped;
                    this.AmazonSourceData.Update(data);
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
