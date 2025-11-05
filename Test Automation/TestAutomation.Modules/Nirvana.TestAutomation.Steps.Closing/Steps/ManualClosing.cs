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

namespace Nirvana.TestAutomation.Steps.Closing
{
    class ManualClosing : ClosingUIMap, ITestStep
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
                OpenClosingUI();
                ClosedAmend.Click(MouseButtons.Left);
                CmbMethodology.Click(MouseButtons.Left);
                Keyboard.SendKeys(TestDataConstants.COL_MANUAL + KeyboardConstants.ENTERKEY);
                ManualClose(testData, sheetIndexToName);
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
                MinimizeClosing();
            }
            return _result;
        }

        /// <summary>
        /// Manual Close
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private TestResult ManualClose(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                DataTable excelFileData = testData.Tables[sheetIndexToName[0]];
                DataRow[] foundLongGridRows = null;
                DataRow[] foundShortGridRows = null;

                foreach (DataRow dataRow in excelFileData.Rows)
                {
                    DataTable currentDataGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.GrdLong.InvokeMethod(ExcelStructureConstants.COL_GET_ALL_VISIBLE_DATA_FROM_THE_GRID, null).ToString()));
                    GetMatchingDataRows(currentDataGrid, dataRow, ref foundLongGridRows, ref foundShortGridRows);

                    if (Convert.ToBoolean(dataRow[TestDataConstants.COL_CLOSESSW_BUYBTW].ToString()) && !ChkBoxBuyAndBuyToCover.IsChecked)
                        ChkBoxBuyAndBuyToCover.Click(MouseButtons.Left);
                    else if (ChkBoxBuyAndBuyToCover.IsChecked && !Convert.ToBoolean(dataRow[TestDataConstants.COL_CLOSESSW_BUYBTW].ToString()))
                        ChkBoxBuyAndBuyToCover.Click(MouseButtons.Left);

                    if (Convert.ToBoolean(dataRow[TestDataConstants.COL_AUTOCLOSE_STRATEGY].ToString()) && !ChkBxIsAutoCloseStrategy.IsChecked)
                        ChkBxIsAutoCloseStrategy.Click(MouseButtons.Left);
                    else if (ChkBxIsAutoCloseStrategy.IsChecked && !Convert.ToBoolean(dataRow[TestDataConstants.COL_AUTOCLOSE_STRATEGY].ToString()))
                        ChkBxIsAutoCloseStrategy.Click(MouseButtons.Left);

                    int indexLong = currentDataGrid.Rows.IndexOf(foundLongGridRows[0]);
                    int indexShort = currentDataGrid.Rows.IndexOf(foundShortGridRows[0]);
                    
                    var shortrow = GrdShort.MsaaObject.CachedChildren[0].CachedChildren[indexShort + 1];
                    if (GrdLong.MsaaObject.CachedChildren[0].CachedChildren.Count() > indexLong + 1)
                    {
                        var longRow = GrdLong.MsaaObject.CachedChildren[0].CachedChildren[indexLong + 1];

                        GrdLong.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, indexLong);
                        GrdShort.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, indexShort);
                        longRow.Click(MouseButtons.Left);
                        longRow.Click(shortrow);
                    }
                    else
                    {
                        var longRow = GrdLong.MsaaObject.CachedChildren[1].CachedChildren[indexLong + 1];

                        GrdLong.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, indexLong);
                        GrdShort.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, indexShort);
                        longRow.Click(MouseButtons.Left);
                        longRow.Click(shortrow);
                    }

                    if (InformationMsgButtonOK.IsVisible)
                        InformationMsgButtonOK.Click(MouseButtons.Left);
                    if (CloseTradeErrorButtonOK.IsVisible)
                        CloseTradeErrorButtonOK.Click(MouseButtons.Left);
                }
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
              
            }
            return _res;
        }

        /// <summary>
        /// Gets the matching data rows.
        /// </summary>
        /// <param name="masterDataTable">The master data table.</param>
        /// <param name="rowToMatch">The row to match.</param>
        /// <returns></returns>
        public string GetMatchingDataRows(DataTable masterDataTable, DataRow rowToMatch, ref DataRow[] foundLongGridRows, ref DataRow[] foundShortGridRows)
        {
            string errorMessage = string.Empty;
            try
            {
                string shortExpression = string.Empty;
                string longExpression = string.Empty;

                for (int i = 0; i < rowToMatch.Table.Columns.Count; i++)
                {
                    string colValue = rowToMatch.ItemArray[i].ToString();
                    if (!string.IsNullOrWhiteSpace(colValue))
                    {
                        if (colValue.Equals(ExcelStructureConstants.BLANK_CONST))
                            colValue = string.Empty;
                        string check = rowToMatch.Table.Columns[i].Caption.Substring(0, 5);
                        if (check == "Short")
                        {
                            shortExpression = string.IsNullOrWhiteSpace(shortExpression) ? shortExpression : shortExpression + " AND ";
                            shortExpression = shortExpression + "[" + rowToMatch.Table.Columns[i].Caption.Substring(5) + "] = '" + colValue + "' ";
                        }
                        else if (check.Substring(0, 4) == "Long")
                        {
                            longExpression = string.IsNullOrWhiteSpace(longExpression) ? longExpression : longExpression + " AND ";
                            longExpression = longExpression + "[" + rowToMatch.Table.Columns[i].Caption.Substring(4) + "] = '" + colValue + "' ";
                        }
                    }
                }

                foundLongGridRows = masterDataTable.Select(longExpression);
                foundShortGridRows = masterDataTable.Select(shortExpression);
            }

            catch (Exception ex)
            {
				bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }
        
        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
