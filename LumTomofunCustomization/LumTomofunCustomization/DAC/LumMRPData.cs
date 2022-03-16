using System;
using PX.Data;

namespace LumTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LumMRPData")]
    public class LumMRPData : IBqlTable
    {
        #region Openso
        [PXDBInt()]
        [PXUIField(DisplayName = "Openso")]
        public virtual int? Openso { get; set; }
        public abstract class openso : PX.Data.BQL.BqlInt.Field<openso> { }
        #endregion

        #region Issued
        [PXDBInt()]
        [PXUIField(DisplayName = "Issued")]
        public virtual int? Issued { get; set; }
        public abstract class issued : PX.Data.BQL.BqlInt.Field<issued> { }
        #endregion

        #region Stock
        [PXDBInt()]
        [PXUIField(DisplayName = "Stock")]
        public virtual int? Stock { get; set; }
        public abstract class stock : PX.Data.BQL.BqlInt.Field<stock> { }
        #endregion

        #region ActStock
        [PXDBInt()]
        [PXUIField(DisplayName = "Act Stock OH")]
        public virtual int? ActStock { get; set; }
        public abstract class actStock : PX.Data.BQL.BqlInt.Field<actStock> { }
        #endregion

        #region Supply
        [PXDBInt()]
        [PXUIField(DisplayName = "Supply")]
        public virtual int? Supply { get; set; }
        public abstract class supply : PX.Data.BQL.BqlInt.Field<supply> { }
        #endregion

        #region Date
        [PXDBDate(IsKey = true)]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.BQL.BqlDateTime.Field<date> { }
        #endregion

        #region Sku
        [PXDBString(20, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Sku")]
        public virtual string Sku { get; set; }
        public abstract class sku : PX.Data.BQL.BqlString.Field<sku> { }
        #endregion

        #region Country
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Country")]
        public virtual string Country { get; set; }
        public abstract class country : PX.Data.BQL.BqlString.Field<country> { }
        #endregion

        #region WareHouse
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Ware House")]
        public virtual string WareHouse { get; set; }
        public abstract class wareHouse : PX.Data.BQL.BqlString.Field<wareHouse> { }
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