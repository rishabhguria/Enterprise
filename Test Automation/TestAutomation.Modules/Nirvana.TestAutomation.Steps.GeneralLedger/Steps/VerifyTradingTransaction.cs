using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.Utilities;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    /// <summary>
    /// Verifies transactions data
    /// </summary>
    class VerifyTradingTransaction: CashJournalUIMap, ITestStep
    {
        /// <summary>
        /// Runs the verification for trading transactions
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenCashJournal();
                OpenTradingTransaction();
                StringBuilder cashErrors = new StringBuilder(string.Empty);
                List<String> errors = VerifyTransactionsData(testData, sheetIndexToName[0], GrdTradingTransactions);
                if (errors.Count > 0)
                    cashErrors.Append("TradingTabErrors:-" + String.Join("\n\r", errors));
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
        /// Opens trading transaction tab in Cash journal
        /// </summary>
        private void OpenTradingTransaction()
        {
            try
            {
                TradingTransaction.Click(MouseButtons.Left);
                BtnGetCash.Click(MouseButtons.Left);
                ExtentionMethods.WaitForEnabled(ref BtnGetCash, TestDataConstants.GL_DATA_FETCHING_TIME);
            }
            catch
            {                
                throw;
            }
        }

        /// <summary>
        /// Verifies transactions data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetName"></param>
        /// <param name="CashGrid"></param>
        /// <returns></returns>
        private List<String> VerifyTransactionsData(DataSet testData, string sheetName, UIUltraGrid CashGrid)
        {
            try
            {
                DataTable subset = testData.Tables[sheetName];
                List<String> colList= new List<String>();
                for(int i=0;i< subset.Columns.Count;i++)
                {
                    colList.Add(subset.Columns[i].ColumnName);
                }
                CashGrid.InvokeMethod("AddColumnsToGrid",colList);
                CashGrid.InvokeMethod("RemoveGrouping", null);                
                DataTable dtCashGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(CashGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                List<String> columns = new List<String>();
                columns = GetKeyColumnsForCashJournal();
                List<String> errors = new List<String>();
                errors = Recon.RunRecon(dtCashGrid, subset, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
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
