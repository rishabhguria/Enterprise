using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using TestAutomationFX.Core;
using UIAutomationClient;
using WindowsInput;
using WindowsInput.Native;
using Nirvana.TestAutomation.Utilities;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using TestAutomationFX.UI;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace Nirvana.TestAutomation.UIAutomation
{
    public class UIAutomationHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        private IUIAutomation _uiAutomation;
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        private const uint WM_CLOSE = 0x0010;
        private static string treeViewAutomationId = "ColScrollRegion: 0, RowScrollRegion: 0";
        private const int SW_MINIMIZE = 6;
        private const int SW_MAXIMIZE = 3;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);


        private static void MouseLeftClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        private static void MouseRightClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
        }
        public UIAutomationHelper()
        {
            try
            {
                _uiAutomation = new CUIAutomation();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initializing UIAutomation: " + ex.Message);
            }
        }

        public void VerifyComplianceExportVisibility(string valueToCheckVisibility, bool isdisable)
        {
            try
            {
                ApplicationArguments.mapdictionary = CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["ComplianceEngine"]);

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement complianceEngineElement = null;
                try
                {
                    complianceEngineElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ComplianceAutomationID"].UniquePropertyType, ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    throw new Exception("complianceEngineElement Window not vsisible");

                }

                if (complianceEngineElement != null)
                {
                    DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);

                    IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["PendingApprovalGrid"].AutomationUniqueValue);
                    IUIAutomationElement gridElement = complianceEngineElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition);


                    if (gridElement == null)
                    {
                        Console.WriteLine("Grid element with AutomationId " + ApplicationArguments.mapdictionary["PendingApprovalGrid"].AutomationUniqueValue + "not found.");
                        throw new Exception("Grid element with AutomationId " + ApplicationArguments.mapdictionary["PendingApprovalGrid"].AutomationUniqueValue + "not found.");
                    }
                    else
                    {
                        if (gridElement != null)
                        {
                            Thread.Sleep(4000);
                            MouseOperations.ClickElement(gridElement, "Right");
                            Thread.Sleep(4000);
                        }

                        IUIAutomationElement PendingApprovalGridDropDownMenu = FindElementByUniquePropertyType(automation, complianceEngineElement, ApplicationArguments.mapdictionary["PendingApprovalGridDropDownMenu"].UniquePropertyType, ApplicationArguments.mapdictionary["PendingApprovalGridDropDownMenu"].AutomationUniqueValue);

                        if (PendingApprovalGridDropDownMenu == null)
                        {
                            Console.WriteLine("Grid element with AutomationId " + ApplicationArguments.mapdictionary["PendingApprovalGridDropDownMenu"].AutomationUniqueValue + "not found.");
                            throw new Exception("Grid element with AutomationId " + ApplicationArguments.mapdictionary["PendingApprovalGridDropDownMenu"].AutomationUniqueValue + "not found.");
                        }
                        else
                        {


                            IUIAutomationElementArray childElements = PendingApprovalGridDropDownMenu.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                            if (childElements != null)
                            {

                                for (int i = 0; i < childElements.Length; i++)
                                {
                                    IUIAutomationElement childElement = childElements.GetElement(i);
                                    string elementName = childElement.CurrentName;
                                    string elementAutomationId = childElement.CurrentAutomationId;
                                    Console.WriteLine("Name: " + elementName + " AutomationId: " + elementAutomationId);

                                    if (string.Equals(elementName, valueToCheckVisibility, StringComparison.OrdinalIgnoreCase) || string.Equals(elementAutomationId, valueToCheckVisibility, StringComparison.OrdinalIgnoreCase))
                                    {

                                        IUIAutomationLegacyIAccessiblePattern legacyPatternObj =
               childElement.GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId) as IUIAutomationLegacyIAccessiblePattern;

                                        if (legacyPatternObj != null)
                                        {
                                            uint accessibleStateUInt = legacyPatternObj.CurrentState;
                                            Console.WriteLine("LegacyIAccessible.State: " + accessibleStateUInt.ToString("X"));
                                            bool isUnavailable = (accessibleStateUInt & 0x1) != 0;

                                            if (isUnavailable)
                                            {
                                                if (isdisable)
                                                    Console.WriteLine("The element is disabled (unavailable state) Verfied.");
                                                else
                                                {
                                                    throw new Exception("The element is disabled (unavailable state) in UI. But query states enabled");
                                                }
                                            }
                                            else
                                            {
                                                if (!isdisable)
                                                {
                                                    Console.WriteLine("The element is enabled Verfied.");
                                                }
                                                else
                                                {
                                                    throw new Exception("The element is enabled (available state) in UI. But query states disabled");
                                                }

                                            }

                                        }
                                        else
                                        {
                                            Console.WriteLine("LegacyIAccessiblePattern is not available for this element.");
                                        }
                                    }

                                }

                            }


                        }
                    }



                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IUIAutomationElement GetRootElement()
        {
            try
            {
                return _uiAutomation.GetRootElement();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting root element: " + ex.Message);
                return null;
            }
        }






        public void ClickButton(IUIAutomationElement buttonElement)
        {
            try
            {
                try
                {
                    var elementRect = buttonElement.CurrentBoundingRectangle;
                    Point clickPoint = new Point(
                                           (int)(elementRect.left + (elementRect.right - elementRect.left) / 2),
                                           (int)(elementRect.top + (elementRect.bottom - elementRect.top) / 2));

                    Cursor.Position = clickPoint;

                    MouseLeftClick();
                    Console.WriteLine("Button clicked using mouse coordinates.");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Button clicked using mouse coordinates failed now using invoke");
                }


                IUIAutomationInvokePattern invokePattern = (IUIAutomationInvokePattern)buttonElement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                if (invokePattern != null)
                {
                    invokePattern.Invoke();
                    Console.WriteLine("Button clicked.");
                }
                else
                {
                    Console.WriteLine("Button does not support InvokePattern.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error clicking button: " + ex.Message);
            }
        }

        public void SetTextBoxText(IUIAutomationElement textBoxElement, string text)
        {
            try
            {
                IUIAutomationValuePattern valuePattern = (IUIAutomationValuePattern)textBoxElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                if (valuePattern != null)
                {
                    valuePattern.SetValue(text);
                    Console.WriteLine("Text set: " + text);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                else
                {
                    Console.WriteLine("Text box does not support ValuePattern.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error setting text in text box: " + ex.Message);
                Console.WriteLine("Trying UI Interaction ");



            }
        }
        public bool TestScriptActiononButton(DataTable dt, Dictionary<string, AutomationUniqueControlType> mapdictionary)
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement windowElement = null;
                try
                {
                    windowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error finding Widdow element: {0}", ex.Message));

                    return false;
                }

                if (windowElement != null)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn col in dt.Columns)
                        {
                            var value = dr[col];
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                Console.WriteLine(col.ColumnName + ": " + value);
                                if (mapdictionary.ContainsKey(col.ColumnName))
                                {
                                    IUIAutomationElement targetElement = FindElementByUniquePropertyType(automation, windowElement, ApplicationArguments.mapdictionary[col.ColumnName].UniquePropertyType, ApplicationArguments.mapdictionary[col.ColumnName].AutomationUniqueValue);
                                    if (targetElement != null)
                                    {
                                        try
                                        {
                                            DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                                            Thread.Sleep(2000);
                                            MouseOperations.ClickElement(targetElement, "Left");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);

                                        }
                                        return true;
                                    }
                                }
                            }
                        }
                    }

                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in TestScriptActiononButton " + ex.Message);
                return false;
            }
            return true;
        }
        public bool SelectDateAtMPFC(DataTable dt, Dictionary<string, AutomationUniqueControlType> mapdictionary)
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement windowElement = null;
                try
                {
                    windowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error finding Widdow element: {0}", ex.Message));

                    return false;
                }

                if (windowElement != null)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn col in dt.Columns)
                        {
                            var value = dr[col];
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                Console.WriteLine(col.ColumnName + ": " + value);
                                if (mapdictionary.ContainsKey(col.ColumnName))
                                {
                                    IUIAutomationElement targetElement = FindElementByUniquePropertyType(automation, windowElement, ApplicationArguments.mapdictionary[col.ColumnName].UniquePropertyType, ApplicationArguments.mapdictionary[col.ColumnName].AutomationUniqueValue);
                                    if (targetElement != null)
                                    {
                                        try
                                        {
                                            DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                                            Thread.Sleep(2000);

                                            if (string.Equals(col.ColumnName, "Select Date") || string.Equals(col.ColumnName, "Copy From") || ApplicationArguments.mapdictionary["InputColumns"].AutomationUniqueValue.Contains(col.ColumnName))
                                            {

                                                string tempDate = DataUtilities.DateHandler(dr[col].ToString());
                                                string tempText = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(tempDate));
                                                try
                                                {
                                                    MouseOperations.ClickElement(targetElement, "Left");
                                                    Thread.Sleep(3000);

                                                    //ClearText(targetElement, automation);

                                                    //bool success = EnterTextAndWaitForMatch(
                                                    //                                       automation,
                                                    //                                       targetElement,
                                                    //                                       ApplicationArguments.mapdictionary[col.ColumnName].UniquePropertyType,
                                                    //                                       ApplicationArguments.mapdictionary[col.ColumnName].AutomationUniqueValue,
                                                    //                                       dr[col].ToString(), 
                                                    //                                       maxAttempts: 10, 
                                                    //                                       pollIntervalMilliseconds: 500 
                                                    //                                   );


                                                    SetValueUsingValuePattern(targetElement, tempText);

                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                            }
                                            else
                                            {
                                                Thread.Sleep(2000);
                                                MouseOperations.ClickElement(targetElement, "Left");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);

                                        }

                                    }
                                }
                            }
                        }
                    }
                    IUIAutomationElement btnFetchData = FindElementByUniquePropertyType(automation, windowElement, ApplicationArguments.mapdictionary["btnFetchData"].UniquePropertyType, ApplicationArguments.mapdictionary["btnFetchData"].AutomationUniqueValue);
                    if (btnFetchData != null)
                    {
                        try
                        {
                            MouseOperations.ClickElement(btnFetchData, "Left", true);



                        }


                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                        }
                    }


                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in  SelectDateAtMPFC" + ex.Message);
                return false;
            }
            return true;
        }

        public void ExpandElement(IUIAutomationElement element)
        {
            try
            {
                IUIAutomationExpandCollapsePattern expandPattern = (IUIAutomationExpandCollapsePattern)element.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
                if (expandPattern != null)
                {
                    expandPattern.Expand();
                    Console.WriteLine("Element expanded.");
                }
                else
                {
                    Console.WriteLine("Element does not support ExpandCollapsePattern.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error expanding element: " + ex.Message);
            }
        }

        public string GetElementText(IUIAutomationElement element)
        {
            try
            {
                IUIAutomationTextPattern textPattern = (IUIAutomationTextPattern)element.GetCurrentPattern(UIA_PatternIds.UIA_TextPatternId);
                if (textPattern != null)
                {
                    return textPattern.DocumentRange.GetText(-1);
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting element text: " + ex.Message);
                return string.Empty;
            }
        }

        public static bool DetectAndSwitchWindow(string automationId, string waitAfterSwitch = "3")
        {
            try
            {
                Thread.Sleep(3000);
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = null;
                if (automationId.Equals("PranaMain"))
                {
                    rootElement = automation.GetRootElement();
                }
                else
                {
                    rootElement = automation.GetRootElement().FindFirst(
                                    TreeScope.TreeScope_Children,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));

                }
                IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, automationId);
                IUIAutomationElement targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, condition);

                if (targetElement != null)
                {
                    IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                    SetForegroundWindow(hWnd);
                    int waitTime = int.TryParse(waitAfterSwitch, out waitTime) ? waitTime : 3;
                    System.Threading.Thread.Sleep(waitTime * 1000);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                return false;
            }
        }
        public static void OpenModule(string targetWindowID, string module, string type = "button", string menuItemID = "")
        {
            try
            {
                // Initialize InputSimulator
                var inputSimulator = new InputSimulator();

                // Detect and switch to the target window
                bool isApplicationWindowVisible = DetectAndSwitchWindow("PranaMain");

                if (isApplicationWindowVisible)
                {
                    string selectedModule = module;
                    string shortcut;

                    if (selectedModule.Contains("Blotter"))
                    {
                        shortcut = ConfigurationManager.AppSettings["OPEN_BLOTTER"];
                        SendShortcut(inputSimulator, shortcut);
                    }
                    else if (selectedModule.Contains("Allocation"))
                    {
                        shortcut = ConfigurationManager.AppSettings["OPEN_ALLOCATION"];
                        SendShortcut(inputSimulator, shortcut);
                    }
                    else if (selectedModule.Contains("PTT"))
                    {
                        shortcut = ConfigurationManager.AppSettings["OPEN_PTT"];
                        SendShortcut(inputSimulator, shortcut);
                    }
                    else if (selectedModule.Contains("Rebalancer"))
                    {
                        shortcut = ConfigurationManager.AppSettings["OPEN_REBALANCER"];
                        SendShortcut(inputSimulator, shortcut);
                    }
                    else if (selectedModule.Contains("Watchlist"))
                    {
                        shortcut = ConfigurationManager.AppSettings["OPEN_WATCHLIST"];
                        SendShortcut(inputSimulator, shortcut);
                    }
                    else if (selectedModule.Contains("OptChain"))
                    {
                        shortcut = ConfigurationManager.AppSettings["OPEN_OPT_CHAIN"];
                        SendShortcut(inputSimulator, shortcut);
                    }
                    else if (selectedModule.Contains("Compliance"))
                    {
                        shortcut = ConfigurationManager.AppSettings["OPEN_COM_ENGINE"];
                        SendShortcut(inputSimulator, shortcut);
                    }
                    else if (selectedModule.Contains("PortfolioManagement"))
                    {
                        shortcut = ConfigurationManager.AppSettings["OPEN_PM"];
                        SendShortcut(inputSimulator, shortcut);
                    }
                    else if (selectedModule.Contains("Closing"))
                    {
                        shortcut = ConfigurationManager.AppSettings["OPEN_CLOSING"];
                        SendShortcut(inputSimulator, shortcut);
                    }
                }
                else
                {
                    throw new Exception("Main Application window not available, please check.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening module: " + ex.Message);
            }
        }
        private static void SendShortcut(InputSimulator inputSimulator, string shortcut)
        {
            if (!string.IsNullOrEmpty(shortcut))
            {
                // Remove brackets if present
                shortcut = shortcut.Trim('[', ']');

                // Split the shortcut keys by "+" symbol
                string[] keys = shortcut.Split('+');

                var keyDownList = new List<VirtualKeyCode>();

                // Identify modifiers and main keys to press
                foreach (string key in keys)
                {
                    switch (key.ToUpper())
                    {
                        case "CTRL":
                            keyDownList.Add(VirtualKeyCode.CONTROL);
                            break;
                        case "SHIFT":
                            keyDownList.Add(VirtualKeyCode.SHIFT);
                            break;
                        case "ALT":
                            keyDownList.Add(VirtualKeyCode.MENU); // VirtualKeyCode.MENU represents ALT
                            break;
                        default:
                            // Try parsing alphabet keys and numeric keys
                            if (key.Length == 1 && char.IsLetterOrDigit(key[0]))
                            {
                                keyDownList.Add((VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), "VK_" + key.ToUpper()));
                            }
                            else
                            {
                                Console.WriteLine(string.Format("Unsupported key: {0}", key));

                            }
                            break;
                    }
                }

                // Press all keys in the list
                foreach (var key in keyDownList)
                {
                    inputSimulator.Keyboard.KeyDown(key);
                }

                foreach (var key in keyDownList)
                {
                    inputSimulator.Keyboard.KeyUp(key);
                }
            }
        }


        public static List<IUIAutomationElement> FindElementsByUniquePropertyType(
                IUIAutomation automation,
                IUIAutomationElement parentElement,
                string uniquePropertyType,
                string automationUniqueValue)
        {
            try
            {
                IUIAutomationCondition condition = null;
                if (string.IsNullOrEmpty(uniquePropertyType) || uniquePropertyType.Equals("AutomationId", StringComparison.OrdinalIgnoreCase))
                {
                    condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, automationUniqueValue);
                }
                else if (uniquePropertyType.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, automationUniqueValue);
                }

                if (condition != null)
                {
                    IUIAutomationElementArray elementArray = parentElement.FindAll(TreeScope.TreeScope_Descendants, condition);

                    List<IUIAutomationElement> elements = new List<IUIAutomationElement>();
                    for (int i = 0; i < elementArray.Length; i++)
                    {
                        elements.Add(elementArray.GetElement(i));
                    }

                    return elements;
                }

                return new List<IUIAutomationElement>();
            }
            catch (Exception ex)
            {
                return new List<IUIAutomationElement>();
            }
        }


        public static IUIAutomationElement FindElementByUniquePropertyType(IUIAutomation automation, IUIAutomationElement parentElement, string uniquePropertyType, string automationUniqueValue, string controltype = "")
        {
            try
            {
                IUIAutomationCondition condition = null;
                if (!controltype.Equals(""))
                {
                    IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId, automationUniqueValue);

                    IUIAutomationCondition controlTypeCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TabItemControlTypeId);

                    IUIAutomationCondition combinedCondition = automation.CreateAndCondition(nameCondition, controlTypeCondition);

                    IUIAutomationElement foundElement = parentElement.FindFirst(TreeScope.TreeScope_Subtree, combinedCondition);
                    return foundElement;

                }
                if (string.IsNullOrEmpty(uniquePropertyType) || uniquePropertyType.Equals("AutomationId", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(parentElement.CurrentAutomationId, automationUniqueValue, StringComparison.OrdinalIgnoreCase))
                    {
                        return parentElement;
                    }
                    condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, automationUniqueValue);
                }
                else if (uniquePropertyType.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(parentElement.CurrentName, automationUniqueValue, StringComparison.OrdinalIgnoreCase))
                    {
                        return parentElement;
                    }
                    condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, automationUniqueValue);
                }

                if (condition != null)
                {
                   
                    return parentElement.FindFirst(TreeScope.TreeScope_Descendants, condition);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error finding element by {0}: {1}", uniquePropertyType, ex.Message));

                return null;
            }
        }

        public static Dictionary<string, AutomationUniqueControlType> CreateDictionaryFromDataTable(DataTable table)
        {
            var dictionary = new Dictionary<string, AutomationUniqueControlType>();

            foreach (DataRow row in table.Rows)
            {
                try
                {
                    if (row["DataName"] != DBNull.Value && !string.IsNullOrEmpty(row["DataName"].ToString()) &&
                        row["AutomationUniqueValue"] != DBNull.Value && !string.IsNullOrEmpty(row["AutomationUniqueValue"].ToString()))
                    {
                        string dataName = row["DataName"].ToString();
                        string automationUniqueValue = row["AutomationUniqueValue"].ToString();
                        string uniquePropertyType = row["UniquePropertyType"] != null ? row["UniquePropertyType"].ToString() : string.Empty;
                        string controlType = row["ControlType"] != null ? row["ControlType"].ToString() : string.Empty;


                        // Add to dictionary
                        dictionary[dataName] = new AutomationUniqueControlType
                        {
                            AutomationUniqueValue = automationUniqueValue,
                            UniquePropertyType = uniquePropertyType,
                            ControlType = controlType
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error processing row with DataName: {0}. Exception: {1}", row["DataName"], ex.Message));
                }
            }

            return dictionary;
        }

        public static void SetValueUsingValuePattern(IUIAutomationElement element, string value)
        {
            if (element == null)
                return;

            try
            {
                object valuePatternObj = null;
                valuePatternObj = element.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;
                if (valuePattern != null)
                {
                    valuePattern.SetValue(value);
                    Console.WriteLine("Set the value to " + value + " for element " + element.CurrentName + ".");
                }
                else
                {
                    throw new InvalidOperationException("Element does not support ValuePattern.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error setting value: " + ex.Message);

            }
        }
        public bool TestScriptEnableDisableRule(DataTable table)
        {
            try
            {
                //OpenModule("ComplianceEngine", "Compliance", "button");
                ApplicationArguments.mapdictionary = CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["ComplianceEngine"]);
                foreach (var entry in ApplicationArguments.mapdictionary)
                {
                    Console.WriteLine(string.Format("DataName: {0}, AutomationUniqueValue: {1}, UniquePropertyType: {2}, ControlType: {3}",
                     entry.Key,
                     entry.Value.AutomationUniqueValue,
                     entry.Value.UniquePropertyType,
                     entry.Value.ControlType));

                }
                OpenModule(ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue, ApplicationArguments.mapdictionary["Module"].AutomationUniqueValue, ApplicationArguments.mapdictionary["PranaType"].AutomationUniqueValue);




                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement complianceEngineElement = null;
                try
                {
                    complianceEngineElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ComplianceAutomationID"].UniquePropertyType, ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error finding ComplianceEngine element: {0}", ex.Message));

                    return false;
                }

                if (complianceEngineElement != null)
                {
                    DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                    IUIAutomationElement targetElementTab = FindElementByUniquePropertyType(automation, complianceEngineElement, ApplicationArguments.mapdictionary["RuleDefinition"].UniquePropertyType, ApplicationArguments.mapdictionary["RuleDefinition"].AutomationUniqueValue);
                    if (targetElementTab != null)
                    {
                        Thread.Sleep(4000);
                        MouseOperations.ClickElement(targetElementTab, "Left");




                        IUIAutomationElement expandButton = FindElementByUniquePropertyType(automation, complianceEngineElement, ApplicationArguments.mapdictionary["ExpandButton"].UniquePropertyType, ApplicationArguments.mapdictionary["ExpandButton"].AutomationUniqueValue);
                        if (expandButton != null)
                        {
                            MouseOperations.ClickElement(expandButton, "Left");
                        }
                        Thread.Sleep(4000);

                        IUIAutomationElement textSearch = FindElementByUniquePropertyType(automation, complianceEngineElement, ApplicationArguments.mapdictionary["TextSearch"].UniquePropertyType, ApplicationArguments.mapdictionary["TextSearch"].AutomationUniqueValue);

                        MouseOperations.ClickElement(textSearch, "Left");

                        IUIAutomationElement innertextSearch = textSearch.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["textboxInnerElement"].AutomationUniqueValue));

                        bool result = true;
                        string foundElementValue = ControlTypeHandler.getValueOfElement(textSearch, ref result);
                        if (!string.IsNullOrEmpty(foundElementValue))
                        {
                            while (!string.IsNullOrEmpty(foundElementValue))
                            {
                                DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                                ClearText(innertextSearch, automation);
                                foundElementValue = ControlTypeHandler.getValueOfElement(textSearch, ref result);
                            }
                        }

                        bool isDisableAllComplianceRulesCompleted = DisableAllComplianceRules(complianceEngineElement, automation);//first disable all rules
                        DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                        try
                        {
                            complianceEngineElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ComplianceAutomationID"].UniquePropertyType, ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                        }
                        catch
                        {
                            return false;
                        }
                        textSearch = FindElementByUniquePropertyType(automation, complianceEngineElement, ApplicationArguments.mapdictionary["TextSearch"].UniquePropertyType, ApplicationArguments.mapdictionary["TextSearch"].AutomationUniqueValue);
                        if (isDisableAllComplianceRulesCompleted && textSearch != null)
                        {

                            try
                            {
                                /*if (textSearch.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) is IUIAutomationValuePattern valuePattern)
                                {
                                    valuePattern.SetValue("Test4");
                                }*/


                                foreach (DataRow dr in table.Rows)
                                {
                                    if (!string.IsNullOrEmpty(dr["RuleName"].ToString()))
                                        EnableDisableRule(dr, complianceEngineElement, automation);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(string.Format("Error setting value on textSearch: {0}", ex.Message));

                                return false;
                            }



                        }
                        else
                        {
                            return false;

                        }
                        MouseOperations.ClickElement(expandButton, "Left");




                    }
                }
                DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                IUIAutomationElement DockTop = FindElementByUniquePropertyType(automation, complianceEngineElement, ApplicationArguments.mapdictionary["DockTop"].UniquePropertyType, ApplicationArguments.mapdictionary["DockTop"].AutomationUniqueValue);

                MouseOperations.ClickElement(DockTop, "Right");
                var inputSimulator = new InputSimulator();
                inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F4);



            }
            catch (Exception ex)
            {
                Console.WriteLine("Error enabling/disabling rule: " + ex.Message);
                return false;
            }

            return true;
        }
        public bool DisableAllComplianceRules(IUIAutomationElement complianceEngineElement, IUIAutomation automation)
        {
            try
            {
                if (complianceEngineElement != null)
                {
                    DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);

                    Thread.Sleep(6000);
                    IUIAutomationElement pretradeUserDefined = FindElementByUniquePropertyType(automation, complianceEngineElement, ApplicationArguments.mapdictionary["PreTradeUserDefined"].UniquePropertyType, ApplicationArguments.mapdictionary["PreTradeUserDefined"].AutomationUniqueValue);
                    Thread.Sleep(10000);
                    if (pretradeUserDefined != null)
                    {


                        try
                        {

                            MouseOperations.ClickElement(pretradeUserDefined, "Right");
                            Thread.Sleep(3000);
                            try
                            {
                                IUIAutomationElement DropDownDisableAllRules = complianceEngineElement.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, ApplicationArguments.mapdictionary["DropDownDisableAllRules"].AutomationUniqueValue));

                                if (DropDownDisableAllRules == null)
                                {
                                    return true;
                                }

                                MouseOperations.ClickElement(DropDownDisableAllRules, "Left");


                                IUIAutomationCondition ComplianceDiaglogueBox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, ApplicationArguments.mapdictionary["ComplianceDiaglogueBox"].AutomationUniqueValue);


                                IUIAutomationElement complianceDialogueBox = UIAutomationHelper.FluentWaitForElement(
     complianceEngineElement,
     automation,
     UIA_PropertyIds.UIA_NamePropertyId,
     ApplicationArguments.mapdictionary["ComplianceDiaglogueBox"].AutomationUniqueValue
 );

                                if (complianceDialogueBox != null)
                                {
                                    //DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                                    MouseOperations.ClickElement(complianceDialogueBox, "Left");
                                    Console.WriteLine("Compliance Dialogue Box found.");
                                    Thread.Sleep(3000);
                                    // Proceed with actions on complianceDialogueBox
                                    IUIAutomationElement complianceDialogueBoxButtonOK = complianceDialogueBox.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, ApplicationArguments.mapdictionary["ComplianceDiaglogueBoxOKButton"].AutomationUniqueValue));
                                    MouseOperations.ClickElement(complianceDialogueBoxButtonOK, "Left");
                                }
                                else
                                {
                                    Console.WriteLine("Compliance Dialogue Box not found within 6 minutes.Retrying with hard code way");
                                    DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ComplianceMainWindow"].AutomationUniqueValue);
                                    IUIAutomationElement rootElement = automation.GetRootElement();
                                    complianceEngineElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ComplianceAutomationID"].UniquePropertyType, ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                                    IUIAutomationElement complianceDialogue = complianceEngineElement.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, ApplicationArguments.mapdictionary["ComplianceDiaglogueBoxError"].AutomationUniqueValue));
                                    MouseOperations.ClickElement(complianceDialogue, "Left");
                                    IUIAutomationElement complianceDialogueBoxButtonOK = complianceDialogue.FindFirst(
                   TreeScope.TreeScope_Descendants,
                   automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, ApplicationArguments.mapdictionary["ComplianceDiaglogueBoxOKButton"].AutomationUniqueValue));
                                    MouseOperations.ClickElement(complianceDialogueBoxButtonOK, "Left");


                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(string.Format("Error setting value on textSearch: {0}", ex.Message));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("Error setting value on textSearch: {0}", ex.Message));
                            return false;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error enabling/disabling rule: " + ex.Message);
                return false;
            }

            return true;
        }
        public static IUIAutomationElement FluentWaitForElement(
     IUIAutomationElement parentElement,
     IUIAutomation automation,
     int propertyId,
     string propertyValue,
     int timeoutSeconds = 360,
     int pollIntervalMilliseconds = 500)
        {
            DateTime endTime = DateTime.Now.AddSeconds(timeoutSeconds);
            IUIAutomationElement foundElement = null;

            while (DateTime.Now < endTime)
            {
                try
                {
                    foundElement = parentElement.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(propertyId, propertyValue)
                    );

                    if (foundElement != null)
                    {
                        return foundElement;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Exception while waiting for element (Retrying): {0}", ex.Message));
                }

                Thread.Sleep(pollIntervalMilliseconds);
            }

            Console.WriteLine(string.Format("Element with Property ID: {0} and Value: {1} not found within {2} seconds.", propertyId, propertyValue, timeoutSeconds));
            return null;
        }

        public static bool EnableDisableRule(DataRow dr, IUIAutomationElement complianceEngineElement, IUIAutomation automation)
        {
            try
            {

                //   DisableAllComplianceRules();
                /* IUIAutomation automation = new CUIAutomation();
                 IUIAutomationElement rootElement = automation.GetRootElement();

                 IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ComplianceEngine");
                 IUIAutomationElement complianceEngineElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, condition);
 */
                if (complianceEngineElement != null)
                {
                    var inputSimulator = new InputSimulator();
                    DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                    IUIAutomationElement targetElementTab = complianceEngineElement.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["RuleDefinition"].AutomationUniqueValue));

                    if (targetElementTab != null)
                    {
                        IUIAutomationElement textSearch = FindElementByUniquePropertyType(automation, complianceEngineElement, ApplicationArguments.mapdictionary["TextSearch"].UniquePropertyType, ApplicationArguments.mapdictionary["TextSearch"].AutomationUniqueValue);
                        IUIAutomationElement innertextSearch = textSearch.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["textboxInnerElement"].AutomationUniqueValue));

                        bool result = true;
                        string foundElementValue = ControlTypeHandler.getValueOfElement(textSearch, ref result);
                        if (!string.IsNullOrEmpty(foundElementValue))
                        {
                            while (!string.IsNullOrEmpty(foundElementValue))
                            {
                                DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                                ClearText(innertextSearch, automation);
                                foundElementValue = ControlTypeHandler.getValueOfElement(textSearch, ref result);
                            }
                        }

                        MouseOperations.ClickElement(textSearch, "Left");
                        /*inputSimulator.Keyboard.TextEntry("Test4");*/
                        bool success = EnterTextAndWaitForMatch(
        automation,
        textSearch,
        ApplicationArguments.mapdictionary["textboxInnerElement"].UniquePropertyType,
        ApplicationArguments.mapdictionary["textboxInnerElement"].AutomationUniqueValue,
        dr["RuleName"].ToString(), // The text to enter
        maxAttempts: 10, // Max attempts to enter the text
        pollIntervalMilliseconds: 500 // Polling interval (adjust as necessary)
    );

                        Console.WriteLine(foundElementValue);
                        Console.WriteLine(foundElementValue);



                        string ruleTradeType = dr["TradeType"].ToString();
                        string ruleType = dr["RuleType"].ToString();
                        string ruleState = dr["RuleState"].ToString();


                        if (string.Equals(ruleTradeType, "Pre Trade", StringComparison.OrdinalIgnoreCase))
                        {
                            IUIAutomationElement userDefinedElement = complianceEngineElement.FindFirst(
                                TreeScope.TreeScope_Descendants,
                                automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["PreTradeUserDefined"].AutomationUniqueValue));


                            if (userDefinedElement != null)
                            {

                                IUIAutomationElementArray childElements = userDefinedElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                                if (childElements != null)
                                {

                                    for (int i = 0; i < childElements.Length; i++)
                                    {
                                        IUIAutomationElement childElement = childElements.GetElement(i);
                                        string elementName = childElement.CurrentName;
                                        string elementAutomationId = childElement.CurrentAutomationId;
                                        Console.WriteLine("Name: " + elementName + " AutomationId: " + elementAutomationId);

                                        if (string.Equals(elementName, dr["RuleName"].ToString(), StringComparison.OrdinalIgnoreCase))
                                        {
                                            MouseOperations.ClickElement(childElement, "Right");
                                            Thread.Sleep(3000);

                                            if (string.Equals(ruleState, "Enable", StringComparison.OrdinalIgnoreCase))
                                            {
                                                IUIAutomationElement EnableRule = complianceEngineElement.FindFirst(
                                    TreeScope.TreeScope_Descendants,
                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, ApplicationArguments.mapdictionary["DropDownEnableRule"].AutomationUniqueValue));

                                                MouseOperations.ClickElement(EnableRule, "Left");
                                            }
                                            else
                                            {
                                                IUIAutomationElement EnableRule = complianceEngineElement.FindFirst(
                                   TreeScope.TreeScope_Descendants,
                                   automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, ApplicationArguments.mapdictionary["DropDownDisableRule"].AutomationUniqueValue));


                                                MouseOperations.ClickElement(EnableRule, "Left");
                                            }

                                        }
                                    }



                                    IUIAutomationCondition ComplianceDiaglogueBox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, ApplicationArguments.mapdictionary["ComplianceDiaglogueBox"].AutomationUniqueValue);


                                    IUIAutomationElement complianceDialogueBox = UIAutomationHelper.FluentWaitForElement(
         complianceEngineElement,
         automation,
         UIA_PropertyIds.UIA_NamePropertyId,
         ApplicationArguments.mapdictionary["ComplianceDiaglogueBox"].AutomationUniqueValue
     );

                                    if (complianceDialogueBox != null)
                                    {
                                        //DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                                        MouseOperations.ClickElement(complianceDialogueBox, "Left");
                                        Console.WriteLine("Compliance Dialogue Box found.");
                                        Thread.Sleep(3000);
                                        // Proceed with actions on complianceDialogueBox
                                        IUIAutomationElement complianceDialogueBoxButtonOK = complianceDialogueBox.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, ApplicationArguments.mapdictionary["ComplianceDiaglogueBoxOKButton"].AutomationUniqueValue));
                                        MouseOperations.ClickElement(complianceDialogueBoxButtonOK, "Left");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Compliance Dialogue Box not found within 6 minutes.Retrying with hard code way");
                                        DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ComplianceMainWindow"].AutomationUniqueValue);
                                        IUIAutomationElement rootElement = automation.GetRootElement();
                                        complianceEngineElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ComplianceAutomationID"].UniquePropertyType, ApplicationArguments.mapdictionary["ComplianceAutomationID"].AutomationUniqueValue);
                                        IUIAutomationElement complianceDialogue = complianceEngineElement.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, ApplicationArguments.mapdictionary["ComplianceDiaglogueBoxError"].AutomationUniqueValue));
                                        MouseOperations.ClickElement(complianceDialogue, "Left");
                                        IUIAutomationElement complianceDialogueBoxButtonOK = complianceDialogue.FindFirst(
                       TreeScope.TreeScope_Descendants,
                       automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, ApplicationArguments.mapdictionary["ComplianceDiaglogueBoxOKButton"].AutomationUniqueValue));
                                        MouseOperations.ClickElement(complianceDialogueBoxButtonOK, "Left");


                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error enabling/disabling rule: " + ex.Message);
                return false;
            }
        }

        public static void ClearText(IUIAutomationElement textSearch, IUIAutomation automation, int maxAttempts = 10)
        {
            
            int attemptCount = 0;

            try
            {
                while (attemptCount < maxAttempts)
                {
                    attemptCount++;

                    var inputSimulator = new InputSimulator();
                    MouseOperations.ClickElement(textSearch, "Left");
                    Thread.Sleep(1000);
                    inputSimulator.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL }, new[] { VirtualKeyCode.VK_A });
                    Thread.Sleep(1000);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.DELETE);
                    MouseOperations.ClickElement(textSearch, "Left");
                    PressKeys(inputSimulator, VirtualKeyCode.BACK, 10);
                    PressKeys(inputSimulator, VirtualKeyCode.DELETE, 10);
                    for (int i = 0; i < 5; i++)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
                    }

                    var valuePattern = textSearch.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                    if (valuePattern != null)
                    {
                        valuePattern.SetValue(string.Empty);
                    }
                }

                if (attemptCount >= maxAttempts)
                {
                    Console.WriteLine("Maximum number of attempts reached for clearing text.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error clearing text: {0}", ex.Message));
            }
        }

        private static void PressKeys(InputSimulator inputSimulator, VirtualKeyCode key, int count)
        {
            for (int i = 0; i < count; i++)
            {
                inputSimulator.Keyboard.KeyPress(key);
            }
        }

        public static bool EnterTextAndWaitForMatch(
          IUIAutomation automation,
          IUIAutomationElement parentElement,
          string uniquePropertyType,
          string automationUniqueValue,
          string expectedValue,
          int maxAttempts = 10,
          int pollIntervalMilliseconds = 500)
        {
            var inputSimulator = new InputSimulator();
            try
            {
                IUIAutomationElement textSearch = FindElementByUniquePropertyType(
                    automation,
                    parentElement,
                    uniquePropertyType,
                    automationUniqueValue
                );

                if (textSearch == null)
                {
                    Console.WriteLine("TextSearch element not found.");
                    return false;
                }

                bool result = true;
                string currentValue = ControlTypeHandler.getValueOfElement(textSearch, ref result);
                int attempts = 0;

                while (currentValue != expectedValue && attempts < maxAttempts)
                {
                    MouseOperations.ClickElement(textSearch, "Left");


                    inputSimulator.Keyboard.TextEntry(expectedValue);
                    currentValue = ControlTypeHandler.getValueOfElement(textSearch, ref result);
                    attempts++;
                    Thread.Sleep(pollIntervalMilliseconds);
                }

                if (currentValue == expectedValue)
                {
                    Console.WriteLine("Text successfully entered: " + currentValue);
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to enter the expected text after " + maxAttempts + " attempts.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }
        public void ValidateImportPositions(string moduleID, DataTable dt)
        {
            try
            {
                ApplicationArguments.mapdictionary = CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[moduleID]);
                foreach (var entry in ApplicationArguments.mapdictionary)
                {
                    Console.WriteLine("DataName: " + entry.Key + ", AutomationUniqueValue: " + entry.Value.AutomationUniqueValue +
                    ", UniquePropertyType: " + entry.Value.UniquePropertyType + ", ControlType: " + entry.Value.ControlType);

                }

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement windowElement = null;
                try
                {
                    windowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                    if (windowElement != null)
                    {
                        DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(" Window not visible");

                }

                List<IUIAutomationElement> elements = FindElementsByUniquePropertyType(
                     automation,
                     rootElement,
                     ApplicationArguments.mapdictionary["HeaderSelectAllcheckBox"].UniquePropertyType,
                     ApplicationArguments.mapdictionary["HeaderSelectAllcheckBox"].AutomationUniqueValue
                 );

                if (elements != null && elements.Count > 0)
                {
                    for (int i = elements.Count - 1; i >= 0; i--)
                    {
                        try
                        {
                            IUIAutomationElement HeaderSelectAllcheckBox = elements[i];

                            if (HeaderSelectAllcheckBox != null)
                            {
                                Thread.Sleep(3000);
                                MouseOperations.ClickElement(HeaderSelectAllcheckBox);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }


                if (!string.IsNullOrEmpty(dt.Rows[0]["Continue"].ToString()))
                {
                    IUIAutomationElement continueBtn = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["Continue"].UniquePropertyType, ApplicationArguments.mapdictionary["Continue"].AutomationUniqueValue);

                    if (continueBtn != null)
                    {
                        try
                        {
                            Thread.Sleep(3000);
                            ClickButton(continueBtn);

                            windowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ParentModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ParentModuleWindow"].AutomationUniqueValue);
                            if (windowElement != null)
                            {
                                DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ParentModuleWindow"].AutomationUniqueValue);
                            }

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(" Window not visible");

                        }

                    }
                }
                else if (!string.IsNullOrEmpty(dt.Rows[0]["Abort"].ToString()))
                {
                    IUIAutomationElement continueBtn = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["Abort"].UniquePropertyType, ApplicationArguments.mapdictionary["Abort"].AutomationUniqueValue);

                    if (continueBtn != null)
                    {
                        ClickButton(continueBtn);
                        try
                        {
                            windowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ParentModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ParentModuleWindow"].AutomationUniqueValue);
                            if (windowElement != null)
                            {
                                DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ParentModuleWindow"].AutomationUniqueValue);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Window not visible");

                        }

                    }
                }
                else if (!string.IsNullOrEmpty(dt.Rows[0]["Security Master"].ToString()))
                {
                    IUIAutomationElement continueBtn = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["Security Master"].UniquePropertyType, ApplicationArguments.mapdictionary["Security Master"].AutomationUniqueValue);

                    if (continueBtn != null)
                    {

                        ClickButton(continueBtn);

                        try
                        {
                            windowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ParentModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ParentModuleWindow"].AutomationUniqueValue);
                            if (windowElement != null)
                            {
                                DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ParentModuleWindow"].AutomationUniqueValue);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Window not visible");

                        }

                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        HandleDialogWithYesOrOk(windowElement, maxRetries: 1, maxTimeInSeconds: 10);
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static bool HandleDialogWithYesOrOk(IUIAutomationElement element, int maxRetries = 3, int maxTimeInSeconds = 30, int intervalInMilliseconds = 4000)
        {
            int retryCount = 0;
            DateTime startTime = DateTime.Now;

            while (retryCount < maxRetries && (DateTime.Now - startTime).TotalSeconds < maxTimeInSeconds)
            {
                try
                {
                    IUIAutomation automation = new CUIAutomation();
                    IUIAutomationElement rootElement = automation.GetRootElement();

                    IUIAutomationCondition dialogCondition = automation.CreateOrCondition(
                     automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "dialog"),
                     automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "dialogue")
                 );

                    IUIAutomationCondition condition = automation.CreateTrueCondition();

                    IUIAutomationElement dialogElement = element.FindFirst(TreeScope.TreeScope_Descendants, dialogCondition);

                    if (dialogElement != null)
                    {
                        IntPtr hWnd = (IntPtr)dialogElement.CurrentNativeWindowHandle;
                        SetForegroundWindow(hWnd);

                        IUIAutomationCondition buttonCondition = automation.CreateOrCondition(
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "OK"),
                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes")
                        );

                        IUIAutomationElement button = dialogElement.FindFirst(TreeScope.TreeScope_Descendants, buttonCondition);

                        if (button != null)
                        {
                            var invokePattern = button.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                            if (invokePattern != null)
                            {
                                invokePattern.Invoke();
                            }

                            return true;
                        }
                    }

                    retryCount++;
                    System.Threading.Thread.Sleep(intervalInMilliseconds);
                }
                catch (Exception ex)
                {
                    retryCount++;
                    System.Threading.Thread.Sleep(intervalInMilliseconds);
                }
            }
            return false;
        }

        public void Maximize(string windowName)
        {
            IUIAutomation automation = new CUIAutomation();
            IUIAutomationElement rootElement = automation.GetRootElement();
            IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, windowName);
            IUIAutomationElement targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, condition);
            if (targetElement != null)
            {
                IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                UIAutomationHelper.SetForegroundWindow(hWnd);
                MaximizeWindowAction(targetElement);
            }
        }

        public bool WindowAction(DataTable dt)
        {
            try
            {

                if (!dt.Columns.Contains("PreviousWindow") && !dt.Columns.Contains("CloseWindow") && !dt.Columns.Contains("MaximizeWindow"))
                    return false;

                foreach (DataRow row in dt.Rows)
                {
                    string previousWindow = row["PreviousWindow"] != null ? row["PreviousWindow"].ToString().Trim() : "";

                    if (!string.IsNullOrEmpty(previousWindow))
                    {
                        IUIAutomation automation = new CUIAutomation();
                        IUIAutomationElement rootElement = automation.GetRootElement();
                        IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, previousWindow);
                        IUIAutomationElement targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, condition);

                        if (targetElement != null)
                        {
                            IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                            SetForegroundWindow(hWnd);

                            string closeWindow = row["CloseWindow"] != null ? row["CloseWindow"].ToString().Trim() : "";

                            if (!string.IsNullOrEmpty(closeWindow))
                                CloseWindowAction(targetElement);


                            string maximizeWindow = row["MaximizeWindow"] != null ? row["MaximizeWindow"].ToString().Trim() : "";
                            if (!string.IsNullOrEmpty(maximizeWindow))
                                MaximizeWindowAction(targetElement);
                        }


                    }
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        public static bool DetectAndCloseWindow(string automationId)
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, automationId);
                IUIAutomationElement targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, condition);

                if (targetElement != null)
                {
                    IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                    SetForegroundWindow(hWnd);
                    UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                    uiAutomationHelper.CloseWindowAction(targetElement);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                return false;
            }
        }

        private void CloseWindowAction(IUIAutomationElement targetElement)
        {
            try
            {
                if (targetElement != null)
                {
                    IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                    PostMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error closing window: " + ex.Message);
            }
        }
        private void MinimizeWindowAction(IUIAutomationElement targetElement)
        {
            try
            {
                if (targetElement != null)
                {
                    IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                    ShowWindow(hWnd, SW_MINIMIZE);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error minimizing window: " + ex.Message);
            }
        }

        public void MaximizeWindowAction(IUIAutomationElement targetElement)
        {
            try
            {
                if (targetElement != null)
                {
                    IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                    ShowWindow(hWnd, SW_MAXIMIZE);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error maximizing window: " + ex.Message);
            }
        }


        public bool VerifyMessageBoxofDialogueBox(IUIAutomationElement WindowElement, string columnName, string stringToVerify)
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();


                IUIAutomationCondition xCondition = automation.CreatePropertyCondition(
                       UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary[columnName].AutomationUniqueValue);


                IUIAutomationElement targetElement = WindowElement.FindFirst(TreeScope.TreeScope_Descendants, xCondition);

                if (targetElement != null)
                {
                    string targetElementName = targetElement.CurrentName;
                    return string.Equals(targetElementName, stringToVerify, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    Console.WriteLine("Target element not found.");
                    return false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error VerifyMessageBoxofDialogueBox UIAutomation: " + ex.Message);
            }
            return false;
        }

        public string NirvanaDialogueBoxHandler(DataTable dt)
        {
            string detailedInfo2 = "";
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                if (rootElement == null)
                    return detailedInfo2;

                IUIAutomationCondition controlTypeCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_LocalizedControlTypePropertyId,
                    ApplicationArguments.mapdictionary["dialogue"].AutomationUniqueValue);

                IUIAutomationCondition classNameCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_ClassNamePropertyId,
                    "#" + ApplicationArguments.mapdictionary["ClassNameDialogueBox"].AutomationUniqueValue);

                IUIAutomationCondition combinedCondition = automation.CreateAndCondition(controlTypeCondition, classNameCondition);

                int waitTime = 3000;
                if (ApplicationArguments.mapdictionary.ContainsKey("Wait"))
                {
                    int configuredWaitTime;
                    if (int.TryParse(ApplicationArguments.mapdictionary["Wait"].AutomationUniqueValue, out configuredWaitTime))
                    {
                        Console.WriteLine("Found Wait " + ApplicationArguments.mapdictionary["Wait"].AutomationUniqueValue);
                        waitTime = configuredWaitTime * 1000;
                    }
                }

                IUIAutomationElement targetElement = null;
                int retryCount = 3;

                for (int i = 0; i < retryCount; i++)
                {
                    Console.WriteLine("Finding NirvanaDialogue Box Try " + i);
                    targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);
                    if (targetElement != null)
                        break;

                    Thread.Sleep(waitTime);
                }

                if (targetElement == null)
                {
                    Console.WriteLine("NirvanaDialogue Box Not Found, Retrying with individual conditions");

                    targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, controlTypeCondition);
                    if (targetElement == null)
                        targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, classNameCondition);

                    if (targetElement == null)
                    {
                        Console.WriteLine("NirvanaDialogue Box still not found, Verification Failed");
                        return detailedInfo2;
                    }
                }

                Console.WriteLine("NirvanaDialogue Box Found");

                try
                {
                    if (!dt.Columns.Contains("Message") && !dt.Columns.Contains("ButtonOk"))
                        return detailedInfo2;

                    foreach (DataRow row in dt.Rows)
                    {
                        IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                        SetForegroundWindow(hWnd);
                        Console.WriteLine("Switched to dialogue box window");

                        foreach (DataColumn column in dt.Columns)
                        {
                            string columnName = column.ColumnName.Trim();
                            if (columnName.Equals("TitleBar", StringComparison.OrdinalIgnoreCase))
                                continue;

                            string columnValue = row[columnName] != DBNull.Value && row[columnName] != null ? row[columnName].ToString().Trim() : "";

                            if (string.IsNullOrEmpty(columnValue))
                                continue;

                            if (columnName.Equals("Message", StringComparison.OrdinalIgnoreCase))
                            {
                                bool isVerificationSuccessful = VerifyMessageBoxofDialogueBox(targetElement, columnName, columnValue);
                                Console.WriteLine("Message verification: " + isVerificationSuccessful);
                            }
                            else
                            {
                                IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_NamePropertyId,
                                    ApplicationArguments.mapdictionary[columnName].AutomationUniqueValue);

                                IUIAutomationElement targetElement2 = targetElement.FindFirst(TreeScope.TreeScope_Descendants, nameCondition);

                                if (targetElement2 != null)
                                {
                                    try
                                    {
                                        MouseOperations.ClickElement(targetElement2);
                                        Console.WriteLine("Clicked on element: " + columnName);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error interacting with element " + columnName + ": " + ex.Message);
                                    }
                                }
                            }
                        }
                    }

                    Thread.Sleep(2000);
                    targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);
                    if (targetElement != null)
                    {
                        IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                        SetForegroundWindow(hWnd);
                        Console.WriteLine("Switched to dialogue box window");
                        Console.WriteLine("Popup still present, pressing Enter key");
                        SendKeys.SendWait("{ENTER}");
                    }

                    return "";
                }
                catch
                {
                    return detailedInfo2;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in NirvanaDialogueBoxHandler " + ex.Message);
                return detailedInfo2;
            }
        }
        public static IUIAutomationElement GetMainWindowElement(string key)
        {
            IUIAutomation automation = new CUIAutomation();
            IUIAutomationElement rootElement = automation.GetRootElement();

            if (!ApplicationArguments.mapdictionary.ContainsKey(key))
            {
                Console.WriteLine(string.Format("Key '{0}' not found in dictionary.", key));
                return null;
            }

            var automationData = ApplicationArguments.mapdictionary[key];

            try
            {
                return FindElementByUniquePropertyType(
                    automation,
                    rootElement,
                    automationData.UniquePropertyType,
                    automationData.AutomationUniqueValue
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error finding window element for '{0}': {1}", key, ex.Message));
                return null;
            }
        }

        public static DataSet DetectBlotterGridAndExtractData()
        {
            DataSet ds = new DataSet();
            try
            {
                ApplicationArguments.mapdictionary = CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["CommonMappings"]);

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement mainWindowElement = null;
                try
                {
                    mainWindowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["BlotterMainWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["BlotterMainWindow"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error finding Widdow element: {0}", ex.Message));

                    return null;
                }

                if (mainWindowElement == null)
                {
                    Console.WriteLine("Main window element not found.");
                    return null;
                }
                else
                {
                    UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                    uiAutomationHelper.MaximizeWindowAction(mainWindowElement);

                    IUIAutomationCondition BTTNCondition = automation.CreatePropertyCondition(
                                   UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["btnExpansion"].AutomationUniqueValue);

                    IUIAutomationElement isSummaryExpandButtonAvailable = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, BTTNCondition);

                    if (isSummaryExpandButtonAvailable != null)
                    {
                        string buttonName = isSummaryExpandButtonAvailable.CurrentName;

                        if (buttonName.Contains("+"))
                        {
                            MouseOperations.ClickElement(isSummaryExpandButtonAvailable);
                            Console.WriteLine("Button clicked to expand.");
                        }
                        else
                        {
                            Console.WriteLine("Button is already expanded. No action needed.");
                        }

                    }

                    ds = BlotterData(mainWindowElement, mainWindowElement.CurrentAutomationId, ApplicationArguments.mapdictionary["Data"].AutomationUniqueValue);
                }

                return ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DetectBlotterGridAndExtractData " + ex.Message);
                return ds; ;
            }
            return ds;
        }
        public static DataSet ExpandGridElements()
        {
            DataSet ds = new DataSet();

            IUIAutomationElementArray gridElements = null;
            try
            {
                ApplicationArguments.mapdictionary = CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["CommonMappings"]);

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement mainWindowElement = null;
                try
                {
                    mainWindowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["BlotterReports"].UniquePropertyType, ApplicationArguments.mapdictionary["BlotterReports"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error finding Widdow element: {0}", ex.Message));

                    return null;
                }

                if (mainWindowElement == null)
                {
                    Console.WriteLine("Main window element not found.");
                    return null;
                }
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                uiAutomationHelper.MaximizeWindowAction(mainWindowElement);
                IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["executionReportGrid"].AutomationUniqueValue);
                gridElements = mainWindowElement.FindAll(TreeScope.TreeScope_Descendants, grdCondition);

                if (gridElements == null)
                {
                    return null;
                }
                else
                {
                    string automationValue = ApplicationArguments.mapdictionary["VerifyExpandedBR"].AutomationUniqueValue.ToString();
                    string[] dataSetArray = automationValue.Split(',');
                    List<string> dataSetTableName = new List<string>(dataSetArray);



                    for (int iNDEX = 0; iNDEX < gridElements.Length; iNDEX++)
                    {
                        DataTable dt = new DataTable();
                        IUIAutomationElement gridElement = gridElements.GetElement(iNDEX);
                        try
                        {

                            IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                            IUIAutomationElement gridControlTypeConditionElement = gridElement.FindFirst(TreeScope.TreeScope_Descendants, gridControlTypeCondition);

                            if (gridControlTypeConditionElement == null)
                            {
                                Console.WriteLine("TreeView element not found.");
                                return null;
                            }
                            else
                            {
                                gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);

                                IUIAutomationElementArray gridControlTypeConditionElement2 = gridControlTypeConditionElement.FindAll(TreeScope.TreeScope_Children, gridControlTypeCondition);
                                int size = gridControlTypeConditionElement2.Length;
                                for (int i = size - 1; i >= 0; i--)
                                {
                                    try
                                    {
                                        gridElements = mainWindowElement.FindAll(TreeScope.TreeScope_Descendants, grdCondition);
                                        gridElement = gridElements.GetElement(iNDEX);

                                        gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                                        gridControlTypeConditionElement = gridElement.FindFirst(TreeScope.TreeScope_Descendants, gridControlTypeCondition);

                                        if (gridControlTypeConditionElement == null)
                                        {
                                            Console.WriteLine("TreeView element not found.");
                                            return null;
                                        }
                                        else
                                        {
                                            gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
                                            gridControlTypeConditionElement2 = gridControlTypeConditionElement.FindAll(TreeScope.TreeScope_Children, gridControlTypeCondition);
                                            IUIAutomationElement ChildElement = gridControlTypeConditionElement2.GetElement(i);
                                            IUIAutomationElement thirdChildElement = ChildElement.FindFirst(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                                            Console.WriteLine(thirdChildElement.CurrentName);
                                            MouseOperations.ClickElement(thirdChildElement);
                                            var inputSimulator = new InputSimulator();
                                            inputSimulator.Mouse.LeftButtonClick();
                                            Thread.Sleep(500);
                                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RIGHT);
                                            Thread.Sleep(500);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error at index " + i + ": " + ex.Message);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                }


                ds = VerifyExpandedBR(mainWindowElement, mainWindowElement.CurrentAutomationId, ApplicationArguments.mapdictionary["BlotterReportsTrigger"].AutomationUniqueValue);

                uiAutomationHelper.MinimizeWindowAction(mainWindowElement);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error processing row with DataName: {0}. Exception: {1}", ex.Message));
                return ds;
            }


            return ds;
        }

        private static DataSet VerifyExpandedBR(IUIAutomationElement mainWindowElement, string parentAutomationID, string firstParentofElement)
        {
            DataSet ds = new DataSet();

            IUIAutomationElementArray gridElements = null;
            try
            {
                ApplicationArguments.mapdictionary = CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["CommonMappings"]);



                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                if (mainWindowElement == null)
                {
                    Console.WriteLine("Main window element not found.");
                    return null;
                }
                IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["executionReportGrid"].AutomationUniqueValue);
                gridElements = mainWindowElement.FindAll(TreeScope.TreeScope_Descendants, grdCondition);

                if (gridElements == null)
                {
                    return null;
                }
                else
                {
                    // List<string> dataSetTableName = ApplicationArguments.mapdictionary["VerifyExpandedBR"].AutomationUniqueValue.ToString().Split(',').ToList();
                    string automationValue = ApplicationArguments.mapdictionary["VerifyExpandedBR"].AutomationUniqueValue.ToString();
                    string[] dataSetArray = automationValue.Split(',');
                    List<string> dataSetTableName = new List<string>(dataSetArray);

                    for (int iNDEX = 0; iNDEX < gridElements.Length; iNDEX++)
                    {
                        DataTable dt = new DataTable();
                        IUIAutomationElement gridElement = gridElements.GetElement(iNDEX);
                        try
                        {

                            IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                            IUIAutomationElementArray gridControlTypeConditionElement = gridElement.FindAll(TreeScope.TreeScope_Descendants, gridControlTypeCondition);
                            if (gridControlTypeConditionElement == null)
                            {
                                Console.WriteLine("TreeView element not found.");
                                return null;
                            }
                            else
                            {


                                for (int i = 0; i < gridControlTypeConditionElement.Length; i++)
                                {
                                    try
                                    {
                                        IUIAutomationElement currentElement = gridControlTypeConditionElement.GetElement(i);
                                        if (dt.Columns.Count <= 0)
                                        {
                                            dt = CreateDataTable(currentElement, automation);
                                        }
                                        PopulateDataRowsOptimized(dt, currentElement, automation, parentAutomationID);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }


                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        if (dt != null)
                        {
                            dt.TableName = dataSetTableName[iNDEX];

                            ds.Tables.Add(dt);
                        }
                    }

                }



            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in BlotterData: " + ex.Message);
                return null;
            }
            return ds;
        }
        private static void PopulateDataRowsOptimized(DataTable dataTable, IUIAutomationElement gridElement, IUIAutomation automation, string parentAutomationID, bool allowParallel = true)
        {
            try
            {
                var dataItemCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
                IUIAutomationElementArray dataItems = gridElement.FindAll(TreeScope.TreeScope_Descendants, dataItemCondition);

                if (allowParallel)
                {
                    Parallel.For(0, (int)dataItems.Length, i =>
                    {
                        try
                        {
                            IUIAutomationElement dataItem = dataItems.GetElement(i);
                            DataRow row = dataTable.NewRow();
                            IUIAutomationElementArray dataItemChildren = dataItem.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                            for (int j = 0; j < dataItemChildren.Length; j++)
                            {
                                IUIAutomationElement cellElement = dataItemChildren.GetElement(j);
                                string columnName = cellElement.CurrentName;
                                bool resulttemp = false;
                                string cellValue = ControlTypeHandler.getValueOfElement(cellElement, ref resulttemp);
                                if (dataTable.Columns.Contains(columnName))
                                {
                                    row[columnName] = cellValue;
                                }
                            }

                            lock (dataTable)
                            {
                                dataTable.Rows.Add(row);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing data item " + i + ": " + ex.Message);
                        }
                    });
                }
                else
                {
                    for (int i = 0; i < dataItems.Length; i++)
                    {
                        try
                        {
                            IUIAutomationElement dataItem = dataItems.GetElement(i);
                            DataRow row = dataTable.NewRow();

                            IUIAutomationElementArray dataItemChildren = dataItem.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                            for (int j = 0; j < dataItemChildren.Length; j++)
                            {
                                IUIAutomationElement cellElement = dataItemChildren.GetElement(j);
                                string columnName = cellElement.CurrentName;
                                bool resulttemp = false;
                                string cellValue = ControlTypeHandler.getValueOfElement(cellElement, ref resulttemp);
                                if (dataTable.Columns.Contains(columnName))
                                {
                                    row[columnName] = cellValue;
                                }
                            }

                            dataTable.Rows.Add(row);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing data item " + i + ": " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in PopulateDataRowsOptimized: " + ex.Message);
            }
        }

        private static List<IUIAutomationElement> GetHeaderElements(IUIAutomationElement gridElement, IUIAutomation automation)
        {
            try
            {
                List<IUIAutomationElement> headerElements = new List<IUIAutomationElement>();

                // Define condition to find header elements
                IUIAutomationCondition headerCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_ControlTypePropertyId,
                    UIA_ControlTypeIds.UIA_HeaderControlTypeId);

                IUIAutomationElementArray headerArray = gridElement.FindAll(TreeScope.TreeScope_Descendants, headerCondition);

                for (int i = 0; i < headerArray.Length; i++)
                {
                    headerElements.Add(headerArray.GetElement(i));
                }

                return headerElements;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<IUIAutomationElement>();
            }
        }
        private static DataTable CreateDataTable(IUIAutomationElement gridElement, IUIAutomation automation)
        {
            try
            {
                DataTable dataTable = new DataTable();

                // Get headers
                var headerElements = GetHeaderElements(gridElement, automation);
                foreach (var headerElement in headerElements)
                {
                    IUIAutomationCondition condition = automation.CreateTrueCondition(); // Match all sub-elements
                    IUIAutomationElementArray childArray = headerElement.FindAll(TreeScope.TreeScope_Children, condition);

                    for (int i = 0; i < childArray.Length; i++)
                    {
                        IUIAutomationElement childElement = childArray.GetElement(i);
                        string headerName = childElement.CurrentName ?? "Unnamed Header";
                        if (!dataTable.Columns.Contains(headerName))
                        {
                            dataTable.Columns.Add(headerName);
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataTable();
            }
        }





        private static DataSet BlotterData(IUIAutomationElement mainWindowElement, string parentAutomationID, string firstParentofElement)
        {
            DataSet ds = new DataSet();

            IUIAutomationElementArray gridElements = null;
            try
            {
                ApplicationArguments.mapdictionary = CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["CommonMappings"]);

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                if (mainWindowElement == null)
                {
                    Console.WriteLine("Main window element not found.");
                    return null;
                }
                IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["BlotterGrid"].AutomationUniqueValue);
                gridElements = mainWindowElement.FindAll(TreeScope.TreeScope_Descendants, grdCondition);

                if (gridElements == null)
                {
                    return null;
                }
                else
                {
                    string automationValue = ApplicationArguments.mapdictionary["VerifyBlotter,VerifySubOrder"].AutomationUniqueValue.ToString();
                    string[] dataSetArray = automationValue.Split(',');
                    List<string> dataSetTableName = new List<string>(dataSetArray);

                    for (int iNDEX = 0; iNDEX < gridElements.Length; iNDEX++)
                    {
                        DataTable dt = new DataTable();
                        IUIAutomationElement gridElement = gridElements.GetElement(iNDEX);
                        try
                        {
                            IUIAutomationCondition treeViewCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                            IUIAutomationElement treeViewElement = gridElement.FindFirst(TreeScope.TreeScope_Descendants, treeViewCondition);
                            if (treeViewElement == null)
                            {
                                Console.WriteLine("TreeView element not found.");
                                return null;
                            }


                            dt = CreateDataTable(gridElement, automation);

                            PopulateDataRowsOptimized(dt, gridElement, automation, parentAutomationID, false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        if (dt != null)
                        {
                            dt.TableName = dataSetTableName[iNDEX];

                            ds.Tables.Add(dt);
                        }
                    }

                }



            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in BlotterData: " + ex.Message);
                return null;
            }
            return ds;
        }
        public static void PerformActionWithRetry(string moduleWindow, string sendKeys)
        {
            int maxRetries = 3;
            int waitTimeMs = 3000;
            int attempts = 0;
            bool success = false;

            while (attempts < maxRetries && !success)
            {
                attempts++;
                Console.WriteLine("Attempt " + attempts + " to detect and switch window: " + moduleWindow);
                success = UIAutomationHelper.DetectAndSwitchWindow(moduleWindow);

                if (!success)
                {
                    Keyboard.SendKeys(sendKeys);
                    break;
                }
                else if (attempts < maxRetries)
                {
                    Console.WriteLine("Attempt " + attempts + " failed. Retrying in " + (waitTimeMs / 1000) + " seconds...");
                    Thread.Sleep(waitTimeMs);
                }
            }

            if (!success)
            {
                Console.WriteLine("Max retry limit reached. Unable to switch window.");
            }
        }
        public void HandleSolutionExplorerFilePath(string columnValue, string columnName)
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                IUIAutomationElement mainWindowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["PranaMain"].UniquePropertyType, ApplicationArguments.mapdictionary["PranaMain"].AutomationUniqueValue);

                if (rootElement != null && mainWindowElement != null)
                {

                    IUIAutomationCondition controlTypeCondition = automation.CreatePropertyCondition(
    UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "dialogue");

                    IUIAutomationCondition classNameCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_ClassNamePropertyId, "#32770");

                    IUIAutomationCondition combinedCondition = automation.CreateAndCondition(controlTypeCondition, classNameCondition);

                    IUIAutomationElement targetElement = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);
                    if (targetElement == null)
                    {
                        targetElement = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, controlTypeCondition);
                        if (targetElement == null)
                        {
                            targetElement = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, classNameCondition);
                        }
                    }
                    if (targetElement != null)
                    {
                        try
                        {
                            if (targetElement != null)
                            {
                                IntPtr hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                                SetForegroundWindow(hWnd);
                                Keyboard.SendKeys(columnValue);
                                IUIAutomationCondition buttonNameCondition = automation.CreatePropertyCondition(
                       UIA_PropertyIds.UIA_NamePropertyId, "Open");
                                IUIAutomationElement openButton = targetElement.FindFirst(TreeScope.TreeScope_Descendants, buttonNameCondition);
                                if (openButton != null)
                                {
                                    try
                                    {
                                        hWnd = (IntPtr)targetElement.CurrentNativeWindowHandle;
                                        SetForegroundWindow(hWnd);
                                        Thread.Sleep(3000);
                                        MouseOperations.ClickElement(openButton, "Left");
                                        Thread.Sleep(3000);
                                        targetElement = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);
                                        if (targetElement == null)
                                        {
                                            targetElement = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, controlTypeCondition);
                                            if (targetElement == null)
                                            {
                                                targetElement = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, classNameCondition);
                                            }
                                        }
                                        if (targetElement != null)
                                        {
                                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                            Thread.Sleep(3000);
                                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                            SendKeys.SendWait("{ESC}");
                                        }
                                    }
                                    catch { }
                                }
                                else
                                {
                                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                }
                            }

                        }
                        catch
                        {

                        }
                    }
                }

            }
            catch { }
        }

        public bool EditTradeSidePanelAction(DataTable dt, string key, string sheet)
        {
            bool success = true;
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[sheet]);

                PerformActionWithRetry(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue, key);

                IUIAutomationElement windowElement = null;
                try
                {
                    windowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error finding Widdow element: {0}", ex.Message));
                    return false;
                }

                if (windowElement != null)
                {

                    IUIAutomationCondition controlTypeCondition = automation.CreatePropertyCondition(
      UIA_PropertyIds.UIA_NamePropertyId, ApplicationArguments.mapdictionary["EditTradeTabItemName"].AutomationUniqueValue);

                    IUIAutomationCondition classNameCondition = automation.CreatePropertyCondition(
                        UIA_PropertyIds.UIA_ClassNamePropertyId, ApplicationArguments.mapdictionary["EditTradeTabItemClassName"].AutomationUniqueValue);

                    IUIAutomationCondition combinedCondition = automation.CreateAndCondition(controlTypeCondition, classNameCondition);

                    IUIAutomationElement targetElement = windowElement.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                    if (targetElement != null)
                    {
                        MouseOperations.ClickElement(targetElement);

                        targetElement = windowElement.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);
                        IUIAutomationCondition paneTypeCondition = automation.CreatePropertyCondition(
    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["EditTradePane"].AutomationUniqueValue);

                        IUIAutomationElement targetpaneElement = windowElement.FindFirst(TreeScope.TreeScope_Descendants, paneTypeCondition);

                        if (targetpaneElement != null)
                        {
                            MouseOperations.ClickElement(targetElement);

                            IUIAutomationCondition VerticalScrollBarIDCondition = automation.CreatePropertyCondition(
UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["VerticalScrollBar"].AutomationUniqueValue);

                            IUIAutomationCondition ScrollBarCondition = automation.CreatePropertyCondition(
                              UIA_PropertyIds.UIA_ClassNamePropertyId, ApplicationArguments.mapdictionary["ScrollBar"].AutomationUniqueValue);

                            IUIAutomationCondition ScrollBarCombinedCondition = automation.CreateAndCondition(VerticalScrollBarIDCondition, ScrollBarCondition);

                            IUIAutomationElement VerticalScrollBarElement = windowElement.FindFirst(TreeScope.TreeScope_Descendants, ScrollBarCombinedCondition);
                            IUIAutomationElement mainScrollerBar = null;
                            if (VerticalScrollBarElement != null)
                            {
                                IUIAutomationCondition mainScrollerBarCondition = automation.CreatePropertyCondition(
                              UIA_PropertyIds.UIA_ClassNamePropertyId, ApplicationArguments.mapdictionary["Thumb"].AutomationUniqueValue);

                                mainScrollerBar = VerticalScrollBarElement.FindFirst(TreeScope.TreeScope_Children, ScrollBarCondition);

                            }

                            if (mainScrollerBar != null)
                            {
                                int index = 15;
                                //reset statusbar
                                for (int i = 0; i < index; i++)
                                {
                                    MouseOperations.ClickElement(mainScrollerBar);
                                    Keyboard.SendKeys(KeyboardConstants.PAGEUP_KEY);
                                }
                                int maxtry = 9;
                                try
                                {
                                    IUIAutomationElement TradeAttribute6Element = null;
                                    while (TradeAttribute6Element != null)
                                    {
                                        MouseOperations.ClickElement(targetElement);
                                        CommonAction(dt, "OPEN_ALLOCATION", "AllocationClientWindow");
                                        Keyboard.SendKeys(KeyboardConstants.PAGEDOWN_KEY);
                                        TradeAttribute6Element = FindElementByUniquePropertyType(automation, targetpaneElement, ApplicationArguments.mapdictionary["TradeAttribute6"].UniquePropertyType, ApplicationArguments.mapdictionary["TradeAttribute6"].AutomationUniqueValue);
                                        maxtry--;
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
                CommonAction(dt, "OPEN_ALLOCATION", "AllocationClientWindow");

                return success;
            }
            catch { return false; }
        }

        public bool OperationsonMPRB(DataTable dt, string key, string sheet)
        {
            bool success = true;
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[sheet]);

                PerformActionWithRetry(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue, key);

                IUIAutomationElement windowElement = null;
                try
                {
                    windowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error finding Widdow element: {0}", ex.Message));

                    return false;
                }

                if (windowElement != null)
                {
                    DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                    IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["ModelPortfolioGridGridName"].AutomationUniqueValue);
                    IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition);


                    if (gridElementMain == null)
                    {
                        Console.WriteLine("Grid element with AutomationId " + ApplicationArguments.mapdictionary["ModelPortfolioGridGridName"].AutomationUniqueValue + "not found.");
                        throw new Exception("Grid element with AutomationId " + ApplicationArguments.mapdictionary["ModelPortfolioGridGridName"].AutomationUniqueValue + "not found.");
                    }
                    else
                    {
                        Console.WriteLine(gridElementMain.CurrentName);
                        Console.WriteLine(gridElementMain.CurrentAutomationId);

                        MouseOperations.ClickElement(gridElementMain, "Right");
                        IUIAutomationCondition ContextMenuScrollViewerCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["ContextMenuScrollViewer"].AutomationUniqueValue);

                        IUIAutomationElement ContextMenuScrollViewer = windowElement.FindFirst(TreeScope.TreeScope_Descendants, ContextMenuScrollViewerCondition);
                        IUIAutomationElementArray ContextMenuScrolgridChildren = null;
                        if (ContextMenuScrollViewer != null)
                        {
                            ContextMenuScrolgridChildren = ContextMenuScrollViewer.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                        }
                        if (ContextMenuScrolgridChildren != null)
                        {
                            for (int i = 0; i < ContextMenuScrolgridChildren.Length; i++)
                            {
                                IUIAutomationElement childElement = ContextMenuScrolgridChildren.GetElement(i);
                                if (!string.IsNullOrEmpty(childElement.CurrentName))
                                {
                                    Console.WriteLine(childElement.CurrentName);
                                    if (!string.IsNullOrEmpty(dt.Rows[0]["RightClickAdd"].ToString()) && string.Equals(childElement.CurrentName, "Add", StringComparison.OrdinalIgnoreCase))
                                    {
                                        MouseOperations.ClickElement(childElement);
                                    }
                                    else if (!string.IsNullOrEmpty(dt.Rows[0]["RightClickDelete"].ToString()) && string.Equals(childElement.CurrentName, "Delete", StringComparison.OrdinalIgnoreCase))
                                    {
                                        MouseOperations.ClickElement(childElement);
                                    }
                                }


                            }

                        }
                        if (ContextMenuScrolgridChildren == null)
                        {//need to handle it with hardcoded clicks as automationID missing currently
                            Thread.Sleep(3000);
                            if (!string.IsNullOrEmpty(dt.Rows[0]["RightClickAdd"].ToString()))
                            {
                                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                            }
                            else if (!string.IsNullOrEmpty(dt.Rows[0]["RightClickDelete"].ToString()))
                            {
                                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                            }
                        }


                    }

                }
                return false;
            }
            catch { return false; }
        }
        public bool SelectModelPortfolio(DataTable dt, string key, string sheet)
        {
            bool success = true;
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[sheet]);

                PerformActionWithRetry(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue, key);

                IUIAutomationElement windowElement = null;
                try
                {
                    windowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error finding Widdow element: {0}", ex.Message));

                    return false;
                }

                if (windowElement != null)
                {
                    try
                    {
                        GridDataProvider gridDataProvider = new GridDataProvider();
                        DataTable dtable = gridDataProvider.GetWPFGridData(sheet, "ModelPortfolioGrid");
                        IUIAutomationElement selectedRow = null;

                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                string action = dr["Action"].ToString();
                                string CheckBox = dr["CheckBox"].ToString();

                                DataRow matchingRow = null;
                                dr["Action"] = "";
                                dr["CheckBox"] = "";
                                if (string.IsNullOrEmpty(action) || string.Equals(action, "Select", StringComparison.OrdinalIgnoreCase))
                                {
                                    try
                                    {
                                        matchingRow = DataUtilities.GetMatchingDataRow(dtable, dr);
                                        int index = dtable.Rows.IndexOf(matchingRow);
                                        selectedRow = gridDataProvider.GetWPFGridRowElementByIndex("RebalancerWindow", dtable, index, "ModelPortfolioGrid");
                                        IUIAutomationElementArray childElements = selectedRow.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                                        if (childElements != null && childElements.Length > 0)
                                        {
                                            IUIAutomationElement lastChildElement = childElements.GetElement(childElements.Length - 1);
                                            Console.WriteLine(lastChildElement.CurrentName);
                                            MouseOperations.ClickElement(lastChildElement);
                                        }
                                    }
                                    catch (Exception ex)
                                    { Console.WriteLine(ex.Message); }
                                }
                                else if (string.Equals(action, "Edit", StringComparison.OrdinalIgnoreCase))
                                {
                                    try
                                    {
                                        if (selectedRow == null)
                                        {
                                            throw new Exception("Selected Row is null");
                                        }
                                        else
                                        {
                                            gridDataProvider.EditWPFGrid("RebalancerWindow", dr, selectedRow);
                                        }

                                    }
                                    catch (Exception ex)
                                    { Console.WriteLine(ex.Message); }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                    }

                    catch { return false; }
                }
                return success;
            }
            catch { return false; }
        }

        public DataSet GetEditCustomGroupData(string parentAutomationID, string firstParentofElement)
        {
            DataSet ds = new DataSet();

            IUIAutomationElement gridElements = null;
            try
            {

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationCondition mainCondition = automation.CreatePropertyCondition(
                                  UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary[parentAutomationID].AutomationUniqueValue);
                IUIAutomationElement mainWindowElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, mainCondition);

                if (mainWindowElement == null)
                {
                    Console.WriteLine("Main window element not found.");
                    return null;
                }
                IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["gbEditCustomGroups"].AutomationUniqueValue);
                IUIAutomationElement paneParentElement = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition);

                if (paneParentElement == null)
                {
                    return null;
                }
                else
                {
                    string automationValue = ApplicationArguments.mapdictionary["PaneListLogAutomationID"].AutomationUniqueValue.ToString();
                    string[] dataSetArray = automationValue.Split(',');
                    List<string> dataSetTableName = new List<string>(dataSetArray);
                    for (int iNDEX = 0; iNDEX < dataSetTableName.Count; iNDEX++)
                    {
                        string automationID = dataSetTableName[iNDEX];
                        DataTable dt = new DataTable();
                        try
                        {
                            IUIAutomationCondition listControlTypeCondition = automation.CreatePropertyCondition(
                           UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary[automationID].AutomationUniqueValue);

                            gridElements = paneParentElement.FindFirst(TreeScope.TreeScope_Descendants, listControlTypeCondition);


                            IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
                            IUIAutomationElementArray gridControlTypeConditionElement = gridElements.FindAll(TreeScope.TreeScope_Descendants, gridControlTypeCondition);

                            if (gridControlTypeConditionElement == null)
                            {
                                Console.WriteLine("TreeView element not found.");
                                return null;
                            }
                            else
                            {


                                for (int i = 0; i < gridControlTypeConditionElement.Length; i++)
                                {
                                    try
                                    {
                                        IUIAutomationElement currentElement = gridControlTypeConditionElement.GetElement(i);
                                        if (dt.Columns.Count <= 0)
                                        {
                                            dt = CreateDataTableForList(currentElement, automation);
                                        }
                                        if (currentElement.CurrentBoundingRectangle.left > 0 &&
               currentElement.CurrentBoundingRectangle.top > 0 && currentElement.CurrentBoundingRectangle.right > 0 &&
               currentElement.CurrentBoundingRectangle.bottom > 0)
                                        {
                                            PopulateDataFromList(dt, currentElement, automation, parentAutomationID);

                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }


                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        if (dt != null)
                        {
                            dt.TableName = dataSetTableName[iNDEX];

                            ds.Tables.Add(dt);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return null;
            }
            return ds;
        }

        public bool CustomGroupDataSelectItem(string parentAutomationID, string firstParentofElement, DataTable dt, string stepName, string columnToFocus)
        {
            try
            {
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                IUIAutomationElement gridElements = null;
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationCondition mainCondition = automation.CreatePropertyCondition(
                                  UIA_PropertyIds.UIA_AutomationIdPropertyId, parentAutomationID);
                IUIAutomationElement mainWindowElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, mainCondition);

                if (mainWindowElement == null)
                {
                    Console.WriteLine("Main window element not found.");
                    return false;
                }
                IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["gbEditCustomGroups"].AutomationUniqueValue);
                IUIAutomationElement paneParentElement = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition);

                if (paneParentElement == null)
                {
                    return false;
                }
                else
                {
                    string automationValue = ApplicationArguments.mapdictionary[stepName].AutomationUniqueValue.ToString();
                    string[] dataSetArray = automationValue.Split(',');
                    List<string> dataSetTableName = new List<string>(dataSetArray);
                    for (int iNDEX = 0; iNDEX < dataSetTableName.Count; iNDEX++)
                    {
                        string automationID = dataSetTableName[iNDEX];
                        try
                        {
                            IUIAutomationCondition listControlTypeCondition = automation.CreatePropertyCondition(
                           UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary[automationID].AutomationUniqueValue);

                            gridElements = paneParentElement.FindFirst(TreeScope.TreeScope_Descendants, listControlTypeCondition);


                            IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
                            IUIAutomationElementArray gridControlTypeConditionElement = gridElements.FindAll(TreeScope.TreeScope_Descendants, gridControlTypeCondition);

                            if (gridControlTypeConditionElement == null)
                            {
                                Console.WriteLine("TreeView element not found.");
                                return false;
                            }
                            else
                            {
                                foreach (DataRow drow in dt.Rows)
                                {

                                    for (int i = 0; i < gridControlTypeConditionElement.Length; i++)
                                    {
                                        try
                                        {
                                            IUIAutomationElement currentElement = gridControlTypeConditionElement.GetElement(i);

                                            if (currentElement.CurrentBoundingRectangle.left > 0 &&
                   currentElement.CurrentBoundingRectangle.top > 0 && currentElement.CurrentBoundingRectangle.right > 0 &&
                   currentElement.CurrentBoundingRectangle.bottom > 0)
                                            {
                                                var dataItemCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TextControlTypeId);
                                                IUIAutomationElement dataItem = currentElement.FindFirst(TreeScope.TreeScope_Descendants, dataItemCondition);
                                                string columnName = dataItem.CurrentName;
                                                if (string.Equals(columnName, drow[columnToFocus].ToString(), StringComparison.OrdinalIgnoreCase))
                                                {
                                                    MouseOperations.ClickElement(dataItem);
                                                }

                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }


                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return false;
            }
            return true;
        }
        private static void PopulateDataFromList(DataTable dataTable, IUIAutomationElement gridElement, IUIAutomation automation, string parentAutomationID)
        {
            try
            {
                var dataItemCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TextControlTypeId);
                IUIAutomationElement dataItem = gridElement.FindFirst(TreeScope.TreeScope_Descendants, dataItemCondition);

                try
                {
                    DataRow row = dataTable.NewRow();

                    string columnName = dataItem.CurrentName;
                    if (row.Table.Columns.Contains("ListItems"))
                    {
                        row["ListItems"] = columnName;
                    }
                    lock (dataTable)
                    {
                        dataTable.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in PopulateDataRowsOptimized: " + ex.Message);
            }
        }
        private DataTable CreateDataTableForList(IUIAutomationElement gridElement, IUIAutomation automation)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ListItems");
                return dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CreateDataTable: " + ex.Message);
                return new DataTable();
            }
        }
        public DataSet ExtractWinformGridData()
        {
            DataSet dataSet = new DataSet();
            try
            {
                dataSet = GridDataProvider.GetWinformGridData(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CreateDataTable: " + ex.Message);
                return new DataSet();
            }
            return dataSet;
        }

        public bool CommonAction(DataTable dt, string key, string sheet)
        {
            bool success = true;
            try
            {

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[sheet]);
                if (!string.IsNullOrEmpty(key))
                { PerformActionWithRetry(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue, key); }


                IUIAutomationElement windowElement = null;
                int retryCount = 3;
                int delayMilliseconds = 2000;

                for (int attempt = 1; attempt <= retryCount; attempt++)
                {
                    try
                    {
                        windowElement = FindElementByUniquePropertyType(
                            automation,
                            rootElement,
                            ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType,
                            ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue
                        );

                        if (windowElement != null)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        if (attempt == retryCount)
                        {
                            return false;
                        }
                        Thread.Sleep(delayMilliseconds);
                    }
                }

                if (windowElement != null)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn col in dt.Columns)
                        {
                            var value = dr[col];
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                Console.WriteLine(col.ColumnName + ": " + value);
                                if (ApplicationArguments.mapdictionary.ContainsKey(col.ColumnName))
                                {
                                    if (ApplicationArguments.mapdictionary[col.ColumnName].AutomationUniqueValue.Contains("FilePath"))
                                    {
                                        HandleSolutionExplorerFilePath(value.ToString(), col.ColumnName.ToString());
                                    }
                                    if (col.ColumnName.Contains("Verify"))
                                    {

                                        var uniqueValues = ApplicationArguments.mapdictionary[col.ColumnName].AutomationUniqueValue.Split(',');
                                        foreach (var uniqueVal in uniqueValues)
                                        {
                                            IUIAutomationElement targetElement = FindElementByUniquePropertyType(
                                               automation,
                                               windowElement,
                                               ApplicationArguments.mapdictionary[col.ColumnName].UniquePropertyType,
                                               uniqueVal);

                                            if (targetElement != null)
                                            {
                                                bool result = true;
                                                string foundElementValue = ControlTypeHandler.getValueOfElementAdvance(targetElement, ref result);
                                                if (string.Equals("VerifyContainslblMsg", col.ColumnName, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    if (string.IsNullOrEmpty(foundElementValue))
                                                    {
                                                        try
                                                        {
                                                            foundElementValue = targetElement.CurrentName;
                                                        }
                                                        catch { }
                                                    }
                                                    bool isverificationSucceeded = false;

                                                    if (string.Equals(foundElementValue, value.ToString(), StringComparison.OrdinalIgnoreCase) || foundElementValue.Contains(value.ToString()))
                                                    {
                                                        isverificationSucceeded = true;
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            isverificationSucceeded = CompareStringsSubstringWise(foundElementValue, value.ToString());
                                                            Console.WriteLine("Are the strings equal? " + isverificationSucceeded);
                                                            if (!isverificationSucceeded)
                                                            {
                                                                throw new Exception("strings equal? " + isverificationSucceeded);
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            ApplicationArguments.UIAutomationCommonActionResult = " Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString() + "Exception : " + ex.Message;
                                                            Console.WriteLine(" Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString() + "Exception : " + ex.Message);
                                                            throw new Exception(" Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString());
                                                        }
                                                    }

                                                    if (isverificationSucceeded)
                                                    {
                                                        Console.WriteLine(" Column" + col.ColumnName + " Verification Succeeded..");
                                                    }
                                                    else
                                                    {
                                                        ApplicationArguments.UIAutomationCommonActionResult = " Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString();
                                                        Console.WriteLine(" Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString());
                                                        throw new Exception(" Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString());
                                                    }
                                                }
                                                else if (col.ColumnName.Contains("VerifyToggleState"))
                                                {

                                                    try
                                                    {
                                                        object togglePatternObj = targetElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                                                        if (togglePatternObj != null)
                                                        {
                                                            IUIAutomationTogglePattern togglePattern = togglePatternObj as IUIAutomationTogglePattern;
                                                            if (togglePattern != null)
                                                            {
                                                                ToggleState toggleState = togglePattern.CurrentToggleState;
                                                                Console.WriteLine("Current toggle state: " + toggleState.ToString());

                                                                if (value.ToString().Contains(toggleState.ToString()))
                                                                {
                                                                    Console.WriteLine(" Column" + col.ColumnName + " Verification Succeeded..");
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine(" Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString());
                                                                    ApplicationArguments.UIAutomationCommonActionResult = " Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString();
                                                                    throw new Exception(" Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString());
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch { }



                                                }
                                                else if (col.ColumnName.Contains("VerifyContains"))
                                                {
                                                    if (string.IsNullOrEmpty(foundElementValue))
                                                    {
                                                        try
                                                        {
                                                            foundElementValue = targetElement.CurrentName;
                                                        }
                                                        catch { }
                                                    }
                                                    if (foundElementValue.Contains(value.ToString()))
                                                    {
                                                        Console.WriteLine(" Column" + col.ColumnName + " Verification Succeeded..");
                                                    }
                                                    else
                                                    {
                                                        ApplicationArguments.UIAutomationCommonActionResult = " Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString();
                                                        Console.WriteLine(" Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString());
                                                        throw new Exception(" Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString());
                                                    }
                                                }
                                                else if (string.Equals(foundElementValue, value.ToString(), StringComparison.OrdinalIgnoreCase))
                                                {
                                                    Console.WriteLine(" Column Specific Verification Succeeded..");
                                                }
                                                else
                                                {
                                                    ApplicationArguments.UIAutomationCommonActionResult = " Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString();
                                                    Console.WriteLine(" Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString());
                                                    throw new Exception(" Column Specific Verification failed..UI :" + foundElementValue + "Excel:" + value.ToString());
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        var uniqueValues = ApplicationArguments.mapdictionary[col.ColumnName].AutomationUniqueValue.Split(',');
                                        foreach (var uniqueVal in uniqueValues)
                                        {
                                            System.Threading.Thread.Sleep(4 * 1000);
                                            //IUIAutomationElement targetElement = FindElementByUniquePropertyType(
                                            //   automation,
                                            //   windowElement,
                                            //   ApplicationArguments.mapdictionary[col.ColumnName].UniquePropertyType,
                                            //   uniqueVal);

                                            IUIAutomationElement targetElement = null;
                                            int retryCount2 = 0;
                                            const int maxRetries = 3;
                                            const int retryDelayMilliseconds = 300;

                                            while (retryCount2 < maxRetries)
                                            {
                                                IntPtr hwnd = (IntPtr)windowElement.CurrentNativeWindowHandle;
                                                SetForegroundWindow(hwnd);
                                                targetElement = FindElementByUniquePropertyType(
                                                    automation,
                                                    windowElement,
                                                    ApplicationArguments.mapdictionary[col.ColumnName].UniquePropertyType,
                                                    uniqueVal
                                                );

                                                if (targetElement != null)
                                                {
                                                    break;
                                                }
                                                Console.WriteLine("Target element not found for value: " + col.ColumnName + "Retrying :" + retryCount2);
                                                retryCount2++;
                                                System.Threading.Thread.Sleep(retryDelayMilliseconds);
                                            }

                                            if (targetElement == null)
                                            {
                                                try
                                                {
                                                    string WindowAutomationID = ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue;
                                                    string ElementDetail = col.ColumnName.ToString();

                                                    string AutomationUniqueValue = ApplicationArguments.mapdictionary[col.ColumnName].AutomationUniqueValue;
                                                    string ControlTypeOfElementDetail = ApplicationArguments.mapdictionary[col.ColumnName].UniquePropertyType;
                                                    string ValueToPush = value.ToString();
                                                    string IUIAutomationMappingFile = ConfigurationManager.AppSettings["IUIAutomationMappingFile"].ToString();

                                                    string args = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"",
                                                        WindowAutomationID,
                                                        ElementDetail,
                                                        ControlTypeOfElementDetail,
                                                        ValueToPush,
                                                        IUIAutomationMappingFile,
                                                        AutomationUniqueValue);
                                                    
                                                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                                                    Console.WriteLine("Third-party execution start time: " + stopwatch.ElapsedMilliseconds + " ms");
                                                    int exitCode = ThirdPartyExecutionStatusReporter.RunHelperExe(args);
                                                    stopwatch.Stop();
                                                    Console.WriteLine("Third-party execution time: " + stopwatch.ElapsedMilliseconds + " ms");
                                                    if (exitCode != 0)
                                                    {
                                                        Console.WriteLine("Third-party execution failed. Exit Code: " + exitCode);
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Third-party execution succeeded.");
                                                        continue;
                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine("Error while dumping descendants: " + ex.Message);
                                                }
                                            }
                                            if (targetElement == null)
                                            {
                                                try
                                                {
                                                    IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                                                        TreeScope.TreeScope_Children,
                                                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                                                    IUIAutomationElement gridElement = appWindow.FindFirst(
                                                            TreeScope.TreeScope_Descendants,
                                                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "TradingTicket"));
                                                    targetElement = gridElement.FindFirst(
                                                    TreeScope.TreeScope_Descendants,
                                                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, uniqueVal));
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }

                                            }
                                            if (targetElement != null)
                                            {

                                                try
                                                {
                                                    int controlType = targetElement.CurrentControlType;
                                                    switch (controlType)
                                                    {
                                                        case UIA_ControlTypeIds.UIA_EditControlTypeId:
                                                        case UIA_ControlTypeIds.UIA_ComboBoxControlTypeId:
                                                            UIAutomationHelper.SetValue(targetElement, value.ToString());
                                                            break;

                                                        case UIA_ControlTypeIds.UIA_RadioButtonControlTypeId:
                                                            UIAutomationHelper.SetRadioButton(targetElement, value.ToString());
                                                            break;

                                                        case UIA_ControlTypeIds.UIA_CustomControlTypeId:
                                                            UIAutomationHelper.SetValueCustom(targetElement, value.ToString());
                                                            break;

                                                        case UIA_ControlTypeIds.UIA_TabItemControlTypeId:
                                                            UIAutomationHelper.ClickElement(targetElement);
                                                            break;

                                                        case UIA_ControlTypeIds.UIA_CheckBoxControlTypeId:
                                                            UIAutomationHelper.PerformCheckBoxActionsClickElement(targetElement, value.ToString(), "Left");
                                                            break;

                                                        case UIA_ControlTypeIds.UIA_SpinnerControlTypeId:
                                                            UIAutomationHelper.SetValueForSpinner(targetElement, value.ToString());
                                                            break;
                                                        case UIA_ControlTypeIds.UIA_PaneControlTypeId:
                                                            UIAutomationHelper.SetvalueForPane(targetElement, value.ToString());
                                                            break;
                                                        default:
                                                            UIAutomationHelper.ClickElement(targetElement);
                                                            break;
                                                    }


                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);

                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    IUIAutomationElement targetElement = FindElementByUniquePropertyType(
                                                   automation,
                                                   windowElement,
                                                   "AutomationId",
                                                   col.ColumnName.ToString());

                                    MouseOperations.ClickElement(targetElement);
                                }
                            }
                        }
                    }

                }
                else
                    return false;

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        private static StreamWriter _logWriter;
        public static void DumpAllElementsRecursiveToFile(IUIAutomationElement parent, IUIAutomation automation, int depth, string logFilePath, string prefix = "", bool isLast = true)
        {
            try
            {
                if (_logWriter == null)
                {
                    _logWriter = new StreamWriter(logFilePath, false); // Overwrite
                    _logWriter.AutoFlush = true;
                }

                IUIAutomationCondition trueCondition = automation.CreateTrueCondition();
                IUIAutomationElementArray children = parent.FindAll(TreeScope.TreeScope_Children, trueCondition);
                int count = children.Length;

                for (int i = 0; i < count; i++)
                {
                    IUIAutomationElement child = children.GetElement(i);

                    string childId = child.CurrentAutomationId;
                    string childName = child.CurrentName;
                    int childControlType = child.CurrentControlType;
                    string localizedControlType = child.CurrentLocalizedControlType;

                    // Extract value
                    bool getValueSucceeded = false;
                    string valueObj = ControlTypeHandler.getValueOfElement(child, ref getValueSucceeded);
                    string valueDisplay = getValueSucceeded ? valueObj : "N/A";

                    // Build tree-style prefix
                    bool isLastChild = (i == count - 1);
                    string branch = isLastChild ? "└── " : "├── ";
                    string newPrefix = prefix + (depth > 0 ? (isLast ? "    " : "│   ") : "");

                    string line = string.Format(
                        "{0}{1}Element - AutomationId: '{2}', Name: '{3}', ControlType: {4}, Value: '{5}', LocalizedControlType: '{6}'",
                        prefix, branch, childId, childName, childControlType, valueDisplay, localizedControlType);

                    Console.WriteLine(line);
                    _logWriter.WriteLine(line);

                    // Recurse into children
                    DumpAllElementsRecursiveToFile(child, automation, depth + 1, logFilePath, newPrefix, isLastChild);
                }

                if (depth == 0 && _logWriter != null)
                {
                    _logWriter.Close();
                    _logWriter = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during recursive traversal: " + ex.Message);
                if (_logWriter != null)
                {
                    _logWriter.WriteLine("Error: " + ex.Message);
                }
            }
        }

        private static void SetvalueForPane(IUIAutomationElement targetElement, string p)
        {
            IUIAutomationValuePattern valuePattern = (IUIAutomationValuePattern)targetElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
            if (valuePattern != null)
            {
                valuePattern.SetValue(p);
                return;
            }
            IUIAutomation automation = new CUIAutomation();
            IUIAutomationElement rootElement = automation.GetRootElement();
            IUIAutomationCondition editCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId);
            IUIAutomationElement editElement = targetElement.FindFirst(TreeScope.TreeScope_Children, editCondition);

            if (editElement != null)
            {
                valuePattern = (IUIAutomationValuePattern)editElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                if (valuePattern != null)
                {
                    valuePattern.SetValue(p);
                }
            }
            else
            {
                GridDataProvider.click(targetElement);
                if (!string.IsNullOrEmpty(p))
                {
                    DataUtilities.clearTextData();
                    Thread.Sleep(1000);
                    Keyboard.SendKeys(p);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }

            }
        }

        private static void SetValueForSpinner(IUIAutomationElement spinnerElement, string p)
        {
            IUIAutomation automation = new CUIAutomation();
            IUIAutomationElement rootElement = automation.GetRootElement();
            try
            {
                IUIAutomationValuePattern valuePattern = (IUIAutomationValuePattern)spinnerElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                if (valuePattern != null)
                {
                    valuePattern.SetValue(p);
                    return;
                }
               
                IUIAutomationCondition editCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId);
                IUIAutomationElement editElement = spinnerElement.FindFirst(TreeScope.TreeScope_Children, editCondition);

                if (editElement != null)
                {
                    valuePattern = (IUIAutomationValuePattern)editElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    if (valuePattern != null)
                    {
                        valuePattern.SetValue(p);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }
                }
                else
                {
                    GridDataProvider.click(spinnerElement);
                    Thread.Sleep(1000);
                    DataUtilities.clearTextData();
                    Keyboard.SendKeys(p);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);

                }
            }
            catch { }
            finally
            {
                 bool getValueSucceeded = false;
                    string valueObj = ControlTypeHandler.getValueOfElement(
                                          spinnerElement, ref getValueSucceeded);
                    string foundElementValue =
                        !string.IsNullOrEmpty(valueObj) ? valueObj : string.Empty;

                    if (string.IsNullOrEmpty(foundElementValue) || !foundElementValue.Equals(p))
                    {
                        for (int attempt = 1; attempt <= 2; attempt++)
                        {
                            Console.WriteLine(
                                string.Format("[INFO] Retrying input… Attempt {0}", attempt));

                            MouseOperations.ClickElement(spinnerElement);
                            ClearText(spinnerElement, automation,6);
                            Keyboard.SendKeys(p);
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);

                            Thread.Sleep(500);

                            getValueSucceeded = false;
                            valueObj = ControlTypeHandler.getValueOfElement(
                                           spinnerElement, ref getValueSucceeded);
                            foundElementValue =
                                !string.IsNullOrEmpty(valueObj) ? valueObj : string.Empty;

                            if (foundElementValue.Equals(p))
                            {
                                Console.WriteLine("[SUCCESS] Value matched after retry.");
                                break;
                            }

                            if (attempt == 2 && !foundElementValue.Equals(p))
                            {
                                Console.WriteLine(
                                    string.Format("[WARNING] Final attempt failed. Spinner value is '{0}' but expected '{1}'.",
                                                  foundElementValue, p));
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("[INFO] Spinner value successfully set in first attempt.");
                    }

                //bool result4 = false;
                //string valueObj = ControlTypeHandler.getValueOfElement(spinnerElement, ref result4);
                //if (string.IsNullOrEmpty(valueObj))
                //{
                //    ClearText(spinnerElement, automation);
                //}
                //string foundElementValue = !string.IsNullOrEmpty(valueObj) ? valueObj : "";
                ////if value not macthes or value appears empty or null
                //MouseOperations.ClickElement(spinnerElement);//do this operatiion twice
                ////
                //ClearText(spinnerElement, automation);
                ////
                //Keyboard.SendKeys(p);
                //PerformActionWithRetry this twice until it matches


            }

        }
        public bool WaitForElement(string automationId, int timeoutInSeconds)
        {
            int pollingIntervalMs = 500;
            int waitedTimeMs = 0;

            while (waitedTimeMs < timeoutInSeconds * 1000)
            {
                try
                {
                    IUIAutomation automation = new CUIAutomation();
                    IUIAutomationElement rootElement = automation.GetRootElement();
                    IUIAutomationElement Element = null;
                    try
                    {
                        Element = FindElementByUniquePropertyType(automation, rootElement, "", automationId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Error finding Widdow element: {0}", ex.Message));
                    }
                    if (Element != null)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception while finding element: " + ex.Message);
                }

                System.Threading.Thread.Sleep(pollingIntervalMs);
                waitedTimeMs += pollingIntervalMs;
            }

            return false;
        }

        public void FindAndDoubleClickElement(IUIAutomationElement gridElement, string automationId)
        {
            CUIAutomation automation = new CUIAutomation();
            IUIAutomationElement targetElement = null;
            if (automationId != "")
            {
                targetElement = gridElement.FindFirst(
                           TreeScope.TreeScope_Descendants,
                           automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, automationId));
            }
            else
            {
                targetElement = gridElement;
            }

            GridDataProvider.doubleClick(targetElement);
        }

        public void FindAndClickElement(string element, string button = "Left", bool usecursorclick =false)
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement().FindFirst(
                                            TreeScope.TreeScope_Children,
                                            automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));
                IUIAutomationElement windowElement = null;
                try
                {
                    windowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error finding Widdow element: {0}", ex.Message));
                }

                if (windowElement != null)
                {
                    IUIAutomationElement elementElement = FindElementByUniquePropertyType(automation, windowElement, ApplicationArguments.mapdictionary[element].UniquePropertyType, ApplicationArguments.mapdictionary[element].AutomationUniqueValue, ApplicationArguments.mapdictionary[element].ControlType);
                    MouseOperations.ClickElement(elementElement, button, usecursorclick);
                    Thread.Sleep(4000);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }
        public void FindAndClickElementPartiallyLeft(string element, string button = "Left")
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                IUIAutomationElement windowElement = null;
                try
                {
                    windowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error finding Widdow element: {0}", ex.Message));
                }

                if (windowElement != null)
                {
                    IUIAutomationElement elementElement = FindElementByUniquePropertyType(automation, windowElement, ApplicationArguments.mapdictionary[element].UniquePropertyType, ApplicationArguments.mapdictionary[element].AutomationUniqueValue);
                    MouseOperations.ClickElementLeftPartial(elementElement, button);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }
        
        public static void ClickElement(IUIAutomationElement element, string button = "Left")
        {
            MouseOperations.ClickElement(element, button);
        }
        public static void SetValueCustom(IUIAutomationElement element, string value)
        {
            try
            {
                string targetelementValue = value;
                try
                {
                    string tempDate = DataUtilities.DateHandler(value);
                    targetelementValue = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));
                }
                catch { targetelementValue = value; }
                try
                {
                    MouseOperations.ClickElement(element, "Left");
                }
                catch { }
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                uiAutomationHelper.SetTextBoxText(element, targetelementValue);
                Keyboard.SendKeys(KeyboardConstants.TABKEY);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public static void SetValue(IUIAutomationElement element, string value)
        {
            try
            {
                string targetelementValue = value;
                double dummy;
                const int UIA_ExpandCollapsePatternId = 10005;
                const int UIA_SelectionItemPatternId = 10010;
                const int UIA_SelectionPatternId = 10001;
                
                if (!double.TryParse(value, out dummy))
                {
                    object selectionPatternObj = null;
                    try
                    {
                        selectionPatternObj = element.GetCurrentPattern(UIA_SelectionPatternId);
                    }
                    catch { }

                    IUIAutomationSelectionPattern selectionPattern = selectionPatternObj as IUIAutomationSelectionPattern;

                    if (selectionPattern != null)
                    {
                        int maxRetries = 3;
                        
                        for (int attempt = 1; attempt <= maxRetries; attempt++)
                        {
                            try
                            {
                                List<IUIAutomationElement> containingItems = new List<IUIAutomationElement>();
                                object expandCollapseObj = null;
                                try
                                {
                                    expandCollapseObj = element.GetCurrentPattern(UIA_ExpandCollapsePatternId);
                                }
                                catch { }

                                IUIAutomationExpandCollapsePattern expandCollapsePattern = expandCollapseObj as IUIAutomationExpandCollapsePattern;

                                if (expandCollapsePattern != null)
                                {
                                    expandCollapsePattern.Expand();
                                    System.Threading.Thread.Sleep(300);
                                }

                                CUIAutomation automation = new CUIAutomation();
                                IUIAutomationElementArray listItems = element.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                                bool exactmatchFound = false;

                                for (int i = 0; i < listItems.Length; i++)
                                {
                                    bool isSelected = false;
                                    IUIAutomationElement item = listItems.GetElement(i);
                                    string itemName = string.Empty;
                                    try
                                    {
                                        itemName = item.CurrentName != null ? item.CurrentName.Trim() : string.Empty;
                                    }
                                    catch { }
                                  

                                    if (string.Equals(itemName, value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        exactmatchFound = true;

                                        object selectionItemObj = null;
                                        try
                                        {
                                            selectionItemObj = item.GetCurrentPattern(UIA_SelectionItemPatternId);
                                        }
                                        catch { }

                                        IUIAutomationSelectionItemPattern selectionItemPattern = selectionItemObj as IUIAutomationSelectionItemPattern;

                                        if (selectionItemPattern != null)
                                        {
                                            try
                                            {
                                                isSelected = selectionItemPattern.CurrentIsSelected == 1;
                                            }
                                            catch { }

                                            if (!isSelected)
                                            {
                                                try
                                                {
                                                    selectionItemPattern.Select();
                                                    System.Threading.Thread.Sleep(200);

                                                    try
                                                    {
                                                        isSelected = selectionItemPattern.CurrentIsSelected == 1;
                                                    }
                                                    catch { }

                                                    if (isSelected)
                                                    {
                                                        return;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine("Attempt " + attempt + " - Select() failed: " + ex.Message);
                                                }
                                            }
                                            else
                                            {
                                                return; 
                                            }
                                        }

                                        try
                                        {
                                            item.SetFocus();
                                            System.Threading.Thread.Sleep(200);

                                            IUIAutomationCondition condition = automation.CreatePropertyCondition(
     UIA_PropertyIds.UIA_NamePropertyId, itemName);
                                            IUIAutomationElement rootElement = automation.GetRootElement();
                                            IUIAutomationElement tempelement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, condition);
                                            if (tempelement != null &&
     tempelement.CurrentBoundingRectangle.left > 0 &&
     tempelement.CurrentBoundingRectangle.top > 0 &&
     tempelement.CurrentBoundingRectangle.right > 0 &&
     tempelement.CurrentBoundingRectangle.bottom > 0)
                                            {
                                                MouseOperations.ClickElement(tempelement, "Left", true);
                                            }
                                            else
                                            {
                                                expandCollapsePattern = expandCollapseObj as IUIAutomationExpandCollapsePattern;
                                                if (expandCollapsePattern != null)
                                                {
                                                    ExpandCollapseState state = expandCollapsePattern.CurrentExpandCollapseState;

                                                    if (state != ExpandCollapseState.ExpandCollapseState_Expanded)
                                                    {
                                                        expandCollapsePattern.Expand();
                                                       
                                                        System.Threading.Thread.Sleep(300);
                                                    }
                                                }
                                                Keyboard.SendKeys(targetelementValue);
                                                
                                            }
                                            Thread.Sleep(3000);
                                            try
                                            {
                                                selectionItemObj = item.GetCurrentPattern(UIA_SelectionItemPatternId);
                                                selectionItemPattern = selectionItemObj as IUIAutomationSelectionItemPattern;

                                                if (selectionItemPattern != null)
                                                {
                                                    isSelected = selectionItemPattern.CurrentIsSelected == 1;
                                                    if (isSelected)
                                                    {
                                                        return; 
                                                    }
                                                }
                                            }
                                            catch { }
                                        }
                                        catch { }
                                    }

                                    if (itemName.Contains(value))
                                    {
                                        containingItems.Add(item);
                                    }

                                }
                                //end of forloop
                                if (!exactmatchFound && containingItems.Count>0)
                                {
                                    IUIAutomationElement item = containingItems[0];

                                    object selectionItemObj = null;
                                    try
                                    {
                                        selectionItemObj = item.GetCurrentPattern(UIA_SelectionItemPatternId);
                                    }
                                    catch { }

                                    IUIAutomationSelectionItemPattern selectionItemPattern = selectionItemObj as IUIAutomationSelectionItemPattern;
                                    bool isSelected = false;
                                    if (selectionItemPattern != null)
                                    {
                                        try
                                        {
                                            isSelected = selectionItemPattern.CurrentIsSelected == 1;
                                        }
                                        catch { }

                                        if (!isSelected)
                                        {
                                            try
                                            {
                                                selectionItemPattern.Select();
                                                System.Threading.Thread.Sleep(200);

                                                try
                                                {
                                                    isSelected = selectionItemPattern.CurrentIsSelected == 1;
                                                }
                                                catch { }

                                                if (isSelected)
                                                {
                                                    return;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("Attempt " + attempt + " - Select() failed: " + ex.Message);
                                            }
                                        }
                                        else
                                        {
                                            return;
                                        }
                                    }

                                    try
                                    {
                                        string itemName = string.Empty;
                                        try
                                        {
                                            itemName = item.CurrentName != null ? item.CurrentName.Trim() : string.Empty;
                                        }
                                        catch { }
                                  
                                        item.SetFocus();
                                        System.Threading.Thread.Sleep(200);

                                        IUIAutomationCondition condition = automation.CreatePropertyCondition(
 UIA_PropertyIds.UIA_NamePropertyId, itemName);
                                        IUIAutomationElement rootElement = automation.GetRootElement();
                                        IUIAutomationElement tempelement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, condition);
                                        if (tempelement != null &&
 tempelement.CurrentBoundingRectangle.left > 0 &&
 tempelement.CurrentBoundingRectangle.top > 0 &&
 tempelement.CurrentBoundingRectangle.right > 0 &&
 tempelement.CurrentBoundingRectangle.bottom > 0)
                                        {
                                            MouseOperations.ClickElement(tempelement, "Left", true);
                                        }
                                        else
                                        {
                                            expandCollapsePattern = expandCollapseObj as IUIAutomationExpandCollapsePattern;
                                            if (expandCollapsePattern != null)
                                            {
                                                ExpandCollapseState state = expandCollapsePattern.CurrentExpandCollapseState;

                                                if (state != ExpandCollapseState.ExpandCollapseState_Expanded)
                                                {
                                                    expandCollapsePattern.Expand();

                                                    System.Threading.Thread.Sleep(300);
                                                }
                                            }
                                            Keyboard.SendKeys(targetelementValue);

                                        }
                                        Thread.Sleep(3000);
                                        try
                                        {
                                            selectionItemObj = item.GetCurrentPattern(UIA_SelectionItemPatternId);
                                            selectionItemPattern = selectionItemObj as IUIAutomationSelectionItemPattern;

                                            if (selectionItemPattern != null)
                                            {
                                                isSelected = selectionItemPattern.CurrentIsSelected == 1;
                                                if (isSelected)
                                                {
                                                    return;
                                                }
                                            }
                                        }
                                        catch { }
                                    }
                                    catch { }
                                }

                                System.Threading.Thread.Sleep(200); 
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Attempt " + attempt + " - ComboBox Selection Exception: " + ex.Message);
                                Keyboard.SendKeys(targetelementValue);
                            }
                        }

                       
                    }

                   
                }

                try
                {
                    string tempDate = DataUtilities.DateHandler(value);
                    targetelementValue = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));
                }
                catch { targetelementValue = value; }
                try
                {
                    MouseOperations.ClickElement(element, "Left");
                }
                catch { }
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                uiAutomationHelper.SetTextBoxText(element, targetelementValue);
                Keyboard.SendKeys(KeyboardConstants.TABKEY);
            
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public static void SetRadioButton(IUIAutomationElement element, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                    MouseOperations.ClickElement(element, "Left");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public Boolean RightClickOnGridAndSelectItem(DataTable dt, string sheet)
        {
            bool success = true;
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[sheet]);

                DetectAndSwitchWindow(sheet);

                IUIAutomationElement windowElement = null;
                int maxRetries = 3;
                int attempt = 0;

                while (attempt < maxRetries)
                {
                    try
                    {
                        windowElement = FindElementByUniquePropertyType(
                            automation,
                            rootElement,
                            ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType,
                            ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue
                        );

                        if (windowElement != null)
                        {
                            Console.WriteLine("Window element found.");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Error finding Window element (Attempt {0}): {1}", attempt + 1, ex.Message));
                    }

                    attempt++;
                }
                if (windowElement != null)
                {
                    DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                    IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["GridName"].AutomationUniqueValue);
                    IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition);


                    if (gridElementMain == null)
                    {
                        Console.WriteLine("Grid element with AutomationId " + ApplicationArguments.mapdictionary["GridName"].AutomationUniqueValue + "not found.");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine(gridElementMain.CurrentName);
                        Console.WriteLine(gridElementMain.CurrentAutomationId);

                        MouseOperations.ClickElement(gridElementMain, "Right");
                        windowElement = FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                        IUIAutomationCondition ContextMenuScrollViewerCondition = automation.CreatePropertyCondition(
                                UIA_PropertyIds.UIA_ControlTypePropertyId,
                                UIA_ControlTypeIds.UIA_MenuControlTypeId);

                        IUIAutomationElement ContextMenuScrollViewer = windowElement.FindFirst(TreeScope.TreeScope_Descendants, ContextMenuScrollViewerCondition);

                        IUIAutomationElementArray ContextMenuScrolgridChildren = null;

                        if (ContextMenuScrollViewer != null)
                        {
                            ContextMenuScrolgridChildren = ContextMenuScrollViewer.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                        }
                        if (ContextMenuScrolgridChildren != null)
                        {
                            bool foundone = false;
                            for (int i = 0; i < ContextMenuScrolgridChildren.Length; i++)
                            {
                                IUIAutomationElement childElement = ContextMenuScrolgridChildren.GetElement(i);
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (!string.IsNullOrEmpty(childElement.CurrentName))
                                    {
                                        if (dt.Columns.Contains(childElement.CurrentName) && !string.IsNullOrEmpty(row[childElement.CurrentName].ToString()))
                                        {
                                            MouseOperations.ClickElement(childElement);
                                            foundone = true;
                                            break;
                                        }
                                    }
                                }

                            }
                            return foundone;

                        }


                    }

                }
                return success;
            }
            catch { return false; }
        }

        static string ReplaceDateWithTodayNotation(string input)
        {
            string datePattern = @"\d{1,2}/\d{1,2}/\d{4} \d{1,2}:\d{2}:\d{2} (AM|PM|am|pm)";

            Match dateMatch = Regex.Match(input, datePattern);
            if (!dateMatch.Success) return input;

            string extractedDate = dateMatch.Value;

            string[] dateFormats = {
            "M/dd/yyyy h:mm:ss tt",
            "MM/dd/yyyy hh:mm:ss tt",
            "M/dd/yyyy hh:mm:ss tt",
            "MM/dd/yyyy h:mm:ss tt"
        };

            DateTime extractedDateTime;

            if (DateTime.TryParseExact(extractedDate, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out extractedDateTime))
            {
                DateTime today = DateTime.Today;
                int daysDifference = (extractedDateTime.Date - today).Days;

                string dayReplacement;
                if (daysDifference == 0)
                    dayReplacement = "TODAY()";
                else if (daysDifference > 0)
                    dayReplacement = "TODAY()+" + daysDifference;
                else
                    dayReplacement = "TODAY()" + daysDifference;

                string replacement = string.Format("{0} {1}", dayReplacement, extractedDateTime.ToString("h:mm:ss tt", CultureInfo.InvariantCulture));

                return input.Replace(extractedDate, replacement);
            }



            return input;
        }
        static string Normalize(string input)
        {
            return input.Replace(",", "").Trim();
        }
        private static string ReplaceTodayExpression(string input, string referenceText)
        {
            var pattern = @"TODAY\(\)([+-]\d+)?"; 
            var regex = new System.Text.RegularExpressions.Regex(pattern);

            return regex.Replace(input, match =>
            {
                string expr = match.Value; 
                int offset = 0;
                if (expr.Contains("+") || expr.Contains("-"))
                {
                    int.TryParse(expr.Substring(7), out offset);
                }
                DateTime referenceDate;
                if (TryExtractDate(referenceText, out referenceDate))
                {
                    return referenceDate.ToShortDateString(); 
                }
                DateTime targetDate = DateTime.Today.AddDays(offset);
                return targetDate.ToShortDateString(); 
            });
        }

        private static bool TryExtractDate(string input, out DateTime extractedDate)
        {
            foreach (var word in input.Split(' '))
            {
                DateTime temp;
                if (DateTime.TryParse(word, out temp))
                {
                    extractedDate = temp;
                    return true;
                }
            }

            extractedDate = DateTime.MinValue;
            return false;
        }
        static bool CompareStringsSubstringWise(string str1, string str2)
        {
            str1 = str1.Replace(",", "");
            string[] parts = str1.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            string mainText = parts[0].Trim();
            bool foundwithNormalize = false;
            
            if (parts.Length == 1 && str2.Contains("TODAY()"))
            {
                str2 = ReplaceTodayExpression(str2, mainText);
                if (str2.Contains(mainText))
                {
                    return true;
                }
            }
            if (!str2.Contains(mainText))
            {
                if (!Normalize(str2).Contains(Normalize(mainText)))
                {
                    return false;
                }
                else
                    foundwithNormalize = true;

            }
            int mainTextIndex = str2.IndexOf(mainText);
            try
            {
                if (mainTextIndex == -1)
                {
                    str2 = Normalize(str2);
                    mainTextIndex = str2.IndexOf(mainText);
                }
            }
            catch { }

            if (mainTextIndex == -1) return false;

            string remainingStr2 = str2.Substring(mainTextIndex + mainText.Length).Trim();

            if (parts.Length > 1 && remainingStr2.Length > 0)
            {
                string secondPart = parts[1].Trim();

                string normalizedStr1 = ReplaceDateWithTodayNotation(secondPart);
                string normalizedStr2 = AdjustTodayNotationForWeekend(remainingStr2);
                if (string.Equals(normalizedStr1, normalizedStr2, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }


            return false;
        }

        static string NormalizeSpaces(string input)
        {
            return Regex.Replace(input.Trim(), @"\s+", " ");
        }


        static string AdjustTodayNotationForWeekend(string input)
        {
            string todayPattern = @"TODAY\(\)\+(\d+)";
            Match match = Regex.Match(input, todayPattern);
            if (!match.Success) return input;

            int daysOffset;
            if (!int.TryParse(match.Groups[1].Value, out daysOffset)) return input;

            int adjustedOffset = AdjustForWeekend(daysOffset);
            string oldTodayFormat = "TODAY()+" + daysOffset;
            string newTodayFormat = "TODAY()+" + adjustedOffset;

            return input.Replace(oldTodayFormat, newTodayFormat);
        }

        static int AdjustForWeekend(int daysOffset)
        {
            DateTime targetDate = DateTime.Today.AddDays(daysOffset);

            if (targetDate.DayOfWeek == DayOfWeek.Saturday)
                return daysOffset + 2;
            else if (targetDate.DayOfWeek == DayOfWeek.Sunday)
                return daysOffset + 1;
            else
                return daysOffset;
        }

        public void HandleExpandedState(string parentgroupElementName, string MainButtonName, string state = "ToggleState_On")
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                IUIAutomationElement windowElement = null;
                try
                {
                    int retryCount = 3;
                    int delayBetweenRetriesMs = 1000;

                    for (int i = 0; i < retryCount; i++)
                    {
                        System.Threading.Thread.Sleep(3 * 1000);
                        try
                        {
                            windowElement = FindElementByUniquePropertyType(
                                automation,
                                rootElement,
                                ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType,
                                ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue
                            );

                            if (windowElement != null)
                                break;
                        }
                        catch
                        {
                            if (i == retryCount - 1)
                                throw;

                            System.Threading.Thread.Sleep(delayBetweenRetriesMs);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error finding Widdow element: {0}", ex.Message));
                }

                if (windowElement != null)
                {
                    if (windowElement != null)
                    {
                        IntPtr hWnd = (IntPtr)windowElement.CurrentNativeWindowHandle;
                        SetForegroundWindow(hWnd);
                        System.Threading.Thread.Sleep(3 * 1000);
                    }

                    IUIAutomationElement elementElement = FindElementByUniquePropertyType(automation, windowElement, ApplicationArguments.mapdictionary[parentgroupElementName].UniquePropertyType, ApplicationArguments.mapdictionary[parentgroupElementName].AutomationUniqueValue);
                    if (elementElement != null)
                    {
                        IUIAutomationElement checkboxSubchild = FindElementByUniquePropertyType(automation, elementElement, ApplicationArguments.mapdictionary[MainButtonName].UniquePropertyType, ApplicationArguments.mapdictionary[MainButtonName].AutomationUniqueValue);

                        if (checkboxSubchild != null)
                        {
                            object togglePatternObj = checkboxSubchild.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                            if (togglePatternObj != null)
                            {
                                IUIAutomationTogglePattern togglePattern = togglePatternObj as IUIAutomationTogglePattern;
                                if (togglePattern != null)
                                {
                                    ToggleState toggleState = togglePattern.CurrentToggleState;
                                    Console.WriteLine("Current toggle state: " + toggleState.ToString());
                                    string foundElementValue = !string.IsNullOrEmpty(toggleState.ToString()) ? toggleState.ToString() : "";

                                    if (!string.Equals(foundElementValue, state, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ClickButton(checkboxSubchild);
                                    }

                                }

                            }


                        }

                    }


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public static void HandleCheckBoxElement(IUIAutomationElement target, string value)
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                if (target != null)
                {
                    bool result4 = false;
                    string valueObj = ControlTypeHandler.getValueOfElement(target, ref result4);
                    if (string.IsNullOrEmpty(valueObj))
                    {
                        object propertyValue = target.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
                        valueObj = propertyValue != null ? propertyValue.ToString() : null;
                    }
                    string foundElementValue = !string.IsNullOrEmpty(valueObj) ? valueObj : "";
                    if (value.Contains("ToggleState_"))
                    {
                        if (!string.Equals(foundElementValue, value, StringComparison.OrdinalIgnoreCase))
                        {
                            UIAutomationHelper.ClickElement(target);
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            bool stateUpdated = VerifyElementStateAfterRetry(target, value);
                            if (stateUpdated)
                            {
                                Console.WriteLine("The state is now as expected.");
                            }
                            else
                            {
                                Console.WriteLine("The state did not update as expected after retries.");
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        private static bool VerifyElementStateAfterRetry(IUIAutomationElement target, string expectedValue, int maxRetries = 1, int delayBetweenRetriesMs = 1000)
        {
            bool result4 = false;
            string foundElementValue = string.Empty;
            for (int retry = 0; retry < maxRetries; retry++)
            {
                string valueObj = ControlTypeHandler.getValueOfElement(target, ref result4);

                if (string.IsNullOrEmpty(valueObj))
                {
                    object propertyValue = target.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
                    valueObj = propertyValue != null ? propertyValue.ToString() : null;
                }

                foundElementValue = !string.IsNullOrEmpty(valueObj) ? valueObj : "";
                if (string.Equals(foundElementValue, expectedValue, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                System.Threading.Thread.Sleep(delayBetweenRetriesMs);
            }

            return false;
        }
        public static void PerformCheckBoxActionsClickElement(IUIAutomationElement targetElement, string value, string button = "Left")
        {
            try
            {
                object togglePatternObj = targetElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                if (togglePatternObj != null)
                {
                    IUIAutomationTogglePattern togglePattern = togglePatternObj as IUIAutomationTogglePattern;
                    if (togglePattern != null)
                    {
                        ToggleState toggleState = togglePattern.CurrentToggleState;
                        Console.WriteLine("Current toggle state: " + toggleState.ToString());
                        if (value.ToString().Contains("ToggleState_"))
                        {
                            if (!string.Equals(toggleState.ToString(), value.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    togglePattern.Toggle();
                                    Thread.Sleep(1500);
                                    toggleState = togglePattern.CurrentToggleState;

                                    if (!string.Equals(toggleState.ToString(), value.ToString(), StringComparison.OrdinalIgnoreCase))
                                    {
                                        MouseOperations.ClickElement(targetElement, button);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Toggle failed: " + ex.Message);
                                    MouseOperations.ClickElement(targetElement, button);
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(value))
                        {
                            toggleState = togglePattern.CurrentToggleState;
                            if (!toggleState.ToString().Equals("ToggleState_On"))
                            {
                                togglePattern.Toggle();
                            }
                        }

                    }
                }
            }
            catch { }


        }

        public void EditRowDetails(IUIAutomationElement gridElement, DataTable dataTable)
        {
            var rawChildren = gridElement.FindAll(
                        TreeScope.TreeScope_Descendants,
                        _uiAutomation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId));

            for (int i = 0; i < rawChildren.Length; i++)
            {
                var childs = rawChildren.GetElement(i).FindAll(TreeScope.TreeScope_Children, _uiAutomation.CreateTrueCondition());
                DataRow dr = dataTable.Rows[i];
                for (int j = 1; j < childs.Length; j++)
                {
                    string value = GetValueOfChild(childs.GetElement(j));
                    Console.WriteLine(childs.GetElement(j).CurrentName + " -> " + value);
                    if (dr.Table.Columns.Contains(childs.GetElement(j).CurrentName.ToString()) && !dr[childs.GetElement(j).CurrentName.ToString()].ToString().ToLower().Equals(value.ToLower()))
                    {
                        if (string.IsNullOrEmpty(value))
                            continue;
                        SetValueOfChild(childs.GetElement(j), dr[childs.GetElement(j).CurrentName.ToString()].ToString());
                    }

                }
            }
        }

        public void SetValueOfChild(IUIAutomationElement targetElement, string value)
        {
            try
            {
                int controlType = targetElement.CurrentControlType;
                switch (controlType)
                {
                    case UIA_ControlTypeIds.UIA_EditControlTypeId:
                        UIAutomationHelper.SetValue(targetElement, value.ToString());
                        break;
                    case UIA_ControlTypeIds.UIA_ComboBoxControlTypeId:
                        ComboBoxSetValue(targetElement, value.ToString());
                        break;

                    case UIA_ControlTypeIds.UIA_RadioButtonControlTypeId:
                        UIAutomationHelper.SetRadioButton(targetElement, value.ToString());
                        break;

                    case UIA_ControlTypeIds.UIA_CustomControlTypeId:
                        UIAutomationHelper.SetValueCustom(targetElement, value.ToString());
                        break;

                    case UIA_ControlTypeIds.UIA_TabItemControlTypeId:
                        UIAutomationHelper.ClickElement(targetElement);
                        break;

                    case UIA_ControlTypeIds.UIA_CheckBoxControlTypeId:
                        UIAutomationHelper.PerformCheckBoxActionsClickElement(targetElement, value.ToString(), "Left");
                        break;

                    case UIA_ControlTypeIds.UIA_SpinnerControlTypeId:
                        UIAutomationHelper.SetValueForSpinner(targetElement, value.ToString());
                        break;
                    case UIA_ControlTypeIds.UIA_PaneControlTypeId:
                        UIAutomationHelper.SetvalueForPane(targetElement, value.ToString());
                        break;
                    default:
                        UIAutomationHelper.ClickElement(targetElement);
                        break;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }

        private void ComboBoxSetValue(IUIAutomationElement comboBox, string valueToSelect)
        {
            object expandPatternObj = comboBox.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
            if (expandPatternObj != null)
            {
                IUIAutomationExpandCollapsePattern expandCollapse = expandPatternObj as IUIAutomationExpandCollapsePattern;
                if (expandCollapse.CurrentExpandCollapseState == ExpandCollapseState.ExpandCollapseState_Collapsed)
                {
                    expandCollapse.Expand();
                    Thread.Sleep(600); // Allow dropdown to appear
                }
            }

            IUIAutomationCondition listItemCondition = _uiAutomation.CreateAndCondition(
                _uiAutomation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId),
                _uiAutomation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, valueToSelect)
            );

            IUIAutomationElement root = _uiAutomation.GetRootElement();
            IUIAutomationElement listItem = root.FindFirst(TreeScope.TreeScope_Subtree, listItemCondition);

            if (listItem != null)
            {
                object scrollObj = listItem.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);
                if (scrollObj != null)
                {
                    IUIAutomationScrollItemPattern scrollItem = scrollObj as IUIAutomationScrollItemPattern;
                    scrollItem.ScrollIntoView();
                }

                object selectionObj = listItem.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                if (selectionObj != null)
                {
                    IUIAutomationSelectionItemPattern selectionItem = selectionObj as IUIAutomationSelectionItemPattern;
                    selectionItem.Select();
                    Console.WriteLine("Selected ComboBox item: " + valueToSelect);
                    GridDataProvider.click(listItem);
                }
                else
                {
                    Console.WriteLine("SelectionItemPattern not supported on item.");
                }
            }
            else
            {
                Console.WriteLine("ListItem with name '" + valueToSelect + "' not found.");
            }
        }


        public string GetValueOfChild(IUIAutomationElement iUIAutomationElement)
        {
            string value = string.Empty;
            var controlType = iUIAutomationElement.CurrentControlType;
            switch (controlType)
            {
                case UIA_ControlTypeIds.UIA_CheckBoxControlTypeId:
                    var togglePatternObj = iUIAutomationElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                    if (togglePatternObj != null)
                    {
                        var togglePattern = (IUIAutomationTogglePattern)togglePatternObj;
                        value = togglePattern.CurrentToggleState.ToString();
                    }
                    break;

                case UIA_ControlTypeIds.UIA_ComboBoxControlTypeId:
                    var selectionPatternObj = iUIAutomationElement.GetCurrentPattern(UIA_PatternIds.UIA_SelectionPatternId);
                    if (selectionPatternObj != null)
                    {
                        var selectionPattern = (IUIAutomationSelectionPattern)selectionPatternObj;
                        var selectedItems = selectionPattern.GetCurrentSelection();
                        if (selectedItems.Length > 0)
                        {
                            value = selectedItems.GetElement(0).CurrentName;
                        }
                    }
                    if (string.IsNullOrEmpty(value) || value == "")
                    {
                        var listItemCondition = _uiAutomation.CreatePropertyCondition(
                            UIA_PropertyIds.UIA_ControlTypePropertyId,
                            UIA_ControlTypeIds.UIA_ListItemControlTypeId);

                        var listItems = iUIAutomationElement.FindAll(TreeScope.TreeScope_Subtree, listItemCondition);

                        for (int j = 0; j < listItems.Length; j++)
                        {
                            var listItem = listItems.GetElement(j);
                            var patternObj = listItem.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                            if (patternObj != null)
                            {
                                var selectionItemPattern = (IUIAutomationSelectionItemPattern)patternObj;
                                if (selectionItemPattern.CurrentIsSelected == 1)
                                {
                                    value = listItem.CurrentName;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case UIA_ControlTypeIds.UIA_EditControlTypeId:
                    var valuePatternObj = iUIAutomationElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    if (valuePatternObj != null)
                    {
                        var valuePattern = (IUIAutomationValuePattern)valuePatternObj;
                        value = valuePattern.CurrentValue;
                    }
                    break;
            }
            return value;
        }

        public IUIAutomationElement CreateUIElement(string value)
        {
            IUIAutomationElement appWindow = null;
            IUIAutomationElement securityMaster = null;
            if (value.Split(',').Length == 1)
            {
                appWindow = _uiAutomation.GetRootElement().FindFirst(
                    TreeScope.TreeScope_Children,
                    _uiAutomation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, value.Split(',')[0].ToString()));
                return appWindow;
            }
            else if (value.Split(',').Length == 2)
            {
                appWindow = _uiAutomation.GetRootElement().FindFirst(
                     TreeScope.TreeScope_Children,
                     _uiAutomation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, value.Split(',')[0].ToString()));
                securityMaster = appWindow.FindFirst(
                TreeScope.TreeScope_Descendants,
                _uiAutomation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, value.Split(',')[1].ToString()));
                return securityMaster;
            }
            else
            {
                appWindow = _uiAutomation.GetRootElement().FindFirst(
                     TreeScope.TreeScope_Children,
                     _uiAutomation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, value.Split(',')[0].ToString()));
                securityMaster = appWindow.FindFirst(
                TreeScope.TreeScope_Descendants,
                _uiAutomation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, value.Split(',')[1].ToString()));
                IUIAutomationElement gridElement = securityMaster.FindFirst(
                        TreeScope.TreeScope_Descendants,
                        _uiAutomation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ColScrollRegion: 0, RowScrollRegion: 0"));
                return gridElement;
            }

        }

        public void OpenSmGrid(IUIAutomationElement targetElement)
        {

            var rawChildren = targetElement.FindAll(
                        TreeScope.TreeScope_Descendants,
                        _uiAutomation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId));
            var child = rawChildren.GetElement(0);
            var childs = child.FindAll(TreeScope.TreeScope_Children, _uiAutomation.CreateTrueCondition());
            UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
            uiAutomationHelper.FindAndDoubleClickElement(childs.GetElement(1), "");

        }

        public void FindAndClickElementIfVisible(string element, string button = "Left")
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();

                IUIAutomationElement rootElement = automation.GetRootElement().FindFirst(
                    TreeScope.TreeScope_Children,
                    automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana"));

                IUIAutomationElement windowElement = null;
                try
                {
                    windowElement = FindElementByUniquePropertyType(
                        automation,
                        rootElement,
                        ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType,
                        ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error finding Window element: {0}", ex.Message));
                }

                if (windowElement != null)
                {
                    IUIAutomationElement targetElement = FindElementByUniquePropertyType(
                        automation,
                        windowElement,
                        ApplicationArguments.mapdictionary[element].UniquePropertyType,
                        ApplicationArguments.mapdictionary[element].AutomationUniqueValue,
                        ApplicationArguments.mapdictionary[element].ControlType);

                    if (targetElement != null)
                    {

                        if (targetElement.CurrentBoundingRectangle.left > 0 &&
targetElement.CurrentBoundingRectangle.top > 0 && targetElement.CurrentBoundingRectangle.right > 0 &&
targetElement.CurrentBoundingRectangle.bottom > 0)
                        {
                            MouseOperations.ClickElement(targetElement, button);
                            Thread.Sleep(4000);
                        }

                        else
                        {
                            Console.WriteLine("Element found, but bounding rectangle is not valid for clicking.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Target element not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception: {0}", ex.Message));
                throw;
            }
        }

        /// <summary>
        /// ///
        /// </summary>
        /// <param name="GetTargetedGridData"></param>
        /// <returns>DataSet</returns>
        public DataSet GetTargetedGridData(Dictionary<string, List<FocusGridData>> GridFocusDataDictionary, string moduleID, string gridID, string DataExtractionScriptName, string framework)
        {
            DataSet dataSet = new DataSet();
            IUIAutomation automation = new CUIAutomation();
            IUIAutomationElement rootElement = automation.GetRootElement();
            UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
            try
            {
                if (GridFocusDataDictionary.ContainsKey(gridID))
                {
                    List<FocusGridData> focusGridList = GridFocusDataDictionary[gridID];

                    foreach (FocusGridData gridData in focusGridList)
                    {
                        if (!string.Equals(gridData.WindowAutomationId, moduleID, StringComparison.OrdinalIgnoreCase))
                            continue;

                        string scriptName = gridData.DataExtractionScriptName;

                        IUIAutomationCondition windowCondition = automation.CreatePropertyCondition(
                            UIA_PropertyIds.UIA_AutomationIdPropertyId, moduleID);
                        IUIAutomationElement mainWindowElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, windowCondition);

                        if (mainWindowElement == null)
                        {
                            Console.WriteLine("Main window element not found.");
                            return null;
                        }

                        IntPtr hWnd = (IntPtr)mainWindowElement.CurrentNativeWindowHandle;
                        SetForegroundWindow(hWnd);

                        IUIAutomationElement GridCustomControlType = null;
                        IUIAutomationElement TreeControlType = null;
                        IUIAutomationElement DataGridControlTypeId = null;

                        //uiAutomationHelper.FindRequiredControlTypes(
                        //                                                     gridID,
                        //                                                     automation,
                        //                                                     mainWindowElement,
                        //                                                     ref GridCustomControlType,
                        //                                                     ref TreeControlType,
                        //                                                     ref DataGridControlTypeId,
                        //                                                     framework);
                        //method showing argument errors due to lower version
                        mainWindowElement.SetFocus();
                        try
                        {
                            if (framework == "WPF")
                            {
                                if (automation != null && mainWindowElement != null)
                                {

                                    IUIAutomationCondition controlTypeCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId, gridID);

                                    GridCustomControlType = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, controlTypeCondition);

                                    IUIAutomationCondition dataGridCondition = automation.CreatePropertyCondition(
                                        UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);

                                    IUIAutomationCondition combinedCondition = automation.CreateAndCondition(controlTypeCondition, dataGridCondition);

                                }
                            }
                            else if (framework == "WinForm")
                            {
                                string treeViewAutomationId = "ColScrollRegion: 0, RowScrollRegion: 0";

                                if (automation != null && mainWindowElement != null)
                                {

                                    IUIAutomationCondition combinedCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, gridID);
                                    GridCustomControlType = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                                    IUIAutomationCondition treeViewCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                                    TreeControlType = GridCustomControlType.FindFirst(TreeScope.TreeScope_Descendants, treeViewCondition);

                                }
                            }
                        }
                        catch { }

                        if (string.Equals(scriptName, "ReviewExistingPositions"))
                        {
                            dataSet = ReviewExistingPositions(mainWindowElement, moduleID, "", gridData.LogAutomationID);
                            return dataSet;
                        }
                        else if (string.Equals(scriptName, "NAVLockGridData") || string.Equals(scriptName, "getCreatePositionData"))
                        {
                            DataTable dt = Retry3Times(() =>
                            {
                                DataTable temp = CreateNAVGRIDDataTable(GridCustomControlType, automation);
                                Stopwatch stopwatch = new Stopwatch();
                                stopwatch.Start();
                                PopulateDataRowsNAVGridOptimized(temp, GridCustomControlType, automation, moduleID, false);
                                stopwatch.Stop();
                                Console.WriteLine("Time taken to extract data: " + stopwatch.Elapsed.TotalSeconds + " seconds");
                                return temp;
                            });
                            if (dt != null)
                            {
                                dt.TableName = gridData.LogAutomationID;
                                dataSet.Tables.Add(dt);
                            }
                            return dataSet;
                        }
                        else if (string.Equals(scriptName, "AttributeRemaining",StringComparison.OrdinalIgnoreCase) )
                        {
                            DataTable dt = Retry3Times(() =>
                            {
                                DataTable temp = AttributeRemaining(moduleID, gridID);
                                if (temp != null && temp.Rows.Count > 0)
                                {
                                    return temp;
                                }

                                return null; // Retry if empty or null
                            });

                            if (dt != null)
                            {
                                dt.TableName = gridData.LogAutomationID;
                                dataSet.Tables.Add(dt);
                            }
                            else
                            {
                                Console.WriteLine("Failed to retrieve non-empty data after 3 attempts.");
                            }

                            return dataSet;
                        }
                        else if (string.Equals(scriptName, "ColumnChooserDialog", StringComparison.OrdinalIgnoreCase))
                        {
                            DataTable dt = Retry3Times(() =>
                            {
                                DataTable temp = ColumnChooserDialog(moduleID, gridID);
                                if (temp != null && temp.Rows.Count > 0)
                                {
                                    return temp;
                                }

                                return null; 
                            });

                            if (dt != null)
                            {
                                dt.TableName = gridData.LogAutomationID;
                                dataSet.Tables.Add(dt);
                            }
                            else
                            {
                                Console.WriteLine("Failed to retrieve ColumnChooserDialog data after 3 attempts.");
                            }

                            return dataSet;
                        }
                        else
                        {
                            DataTable dataTable = null;
                            bool isDataTableEmpty = true;
                            int retryCount = 0;

                            while (retryCount < 3 && isDataTableEmpty)
                            {
                                try
                                {
                                    AsyncGridExtractor asyncGridExtractor = new AsyncGridExtractor();
                                    DataSet asyncDataSet = asyncGridExtractor.ExtractGridDataAsDataSet(moduleID, gridID, gridData.LogAutomationID);
                                    if (asyncDataSet != null)
                                        return asyncDataSet;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Exception occurred while Async: " + ex.Message);
                                }

                                if (framework == "WPF")
                                {
                                    GridDataProvider gridDataProvider = new GridDataProvider();
                                    dataTable = gridDataProvider.GetWPFGridDataNew(moduleID, gridID, false);
                                }
                                else if (framework == "WinForm")
                                {
                                    dataTable = CreateDataTable(GridCustomControlType, automation);
                                    Stopwatch stopwatch = new Stopwatch();
                                    stopwatch.Start();
                                    PopulateDataRowsOptimized(dataTable, GridCustomControlType, automation, moduleID, false);
                                    stopwatch.Stop();
                                    Console.WriteLine("Time taken to extract data: " + stopwatch.Elapsed.TotalSeconds + " seconds");
                                }

                                if (dataTable != null && dataTable.Rows.Count > 0)
                                {
                                    isDataTableEmpty = false;
                                }
                                else
                                {
                                    retryCount++;
                                }
                            }

                            if (!isDataTableEmpty && dataTable != null)
                            {
                                dataTable.TableName = gridID;
                                dataSet.Tables.Add(dataTable);
                            }

                            return dataSet;
                        }
                    }
                }

                // fallback async call
                bool isAsyncDataSucceeded = false;
                try
                {
                    AsyncGridExtractor asyncGridExtractor = new AsyncGridExtractor();
                    DataSet asyncDataSet = asyncGridExtractor.ExtractGridDataAsDataSet(moduleID, gridID);
                    if (asyncDataSet != null && asyncDataSet.Tables.Count > 0)
                    {
                        isAsyncDataSucceeded = true;
                        return asyncDataSet;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occurred while Async: " + ex.Message);
                }

                if (!isAsyncDataSucceeded)
                {
                    DataTable fallbackTable = null;
                    GridDataProvider gridDataProvider = new GridDataProvider();
                    if (framework == "WPF")
                    {
                        fallbackTable = gridDataProvider.GetWPFGridDataNew(moduleID, gridID, false);
                    }
                    else if (framework == "WinForm")
                    {
                        IUIAutomationCondition windowCondition = automation.CreatePropertyCondition(
                           UIA_PropertyIds.UIA_AutomationIdPropertyId, moduleID);
                        IUIAutomationElement mainWindowElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, windowCondition);

                        if (mainWindowElement == null)
                        {
                            Console.WriteLine("Main window element not found.");
                            return null;
                        }
                        IUIAutomationElement GridCustomControlType = null;
                        IUIAutomationElement TreeControlType = null;
                        IUIAutomationElement DataGridControlTypeId = null;

                        try
                        {
                            mainWindowElement.SetFocus();
                            if (framework == "WPF")
                            {
                                if (automation != null && mainWindowElement != null)
                                {

                                    IUIAutomationCondition controlTypeCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId, gridID);

                                    GridCustomControlType = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, controlTypeCondition);

                                    IUIAutomationCondition dataGridCondition = automation.CreatePropertyCondition(
                                        UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);

                                    IUIAutomationCondition combinedCondition = automation.CreateAndCondition(controlTypeCondition, dataGridCondition);

                                }
                            }
                            else if (framework == "WinForm")
                            {
                                string treeViewAutomationId = "ColScrollRegion: 0, RowScrollRegion: 0";

                                if (automation != null && mainWindowElement != null)
                                {

                                    IUIAutomationCondition combinedCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, gridID);
                                    GridCustomControlType = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                                    IUIAutomationCondition treeViewCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                                    TreeControlType = GridCustomControlType.FindFirst(TreeScope.TreeScope_Descendants, treeViewCondition);

                                }
                            }
                        }
                        catch { }

                        fallbackTable = CreateDataTable(GridCustomControlType, automation);
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        PopulateDataRowsOptimized(fallbackTable, GridCustomControlType, automation, moduleID, false);
                        stopwatch.Stop();
                        Console.WriteLine("Time taken to extract data: " + stopwatch.Elapsed.TotalSeconds + " seconds");
                    }

                    if (fallbackTable != null)
                    {
                        fallbackTable.TableName = gridID;
                        dataSet.Tables.Add(fallbackTable);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred while printing element details: " + ex.Message);
            }

            return dataSet;
        }
        public static DataTable ColumnChooserDialog(string moduleID, string gridID)
        {
            DataTable dt = new DataTable();

            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[moduleID]);

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement windowElement = FindElementByUniquePropertyType(
                    automation,
                    rootElement,
                    ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].UniquePropertyType,
                    ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                if (windowElement == null)
                {
                    throw new Exception("Module window not found.");
                }

                DetectAndSwitchWindow(ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                IUIAutomationCondition gridCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    ApplicationArguments.mapdictionary[gridID + "GridName"].AutomationUniqueValue);

                IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, gridCondition);

                IUIAutomationElementArray gridChildren = gridElementMain.FindAll(
                    TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                for (int i = 0; i < gridChildren.Length; i++)
                {
                    IUIAutomationElement potentialHeaderRow = gridChildren.GetElement(i);
                    string name = potentialHeaderRow.CurrentName;

                    if (string.IsNullOrEmpty(name))
                    {
                        IUIAutomationElementArray headerChildren = potentialHeaderRow.FindAll(
                            TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                        for (int j = 0; j < headerChildren.Length; j++)
                        {
                            IUIAutomationElement headerElement = headerChildren.GetElement(j);
                            string columnName = headerElement.CurrentName;

                            if (!string.IsNullOrEmpty(columnName))
                            {
                                if (!dt.Columns.Contains(columnName))
                                {
                                    dt.Columns.Add(columnName);
                                }
                            }
                        }

                        break; 
                    }
                }
                for (int i = 0; i < gridChildren.Length; i++)
                {
                    IUIAutomationElement rowElement = gridChildren.GetElement(i);

                    try
                    {
                        string rowName = rowElement.CurrentName;
                        if (!string.IsNullOrEmpty(rowName)) 
                        {
                            DataRow dr = dt.NewRow();

                            IUIAutomationElementArray rowChildren = rowElement.FindAll(
                                TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                            for (int k = 0; k < rowChildren.Length && k < dt.Columns.Count; k++)
                            {
                                try
                                {
                                    IUIAutomationElement cellElement = rowChildren.GetElement(k);
                                    object value = cellElement.GetCurrentPropertyValue(
                                        UIA_PropertyIds.UIA_ValueValuePropertyId);
                                    dr[k] = value != null ? value.ToString() : string.Empty;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error reading cell value at row " + i + ", column " + k + ": " + ex.Message);
                                }
                            }

                            bool hasData = false;
                            for (int m = 0; m < dr.ItemArray.Length; m++)
                            {
                                if (dr[m] != null && dr[m].ToString() != string.Empty)
                                {
                                    hasData = true;
                                    break;
                                }
                            }

                            if (hasData && !RowExists(dt, dr))
                            {
                                lock (dt)
                                {
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error processing row " + i + ": " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting grid data: " + ex.Message);
            }

            return dt;
        }

        public static DataTable AttributeRemaining(string moduleID, string gridID)
        {
            DataTable dt = new DataTable();
            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[moduleID]);

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement windowElement = FindElementByUniquePropertyType(
                    automation,
                    rootElement,
                    ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType,
                    ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                if (windowElement == null)
                {
                    throw new Exception("Module window not found.");
                }

                DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                IUIAutomationCondition gridCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    ApplicationArguments.mapdictionary[gridID + "GridName"].AutomationUniqueValue);

                IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, gridCondition);

                if (gridElementMain == null)
                {
                    IUIAutomationElementArray descendants = windowElement.FindAll(TreeScope.TreeScope_Descendants, automation.CreateTrueCondition());
                    Console.WriteLine("Logging all descendants under the window element:");

                    for (int i = 0; i < descendants.Length; i++)
                    {
                        IUIAutomationElement element = descendants.GetElement(i);
                        Console.WriteLine("Element " + (i + 1) + ": AutomationId = " + element.CurrentAutomationId + ", Name = " + element.CurrentName);
                    }

                    throw new Exception("Grid element not found.");
                }

                IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_NamePropertyId,
                    ApplicationArguments.mapdictionary[gridID + "FirstDataGridName"].AutomationUniqueValue);

                IUIAutomationCondition combinedCondition = automation.CreateAndCondition(gridControlTypeCondition, nameCondition);

                IUIAutomationElement recordElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, gridControlTypeCondition);
                IUIAutomationElement gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                if (gridElement == null)
                {
                    IUIAutomationCondition listControlCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListControlTypeId);
                    gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, listControlCondition);
                }

                if (gridElement == null)
                {
                    IUIAutomationElementArray recordElementChildren = recordElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    if (recordElementChildren != null)
                    {
                        gridElement = recordElement;
                    }
                }

                IUIAutomationElementArray gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                IUIAutomationElement thirdChildren = gridChildren.GetElement(3).FindFirst(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                MouseOperations.ClickElement(thirdChildren);
                MouseOperations.ClickElement(thirdChildren);

                var sim = new InputSimulator();
                for (int i = 0; i < gridChildren.Length + 10; i++)
                {
                    try
                    {
                        sim.Keyboard.KeyPress(VirtualKeyCode.UP);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                string[] columnNames = ApplicationArguments.mapdictionary["ColumnNames"].AutomationUniqueValue.ToString().Split(',');
                for (int i = 0; i < columnNames.Length; i++)
                {
                    string columnName = columnNames[i].Trim();
                    if (!dt.Columns.Contains(columnName))
                    {
                        dt.Columns.Add(columnName, typeof(string));
                    }
                }

                gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[moduleID]);

                int limit;
                string limitString = ApplicationArguments.mapdictionary["TradeAttributeLimit"].ToString();
                if (!int.TryParse(limitString, out limit))
                {
                    Console.WriteLine("Invalid TradeAttributeLimit value. Defaulting to 3.");
                    limit = 3;
                }

                while (limit > 0)
                {
                    Console.WriteLine("[Loop Start] limit = " + limit.ToString());

                    try
                    {
                        ExtractGridDataToDataTable(gridElement, dt, automation);
                        Console.WriteLine("ExtractGridDataToDataTable succeeded.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in ExtractGridDataToDataTable: " + ex.Message);
                        break;
                    }

                     gridChildren = null;
                    try
                    {
                        gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        Console.WriteLine("Found " + gridChildren.Length.ToString() + " children in gridElement.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error finding grid children: " + ex.Message);
                        break;
                    }

                     thirdChildren = null;
                    try
                    {
                        int targetIndex = gridChildren.Length >= 3 ? gridChildren.Length - 3 : 0;
                        IUIAutomationElement fallbackChild = gridChildren.GetElement(targetIndex);
                        thirdChildren = fallbackChild.FindFirst(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                        Console.WriteLine("Retrieved thirdChildren element.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error accessing thirdChildren: " + ex.Message);
                        break;
                    }

                    try
                    {
                        MouseOperations.ClickElement(thirdChildren);
                        MouseOperations.ClickElement(thirdChildren);
                        Console.WriteLine("Clicked thirdChildren element twice.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error clicking thirdChildren: " + ex.Message);
                    }
                    for (int i = 0; i < gridChildren.Length; i++)
                    {
                        try
                        {
                            sim.Keyboard.KeyPress(VirtualKeyCode.DOWN);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    limit--;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return dt;
        }

        public static DataTable EditAttributeRemaining(string moduleID, string gridID,DataTable UIData,DataTable ExcelData)
        {
            DataTable dt = new DataTable();
            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[moduleID]);

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement windowElement = FindElementByUniquePropertyType(
                    automation,
                    rootElement,
                    ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType,
                    ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                if (windowElement == null)
                {
                    throw new Exception("Module window not found.");
                }

                DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                IUIAutomationCondition gridCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    ApplicationArguments.mapdictionary[gridID + "GridName"].AutomationUniqueValue);

                IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, gridCondition);

                if (gridElementMain == null)
                {
                    IUIAutomationElementArray descendants = windowElement.FindAll(TreeScope.TreeScope_Descendants, automation.CreateTrueCondition());
                    Console.WriteLine("Logging all descendants under the window element:");

                    for (int i = 0; i < descendants.Length; i++)
                    {
                        IUIAutomationElement element = descendants.GetElement(i);
                        Console.WriteLine("Element " + (i + 1) + ": AutomationId = " + element.CurrentAutomationId + ", Name = " + element.CurrentName);
                    }

                    throw new Exception("Grid element not found.");
                }

                IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_NamePropertyId,
                    ApplicationArguments.mapdictionary[gridID + "FirstDataGridName"].AutomationUniqueValue);

                IUIAutomationCondition combinedCondition = automation.CreateAndCondition(gridControlTypeCondition, nameCondition);

                IUIAutomationElement recordElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, gridControlTypeCondition);
                IUIAutomationElement gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                if (gridElement == null)
                {
                    IUIAutomationCondition listControlCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListControlTypeId);
                    gridElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, listControlCondition);
                }

                if (gridElement == null)
                {
                    IUIAutomationElementArray recordElementChildren = recordElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    if (recordElementChildren != null)
                    {
                        gridElement = recordElement;
                    }
                }

                IUIAutomationElementArray gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                IUIAutomationElement thirdChildren = gridChildren.GetElement(3).FindFirst(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                MouseOperations.ClickElement(thirdChildren);
                MouseOperations.ClickElement(thirdChildren);

                var sim = new InputSimulator();
                for (int i = 0; i < gridChildren.Length + 30; i++)
                {
                    try
                    {
                        sim.Keyboard.KeyPress(VirtualKeyCode.UP);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                string[] columnNames = ApplicationArguments.mapdictionary["ColumnNames"].AutomationUniqueValue.ToString().Split(',');
                
                for (int i = 0; i < columnNames.Length; i++)
                {
                    string columnName = columnNames[i].Trim();
                    if (!dt.Columns.Contains(columnName))
                    {
                        dt.Columns.Add(columnName, typeof(string));
                    }
                }

                gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[moduleID]);

                int limit;
                string limitString = ApplicationArguments.mapdictionary["TradeAttributeLimit"].ToString();
                if (!int.TryParse(limitString, out limit))
                {
                    Console.WriteLine("Invalid TradeAttributeLimit value. Defaulting to 3.");
                    limit = 3;
                }

                while (limit > 0)
                {
                    Console.WriteLine("[Loop Start] limit = " + limit.ToString());

                    try
                    {
                        ExtractGridDataToDataTableAndEdit(gridElement, dt, automation, UIData, ExcelData);
                        Console.WriteLine("ExtractGridDataToDataTable succeeded.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in ExtractGridDataToDataTable: " + ex.Message);
                        break;
                    }

                    gridChildren = null;
                    try
                    {
                        gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        Console.WriteLine("Found " + gridChildren.Length.ToString() + " children in gridElement.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error finding grid children: " + ex.Message);
                        break;
                    }

                    thirdChildren = null;
                    try
                    {
                        int targetIndex = gridChildren.Length >= 3 ? gridChildren.Length - 3 : 0;
                        IUIAutomationElement fallbackChild = gridChildren.GetElement(targetIndex);
                        thirdChildren = fallbackChild.FindFirst(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                        Console.WriteLine("Retrieved thirdChildren element.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error accessing thirdChildren: " + ex.Message);
                        break;
                    }

                    try
                    {
                        MouseOperations.ClickElement(thirdChildren);
                        MouseOperations.ClickElement(thirdChildren);
                        Console.WriteLine("Clicked thirdChildren element twice.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error clicking thirdChildren: " + ex.Message);
                    }
                    for (int i = 0; i < gridChildren.Length; i++)
                    {
                        try
                        {
                            sim.Keyboard.KeyPress(VirtualKeyCode.DOWN);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    limit--;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return dt;
        }


        public static void ExtractGridDataToDataTable(IUIAutomationElement gridElement, DataTable dt, IUIAutomation automation)
        {
            try
            {
                IUIAutomationElementArray gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                for (int i = 0; i < gridChildren.Length; i++)
                {
                    IUIAutomationElement rowElement = gridChildren.GetElement(i);

                    try
                    {
                        DataRow dr = dt.NewRow();
                        IUIAutomationElementArray rowChildren = rowElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                        for (int k = 0; k < rowChildren.Length && k < dt.Columns.Count; k++)
                        {
                            try
                            {
                                object value = rowChildren.GetElement(k).GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
                                if (value != null)
                                {
                                    dr[k] = value.ToString();
                                }
                                else
                                {
                                    dr[k] = string.Empty;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error reading cell value at row " + i + ", column " + k + ": " + ex.Message);
                            }
                        }

                        lock (dt)
                        {
                            bool hasNonEmpty = false;
                            for (int x = 0; x < dr.ItemArray.Length; x++)
                            {
                                if (dr.ItemArray[x] != null && !string.IsNullOrEmpty(dr.ItemArray[x].ToString()))
                                {
                                    hasNonEmpty = true;
                                    break;
                                }
                            }

                            if (hasNonEmpty && !RowExists(dt, dr))
                            {
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error processing row " + i + ": " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting grid data: " + ex.Message);
            }
        }
        public static void ExtractGridDataToDataTableAndEdit(IUIAutomationElement gridElement, DataTable dt,IUIAutomation automation,DataTable UIData, DataTable ExcelData)
        {
            try
            {
                IUIAutomationElementArray gridChildren = gridElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                for (int i = 0; i < gridChildren.Length; i++)
                {
                    IUIAutomationElement rowElement = gridChildren.GetElement(i);

                    try
                    {
                        DataRow dr = dt.NewRow();

                        //find index of AttributeValue column
                        IUIAutomationElementArray rowChildren = rowElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        string valueToEnter = string.Empty;
                        
                        for (int k = 0; k < rowChildren.Length && k < dt.Columns.Count; k++)
                        {
                            try
                            {
                               
                                object value = rowChildren.GetElement(k).GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);

                                if (value != null)
                                {
                                    dr[k] = value.ToString();
                                }
                                else
                                {
                                    dr[k] = string.Empty;
                                }


                                
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error reading cell value at row " + i + ", column " + k + ": " + ex.Message);
                            }
                        }

                        if (dt.Columns.Contains("AttributeValue"))
                        {
                            string attrValue = dr["AttributeValue"] != null ? dr["AttributeValue"].ToString() : string.Empty;
                            string filter = "AttributeValue = '" + attrValue.Replace("'", "''") + "'";
                            DataRow[] foundExcelRows = ExcelData.Select(filter);

                            if (foundExcelRows.Length > 0)
                            {
                                DataRow excelRow = foundExcelRows[0];
                                int maxCols = Math.Min(4, Math.Min(rowChildren.Length, dt.Columns.Count));
                                for (int k = 0; k < maxCols; k++)
                                {
                                    string colName = dt.Columns[k].ColumnName;
                                    string uiValue = dr[k] != null ? dr[k].ToString() : string.Empty;
                                    string excelValue = excelRow[colName] != null ? excelRow[colName].ToString() : string.Empty;

                                    if (uiValue != excelValue)
                                    {
                                        try
                                        {
                                            IUIAutomationElement cell = rowChildren.GetElement(k);

                                            string cellName = cell.CurrentName != null ? cell.CurrentName : "";

                                            if (cellName.Equals("KeepRecord", StringComparison.OrdinalIgnoreCase))
                                            {
                                                MouseOperations.ClickElementLeftPartial(cell);
                                                Keyboard.SendKeys(KeyboardConstants.SPACE);
                                                Console.WriteLine("Toggled 'KeepRecord' cell at row " + i + ", column '" + colName + "'");
                                            }
                                            else
                                            {
                                                bool setSuccess = false;
                                                int retryCount = 3;

                                                while (!setSuccess && retryCount > 0)
                                                {
                                                    try
                                                    {
                                                        MouseOperations.ClickElement(cell);
                                                        
                                                            MouseOperations.ClickElement(cell);
                                                            Keyboard.SendKeys(KeyboardConstants.CTRLA);
                                                            Thread.Sleep(100); // brief pause to allow key processing
                                                            Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                                                            Thread.Sleep(100);

                                                            Keyboard.SendKeys(excelValue);

                                                            bool result = true;
                                                            object updatedVal = ControlTypeHandler.getValueOfElementAdvance(cell, ref result);

                                                        string updated = updatedVal != null ? updatedVal.ToString() : "";

                                                        if (updated == excelValue)
                                                        {
                                                            Console.WriteLine("Successfully updated value at row " + i + ", column '" + colName + "'");
                                                            setSuccess = true;
                                                        }
                                                        else
                                                        {
                                                            retryCount--;
                                                            Console.WriteLine("Retrying to update value at row " + i + ", column '" + colName + "'. Remaining attempts: " + retryCount);
                                                        }
                                                    }
                                                    catch (Exception retryEx)
                                                    {
                                                        Console.WriteLine("Retry error at row " + i + ", column '" + colName + "': " + retryEx.Message);
                                                        retryCount--;
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception setEx)
                                        {
                                            Console.WriteLine("Error setting value at row " + i + ", column '" + colName + "': " + setEx.Message);
                                        }
                                    }
                                }
                            }
                        }


                        //now check dr contains ("AttributeValue") column, if yes then
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error processing row " + i + ": " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting grid data: " + ex.Message);
            }
        }
        public static bool RowExists(DataTable dt, DataRow newRow)
        {
            try
            {
                foreach (DataRow existingRow in dt.Rows)
                {
                    bool isDuplicate = true;

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string existingValue = "";
                        string newValue = "";

                        if (existingRow[i] != null)
                        {
                            existingValue = existingRow[i].ToString();
                        }

                        if (newRow[i] != null)
                        {
                            newValue = newRow[i].ToString();
                        }

                        if (!existingValue.Equals(newValue))
                        {
                            isDuplicate = false;
                            break;
                        }
                    }

                    if (isDuplicate)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in RowExists: " + ex.Message);
            }

            return false;
        }

        private static DataTable CreateNAVGRIDDataTable(IUIAutomationElement gridElement, IUIAutomation automation)
        {
            try
            {
                DataTable dataTable = new DataTable();

                // Get headers
                List<IUIAutomationElement> headerElements = GetHeaderElements(gridElement, automation);
                foreach (IUIAutomationElement headerElement in headerElements)
                {
                    IUIAutomationCondition condition = automation.CreateTrueCondition(); // Match all sub-elements
                    IUIAutomationElementArray childArray = headerElement.FindAll(TreeScope.TreeScope_Children, condition);

                    for (int i = 0; i < childArray.Length; i++)
                    {
                        IUIAutomationElement childElement = childArray.GetElement(i);
                        string headerName = null;

                        if (!string.IsNullOrWhiteSpace(childElement.CurrentName))
                        {
                            headerName = childElement.CurrentName;
                        }
                        else if (!string.IsNullOrWhiteSpace(childElement.CurrentAutomationId))
                        {
                            headerName = childElement.CurrentAutomationId;
                        }
                        else
                        {
                            headerName = "Unnamed Header";
                        }

                        if (!dataTable.Columns.Contains(headerName))
                        {
                            dataTable.Columns.Add(headerName);
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error in CreateDataTable: {0}", ex.Message));
                return new DataTable(); // Return an empty DataTable on error
            }
        }

        private static void PopulateDataRowsNAVGridOptimized(DataTable dataTable, IUIAutomationElement gridElement, IUIAutomation automation, string parentAutomationID, bool allowParallel)
        {
            try
            {
                var dataItemCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_ControlTypePropertyId,
                    UIA_ControlTypeIds.UIA_DataItemControlTypeId);

                IUIAutomationElementArray dataItems = gridElement.FindAll(TreeScope.TreeScope_Descendants, dataItemCondition);

                for (int i = 0; i < dataItems.Length; i++)
                {
                    try
                    {
                        IUIAutomationElement dataItem = dataItems.GetElement(i);
                        DataRow row = dataTable.NewRow();

                        IUIAutomationElementArray dataItemChildren = dataItem.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        for (int j = 0; j < dataItemChildren.Length; j++)
                        {
                            IUIAutomationElement cellElement = dataItemChildren.GetElement(j);
                            string columnName = cellElement.CurrentName;
                            bool resulttemp = false;
                            string cellValue = ControlTypeHandler.getValueOfElement(cellElement, ref resulttemp);

                            if (string.IsNullOrEmpty(columnName))
                            {
                                columnName = "[Column Header] Delete";
                                if (string.Equals(cellValue, "Delete", StringComparison.OrdinalIgnoreCase))
                                {
                                    row[columnName] = cellValue;
                                }
                                else
                                {
                                    row[columnName] = "BLANK";
                                }
                            }
                            else if (dataTable.Columns.Contains(columnName))
                            {
                                row[columnName] = cellValue;
                            }
                        }

                        dataTable.Rows.Add(row);
                    }
                    catch (Exception exInner)
                    {
                        Console.WriteLine(string.Format("Error processing data item {0}: {1}", i, exInner.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error in PopulateDataRowsOptimized: {0}", ex.Message));
            }
        }
        private static DataSet ReviewExistingPositions(IUIAutomationElement mainWindowElement, string parentAutomationID, string firstParentofElement, string LogAutomationID = "")
        {
            DataSet ds = new DataSet();
            IUIAutomation automation = new CUIAutomation();
            IUIAutomationElement rootElement = automation.GetRootElement();
            IUIAutomationElementArray gridElements = null;
            try
            {
                //ProgramMain.RecorderMappings
                IUIAutomationCondition CurrentStatePaneCondition = automation.CreatePropertyCondition(
                                    UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["CurrentStatePane"].AutomationUniqueValue);


                IUIAutomationElement panel = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, CurrentStatePaneCondition);

                if (panel != null)
                {

                    IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);

                    gridElements = panel.FindAll(TreeScope.TreeScope_Descendants, gridControlTypeCondition);

                    List<string> dataSetTableName = ApplicationArgumentsAndConstants
                                                    .ReviewExistingPositionsLogAutomationID
                                                    .Split(',')
                                                    .ToList();

                    for (int iNDEX = 0; iNDEX < gridElements.Length; iNDEX++)
                    {

                        IUIAutomationElement gridElement = gridElements.GetElement(iNDEX);
                        try
                        {
                            //DataTable dataTable = GetNormalWPFGridData(gridElement, automation);

                            DataTable dt = new DataTable();
                            dt.Columns.Add("Symbol");
                            dt.Columns.Add("AccountName");
                            dt.Columns.Add("Quantity");
                            dt.Columns.Add("Notional");

                            var rawChildren = gridElement.FindAll(TreeScope.TreeScope_Subtree, automation.CreateTrueCondition());
                            for (int i = 0; i < rawChildren.Length; i++)
                            {
                                var child = rawChildren.GetElement(i);

                                if (child.CurrentControlType == UIA_ControlTypeIds.UIA_DataGridControlTypeId)
                                {
                                    var dataItemChildren = child.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                                    for (int j = 0; j < dataItemChildren.Length; j++)
                                    {
                                        DataRow dr = dt.NewRow();
                                        var dataItemChild = dataItemChildren.GetElement(j);
                                        var dataItemDesc = dataItemChild.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                                        for (int k = 0; k < dataItemDesc.Length; k++)
                                        {
                                            var ele = dataItemDesc.GetElement(k);
                                            dr[k] = ele.CurrentName.ToString();
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            if (dt != null)
                            {
                                dt.TableName = dataSetTableName[iNDEX];
                                ds.Tables.Add(dt);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception in ReviewExistingPositions: " + ex.Message);
                            return null;
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ReviewExistingPositions: " + ex.Message);
                return null;
            }
        }
        private static  DataTable Retry3Times(Func<DataTable> func)
        {
            int retryCount = 0;
            DataTable result = null;

            while (retryCount < 3)
            {
                result = func();
                if (result != null && result.Rows.Count > 0)
                    return result;

                retryCount++;
            }

            Console.WriteLine("Failed to retrieve non-empty data after 3 attempts.");
            return null;
        }

        private void FindRequiredControlTypes(
        string gridID,
        IUIAutomation automation,
        IUIAutomationElement mainWindowElement,
      ref IUIAutomationElement GridCustomControlType,
  ref IUIAutomationElement TreeControlType,
  ref IUIAutomationElement DataGridControlTypeId,
        string framework)
        {
            try
            {
                mainWindowElement.SetFocus();
                if (framework == "WPF")
                {
                    if (automation != null && mainWindowElement != null)
                    {

                        IUIAutomationCondition controlTypeCondition = automation.CreatePropertyCondition(
        UIA_PropertyIds.UIA_AutomationIdPropertyId, gridID);

                        GridCustomControlType = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, controlTypeCondition);

                        IUIAutomationCondition dataGridCondition = automation.CreatePropertyCondition(
                            UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);

                        IUIAutomationCondition combinedCondition = automation.CreateAndCondition(controlTypeCondition, dataGridCondition);

                    }
                }
                else if (framework == "WinForm")
                {
                    string treeViewAutomationId = "ColScrollRegion: 0, RowScrollRegion: 0";

                    if (automation != null && mainWindowElement != null)
                    {

                        IUIAutomationCondition combinedCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, gridID);
                        GridCustomControlType = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                        IUIAutomationCondition treeViewCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                        TreeControlType = GridCustomControlType.FindFirst(TreeScope.TreeScope_Descendants, treeViewCondition);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static DataTable NAVLockGridData(IUIAutomationElement gridElement, IUIAutomation automation)
        {
            try
            {
                DataTable dataTable = new DataTable();

                // Get headers
                var headerElements = GetHeaderElements(gridElement, automation);
                foreach (var headerElement in headerElements)
                {
                    IUIAutomationCondition condition = automation.CreateTrueCondition(); // Match all sub-elements
                    IUIAutomationElementArray childArray = headerElement.FindAll(TreeScope.TreeScope_Children, condition);

                    for (int i = 0; i < childArray.Length; i++)
                    {
                        IUIAutomationElement childElement = childArray.GetElement(i);
                        string headerName = childElement.CurrentName ?? "Unnamed Header";
                        if (!dataTable.Columns.Contains(headerName))
                        {
                            dataTable.Columns.Add(headerName);
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataTable();
            }
        }

        public static void HandleNavLockTablesDeleteColumnConflict(DataTable table)
        {
            try
            {
                if (table == null)
                {
                    Console.WriteLine("The provided DataTable is null.");
                    return;
                }

                const string targetColumnName = "[Column Header] Delete";
                const string newColumnName = "ColumnHeader_Delete";

                if (table.Columns.Contains(targetColumnName))
                {
                    try
                    {
                        if (!table.Columns.Contains(newColumnName))
                        {
                            table.Columns[targetColumnName].ColumnName = newColumnName;
                            Console.WriteLine("Renamed column '{0}' to '{1}'", targetColumnName, newColumnName);
                        }
                        else
                        {
                            Console.WriteLine("Conflict: target name '{0}' already exists.", newColumnName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error renaming column '{0}': {1}", targetColumnName, ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Column '{0}' not found. No renaming applied.", targetColumnName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HandleNavLockTablesDeleteColumnConflict: " + ex.Message);
            }
        }

        /// <summary>
        /// ///
       
        /// </summary>
        /// <param name="EditGridData"></param>
        /// <returns>bool</returns>
        public bool EditGridData(Dictionary<string, List<FocusGridData>> GridFocusDataDictionary, string moduleID, string gridID, string DataExtractionScriptName, string framework,DataTable ExcelData )
        {

            bool successEditGridDataState = false;
            IUIAutomation automation = new CUIAutomation();
            IUIAutomationElement rootElement = automation.GetRootElement();
            UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
           
            try
            {
                if (GridFocusDataDictionary.ContainsKey(gridID))
                {
                    List<FocusGridData> focusGridList = GridFocusDataDictionary[gridID];

                    foreach (FocusGridData gridData in focusGridList)
                    {
                        if (!string.Equals(gridData.WindowAutomationId, moduleID, StringComparison.OrdinalIgnoreCase))
                            continue;

                        string scriptName = gridData.DataExtractionScriptName;

                        IUIAutomationCondition windowCondition = automation.CreatePropertyCondition(
                            UIA_PropertyIds.UIA_AutomationIdPropertyId, moduleID);
                        IUIAutomationElement mainWindowElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, windowCondition);

                        if (mainWindowElement == null)
                        {
                            Console.WriteLine("Main window element not found.");
                            return successEditGridDataState;
                        }

                        IntPtr hWnd = (IntPtr)mainWindowElement.CurrentNativeWindowHandle;
                        SetForegroundWindow(hWnd);

                        IUIAutomationElement GridCustomControlType = null;
                        IUIAutomationElement TreeControlType = null;
                        IUIAutomationElement DataGridControlTypeId = null;

                        mainWindowElement.SetFocus();
                        try
                        {
                            if (framework == "WPF")
                            {
                                if (automation != null && mainWindowElement != null)
                                {

                                    IUIAutomationCondition controlTypeCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId, gridID);

                                    GridCustomControlType = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, controlTypeCondition);

                                    IUIAutomationCondition dataGridCondition = automation.CreatePropertyCondition(
                                        UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);

                                    IUIAutomationCondition combinedCondition = automation.CreateAndCondition(controlTypeCondition, dataGridCondition);

                                }
                            }
                            else if (framework == "WinForm")
                            {
                                string treeViewAutomationId = "ColScrollRegion: 0, RowScrollRegion: 0";

                                if (automation != null && mainWindowElement != null)
                                {

                                    IUIAutomationCondition combinedCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, gridID);
                                    GridCustomControlType = mainWindowElement.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                                    IUIAutomationCondition treeViewCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                                    TreeControlType = GridCustomControlType.FindFirst(TreeScope.TreeScope_Descendants, treeViewCondition);

                                }
                            }
                        }
                        catch { }
                        
                        if (string.Equals(scriptName, "AttributeRemaining", StringComparison.OrdinalIgnoreCase))
                        {
                            DataTable dt = Retry3Times(() =>
                            {
                                DataTable temp = AttributeRemaining(moduleID, gridID);
                                if (temp != null && temp.Rows.Count > 0)
                                {
                                    return temp;
                                }

                                return null; // Retry if empty or null
                            });

                            EditAttributeRemaining(moduleID, gridID, dt,ExcelData);

                            successEditGridDataState = true;
                            return successEditGridDataState;


                        }
                        else if (string.Equals(scriptName, "ColumnChooserDialog", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                return EditColumnChooserDialog(moduleID, gridID, ExcelData);
                                
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Exception occurred : " + ex.Message);
                                
                            }



                        }
                        else if (string.Equals(scriptName, "EditTTCA", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                return EditTTCA(moduleID, gridID, ExcelData);
                        
                   }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Exception occurred : " + ex.Message);

                }
                


            }
                        else if (string.Equals(scriptName, "EditCT", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                return GridDataProvider.EditCT(moduleID, gridID, ExcelData, treeViewAutomationId);

                            }
            catch (Exception ex)
            {
                                Console.WriteLine("Exception occurred : " + ex.Message);

                            }



                        }
                        
                   }
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred while printing element details: " + ex.Message);
            }

            return successEditGridDataState;
        }

        public static bool EditColumnChooserDialog(string moduleID, string gridID, DataTable excelData)
        {
            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(
                    ApplicationArguments.IUIAutomationMappingTables[moduleID]);

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement windowElement = FindElementByUniquePropertyType(
                    automation,
                    rootElement,
                    ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].UniquePropertyType,
                    ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                if (windowElement == null)
                    throw new Exception("Module window not found.");

                DetectAndSwitchWindow(ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                IUIAutomationCondition gridCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    ApplicationArguments.mapdictionary[gridID + "GridName"].AutomationUniqueValue);

                IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, gridCondition);
                IUIAutomationElementArray gridChildren = gridElementMain.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                for (int i = 0; i < gridChildren.Length; i++)
                {
                    IUIAutomationElement rowElement = gridChildren.GetElement(i);

                            IUIAutomationElementArray subElements = rowElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                            string editValue = string.Empty;
                            string checkboxValue = string.Empty;
                            IUIAutomationElement checkboxElement = null;

                            for (int k = 0; k < subElements.Length; k++)
                            {
                                IUIAutomationElement sub = subElements.GetElement(k);

                                object valueObj = sub.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ValueValuePropertyId);
                                string valStr = valueObj != null ? valueObj.ToString().Trim().ToLower() : "";

                                if (valStr == "true" || valStr == "false")
                                {
                                    checkboxValue = valStr;
                                    checkboxElement = sub;
                                }
                                else if (!string.IsNullOrEmpty(valStr))
                                {
                                    editValue = valStr;
                                }
                            }

                            if (string.IsNullOrEmpty(editValue) || checkboxElement == null) 
                                continue;

                            DataRow[] matchedRows = excelData.Select("Value = '" + editValue.Replace("'", "''") + "'");
                            if (matchedRows.Length == 0) 
                                continue;

                            string excelVisible = matchedRows[0]["Visible"].ToString().Trim().ToLower();

                            if (checkboxValue != excelVisible)
                            {
                                try
                                {
                                    bool toggled = false;
                                    if (!toggled)
                                    {
                                        try
                                        {
                                            object scrollItemPatternObj = checkboxElement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);
                                            IUIAutomationScrollItemPattern scrollItemPattern = scrollItemPatternObj as IUIAutomationScrollItemPattern;

                                            if (scrollItemPattern != null)
                                            {
                                                scrollItemPattern.ScrollIntoView();
                                                Thread.Sleep(3000); // Allow scrolling to finish
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("ScrollIntoView failed: " + ex.Message);
                                        }

                                        try
                                        {
                                            int retryCount = 0;
                                            while (retryCount < 3)
                                            {
                                                try
                                                {
                                                    checkboxElement.SetFocus();
                                                    //MouseOperations.ClickElement(checkboxElement,"Left",true);
                                                    var elementRect = checkboxElement.CurrentBoundingRectangle;
                                                    int xelementRect = elementRect.left + (elementRect.right - elementRect.left) / 2;
                                                    int yelementRect = elementRect.top + (elementRect.bottom - elementRect.top) / 2;
                                                    Cursor.Position = new System.Drawing.Point(xelementRect, yelementRect);
                                                    Thread.Sleep(3000);
                                                    mouse_event(MOUSEEVENTF_LEFTDOWN, xelementRect, yelementRect, 0, 0);
                                                    mouse_event(MOUSEEVENTF_LEFTUP, xelementRect, yelementRect, 0, 0);
                                                    Thread.Sleep(3000);
                                                    
                                                    checkboxValue = ((IUIAutomationValuePattern)checkboxElement
                   .GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId))
                   .CurrentValue.ToString().Trim().ToLower();
                                                    
                                                    if (string.Equals(checkboxValue, excelVisible, StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        toggled = true;
                                                        break; // 
                                                    }
                                                    else
                                                    {
                                                        // Optional: fallback to other click method
                                        MouseOperations.ClickElement(checkboxElement,"Left",true);
                                                        Thread.Sleep(1000);
                                        }
                                                    checkboxValue = ((IUIAutomationValuePattern)checkboxElement
                  .GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId))
                  .CurrentValue.ToString().Trim().ToLower();
                                                    if (string.Equals(checkboxValue, excelVisible, StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        toggled = true;
                                                        break;  
                                                    }

                                                }
                                        catch { }
                                                retryCount++;
                                                if (toggled)
                                                {
                                                    Console.WriteLine(" Checkbox toggled successfully for: " + editValue);
                                    }
                                                else
                                                {
                                                    Console.WriteLine(" Failed to toggle checkbox for: " + editValue);
                                                }
                                            }
                                        }
                                        catch { }
                                    }
                                  
                                    if (toggled)
                                    {
                                        Console.WriteLine(" Checkbox toggled for " + editValue);
                                    }
                                    else
                                    {
                                        Console.WriteLine(" Failed to toggle checkbox for " + editValue);
                                    }
                                }
                                catch (Exception exToggle)
                                {
                                    Console.WriteLine("Toggle failed for " + editValue + ": " + exToggle.Message);
                                }
                            }
                   
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in EditColumnChooserDialog: " + ex.Message);
                return false;
            }
            return true;
        }

        public static bool EditTTCA(string moduleID, string gridID, DataTable excelData)
        {
            UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(
                    ApplicationArguments.IUIAutomationMappingTables[moduleID]);

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                IUIAutomationElement windowElement = FindElementByUniquePropertyType(
                    automation,
                    rootElement,
                    ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].UniquePropertyType,
                    ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                if (windowElement == null)
                    throw new Exception("Module window not found.");

               // DetectAndSwitchWindow(ApplicationArguments.mapdictionary[gridID + "ModuleWindow"].AutomationUniqueValue);

                IUIAutomationCondition gridCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    ApplicationArguments.mapdictionary[gridID + "GridName"].AutomationUniqueValue);

                IUIAutomationElement gridElementMain = windowElement.FindFirst(TreeScope.TreeScope_Descendants, gridCondition);


                IUIAutomationCondition gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                IUIAutomationElement gridControlTypeConditionElement = gridElementMain.FindFirst(TreeScope.TreeScope_Descendants, gridControlTypeCondition);

                gridControlTypeCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);

                IUIAutomationElementArray gridChildren = gridControlTypeConditionElement.FindAll(TreeScope.TreeScope_Children, gridControlTypeCondition);
               
                Dictionary<string, bool> accountProcessedMap = new Dictionary<string, bool>();
                Dictionary<string, string> accountProcessedValue = new Dictionary<string, string>();
                
                foreach (DataRow row in excelData.Rows)
                {
                    string account = row["Account"].ToString();
                    if (!accountProcessedMap.ContainsKey(account))
                    {
                        accountProcessedMap[account] = false;
                        string allocatedQuantity = row["Allocation %"].ToString();
                        accountProcessedValue[account] = allocatedQuantity;
    }


}
              
                int excelDataRowCount = excelData.Rows.Count;

                for (int i = 0; i <= excelDataRowCount; i++)
                {
                    IUIAutomationElement rowElement = gridChildren.GetElement(i);

                    if (!string.IsNullOrEmpty(rowElement.CurrentName))
                    {
                        string accountName = rowElement.CurrentName.Trim();

                        DataRow[] matchedRows = excelData.Select("Account = '" + accountName.Replace("'", "''") + "'");
                        if (matchedRows.Length > 0)
                        {
                            accountProcessedMap[accountName] = true;
                        }

                    }
                    else
                    {
                        IUIAutomationElementArray subrowElementArr = rowElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                        for (int subrowElementArrindex = 0; subrowElementArrindex < subrowElementArr.Length; subrowElementArrindex++)
                        {
                            IUIAutomationElement subrowElement = subrowElementArr.GetElement(subrowElementArrindex);

                            if (!string.IsNullOrEmpty(subrowElement.CurrentName))
                            {
                                string accountName = subrowElement.CurrentName.Trim();

                                if (string.Equals(accountName, "Account", StringComparison.OrdinalIgnoreCase))
                                {
                                    var excelAccountValue = FindFirstUnprocessedAccount(accountProcessedMap);
                                    if (excelAccountValue == null)
                                    {
                                        break;
                                    }

                                    if (excelAccountValue != null)
                                    {
                                        try
                                        {
                                            subrowElement.SetFocus();
                                            MouseOperations.ClickElement(subrowElement, "Left", true);
                                            UIAutomationHelper.SetValue(subrowElement, excelAccountValue.ToString());
                                            SendKeys.SendWait("{TAB}");
                                            accountProcessedMap[excelAccountValue] = true;
                                            break;
                                        }
                                        catch { }
                                    }
                                }
                                

                            }


                        }

                    }

                    gridChildren = gridControlTypeConditionElement.FindAll(TreeScope.TreeScope_Children, gridControlTypeCondition);
                }
                gridChildren = gridControlTypeConditionElement.FindAll(TreeScope.TreeScope_Children, gridControlTypeCondition);

                try
                {
                    for (int i = 0; i <= gridChildren.Length; i++)
                    {

                        IUIAutomationElement subrowElement = gridChildren.GetElement(i);

                        if (!string.IsNullOrEmpty(subrowElement.CurrentName))
                        {
                            string accountName = subrowElement.CurrentName.Trim();
                            DataRow[] matchedRows = excelData.Select("Account = '" + accountName.Replace("'", "''") + "'");
                            if (matchedRows.Length > 0)
                            {
                                IUIAutomationElementArray subrowElementArr = subrowElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                                for (int subrowElementArrindex = 0; subrowElementArrindex < subrowElementArr.Length; subrowElementArrindex++)
                                {
                                    IUIAutomationElement subchild = subrowElementArr.GetElement(subrowElementArrindex);

                                    if (string.Equals(subchild.CurrentName, "Allocation %", StringComparison.OrdinalIgnoreCase))
                                    {
                                        var excelAccountValue = accountProcessedValue[accountName];
                                        if (excelAccountValue != null)
                                        {
                                            subchild.SetFocus();
                                            MouseOperations.ClickElement(subchild, "Left", true);
                                            uiAutomationHelper.SetTextBoxText(subchild, excelAccountValue.ToString());
                                            SendKeys.SendWait("{TAB}");
                                        }
                                    }
                                }
                            }
                        }

                    }

                }
                catch { }
              
                   

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in EditTTCA: " + ex.Message);
                return false;
            }
            return true;
        }

        public static string FindFirstUnprocessedAccount(Dictionary<string, bool> accountProcessedMap)
        {
            foreach (var kvp in accountProcessedMap)
            {
                if (!kvp.Value) 
                {
                    return kvp.Key;  
                }
            }
            return null;  
        }

        public string CheckOptionalNirvanaDialogueBoxHandlerPopup(string PromptYesOrNo, string allowHardCodedDefaultPrompt)
        {
            string detailedInfo2 = "";
            if (string.Equals(allowHardCodedDefaultPrompt, "true", StringComparison.OrdinalIgnoreCase))
            {
                SendKeys.SendWait("{ENTER}");
                return detailedInfo2;
            }

           
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();

                if (rootElement == null)
                    return detailedInfo2;

                IUIAutomationCondition controlTypeCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_LocalizedControlTypePropertyId,
                    ApplicationArguments.mapdictionary["dialogue"].AutomationUniqueValue);

                IUIAutomationCondition classNameCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_ClassNamePropertyId,
                    "#" + ApplicationArguments.mapdictionary["ClassNameDialogueBox"].AutomationUniqueValue);

                IUIAutomationCondition combinedCondition = automation.CreateAndCondition(controlTypeCondition, classNameCondition);

                int waitTime = 3000;
                if (ApplicationArguments.mapdictionary.ContainsKey("Wait"))
                {
                    int configuredWaitTime;
                    if (int.TryParse(ApplicationArguments.mapdictionary["Wait"].AutomationUniqueValue, out configuredWaitTime))
                    {
                        Console.WriteLine("Found Wait " + ApplicationArguments.mapdictionary["Wait"].AutomationUniqueValue);
                        waitTime = configuredWaitTime * 1000;
                    }
                }

                IUIAutomationElement targetElement = null;
                int retryCount = 3;

                for (int i = 0; i < retryCount; i++)
                {
                    Console.WriteLine("Finding NirvanaDialogue Box Try " + i);
                    targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);
                    if (targetElement != null)
                        break;

                    Thread.Sleep(waitTime);
                }

                if (targetElement == null)
                {
                    Console.WriteLine("NirvanaDialogue Box Not Found, Retrying with individual conditions");

                    targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, controlTypeCondition);
                    if (targetElement == null)
                        targetElement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, classNameCondition);

                    if (targetElement == null)
                    {
                        Console.WriteLine("NirvanaDialogue Box still not found, Verification Failed");
                        return detailedInfo2;
                    }
                }

                Console.WriteLine("NirvanaDialogue Box Found");

                try
                {
                    targetElement.SetFocus();
                    Thread.Sleep(2000);

                    IUIAutomationCondition nameCondition = automation.CreatePropertyCondition(
                                                            UIA_PropertyIds.UIA_NamePropertyId,
                                                            PromptYesOrNo
                                                            );

                    IUIAutomationElement nameConditionElement= targetElement.FindFirst(TreeScope.TreeScope_Descendants, nameCondition);
                    if (nameConditionElement != null)
                    {
                        nameConditionElement.SetFocus();
                        MouseOperations.ClickElement(nameConditionElement,"Left",true);
                        SendKeys.SendWait("{ENTER}");
                    }

                    return "";
                }
                catch
                {
                    return detailedInfo2;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in NirvanaDialogueBoxHandler " + ex.Message);
                return detailedInfo2;
            }
        }

        public bool UpdateMPFCNav(DataTable dt, string key, string sheet)
        {
            bool success = true;

            try
            {
                

                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                if (ApplicationArguments.IUIAutomationDataTables == null)
                {
                    ApplicationArguments.IUIAutomationDataTables = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["IUIAutomationDataFile"]);
                }
                if (ApplicationArguments.IUIAutomationMappingTables == null)
                {
                    ApplicationArguments.IUIAutomationMappingTables = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["IUIAutomationMappingFile"]);
                }
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[sheet]);
                if (!string.IsNullOrEmpty(key))
                { PerformActionWithRetry(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue, key); }

                IUIAutomationElement windowElement = null;
                int retryCount = 3;
                int delayMilliseconds = 2000;

                for (int attempt = 1; attempt <= retryCount; attempt++)
                {
                    try
                    {
                        windowElement = FindElementByUniquePropertyType(
                            automation,
                            rootElement,
                            ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType,
                            ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue
                        );

                        if (windowElement != null)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        if (attempt == retryCount)
                        {
                            return false;
                        }
                        Thread.Sleep(delayMilliseconds);
                    }
                }

                if (windowElement != null)
                {
                    IUIAutomationCondition grdCondition = automation.CreatePropertyCondition(
                                   UIA_PropertyIds.UIA_AutomationIdPropertyId, ApplicationArguments.mapdictionary["GridID"].AutomationUniqueValue);
                    IUIAutomationElement gridElement = windowElement.FindFirst(TreeScope.TreeScope_Descendants, grdCondition);

                    IUIAutomationCondition treeViewCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, treeViewAutomationId);
                    IUIAutomationElement treeViewElement = gridElement.FindFirst(TreeScope.TreeScope_Descendants, treeViewCondition);
                    if (treeViewElement == null)
                    {
                        Console.WriteLine("TreeView element not found.");
                        return false;
                    }

                    //check add and remove buttons
                    IUIAutomationElement btnRemove = FindElementByUniquePropertyType(
                       automation,
                       rootElement,
                       ApplicationArguments.mapdictionary["btnRemove"].UniquePropertyType,
                       ApplicationArguments.mapdictionary["btnRemove"].AutomationUniqueValue);
                    IUIAutomationElement btnAdd = FindElementByUniquePropertyType(
                   automation,
                   rootElement,
                   ApplicationArguments.mapdictionary["btnAdd"].UniquePropertyType,
                   ApplicationArguments.mapdictionary["btnAdd"].AutomationUniqueValue);

                    if (btnRemove != null && btnAdd != null)
                    {

                        IUIAutomationElement btnSave = FindElementByUniquePropertyType(
                                         automation,
                                         rootElement,
                                         ApplicationArguments.mapdictionary["btnSave"].UniquePropertyType,
                                         ApplicationArguments.mapdictionary["btnSave"].AutomationUniqueValue);

                        if (btnSave != null)
                        {
                            var dataItemCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
                            IUIAutomationElementArray dataItems = treeViewElement.FindAll(TreeScope.TreeScope_Descendants, dataItemCondition);

                            if (dataItems != null)
                            {
                                // need to remove old data
                                for (int i = dataItems.Length - 1; i >= 0; i--)
                                {
                                    IUIAutomationElement dataItem = dataItems.GetElement(i);
                                    try
                                    {
                                        IUIAutomationElementArray dataItemChildren = dataItem.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                                        for (int j = dataItemChildren.Length - 1; j >= 0; j--)
                                        {
                                            IUIAutomationElement cellElement = dataItemChildren.GetElement(j);
                                            if (cellElement.CurrentName.Contains("Master Fund"))
                                            {
                                                cellElement.SetFocus();
                                                MouseOperations.ClickElement(cellElement);
                                                Thread.Sleep(1000);
                                                SendKeys.SendWait("{TAB}");
                                                Thread.Sleep(3000);
                                                btnRemove.SetFocus();
                                                MouseOperations.ClickElement(btnRemove,"Left",true);
                                               
                                                Thread.Sleep(3000);
                                                Console.WriteLine("Switched to dialogue box window");// quality code will be updated later
                                                Console.WriteLine("Popup still present, pressing Enter key");
                                                SendKeys.SendWait("{ENTER}");
                                                break;
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }


                            }

                            //now add new data
                            int count = dt.Rows.Count;

                            for (int counter = 0; counter < count; counter++)
                            {
                                MouseOperations.ClickElement(btnAdd);
                                Thread.Sleep(1000);
                            }

                            for (int row = 0; row < count; row++)
                            {
                                DataRow dr = dt.Rows[row];

                                dataItems = treeViewElement.FindAll(TreeScope.TreeScope_Descendants, dataItemCondition);

                                if (dataItems != null)
                                {
                                    IUIAutomationElement dataItem = dataItems.GetElement(row);
                                     try
                                     {
                                          IUIAutomationElementArray dataItemChildren = dataItem.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                                          for (int j = 0; j < dataItemChildren.Length; j++)
                                          {
                                              IUIAutomationElement childElement = dataItemChildren.GetElement(j);
                                                string elementName = childElement.CurrentName;
                                                string elementAutomationId = childElement.CurrentAutomationId;
                                                Console.WriteLine("Name: " + elementName + " AutomationId: " + elementAutomationId);

                                                DateTime cellDate;
                                                string formula = elementName;
                                                if (DateTime.TryParse(elementName, out cellDate))
                                                {
                                                     formula = GetExcelDateFormulaAsString(cellDate);
                                                   
                                                }

                                                if (dr.Table.Columns.Contains(formula))
                                                {
                                                    UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                                                    Console.WriteLine("Name: " + elementName + " Value to Fill: " + dr[formula].ToString());
                                                    string valueToFill = dr[formula].ToString();
                                                    double numericValue;
                                                    try
                                                    {
                                                        object scrollItemPatternObj = childElement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);
                                                        IUIAutomationScrollItemPattern scrollItemPattern = scrollItemPatternObj as IUIAutomationScrollItemPattern;

                                                        if (scrollItemPattern != null)
                                                        {
                                                            scrollItemPattern.ScrollIntoView();
                                                            Thread.Sleep(2000); // Allow scrolling to finish
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine("ScrollIntoView failed: " + ex.Message);
                                                    }


                                                    childElement.SetFocus();
                                                  
                                                    MouseOperations.ClickElement(childElement);
                                                    Thread.Sleep(1000);
                                                    if (double.TryParse(valueToFill, out numericValue))
                                                    {
                                                        uiAutomationHelper.SetTextBoxText(childElement, valueToFill);
                                                    }
                                                    else
                                                    {
                                                        IUIAutomationElementArray miniitemItemChildren = childElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                                                        for (int minichild = 0; minichild < miniitemItemChildren.Length; minichild++)
                                                        {
                                                            try
                                                            {
                                                                MouseOperations.ClickElement(miniitemItemChildren.GetElement(minichild));
                                                            }
                                                            catch { }
                                                        }

                                                        SetValue(childElement, valueToFill);
                                                    }


                                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                                }

                                          }


                                     }
                                     catch (Exception ex)
                                     {
                                         Console.WriteLine(ex.Message);
                                     }
                                }

                            }


                            MouseOperations.ClickElement(btnSave);
                        }
                    }
                   
                    
                }
                else
                    return false;

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public static string GetExcelDateFormulaAsString(DateTime cellDate)
        {
            try
            {
                int offset = (cellDate.Date - DateTime.Today).Days;

                if (offset == 0)
                {
                    return "TODAY()";
                }
                else if (offset > 0)
                {
                    return string.Format("TODAY()+{0}", offset);
                }
                else 
                {
                    return string.Format("TODAY()-{0}", Math.Abs(offset));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

    }


}
