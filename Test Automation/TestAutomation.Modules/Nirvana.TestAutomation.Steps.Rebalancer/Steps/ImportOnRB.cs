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
using System.Data.SqlClient;
using System.Configuration;
using Nirvana.TestAutomation.UIAutomation;


namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class ImportOnRB : RebalancerUIMap, IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
                Wait(2000);
                RebalancerTabButton.Click(MouseButtons.Left);
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]);
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement gridElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "RebalancerWindow"));
                IUIAutomationElement ras = gridElement.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["RASExpander"].AutomationUniqueValue));

                if (ras != null)
                {
                    var legacyPatternObj = ras.GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId) as IUIAutomationLegacyIAccessiblePattern;

                    if (legacyPatternObj != null)
                    {
                        IUIAutomationLegacyIAccessiblePattern legacyPattern = (IUIAutomationLegacyIAccessiblePattern)legacyPatternObj;

                        var legacyState = legacyPattern.CurrentDefaultAction;
                        if (!legacyState.ToString().Contains("Collapse"))
                        {
                            GridDataProvider.click(ras);
                            Wait(2000);
                        }
                    }
                }

                IUIAutomationElement actionItem = gridElement.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["ImportRASbutton"].AutomationUniqueValue));
                //BtnImport.Click(MouseButtons.Left);
                if (actionItem != null)
                {
                    IUIAutomationInvokePattern invokePattern = actionItem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                    if (invokePattern != null)
                    {
                        invokePattern.Invoke();
                    }
                }
                Wait(3000);

                actionItem = gridElement.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "1148"));
                if (actionItem != null)
                {
                    IUIAutomationInvokePattern invokePattern = actionItem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                    if (invokePattern != null)
                    {
                        invokePattern.Invoke();
                    }
                    Wait(2000);
                    Keyboard.SendKeys(testData.Tables[0].Rows[0]["FileForImport"].ToString());
                    Wait(2000);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                }
                IUIAutomationCondition buttonCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    "1"
                );

                IUIAutomationElement buttonElement = gridElement.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    buttonCondition
                );

                if (buttonElement != null)
                {
                    IUIAutomationInvokePattern invokePattern = buttonElement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                    if (invokePattern != null)
                    {
                        invokePattern.Invoke();
                    }
                    Wait(2000);
                }
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                if (ApplicationError.IsVisible)
                {
                    ButtonOK4.Click(MouseButtons.Left);
                }
                if (NirvanaAlert1.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
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