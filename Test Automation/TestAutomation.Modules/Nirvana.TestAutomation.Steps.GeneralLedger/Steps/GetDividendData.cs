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

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    class GetDividendData : CashJournalUIMap, ITestStep
    {
        /// <summary>
        /// Run the test
        /// </summary>
        /// <param name="testData">Contains sheet data.</param>
        /// <param name="sheetIndexToName">The sheet no.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenDividendTab();
                GetDividend(testData, sheetIndexToName);
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
        /// Get Dividend Data
        /// </summary>
        /// <param name="testData">Contains sheet data.</param>
        /// <param name="sheetIndexToName">The sheet No.</param>
        private void GetDividend(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataTable inputData = testData.Tables[sheetIndexToName[0]];
                if (inputData.Rows.Count > 0)
                {
                    if (inputData.Rows[0][TestDataConstants.COL_MASTER_FUND] != null && !string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_MASTER_FUND].ToString()))
                    {
                        String masterFundData1 = inputData.Rows[0][TestDataConstants.COL_MASTER_FUND].ToString();
                        String[] listItem1 = masterFundData1.Split(',');
                        List<String> masterFundItem = new List<string>();
                        foreach (String str in listItem1)
                        {
                            masterFundItem.Add(str);
                        }
                        CmbMasterFund.Click(MouseButtons.Left);
                        ExtentionMethods.SelectMultipleItemsFromCombo(masterFundItem, CmbMasterFund);
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
                        CmbMultiAccounts.Click(MouseButtons.Left);
                        ExtentionMethods.SelectMultipleItemsFromCombo(accountListItem, CmbMultiAccounts);
                    }

                    String date = string.Empty;
                    if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_FROM].ToString()))
                    {
                        date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(inputData.Rows[0][TestDataConstants.COL_FROM].ToString()));
                        DtPickerlower.Click(MouseButtons.Left);
                        DtPickerlower.Properties["Text"] = date;
                        //ExtentionMethods.CheckCellValueConditions(date, string.Empty, true);
                    }
                    if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_TO].ToString()))
                    {
                        date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(inputData.Rows[0][TestDataConstants.COL_TO].ToString()));
                        DtPickerUpper.Click(MouseButtons.Left);
                        DtPickerUpper.Properties["Text"] = date;
                        //ExtentionMethods.CheckCellValueConditions(date, string.Empty, true);
                    }
                    BtnGetCash.Click(MouseButtons.Left);
                    ExtentionMethods.WaitForEnabled(ref BtnGetCash, TestDataConstants.GL_DATA_FETCHING_TIME);
                }
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
