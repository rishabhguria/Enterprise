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

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class SetMFPrefStrategyValues : MasterFundPreferencesUIMap, ITestStep
    {

        /// <summary>
        /// Runs the test
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
                SetStrategyValues(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetMFPrefStrategyValues");
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
        /// Sets the strategy values
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void SetStrategyValues(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                Dictionary<string, int> preferenceIndexMap = GetPreferenceIndexMap();
                Dictionary<string, int> accountIndexMap = GetAccountIndexMap(AccountStrategyGrid);
                Dictionary<string, int> strategyIndexMap = GetStrategyIndexMap(AccountStrategyGrid);

                Dictionary<string, Dictionary<string, List<DataRow>>> dictionary = new Dictionary<string, Dictionary<string, List<DataRow>>>();

                foreach (DataRow dr in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    if (dictionary.ContainsKey(dr[TestDataConstants.COL_PREFERENCE_NAME].ToString()))
                    {
                        if (dictionary[dr[TestDataConstants.COL_PREFERENCE_NAME].ToString()].ContainsKey(dr[TestDataConstants.COL_ACCOUNTNAME].ToString()))
                        {
                            dictionary[dr[TestDataConstants.COL_PREFERENCE_NAME].ToString()][dr[TestDataConstants.COL_ACCOUNTNAME].ToString()].Add(dr);
                        }
                        else
                        {
                            List<DataRow> list = new List<DataRow>() { dr };
                            dictionary[dr[TestDataConstants.COL_PREFERENCE_NAME].ToString()].Add(dr[TestDataConstants.COL_ACCOUNTNAME].ToString(), list);
                        }
                    }
                    else
                    {
                        Dictionary<string, List<DataRow>> dic = new Dictionary<string, List<DataRow>>();
                        dic.Add(dr[TestDataConstants.COL_ACCOUNTNAME].ToString(), new List<DataRow>() { dr });
                        dictionary.Add(dr[TestDataConstants.COL_PREFERENCE_NAME].ToString(), dic);
                    }
                }


                foreach (string prefName in dictionary.Keys)
                {
                    Records.AutomationElementWrapper.FindDescendantByName(prefName).WpfClick();
                    String MasterFund = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_MASTER_FUND].ToString();
                    if (!MasterFund.Equals(String.Empty))
                    {
                        Clickdropdownbutton(CmbboxMFList);
                        Wait(2000);
                        ClickOnComboBoxItem(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_MASTER_FUND].ToString(), CmbboxMFList);
                    }
                    accountIndexMap.Clear();
                    accountIndexMap = GetAccountIndexMap(AccountStrategyGrid);

                    foreach (string account in dictionary[prefName].Keys)
                    {
                        ResetStrategies(account);
                        foreach (DataRow dtrow in dictionary[prefName][account])
                        {
                            string strategy = dtrow[TestDataConstants.COL_STRATEGYNAME].ToString();
                            string percentage = dtrow[TestDataConstants.COL_PERCENTAGE].ToString();
                            AccountStrategyGrid.AutomationElementWrapper.Children[accountIndexMap[account]].Children[strategyIndexMap[strategy]].Children[0].WpfClick();
                            Keyboard.SendKeys(percentage);
                            Wait(1000);
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
        /// resets the strategies for specific account
        /// </summary>
        /// <param name="accountName"></param>
        private void ResetStrategies(string accountName)
        {
            try
            {
                var accountIndexMap = GetAccountIndexMap(AccountStrategyGrid);
                int count = AccountStrategyGrid.AutomationElementWrapper.Children[accountIndexMap[accountName]].Children.Count;
                for (int i = 1; i < count - 1; i++)
                {
                    AccountStrategyGrid.AutomationElementWrapper.Children[accountIndexMap[accountName]].Children[i].Children[0].WpfClick();
                    Keyboard.SendKeys("0");
                    Wait(1000);
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
