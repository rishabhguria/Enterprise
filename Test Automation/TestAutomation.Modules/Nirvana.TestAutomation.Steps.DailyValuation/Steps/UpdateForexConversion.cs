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
    public class UpdateForexConversion : ForexConversionUIMap , ITestStep
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
                UpdateForexConversionData(testData, sheetIndexToName);
                BtnSave.Click(MouseButtons.Left);
                Wait(2000);
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
                Wait(1000);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }
            return _result;
        }

        /// <summary>
        /// Update forex conversion data.
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
                    DataRow[] foundRow;
                    DataRow[] foundRow1;
                    if (testData.Tables[sheetIndexToName[0]].Columns.Contains(TestDataConstants.COL_Account))
                    {
                        foundRow = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}' AND [" + TestDataConstants.COL_Account + "] ='{2}'", dataRow[TestDataConstants.COL_FROM_CURRENCY], dataRow[TestDataConstants.COL_TO_CURRENCY], dataRow[TestDataConstants.COL_Account]));
                        foundRow1 = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}' AND [" + TestDataConstants.COL_Account + "] ='{2}'", dataRow[TestDataConstants.COL_TO_CURRENCY], dataRow[TestDataConstants.COL_FROM_CURRENCY], dataRow[TestDataConstants.COL_Account]));
                    }
                    else
                    {
                        foundRow = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_FROM_CURRENCY], dataRow[TestDataConstants.COL_TO_CURRENCY]));
                        foundRow1 = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_TO_CURRENCY], dataRow[TestDataConstants.COL_FROM_CURRENCY]));
                    }
                    if (foundRow.Length > 0 || foundRow1.Length > 0)
                    {
                        int index;
                        if (foundRow.Length > 0)
                        {
                             index = dtForexConversion.Rows.IndexOf(foundRow[0]);
                        }
                        else
                        {
                            index = dtForexConversion.Rows.IndexOf(foundRow1[0]);
 
                        }
                        var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                        if (columToMSAAIndexMapping.Count == 0)
                        {
                            columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtForexConversion);
                        }
                        SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, DtDateMonth.Properties["Text"].ToString());
                    }                    
                    else
                    {
                        if (!dataRow[TestDataConstants.COL_FROM_CURRENCY].ToString().Contains(dataRow[TestDataConstants.COL_TO_CURRENCY].ToString()))
                        {
                            AddForexConversionData(dataRow, sheetIndexToName);
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
        /// Set value in the grid.
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
                        if (!gridRow.CachedChildren[columnIndex].Value.ToString().Equals(dataRow["Price"].ToString()))
                        {
                            Keyboard.SendKeys(dataRow["Price"].ToString());
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
        /// Add forex conversion data.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet no.</param>
        private void AddForexConversionData(DataRow dataRow, Dictionary<int, string> sheetIndexToName)
        {
            try
            {              
                               
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
                    Wait(3000);
                    UpdateAddedForexConversion(dataRow, dtForexConversion);
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Update added pair of Currency.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="dtForexConversion"></param>
        private void UpdateAddedForexConversion(DataRow dataRow, DataTable dtForexConversion)
        {
            try
            {
                int index;
                DataTable dtForexConversionUpdated = CSVHelper.CSVAsDataTable(GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] foundRowUpdated = dtForexConversionUpdated.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_FROM_CURRENCY], dataRow[TestDataConstants.COL_TO_CURRENCY]));
                DataRow[] foundRow1Updated = dtForexConversionUpdated.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_TO_CURRENCY], dataRow[TestDataConstants.COL_FROM_CURRENCY]));
                if (foundRowUpdated.Length > 0)
                {
                    index = dtForexConversionUpdated.Rows.IndexOf(foundRowUpdated[0]);
                }
                else
                {
                    index = dtForexConversionUpdated.Rows.IndexOf(foundRow1Updated[0]);

                }
                GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                var msaaObj = GrdPivotDisplay.MsaaObject;
                var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();
                GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                if (columToMSAAIndexMapping.Count == 0)
                {
                    columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtForexConversion);
                    SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, DtDateMonth.Properties["Text"].ToString());
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