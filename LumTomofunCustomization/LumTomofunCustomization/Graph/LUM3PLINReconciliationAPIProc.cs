using PX.Common;
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
    public class LUM3PLINReconciliationAPIProc : PXGraph<LUM3PLINReconciliationAPIProc>
    {
        #region Features & Selects
        public PXCancel<LUM3PLINReconciliation> Cancel;

        public PXProcessing<LUM3PLINReconciliation, Where<LUM3PLINReconciliation.thirdPLType, Equal<ThirdPLType.topest>>, OrderBy<Desc<LUM3PLINReconciliation.tranDate>>> TopestReconciliation;

        public SelectFrom<LUM3PLINReconciliation>.Where<LUM3PLINReconciliation.thirdPLType.IsEqual<ThirdPLType.returnHelper>>.OrderBy<LUM3PLINReconciliation.tranDate.Desc>.View RHReconciliation;

        public SelectFrom<LUM3PLINReconciliation>.Where<LUM3PLINReconciliation.thirdPLType.IsEqual<ThirdPLType.fedEx>>.OrderBy<LUM3PLINReconciliation.tranDate.Desc>.View FedExReconciliation;

        public SelectFrom<LUM3PLINReconciliation>.Where<LUM3PLINReconciliation.thirdPLType.IsEqual<ThirdPLType.googleSheets>>.OrderBy<LUM3PLINReconciliation.tranDate.Desc>.View GSheetsReconciliation;

        public PXSetup<LUM3PLSetup> Setup;
        #endregion

        #region Ctor
        public LUM3PLINReconciliationAPIProc()
        {
            if (TopestReconciliation.Select().Count == 0) { InsertInitializedData(this, ThirdPLType.Topest); }

            TopestReconciliation.SetProcessAllCaption("Import 3PL IN");
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
            LUM3PLINReconciliationAPIProc graph = CreateInstance<LUM3PLINReconciliationAPIProc>();

            graph.Import3PLRecords();
        }

        public static void Update3PLINReconciliation(PXCache cache, LUM3PLINReconciliation record)
        {
            if (record == null) { return; }

            LUMAmzINReconciliationProc graph = new LUMAmzINReconciliationProc();

            record.ERPSku = graph.GetStockItemOrCrossRef((record.Sku ?? string.Empty).Trim());
            record.Location = record.Location ?? graph.GetLocationIDByWarehouse(record.Warehouse, record.DetailedDesc);

            if (record.ProductName == null)
            {
                record.ProductName = InventoryItem.UK.Find(graph, record.ERPSku)?.Descr;
            }

            cache.Update(record);
        }

        public static void InsertInitializedData(PXGraph graph, string tPLType)
        {
            string screenIDWODot = graph.Accessinfo.ScreenID.ToString().Replace(".", "");

            PXDatabase.Insert<LUM3PLINReconciliation>(new PXDataFieldAssign<LUM3PLINReconciliation.createdByID>(graph.Accessinfo.UserID),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.createdByScreenID>(screenIDWODot),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.createdDateTime>(graph.Accessinfo.BusinessDate),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.lastModifiedByID>(graph.Accessinfo.UserID),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.lastModifiedByScreenID>(screenIDWODot),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.lastModifiedDateTime>(graph.Accessinfo.BusinessDate),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.noteID>(Guid.NewGuid()),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.thirdPLType>(tPLType),
                                                     new PXDataFieldAssign<LUM3PLINReconciliation.isProcessed>(false));
        }

        public static void DeleteDataByScript(bool isTW, DateTime? iNDate = null)
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

        #region Methods
        public virtual void Import3PLRecords()
        {
            LUM3PLSetup setup = Setup.Select();

            DeleteDataByScript(false);

            try
            {
                CreateDataFromTopest(setup);
            }
            catch (Exception e)
            {
                PXProcessing<LUM3PLINReconciliation>.SetError(e);
            }
            try
            {
                CreateDataFromRH(setup);
            }
            catch (Exception e)
            {
                PXProcessing<LUM3PLINReconciliation>.SetError(e);
            }
            try
            {
                CreateDataFromFedEx(setup);
            }
            catch (Exception e)
            {
                PXProcessing<LUM3PLINReconciliation>.SetError(e);
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
        public virtual LUMAPIResults GetRHAllCountriesWarehouse(string authzToken, string aPIKey, string aPIToken, string countryCode)
        {
            bool noCtryCode = string.IsNullOrEmpty(countryCode);

            var helper = new LUMAPIHelper(new LUMAPIConfig()
                                          {
                                              RequestMethod = HttpMethod.Get,
                                              RequestUrl = noCtryCode == false ? $"https://api.returnhelpercentre.com/v1/user/api/warehouse/getWarehouseByFromCountry" : 
                                                                                 $"https://api.returnhelpercentre.com/v1/public/api/country/getAllCountries",
                                              AuthType = "Bearer",
                                              Token = authzToken
                                          },
                                          new Dictionary<string, string>()
                                          {
                                              { "x-rr-apikey", aPIKey },
                                              { "x-rr-apitoken", aPIToken },
                                              { "Content-Type", "application/json" }
                                          });

            return helper.GetResults(noCtryCode == false ? $"?countryCode={countryCode}" : string.Empty);
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

            return helper.GetResults(pageCounts > 0 ? $"?pageSize=100&warehouseId={warehouse}&handlingCode=tbc&offset={pageCounts}" : $"?pageSize=1&warehouseId={warehouse}&handlingCode=tbc");
        }

        private void CreateDataFromRH(LUM3PLSetup setup)
        {
            var countries = LUMAPIHelper.DeserializeJSONString<ReturnHelperEntity.CountryRoot>(GetRHAllCountriesWarehouse(setup.RHAuthzToken, setup.RHApiKey, setup.RHApiToken, null).ContentResult);

            Dictionary<int, int> dic = new Dictionary<int, int>();
            // #1, get all the defined warehouses and calculate how many pages of transaction records each warehouse has.
            for (int i = 0; i < countries?.countries?.Count; i++)
            {
                //if (countries?.countries[i].countryCode != "chn") { continue; }

                var warehouses = LUMAPIHelper.DeserializeJSONString<ReturnHelperEntity.WarehouseRoot>(GetRHAllCountriesWarehouse(setup.RHAuthzToken, setup.RHApiKey, setup.RHApiToken, countries?.countries[i].countryCode).ContentResult);

                for (int j = 0; j < warehouses?.warehouses?.Count; j++)
                {
                    var inventories = LUMAPIHelper.DeserializeJSONString<ReturnHelperEntity.ReturnInvtRoot>(GetRHReturnInventory(setup.RHAuthzToken, setup.RHApiKey, setup.RHApiToken, warehouses.warehouses[j].warehouseId).ContentResult);

                    // if totalNumberOfRecords % 100 > 0 -> (totalNumberOfRecords / 100) + 2 else (totalNumberOfRecords / 100)
                    int quotient = inventories.totalNumberOfRecords / 100;
                    dic.Add(warehouses.warehouses[j].warehouseId, (inventories.totalNumberOfRecords % 100) > 0 ? quotient + 2 : quotient);
                }
            }

            List<LUM3PLINReconciliation> lists = new List<LUM3PLINReconciliation>();

            foreach(var key in dic.Keys)
            {
                dic.TryGetValue(key, out int value);

                // #2, get whole transaction records of each page.
                for (int j = value; j > 0; j--)
                {
                    var inventories = LUMAPIHelper.DeserializeJSONString<ReturnHelperEntity.ReturnInvtRoot>(GetRHReturnInventory(setup.RHAuthzToken, setup.RHApiKey, setup.RHApiToken, key, j * 100).ContentResult);
                    // #3, get the inventory data from API.
                    for (int k = 0; k < inventories?.returnInventoryList?.Count; k++)
                    {
                        // Only these two states need to be included in the cost quantity.
                        if (!inventories.returnInventoryList[k].handlingStatusCode.IsIn("pending", "onhold")) { continue; }

                        LUM3PLWarehouseMapping wHMapping = LUM3PLWarehouseMapping.PK.Find(this, ThirdPLType.ReturnHelper, Convert.ToString(inventories.returnInventoryList[k].warehouseId));

                        string whRemarks = inventories.returnInventoryList[k].warehouseRemarks, rMACode = inventories.returnInventoryList[k].itemRma;
                        bool   newItem   = rMACode.Contains("NEW");
                        // if 'Warehouse remark' is not Empty then ='Warehouse Remark',
                        // if 'SKU' is not Empty and 'Warehouse remark' is Empty then = 'SKU',
                        // if 'RMA Code' 有 NEW 抓 'NEW -' 之後的 SKU
                        string sku = !string.IsNullOrEmpty(whRemarks) ? whRemarks :
                                                                        newItem ? rMACode.Split('-')[2] :
                                                                        !string.IsNullOrEmpty(inventories.returnInventoryList[k].sku) &&
                                                                        string.IsNullOrEmpty(whRemarks) ? inventories.returnInventoryList[k].sku : null;

                        int assignSku = newItem ? Convert.ToInt32(inventories.returnInventoryList[k].sku) : 0;

                        LUM3PLINReconciliation newData = new LUM3PLINReconciliation()
                        {
                            ThirdPLType = ThirdPLType.ReturnHelper,
                            TranDate = Accessinfo.BusinessDate.Value.ToUniversalTime(),
                            INDate = Accessinfo.BusinessDate,
                            Sku = sku,
                            ProductName = null,
                            Qty = assignSku > 0 ? assignSku : 1,
                            // If (Upper('SKU') contains 'GRADE-C' or 'GRADE C') or ('SKU' is not Empty and 'Warehouse Remark' is Empty) then Location = '602' else Location = 601'
                            DetailedDesc = (!string.IsNullOrEmpty(sku) && (sku.ToUpper().Contains("GRADE-C") || sku.ToUpper().Contains("GRADE C"))) ||
                                           (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(whRemarks) && newItem == false) ? "NON-SELLABLE" : "SELLABLE",
                            CountryID = wHMapping?.CountryID,
                            Warehouse = wHMapping?.ERPWH,
                            FBACenterID = key.ToString(),
                            RMACode = rMACode,
                            WHRemarks = inventories.returnInventoryList[k].warehouseRemarks,
                            AssignSku = assignSku
                        };

                        // #3, Since this 3PL only has inventory transaction records, if these fields are the same, they can only be accumulated to calculate the quantity.
                        LUM3PLINReconciliation existed = lists.Find(e => e.TranDate == newData.TranDate && e.Sku == newData.Sku && e.DetailedDesc == newData.DetailedDesc &&
                                                                         e.CountryID == newData.CountryID && e.Warehouse == newData.Warehouse);
                        
                        if (existed == null)
                        {
                            lists.Add(newData);
                        }
                        else
                        {
                            existed.Qty += 1;
                        }
                    }
                }
            }

            for (int x = 0; x < lists.Count; x++)
            {
                Update3PLINReconciliation(RHReconciliation.Cache, RHReconciliation.Insert(lists[x]));
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
        #endregion
    }
}