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
    class GetDayEndCash : DayEndCashUIMap, ITestStep
    {
        /// <summary>
        /// To get data in the Day End Cash
        /// </summary>
        /// <param name="testData">Test case data</param>
        /// <param name="sheetIndexToName">Sheet name of step</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenDayEndCash();
                DataTable sheet = testData.Tables[sheetIndexToName[0]];
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    EnterDayEndCashFields(sheet);
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
        /// <param name="sheet">The test case sheet</param>
        private void EnterDayEndCashFields(DataTable sheet)
        {
            try
            {
                if (sheet.Columns.Contains(TestDataConstants.COL_MASTER_FUND))
                {
                    List<string> selectItems = new List<string>();
                    String masterFund = string.Empty;
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
                    String accountData = sheet.Rows[0][TestDataConstants.COL_ACCOUNTS].ToString();
                    String[] listItem2 = accountData.Split(',');
                    List<String> accountListItem = new List<string>();
                    foreach (String str in listItem2)
                    {
                        string s = str.Trim();
                        accountListItem.Add(s);
                    }
                    CmbMultiAccounts.Click(MouseButtons.Left);
                    ExtentionMethods.SelectMultipleItemsFromCombo(accountListItem, CmbMultiAccounts);
                }
                if (sheet.Columns.Contains(TestDataConstants.COL_FROM_DATE))
                {
                    String fromDate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(sheet.Rows[0][TestDataConstants.COL_FROM_DATE].ToString()))
                    {
                        fromDate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(sheet.Rows[0][TestDataConstants.COL_FROM_DATE].ToString()));
                        DtFromDate.Click(MouseButtons.Left);
                        DtFromDate.Properties["Text"] = fromDate;
                        //ExtentionMethods.CheckCellValueConditions(fromDate, string.Empty, true);
                    }
                }
                if (sheet.Columns.Contains(TestDataConstants.COL_TO_DATE))
                {
                    String toDate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(sheet.Rows[0][TestDataConstants.COL_TO_DATE].ToString()))
                    {
                        toDate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(sheet.Rows[0][TestDataConstants.COL_TO_DATE].ToString()));
                        DtToDate.Click(MouseButtons.Left);
                        DtToDate.Properties["Text"] = toDate;
                        //ExtentionMethods.CheckCellValueConditions(toDate, string.Empty, true);
                    }
                }
                BtnGet.Click(MouseButtons.Left);
                ExtentionMethods.WaitForEnabled(ref BtnGet, TestDataConstants.GL_DATA_FETCHING_TIME);
                //BtnSave.Click(MouseButtons.Left);
            }
            catch (Exception) { throw; }
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
