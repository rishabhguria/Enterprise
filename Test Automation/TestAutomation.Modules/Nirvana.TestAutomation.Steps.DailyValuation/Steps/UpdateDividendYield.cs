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
    
    public class UpdateDividendYield : DailyDividendYieldUIMap, ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet no.</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenDividnedYieldTab();
                UpdateDividendYieldData(testData, sheetIndexToName);
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

        /// <summary>
        /// Update Daily dividend data.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet no.</param>
        private void UpdateDividendYieldData(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataTable dtDailyDividendYield = CSVHelper.CSVAsDataTable(GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();
                var msaaObj = GrdPivotDisplay.MsaaObject;
                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    DataRow[] foundRow = dtDailyDividendYield.Select(String.Format(@"[" + TestDataConstants.COL_SYMBOL + "] ='{0}'", dataRow[TestDataConstants.COL_SYMBOL]));
                    if (foundRow.Length > 0)
                    {
                        int index = dtDailyDividendYield.Rows.IndexOf(foundRow[0]);
                        var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                        if (columToMSAAIndexMapping.Count == 0)
                        {
                            columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtDailyDividendYield);
                            SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, DtDateMonth.Properties["Text"].ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetValueInGrid(Dictionary<string, int> columToIndexMapping, DataRow dataRow, MsaaObject gridRow, string columnName)
        {

            try
            {
                if (!String.IsNullOrEmpty(dataRow["Price"].ToString()))
                {
                    if (columToIndexMapping.ContainsKey(columnName))
                    {
                        int columnIndex = columToIndexMapping[columnName];
                        GrdPivotDisplay.Click(MouseButtons.Left);
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, columnName);
                        Wait(1000);
                        gridRow.CachedChildren[columnIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(dataRow["Price"].ToString());
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
