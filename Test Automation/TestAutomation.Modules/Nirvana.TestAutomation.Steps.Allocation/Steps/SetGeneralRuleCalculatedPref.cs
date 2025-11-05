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
    public class SetGeneralRuleCalculatedPref : CalculatedPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// begins the test execution process
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
                Records.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                GeneralRuleCalPreference(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetGeneralRuleCalculatedPref");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

       
        /// <summary>
        /// Generals the rule cal preference.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <exception cref="System.Exception"></exception>
        private void GeneralRuleCalPreference(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                Dictionary<string, int> preferenceIndexMap = GetPreferenceIndexMap();
                DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];

                string PrefName = dr[TestDataConstants.COL_PREFERENCE_NAME].ToString();
                if (!PrefName.Equals(string.Empty))
                {
                    Records.Click(MouseButtons.Left);
                    if (preferenceIndexMap.ContainsKey(PrefName))
                    {
                        Records.AutomationElementWrapper.FindDescendantByName(PrefName).WpfClick(); 
                        Records3.Click(MouseButtons.Left);

                        //code for clicking on scroll bar
                        Records3.AutomationElementWrapper.WpfClickBottomBound(MouseButtons.Right, 10);
                        Wait(1000);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                        Records3.Click(MouseButtons.Right);
                        Add4.Click(MouseButtons.Left);
                        int i = Records3.AutomationElementWrapper.CachedChildren.Count - 1;
                            if (dr[TestDataConstants.COL_EXCHANGE_SELECTOR].ToString() != string.Empty)
                            {
                                Records3.AutomationElementWrapper.CachedChildren[i].CachedChildren[2].WpfClick();
                                Keyboard.SendKeys(dr[TestDataConstants.COL_EXCHANGE_SELECTOR].ToString());
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                if (dr[TestDataConstants.COL_EXCHANGE_LIST].ToString() != string.Empty)
                                {
                                    Records3.AutomationElementWrapper.CachedChildren[i].CachedChildren[3].WpfClick();
                                    string[] commaSeparatedComboValues = dr[TestDataConstants.COL_EXCHANGE_LIST].ToString().Split(',');
                                    foreach (string combovalue in commaSeparatedComboValues)
                                    {
                                        Keyboard.SendKeys(combovalue);
                                        //Wait(1000);
                                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                        Wait(1000);
                                    }
                                }
                            }
                            if (dr[TestDataConstants.COL_ASSET_SELECTOR].ToString() != string.Empty)
                            {
                                Records3.AutomationElementWrapper.CachedChildren[i].CachedChildren[4].WpfClick();
                                Keyboard.SendKeys(dr[TestDataConstants.COL_ASSET_SELECTOR].ToString());
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                if (dr[TestDataConstants.COL_ASSET_LIST].ToString() != string.Empty)
                                {
                                    Records3.AutomationElementWrapper.CachedChildren[i].CachedChildren[5].WpfClick();
                                    string[] commaSeparatedComboValues = dr[TestDataConstants.COL_ASSET_LIST].ToString().Split(',');
                                    foreach (string combovalue in commaSeparatedComboValues)
                                    {
                                        Keyboard.SendKeys(combovalue);
                                       // Wait(1000);
                                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                        Wait(1000);
                                    }
                                }
                            }

                            //Code for clicking on scroll bar
                            Records3.AutomationElementWrapper.WpfClickBottomBound(MouseButtons.Right, 10);
                            Wait(1000);
                            Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                            Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                            Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                   
                            if (dr[TestDataConstants.COL_PR_SELECTOR].ToString() != string.Empty)
                            {
                                Records3.AutomationElementWrapper.CachedChildren[i].CachedChildren[6].WpfClick();
                                Keyboard.SendKeys(dr[TestDataConstants.COL_PR_SELECTOR].ToString());
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                if (dr[TestDataConstants.COL_PR_LIST].ToString() != string.Empty)
                                {
                                    Records3.AutomationElementWrapper.CachedChildren[i].CachedChildren[7].WpfClick();
                                    string[] commaSeparatedComboValues = dr[TestDataConstants.COL_PR_LIST].ToString().Split(',');
                                    foreach (string combovalue in commaSeparatedComboValues)
                                    {
                                        Keyboard.SendKeys(combovalue);
                                        //Wait(1000);
                                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                        Wait(1000);
                                    }
                                }
                            }
                           
                            if (dr[TestDataConstants.COL_ORDER_SIDE_SELECTOR].ToString() != string.Empty)
                            {
                                Records3.AutomationElementWrapper.CachedChildren[i].CachedChildren[8].WpfClick();
                                Keyboard.SendKeys(dr[TestDataConstants.COL_ORDER_SIDE_SELECTOR].ToString());
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                if (dr[TestDataConstants.COL_ORDER_SIDE_LIST].ToString() != string.Empty)
                                {
                                    Records3.AutomationElementWrapper.CachedChildren[i].CachedChildren[9].WpfClick();
                                    string[] commaSeparatedComboValues = dr[TestDataConstants.COL_ORDER_SIDE_LIST].ToString().Split(',');
                                    foreach (string combovalue in commaSeparatedComboValues)
                                    {
                                        Keyboard.SendKeys(combovalue);
                                       // Wait(1000);
                                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                        Wait(1000);
                                    }
                                }
                         }

                            if (dr.Table.Columns.Contains(TestDataConstants.COL_UPDATE_PREFERENCE))
                            {
                                if (dr[TestDataConstants.COL_UPDATE_PREFERENCE].ToString().Equals("TRUE"))
                                    Console.WriteLine("Click on Update Button");
                                    Records3.AutomationElementWrapper.CachedChildren[i].CachedChildren[10].WpfClick();
                            }
                        
                        }
                    }
                    else
                    {
                        throw new Exception(MessageConstants.MSG_PREF_NOT_FOUND + " for setting the general rule. ");
                    }
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
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

