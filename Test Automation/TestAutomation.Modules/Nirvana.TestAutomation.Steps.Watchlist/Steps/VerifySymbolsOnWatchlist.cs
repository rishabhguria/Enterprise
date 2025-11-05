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
    class VerifySymbolsOnWatchlist : WatchlistUIMap, ITestStep
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
                List<String> errors = InputEnter(testData.Tables[0]);
                if (errors.Count > 0)
                    _result.ErrorMessage = String.Join("\n\r", errors);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifySymbolsOnWatchlist");
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
        private List<String> InputEnter(DataTable dTable)
        {
            try
            {
                uiUltraGrid1.Click(MouseButtons.Left);
                //var msaaObj = uiUltraGrid1.MsaaObject;
                DataTable dtWatchList = CSVHelper.CSVAsDataTable(this.uiUltraGrid1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                dtWatchList = DataUtilities.RemoveCommas(dtWatchList);
                List<String> columns = new List<string>();
                columns.Add("Symbol");
                List<String> errors = Recon.RunRecon(dtWatchList, dTable, columns, 0.01);
                return errors;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
