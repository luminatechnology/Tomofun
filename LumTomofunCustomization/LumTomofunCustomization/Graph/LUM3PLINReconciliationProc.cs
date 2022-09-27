using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using System;
using System.Net.Http;
using System.Collections.Generic;
using LUMTomofunCustomization.DAC;
using LumTomofunCustomization.API_Helper;
using LumTomofunCustomization.API_Entity;

namespace LUMTomofunCustomization.Graph
{
    public class LUM3PLINReconciliationProc : PXGraph<LUM3PLINReconciliationProc>
    {
        #region Features & Selects
        public PXCancel<LUM3PLINReconciliation> Cancel;

        public PXProcessing<LUM3PLINReconciliation, Where<LUM3PLINReconciliation.thirdPLType, Equal<ThirdPLType.topest>>> TopestReconciliation;

        public SelectFrom<LUM3PLINReconciliation>.Where<LUM3PLINReconciliation.thirdPLType.IsEqual<ThirdPLType.returnHelper>>.View RHReconciliation;

        public SelectFrom<LUM3PLINReconciliation>.Where<LUM3PLINReconciliation.thirdPLType.IsEqual<ThirdPLType.fedEx>>.View FedExReconciliation;

        public SelectFrom<LUM3PLINReconciliation>.Where<LUM3PLINReconciliation.thirdPLType.IsEqual<ThirdPLType.googleSheets>>.View GSheetsReconciliation;

        public PXSetup<LUM3PLSetup> Setup;
        #endregion

        #region Ctor
        public LUM3PLINReconciliationProc()
        {
            if (TopestReconciliation.Select().Count == 0) { InsertInitializedData(); }

            TopestReconciliation.SetProcessVisible(false);
            TopestReconciliation.SetProcessDelegate(delegate(List<LUM3PLINReconciliation> lists)
            {
                ImportRecords(lists);
            });
        }
        #endregion

        #region Static Methods
        public static void ImportRecords(List<LUM3PLINReconciliation> lists)
        {
            LUM3PLINReconciliationProc graph = CreateInstance<LUM3PLINReconciliationProc>();

            graph.Import3PLRecords();
        }
        #endregion

        #region Methods
        public virtual void Import3PLRecords()
        {
            LUM3PLSetup setup = Setup.Select();

            if (this.Accessinfo.CompanyName == "US")
            {
                DeleteDataByScript(false);

                try
                {
                    CreateDataFromTopest(setup);
                }
                catch (Exception e)
                {
                    PXProcessing<LUM3PLINReconciliation>.SetError(e);
                }
                //try
                //{
                //    CreateDataFromRH(setup);
                //}
                //catch (Exception e)
                //{
                //    PXProcessing<LUM3PLINReconciliation>.SetError(e);
                //}
                try
                {
                    CreateDataFromFedEx(setup);
                }
                catch (Exception e)
                {
                    PXProcessing<LUM3PLINReconciliation>.SetError(e);
                }
            }
            else if (this.Accessinfo.CompanyName == "TW")
            {
                try
                {
                    DeleteDataByScript(true);
                    CreateDateFromGSheets(setup);
                }
                catch (Exception e)
                {
                    PXProcessing<LUM3PLINReconciliation>.SetError(e);
                    throw;
                }
            }

            this.Actions.PressSave();
        }

        #region Topest
        public virtual LUMAPIResults GetTopestStockList(string token)
        {
            var helper = new LUMAPIHelper(new LUMAPIConfig()
                                          {
                                              RequestMethod = HttpMethod.Post,
                                              RequestUrl = @"http://oms.topestexpress.com/WebService/PublicService.asmx/GetStockListJson"
                                          },
                                          new Dictionary<string, string>()
                                          {
                                              {"Token", token }
                                          });

            return helper.GetResults();
        }

        public virtual LUMAPIResults GetTopestProductList(string token, string sKU)
        {
            var helper = new LUMAPIHelper(new LUMAPIConfig()
                                          {
                                              RequestMethod = HttpMethod.Post,
                                              RequestUrl = @"http://oms.topestexpress.com/WebService/PublicService.asmx/GetProductList"
                                          },
                                          new Dictionary<string, string>()
                                          {
                                              { "Token", token },
                                              { "SKU", sKU}
                                          });

            return helper.GetResults();
        }

        public virtual LUMAPIResults GetTopestInventoryList(string token, string sKU, int stockID)
        {
            var helper = new LUMAPIHelper(new LUMAPIConfig()
                                          {
                                              RequestMethod = HttpMethod.Post,
                                              RequestUrl = @"http://oms.topestexpress.com/WebService/PublicService.asmx/GetInventoryList"
            },
                                          new Dictionary<string, string>()
                                          {
                                              {"Token", token },
                                              { "SKU", sKU},
                                              { "StockID", stockID.ToString()}
                                          });

            return helper.GetResults();
        }

        private void CreateDataFromTopest(LUM3PLSetup setup)
        {
            string[] countries = new string[] { "US", "CA" };

            for (int a = 0; a < countries.Length; a++)
            {
                string token = countries[a] == "CA" ? setup.TopestTokenCA : setup.TopestToken;

                var stocks = LUMAPIHelper.DeserializeJSONString<TopestEntity.StockRoot>(GetTopestStockList(token).ContentResult);

                string prodConRes = GetTopestProductList(token, null).ContentResult;

                var products = LUMAPIHelper.DeserializeJSONString<TopestEntity.ProductRoot>(prodConRes.Substring(prodConRes.IndexOf('{'), prodConRes.LastIndexOf('}') - prodConRes.IndexOf('{') + 1));

                DeleteDataByScript(false, Accessinfo.BusinessDate);

                for (int i = 0; i < stocks?.data?.Count; i++)
                {
                    for (int j = 0; j < products?.data?.Count; j++)
                    {
                        string invtConRes = GetTopestInventoryList(token, products.data[j].SKU, stocks.data[i].StockID).ContentResult;

                        var inventories = LUMAPIHelper.DeserializeJSONString<TopestEntity.InventoryRoot>(invtConRes.Substring(invtConRes.IndexOf('{'), invtConRes.LastIndexOf('}') - invtConRes.IndexOf('{') + 1));

                        for (int k = 0; k < inventories?.data?.Count; k++)
                        {
                            Update3PLINReconciliation(TopestReconciliation.Cache, TopestReconciliation.Insert(new LUM3PLINReconciliation()
                                                                                                              {
                                                                                                                  ThirdPLType = ThirdPLType.Topest,
                                                                                                                  TranDate = Accessinfo.BusinessDate.Value.ToUniversalTime(),
                                                                                                                  INDate = Accessinfo.BusinessDate,
                                                                                                                  Sku = products.data[j].SKU,
                                                                                                                  ProductName = products.data[j].EnName,
                                                                                                                  Qty = inventories.data[k].AvailableQty,
                                                                                                                  DetailedDesc = "SELLABLE",
                                                                                                                  CountryID = countries[a],
                                                                                                                  Warehouse = LUM3PLWarehouseMapping.PK.Find(this, ThirdPLType.Topest, stocks.data[i].Code)?.ERPWH,
                                                                                                                  FBACenterID = stocks.data[i].Code
                                                                                                              }));
                        }
                    }
                }
            }
        }
        #endregion

        #region Return Helper
        public virtual LUMAPIResults GetRHWarehouseByCountry(string authzToken, string aPIKey, string aPIToken, string countryCode)
        {          
            var helper = new LUMAPIHelper(new LUMAPIConfig()
                                          {
                                              RequestMethod = HttpMethod.Get,
                                              RequestUrl = $"https://api.returnhelpercentre.com/v1/user/api/warehouse/getWarehouseByFromCountry",
                                              AuthType = "Bearer",
                                              Token = authzToken
                                          },
                                          new Dictionary<string, string>()
                                          {
                                              { "x-rr-apikey", aPIKey },
                                              { "x-rr-apitoken", aPIToken },
                                              { "Content-Type", "application/json" }
                                          });

            return helper.GetResults($"?countryCode={countryCode}");
        }

        public virtual LUMAPIResults GetRHReturnInventory(string authzToken, string aPIKey, string aPIToken, int warehouse, int pageCounts = 0)
        {
            var helper = new LUMAPIHelper(new LUMAPIConfig()
                                          {
                                              RequestMethod = HttpMethod.Get,
                                              RequestUrl = $"https://api.returnhelpercentre.com/v1/user/api/returninventory/searchReturnInventory",
                                              AuthType = "Bearer",
                                              Token = authzToken
                                          },
                                          new Dictionary<string, string>()
                                          {
                                              { "x-rr-apikey", aPIKey },
                                              { "x-rr-apitoken", aPIToken },
                                              { "Content-Type", "application/json" }
                                          });

            return helper.GetResults(pageCounts > 0 ? $"?pageSize=100&warehouseId={warehouse}&handlingCode=tbc&offset={pageCounts}" : 
                                                      $"?pageSize=1&warehouseId={warehouse}&handlingCode=tbc");
        }

        private void CreateDataFromRH(LUM3PLSetup setup)
        {
            string[] fixedCountries = new string[] { "jpn", "gbr", "deu", "aus", "can" };

            Dictionary<int, int> dic = new Dictionary<int, int>();

            for (int i = 0; i < fixedCountries.Length; i++)
            {
                var warehouses = LUMAPIHelper.DeserializeJSONString<ReturnHelperEntity.WarehouseRoot>(GetRHWarehouseByCountry(setup.RHAuthzToken, setup.RHApiKey, setup.RHApiToken, fixedCountries[i]).ContentResult);

                for (int j = 0; j < warehouses?.warehouses?.Count; j++)
                {
                    var inventories = LUMAPIHelper.DeserializeJSONString<ReturnHelperEntity.ReturnInvtRoot>(GetRHReturnInventory(setup.RHAuthzToken, setup.RHApiKey, setup.RHApiToken, warehouses.warehouses[j].warehouseId).ContentResult);

                    // if totalNumberOfRecords % 100 > 0 -> (totalNumberOfRecords / 100) + 2 else (totalNumberOfRecords / 100)
                    int quotient = inventories.totalNumberOfRecords / 100;
                    dic.Add(warehouses.warehouses[j].warehouseId, (inventories.totalNumberOfRecords % 100) > 0 ? quotient + 2 : quotient);
                }
            }

            foreach(var key in dic.Keys)
            {
                dic.TryGetValue(key, out int value);

                var inventories = LUMAPIHelper.DeserializeJSONString<ReturnHelperEntity.ReturnInvtRoot>(GetRHReturnInventory(setup.RHAuthzToken, setup.RHApiKey, setup.RHApiToken, key, value * 100).ContentResult);

                for (int k = 0; k < inventories?.returnInventoryList?.Count; k++)
                {
                    Update3PLINReconciliation(RHReconciliation.Cache, RHReconciliation.Insert(new LUM3PLINReconciliation()
                                                                                              {
                                                                                                  ThirdPLType = ThirdPLType.ReturnHelper,
                                                                                                  TranDate = null,//inventories.transactionDate,
                                                                                                  Sku = inventories.returnInventoryList[k].sku,
                                                                                                  ProductName = null,
                                                                                                  Qty = k,
                                                                                                  DetailedDesc = "SELLABLE",
                                                                                                  CountryID = "US",
                                                                                                  Warehouse = null//PX.Objects.IN.INSite.UK.Find(this, "3PLUS00").SiteID
                                                                                              }));
                }
            }
        }
        #endregion

        #region FedEx
        public virtual LUMAPIResults GetFedExInventory(LUM3PLSetup setup)
        {
            string newAccessToken = setup.FedExAccessToken;

            Reacquire:
            var helper = new LUMAPIHelper(new LUMAPIConfig()
                                          {
                                              RequestMethod = HttpMethod.Get,
                                              RequestUrl = @"https://connect.supplychain.fedex.com/api/v1/inventory",
                                              AuthType = "Bearer",
                                              Token = newAccessToken
                                          }, 
                                          new Dictionary<string, string>());

            LUMAPIResults aPIResult = helper.GetResults();

            if (aPIResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                newAccessToken = GetNewAccessToken(setup);

                goto Reacquire;
            }

            return aPIResult;
        }

        public virtual string GetNewAccessToken(LUM3PLSetup setup)
        {
            var helper = new LUMAPIHelper(new LUMAPIConfig()
                                          {
                                              RequestMethod = HttpMethod.Post,
                                              RequestUrl = @"https://kxmq24ujj4.execute-api.us-east-1.amazonaws.com/prod"
                                          }, null);
         
            LUMAPIResults aPIResult = helper.GetResults(LUMAPIHelper.SerialzeJSONString(new FedExEntity.AccessToken() { client_id = setup.FedExClientID, client_secret = setup.FedExClientSecret }));

            if (aPIResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(aPIResult.Content.ReadAsStringAsync().Result);
            }

            var access = LUMAPIHelper.DeserializeJSONString<FedExEntity.Access>(aPIResult.ContentResult);

            UpdateFedExNewToken(access.access_token);

            return access.access_token;
        }

        private void CreateDataFromFedEx(LUM3PLSetup setup)
        {
            var inventories = LUMAPIHelper.DeserializeJSONString<FedExEntity.Root>(GetFedExInventory(setup).ContentResult);

            DeleteDataByScript(false, inventories.transactionDate.Date);

            for (int i = 0; i < inventories?.inventory?.Count; i++)
            {
                Update3PLINReconciliation(FedExReconciliation.Cache, FedExReconciliation.Insert(new LUM3PLINReconciliation()
                                                                     {
                                                                         ThirdPLType = ThirdPLType.FedEx,
                                                                         TranDate = inventories.transactionDate,
                                                                         INDate = inventories.transactionDate.Date,
                                                                         Sku = inventories.inventory[i].sku,
                                                                         ProductName = null,
                                                                         Qty = Convert.ToInt32(inventories.inventory[i].availableCount),
                                                                         DetailedDesc = "SELLABLE",
                                                                         CountryID = "US",
                                                                         Warehouse = INSite.UK.Find(this, "3PLUS00")?.SiteID
                                                                     }));
            }
        }
        #endregion

        #region GoogleSheets
        public virtual LUMAPIResults GetSpecifiedSheet(LUM3PLSetup setup)
        {
            var helper = new LUMAPIHelper(new LUMAPIConfig()
                                          {
                                              RequestMethod = HttpMethod.Post,
                                              RequestUrl = setup.GoogleSheetsURL
                                          },
                                          null);

            return helper.GetResults(LUMAPIHelper.SerialzeJSONString(new GoogleSheetsEntity.RequestRoot() { SheetName = setup.GoogleSheetName }));
        }

        public virtual void CreateDateFromGSheets(LUM3PLSetup setup)
        {
            var sheets = LUMAPIHelper.DeserializeJSONString<GoogleSheetsEntity.DataRoot>(GetSpecifiedSheet(setup).ContentResult);

            // Since first array is label row.
            for (int i = 1; i < sheets.Data.Count; i++)
            {
                // ["Inventory Date", "Warehouse", "Location", "Sku", "Qty", "Country"]
                var row = sheets.Data[i];

                DateTime invetDate = Convert.ToDateTime(row[0]);
                 
                if (i == 1)
                {
                    DeleteDataByScript(true, invetDate.Date);
                }

                INSite site = INSite.UK.Find(this, Convert.ToString(row[1]));

                Update3PLINReconciliation(GSheetsReconciliation.Cache, 
                                          GSheetsReconciliation.Insert(new LUM3PLINReconciliation()
                                                                       {
                                                                           ThirdPLType = ThirdPLType.GoogleSheets,
                                                                           TranDate = invetDate,
                                                                           INDate = invetDate.Date,
                                                                           Sku = Convert.ToString(row[3]),
                                                                           ProductName = null,
                                                                           Qty = row[4].Equals("") ? 0m : Convert.ToInt32(row[4]),
                                                                           DetailedDesc = "SELLABLE",
                                                                           CountryID = Convert.ToString(row[5]),
                                                                           Warehouse = site?.SiteID,
                                                                           Location = PX.Objects.CR.Location.UK.Find(this, site.BAccountID, Convert.ToString(row[2]))?.LocationID
                                                                        }) );
            }          
        }
    
        #endregion

        /// <summary>
        /// Since the access token expires after each 3600(s) acquisition, it must be re-acquired again and the new value stored.
        /// </summary>
        /// <param name="newToken"></param>
        private void UpdateFedExNewToken(string newAccessToken)
        {
            PXUpdate<Set<LUM3PLSetup.fedExAccessToken, Required<LUM3PLSetup.fedExAccessToken>>,
                     LUM3PLSetup,
                     Where<LUM3PLSetup.fedExClientID, IsNotNull>>.Update(this, newAccessToken);
        }

        private void Update3PLINReconciliation(PXCache cache, LUM3PLINReconciliation record)
        {
            if (record == null) { return; }

            LUMAmzINReconciliationProc graph = new LUMAmzINReconciliationProc();

            record.ERPSku   = graph.GetStockItemOrCrossRef(record.Sku);
            record.Location = record.Location ?? graph.GetLocationIDByWarehouse(record.Warehouse, record.DetailedDesc);

            if (record.ProductName == null)
            {
                record.ProductName = InventoryItem.UK.Find(graph, record.ERPSku)?.Descr;
            }

            cache.Update(record);
        }

        private void InsertInitializedData()
        {
            string screenIDWODot = this.Accessinfo.ScreenID.ToString().Replace(".", "");

            PXDatabase.Insert<LUM3PLINReconciliation>(new PXDataFieldAssign<LUM3PLINReconciliation.createdByID>(this.Accessinfo.UserID),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.createdByScreenID>(screenIDWODot),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.createdDateTime>(this.Accessinfo.BusinessDate),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.lastModifiedByID>(this.Accessinfo.UserID),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.lastModifiedByScreenID>(screenIDWODot),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.lastModifiedDateTime>(this.Accessinfo.BusinessDate),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.noteID>(Guid.NewGuid()),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.thirdPLType>(ThirdPLType.Topest),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.isProcessed>(false));
        }

        private void DeleteDataByScript(bool isTW, DateTime? iNDate = null)
        {
            if (iNDate != null)
            {
                PXDatabase.Delete<LUM3PLINReconciliation>(new PXDataFieldRestrict<LUM3PLINReconciliation.isProcessed>(false),
                                                          new PXDataFieldRestrict<LUM3PLINReconciliation.iNDate>(PXDbType.DateTime, 8, iNDate, PXComp.EQ),
                                                          new PXDataFieldRestrict<LUM3PLINReconciliation.thirdPLType>(PXDbType.Char, 1, ThirdPLType.GoogleSheets, isTW == true ? PXComp.EQ : PXComp.NE));
            }
            else
            {
                PXDatabase.Delete<LUM3PLINReconciliation>(new PXDataFieldRestrict<LUM3PLINReconciliation.isProcessed>(false),
                                                          new PXDataFieldRestrict<LUM3PLINReconciliation.iNDate>(PXDbType.DateTime, 8, iNDate, PXComp.ISNULL));
            }
        }
        #endregion
    }
}