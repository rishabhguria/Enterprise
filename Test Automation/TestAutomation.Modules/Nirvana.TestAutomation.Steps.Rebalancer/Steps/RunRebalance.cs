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
using System.Diagnostics;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class RunRebalance : RebalancerUIMap, ITestStep
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
                    DataRow row = testData.Tables[sheetIndexToName[0]].Rows[0];
                    InputDataToRunRebalancer(row, testData.Tables[0]);
                
                }
                //Code to handle Pop-up of No shortning
                if (NirvanaAlert1.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                    //Minimize Rebalancer
                    KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
                }
                else if (NirvanaAlert7.IsVisible)
                {
                    ButtonOK2.Click(MouseButtons.Left);
                    //Minimize Rebalancer
                    KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
                }
                else
                {
                    //Minimize Rebalancer
                    KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
                }
                //KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
                            
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
        /// Input Data To Run Rebalancer
        /// </summary>
        /// <param name="dr"></param>
        private void InputDataToRunRebalancer(DataRow dr, DataTable dt)
        {
            try
            {
                string value = string.Empty;

                //Select Round Lot Check Box
                if (dt.Columns.Contains("Round Lot"))
                {
                   
                    if (dr[TestDataConstants.COL_ROUNDLOT].ToString().Equals("True") || string.Equals(dr[TestDataConstants.COL_ROUNDLOT].ToString(), "ToggleState_On", StringComparison.OrdinalIgnoreCase))
                    {
                        ReMinusbalancebasedonRoundlots.Click(MouseButtons.Left);
                        Wait(2000);
                    }
                    
                }

                //Select Rounding Off
                if (!dr[TestDataConstants.COL_ROUNDINGTYPE].ToString().Equals(string.Empty))
                {
                    value = dr[TestDataConstants.COL_ROUNDINGTYPE].ToString();

                    CmbRoundType.Click(MouseButtons.Left);
                    Wait(4000);

                    if (value.Equals("Round Down"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }

                    if (value.Equals("Round Up"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }

                    if (value.Equals("Nearest Round Off"))
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                }
                //Click on Rebalance button
                Rebalance3.Click(MouseButtons.Left);
                Wait(1000);

                if (WindowsCustomMessageBox.IsVisible)
                {
                    string value1 = string.Empty;

                    if (!dr[TestDataConstants.COL_AllowNegCash].ToString().Equals(string.Empty))
                    {
                        value1 = dr[TestDataConstants.COL_AllowNegCash].ToString();
                        if (value1.Equals("Yes"))
                        {
                            UltraButton1.Click(MouseButtons.Left);
                        }
                    }
                    else if (!dr[TestDataConstants.COL_SellToRaiseCash].ToString().Equals(string.Empty))
                    {
                        value1 = dr[TestDataConstants.COL_SellToRaiseCash].ToString();
                        if (value1.Equals("Yes"))
                        {
                            UltraButton2.Click(MouseButtons.Left);
                        }
                    }
                    else
                        UltraButton3.Click(MouseButtons.Left);
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
