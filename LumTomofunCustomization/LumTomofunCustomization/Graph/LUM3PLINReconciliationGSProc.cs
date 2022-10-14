using PX.Data;
using PX.Objects.IN;
using System;
using System.Net.Http;
using System.Collections.Generic;
using LUMTomofunCustomization.DAC;
using LUMTomofunCustomization.Graph;
using LumTomofunCustomization.API_Helper;
using LumTomofunCustomization.API_Entity;

namespace LumTomofunCustomization.Graph
{
    public class LUM3PLINReconciliationGSProc :PXGraph<LUM3PLINReconciliationGSProc>
    {
        #region Features & Selects
        public PXCancel<LUM3PLINReconciliation> Cancel;

        public PXProcessing<LUM3PLINReconciliation, Where<LUM3PLINReconciliation.thirdPLType, Equal<ThirdPLType.googleSheets>>, OrderBy<Desc<LUM3PLINReconciliation.tranDate>>> GSheetsReconciliation;

        public PXSetup<LUM3PLSetup> Setup;
        #endregion

        #region Ctor
        public LUM3PLINReconciliationGSProc()
        {
            if (GSheetsReconciliation.Select().Count == 0) { LUM3PLINReconciliationAPIProc.InsertInitializedData(this, ThirdPLType.GoogleSheets); }

            GSheetsReconciliation.SetProcessAllCaption("Import GS IN");
            GSheetsReconciliation.SetProcessVisible(false);
            GSheetsReconciliation.SetProcessDelegate(delegate (List<LUM3PLINReconciliation> lists)
            {
                ImportRecords(lists);
            });
        }
        #endregion

        #region Static Methods
        public static void ImportRecords(List<LUM3PLINReconciliation> lists)
        {
            LUM3PLINReconciliationGSProc graph = CreateInstance<LUM3PLINReconciliationGSProc>();

            graph.ImportGSRecords();
        }
        #endregion

        #region Methods
        public virtual void ImportGSRecords()
        {
            LUM3PLSetup setup = Setup.Select();

            try
            {
                LUM3PLINReconciliationAPIProc.DeleteDataByScript(true);
                CreateDateFromGSheets(setup);
            }
            catch (Exception e)
            {
                PXProcessing<LUM3PLINReconciliation>.SetError(e);
                //throw;
            }

            this.Actions.PressSave();
        }

        public virtual LUMAPIResults GetSpecifiedSheet(LUM3PLSetup setup)
        {
            var helper = new LUMAPIHelper(new LUMAPIConfig()
                                          {
                                              RequestMethod = HttpMethod.Post,
                                              RequestUrl = setup.GoogleSheetsURL
                                          },
                                          null);

            return helper.GetResults(LUMAPIHelper.SerialzeJSONString(new GoogleSheetsEntity.RequestRoot() { SheetName = setup.GoogleSheetName }));
        }

        public virtual void CreateDateFromGSheets(LUM3PLSetup setup)
        {
            var sheets = LUMAPIHelper.DeserializeJSONString<GoogleSheetsEntity.DataRoot>(GetSpecifiedSheet(setup).ContentResult);

            // Since first array is label row.
            for (int i = 1; i < sheets.Data.Count; i++)
            {
                // ["Inventory Date", "Warehouse", "Location", "Sku", "Qty", "Country"]
                var row = sheets.Data[i];

                DateTime invetDate = Convert.ToDateTime(row[0]);

                if (i == 1)
                {
                    LUM3PLINReconciliationAPIProc.DeleteDataByScript(true, invetDate.Date);
                }

                INSite site = INSite.UK.Find(this, Convert.ToString(row[1]));

                LUM3PLINReconciliationAPIProc.Update3PLINReconciliation(GSheetsReconciliation.Cache,
                                                                        GSheetsReconciliation.Insert(new LUM3PLINReconciliation()
                                                                                                     {
                                                                                                         ThirdPLType = ThirdPLType.GoogleSheets,
                                                                                                         TranDate = invetDate,
                                                                                                         INDate = invetDate.Date,
                                                                                                         Sku = Convert.ToString(row[3]),
                                                                                                         ProductName = null,
                                                                                                         Qty = row[4].Equals("") ? 0m : Convert.ToInt32(row[4]),
                                                                                                         DetailedDesc = "SELLABLE",
                                                                                                         CountryID = Convert.ToString(row[5]),
                                                                                                         Warehouse = site?.SiteID,
                                                                                                         Location = PX.Objects.CR.Location.UK.Find(this, site.BAccountID, Convert.ToString(row[2]))?.LocationID
                                                                                                     }));
            }
        }
        #endregion
    }
}
