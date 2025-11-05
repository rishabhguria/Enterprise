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
    class AddCustomCashUsingImport : RebalancerUIMap, ITestStep
    {
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
                string Path = string.Empty;
                //Custom Cash Flow Pth
                uiAutomationElement5.Click(MouseButtons.Left);
                Wait(2000);
                if (CustomCashFlow2.IsEnabled)
                {
                    if (Import.IsEnabled)
                    {
                        Import.Click();
                    }
                    if (SelectFiletoImport.IsEnabled)
                    {
                        TextBoxFilename3.Click(MouseButtons.Left);
                        //TextBoxFilename3.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.CUSTOMCASHIMPORT].ToString();
                        Keyboard.SendKeys(dr[TestDataConstants.CUSTOMCASHIMPORT].ToString());
                        ButtonOpen.Click();

                        if (CashFlowImport.IsEnabled)
                        {
                            OK4.Click();
                        }
                        if (CustomCashFlow2.IsEnabled)
                        {
                            if (OK.IsEnabled)
                            {
                                PranaRebalancerRebalancerNewModelsAdjustedAccountLevelNAV1.DoubleClick();
                                OK1.Click();
                            }
                        }

                    }
                    
                }


                ////
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
                    if (value.Equals("None"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }

                    if (value.Equals("Target Cash"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    if (value.Equals("Target Cash Negative"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    if (value.Equals("TargetCashEqual%"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    if (value.Equals("Target Cash -2"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }

                    if (value.Equals("Target Cash-3"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);

                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }

                    if (value.Equals("Target Cash-4"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);

                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }

                    if (value.Equals("Target Cash-5"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);

                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }

                    if (value.Equals("Target Cash-6"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);

                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }

                    if (value.Equals("Target Cash-7"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);

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
