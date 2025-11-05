using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    class VerifyAccountBalances: ChartOfCashAccountsUIMap,ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAccountBalancesForRevaluation();
                StringBuilder cashErrors = new StringBuilder(string.Empty);
                List<String> errors = VerifyData(testData, sheetIndexToName[0], GrdAccBalances);
                if (errors.Count > 0)
                    cashErrors.Append("Errors:-" + String.Join("\n\r", errors));

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
        private List<String> VerifyData(DataSet testData, string sheetName, UIUltraGrid AccountBalancesGrid)
        {
            try
            {
                DataTable subset = testData.Tables[sheetName];
                List<String> colList = new List<String>();
                for (int i = 0; i < subset.Columns.Count; i++)
                {
                    colList.Add(subset.Columns[i].ColumnName);
                }
                AccountBalancesGrid.Click(MouseButtons.Left);
                AccountBalancesGrid.InvokeMethod("AddColumnsToGrid", colList);
                AccountBalancesGrid.InvokeMethod("RemoveGrouping", null);
                DataTable dtCashGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(AccountBalancesGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));                
                List<String> columns = new List<String>();
                columns = GetKeyColumnsForAccountBalances();
                List<String> errors = new List<String>();
                errors = Recon.RunRecon(dtCashGrid, subset, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
                return errors;
            }
            catch (Exception ex )
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
					throw;
                return null;  
            }
        }

        /// <summary>
        /// get key columns for comparing rows in cash journal
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeyColumnsForAccountBalances()
        {
            List<string> columnList = new List<string>();
            try
            {
                columnList.Add(TestDataConstants.COL_ACCOUNT);
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
