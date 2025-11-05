using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Steps;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;
using System;
using System.Runtime.InteropServices;
using UIAutomationClient;


namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class ActionOnTradeBuySellListView : RebalancerUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                Window.BringToFront();
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "TradeBuySellListView"));
                if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0]["Save"].ToString())) {
                    IUIAutomationElement actionItem = gridElement.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnSave"));
                    IUIAutomationInvokePattern invokePattern = actionItem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                    if (invokePattern != null)
                    {
                        invokePattern.Invoke();
                    }
                    Wait(2000);
                    actionItem = gridElement.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "6"));
                    invokePattern = actionItem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                    if (invokePattern != null)
                    {
                        invokePattern.Invoke();
                    }
                    Wait(2000);
                }

                if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0]["SendToStage"].ToString()))
                {
                    IUIAutomationElement actionItem = gridElement.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnSendToStage"));
                    IUIAutomationInvokePattern invokePattern = actionItem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                    if (invokePattern != null)
                    {
                        invokePattern.Invoke();
                    }
                    Wait(2000);
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
