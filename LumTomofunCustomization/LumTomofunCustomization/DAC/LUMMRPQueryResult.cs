using System;
using PX.Data;

namespace LUMTomofunCustomization
{
    [Serializable]
    [PXCacheName("LUMMRPQueryResult")]
    public class LUMMRPQueryResult : IBqlTable
    {
        #region SequenceID
        [PXDBIdentity(IsKey = true)]
        public virtual int? SequenceID { get; set; }
        public abstract class sequenceID : PX.Data.BQL.BqlInt.Field<sequenceID> { }
        #endregion

        #region Mrptype
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Mrptype")]
        public virtual string Mrptype { get; set; }
        public abstract class mrptype : PX.Data.BQL.BqlString.Field<mrptype> { }
        #endregion

        #region Sku
        [PXDBString(30, IsUnicode = true, InputMask = "")]
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
        [PXDBString(128, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Country")]
        public virtual string Country { get; set; }
        public abstract class country : PX.Data.BQL.BqlString.Field<country> { }
        #endregion

        #region Warehouse
        [PXDBString(30, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Warehouse")]
        public virtual string Warehouse { get; set; }
        public abstract class warehouse : PX.Data.BQL.BqlString.Field<warehouse> { }
        #endregion

        #region Date
        [PXDBDate()]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
        #endregion

        #region Qty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty")]
        public virtual Decimal? Qty { get; set; }
        public abstract class qty : PX.Data.BQL.BqlDecimal.Field<qty> { }
        #endregion

        #region QtyWeek
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Week")]
        public virtual Decimal? QtyWeek { get; set; }
        public abstract class qtyWeek : PX.Data.BQL.BqlDecimal.Field<qtyWeek> { }
        #endregion

        #region QtyMonth
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty Month")]
        public virtual Decimal? QtyMonth { get; set; }
        public abstract class qtyMonth : PX.Data.BQL.BqlDecimal.Field<qtyMonth> { }
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