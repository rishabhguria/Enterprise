using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using System.Data;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    /// <summary>
    /// Gets the Account details
    /// </summary>
    class GetAccountDetails: ChartOfCashAccountsUIMap, ITestStep
    {
        /// <summary>
        /// Runs the retrieval of account details
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAccountDetails();
                DataTable sheet = testData.Tables[sheetIndexToName[0]];
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    EnterAccountFields(sheet);
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
        /// Enters selection for different field values for accounts
        /// </summary>
        /// <param name="coList"></param>
        /// <param name="sheetRows"></param>
        private void EnterAccountFields(DataTable sheet)
        {
            try
            {
                if (sheet.Columns.Contains(TestDataConstants.COL_ACCOUNT))
                {
                    List<string> selectItems = new List<string>();
                    String account = sheet.Rows[0][TestDataConstants.COL_ACCOUNT].ToString();
                    if (!string.IsNullOrWhiteSpace(account))
                    {
                        CmbAccounts.Click(MouseButtons.Left);
                        CmbAccounts.Properties["Text"] = account;
                    }
                }
                if (sheet.Columns.Contains(TestDataConstants.COL_FROM_DATE))
                {

                    String fromDate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(sheet.Rows[0][TestDataConstants.COL_FROM_DATE].ToString()))
                    {
                        fromDate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(sheet.Rows[0][TestDataConstants.COL_FROM_DATE].ToString()));
                        UdtFromDate.Click(MouseButtons.Left);
                        UdtFromDate.Properties["Text"] = fromDate;
                        //ExtentionMethods.CheckCellValueConditions(fromDate, string.Empty, true);
                    }
                }
                if (sheet.Columns.Contains(TestDataConstants.COL_TO_DATE))
                {
                    String toDate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(sheet.Rows[0][TestDataConstants.COL_TO_DATE].ToString()))
                    {
                        toDate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(sheet.Rows[0][TestDataConstants.COL_TO_DATE].ToString()));
                        UdtToDate.Click(MouseButtons.Left);
                        UdtToDate.Properties["Text"] = toDate;
                        //ExtentionMethods.CheckCellValueConditions(toDate, string.Empty, true);
                    }
                }
                BtnGetAccountDetails.Click(MouseButtons.Left);
                ExtentionMethods.WaitForEnabled(ref BtnGetAccountDetails, TestDataConstants.GL_DATA_FETCHING_TIME);
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
