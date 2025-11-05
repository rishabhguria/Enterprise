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

    // NEW IMPLEMENTATION DICTIONARIES 
    static Dictionary<KeyValuePair<string, string>, Dictionary<string, List<string>>> dictmap = new Dictionary<KeyValuePair<string, string>, Dictionary<string, List<string>>>();
    static Dictionary<KeyValuePair<string, string>, Dictionary<string, string>> dictmapalldata = new Dictionary<KeyValuePair<string, string>, Dictionary<string, string>>();
    static Dictionary<string, string> map = new Dictionary<string, string>();

    IUIAutomationElement _globalelement = null;

    private static int exitcode = -1;

    static List<string> _stepnameloglist = new List<string>();
    //generic sheet writing 
    static string gstring = "https://docs.google.com/spreadsheets/d/1PQekmcHmBX57NCMW0aNZz8TySEt2RdTHqlYOo6WTTYU/edit#gid=661870191";
    static int rownum = 6;
    // static Dictionary<string , List<string>> _moduleactionelements=new Dictionary<string , List<string>>();
    static Dictionary<string, string> _stepnamecorrection = new Dictionary<string, string>();

    //cmborderside,<"1":buy>,<"2":sell
    static Dictionary<string, Dictionary<string, string>> _correctioncolumn = new Dictionary<string, Dictionary<string, string>>();
    static Dictionary<string, int> stepnametoindex = new Dictionary<string, int>();
    //new method 
    static Dictionary<string, List<string>> _moduletostepname = new Dictionary<string, List<string>>();
    //<tradingticket , ["createorder" , "doneaway"]>;
    //<blotter,["verifyordergrid"];
    static Dictionary<string, List<string>> _stepnametoelement = new Dictionary<string, List<string>>();
    static Dictionary<string, List<string>> _moduletoelement = new Dictionary<string, List<string>>();


    static Dictionary<string, List<string>> _stepnametobutton = new Dictionary<string, List<string>>();

    static Dictionary<string, IUIAutomationElement> _elementtoiuireference = new Dictionary<string, IUIAutomationElement>();

    static Dictionary<string, Dictionary<string, string>> elementtoxpath = new Dictionary<string, Dictionary<string, string>>();
    static List<string> gridnames = new List<string> { "GridUnallocated", "grpBoxGrid", "pmDashboard", "pmGrid", "WorkingSubBlotterGrid", "OrderBlotterGrid", "grdLong", "grdShort", "grdNetPosition", "grdData", "grdCreatePosition", "GridAllocated", "gridRunUpload" };

    static Dictionary<string, AutomationPropertyChangedEventHandler> _eventhandledict = new Dictionary<string, AutomationPropertyChangedEventHandler>();
    static Dictionary<string, Dictionary<string, string>> _moduletoelementvalue = new Dictionary<string, Dictionary<string, string>>();

    //static Dictionary<KeyValuePair<string, string>, IUIAutomationElement> _elementtoiuireference = new Dictionary<KeyValuePair<string, string>, IUIAutomationElement>();

    static string _currenttabname = "";
    static Dictionary<string, List<string>> _tabname_to_stepname = new Dictionary<string, List<string>>();
    private static readonly object _fileLock = new object();
    public static IUIAutomationElement _globalcurrentwin = null;

    //<createorder,<txtsymbol,cmbquantity>
    //    <verifyordergrid,<"ordersblottergrid">

    //_actiontostepname cr
    static Dictionary<string, List<string>> _btntostepname = new Dictionary<string, List<string>>();
    private static Dictionary<string, List<string>> mappingCorrection = new Dictionary<string, List<string>>();

    private static string logFilename;

    static Dictionary<string, List<string>> alldata = new Dictionary<string, List<string>>();
    static Dictionary<string, string> exepaths = new Dictionary<string, string>();
    static IUIAutomationElement _currwinglobal = null;
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
    private static string _columnname = "";
    private static string _filterbasis = "";
    private static bool _filteron = false;
    static string _filterstep = "";
    static bool _uiInteraction = true;

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
    //    private static List<KeyValuePair<int, IUIAutomationElement>> findiuielementofrows(IUIAutomationElement currwin)
    //{
    //    List<KeyValuePair<int, IUIAutomationElement>> uIAutomationElements = new List<KeyValuePair<int, IUIAutomationElement>>();

    //    KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("Import Data", "Import Data");

    //    IUIAutomation automation = new CUIAutomation8();
    //    IUIAutomationCondition condition1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TreeControlTypeId);

    //    IUIAutomationElement grid = currwin.FindFirst(
    //       TreeScope.TreeScope_Descendants, condition1);

    //    IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "[Editor] Edit Area");
    //    IUIAutomationCondition conditionditems = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
    //    IUIAutomationCondition conditioncols = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderItemControlTypeId);

    //    if (grid != null)
    //    {

    //        IUIAutomationElementArray rowelements = grid.FindAll(TreeScope.TreeScope_Descendants, condition);
    //        IUIAutomationElementArray dataitems = grid.FindAll(TreeScope.TreeScope_Descendants, conditionditems);


    //        List<string> checkcols = new List<string>();


    //        if (dataitems != null)

    //        {


    //            string parentElement = "";
    //            string realvalue = "";
    //            for (int i = 0; i < dataitems.Length; i++)
    //            {
    //                bool cond1 = false;
    //                bool cond2 = false;
    //                bool cond3 = false;
    //                if (uIAutomationElements.Count == dictmap[keyValuePair]["Import Type"].Count)
    //                {
    //                    break;
    //                }

    //                IUIAutomationCondition uIAutomationCondition = automation.CreateTrueCondition();

    //                IUIAutomationElement e = dataitems.GetElement(i);
    //                IUIAutomationElementArray complexelements = e.FindAll(TreeScope.TreeScope_Children, uIAutomationCondition);
    //                if (complexelements != null)

    //                {
    //                    int idx = -1;
    //                    for (int j = 0; j < complexelements.Length; j++)
    //                    {

    //                        IUIAutomationElement cell = complexelements.GetElement(j);
    //                        realvalue = getnewvalue(cell);//value of cell

    //                        parentElement = cell.CurrentName;//column header

    //                        if (parentElement == "Upload ThirdParty")
    //                        {
    //                            idx = dictmap[keyValuePair]["Upload ThirdParty"].IndexOf(realvalue);
    //                            if (idx == -1)
    //                            {
    //                                break; // break from row 
    //                            }
    //                            else
    //                            {
    //                                cond1 = true;
    //                            }
    //                        }


    //                        if (parentElement == "Import Type" && cond1)
    //                        {
    //                            if (dictmap[keyValuePair]["Import Type"][idx] == realvalue)
    //                            {
    //                                cond2 = true;
    //                            }
    //                            else
    //                            {
    //                                break;
    //                            }
    //                        }
    //                        if (parentElement == "Import Type Format Name" && cond2)
    //                        {
    //                            if (dictmap[keyValuePair]["Import Type Format Name"][idx] == realvalue)
    //                            {
    //                                cond3 = true;
    //                            }
    //                            else
    //                            {
    //                                break;
    //                            }
    //                        }
    //                        if (cond1 && cond2 && cond3)
    //                        {
    //                            IUIAutomationElement parentrow = automation.ContentViewWalker.GetParentElement(cell);
    //                            if (parentrow != null)
    //                            {
    //                                uIAutomationElements.Add(new KeyValuePair<int, IUIAutomationElement>(idx, parentrow));
    //                            }
    //                            break;
    //                        }

    //                    }
    //                }


    //            }

    //        }



    //    }
    //    return uIAutomationElements;
    //}
    public static void OpenImportModule()
    {


        InputSimulator inputSimulator = new InputSimulator();
        inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
        inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
        inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_I);
        inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);
        inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
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
    public static bool CheckDuplicateFileImport(string uploadThirdParty, string importFileTypeFormatName, string importFilePath)
    {
        string logFile = Directory.GetCurrentDirectory() + $@"\{logFilePath}";
        string fileToSearch = $"{uploadThirdParty}    {importFileTypeFormatName}    " + importFilePath + "   :   Imported Successfully";

        try
        {
            string logContent = File.ReadAllText(logFile);

            if (logContent.Contains(fileToSearch))
            {
                return true;
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine($"File not found: {logFile}");
            CreateLogs($"File not found: {logFile}", logFilePath);
            CreateExceptionReport(ex.StackTrace);
            return false;
        }
;
        return false;
    }
    public static bool ImportFile(IUIAutomationElement currentwin, IUIAutomationElementArray grdRows, DataTable dataFromCsvFile, string uploadThirdParty, string importFileTypeFormatName, string importFilePath, string account, string date, int wait, string exportData, DataTable mainFileData, string exePath)
    {
        _globalcurrentwin = currentwin;
        int rowCount = 0;
        string checkDuplicateImport = GetValueFromMainFile(mainFileData, "duplicateFileCheck");

        if (checkDuplicateImport.ToLower() == "yes")
        {
            bool isDuplicateFileImport = CheckDuplicateFileImport(uploadThirdParty, importFileTypeFormatName, importFilePath);
            if (isDuplicateFileImport == true)
            {
                Console.WriteLine($"{importFilePath} : Already Imported!!");
                CreateLogs($"{importFilePath} : Already Imported!!", logFilePath);
                exitcode = 3;
                return false;
            }
        }
        IUIAutomation automation = new CUIAutomation8();
        tagPOINT p;

        for (int i = 0; i < grdRows.Length; i++)
        {
            IUIAutomationElement element = grdRows.GetElement(i);

            try
            {

                //...................

                //BringToForeground("Import Data", ref currentwin, exePath);

                var condUploadThirdParty = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Upload ThirdParty");
                var condImportTypeFormatName = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "27");
                //...............
                IUIAutomationElement ImportTypeFormatName = element.FindFirst(TreeScope.TreeScope_Descendants, condImportTypeFormatName);
                IUIAutomationElement UploadThirdParty = element.FindFirst(TreeScope.TreeScope_Descendants, condUploadThirdParty);

                string importTypeFormatNameFromUI = getnewvalue(ImportTypeFormatName);
                string uploadThirdPartyFromUI = getnewvalue(UploadThirdParty);
                ++rowCount;

                if (uploadThirdPartyFromUI == uploadThirdParty && importTypeFormatNameFromUI == importFileTypeFormatName)
                {
                    //BringToForeground("Import Data", ref currentwin, exePath);
                    var row = element;
                    InputSimulator simulator = new InputSimulator();
                    //....................

                    object scrollitemobj;
                    IUIAutomationElement checkbox113 = null;
                    IUIAutomationElement checkbox = null;
                    IUIAutomationElement btnUpload = null;
                    if (row.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                    {
                        scrollitemobj = row.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                        IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                        selectionpatternprovider.ScrollIntoView();

                    }

                    IUIAutomationCondition condchckbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_CheckBoxControlTypeId);
                    IUIAutomationCondition condchckbox2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Select Record");

                    IUIAutomationCondition condandchckbox = automation.CreateAndCondition(condchckbox1, condchckbox2);
                    checkbox = row.FindFirst(TreeScope.TreeScope_Descendants, condandchckbox);

                    if (checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                    {
                        scrollitemobj = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                        IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                        selectionpatternprovider.ScrollIntoView();

                    }
                    replayaction(checkbox, "");

                    if (account != "")
                    {
                        condchckbox1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "45");

                        IUIAutomationElement e1 = row.FindFirst(TreeScope.TreeScope_Descendants, condchckbox1);
                        if (e1.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                        {
                            scrollitemobj = e1.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                            IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                            selectionpatternprovider.ScrollIntoView();

                        }

                        replayaction(e1, account);

                    }
                    if (date != "")
                    {
                        IUIAutomationCondition condchckbox111 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_CheckBoxControlTypeId);
                        IUIAutomationCondition condchckbox112 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "46");

                        IUIAutomationCondition condandchckbox113 = automation.CreateAndCondition(condchckbox111, condchckbox112);
                        checkbox113 = row.FindFirst(TreeScope.TreeScope_Descendants, condandchckbox113);
                        if (checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                        {
                            scrollitemobj = checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                            IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                            selectionpatternprovider.ScrollIntoView();

                        }
                        replayaction(checkbox113, "");

                        condchckbox112 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "47");
                        IUIAutomationElement checkbox1133 = null;
                        checkbox1133 = row.FindFirst(TreeScope.TreeScope_Descendants, condchckbox112);
                        if (checkbox1133.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                        {
                            scrollitemobj = checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                            IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                            selectionpatternprovider.ScrollIntoView();

                        }
                        string processDateForUI = processDate.ToString("MMddyyyy");
                        if (!processDateForUI.Contains("/"))
                        {
                            // If not, add slashes using string interpolation
                            processDateForUI = $"{processDateForUI.Substring(0, 2)}/{processDateForUI.Substring(2, 2)}/{processDateForUI.Substring(4)}";
                        }
                        replayaction(checkbox1133, processDateForUI);
                    }


                    BringToForeground("Import Data", ref currentwin, exePath);
                    IUIAutomationCondition condfilepath1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId);
                    IUIAutomationCondition condfilepath2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Select File");
                    IUIAutomationCondition condand = automation.CreateAndCondition(condfilepath1, condfilepath2);
                    IUIAutomationElement cellselectfile = row.FindFirst(TreeScope.TreeScope_Descendants, condand);
                    if (cellselectfile.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                    {
                        scrollitemobj = cellselectfile.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                        IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                        selectionpatternprovider.ScrollIntoView();

                    }
                    if (cellselectfile != null)
                    {
                        // Get the bounding rectangle of the element
                        // Simulate a mouse click at the clickable point
                        cellselectfile.GetClickablePoint(out p);
                        System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)p.x, (int)p.y);

                        simulator.Mouse.LeftButtonClick();
                        Thread.Sleep(2000);
                        IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Select File to Import");
                        IUIAutomationElement dialogbox = currentwin.FindFirst(TreeScope.TreeScope_Descendants, conddialogbox);

                        if (dialogbox != null)
                        {
                            IUIAutomationCondition conditionfilename = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId);
                            IUIAutomationCondition conditionfilename1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "File name:");
                            IUIAutomationAndCondition newandcond = (IUIAutomationAndCondition)automation.CreateAndCondition(conditionfilename1, conditionfilename);
                            IUIAutomationElement filename = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, newandcond);
                            BringToForeground("Select File to Import", ref currentwin, exePath);
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
                                Thread.Sleep(500);
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


                        }
                        try
                        {

                            IUIAutomationCondition selectFileAlertcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Select File to Import");
                            IUIAutomationCondition okbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "OK");


                            var selectFileAlert = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, selectFileAlertcond);
                            if (selectFileAlert == null)
                            {
                                throw new Exception("everything is ok");
                            }
                            // var txt = window.FindFirst(TreeScope.TreeScope_Descendants,okbtncond);
                            string logMsg = uploadThirdParty + "    " + importFileTypeFormatName + "    " + importFilePath + "   :   " + "Path Not Found !!!";
                            Console.WriteLine(logMsg);
                            CreateLogs(logMsg, logFilePath);

                            var btn = selectFileAlert.FindFirst(TreeScope.TreeScope_Descendants, okbtncond);
                            replayaction(btn, "");
                            Thread.Sleep(500);
                            IUIAutomationCondition titlebardialogboxcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "TitleBar");

                            IUIAutomationCondition closedialogboxcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "Close");

                            IUIAutomationElement TitleBar = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, titlebardialogboxcond);
                            IUIAutomationElement cross = TitleBar.FindFirst(TreeScope.TreeScope_Descendants, closedialogboxcond);
                            replayaction(cross, "");
                            Thread.Sleep(500);
                            if (date != "")
                            {
                                if (checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                {
                                    scrollitemobj = checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                    IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                                    selectionpatternprovider.ScrollIntoView();

                                }
                                replayaction(checkbox113, "");
                                Thread.Sleep(500);
                            }
                            if (checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                            {
                                scrollitemobj = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                                selectionpatternprovider.ScrollIntoView();

                            }
                            replayaction(checkbox, "");
                            Thread.Sleep(500);
                            exitcode = 13;
                            return false;

                        }

                        catch (Exception ex1)
                        {
                            
                            IUIAutomationCondition uploadcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnUpload");
                            try
                            {

                                btnUpload = currentwin.FindFirst(TreeScope.TreeScope_Descendants, uploadcond);
                                BringToForeground("Import Data", ref currentwin, exePath);
                                Thread modalThread = new Thread(HandleModalDialog);
                                modalThread.Start();
                                
                                ClickElement(btnUpload, "Left Mouse Button Clicked");
                                CreateExceptionReport(_currwinglobal.CurrentName);
                                modalThread.Join();
                                Thread.Sleep(1000);
                            }

                            catch (Exception ex)
                            {
                                CreateExceptionReport(ex.Message);
                                CreateExceptionReport(ex.StackTrace);

                            }
                            CreateExceptionReport(ex1.StackTrace);
                        }

                        try
                        {
                            
                            if (_globalApplicationerror == true)
                            {
                                _globalApplicationerror = false;

                                string logmsg = uploadThirdParty + "    " + importFileTypeFormatName + "    " + importFilePath + "    :    " + _global_appication_error_msg;
                                Console.WriteLine(logmsg);
                                CreateLogs(logmsg, logFilePath);
                                _global_appication_error_msg = "";

                                if (date != "")
                                {
                                    if (checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                    {
                                        scrollitemobj = checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                        IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                                        selectionpatternprovider.ScrollIntoView();

                                    }
                                    replayaction(checkbox113, "");
                                }
                                if (checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                {
                                    scrollitemobj = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                    IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                                    selectionpatternprovider.ScrollIntoView();

                                }
                                replayaction(checkbox, "");
                                exitcode = 13;
                                return false;
                            }
                            else
                            {


                                IUIAutomationCondition impPositionscond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ImportPositionsDisplayForm");

                                IUIAutomationElement impPositions = currentwin.FindFirst(TreeScope.TreeScope_Descendants, impPositionscond);


                                IUIAutomationCondition validStatusBtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Validation Status");

                                IUIAutomationElement validStatusBtn = impPositions.FindFirst(TreeScope.TreeScope_Descendants, validStatusBtncond);

                                IUIAutomationCondition grdcond = null;

                                IUIAutomationElement grd = null;
                                IUIAutomationCondition treecond = null;

                                IUIAutomationElement tree = null;
                                //"Validation Status : Validated"
                                IUIAutomationCondition dataitemscond = null;

                                IUIAutomationElementArray dataitems = null;


                                try
                                {
                                    int maxRetries = 50;
                                    int retryDelayMilliseconds = 1000; // 1 second

                                    for (int retryCount = 0; retryCount < maxRetries; retryCount++)
                                    {
                                        try
                                        {
                                            // Your UI automation code here
                                            grdcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "grdImportData");
                                            CreateExceptionReport("before finding importpositiongrid");
                                            grd = impPositions.FindFirst(TreeScope.TreeScope_Descendants, grdcond);
                                            CreateExceptionReport("after finding importpositiongrid");
                                            if (grd != null)
                                            {
                                                CreateExceptionReport(" importpositiongrid is not null");
                                            }
                                            treecond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TreeControlTypeId);

                                            tree = grd.FindFirst(TreeScope.TreeScope_Descendants, treecond);
                                            //"Validation Status : Validated"
                                            dataitemscond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);

                                            dataitems = tree.FindAll(TreeScope.TreeScope_Children, dataitemscond);


                                            // If the code above executes successfully, break out of the retry loop
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            CreateExceptionReport($"{retryCount} - Error during UI automation: {ex.Message}");

                                            // Add a delay before the next retry
                                            Thread.Sleep(retryDelayMilliseconds);
                                            CreateExceptionReport(ex.StackTrace);
                                        }
                                    }
                                }
                                catch (Exception mainException)
                                {
                                    CreateExceptionReport($"Main exception: {mainException.Message}");
                                    CreateExceptionReport(mainException.StackTrace);
                                }
                                //


                                //
                                //  loop all dataitem check if there is any grid othe rthan validated  if yes then make abort true and nreak the loop


                                bool abort = true;
                                if (dataitems.Length > 0)
                                {
                                    for (int k = 0; k < dataitems.Length; k++)
                                    {
                                        IUIAutomationElement dataitem = dataitems.GetElement(k);
                                        if (dataitem != null)
                                        {
                                            string validationstatus = dataitem.CurrentName;
                                            if (validationstatus.Contains("Validation Status : Validated") && dataitems.Length == 1)
                                            {


                                                abort = false;
                                            }
                                        }
                                    }
                                }
                                // then check if abort is true then abort and dont untick the date and check box

                                if (abort == true)
                                {

                                    IUIAutomationElement contiueRoot = null;
                                    contiueRoot = automation.ContentViewWalker.GetFirstChildElement(impPositions);
                                    contiueRoot = automation.ContentViewWalker.GetNextSiblingElement(contiueRoot);

                                    IUIAutomationCondition abortBtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Abort");
                                    BringToForeground("Import Positions", ref currentwin, exePath);
                                    IUIAutomationElement abortBtn = contiueRoot.FindFirst(TreeScope.TreeScope_Descendants, abortBtncond);
                                    ClickElement(abortBtn, "Left Mouse Button Clicked");
                                    Thread.Sleep(500);
                                    IUIAutomationCondition ImportDialogcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Import");

                                    IUIAutomationElement ImportDialog = impPositions.FindFirst(TreeScope.TreeScope_Descendants, ImportDialogcond);
                                    IUIAutomationCondition Yesbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "6");

                                    IUIAutomationElement Yesbtn = ImportDialog.FindFirst(TreeScope.TreeScope_Children, Yesbtncond);
                                    ClickElement(Yesbtn, "Left Mouse Button Clicked");

                                    Thread.Sleep(500);


                                    Thread.Sleep(500);

                                    IUIAutomationCondition uploadcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "btnUpload");
                                    try
                                    {

                                        BringToForeground("Import Data", ref currentwin, exePath);
                                        btnUpload = currentwin.FindFirst(TreeScope.TreeScope_Descendants, uploadcond);
                                        Thread modalThread = new Thread(HandleModalDialog);
                                        modalThread.Start();

                                        ClickElement(btnUpload, "Left Mouse Button Clicked");
                                        CreateExceptionReport(_currwinglobal.CurrentName);
                                        modalThread.Join();
                                        Thread.Sleep(1000);
                                    }

                                    catch (Exception ex)
                                    {
                                        CreateExceptionReport(ex.StackTrace);
                                    }
                                    string logMsg = uploadThirdParty + "    " + importFileTypeFormatName + "    " + importFilePath + "   :   " + "First tried abort";
                                    Console.WriteLine(logMsg);
                                    CreateLogs(logMsg, logFilePath);


                                }
                                //then again click upload button and then export do the rest thing
                                

                                impPositionscond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ImportPositionsDisplayForm");

                                impPositions = currentwin.FindFirst(TreeScope.TreeScope_Descendants, impPositionscond);


                                validStatusBtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Validation Status");

                                validStatusBtn = impPositions.FindFirst(TreeScope.TreeScope_Descendants, validStatusBtncond);

                                grdcond = null;

                                grd = null;
                                treecond = null;

                                tree = null;
                                //"Validation Status : Validated"
                                dataitemscond = null;

                                dataitems = null;

                                //if (exportData == "true")
                                //{
                                //    ClickElement(validStatusBtn, "Right Mouse Button Clicked");
                                //    Thread.Sleep(500);
                                //    IUIAutomationCondition DropDowncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "DropDown");

                                //    IUIAutomationElement DropDown = impPositions.FindFirst(TreeScope.TreeScope_Descendants, DropDowncond);
                                //    IUIAutomationCondition exportcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Export Data");

                                //    IUIAutomationElement export = DropDown.FindFirst(TreeScope.TreeScope_Descendants, exportcond);
                                //    ClickElement(export, "Left Mouse Button Clicked");
                                //    Thread.Sleep(2000);
                                //    simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                                //    string exportpath = "Exported_Data";
                                //    //if (!Directory.Exists(exportpath))
                                //    //{
                                //    //    Directory.CreateDirectory(exportpath);
                                //    //}
                                //    //}

                                //    //string exportFileName = Directory.GetCurrentDirectory() + @"\Exported_Data\";
                                //    string fullPath = Path.GetDirectoryName(importFilePath);
                                //    string fullPathbackup = Path.GetDirectoryName(importFilePath) + @"\Backup\";

                                //    string formattedDate = currentDate.ToString("ddMM");
                                //    //string fullPathCsv = currentDirectory + $@"\Downloaded Files\{brokerName}\{brokerName}{formattedDate}";
                                //    //string fullPath = exportFileName + $@"{uploadThirdParty}\";
                                //    //string fullPathbackup = exportFileNamebackup + $@"{uploadThirdParty}\";



                                //    string outputFolder = fullPath;
                                //    string outputFolderbackup = fullPathbackup + $@"{uploadThirdParty}{formattedDate}_{DateTime.Now.ToString("HHmmss")}";



                                //    IUIAutomationCondition SaveAsElementcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Save As");

                                //    IUIAutomationElement SaveAsElement = impPositions.FindFirst(TreeScope.TreeScope_Descendants, SaveAsElementcond);
                                //    IUIAutomationCondition ExportFileInputcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "1001");

                                //    IUIAutomationElement ExportFileInput = SaveAsElement.FindFirst(TreeScope.TreeScope_Descendants, ExportFileInputcond);
                                //    IUIAutomationCondition savecond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Save");

                                //    IUIAutomationElement save = SaveAsElement.FindFirst(TreeScope.TreeScope_Descendants, savecond);

                                //    string fileNameWithExtenstion = Path.GetFileName(importFilePath);

                                //    string outputFileName = outputFolder + $@"\ExportedFile_{uploadThirdParty}_{importFileTypeFormatName}_{fileNameWithExtenstion}";
                                //    string outputFileNamebackup = outputFolderbackup + $@"\ExportedFile_{uploadThirdParty}_{importFileTypeFormatName}_{fileNameWithExtenstion}";

                                //    if (File.Exists(outputFileName))
                                //    {
                                //        // Move the existing file to the new output folder
                                //        if (!Directory.Exists(outputFolderbackup))
                                //        {
                                //            Directory.CreateDirectory(outputFolderbackup);
                                //        }

                                //        // Move the existing file to the new output folder
                                //        File.Move(outputFileName, outputFileNamebackup);
                                //        Thread.Sleep(200);
                                //        CreateExceptionReport($"File already exists. Moved to: {outputFileNamebackup}");

                                //    }

                                //    replayaction(ExportFileInput, outputFileName);
                                //    Thread.Sleep(400);
                                //    ClickElement(save, "Left Mouse Button Clicked");
                                //    Thread.Sleep(400);




                                //    try
                                //    {
                                //        IUIAutomationCondition ConfirmPopUpcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Confirm Save As");

                                //        IUIAutomationElement ConfirmPopUp = SaveAsElement.FindFirst(TreeScope.TreeScope_Descendants, ConfirmPopUpcond);
                                //        IUIAutomationCondition Yescond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                                //        IUIAutomationElement Yes = SaveAsElement.FindFirst(TreeScope.TreeScope_Descendants, Yescond);
                                //    }
                                //    catch
                                //    {

                                //    }

                                //    //!!!!!!!!!!!!!!!!!!!!!!!more Code to be done 
                                //}
                                try
                                {
                                    if (exportData == "true")
                                    {
                                        BringToForeground("Import Positions", ref currentwin, exePath);
                                        ClickElement(validStatusBtn, "Right Mouse Button Clicked");
                                        Thread.Sleep(500);
                                        IUIAutomationCondition DropDowncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "DropDown");

                                        IUIAutomationElement DropDown = impPositions.FindFirst(TreeScope.TreeScope_Descendants, DropDowncond);
                                        IUIAutomationCondition exportcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Export Data");

                                        IUIAutomationElement export = DropDown.FindFirst(TreeScope.TreeScope_Descendants, exportcond);
                                        ClickElement(export, "Left Mouse Button Clicked");
                                        Thread.Sleep(2000);
                                        simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);

                                    }
                                    int maxRetries = 50;
                                    int retryDelayMilliseconds = 1000; // 1 second

                                    for (int retryCount = 0; retryCount < maxRetries; retryCount++)
                                    {
                                        try
                                        {
                                            if (exportData == "true")
                                            {
                                                string exportpath = "Exported_Data";
                                                //if (!Directory.Exists(exportpath))
                                                //{
                                                //    Directory.CreateDirectory(exportpath);
                                                //}
                                                //}

                                                //string exportFileName = Directory.GetCurrentDirectory() + @"\Exported_Data\";
                                                string fullPath = Path.GetDirectoryName(importFilePath);
                                                string fullPathbackup = Path.GetDirectoryName(importFilePath) + @"\Backup\";

                                                string formattedDate = currentDate.ToString("ddMM");
                                                //string fullPathCsv = currentDirectory + $@"\Downloaded Files\{brokerName}\{brokerName}{formattedDate}";
                                                //string fullPath = exportFileName + $@"{uploadThirdParty}\";
                                                //string fullPathbackup = exportFileNamebackup + $@"{uploadThirdParty}\";



                                                string outputFolder = fullPath;
                                                string outputFolderbackup = fullPathbackup + $@"{uploadThirdParty}{formattedDate}_{DateTime.Now.ToString("HHmmss")}";


                                                
                                                IUIAutomationCondition SaveAsElementcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Save As");

                                                IUIAutomationElement SaveAsElement = impPositions.FindFirst(TreeScope.TreeScope_Descendants, SaveAsElementcond);
                                                IUIAutomationCondition ExportFileInputcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "1001");

                                                IUIAutomationElement ExportFileInput = SaveAsElement.FindFirst(TreeScope.TreeScope_Descendants, ExportFileInputcond);
                                                IUIAutomationCondition savecond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Save");

                                                IUIAutomationElement save = SaveAsElement.FindFirst(TreeScope.TreeScope_Descendants, savecond);
                                                BringToForeground("Save As", ref currentwin, exePath);
                                                string fileNameWithExtenstion = Path.GetFileName(importFilePath);

                                                string outputFileName = outputFolder + $@"\ExportedFile_{uploadThirdParty}_{importFileTypeFormatName}_{fileNameWithExtenstion}";
                                                string outputFileNamebackup = outputFolderbackup + $@"\ExportedFile_{uploadThirdParty}_{importFileTypeFormatName}_{fileNameWithExtenstion}";

                                                if (File.Exists(outputFileName))
                                                {
                                                    // Move the existing file to the new output folder
                                                    if (!Directory.Exists(outputFolderbackup))
                                                    {
                                                        Directory.CreateDirectory(outputFolderbackup);
                                                    }

                                                    // Move the existing file to the new output folder
                                                    File.Move(outputFileName, outputFileNamebackup);
                                                    Thread.Sleep(200);
                                                    CreateExceptionReport($"File already exists. Moved to: {outputFileNamebackup}");

                                                }
                                                //BringToForeground("Save As", ref currentwin, exePath);
                                                replayaction(ExportFileInput, outputFileName);
                                                Thread.Sleep(400);
                                                ClickElement(save, "Left Mouse Button Clicked");
                                                Thread.Sleep(400);




                                                try
                                                {
                                                    IUIAutomationCondition ConfirmPopUpcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Confirm Save As");

                                                    IUIAutomationElement ConfirmPopUp = SaveAsElement.FindFirst(TreeScope.TreeScope_Descendants, ConfirmPopUpcond);
                                                    IUIAutomationCondition Yescond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Yes");

                                                    IUIAutomationElement Yes = SaveAsElement.FindFirst(TreeScope.TreeScope_Descendants, Yescond);
                                                }
                                                catch (Exception ex)
                                                {
                                                    CreateExceptionReport(ex.StackTrace);

                                                }

                                                //!!!!!!!!!!!!!!!!!!!!!!!more Code to be done 
                                            }
                                            // Your UI automation code here



                                            // If the code above executes successfully, break out of the retry loop
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            CreateExceptionReport($"{retryCount} - Error during UI automation: {ex.Message}");

                                            // Add a delay before the next retry
                                            CreateExceptionReport(ex.StackTrace);
                                            Thread.Sleep(retryDelayMilliseconds);
                                            //exitcode = 23;
                                        }
                                    }
                                }
                                catch (Exception mainException)
                                {
                                    CreateExceptionReport($"Main exception: {mainException.Message}");
                                    // exitcode = 23;
                                }
                                Thread.Sleep(2000);

                                try
                                {
                                    int maxRetries = 50;
                                    int retryDelayMilliseconds = 1000; // 1 second

                                    for (int retryCount = 0; retryCount < maxRetries; retryCount++)
                                    {
                                        try
                                        {
                                            // Your UI automation code here
                                            BringToForeground("Import Data", ref currentwin, exePath);
                                            grdcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "grdImportData");
                                            CreateExceptionReport("before finding importpositiongrid");
                                            grd = impPositions.FindFirst(TreeScope.TreeScope_Descendants, grdcond);
                                            CreateExceptionReport("after finding importpositiongrid");
                                            if (grd != null)
                                            {
                                                CreateExceptionReport(" importpositiongrid is not null");
                                            }
                                            treecond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TreeControlTypeId);

                                            tree = grd.FindFirst(TreeScope.TreeScope_Descendants, treecond);
                                            //"Validation Status : Validated"
                                            dataitemscond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);

                                            dataitems = tree.FindAll(TreeScope.TreeScope_Children, dataitemscond);


                                            // If the code above executes successfully, break out of the retry loop
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            CreateExceptionReport($"{retryCount} - Error during UI automation: {ex.Message}");

                                            // Add a delay before the next retry
                                            Thread.Sleep(retryDelayMilliseconds);
                                            CreateExceptionReport(ex.StackTrace);
                                            //exitcode = 23;
                                        }
                                    }
                                }
                                catch (Exception mainException)
                                {
                                    CreateExceptionReport($"Main exception: {mainException.Message}");
                                    exitcode = 19;
                                    CreateExceptionReport(mainException.StackTrace);
                                }
                                //

                                abort = true;
                                if (dataitems.Length > 0)
                                {
                                    for (int k = 0; k < dataitems.Length; k++)
                                    {
                                        IUIAutomationElement dataitem = dataitems.GetElement(k);
                                        if (dataitem != null)
                                        {
                                            string validationstatus = dataitem.CurrentName;
                                            if (validationstatus.Contains("Validation Status : Validated"))
                                            {
                                                IUIAutomationCondition headercond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderControlTypeId);
                                                IUIAutomationElement header = dataitem.FindFirst(TreeScope.TreeScope_Descendants, headercond);

                                                abort = false;
                                                if (dataitem.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) != null)
                                                {
                                                    scrollitemobj = dataitem.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);

                                                    IUIAutomationExpandCollapsePattern selectionpatternprovider = scrollitemobj as IUIAutomationExpandCollapsePattern;
                                                    if (selectionpatternprovider.CurrentExpandCollapseState == ExpandCollapseState.ExpandCollapseState_Collapsed)
                                                    {

                                                        selectionpatternprovider.Expand();
                                                        Thread.Sleep(1000);
                                                    }

                                                }
                                                if (header.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                                {
                                                    scrollitemobj = header.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                                    IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                                                    selectionpatternprovider.ScrollIntoView();
                                                    Thread.Sleep(500);

                                                }

                                                //"[Column Header] checkBox"
                                                IUIAutomationCondition columnheaderchkboxcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "[Column Header] checkBox");
                                                IUIAutomationCondition columnheaderchkboxcond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderItemControlTypeId);
                                                IUIAutomationCondition columnheaderchkboxcondAND = automation.CreateAndCondition(columnheaderchkboxcond2, columnheaderchkboxcond);
                                                IUIAutomationElement columnheaderchkbox = dataitem.FindFirst(TreeScope.TreeScope_Descendants, columnheaderchkboxcondAND);
                                                // IUIAutomationElementArray columnheaderchkboxs = dataitem.FindAll(TreeScope.TreeScope_Descendants, columnheaderchkboxcondAND);
                                                //if(columnheaderchkboxs.Length > 0)
                                                //{
                                                //    CreateExceptionReport("m");
                                                //}
                                                //if (columnheaderchkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                                //{
                                                //    scrollitemobj = columnheaderchkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                                //    IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                                                //    selectionpatternprovider.ScrollIntoView();
                                                //    Thread.Sleep(100);


                                                //}
                                                BringToForeground("Import Positions", ref currentwin, exePath);
                                                ClickElement(columnheaderchkbox, "Left Mouse Button Clicked");
                                                Thread.Sleep(100);
                                                try
                                                {
                                                    IUIAutomationElement continueBtn = null;
                                                    int maxRetries = 100;
                                                    int retryDelayMilliseconds = 2000; // 1 second
                                                    Thread.Sleep(500);
                                                    for (int retryCount = 0; retryCount < maxRetries; retryCount++)
                                                    {
                                                        try
                                                        {
                                                            IUIAutomationElement contiueRoot = null;
                                                            contiueRoot = automation.ContentViewWalker.GetFirstChildElement(impPositions);
                                                            contiueRoot = automation.ContentViewWalker.GetNextSiblingElement(contiueRoot);
                                                            IUIAutomationCondition continueBtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Continue");
                                                            CreateExceptionReport("before finding continue button");
                                                            continueBtn = contiueRoot.FindFirst(TreeScope.TreeScope_Descendants, continueBtncond);
                                                            CreateExceptionReport("after finding continue button");

                                                            //Thread modalThread = new Thread(HandleModalDialog);
                                                            //modalThread.Start();
                                                            if (continueBtn == null)
                                                            {
                                                                throw new Exception("Continue  not found");
                                                            }
                                                            // replayaction(continueBtn, "");


                                                            // If the code above executes successfully, break out of the retry loop
                                                            break;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            CreateExceptionReport($"{retryCount} - Error during finding and clicking Continue: {ex.Message}");

                                                            // Add a delay before the next retry
                                                            Thread.Sleep(retryDelayMilliseconds);
                                                            CreateExceptionReport(ex.StackTrace);

                                                            // exitcode = 23;
                                                        }
                                                    }
                                                    if (continueBtn != null)
                                                    {
                                                        BringToForeground("Import Positions", ref currentwin, exePath);
                                                        CreateExceptionReport("Going to click continue button");
                                                        ClickElement(continueBtn, "Left Mouse Button Clicked");
                                                        CreateExceptionReport("After Clicking Continue button");
                                                    }
                                                    else
                                                    {
                                                        CreateExceptionReport("conitune button is null");
                                                    }
                                                }
                                                catch (Exception mainException)
                                                {
                                                    CreateExceptionReport($"Main exception: {mainException.Message}");
                                                    CreateExceptionReport(mainException.StackTrace);
                                                }

                                                try
                                                {

                                                    //pBarProgressing
                                                    Thread.Sleep(200);
                                                    IUIAutomationCondition progressbarcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "pBarProgressing");
                                                    IUIAutomationElement progressbar = currentwin.FindFirst(TreeScope.TreeScope_Descendants, progressbarcond);
                                                    string value = "";
                                                    if (progressbar != null)
                                                    {
                                                        if (progressbar.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                                                        {
                                                            scrollitemobj = progressbar.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                                                            IUIAutomationValuePattern selectionpatternprovider = scrollitemobj as IUIAutomationValuePattern;
                                                            value = selectionpatternprovider.CurrentValue;
                                                        }
                                                        while (progressbar != null && value != "100%")
                                                        {
                                                            progressbar = currentwin.FindFirst(TreeScope.TreeScope_Descendants, progressbarcond);
                                                            scrollitemobj = progressbar.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                                                            IUIAutomationValuePattern selectionpatternprovider = scrollitemobj as IUIAutomationValuePattern;
                                                            value = selectionpatternprovider.CurrentValue;
                                                        }

                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    //exitcode = 23;
                                                    CreateExceptionReport(ex.StackTrace);

                                                }


                                                //modalThread.Join();
                                                Thread.Sleep(500);
                                                try
                                                {

                                                    //.........................
                                                    try
                                                    {
                                                        int maxRetries = 10;
                                                        int retryDelayMilliseconds = 500; // 1 second

                                                        for (int retryCount = 0; retryCount < maxRetries; retryCount++)
                                                        {
                                                            try
                                                            {
                                                                // Your UI automation code here
                                                                IUIAutomationCondition successMsgcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ClassNamePropertyId, "#32770");

                                                                IUIAutomationElement successMsg = currentwin.FindFirst(TreeScope.TreeScope_Descendants, successMsgcond);
                                                                IUIAutomationCondition OkMsgcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "OK");

                                                                IUIAutomationElement OkMsg = successMsg.FindFirst(TreeScope.TreeScope_Descendants, OkMsgcond);
                                                                if (OkMsg != null)
                                                                {
                                                                    CreateExceptionReport("OK button found in POP UP!!");
                                                                    replayaction(OkMsg, "");
                                                                    break;
                                                                }

                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                CreateExceptionReport($"{retryCount} - Error during UI automation: {ex.Message}");
                                                                //exitcode = 23;
                                                                // Add a delay before the next retry
                                                                //CreateLogs($"{retryCount} - Error during UI automation: {ex.Message}", logFilePath);
                                                                Thread.Sleep(retryDelayMilliseconds);
                                                                CreateExceptionReport(ex.StackTrace);
                                                            }
                                                        }
                                                    }
                                                    catch (Exception mainException)
                                                    {
                                                        CreateExceptionReport($"Main exception: {mainException.Message}");
                                                        CreateExceptionReport(mainException.StackTrace);
                                                        CreateLogs(mainException.StackTrace, logFilePath);
                                                        // exitcode = 23;
                                                    }

                                                    Thread.Sleep(400);
                                                    //.....................

                                                    if (date != "")
                                                    {
                                                        if (checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                                        {
                                                            scrollitemobj = checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                                            IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                                                            selectionpatternprovider.ScrollIntoView();

                                                        }
                                                        replayaction(checkbox113, "");
                                                        Thread.Sleep(500);
                                                    }
                                                    if (checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                                    {
                                                        scrollitemobj = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                                        IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                                                        selectionpatternprovider.ScrollIntoView();

                                                    }
                                                    replayaction(checkbox, "");
                                                    Thread.Sleep(500);
                                                    string logMsg = uploadThirdParty + "    " + importFileTypeFormatName + "    " + importFilePath + "   :   " + "Imported Successfully !!";
                                                    Console.WriteLine(logMsg);
                                                    CreateLogs(logMsg, logFilePath);
                                                    successCounter++;

                                                    return true;
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (date != "")
                                                    {
                                                        if (checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                                        {
                                                            scrollitemobj = checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                                            IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                                                            selectionpatternprovider.ScrollIntoView();

                                                        }
                                                        replayaction(checkbox113, "");
                                                        Thread.Sleep(500);
                                                    }
                                                    if (checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                                    {
                                                        scrollitemobj = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                                        IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                                                        selectionpatternprovider.ScrollIntoView();

                                                    }
                                                    replayaction(checkbox, "");
                                                    Thread.Sleep(500);
                                                    //string logMsg = uploadThirdParty + "    " + importFileTypeFormatName + "    " + importFilePath + "   :   " + "Imported Successfully !!";
                                                    //CreateLogs(logMsg, logFilePath);

                                                    return true;
                                                }

                                            }

                                        }
                                    }
                                    if (abort == true)
                                    {

                                        IUIAutomationElement contiueRoot = null;
                                        contiueRoot = automation.ContentViewWalker.GetFirstChildElement(impPositions);
                                        contiueRoot = automation.ContentViewWalker.GetNextSiblingElement(contiueRoot);

                                        IUIAutomationCondition abortBtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Abort");

                                        IUIAutomationElement abortBtn = contiueRoot.FindFirst(TreeScope.TreeScope_Descendants, abortBtncond);
                                        BringToForeground("Import Positions", ref currentwin, exePath);
                                        ClickElement(abortBtn, "Left Mouse Button Clicked");
                                        Thread.Sleep(500);
                                        IUIAutomationCondition ImportDialogcond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Import");

                                        IUIAutomationElement ImportDialog = impPositions.FindFirst(TreeScope.TreeScope_Descendants, ImportDialogcond);



                                        //IUIAutomationCondition ImportDialogcond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId,"txtloginid");

                                        IUIAutomationCondition Yesbtncond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "6");

                                        IUIAutomationElement Yesbtn = ImportDialog.FindFirst(TreeScope.TreeScope_Children, Yesbtncond);
                                        ClickElement(Yesbtn, "Left Mouse Button Clicked");

                                        Thread.Sleep(500);

                                        if (date != "")
                                        {
                                            if (checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                            {
                                                scrollitemobj = checkbox113.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                                IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                                                selectionpatternprovider.ScrollIntoView();

                                            }
                                            replayaction(checkbox113, "");
                                            Thread.Sleep(500);
                                        }
                                        if (checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null)
                                        {
                                            scrollitemobj = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);

                                            IUIAutomationScrollItemPattern selectionpatternprovider = scrollitemobj as IUIAutomationScrollItemPattern;
                                            selectionpatternprovider.ScrollIntoView();

                                        }
                                        replayaction(checkbox, "");
                                        Thread.Sleep(500);
                                        string logMsg = uploadThirdParty + "    " + importFileTypeFormatName + "    " + importFilePath + "   :   " + "Not Imported!!";
                                        Console.WriteLine(logMsg);
                                        CreateLogs(logMsg, logFilePath);
                                        exitcode = 13;
                                        return false;
                                    }

                                }



                            }


                        }
                        catch (Exception ex)
                        {
                            // exitcode = 23;
                            CreateExceptionReport(ex.StackTrace);
                        }




                    }



                    //........................
















                }
                else
                {
                    if ((uploadThirdPartyFromUI != uploadThirdParty || importTypeFormatNameFromUI != importFileTypeFormatName) && grdRows.Length == rowCount)
                    {
                        string logMsg = uploadThirdParty + "    " + importFileTypeFormatName + "    " + importFilePath + "    " + " Element Not Found On UI";
                        Console.WriteLine(logMsg);
                        CreateLogs(logMsg, logFilePath);
                        exitcode = 13;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.Message);
                CreateExceptionReport(ex.StackTrace);
                exitcode = 13;
                return false;

            }


        }
        exitcode = 13;
        return false;

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
                    if (processPath != null && processPath.Equals(exePath))
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

    public static void importreplayer(DataTable dataFromCsvFile, DataTable mainFileData, string exePath)
    {
        try
        {
            int i = 0;
            
            bool isLogin = false;
            //IntPtr handle = IntPtr.Zero;
            //isLogin = findpranaoncurrentsession(exePath);
            isLogin = IsClientLogin(exePath);

            if (!isLogin)
            {
                isLogin = Login(mainFileData, exePath);
            }
            //todo: have to remove this using check for foreground window 
            //Thread.Sleep(5000);
            if (isLogin == false)
            {
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
            OpenImportModule();

            Thread.Sleep(5000);

            if (currentwin != null)
            {
                CreateExceptionReport(currentwin.CurrentName);
            }
            BringToForeground("Import Data", ref currentwin, exePath);
            if (currentwin != null)
            {
                CreateExceptionReport(currentwin.CurrentName);
            }
            _currwinglobal = currentwin;

            //if (currentwin.CurrentAutomationId == "TradingTicket")
            //{
            //    winname = "Trading Ticket";
            //}
            //filename for import 
            //can be removed from here if open button working fine 
            while (currentwin == null || (currentwin.CurrentAutomationId != "ImportData"))
            {
                // CreateExceptionReport("infinite");
                BringToForeground("Nirvana", ref currentwin, exePath);
                CreateExceptionReport(currentwin.CurrentName);
                OpenImportModule();
                MaximizeWindow("Import Data", ref currentwin);
                if (currentwin != null)
                {
                    CreateExceptionReport(currentwin.CurrentName);
                }
                _currwinglobal = currentwin;
            }
            Thread.Sleep(2000);
            if ((currentwin.CurrentAutomationId == "ImportData"))
            {
                IUIAutomationCondition condtitlebar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "_ImportData_UltraFormManager_Dock_Area_Top");
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
                MaximizeWindow("Import Data", ref currentwin);
                Thread.Sleep(1000);
                //var boundingRectangle1 = titlebar.CurrentBoundingRectangle;

                //// Calculate the center of the element
                //int centerX1 = (int)(boundingRectangle1.right -40);
                //        int centerY1 = (int)(boundingRectangle1.bottom - 15);
                //MouseClick(centerX1, centerY1);

                CreateExceptionReport("Import Data Opened Successfully.................................");
                IUIAutomationElementArray allrows = findiuielementofrows1(currentwin);
                //  List<KeyValuePair<int, IUIAutomationElement>> selectedrows = findiuielementofrows(currentwin);
                KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("Import Data", "Import Data");
                foreach (DataRow row in dataFromCsvFile.Rows)
                {
                    string uploadThirdParty = row["UploadThirdParty"].ToString();
                    string importTypeFormatName = row["ImportTypeFormatName"].ToString();
                    string currentDirectory = Directory.GetCurrentDirectory();
                    //  string importFilePath = currentDirectory + row["FilePath"].ToString();
                    // string importFilePath = importFileFolderPath + $@"\{processDate.ToString("MMMM")}\{processDate.ToString("MMdd")}\{row["FilePath"].ToString()}";
                    string importFilePath = importFileFolderPath + ReplaceDateTimePlaceholders(row["FullPath"].ToString());

                    string account = row["Account"].ToString();
                    string date = row["Date"].ToString();
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

                    string exportData = row["Export"].ToString().ToLower();

                    if (uploadThirdParty != "" || importTypeFormatName != "" || importFilePath != "")
                    {
                        bool isImpFile = ImportFile(currentwin, allrows, dataFromCsvFile, uploadThirdParty, importTypeFormatName, importFilePath, account, date, wait, exportData, mainFileData, exePath);
                    }
                    else
                    {
                        string msg = "Any Of the The Columns  UploadThirdParty, ImportTypeFormatName, FilePath Can't be blank :    " + uploadThirdParty + "  " + importTypeFormatName + "  " + importFilePath;
                        CreateLogs(msg, logFilePath);
                        exitcode = 15;


                    }





                }





            }

        }
        catch (Exception ex)
        {
            string msg = ex.Message.ToString();
            CreateLogs(msg, logFilePath);
            CreateExceptionReport(ex.StackTrace);
            // exitcode = 23;
        }
        // CreateExceptionReport(GetActiveWindowTitle());
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
    public static DateTime processDate = GetProcessDate(Directory.GetCurrentDirectory() + @"\ImportConfigs\ProcessDateXmlPath.txt");
    public static string importFileFolderPath = ReadImportPathFromConfig(Directory.GetCurrentDirectory() + @"\ImportConfigs\ImportFileFolderPath.txt");

    public static string logFolderPath = "Logs";
    public static string exceptionReportFolderPath = "ExceptionReport";

    public static string logFilePath = Path.Combine(logFolderPath, $"Import_Log_{currentDate.ToString("yyyyMMdd")}.txt");
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
        if (command == "i")
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

            string importConfig = "ImportConfigs";
            if (!Directory.Exists(importConfig))
            {
                Directory.CreateDirectory(importConfig);
                exitcode = 15;
                //Environment.Exit(exitcode);
            }


            string mainfilePath = @"ImportConfigs\MainFile.csv";
            string importDataFilePath = @"ImportConfigs\ImportDataFile.csv";

            DataTable dataFromCsvFile = DataFromCsvFile(importDataFilePath);
            DataTable mainFileData = DataFromCsvFile(mainfilePath);
            int configRowCount = dataFromCsvFile.Rows.Count;    
            string exePath = GetValueFromMainFile(mainFileData, "exePath");
            //Process.Start(exePath);
            Thread.Sleep(4000);
            try
            {

                importreplayer(dataFromCsvFile, mainFileData, exePath);

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
                
                if(successCounter == configRowCount)
                {
                    Console.WriteLine("Import Done Successfully!!");
                    CreateLogs("Import Done Successfully!!",logFilePath);
                    exitcode = 0;
                }
                else if(successCounter > 0)
                {
                    Console.WriteLine("Partially Imported!!");
                    Console.WriteLine(successCounter +" out of "+configRowCount+" Imported Successfully!!");
                    CreateLogs("Partially Imported!!", logFilePath);
                    CreateLogs(successCounter + " out of " + configRowCount + " Imported Successfully!!",logFilePath);
                    Console.WriteLine("ExitCode :  " + exitcode + @"  Please Refer Exit Error Code File For More Information");
                    CreateLogs("ExitCode :  " + exitcode + @"  Please Refer Exit Error Code File For More Information", logFilePath);
                }
                else
                {
                    Console.WriteLine("None Of the Rows Imported!!");
                    CreateLogs("None Of the Rows Imported!!", logFilePath);
                    Console.WriteLine("ExitCode :  " + exitcode + @"  Please Refer Exit Error Code File For More Information");
                    CreateLogs("ExitCode :  " + exitcode + @"  Please Refer Exit Error Code File For More Information", logFilePath);
                }

                string logMsg = "*************************************************************SUCCESS!!*************************************************************";
                CreateLogs(logMsg, logFilePath, 0);

                // CreateExceptionReport("Press any key to close the console window...");

                // Wait for any key press
                

                // Close the console window
                //string errcodemsg1 = "ExitCode :  " + exitcode + @"  Please Refer Exit Error Code File For More Information";
                //CreateLogs(errcodemsg1, logFilePath, 0);
                //Environment.Exit(exitcode);


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

    //public static void ActionButton(string buttonname)
    //{
    //    try
    //    {
    //        if (_btntostepname.Keys.Contains(buttonname))
    //        {
    //            Stopwatch s2 = new Stopwatch();
    //            s2.Start();

    //            List<string> stepname = _btntostepname[buttonname];
    //            Stopwatch s8 = new Stopwatch();
    //            s8.Start();
    //            foreach (var outerKvp in dictmapalldata)
    //            {

    //                KeyValuePair<string, string> kvp = outerKvp.Key;
    //                foreach (string step1 in stepname)
    //                {
    //                    string step = step1;
    //                    if (kvp.Value == step)
    //                    {
    //                        if (!_stepnameloglist.Contains(step))
    //                        {
    //                            fillstepnamesheet(kvp.Key, step);
    //                            _stepnameloglist.Add(step);
    //                        }
    //                        if (_filecreated)
    //                        {
    //                            if (stepnametoindex.ContainsKey(step1))
    //                            {
    //                                int index = stepnametoindex[step1];
    //                                if (index > 0)
    //                                {
    //                                    step = step1 + $"_{index}";
    //                                }
    //                                stepnametoindex[step1] += 1;
    //                            }
    //                            string dpath = writedatatoexcelstep(_filename, step);
    //                            // if global static 
    //                            FillExcelSheetWithDictMapstep(dpath, step);

    //                        }
    //                        else
    //                        {
    //                            _filename = "demo" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
    //                            _filecreated = true;
    //                            //copy the headers from headers.xl
    //                            if (stepnametoindex.ContainsKey(step1))
    //                            {
    //                                int index = stepnametoindex[step1];
    //                                if (index > 0)
    //                                {
    //                                    step = step1 + $"_{index}";
    //                                }
    //                                stepnametoindex[step1] += 1;
    //                            }
    //                            string dpath = writedatatoexcelstep(_filename, step);
    //                            //step will be import1 and change it to import while finding headers 

    //                            // fill sheet with appropriate data by matching column names

    //                            FillExcelSheetWithDictMapstep(dpath, step);
    //                        }
    //                        //  CreateExceptionReport("found data for " + buttonname);
    //                        // CreateExceptionReport($"ModuleName: {kvp.Key}\tstepname: {kvp.Value}");
    //                    }
    //                }
    //            }
    //            s8.Stop();
    //            long elapsedMilliseconds7 = s8.ElapsedMilliseconds;
    //            // CreateExceptionReport(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    //            // CreateExceptionReport("Time taken to write data in EXCEL file for Other type of data    : " + "(" + elapsedMilliseconds7 + " ms)");
    //            Stopwatch s9 = new Stopwatch();
    //            s9.Start();
    //            foreach (var outerKvp in dictmap)
    //            {
    //                KeyValuePair<string, string> kvp = outerKvp.Key;
    //                foreach (string step1 in stepname)
    //                {
    //                    string step = step1;
    //                    if (kvp.Value == step)
    //                    {
    //                        if (!_stepnameloglist.Contains(step))
    //                        {
    //                            fillstepnamesheet(kvp.Key, step);
    //                            _stepnameloglist.Add(step);
    //                        }
    //                        //fillstepnamesheet(Foregroundwin, step);
    //                        //foreach (string ele in _stepnametoelement[step])
    //                        //{
    //                        //    //if current win contains the elements mentioned in stepname
    //                        //    IUIAutomationCondition getelement = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ele);
    //                        //    IUIAutomationElement targetelement = currentwin.FindFirst(TreeScope.TreeScope_Descendants, getelement);
    //                        //    if (targetelement != null)
    //                        //    {
    //                        if (_filecreated)
    //                        {
    //                            if (stepnametoindex.ContainsKey(step1))
    //                            {
    //                                int index = stepnametoindex[step1];
    //                                if (index > 0)
    //                                {
    //                                    step = step1 + $"_{index}";
    //                                }
    //                                stepnametoindex[step1] += 1;
    //                            }
    //                            string dpath = writedatatoexcelstep(_filename, step);
    //                            FillExcelSheetWithDictMapstep(dpath, step);
    //                        }
    //                        else
    //                        {
    //                            _filename = "demo" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
    //                            _filecreated = true;
    //                            if (stepnametoindex.ContainsKey(step1))
    //                            {
    //                                int index = stepnametoindex[step1];
    //                                if (index > 0)
    //                                {
    //                                    step = step1 + $"_{index}";
    //                                }
    //                                stepnametoindex[step1] += 1;
    //                            }
    //                            string dpath = writedatatoexcelstep(_filename, step);
    //                            FillExcelSheetWithDictMapstep(dpath, step);
    //                        }
    //                        //CreateExceptionReport("found data for " + buttonname);
    //                        //  CreateExceptionReport($"ModuleName: {kvp.Key}\tstepname: {kvp.Value}");
    //                        //    }
    //                        //}

    //                    }
    //                }
    //            }


    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        CreateExceptionReport(ex.Message);
    //        CreateExceptionReport(ex.StackTrace);
    //    }

    //}

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
                    ClickElement(btnelement, "Left Mouse Button Clicked");
                    Thread.Sleep(500);


                    IUIAutomationCondition selectOptioncond1 = automation1.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, value);
                    IUIAutomationCondition selectOptioncond2 = automation1.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);

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
            //if (currentwin.CurrentName == null) { }

            //else if (f == false && currwin.Contains("Nirvana"))
            //{
            //    //Process[] processes = Process.GetProcessesByName("Prana");
            //    //foreach (Process process in processes)
            //    //{
            //    //    // Get the process ID
            //    //    int processId = process.Id;

            //    //    // Get the path of the executable
            //    //    string path = process.MainModule.FileName;
            //    //    if (path.Equals(exePath))
            //    //    {
            //    //        var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");
            //    //        var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, currwin);
            //    //        var wincond = automation.CreateAndCondition(cond1, cond2);
            //    //        element = root.FindFirst(TreeScope.TreeScope_Descendants, wincond);
            //    //    }
            //    //    else
            //    // MinimizeWindow(currwin, ref currentwin);
            //    //}
            //    //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='Prana'");
            //    //foreach (ManagementObject process in searcher.Get())
            //    //{
            //    //    string processPath = process["ExecutablePath"].ToString();
            //    //    if (processPath.Equals(exePath))
            //    //    {
            //    //        CreateExceptionReport("found path");
            //    //        var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");
            //    //        var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, currwin);
            //    //        var wincond = automation.CreateAndCondition(cond1, cond2);
            //    //        element = root.FindFirst(TreeScope.TreeScope_Descendants, wincond);
            //    //    }
            //    //    else
            //    //        MinimizeWindow(currwin, ref currentwin);
            //    //    // Check if processPath matches exePath and perform actions accordingly
            //    //}
            //    var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");
            //    var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, currwin);
            //    var wincond = automation.CreateAndCondition(cond1, cond2);
            //    element = root.FindFirst(TreeScope.TreeScope_Descendants, wincond);


            //}
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
            CreateExceptionReport(ex.StackTrace);
            // If there is an error, display the error message
            CreateExceptionReport("Error writing to log file: " + ex.Message);
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
            CreateExceptionReport(ex.StackTrace);
            // If there is an error, display the error message
            CreateExceptionReport("Error writing to log file: " + ex.Message);
        }
    }





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
    static void CaptureAndSaveScreenshot(string moduleName)
    {
        string pictureFilePath = Directory.GetCurrentDirectory() + @"\ImportConfigs\SSPath.txt";
        string pathToSave = File.ReadAllText(pictureFilePath);
        // Get the current directory where the program is running
        //string currentDirectory = Directory.GetCurrentDirectory();

        // Get current date for folder name
        string folderName = $@"ProcessScreenshot\Pictures_{DateTime.Now.ToString("yyyyMMdd")}";
        string folderPath = Path.Combine(pathToSave, folderName);



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
            string fileName = $"{moduleName}_Screenshot_{currentDateTime}.png";
            string filePath = Path.Combine(folderPath, fileName);

            bitmap.Save(filePath, ImageFormat.Png);

            string logmessage = $"Screenshot saved as {fileName} in folder {folderName}.";
            CreateExceptionReport(logmessage);

            Task.Run(async () =>
            {
                await LogAsync(logmessage);
            });
        }
    }
    public static void FillModuleToElementValue()
    {
        string[] liness = File.ReadAllLines("moduletoelementsid.txt");

        foreach (string line in liness)
        {
            // Split each line by ':' to separate the button and step names
            string[] parts = line.Split(':');

            if (parts.Length == 2)
            {
                string button = parts[0].Trim();
                string[] stepNames = parts[1].Split(',').Select(s => s.Trim()).ToArray();

                // Add the button and step names to the dictionary
                _moduletoelementvalue.Add(button, new Dictionary<string, string> { });
                foreach (string s in stepNames)
                {
                    _moduletoelementvalue[button].Add(s, null);
                }
            }
        }
    }
    private static void printimportgrid(IUIAutomationElement targetelement)
    {
        string logmessage = "";
        logmessage += Environment.NewLine + "[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "] " + "GridName:" + targetelement.CurrentName + ", " + "Modulename:Import Data";
        IUIAutomation automation = new CUIAutomation8();

        IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "[Editor] Edit Area");
        IUIAutomationCondition conditionditems = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
        IUIAutomationCondition conditioncols = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderItemControlTypeId);
        IUIAutomationCondition conditiontrueelem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "True");

        IUIAutomationCondition condition1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TreeControlTypeId);

        IUIAutomationElement grid = targetelement.FindFirst(
           TreeScope.TreeScope_Descendants, condition1);

        if (grid != null)
        {

            CreateExceptionReport("FOUND TREE CONTROL");
            IUIAutomationElementArray dataitems = grid.FindAll(TreeScope.TreeScope_Children, conditiontrueelem);
            //IUIAutomationElementArray rowelements = grid.FindAll(TreeScope.TreeScope_Descendants, condition);
            //IUIAutomationElementArray dataitems = grid.FindAll(TreeScope.TreeScope_Children, conditionditems);

            List<string> checkcols = new List<string>();
            CreateExceptionReport("number of TRUE elements in grid " + dataitems.Length);

            // CreateExceptionReport("number of elements in grid " + rowelements.Length);
            //logmessage += Environment.NewLine + "[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "] " + "number of elements in grid: " + rowelements.Length;
            logmessage += Environment.NewLine + "$" + Environment.NewLine;

            if (dataitems != null)

            {
                logmessage += Environment.NewLine;
                //IUIAutomationElementArray colitems = grid.FindAll(TreeScope.TreeScope_Children, conditioncols);
                string parentElement = "";
                string realvalue = "";
                for (int i = 0; i < dataitems.Length; i++)
                {
                    IUIAutomationCondition uIAutomationCondition = automation.CreateTrueCondition();

                    IUIAutomationElement e = dataitems.GetElement(i);
                    IUIAutomationElementArray complexelements = e.FindAll(TreeScope.TreeScope_Children, uIAutomationCondition);
                    string togglestate = "";
                    if (complexelements != null)

                    {
                        //CreateExceptionReport("INSIDE GRID DATA WRITING");

                        for (int j = 0; j < complexelements.Length; j++)
                        {

                            IUIAutomationElement cell = complexelements.GetElement(j);
                            realvalue = getnewvalue(cell);
                            // CreateExceptionReport("VALUE OF CELL   " + realvalue);

                            parentElement = cell.CurrentName;
                            if (parentElement == "")
                            {
                                parentElement = "Blank";
                            }
                            if (realvalue == "")
                            {
                                realvalue = "blank";
                            }
                            // CreateExceptionReport(cell.CurrentName);
                            if (checkcols.Contains(parentElement))
                            {
                                logmessage += realvalue + new string(' ', (100 - realvalue.Length));
                            }
                            else
                            {
                                logmessage += realvalue + new string(' ', (100 - realvalue.Length));
                                checkcols.Add(parentElement);
                            }

                            //TODO: have done this to include rows which are checked on import grid might have to change 
                            if (parentElement == "Select Record")
                            {
                                togglestate = realvalue;
                            }
                            if (parentElement == "Select Record" && togglestate == "ToggleState_On")
                            {
                                //CreateExceptionReport("togglestate on ");
                            }
                            if (togglestate == "ToggleState_On")
                            {
                                if (parentElement == "Account")
                                {
                                    //CreateExceptionReport(realvalue);
                                }

                            }
                        }
                    }

                    togglestate = "";
                    logmessage += Environment.NewLine + "^^" + Environment.NewLine;
                }
                if (checkcols.Count > 0)
                {
                    logmessage += "@" + Environment.NewLine;
                }
                foreach (string s in checkcols)
                {
                    //  logmessage += s.PadRight(25-s.Length) + "\t";
                    logmessage += s + new string(' ', (50 - s.Length));
                }
                logmessage += Environment.NewLine + "!!" + Environment.NewLine;
            }

        }
        Log(logmessage);
    }

    private static void filldataindictfromgrid(KeyValuePair<string, string> dictname, IUIAutomationElement targetelement)
    {
        string logmessage = "";


        if (dictmap.TryGetValue(dictname, out var targetDict))
        {
            // Dictionary already exists, so clear the specific dictionary entry
            dictmap.Remove(dictname);
        }

        // Create a new dictionary and add it to the dictmap
        targetDict = new Dictionary<string, List<string>>();
        dictmap[dictname] = targetDict;
        IUIAutomation automation = new CUIAutomation8();
        IUIAutomationCondition condition1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_TreeControlTypeId);

        IUIAutomationElement grid = targetelement.FindFirst(
           TreeScope.TreeScope_Descendants, condition1);



        //Condition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tree);
        // AutomationElement grid = targetelement.FindFirst(System.Windows.Automation.TreeScope.Descendants, condition1);
        //Condition condition = new PropertyCondition(AutomationElement.AutomationIdProperty, "[Editor] Edit Area");
        IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "[Editor] Edit Area");
        IUIAutomationCondition conditionditems = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);
        IUIAutomationCondition conditioncols = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderItemControlTypeId);

        if (grid != null)
        {

            CreateExceptionReport("FOUND TREE CONTROL");
            IUIAutomationElementArray rowelements = grid.FindAll(TreeScope.TreeScope_Descendants, condition);
            //AutomationElementCollection rowelements = grid.FindAll(System.Windows.Automation.TreeScope.Descendants, condition);
            IUIAutomationElementArray dataitems = grid.FindAll(TreeScope.TreeScope_Descendants, conditionditems);

            //IUIAutomationElementArray colitems = grid.FindAll(TreeScope.TreeScope_Descendants, conditioncols);

            //for (int i = 0; i < colitems.Length; i++)
            //    logmessage += colitems.GetElement(i).CurrentName;            //{

            //}
            //logmessage+=Environment.NewLine;
            List<string> checkcols = new List<string>();

            CreateExceptionReport("number of elements in grid " + rowelements.Length);
            logmessage += Environment.NewLine + "[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "] " + "number of elements in grid: " + rowelements.Length;
            logmessage += Environment.NewLine + "$" + Environment.NewLine;
            logmessage += Environment.NewLine + "[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "] " + "GridName:" + dictname.Key + ", " + "StepName:" + dictname.Value;

            // CreateExceptionReport("number of columns in grid " + columnhead.Count);

            // inside grid find data elements and then inside dataelements get all data
            // 
            if (dataitems != null)

            {
                logmessage += Environment.NewLine;
                IUIAutomationElementArray colitems = grid.FindAll(TreeScope.TreeScope_Descendants, conditioncols);

                //for (int i = 0; i < colitems.Length; i++)
                //{
                //    logmessage += colitems.GetElement(i).CurrentName + "\t";
                //}
                //logmessage += Environment.NewLine;
                //CreateExceptionReport("INSIDE GRID DATA WRITING");
                string parentElement = "";
                string realvalue = "";
                for (int i = 0; i < dataitems.Length; i++)
                {
                    IUIAutomationCondition uIAutomationCondition = automation.CreateTrueCondition();

                    IUIAutomationElement e = dataitems.GetElement(i);
                    IUIAutomationElementArray complexelements = e.FindAll(TreeScope.TreeScope_Children, uIAutomationCondition);
                    string togglestate = "";
                    if (complexelements != null)

                    {
                        //CreateExceptionReport("INSIDE GRID DATA WRITING");

                        for (int j = 0; j < complexelements.Length; j++)
                        {

                            IUIAutomationElement cell = complexelements.GetElement(j);
                            realvalue = getnewvalue(cell);
                            CreateExceptionReport("VALUE OF CELL   " + realvalue);
                            //  logmessage += Environment.NewLine + "[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + "VALUE OF CELL   " + realvalue;



                            //AutomationElement parentElement = TreeWalker.ControlViewWalker.GetParent(cell);

                            //parentElement = automation.ContentViewWalker.GetParentElement(cell);
                            // CreateExceptionReport(parentElement.CurrentName);
                            parentElement = cell.CurrentName;
                            if (parentElement == "")
                            {
                                parentElement = "Blank";
                            }
                            if (realvalue == "")
                            {
                                realvalue = "blank";
                            }
                            CreateExceptionReport(cell.CurrentName);
                            if (checkcols.Contains(parentElement))
                            {
                                // logmessage += realvalue.PadRight(25-realvalue.Length) + "\t";
                                logmessage += realvalue + new string(' ', (50 - realvalue.Length));
                                //logmessage += realvalue + "\t";
                            }
                            else
                            {
                                //logmessage+=realvalue.PadRight(25-realvalue.Length) + "\t";
                                logmessage += realvalue + new string(' ', (50 - realvalue.Length));
                                //logmessage += realvalue + "\t";
                                checkcols.Add(parentElement);
                            }
                            //logmessage += Environment.NewLine + "[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + cell.CurrentName;

                            //TODO: have done this to include rows which are checked on import grid might have to change 
                            if (parentElement == "Select Record")
                            {
                                togglestate = realvalue;
                            }
                            if (togglestate == "ToggleState_On")
                            {
                                if (dictmap[dictname].ContainsKey(parentElement))
                                {
                                    dictmap[dictname][parentElement].Add(realvalue);
                                }
                                else
                                {
                                    dictmap[dictname].Add(parentElement, new List<string> { realvalue });
                                }
                            }
                        }
                    }

                    //string eleme = "";
                    //IUIAutomationValuePattern valuePattern = e.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                    //if (valuePattern != null)
                    //{
                    //    eleme = valuePattern.CurrentValue;
                    //}
                    //ValuePattern valuePattern = e.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
                    //if (valuePattern != null)
                    //{
                    //    eleme = valuePattern.Current.Value;
                    //}

                    logmessage += Environment.NewLine + "^^" + Environment.NewLine;
                }
                if (checkcols.Count > 0)
                {
                    logmessage += "@" + Environment.NewLine;
                }
                foreach (string s in checkcols)
                {
                    //  logmessage += s.PadRight(25-s.Length) + "\t";
                    logmessage += s + new string(' ', (50 - s.Length));
                }
                logmessage += Environment.NewLine + "!!" + Environment.NewLine;
            }

            else
            {
                if (rowelements != null)
                {
                    logmessage += Environment.NewLine;
                    IUIAutomationElementArray colitems = grid.FindAll(TreeScope.TreeScope_Descendants, conditioncols);

                    //for (int i = 0; i < colitems.Length; i++)
                    //{
                    //    logmessage += colitems.GetElement(i).CurrentName + "\t";
                    //}
                    //logmessage += Environment.NewLine;

                    for (int i = 0; i < rowelements.Length; i++)
                    {

                        IUIAutomationElement e = rowelements.GetElement(i);

                        //string realvalue = getnewvalue(e);

                        //AutomationElement parentElement = TreeWalker.ControlViewWalker.GetParent(e);
                        IUIAutomationElement parentElement = automation.ContentViewWalker.GetParentElement(e);
                        // IUIAutomationElement parentElement = automation.ContentViewWalker.

                        string eleme = "";
                        IUIAutomationValuePattern valuePattern = e.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                        if (valuePattern != null)
                        {
                            eleme = valuePattern.CurrentValue;
                        }
                        //ValuePattern valuePattern = e.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
                        //if (valuePattern != null)
                        //{
                        //    eleme = valuePattern.Current.Value;
                        //}
                        // logmessage += Environment.NewLine + "[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + parentElement.CurrentName;
                        //logmessage += Environment.NewLine + "[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + eleme;
                        //if (eleme == "")
                        //{
                        //    eleme = "NA";
                        //}
                        string pelement = parentElement.CurrentName;
                        if (parentElement.CurrentName == "")
                        {
                            pelement = "Blank";
                        }
                        if (checkcols.Contains(parentElement.CurrentName))
                        {
                            // logmessage += eleme + new string(' ', (25 - eleme.Length)) + "\t";
                            //logmessage += eleme + " ";
                            logmessage += eleme + new string(' ', (40 - eleme.Length));
                        }
                        else
                        {
                            //logmessage+=eleme+new string(' ',(25-eleme.Length))+ "\t";
                            logmessage += eleme + new string(' ', (40 - eleme.Length));


                            checkcols.Add(pelement);

                        }

                        if (dictmap[dictname].ContainsKey(parentElement.CurrentName))
                        {
                            dictmap[dictname][parentElement.CurrentName].Add(eleme);
                        }
                        else
                        {
                            dictmap[dictname].Add(parentElement.CurrentName, new List<string> { eleme });
                        }
                        if ((i + 1) % colitems.Length == 0)
                        {
                            logmessage += Environment.NewLine + "^^" + Environment.NewLine;
                        }
                    }
                }
                if (checkcols.Count > 0)
                {
                    logmessage += "@" + Environment.NewLine;
                }
                foreach (string s in checkcols)
                {
                    logmessage += s + new string(' ', (40 - s.Length));
                }
                logmessage += Environment.NewLine + "!!" + Environment.NewLine;
            }

        }
        logmessage += Environment.NewLine;

        // Log(logmessage);
        Task.Run(async () =>
        {

            await LogAsync(logmessage);

        });
    }



    static void WriteDictmapToTextFile(string fileName)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(@"griddata.txt"))
            {
                foreach (var outerKvp in dictmap)
                {
                    KeyValuePair<string, string> kvp = outerKvp.Key;
                    Dictionary<string, List<string>> innerDict = outerKvp.Value;

                    writer.WriteLine($"gridname: {kvp.Key}, stepname: {kvp.Value}");

                    foreach (var innerKvp in innerDict)
                    {
                        string string3 = innerKvp.Key;
                        List<string> listOfStrings = innerKvp.Value;
                        writer.WriteLine($"{string3}:{string.Join(", ", listOfStrings)}");
                    }
                    // Add a blank line to separate different entries
                    writer.WriteLine();
                }
            }

            //CreateExceptionReport("Data has been written to the file successfully.");
        }
        catch (Exception ex)
        {
            CreateExceptionReport(ex.StackTrace);
            CreateExceptionReport($"An error occurred while writing to the file: {ex.Message}");
        }
    }

    static void WriteDictmapalldataToTextFile(string fileName)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(@"allvalues.txt"))
            {
                foreach (var outerKvp in dictmapalldata)
                {
                    KeyValuePair<string, string> kvp = outerKvp.Key;
                    Dictionary<string, string> innerDict = outerKvp.Value;

                    writer.WriteLine($"ModuleName: {kvp.Key}\tstepname: {kvp.Value}");

                    foreach (var innerKvp in innerDict)
                    {
                        string string3 = innerKvp.Key;
                        string string4 = innerKvp.Value;

                        writer.WriteLine($"{string3} = {string4}");
                    }

                    // Add a blank line to separate different entries
                    writer.WriteLine();
                }
            }

            //CreateExceptionReport("Data has been written to the file successfully.");
        }
        catch (Exception ex)
        {
            CreateExceptionReport(ex.StackTrace);
            CreateExceptionReport($"An error occurred while writing to the file: {ex.Message}");
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

    private static void filldataindict(KeyValuePair<string, string> dictname, IUIAutomationElement targetelement)
    {
        Stopwatch sw1 = new Stopwatch();
        sw1.Start();
        if (!dictmapalldata.TryGetValue(dictname, out var targetDict))
        {
            // Dictionary doesn't exist, so create a new one and add it to the dictmapalldata
            targetDict = new Dictionary<string, string>();
            dictmapalldata[dictname] = targetDict;
        }
        string valuetouse = "";
        string elementtoget = targetelement.CurrentAutomationId;
        IUIAutomation automation11 = new CUIAutomation8();
        IUIAutomationElement childelement = automation11.ContentViewWalker.GetFirstChildElement(targetelement);
        IUIAutomationElement lastchildelement = automation11.ContentViewWalker.GetLastChildElement(targetelement);


        if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_ComboBoxControlTypeId)
        {
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            valuetouse = "";
            IUIAutomation automation = new CUIAutomation8();
            IUIAutomationCondition lstitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);

            IUIAutomationElementArray lstelement = targetelement.FindAll(
               TreeScope.TreeScope_Children, lstitem);

            //changed to children

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
                                break;
                            }
                            //CreateExceptionReport($"ComboBox value: {comboBoxValue}");

                        }
                    }
                }
                if (valuetouse == "")
                {
                    object patternprovider;
                    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                    {
                        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                        IUIAutomationValuePattern selectionpatternprovider = patternprovider as IUIAutomationValuePattern;
                        valuetouse = selectionpatternprovider.CurrentValue.ToString();


                    }
                }
            }
            sw2.Stop();
            long etime = sw2.ElapsedMilliseconds;
            // CreateExceptionReport("time taken for one combobox " + etime);
        }
        //childelement.currname
        else if (targetelement.CurrentName == "PranaNumericUpDown" || (lastchildelement != null && lastchildelement.CurrentName == "PranaNumericUpDown"))
        {
            if (lastchildelement != null)
            {
                if (lastchildelement.CurrentName == "PranaNumericUpDown")
                {
                    targetelement = lastchildelement;
                }
            }
            //CreateExceptionReport("inside numericupdown");

            try
            {
                string value = null;
                object patternprovider;
                if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                {
                    patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                    IUIAutomationValuePattern selectionpatternprovider = patternprovider as IUIAutomationValuePattern;
                    value = selectionpatternprovider.CurrentValue.ToString();

                    IUIAutomation automation1 = new CUIAutomation8();
                    valuetouse = value;
                    IUIAutomationElement numericupdownparent = automation1.ContentViewWalker.GetParentElement(targetelement);
                    string label = numericupdownparent.CurrentAutomationId;
                    string result1 = label.Replace("nmrc", "");

                    elementtoget = result1.Trim();
                }
            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.StackTrace);
                CreateExceptionReport(ex.ToString());
            }
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
                    //  CreateExceptionReport(selectionpatternprovider.CurrentToggleState.ToString());
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
            //changed on 17/october 
            // valuetouse = targetelement.CurrentName;
            object selectPatternObj;
            selectPatternObj = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);

            IUIAutomationSelectionItemPattern selectPattern = selectPatternObj as IUIAutomationSelectionItemPattern;

            if (selectPattern != null)
            {
                // Extract the value from the ComboBox
                valuetouse = selectPattern.CurrentIsSelected.ToString();
                //CreateExceptionReport($"ComboBox value: {comboBoxValue}");

            }
            else
            {
                valuetouse = targetelement.CurrentName;
            }


        }
        else if (targetelement.CurrentAutomationId == "MultiSelectEditor" || (childelement != null && childelement.CurrentAutomationId == "MultiSelectEditor"))
        {
            if (childelement.CurrentAutomationId == "MultiSelectEditor")
            {
                targetelement = childelement;
            }
            // CreateExceptionReport("inside multi select ");

            IUIAutomation automation2 = new CUIAutomation8();
            IUIAutomationCondition bttn = automation2.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
            IUIAutomationElement btnelement = targetelement.FindFirst(TreeScope.TreeScope_Children, bttn);
            //changed to children 

            if (_uiInteraction == false)
            {
                if (btnelement != null)
                {
                    //CreateExceptionReport("Found Button element ");
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
                            //   CreateExceptionReport(selectionpatternprovider.CurrentExpandCollapseState.ToString());
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
                                //    CreateExceptionReport("SIZE OF List" + checkboxes.Length);
                                bool flag = false;
                                if (checkboxes != null)
                                {
                                    // Console.Write("inside checkboxes ");
                                    for (int i = 0; i < checkboxes.Length; i++)
                                    {
                                        IUIAutomationElement checkbox = checkboxes.GetElement(i);
                                        //   CreateExceptionReport(checkbox.CurrentName + "..");
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
                                                //    CreateExceptionReport(chkboxpatternprovider.ToString());
                                                //    CreateExceptionReport(chkboxpatternprovider.CurrentToggleState.ToString());
                                                if (chkboxpatternprovider.CurrentToggleState.ToString() == "ToggleState_On")
                                                {
                                                    if (!flag)
                                                    {
                                                        if (checkbox.CurrentName == "Select All")
                                                        {
                                                            continue;
                                                        }
                                                        valuetouse += (checkbox.CurrentName);
                                                        flag = true;

                                                    }
                                                    else
                                                    {

                                                        valuetouse += ("," + checkbox.CurrentName);
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
                                    IUIAutomationElement multiselectparent = automation1.ContentViewWalker.GetParentElement(targetelement);
                                    IUIAutomationElement labelelement = automation1.ContentViewWalker.GetPreviousSiblingElement(multiselectparent);
                                    string label = labelelement.CurrentName;
                                    elementtoget = label.Trim();
                                }
                            }
                            selectionpatternprovider.Collapse();

                        }
                    }
                    catch (Exception ex)
                    {
                        CreateExceptionReport(ex.StackTrace);
                        CreateExceptionReport(ex.Message);
                    }

                }
            }
            else
            {
                try
                {
                    string value = null;
                    object patternprovider;
                    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                    {
                        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                        IUIAutomationValuePattern selectionpatternprovider = patternprovider as IUIAutomationValuePattern;
                        value = selectionpatternprovider.CurrentValue.ToString();
                        //CreateExceptionReport("......////...........");

                        IUIAutomation automation1 = new CUIAutomation8();
                        valuetouse = value;
                        IUIAutomationElement multiselectparent = automation1.ContentViewWalker.GetParentElement(targetelement);
                        IUIAutomationElement labelelement = automation1.ContentViewWalker.GetPreviousSiblingElement(multiselectparent);
                        string label = labelelement.CurrentName;
                        elementtoget = label.Trim();
                    }
                }
                catch (Exception ex)
                {
                    CreateExceptionReport(ex.StackTrace);
                    CreateExceptionReport(ex.Message);

                }
            }
        }
        //else if (targetelement.CurrentAutomationId == "MultiSelectEditor")
        //{
        //    CreateExceptionReport("inside multi select ");

        //    IUIAutomation automation2 = new CUIAutomation8();
        //    IUIAutomationCondition bttn = automation2.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
        //    IUIAutomationElement btnelement = targetelement.FindFirst(TreeScope.TreeScope_Descendants, bttn);


        //    if (btnelement != null)
        //    {
        //        CreateExceptionReport("Found Button element ");
        //        //expand pattern invoke
        //        try
        //        {
        //            string value = null;
        //            object patternprovider;
        //            if (btnelement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId)!=null)
        //            {
        //                patternprovider = btnelement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
        //                CreateExceptionReport("inside expand pattern");
        //                IUIAutomationExpandCollapsePattern selectionpatternprovider = patternprovider as IUIAutomationExpandCollapsePattern;
        //                value = selectionpatternprovider.CurrentExpandCollapseState.ToString();
        //                //CreateExceptionReport("......////...........");
        //                CreateExceptionReport(selectionpatternprovider.CurrentExpandCollapseState.ToString());
        //                selectionpatternprovider.Expand();
        //                CreateExceptionReport(selectionpatternprovider.CurrentExpandCollapseState.ToString());
        //                IUIAutomation automation1 = new CUIAutomation8();
        //                IUIAutomationCondition listt = automation1.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "checkedMultipleItems");
        //                IUIAutomationElement listelement = _currwinglobal.FindFirst(TreeScope.TreeScope_Descendants, listt);
        //                if (listelement != null)
        //                {
        //                    // Condition checkboxitem = new PropertyCondition(AutomationElement.ControlTypeProperty,listelement);
        //                    // AutomationElementCollection checkboxes = listelement.FindAll(TreeScope.Children, checkboxitem);
        //                    IUIAutomation automation = new CUIAutomation8();

        //                    IUIAutomationCondition conditiont = automation.CreateTrueCondition();

        //                    IUIAutomationElementArray checkboxes = listelement.FindAll(TreeScope.TreeScope_Descendants,conditiont);
        //                    //CreateExceptionReport(checkboxes[0].Current.Name);
        //                    CreateExceptionReport("SIZE OF List" + checkboxes.Length);

        //                    if (checkboxes != null)
        //                    {
        //                        Console.Write("inside checkboxes ");
        //                        for (int i=0;i<checkboxes.Length;i++)
        //                        {
        //                            IUIAutomationElement checkbox = checkboxes.GetElement(i);
        //                            CreateExceptionReport(checkbox.CurrentName + "..");
        //                            try
        //                            {
        //                               // IUIAutomationValuePattern valuePattern = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
        //                               // IUIAutomationTogglePattern togglePattern = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) as IUIAutomationTogglePattern;
        //                                //string value = null;
        //                                object patternproviderrr;
        //                                if (checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId)!=null)
        //                                {
        //                                    patternproviderrr = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
        //                                    IUIAutomationTogglePattern chkboxpatternprovider = patternproviderrr as IUIAutomationTogglePattern;
        //                                    // value = selectionpatternprovider.Current.ToString();
        //                                    // CreateExceptionReport("......////...........");
        //                                    CreateExceptionReport(chkboxpatternprovider.ToString());
        //                                    CreateExceptionReport(chkboxpatternprovider.CurrentToggleState.ToString());
        //                                    if (chkboxpatternprovider.CurrentToggleState.ToString() == "ToggleState_On")
        //                                    {
        //                                        valuetouse += ("," + checkbox.CurrentName);
        //                                    }
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {

        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //    }
        //}
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
            // CreateExceptionReport("checking");
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
        string logmessage = "";
        logmessage += Environment.NewLine + "[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "] " + dictname.Key + dictname.Value + elementtoget + valuetouse;
        try
        {
            // Add elementtoget and elementValue to the dictionary

            dictmapalldata[dictname].Add(elementtoget, valuetouse);
        }
        catch
        {
            dictmapalldata[dictname][elementtoget] = valuetouse;
        }
        if (dictname.Value == "GetDataAllocation")
        {
            try
            {
                // Add elementtoget and elementValue to the dictionary

                dictmapalldata[dictname].Add("Account", "Tech2 GS PB 002719045");
                dictmapalldata[dictname].Add("FilterTabName", "All");
            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.StackTrace);
                dictmapalldata[dictname]["Account"] = "Tech2 GS PB 002719045";
                dictmapalldata[dictname]["FilterTabName"] = "All";
            }
        }
        sw1.Stop();
        long elaptime = sw1.ElapsedMilliseconds;
        // CreateExceptionReport(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //CreateExceptionReport("THIS IS TIME TAKEN BY FILL DATA DICT " + elaptime  );

    }
    private static void logdata(string modulename, IUIAutomationElement targetelement)
    {

        string logmessage = "";
        string valuetouse = "";
        string elementtoget = targetelement.CurrentAutomationId;
        IUIAutomation automation11 = new CUIAutomation8();
        IUIAutomationElement childelement = null;
        IUIAutomationElement lastchildelement = null;
        try
        {
            childelement = automation11.ContentViewWalker.GetFirstChildElement(targetelement);
            lastchildelement = automation11.ContentViewWalker.GetLastChildElement(targetelement);
        }
        catch (Exception ex)
        {
            CreateExceptionReport(ex.StackTrace);
            CreateExceptionReport(ex.Message);

        }

        if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_ComboBoxControlTypeId)
        {
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            valuetouse = "";
            IUIAutomation automation = new CUIAutomation8();
            IUIAutomationCondition lstitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);

            IUIAutomationElementArray lstelement = targetelement.FindAll(
               TreeScope.TreeScope_Children, lstitem);

            //changed to children

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
                                break;
                            }
                            //CreateExceptionReport($"ComboBox value: {comboBoxValue}");

                        }
                    }
                }
                if (valuetouse == "")
                {
                    object patternprovider;
                    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                    {
                        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                        IUIAutomationValuePattern selectionpatternprovider = patternprovider as IUIAutomationValuePattern;
                        valuetouse = selectionpatternprovider.CurrentValue.ToString();


                    }
                }
            }
            sw2.Stop();
            long etime = sw2.ElapsedMilliseconds;
            // CreateExceptionReport("time taken for one combobox " + etime);
        }
        //childelement.currname
        else if (targetelement.CurrentName == "PranaNumericUpDown" || (lastchildelement != null && lastchildelement.CurrentName == "PranaNumericUpDown"))
        {
            if (lastchildelement != null)
            {
                if (lastchildelement.CurrentName == "PranaNumericUpDown")
                {
                    targetelement = lastchildelement;
                }
            }
            //CreateExceptionReport("inside numericupdown");

            try
            {
                string value = null;
                object patternprovider;
                if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                {
                    patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                    IUIAutomationValuePattern selectionpatternprovider = patternprovider as IUIAutomationValuePattern;
                    value = selectionpatternprovider.CurrentValue.ToString();

                    IUIAutomation automation1 = new CUIAutomation8();
                    valuetouse = value;
                    IUIAutomationElement numericupdownparent = automation1.ContentViewWalker.GetParentElement(targetelement);
                    string label = numericupdownparent.CurrentAutomationId;
                    string result1 = label.Replace("nmrc", "");

                    elementtoget = result1.Trim();
                }
            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.StackTrace);
                CreateExceptionReport(ex.Message);

            }
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
                    //  CreateExceptionReport(selectionpatternprovider.CurrentToggleState.ToString());
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
            //changed on 17/october 
            // valuetouse = targetelement.CurrentName;
            object selectPatternObj;
            selectPatternObj = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);

            IUIAutomationSelectionItemPattern selectPattern = selectPatternObj as IUIAutomationSelectionItemPattern;

            if (selectPattern != null)
            {
                // Extract the value from the ComboBox
                valuetouse = selectPattern.CurrentIsSelected.ToString();
                //CreateExceptionReport($"ComboBox value: {comboBoxValue}");

            }
            else
            {
                valuetouse = targetelement.CurrentName;
            }


        }
        else if (targetelement.CurrentAutomationId == "MultiSelectEditor" || (childelement != null && childelement.CurrentAutomationId == "MultiSelectEditor"))
        {
            if (childelement.CurrentAutomationId == "MultiSelectEditor")
            {
                targetelement = childelement;
            }
            // CreateExceptionReport("inside multi select ");

            IUIAutomation automation2 = new CUIAutomation8();
            IUIAutomationCondition bttn = automation2.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
            IUIAutomationElement btnelement = targetelement.FindFirst(TreeScope.TreeScope_Children, bttn);
            //changed to children 

            if (_uiInteraction == false)
            {
                if (btnelement != null)
                {
                    //CreateExceptionReport("Found Button element ");
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
                            //   CreateExceptionReport(selectionpatternprovider.CurrentExpandCollapseState.ToString());
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
                                //    CreateExceptionReport("SIZE OF List" + checkboxes.Length);
                                bool flag = false;
                                if (checkboxes != null)
                                {
                                    // Console.Write("inside checkboxes ");
                                    for (int i = 0; i < checkboxes.Length; i++)
                                    {
                                        IUIAutomationElement checkbox = checkboxes.GetElement(i);
                                        //   CreateExceptionReport(checkbox.CurrentName + "..");
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
                                                //    CreateExceptionReport(chkboxpatternprovider.ToString());
                                                //    CreateExceptionReport(chkboxpatternprovider.CurrentToggleState.ToString());
                                                if (chkboxpatternprovider.CurrentToggleState.ToString() == "ToggleState_On")
                                                {
                                                    if (!flag)
                                                    {
                                                        if (checkbox.CurrentName == "Select All")
                                                        {
                                                            continue;
                                                        }
                                                        valuetouse += (checkbox.CurrentName);
                                                        flag = true;

                                                    }
                                                    else
                                                    {

                                                        valuetouse += ("," + checkbox.CurrentName);
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            CreateExceptionReport(ex.Message);

                                        }
                                    }
                                    IUIAutomationElement multiselectparent = automation1.ContentViewWalker.GetParentElement(targetelement);
                                    IUIAutomationElement labelelement = automation1.ContentViewWalker.GetPreviousSiblingElement(multiselectparent);
                                    string label = labelelement.CurrentName;
                                    elementtoget = label.Trim();
                                }
                            }
                            selectionpatternprovider.Collapse();

                        }
                    }
                    catch (Exception ex)
                    {
                        CreateExceptionReport(ex.StackTrace);
                        CreateExceptionReport(ex.Message);

                    }

                }
            }
            else
            {
                try
                {
                    string value = null;
                    object patternprovider;
                    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) != null)
                    {
                        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                        IUIAutomationValuePattern selectionpatternprovider = patternprovider as IUIAutomationValuePattern;
                        value = selectionpatternprovider.CurrentValue.ToString();
                        //CreateExceptionReport("......////...........");

                        IUIAutomation automation1 = new CUIAutomation8();
                        valuetouse = value;
                        IUIAutomationElement multiselectparent = automation1.ContentViewWalker.GetParentElement(targetelement);
                        IUIAutomationElement labelelement = automation1.ContentViewWalker.GetPreviousSiblingElement(multiselectparent);
                        string label = labelelement.CurrentName;
                        elementtoget = label.Trim();
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
            // CreateExceptionReport("checking");
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
        //string logmessage = "";
        // elementid=element log
        logmessage += "[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "] " + elementtoget + " = " + valuetouse;
        //try
        //{
        //    // Add elementtoget and elementValue to the dictionary

        //    dictmapalldata[dictname].Add(elementtoget, valuetouse);
        //}
        //catch
        //{
        //    dictmapalldata[dictname][elementtoget] = valuetouse;
        //}
        //if (dictname.Value == "GetDataAllocation")
        //{
        //    try
        //    {
        //        // Add elementtoget and elementValue to the dictionary

        //        dictmapalldata[dictname].Add("Account", "Tech2 GS PB 002719045");
        //        dictmapalldata[dictname].Add("FilterTabName", "All");
        //    }
        //    catch
        //    {

        //        dictmapalldata[dictname]["Account"] = "Tech2 GS PB 002719045";
        //        dictmapalldata[dictname]["FilterTabName"] = "All";
        //    }
        //}

        // CreateExceptionReport(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //CreateExceptionReport("THIS IS TIME TAKEN BY FILL DATA DICT " + elaptime  );

        Task.Run(async () =>
        {
            await LogAsync(logmessage);
        });

        //Log(logmessage);
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
                    CreateExceptionReport("This is key " + innerEntry.Key);
                    // outerEntry.Value[innerEntry.Key] = newValue;
                    return true;
                }
            }
        }

        return false;
    }
    static bool UpdateValueByInnerKey(string targetValue, string newValue)
    {
        foreach (var outerEntry in dictmapalldata)
        {
            foreach (var innerEntry in outerEntry.Value)
            {
                if (innerEntry.Key == targetValue)
                {
                    CreateExceptionReport("this is value " + outerEntry.Value[innerEntry.Key]);
                    outerEntry.Value[innerEntry.Key] = newValue;
                    return true;
                }
            }
        }

        return false;
    }
    //new imports end 
    private static void getprananumericupdown()
    {
        //IUIAutomationElement element1
        IUIAutomation automation = new CUIAutomation8();
        try
        {
            IUIAutomationCondition getdiffdata = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "PranaNumericUpDown");
            IUIAutomationElementArray diffdata = _currwinglobal.FindAll(TreeScope.TreeScope_Descendants, getdiffdata);
            if (diffdata != null)
            {
                for (int i = 0; i < diffdata.Length; i++)
                {
                    IUIAutomationElement element1 = diffdata.GetElement(i);
                    IUIAutomationElement parentElement = automation.ContentViewWalker.GetParentElement(element1);
                    string valuetouse = "";
                    IUIAutomationValuePattern valuePattern = element1.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                    if (valuePattern != null)
                    {
                        valuetouse = valuePattern.CurrentValue;
                        CreateExceptionReport(parentElement.CurrentAutomationId);
                        CreateExceptionReport(valuetouse);
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
    static string[] elementNamesToFind = new string[]
        {
            "cmbMethodology",
            "cmbAlgorithm",
            "Element3Name",
            "cmbSecondarySort"
        };
    static async Task FindAndAddElementAsync(IUIAutomationElement automation1, string name, ConcurrentBag<IUIAutomationElement> foundElements)
    {
        // await Task.Yield();
        CreateExceptionReport(automation1.CurrentName);
        CreateExceptionReport(name);
        IUIAutomation automation = new CUIAutomation8();

        IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, name);
        IUIAutomationElement foundElement = automation1.FindFirst(TreeScope.TreeScope_Descendants, condition);

        if (foundElement != null)
        {
            IUIAutomationCondition lstitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
            //IUIAutomationElementArray lstelement = targetelement.FindAll(
            //   TreeScope.TreeScope_Children, lstitem);
            IUIAutomationElementArray lstelement = foundElement.FindAll(
               TreeScope.TreeScope_Children, lstitem);

            string valuetouse = "";
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
                    for (int j = 0; j < lstelement.Length; j++)
                    {
                        IUIAutomationElement listitem = lstelement.GetElement(j);
                        string listitemText = listitem.GetCurrentPropertyValue(
                           UIA_PropertyIds.UIA_NamePropertyId).ToString();

                        object selectPatternObj;
                        selectPatternObj = listitem.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);

                        IUIAutomationSelectionItemPattern selectPattern = selectPatternObj as IUIAutomationSelectionItemPattern;

                        if (selectPattern != null)
                        {

                            int _isselected = selectPattern.CurrentIsSelected;
                            if (_isselected == 1)
                            {
                                valuetouse = listitem.CurrentName;
                                CreateExceptionReport(valuetouse);
                                break;

                            }


                        }
                    }
                }
            }
            CreateExceptionReport("found");
            foundElements.Add(foundElement);
        }
    }
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
            CreateExceptionReport(ex.StackTrace);
            CreateExceptionReport(ex.Message);

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
    //        CreateExceptionReport("inside hook");
    //       // WindowActivated?.Invoke(this, EventArgs.Empty);
    //    }
    //    if (nCode == 5)
    //    {
    //        CreateExceptionReport("inside hook 2");

    //    }
    //    if (nCode == 1)
    //    {
    //        CreateExceptionReport("inside hook 3");

    //    }
    //    if (nCode == 4)
    //    {
    //        CreateExceptionReport("inside hook 4");

    //    }

    //    return CallNextHookEx(_cbtHookID, nCode, wParam, lParam);
    //}




    private static IUIAutomationElement GetElementByXPath(string Foregroundwin, IUIAutomationElement currentwin, string XPath)
    {
        IUIAutomationElement bfsroot = currentwin;
        IUIAutomation automat = new CUIAutomation8();

        foreach (string step in XPath.Split('/'))
        {
            if (step.Length >= 2)
            {
                char action = step[0];
                int count = int.Parse(step.Substring(1));
                IUIAutomationCondition truecond = automat.CreateTrueCondition();

                for (int i = 0; i < count; i++)
                {
                    if (action == 'c')
                    {
                        if (bfsroot != null)
                            bfsroot = bfsroot.FindFirst(TreeScope.TreeScope_Children, truecond);
                    }
                    else if (action == 's')
                    {
                        IUIAutomationTreeWalker treeWalker = automat.CreateTreeWalker(truecond);
                        if (bfsroot != null)
                            bfsroot = treeWalker.GetNextSiblingElement(bfsroot);

                    }
                    else
                    {
                        // Handle invalid action character
                        throw new ArgumentException("Invalid action character in XPath.");
                    }
                }
            }
            else
            {
                // Handle invalid XPath step format
                throw new ArgumentException("Invalid XPath format.");
            }
        }

        return bfsroot;
    }

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

    private static void AddPropertyChangedEventHandler(string telement, IUIAutomationElement element)
    {
        CreateExceptionReport("inside element change handler ");
        var eventHandler = new AutomationPropertyChangedEventHandler((sender, pid, value) =>
        {
            IUIAutomationValuePattern valuePattern = sender.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
            string change_autoid = "";
            string logmessage = "";
            change_autoid = sender.CurrentAutomationId.ToString();

            logmessage += "[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "] " + "value changed: " + change_autoid + " " + valuePattern.CurrentValue + Environment.NewLine;
            //  CreateExceptionReport("value changed: " + change_autoid + " "+ valuePattern.CurrentValue);
            CreateExceptionReport(logmessage);
            Task.Run(async () =>
            {
                await LogAsync(logmessage);
            });
            //  Log(logmessage);
        });

        _automation.AddPropertyChangedEventHandler(element, TreeScope.TreeScope_Descendants, null, eventHandler, new[] { UIA_PropertyIds.UIA_ValueValuePropertyId });

        // Store the event handler in the dictionary
        _eventhandledict[telement] = eventHandler;
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
                    if (_correctioncolumn.Keys.Contains(elementtoget))
                    {
                        elementValue = _correctioncolumn[elementtoget][elementValue];
                    }
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
    
    // bool filecreated=false;
    //string filename="";
    // if filecreated==false
    //create new file 
    //send filename and stepname
    //stepwise excel sheet creation with headers 
    static private void fillstepnamesheet(string modulename, string stepnamee)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        string filePath = @"test\demo.xlsx";
        FileInfo fileInfo = new FileInfo(filePath);
        if (_stepnamecorrection.ContainsKey(modulename))
        {
            modulename = _stepnamecorrection[modulename];
        }

        ExcelPackage package = new ExcelPackage(fileInfo);
        ExcelWorksheet worksheet = package.Workbook.Worksheets["Automatic"];
        if (!_filecreated)
        {
            int lastRow = worksheet.Dimension.End.Row;

            for (int row = rownum; row <= lastRow; row++)
            {
                for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                {
                    var cell = worksheet.Cells[row, col];
                    cell.Clear(); // Clear cell content
                }
            }
            package.Save();
        }
        worksheet.Cells[rownum, 4].Value = modulename;
        worksheet.Cells[rownum, 5].Value = stepnamee;
        worksheet.Cells[rownum, 6].Value = gstring;
        worksheet.Cells[rownum, 7].Value = stepnamee;

        package.Save();
        package.Dispose();
        rownum++;

    }

    // header fill { step, file  } { headers step{index+1}}

    static private string writedatatoexcelstep(string filename, string stepname)
    {

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        string sourceFolderPath = @"test\";
        string sourceFilePath = Path.Combine(sourceFolderPath, "headers.xlsx");
        FileInfo sourceFile = new FileInfo(sourceFilePath);
        ExcelPackage sourcePackage = new ExcelPackage(sourceFile);

        // create a new file dynamically 
        //new file 

        // string filename = "demo" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        string stepname1 = stepname;
        //string destinationFilePath = Path.Combine(sourceFolderPath, filename);
        string destinationFolderPath = @"test\";
        string destinationFilePath = Path.Combine(destinationFolderPath, filename);
        FileInfo destinationFile = new FileInfo(destinationFilePath);
        ExcelPackage destinationPackage = new ExcelPackage(destinationFile);

        //stepnametoindex handling
        int indexOfUnderscore = stepname.IndexOf("_");
        string extractedSubstring = "";
        if (indexOfUnderscore != -1)
        {
            // Extract the part of the string after the underscore
            extractedSubstring = stepname.Substring(indexOfUnderscore + 1);
            stepname = stepname.Substring(0, indexOfUnderscore);
            // Perform your checks on the extracted substring



        }


        foreach (var outerKvp in dictmapalldata)
        {
            var outerKey = outerKvp.Key;
            if (stepname == outerKey.Value)
            {
                string sheetName = outerKey.Value;
                ExcelWorksheet sourceSheet = sourcePackage.Workbook.Worksheets[sheetName];
                try
                {
                    sheetName = sheetName + extractedSubstring;
                    ExcelWorksheet destinationSheet = destinationPackage.Workbook.Worksheets.Add(sheetName, sourceSheet);
                }
                catch (Exception ex)
                {
                    CreateExceptionReport(ex.StackTrace);
                    CreateExceptionReport(ex.Message);

                }
                destinationPackage.Save();
            }
        }
        foreach (var outerKvp in dictmap)
        {
            var outerKey = outerKvp.Key;
            if (stepname == outerKey.Value)
            {
                string sheetName = outerKey.Value;
                ExcelWorksheet sourceSheet = sourcePackage.Workbook.Worksheets[sheetName];
                try
                {
                    sheetName = sheetName + extractedSubstring;
                    ExcelWorksheet destinationSheet = destinationPackage.Workbook.Worksheets.Add(sheetName, sourceSheet);
                }
                catch (Exception ex)
                {
                    CreateExceptionReport(ex.StackTrace);
                    CreateExceptionReport(ex.Message);

                }
                destinationPackage.Save();
            }
        }
        try
        {
            destinationPackage.Workbook.Worksheets.Delete("Sheet1");
        }
        catch (Exception ex)
        {
            CreateExceptionReport(ex.StackTrace);
            CreateExceptionReport(ex.Message);

        }
        destinationPackage.Save();
        sourcePackage.Dispose();
        destinationPackage.Dispose();

        return destinationFilePath;
    }



    static private string writedatatoexcel()
    {
        // i have all the dictionaries ready now 
        // find for which step i have the data and create sheet for that 
        // go to headers folder copy header 
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        string sourceFolderPath = @"test\";
        string sourceFilePath = Path.Combine(sourceFolderPath, "headers.xlsx");


        FileInfo sourceFile = new FileInfo(sourceFilePath);
        ExcelPackage sourcePackage = new ExcelPackage(sourceFile);

        // create a new file dynamically 
        //new file 

        string filename = "demo" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

        string destinationFilePath = Path.Combine(sourceFolderPath, filename);
        FileInfo destinationFile = new FileInfo(destinationFilePath);
        ExcelPackage destinationPackage = new ExcelPackage(destinationFile);
        foreach (var outerKvp in dictmapalldata)
        {
            var outerKey = outerKvp.Key;
            string sheetName = outerKey.Value;
            ExcelWorksheet sourceSheet = sourcePackage.Workbook.Worksheets[sheetName];
            try
            {
                ExcelWorksheet destinationSheet = destinationPackage.Workbook.Worksheets.Add(sheetName, sourceSheet);
            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.StackTrace);
                CreateExceptionReport(ex.Message);


            }
            destinationPackage.Save();
        }
        foreach (var outerKvp in dictmap)
        {
            var outerKey = outerKvp.Key;
            string sheetName = outerKey.Value;
            ExcelWorksheet sourceSheet = sourcePackage.Workbook.Worksheets[sheetName];
            try
            {
                ExcelWorksheet destinationSheet = destinationPackage.Workbook.Worksheets.Add(sheetName, sourceSheet);
            }
            catch (Exception ex)
            {
                CreateExceptionReport(ex.StackTrace);
                CreateExceptionReport(ex.Message);

            }
            destinationPackage.Save();
        }
        try
        {
            destinationPackage.Workbook.Worksheets.Delete("Sheet1");
        }
        catch (Exception ex)
        {
            CreateExceptionReport(ex.StackTrace);
            CreateExceptionReport(ex.Message);

        }
        destinationPackage.Save();
        sourcePackage.Dispose();
        destinationPackage.Dispose();

        return destinationFilePath;
    }
    // import change
    private static void FillExcelSheetWithDictMapstep(string filePath, string stepname)
    {
        // Load the existing Excel file
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var fileInfo = new FileInfo(filePath);
        int indexOfUnderscore = stepname.IndexOf("_");
        string extractedSubstring = "";
        string stepname1 = stepname;
        if (indexOfUnderscore != -1)
        {
            // Extract the part of the string after the underscore
            extractedSubstring = stepname.Substring(indexOfUnderscore + 1);
            stepname = stepname.Substring(0, indexOfUnderscore);
            // Perform your checks on the extracted substring



        }
        using (var package = new ExcelPackage(fileInfo))
        {
            foreach (var outerKvp in dictmap)
            {
                var outerKeyPair = outerKvp.Key;

                if (stepname == outerKeyPair.Value)
                {
                    var innerDict = outerKvp.Value;

                    // Get or create the subsheet based on string2 (outerKeyPair.Value)
                    //change[import dynamic sheet]
                    // dict<step,index>
                    // if dict contain (import) import no. +1 
                    stepname = stepname + extractedSubstring;
                    var worksheet = package.Workbook.Worksheets[stepname] ?? package.Workbook.Worksheets.Add(stepname);
                    // Console.Write(worksheet.Name.ToString());
                    //var worksheet = xlpackage.wor
                    int startingRow = 5;


                    foreach (var innerKvp in innerDict)
                    {
                        List<string> columnHeaderlist = new List<string>();
                        // if key matched 
                        // iterate through all values and check if column matched 
                        // if column matched break

                        if (mappingCorrection.Keys.Contains(innerKvp.Key))
                        {
                            columnHeaderlist = mappingCorrection[innerKvp.Key];
                        }
                        else
                        {
                            columnHeaderlist.Add(innerKvp.Key);
                            // columnHeaderist = innerKvp.Key;
                        }
                        foreach (string cheader in columnHeaderlist)
                        {
                            //have to handle this later for daily valuation thrd column is dynamic date unable to map with excel column
                            string columnHeader = cheader;

                            //have to handle this later for daily valuation third column is dynamic date unable to map with excel column
                            if (columnHeader.Contains("2023"))
                            {
                                columnHeader = "Price";
                            }
                            // Find the matching column based on the header
                            var column = worksheet.Cells[startingRow, 1, startingRow, worksheet.Dimension.End.Column].FirstOrDefault(c =>
                                c.Text == columnHeader);
                            //Console.Write(column.ToString(),columnHeader);

                            if (column != null)
                            {
                                // Console.Write(column.ToString() + "MATCHED WITH" + columnHeader);
                                // Get the data list for the current column header
                                var dataList = innerKvp.Value;

                                // Fill the data from the dataList into the corresponding cells
                                for (int i = 0; i < dataList.Count; i++)
                                {
                                    //worksheet.Cells[startingRow + i, column.Start.Column].Value = dataList[i];
                                    worksheet.Cells[startingRow + 1 + i, column.Start.Column].Value = dataList[i];

                                    //Console.Write(column.Start.Column.ToString() + "......................inserted in cell");
                                }
                            }
                        }
                    }
                }
            }
            stepname = stepname1;
            indexOfUnderscore = stepname.IndexOf("_");
            extractedSubstring = "";
            if (indexOfUnderscore != -1)
            {
                // Extract the part of the string after the underscore
                extractedSubstring = stepname.Substring(indexOfUnderscore + 1);
                stepname = stepname.Substring(0, indexOfUnderscore);
                // Perform your checks on the extracted substring



            }

            // Process dictmapalldata
            foreach (var outerKvp in dictmapalldata)
            {
                var outerKeyPair = outerKvp.Key;
                if (stepname == outerKeyPair.Value)
                {
                    var innerDict = outerKvp.Value;

                    // Get or create the subsheet based on string2 (outerKeyPair.Value)
                    stepname = stepname + extractedSubstring;
                    var worksheet = package.Workbook.Worksheets[stepname] ?? package.Workbook.Worksheets.Add(stepname);
                    //   Console.Write(worksheet.Name.ToString());

                    int startingRow = 5; // Start filling data from row 6 in the subsheet

                    int lastFilledRow = worksheet.Dimension?.End.Row ?? startingRow;


                    // Fill the column headers (string3) in row 5
                    int currentColumn = 1;
                    //foreach (var columnHeader in innerDict.Keys)
                    //{
                    //    worksheet.Cells[startingRow, currentColumn].Value = columnHeader;
                    //    currentColumn++;
                    //}

                    // Fill the data (string4) for each column header (string3)
                    foreach (var innerKvp in innerDict)
                    {
                        //var columnHeader = innerKvp.Key;
                        List<string> columnHeaderlist = new List<string>();
                        // if key matched 
                        // iterate through all values and check if column matched 
                        // if column matched break

                        if (mappingCorrection.Keys.Contains(innerKvp.Key))
                        {
                            columnHeaderlist = mappingCorrection[innerKvp.Key];
                        }
                        else
                        {
                            columnHeaderlist.Add(innerKvp.Key);
                            // columnHeaderist = innerKvp.Key;
                        }
                        foreach (string cheader in columnHeaderlist)
                        {
                            //have to handle this later for daily valuation thrd column is dynamic date unable to map with excel column
                            string columnHeader = cheader;

                            // Find the matching column based on the header
                            var column = worksheet.Cells[startingRow, 1, startingRow, worksheet.Dimension.End.Column].FirstOrDefault(c =>
                            c.Text == columnHeader);

                            if (column != null)
                            {
                                var dataValue = innerKvp.Value;

                                // Fill the data in the corresponding cells
                                // worksheet.Cells[startingRow + 1, column.Start.Column].Value = dataValue;
                                worksheet.Cells[startingRow + 1, column.Start.Column].Value = dataValue;
                                // if need to append without overriting then use last index
                                //worksheet.Cells[lastFilledRow + 1, column.Start.Column].Value = dataValue;

                            }
                        }
                    }
                }
            }

            // Save the changes back to the file
            package.Save();
        }

    }

    //private static void FillExcelSheetWithDictMap(string filePath)
    //{
    //    // Load the existing Excel file
    //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    //    var fileInfo = new FileInfo(filePath);
    //    using (var package = new ExcelPackage(fileInfo))
    //    {
    //        foreach (var outerKvp in dictmap)
    //        {
    //            var outerKeyPair = outerKvp.Key;
    //            var innerDict = outerKvp.Value;

    //            // Get or create the subsheet based on string2 (outerKeyPair.Value)
    //            var worksheet = package.Workbook.Worksheets[outerKeyPair.Value] ?? package.Workbook.Worksheets.Add(outerKeyPair.Value);
    //           // Console.Write(worksheet.Name.ToString());

    //            int startingRow = 5; 


    //            foreach (var innerKvp in innerDict)
    //            {
    //                var columnHeader = innerKvp.Key;

    //                // Find the matching column based on the header
    //                var column = worksheet.Cells[startingRow, 1, startingRow, worksheet.Dimension.End.Column].FirstOrDefault(c =>
    //                    c.Text == columnHeader);
    //                //Console.Write(column.ToString(),columnHeader);

    //                if (column != null)
    //                {
    //                   // Console.Write(column.ToString() + "MATCHED WITH" + columnHeader);
    //                    // Get the data list for the current column header
    //                    var dataList = innerKvp.Value;

    //                    // Fill the data from the dataList into the corresponding cells
    //                    for (int i = 0; i < dataList.Count; i++)
    //                    {
    //                        worksheet.Cells[startingRow + 1 + i, column.Start.Column].Value = dataList[i];
    //                        //Console.Write(column.Start.Column.ToString() + "......................inserted in cell");
    //                    }
    //                }
    //            }
    //        }

    //        // Process dictmapalldata
    //        foreach (var outerKvp in dictmapalldata)
    //        {
    //            var outerKeyPair = outerKvp.Key;
    //            var innerDict = outerKvp.Value;

    //            // Get or create the subsheet based on string2 (outerKeyPair.Value)
    //            var worksheet = package.Workbook.Worksheets[outerKeyPair.Value] ?? package.Workbook.Worksheets.Add(outerKeyPair.Value);
    //         //   Console.Write(worksheet.Name.ToString());

    //            int startingRow = 5; // Start filling data from row 6 in the subsheet

    //            // Fill the column headers (string3) in row 5
    //            int currentColumn = 1;
    //            //foreach (var columnHeader in innerDict.Keys)
    //            //{
    //            //    worksheet.Cells[startingRow, currentColumn].Value = columnHeader;
    //            //    currentColumn++;
    //            //}

    //            // Fill the data (string4) for each column header (string3)
    //            foreach (var innerKvp in innerDict)
    //            {
    //                var columnHeader = innerKvp.Key;

    //                // Find the matching column based on the header
    //                var column = worksheet.Cells[startingRow, 1, startingRow, worksheet.Dimension.End.Column].FirstOrDefault(c =>
    //                    c.Text == columnHeader);

    //                if (column != null)
    //                {
    //                    var dataValue = innerKvp.Value;

    //                    // Fill the data in the corresponding cells
    //                    worksheet.Cells[startingRow + 1, column.Start.Column].Value = dataValue;
    //                }
    //            }
    //        }

    //        // Save the changes back to the file
    //        package.Save();
    //    }

    //}
    //static private void filldatafromgrid(AutomationElement gridelement)
    //{
    //    Condition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tree);
    //    AutomationElement grid = gridelement.FindFirst(System.Windows.Automation.TreeScope.Descendants, condition1);
    //    Condition condition = new PropertyCondition(AutomationElement.AutomationIdProperty, "[Editor] Edit Area");
    //    AutomationElementCollection rowelements = grid.FindAll(System.Windows.Automation.TreeScope.Descendants, condition);


    //    CreateExceptionReport("number of elements in grid " + rowelements.Count);
    //    // CreateExceptionReport("number of columns in grid " + columnhead.Count);

    //    if (rowelements != null)
    //        CreateExceptionReport("INSIDE GRID DATA WRITING");
    //    {
    //        foreach (AutomationElement e in rowelements)
    //        {

    //            AutomationElement parentElement = TreeWalker.ControlViewWalker.GetParent(e);


    //            string eleme = "";
    //            ValuePattern valuePattern = e.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
    //            if (valuePattern != null)
    //            {
    //                eleme = valuePattern.Current.Value;

    //            }

    //            //if (dataidmap.ContainsKey(parentElement.Current.Name))
    //            //{
    //            //    dataidmap[parentElement.Current.Name].Add(eleme);
    //            //}
    //            //else
    //            //{
    //            //    dataidmap.Add(parentElement.Current.Name,new List<string> { eleme });
    //            //}
    //        }

    //    }

    //}

    //static private void writerdata(string sheetid)
    //{
    //    // initialize service
    //    // give sheetid 
    //    // iterate through map data if column found fill data in sheet 
    //    // for textboxes only one value per automation id 
    //    // for grid column name -> value1 value2 value3 

    //    // map of string,list<string>
    //    // column header in sheet 
    //    //txtsymbol:[aapl], orderside:[buy], tradedate:[1,2,3]
    //    // Dictionary<string, string> data = new Dictionary<string, string[]>();

    //    // here if current contains grid get the grid and write data 
    //    //combobox editbox
    //    //grid 
    //    var data = dataidmap;
    //    //{
    //    //    { "txtSymbol", new string[] { "x", "y", "z" } },
    //    //    { "cmbFxOperator", new string[] { "4" } }
    //    //};
    //    GoogleCredential credential;
    //    using (var stream = new FileStream(@"C:\Users\vineet.tanwar\source\repos\ConsoleApp12\named-inn-392808-5a7c71940ad3.json", FileMode.Open, FileAccess.Read))
    //    {
    //        credential = GoogleCredential.FromStream(stream)
    //            .CreateScoped(SheetsService.Scope.Spreadsheets);
    //    }

    //    var service = new SheetsService(new BaseClientService.Initializer()
    //    {
    //        HttpClientInitializer = credential,
    //        ApplicationName = "named-inn-392808"
    //    });


    //    string spreadsheetId = sheetid;
    //    string sheetName = "Sheet1";
    //    var columnHeaders = GetColumnHeaders(service, spreadsheetId, sheetName);
    //    var lastRowIndex = GetLastRowIndex(service, spreadsheetId, sheetName);
    //    int maxRowCount = 0;
    //    foreach (var columnData in data.Values)
    //    {
    //        if (columnData.Count > maxRowCount)
    //            maxRowCount = columnData.Count;
    //    }
    //    //var range = $"{sheetName}!A{lastRowIndex + 1}";
    //    SpreadsheetsResource.ValuesResource.GetRequest getRequest = service.Spreadsheets.Values.Get(spreadsheetId, "A1:1");
    //    ValueRange getResponse = getRequest.Execute();
    //    IList<IList<object>> existingValues = getResponse.Values;

    //    HashSet<string> existingColumns = new HashSet<string>();
    //    foreach (var existingColumnName in existingValues[0])
    //    {
    //        existingColumns.Add(existingColumnName.ToString());
    //    }

    //    List<IList<object>> rows = new List<IList<object>>();

    //    for (int i = 0; i < maxRowCount; i++)
    //    {
    //        List<object> row = new List<object>();
    //        foreach (var columnName in existingColumns)
    //        {
    //            if (data.ContainsKey(columnName))
    //            {
    //                if (data[columnName].Count >= i)
    //                    row.Add(data[columnName][i]);
    //                else
    //                    row.Add("");
    //            }
    //            else
    //            {
    //                row.Add("");
    //            }
    //        }
    //        rows.Add(row);
    //    }


    //    ValueRange valueRange = new ValueRange
    //    {
    //        Values = rows,
    //    };
    //    var range = $"{sheetName}!A{lastRowIndex + 1}";
    //    SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
    //    updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
    //    UpdateValuesResponse response = updateRequest.Execute();


    //}


    //static private string writermain(string currwin, string stepname)
    //{
    //    stepname = getstepnamefrommap(stepname);
    //    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
    //    keyValuePairs.Add("window", currwin);
    //    keyValuePairs.Add("stepname", stepname);
    //    GoogleCredential credential;

    //    using (var stream = new FileStream(@"C:\Users\vineet.tanwar\source\repos\ConsoleApp12\named-inn-392808-5a7c71940ad3.json", FileMode.Open, FileAccess.Read))
    //    {
    //        credential = GoogleCredential.FromStream(stream)
    //            .CreateScoped(SheetsService.Scope.Spreadsheets);
    //    }

    //    var service = new SheetsService(new BaseClientService.Initializer()
    //    {
    //        HttpClientInitializer = credential,
    //        ApplicationName = "named-inn-392808"
    //    });
    //    string spreadsheetId = "1XkpCqbhbDE4_prSEjRtIOyQi_PAFmZVGE8G4DdNqMAA";
    //    string sheetName = "Sheet1";
    //    var columnData = new Dictionary<string, object>();

    //    foreach (var kvp in keyValuePairs)
    //    {
    //        columnData[kvp.Key] = kvp.Value;
    //    }


    //    var columnHeaders = GetColumnHeaders(service, spreadsheetId, sheetName);
    //    var lastRowIndex = GetLastRowIndex(service, spreadsheetId, sheetName);

    //    var range = $"{sheetName}!A{lastRowIndex + 1}";

    //    var values = new List<object>();
    //    foreach (var columnHeader in columnHeaders.Keys)
    //    {
    //        if (columnData.ContainsKey(columnHeader))
    //            values.Add(columnData[columnHeader]);
    //        //else
    //        // values.Add(string.Empty);
    //    }

    //    var valueRange = new ValueRange()
    //    {
    //        Values = new List<IList<object>>()
    //                {
    //                    values
    //                }
    //    };

    //    var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
    //    appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
    //    var appendResponse = appendRequest.Execute();

    //    CreateExceptionReport("Data written successfully to Google Sheet for step file.");

    //    //link string reading code 
    //    string cellRange = $"{sheetName}!D{lastRowIndex + 1}";
    //    string linkstring = ReadCellValue(service, spreadsheetId, cellRange);
    //    return linkstring;


    //}
    //public static string getstepnamefrommap(string stepname)
    //{
    //    return _stepnamemap[stepname];
    //}
    //public static string ExtractSheetIdFromUrl(string url)
    //{

    //    string pattern = @"/d/([a-zA-Z0-9-_]+)";


    //    Match match = Regex.Match(url, pattern);

    //    if (match.Success)
    //    {
    //        string sheetId = match.Groups[1].Value;
    //        return sheetId;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}
    //private static string ReadCellValue(SheetsService service, string spreadsheetId, string cellRange)
    //{
    //    SpreadsheetsResource.ValuesResource.GetRequest request =
    //        service.Spreadsheets.Values.Get(spreadsheetId, cellRange);

    //    ValueRange response = request.Execute();
    //    var values = response.Values;

    //    if (values != null && values.Count > 0)
    //    {
    //        return values[0][0].ToString();
    //    }
    //    else
    //    {
    //        return "No data found.";
    //    }
    //}

    //static int GetLastRowIndex(SheetsService service, string spreadsheetId, string sheetName)
    //{
    //    var request = service.Spreadsheets.Values.Get(spreadsheetId, $"{sheetName}!A:A");
    //    request.MajorDimension = SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.COLUMNS;
    //    var response = request.Execute();

    //    var values = response.Values;
    //    if (values == null || values.Count == 0)
    //    {
    //        return 0;
    //    }
    //    return values[0].Count;
    //}
    //static Dictionary<string, object> GetColumnHeaders(SheetsService service, string spreadsheetId, string sheetName)
    //{
    //    var range = $"{sheetName}!1:1";
    //    var request = service.Spreadsheets.Values.Get(spreadsheetId, range);

    //    var response = request.Execute();
    //    var values = response.Values;

    //    var columnHeaders = new Dictionary<string, object>();

    //    if (values != null && values.Count > 0)
    //    {
    //        foreach (var columnHeader in values[0])
    //        {
    //            columnHeaders.Add(columnHeader.ToString(), string.Empty);
    //        }
    //    }

    //    return columnHeaders;
    //}




    //static private void getdatafromgrid(AutomationElement currgrid, string foregroundwin, string prevwin)
    //{
    //    Condition condition = new PropertyCondition(AutomationElement.AutomationIdProperty, "[Editor] Edit Area");
    //    AutomationElementCollection rowelements = currgrid.FindAll(System.Windows.Automation.TreeScope.Descendants, condition);
    //    Condition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.HeaderItem);

    //    AutomationElementCollection columnhead = currgrid.FindAll(System.Windows.Automation.TreeScope.Descendants, condition1);
    //    CreateExceptionReport("number of elements in grid " + rowelements.Count);
    //    CreateExceptionReport("number of columns in grid " + columnhead.Count);


    //    if (foregroundwin != prevwin)
    //    {
    //        if (rowelements != null)

    //        {
    //            string currentDirectoryy = AppDomain.CurrentDomain.BaseDirectory;

    //            string filepathh = currentDirectoryy + "files\\" + "griddata.txt";
    //            StreamWriter writer = new StreamWriter(filepathh, true);
    //            int count = 0;

    //            foreach (AutomationElement e in rowelements)
    //            {
    //                AutomationElement parentcheck = TreeWalker.ControlViewWalker.GetParent(e);
    //                if (parentcheck != null)
    //                {

    //                    getparent(e);

    //                    // this is defining which grid data belongs too 

    //                }

    //                AutomationElement parentElement = TreeWalker.ControlViewWalker.GetParent(e);
    //                if (parentElement != null)
    //                {
    //                    //CreateExceptionReport("INFORMATION OF ROW  PARENT ");
    //                    //CreateExceptionReport(parentElement);
    //                    CreateExceptionReport("this is parent name :" + parentElement.Current.Name);
    //                    writer.Write(parentElement.Current.Name + "=");
    //                }
    //                string eleme = "";
    //                ValuePattern valuePattern = e.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
    //                if (valuePattern != null)
    //                {
    //                    eleme = valuePattern.Current.Value;
    //                    writer.Write(eleme + "^");
    //                    CreateExceptionReport("this is the element :" + eleme);
    //                }
    //                writer.WriteLine();
    //                count++;
    //            }
    //            //CreateExceptionReport(count);
    //            // parentset.Clear();


    //            writer.Close();
    //            writer.Dispose();



    //        }

    //    }
    //}

    //               CreateExceptionReport("seventh parent " + parentElement6.Current.AutomationId);
    //                                CreateExceptionReport(parentElement6.Current.Name);

    //                                AutomationElement parentElement7 = TreeWalker.ControlViewWalker.GetParent(parentElement6);
    //                                if (parentElement7 != null)
    //                                {
    //                                    CreateExceptionReport("eighth parent " + parentElement7.Current.AutomationId);
















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