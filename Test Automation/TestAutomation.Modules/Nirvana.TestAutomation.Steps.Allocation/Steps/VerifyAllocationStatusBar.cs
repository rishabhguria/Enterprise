using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Data;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.UIAutomation;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class VerifyAllocationStatusBar
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (testData.Tables[0] == null || testData.Tables[0].Rows.Count > 0)
                {
                    throw new Exception("VerifyAllocationStatusBar failed as DataTable is empty.");
                }
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                uiAutomationHelper.CommonAction(testData.Tables[0], "OPEN_ALLOCATION", "AllocationClientWindow");
                if (!string.IsNullOrEmpty(ApplicationArguments.UIAutomationCommonActionResult))
                {
                    string error = ApplicationArguments.UIAutomationCommonActionResult;
                    ApplicationArguments.UIAutomationCommonActionResult = "";
                    throw new Exception(error);
                }
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
    }
}
