using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Nirvana.TestAutomation.Steps.PTT
{
    class OperationsonPTT : PTTUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            DataTable ptt = ApplicationArguments.IUIAutomationMappingTables["Window"];
            var dict = SamsaraHelperClass.GetDict(ptt);
            TestResult _result = new TestResult();
            try
            {
                OpenPTT();
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "Window"));
                for (int i = 0; i < testData.Tables[0].Columns.Count; i++) {
                    string columnName = testData.Tables[0].Columns[i].ToString();
                    if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0][columnName].ToString())) {
                        string action = dict[columnName].ToString();
                        IUIAutomationElement actionItem = gridElement.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, action));
                        if (actionItem != null)
                        {
                            Wait(2000);
                            IUIAutomationInvokePattern invokePattern = actionItem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                            if (invokePattern != null)
                            {
                                invokePattern.Invoke();
                            }
                            Wait(2000);
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "OperationsonPTT");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

            return _result;
        }
    }
}
