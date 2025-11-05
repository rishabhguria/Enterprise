using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.UI;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
     class VerifyDailyCalTransactions : DailyCalculationUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
         {
             TestResult _res = new TestResult();
            try
            {
                OpenDailyCalculations();
                BtnGetData.Click(MouseButtons.Left);
                ExtentionMethods.WaitForEnabled(ref BtnGetData, TestDataConstants.GL_DATA_FETCHING_TIME);

                // Adding wait for 6 seconds, as grid taking time to get data for the multiple months
                Wait(6000);
                StringBuilder cashErrors = new StringBuilder(string.Empty);
                List<String> errors = VerifyData(testData, sheetIndexToName[0], UgCalculatedTransactions);
                if (errors.Count > 0)
                    cashErrors.Append("DailyCalculationsTabErrors:-" + String.Join("\n\r", errors));
                if (!string.IsNullOrEmpty(cashErrors.ToString()))
                {
                    _res.ErrorMessage = cashErrors.ToString();
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
        /// Verifies daily calculations data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetName"></param>
        /// <param name="TransactionsGrid"></param>
        /// <returns></returns>
        private List<String> VerifyData(DataSet testData, string sheetName, UIUltraGrid UgCalculatedTransactions)
        {
            try
            {
                DataTable subset = testData.Tables[sheetName];
                List<String> colList = new List<String>();
                for (int i = 0; i < subset.Columns.Count; i++)
                {
                    colList.Add(subset.Columns[i].ColumnName);
                }
                UgCalculatedTransactions.Click(MouseButtons.Left);
                UgCalculatedTransactions.InvokeMethod("AddColumnsToGrid", colList);
                UgCalculatedTransactions.InvokeMethod("RemoveGrouping", null);                
                DataTable dtCashGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(UgCalculatedTransactions.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));                
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
        /// get key columns for comparing rows in cash journal
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeyColumnsForCashJournal()
        {
            List<string> columnList = new List<string>();
            try
            {
                columnList.Add(TestDataConstants.COL_ACCOUNT);
                columnList.Add(TestDataConstants.COL_SYMBOL);
                columnList.Add(TestDataConstants.COL_CURRENCY);
            }
            catch (Exception)
            {
                throw;
            }
            return columnList;
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
