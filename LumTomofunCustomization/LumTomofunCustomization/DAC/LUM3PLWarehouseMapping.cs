using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("3PL Warehouse Mapping")]
    [PXPrimaryGraph(typeof(Graph.LUM3PLWHMappingMaint))]
    public class LUM3PLWarehouseMapping : IBqlTable
    {
        #region Keys
        public class PK : PrimaryKeyOf<LUM3PLWarehouseMapping>.By<thirdPLType, sourceWH>
        {
            public static LUM3PLWarehouseMapping Find(PXGraph graph, string type, string sourceWH) => FindBy(graph, type, sourceWH);
        }
        #endregion

        #region ThirdPLType
        [PXDBString(1, IsKey = true, IsFixed = true)]
        [PXUIField(DisplayName = "3PL Name")]
        [ThirdPLType.List]
        [PXDefault()]
        public virtual string ThirdPLType { get; set; }
        public abstract class thirdPLType : PX.Data.BQL.BqlString.Field<thirdPLType> { }
        #endregion
    
        #region SourceWH
        [PXDBString(50, IsKey = true, IsUnicode = true)]
        [PXUIField(DisplayName = "3PL Warehouse")]
        [PXDefault()]
        public virtual string SourceWH { get; set; }
        public abstract class sourceWH : PX.Data.BQL.BqlString.Field<sourceWH> { }
        #endregion

        #region CountryID
        [PXDBString(2, IsUnicode = true)]
        [PX.Objects.CR.Country()]
        [PXUIField(DisplayName = "Country")]
        public virtual string CountryID { get; set; }
        public abstract class countryID : PX.Data.BQL.BqlString.Field<countryID> { }
        #endregion
    
        #region ERPWH
        [PX.Objects.IN.Site(DisplayName = "ERP Warehouse")]
        public virtual int? ERPWH { get; set; }
        public abstract class eRPWH : PX.Data.BQL.BqlInt.Field<eRPWH> { }
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
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }
}