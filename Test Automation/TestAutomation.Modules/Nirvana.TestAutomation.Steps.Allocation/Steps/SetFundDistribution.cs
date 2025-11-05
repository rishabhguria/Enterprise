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
using TestAutomationFX.UI;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class SetFundDistribution : AllocationUIMap, IUIAutomationTestStep
    {

        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (testData.Tables[0].Columns.Contains("Account ID")) {
                    testData.Tables[0].Columns.Remove("Account ID");
                }

                bool RowToDelete = true;
                for (int i = testData.Tables[0].Rows.Count - 1; i >= 0; i--) {
                    foreach (DataColumn dt in testData.Tables[0].Columns) {
                        if (testData.Tables[0].Rows[i][dt].ToString().All(char.IsDigit) && testData.Tables[0].Rows[i][dt].ToString() != "0")
                        {
                            RowToDelete = false;
                        }
                    }
                    if (RowToDelete) {
                        testData.Tables[0].Rows[i].Delete();
                    }
                }
                for (int col = 3; col <= testData.Tables[0].Columns.Count; col += 2)
                {
                    if (col + 1 <= testData.Tables[0].Columns.Count) // Ensure even column exists
                    {
                        for (int row = 0; row < testData.Tables[0].Rows.Count; row++)
                        {
                            testData.Tables[0].Rows[row][col] = testData.Tables[0].Rows[row][col + 1];
                        }
                    }
                }
                for (int col = testData.Tables[0].Columns.Count - 1; col >= 3; col -= 2)
                {
                    testData.Tables[0].Columns.RemoveAt(col);
                }
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "AllocationClientWindow"));
                IUIAutomationCondition buttonCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    "btnEditAllocationPreferences"
                );

                IUIAutomationElement buttonElement = gridElement.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    buttonCondition
                );

                if (buttonElement != null)
                {
                    IUIAutomationInvokePattern invokePattern = buttonElement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                    if (invokePattern != null)
                    {
                        invokePattern.Invoke();
                    }
                }
                KeyboardUtilities.MaximizeWindow(ref EditAllocationPreferences1);
                try
                {
                    Records2.Click(MouseButtons.Left);
                    SetAccountValues(testData, sheetIndexToName);
                }
                catch {
                    SetAccountValues1(testData, sheetIndexToName);
                    DeletePrevSearchedAccount(AccountStrategyGrid1);
                
                }

            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }
        private Dictionary<string, int> GetIndexNameMap()
        {
            try
            {
                Dictionary<string, int> indexToName = new Dictionary<string, int>();
                for (int i = 1; i < Records3.AutomationElementWrapper.CachedChildren.Count; i++)
                {
                    indexToName.Add(Records3.AutomationElementWrapper.CachedChildren[i].Children[1].Name, i);
                }
                return indexToName;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        private Dictionary<string, int> GetIndexNameMap1()
        {
            try
            {
                Dictionary<string, int> indexToName = new Dictionary<string, int>();
                for (int i = 1; i < Records2.AutomationElementWrapper.CachedChildren.Count; i++)
                {
                    indexToName.Add(Records2.AutomationElementWrapper.CachedChildren[i].Children[1].Name, i);
                }
                return indexToName;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public Dictionary<string, int> GetAccountIndexMap(UIAutomationElement element)
        {
            try
            {
                Dictionary<string, int> accountIndexMap = new Dictionary<string, int>();
                for (int i = 0; i < element.AutomationElementWrapper.Children.Count; i++)
                {
                    accountIndexMap.Add(element.AutomationElementWrapper.Children[i].Children[0].Children[1].Name, i);
                }
                return accountIndexMap;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                return null;
            }
        }


        private void SetAccountValues(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                Dictionary<string, int> accountIndexMap = GetAccountIndexMap(AccountStrategyGrid);
                Dictionary<string, int> indexToName = GetIndexNameMap1();

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
                    Records2.AutomationElementWrapper.FindDescendantByName(prefName).WpfClick();
                    Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
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
        public void DeletePrevSearchedAccount(UIAutomationElement AccountStrategyGrid)
        {
            try
            {
                AccountStrategyGrid.AutomationElementWrapper.Children[0].CachedChildren[0].CachedChildren[0].CachedChildren[0].WpfClick();
                AccountStrategyGrid.AutomationElementWrapper.Children[0].CachedChildren[0].CachedChildren[0].CachedChildren[0].WpfClick();
                Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                DataUtilities.clearTextData(12, true);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;

            }
        }
        

        private void SetAccountValues1(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                Dictionary<string, int> accountIndexMap = GetAccountIndexMap(AccountStrategyGrid1);
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
                    Records3.AutomationElementWrapper.FindDescendantByName(prefName).WpfClick();
                    Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                    ResetAccountValues1();
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
        private void ResetAccountValues1()
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


    }
}
