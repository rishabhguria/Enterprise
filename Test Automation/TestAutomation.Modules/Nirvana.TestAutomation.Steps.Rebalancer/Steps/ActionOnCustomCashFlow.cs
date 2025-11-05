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
    class ActionOnCustomCashFlow : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_REBALANCER"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]);
                try
                {
                    UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                    Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_REBALANCER"]);
                    uiAutomationHelper.FindAndClickElementIfVisible(ApplicationArguments.mapdictionary["btnCustomCashFlow"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["CustomCashFlowWindow"]);
                uiAutomationHelper.CommonAction(testData.Tables[0], "", ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

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

