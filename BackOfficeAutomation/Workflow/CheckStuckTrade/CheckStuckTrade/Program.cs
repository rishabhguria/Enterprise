using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    public static DateTime currentDate = DateTime.Now;
    public static string logFolderPath = "Logs";
    public static string logFilePath = Path.Combine(logFolderPath, $"Log_{currentDate.ToString("yyyyMMdd")}.txt");
    public static DateTime initialTime = DateTime.Now;
    public static DateTime finalTime;

    static void Main(string[] args)
    {
        MinimizeConsoleWindow();
        string exeName = "Prana.TradeServiceUI";
        int processId = GetApplicationProcessId(exeName);
        bool startDriver = StartDriver();

        // Start the Appium local service
        WindowsDriver<WindowsElement> session;
        if (processId != -1)
        {
            //var appiumOptions = new AppiumOptions();
            //string exePath = @"E:\Enterprise v2.12\Enterprise\Application\Prana.Server\Prana.TradeServiceUI\bin\Debug\Prana.TradeServiceUI.exe";
            //appiumOptions.AddAdditionalCapability("app", exePath);
            //appiumOptions.AddAdditionalCapability("appTopLevelWindow", processId);
            //appiumOptions.AddAdditionalCapability("appId", @"E:\Enterprise v2.12\Enterprise\Application\Prana.Server\Prana.TradeServiceUI\bin\Debug\Prana.TradeServiceUI.exe");

            AppiumOptions appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", "Root");
            session = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptions);
        }
        else
        {
            string exeConfigFileFolder = $@"{Directory.GetCurrentDirectory()}\Config";

            if (!Directory.Exists(exeConfigFileFolder))
            {
                // If the directory doesn't exist, create it
                try
                {
                    Directory.CreateDirectory(exeConfigFileFolder);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating directory: {ex.Message}");
                }
            }

            
            string exeConfigFilePath = $@"{exeConfigFileFolder}\TUIexePath.txt";

            if (!File.Exists(exeConfigFilePath))
            {
                // If the file does not exist, create it
                using (StreamWriter writer = File.CreateText(exeConfigFilePath))
                {
                }

                Console.WriteLine($"The file '{exeConfigFilePath}' is created please put the exe path in the file.");
                Console.WriteLine("Press any key to continue !!");
                Console.ReadKey();
                return;
            }


            string exePath = ReadExeConfig(exeConfigFilePath);

            if (exePath != null)
            {
                AppiumOptions appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", exePath);
                session = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptions);
                Thread.Sleep(3000);
            }
            else
            {
                session = null;
                return;
            }
        }


        var TUIElement = session.FindElementByAccessibilityId("TradeServiceUI");
        TUIElement.Click();

        var MenuStrip = TUIElement.FindElementByAccessibilityId("menuStrip1");
        var MessagesBtn = MenuStrip.FindElementByName("Messages");
        new Actions(session).MoveToElement(MessagesBtn, 25, 15).MoveByOffset(0, 0).Click().Build().Perform();

        var errMsgBtn = MessagesBtn.FindElementByName("Show Error Messages");
        new Actions(session).MoveToElement(errMsgBtn, 25, 15).MoveByOffset(0, 0).Click().Build().Perform();

        var PostTradeOrderBtn = errMsgBtn.FindElementByName("DropCopy_PostTrade");
        new Actions(session).MoveToElement(PostTradeOrderBtn, 25, 15).MoveByOffset(0, 0).Click().Build().Perform();

        Thread.Sleep(2000);

        var UltraPanelButtons = TUIElement.FindElementByAccessibilityId("ultraPanelButtons");

        var ExportExcelBtn = UltraPanelButtons.FindElementByAccessibilityId("ultraButtonExcel");
        new Actions(session).MoveToElement(ExportExcelBtn, 35, 15).MoveByOffset(0, 0).Click().Build().Perform();

        Thread.Sleep(2000);

        var saveAs = session.FindElementByClassName("#32770");
        var FileNameTextElement = saveAs.FindElementByAccessibilityId("1001");
        var currentDir = Directory.GetCurrentDirectory();
        string exportFolder = $@"{currentDir}\StuckTradesOutput";
        string currDate = DateTime.Now.ToString("ddMMyyyy");
        string currDateFolder = DateTime.Now.ToString("ddMM");

        string exportFolderName = $@"{exportFolder}\StuckTrades{currDateFolder}";

        string exportFileName = $@"{exportFolderName}\StuckTrades{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}_{currDate}";

        if (!Directory.Exists(exportFolderName))
        {
            // If the directory doesn't exist, create it
            try
            {
                Directory.CreateDirectory(exportFolderName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating directory: {ex.Message}");
            }
        }

        FileNameTextElement.SendKeys(exportFileName);

        var saveBtn = saveAs.FindElementByName("Save");
        new Actions(session).MoveToElement(saveBtn, 35, 15).MoveByOffset(0, 0).Click().Build().Perform();

        Thread.Sleep(4000);

        string completeFileName = $@"{exportFileName}.xls";
        DataTable stuckTradeDataTable = ReadExcelToDataTable(completeFileName);
        int rowCount = (stuckTradeDataTable != null) ? stuckTradeDataTable.Rows.Count : 0;




        Console.Clear(); // Clear the console
        RestoreConsoleWindow();
        int cnt = 0;

        if (rowCount == 0)
        {
            //Console.WriteLine("There are no Stuck Trade !!");
            Console.WriteLine("There are no Stuck Trade !!");
        }
        else
        {
            try
            {
                foreach (DataRow row in stuckTradeDataTable.Rows)
                {
                    cnt++;
                    foreach (DataColumn column in stuckTradeDataTable.Columns)
                    {
                        Console.Write(row[column] + "\t");
                    }
                    Console.WriteLine();
                    Console.WriteLine("");
                }
            }
            catch
            {

            }

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total Stuck Trades : {cnt}");
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Press any key to continue !!");
        Console.ReadKey();
        Console.ResetColor(); // Reset color to default

        session.Quit();


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
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
        return path;
    }

    static DataTable ReadExcelToDataTable(string filePath)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            HSSFWorkbook workbook = new HSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(0); // Assuming you are reading the first sheet

            DataTable dataTable = new DataTable(sheet.SheetName);

            // Assuming the first row contains column headers
            IRow headerRow = sheet.GetRow(0);

            foreach (ICell headerCell in headerRow)
            {
                dataTable.Columns.Add(headerCell.StringCellValue);
            }

            // Start reading from the second row
            for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                IRow dataRow = sheet.GetRow(rowIndex);
                DataRow newRow = dataTable.NewRow();

                for (int cellIndex = 0; cellIndex < headerRow.LastCellNum; cellIndex++)
                {
                    newRow[cellIndex] = dataRow.GetCell(cellIndex)?.ToString();
                }

                dataTable.Rows.Add(newRow);
            }

            return dataTable;
        }
    }

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const int SW_MINIMIZE = 6;
    const int SW_RESTORE = 9;


    public static void MinimizeConsoleWindow()
    {
        IntPtr hWndConsole = GetConsoleWindow();
        if (hWndConsole != IntPtr.Zero)
        {
            ShowWindow(hWndConsole, SW_MINIMIZE);
        }
    }

    public static void RestoreConsoleWindow()
    {
        IntPtr hWndConsole = GetConsoleWindow();
        if (hWndConsole != IntPtr.Zero)
        {
            ShowWindow(hWndConsole, SW_RESTORE);
        }
    }


    public static int GetApplicationProcessId(string exeName)
    {
        Process[] processes = Process.GetProcessesByName(exeName);
        if (processes.Length > 0)
        {
            // Assuming the first instance of the process is the one you want
            return processes[0].Id;
        }

        // Return -1 if the process is not found
        return -1;
    }



    static bool StartDriver()
    {

        string driverPath = @"C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe";


        if (System.IO.File.Exists(driverPath))
        {
            try
            {
                Process.Start(driverPath);
                return true;
            }
            catch (Exception ex) { return false; }
        }
        return false;

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
}