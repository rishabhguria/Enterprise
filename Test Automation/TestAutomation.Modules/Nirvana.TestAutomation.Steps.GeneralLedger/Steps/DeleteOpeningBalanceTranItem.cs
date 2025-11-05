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
    class DeleteOpeningBalanceTranItem : CashJournalUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenCashJournal();
                OpeningBalance.Click(MouseButtons.Left);
                GetingMatchingData(testData, sheetIndexToName[0]);

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
        /// Getting matched data.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetName">The grid table.</param>
        /// <param name="DataGrid"></param>
        private void GetingMatchingData(DataSet testData, string sheetName)
        {
            try
            {
                DataTable tableData = testData.Tables[sheetName];
                List<String> colList = new List<String>();
                for (int i = 0; i < tableData.Columns.Count; i++)
                {
                    colList.Add(tableData.Columns[i].ColumnName);
                }
                GrdOpeningBalance.InvokeMethod("AddColumnsToGrid", colList);
                GrdOpeningBalance.InvokeMethod("RemoveGrouping", null);
                DataTable dtGridData = CSVHelper.CSVAsDataTable(GrdOpeningBalance.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] matchedRow = DataUtilities.GetMatchingSingleDataRows(dtGridData, tableData);
                if (matchedRow.Length > 0)
                {
                    MsaaObject row = SelectTransactionItem(matchedRow[0], dtGridData, GrdOpeningBalance);
                    row.CachedChildren[1].Click(MouseButtons.Right);
                    DeleteTransactionItem.Click(MouseButtons.Left); 
                }
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
