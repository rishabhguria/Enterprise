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
    class GetDailyCalculationsData: DailyCalculationUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenDailyCalculations();
                DataTable sheet = testData.Tables[sheetIndexToName[0]];
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    EnterNonTradingTransactionFields(sheet);
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
        /// Enters selection for different field values for transactions
        /// </summary>
        /// <param name="sheet"></param>
        private void EnterNonTradingTransactionFields(DataTable sheet)
        {
            try
            {
                if (sheet.Columns.Contains(TestDataConstants.COL_MASTER_FUND))
                {
                    List<string> selectItems = new List<string>();
                    String masterFund = string.Empty;
                    if (!string.IsNullOrWhiteSpace(sheet.Rows[0][TestDataConstants.COL_MASTER_FUND].ToString()))
                        masterFund = sheet.Rows[0][TestDataConstants.COL_MASTER_FUND].ToString();
                    string[] fundsList = masterFund.Split(',');
                    foreach (string fund in fundsList)
                    {
                        masterFund = fund.Trim();
                        selectItems.Add(masterFund);
                    }
                    CmbMasterFund.Click(MouseButtons.Left);
                    ExtentionMethods.SelectMultipleItemsFromCombo(selectItems, CmbMasterFund);

                }
                if (sheet.Columns.Contains(TestDataConstants.COL_ACCOUNTS))
                {
                    List<string> selectItems = new List<string>();
                    String account = string.Empty;
                    if (!string.IsNullOrWhiteSpace(sheet.Rows[0][TestDataConstants.COL_ACCOUNTS].ToString()))
                        account = sheet.Rows[0][TestDataConstants.COL_ACCOUNTS].ToString();
                    string[] accountList = account.Split(',');
                    foreach (string acc in accountList)
                    {
                        account = acc.Trim();
                        selectItems.Add(account);
                    }

                    CmbMultiAccounts.Click(MouseButtons.Left);
                    ExtentionMethods.SelectMultipleItemsFromCombo(selectItems, CmbMultiAccounts);

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
                BtnGetData.Click(MouseButtons.Left);
                ExtentionMethods.WaitForEnabled(ref BtnGetData, TestDataConstants.GL_DATA_FETCHING_TIME);
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
