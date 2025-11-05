using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
//using System.Windows.Automation;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text;
using WindowsInput;
using WindowsInput.Native;
using System.Windows;
using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeOpenXml;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Sheets.v4;
//using Google.Apis.Sheets.v4.Data;
//using Google.Apis.Services;
using System.Text.RegularExpressions;
using System.Linq;
//using WorkflowAutomation;
using UIAutomationClient;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Drawing;
using System.Drawing.Imaging;
using WindowsInput;
using WindowsInput.Native;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using System.Xml;
using System.Windows.Media.Media3D;
using System.Runtime.ExceptionServices;
using System.Configuration;
using System.Xml.Linq;
using System.Management;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

using System.Data;
using System.Diagnostics;
using System.Management;
using System.Security.Cryptography;
using OfficeOpenXml;
using ExcelDataReader;
using AngleSharp.Text;

using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using AngleSharp.Dom;
using OpenQA.Selenium.Support.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using FactSetSample;
using OpenQA.Selenium.DevTools.V123.Runtime;
using System.Windows.Interop;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;


class User32Interop
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
}
public class XMLDateExtractor
{
    private string xmlContent;

    public XMLDateExtractor() => this.xmlContent = string.Empty;

    public XMLDateExtractor(string xmlContent) => this.xmlContent = xmlContent;

    public DateTime ExtractFilterValue()
    {
        try
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(this.xmlContent);
            XmlNode xmlNode = xmlDocument.SelectSingleNode("//FilterValue");
            if (xmlNode != null)
            {
                DateTime result;
                if (DateTime.TryParse(xmlNode.InnerText, out result))
                    return result;
                Console.WriteLine("Failed to parse FilterValue as DateTime.");
            }
            else
                Console.WriteLine("FilterValue node not found in XML.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine("Error extracting FilterValue: " + ex.Message);
        }
        return DateTime.MinValue;
    }
}


class InterceptMouseAndKeyboard
{
    private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    private const uint EVENT_OBJECT_CREATE = 0x8000;

    private static string prevwin = "";

    private static int successCounter = 0;

   
    

    IUIAutomationElement _globalelement = null;

    private static int exitcode = -1;

    
   
    private static readonly object _fileLock = new object();
    public static IUIAutomationElement _globalcurrentwin = null;

    
    private static string logFilename;

    static Dictionary<string, List<string>> alldata = new Dictionary<string, List<string>>();
    static Dictionary<string, string> exepaths = new Dictionary<string, string>();
    static IUIAutomationElement _currwinglobal = null;
    static IUIAutomationElement _datetoelement = null;
    static string _datestring = "";

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll", SetLastError = true)]
    static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

    const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    const uint MOUSEEVENTF_LEFTUP = 0x0004;


    private static IntPtr _mouseHookID = IntPtr.Zero;
    private static IntPtr _keyboardHookID = IntPtr.Zero;
    //  IntPtr hook = SetWinEventHook(EVENT_OBJECT_CREATE, EVENT_OBJECT_CREATE, IntPtr.Zero, WinEventCallback, 0, 0, 0);
    private static IntPtr _mainWindowHandle = IntPtr.Zero;
    private static bool _filecreated = false;
    private static string _filename = "";
    
    static bool screenshotModeIsOn = false;
    static bool _globalApplicationerror = false;
    static bool finalImportresult = false;

    //window hook new 7/11
    //delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
    // [DllImport("user32.dll")]
    //static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    //[DllImport("user32.dll")]
    // static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    private const uint WINEVENT_OUTOFCONTEXT = 0;
    private const uint EVENT_SYSTEM_FOREGROUND = 3;
    public static void HandleConfirmationAndWarning2()
    {
        Console.WriteLine("inside before set date modal");
        CreateExceptionReport("Inside Warning Handling");

        try
        {
            Thread.Sleep(10000);


        }
        catch (Exception ex)
        {
            Console.WriteLine("inside catch modal" + ex.StackTrace.ToString());
            CreateExceptionReport(ex.StackTrace.ToString());
        }
        Console.WriteLine("thread before date set completed");

    }

    public static void HandleConfirmationAndWarning1()
    {
        IUIAutomation automation = new CUIAutomation8();

        Console.WriteLine("inside modal");
        CreateExceptionReport("Inside Warning Handling");

        InputSimulator sim = new InputSimulator();


        try
        {
            //Thread.Sleep(1000);
            Thread modalThread = new Thread(HandleConfirmationAndWarning2);
            object copydateobj;
            IUIAutomationElement selectdateelement =_datetoelement;
            string copytodate = _datestring;
            if (selectdateelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
            {
                copydateobj = selectdateelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                IUIAutomationValuePattern valuepatt = copydateobj as IUIAutomationValuePattern;
                //modalThread.Start();

                //Console.WriteLine("before date entered");
                modalThread.Start();
                valuepatt.SetValue(copytodate);
                modalThread.Join();
                

            }


        }
        catch (Exception ex)
        {
            Console.WriteLine("inside catch modal" + ex.StackTrace.ToString());
            CreateExceptionReport(ex.StackTrace.ToString());
        }

    }
    public static void HandleConfirmationAndWarning()
    {
        IUIAutomation automation = new CUIAutomation8();


        CreateExceptionReport("Inside Warning Handling");

        InputSimulator sim = new InputSimulator();

      
        try
        {
            Thread.Sleep(5000);
            IUIAutomationElement currwin = _currwinglobal;

            IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "WARNING");
            IUIAutomationCondition conddialogbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "CONFIRMATION");
            IUIAutomationOrCondition newandcond = (IUIAutomationOrCondition)automation.CreateOrCondition(conddialogbox, conddialogbox1);

            IUIAutomationElement dialogbox = currwin.FindFirst(TreeScope.TreeScope_Descendants, newandcond);

            if (dialogbox != null)
            {
                IUIAutomationCondition okbtncond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                var btn1 = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, okbtncond1);
                replayaction(btn1, "");
            }
            else
            {
                IUIAutomationCondition okbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                var btn = currwin.FindFirst(TreeScope.TreeScope_Descendants, okbtncond);
                if (btn != null)
                {
                    replayaction(btn, "");
                }
            }



        }
        catch (Exception ex)
        {
            CreateExceptionReport(ex.StackTrace.ToString());
        }

    }
        public static void HandleModalDialog()
    {




        IUIAutomation automation = new CUIAutomation8();


        CreateExceptionReport("after upload button");

        InputSimulator sim = new InputSimulator();

        try
        {
            Thread.Sleep(2000);


            IUIAutomationCondition applicationErrorcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Application Error");

            CreateExceptionReport(_currwinglobal.CurrentName + "Inside modal");
            IUIAutomationElement applicationError = _currwinglobal.FindFirst(TreeScope.TreeScope_Element | TreeScope.TreeScope_Descendants, applicationErrorcond);
            Thread.Sleep(500);
            IUIAutomationCondition appErrorMsgcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "65535");



            if (applicationError != null)
            {
                IUIAutomationElement appErrorMsg = applicationError.FindFirst(TreeScope.TreeScope_Descendants, appErrorMsgcond);
                Thread.Sleep(500);
                _global_appication_error_msg = applicationError.CurrentName + " : ";
                if (appErrorMsgcond != null)
                {

                    _global_appication_error_msg += appErrorMsg.CurrentName;
                }




                _globalApplicationerror = true;
                CreateExceptionReport("before pressing enter");



                sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                CreateExceptionReport("after pressing enter");

            }
        }
        catch (Exception ex)
        {
            CreateExceptionReport("Error inside Modal");
            string exceptionLog = ex.Message.ToString();
            CreateLogs(exceptionLog, logFilePath);
            CreateExceptionReport(ex.StackTrace);
            //exitcode = 23;


        }

        //sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
    }
    static IUIAutomationElement globalcurrentwin = null;
    static string importpathglobal = "";
    public static void Handle_File_Import(IUIAutomationElement currentwin,string importFilePath)
    {

        CreateLogs("Inside file import function", logFilePath);
        IUIAutomation automation = new CUIAutomation8();

        IUIAutomationCondition condimportbttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnImport");
        IUIAutomationElement importbttn = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condimportbttn);


        IUIAutomationCondition condfromfilebttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "optUseImportExport");
        IUIAutomationElement fromfilebttn = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condfromfilebttn);
        object patternsavebttn1;
        if (fromfilebttn.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
        {
            patternsavebttn1 = fromfilebttn.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

            IUIAutomationInvokePattern savebttnpatt = patternsavebttn1 as IUIAutomationInvokePattern;
            savebttnpatt.Invoke();

            //getpricespattern.Invoke();

        }
        globalcurrentwin = currentwin;
        importpathglobal = importFilePath;
       
        InputSimulator simulator = new InputSimulator();
        tagPOINT p;
        importbttn.GetClickablePoint(out p);
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)p.x, (int)p.y);

        simulator.Mouse.LeftButtonClick();

        Thread.Sleep(2000);

        bool flag = false;
        int maxRetries = 50;
        int retryDelayMilliseconds = 2000; // 2 second

        for (int retryCount = 0; retryCount < maxRetries; retryCount++)
        {
            IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Select file for import");
        IUIAutomationElement dialogbox = currentwin.FindFirst(TreeScope.TreeScope_Descendants, conddialogbox);

            if (dialogbox != null)
            {
                IUIAutomationCondition conditionfilename = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId);
                IUIAutomationCondition conditionfilename1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "File name:");
                IUIAutomationAndCondition newandcond = (IUIAutomationAndCondition)automation.CreateAndCondition(conditionfilename1, conditionfilename);
                IUIAutomationElement filename = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, newandcond);
                BringToForeground("Select file for import", ref currentwin, exePath);
                if (filename != null)
                {
                    object valuePatternObj;
                    object textPatternObj;
                    valuePatternObj = filename.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    textPatternObj = filename.GetCurrentPattern(UIA_PatternIds.UIA_TextEditPatternId);

                    IUIAutomationTextEditPattern textpattern = textPatternObj as IUIAutomationTextEditPattern;
                    IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;
                    CreateExceptionReport("This is from file name " + valuePattern.CurrentValue);
                    //string value= dictmap[keyValuePair]["Select File"][rowidx.Key];
                    string value = importFilePath;

                    valuePattern.SetValue(value);



                    Thread.Sleep(1000);
                    if (valuePattern.CurrentValue == value)
                    {
                        flag = true;
                    }
                    IUIAutomationCondition condopen = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Open");
                    IUIAutomationCondition condopen1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "1");
                    IUIAutomationCondition condopenand = automation.CreateAndCondition(condopen, condopen1);
                    IUIAutomationElement openbtn = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, condopenand);
                    //BringToForeground("Select File to Import", ref currentwin, exePath);
                    if (openbtn != null)
                    {
                        replayaction(openbtn, "");
                        Thread.Sleep(2000);
                        CreateExceptionReport("OPEN button pressed successfully................");



                    }

                }

                try
                {

                    IUIAutomationCondition selectFileAlertcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Mark Price Import");
                    IUIAutomationCondition okbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "OK");


                    var selectFileAlert = currentwin.FindFirst(TreeScope.TreeScope_Descendants, selectFileAlertcond);
                    if (selectFileAlert == null)
                    {
                        throw new Exception("everything is ok");
                    }
                    Console.WriteLine("Error when Importing Prices From File In Daily Valuation");
                    var btn = selectFileAlert.FindFirst(TreeScope.TreeScope_Descendants, okbtncond);
                    replayaction(btn, "");
                    Thread.Sleep(500);
                    //IUIAutomationCondition titlebardialogboxcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "TitleBar");

                    //IUIAutomationCondition closedialogboxcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "Close");

                    //IUIAutomationElement TitleBar = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, titlebardialogboxcond);
                    //IUIAutomationElement cross = TitleBar.FindFirst(TreeScope.TreeScope_Descendants, closedialogboxcond);
                    //replayaction(cross, "");
                    Thread.Sleep(500);



                }
                catch (Exception ex)
                {
                    CreateExceptionReport("Inside File Import Finding Error Boxes");
                    string exceptionLog = ex.Message.ToString();
                    CreateLogs(exceptionLog, logFilePath);
                    CreateExceptionReport(ex.StackTrace);

                }
                if (flag)
                {
                    break;
                }
                Thread.Sleep(retryDelayMilliseconds);
            }


        }
        //before this i have to open tab and press import button 

        // modalThread.Join();
        CreateLogs("File Import in Daily Valuation completed", logFilePath);
    }
    

  
    

    public static string GetFormattedDateTime(string format)
    {
        return processDate.ToString(format);
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
    public static Process GetProcessByFullPath(string fullPath)
    {
        var snapshot = CreateToolhelp32Snapshot(SnapshotFlags.Process, 0);
        if (snapshot == IntPtr.Zero)
            return null;

        try
        {
            var entry = new PROCESSENTRY32 { dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32)) };
            if (Process32First(snapshot, ref entry))
            {
                do
                {
                    string processPath = entry.szExeFile;
                    if (string.Equals(processPath, fullPath, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            return Process.GetProcessById((int)entry.th32ProcessID);
                        }
                        catch (ArgumentException)
                        {
                            // Process with the given ID does not exist
                        }
                    }
                } while (Process32Next(snapshot, ref entry));
            }
        }
        finally
        {
            CloseHandle(snapshot);
        }

        return null;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PROCESSENTRY32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szExeFile;
    }

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr CreateToolhelp32Snapshot(SnapshotFlags dwFlags, uint th32ProcessID);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseHandle(IntPtr hObject);

    [Flags]
    public enum SnapshotFlags : uint
    {
        Process = 0x00000002
    }

    public static bool findpranaoncurrentsession(string exepath)
    {
        IUIAutomation automation = new CUIAutomation8();
        var root = automation.GetRootElement();
        IUIAutomationCondition condtitlebar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana");
        //PranaMain
        IUIAutomationCondition condtitlebar1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "PranaMain");

        IUIAutomationCondition selectOptioncondand = automation.CreateAndCondition(condtitlebar, condtitlebar1);
        var allElements = root.FindAll(TreeScope.TreeScope_Descendants, selectOptioncondand);

        // Find the first element that matches the name using the regular expression

        //CreateExceptionReport("Found Prana Instance Already Open");
        for (int i = 0; i < allElements.Length; i++)
        {
            var currentElement = allElements.GetElement(i);
            //CreateExceptionReport(currentElement.CurrentProcessId);
            Process myProcess = Process.GetProcessById(currentElement.CurrentProcessId);
            string path = GetProcessFilename(myProcess);
            //CreateExceptionReport(path);
            if (path.ToLower().Equals(exepath.ToLower()))
            {
                // CreateExceptionReport("Found Prana Instance Already Open Please close and run again");
                //CreateLogs("Found Prana Instance Already Open Please close and run again", logFilePath);
                return true;
            }
        }
        return false;
    }
    public static bool IsClientLogin(string exePath)
    {
        string logMsg = "";
        bool isLogin = false;
        string[] ownerInfo = new string[2];
        uint processId = 0;
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
                logMsg = "Elements in process collection are " + processCollection.Count + " and target path is:" + targetPath;
                CreateLogs(logMsg, logFilePath);
                // Iterate through each process
                foreach (ManagementObject process in processCollection)
                {
                    logMsg = "Entered for loop in IsClientLogin method. and found " + processCollection.Count + " elements.";
                    CreateLogs(logMsg, logFilePath);
                    string processPath = process["ExecutablePath"]?.ToString();
                    if(string.Equals(processPath, exePath, StringComparison.OrdinalIgnoreCase))
                    //if (processPath != null && processPath.ToLower().Equals(exePath.ToLower()))
                    {
                        // Get the owner information
                        process.InvokeMethod("GetOwner", (object[])ownerInfo);
                        // Get the process ID and name
                        processId = (uint)process["ProcessId"];
                        string processName = process["Name"].ToString();
                        CreateExceptionReport($"Process: {processName} (PID: {processId}), Owner: {ownerInfo[0]}, domain: {ownerInfo[1]}");

                        logMsg = $"Process: {processName} (PID: {processId}), Owner: {ownerInfo[0]}, domain: {ownerInfo[1]}";
                        CreateLogs(logMsg, logFilePath);
                        isLogin = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logMsg = "Getting exception " + ex.Message + "for owner:" + ownerInfo[0] + "PID: " + processId + ", Owner: " + ownerInfo[0] + ", domain: " + ownerInfo[1] + " while getting all the opened instances of Prana.\n " + ex.StackTrace;
            CreateLogs(logMsg, logFilePath);
        }
        return isLogin;
    }
    private static void Setprevious3daysdate(string currdate)
    {
        if (DateTime.TryParseExact(currdate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime inputDate))
        {
            DateTime yesterdaydateoriginal = inputDate.AddDays(-1);
            yesterdayoriginal = yesterdaydateoriginal.ToString("MM/dd/yyyy");

            // Date for yesterday
            DateTime yesterdayDate = inputDate.AddDays(+1);
            yesterdayString = yesterdayDate.ToString("MM/dd/yyyy");

            // Date for the day before yesterday
            DateTime dayBeforeYesterdayDate = inputDate.AddDays(+2);
            dayBeforeYesterdayString = dayBeforeYesterdayDate.ToString("MM/dd/yyyy");

            //friday date 
            DateTime previous3daydate = inputDate.AddDays(+3);
            previous3daystring = previous3daydate.ToString("MM/dd/yyyy");

            //CreateLogs($"Original Date: {currdate}");
            //CreateLogs($"Date for Yesterday: {yesterdayString}");
            //CreateLogs($"Date for Day Before Yesterday: {dayBeforeYesterdayString}");
        }
        else
        {
            CreateLogs("Invalid date format",logFilePath);
        }
    }
    private static bool checkisprocessdatemonday(string currdate)
    {
        if (DateTime.TryParseExact(currdate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime inputDate))
        {
            if (inputDate.DayOfWeek == DayOfWeek.Monday)
            {
                return true;
            }
        }
        else
        {
            CreateLogs("Incorrect Date Format",logFilePath);
        }
        return false;
    }
   
    private static string setcurrdate(string currdate)
    {
        DateTime parsedDate = DateTime.ParseExact(currdate, "M/d/yyyy", null);
        // Format the date as "mm/dd/yyyy"
        string formattedDate = parsedDate.ToString("MM/dd/yyyy");
        return formattedDate;
    }
    static List<string> copytabsindv = new List<string>();
    static List<string> copyincurrdate = new List<string>();
    static List<string> importtabs = new List<string>();
    static List<string> Tabstobeused = new List<string>();
    static List<string> targettabsindv = new List<string>();
    static Dictionary<string, string> importtabspathdict = new Dictionary<string, string>();    
    static string targetmenuitemname;
    private static IUIAutomationElement _globaldvwindow;
    static string processdate = "";
    static string processDateForUI = "";
    static string yesterdayString = "";
    static string yesterdayoriginal = "";
    static string dayBeforeYesterdayString = "";
    static string previous3daystring = "";
    static string[] SetZeroFilter=new string[0];
    static bool ismonday = false;
    static List<string> factsetsymbollistglobal = new List<string>();
    static List<string> getpricesbttntablist = new List<string>();
    static Dictionary<string, string> manualpriceentrydict = new Dictionary<string, string>();
    static string PressGetpricesbttn = "";
    static List<string> symbolstofetchfromfactset = new List<string>();
    static List<string> yahoolistglobal = new List<string>();
    public static DateTime processDate = GetProcessDate(Directory.GetCurrentDirectory() + @"\DailyValuationConfigs\ProcessDateXmlPath.txt");
    public static Dictionary<string, Dictionary<string, List<string>>> TabnametoCopyAccounts =
            new Dictionary<string, Dictionary<string, List<string>>>();

    public static void DV_YahooPrice( DataTable mainFileData, string exePath)
    {
        try
        {
            int i = 0;

            bool isLogin = false;
            //IntPtr handle = IntPtr.Zero;
            //isLogin = findpranaoncurrentsession(exePath);
            CreateLogs("Before Checking Client Login", logFilePath);
            isLogin = IsClientLogin(exePath);
            CreateLogs("After Checking Client Login", logFilePath);

            if (!isLogin)
            {
                CreateLogs("Trying for Client Login", logFilePath);
                isLogin = Login(mainFileData, exePath);
            }
            CreateLogs("Client Login Completed", logFilePath);
            //todo: have to remove this using check for foreground window 
            //Thread.Sleep(5000);
            if (isLogin == false)
            {
                CreateLogs("Client Login Failed", logFilePath);
                //exitcode = 22;
                return;
            }

            IUIAutomation automation = new CUIAutomation8();
            IUIAutomationElement currentwin = null;
            try
            {

                BringToForeground("Nirvana", ref currentwin, exePath);

                //BringToForeground(currentwin)
                _currwinglobal = currentwin;

            }
            catch (Exception ex)
            {
                CreateExceptionReport("Error while getting foregroundwindow");
                string msg = ex.Message.ToString();
                CreateLogs(msg, logFilePath);
                CreateExceptionReport(ex.StackTrace);
            }
            Thread.Sleep(1000);
            //StringBuilder Buff = new StringBuilder(nChars);
            CreateLogs("Pressing Shortcuts for daily valuation", logFilePath);
            OpenDailyValuation();
            CreateLogs("After Pressing Shortcuts for daily valuation", logFilePath);
            // OpenImportModule();

            Thread.Sleep(5000);

            if (currentwin != null)
            {

                CreateExceptionReport(currentwin.CurrentName);
            }
            //MarkPriceAndForexConversion
            BringToForeground("Daily Valuation", ref currentwin, exePath);
            if (currentwin != null)
            {
                CreateExceptionReport(currentwin.CurrentName);
            }
            _currwinglobal = currentwin;
            globalcurrentwin = currentwin;
            _globalcurrentwin = currentwin;



            while (currentwin == null || (currentwin.CurrentAutomationId != "MarkPriceAndForexConversion"))
            {
                
                BringToForeground("Nirvana", ref currentwin, exePath);
                CreateExceptionReport(currentwin.CurrentName);
                OpenDailyValuation();
                // OpenImportModule();
                MaximizeWindow("Daily Valuation", ref currentwin);
                if (currentwin != null)
                {
                    CreateExceptionReport(currentwin.CurrentName);
                }
                _currwinglobal = currentwin;
            }
            Thread.Sleep(2000);
            if ((currentwin.CurrentAutomationId == "MarkPriceAndForexConversion"))
            {
                IUIAutomationCondition condtitlebar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "_MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top");
                //IUIAutomationCondition condtitlebar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnUpload");
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
                MaximizeWindow("Daily Valuation", ref currentwin);
                Thread.Sleep(1000);
                CreateExceptionReport("Daily Valuation Opened Successfully.................................");
                //IUIAutomationElementArray allrows = findiuielementofrows1(currentwin);
                //  List<KeyValuePair<int, IUIAutomationElement>> selectedrows = findiuielementofrows(currentwin);
                IUIAutomationCondition tabcondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TabItemControlTypeId);
                CreateLogs("Initializing Variables from MainFile", logFilePath);
                string targetFilename = exePath;
                string targettabs = GetValueFromMainFile(mainFileData, "targettabs");
                string targetmodule = GetValueFromMainFile(mainFileData, "targetmodule");
    //string currdate = GetValueFromMainFile(mainFileData, "Dateofprocess");
                processDateForUI = processDate.ToString("MMddyyyy");
                if (!processDateForUI.Contains("/"))
                {
                    // If not, add slashes using string interpolation
                    processDateForUI = $"{processDateForUI.Substring(0, 2)}/{processDateForUI.Substring(2, 2)}/{processDateForUI.Substring(4)}";
                }
                string currdate = processDateForUI;
                string copytabs = GetValueFromMainFile(mainFileData, "copytabs");
                string copycurrtabs = GetValueFromMainFile(mainFileData, "copycurrdatetabs");
                targetmenuitemname = GetValueFromMainFile(mainFileData, "Targetsubmodule");
                string importtabsfile = GetValueFromMainFile(mainFileData, "importtabs");
                string factsymbols = GetValueFromMainFile(mainFileData, "FactSetSymbols");
                SetZeroFilter= GetValueFromMainFile(mainFileData, "SetZeroFilter").Split(',');

                string yahoofinancesymbols = GetValueFromMainFile(mainFileData, "YahooFinanceSymbols");
                string manualpricesymbolarray= GetValueFromMainFile(mainFileData, "Manualentrysymbol");
                string[] manualpricesymbolarraylist = manualpricesymbolarray.Split(',');
                foreach (string symbolconfig in manualpricesymbolarraylist)
                {
                    string[] singleimportconfig = symbolconfig.Split('|');
                    if (singleimportconfig.Length >= 2)
                    {
                        string symbol = singleimportconfig[0];
                        string price = singleimportconfig[1];
                        if (!manualpriceentrydict.ContainsKey(symbol))
                        {
                            manualpriceentrydict.Add(symbol, price);
                        }
                    }

                }

                string copyaccountsconfig= GetValueFromMainFile(mainFileData, "Copyaccountsconfig");
                string[] segments = copyaccountsconfig.Split(',');

                if (segments.Length > 0)
                {
                    foreach (string segment in segments)
                    {
                        if (segment != "")
                        {
                            // Trim any leading/trailing whitespace from the segment
                            string trimmedSegment = segment.Trim();

                            // Split the segment by the '#' to separate the outer key and the inner part
                            string[] outerParts = trimmedSegment.Split('#');
                            string outerKey = outerParts[0].Trim();

                            // Further split the inner part by '!' to separate the inner key and the list part
                            string[] innerParts = outerParts[1].Split('!');
                            string innerKey = innerParts[0].Trim();

                            // Split the list part by '|' to create the list of strings
                            List<string> listValues = new List<string>(innerParts[1].Split('|'));

                            // Initialize the structure
                            if (!TabnametoCopyAccounts.ContainsKey(outerKey))
                            {
                                TabnametoCopyAccounts[outerKey] = new Dictionary<string, List<string>>();
                            }
                            if (!Tabstobeused.Contains(outerKey))
                            {
                                Tabstobeused.Add(outerKey);
                            }
                            TabnametoCopyAccounts[outerKey][innerKey] = listValues;
                        }
                        }
                }

                

                string[] importtabconfig = importtabsfile.Split(',');
                string[] factsetsymbolslist = factsymbols.Split(',');
                string[] YFsymbolslist = yahoofinancesymbols.Split(',');
                PressGetpricesbttn= GetValueFromMainFile(mainFileData, "PressGetpricesbttn");
                string[] tablistforpricesbttn = PressGetpricesbttn.Split(',');

                //try
                //{
                //    =GetValueFromMainFile(mainFileData, "PreviousDayCopy");

                //}
                //catch(Exception ex)
                //{

                //}


                foreach (string s in tablistforpricesbttn)
                {
                    if (s != "")
                    {
                        getpricesbttntablist.Add(s);
                        Tabstobeused.Add(s);
                    }
                }

                foreach (string s in factsetsymbolslist)
                {
                    if(s!="")
                    {
                        factsetsymbollistglobal.Add(s);
                    }
                }
                foreach (string s in YFsymbolslist)
                {
                    if (s != "")
                    {
                        yahoolistglobal.Add(s);
                    }
                }

                foreach (string importconfig in importtabconfig)
                {
                    string[] singleimportconfig = importconfig.Split('|');
                    if(singleimportconfig.Length >= 2)
                    {
                        string tab = singleimportconfig[0];
                        string filepath = singleimportconfig[1];
                        importtabs.Add(tab);
                        if(!importtabspathdict.ContainsKey(tab))
                        importtabspathdict.Add(tab, filepath);
                    }

                }

                Setprevious3daysdate(currdate);
                processdate = yesterdayString;
                ismonday = checkisprocessdatemonday(previous3daystring);
                //Date check 
                char delimiter = ',';
                string[] tabsarray = targettabs.Split(delimiter);
                string[] modulearray = targetmodule.Split(delimiter);
                string[] copytabsarray = copytabs.Split(delimiter);
                string[] copycurrtabsarray= copycurrtabs.Split(delimiter);

                foreach (var value in copytabsarray)
                {
                    if (value != "")
                    {
                        copytabsindv.Add(value);
                        if (!Tabstobeused.Contains(value))
                        {
                            Tabstobeused.Add(value);
                        }
                    }
                }
                foreach (var value in copycurrtabsarray)
                {
                    if (value != "")
                    {
                        copyincurrdate.Add(value);
                        if (!Tabstobeused.Contains(value))
                        {
                            Tabstobeused.Add(value);
                        }
                    }
                }

                foreach (var value in importtabs)
                {
                    if (value != "")
                    {
                        // copytabsindv.Add(value);
                        if (!Tabstobeused.Contains(value))
                        {
                            Tabstobeused.Add(value);
                        }
                    }
                }

                foreach (var value in tabsarray)
                {
                    if (value != "")
                    {
                        targettabsindv.Add(value);
                        if (!Tabstobeused.Contains(value))
                        {
                            Tabstobeused.Add(value);
                        }
                    }
                }

                CreateLogs("Variables Initialized from MainFile", logFilePath);
                //IUIAutomationElementArray tabitemarray = currentwindow1.FindAll(TreeScope.TreeScope_Descendants, tabcondition);

                for (int j = 0; j < Tabstobeused.Count; j++)
                {
                    BringToForeground("Daily Valuation", ref currentwin, exePath);
                    string tabname = Tabstobeused[j];
                    CreateLogs("Working For Tab :  " +tabname, logFilePath);
                    IUIAutomationCondition tabnamecond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, tabname);
                    IUIAutomationAndCondition newandcond = (IUIAutomationAndCondition)automation.CreateAndCondition(tabcondition, tabnamecond);
                    IUIAutomationElement tabitem = currentwin.FindFirst(TreeScope.TreeScope_Descendants, newandcond);
                    if (tabitem == null)
                    {
                        CreateLogs(tabname + "Not found on UI ",logFilePath);
                        continue;
                    }
                    //CreateLogs("tabnames");
                    CreateLogs(tabitem.CurrentAutomationId,logFilePath);
                    CreateLogs(tabitem.CurrentName,logFilePath);
                    if (TabnametoCopyAccounts.Keys.Contains(tabname) || targettabsindv.Contains(tabitem.CurrentName) || copytabsindv.Contains(tabitem.CurrentName)||importtabs.Contains(tabitem.CurrentName) || getpricesbttntablist.Contains(tabitem.CurrentName) || copyincurrdate.Contains(tabitem.CurrentName))
                    {
                        object patternobj;
                        if (tabitem.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                        {
                            patternobj = tabitem.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                            IUIAutomationScrollItemPattern scrolltoview = patternobj as IUIAutomationScrollItemPattern;
                            scrolltoview.ScrollIntoView();
                        }
                        
                        //there was one extra tab element when accessing tabs through code. it was to prevent clicking that tab which is 
                        //not visible on ui. 
                        //if (j != tabitemarray.Length - 1)
                        // {
                        _currwinglobal = currentwin;
                        //Thread modalThread = new Thread(HandleConfirmationAndWarning);
                        //modalThread.Start();
                        // object patternprovider11;
                        //if (tabitem.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId) != null)
                        //{
                        //    patternprovider11 = tabitem.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                        //    IUIAutomationSelectionItemPattern selectionpatternprovider = patternprovider11 as IUIAutomationSelectionItemPattern;
                        //    selectionpatternprovider.Select();

                        //}
                        ClickElement(tabitem, "Left Mouse Button Clicked");
                        Thread.Sleep(1000);
                        try
                        {

                            IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "WARNING");
                            IUIAutomationCondition conddialogbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "CONFIRMATION");
                            IUIAutomationOrCondition newandcond1 = (IUIAutomationOrCondition)automation.CreateOrCondition(conddialogbox, conddialogbox1);

                            IUIAutomationElement dialogbox = currentwin.FindFirst(TreeScope.TreeScope_Descendants, newandcond1);

                            if (dialogbox != null)
                            {
                                IUIAutomationCondition okbtncond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                                var btn1 = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, okbtncond1);
                                replayaction(btn1, "");
                            }
                            IUIAutomationCondition okbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                            var btn = currentwin.FindFirst(TreeScope.TreeScope_Descendants, okbtncond);
                            if (btn != null)
                            {
                                replayaction(btn, "");
                            }



                        }
                        catch (Exception ex)
                        {
                            CreateExceptionReport(ex.StackTrace.ToString());
                        }

                        // modalThread.Join();
                        // }
                    }

                    if (TabnametoCopyAccounts.Keys.Contains(tabname) || targettabsindv.Contains(tabitem.CurrentName) || copytabsindv.Contains(tabitem.CurrentName) || importtabs.Contains(tabitem.CurrentName) || getpricesbttntablist.Contains(tabitem.CurrentName) || copyincurrdate.Contains(tabitem.CurrentName))
                    {
                        //Handle_File_Export(currentwindow1);
                        CreateLogs("Going for Daily Valuation Work in Tab :  " + tabitem.CurrentName, logFilePath);
                        handledvwork(currentwin, tabitem.CurrentName,mainFileData);
                        CreateLogs("Daily Valuation Work in Tab :  " + tabitem.CurrentName + "  Completed", logFilePath);
                        IUIAutomationCondition condsavebttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnSave");
                        IUIAutomationElement savebttnelement = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condsavebttn);

                        object patternsavebttn;
                        if (savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                        {
                            patternsavebttn = savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                            IUIAutomationInvokePattern savebttnpatt = patternsavebttn as IUIAutomationInvokePattern;
                            savebttnpatt.Invoke();

                            //getpricespattern.Invoke();

                        }
                    }

                     Thread.Sleep(2000);
                    //cannot move ahead without saving data 


                }

            }
        }
        catch (Exception ex)
        {
            string msg = ex.Message.ToString();
            Console.WriteLine(ex.Message);
            //Console.WriteLine(ex.StackTrace);
            CreateLogs(msg, logFilePath);
            CreateExceptionReport(ex.StackTrace);
            Console.WriteLine(ex.StackTrace);
            // exitcode = 23;
        }

    }



    public static string ReplaceDateTimePlaceholders(string input)
    {
        string pattern = @"\(([^)]+)\)";
        Regex regex = new Regex(pattern);

        MatchCollection matches = regex.Matches(input);

        foreach (Match match in matches)
        {
            string placeholder = match.Groups[1].Value;
            string replacement = GetFormattedDateTime(placeholder);
            input = input.Replace(match.Value, replacement);
        }

        return input;
    }

    public static void HandleAccountsDataCopy(IUIAutomationElement currwin,string tabname)
    {
        try
        {

            Console.WriteLine("Inside Copy Data From One ACCOUNT TO OTHER");
            IUIAutomation automation = new CUIAutomation8();

            WindowsInput.InputSimulator inputSimulator = new WindowsInput.InputSimulator();

            IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "cmbCopyFromAccount");
            IUIAutomationElement copyfromcmbbox = currwin.FindFirst(TreeScope.TreeScope_Descendants, condition);
            IUIAutomationCondition condsavebttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnSave");
            IUIAutomationElement savebttnelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, condsavebttn);

            var accconfig = TabnametoCopyAccounts[tabname];
            

            foreach (var s in accconfig.Keys)
            {
                SetFilter(currwin, tabname);
                Console.WriteLine(s);
                replayaction(copyfromcmbbox, s);
                object patternobj;
                if (copyfromcmbbox.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) != null)
                {
                    patternobj = copyfromcmbbox.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);

                    IUIAutomationExpandCollapsePattern valuepatt = patternobj as IUIAutomationExpandCollapsePattern;
                    Console.WriteLine(valuepatt.CurrentExpandCollapseState);
                    if(valuepatt.CurrentExpandCollapseState!=ExpandCollapseState.ExpandCollapseState_Collapsed)
                    {
                        valuepatt.Collapse();
                    }

                }
                Thread.Sleep(1000);
                IUIAutomationCondition bttncondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
                IUIAutomationElement bttnofcopyaccounts = copyfromcmbbox.FindFirst(TreeScope.TreeScope_Descendants, bttncondition);

                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(bttnofcopyaccounts.CurrentBoundingRectangle.left + 4, bttnofcopyaccounts.CurrentBoundingRectangle.bottom + 15);
                Thread.Sleep(1000);
                inputSimulator.Mouse.LeftButtonClick();
                Thread.Sleep(5000);

                foreach(string copyto in accconfig[s])
                {
                    Console.WriteLine(copyto);
                    IUIAutomationCondition listitemcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_CheckBoxControlTypeId);

                    IUIAutomationCondition listitemname = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, copyto);

                    IUIAutomationAndCondition andcondforzero = (IUIAutomationAndCondition)automation.CreateAndCondition(listitemcond, listitemname);

                    IUIAutomationElement copytoaccountelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, andcondforzero);
                    if (copytoaccountelement != null)
                    {
                        replayaction(copytoaccountelement, "");
                    }
                    else
                    {
                        Console.WriteLine(copyto + "Not Found");
                    }

                    IUIAutomationCondition bttncondition1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnAccountCopy");
                    IUIAutomationElement bttnofcopyaccounts1 = currwin.FindFirst(TreeScope.TreeScope_Descendants, bttncondition1);

                    replayaction(bttnofcopyaccounts1, "");

                    
                    object patternsavebttn1;
                    if (savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                    {
                        patternsavebttn1 = savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                        IUIAutomationInvokePattern savebttnpatt = patternsavebttn1 as IUIAutomationInvokePattern;
                        savebttnpatt.Invoke();
                    }

                    IUIAutomationCondition condStatusBar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "statusStrip1");
                    IUIAutomationElement statusBar = currwin.FindFirst(TreeScope.TreeScope_Descendants, condStatusBar);

                    if (statusBar != null)
                    {
                        IUIAutomationElementArray childElements = statusBar.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                        IUIAutomationElement childElement = childElements.GetElement(0);
                        while (childElement.CurrentName.Equals("Saving..."))
                            Thread.Sleep(1000);
                        Thread.Sleep(1000);
                    }

                    break;

                }

            }

        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }





    }
    public static void handledvwork(IUIAutomationElement currwin, string tabname,DataTable mainFileData)
    {
        //Console.WriteLine("hello");
        IUIAutomation automation = new CUIAutomation8();

        IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "dtLiveFeed");
        IUIAutomationElement datefield = currwin.FindFirst(TreeScope.TreeScope_Descendants, condition);
        IUIAutomationCondition leftpanedatecondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "dtDateMonth");
        IUIAutomationElement datefield1 = currwin.FindFirst(TreeScope.TreeScope_Descendants, leftpanedatecondition);
        List<string> symbols = new List<string>();
        List<string> removesymbols = new List<string>();
        //if (!copytabsindv.Contains(tabname))
        // {
        IUIAutomationElement datefield11 = null;
        if (datefield != null)
        {
            object patternobj;
            if (datefield.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
            {
                patternobj = datefield.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                IUIAutomationValuePattern valuepatt = patternobj as IUIAutomationValuePattern;
                valuepatt.SetValue(processDateForUI);

            }

            Thread.Sleep(500);
            IUIAutomationCondition condition11 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
             datefield11 = datefield.FindFirst(TreeScope.TreeScope_Descendants, condition11);
            if (datefield11 != null)
            {
                object patternobj1;
                if (datefield11.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                {
                    patternobj1 = datefield11.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                    IUIAutomationInvokePattern valuepatt = patternobj1 as IUIAutomationInvokePattern;
                    valuepatt.Invoke();
                    Thread.Sleep(500);
                    valuepatt.Invoke();

                }
            }
        }
        else
        {
            object patternobj;
            if (datefield1.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
            {
                patternobj = datefield1.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                IUIAutomationValuePattern valuepatt = patternobj as IUIAutomationValuePattern;
                valuepatt.SetValue(processDateForUI);

            }

            Thread.Sleep(500);
            IUIAutomationCondition condition11 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
             datefield11 = datefield1.FindFirst(TreeScope.TreeScope_Descendants, condition11);
            if (datefield11 != null)
            {
                object patternobj1;
                if (datefield11.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                {
                    patternobj1 = datefield11.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                    IUIAutomationInvokePattern valuepatt = patternobj1 as IUIAutomationInvokePattern;
                    valuepatt.Invoke();
                    Thread.Sleep(500);
                    valuepatt.Invoke();

                }
            }
        }
        IUIAutomationCondition conditioncmbbx = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "cmbExchangeGroup");
        IUIAutomationElement cmbboxfield = currwin.FindFirst(TreeScope.TreeScope_Descendants, conditioncmbbx);
        if (cmbboxfield != null)
        {
            IUIAutomationCondition conditionlistitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
            IUIAutomationElementArray listitems = cmbboxfield.FindAll(TreeScope.TreeScope_Descendants, conditionlistitem);
            
                WindowsInput.InputSimulator inputSimulator = new WindowsInput.InputSimulator();

                IUIAutomationElement singlelistitem = listitems.GetElement(0);


                IUIAutomationCondition condbttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
                IUIAutomationElement bttn = cmbboxfield.FindFirst(TreeScope.TreeScope_Descendants, condbttn);

                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(bttn.CurrentBoundingRectangle.right - 5, bttn.CurrentBoundingRectangle.bottom - 10);
                Thread.Sleep(500);
                inputSimulator.Mouse.LeftButtonClick();
                IUIAutomationCondition conditiondataitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, singlelistitem.CurrentName);
                IUIAutomationElement dataitemcmb = currwin.FindFirst(TreeScope.TreeScope_Descendants, conditiondataitem);
                object patternobj2;
                if (dataitemcmb.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                {
                    patternobj2 = dataitemcmb.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                    IUIAutomationScrollItemPattern selectpatt = patternobj2 as IUIAutomationScrollItemPattern;

                    selectpatt.ScrollIntoView();

                }
                //CreateLogs(element.CurrentBoundingRectangle.right - 30);
                //CreateLogs(element.CurrentBoundingRectangle.right);
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(dataitemcmb.CurrentBoundingRectangle.right - 10, dataitemcmb.CurrentBoundingRectangle.bottom - 15);
                Thread.Sleep(500);
                inputSimulator.Mouse.LeftButtonClick();
                Thread.Sleep(5000);
            
        }

        if (TabnametoCopyAccounts.Keys.Contains(tabname))
        {
            Console.WriteLine("Going for account data copy");
            HandleAccountsDataCopy(currwin, tabname);
        }

        if (copyincurrdate.Contains(tabname))
        {
         
                CreateLogs("Inside Copy in Current Date From Process Date", logFilePath);
                CreateLogs("going for copy in" + tabname + "copy from date" + processDateForUI + "copy to date " + yesterdayString, logFilePath);
            
            if (buttonsconfigs.ContainsKey(tabname.ToLower()) || buttonsconfigs.ContainsKey(tabname))
            {
                Setbuttons(currwin, tabname);
            }
            SetFilter(currwin, tabname);
            copytabname = tabname;
            if (!ismonday)
            {
                handlecopywork(currwin, processDateForUI, yesterdayString);
            }
            else
            {
                handlecopywork(currwin, processDateForUI, previous3daystring);
            }

        }

        if (copytabsindv.Contains(tabname))
        {
            SetFilter(currwin, tabname);
            CreateLogs("Copying data From Previous Date In Daily Valuation For  Tab :  " + tabname, logFilePath);
            CreateLogs("Value of isMonday For Copy " + ismonday+tabname, logFilePath);

            if (copytabsindv.Contains(tabname) && targettabsindv.Contains(tabname))
            {
                CreateLogs("Process Date is Not Monday", logFilePath);
                CreateLogs("going for copy in" + tabname + "copy from date" + yesterdayoriginal + "copy to date " + processDateForUI, logFilePath);
                if (buttonsconfigs.ContainsKey(tabname.ToLower()) || buttonsconfigs.ContainsKey(tabname))
                {
                    Setbuttons(currwin, tabname);
                }
                copytabname=tabname;
                handlecopywork(currwin, yesterdayoriginal, processDateForUI);
            }
            else if (ismonday)
            {
                CreateLogs("Process Date is Monday", logFilePath);
                CreateLogs("going for copy in" + tabname + "copy from date" + processDateForUI + "copy to date " + yesterdayString, logFilePath);
                copytabname = tabname;
                handlecopywork(currwin, processDateForUI, yesterdayString);
                CreateLogs("going for copy in" + tabname + "copy from date" + yesterdayString + "copy to date " + dayBeforeYesterdayString, logFilePath);
                handlecopywork(currwin, yesterdayString, dayBeforeYesterdayString);
            }
            else
            {
                copytabname = tabname;
                CreateLogs("Process Date is Not Monday", logFilePath);
                CreateLogs("going for copy in" + tabname + "copy from date" + yesterdayoriginal + "copy to date " + processDateForUI, logFilePath);
                handlecopywork(currwin, yesterdayoriginal, processDateForUI);
            }
        }
        IUIAutomationCondition conditiongrid = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "grdPivotDisplay");
        IUIAutomationElement gridelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, conditiongrid);
        string[] exchangegroupvalues = new string[0];
        string exchangegroup = GetValueFromMainFile(mainFileData, "Exchangegroup");
        string[] exchangegrouplist = exchangegroup.Split(',');
        string targettab = "notfound";
        foreach (string ss in exchangegrouplist)
        {
            string[] configarr = ss.Split('#');

            targettab = "notfound";
            if (configarr.Length == 2)
            {
                targettab = configarr[0];
                exchangegroupvalues = configarr[1].Split('|');
            }
        }
        if (getpricesbttntablist.Contains(tabname.ToLower()) || getpricesbttntablist.Contains(tabname))
        {
            CreateLogs("Inside Get Prices Bttn" + tabname,logFilePath);
            if (buttonsconfigs.ContainsKey(tabname.ToLower()) || buttonsconfigs.ContainsKey(tabname))
            {
                Setbuttons(currwin, tabname);

            }
            bool flag = true;
            if (getpricesbttntablist.Contains("ALL") || getpricesbttntablist.Contains("all"))
            {
                HandleAllMarkPriceExchange(currwin, tabname);
                flag = false;
            }

           
            if (datefield != null)
            {
                object patternobj;
                if (datefield.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                {
                    patternobj = datefield.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                    IUIAutomationValuePattern valuepatt = patternobj as IUIAutomationValuePattern;
                    valuepatt.SetValue(processDateForUI);

                }
            }
            Thread.Sleep(500);

            if (datefield11 != null)
            {
                object patternobj;
                if (datefield11.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                {
                    patternobj = datefield11.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                    IUIAutomationInvokePattern valuepatt = patternobj as IUIAutomationInvokePattern;
                    valuepatt.Invoke();
                    Thread.Sleep(500);
                    valuepatt.Invoke();

                }
            }
            IUIAutomationCondition condgetprice = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnGetLiveFeedData");
            IUIAutomationElement getpriceselement = currwin.FindFirst(TreeScope.TreeScope_Descendants, condgetprice);
            IUIAutomationCondition condsavebttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnSave");
            IUIAutomationElement savebttnelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, condsavebttn);
            if (cmbboxfield != null && targettab.ToLower().Replace(" ","") == tabname.ToLower().Replace(" ", ""))
            {
                
                CreateLogs("Inside Get Prices Bttn" + tabname,logFilePath);
                Console.WriteLine("Inside Get Prices Bttn");
               
                IUIAutomationCondition conditionlistitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
                IUIAutomationElementArray listitems = cmbboxfield.FindAll(TreeScope.TreeScope_Descendants, conditionlistitem);
                for (int i = 0; i < listitems.Length; i++)
                {
                    WindowsInput.InputSimulator inputSimulator = new WindowsInput.InputSimulator();

                    IUIAutomationElement singlelistitem = listitems.GetElement(i);
                    string listnamefromcombobox = singlelistitem.CurrentName;

                    listnamefromcombobox = listnamefromcombobox.Trim();
                    listnamefromcombobox = listnamefromcombobox.Replace(" ", "");
                    listnamefromcombobox = listnamefromcombobox.ToLower();
                    CreateLogs("Listname from combobox for exchange group and get prices button " + listnamefromcombobox, logFilePath);
                    if (exchangegroupvalues.Contains(listnamefromcombobox))
                    {
                        IUIAutomationCondition condbttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
                        IUIAutomationElement bttn = cmbboxfield.FindFirst(TreeScope.TreeScope_Descendants, condbttn);

                        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(bttn.CurrentBoundingRectangle.right - 5, bttn.CurrentBoundingRectangle.bottom - 10);
                        Thread.Sleep(500);
                        inputSimulator.Mouse.LeftButtonClick();
                        IUIAutomationCondition conditiondataitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, singlelistitem.CurrentName);
                        IUIAutomationElement dataitemcmb = currwin.FindFirst(TreeScope.TreeScope_Descendants, conditiondataitem);
                        object patternobj2;
                        if (dataitemcmb.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                        {
                            patternobj2 = dataitemcmb.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                            IUIAutomationScrollItemPattern selectpatt = patternobj2 as IUIAutomationScrollItemPattern;

                            selectpatt.ScrollIntoView();

                        }
                        //CreateLogs(element.CurrentBoundingRectangle.right - 30);
                        //CreateLogs(element.CurrentBoundingRectangle.right);
                        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(dataitemcmb.CurrentBoundingRectangle.right - 10, dataitemcmb.CurrentBoundingRectangle.bottom - 15);
                        Thread.Sleep(500);
                        inputSimulator.Mouse.LeftButtonClick();
                        Thread.Sleep(2000);

                        if (getpricesbttntablist.Contains(listnamefromcombobox))
                        {
                            Console.WriteLine("Get Price Button Trying for " + listnamefromcombobox);
                            if (buttonsconfigs.ContainsKey(tabname.ToLower()) || buttonsconfigs.ContainsKey(tabname))
                            {
                                Setbuttons(currwin, tabname);
                            }
                            flag = false;
                            SetFilter(currwin, tabname);
                            IUIAutomationCondition lastprice = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "optLastPrice");
                            IUIAutomationElement lastpriceelem = currwin.FindFirst(TreeScope.TreeScope_Descendants, lastprice);

                            IUIAutomationCondition previousprice = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "optPreviousPrice");
                            IUIAutomationElement previouspriceelem = currwin.FindFirst(TreeScope.TreeScope_Descendants, previousprice);

                            IUIAutomationCondition lbllprice = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "lblLastDate");
                            IUIAutomationElement lbllpriceelem = currwin.FindFirst(TreeScope.TreeScope_Descendants, lbllprice);

                            IUIAutomationCondition lblpprice = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "lblPreviousDate");
                            IUIAutomationElement lblppriceelem = currwin.FindFirst(TreeScope.TreeScope_Descendants, lblpprice);
                            IUIAutomationCondition condlivebttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "optUseLiveFeed");
                            IUIAutomationElement livebttnelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, condlivebttn);
                            if (livebttnelement != null)
                            {
                                object patterngetprice;
                                if (livebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                                {
                                    patterngetprice = livebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                                    IUIAutomationInvokePattern getpricespattern = patterngetprice as IUIAutomationInvokePattern;
                                    getpricespattern.Invoke();
                                }
                            }
                            Thread.Sleep(1000);
                            //todo: press lastdate radio button 
                            //optUseLiveFeed
                            if (lastpriceelem != null)
                            {
                                object patterngetprice;
                                if (lastpriceelem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                                {
                                    patterngetprice = lastpriceelem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                                    IUIAutomationInvokePattern getpricespattern = patterngetprice as IUIAutomationInvokePattern;
                                    getpricespattern.Invoke();
                                }
                            }
                            //Thread.Sleep(5000);
                            if (lblppriceelem != null && lbllpriceelem != null)
                            {
                                if (lbllpriceelem.CurrentName != "(" + processDateForUI + ")")
                                {
                                    if (previouspriceelem != null)
                                    {
                                        object patterngetprice;
                                        if (previouspriceelem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                                        {
                                            patterngetprice = previouspriceelem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                                            IUIAutomationInvokePattern getpricespattern = patterngetprice as IUIAutomationInvokePattern;
                                            getpricespattern.Invoke();
                                            
                                            CreateLogs("changed radio button to " + previouspriceelem.CurrentName + "tabname" + tabname, logFilePath);

                                        }
                                    }
                                }
                                else
                                {
                                    CreateLogs("changed radio button to " + lastpriceelem.CurrentName + "tabname" + tabname, logFilePath);
                                }
                            }
                            Thread.Sleep(1000);
                            bool insidezerofilter = true;
                            if (SetZeroFilter.Contains(singlelistitem.CurrentName.Replace(" ", "").ToLower()))
                            {
                                insidezerofilter = false;
                                CreateLogs("Going to set zero filetr", logFilePath);
                                Thread.Sleep(2000);
                                setzerofilter(currwin);
                            }

                            if (buttonsconfigs.ContainsKey(tabname.ToLower()) || buttonsconfigs.ContainsKey(tabname) || buttonsconfigs.ContainsKey(singlelistitem.CurrentName.Replace(" ", "").ToLower())|| buttonsconfigs.ContainsKey(listnamefromcombobox))
                            {
                                Console.WriteLine("Inside set buttons get pricess buttons");
                                Setbuttons(currwin, tabname);
                                Setbuttons(currwin, singlelistitem.CurrentName.Replace(" ", "").ToLower());

                            }
                            if (getpriceselement.CurrentIsEnabled == 1 && (insidezerofilter || foundzero))
                            {
                                object patterngetprice;
                                if (getpriceselement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                                {
                                    patterngetprice = getpriceselement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                                    IUIAutomationInvokePattern getpricespattern = patterngetprice as IUIAutomationInvokePattern;

                                    for (int x = 0; x < 2; x++)
                                    {
                                        getpricespattern.Invoke();
                                        CreateLogs("Get prices clicked for " + tabname, logFilePath);
                                        Thread.Sleep(1000);
                                    }

                                }
                            }

                            object patternsavebttn1;
                            if (savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                            {
                                patternsavebttn1 = savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                                IUIAutomationInvokePattern savebttnpatt = patternsavebttn1 as IUIAutomationInvokePattern;
                                savebttnpatt.Invoke();
                                CreateLogs("Save button clicked for tabname  " + tabname, logFilePath);


                            }


                        }

                    }
                }
            }
            if (flag)
            {
                if (buttonsconfigs.ContainsKey(tabname.ToLower()) || buttonsconfigs.ContainsKey(tabname))
                {
                    Setbuttons(currwin, tabname);
                }
                SetFilter(currwin, tabname);

                bool insidezerofilter = true;
                if (SetZeroFilter.Contains(tabname))
                {
                    insidezerofilter = false;
                    CreateLogs("Going to set zero filter", logFilePath);
                    Thread.Sleep(2000);
                    setzerofilter(currwin);
                }

                if (getpriceselement.CurrentIsEnabled == 1 &&  (insidezerofilter || foundzero))
                {
                    object patterngetprice;
                    if (getpriceselement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                    {
                        patterngetprice = getpriceselement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                        IUIAutomationInvokePattern getpricespattern = patterngetprice as IUIAutomationInvokePattern;

                        for (int x = 0; x < 2; x++)
                        {
                            getpricespattern.Invoke();
                            CreateLogs("Get prices clicked for " + tabname, logFilePath);
                            Thread.Sleep(1000);
                        }

                    }
                }

                object patternsavebttn;
                if (savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                {
                    patternsavebttn = savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                    IUIAutomationInvokePattern savebttnpatt = patternsavebttn as IUIAutomationInvokePattern;
                    savebttnpatt.Invoke();
                    CreateLogs("Save button clicked for tabname  " + tabname, logFilePath);


                }
            }

        }

        if (importtabs.Contains(tabname))
        {
            
            //if (!copytabsindv.Contains(tabname))
            // {
            if (datefield != null)
            {
                object patternobj;
                if (datefield.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                {
                    patternobj = datefield.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                    IUIAutomationValuePattern valuepatt = patternobj as IUIAutomationValuePattern;
                    valuepatt.SetValue(processDateForUI);

                }
            }
            Thread.Sleep(500);
            
            if (datefield11 != null)
            {
                object patternobj;
                if (datefield11.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                {
                    patternobj = datefield11.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                    IUIAutomationInvokePattern valuepatt = patternobj as IUIAutomationInvokePattern;
                    valuepatt.Invoke();
                    Thread.Sleep(500);
                    valuepatt.Invoke();

                }
            }
            Console.WriteLine("Going to Import From File ");
            if (importtabspathdict.ContainsKey(tabname))
            {
                string filepath = importtabspathdict[tabname];
                filepath= ReplaceDateTimePlaceholders(filepath);
                if (File.Exists(filepath))
                {
                    Handle_File_Import(currwin, filepath);
                }
                else
                {
                    Console.WriteLine("Mark Price From File Not imported File not available : " + filepath);
                }
            }
        }

        

        //if exchange group available in config set exchange group for this particular tab
        //
        
            if (targettabsindv.Contains(tabname) && !copytabsindv.Contains(tabname) )
            {

                //if (!copytabsindv.Contains(tabname))
                // {
                if (datefield != null)
                {
                    object patternobj;
                    if (datefield.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                    {
                        patternobj = datefield.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                        IUIAutomationValuePattern valuepatt = patternobj as IUIAutomationValuePattern;
                        valuepatt.SetValue(processDateForUI);

                    }
                }
                Thread.Sleep(500);

                if (datefield11 != null)
                {
                    object patternobj;
                    if (datefield11.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                    {
                        patternobj = datefield11.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                        IUIAutomationInvokePattern valuepatt = patternobj as IUIAutomationInvokePattern;
                        valuepatt.Invoke();
                        Thread.Sleep(500);
                        valuepatt.Invoke();

                    }
                }
                if (cmbboxfield != null && targettab.ToLower() == tabname.ToLower())
                {
                    IUIAutomationCondition conditionlistitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
                    IUIAutomationElementArray listitems = cmbboxfield.FindAll(TreeScope.TreeScope_Descendants, conditionlistitem);
                    for (int i = 0; i < listitems.Length; i++)
                    {
                        WindowsInput.InputSimulator inputSimulator = new WindowsInput.InputSimulator();

                        IUIAutomationElement singlelistitem = listitems.GetElement(i);
                        //object patternobj1;
                        //if (cmbboxfield.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) != null)
                        //{
                        //    patternobj1 = cmbboxfield.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);

                        //    IUIAutomationExpandCollapsePattern selectpatt = patternobj1 as IUIAutomationExpandCollapsePattern;
                        //    // selectpatt.SetValue(singlelistitem.CurrentName);
                        //    //selectpatt.CurrentIsSelected = true;
                        //    selectpatt.Expand();
                        //}
                        string listnamefromcombobox = singlelistitem.CurrentName;

                        listnamefromcombobox = listnamefromcombobox.Trim();
                        listnamefromcombobox = listnamefromcombobox.Replace(" ", "");
                        listnamefromcombobox = listnamefromcombobox.ToLower();
                    CreateLogs("Listname from combobox for exchange group and get prices button " + listnamefromcombobox, logFilePath);
                        if (exchangegroupvalues.Contains(listnamefromcombobox))
                        {
                            IUIAutomationCondition condbttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
                            IUIAutomationElement bttn = cmbboxfield.FindFirst(TreeScope.TreeScope_Descendants, condbttn);

                            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(bttn.CurrentBoundingRectangle.right - 5, bttn.CurrentBoundingRectangle.bottom - 10);
                            Thread.Sleep(500);
                            inputSimulator.Mouse.LeftButtonClick();
                            IUIAutomationCondition conditiondataitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, singlelistitem.CurrentName);
                            IUIAutomationElement dataitemcmb = currwin.FindFirst(TreeScope.TreeScope_Descendants, conditiondataitem);
                            object patternobj2;
                            if (dataitemcmb.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                            {
                                patternobj2 = dataitemcmb.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                IUIAutomationScrollItemPattern selectpatt = patternobj2 as IUIAutomationScrollItemPattern;

                                selectpatt.ScrollIntoView();

                            }
                            //CreateLogs(element.CurrentBoundingRectangle.right - 30);
                            //CreateLogs(element.CurrentBoundingRectangle.right);
                            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(dataitemcmb.CurrentBoundingRectangle.right - 10, dataitemcmb.CurrentBoundingRectangle.bottom - 15);
                            Thread.Sleep(500);
                            inputSimulator.Mouse.LeftButtonClick();
                        Thread.Sleep(2000);
                        if (buttonsconfigs.ContainsKey(tabname.ToLower()) || buttonsconfigs.ContainsKey(tabname))
                        {
                            Setbuttons(currwin, tabname);
                        }
                        SetFilter(currwin, tabname);
                        if (getpricesbttntablist.Contains(listnamefromcombobox))
                        {
                            Console.WriteLine("Inside Get Prices Bttn");
                            if (buttonsconfigs.ContainsKey(tabname.ToLower()) || buttonsconfigs.ContainsKey(tabname))
                            {
                                Setbuttons(currwin, tabname);
                            }
                            SetFilter(currwin, tabname);
                            CreateLogs("Going to press Get prices Button For " + listnamefromcombobox, logFilePath);
                            IUIAutomationCondition condgetprice = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnGetLiveFeedData");
                            IUIAutomationElement getpriceselement = currwin.FindFirst(TreeScope.TreeScope_Descendants, condgetprice);
                            IUIAutomationCondition condsavebttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnSave");
                            IUIAutomationElement savebttnelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, condsavebttn);
                            if (getpriceselement.CurrentIsEnabled == 1)
                            {
                                object patterngetprice;
                                if (getpriceselement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                                {
                                    patterngetprice = getpriceselement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                                    IUIAutomationInvokePattern getpricespattern = patterngetprice as IUIAutomationInvokePattern;
                                    
                                    for (int x = 0; x < 2; x++)
                                    {
                                        getpricespattern.Invoke();
                                        CreateLogs("Get prices clicked for " + tabname, logFilePath);
                                        Thread.Sleep(1000);
                                    }

                                }
                            }

                            object patternsavebttn;
                            if (savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                            {
                                patternsavebttn = savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                                IUIAutomationInvokePattern savebttnpatt = patternsavebttn as IUIAutomationInvokePattern;
                                savebttnpatt.Invoke();
                                CreateLogs("Save button clicked for tabname  " + tabname, logFilePath);
                            }
                        }

                        CreateLogs("Zero Filter Check Condition " + singlelistitem.CurrentName.Replace(" ", "").ToLower() , logFilePath);
                        if (SetZeroFilter.Contains(singlelistitem.CurrentName.Replace(" ", "").ToLower()))
                        {
                            CreateLogs("Going to set zero filetr", logFilePath);
                            Thread.Sleep(2000);
                            setzerofilter(currwin);
                        }
                        Thread.Sleep(5000);
                        CreateLogs("Going to Fetch symbols from Grid", logFilePath);
                        SetFilter(currwin, tabname);
                        symbols = getsymbolsfromgrid(gridelement, singlelistitem.CurrentName);

                            foreach (string sym in symbols)
                            {
                                if (manualpriceentrydict.ContainsKey(sym))
                                {
                                if (!symboltopricefromyahoo.ContainsKey(sym))
                                {
                                    symboltopricefromyahoo.Add(sym, manualpriceentrydict[sym]);
                                }
                                   removesymbols.Add(sym);

                                }
                                if (enterprisemapping.ContainsKey(sym))
                                {
                                    if (manualpriceentrydict.ContainsKey(enterprisemapping[sym]))
                                    {
                                    if (!symboltopricefromyahoo.ContainsKey(enterprisemapping[sym]))
                                    {
                                        symboltopricefromyahoo.Add(enterprisemapping[sym], manualpriceentrydict[enterprisemapping[sym]]);
                                    }
                                    removesymbols.Add(enterprisemapping[sym]);
                                    }
                                }
                            }
                            foreach (string sym in symbols)
                            {
                                if (sym != " " && factsetsymbollistglobal.Contains(sym))
                                {
                                    symbolstofetchfromfactset.Add(sym);
                                    if (enterprisemapping.ContainsKey(sym))
                                    {
                                    removesymbols.Add(enterprisemapping[sym]);
                                    }
                                    else
                                    {
                                        removesymbols.Add(sym);
                                    }
                                }
                            }
                            if(removesymbols.Count > 0)
                        {
                            foreach(string symboltoremove in removesymbols)
                            {
                                Console.WriteLine("Removing symbol from price fetching list from yahoo " + symboltoremove);
                                symbols.Remove(symboltoremove);
                            }
                        }
                            if (symbolstofetchfromfactset.Count > 0)
                            {

                                foreach (string symbol in symbolstofetchfromfactset)
                                {
                                    string orgsymbol = symbol;
                                    bool flag = true;
                                    if (replaceMapping.ContainsKey(symbol))
                                    {
                                        orgsymbol = replaceMapping[symbol];
                                    if (!originalmapping.ContainsKey(symbol))
                                    {
                                        originalmapping.Add(symbol, replaceMapping[symbol]);
                                    }
                                    if (!yahootoenterprisemapping.ContainsKey(replaceMapping[symbol]))
                                    {
                                        yahootoenterprisemapping.Add(replaceMapping[symbol], symbol);
                                    }

                                        flag = false;
                                    }
                                    else
                                    {

                                        foreach (string s in _exchangeMapping.Keys)
                                        {
                                            if (symbol.Contains(s))
                                            {
                                                string orgsym = symbol;
                                            if (!originalmapping.ContainsKey(orgsym))
                                            {
                                                originalmapping.Add(orgsym, symbol.Replace(s, _exchangeMapping[s]));
                                            }
                                            if (!yahootoenterprisemapping.ContainsKey(symbol.Replace(s, _exchangeMapping[s])))
                                            {
                                                yahootoenterprisemapping.Add(symbol.Replace(s, _exchangeMapping[s]), orgsym);
                                            }
                                                orgsymbol = symbol.Replace(s, _exchangeMapping[s]);
                                                flag = false;
                                            }
                                            else
                                                continue;
                                        }
                                    }
                                    if (symbol.Contains('$'))
                                    {
                                    if (!originalmapping.ContainsKey(symbol))
                                    {
                                        originalmapping.Add(symbol, symbol.Replace('$', '^'));
                                    }
                                    if (!yahootoenterprisemapping.ContainsKey(symbol.Replace('$', '^')))
                                    {
                                        yahootoenterprisemapping.Add(symbol.Replace('$', '^'), symbol);
                                    }
                                        string new_symbol = symbol.Replace('$', '^');

                                        orgsymbol = new_symbol;
                                        flag = false;
                                    }
                                    if (flag)
                                    {
                                        orgsymbol = symbol;
                                    }
                                    FactSetDataFeeder fs = new FactSetDataFeeder();
                                    //FactSetDataFeeder.conn
                                    FactSetDataFeeder.Connect();
                                    FactSetDataFeeder.GetSnapShotData(orgsymbol);
                                    Thread.Sleep(5000);
                                    string price = FactSetDataFeeder.midcloseprice;
                                    if (price != "")
                                    {
                                    if(!symboltopricefromyahoo.ContainsKey(orgsymbol))
                                        symboltopricefromyahoo.Add(orgsymbol, price);
                                        Console.WriteLine("Price From Factset for symbol : " + symbol + FactSetDataFeeder.midcloseprice);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Not Found On Factset for symbol : " + symbol);
                                    }

                                    FactSetDataFeeder.Disconnect();
                                }

                            }
                            if (symbols.Count != 0)
                            {

                                if (singlelistitem.CurrentName.ToLower().Contains("option"))
                                {
                                CreateLogs("Going to fetch prices from factset for tabname " + singlelistitem.CurrentName.ToLower(), logFilePath);
                                    // symbols.Add("AAPL:D");
                                    //symbols.Add("EG#A1725C390000:D");
                                    foreach (string symbol in symbols)
                                    {
                                        FactSetDataFeeder fs = new FactSetDataFeeder();
                                        //FactSetDataFeeder.conn
                                        FactSetDataFeeder.Connect();
                                        FactSetDataFeeder.GetSnapShotData(symbol);
                                        Thread.Sleep(5000);
                                        string price = FactSetDataFeeder.midcloseprice;
                                        if (price != "")
                                        {
                                        if(!symboltopricefromyahoo.ContainsKey(symbol))
                                            symboltopricefromyahoo.Add(symbol, price);
                                            Console.WriteLine("Price From Factset for symbol : " + symbol + FactSetDataFeeder.midcloseprice);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Not Found On Factset for symbol : " + symbol);
                                        }

                                        FactSetDataFeeder.Disconnect();
                                    }

                                }
                                else
                                {
                                    getpricesfromyahoo(symbols);
                                }

                            
                        }
                        if (symboltopricefromyahoo.Count > 0)
                        {
                            filldatainenterprise(gridelement, tabname);
                        }

                    }

                    }

                }
                else
                {
                CreateLogs("Exchnage Group Not Selected Tabname is  :" + tabname, logFilePath);
               
                SetFilter(currwin, tabname);

                if (SetZeroFilter.Contains(tabname.Replace(" ","").ToLower()))
                {
                    CreateLogs("Going to set Zero Filter " + tabname, logFilePath);

                    Thread.Sleep(2000);
                    setzerofilter(currwin);
                }
                SetFilter(currwin, tabname);
                symbols = getsymbolsfromgrid(gridelement, tabname);
                foreach (string sym in symbols)
                {
                    if (manualpriceentrydict.ContainsKey(sym))
                    {
                        if (!symboltopricefromyahoo.ContainsKey(sym)) {
                            symboltopricefromyahoo.Add(sym, manualpriceentrydict[sym]);
                        }
                        removesymbols.Add(sym);

                    }
                    if (enterprisemapping.ContainsKey(sym))
                    {
                        if (manualpriceentrydict.ContainsKey(enterprisemapping[sym]))
                        {
                            if(!symboltopricefromyahoo.ContainsKey(enterprisemapping[sym]))
                            symboltopricefromyahoo.Add(enterprisemapping[sym], manualpriceentrydict[enterprisemapping[sym]]);
                            removesymbols.Add(enterprisemapping[sym]);
                        }
                    }
                }
                if (removesymbols.Count > 0)
                {
                    foreach (string symboltoremove in removesymbols)
                    {
                        Console.WriteLine("Removing symbol from price fetching list from yahoo " + symboltoremove);
                        symbols.Remove(symboltoremove);
                    }
                }
                if (symbols.Count != 0)
                    {
                        getpricesfromyahoo(symbols);
                        
                    }
                if (symboltopricefromyahoo.Count > 0)
                {
                    filldatainenterprise(gridelement, tabname);
                }

                }
            }
        
    }
    public static void setzerofilter(IUIAutomationElement currwin)
    {
        IUIAutomation automation = new CUIAutomation8();
        InputSimulator inputSimulator = new InputSimulator();
        IUIAutomationCondition conditiongrid = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "grdPivotDisplay");
        IUIAutomationElement gridelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, conditiongrid);

        IUIAutomationCondition conddataitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
        IUIAutomationElementArray dataitemelement = gridelement.FindAll(TreeScope.TreeScope_Descendants, conddataitem);
        IUIAutomationCondition condheaderitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderItemControlTypeId);
        IUIAutomationElementArray headeritemarray = gridelement.FindAll(TreeScope.TreeScope_Descendants, condheaderitem);

        IUIAutomationCondition conddailybttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "optDaily");
        IUIAutomationElement dailybttnelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, conddailybttn);

        if (dataitemelement.Length >= 1)
        {
            
            IUIAutomationElement lastheaderelement = headeritemarray.GetElement(headeritemarray.Length - 1);
            Console.WriteLine(lastheaderelement.CurrentName);
            Console.WriteLine(lastheaderelement.CurrentAutomationId);

            if (dailybttnelement != null)
            {
                object copydateobj;
                if (dailybttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                {
                    copydateobj = dailybttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                    IUIAutomationInvokePattern valuepatt = copydateobj as IUIAutomationInvokePattern;
                    valuepatt.Invoke();
                }
            }
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(lastheaderelement.CurrentBoundingRectangle.right - 16, lastheaderelement.CurrentBoundingRectangle.bottom - 10);
            Thread.Sleep(500);
            inputSimulator.Mouse.LeftButtonClick();
            Thread.Sleep(2000);
            IUIAutomationCondition listitemcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);

            IUIAutomationCondition listitemname = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "0");
            IUIAutomationAndCondition andcondforzero = (IUIAutomationAndCondition)automation.CreateAndCondition(listitemcond, listitemname);
            IUIAutomationElement zeroelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, andcondforzero);
            foundzero = false;
            if (zeroelement != null && automation.ContentViewWalker.GetParentElement(zeroelement).CurrentAutomationId=="ValueListDropDown")
            {

                foundzero = true;
                if (zeroelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                {
                    object scrollitemobj;
                    scrollitemobj = zeroelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                    IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                    selectionpatternprovider.ScrollIntoView();

                }
                //Console.WriteLine(zeroelement.GetCachedParent().CurrentName);
                //Console.WriteLine(zeroelement.GetCachedParent().CurrentAutomationId);
                Console.WriteLine(automation.ContentViewWalker.GetParentElement(zeroelement).CurrentAutomationId);
                Console.WriteLine(automation.ContentViewWalker.GetParentElement(zeroelement).CurrentName);
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(zeroelement.CurrentBoundingRectangle.right - 10, zeroelement.CurrentBoundingRectangle.bottom - 15);
                Thread.Sleep(500);
                inputSimulator.Mouse.LeftButtonClick();
            }
            else
            {
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(lastheaderelement.CurrentBoundingRectangle.right - 16, lastheaderelement.CurrentBoundingRectangle.bottom - 10);
                Thread.Sleep(500);
                inputSimulator.Mouse.LeftButtonClick();
                Thread.Sleep(2000);
            }

        }
    }
    //static Dictionary<string, string> filterconfig = new Dictionary<string, string>();
    static bool foundzero = false;
    public static void HandleAllMarkPriceExchange(IUIAutomationElement currwin, string tabname)
    {
        Console.WriteLine("Inside handle all markprice exchange " + tabname);
        IUIAutomation automation = new CUIAutomation8();
        //InputSimulator inputSimulator = new InputSimulator();
        IUIAutomationCondition conditioncmbbx = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "cmbExchangeGroup");
        IUIAutomationElement cmbboxfield = currwin.FindFirst(TreeScope.TreeScope_Descendants, conditioncmbbx);
        IUIAutomationCondition conditiongrid = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "grdPivotDisplay");
        IUIAutomationElement gridelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, conditiongrid);
        SetFilter(currwin, tabname);
        if (cmbboxfield != null)
        {
            IUIAutomationCondition conditionlistitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
            IUIAutomationElementArray listitems = cmbboxfield.FindAll(TreeScope.TreeScope_Descendants, conditionlistitem);
            for (int i = 0; i < listitems.Length; i++)
            {
                
                WindowsInput.InputSimulator inputSimulator = new WindowsInput.InputSimulator();

                IUIAutomationElement singlelistitem = listitems.GetElement(i);
               
                IUIAutomationCondition condbttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
                IUIAutomationElement bttn = cmbboxfield.FindFirst(TreeScope.TreeScope_Descendants, condbttn);

                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(bttn.CurrentBoundingRectangle.right - 5, bttn.CurrentBoundingRectangle.bottom - 10);
                Thread.Sleep(500);
                inputSimulator.Mouse.LeftButtonClick();
                IUIAutomationCondition conditiondataitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, singlelistitem.CurrentName);
                IUIAutomationElement dataitemcmb = currwin.FindFirst(TreeScope.TreeScope_Descendants, conditiondataitem);
                object patternobj2;
                if (dataitemcmb.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                {
                    patternobj2 = dataitemcmb.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                    IUIAutomationScrollItemPattern selectpatt = patternobj2 as IUIAutomationScrollItemPattern;

                    selectpatt.ScrollIntoView();

                }
                //CreateLogs(element.CurrentBoundingRectangle.right - 30);
                //CreateLogs(element.CurrentBoundingRectangle.right);
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(dataitemcmb.CurrentBoundingRectangle.right - 10, dataitemcmb.CurrentBoundingRectangle.bottom - 15);
                Thread.Sleep(500);
                inputSimulator.Mouse.LeftButtonClick();
                
                //CreateLogs(singlelistitem.CurrentName + "----------------------" + dataitemelement.Length);
                IUIAutomationCondition conddataitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
                IUIAutomationElementArray dataitemelement = gridelement.FindAll(TreeScope.TreeScope_Descendants, conddataitem);

                // Thread.Sleep(2000);
                if (dataitemelement.Length >= 1)
                {
                    SetFilter(currwin, tabname);
                    if (SetZeroFilter.Contains("ALL") || SetZeroFilter.Contains("all"))
                    {
                        CreateLogs("Going to set zero filetr", logFilePath);
                        Thread.Sleep(1000);
                        setzerofilter(currwin);
                    }
                    Console.WriteLine("Found Zero varible " + foundzero);

                    if (!foundzero && (SetZeroFilter.Contains("ALL") || SetZeroFilter.Contains("all")))
                    {
                        Console.WriteLine("INSIDE NOT FOUND ZERO");
                        continue;
                    }
                    Thread.Sleep(2000);
                    Console.WriteLine("FOUND DATA ROWS FOR  " + singlelistitem.CurrentName);
                    IUIAutomationCondition lastprice = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "optLastPrice");
                    IUIAutomationElement lastpriceelem = currwin.FindFirst(TreeScope.TreeScope_Descendants, lastprice);

                    IUIAutomationCondition previousprice = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "optPreviousPrice");
                    IUIAutomationElement previouspriceelem = currwin.FindFirst(TreeScope.TreeScope_Descendants, previousprice);

                    IUIAutomationCondition lbllprice = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "lblLastDate");
                    IUIAutomationElement lbllpriceelem = currwin.FindFirst(TreeScope.TreeScope_Descendants, lbllprice);

                    IUIAutomationCondition lblpprice = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "lblPreviousDate");
                    IUIAutomationElement lblppriceelem = currwin.FindFirst(TreeScope.TreeScope_Descendants, lblpprice);

                    IUIAutomationCondition selectedfeeddatecondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "lblSelectedFeedDate");
                    IUIAutomationElement selectedfeeddateelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, selectedfeeddatecondition);

                    IUIAutomationCondition condgetprice = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnGetLiveFeedData");
                    IUIAutomationElement getpriceselement = currwin.FindFirst(TreeScope.TreeScope_Descendants, condgetprice);

                    IUIAutomationCondition condsavebttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnSave");
                    IUIAutomationElement savebttnelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, condsavebttn);

                    IUIAutomationCondition condlivebttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "optUseLiveFeed");
                    IUIAutomationElement livebttnelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, condlivebttn);
                    if (livebttnelement != null)
                    {
                        object patterngetprice;
                        if (livebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                        {
                            patterngetprice = livebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                            IUIAutomationInvokePattern getpricespattern = patterngetprice as IUIAutomationInvokePattern;
                            getpricespattern.Invoke();
                        }
                    }
                    Thread.Sleep(1000);
                    //todo: press lastdate radio button 
                    //optUseLiveFeed
                    if (lastpriceelem != null)
                    {
                        CreateLogs("Changed to Last Price for tab " + singlelistitem.CurrentName, logFilePath);
                        object patterngetprice;
                        if (lastpriceelem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                        {
                            patterngetprice = lastpriceelem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                            IUIAutomationInvokePattern getpricespattern = patterngetprice as IUIAutomationInvokePattern;
                            getpricespattern.Invoke();
                        }
                    }
                    //Thread.Sleep(5000);
                    //if buttons config selectedfeed price 
                    // change button based on date of selectedfeed date 

                   
                    if (lblppriceelem != null && lbllpriceelem != null)
                    {
                        if (lbllpriceelem.CurrentName != "(" + processDateForUI + ")")
                        {
                            if (previouspriceelem != null)
                            {
                                object patterngetprice;
                                if (previouspriceelem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                                {
                                    patterngetprice = previouspriceelem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                                    IUIAutomationInvokePattern getpricespattern = patterngetprice as IUIAutomationInvokePattern;
                                    getpricespattern.Invoke();
                                    // Thread.Sleep(2000);
                                    //getpricespattern.Invoke();
                                    CreateLogs("changed radio button to " + previouspriceelem.CurrentName + "tabname" + tabname, logFilePath);

                                }
                            }
                        }
                        else
                        {
                            CreateLogs("changed radio button to " + lastpriceelem + "tabname" + tabname, logFilePath);
                        }
                    }
                    Console.WriteLine("Checking Buttons Config for tabname : " + tabname);
                    if (buttonsconfigs.ContainsKey(tabname.ToLower()) || buttonsconfigs.ContainsKey(tabname))
                    {
                        Setbuttons(currwin, tabname);
                    }
                    if (buttonsconfigs.ContainsKey(tabname))
                    {
                        if (buttonsconfigs[tabname].ContainsKey("optSelectedFeedPrice"))
                        {
                            Console.WriteLine("Inside Setting Close Button AFter Selected feed price");
                            if (selectedfeeddateelement.CurrentName != "(" + processDateForUI + ")")
                            {
                                object patterngetprice;
                                if (previouspriceelem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                                {
                                    patterngetprice = previouspriceelem.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                                    IUIAutomationInvokePattern getpricespattern = patterngetprice as IUIAutomationInvokePattern;
                                    getpricespattern.Invoke();
                                    // Thread.Sleep(2000);
                                    //getpricespattern.Invoke();
                                    CreateLogs("changed radio button to " + previouspriceelem.CurrentName + "tabname" + tabname, logFilePath);

                                }
                            }
                        }
                    }
                    Thread.Sleep(2000);
                    if (getpriceselement.CurrentIsEnabled == 1 && foundzero)
                    {
                        Console.WriteLine("Inside Get Button");
                        object patterngetprice;
                        if (getpriceselement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                        {
                            patterngetprice = getpriceselement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                            IUIAutomationInvokePattern getpricespattern = patterngetprice as IUIAutomationInvokePattern;
                            
                            for (int x = 0; x < 3; x++)
                            {
                                getpricespattern.Invoke();
                                CreateLogs("Get prices clicked for " + tabname, logFilePath);
                                Thread.Sleep(1000);
                            }

                        }
                    }
                    else
                    {
                        CreateLogs("Get button not enabled for " + singlelistitem.CurrentName, logFilePath);
                    }

                    object patternsavebttn;
                    if (savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                    {
                        patternsavebttn = savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                        IUIAutomationInvokePattern savebttnpatt = patternsavebttn as IUIAutomationInvokePattern;
                        savebttnpatt.Invoke();
                        CreateLogs("Save button clicked for tabname  " + tabname, logFilePath);
                    }
                    Thread.Sleep(1000);

                }
                else
                {
                    CreateLogs("Rows not found for tabname " + singlelistitem.CurrentName + "  " + tabname, logFilePath);
                }
            }
            // after processing copy data from previous date to current date 
            // get previous date from current date 
            //getpricesclick 
            //save click 
        }

    }
    public static void Setbuttons(IUIAutomationElement currwin, string tabname)
    {
        //based on config change current window buttons
        //
        try
        {
            Console.WriteLine("Inside Set Buttons");
            Console.WriteLine(tabname);
            IUIAutomation automation = new CUIAutomation8();
            if (buttonsconfigs.ContainsKey(tabname) || buttonsconfigs.ContainsKey(tabname.ToLower()))
            {
                foreach (var preconfig in buttonsconfigs[tabname])
                {
                    var autoid = preconfig.Key;
                    var value = preconfig.Value;
                    Console.WriteLine("Changing value of buttons " + autoid);
                    IUIAutomationCondition conditiongrid = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, autoid);
                    IUIAutomationElement gridelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, conditiongrid);
                    if (gridelement.CurrentControlType == UIA_ControlTypeIds.UIA_ComboBoxControlTypeId)
                    {
                        Console.WriteLine("inside combobox control type");
                        Console.WriteLine("value :" + value);
                        _globalcurrentwin = currwin;
                        replayaction(gridelement, value);
                    }
                    else if (gridelement != null)
                    {
                        object patterngetprice;
                        if (gridelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                        {
                            patterngetprice = gridelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                            IUIAutomationInvokePattern getpricespattern = patterngetprice as IUIAutomationInvokePattern;
                            getpricespattern.Invoke();
                        }
                    }
                    // replayaction(gridelement, "");
                }
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(ex.Message);
        }
    }
        public static void SetFilter(IUIAutomationElement currwin,string tabname)
    {
        if(!result.ContainsKey(tabname)){
            return;
        }
        IUIAutomation automation = new CUIAutomation8();
        InputSimulator inputSimulator = new InputSimulator();
        try
        {

            IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "WARNING");
            IUIAutomationCondition conddialogbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "CONFIRMATION");
            IUIAutomationOrCondition newandcond = (IUIAutomationOrCondition)automation.CreateOrCondition(conddialogbox, conddialogbox1);

            IUIAutomationElement dialogbox = currwin.FindFirst(TreeScope.TreeScope_Descendants, newandcond);

            if (dialogbox != null)
            {
                IUIAutomationCondition okbtncond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                var btn1 = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, okbtncond1);
                replayaction(btn1, "");
            }
            IUIAutomationCondition okbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

            var btn = currwin.FindFirst(TreeScope.TreeScope_Descendants, okbtncond);
            if (btn != null)
            {
                replayaction(btn, "");
            }



        }
        catch (Exception ex)
        {
            CreateExceptionReport(ex.StackTrace.ToString());
        }
        Console.WriteLine("Inside set fILTER");
        Console.WriteLine("Tabname : " + tabname);
        //IUIAutomation automation = new CUIAutomation8();
       // InputSimulator inputSimulator = new InputSimulator();
        IUIAutomationCondition conditiongrid = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "grdPivotDisplay");
        IUIAutomationElement gridelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, conditiongrid);
        int j = 0;
        if(gridelement==null && j < 20)
        {
            gridelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, conditiongrid);
            Thread.Sleep(1000);
            j++;
        }

        //IUIAutomationCondition conddataitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
        //IUIAutomationElementArray dataitemelement = gridelement.FindAll(TreeScope.TreeScope_Descendants, conddataitem);
        IUIAutomationCondition condheaderitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderItemControlTypeId);
        IUIAutomationElementArray headeritemarray = gridelement.FindAll(TreeScope.TreeScope_Descendants, condheaderitem);
        IUIAutomationCondition conddailybttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "optDaily");
        IUIAutomationElement dailybttnelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, conddailybttn);
        //go in all header elements if tabname and header is in config then change the filter 
        //if (dataitemelement.Length >= 1)
        //{
            for (int i = 0; i < headeritemarray.Length; i++)
            {
                IUIAutomationElement lastheaderelement = headeritemarray.GetElement(i);
                Console.WriteLine(lastheaderelement.CurrentName);
                Console.WriteLine(lastheaderelement.CurrentAutomationId);
                foreach(string s in result.Keys)
                {
                    Console.WriteLine(s);
                    foreach(var d in result[s])
                    {
                        Console.WriteLine(d.Key);
                        Console.WriteLine(d.Value);
                    }
                }
                if (result.Count > 0 && result[tabname].ContainsKey(lastheaderelement.CurrentName.ToLower().Trim()))
                {
                    CreateLogs("Inside Setting Filter : Name - " + lastheaderelement.CurrentName + " Auto Id- " + lastheaderelement.CurrentAutomationId, logFilePath);

                    Console.WriteLine(lastheaderelement.CurrentName);
                    Console.WriteLine(lastheaderelement.CurrentAutomationId);
                    //if 
                    if (dailybttnelement != null)
                    {
                        object copydateobj;
                        if (dailybttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                        {
                            copydateobj = dailybttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

                            IUIAutomationInvokePattern valuepatt = copydateobj as IUIAutomationInvokePattern;
                            valuepatt.Invoke();
                        }
                    }
                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(lastheaderelement.CurrentBoundingRectangle.right - 16, lastheaderelement.CurrentBoundingRectangle.bottom - 10);
                    Thread.Sleep(500);
                    inputSimulator.Mouse.LeftButtonClick();
                    Thread.Sleep(2000);
                    IUIAutomationCondition listitemcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
                    //get header filter from config 
                    string filtervalue = result[tabname][lastheaderelement.CurrentName.ToLower().Trim()];
                    IUIAutomationCondition listitemname = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, filtervalue);
                    IUIAutomationAndCondition andcondforzero = (IUIAutomationAndCondition)automation.CreateAndCondition(listitemcond, listitemname);
                    IUIAutomationElement zeroelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, andcondforzero);
                if (zeroelement != null)
                {
                    if (zeroelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                    {
                        object scrollitemobj;
                        scrollitemobj = zeroelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                        IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                        selectionpatternprovider.ScrollIntoView();

                    }
                    //sometimes zero element is not found 
                    DateTime startTime = DateTime.Now;
                    while (zeroelement == null && (DateTime.Now - startTime).TotalSeconds <= 10)
                    {
                        lastheaderelement = headeritemarray.GetElement(i);
                        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(lastheaderelement.CurrentBoundingRectangle.right - 17, lastheaderelement.CurrentBoundingRectangle.bottom - 10);
                        Thread.Sleep(500);
                        inputSimulator.Mouse.LeftButtonClick();
                        Thread.Sleep(2000);
                        zeroelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, andcondforzero);
                        if (zeroelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                        {
                            object scrollitemobj;
                            scrollitemobj = zeroelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                            IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                            selectionpatternprovider.ScrollIntoView();

                        }
                    }


                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(zeroelement.CurrentBoundingRectangle.right - 10, zeroelement.CurrentBoundingRectangle.bottom - 15);
                    Thread.Sleep(500);
                    inputSimulator.Mouse.LeftButtonClick();
                }
                }


            //}




        }
    }
    static string copytabname = "";
    public static void handlecopywork(IUIAutomationElement currwin, string copyfromdate, string copytodate)
    {
        IUIAutomation automation = new CUIAutomation8();

        IUIAutomationCondition copydatecond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "dtCopyFromDate");
        IUIAutomationElement copydateelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, copydatecond);

        IUIAutomationCondition selectdatecond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "dtDateMonth");
        IUIAutomationElement selectdateelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, selectdatecond);

        IUIAutomationCondition getbttncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnFetchData");
        IUIAutomationElement getbttnelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, getbttncond);

        IUIAutomationCondition condsavebttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnSave");
        IUIAutomationElement savebttnelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, condsavebttn);

        IUIAutomationCondition conddailybttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "optDaily");
        IUIAutomationElement dailybttnelement = currwin.FindFirst(TreeScope.TreeScope_Descendants, conddailybttn);

        IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "dtLiveFeed");
        IUIAutomationElement datefield = currwin.FindFirst(TreeScope.TreeScope_Descendants, condition);
        
        try
        {

            ClickElement(savebttnelement, "Left Mouse Button Clicked");
            Thread.Sleep(1000);
            try
            {

                IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "WARNING");
                IUIAutomationCondition conddialogbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "CONFIRMATION");
                IUIAutomationOrCondition newandcond = (IUIAutomationOrCondition)automation.CreateOrCondition(conddialogbox, conddialogbox1);

                IUIAutomationElement dialogbox = currwin.FindFirst(TreeScope.TreeScope_Descendants, newandcond);

                if (dialogbox != null)
                {
                    IUIAutomationCondition okbtncond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                    var btn1 = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, okbtncond1);
                    replayaction(btn1, "");
                }
                IUIAutomationCondition okbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                var btn = currwin.FindFirst(TreeScope.TreeScope_Descendants, okbtncond);
                if (btn != null)
                {
                    replayaction(btn, "");
                }



            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.StackTrace.ToString());
            }



            //change tab and come to this tab again
            IUIAutomationCondition tabcondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TabItemControlTypeId);
            IUIAutomationCondition tabnamecond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId,"MarkPrice");
            IUIAutomationAndCondition tabconditionchange = (IUIAutomationAndCondition)automation.CreateAndCondition(tabcondition, tabnamecond);
            IUIAutomationElement tabitem = currwin.FindFirst(TreeScope.TreeScope_Descendants, tabconditionchange);
            
           
                object patternobj;
                if (tabitem.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                {
                    patternobj = tabitem.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                    IUIAutomationScrollItemPattern scrolltoview = patternobj as IUIAutomationScrollItemPattern;
                    scrolltoview.ScrollIntoView();
                }
                _currwinglobal = currwin;
                
                ClickElement(tabitem, "Left Mouse Button Clicked");
                Thread.Sleep(1000);
                try
                {

                    IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "WARNING");
                    IUIAutomationCondition conddialogbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "CONFIRMATION");
                    IUIAutomationOrCondition newandcond1 = (IUIAutomationOrCondition)automation.CreateOrCondition(conddialogbox, conddialogbox1);

                    IUIAutomationElement dialogbox = currwin.FindFirst(TreeScope.TreeScope_Descendants, newandcond1);

                    if (dialogbox != null)
                    {
                        IUIAutomationCondition okbtncond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                        var btn1 = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, okbtncond1);
                        replayaction(btn1, "");
                    }
                    IUIAutomationCondition okbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                    var btn = currwin.FindFirst(TreeScope.TreeScope_Descendants, okbtncond);
                    if (btn != null)
                    {
                        replayaction(btn, "");
                    }

                }
                catch (Exception ex)
                {
                    CreateExceptionReport(ex.StackTrace.ToString());
                }

                IUIAutomationCondition tabcondition1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TabItemControlTypeId);
                IUIAutomationCondition tabnamecond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, copytabname);
                IUIAutomationAndCondition newandcond11 = (IUIAutomationAndCondition)automation.CreateAndCondition(tabcondition1, tabnamecond1);
                IUIAutomationElement tabitem1 = currwin.FindFirst(TreeScope.TreeScope_Descendants, newandcond11);

                //CreateLogs("tabnames");
                CreateLogs(tabitem1.CurrentAutomationId, logFilePath);
                CreateLogs(tabitem1.CurrentName, logFilePath);
                
                    object patternob;
                    if (tabitem1.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                    {
                        patternob = tabitem1.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                        IUIAutomationScrollItemPattern scrolltoview = patternob as IUIAutomationScrollItemPattern;
                        scrolltoview.ScrollIntoView();
                    }


                    _currwinglobal = currwin;

                    ClickElement(tabitem1, "Left Mouse Button Clicked");
                    Thread.Sleep(1000);
                    try
                    {

                        IUIAutomationCondition conddialogbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "WARNING");
                        IUIAutomationCondition conddialogbox11 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "CONFIRMATION");
                        IUIAutomationOrCondition errorcondition = (IUIAutomationOrCondition)automation.CreateOrCondition(conddialogbox1, conddialogbox11);

                        IUIAutomationElement dialogbox = currwin.FindFirst(TreeScope.TreeScope_Descendants, errorcondition);

                        if (dialogbox != null)
                        {
                            IUIAutomationCondition okbtncond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                            var btn1 = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, okbtncond1);
                            replayaction(btn1, "");
                        }
                        IUIAutomationCondition okbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                        var btn = currwin.FindFirst(TreeScope.TreeScope_Descendants, okbtncond);
                        if (btn != null)
                        {
                            replayaction(btn, "");
                        }
                    }
                    catch (Exception ex)
                    {
                        CreateExceptionReport(ex.StackTrace.ToString());
                    }





                //chnage tab come to this tab again
                    Thread.Sleep(1000);
            object copydateobj;
            if (copydateelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
            {
                copydateobj = copydateelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                IUIAutomationValuePattern valuepatt = copydateobj as IUIAutomationValuePattern;
                valuepatt.SetValue(copyfromdate);

            }
            Thread.Sleep(2000);
            try
            {

                IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "WARNING");
                IUIAutomationCondition conddialogbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "CONFIRMATION");
                IUIAutomationOrCondition newandcond = (IUIAutomationOrCondition)automation.CreateOrCondition(conddialogbox, conddialogbox1);

                IUIAutomationElement dialogbox = currwin.FindFirst(TreeScope.TreeScope_Descendants, newandcond);

                if (dialogbox != null)
                {
                    IUIAutomationCondition okbtncond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                    var btn1 = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, okbtncond1);
                    replayaction(btn1, "");
                }
                IUIAutomationCondition okbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                var btn = currwin.FindFirst(TreeScope.TreeScope_Descendants, okbtncond);
                if (btn != null)
                {
                    replayaction(btn, "");
                }



            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.StackTrace.ToString());
            }
           


            Thread.Sleep(2000);
            _currwinglobal = currwin;
            _datetoelement = selectdateelement;
            _datestring = copytodate;
            Thread modalThread = new Thread(HandleConfirmationAndWarning1);

            if (selectdateelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
            {
                copydateobj = selectdateelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                IUIAutomationValuePattern valuepatt = copydateobj as IUIAutomationValuePattern;
                //modalThread.Start();

               // Console.WriteLine("before date entered");

                valuepatt.SetValue(copytodate);
               // Console.WriteLine("date entered");

               // modalThread.Join();

            }
            //modalThread.Start();
            // modalThread.Join();
            Thread.Sleep(1000);
            Console.WriteLine("Dates filled");
           
           
            try
            {

                IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "WARNING");
                IUIAutomationCondition conddialogbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "CONFIRMATION");
                IUIAutomationOrCondition newandcond = (IUIAutomationOrCondition)automation.CreateOrCondition(conddialogbox, conddialogbox1);

                IUIAutomationElement dialogbox = currwin.FindFirst(TreeScope.TreeScope_Descendants, newandcond);

                if (dialogbox != null)
                {
                    IUIAutomationCondition okbtncond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                    var btn1 = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, okbtncond1);
                    replayaction(btn1, "");
                }
                IUIAutomationCondition okbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                var btn = currwin.FindFirst(TreeScope.TreeScope_Descendants, okbtncond);
                if (btn != null)
                {
                    replayaction(btn, "");
                }

            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.StackTrace.ToString());
            }
           
            if (dailybttnelement != null)
            {
                ClickElement(dailybttnelement, "Left Mouse Button Clicked");
                    }
            //Thread.Sleep(2000);
            //modalThread2.Join();
            Console.WriteLine("Daily bttn pressed");
            Thread.Sleep(5000);
            try
            {

                IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "WARNING");
                IUIAutomationCondition conddialogbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "CONFIRMATION");
                IUIAutomationOrCondition newandcond = (IUIAutomationOrCondition)automation.CreateOrCondition(conddialogbox1, conddialogbox);

                IUIAutomationElement dialogbox = currwin.FindFirst(TreeScope.TreeScope_Descendants, newandcond);

                if (dialogbox != null)
                {
                    IUIAutomationCondition okbtncond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                    var btn1 = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, okbtncond1);
                    replayaction(btn1, "");
                }
                IUIAutomationCondition okbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                var btn = currwin.FindFirst(TreeScope.TreeScope_Descendants, okbtncond);
                if (btn != null)
                {
                    replayaction(btn, "");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in dialog box");
                CreateExceptionReport(ex.StackTrace.ToString());
            }
        }
        catch(Exception ex)
        {
            CreateLogs("Exception in Copy Data ", logFilePath);
            CreateExceptionReport(ex.StackTrace.ToString());
        }
        Thread.Sleep(1000);

        if (copytabname != "")
        {
            Thread modalThread3 = new Thread(HandleConfirmationAndWarning);
            modalThread3.Start();
            modalThread3.Join();
            SetFilter(currwin, copytabname);
        }
        //Thread modalThread = new Thread(HandleConfirmationAndWarning);
        //modalThread.Start();

        Thread modalThread1 = new Thread(HandleConfirmationAndWarning);
        modalThread1.Start();
        InputSimulator simulator = new InputSimulator();
            tagPOINT p;
            getbttnelement.GetClickablePoint(out p);
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)p.x, (int)p.y);

            simulator.Mouse.LeftButtonClick();

            Thread.Sleep(2000);
            CreateLogs("Get Button Pressed ", logFilePath);
        modalThread1.Join();
        
        try
        {

            IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "WARNING");
            IUIAutomationCondition conddialogbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "CONFIRMATION");
            IUIAutomationOrCondition newandcond = (IUIAutomationOrCondition)automation.CreateOrCondition(conddialogbox, conddialogbox1);

            IUIAutomationElement dialogbox = currwin.FindFirst(TreeScope.TreeScope_Descendants, newandcond);

            if (dialogbox != null)
            {
                IUIAutomationCondition okbtncond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                var btn1 = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, okbtncond1);
                replayaction(btn1, "");
            }
            IUIAutomationCondition okbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

            var btn = currwin.FindFirst(TreeScope.TreeScope_Descendants, okbtncond);
            if (btn != null)
            {
                replayaction(btn, "");
            }



        }
        catch(Exception ex)
        {
            CreateExceptionReport(ex.StackTrace.ToString());
        }

        //    object patternsavebttn1;
        //if (savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
        //{
        //    patternsavebttn1 = savebttnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);

        //    IUIAutomationInvokePattern savebttnpatt = patternsavebttn1 as IUIAutomationInvokePattern;
        //    savebttnpatt.Invoke();
        //}
        ClickElement(savebttnelement, "Left Mouse Button Clicked");
        Thread.Sleep(1000);
       
        
        CreateLogs("Completed Copy work for dates " + copyfromdate + copytodate,logFilePath);


    }
    public static void filldatainenterprise(IUIAutomationElement gridelement,string tabname)
    {
        try
        {
            CreateLogs("Going to Fill Prices in Enterprise ", logFilePath);
            IUIAutomationElement currentwin = null;
            BringToForeground("Daily Valuation", ref currentwin, exePath);
            IUIAutomation automation = new CUIAutomation8();


            IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TreeControlTypeId);
            IUIAutomationElement treecontrolgrid = gridelement.FindFirst(TreeScope.TreeScope_Descendants, condition);

            IUIAutomationCondition condition1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderControlTypeId);
            IUIAutomationElement headercontrolgrid = gridelement.FindFirst(TreeScope.TreeScope_Descendants, condition1);



            IUIAutomationCondition condition2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
            IUIAutomationElementArray dataitemsingrid = treecontrolgrid.FindAll(TreeScope.TreeScope_Descendants, condition2);

            IUIAutomationCondition condition3 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId);
            IUIAutomationCondition condition4 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Symbol");
            IUIAutomationAndCondition newandcond = (IUIAutomationAndCondition)automation.CreateAndCondition(condition3, condition4);
            //  IUIAutomationElementArray symbolsdata = treecontrolgrid.FindAll(TreeScope.TreeScope_Descendants, newandcond);
            IUIAutomationCondition condition6 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId);
            IUIAutomationCondition condition7 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, processDateForUI);
            IUIAutomationAndCondition markpriceelement = (IUIAutomationAndCondition)automation.CreateAndCondition(condition6, condition7);



            for (int i = 0; i < dataitemsingrid.Length; i++)
            {
                IUIAutomationElement onedataelement = dataitemsingrid.GetElement(i);
                IUIAutomationElement symbolsdata = onedataelement.FindFirst(TreeScope.TreeScope_Descendants, newandcond);
                IUIAutomationElement markpriceinput = onedataelement.FindFirst(TreeScope.TreeScope_Descendants, markpriceelement);
                string symbolval = getnewvalue(symbolsdata);
                CreateLogs("Current Symbol is " + symbolval, logFilePath);

                if (tabnametoincludeflag.ContainsKey(tabname) && tabnametoincludeflag[tabname].ToLower() == "true")
                {
                    if (includesymbols != null && !includesymbols.Contains(symbolval))
                    {
                        CreateLogs("Symbol Not in Include List", logFilePath);
                        continue;
                    }
                }
                if (excludesymbols != null && excludesymbols.Contains(symbolval))
                {
                    CreateLogs("Symbol is in Exclude list Data Not Filled for this symbol : " + symbolval, logFilePath);
                    continue;
                }
                if (enterprisemapping.ContainsKey(symbolval))
                {
                    symbolval = enterprisemapping[symbolval];
                }
                if (originalmapping.ContainsKey(symbolval))
                {
                    symbolval = originalmapping[symbolval];
                }
                if (yahootoenterprisemapping.ContainsKey(symbolval))
                {
                    // symbolval = yahootoenterprisemapping[symbolval];

                }

                CreateLogs("Symbol For Yahoo is : " + symbolval, logFilePath);

                if (symboltopricefromyahoo.ContainsKey(symbolval))
                {
                    replayaction(markpriceinput, symboltopricefromyahoo[symbolval]);
                    Thread.Sleep(500);
                }

                CreateLogs("Data Filled For Symbol : " + symbolval + "  -  " + markpriceinput, logFilePath);


            }
        }
        catch(Exception ex)
        {
            CreateLogs("Exception While Filling Prices In Enterprise ", logFilePath);
            CreateExceptionReport(ex.StackTrace.ToString());
            CreateExceptionReport(ex.Message);

        }
    }

    static Dictionary<string, string> ReadCsvToDictionary(string filePath)
    {
        CreateLogs("Reading CSV " + filePath, logFilePath);
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();

                        if (!dictionary.ContainsKey(key))
                        {
                            dictionary.Add(key, value);
                        }
                        else
                        {
                            Console.WriteLine($"Duplicate key found: {key}. Skipping this entry.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid line format: {line}. Skipping this entry.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            CreateExceptionReport(ex.Message);
            CreateExceptionReport(ex.StackTrace.ToString());
        }

        return dictionary;
    }
    static Dictionary<string, string> replaceMapping = new Dictionary<string, string>();
    static Dictionary<string, string> _exchangeMapping = new Dictionary<string, string>();
    static Dictionary<string, string> originalmapping = new Dictionary<string, string>();
    static Dictionary<string, string> yahootoenterprisemapping = new Dictionary<string, string>();
    public class CsvRecord
    {
        public string Client { get; set; }
        public string URL { get; set; }
    }

    static List<CsvRecord> ReadCsvFile(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            return csv.GetRecords<CsvRecord>().ToList();
        }
    }
    static bool setupflag = false;
    static IWebDriver driver = null;
    public static Dictionary<string, string> symboltopricefromyahoo = new Dictionary<string, string>();
    public static void getpricesfromyahoo(List<string> symbols)
    {
        try
        {
            CreateLogs("Getting Prices From Yahoo", logFilePath);
            Console.WriteLine("Getting prices from yahoo.....");
            string currdirectory = Directory.GetCurrentDirectory();
            string filePath1 = currdirectory + @"\\Configs\\symbolmapping.csv";
            string filePath2 = currdirectory + @"\\Configs\\exchangemapping.csv";
            string csvFilePath = currdirectory + @"\\Configs\\quotes.csv";
            string csvFilePath1 = currdirectory + @"\\Configs\\clienttourlmapping.csv";
            // string currencymappingfile= currdirectory + @"\\Configs\\clienttourlmapping.csv";
            string csvFilePathmapping = currdirectory + @"\\Configs\\clientmenumapping.csv";

            replaceMapping = ReadCsvToDictionary(filePath1);
            _exchangeMapping = ReadCsvToDictionary(filePath2);

            if (!setupflag)
            {
                CreateLogs("Setting up chrome driver", logFilePath);
                setupflag = true;
                new DriverManager().SetUpDriver(new ChromeConfig());

                ChromeOptions options = new ChromeOptions();
                //options.AddArgument("--window-size=1920,1080");
                options.AddArgument("--disable-extensions");
                
                options.AddArgument("--disable-gpu");
                //options.AddArgument("--disable-dev-shm-usage");
                // options.AddArgument("--no-sandbox");
                options.AddArgument("--ignore-certificate-errors");
                options.AddArgument("--disable-web-security");
                //  options.AddArgument("--headless");
                ////options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");
                string currdir = Directory.GetCurrentDirectory();

                string userDataDir = Path.Combine(currdir, "selenium", "AutomationChrome");

                // Create the directory if it doesn't exist
                Directory.CreateDirectory(userDataDir);
                options.AddArgument($"--user-data-dir={userDataDir}");
                options.AddArgument("--remote-debugging-port=60407");
                driver = new ChromeDriver(options);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                // Now the driver is initialized and ready for use
                Console.WriteLine("WebDriver is initialized and ready.");
                CreateLogs("Chrome driver setup completed", logFilePath);
            }

            for (int i = 0; i < symbols.Count; i++)
            {
                string symbol = symbols[i];
                bool flag = true;
                if (replaceMapping.ContainsKey(symbol))
                {
                    symbols[i] = replaceMapping[symbol];
                    if(!originalmapping.ContainsKey(symbol))
                    originalmapping.Add(symbol, replaceMapping[symbol]);
                    if (!yahootoenterprisemapping.ContainsKey(replaceMapping[symbol]))
                        yahootoenterprisemapping.Add(replaceMapping[symbol], symbol);

                    flag = false;
                }
                else
                {

                    foreach (string s in _exchangeMapping.Keys)
                    {
                        if (symbol.Contains(s))
                        {
                            string orgsym = symbol;
                            if (!originalmapping.ContainsKey(orgsym))
                                originalmapping.Add(orgsym, symbol.Replace(s, _exchangeMapping[s]));
                            if (!yahootoenterprisemapping.ContainsKey(symbol.Replace(s, _exchangeMapping[s])))
                                yahootoenterprisemapping.Add(symbol.Replace(s, _exchangeMapping[s]), orgsym);
                            symbols[i] = symbol.Replace(s, _exchangeMapping[s]);
                            flag = false;
                        }
                        else
                            continue;
                    }
                }
                if (symbol.Contains('$'))
                {
                    if (!originalmapping.ContainsKey(symbol))
                        originalmapping.Add(symbol, symbol.Replace('$', '^'));
                    if (!yahootoenterprisemapping.ContainsKey(symbol.Replace('$', '^')))
                        yahootoenterprisemapping.Add(symbol.Replace('$', '^'), symbol);
                    string new_symbol = symbol.Replace('$', '^');

                    symbols[i] = new_symbol;
                    flag = false;
                }
                if (flag)
                {
                    symbols[i] = symbol;
                }
            }


            string searchValue = "test";
            string resultValue = null;

            try
            {
                // Read the CSV file
                List<CsvRecord> records = ReadCsvFile(csvFilePath1);

                // Find the matching record and initialize resultValue
                var matchingRecord = records.FirstOrDefault(r => r.Client == searchValue);
                if (matchingRecord != null)
                {
                    resultValue = matchingRecord.URL;
                }
            }
            catch (Exception ex)
            {

            }

            if (resultValue != null)
            {
                driver.Navigate().GoToUrl(resultValue);
            }
            ///html/body/div[1]/header/div/div/div/div[2]/div/div[3]/div[3]/div/div/a
            ///html/body/div[1]/header/div/div/div/div[2]/div/div[3]/div[3]/div/div/a
            //#login-container > a
            //#login-container > a
            Console.WriteLine("url launched");

            //Thread.Sleep(10000);
            //#Nav-0-DesktopNav-0-DesktopNav > div > div.nr-applet-main-nav-right.Bxz\(bb\).Fl\(end\).Px\(navPaddings\).H\(navHeight\).Bdc\(t\).Bdrs\(1\.5px\).Bdbs\(s\)\:h.Px\(10px\)\!.Miw\(160px\).H\(itemHeight_uhMagDesign\)\! > nav > ul > li > div > a
            IWebElement newyahooelement = null;
            try
            {

                newyahooelement = driver.FindElement(By.CssSelector("#Nav-0-DesktopNav-0-DesktopNav > div > div.nr-applet-main-nav-right.Bxz\\(bb\\).Fl\\(end\\).Px\\(navPaddings\\).H\\(navHeight\\).Bdc\\(t\\).Bdrs\\(1\\.5px\\).Bdbs\\(s\\)\\:h.Px\\(10px\\)\\!.Miw\\(160px\\).H\\(itemHeight_uhMagDesign\\)\\! > nav > ul > li > div > a"));
                if (newyahooelement == null)
                {
                    newyahooelement = driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[1]/div/div[1]/div[2]/div[1]/div/div/div/div[1]/div/div/div/div/nav/div/div/div/div[2]/nav/ul/li/div/a"));
                }

            }
            catch (Exception ex)
            {
                


            }
            if (newyahooelement != null)
            {
                newyahooelement.Click();
            }
            IWebElement signinelement = null;
            try
            {

                signinelement = driver.FindElement(By.CssSelector("#login-container > a"));
                if (signinelement == null)
                {
                    signinelement = driver.FindElement(By.XPath("/html/body/div[1]/header/div/div/div/div[2]/div/div[3]/div[3]/div/div/a"));
                }


            }
            catch (Exception ex)
            {
                

            }

            if (signinelement != null)
            {
                signinelement.Click();
                Thread.Sleep(5000);
                IWebElement pickusername = null;
                try
                {
                    pickusername = driver.FindElement(By.CssSelector("#account-switcher-form > ul > li > a"));
                    if (pickusername == null)
                    {
                        pickusername = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[2]/form/ul/li/a"));
                    }


                }
                catch (Exception ex)
                {

                    
                }

                if (pickusername != null)
                {
                    pickusername.Click();
                }
                IWebElement usernameelement = null;
                try
                {

                    usernameelement = driver.FindElement(By.CssSelector("#login-username"));
                    

                }
                catch (Exception ex)
                {
                    if (usernameelement == null)
                    {
                        usernameelement = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[2]/div[2]/form/div[2]/input"));
                    }


                }
                IWebElement identifyfield = null;

                if (usernameelement != null)
                {
                    //#password-challenge > strong
                    // IWebElement identifyfield;
                    try
                    {

                        identifyfield = driver.FindElement(By.CssSelector("#password-challenge > strong"));
                        
                    }
                    catch (Exception ex)
                    {
                        if (identifyfield == null)
                        {
                            identifyfield = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[2]/div[2]/strong"));
                        }


                    }

                    if (identifyfield.Text.ToLower().Contains("sign in"))
                    {

                        usernameelement.SendKeys("Markpriceyahoo@gmail.com");
                    }
                }

                IWebElement signinbttn = null;
                try
                {

                    signinbttn = driver.FindElement(By.CssSelector("#login-signin"));
                    

                }
                catch (Exception ex)
                {
                    if (signinbttn == null)
                    {
                        signinbttn = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[2]/div[2]/form/div[3]/button"));
                    }


                }


                if (signinbttn != null)
                {
                    if (identifyfield.Text.ToLower().Contains("sign in"))
                    {

                        signinbttn.Click();
                    }
                }

                Thread.Sleep(2000);
                IWebElement passwordelement = null;
                try
                {

                    passwordelement = driver.FindElement(By.CssSelector("#login-passwd"));
                    

                }
                catch (Exception ex)
                {
                    if (passwordelement == null)
                    {
                        passwordelement = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[2]/div[2]/form/div[2]/input"));
                    }



                }
                if (passwordelement != null)
                {
                    passwordelement.SendKeys("Symbol@321");
                }
                try
                {

                    signinbttn = driver.FindElement(By.CssSelector("#login-signin"));
                    
                }
                catch (Exception ex)
                {
                    if (signinbttn == null)
                    {
                        signinbttn = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[2]/div[2]/form/div[3]/button"));
                    }



                }
                try
                {

                    identifyfield = driver.FindElement(By.CssSelector("#password-challenge > strong"));
                    

                }
                catch (Exception ex)
                {
                    if (identifyfield == null)
                    {
                        identifyfield = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[2]/div[2]/strong"));

                    }

                }

                if (signinbttn != null)
                {
                    if (identifyfield.Text.ToLower().Contains("password"))
                    {
                        signinbttn.Click();
                    }
                }

                IWebElement gotooldyahooelement = null;
                try
                {

                    gotooldyahooelement = driver.FindElement(By.CssSelector("#ybar-nav-placement > a > span"));
                    

                }
                catch (Exception ex)
                {
                    //#ybar-nav-placement > a
                    if (gotooldyahooelement == null)
                    {
                        gotooldyahooelement = driver.FindElement(By.XPath("/html/body/div[1]/header/div/div/div/div[4]/div/div/div[2]/a"));
                    }


                }
                if (gotooldyahooelement != null)
                {
                    // driver.execute_script("arguments[0].scrollIntoView(true);", gotooldyahooelement);
                    gotooldyahooelement.Click();
                }
                Thread.Sleep(10000);
                if (resultValue != null)
                {
                    driver.Navigate().GoToUrl(resultValue);
                }





            }

            Thread.Sleep(10000);
            IWebElement tableElement = null;
            try
            {
                IWebElement GotoOldYahoo = null;
                try
                {

                    GotoOldYahoo = driver.FindElement(By.CssSelector("#ybar-nav-placement > a"));
                }
                catch(Exception ex)
                {
                    try
                    {
                        //#ybar-nav-placement > a
                        GotoOldYahoo = driver.FindElement(By.XPath("/html/body/div[1]/header/div/div/div/div[4]/div/div/div[2]/a"));
                    }
                    catch (Exception ex1)
                    {

                    }
                }
                if (GotoOldYahoo != null)
                {
                    Console.WriteLine(GotoOldYahoo.GetAttribute("aria-label"));
                    if (GotoOldYahoo.GetAttribute("aria-label") == "Back to Yahoo Finance classic")
                    {
                        // driver.execute_script("arguments[0].scrollIntoView(true);", gotooldyahooelement);
                        GotoOldYahoo.Click();
                    }
                }

                IWebElement popupelement = null;

                try
                {

                    popupelement = driver.FindElement(By.CssSelector("#myLightboxContainer > div > button"));

                }
                catch (Exception ex2)
                {
                    try
                    {
                        //#ybar-nav-placement > a
                        popupelement = driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[1]/div/div[4]/div/div/div[1]/div/div/div/div/div/div/button"));
                    }
                    catch (Exception ex1)
                    {

                    }

                }
                if (popupelement != null)
                {
                    // driver.execute_script("arguments[0].scrollIntoView(true);", gotooldyahooelement);
                    popupelement.Click();
                }


                tableElement = driver.FindElement(By.CssSelector(@"#pf-detail-table > div.Ovx\(a\).Ovx\(h\)--print.Ovy\(h\).W\(100\%\) > table"));

            }
            catch (Exception ex)
            {
                tableElement = driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[1]/div/div[2]/div/div/div[4]/div/div/main/div/div/div[3]/div/div[1]/table"));
            }
            if (tableElement == null)
            {
                Console.WriteLine("Unable to find table");

            }
            DataTable portfoliotable = new DataTable();

            // Find all table headers
            IList<IWebElement> headerCells1 = tableElement.FindElements(By.XPath(".//th"));
            foreach (IWebElement headerCell in headerCells1)
            {
                // Get the header text and add it as a DataColumn to the DataTable
                string headerText = headerCell.Text;
                //Console.WriteLine(headerText);
                if (headerText != "")
                {
                    portfoliotable.Columns.Add(headerText);
                }
            }

            // Find all table rows
            IList<IWebElement> tableRows1 = tableElement.FindElements(By.XPath(".//tbody/tr"));
            foreach (IWebElement row in tableRows1)
            {
                // Create a new DataRow
                DataRow dataRow = portfoliotable.NewRow();

                // Find all cells in the row
                IList<IWebElement> cells = row.FindElements(By.XPath(".//td"));

                // Set the cell values in the DataRow
                for (int i = 0; i < cells.Count - 2; i++)
                {
                    dataRow[i] = cells[i].Text;
                }

                // Add the DataRow to the DataTable
                portfoliotable.Rows.Add(dataRow);
            }

            List<string> symbolList = new List<string>();

            // Loop through each row in the DataTable
            foreach (DataRow row in portfoliotable.Rows)
            {
                // Add the value of the "symbol" column to the list
                if (row["Symbol"] != DBNull.Value)
                {
                    symbolList.Add(row["Symbol"].ToString());
                }
            }
            foreach (var syminportfolio in symbols)
            {
                string symbol = syminportfolio;
                if (originalmapping.ContainsKey(syminportfolio))
                {
                    symbol = originalmapping[syminportfolio];
                }
                if (!symbolList.Contains(symbol))
                {
                    Console.WriteLine("Adding Symbol  : " + symbol);
                    try
                    {

                        IWebElement addsymbolbttn;
                        IWebElement symbolinputbx;
                        try
                        {
                            // Try finding the element using XPath
                            addsymbolbttn = driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[1]/div/div[2]/div/div/div[4]/div/div/main/div/div/div[1]/div/div/div"));

                            addsymbolbttn.Click();

                            //Console.WriteLine("Element found by XPath");
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                // If element not found by XPath, try finding it using CSS selector
                                addsymbolbttn = driver.FindElement(By.CssSelector("#Lead-3-Portfolios-Proxy > main > div > div > div:nth-child(1) > div > div > div"));
                                addsymbolbttn.Click();



                                Console.WriteLine("Element found by CSS selector");
                            }
                            catch (Exception ex1)
                            {
                                Console.WriteLine("Element not found by XPath or CSS selector");

                            }
                        }


                        Thread.Sleep(2000);
                        try
                        {

                            symbolinputbx = driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[1]/div/div[2]/div/div/div[4]/div/div/main/div/div/div[1]/div/div[2]/div[3]/form/input"));

                            symbolinputbx.SendKeys(symbol);
                            Thread.Sleep(2000);
                            symbolinputbx.SendKeys(OpenQA.Selenium.Keys.Enter);
                        }
                        catch (Exception ex)
                        {
                            try
                            {

                                symbolinputbx = driver.FindElement(By.CssSelector("#dropdown-menu > div.Bxz\\(bb\\).Z\\(2\\).My\\(8px\\).Py\\(0\\).Px\\(8px\\).W\\(300px\\).Maw\\(98\\%\\) > form > input"));
                                symbolinputbx.SendKeys(symbol);
                                Thread.Sleep(2000);
                                symbolinputbx.SendKeys(OpenQA.Selenium.Keys.Enter);
                                Console.WriteLine("Element found by CSS selector");
                            }
                            catch (Exception ex1)
                            {
                                Console.WriteLine("Element not found by XPath or CSS selector");

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Element not found by XPath or CSS selector");
                    }

                }
            }


            Thread.Sleep(5000);

            //IWebElement tableElement;
            try
            {

                tableElement = driver.FindElement(By.CssSelector(@"#pf-detail-table > div.Ovx(a).Ovx(h)--print.Ovy(h).W(100%) > table"));

            }
            catch (Exception ex)
            {

                tableElement = driver.FindElement(By.XPath("//*[@id=\"pf-detail-table\"]/div[1]/table"));

            }


            DataTable dataTable = new DataTable();

            // Find all table headers
            IList<IWebElement> headerCells = tableElement.FindElements(By.XPath(".//th"));
            foreach (IWebElement headerCell in headerCells)
            {
                // Get the header text and add it as a DataColumn to the DataTable
                string headerText = headerCell.Text;
                //Console.WriteLine(headerText);
                if (headerText != "")
                {
                    dataTable.Columns.Add(headerText);
                }
            }

            // Find all table rows
            IList<IWebElement> tableRows = tableElement.FindElements(By.XPath(".//tbody/tr"));
            foreach (IWebElement row in tableRows)
            {
                // Create a new DataRow
                DataRow dataRow = dataTable.NewRow();

                // Find all cells in the row
                IList<IWebElement> cells = row.FindElements(By.XPath(".//td"));

                // Set the cell values in the DataRow
                for (int i = 0; i < cells.Count - 2; i++)
                {
                    dataRow[i] = cells[i].Text;
                }

                // Add the DataRow to the DataTable
                dataTable.Rows.Add(dataRow);
            }

            foreach (DataRow dataRow in dataTable.Rows)
            {
                string symbol = dataRow["Symbol"].ToString();
                string data = dataRow["Last Price"].ToString();

                if (!symboltopricefromyahoo.ContainsKey(symbol))
                {
                    symboltopricefromyahoo.Add(symbol, data);
                }

            }
            setupflag = false;
            driver.Quit();
            Console.WriteLine("Prices fetched from Yahoo........");
            CreateLogs("Completed Price Fetching for Yahoo", logFilePath);
        }
        catch(Exception ex)
        {
            CreateLogs("Exception in yahoo price fetching ", logFilePath);
            CreateExceptionReport(ex.StackTrace.ToString());
            CreateExceptionReport(ex.Message);
            Console.WriteLine("Error in getting prices " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

    }

    

    public static Dictionary<string, string> enterprisemapping = new Dictionary<string, string>();
    public static List<string> getsymbolsfromgrid(IUIAutomationElement gridelement,string tabname)
    {
        CreateLogs("Getting Symbols From Grid for tabname : "+ tabname , logFilePath);
        List<string> symbols = new List<string>();
        //get header from grid 
        //if symbol not found in header 
        //look for symbol in other header
        //fill symbol 
        IUIAutomation automation = new CUIAutomation8();

        IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TreeControlTypeId);
        IUIAutomationElement treecontrolgrid = gridelement.FindFirst(TreeScope.TreeScope_Descendants, condition);

        IUIAutomationCondition condition1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderControlTypeId);
        IUIAutomationElement headercontrolgrid = gridelement.FindFirst(TreeScope.TreeScope_Descendants, condition1);

        
        
        IUIAutomationCondition condition2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
        IUIAutomationElementArray dataitemsingrid = treecontrolgrid.FindAll(TreeScope.TreeScope_Descendants, condition2);

        IUIAutomationCondition condition3 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId);
        IUIAutomationCondition condition4 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Symbol");
        IUIAutomationCondition condition5 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Bloomberg Symbol");
        IUIAutomationAndCondition newandcond1 = (IUIAutomationAndCondition)automation.CreateAndCondition(condition3, condition5);
        IUIAutomationAndCondition newandcond = (IUIAutomationAndCondition)automation.CreateAndCondition(condition3, condition4);
        IUIAutomationElementArray symbolsdata = treecontrolgrid.FindAll(TreeScope.TreeScope_Descendants, newandcond);
        IUIAutomationElementArray bloombergsymbdata = null;
        try
        {
             bloombergsymbdata = treecontrolgrid.FindAll(TreeScope.TreeScope_Descendants, newandcond1);
        }
        catch(Exception ex)
        {
            Console.WriteLine("Bloomberg Symbol Not Found on grid for tabname : " + tabname);
        }



        for (int i = 0; i < symbolsdata.Length; i++)
        {
            IUIAutomationElement element = symbolsdata.GetElement(i);
            IUIAutomationElement bloomelement = null;
            try
            {
                if (bloombergsymbdata != null && bloombergsymbdata.Length >= i)
                {
                    bloomelement = bloombergsymbdata.GetElement(i);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            string bloomsymbol = "";
            if (bloomelement != null)
            {
                bloomsymbol = getnewvalue(bloomelement);

            }
            string symbol = getnewvalue(element);
           // symbol = "O:EG 25A390.00D17";
            //bloomsymbol = "EG US 01/17/25 C390 EQUITY";

            if (tabnametoincludeflag.ContainsKey(tabname) && tabnametoincludeflag[tabname].ToLower() == "true")
            {
                if (includesymbols!=null && !includesymbols.Contains(symbol))
                {
                    continue;
                }
            }

            if (excludesymbols != null && excludesymbols.Contains(symbol))
            {
                continue;
            }

            if (symbol.ToUpper().Contains("O:") && (tabname.ToLower().Contains("option") || tabname.ToLower().Contains("opt")))
            {
                CreateLogs("Option Symbol : " + symbol, logFilePath);

                string orgsymbol = symbol;

                string[] symbolarr = symbol.Split(' ');
                char charforsymbol = ' ';
                if(symbolarr.Length >= 2)
                {
                    charforsymbol = symbolarr[1][2];
                }
                string strikeprice = "";
                string date = "";
                string symbolticker = "";
                string[] bloomarr = bloomsymbol.Split(' ');
                if (bloomarr.Length >=2)
                {
                    symbolticker = bloomarr[0];
                    date = bloomarr[2];
                    strikeprice = bloomarr[bloomarr.Length - 2];
                }
                strikeprice=ExtractNumber(strikeprice);

               date=getdateforoptionssymbol(date);
                strikeprice=getstrikepricewithdenominator(strikeprice);


                symbol = symbolticker + "#" + charforsymbol + date + strikeprice+":D";
              if(!enterprisemapping.ContainsKey(orgsymbol))
                enterprisemapping.Add(orgsymbol, symbol);
                CreateLogs("Option Symbol After Mapping is : " + symbol, logFilePath);
                Console.WriteLine("Option Symbol For FACTSET is : " + symbolticker + "#" + charforsymbol + date + strikeprice);


            }


                if ((symbol.ToUpper().Contains("FX") && (tabname.ToLower().Contains("fx") || tabname.ToLower().Contains("forex")))|| (tabname.ToLower().Contains("fx") || tabname.ToLower().Contains("forex")))
            {

                CreateLogs("FX Symbol : " + symbol, logFilePath);
                // string[] arr = symbol.Split(' ');
                // string curr = arr[0];
                string orgsymbol = symbol;
                string curr = symbol;
                if (symbol.ToLower().Contains("eur") || symbol.ToLower().Contains("aud") || symbol.ToLower().Contains("gbp") || symbol.ToLower().Contains("nzd"))
                {
                    curr = getsymbolusingregex(symbol,pattern2);
                }
                else
                {
                    curr = getsymbolusingregex(symbol, pattern1);
                }
                
                switch (curr)
                {
                    case "EUR":
                    case "AUD":
                    case "GBP":
                    case "NZD":
                       // enterprisemapping.Add(cur);
                       
                        symbol = curr + "USD=X";
                        if(!enterprisemapping.ContainsKey(orgsymbol))
                        enterprisemapping.Add(orgsymbol,symbol);
                        break;
                    default:
                        symbol = curr + "=X";
                        if (!enterprisemapping.ContainsKey(orgsymbol))
                            enterprisemapping.Add(orgsymbol, symbol);
                        break;
                }
                CreateLogs("FX Symbol After Mapping is  : " + symbol, logFilePath);
            }
            //symbol = getsymbolmapping(symbol);
            symbols.Add(symbol);    
            Console.WriteLine(symbol);
        }

        return symbols;

    }
    public static string ExtractNumber(string input)
    {
        string num = "";
        // Regular expression to match the numeric part
        var match = Regex.Match(input, @"\d+(\.\d+)?");

        if (match.Success)
        {
            // Parse the integer part of the number
            num=match.Value.Split('.')[0];
        }
        return num;
    }
        public static string getdateforoptionssymbol(string dateString)
    {
        string dateforoptions = "";
        DateTime date;

        // Try parsing the date string in both formats
        if (DateTime.TryParseExact(dateString, new[] { "MM/dd/yyyy", "MM/dd/yy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            string day = date.ToString("dd");
            string year = date.ToString("yy");
            dateforoptions += day + year;
            Console.WriteLine($"Day: {day}");
            Console.WriteLine($"Year: {year}");
        }
        return dateforoptions;

    }
    public static string getstrikepricewithdenominator(string strikePrice)
    {
        int targetLength = 6;

        // Pad the strike price with zeros to the right to make it 6 digits
        string paddedStrikePrice = strikePrice.PadRight(targetLength, '0');

        // Calculate the number of zeros added
        int zerosAdded = targetLength - strikePrice.Length;

        Console.WriteLine($"Original Strike Price: {strikePrice}");
        Console.WriteLine($"Padded Strike Price: {paddedStrikePrice}");
        Console.WriteLine($"Zeros Added: {zerosAdded}");
        char denominator = (char)('A' + (zerosAdded - 1));
        return denominator+ paddedStrikePrice;
    }
    public static string getsymbolusingregex(string symbol,string pattern)
    {
        //pattern = @"(?<=USD)([A-Z]{3})(?=-FX1)";

        pattern = pattern.Trim();
       // pattern = "@\"" + pattern + "\"";
        Match match = Regex.Match(symbol, pattern, RegexOptions.IgnoreCase);
        
        if (match.Success)
        {
            string currency = match.Value;  // Convert to uppercase if needed
            Console.WriteLine(currency + "Pattern matched for fx");
            return currency;

        }
        else
        {
            Console.WriteLine("Pattern not matched for " + symbol);
        }

        return symbol;

      
    }
       
    public static void OpenDailyValuation()
    {
        InputSimulator inputSimulator = new InputSimulator();
        inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
        inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
        inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_D);
        inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);
        inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
    }
    public static DateTime GetProcessDate(string configPath)
    {
        try
        {
            string xmlFilePath = ReadXmlPathFromConfig(configPath);

            if (!string.IsNullOrEmpty(xmlFilePath))
            {
                // Read the XML content from the specified path
                string xmlContent = File.ReadAllText(xmlFilePath);
                XMLDateExtractor parser = new XMLDateExtractor(xmlContent);
                DateTime filterValue = parser.ExtractFilterValue();

                if (filterValue != DateTime.MinValue)
                {
                    return filterValue;
                }
                else
                {
                    CreateExceptionReport("Failed to extract FilterValue.");
                }

            }
        }
        catch (Exception ex)
        {
            CreateExceptionReport(ex.StackTrace);
        }
        return DateTime.MinValue;
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
            CreateExceptionReport($"Error reading XML path from config: {ex.Message}");
            CreateExceptionReport(ex.StackTrace);
            return null;
        }
    }
    public static string ReadImportPathFromConfig(string configPath)
    {
        try
        {
            // Read the XML file path from the config file
            string importFilePath = File.ReadAllText(configPath).Trim();
            return importFilePath;
        }
        catch (Exception ex)
        {
            CreateExceptionReport($"Error reading XML path from config: {ex.Message}");
            CreateExceptionReport(ex.StackTrace);
            return null;
        }
    }

    

    //window hook new 7/11

    static WinEventDelegate dele = null;
    static WinEventDelegate dele1 = null;

    public static DateTime currentDate = DateTime.Now;
    public static string currentDir = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
    //public static DateTime processDate = GetProcessDate(Directory.GetCurrentDirectory() + @"\ImportConfigs\ProcessDateXmlPath.txt");
    public static string importFileFolderPath = ReadImportPathFromConfig(Directory.GetCurrentDirectory() + @"\ImportConfigs\ImportFileFolderPath.txt");

    public static string logFolderPath = "Logs";
    public static string exceptionReportFolderPath = "ExceptionReport";

    public static string logFilePath = Path.Combine(logFolderPath, $"DailyValuation_Log_{currentDate.ToString("yyyyMMdd")}.txt");
    public static string exceptionFilePath = Path.Combine(exceptionReportFolderPath, $"ExceptionReport_Log_{currentDate.ToString("yyyyMMdd")}.txt");
    public static DateTime initialTime = DateTime.Now;
    public static DateTime finalTime;
    public static string _global_appication_error_msg = "";
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
    public static void MinimizeConsole()
    {


        const int SW_MINIMIZE = 6;

        IntPtr hWndConsole = GetConsoleWindow();
        if (hWndConsole != IntPtr.Zero)
        {
            ShowWindow(hWndConsole, SW_MINIMIZE);
        }
    }
    public static void MinimizeConsoleWindow()
    {
        // int screenCount = System.Windows.Forms.Screen.AllScreens.Length;

        //if (screenCount > 1)
        //{
        //    OpenConsoleOnScreen(screenIndex: 1);
        //}
        //else
        //{
        MinimizeConsole();
        //}

    }
    public static string GetValueFromMainFile(DataTable dt, string Name)
    {
        foreach (DataRow row in dt.Rows)
        {
            string data = row["Name"].ToString();
            if (data == Name)
            {
                return row["Value"].ToString();
            }
        }
        return null;
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
    static string pattern1 = "";
    static string pattern2 = "";
    public static string includeflag = "false";
    public static string[] includesymbols;
    public static string[] excludesymbols;
    public static bool Login(DataTable dt, string exePath)
    {
        try
        {
            int i = 0;
            Process process = Process.Start(exePath);
            Thread.Sleep(5000);
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
                // IntPtr handle = IntPtr.Zero;
                //handle = GetMainWindowHandle(process);
                //while (handle == IntPtr.Zero && i < 10)
                //{
                //    Thread.Sleep(500);
                //    handle = GetForegroundWindow();
                //    i++;
                //}
                //if (handle == IntPtr.Zero)
                //{
                //    string logMsg = "Window handle is still zero after waiting.";
                //    CreateLogs(logMsg, logFilePath);
                //    exitcode = 30;
                //    Environment.Exit(exitcode);
                //}
                //else
                //currentwin = automation.ElementFromHandle(handle);
                BringToForeground(currwin, ref currentwin, exePath);
                Thread.Sleep(500);

                while (currentwin == null || (currentwin.CurrentName != currwin))
                {
                    MaximizeWindow(currwin, ref currentwin);


                    if (currentwin != null)
                    {

                        CreateExceptionReport(currentwin.CurrentName);
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
                    CreateExceptionReport(password.CurrentAutomationId);

                    replayaction(username, loginId);
                    Thread.Sleep(200);
                    replayaction(password, loginPass);
                    Thread.Sleep(200);

                    CreateExceptionReport(currentwin.CurrentName);
                    replayaction(btn, "");
                    Thread.Sleep(200);
                }
                //handle = GetMainWindowHandle(process);
                return true;

            }
            catch (Exception ex)
            {

                //bool isApplicationClose = StopApplication(exePath);
                string logMsg = Environment.NewLine + ex.Message.ToString();
                CreateLogs(logMsg, logFilePath);
                CreateExceptionReport(ex.StackTrace);
                return false;
            }
        }
        catch (Exception ex)
        {
            CreateExceptionReport(ex.StackTrace);
            CreateExceptionReport(ex.Message);
            return false;
        }
    }
    
    public static void CreateLogs(string logMessage, string logFilePath, int dateCheck = 1)
    {

        try
        {
           // Console.WriteLine(logMessage);
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
        catch (Exception ex)
        {
            CreateExceptionReport(ex.StackTrace);
        }

    }

    public static void CreateExceptionReport(string logMessage, int dateCheck = 1)
    {

        try
        {
            if (File.Exists(exceptionFilePath))
            {
                using (StreamWriter writer = File.AppendText(exceptionFilePath))
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
                using (StreamWriter writer = File.CreateText(exceptionFilePath))
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
        catch (Exception ex)
        {
            //CreateExceptionReport(ex.StackTrace);
        }

    }

    static string exePath = "";
    static Dictionary<string ,string> tabnametoincludeflag = new Dictionary<string ,string>();
    static Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
    static Dictionary<string, Dictionary<string, string>> buttonsconfigs = new Dictionary<string, Dictionary<string, string>>();
    [STAThread]

    public static void Main(string[] args)
    {

        //Process.Start(@"C:\Users\Tarun.Gupta1\Desktop\JonesMonomoy\Prana.exe - Shortcut.lnk");
        // Your application logic loop
        string command = "";
        if (args.Length == 0)
        {
            CreateExceptionReport("Enter c for CREATION of testcases and r for RECORDING workflow ");
            command = Console.ReadLine();
        }
        else
        {
            command = args[0];
        }
        if (command == "d")
        {

            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }
            if (!Directory.Exists(exceptionReportFolderPath))
            {
                Directory.CreateDirectory(exceptionReportFolderPath);
            }
            //initialTime = DateTime.Now;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            MinimizeConsoleWindow();

            


            string mainfilePath = @"DailyValuationConfigs\MainFile.csv";
           string importDataFilePath = @"DailyValuationConfigs\DV_DataFile.csv";

           // DataTable dataFromCsvFile = DataFromCsvFile(importDataFilePath);
            DataTable mainFileData = DataFromCsvFile(mainfilePath);
           /// int configRowCount = dataFromCsvFile.Rows.Count;    
             exePath = GetValueFromMainFile(mainFileData, "exePath");
            pattern1 = GetValueFromMainFile(mainFileData, "regexforfxlusd");
            pattern2 = GetValueFromMainFile(mainFileData, "regexforfxgusd");
            string flagconfig = GetValueFromMainFile(mainFileData, "Includeflag");
            string includesymbol = GetValueFromMainFile(mainFileData, "Includesymbols");
            string excludesymbol = GetValueFromMainFile(mainFileData, "Excludesymbols");
            string configString = GetValueFromMainFile(mainFileData, "FilterConfig");
            string tabconfigstring = GetValueFromMainFile(mainFileData, "TabConfig");
            var categories1 = tabconfigstring.Split(';');
            foreach (var category in categories1)
            {
                // Split each category into category name and data
                var parts = category.Split('#');
                if (parts.Length == 2)
                {
                    var categoryName = parts[0];
                    var data = parts[1];

                    var dataDict = new Dictionary<string, string>();

                    // Split the data into key-value pairs
                    var items = data.Split('!');
                    foreach (var item in items)
                    {
                        var kvp = item.Split('|');
                        if (kvp.Length == 2)
                        {
                            var key = kvp[0]; // Convert key to lowercase
                            var value = kvp[1];
                            dataDict[key] = value;
                        }
                    }
                    
                    buttonsconfigs[categoryName] = dataDict;
                }
            }


            var categories = configString.Split(';');
            foreach (var category in categories)
            {
                // Split each category into category name and data
                var parts = category.Split('#');
                if (parts.Length == 2)
                {
                    var categoryName = parts[0];
                    var data = parts[1];

                    var dataDict = new Dictionary<string, string>();

                    // Split the data into key-value pairs
                    var items = data.Split('!');
                    foreach (var item in items)
                    {
                        var kvp = item.Split('|');
                        if (kvp.Length == 2)
                        {
                            var key = kvp[0].ToLower(); // Convert key to lowercase
                            var value = kvp[1];
                            dataDict[key] = value;
                        }
                    }

                    result[categoryName] = dataDict;
                }
            }



            string[] arr = flagconfig.Split(',');
            foreach(string s in arr)
            {
                string[] arr1 = s.Split('|');
                if (arr1.Length == 2)
                {
                    string tabname = arr1[0];
                    string flag = arr1[1];
                    if(!tabnametoincludeflag.ContainsKey(tabname))
                    tabnametoincludeflag.Add(tabname, flag);
                }
                
            }
           //string tabnameforincludeconfig=flagconfig.split
            includesymbols = includesymbol.Split(',');
            excludesymbols = excludesymbol.Split(',');
            //Process.Start(exePath);
            Thread.Sleep(4000);
            try
            {
                DV_YahooPrice(mainFileData, exePath);
                //importreplayer(dataFromCsvFile, mainFileData, exePath);

                //Console.Clear();

                finalTime = DateTime.Now;

                TimeSpan timeDifference = finalTime - initialTime;
                Console.WriteLine("");
                Console.WriteLine("");
                string totalTimeTaken = timeDifference.Hours + " Hours " + timeDifference.Minutes + " Minutes " + timeDifference.Seconds + " Seconds " + timeDifference.Milliseconds + " Milliseconds ";
                Console.WriteLine("Total Time Taken : " + totalTimeTaken);

                string timeLogMessage = "Total Time Taken : " + totalTimeTaken;
                CreateLogs(timeLogMessage, logFilePath, 0);

                
                RestoreConsoleWindow();
                //Console.WriteLine(successCounter);
                
                

                string logMsg = "*************************************************************SUCCESS!!*************************************************************";
                CreateLogs(logMsg, logFilePath, 0);

                


            }


            catch (Exception ex)
            {
                CreateExceptionReport("Exception : " + ex.Message);
                CreateExceptionReport(ex.StackTrace);
            }

            finally
            {

            }

            // Console.ReadLine();



        }


        else
        {
            CreateExceptionReport("Please give a valid input ");
            exitcode = 5;
        }
        string errcodemsg = "ExitCode :  " + exitcode + @"  Please Refer Exit Error Code File For More Information";
        CreateLogs(errcodemsg, logFilePath, 0);
        // Console.ReadLine();

        //Console.WriteLine(exitcode);
        //Console.ReadLine();
        Environment.Exit(exitcode);


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
            //if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_RadioButtonControlTypeId)
            //{
            //    //string value = null;
            //    object patternprovider;
            //    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) != null)
            //    {
            //        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
            //        CreateExceptionReport(targetelement.CurrentName);

            //        IUIAutomationTogglePattern selectionpatternprovider = patternprovider as IUIAutomationTogglePattern;
            //        //Thread modalThread = new Thread(HandleModalDialog);
            //        //modalThread.Start();
            //        selectionpatternprovider.Toggle();
            //        //modalThread.Join();


            //    }
            //    return;
            //}
            if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
            {
                string value = null;
                object patternprovider;
                if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                {
                    patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                    CreateExceptionReport(targetelement.CurrentName);

                    IUIAutomationInvokePattern selectionpatternprovider = patternprovider as IUIAutomationInvokePattern;
                    //Thread modalThread = new Thread(HandleModalDialog);
                    //modalThread.Start();
                    selectionpatternprovider.Invoke();
                    //modalThread.Join();


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
                IUIAutomation automation1 = new CUIAutomation8();

                IUIAutomationCondition bttn = automation1.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);

                IUIAutomationElement btnelement = targetelement.FindFirst(TreeScope.TreeScope_Children, bttn);
                //if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                //{
                //    patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                //    IUIAutomationValuePattern selectionpatternprovider = patternprovider as IUIAutomationValuePattern;
                //    selectionpatternprovider.SetValue(value);

                //}
                if (btnelement != null)
                {
                    Console.WriteLine("Found button for combo box");
                    ClickElement(btnelement, "Left Mouse Button Clicked");
                    Thread.Sleep(2000);


                    IUIAutomationCondition selectOptioncond1 = automation1.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, value);
                    IUIAutomationCondition selectOptioncond2 = automation1.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);

                    IUIAutomationCondition selectOptioncondand = automation1.CreateAndCondition(selectOptioncond1, selectOptioncond2);

                    IUIAutomationElement listelement = _globalcurrentwin.FindFirst(TreeScope.TreeScope_Descendants, selectOptioncondand);
                    if (listelement != null)
                    {

                        CreateExceptionReport(listelement.CurrentName);
                        ClickElement(listelement, "Left Mouse Button Clicked");
                        Thread.Sleep(500);

                    }
                    return;

                    //ClickElement(btnelement, "Left Mouse Button Clicked");
                }

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
            // exitcode = 23;
            CreateExceptionReport(ex.StackTrace);
            return;
        }
    }
    public static void BringToForeground(string currwin, ref IUIAutomationElement currentwin, string exePath)
    {
        try
        {
            // Check if Calculator is already open
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
                       else if (f == false)
            {

                var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");
                var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, currwin);


                var wincond = automation.CreateAndCondition(cond1, cond2);

                element = root.FindFirst(TreeScope.TreeScope_Descendants, wincond);

            }
            if (element != null)
            {
                currentwin = element;
                CreateExceptionReport(currentwin.CurrentName);
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
                                //windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Maximized);

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
            //else
            //{
            //    // Calculator is not open, start a new instance
            //    //if (exepaths.ContainsKey(currwin))
            //    //{
            //    //    Process.Start(exepaths[currwin]);

            //    //}
            //}
        }
        catch (Exception ex)
        {
            CreateExceptionReport(ex.Message);
            CreateExceptionReport(ex.StackTrace);
            CreateExceptionReport("exception due to process");
        }
    }
    public static void MaximizeWindow(string currwin, ref IUIAutomationElement currentwin)
    {
        try
        {


            // Check if Calculator is already open
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
                CreateExceptionReport(currentwin.CurrentName);
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
            else
            {
                // Calculator is not open, start a new instance
                //if (exepaths.ContainsKey(currwin))
                //{
                //    Process.Start(exepaths[currwin]);

                //}
            }
        }
        catch (Exception ex)
        {
            string logMsg = Environment.NewLine + ex.Message.ToString();
            CreateExceptionReport(ex.StackTrace);
            CreateLogs(logMsg, logFilePath);

        }
    }

    //public static void MinimizeWindow(string currwin, ref IUIAutomationElement currentwin)
    //{
    //    try
    //    {
    //        // Check if Calculator is already open
    //        if (currwin == "")
    //        {
    //            return;
    //        }

    //        IUIAutomation automation = new CUIAutomation8();
    //        var root = automation.GetRootElement();
    //        IUIAutomationElement element = null;
    //        bool f = false;
    //        if (currentwin != null && currentwin.CurrentName == currwin)
    //        {
    //            f = true;
    //            element = currentwin;
    //        }
    //        if (f == false && currwin.Contains("Trading Ticket"))
    //        {
    //            currwin = ".*Trading Ticket.*";
    //            // Create an AutomationElement representing the root element of the desktop

    //            // Define the regular expression for matching "Trading Ticket" anywhere in the name
    //            Regex regex = new Regex(".*Trading Ticket.*", RegexOptions.IgnoreCase);

    //            // Define the condition for the element you want to find
    //            var condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");

    //            // Find all elements that match the control type condition
    //            var allElements = root.FindAll(TreeScope.TreeScope_Descendants, condition);

    //            // Find the first element that matches the name using the regular expression

    //            for (int i = 0; i < allElements.Length; i++)
    //            {
    //                var currentElement = allElements.GetElement(i);
    //                if (regex.IsMatch(currentElement.CurrentName))
    //                {
    //                    element = currentElement;
    //                    break;
    //                }
    //            }

    //            // Check if the element is found
    //            if (element != null)
    //            {
    //                // Do something with the found element
    //                CreateExceptionReport("Element found: " + element.CurrentName);
    //            }
    //            else
    //            {
    //                CreateExceptionReport("Element not found.");
    //            }

    //        }
    //        else if (f == false)
    //        {

    //            var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");
    //            var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, currwin);


    //            var wincond = automation.CreateAndCondition(cond1, cond2);

    //            element = root.FindFirst(TreeScope.TreeScope_Descendants, wincond);

    //        }




    //        if (element != null)
    //        {
    //            currentwin = element;
    //            CreateExceptionReport(currentwin.CurrentName);
    //            object patternprovider;
    //            // Calculator is already open, bring it to the foreground
    //            if (element.GetCurrentPattern(UIA_PatternIds.UIA_WindowPatternId) != null)
    //            {
    //                patternprovider = element.GetCurrentPattern(UIA_PatternIds.UIA_WindowPatternId);
    //                IUIAutomationWindowPattern windowpatternprovider = patternprovider as IUIAutomationWindowPattern;
    //                WindowVisualState calstate = windowpatternprovider.CurrentWindowVisualState;
    //                if (calstate != null)
    //                {
    //                    if (calstate == WindowVisualState.WindowVisualState_Minimized)
    //                    {

    //                    }
    //                    else if (calstate == WindowVisualState.WindowVisualState_Normal)
    //                    {
    //                        if (windowpatternprovider.CurrentCanMinimize == 1)
    //                        {

    //                            windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Minimized);
    //                        }
    //                    }
    //                    else if (calstate == WindowVisualState.WindowVisualState_Maximized)
    //                    {
    //                        if (windowpatternprovider.CurrentCanMinimize == 1)
    //                        {
    //                            windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Minimized);
    //                        }
    //                        Thread.Sleep(100);
    //                    }

    //                }
    //            }
    //        }
    //        else
    //        {
    //            // Calculator is not open, start a new instance
    //            //if (exepaths.ContainsKey(currwin))
    //            //{
    //            //    Process.Start(exepaths[currwin]);

    //            //}
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string logMsg = Environment.NewLine + ex.Message.ToString();
    //        CreateExceptionReport(ex.StackTrace);
    //        CreateLogs(logMsg, logFilePath);

    //    }
    //}

    private static void ClickElement(IUIAutomationElement element, string clicktype)
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

            //while (true)
            //{
            //    simulator.Mouse.MoveMouseTo(-85, -y);

            //}
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






    

    //private static void CreateLogs(string logMessage)
    //{
    //    try
    //    {
    //        // Append the log message to the file
    //        if (_keyboardpressed)
    //        {
    //            using (StreamWriter writer = File.AppendText(logFilename))
    //            {
    //                logMessage += " ";
    //                writer.Write(logMessage);
    //            }
    //        }
    //        else
    //        {
    //            using (StreamWriter writer = File.AppendText(logFilename))
    //            {
    //                writer.WriteLine(logMessage);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        CreateExceptionReport(ex.StackTrace);
    //        // If there is an error, display the error message
    //        CreateExceptionReport("Error writing to log file: " + ex.Message);
    //    }
    //}





    private static string GetWindowText(IntPtr hwnd)
    {
        int length = User32Interop.GetWindowTextLength(hwnd);
        if (length > 0)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(length + 1);

            if (hwnd != null)
            {
                User32Interop.GetWindowText(hwnd, sb, sb.Capacity);
            }

            return sb.ToString();
        }
        return string.Empty;
    }





    private static IUIAutomationElement FindElementAtPoint(int x, int y)
    {
        IUIAutomationElement element = null;
        IUIAutomation automation = new CUIAutomation8();
        try
        {

            element = automation.ElementFromPoint(new tagPOINT() { x = (int)x, y = (int)y });
            //CreateExceptionReport(element.CurrentName);
            // CreateExceptionReport(element.CurrentAutomationId);
        }
        catch (Exception ex)
        {
            CreateExceptionReport(ex.StackTrace);
            CreateExceptionReport(ex.Message);
        }
        return element;

    }
    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x;
        public int y;
    }
    

  

    


    
    private static string getgridname(IUIAutomationElement targetelement)
    {
        if (targetelement.CurrentAutomationId.Contains("Grid") || targetelement.CurrentAutomationId.Contains("grd") || targetelement.CurrentAutomationId.Contains("board"))
        {
            return targetelement.CurrentAutomationId;
        }

        IUIAutomation automation = new CUIAutomation8();

        IUIAutomationElement pp = automation.ContentViewWalker.GetParentElement(targetelement);
        if (pp != null)
        {
            return getgridname(pp);
        }

        return "Grid not found";

    }

    


    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);

    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private delegate IntPtr CBTProc(int nCode, IntPtr wParam, IntPtr lParam);

    //new imports 
    // COM Initialization flags
    [DllImport("ole32.dll")]
    private static extern int CoInitializeEx(IntPtr pvReserved, COINIT dwCoInit);

    // COM Uninitialization
    [DllImport("ole32.dll")]
    private static extern void CoUninitialize();

    // COM Initialization constants
    [Flags]
    private enum COINIT : uint
    {
        COINIT_APARTMENTTHREADED = 0x2,
        COINIT_MULTITHREADED = 0x0,
        COINIT_DISABLE_OLE1DDE = 0x4,
        COINIT_SPEED_OVER_MEMORY = 0x8
    }
 
    
    private static readonly CUIAutomation8 _automation = new CUIAutomation8();
    private class AutomationPropertyChangedEventHandler : IUIAutomationPropertyChangedEventHandler
    {
        public AutomationPropertyChangedEventHandler(Action<IUIAutomationElement, int, object> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            Action = action;
        }

        public Action<IUIAutomationElement, int, object> Action { get; }
        public void HandlePropertyChangedEvent(IUIAutomationElement sender, int propertyId, object newValue) => Action(sender, propertyId, newValue);
    }

   
    static bool _flag = true;




    
    

    private static string getnewvalue(IUIAutomationElement targetelement)
    {
        Stopwatch sw1 = new Stopwatch();
        sw1.Start();
        string valuetouse = "";
        if (targetelement == null)
        {
            return valuetouse;
        }
        string elementtoget = targetelement.CurrentAutomationId;

        if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_ComboBoxControlTypeId)
        {
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            valuetouse = "";
            IUIAutomation automation = new CUIAutomation8();
            IUIAutomationCondition lstitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
            //IUIAutomationElementArray lstelement = targetelement.FindAll(
            //   TreeScope.TreeScope_Children, lstitem);
            IUIAutomationElementArray lstelement = targetelement.FindAll(
               TreeScope.TreeScope_Descendants, lstitem);


            if (lstelement != null)
            {
                if (lstelement.Length == 1)
                {
                    IUIAutomationElement listitem = lstelement.GetElement(0);
                    string listitemText = listitem.GetCurrentPropertyValue(
                       UIA_PropertyIds.UIA_NamePropertyId).ToString();
                    valuetouse = listitem.CurrentName;
                }
                else
                {
                    for (int i = 0; i < lstelement.Length; i++)
                    {
                        IUIAutomationElement listitem = lstelement.GetElement(i);
                        string listitemText = listitem.GetCurrentPropertyValue(
                           UIA_PropertyIds.UIA_NamePropertyId).ToString();
                        //CreateExceptionReport(listitemText);
                        object selectPatternObj;
                        selectPatternObj = listitem.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);

                        IUIAutomationSelectionItemPattern selectPattern = selectPatternObj as IUIAutomationSelectionItemPattern;

                        if (selectPattern != null)
                        {
                            // Extract the value from the ComboBox
                            int _isselected = selectPattern.CurrentIsSelected;
                            if (_isselected == 1)
                            {
                                valuetouse = listitem.CurrentName;
                            }
                            //CreateExceptionReport($"ComboBox value: {comboBoxValue}");

                        }
                    }
                }
            }
            sw2.Stop();
            long etime = sw2.ElapsedMilliseconds;
            // CreateExceptionReport("time taken for one combobox " + etime);
        }
        else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_CheckBoxControlTypeId)
        {
            try
            {
                string value = null;
                object patternprovider;
                if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) != null)
                {
                    patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                    IUIAutomationTogglePattern selectionpatternprovider = patternprovider as IUIAutomationTogglePattern;
                    value = selectionpatternprovider.CurrentToggleState.ToString();
                    string togglestate = selectionpatternprovider.CurrentToggleState.ToString();
                    //CreateExceptionReport(selectionpatternprovider.CurrentToggleState.ToString());
                    valuetouse = togglestate;
                }
            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.StackTrace);
                CreateExceptionReport(ex.Message);

            }

        }
        else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_RadioButtonControlTypeId)
        {
            valuetouse = targetelement.CurrentName;

        }
        else if (targetelement.CurrentAutomationId == "MultiSelectEditor")
        {
            //  CreateExceptionReport("inside multi select ");

            IUIAutomation automation2 = new CUIAutomation8();
            IUIAutomationCondition bttn = automation2.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
            IUIAutomationElement btnelement = targetelement.FindFirst(TreeScope.TreeScope_Descendants, bttn);


            if (btnelement != null)
            {
                // CreateExceptionReport("Found Button element ");
                //expand pattern invoke
                try
                {
                    string value = null;
                    object patternprovider;
                    if (btnelement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) != null)
                    {
                        patternprovider = btnelement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
                        // CreateExceptionReport("inside expand pattern");
                        IUIAutomationExpandCollapsePattern selectionpatternprovider = patternprovider as IUIAutomationExpandCollapsePattern;
                        value = selectionpatternprovider.CurrentExpandCollapseState.ToString();
                        //CreateExceptionReport("......////...........");
                        //   CreateExceptionReport(selectionpatternprovider.CurrentExpandCollapseState.ToString());
                        selectionpatternprovider.Expand();
                        //  CreateExceptionReport(selectionpatternprovider.CurrentExpandCollapseState.ToString());
                        IUIAutomation automation1 = new CUIAutomation8();
                        IUIAutomationCondition listt = automation1.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "checkedMultipleItems");
                        IUIAutomationElement listelement = _currwinglobal.FindFirst(TreeScope.TreeScope_Descendants, listt);
                        if (listelement != null)
                        {
                            // Condition checkboxitem = new PropertyCondition(AutomationElement.ControlTypeProperty,listelement);
                            // AutomationElementCollection checkboxes = listelement.FindAll(TreeScope.Children, checkboxitem);
                            IUIAutomation automation = new CUIAutomation8();

                            IUIAutomationCondition conditiont = automation.CreateTrueCondition();

                            IUIAutomationElementArray checkboxes = listelement.FindAll(TreeScope.TreeScope_Descendants, conditiont);
                            //CreateExceptionReport(checkboxes[0].Current.Name);
                            //   CreateExceptionReport("SIZE OF List" + checkboxes.Length);

                            if (checkboxes != null)
                            {
                                // Console.Write("inside checkboxes ");
                                for (int i = 0; i < checkboxes.Length; i++)
                                {
                                    IUIAutomationElement checkbox = checkboxes.GetElement(i);
                                    //  CreateExceptionReport(checkbox.CurrentName + "..");
                                    try
                                    {
                                        // IUIAutomationValuePattern valuePattern = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                                        // IUIAutomationTogglePattern togglePattern = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;
                                        //string value = null;
                                        object patternproviderrr;
                                        if (checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) != null)
                                        {
                                            patternproviderrr = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                                            IUIAutomationTogglePattern chkboxpatternprovider = patternproviderrr as IUIAutomationTogglePattern;
                                            // value = selectionpatternprovider.Current.ToString();
                                            // CreateExceptionReport("......////...........");
                                            //  CreateExceptionReport(chkboxpatternprovider.ToString());
                                            //   CreateExceptionReport(chkboxpatternprovider.CurrentToggleState.ToString());
                                            if (chkboxpatternprovider.CurrentToggleState.ToString() == "ToggleState_On")
                                            {
                                                valuetouse += ("," + checkbox.CurrentName);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    CreateExceptionReport(ex.StackTrace);
                    CreateExceptionReport(ex.Message);

                }

            }
        }
        else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_EditControlTypeId)
        {
            try
            {
                IUIAutomationValuePattern valuePattern = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                if (valuePattern != null)
                {
                    valuetouse = valuePattern.CurrentValue;
                    // CreateExceptionReport(valuetouse);
                }
            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.StackTrace);
                CreateExceptionReport("INSIDE EDIT ITEM CANNOT GET VALUE");
                CreateExceptionReport(ex.Message);

            }

        }
        // have to map this id to proper name 
        // this will give a list of string 

        else
        {
            string elementValue = "";
            try
            {
                IUIAutomationValuePattern valuePattern = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                if (valuePattern != null)
                {
                    elementValue = valuePattern.CurrentValue;
                    
                }
            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.StackTrace);
                CreateExceptionReport(ex.Message);

                // Handle exceptions as needed
            }
            string selectedText = "";
            try
            {
                IUIAutomationSelectionPattern selectionPattern = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_SelectionPatternId) as IUIAutomationSelectionPattern;
                if (selectionPattern != null)
                {
                    IUIAutomationElement selectedItem = selectionPattern.GetCurrentSelection().GetElement(0);
                    selectedText = selectedItem.CurrentName;

                }
            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.StackTrace);
                CreateExceptionReport(ex.Message);

                // Handle exceptions as needed
            }
            //CreateExceptionReport("checking");
            //CreateExceptionReport(targetelement.Current.AutomationId  +"    " +   elementValue);
            //CreateExceptionReport(targetelement.Current.AutomationId + "    " + selectedText);    
            if (elementValue != "")
            {
                valuetouse = elementValue;
            }
            else
            {
                valuetouse = selectedText;
            }
        }
        return valuetouse;
    }
   

    

    

    static bool _keyboardpressed = false;
    private static bool _donotupdate = false;
    private static List<System.Windows.Forms.Keys> _keystore = new List<System.Windows.Forms.Keys>();


    private const int WH_MOUSE_LL = 14;
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;

    

    
    
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr WindowFromPoint(POINT point);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
       CBTProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr GetFocus();
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int GetClassName(IntPtr hWnd, System.Text.StringBuilder lpClassName, int nMaxCount);


}