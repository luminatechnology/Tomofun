using System;
using PX.Data;

namespace LUMTomofunCustomization
{
    [Serializable]
    [PXCacheName("v_OpenTRSourceData")]
    public class v_OpenTRSourceData : IBqlTable
    {
        #region Company
        [PXDBString(128, IsUnicode = true, InputMask = "",IsKey = true)]
        [PXUIField(DisplayName = "Company")]
        public virtual string Company { get; set; }
        public abstract class company : PX.Data.BQL.BqlString.Field<company> { }
        #endregion

        #region Sku
        [PXDBString(30, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Sku")]
        public virtual string Sku { get; set; }
        public abstract class sku : PX.Data.BQL.BqlString.Field<sku> { }
        #endregion

        #region Warehouse
        [PXDBString(30, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Warehouse")]
        public virtual string Warehouse { get; set; }
        public abstract class warehouse : PX.Data.BQL.BqlString.Field<warehouse> { }
        #endregion

        #region Eta
        [PXDBDate(IsKey = true)]
        [PXUIField(DisplayName = "Eta")]
        public virtual DateTime? Eta { get; set; }
        public abstract class eta : PX.Data.BQL.BqlDateTime.Field<eta> { }
        #endregion

        #region OpenQty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Open Qty")]
        public virtual Decimal? OpenQty { get; set; }
        public abstract class openQty : PX.Data.BQL.BqlDecimal.Field<openQty> { }
        #endregion
    }
}