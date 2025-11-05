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
    public class RejectTrade : CameronSimulator, ITestStep
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
                Wait(800);
                AccessBridgeHelper.SendMessage(CameronConstants.gridCommand, GetTradeIndex(testData.Tables[0]));
                Wait(500);
                AccessBridgeHelper.SendMessage(CameronConstants.buttonCommand, CameronConstants.rejButton);
                if (retry == 2)
                {
                    retry = 0;
                    return _res;
                }
                string result = ExtentionMethods.VerifyAndControlSimulatorAction(ApplicationArguments.JarPath, ApplicationArguments.ScriptFilePath, TestDataConstants.VerifyTradeColourSimulator, TestDataConstants.VerifyRejectedTradeColourSimulator);
                if (result.Contains("Trade is not rejected."))
                {
                    retry++;
                    ITestStep step = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, "Simulator", "RejectTrade");
                    TestResult stepResult = (TestResult)step.RunTest(testData, sheetIndexToName);
                }
                _res.IsPassed = true;
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
    }
}
