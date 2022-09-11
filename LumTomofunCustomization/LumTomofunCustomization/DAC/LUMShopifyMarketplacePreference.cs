using PX.Data;
using PX.Objects.AR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMShopifyMarketplacePreference")]
    public class LUMShopifyMarketplacePreference : IBqlTable
    {
        #region BAccount
        [PXDBInt(IsKey = true)]
        [PXSelector(typeof(Search<Customer.bAccountID>),
            typeof(Customer.acctCD),
            typeof(Customer.acctName),
            DescriptionField = typeof(Customer.acctName),
            SubstituteKey = typeof(Customer.acctCD))]
        [PXUIField(DisplayName = "CustomerID")]
        public virtual int? BAccountID { get; set; }
        public abstract class bAccountID : PX.Data.BQL.BqlInt.Field<bAccountID> { }
        #endregion

        #region Marketplace
        [PXDBString(20, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Marketplace")]
        public virtual string Marketplace { get; set; }
        public abstract class marketplace : PX.Data.BQL.BqlString.Field<marketplace> { }
        #endregion

        #region IsTaxCalculation
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Tax Calculation")]
        public virtual bool? IsTaxCalculation { get;set;}
        public abstract class isTaxCalculation : PX.Data.BQL.BqlBool.Field<isTaxCalculation> { }
        #endregion

        #region TimeZone
        [PXDBInt]
        [PXUIField(DisplayName = "Time Zone(GMT+?)")]
        public virtual int? TimeZone { get; set; }
        public abstract class timeZone : PX.Data.BQL.BqlInt.Field<timeZone> { }
        #endregion

        #region Noteid
        [PXNote()]
        public virtual Guid? Noteid { get; set; }
        public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
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
