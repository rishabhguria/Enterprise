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
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    /// <summary>
    /// Selects the non-trading transaction
    /// </summary>
    class SelectNonTradingTransaction: CashJournalUIMap, ITestStep
    {
        /// <summary>
        /// To select the non trading transaction
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
                GetMatchingData(testData,sheetIndexToName[0],GrdNonTradingTransactions);
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
        /// To get the matching data item row
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetName"></param>
        /// <param name="DataGrid"></param>
        private string GetMatchingData(System.Data.DataSet testData, string sheetName, UIUltraGrid GrdNonTradingTransactions)
        {
            string errorMessage = string.Empty;
            try
            {
                DataTable dtGridData = CSVHelper.CSVAsDataTable(GrdNonTradingTransactions.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataTable tableData = testData.Tables[sheetName];
                DataRow[] matchedRows = DataUtilities.GetMatchingMultipleDataRows(dtGridData, tableData, errorMessage);
                if (tableData.Rows.Count == matchedRows.Length)
                {
                    SelectTransaction(matchedRows, dtGridData, GrdNonTradingTransactions);
                }
            }
            catch(Exception ex)
            {
				bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }

        /// <summary>
        /// Open non trading transactions tab in the cash journal 
        /// </summary>
        private void OpenNonTradingTransaction()
        {
            try
            {
                OpenCashJournal();
                NonTradingTransaction.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
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
