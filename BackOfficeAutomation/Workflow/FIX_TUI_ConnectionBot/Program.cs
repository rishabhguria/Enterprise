using Microsoft.VisualBasic.FileIO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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

namespace CheckStuckTrades
{
    
    internal class StuckedTradesExporter
    {
        public static DateTime initialTime = DateTime.Now;
        public static DateTime finalTime;
        public static string logFolderPath = Directory.GetCurrentDirectory() + "\\Logs";
        public static string logFilePath = logFolderPath + "\\FixTUI_Logs_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        static IUIAutomationElement _currwinglobal = null;
        public static string logMsg = string.Empty;
        public static int exitcode = -1;

        public static int totalfixprocessed=0;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        public static void MinimizeConsoleWindow()
        {
            const int SW_MINIMIZE = 6;
            IntPtr hWndConsole = GetConsoleWindow();
            if (hWndConsole != IntPtr.Zero)
            {
                ShowWindow(hWndConsole, SW_MINIMIZE);
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
                    CreateLogs(currentwin.CurrentName);
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
                logMsg = "Getting exception " + ex.Message + " while maximizing " + currentwin.CurrentName + " window.";
                CreateLogs(logMsg);
            }
        }

        public static DataTable GetDataFromCsvFile(string csvFile)
        {
            DataTable dataTable = new DataTable();
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
                    logMsg = "Stucked trade config files does not exists.";
                   // CreateLogswithoutconsolelog(logMsg + ".\nExited with code : " + exitcode);
                    CreateLogs("Exited with code : " + exitcode);
                  //  Environment.Exit(exitcode);
                }
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while reading data from file " + csvFile;
                CreateLogs(logMsg);
            }
            return dataTable;
        }

        public static string GetValueFromMainFile(DataTable dt, string Name)
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
                logMsg = "Getting exception " + ex.Message + " while reading data from Main Data file.";
                CreateLogs(logMsg);
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
                    CreateLogs("Entered for button and elements is:" + targetelement);
                    string value = null;
                    object patternprovider;
                    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                    {
                        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                        CreateLogs(targetelement.CurrentName);

                        IUIAutomationInvokePattern selectionpatternprovider = patternprovider as IUIAutomationInvokePattern;
                        //Thread modalThread = new Thread(HandleModalDialog);
                       // modalThread.Start();
                        selectionpatternprovider.Invoke();
                        //modalThread.Join();
                        CreateLogs("Clicked on " + targetelement);

                    }
                    return;
                }
                if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_CheckBoxControlTypeId)
                {
                    //todo: check if aready false or true!!
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
                logMsg = "Getting exception " + ex.Message + " while clicking element " + targetelement.CurrentName + ".";
                exitcode = 23;
                CreateLogswithoutconsolelog(logMsg + ".\nExited with code : " + exitcode);
                CreateLogs("Exited with code : " + exitcode);
                //Environment.Exit(exitcode);
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

        public static string ReadExeConfig(string filePath)
        {
            string path;
            try
            {
                // Open the file with a StreamReader
                using (StreamReader reader = new StreamReader(filePath))
                {
                    // Read the first line (assuming it contains the path)
                    path = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                CreateLogs($"An error occurred: {ex.Message}");
                return null;
            }
            return path;
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
                    logMsg = "Gettng exception " + ex.Message + " while waiting for element " + element.CurrentName + " to be visible";
                    CreateLogs(logMsg);
                    //CreateLogs(logMsg);
                    Thread.Sleep(retryDelayMilliseconds);
                }
            }
        }

        private static void ClickElement(IUIAutomationElement element, string clicktype)
        {
            try
            {
                if (element == null)
                {
                    return;
                }
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


                }
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while clicking element " + element.CurrentName + ".";
                exitcode = 23;
                CreateLogswithoutconsolelog(logMsg + ".\nExited with code : " + exitcode);
                CreateLogs("Exited with code : " + exitcode);
               // Environment.Exit(exitcode);
            }
        }

        public static void CreateLogs(string logMessage, int dateCheck = 1)
        {
            try
            {
                Console.WriteLine(logMessage);
                if (File.Exists(logFilePath))
                {
                    using (StreamWriter writer = File.AppendText(logFilePath))
                    {
                        string formattedDateTime = DateTime.Now.ToString("[yy-MM-dd HH:mm:ss]");
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
                        string formattedDateTime = DateTime.Now.ToString("[yy-MM-dd HH:mm:ss]");
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
            catch (Exception ex)
            {
                exitcode = 9;
                logMsg = "Getting exception " + ex.Message + " while saving logs.";
                CreateLogs(logMessage);
            }

        }

        public static void CreateLogswithoutconsolelog(string logMessage, int dateCheck = 1)
        {

            try
            {
                // CreateLogs(logMessage);
                if (File.Exists(logFilePath))
                {
                    using (StreamWriter writer = File.AppendText(logFilePath))
                    {
                        string formattedDateTime = DateTime.Now.ToString("[yy-MM-dd HH:mm:ss]");
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
                        string formattedDateTime = DateTime.Now.ToString("[yy-MM-dd HH:mm:ss]");
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
            catch { }

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

                if (f == false && currwin.Contains("TradeServiceUI"))
                {
                    Process[] processes = Process.GetProcessesByName("Prana.TradeServiceUI");
                    foreach (Process process in processes)
                    {
                        try
                        {
                            // Get the process ID
                            int processId = process.Id;

                            // Get the path of the executable
                            string path = process.MainModule.FileName;
                            if (path.Equals(exePath))
                            {
                                var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");
                                var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, currwin);
                                var wincond = automation.CreateAndCondition(cond1, cond2);
                                element = root.FindFirst(TreeScope.TreeScope_Descendants, wincond);
                            }
                            else
                                MinimizeWindow(currwin, ref currentwin);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
                else if (f == false)
                {

                    var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");
                    var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, currwin);


                    var wincond = automation.CreateAndCondition(cond1, cond2);

                    element = root.FindFirst(TreeScope.TreeScope_Descendants, wincond);

                }




                if (element != null)
                {
                    currentwin = element;
                    CreateLogs(currentwin.CurrentName);
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
                logMsg = "Getting exception " + ex.Message + " while bringing " + currentwin.CurrentName + " to foreground.";
                exitcode = 22;
                CreateLogswithoutconsolelog(logMsg + ".\nExited with code : " + exitcode);
                CreateLogs("Exited with code : " + exitcode);
              //  Environment.Exit(exitcode);
            }
        }

        public static void HandleFIXConnection(string exePath)
        {
            try
            {
                Dictionary<string, List<IUIAutomationElement>> FixtoStatusBttnMapping = new Dictionary<string, List<IUIAutomationElement>>();
                IntPtr handle = IntPtr.Zero;
                IUIAutomation automation = new CUIAutomation8();
                IUIAutomationElement currentwin = null;
                try
                {
                    handle = GetForegroundWindow();
                    currentwin = automation.ElementFromHandle(handle);
                    BringToForeground("TradeServiceUI", ref currentwin, exePath);
                    _currwinglobal = currentwin;
                }
                catch (Exception ex)
                {
                    CreateLogs("Error while getting foregroundwindow");
                    string msg = ex.Message.ToString();
                    CreateLogs(msg);
                }
                Thread.Sleep(5000);
                if (currentwin != null)
                {
                    CreateLogs(currentwin.CurrentName);
                }
                _currwinglobal = currentwin;

                while (currentwin == null || (currentwin.CurrentAutomationId != "TradeServiceUI"))
                {
                    BringToForeground("TradeServiceUI", ref currentwin, exePath);
                    CreateLogs(currentwin.CurrentName);
                    Thread.Sleep(5000);
                    if (currentwin != null)
                    {
                        CreateLogs(currentwin.CurrentName);
                    }
                    _currwinglobal = currentwin;
                }

                if ((currentwin.CurrentAutomationId == "TradeServiceUI"))
                {

                    IUIAutomationCondition statusElementcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "lblStatus");
                    IUIAutomationCondition NameElementcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "lblCpName");
                    IUIAutomationCondition BttnElementcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnConnect");
                    IUIAutomationCondition condtitlebar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "TitleBar");

                    IUIAutomationElement titlebar = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condtitlebar);
                    InputSimulator inputSimulator = new InputSimulator();
                    tagPOINT p;
                    if (titlebar != null)
                    {
                        titlebar.GetClickablePoint(out p);
                        System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)p.x, (int)p.y);
                        inputSimulator.Mouse.LeftButtonClick();
                        Thread.Sleep(500);
                    }

                    // do connection disconnection based on 
                    IUIAutomationCondition conditionFixConnectionPanelall = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ultraPanelFixConnections");
                    IUIAutomationElement FixconnectionPanelElementall = currentwin.FindFirst(TreeScope.TreeScope_Descendants, conditionFixConnectionPanelall);
                    int bottomcontainerlength = FixconnectionPanelElementall.CurrentBoundingRectangle.bottom;
                    int topcontainerlength= FixconnectionPanelElementall.CurrentBoundingRectangle.top;
                    int rightcontainerlength= FixconnectionPanelElementall.CurrentBoundingRectangle.right;
                    int leftcontainerlength= FixconnectionPanelElementall.CurrentBoundingRectangle.left;
                    IUIAutomationCondition conditionFixConnectionPanel = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "UsrCtrlConnectionStatus");
                    IUIAutomationElementArray FixconnectionPanelElement = currentwin.FindAll(TreeScope.TreeScope_Descendants, conditionFixConnectionPanel);
                    CreateLogs("Found " + FixconnectionPanelElement.Length + " Fix on TUI");
                    IUIAutomationElement LastElementInfo=null;
                    for (int i = 0; i < FixconnectionPanelElement.Length; i++)
                    {
                        
                        try
                        {

                            IUIAutomationElement IndividualFixElement = FixconnectionPanelElement.GetElement(i);

                            IUIAutomationElement NameElement = IndividualFixElement.FindFirst(TreeScope.TreeScope_Descendants, NameElementcond);
                            IUIAutomationElement BttnElement = IndividualFixElement.FindFirst(TreeScope.TreeScope_Descendants, BttnElementcond);
                            IUIAutomationElement StatusElement = IndividualFixElement.FindFirst(TreeScope.TreeScope_Descendants, statusElementcond);
                            if (i == FixconnectionPanelElement.Length - 1)
                            {
                                LastElementInfo = NameElement;
                            }
                            if (FixtoStatusBttnMapping.ContainsKey(NameElement.CurrentName))
                            {
                                CreateLogs("Duplicate Fix Name Found On TUI Aborting Fix Work Please Check Config");
                               // return;
                            }
                            else
                            {
                                FixtoStatusBttnMapping.Add(NameElement.CurrentName, new List<IUIAutomationElement> { StatusElement, BttnElement });
                            }
                        }
                        catch (Exception ex)
                        {
                            CreateLogs(ex.ToString());
                        }

                    }
                    CreateLogs(Status.ToLower());
                    foreach(string s in FixtoStatusBttnMapping.Keys)
                    {
                        CreateLogs("FIX NAME ON UI : " + s);
                        CreateLogs(s);

                    }
                    foreach(string s in ListOfFixNames)
                    {
                        CreateLogs("FIX NAME IN LOGFILE : " + s);
                        CreateLogs(s);
                    }
                    //Console.ReadLine();
                    if (Status.ToLower() == "connect")
                    {
                        CreateLogs("Inside Connect");
                        //Console.ReadLine();
                        foreach (string fixname in ListOfFixNames)
                        {
                            if (FixtoStatusBttnMapping.ContainsKey(fixname))
                            {
                               // CreateLogs("Inside list");
                               // Console.ReadLine();
                                IUIAutomationElement statuselement = FixtoStatusBttnMapping[fixname][0];
                                if (statuselement.CurrentName == "NotConnected" || statuselement.CurrentName == "NoSellServer")
                                {
                                    IUIAutomationElement bttnelement = FixtoStatusBttnMapping[fixname][1];
                                   
                                    tagPOINT cp;
                                    bttnelement.GetClickablePoint(out cp);
                                    DateTime startTime = DateTime.Now;
                                    while ( (bttnelement.CurrentBoundingRectangle.top < topcontainerlength) && (DateTime.Now - startTime).TotalSeconds <= 120)
                                    {
                                        if ((int)cp.y != 0)
                                        {
                                            break;
                                        }

                                        Console.WriteLine((int)cp.y);
                                        Console.WriteLine(topcontainerlength);
                                        BringToForeground("TradeServiceUI", ref currentwin, exePath);
                                        if (currentwin.CurrentName.Trim().Replace(" ", "").ToLower().Contains("tradeserviceui"))
                                        {
                                            Console.WriteLine("Current Win : " + currentwin.CurrentName);
                                            int right = rightcontainerlength - 20;
                                            int top = topcontainerlength + 20;
                                            System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)right, (int)top);
                                            Thread.Sleep(5000);

                                            inputSimulator.Mouse.LeftButtonClick();

                                            inputSimulator.Mouse.VerticalScroll(1);
                                        }
                                        bttnelement.GetClickablePoint(out cp);

                                        Thread.Sleep(1000);
                                    }
                                    startTime = DateTime.Now;
                                    while ( (bttnelement.CurrentBoundingRectangle.bottom> bottomcontainerlength) && (DateTime.Now - startTime).TotalSeconds <= 120)
                                    {

                                        if((int)cp.y != 0)
                                        {
                                            break;
                                        }
                                        BringToForeground("TradeServiceUI", ref currentwin, exePath);
                                        if (currentwin.CurrentName.Trim().Replace(" ", "").ToLower().Contains("tradeserviceui"))
                                        {

                                            Console.WriteLine((int)cp.y);
                                            Console.WriteLine(bottomcontainerlength);
                                            int right = rightcontainerlength - 20;
                                            int top = topcontainerlength + 20;
                                            System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)right, (int)top);
                                            Thread.Sleep(5000);

                                            inputSimulator.Mouse.LeftButtonClick();

                                            inputSimulator.Mouse.VerticalScroll(-1);
                                            

                                        }
                                        bttnelement.GetClickablePoint(out cp);

                                        Thread.Sleep(1000);
                                    }
                                    try
                                    {
                                        object patternobj;
                                        if (bttnelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                        {
                                            patternobj = bttnelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                            IUIAutomationScrollItemPattern scrolltoview = patternobj as IUIAutomationScrollItemPattern;
                                            scrolltoview.ScrollIntoView();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        CreateLogs(ex.ToString());
                                    }
                                    object patternsavebttn1;
                                    if (bttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                                    {
                                        patternsavebttn1 = bttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                                        IUIAutomationInvokePattern bttnpattern = patternsavebttn1 as IUIAutomationInvokePattern;
                                        bttnpattern.Invoke();
                                    }
                                    CreateLogs(bttnelement.CurrentName);
                                    int maxRetries = 20;
                                    int retryDelayMilliseconds = 2000; // 2 second

                                    for (int retryCount = 0; retryCount < maxRetries; retryCount++)
                                    {
                                        try
                                        {
                                            if (bttnelement.CurrentName == "Disconnect")
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Thread.Sleep(retryDelayMilliseconds);
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                    if (bttnelement.CurrentName != "Disconnect")
                                    {
                                        CreateLogs(fixname + " not CONNECTED PLEASE CHECK");
                                        CreateLogs(statuselement.CurrentName + " for " + fixname);
                                       // Environment.Exit(-1);
                                    }
                                    else
                                    {
                                        CreateLogs(fixname + "CONNECTED");
                                        CreateLogs(statuselement.CurrentName + " for " + fixname);
                                        totalfixprocessed += 1;
                                    }
                                    
                                }
                                else
                                {
                                    CreateLogs(fixname + "  Already Connected Skipped Connecting This.");
                                }

                            }

                        }
                    }
                    else if (Status.ToLower() == "disconnect")
                    {
                        CreateLogs("Inside Disconnect");
                       // Console.ReadLine();
                        foreach (string fixname in ListOfFixNames)
                        {
                            if (FixtoStatusBttnMapping.ContainsKey(fixname))
                            {
                                IUIAutomationElement statuselement = FixtoStatusBttnMapping[fixname][0];
                                if (statuselement.CurrentName == "Connected" || statuselement.CurrentName=="NoSellServer")
                                {
                                    IUIAutomationElement bttnelement = FixtoStatusBttnMapping[fixname][1];
                                    Console.WriteLine(bttnelement.CurrentBoundingRectangle.bottom);
                                    Console.WriteLine(bttnelement.CurrentBoundingRectangle.bottom + 6);
                                    Console.WriteLine(bottomcontainerlength);
                                    tagPOINT cp;
                                    bttnelement.GetClickablePoint(out cp);
                                    DateTime startTime = DateTime.Now;
                                    while ((bttnelement.CurrentBoundingRectangle.top < topcontainerlength) && (DateTime.Now - startTime).TotalSeconds <= 120)
                                    {
                                        if ((int)cp.y != 0)
                                        {
                                            break;
                                        }

                                        BringToForeground("TradeServiceUI", ref currentwin, exePath);
                                        if (currentwin.CurrentName.Trim().Replace(" ", "").ToLower().Contains("tradeserviceui"))
                                        {

                                            int right =rightcontainerlength-20;
                                            int top =topcontainerlength+20;
                                            System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)right, (int)top);
                                            Thread.Sleep(5000);

                                            inputSimulator.Mouse.LeftButtonClick();

                                            inputSimulator.Mouse.VerticalScroll(1);
                                        }

                                        Thread.Sleep(1000);
                                    }
                                    startTime = DateTime.Now;
                                    while ((bttnelement.CurrentBoundingRectangle.bottom > bottomcontainerlength) && (DateTime.Now - startTime).TotalSeconds <= 120)
                                    {
                                        if ((int)cp.y != 0)
                                        {
                                            break;
                                        }

                                        BringToForeground("TradeServiceUI", ref currentwin, exePath);
                                        if (currentwin.CurrentName.Trim().Replace(" ", "").ToLower().Contains("tradeserviceui"))
                                        {

                                            int right = rightcontainerlength - 20;
                                            int top = topcontainerlength + 20;
                                            System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)right, (int)top);
                                            Thread.Sleep(5000);

                                            inputSimulator.Mouse.LeftButtonClick();

                                            inputSimulator.Mouse.VerticalScroll(-1);
                                        }

                                        Thread.Sleep(1000);
                                    }


                                    object patternobj;
                                    if (bttnelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                    {
                                        patternobj = bttnelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                        IUIAutomationScrollItemPattern scrolltoview = patternobj as IUIAutomationScrollItemPattern;
                                        scrolltoview.ScrollIntoView();
                                    }
                                    CreateLogs(bttnelement.CurrentName);



                                    tagPOINT pp;
                                    bttnelement.GetClickablePoint(out pp);
                                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)pp.x, (int)pp.y);

                                    inputSimulator.Mouse.LeftButtonClick();
                                    Thread.Sleep(2000);
                                    //handle dialog box here 
                                    CreateLogs(currentwin.CurrentName);
                                    IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Prana Warning");
                                    IUIAutomationCondition conddialogbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_WindowControlTypeId);
                                    IUIAutomationAndCondition newandcond = (IUIAutomationAndCondition)automation.CreateAndCondition(conddialogbox, conddialogbox1);

                                    IUIAutomationElement dialogbox = currentwin.FindFirst(TreeScope.TreeScope_Descendants, newandcond);
                                    if (dialogbox != null)
                                    {
                                        IUIAutomationCondition okbttncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");
                                        IUIAutomationElement okbttnelement = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, okbttncond);

                                        object patternsavebttn;
                                        if (okbttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                                        {
                                            patternsavebttn = okbttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                                            IUIAutomationInvokePattern bttnpattern = patternsavebttn as IUIAutomationInvokePattern;
                                            bttnpattern.Invoke();


                                        }

                                    }
                                    int maxRetries = 20;
                                    int retryDelayMilliseconds = 2000; // 2 second

                                    for (int retryCount = 0; retryCount < maxRetries; retryCount++)
                                    {
                                        try
                                        {
                                            if (bttnelement.CurrentName == "Connect")
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Thread.Sleep(retryDelayMilliseconds);
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                    if (bttnelement.CurrentName != "Connect")
                                    {
                                       // CreateLogs(fixname + " not DISCONNECTED PLEASE CHECK");
                                        CreateLogs(fixname + " not DISCONNECTED PLEASE CHECK");
                                        CreateLogs(statuselement.CurrentName + " for " + fixname);
                                       // Environment.Exit(-1);
                                    }
                                    else
                                    {
                                       // CreateLogs(fixname + "DISCONNECTED");
                                        CreateLogs(fixname + "DISCONNECTED");
                                        CreateLogs(statuselement.CurrentName + " for " + fixname);
                                        totalfixprocessed += 1;
                                    }
                                    

                                }
                                else
                                {
                                    CreateLogs(fixname + "  Already Disconnected Skipped Connecting This.");
                                }
                            }

                        }
                    }





                }
                
            }
            catch (Exception ex)
            {
               CreateLogs (ex.Message);
                CreateLogs (ex.StackTrace);
                logMsg = "Getting exception " + ex.Message + " while doing fix work on TUI.";
                CreateLogs(logMsg);
                //Console.ReadLine();
            }
        }
        public static DataTable DataFromCsvFile(string csvFile)
        {

            DataTable dataTable = new DataTable();

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

            return dataTable;
        }

        public static string Status = "";

        public static string FixNames = "";

        public static List<string> ListOfFixNames=new List<string>();
        public static void Main(string[] args)
        {
            string command = "";
            if (args.Length == 0)
            {
                Console.WriteLine("Enter ch to Proceed for Fix Connection/Disconnection work on TUI");
                command = Console.ReadLine();
            }
            else
            {
                command = args[0];
            }
            if (command == "ch")
            {

                CreateLogs("Starting Fix work ...");
                if (!Directory.Exists(logFolderPath))
                {
                    Directory.CreateDirectory(logFolderPath);
                }
               

                string stuckedTradesConfig =Directory.GetCurrentDirectory() + @"\FIXTUIConfigs";
                if (!Directory.Exists(stuckedTradesConfig))
                {
                    logMsg = "FIXTUIConfigs  folder does not exists.Please create folder to add all the required files.";
                    CreateLogs(logMsg, 0);
                }

                string exeConfigFilePath = Directory.GetCurrentDirectory() + @"\FIXTUIConfigs\TUIexePath.txt";

                if (!File.Exists(exeConfigFilePath))
                {
                    // If the file does not exist, create it
                    using (StreamWriter writer = File.CreateText(exeConfigFilePath))
                    {
                    }

                    CreateLogs($"The file '{exeConfigFilePath}' is created please put the exe path in the file.");
                    CreateLogs("Press any key to continue !!");
                    Console.ReadKey();
                    return;
                }
                string mainfilePath = Directory.GetCurrentDirectory() + @"\FIXTUIConfigs\MainFile.csv";
                DataTable mainFileData = DataFromCsvFile(mainfilePath);
                /// int configRowCount = dataFromCsvFile.Rows.Count;  
                try
                {
                    Status = GetValueFromMainFile(mainFileData, "Status");
                }
                catch (Exception ex)
                {
                    CreateLogs("Please Check Config File Error While Getting Status Value");
                    
                }
                try
                {
                    FixNames = GetValueFromMainFile(mainFileData, "NamesOfFix");
                }
                catch(Exception ex)
                {
                    CreateLogs("Please Check Config File Error While Getting Fixnames Value");
                    //CreateLogs("Please Check Config File Error While Getting Fixnames Value");
                }
                if(Status=="" || FixNames == "")
                {
                    CreateLogs("Please Check Config File Error While Getting Fixnames Value");
                    //CreateLogs("Please Check Config File Error While Getting Fixnames Value");
                   // Environment.Exit(2);
                }

                 ListOfFixNames=FixNames.Split(',').ToList();
                if (ListOfFixNames.Count == 0)
                {
                    CreateLogs("Fix Names Not Given In Config Please Check");
                   // CreateLogs("Fix Names Not Given In Config Please Check");
                    return;
                }
                string exePath = ReadExeConfig(exeConfigFilePath);
                try
                {
                    MinimizeConsoleWindow();
                    Thread.Sleep(4000);
                    if (Status.ToLower() == "connect")
                    {
                        CreateLogs("Starting Fix CONNECTION Work On TUI....");

                    }
                    else if(Status.ToLower() == "disconnect")
                    {
                        CreateLogs("Starting Fix DISCONNECTION Work On TUI....");
                    }
                    else
                    {
                        CreateLogs("Please Check Config Status Is Incorrect");
                        return;
                    }
                    HandleFIXConnection(exePath);
                    finalTime = DateTime.Now;

                    TimeSpan timeDifference = finalTime - initialTime;
                    string totalTimeTaken = timeDifference.Hours + " Hours " + timeDifference.Minutes + " Minutes " + timeDifference.Seconds + " Seconds " + timeDifference.Milliseconds + " Milliseconds ";
                    string timeLogMessage = "Total Time Taken : " + totalTimeTaken;
                    CreateLogswithoutconsolelog(timeLogMessage, 0);

                    logMsg = "*************************************************************SUCCESS!!*************************************************************";
                    CreateLogswithoutconsolelog(logMsg + ".\nExited with code : " + exitcode, 0);
                   
                    string errorCode = "ExitCode : " + exitcode + "Please refer Exit Error Code File for more information.";
                    CreateLogs(errorCode, 0);
                   // Environment.Exit(exitcode);
                }
                catch (Exception ex)
                {
                    logMsg = "Getting exception " + ex.Message + " while runnning main method.";
                    CreateLogs(logMsg);
                   // CreateLogs(logMsg);
                }
                finally
                {

                }
            }
            else
            {
                CreateLogs("Please give a valid input.");
            }
            if (totalfixprocessed == ListOfFixNames.Count)
            {
                Console.WriteLine("All Fix Processed");
            }
            else
            {
                Console.WriteLine(totalfixprocessed + " Out of " + ListOfFixNames.Count + " Given in Config Please Check TUI");
            }
            Console.WriteLine("Completed FIX Work ");
            Console.ReadLine();
        }
    }
}
