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
     class SelectOpeningBalanceTransaction : CashJournalUIMap, ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet no.</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
         {
             TestResult _res = new TestResult();
            try
            {
                OpenCashJournal();
                OpenBalanceTab();
                MatchingData(testData, sheetIndexToName[0]);
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
        /// Get matching data row
        /// </summary>
        /// <param name="testData">The sheet no.</param>
        /// <param name="sheetName"></param>
        /// <param name="DataGrid"></param>
        private void MatchingData(DataSet testData, string sheetName)
        {
            try
            {
                DataTable dtGridData = CSVHelper.CSVAsDataTable(GrdOpeningBalance.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataTable tableData = testData.Tables[sheetName];
                DataRow[] matchedRows = DataUtilities.GetMatchingSingleDataRows(dtGridData, tableData); // Calling GetMatchingMultipleDataRows
                if (tableData.Rows.Count == matchedRows.Length)
                {
                    SelectTransaction(matchedRows, dtGridData, GrdOpeningBalance);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Open Balance Tab.
        /// </summary>
        public void OpenBalanceTab()
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
