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
    class DeleteOpeningBalanceTransaction : CashJournalUIMap, ITestStep
    {
       /// <summary>
       /// Run the test.
       /// </summary>
       /// <param name="testData">The test data.</param>
       /// <param name="sheetIndexToName">The sheet no.</param>
       /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenCashJournal();
                OpenOpeningBalanceTab();
                GettingMatchingData(testData, sheetIndexToName[0]);
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
        /// Get the matching data item row
        /// </summary>
        /// <param name="testData">The test data</param>
        /// <param name="sheetName">The sheet name</param>
        /// <param name="DataGrid"></param>
        private void GettingMatchingData(DataSet testData, string sheetName)
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

                if (tableData.Rows.Count == matchedRow.Length)
                {
                    MsaaObject row = SelectTransaction(matchedRow, dtGridData, GrdOpeningBalance);
                    row.CachedChildren[1].Click(MouseButtons.Right);
                    DeleteTransaction.Click(MouseButtons.Left);
                    BtnSave.Click(MouseButtons.Left);
                }
            }
            catch
            {
                throw;
            }          
       }            

        /// <summary>
        /// Opens balance tab.
        /// </summary>
        private void OpenOpeningBalanceTab()
        {
            try
            {
                OpeningBalance.Click(MouseButtons.Left);
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
