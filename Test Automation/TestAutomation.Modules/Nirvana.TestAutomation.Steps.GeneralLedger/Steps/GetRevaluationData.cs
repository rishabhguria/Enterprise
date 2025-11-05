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

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    /// <summary>
    /// Gets the revaluation data
    /// </summary>
    class GetRevaluationData: CashJournalUIMap, ITestStep
    {
        /// <summary>
        /// Runs the retrieval of revaluation data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenCashJournal();
                OpenRevaluation();
                DataTable sheet = testData.Tables[sheetIndexToName[0]];
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    EnterRevaluationFields(sheet);
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
        /// Enters selection for different field values for retrieving transactions in revaluation
        /// </summary>
        /// <param name="coList"></param>
        /// <param name="sheetRows"></param>
        private void EnterRevaluationFields(DataTable sheet)
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
                        DtPickerlower.Click(MouseButtons.Left);
                        DtPickerlower.Properties["Text"] = fromDate;
                        //ExtentionMethods.CheckCellValueConditions(fromDate, string.Empty, true);
                    }
                }
                if (sheet.Columns.Contains(TestDataConstants.COL_TO_DATE))
                {
                    String toDate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(sheet.Rows[0][TestDataConstants.COL_TO_DATE].ToString()))
                    {
                        toDate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(sheet.Rows[0][TestDataConstants.COL_TO_DATE].ToString()));
                        DtPickerUpper.Click(MouseButtons.Left);

                        DtPickerUpper.Properties["Text"] = toDate;
                        //ExtentionMethods.CheckCellValueConditions(toDate, string.Empty, true);
                    }
                }
                BtnGetCash.Click(MouseButtons.Left);
                Wait(10000);
                ExtentionMethods.WaitForEnabled(ref BtnGetCash, TestDataConstants.GL_DATA_FETCHING_TIME);
                if (RevaluationPopUp.IsVisible)
                {
                    ButtonOK2.Click(MouseButtons.Left);
                    BtnGetCash.Click(MouseButtons.Left);
                }
            }
            catch
            {                
                throw;
            }
        }

        /// <summary>
        /// Open revaluation tab in the cash journal 
        /// </summary>
        private void OpenRevaluation()
        {
            Revaluation.Click(MouseButtons.Left);
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
