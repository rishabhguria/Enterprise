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

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class OperationsonAllocation : AllocationUIMap, IUIAutomationTestStep
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private static CUIAutomation automation = new CUIAutomation();
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenAllocation();
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                DataUtilities.RemoveCommas(testData.Tables[0]);
                string grid = string.Empty;
                if (testData.Tables[0].Rows[0]["Previous Step"].ToString().Equals("SelectAllocatedGroup"))
                {
                    grid = "GridAllocated";
                    testData.Tables[0].Columns.Remove("Previous Step");
                }
                else {
                    grid = "GridUnallocated";
                    testData.Tables[0].Columns.Remove("Previous Step");
                }
                string action = string.Empty;
                foreach (DataColumn dc in testData.Tables[0].Columns) {
                    if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0][dc].ToString())) {
                        action = testData.Tables[0].Rows[0][dc].ToString();
                        break;
                    }
                }

                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                TreeScope.TreeScope_Children,
                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                if (appWindow != null)
                {
                    IUIAutomationElement targetElement = appWindow.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, grid));

                    if (targetElement != null)
                    {
                        var elementRect = targetElement.CurrentBoundingRectangle;

                        System.Drawing.Point clickPoint = new System.Drawing.Point(
                            (int)(elementRect.left + (elementRect.right - elementRect.left) / 2),
                            (int)(elementRect.top + (elementRect.bottom - elementRect.top) / 2));

                        Cursor.Position = clickPoint;
                        MouseRightClick();
                        Wait(2000);

                        IUIAutomationElement contextMenu = automation.GetRootElement().FindFirst(
                        TreeScope.TreeScope_Subtree,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_MenuControlTypeId));

                        if (contextMenu != null)
                        {
                            if (!string.IsNullOrEmpty(action))
                            {
                                IUIAutomationElement menuItem = contextMenu.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, action));

                                if (menuItem != null)
                                {
                                    IUIAutomationInvokePattern invokePattern = menuItem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                                    if (invokePattern != null)
                                    {
                                        invokePattern.Invoke();
                                    }
                                    Wait(2000);
                                }
                                else
                                {
                                    Console.WriteLine("Menu item not found.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Context menu not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Target element not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Application window not found.");
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
        private static void MouseRightClick()
        {
            // Simulate a right-click at the current cursor position
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, (UIntPtr)0);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, (UIntPtr)0);
        }
        
    }
}
