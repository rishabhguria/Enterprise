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
    class ModifyGRPreferenceValues : CalculatedPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Values of general rule Preferences not updated.!\n ( " + ex.Message + " )</exception>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                PranaApplication.BringToFrontOnAttach = false;
                UpdateGRPreference(testData, sheetIndexToName);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ModifyGRPreferenceValues");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Updates the gr preference.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        private void UpdateGRPreference(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                int i = Records3.AutomationElementWrapper.CachedChildren.Count - 1;
                Records3.AutomationElementWrapper.CachedChildren[i].CachedChildren[10].WpfClick();
                // For entering the value in the accounts in the Add/Update UI
                for (int j = 0; j < testData.Tables.Count; j++)
                {
                    if (testData.Tables[sheetIndexToName[j]].TableName.Contains("UpdateGRAccountValue"))
                        UpdateAccountValues(testData, sheetIndexToName, j);
                    // For entering the strategy values in the Add/Update UI

                    if (testData.Tables[sheetIndexToName[j]].TableName.Contains("UpdateGRStrategyValue"))
                        UpdateStrategyValues(testData, sheetIndexToName, j);

                    // For setting the default rule in the Add/Update UI

                    if (testData.Tables[sheetIndexToName[j]].TableName.Contains("UpdateGRDefaultRule"))
                        UpdateDefaultRule(testData, sheetIndexToName, j);

                }
                // For updating the values in the Add/Update UI nad close the UI
                Update.Click(MouseButtons.Left);
               // Wait(1000);
                ClosePopups();
                Close1.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Updates the account values
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void UpdateAccountValues(DataSet testData, Dictionary<int, string> sheetIndexToName, int j)
        {
            try
            {
                Dictionary<string, int> accountIndexMap = GetAccountIndexMap(AccountStrategyGrid1);
                ResetAccountValues();
                accountIndexMap.Clear();
                accountIndexMap = GetAccountIndexMap(AccountStrategyGrid1);
                foreach (DataRow row in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    string accountName = row[TestDataConstants.COL_ACCOUNTNAME].ToString();
                    string percentage = row[TestDataConstants.COL_PERCENTAGE].ToString();
                    if (accountName != string.Empty)
                    {
                        AccountStrategyGrid1.AutomationElementWrapper.Children[accountIndexMap[accountName]].FindDescendantByName("Account").Children[2].WpfClick();
                        Keyboard.SendKeys(percentage);
                       // Wait(1000);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Account values of general rule preference has not been updated! (" + ex.Message + ")");
            }
        }
        /// <summary>
        /// reset the previous account values
        /// </summary>
        /// <param name="accountIndexMap"></param>
        public void ResetAccountValues()
        {
            try
            {
                int count = AccountStrategyGrid1.AutomationElementWrapper.Children.Count;
                AccountStrategyGrid1.AutomationElementWrapper.Children[0].Children[0].Children[2].WpfClick();
                for (int index = count - 1; index >= 0; index--)
                {
                    AccountStrategyGrid1.AutomationElementWrapper.Children[index].Children[0].Children[2].WpfClick();
                    Keyboard.SendKeys("0");
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                }
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Sets the strategy values
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void UpdateStrategyValues(DataSet testData, Dictionary<int, string> sheetIndexToName, int j)
        {
            try
            {
                Dictionary<string, int> preferenceIndexMap = GetPreferenceIndexMap();
                Dictionary<string, int> accountIndexMap = GetAccountIndexMap(AccountStrategyGrid1);
                Dictionary<string, int> strategyIndexMap = GetStrategyIndexMap(AccountStrategyGrid1);

                Dictionary<string, List<DataRow>> dictionary = new Dictionary<string, List<DataRow>>();

                foreach (DataRow dr in testData.Tables[sheetIndexToName[j]].Rows)
                {
                    if (dictionary.ContainsKey(dr[TestDataConstants.COL_ACCOUNTNAME].ToString()))
                    {
                        dictionary[dr[TestDataConstants.COL_ACCOUNTNAME].ToString()].Add(dr);
                    }
                    else
                    {
                        List<DataRow> list = new List<DataRow>() { dr };
                        dictionary.Add(dr[TestDataConstants.COL_ACCOUNTNAME].ToString(), list);
                    }
                }

                accountIndexMap.Clear();
                accountIndexMap = GetAccountIndexMap(AccountStrategyGrid1);

                foreach (string account in dictionary.Keys)
                {
                    ResetStrategies(account);
                    foreach (DataRow dtrow in dictionary[account])
                    {
                        string strategy = dtrow[TestDataConstants.COL_STRATEGYNAME].ToString();
                        string percentage = dtrow[TestDataConstants.COL_PERCENTAGE].ToString();
                        AccountStrategyGrid1.AutomationElementWrapper.Children[accountIndexMap[account]].Children[strategyIndexMap[strategy]].Children[0].WpfClick();
                        Keyboard.SendKeys(percentage);
                        Wait(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Strategy values of general rule preference has not been updated! (" + ex.Message + ")");
            }
        }
        /// <summary>
        /// resets the strategies for specific account
        /// </summary>
        /// <param name="accountName"></param>
        private void ResetStrategies(string accountName)
        {
            try
            {
                var accountIndexMap = GetAccountIndexMap(AccountStrategyGrid1);
                int count = AccountStrategyGrid1.AutomationElementWrapper.Children[accountIndexMap[accountName]].Children.Count;
                for (int i = 1; i < count - 1; i++)
                {
                    AccountStrategyGrid1.AutomationElementWrapper.Children[accountIndexMap[accountName]].Children[i].Children[0].WpfClick();
                    Keyboard.SendKeys("0");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Updates the Default Rule
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void UpdateDefaultRule(DataSet testData, Dictionary<int, string> sheetIndexToName, int j)
        {
            try
            {
                DataRow row = testData.Tables[sheetIndexToName[j]].Rows[0];
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
            catch (Exception ex)
            {
                throw new Exception("Default rule of general rule preference has not been updated! (" + ex.Message + ")");
            }
        }

        /// <summary>
        /// Closes the popups.
        /// </summary>
        private void ClosePopups()
        {
            try
            {
                while (NirvanaPreferences1.IsVisible)
                {
                    if (ButtonYes3.IsVisible)
                        ButtonYes3.Click(MouseButtons.Left);
                   // Wait(1000);
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
