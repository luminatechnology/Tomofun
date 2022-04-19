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

        #region USMarketplaceID
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "USMarketplace ID")]
        public virtual string USMarketplaceID { get; set; }
        public abstract class uSMarketplaceID : PX.Data.BQL.BqlString.Field<uSMarketplaceID> { }
        #endregion

        #region USRefreshToken
        [PXDBString(400,IsUnicode = true)]
        [PXUIField(DisplayName = "USRefresh Token")]
        public virtual string USRefreshToken { get; set; }
        public abstract class uSRefreshToken : PX.Data.BQL.BqlString.Field<uSRefreshToken> { }
        #endregion

        #region EUMarketplaceID
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "EUMarketplace ID")]
        public virtual string EUMarketplaceID { get; set; }
        public abstract class eUMarketplaceID : PX.Data.BQL.BqlString.Field<eUMarketplaceID> { }
        #endregion

        #region EURefreshToken
        [PXDBString(IsUnicode = true)]
        [PXUIField(DisplayName = "EURefresh Token")]
        public virtual string EURefreshToken { get; set; }
        public abstract class eURefreshToken : PX.Data.BQL.BqlString.Field<eURefreshToken> { }
        #endregion

        #region JPMarketplaceID
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "JPMarketplace ID")]
        public virtual string JPMarketplaceID { get; set; }
        public abstract class jPMarketplaceID : PX.Data.BQL.BqlString.Field<jPMarketplaceID> { }
        #endregion

        #region JPRefreshToken
        [PXDBString(IsUnicode = true)]
        [PXUIField(DisplayName = "JPRefresh Token")]
        public virtual string JPRefreshToken { get; set; }
        public abstract class jPRefreshToken : PX.Data.BQL.BqlString.Field<jPRefreshToken> { }
        #endregion

        #region AUMarketplaceID
        [PXDBString(200, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "AUMarketplace ID")]
        public virtual string AUMarketplaceID { get; set; }
        public abstract class aUMarketplaceID : PX.Data.BQL.BqlString.Field<aUMarketplaceID> { }
        #endregion

        #region AURefreshToken
        [PXDBString(IsUnicode = true)]
        [PXUIField(DisplayName = "AURefresh Token")]
        public virtual string AURefreshToken { get; set; }
        public abstract class aURefreshToken : PX.Data.BQL.BqlString.Field<aURefreshToken> { }
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