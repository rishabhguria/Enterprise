using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Diagnostics;
using Nirvana.TestAutomation.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Steps.Simulator;
using Nirvana.TestAutomation.Factory;


namespace Nirvana.TestAutomation.Steps.TradeServer
{
    public class ConnectToSimAutoMan : TradeServerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                
                DataRow dr = testData.Tables[0].Rows[0];
                ShutDownSimulator();
                Wait(5000);
                StartSimulator(testData.Tables[0]);
                Wait(5000);
                PranaTradeServiceUIApplication.Start();
                Wait(6000);
                TradeServiceUI.WaitForRespondingOrExited();
                Wait(6000);
                if (dr[TestDataConstants.COL_BROKER].ToString().ToLower().Equals("ms") )
                {
                    BtnConnect1.Click();
                }
                else if (dr[TestDataConstants.COL_BROKER].ToString().ToLower().Equals("gs"))
                {
                    BtnConnect2.Click();
                }
                else
                {
                    BtnConnect1.Click();
                    BtnConnect2.Click();
                }
                if (TitleBar3.IsVisible)
                {
                    TitleBar3.Click(MouseButtons.Left);
                    KeyboardUtilities.MinimizeWindow(ref TitleBar3);
                
                    Wait(6000);
                }
                ExtentionMethods.VerifyAndControlSimulatorAction(ApplicationArguments.JarPath, ApplicationArguments.ScriptFilePath, TestDataConstants.SetResponseTo, TestDataConstants.Auto);
                return _result;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
                return _result;
            }


        }
        private static void ShutDownSimulator()
        {
            try
            {
                Process[] proc = Process.GetProcessesByName("cmd");
                foreach (Process myProc in proc)
                {
                    if (myProc.MainWindowTitle.Contains("Buy") || myProc.MainWindowTitle.Contains("Sell Side Simulator"))
                    {
                        myProc.CloseMainWindow();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void StartSimulator(DataTable dt)
        {
            try
            {
                CameronSimulator cs = new CameronSimulator();
                cs.StartFixApplication.ImagePath = ApplicationArguments.CameronSimulatorPath + "\\StartFix.exe";
                  cs.StartFixApplication.Start();
                    cs.Config_TT.Click(MouseButtons.Left);
                    Wait(15000);
                    
                    // Working fine without minimizing the window but need to look for right solution
                    //KeyboardUtilities.MinimizeWindow(ref TitleBar3);
                    if (!string.IsNullOrEmpty(dt.Rows[0][TestDataConstants.COL_SETRESPONSE].ToString()) && (dt.Rows[0][TestDataConstants.COL_SETRESPONSE].ToString().Equals("Manual")))
                    {
                        SetManualResponse();
                    }
                    else
                    {
                        ExtentionMethods.VerifyAndControlSimulatorAction(ApplicationArguments.JarPath, ApplicationArguments.ScriptFilePath, TestDataConstants.SetResponseTo, TestDataConstants.Auto);
                    }
                    //
                    AccessBridgeHelper.Inititalize();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        private void SetManualResponse()
        {
            try
            {
                ITestStep SettingManualResp = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, ExcelStructureConstants.CAMERON_SIMULATOR, ExcelStructureConstants.SET_MANUAL_RESP);
                TestResult obj = (TestResult)SettingManualResp.RunTest(null, null);
                if (!obj.IsPassed)
                {
                    Log.Error("Setting manual response failed.");
                }
                ExtentionMethods.VerifyAndControlSimulatorAction(ApplicationArguments.JarPath, ApplicationArguments.ScriptFilePath, TestDataConstants.SetResponseTo, TestDataConstants.Manual);
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
