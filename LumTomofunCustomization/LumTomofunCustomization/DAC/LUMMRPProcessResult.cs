using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;

namespace LumTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMMRPProcessResult")]
    public class LUMMRPProcessResult : IBqlTable
    {

        #region Selected
        /// <summary>
        /// Indicates whether the record is selected for processing.
        /// </summary>
        [PXBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region Sku
        [PXDBInt(IsKey = true)]
        [PXSelector(
            typeof(SearchFor<InventoryItem.inventoryID>),
            DescriptionField = typeof(InventoryItem.inventoryCD),
            SubstituteKey = typeof(InventoryItem.inventoryCD))]
        [PXUIField(DisplayName = "Sku")]
        public virtual int? Sku { get; set; }
        public abstract class sku : PX.Data.BQL.BqlInt.Field<sku> { }
        #endregion

        #region Warehouse
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Warehouse")]
        [PXSelector(
            typeof(SearchFor<INSite.siteID>),
            SubstituteKey = typeof(INSite.siteCD))]
        public virtual int? Warehouse { get; set; }
        public abstract class warehouse : PX.Data.BQL.BqlInt.Field<warehouse> { }
        #endregion

        #region Date
        [PXDBDate(IsKey = true)]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
        #endregion

        #region Forecase
        [PXDBInt()]
        [PXUIField(DisplayName = "Forecase")]
        public virtual int? Forecase { get; set; }
        public abstract class forecase : PX.Data.BQL.BqlInt.Field<forecase> { }
        #endregion

        #region ForecaseBase
        [PXDBInt()]
        [PXUIField(DisplayName = "Forecase Base")]
        public virtual int? ForecaseBase { get; set; }
        public abstract class forecaseBase : PX.Data.BQL.BqlInt.Field<forecaseBase> { }
        #endregion

        #region OpenSo
        [PXDBInt()]
        [PXUIField(DisplayName = "Open So")]
        public virtual int? OpenSo { get; set; }
        public abstract class openSo : PX.Data.BQL.BqlInt.Field<openSo> { }
        #endregion

        #region PastOpenSo
        [PXDBInt()]
        [PXUIField(DisplayName = "Open So(Date-1)")]
        public virtual int? PastOpenSo { get; set; }
        public abstract class pastOpenSo : PX.Data.BQL.BqlInt.Field<pastOpenSo> { }
        #endregion

        #region ForecastIntial
        [PXDBInt()]
        [PXUIField(DisplayName = "Forecast Intial")]
        public virtual int? ForecastIntial { get; set; }
        public abstract class forecastIntial : PX.Data.BQL.BqlInt.Field<forecastIntial> { }
        #endregion

        #region ForecastComsumption
        [PXDBInt()]
        [PXUIField(DisplayName = "Forecast Comsumption")]
        public virtual int? ForecastComsumption { get; set; }
        public abstract class forecastComsumption : PX.Data.BQL.BqlInt.Field<forecastComsumption> { }
        #endregion

        #region ForecaseRemains
        [PXDBInt()]
        [PXUIField(DisplayName = "Forecase Remains")]
        public virtual int? ForecaseRemains { get; set; }
        public abstract class forecaseRemains : PX.Data.BQL.BqlInt.Field<forecaseRemains> { }
        #endregion

        #region DemandAdj
        [PXDBInt()]
        [PXUIField(DisplayName = "Demand Adj")]
        public virtual int? DemandAdj { get; set; }
        public abstract class demandAdj : PX.Data.BQL.BqlInt.Field<demandAdj> { }
        #endregion

        #region NetDemand
        [PXDBInt()]
        [PXUIField(DisplayName = "Net Demand")]
        public virtual int? NetDemand { get; set; }
        public abstract class netDemand : PX.Data.BQL.BqlInt.Field<netDemand> { }
        #endregion

        #region Demand
        [PXDBInt()]
        [PXUIField(DisplayName = "Demand")]
        public virtual int? Demand { get; set; }
        public abstract class demand : PX.Data.BQL.BqlInt.Field<demand> { }
        #endregion

        #region StockInitial
        [PXDBInt()]
        [PXUIField(DisplayName = "Stock Initial")]
        public virtual int? StockInitial { get; set; }
        public abstract class stockInitial : PX.Data.BQL.BqlInt.Field<stockInitial> { }
        #endregion

        #region Supply
        [PXDBInt()]
        [PXUIField(DisplayName = "Supply")]
        public virtual int? Supply { get; set; }
        public abstract class supply : PX.Data.BQL.BqlInt.Field<supply> { }
        #endregion

        #region StockAva
        [PXDBInt()]
        [PXUIField(DisplayName = "Stock Ava")]
        public virtual int? StockAva { get; set; }
        public abstract class stockAva : PX.Data.BQL.BqlInt.Field<stockAva> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }
}