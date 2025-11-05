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
using System.Threading;
using Nirvana.TestAutomation.UIAutomation;

namespace Nirvana.TestAutomation.Steps.SymbolLookup
{
    class EditSMGrid : IUIAutomationTestStep
    {
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                Thread.Sleep(6000);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                IUIAutomationElement rootElement = automation.GetRootElement();
                IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "SymbolLookUp");
                IUIAutomationElement targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, condition);
                if (targetElement != null)
                {
                    IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                    UIAutomationHelper.SetForegroundWindow(hWnd);
                    uiAutomationHelper.MaximizeWindowAction(targetElement);
                }
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                 TreeScope.TreeScope_Children,
                 automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement securityMaster = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "SymbolLookUp"));
                IUIAutomationElement gridElement = securityMaster.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ColScrollRegion: 0, RowScrollRegion: 0"));

                uiAutomationHelper.EditRowDetails(gridElement, testData.Tables[0]);

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
