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
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Factory;

namespace Nirvana.TestAutomation.Steps.Simulator
{
    public class ExecuteTrade : CameronSimulator, ITestStep
    {
        private static int retry = 0;
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();

            try
            {
                if (aePrana != null)
                {
                    aePrana.SetFocus();
                }
                while (retry < 3)
                {
                    foreach (DataRow row in testData.Tables[0].Rows)
                    {
                        AccessBridgeHelper.SendMessage(CameronConstants.bottomGridCommand, CameronConstants.priceField);
                        Wait(2000);
                        Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                        if (testData.Tables[0].Columns.Contains("ClearAllPrice") && !string.IsNullOrEmpty(row["ClearAllPrice"].ToString()))
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                            }
                        }
                        else
                        {
                            Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY + KeyboardConstants.BACKSPACEKEY + KeyboardConstants.BACKSPACEKEY + KeyboardConstants.BACKSPACEKEY + KeyboardConstants.BACKSPACEKEY);
                        }
                        Keyboard.SendKeys(row["Price"].ToString());
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        AccessBridgeHelper.SendMessage(CameronConstants.bottomGridCommand, CameronConstants.sharesField);
                        Wait(2000);
                        Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                        
                        Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY + KeyboardConstants.BACKSPACEKEY + KeyboardConstants.BACKSPACEKEY + KeyboardConstants.BACKSPACEKEY + KeyboardConstants.BACKSPACEKEY + KeyboardConstants.BACKSPACEKEY + KeyboardConstants.BACKSPACEKEY);
                        Keyboard.SendKeys(row["Shares"].ToString());
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);

                        Wait(500);
                        AccessBridgeHelper.SendMessage(CameronConstants.buttonCommand, CameronConstants.exeButton);
                        Wait(500);
                    }
                    /* Negative testing has been implemented to handle scenarios where an attempt is made to execute more than the available trade quantity.
                     * https://dev.azure.com/NirvanaSolutions/NirvanaOne/_workitems/edit/60488
                     * Modified by Rishabh Jaiswal
                    */
                    if (testData.Tables[0].Columns.Contains("TestingType") && !string.IsNullOrEmpty(testData.Tables[0].Rows[0]["TestingType"].ToString()))
                    {
                        string val = testData.Tables[0].Rows[0]["TestingType"].ToString().ToUpper();
                        if (val == "NEGATIVE")
                        {
                            break;
                        }
                    }
                    else
                    {
                        string result = ExtentionMethods.VerifyAndControlSimulatorAction(ApplicationArguments.JarPath, ApplicationArguments.ScriptFilePath, TestDataConstants.VerifyTradeColourSimulator, TestDataConstants.VerifyExecutedTradeColourSimulator);
                        if (result.Contains("Trade is not executed."))
                        {
                            retry++;
                        }
                        else
                        {
                            break;
                        }
                        if (retry == 2)
                        {
                            retry = 0;
                            _res.IsPassed = false;
                            _res.ErrorMessage = "Simulator ISSUE";
                            return _res;
                        }
                    }
                }
                retry = 0;
            }
            catch (Exception ex)
            {
                _res.ErrorMessage = ex.Message;
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            retry = 0;
            return _res;
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}
