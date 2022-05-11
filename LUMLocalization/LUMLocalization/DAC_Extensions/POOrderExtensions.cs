using CRLocation = PX.Objects.CR.Standalone.Location;
using PX.Common;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.CM;
using PX.Objects.Common.Bql;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects.PO;
using PX.Objects;
using PX.SM;
using PX.TM;
using System.Collections.Generic;
using System;

namespace PX.Objects.PO
{
    public class POOrderExt : PXCacheExtension<POOrder>
    {
        #region UsrICSOCreated
        [PXDBBool]
        [PXUIField(DisplayName = "IC SO Created", Enabled = false)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? UsrICSOCreated { get; set; }
        public abstract class usrICSOCreated : PX.Data.BQL.BqlBool.Field<usrICSOCreated> { }
        #endregion
    }
}