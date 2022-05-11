using System;
using System.Collections;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data.Update;
using PX.Objects.AR;
using PX.Objects.CR;
using PX.SM;

namespace LUMLocalization.DAC
{
    [Serializable]
    [PXCacheName("v_LumICFromPO")]
    public class v_LumICFromPO : IBqlTable
    {
        #region Selected
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        protected bool? _Selected = false;
        [PXBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Switch<Case<Where<excluded, Equal<True>>, True>, Current<selected>>))]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
            }
        }
        #endregion

        #region POOrderNbr
        [PXDBString(15, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "POOrder Nbr")]
        public virtual string POOrderNbr { get; set; }
        public abstract class pOOrderNbr : PX.Data.BQL.BqlString.Field<pOOrderNbr> { }
        #endregion

        #region POOrderType
        [PXDBString(2, IsFixed = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "POOrder Type")]
        public virtual string POOrderType { get; set; }
        public abstract class pOOrderType : PX.Data.BQL.BqlString.Field<pOOrderType> { }
        #endregion

        #region Pobranchid
        [PXDBInt()]
        [PXUIField(DisplayName = "PO Branch ID")]
        public virtual int? Pobranchid { get; set; }
        public abstract class pobranchid : PX.Data.BQL.BqlInt.Field<pobranchid> { }
        #endregion

        #region Pobranchcd
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "PO Branch")]
        public virtual string Pobranchcd { get; set; }
        public abstract class pobranchcd : PX.Data.BQL.BqlString.Field<pobranchcd> { }
        #endregion

        #region POAcctName
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "PO Acct Name")]
        public virtual string POAcctName { get; set; }
        public abstract class pOAcctName : PX.Data.BQL.BqlString.Field<pOAcctName> { }
        #endregion

        #region SOCompanyID
        [PXDBInt()]
        [PXUIField(DisplayName = "SO Company ID")]
        public virtual int? SOCompanyID { get; set; }
        public abstract class sOCompanyID : PX.Data.BQL.BqlInt.Field<sOCompanyID> { }
        #endregion

        #region SOOrderNbr
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "SOOrder Nbr")]
        public virtual string SOOrderNbr { get; set; }
        public abstract class sOOrderNbr : PX.Data.BQL.BqlString.Field<sOOrderNbr> { }
        #endregion

        #region SOOrderType
        [PXDBString(2, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "SOOrder Type")]
        public virtual string SOOrderType { get; set; }
        public abstract class sOOrderType : PX.Data.BQL.BqlString.Field<sOOrderType> { }
        #endregion

        #region Sobranchid
        [PXDBInt()]
        [PXUIField(DisplayName = "SO Branch ID")]
        public virtual int? Sobranchid { get; set; }
        public abstract class sobranchid : PX.Data.BQL.BqlInt.Field<sobranchid> { }
        #endregion

        #region Sobranchcd
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "SO Branch")]
        public virtual string Sobranchcd { get; set; }
        public abstract class sobranchcd : PX.Data.BQL.BqlString.Field<sobranchcd> { }
        #endregion

        #region SOAcctName
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "SO Acct Name")]
        public virtual string SOAcctName { get; set; }
        public abstract class sOAcctName : PX.Data.BQL.BqlString.Field<sOAcctName> { }
        #endregion

        #region ShippingRefNoteID
        [PXDBGuid()]
        [PXUIField(DisplayName = "Shipping Ref Note ID")]
        public virtual Guid? ShippingRefNoteID { get; set; }
        public abstract class shippingRefNoteID : PX.Data.BQL.BqlGuid.Field<shippingRefNoteID> { }
        #endregion

        #region SOShipmentShipmentNbr
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Shipment Nbr")]
        public virtual string SOShipmentShipmentNbr { get; set; }
        public abstract class sOShipmentShipmentNbr : PX.Data.BQL.BqlString.Field<sOShipmentShipmentNbr> { }
        #endregion

        #region SOShipmentNoteID
        [PXDBGuid()]
        [PXUIField(DisplayName = "SOShipment Note ID")]
        public virtual Guid? SOShipmentNoteID { get; set; }
        public abstract class sOShipmentNoteID : PX.Data.BQL.BqlGuid.Field<sOShipmentNoteID> { }
        #endregion

        #region SOShipmentStatus
        [PXDBString(1, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Shipment Status")]
        public virtual string SOShipmentStatus { get; set; }
        public abstract class sOShipmentStatus : PX.Data.BQL.BqlString.Field<sOShipmentStatus> { }
        #endregion

        #region SOShipmentShipDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Ship Date")]
        public virtual DateTime? SOShipmentShipDate { get; set; }
        public abstract class sOShipmentShipDate : PX.Data.BQL.BqlDateTime.Field<sOShipmentShipDate> { }
        #endregion

        #region SOShipmentShipmentQty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Shipment Qty")]
        public virtual Decimal? SOShipmentShipmentQty { get; set; }
        public abstract class sOShipmentShipmentQty : PX.Data.BQL.BqlDecimal.Field<sOShipmentShipmentQty> { }
        #endregion

        #region SOShipmentSiteID
        [PXDBInt()]
        [PXUIField(DisplayName = "Shipment Site ID")]
        public virtual int? SOShipmentSiteID { get; set; }
        public abstract class sOShipmentSiteID : PX.Data.BQL.BqlInt.Field<sOShipmentSiteID> { }
        #endregion

        #region SOShipmentShipmentDesc
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Shipment Desc")]
        public virtual string SOShipmentShipmentDesc { get; set; }
        public abstract class sOShipmentShipmentDesc : PX.Data.BQL.BqlString.Field<sOShipmentShipmentDesc> { }
        #endregion

        #region SOShipmentWorkgroupID
        [PXDBInt()]
        [PXUIField(DisplayName = "Shipment Workgroup ID")]
        public virtual int? SOShipmentWorkgroupID { get; set; }
        public abstract class sOShipmentWorkgroupID : PX.Data.BQL.BqlInt.Field<sOShipmentWorkgroupID> { }
        #endregion

        #region ShipmentWeight
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Shipment Weight")]
        public virtual Decimal? ShipmentWeight { get; set; }
        public abstract class shipmentWeight : PX.Data.BQL.BqlDecimal.Field<shipmentWeight> { }
        #endregion

        #region ShipmentVolume
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Shipment Volume")]
        public virtual Decimal? ShipmentVolume { get; set; }
        public abstract class shipmentVolume : PX.Data.BQL.BqlDecimal.Field<shipmentVolume> { }
        #endregion

        #region SOShipmentPackageCount
        [PXDBInt()]
        [PXUIField(DisplayName = "Shipment Package Count")]
        public virtual int? SOShipmentPackageCount { get; set; }
        public abstract class sOShipmentPackageCount : PX.Data.BQL.BqlInt.Field<sOShipmentPackageCount> { }
        #endregion

        #region SOShipmentPackageWeight
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Shipment Package Weight")]
        public virtual Decimal? SOShipmentPackageWeight { get; set; }
        public abstract class sOShipmentPackageWeight : PX.Data.BQL.BqlDecimal.Field<sOShipmentPackageWeight> { }
        #endregion

        #region ReceiptNbr
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Receipt Nbr")]
        public virtual string ReceiptNbr { get; set; }
        public abstract class receiptNbr : PX.Data.BQL.BqlString.Field<receiptNbr> { }
        #endregion

        #region ReceiptDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Receipt Date")]
        public virtual DateTime? ReceiptDate { get; set; }
        public abstract class receiptDate : PX.Data.BQL.BqlDateTime.Field<receiptDate> { }
        #endregion

        #region Excluded
        public abstract class excluded : PX.Data.BQL.BqlBool.Field<excluded> { }
        [PXBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Excluded")]
        public virtual bool? Excluded { get; set; }
        #endregion
    }
}