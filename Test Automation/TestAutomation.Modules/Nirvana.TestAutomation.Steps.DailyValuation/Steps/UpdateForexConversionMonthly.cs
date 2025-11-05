using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    class UpdateForexConversionMonthly : ForexConversionUIMap,ITestStep
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
                OpenForexConversion();
                UpdateForexConversionData(testData,sheetIndexToName);
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
        /// Update Forex Conversion Data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void UpdateForexConversionData(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataTable dtForexConversion = CSVHelper.CSVAsDataTable(GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();
                var msaaObj = GrdPivotDisplay.MsaaObject;
                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                     DataRow[] foundRow = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_FROM_CURRENCY], dataRow[TestDataConstants.COL_TO_CURRENCY]));
                    if (foundRow.Length > 0)
                    {
                        int index;
                        index = dtForexConversion.Rows.IndexOf(foundRow[0]);
                        var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                        if (columToMSAAIndexMapping.Count == 0)
                        {
                            columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtForexConversion);
                            SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, dataRow[TestDataConstants.COL_DATE].ToString());
                            columToMSAAIndexMapping.Clear();
                        }
                        else
                        {
                            if (!dataRow[TestDataConstants.COL_FROM_CURRENCY].ToString().Contains(dataRow[TestDataConstants.COL_TO_CURRENCY].ToString()))
                            {
                                AddForexConversionData(testData, sheetIndexToName);
                            }
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
                if (!string.IsNullOrWhiteSpace(dataRow[TestDataConstants.COL_DATE].ToString()))
                {
                    columnName = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(dataRow[TestDataConstants.COL_DATE].ToString()));
                }
                if (!String.IsNullOrEmpty(dataRow["Price"].ToString()))
                {
                    if (columToIndexMapping.ContainsKey(columnName))
                    {
                        int columnIndex = columToIndexMapping[columnName];
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, columnName);
                        gridRow.CachedChildren[columnIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(dataRow["Price"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        
        /// <summary>
        /// Set value in the grid
        /// </summary>
        /// <param name="columToMSAAIndexMapping"></param>
        /// <param name="gridRow"></param>
        /// <param name="datePriceList"></param>
        private void SetValueInGrid(Dictionary<string, int> columToMSAAIndexMapping, MsaaObject gridRow, Dictionary<string, string> datePriceList)
        {
            try
            {
                foreach (string date in datePriceList.Keys)
                {
                    if (!String.IsNullOrEmpty(datePriceList[date]))
                    {
                        if (columToMSAAIndexMapping.ContainsKey(date))
                        {
                            int columnIndex = columToMSAAIndexMapping[date];
                            GrdPivotDisplay.Click(MouseButtons.Left);
                            GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, date);
                            Wait(1000);
                            gridRow.CachedChildren[columnIndex].Click(MouseButtons.Left);
                            if (!gridRow.CachedChildren[columnIndex].Value.ToString().Equals(datePriceList[date]))
                            {
                                Keyboard.SendKeys(datePriceList[date]);
                            }
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
        /// Add Forex Conversion Data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        internal void AddForexConversionData(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataRow dataRow = testData.Tables[0].Rows[0];
                BtnAdd.Click(MouseButtons.Left);
                Wait(1000);
                CmbFromCurrency.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                KeyboardUtilities.PressKey(TestDataConstants.NO_OF_TIMES_BACKSPACE, KeyboardConstants.BACKSPACEKEY);
                Keyboard.SendKeys(dataRow[TestDataConstants.COL_FROM_CURRENCY].ToString());

                CmbToCurrency.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                KeyboardUtilities.PressKey(TestDataConstants.NO_OF_TIMES_BACKSPACE, KeyboardConstants.BACKSPACEKEY);
                Keyboard.SendKeys(dataRow[TestDataConstants.COL_TO_CURRENCY].ToString());

                TxtFxSymbol.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                KeyboardUtilities.PressKey(TestDataConstants.NO_OF_TIMES_BACKSPACE, KeyboardConstants.BACKSPACEKEY);
                Keyboard.SendKeys(dataRow[TestDataConstants.COL_SYMBOL].ToString());

                DataTable dtForexConversion = CSVHelper.CSVAsDataTable(GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] foundRow = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_FROM_CURRENCY], dataRow[TestDataConstants.COL_TO_CURRENCY]));
                DataRow[] foundRow1 = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_TO_CURRENCY], dataRow[TestDataConstants.COL_FROM_CURRENCY]));

                if (String.IsNullOrEmpty(TestDataConstants.COL_SYMBOL.ToString()))
                {
                    throw new System.InvalidOperationException("Add of new pair of currency not done. Symbol is not present in the sheet.");
                }
                if (foundRow.Length > 0 || foundRow1.Length > 0)
                {
                    throw new System.InvalidOperationException("same pair-currency alredy exit or oppsotie, select new pair.");
                }

                BtnAdd1.Click(MouseButtons.Left);
                Wait(1000);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                Wait(3000);
                UpdateAddedForexConversion(testData, dtForexConversion);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Update the added currency pair
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="dtForexConversion"></param>
        private void UpdateAddedForexConversion(DataSet testData, DataTable dtForexConversion)
        {
            try
            {
                int index=-2;
                DataRow dataRow = testData.Tables[0].Rows[0];
                DataTable dtForexConversionUpdated = CSVHelper.CSVAsDataTable(GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] foundRowUpdated = dtForexConversionUpdated.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_FROM_CURRENCY], dataRow[TestDataConstants.COL_TO_CURRENCY]));
      
                if (foundRowUpdated.Length > 0)
                {
                    index = dtForexConversionUpdated.Rows.IndexOf(foundRowUpdated[0]);
                }
     
                GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                var msaaObj = GrdPivotDisplay.MsaaObject;
                var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();
                GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                Dictionary<string, string> datePriceList = new Dictionary<string, string>();
                foreach (DataRow row in testData.Tables[0].Rows)
                {
                    datePriceList.Add(row["Date"].ToString(), row["Price"].ToString());
                }
                if (columToMSAAIndexMapping.Count == 0)
                {
                    columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtForexConversion);
                    SetValueInGrid(columToMSAAIndexMapping,gridRow,datePriceList);
                }

                BtnSave.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
