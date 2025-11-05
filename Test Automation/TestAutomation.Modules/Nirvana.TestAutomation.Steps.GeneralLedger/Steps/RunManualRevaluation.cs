using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Data;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    /// <summary>
    /// To run revaluation step
    /// </summary>
    class RunManualRevaluation: JournalExceptionsUIMap,ITestStep
    {
        /// <summary>
        /// Runs the runmanualrevaluation function
        /// </summary>
        /// /// <param name="testData">Test case data</param>
        /// <param name="sheetIndexToName">Sheet name of step</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            OpenJournalExceptions();
            TestResult _res = new TestResult();
            try
            {
                DataTable inputData = testData.Tables[sheetIndexToName[0]];
                if (inputData.Rows.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_MASTER_FUND].ToString()))
                    {
                        String masterFundData = inputData.Rows[0][TestDataConstants.COL_MASTER_FUND].ToString();
                        String[] listItem1 = masterFundData.Split(',');
                        List<String> masterFundItem = new List<string>();
                        foreach (String str in listItem1)
                        {
                            masterFundItem.Add(str);
                        }
                        CmbMasterFund.Click(MouseButtons.Left);
                        ExtentionMethods.SelectMultipleItemsFromCombo(masterFundItem, CmbMasterFund);
                    }
                    if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_ACCOUNTS].ToString()))
                    {
                        String accountData = inputData.Rows[0][TestDataConstants.COL_ACCOUNTS].ToString();
                        String[] listItem2 = accountData.Split(',');
                        List<String> accountListItem = new List<string>();
                        foreach (String str in listItem2)
                        {
                            accountListItem.Add(str);
                        }
                        CmbMultiAccounts.Click(MouseButtons.Left);
                        ExtentionMethods.SelectMultipleItemsFromCombo(accountListItem, CmbMultiAccounts, false);
                    }

                    String date = string.Empty;

                    if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COLUMN_FROM].ToString()))
                    {
                        date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(inputData.Rows[0][TestDataConstants.COLUMN_FROM].ToString()));
                        DtFromDate.Click(MouseButtons.Left);
                        DtFromDate.Properties["Text"] = date;

                    }

                    if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COLUMN_TO].ToString()))
                    {
                        date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(inputData.Rows[0][TestDataConstants.COLUMN_TO].ToString()));
                        DtToDate.Click(MouseButtons.Left);
                        DtToDate.Properties["Text"] = date;

                    }
                    
                    BtnRunRevaluation.Click(MouseButtons.Left);
                    Wait(100); //when trying to use waitforvisible() then the exception was coming has to add manual Wait
                    if (Revaluation.IsVisible)
                    {
                        ButtonYes.Click(MouseButtons.Left);
                    }
                    ExtentionMethods.WaitForEnabled(ref BtnRunRevaluation, TestDataConstants.GL_DATA_FETCHING_TIME);
                }

            }
            catch (Exception ex)
            {
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
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch 
            {
                throw;
            }
        }
    }
}
