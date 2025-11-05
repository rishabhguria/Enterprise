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
    class EnterCustomAllocatePreference : AllocationUIMap, ITestStep
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
                OpenAllocation();
                AllocateUnallocatePinTab.Click(MouseButtons.Left);
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    DataRow row = testData.Tables[sheetIndexToName[0]].Rows[0];
                    SetPreferenceFromEditPrefUI1(row);
                    AllocateUnallocatePinTab.Click(MouseButtons.Left);
                    AllocateByPreference1(row);
                    AllocateUnallocatePinTab.Click(MouseButtons.Left);
                    SetForceAllocation1(row);
                    SetCustomCheckBox(row);
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "EnterCustomAllocatePreference");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }


        /// <summary>
        /// Sets preferences from Edit Preferences
        /// </summary>
        /// <param name="row"></param>
        private void SetPreferenceFromEditPrefUI1(DataRow row)
        {
            try
            {
                Fixed1.Click(MouseButtons.Left);
                Calculated.Click(MouseButtons.Left);
                EditPreferences.Click(MouseButtons.Left);
                string targetPercentage = row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString();
                if (targetPercentage != string.Empty)
                {
                    Clickdropdownbutton(CmbboxTargetPercentage);
                    Wait(2000);
                    ClickOnComboBoxItem(row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString(), CmbboxTargetPercentage);
                }

                if (row[TestDataConstants.COL_ALLOCATION_METHOD].ToString() != string.Empty && !targetPercentage.Equals(TestDataConstants.COL_PRORATA) && !targetPercentage.Equals(TestDataConstants.COL_LEVELING))
                {
                    CmbboxAllocationMethod.Click(MouseButtons.Left);
                    Clickdropdownbutton(CmbboxAllocationMethod);
                    Wait(2000);
                    ClickOnComboBoxItem(row[TestDataConstants.COL_ALLOCATION_METHOD].ToString(), CmbboxAllocationMethod);
                }
                if (row[TestDataConstants.COL_REMAINDER_ALLOCATION_TO].ToString() != string.Empty)
                {
                    Clickdropdownbutton(CmbboxRemainderAllocation);
                    Wait(2000);
                    ClickOnComboBoxItem(row[TestDataConstants.COL_REMAINDER_ALLOCATION_TO].ToString(), CmbboxRemainderAllocation);

                }

                if (row[TestDataConstants.COL_MATCH_CLOSING_TRANSACTIONS].ToString() != string.Empty)
                {
                    if (!ChkboxMatchClosingTransaction.IsChecked && Convert.ToBoolean(row[TestDataConstants.COL_MATCH_CLOSING_TRANSACTIONS].ToString()))
                        ChkboxMatchClosingTransaction.Click(MouseButtons.Left);
                    else
                    {
                        if (ChkboxMatchClosingTransaction.IsChecked && !Convert.ToBoolean(row[TestDataConstants.COL_MATCH_CLOSING_TRANSACTIONS].ToString()))
                        {
                            ChkboxMatchClosingTransaction.Click(MouseButtons.Left);
                        }
                    }
                }
                if (row[TestDataConstants.COL_ACCOUNTS_FOR_PRORATA].ToString() != string.Empty && targetPercentage.Equals(TestDataConstants.COL_PRORATA) || targetPercentage.Equals(TestDataConstants.COL_LEVELING) || targetPercentage.Equals(TestDataConstants.COL_PRORATA_NAV))
                {
                    TextBoxPresenter7.Click(MouseButtons.Left);
                    clearText(TextBoxPresenter7);
                    Keyboard.SendKeys(row[TestDataConstants.COL_ACCOUNTS_FOR_PRORATA] + ",");
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (row[TestDataConstants.COL_DATE_UPTO_DAYS].ToString() != string.Empty && row[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString().Equals("Prorata"))
                {
                    XamMaskedEditor1.Click(MouseButtons.Left);
                    string date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(row[TestDataConstants.COL_DATE_UP_TO_DAYS].ToString()));
                    ExtentionMethods.CheckCellValueConditions(date, string.Empty, true);
                }
                ApplyandClose.Click(MouseButtons.Left);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Allocates the by preference.
        /// </summary>
        /// <param name="PrefName">Name of the preference.</param>
        /// <param name="testData">The test data.</param>
        private void AllocateByPreference1(DataRow row)
        {
            try
            {
                string prefName = row[TestDataConstants.COL_PREFERENCE_NAME].ToString();
                bool result = false;
                if (row[TestDataConstants.COL_PREFERENCE_TYPE].ToString().Equals("Fixed"))
                {
                    Fixed1.Click(MouseButtons.Left);
                    Fixed1.Click(MouseButtons.Left);
                    ComboBox13.ClickRightBound(MouseButtons.Left);
                    Wait(2000);
                    Dictionary<string, int> dictprefToColumnIndexMapping = new Dictionary<string, int>();

                    for (int i = 0; i < ComboBox13.AutomationElementWrapper.CachedChildren.Count; i++)
                    {
                        dictprefToColumnIndexMapping.Add(ComboBox13.AutomationElementWrapper.CachedChildren[i].CachedChildren[0].Name, i);
                    }
                    ComboBox13.AutomationElementWrapper.CachedChildren[dictprefToColumnIndexMapping[prefName]].CachedChildren[0].WpfClick();
                    if (!string.IsNullOrEmpty(row[TestDataConstants.COL_PTTPREFNEEDED].ToString()))
                    {
                        Boolean.TryParse(row[TestDataConstants.COL_PTTPREFNEEDED].ToString(), out result);
                        if (result && !Custom.IsChecked)
                        {
                            Custom.Click(MouseButtons.Left);
                        }
                        else if (!result && Custom.IsChecked)
                        {
                            Custom.Click(MouseButtons.Left);
                        }
                    }
                }
                else
                {
                    Calculated.Click(MouseButtons.Left);
                    Calculated.Click(MouseButtons.Left);
                     /*ToggleButton12.Click(MouseButtons.Left);
                    Wait(2000);
                    Dictionary<string, int> dictprefToColumnIndexMapping = new Dictionary<string, int>();

                    for (int i = 0; i < XamComboEditor38.AutomationElementWrapper.CachedChildren.Count; i++)
                    {
                        dictprefToColumnIndexMapping.Add(XamComboEditor38.AutomationElementWrapper.CachedChildren[i].CachedChildren[0].Name, i);
                    }
                    XamComboEditor38.AutomationElementWrapper.CachedChildren[dictprefToColumnIndexMapping[prefName]].CachedChildren[0].WpfClick(); */

                    // Enter Preference by addidng text to the editor box
                    TextBoxPresenter9.Click(MouseButtons.Left);
                    Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                    KeyboardUtilities.PressKey(TestDataConstants.NO_OF_TIMES_BACKSPACE, KeyboardConstants.BACKSPACEKEY);
                    Keyboard.SendKeys(prefName);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                    if (!string.IsNullOrEmpty(row[TestDataConstants.COL_PTTPREFNEEDED].ToString()))
                    {
                        Boolean.TryParse(row[TestDataConstants.COL_PTTPREFNEEDED].ToString(), out result);
                        if (result && !Custom.IsChecked)
                        {
                            Custom.Click(MouseButtons.Left);
                        }
                        else if (!result && Custom.IsChecked)
                        {
                            Custom.Click(MouseButtons.Left);
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
        /// Check/Uncheck the Force Allocation checkbox
        /// </summary>
        /// <param name="row"></param>
        private void SetForceAllocation1(DataRow row)
        {
            try
            {
                bool forceAllocation = false;
                Boolean.TryParse(row[TestDataConstants.COL_IS_FORCE_ALLOCATION].ToString(), out forceAllocation);
                if (ForceAllocation.IsEnabled)
                {
                    if (forceAllocation)
                    {
                        while (!ForceAllocation.IsChecked)
                        {
                            ForceAllocation.Click(MouseButtons.Left);
                        }

                    }
                    else if (!forceAllocation)
                    {
                        if (ForceAllocation.IsChecked)
                        {
                            ForceAllocation.Click(MouseButtons.Left);
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
        /// Check/Uncheck the Custom checkbox
        /// </summary>
        /// <param name="row"></param>
        private void SetCustomCheckBox(DataRow row)
        {
            try
            {
                bool customCheckBox = false;
                Boolean.TryParse(row[TestDataConstants.COL_ISCUSTOM].ToString(), out customCheckBox);
                if (Custom.IsEnabled)
                {
                    if (customCheckBox)
                    {
                        while (!Custom.IsChecked)
                        {
                            Custom.Click(MouseButtons.Left);
                        }

                    }
                    else if (!customCheckBox)
                    {
                        if (Custom.IsChecked)
                        {
                            Custom.Click(MouseButtons.Left);
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
