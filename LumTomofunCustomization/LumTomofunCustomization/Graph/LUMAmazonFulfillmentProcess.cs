using FikaAmazonAPI;
using FikaAmazonAPI.Parameter.Report;
using FikaAmazonAPI.Utils;
using LUMTomofunCustomization.DAC;
using PX.Common;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FikaAmazonAPI.Utils.Constants;

namespace LumTomofunCustomization.Graph
{
    public class LUMAmazonFulfillmentProcess : PXGraph<LUMAmazonFulfillmentProcess>
    {
        public PXSave<FulfillmentFilter> Save;
        public PXCancel<FulfillmentFilter> Cancel;

        public PXFilter<FulfillmentFilter> Filter;
        [PXImport(typeof(LUMAmazonFulfillmentTransData))]
        public PXFilteredProcessing<LUMAmazonFulfillmentTransData, FulfillmentFilter> FulfillmentTransactions;
        public SelectFrom<LUMMWSPreference>.View Setup;

        [InjectDependency]
        private ILegacyCompanyService _legacyCompanyService { get; set; }

        public LUMAmazonFulfillmentProcess()
        {
            var filter = this.Filter.Current;
            this.FulfillmentTransactions.Cache.AllowInsert = this.FulfillmentTransactions.Cache.AllowUpdate = this.FulfillmentTransactions.Cache.AllowDelete = true;
            PXUIFieldAttribute.SetEnabled<LUMAmazonFulfillmentTransData.amazonOrderID>(this.FulfillmentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonFulfillmentTransData.marketPlace>(this.FulfillmentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonFulfillmentTransData.shipmentDate>(this.FulfillmentTransactions.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<LUMAmazonFulfillmentTransData.reportID>(this.FulfillmentTransactions.Cache, null, true);

            this.FulfillmentTransactions.SetProcessVisible(false);
            FulfillmentTransactions.SetProcessDelegate(delegate (List<LUMAmazonFulfillmentTransData> list)
            {
                GoProcessing(list, filter);
            });
            // Initial Data
            if (this.FulfillmentTransactions.Select().Count == 0)
                InitialData();
        }

        #region Events
        public virtual void _(Events.RowSelected<FulfillmentFilter> e)
        {
            var row = (FulfillmentFilter)e.Row;
            if (row != null)
            {
                if (!row.StartDate.HasValue)
                    row.StartDate = DateTime.Now.Date.AddDays(-1);
                if (!row.EndDate.HasValue)
                    row.EndDate = DateTime.Now.Date;
            }
        }

        #endregion

        #region Method

        /// <summary> 執行Process </summary>
        public static void GoProcessing(List<LUMAmazonFulfillmentTransData> list, FulfillmentFilter filter)
        {
            var baseGraph = CreateInstance<LUMAmazonFulfillmentProcess>();
            if ((filter.EndDate - filter.StartDate).Value.TotalDays > 60)
                throw new Exception("The time interval cannot exceed 60 days");
            baseGraph.DeleteDefaultData();
            baseGraph.PrepareFulfillmentData(baseGraph, filter);
        }

        public virtual void PrepareFulfillmentData(LUMAmazonFulfillmentProcess baseGraph, FulfillmentFilter filter)
        {
            try
            {
                var oldDatas = baseGraph.FulfillmentTransactions.Select().RowCast<LUMAmazonFulfillmentTransData>();
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
                foreach (var connItem in amzConnObjs)
                {
                    List<string> reportIDs = new List<string>();
                    // Get Fulfillment Report
                    var parameters = new ParameterReportList();
                    parameters.pageSize = 100;
                    parameters.reportTypes = new List<ReportTypes>();
                    parameters.reportTypes.Add(ReportTypes.GET_AMAZON_FULFILLED_SHIPMENTS_DATA_GENERAL);
                    parameters.marketplaceIds = new List<string>();
                    parameters.marketplaceIds.Add(GetAmazonMarketPlaceId(connItem.Key));
                    parameters.createdSince = filter.StartDate;
                    parameters.createdUntil = filter.EndDate;
                    var reports = connItem.Value.Reports.GetReports(parameters);
                    var reportFilePaths = new List<string>();
                    if (reports.Count == 0)
                        PXTrace.WriteWarning($"MarketPlace: {connItem.Key} report count = 0!!! ({DateTime.Now})");
                    // Get all report file txt
                    foreach (var reportData in reports)
                    {
                        reportIDs.Add(reportData.ReportId);
                        if (!string.IsNullOrEmpty(reportData.ReportDocumentId))
                            reportFilePaths.Add(connItem.Value.Reports.GetReportFile(reportData.ReportDocumentId));
                    }
                    int count = 0;
                    // Read txt file and insert data
                    foreach (var filepath in reportFilePaths)
                    {
                        using (StreamReader sr = new StreamReader(filepath, System.Text.Encoding.GetEncoding(connItem.Key == "JP" ? "Shift-JIS" : nameof(System.Text.Encoding.ASCII)), true))
                        {
                            DateTime newShipmentDate;
                            var data = sr.ReadToEnd().Split('\n');
                            // Skpe First row (header)
                            for (int idx = 1; idx < data.Length; idx++)
                            {
                                if (string.IsNullOrEmpty(data[idx]))
                                    continue;
                                var line = data[idx].Replace("\"", "").Split('\t');
                                // 代表不是用\t切割
                                if (line.Count() < 9)
                                    line = data[idx].Replace("\"", "").Split(',');
                                // Find old data
                                var oldRow = oldDatas.FirstOrDefault(x => x.AmazonOrderID == line[0] && x.MarketPlace == connItem.Key);
                                var isExists = oldRow == null ? false : true;
                                LUMAmazonFulfillmentTransData row = oldRow ?? new LUMAmazonFulfillmentTransData();
                                // 如果這張Order已經存在且Process過就不更新
                                if (!(row?.IsProcessed ?? false))
                                {
                                    // amazon-order-id
                                    row.AmazonOrderID = line[0];
                                    row.MarketPlace = connItem.Key;
                                    // shipmentDate
                                    if (DateTime.TryParse(line[8].Split('+')[0], out newShipmentDate))
                                        row.ShipmentDate = newShipmentDate;
                                    row.ReportID = reportIDs[count];
                                    row.IsProcessed = false;
                                    if (isExists)
                                        baseGraph.FulfillmentTransactions.Update(row);
                                    else
                                        baseGraph.FulfillmentTransactions.Insert(row);
                                }
                            }
                        }
                        count++;
                    }
                }// end Connection object
                baseGraph.Save.Press();
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

        /// <summary> 產生一筆固定資料 </summary>
        public virtual void InitialData()
        {
            string screenIDWODot = this.Accessinfo.ScreenID.ToString().Replace(".", "");

            PXDatabase.Insert<LUMAmazonFulfillmentTransData>(
            new PXDataFieldAssign<LUMAmazonFulfillmentTransData.amazonOrderID>("Default"),
            new PXDataFieldAssign<LUMAmazonFulfillmentTransData.marketPlace>("Default"),
            new PXDataFieldAssign<LUMAmazonFulfillmentTransData.createdByID>(this.Accessinfo.UserID),
            new PXDataFieldAssign<LUMAmazonFulfillmentTransData.createdByScreenID>(screenIDWODot),
            new PXDataFieldAssign<LUMAmazonFulfillmentTransData.createdDateTime>(this.Accessinfo.BusinessDate),
            new PXDataFieldAssign<LUMAmazonFulfillmentTransData.lastModifiedByID>(this.Accessinfo.UserID),
            new PXDataFieldAssign<LUMAmazonFulfillmentTransData.lastModifiedByScreenID>(screenIDWODot),
            new PXDataFieldAssign<LUMAmazonFulfillmentTransData.lastModifiedDateTime>(this.Accessinfo.BusinessDate));
        }

        /// <summary> 刪除固定資料 </summary>
        public virtual void DeleteDefaultData()
            => PXDatabase.Delete<LUMAmazonFulfillmentTransData>(
                   new PXDataFieldRestrict<LUMAmazonFulfillmentTransData.amazonOrderID>("Default"),
                   new PXDataFieldRestrict<LUMAmazonFulfillmentTransData.marketPlace>("Default"));

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

        public virtual string GetAmazonMarketPlaceId(string _marketPlace)
        {
            var setup = this.Setup.Select().TopFirst;
            if (setup == null)
                throw new Exception("MWS Preference is null");
            return _marketPlace == "SG" ? setup.SGMarketplaceID :
                   _marketPlace == "US" ? setup.USMarketplaceID :
                   _marketPlace == "MX" ? setup.MXMarketplaceID :
                   _marketPlace == "EU" ? setup.EUMarketplaceID :
                   _marketPlace == "JP" ? setup.JPMarketplaceID :
                   setup.AUMarketplaceID;
        }

        #endregion
    }

    [Serializable]
    public class FulfillmentFilter : IBqlTable
    {
        [PXDBDate]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = "Start Date")]
        public virtual DateTime? StartDate { get; set; }
        public abstract class startDate : PX.Data.BQL.BqlDateTime.Field<startDate> { }

        [PXDBDate]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = "End Date")]
        public virtual DateTime? EndDate { get; set; }
        public abstract class endDate : PX.Data.BQL.BqlDateTime.Field<endDate> { }

    }
}
