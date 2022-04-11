using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.GL;

namespace LUMTomofunCustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMAmazonSourceData")]
    public class LUMAmazonSourceData : IBqlTable
    {

        public class PK : PrimaryKeyOf<LUMAmazonSourceData>.By<sequenceNumber>
        {
            public static LUMAmazonSourceData Find(PXGraph graph, int seqNo) => FindBy(graph, seqNo);
        }

        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region SequenceNumber
        [PXDBIdentity(IsKey = true)]
        [PXUIField(DisplayName = "Sequence Number")]
        public virtual int? SequenceNumber { get; set; }
        public abstract class sequenceNumber : PX.Data.BQL.BqlInt.Field<sequenceNumber> { }
        #endregion

        #region BranchID
        [Branch()]
        [PXDefault(typeof(AccessInfo.branchID), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region APIType
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "API Type")]
        public virtual string APIType { get; set; }
        public abstract class aPIType : PX.Data.BQL.BqlString.Field<aPIType> { }
        #endregion

        #region TransactionType
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Transaction Type")]
        public virtual string TransactionType { get; set; }
        public abstract class transactionType : PX.Data.BQL.BqlString.Field<transactionType> { }
        #endregion

        #region Marketplace
        [PXDBString(20, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Marketplace")]
        public virtual string Marketplace { get; set; }
        public abstract class marketplace : PX.Data.BQL.BqlString.Field<marketplace> { }
        #endregion

        #region JsonSource
        [PXDBString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Json Source")]
        public virtual string JsonSource { get; set; }
        public abstract class jsonSource : PX.Data.BQL.BqlString.Field<jsonSource> { }
        #endregion

        #region IsProcessed
        [PXDBBool]
        [PXUIField(DisplayName = "IsProcessed")]
        public virtual bool? IsProcessed { get; set; }
        public abstract class isProcessed : PX.Data.BQL.BqlBool.Field<isProcessed> { }
        #endregion

        #region IsSkippedProcess
        [PXDBBool]
        [PXUIField(DisplayName = "IsSkipped")]
        public virtual bool? IsSkippedProcess { get; set; }
        public abstract class isSkippedProcess : PX.Data.BQL.BqlBool.Field<isSkippedProcess> { }
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
        [PXUIField(Enabled = false)]
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