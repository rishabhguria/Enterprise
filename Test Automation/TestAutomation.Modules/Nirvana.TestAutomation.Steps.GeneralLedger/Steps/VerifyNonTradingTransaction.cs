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
    /// <summary>
    /// Verifies non trading transactions data but date columns could not be verified yet
    /// </summary>
    class VerifyNonTradingTransaction: CashJournalUIMap, ITestStep
    {
        /// <summary>
        /// Method to run test for verifying non trading transactions data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenNonTradingTransaction();
                BtnGetCash.Click(MouseButtons.Left);
                ExtentionMethods.WaitForEnabled(ref BtnGetCash, TestDataConstants.GL_DATA_FETCHING_TIME);
                StringBuilder cashErrors = new StringBuilder(string.Empty);
                List<String> errors = VerifyNonTradingTransactionsData(testData, sheetIndexToName[0], GrdNonTradingTransactions);
                if (errors.Count > 0)
                    cashErrors.Append("NonTradingTransactionsTabErrors:-" + String.Join("\n\r", errors));
                if (!string.IsNullOrEmpty(cashErrors.ToString()))
                {
                    _res.ErrorMessage=cashErrors.ToString();
                }
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
        /// Opens non trading transaction tab in Cash journal
        /// </summary>
        private void OpenNonTradingTransaction()
        {
            try
            {
                OpenCashJournal();
                NonTradingTransaction.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Verifies non trading transactions data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetName"></param>
        /// <param name="TransactionsGrid"></param>
        /// <returns></returns>
        private List<String> VerifyNonTradingTransactionsData(DataSet testData, string sheetName, UIUltraGrid CashGrid)
        {
            try
            {
                DataTable subset = testData.Tables[sheetName];
                List<String> colList = new List<String>();
                for (int i = 0; i < subset.Columns.Count; i++)
                {
                    colList.Add(subset.Columns[i].ColumnName);
                }
                CashGrid.InvokeMethod("AddColumnsToGrid", colList);
                CashGrid.InvokeMethod("RemoveGrouping", null);                
                DataTable dtCashGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(CashGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));                
                List<String> columns = new List<String>();
                columns = GetKeyColumnsForCashJournal();
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
