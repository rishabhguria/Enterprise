using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using System.Data;
using System.Collections.Generic;
using System;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;


namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class UpdateGRDefaultRule : CalculatedPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// begins the test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                PranaApplication.BringToFrontOnAttach = false;
                UpdateDefaultRule(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "UpdateGRDefaultRule");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

       /// <summary>
        /// Updates the Default Rule
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void UpdateDefaultRule(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataRow row = testData.Tables[sheetIndexToName[0]].Rows[0];
                String TargetPercentage = row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString();
                if (!TargetPercentage.Equals(String.Empty))
                {
                    Clickdropdownbutton(CmbboxTargetPercentage2);
                    Wait(2000);
                    ClickOnComboBoxItem(row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString(), CmbboxTargetPercentage2);
                }

                String AllocationMethod = row[TestDataConstants.COL_ALLOCATION_METHOD].ToString();
                if (!AllocationMethod.Equals(String.Empty) && !TargetPercentage.Equals(TestDataConstants.COL_PRORATA) && !TargetPercentage.Equals(TestDataConstants.COL_LEVELING))
                {
                    CmbboxAllocationMethod2.Click(MouseButtons.Left);
                    Clickdropdownbutton(CmbboxAllocationMethod2);
                    Wait(2000);
                    ClickOnComboBoxItem(row[TestDataConstants.COL_ALLOCATION_METHOD].ToString(), CmbboxAllocationMethod2);
                }

                String RemainderAllocation = row[TestDataConstants.COL_REMAINDER_ALLOCATION_TO].ToString();
                if (!RemainderAllocation.Equals(String.Empty))
                {
                    Clickdropdownbutton(CmbboxRemainderAllocation2);
                    Wait(2000);
                    ClickOnComboBoxItem(row[TestDataConstants.COL_REMAINDER_ALLOCATION_TO].ToString(), CmbboxRemainderAllocation2);
                }

                if (TargetPercentage.Equals(TestDataConstants.COL_PRORATA) || TargetPercentage.Equals(TestDataConstants.COL_LEVELING) || TargetPercentage.Equals(TestDataConstants.COL_PRORATA_NAV))
                {
                    if (!row[TestDataConstants.COL_PRORATA_ACCOUNTS].ToString().Equals(String.Empty))
                    {
                        TextBoxPresenter2.Click(MouseButtons.Left);
                        ExtentionMethods.CheckCellValueConditions(row[TestDataConstants.COL_PRORATA_ACCOUNTS].ToString(), string.Empty, true);
                    }
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (TargetPercentage.Equals(TestDataConstants.COL_PRORATA))
                {

                    if (row[TestDataConstants.COL_DATE_UP_TO_DAYS].ToString().Equals(String.Empty))
                    {
                        ExtentionMethods.CheckCellValueConditions("0", string.Empty, true);
                    }
                    else
                    {
                        ExtentionMethods.CheckCellValueConditions(row[TestDataConstants.COL_DATE_UP_TO_DAYS].ToString(), string.Empty, true);
                    }
                }

                if (!row[TestDataConstants.COL_MATCH_CLOSING_TRANSACTIONS].ToString().Equals(string.Empty))
                {
                    ControlPartOfCmbboxMatchClosingTransaction2.Click(MouseButtons.Left);
                    Wait(2000);
                    ClickOnComboBoxItem(row[TestDataConstants.COL_MATCH_CLOSING_TRANSACTIONS].ToString(), CmbboxMatchClosingTransaction2);
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
