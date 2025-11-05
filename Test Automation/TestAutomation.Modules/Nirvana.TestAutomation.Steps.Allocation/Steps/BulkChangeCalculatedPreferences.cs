using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
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
    class BulkChangeCalculatedPreferences : CalculatedPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenCalculatedPreferences();
                BulkChangeOnCalculatedPreference(testData, sheetIndexToName);
                Records.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch(Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "BulkChangeCalculatedPreferences");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
        /// <summary>
        /// Bulks the change on calculated preference.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        private void BulkChangeOnCalculatedPreference(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                BulkChange.Click(MouseButtons.Left);
                DataRow row = testData.Tables[sheetIndexToName[0]].Rows[0];

                if (!row[TestDataConstants.COL_APPLY_ON_DEFAULT_RULE].ToString().Equals(String.Empty) && !ApplyonDefaultRule.IsChecked)
                {
                    ApplyonDefaultRule.Click(MouseButtons.Left);

                }
                if (!row[TestDataConstants.COL_APPLY_ON_SELECTED_PREFERENCE].ToString().Equals(String.Empty) && !ApplyonselectedPreferences.IsChecked)
                {
                    ApplyonselectedPreferences.Click(MouseButtons.Left);
                    // Handling for Select Preferences
                    TextBoxPresenter3.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(row[TestDataConstants.COL_APPLY_ON_SELECTED_PREFERENCE].ToString(), string.Empty, true);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                }
                 if (!row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString().Equals(String.Empty) && !TargetPercentasof.IsChecked)
                {
                    TargetPercentasof.Click(MouseButtons.Left);
                    CmbboxTargetPercentage.Click(MouseButtons.Left);
                    Clickdropdown(CmbboxTargetPercentage);
                    Wait(2000);
                    ClickOnComboBoxItem(row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString(), CmbboxTargetPercentage);
                }
                 if (!row[TestDataConstants.COL_ALLOCATION_METHOD].ToString().Equals(String.Empty) && !row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString().Equals(TestDataConstants.COL_PRORATA) && !row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString().Equals(TestDataConstants.COL_LEVELING) && !AllocationMethod.IsChecked)
                {
                    AllocationMethod.Click(MouseButtons.Left);
                    CmbboxAllocationMethod.Click(MouseButtons.Left);
                    Clickdropdown(CmbboxAllocationMethod);
                    Wait(2000);
                    ClickOnComboBoxItem(row[TestDataConstants.COL_ALLOCATION_METHOD].ToString(), CmbboxAllocationMethod);
                }
                if (!row[TestDataConstants.COL_REMAINDER_ALLOCATION_TO].ToString().Equals(String.Empty) && !RemainderAllocationto.IsChecked)
                {
                    RemainderAllocationto.Click(MouseButtons.Left);
                    // Correct method to click on the drop down
                    Clickdropdown(CmbboxRemainderAllocation);
                    var b = CmbboxRemainderAllocation.AutomationElementWrapper.CachedChildren;
                    Wait(2000);
                    ClickOnComboBoxItem(row[TestDataConstants.COL_REMAINDER_ALLOCATION_TO].ToString(), CmbboxRemainderAllocation);

                }
               
                if (!row[TestDataConstants.COL_MATCH_CLOSING_TRANSACTIONS].ToString().Equals(String.Empty) && !Matchclosingtrasactionsexactly.IsChecked)
                {
                    Matchclosingtrasactionsexactly.Click(MouseButtons.Left);
                    Clickdropdown(CmbboxMatchClosingTransaction);
                    Wait(2000);
                    ClickOnComboBoxItem(row[TestDataConstants.COL_MATCH_CLOSING_TRANSACTIONS].ToString(), CmbboxMatchClosingTransaction);
                }
                if(row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString().Equals(TestDataConstants.COL_PRORATA) || row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString().Equals(TestDataConstants.COL_LEVELING))
                {
                    if(!row[TestDataConstants.COL_ACCOUNTS_FOR_PRORATA].ToString().Equals(String.Empty))
                     {
                    TextBoxPresenter.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(row["AccountsForProrata"].ToString(), string.Empty, true);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                }
                if (row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString().Equals(TestDataConstants.COL_PRORATA))
                {
                    if (!row[TestDataConstants.COL_DATE_UPTO_DAYS].ToString().Equals(String.Empty))
                    {
                        TxtboxDateUptoDays.Click(MouseButtons.Left);
                        Keyboard.SendKeys(row[TestDataConstants.COL_DATE_UPTO_DAYS].ToString());
                    }
                }
                Apply.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Clickdropdownbuttons the specified parent automation element.
        /// </summary>
        /// <param name="parentAutomationElement">The parent automation element.</param>
        void Clickdropdown(UIAutomationElement parentAutomationElement)
        {
            try
            {
                UIControlPart dropdownButton = new UIControlPart();
                dropdownButton.BoundsInParent = new System.Drawing.Rectangle(100, 0, 15, 20);
                dropdownButton.Comment = null;
                dropdownButton.ControlPartProvider = null;
                dropdownButton.Name = "dropdownButton";
                dropdownButton.ObjectImage = null;
                dropdownButton.Parent = parentAutomationElement;
                dropdownButton.Path = null;
                dropdownButton.UIObjectType = TestAutomationFX.UI.UIObjectTypes.ControlPart;
                dropdownButton.UseCoordinatesOnClick = false;
                dropdownButton.Click(MouseButtons.Left);
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
