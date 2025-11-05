using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Watchlist
{
    public class DeleteSymbolFromWatchlist : WatchlistUIMap, ITestStep
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
                OpenWatchList();
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputEnter(dr);
                    }
                }
                uiUltraGrid1.Click(MouseButtons.Right);
                DeleteSymbol.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "DeleteSymbolFromWatchlist");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseWatchlist();
            }
            return _result;
        }
        private void InputEnter(DataRow dr)
        {
            try
            {
                uiUltraGrid1.Click(MouseButtons.Left);
                var msaaObj = uiUltraGrid1.MsaaObject;
                DataTable dtWatchList = CSVHelper.CSVAsDataTable(this.uiUltraGrid1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtWatchList), dr);
                int index = dtWatchList.Rows.IndexOf(dtRow);
                uiUltraGrid1.InvokeMethod("ScrollToRow", index);
                msaaObj.CachedChildren[1].CachedChildren[index + 1].CachedChildren[0].Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
