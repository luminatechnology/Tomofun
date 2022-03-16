using System;
using PX.Data;
using PX.Objects.IN;

namespace LumTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMMRPPreference")]
    public class LUMMRPPreference : IBqlTable
    {
        #region AllocationType
        [QtyAllocType.List]
        [PXDBString(50, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Allocation Type")]
        public virtual string AllocationType { get; set; }
        public abstract class allocationType : PX.Data.BQL.BqlString.Field<allocationType> { }
        #endregion

        #region PlanType
        [PXDBString(100, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Plan Type")]
        [PXStringList(
            new string[]{
"InclQtyFSSrvOrdAllocated",
"InclQtyFSSrvOrdBooked",
"InclQtyFSSrvOrdPrepared",
"InclQtyINAssemblyDemand",
"InclQtyINAssemblySupply",
"InclQtyINIssues",
"InclQtyINReceipts",
"InclQtyInTransit",
"InclQtyPOOrders",
"InclQtyPOPrepared",
"InclQtyPOReceipts",
"InclQtyProductionAllocated",
"InclQtyProductionDemand",
"InclQtyProductionDemandPrepared",
"InclQtyProductionSupply",
"InclQtyProductionSupplyPrepared",
"InclQtySOBackOrdered",
"InclQtySOBooked ",
"InclQtySOPrepared",
"InclQtySOReverse",
"InclQtySOShipped",
"InclQtySOShipping" },
            new string[] { "Deduct Qty. Allocated for Service Orders",
"Deduct Qty. on Service Orders",
"Deduct Qty. on Service Orders Prepared",
"Deduct Qty. of Kit Assembly Demand",
"Include Qty. of Kit Assembly Supply",
"Deduct Qty. on Issues",
"Include Qty. on Receipts",
"Include Qty. in Transit",
"Include Qty. on Purchase Orders",
"Include Qty. on Purchase Prepared",
"Include Qty. on PO Receipts",
"Deduct Qty. on Production Allocated",
"Deduct Qty. on Production Demand",
"Deduct Qty. on Production Demand Prepared",
"Include Qty. of Production Supply",
"Include Qty. of Production Supply Prepared",
"Deduct Qty. on Back Orders",
"Deduct Qty. on Sales Orders",
"Deduct Qty. on Sales Prepared",
"Include Qty. on Sales Returns",
"Deduct Qty. Shipped",
"Deduct Qty. Allocated"})]
        public virtual string PlanType { get; set; }
        public abstract class planType : PX.Data.BQL.BqlString.Field<planType> { }
        #endregion

        #region Mrptype
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXStringList(new string[] { "Demand", "OpenSO", "Supply" }, new string[] { "Demand", "OpenSO", "Supply" })]
        [PXUIField(DisplayName = "Mrptype")]
        public virtual string Mrptype { get; set; }
        public abstract class mrptype : PX.Data.BQL.BqlString.Field<mrptype> { }
        #endregion

        #region GroupingType
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Grouping Type")]
        public virtual string GroupingType { get; set; }
        public abstract class groupingType : PX.Data.BQL.BqlString.Field<groupingType> { }
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