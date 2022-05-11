using System;
using PX.Data;

namespace LUMLocalization.DAC
{
    [Serializable]
    [PXCacheName("GlobalSiteStatus")]
    public class GlobalSiteStatus : IBqlTable
    {
        #region TenantName
        [PXDBString(128, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Tenant Name")]
        public virtual string TenantName { get; set; }
        public abstract class tenantName : PX.Data.BQL.BqlString.Field<tenantName> { }
        #endregion

        #region Warehouse ID
        [PXDBString(30, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Warehouse  ID")]
        public virtual string WarehouseID { get; set; }
        public abstract class warehouseID : PX.Data.BQL.BqlString.Field<warehouseID> { }
        #endregion

        #region Warehouse Description
        [PXDBString(60, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Warehouse  Description")]
        public virtual string WarehouseDescription { get; set; }
        public abstract class warehouseDescription : PX.Data.BQL.BqlString.Field<warehouseDescription> { }
        #endregion

        #region Inventory ID
        [PXDBString(30, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Inventory ID")]
        public virtual string InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlString.Field<inventoryID> { }
        #endregion

        #region Inventory Description
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Inventory  Description")]
        public virtual string InventoryDescription { get; set; }
        public abstract class inventoryDescription : PX.Data.BQL.BqlString.Field<inventoryDescription> { }
        #endregion

        #region Qty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty")]
        public virtual Decimal? Qty { get; set; }
        public abstract class qty : PX.Data.BQL.BqlDecimal.Field<qty> { }
        #endregion

        #region QtyOnHand
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty On Hand")]
        public virtual Decimal? QtyOnHand { get; set; }
        public abstract class qtyOnHand : PX.Data.BQL.BqlDecimal.Field<qtyOnHand> { }
        #endregion

        #region QtyNotAvail
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Not Avail")]
        public virtual Decimal? QtyNotAvail { get; set; }
        public abstract class qtyNotAvail : PX.Data.BQL.BqlDecimal.Field<qtyNotAvail> { }
        #endregion

        #region QtyAvail
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Avail")]
        public virtual Decimal? QtyAvail { get; set; }
        public abstract class qtyAvail : PX.Data.BQL.BqlDecimal.Field<qtyAvail> { }
        #endregion

        #region QtyHardAvail
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Hard Avail")]
        public virtual Decimal? QtyHardAvail { get; set; }
        public abstract class qtyHardAvail : PX.Data.BQL.BqlDecimal.Field<qtyHardAvail> { }
        #endregion

        #region QtyActual
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Actual")]
        public virtual Decimal? QtyActual { get; set; }
        public abstract class qtyActual : PX.Data.BQL.BqlDecimal.Field<qtyActual> { }
        #endregion

        #region QtyInTransit
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty In Transit")]
        public virtual Decimal? QtyInTransit { get; set; }
        public abstract class qtyInTransit : PX.Data.BQL.BqlDecimal.Field<qtyInTransit> { }
        #endregion

        #region QtyInTransitToSO
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty In Transit To SO")]
        public virtual Decimal? QtyInTransitToSO { get; set; }
        public abstract class qtyInTransitToSO : PX.Data.BQL.BqlDecimal.Field<qtyInTransitToSO> { }
        #endregion

        #region QtyINReplaned
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty INReplaned")]
        public virtual Decimal? QtyINReplaned { get; set; }
        public abstract class qtyINReplaned : PX.Data.BQL.BqlDecimal.Field<qtyINReplaned> { }
        #endregion

        #region QtyPOPrepared
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty POPrepared")]
        public virtual Decimal? QtyPOPrepared { get; set; }
        public abstract class qtyPOPrepared : PX.Data.BQL.BqlDecimal.Field<qtyPOPrepared> { }
        #endregion

        #region QtyPOOrders
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty POOrders")]
        public virtual Decimal? QtyPOOrders { get; set; }
        public abstract class qtyPOOrders : PX.Data.BQL.BqlDecimal.Field<qtyPOOrders> { }
        #endregion

        #region QtyPOReceipts
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty POReceipts")]
        public virtual Decimal? QtyPOReceipts { get; set; }
        public abstract class qtyPOReceipts : PX.Data.BQL.BqlDecimal.Field<qtyPOReceipts> { }
        #endregion

        #region QtyFSSrvOrdBooked
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty FSSrv Ord Booked")]
        public virtual Decimal? QtyFSSrvOrdBooked { get; set; }
        public abstract class qtyFSSrvOrdBooked : PX.Data.BQL.BqlDecimal.Field<qtyFSSrvOrdBooked> { }
        #endregion

        #region QtyFSSrvOrdAllocated
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty FSSrv Ord Allocated")]
        public virtual Decimal? QtyFSSrvOrdAllocated { get; set; }
        public abstract class qtyFSSrvOrdAllocated : PX.Data.BQL.BqlDecimal.Field<qtyFSSrvOrdAllocated> { }
        #endregion

        #region QtyFSSrvOrdPrepared
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty FSSrv Ord Prepared")]
        public virtual Decimal? QtyFSSrvOrdPrepared { get; set; }
        public abstract class qtyFSSrvOrdPrepared : PX.Data.BQL.BqlDecimal.Field<qtyFSSrvOrdPrepared> { }
        #endregion

        #region QtySOBackOrdered
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty SOBack Ordered")]
        public virtual Decimal? QtySOBackOrdered { get; set; }
        public abstract class qtySOBackOrdered : PX.Data.BQL.BqlDecimal.Field<qtySOBackOrdered> { }
        #endregion

        #region QtySOPrepared
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty SOPrepared")]
        public virtual Decimal? QtySOPrepared { get; set; }
        public abstract class qtySOPrepared : PX.Data.BQL.BqlDecimal.Field<qtySOPrepared> { }
        #endregion

        #region QtySOBooked
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty SOBooked")]
        public virtual Decimal? QtySOBooked { get; set; }
        public abstract class qtySOBooked : PX.Data.BQL.BqlDecimal.Field<qtySOBooked> { }
        #endregion

        #region QtySOShipped
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty SOShipped")]
        public virtual Decimal? QtySOShipped { get; set; }
        public abstract class qtySOShipped : PX.Data.BQL.BqlDecimal.Field<qtySOShipped> { }
        #endregion

        #region QtySOShipping
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty SOShipping")]
        public virtual Decimal? QtySOShipping { get; set; }
        public abstract class qtySOShipping : PX.Data.BQL.BqlDecimal.Field<qtySOShipping> { }
        #endregion

        #region QtyFixedFSSrvOrd
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Fixed FSSrv Ord")]
        public virtual Decimal? QtyFixedFSSrvOrd { get; set; }
        public abstract class qtyFixedFSSrvOrd : PX.Data.BQL.BqlDecimal.Field<qtyFixedFSSrvOrd> { }
        #endregion

        #region QtyPOFixedFSSrvOrd
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty POFixed FSSrv Ord")]
        public virtual Decimal? QtyPOFixedFSSrvOrd { get; set; }
        public abstract class qtyPOFixedFSSrvOrd : PX.Data.BQL.BqlDecimal.Field<qtyPOFixedFSSrvOrd> { }
        #endregion

        #region QtyPOFixedFSSrvOrdPrepared
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty POFixed FSSrv Ord Prepared")]
        public virtual Decimal? QtyPOFixedFSSrvOrdPrepared { get; set; }
        public abstract class qtyPOFixedFSSrvOrdPrepared : PX.Data.BQL.BqlDecimal.Field<qtyPOFixedFSSrvOrdPrepared> { }
        #endregion

        #region QtyPOFixedFSSrvOrdReceipts
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty POFixed FSSrv Ord Receipts")]
        public virtual Decimal? QtyPOFixedFSSrvOrdReceipts { get; set; }
        public abstract class qtyPOFixedFSSrvOrdReceipts : PX.Data.BQL.BqlDecimal.Field<qtyPOFixedFSSrvOrdReceipts> { }
        #endregion

        #region QtySOFixed
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty SOFixed")]
        public virtual Decimal? QtySOFixed { get; set; }
        public abstract class qtySOFixed : PX.Data.BQL.BqlDecimal.Field<qtySOFixed> { }
        #endregion

        #region QtyPOFixedOrders
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty POFixed Orders")]
        public virtual Decimal? QtyPOFixedOrders { get; set; }
        public abstract class qtyPOFixedOrders : PX.Data.BQL.BqlDecimal.Field<qtyPOFixedOrders> { }
        #endregion

        #region QtyPOFixedPrepared
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty POFixed Prepared")]
        public virtual Decimal? QtyPOFixedPrepared { get; set; }
        public abstract class qtyPOFixedPrepared : PX.Data.BQL.BqlDecimal.Field<qtyPOFixedPrepared> { }
        #endregion

        #region QtyPOFixedReceipts
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty POFixed Receipts")]
        public virtual Decimal? QtyPOFixedReceipts { get; set; }
        public abstract class qtyPOFixedReceipts : PX.Data.BQL.BqlDecimal.Field<qtyPOFixedReceipts> { }
        #endregion

        #region QtySODropShip
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty SODrop Ship")]
        public virtual Decimal? QtySODropShip { get; set; }
        public abstract class qtySODropShip : PX.Data.BQL.BqlDecimal.Field<qtySODropShip> { }
        #endregion

        #region QtyPODropShipOrders
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty PODrop Ship Orders")]
        public virtual Decimal? QtyPODropShipOrders { get; set; }
        public abstract class qtyPODropShipOrders : PX.Data.BQL.BqlDecimal.Field<qtyPODropShipOrders> { }
        #endregion

        #region QtyPODropShipPrepared
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty PODrop Ship Prepared")]
        public virtual Decimal? QtyPODropShipPrepared { get; set; }
        public abstract class qtyPODropShipPrepared : PX.Data.BQL.BqlDecimal.Field<qtyPODropShipPrepared> { }
        #endregion

        #region QtyPODropShipReceipts
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty PODrop Ship Receipts")]
        public virtual Decimal? QtyPODropShipReceipts { get; set; }
        public abstract class qtyPODropShipReceipts : PX.Data.BQL.BqlDecimal.Field<qtyPODropShipReceipts> { }
        #endregion

        #region QtyINIssues
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty INIssues")]
        public virtual Decimal? QtyINIssues { get; set; }
        public abstract class qtyINIssues : PX.Data.BQL.BqlDecimal.Field<qtyINIssues> { }
        #endregion

        #region QtyINReceipts
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty INReceipts")]
        public virtual Decimal? QtyINReceipts { get; set; }
        public abstract class qtyINReceipts : PX.Data.BQL.BqlDecimal.Field<qtyINReceipts> { }
        #endregion

        #region QtyINAssemblySupply
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty INAssembly Supply")]
        public virtual Decimal? QtyINAssemblySupply { get; set; }
        public abstract class qtyINAssemblySupply : PX.Data.BQL.BqlDecimal.Field<qtyINAssemblySupply> { }
        #endregion

        #region QtyINAssemblyDemand
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty INAssembly Demand")]
        public virtual Decimal? QtyINAssemblyDemand { get; set; }
        public abstract class qtyINAssemblyDemand : PX.Data.BQL.BqlDecimal.Field<qtyINAssemblyDemand> { }
        #endregion

        #region QtyInTransitToProduction
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty In Transit To Production")]
        public virtual Decimal? QtyInTransitToProduction { get; set; }
        public abstract class qtyInTransitToProduction : PX.Data.BQL.BqlDecimal.Field<qtyInTransitToProduction> { }
        #endregion

        #region QtyProductionSupplyPrepared
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Production Supply Prepared")]
        public virtual Decimal? QtyProductionSupplyPrepared { get; set; }
        public abstract class qtyProductionSupplyPrepared : PX.Data.BQL.BqlDecimal.Field<qtyProductionSupplyPrepared> { }
        #endregion

        #region QtyProductionSupply
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Production Supply")]
        public virtual Decimal? QtyProductionSupply { get; set; }
        public abstract class qtyProductionSupply : PX.Data.BQL.BqlDecimal.Field<qtyProductionSupply> { }
        #endregion

        #region QtyPOFixedProductionPrepared
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty POFixed Production Prepared")]
        public virtual Decimal? QtyPOFixedProductionPrepared { get; set; }
        public abstract class qtyPOFixedProductionPrepared : PX.Data.BQL.BqlDecimal.Field<qtyPOFixedProductionPrepared> { }
        #endregion

        #region QtyPOFixedProductionOrders
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty POFixed Production Orders")]
        public virtual Decimal? QtyPOFixedProductionOrders { get; set; }
        public abstract class qtyPOFixedProductionOrders : PX.Data.BQL.BqlDecimal.Field<qtyPOFixedProductionOrders> { }
        #endregion

        #region QtyProductionDemandPrepared
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Production Demand Prepared")]
        public virtual Decimal? QtyProductionDemandPrepared { get; set; }
        public abstract class qtyProductionDemandPrepared : PX.Data.BQL.BqlDecimal.Field<qtyProductionDemandPrepared> { }
        #endregion

        #region QtyProductionDemand
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Production Demand")]
        public virtual Decimal? QtyProductionDemand { get; set; }
        public abstract class qtyProductionDemand : PX.Data.BQL.BqlDecimal.Field<qtyProductionDemand> { }
        #endregion

        #region QtyProductionAllocated
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Production Allocated")]
        public virtual Decimal? QtyProductionAllocated { get; set; }
        public abstract class qtyProductionAllocated : PX.Data.BQL.BqlDecimal.Field<qtyProductionAllocated> { }
        #endregion

        #region QtySOFixedProduction
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty SOFixed Production")]
        public virtual Decimal? QtySOFixedProduction { get; set; }
        public abstract class qtySOFixedProduction : PX.Data.BQL.BqlDecimal.Field<qtySOFixedProduction> { }
        #endregion

        #region QtyProdFixedPurchase
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Prod Fixed Purchase")]
        public virtual Decimal? QtyProdFixedPurchase { get; set; }
        public abstract class qtyProdFixedPurchase : PX.Data.BQL.BqlDecimal.Field<qtyProdFixedPurchase> { }
        #endregion

        #region QtyProdFixedProduction
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Prod Fixed Production")]
        public virtual Decimal? QtyProdFixedProduction { get; set; }
        public abstract class qtyProdFixedProduction : PX.Data.BQL.BqlDecimal.Field<qtyProdFixedProduction> { }
        #endregion

        #region QtyProdFixedProdOrdersPrepared
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Prod Fixed Prod Orders Prepared")]
        public virtual Decimal? QtyProdFixedProdOrdersPrepared { get; set; }
        public abstract class qtyProdFixedProdOrdersPrepared : PX.Data.BQL.BqlDecimal.Field<qtyProdFixedProdOrdersPrepared> { }
        #endregion

        #region QtyProdFixedProdOrders
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Prod Fixed Prod Orders")]
        public virtual Decimal? QtyProdFixedProdOrders { get; set; }
        public abstract class qtyProdFixedProdOrders : PX.Data.BQL.BqlDecimal.Field<qtyProdFixedProdOrders> { }
        #endregion

        #region QtyProdFixedSalesOrdersPrepared
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Prod Fixed Sales Orders Prepared")]
        public virtual Decimal? QtyProdFixedSalesOrdersPrepared { get; set; }
        public abstract class qtyProdFixedSalesOrdersPrepared : PX.Data.BQL.BqlDecimal.Field<qtyProdFixedSalesOrdersPrepared> { }
        #endregion

        #region QtyProdFixedSalesOrders
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Prod Fixed Sales Orders")]
        public virtual Decimal? QtyProdFixedSalesOrders { get; set; }
        public abstract class qtyProdFixedSalesOrders : PX.Data.BQL.BqlDecimal.Field<qtyProdFixedSalesOrders> { }
        #endregion
    }
}