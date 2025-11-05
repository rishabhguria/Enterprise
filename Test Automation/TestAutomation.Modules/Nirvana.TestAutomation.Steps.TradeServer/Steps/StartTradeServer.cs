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


namespace Nirvana.TestAutomation.Steps.TradeServer
{
    public partial class StartTradeServer : TradeServerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                    PranaTradeConsoleHostApplication.Start();
                    Wait(3000);
                    PranaTradeServiceMinusv2600.WaitForRespondingOrExited();
                    Wait(5000);
                    KeyboardUtilities.MinimizeWindow(ref TitleBar2);
                    Wait(7000);
              
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
    }
}
