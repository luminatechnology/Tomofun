using System;
using PX.Data;

namespace LUMTomofunCustomization
{
    [Serializable]
    [PXCacheName("v_MRPStockInitial")]
    public class v_MRPStockInitial : IBqlTable
    {
        #region Mrptype
        [PXDBString(13, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Mrptype")]
        public virtual string Mrptype { get; set; }
        public abstract class mrptype : PX.Data.BQL.BqlString.Field<mrptype> { }
        #endregion

        #region Sku
        [PXDBString(30, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Sku")]
        public virtual string Sku { get; set; }
        public abstract class sku : PX.Data.BQL.BqlString.Field<sku> { }
        #endregion

        #region Company
        [PXDBString(128, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Company")]
        public virtual string Company { get; set; }
        public abstract class company : PX.Data.BQL.BqlString.Field<company> { }
        #endregion

        #region Country
        [PXDBString(2, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Country")]
        public virtual string Country { get; set; }
        public abstract class country : PX.Data.BQL.BqlString.Field<country> { }
        #endregion

        #region Warehouse
        [PXDBString(30, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Warehouse")]
        public virtual string Warehouse { get; set; }
        public abstract class warehouse : PX.Data.BQL.BqlString.Field<warehouse> { }
        #endregion

        #region Date
        [PXDBDate(IsKey = true)]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
        #endregion

        #region Qty
        [PXDBInt()]
        [PXUIField(DisplayName = "Qty")]
        public virtual int? Qty { get; set; }
        public abstract class qty : PX.Data.BQL.BqlInt.Field<qty> { }
        #endregion

        #region QtyWeek
        [PXDBInt()]
        [PXUIField(DisplayName = "Qty Week")]
        public virtual int? QtyWeek { get; set; }
        public abstract class qtyWeek : PX.Data.BQL.BqlInt.Field<qtyWeek> { }
        #endregion

        #region QtyMonth
        [PXDBInt()]
        [PXUIField(DisplayName = "Qty Month")]
        public virtual int? QtyMonth { get; set; }
        public abstract class qtyMonth : PX.Data.BQL.BqlInt.Field<qtyMonth> { }
        #endregion

        #region Revision
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Revision")]
        public virtual string Revision { get; set; }
        public abstract class revision : PX.Data.BQL.BqlString.Field<revision> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region TransactionExistsFlag
        [PXDBString(1, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction Exists Flag")]
        public virtual string TransactionExistsFlag { get; set; }
        public abstract class transactionExistsFlag : PX.Data.BQL.BqlString.Field<transactionExistsFlag> { }
        #endregion
    }
}