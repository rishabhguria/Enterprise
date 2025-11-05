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
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    /// <summary>
    /// Verifies revaluation data but date columns could not be verified yet
    /// </summary>
    class VerifyRevaluationData: CashJournalUIMap, ITestStep
    {
        /// <summary>
        /// Method to run test for verifying revaluation data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenRevaluation();
                BtnGetCash.Click(MouseButtons.Left);
                Wait(5000);
                ExtentionMethods.WaitForEnabled(ref BtnGetCash, TestDataConstants.GL_DATA_FETCHING_TIME);
                if (RevaluationPopUp.IsVisible)
                {
                    ButtonOK2.Click(MouseButtons.Left);
                    BtnGetCash.Click(MouseButtons.Left);
                }
                StringBuilder cashErrors = new StringBuilder(string.Empty);
                List<String> errors = VerifyTransactionsData(testData, sheetIndexToName[0], GrdRevaluation);
                if (errors.Count > 0)
                    cashErrors.Append("RevaluationTabErrors:-" + String.Join("\n\r", errors));
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
        /// Open revaluation tab in the cash journal 
        /// </summary>
        private void OpenRevaluation()
        {
            try
            {
                OpenCashJournal();
                Revaluation.Click(MouseButtons.Left);
            }
            catch
            {                
                throw;
            }
        }

        /// <summary>
        /// Verifies revaluation data
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
                errors = Recon.RunRecon(dtCashGrid, subset, columns, 0.1, true, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
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
