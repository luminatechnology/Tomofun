using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PX.Objects.PO;

namespace PX.Objects.SO
{
    public class SOShipmentEntry_Extension : PXGraphExtension<SOShipmentEntry>
    {
        public override void Initialize()
        {
            base.Initialize();
            Base.report.AddMenuAction(PackingList);
            Base.report.AddMenuAction(CommercialInvoice);
        }

        #region Attributes
        //Attribute : Qty Per Carton, ID = QTYCARTON
        public class QtyPerCartonAttr : BqlString.Constant<QtyPerCartonAttr>
        {
            public QtyPerCartonAttr() : base("QTYCARTON") { }
        }

        //Attribute : Gross Weight Per Carton, ID = GRSWHTCART
        public class GrossWeightPerCartonAttr : BqlString.Constant<GrossWeightPerCartonAttr>
        {
            public GrossWeightPerCartonAttr() : base("GRSWHTCART") { }
        }

        //Attribute : Net Weight Per Carton, ID = NETWHTCART
        public class NetWeightPerCartonAttr : BqlString.Constant<NetWeightPerCartonAttr>
        {
            public NetWeightPerCartonAttr() : base("NETWHTCART") { }
        }

        //Attribute : Cartons Per Pallet, ID = PLTAIREU
        public class CartonsPerPalletAttrAirEU : BqlString.Constant<CartonsPerPalletAttrAirEU>
        {
            public CartonsPerPalletAttrAirEU() : base("PLTAIREU") { }
        }

        //Attribute : Cartons Per Pallet, ID = PLTAIRUS
        public class CartonsPerPalletAttrAirUS : BqlString.Constant<CartonsPerPalletAttrAirUS>
        {
            public CartonsPerPalletAttrAirUS() : base("PLTAIRUS") { }
        }

        //Attribute : Cartons Per Pallet, ID = PLTSEAEU
        public class CartonsPerPalletAttrSeaEU : BqlString.Constant<CartonsPerPalletAttrSeaEU>
        {
            public CartonsPerPalletAttrSeaEU() : base("PLTSEAEU") { }
        }

        //Attribute : Cartons Per Pallet, ID = PLTSEAUS
        public class CartonsPerPalletAttrSeaUS : BqlString.Constant<CartonsPerPalletAttrSeaUS>
        {
            public CartonsPerPalletAttrSeaUS() : base("PLTSEAUS") { }
        }
        #endregion

        #region Event
        protected virtual void _(Events.FieldUpdated<SOPackageDetailExt.usrShipmentSplitLineNbr> e)
        {
            if (e.NewValue == null) return;

            var _shipLine = Base.Transactions.Cache.Cached.RowCast<SOShipLine>().Where(x => x.LineNbr == (int?)e.NewValue).SingleOrDefault();
            e.Cache.SetValueExt<SOPackageDetail.inventoryID>(e.Row, _shipLine.InventoryID);
            e.Cache.SetValueExt<SOPackageDetail.qty>(e.Row, _shipLine.ShippedQty);

            //Box
            var _defBoxID = SelectFrom<CSBox>.Where<CSBox.activeByDefault.IsEqual<True>>.View.Select(Base).TopFirst?.BoxID;
            if (_defBoxID != null) e.Cache.SetValueExt<SOPackageDetail.boxID>(e.Row, _defBoxID);

        }

        protected virtual void _(Events.FieldUpdated<SOPackageDetailExt.qty> e)
        {
            if (e.NewValue == null) return;

            SOPackageDetailEx curSOPackageDetailExLine = e.Row as SOPackageDetailEx;
            SOPackageDetailExt _line = ((SOPackageDetail)Base.Packages.Cache.Current).GetExtension<SOPackageDetailExt>();

            //TotalCartons
            var _QtyPerCarton = SelectFrom<CSAnswers>.
                                LeftJoin<InventoryItem>.On<InventoryItem.noteID.IsEqual<CSAnswers.refNoteID>.And<CSAnswers.attributeID.IsEqual<QtyPerCartonAttr>>>.
                                Where<InventoryItem.inventoryID.IsEqual<@P.AsInt>>.View.Select(Base, curSOPackageDetailExLine.InventoryID).TopFirst?.Value;

            if (_QtyPerCarton != null)
            {
                if (Convert.ToDecimal(_QtyPerCarton) > 0)
                {
                    var totalCartons = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(curSOPackageDetailExLine.Qty) / Convert.ToDecimal(_QtyPerCarton)));
                    e.Cache.SetValueExt<SOPackageDetailExt.usrTotalCartons>(curSOPackageDetailExLine, totalCartons);
                    //Carton No
                    e.Cache.SetValueExt<SOPackageDetailExt.customRefNbr2>(curSOPackageDetailExLine, totalCartons);
                }
            }

            //Cartons per Pallet
            var _attrCartonsPerPallet = "";
            if (Base.Document.Current.ShipVia == "AIR")
            {
                if (Base.Shipping_Address.Current.CountryID == "US")
                    _attrCartonsPerPallet = SelectFrom<CSAnswers>.
                                            LeftJoin<InventoryItem>.On<InventoryItem.noteID.IsEqual<CSAnswers.refNoteID>.And<CSAnswers.attributeID.IsEqual<CartonsPerPalletAttrAirUS>>>.
                                            Where<InventoryItem.inventoryID.IsEqual<@P.AsInt>>.View.Select(Base, curSOPackageDetailExLine.InventoryID).TopFirst?.Value;
                else
                    _attrCartonsPerPallet = SelectFrom<CSAnswers>.
                                            LeftJoin<InventoryItem>.On<InventoryItem.noteID.IsEqual<CSAnswers.refNoteID>.And<CSAnswers.attributeID.IsEqual<CartonsPerPalletAttrAirEU>>>.
                                            Where<InventoryItem.inventoryID.IsEqual<@P.AsInt>>.View.Select(Base, curSOPackageDetailExLine.InventoryID).TopFirst?.Value;
            }
            else if (Base.Document.Current.ShipVia == "OCEAN")
            {
                if (Base.Shipping_Address.Current.CountryID == "US")
                    _attrCartonsPerPallet = SelectFrom<CSAnswers>.
                                            LeftJoin<InventoryItem>.On<InventoryItem.noteID.IsEqual<CSAnswers.refNoteID>.And<CSAnswers.attributeID.IsEqual<CartonsPerPalletAttrSeaUS>>>.
                                            Where<InventoryItem.inventoryID.IsEqual<@P.AsInt>>.View.Select(Base, curSOPackageDetailExLine.InventoryID).TopFirst?.Value;
                else
                    _attrCartonsPerPallet = SelectFrom<CSAnswers>.
                                            LeftJoin<InventoryItem>.On<InventoryItem.noteID.IsEqual<CSAnswers.refNoteID>.And<CSAnswers.attributeID.IsEqual<CartonsPerPalletAttrSeaEU>>>.
                                            Where<InventoryItem.inventoryID.IsEqual<@P.AsInt>>.View.Select(Base, curSOPackageDetailExLine.InventoryID).TopFirst?.Value;
            }
            else
            {
                _attrCartonsPerPallet = SelectFrom<CSAnswers>.
                                            LeftJoin<InventoryItem>.On<InventoryItem.noteID.IsEqual<CSAnswers.refNoteID>.And<CSAnswers.attributeID.IsEqual<CartonsPerPalletAttrSeaUS>>>.
                                            Where<InventoryItem.inventoryID.IsEqual<@P.AsInt>>.View.Select(Base, curSOPackageDetailExLine.InventoryID).TopFirst?.Value;
            }

            if (_attrCartonsPerPallet != null)
            {
                if (Convert.ToDecimal(_attrCartonsPerPallet) > 0)
                    e.Cache.SetValueExt<SOPackageDetailExt.customRefNbr1>((SOPackageDetailEx)e.Row, Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(_line.UsrTotalCartons) / Convert.ToDecimal(_attrCartonsPerPallet))));
            }
        }
        #endregion

        #region Actions
        public PXAction<SOShipment> PackingList;
        [PXButton(DisplayOnMainToolbar = false)]
        [PXUIField(DisplayName = "Print Packing List", Enabled = true, MapEnableRights = PXCacheRights.Select)]
        protected virtual IEnumerable packingList(PXAdapter adapter)
        {
            if (Base.Document.Current != null)
            {
                var _reportID = "LM642005";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["ShipmentNbr"] = Base.Document.Current.ShipmentNbr;
                throw new PXReportRequiredException(parameters, _reportID, $"Report {_reportID}") { Mode = PXBaseRedirectException.WindowMode.New };
            }
            return adapter.Get();
        }

        public PXAction<SOShipment> CommercialInvoice;
        [PXButton(DisplayOnMainToolbar = false)]
        [PXUIField(DisplayName = "Print Commercial Invoice", Enabled = true, MapEnableRights = PXCacheRights.Select)]
        protected virtual IEnumerable commercialInvoice(PXAdapter adapter)
        {
            if (Base.Document.Current != null)
            {
                if (Base.Document.Current.Status == SOShipmentStatus.Open || Base.Document.Current.Status == SOShipmentStatus.Hold) Base.Actions.PressSave();

                var poLineData = SelectFrom<SOShipLine>
                                 .InnerJoin<SOLine>.On<SOShipLine.origOrderNbr.IsEqual<SOLine.orderNbr>
                                                   .And<SOShipLine.origOrderType.IsEqual<SOLine.orderType>>
                                                   .And<SOShipLine.origLineNbr.IsEqual<SOLine.lineNbr>>>
                                 .InnerJoin<SOLineSplit>.On<SOLine.orderNbr.IsEqual<SOLineSplit.orderNbr>
                                                        .And<SOLine.orderType.IsEqual<SOLineSplit.orderType>>
                                                        .And<SOLine.lineNbr.IsEqual<SOLineSplit.lineNbr>>>
                                 .InnerJoin<POLine>.On<SOLineSplit.pOType.IsEqual<POLine.orderType>
                                                   .And<SOLineSplit.pONbr.IsEqual<POLine.orderNbr>>
                                                   .And<SOLineSplit.pOLineNbr.IsEqual<POLine.lineNbr>>>
                                .Where<SOShipLine.shipmentNbr.IsEqual<P.AsString>
                                  .And<SOShipLine.shipmentType.IsEqual<P.AsString>>>
                                 .View.Select(Base, Base.Document.Current.ShipmentNbr, Base.Document.Current.ShipmentType);
                decimal sumUsrTotalCartons = 0;
                foreach (PXResult<SOShipLine> line in Base.Transactions.Select())
                {
                    if (Base.Document.Current.ShipmentType == "I")
                    {
                        var _soShipline = SOShipLine.PK.Find(Base, ((SOShipLine)line).ShipmentNbr, ((SOShipLine)line).LineNbr);
                        var _soLine = SOLine.PK.Find(Base, _soShipline.OrigOrderType, _soShipline.OrigOrderNbr, _soShipline.OrigLineNbr);
                        sumUsrTotalCartons += (decimal)(_soLine?.CuryUnitPrice ?? 0) * (decimal)((SOShipLine)line).ShippedQty;
                    }
                    else
                    {
                        var currentPOLine = poLineData.RowCast<POLine>().FirstOrDefault(x => x.LineNbr == ((SOShipLine)line).LineNbr);
                        sumUsrTotalCartons += (decimal)((currentPOLine?.CuryUnitCost ?? 0) * (decimal)((SOShipLine)line).ShippedQty);
                    }
                }

                var shipFreight = Base.Document.Cache.GetValueExt(Base.Document.Current, PX.Objects.CS.Messages.Attribute + "SPFREIGHT") as PXFieldState;
                var shipInsuname = Base.Document.Cache.GetValueExt(Base.Document.Current, PX.Objects.CS.Messages.Attribute + "SPINSUR") as PXFieldState;
                try
                {
                    sumUsrTotalCartons = sumUsrTotalCartons + decimal.Parse((string)shipFreight?.Value ?? "0") + decimal.Parse((string)shipInsuname?.Value ?? "0");
                }
                catch
                {
                    // 轉型失敗不做任何事
                }
                var _reportID = "LM642010";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["ShipmentNbr"] = Base.Document.Current.ShipmentNbr;
                parameters["TotalValueEn"] = Number2English(Convert.ToDecimal(sumUsrTotalCartons));
                throw new PXReportRequiredException(parameters, _reportID, $"Report {_reportID}") { Mode = PXBaseRedirectException.WindowMode.New };
            }
            return adapter.Get();
        }
        #endregion

        #region Other Custom Method

        #region Number2English mothod
        public string Number2English(decimal num)
        {
            string nu = num.ToString("#00.00");
            string dollars = "";
            string cents = "";
            string tp = "";
            string[] temp;
            string[] tx = { "", "THOUSAND", "MILLION", "BILLION", "TRILLION" };

            if (decimal.Parse(nu) == 0) return "ZERO";
            else if (decimal.Parse(nu) <= 0) return "ERROR!! ";
            else
            { //處理小數點(通常是兩位)
                temp = nu.Split('.');
                string strx = temp[1].ToString();

                string cent = GetEnglish(strx);
                if (!cent.Equals("")) cents = cent + "CENTS";
            }

            //處理整數部分
            //先將資料格式化，只取出整數
            decimal x = Math.Truncate(decimal.Parse(nu));
            //格式化整數部分
            temp = x.ToString("#,0").Split(',');
            //利用整數,號檢查千、萬、百萬....
            int j = temp.Length - 1;

            for (int i = 0; i < temp.Length; i++)
            {
                tp = tp + GetEnglish(temp[i]);
                if (tp != "")
                {
                    tp = tp + tx[j] + " ";
                }
                else
                {
                    tp = tp + " ";
                }

                j = j - 1;
            }
            if (x == 0 && cents != "") // 如果整數部位= 0 ，且有小數
            {
                dollars = "ZERO AND" + cents + " ONLY.";
            }
            else
            {
                if (cents == "")
                {
                    dollars = tp + "DOLLARS ONLY.";
                }
                else
                {
                    dollars = tp + "DOLLARS AND" + cents + " ONLY.";
                }
            }
            return "SAY TOTAL US DOLLARS " + dollars.Replace("DOLLARS", "").Replace("ONLY.", "ONLY").Replace("  ", " ");
        }
        private string GetEnglish(string nu)
        {
            string x = "";
            string str1;
            string str2;
            string[] tr = { "", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
            string[] ty = { "", "", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

            //處理百位數
            str1 = tr[int.Parse(nu) / 100] + " HUNDRED"; //EX  當315除以100 用於int 會取出3 ..對應到字串陣列，就是 Three
            if (str1.Equals(" HUNDRED")) str1 = ""; //如果結果是空值，表示沒有百分位
            //處理十位數
            int temp = int.Parse(nu) % 100;   //  當315 除100 會剩餘 15 

            if (temp < 20)
            {
                str2 = tr[temp]; //取字串陣列 
            }
            else
            {
                str2 = ty[(temp / 10)].ToString();  //十位數  10/20/30的數量確認 

                if (str2.Equals(""))
                {
                    str2 = tr[(temp % 10)];
                }
                else
                {
                    str2 = str2 + "-" + tr[(temp % 10)];  //十位數組成
                }

            }
            if (str1 == "" && str2 == "")
            {
                x = "";
            }
            else
            {
                x = str1 + " " + str2 + " ";
            }

            return x;
        }
        #endregion

        #endregion
    }
}
