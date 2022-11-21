using LumTomofunCustomization.DAC;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.Licensing;
using PX.Objects.CS;
using PX.Objects.IN;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.Graph
{
    public class LUMMRPProcess : PXGraph<LUMMRPProcess>
    {
        public PXCancel<MRPFilter> Cancel;

        public PXFilter<MRPFilter> Filter;
        [PXFilterable]
        public PXFilteredProcessing<LUMMRPProcessResult, MRPFilter> Transaction;

        // ReSharper disable InconsistentNaming
        [InjectDependency]
        private ILegacyCompanyService _legacyCompanyService { get; set; }

        private List<DateTime> _transactionExistsDays = new List<DateTime>();

        #region Constructor

        public LUMMRPProcess()
        {
            var filter = this.Filter.Current;
            Transaction.SetProcessVisible(false);
            Transaction.SetProcessAllCaption("Process MRP");
            Transaction.SetProcessDelegate(
                delegate (List<LUMMRPProcessResult> list)
                {
                    GoProcessing(filter);
                });
            if (this.Transaction.Select().Count == 0 && SelectFrom<LUMMRPProcessResult>.View.Select(this).TopFirst == null)
                InitialData();
        }

        #endregion

        #region Action

        public PXAction<MRPFilter> GenerateMRPResultQuery;
        [PXProcessButton(CommitChanges = true), PXUIField(DisplayName = "Generate MRP Result", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        protected virtual IEnumerable generateMRPResultQuery(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                ExecuteMRPSP();
            });

            return adapter.Get();
        }

        #endregion

        public IEnumerable transaction()
        {
            PXView select = new PXView(this, false, Transaction.View.BqlSelect);
            Int32 totalrow = 1;
            Int32 startrow = PXView.StartRow;
            return select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters, ref startrow, 1, ref totalrow);
        }

        #region Event
        public virtual void _(Events.FieldDefaulting<MRPFilter.revision> e)
            => e.NewValue = SelectFrom<LUMForecastUploadPreference>.View.Select(this).TopFirst?.Revision;
        #endregion

        #region Event

        public virtual void _(Events.FieldDefaulting<MRPFilter.date> e)
            => e.NewValue = DateTime.Now;

        #endregion

        #region Method
        /// <summary> 非同步執行程序 </summary>
        public static void GoProcessing(MRPFilter filter)
        {
            var baseGraph = CreateInstance<LUMMRPProcess>();
            baseGraph.LumMRPProcess(baseGraph, filter);
        }

        /// <summary> 執行MRP </summary>
        public virtual void LumMRPProcess(LUMMRPProcess baseGraph, MRPFilter filter)
        {
            try
            {
                PXLongOperation.StartOperation(this, delegate ()
                {

                    #region Valid
                    if (filter.ItemClassID == null)
                    {
                        PXProcessing.SetError<LUMMRPProcessResult>("Item classID can not be empty!!");
                        throw new Exception();
                    }
                    if (filter.Revision == null)
                    {
                        PXProcessing.SetError<LUMMRPProcessResult>("Revision can not be empty!!");
                        throw new Exception();
                    }
                    #endregion

                    // 刪除預設資料
                    DeleteData();

                    #region Variable
                    // Initial parameter
                    var actCompanyName = _legacyCompanyService.ExtractCompany(PX.Common.PXContext.PXIdentity.IdentityName);
                    // MRPPreference
                    var mrpPreference = SelectFrom<LUMMRPPreference>.View.Select(baseGraph).RowCast<LUMMRPPreference>();
                    // 執行截止日期
                    var lastDate = GetProcessLastDate(filter.Revision);
                    #endregion                               // MRP 計算最後一天

                    #region 準備前置資料
                    // Get All Process Stock Item
                    var allInventoryItem = GetAllProcessItem(baseGraph, filter);
                    // Get All Process Warehouse
                    var allWarehouseData = GetAllProcessWarehouse(baseGraph, filter);
                    // Get All Upload Forecast Data
                    var allForecastData = GetAllForecastData(baseGraph, actCompanyName, filter.Revision);
                    // Get All INLocation Data
                    var allINlocationData = GetAllINlocationData(baseGraph);
                    // Get All INTran Data
                    var allINTranData = GetAllINTranData(baseGraph);
                    // Get All Available Inventory Data
                    var allInvInfoData = GetAllInventoryInfoData(baseGraph);
                    // Get All Safety Stock Data
                    var allSafetyStockData = GetAllSafetyStockData(baseGraph);
                    #endregion

                    // 執行每一個Inventory Item
                    foreach (var actSku in allInventoryItem)
                    {
                        // 執行每個倉庫
                        foreach (var actWarehouse in allWarehouseData.Where(x => x.SiteCD.Trim().ToUpper() != "INTR"))
                        {
                            var startDate = filter.Date;             // MRP 起始日期
                            var actDate = startDate;                 // MRP 執行日
                            var lastDayRemainForecast = 0;           // MRP 前一天剩餘 Forecast
                            var lastDayStock = 0;                    // MRP 前一天剩餘的 Stock
                            decimal safetyStock = 0;                 // 安全庫存
                            var _Sku = actSku.InventoryID;            // MRP Sku
                            var _Warehouse = actWarehouse.SiteID;     // MRP Warehouse
                            var _revision = filter.Revision;          // MRP Revision
                            LUMForecastUpload firstForecastData;     // 離計算日最新的Forecast資料

                            // Forecast upload data
                            var forecastData = allForecastData.Where(x => x.GetItem<InventoryItem>().InventoryID == _Sku &&
                                                                          x.GetItem<INSite>().SiteID == _Warehouse)
                                                              .RowCast<LUMForecastUpload>();
                            // 執行日期前的最新一筆Forecast Upload 資料
                            firstForecastData = forecastData.OrderBy(x => x.Date).LastOrDefault(x => x.Date.Value.Date < actDate.Value.Date && x.Mrptype == "Forecast");

                            #region Storage Summary(當下庫存)
                            // 庫存Graph
                            var storageGraph = PXGraph.CreateInstance<StoragePlaceEnq>();
                            storageGraph.Filter.Cache.SetValueExt<StoragePlaceEnq.StoragePlaceFilter.siteID>(storageGraph.Filter.Current, actWarehouse.SiteID);
                            storageGraph.Filter.Cache.SetValueExt<StoragePlaceEnq.StoragePlaceFilter.inventoryID>(storageGraph.Filter.Current, actSku.InventoryID);
                            var storageSummary = storageGraph.storages.Select().RowCast<StoragePlaceStatus>();
                            var inLocation = allINlocationData.Where(x => x.SiteID == actWarehouse.SiteID).Select(x => x.LocationID).ToList();
                            storageSummary = storageSummary.Where(x => inLocation.IndexOf(x.LocationID) > -1);
                            #endregion

                            #region Inventory Allocation Details
                            // 交易Graph
                            var invGraph = PXGraph.CreateInstance<InventoryAllocDetEnq>();
                            invGraph.Filter.Cache.SetValueExt<InventoryAllocDetEnqFilter.inventoryID>(invGraph.Filter.Current, actSku.InventoryID);
                            invGraph.Filter.Cache.SetValueExt<InventoryAllocDetEnqFilter.siteID>(invGraph.Filter.Current, actWarehouse.SiteID);
                            var invAllocDetails = invGraph.ResultRecords.Select().RowCast<InventoryAllocDetEnqResult>();
                            #endregion

                            #region Skip Warehouse
                            if (forecastData.Count() == 0 && storageSummary.Count() == 0 && invAllocDetails.Count() == 0)
                                continue;
                            #endregion

                            #region Act Issue (Release INTran)
                            var inTransData = firstForecastData == null ? null :
                                              allINTranData.Where(x => x.SiteID == actWarehouse.SiteID &&
                                                                       x.InventoryID == actSku.InventoryID &&
                                                                       x.ReleasedDateTime.Value.Date >= firstForecastData.Date.Value.Date &&
                                                                       x.ReleasedDateTime.Value.Date <= actDate.Value.Date);
                            #endregion

                            #region Get Inventory calculate rules
                            var invRules = new List<string>();
                            var invInfo = allInvInfoData.Where(x => x.GetItem<InventoryItem>().InventoryID == actSku.InventoryID);
                            var invSchema = invInfo.RowCast<INAvailabilityScheme>().FirstOrDefault();
                            foreach (var item in invSchema.GetType().GetProperties())
                            {
                                if (item.Name.Contains("InclQty") && item.PropertyType == typeof(bool?) && (bool?)item.GetValue(invSchema) == true)
                                    invRules.Add(item.Name);
                            }
                            #endregion

                            #region Safety Stock
                            safetyStock = allSafetyStockData.FirstOrDefault(x => x.InventoryID == actSku.InventoryID &&
                                                                                 x.SiteID == actWarehouse.SiteID)?.SafetyStock ?? 0;
                            #endregion 

                            // initial 
                            while (actDate.Value.Date <= lastDate.Date)
                            {
                                var result = baseGraph.Transaction.Insert((LUMMRPProcessResult)baseGraph.Transaction.Cache.CreateInstance());
                                result.Revision = _revision;

                                // 只計算第一天
                                if (startDate.Value.Date == actDate.Value.Date)
                                {
                                    #region Last Stock Initial

                                    lastDayStock = (int)storageSummary.Sum(x => x?.Qty ?? 0);

                                    #endregion

                                    #region Open SO(Date-1)

                                    result.PastOpenSo = (int?)
                                                        (from inv in invAllocDetails
                                                         join preference in mrpPreference
                                                         on new { A = inv.AllocationType, B = "OpenSO" } equals new { A = preference.AllocationType, B = preference.Mrptype }
                                                         where inv.PlanDate.Value.Date < actDate.Value.Date &&
                                                               invRules.IndexOf(preference.PlanType) > 0
                                                         select inv.PlanQty).Sum(x => x.Value) ?? 0;
                                    #endregion

                                    #region Safety Stock
                                    result.SafetyStock = safetyStock;
                                    #endregion
                                }

                                #region Open SO

                                result.OpenSo = (int?)
                                                (from inv in invAllocDetails
                                                 join preference in mrpPreference
                                                 on new { A = inv.AllocationType, B = "OpenSO" } equals new { A = preference.AllocationType, B = preference.Mrptype }
                                                 where inv.PlanDate.Value.Date == actDate.Value.Date &&
                                                       invRules.IndexOf(preference.PlanType) > 0
                                                 select inv.PlanQty).Sum(x => x.Value) ?? 0;

                                #endregion

                                #region Open So Adj

                                var openSOAdj = result.OpenSo + (result.PastOpenSo ?? 0);

                                #endregion

                                // 如果ActDate有上傳Forecast
                                var actDayExistsForecast = forecastData.FirstOrDefault(x => x.Date.Value.Date == actDate.Value.Date && x.Mrptype == "Forecast") != null;
                                // 計算第一天或 當天有上傳Forcast Forecast Base & Last Stock initial
                                if (startDate.Value.Date == actDate.Value.Date || actDayExistsForecast)
                                {
                                    #region 計算 Forecast & Forecast Base
                                    result.Forecast = (int?)forecastData.FirstOrDefault(x => x.Date.Value.Date == actDate.Value.Date && x.Mrptype == "Forecast")?.Qty;
                                    // 如果當天有forecast 就用當天上傳資料; 
                                    if (actDayExistsForecast)
                                        result.ForecastBase = (int?)forecastData.FirstOrDefault(x => x.Date.Value.Date == actDate.Value.Date && x.Mrptype == "Forecast")?.Qty;
                                    // 過往無任何forecast則取0; 
                                    else if (forecastData.FirstOrDefault(x => x.Date.Value.Date < actDate.Value.Date && x.Mrptype == "Forecast") == null)
                                        result.ForecastBase = 0;
                                    // 找最近的上傳Forecast資料並扣除ActIssues
                                    else
                                    {
                                        // 最新一筆的forecast 資料
                                        result.ForecastBase = (int?)firstForecastData?.Qty - (int)inTransData?.Sum(x => x.Qty ?? 0);
                                        result.ForecastBase = result.ForecastBase < 0 ? 0 : result.ForecastBase;
                                    }
                                    #endregion
                                }

                                #region Transaction Exists Flag
                                _transactionExistsDays = _transactionExistsDays.Distinct().ToList();
                                if (actDate.Value.Date == lastDate.Date ||
                                    actDate.Value.Date == startDate.Value.Date ||
                                    _transactionExistsDays.IndexOf(actDate.Value.Date) > 0)
                                    result.TransactionExistsFlag = "Y";
                                #endregion

                                #region Foreacse Initial

                                result.ForecastIntial = result.ForecastBase == null ? lastDayRemainForecast : result.ForecastBase;

                                #endregion

                                #region Forecast Remains + LastDay Forecast Remains

                                result.ForecastRemains = Math.Max(result.ForecastIntial.Value - openSOAdj.Value, 0);
                                lastDayRemainForecast = result.ForecastRemains ?? 0;

                                #endregion

                                #region Forecast Comsumption

                                result.ForecastComsumption = result?.ForecastIntial - result?.ForecastRemains;

                                #endregion

                                #region Demand Adj

                                result.DemandAdj = Math.Max(openSOAdj.Value - result.ForecastComsumption.Value, 0);

                                #endregion

                                #region Net Demand

                                result.NetDemand = result.ForecastBase == null ? (result.Forecast ?? 0) + result.DemandAdj : Math.Max(openSOAdj.Value, result.ForecastIntial.Value);

                                #endregion

                                #region Stock initial

                                result.StockInitial = lastDayStock;

                                #endregion

                                #region Demand
                                var mapDemand = (int?)
                                                (from inv in invAllocDetails
                                                 join preference in mrpPreference
                                                 on new { A = inv.AllocationType, B = "Demand" } equals new { A = preference.AllocationType, B = preference.Mrptype }
                                                 where inv.PlanDate.Value.Date == actDate.Value.Date &&
                                                       invRules.IndexOf(preference.PlanType) > 0
                                                 select inv.PlanQty).Sum(x => x.Value) ?? 0;
                                result.Demand = result.NetDemand + mapDemand;

                                #endregion

                                #region Supply

                                result.Supply = (int?)
                                                (from inv in invAllocDetails
                                                 join preference in mrpPreference
                                                 on new { A = inv.AllocationType, B = "Supply" } equals new { A = preference.AllocationType, B = preference.Mrptype }
                                                 where inv.PlanDate.Value.Date == actDate.Value.Date &&
                                                       invRules.IndexOf(preference.PlanType) > 0
                                                 select inv.PlanQty).Sum(x => x.Value) ?? 0;

                                #endregion

                                #region Stock Ava

                                result.StockAva = result.StockInitial - result.Demand + result.Supply - decimal.ToInt32(result.SafetyStock ?? 0);
                                lastDayStock = result.StockAva ?? 0;

                                #endregion

                                #region Sku + Warehouse + Date

                                result.Sku = _Sku;
                                result.Warehouse = _Warehouse;
                                result.Date = actDate;

                                #endregion
                                actDate = actDate.Value.AddDays(1);
                            }

                        }
                    }

                    // Save data
                    baseGraph.Actions.PressSave();

                    ExecuteMRPSP();
                });
            }
            catch (PXOuterException ex)
            {
                throw new PXOperationCompletedWithErrorException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new PXOperationCompletedWithErrorException(ex.Message);
            }
        }

        /// <summary> 執行SP將資料寫入MRPQueryResult </summary>
        public static void ExecuteMRPSP()
        {
            using (new PXCommandScope(1800))
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    /*Execute Stored Procedure*/
                    PXDatabase.Execute("SP_MRPResult", new PXSPParameter[0]);
                    ts.Complete();
                }
            }
        }

        /// <summary> 產生一筆固定資料 </summary>
        public virtual void InitialData()
        {
            string screenIDWODot = this.Accessinfo.ScreenID.ToString().Replace(".", "");

            PXDatabase.Insert<LUMMRPProcessResult>(
                                 new PXDataFieldAssign<LUMMRPProcessResult.sku>(GetFixInventoryItem()),
                                 new PXDataFieldAssign<LUMMRPProcessResult.warehouse>(GetFixSiteID()),
                                 new PXDataFieldAssign<LUMMRPProcessResult.date>(this.Accessinfo.BusinessDate),
                                 new PXDataFieldAssign<LUMMRPProcessResult.createdByID>(this.Accessinfo.UserID),
                                 new PXDataFieldAssign<LUMMRPProcessResult.createdByScreenID>(screenIDWODot),
                                 new PXDataFieldAssign<LUMMRPProcessResult.createdDateTime>(this.Accessinfo.BusinessDate),
                                 new PXDataFieldAssign<LUMMRPProcessResult.lastModifiedByID>(this.Accessinfo.UserID),
                                 new PXDataFieldAssign<LUMMRPProcessResult.lastModifiedByScreenID>(screenIDWODot),
                                 new PXDataFieldAssign<LUMMRPProcessResult.lastModifiedDateTime>(this.Accessinfo.BusinessDate));
        }

        /// <summary> 取固定資料的InventoryItem </summary>
        public int? GetFixInventoryItem()
        {
            return SelectFrom<InventoryItem>
                   .Where<InventoryItem.itemStatus.IsEqual<P.AsString>>
                   .View.Select(this, "AC").TopFirst.InventoryID;
        }

        /// <summary> 取固定資料的SiteID </summary>
        public int? GetFixSiteID()
        {
            return SelectFrom<INSite>
                   .Where<INSite.active.IsEqual<True>>
                   .View.Select(this).RowCast<INSite>().FirstOrDefault(x => x.SiteCD.Trim().ToUpper() != "INTR").SiteID;
        }

        /// <summary> 刪除所有資料 </summary>
        public void DeleteData()
        {
            PXDatabase.Delete<LUMMRPProcessResult>();
            this.Transaction.Cache.Clear();
        }

        /// <summary> 找出所有Tenant 中最後一筆交易/Forecast日期 </summary>
        public DateTime GetProcessLastDate(string revision)
        {
            var compArray = new string[] { "US", "TW", "JP" };
            var FinalProcessLastDate = new DateTime();
            var actUserName = _legacyCompanyService.ExtractUsername(PXContext.PXIdentity.IdentityName);
            // 找出所有Tenant中 InventoryAllocateDetail 最後一筆交易日期
            foreach (var comp in compArray)
            {
                using (new PXLoginScope($"{actUserName}@{comp}", PXAccess.GetAdministratorRoles()))
                {
                    var invGraph = PXGraph.CreateInstance<InventoryAllocDetEnq>();
                    #region CMD
                    PXSelectBase<InventoryAllocDetEnq.INItemPlan> cmd = new PXSelectJoin<InventoryAllocDetEnq.INItemPlan,
                                                InnerJoin<INPlanType,
                                                    On<InventoryAllocDetEnq.INItemPlan.FK.PlanType>,
                                                LeftJoin<InventoryAllocDetEnq.INLocation,
                                                    On<InventoryAllocDetEnq.INItemPlan.FK.Location>,
                                                LeftJoin<INLotSerialStatus,
                                                    On<InventoryAllocDetEnq.INItemPlan.FK.LotSerialStatus>,
                                                LeftJoin<InventoryAllocDetEnq.BAccount,
                                                    On<InventoryAllocDetEnq.INItemPlan.FK.BAccount>,
                                                LeftJoin<INSubItem,
                                                    On<InventoryAllocDetEnq.INItemPlan.FK.SubItem>,
                                                InnerJoin<InventoryAllocDetEnq.INSite,
                                                    On<InventoryAllocDetEnq.INItemPlan.FK.Site>,
                                                LeftJoin<InventoryAllocDetEnq.SOShipment,
                                                    On<InventoryAllocDetEnq.SOShipment.noteID, Equal<InventoryAllocDetEnq.INItemPlan.refNoteID>>,
                                                LeftJoin<InventoryAllocDetEnq.ARRegister,
                                                    On<InventoryAllocDetEnq.ARRegister.noteID, Equal<InventoryAllocDetEnq.INItemPlan.refNoteID>>,
                                                LeftJoin<InventoryAllocDetEnq.INRegister,
                                                    On<InventoryAllocDetEnq.INRegister.noteID, Equal<InventoryAllocDetEnq.INItemPlan.refNoteID>>,
                                                LeftJoin<InventoryAllocDetEnq.SOOrder,
                                                    On<InventoryAllocDetEnq.SOOrder.noteID, Equal<InventoryAllocDetEnq.INItemPlan.refNoteID>>,
                                                LeftJoin<InventoryAllocDetEnq.POOrder,
                                                    On<InventoryAllocDetEnq.POOrder.noteID, Equal<InventoryAllocDetEnq.INItemPlan.refNoteID>>,
                                                LeftJoin<InventoryAllocDetEnq.POReceipt,
                                                    On<InventoryAllocDetEnq.POReceipt.noteID, Equal<InventoryAllocDetEnq.INItemPlan.refNoteID>>,
                                                LeftJoin<InventoryAllocDetEnq.INTransitLine,
                                                    On<InventoryAllocDetEnq.INTransitLine.noteID, Equal<InventoryAllocDetEnq.INItemPlan.refNoteID>>
                                                >>>>>>>>>>>>>,
                                            Where<InventoryAllocDetEnq.INItemPlan.planQty, NotEqual<decimal0>,
                                                And<Match<InventoryAllocDetEnq.INSite, Current<AccessInfo.userName>>>>,
                                            OrderBy<Asc<INSubItem.subItemCD, // sorting must be done with PlanType preceding location
                                                    Asc<InventoryAllocDetEnq.INSite.siteCD,
                                                    Asc<InventoryAllocDetEnq.INItemPlan.origPlanType,
                                                    Asc<InventoryAllocDetEnq.INItemPlan.planType,
                                                    Asc<InventoryAllocDetEnq.INLocation.locationCD>>>>>>>(invGraph);
                    #endregion
                    PXResultset<InventoryAllocDetEnq.INItemPlan> itemPlansWithExtraInfo = cmd.Select();
                    var resultList = new List<InventoryAllocDetEnqResult>();
                    foreach (InventoryAllocDetEnq.ItemPlanWithExtraInfo ip in invGraph.UnwrapAndGroup(itemPlansWithExtraInfo))
                    {
                        Type inclQtyField = INPlanConstants.ToInclQtyField(ip.ItemPlan.PlanType);
                        if (inclQtyField != null && inclQtyField != typeof(INPlanType.inclQtyINReplaned))
                            invGraph.ProcessItemPlanRecAs(inclQtyField, resultList, ip);
                    }
                    FinalProcessLastDate = new DateTime(Math.Max(FinalProcessLastDate.Ticks, resultList.Max(x => x.PlanDate).Value.Ticks));
                    // 找出所有有交易的日期
                    _transactionExistsDays.AddRange(resultList.Select(x => x.PlanDate.Value.Date));
                }
            }
            // 找出所有Forecast Revision中最後一筆日期
            var forecastData = SelectFrom<LUMForecastUpload>
                               .InnerJoin<InventoryItem>.On<LUMForecastUpload.sku.IsEqual<InventoryItem.inventoryCD>>
                               .InnerJoin<INSite>.On<LUMForecastUpload.warehouse.IsEqual<INSite.siteCD>>
                               .Where<LUMForecastUpload.revision.IsEqual<@P.AsString>>
                               .View.Select(this, revision).RowCast<LUMForecastUpload>().OrderByDescending(x => x.Date);
            // 找出所有有Forecast的日期
            _transactionExistsDays.AddRange(forecastData.Select(x => x.Date.Value.Date));
            if (forecastData != null && forecastData.Count() > 0)
                FinalProcessLastDate = new DateTime(Math.Max(FinalProcessLastDate.Ticks, forecastData.FirstOrDefault().Date.Value.Ticks));
            return FinalProcessLastDate;
        }

        /// <summary> Get All Upload Forcast Data (CompanyName, Revision) </summary>
        public PXResultset<LUMForecastUpload> GetAllForecastData(LUMMRPProcess baseGraph, string actCompanyName, string revision)
        {
            return SelectFrom<LUMForecastUpload>
                   .InnerJoin<InventoryItem>.On<LUMForecastUpload.sku.IsEqual<InventoryItem.inventoryCD>>
                   .InnerJoin<INSite>.On<LUMForecastUpload.warehouse.IsEqual<INSite.siteCD>>
                   .Where<LUMForecastUpload.company.IsEqual<@P.AsString>
                     .And<LUMForecastUpload.revision.IsEqual<@P.AsString>>>
                   .View.Select(baseGraph, actCompanyName, revision);
        }

        /// <summary> Get All INLocation Data </summary>
        public IEnumerable<INLocation> GetAllINlocationData(LUMMRPProcess baseGraph)
        {
            return SelectFrom<INLocation>
                   .Where<INLocation.inclQtyAvail.IsEqual<True>>
                   .View.Select(baseGraph).RowCast<INLocation>();
        }

        /// <summary> Get All INTran Data </summary>
        public IEnumerable<INTran> GetAllINTranData(LUMMRPProcess baseGraph)
        {
            return SelectFrom<INTran>
                   .InnerJoin<INLocation>.On<INTran.siteID.IsEqual<INLocation.siteID>
                                        .And<INLocation.inclQtyAvail.IsEqual<True>>
                                        .And<INTran.locationID.IsEqual<INLocation.locationID>>>
                   .Where<INTran.sOShipmentType.IsEqual<P.AsString>
                     .And<INTran.released.IsEqual<True>>>
                   .View.Select(baseGraph, "I").RowCast<INTran>();
        }

        /// <summary> Get All Inventory Info Data </summary>
        public PXResultset<InventoryItem> GetAllInventoryInfoData(LUMMRPProcess baseGraph)
        {
            return SelectFrom<InventoryItem>
                   .InnerJoin<INItemClass>.On<InventoryItem.itemClassID.IsEqual<INItemClass.itemClassID>>
                   .InnerJoin<INAvailabilityScheme>.On<INItemClass.availabilitySchemeID.IsEqual<INAvailabilityScheme.availabilitySchemeID>>
                   .View.Select(baseGraph);
        }

        /// <summary> Get All Safety Stock Data </summary>
        public IEnumerable<INItemSite> GetAllSafetyStockData(LUMMRPProcess baseGraph)
        {
            return SelectFrom<INItemSite>
                   .Where<INItemSite.siteStatus.IsEqual<P.AsString>>
                   .View.Select(baseGraph, "AC").RowCast<INItemSite>();
        }

        /// <summary> Get All Process Inventory Item </summary>
        public IEnumerable<InventoryItem> GetAllProcessItem(LUMMRPProcess baseGraph, MRPFilter filter)
        {
            return SelectFrom<InventoryItem>
                   .Where<InventoryItem.itemClassID.IsEqual<P.AsInt>
                     .And<InventoryItem.itemStatus.IsEqual<P.AsString>>>
                   .View.Select(baseGraph, filter.ItemClassID, "AC").RowCast<InventoryItem>()
                   .Where(x => filter.Sku == null || x.InventoryID == filter?.Sku);
        }

        /// <summary> Get All Process Warehouse </summary>
        public IEnumerable<INSite> GetAllProcessWarehouse(LUMMRPProcess baseGraph, MRPFilter filter)
        {
            return SelectFrom<INSite>
                   .Where<INSite.active.IsEqual<True>>
                   .View.Select(baseGraph).RowCast<INSite>()
                   .Where(x => filter.Warehouse == null || x.SiteID == filter?.Warehouse);
        }

        #endregion
    }

    [Serializable]
    public class MRPFilter : IBqlTable
    {
        [PXDBDate]
        [PXDefault()]
        [PXUIField(DisplayName = "Start Date")]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }

        [PXDBInt]
        [StockItem(Required = true)]
        [PXDefault()]
        [PXUIField(DisplayName = "Stock item", Required = true)]
        public virtual int? Sku { get; set; }
        public abstract class sku : PX.Data.BQL.BqlInt.Field<sku> { }

        [PXDBInt]
        [Site(Required = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Warehouse", Required = true)]
        public virtual int? Warehouse { get; set; }
        public abstract class warehouse : PX.Data.BQL.BqlInt.Field<warehouse> { }

        [PXDBInt]
        [PXDefault("MERCHAN")]
        [PXUIField(DisplayName = "Item ClassID", Required = true)]
        [PXSelector(typeof(SearchFor<INItemClass.itemClassID>),
            DescriptionField = typeof(INItemClass.itemClassCD),
            SubstituteKey = typeof(INItemClass.itemClassCD))]
        public virtual int? ItemClassID { get; set; }
        public abstract class itemClassID : PX.Data.BQL.BqlInt.Field<itemClassID> { }

        [PXDBString]
        [PXDefault]
        [PXUIField(DisplayName = "Revision", Required = true)]
        [PXSelector(typeof(SelectFrom<LUMForecastUpload>.AggregateTo<GroupBy<LUMForecastUpload.revision>>.SearchFor<LUMForecastUpload.revision>))]
        public virtual string Revision { get; set; }
        public abstract class revision : PX.Data.BQL.BqlString.Field<revision> { }

    }

}
