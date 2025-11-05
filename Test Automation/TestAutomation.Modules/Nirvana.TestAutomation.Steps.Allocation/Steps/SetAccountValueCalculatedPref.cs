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
using Nirvana.TestAutomation.Utilities;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class SetAccountValueCalculatedPref : CalculatedPreferencesUIMap, ITestStep
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
                OpenCalculatedPreferences();
                SetAccountValues(testData, sheetIndexToName);
                DeletePrevSearchedAccount(AccountStrategyGrid);
                Records.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetAccountValueCalculatedPref");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
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
                    ResetAccountValues();
                    accountIndexMap.Clear();
                   // accountIndexMap = GetAccountIndexMap(AccountStrategyGrid);

                    foreach (DataRow row in dictionary[prefName])
                    {

                        string accountName = row[TestDataConstants.COL_ACCOUNTNAME].ToString();
                        string percentage = row[TestDataConstants.COL_PERCENTAGE].ToString();
                        DeletePrevSearchedAccount(AccountStrategyGrid);
                        Wait(2000);
                        Keyboard.SendKeys(accountName);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        Wait(6000);
                        accountIndexMap = GetAccountIndexMap(AccountStrategyGrid);
                        AccountStrategyGrid.AutomationElementWrapper.Children[accountIndexMap[accountName]].FindDescendantByName("Account").Children[2].WpfClick();
                       
                        Wait(1000);
                        Keyboard.SendKeys(percentage);
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
                count = count - 1;
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
        //private Dictionary<string, int> GetIndexNameMap()
        //{
        //    try
        //    {
        //        Dictionary<string, int> indexToName = new Dictionary<string, int>();
        //        for (int i = 1; i < Records.AutomationElementWrapper.CachedChildren.Count; i++)
        //        {
        //            indexToName.Add(Records.AutomationElementWrapper.CachedChildren[i].Children[1].Name, i);
        //        }
        //        return indexToName;
        //    }
        //    catch (System.Exception)
        //    {

        //        throw;
        //    }
        //}

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
