using System;
using PX.Data;
using PX.Objects.IN;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUM 3PL IN Reconciliation")]
    public class LUM3PLINReconciliation : IBqlTable
    {
        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region IdentityID
        [PXDBIdentity(IsKey = true)]
        public virtual int? IdentityID { get; set; }
        public abstract class identityID : PX.Data.BQL.BqlInt.Field<identityID> { }
        #endregion
    
        #region ThirdPLType
        [PXDBString(1, IsFixed = true)]
        [PXUIField(DisplayName = "3PL Type")]
        [ThirdPLType.List]
        public virtual string ThirdPLType { get; set; }
        public abstract class thirdPLType : PX.Data.BQL.BqlString.Field<thirdPLType> { }
        #endregion
    
        #region TranDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Tran Date")]
        public virtual DateTime? TranDate { get; set; }
        public abstract class tranDate : PX.Data.BQL.BqlDateTime.Field<tranDate> { }
        #endregion
    
        #region Sku
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "Sku")]
        public virtual string Sku { get; set; }
        public abstract class sku : PX.Data.BQL.BqlString.Field<sku> { }
        #endregion
    
        #region ProductName
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = "Product Name")]
        public virtual string ProductName { get; set; }
        public abstract class productName : PX.Data.BQL.BqlString.Field<productName> { }
        #endregion
    
        #region Qty
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Qty")]
        public virtual decimal? Qty { get; set; }
        public abstract class qty : PX.Data.BQL.BqlDecimal.Field<qty> { }
        #endregion
    
        #region FBACenterID
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Fulfillment Center ID")]
        public virtual string FBACenterID { get; set; }
        public abstract class fBACenterID : PX.Data.BQL.BqlString.Field<fBACenterID> { }
        #endregion
    
        #region DetailedDesc
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Detailed Description", Visible = false)]
        public virtual string DetailedDesc { get; set; }
        public abstract class detailedDesc : PX.Data.BQL.BqlString.Field<detailedDesc> { }
        #endregion
    
        #region CountryID
        [PXDBString(2, IsUnicode = true)]
        [PXUIField(DisplayName = "Country")]
        public virtual string CountryID { get; set; }
        public abstract class countryID : PX.Data.BQL.BqlString.Field<countryID> { }
        #endregion

        #region Warehouse
        [PXDBInt()]
        [PXUIField(DisplayName = "Warehouse")]
        [PXSelector(typeof(Search<INSite.siteID>), SubstituteKey = typeof(INSite.siteCD), DescriptionField = typeof(INSite.descr))]
        public virtual int? Warehouse { get; set; }
        public abstract class warehouse : PX.Data.BQL.BqlInt.Field<warehouse> { }
        #endregion

        #region Location
        [Location(typeof(warehouse))]
        public virtual int? Location { get; set; }
        public abstract class location : PX.Data.BQL.BqlInt.Field<location> { }
        #endregion
    
        #region ERPSku
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "ERP Sku")]
        public virtual string ERPSku { get; set; }
        public abstract class eRPSku : PX.Data.BQL.BqlString.Field<eRPSku> { }
        #endregion
    
        #region IsProcessed
        [PXDBBool()]
        [PXUIField(DisplayName = "Is Processed")]
        [PXDefault(false)]
        public virtual bool? IsProcessed { get; set; }
        public abstract class isProcessed : PX.Data.BQL.BqlBool.Field<isProcessed> { }
        #endregion

        #region INDate
        [PXDBDate()]
        [PXUIField(DisplayName = "IN Date")]
        public virtual DateTime? INDate { get; set; }
        public abstract class iNDate : PX.Data.BQL.BqlDateTime.Field<iNDate> { }
        #endregion

        #region RMACode
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "RMA Code")]
        public virtual string RMACode { get; set; }
        public abstract class rMACode : PX.Data.BQL.BqlString.Field<rMACode> { }
        #endregion

        #region WHRemarks
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "Warehouse Remarks")]
        public virtual string WHRemarks { get; set; }
        public abstract class wHRemarks : PX.Data.BQL.BqlString.Field<wHRemarks> { }
        #endregion

        #region AssignSku
        [PXDBInt()]
        [PXUIField(DisplayName = "Assign SKU")]
        public virtual int? AssignSku { get; set; }
        public abstract class assignSku : PX.Data.BQL.BqlInt.Field<assignSku> { }
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
        [PXUIField(DisplayName = "Created On")]
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
    
        #region NoteID
        [PXNote()]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
    
        #region Tstamp
        [PXDBTimestamp()]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }

    #region ThirdPLType
    public class ThirdPLType
    {
        public const string Topest = "T";
        public const string ReturnHelper = "R";
        public const string FedEx = "F";
        public const string GoogleSheets = "G";

        public class topest : PX.Data.BQL.BqlString.Constant<topest>
        {
            public topest() : base(Topest) { }
        }
        public class returnHelper : PX.Data.BQL.BqlString.Constant<returnHelper>
        {
            public returnHelper() : base(ReturnHelper) { }
        }
        public class fedEx : PX.Data.BQL.BqlString.Constant<fedEx>
        {
            public fedEx() : base(FedEx) { }
        }
        public class googleSheets : PX.Data.BQL.BqlString.Constant<googleSheets>
        {
            public googleSheets() : base(GoogleSheets) { }
        }

        public class ListAttribute : PXStringListAttribute
        {
            public ListAttribute() : base(new[]
                                          {
                                              Pair(Topest, nameof(Topest)),
                                              Pair(ReturnHelper, "Return Helper"),
                                              Pair(FedEx, nameof(FedEx)),
                                              Pair(GoogleSheets, "Google Sheets"),
                                          }) { }
        }
    }
    #endregion

    public class TopestStock
    {
        public const int NY1C = 7;
        public const int LA1C = 8;
        public const int USNJ = 10;
        public const int NY2C = 11;
        public const int LA2C = 12;
        public const int SZ1C = 13;
        public const int LA90638 = 14;
        public const int CA1C = 18;
        public const int UK1C = 19;
        public const int DE1C = 20;
        public const int AU1C = 21; 
        public const int LAFBA = 22; 
        public const int USFP = 23;
        public const int LA3C = 24;

        public class ListAttribute : PXIntListAttribute
        {
            public ListAttribute() : base(new[]
                                          {
                                              Pair(NY1C, nameof(NY1C)),
                                              Pair(LA1C, nameof(LA1C)),
                                              Pair(USNJ, nameof(USNJ)),
                                              Pair(NY2C, nameof(NY2C)),
                                              Pair(LA2C, nameof(LA2C)),
                                              Pair(SZ1C, nameof(SZ1C)),
                                              Pair(LA90638, "LA-90638 仓"),
                                              Pair(CA1C, nameof(CA1C)),
                                              Pair(DE1C, nameof(DE1C)),
                                              Pair(AU1C, nameof(AU1C)),
                                              Pair(LAFBA, "LA-FBA"),
                                              Pair(USFP, "US-FP"),
                                              Pair(LA3C, nameof(LA3C))
                                          })
            { }
        }
    }
}