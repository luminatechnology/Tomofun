using PX.Data;
using PX.Objects.CM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUMLocalization.DAC
{
    [Serializable]
    [PXCacheName("RefreshTWCustomsDutyFilter")]
    public class RefreshTWCustomsDutyFilter : IBqlTable
    {
        #region Crrncd
        [PXDBString(3, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Currency")]
        [PXStringList(
                new string[] { "ALL", "ARS", "AUD", "BRL", "CAD",
                               "CHF", "CLP", "CNY", "DKK", "EUR",
                               "GBP", "HKD", "IDR", "ILS", "INR",
                               "JPY", "KRW", "MYR", "NOK", "NZD",
                               "PEN", "PHP", "PLN", "SEK", "SGD",
                               "THB", "TWD", "USD", "ZAR"},
                new string[] { "ALL", "ARS", "AUD", "BRL", "CAD",
                               "CHF", "CLP", "CNY", "DKK", "EUR",
                               "GBP", "HKD", "IDR", "ILS", "INR",
                               "JPY", "KRW", "MYR", "NOK", "NZD",
                               "PEN", "PHP", "PLN", "SEK", "SGD",
                               "THB", "TWD", "USD", "ZAR"}
        )]
        [PXDefault("ALL")]
        public virtual string Crrncd { get; set; }
        public abstract class crrncd : PX.Data.BQL.BqlString.Field<crrncd> { }
        #endregion

        #region CuryEffDate
        [PXDBDate(MinValue = "01/01/2020")]
		[PXDefault(typeof(AccessInfo.businessDate))]
		[PXUIField(DisplayName = "Currency Effective Date", Visibility = PXUIVisibility.Visible)]
		public virtual DateTime? CuryEffDate { get; set; }
        public abstract class curyEffDate : PX.Data.BQL.BqlDateTime.Field<curyEffDate> { }
        #endregion

        #region BuyingCuryRateTypeID
        [PXDBString(6, IsUnicode = true)]
        [PXUIField(DisplayName = "Buying Rate Type")]
        [PXSelector(typeof(Search<CurrencyRateType.curyRateTypeID>))]
        public virtual string BuyingCuryRateTypeID { get; set; }
        public abstract class buyingCuryRateTypeID : PX.Data.BQL.BqlString.Field<buyingCuryRateTypeID> { }
        #endregion

        #region SellingCuryRateTypeID
        [PXDBString(6, IsUnicode = true)]
        [PXUIField(DisplayName = "Selling Rate Type")]
        [PXSelector(typeof(Search<CurrencyRateType.curyRateTypeID>))]
        public virtual string SellingCuryRateTypeID { get; set; }
        public abstract class sellingCuryRateTypeID : PX.Data.BQL.BqlString.Field<sellingCuryRateTypeID> { }
        #endregion
    }
}
