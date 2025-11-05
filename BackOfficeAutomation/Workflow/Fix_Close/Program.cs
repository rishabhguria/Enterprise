using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UIAutomationClient;

namespace ConsoleMonitor
{
    class Program
    {
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
        public static void CreateLogs(string logMessage, int dateCheck = 1)
        {
            try
            {
                Console.WriteLine(logMessage);
                if (File.Exists(MainLogFilePath))
                {
                    using (StreamWriter writer = File.AppendText(MainLogFilePath))
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
                    using (StreamWriter writer = File.CreateText(MainLogFilePath))
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
                //exitcode = 9;
                logMessage = "Getting exception " + ex.Message + " while saving logs.";
                Console.WriteLine(logMessage);
                //CreateLogs(logMessage);
            }

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
        public static string MainLogFilePath = "";
        private static void InitializeLogFile()
        {
            // Get the current date and time
            DateTime currentDate = DateTime.Now;

            // Create a filename with the current date and time

            string logFilename = Directory.GetCurrentDirectory() + $"\\Logs\\FixClose_Log_{currentDate.ToString("yyyyMMdd_HHmmss")}.txt";
            MainLogFilePath = logFilename;
            Directory.CreateDirectory(Path.GetDirectoryName(logFilename));

            try
            {
                // Create the log file
                Directory.CreateDirectory(Path.GetDirectoryName(logFilename));
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
        static Dictionary<string, List<string>> WindowToSearchTextList = new Dictionary<string, List<string>>();

        static int configgetwindowtime = 8;
        static int configConsoleReadTime = 5;
        static void Main(string[] args)
        {
            try
            {
                InitializeLogFile();
                string mainfilePath = @"FixCloseConfigs\MainFile.csv";
                DataTable mainFileData = DataFromCsvFile(mainfilePath);
                string AllWindowTitle = GetValueFromMainFile(mainFileData, "AllConsoleWindowTitle");
                List<string> AllWindowTitleList = AllWindowTitle.Split(',').ToList();

                string DirectWindowTitle = GetValueFromMainFile(mainFileData, "DirectWindowTitle");
                List<string> DirectWindowTitleList = DirectWindowTitle.Split(',').ToList();

                configgetwindowtime = int.Parse(GetValueFromMainFile(mainFileData, "BringToForegroundTime"));
                configConsoleReadTime = int.Parse(GetValueFromMainFile(mainFileData, "ConsoleReadTime"));

                string searchtext = GetValueFromMainFile(mainFileData, "WindowToSearchText");
                string[] searchtextlist = searchtext.Split('#');
                

                
                foreach(string s in searchtextlist)
                {
                    string[] searchlist = s.Split(':');
                    if (searchtextlist.Length == 2)
                    {
                        string key = searchlist[0];
                        List<string> values = new List<string>(searchlist[1].Split(','));

                        WindowToSearchTextList[key] = values;
                    }


                }

                Process[] processes = Process.GetProcesses();

                foreach (Process process in processes)
                {
                    string mainwindowtitle = "";
                    try
                    {
                        mainwindowtitle = process.MainWindowTitle;
                    }
                    catch
                    {

                    }
                    //allprocesslist.contains(mainwindowtitle)
                    if (process.ProcessName.ToLower().Contains("cmd") && AllWindowTitleList.Contains(mainwindowtitle))
                    {
                        try
                        {
                            //directclosewindownameslist.contains(mainwindowtitle)
                            if(DirectWindowTitleList.Contains(mainwindowtitle))
                            {
                                CloseConsoleWindow(process);
                                continue;
                            }
                            Console.WriteLine(process.Id);
                            Console.WriteLine(process.MainWindowTitle);
                            Console.WriteLine(process.SessionId);
                            Console.WriteLine(process.StartInfo);
                            //Console.WriteLine(process.StandardOutput);
                            Console.WriteLine("Inside CMD");
                            try
                            {
                                bool consoleText = ReadConsoleWindow(process.MainWindowTitle,process.Id);

                                if (consoleText)
                                {
                                    CloseConsoleWindow(process);
                                    Console.WriteLine($"Closed console window: {process.ProcessName}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.StackTrace);
                                Console.WriteLine($"Error processing console window: {process.ProcessName} - {ex.Message}");
                            }
                          //  Console.ReadLine();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No CMD Window with Name In Config Found. Check Config");
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                CreateLogs(ex.StackTrace);
                CreateLogs(ex.Message);
            }
            //Console.ReadLine();
        }
        public static bool ReadConsoleWindow(string windowName,int processId)
        {
            int i = 0;
            IUIAutomation automation = new CUIAutomation8();
            IUIAutomationElement currentwin = null;
            //string windowName = GetWindowName(exePath);
            List<string> ProcessIdIgnoreList = new List<string>();
            //processidtoignorelist
            //configurable iteration

            while (currentwin == null && i < configgetwindowtime)
            {
                BringToForeground(windowName, ref automation, ref currentwin,processId);
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
                        Console.WriteLine("Process Id of text area " + documentElement.CurrentProcessId);
                        // Retrieve the text pattern for the element
                        object patternObj = null;
                        try
                        {
                            patternObj = documentElement.GetCurrentPattern(UIA_PatternIds.UIA_TextPatternId);
                            Console.WriteLine(patternObj.ToString());
                           
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
                                int x = 0;
                                // Get the text range of the document

                                while (x<configConsoleReadTime)
                                {
                                    //foreach search text in windowtosearchtextdict[mainwindowhandle]
                                    //if consoletext contain this break
                                    bool flag = false;
                                    foreach(string s in WindowToSearchTextList[windowName])
                                    {
                                        if (consoleText.Contains(s))
                                            flag = true;
                                    }
                                    if (flag)
                                    {
                                        break;
                                    }
                                    
                                    IUIAutomationTextRange documentRange = textPattern.DocumentRange;
                                    // Retrieve the text from the document range
                                    consoleText = documentRange.GetText(-1); 
                                   // Console.WriteLine(consoleText);
                                    x++;
                                    // -1 indicates the entire document
                                    Thread.Sleep(2000);
                                }
                                if (x < configConsoleReadTime)
                                {
                                    return true;
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
                Console.WriteLine(ex.StackTrace);
                //logMsg = "Getting exception " + ex.Message + " in method ReadConsoleWindow \n" + ex.StackTrace;
               // CreateLogs(logMsg);
            }
            return false;
        }
        public static void BringToForeground(string currwin, ref IUIAutomation automation, ref IUIAutomationElement currentwin,int processId)
        {
            try
            {
               
                var root = automation.GetRootElement();
                string processOwner = string.Empty;
                
                    Console.WriteLine("Searching for the window.");
                    Thread.Sleep(1000);
                    int namePropertyId = UIA_PropertyIds.UIA_NamePropertyId;
                    IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_WindowControlTypeId);
                    IUIAutomationElementArray windowElements = automation.GetRootElement().FindAll(TreeScope.TreeScope_Children, condition);
                    Console.WriteLine("Number of window elements are:" + windowElements.Length);
                    for (int i = 0; i < windowElements.Length; i++)
                    {
                        IUIAutomationElement windowElement = windowElements.GetElement(i);
                        string windowName = windowElement.GetCurrentPropertyValue(UIA_PropertyIds.UIA_NamePropertyId).ToString();
                        Console.WriteLine("Window Element Process ID" + windowElement.CurrentProcessId);
                        Console.WriteLine("WindoW Current Name" + windowElement.CurrentName);
                        Console.WriteLine(windowName);
                        Console.WriteLine(windowElement.CurrentAutomationId);
                        // Check if the name contains the search string
                        if (windowName==currwin)
                        {
                            currentwin = windowElement;
                            Console.WriteLine("Found window with name: " + windowName);
                            break;
                        }
                    }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Getting exception " + ex.Message + " while bringing " + currentwin.CurrentName + " to foreground.\n" + ex.StackTrace);
                
            }
        }

        static void CloseConsoleWindow(Process process)
        {
            try
            {

                process.CloseMainWindow();
                CreateLogs(process.MainWindowTitle + " Window Closed Successfully");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                CreateLogs(ex.StackTrace);
            }
        }
    }
}