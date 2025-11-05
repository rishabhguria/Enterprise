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
    /// Selects the non-trading transaction item
    /// </summary>
    class SelectNonTradingTranItem: CashJournalUIMap, ITestStep
    {
        /// <summary>
        /// To select the non trading transaction item
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
                GetMatchingData(testData, sheetIndexToName[0], GrdNonTradingTransactions);
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
        /// <param name="GrdNonTradingTransactions"></param>
        private void GetMatchingData(System.Data.DataSet testData, string sheetName, UIUltraGrid GrdNonTradingTransactions)
        {
            try
            {
                DataTable tableData = testData.Tables[sheetName];
                List<String> colList = new List<String>();
                for (int i = 0; i < tableData.Columns.Count; i++)
                {
                    colList.Add(tableData.Columns[i].ColumnName);
                }
                GrdNonTradingTransactions.InvokeMethod("AddColumnsToGrid", colList);
                GrdNonTradingTransactions.InvokeMethod("RemoveGrouping",null);
                DataTable dtGridData = CSVHelper.CSVAsDataTable(GrdNonTradingTransactions.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] matchedRow = DataUtilities.GetMatchingSingleDataRows(dtGridData, tableData);
                if (matchedRow.Length > 0)
                {
                    SelectTransactionItem(matchedRow[0], dtGridData, GrdNonTradingTransactions);
                }
            }
            catch
            {
                throw;
            }
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
