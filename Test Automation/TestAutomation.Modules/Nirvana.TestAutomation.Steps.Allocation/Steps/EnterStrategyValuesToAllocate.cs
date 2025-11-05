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
    class EnterStrategyValuesToAllocate : AllocationUIMap, ITestStep
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
                AllocationGrids1.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                AllocateUnallocatePinTab.Click(MouseButtons.Left);
                PinnedIcon.Click(MouseButtons.Left);
                Dictionary<string, int> accountIndexMap = GetAccountIndexMap();
                Dictionary<string, List<DataRow>> dictionary = new Dictionary<string, List<DataRow>>();
                foreach (DataRow dr in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    if (dictionary.ContainsKey(dr[TestDataConstants.COL_STRATEGY_NAME].ToString()))
                        dictionary[dr[TestDataConstants.COL_STRATEGY_NAME].ToString()].Add(dr);
                    else
                    {
                        dictionary.Add(dr[TestDataConstants.COL_STRATEGY_NAME].ToString(), new List<DataRow>() { dr });
                    }
                }
                AllocateStrategies(accountIndexMap, dictionary);
                PinnedIcon.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "EnterStrategyValuesToAllocate");
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
        /// Allocates strategies
        /// </summary>
        /// <param name="accountIndexMap"></param>
        /// <param name="dictionary"></param>
        private void AllocateStrategies(Dictionary<string, int> accountIndexMap, Dictionary<string, List<DataRow>> dictionary)
        {
            try
            {
                foreach (string strategy in dictionary.Keys)
                {
                    Clickdropdownbutton(ComboBox11);
                    Wait(1000);
                    ClickOnComboBoxItem(strategy, ComboBox11);
                    SearchStrategy.Click(MouseButtons.Left);
                    //Wait(1000);
                    Dictionary<string, int> strategies = new Dictionary<string, int>();
                    strategies = GetStrategyIndexMap();
                    foreach (DataRow row in dictionary[strategy])
                    {
                        string account = row[TestDataConstants.COL_ACCOUNT_NAME].ToString();
                        if (!string.IsNullOrEmpty(row[TestDataConstants.COL_STRATEGY_QUANTITY].ToString()))
                        {
                            AccountStartegyGrid1.AutomationElementWrapper.CachedChildren[accountIndexMap[account]].FindChildByName(strategy).CachedChildren[1].WpfClick();
                            Keyboard.SendKeys(row[TestDataConstants.COL_STRATEGY_QUANTITY].ToString());
                        }
                        if (!string.IsNullOrEmpty(row[TestDataConstants.COL_STRATEGY_PERCENTAGE].ToString()))
                        {
                            AccountStartegyGrid1.AutomationElementWrapper.CachedChildren[accountIndexMap[account]].FindChildByName(strategy).CachedChildren[0].WpfClick();
                            Keyboard.SendKeys(row[TestDataConstants.COL_STRATEGY_PERCENTAGE].ToString());
                        }
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        Wait(500);
                    }
                    Keyboard.SendKeys(KeyboardConstants.HOMEKEY);
                }
            }
            catch (Exception) { throw; }
        }


        /// <summary>
        /// get account index map
        /// </summary>
        /// <returns>map of account name to index from the account strategy grid</returns>
        public Dictionary<string, int> GetAccountIndexMap()
        {
            try
            {
                Dictionary<string, int> accountIndexMap = new Dictionary<string, int>();
                for (int i = 0; i < AccountStartegyGrid1.AutomationElementWrapper.CachedChildren.Count; i++)
                {
                    accountIndexMap.Add(AccountStartegyGrid1.AutomationElementWrapper.CachedChildren[i].CachedChildren[0].CachedChildren[0].Name, i);
                }
                return accountIndexMap;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// get strategy name index map
        /// </summary>
        /// <returns>map of strategy name to index</returns>
        public Dictionary<string, int> GetStrategyIndexMap()
        {
            try
            {
                Dictionary<string, int> strategyIndexMap = new Dictionary<string, int>();
                for (int i = 0; i < AccountStartegyGrid1.AutomationElementWrapper.CachedChildren[0].CachedChildren.Count; i++)
                {
                    strategyIndexMap.Add(AccountStartegyGrid1.AutomationElementWrapper.CachedChildren[0].CachedChildren[i].Name, i);
                }
                return strategyIndexMap;
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
