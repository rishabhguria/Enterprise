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
using System.Management;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

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
            Console.WriteLine("Error extracting FilterValue: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
        return DateTime.MinValue;
    }
}


    class InterceptMouseAndKeyboard
{
    private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    private const uint EVENT_OBJECT_CREATE = 0x8000;

    private static string prevwin = "";

    // NEW IMPLEMENTATION DICTIONARIES 
    static Dictionary<KeyValuePair<string, string>, Dictionary<string, List<string>>> dictmap = new Dictionary<KeyValuePair<string, string>, Dictionary<string, List<string>>>();
    static Dictionary<KeyValuePair<string, string>, Dictionary<string, string>> dictmapalldata = new Dictionary<KeyValuePair<string, string>, Dictionary<string, string>>();
    static Dictionary<string, string> map = new Dictionary<string, string>();

    IUIAutomationElement _globalelement = null;
    IUIAutomationElement _globalcurrentwin = null;


   
    static int rownum = 6;
    // static Dictionary<string , List<string>> _moduleactionelements=new Dictionary<string , List<string>>();
    static Dictionary<string, string> _stepnamecorrection = new Dictionary<string, string>();

    //cmborderside,<"1":buy>,<"2":sell
    static Dictionary<string, Dictionary<string, string>> _correctioncolumn = new Dictionary<string, Dictionary<string, string>>();
    static Dictionary<string, int> stepnametoindex = new Dictionary<string, int>();
    
    static Dictionary<string, AutomationPropertyChangedEventHandler> _eventhandledict = new Dictionary<string, AutomationPropertyChangedEventHandler>();
    static Dictionary<string, Dictionary<string, string>> _moduletoelementvalue = new Dictionary<string, Dictionary<string, string>>();

    //static Dictionary<KeyValuePair<string, string>, IUIAutomationElement> _elementtoiuireference = new Dictionary<KeyValuePair<string, string>, IUIAutomationElement>();

    static string _currenttabname = "";
    static Dictionary<string, List<string>> _tabname_to_stepname = new Dictionary<string, List<string>>();
    private static readonly object _fileLock = new object();

    //<createorder,<txtsymbol,cmbquantity>
    //    <verifyordergrid,<"ordersblottergrid">

    //_actiontostepname cr
    static Dictionary<string, List<string>> _btntostepname = new Dictionary<string, List<string>>();
    private static Dictionary<string, List<string>> mappingCorrection = new Dictionary<string, List<string>>();

    private static string logFilename;

    static Dictionary<string, List<string>> alldata = new Dictionary<string, List<string>>();
    static Dictionary<string, string> exepaths = new Dictionary<string, string>();
    static IUIAutomationElement _currwinglobal = null;
    static IUIAutomationElement WindowGlobal = null;
    static string retry_abort = "yes";
    static int importcount = 0;
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

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);


    [DllImport("user32.dll", SetLastError = true)]
    static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    const uint MOUSEEVENTF_LEFTUP = 0x0004;


    private static IntPtr _mouseHookID = IntPtr.Zero;
    private static IntPtr _keyboardHookID = IntPtr.Zero;
    //  IntPtr hook = SetWinEventHook(EVENT_OBJECT_CREATE, EVENT_OBJECT_CREATE, IntPtr.Zero, WinEventCallback, 0, 0, 0);
    private static IntPtr _mainWindowHandle = IntPtr.Zero;
    private static bool _filecreated = false;
    private static string _filename = "";
    private static string _columnname = "";
    private static string _filterbasis = "";
    private static bool _filteron = false;
    static string _filterstep = "";
    static bool _uiInteraction = true;

    static bool screenshotModeIsOn = false;
    static bool _globalApplicationerror = false;
    static bool finalImportresult = false;
    static bool application_running = false;
    static int successCounter = 0;
    static int totalRecons = 0;

    static int exitcode = -1;
    //window hook new 7/11
    //delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
    // [DllImport("user32.dll")]
    //static extern bool UnhookWinEvent(IntPtr hWinEventHook);

    //[DllImport("user32.dll")]
    // static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    private const uint WINEVENT_OUTOFCONTEXT = 0;
    private const uint EVENT_SYSTEM_FOREGROUND = 3;

    // [DllImport("user32.dll")]
    // static extern IntPtr GetForegroundWindow();

    // [DllImport("user32.dll")]
    //static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
    
    private static IUIAutomationElementArray findiuielementofrows1(IUIAutomationElement currwin)
    {
        List<KeyValuePair<int, IUIAutomationElement>> uIAutomationElements = new List<KeyValuePair<int, IUIAutomationElement>>();

        KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("Import Data", "Import Data");

        IUIAutomation automation = new CUIAutomation8();
        IUIAutomationCondition condition1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TreeControlTypeId);

        IUIAutomationElement grid = currwin.FindFirst(
           TreeScope.TreeScope_Descendants, condition1);

        IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "[Editor] Edit Area");
        IUIAutomationCondition conditionditems = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
        IUIAutomationCondition conditioncols = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderItemControlTypeId);
        IUIAutomationElementArray dataitems = null;
        if (grid != null)
        {

            IUIAutomationElementArray rowelements = grid.FindAll(TreeScope.TreeScope_Descendants, condition);
             dataitems = grid.FindAll(TreeScope.TreeScope_Descendants, conditionditems);


        }
        return dataitems;
    }
        private static List<KeyValuePair<int, IUIAutomationElement>> findiuielementofrows(IUIAutomationElement currwin)
    {
        List<KeyValuePair<int, IUIAutomationElement>> uIAutomationElements = new List<KeyValuePair<int, IUIAutomationElement>>();

        KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("Import Data", "Import Data");

        IUIAutomation automation = new CUIAutomation8();
        IUIAutomationCondition condition1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TreeControlTypeId);

        IUIAutomationElement grid = currwin.FindFirst(
           TreeScope.TreeScope_Descendants, condition1);

        IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "[Editor] Edit Area");
        IUIAutomationCondition conditionditems = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
        IUIAutomationCondition conditioncols = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderItemControlTypeId);

        if (grid != null)
        {

            IUIAutomationElementArray rowelements = grid.FindAll(TreeScope.TreeScope_Descendants, condition);
            IUIAutomationElementArray dataitems = grid.FindAll(TreeScope.TreeScope_Descendants, conditionditems);


            List<string> checkcols = new List<string>();


            if (dataitems != null)

            {


                string parentElement = "";
                string realvalue = "";
                for (int i = 0; i < dataitems.Length; i++)
                {
                    bool cond1 = false;
                    bool cond2 = false;
                    bool cond3 = false;
                    if (uIAutomationElements.Count == dictmap[keyValuePair]["Import Type"].Count)
                    {
                        break;
                    }

                    IUIAutomationCondition uIAutomationCondition = automation.CreateTrueCondition();

                    IUIAutomationElement e = dataitems.GetElement(i);
                    IUIAutomationElementArray complexelements = e.FindAll(TreeScope.TreeScope_Children, uIAutomationCondition);
                    if (complexelements != null)

                    {
                        int idx = -1;
                        for (int j = 0; j < complexelements.Length; j++)
                        {

                            IUIAutomationElement cell = complexelements.GetElement(j);
                            realvalue = getnewvalue(cell);//value of cell

                            parentElement = cell.CurrentName;//column header

                            if (parentElement == "Upload ThirdParty")
                            {
                                idx = dictmap[keyValuePair]["Upload ThirdParty"].IndexOf(realvalue);
                                if (idx == -1)
                                {
                                    break; // break from row 
                                }
                                else
                                {
                                    cond1 = true;
                                }
                            }


                            if (parentElement == "Import Type" && cond1)
                            {
                                if (dictmap[keyValuePair]["Import Type"][idx] == realvalue)
                                {
                                    cond2 = true;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (parentElement == "Import Type Format Name" && cond2)
                            {
                                if (dictmap[keyValuePair]["Import Type Format Name"][idx] == realvalue)
                                {
                                    cond3 = true;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (cond1 && cond2 && cond3)
                            {
                                IUIAutomationElement parentrow = automation.ContentViewWalker.GetParentElement(cell);
                                if (parentrow != null)
                                {
                                    uIAutomationElements.Add(new KeyValuePair<int, IUIAutomationElement>(idx, parentrow));
                                }
                                break;
                            }

                        }
                    }


                }

            }



        }
        return uIAutomationElements;
    }
    public static void OpenReconModule()
    {


        InputSimulator inputSimulator = new InputSimulator();
        inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
        inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.MENU);
        inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_E);
        inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.MENU);
        inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
    }
   
    public static void HandleInputFileName()
    {
        Thread.Sleep(1000);
        string logMsg = "";
        IUIAutomation automation = new CUIAutomation8();
        Console.WriteLine("after Open button");
        InputSimulator sim = new InputSimulator();
        try
        {
            IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Select File to Import");
            IUIAutomationElement dialogbox = WindowGlobal.FindFirst(TreeScope.TreeScope_Descendants, conddialogbox);
            IUIAutomationCondition conderrorbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Exception Report");
            IUIAutomationElement errorbox = WindowGlobal.FindFirst(TreeScope.TreeScope_Descendants, conderrorbox);
            if (dialogbox != null)
            {
                IUIAutomationCondition conderrormsg = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId,UIA_ControlTypeIds.UIA_TextControlTypeId );
                IUIAutomationElement errormsgelement = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, conderrormsg);
                Console.WriteLine("Inside Modal   " + dialogbox.CurrentName);
                Console.WriteLine("ERROR MESSAGE : " + errormsgelement.CurrentName);
                logMsg += Environment.NewLine + "ERROR MESSAGE : " + errormsgelement.CurrentName;
                CreateLogs(logMsg, logFilePath);
                IUIAutomationCondition condokbttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "OK");
                IUIAutomationElement okbttnelement = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, condokbttn);
                replayaction(okbttnelement, "");
            }
            if (errorbox != null)
            {
                IUIAutomationCondition conderrormsg = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TextControlTypeId);
                IUIAutomationElement errormsgelement = errorbox.FindFirst(TreeScope.TreeScope_Descendants, conderrormsg);
                Console.WriteLine("Inside Modal   " + dialogbox.CurrentName);
                Console.WriteLine("ERROR MESSAGE : " + errormsgelement.CurrentName);
                logMsg += "ERROR MESSAGE : " + errormsgelement.CurrentName;
                CreateLogs(logMsg, logFilePath);
                IUIAutomationCondition condokbttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "OK");
                IUIAutomationElement okbttnelement = errorbox.FindFirst(TreeScope.TreeScope_Descendants, condokbttn);
                replayaction(okbttnelement, "");

            }

        }
        catch(Exception ex) {
            Console.WriteLine(ex.Message.ToString());
            Console.WriteLine(ex.StackTrace);
        }
    }
    public static void HandleExceptionReport()
    {
        Thread.Sleep(2000);
        string logMsg = "";
        IUIAutomation automation = new CUIAutomation8();
        Console.WriteLine("after Cancel button");
        InputSimulator sim = new InputSimulator();
        try
        {
            IUIAutomationCondition conderrorbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Exception Report");
            IUIAutomationElement errorbox = WindowGlobal.FindFirst(TreeScope.TreeScope_Descendants, conderrorbox);
            if (errorbox != null)
            {
                IUIAutomationCondition conderrormsg = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TextControlTypeId);
                IUIAutomationElement errormsgelement = errorbox.FindFirst(TreeScope.TreeScope_Descendants, conderrormsg);
                Console.WriteLine("Inside Modal   " + errorbox.CurrentName);
                Console.WriteLine("ERROR MESSAGE : " + errormsgelement.CurrentName);
                //logMsg += "ERROR MESSAGE : " + errormsgelement.CurrentName;
                //CreateLogs(logMsg, logFilePath);
                IUIAutomationCondition condokbttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "OK");
                IUIAutomationElement okbttnelement = errorbox.FindFirst(TreeScope.TreeScope_Descendants, condokbttn);
                replayaction(okbttnelement, "");

            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            Console.WriteLine(ex.StackTrace);
        }
    }
        public static void HandleModalDialog() {

        


        IUIAutomation automation = new CUIAutomation8();


        Console.WriteLine("after upload button");
        //IUIAutomationCondition ImportPositionsDisplayFormwindowcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ImportPositionsDisplayForm");
        //IUIAutomationElement ImportPositionsDisplayFormwindow = _currwinglobal.FindFirst(TreeScope.TreeScope_Descendants, ImportPositionsDisplayFormwindowcond);
        //Console.WriteLine(ImportPositionsDisplayFormwindow.CurrentName);
        ////IntPtr foregroundwinhandle = GetForegroundWindow();

        ////string foregroundwin = GetWindowText(foregroundwinhandle);

        ////BringToForeground(foregroundwin,ref _currwinglobal);
        //Console.WriteLine(_currwinglobal.CurrentName);
        InputSimulator sim=new InputSimulator();
        //_globalApplicationerror = true;
        //sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
        //string logMsg = "Application Error";
        //CreateLogs(logMsg, logFilePath);
        try
        {
            Thread.Sleep(2000);
            
            
            IUIAutomationCondition applicationErrorcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Application Error");
            //IntPtr handle = IntPtr.Zero;
            ////StringBuilder Buff = new StringBuilder(nChars);
            //handle = GetForegroundWindow();
            //IUIAutomationElement currentwin = null;
            //string foregrounwin = GetWindowText(handle);
            //IUIAutomationElement currwin= null;
            //BringToForeground(foregrounwin, ref currwin);
            Console.WriteLine(_currwinglobal.CurrentName + "Inside modal");
            IUIAutomationElement applicationError = _currwinglobal.FindFirst(TreeScope.TreeScope_Element | TreeScope.TreeScope_Descendants, applicationErrorcond);
            Thread.Sleep(500);
            IUIAutomationCondition appErrorMsgcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "65535");
            IUIAutomationCondition dialogboxcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId,"dialog");


            if (applicationError != null)
            {
            IUIAutomationElement appErrorMsg = applicationError.FindFirst(TreeScope.TreeScope_Descendants, appErrorMsgcond);
            Thread.Sleep(500);
                _global_appication_error_msg = applicationError.CurrentName + " : ";
                if (appErrorMsgcond != null) { 
                
                _global_appication_error_msg += appErrorMsg.CurrentName;
                }
                
               


                _globalApplicationerror = true;
                Console.WriteLine("before pressing enter");
                
                
                
                sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);


                Console.WriteLine("after pressing enter");
                Thread.Sleep(2000);
                IUIAutomationElement dialogbox = _currwinglobal.FindFirst(TreeScope.TreeScope_Descendants, appErrorMsgcond);
                //65535
                if (dialogbox != null)
                {
                    sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                }

            }
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error inside Modal");   
            string exceptionLog = ex.Message.ToString();
            CreateLogs(exceptionLog, logFilePath);
            Console.WriteLine(ex.StackTrace);


        }

        //sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
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

    public static string GetFormattedDateTime(string format)
    {
        return processDate.ToString(format);
    }

    public static void ReconReplayer(DataTable dataFromCsvFile, DataTable mainFileData, string exePath)
    {
        try
        {
            int i = 0;
            
            if (!application_running)
            {
                bool isLogin = Login(mainFileData, exePath);
                //todo: have to remove this using check for foreground window 
                Thread.Sleep(5000);
                if (isLogin == false) return;
            }
            
                IUIAutomation automation = new CUIAutomation8();
                IUIAutomationElement currentwin = null;
            try
            {

            BringToForeground("Nirvana", ref currentwin);
            //BringToForeground(currentwin)
            _currwinglobal = currentwin;

            }
            catch (Exception ex) {
                Console.WriteLine("Error while getting foregroundwindow");
                string msg = ex.Message.ToString();
                CreateLogs(msg, logFilePath);
                exitcode = 23;
                Console.WriteLine(ex.StackTrace);
            }
            Thread.Sleep(500);
                //StringBuilder Buff = new StringBuilder(nChars);
                OpenReconModule();

                Thread.Sleep(5000);
           
            if(currentwin != null)
            {


                Console.WriteLine(currentwin.CurrentName);
            }
            MaximizeWindow("Reconciliation", ref currentwin);
            if (currentwin != null)
            {


                Console.WriteLine(currentwin.CurrentName);
            }
            _currwinglobal = currentwin;

            
            while (currentwin==null || (currentwin.CurrentAutomationId != "DataCompareForm")) {
                BringToForeground("Nirvana", ref currentwin);
                Console.WriteLine(currentwin.CurrentName);
                OpenReconModule();
                MaximizeWindow("Reconciliation", ref currentwin);
                if (currentwin != null)
                {


                    Console.WriteLine(currentwin.CurrentName);
                }
                _currwinglobal = currentwin;
            }
            if ((currentwin.CurrentAutomationId == "DataCompareForm"))
                {

                    
               
                    IUIAutomationCondition condtitlebar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "_DataCompareForm_UltraFormManager_Dock_Area_Top");
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
                MaximizeWindow("Reconciliation", ref currentwin);
                Thread.Sleep(1000);
           
                Console.WriteLine("Recon Opened Successfully.................................");
                    IUIAutomationElementArray allrows = findiuielementofrows1(currentwin);
                //IUIAutomationElement grid = currwin.FindFirst(
          // TreeScope.TreeScope_Descendants, condition1);

                IUIAutomationCondition Conditioncmbbxclient = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "cmbbxClient");
                IUIAutomationElement cmbboxClient= currentwin.FindFirst(TreeScope.TreeScope_Descendants, Conditioncmbbxclient);
                
                IUIAutomationCondition Conditioncmbbxrecontype= automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "cmbbxReconType");
                IUIAutomationElement cmbboxReconType = currentwin.FindFirst(TreeScope.TreeScope_Descendants, Conditioncmbbxrecontype);
                
                IUIAutomationCondition Conditioncmbbxrecontemplates = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "cmbbxReconTemplates");
                IUIAutomationElement cmbboxRecontemplates = currentwin.FindFirst(TreeScope.TreeScope_Descendants, Conditioncmbbxrecontemplates);
                
                IUIAutomationCondition ConditionBttnView = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnView");
                IUIAutomationElement BttnView = currentwin.FindFirst(TreeScope.TreeScope_Descendants, ConditionBttnView);

                IUIAutomationCondition conditionapplicationgrid = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Un-Matched Broker Data");
                IUIAutomationElement applicationgrid = currentwin.FindFirst(TreeScope.TreeScope_Descendants, conditionapplicationgrid);
                
                IUIAutomationCondition ConditionBttnImport = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnImport");
                IUIAutomationElement BttnImport = applicationgrid.FindFirst(TreeScope.TreeScope_Descendants, ConditionBttnImport);
                
                IUIAutomationCondition ConditionBttnCompare = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnCompare");
                IUIAutomationElement BttnCompare = currentwin.FindFirst(TreeScope.TreeScope_Descendants, ConditionBttnCompare);
                
                IUIAutomationCondition ConditionFromDate = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "dtFromDatePicker");
                IUIAutomationElement FromDateElement = currentwin.FindFirst(TreeScope.TreeScope_Descendants, ConditionFromDate);
                
                IUIAutomationCondition ConditionToDate = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "dtToDatePicker");
                IUIAutomationElement ToDateElement = currentwin.FindFirst(TreeScope.TreeScope_Descendants, ConditionToDate);
                
                IUIAutomationCondition ConditionGeneratereportBttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Generate ExceptionReport");
                IUIAutomationElement BttnGenerateReport = currentwin.FindFirst(TreeScope.TreeScope_Descendants, ConditionGeneratereportBttn);

                //checkbox
                //chkBoxClrExpCache
                IUIAutomationCondition ConditionCacheChkBox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "chkBoxClrExpCache");
                IUIAutomationElement CacheChkBox = currentwin.FindFirst(TreeScope.TreeScope_Descendants, ConditionCacheChkBox);
                
                //  List<KeyValuePair<int, IUIAutomationElement>> selectedrows = findiuielementofrows(currentwin);
                KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("Import Data", "Import Data");
                    foreach (DataRow row in dataFromCsvFile.Rows)
                    {
                        string ClientName= row["Client Name"].ToString();
                    string ReconType= row["Recon Type"].ToString();
                    string ReconTemplate = row["Recon Template"].ToString();
                    string checkbox = row["Clear Exception"].ToString();
                    //string uploadThirdParty = row["UploadThirdParty"].ToString();
                    // string importTypeFormatName = row["ImportTypeFormatName"].ToString();
                    string currentDirectory = Directory.GetCurrentDirectory();
                        //  string importFilePath = currentDirectory + row["FilePath"].ToString();
                        // string importFilePath = ReconFileFolderPath + $@"\{processDate.ToString("MMMM")}\{processDate.ToString("MMdd")}\{row["FilePath"].ToString()}";
                        string importFilePath = ReconFileFolderPath + ReplaceDateTimePlaceholders(row["Recon File Path"].ToString());

                        string FromDate = row["From Date"].ToString();
                        string Todate = row["To Date"].ToString();
                    if (FromDate != "")
                    {
                        FromDate = setcurrdate(FromDate);
                    }
                    if (Todate != "")
                    {
                        Todate = setcurrdate(Todate);
                    }
                    //if (DateTime.TryParseExact(FromDate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime inputDate))
                    //{
                        
                    //}
                    //else
                    //{
                    //    Log("Invalid date format");
                    //}

                    // string date = row["Date"].ToString();
                    string waitFromSheet = row["Wait"].ToString();
                        int wait;
                        if (waitFromSheet != null)
                        {
                            wait = int.Parse(row["Wait"].ToString());
                        }
                        else
                        {
                            wait = 0;
                        }
                    if (CacheChkBox != null)
                    {
                        string togglestate = getnewvalue(CacheChkBox);
                        if(checkbox=="TRUE" && togglestate == "ToggleState_Off")
                        {
                            replayaction(CacheChkBox, "");
                        }
                        else if(checkbox == "FALSE" && togglestate == "ToggleState_On")
                        {
                            replayaction(CacheChkBox, "");
                        }
                    }
                    string reloadfileflag = "";
                    try {
                        reloadfileflag = row["Reload File"].ToString().ToUpper();

                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString() + "Reload column not available in sheet ");
                        Console.WriteLine(ex.StackTrace);

                    }

                        //string exportData = row["Export"].ToString().ToLower();

                       
                    if (ClientName != "" && ReconType != "" && importFilePath != "" && ReconTemplate!="")
                        {
                        bool reconprocess = ReconFile(currentwin,  importFilePath,  cmbboxClient,  cmbboxReconType,  cmbboxRecontemplates
                            ,  BttnView,  BttnImport,BttnCompare,  FromDateElement,  ToDateElement,  BttnGenerateReport,ClientName,ReconType,ReconTemplate,FromDate,Todate,reloadfileflag);
                        string msg = "";
                        if(reconprocess) {
                            msg += Environment.NewLine + "File Generated For   " + ClientName + "  " + ReconType + "  " + ReconTemplate + "  " + importFilePath + "   " + "Successfully!!!";
                            successCounter++;
                            CreateLogs(msg, logFilePath);
                            Console.WriteLine(msg);
                        }
                        else
                        {
                            msg += Environment.NewLine + "ERROR    " + ClientName + "  " + ReconType + "  " + ReconTemplate + "  " + importFilePath + "   " + "  File Not Generated ";
                            CreateLogs(msg, logFilePath);
                            Console.WriteLine(msg);
                            exitcode = 29;
                        }
                       // bool isImpFile = ImportFile(currentwin, allrows, dataFromCsvFile, uploadThirdParty, importTypeFormatName, importFilePath, account, date, wait, exportData, mainFileData);
                    }
                        else
                        {
                            string msg = "Any Of the The Columns  ClientName, ReconType, FilePath, ReconTemplate Can't be blank :    " + ClientName + "  " + ReconType + "  " + importFilePath + " "+ ReconTemplate;
                            CreateLogs(msg, logFilePath);
                        Console.WriteLine(msg);
                        exitcode = 29;
                    }





                    }

                    



                }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            string msg =ex.Message.ToString();
            CreateLogs(msg, logFilePath);
            Console.WriteLine(ex.StackTrace);
        }
        // Console.WriteLine(GetActiveWindowTitle());
    }

    public static bool ReconFile(IUIAutomationElement currentwin, string importFilePath, IUIAutomationElement cmbboxClient,
        IUIAutomationElement cmbboxReconType, IUIAutomationElement cmbboxRecontemplates, IUIAutomationElement BttnView,
        IUIAutomationElement BttnImport, IUIAutomationElement BttnCompare, IUIAutomationElement FromDateElement, IUIAutomationElement ToDateElement,
        IUIAutomationElement BttnGenerateReport, string clientname, string recontype, string recontemplate, string fromdate, string todate, string reloadfileflag)
    {
        try
        {
            IUIAutomation automation = new CUIAutomation8();
            string processDateForUI = processDate.ToString("MMddyyyy");
            if (!processDateForUI.Contains("/"))
            {
                // If not, add slashes using string interpolation
                processDateForUI = $"{processDateForUI.Substring(0, 2)}/{processDateForUI.Substring(2, 2)}/{processDateForUI.Substring(4)}";
            }
            if (fromdate != "" && todate != "")
            {
                object copydateobj;
                if (FromDateElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                {
                    copydateobj = FromDateElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                    IUIAutomationValuePattern valuepatt = copydateobj as IUIAutomationValuePattern;
                    valuepatt.SetValue(fromdate);

                }
                //Thread.Sleep(2000);
                if (ToDateElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                {
                    copydateobj = ToDateElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                    IUIAutomationValuePattern valuepatt = copydateobj as IUIAutomationValuePattern;
                    valuepatt.SetValue(processDateForUI);

                }

            }
            else
            {
                object copydateobj;
                if (FromDateElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                {
                    copydateobj = FromDateElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                    IUIAutomationValuePattern valuepatt = copydateobj as IUIAutomationValuePattern;
                    valuepatt.SetValue(processDateForUI);

                }
            }

            if (CheckValueChangeComboBox(cmbboxClient, clientname, currentwin))
            {
                bool x = SelectValueInComboBox(cmbboxClient, clientname, currentwin);
                if (!x)
                {
                    string msg = "Option Not Found On UI Please check Config  " + clientname;
                    Console.WriteLine(msg);
                    CreateLogs(msg, logFilePath);
                    return false;
                }
            }

            if (CheckValueChangeComboBox(cmbboxReconType, recontype, currentwin))
            {
                bool x = SelectValueInComboBox(cmbboxReconType, recontype, currentwin);
                if (!x)
                {
                    string msg = "Option Not Found On UI Please check Config  " + recontype;
                    Console.WriteLine(msg);
                    CreateLogs(msg, logFilePath);
                    return false;
                }
            }
            if (CheckValueChangeComboBox(cmbboxRecontemplates, recontemplate, currentwin))
            {
                bool x = SelectValueInComboBox(cmbboxRecontemplates, recontemplate, currentwin);
                if (!x)
                {
                    string msg = "Option Not Found On UI Please check Config  " + recontemplate;
                    Console.WriteLine(msg);
                    CreateLogs(msg, logFilePath);
                    return false;
                }
            }

            Thread modalThread0 = new Thread(HandleModalDialog);
            modalThread0.Start();
            replayaction(BttnView, "");
            modalThread0.Join();


            // error import or wait for grid loading
            Thread.Sleep(500);
            int maxRetries = 400;
            int retryDelayMilliseconds = 1000; // 1 second
                                               //Fetching Data...
            for (int retryCount = 0; retryCount < maxRetries; retryCount++)
            {
                try
                {
                    IUIAutomationCondition conditionapplicationgrid = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Application Data");
                    IUIAutomationElement applicationgrid = currentwin.FindFirst(TreeScope.TreeScope_Descendants, conditionapplicationgrid);
                    // IUIAutomationCondition dataitemcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
                    // IUIAutomationElement dataitemelement = applicationgrid.FindFirst(TreeScope.TreeScope_Descendants, dataitemcond);
                    IUIAutomationCondition dataitemcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Fetching Data...");
                    IUIAutomationElement dataitemelement = applicationgrid.FindFirst(TreeScope.TreeScope_Descendants, dataitemcond);
                    if (currentwin != null && (int)currentwin.GetCurrentPropertyValue(UIA_PropertyIds.UIA_WindowWindowInteractionStatePropertyId) == 2)
                    {
                        if (dataitemelement == null)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(retryDelayMilliseconds);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{retryCount} - Error during Recon Grid: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);

                    // Add a delay before the next retry
                    Thread.Sleep(retryDelayMilliseconds);
                }
            }
            int reloadfiletimes = 1;
            if (reloadfileflag == "TRUE")
            {
                reloadfiletimes = 2;
            }
            for (int i = 0; i < reloadfiletimes; i++)
            {
                //Thread.Sleep(2000);
                Thread modalThread = new Thread(HandleModalDialog);
                _currwinglobal = currentwin;
                modalThread.Start();
                //replayaction(BttnImport, "");
                ClickElement(BttnImport, "Left Mouse Button Clicked");
                modalThread.Join();

                Thread.Sleep(2000);
                //Console.WriteLine("Outside Modal");
                IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Select File to Import");
                IUIAutomationElement dialogbox = currentwin.FindFirst(TreeScope.TreeScope_Descendants, conddialogbox);
                try
                {
                    if (dialogbox == null)
                    {
                        for (int retryCount = 0; retryCount < maxRetries; retryCount++)
                        {

                            try
                            {
                                ClickElement(BttnImport, "Left Mouse Button Clicked");

                                dialogbox = currentwin.FindFirst(TreeScope.TreeScope_Descendants, conddialogbox);
                                if (dialogbox != null)
                                {
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"{retryCount} - Error during Recon Grid: {ex.Message}");

                                // Add a delay before the next retry
                                Thread.Sleep(retryDelayMilliseconds);
                                Console.WriteLine(ex.StackTrace);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Message);
                }


                if (dialogbox != null)
                {
                    IUIAutomationCondition conditionfilename = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId);
                    IUIAutomationCondition conditionfilename1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "File name:");
                    IUIAutomationAndCondition newandcond = (IUIAutomationAndCondition)automation.CreateAndCondition(conditionfilename1, conditionfilename);
                    IUIAutomationElement filename = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, newandcond);
                    if (filename != null)
                    {
                        object valuePatternObj;
                        object textPatternObj;
                        valuePatternObj = filename.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                        textPatternObj = filename.GetCurrentPattern(UIA_PatternIds.UIA_TextEditPatternId);

                        IUIAutomationTextEditPattern textpattern = textPatternObj as IUIAutomationTextEditPattern;
                        IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;
                        Console.WriteLine("This is from file name " + valuePattern.CurrentValue);
                        //string value= dictmap[keyValuePair]["Select File"][rowidx.Key];
                        string value = importFilePath;

                        valuePattern.SetValue(value);
                        Thread.Sleep(500);
                        IUIAutomationCondition condopen = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Open");
                        IUIAutomationCondition condopen1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "1");
                        IUIAutomationCondition condopenand = automation.CreateAndCondition(condopen, condopen1);
                        IUIAutomationElement openbtn = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, condopenand);
                        if (openbtn != null)
                        {
                            WindowGlobal = currentwin;
                            Thread modalThread2 = new Thread(HandleInputFileName);
                            modalThread2.Start();
                            replayaction(openbtn, "");
                            modalThread2.Join();
                            Thread.Sleep(2000);
                            try
                            {
                                // IUIAutomationCondition conddialogbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Select File to Import");
                                IUIAutomationElement dialogbox1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, conddialogbox);
                                if (dialogbox1 != null)
                                {
                                    IUIAutomationCondition condcancel = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Cancel");
                                    IUIAutomationCondition condcancel1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "2");
                                    IUIAutomationCondition condcanceland = automation.CreateAndCondition(condcancel, condcancel1);
                                    IUIAutomationElement cancelbtn = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, condcanceland);
                                    if (cancelbtn != null)
                                    {
                                        Thread modalThread1 = new Thread(HandleExceptionReport);
                                        modalThread1.Start();
                                        replayaction(cancelbtn, "");
                                        modalThread1.Join();
                                        exitcode = 13;
                                        return false;
                                    }


                                }



                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.StackTrace);
                                Console.WriteLine(ex.ToString());
                            }

                            Console.WriteLine("OPEN button pressed successfully................");

                            //here check if file not found and continue if error occured.


                        }

                    }


                }


                //wait till fetching data button active 
                try
                {
                    for (int retryCount = 0; retryCount < maxRetries; retryCount++)
                    {
                        try
                        {
                            IUIAutomationCondition conditionFetchingStatus = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Fetching Data");
                            IUIAutomationElement FetchingStatusBttn = currentwin.FindFirst(TreeScope.TreeScope_Descendants, conditionFetchingStatus);
                            //adding condition to move forward if window is not busy
                         if(currentwin != null  && (int)currentwin.GetCurrentPropertyValue(UIA_PropertyIds.UIA_WindowWindowInteractionStatePropertyId) == 2)
                            {
                                  
                                if (FetchingStatusBttn == null)
                                {
                                    break;
                                }
                            }
                            Thread.Sleep(retryDelayMilliseconds);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                            Console.WriteLine($"{retryCount} - Error during Recon Grid: {ex.Message}");

                            // Add a delay before the next retry
                            Thread.Sleep(retryDelayMilliseconds);
                        }
                        Console.WriteLine("Waiting For Data.....");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Message + " line 1833");

                }

            }


            Thread modalThread4 = new Thread(HandleModalDialog);
            _currwinglobal = currentwin;
            modalThread4.Start();
            replayaction(BttnCompare, "");
            modalThread4.Join();

            // Thread.Sleep(5000);
            //Matched Broker Data
            //for (int retryCount = 0; retryCount < maxRetries; retryCount++)
            //{
            //    try
            //    {
            //        IUIAutomationCondition conditionapplicationgrid = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Matched Broker Data");
            //        IUIAutomationElement applicationgrid = currentwin.FindFirst(TreeScope.TreeScope_Descendants, conditionapplicationgrid);
            //        IUIAutomationCondition dataitemcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
            //        IUIAutomationElement dataitemelement = applicationgrid.FindFirst(TreeScope.TreeScope_Descendants, dataitemcond);
            //        if (dataitemelement != null)
            //        {
            //            break;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"{retryCount} - Error during Recon Grid: {ex.Message}");

            //        // Add a delay before the next retry
            //        Thread.Sleep(retryDelayMilliseconds);
            //    }
            //}



            //replayaction(BttnGenerateReport, "");
            Thread modalThread3 = new Thread(HandleModalDialog);
            _currwinglobal = currentwin;
            modalThread3.Start();
            ClickElement(BttnGenerateReport, "Left Mouse Button Clicked");
            modalThread3.Join();

            Thread.Sleep(2000);

            IUIAutomationCondition conditionFilename = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "lblFileName");
            IUIAutomationElement FilenameElement = currentwin.FindFirst(TreeScope.TreeScope_Descendants, conditionFilename);
            if (FilenameElement != null)
            {
                Console.WriteLine(FilenameElement.CurrentName);
            }
            IUIAutomationCondition contionBttnClose = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnClose");
            IUIAutomationElement BttnClose = currentwin.FindFirst(TreeScope.TreeScope_Descendants, contionBttnClose);

            //replayaction(BttnClose, "");

            ClickElement(BttnClose, "Left Mouse Button Clicked");
            //click import bttn

            //set file path 
            //fillcomboboxes
        }
        catch(Exception ex) {
            Console.WriteLine(ex.StackTrace);
            exitcode = 29;
        }
        return true;
    }
    private static string setcurrdate(string currdate)
    {
        DateTime parsedDate = DateTime.ParseExact(currdate, "M/d/yyyy", null);
        // Format the date as "mm/dd/yyyy"
        string formattedDate = parsedDate.ToString("MM/dd/yyyy");
        return formattedDate;
    }
    public static bool CheckValueChangeComboBox(IUIAutomationElement cmbboxClient, string Valuetoselect, IUIAutomationElement currentwin)
    {
        IUIAutomation automation = new CUIAutomation8();
        string previousvalue=getnewvalue(cmbboxClient);
        if (previousvalue != Valuetoselect)
        {
            return true;
        }
        return false;


    }
        public static bool SelectValueInComboBox(IUIAutomationElement cmbboxClient,string Valuetoselect,IUIAutomationElement currentwin)
    {
        try
        {
            InputSimulator inputSimulator = new InputSimulator();
            IUIAutomation automation = new CUIAutomation8();

            IUIAutomationCondition ConditioncmbbxDropdownBttn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
            IUIAutomationElement cmbbxDropdownBttn = cmbboxClient.FindFirst(TreeScope.TreeScope_Descendants, ConditioncmbbxDropdownBttn);
            //invoke
            object patternprovider;
            if (cmbbxDropdownBttn.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
            {
                patternprovider = cmbbxDropdownBttn.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                Console.WriteLine(cmbbxDropdownBttn.CurrentName);

                IUIAutomationInvokePattern selectionpatternprovider = patternprovider as IUIAutomationInvokePattern;
                //Thread modalThread = new Thread(HandleModalDialog);
                //modalThread.Start();
                selectionpatternprovider.Invoke();
                //modalThread.Join();


            }
            Thread.Sleep(500);
            //findlistitem
            IUIAutomationCondition Conditionlistitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, Valuetoselect);
            IUIAutomationElement Listitemelement = currentwin.FindFirst(TreeScope.TreeScope_Descendants, Conditionlistitem);
            Console.WriteLine(Listitemelement.CurrentName);

            object patternobj2;
            if (Listitemelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
            {
                patternobj2 = Listitemelement.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                IUIAutomationScrollItemPattern selectpatt = patternobj2 as IUIAutomationScrollItemPattern;

                selectpatt.ScrollIntoView();


            }
            //Log(element.CurrentBoundingRectangle.right - 30);
            //Log(element.CurrentBoundingRectangle.right);
            Console.WriteLine(Listitemelement.CurrentBoundingRectangle.right - 10);
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(Listitemelement.CurrentBoundingRectangle.right - 10, Listitemelement.CurrentBoundingRectangle.bottom - 15);
            Thread.Sleep(500);
            inputSimulator.Mouse.LeftButtonClick();
            Thread.Sleep(500);


            //selectionpattern select
            //Thread.Sleep(2000);

            if (cmbbxDropdownBttn.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
            {
                patternprovider = cmbbxDropdownBttn.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                Console.WriteLine(cmbbxDropdownBttn.CurrentName);

                IUIAutomationInvokePattern selectionpatternprovider = patternprovider as IUIAutomationInvokePattern;
                //Thread modalThread = new Thread(HandleModalDialog);
                //modalThread.Start();
                selectionpatternprovider.Invoke();
                //modalThread.Join();


            }
            return true;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine();
                }
        return false;
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
                    Console.WriteLine("Failed to extract FilterValue.");
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);

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
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine($"Error reading XML path from config: {ex.Message}");
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
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine($"Error reading XML path from config: {ex.Message}");
            return null;
        }
    }

   

    //window hook new 7/11

    static WinEventDelegate dele = null;
    static WinEventDelegate dele1 = null;

    public static DateTime currentDate = DateTime.Now;
    public static string currentDir = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
    public static DateTime processDate = GetProcessDate(Directory.GetCurrentDirectory() + @"\ReconConfigs\ProcessDateXmlPath.txt");
    public static string ReconFileFolderPath = ReadImportPathFromConfig(Directory.GetCurrentDirectory() + @"\ReconConfigs\ReconFileFolderPath.txt");

    public static string logFolderPath = "Logs";
    public static string logFilePath = Path.Combine(logFolderPath, $"Recon_Log_{currentDate.ToString("yyyyMMdd")}.txt");
    public static DateTime initialTime = DateTime.Now;
    public static DateTime finalTime;
    public static string _global_appication_error_msg="";
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
    public static bool Login(DataTable dt, string exePath)
    {
        IUIAutomation automation = new CUIAutomation8();
        IUIAutomationElement currentwin = null;
        string currwin = "Nirvana: User Login";
        string loginId = GetValueFromMainFile(dt, "loginId");
        string loginPass = GetValueFromMainFile(dt, "loginPassword");
       
       IUIAutomationElement username=null;
        IUIAutomationElement password = null;
        IUIAutomationElement btn = null;


        try
        {
            
        
            BringToForeground(currwin, ref currentwin);
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
        catch(Exception ex)
        {
            Console.WriteLine(ex.StackTrace);

            //bool isApplicationClose = StopApplication(exePath);
            string logMsg = Environment.NewLine + ex.Message.ToString();
            CreateLogs(logMsg, logFilePath);
            return false;
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
        catch(Exception ex){
            Console.WriteLine(ex.StackTrace);
        }

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
    static bool IsApplicationRunning(string exePath)
    {
        //IUIAutomation automation = new CUIAutomation8();
        //var root = automation.GetRootElement();
        //IUIAutomationCondition condtitlebar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Nirvana");
        //IUIAutomationCondition condtitlebar1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "PranaMain");

        //IUIAutomationCondition selectOptioncondand = automation.CreateAndCondition(condtitlebar, condtitlebar1);
        //var allElements = root.FindAll(TreeScope.TreeScope_Descendants, selectOptioncondand);
        //// Find the first element that matches the name using the regular expression

        //for (int i = 0; i < allElements.Length; i++)
        //{
        //    var currentElement = allElements.GetElement(i);
        //    //Console.WriteLine(currentElement.CurrentProcessId);
        //    Process myProcess = Process.GetProcessById(currentElement.CurrentProcessId);
        //    string path = GetProcessFilename(myProcess);
        //    //Console.WriteLine(path);
        //    if (path.ToLower().Equals(applicationPath.ToLower()))
        //    {
        //        return true;
        //    }
        //}
        //return false;
        
        
            bool isLogin = false;
            string[] ownerInfo = new string[2];
            uint processId = 0;
        string logMsg = "";
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
                    // if (processPath != null && processPath.ToLower().Equals(exePath.ToLower()))
                    if (processPath != null && processPath.ToLower().Equals(exePath.ToLower()))
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
                logMsg = "Getting exception " + ex.Message + "for owner:" + ownerInfo[0] + "PID: " + processId + ", Owner: " + ownerInfo[0] + ", domain: " + ownerInfo[1] + " while getting all the opened instances of Prana.\n " + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
            return isLogin;
        


    }


    [STAThread]

    public static void Main(string[] args)
    {

        //Process.Start(@"C:\Users\Tarun.Gupta1\Desktop\JonesMonomoy\Prana.exe - Shortcut.lnk");
        // Your application logic loop
        try
        {
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
            if (command == "i")
            {

                if (!Directory.Exists(logFolderPath))
                {
                    Directory.CreateDirectory(logFolderPath);
                }
                //initialTime = DateTime.Now;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                MinimizeConsoleWindow();

                string ReconConfig = "ReconConfigs";
                if (!Directory.Exists(ReconConfig))
                {
                    Directory.CreateDirectory(ReconConfig);
                }


                string mainfilePath = @"ReconConfigs\MainFile.csv";
                string importDataFilePath = @"ReconConfigs\Recon Details.csv";

                DataTable dataFromCsvFile = DataFromCsvFile(importDataFilePath);
                DataTable mainFileData = DataFromCsvFile(mainfilePath);
                string exePath = GetValueFromMainFile(mainFileData, "exePath");
                application_running = IsApplicationRunning(exePath);
                totalRecons = dataFromCsvFile.Rows.Count;
                if (!application_running)
                {
                    Process.Start(exePath);
                    CreateLogs(exePath + "is not running.", logFilePath);
                }


                //Process.Start(exePath);
                //Thread.Sleep(4000);
                try
                {

                    ReconReplayer(dataFromCsvFile, mainFileData, exePath);
                    finalTime = DateTime.Now;
                    TimeSpan timeDifference = finalTime - initialTime;
                    Console.WriteLine("");
                    Console.WriteLine("");
                    string totalTimeTaken = timeDifference.Hours + " Hours " + timeDifference.Minutes + " Minutes " + timeDifference.Seconds + " Seconds " + timeDifference.Milliseconds + " Milliseconds ";
                    Console.WriteLine("Total Time Taken : " + totalTimeTaken);

                    string timeLogMessage = "Total Time Taken : " + totalTimeTaken;
                    CreateLogs(timeLogMessage, logFilePath, 0);
                    if (successCounter > 0)
                    {
                        if (successCounter == totalRecons)
                        {
                            string message = $"All Recons Done Successfully -- ({successCounter}/{totalRecons})";
                            Console.WriteLine(message);

                            CreateLogs(message, logFilePath, 0);
                            exitcode = 0;
                        }
                        else {
                            string message = $"Partial Recons Done Successfully -- ({successCounter}/{totalRecons})";
                            Console.WriteLine(message);

                            CreateLogs(message, logFilePath, 0);
                           
                        }
                    }
                    else {
                        string message = $"No Recons Done Successfully -- ({successCounter}/{totalRecons})";
                        Console.WriteLine(message);

                        CreateLogs(message, logFilePath, 0);
                       
                    }

                    string logMsg = "*************************************************************SUCCESS!!*************************************************************";
                    CreateLogs(logMsg, logFilePath, 0);

                   

                   // Console.Clear(); // Clear the console
                    RestoreConsoleWindow();

                    Console.WriteLine("Press any key to close the console window...");

                    // Wait for any key press
                    //Console.ReadKey(true);

                    // Close the console window
                   // Environment.Exit(0);


                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception : " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    exitcode = 2;
                    Console.WriteLine(ex.StackTrace);
                }

                finally
                {

                }



            }


            else
            {
                Console.WriteLine("Please give a valid input ");
            }
        }
        catch(Exception ex)
        {
            exitcode = 2;
            Console.WriteLine(ex.StackTrace);
        }
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
    public static void CaptureScreenshot(string folderPath,string fileName)
    {
        try
        {
            // Determine the screen size
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Create a bitmap with the same dimensions as the screen
            using (Bitmap bitmap = new Bitmap(screenWidth, screenHeight))
            {
                // Create a graphics object from the bitmap
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // Copy the screen into the bitmap
                    graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                }

                // Generate a file name
                 fileName = $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                string filePath = System.IO.Path.Combine(folderPath, fileName);

                // Save the bitmap to the specified folder
                bitmap.Save(filePath, ImageFormat.Png);

                Console.WriteLine($"Screenshot saved to: {filePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while capturing the screenshot: {ex.Message}");
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
            if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
            {
                string value = null;
                object patternprovider;
                if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                {
                    patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                    Console.WriteLine(targetelement.CurrentName);

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
                if (btnelement != null)
                {
                    if (btnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                    {
                        patternprovider = btnelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                        // Console.WriteLine("inside expand pattern");
                        IUIAutomationInvokePattern expandpatternprovider = patternprovider as IUIAutomationInvokePattern;

                        expandpatternprovider.Invoke();

                        IUIAutomationCondition selectOptioncond = automation1.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, value);

                        IUIAutomationElement listelement = targetelement.FindFirst(TreeScope.TreeScope_Children, selectOptioncond);
                        if (listelement != null)
                        {
                            if (listelement.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId) != null)
                            {
                                Console.WriteLine(listelement.CurrentName);
                                patternprovider = listelement.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                                IUIAutomationSelectionItemPattern selectionpatternprovider = patternprovider as IUIAutomationSelectionItemPattern;
                                selectionpatternprovider.Select();
                                expandpatternprovider.Invoke();
                                return;

                            }
                        }
                        expandpatternprovider.Invoke();
                    }
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
            exitcode = 23;
            Console.WriteLine(ex.StackTrace);
            return;
        }
    }
    public static void BringToForeground(string currwin, ref IUIAutomationElement currentwin)
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
        if (currentwin!=null && currentwin.CurrentName == currwin) {
            f = true;
            element = currentwin;
        }
        if (f==false && currwin.Contains("Trading Ticket"))
        {
            currwin = ".*Trading Ticket.*";
            // Create an AutomationElement representing the root element of the desktop

            // Define the regular expression for matching "Trading Ticket" anywhere in the name
            Regex regex = new Regex(".*Trading Ticket.*", RegexOptions.IgnoreCase);

            // Define the condition for the element you want to find
            var condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");

            // Find all elements that match the control type condition
            var allElements = root.FindAll(TreeScope.TreeScope_Descendants, condition);

            // Find the first element that matches the name using the regular expression

            for (int i = 0; i < allElements.Length; i++)
            {
                var currentElement = allElements.GetElement(i);
                if (regex.IsMatch(currentElement.CurrentName))
                {
                    element = currentElement;
                    break;
                }
            }

            // Check if the element is found
            if (element != null)
            {
                // Do something with the found element
                Console.WriteLine("Element found: " + element.CurrentName);
            }
            else
            {
                Console.WriteLine("Element not found.");
            }

        }
        else if(f==false)
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
        else
        {
            // Calculator is not open, start a new instance
            //if (exepaths.ContainsKey(currwin))
            //{
            //    Process.Start(exepaths[currwin]);

            //}
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
        if (f == false && currwin.Contains("Trading Ticket"))
        {
            currwin = ".*Trading Ticket.*";
            // Create an AutomationElement representing the root element of the desktop

            // Define the regular expression for matching "Trading Ticket" anywhere in the name
            Regex regex = new Regex(".*Trading Ticket.*", RegexOptions.IgnoreCase);

            // Define the condition for the element you want to find
            var condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");

            // Find all elements that match the control type condition
            var allElements = root.FindAll(TreeScope.TreeScope_Descendants, condition);

            // Find the first element that matches the name using the regular expression

            for (int i = 0; i < allElements.Length; i++)
            {
                var currentElement = allElements.GetElement(i);
                if (regex.IsMatch(currentElement.CurrentName))
                {
                    element = currentElement;
                    break;
                }
            }

            // Check if the element is found
            if (element != null)
            {
                // Do something with the found element
                Console.WriteLine("Element found: " + element.CurrentName);
            }
            else
            {
                Console.WriteLine("Element not found.");
            }

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
            Console.WriteLine(ex.StackTrace);
            CreateLogs(logMsg, logFilePath);
            
        }
    }

    private static void ClickElement(IUIAutomationElement element, string clicktype)
    {
        if (element == null) {
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
   
   




    private static async Task LogAsync(string logMessage)
    {
        try
        {
            lock (_fileLock)
            {
                // Append the log message to the file
                if (_keyboardpressed)
                {
                    using (StreamWriter writer = File.AppendText(logFilename))
                    {
                        logMessage += " ";
                        writer.WriteAsync(logMessage);
                    }
                }
                else
                {
                    using (StreamWriter writer = File.AppendText(logFilename))
                    {
                        writer.WriteLineAsync(logMessage);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // If there is an error, display the error message
            Console.WriteLine("Error writing to log file: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }

    private static void Log(string logMessage)
    {
        try
        {
            // Append the log message to the file
            if (_keyboardpressed)
            {
                using (StreamWriter writer = File.AppendText(logFilename))
                {
                    logMessage += " ";
                    writer.Write(logMessage);
                }
            }
            else
            {
                using (StreamWriter writer = File.AppendText(logFilename))
                {
                    writer.WriteLine(logMessage);
                }
            }
        }
        catch (Exception ex)
        {
            // If there is an error, display the error message
            Console.WriteLine("Error writing to log file: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }





    




    private static IUIAutomationElement FindElementAtPoint(int x, int y)
    {
        IUIAutomationElement element = null;
        IUIAutomation automation = new CUIAutomation8();
        try
        {

            element = automation.ElementFromPoint(new tagPOINT() { x = (int)x, y = (int)y });
            //Console.WriteLine(element.CurrentName);
            // Console.WriteLine(element.CurrentAutomationId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
        return element;

    }
    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x;
        public int y;
    }
    static void CaptureAndSaveScreenshot()
    {
        // Get the current directory where the program is running
        string currentDirectory = Directory.GetCurrentDirectory();

        // Get current date for folder name
        string folderName = $"Pictures_{DateTime.Now.ToString("yyyyMMdd")}";
        string folderPath = Path.Combine(currentDirectory, folderName);

        // Create the folder if it doesn't exist
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Capture the screen
        using (Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
        {
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            }

            // Get current date and time for filename
            string currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            // Save the screenshot with the filename based on current date and time
            string fileName = $"Screenshot_{currentDateTime}.png";
            string filePath = Path.Combine(folderPath, fileName);

            bitmap.Save(filePath, ImageFormat.Png);

            string logmessage = $"Screenshot saved as {fileName} in folder {folderName}.";
            Console.WriteLine(logmessage);

            Task.Run(async () =>
            {
                await LogAsync(logmessage);
            });
        }
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
    static bool findvalueindict(string targetValue)
    {
        foreach (var outerEntry in dictmapalldata)
        {
            foreach (var innerEntry in outerEntry.Value)
            {
                if (innerEntry.Key == targetValue)
                {
                    Console.WriteLine("This is key " + innerEntry.Key);
                    // outerEntry.Value[innerEntry.Key] = newValue;
                    return true;
                }
            }
        }

        return false;
    }
    
    static string[] elementNamesToFind = new string[]
        {
            "cmbMethodology",
            "cmbAlgorithm",
            "Element3Name",
            "cmbSecondarySort"
        };
    
    // private static IUIAutomationElement buttonsample = null;
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

    private static void RemovePropertyChangedEventHandler(string elementeach, IUIAutomationElement element)
    {

        try
        {
            //if (_eventhandledict.ContainsKey(elementeach))
            // {
            // Check if the element exists in the dictionary

            //need automation element also with handler 
            // eventHandler = 
            //var eventHandler = _eventhandledict[elementeach]; ;

            //_automation.RemovePropertyChangedEventHandler(element, eventHandler);
            //_eventhandledict.Remove(elementeach);

            // Remove the event handler from the dictionary

            // }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

        }
        finally
        {


        }
    }
    static bool _flag = true;




    //private static IntPtr HookCallbackCBT(int nCode, IntPtr wParam, IntPtr lParam)
    //{
    //    if (nCode == 3) // HCBT_ACTIVATE
    //    {
    //        Console.WriteLine("inside hook");
    //       // WindowActivated?.Invoke(this, EventArgs.Empty);
    //    }
    //    if (nCode == 5)
    //    {
    //        Console.WriteLine("inside hook 2");

    //    }
    //    if (nCode == 1)
    //    {
    //        Console.WriteLine("inside hook 3");

    //    }
    //    if (nCode == 4)
    //    {
    //        Console.WriteLine("inside hook 4");

    //    }

    //    return CallNextHookEx(_cbtHookID, nCode, wParam, lParam);
    //}




    
    private static bool FindElementDFS(IUIAutomationElement currWin, IUIAutomationElement targetElement, ref string path)
    {

        string currwinid = currWin.CurrentAutomationId;

        string targetElementid = targetElement.CurrentAutomationId;
        if (currwinid == targetElementid)
        {
            if (TreeNode.Equals(targetElement, currWin))
            {
                return true;
            }

            return true;
        }
        if (TreeNode.ReferenceEquals(currWin, targetElement))
        {
            return true;
        }
        //if (currWin == targetElement)
        //{
        //    return true;
        //}
        IUIAutomation automat = new CUIAutomation8();

        IUIAutomationCondition truecond = automat.CreateTrueCondition();
        IUIAutomationTreeWalker treeWalker = automat.CreateTreeWalker(truecond);
        IUIAutomationElement child = treeWalker.GetFirstChildElement(currWin);

        int count = 0;
        while (child != null)
        {
            count++;
            string childPath = "";
            bool found = FindElementDFS(child, targetElement, ref childPath);

            if (found)
            {
                path = $"c{count}/{childPath}";
                return true;
            }


            child = treeWalker.GetNextSiblingElement(child);
        }

        return false;
    }
    public static string ModifyPath(string originalPath)
    {
        string[] steps = originalPath.Split('/');
        List<string> modifiedSteps = new List<string>();

        int count = 0;
        foreach (string step in steps)
        {
            if (step.StartsWith("c"))
            {
                int childCount = int.Parse(step.Substring(1));
                if (childCount == 1)
                {
                    count++;
                }
                if (childCount > 1)
                {
                    modifiedSteps.Add($"c{++count}");
                    modifiedSteps.Add($"s{childCount - 1}");
                    count = 0;

                }

            }

        }
        if (count >= 1)
        {
            modifiedSteps.Add($"c{count}");

        }

        return string.Join("/", modifiedSteps);
    }



    public static string GetElementPath(IUIAutomationElement currWin, IUIAutomationElement targetElement)
    {
        string path = "";
        bool found = FindElementDFS(currWin, targetElement, ref path);
        if (found)
        {
            return path;
        }

        return "Element not found in the current UI Automation tree.";
    }

    

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
                        //Console.WriteLine(listitemText);
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
                            //Console.WriteLine($"ComboBox value: {comboBoxValue}");

                        }
                    }
                }
            }
            sw2.Stop();
            long etime = sw2.ElapsedMilliseconds;
            // Console.WriteLine("time taken for one combobox " + etime);
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
                    //Console.WriteLine(selectionpatternprovider.CurrentToggleState.ToString());
                    valuetouse = togglestate;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }

        }
        else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_RadioButtonControlTypeId)
        {
            valuetouse = targetelement.CurrentName;

        }
        else if (targetelement.CurrentAutomationId == "MultiSelectEditor")
        {
            //  Console.WriteLine("inside multi select ");

            IUIAutomation automation2 = new CUIAutomation8();
            IUIAutomationCondition bttn = automation2.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
            IUIAutomationElement btnelement = targetelement.FindFirst(TreeScope.TreeScope_Descendants, bttn);


            if (btnelement != null)
            {
                // Console.WriteLine("Found Button element ");
                //expand pattern invoke
                try
                {
                    string value = null;
                    object patternprovider;
                    if (btnelement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) != null)
                    {
                        patternprovider = btnelement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
                        // Console.WriteLine("inside expand pattern");
                        IUIAutomationExpandCollapsePattern selectionpatternprovider = patternprovider as IUIAutomationExpandCollapsePattern;
                        value = selectionpatternprovider.CurrentExpandCollapseState.ToString();
                        //Console.WriteLine("......////...........");
                        //   Console.WriteLine(selectionpatternprovider.CurrentExpandCollapseState.ToString());
                        selectionpatternprovider.Expand();
                        //  Console.WriteLine(selectionpatternprovider.CurrentExpandCollapseState.ToString());
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
                            //Console.WriteLine(checkboxes[0].Current.Name);
                            //   Console.WriteLine("SIZE OF List" + checkboxes.Length);

                            if (checkboxes != null)
                            {
                                // Console.Write("inside checkboxes ");
                                for (int i = 0; i < checkboxes.Length; i++)
                                {
                                    IUIAutomationElement checkbox = checkboxes.GetElement(i);
                                    //  Console.WriteLine(checkbox.CurrentName + "..");
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
                                            // Console.WriteLine("......////...........");
                                            //  Console.WriteLine(chkboxpatternprovider.ToString());
                                            //   Console.WriteLine(chkboxpatternprovider.CurrentToggleState.ToString());
                                            if (chkboxpatternprovider.CurrentToggleState.ToString() == "ToggleState_On")
                                            {
                                                valuetouse += ("," + checkbox.CurrentName);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.StackTrace);
                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);

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
                    // Console.WriteLine(valuetouse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("INSIDE EDIT ITEM CANNOT GET VALUE");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

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
                    if (_correctioncolumn.Keys.Contains(elementtoget))
                    {
                        elementValue = _correctioncolumn[elementtoget][elementValue];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

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
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                // Handle exceptions as needed
            }
            //Console.WriteLine("checking");
            //Console.WriteLine(targetelement.Current.AutomationId  +"    " +   elementValue);
            //Console.WriteLine(targetelement.Current.AutomationId + "    " + selectedText);    
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
    private static List<Keys> _keystore = new List<Keys>();


    private const int WH_MOUSE_LL = 14;
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;

    private enum MouseMessages
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
    }
    [StructLayout(LayoutKind.Sequential)]
    private struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KBDLLHOOKSTRUCT
    {
        public uint vkCode;
        public uint scanCode;
        public KBDLLHOOKSTRUCTFlags flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [Flags]
    private enum KBDLLHOOKSTRUCTFlags : uint
    {
        LLKHF_EXTENDED = 0x01,
        LLKHF_INJECTED = 0x10,
        LLKHF_ALTDOWN = 0x20,
        LLKHF_UP = 0x80,
    }
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

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr GetFocus();


    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int GetClassName(IntPtr hWnd, System.Text.StringBuilder lpClassName, int nMaxCount);


}