using System;
using System.Collections;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data.Update;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.CR;
using PX.SM;

namespace LUMLocalization.DAC
{
	[Serializable]
	[PXCacheName("LumSOForPurchaseReceiptFilter ")]
	public partial class LumSOForPurchaseReceiptFilter : IBqlTable
	{
		#region DocDate
		public abstract class docDate : PX.Data.BQL.BqlDateTime.Field<docDate> { }
		[PXDate()]
		[PXDefault(typeof(AccessInfo.businessDate), PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "Date", Visibility = PXUIVisibility.SelectorVisible)]
		public virtual DateTime? DocDate { get; set; }
		#endregion

		#region PurchasingCompany
		public abstract class purchasingCompany : PX.Data.BQL.BqlInt.Field<purchasingCompany> { }
		//[CustomerActive(DisplayName = "Purchasing Company", Visibility = PXUIVisibility.SelectorVisible, Required = false, DescriptionField = typeof(Customer.acctName), Filterable = true)]
		[VendorActive(DisplayName = "Purchasing Company", Visibility = PXUIVisibility.SelectorVisible, Required = false, DescriptionField = typeof(Branch.branchCD), Filterable = true)]
		//[PXRestrictor(typeof(Where<Customer.isBranch, Equal<True>>), "Customer Is Not Branch", typeof(Customer.acctCD))]
		[PXRestrictor(typeof(Where<Vendor.isBranch, Equal<True>>), "Vendor Is Not Branch", typeof(Vendor.acctCD))]
		public virtual int? PurchasingCompany { get; set; }
		#endregion

		#region SellingCompany 
		public abstract class sellingCompany : PX.Data.BQL.BqlInt.Field<sellingCompany> { }
		//[VendorActive(DisplayName = "Selling Company", Visibility = PXUIVisibility.SelectorVisible, Required = false, DescriptionField = typeof(Branch.branchCD), Filterable = true)]
		//[CustomerActive(DisplayName = "Purchasing Company", Visibility = PXUIVisibility.SelectorVisible, Required = false, DescriptionField = typeof(Customer.acctName), Filterable = true)]
		//[PXRestrictor(typeof(Where<Vendor.isBranch, Equal<True>>), "Vendor Is Not Branch", typeof(Vendor.acctCD))]
		//[PXRestrictor(typeof(Where<Customer.isBranch, Equal<True>>), "Customer Is Not Branch", typeof(Customer.acctCD))]
		//[PXSelector(typeof(Search<Branch.branchID>), SubstituteKey = typeof(Branch.branchCD))]
		[PXUIField(DisplayName = "Selling Company")]
		[PXDimensionSelectorAttribute("VENDOR", typeof(Search<VendorR.bAccountID,
														Where<VendorR.type, Equal<BAccountType.vendorType>,
														And<VendorR.vStatus, Equal<CustomerStatus.active>>>>),
												typeof(VendorR.acctCD),
												new Type[] { typeof(VendorR.bAccountID), typeof(VendorR.acctCD), typeof(VendorR.acctName) },
									  IsDirty = true)]
		public virtual int? SellingCompany { get; set; }
		#endregion
	}
}