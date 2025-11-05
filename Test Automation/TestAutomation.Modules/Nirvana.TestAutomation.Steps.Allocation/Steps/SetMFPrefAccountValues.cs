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
    class SetMFPrefAccountValues : MasterFundPreferencesUIMap, ITestStep
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
                OpenMFPreferences();
                SetAccountValues(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetMFPrefAccountValues");
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
        /// sets the account values
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void SetAccountValues(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                Dictionary<string, int> accountIndexMap = GetAccountIndexMap(AccountStrategyGrid);
                Dictionary<string, int> indexToName = GetIndexNameMap();

                Dictionary<string, List<DataRow>> dictionary = new Dictionary<string, List<DataRow>>();
                foreach (DataRow dr in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    if (dictionary.ContainsKey(dr[TestDataConstants.COL_PREFERENCE_NAME].ToString()))
                        dictionary[dr[TestDataConstants.COL_PREFERENCE_NAME].ToString()].Add(dr);
                    else
                    {
                        dictionary.Add(dr[TestDataConstants.COL_PREFERENCE_NAME].ToString(), new List<DataRow>() { dr });
                    }
                }

                foreach (string prefName in dictionary.Keys)
                {
                    Records.AutomationElementWrapper.FindDescendantByName(prefName).WpfClick();
                    Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                    String MasterFund = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_MASTER_FUND].ToString();
                    if (!MasterFund.Equals(String.Empty))
                    {
                       // Clickdropdownbutton(CmbboxMFList);                       
                        ControlPartOfCmbboxMFList.Click(MouseButtons.Left);                      
                        Wait(2000);                    
                        ClickOnComboBoxItem(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_MASTER_FUND].ToString(), CmbboxMFList);
                    }
                    ResetAccountValues();
                    accountIndexMap.Clear();
                    accountIndexMap = GetAccountIndexMap(AccountStrategyGrid);

                    foreach (DataRow row in dictionary[prefName])
                    {
                        string accountName = row[TestDataConstants.COL_ACCOUNTNAME].ToString();
                        string percentage = row[TestDataConstants.COL_PERCENTAGE].ToString();
                        AccountStrategyGrid.AutomationElementWrapper.Children[accountIndexMap[accountName]].FindDescendantByName("Account").Children[2].WpfClick();
                        Keyboard.SendKeys(percentage);
                        Wait(1000);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// reset the previous account values
        /// </summary>
        /// <param name="accountIndexMap"></param>
        private void ResetAccountValues()
        {
            try
            {
                int count = AccountStrategyGrid.AutomationElementWrapper.Children.Count;
                AccountStrategyGrid.AutomationElementWrapper.Children[0].Children[0].Children[2].WpfClick();
                for (int index = count - 1; index >= 0; index--)
                {
                    AccountStrategyGrid.AutomationElementWrapper.Children[index].Children[0].Children[2].WpfClick();
                    Keyboard.SendKeys("0");
                    Wait(1000);
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
        /// 
        /// </summary>
        /// <returns>map of Calculated Preference to index</returns>
        private Dictionary<string, int> GetIndexNameMap()
        {
            try
            {
                Dictionary<string, int> indexToName = new Dictionary<string, int>();
                for (int i = 1; i < Records.AutomationElementWrapper.CachedChildren.Count; i++)
                {
                    indexToName.Add(Records.AutomationElementWrapper.CachedChildren[i].Children[1].Name, i);
                }
                return indexToName;
            }
            catch (System.Exception)
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
 
    