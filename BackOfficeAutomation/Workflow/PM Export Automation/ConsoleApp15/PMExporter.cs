using Microsoft.VisualBasic.FileIO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UIAutomationClient;
using WindowsInput;
using System.Management;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using MimeKit;
using System.Net.Mail;
using System.Configuration;
using System.Drawing.Imaging;
using System.Drawing;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Microsoft.Office.Interop.Excel;
using System.Windows.Interop;
using System.Windows.Forms;
using static NPOI.HSSF.Util.HSSFColor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

using System.Security.Cryptography;
using System.Xml.Linq;
//using static Org.BouncyCastle.Math.EC.ECCurve;

namespace PMExporter
{
    internal class PMExporter
    {
        public static DateTime initialTime = DateTime.Now;
        public static DateTime finalTime;
        public static string logFolderPath = Directory.GetCurrentDirectory() + "\\Logs";
        public static string logFilePath = logFolderPath + "\\PM_Logs_"+DateTime.Now.ToString("yyyyMMdd") +".txt";
        static IUIAutomationElement _currwinglobal = null;
        public static string logMsg = string.Empty;
        public static int exitcode = -1;
        public static string clientName = String.Empty;
        public static string destinationPath = ConfigurationManager.AppSettings["DestinationPath"];
        //public static string encryptedUsername = ConfigurationManager.AppSettings["username"];
        //public static string encryptedPassword = ConfigurationManager.AppSettings["password"];

        static string[] Scopes = {
             GmailService.Scope.MailGoogleCom
        };
        static string ApplicationName = "UAT Automation";

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out System.Drawing.Rectangle lpRect);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BlockInput(bool fBlockIt);
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(ref NETRESOURCE netResource, string password, string username, int flags);

        [System.Runtime.InteropServices.DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags, bool force);

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct NETRESOURCE
        {
            public int dwScope;
            public int dwType;
            public int dwDisplayType;
            public int dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }

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
                exitcode = 9;
                logMsg = "Getting exception " + ex.Message + " while maximizing " + currentwin.CurrentName + " window.\n Exited with code : " + exitcode + "\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
        }

        public static void MinimizeWindow(string currwin, ref IUIAutomationElement currentwin)
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
                CreateLogs(logMsg, logFilePath);

            }
        }

        public static System.Data.DataTable DataFromCsvFile(string csvFile)
        {

            System.Data.DataTable dataTable = new System.Data.DataTable();

            try
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
            catch (Exception ex)
            {
                logMsg = Environment.NewLine + ex.Message.ToString() + "\n" + ex.StackTrace;
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
                logMsg = Environment.NewLine + ex.Message.ToString() + "\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
            return null;
        }

        static void MoveCursor(int x, int y)
        {
            SetCursorPos(x, y);
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
                        //Thread modalThread = new Thread(HandleModalDialog);
                        //modalThread.Start();
                        selectionpatternprovider.Invoke();
                        //modalThread.Join();
                        Console.WriteLine("Clicked on " + targetelement);

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
                exitcode = 23;
                logMsg = "Getting exception " + ex.Message + " while clicking element " + targetelement.CurrentName + ".\n\"Exited with code : \"" + exitcode + "\n" + ex.StackTrace;
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
            catch { }

        }

        public static GmailService GetGmailService()
        {
            try
            {
                UserCredential credential;
                using (var stream =
                      new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json1";


                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        Environment.UserName,
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                // Create Gmail API service.   
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                return service;
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while getting gmail service.\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
                return null;
            }

        }

        public static string Encode(MimeMessage mimeMessage)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    mimeMessage.WriteTo(ms);
                    return Convert.ToBase64String(ms.GetBuffer())
                        .TrimEnd('=')
                        .Replace('+', '-')
                        .Replace('/', '_');
                }
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while encoding the email data.\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
                return null;
            }
        }

        public static void SendMail(string clientName,string filePath,string pmFile, string ssPath)
        {
            try
            {
                GmailService service = GetGmailService();
                //string subject = clientName + " - PM Reports (Parallel Bots)";
                string subject = clientName + " - PM Reports";
                var mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress("reportsvalidator@nirvanasolutions.com");
                mailMessage.To.Add("reportsvalidator@nirvanasolutions.com");
                mailMessage.Subject = subject;
                mailMessage.Body = "This email contains PM Dashboard data.";
                mailMessage.IsBodyHtml = true;
                if (File.Exists(filePath))
                {
                    Attachment attachment = new Attachment(filePath);
                    mailMessage.Attachments.Add(attachment);
                    if (File.Exists(pmFile))
                        {
                        attachment = new Attachment(pmFile);
                        mailMessage.Attachments.Add(attachment);
                    }
                    attachment = new Attachment(ssPath);
                    mailMessage.Attachments.Add(attachment);
                }
                var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);

                var gmailMessage = new Google.Apis.Gmail.v1.Data.Message
                {
                    Raw = Encode(mimeMessage)
                };
                if (gmailMessage.Raw == null)
                {
                    Console.WriteLine("Failed to encode MimeMessage.");
                    return;
                }
                var request = service.Users.Messages.Send(gmailMessage, "me");
                var mailSent = request.Execute();

            }
            catch (Exception ex)
            {
                exitcode = 30;
                logMsg = "Getting exception " + ex.Message + " while sending email.\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }

        }

        public static void BringToForeground(string currwin, ref IUIAutomationElement currentwin, string exePath)
        {
            try
            {
                CreateLogs("Window to bring to foreground is :"+currwin, logFilePath);
                // Check if Calculator is already open
                if (currwin == "")
                {
                    CreateLogs("Curren win is blank.", logFilePath);
                    return;
                }

                IUIAutomation automation = new CUIAutomation8();
                var root = automation.GetRootElement();
                IUIAutomationElement element = null;
                bool f = false;



                if (currentwin != null && currentwin.CurrentName == currwin)
                {
                    CreateLogs("Window is not null :" + currentwin.CurrentName, logFilePath);
                    f = true;
                    element = currentwin;
                    CreateLogs("Current window is" + currentwin.CurrentName,logFilePath);
                }
                else if (f == false && currwin.Equals("ExpnlServiceUI"))
                {
                    Thread.Sleep(5000);
                    var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");
                    var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, currwin);


                    var wincond = automation.CreateAndCondition(cond1, cond2);

                    element = root.FindFirst(TreeScope.TreeScope_Descendants, wincond);

                }
                else if (f == false)
                {
                    Thread.Sleep(5000);
                    CreateLogs("Finding current window",logFilePath);
                    var cond1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "window");
                    var cond2 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, currwin);


                    var wincond = automation.CreateAndCondition(cond1, cond2);

                    element = root.FindFirst(TreeScope.TreeScope_Descendants, wincond);
                    //CreateLogs("Window name is "+element.CurrentName, logFilePath);
                    if(element == null)
                        CreateLogs("Window is null ", logFilePath);
                }
                if (element != null)
                {
                    CreateLogs("element is not null.", logFilePath);
                    currentwin = element;
                    CreateLogs(currentwin.CurrentName, logFilePath);
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
                            CreateLogs("calstate is not null here", logFilePath);
                            if (calstate == WindowVisualState.WindowVisualState_Minimized)
                            {
                                CreateLogs("calstate == WindowVisualState.WindowVisualState_Minimized", logFilePath);
                                windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Normal);
                                CreateLogs("Normalized the window", logFilePath);
                            }
                            else if (calstate == WindowVisualState.WindowVisualState_Normal)
                            {
                                CreateLogs("calstate == WindowVisualState.WindowVisualState_Normal", logFilePath);
                                if (windowpatternprovider.CurrentCanMinimize == 1)
                                {
                                    CreateLogs("Minimized the window", logFilePath);
                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Minimized);
                                }
                                Thread.Sleep(100);
                                windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Normal);
                                //windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Maximized);

                            }
                            else if (calstate == WindowVisualState.WindowVisualState_Maximized)
                            {
                                CreateLogs("calstate == WindowVisualState.WindowVisualState_Maximized", logFilePath);
                                if (windowpatternprovider.CurrentCanMinimize == 1)
                                {
                                    CreateLogs("windowpatternprovider.CurrentCanMinimize", logFilePath);

                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Minimized);
                                    CreateLogs("Minimized the window 2nd", logFilePath);
                                }
                                Thread.Sleep(100);
                                if (windowpatternprovider.CurrentCanMaximize == 1)
                                {
                                    CreateLogs("windowpatternprovider.CurrentCanMaximize", logFilePath);
                                    windowpatternprovider.SetWindowVisualState(WindowVisualState.WindowVisualState_Maximized);
                                    CreateLogs("Maximized the window", logFilePath);
                                }

                            }

                        }

                    }
                    CreateLogs("Successfully completed method to bring current prana window on foreground", logFilePath);
                }

            }
            catch (Exception ex)
            {
                CreateLogs(ex.Message, logFilePath);
                CreateLogs(ex.StackTrace, logFilePath);
                CreateLogs("exception due to process", logFilePath);
            }
        }

        public static void OpenPMModule()
        {
            InputSimulator inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
            inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
            inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_P);
            inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);
            inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
        }

        public static bool IsClientLogin(string exePath)
        {
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
                logMsg = Environment.NewLine + ex.Message.ToString() + "\n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
                return false;
            }
        }

        public static string SaveFile(IUIAutomationElement currentwin, string exepath)
        {
            CreateLogs("Entered save file method", logFilePath);
            string saveFilePath = Directory.GetCurrentDirectory() + "\\Files\\" + DateTime.Now.ToString("yyyyMMdd");
            try
            {
                IUIAutomation automation = new CUIAutomation8();
                IUIAutomationCondition conddialogbox = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Save As");
                IUIAutomationElement dialogbox = currentwin.FindFirst(TreeScope.TreeScope_Descendants, conddialogbox);
                if (!Directory.Exists(saveFilePath))
                    Directory.CreateDirectory(saveFilePath);
                if (File.Exists(exepath.Substring(0, exepath.LastIndexOf('\\')) + "\\.xls"))
                    File.Delete(exepath.Substring(0, exepath.LastIndexOf('\\')) + "\\.xls");
                saveFilePath = saveFilePath + "\\PM.xls";
                if (File.Exists(saveFilePath))
                    File.Delete(saveFilePath);
                Console.WriteLine(saveFilePath);
                if (File.Exists(saveFilePath))
                    File.Delete(saveFilePath);
                if (dialogbox != null)
                {
                    IUIAutomationCondition conditionfilename = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId);
                    IUIAutomationCondition conditionfilename1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "File name:");
                    IUIAutomationAndCondition newandcond = (IUIAutomationAndCondition)automation.CreateAndCondition(conditionfilename1, conditionfilename);
                    IUIAutomationElement fileName = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, newandcond);

                    if (fileName != null)
                    {
                        object valuePatternObj;
                        object textPatternObj;
                        valuePatternObj = fileName.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                        textPatternObj = fileName.GetCurrentPattern(UIA_PatternIds.UIA_TextEditPatternId);

                        IUIAutomationTextEditPattern textpattern = textPatternObj as IUIAutomationTextEditPattern;
                        IUIAutomationValuePattern valuePattern = valuePatternObj as IUIAutomationValuePattern;
                        Console.WriteLine("This is from file name " + valuePattern.CurrentValue);
                        CreateLogs("This is from file name " + valuePattern.CurrentValue, logFilePath);
                        string value = saveFilePath;

                        valuePattern.SetValue(value);
                        Thread.Sleep(5000);
                    }

                    IUIAutomationCondition condopen = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Save");
                    IUIAutomationCondition condopen1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "1");
                    IUIAutomationCondition condopenand = automation.CreateAndCondition(condopen, condopen1);
                    IUIAutomationElement saveBtn = dialogbox.FindFirst(TreeScope.TreeScope_Descendants, condopenand);
                    if (saveBtn != null)
                    {
                        replayaction(saveBtn, "");
                        Thread.Sleep(2000);
                        Console.WriteLine("SAVE button pressed successfully................");
                        CreateLogs("SAVE button pressed successfully................", logFilePath);
                    }
                    Thread.Sleep(5000);
                    CreateLogs((exepath.Substring(0, exepath.LastIndexOf('\\')) + "\\.xls"), logFilePath);
                    if (File.Exists(exepath.Substring(0, exepath.LastIndexOf('\\')) + "\\.xls"))
                    {
                        CreateLogs("Entered copy method and destinatiojn path is : "+ saveFilePath, logFilePath);
                        // Rename the file
                        File.Move(exepath.Substring(0, exepath.LastIndexOf('\\')) + "\\.xls", saveFilePath);
                        Console.WriteLine("File renamed successfully.");
                        CreateLogs("File renamed successfully.", logFilePath);
                    }
                    Thread.Sleep(2000);
                    Console.WriteLine("File exported successfully");
                    CreateLogs("File exported successfully", logFilePath);
                }
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while saving exported file \n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
            return saveFilePath;
        }

        private static void SaveJpeg(string path, Bitmap img, long quality)
        {
            try
            {
                // Encoder parameter for image quality
                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                // JPEG image codec
                ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

                if (jpegCodec == null)
                    return;

                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;

                img.Save(path, jpegCodec, encoderParams);
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while saving screenshot \n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == mimeType)
                    return codec;
            }
            return null;
        }

        public static void CaptureActiveWindow(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                // Get the handle of the active window
                IntPtr hWnd = GetForegroundWindow();

                // Get the window rectangle
                GetWindowRect(hWnd, out System.Drawing.Rectangle rect);

                // Calculate the width and height
                int width = rect.Width - rect.X;
                int height = rect.Height - rect.Y;

                // Create a bitmap with the size of the window
                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    // Create a graphics object from the bitmap
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        // Copy the window into the bitmap
                        graphics.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
                    }

                    // Save the bitmap to a file
                    SaveJpeg(filePath, bitmap, 50L);
                    //bitmap.Save(filePath, ImageFormat.Png);
                }

                Console.WriteLine("Screenshot saved successfully.");
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while capturing the screen \n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
        }


        public static void RefreshExpnlData()
        {
            IUIAutomation automation = new CUIAutomation8();
            IUIAutomationElement currentwin = null;
            bool isExpnlStarted = false;
            BringToForeground("ExpnlServiceUI", ref currentwin, "");
            IUIAutomationCondition expnlCond = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ultraButtonRefreshData");
            IUIAutomationElement refreshButton = currentwin.FindFirst(TreeScope.TreeScope_Descendants, expnlCond);
            ClickElement(refreshButton, "Left Mouse Button Clicked");
            //IUIAutomationCondition condList1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "listBoxErrorMessages");
            //IUIAutomationElement listItem2 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condList1);
            //IUIAutomationElementArray listItem1 = null;
            ////if (listItem != null)
            //while (!isExpnlStarted)
            //{
            //    try
            //    {
            //        listItem1 = listItem2.FindAll(TreeScope.TreeScope_Children, automation.CreateTrueCondition());
            //        if (listItem1 != null)
            //        {
            //            for (int i = 0; i < listItem1.Length; i++)
            //            {
            //                IUIAutomationElement childElement = listItem1.GetElement(i);

            //                if (childElement.CurrentName.Contains("Data refresh was completed"))
            //                {
            //                    logMsg = childElement.CurrentName;
            //                    CreateLogs(logMsg, logFilePath);
            //                    isExpnlStarted = true;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        continue;
            //    }
            //}
            Thread.Sleep(5000);
        }

        public static void ModifyExcelSheet(string filePath)
        {
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheetAt(0);

                    // Remove the first row
                    sheet.RemoveRow(sheet.GetRow(0));
                    sheet.ShiftRows(1, sheet.LastRowNum, -1);

                    // Delete the first column
                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row != null)
                        {
                            ICell cell = row.GetCell(0);
                            if (cell != null)
                            {
                                row.RemoveCell(cell);
                            }
                        }
                    }

                    // Shift columns left to remove empty column after deletion
                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row != null)
                        {
                            for (int j = 1; j < row.LastCellNum; j++)
                            {
                                ICell sourceCell = row.GetCell(j);
                                if (sourceCell != null)
                                {
                                    row.MoveCell(sourceCell, sourceCell.ColumnIndex - 1);
                                }
                            }
                        }
                    }

                    // Rename the worksheet
                    workbook.SetSheetName(0, "Main");

                    // Write changes back to the file
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        workbook.Write(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                logMsg = "Getting exception " + ex.Message + " while modifying exported data sheet \n" + ex.StackTrace;
                CreateLogs(logMsg, logFilePath);
            }
        }

        public static void ReplayPmExport(System.Data.DataTable mainFileData, string exePath, string symbolGroupingTabName, bool isExpnlRefresh)
        {
            try
            {
                // Get the directory part of the path
                //string directoryPath = Path.GetDirectoryName(exePath);

                //// Get the parent directory of the specified file path
                //DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                //DirectoryInfo parentDirectoryInfo = directoryInfo.Parent;

                //// Extract the name of the parent directory (Tenac)
                //string clientName = parentDirectoryInfo.Name;
                bool isLogin = false;
                Process[] processes = Process.GetProcessesByName("Prana");
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
                    logMsg = "Getting exception " + ex.Message + " while getting foreground window.\n" + ex.StackTrace;
                    CreateLogs(logMsg, logFilePath);
                }

                Thread.Sleep(500);
                if(isExpnlRefresh)
                RefreshExpnlData();
                OpenPMModule();
                Thread.Sleep(5000);
                if (currentwin != null)
                {
                    Console.WriteLine(currentwin.CurrentName);
                }
                MaximizeWindow("Portfolio Management", ref currentwin);
                if (currentwin != null)
                {
                    Console.WriteLine(currentwin.CurrentName);
                }
                _currwinglobal = currentwin;

                while (currentwin == null || (currentwin.CurrentAutomationId != "PM"))
                {
                    BringToForeground("Nirvana", ref currentwin, exePath);
                    Console.WriteLine(currentwin.CurrentName);
                    OpenPMModule();
                    MaximizeWindow("Portfolio Management", ref currentwin);
                    Thread.Sleep(5000);
                    if (currentwin != null)
                    {
                        Console.WriteLine(currentwin.CurrentName);
                    }
                    _currwinglobal = currentwin;
                }

                if ((currentwin.CurrentAutomationId == "PM"))
                {
                    Thread.Sleep(30000);
                    //Read Start of Day Nav
                    IUIAutomationCondition condList = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "pmDashboard");
                    IUIAutomationElement listItem = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condList);
                    string path = Directory.GetCurrentDirectory() + @"\PMDashboardData_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

                    if (listItem == null)
                    {
                        logMsg = "Dashboard is not visible on client";
                        CreateLogs(logMsg, logFilePath);
                    }
                    else
                    {
                        XSSFWorkbook workbook = new XSSFWorkbook();
                        ISheet worksheet = workbook.CreateSheet("Sheet1");
                        IRow row1 = worksheet.CreateRow(0);
                        IRow row2 = worksheet.CreateRow(1);
                        workbook.SetSheetName(0, "PMDashboardData");
                        //1st approach
                        IUIAutomationCondition headerCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_HeaderItemControlTypeId);
                        IUIAutomationElementArray headerListItem = listItem.FindAll(TreeScope.TreeScope_Descendants, headerCondition);

                        IUIAutomationCondition editCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_EditControlTypeId);
                        //IUIAutomationElementArray editListItem = listItem.FindAll(TreeScope.TreeScope_Descendants, editCondition);

                        row1.CreateCell(0).SetCellValue("Header");
                        row2.CreateCell(0).SetCellValue("Values");
                        worksheet.AutoSizeColumn(0);
                        for (int j = 0; j < headerListItem.Length; j++)
                        {
                            IUIAutomationElement headerElement = headerListItem.GetElement(j);
                            if (headerElement.CurrentAutomationId.Contains("[Column Header]"))
                            {
                                IUIAutomationCondition valCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, headerElement.CurrentName);
                                IUIAutomationCondition combinedCondition = automation.CreateAndCondition(editCondition, valCondition);
                                IUIAutomationElement valElement = listItem.FindFirst(TreeScope.TreeScope_Descendants, combinedCondition);

                                IUIAutomationValuePattern valuePattern = valElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId) as IUIAutomationValuePattern;
                                if (valuePattern == null)
                                {
                                    Console.WriteLine("Failed to get Value pattern.");
                                    return;
                                }
                                string value = valuePattern.CurrentValue;
                                row1.CreateCell(j + 1).SetCellValue(headerElement.CurrentName);
                                row2.CreateCell(j + 1).SetCellValue(value);
                                worksheet.AutoSizeColumn(j + 1);
                            }
                        }
                        CaptureActiveWindow(Directory.GetCurrentDirectory() + "\\PMDashboard_ss.png");
                        if (File.Exists(path))
                            File.Delete(path);
                        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                        {
                            workbook.Write(fileStream);
                        }
                        // Clean up
                        workbook.Close();
                    }

                    IUIAutomationCondition condtitlebar = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "_PM_UltraFormManager_Dock_Area_Top");
                    IUIAutomationElement titlebar = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condtitlebar);
                    //InputSimulator inputSimulator = new InputSimulator();
                    //tagPOINT p;
                    //if (titlebar != null)
                    //{
                    //    titlebar.GetClickablePoint(out p);
                    //    System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)p.x, (int)p.y);
                    //    inputSimulator.Mouse.LeftButtonClick();
                    //    Thread.Sleep(500);
                    //}

                    try
                    {
                        IUIAutomationCondition tab = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, symbolGroupingTabName);
                        IUIAutomationElement tabItem = currentwin.FindFirst(TreeScope.TreeScope_Descendants, tab);
                        if (tabItem == null)
                        {
                            Console.WriteLine("PM export failed!!" + "\nSymbol grouping tab name does not exists.");
                            string msg = "Symbol grouping tab name does not exists.";
                            CreateLogs(msg, logFilePath);
                            Environment.Exit(0);
                        }
                        ClickElement(tabItem, "Left Mouse Button Clicked");
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        logMsg = "getting exception " + ex.Message + " while selecting symbol grouping tab name.\n" + ex.StackTrace;
                        CreateLogs(logMsg, logFilePath);
                        Console.WriteLine(logMsg);
                        throw;
                    }

                    MaximizeWindow("Portfolio Management", ref currentwin);
                    Thread.Sleep(3000);
                    IUIAutomationCondition condGrid = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "pmGrid");
                    IUIAutomationElement element = currentwin.FindFirst(TreeScope.TreeScope_Descendants, condGrid);

                    int left = int.MaxValue;
                    int top = int.MaxValue;
                    int right = int.MaxValue;
                    int bottom = int.MaxValue;

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
                        simulator.Mouse.RightButtonClick();
                    }
                    Thread.Sleep(3000);
                    IUIAutomationCondition menu = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Export");
                    IUIAutomationElement menuitem = currentwin.FindFirst(TreeScope.TreeScope_Descendants, menu);
                    ClickElement(menuitem, "Left Mouse Button Clicked");
                    Thread.Sleep(3000);
                    IUIAutomationCondition menu1 = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Excel");
                    IUIAutomationElement menuitem1 = currentwin.FindFirst(TreeScope.TreeScope_Descendants, menu1);
                    ClickElement(menuitem1, "Left Mouse Button Clicked");
                    Thread.Sleep(3000);
                    string filePath = SaveFile(currentwin, exePath);
                    if (File.Exists(filePath))
                    {
                        //string configFilePath = Directory.GetCurrentDirectory()+"\\Credentials.xml";
                        //XElement config = XElement.Load(configFilePath);
                        ModifyExcelSheet(filePath);
                        //string encryptedUsername = config.Element("serverSettings").Element("username").Value;
                        //string encryptedPassword = config.Element("serverSettings").Element("password").Value;
                        //string serverUrl = config.Element("serverSettings").Element("serverUrl").Value;
                        string clientQC = GetValueFromMainFile(mainFileData, "clientName_QC");
                        destinationPath += DateTime.Now.ToString("yyyyMMdd") + "\\" + clientQC;
                        //string username = Decrypt(encryptedUsername);
                        //string password = Decrypt(encryptedPassword);
                        if (!Directory.Exists(destinationPath))
                            Directory.CreateDirectory(destinationPath);
                            //DeleteFolderWithCredentials(destinationPath, username, password);
                        File.Copy(filePath,destinationPath+"\\PM.xls", true);
                        //CopyFileWithCredentials(filePath, destinationPath + "\\PM.xls", username, password);
                    }
                    SendMail(clientName,path,filePath, Directory.GetCurrentDirectory() + "\\PMDashboard_ss.png");
                    exitcode = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        static void DeleteFolderWithCredentials(string folderPath, string username, string password)
        {
            var netResource = new NETRESOURCE
            {
                dwType = 1, // Disk resource
                lpLocalName = null,
                lpRemoteName = folderPath,
                lpProvider = null
            };

            // Connect to the server with credentials
            var result = WNetAddConnection2(ref netResource, password, username, 0);
            if (result != 0)
            {
                Console.WriteLine("Error connecting to remote server: " + result);
                return;
            }

            try
            {
                if (Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    Console.WriteLine("Folder created successfully.");
                }
                else
                {
                    Console.WriteLine("Folder not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting folder: " + ex.Message);
            }
            finally
            {
                WNetCancelConnection2(folderPath, 0, true);
            }
        }

        static string Decrypt(string encryptedText)
        {
            // Implement your decryption logic here
            return encryptedText; // Placeholder, replace with actual decryption logic
        }
        static void CopyFileWithCredentials(string sourceFile, string destinationFile, string username, string password)
        {
            var netResource = new NETRESOURCE
            {
                dwType = 1, // Disk resource
                lpLocalName = null,
                lpRemoteName = destinationFile,
                lpProvider = null
            };

            // Connect to the server with credentials
            var result = WNetAddConnection2(ref netResource, password, username, 0);
            if (result != 0)
            {
                Console.WriteLine("Error connecting to remote server: " + result);
                return;
            }

            try
            {
                File.Copy(sourceFile, destinationFile, true);
                Console.WriteLine("File copied successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error copying file: " + ex.Message);
            }
            finally
            {
                WNetCancelConnection2(destinationFile, 0, true);
            }
        }

        public static void Main(string[] args)
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
            if (command == "p")
            {
                if (!Directory.Exists(logFolderPath))
                {
                    Directory.CreateDirectory(logFolderPath);
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                MinimizeConsoleWindow();
                string pmConfig = "PMConfigs";
                if (!Directory.Exists(pmConfig))
                {
                    Directory.CreateDirectory(pmConfig);
                }

                string mainfilePath = @"PMConfigs\MainFile.csv";

                System.Data.DataTable mainFileData = DataFromCsvFile(mainfilePath);
                string exePath = GetValueFromMainFile(mainFileData, "exePath");
                string symbolGroupingTabName = GetValueFromMainFile(mainFileData, "SymbolGroupingTabName");
                bool isExpnlRefresh = false;
                string expnlRef = GetValueFromMainFile(mainFileData, "IsExpnlRefresh");
                if (expnlRef.Equals("true"))
                    isExpnlRefresh = true;
                clientName = GetValueFromMainFile(mainFileData, "clientName");

                Thread.Sleep(4000);
                try
                {
                    Console.WriteLine("Started export for PM module");
                    ReplayPmExport(mainFileData, exePath, symbolGroupingTabName, isExpnlRefresh);
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
                        logMsg = "PM Export completed successfully!!";
                    else
                        logMsg = "Exited with exitcode : " + exitcode;
                    Console.WriteLine(logMsg);
                    CreateLogs(logMsg, logFilePath);

                    Environment.Exit(exitcode);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception : " + ex.Message);
                }
                finally
                {
                    //if (!BlockInput(false))
                    //{
                    //    Console.WriteLine("Failed to unblock input.");
                    //}
                }
            }
            else
            {
                Console.WriteLine("Please give a valid input ");
            }
        }
    }
}
