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
using Nirvana.TestAutomation.Factory;
using System.Diagnostics;


namespace Nirvana.TestAutomation.Steps.Simulator
{
    public class AcknowledgeTrade : CameronSimulator, ITestStep
    {
        private static int retry = 0;
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    if (process.MainWindowTitle.Contains("MS"))
                    {
                        IntPtr hWnd = process.MainWindowHandle;
                        ExtentionMethods.ShowWindow(hWnd, 9);
                    }
                }
                while (retry < 3)
                {
                    BringSimToFront();
                    ExtentionMethods.SwitchToWindowTitle("MS");
                    if (aePrana != null)
                    {
                        aePrana.SetFocus();
                    }
                    AccessBridgeHelper.SendMessage(CameronConstants.gridCommand, GetTradeIndex(testData.Tables[0]));
                    Wait(500);
                    AccessBridgeHelper.SendMessage(CameronConstants.buttonCommand, CameronConstants.ackButton);
                    Wait(1000);
                    if (retry == 2)
                    {
                        retry = 0;
                        _res.IsPassed = false;
                        _res.ErrorMessage = "Simulator ISSUE";
                        return _res;
                    }
                    string result = ExtentionMethods.VerifyAndControlSimulatorAction(ApplicationArguments.JarPath, ApplicationArguments.ScriptFilePath, TestDataConstants.VerifyTradeColourSimulator, TestDataConstants.VerifyAcknowledgeTradeColourSimulator);
                    if (result.Contains("Trade is not acknowledged."))
                    {
                        retry++;
                    }
                    else {
                        break;
                    }
                }
                retry = 0;
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                _res.ErrorMessage = ex.Message;
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
    }
}
