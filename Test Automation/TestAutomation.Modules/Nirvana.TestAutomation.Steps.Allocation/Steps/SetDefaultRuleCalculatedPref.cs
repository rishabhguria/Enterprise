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
    internal class SetDefaultRuleCalculatedPref : CalculatedPreferencesUIMap, ITestStep
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
                OpenCalculatedPreferences();
                Records.Click(MouseButtons.Left);
                //Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                SetDefaultRule(testData, sheetIndexToName);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetDefaultRuleCalculatedPref");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Sets the default rule
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
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
                    Wait(200);
                    Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                    //set default rule values
                    String TargetPercentage = row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString();
                    if (!TargetPercentage.Equals(String.Empty))
                    {
                        Clickdropdownbutton(CmbboxTargetPercentage1);
                        Wait(2000);
                        ClickOnComboBoxItem(row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString(), CmbboxTargetPercentage1);
                    }

                    String AllocationMethod = row[TestDataConstants.COL_ALLOCATION_METHOD].ToString();
                    if (!AllocationMethod.Equals(String.Empty) && !TargetPercentage.Equals(TestDataConstants.COL_PRORATA) && !TargetPercentage.Equals(TestDataConstants.COL_LEVELING))
                    {
                        CmbboxAllocationMethod1.Click(MouseButtons.Left);
                        Clickdropdownbutton(CmbboxAllocationMethod1);
                        Wait(2000);
                        ClickOnComboBoxItem(row[TestDataConstants.COL_ALLOCATION_METHOD].ToString(), CmbboxAllocationMethod1);
                    }

                    String RemainderAllocation = row[TestDataConstants.COL_REMAINDER_ALLOCATION_TO].ToString();
                    if (!RemainderAllocation.Equals(String.Empty))
                    {
                        Clickdropdownbutton(CmbboxRemainderAllocation1);
                        Wait(2000);
                        ClickOnComboBoxItem(row[TestDataConstants.COL_REMAINDER_ALLOCATION_TO].ToString(), CmbboxRemainderAllocation1);
                    }

                    if (TargetPercentage.Equals(TestDataConstants.COL_PRORATA) || TargetPercentage.Equals(TestDataConstants.COL_LEVELING) || TargetPercentage.Equals(TestDataConstants.COL_PRORATA_NAV))
                    {
                        if (!row[TestDataConstants.COL_PRORATA_ACCOUNTS].ToString().Equals(String.Empty))
                        {
                            TextBoxPresenter1.Click(MouseButtons.Left);
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
                        ControlPartOfCmbboxMatchClosingTransaction1.Click(MouseButtons.Left);
                        Wait(2000);
                        ClickOnComboBoxItem(row[TestDataConstants.COL_MATCH_CLOSING_TRANSACTIONS].ToString(), CmbboxMatchClosingTransaction1);
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
