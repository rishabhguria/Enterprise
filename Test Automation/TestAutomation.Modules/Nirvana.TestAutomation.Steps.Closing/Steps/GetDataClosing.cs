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

namespace Nirvana.TestAutomation.Steps.Closing
{
    class GetDataClosing : ClosingUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenClosingUI();
                //Wait(3000);
                ClosedAmend.Click(MouseButtons.Left);
                GetDataClose(testData, sheetIndexToName);

            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeClosing();
            }
            return _result;
        }
        /// <summary>
        /// Checks the close.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        private string GetDataClose(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            string errorMessage = string.Empty;
            try
            {
                //If RangeType cell value is Blank or White Space then by default Current Radio button will be selected
                String rangetype = TestDataConstants.COL_CURRENT;
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    if (!String.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_RANGETYPE].ToString()))
                        rangetype = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_RANGETYPE].ToString();

                    if (rangetype.Equals(TestDataConstants.COL_CURRENT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // In the case of range type current , Account filter select all the account by default
                        //select all in account filter by default
                        Dictionary<int, string> allItems = (Dictionary<int, string>)MultiSelectAccountFilterCombo.InvokeMethod("GetAllItemsInDictionary", null);
                        object[] parameters = new object[2];
                        parameters[0] = allItems;
                        parameters[1] = CheckState.Checked;
                        MultiSelectAccountFilterCombo.InvokeMethod("SelectUnselectItems", parameters);
                        //radio button current click
                        RbCurrent.Click(MouseButtons.Left);
                    }
                    else
                    {
                        ExtentionMethods.WaitForEnabled(ref RbHistorical, 20);
                        if (RbHistorical.IsEnabled)
                        {
                            RbHistorical.Click(MouseButtons.Left);

                            //From Date
                            String fromdate = string.Empty;
                            if (!string.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FROM].ToString()))
                                fromdate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FROM].ToString()));
                            FromDatePicker.Click(MouseButtons.Left);
                            DtFromDate.Properties["Text"] = fromdate;

                            //To Date
                            String todate = string.Empty;
                            if (!string.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TO].ToString()))
                                todate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TO].ToString()));
                            ToDatePicker.Click(MouseButtons.Left);
                            DtToDate.Properties["Text"] = todate;

                            //Account Filter
                            if (!string.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ACCOUNT_FILTER].ToString()) && !testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ACCOUNT_FILTER].ToString().Equals("Select All"))
                            {
                                String accountFilter = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ACCOUNT_FILTER].ToString();
                                List<string> accountFilterlist = accountFilter.Split(',').ToList();
                                ExtentionMethods.SelectMultipleItemsFromCombo(accountFilterlist, MultiSelectAccountFilterCombo);
                            }
                            else if (testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ACCOUNT_FILTER].ToString().Equals("Select All"))
                            {
                                //if Account Filter cell value is Blank or White Space then by default select all in Account filter
                                Dictionary<int, string> allItems = (Dictionary<int, string>)MultiSelectAccountFilterCombo.InvokeMethod("GetAllItemsInDictionary", null);
                                object[] parameters = new object[2];
                                parameters[0] = allItems;
                                parameters[1] = CheckState.Checked;
                                MultiSelectAccountFilterCombo.InvokeMethod("SelectUnselectItems", parameters);
                            }
                        }
                    }
                    BtnRefresh.Click(MouseButtons.Left);
                    if (NoData.IsVisible)
                    {
                        NoDataButtonOK.Click(MouseButtons.Left);
                    }
                }
            }
            catch (Exception ex)
            {
               errorMessage="Date format has mismatched.!\n ( " + ex.Message + " )";
               throw;
            }
            return errorMessage;
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
