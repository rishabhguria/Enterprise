using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.UIAutomation;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutomationFX.Core;


namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class ActiononRebalance : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                uiAutomationHelper.CommonAction(testData.Tables[0], "OPEN_REBALANCER", "RebalancerWindow");
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
    }
}