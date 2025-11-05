using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.IO;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Steps.Rebalancer;



namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class AddCash : RebalancerUIMap, ITestStep
    {
        /// <summary>
        /// Run Test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
                Wait(4000);
                RebalancerTabButton.Click(MouseButtons.Left);

                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                    InputCashFlow(dr);
                }

                Wait(4000);

                //Minimize Rebalancer
                KeyboardUtilities.MinimizeWindow(ref RebalanceTab);

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

        /// <summary>
        /// Input Cash Flow
        /// </summary>
        /// <param name="dr"></param>
        private void InputCashFlow(DataRow dr)
        {
            try
            {
                string value = string.Empty;

                //Select Cash Flow
                if (!dr[TestDataConstants.Text_CASHFLOW].ToString().Equals(string.Empty))
                {
                    CashFowEditor.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.Text_CASHFLOW].ToString());
                }

                /*SELECT CUSTOM CASH FLOW*/

                if (dr[TestDataConstants.Text_CASHFLOW].ToString().Equals(string.Empty) && ((dr[TestDataConstants.COL_ALLOW_CUSTOMCASHFLOW].ToString() == "TRUE") || dr[TestDataConstants.COL_ALLOW_CUSTOMCASHFLOW].ToString() == "True"))
                {
                    String[] Accounts = dr["Accounts"].ToString().Split(',');
                    String[] CashFlow = dr["Cash Flow Account Wise"].ToString().Split('|');


                    uiAutomationElement5.Click(MouseButtons.Left);
                    Wait(3000);
                    if (CustomCashFlow2.IsEnabled)
                    {
                        if (dr[TestDataConstants.COL_MFGROUPS].ToString() == "NewMF" || dr[TestDataConstants.COL_MFGROUPS].ToString() == "NewMF2" || dr[TestDataConstants.COL_MFGROUPS].ToString().Equals(string.Empty))
                        {
                            for (int i = 0; i < Accounts.Length; i++)
                            {
                                if (Accounts[i].ToString() == "Allocation1" || (Accounts[i].ToString() == "LP C/O"))
                                {
                                    PranaRebalancerRebalancerNewModelsAdjustedAccountLevelNAV1.DoubleClick();
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);

                                    Keyboard.SendKeys(CashFlow[i].ToString());

                                }
                                if (Accounts[i].ToString() == "Allocation3" || (Accounts[i].ToString() == "rt"))
                                {
                                    PranaRebalancerRebalancerNewModelsAdjustedAccountLevelNAV2.DoubleClick();
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);

                                    Keyboard.SendKeys(CashFlow[i].ToString());

                                }
                            }



                        }
                        if (dr[TestDataConstants.COL_MFGROUPS].ToString() == "NewMF1")
                        {
                            for (int i = 0; i < Accounts.Length; i++)
                            {
                                if (Accounts[i].ToString() == "Allocation2")
                                {
                                    PranaRebalancerRebalancerNewModelsAdjustedAccountLevelNAV1.DoubleClick();
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                                    Keyboard.SendKeys(CashFlow[i].ToString());
                                }
                                if (Accounts[i].ToString() == "OFFSHORE")
                                {
                                    PranaRebalancerRebalancerNewModelsAdjustedAccountLevelNAV2.DoubleClick();
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                                    Keyboard.SendKeys(CashFlow[i].ToString());

                                }

                                if (Accounts[i].ToString() == "Allocation4")
                                {
                                    PranaRebalancerRebalancerNewModelsAdjustedAccountLevelNAV3.DoubleClick();
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                                    Keyboard.SendKeys(CashFlow[i].ToString());

                                }
                            }


                        }

                    }
                    if (OK.IsEnabled)
                    {
                        PranaRebalancerRebalancerNewModelsAdjustedAccountLevelNAV1.DoubleClick();
                        OK1.Click();
                    }

                    /*if (CustomCashFlow4.IsEnabled)
                    {
                        CustomCashFlow4.Click();
                        KeyboardUtilities.CloseWindow(ref CustomCashFlow4);
 
                    }*/

                }



                //
                CmbCashImpactOnNAV.Click(MouseButtons.Left);
                Wait(2000);

                //Select cash flow impact on NAV
                if (!dr[TestDataConstants.COL_CASHFLOWIMPACTONNAV].ToString().Equals(string.Empty))
                {
                    value = dr[TestDataConstants.COL_CASHFLOWIMPACTONNAV].ToString();
                    if (value.Equals("Impact NAV"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }

                    if (value.Equals("No Impact"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                }
                //Select Model
                SelectModelComboEditor.Click(MouseButtons.Left);
                Wait(2000);
                if (!dr[TestDataConstants.COL_SELECTMODEL].ToString().Equals(string.Empty))
                {
                    value = dr[TestDataConstants.COL_SELECTMODEL].ToString();


                    if (String.IsNullOrEmpty(value))
                    {
                        return;
                    }

                    if (value.Equals("None"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        return;
                    }

                    if (value.Equals("Target Cash"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        return;
                    }
                    if (value.Equals("Target Cash Negative"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        return;
                    }
                    if (value.Equals("TargetCashEqual%"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        return;
                    }

                    if (value.Equals("Target Cash -2"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        return;
                    }

                    // Assuming comboBox is the ComboBox control on your form


                    if (value.Contains("Target"))
                    {
                        char[] arr = new char[value.Length];
                        for (int i = 0; i < value.Length; i++)
                        {
                            arr[i] = value[i];
                        }
                        int num1 = -1;
                        if (Char.IsNumber(arr[arr.Length - 2]))
                        {
                            num1 = Convert.ToInt32(new string(arr[arr.Length - 2], 1));
                        }
                        int num2 = Convert.ToInt32(new string(arr[arr.Length - 1], 1));

                        if (num1 != -1)
                        {
                            for (int i = 0; i < (num1 * 10 + num2) + 2; i++)
                            {
                                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                            }
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                        else
                        {
                            for (int i = 0; i < num2 + 2; i++)
                            {
                                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                            }
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                    }
                    else {
                        Keyboard.SendKeys(value);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }


                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}
