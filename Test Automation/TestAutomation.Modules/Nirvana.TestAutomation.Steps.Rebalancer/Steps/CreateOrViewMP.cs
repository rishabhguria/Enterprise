using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
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
    class CreateOrViewMP : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {

                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]); 
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();

                UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["PranaMain"].AutomationUniqueValue);
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_REBALANCER"]);
                UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue,"6");
                
                uiAutomationHelper.FindAndClickElement(ApplicationArguments.mapdictionary["ModelPortfolioTab"].AutomationUniqueValue);
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
