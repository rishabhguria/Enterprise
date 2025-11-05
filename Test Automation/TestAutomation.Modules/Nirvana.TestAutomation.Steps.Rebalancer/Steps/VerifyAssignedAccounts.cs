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
    class VerifyAssignedAccounts: IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (testData.Tables[0] == null || testData.Tables[0].Rows.Count <= 0)
                {
                    throw new Exception("VerifyAssignedAccounts failed as DataSet is empty.");
                }
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                GridDataProvider gridDataobj = new GridDataProvider();
                UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["PranaMain"].AutomationUniqueValue);
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_REBALANCER"]);
                UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                uiAutomationHelper.FindAndClickElement(ApplicationArguments.mapdictionary["CustomGroupsTab"].AutomationUniqueValue);
                DataSet ds = uiAutomationHelper.GetEditCustomGroupData("RebalancerWindow", "RebalancerWindow");
                List<string> errors = new List<string>();
                List<string> columns = new List<string>();

                bool result = gridDataobj.CheckEmptyGridData(testData.Tables[0], ds.Tables["AssignedAccounts"]);
                if (!result)
                {
                    errors = Recon.RunRecon(ds.Tables["AssignedAccounts"], testData.Tables[0], columns, 0.01);
                    if (errors.Count > 0)
                        _result.ErrorMessage = String.Join("\n\r", errors);
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
