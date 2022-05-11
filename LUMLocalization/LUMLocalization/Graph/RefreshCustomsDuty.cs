using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CM;
using PX.Objects.CS;
using LUMLocalization.DAC;
using LUMLocalization.Helper;

namespace LUMLocalization.Graph
{
    public class RefreshCustomsDuty : PXGraph<RefreshCustomsDuty>
    {
		public PXCancel<RefreshTWCustomsDutyFilter> Cancel;
		public PXFilter<RefreshTWCustomsDutyFilter> Filter;
		[PXFilterable(new Type[]{})]
		public PXFilteredProcessing<LumTWCustomsDutyRefresh, RefreshTWCustomsDutyFilter> CurrencyRateList;
		private Dictionary<string, int> CustomsDutyCurrDic;
		public class ToTWDCurrency : PX.Data.BQL.BqlString.Constant<ToTWDCurrency>
		{
			public ToTWDCurrency() : base("TWD") { }
		}

		#region Constructor
		public RefreshCustomsDuty()
		{
			CustomsDutyCurrDic = new Dictionary<string, int>();
		}
        #endregion

        #region Event Method
        protected virtual void RefreshTWCustomsDutyFilter_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
		{
			RefreshTWCustomsDutyFilter filter = (RefreshTWCustomsDutyFilter)e.Row;
			if (filter != null)
			{
				this.CurrencyRateList.SetProcessDelegate(delegate (List<LumTWCustomsDutyRefresh> list)
				{
					this.RefreshRates(filter, list);
				});
			}
		}

		protected virtual void RefreshTWCustomsDutyFilter_CuryEffDate_FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
		{
			DateTime? newValue = (DateTime?)e.NewValue;
			if (newValue == null)
			{
				throw new PXSetPropertyException("You must enter a valid date. Rates can be refreshed up to the current ten days.");
			}
		}
        #endregion

        protected virtual IEnumerable currencyRateList()
		{
			foreach (PXResult<CurrencyList> pxresult in PXSelect<CurrencyList,
														Where<CurrencyList.isActive, Equal<boolTrue>, And<CurrencyList.curyID, NotEqual<ToTWDCurrency>>>
														>.Select(this, Array.Empty<object>()))
			{
				PXResult<CurrencyList> r = (PXResult<CurrencyList>)pxresult;
				CurrencyList curr = r;
				LumTWCustomsDutyRefresh rate = new LumTWCustomsDutyRefresh();
				rate.FromCuryID = curr.CuryID;
				this.CurrencyRateList.Cache.SetStatus(rate, PXEntryStatus.Held);
				yield return rate;
			}
			IEnumerator<PXResult<CurrencyList>> enumerator = null;
			yield break;
			yield break;
		}

		[WebPermission(SecurityAction.Assert, Unrestricted = true)]
		public virtual void RefreshRates(RefreshTWCustomsDutyFilter filter, List<LumTWCustomsDutyRefresh> list)
		{
			RefreshCustomsDuty refreshCustomsDuty = PXGraph.CreateInstance<RefreshCustomsDuty>();
			TWCustomsDutyResponse currentCustomsDuty = refreshCustomsDuty.GetCurrentCustomsDutyFromService(filter);//GetLatestCustomsDutyFromService();
			CreateCustomsDutyDicByResp(currentCustomsDuty.data);

			CuryRateMaint graph = PXGraph.CreateInstance<CuryRateMaint>();
			graph.Filter.Current.ToCurrency = "TWD"; //filter.CuryID;
			graph.Filter.Current.EffDate = filter.CuryEffDate.Value.AddDays(-GetEffDaysBasedOnTenDay(filter.CuryEffDate.Value.Day));
			bool hasError = false;
			bool reateTypeError = false;
			for (int i = 0; i < list.Count; i++)
			{
				LumTWCustomsDutyRefresh rr = list[i];
				if (CustomsDutyCurrDic.ContainsKey(list[i].FromCuryID))
				{
					if (filter.BuyingCuryRateTypeID == null && filter.SellingCuryRateTypeID == null) 
					{
						PXProcessing<LumTWCustomsDutyRefresh>.SetError(i, PXMessages.LocalizeFormatNoPrefixNLA("Buying and Selling Rate Type must be chosen at least one."));
						reateTypeError = true;
					}

					int index = CustomsDutyCurrDic[list[i].FromCuryID];

					if (!reateTypeError)
                    {
						//Buying Rate
						if (filter.BuyingCuryRateTypeID != null)
						{
							CurrencyRate curyBuyingRate = graph.CuryRateRecordsEntry.Insert();
							curyBuyingRate.FromCuryID = currentCustomsDuty.data[index].CRRN_CD;
							curyBuyingRate.ToCuryID = "TWD";
							curyBuyingRate.CuryRateType = filter.BuyingCuryRateTypeID;
							curyBuyingRate.CuryRate = currentCustomsDuty.data[index].IN_RATE;
							curyBuyingRate.CuryMultDiv = "M";
							rr.BuyingCuryRateType = filter.BuyingCuryRateTypeID;
							rr.BuyingCuryRate = curyBuyingRate.CuryRate;
							graph.CuryRateRecordsEntry.Update(curyBuyingRate);
						}

						//Selling Rate
						if (filter.SellingCuryRateTypeID != null)
						{
							CurrencyRate curySellingRate = graph.CuryRateRecordsEntry.Insert();
							curySellingRate.FromCuryID = currentCustomsDuty.data[index].CRRN_CD;
							curySellingRate.ToCuryID = "TWD";
							curySellingRate.CuryRateType = filter.SellingCuryRateTypeID;
							curySellingRate.CuryRate = currentCustomsDuty.data[index].EX_RATE;
							curySellingRate.CuryMultDiv = "M";
							rr.SellingCuryRateType = filter.SellingCuryRateTypeID;
							rr.SellingCuryRate = curySellingRate.CuryRate;
							graph.CuryRateRecordsEntry.Update(curySellingRate);
						}
						PXProcessing<LumTWCustomsDutyRefresh>.SetInfo(i, $"The currency {rr.FromCuryID} rate has been processed successfully.");
					}
				}
				else
				{
					/*
					PXProcessing<LumTWCustomsDutyRefresh>.SetError(i, PXMessages.LocalizeFormatNoPrefixNLA("No exchange rate could be found online for currency {0}.", new object[]
					{
						rr.FromCuryID
					}));
					*/
					PXProcessing<LumTWCustomsDutyRefresh>.SetInfo(i, $"The currency {list[i].FromCuryID} couldn't find.");
					hasError = true;
				}
			}
			graph.Actions.PressSave();

			if (reateTypeError) throw new PXOperationCompletedWithErrorException("Buying and Selling Rate Type must be chosen at least one.");
			if (hasError) throw new PXOperationCompletedWithErrorException("Rates for one or more currencies couldn't be refreshed.");
		}

        #region Customization Methods for getting customs duty by API 

        /**
		 * For effDay
		 * 1 => 1
		 * 2 => 11
		 * 3 => 21
		 */
        private int GetEffDaysBasedOnTenDay(int day)
        {
			if (day > 30) return ((day % 10) - 1 + 10) % 10 + 10;
			else return ((day % 10) - 1 + 10) % 10;
		}

		private string GetTenDays(int day)
		{
			if (day > 20) return "3";
			else if (day > 10) return "2";
			else return "1";
		}

		/**
		 * Get ALL Currencies Customs Duty from https://portal.sw.nat.gov.tw/APGQO/GC331
		 */
		public TWCustomsDutyResponse GetCurrentCustomsDutyFromService(RefreshTWCustomsDutyFilter filter)
		{
			//Next Duty Date
			string year = filter.CuryEffDate.Value.Year.ToString();
			string mon = filter.CuryEffDate.Value.Month.ToString().PadLeft(2, '0');
			string tenDay = GetTenDays(filter.CuryEffDate.Value.Day);
			string ratesRequestURL = $"https://portal.sw.nat.gov.tw/APGQO/GC331!query?formBean.year={year}&formBean.mon={mon}&formBean.tenDay={tenDay}";
			PXTrace.WriteInformation("Refresh rates URL: " + ratesRequestURL);

			string response = new WebClient().DownloadString(new Uri(ratesRequestURL));
			var tWCustomsDutyResp = JsonConvert.DeserializeObject<TWCustomsDutyResponse>(response);
			if (tWCustomsDutyResp == null) PXTrace.WriteInformation("Refresh API Response Null");

			return tWCustomsDutyResp;
		}

		/**
		 * For tenDay
		 * 1 => 1
		 * 2 => 11
		 * 3 => 21
		 */
		public virtual int GetDiffDaysBasedOnTenDay(int day)
		{
			if (day > 21) return (day - 21);
			if (day > 11) return (day - 11);
			if (day > 1) return (day - 1);
			return 0;
		}

		/**
		 * Get ALL Currencies Next Customs Duty from https://portal.sw.nat.gov.tw/APGQO/GC331
		 */
		public TWCustomsDutyResponse GetLatestCustomsDutyFromService()
		{
			//Next Duty Date
			string year = DateTime.Now.AddDays(10).Year.ToString();
			string mon = DateTime.Now.AddDays(10).Month.ToString().PadLeft(2, '0');
			string tenDay = (int.Parse(DateTime.Now.AddDays(10).Day.ToString()) / 10 + 1).ToString();
			string ratesRequestURL = $"https://portal.sw.nat.gov.tw/APGQO/GC331!query?formBean.year={year}&formBean.mon={mon}&formBean.tenDay={tenDay}";
			PXTrace.WriteInformation("Refresh rates URL: " + ratesRequestURL);
			
			string response = new WebClient().DownloadString(new Uri(ratesRequestURL));
			var tWCustomsDutyResp = JsonConvert.DeserializeObject<TWCustomsDutyResponse>(response);
			if (tWCustomsDutyResp == null) PXTrace.WriteInformation("Refresh API Response Null");

			return tWCustomsDutyResp;
		}

		/**
		 * Add ALL Currencies into Dictionary from API response
		 */
		public virtual Dictionary<string, int> CreateCustomsDutyDicByResp(List<TWCustomsDutyData> data)
        {
			int index = 0;
            foreach (TWCustomsDutyData line in data)
            {
				CustomsDutyCurrDic.Add(line.CRRN_CD, index);
				index++;
			}
			return CustomsDutyCurrDic;
		}
        #endregion
    }
}