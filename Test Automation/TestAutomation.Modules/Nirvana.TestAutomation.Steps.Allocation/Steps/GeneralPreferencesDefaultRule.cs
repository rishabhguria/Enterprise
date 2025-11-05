using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.TestDataProvider;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;


namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class GeneralPreferencesDefaultRule : PreferencesUIMap , ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                SetPreferences(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "GeneralPreferencesDefaultRule");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeAllocation();
            }
            return _res;
        }

        /// <summary>
        /// Opens the General Preferences.
        /// </summary>
        private void OpenGeneralPreferences()
        {
            try
            {
                Preferences.Click(MouseButtons.Left);
                GeneralPreferences.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the General Preferences.
        /// </summary>
        private void SetPreferences(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                    OpenGeneralPreferences();
                    InputPreferences(testData.Tables[sheetIndexToName[0]].Rows[0]);
                    Save.Click(MouseButtons.Left);
                    Close.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

  
        private void InputPreferences(DataRow dtRow)
        {
            try
            {
                String TargetPercentage = dtRow[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString();
                if (!TargetPercentage.Equals(String.Empty))
                {
                    ToggleBtnTargetPercentage.Click(MouseButtons.Left);
                    Wait(2000);
                    ClickOnComboBoxItem(TargetPercentage, CmbboxTargetPercentage);
                }

                String AllocationMethod = dtRow[TestDataConstants.COL_ALLOCATION_METHOD].ToString();
                if (!AllocationMethod.Equals(String.Empty) && !TargetPercentage.Equals(TestDataConstants.COL_PRORATA) && !TargetPercentage.Equals(TestDataConstants.COL_LEVELING))
                {
                    ToggleBtnCmbboxAllocationMethod.Click(MouseButtons.Left);
                    Wait(2000);
                    ClickOnComboBoxItem(AllocationMethod, CmbboxAllocationMethod);
                }

                String RemainderAllocation = dtRow[TestDataConstants.COL_REMAINDER_ALLOCATION_TO].ToString();
                ToggleBtnRemainderAllocation.Click(MouseButtons.Left);
                Wait(2000);
                if (RemainderAllocation.Equals(String.Empty))
                {
                    CmbboxRemainderAllocation.AutomationElementWrapper.CachedChildren[1].CachedChildren[0].WpfClick();
                }
                else
                {
                    ClickOnComboBoxItem(RemainderAllocation, CmbboxRemainderAllocation);
                }

                if (TargetPercentage.Equals(TestDataConstants.COL_PRORATA) || TargetPercentage.Equals(TestDataConstants.COL_LEVELING))
                {
                    if (!dtRow[TestDataConstants.COL_PRORATA_ACCOUNTS].ToString().Equals(String.Empty))
                    {
                        CmbboxAccountsForProrata.Click(MouseButtons.Left);
                        ExtentionMethods.CheckCellValueConditions(dtRow[TestDataConstants.COL_PRORATA_ACCOUNTS].ToString(), string.Empty, true);
                    }
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (TargetPercentage.Equals(TestDataConstants.COL_PRORATA))
                {

                    if (dtRow[TestDataConstants.COL_DATE_UP_TO_DAYS].ToString().Equals(String.Empty))
                    {
                        ExtentionMethods.CheckCellValueConditions("0", string.Empty, true);
                    }
                    else
                    {
                        ExtentionMethods.CheckCellValueConditions(dtRow[TestDataConstants.COL_DATE_UP_TO_DAYS].ToString(), string.Empty, true);
                    }
                }
                bool result = false;
                Boolean.TryParse(dtRow[TestDataConstants.COL_MATCH_CLOSING].ToString(), out result);

                if (result && !ChkboxMatchClosingTransaction.IsChecked)
                {
                    ChkboxMatchClosingTransaction.Click(MouseButtons.Left);
                }
                else if (!result && ChkboxMatchClosingTransaction.IsChecked)
                {
                    ChkboxMatchClosingTransaction.Click(MouseButtons.Left);
                }
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
