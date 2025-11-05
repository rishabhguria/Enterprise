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
    class SelectUnAssignedAccounts : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (testData.Tables[0] == null || testData.Tables[0].Rows.Count <= 0)
                {
                    throw new Exception("SelectAssignedAccounts failed as DataSet is empty.");
                }
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();

                UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["PranaMain"].AutomationUniqueValue);
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_REBALANCER"]);
                UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                uiAutomationHelper.FindAndClickElement(ApplicationArguments.mapdictionary["CustomGroupsTab"].AutomationUniqueValue);
                bool selectionSucceeded = uiAutomationHelper.CustomGroupDataSelectItem("RebalancerWindow", "RebalancerWindow", testData.Tables[0], "SelectUnAssignedAccounts", "ListItems");

                if (!selectionSucceeded)
                {
                    throw new Exception("SelectAssignedAccounts" + selectionSucceeded);
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
