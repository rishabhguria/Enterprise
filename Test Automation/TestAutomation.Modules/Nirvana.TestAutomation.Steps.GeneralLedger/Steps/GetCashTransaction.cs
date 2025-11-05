using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    /// <summary>
    /// Get cash transactions data
    /// </summary>
    class GetCashTransaction : CashTransactionsUIMap, ITestStep
    {
        /// <summary>
        /// To get cash transactions data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenCashTransactions();
                DataTable sheet = testData.Tables[sheetIndexToName[0]];
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    EnterCashTransactionFields(sheet);
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
        /// To enter the field value to get cash transactions data
        /// </summary>
        /// <param name="sheet"></param>
        private void EnterCashTransactionFields(DataTable sheet)
        {
            try
            {
                if (sheet.Columns.Contains(TestDataConstants.COL_ACCOUNT))
                {
                    List<string> selectItems = new List<string>();
                    String account = string.Empty;
                    if (!string.IsNullOrWhiteSpace(sheet.Rows[0][TestDataConstants.COL_ACCOUNT].ToString()))
                        account = sheet.Rows[0][TestDataConstants.COL_ACCOUNT].ToString();
                    string[] accountList = account.Split(',');
                    foreach (string acc in accountList)
                    {
                        account = acc.Trim();
                        selectItems.Add(account);
                    }

                    MultiSelectDropDown1.Click(MouseButtons.Left);
                    ExtentionMethods.SelectMultipleItemsFromCombo(selectItems, MultiSelectDropDown1);

                }
                if (sheet.Columns.Contains(TestDataConstants.COL_DATE_TYPE))
                {
                    List<string> selectItems = new List<string>();
                    String dateType = sheet.Rows[0][TestDataConstants.COL_DATE_TYPE].ToString();
                    if (!string.IsNullOrWhiteSpace(dateType))
                    {
                        DtDateType.Click(MouseButtons.Left);
                        DtDateType.Properties["Text"] = dateType;                     
                    }
                }
                if (sheet.Columns.Contains(TestDataConstants.COL_FROM_DATE))
                {

                    String fromDate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(sheet.Rows[0][TestDataConstants.COL_FROM_DATE].ToString()))
                    {
                        fromDate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(sheet.Rows[0][TestDataConstants.COL_FROM_DATE].ToString()));
                        DtFrom.Click(MouseButtons.Left);
                        DtFrom.Properties["Text"] = fromDate;
                        //ExtentionMethods.CheckCellValueConditions(fromDate, string.Empty, true);
                    }
                }
                if (sheet.Columns.Contains(TestDataConstants.COL_TO_DATE))
                {
                    String toDate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(sheet.Rows[0][TestDataConstants.COL_TO_DATE].ToString()))
                    {
                        toDate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(sheet.Rows[0][TestDataConstants.COL_TO_DATE].ToString()));
                        DtTo.Click(MouseButtons.Left);
                        DtTo.Properties["Text"] = toDate;
                        //ExtentionMethods.CheckCellValueConditions(toDate, string.Empty, true);
                    }
                }
                if (sheet.Columns.Contains(TestDataConstants.COL_SYMBOL))
                {
                    String symbol = sheet.Rows[0][TestDataConstants.COL_SYMBOL].ToString();
                    TxtBoxSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                    Keyboard.SendKeys(KeyboardConstants.SHIFTHOMEKEY);
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    Keyboard.SendKeys(symbol);
                }
                if (sheet.Rows[0][TestDataConstants.COL_STARTS_WITH].ToString().Equals("Yes"))
                {
                    CbMatchStartsWith.Click(MouseButtons.Left);
                }
                else if (sheet.Rows[0][TestDataConstants.COL_EXACT].ToString().Equals("Yes"))
                {
                    CbMatchExact.Click(MouseButtons.Left);
                }
                else
                {
                    CbMatchContains.Click(MouseButtons.Left);
                }
                BtnGet.Click(MouseButtons.Left);
                ExtentionMethods.WaitForEnabled(ref BtnGet, TestDataConstants.GL_DATA_FETCHING_TIME);
                BtnSave.Click(MouseButtons.Left);
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
