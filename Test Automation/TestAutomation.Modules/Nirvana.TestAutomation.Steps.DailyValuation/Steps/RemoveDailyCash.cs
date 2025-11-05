using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    public class RemoveDailyCash : DailyCashUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenDailyCashTab();
                UpdateDailyCashData(testData, sheetIndexToName);
                BtnSave.Click(MouseButtons.Left);
                Wait(1000);
                GrdPivotDisplay.Click(MouseButtons.Left); 
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                KeyboardUtilities.CloseWindow(ref MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        private void UpdateDailyCashData(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {

                DataTable dtDailyVolatility = CSVHelper.CSVAsDataTable(GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();
                var msaaObj = GrdPivotDisplay.MsaaObject;
                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    DataRow[] foundRow = dtDailyVolatility.Select(String.Format(@"[" + TestDataConstants.COL_ACCOUNT + "]='{0}'", dataRow[TestDataConstants.COL_ACCOUNT]));
                    if (foundRow.Length > 0)
                    {
                        int index = dtDailyVolatility.Rows.IndexOf(foundRow[0]);
                        var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                        if (columToMSAAIndexMapping.Count == 0)
                        {
                            columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtDailyVolatility);
                            RemoveDailyCashAccount(columToMSAAIndexMapping, dataRow, gridRow);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RemoveDailyCashAccount(Dictionary<string, int> columToIndexMapping, DataRow dataRow, MsaaObject gridRow)
        {
            try
            {
                if (!String.IsNullOrEmpty(dataRow[TestDataConstants.COL_ACCOUNT].ToString()))
                {
                    if (columToIndexMapping.ContainsKey(TestDataConstants.COL_ACCOUNT))
                    {
                        int columnIndex = columToIndexMapping[TestDataConstants.COL_ACCOUNT];
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, TestDataConstants.COL_ACCOUNT);
                        Wait(1000);
                        gridRow.CachedChildren[columnIndex].Click(MouseButtons.Left);
                        BtnRemove.Click(MouseButtons.Left);

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
