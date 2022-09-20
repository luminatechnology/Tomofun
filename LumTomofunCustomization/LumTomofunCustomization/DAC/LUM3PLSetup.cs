using System;
using PX.Data;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUM 3PL Setup")]
    public class LUM3PLSetup : IBqlTable
    {
        #region TopestToken
        [PXDBString(1000, IsUnicode = true)]
        [PXUIField(DisplayName = "Token")]
        public virtual string TopestToken { get; set; }
        public abstract class topestToken : PX.Data.BQL.BqlString.Field<topestToken> { }
        #endregion

        #region TopestTokenCA
        [PXDBString(1000, IsUnicode = true)]
        [PXUIField(DisplayName = "Token_CA")]
        public virtual string TopestTokenCA { get; set; }
        public abstract class topestTokenCA : PX.Data.BQL.BqlString.Field<topestTokenCA> { }
        #endregion

        #region RHAuthzToken
        [PXDBString(1000, IsUnicode = true)]
        [PXUIField(DisplayName = "Authorization Token")]
        public virtual string RHAuthzToken { get; set; }
        public abstract class rHAuthzToken : PX.Data.BQL.BqlString.Field<rHAuthzToken> { }
        #endregion

        #region RHApiKey
        [PXDBString(200, IsUnicode = true)]
        [PXUIField(DisplayName = "API Key")]
        public virtual string RHApiKey { get; set; }
        public abstract class rHApiKey : PX.Data.BQL.BqlString.Field<rHApiKey> { }
        #endregion
    
        #region RHApiToken
        [PXDBString(1000, IsUnicode = true)]
        [PXUIField(DisplayName = "API Token")]
        public virtual string RHApiToken { get; set; }
        public abstract class rHApiToken : PX.Data.BQL.BqlString.Field<rHApiToken> { }
        #endregion
    
        #region FedExOrgName
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "Org. Name")]
        public virtual string FedExOrgName { get; set; }
        public abstract class fedExOrgName : PX.Data.BQL.BqlString.Field<fedExOrgName> { }
        #endregion
    
        #region FedExRefreshToken
        [PXDBString(1000, IsUnicode = true)]
        [PXUIField(DisplayName = "Refresh Token")]
        public virtual string FedExRefreshToken { get; set; }
        public abstract class fedExRefreshToken : PX.Data.BQL.BqlString.Field<fedExRefreshToken> { }
        #endregion
    
        #region FedExClientID
        [PXDBString(200, IsUnicode = true)]
        [PXUIField(DisplayName = "Client ID")]
        public virtual string FedExClientID { get; set; }
        public abstract class fedExClientID : PX.Data.BQL.BqlString.Field<fedExClientID> { }
        #endregion
    
        #region FedExClientSecret
        [PXDBString(200, IsUnicode = true)]
        [PXUIField(DisplayName = "Client Secret")]
        public virtual string FedExClientSecret { get; set; }
        public abstract class fedExClientSecret : PX.Data.BQL.BqlString.Field<fedExClientSecret> { }
        #endregion

        #region FedExAccessToken
        [PXDBString(1000, IsUnicode = true)]
        [PXUIField(DisplayName = "Access Token", Visible = false)]
        public virtual string FedExAccessToken { get; set; }
        public abstract class fedExAccessToken : PX.Data.BQL.BqlString.Field<fedExAccessToken> { }
        #endregion

        #region GoogleSheetsURL
        [PXDBString(1000, IsUnicode = true)]
        [PXUIField(DisplayName = "Google Sheets Address")]
        public virtual string GoogleSheetsURL { get; set; }
        public abstract class googleSheetsURL : PX.Data.BQL.BqlString.Field<googleSheetsURL> { }
        #endregion

        #region GoogleSheetName
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "Google Sheets Name")]
        public virtual string GoogleSheetName { get; set; }
        public abstract class googleSheetName : PX.Data.BQL.BqlString.Field<googleSheetName> { }
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
    }
}