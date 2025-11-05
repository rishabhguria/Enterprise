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
using System.Configuration;


namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    class GetAccountBalance : CashJournalUIMap, ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                bool isRunRevalOnGetAccountBalances = false;
                OpenAccountTab();
                GetAccountData(testData, sheetIndexToName, ref isRunRevalOnGetAccountBalances);
                if (OutdatedRevaluation.IsVisible)
                {
                    if (isRunRevalOnGetAccountBalances)
                        ButtonYes.Click(MouseButtons.Left);
                    else
                        ButtonNo.Click(MouseButtons.Left);
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
        /// Get account balance data
        /// </summary>
        /// <param name="testData">Test data.</param>
        /// <param name="sheetIndexToName">The sheet no.</param>
        public void GetAccountData(DataSet testData, Dictionary<int, string> sheetIndexToName, ref bool isRunRevalOnGetAccountBalances)
        {
            try
            {
                DataTable inputData = testData.Tables[sheetIndexToName[0]];
                if (inputData.Rows.Count > 0)
                {
                    if (inputData.Rows[0][TestDataConstants.COL_MASTER_FUND] != null && !string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_MASTER_FUND].ToString()))
                    {
                        String masterFundData = inputData.Rows[0][TestDataConstants.COL_MASTER_FUND].ToString();
                        String[] listItem1 = masterFundData.Split(',');
                        List<String> masterFundItem = new List<string>();
                        foreach (String str in listItem1)
                        {
                            masterFundItem.Add(str);
                        }
                        CmbMasterFund1.Click(MouseButtons.Left);
                        ExtentionMethods.SelectMultipleItemsFromCombo(masterFundItem, CmbMasterFund1);
                    }
                    if (inputData.Rows[0][TestDataConstants.COL_ACCOUNTS] != null && !string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_ACCOUNTS].ToString()))
                    {
                        String accountData = inputData.Rows[0][TestDataConstants.COL_ACCOUNTS].ToString();
                        String[] listItem2 = accountData.Split(',');
                        List<String> accountListItem = new List<string>();
                        foreach (String str in listItem2)
                        {
                            accountListItem.Add(str);
                        }
                        CmbMultiAccounts1.Click(MouseButtons.Left);
                        ExtentionMethods.SelectMultipleItemsFromCombo(accountListItem, CmbMultiAccounts1, false);
                    }

                    String date = string.Empty;

                    if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_BALANCES_AS_ON_DATE].ToString()))
                    {
                        date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(inputData.Rows[0][TestDataConstants.COL_BALANCES_AS_ON_DATE].ToString()));
                        UdtBalanceDate.Click(MouseButtons.Left);
                        UdtBalanceDate.Properties["Text"] = date;
                        //ExtentionMethods.CheckCellValueConditions(date, string.Empty, true);                     
                    }

                    //This particular change is done for checking revaluation entries upto balance as on date inclusive of markprice effect.
                    // In case the COL_IS_RUNREVAL_ON_GETACCOUNTBALANCES is not present in schema then it will not impact any other test cases.
                    if (inputData.Columns.Contains(TestDataConstants.COL_IS_RUNREVAL_ON_GETACCOUNTBALANCES))
                    {
                        if (inputData.Rows[0][TestDataConstants.COL_IS_RUNREVAL_ON_GETACCOUNTBALANCES] != null && !string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_IS_RUNREVAL_ON_GETACCOUNTBALANCES].ToString()))
                        {
                            isRunRevalOnGetAccountBalances = Convert.ToBoolean(inputData.Rows[0][TestDataConstants.COL_IS_RUNREVAL_ON_GETACCOUNTBALANCES]);
                        }
                    }
                    BtnGetAccBalances.Click(MouseButtons.Left);

                    // Changes done for timely calculation of revaluation entries in case excessive data is there.

                    ExtentionMethods.WaitForEnabled(ref BtnGetAccBalances, TestDataConstants.GL_DATA_FETCHING_TIME);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Opening account tab.
        /// </summary>
        public void OpenAccountTab()
        {
            try
            {
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_GL"]);
                ExtentionMethods.WaitForVisible(ref FrmCashManagementMain, 15);
                // GeneralLedger.Click(MouseButtons.Left);
                ChartofCashAccounts.Click(MouseButtons.Left);
                AccountBalances.Click(MouseButtons.Left);
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
