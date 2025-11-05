using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.UIAutomation;
using TestAutomationFX.Core;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class RecalculateNIC : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
            TestResult _res = new TestResult();
            try
            {
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_REBALANCER"]);
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]);
               
                if (testData.Tables[0] != null && testData.Tables[0].Rows.Count > 0)
                {
                    uiAutomationHelper.HandleExpandedState(ApplicationArguments.mapdictionary["NAV Impacting Components Groups"].AutomationUniqueValue, ApplicationArguments.mapdictionary["HeaderSite"].AutomationUniqueValue, "ToggleState_On");
                    uiAutomationHelper.CommonAction(testData.Tables[0], "OPEN_REBALANCER", "RebalancerWindow");
                }
                else
                {
                    throw new Exception("RecalculateNIC DataTable is Null");
                }
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                try
                {
                    uiAutomationHelper.HandleExpandedState(ApplicationArguments.mapdictionary["NAV Impacting Components Groups"].AutomationUniqueValue, ApplicationArguments.mapdictionary["HeaderSite"].AutomationUniqueValue, "ToggleState_Off");
                }
                catch { }
            }
            return _res;
        }
    }
}