using Nirvana.TestAutomation.Interfaces;
using System;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.BussinessObjects;
using System.Collections.Generic;
using System.Linq;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class ModifyRebalance : RebalancerUIMap ,ITestStep
        /// <summary>
        /// Run Modify
        /// </summary>RebalancerUIMap, ITestStep
    {
        
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
               // Wait(4000);
                RebalancerTabButton.Click(MouseButtons.Left);
          
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    DataRow row = testData.Tables[sheetIndexToName[0]].Rows[0];
                  
                    ModifyRebal(row);
                }
                if (NirvanaAlert1.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                    //Minimize Rebalancer
                    KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
                }
                //Test1();
               


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
        ///<summary>
        ///Click clear button
        ///</summary>

        private void ModifyRebal(DataRow dr)
        {
            try
            {
                string value = string.Empty;

                if (!dr[TestDataConstants.COL_ModifyRebalance].ToString().Equals(string.Empty))
                {
                    value = dr[TestDataConstants.COL_ModifyRebalance].ToString();
                    if (value.Equals("Yes"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                        IncreaseBtn.Click(MouseButtons.Right);
                        for (int i = 1; i < 7; i++)
                        {
                            Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        }

                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
                        ModifyRebalance1.Click(MouseButtons.Left);
                    }
                }

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
                    else  if (!dr[TestDataConstants.COL_SellToRaiseCash].ToString().Equals(string.Empty))
                        {
                            value1 = dr[TestDataConstants.COL_SellToRaiseCash].ToString();
                            if (value1.Equals("Yes"))
                            {
                                UltraButton2.Click(MouseButtons.Left);
                            }
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
