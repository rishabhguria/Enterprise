using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIAutomationClient;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using OfficeOpenXml;

namespace WorkflowAutomation
{
    internal class InterceptMouseAndKeyboard
    {
        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        private const uint EVENT_OBJECT_CREATE = 0x8000;

        private static string prevwin = "";

        // NEW IMPLEMENTATION DICTIONARIES 
        static Dictionary<KeyValuePair<string, string>, Dictionary<string, List<string>>> dictmap = new Dictionary<KeyValuePair<string, string>, Dictionary<string, List<string>>>();
        static Dictionary<KeyValuePair<string, string>, Dictionary<string, string>> dictmapalldata = new Dictionary<KeyValuePair<string, string>, Dictionary<string, string>>();


        static Dictionary<string, string> map = new Dictionary<string, string>();
        static Dictionary<string, KeyValuePair<string, string>> tradingdata = new Dictionary<string, KeyValuePair<string, string>>();

        static Dictionary<string, List<string>> dataidmap = new Dictionary<string, List<string>>();

        // static Dictionary<string, List<string>> comboboxes = new Dictionary<string, List<string>>();
        static Dictionary<string, List<string>> blotterorder = new Dictionary<string, List<string>>();
        static Dictionary<string, List<string>> workingsubs = new Dictionary<string, List<string>>();
        // Dictionary<string, string>={"OrderBlotterGrid",}
        static List<string> _stepnameloglist = new List<string>();
        static Dictionary<string, KeyValuePair<string, KeyValuePair<string, string>>> ttdata = new Dictionary<string, KeyValuePair<string, KeyValuePair<string, string>>>();
        static HashSet<string> parentset = new HashSet<string>();
        static List<string> autoids = new List<string>();
        //generic sheet writing 
        static string gstring = "https://docs.google.com/spreadsheets/d/1PQekmcHmBX57NCMW0aNZz8TySEt2RdTHqlYOo6WTTYU/edit#gid=661870191";
        static int rownum = 6;
        static Dictionary<string, List<string>> _moduleAutomationElements = new Dictionary<string, List<string>>();
        // static Dictionary<string , List<string>> _moduleactionelements=new Dictionary<string , List<string>>();
        static List<string> _moduleactionelements = new List<string>();
        static Dictionary<string, List<string>> _mapofdata = new Dictionary<string, List<string>>();
        static Dictionary<string, string> _stepnamemap = new Dictionary<string, string>();
        static Dictionary<string, string> _stepnamecorrection = new Dictionary<string, string>();

        //cmborderside,<"1":buy>,<"2":sell
        static Dictionary<string, Dictionary<string, string>> _correctioncolumn = new Dictionary<string, Dictionary<string, string>>();

        //new method 
        static Dictionary<string, List<string>> _moduletostepname = new Dictionary<string, List<string>>();
        //<tradingticket , ["createorder" , "doneaway"]>;
        //<blotter,["verifyordergrid"];
        static Dictionary<string, List<string>> _stepnametoelement = new Dictionary<string, List<string>>();
        //<createorder,<txtsymbol,cmbquantity>
        //    <verifyordergrid,<"ordersblottergrid">

        //_actiontostepname cr
        static Dictionary<string, List<string>> _btntostepname = new Dictionary<string, List<string>>();
        private static Dictionary<string, string> mappingCorrection = new Dictionary<string, string>();

        private static string logFilename;

        static Dictionary<string, List<string>> ordersblottergrid = new Dictionary<string, List<string>>();
        static Dictionary<string, List<string>> alldata = new Dictionary<string, List<string>>();
        static Dictionary<string, List<string>> workingsubsgrid = new Dictionary<string, List<string>>();
        static IUIAutomationElement _currwinglobal = null;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private static LowLevelMouseProc _mouseProc = MouseHookCallback;
        private static LowLevelKeyboardProc _keyboardProc = KeyboardHookCallback;
        //private static IntPtr _mouseHookID = IntPtr.Zero;
        //private static IntPtr _keyboardHookID = IntPtr.Zero;
        //  IntPtr hook = SetWinEventHook(EVENT_OBJECT_CREATE, EVENT_OBJECT_CREATE, IntPtr.Zero, WinEventCallback, 0, 0, 0);
        private static IntPtr _mainWindowHandle = IntPtr.Zero;
        private static bool _filecreated = false;
        private static string _filename = "";
        private static string _columnname = "";
        private static string _filterbasis = "";
        private static bool _filteron = false;


        //public static void Main()
        //{
        //    _mouseHookID = SetMouseHook(_mouseProc);
        //    _keyboardHookID = SetKeyboardHook(_keyboardProc);
        //    _mainWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
        //    //yesautoids.AddRange(new string[] { "txtSymbol_EmbeddableTextBox", "cmbOrderSide_EmbeddableTextBox", "cmbAllocation_EmbeddableTextBox", "cmbOrderType", "cmbTIF_EmbeddableTextBox", "cmbVenue_EmbeddableTextBox" });
        //    FillAutomationElementsByModule();
        //    FillModuleWiseBizSteps();

        //    ExtractDataForBizActions();
        //    CreateRoboInstOnBizActions();

        //    //TODO : REplace this mapping by the logic
        //    MapModuleNameForWriting();

        // InitializeMapping();
        //    InitializeLogFile();

        //    Application.Run();

        //    UnhookWindowsHookEx(_mouseHookID);
        //    UnhookWindowsHookEx(_keyboardHookID);

        //}
        private static IntPtr _mouseHookID = IntPtr.Zero;
        private static IntPtr _keyboardHookID = IntPtr.Zero;
       

        internal static void Setup()
        {
            _mouseHookID = InterceptMouseAndKeyboard.SetMouseHook(_mouseProc);
            _keyboardHookID = InterceptMouseAndKeyboard.SetKeyboardHook(_keyboardProc);
        }

        internal static void Cleanup()
        {

            UnhookWindowsHookEx(_mouseHookID);
            UnhookWindowsHookEx(_keyboardHookID);
        }




        static void ExtractDataForBizActions()
        {
            _stepnametoelement.Add("CreateStageOrder", new List<string> { "txtSymbol", "cmbStrategy", "cmbFxOperator", "cmbTIF", "cmbOrderType", "cmbCommissionBasis", "cmbSoftCommissionBasis", "cmbOrderSide", "cmbVenue", "cmbAllocation", "cmbBroker" });
            //_stepnametoelement.Add("checkBoth", new List<string> { "grdLong", "cmbSecondarySort", "cmbMethodology", "cmbAlgorithm" });
            _stepnametoelement.Add("checkclosing", new List<string> { "cmbSecondarySort", "cmbMethodology", "cmbAlgorithm" });
            _stepnametoelement.Add("GetCashTransaction", new List<string> { "dtDateType" });
            _stepnametoelement.Add("ViewDataOnRecon", new List<string> { "dtFromDatePicker", "dtToDatePicker", "cmbbxReconTemplates", "cmbbxReconType", "cmbbxClient" });
            _stepnametoelement.Add("CheckAllocatedGroups", new List<string> { "GridAllocated" });
            _stepnametoelement.Add("Import", new List<string> { "gridRunUpload" });
            _stepnametoelement.Add("GetDayEndCash", new List<string> { "dtPickerUpper", "dtPickerlower" });
            _stepnametoelement.Add("GetNonTradingTransaction", new List<string> { "dtPickerUpper", "dtPickerlower" });

            _stepnametoelement.Add("VerifyCashTransaction", new List<string> { "grdCreatePosition" });
            _stepnametoelement.Add("AddCashTransaction", new List<string> { "grdCreatePosition" });

            _stepnametoelement.Add("VerifyApplicationData", new List<string> { "grdData" });
            _stepnametoelement.Add("RunManualRevaluation", new List<string> { "udtBalanceDate" });

            _stepnametoelement.Add("VerifyClosedTrades", new List<string> { "grdNetPosition" });
            _stepnametoelement.Add("VerifyShortPositionsGrid", new List<string> { "grdShort" });
            _stepnametoelement.Add("VerifyLongPositionsGrid", new List<string> { "grdLong" });
            _stepnametoelement.Add("GetDataClosing", new List<string> { "rbHistorical", "dtFromDate", "dtToDate", "MultiSelectEditor" });
            _stepnametoelement.Add("AutomaticClosing", new List<string> { "chkBoxBuyAndBuyToCover", "chkBxIsAutoCloseStrategy", "cmbAlgorithm", "cmbSecondarySort", "cmbClosingField" });



            // _stepnametoelement.Add("checkgridlong", new List<string> { "grdLong" });
            _stepnametoelement.Add("CreateDoneAwayOrder", new List<string> {"txtSymbol","cmbStrategy","cmbFxOperator","cmbTIF","cmbOrderType","cmbCommissionBasis","cmbSoftCommissionBasis","cmbOrderSide","cmbVenue","cmbAllocation","cmbBroker"
 });
            _stepnametoelement.Add("VerifyOrder", new List<string> { "OrderBlotterGrid" });
            _stepnametoelement.Add("VerifyWorkingSubs", new List<string> { "WorkingSubBlotterGrid" });
            _stepnametoelement.Add("VerifyPMGrid", new List<string> { "pmGrid" });
            _stepnametoelement.Add("VerifyPMDashboard", new List<string> { "pmDashboard" });

            //new step
            // _stepnametoelement.Add("ValidateSymbolTT", new List<string> { "txtSymbol" });
            _stepnametoelement.Add("UpdateMarkPrice", new List<string> { "grpBoxGrid" });
        }

        internal static void FillModuleWiseBizSteps()
        {
            _moduletostepname.Add("Blotter", new List<string> { "VerifyOrder", "VerifyWorkingSubs" });
            _moduletostepname.Add("Trading Ticket", new List<string> { "CreateStageOrder", "CreateDoneAwayOrder" });
            _moduletostepname.Add("Close Trade", new List<string> { "VerifyClosedTrades", "VerifyShortPositionsGrid", "VerifyLongPositionsGrid", "GetDataClosing", "AutomaticClosing" });
            _moduletostepname.Add("General Ledger", new List<string> { "GetCashTransaction", "GetDayEndCash", "GetNonTradingTransaction", "RunManualRevaluation" });
            _moduletostepname.Add("Reconciliation", new List<string> { "ViewDataOnRecon", "VerifyApplicationData" });
            _moduletostepname.Add("Allocation", new List<string> { "CheckAllocatedGroups", "GetDataAllocation" });

            _moduletostepname.Add("Daily Valuation", new List<string> { "UpdateMarkPrice" });
            _moduletostepname.Add("Portfolio Management", new List<string> { "VerifyPMGrid", "VerifyPMDashboard" });
            _moduletostepname.Add("Import", new List<string> { "Import" });
            _moduletostepname.Add("Create Transaction", new List<string> { "VerifyCashTransaction", "AddCashTransaction" });
        }

        static void CreateRoboInstOnBizActions()
        {
            _btntostepname.Add("Create Order", new List<string> { "CreateStageOrder" });
            _btntostepname.Add("Done Away", new List<string> { "CreateDoneAwayOrder" });
            _btntostepname.Add("Save All Layout", new List<string> { "VerifyWorkingSubs", "VerifyOrder" });
            _btntostepname.Add("Save", new List<string> { "UpdateMarkPrice", "VerifyCashTransaction", "GetNonTradingTransaction" });
            _btntostepname.Add("Save Layout", new List<string> { "VerifyPMGrid" });
            _btntostepname.Add("Main", new List<string> { "VerifyPMDashboard" });
            _btntostepname.Add("Current", new List<string> { "VerifyShortPositionsGrid", "VerifyLongPositionsGrid" });
            _btntostepname.Add("Closed Amend", new List<string> { "VerifyClosedTrades" });

            _btntostepname.Add("Historical", new List<string> { "GetDataClosing" });
            _btntostepname.Add("Run", new List<string> { "AutomaticClosing" });
            _btntostepname.Add("Cash Transactions", new List<string> { "GetCashTransaction", "AddCashTransaction" });
            _btntostepname.Add("View", new List<string> { "ViewDataOnRecon" });
            _btntostepname.Add("Save w/Status", new List<string> { "CheckAllocatedGroups" });
            _btntostepname.Add("Get Data", new List<string> { "GetDayEndCash" });
            _btntostepname.Add("Export To Excel", new List<string> { "VerifyApplicationData" });
            _btntostepname.Add("Run Revaluation", new List<string> { "RunManualRevaluation" });

        }

        static void MapModuleNameForWriting()
        {
            _stepnamecorrection.Add("Trading Ticket", "TradingTicket");
            _stepnamecorrection.Add("Portfolio Management", "PortfolioManagement");
            _stepnamecorrection.Add("Close Trade", "Closing");
            _stepnamecorrection.Add("Daily Valuation", "DailyValuation");
            _stepnamecorrection.Add("APPLE", "TradingTicket");
            _stepnamecorrection.Add("APPLE - Trading Ticket", "TradingTicket");
        }

        //private static void fillfromfileAutomationElementsByModule()
        //{
        //    tradingdata ticket ^ txtsymbol,cmbquantity;
        //    blotter ^ 
        //    //filepath=.txt
        //    //key 
        //    //value 
        //}

        
        internal static void InitializeLogFile()
        {
            // Get the current date and time
            DateTime currentDate = DateTime.Now;

            // Create a filename with the current date and time
            logFilename = $"Log_{currentDate.ToString("yyyyMMdd_HHmmss")}.txt";

            try
            {
                // Create the log file
                using (StreamWriter writer = File.CreateText(logFilename))
                {
                    writer.WriteLine($"--- Log file created on {currentDate} ---");
                }

                Console.WriteLine("Log file created: " + logFilename);
            }
            catch (Exception ex)
            {
                // If there is an error, display the error message
                Console.WriteLine("Error creating log file: " + ex.Message);
            }
        }

        private static void Log(string logMessage)
        {
            try
            {
                // Append the log message to the file
                using (StreamWriter writer = File.AppendText(logFilename))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                // If there is an error, display the error message
                Console.WriteLine("Error writing to log file: " + ex.Message);
            }
        }

        internal static void InitializeMapping()
        {
            // Add key-value pairs to the mappingCorrection dictionary
            mappingCorrection.Add("txtSymbol", "Symbol");
            mappingCorrection.Add("cmbStrategy", "Strategy");
            mappingCorrection.Add("cmbFxOperator", "Fx Operator");
            mappingCorrection.Add("cmbTIF", "TIF");
            mappingCorrection.Add("cmbOrderType", "Order Type");
            mappingCorrection.Add("cmbCommissionBasis", "Commission Basis");
            mappingCorrection.Add("cmbSoftCommissionBasis", "Soft Commission Basis");
            mappingCorrection.Add("cmbOrderSide", "Order Side");
            mappingCorrection.Add("cmbVenue", "Venue");
            mappingCorrection.Add("cmbAllocation", "AllocationMethod");
            mappingCorrection.Add("cmbBroker", "Broker");
            mappingCorrection.Add("udtBalanceDate", "Balance Date");

        }
        //private static List<string> GetAutomationIdByModule()
        //{

        //}

        private static int initialWindowCount;
        internal static IntPtr SetMouseHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {


                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }


        }

        //private static void OnWindowOpened(object sender, AutomationEventArgs e)
        //{
        //    // Get the current window count
        //    //int currentWindowCount = AutomationElement.RootElement.FindAll(TreeScope.Children, Condition.TrueCondition).Count;

        //    // Compare the current window count with the initial count
        //    //if (currentWindowCount > initialWindowCount)
        //    //{
        //    //    Console.WriteLine("New window opened!");
        //    //}
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
        internal static IntPtr SetKeyboardHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IUIAutomationElement FindElementAtPoint(int x, int y)
        {
            IUIAutomationElement element = null;
            IUIAutomation automation = new CUIAutomation8();

            try
            {
                element = automation.ElementFromPoint(new tagPOINT() { x = (int)x, y = (int)y });

                //element = AutomationElement.FromPoint(new System.Windows.Point(x, y));
                try {
                    Console.WriteLine(element.CurrentName);
                    Console.WriteLine(element.CurrentAutomationId);


                }
                catch { }
            }
            catch (Exception ex)
            {

            }
            return element;

        }
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }
        private static void PrintTextBoxDetails(IUIAutomationElement element)
        {
            try
            {
                string elementName = element.CurrentName;
                string elementAutomationId = element.CurrentAutomationId;
                string elementLocalizedControlType = element.CurrentLocalizedControlType;
                string elementClassname = element.CurrentClassName;
                string m = element.ToString();
                string n = element.CurrentHelpText;
                string b = element.CurrentAccessKey;
                string elementValue = string.Empty;

                try
                {
                    object valuePatternObj;
                    valuePatternObj = element.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);

                    IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;

                    if (valuePattern != null)
                    {
                        // Extract the value from the ComboBox
                        elementValue = valuePattern.CurrentValue;
                        //Console.WriteLine($"ComboBox value: {comboBoxValue}");

                    }

                }
                catch (Exception ex)
                {

                }
                Console.WriteLine("Clicked Element:");
                Console.WriteLine("Name: " + elementName);
                Console.WriteLine("Automation ID: " + elementAutomationId);
                Console.WriteLine("Control Type: " + elementLocalizedControlType);
                Console.WriteLine("Help Text: " + n);
                Console.WriteLine(elementClassname);
                Console.WriteLine(m);
                Console.WriteLine(b);
                Console.WriteLine("VALUE INSIDE TEXTBOX   " + elementValue);
                if (!string.IsNullOrEmpty(elementAutomationId) && !string.IsNullOrEmpty(elementValue))
                {
                    map[elementAutomationId] = elementValue;
                    Console.WriteLine("VALUE GOINGGGGGGG INSIDE MAP  " + elementValue);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private static void filldataindictfromgrid(KeyValuePair<string, string> dictname, IUIAutomationElement targetelement)
        {
            // Check if the dictionary with dictname exists in the dictmap
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

            if (grid != null)
            {
                IUIAutomationElementArray rowelements = grid.FindAll(
                           TreeScope.TreeScope_Descendants, condition);
                //AutomationElementCollection rowelements = grid.FindAll(System.Windows.Automation.TreeScope.Descendants, condition);

                Console.WriteLine("number of elements in grid " + rowelements.Length);
                // Console.WriteLine("number of columns in grid " + columnhead.Count);

                if (rowelements != null)
                    Console.WriteLine("INSIDE GRID DATA WRITING");

                {

                    for (int i = 0; i < rowelements.Length; i++)
                    {
                        IUIAutomationElement e = rowelements.GetElement(i);
                        //AutomationElement parentElement = TreeWalker.ControlViewWalker.GetParent(e);
                        IUIAutomationElement parentElement = automation.ContentViewWalker.GetParentElement(e);

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

                        if (dictmap[dictname].ContainsKey(parentElement.CurrentName))
                        {
                            dictmap[dictname][parentElement.CurrentName].Add(eleme);
                        }
                        else
                        {
                            dictmap[dictname].Add(parentElement.CurrentName, new List<string> { eleme });
                        }
                    }
                }
            }
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

                        writer.WriteLine($"gridname: {kvp.Key}\tstepname: {kvp.Value}");

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

                //Console.WriteLine("Data has been written to the file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
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

                //Console.WriteLine("Data has been written to the file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
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

            if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_ComboBoxControlTypeId)
            {
                Stopwatch sw2 = new Stopwatch();
                sw2.Start();
                valuetouse = "";
                IUIAutomation automation = new CUIAutomation8();
                IUIAutomationCondition lstitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);

                IUIAutomationElementArray lstelement = targetelement.FindAll(
                   TreeScope.TreeScope_Descendants, lstitem);

                //Condition lstitem = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem);
                //AutomationElementCollection lstelement = targetelement.FindAll(System.Windows.Automation.TreeScope.Descendants, lstitem);


                //if (lstelement != null)
                //{


                //    foreach (AutomationElement e2 in lstelement)
                //    {
                //        // Console.WriteLine(e2.Current.Name);
                //        try
                //        {
                //            string value = null;
                //            object patternprovider;
                //            if (e2.TryGetCurrentPattern(SelectionItemPattern.Pattern, out patternprovider))
                //            {
                //                SelectionItemPattern selectionpatternprovider = patternprovider as SelectionItemPattern;
                //                value = selectionpatternprovider.Current.ToString();
                //                bool _isselected = selectionpatternprovider.Current.IsSelected;
                //                // Console.WriteLine(selectionpatternprovider.Current.IsSelected.ToString());
                //                if (_isselected)
                //                {
                //                    valuetouse = e2.Current.Name;
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        {

                //        }
                //    }
                //}
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
                Console.WriteLine("time taken for one combobox " + etime);
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
                        Console.WriteLine(selectionpatternprovider.CurrentToggleState.ToString());
                        valuetouse = togglestate;
                    }
                }
                catch (Exception ex)
                {

                }

            }
            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_RadioButtonControlTypeId)
            {
                valuetouse = targetelement.CurrentName;

            }
            else if (targetelement.CurrentAutomationId == "MultiSelectEditor")
            {
                Console.WriteLine("inside multi select ");
                
                //IUIAutomation automation2 = new CUIAutomation8();
                //IUIAutomationCondition bttn = automation2.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ButtonControlTypeId);
                //IUIAutomationElement btnelement = targetelement.FindFirst(TreeScope.TreeScope_Descendants, bttn);
                //if (btnelement != null)
                //{
                //    Console.WriteLine("Found Button element ");
                //    //expand pattern invoke
                //    try
                //    {
                //        string value = null;
                //        object patternprovider;
                //        if (btnelement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) != null)
                //        {
                //            patternprovider = btnelement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
                //            Console.WriteLine("inside expand pattern");
                //            IUIAutomationExpandCollapsePattern selectionpatternprovider = patternprovider as IUIAutomationExpandCollapsePattern;
                //            value = selectionpatternprovider.CurrentExpandCollapseState.ToString();
                //            //Console.WriteLine("......////...........");
                //            Console.WriteLine(selectionpatternprovider.CurrentExpandCollapseState.ToString());
                //            selectionpatternprovider.Expand();
                //            Console.WriteLine(selectionpatternprovider.CurrentExpandCollapseState.ToString());
                //            IUIAutomation automation1 = new CUIAutomation8();
                //            IUIAutomationCondition listt = automation1.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "checkedMultipleItems");
                //            IUIAutomationElement listelement = _currwinglobal.FindFirst(TreeScope.TreeScope_Descendants, listt);
                //            if (listelement != null)
                //            {
                //                // Condition checkboxitem = new PropertyCondition(AutomationElement.ControlTypeProperty,listelement);
                //                // AutomationElementCollection checkboxes = listelement.FindAll(TreeScope.Children, checkboxitem);
                //                IUIAutomation automation = new CUIAutomation8();

                //                IUIAutomationCondition conditiont = automation.CreateTrueCondition();

                //                IUIAutomationElementArray checkboxes = listelement.FindAll(TreeScope.TreeScope_Descendants, conditiont);
                //                //Console.WriteLine(checkboxes[0].Current.Name);
                //                Console.WriteLine("SIZE OF List" + checkboxes.Length);

                //                if (checkboxes != null)
                //                {
                //                    Console.Write("inside checkboxes ");
                //                    for (int i = 0; i < checkboxes.Length; i++)
                //                    {
                //                        IUIAutomationElement checkbox = checkboxes.GetElement(i);
                //                        Console.WriteLine(checkbox.CurrentName + "..");
                //                        try
                //                        {
                //                            //string value = null;
                //                            object patternproviderrr;
                //                            if (checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId) != null)
                //                            {
                //                                patternproviderrr = checkbox.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                //                                IUIAutomationTogglePattern chkboxpatternprovider = patternproviderrr as IUIAutomationTogglePattern;
                //                                // value = selectionpatternprovider.Current.ToString();
                //                                // Console.WriteLine("......////...........");
                //                                Console.WriteLine(chkboxpatternprovider.ToString());
                //                                Console.WriteLine(chkboxpatternprovider.CurrentToggleState.ToString());
                //                                if (chkboxpatternprovider.CurrentToggleState.ToString() == "On")
                //                                {
                //                                    valuetouse += ("," + checkbox.CurrentName);
                //                                }
                //                            }
                //                        }
                //                        catch (Exception ex)
                //                        {

                //                        }
                //                    }
                //                }
                //            }

                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}
                valuetouse = "dummy";

                
            }
            else if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_EditControlTypeId)
            {
                try
                {
                    IUIAutomationValuePattern valuePattern = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                    if (valuePattern != null)
                    {
                        valuetouse = valuePattern.CurrentValue;
                        Console.WriteLine(valuetouse);
                        Console.WriteLine(DateTime.Now);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("INSIDE EDIT ITEM CANNOT GET VALUE");
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
                    // Handle exceptions as needed
                }
                Console.WriteLine("checking");
                Console.WriteLine(DateTime.Now.ToString());
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
            try
            {
                // Add elementtoget and elementValue to the dictionary
                dictmapalldata[dictname].Add(elementtoget, valuetouse);
            }
            catch
            {
                dictmapalldata[dictname][elementtoget] = valuetouse;
            }
            //string elementtoget = targetelement.Current.AutomationId;

            //string elementValue = "";
            //try
            //{
            //    ValuePattern valuePattern = targetelement.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            //    if (valuePattern != null)
            //    {
            //        elementValue = valuePattern.Current.Value;
            //        if (_correctioncolumn.Keys.Contains(elementtoget))
            //        {
            //            elementValue=_correctioncolumn[elementtoget][elementValue];
            //        }

            //        //if targetelement id in correctvalues 
            //        // go to targetelement 
            //        //element value= correctvalues[targetelementid][elementvalue]
            //        //if()


            //        //if (targetelement.Current.ControlType == ControlType.ComboBox)
            //        //{

            //        //    AutomationElementCollection listitems = targetelement.FindAll(TreeScope.Descendants, Condition.TrueCondition);
            //        //    foreach (AutomationElement item in listitems)
            //        //    {
            //        //        if (item.Current.AutomationId.Contains("[valuelist] ValueListItem"))
            //        //        {
            //        //            if (item.Current.AutomationId.Contains($"{elementValue}"))
            //        //            {

            //        //                elementValue = item.Current.Name;
            //        //            }
            //        //        }
            //        //    }
            //        //}
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Handle exceptions as needed
            //}


            //string selectedText = "";
            //try
            //{
            //    SelectionPattern selectionPattern = targetelement.GetCurrentPattern(SelectionPattern.Pattern) as SelectionPattern;
            //    if (selectionPattern != null)
            //    {
            //        AutomationElement selectedItem = selectionPattern.Current.GetSelection()[0];
            //      // AutomationElement selectedItem = selectionPattern.Current.GetSelection()[0];
            //        selectedText = selectedItem.Current.Name;
            //        //if (targetelement.Current.ControlType == ControlType.ComboBox)
            //        //{

            //        //    AutomationElementCollection listitems = targetelement.FindAll(TreeScope.Descendants, Condition.TrueCondition);
            //        //    foreach (AutomationElement item in listitems)
            //        //    {
            //        //        if (item.Current.AutomationId.Contains("[valuelist] ValueListItem"))
            //        //        {
            //        //            if (item.Current.AutomationId.Contains($"{selectedText}"))
            //        //            {

            //        //                selectedText = item.Current.Name;
            //        //            }
            //        //        }
            //        //    }
            //        //}
            //        //Console.WriteLine(selectedItem.ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Handle exceptions as needed
            //}
            //Console.WriteLine("checking");
            //Console.WriteLine(targetelement.Current.AutomationId  +"    " +   elementValue);
            //Console.WriteLine(targetelement.Current.AutomationId + "    " + selectedText);    
            //if (elementValue != "")
            //{
            //    try
            //    {
            //        // Add elementtoget and elementValue to the dictionary
            //        dictmapalldata[dictname].Add(elementtoget, elementValue);
            //    }
            //    catch
            //    {
            //        dictmapalldata[dictname][elementtoget] = elementValue;
            //    }
            //}
            //else
            //{
            //    try
            //    {
            //        // Add elementtoget and selectedText to the dictionary
            //        dictmapalldata[dictname].Add(elementtoget, selectedText);
            //    }
            //    catch
            //    {
            //      // dictmapalldata[dictname][elementtoget] = selectedText;
            //    }
            //}
            sw1.Stop();
            long elaptime = sw1.ElapsedMilliseconds;
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("THIS IS TIME TAKEN BY FILL DATA DICT " + elaptime);
        }


        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);

        internal delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        internal delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
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
        //new imports end 
        internal static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {

            if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            {
                DateTime sdt = DateTime.Now;
                Stopwatch s1 = new Stopwatch();
                s1.Start();
                //Thread.Sleep(1000);
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                POINT clickPoint = hookStruct.pt;
                IntPtr hwnd = IntPtr.Zero; // WindowFromPoint(clickPoint);
                string buttonautoid = "";
                string buttonname = "";
                string logmessage = "";
                IUIAutomationElement element = null;
                try
                {
                    element = FindElementAtPoint(hookStruct.pt.x, hookStruct.pt.y);
                    buttonautoid = element.CurrentAutomationId;
                    buttonname += element.CurrentName.ToString();
                    if (_filteron)
                    {
                        if (buttonautoid.Contains("ValueListItem") || element.CurrentControlType==UIA_ControlTypeIds.UIA_ListItemControlTypeId)
                        {
                            _filterbasis = buttonname;
                        }
                    }
                    logmessage = "THIS IS NAME of Element :" + buttonname;
                    logmessage += Environment.NewLine + "THIS IS AUTO ID of Element : " + buttonautoid + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("THIS IS NAME of Element :" + buttonname);
                    Console.WriteLine("THIS IS AUTO ID of Element : " + buttonautoid);
                    

                    if (element.CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
                    {
                        Console.WriteLine("BUTTON ID FROM 1 " + element.CurrentAutomationId);
                    }
                    // Console.WriteLine("current element type " + element.Current.AutomationId);
                }
                catch (Exception ex)
                {

                }
                try
                {
                    if (hwnd != null)
                    {
                        string windowTxt = GetWindowText(hwnd);
                       // logmessage = "THIS IS NAME of Element :" + buttonname;
                        logmessage += Environment.NewLine + "THIS IS WINDOW TEXT    " + windowTxt + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        Console.WriteLine("THIS IS WINDOW TEXT    " + windowTxt);
                    }

                }
                catch
                {

                }
                IntPtr foregroundWindow = GetForegroundWindow();
                //GetWindowThreadProcessId(foregroundWindow, out uint processId);
                //Process activeProcess = Process.GetProcessById((int)processId);

                string Foregroundwin = GetWindowText(foregroundWindow);
                Console.WriteLine(Foregroundwin);

                if (Foregroundwin != prevwin)
                {
                    Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
                   
                    Console.WriteLine(Foregroundwin);
                    //logmessage += Environment.NewLine + Foregroundwin;
                    logmessage += Environment.NewLine + Foregroundwin + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                }
                IUIAutomation automation = new CUIAutomation8();

                IUIAutomationElement currentwin = automation.ElementFromHandle(foregroundWindow);
                _currwinglobal = currentwin;
                //WorkflowAutomation_COMLibrary.FetchElements();

                if (Foregroundwin.Contains("Trading Ticket"))
                {
                    Foregroundwin = "Trading Ticket";
                }

                if (_moduletostepname.ContainsKey(Foregroundwin))
                {
                    Stopwatch s3 = new Stopwatch();
                    s3.Start();
                    foreach (string step in _moduletostepname[Foregroundwin])
                    {
                        IUIAutomation automat = new CUIAutomation8();

                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        //currnt datetime
                        foreach (string telement in _stepnametoelement[step])
                        {
                            // get element from current window 
                            // check if grid get the dict for grid and fill data
                            //if not get all data dict and fill data 

                            IUIAutomationCondition getelement = automat.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, telement);
                            IUIAutomationElement targetelement = currentwin.FindFirst(TreeScope.TreeScope_Descendants, getelement);
                            IUIAutomationElement dataGrid = null;
                            if (targetelement != null)
                            {
                                try
                                {
                                    IUIAutomationCondition conditiongrid = automat.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                                    dataGrid = targetelement.FindFirst(TreeScope.TreeScope_Descendants, conditiongrid);
                                }
                                catch (Exception ex) { }
                            }
                            if (targetelement == null)
                            {
                                Console.WriteLine("THIS ELEMENT NOT FOUND ON CURRENT WINDOW " + telement);
                            }
                            //have to think of a better way to find grid type element 
                            else if (targetelement.CurrentControlType.ToString() == "ControlType.Custom" && dataGrid != null)
                            {
                                Console.WriteLine("inside wpf grid ");
                                logmessage += Environment.NewLine + "inside wpf grid " + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                Stopwatch s4 = new Stopwatch();
                                s4.Start();
                                filldictmapwithwpfgrid(new KeyValuePair<string, string>(telement, step), targetelement);
                                s4.Stop();
                                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                logmessage += Environment.NewLine + "DATAGRID WPF FILLED IN DICT" + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                long elapsedMilliseconds3 = s4.ElapsedMilliseconds;

                                Console.WriteLine("Time taken to fill" + telement + "wpf grid     : " + elapsedMilliseconds3);
                                logmessage += Environment.NewLine + "Time taken to fill" + telement + "wpf grid     : " + elapsedMilliseconds3 + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            }
                            //programmatic name problem 
                            else if ((targetelement.CurrentAutomationId.Contains("Grid") || targetelement.CurrentAutomationId.Contains("grd") || targetelement.CurrentAutomationId.Contains("board") || targetelement.CurrentAutomationId.Contains("grid")) && !targetelement.CurrentControlType.Equals(UIA_ControlTypeIds.UIA_CustomControlTypeId))
                            {
                                Stopwatch s5 = new Stopwatch();
                                s5.Start();
                                filldataindictfromgrid(new KeyValuePair<string, string>(targetelement.CurrentAutomationId, step), targetelement);
                                s5.Stop();
                                long elapsedMilliseconds4 = s5.ElapsedMilliseconds;
                                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                Console.WriteLine("time taken to    " + telement + "     : " + elapsedMilliseconds4);
                                logmessage += Environment.NewLine + "time taken to    " + telement + "     : " + elapsedMilliseconds4 + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            }
                            else
                            {
                                Stopwatch s6 = new Stopwatch();
                                s6.Start();
                                filldataindict(new KeyValuePair<string, string>(Foregroundwin, step), targetelement);
                                s6.Stop();
                                long elapsedMilliseconds5 = s6.ElapsedMilliseconds;
                                //Stopwatch exp=new Stopwatch();
                                //exp.Start();
                                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                Console.WriteLine("time taken to fill" + telement + "     : " + elapsedMilliseconds5);
                                logmessage += Environment.NewLine + "time taken to fill" + telement + "     : " + elapsedMilliseconds5 + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                // exp.Stop();
                                //long expMilliseconds6 = exp.ElapsedMilliseconds;
                                // Console.WriteLine("TIME TAKEN BY CONSOLE LOG " + expMilliseconds6);
                                //module wise alldata dictionary
                            }
                            sw.Stop();
                        }
                        long elap = sw.ElapsedMilliseconds;
                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        Console.WriteLine("Time taken for stepname " + step + "     : " + elap);
                        logmessage += Environment.NewLine + "Time taken for stepname " + step + "     : " + elap + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        //element exists on window and action button clicked :true 
                        //timeend
                        //fill data in file for each stepname with time 
                        //ttalldata ,ordersgrid , working
                        //.txt dictionary key txt:symbol - aapl  trade date -
                        Stopwatch s7 = new Stopwatch();
                        s7.Start();
                        //foreach (string telement in _stepnametoelement[step])
                        //{
                        //    Condition getelement = new PropertyCondition(AutomationElement.AutomationIdProperty, telement);
                        //    AutomationElement targetelement = currentwin.FindFirst(TreeScope.Descendants, getelement);
                        //    if (targetelement == null)
                        //    {
                        //        Console.WriteLine("THIS ELEMENT NOT FOUND ON CURRENT WINDOW " + telement);
                        //    }
                        //    else if (targetelement.Current.AutomationId.Contains("Grid") || targetelement.Current.AutomationId.Contains("grd"))
                        //    {
                        //        //get the dictionary for grid 
                        //        //only write that data for which action button is clicked 
                        //        WriteDictmapToTextFile("griddata.txt");
                        //        foreach (var outerKvp in dictmap)
                        //        {
                        //            var outerKey = outerKvp.Key;
                        //            var outerValue = outerKvp.Value;

                        //            //   Console.WriteLine($"gridname: {outerKey.Key}\tstepname: {outerKey.Value}");

                        //            foreach (var innerKvp in outerValue)
                        //            {
                        //                var innerKey = innerKvp.Key;
                        //                var innerValue = innerKvp.Value;

                        //                //  Console.WriteLine($"{innerKey}: {string.Join(", ", innerValue)}");
                        //            }

                        //            // Console.WriteLine(); // Empty line to separate different sections
                        //        }
                        //    }
                        //    else
                        //    {
                        //        WriteDictmapalldataToTextFile("allvalues.txt");
                        //        foreach (var outerKvp in dictmapalldata)
                        //        {
                        //            KeyValuePair<string, string> kvp = outerKvp.Key;
                        //            Dictionary<string, string> innerDict = outerKvp.Value;

                        //            //   Console.WriteLine($"ModuleName: {kvp.Key}\tstepname: {kvp.Value}");

                        //            foreach (var innerKvp in innerDict)
                        //            {
                        //                string string3 = innerKvp.Key;
                        //                string string4 = innerKvp.Value;

                        //                //   Console.WriteLine($"{string3} = {string4}");
                        //            }

                        //            //  Console.WriteLine(); // Add a blank line to separate different entries
                        //        }
                        //        //module wise alldata dictionary
                        //    }
                        //}
                        s7.Stop();
                        long elapsedMilliseconds6 = s7.ElapsedMilliseconds;
                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        Console.WriteLine("Time taken to write data in text file grid and other combined   :  " + elapsedMilliseconds6);
                        logmessage += Environment.NewLine + "Time taken to write data in text file grid and other combined   :  " + elapsedMilliseconds6 + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    }
                    s3.Stop();
                    long elapsedMilliseconds2 = s3.ElapsedMilliseconds;
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("Time taken for whole module" + Foregroundwin + "    : " + elapsedMilliseconds2);
                    logmessage += Environment.NewLine + "Time taken for whole module" + Foregroundwin + "    : " + elapsedMilliseconds2 + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                }
                logmessage += Environment.NewLine + "PRINTING DICT MAP ALL DATA" + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                foreach (var outerKey in dictmapalldata.Keys)
                {
                    var innerDictionary = dictmapalldata[outerKey];
                    Console.WriteLine($"Outer Key: ({outerKey.Key}, {outerKey.Value})");
                    logmessage += Environment.NewLine + $"Outer Key: ({outerKey.Key}, {outerKey.Value})" + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    foreach (var innerKey in innerDictionary.Keys)
                    {
                        var innerValue = innerDictionary[innerKey];
                        Console.WriteLine($"   Inner Key: {innerKey}, Inner Value: {innerValue}");
                        logmessage += Environment.NewLine + $"   Inner Key: {innerKey}, Inner Value: {innerValue}" + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    }
                }
                if (_btntostepname.Keys.Contains(buttonname))
                {
                    //debug
                   
                    //debug
                    Stopwatch s2 = new Stopwatch();
                    s2.Start();

                    List<string> stepname = _btntostepname[buttonname];
                    Stopwatch s8 = new Stopwatch();
                    s8.Start();
                    foreach (var outerKvp in dictmapalldata)
                    {

                        KeyValuePair<string, string> kvp = outerKvp.Key;
                        foreach (string step in stepname)
                        {
                            if (kvp.Value == step)
                            {
                                if (!_stepnameloglist.Contains(step))
                                {
                                    fillstepnamesheet(Foregroundwin, step);
                                    _stepnameloglist.Add(step);
                                }
                                if (_filecreated)
                                {
                                    string dpath = writedatatoexcelstep(_filename, step);
                                    FillExcelSheetWithDictMapstep(dpath, step);

                                }
                                else
                                {
                                    _filename = "demo" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".xlsx";
                                    _filecreated = true;
                                    string dpath = writedatatoexcelstep(_filename, step);
                                    FillExcelSheetWithDictMapstep(dpath, step);
                                }
                                Console.WriteLine("found data for " + buttonname);
                                Console.WriteLine($"ModuleName: {kvp.Key}\tstepname: {kvp.Value}");

                            }
                        }
                    }
                    s8.Stop();
                    long elapsedMilliseconds7 = s8.ElapsedMilliseconds;
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("Time taken to write data in EXCEL file for Other type of data    : " + elapsedMilliseconds7);
                    Stopwatch s9 = new Stopwatch();
                    s9.Start();
                    foreach (var outerKvp in dictmap)
                    {
                        KeyValuePair<string, string> kvp = outerKvp.Key;
                        foreach (string step in stepname)
                        {
                            if (kvp.Value == step)
                            {
                                if (!_stepnameloglist.Contains(step))
                                {
                                    fillstepnamesheet(Foregroundwin, step);
                                    _stepnameloglist.Add(step);
                                }
                                //fillstepnamesheet(Foregroundwin, step);

                                foreach (string ele in _stepnametoelement[step])
                                {
                                    //if current win contains the elements mentioned in stepname
                                    IUIAutomationCondition getelement = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, ele);
                                    IUIAutomationElement targetelement = currentwin.FindFirst(TreeScope.TreeScope_Descendants, getelement);
                                    if (targetelement != null)
                                    {
                                        if (_filecreated)
                                        {
                                            string dpath = writedatatoexcelstep(_filename, step);
                                            FillExcelSheetWithDictMapstep(dpath, step);
                                        }
                                        else
                                        {
                                            _filename = "demo" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".xlsx";
                                            _filecreated = true;
                                            string dpath = writedatatoexcelstep(_filename, step);
                                            FillExcelSheetWithDictMapstep(dpath, step);
                                        }
                                        Console.WriteLine("found data for " + buttonname);
                                        Console.WriteLine($"ModuleName: {kvp.Key}\tstepname: {kvp.Value}");
                                        logmessage += Environment.NewLine + "found data for " + buttonname+ $"ModuleName: {kvp.Key}\tstepname: {kvp.Value}" + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                    }
                                }

                            }
                        }
                    }
                    s9.Stop();
                    long elapsedMilliseconds8 = s8.ElapsedMilliseconds;
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("Time taken to write data to Excel sheet     : " + elapsedMilliseconds8);
                    logmessage += Environment.NewLine + "Time taken to write data to Excel sheet     : " + elapsedMilliseconds8 + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    s2.Stop();
                    long elapsedMilliseconds1 = s2.ElapsedMilliseconds;
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Console.WriteLine("Time taken after pressing action button     : " + elapsedMilliseconds1);
                    logmessage += Environment.NewLine + "Time taken after pressing action button     : " + elapsedMilliseconds1 + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                }

                //new method 


                //if foreground windows contains trading ticket change it to trading ticket apple-trading ticket ->trading ticket
                if (Foregroundwin.Contains("Trading Ticket"))
                {
                    Foregroundwin = "Trading Ticket";
                }

                s1.Stop();
                long elapsedMilliseconds = s1.ElapsedMilliseconds;
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Console.WriteLine("time taken to complete one click     : " + elapsedMilliseconds);
                logmessage += Environment.NewLine + "time taken to complete one click     : " + elapsedMilliseconds + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime edt = DateTime.Now;
                TimeSpan timeSpan = edt - sdt;
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Console.WriteLine("TOTAL TIME ELAPSED     : " + timeSpan.TotalMilliseconds);
                logmessage += Environment.NewLine + "TOTAL TIME ELAPSED     : " + timeSpan.TotalMilliseconds + " TIME :  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                prevwin = Foregroundwin;
                Log(logmessage);
            }


            return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);


        }
        internal static void filldictmapwithwpfgrid(KeyValuePair<string, string> dictname, IUIAutomationElement pelement)
        {
            if (dictmap.TryGetValue(dictname, out var targetDict))
            {
                // Dictionary already exists, so clear the specific dictionary entry
                dictmap.Remove(dictname);
            }
            targetDict = new Dictionary<string, List<string>>();
            dictmap[dictname] = targetDict;
            IUIAutomation automation = new CUIAutomation8();

            IUIAutomationCondition conditiongrid = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
            IUIAutomationElement dataGrid = pelement.FindFirst(TreeScope.TreeScope_Descendants, conditiongrid);
            IUIAutomationCondition conditiondataitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);

            var dataItems = dataGrid.FindAll(TreeScope.TreeScope_Descendants, conditiondataitem);

            // Process each row and map cell values to their corresponding column headers
            for (int i = 0; i < dataItems.Length; i++)
            {
                IUIAutomationElement dataItem = dataItems.GetElement(i);
                // Find all cells in the current row
                IUIAutomationCondition conditioncell = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "cell");

                var cells = dataItem.FindAll(TreeScope.TreeScope_Children, conditioncell);

                for (int j = 0; j < cells.Length; j++)
                {
                    IUIAutomationElement cell = cells.GetElement(j);
                    try
                    {
                        string value = null;
                        object patternprovider;
                        if (cell.GetCurrentPattern(UIA_PatternIds.UIA_TableItemPatternId) != null)
                        {
                            patternprovider = cell.GetCurrentPattern(UIA_PatternIds.UIA_TableItemPatternId);
                            IUIAutomationTableItemPattern selectionpatternprovider = patternprovider as IUIAutomationTableItemPattern;
                            //value = selectionpatternprovider.Current.ToString();
                            value = selectionpatternprovider.GetCurrentColumnHeaderItems().GetElement(0).CurrentName;
                            //  value = selectionpatternprovider.Current.GetColumnHeaderItems()[0].Current.Name;
                            //Console.WriteLine(cell.Current.Name + ": " + value);

                            if (dictmap[dictname].ContainsKey(value))
                            {
                                dictmap[dictname][value].Add(cell.CurrentName);
                            }
                            else
                            {
                                dictmap[dictname].Add(value, new List<string> { cell.CurrentName });
                            }
                            // columnToCellValuesMap[value].Add(cell.Current.Name);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
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
            worksheet.Cells[rownum, 4].Value = modulename;
            worksheet.Cells[rownum, 5].Value = stepnamee;
            worksheet.Cells[rownum, 6].Value = gstring;
            worksheet.Cells[rownum, 7].Value = stepnamee;

            package.Save();
            package.Dispose();
            rownum++;

        }

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

            string destinationFilePath = Path.Combine(sourceFolderPath, filename);
            FileInfo destinationFile = new FileInfo(destinationFilePath);
            ExcelPackage destinationPackage = new ExcelPackage(destinationFile);
            foreach (var outerKvp in dictmapalldata)
            {
                var outerKey = outerKvp.Key;
                if (stepname == outerKey.Value)
                {
                    string sheetName = outerKey.Value;
                    ExcelWorksheet sourceSheet = sourcePackage.Workbook.Worksheets[sheetName];
                    try
                    {
                        ExcelWorksheet destinationSheet = destinationPackage.Workbook.Worksheets.Add(sheetName, sourceSheet);
                    }
                    catch
                    {

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
                        ExcelWorksheet destinationSheet = destinationPackage.Workbook.Worksheets.Add(sheetName, sourceSheet);
                    }
                    catch
                    {

                    }
                    destinationPackage.Save();
                }
            }
            try
            {
                destinationPackage.Workbook.Worksheets.Delete("Sheet1");
            }
            catch
            {

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
                catch
                {

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
                catch
                {

                }
                destinationPackage.Save();
            }
            try
            {
                destinationPackage.Workbook.Worksheets.Delete("Sheet1");
            }
            catch
            {

            }
            destinationPackage.Save();
            sourcePackage.Dispose();
            destinationPackage.Dispose();

            return destinationFilePath;
        }
        static private void filldatainsidesheet()
        {

            // sheet path 
            // sheet load 
            // sub sheet column headers at row 5 
            // traverse in dictionary 
            // navigate to subsheet through stepname 
            // go to inner dictionary which contains data 
            // match columns of sheet with keys of inner dictionary and fill data 

        }

        private static void FillExcelSheetWithDictMapstep(string filePath, string stepname)
        {
            // Load the existing Excel file
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var fileInfo = new FileInfo(filePath);
            using (var package = new ExcelPackage(fileInfo))
            {
                foreach (var outerKvp in dictmap)
                {
                    var outerKeyPair = outerKvp.Key;
                    if (stepname == outerKeyPair.Value)
                    {
                        var innerDict = outerKvp.Value;

                        // Get or create the subsheet based on string2 (outerKeyPair.Value)
                        var worksheet = package.Workbook.Worksheets[outerKeyPair.Value] ?? package.Workbook.Worksheets.Add(outerKeyPair.Value);
                        // Console.Write(worksheet.Name.ToString());
                        //var worksheet = xlpackage.wor
                        int startingRow = 5;


                        foreach (var innerKvp in innerDict)
                        {
                            var columnHeader = "";
                            //
                            if (mappingCorrection.Keys.Contains(innerKvp.Key))
                            {
                                columnHeader = mappingCorrection[innerKvp.Key];
                            }
                            else
                            {
                                columnHeader = innerKvp.Key;
                            }
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
                                    worksheet.Cells[startingRow + 1 + i, column.Start.Column].Value = dataList[i];
                                    //Console.Write(column.Start.Column.ToString() + "......................inserted in cell");
                                }
                            }
                        }
                    }
                }

                // Process dictmapalldata
                foreach (var outerKvp in dictmapalldata)
                {
                    var outerKeyPair = outerKvp.Key;
                    if (stepname == outerKeyPair.Value)
                    {
                        var innerDict = outerKvp.Value;

                        // Get or create the subsheet based on string2 (outerKeyPair.Value)
                        var worksheet = package.Workbook.Worksheets[outerKeyPair.Value] ?? package.Workbook.Worksheets.Add(outerKeyPair.Value);
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
                            var columnHeader = "";
                            //
                            if (mappingCorrection.Keys.Contains(innerKvp.Key))
                            {
                                columnHeader = mappingCorrection[innerKvp.Key];
                            }
                            else
                            {
                                columnHeader = innerKvp.Key;
                            }
                            // Find the matching column based on the header
                            var column = worksheet.Cells[startingRow, 1, startingRow, worksheet.Dimension.End.Column].FirstOrDefault(c =>
                                c.Text == columnHeader);

                            if (column != null)
                            {
                                var dataValue = innerKvp.Value;

                                // Fill the data in the corresponding cells
                                // worksheet.Cells[startingRow + 1, column.Start.Column].Value = dataValue;
                                worksheet.Cells[lastFilledRow + 1, column.Start.Column].Value = dataValue;
                            }
                        }
                    }
                }

                // Save the changes back to the file
                package.Save();
            }

        }
        private static void FillExcelSheetWithDictMap(string filePath)
        {
            // Load the existing Excel file
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var fileInfo = new FileInfo(filePath);
            using (var package = new ExcelPackage(fileInfo))
            {
                foreach (var outerKvp in dictmap)
                {
                    var outerKeyPair = outerKvp.Key;
                    var innerDict = outerKvp.Value;

                    // Get or create the subsheet based on string2 (outerKeyPair.Value)
                    var worksheet = package.Workbook.Worksheets[outerKeyPair.Value] ?? package.Workbook.Worksheets.Add(outerKeyPair.Value);
                    // Console.Write(worksheet.Name.ToString());

                    int startingRow = 5;


                    foreach (var innerKvp in innerDict)
                    {
                        var columnHeader = innerKvp.Key;

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
                                worksheet.Cells[startingRow + 1 + i, column.Start.Column].Value = dataList[i];
                                //Console.Write(column.Start.Column.ToString() + "......................inserted in cell");
                            }
                        }
                    }
                }

                // Process dictmapalldata
                foreach (var outerKvp in dictmapalldata)
                {
                    var outerKeyPair = outerKvp.Key;
                    var innerDict = outerKvp.Value;

                    // Get or create the subsheet based on string2 (outerKeyPair.Value)
                    var worksheet = package.Workbook.Worksheets[outerKeyPair.Value] ?? package.Workbook.Worksheets.Add(outerKeyPair.Value);
                    //   Console.Write(worksheet.Name.ToString());

                    int startingRow = 5; // Start filling data from row 6 in the subsheet

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
                        var columnHeader = innerKvp.Key;

                        // Find the matching column based on the header
                        var column = worksheet.Cells[startingRow, 1, startingRow, worksheet.Dimension.End.Column].FirstOrDefault(c =>
                            c.Text == columnHeader);

                        if (column != null)
                        {
                            var dataValue = innerKvp.Value;

                            // Fill the data in the corresponding cells
                            worksheet.Cells[startingRow + 1, column.Start.Column].Value = dataValue;
                        }
                    }
                }

                // Save the changes back to the file
                package.Save();
            }

        }
        //static private void filldatafromgrid(AutomationElement gridelement)
        //{
        //    Condition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tree);
        //    AutomationElement grid = gridelement.FindFirst(System.Windows.Automation.TreeScope.Descendants, condition1);
        //    Condition condition = new PropertyCondition(AutomationElement.AutomationIdProperty, "[Editor] Edit Area");
        //    AutomationElementCollection rowelements = grid.FindAll(System.Windows.Automation.TreeScope.Descendants, condition);


        //    Console.WriteLine("number of elements in grid " + rowelements.Count);
        //    // Console.WriteLine("number of columns in grid " + columnhead.Count);

        //    if (rowelements != null)
        //        Console.WriteLine("INSIDE GRID DATA WRITING");
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

        //    Console.WriteLine("Data written successfully to Google Sheet for step file.");

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

        //static private string getcurrentmodule(AutomationElement currwin, string foregroundwin)
        //{
        //    switch (foregroundwin)
        //    {
        //        case "Trading Ticket":
        //            return "tradingticket";
        //            break;
        //        case "Blotter":
        //            return "blotter";
        //            break;
        //        case "Close Trade":
        //            return "closing";
        //            break;
        //        case "Allocation":
        //            return "allocation";
        //            break;
        //        case "Portfolio Management":
        //            return "pm";
        //            break;
        //        default:
        //            return "alldata";

        //    }
        //}

        //static private void getdatafromgrid(AutomationElement currgrid, string foregroundwin, string prevwin)
        //{
        //    Condition condition = new PropertyCondition(AutomationElement.AutomationIdProperty, "[Editor] Edit Area");
        //    AutomationElementCollection rowelements = currgrid.FindAll(System.Windows.Automation.TreeScope.Descendants, condition);
        //    Condition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.HeaderItem);

        //    AutomationElementCollection columnhead = currgrid.FindAll(System.Windows.Automation.TreeScope.Descendants, condition1);
        //    Console.WriteLine("number of elements in grid " + rowelements.Count);
        //    Console.WriteLine("number of columns in grid " + columnhead.Count);


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
        //                    //Console.WriteLine("INFORMATION OF ROW  PARENT ");
        //                    //Console.WriteLine(parentElement);
        //                    Console.WriteLine("this is parent name :" + parentElement.Current.Name);
        //                    writer.Write(parentElement.Current.Name + "=");
        //                }
        //                string eleme = "";
        //                ValuePattern valuePattern = e.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
        //                if (valuePattern != null)
        //                {
        //                    eleme = valuePattern.Current.Value;
        //                    writer.Write(eleme + "^");
        //                    Console.WriteLine("this is the element :" + eleme);
        //                }
        //                writer.WriteLine();
        //                count++;
        //            }
        //            //Console.WriteLine(count);
        //            // parentset.Clear();


        //            writer.Close();
        //            writer.Dispose();



        //        }

        //    }
        //}

        //static private void getparent(AutomationElement e)
        //{
        //    AutomationElement parentElement = TreeWalker.ControlViewWalker.GetParent(e);
        //    if (parentElement != null)
        //    {
        //        Console.WriteLine("first parent   " + parentElement.Current.Name);
        //        AutomationElement parentElement1 = TreeWalker.ControlViewWalker.GetParent(parentElement);
        //        if (parentElement1 != null)
        //        {
        //            Console.WriteLine("second parent " + parentElement1.Current.Name);
        //            AutomationElement parentElement2 = TreeWalker.ControlViewWalker.GetParent(parentElement1);
        //            if (parentElement2 != null)
        //            {
        //                Console.WriteLine("third parent " + parentElement2.Current.AutomationId);
        //                AutomationElement parentElement3 = TreeWalker.ControlViewWalker.GetParent(parentElement2);
        //                if (parentElement3 != null)
        //                {
        //                    Console.WriteLine("fourth parent " + parentElement3.Current.AutomationId);
        //                    AutomationElement parentElement4 = TreeWalker.ControlViewWalker.GetParent(parentElement3);
        //                    if (parentElement4 != null)
        //                    {
        //                        Console.WriteLine("fifth parent " + parentElement4.Current.AutomationId);
        //                        Console.WriteLine(parentElement4.Current.AutomationId);
        //                        Console.WriteLine(parentElement4.Current.ControlType.ProgrammaticName);
        //                        AutomationElement parentElement5 = TreeWalker.ControlViewWalker.GetParent(parentElement4);
        //                        if (parentElement5 != null)
        //                        {
        //                            Console.WriteLine("sixth parent " + parentElement5.Current.AutomationId);
        //                            parentset.Add(parentElement5.Current.AutomationId);
        //                            AutomationElement parentElement6 = TreeWalker.ControlViewWalker.GetParent(parentElement5);
        //                            if (parentElement6 != null)
        //                            {
        //                                Console.WriteLine("seventh parent " + parentElement6.Current.AutomationId);
        //                                Console.WriteLine(parentElement6.Current.Name);

        //                                AutomationElement parentElement7 = TreeWalker.ControlViewWalker.GetParent(parentElement6);
        //                                if (parentElement7 != null)
        //                                {
        //                                    Console.WriteLine("eighth parent " + parentElement7.Current.AutomationId);

        //                                }
        //                            }
        //                        }
        //                    }

        //                }

        //            }

        //        }
        //    }
        //}
        //static private List<List<string>> GetGridData(AutomationElement gridElement)
        //{
        //    var data = new List<List<string>>();

        //    // Retrieve the grid items or cells within the grid element
        //    AutomationElementCollection gridItems = gridElement.FindAll(System.Windows.Automation.TreeScope.Children, Condition.TrueCondition);

        //    foreach (AutomationElement item in gridItems)
        //    {
        //        Console.WriteLine(item.Current.Name);
        //    }

        //    foreach (AutomationElement item in gridItems)
        //    {
        //        var rowData = new List<string>();

        //        // Retrieve the data from each cell within the grid item
        //        AutomationElementCollection cellItems = item.FindAll(System.Windows.Automation.TreeScope.Children, Condition.TrueCondition);
        //        foreach (AutomationElement cellItem in cellItems)
        //        {
        //            string cellValue = cellItem.Current.Name;
        //            rowData.Add(cellValue);
        //        }

        //        data.Add(rowData);
        //    }

        //    return data;
        //}






        //static AutomationElement FindElementByAutomationId(string automationId)
        //{
        //    Condition condition = new PropertyCondition(AutomationElement.AutomationIdProperty, automationId);
        //    AutomationElement element = AutomationElement.RootElement.FindFirst(System.Windows.Automation.TreeScope.Descendants, condition);
        //    return element;
        //}





        private static void OnProcessExited(object sender, EventArgs e)
        {
            Process process = (Process)sender;
            Console.WriteLine("Window closed: " + process.MainWindowTitle);
            process.Exited -= OnProcessExited;
        }


        public class ProcessEventArgs : EventArgs
        {
            public Process Process { get; }

            public ProcessEventArgs(Process process)
            {
                Process = process;
            }
        }



        internal static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                KBDLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
                int keyCode = (int)hookStruct.vkCode;
                //Console.WriteLine(keyCode);
                //Console.WriteLine("Pressed Key: " + ((Keys)keyCode).ToString());
                //if (keyCode == 88)
                //{
                //    string filepath=writedatatoexcel();
                //    FillExcelSheetWithDictMap(filepath);

                //}

            }

            return CallNextHookEx(_keyboardHookID, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

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
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    }
}
