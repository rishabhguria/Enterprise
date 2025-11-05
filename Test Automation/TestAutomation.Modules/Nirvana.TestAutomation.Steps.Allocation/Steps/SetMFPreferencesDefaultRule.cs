using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class SetMFPreferencesDefaultRule : MasterFundPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Begins the test execution
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenMFPreferences();
                Records.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                SetDefaultRule(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetMFPreferencesDefaultRule");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeEditAllocationPreference();
            }
            return _res;
        }
        /// <summary>
        /// Sets the default rule.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        private void SetDefaultRule(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataRow row = testData.Tables[sheetIndexToName[0]].Rows[0];

                string preferenceName = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_PREFERENCE_NAME].ToString();
                if (!preferenceName.Equals(string.Empty))
                {
                    Records.Click(MouseButtons.Left);
                    Dictionary<string, int> indexToName = new Dictionary<string, int>();
                    for (int i = 1; i < Records.AutomationElementWrapper.CachedChildren.Count; i++)
                    {
                        indexToName.Add(Records.AutomationElementWrapper.CachedChildren[i].CachedChildren[1].Name, i);
                    }
                    Records.AutomationElementWrapper.FindDescendantByName(preferenceName).WpfClick();
                    //set default rule values

                    String TargetPercentage = row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString();
                    if (!TargetPercentage.Equals(String.Empty))
                    {
                        Clickdropdownbutton(CmbboxTargetPercentage);
                        Wait(2000);
                        ClickOnComboBoxItem(row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString(), CmbboxTargetPercentage);
                    }

                    String RemainderAllocation = row[TestDataConstants.COL_REMAINDER_ALLOCATION_TO].ToString();
                    if (!RemainderAllocation.Equals(String.Empty))
                    {
                        Clickdropdownbutton(CmbboxRemainderAllocation);
                        Wait(2000);
                        ClickOnComboBoxItem(row[TestDataConstants.COL_REMAINDER_ALLOCATION_TO].ToString(), CmbboxRemainderAllocation);
                    }

                    if (TargetPercentage.Equals(TestDataConstants.COL_PRORATA) || TargetPercentage.Equals(TestDataConstants.COL_LEVELING) || TargetPercentage.Equals(TestDataConstants.COL_PRORATA_NAV))
                    {
                        if (!row[TestDataConstants.COL_PRORATA_ACCOUNTS].ToString().Equals(String.Empty))
                        {
                            TextBoxPresenter1.Click(MouseButtons.Left);
                            TextBoxPresenter.Click(MouseButtons.Left);
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
                    if (testData.Tables[sheetIndexToName[0]].Columns.Contains(TestDataConstants.COL_MATCH_CLOSING_TRANSACTION))
                    {
                        String MatchClosingTransaction = row[TestDataConstants.COL_MATCH_CLOSING_TRANSACTION].ToString();
                        if (!MatchClosingTransaction.Equals(string.Empty))
                        {
                            Clickdropdownbutton(CmbboxMatchClosingTransaction2);
                            Wait(2000);
                            ClickOnComboBoxItem(row[TestDataConstants.COL_MATCH_CLOSING_TRANSACTION].ToString(), CmbboxMatchClosingTransaction2);
                        }
                    }
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