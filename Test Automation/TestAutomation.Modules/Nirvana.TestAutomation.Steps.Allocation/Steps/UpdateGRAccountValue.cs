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

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class UpdateGRAccountValue : CalculatedPreferencesUIMap, ITestStep
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
                //PranaApplication.BringToFrontOnAttach = false;
                AccountStrategyGrid1.Click(MouseButtons.Left);
                UpdateAccountValues(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "UpdateGRAccountValue");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Updates the account values
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void UpdateAccountValues(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            /*try
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
                            Wait(1000);
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                    }
                }
            catch (Exception)
            {
                throw;
            }*/
            try
            {
                Dictionary<string, int> accountIndexMap = GetAccountIndexMap(AccountStrategyGrid1);
                /* This dictionary hasn't used anywhere in this step
                Dictionary<string, int> indexToName = GetIndexNameMap(); */

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
                    //Records.AutomationElementWrapper.FindDescendantByName(prefName).WpfClick();
                    //Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                    //ResetAccountValues();
                    accountIndexMap.Clear();
                    // accountIndexMap = GetAccountIndexMap(AccountStrategyGrid);

                    foreach (DataRow row in dictionary[prefName])
                    {

                        string accountName = row[TestDataConstants.COL_ACCOUNTNAME].ToString();
                        string percentage = row[TestDataConstants.COL_PERCENTAGE].ToString();
                        DeletePrevSearchedAccount(AccountStrategyGrid1);
                        Wait(2000);
                        Keyboard.SendKeys(accountName);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        Wait(6000);
                        accountIndexMap = GetAccountIndexMap(AccountStrategyGrid1);
                        AccountStrategyGrid1.AutomationElementWrapper.Children[accountIndexMap[accountName]].FindDescendantByName("Account").Children[2].WpfClick();

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
        /*private void ResetAccountValues()
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
        }*/

        private void ResetAccountValues()
        {
            try
            {
                int count = AccountStrategyGrid1.AutomationElementWrapper.Children.Count;
                count = count - 1;
                AccountStrategyGrid1.AutomationElementWrapper.Children[0].Children[0].Children[2].WpfClick();
                for (int index = count - 1; index >= 0; index--)
                {
                    AccountStrategyGrid1.AutomationElementWrapper.Children[index].Children[0].Children[2].WpfClick();
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
