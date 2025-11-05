using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.UIAutomation;
using System.Data;

namespace Nirvana.TestAutomation.Steps.Compliance
{
    public class EnableDisableRule : ComplianceEngineUIMap, IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();

                try
                {
                    //need to add method check for allowedCases here
                    UIAutomationHelper uiAutomationHelperCompliance = new UIAutomationHelper();

                    uiAutomationHelperCompliance.TestScriptEnableDisableRule(testData.Tables[0]);
                   

                }
                catch (Exception ex)
                {
                    SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "EnableDisableRule");
                    _result.IsPassed = false;
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                    if (rethrow)
                        throw;
                }
                finally
                {
                    CloseComplianceEngine();
                }
                return _result;
            }


    }
}
