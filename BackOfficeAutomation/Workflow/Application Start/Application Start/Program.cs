using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using WindowsInput;
using System.Windows.Forms;
using UIAutomationClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using static System.Windows.Forms.LinkLabel;


class Program
{
    public static DateTime currentDate = DateTime.Now;
    public static string currentDir = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
    public static string logFolderPath = "StartingLogs";
    public static string logFilePath = Path.Combine(logFolderPath, $"Starting_Log_{currentDate.ToString("yyyyMMdd")}.txt");
    public static DateTime initialTime;
    public static DateTime finalTime;
    public static int exitcode = 0;
    static IUIAutomationElement _currwinglobal = null;
    public static string logMsg = null;

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
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr GetFocus();
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
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
    static int configurabletime=10;
    static void Main(string[] args)
    {
        //string command = "";
        //if(args.Length > 0)
        //{
        //    command = args[0];
        //    configurabletime = int.Parse(command);
        //}
        if (!Directory.Exists(logFolderPath))
        {
            Directory.CreateDirectory(logFolderPath);
        }

        initialTime = DateTime.Now;

        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        string currentUser = "Current User : " + identity?.Name;

        // Get the local IP address of the machine
        string localIp = "IP : " + GetLocalIPAddress();

        CreateLogs("", 0);
        CreateLogs(currentUser, 0);
        CreateLogs(localIp, 0);
        CreateLogs("", 0);

        //readconfig and initialize configurabletime


        string[] applicationFullPaths = ReadApplicationFullPathsFromFile("ApplicationListToStart.txt");

        if (applicationFullPaths == null)
        {
            Console.WriteLine("The Given file path is Not valid for ApplicationListToStart.txt !!");
            string msg = "The Given file path is Not valid for ApplicationListToStart.txt !!";
            CreateLogs(msg);
            exitcode = 15;
            return;
        }

        int isNotStarted = 0;
        int started = 0;
        bool runMdDU = false;
        foreach (string fullPath in applicationFullPaths)
        {
            try
            {
                bool isApplicationStart = false;
                if (fullPath.Substring(fullPath.LastIndexOf('\\') + 1).Equals("LiveFeedUtility.exe"))
                {
                    isApplicationStart = StartMDU(fullPath);
                    runMdDU = true;
                    Thread.Sleep(1000);
                }
                else
                {
                    isApplicationStart = StartApplication(fullPath);
                    if(runMdDU && !fullPath.Contains("UI.exe"))
                    ReadConsoleWindow(fullPath);
                }

                if (isApplicationStart == false)
                {
                    isNotStarted++;
                    Console.WriteLine($"{fullPath} Not Started!!");
                    string msg = $"{fullPath} Not Started!!";
                    CreateLogs(msg);
                    exitcode = 2;
                }
                else
                {
                    started++;
                }
            }
            catch { }
        }

        finalTime = DateTime.Now;

        Console.WriteLine("");
        CreateLogs("", 0);
        Console.WriteLine("[" + started + "]     Applications Started SuccessFully!!");
        CreateLogs("[" + started + "]     Applications Started SuccessFully!!", 0);

        TimeSpan timeDifference = finalTime - initialTime;
        Console.WriteLine("");
        string totalTimeTaken = timeDifference.Minutes + " Minutes " + timeDifference.Seconds + " Seconds " + timeDifference.Milliseconds + " Milliseconds ";
        
        CreateLogs("Total Time Taken : " + totalTimeTaken, 0);
        string msg1 = "*****************************************************************************************************************************";
        CreateLogs(msg1, 0);

        string errorCode = "ExitCode : " + exitcode + "Please check the Exit Code File for more information.";
        CreateLogs(errorCode, 0);
        Environment.Exit(exitcode);
    }

    static bool StartApplication(string applicationPath)
    {
        try
        {
            //start "" "E:\Enterprise v2.12\Enterprise\Application\Prana.PricingAnalysisModels\Prana.PricingService2Host\bin\Debug\Prana.PricingService2Host.exe"
            string a = $"/c start \"\" \"{applicationPath}\"";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c start \"\" \"{applicationPath}\"",
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(applicationPath),
            };

            Process cmdProcess = new Process
            {
                StartInfo = psi,
            };

            cmdProcess.Start();
            //process = Process.Start(applicationPath);

            Console.WriteLine(applicationPath + " is Started Successfully!!");
            string msg = applicationPath + " is Started Successfully!!";
            CreateLogs(msg);
            return true;
        }

        catch (Exception ex)
        {
            //Console.WriteLine($"Error starting application: {ex.Message}");
            //string msg = $"Error starting application: {ex.Message}";
            logMsg = "Error Starting application:"+ex.Message+"\n" + ex.StackTrace;
            CreateLogs(logMsg);
            exitcode = 2;
            return false;
        }

        return false;
    }

    public static string GetLocalIPAddress()
    {
        string localIp = "N/A";

        try
        {
            // Get the host name of the local machine
            string hostName = Dns.GetHostName();

            // Get the IP addresses associated with the host
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            // Find the first IPv4 address (ignore IPv6 for simplicity)
            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIp = address.ToString();
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting local IP address: {ex.Message}");
        }

        return localIp;
    }

    static string[] ReadApplicationFullPathsFromFile(string filePath)
    {
        string[] fullPaths = null;
        try
        {
            if (!File.Exists(filePath))
            {
                exitcode = 15;
                return null;
            }


            string[] lines = File.ReadAllLines(filePath);
            try
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] checkconfigtime = lines[i].Split(',');
                    if (checkconfigtime.Length == 2)
                    {
                        lines[i] = checkconfigtime[0];
                        configurabletime = int.Parse(checkconfigtime[1]);

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("unable ot set configurable time");
            }
            if (lines.Length == 0)
            {
                return new string[0];
            }

            string mainFolderPath = lines[0];

            fullPaths = new string[lines.Length - 1];
            for (int i = 1; i < lines.Length; i++)
            {
                fullPaths[i - 1] = Path.Combine(mainFolderPath, lines[i]);
            }
        }
        catch(Exception ex){
            Console.WriteLine(ex.StackTrace);
        }

        return fullPaths;
    }

    public static void CreateLogs(string logMessage, int dateCheck = 1)
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
                }
            }
        }
        catch { }

    }

    public static bool ReadMessages(IUIAutomation automation, IUIAutomationElement currentwin)
    {
        bool isMDUStarted = false;
        try
        {
            IUIAutomationCondition condList = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "messageList");
            IUIAutomationElement listItem = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condList);
            IUIAutomationElementArray listItem1 = null;
            //if (listItem != null)
            while (!isMDUStarted)
            {
                try
                {
                    listItem1 = listItem.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
                    if (listItem1 != null)
                    {
                        for (int i = 0; i < listItem1.Length; i++)
                        {
                            IUIAutomationElement childElement = listItem1.GetElement(i);

                            if (childElement.CurrentName.Contains("Initialization Started"))
                            {
                                logMsg = childElement.CurrentName;
                                CreateLogs(logMsg);
                                isMDUStarted = true;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }
        catch (Exception ex)
        {
            exitcode = 23;
            logMsg = "Getting exception "+ex.Message + " while reading messages from MDU UI.\n" + ex.StackTrace;
            CreateLogs(logMsg);
        }
        return isMDUStarted;
    }

    //To Do:we can make this method as common in all 
    public static void BringToForeground(string currwin, ref IUIAutomationElement currentwin, string exePath)
    {
        InputSimulator simulator = new InputSimulator();

        //if (currwin.ToLower() == "importpositions" || currwin.ToLower() == "importposition")
        //{
        //    simulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LMENU);
        //    simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
        //    simulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LMENU);
        //    return;
        //}
        Console.WriteLine("Inside Bring To ForeGround" + currwin);
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
                CreateLogs(currentwin.CurrentName);
                object patternprovider;
                // Calculator is already open, bring it to the foreground
                if (element.GetCurrentPattern(UIA_PatternIds.UIA_WindowPatternId) != null)
                {
                    patternprovider = element.GetCurrentPattern(UIA_PatternIds.UIA_WindowPatternId);
                    IUIAutomationWindowPattern windowpatternprovider = patternprovider as IUIAutomationWindowPattern;
                    WindowVisualState calstate = windowpatternprovider.CurrentWindowVisualState;

                    Console.WriteLine(windowpatternprovider.CurrentIsTopmost);
                    Console.WriteLine(windowpatternprovider.CurrentWindowInteractionState);

                    IntPtr currentForegroundWindowHandle = GetForegroundWindow();
                    IUIAutomationElement currentForegroundwindow = automation.ElementFromHandle(currentForegroundWindowHandle);


                    while (currentForegroundwindow.CurrentName != currwin)
                    {
                        Console.WriteLine(windowpatternprovider.CurrentIsTopmost);
                        Console.WriteLine(windowpatternprovider.CurrentWindowInteractionState);
                        Thread.Sleep(2000);
                        if (windowpatternprovider.CurrentWindowInteractionState == WindowInteractionState.WindowInteractionState_BlockedByModalWindow)
                        {
                            break;
                        }
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
                        currentForegroundWindowHandle = GetForegroundWindow();
                        currentForegroundwindow = automation.ElementFromHandle(currentForegroundWindowHandle);


                    }
                    //if (currentForegroundwindow.CurrentName == currwin) return;



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

        }
        catch (Exception ex)
        {
            CreateLogs(ex.Message);
            CreateLogs(ex.StackTrace);
            CreateLogs("exception due to process");
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
                
                object patternprovider;
                if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                {
                    patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                  

                    IUIAutomationInvokePattern selectionpatternprovider = patternprovider as IUIAutomationInvokePattern;
                    //Thread modalThread = new Thread(HandleModalDialog);
                    //modalThread.Start();
                    selectionpatternprovider.Invoke();
                    //modalThread.Join();


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
            exitcode = 23;
            string logMsg = "Getting exception " + ex.Message + " while clicking element " + element.CurrentName + ".\nExited with code : " + exitcode + "\n" + ex.StackTrace;
            CreateLogs(logMsg);
            Console.WriteLine("Exited with code : " + exitcode);
            Environment.Exit(exitcode);
        }
    }

    public static bool StartMDU(string exePath)
    {
        int i = 0;
        StartApplication(exePath);
        Thread.Sleep(1000);
        IUIAutomation automation = new CUIAutomation8();
        IUIAutomationElement currentwin = null;
        while (currentwin == null && i < configurabletime)
        {
            BringToForeground("Market Data Utility", ref currentwin, exePath);
            BringToForeground("Market Data Service", ref currentwin, exePath);
            
            Thread.Sleep(500);
            i++;
        }
        //i = 0;
        //while (currentwin == null && i < 10)
        //{
        //    BringToForeground("Market Data Utility", ref automation, ref currentwin);
        //    Thread.Sleep(500);
        //    i++;
        //}

        try
        {
            if(currentwin == null)
                Console.WriteLine("Not able to find the window.");
            if ((currentwin.CurrentName == "Market Data Service") || (currentwin.CurrentName == "Market Data Utility"))
            {
                //IUIAutomationCondition condtitlebar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "TitleBar");
                //IUIAutomationElement titlebar = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condtitlebar);
                //InputSimulator inputSimulator = new InputSimulator(); 
                IUIAutomationCondition condBtn = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "StartButton");
                IUIAutomationElement targetelement = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condBtn);
                //tagPOINT p;
                //if (titlebar != null)
                //{
                //    titlebar.GetClickablePoint(out p);
                //    System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)p.x, (int)p.y);
                //    inputSimulator.Mouse.LeftButtonClick();
                //}
                // ClickElement(btn, "Left Mouse Button Clicked");
                //replayaction(btn, "");
                if (targetelement.CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
                {
                    BringToForeground("Market Data Utility", ref currentwin, exePath);
                    BringToForeground("Market Data Service", ref currentwin, exePath);

                    object patternprovider;
                    if (targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null)
                    {
                        patternprovider = targetelement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);


                        IUIAutomationInvokePattern selectionpatternprovider = patternprovider as IUIAutomationInvokePattern;
                       
                        selectionpatternprovider.Invoke();                       


                    }
                   
                }
            }
            else
            {
                logMsg = "Not able to find market data service UI. Current found window is " + currentwin.CurrentName;
                CreateLogs(logMsg);
            }
        }
        catch (Exception ex)
        {
            //exitcode = 23;
            logMsg = "Getting exception " + ex.Message + " in method StartMDU \n" + ex.StackTrace;
            CreateLogs(logMsg);
        }
        return ReadMessages(automation,currentwin);
    }
    public static bool ReadConsoleWindow(string exePath)
    {
        int i = 0;
        IUIAutomation automation = new CUIAutomation8();
        IUIAutomationElement currentwin = null;
        string windowName = GetWindowName(exePath);
        while (currentwin == null && i < configurabletime)
        {
            BringToForeground(windowName, ref currentwin, exePath);
            Thread.Sleep(500);
            i++;
        }
        try
        {
            if ((currentwin.CurrentName.Contains(windowName)))
            {
                IUIAutomationCondition condText = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "Text Area");
                IUIAutomationElement documentElement = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condText);

                if (documentElement != null)
                {
                    // Retrieve the text pattern for the element
                    object patternObj = null;
                    try
                    {
                        patternObj = documentElement.GetCurrentPattern(UIA_PatternIds.UIA_TextPatternId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception occurred while retrieving text pattern: " + ex.Message);
                    }

                    if (patternObj != null)
                    {
                        // Document supports Text pattern, so retrieve the text content
                        IUIAutomationTextPattern textPattern = patternObj as IUIAutomationTextPattern;
                        if (textPattern != null)
                        {
                            string consoleText = string.Empty;
                            // Get the text range of the document
                            while (!consoleText.Contains("Successfully hosted service."))
                            {
                                IUIAutomationTextRange documentRange = textPattern.DocumentRange;
                                // Retrieve the text from the document range
                                consoleText = documentRange.GetText(-1); // -1 indicates the entire document
                                Thread.Sleep(1000);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Document does not support Text pattern.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logMsg = "Getting exception " + ex.Message + " in method ReadConsoleWindow \n" + ex.StackTrace;
            CreateLogs(logMsg);
        }
        return true;
    }

    public static string GetWindowName(string exePath)
    {
        string windowName = string.Empty;
        try
        {
            exePath = exePath.Substring(exePath.LastIndexOf('\\') + 1);
            switch (exePath)
            {
                case "Prana.PricingService2Host.exe":
                    windowName = "Prana Pricing Service";
                    break;
                case "Prana.PricingService2UI.exe":
                    windowName = "Prana PricingService2 UI";
                    break;
                case "Prana.TradeServiceHost.exe":
                    windowName = "Prana Trade Service";
                    break;
                case "Prana.TradeServiceUI.exe":
                    windowName = "Prana TradeService UI";
                    break;
                case "Prana.ExpnlServiceHost.exe":
                    windowName = "Prana Expnl Service";
                    break;
                case "Prana.ExpnlServiceUI.exe":
                    windowName = "Prana ExpnlService UI";
                    break;
            }
        }
        catch (Exception ex)
        {
            logMsg = "Getting exception " + ex.Message + " in method GetWindowName \n" + ex.StackTrace;
            CreateLogs(logMsg);
        }
        return windowName;
    }

    //static IUIAutomationCondition CreatePropertyConditionWithRegex(IUIAutomation automation, int propertyId, string pattern)
    //{
    //    // Create a property condition with regular expression
    //    return new UIAutomationRegexCondition(automation, propertyId, pattern);
    //}

    //class UIAutomationRegexCondition : IUIAutomationCondition
    //{
    //    private readonly Regex _regex;
    //    private readonly int _propertyId;

    //    public UIAutomationRegexCondition(IUIAutomation automation, int propertyId, string pattern)
    //    {
    //        _propertyId = propertyId;
    //        // Create a regular expression
    //        _regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    //    }

    //    public int AddRef()
    //    {
    //        return 1; // No-op
    //    }

    //    public int Release()
    //    {
    //        return 1; // No-op
    //    }

    //    public int Compare(IUIAutomationCondition pCondition)
    //    {
    //        return 0; // Not implemented
    //    }

    //    public bool Matches(IUIAutomationElement pElement)
    //    {
    //        // Get the value of the specified property
    //        string propertyValue = pElement.GetCurrentPropertyValue(_propertyId).ToString();

    //        // Check if the property value matches the regex pattern
    //        return _regex.IsMatch(propertyValue);
    //    }
    //}
}