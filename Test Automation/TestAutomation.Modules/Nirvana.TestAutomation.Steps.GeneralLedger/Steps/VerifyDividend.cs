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
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    class VerifyDividend : CashJournalUIMap, ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData">contains sheet data.</param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenDividendTab();
                BtnGetCash.Click(MouseButtons.Left);
                ExtentionMethods.WaitForEnabled(ref BtnGetCash, TestDataConstants.GL_DATA_FETCHING_TIME);
                StringBuilder activityError = new StringBuilder(String.Empty);
                List<String> error = VerifyDividendOperation(testData, sheetIndexToName[0]);
                if (error.Count > 0)
                    activityError.Append("Errors:-" + String.Join("\n\r", error));
                if (!string.IsNullOrEmpty(activityError.ToString()))
                    _res.ErrorMessage = activityError.ToString();
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally 
            {
                MinimizeGeneralLedger();
            }
            return _res;
        }

        /// <summary>
        /// Verify Dividend tab.
        /// </summary>
        /// <param name="testData">Contains test data </param>
        /// <param name="sheetName"></param>
        public List<String> VerifyDividendOperation(DataSet testData, string sheetName)
        {
            try
            {
                DataTable subset = testData.Tables[sheetName];
                List<String> colList = new List<String>();
                for (int i = 0; i < subset.Columns.Count; i++)
                {
                    colList.Add(subset.Columns[i].ColumnName);
                }
                GrdDividend.Click(MouseButtons.Left);
                GrdDividend.InvokeMethod("AddColumnsToGrid", colList);
                GrdDividend.InvokeMethod("RemoveGrouping", null);                
                DataTable dtCashGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(GrdDividend.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));                
                List<String> columns = GetKeyColumnsForCashJournal();
                List<String> errors = new List<String>();
                errors = Recon.RunRecon(dtCashGrid, subset, columns, 0.01);
                return errors;
            }
            catch (Exception)
            {
                throw;
            } 
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
