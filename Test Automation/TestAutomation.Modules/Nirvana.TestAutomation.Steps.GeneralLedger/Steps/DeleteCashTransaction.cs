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
    /// Deletes the matched cash transaction data
    /// </summary>
    class DeleteCashTransaction: CashTransactionsUIMap,ITestStep
    {
        /// <summary>
        /// To delete the matched cash transaction data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenCashTransactions();
                GetMatchedData(testData, sheetIndexToName[0], GrdCashDividends);
                DeleteTransaction();

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
                if (!NAVLock.IsVisible)
                {
                    MinimizeGeneralLedger();
                }
            }
            return _res;
        }

        /// <summary>
        /// To get the matching data item row
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetName"></param>
        /// <param name="DataGrid"></param>
        public void GetMatchedData(System.Data.DataSet testData, string sheetName, UIUltraGrid GrdNonTradingTransactions)
        {
            try
            {
                DataTable tableData = testData.Tables[sheetName];
                List<String> colList = new List<String>();
                for (int i = 0; i < tableData.Columns.Count; i++)
                {
                    colList.Add(tableData.Columns[i].ColumnName);
                }
                GrdCashDividends.InvokeMethod("AddColumnsToGrid", colList);
                DataTable dtGridData = CSVHelper.CSVAsDataTable(GrdCashDividends.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] matchedRow = DataUtilities.GetMatchingSingleDataRows(dtGridData, tableData);
                if (matchedRow.Length > 0)
                {
                    SelectMatchingCashTransaction(matchedRow[0], dtGridData, GrdNonTradingTransactions);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// selecting the row matched.
        /// </summary>
        /// <param name="matchedRow">The matched rows.</param>
        /// <param name="dtGridData">The grid table.</param>
        public void SelectMatchingCashTransaction(DataRow matchedRow, DataTable dtGridData, UIUltraGrid grid)
        {
            try
            {
                MsaaObject msaaObject = grid.MsaaObject;
                int rowIndex = dtGridData.Rows.IndexOf(matchedRow);
                var Row = msaaObject.CachedChildren[0].CachedChildren[rowIndex + 1];
                grid.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, rowIndex);
                grid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN,0);
                Row.CachedChildren[1].Click(MouseButtons.Left);
                Row.CachedChildren[1].Click(MouseButtons.Right);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To delete transaction matching the record
        /// </summary>
        private void DeleteTransaction()
        {
            try
            {
                Delete.Click(MouseButtons.Left);
                BtnSave.Click(MouseButtons.Left);
            }
            catch
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
