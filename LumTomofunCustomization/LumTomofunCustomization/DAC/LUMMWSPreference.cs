using System;
using PX.Data;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMMWSPreference")]
    public class LUMMWSPreference : IBqlTable
    {
        #region AccessKey
        [PXRSACryptString(IsUnicode = true)]
        [PXUIField(DisplayName = "Access Key")]
        public virtual string AccessKey { get; set; }
        public abstract class accessKey : PX.Data.BQL.BqlString.Field<accessKey> { }
        #endregion

        #region SecretKey
        [PXRSACryptString(IsUnicode = true)]
        [PXUIField(DisplayName = "Secret Key")]
        public virtual string SecretKey { get; set; }
        public abstract class secretKey : PX.Data.BQL.BqlString.Field<secretKey> { }
        #endregion

        #region RoleArn
        [PXRSACryptString(IsUnicode = true)]
        [PXUIField(DisplayName = "Role Arn")]
        public virtual string RoleArn { get; set; }
        public abstract class roleArn : PX.Data.BQL.BqlString.Field<roleArn> { }
        #endregion

        #region ClientID
        [PXRSACryptString(IsUnicode = true)]
        [PXUIField(DisplayName = "Client ID")]
        public virtual string ClientID { get; set; }
        public abstract class clientID : PX.Data.BQL.BqlString.Field<clientID> { }
        #endregion

        #region ClientSecret
        [PXRSACryptString(IsUnicode = true)]
        [PXUIField(DisplayName = "Client Secret")]
        public virtual string ClientSecret { get; set; }
        public abstract class clientSecret : PX.Data.BQL.BqlString.Field<clientSecret> { }
        #endregion

        #region US MarketplaceID
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "US Marketplace ID")]
        public virtual string USMarketplaceID { get; set; }
        public abstract class uSMarketplaceID : PX.Data.BQL.BqlString.Field<uSMarketplaceID> { }
        #endregion

        #region US RefreshToken
        [PXDBString(400, IsUnicode = true)]
        [PXUIField(DisplayName = "US Refresh Token")]
        public virtual string USRefreshToken { get; set; }
        public abstract class uSRefreshToken : PX.Data.BQL.BqlString.Field<uSRefreshToken> { }
        #endregion

        #region EU MarketplaceID
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "EU Marketplace ID")]
        public virtual string EUMarketplaceID { get; set; }
        public abstract class eUMarketplaceID : PX.Data.BQL.BqlString.Field<eUMarketplaceID> { }
        #endregion

        #region EU RefreshToken
        [PXDBString(IsUnicode = true)]
        [PXUIField(DisplayName = "EU Refresh Token")]
        public virtual string EURefreshToken { get; set; }
        public abstract class eURefreshToken : PX.Data.BQL.BqlString.Field<eURefreshToken> { }
        #endregion

        #region JP MarketplaceID
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "JP Marketplace ID")]
        public virtual string JPMarketplaceID { get; set; }
        public abstract class jPMarketplaceID : PX.Data.BQL.BqlString.Field<jPMarketplaceID> { }
        #endregion

        #region JP RefreshToken
        [PXDBString(IsUnicode = true)]
        [PXUIField(DisplayName = "JP Refresh Token")]
        public virtual string JPRefreshToken { get; set; }
        public abstract class jPRefreshToken : PX.Data.BQL.BqlString.Field<jPRefreshToken> { }
        #endregion

        #region AU MarketplaceID
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "AU Marketplace ID")]
        public virtual string AUMarketplaceID { get; set; }
        public abstract class aUMarketplaceID : PX.Data.BQL.BqlString.Field<aUMarketplaceID> { }
        #endregion

        #region AU RefreshToken
        [PXDBString(IsUnicode = true)]
        [PXUIField(DisplayName = "AU Refresh Token")]
        public virtual string AURefreshToken { get; set; }
        public abstract class aURefreshToken : PX.Data.BQL.BqlString.Field<aURefreshToken> { }
        #endregion

        #region MX MarketplaceID
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "MX Marketplace ID")]
        public virtual string MXMarketplaceID { get; set; }
        public abstract class mXMarketplaceID : PX.Data.BQL.BqlString.Field<mXMarketplaceID> { }
        #endregion

        #region MX RefreshToken
        [PXDBString(IsUnicode = true)]
        [PXUIField(DisplayName = "MX Refresh Token")]
        public virtual string MXRefreshToken { get; set; }
        public abstract class mXRefreshToken : PX.Data.BQL.BqlString.Field<mXRefreshToken> { }
        #endregion

        #region MXClientID
        [PXRSACryptString(IsUnicode = true)]
        [PXUIField(DisplayName = "MX Client ID")]
        public virtual string MXClientID { get; set; }
        public abstract class mxClientID : PX.Data.BQL.BqlString.Field<mxClientID> { }
        #endregion

        #region MXClientSecret
        [PXRSACryptString(IsUnicode = true)]
        [PXUIField(DisplayName = "MX Client Secret")]
        public virtual string MXClientSecret { get; set; }
        public abstract class mxClientSecret : PX.Data.BQL.BqlString.Field<mxClientSecret> { }
        #endregion

        #region SGAccessKey
        [PXRSACryptString(IsUnicode = true)]
        [PXUIField(DisplayName = "SG Access Key")]
        public virtual string SGAccessKey { get; set; }
        public abstract class sgAccessKey : PX.Data.BQL.BqlString.Field<sgAccessKey> { }
        #endregion

        #region SGSecretKey
        [PXRSACryptString(IsUnicode = true)]
        [PXUIField(DisplayName = "SG Secret Key")]
        public virtual string SGSecretKey { get; set; }
        public abstract class sgSecretKey : PX.Data.BQL.BqlString.Field<sgSecretKey> { }
        #endregion

        #region SGRoleArn
        [PXRSACryptString(IsUnicode = true)]
        [PXUIField(DisplayName = "SG Role Arn")]
        public virtual string SGRoleArn { get; set; }
        public abstract class sgRoleArn : PX.Data.BQL.BqlString.Field<sgRoleArn> { }
        #endregion

        #region SGClientID
        [PXRSACryptString(IsUnicode = true)]
        [PXUIField(DisplayName = "SG Client ID")]
        public virtual string SGClientID { get; set; }
        public abstract class sgClientID : PX.Data.BQL.BqlString.Field<sgClientID> { }
        #endregion

        #region SGClientSecret
        [PXRSACryptString(IsUnicode = true)]
        [PXUIField(DisplayName = "SG Client Secret")]
        public virtual string SGClientSecret { get; set; }
        public abstract class sgClientSecret : PX.Data.BQL.BqlString.Field<sgClientSecret> { }
        #endregion

        #region SGMarketplaceID
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "SG Marketplace ID")]
        public virtual string SGMarketplaceID { get; set; }
        public abstract class sgMarketplaceID : PX.Data.BQL.BqlString.Field<sgMarketplaceID> { }
        #endregion

        #region SGRefreshToken
        [PXDBString(400, IsUnicode = true)]
        [PXUIField(DisplayName = "SG Refresh Token")]
        public virtual string SGRefreshToken { get; set; }
        public abstract class sgRefreshToken : PX.Data.BQL.BqlString.Field<sgRefreshToken> { }
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