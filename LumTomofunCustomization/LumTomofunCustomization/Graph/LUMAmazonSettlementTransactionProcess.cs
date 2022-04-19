using FikaAmazonAPI;
using FikaAmazonAPI.ReportGeneration;
using FikaAmazonAPI.Utils;
using LUMTomofunCustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.Graph
{
    public class LUMAmazonSettlementTransactionProcess : PXGraph<LUMAmazonSettlementTransactionProcess>
    {
        public PXSave<LUMAmazonSettlementTransData> Save;
        public PXCancel<LUMAmazonSettlementTransData> Cancel;
        public PXFilter<SettlementFilter> Filter;
        public PXFilteredProcessing<LUMAmazonSettlementTransData, SettlementFilter> SettlementTransaction;
        public SelectFrom<LUMMWSPreference>.View Setup;

        [InjectDependency]
        private ILegacyCompanyService _legacyCompanyService { get; set; }

        public LUMAmazonSettlementTransactionProcess()
        {
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
            baseGraph.DeleteDefaultData();
            baseGraph.PreparePaymentData(baseGraph, filter);
        }

        /// <summary> 執行 Get Amazon Payment Data </summary>
        public virtual void PreparePaymentData(LUMAmazonSettlementTransactionProcess baseGraph, SettlementFilter filter)
        {
            try
            {
                var actCompanyName = _legacyCompanyService.ExtractCompany(PX.Common.PXContext.PXIdentity.IdentityName);
                Dictionary<string, AmazonConnection> amzConnObjs = new Dictionary<string, AmazonConnection>();
                // TW Tenant要執行兩次
                if (actCompanyName == "TW")
                {
                    amzConnObjs.Add("EU", GetAmazonConnObject("EU"));
                    amzConnObjs.Add("AU", GetAmazonConnObject("AU"));
                }
                else
                    amzConnObjs.Add(actCompanyName, GetAmazonConnObject(actCompanyName));
                foreach (var dic in amzConnObjs)
                {
                    ReportManager reportManager = new ReportManager(dic.Value);
                    foreach (var item in reportManager.GetSettlementOrderAsync(new DateTime(2022, 4, 16), new DateTime(2022, 4, 20)).Result)
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
                        trans.PostedDate = item.PostedDate;
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

        public virtual AmazonConnection GetAmazonConnObject(string _marketPlace)
        {
            var setup = this.Setup.Select().TopFirst;
            if (setup == null)
                throw new Exception("MWS Preference is null");
            return new AmazonConnection(new AmazonCredential()
            {
                AccessKey = setup.AccessKey,
                SecretKey = setup.SecretKey,
                RoleArn = setup.RoleArn,
                ClientId = setup.ClientID,
                ClientSecret = setup.ClientSecret,
                MarketPlace = _marketPlace == "US" ? MarketPlace.GetMarketPlaceByID(setup.USMarketplaceID) :
                               _marketPlace == "EU" ? MarketPlace.GetMarketPlaceByID(setup.EUMarketplaceID) :
                               _marketPlace == "JP" ? MarketPlace.GetMarketPlaceByID(setup.JPMarketplaceID) : MarketPlace.GetMarketPlaceByID(setup.AUMarketplaceID),
                RefreshToken = _marketPlace == "US" ? setup.USRefreshToken :
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

        public virtual void DeleteDefaultData()
            => PXDatabase.Delete<LUMAmazonSettlementTransData>(
                   new PXDataFieldRestrict<LUMAmazonSettlementTransData.marketplace>("Default"));

        #endregion

    }

    [Serializable]
    public class SettlementFilter : IBqlTable
    {
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXDefault("Prepare Data")]
        [PXUIField(DisplayName = "Process type")]
        [PXStringList(new string[] { "Prepare Data" }, new string[] { "Prepare Data" })]
        public virtual string ProcessType { get; set; }
        public abstract class processType : PX.Data.BQL.BqlString.Field<processType> { }
    }
}
