using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestAutomationFX.Core.UI;

namespace Nirvana.TestAutomation.Steps.Simulator
{
    public class SetManualResponse : CameronSimulator, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                AccessBridgeHelper.Inititalize();
                BringSimToFront();
                ClearUI();
                if (SellSideLog.IsVisible)
                {
                    KeyboardUtilities.CloseWindow(ref TitleBar);
                }
                if (aePrana != null)
                {
                    aePrana.SetFocus();
                }
                AccessBridgeHelper.SendMessage(CameronConstants.menuButtonCommand, CameronConstants.responseButton);
                ExtentionMethods.VerifyAndControlSimulatorAction(ApplicationArguments.JarPath, ApplicationArguments.ScriptFilePath, TestDataConstants.SetResponseTo, TestDataConstants.Manual);
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
            return _res;
        }

    }
}
