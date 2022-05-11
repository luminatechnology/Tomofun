using PX.Common;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.SO;
using PX.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;

namespace PX.Objects.SO
{
    public class SOPackageDetailExt : PXCacheExtension<SOPackageDetail>
    {
        #region Qty
        [PXDBQuantity]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Qty", Enabled = true)]
        public virtual decimal? Qty { get; set; }
        public abstract class qty : BqlDecimal.Field<qty> { }
        #endregion

        #region CustomRefNbr1
        [PXDBString(30, IsUnicode = true)]
        [PXUIField(DisplayName = "Pallet No.")]
        public virtual string CustomRefNbr1 { get; set; }
        public abstract class customRefNbr1 : BqlString.Field<customRefNbr1> { }
        #endregion

        #region CustomRefNbr2
        [PXDBString(30, IsUnicode = true)]
        [PXUIField(DisplayName = "Carton No.")]
        public virtual string CustomRefNbr2 { get; set; }
        public abstract class customRefNbr2 : BqlString.Field<customRefNbr2> { }
        #endregion

        #region UsrShipmentSplitLineNbr
        [PXDBInt]
        [PXUIField(DisplayName = "Shipment Split Line Nbr")]
        [PXSelector(typeof(Search<SOShipLine.lineNbr,
                           Where<SOShipLine.shipmentNbr, Equal<Current<SOPackageDetail.shipmentNbr>>,
                           And<SOShipLine.packedQty, NotEqual<SOShipLine.shippedQty>>>>),
                    typeof(SOShipLine.origOrderType),
                    typeof(SOShipLine.origOrderNbr),
                    typeof(SOShipLine.inventoryID),
                    typeof(SOShipLine.shippedQty),
                    typeof(SOShipLine.packedQty),
                    typeof(SOShipLine.uOM))]
        public virtual int? UsrShipmentSplitLineNbr { get; set; }
        public abstract class usrShipmentSplitLineNbr : BqlInt.Field<usrShipmentSplitLineNbr> { }
        #endregion

        #region UsrTotalCartons
        [PXDBInt]
        [PXUIField(DisplayName = "Total Cartons")]
        public virtual int? UsrTotalCartons { get; set; }
        public abstract class usrTotalCartons : BqlInt.Field<usrTotalCartons> { }
        #endregion
    }
}