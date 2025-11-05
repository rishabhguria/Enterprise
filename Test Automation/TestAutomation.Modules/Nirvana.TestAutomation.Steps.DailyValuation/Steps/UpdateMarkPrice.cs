using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    public class UpdateMarkPrice : MarkPriceUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenMarkPriceTab();
                UpdateNewMarkPrices(testData, sheetIndexToName);
                BtnSave.Click(MouseButtons.Left);
                Wait(1000);
                GrdPivotDisplay.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                _result.IsPassed=false;
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
        /// Updates the new mark prices.
        /// </summary>
        /// <param name="testData">The test data.</param>
        private void UpdateNewMarkPrices(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {

                DataTable dtDailyVolatility = CSVHelper.CSVAsDataTable(GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();
                var msaaObj = GrdPivotDisplay.MsaaObject;
                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    DataRow[] foundRow;
                    if (testData.Tables[sheetIndexToName[0]].Columns.Contains(TestDataConstants.COL_Account))
                      foundRow  = dtDailyVolatility.Select(String.Format(@"[" + TestDataConstants.COL_SYMBOL + "]='{0}' AND [" + TestDataConstants.COL_Account + "] ='{1}'", dataRow[TestDataConstants.COL_SYMBOL], dataRow[TestDataConstants.COL_Account]));
                    else
                        foundRow = dtDailyVolatility.Select(String.Format(@"[" + TestDataConstants.COL_SYMBOL + "]='{0}'", dataRow[TestDataConstants.COL_SYMBOL]));
                    if (foundRow.Length > 0)
                    {
                        int index = dtDailyVolatility.Rows.IndexOf(foundRow[0]);
                        var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index + 1);
                        if (columToMSAAIndexMapping.Count == 0)
                        {
                            //TODO: GetColumnIndexMaping() call this method on grid and out of this for loop
                            columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtDailyVolatility);
                            SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, DtDateMonth.Properties["Text"].ToString());

                            // Changes done for adding mark prices in case of single date and multiple symbols.
                            columToMSAAIndexMapping.Clear();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Set value in grid.
        /// </summary>
        /// <param name="columToIndexMapping"></param>
        /// <param name="dataRow"></param>
        /// <param name="gridRow"></param>
        /// <param name="columnName"></param>
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