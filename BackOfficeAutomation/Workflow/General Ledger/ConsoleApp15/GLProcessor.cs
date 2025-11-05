using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.FileIO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using UIAutomationClient;
using WindowsInput;
using static NPOI.HSSF.Util.HSSFColor;

namespace GLProcessor
{
    //public class XMLDateExtractor
    //{
    //    private string xmlContent;
    //    public XMLDateExtractor(string xmlContent) => this.xmlContent = xmlContent;
    //    public DateTime ExtractFilterValue()
    //    {
    //        try
    //        {
    //            XmlDocument xmlDocument = new XmlDocument();
    //            xmlDocument.LoadXml(this.xmlContent);
    //            XmlNode xmlNode = xmlDocument.SelectSingleNode("//FilterValue");
    //            if (xmlNode != null)
    //            {
    //                DateTime result;
    //                if (DateTime.TryParse(xmlNode.InnerText, out result))
    //                    return result;
    //                Console.WriteLine("Failed to parse FilterValue as DateTime.");
    //            }
    //            else
    //                Console.WriteLine("FilterValue node not found in XML.");
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("Error extracting FilterValue: " + ex.Message);
    //        }
    //        return DateTime.MinValue;
    //    }
    //}
    internal class GLProcessor
    {
        public static DateTime initialTime = DateTime.Now;
        public static DateTime finalTime;
        public static string logFolderPath = Directory.GetCurrentDirectory() + "\\Logs";
        public static string logFilePath = logFolderPath + "\\GL_Logs_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        static IUIAutomationElement _currwinglobal = null;
        public static string logMsg = string.Empty;
        public static int exitcode = -1;
        public static string exePath = String.Empty;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BlockInput(bool fBlockIt);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        private static GlobalKeyboardHook _globalKeyboardHook;

        public static void MinimizeConsoleWindow()
        {
            const int SW_MINIMIZE = 6;
            IntPtr hWndConsole = GetConsoleWindow();
            if (hWndConsole != IntPtr.Zero)
            {
                ShowWindow(hWndConsole, SW_MINIMIZE);
            }
        }

        public static void MaximizeWindow(string currwin, ref IUIAutomationElement currentwin)
        {
            try
            {
                if (currwin == "")
                {
                    return;
                }
                IUIAutomation automation = new CUIAutomation8();
                var root = automation.GetRootElement();
                IUIAutomationElement element = null;
                bool f = false;
                if (currentwin != null && currentwin.CurrentName == currwin)
                {
                    f = true;
                    element = currentwin;
                }

                if (f == false)
                {
                    var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");
                    var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, currwin);
                    var wincond = automation.CreateAndCondition(cond1, cond2);
                    element = root.FindFirst(TreeScope.TreeScope_Descendants, wincond);
                }

                if (element != null)
                {
                    currentwin = element;
                    Console.WriteLine(currentwin.CurrentName);
                    object patternprovider;
                    if (element.GetCurrentPattern(UIA_PatternIds.UIA_WindowPatternId) != null)
                    {
                        patternprovider = element.GetCurrentPattern(UIA_PatternIds.UIA_WindowPatternId);
                        IUIAutomationWindowPattern windowpatternprovider = patternprovider as IUIAutomationWindowPattern;
                        WindowVisualState calstate = windowpatternprovider.CurrentWindowVisualState;
                        if (calstate != null)
                        {
                            if (calstate == WindowVisualState.WindowVisualState_Minimized)
                            {
                                if (windowpatternprovider.CurrentCanMaximize == 1)
                                {

                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Maximized);
                                }

                            }
                            else if (calstate == WindowVisualState.WindowVisualState_Normal)
                            {
                                if (windowpatternprovider.CurrentCanMinimize == 1)
                                {

                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Minimized);
                                }
                                Thread.Sleep(100);
                                if (windowpatternprovider.CurrentCanMaximize == 1)
                                {

                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Maximized);
                                }
                            }
                            else if (calstate == WindowVisualState.WindowVisualState_Maximized)
                            {
                                if (windowpatternprovider.CurrentCanMinimize == 1)
                                {

                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Minimized);
                                }
                                Thread.Sleep(100);
                                if (windowpatternprovider.CurrentCanMaximize == 1)
                                {
                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Maximized);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exitcode = 9;
                logMsg = "Getting exception " + ex.Message + " while maximizing " + currentwin.CurrentName + " window.\n Exited with code : "+exitcode+"\n"+ ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
        }

        public static void MinimizeWindow(string currwin, ref IUIAutomationElement currentwin)
        {
            try
            {
                IUIAutomation automation = new CUIAutomation8();
                var root = automation.GetRootElement();
                IUIAutomationElement element = null;
                bool f = false;
                if (currentwin != null && currentwin.CurrentName == currwin)
                {
                    f = true;
                    element = currentwin;
                }
                if (f == false)
                {
                    var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");
                    var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, currwin);
                    var wincond = automation.CreateAndCondition(cond1, cond2);
                    element = root.FindFirst(TreeScope.TreeScope_Descendants, wincond);
                }
                if (element != null)
                {
                    currentwin = element;
                    Console.WriteLine(currentwin.CurrentName);
                    object patternprovider;
                    // Calculator is already open, bring it to the foreground
                    if (element.GetCurrentPattern(UIA_PatternIds.UIA_WindowPatternId) != null)
                    {
                        patternprovider = element.GetCurrentPattern(UIA_PatternIds.UIA_WindowPatternId);
                        IUIAutomationWindowPattern windowpatternprovider = patternprovider as IUIAutomationWindowPattern;
                        WindowVisualState calstate = windowpatternprovider.CurrentWindowVisualState;
                        if (calstate != null)
                        {
                            if (calstate == WindowVisualState.WindowVisualState_Minimized)
                            {
                            }
                            else if (calstate == WindowVisualState.WindowVisualState_Normal)
                            {
                                if (windowpatternprovider.CurrentCanMinimize == 1)
                                {
                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Minimized);
                                }
                            }
                            else if (calstate == WindowVisualState.WindowVisualState_Maximized)
                            {
                                if (windowpatternprovider.CurrentCanMinimize == 1)
                                {
                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Minimized);
                                }
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while minimizing " + currentwin.CurrentName + " window.\n"+ ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
        }

        public static System.Data.DataTable GetDataFromCsvFile(string csvFile)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            try
            {
                if (File.Exists(csvFile))
                {
                    using (TextFieldParser parser = new TextFieldParser(csvFile))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");

                        if (!parser.EndOfData)
                        {
                            string[] fields = parser.ReadFields();
                            foreach (string field in fields)
                            {
                                dataTable.Columns.Add(field);
                            }
                            while (!parser.EndOfData)
                            {
                                string[] data = parser.ReadFields();
                                DataRow row = dataTable.NewRow();
                                for (int i = 0; i < data.Length; i++)
                                {
                                    row[i] = data[i];
                                }
                                dataTable.Rows.Add(row);
                            }
                        }
                    }
                }
                else
                {
                    exitcode = 15;
                    logMsg = "GL config files does not exists.";
                    CreateLogs(logMsg, logFilePath);
                    Console.Write("Exited with code : "+exitcode);
                    Environment.Exit(exitcode);
                }
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while reading data from file " + csvFile +"\n"+ ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
            return dataTable;
        }

        public static string GetValueFromMainFile(System.Data.DataTable dt, string Name)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    string data = row["Name"].ToString();
                    if (data == Name)
                    {
                        return row["Value"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while reading data from Main Data file.\n" +ex.StackTrace;
                CreateLogs(logMsg,logFilePath);
            }
            return null;
        }

        public static void replayaction(IUIAutomationElement targetelement, string message)
        {
            try
            {
                if (targetelement == null) return;
                if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                {
                    object scrollitemobj;
                    scrollitemobj = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                    IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                    selectionpatternprovider.ScrollIntoView();
                }
                if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
                {
                    Console.WriteLine("Entered for button and elements is:" + targetelement);
                    string value = null;
                    object patternprovider;
                    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                    {
                        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                        Console.WriteLine(targetelement.CurrentName);
                        IUIAutomationInvokePattern selectionpatternprovider = patternprovider as IUIAutomationInvokePattern;
                        selectionpatternprovider.Invoke();
                        Console.WriteLine("Clicked on " + targetelement);
                    }
                    return;
                }
                if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_CheckBoxControlTypeId)
                {
                    string value = null;
                    object patternprovider;
                    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                    {
                        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                        IUIAutomationInvokePattern selectionpatternprovider = patternprovider as IUIAutomationInvokePattern;
                        selectionpatternprovider.Invoke();
                    }
                    return;
                }
                if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_ComboBoxControlTypeId)
                {
                    string value = message;
                    object patternprovider;
                    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                    {
                        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                        IUIAutomationValuePattern selectionpatternprovider = patternprovider as IUIAutomationValuePattern;
                        selectionpatternprovider.SetValue(value);
                    }
                    return;
                }
                if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_EditControlTypeId)
                {
                    string value = message;
                    object patternprovider;
                    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                    {
                        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                        IUIAutomationValuePattern selectionpatternprovider = patternprovider as IUIAutomationValuePattern;
                        selectionpatternprovider.SetValue(value);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                exitcode = 23;
                logMsg = "Getting exception " + ex.Message + " while clicking element " + targetelement.CurrentName + ".\n\"Exited with code : \"" + exitcode +"\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
                Console.WriteLine("Exited with code : " + exitcode);
                Environment.Exit(exitcode);
            }
        }

        const int SW_RESTORE = 9;
        public static void RestoreConsoleWindow()
        {
            IntPtr hWndConsole = GetConsoleWindow();
            if (hWndConsole != IntPtr.Zero)
            {
                ShowWindow(hWndConsole, SW_RESTORE);
            }
        }

        public static void WaitElementToBeVisible(IUIAutomationElement element)
        {
            Thread.Sleep(500);
            int maxRetries = 50;
            int retryDelayMilliseconds = 1000;
            for (int retryCount = 0; retryCount < maxRetries; retryCount++)
            {
                try
                {
                    if (element == null)
                    {
                        break;
                    }
                    Thread.Sleep(retryDelayMilliseconds);
                }
                catch (Exception ex)
                {
                    logMsg = "Gettng exception " + ex.Message + " while waiting for element " + element.CurrentName + " to be visible \n" + ex.StackTrace;
                    //Console.WriteLine(logMsg);
                    CreateLogs(logMsg, logFilePath);
                    Thread.Sleep(retryDelayMilliseconds);
                }
            }
        }

        private static void ClickElement(IUIAutomationElement element, string clicktype, ref IUIAutomationElement currentwin)
        {
            try
            {
                if (element == null)
                {
                    return;
                }
                //string type = element.CachedItemType;
                int left = int.MaxValue;
                int top = int.MaxValue;
                int right = int.MaxValue;
                int bottom = int.MaxValue;

                object scrollitemobj;

                if (element.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                {
                    scrollitemobj = element.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                    IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                    selectionpatternprovider.ScrollIntoView();

                }
                InputSimulator simulator = new InputSimulator();

                left = element.CurrentBoundingRectangle.left;
                top = element.CurrentBoundingRectangle.top;
                right = element.CurrentBoundingRectangle.right;
                bottom = element.CurrentBoundingRectangle.bottom;
                if (left != int.MaxValue && top != int.MaxValue && right != int.MaxValue && bottom != int.MaxValue)
                {
                    int x = left + (right - left) / 2;
                    int y = top + (bottom - top) / 2;

                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)x, (int)y);
                    bool isclicked = Convert.ToBoolean(element.GetCurrentPropertyValue(UIA_PropertyIds.UIA_SelectionItemIsSelectedPropertyId));

                    //while (!isclicked)
                    {
                        //BringToForeground("General Ledger", ref currentwin, exePath);
                        if (clicktype == "Left Mouse Button Clicked")
                        {
                            var x1 = element.CurrentName;
                            simulator.Mouse.LeftButtonClick();
                            var x2 = element.CurrentAutomationId;
                        }
                        if (clicktype == "Right Mouse Button Clicked")
                        {
                            simulator.Mouse.RightButtonClick();

                        }
                        isclicked = Convert.ToBoolean(element.GetCurrentPropertyValue(UIA_PropertyIds.UIA_SelectionItemIsSelectedPropertyId));

                    }


                }
            }
            catch (Exception ex)
            {
                exitcode = 23;
                logMsg = "Getting exception " + ex.Message + " while clicking element " + element.CurrentName + ".\nExited with code : " + exitcode + "\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
                Console.WriteLine("Exited with code : " + exitcode);
                Environment.Exit(exitcode);
            }
        }

        public static void CreateLogs(string logMessage, string logFilePath, int dateCheck = 1)
        {

            try
            {
                if (File.Exists(logFilePath))
                {
                    using (StreamWriter writer = File.AppendText(logFilePath))
                    {
                        string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
                        string msg;

                        if (dateCheck == 0)
                        {
                            msg = logMessage;
                        }
                        else
                        {
                            msg = formattedDateTime + "    " + logMessage;
                        }
                        writer.WriteLine(msg);
                        writer.WriteLine("");
                    }

                }
                else
                {
                    using (StreamWriter writer = File.CreateText(logFilePath))
                    {
                        string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
                        string msg;

                        if (dateCheck == 0)
                        {
                            msg = logMessage;
                        }
                        else
                        {
                            msg = formattedDateTime + "    " + logMessage;
                        }
                        writer.WriteLine(msg);
                        writer.WriteLine("");
                    }
                }
            }
            catch(Exception ex) 
            {
                logMsg = "Getting exception " + ex.Message + " while saving logs.\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }

        }

        public static void BringToForeground(string currwin, ref IUIAutomationElement currentwin, string exePath)
        {
            // Check if Calculator is already open
            try
            {
                if (currwin == "")
                {
                    return;
                }

                IUIAutomation automation = new CUIAutomation8();
                var root = automation.GetRootElement();
                IUIAutomationElement element = null;
                bool f = false;
                if (currentwin != null && currentwin.CurrentName == currwin)
                {
                    f = true;
                    element = currentwin;
                }
                if (f == false && currwin.Contains("Nirvana"))
                {
                    var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");
                    var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, currwin);
                    var wincond = automation.CreateAndCondition(cond1, cond2);
                    element = root.FindFirst(TreeScope.TreeScope_Descendants, wincond);
                }

                if (element != null)
                {
                    currentwin = element;
                    Console.WriteLine(currentwin.CurrentName);
                    object patternprovider;
                    // Calculator is already open, bring it to the foreground
                    if (element.GetCurrentPattern(UIA_PatternIds.UIA_WindowPatternId) != null)
                    {
                        patternprovider = element.GetCurrentPattern(UIA_PatternIds.UIA_WindowPatternId);
                        IUIAutomationWindowPattern windowpatternprovider = patternprovider as IUIAutomationWindowPattern;
                        WindowVisualState calstate = windowpatternprovider.CurrentWindowVisualState;
                        IntPtr currentForegroundWindowHandle = GetForegroundWindow();
                        IUIAutomationElement currentForegroundwindow = automation.ElementFromHandle(currentForegroundWindowHandle);
                        if (currentForegroundwindow.CurrentName == currwin) return;
                        if (calstate != null)
                        {
                            if (calstate == WindowVisualState.WindowVisualState_Minimized)
                            {
                                windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Normal);
                            }
                            else if (calstate == WindowVisualState.WindowVisualState_Normal)
                            {
                                if (windowpatternprovider.CurrentCanMinimize == 1)
                                {

                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Minimized);
                                }


                                Thread.Sleep(100);
                                windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Normal);

                            }
                            else if (calstate == WindowVisualState.WindowVisualState_Maximized)
                            {
                                if (windowpatternprovider.CurrentCanMinimize == 1)
                                {

                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Minimized);
                                }
                                Thread.Sleep(100);
                                if (windowpatternprovider.CurrentCanMaximize == 1)
                                {

                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Maximized);
                                }

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while bringing " + currentwin.CurrentName + " to foreground.\n" + ex.StackTrace;
                exitcode = 22;
                CreateLogs(logMsg + ".\nExited with code : " + exitcode,logFilePath);
                Console.WriteLine("Exited with code : " + exitcode);
                Environment.Exit(exitcode);
            }
        }

        public static string ReadXmlPathFromConfig(string configPath)
        {
            try
            {
                // Read the XML file path from the config file
                string xmlFilePath = File.ReadAllText(configPath).Trim();
                return xmlFilePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading XML path from config: {ex.Message}");
                return null;
            }
        }

        //public static DateTime GetProcessDate(string configPath)
        //{
        //    try
        //    {
        //        string xmlFilePath = ReadXmlPathFromConfig(configPath);

        //        if (!string.IsNullOrEmpty(xmlFilePath))
        //        {
        //            // Read the XML content from the specified path
        //            string xmlContent = File.ReadAllText(xmlFilePath);
        //            XMLDateExtractor parser = new XMLDateExtractor(xmlContent);
        //            DateTime filterValue = parser.ExtractFilterValue();

        //            if (filterValue != DateTime.MinValue)
        //            {
        //                return filterValue;
        //            }
        //            else
        //            {
        //                Console.WriteLine("Failed to extract FilterValue.");
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return DateTime.MinValue;
        //}

        public static void OpenGLModule()
        {
            InputSimulator inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
            inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
            inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_G);
            inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);
            inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
        }

        public static bool Login(System.Data.DataTable dt, string exePath)
        {
            int i = 0;
            Process.Start(exePath);
            Thread.Sleep(3000);
            IUIAutomation automation = new CUIAutomation8();
            IUIAutomationElement currentwin = null;
            string currwin = "Nirvana: User Login";
            string loginId = GetValueFromMainFile(dt, "loginId");
            string loginPass = GetValueFromMainFile(dt, "loginPassword");

            IUIAutomationElement username = null;
            IUIAutomationElement password = null;
            IUIAutomationElement btn = null;
            try
            {
                //handle = GetMainWindowHandle(process);
                //    currentwin = automation.ElementFromHandle(handle);
                BringToForeground(currwin, ref currentwin, exePath);
                Thread.Sleep(500);

                while (currentwin == null || (currentwin.CurrentName != currwin))
                {
                    MaximizeWindow(currwin, ref currentwin);
                    if (currentwin != null)
                    {
                        Console.WriteLine(currentwin.CurrentName);
                    }
                    _currwinglobal = currentwin;
                }

                if (currentwin != null && (currentwin.CurrentName == currwin))
                {
                    var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "txtLoginID");
                    var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "txtPassword");
                    var cond3 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnLogin");


                    username = currentwin.FindFirst(TreeScope.TreeScope_Descendants, cond1);
                    password = currentwin.FindFirst(TreeScope.TreeScope_Descendants, cond2);

                    btn = currentwin.FindFirst(TreeScope.TreeScope_Descendants, cond3);
                    Console.WriteLine(password.CurrentAutomationId);

                    replayaction(username, loginId);
                    Thread.Sleep(200);
                    replayaction(password, loginPass);
                    Thread.Sleep(200);

                    Console.WriteLine(currentwin.CurrentName);
                    replayaction(btn, "");
                    Thread.Sleep(200);
                }
                return true;

            }
            catch (Exception ex)
            {

                //bool isApplicationClose = StopApplication(exePath);
                logMsg = Environment.NewLine + ex.Message.ToString()+"\n"+ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
                return false;
            }
        }

        private static string GetProcessOwner(int processId)
        {
            string owner = string.Empty;
            try
            {
                string query = "Select * From Win32_Process Where ProcessID = " + processId;
                using (var searcher = new System.Management.ManagementObjectSearcher(query))
                {
                    foreach (System.Management.ManagementObject process in searcher.Get())
                    {
                        string[] ownerInfo = new string[2];
                        process.InvokeMethod("GetOwner", (object[])ownerInfo);
                        owner = ownerInfo[0]; // Return the user who owns the process
                    }
                }
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while getting process owner \n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
            return owner;

        }

        public static bool IsClientLogin(string exePath)
        {
            bool isLogin = false;
            string[] ownerInfo = new string[2];
            uint processId =0;
            try
            {
                string targetPath = exePath.Substring(0, exePath.LastIndexOf('\\'));
                // Get the current session ID
                int currentSessionId = Process.GetCurrentProcess().SessionId;
                logMsg = "Current session id is: " + currentSessionId + ".";
                CreateLogs(logMsg, logFilePath);
                // Query to get processes with a specific executable path and owner name
                string query = $"SELECT * FROM Win32_Process WHERE ExecutablePath LIKE '{targetPath.Replace("\\", "\\\\")}%' AND SessionId = {currentSessionId}";

                // Create a ManagementObjectSearcher with the query
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                {
                    // Execute the query and get the collection of processes
                    ManagementObjectCollection processCollection = searcher.Get();
                    logMsg = "Elements in process collection are " + processCollection.Count+" and target path is:"+targetPath;
                    CreateLogs(logMsg, logFilePath);
                    // Iterate through each process
                    foreach (ManagementObject process in processCollection)
                    {
                        logMsg = "Entered for loop in IsClientLogin method. and found " + processCollection.Count + " elements.";
                        CreateLogs(logMsg, logFilePath);
                        string processPath = process["ExecutablePath"]?.ToString();
                        if (string.Equals(processPath, exePath, StringComparison.OrdinalIgnoreCase))
                            //if (processPath != null && processPath.ToLower().Equals(exePath.ToLower()))
                        {
                            // Get the owner information
                            process.InvokeMethod("GetOwner", (object[])ownerInfo);
                            // Get the process ID and name
                            processId = (uint)process["ProcessId"];
                            string processName = process["Name"].ToString();
                            logMsg = $"Process: {processName} (PID: {processId}), Owner: {ownerInfo[0]}, domain: {ownerInfo[1]}";
                            CreateLogs(logMsg, logFilePath);
                            isLogin = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + "for owner:" + ownerInfo[0] + "PID: "+processId+", Owner: "+ ownerInfo[0]+", domain: " +ownerInfo[1]+ " while getting all the opened instances of Prana.\n " + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
            return isLogin;
        }

        [Flags]
        private enum ProcessAccessFlags : uint
        {
            QueryLimitedInformation = 0x00001000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool QueryFullProcessImageName(
              [In] IntPtr hProcess,
              [In] int dwFlags,
              [Out] StringBuilder lpExeName,
              ref int lpdwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(
         ProcessAccessFlags processAccess,
         bool bInheritHandle,
         int processId);

        static String GetProcessFilename(Process p)
        {
            int capacity = 2000;
            StringBuilder builder = new StringBuilder(capacity);
            IntPtr ptr = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, p.Id);
            if (!QueryFullProcessImageName(ptr, 0, builder, ref capacity))
            {
                return String.Empty;
            }

            return builder.ToString();
        }

        
        public static void RunGLProcesses(System.Data.DataTable mainFileData, string exePath, Dictionary<string, string> glDetails, System.Data.DataTable glData)
        {
            try
            {
                int i = 0;
                bool isLogin = false;
                //IntPtr handle = IntPtr.Zero;
                isLogin = IsClientLogin(exePath);
                if (!isLogin)
                {
                    CreateLogs("Prana found:" + logMsg, logFilePath);
                    isLogin = Login(mainFileData, exePath);
                }
                if (isLogin == false) return;
                IUIAutomation automation = new CUIAutomation8();
                IUIAutomationElement currentwin = null;
                try
                {
                    BringToForeground("Nirvana", ref currentwin, exePath);
                    _currwinglobal = currentwin;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error while getting foregroundwindow");
                    logMsg = "Getting exception " + ex.Message + " while getting foreground window.\n"+ex.StackTrace;
                    CreateLogs(logMsg, logFilePath);
                }
                Thread.Sleep(500);
                OpenGLModule();
                Thread.Sleep(500);
                MaximizeWindow("General Ledger", ref currentwin); 
                if (currentwin != null)
                {
                    Console.WriteLine(currentwin.CurrentName);
                }
                _currwinglobal = currentwin;
                while (currentwin == null || (currentwin.CurrentAutomationId != "frmCashManagementMain"))
                {
                    BringToForeground("Nirvana", ref currentwin, exePath);
                    Console.WriteLine(currentwin.CurrentName);
                    OpenGLModule();
                    MaximizeWindow("General Ledger", ref currentwin);
                    Thread.Sleep(2000);
                    if (currentwin != null)
                    {
                        Console.WriteLine("Opened " + currentwin.CurrentName + " window.");
                    }
                    _currwinglobal = currentwin;
                }

                if ((currentwin.CurrentAutomationId == "frmCashManagementMain"))
                {
                    IUIAutomationCondition condtitlebar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "_frmCashManagementMain_UltraFormManager_Dock_Area_Top");
                    IUIAutomationElement titlebar = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condtitlebar);
                    InputSimulator inputSimulator = new InputSimulator();
                    tagPOINT p;
                    if (titlebar != null)
                    {
                        titlebar.GetClickablePoint(out p);
                        System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)p.x, (int)p.y);
                        inputSimulator.Mouse.LeftButtonClick();
                    }
                    DateTime startTime = DateTime.Now;
                    MaximizeWindow("General Ledger", ref currentwin);
                    //WaitElementToBeVisible(currentwin);
                    Thread.Sleep(1000);
                    foreach (DataRow row in glData.Rows)
                    {
                        if (exitcode == 0)
                            exitcode = -1;
                        string tabName = row["TabName"].ToString();
                        IUIAutomationCondition condTab = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, tabName);
                        IUIAutomationElement element = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condTab);
                        BringToForeground("General Ledger", ref currentwin, exePath);
                        ClickElement(element, "Left Mouse Button Clicked", ref currentwin);
                        logMsg = "Clicked on tab " + tabName;
                        Console.WriteLine(logMsg);
                        CreateLogs(logMsg, logFilePath);
                        Thread.Sleep(1000);
                        if (tabName.Equals("Daily Calculations"))
                            GetDailyCalculations(automation, currentwin, glDetails[tabName], row);

                        else if (tabName.Equals("Day End Cash"))
                            GetDayEndCash(automation, currentwin, glDetails[tabName], row);

                        else if (tabName.Equals("Chart of CashAccounts"))
                            GetAccountBalances(automation, currentwin, row);

                        else if (tabName.Equals("Journal Exceptions"))
                            RunManualRevaluation(automation, currentwin, glDetails[tabName], row);
                        Thread.Sleep(2000);
                        finalTime = DateTime.Now;
                        TimeSpan timeDifference = finalTime - startTime;
                        string totalTimeTaken = timeDifference.Hours + " Hours " + timeDifference.Minutes + " Minutes " + timeDifference.Seconds + " Seconds " + timeDifference.Milliseconds + " Milliseconds ";
                        logMsg = "Total Time Taken :" + totalTimeTaken+" for "+tabName+" process.";
                        CreateLogs(logMsg,logFilePath,0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Getting exception inside RunGLProcesses method.");
                logMsg = "Getting exception " + ex.Message.ToString() + " inside RunGLProcesses method.\n"+ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
        }

        public static void GetDayEndCash(IUIAutomation automation, IUIAutomationElement currentwin, string fromDate, DataRow dr)
        {
            try
            {
                String toDate = GetToDate(dr["CurrentDate"].ToString());
                SelectAccounts(automation, currentwin, dr["Accounts"].ToString().Split(',').ToList());

                IUIAutomationCondition condList = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "Get Data");
                IUIAutomationElementArray list = currentwin.FindAll(TreeScope.TreeScope_Descendants, condList);
                int count = list.Length;

                if (fromDate != null)
                {
                    IUIAutomationCondition editControl = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "dtFromDate");
                    IUIAutomationElement editItem = currentwin.FindFirst(TreeScope.TreeScope_Descendants, editControl);
                    object valuePatternObj;
                    object textPatternObj;
                    valuePatternObj = editItem.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    textPatternObj = editItem.GetCurrentPattern(UIA_PatternIds.UIA_TextEditPatternId);

                    IUIAutomationTextEditPattern textpattern = textPatternObj as IUIAutomationTextEditPattern;
                    IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;

                    string value = fromDate;
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    try
                    {
                        valuePattern.SetValue(value);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("One of the identified items was in an invalid format"))
                        {
                            RestoreConsoleWindow();
                            Console.WriteLine("You have entered date in wrong format.Please enter the from date in format MM/dd/yyyy i.e. 05/29/2024");
                            value = Console.ReadLine();
                            valuePattern.SetValue(value);
                        }
                    }
                    Thread.Sleep(500);
                }

                if (toDate != null)
                {
                    IUIAutomationCondition editControl1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "dtToDate");
                    IUIAutomationElement editItem1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, editControl1);
                    object valuePatternObj;
                    object textPatternObj;
                    valuePatternObj = editItem1.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    textPatternObj = editItem1.GetCurrentPattern(UIA_PatternIds.UIA_TextEditPatternId);

                    IUIAutomationTextEditPattern textpattern = textPatternObj as IUIAutomationTextEditPattern;
                    IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;

                    string value = toDate;
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    valuePattern.SetValue(value);
                    Thread.Sleep(500);
                }

                IUIAutomationCondition condBtn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Get Data");
                IUIAutomationElement btn = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn);
                IUIAutomationCondition condBtn1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Calculate");
                IUIAutomationElement btn1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn1);
                IUIAutomationCondition condBtn2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Save");
                IUIAutomationElement btn2 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn2);
                BringToForeground("General Ledger", ref currentwin, exePath);
                ClickElement(btn, "Left Mouse Button Clicked", ref currentwin);
                logMsg = "Clicked on Get data button.";
                Console.WriteLine(logMsg);
                CreateLogs(logMsg, logFilePath);
                IUIAutomationCondition condStatusBar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "statusStrip1");
                IUIAutomationElement statusBar = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condStatusBar);

                if (statusBar != null)
                {
                    IUIAutomationElementArray childElements = statusBar.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    IUIAutomationElement childElement = childElements.GetElement(0);
                    while (childElement.CurrentName.Equals("Getting....."))
                        Thread.Sleep(1000);
                    Thread.Sleep(1000);
                }
                BringToForeground("General Ledger", ref currentwin, exePath);
                ClickElement(btn1, "Left Mouse Button Clicked",ref currentwin);
                logMsg = "Clicked on calculate button.";
                Console.WriteLine(logMsg);
                CreateLogs(logMsg, logFilePath);
                Thread.Sleep(2000);
                IUIAutomationCondition condDialogBox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "DayEndCash");
                IUIAutomationElement dialogBox1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condDialogBox1);

                if (dialogBox1 != null)
                {
                    IUIAutomationCondition condYes = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");
                    IUIAutomationElement yesBtn = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condYes);
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    ClickElement(yesBtn, "Left Mouse Button Clicked", ref currentwin);
                }

                if (statusBar != null)
                {
                    IUIAutomationElementArray childElements = statusBar.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    IUIAutomationElement childElement = childElements.GetElement(0);
                    while (childElement.CurrentName.Equals("Calculating....."))
                        Thread.Sleep(1000);
                    Thread.Sleep(1000);
                }
                BringToForeground("General Ledger", ref currentwin, exePath);
                ClickElement(btn2, "Left Mouse Button Clicked", ref currentwin);
                logMsg = "Clicked on save button.";
                Console.WriteLine(logMsg);
                CreateLogs(logMsg, logFilePath);
                if (statusBar != null)
                {
                    IUIAutomationElementArray childElements = statusBar.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    IUIAutomationElement childElement = childElements.GetElement(0);
                    while (!childElement.CurrentName.Equals("Day End Cash Saved."))
                        Thread.Sleep(1000);
                    exitcode = 0;
                    logMsg = "Get Day End Cash completed successfully.";
                    Console.WriteLine(logMsg);
                    CreateLogs(logMsg, logFilePath);
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Getting exception inside GetDayEndCash method.");
                logMsg = "Getting exception " + ex.Message.ToString() + " inside GetDayEndCash method.\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
                if (exitcode == -1)
                {
                    logMsg = "\nExited with code :" +exitcode;
                    CreateLogs(logMsg, logFilePath);
                    Console.WriteLine("Get day end cash failed.");
                    Environment.Exit(exitcode);
                }
            }
        }

        public static void RunManualRevaluation(IUIAutomation automation, IUIAutomationElement currentwin, string fromDate, DataRow dr)
        {
            try
            {
                String toDate = GetToDate(dr["CurrentDate"].ToString());
                SelectAccounts(automation, currentwin, dr["Accounts"].ToString().Split(',').ToList());

                if (fromDate != null)
                {
                    IUIAutomationCondition editControl = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "dtFromDate");
                    IUIAutomationElement editItem = currentwin.FindFirst(TreeScope.TreeScope_Descendants, editControl);
                    object valuePatternObj;
                    object textPatternObj;
                    valuePatternObj = editItem.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    textPatternObj = editItem.GetCurrentPattern(UIA_PatternIds.UIA_TextEditPatternId);

                    IUIAutomationTextEditPattern textpattern = textPatternObj as IUIAutomationTextEditPattern;
                    IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;

                    string value = fromDate;
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    try
                    {
                        valuePattern.SetValue(value);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("One of the identified items was in an invalid format"))
                        {
                            RestoreConsoleWindow();
                            Console.WriteLine("You have entered date in wrong format.Please enter the from date in format MM/dd/yyyy i.e. 05/29/2024");
                            value = Console.ReadLine();
                            valuePattern.SetValue(value);
                        }
                    }
                    Thread.Sleep(500);
                }

                if (toDate != null)
                {
                    IUIAutomationCondition editControl1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "dtToDate");
                    IUIAutomationElement editItem1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, editControl1);
                    object valuePatternObj;
                    object textPatternObj;
                    valuePatternObj = editItem1.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    textPatternObj = editItem1.GetCurrentPattern(UIA_PatternIds.UIA_TextEditPatternId);

                    IUIAutomationTextEditPattern textpattern = textPatternObj as IUIAutomationTextEditPattern;
                    IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;

                    string value = toDate;
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    valuePattern.SetValue(value);
                    Thread.Sleep(500);
                }

                IUIAutomationCondition condBtn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnRunRevaluation");
                IUIAutomationElement btn = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn);
                BringToForeground("General Ledger", ref currentwin, exePath);
                ClickElement(btn, "Left Mouse Button Clicked", ref currentwin);
                logMsg = "Clicked on run manual revaluation button.";
                Console.WriteLine(logMsg);
                CreateLogs(logMsg, logFilePath);
                Thread.Sleep(1000);
                IUIAutomationCondition condDialogBox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Revaluation");
                IUIAutomationElement dialogBox1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condDialogBox1);

                if (dialogBox1 != null && dr["RunManualRevaluation"].ToString().Equals("TRUE"))
                {
                    IUIAutomationCondition condBtn1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");
                    IUIAutomationElement btn1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn1);
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    ClickElement(btn1, "Left Mouse Button Clicked", ref currentwin);
                }
                else if (dialogBox1 != null && dr["RunManualRevaluation"].ToString().Equals("FALSE"))
                {
                    IUIAutomationCondition condBtn1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "No");
                    IUIAutomationElement btn1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn1);
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    ClickElement(btn1, "Left Mouse Button Clicked", ref currentwin);
                }

                IUIAutomationCondition condStatusBar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "statusStrip1");
                IUIAutomationElement statusBar = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condStatusBar);

                if (statusBar != null)
                {
                    IUIAutomationElementArray childElements = statusBar.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    IUIAutomationElement childElement = childElements.GetElement(childElements.Length - 1);
                    while (!childElement.CurrentName.Equals("Revaluation process completed"))
                        Thread.Sleep(1000);
                    exitcode = 0;
                    logMsg = "Run Manual Revaluation completed successfully.";
                    Console.WriteLine(logMsg);
                    CreateLogs(logMsg, logFilePath);
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Getting exception inside GetDayEndCash method.");
                logMsg = "Getting exception " + ex.Message.ToString() + " inside GetDayEndCash method.\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
                if (exitcode == -1)
                {
                    logMsg = "\nExited with code :" + exitcode;
                    CreateLogs(logMsg, logFilePath);
                    Console.WriteLine("Run manual revaluation failed.");
                    Environment.Exit(exitcode);
                }
            }
        }

        public static void GetAccountBalances(IUIAutomation automation, IUIAutomationElement currentwin, DataRow dr)
        {
            try
            {
                IUIAutomationCondition condTabItem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Account Balances");
                IUIAutomationElement tabItem = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condTabItem);
                BringToForeground("General Ledger", ref currentwin, exePath);
                ClickElement(tabItem, "Left Mouse Button Clicked", ref currentwin);
                String toDate = GetToDate(dr["CurrentDate"].ToString());
                SelectAccounts(automation, currentwin, dr["Accounts"].ToString().Split(',').ToList());
                if (toDate != null)
                {
                    IUIAutomationCondition editControl = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "udtBalanceDate");
                    IUIAutomationElement editItem = currentwin.FindFirst(TreeScope.TreeScope_Descendants, editControl);
                    object valuePatternObj;
                    object textPatternObj;
                    valuePatternObj = editItem.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    textPatternObj = editItem.GetCurrentPattern(UIA_PatternIds.UIA_TextEditPatternId);

                    IUIAutomationTextEditPattern textpattern = textPatternObj as IUIAutomationTextEditPattern;
                    IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;

                    string value = toDate.ToString();
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    valuePattern.SetValue(value);
                    Thread.Sleep(1000);
                }

                IUIAutomationCondition condBtn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnGetAccBalances");
                IUIAutomationElement btn = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn);
                BringToForeground("General Ledger", ref currentwin, exePath);
                ClickElement(btn, "Left Mouse Button Clicked", ref currentwin);
                logMsg = "Clicked on Get account balances button.";
                Console.WriteLine(logMsg);
                CreateLogs(logMsg, logFilePath);
                Thread.Sleep(2000);
                IUIAutomationCondition condDialogBox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Revaluation ");
                IUIAutomationElement dialogBox = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condDialogBox);

                if (dialogBox != null)
                {
                    IUIAutomationCondition condBtn2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "OK");
                    IUIAutomationElement btn2 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn2);
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    ClickElement(btn2, "Left Mouse Button Clicked", ref currentwin);
                }

                Thread.Sleep(2000);
                IUIAutomationCondition condDialogBox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Outdated Revaluation");
                IUIAutomationElement dialogBox1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condDialogBox1);

                if (dialogBox1 != null && dr["IsRevaluation"].ToString().Equals("TRUE"))
                {
                    IUIAutomationCondition condBtn1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");
                    IUIAutomationElement btn1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn1);
                    logMsg = "Clicked on Yes button on dialog box.";
                    ClickElement(btn1, "Left Mouse Button Clicked", ref currentwin);
                }
                else if (dialogBox1 != null && dr["IsRevaluation"].ToString().Equals("FALSE"))
                {
                    IUIAutomationCondition condBtn1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "No");
                    IUIAutomationElement btn1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn1);
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    ClickElement(btn1, "Left Mouse Button Clicked", ref currentwin);
                    logMsg = "Clicked on NO button on dialog box.";
                    Console.WriteLine(logMsg);
                    CreateLogs(logMsg, logFilePath);
                }


                IUIAutomationCondition condStatusBar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "statusStrip1");
                IUIAutomationElement statusBar = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condStatusBar);

                if (statusBar != null)
                {
                    IUIAutomationElementArray childElements = statusBar.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    IUIAutomationElement childElement = childElements.GetElement(childElements.Length - 1);
                    while (!childElement.CurrentName.Equals("A/C Balances Calculated Successfully"))
                        Thread.Sleep(1000);
                    exitcode = 0;
                    logMsg = "Get Account Balances completed successfully.";
                    Console.WriteLine(logMsg);
                    CreateLogs(logMsg, logFilePath);
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Getting exception while running method GetAccountBalances");
                logMsg = "Getting exception " + ex.Message + " while running method GetAccountBalances \n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
                if (exitcode == -1)
                {
                    logMsg = "\nExited with code :" + exitcode;
                    CreateLogs(logMsg, logFilePath);
                    Console.WriteLine("Get account balances failed.");
                    Environment.Exit(exitcode);
                }
            }
        }

        public static void SelectAccounts(IUIAutomation automation, IUIAutomationElement currentwin, List<string> accountList)
        {
            try
            {
                if (accountList.Count == 1 && accountList[0].Equals(""))
                    return;
                else
                {
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    IUIAutomationCondition condList = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "cmbMultiAccounts");
                    IUIAutomationElement listItem = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condList);
                    IUIAutomationElementArray listItem1 = listItem.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    for (int i = 0; i < listItem1.Length; i++)
                    {
                        IUIAutomationElement childElement = listItem1.GetElement(i);
                        if (childElement.CurrentAutomationId.Equals("MultiSelectEditor"))
                        {
                            IUIAutomationElementArray listItem3 = childElement.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                            for (int j = 0; j < listItem3.Length; j++)
                            {
                                IUIAutomationElement childElement1 = listItem3.GetElement(j);
                                // Check if the child element is a button
                                if (childElement1.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ControlTypePropertyId).Equals(UIA_ControlTypeIds.UIA_ButtonControlTypeId))
                                {
                                    ClickElement(childElement1, "Left Mouse Button Clicked", ref currentwin);
                                    break;
                                }
                            }
                        }
                    }

                    IUIAutomationCondition condList1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "checkedMultipleItems");
                    IUIAutomationElement listItem2 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condList1);
                    IUIAutomationElementArray list = listItem2.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());

                    for (int i = 0; i < list.Length; i++)
                    {
                        IUIAutomationElement childElement = list.GetElement(i);
                        // Check if the child element is a checkbox
                        if (childElement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_ControlTypePropertyId).Equals(UIA_ControlTypeIds.UIA_CheckBoxControlTypeId))
                        {
                            string checkboxName = childElement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_NamePropertyId).ToString();
                            //Console.WriteLine("Checkbox Item: " + checkboxName);
                            if (checkboxName == "Select All" || accountList.Contains(checkboxName) || (accountList.Any(s => checkboxName.Contains(s)) && checkboxName.Contains("( Last Modified On:")))
                            {
                                bool isTogglePatternAvailable = (bool)childElement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_IsTogglePatternAvailablePropertyId);

                                if (isTogglePatternAvailable)
                                {
                                    // Get the toggle pattern
                                    IUIAutomationTogglePattern togglePattern = childElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;

                                    // Toggle the checkbox
                                    togglePattern.Toggle();
                                }
                                else
                                {
                                    Console.WriteLine("Toggle pattern is not available for the checkbox.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logMsg = "getting exception " + ex.Message + " while selecting the accounts from drop down.\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
        }

        public static string GetToDate(string isCurrent)
        {
            DateTime toDate = DateTime.Now;
            try
            {
                if (isCurrent.Equals("FALSE"))
                {
                    toDate = toDate.AddDays(-1);
                    if (toDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        toDate = toDate.AddDays(-2);
                    }
                    else if (toDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        toDate = toDate.AddDays(-2);
                    }
                }
            }
            catch (Exception ex)
            {
                logMsg = "getting exception " + ex.Message + " while selecting to date.\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
            return toDate.ToString();
        }

        public static void GetDailyCalculations(IUIAutomation automation, IUIAutomationElement currentwin, string fromDate, DataRow dr)
        {
            try
            {
                String toDate = GetToDate(dr["CurrentDate"].ToString());
                SelectAccounts(automation, currentwin, dr["Accounts"].ToString().Split(',').ToList());
                if (fromDate != null)
                {
                    IUIAutomationCondition editControl = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "udtFromDate");
                    IUIAutomationElement editItem = currentwin.FindFirst(TreeScope.TreeScope_Descendants, editControl);
                    object valuePatternObj;
                    object textPatternObj;
                    valuePatternObj = editItem.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    textPatternObj = editItem.GetCurrentPattern(UIA_PatternIds.UIA_TextEditPatternId);

                    IUIAutomationTextEditPattern textpattern = textPatternObj as IUIAutomationTextEditPattern;
                    IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;

                    string value = fromDate.ToString();
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    try
                    {
                        valuePattern.SetValue(value);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("One of the identified items was in an invalid format"))
                        {
                            RestoreConsoleWindow();
                            Console.WriteLine("You have entered date in wrong format.Please enter the from date in format MM/dd/yyyy i.e. 05/29/2024");
                            value = Console.ReadLine();
                            valuePattern.SetValue(value);
                        }
                    }
                    Thread.Sleep(500);
                }

                if (toDate != null)
                {
                    IUIAutomationCondition editControl1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "udtToDate");
                    IUIAutomationElement editItem1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, editControl1);
                    object valuePatternObj;
                    object textPatternObj;
                    valuePatternObj = editItem1.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    textPatternObj = editItem1.GetCurrentPattern(UIA_PatternIds.UIA_TextEditPatternId);

                    IUIAutomationTextEditPattern textpattern = textPatternObj as IUIAutomationTextEditPattern;
                    IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;

                    string value = toDate.ToString();
                    BringToForeground("General Ledger", ref currentwin, exePath);
                    valuePattern.SetValue(value);
                    Thread.Sleep(500);
                }

                IUIAutomationCondition condBtn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Get Data");
                IUIAutomationElement btn = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn);

                IUIAutomationCondition condBtn1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnCalculate");
                IUIAutomationElement btn1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn1);

                IUIAutomationCondition condBtn2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Save");
                IUIAutomationElement btn2 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn2);
                BringToForeground("General Ledger", ref currentwin, exePath);
                ClickElement(btn, "Left Mouse Button Clicked", ref currentwin);
                logMsg = "Clicked on Get data button";
                Console.WriteLine(logMsg);
                CreateLogs(logMsg, logFilePath);
                IUIAutomationCondition condStatusBar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "statusStrip1");
                IUIAutomationElement statusBar = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condStatusBar);

                if (statusBar != null)
                {
                    IUIAutomationElementArray childElements = statusBar.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    IUIAutomationElement childElement = childElements.GetElement(0);
                    while (childElement.CurrentName.Equals("Getting Data...."))
                        Thread.Sleep(1000);
                    Thread.Sleep(2000);
                }
                BringToForeground("General Ledger", ref currentwin, exePath);
                ClickElement(btn1, "Left Mouse Button Clicked", ref currentwin);
                logMsg = "Clicked on calculate buttton.";
                Console.WriteLine(logMsg);
                CreateLogs(logMsg, logFilePath);
                if (statusBar != null)
                {
                    IUIAutomationElementArray childElements = statusBar.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    IUIAutomationElement childElement = childElements.GetElement(0);
                    while (childElement.CurrentName.Equals("Calculating..."))
                        Thread.Sleep(1000);
                    Thread.Sleep(500);
                }
                BringToForeground("General Ledger", ref currentwin, exePath);
                ClickElement(btn2, "Left Mouse Button Clicked", ref currentwin);
                logMsg = "Clicked on save button.";
                Console.WriteLine(logMsg);
                CreateLogs(logMsg, logFilePath);
                if (statusBar != null)
                {
                    IUIAutomationElementArray childElements = statusBar.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    IUIAutomationElement childElement = childElements.GetElement(0);
                    while (!childElement.CurrentName.Equals("Daily Calculation Data Saved."))
                        Thread.Sleep(1000);
                    logMsg = "Daily Calculations completed successfully.";
                    Console.WriteLine(logMsg);
                    CreateLogs(logMsg, logFilePath);
                    exitcode = 0;
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Getting exception inside GetDailyCalculations method.");
                logMsg = "Getting exception " + ex.Message.ToString() + " inside GetDailyCalculations method.\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
                if (exitcode == -1)
                {
                    logMsg = "\nExited with code :" + exitcode;
                    CreateLogs(logMsg, logFilePath);
                    Console.WriteLine("Get daily calculations failed.");
                    Environment.Exit(exitcode);
                }
            }
        }

        public static void Main(string[] args)
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += GlobalKeyboardHook_KeyboardPressed;

            string command = "";
            if (args.Length == 0)
            {
                Console.WriteLine("Enter c for CREATION of testcases and r for RECORDING workflow ");
                command = Console.ReadLine();
            }
            else
            {
                command = args[0];
            }
            if (command == "g")
            {
                if (!Directory.Exists(logFolderPath))
                {
                    Directory.CreateDirectory(logFolderPath);
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                string glConfig = "GLConfigs";
                if (!Directory.Exists(glConfig))
                {
                    exitcode = 15;
                    logMsg = "GLConfigs folder does not exists.Please craete folder to add all the required files.\nExited with code : "+exitcode;
                    CreateLogs(logMsg, logFilePath, 0);
                    Environment.Exit(exitcode);
                }
                string mainfilePath = @"GLConfigs\MainFile.csv";
                string dataFilePath = @"GLConfigs\GLDataFile.csv";
                Dictionary<string, string> tabNameDict = new Dictionary<string, string>();
                System.Data.DataTable mainFileData = GetDataFromCsvFile(mainfilePath);
                System.Data.DataTable glData = GetDataFromCsvFile(dataFilePath);
                exePath = GetValueFromMainFile(mainFileData, "exePath");
                try
                {
                    foreach (DataRow dr in glData.Rows)
                    {
                        string tabName = dr[0].ToString();
                        if (tabName.Equals("Chart of CashAccounts"))
                        {
                            tabNameDict[tabName] = "";
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Please enter the from date in format MM/dd/yyyy i.e. 05/29/2024 for " + tabName);
                            command = Console.ReadLine();
                            tabNameDict[tabName] = command;
                        }
                    }
                    MinimizeConsoleWindow();
                    Thread.Sleep(4000);
                    Console.WriteLine("Started GL processes.");
                    int fixedX = 500; // Adjust these values as needed
                    int fixedY = 500;
                    // Block mouse input
                    //BlockMouseInput(true);
                    //SetCursorPosition(fixedX, fixedY);
                    RunGLProcesses(mainFileData, exePath, tabNameDict, glData);
                    finalTime = DateTime.Now;

                    TimeSpan timeDifference = finalTime - initialTime;
                    string totalTimeTaken = timeDifference.Hours + " Hours " + timeDifference.Minutes + " Minutes " + timeDifference.Seconds + " Seconds " + timeDifference.Milliseconds + " Milliseconds ";
                    Console.WriteLine("Total Time Taken : " + totalTimeTaken);

                    string timeLogMessage = "Total Time Taken : " + totalTimeTaken;
                    CreateLogs(timeLogMessage, logFilePath, 0);

                    logMsg = "*************************************************************SUCCESS!!*************************************************************";
                    CreateLogs(logMsg, logFilePath, 0);

                    Console.WriteLine("SUCCESS!!");

                    Console.Clear(); // Clear the console
                    RestoreConsoleWindow();
                    if (exitcode == 0)
                        logMsg="GL processes completed successfully!!";
                    else
                        logMsg="Exited with exitcode : "+exitcode;
                    Console.WriteLine(logMsg);
                    CreateLogs(logMsg, logFilePath, 0);

                    //if (!BlockInput(false))
                    //{
                    //    Console.WriteLine("Failed to unblock input.");
                    //}
                    
                    // Block mouse input
                    //BlockMouseInput(false);
                    Environment.Exit(exitcode);
                }
                catch (Exception ex)
                {
                    logMsg = "Getting exception " + ex.Message + " while runnning main method.\n" + ex.StackTrace;
                    Console.WriteLine(logMsg);
                    CreateLogs(logMsg, logFilePath);
                }
                finally
                {

                }
            }
            else
            {
                Console.WriteLine("Please give a valid input.");
            }
        }

        private static IntPtr GetMainWindowHandle(Process process)
        {
            IntPtr mainWindowHandle = IntPtr.Zero;
            EnumWindows((hWnd, lParam) =>
            {
                uint pid;
                GetWindowThreadProcessId(hWnd, out pid);
                if (pid == process.Id)
                {
                    mainWindowHandle = hWnd;
                    return false; // Stop enumerating
                }
                return true;
            }, IntPtr.Zero);
            return mainWindowHandle;
        }

        private static void GlobalKeyboardHook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            // Specify the key you want to use to stop the application (e.g., Escape key)
            if ((Keys)e.VirtualCode == Keys.Escape)
            {
                // Terminate the application
                Console.WriteLine("User exited the application" );
                //BlockMouseInput(false);
                Process.GetCurrentProcess().Kill();
            }
        }

        static void BlockMouseInput(bool block)
        {
            if (!BlockInput(block))
            {
                int errorCode = Marshal.GetLastWin32Error();
                Console.WriteLine("Failed to block mouse input. Error code: " + errorCode);
            }
        }

        static void SetCursorPosition(int fixedX, int fixedY)
        {
            // Continuously set the cursor position to the fixed point
            while (true)
            {
                SetCursorPos(fixedX, fixedY);
                Thread.Sleep(100); // Adjust the delay as needed
            }
        }
    }


    public class GlobalKeyboardHook : IDisposable
    {
        private IntPtr _windowsHookHandle;
        private IntPtr _user32LibraryHandle;
        private HookProc _hookProc;

        public event EventHandler<GlobalKeyboardHookEventArgs> KeyboardPressed;

        public GlobalKeyboardHook()
        {
            _hookProc = HookCallback;
            _user32LibraryHandle = LoadLibrary("User32");
            _windowsHookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, _hookProc, _user32LibraryHandle, 0);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                KeyboardPressed?.Invoke(this, new GlobalKeyboardHookEventArgs(vkCode));
            }
            return CallNextHookEx(_windowsHookHandle, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(_windowsHookHandle);
        }

        ~GlobalKeyboardHook()
        {
            Dispose();
        }

        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x0100;

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibrary(string lpFileName);
    }

    public class GlobalKeyboardHookEventArgs : EventArgs
    {
        public int VirtualCode { get; private set; }

        public GlobalKeyboardHookEventArgs(int virtualCode)
        {
            VirtualCode = virtualCode;
        }
    }
}
