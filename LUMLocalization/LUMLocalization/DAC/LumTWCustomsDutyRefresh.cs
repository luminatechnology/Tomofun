using System;
using PX.Data;
using PX.Objects.CM;

namespace LUMLocalization.DAC
{
    [Serializable]
    public class LumTWCustomsDutyRefresh : IBqlTable
    {
        #region Selected
        [PXBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
		#endregion

		#region FromCuryID
		[PXDBString(5, IsKey = true, IsUnicode = true)]
		[PXDefault]
		[PXUIField(DisplayName = "From Currency", Visibility = PXUIVisibility.SelectorVisible, Required = true)]
		[PXSelector(typeof(Search<CurrencyList.curyID, Where<CurrencyList.isActive, Equal<True>>>))]
		public virtual string FromCuryID { get; set; }
		public abstract class fromCuryID : PX.Data.BQL.BqlString.Field<fromCuryID> { }
		#endregion

		#region BuyingCuryRateType
		[PXDBString(6, IsUnicode = true)]
		[PXDefault]
		[PXUIField(DisplayName = "Buying Currency Rate Type", Visibility = PXUIVisibility.Visible)]
		[PXSelector(typeof(CurrencyRateType.curyRateTypeID), DescriptionField = typeof(CurrencyRateType.descr))]
		public virtual string BuyingCuryRateType { get; set; }
		public abstract class buyingCuryRateType : PX.Data.BQL.BqlString.Field<buyingCuryRateType> { }
		#endregion

		#region BuyingCuryRate
		[PXDBDecimal(8, MinValue = 0.0)]
		[PXDefault]
		[PXUIField(DisplayName = "Buying Currency Rate", Visibility = PXUIVisibility.SelectorVisible)]
		public virtual decimal? BuyingCuryRate { get; set; }
		public abstract class buyingCuryRate : PX.Data.BQL.BqlDecimal.Field<buyingCuryRate> { }
		#endregion

		#region SellingCuryRateType
		[PXDBString(6, IsUnicode = true)]
		[PXDefault]
		[PXUIField(DisplayName = "Selling Currency Rate Type", Visibility = PXUIVisibility.Visible)]
		[PXSelector(typeof(CurrencyRateType.curyRateTypeID), DescriptionField = typeof(CurrencyRateType.descr))]
		public virtual string SellingCuryRateType { get; set; }
		public abstract class sellingCuryRateType : PX.Data.BQL.BqlString.Field<sellingCuryRateType> { }
		#endregion

		#region SellingingCuryRate
		[PXDBDecimal(8, MinValue = 0.0)]
		[PXDefault]
		[PXUIField(DisplayName = "Selling Currency Rate", Visibility = PXUIVisibility.SelectorVisible)]
		public virtual decimal? SellingCuryRate { get; set; }
		public abstract class sellingCuryRate : PX.Data.BQL.BqlDecimal.Field<sellingCuryRate> { }
		#endregion
	}
}