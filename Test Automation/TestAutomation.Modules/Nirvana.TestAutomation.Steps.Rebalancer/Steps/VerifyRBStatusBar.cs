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
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Configuration;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.UIAutomation;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class VerifyRBStatusBar: IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["CommonMappings"]); 
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                uiAutomationHelper.CommonAction(testData.Tables[0], "OPEN_REBALANCER", "RebalancerWindow");

                if (!string.IsNullOrEmpty(ApplicationArguments.UIAutomationCommonActionResult) && testData.Tables[0].Columns.Contains("VerifyStatusCompulsory") && !string.IsNullOrEmpty(testData.Tables[0].Rows[0]["VerifyStatusCompulsory"].ToString()))
                {
                    string error = ApplicationArguments.UIAutomationCommonActionResult;
                    ApplicationArguments.UIAutomationCommonActionResult = "";
                    throw new Exception(error);
                }
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


