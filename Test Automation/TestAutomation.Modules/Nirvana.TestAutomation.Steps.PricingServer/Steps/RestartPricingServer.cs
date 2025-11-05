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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Diagnostics;
using Nirvana.TestAutomation.Factory;
using CommandType = Nirvana.TestAutomation.Interfaces.Enums.CommandType;

namespace Nirvana.TestAutomation.Steps.PricingServer
{

    class RestartPricingServer : PricingServerUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                /*
              //if option calculator server is visible then close it and restart it
                if (OptionCalculatorServer.IsVisible)
                {
                    Close.Click(MouseButtons.Left);
                    if(Warning.IsVisible)
                        ButtonYes.Click(MouseButtons.Left);
                    OptionCalculatorServer.WaitForVisible();
                }
                //click on start button
                BtnStart.Click(MouseButtons.Left);
                //wait for resonding the server UI
                OptionCalculatorServer.WaitForResponding();
                Minimize.Click(MouseButtons.Left);
                 * */
                if (PranaPricingService2HostApplication.IsVisible)
                {
                    //Process.GetProcessesByName("Prana.PricingService2Host")[0].Kill();
                    ICommandUtilities cmdUtilities = null;
                    cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(CommandType.Bat);
                    cmdUtilities.ExecuteCommand("ShutDownPricingServer.Bat");
                   // cmdUtilities.ExecuteCommand("KillPricing.Bat");
                    Process[] processes = Process.GetProcessesByName("Prana.PricingService2Host");
                    foreach (Process process in processes)
                    {
                        process.Kill();
                    }
                    Wait(1000);
                    PranaPricingService2HostApplication.Start();
                    Wait(3000);
                    PranaPricingServiceMinusv2700.WaitForRespondingOrExited();
                    Wait(5000);
                    KeyboardUtilities.MinimizeWindow(ref TitleBar1);
                    //Minimise.Click(MouseButtons.Left);
                    Wait(7000);
                    
                    //cmdUtilities.ExecuteCommand("ShutdownRelease.Bat");
                }
                return _result;

            }
            catch (Exception ex)
            {
                _result.IsPassed=false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
                return _result;
            }
        }
    }
}
