using HtmlAgilityPack;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nirvana.TestAutomation.TestDataProvider;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Xml;
using TestAutomationFX.Core;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using OpenQA.Selenium.Support.UI;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Support.UI;
using Nirvana.TestAutomation.Interfaces.Enums;
//using System.Windows.Forms;

namespace Nirvana.TestAutomation.Utilities
{
    public static class SamsaraHelperClass
    {
        enum WindowDirection
        {
            RightTop,
            RightBottom,
            LeftTop,
            LeftBottom
        }
        public static DataTable ColumnValues = new DataTable();
        public static Dictionary<string, DataTable> DataTables = ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["SamsaraDataFile"]);
        public static Dictionary<string, DataTable> SamsaraMappingTables = ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["SamsaraMappingFile"]);
        public static List<string> CheckOpenModuleFirstSteps = ConfigurationManager.AppSettings["-checkOpenModuleFirstSteps"].ToString().Split(',').ToList();
        static string DefaultCellValue = "";
        public static DataSet TestCaseSheet_Temp = null;
        static int TradeIndex = 0;
        static int RowIndex = 1;
        static int GlobalRowCount = 0;
        static bool needNextIteration = true;
        static string temp_ModuleName = string.Empty;
        public static WebDriver GlobalDriver;
        private static Dictionary<string, object> dict2 = new Dictionary<string, object>();
        private static List<string> toastList = new List<string>();

        
        public static Dictionary<string, object> GetDict(DataTable dt, int columnnumber = 1)
        {
            try
            {
                if (columnnumber != 1)
                    return dt.AsEnumerable().ToDictionary<DataRow, string, object>(row => row.Field<string>(0),
                                           row => row.Field<object>(2));
                return dt.AsEnumerable().ToDictionary<DataRow, string, object>(row => row.Field<string>(0),
                                            row => row.Field<object>(1));
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while converting DataTable to Dictionary in GetDict Method: " + ex.Message);
            }
            return new Dictionary<string, object>();
        }

        public static Dictionary<string, DataTable> ExcelToSamsaraDataTable(string path = "")
        {
            var dataTables = new Dictionary<string, DataTable>();
            string filePath = ConfigurationManager.AppSettings["SamsaraMappingFile"];

            if (!string.IsNullOrEmpty(path))
                filePath = path;

            string tempFilePath = null;

            try
            {
                // Check if the file is accessible
                if (IsFileLocked(new FileInfo(filePath)))
                {
                    Console.WriteLine("The file is currently in use. Creating a temporary copy.");

                    // Create a temporary copy if the file is locked
                    tempFilePath = Path.Combine(Path.GetDirectoryName(filePath), "temp_" + Path.GetFileName(filePath));
                    File.Copy(filePath, tempFilePath, true);
                    filePath = tempFilePath;

                    Console.WriteLine("Temporary copy created: " + tempFilePath);
                }
                else
                {
                    Console.WriteLine("The file is accessible. Using the original file. File Name: " + filePath.Substring(filePath.LastIndexOf("\\") + 1));
                }

                using (ExcelPackage xlpackage = new ExcelPackage(new FileInfo(filePath)))
                {
                    foreach (var worksheet in xlpackage.Workbook.Worksheets)
                    {
                        DataTable datatable = new DataTable(worksheet.Name);

                        for (int column = 1; column <= worksheet.Dimension.Columns; column++)
                        {
                            try
                            {
                                datatable.Columns.Add(worksheet.Cells[1, column].Value.ToString());
                            }
                            catch { }
                        }

                        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                        {
                            try
                            {
                                DataRow dataRow = datatable.NewRow();
                                for (int column = 1; column <= worksheet.Dimension.Columns; column++)
                                {
                                    dataRow[column - 1] = worksheet.Cells[row, column].Value;
                                }
                                datatable.Rows.Add(dataRow);
                            }
                            catch { }
                        }

                        dataTables.Add(worksheet.Name, datatable);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred in method: ExcelToSamsaraDataTable: " + ex.Message);
            }
            finally
            {
                // Delete the temporary file if it was created
                if (!string.IsNullOrEmpty(tempFilePath) && File.Exists(tempFilePath))
                {
                    try
                    {
                        File.Delete(tempFilePath);
                        Console.WriteLine("Temporary file deleted: " + tempFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error deleting temporary file: " + ex.Message);
                    }
                }
            }

            return dataTables;
        }

        // Method to check if the file is locked by another process
        private static bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }
        //public static Dictionary<string, string> SamsaraXpath = new Dictionary<string, string>();
        public static string SamsaraXpath(string key, string moduleName)
        {
            try
            {
                var value = GetDict(SamsaraMappingTables[moduleName]);
                return value[key].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred in method:SamsaraXpath: " + ex.Message);
            }
            return null;
        }
        public static string GetVariableValue(string variableName)
        {
            try
            {
                Type applicationArgumentsType = typeof(ApplicationArguments);
                FieldInfo FieldName = applicationArgumentsType.GetField(variableName);

                if (FieldName != null)
                {
                    return FieldName.GetValue(null).ToString();
                }
                else
                {
                    PropertyInfo PropertyName = applicationArgumentsType.GetProperty(variableName);
                    return PropertyName.GetValue(null).ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred in method:GetVariableValue: " + ex.Message);
            }
            return null;

        }

        public static TestResult GenericMethod(string moduleName, DataSet TestCaseSheet, string StepName)
        {
            temp_ModuleName = moduleName;
            TestResult _res = new TestResult();
            WebDriver driver = InitializeDriver();
            TestCaseSheet_Temp = TestCaseSheet;
            if (ConfigurationManager.AppSettings["OpenSpecifiedWindow"].ToString().Split(',').Contains(StepName))
            {
                moduleName = TestCaseSheet_Temp.Tables[0].Rows[0]["OpenWindow"].ToString();
            }

            if (StepName.ToLower() == "loginclient")
            {
                List<string> columns = new List<string>();
                string username = Convert.ToString(TestCaseSheet.Tables[0].Rows[0]["LoginUserName"]);
                string password = Convert.ToString(TestCaseSheet.Tables[0].Rows[0]["LoginPassword"]);
                ApplicationArguments.RequiredActiveUser = username;
                if (username == "Support1")
                    ApplicationArguments.ReleaseUserName = ConfigurationManager.AppSettings["EnterpriseUser"];
                RunOpenFinTest("samsara", username, password);
                driver = InitializeDriver();
                return _res;
            }
            var dict = GetDict(SamsaraMappingTables[temp_ModuleName]);
            try
            {
                dict2 = GetDict(SamsaraMappingTables[temp_ModuleName], 2);
            }
            catch { }
            if (!ConfigurationManager.AppSettings["SkipOpenModule"].Contains(StepName))
            {
                if (CheckOpenModuleFirstSteps.Contains(StepName))
                {
                    bool isWindwOpen = IsWindowOpen("Trading Ticket");
                    if (!isWindwOpen)
                    {
                        OpenModule(moduleName, driver);
                        Console.WriteLine(moduleName + " Opened ,StepName:" + StepName);
                    }
                    else
                    {
                        Console.WriteLine(moduleName + " Already Opened ,StepName:" + StepName);

                    }
                }
                else
                    OpenModule(moduleName, driver);
            }
            moduleName = temp_ModuleName;
            Wait(10000);
            if (ApplicationArguments._ModuleStepMapping[moduleName + StepName] == "Input")
            {
                //Task.Delay(10000);
                foreach (DataRow row in TestCaseSheet.Tables[0].Rows)
                {
                    needNextIteration = true;
                    ResetValues();
                    if (!ConfigurationManager.AppSettings["SkipOpenModule"].Contains(StepName))
                    {
                        if (CheckOpenModuleFirstSteps.Contains(StepName))
                        {
                            bool isWindwOpen = IsWindowOpen("Trading Ticket");
                            if (!isWindwOpen)
                            {
                                Console.WriteLine(moduleName + " Opened ,StepName:" + StepName);
                                OpenModule(moduleName, driver);
                            }
                            else
                            {
                                Console.WriteLine(moduleName + " Already Opened ,StepName:" + StepName);

                            }
                        }
                        else
                            OpenModule(moduleName, driver);
                    }
                    _res = InputDetails(row, StepName, dict, driver, moduleName, TestCaseSheet);
                    if (!_res.IsPassed)
                        return _res;
                    if (needNextIteration == false)
                    {
                        break;
                    }
                }
            }
            else
            {

                ApplicationArguments.isVerificationSuceeded = false;

                DataTable UIData = FetchAndVerifyDataFromGrid(DataTables[StepName], dict, driver, StepName, TestCaseSheet, ref _res);
                try
                {
                    if (!ApplicationArguments.isVerificationSuceeded)
                    {
                        //string filepath = @"\\192.168.2.108\DistributedAutomation_Master\Column_Mapping.xlsx";
                        //string sheetName = moduleName;
                        //var columnMapping = CreateColumnMapping(filepath, sheetName);
                        // string steptype = DataTables[StepName].Rows[0]["StepType"].ToString();
                    }
                }
                //}

                catch (Exception ex)
                {
                    _res.IsPassed = false;
                    throw ex;
                }

            }




            return _res;
        }


        public static TestResult RunOpenFinTest(string PD, string username = "", string password = "", string stepName = "LoginClient", string sheetName = "Login", string handleName = "Login")
        {
            WebDriver driver = null;
            System.Threading.Thread.Sleep(5000);
            //StartSamsaraApplication();
            var table = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["SamsaraDataFile"])[stepName];

            TestResult _result = new TestResult();
            if (PD.ToLower() == "samsara")
            {

                System.Threading.Thread.Sleep(5000);
                try
                {
                    driver = GetSamsaraDriver(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " . Exception on RunOpenFinTest at Step :" + stepName);
                }
                SwitchWindow.SwitchToWindow(driver, handleName);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    try
                    {
                        string KeyName = table.Rows[i]["Key Name"].ToString();
                        string action = table.Rows[i]["Actions"].ToString();
                        string VariableName = table.Rows[i]["Variable Name"].ToString();

                        if (VariableName == "SamsaraReleaseUserName" || VariableName == "RequiredActiveUser")
                        {
                            if (!string.IsNullOrEmpty(username))
                                VariableName = username;
                            else
                            {
                                VariableName = SamsaraHelperClass.GetVariableValue(VariableName);
                            }


                            if (table.Columns.Contains("VariableValue") && !string.IsNullOrEmpty(table.Rows[i]["VariableValue"].ToString()))
                            {
                                VariableName = table.Rows[i]["VariableValue"].ToString();
                            }


                        }
                        else if (VariableName == "ReleasePassword")
                        {
                            if (!string.IsNullOrEmpty(password))
                                VariableName = password;
                            else
                            {
                                VariableName = SamsaraHelperClass.GetVariableValue(VariableName);
                            }


                            if (table.Columns.Contains("VariableValue") && !string.IsNullOrEmpty(table.Rows[i]["VariableValue"].ToString()))
                            {
                                VariableName = table.Rows[i]["VariableValue"].ToString();
                            }
                        }
                        string moduleName2 = "HomeDock";
                        if (action == "SendKeys")
                        {
                            driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath(KeyName, sheetName))).SendKeys(VariableName);
                        }
                        else if (action == "Click")
                        {
                            Wait(10000);
                            try
                            {
                                driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath(KeyName, moduleName2))).Click();
                            }
                            catch { }
                            if (string.Equals(KeyName, "More", StringComparison.OrdinalIgnoreCase))
                            {
                                Wait(3000);
                                //Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                                Keyboard.SendKeys("M");
                                //Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                            }
                        }
                        else if (KeyName == "ShiftWindow")
                        {

                            SamsaraHelperClass.MoveWindow(ref driver, ref action);
                        }
                        else if (KeyName == "SwitchWindow")
                        {
                            SwitchWindow.SwitchToWindow(driver, action);
                        }
                        else if (KeyName == "Wait")
                        {
                            try
                            {
                                Console.WriteLine("Waiting...");
                                string timeInSecondsAsString = table.Rows[i]["Time(In Seconds)"].ToString();
                                if (!string.IsNullOrEmpty(timeInSecondsAsString))
                                {
                                    int timeInSeconds = int.Parse(timeInSecondsAsString);
                                    Thread.Sleep(timeInSeconds * 1000);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    //driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath("Username_InputBox", "Login"))).SendKeys("Support2");
                    //driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath("Password_InputBox", "Login"))).SendKeys("Nirvana@1");
                    //Wait(5000);
                    //driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath("Login_Button", "Login"))).Click();
                    //Wait(10000);
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception at RunOpenFinTest Method :" + ex.Message);
                    }
                }
            }
            return new TestResult();
        }

        static void StartSamsaraApplication()
        {
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {
                    Wait(5000);
                    ProcessStartInfo StartChromeExe = new ProcessStartInfo();
                    StartChromeExe.FileName = "Webapplication.bat";
                    StartChromeExe.WorkingDirectory = ConfigurationManager.AppSettings["ChromeDriverExePath"];
                    StartChromeExe.WindowStyle = ProcessWindowStyle.Minimized;
                    Process ChromeProcess = new Process();
                    ChromeProcess.StartInfo = StartChromeExe;
                    ChromeProcess.Start();
                    Wait(35000);
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw;
                }
            }
        }



        public static WebDriver InitializeDriver()
        {

            WebDriver driver = null;
            try
            {
                ChromeOptions options = new ChromeOptions();
                options.DebuggerAddress = "localhost:8084";
                driver = new RemoteWebDriver(new Uri("http://localhost:9515"), options);
                return driver;
            }
            catch (Exception ex)
            {
                try
                {
                    driver = restartChromeDriver(driver);
                    SamsaraHelperClass.CaptureMyScreen("DriverIssue", ApplicationArguments.TestCaseToBeRun);
                    return driver;
                }
                catch
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw;
                }
            }
            return driver;
        }

        public static WebDriver restartChromeDriver(WebDriver driver)
        {
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {
                    Process[] processes = Process.GetProcessesByName("chromedriver");
                    foreach (Process process in processes)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }
                catch (Exception ex)
                {

                    Log.Error("Error killing the 'chromedriver' process: " + ex.Message);
                }
                Wait(5000);
                try
                {
                    ProcessStartInfo StartChromeExe = new ProcessStartInfo();
                    StartChromeExe.FileName = "chromedriver.exe";
                    StartChromeExe.WorkingDirectory = ConfigurationManager.AppSettings["ChromeDriverExePath"];
                    StartChromeExe.WindowStyle = ProcessWindowStyle.Minimized;
                    StartChromeExe.Arguments = "--port=9515";
                    Process ChromeProcess = new Process();
                    ChromeProcess.StartInfo = StartChromeExe;
                    ChromeProcess.Start();
                    Wait(5000);
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw;
                }
            }
            try
            {
                ChromeOptions options = new ChromeOptions();
                options.DebuggerAddress = "localhost:8084";
                driver = new RemoteWebDriver(new Uri("http://localhost:9515"), options);
            }
            catch { }
            return driver;
        
        }

        private static void SelectTrade(DataSet TestCaseSheet, string StepName, WebDriver driver, DataTable UIData)
        {
            try
            {
                SwitchWindow.SwitchToWindow(driver, DataTables[StepName].Rows[0]["Action"].ToString());
                //DataTable dtBlotter = UIData;
                //DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), TestCaseSheet.Tables[0].Rows[0]);
                //int index = dtBlotter.Rows.IndexOf(dtRow);
                int index = VerifyData(TestCaseSheet, UIData) + 1;
                string id = "#cellid0r" + index + "c0";
                IWebElement element = driver.FindElement(By.CssSelector(id));
                Actions actions = new Actions(driver);

                actions.MoveToElement(element).Perform();
                actions.Click(element).Perform();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured at SelectTrade Method: " + ex.Message);
            }


        }

        public static int VerifyData(DataSet dataset, DataTable datatable)
        {
            try
            {
                // Iterate through each row in the dataset
                foreach (DataRow datasetRow in dataset.Tables[0].Rows)
                {
                    int rowcount = 0;
                    foreach (DataRow datatableRow in datatable.Rows)
                    {
                        bool isMatching = true;
                        rowcount += 1;
                        // Compare the data in columns with the same name
                        foreach (DataColumn column in dataset.Tables[0].Columns)
                        {
                            if (datasetRow[column.ColumnName].ToString() != null)
                            {


                                if (datatable.Columns.Contains(column.ColumnName))
                                {
                                    try
                                    {
                                        if (datasetRow[column.ColumnName].Equals(datatableRow[column.ColumnName]))
                                        {
                                            continue;
                                        }
                                    }
                                    catch
                                    {

                                        if (IsDecimal(datasetRow[column.ColumnName]))
                                        {

                                            decimal decimalValueFromExcel = (decimal)datasetRow[column.ColumnName];
                                            decimal decimalValueFromGrid = (decimal)datatableRow[column.ColumnName];
                                            if (!(Convert.ToInt32(decimalValueFromExcel) == Convert.ToInt32(decimalValueFromGrid)))
                                            {
                                                isMatching = false;
                                                break;
                                            }

                                        }

                                        //Checking Blotter grid contains Sheet data or not
                                        if (!(datatableRow[column.ColumnName].ToString().Contains(datasetRow[column.ColumnName].ToString())))
                                        {
                                            isMatching = false;
                                            break;
                                        }

                                    }

                                }

                                else
                                {
                                    // Compare the data in columns with different names using the mapping
                                    //foreach (var entry in columnMapping)
                                    //{
                                    //    string columnFromExcelSheet = entry.Key;
                                    //    string columnFromUIDataTable = entry.Value;
                                    //    try
                                    //    {
                                    //        if (!datasetRow[columnFromExcelSheet].Equals(datatableRow[columnFromUIDataTable]))
                                    //        {
                                    //            if (IsDecimal(datasetRow[columnFromExcelSheet]))
                                    //            {

                                    //                decimal decimalValueFromExcel = (decimal)datasetRow[columnFromExcelSheet];
                                    //                decimal decimalValueFromGrid = (decimal)datatableRow[columnFromUIDataTable];
                                    //                if (!(Convert.ToInt32(decimalValueFromExcel) == Convert.ToInt32(decimalValueFromGrid)))
                                    //                {
                                    //                    isMatching = false;
                                    //                    break;
                                    //                }

                                    //            }

                                    //            //Checking Blotter grid contains Sheet data or not
                                    //            if (!(datatableRow[columnFromExcelSheet].ToString().Contains(datasetRow[columnFromUIDataTable].ToString())))
                                    //            {
                                    //                isMatching = false;
                                    //                break;
                                    //            }
                                    //        }

                                    //    }
                                    //    catch { }
                                    //}
                                }
                            }



                        }
                        // If all columns (same name and different name) match, consider the rows as matching
                        if (isMatching)
                        {

                            return rowcount;

                        }

                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return 0;
        }




        private static List<String> VerifyData(DataTable dTable, DataTable dtBlotter, Dictionary<string, string> columnMapping = null)
        {
            List<String> errors = new List<String>();
            try
            {
                //List<string> columns = new List<string>();
                foreach (DataRow dTableRow in dTable.Rows)
                {
                    foreach (DataRow dtBlotterRow in dtBlotter.Rows)
                    {
                        //bool isMatching = true;

                        string errorMessage;
                        // Compare the data in columns with the same name
                        foreach (DataColumn column in dTable.Columns)
                        {
                            if (dTableRow[column.ColumnName].ToString() == "null")
                            {
                                break;
                            }

                            else if (dtBlotter.Columns.Contains(column.ColumnName))
                            {
                                try
                                {
                                    if (dTableRow[column.ColumnName].Equals(dtBlotterRow[column.ColumnName]))
                                    {
                                        break;
                                    }
                                }

                                catch
                                {

                                    if (IsDecimal(dTableRow[column.ColumnName]))
                                    {

                                        decimal decimalValueFromExcel = (decimal)dTableRow[column.ColumnName];
                                        decimal decimalValueFromGrid = (decimal)dtBlotterRow[column.ColumnName];
                                        if (!(Convert.ToInt32(decimalValueFromExcel) == Convert.ToInt32(decimalValueFromGrid)))
                                        {
                                            errorMessage = "The Row : " + dTableRow + " Value : " + dTableRow[column.ColumnName] + " doesn't Match";
                                            errors.Add(errorMessage);
                                            break;
                                        }

                                    }



                                    if (!(dtBlotterRow[column.ColumnName].ToString().Contains(dTableRow[column.ColumnName].ToString())))
                                    {
                                        errorMessage = "The Row : " + dTableRow + " Value : " + dTableRow[column.ColumnName] + " doesn't Match";
                                        errors.Add(errorMessage);
                                        break;
                                    }

                                }

                            }

                            else
                            {


                                string columnFromExcelSheet = column.ColumnName.ToString();
                                string columnFromUIDataTable = columnMapping[columnFromExcelSheet];

                                try
                                {
                                    if (dTableRow[columnFromExcelSheet].Equals(dtBlotterRow[columnFromUIDataTable]))
                                    {
                                        //isMatching = false;
                                        break;
                                    }
                                }

                                catch
                                {
                                    if (IsDecimal(dTableRow[columnFromExcelSheet]))
                                    {

                                        decimal decimalValueFromExcel = (decimal)dTableRow[columnFromExcelSheet];
                                        decimal decimalValueFromGrid = (decimal)dtBlotterRow[columnFromUIDataTable];
                                        if (!(Convert.ToInt32(decimalValueFromExcel) == Convert.ToInt32(decimalValueFromGrid)))
                                        {
                                            errorMessage = "The Row : " + dTableRow + " Value : " + dTableRow[column.ColumnName] + " doesn't Match";
                                            errors.Add(errorMessage);
                                            break;
                                        }

                                    }



                                    if (!(dtBlotterRow[columnFromExcelSheet].ToString().Contains(dTableRow[columnFromUIDataTable].ToString())))
                                    {
                                        errorMessage = "The Row : " + dTableRow + " Value : " + dTableRow[column.ColumnName] + " doesn't Match";
                                        errors.Add(errorMessage);
                                        break;
                                    }
                                }




                            }


                        }




                    }
                }

                return errors;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errors;
        }
        public static bool IsDecimal(object value)
        {
            if (value == null) return false;
            if (value is decimal) return true;

            string strValue = value.ToString().Trim();
            decimal result;
            return decimal.TryParse(strValue, out result);
        }

        public static Dictionary<string, string> CreateColumnMapping(string filePath, string sheetName)
        {
            var columnMapping = new Dictionary<string, string>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet != null)
                {
                    int rowCount = worksheet.Dimension.Rows;
                    int columnCount = worksheet.Dimension.Columns;

                    // Check if the worksheet contains exactly two columns
                    if (columnCount == 2)
                    {
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var key = worksheet.Cells[row, 1].Text;
                            var value = worksheet.Cells[row, 2].Text;

                            // Add key-value pair to the dictionary
                            if (!string.IsNullOrEmpty(key))
                            {
                                columnMapping[key] = value;
                            }
                        }

                    }
                    else
                    {
                        throw new Exception("The worksheet does not contain exactly two columns.");
                    }

                }

                else
                {
                    throw new Exception("Worksheet not found.");
                }
            }

            return columnMapping;
        }

        private static bool OpenTabOfRTPNL(string tabName, Dictionary<string, object> dict, WebDriver driver)
        {
            string path;
            int str = 1;
            bool flag = false;
            while (true)
            {
                string divElement = null;
                try
                {
                    path = dict["TabPath"].ToString().Replace("str", str.ToString());

                    divElement = driver.FindElement(By.XPath(path)).GetAttribute("title").ToString();
                }
                catch (Exception)
                {
                    Console.WriteLine("Tab not found");
                    flag = false;
                    break;
                }
                if (divElement == tabName)
                {
                    Console.WriteLine("Tab founded " + divElement);
                    driver.FindElement(By.XPath(path)).Click();
                    flag = true;
                    break;
                }
                str++;

            }
            return flag;
        }

        private static bool NewTabNameRTPNL(string tabName, string description, Dictionary<string, object> dict, WebDriver driver)
        {
            SwitchWindow.SwitchToWindow(driver, dict["DefaultTab"].ToString(), true);

            IWebElement name = driver.FindElement(By.XPath(dict["InputNameBox"].ToString()));
            Actions action = new Actions(driver);
            name.Clear();
            action.Click(name).SendKeys(tabName)
                .Build()
                .Perform();

            if (!String.IsNullOrEmpty(description))
            {
                IWebElement desc = driver.FindElement(By.XPath(dict["InputDescriptionBox"].ToString()));
                action.Click(desc).SendKeys(Keys.Control + "a")
                                          .SendKeys(Keys.Backspace).SendKeys(description)
                                          .Perform();
            }
            if (!TestCaseSheet_Temp.Tables[0].Columns.Contains("Save?")) 
            {
                driver.FindElement(By.XPath(dict["Button_Save"].ToString())).Click();
                Wait(6000);
                SwitchWindow.SwitchToWindow(driver, tabName, true);
                return true;
            }
            else if (TestCaseSheet_Temp.Tables[0].Rows[0]["Save?"].ToString().ToUpper().Equals("TRUE"))
            {
                driver.FindElement(By.XPath(dict["Button_Save"].ToString())).Click();
                Wait(6000);
                SwitchWindow.SwitchToWindow(driver, tabName, true);
                return true;
            }
            else
            {
                action.MoveToElement(name)
               .KeyDown(Keys.Control)
               .SendKeys("a")
               .KeyUp(Keys.Control).SendKeys(Keys.Backspace)
               .Perform();
                return false;
            }
        }

        private static DataTable changeDataSheet(DataTable dataTable, string valueToChange, string newValue, string StepName) 
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (dataTable.Rows[i]["Steps"].ToString().Equals("GetRTPNLGridData")) {
                    continue;
                }
                if (dataTable.Rows[i]["Action"].ToString().Equals(valueToChange)) {
                    dataTable.Rows[i]["Action"] = "Nirvana," + newValue;  
                }
                if (!String.IsNullOrEmpty(dataTable.Rows[i]["Action"].ToString()))
                {
                    if (StepName.Contains(dataTable.Rows[i]["Action"].ToString()))
                    {
                        dataTable.Rows[i]["Action"] = TestCaseSheet_Temp.Tables[0].Rows[0]["GridDataWidgetName"].ToString();
                    }
                }
            }
            return dataTable;   
        }

        public static HashSet<string> GetDropdownTexts(WebDriver driver, string ClassName, string keyToClick = "")
        {
            HashSet<string> texts = new HashSet<string>();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement parent = wait.Until(d => d.FindElement(By.ClassName(ClassName)));
            int retries = 5;
            while (retries > 0)
            {
                try
                {
                    // Re-fetch child elements each time to avoid StaleElementReferenceException
                    var childElements = parent.FindElements(By.XPath(".//*"));

                    foreach (var element in childElements)
                    {
                        if (!string.IsNullOrWhiteSpace(element.Text))
                        {
                            texts.Add(element.Text.Trim());
                        }
                    }

                    break; // Success, exit loop
                }
                catch (StaleElementReferenceException)
                {
                    retries--;
                    parent = driver.FindElement(By.ClassName(ClassName)); // Re-fetch parent
                }
            }

            return texts;
        }

        public static void ClickElementWithTextUnderClass(WebDriver driver, string className, string visibleText)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int retries = 7;

            while (retries > 0)
            {
                try
                {
                    // Build XPath dynamically to find the element with text "kartik" under the given class
                    string xpath = string.Format("//div[contains(@class, '{0}')]//*[contains(text(), '{1}')]", className, visibleText);

                    // Wait until the element is present
                    IWebElement element = wait.Until(
                        delegate(IWebDriver d)
                        {
                            return d.FindElement(By.XPath(xpath));
                        });

                    // Click the element
                    element.Click();
                    return; // success, exit
                }
                catch (StaleElementReferenceException)
                {
                    retries--;
                    Thread.Sleep(500); // let the DOM stabilize
                }
                catch (NoSuchElementException)
                {
                    retries--;
                    Thread.Sleep(500); // element may not be ready yet
                }
                catch (WebDriverException e)
                {
                    Console.WriteLine("WebDriver Exception: " + e.Message);
                    retries--;
                    Thread.Sleep(500);
                }
            }

            throw new Exception("Element with text '" + visibleText + "' under class '" + className + "' could not be clicked.");
        }

        public static void ClickDeleteIconForText(IWebDriver driver, string parentClass, string matchText, string siblingClass)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int retries = 5;

            while (retries > 0)
            {
                try
                {
                    // XPath:
                    // 1. Find the div inside qtt-hamburger-dropdown that has text 'kartik'
                    // 2. Navigate to its following-sibling with class hamburger-delete
                    string xpath = string.Format(
                        "//div[contains(@class, '{0}')]//div[contains(text(), '{1}')]/following-sibling::*[contains(@class, '{2}')]",
                        parentClass, matchText, siblingClass);

                    // Wait and find the sibling element
                    IWebElement deleteElement = wait.Until(
                        delegate(IWebDriver d)
                        {
                            return d.FindElement(By.XPath(xpath));
                        });

                    // Click it
                    deleteElement.Click();
                    return; // success
                }
                catch (StaleElementReferenceException)
                {
                    retries--;
                    Thread.Sleep(500); // re-fetch everything on next iteration
                }
                catch (NoSuchElementException)
                {
                    retries--;
                    Thread.Sleep(500);
                }
                catch (WebDriverException e)
                {
                    Console.WriteLine("WebDriver error: " + e.Message);
                    retries--;
                    Thread.Sleep(500);
                }
            }

            throw new Exception("Could not find and click delete icon for text '" + matchText + "'.");
        }


        static DataSet ds = new DataSet();
        private static DataTable FetchAndVerifyDataFromGrid(DataTable dataTable, Dictionary<string, object> MappingDictionary, WebDriver driver, string StepName, DataSet TestCaseSheet, ref TestResult _res, bool temp = false)
        {
            DataTable Data = new DataTable();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            Actions actions = new Actions(driver);
            var dict = MappingDictionary;
            var table = DataTables[StepName];
            Dictionary<string, string> columnValues = new Dictionary<string, string>();
            DataTable ExcelData = null;
            if (ConfigurationManager.AppSettings["RTPNLVerificationSteps"].ToString().Split(',').Contains(StepName))
            {
                if (TestCaseSheet_Temp.Tables[0].Columns.Contains("TabName")) {
                    if(!String.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["TabName"].ToString())){
                        dataTable = changeDataSheet(dataTable, "Nirvana,PM", TestCaseSheet_Temp.Tables[0].Rows[0]["TabName"].ToString(), StepName);
                    }
                    TestCaseSheet.Tables[0].Columns.Remove("TabName");
                    TestCaseSheet.Tables[0].Columns.Remove("GridDataWidgetName");
                }
            
            }
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {

                if (!string.IsNullOrEmpty(dataTable.Rows[i]["Steps"].ToString()) &&  dataTable.Rows[i]["Steps"].ToString().Contains("IncreaseOrUnwindPositionRTPNL"))
                {
                    try
                    {               
                      SamsaraGridOperationHelper.ClickElement(driver, table.Rows[i]["Steps"].ToString(), ref dict, table.Rows[i]["Action"].ToString(), TestCaseSheet.Tables[0]);                      
                    }
                    catch (Exception ex)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                        Console.WriteLine("There is an issue while performing " + StepName + "  : " + ex.Message);
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                        if (rethrow)
                            throw;
                       
                    }

                }
                else if (table.Rows[i]["Steps"].ToString().Equals("TradeAttributeWindow"))
                {
                    IList<IWebElement> divElements = null;
                    IWebElement element = driver.FindElement(By.ClassName(dict["TradeAttributeClass"].ToString()));
                    divElements = element.FindElements(By.Id(dict["AttributesID"].ToString()));
                    Actions action = new Actions(driver);
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 01") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 01"].ToString()))
                    {
                        action.Click(divElements[0]).KeyDown(Keys.Control)
                        .SendKeys("a")
                        .KeyUp(Keys.Control).SendKeys(Keys.Backspace)
                        .SendKeys(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 01"].ToString()).SendKeys(Keys.Tab)
                        .Build()
                        .Perform();
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 02") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 02"].ToString()))
                    {
                        action.Click(divElements[1]).KeyDown(Keys.Control)
                        .SendKeys("a")
                        .KeyUp(Keys.Control).SendKeys(Keys.Backspace)
                        .SendKeys(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 02"].ToString()).SendKeys(Keys.Tab)
                        .Build()
                        .Perform();
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 03") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 03"].ToString()))
                    {
                        action.Click(divElements[2]).KeyDown(Keys.Control)
                        .SendKeys("a")
                        .KeyUp(Keys.Control).SendKeys(Keys.Backspace)
                                    .SendKeys(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 03"].ToString()).SendKeys(Keys.Tab)
                                    .Build()
                                    .Perform();
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 04") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 04"].ToString()))
                    {
                        action.Click(divElements[3]).KeyDown(Keys.Control)
                        .SendKeys("a")
                        .KeyUp(Keys.Control).SendKeys(Keys.Backspace)
                        .SendKeys(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 04"].ToString()).SendKeys(Keys.Tab)
                        .Build()
                        .Perform();
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 05") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 05"].ToString()))
                    {
                        action.Click(divElements[4]).KeyDown(Keys.Control)
                        .SendKeys("a")
                        .KeyUp(Keys.Control).SendKeys(Keys.Backspace)
                        .SendKeys(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 05"].ToString()).SendKeys(Keys.Tab)
                        .Build()
                        .Perform();
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 06") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 06"].ToString()))
                    {
                        action.Click(divElements[5]).KeyDown(Keys.Control)
                        .SendKeys("a")
                        .KeyUp(Keys.Control).SendKeys(Keys.Backspace)
                        .SendKeys(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 06"].ToString()).SendKeys(Keys.Tab)
                        .Build()
                        .Perform();
                    }
                    
                    IWebElement suggest = null;
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 01 suggestion") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 01 suggestion"].ToString()))
                    {
                        action.Click(divElements[0])
                                    .Build()
                                    .Perform();
                        Wait(1500);
                        suggest = driver.FindElement(By.ClassName(dict["suggestTradeAttribute"].ToString()));
                        if (suggest.Text.ToString().ToLower().Equals(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 01 suggestion"].ToString().ToLower()))
                        {
                            Console.WriteLine(suggest.Text + " Trade attribute suggestion verified");
                            suggest.Click();
                        }
                        else
                        {
                            throw new Exception(suggest.Text + " Trade attribute suggestion not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 02 suggestion") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 02 suggestion"].ToString()))
                    {
                        action.Click(divElements[1])
                                    .Build()
                                    .Perform();
                        Wait(1500);
                        suggest = driver.FindElement(By.ClassName(dict["suggestTradeAttribute"].ToString()));
                        if (suggest.Text.ToString().ToLower().Equals(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 02 suggestion"].ToString().ToLower()))
                        {
                            Console.WriteLine(suggest.Text + " Trade attribute suggestion verified");
                            suggest.Click();
                        }
                        else
                        {
                            throw new Exception(suggest.Text + " Trade attribute suggestion not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 03 suggestion") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 03 suggestion"].ToString()))
                    {
                        action.Click(divElements[2])
                                    .Build()
                                    .Perform();
                        Wait(1500);
                        suggest = driver.FindElement(By.ClassName(dict["suggestTradeAttribute"].ToString()));
                        if (suggest.Text.ToString().ToLower().Equals(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 03 suggestion"].ToString().ToLower()))
                        {
                            Console.WriteLine(suggest.Text + " Trade attribute suggestion verified");
                            suggest.Click();
                        }
                        else
                        {
                            throw new Exception(suggest.Text + " Trade attribute suggestion not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 04 suggestion") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 04 suggestion"].ToString()))
                    {
                        action.Click(divElements[3])
                                    .Build()
                                    .Perform();
                        suggest = driver.FindElement(By.ClassName(dict["suggestTradeAttribute"].ToString()));
                        if (suggest.Text.ToString().ToLower().Equals(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 04 suggestion"].ToString().ToLower()))
                        {
                            Console.WriteLine(suggest.Text + " Trade attribute suggestion verified");
                            suggest.Click();
                        }
                        else
                        {
                            throw new Exception(suggest.Text + " Trade attribute suggestion not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 05 suggestion") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 05 suggestion"].ToString()))
                    {
                        action.Click(divElements[4])
                                    .Build()
                                    .Perform();
                        Wait(1500);
                        suggest = driver.FindElement(By.ClassName(dict["suggestTradeAttribute"].ToString()));
                        if (suggest.Text.ToString().ToLower().Equals(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 05 suggestion"].ToString().ToLower()))
                        {
                            Console.WriteLine(suggest.Text + " Trade attribute suggestion verified");
                            suggest.Click();
                        }
                        else
                        {
                            throw new Exception(suggest.Text + " Trade attribute suggestion not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 06 suggestion") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 06 suggestion"].ToString()))
                    {
                        action.Click(divElements[5])
                                    .Build()
                                    .Perform();
                        Wait(1500);
                        suggest = driver.FindElement(By.ClassName(dict["suggestTradeAttribute"].ToString()));
                        if (suggest.Text.ToString().ToLower().Equals(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 06 suggestion"].ToString().ToLower()))
                        {
                            Console.WriteLine(suggest.Text + " Trade attribute suggestion verified");
                            suggest.Click();
                        }
                        else
                        {
                            throw new Exception(suggest.Text + " Trade attribute suggestion not verified");
                        }
                    }
                   
                    action = null;
                    IWebElement L1 = driver.FindElement(By.ClassName(dict["L1stripAttributeWindow"].ToString()));
                    var divs = L1.FindElements(By.XPath("//div[h5[@class='info-label'] and h4[@class='info-value']]"));

                    foreach (var div in divs)
                    {
                        var label = div.FindElement(By.XPath(".//h5[@class='info-label']")).Text;
                        var value = div.FindElement(By.XPath(".//h4[@class='info-value']")).Text;
                        Console.WriteLine(label + " : " + value);
                        foreach (String key in columnValues.Keys) 
                        {
                            if (key.ToString().ToUpper().Equals(label.ToString().ToUpper()))
                            {
                                if (columnValues[key].ToString().ToUpper().Equals(value.Replace(",", "").ToString().ToUpper()))
                                {
                                    Console.WriteLine(columnValues[key] + " has " + value);
                                }
                                else 
                                {
                                    throw new Exception(columnValues[key] + " doesnot have " + value);
                                }
                            }
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 01 value") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 01 value"].ToString()))
                    {
                        Console.WriteLine(divElements[0].GetAttribute("value") + " " + TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 01 value"].ToString().ToUpper());
                        if (!divElements[0].GetAttribute("value").ToString().ToUpper().Equals(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 01 value"].ToString().ToUpper())) 
                        {
                            throw new Exception(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 01 value"].ToString() + " value not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 02 value") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 02 value"].ToString()))
                    {
                        if (!divElements[1].GetAttribute("value").ToString().ToUpper().Equals(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 02 value"].ToString().ToUpper()))
                        {
                            throw new Exception(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 02 value"].ToString() + " value not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 03 value") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 03 value"].ToString()))
                    {
                        if (!divElements[2].GetAttribute("value").ToString().ToUpper().Equals(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 03 value"].ToString().ToUpper()))
                        {
                            throw new Exception(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 03 value"].ToString() + " value not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 04 value") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 04 value"].ToString()))
                    {
                        if (!divElements[3].GetAttribute("value").ToString().ToUpper().Equals(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 04 value"].ToString().ToUpper()))
                        {
                            throw new Exception(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 04 value"].ToString() + " value not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 05 value") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 05 value"].ToString()))
                    {
                        if (!divElements[4].GetAttribute("value").ToString().ToUpper().Equals(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 05 value"].ToString().ToUpper()))
                        {
                            throw new Exception(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 05 value"].ToString() + " value not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 06") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 06 value"].ToString()))
                    {
                        if (!divElements[5].GetAttribute("value").ToString().ToUpper().Equals(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 06 value"].ToString().ToUpper()))
                        {
                            throw new Exception(TestCaseSheet.Tables[0].Rows[0]["Trade Attribute 06 value"].ToString() + " value not verified");
                        }
                    }
                    //
                    if (TestCaseSheet.Tables[0].Columns.Contains("CloseOrSubmit") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["CloseOrSubmit"].ToString()))
                    {
                        if (TestCaseSheet.Tables[0].Rows[0]["CloseOrSubmit"].ToString().ToLower().Contains("close"))
                        {
                            PerformActionThroughClass(driver, "Click", dict["CloseButtonAttribute"].ToString(), TestCaseSheet.Tables[0].Rows[0], "CloseButtonAttribute");
                        }
                        else 
                        {
                            PerformActions(driver, "Click", dict["SubmitAttribute"].ToString(), TestCaseSheet.Tables[0].Rows[0], "SubmitAttribute");
                        }
                    }

                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("NirvanaDialogueBoxHandler"))
                {
                    try
                    {
                        bool flag = SwitchWindow.SwitchToWindow(driver, "Blotter");
                        if (!flag)
                            flag = SwitchWindow.SwitchToWindow(driver, "- Trading Ticket");
                        if (!flag)
                            SwitchWindow.SwitchToWindow(driver, "Trading Ticket");
                        foreach (DataRow dr in TestCaseSheet_Temp.Tables[0].Rows)
                        {
                            if (!string.IsNullOrEmpty(dr["Message"].ToString()) && TestCaseSheet.Tables[0].Columns.Contains("VerifyBoxCompulsory") && !string.IsNullOrEmpty(dr["VerifyBoxCompulsory"].ToString())) 
                            {
                                IWebElement bodyElement = driver.FindElement(By.XPath(dict["NewTradePopup"].ToString()));
                                string newText = Regex.Replace(bodyElement.Text.ToLower(), @"[\s,]+()", "");
                                string str = Regex.Replace(columnValues["VerifyContainslblMsg"].ToString().ToLower(), @"[\s,]+()", "");
                                string srt = bodyElement.Text.ToLower().ToString();
                                double similarity = CalculateSimilarity(srt, columnValues["VerifyContainslblMsg"].ToLower());
                                if (similarity >= 0.50 || newText.ToLower().Contains(str.ToLower()))
                                {
                                    Console.WriteLine("Prompt message verified");
                                }
                                else
                                {
                                    throw new Exception("Prompt message is not Verifying");
                                }
                            }
                            string action = string.Empty;
                            for (int k = 1; k < TestCaseSheet_Temp.Tables[0].Columns.Count; k++) 
                            {
                                if (dr[k].ToString().ToLower().Contains("no") || dr[k].ToString().ToLower().Contains("cancel"))
                                {
                                    action = "no";
                                }
                                else if (!string.IsNullOrEmpty(dr[k].ToString()))
                                {
                                    action = "yes";
                                }
                            }
                            if (action.Equals("no"))
                            {
                                PerformActions(driver, "Click", dict["AllowTrade_No"].ToString(), dr, "AllowTrade_No");
                            }
                            else 
                            {
                                PerformActions(driver, "Click", dict["AllowTrade_Yes"].ToString(), dr, "AllowTrade_Yes");
                            }
                        }
                    }
                    catch (Exception ex) 
                    {
                        if (ex.Message.ToString().Contains("Prompt message is not Verifying"))
                            throw ex;
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (!string.IsNullOrEmpty(dataTable.Rows[i]["Steps"].ToString()) && dataTable.Rows[i]["Steps"].ToString().Contains("CloseRTPNL"))
                {
                    try
                    {
                        CloseRTPNL(driver, ref dict, TestCaseSheet.Tables[0]);
                    }
                    catch (Exception ex)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                        Console.WriteLine("There is an issue while performing " + StepName + "  : " + ex.Message);
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                        if (rethrow)
                            throw;

                    }

                }
                else if (table.Rows[i]["Steps"].ToString().Equals("ShiftWindow"))
                {
                    string val = table.Rows[i]["Action"].ToString();
                    SamsaraHelperClass.MoveWindow(ref driver, ref val);
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("GroupingWidget"))
                {
                    IWebElement element = driver.FindElement(By.ClassName(dict["ChipList"].ToString()));
                    IReadOnlyCollection<IWebElement> childs = null;
                    Actions action = new Actions(driver);
                    if (!string.IsNullOrEmpty(columnValues["Chip 1"].ToString())) 
                    {
                        childs = element.FindElements(By.ClassName("chip"));
                        IWebElement chip1 = null;
                        foreach (var ele in childs) 
                        {
                            if (ele.Text.ToString().ToLower().Equals(columnValues["Chip 1"].ToString().ToLower())) 
                            {
                                chip1 = ele;
                                break;
                            }
                        }
                        action.MoveToElement(chip1).Perform();
                        action.ClickAndHold(chip1);

                        IWebElement targetElement = driver.FindElement(By.XPath(dict["chip1"].ToString()));
                        action.MoveToElement(targetElement).Perform();
                        action.Release().Perform();
                        Wait(4000);
                
                    }
                    if (!string.IsNullOrEmpty(columnValues["Chip 2"].ToString()))
                    {
                        childs = element.FindElements(By.ClassName("chip"));
                        IWebElement chip2 = null;
                        foreach (var ele in childs)
                        {
                            if (ele.Text.ToString().ToLower().Equals(columnValues["Chip 2"].ToString().ToLower()))
                            {
                                chip2 = ele;
                                break;
                            }
                        }
                        action.MoveToElement(chip2).Perform();
                        action.ClickAndHold(chip2);

                        IWebElement targetElement = driver.FindElement(By.XPath(dict["chip2"].ToString()));
                        action.MoveToElement(targetElement).Perform();
                        action.Release().Perform();
                        Wait(4000);
                
                    }
                    if (!string.IsNullOrEmpty(columnValues["Chip 3"].ToString()))
                    {
                        childs = element.FindElements(By.ClassName("chip"));
                        IWebElement chip3 = null;
                        foreach (var ele in childs)
                        {
                            if (ele.Text.ToString().ToLower().Equals(columnValues["Chip 3"].ToString().ToLower()))
                            {
                                chip3 = ele;
                                break;
                            }
                        }
                        action.MoveToElement(chip3).Perform();
                        action.ClickAndHold(chip3);

                        IWebElement targetElement = driver.FindElement(By.XPath(dict["chip3"].ToString()));
                        action.MoveToElement(targetElement).Perform();
                        action.Release().Perform();
                        Wait(4000);
                    }
                    if (!string.IsNullOrEmpty(columnValues["Verify Chip Values"].ToString()))
                    {
                        foreach (string str in columnValues["Verify Chip Values"].ToString().Split(',')) 
                        {
                            bool flag = false;
                            if (str.ToLower().Equals(driver.FindElement(By.XPath(dict["chip3"].ToString())).Text.ToString().ToLower()))
                            {
                                flag = true;
                            }
                            if (str.ToLower().Equals(driver.FindElement(By.XPath(dict["chip1"].ToString())).Text.ToString().ToLower()))
                            {
                                flag = true;
                            }
                            if (str.ToLower().Equals(driver.FindElement(By.XPath(dict["chip2"].ToString())).Text.ToString().ToLower()))
                            {
                                flag = true;
                            }
                            if (!flag) 
                            {
                                throw new Exception("Chips values are incorrect in Excel sheet");
                            }
                        }
                        Wait(2000);
                    }
                    IWebElement chipExpad = driver.FindElement(By.XPath(dict["ChipExpand"].ToString()));
                    if (!string.IsNullOrEmpty(columnValues["MakeColumFavourite"].ToString()))
                    {
                        chipExpad.Click();
                        verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, 75);
                        element = driver.FindElement(By.ClassName(dict["Allchipclass"].ToString()));
                        childs = element.FindElements(By.TagName("li"));
                        foreach (string str in columnValues["MakeColumFavourite"].ToString().Split(',')) 
                        { 
                            foreach (var ele in childs) 
                            {
                                if (ele.Text.ToString().ToLower().Equals(str.ToLower())) 
                                {
                                    string className = ele.GetAttribute("class").ToString();
                                    if (!className.Contains("selected")) 
                                    {
                                        ele.Click();
                                        break;
                                    }
                                }
                            }
                        }
                        verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, -90);
                        chipExpad.Click();
                        Wait(4000);
                    }

                    if (!string.IsNullOrEmpty(columnValues["MakeColumnUnfavourite"].ToString()))
                    {
                        chipExpad.Click();
                        verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, 75);
                        element = driver.FindElement(By.ClassName(dict["Allchipclass"].ToString()));
                        childs = element.FindElements(By.TagName("li"));
                        foreach (string str in columnValues["MakeColumnUnfavourite"].ToString().Split(','))
                        {
                            foreach (var ele in childs)
                            {
                                if (ele.Text.ToString().ToLower().Equals(str.ToLower()))
                                {
                                    string className = ele.GetAttribute("class").ToString();
                                    if (className.Contains("selected"))
                                    {
                                        ele.Click();
                                        break;
                                    }
                                }
                            }
                        }
                        verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, -90);
                        chipExpad.Click();
                        Wait(4000);
                    }
                    
                    if (!string.IsNullOrEmpty(columnValues["ColumnsToAdd"].ToString()))
                    {
                        string[] columns = columnValues["ColumnsToAdd"].ToString().Split(',');
                        element = driver.FindElement(By.ClassName(dict["ClassOfColumnSelector"].ToString()));
                        verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, 80);
                        element.Click();
                        IWebElement SelectAll = driver.FindElement(By.ClassName("MuiButtonBase-root"));
                        while (true)
                        {
                            Console.WriteLine(SelectAll.GetAttribute("class").ToString());
                            if (SelectAll.GetAttribute("class").ToString().ToLower().Contains("checked") || SelectAll.GetAttribute("class").ToString().ToLower().Contains("indeterminate"))
                            {
                                SelectAll.Click();
                            }
                            else {
                                break;
                            }
                            Wait(1500);
                        }
                        string Col = columnValues["ColumnsToAdd"].ToString().ToUpper();
                        if (!Col.Equals("NONE") && !Col.Equals("SELECTALL"))
                        {
                            foreach (string col in columns)
                            {
                                string valCheckBox = dict["SelectColumnValue"].ToString();
                                valCheckBox = valCheckBox.Replace("Exposure", col);
                                IWebElement checkBox = driver.FindElement(By.XPath(valCheckBox));
                                checkBox.Click();
                                Thread.Sleep(2000);
                            }
                        }

                        if (Col.Equals("SELECTALL"))
                        {
                            SelectAll.Click();
                        }

                        element.Click();
                        verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, -100);
                        
                    }
                    if (TestCaseSheet_Temp.Tables[0].Columns.Contains("ColumnsToAdd2") && !string.IsNullOrEmpty(columnValues["ColumnsToAdd2"].ToString()))
                    {
                        string[] columns = columnValues["ColumnsToAdd2"].ToString().Split(',');
                        var elems = driver.FindElements(By.ClassName(dict["ClassOfColumnSelector"].ToString()));
                        element = elems[1];
                        verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, 80);
                        element.Click();
                        var Select = driver.FindElements(By.ClassName("MuiButtonBase-root"));
                        IWebElement SelectAll = Select[1];
                        while (true)
                        {
                            Console.WriteLine(SelectAll.GetAttribute("class").ToString());
                            if (SelectAll.GetAttribute("class").ToString().ToLower().Contains("checked") || SelectAll.GetAttribute("class").ToString().ToLower().Contains("indeterminate"))
                            {
                                SelectAll.Click();
                            }
                            else
                            {
                                break;
                            }
                            Wait(1500);
                        }
                        string Col = columnValues["ColumnsToAdd"].ToString().ToUpper();
                        if (!Col.Equals("NONE") && !Col.Equals("SELECTALL"))
                        {
                            foreach (string col in columns)
                            {
                                string valCheckBox = dict["SelectColumnValue"].ToString();
                                valCheckBox = valCheckBox.Replace("Exposure", col);
                                IWebElement checkBox = driver.FindElement(By.XPath(valCheckBox));
                                checkBox.Click();
                                Thread.Sleep(2000);
                            }
                        }

                        if (Col.Equals("SELECTALL"))
                        {
                            SelectAll.Click();
                        }

                        element.Click();
                        verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, -120);

                    }
                    if (TestCaseSheet_Temp.Tables[0].Columns.Contains("FlashButton"))
                    {
                        verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, 130);
                        if (!string.IsNullOrEmpty(columnValues["FlashButton"].ToString()))
                        {
                            driver.FindElement(By.Id(dict["flashButton"].ToString())).Click();
                            Wait(1000);
                        }
                        if (!string.IsNullOrEmpty(columnValues["ColumnsToCheckForFlash"].ToString()))
                        {
                            foreach (string str in columnValues["ColumnsToCheckForFlash"].ToString().Split(',')) 
                            {
                                string xpath = dict["flashcolumns"].ToString().Replace("Px Selected Feed (Base)", str);
                                driver.FindElement(By.XPath(xpath)).Click();
                                Wait(1000);
                            }
                        }
                        if (!string.IsNullOrEmpty(columnValues["LinkWidgetColor"].ToString()))
                        {
                            verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, 30);
                            driver.FindElement(By.Id(dict["WidgetLink"].ToString())).Click();
                            Wait(1000);
                            string xpath = dict["widgetColor"].ToString().Replace("color", columnValues["LinkWidgetColor"].ToString());
                            driver.FindElement(By.XPath(xpath)).Click();
                        }
                        verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, -130);
                        
                    }
                    if (TestCaseSheet_Temp.Tables[0].Columns.Contains("LinkColour"))
                    {
                        if (!string.IsNullOrEmpty(columnValues["LinkColour"].ToString()))
                        {
                            string colorToSelect = columnValues["LinkColour"].ToString().Trim().ToUpper();

                            try
                            {
                                verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, 140);
                                IWebElement dropdownButton = driver.FindElement(By.Id(dict["GroupingWidgetLinkDropdown"].ToString()));
                                dropdownButton.Click();
                                Thread.Sleep(500);

                                IList<IWebElement> colorOptions = driver.FindElements(By.XPath(dict["GroupingWidgetLinkColourOptions"].ToString()));

                                bool found = false;

                                foreach (var option in colorOptions)
                                {
                                    string optionText = option.Text.Trim().ToUpper();

                                    if (optionText.Equals(colorToSelect, StringComparison.OrdinalIgnoreCase))
                                    {
                                        option.Click();
                                        Thread.Sleep(500);
                                        found = true;
                                        break;
                                    }
                                }

                                if (!found)
                                {
                                    CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Color '" + colorToSelect + "' not found in dropdown.");
                                    throw new Exception("Could not find the link color option: " + colorToSelect);
                                }
                            }
                            catch (Exception ex)
                            {
                                CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error selecting color: " + columnValues["LinkColour"]);
                                throw new Exception("Color selection failed for '" + columnValues["LinkColour"] + "': " + ex.Message);
                            }
                            verticalScroll(driver.FindElement(By.XPath(dict["ScrollerChipWindow"].ToString())), action, null, driver, -140);
                        }
                    }
                    if (TestCaseSheet_Temp.Tables[0].Columns.Contains("VerifyActionButtonVisibility"))
                    {
                        if (!string.IsNullOrEmpty(columnValues["VerifyActionButtonVisibility"].ToString()))
                        {
                            bool flag = false;
                            try
                            {
                                var ele = driver.FindElement(By.CssSelector(dict["CreateGroupWidget"].ToString()));
                                flag = ele.Displayed && ele.Enabled;
                            }
                            catch
                            {
                                flag = false;
                            }
                            if (!columnValues["VerifyActionButtonVisibility"].ToString().ToLower().Equals(flag.ToString().ToLower()))
                            {
                                throw new Exception("Action button is visible/enabled = " + flag + " but excel has " + columnValues["VerifyActionButtonVisibility"]);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(columnValues["NextAction"].ToString()))
                    {
                        if (columnValues["NextAction"].ToString().ToLower().Contains("create")) 
                        {
                            try
                            {
                                driver.FindElement(By.CssSelector(dict["CreateGroupWidget"].ToString())).Click();
                                Wait(1000);
                            }
                            catch 
                            { 
                                Console.WriteLine("Create button is disable."); 
                            }
                        }
                        else if (columnValues["NextAction"].ToString().ToLower().Contains("back")) 
                        {
                            PerformActionThroughClass(driver, "Click", dict["CancelGroupWidget"].ToString(), table.Rows[0], "CancelGroupWidget");
                        }
                        else if (columnValues["NextAction"].ToString().ToLower().Contains("reset"))
                        {
                            PerformActions(driver, "Click", dict["ResetGroupWidget"].ToString(), table.Rows[0], "ResetGroupWidget");
                        }
                    }

                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("RTPNLPopUpVerify") && TestCaseSheet_Temp.Tables[0].Columns.Contains("ActionOnPopUp") && !string.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["ActionOnPopUp"].ToString()))
                {
                    IWebElement activeElement = driver.SwitchTo().ActiveElement();
                    string elementText = activeElement.Text;
                    if (!string.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["VerifyPopUpText"].ToString())) 
                    {
                        if (elementText.ToLower().Contains(TestCaseSheet_Temp.Tables[0].Rows[0]["VerifyPopUpText"].ToString().ToLower()))
                        {
                            Console.WriteLine(TestCaseSheet_Temp.Tables[0].Rows[0]["VerifyPopUpText"].ToString() + " text verified");
                        }
                        else 
                        {
                            throw new Exception(TestCaseSheet_Temp.Tables[0].Rows[0]["VerifyPopUpText"].ToString() + " text not verified");
                        }
                    }
                    if (!string.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["ActionOnPopUp"].ToString()))
                    {
                        if (TestCaseSheet_Temp.Tables[0].Rows[0]["ActionOnPopUp"].ToString().ToLower().Contains("discard"))
                        {
                            PerformActionsThroughID(driver, "Click", dict["DiscardRTPNLclose"].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], "ActionOnPopUp");
                        }
                        if (TestCaseSheet_Temp.Tables[0].Rows[0]["ActionOnPopUp"].ToString().ToLower().Contains("save"))
                        {
                            PerformActionsThroughID(driver, "Click", dict["SaveRTPNLclose"].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], "SaveRTPNLclose");
                        }
                        if (TestCaseSheet_Temp.Tables[0].Rows[0]["ActionOnPopUp"].ToString().ToLower().Contains("cancel"))
                        {
                            PerformActionsThroughID(driver, "Click", dict["CancelPopUp"].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], "CancelPopUp");
                        }
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("VerifyExcelFile"))
                {
                    SwitchWindow.SwitchToWindow(driver, columnValues["TabName"].ToString());
                    PerformActions(driver, "Click", dict["ExcelDropDown"].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], "ExcelDropDown");
                    Wait(3000);
                    PerformActions(driver, "Click", dict["ExcelFileClick"].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], "ExcelFileClick");
                    Wait(3000);
                    if (IsFileLocked(new FileInfo("E:\\" + columnValues["File Name"].ToString())))
                    {
                        Console.WriteLine(columnValues["File Name"].ToString() + " file is opened");
                    }
                    else 
                    {
                        throw new Exception(columnValues["File Name"].ToString() + " file is not opened");
                    }
                    Wait(3000);
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("QTTHamburg"))
                {
                    try
                    {
                        driver.FindElement(By.ClassName(dict["HamburgClassQTT"].ToString()));
                    }
                    catch
                    {
                        driver.FindElement(By.XPath(dict["QTTexpandButton"].ToString())).Click();
                    }
                    Wait(2000);
                    HashSet<string> set = GetDropdownTexts(driver, dict["HamburgClassQTT"].ToString());
                    if (!string.IsNullOrEmpty(columnValues["KeysToVerify"].ToString()))
                    {
                        foreach (string str in columnValues["KeysToVerify"].ToString().Split(','))
                        {
                            if (set.Contains(str))
                            {
                                Console.WriteLine("QTT hamburg has a key with name " + str);
                            }
                            else
                            {
                                throw new Exception("QTT hamburg dont have a key with name " + str);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(columnValues["KeyToDelete"].ToString()))
                    {
                        ClickDeleteIconForText(driver, dict["HamburgClassQTT"].ToString(), columnValues["KeyToDelete"].ToString(), "hamburger-delete");
                        Wait(3000);
                        if (TestCaseSheet_Temp.Tables[0].Columns.Contains("CancelPopup") && !string.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["CancelPopup"].ToString()))
                        {
                            try
                            {
                                driver.FindElement(By.XPath(dict["QTTCancel"].ToString())).Click();
                            }
                            catch
                            {
                                Console.WriteLine("Cancel button not found.");
                            }
                        }
                        else
                        {

                            PerformActions(driver, "Click", dict["UnallocatedPopUp"].ToString(), table.Rows[0], "UnallocatedPopUp");
                        }
                    }

                    if (!string.IsNullOrEmpty(columnValues["KeyToClick"].ToString()))
                    {
                        ClickElementWithTextUnderClass(driver, dict["HamburgClassQTT"].ToString(), columnValues["KeyToClick"].ToString());
                    }

                    try
                    {
                        driver.FindElement(By.ClassName(dict["HamburgClassQTT"].ToString()));
                        driver.FindElement(By.XPath(dict["QTTexpandButton"].ToString())).Click();
                    }
                    catch { }

                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("ColumnChooser"))
                {
                    IWebElement grid = null;
                    if (columnValues["Grid"].ToString().ToLower().Contains("suborder"))
                    {
                        PerformActionsVerification(driver, "Click", dict["OrderButton"].ToString(), "");
                        Wait(2000);
                        grid = driver.FindElement(By.Id(dict["subOrderGridID"].ToString()));
                    }
                    else if (columnValues["Grid"].ToString().ToLower().Contains("summary"))
                    {
                        PerformActionsVerification(driver, "Click", dict["SummaryButton"].ToString(), "");
                        Wait(2000);
                        grid = driver.FindElement(By.Id(dict["SummaryGridID"].ToString()));
                    }
                    else if (columnValues["Grid"].ToString().ToLower().Contains("working"))
                    {
                        PerformActionsVerification(driver, "Click", dict["WorkingTab"].ToString(), "");
                        Wait(2000);
                        grid = driver.FindElement(By.Id(dict["WorkingGridID"].ToString()));
                    }
                    else if (columnValues["Grid"].ToString().ToLower().Contains("order"))
                    {
                        PerformActionsVerification(driver, "Click", dict["OrderButton"].ToString(), "");
                        Wait(2000);
                        grid = driver.FindElement(By.Id(dict["OrderGridID"].ToString()));
                    }

                    grid.FindElement(By.Id(dict["ColumnChooserButtonID"].ToString())).Click();
                    Wait(2000);
                    IWebElement columnChooserWindow = grid.FindElement(By.ClassName(dict["ColumnChooserWindow"].ToString()));
                    IWebElement inputColumn = columnChooserWindow.FindElement(By.CssSelector("input[placeholder='Filter columns list ...']"));
                    actions.MoveToElement(inputColumn).DoubleClick().Perform();
                    for (int j = 0; j < 20; j++)
                    {
                        actions.MoveToElement(inputColumn).SendKeys(Keys.Backspace).Perform();
                    }
                    actions.MoveToElement(inputColumn).SendKeys("Select All")
                        .SendKeys(Keys.Tab)
                        .Perform();
                    Wait(1000);
                    IWebElement checkbox = columnChooserWindow.FindElement(By.XPath("//div[@class='ig-checkbox-box']"));
                    checkbox.Click();
                    Wait(1000);
                    foreach (string columName in columnValues["ColumnsToAdd"].ToString().Split(','))
                    {
                        SamsaraGridOperationHelper.EditColumnChooser(driver, columnChooserWindow, inputColumn, columName, actions);
                    }
                    grid.FindElement(By.Id(dict["ColumnChooserButtonID"].ToString())).Click();
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("VerifyToast"))
                {
                    if (toastList.Contains(columnValues["Message"].ToString()))
                    {
                        Console.WriteLine(columnValues["Message"].ToString() + " toast verified");
                    }
                    else 
                    {
                        throw new Exception(columnValues["Message"].ToString() + " toast not verified");
                    }
                    columnValues.Clear();
                    toastList.Clear();
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("VerifyExecutionType"))
                {
                    string[] enableButtons = columnValues["EnabledButtons"].Split(',');
                    string[] disableButtons = columnValues["DisabledButtons"].Split(',');
                    IWebElement parentOfExecutionButtons = driver.FindElement(By.ClassName(dict["ClassOfExecutionType"].ToString()));
                    IReadOnlyCollection<IWebElement> childDivs = parentOfExecutionButtons.FindElements(By.TagName("div"));
                    if (!string.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["EnabledButtons"].ToString()))
                    {
                        foreach (string str in enableButtons)
                        {
                            bool flag = false;
                            foreach (var ele in childDivs)
                            {
                                string classOfchild = ele.GetAttribute("class");
                                if (classOfchild.ToLower().Contains(str.ToLower()))
                                {
                                    string styleAttribute = ele.GetAttribute("style");
                                    if (!string.IsNullOrEmpty(styleAttribute) && ele.GetAttribute("style").ToLower().Contains("hidden"))
                                    {
                                        flag = false;
                                        break;
                                    }
                                    else
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                            }
                            if (!flag)
                            {
                                throw new Exception(str + " execution is not enable or found");
                            }
                            else
                            {
                                Console.WriteLine(str + " Execution type is enabled");
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["DisabledButtons"].ToString()))
                    {
                        foreach (string str in disableButtons)
                        {
                            bool flag = false;
                            foreach (var ele in childDivs)
                            {
                                string classOfchild = ele.GetAttribute("class");
                                if (classOfchild.ToLower().Contains(str.ToLower()))
                                {
                                    string styleAttribute = ele.GetAttribute("style");
                                    if (!string.IsNullOrEmpty(styleAttribute) && !ele.GetAttribute("style").ToLower().Contains("hidden"))
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                            }
                            if (flag)
                            {
                                throw new Exception(str + " Execution is enable or found");
                            }
                            else
                            {
                                Console.WriteLine(str + " Execution type is disabled");
                            }
                        }
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("SwitchToSameNameWindow"))
                {
                    SwitchWindow.SwitchToWindow(driver, dataTable.Rows[i]["Action"].ToString(), false, dict["CloseHotButton"].ToString());
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("FillHbDetails"))
                {
                    if (!string.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["Quantity"].ToString()))
                    {
                        PerformActions(driver, "SendKeys", dict["Quantity"].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], "Quantity");
                        driver.FindElement(By.XPath(dict["Quantity"].ToString())).Click();
                    }
                    if (!string.IsNullOrEmpty(columnValues["Execution Type"].ToString()))
                    {
                        if (columnValues["Execution Type"].ToString().ToLower().Contains("send"))
                        {
                            driver.FindElement(By.Id(dict["Live"].ToString())).Click();
                        }
                        else if (columnValues["Execution Type"].ToString().ToLower().Contains("manual"))
                        {
                            driver.FindElement(By.Id(dict["Manual"].ToString())).Click();
                        }
                        else if (columnValues["Execution Type"].ToString().ToLower().Contains("stage"))
                        {
                            driver.FindElement(By.Id(dict["Stage"].ToString())).Click();
                        }
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(columnValues["TIF"].ToString()) && columnValues["TIF"].ToString().ToLower().Contains("good till date"))
                        {
                            Wait(1000);
                            int selectionDate = 0;
                            try
                            {
                                driver.FindElement(By.XPath(dict["GtdCalendar"].ToString()));
                            }
                            catch
                            {
                                driver.FindElement(By.XPath(dict["GtdCalendarButton"].ToString())).Click();
                            }
                            selectionDate = GTD(TestCaseSheet_Temp.Tables[0].Rows[0]);
                            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, selectionDate);
                            if (IsWeekend(date))
                            {
                                DateTime nextWorkingDay = GetNextWorkingDay(date);
                                selectionDate = nextWorkingDay.Day;
                            }
                            IWebElement element = driver.FindElement(By.XPath(dict["GtdCalendarDate"].ToString()));
                            var allElements = element.FindElements(By.XPath(".//*"));
                            var columnHeaderElements = allElements.Where(e => e.GetAttribute("type") == "button").ToList();
                            bool currentMonth = false;
                            for (int j = 0; j < columnHeaderElements.Count; j++)
                            {
                                if (!currentMonth)
                                {
                                    while (!columnHeaderElements[j].Text.ToString().Equals("1"))
                                    {
                                        j++;
                                    }
                                    currentMonth = true;
                                }
                                if (columnHeaderElements[j].Text.Equals(selectionDate.ToString()))
                                {
                                    columnHeaderElements[j].Click();
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(columnValues["Order Type"].ToString()))
                        {
                            if (columnValues["Order Type"].ToString().ToLower().Contains("limit"))
                            {
                                driver.FindElement(By.Id(dict["Limit"].ToString())).Click();
                                Wait(1500);
                                if (columnValues.Keys.Contains("Cent/BPS") && !string.IsNullOrEmpty(columnValues["Cent/BPS"].ToString()))
                                {
                                    PerformActionsThroughID(driver, "SendKeys", dict["CentBpsHotButton"].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], "Cent/BPS");
                                }

                                if (columnValues.Keys.Contains("Ask/Bid/Last/Mid") && !string.IsNullOrEmpty(columnValues["Ask/Bid/Last/Mid"].ToString()))
                                {
                                    PerformActionsThroughID(driver, "SendKeys", dict["LimitOptionHotButton"].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], "Ask/Bid/Last/Mid");
                                }
                                if (columnValues.Keys.Contains("Limit Price") && !string.IsNullOrEmpty(columnValues["Limit Price"].ToString()))
                                {
                                    PerformActions(driver, "SendKeys", dict["Limit Price"].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], "Limit Price");
                                }
                            }
                            else if (columnValues["Order Type"].ToString().ToLower().Contains("market"))
                            {
                                driver.FindElement(By.Id(dict["Market"].ToString())).Click();
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    /*To select Order Type in Blotter hotButton Panel
                      *https://dev.azure.com/NirvanaSolutions/NirvanaOne/_workitems/edit/60745
                      *Bhoomika Mittal
                     */
                    if (!string.IsNullOrEmpty(columnValues["Add to Favourite"].ToString()) && columnValues["Add to Favourite"].ToString().ToLower().Equals("true"))
                    {
                        driver.FindElement(By.Id(dict["AddToFavHotButton"].ToString())).Click();
                    }
                    if (TestCaseSheet_Temp.Tables[0].Columns.Contains("ButtonName") && !string.IsNullOrEmpty(columnValues["ButtonName"].ToString())) 
                    {
                        PerformActions(driver, "SendKeys", dict["ButtonName"].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], "ButtonName");
                    }
                    try
                    {
                        if (TestCaseSheet_Temp.Tables[0].Columns.Contains("VerifyActionButtonVisibility"))
                        {
                            if (!string.IsNullOrEmpty(columnValues["VerifyActionButtonVisibility"].ToString()))
                            {
                                bool flag = false;
                                try
                                {
                                    var ele = driver.FindElement(By.XPath(dict["NextHotButtonPanel"].ToString()));
                                    flag = ele.Displayed && ele.Enabled;
                                }
                                catch
                                {
                                    flag = false;
                                }
                                if (!columnValues["VerifyActionButtonVisibility"].ToString().ToLower().Equals(flag.ToString().ToLower()))
                                {
                                    throw new Exception("Action button is visible/enabled = " + flag + " but excel has " + columnValues["VerifyActionButtonVisibility"]);
                                }
                                else {
                                    Console.WriteLine("Create Hot Button enable = " + flag);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(columnValues["Action"].ToString()))
                        {
                            if (!columnValues["Action"].ToString().ToLower().Equals("back"))
                            {
                                driver.FindElement(By.XPath(dict["NextHotButtonPanel"].ToString())).Click();
                            }
                            else
                            {
                                driver.FindElement(By.XPath(dict["CloseHotButtonPanel"].ToString())).Click();
                            }
                        }
                        else
                        {
                            driver.FindElement(By.XPath(dict["NextHotButtonPanel"].ToString())).Click();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("HotButtonColor"))
                {
                    if (!string.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["ButtonName"].ToString()))
                    {
                        PerformActions(driver, "SendKeys", dict["ButtonName"].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], "ButtonName");
                    }
                    if (!string.IsNullOrEmpty(columnValues["Button Color"].ToString()))
                    {
                        driver.FindElement(By.ClassName(dict["HotButtonColorButton"].ToString())).Click();
                        Wait(2000);
                        IWebElement element = driver.FindElement(By.ClassName(dict["HotButtonColorDropdown"].ToString()));
                        IList<IWebElement> divElements = element.FindElements(By.TagName("a"));
                        foreach (var ele in divElements)
                        {
                            if (ele.Text.ToString().ToLower().Equals(columnValues["Button Color"].ToString().ToLower()))
                            {
                                ele.Click();
                                Wait(2000);
                                break;
                            }
                        }
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(columnValues["Action"].ToString()))
                        {
                            if (!columnValues["Action"].ToString().ToLower().Equals("back"))
                            {
                                driver.FindElement(By.XPath(dict["NextHotButtonPanel"].ToString())).Click();
                            }
                            else
                            {
                                driver.FindElement(By.XPath(dict["CloseHotButtonPanel"].ToString())).Click();
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    columnValues.Clear();
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("VerifyOrders"))
                {
                    if (ExcelData == null)
                    {
                        ExcelData = TestCaseSheet.Tables[0];
                    }
                    string fileName = dataTable.Rows[i]["Action"].ToString();
                    string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    string filePath2 = SearchFile(downloadsPath, fileName);
                    if (!string.IsNullOrEmpty(filePath2))
                    {
                        List<string> list = new List<string>();
                        DataSet ds = DataUtilities.GetTestCaseTestData(filePath2, 1, 1, list);
                        if (!ExcelData.Columns.Contains("Allocation Status"))
                        {
                            Data = ds.Tables.Count > 0 ? ds.Tables[0] : null;
                        }
                        else
                        {
                            Data = ds.Tables["Working"];
                        }
                    }
                    Data = removeSuborderRows(Data);
                    if (ExcelData == null)
                    {
                        ExcelData = TestCaseSheet.Tables[0];
                    }
                    List<String> columns = new List<string>();

                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref ExcelData);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandler"], ref ExcelData);

                    DataUtilities.VerifyDate(ExcelData, Data);
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("TransferOrder"))
                {
                    if (StepName.Equals("TransferToUserOrder"))
                    {
                        SamsaraGridOperationHelper.PerformRightClickActions(driver, columnValues["userName"].ToString(), dict["TransferToUserOrderMenu"].ToString(), dict2["TransferToUserOrderMenu"].ToString());
                    }
                    else if (StepName.Equals("TransferToUserSubOrder"))
                    {
                        SamsaraGridOperationHelper.PerformRightClickActions(driver, columnValues["userName"].ToString(), dict["TransferToUserSubOrderMenu"].ToString());
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("ToolBarActions"))
                {
                    if (!string.IsNullOrEmpty(columnValues["Toolbar : BlotterToolBar"].ToString()))
                    {
                        if (columnValues["Toolbar : BlotterToolBar"].ToString().Equals("Refresh Data"))
                        {
                            PerformActions(driver, "Click", dict["RefreshButton"].ToString(), TestCaseSheet.Tables[0].Rows[0], "Refresh Data");
                        }
                        if (columnValues["Toolbar : BlotterToolBar"].ToString().Equals("Save All Layout"))
                        {
                            PerformActions(driver, "Click", dict["SaveAllLayout"].ToString(), TestCaseSheet.Tables[0].Rows[0], "Save All Layout");
                        }
                        if (columnValues["Toolbar : BlotterToolBar"].ToString().Equals("Add Tab (Working)"))
                        {
                            PerformActions(driver, "Click", dict["Button Name_Add Tab(Working)"].ToString(), TestCaseSheet.Tables[0].Rows[0], "Add Tab (Working)");
                        }
                        if (columnValues["Toolbar : BlotterToolBar"].ToString().Equals("Add Tab (Order)"))
                        {
                            PerformActions(driver, "Click", dict["Button Name_Add Tab(Order)"].ToString(), TestCaseSheet.Tables[0].Rows[0], "Add Tab (Order)");
                        }
                    }
                    if (!string.IsNullOrEmpty(columnValues["Refresh Data"].ToString()))
                    {
                        PerformActions(driver, "Click", dict["RefreshButton"].ToString(), TestCaseSheet.Tables[0].Rows[0], "Refresh Data");
                    }
                    if (!string.IsNullOrEmpty(columnValues["Add Tab (Working)"].ToString()))
                    {
                        PerformActions(driver, "Click", dict["Button Name_Add Tab(Working)"].ToString(), TestCaseSheet.Tables[0].Rows[0], "Add Tab (Working)");
                    }
                    if (!string.IsNullOrEmpty(columnValues["Add Tab (Order)"].ToString()))
                    {
                        PerformActions(driver, "Click", dict["Button Name_Add Tab(Order)"].ToString(), TestCaseSheet.Tables[0].Rows[0], "Add Tab (Order)");
                    }
                    if (!string.IsNullOrEmpty(columnValues["Save All Layout"].ToString()))
                    {
                        PerformActions(driver, "Click", dict["SaveAllLayout"].ToString(), TestCaseSheet.Tables[0].Rows[0], "Save All Layout");
                    }
                }

                else if (!string.IsNullOrEmpty(dataTable.Rows[i]["Steps"].ToString()) && !string.IsNullOrEmpty(dataTable.Rows[i]["Action"].ToString()) && dataTable.Rows[i]["Steps"].ToString().Contains("Ensure"))
                {
                    try
                    {
                        List<string> DList = dataTable.Rows[i]["Action"].ToString().Split(',').ToList();
                        if (DList.Count > 1)
                        {
                            SamsaraGridOperationHelper.ClickElement(driver, table.Rows[i]["Steps"].ToString(), ref dict, table.Rows[i]["Action"].ToString(), null);
                            //  SwitchWindow.SwitchToChildWindow(driver, DashBoardList[0].ToString(), dict[DashBoardList[1].ToString() + "Dashboard"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There is an issue while performing " + StepName + "  : " + ex.Message);
                    }

                }
                else if (table.Rows[i]["Steps"].ToString().Equals("ACCenvironment"))
                {
                    if (ConfigurationManager.AppSettings["ACCenvironment"].ToString().Equals("true"))
                    {
                        if (StepName.Equals("VerifySubOrder"))
                        {
                            return new DataTable();
                        }

                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("EditPTTgrid"))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        IWebElement element = driver.FindElement(By.XPath(dict["PTTGrid"].ToString()));
                        string id = string.Empty;
                        var allElements = element.FindElements(By.XPath(".//*"));
                        var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                        Dictionary<string, string> columnMap = new Dictionary<string, string>();
                        int Tempid = 0;
                        for (int j = 0; j < columnHeaderElements.Count; j++)
                        {
                            string elementId = columnHeaderElements[j].GetAttribute("id");
                            id = elementId.Substring(0, elementId.IndexOf("r") + 1);
                            try
                            {
                                IWebElement ele = element.FindElement(By.Id(id + "1c" + Tempid));
                                columnMap.Add(ele.Text, Tempid.ToString());
                                dt.Columns.Add(ele.Text);
                                Tempid++;
                            }
                            catch
                            {
                                Tempid++;
                            }
                        }
                        Wait(2000);
                        int row = 2;
                        while (true)
                        {
                            try
                            {
                                IWebElement ele = element.FindElement(By.Id(id + row + "c1"));
                                row++;
                            }
                            catch
                            {
                                break;
                            }
                        }
                        int flag = 2;
                        while (flag < row)
                        {
                            if (columnMap.Keys.Contains("Fund") || columnMap.Keys.Contains("Group"))
                            {
                                DataRow drow = dt.NewRow();
                                for (int k = 0; k <= dt.Columns.Count; k++)
                                {
                                    try
                                    {
                                        IWebElement elementRow = element.FindElement(By.Id(id + flag + "c" + k));
                                        var spanElements = elementRow.FindElements(By.TagName("span"));
                                        if (spanElements.Count > 0)
                                        {
                                            drow[k] = elementRow.FindElement(By.TagName("span")).Text;
                                        }
                                    }
                                    catch { }

                                }
                                dt.Rows.Add(drow);
                                flag++;
                            }
                            else
                            {
                                DataRow drow = dt.NewRow();
                                for (int k = 1; k <= dt.Columns.Count; k++)
                                {
                                    try
                                    {
                                        IWebElement elementRow = element.FindElement(By.Id(id + flag + "c" + k));
                                        var spanElements = elementRow.FindElements(By.TagName("span"));
                                        if (spanElements.Count > 0)
                                        {
                                            drow[k - 1] = elementRow.FindElement(By.TagName("span")).Text;
                                        }
                                    }
                                    catch { }

                                }
                                dt.Rows.Add(drow);
                                flag++;
                            }
                        }
                        Wait(2000);
                        if (ExcelData == null)
                        {
                            ExcelData = TestCaseSheet_Temp.Tables[0];
                        }
                        List<String> columns = new List<string>();
                        DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                        SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandler"], ref ExcelData);
                        Actions act = new Actions(driver);
                        int rowId = 0;
                        string col = string.Empty;
                        foreach (DataRow dr in ExcelData.Rows)
                        {
                            if (dr["Action"].ToString().ToUpper().Equals("SELECT"))
                            {
                                dr["Action"] = string.Empty;
                                DataRow dtRow = GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dt), dr, new List<string>());
                                rowId = dt.Rows.IndexOf(dtRow) + 2;
                                if (!string.IsNullOrEmpty(dr["Account"].ToString()))
                                {
                                    col = columnMap["Account"];
                                }
                                else if (!string.IsNullOrEmpty(dr["Fund"].ToString()))
                                {
                                    col = columnMap["Fund"];
                                }
                                else if (!string.IsNullOrEmpty(dr["Group"].ToString()))
                                {
                                    col = columnMap["Group"];
                                }
                                IWebElement elementRow = element.FindElement(By.Id(id + rowId + "c" + col));
                                act.Click(elementRow).Build().Perform();
                                Wait(4000);
                            }
                            else
                            {
                                //edit-icon
                                for (int k = 0; k < ExcelData.Columns.Count; k++)
                                {
                                    if (!string.IsNullOrEmpty(dr[k].ToString()))
                                    {
                                        if (ExcelData.Columns[k].ToString().Equals("Action"))
                                        {
                                            continue;
                                        }
                                        string column = columnMap[ExcelData.Columns[k].ToString()];
                                        string CellId = id + rowId + "c" + column;
                                        IWebElement changeBox = element.FindElement(By.Id(CellId));
                                        IWebElement pencil = changeBox.FindElement(By.ClassName("edit-icon"));
                                        act.DoubleClick(pencil).Build().Perform();
                                        IWebElement inputBox = changeBox.FindElement(By.TagName("input"));
                                        act.MoveToElement(inputBox).Click().SendKeys(Keys.Backspace).SendKeys(Keys.Backspace)
                                            .SendKeys(Keys.Backspace).SendKeys(Keys.Backspace).SendKeys(Keys.Backspace)
                                            .SendKeys(Keys.Backspace).SendKeys(Keys.Backspace).SendKeys(Keys.Backspace).SendKeys(dr[k].ToString()).SendKeys(Keys.Tab)
                                        .Perform();
                                        Wait(2000);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                else if (!string.IsNullOrEmpty(dataTable.Rows[i]["Steps"].ToString()) && string.Equals(dataTable.Rows[i]["Steps"].ToString(), "SwitchWithChildTabVerification", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        List<string> DashBoardList = dataTable.Rows[i]["Action"].ToString().Split(',').ToList();
                        if (DashBoardList.Count > 0)
                        {
                            if (DashBoardList.Count == 1)
                            {
                                SwitchWindow.SwitchToWindow(driver, DashBoardList[0].ToString(), true);
                            }
                            else
                            {
                                SwitchWindow.SwitchToChildWindow(driver, DashBoardList[0].ToString(), dict[DashBoardList[1].ToString() + "Dashboard"].ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There is an issue while performing SwitchWithChildTabVerification     : " + ex.Message);
                    }

                }
                else if (table.Columns.Contains("ActionThroughClass") && table.Rows[i]["ActionThroughClass"].ToString().ToUpper().Equals("TRUE"))
                {
                    try
                    {
                        PerformActionThroughClass(driver, table.Rows[i]["Action"].ToString(), dict[table.Rows[i]["Steps"].ToString()].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], table.Rows[i]["Steps"].ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                else if (table.Columns.Contains("ActionThroughId") && table.Rows[i]["ActionThroughId"].ToString().ToUpper().Equals("TRUE"))
                {
                    try
                    {
                        PerformActionsThroughID(driver, table.Rows[i]["Action"].ToString(), dict[table.Rows[i]["Steps"].ToString()].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0], table.Rows[i]["Steps"].ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("L1L2PanelRTPNL"))
                {
                    if (!string.IsNullOrEmpty(columnValues["IsL1isVisible"].ToString()))
                    {
                        bool flag = true;
                        try
                        {
                            driver.FindElement(By.ClassName(dict["L1Button"].ToString()));
                            flag = true;
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!columnValues["IsL1isVisible"].ToString().ToLower().Equals(flag.ToString().ToLower()))
                        {
                            throw new Exception("L1 strip visibility is " + flag + " but excel has " + columnValues["IsL1isVisible"]);
                        }
                    }
                    if (!string.IsNullOrEmpty(columnValues["IsL2isVisible"].ToString()))
                    {
                        bool flag = true;
                        try
                        {
                            driver.FindElement(By.ClassName(dict["L2Button"].ToString()));
                            flag = true;
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!columnValues["IsL2isVisible"].ToString().ToLower().Equals(flag.ToString().ToLower()))
                        {
                            throw new Exception("L2 strip visibility is " + flag + " but excel has " + columnValues["IsL2isVisible"]);
                        }
                    }
                    if (!string.IsNullOrEmpty(columnValues["ClickOnL1"].ToString()))
                    {
                        driver.FindElement(By.ClassName(dict["L1Button"].ToString())).Click();
                        Wait(2000);
                    }
                    if (!string.IsNullOrEmpty(columnValues["ClickOnL2"].ToString()))
                    {
                        driver.FindElement(By.ClassName(dict["L2Button"].ToString())).Click();
                    }

                    if (!string.IsNullOrEmpty(columnValues["L1State"].ToString()))
                    {
                        Wait(4000);
                        IReadOnlyCollection<IWebElement> grids = driver.FindElements(By.CssSelector("[role='grid']"));
                        if (columnValues["L1State"].ToString().ToLower().Equals("true"))
                        {
                            if (grids.Count < 2)
                            {
                                throw new Exception("L1 grid is not visible");
                            }
                        }
                        else
                        {
                            if (grids.Count > 1)
                            {
                                throw new Exception("L1 grid is visible");
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(columnValues["L2State"].ToString()))
                    {
                        Wait(4000);
                        IReadOnlyCollection<IWebElement> grids = driver.FindElements(By.CssSelector("[role='grid']"));
                        if (columnValues["L2State"].ToString().ToLower().Equals("true"))
                        {
                            if (grids.Count < 3)
                            {
                                throw new Exception("L2 grid is not visible");
                            }
                        }
                        else
                        {
                            if (grids.Count > 2)
                            {
                                throw new Exception("L2 grid is visible");
                            }
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("L1Label") && !string.IsNullOrEmpty(columnValues["L1Label"].ToString()))
                    {
                        string Label1 = columnValues["L1Label"].ToString();
                        IWebElement L1 = driver.FindElement(By.ClassName(dict["L1Button"].ToString()));
                        if (!L1.Text.ToString().ToLower().Equals(Label1.ToLower()))
                        {
                            throw new Exception("L1 strip visibility is " + L1 + " but excel has " + columnValues["L1Label"]);
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("L2Label") && !string.IsNullOrEmpty(columnValues["L2Label"].ToString()))
                    {
                        string Label2 = columnValues["L2Label"].ToString();
                        IWebElement L2 = driver.FindElement(By.ClassName(dict["L2Button"].ToString()));
                        if (!L2.Text.ToString().ToLower().Equals(Label2.ToLower()))
                        {
                            throw new Exception("L2 strip visibility is " + L2 + " but excel has " + columnValues["L2Label"]);
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("WidgetName") && !string.IsNullOrEmpty(columnValues["WidgetName"].ToString()))
                    {
                        //
                        if (columnValues["WidgetName"].ToString().ToLower().Equals(driver.FindElement(By.ClassName(dict["widgetNameOfGrouping"].ToString())).Text.ToString().ToLower()))
                        {
                            Console.WriteLine(columnValues["WidgetName"].ToString() + " widget name verified");
                        }
                        else 
                        {
                            throw new Exception(columnValues["WidgetName"].ToString() + " widget name not verified");
                        }
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("EditAuecWindow"))
                {
                    if (!string.IsNullOrEmpty(columnValues["Lock/Unlock Padlock"].ToString()) && columnValues["Lock/Unlock Padlock"].ToString().ToLower().Contains("unlock"))
                    {
                        PerformActionsVerification(driver, "Click", dict["Lock/Unlock Padlock"].ToString(), "");
                    }
                    if (!string.IsNullOrEmpty(columnValues["Broker"].ToString()))
                    {
                        PerformActionsVerification(driver, "Click", dict["OpenAUEC"].ToString(), "");
                        Wait(2000);
                        DataTable agGridDataTable = SamsaraGridOperationHelper.GetAGgridData(driver);
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Account");
                        DataRow dataRow = dt.NewRow();
                        dataRow[0] = TestCaseSheet.Tables[0].Rows[0]["Account"];
                        dt.Rows.Add(dataRow);
                        DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(agGridDataTable), dt.Rows[0]);
                        int index = agGridDataTable.Rows.IndexOf(dtRow);
                        var row = driver.FindElement(By.XPath("//div[@class='ag-center-cols-container']//div[@row-index='" + index + "']"));
                        var cells = row.FindElements(By.XPath(".//div[@role='gridcell']"));
                        cells[agGridDataTable.Columns.IndexOf("Broker")].Click();
                        Wait(1500);
                        try
                        {
                            var ele = cells[agGridDataTable.Columns.IndexOf("Broker")].FindElement(By.TagName("input"));
                            actions.MoveToElement(ele)
                            .KeyDown(Keys.Control)
                            .SendKeys("a")
                            .KeyUp(Keys.Control).SendKeys(Keys.Backspace).SendKeys(columnValues["Broker"].ToString())
                            .SendKeys(Keys.Enter)
                            .Build()
                            .Perform();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (!string.IsNullOrEmpty(columnValues["NextAction"].ToString()))
                    {
                        if (columnValues["NextAction"].ToString().ToLower().Contains("summary"))
                        {
                            PerformActionsVerification(driver, "Click", dict["ShowSummary"].ToString(), "");
                        }
                        else
                        {
                            PerformActionsVerification(driver, "Click", dict["OkAUEC"].ToString(), "");
                        }
                    }

                }
                else if (table.Rows[i]["Steps"].ToString().Equals("EditQuickPreference"))
                {
                    IWebElement element = driver.FindElement(By.ClassName("checkbox-list"));
                    IReadOnlyCollection<IWebElement> childs = element.FindElements(By.TagName("label"));
                    if (!string.IsNullOrEmpty(columnValues["ColumnsToCheck"].ToString()))
                    {
                        foreach (string str in columnValues["ColumnsToCheck"].ToString().Split(','))
                        {
                            foreach (var ele in childs)
                            {
                                if (ele.Text.ToString().ToLower().Equals(str.ToLower()))
                                {
                                    string inputId = ele.GetAttribute("for");
                                    IWebElement checkbox = driver.FindElement(By.Id(inputId)); // find the input by id
                                    checkbox.Click();
                                    Wait(2000);
                                    break;
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(columnValues["ColumnsToUncheck"].ToString()))
                    {
                        foreach (string str in columnValues["ColumnsToUncheck"].ToString().Split(','))
                        {
                            foreach (var ele in childs)
                            {
                                if (ele.Text.ToString().ToLower().Equals(str.ToLower()))
                                {
                                    string inputId = ele.GetAttribute("for");
                                    IWebElement checkbox = driver.FindElement(By.Id(inputId)); // find the input by id
                                    checkbox.Click();
                                    Wait(2000);
                                    break;
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(columnValues["VerifyCheckedColumn"].ToString()))
                    {
                        foreach (string str in columnValues["VerifyCheckedColumn"].ToString().Split(','))
                        {
                            bool flag = false;
                            foreach (var ele in childs)
                            {
                                if (ele.Text.ToString().ToLower().Equals(str.ToLower()))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                                throw new Exception(str + " column not verified on QTT");
                        }
                    }
                    if (!string.IsNullOrEmpty(columnValues["VerifyUnCheckedColumn"].ToString()))
                    {
                        foreach (string str in columnValues["VerifyUnCheckedColumn"].ToString().Split(','))
                        {
                            bool flag = false;
                            foreach (var ele in childs)
                            {
                                if (ele.Text.ToString().ToLower().Equals(str.ToLower()))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                                throw new Exception(str + " column not verified on QTT");
                        }
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("VerifyBrokerSummary"))
                {
                    DataTable agGridDataTable = SamsaraGridOperationHelper.GetAGgridData(driver);
                    TestCaseSheet_Temp.Tables[0].Columns.Remove("Next Action");
                    if (TestCaseSheet_Temp.Tables[0].Columns.Contains("Connection Status"))
                        TestCaseSheet_Temp.Tables[0].Columns.Remove("Connection Status");
                    List<String> columns = new List<string>();
                    columns.Add("Account");
                    List<String> errors = Recon.RunRecon(agGridDataTable, TestCaseSheet_Temp.Tables[0], columns, 0.01);
                    if (errors.Count > 0)
                    {
                        if (ExcelData.DataSet.Tables[0].TableName.ToString().Contains("VerifyPTT"))
                        {
                            errors = Recon.RunRecon(Data, ExcelData, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
                            if (errors.Count == 0)
                            {
                                ApplicationArguments.isVerificationSuceeded = true;
                                continue;
                            }
                        }
                        try
                        {
                            bool anydataChanged = SamsaraCustomizableVerificationHandler.CustomizableDataManipulator(StepName.ToString(), ExcelData, Data, DataTables["DataManipulator"]);
                            if (anydataChanged)
                                errors = Recon.RunRecon(Data, ExcelData, columns, 0.01);
                        }
                        catch (Exception ex)
                        { Console.WriteLine(ex.Message); }

                        if (errors.Count > 0)
                        {
                            foreach (string error in errors)
                            {
                                Console.WriteLine(error);
                            }
                            string errorMessage = ApplicationArguments.TestCaseToBeRun + " failed at step - " + StepName + " Errors:\n";
                            foreach (string error in errors)
                            {
                                Console.WriteLine(error);
                                errorMessage += error + "\n";
                            }
                            CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " Verification failure");
                            throw new Exception(errorMessage);
                        }

                    }
                    else
                    {
                        ApplicationArguments.isVerificationSuceeded = true;
                        if (!string.IsNullOrEmpty(columnValues["Next Action"].ToString()))
                        {
                            if (columnValues["Next Action"].ToString().ToLower().Contains("ok"))
                            {
                                PerformActionsVerification(driver, "Click", dict["OkBrokerSummary"].ToString(), "");
                            }
                            else
                            {
                                PerformActionsVerification(driver, "Click", dict["BackBrokerSummary"].ToString(), "");
                            }
                        }
                    }

                }
                else if (table.Rows[i]["Steps"].ToString().Equals("VerifyShortLocateWindow"))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        IWebElement element = driver.FindElement(By.ClassName(dict["ShorLocateGridClass"].ToString()));
                        string maxRow = element.GetAttribute("aria-rowcount");
                        string id = string.Empty;
                        var allElements = element.FindElements(By.XPath(".//*"));
                        var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                        foreach (var elements in columnHeaderElements)
                        {
                            string elementId = elements.GetAttribute("id");
                            id = elementId.Substring(0, elementId.IndexOf("r") + 1);
                            IWebElement ele = elements.FindElement(By.TagName("span"));
                            dt.Columns.Add(ele.Text);
                        }
                        int min_row = 2;
                        while (min_row <= Int32.Parse(maxRow))
                        {
                            int col = 0;
                            DataRow ds = dt.NewRow();
                            while (col < 5)
                            {
                                IWebElement ele = element.FindElement(By.Id(id + min_row + "c" + (col + 1)));
                                ds[col] = ele.FindElement(By.TagName("span")).Text;
                                col++;
                            }
                            dt.Rows.Add(ds);
                            min_row++;
                        }
                        dt = DataUtilities.RemoveCommas(dt);

                        dt = DataUtilities.RemovePercent(dt);
                        dt = DataUtilities.RemoveTrailingZeroes(dt);
                        List<String> errors = Recon.RunRecon(dt, TestCaseSheet_Temp.Tables[0], new List<string>(), 0.01);
                        if (errors.Count > 0)
                        {
                            foreach (string error in errors)
                            {
                                Console.WriteLine(error);
                            }
                            string errorMessage = ApplicationArguments.TestCaseToBeRun + " failed at step - " + StepName + " Errors:\n";
                            foreach (string error in errors)
                            {
                                Console.WriteLine(error);
                                errorMessage += error + "\n";
                            }
                            throw new Exception(errorMessage);

                        }
                        else
                        {
                            ApplicationArguments.isVerificationSuceeded = true;
                            Console.WriteLine("ShortLocate window verified");
                        }
                    }
                    catch (Exception ex)
                    {

                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " ShortLocate");
                        throw ex;
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("SwitchToParentWindow"))
                {
                    string[] parentname = dataTable.Rows[i]["Action"].ToString().Split(',');
                    if (ConfigurationManager.AppSettings["OpenSpecifiedWindow"].ToString().Split(',').Contains(StepName))
                    {
                        parentname = TestCaseSheet_Temp.Tables[0].Rows[0]["OpenWindow"].ToString().Split(',');
                        dataTable.Rows[i]["Action"] = parentname[0];
                        if (parentname[0].Equals("Nirvana"))
                        {
                            if (parentname.Length < 2)
                            {
                                NewTabNameRTPNL("ThemeVerify", "ThemeVerify", dict, driver);
                            }
                            if (StepName.Equals("SavePageView"))
                            {
                                i = 1;
                            }
                        }
                    }
                    SwitchWindow.SwitchToParentWindow(driver, parentname[0], dict["CloseModule"].ToString());
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("SavePageViewofModule"))
                {
                    PerformActionsVerification(driver, "Click", dict["BrowserMenuDropdown"].ToString(), "");
                    Wait(2000);
                    if (TestCaseSheet.Tables[0].Rows[0]["OpenWindow"].ToString().Contains("Nirvana"))
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        }
                    }
                    Wait(1000);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(1000);
                    for (int j = 0; j < 15; j++)
                    {
                        Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    }
                    Keyboard.SendKeys(TestCaseSheet_Temp.Tables[0].Rows[0]["TabName"].ToString());
                    Wait(1000);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                }

                else if (dataTable.Rows[i]["Steps"].ToString().Equals("SavePageOnRTPNL"))
                {
                    PerformActionsVerification(driver, "Click", dict["BrowserMenuDropdown"].ToString(), "");
                    Wait(2000);
                    if (TestCaseSheet.Tables[0].Rows[0]["OpenWindow"].ToString().Contains("Nirvana"))
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        }
                    }
                    Wait(1000);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Wait(1000);
                }

                else if (dataTable.Rows[i]["Steps"].ToString().Equals("FavouriteTab"))
                {
                    int str = 1;
                    if (!string.IsNullOrEmpty(columnValues["TabToMarkFavourite"]))
                    {
                        string path = null;
                        bool flag;
                        while (true)
                        {
                            IWebElement divElement = null;
                            try
                            {
                                path = dict["CustomTabPath"].ToString().Replace("str", str.ToString());

                                divElement = driver.FindElement(By.XPath(path));
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Tab not found");
                                flag = false;
                                break;
                            }
                            if (divElement.Text == columnValues["TabToMarkFavourite"])
                            {
                                Console.WriteLine("Tab founded");
                                flag = true;
                                driver.FindElement(By.XPath(path)).Click();
                                break;
                            }
                            str++;

                        }
                        driver.FindElement(By.XPath(dict["CustomTabAction"].ToString().Replace("str", str.ToString()))).Click();
                        Wait(2000);
                        string xpath = dict["FavouriteTab"].ToString().Replace("str", str.ToString());
                        Wait(2000);
                        PerformActionsVerification(driver, "Click", xpath, table.Rows[i]["Steps"].ToString());
                        Wait(2000);
                    }

                    if (!string.IsNullOrEmpty(columnValues["CurrentActiveTab"]))
                    {
                        string path = dict["TabProperties"].ToString().Replace("str", str.ToString());
                        string className = driver.FindElement(By.XPath(path)).GetAttribute("class");
                        if (!className.ToString().Contains("active"))
                        {
                            throw new Exception(columnValues["CurrentActiveTab"] + " is not a favourite tab");
                        }
                        else
                        {
                            Console.WriteLine(columnValues["CurrentActiveTab"] + " is a favourite tab");
                        }
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString() == "SwitchWindow")
                {
                    if (ConfigurationManager.AppSettings["OpenSpecifiedWindow"].ToString().Split(',').Contains(StepName))
                    {
                        string[] arr = TestCaseSheet_Temp.Tables[0].Rows[0]["OpenWindow"].ToString().Split(',');
                        dataTable.Rows[i]["Action"] = arr[0];
                        if (arr.Length > 1)
                        {
                            dataTable.Rows[i]["Action"] = TestCaseSheet_Temp.Tables[0].Rows[0]["TabName"].ToString();
                            bool flag = OpenTabOfRTPNL(TestCaseSheet_Temp.Tables[0].Rows[0]["TabName"].ToString(), dict, driver);
                            if (!flag)
                            {
                                Console.WriteLine("Tab with name " + TestCaseSheet_Temp.Tables[0].Rows[0]["TabName"].ToString() + " does not exist");
                            }
                            else
                            {
                                SwitchWindow.SwitchToWindow(driver, dataTable.Rows[i]["Action"].ToString(), true);
                            }
                        }
                        else if (arr.Length == 1)
                        {
                            if (TestCaseSheet_Temp.Tables[0].Columns.Contains("TabName") && !string.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["TabName"].ToString()))
                            {
                                bool flag = NewTabNameRTPNL(TestCaseSheet_Temp.Tables[0].Rows[0]["TabName"].ToString(), TestCaseSheet_Temp.Tables[0].Rows[0]["Description"].ToString(), dict, driver);
                                if (flag)
                                {
                                    dataTable.Rows[i]["Action"] = TestCaseSheet_Temp.Tables[0].Rows[0]["TabName"].ToString();
                                }
                                else
                                {
                                    dataTable.Rows[i]["Action"] = dict["DefaultTab"].ToString();
                                }
                            }
                            else
                            {
                                SwitchWindow.SwitchToWindow(driver, dataTable.Rows[i]["Action"].ToString());
                            }
                        }
                        continue;
                    }
                    SwitchWindow.SwitchToWindow(driver, dataTable.Rows[i]["Action"].ToString());
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("SavePage"))
                {
                    int str = 1;
                    Wait(2000);
                    string path = null;
                    while (true)
                    {
                        try
                        {
                            path = dict["SaveDropDownAction"].ToString().Replace("str", str.ToString());
                            if (driver.FindElement(By.XPath(path)).Text.Equals(columnValues["Action"]))
                            {
                                Console.WriteLine(columnValues["Action"] + " Action founded");
                                driver.FindElement(By.XPath(path)).Click();
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(columnValues["Action"] + " Action not found");
                            break;
                        }
                        str++;
                    }
                    if (columnValues["Action"].Equals("Rename and Save Page"))
                    {
                        SwitchWindow.SwitchToWindow(driver, "OpenFin Popup Window", true);
                        IWebElement element = driver.FindElement(By.XPath(dict["RenameTabSaveInputBox"].ToString()));
                        element.Clear();
                        Actions actions2 = new Actions(driver);
                        actions2.Click(element).SendKeys(columnValues["RenameTabName"])
                                      .Perform();
                        driver.FindElement(By.XPath(dict["RenameTabSaveButton"].ToString())).Click();
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().ToLower().Equals("switchcolumnwindow"))
                {
                    try
                    {
                        string value = TestCaseSheet.Tables[0].Rows[0][table.Rows[i]["Action"].ToString()].ToString();
                        if (!SwitchWindow.SwitchToWindow(driver, value))
                        {
                            Console.WriteLine("Window not switched");
                        }
                    }
                    catch (Exception ColumnNotFound)
                    {
                        throw new Exception(table.Rows[i]["Action"].ToString() + "Column not found in Testcase sheet" + ColumnNotFound);
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("SelectColumns"))
                {
                    try
                    {
                        DataTable dtable = TestCaseSheet.Tables[0];
                        bool isSelect = SamsaraGridOperationHelper.SelectColumn(driver, ref dict, dtable);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().ToLower().Equals("exactswitchwindow"))
                {
                    string value = table.Rows[i]["Action"].ToString();
                    SwitchWindow.SwitchToWindow(driver, value, true);
                }
                else if (table.Rows[i]["Steps"].ToString().ToLower().Equals("verifywidget"))
                {
                    try
                    {
                        bool flag = true;
                        string value = TestCaseSheet.Tables[0].Rows[0][table.Rows[i]["Action"].ToString()].ToString();
                        IReadOnlyCollection<IWebElement> widgetList = driver.FindElements(By.XPath(dict[table.Rows[i]["Steps"].ToString()].ToString()));
                        //string widgetName = driver.FindElement(By.XPath(dict[table.Rows[i]["Steps"].ToString()].ToString())).Text;
                        foreach (IWebElement widget in widgetList)
                            if (value == widget.Text)
                                flag = false;
                        //if (value != widgetName)
                        if (flag)
                            throw new Exception("Widget name not matched UI name: ");
                    }
                    catch
                    {
                        throw new Exception(table.Rows[i]["Action"].ToString() + "Column not found in Testcase sheet");
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("CloseApplication"))
                {
                    ProcessControlManager.KillApplication(dataTable.Rows[i]["Action"].ToString());
                }
                else if (dataTable.Rows[i]["Action"].ToString() == "VerifyOpenFinWIndow")
                {
                    //string value = dict[dataTable.Rows[i]["Steps"].ToString()].ToString();
                    string value1 = TestCaseSheet.Tables[0].Rows[0][dataTable.Rows[i]["Steps"].ToString()].ToString();
                    if (TestCaseSheet.Tables[0].Columns.Contains("WindowPresentOrNot") && !string.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["WindowPresentOrNot"].ToString()))
                    {
                        if (!SwitchWindow.SwitchToWindow(driver, value1).ToString().ToUpper().Equals(TestCaseSheet.Tables[0].Rows[0]["WindowPresentOrNot"].ToString().ToUpper()))
                            throw new Exception("Window Availablity is " + TestCaseSheet.Tables[0].Rows[0]["WindowPresentOrNot"].ToString());
                    }
                    else if (!SwitchWindow.SwitchToWindow(driver, value1))
                        throw new Exception("Not Verified Window");
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("RemoveColumnsFromDataSheet"))
                {

                    ExcelData = DataUtilities.RemoveColumnsAndRows(dataTable.Rows[i]["Action"].ToString(), TestCaseSheet.Tables[0]);

                }
                else if (table.Rows[i]["Steps"].ToString() == "CloseWindow")
                {
                    // SwitchWindow.SwitchToWindow(driver, "Nirvana");
                    // driver.Close();
                    if (ConfigurationManager.AppSettings["OpenSpecifiedWindow"].ToString().Split(',').Contains(StepName))
                    {
                        string[] arr = TestCaseSheet_Temp.Tables[0].Rows[0]["OpenWindow"].ToString().Split(',');
                        dataTable.Rows[i]["Action"] = arr[0];
                    }
                    CloseWindow(driver, table.Rows[i]["Action"].ToString(), dict["CloseModule"].ToString());
                }
                else if (table.Rows[i]["Steps"].ToString() == "CloseWindowNirvana")
                {
                    SwitchWindow.SwitchToWindow(driver, "Nirvana", true);
                    driver.Close();
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("RightClickOperations"))
                {
                    IWebElement element = driver.FindElement(By.CssSelector(DefaultCellValue));
                    actions.ContextClick(element).Perform();
                    string Xpath = string.Empty;
                    if (columnValues["GridName"].ToString().ToLower().Contains("suborder"))
                    {
                        Xpath = dict["suborderContextMenu"].ToString();
                    }
                    else if (columnValues["GridName"].ToString().ToLower().Contains("order"))
                    {
                        Xpath = dict["orderContextMenu"].ToString();
                    }
                    else
                    {
                        Xpath = dict["workingContextMenu"].ToString();
                    }
                    Wait(2000);
                    SamsaraGridOperationHelper.PerformRightClickActions(driver, columnValues["Action"], Xpath);
                }
                else if (dataTable.Rows[i]["Action"].ToString().Contains("RightClick"))
                {
                    try
                    {
                        if (RowIndex >= 0)
                        {
                            PerformActionsVerification(driver, dataTable.Rows[i]["Action"].ToString(), dict["OrderGridAGgridID"].ToString(), "");
                        }
                    }
                    catch
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                    }
                }
                else if (dataTable.Rows[i]["Action"].ToString().Contains("Select"))
                {
                    try
                    {
                        if (RowIndex >= 0)
                        {
                            PerformActionsVerification(driver, dataTable.Rows[i]["Action"].ToString(), dict["OrderGridAGgridID"].ToString(), "");
                        }
                    }
                    catch
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("VerifyIndexWise"))
                {

                    DataTable dtable = TestCaseSheet.Tables[StepName];
                    if (dtable == null && TestCaseSheet.Tables.Count > 0)
                    {
                        dtable = TestCaseSheet.Tables[0];
                    }
                    try
                    {
                        bool verificationSucceeded = VerifyIndexWise(driver, dtable, StepName, ref dict);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    // SamsaraGridOperationHelper.AllocateFromBlotterGridFill(driver, dtable, StepName, table.Rows[i]["Action"].ToString(), ref dict);
                    //asasas
                }

                else if (dataTable.Rows[i]["Steps"].ToString().Contains("VerifyWithRecon"))
                {
                    try
                    {
                        if (ExcelData == null)
                        {
                            ExcelData = TestCaseSheet.Tables[0];
                        }
                        List<String> columns = new List<string>();

                        DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                        Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref ExcelData);
                        SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandler"], ref ExcelData);

                        if (StepName.ToString() == "VerifyComplianceOrder" || StepName.ToString() == "VerifyPTTCompliance")
                        {
                            columns.Add("Rule Name");
                            DataUtilities.CleanColumnValues(Data, "Threshold");
                            DataUtilities.CleanColumnValues(Data, "Actual Result");
                        }
                        else
                        {
                            string keyToCheck = TestStatusLog.removeDigitsFromModule(StepName);

                            if (ConfigurationManager.AppSettings.AllKeys.Contains("-" + keyToCheck + "CompulsoryColumnsList"))
                            {
                                if (keyToCheck == "CheckSummaryDashboard") { }
                                else
                                {
                                    Console.WriteLine("Key :" + keyToCheck + " exists.");
                                    columns = ConfigurationManager.AppSettings["-" + keyToCheck + "CompulsoryColumnsList"].Split(',').Select(x => x.Trim()).ToList();
                                }
                            }
                            Data = DataUtilities.RemoveCommas(Data);

                            Data = DataUtilities.RemovePercent(Data);
                            Data = DataUtilities.RemoveTrailingZeroes(Data);
                        }

                        if (Data != null)
                        {
                            SamsaraGridOperationHelper.CreateExcelFileFromDataTable(Data, "UIData");
                        }
                        if (ExcelData != null)
                        {
                            SamsaraGridOperationHelper.CreateExcelFileFromDataTable(ExcelData, "ExcelData");
                        }

                        // Data = DataUtilities.RemoveCommas(Data); to check if this data coming without comma values?

                        List<String> errors = Recon.RunRecon(Data, ExcelData, columns, 0.01);
                        if (errors.Count > 0)
                        {
                            try
                            {
                                bool anydataChanged = SamsaraCustomizableVerificationHandler.CustomizableDataManipulator(StepName.ToString(), ExcelData, Data, DataTables["DataManipulator"]);
                                if (anydataChanged)
                                    errors = Recon.RunRecon(Data, ExcelData, columns, 0.01);
                            }
                            catch (Exception ex)
                            { Console.WriteLine(ex.Message); }

                            if (errors.Count > 0)
                            {
                                foreach (string error in errors)
                                {
                                    Console.WriteLine(error);
                                }
                                string errorMessage = ApplicationArguments.TestCaseToBeRun + " failed at step - " + StepName + " Errors:\n";
                                foreach (string error in errors)
                                {
                                    Console.WriteLine(error);
                                    errorMessage += error + "\n";
                                }
                                CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " Verification failure");
                                throw new Exception(errorMessage);
                            }

                        }
                        else
                        {
                            ApplicationArguments.isVerificationSuceeded = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " Recon expection error");
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                        if (rethrow)
                            throw;
                    }


                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("SelectRTPNLRow"))
                {
                    try
                    {
                        DataTable dtable = TestCaseSheet.Tables[StepName];
                        if (dtable == null && TestCaseSheet.Tables.Count > 0)
                        {
                            dtable = TestCaseSheet.Tables[0];
                        }
                        SamsaraGridOperationHelper.SelectRTPNLRow(driver, dtable, ref dict, Data, DefaultCellValue, RowIndex);

                    }
                    catch (Exception ex)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                        if (rethrow)
                            throw;
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("GetRTPNLGridData"))
                {
                    try
                    {
                        Data = SamsaraGridOperationHelper.ExtractGridData(driver, ref dict, table.Rows[i]["Action"].ToString() + "Headers", table.Rows[i]["Action"].ToString() + "DataItems", table.Rows[i]["Action"].ToString() + "VerticalScroller", table.Rows[i]["Action"].ToString() + "HorizontalScroller");
                        if (Data.Rows.Count == 0)
                        {
                            Data = SamsaraGridOperationHelper.GetGridData(driver, ref dict, table.Rows[i]["Action"].ToString() + "Headers", table.Rows[i]["Action"].ToString() + "DataItems", table.Rows[i]["Action"].ToString() + "VerticalScroller", table.Rows[i]["Action"].ToString() + "HorizontalScroller");
                        }
                        if (ConfigurationManager.AppSettings["RTPNLVerificationSteps"].ToString().Split(',').Contains(StepName))
                        {
                            if (Data.Columns.Contains("NAV") || Data.Columns.Contains("Bloomberg Symbol"))
                            {
                                string col = Data.Columns.Contains("NAV") ? "NAV" : "BLOOMBERG SYMBOL";
                                Console.WriteLine("Grid has " + col + " column");
                            }
                            else
                            {
                                Data = SamsaraGridOperationHelper.ExtractGridData(driver, ref dict, table.Rows[i]["Action"].ToString() + "Headers", table.Rows[i]["Action"].ToString() + "DataItems", table.Rows[i]["Action"].ToString() + "VerticalScroller", table.Rows[i]["Action"].ToString() + "HorizontalScroller");
                            }
                        }
                        if (Data != null)
                        {
                            SamsaraGridOperationHelper.CreateExcelFileFromDataTable(Data, StepName);
                        }
                        else
                        {
                            CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " RTPNL Grid Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " RTPNL Grid Error");
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                        if (rethrow)
                            throw;
                    }
                }

                else if (dataTable.Rows[i]["Steps"].ToString().Contains("GetPSTGridData"))
                {
                    try
                    {
                        List<DataTable> tableList = new List<DataTable>();
                        bool hasMoreData = true;

                        while (hasMoreData)
                        {
                            try
                            {
                                DataTable dt = SamsaraGridOperationHelper.ExtractPSTGridData(driver, ref dict,
                                    table.Rows[i]["Action"].ToString() + "Headers",
                                    table.Rows[i]["Action"].ToString() + "DataItems",
                                    table.Rows[i]["Action"].ToString() + "VerticalScroller",
                                    table.Rows[i]["Action"].ToString() + "HorizontalScroller");

                                if (dt != null)
                                {
                                    tableList.Add(dt);
                                }

                                hasMoreData = SamsaraGridOperationHelper.MoveSliderToBottomMost(driver, dict["PTTGridVerticalScroller"].ToString(), dict);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("No more data " + ex.Message);
                            }
                        }
                        hasMoreData = true;
                        while (hasMoreData)
                        {
                            try
                            {
                                hasMoreData = SamsaraGridOperationHelper.MoveSliderToTopMost(driver, dict["PTTGridVerticalScroller"].ToString(), dict);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("No more data " + ex.Message);
                            }
                        }

                        Data = DataUtilities.MergeDataTablesWithMatchingColumns(tableList.ToArray(), true);
                        Data = DataUtilities.DeleteDuplicateData(Data, true);

                        if (Data != null)
                        {
                            List<string> pstGridUIDataCharRemovalList = ConfigurationManager.AppSettings["-pstGridUIDataCharRemoval"].ToString().Split(',').ToList();
                            Data = DataUtilities.RemoveStrings(Data, pstGridUIDataCharRemovalList, "");
                            Data = DataUtilities.MergeCellLines(Data);
                            Data = DataUtilities.RemoveCommas(Data);
                            Data = DataUtilities.RemoveTrailingZeroes(Data);
                            SamsaraGridOperationHelper.CreateExcelFileFromDataTable(Data, StepName);
                        }
                        else
                        {
                            CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " PST Grid Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " PST Grid Error");
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                        if (rethrow)
                            throw;
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("RenameRTPNLtab"))
                {
                    OpenTabOfRTPNL(columnValues["TabName"].ToString(), dict, driver);
                    Wait(2000);
                    SwitchWindow.SwitchToWindow(driver, columnValues["TabName"]);
                    PerformActionThroughClass(driver, "Click", dict["RenameTabButton"].ToString(), TestCaseSheet.Tables[0].Rows[0], "");
                    Wait(2000);
                    IWebElement name = driver.FindElement(By.XPath(dict["InputNameBox"].ToString()));
                    Actions action = new Actions(driver);
                    action.MoveToElement(name).Click()
                        .KeyDown(Keys.Control)
                        .SendKeys("a")
                        .KeyUp(Keys.Control).SendKeys(Keys.Backspace).SendKeys(columnValues["New Name"])
                        .Perform();
                    driver.FindElement(By.XPath(dict["Button_Save"].ToString())).Click();
                    Wait(6000);
                    SwitchWindow.SwitchToWindow(driver, columnValues["New Name"], true);

                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("NewTabRTPNL"))
                {
                    IWebElement name = driver.FindElement(By.XPath(dict["InputNameBox"].ToString()));
                    Actions action = new Actions(driver);
                    action.MoveToElement(name).Click().Perform();
                    action.SendKeys(columnValues["TabName"].ToString()).Perform();
                    driver.FindElement(By.XPath(dict["Button_Save"].ToString())).Click();
                    Wait(3000);
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("ClickElement"))
                {
                    try
                    {
                        SamsaraGridOperationHelper.ClickElement(driver, table.Rows[i]["Steps"].ToString(), ref dict, table.Rows[i]["Action"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                        if (rethrow)
                            throw;
                    }
                }

                else if (dataTable.Rows[i]["Steps"].ToString().Contains("VerifyExportedData"))
                {
                    try
                    {
                        Thread.Sleep(6000);
                        string input = dataTable.Rows[i]["Action"].ToString();
                        int indexOfDash = input.IndexOf("-");
                        string fileName = input.Substring(0, indexOfDash);
                        string sheetName = input.Substring(indexOfDash + 1);

                        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                        string filePath2 = SearchFile(downloadsPath, fileName);
                        if (!string.IsNullOrEmpty(filePath2))
                        {
                            DataSet ds = new DataSet();
                            List<string> list = new List<string>();

                            // if(string.Equals(StepName,"VerifySubOrder",StringComparison.OrdinalIgnoreCase))
                            if (ApplicationArguments.GroupedDataOnStepsList.Contains(StepName))
                            {
                                DataTable dt = getGroupedData(filePath2);
                                ds.Tables.Add(dt);

                            }
                            else
                                ds = DataUtilities.GetTestCaseTestData(filePath2, 1, 1, list);

                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables.Contains(sheetName))
                                    Data = ds.Tables[sheetName];
                                else
                                    Data = ds.Tables[0];
                            }
                            else
                            {
                                Data = null;
                                Console.WriteLine("VerifyExportedData teststep requires correction...Data not found");
                            }

                            if (ExcelData == null)
                            {
                                ExcelData = TestCaseSheet.Tables[0];
                            }
                            List<String> columns = new List<string>();

                            DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                            Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref ExcelData);
                            SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandler"], ref ExcelData);

                            DataUtilities.VerifyDate(ExcelData, Data);

                        }


                    }
                    catch { }


                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("VerifyExported"))
                {
                    List<string> errors = new List<string>();
                    string[] columns = { };
                    string Sheet = dataTable.Rows[i]["Action"].ToString().Split(',')[1];
                    string fileName = dataTable.Rows[i]["Action"].ToString().Split(',')[0];
                    string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    string filePath2 = SearchFile(downloadsPath, fileName);
                    if (!string.IsNullOrEmpty(filePath2))
                    {
                        List<string> list = new List<string>();
                        DataSet ds = DataUtilities.GetTestCaseTestData(filePath2, 1, 1, list);
                        Data = ds.Tables.Count > 0 ? ds.Tables[Sheet] : null;
                    }
                    Data = removeSuborderRows(Data);
                    if (Data != null)
                        errors = verifyData(ExcelData, Data, columns);
                    else
                        throw new Exception("Data is empty");
                    if (errors.Count > 0)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                        _res.ErrorMessage = String.Join("\n\r", errors);
                        throw new Exception("Value not matched" + errors.ToArray());
                    }

                }
                else if (table.Rows[i]["Steps"].ToString() == "Wait")
                {
                    int time = 3;
                    try
                    {
                        time = Convert.ToInt32(table.Rows[i]["Time(In Seconds)"]);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //Task.Delay(time);
                    Wait(time * 1000);

                }
                else if (table.Rows[i]["Steps"].ToString().Equals("CheckRowCountOnUIGrid"))
                {
                    try
                    {
                        int tempRowCount = SamsaraGridOperationHelper.CheckRowCountOnUIGrid(driver, table.Rows[i]["Action"].ToString());
                        string ans = string.Empty;
                        if (!string.IsNullOrEmpty(table.Rows[i]["OpenOtherModule"].ToString()))
                        {
                            ans = table.Rows[i]["OpenOtherModule"].ToString();
                            if (tempRowCount == 1)
                            {
                                DataTable dt = DataTables[table.Rows[i]["OpenOtherModule"].ToString()];
                                //SamsaraGridOperationHelper.PerformActionsOnGrid(driver, DataTables[table.Rows[i]["OpenOtherModule"].ToString()] );
                                for (int iterator = 0; iterator < dt.Rows.Count; iterator++)
                                {
                                    try
                                    {
                                        if (dt.Rows[iterator]["Steps"].ToString() == "SwitchWindow")
                                        {
                                            SwitchWindow.SwitchToWindow(driver, dt.Rows[iterator]["Action"].ToString());
                                        }
                                        else if (dt.Rows[iterator]["Steps"].ToString().Contains("DefaultCellValue"))
                                        {
                                            SetDefaultSetValue(dt.Rows[iterator]["Action"].ToString());
                                        }
                                        else if (dt.Rows[iterator]["Steps"].ToString().Contains("UpdateRowIndex"))
                                        {
                                            UpdateRowIndex(int.Parse(dt.Rows[iterator]["Action"].ToString()));
                                        }
                                        else if (dt.Rows[iterator]["Steps"].ToString().Contains("ClickMenuItemByText"))
                                        {
                                            ClickMenuItemByText(driver, dt.Rows[iterator]["Action"].ToString(), 3);
                                        }
                                        else if (dt.Rows[iterator]["Action"].ToString().Contains("BreakLoop"))
                                        {
                                            needNextIteration = false;
                                            break;
                                        }
                                        else
                                            PerformActionsVerification(driver, dt.Rows[iterator]["Action"].ToString(), dict["SubOrderGridAGgridID"].ToString(), "Suborder");

                                    }
                                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                                }

                                break;
                            }
                        }
                        else
                        {
                            GlobalRowCount = tempRowCount;

                        }




                    }
                    catch { }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("VerifyData") || dataTable.Rows[i]["Steps"].ToString().Contains("CheckRow"))
                {
                    try
                    {
                        var XPath = MappingDictionary[dataTable.Rows[i]["Action"].ToString()];
                        //var Grid = driver.FindElement(By.XPath(XPath.ToString()));
                        var url = driver.Title;
                        //var ScrollBar = MappingDictionary[dataTable.Rows[i]["Action"].ToString() + "ScrollBar"];
                        //var ScrollBarElement = driver.FindElement(By.XPath(ScrollBar.ToString()));
                        Data = GetData(dict["SubOrderColumnsAG"].ToString(), dict["SubOrderRowAG"].ToString(), actions, js, driver);

                    }
                    catch { }


                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("VerifyRollover"))
                {
                    Wait(12000);
                    Data = SamsaraGridOperationHelper.ExtractGridData(driver, ref dict, table.Rows[i]["Action"].ToString() + "Headers", table.Rows[i]["Action"].ToString() + "DataItems", table.Rows[i]["Action"].ToString() + "VerticalScroller", table.Rows[i]["Action"].ToString() + "HorizontalScroller");

                    if (ExcelData == null)
                    {
                        ExcelData = TestCaseSheet.Tables[0];
                    }
                    Data = DataUtilities.RemoveCommas(Data);
                    Data = DataUtilities.RemovePercent(Data);
                    Data = DataUtilities.RemoveTrailingZeroes(Data);
                    List<String> columns = new List<string>();

                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref ExcelData);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandler"], ref ExcelData);

                    columns.Add("Status (With Rollover)");
                    List<String> errors = Recon.RunRecon(Data, ExcelData, columns, 0.01);
                    if (errors.Count > 0)
                    {
                        foreach (string error in errors)
                        {
                            Console.WriteLine(error);
                        }
                        string errorMessage = ApplicationArguments.TestCaseToBeRun + " failed at step - " + StepName + " Errors:\n";
                        foreach (string error in errors)
                        {
                            Console.WriteLine(error);
                            errorMessage += error + "\n";
                        }
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " Verification failure");
                        throw new Exception(errorMessage);
                    }


                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("SelectSubOrder"))
                {
                    try
                    {
                        var XPath = "";
                        String[] arr = dataTable.Rows[i]["Action"].ToString().Split(',');
                        if (arr.Length == 1)
                        {
                            XPath = MappingDictionary[dataTable.Rows[i]["Action"].ToString()].ToString();
                        }
                        else
                        {
                            XPath = MappingDictionary[arr[0]].ToString();
                        }

                        //var Grid = driver.FindElement(By.XPath(XPath.ToString()));
                        var url = driver.Title;
                        /*var ScrollBar = MappingDictionary[dataTable.Rows[i]["Action"].ToString() + "ScrollBar"];
                        var ScrollBarElement = driver.FindElement(By.XPath(ScrollBar.ToString()));*/
                        IWebElement ScrollBarElement = driver.FindElement(By.XPath(dict["SubOrderGridHorizontalScroller"].ToString()));
                        Data = GetData(dict["SubOrderColumnsAG"].ToString(), dict["SubOrderRowAG"].ToString(), actions, js, driver);

                        //RemoveColumnsAndRows
                        List<string> Columns = new List<string>();
                        Columns.Add("Symbol");
                        Columns.Add("Side");
                        Columns.Add("Quantity");
                        DataRow dtRow = GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(Data), TestCaseSheet.Tables[0].Rows[0], Columns);
                        RowIndex = Data.Rows.IndexOf(dtRow) + 2;

                        string id = string.Format(dict["SubOrderGridAGgridID"].ToString(), RowIndex, 2); 
                        //DefaultCellValue = id;
                        if (ScrollBarElement != null)
                        {
                            SamsaraGridOperationHelper.ResetSlider(driver, dict["SubOrderGridHorizontalScroller"].ToString());
                        }
                        actions = new Actions(driver);
                        IWebElement divElement = driver.FindElement(By.XPath(id));
                        //IList<IWebElement> spanElements = divElement.FindElements(By.TagName("span"));
                        if (arr.Length == 1)
                        {
                            actions.DoubleClick(divElement)
                                       .SendKeys(Keys.Enter)
                                       .Build()
                                       .Perform();
                        }
                        else
                        {
                            actions.ContextClick(divElement)
                                       .Build()
                                       .Perform();
                        }



                        RowIndex = 1;

                    }
                    catch { }
                }

                else if (dataTable.Rows[i]["Steps"].ToString().Contains("SelectSummaryTab"))
                {
                    try
                    {
                        if (ExcelData == null)
                        {
                            ExcelData = TestCaseSheet.Tables[0];
                        }
                        var XPath = MappingDictionary[dataTable.Rows[i]["Action"].ToString()].ToString();


                        //var Grid = driver.FindElement(By.XPath(XPath.ToString()));
                        var url = driver.Title;
                        /*var ScrollBar = MappingDictionary[dataTable.Rows[i]["Action"].ToString() + "ScrollBar"];
                        var ScrollBarElement = driver.FindElement(By.XPath(ScrollBar.ToString()));*/

                        Data = GetData(XPath.ToString(), null, actions, js, driver);

                        //RemoveColumnsAndRows
                        List<string> Columns = new List<string>();
                        Columns.Add("Symbol");
                        Columns.Add("Side");
                        DataRow dtRow = GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(Data), ExcelData.Rows[0], Columns);
                        RowIndex = Data.Rows.IndexOf(dtRow) + 2;
                        if (Data.Rows.IndexOf(dtRow) < 0)
                        {
                            CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " Trade not found[SummaryGrid]");
                            string quantity = TestCaseSheet.Tables[0].Columns.Contains("Target Qty") ? TestCaseSheet.Tables[0].Rows[0]["Target Qty"].ToString() : TestCaseSheet.Tables[0].Rows[0]["Quantity"].ToString();
                            RowIndex = 1;
                            throw new Exception("Trade not found during " + StepName + " step on Summary Grid. [Symbol= " + TestCaseSheet.Tables[0].Rows[0]["Symbol"] + "], Quantity = [" + quantity + "] Side = [" + TestCaseSheet.Tables[0].Rows[0]["Side"] + "]. Please refer to Samsara Screenshot");
                        }
                        string id = "#cellid3r" + RowIndex + "c4";
                        actions = new Actions(driver);
                        IWebElement divElement = driver.FindElement(By.CssSelector(id));
                        IList<IWebElement> spanElements = divElement.FindElements(By.TagName("span"));

                        foreach (var spanElement in spanElements)
                        {
                            actions.ContextClick(spanElement)
                                   .Build()
                                   .Perform();
                        }




                        RowIndex = 1;

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("DeleteOldFile"))
                {
                    try
                    {
                        string fileName = dataTable.Rows[i]["Action"].ToString();
                        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                        string filePath2 = SearchFile(downloadsPath, fileName);
                        if (!string.IsNullOrEmpty(filePath2))
                        {
                            DeleteFile(filePath2);
                            Console.WriteLine("File " + fileName + " found and deleted successfully.");

                        }
                        else
                        {
                            Console.WriteLine("File " + fileName + " not found in the Downloads directory.");
                        }

                    }
                    catch { }


                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("GetColumnValue"))
                {
                    try
                    {
                        if (table.Rows[i]["Steps"].ToString().Contains("GetColumnValue"))
                        {
                            List<string> columns = new List<string>();
                            columns.AddRange(table.Rows[i]["Action"].ToString().Split(','));
                            for (int k = 0; k < columns.Count; k++)
                            {
                                
                                    columnValues.Add(columns[k], Convert.ToString(TestCaseSheet.Tables[0].Rows[0][columns[k]]));
                                
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                    }

                }
                else if (dataTable.Rows[i]["Action"].ToString().Contains("ClickAndSendKeysOnGrid"))
                {
                    try
                    {
                        if (dataTable.Rows[i]["Steps"].ToString().Contains("HandleSoftWithNotes"))
                        {
                            int colindex = Data.Columns.IndexOf("User Notes");
                            string searchString = "User Notes";
                            int maxTry = 20;
                            List<int> foundIndices = DataUtilities.FindIndicesInDataTable(ref Data, ref searchString);
                            if (foundIndices.Count != 0)
                            {

                                for (int ik = 0; ik < foundIndices.Count; ik++)
                                {
                                    string id = DefaultCellValue + (ik + 2) + "c" + colindex;
                                    IWebElement element = driver.FindElement(By.CssSelector(id));

                                    IList<IWebElement> divElements = driver.FindElements(By.CssSelector(id));
                                    Actions actions2 = new Actions(driver);
                                    foreach (var divElement in divElements)
                                    {
                                        string existingValue = divElement.Text;
                                        Console.WriteLine("Existing value in cell " + id + ": " + existingValue);

                                        IList<IWebElement> spanElements = divElement.FindElements(By.TagName("span"));
                                        bool rightValue = false;
                                        foreach (var spanElement in spanElements)
                                        {
                                            while (!rightValue && maxTry > 0)
                                            {
                                                try
                                                {
                                                    actions2.Click(spanElement)
                                                     .SendKeys(Keys.Control + "a")
                                                     .SendKeys(Keys.Backspace)
                                                     .Perform();
                                                    actions.SendKeys(columnValues["Write UserNote"].ToString())
                                             .SendKeys(Keys.Enter)
                                             .Build()
                                             .Perform();
                                                    Thread.Sleep(3000);
                                                    Console.WriteLine(spanElement.Text);


                                                    string value = spanElement.Text.ToString();
                                                    string cleanedString = value.Replace(",", "");
                                                    if (cleanedString == columnValues["Write UserNote"].ToString())
                                                    {

                                                        rightValue = true;

                                                    }
                                                    maxTry--;
                                                }

                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                            }
                                        }
                                    }


                                }


                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                else if (dataTable.Rows[i]["Steps"].ToString().StartsWith("Column"))
                {
                    try
                    {
                        string name = table.Rows[i]["Steps"].ToString().Replace("Column_", "");
                        string xpathName = string.Empty;
                        if (!columnValues.Keys.Contains(name))
                        {
                            xpathName = name + "_";
                        }
                        else
                        {
                            xpathName = name + "_" + columnValues[name];
                        }

                        PerformActionsVerification(driver, dataTable.Rows[i]["Action"].ToString(), MappingDictionary[xpathName].ToString(), table.Rows[i]["Steps"].ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                        if (rethrow)
                            throw;
                    }

                }
                else if (dataTable.Rows[i]["Steps"].ToString() == "DefaultCellValue")
                {
                    try
                    {
                        DefaultCellValue = dataTable.Rows[i]["Action"].ToString();
                    }
                    catch { }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Contains("GetIndex"))
                {
                    try
                    {
                        Thread.Sleep(6000);
                        if (ExcelData == null)
                        {
                            ExcelData = TestCaseSheet.Tables[0];
                        }
                        string fileName = dataTable.Rows[i]["Action"].ToString();
                        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                        string filePath2 = SearchFile(downloadsPath, fileName);
                        if (!string.IsNullOrEmpty(filePath2))
                        {
                            List<string> list = new List<string>();
                            DataSet ds = DataUtilities.GetTestCaseTestData(filePath2, 1, 1, list);
                            Data = ds.Tables.Count > 0 ? ds.Tables[0] : null;
                        }
                        Data = removeSuborderRows(Data);
                        //RemoveColumnsAndRows
                        List<string> Columns = new List<string>();
                        Columns.Add("Symbol");
                        Columns.Add("Quantity");
                        Columns.Add("Side");
                        DataRow dtRow = GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(Data), ExcelData.Rows[0], Columns);
                        RowIndex = Data.Rows.IndexOf(dtRow);
                        //RowIndex += 1;
                        if (Data.Rows.IndexOf(dtRow) < 0)
                        {
                            CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " Trade not found");
                            string quantity = TestCaseSheet.Tables[0].Columns.Contains("Target Qty") ? TestCaseSheet.Tables[0].Rows[0]["Target Qty"].ToString() : TestCaseSheet.Tables[0].Rows[0]["Quantity"].ToString();
                            RowIndex = 1;
                            throw new Exception("Trade not found during " + StepName + " step: [Symbol= " + TestCaseSheet.Tables[0].Rows[0]["Symbol"] + "], Quantity = [" + quantity + "] Side = [" + TestCaseSheet.Tables[0].Rows[0]["Side"] + "]. Please refer to Samsara Screenshot");
                        }
                        if (RowIndex >= 7)
                        {
                            try
                            {
                                var VerticalscrollBar = MappingDictionary["OrderGridVerticalScrollBar"];
                                var VerticalscrollBarElement = driver.FindElement(By.XPath(VerticalscrollBar.ToString()));
                                verticalScroll(VerticalscrollBarElement, actions, js, driver, 300);
                            }
                            catch { }
                            // dtRow = GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(Data), TestCaseSheet.Tables[0].Rows[0], Columns);
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("TradeData"))
                {
                    try
                    {
                        OpenModule("TradingTicket", driver);
                        var TTDict = GetDict(SamsaraMappingTables["TradingTicket"]);
                        SwitchWindow.SwitchToWindow(driver, "Trading Ticket");

                        string xpath = TTDict["Symbol"].ToString();
                        PerformActionByData(dataTable, driver, i, xpath, "SendKeys", dataTable.Rows[i]["Action"].ToString());
                        Thread.Sleep(10000);

                        xpath = TTDict["Quantity"].ToString();
                        PerformActionByData(dataTable, driver, i, xpath, "SendKeys", "50");

                        xpath = TTDict["AllocationMethod"].ToString();
                        PerformActionByData(dataTable, driver, i, xpath, "SendKeys", "Unallocated");

                        xpath = TTDict["Price"].ToString();
                        PerformActionByData(dataTable, driver, i, xpath, "SendKeys", "50");

                        xpath = TTDict["Button_DoneAway"].ToString();
                        PerformActionByData(dataTable, driver, i, xpath, "Click");
                        
                        xpath = TTDict["Buy"].ToString();
                        PerformActionByData(dataTable, driver, i, xpath, "Click");

                        xpath = TTDict["ContinueButton_NewSub"].ToString();
                        try
                        {
                            PerformActionByData(dataTable, driver, i, xpath, "Click");
                        }
                        catch { }
                        SwitchWindow.SwitchToWindow(driver, "Pre-Trade Compliance Results");
                        xpath = TTDict["AllowTrade_Yes"].ToString();
                        PerformActionByData(dataTable, driver, i, xpath, "Click");

                        Thread.Sleep(5000);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in Trade Data action");
                    }

                }
                    //Made a better effecient method to verify columns , Modified by yash
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("VerifyColumns"))
                {
                    Dictionary<string, string> columnDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    HashSet<string> discoveredColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    string headerXPath = dict["FundLevelAggregationHeaders"].ToString();
                    int maxScrollAttempts = 60;
                    int scrollCount = 0;
                    IWebElement startingCell = null;
                    try { startingCell = driver.FindElement(By.XPath(dict["FundLevelAggregationHorizontalScroller"].ToString())); }
                    catch { startingCell = driver.FindElement(By.XPath(dict2["FundLevelAggregationHorizontalScroller"].ToString())); }
                    startingCell.Click();
                    //  Step 1: Collect all columns
                    while (scrollCount < maxScrollAttempts)
                    {
                        IList<IWebElement> headerElements = driver.FindElements(By.XPath(headerXPath));
                        int visibleCount = headerElements.Count;

                        foreach (var header in headerElements)
                        {
                            try
                            {
                                string colName = header.Text.Trim();
                                if (string.IsNullOrEmpty(colName)) continue;

                                if (!discoveredColumns.Contains(colName))
                                {
                                    IWebElement parent = header.FindElement(
                                        By.XPath("ancestor::div[contains(@class,'ag-header-cell')][@col-id]"));
                                    string colId = parent.GetAttribute("col-id");

                                    discoveredColumns.Add(colName);
                                    if (!columnDict.ContainsKey(colName))
                                        columnDict.Add(colName, colId);

                                    Console.WriteLine("Collected column: " + colName + " (col-id: " + colId + ")");
                                }
                            }
                            catch { }
                        }

                        //  Scroll right = number of visible columns
                        for (int j = 0; j < visibleCount; j++)
                        {
                            try
                            {
                                actions.SendKeys(Keys.ArrowRight).Perform();
                                Thread.Sleep(100);
                            }
                            catch { }
                        }

                        scrollCount++;
                    }

                    //  Step 2: Scroll back left (reset to start)
                    for (int j = 0; j < 56; j++)
                    {
                        try
                        {
                            actions.SendKeys(Keys.ArrowLeft).Perform();
                            Thread.Sleep(30);
                        }
                        catch { }
                    }

                    //  Step 3: Verify required columns
                    string[] requiredColumns = TestCaseSheet_Temp.Tables[0].Rows[0]["ColumnToVerify"]
                                               .ToString()
                                               .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string colName in requiredColumns)
                    {
                        string trimmedCol = colName.Trim();

                        if (columnDict.ContainsKey(trimmedCol))
                        {
                            Console.WriteLine("Verified: " + trimmedCol + " (col-id: " + columnDict[trimmedCol] + ")");
                        }
                        else
                        {
                            CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun,
                                "Column '" + trimmedCol + "' not found in grid header.");
                            throw new Exception("Column not found: " + trimmedCol);
                        }
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("ApplyColumnFilterRTPNL"))
                {
                    string inputColumnName = TestCaseSheet_Temp.Tables[0].Rows[0]["Column"].ToString().Trim();
                    string filterType1 = TestCaseSheet_Temp.Tables[0].Rows[0]["FilterType1"].ToString().Trim();
                    string input1 = TestCaseSheet_Temp.Tables[0].Rows[0]["Input1"].ToString().Trim();
                    string andOr = TestCaseSheet_Temp.Tables[0].Rows[0]["AND/OR"].ToString().Trim().ToUpper();
                    string filterType2 = TestCaseSheet_Temp.Tables[0].Rows[0]["FilterType2"].ToString().Trim();
                    string input2 = TestCaseSheet_Temp.Tables[0].Rows[0]["Input2"].ToString().Trim();

                    try
                    {
                        string targetColId = null;
                        int maxScrollAttempts = 50;

                        IList<IWebElement> headerElement = driver.FindElements(By.XPath(dict["HeaderElement"].ToString()));
                        if (headerElement.Count > 0)
                        {
                            headerElement[2].Click();
                        }

                        for (int attempt = 0; attempt < maxScrollAttempts; attempt++)
                        {
                            IList<IWebElement> headerElements = driver.FindElements(By.XPath(dict["HeaderElement"].ToString()));

                            foreach (var header in headerElements)
                            {
                                string text = header.Text.Trim();
                                if (text.Equals(inputColumnName, StringComparison.OrdinalIgnoreCase))
                                {
                                    targetColId = header.GetAttribute("col-id");
                                    break;
                                }
                            }

                            if (!string.IsNullOrEmpty(targetColId))
                                break;

                            actions.SendKeys(Keys.ArrowRight).Perform();
                            Thread.Sleep(150);
                        }

                        if (string.IsNullOrEmpty(targetColId))
                        {
                            CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Column '" + inputColumnName + "' not found in grid header.");
                            throw new Exception("Column name not found after scrolling: " + inputColumnName);
                        }

                        string filterIconXPath = string.Format(dict["FilterIcon"].ToString(), targetColId);
                        IWebElement filterButton = driver.FindElement(By.XPath(filterIconXPath));
                        filterButton.Click();
                        Thread.Sleep(500);

                        if (!string.IsNullOrEmpty(filterType1))
                        {
                            IList<IWebElement> dropdowns = driver.FindElements(By.XPath(dict["FilterType"].ToString()));
                            if (dropdowns.Count >= 1)
                            {
                                dropdowns[0].Click();
                                Thread.Sleep(200);
                                IList<IWebElement> options1 = driver.FindElements(By.XPath(dict["FilterTypeList"].ToString()));
                                foreach (var option in options1)
                                {
                                    if (option.Text.Trim().Equals(filterType1, StringComparison.OrdinalIgnoreCase))
                                    {
                                        option.Click();
                                        break;
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(input1))
                        {
                            IList<IWebElement> inputs = driver.FindElements(By.XPath(dict["FilterInput"].ToString()));
                            {
                                inputs[0].Clear();
                                inputs[0].SendKeys(input1);
                                inputs[0].SendKeys(Keys.Enter);
                            }
                        }

                        if (!string.IsNullOrEmpty(andOr))
                        {
                            IList<IWebElement> radioButtons = driver.FindElements(By.XPath(dict["FilterRadio"].ToString()));
                            foreach (var btn in radioButtons)
                            {
                                if (btn.Text.Trim().ToUpper() == andOr)
                                {
                                    btn.Click();
                                    break;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(filterType2))
                        {
                            IList<IWebElement> dropdowns = driver.FindElements(By.XPath(dict["FilterType"].ToString()));
                            if (dropdowns.Count >= 2)
                            {
                                dropdowns[1].Click();
                                Thread.Sleep(200);
                                IList<IWebElement> options2 = driver.FindElements(By.XPath(dict["FilterTypeList"].ToString()));
                                foreach (var option in options2)
                                {
                                    if (option.Text.Trim().Equals(filterType2, StringComparison.OrdinalIgnoreCase))
                                    {
                                        option.Click();
                                        break;
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(input2))
                        {
                            IList<IWebElement> inputs = driver.FindElements(By.XPath(dict["FilterInput"].ToString()));
                            if (inputs.Count >= 2)
                            {
                                inputs[1].Clear();
                                inputs[1].SendKeys(input2);
                                inputs[1].SendKeys(Keys.Enter);
                            }
                        }
                        IList<IWebElement> headerElementNew = driver.FindElements(By.XPath(dict["HeaderElement"].ToString()));
                        if (headerElementNew.Count > 0)
                        {
                            headerElementNew[2].Click();
                        }
                        Thread.Sleep(500);
                    }
                    catch (Exception ex)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error applying filter for column: " + inputColumnName);
                        throw new Exception("Failed to apply filter for column '" + inputColumnName + "': " + ex.Message);
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("SwitchTabBlotter"))
                {
                    if (!string.IsNullOrEmpty(columnValues["TabItem Key Orders"].ToString()))
                    {
                        PerformActions(driver, "Click", dict["OrderButton"].ToString(), TestCaseSheet.Tables[0].Rows[0], "OrderButton");
                    }

                    if (!string.IsNullOrEmpty(columnValues["TabItem Key WorkingSubs"].ToString()))
                    {
                        PerformActions(driver, "Click", dict["WorkingTab"].ToString(), TestCaseSheet.Tables[0].Rows[0], "WorkingTab");
                    }

                    if (!string.IsNullOrEmpty(columnValues["TabItem Key Summary"].ToString()))
                    {
                        PerformActions(driver, "Click", dict["SummaryButton"].ToString(), TestCaseSheet.Tables[0].Rows[0], "SummaryButton");
                    }

                    if (!string.IsNullOrEmpty(columnValues["TabItem Key Dynamic"].ToString()))
                    {
                        int str = 4;
                        string path = null;
                        while (true)
                        {
                            IWebElement divElement = null;
                            try
                            {
                                path = dict["CustomTabPath"].ToString().Replace("str", str.ToString());

                                divElement = driver.FindElement(By.XPath(path));
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Tab not found");
                                break;
                            }
                            if (divElement.Text == columnValues["TabItem Key Dynamic"].ToString())
                            {
                                Console.WriteLine("Tab founded");
                                driver.FindElement(By.XPath(path)).Click();
                                break;
                            }
                            str++;

                        }
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("VerifyColour"))
                {
                    int row = 1;
                    int column = 1;
                    string bgc, col, style, xpath, classofobject, outline = "";
                    string attribute = TestCaseSheet.Tables[0].Rows[0]["Attribute"].ToString();
                    string horizontalScroller = "//*[@role='row'][@row-index='0']//*[contains(@role, 'gridcell')][@aria-colindex='2']";
                    bool Flag = true;
                    if (TestCaseSheet.Tables[0].Columns.Contains("CreateTableForRTPNL") && TestCaseSheet.Tables[0].Rows[0]["CreateTableForRTPNL"].ToString().ToUpper().Equals("TRUE"))
                    {
                        while (Flag)
                        {
                            try
                            {
                                string valueToFind = TestCaseSheet.Tables[0].Rows[0]["Account/MF name"].ToString();
                                Flag = driver.FindElements(By.XPath("//*[@role='gridcell'][@col-id][normalize-space(text())='" + valueToFind + "']")).Count > 0;
                                if (Flag)
                                {
                                    Console.WriteLine(valueToFind + "Found");
                                    break;
                                }
                                else
                                    row++;
                            }
                            catch (Exception ex)
                            { Console.WriteLine(ex.Message); }
                        }

                        if (!Flag)
                        {
                            int steps = 7;
                            if (row == 1)
                            {
                                steps = 16;
                            }
                            SamsaraGridOperationHelper.RightSlider(driver, horizontalScroller, steps);
                        }
                        else if (row > 1)
                            SamsaraGridOperationHelper.MoveSlider(driver, horizontalScroller);

                        string expectedColor = columnValues["Colour"].ToLower();
                        string[] arr = columnValues["Object"].ToString().Split(',');
                        bool Flag2 = true;
                        string styleAttr = "";
                        try
                        {
                            foreach (string colName in arr)
                            {
                                int retry = 0;
                                int steps = 7;
                                bool isColorMatched = false;
                                bool canSlide = true;
                                while (canSlide)
                                {
                                    IWebElement headerCell = null;
                                    var headerElements = driver.FindElements(By.XPath("//span[normalize-space(text())='" + colName + "']"));
                                    if (headerElements.Count > 0)
                                    {
                                        headerCell = headerElements[0];
                                        IWebElement parentHeaderDiv = headerCell.FindElement(By.XPath("./ancestor::div[@class = 'ag-header-cell ag-header-parent-hidden ag-header-cell-sortable custom ag-focus-managed']"));
                                        string colId = parentHeaderDiv.GetAttribute("col-id");
                                        IList<IWebElement> gridCells = driver.FindElements(By.XPath("//div[@role='gridcell' and @col-id='" + colId + "']"));
                                        foreach (IWebElement gridCell in gridCells)
                                        {
                                            styleAttr = gridCell.GetAttribute("style");
                                            isColorMatched = styleAttr.Contains(expectedColor);
                                            if (isColorMatched)
                                            {
                                                canSlide = false;
                                                break; // Exit the loop as soon as the color matches
                                            }
                                        }
                                        if (!isColorMatched)
                                        {
                                            throw new Exception("Colour mention in Excel is " + columnValues["Colour"] + " and UI is showing " + styleAttr);
                                        }
                                    }
                                    else
                                    {
                                        canSlide = true;
                                        Flag2 = false;
                                    }
                                    if (!Flag2)
                                    {
                                        if (retry == 0)
                                            steps = 16;
                                        SamsaraGridOperationHelper.RightSlider(driver, horizontalScroller, steps);
                                        retry++;
                                    }
                                }
                            }
                            SamsaraGridOperationHelper.MoveSlider(driver, horizontalScroller);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


                        /* string id = "cellid0r" + row + "c";
                         Dictionary<string, string> columnDict = new Dictionary<string, string>();
                         int retry = 0;

                         while (true)
                         {
                             try
                             {
                                 string temp_id = id + column;
                                 IWebElement parentDiv = driver.FindElement(By.Id(temp_id));
                                 IWebElement childElement = parentDiv.FindElement(By.TagName("span"));
                                 string text = childElement.Text.ToString();
                                 columnDict.Add(text, column.ToString());
                                 column++;
                             }
                             catch (Exception)
                             {
                                 try
                                 {

                                     IWebElement elm;
                                     try
                                     {
                                         elm = driver.FindElement(By.XPath(dict["ScrollBar"].ToString())); ;
                                     }
                                     catch
                                     {
                                         elm = driver.FindElement(By.XPath(dict["ScrollBar_RealTime"].ToString()));
                                     }
                                     actions.MoveToElement(elm).Perform();


                                     actions
                                         .ClickAndHold(elm)
                                         .MoveByOffset(240, 0)
                                         .Release()
                                         .Perform();

                                     Thread.Sleep(2000);
                                     retry++;
                                     if (retry == 5)
                                     {
                                         break;
                                     }
                                 }
                                 catch
                                 {
                                     break;
                                 }
                             }
                         }

                         string valueToFind = TestCaseSheet.Tables[0].Rows[0]["Account/MF name"].ToString();
                         Actions actions1 = new Actions(driver);
                         try
                         {
                             Thread.Sleep(2000);
                             IWebElement elm;
                             try
                             {
                                 elm = driver.FindElement(By.XPath(dict["ScrollBar"].ToString())); ;
                             }
                             catch
                             {
                                 elm = driver.FindElement(By.XPath(dict["ScrollBar_RealTime"].ToString()));
                             }


                             // Move to the element first
                             int retry1 = 0;
                             while (retry1 < 5)
                             {
                                 actions1.MoveToElement(elm).Perform();
                                 actions1
                                             .ClickAndHold(elm)
                                             .MoveByOffset(-130, 0)
                                             .Release()
                                             .Perform();
                                 retry1++;
                             }

                         }
                         catch
                         {
                             IWebElement elm;
                             try
                             {
                                 elm = driver.FindElement(By.XPath(dict["ScrollBar"].ToString())); ;
                             }
                             catch
                             {
                                 elm = driver.FindElement(By.XPath(dict["ScrollBar_RealTime"].ToString()));
                             }
                             actions1.MoveToElement(elm).Perform();
                             actions1
                                         .ClickAndHold(elm)
                                         .MoveByOffset(-30, 0)
                                         .Release()
                                         .Perform();
                         }

                         while (true)
                         {
                             try
                             {
                                 string FindingColumnId = "cellid0r" + row + "c1";
                                 IWebElement parentDiv = driver.FindElement(By.Id(FindingColumnId));
                                 IWebElement childElement = parentDiv.FindElement(By.TagName("span"));
                                 string text = childElement.Text.ToString();
                                 if (text.ToLower().Equals(valueToFind.ToLower()))
                                 {
                                     Console.WriteLine(valueToFind + " founded on row number " + (row - 1));
                                     break;
                                 }
                             }
                             catch (Exception)
                             {
                                 break;
                             }
                             row++;
                         }

                         string[] arr = columnValues["Object"].ToString().Split(',');
                         for (int j = 0; j < arr.Length; j++)
                         {
                             try
                             {
                                 string temp_id;
                                 try
                                 {
                                     temp_id = "cellid0r" + row + "c" + columnDict[arr[j]];
                                 }
                                 catch (Exception)
                                 {
                                     j += 1;
                                     throw new Exception("Dictionary does not contain Defination for " + arr[j]);
                                 }
                                 IWebElement parentDiv = driver.FindElement(By.Id(temp_id));
                                 IWebElement childElement = parentDiv.FindElement(By.TagName("div"));
                                 var attributes = childElement.GetAttribute(attribute);
                                 if (attributes.Contains(columnValues["Colour"].ToLower()))
                                 {
                                     Console.WriteLine(columnValues["Colour"] + " Colour Founded");
                                     continue;
                                 }
                                 int index = attributes.IndexOf("(") + 1;
                                 style = rgbcolor(attributes.Substring(index));
                                 bool flag = style.Equals(columnValues["Colour"].ToLower());
                                 if (!flag)
                                 {
                                     throw new Exception("Colour mention in Excel is " + columnValues["Colour"] + " and UI is showing " + style);
                                 }

                             }
                             catch (Exception)
                             {

                                 try
                                 {
                                     IWebElement elm;
                                     try
                                     {
                                         elm = driver.FindElement(By.XPath(dict["ScrollBar"].ToString())); ;
                                     }
                                     catch
                                     {
                                         elm = driver.FindElement(By.XPath(dict["ScrollBar_RealTime"].ToString()));
                                     }
                                     actions.MoveToElement(elm).Perform();


                                     actions
                                         .ClickAndHold(elm)
                                         .MoveByOffset(100, 0)
                                         .Release()
                                         .Perform();

                                     Thread.Sleep(2000);
                                     j = j - 1;
                                 }
                                 catch
                                 {
                                     try
                                     {
                                         IWebElement elm = driver.FindElement(By.XPath(dict["ScrollBar"].ToString()));
                                         actions.MoveToElement(elm).Perform();


                                         actions
                                             .ClickAndHold(elm)
                                             .MoveByOffset(100, 0)
                                             .Release()
                                             .Perform();

                                         Thread.Sleep(2000);
                                         j = j - 1;
                                     }
                                     catch
                                     {
                                         j = j - 1;
                                     }
                                 }
                             }
                         }*/

                    }
                    else
                    {
                        try
                        {
                            xpath = MappingDictionary[columnValues["Object"]].ToString();
                            IWebElement parentDiv = driver.FindElement(By.XPath(xpath));
                            IWebElement childElement = parentDiv;

                            if (!String.IsNullOrEmpty(columnValues["IsSvg?"].ToString()) && columnValues["IsSvg?"].ToString().ToUpper().Equals("TRUE"))
                            {
                                childElement = parentDiv.FindElement(By.TagName("svg"));
                            }
                            var attributes = childElement.GetAttribute(attribute);
                            if (attribute.ToUpper().Equals("STYLE"))
                            {
                                if (attributes.Contains(columnValues["Colour"].ToLower()))
                                {
                                    Console.WriteLine(columnValues["Colour"] + " Colour Founded");
                                    continue;
                                }
                                int index = attributes.IndexOf("(") + 1;
                                style = rgbcolor(attributes.Substring(index));
                                bool flag = style.Equals(columnValues["Colour"].ToLower());
                                if (!flag)
                                {
                                    throw new Exception("Colour mention in Excel is " + columnValues["Colour"] + " and UI is showing " + style);
                                }
                            }
                            else if (attribute.ToUpper().Contains("CLASS"))
                            {
                                if (attribute.ToUpper().Contains("OUTLINE"))
                                {
                                    outline = childElement.GetCssValue("outline");
                                }
                                int index = outline.IndexOf("(") + 1;
                                style = rgbcolor(outline.Substring(index));
                                bool flag = style.Equals(columnValues["Colour"].ToLower());
                                if (!flag)
                                {
                                    throw new Exception("Colour mention in Excel is " + columnValues["Colour"] + " and UI is showing " + style);
                                }

                            }


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("ChangeView"))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(columnValues["ViewName"].ToString()))
                        {
                            string view = columnValues["ViewName"].ToString().ToLower() == "graph" ? "chart" : "grid";
                            IWebElement parentDiv = driver.FindElement(By.XPath(dict["ViewOfWidget"].ToString()));
                            IList<IWebElement> childrenWithTestId = parentDiv.FindElements(By.CssSelector("[data-testid]"));

                            foreach (IWebElement child in childrenWithTestId)
                            {
                                string dataTestIdValue = child.GetAttribute("data-testid");
                                if (dataTestIdValue.ToLower().Equals(view))
                                {
                                    child.Click();
                                    break;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(columnValues["VerifyView"].ToString()))
                        {
                            string view = columnValues["VerifyView"].ToString().ToLower() == "graph" ? "chart" : "grid";
                            IWebElement parentDiv = driver.FindElement(By.XPath(dict["ViewOfWidget"].ToString()));
                            IList<IWebElement> childrenWithTestId = parentDiv.FindElements(By.CssSelector("[data-testid]"));

                            foreach (IWebElement child in childrenWithTestId)
                            {
                                string dataTestIdValue = child.GetAttribute("data-testid");
                                if (dataTestIdValue.ToLower().Equals(view))
                                {
                                    string ClassValue = child.GetAttribute("class");
                                    if (ClassValue.Contains("active"))
                                    {
                                        Console.WriteLine("Current view is " + columnValues["VerifyView"].ToString().ToUpper() + " view");
                                    }
                                    else
                                    {
                                        throw new Exception("Current view is not " + columnValues["VerifyView"].ToString().ToUpper() + " view");
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " error at widget view");
                        throw ex;
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("OpenCustomTab"))
                {

                    int str = 4;
                    string path = null;
                    while (true)
                    {
                        IWebElement divElement = null;
                        try
                        {
                            path = dict["CustomTabPath"].ToString().Replace("str", str.ToString());

                            divElement = driver.FindElement(By.XPath(path));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Tab not found");
                            break;
                        }
                        if (divElement.Text == columnValues["TabName"])
                        {
                            TestCaseSheet_Temp.Tables[0].Columns.Add("tab index");
                            TestCaseSheet_Temp.Tables[0].Rows[0]["tab index"] = str;
                            Console.WriteLine("Tab founded");
                            driver.FindElement(By.XPath(path)).Click();
                            break;
                        }
                        str++;

                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("CustomTabAction"))
                {
                    int tabIndex = Int32.Parse(TestCaseSheet_Temp.Tables[0].Rows[0]["tab index"].ToString());
                    TestCaseSheet_Temp.Tables[0].Columns.Remove("tab index");
                    TestCaseSheet_Temp.Tables[0].Columns.Remove("Action");
                    TestCaseSheet_Temp.Tables[0].Columns.Remove("ActionOnWhichGrid");
                    TestCaseSheet_Temp.Tables[0].Columns.Remove("TabName");
                    columnValues.Add("tab", tabIndex.ToString());

                    string cellID = "cellid" + tabIndex + "r1c1";
                    int retry = 0;
                    while (retry <= 5)
                    {
                        cellID = "cellid" + tabIndex + "r1c1";
                        try
                        {
                            driver.FindElement(By.Id(cellID)).Click();
                            break;
                        }
                        catch
                        {
                            retry++;
                            tabIndex++;
                        }

                    }


                    if (columnValues["ActionOnWhichGrid"].ToLower().Equals("suborder"))
                    {
                        tabIndex++;
                    }
                    int row = 1, column = 1;
                    DataTable dt = new DataTable();
                    while (true)
                    {
                        try
                        {
                            string temp_cellID = "cellid" + tabIndex + "r" + row + "c" + column;
                            IWebElement parentDiv = driver.FindElement(By.Id(temp_cellID));
                            IWebElement childElement = parentDiv.FindElement(By.TagName("span"));
                            string text = childElement.Text.ToString();
                            Console.WriteLine(text + " column added");
                            dt.Columns.Add(text);
                        }
                        catch
                        {
                            break;
                        }
                        column++;

                    }
                    column = 1;
                    row = 2;
                    int tempnum = 0;
                    DataRow newRow = dt.NewRow();
                    while (true)
                    {
                        try
                        {
                            string temp_cellID = "cellid" + tabIndex + "r" + row + "c" + column;
                            IWebElement parentDiv = driver.FindElement(By.Id(temp_cellID));
                            IWebElement childElement = parentDiv.FindElement(By.TagName("span"));
                            string text = childElement.Text.ToString();
                            newRow[tempnum] = text;
                        }
                        catch
                        {
                            row++;
                            column = 1;
                            tempnum = 0;
                            dt.Rows.Add(newRow);
                            newRow = dt.NewRow();
                            try
                            {
                                string temp_cellID = "cellid" + tabIndex + "r" + row + "c" + column;
                                IWebElement parentDiv = driver.FindElement(By.Id(temp_cellID));
                                continue;
                            }
                            catch
                            {
                                break;
                            }
                        }
                        column++;
                        tempnum++;
                    }
                    List<string> Columns = new List<string>();
                    Columns.Add("Symbol");
                    Columns.Add("Status");
                    Columns.Add("Side");

                    DataRow dtRow = GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dt), TestCaseSheet_Temp.Tables[0].Rows[0], Columns);
                    RowIndex = dt.Rows.IndexOf(dtRow) + 2;
                    columnValues.Add("tabindex", tabIndex.ToString());
                    columnValues.Add("rowindex", RowIndex.ToString());


                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("PerformActionsOfBlotter"))
                {
                    string id = "cellid" + columnValues["tabindex"] + "r" + columnValues["rowindex"] + "c0";
                    /*Check box handling in Custom Tab
                      *https://dev.azure.com/NirvanaSolutions/NirvanaOne/_workitems/edit/60744
                      *Bhoomika Mittal
                     */
                    Actions action = new Actions(driver);
                    IWebElement element = driver.FindElement(By.Id(id));
                    if (columnValues["Action"].ToLower().Equals("select") || string.IsNullOrEmpty(columnValues["Action"]))
                    {
                        action.Click(element).Perform();
                    }
                    else
                    {
                        action.ContextClick(element).Perform();
                        string Xpath = dict[columnValues["ActionOnWhichGrid"].ToString().ToLower() + "ContextMenu"].ToString();
                        Xpath = Xpath.Replace("tab", columnValues["tab"]);
                        SamsaraGridOperationHelper.PerformRightClickActions(driver, columnValues["Action"], Xpath);
                        Wait(4000);
                        if (columnValues["Action"].ToLower().Contains("cancel") || columnValues["Action"].ToLower().Contains("remove"))
                        {
                            try
                            {
                                PerformActionsVerification(driver, "Click", dict["ConfirmCancel"].ToString(), "Cancel");
                            }
                            catch
                            {
                                CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                            }

                        }
                    }

                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("VerifyCustomAllocationWindowonTT"))
                {
                    TestCaseSheet_Temp.Tables[0].Columns["Current %"].ColumnName = "Current Percent";
                    TestCaseSheet_Temp.Tables[0].Columns["Allocation %"].ColumnName = "Allocated Percent";
                    IWebElement element = driver.FindElement(By.Id(dict["CustomAllocationID"].ToString()));
                    string id = string.Empty;
                    var allElements = element.FindElements(By.XPath(".//*"));
                    var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                    foreach (var elements in columnHeaderElements)
                    {
                        string elementId = elements.GetAttribute("id");
                        id = elementId.Substring(0, elementId.IndexOf("r") + 1);
                        break;
                    }
                    DataTable dt = new DataTable();
                    for (int col = 0; col < 5; col++)
                    {
                        string temp_cellID = id + "1c" + col;
                        IWebElement parentDiv = driver.FindElement(By.Id(temp_cellID));
                        IWebElement childElement = parentDiv.FindElement(By.TagName("span"));
                        string text = childElement.Text.ToString();
                        Console.WriteLine(text + " column added");
                        dt.Columns.Add(text);
                    }

                    int column;
                    int rowcount = TestCaseSheet_Temp.Tables[0].Rows.Count + 2;
                    for (int row = 2; row < rowcount; row++)
                    {
                        column = 0;
                        DataRow newRow = dt.NewRow();
                        while (column < 5)
                        {
                            string temp_cellID = id + row + "c" + column;
                            IWebElement parentDiv = driver.FindElement(By.Id(temp_cellID));
                            IWebElement childElement = parentDiv.FindElement(By.TagName("span"));
                            string text = childElement.Text.ToString();
                            newRow[column] = text;
                            column++;
                        }
                        dt.Rows.Add(newRow);
                    }

                    List<string> Columns = new List<string>();
                    Columns.Add("Account");
                    Columns.Add("Current Quantity");
                    Columns.Add("Current Percent");
                    dt = DataUtilities.RemoveTrailingZeroes(dt);
                    List<String> errors = Recon.RunRecon(dt, TestCaseSheet_Temp.Tables[0], Columns, 0.01);
                    if (errors.Count > 0)
                    {
                        foreach (string error in errors)
                        {
                            Console.WriteLine(error);
                        }
                        string errorMessage = ApplicationArguments.TestCaseToBeRun + " failed at step - " + StepName + " Errors:\n";
                        foreach (string error in errors)
                        {
                            Console.WriteLine(error);
                            errorMessage += error + "\n";
                        }
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                        throw new Exception(errorMessage);

                    }
                    else
                    {
                        ApplicationArguments.isVerificationSuceeded = true;
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("VerifyTheme"))
                {
                    if (!columnValues["ChangeThemeThrough"].ToUpper().Equals("BUTTON"))
                    {
                        PerformActionsVerification(driver, "Click", dict["BrowserMenuDropdown"].ToString(), "");
                        Wait(2000);
                        for (int j = 0; j < 3; j++)
                        {
                            Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        }
                        Wait(1000);
                        Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                        Wait(1500);
                        if (columnValues["DarkOrLight?"].ToUpper().Equals("LIGHT"))
                        {
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                        }
                        else if (columnValues["DarkOrLight?"].ToUpper().Equals("DARK"))
                        {
                            Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                        else
                        {
                            Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                            Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                    }
                    else
                    {
                        string currentTheme = null;
                        if (String.IsNullOrEmpty(columnValues["TabName"]))
                        {
                            currentTheme = getThemeOfModule(driver, dataTable.Rows[0]["Action"].ToString(), dict["ModuleBody"].ToString());
                        }
                        else
                        {
                            currentTheme = getThemeOfModule(driver, columnValues["TabName"], dict["ModuleBody"].ToString());
                        }
                        if (!currentTheme.ToUpper().Equals(columnValues["DarkOrLight?"].ToUpper()))
                        {
                            SwitchWindow.SwitchToParentWindow(driver, dataTable.Rows[0]["Action"].ToString(), dict["CloseModule"].ToString());
                            PerformActionsVerification(driver, "Click", dict["ThemeChangeButton"].ToString(), "");
                            Console.WriteLine("Change to " + columnValues["DarkOrLight?"].ToUpper() + " theme");
                        }
                    }

                    string getTheme = null;
                    if (String.IsNullOrEmpty(columnValues["TabName"]))
                    {
                        getTheme = getThemeOfModule(driver, dataTable.Rows[0]["Action"].ToString(), dict["ModuleBody"].ToString());
                    }
                    else
                    {
                        getTheme = getThemeOfModule(driver, columnValues["TabName"], dict["ModuleBody"].ToString());
                    }

                    if (!getTheme.ToUpper().Equals(columnValues["VerifyThemeColor"].ToUpper()))
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                        throw new Exception("Current theme is " + getTheme.ToUpper() + " and excel value is " + columnValues["VerifyThemeColor"].ToUpper());
                    }
                    else
                    {
                        Console.WriteLine(getTheme.ToUpper() + " theme verified");
                    }

                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("OptionalWindowClose"))
                {
                    string value = TestCaseSheet.Tables[0].Rows[0][table.Rows[i]["Action"].ToString()].ToString();
                    string[] parentname = table.Rows[0]["Action"].ToString().Split(',');
                    if (ConfigurationManager.AppSettings["OpenSpecifiedWindow"].ToString().Split(',').Contains(StepName))
                    {
                        parentname = TestCaseSheet_Temp.Tables[0].Rows[0]["OpenWindow"].ToString().Split(',');
                    }
                    if (!value.ToUpper().Equals("TRUE"))
                    {
                        CloseWindow(driver, parentname[0], dict["CloseModule"].ToString());
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("UseOrVerifyHotKeys"))
                {
                    IWebElement element = driver.FindElement(By.ClassName(dict["BookmarkOfTT"].ToString()));
                    string classIsActive = element.GetAttribute("class");
                    if (classIsActive.Contains("inactive"))
                    {
                        element.Click();
                    }
                    Wait(2000);
                    element = driver.FindElement(By.ClassName(dict["hotKeyStrip"].ToString()));
                    IList<IWebElement> divElements = element.FindElements(By.TagName("div"));
                    if (!string.IsNullOrEmpty(columnValues["UseHotKey"]))
                    {
                        foreach (IWebElement ele in divElements)
                        {
                            if (ele.Text.ToUpper().Equals(columnValues["UseHotKey"].ToUpper()))
                            {
                                ele.Click();
                                Console.WriteLine(columnValues["UseHotKey"] + " key clicked");
                                break;
                            }
                        }
                        Wait(2000);
                    }


                    if (!string.IsNullOrEmpty(columnValues["VerifyHotKeys"]))
                    {
                        foreach (string val in columnValues["VerifyHotKeys"].Split(','))
                        {
                            bool flag = false;
                            foreach (IWebElement ele in divElements)
                            {
                                if (ele.Text.ToUpper().Equals(val.ToUpper()))
                                {
                                    Console.WriteLine(val + " key found");
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                            {
                                if (!string.IsNullOrEmpty(columnValues["NegativeTesting"]))
                                {
                                    Console.WriteLine(val + " key not found, Negative Testing succeeded");
                                }
                                else
                                {
                                    CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                                    throw new Exception(val + " not key found");
                                }
                            }

                        }
                    }

                    if (!string.IsNullOrEmpty(columnValues["VerifyHotKeysColor"]))
                    {
                        Dictionary<string, string> hotKeyDict = new Dictionary<string, string>();
                        Dictionary<string, string> UserKeyDict = new Dictionary<string, string>();
                        string[] keys = columnValues["VerifyHotKeys"].Split(',');
                        string[] values = columnValues["VerifyHotKeysColor"].Split(',');

                        for (int j = 0; j < keys.Length; j++)
                        {
                            UserKeyDict[keys[j]] = values[j];
                        }

                        foreach (IWebElement ele in divElements)
                        {
                            hotKeyDict.Add(ele.Text, ele.GetAttribute("class"));
                        }

                        foreach (string key in UserKeyDict.Keys)
                        {
                            if (hotKeyDict.ContainsKey(key))
                            {
                                if (hotKeyDict[key].Contains(UserKeyDict[key]))
                                {
                                    Console.WriteLine(key + " has " + UserKeyDict[key] + " colour");
                                }
                                else
                                {
                                    CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " HotKey Error");
                                    throw new Exception("UI has " + hotKeyDict[key] + " color but Excel has " + UserKeyDict[key] + " for Key: " + key);
                                }
                            }
                        }
                    }

                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("Resizewindow"))
                {
                    string action = columnValues["ResizeAction"].ToString().ToUpper();
                    string ensureResize = columnValues["EnsureResize"].ToString().ToUpper();

                    string parentWindowTitle = "Nirvana";


                    //WebDriver driver1 = new ChromeDriver();


                    string DashBoardXpath = dict["PMDashboard"].ToString(); ;

                    string ButtonXpath = dict2["MaximizeWindow"].ToString();
                    string ButtonXpath1 = dict["MaximizeWindow"].ToString();

                    string originalWindow = driver.CurrentWindowHandle;
                    IList<string> windowHandles = driver.WindowHandles;
                    foreach (string handle in windowHandles)
                    {
                        driver.SwitchTo().Window(handle);
                        if (driver.Title.Contains(parentWindowTitle))
                        {
                            Console.WriteLine("Switched to window with title: " + driver.Title);

                            int retries = 0;
                            while (retries < 2)
                            {
                                try
                                {
                                    var elements = driver.FindElements(By.XPath(DashBoardXpath));
                                    if (elements.Count > 0)
                                    {
                                        Console.WriteLine("Element found in the window.");


                                        PerformUserAction(driver, action, ButtonXpath, ButtonXpath1);


                                        EnsureWindowState(driver, ensureResize);

                                        break;
                                    }
                                    else
                                    {
                                        retries++;
                                        Thread.Sleep(500);
                                    }
                                }
                                catch (WebDriverTimeoutException)
                                {
                                    Console.WriteLine("Element not found. Retrying...");
                                    retries++;
                                    Thread.Sleep(500);
                                }
                                catch (WebDriverException e)
                                {
                                    Console.WriteLine("WebDriverException: " + e.Message + ". Retrying...");
                                    retries++;
                                    Thread.Sleep(500);
                                }
                            }
                        }
                    }

                    // Switch back to the original window if needed
                    driver.SwitchTo().Window(originalWindow);

                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("ResizewindowBlotter"))
                {
                    string action = columnValues["ResizeAction"].ToString().ToUpper();
                    string ensureResize = columnValues["EnsureResize"].ToString().ToUpper();
                    string parentWindowTitle = "Blotter";
                    string DashBoardXpath = DashBoardXpath = dict["BlotterDashboard"].ToString();
                    string ButtonXpath = dict["RestoreWindow"].ToString();
                    string ButtonXpath1 = dict["MaximizeWindow"].ToString();

                    string originalWindow = driver.CurrentWindowHandle;
                    IList<string> windowHandles = driver.WindowHandles;

                    SwitchWindow.SwitchToParentWindow(driver, parentWindowTitle, dict["CloseModule"].ToString());

                    if (driver.Title.Contains(parentWindowTitle))
                    {
                        Console.WriteLine("Switched to window with title: " + driver.Title);

                        int retries = 0;
                        while (retries < 2)
                        {
                            try
                            {
                                var elements = driver.FindElements(By.XPath(DashBoardXpath));

                                if (elements.Count > 0)
                                {
                                    Console.WriteLine("Element found in the window.");


                                    PerformUserAction(driver, action, ButtonXpath, ButtonXpath1);


                                    EnsureWindowState(driver, ensureResize);

                                    break;
                                }
                                else
                                {
                                    retries++;
                                    Thread.Sleep(500);
                                }
                            }
                            catch (WebDriverTimeoutException)
                            {
                                Console.WriteLine("Element not found. Retrying...");
                                retries++;
                                Thread.Sleep(500);
                            }
                            catch (WebDriverException e)
                            {
                                Console.WriteLine("WebDriverException: " + e.Message + ". Retrying...");
                                retries++;
                                Thread.Sleep(500);
                            }
                        }
                    }

                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("ResizewindowTT"))
                {
                    string action = columnValues["ResizeAction"].ToString().ToUpper();
                    string ensureResize = columnValues["EnsureResize"].ToString().ToUpper();
                    string parentWindowTitle = "Nirvana";
                    string DashBoardXpath = DashBoardXpath = dict["TTDashboard"].ToString();
                    string ButtonXpath = dict2["MaximizeWindow"].ToString();
                    string ButtonXpath1 = dict["MaximizeWindow"].ToString();
                    string originalWindow = driver.CurrentWindowHandle;
                    IList<string> windowHandles = driver.WindowHandles;
                    SwitchWindow.SwitchToWindow(driver, parentWindowTitle, true);
                    if (driver.Title.Contains(parentWindowTitle))
                    {
                        Console.WriteLine("Switched to window with title: " + driver.Title);
                        int retries = 0;
                        while (retries < 2)
                        {
                            try
                            {
                                var elements = driver.FindElements(By.XPath(DashBoardXpath));
                                if (elements.Count > 0)
                                {
                                    Console.WriteLine("Element found in the window.");
                                    PerformUserAction(driver, action, ButtonXpath, ButtonXpath1);
                                    EnsureWindowState(driver, ensureResize);
                                    break;
                                }
                                else
                                {
                                    retries++;
                                    Thread.Sleep(500);
                                }
                            }
                            catch (WebDriverTimeoutException)
                            {
                                Console.WriteLine("Element not found. Retrying...");
                                retries++;
                                Thread.Sleep(500);
                            }
                            catch (WebDriverException e)
                            {
                                Console.WriteLine("WebDriverException: " + e.Message + ". Retrying...");
                                retries++;
                                Thread.Sleep(500);
                            }
                        }
                    }
                }
                else if (dataTable.Rows[i]["Steps"].ToString().Equals("LogOutSamsara"))
                {
                    if (columnValues["VerifyTextOnLogoutDock"].ToString().Equals("True"))
                    {
                        IWebElement logOutMessage = driver.FindElement(By.Id(dict["logoutPopupMessage"].ToString()));
                        if (logOutMessage.Text.Contains("Logging off will close any workspace or any processes that are running. Do you wish to save the workspace?"))
                        {
                            Console.WriteLine("Logout Message Verified");
                        }

                    }

                    IWebElement parentDiv = driver.FindElement(By.Id(dict["LogOutButtonsId"].ToString()));
                    IList<IWebElement> divElements = parentDiv.FindElements(By.TagName("button"));
                    foreach (IWebElement ele in divElements)
                    {
                        if (ele.Text.Equals(columnValues["Action"].ToString()))
                        {
                            ele.Click();
                            break;
                        }
                    }

                    Wait(10000);

                    if (!string.IsNullOrEmpty(columnValues["VerifyDock"].ToString()))
                    {
                        if (!SwitchWindow.SwitchToWindow(driver, "Dock").ToString().ToUpper().Equals(columnValues["VerifyDock"].ToString().ToUpper()))
                        {
                            CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                            throw new Exception("Window Availablity is " + columnValues["VerifyDock"].ToString());
                        }
                    }
                }
                else
                {

                    try
                    {
                        PerformActionsVerification(driver, dataTable.Rows[i]["Action"].ToString(), dict[table.Rows[i]["Steps"].ToString()].ToString(), table.Rows[i]["Steps"].ToString());
                    }
                    catch
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                    }
                }
                if (ConfigurationManager.AppSettings["ToastVerify"].ToString() == "true")
                {
                    try
                    {
                        IWebElement toast = driver.FindElement(By.ClassName("Toastify"));
                        if (!string.IsNullOrEmpty(toast.Text.ToString()))
                        {
                            toastList.Add(toast.Text.ToString());
                        }
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (!String.IsNullOrEmpty(dataTable.Rows[i]["Time(In Seconds)"].ToString()))
                {
                    Wait(Int32.Parse(dataTable.Rows[i]["Time(In Seconds)"].ToString()) * 1000);
                }
            }
            return Data;
        }

        private static void PerformActionByData(DataTable dataTable, WebDriver driver, int i, string xpath,string action,string data="")
        {
            if (action == "Click")
            {
                driver.FindElement(By.XPath(xpath)).Click();
            }
            else
            {
                clearField(driver, xpath);
                driver.FindElement(By.XPath(xpath)).SendKeys(Keys.Control + Keys.Home);
                driver.FindElement(By.XPath(xpath)).SendKeys(Keys.Control + Keys.Shift + Keys.End);
                
                driver.FindElement(By.XPath(xpath)).SendKeys(data);
                driver.FindElement(By.XPath(xpath)).SendKeys(Keys.Tab);
            }
            Thread.Sleep(3000);
        }

        
        private static string getThemeOfModule(WebDriver driver, string moduleName, string KeyName)
        {
            string theme = "dark";
            SwitchWindow.SwitchToWindow(driver, moduleName);
            
            IWebElement element = driver.FindElement(By.Id(KeyName));
            string className = element.GetAttribute("class");

            if (className.ToString().Contains("dark"))
            {
                theme = "dark";
            }
            else if (className.ToString().Contains("light"))
            {
                theme = "light";
            }
            
            return theme;
        }

        private static void verticalScroll(IWebElement ScrollBarElement, Actions actions, IJavaScriptExecutor js, WebDriver driver, int scrollIndex)
        {
            try
            {
                IWebElement elm = ScrollBarElement;
                actions.MoveToElement(elm).Perform();


                actions
                    .ClickAndHold(elm)
                    .MoveByOffset(0, scrollIndex)
                    .Release()
                    .Perform();

                Thread.Sleep(2000);
            }
            catch (MoveTargetOutOfBoundsException ex) { }
        }


        private static string rgbcolor(string colour)
        {
            if (colour.Contains(")"))
            {
                string[] arr1 = colour.Split(')');
                colour = arr1[0];
            }
            colour = colour.Replace(" ", "").Replace(")", "").Replace(";", "");
            string[] arr = colour.Split(',');
            int[] intArray = arr.Select(int.Parse).ToArray();
            int max = intArray.Max();
            int index = Array.IndexOf(intArray, max);
            if (index == 0)
            {
                return "red";
            }
            else if (index == 1)
            {
                return "green";
            }
            else if (index == 2)
            {
                return "blue";
            }
            return "not known";

        }

        private static DataTable removeSuborderRows(DataTable Data)
        {

            for (int i = 0; i < Data.Rows.Count; i++)
            {
                if (Data.Rows[i][0].ToString() == "")
                {
                    Data.Rows[i].Delete();
                    i = i - 1;
                }
            }
            Data.AcceptChanges();
            return Data;
        }


        private static void RenameColumnName(DataTable dt, string prevColName, string curColName) {
            dt.Columns[prevColName].ColumnName = curColName;
        }


        private static void PerformActionsVerification(WebDriver driver, string Action, string Xpath, string p3)
        {
            Thread.Sleep(2000);
            var temp_dict = GetDict(SamsaraMappingTables[temp_ModuleName], 2);
            if (Action == "Click")
            {
                try { driver.FindElement(By.XPath(Xpath)).Click(); }
                catch
                {
                    try
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        string script = "document.querySelector(" + Xpath + ").click();";
                        js.ExecuteScript(script);
                    }
                    catch
                    {
                        try { driver.FindElement(By.XPath(temp_dict[p3].ToString())).Click(); }
                        catch {
                            try
                            {
                                driver.FindElement(By.XPath(dict2[p3].ToString())).Click();
                            }
                            catch { }
                        }
                    }
                }
            }
            if (Action == "RightClick")
            {
                try
                {
                    //string id = DefaultCellValue + RowIndex + "c1";
                    string xpath = string.Format(Xpath, RowIndex, 2);
                    Actions actions = new Actions(driver);
                    IWebElement element = null;
                    element = driver.FindElement(By.XPath(xpath));
                   
                    actions.ContextClick(element).Perform();
                }
                catch { }
            }
            if (Action == "Select")
            {
                try
                {
                    string id = DefaultCellValue + RowIndex + "c0";
                    string xpath = string.Format(Xpath, RowIndex, 1);
                    Actions actions = new Actions(driver);
                    IWebElement element = driver.FindElement(By.XPath(xpath));

                    actions.Click(element).Perform();
                    DefaultCellValue = id;
                }
                catch { }
            }
        }
        private static TKey GetRandomKey<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            Random randInt = new Random();
            int index = randInt.Next(dictionary.Count); // Generate a random index
            TKey[] keys = new TKey[dictionary.Count];
            dictionary.Keys.CopyTo(keys, 0); // Copy keys to an array

            return keys[index]; // Return a random key
        }
        private static DataTable GetData(String ColumnXpath, string RowXpath, Actions actions, IJavaScriptExecutor js, WebDriver driver)
        {
            var colIdToHeader = new Dictionary<string, string>();
            var orderedColIds = new List<string>(); // To maintain column order
            var dataTable = new DataTable();
            IWebElement gridRoot = driver.FindElement(By.Id("SubOrderList"));
            // Find all header cells with a col-id
            var headerCells = gridRoot.FindElements(By.CssSelector(".ag-header-cell[col-id]"));

            foreach (var headerCell in headerCells)
            {
                string colId = headerCell.GetAttribute("col-id");
                string headerText = headerCell.FindElement(By.CssSelector(".ag-header-cell-text")).Text.Trim();

                // Add to map and DataTable if it has a valid ID and text
                if (!string.IsNullOrEmpty(colId) && !string.IsNullOrEmpty(headerText) && !colIdToHeader.ContainsKey(colId))
                {
                    colIdToHeader[colId] = headerText;
                    orderedColIds.Add(colId);
                    dataTable.Columns.Add(headerText); // Add column to the DataTable
                }
            }

            Console.WriteLine("--- Headers Found ---");
            foreach (var colId in orderedColIds)
            {
                // Replaced string interpolation with string.Format
                Console.WriteLine(string.Format("ID: {0}, Header: {1}", colId, colIdToHeader[colId]));
            }
            Console.WriteLine("---------------------\n");


            // 4. --- Get All Rows and Extract Data ---
            // Find all row elements in the grid's body container
            var rows = gridRoot.FindElements(By.CssSelector(".ag-center-cols-container div[role='row']"));

            foreach (var row in rows)
            {
                DataRow newRow = dataTable.NewRow(); // Create a new row for our table

                // Find all cells in this row and map them by their col-id
                var cellsInRow = new Dictionary<string, string>();
                var cells = row.FindElements(By.CssSelector("div[role='gridcell'][col-id]"));
                foreach (var cell in cells)
                {
                    string colId = cell.GetAttribute("col-id");
                    if (colIdToHeader.ContainsKey(colId))
                    {
                        cellsInRow[colId] = cell.Text;
                    }
                }

                // Declare the out variable *before* the TryGetValue call
                string cellValue;

                // Populate the DataRow in the correct column order
                foreach (string colId in orderedColIds)
                {
                    string headerName = colIdToHeader[colId];

                    // Use the pre-declared cellValue variable
                    if (cellsInRow.TryGetValue(colId, out cellValue))
                    {
                        newRow[headerName] = cellValue;
                    }
                    else
                    {
                        // Handle missing cells if necessary (e.g., cell spans)
                        newRow[headerName] = DBNull.Value;
                    }
                }

                dataTable.Rows.Add(newRow); // Add the populated row to the DataTable
            }
            return dataTable;
        }


        public static DataRow[] GetMatchingDataRows(DataTable masterDataTable1, DataRow rowToMatch, List<string> Columns, bool dateTimeFlag = false)
        {
            List<DataRow> matchedRows = new List<DataRow>();
            List<DataRow> matchedRows1 = new List<DataRow>();
            List<DataRow> matchedRows2 = new List<DataRow>();
            DataTable masterDataTable = DataUtilities.RemoveTrailingZeroes(masterDataTable1);
            try
            {
                string expression = string.Empty;
                string dateExpression = string.Empty;
                DateTime dateValue = new DateTime();
                HashSet<string> dateColumn = new HashSet<string>();
                List<string> DataColumns = new List<string>();
                foreach (var columns in masterDataTable1.Columns)
                {
                    DataColumns.Add(columns.ToString());
                }
                for (int i = 0; i < rowToMatch.Table.Columns.Count; i++)
                {
                    string colValue = rowToMatch.ItemArray[i].ToString();
                    if (DataColumns.Contains(rowToMatch.Table.Columns[i].ToString()))
                    {
                        if (!string.IsNullOrWhiteSpace(colValue))
                        {
                            expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                            expression = expression + "[" + rowToMatch.Table.Columns[i].Caption + "] = '" + colValue.Trim() + "' ";
                        }
                    }
                    if (!Columns.Contains(rowToMatch.Table.Columns[i].ToString()))
                        continue;


                    if (masterDataTable.Select(expression).Length == 1)
                    {
                        matchedRows = masterDataTable.Select(expression).ToList();
                        return matchedRows.ToArray();
                    }

                }
                if (rowToMatch.Table.Columns.Contains("Target Qty") && !String.IsNullOrEmpty(rowToMatch.Table.Rows[0]["Target Qty"].ToString()))
                {
                    expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                    expression = expression + "[Quantity] = '" + rowToMatch.Table.Rows[0]["Target Qty"].ToString().Trim() + "' ";
                }
                matchedRows = masterDataTable.Select(expression).ToList();

                // Convert each value of each column in dateColumns to Date in specified Format by parsing each value to datetime Object

                //Checks if dateTime Flag is true and dateColumns is not empt/y
                if (dateColumn.Count > 0)
                {
                    DataTable subsetTable = matchedRows.CopyToDataTable();
                    foreach (DataRow dr in subsetTable.Rows)
                    {
                        foreach (string column in dateColumn)
                        {
                            DateTime.TryParse(dr[column].ToString(), out dateValue);
                            dr[column] = dateValue.Date.ToString("MM/dd/yyyy");
                        }
                    }
                    matchedRows1 = subsetTable.Select(dateExpression).ToList();// matchedRows1 contains single row that is matched
                    foreach (DataRow dr in matchedRows1)
                    {
                        matchedRows2.Add(matchedRows[dr.Table.Rows.IndexOf(dr)]);

                    }
                    return matchedRows2.ToArray();

                }
                return matchedRows.ToArray();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;

            }
            return null;

        }

        private static List<string> verifyData(DataTable ExcelData, DataTable UIData, string[] CompulsoryColumns)
        {

            List<String> errors = new List<String>();
            try
            {
                //if (ExcelData.Rows.Count == 0 || UIData.Rows.Count == 0 || ExcelData.Rows.Count>UIData.Rows.Count)
                //    return error;
                //else
                //    error = 0;
                List<string> DataColumns = new List<string>();
                foreach (var columns in UIData.Columns)
                {
                    DataColumns.Add(columns.ToString());
                }

                foreach (DataRow row in ExcelData.Rows)
                {
                    string expression = "";
                    foreach (string colName in CompulsoryColumns)
                    {
                        string colValue = row[colName].ToString().Trim();
                        if (colValue.Equals(ExcelStructureConstants.BLANK_CONST))
                            colValue = string.Empty;
                        expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                        expression = expression + "[" + colName + "] = '" + colValue + "' ";
                    }
                    foreach (var column in ExcelData.Columns)
                    {
                        if (DataColumns.Contains(column.ToString()))
                        {
                            string colValue = row[column.ToString()].ToString().Trim();
                            if (colValue.Equals(ExcelStructureConstants.BLANK_CONST))
                                colValue = string.Empty;
                            if (colValue != string.Empty)
                            {
                                expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                                expression = expression + "[" + column + "] = '" + colValue + "' ";
                            }
                        }

                    }
                    DataRow[] matchedRows = UIData.Select(expression);
                    if (matchedRows.Length <= 0)
                    {
                        string expression2 = string.Empty;
                        expression2 = GenerateError(row, UIData, ExcelData, DataColumns);
                        errors.Add("Values did not match for " + expression2);
                        continue;
                    }
                    if (errors.Count > 0)
                        return errors;
                }


            }
            catch { CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun); }
            return errors;
        }

        private static string GenerateError(DataRow row, DataTable UIData, DataTable ExcelData, List<string> DataColumns)
        {
            string Expression = string.Empty;

            foreach (DataRow rows in UIData.Rows)
            {
                foreach (var column in ExcelData.Columns)
                {

                    if (DataColumns.Contains(column))
                    {
                        string Excel = row[column.ToString()].ToString();
                        string UI = rows[column.ToString()].ToString();
                        string colName = column.ToString();
                        Expression = string.IsNullOrWhiteSpace(Expression) ? Expression : Expression + " , \n";
                        Expression = Expression + "[" + colName + "] = UI{" + UI + "}+Excel{" + Excel + "}";
                    }

                }
            }
            return Expression;
        }


        public static DataRow GetMatchingDataRow(DataTable masterDataTable, DataRow rowToMatch, List<string> Columns, bool checkFlag = false)
        {
            try
            {
                return GetMatchingDataRows(masterDataTable, rowToMatch, Columns, checkFlag).FirstOrDefault();
            }
            catch (Exception ex)
            {

                return null;

            }

        }


        private static IEnumerable<HtmlNode> ExtractDivNode(string GridData)
        {
            List<HtmlNode> divElements = new List<HtmlNode>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(GridData);

            foreach (HtmlNode node in doc.DocumentNode.Descendants("div"))
            {
                if (node.HasAttributes && node.Attributes["id"] != null)
                    divElements.Add(node);
            }

            return divElements;
        }

        private static TestResult InputDetails(DataRow row, string StepName, Dictionary<string, object> dict, WebDriver driver, string moduleName, DataSet TestCaseSheet)
        {
            DataTable ExcelData = null;
            TestResult _Res = new TestResult();
            DataTable UIData = null;
            try
            {

                if (DataTables[StepName].Rows[0]["StepType"].ToString().ToLower() == "select")
                {
                    UIData = FetchAndVerifyDataFromGrid(DataTables[StepName], dict, driver, StepName, TestCaseSheet, ref _Res);
                    int rowCount = 0;

                    foreach (DataRow row1 in UIData.Rows)
                    {
                        // Check if any column in the row has a non-null or non-empty value
                        if (row1.ItemArray.Any(field => field != DBNull.Value && !string.IsNullOrEmpty(field.ToString())))
                        {
                            rowCount++;
                        }
                    }

                    if (rowCount == 1)
                    {
                        string filepath = @"\\192.168.2.108\DistributedAutomation_Master\Column_Mapping.xlsx";
                        string sheetName = moduleName;
                        var columnMapping = CreateColumnMapping(filepath, sheetName);
                        string steptype = DataTables[StepName].Rows[0]["StepType"].ToString();
                        SelectTrade(TestCaseSheet, StepName, driver, UIData);
                    }
                }
            }
            catch { }
            var table = DataTables[StepName];
            Dictionary<string, string> columnValues = new Dictionary<string, string>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(table.Rows[i]["Steps"].ToString()) && string.Equals(table.Rows[i]["Steps"].ToString(), "ExtractTabOutsideAndVerify", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        DataTable dtable = TestCaseSheet.Tables[StepName];
                        if (dtable == null && TestCaseSheet.Tables.Count > 0)
                        {
                            dtable = TestCaseSheet.Tables[0];
                        }
                        try
                        {
                            SamsaraGridOperationHelper.ClickElement(driver, "ExtractTabOutsideAndVerify", ref dict, null, dtable, table.Rows[i]["Action"].ToString().Split(',').ToList());
                            //
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                }
                    // to book a unallocated trade with the provided symbol (to handle the AG Grid (No Rows to Show))
                else if (table.Rows[i]["Steps"].ToString().Equals("TradeData"))
                {
                    try
                    {
                        OpenModule("TradingTicket", driver);
                        var TTDict = GetDict(SamsaraMappingTables["TradingTicket"]);
                        SwitchWindow.SwitchToWindow(driver, "Trading Ticket");

                        string xpath = TTDict["Symbol"].ToString();
                        PerformActionByData(table, driver, i, xpath, "SendKeys", table.Rows[i]["Action"].ToString());
                        Thread.Sleep(10000);

                        xpath = TTDict["Quantity"].ToString();
                        PerformActionByData(table, driver, i, xpath, "SendKeys", "50");

                        xpath = TTDict["AllocationMethod"].ToString();
                        PerformActionByData(table, driver, i, xpath, "SendKeys", "Unallocated");

                        xpath = TTDict["Price"].ToString();
                        PerformActionByData(table, driver, i, xpath, "SendKeys", "50");

                        xpath = TTDict["Button_DoneAway"].ToString();
                        PerformActionByData(table, driver, i, xpath, "Click");

                        xpath = TTDict["Buy"].ToString();
                        PerformActionByData(table, driver, i, xpath, "Click");

                        xpath = TTDict["ContinueButton_NewSub"].ToString();
                        try
                        {
                            PerformActionByData(table, driver, i, xpath, "Click");
                        }
                        catch { }
                        SwitchWindow.SwitchToWindow(driver, "Pre-Trade Compliance Results");
                        xpath = TTDict["AllowTrade_Yes"].ToString();
                        PerformActionByData(table, driver, i, xpath, "Click");

                        Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in Trade Data action");
                    }

                }
                else if (table.Columns.Contains("ActionThroughClass") && table.Rows[i]["ActionThroughClass"].ToString().ToUpper().Equals("TRUE"))
                {
                    try
                    {
                        PerformActionThroughClass(driver, table.Rows[i]["Action"].ToString(), dict[table.Rows[i]["Steps"].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                    }
                    catch(Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                }

                else if (table.Columns.Contains("ActionThroughId") && table.Rows[i]["ActionThroughId"].ToString().ToUpper().Equals("TRUE"))
                {
                    try
                    {
                        PerformActionsThroughID(driver, table.Rows[i]["Action"].ToString(), dict[table.Rows[i]["Steps"].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("TradeBy"))
                {
                    if (TestCaseSheet.Tables[0].Columns.Contains("TradeBy") && !string.IsNullOrEmpty(row["TradeBy"].ToString())) {
                        PerformActionsThroughID(driver, "SendKeys", dict["TradeBy"].ToString(), row, "TradeBy");
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("TradeAttributeSet") && TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 01"))
                {
                    IList<IWebElement> divElements = null;
                    try
                    {
                        PerformActionThroughClass(driver, "Click", dict["TradeAttributeExpand"].ToString(), row, "TradeAttributeExpand");
                        Wait(3000);
                        IWebElement element = driver.FindElement(By.ClassName(dict["TradeAttributeClass"].ToString()));
                        divElements = element.FindElements(By.Id(dict["AttributesID"].ToString()));
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Actions action = new Actions(driver);
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 01") && !string.IsNullOrEmpty(row["Trade Attribute 01"].ToString())) 
                    {
                        action.Click(divElements[0])
                                    .SendKeys(row["Trade Attribute 01"].ToString()).SendKeys(Keys.Tab)
                                    .Build()
                                    .Perform();
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 02") && !string.IsNullOrEmpty(row["Trade Attribute 02"].ToString()))
                    {
                        action.Click(divElements[1])
                                    .SendKeys(row["Trade Attribute 02"].ToString()).SendKeys(Keys.Tab)
                                    .Build()
                                    .Perform();
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 03") && !string.IsNullOrEmpty(row["Trade Attribute 03"].ToString()))
                    {
                        action.Click(divElements[2])
                                    .SendKeys(row["Trade Attribute 03"].ToString()).SendKeys(Keys.Tab)
                                    .Build()
                                    .Perform();
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 04") && !string.IsNullOrEmpty(row["Trade Attribute 04"].ToString()))
                    {
                        action.Click(divElements[3])
                                    .SendKeys(row["Trade Attribute 04"].ToString()).SendKeys(Keys.Tab)
                                    .Build()
                                    .Perform();
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 05") && !string.IsNullOrEmpty(row["Trade Attribute 05"].ToString()))
                    {
                        action.Click(divElements[4])
                                    .SendKeys(row["Trade Attribute 05"].ToString()).SendKeys(Keys.Tab)
                                    .Build()
                                    .Perform();
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 06") && !string.IsNullOrEmpty(row["Trade Attribute 06"].ToString()))
                    {
                        action.Click(divElements[5])
                                    .SendKeys(row["Trade Attribute 06"].ToString()).SendKeys(Keys.Tab)
                                    .Build()
                                    .Perform();
                    }
                    //
                    IWebElement suggest = null;
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 01 suggestion") && !string.IsNullOrEmpty(row["Trade Attribute 01 suggestion"].ToString()))
                    {
                        action.Click(divElements[0])
                                    .Build()
                                    .Perform();
                        Wait(1500);
                        suggest = driver.FindElement(By.ClassName(dict["suggestTradeAttribute"].ToString()));
                        string uiText = suggest.Text.ToString().Trim();
                        uiText = uiText.Replace("\r", "");
                        if (uiText.ToString().ToLower().Equals(row["Trade Attribute 01 suggestion"].ToString().ToLower()))
                        {
                            Console.WriteLine(suggest.Text + " Trade attribute suggestion verified");
                            suggest.Click();
                        }
                        else 
                        {
                            throw new Exception (suggest.Text + " Trade attribute suggestion not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 02 suggestion") && !string.IsNullOrEmpty(row["Trade Attribute 02 suggestion"].ToString()))
                    {
                        action.Click(divElements[1])
                                    .Build()
                                    .Perform();
                        Wait(1500);
                        suggest = driver.FindElement(By.ClassName(dict["suggestTradeAttribute"].ToString()));
                        string uiText = suggest.Text.ToString().Trim();
                        uiText = uiText.Replace("\r", "");
                        
                        if (uiText.ToString().ToLower().Equals(row["Trade Attribute 02 suggestion"].ToString().ToLower()))
                        {
                            Console.WriteLine(suggest.Text + " Trade attribute suggestion verified");
                            suggest.Click();
                        }
                        else
                        {
                            throw new Exception(suggest.Text + " Trade attribute suggestion not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 03 suggestion") && !string.IsNullOrEmpty(row["Trade Attribute 03 suggestion"].ToString()))
                    {
                        action.Click(divElements[2])
                                    .Build()
                                    .Perform();
                        Wait(1500);

                        suggest = driver.FindElement(By.ClassName(dict["suggestTradeAttribute"].ToString()));
                        string uiText = suggest.Text.ToString().Trim();
                        uiText = uiText.Replace("\r", "");
                        if (uiText.ToString().ToLower().Equals(row["Trade Attribute 03 suggestion"].ToString().ToLower()))
                        {
                            Console.WriteLine(suggest.Text + " Trade attribute suggestion verified");
                            suggest.Click();
                        }
                        else
                        {
                            throw new Exception(suggest.Text + " Trade attribute suggestion not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 04 suggestion") && !string.IsNullOrEmpty(row["Trade Attribute 04 suggestion"].ToString()))
                    {
                        action.Click(divElements[3])
                                    .Build()
                                    .Perform();
                        suggest = driver.FindElement(By.ClassName(dict["suggestTradeAttribute"].ToString()));
                        string uiText = suggest.Text.ToString().Trim();
                        uiText = uiText.Replace("\r", "");
                        if (uiText.ToString().ToLower().Equals(row["Trade Attribute 04 suggestion"].ToString().ToLower()))
                        {
                            Console.WriteLine(suggest.Text + " Trade attribute suggestion verified");
                            suggest.Click();
                        }
                        else
                        {
                            throw new Exception(suggest.Text + " Trade attribute suggestion not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 05 suggestion") && !string.IsNullOrEmpty(row["Trade Attribute 05 suggestion"].ToString()))
                    {
                        action.Click(divElements[4])
                                    .Build()
                                    .Perform();
                        Wait(1500);
                        suggest = driver.FindElement(By.ClassName(dict["suggestTradeAttribute"].ToString()));
                        string uiText = suggest.Text.ToString().Trim();
                        uiText = uiText.Replace("\r", "");
                        if (uiText.ToString().ToLower().Equals(row["Trade Attribute 05 suggestion"].ToString().ToLower()))
                        {
                            Console.WriteLine(suggest.Text + " Trade attribute suggestion verified");
                            suggest.Click();
                        }
                        else
                        {
                            throw new Exception(suggest.Text + " Trade attribute suggestion not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Trade Attribute 06 suggestion") && !string.IsNullOrEmpty(row["Trade Attribute 06 suggestion"].ToString()))
                    {
                        action.Click(divElements[5])
                                    .Build()
                                    .Perform();
                        Wait(1500);
                        suggest = driver.FindElement(By.ClassName(dict["suggestTradeAttribute"].ToString()));
                        string uiText = suggest.Text.ToString().Trim();
                        uiText = uiText.Replace("\r", "");
                        if (uiText.ToString().ToLower().Equals(row["Trade Attribute 06 suggestion"].ToString().ToLower()))
                        {
                            Console.WriteLine(suggest.Text + " Trade attribute suggestion verified");
                            suggest.Click();
                        }
                        else
                        {
                            throw new Exception(suggest.Text + " Trade attribute suggestion not verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Strategy") && !string.IsNullOrEmpty(row["Strategy"].ToString()))
                    {
                        //strategyID
                        suggest = driver.FindElement(By.Id(dict["strategyID"].ToString()));
                        if (!row["Strategy"].ToString().ToLower().Equals("clear"))
                        {
                            action.Click(suggest)
                            .KeyDown(Keys.Control)
                            .SendKeys("a")
                            .KeyUp(Keys.Control).SendKeys(Keys.Backspace)
                            .SendKeys(row["Strategy"].ToString()).SendKeys(Keys.Tab)
                            .Build()
                            .Perform();
                        }
                        else {
                            action.Click(suggest).MoveToElement(suggest)
                            .KeyDown(Keys.Control)
                            .SendKeys("a")
                            .KeyUp(Keys.Control).SendKeys(Keys.Backspace).Build()
                            .Perform();
                        }
                        Wait(2000);

                    }
                    action = null;
                    PerformActionThroughClass(driver, "Click", dict["TradeAttributeExpand"].ToString(), row, "TradeAttributeExpand");                    
                    
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("TradeSummaryPopup"))
                {
                    try
                    {
                        foreach (DataRow dr in TestCaseSheet_Temp.Tables[0].Rows)
                        {
                            if (columnValues.Keys.Contains("TT_Open") && !columnValues["TT_Open"].ToString().ToUpper().Equals("TRUE"))
                            {
                                PerformActions(driver, "Click", dict["AllowTrade_Yes"].ToString(), dr, "AllowTrade_Yes");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                else if (table.Rows[i]["Steps"].ToString().Equals("EnterSuggestion"))
                {
                    IWebElement element = driver.FindElement(By.XPath(dict["Symbol"].ToString()));
                    Actions actions = new Actions(driver);
                    actions.DoubleClick(element)
                                       .SendKeys(Keys.ArrowUp).SendKeys(Keys.Enter)
                                       .Build()
                                       .Perform();
                    Wait(5000);
                    string symbolValue = element.GetAttribute("value").ToString();
                    if (row["SymbolToVerify"].ToString().ToLower().Equals(symbolValue.ToLower()))
                    {
                        Console.WriteLine(row["SymbolToVerify"].ToString() + " suggestion verified");
                    }
                    else
                    {
                        throw new Exception(row["SymbolToVerify"].ToString() + " suggestion not verified");
                    }


                }
                else if (table.Rows[i]["Steps"].ToString().Equals("ACCenvironment"))
                {
                    if (ConfigurationManager.AppSettings["ACCenvironment"].ToString().Equals("true"))
                    {
                        if (StepName.Equals("CustomOrderTT"))
                        {
                            return _Res;
                        }

                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("ManageHotButton"))
                {
                    try
                    {
                        driver.FindElement(By.ClassName(dict["hotButtonPanelClass"].ToString()));
                    }
                    catch
                    {
                        PerformActions(driver, "Click", dict["hotButtonPanel"].ToString(), row, "");
                    }
                    Wait(3000);
                    PerformActionsThroughID(driver, "Click", dict["ManageHotButton"].ToString(), row, "ManageHotButton");
                    Wait(3000);
                    SwitchWindow.SwitchToWindow(driver, "Nirvāna ONE");
                    IReadOnlyCollection<IWebElement> grids = driver.FindElements(By.ClassName(dict["ClassOfManageHotButton"].ToString()));
                    foreach (string name in columnValues["VerifyHotButtonNames"].ToString().Split(','))
                    {
                        bool flag = false;
                        foreach (var ele in grids)
                        {
                            if (ele.Text.ToString().ToLower().Equals(name.ToLower()))
                            {
                                flag = true;
                                break;
                            }
                        }
                        if(!flag)
                            throw new Exception("HotButton with name " + name + " doesnot found.");
                    }
                    if (!string.IsNullOrEmpty(columnValues["MarkFav"].ToString())) 
                    {
                        IWebElement element = null;
                        foreach (var ele in grids)
                        {
                            if (ele.Text.ToString().ToLower().Equals(columnValues["MarkFav"].ToString().ToLower())) 
                            {
                                element = ele;
                                break;
                            }
                        }
                        var allAction = element.FindElements(By.TagName("li"));
                        allAction[0].Click();
                        
                    }
                    if (!string.IsNullOrEmpty(columnValues["DeleteHotButton"].ToString()))
                    {
                        IWebElement element = null;
                        foreach (var ele in grids)
                        {
                            if (ele.Text.ToString().ToLower().Equals(columnValues["DeleteHotButton"].ToString().ToLower()))
                            {
                                element = ele;
                                break;
                            }
                        }
                        var allAction = element.FindElements(By.TagName("li"));
                        allAction[1].Click();
                        Wait(2000);
                        if (columnValues["ConfirmDeletePopUp"].ToString().ToLower().Contains("false"))
                        {
                            PerformActions(driver, "Click", dict["AllowTrade_No"].ToString(), row, "AllowTrade_No");
                        }
                        else 
                        {
                            PerformActions(driver, "Click", dict["AllowTrade_Yes"].ToString(), row, "AllowTrade_Yes");
                        }

                    }
                    if (!string.IsNullOrEmpty(columnValues["CloneHotButton"].ToString()))
                    {
                        IWebElement element = null;
                        foreach (var ele in grids)
                        {
                            if (ele.Text.ToString().ToLower().Equals(columnValues["CloneHotButton"].ToString().ToLower()))
                            {
                                element = ele;
                                break;
                            }
                        }
                        var allAction = element.FindElements(By.TagName("li"));
                        allAction[2].Click();

                    }

                    if (!string.IsNullOrEmpty(columnValues["EditHotButton"].ToString()))
                    {
                        IWebElement element = null;
                        foreach (var ele in grids)
                        {
                            if (ele.Text.ToString().ToLower().Equals(columnValues["EditHotButton"].ToString().ToLower()))
                            {
                                element = ele;
                                break;
                            }
                        }
                        var allAction = element.FindElements(By.TagName("li"));
                        allAction[3].Click();

                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("FilterUiRighClick"))
                {
                    try
                    {
                        int k = 0;
                        while (true)
                        {
                            try
                            {
                                if (!row["GridType"].ToString().ToUpper().Contains("SUMMARY"))
                                {
                                    DefaultCellValue = "cellid" + k + "r1c2";
                                }
                                else
                                {
                                    DefaultCellValue = "cellid" + k + "r1c19";
                                }
                                driver.FindElement(By.Id(DefaultCellValue)).Click();
                                break;
                            }
                            catch
                            {
                                k++;
                            }
                        }
                        IWebElement element = driver.FindElement(By.Id(DefaultCellValue));
                        IWebElement ele = element.FindElement(By.ClassName(dict["FilterIconClass"].ToString()));
                        ele.Click();
                        Wait(3000);
                        IWebElement ui = driver.FindElement(By.ClassName(dict["FilterUi"].ToString()));
                        Actions actions = new Actions(driver);
                        actions.ContextClick(ui).Perform();
                        Wait(1500);
                        IWebElement context = driver.FindElement(By.ClassName("contexify"));
                        IList<IWebElement> divElements = context.FindElements(By.TagName("span"));
                        bool flag = false;
                        foreach (var el in divElements)
                        {
                            Console.WriteLine(el.Text.ToString());
                            if (el.Text.ToString().Equals(row["RightClickOperationToVerify"].ToString()))
                                flag = true;
                        }
                        if (flag)
                            throw new Exception("Right Click filter UI has context menu issue");
                        columnValues.Clear();
                        ele.Click();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("RoundLot,Increment,Decrement"))
                {
                    if (TestCaseSheet.Tables[0].Columns.Contains("RoundLot") && !string.IsNullOrEmpty(row["RoundLot"].ToString()))
                    {
                        PerformActions(driver, "Click", dict["RoundLotButton"].ToString(), row, "RoundLotButton");
                        Wait(2000);
                        PerformActionThroughClass(driver, "Click", dict["RoundLotSlider"].ToString(), row, "RoundLotSlider");
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Increment") && !string.IsNullOrEmpty(row["Increment"].ToString()))
                    {
                        PerformActionThroughClass(driver, "Click", dict["Increment"].ToString(), row, "Increment");
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Decrement") && !string.IsNullOrEmpty(row["Decrement"].ToString()))
                    {
                        PerformActionThroughClass(driver, "Click", dict["Decrement"].ToString(), row, "Decrement");
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("SaveQTT"))
                {
                    IWebElement element = driver.FindElement(By.ClassName(dict["DropDownActions"].ToString()));
                    IList<IWebElement> divElements = element.FindElements(By.TagName("a"));
                    if (string.IsNullOrEmpty(columnValues["Action"]))
                    {
                        columnValues["Action"] = "Save";
                    }
                    foreach (IWebElement ele in divElements)
                    {
                        if (ele.Text.Equals(columnValues["Action"]))
                        {
                            ele.Click();
                            break;
                        }
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("CheckModuleAndOpen"))
                {
                    bool flag = SwitchWindow.SwitchToWindow(driver, moduleName);
                    if (!flag)
                    {
                        SwitchWindow.SwitchToWindow(driver, "Dock");
                        OpenModule(moduleName, driver);
                    }

                }
                else if (table.Rows[i]["Steps"].ToString().Equals("VerifyPopupAlignment"))
                {
                    string PopupName = columnValues["PopupName"].ToString().ToUpper();
                    int tolerance = Convert.ToInt32(dict["AlignTolerance"]);
                    string popupXPath = "";
                    if (PopupName.Equals("NEWTRADE"))
                    {
                        popupXPath = dict["NewTradePopup"].ToString();
                    }
                    else
                    {
                        popupXPath = dict["DuplicateTradePopup"].ToString();
                    }
                    string parentWindowTitle = "Trading Ticket";
                    SwitchWindow.SwitchToWindow(driver, parentWindowTitle);
                    try
                    {
                        VerifyPopupAlignment(driver, popupXPath, tolerance);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("TradeQTT"))
                {
                    if (columnValues["Add To Favourite"].ToUpper().Equals("TRUE"))
                    {
                        PerformActionsThroughID(driver, "Click", dict["Add To Favourite"].ToString(), row, "Add To Favourite");
                    }
                    if (columnValues["Symbol"].ToUpper().Equals("CLEAR"))
                    {
                        IWebElement ClearBtn = driver.FindElement(By.XPath(dict["ClearSymbol"].ToString()));
                        ClearBtn.Click();
                    }
                    if (columnValues["Quantity"].ToUpper().Equals("CLEAR"))
                    {
                        IWebElement ClearBtn = driver.FindElement(By.XPath(dict["ClearQuantity"].ToString()));
                        ClearBtn.Click();
                    }
                    if (columnValues["Allocation"].ToUpper().Equals("CLEAR"))
                    {
                        IWebElement ClearBtn = driver.FindElement(By.XPath(dict["ClearAllocation"].ToString()));
                        ClearBtn.Click();
                    }
                    if (columnValues["Broker"].ToUpper().Equals("CLEAR"))
                    {
                        IWebElement ClearBtn = driver.FindElement(By.XPath(dict["ClearBroker"].ToString()));
                        ClearBtn.Click();
                    }
                    if (columnValues["Venue"].ToUpper().Equals("CLEAR"))
                    {
                        IWebElement ClearBtn = driver.FindElement(By.XPath(dict["ClearVenue"].ToString()));
                        ClearBtn.Click();
                    }
                    if (columnValues["TIF"].ToUpper().Equals("CLEAR"))
                    {
                        IWebElement ClearBtn = driver.FindElement(By.XPath(dict["ClearTIF"].ToString()));
                        ClearBtn.Click();
                    }
                    if (columnValues["Order Type"].ToUpper().Equals("CLEAR"))
                    {
                        IWebElement ClearBtn = driver.FindElement(By.XPath(dict["ClearOrderType"].ToString()));
                        ClearBtn.Click();
                    }
                    if (columnValues["Execution Type"].ToUpper().Equals("CLEAR"))
                    {
                        IWebElement ClearBtn = driver.FindElement(By.XPath(dict["ClearET"].ToString()));
                        ClearBtn.Click();
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("Expiration Date") && !string.IsNullOrEmpty(columnValues["Expiration Date"]))
                    {
                        Wait(1000);
                        int selectionDate = 0;
                        try
                        {
                            driver.FindElement(By.XPath(dict["GtdCalendar"].ToString())).Click();
                        }
                        catch
                        {
                            driver.FindElement(By.XPath(dict["GtdCalendarButton"].ToString())).Click();
                        }
                        selectionDate = GTD(row);
                        DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, selectionDate);
                        if (IsWeekend(date))
                        {
                            DateTime nextWorkingDay = GetNextWorkingDay(date);
                            selectionDate = nextWorkingDay.Day;
                        }
                        IWebElement element = driver.FindElement(By.XPath(dict["GtdCalendarDate"].ToString()));
                        var allElements = element.FindElements(By.XPath(".//*"));
                        var columnHeaderElements = allElements.Where(e => e.GetAttribute("type") == "button").ToList();
                        bool currentMonth = false;
                        for (int j = 0; j < columnHeaderElements.Count; j++)
                        {
                            if (!currentMonth)
                            {
                                while (!columnHeaderElements[j].Text.ToString().Equals("1"))
                                {
                                    j++;
                                }
                                currentMonth = true;
                            }
                            if (columnHeaderElements[j].Text.Equals(selectionDate.ToString()))
                            {
                                columnHeaderElements[j].Click();
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(columnValues["Select Color"]))
                    {
                        PerformActionThroughClass(driver, "Click", dict["Select Color"].ToString(), row, "Select Color");
                        Wait(3000);
                        /*IWebElement elm = driver.FindElement(By.XPath(dict["QTTscroller"].ToString()));
                        Actions actions = new Actions(driver);
                        actions.MoveToElement(elm).Perform();


                        actions
                            .ClickAndHold(elm)
                            .MoveByOffset(0, 250)
                            .Release()
                            .Perform();
                        Wait(2000);
                         */

                        var elm1 = driver.FindElement(By.ClassName(dict["ColorSelector"].ToString()));
                        var allElements = elm1.FindElements(By.TagName("span"));
                        foreach (var ele in allElements)
                        {
                            Console.WriteLine(ele.GetAttribute("class"));
                            if (ele.GetAttribute("class").ToString().ToUpper().Contains(columnValues["Select Color"].ToString().ToUpper()))
                            {
                                IWebElement parentElement = ele.FindElement(By.XPath(".."));
                                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                                js.ExecuteScript("arguments[0].click();", parentElement);
                                break;
                            }
                        }
                        Wait(2000);

                    }

                    if (!columnValues["Save"].ToUpper().Equals("FALSE"))
                    {
                        PerformActionThroughClass(driver, "Click", dict["Save"].ToString(), row, "Save");
                    }
                }

                else if (table.Rows[i]["Steps"].ToString().Equals("ClearWidgetName"))
                {
                    if (columnValues["WidgetName"].ToUpper().Equals("CLEAR"))
                    {
                        IWebElement element = driver.FindElement(By.XPath(dict2["WidgetName"].ToString()));
                        element.Clear();
                        IWebElement chipExpad = driver.FindElement(By.XPath(dict["ChipExpand"].ToString()));
                        chipExpad.Click();
                        chipExpad.Click();
                    }
                }
                else if (!string.IsNullOrEmpty(table.Rows[i]["Steps"].ToString()) && string.Equals(table.Rows[i]["Steps"].ToString(), "linkdashboard", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        DataTable dtable = TestCaseSheet.Tables[StepName];
                        if (dtable == null && TestCaseSheet.Tables.Count > 0)
                        {
                            dtable = TestCaseSheet.Tables[0];
                        }
                        try
                        {
                            // SamsaraGridOperationHelper.SortGridOnBlotter(driver, dtable, StepName, ref dict);
                            SamsaraGridOperationHelper.LinkDashBoard(driver, dtable, ref dict, table.Rows[i]["Action"].ToString().Split(',').ToList());
                            //
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                }
                else if (string.Equals(table.Rows[i]["Action"].ToString(), "BreakLoop", StringComparison.OrdinalIgnoreCase))
                {
                    needNextIteration = false;
                    break;
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("Order Side"))
                {
                    try
                    {
                        IWebElement element = driver.FindElement(By.ClassName(dict["ClassOfOrderSide"].ToString()));
                        if (StepName.Equals("CustomOrderTT") && string.IsNullOrEmpty(row["Order type button"].ToString()))
                        {
                            continue;
                        }
                        try
                        {
                            IWebElement button = driver.FindElement(By.XPath(dict["ButtonTrade"].ToString()));
                            if (button.Displayed && button.Text.Equals("Trade"))
                            {
                                button.Click();
                                continue;
                            }
                        }
                        catch { }

                        try
                        {
                            IWebElement button = driver.FindElement(By.XPath(dict2["ButtonTrade"].ToString()));
                            if (button.Displayed && button.Text.Equals("Trade"))
                            {
                                button.Click();
                                continue;
                            }
                        }
                        catch { }

                        if (!ConfigurationManager.AppSettings["SkipOpenModule"].Contains(StepName))
                        {
                            IList<IWebElement> divElements = element.FindElements(By.TagName("button"));
                            bool flag = false;
                            if (StepName.Equals("OptionTradingThroughSecurity"))
                            {
                                flag = true;
                            }
                            if (string.IsNullOrEmpty(row["Order Side"].ToString()))
                            {
                                foreach (var ele in divElements)
                                {
                                    try
                                    {
                                        string ordersidename = ele.FindElement(By.TagName("span")).Text;
                                        if (ordersidename.Equals("Buy") || ordersidename.Equals("Buy to Open"))
                                        {
                                            ele.Click();
                                            Console.WriteLine("Clicking on " + ordersidename + " order side button");
                                            flag = true;
                                            break;
                                        }
                                        else if (ordersidename.Equals("Sell"))
                                        {
                                            ele.Click();
                                            Console.WriteLine("Clicking on " + ordersidename + " order side button");
                                            flag = true;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.Message.Contains("{\"method\":\"tag name\",\"selector\":\"span\"}"))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            throw ex;
                                        }
                                    }
                                }

                            }
                            else
                            {
                                if (divElements.Count == 2)
                                {
                                    try
                                    {
                                        string ordersidename = divElements[0].FindElement(By.TagName("span")).Text;
                                    }
                                    catch
                                    {
                                        divElements[1].Click();
                                        flag = true;
                                        continue;
                                    }
                                }
                                foreach (var ele in divElements)
                                {
                                    try
                                    {
                                        string ordersidename = ele.FindElement(By.TagName("span")).Text.ToUpper();
                                        if (ordersidename.Equals(row["Order Side"].ToString().ToUpper()) || (ordersidename.Equals("BUY TO COVER") && row["Order Side"].ToString().ToUpper().Equals("BUY TO CLOSE")))
                                        {
                                            ele.Click();
                                            Console.WriteLine("Clicking on " + ordersidename + " order side button");
                                            flag = true;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.Message.Contains("{\"method\":\"tag name\",\"selector\":\"span\"}"))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            throw ex;
                                        }
                                    }
                                }
                            }
                            if (!flag)
                            {
                                IWebElement elem = element.FindElement(By.ClassName("visible"));
                                elem.Click();
                            }
                        }
                        else
                        {
                            try
                            {
                                IWebElement ele = element.FindElement(By.ClassName("visible"));
                                ele.Click();
                            }
                            catch
                            {
                                IList<IWebElement> divElements = element.FindElements(By.TagName("button"));
                                bool flag = false;
                                if (string.IsNullOrEmpty(row["Order Side"].ToString()))
                                {
                                    foreach (var ele in divElements)
                                    {
                                        try
                                        {
                                            string ordersidename = ele.FindElement(By.TagName("span")).Text;
                                            if (ordersidename.Equals("Buy") || ordersidename.Equals("Buy to Open"))
                                            {
                                                ele.Click();
                                                Console.WriteLine("Clicking on " + ordersidename + " order side button");
                                                flag = true;
                                                break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            if (ex.Message.Contains("{\"method\":\"tag name\",\"selector\":\"span\"}"))
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                throw ex;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var ele in divElements)
                                    {
                                        try
                                        {
                                            string ordersidename = ele.FindElement(By.TagName("span")).Text.ToUpper();
                                            if (ordersidename.Equals(row["Order Side"].ToString().ToUpper()))
                                            {
                                                ele.Click();
                                                Console.WriteLine("Clicking on " + ordersidename + " order side button");
                                                flag = true;
                                                break;
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            if (ex.Message.Contains("{\"method\":\"tag name\",\"selector\":\"span\"}"))
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                throw ex;
                                            }
                                        }
                                    }
                                }
                                if (!flag)
                                {
                                    throw new Exception("Order Side " + row["Order Side"] + " is not found for symbol " + row["Symbol"] + ". Please refer to Samsara Screenshot");
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        CaptureMyScreen("Order Side", ApplicationArguments.TestCaseToBeRun + " Order Side", "Order Side side not found for Step " + StepName);
                        throw new Exception("Order Side " + row["Order Side"] + " is not found for symbol " + row["Symbol"] + ". Please refer to Samsara Screenshot");

                    }
                }
                else if (table.Rows[i]["Action"].ToString().Equals("Select"))
                {
                    try
                    {
                        if (RowIndex >= 0)
                        {
                            PerformActionsVerification(driver, table.Rows[i]["Action"].ToString(), dict["OrderGridAGgridID"].ToString(), "");
                        }
                    }
                    catch
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("HotButtonPanel"))
                {
                    if (!string.IsNullOrEmpty(columnValues["EnableHotButton"].ToString()))
                    {
                        PerformActions(driver, "Click", dict["hotButtonPanel"].ToString(), row, "");
                        Wait(2000);
                    }
                    if (!string.IsNullOrEmpty(columnValues["VisiblityOfHotButtonPanel"].ToString()))
                    {
                        try
                        {
                            driver.FindElement(By.ClassName(dict["hotButtonPanelClass"].ToString()));
                            if (!columnValues["VisiblityOfHotButtonPanel"].ToString().ToUpper().Equals("ENABLE"))
                                throw new Exception("Hot Button Panel is disabled");
                        }
                        catch
                        {
                            if (!columnValues["VisiblityOfHotButtonPanel"].ToString().ToUpper().Equals("DISABLE"))
                                throw new Exception("Hot Button Panel is enabled");
                        }
                    }
                    columnValues.Clear();
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("VerifyHotButtonKeysOnPanel"))
                {
                    if (TestCaseSheet.Tables[0].Columns.Contains("HotKey Name"))
                    {
                        if (!string.IsNullOrEmpty(columnValues["HotKey Name"].ToString()))
                        {
                            bool flag = false;
                            if (columnValues["TabToFind"].ToString().ToLower().Contains("non"))
                            {
                                try
                                {
                                    driver.FindElement(By.ClassName(dict["NonFavPanelClass"].ToString())).Click();
                                }
                                catch
                                {
                                    IWebElement grid = driver.FindElement(By.ClassName(dict["NonFavPanelButton"].ToString()));
                                    IWebElement grid1 = grid.FindElement(By.ClassName("d-flex"));
                                    IList<IWebElement> thirdChild = grid1.FindElements(By.XPath("./*"));
                                    Console.WriteLine(thirdChild.Count);
                                    Actions actions = new Actions(driver);
                                    actions.MoveToElement(thirdChild[2]).Click().Perform();
                                    Wait(2000);
                                }
                                IWebElement nonFav = driver.FindElement(By.ClassName(dict["NonFavPanelClass"].ToString()));
                                IList<IWebElement> divElements = nonFav.FindElements(By.TagName("span"));
                                string hotKeyName = (columnValues["HotKey Name"].ToString().ToUpper());
                                foreach (var ele in divElements)
                                {
                                    if (ele.Text.ToString().ToUpper().Contains(columnValues["HotKey Name"].ToString().ToUpper()))
                                    {
                                        flag = true;
                                        if (row.Table.Columns.Contains("UseHotButton"))
                                        {
                                            if (columnValues["UseHotButton"].ToString().ToUpper().Equals("TRUE"))
                                            {
                                                try
                                                {
                                                    ele.Click();
                                                    Console.WriteLine("Clicked on HotKey (Fav): " + hotKeyName);
                                                    Wait(2000);
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw new Exception("Failed to click HotKey (Fav): " + hotKeyName + " - " + ex.Message);
                                                }
                                            }


                                        }
                                        break;
                                    }
                                }
                                if (!columnValues["HotKey Visiblity"].ToString().ToUpper().Equals(flag.ToString().ToUpper()))
                                {
                                    throw new Exception(columnValues["HotKey Name"].ToString() + " hotKey visiblity is " + flag + " but excel has value as " + columnValues["HotKey Visiblity"].ToString());
                                }

                            }
                            else
                            {
                                IWebElement Fav = driver.FindElement(By.ClassName(dict["hotButtonPanelClass"].ToString()));
                                IList<IWebElement> divElements = Fav.FindElements(By.TagName("span"));
                                string hotKeyName = (columnValues["HotKey Name"].ToString().ToUpper());
                                foreach (var ele in divElements)
                                {
                                    if (ele.Text.ToString().ToUpper().Contains(columnValues["HotKey Name"].ToString().ToUpper()))
                                    {
                                        flag = true;
                                        if (row.Table.Columns.Contains("UseHotButton"))
                                        {
                                            if (columnValues["UseHotButton"].ToString().ToUpper().Equals("TRUE"))
                                            {
                                                try
                                                {
                                                    ele.Click();
                                                    Console.WriteLine("Clicked on HotKey (Fav): " + hotKeyName);
                                                    Wait(2000);
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw new Exception("Failed to click HotKey (Fav): " + hotKeyName + " - " + ex.Message);
                                                }
                                            }


                                        }
                                        break;
                                    }
                                }
                                if (!string.IsNullOrEmpty(columnValues["HotKey Visiblity"].ToString()) && !columnValues["HotKey Visiblity"].ToString().ToUpper().Equals(flag.ToString().ToUpper()))
                                {
                                    throw new Exception(columnValues["HotKey Name"].ToString() + " hotKey visiblity is " + flag + " but excel has value as " + columnValues["HotKey Visiblity"].ToString());
                                }
                                if (TestCaseSheet.Tables[0].Columns.Contains("Hotkey Disbable"))
                                {
                                    IWebElement disable = driver.FindElement(By.ClassName(dict["hotButtonPanelClass"].ToString()));
                                    IList<IWebElement> disableelement = Fav.FindElements(By.TagName("button"));
                                    foreach (var ele in disableelement)
                                    {
                                        bool isDisabled = ele.GetAttribute("disabled") != null;
                                        if (!string.IsNullOrEmpty(row["Hotkey Disbable"].ToString()) && !row["Hotkey Disbable"].ToString().ToLower().Equals(isDisabled.ToString().ToLower()))
                                        {
                                            throw new Exception("Hot Key Disable/Enable not matched according to Excel");
                                        }
                                    }
                                }
                            }

                        }
                        columnValues.Clear();
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("GetIndex"))
                {
                    try
                    {
                        DataTable Data = new DataTable();
                        if (ExcelData == null)
                        {
                            ExcelData = TestCaseSheet.Tables[0];
                        }
                        string fileName = table.Rows[i]["Action"].ToString();
                        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                        string filePath2 = SearchFile(downloadsPath, fileName);
                        if (!string.IsNullOrEmpty(filePath2))
                        {
                            List<string> list = new List<string>();
                            DataSet ds = DataUtilities.GetTestCaseTestData(filePath2, 1, 1, list);
                            Data = ds.Tables.Count > 0 ? ds.Tables[0] : null;
                        }
                        Data = removeSuborderRows(Data);
                        //RemoveColumnsAndRows
                        List<string> Columns = new List<string>();
                        Columns.Add("Symbol");
                        Columns.Add("Quantity");
                        Columns.Add("Side");
                        DataRow dtRow = GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(Data), row, Columns);
                        RowIndex = Data.Rows.IndexOf(dtRow);
                        //RowIndex += 1;
                        if (Data.Rows.IndexOf(dtRow) < 0)
                        {
                            CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " Trade not found");
                            string quantity = TestCaseSheet.Tables[0].Columns.Contains("Target Qty") ? TestCaseSheet.Tables[0].Rows[0]["Target Qty"].ToString() : TestCaseSheet.Tables[0].Rows[0]["Quantity"].ToString();
                            RowIndex = 1;
                            throw new Exception("Trade not found during " + StepName + " step: [Symbol= " + TestCaseSheet.Tables[0].Rows[0]["Symbol"] + "], Quantity = [" + quantity + "] Side = [" + TestCaseSheet.Tables[0].Rows[0]["Side"] + "]. Please refer to Samsara Screenshot");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("TransferOrder"))
                {
                    if (StepName.Equals("TransferToUserOrder") || StepName.Equals("MultipleTradeTransferToUser"))
                    {
                        if (columnValues.Keys.Contains("userName") && !string.IsNullOrEmpty(columnValues["userName"].ToString()))
                        {
                            SamsaraGridOperationHelper.PerformRightClickActions(driver, columnValues["userName"].ToString(), dict["TransferToUserOrderMenu"].ToString(), dict2["TransferToUserOrderMenu"].ToString());
                        }                    
                    }
                    else if (StepName.Equals("TransferToUserSubOrder"))
                    {
                        SamsaraGridOperationHelper.PerformRightClickActions(driver, columnValues["userName"].ToString(), dict["TransferToUserSubOrderMenu"].ToString());
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("CloseService"))
                {
                    if (!string.IsNullOrEmpty(row["ServiceNametoKill"].ToString()))
                    {
                        try
                        {
                            Process[] processes = Process.GetProcessesByName(row["ServiceNametoKill"].ToString());
                            foreach (Process process in processes)
                            {
                                process.Kill();
                                process.WaitForExit();
                                Console.WriteLine(row["ServiceNametoKill"].ToString() + " killed Successfully");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error killing the " + row["ServiceNametoKill"].ToString() + ex.Message);
                        }
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().ToLower().Equals("switchcolumnwindow"))
                {
                    try
                    {
                        string value = TestCaseSheet.Tables[0].Rows[0][table.Rows[i]["Action"].ToString()].ToString();
                        if (!SwitchWindow.SwitchToWindow(driver, value))
                        {
                            _Res.IsPassed = false;
                            return _Res;
                        }
                    }
                    catch (Exception ColumnNotFound)
                    {
                        throw new Exception(table.Rows[i]["Action"].ToString() + "Column not found in Testcase sheet" + ColumnNotFound);
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("SelectTradeOnBlotter"))
                {
                    if (!row.Table.Columns.Contains("Allocation Status"))
                    {
                        if (row.Table.Columns.Contains("Quantity") && row.Table.Columns.Contains("Target Qty"))
                        {
                            row.Table.Columns.Remove("Quantity");
                            row.Table.Columns["Target Qty"].ColumnName = "Quantity";
                        }
                        driver.FindElement(By.XPath(dict["OrderButton"].ToString())).Click();
                    }
                    else
                    {
                        if (row.Table.Columns.Contains("Target Qty"))
                            row.Table.Columns.Remove("Target Qty");
                        driver.FindElement(By.XPath(dict["WorkingTab"].ToString())).Click();
                    }
                    DataTable Data = new DataTable();
                    string orderGrid = string.Empty;
                    try
                    {
                        driver.FindElement(By.Id(dict["OrderGridID"].ToString())).Click();
                        orderGrid = dict["OrderGridID"].ToString();
                    }
                    catch
                    {
                        driver.FindElement(By.Id(dict["WorkingGridID"].ToString())).Click();
                        orderGrid = dict["WorkingGridID"].ToString();
                    }
                    if (ExcelData == null)
                    {
                        ExcelData = TestCaseSheet.Tables[0];
                    }
                    List<String> columns = new List<string>();

                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref ExcelData);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandler"], ref ExcelData);
                    string fileName = table.Rows[i]["Action"].ToString();
                    string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    string filePath2 = SearchFile(downloadsPath, fileName);
                    if (!string.IsNullOrEmpty(filePath2))
                    {
                        List<string> list = new List<string>();
                        DataSet ds = DataUtilities.GetTestCaseTestData(filePath2, 1, 1, list);
                        if (orderGrid.ToLower().Contains("working"))
                        {
                            Data = ds.Tables["Working"];
                        }
                        else
                        {
                            Data = ds.Tables["Orders"];
                        }
                    }
                    Data = removeSuborderRows(Data);
                    List<string> Columns = new List<string>();
                    Columns.Add("Symbol");
                    Columns.Add("Quantity");
                    Columns.Add("Side");
                    DataRow dtRow = GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(Data), row, Columns);
                    RowIndex = Data.Rows.IndexOf(dtRow) + 2;
                    var element = driver.FindElement(By.Id(orderGrid));
                    var allElements = element.FindElements(By.XPath(".//*"));
                    var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                    foreach (var elements in columnHeaderElements)
                    {
                        string elementId = elements.GetAttribute("id");
                        DefaultCellValue = "#" + elementId.Substring(0, elementId.IndexOf("r") + 1);
                        break;
                    }
                    if (Data.Rows.IndexOf(dtRow) < 0)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " Trade not found");
                        string quantity = TestCaseSheet.Tables[0].Columns.Contains("Target Qty") ? TestCaseSheet.Tables[0].Rows[0]["Target Qty"].ToString() : TestCaseSheet.Tables[0].Rows[0]["Quantity"].ToString();
                        RowIndex = 1;
                        throw new Exception("Trade not found during " + StepName + " step: [Symbol= " + TestCaseSheet.Tables[0].Rows[0]["Symbol"] + "], Quantity = [" + quantity + "] Side = [" + TestCaseSheet.Tables[0].Rows[0]["Side"] + "]. Please refer to Samsara Screenshot");
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("RenameColumn"))
                {
                    RenameColumnName(row.Table, table.Rows[i]["Action"].ToString().Split(',')[0], table.Rows[i]["Action"].ToString().Split(',')[1]);
                }
                else if (table.Rows[i]["Steps"].ToString().ToLower().Equals("exactswitchwindow"))
                {
                    string value = table.Rows[i]["Action"].ToString();
                    SwitchWindow.SwitchToWindow(driver, value, true);
                }
                else if (table.Rows[i]["Steps"].ToString().Contains("GetColumnValue"))
                {
                    try
                    {
                        List<string> columns = new List<string>();
                        columns.AddRange(table.Rows[i]["Action"].ToString().Split(','));
                        for (int k = 0; k < columns.Count; k++)
                        {
                            
                                columnValues.Add(columns[k], Convert.ToString(row[columns[k]]));
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Contains("RemoveColumnsFromDataSheet"))
                {

                    ExcelData = DataUtilities.RemoveColumnsAndRows(table.Rows[i]["Action"].ToString(), TestCaseSheet.Tables[0]);

                }
                else if (table.Rows[i]["Steps"].ToString().Contains("SortGridOnBlotter"))
                {
                    try
                    {
                        DataTable dtable = TestCaseSheet.Tables[StepName];
                        if (dtable == null && TestCaseSheet.Tables.Count > 0)
                        {
                            dtable = TestCaseSheet.Tables[0];
                        }
                        try
                        {
                            bool verificationSucceeded = SamsaraGridOperationHelper.SortGridOnBlotter(driver, dtable, StepName, ref dict);
                            //
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                else if (table.Rows[i]["Steps"].ToString().Contains("ClickElement"))
                {
                    try
                    {
                        SamsaraGridOperationHelper.ClickElement(driver, table.Rows[i]["Steps"].ToString(), ref dict, table.Rows[i]["Action"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                        Console.WriteLine(ex.StackTrace);

                        /*if (rethrow)
                            throw;*/
                    }
                }

                else if (!string.IsNullOrEmpty(table.Rows[i]["Steps"].ToString()) && string.Equals(table.Rows[i]["Steps"].ToString(), "SwitchWithChildTabVerification", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        List<string> DashBoardList = table.Rows[i]["Action"].ToString().Split(',').ToList();
                        if (DashBoardList.Count > 0)
                        {
                            if (DashBoardList.Count == 1)
                            {
                                SwitchWindow.SwitchToWindow(driver, DashBoardList[0].ToString(), true);
                            }
                            else
                            {
                                SwitchWindow.SwitchToChildWindow(driver, DashBoardList[0].ToString(), dict[DashBoardList[1].ToString() + "Dashboard"].ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There is an issue while performing SwitchWithChildTabVerification     : " + ex.Message);
                    }

                }

                else if (table.Rows[i]["Steps"].ToString().Contains("Filterongrid"))
                {
                    DataTable dtable = TestCaseSheet.Tables[StepName];
                    if (dtable == null && TestCaseSheet.Tables.Count > 0)
                    {
                        dtable = TestCaseSheet.Tables[0];
                    }
                    try
                    {
                        bool IsFilter = SamsaraGridOperationHelper.FilterGrid(driver, ref dict, table.Rows[i]["Action"].ToString() + "Headers", dtable);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                else if (table.Rows[i]["Steps"].ToString().Contains("SortingOnGrid"))
                {
                    DataTable dtable = TestCaseSheet.Tables[StepName];
                    if (dtable == null && TestCaseSheet.Tables.Count > 0)
                    {
                        dtable = TestCaseSheet.Tables[0];
                    }
                    try
                    {
                        bool IsSorting = SamsaraGridOperationHelper.SortGridOnRtpnl(driver, ref dict, table.Rows[i]["Action"].ToString() + "Headers", dtable);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                else if (table.Rows[i]["Steps"].ToString().Contains("SelectColumns"))
                {
                    try
                    {
                        DataTable dtable = TestCaseSheet.Tables[0];
                        bool isSelect = SamsaraGridOperationHelper.SelectColumn(driver, ref dict, dtable);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                else if (table.Rows[i]["Steps"].ToString().Contains("Column"))
                {
                    string xpathName = string.Empty;
                    try
                    {
                        string name = table.Rows[i]["Steps"].ToString().Replace("Column_", "");
                        xpathName = name + "_" + columnValues[name];
                        PerformActions(driver, table.Rows[i]["Action"].ToString(), dict[xpathName].ToString(), row, xpathName);
                    }
                    catch (Exception)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                        Console.WriteLine(xpathName + " is not enabled or present");
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Contains("EditAddFillsDetails"))
                {
                    SamsaraGridOperationHelper.EditAddFillsDetails(driver, TestCaseSheet.Tables[0], StepName, DefaultCellValue, dict);
                }
                else if (table.Rows[i]["Steps"].ToString() == "")
                {
                    if (table.Rows[i]["OpenOtherModule"].ToString() != "")
                    {
                        try
                        {
                            DataTable table1 = DataTables[table.Rows[i]["OpenOtherModule"].ToString()];
                            for (int rowcount = 0; rowcount < table1.Rows.Count; rowcount++)
                            {
                                var row2 = table1.Rows[rowcount];
                                var value1 = row2[0].ToString();
                                CallStep(driver, value1, rowcount, table1, row2, row, dict, TestCaseSheet);
                            }
                        }
                        catch
                        {
                            CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun);
                        }
                    }
                }
                else if (table.Rows[i]["Action"].ToString().Contains("ClickAndSendKeysOnGrid"))
                {
                    try
                    {
                        if (table.Rows[i]["Steps"].ToString().Contains("HandleSoftWithNotes"))
                        {
                            int colindex = UIData.Columns.IndexOf("User Notes");
                            string searchString = "User Notes";
                            List<int> foundIndices = DataUtilities.FindIndicesInDataTable(ref UIData, ref searchString);
                            if (foundIndices.Count != 0)
                            {

                                for (int ik = 0; ik < foundIndices.Count; ik++)
                                {
                                    string id = DefaultCellValue + (ik + 2) + "c" + colindex;
                                    IWebElement element = driver.FindElement(By.CssSelector(id));
                                    Actions actions2 = new Actions(driver);


                                    actions2.MoveToElement(element).Perform();
                                    actions2.Click(element).Perform();
                                    actions2.SendKeys(columnValues["Write UserNote"]).Build().Perform();
                                }


                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                else if (table.Rows[i]["Steps"].ToString() == "CheckRow")
                    continue;
                else if (table.Rows[i]["steps"].ToString().ToLower() == "hotbuttonqty")
                {
                    try
                    {
                        IWebElement elementToDoubleClick = driver.FindElement(By.XPath(dict["Quantity"].ToString()));

                        Actions actions = new Actions(driver);
                        actions.DoubleClick(elementToDoubleClick).Perform();
                        string hotButtonName = "HotButtonQty_" + row["HotButtonQty"].ToString();
                        string HotButtonXpath = dict[hotButtonName].ToString();
                        PerformActions(driver, table.Rows[i]["Action"].ToString(), HotButtonXpath, row, table.Rows[i]["Steps"].ToString());
                    }
                    catch
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                    }
                }
                else if (table.Columns.Contains("OpenOtherModule") && table.Rows[i]["OpenOtherModule"].ToString().Contains("AllocateFromBlotterGridFill"))
                {

                    SwitchWindow.SwitchToWindow(driver, "Allocation");

                    DataTable dtable = TestCaseSheet.Tables[StepName];
                    if (dtable == null && TestCaseSheet.Tables.Count > 0)
                    {
                        dtable = TestCaseSheet.Tables[0];
                    }
                    SamsaraGridOperationHelper.AllocateFromBlotterGridFill(driver, dtable, StepName, table.Rows[i]["Action"].ToString(), ref dict);
                }

                else if (table.Rows[i]["steps"].ToString().ToLower() == "hotbutton")
                {
                    try
                    {
                        IWebElement elementToDoubleClick = driver.FindElement(By.XPath(dict["Price"].ToString()));

                        Actions actions = new Actions(driver);
                        actions.DoubleClick(elementToDoubleClick).Perform();
                        string ColumnName = table.Rows[i]["Steps"].ToString() + table.Rows[i - 1]["Steps"].ToString();
                        string hotButtonName = ColumnName + "_" + row[ColumnName].ToString();
                        string HotButtonXpath = dict[hotButtonName].ToString();
                        PerformActions(driver, table.Rows[i]["Action"].ToString(), HotButtonXpath, row, table.Rows[i]["Steps"].ToString());
                    }
                    catch { }
                }
                else if (table.Rows[i]["Steps"].ToString().Contains("CheckRowCountOnUIGrid"))
                {
                    try
                    {
                        int tempRowCount = SamsaraGridOperationHelper.CheckRowCountOnUIGrid(driver, table.Rows[i]["Action"].ToString());
                        string ans = string.Empty;
                        if (!string.IsNullOrEmpty(table.Rows[i]["OpenOtherModule"].ToString()))
                        {
                            ans = table.Rows[i]["OpenOtherModule"].ToString();
                            if (tempRowCount == 1)
                            {
                                DataTable dt = DataTables[table.Rows[i]["OpenOtherModule"].ToString()];
                                //SamsaraGridOperationHelper.PerformActionsOnGrid(driver, DataTables[table.Rows[i]["OpenOtherModule"].ToString()] );
                                for (int iterator = 0; iterator < dt.Rows.Count; iterator++)
                                {
                                    try
                                    {
                                        if (dt.Rows[iterator]["Steps"].ToString() == "SwitchWindow")
                                        {
                                            SwitchWindow.SwitchToWindow(driver, dt.Rows[iterator]["Action"].ToString());
                                        }
                                        else if (dt.Rows[iterator]["Steps"].ToString().Contains("DefaultCellValue"))
                                        {
                                            SetDefaultSetValue(dt.Rows[iterator]["Action"].ToString());
                                        }
                                        else if (dt.Rows[iterator]["Steps"].ToString().Contains("UpdateRowIndex"))
                                        {
                                            UpdateRowIndex(int.Parse(dt.Rows[iterator]["Action"].ToString()));
                                        }
                                        else if (dt.Rows[iterator]["Steps"].ToString().Contains("ClickMenuItemByText"))
                                        {
                                            ClickMenuItemByText(driver, dt.Rows[iterator]["Action"].ToString(), 3);
                                        }
                                        else if (dt.Rows[iterator]["Action"].ToString().Contains("BreakLoop"))
                                        {
                                            needNextIteration = false;
                                            break;
                                        }
                                        else
                                            PerformActionsVerification(driver, dt.Rows[iterator]["Action"].ToString(), "", "");

                                    }
                                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                                }

                                break;
                            }
                        }
                        else
                        {
                            GlobalRowCount = tempRowCount;

                        }




                    }
                    catch { }
                }

                else if (string.Equals("AddOrRemoveColWithVerification", table.Rows[i]["Steps"].ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        DataTable dtable = TestCaseSheet.Tables[StepName];
                        if (dtable == null && TestCaseSheet.Tables.Count > 0)
                        {
                            dtable = TestCaseSheet.Tables[0];
                        }
                        SamsaraGridOperationHelper.AddOrRemoveColumnWithVerification(driver, dtable, StepName, table.Rows[i]["Action"].ToString(), ref dict);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                }

                else if (row.Table.Columns.Contains(table.Rows[i]["Steps"].ToString()))
                {
                    if (row[table.Rows[i]["Steps"].ToString()].ToString() != "")
                    {
                        string[] Datacount = row[table.Rows[i]["Steps"].ToString()].ToString().Split(',');
                        if (Datacount.Length > 1)
                        {
                            if (table.Rows[i]["OpenOtherModule"].ToString() != "")
                            {
                                var dataTable = DataTables[table.Rows[i]["OpenOtherModule"].ToString()];
                                for (int j = 0; j < dataTable.Rows.Count; j++)
                                {
                                    var row1 = dataTable.Rows[j];
                                    var value = row1[0].ToString();
                                    if (!CallStep(driver, value, j, dataTable, row1, row, dict))
                                    {
                                        if (value == "AddValues")
                                        {
                                            try
                                            {
                                                int count = 0;
                                                IWebElement element = driver.FindElement(By.XPath(dict["CustomAllocationRowFinder"].ToString()));
                                                IList<IWebElement> divElements = element.FindElements(By.XPath("//*[@role='gridcell']"));
                                                foreach (IWebElement ele in divElements)
                                                {
                                                    Console.WriteLine(ele.FindElement(By.TagName("span")).Text);
                                                    if (ele.FindElement(By.TagName("span")).Text.Contains("Allocation"))
                                                    {
                                                        count++;
                                                    }
                                                }
                                                while (count > 0)
                                                {
                                                    element = driver.FindElement(By.Id("cellid1r" + (count + 1) + "c0"));
                                                    Actions actions = new Actions(driver);
                                                    actions.ContextClick(element).Perform();
                                                    Wait(1500);
                                                    driver.FindElement(By.XPath(dict["DeleteRowCustomAllocation"].ToString())).Click();
                                                    count--;
                                                }
                                            }
                                            catch { }
                                            if (dataTable.Rows[j]["OpenOtherModule"].ToString() != "")
                                            {
                                                var dataTable1 = DataTables[dataTable.Rows[j]["OpenOtherModule"].ToString()];
                                                for (int l = 0; l < Datacount.Length; l++)
                                                {
                                                    for (int k = 0; k < dataTable1.Rows.Count; k++)
                                                    {
                                                        var row2 = dataTable1.Rows[k];
                                                        var value1 = row2[0].ToString();
                                                        CallStep(driver, value1, k, dataTable1, row2, row, dict, TestCaseSheet);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (ConfigurationManager.AppSettings["ACCenvironment"].ToString().Equals("true"))
                            {
                                try
                                {
                                    string title = driver.Title;
                                    string a = table.Rows[i]["Action"].ToString();
                                    string b = dict[table.Rows[i]["Steps"].ToString()].ToString();
                                    PerformActions(driver, table.Rows[i]["Action"].ToString(), dict[table.Rows[i]["Steps"].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                                    if (table.Rows[i]["Steps"].ToString().Equals("TIF") && row["TIF"].ToString().Equals("Good Till Date") && !a.Equals("Click"))
                                    {
                                        Wait(1000);
                                        int selectionDate = 0;
                                        try
                                        {
                                            driver.FindElement(By.XPath(dict["GtdCalendar"].ToString()));
                                        }
                                        catch
                                        {
                                            driver.FindElement(By.XPath(dict["GtdCalendarButton"].ToString())).Click();
                                        }
                                        selectionDate = GTD(row);
                                        DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, selectionDate);
                                        if (IsWeekend(date))
                                        {
                                            DateTime nextWorkingDay = GetNextWorkingDay(date);
                                            selectionDate = nextWorkingDay.Day;
                                        }
                                        IWebElement element = driver.FindElement(By.XPath(dict["GtdCalendarDate"].ToString()));
                                        var allElements = element.FindElements(By.XPath(".//*"));
                                        var columnHeaderElements = allElements.Where(e => e.GetAttribute("type") == "button").ToList();
                                        bool currentMonth = false;
                                        for (int j = 0; j < columnHeaderElements.Count; j++)
                                        {
                                            if (!currentMonth)
                                            {
                                                while (!columnHeaderElements[j].Text.ToString().Equals("1"))
                                                {
                                                    j++;
                                                }
                                                currentMonth = true;
                                            }
                                            if (columnHeaderElements[j].Text.Equals(selectionDate.ToString()))
                                            {
                                                columnHeaderElements[j].Click();
                                                break;
                                            }
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    try
                                    {
                                        try
                                        {
                                            string a = dict[row[table.Rows[i]["Steps"].ToString()].ToString()].ToString();
                                            string textvalue = driver.FindElement(By.XPath(a)).Text;
                                            var dict1 = GetDict(SamsaraMappingTables[moduleName], 2);
                                            if (textvalue.ToLower() == row[table.Rows[i]["Steps"].ToString()].ToString().ToLower())

                                                PerformActions(driver, table.Rows[i]["Action"].ToString(), dict[row[table.Rows[i]["Steps"].ToString()].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                                            else
                                            {
                                                dict1 = GetDict(SamsaraMappingTables[moduleName], 2);

                                                PerformActions(driver, table.Rows[i]["Action"].ToString(), dict1[row[table.Rows[i]["Steps"].ToString()].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                                            }
                                        }

                                        catch
                                        {

                                            var dict1 = GetDict(SamsaraMappingTables[moduleName], 2);
                                            dict1 = GetDict(SamsaraMappingTables[moduleName], 2);

                                            PerformActions(driver, table.Rows[i]["Action"].ToString(), dict1[row[table.Rows[i]["Steps"].ToString()].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());


                                        }
                                    }
                                    catch
                                    {
                                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                                    }

                                }
                            }
                        }

                        else if (table.Rows[i]["Steps"].ToString().Contains("Button"))
                        {
                            if (table.Rows[i]["Steps"].ToString().Contains("SendButton"))
                            {
                                IWebElement brokerconnection = driver.FindElement(By.XPath(@"//[@id='root']/div[1]/div/main/div[2]/div/section[2]/div/div[3]/div/div[2]/div/div[2]//"));
                                string buttonColor = brokerconnection.GetCssValue("color");
                                if (buttonColor == "rgba(134, 221, 155, 1)")
                                {
                                    brokerconnection.Click();
                                }
                                else
                                {
                                    _Res.IsPassed = false;
                                }
                            }
                            else
                            {
                                CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun);
                                driver.FindElement(By.XPath(dict[table.Rows[i]["Steps"].ToString()].ToString())).Click();
                            }
                        }
                        else
                        {
                            try
                            {
                                string title = driver.Title;
                                string a = table.Rows[i]["Action"].ToString();
                                string b = dict[table.Rows[i]["Steps"].ToString()].ToString();
                                PerformActions(driver, table.Rows[i]["Action"].ToString(), dict[table.Rows[i]["Steps"].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                                if (table.Rows[i]["Steps"].ToString().Equals("TIF") && row["TIF"].ToString().Equals("Good Till Date") && !a.Equals("Click"))
                                {
                                    Wait(1000);
                                    int selectionDate = 0;
                                    try
                                    {
                                        driver.FindElement(By.XPath(dict["GtdCalendar"].ToString()));
                                    }
                                    catch
                                    {
                                        driver.FindElement(By.XPath(dict["GtdCalendarButton"].ToString())).Click();
                                    }
                                    selectionDate = GTD(row);
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, selectionDate);
                                    if (IsWeekend(date))
                                    {
                                        DateTime nextWorkingDay = GetNextWorkingDay(date);
                                        selectionDate = nextWorkingDay.Day;
                                    }
                                    IWebElement element = driver.FindElement(By.XPath(dict["GtdCalendarDate"].ToString()));
                                    var allElements = element.FindElements(By.XPath(".//*"));
                                    var columnHeaderElements = allElements.Where(e => e.GetAttribute("type") == "button").ToList();
                                    bool currentMonth = false;
                                    for (int j = 0; j < columnHeaderElements.Count; j++)
                                    {
                                        if (!currentMonth)
                                        {
                                            while (!columnHeaderElements[j].Text.ToString().Equals("1"))
                                            {
                                                j++;
                                            }
                                            currentMonth = true;
                                        }
                                        if (columnHeaderElements[j].Text.Equals(selectionDate.ToString()))
                                        {
                                            columnHeaderElements[j].Click();
                                            break;
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    try
                                    {
                                        string a = dict[row[table.Rows[i]["Steps"].ToString()].ToString()].ToString();
                                        string textvalue = driver.FindElement(By.XPath(a)).Text;
                                        var dict1 = GetDict(SamsaraMappingTables[moduleName], 2);
                                        if (textvalue.ToLower() == row[table.Rows[i]["Steps"].ToString()].ToString().ToLower())

                                            PerformActions(driver, table.Rows[i]["Action"].ToString(), dict[row[table.Rows[i]["Steps"].ToString()].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                                        else
                                        {
                                            dict1 = GetDict(SamsaraMappingTables[moduleName], 2);

                                            PerformActions(driver, table.Rows[i]["Action"].ToString(), dict1[row[table.Rows[i]["Steps"].ToString()].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                                        }
                                    }

                                    catch
                                    {

                                        var dict1 = GetDict(SamsaraMappingTables[moduleName], 2);
                                        dict1 = GetDict(SamsaraMappingTables[moduleName], 2);

                                        PerformActions(driver, table.Rows[i]["Action"].ToString(), dict1[row[table.Rows[i]["Steps"].ToString()].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());


                                    }
                                }
                                catch
                                {
                                    CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                                }

                            }
                        }
                    }


                }
                else if (table.Rows[i]["Steps"].ToString() == "SwitchWindow")
                {
                    if (ConfigurationManager.AppSettings["ACCenvironment"].ToString().Equals("true") && moduleName.Equals("TradingTicket"))
                    {
                        bool flag = SwitchWindow.SwitchToWindow(driver, "- Trading Ticket");
                        if(!flag)
                            SwitchWindow.SwitchToWindow(driver, table.Rows[i]["Action"].ToString());
                    }
                    else
                    {
                        if (table.Rows[i]["Action"].ToString().Contains("Trading Ticket")) 
                        {
                            bool openWindow = IsWindowOpen("- Trading Ticket");
                            if (openWindow)
                            {
                                SwitchWindow.SwitchToWindow(driver, "- Trading Ticket");
                                continue;
                            }
							Wait(4000);
                        }
                        SwitchWindow.SwitchToWindow(driver, table.Rows[i]["Action"].ToString());
                    }
                }

                else if (table.Rows[i]["Steps"].ToString() == "Wait")
                {
                    int time = 3;
                    try
                    {
                        time = Convert.ToInt32(table.Rows[i]["Time(In Seconds)"]);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //Task.Delay(time);
                    Wait(time * 1000);

                }
                else if (table.Rows[i]["Steps"].ToString() == "GetDictData")
                {
                    dict = GetDict(SamsaraMappingTables[table.Rows[i]["Action"].ToString()]);
                }
                else if (table.Rows[i]["Steps"].ToString() == "CloseWindow")
                {
                    // SwitchWindow.SwitchToWindow(driver, "Nirvana");
                    // driver.Close();
                    if (ConfigurationManager.AppSettings["OpenSpecifiedWindow"].ToString().Split(',').Contains(StepName))
                    {
                        string[] arr = TestCaseSheet_Temp.Tables[0].Rows[0]["OpenWindow"].ToString().Split(',');
                        table.Rows[i]["Action"] = arr[0];
                    }
                    CloseWindow(driver, table.Rows[i]["Action"].ToString(), dict["CloseModule"].ToString());
                }
                else if (table.Rows[i]["Steps"].ToString().Contains("DeleteOldFile"))
                {
                    try
                    {
                        string fileName = table.Rows[i]["Action"].ToString();
                        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                        string filePath2 = SearchFile(downloadsPath, fileName);
                        if (!string.IsNullOrEmpty(filePath2))
                        {
                            DeleteFile(filePath2);
                            Console.WriteLine("File " + fileName + " found and deleted successfully.");

                        }
                        else
                        {
                            Console.WriteLine("File " + fileName + " not found in the Downloads directory.");
                        }

                    }
                    catch
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun);
                    }


                }

                else if (table.Rows[i]["Steps"].ToString().Contains("UpdateRowIndex"))
                {
                    try
                    {
                        RowIndex = int.Parse(table.Rows[i]["Action"].ToString());
                    }
                    catch { }
                }

                else if (table.Rows[i]["Steps"].ToString().Equals("VerifyAllocationStripDetails"))
                {
                    IWebElement element = driver.FindElement(By.ClassName(dict["ClassOfAllocationStripInBlotter"].ToString()));
                    IList<IWebElement> divElements = element.FindElements(By.TagName("div"));
                    foreach (var divElement in divElements)
                    {
                        try
                        {

                            string key = divElement.FindElement(By.ClassName("info-label")).Text;
                            string value = divElement.FindElement(By.ClassName("info-value")).Text;
                            value = RemoveTrailingZeroesFromString(value);
                            string verifyValue = columnValues[key];
                            if (!String.IsNullOrEmpty(verifyValue))
                            {
                                if (verifyValue.Equals(value))
                                {
                                    Console.WriteLine(key + " has " + value + " value");
                                }
                                else
                                {
                                    CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                                    throw new Exception(key + " has this value " + value + " but Excel has" + verifyValue);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message);
                            throw;
                        }

                    }
                }

                else if (table.Rows[i]["Steps"].ToString().Contains("VerifyExportedData"))
                {
                    try
                    {
                        VerifyExportedData(table.Rows[i]["Action"].ToString(), ref UIData);
                    }
                    catch
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun);
                    }
                }


                else if (table.Rows[i]["Steps"].ToString().Contains("RemoveColumnsFromDataSheet"))
                {


                    ExcelData = DataUtilities.RemoveColumnsAndRows(table.Rows[i]["Action"].ToString(), TestCaseSheet.Tables[0]);

                }


                else if (table.Rows[i]["Steps"].ToString() == "DefaultCellValue")
                {
                    try
                    {
                        DefaultCellValue = table.Rows[i]["Action"].ToString();
                        try
                        {
                            if (DefaultCellValue.Equals("#cellid0r"))
                            {
                                var element = driver.FindElement(By.Id(dict["OrderGridID"].ToString()));
                                var allElements = element.FindElements(By.XPath(".//*"));
                                var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                                foreach (var elements in columnHeaderElements)
                                {
                                    string elementId = elements.GetAttribute("id");
                                    DefaultCellValue = "#" + elementId.Substring(0, elementId.IndexOf("r") + 1);
                                    break;
                                }
                            }
                            else if (DefaultCellValue.Equals("#cellid1r"))
                            {
                                var element = driver.FindElement(By.Id(dict["subOrderGridID"].ToString()));
                                var allElements = element.FindElements(By.XPath(".//*"));
                                var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                                foreach (var elements in columnHeaderElements)
                                {
                                    string elementId = elements.GetAttribute("id");
                                    DefaultCellValue = "#" + elementId.Substring(0, elementId.IndexOf("r") + 1);
                                    break;
                                }
                            }
                            else if (DefaultCellValue.Equals("#cellid2r"))
                            {
                                var element = driver.FindElement(By.Id(dict["WorkingGridID"].ToString()));
                                var allElements = element.FindElements(By.XPath(".//*"));
                                var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                                foreach (var elements in columnHeaderElements)
                                {
                                    string elementId = elements.GetAttribute("id");
                                    DefaultCellValue = "#" + elementId.Substring(0, elementId.IndexOf("r") + 1);
                                    break;
                                }
                            }
                            else
                            {
                                var allElements = driver.FindElements(By.XPath(".//*"));
                                var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                                foreach (var elements in columnHeaderElements)
                                {
                                    string elementId = elements.GetAttribute("id");
                                    DefaultCellValue = "#" + elementId.Substring(0, elementId.IndexOf("r") + 1);
                                    break;
                                }
                            }
                        }
                        catch
                        {
                            var allElements = driver.FindElements(By.XPath(".//*"));
                            var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                            foreach (var elements in columnHeaderElements)
                            {
                                string elementId = elements.GetAttribute("id");
                                DefaultCellValue = "#" + elementId.Substring(0, elementId.IndexOf("r") + 1);
                                break;
                            }
                        }
                    }
                    catch { }
                }

                else if (table.Rows[i]["Action"].ToString().Contains("RightClick"))
                {
                    try
                    {
                        if (RowIndex >= 0)
                        {
                            PerformActionsVerification(driver, table.Rows[i]["Action"].ToString(), dict["OrderGridAGgridID"].ToString(), "");
                        }
                    }
                    catch
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                    }
                }

                else if (table.Rows[i]["Steps"].ToString().Contains("ExecuteSQLQuery"))
                {
                    try
                    {
                        DataTable dtDuplicate = null;
                        string action = string.Empty;
                        string variableValue;
                        bool isCloneReq = false;

                        if (!string.IsNullOrEmpty(table.Rows[i]["Action"].ToString()))
                        {
                            int indexOfDash = table.Rows[i]["Action"].ToString().IndexOf("-");
                            if (indexOfDash > 0)
                            {
                                action = table.Rows[i]["Action"].ToString().Substring(0, indexOfDash);
                                isCloneReq = true;
                            }
                            else
                                action = table.Rows[i]["Action"].ToString();


                            if (!string.IsNullOrEmpty(table.Rows[i]["Variable Name"].ToString()) && !string.IsNullOrEmpty(table.Rows[i]["VariableValue"].ToString()))
                            {
                                string variableName = table.Rows[i]["Variable Name"].ToString();
                                variableValue = table.Rows[i]["VariableValue"].ToString();

                                if (TestCaseSheet.Tables[0].Columns.Contains(table.Rows[i]["Variable Name"].ToString()))
                                {
                                    if (isCloneReq)
                                    {
                                        dtDuplicate = TestCaseSheet.Tables[0].Copy();
                                        foreach (DataRow dr in dtDuplicate.Rows)
                                        {
                                            if (!string.IsNullOrEmpty(dr[variableName].ToString()))
                                            {
                                                dr[variableName] = variableValue;

                                            }

                                        }
                                    }
                                }
                            }
                            try
                            {

                                if (string.Equals(action, "SetCompliancePermission", StringComparison.OrdinalIgnoreCase))
                                {

                                    SamsaraSQLQueryManager.SetCompliancePermission(TestCaseSheet.Tables[0]);
                                    if (isCloneReq)
                                    {
                                        SamsaraSQLQueryManager.SetCompliancePermission(dtDuplicate);
                                    }
                                }

                                //   SQLQueriesConstants.SetCompliancePermission(testData.Tables[sheetIndexToName[0]]);
                                /// SQLQueriesConstants.SetCompliancePermission(testData.Tables[sheetIndexToName[0]]);
                            }
                            catch (Exception ex)
                            {
                                CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun);
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun);
                        Console.WriteLine(ex.Message);
                    }
                }

                else if (table.Rows[i]["Steps"].ToString().Contains("ClickMenuItemByText"))
                {
                    ClickMenuItemByText(driver, table.Rows[i]["Action"].ToString(), 6);
                }
                else if (table.Rows[i]["Steps"].ToString().Contains("GetIndexAndClick"))
                {
                    try
                    {
                        Thread.Sleep(6000);
                        string Action = table.Rows[i]["Action"].ToString();
                        DataRow newRow = SamsaraGridOperationHelper.DeleteNonMatchingColumnsValues(row, UIData);
                        DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(UIData), newRow);
                        RowIndex = UIData.Rows.IndexOf(dtRow);

                        /*RowIndex = UIData.Rows.IndexOf(newRow) + 2;*/
                        //string id = table.Rows[i]["Action"].ToString() + RowIndex + "c1";
                        string id = string.Format(dict["WorkingGridAGgridID"].ToString(), RowIndex, 2); 
                        
                        Actions actions = new Actions(driver);
                        IWebElement divElement = driver.FindElement(By.XPath(id));
                        //IList<IWebElement> spanElements = divElement.FindElements(By.TagName("span"));
                        actions.ContextClick(divElement)
                                   .Build()
                                   .Perform();

                        //RowIndex = 1;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                else if (table.Rows[i]["Steps"].ToString().Contains("DeleteAllExistingRowFromGridByRightClick"))
                {
                    try
                    {
                        int tempRowCount = SamsaraGridOperationHelper.CheckRowCountOnUIGrid(driver, DefaultCellValue);

                        for (int k = 0; k < tempRowCount; k++)
                        {
                            RowIndex = 2;
                            PerformActionsVerification(driver, "RightClick", "", "");
                            Thread.Sleep(3000);

                            ClickMenuItemByText(driver, table.Rows[i]["Action"].ToString(), 6);

                        }
                    }

                    catch (Exception ex)
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                    }
                    RowIndex = 1;
                }
                else if (table.Rows[i]["Steps"].ToString() == "VerifyTab")
                {
                    int str = 1;
                    string path = null;
                    bool flag;
                    while (true)
                    {
                        IWebElement divElement = null;
                        try
                        {
                            path = dict["CustomTabPath"].ToString().Replace("str", str.ToString());

                            divElement = driver.FindElement(By.XPath(path));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Tab not found");
                            flag = false;
                            break;
                        }
                        if (divElement.Text == columnValues["TabName"])
                        {
                            Console.WriteLine("Tab founded");
                            flag = true;
                            break;
                        }
                        str++;

                    }

                    if (flag && columnValues["Tab Validity"].ToUpper().Equals("TRUE"))
                    {
                        driver.FindElement(By.XPath(path)).Click();
                    }

                    if (!String.IsNullOrEmpty(columnValues["Action"]))
                    {
                        driver.FindElement(By.XPath(dict["CustomTabAction"].ToString().Replace("str", str.ToString()))).Click();

                        string xpath = dict[columnValues["Action"]].ToString().Replace("str", str.ToString());
                        if (columnValues["Action"].Equals("Rename"))
                        {
                            PerformActions(driver, "Click", xpath, row, table.Rows[i]["Steps"].ToString());
                            if (columnValues["ApprovalForAction"].ToUpper().Equals("TRUE"))
                            {
                                PerformActions(driver, "SendKeys", dict["TabName"].ToString(), row, "CustomTabName");
                                PerformActions(driver, "Click", dict["TabSelect"].ToString(), row, table.Rows[i]["Steps"].ToString());
                            }
                            else if (columnValues["ApprovalForAction"].ToUpper().Equals("FALSE"))
                            {
                                PerformActions(driver, "Click", dict["CloseTabRenameBox"].ToString(), row, table.Rows[i]["Steps"].ToString());
                            }
                        }
                        else if (columnValues["Action"].Equals("Remove") && columnValues["ApprovalForAction"].ToUpper().Equals("TRUE"))
                        {
                            PerformActions(driver, "Click", xpath, row, table.Rows[i]["Steps"].ToString());
                        }
                    }


                    columnValues.Clear();

                }
                else if (table.Rows[i]["Steps"].ToString().Equals("OpenCustomTab"))
                {
                    if (TestCaseSheet_Temp.Tables[0].Columns.Contains("TabName"))
                    {
                        int str = 1;
                        string path = null;
                        while (true)
                        {
                            IWebElement divElement = null;
                            try
                            {
                                path = dict["CustomTabPath"].ToString().Replace("str", str.ToString());

                                divElement = driver.FindElement(By.XPath(path));
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Tab not found");
                                break;
                            }
                            if (divElement.Text == columnValues["TabName"])
                            {
                                if (!TestCaseSheet_Temp.Tables[0].Columns.Contains("tab index"))
                                {
                                    TestCaseSheet_Temp.Tables[0].Columns.Add("tab index");
                                    TestCaseSheet_Temp.Tables[0].Rows[0]["tab index"] = str;
                                }
                                Console.WriteLine("Tab founded");
                                driver.FindElement(By.XPath(path)).Click();
                                break;
                            }
                            str++;

                        }
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("RightClickOnBlankGrid"))
                {
                    IReadOnlyCollection<IWebElement> allGrids = driver.FindElements(By.ClassName("custom-grid"));

                    List<IWebElement> visibleGrids = allGrids
                        .Where(grid => grid.Displayed && grid.Enabled)
                        .ToList();
                    IWebElement gridtoact = visibleGrids[0];
                    if (columnValues["GridType"].ToString().ToLower().Contains("sub"))
                    {
                        gridtoact = visibleGrids[1];
                    }
                    Actions action = new Actions(driver);
                    action.ContextClick(gridtoact).Perform();
                    Wait(3000);
                    List<string> str = SamsaraGridOperationHelper.GetAllRightClick(driver);
                    if (!string.IsNullOrEmpty(columnValues["VisibleActionsToVerify"]))
                    {
                        if (str.Contains(columnValues["VisibleActionsToVerify"].ToString()))
                        {
                            Console.WriteLine(columnValues["VisibleActionsToVerify"].ToString() + " action found on grid " + columnValues["TabName"].ToString());
                        }
                        else
                        {
                            throw new Exception(columnValues["VisibleActionsToVerify"].ToString() + " action not found on grid " + columnValues["TabName"].ToString());
                        }
                    }

                    if (!string.IsNullOrEmpty(columnValues["NotVisibleActionToVerify"]))
                    {
                        if (!str.Contains(columnValues["NotVisibleActionToVerify"].ToString()))
                        {
                            Console.WriteLine(columnValues["NotVisibleActionToVerify"].ToString() + " action not found on grid " + columnValues["TabName"].ToString());
                        }
                        else
                        {
                            throw new Exception(columnValues["NotVisibleActionToVerify"].ToString() + " action found on grid " + columnValues["TabName"].ToString());
                        }
                    }


                    columnValues.Clear();
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("ApplyFilter"))
                {
                    if (!TestCaseSheet_Temp.Tables[0].Columns.Contains("TabName") && !columnValues.Keys.Contains("TabName"))
                    {
                        if (columnValues["GridType"].ToString().Equals("Working"))
                        {
                            driver.FindElement(By.XPath(dict["WorkingTab"].ToString())).Click();
                        }
                        if (columnValues["Action"].Equals("Apply"))
                        {
                            try
                            {
                                string item = columnValues["GridType"] + "_" + columnValues["ColmnFilter"];
                                driver.FindElement(By.XPath(dict[item].ToString())).Click();
                                driver.FindElement(By.XPath(dict["SelectAll_Filter"].ToString())).Click();
                                PerformActions(driver, "SendKeys", dict["Filter_InputBox"].ToString(), row, "Values");
                                driver.FindElement(By.XPath(dict["Filter_CheckBox"].ToString())).Click();
                                driver.FindElement(By.XPath(dict["Filter_ApplyButton"].ToString())).Click();
                            }
                            catch { }

                        }
                        else
                        {
                            Actions actions = new Actions(driver);
                            IWebElement element = null;

                            if (!columnValues["GridType"].ToString().ToLower().Contains("working"))
                            {
                                element = driver.FindElement(By.XPath(dict["OrderGrid"].ToString()));
                            }
                            else
                            {
                                element = driver.FindElement(By.XPath(dict["WorkingSubs"].ToString()));
                            }
                            actions.ContextClick(element).Perform();
                            Wait(2000);
                            PerformActions(driver, "Click", dict["ClearFilter"].ToString(), row, "ClearFilter");
                        }
                    }
                    else
                    {
                        string tab = columnValues["GridType"].ToString();
                        driver.FindElement(By.XPath(dict["DynamicTabForBlotter"].ToString().Replace("str", tab))).Click();
                        var allElements = driver.FindElements(By.XPath(".//*"));
                        bool flag = false;
                        string id = string.Empty;
                        IWebElement targetColumn = null;
                        while (!flag)
                        {
                            var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                            foreach (var col in columnHeaderElements)
                            {
                                if (!string.IsNullOrEmpty(col.Text.ToString()) && col.Text.ToString().ToUpper().Equals(row["ColmnFilter"].ToString().ToUpper()))
                                {
                                    targetColumn = col;
                                    flag = true;
                                    break;
                                }
                                if (!string.IsNullOrEmpty(col.Text.ToString()))
                                {
                                    id = col.GetAttribute("id").ToString();
                                }
                            }
                            if (!flag)
                            {
                                try
                                {
                                    IWebElement elm = null;
                                    try
                                    {
                                        elm = driver.FindElement(By.XPath(dict["SrollerOrder"].ToString()));
                                        elm.Click();
                                    }
                                    catch 
                                    {
                                        elm = driver.FindElement(By.XPath(dict["ScrollerWorking"].ToString()));
                                        elm.Click();
                                    }
                                    Actions actions = new Actions(driver);
                                    actions.MoveToElement(elm).Perform();
                                    actions
                                        .ClickAndHold(elm)
                                        .MoveByOffset(200, 0)
                                        .Release()
                                        .Perform();

                                    Thread.Sleep(2000);
                                }
                                catch (MoveTargetOutOfBoundsException ex) { }  
                            }
                        }
                        targetColumn.FindElement(By.ClassName("ig-filter-icon")).Click();
                        driver.FindElement(By.XPath(dict["SelectAll_Filter"].ToString())).Click();
                        if (columnValues["ColmnFilter"].ToString().Contains("Transaction Time"))
                        {
                            IWebElement ele = driver.FindElement(By.ClassName("ig-data-grid"));
                            var childs = ele.FindElements(By.TagName("span"));
                            IWebElement filter = driver.FindElement(By.ClassName("ig-grid-column-options"));
                            var spans = filter.FindElements(By.TagName("span"));
                            foreach (var span in spans) 
                            {
                                if (span.Text.ToString().Equals(DateTime.Now.ToString("M/d/yyyy"))) 
                                {
                                    span.Click();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            PerformActions(driver, "SendKeys", dict["Filter_InputBox"].ToString(), row, "Values");
                        }
                        driver.FindElement(By.XPath(dict["Filter_ApplyButton"].ToString())).Click();
                            
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("ActionOnPageView"))
                {
                    if (String.IsNullOrEmpty(columnValues["Action"]))
                    {
                        try
                        {
                            PerformActionsVerification(driver, "Click", dict["LaunchPage"].ToString(), "");
                        }
                        catch
                        {
                            CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                        }

                    }
                    else if (columnValues["Action"].ToUpper().Equals("DELETE"))
                    {
                        try
                        {
                            PerformActionsVerification(driver, "Click", dict["DeletePage"].ToString(), "");
                            Console.WriteLine(TestCaseSheet_Temp.Tables[0].Rows[0]["OpenWindow"].ToString() + " page deleted successfully");
                        }
                        catch
                        {
                            CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                        }
                    }
                }

                else if (table.Rows[i]["Steps"].ToString().Equals("VerifyTT"))
                {
                    string value;
                    string[] arr;
                    IWebElement element = driver.FindElement(By.ClassName(dict["L1stripClass"].ToString()));
                    IList<IWebElement> divElements = element.FindElements(By.TagName("p"));
                    foreach (var divElement in divElements)
                    {
                        IList<IWebElement> keyvaluepair = divElement.FindElements(By.TagName("span"));
                        string key = keyvaluepair[0].Text;
                        int idx = keyvaluepair[1].Text.IndexOf(".");
                        string values = idx == -1 ? keyvaluepair[1].Text : keyvaluepair[1].Text.Substring(0, idx);
                        values = values.Replace(",", "");
                        try
                        {
                            if (columnValues.ContainsKey(key))
                            {
                                string verifyValue = columnValues[key];
                                if (!String.IsNullOrEmpty(verifyValue))
                                {
                                    if (values.Equals(verifyValue))
                                    {
                                        Console.WriteLine(key + " has " + verifyValue + " value");
                                    }
                                    else
                                    {
                                        throw new Exception(key + " has this value " + values + " but Excel has " + verifyValue);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message);
                            throw;
                        }

                    }


                    if (TestCaseSheet.Tables[0].Columns.Contains("Symbol") && !String.IsNullOrEmpty(columnValues["Symbol"]))
                    {
                        value = driver.FindElement(By.XPath(dict["Symbol"].ToString())).GetAttribute("Value");

                        if (!columnValues["Symbol"].ToString().Equals(value))
                        {
                            throw new Exception("Excel has " + columnValues["Symbol"].ToString() + " but UI has " + value + " value of Column Symbol");
                        }
                        else
                        {
                            Console.WriteLine(columnValues["Symbol"].ToString() + " symbol is validated");
                        }
                    }


                    if (TestCaseSheet.Tables[0].Columns.Contains("Symbol Name") && !String.IsNullOrEmpty(columnValues["Symbol Name"]))
                    {
                        arr = driver.Title.ToString().Split(' ');
                        if (!columnValues["Symbol Name"].ToString().Equals(arr[0]))
                        {
                            throw new Exception("Excel has " + columnValues["Symbol Name"].ToString() + " but UI has " + arr[0] + " value of Column Symbol Name");
                        }
                        else
                        {
                            Console.WriteLine(columnValues["Symbol Name"].ToString() + " symbol name found");
                        }
                    }

                    if (TestCaseSheet.Tables[0].Columns.Contains("Quantity") && !String.IsNullOrEmpty(columnValues["Quantity"]))
                    {
                        try
                        {
                            value = driver.FindElement(By.XPath(dict["Quantity"].ToString())).GetAttribute("Value");
                        }
                        catch
                        {
                            value = driver.FindElement(By.XPath(dict2["Quantity"].ToString())).GetAttribute("Value");
                        }

                        int idx = value.IndexOf(".");
                        string values = idx == -1 ? value : value.Substring(0, idx);
                        values = values.Replace(",", "");
                        if (!columnValues["Quantity"].ToString().Equals(values))
                        {
                            throw new Exception("Excel has " + columnValues["Quantity"].ToString() + " but UI has " + value + " value of Column Quantity");
                        }
                        else
                        {
                            Console.WriteLine(columnValues["Quantity"].ToString() + " Quantity is verified");
                        }
                    }
                    if (TestCaseSheet.Tables[0].Columns.Contains("VerifyDisableVenue") && !String.IsNullOrEmpty(TestCaseSheet.Tables[0].Rows[0]["VerifyDisableVenue"].ToString()))
                    {
                        IWebElement venue = driver.FindElement(By.XPath(dict["Venue"].ToString()));
                        string disabledAttr = venue.GetAttribute("disabled");

                        bool isDisabled = disabledAttr != null;
                        if (!isDisabled.ToString().ToLower().Equals(TestCaseSheet.Tables[0].Rows[0]["VerifyDisableVenue"].ToString().ToLower()))
                        {
                            throw new Exception("Venue disablity on UI is " + isDisabled.ToString() + " and on excel it is " + TestCaseSheet.Tables[0].Rows[0]["VerifyDisableVenue"].ToString());
                        }
                    }

                    columnValues.Clear();
                }
                else if (table.Rows[i]["Steps"].ToString() == "VerifyOrderSideTT")
                {
                    string orderSides = columnValues["OrderSideToVerify"].ToString();
                    string[] orderSidesArray = orderSides.Split(',');
                    IWebElement element = driver.FindElement(By.ClassName(dict["ClassOfOrderSide"].ToString()));

                    foreach (string orderSide in orderSidesArray)
                    {
                        IList<IWebElement> divElements = element.FindElements(By.TagName("button"));
                        foreach (var ele in divElements)
                        {
                            try
                            {
                                try
                                {
                                    string ordersidename = ele.FindElement(By.TagName("span")).Text;
                                    if (ordersidename.Equals(orderSide))
                                    {
                                        Console.WriteLine(orderSide + " found");

                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("{\"method\":\"tag name\",\"selector\":\"span\"}"))
                                {
                                    continue;
                                }
                                else
                                {
                                    throw ex;
                                }
                            }
                        }
                    }

                }
                else if (table.Rows[i]["Steps"].ToString().Equals("UpdateShortLocateFromTT"))
                {

                    try
                    {
                        IWebElement element = driver.FindElement(By.ClassName(dict["ShorLocateGridClass"].ToString()));
                        string maxRow = element.GetAttribute("aria-rowcount");
                        string id = string.Empty;
                        var allElements = element.FindElements(By.XPath(".//*"));
                        var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                        foreach (var elements in columnHeaderElements)
                        {
                            string elementId = elements.GetAttribute("id");
                            id = elementId.Substring(0, elementId.IndexOf("r") + 1);
                            break;
                        }
                        if (TestCaseSheet_Temp.Tables[0].Columns.Contains("RowNoforEdit") && !string.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["RowNoforEdit"].ToString()))
                        {
                            maxRow = (int.Parse(TestCaseSheet_Temp.Tables[0].Rows[0]["RowNoforEdit"].ToString()) + 1).ToString();
                        }
                        int column = 0;
                        while (column < 5)
                        {
                            Wait(1000);
                            PerformActionsThroughID(driver, "Click", id + maxRow + "c" + (column + 1), row, TestCaseSheet_Temp.Tables[0].Columns[column].ToString());
                            PerformActionsThroughID(driver, "Click", id + maxRow + "c" + (column + 1), row, TestCaseSheet_Temp.Tables[0].Columns[column].ToString());
                            Wait(2000);
                            PerformActionsThroughID(driver, "SendKeysWithoutClear", id + maxRow + "c" + (column + 1), row, TestCaseSheet_Temp.Tables[0].Columns[column].ToString());
                            column++;
                        }

                        if (columnValues["Send"].ToUpper().Equals("TRUE"))
                        {
                            driver.FindElement(By.ClassName(dict["SendShortLocate"].ToString())).Click();
                            if (columnValues["SAlessQTYPopup"].ToUpper().Equals("TRUE"))
                            {
                                driver.FindElement(By.XPath(dict["ShortLocateLessQtyPopup_Yes"].ToString())).Click();
                            }
                            else if (columnValues["SAlessQTYPopup"].ToUpper().Equals("FALSE"))
                            {
                                driver.FindElement(By.XPath(dict["ShortLocateLessQtyPopup_No"].ToString())).Click();
                            }
                            if (columnValues["TradeType"].ToUpper().Equals("MANUAL"))
                            {
                                if (columnValues["TargetQuantity"] != null)
                                {
                                    PerformActions(driver, "Click", dict["ManualPopup_TextBox"].ToString(), row, "TargetQuantity");
                                    Wait(2000);
                                    PerformActions(driver, "SendKeys", dict["ManualPopup_TextBox"].ToString(), row, "TargetQuantity");
                                }
                                Wait(2000);
                                driver.FindElement(By.XPath(dict["ManualPopup_Continue"].ToString())).Click();
                            }
                            if (TestCaseSheet_Temp.Tables[0].Columns.Contains("AllowTrade") && !string.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["AllowTrade"].ToString()))
                            {
                                if (columnValues["AllowTrade"].ToUpper().Equals("TRUE"))
                                {
                                    driver.FindElement(By.XPath(dict["AllowTrade_Proceed"].ToString())).Click();
                                }
                                else if (columnValues["AllowTrade"].ToUpper().Equals("FALSE"))
                                {
                                    driver.FindElement(By.XPath(dict["AllowTrade_Cancel"].ToString())).Click();
                                }
                            }
                        }
                        else
                        {
                            Actions action = new Actions(driver);
                            action.SendKeys(Keys.Escape).Perform();
                        }

                        columnValues.Clear();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                else if (table.Rows[i]["Steps"].ToString().Equals("OpenPTT"))
                {
                    if (columnValues["Type"].ToUpper().Equals("PERCENTAGE") || string.IsNullOrEmpty(columnValues["Type"]))
                    {
                        row["Type"] = "%";
                    }
                    else if (columnValues["Type"].ToUpper().Equals("AMT") || columnValues["Type"].Equals("$ Amount"))
                    {
                        row["Type"] = "Amt";
                    }
                    else
                    {
                        row["Type"] = "BPS";
                    }
                    PerformActionsThroughID(driver, "SendKeys", dict["TradeBy"].ToString(), row, "Type");
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("AccountOrMFforPST"))
                {
                    try
                    {
                        if (row["MasterFund/Account"].ToString().ToUpper().Equals("CUSTOM GROUP"))
                        {
                            PerformActions(driver, "Click", dict["AccountPanel"].ToString(), row, "AccountPanel");
                            Wait(2000);
                            PerformActions(driver, "Click", dict["Custom Group"].ToString(), row, "Custom Group");
                            if (!string.IsNullOrEmpty(row["Accounts"].ToString()))
                            {
                                foreach (string account in row["Accounts"].ToString().Split(','))
                                {
                                    SamsaraGridOperationHelper.SetAccoutOrMFForPST(driver, dict["AccountGrid"].ToString(), account, dict["AccountScroller"].ToString());
                                }
                            }
                            PerformActions(driver, "Click", dict["AccountPanel"].ToString(), row, "AccountPanel");

                        }
                        else if (row["MasterFund/Account"].ToString().ToUpper().Equals("ALLOCATION PREFERENCE"))
                        {
                            PerformActions(driver, "Click", dict["AccountPanel"].ToString(), row, "AccountPanel");
                            Wait(2000);
                            PerformActions(driver, "Click", dict["Allocation Preference"].ToString(), row, "Allocation Preference");
                            if (!string.IsNullOrEmpty(row["Accounts"].ToString()))
                            {
                                foreach (string account in row["Accounts"].ToString().Split(','))
                                {
                                    SamsaraGridOperationHelper.SetAccoutOrMFForPST(driver, dict["AccountGrid"].ToString(), account, dict["AccountScroller"].ToString());
                                }
                            }
                            PerformActions(driver, "Click", dict["AccountPanel"].ToString(), row, "AccountPanel");

                        }
                        else if ((TestCaseSheet.Tables[0].Columns.Contains("Master Funds") && !string.IsNullOrEmpty(row["Master Funds"].ToString())) || (!string.IsNullOrEmpty(row["Accounts"].ToString())))
                        {
                            PerformActions(driver, "Click", dict["AccountPanel"].ToString(), row, "AccountPanel");
                            Wait(2000);
                            if (string.IsNullOrEmpty(row["MasterFund/Account"].ToString()) || row["MasterFund/Account"].ToString().Equals("Account"))
                            {
                                PerformActions(driver, "Click", dict["Accounts"].ToString(), row, "Accounts");
                                Wait(2000);
                                if (!string.IsNullOrEmpty(row["Accounts"].ToString()))
                                {
                                    foreach (string account in row["Accounts"].ToString().Split(','))
                                    {
                                        SamsaraGridOperationHelper.SetAccoutOrMFForPST(driver, dict["AccountGrid"].ToString(), account, dict["AccountScroller"].ToString());
                                    }
                                }
                            }
                            else
                            {
                                PerformActions(driver, "Click", dict["MasterFund"].ToString(), row, "MasterFund");
                                Wait(2000);
                                if (!string.IsNullOrEmpty(row["Accounts"].ToString()))
                                {
                                    foreach (string account in row["Accounts"].ToString().Split(','))
                                    {
                                        SamsaraGridOperationHelper.SetAccoutOrMFForPST(driver, dict["AccountGrid"].ToString(), account, dict["AccountScroller"].ToString());
                                    }
                                }
                                else
                                {
                                    foreach (string mf in row["Master Funds"].ToString().Split(','))
                                    {
                                        SamsaraGridOperationHelper.SetAccoutOrMFForPST(driver, dict["MFGrid"].ToString(), mf, dict["MFscroller"].ToString());
                                    }
                                }
                            }
                            PerformActions(driver, "Click", dict["AccountPanel"].ToString(), row, "AccountPanel");
                        }
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("ActionOnPrompt"))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(columnValues["VerifyContainslblMsg"].ToString()))
                        {
                            IWebElement bodyElement = driver.FindElement(By.XPath(dict["NewTradePopup"].ToString()));
                            string newText = Regex.Replace(bodyElement.Text.ToLower(), @"[\s,]+()", "");
                            string str = Regex.Replace(columnValues["VerifyContainslblMsg"].ToString().ToLower(), @"[\s,]+()", "");
                            string srt = bodyElement.Text.ToLower().ToString();
                            double similarity = CalculateSimilarity(srt, columnValues["VerifyContainslblMsg"].ToLower());
                            if (similarity >= 0.50 || newText.ToLower().Contains(str.ToLower()))
                            {
                                Console.WriteLine("Prompt message verified");
                            }
                            else
                            {
                                throw new Exception("Prompt message is not Verifying");
                            }
                        }
                        if (!string.IsNullOrEmpty(columnValues["TargetQuantity"].ToString()))
                        {
                            PerformActions(driver, "Click", dict["TargetQuantityIn"].ToString(), row, "TargetQuantity");
                            PerformActions(driver, "FillValues", dict["TargetQuantityIn"].ToString(), row, "TargetQuantity");
                        }
                        if (!string.IsNullOrEmpty(columnValues["Continue"].ToString()))
                        {
                            PerformActions(driver, "Click", dict["ContinueButton_Replace"].ToString(), row, "Continue");
                        }
                        if (!string.IsNullOrEmpty(columnValues["Edit Order"].ToString()))
                        {
                            PerformActions(driver, "Click", dict["Edit Order"].ToString(), row, "Edit Order");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex + " occured while verifying Prompt window");
                    }
                    columnValues.Clear();
                }
                else if (table.Rows[i]["Steps"].ToString().Equals("WindowAction"))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(columnValues["PreviousWindow"].ToString()))
                        {
                            if (columnValues["PreviousWindow"].ToString().Equals("TradingTicket"))
                            {
                                columnValues["PreviousWindow"] = "Trading Ticket";
                            }
                            if (columnValues["PreviousWindow"].ToString().Equals("BlotterMain"))
                            {
                                columnValues["PreviousWindow"] = "Blotter";
                            }
                            SwitchWindow.SwitchToParentWindow(driver, columnValues["PreviousWindow"].ToString(), dict["CloseModule"].ToString());
                            if (!string.IsNullOrEmpty(columnValues["MaximizeWindow"].ToString()))
                            {
                                PerformActions(driver, "Click", dict["MaximizeWindow"].ToString(), row, "MaximizeWindow");
                            }
                            if (!string.IsNullOrEmpty(columnValues["CloseWindow"].ToString()))
                            {
                                PerformActions(driver, "Click", dict["CloseModule"].ToString(), row, "CloseWindow");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    columnValues.Clear();
                }
                else
                {
                    try
                    {
                        string tt = driver.Title;
                        if ((table.Rows[i]["Action"].ToString().Equals("SendKeys") || table.Rows[i]["Action"].ToString().Equals("DropDown")) && !row.Table.Columns.Contains(table.Rows[i]["Steps"].ToString()))
                        {
                            continue;
                        }
                        PerformActions(driver, table.Rows[i]["Action"].ToString(), dict[table.Rows[i]["Steps"].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                    }
                    catch
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + StepName);
                    }

                }
                if(ConfigurationManager.AppSettings["ToastVerify"].ToString() == "true")
                {
                    try
                    {
                        IWebElement toast = driver.FindElement(By.ClassName("Toastify"));
                        if (!string.IsNullOrEmpty(toast.Text.ToString()))
                        {
                            toastList.Add(toast.Text.ToString());
                        }
                    }catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (table.Rows[i]["Time(In Seconds)"] != null && table.Rows[i]["Time(In Seconds)"].ToString() != "")
                {
                    Wait(Convert.ToInt32(table.Rows[i]["Time(In Seconds)"]) * 1000);
                }


            }

            if (StepName.Equals("CustomOrderTT"))
            {
                if (columnValues.Keys.Contains("TT_Open") && !columnValues["TT_Open"].ToString().ToUpper().Equals("TRUE"))
                {
                    CloseWindow(driver, "Trading Ticket", dict["CloseModule"].ToString());
                }
                columnValues.Clear();
            }
            _Res.IsPassed = true;
            return _Res;

        }

        static DateTime GetNextWorkingDay(DateTime date)
        {
            // Increment the date until it's not a weekend
            do
            {
                date = date.AddDays(1);
            } while (IsWeekend(date));

            return date;
        }

        private static int LevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
                return string.IsNullOrEmpty(t) ? 0 : t.Length;
            if (string.IsNullOrEmpty(t))
                return s.Length;

            var d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++)
                d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++)
                d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(
                        d[i - 1, j] + 1,
                        d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[s.Length, t.Length];
        }
        public static double CalculateSimilarity(string s1, string s2)
        {
            int distance = LevenshteinDistance(s1, s2);
            int maxLen = Math.Max(s1.Length, s2.Length);
            return (1.0 - (double)distance / maxLen) * 100; // Percentage match
        }


        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
        public static void ResetValues()
        {
            rowcount = 0;
        }

        static int rowcount = 0;
        static Dictionary<string, string> columnValues = new Dictionary<string, string>();
        private static bool CallStep(WebDriver driver, string value, int i, DataTable table, DataRow row, DataRow dataRow, Dictionary<string, object> dict, DataSet TestCaseSheet = null)
        {
            if (table.Columns.Contains("ActionThroughId"))
            {
                if (table.Rows[i]["ActionThroughId"].ToString().ToUpper().Equals("TRUE"))
                {
                    string a = table.Rows[i]["Action"].ToString();
                    string b = dict[table.Rows[i]["Steps"].ToString()].ToString();
                    try
                    {
                        PerformActionsThroughID(driver, a, b, dataRow, table.Rows[i]["Steps"].ToString());
                        return true;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(b + " not founded by ID");
                    }


                }
            }
            if (table.Rows[i]["Steps"].ToString() == "SwitchWindow")
            {
                SwitchWindow.SwitchToWindow(driver, table.Rows[i]["Action"].ToString());
                return true;
            }

            else if (table.Rows[i]["Steps"].ToString().Equals("AlgoBroker"))
            {
                string xpath = null;
                string btnpath = null;
                string chkpath = null;
                string btn = "";
                int str = 1;
                string val = ColumnValues.Rows[0]["AlgoType"].ToString().ToUpper();
                if (!table.TableName.Equals("AlgoBrokerForQTT"))
                {
                    btn = ColumnValues.Rows[0]["Order type button"].ToString().ToUpper();
                }
                string chk = ColumnValues.Rows[0]["EndTimeCheckBox"].ToString().ToUpper();
                while (true)
                {
                    try
                    {
                        xpath = dict["AlgoType"].ToString().Replace("str", str.ToString());
                        if (driver.FindElement(By.XPath(xpath)).Text.ToUpper().Equals(val))
                        {
                            driver.FindElement(By.XPath(xpath)).Click();
                            break;
                        }
                        str++;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("AlgoType not found");
                        break;
                    }

                }
                if (!string.IsNullOrEmpty(ColumnValues.Rows[0]["Start time"].ToString()))
                {

                    var today = DateTime.Now;
                    string[] start = ColumnValues.Rows[0]["Start time"].ToString().Split('.');
                    string[] end = ColumnValues.Rows[0]["End Time"].ToString().Split('.');
                    int hour;
                    int min = 00;
                    hour = Int32.Parse(start[0]) + today.Hour;

                    if (start.Length > 1)
                    {
                        min = Int32.Parse(start[1]);
                    }
                    if (hour < 10)
                    {
                        ColumnValues.Rows[0]["Start time"] = "0" + hour + ":" + min;
                    }
                    else
                    {
                        ColumnValues.Rows[0]["Start time"] = hour + ":" + min;
                    }

                    dataRow["Start time"] = ColumnValues.Rows[0]["Start time"];
                    PerformActionsThroughID(driver, "Click", dict["Start time"].ToString(), dataRow, "Start time");
                    PerformActionsThroughID(driver, "SendKeys", dict["Start time"].ToString(), dataRow, "Start time");

                    if (!string.IsNullOrEmpty(end[0]))
                    {
                        hour = Int32.Parse(end[0]) + today.Hour;
                        if (end.Length > 1)
                        {
                            min = Int32.Parse(end[1]);
                        }
                        if (hour < 10)
                        {
                            ColumnValues.Rows[0]["End time"] = "0" + hour + ":" + min;
                        }
                        else
                        {
                            ColumnValues.Rows[0]["End time"] = hour + ":" + min;
                        }
                        dataRow["End time"] = ColumnValues.Rows[0]["End time"];
                        PerformActionsThroughID(driver, "Click", dict["End time"].ToString(), dataRow, "End time");
                        PerformActionsThroughID(driver, "SendKeys", dict["End time"].ToString(), dataRow, "End time");
                    }
                    //for end time check box
                    if (chk.ToString().ToUpper().Equals("TRUE"))
                    {
                        chkpath = dict["AlgoCheckBox"].ToString();
                        driver.FindElement(By.XPath(chkpath)).Click();
                    }
                    if(!table.TableName.Equals("AlgoBrokerForQTT"))
                    {
                        //for send button on algo UI
                        if (btn.ToString().ToUpper().Equals("ALGOSEND"))
                        {
                            btnpath = dict["AlgoSend"].ToString();
                            driver.FindElement(By.XPath(btnpath)).Click();
                        }
                        //for replace button on algo UI
                        if (btn.ToString().ToUpper().Equals("ALGOREPLACE"))
                        {
                            btnpath = dict["AlgoReplace"].ToString();
                            driver.FindElement(By.XPath(btnpath)).Click();
                        }
                    }
                    ColumnValues.Clear();
                }

            }





            else if (table.Rows[i]["Steps"].ToString() == "Wait")
            {
                int time = Convert.ToInt32(table.Rows[i]["Time(In Seconds)"]);
                //Task.Delay(time);
                Wait(time * 1000);
                return true;
            }
            else if (table.Rows[i]["Steps"].ToString() == "CloseWindow")
            {
                // SwitchWindow.SwitchToWindow(driver, "Nirvana");
                //driver.Close();
                CloseWindow(driver, table.Rows[i]["Action"].ToString(), dict["CloseModule"].ToString());
                return true;

            }
            else if (table.Rows[i]["Steps"].ToString() == "FetchColumnValues")
            {
                try
                {
                    ColumnValues.Reset();
                    var columns = table.Rows[i]["Action"].ToString().Split(',');
                    foreach (var column in columns)
                    {
                        ColumnValues.Columns.Add(column);
                    }
                    DataRow dataRow1 = ColumnValues.NewRow();
                    foreach (var column in columns)
                    {
                        dataRow1[column] = dataRow[column].ToString();
                    }
                    ColumnValues.Rows.Add(dataRow1);
                    return true;
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }

            else if (table.Rows[i]["Steps"].ToString() == "AddValues")
            {
                return false;
            }
            else if (table.Rows[i]["Steps"].ToString().StartsWith("Column"))
            {
                try
                {
                    string name = table.Rows[i]["Steps"].ToString().Replace("Column_", "");
                    string xpathName = name + "_" + columnValues[name];

                    PerformActionsVerification(driver, table.Rows[i]["Action"].ToString(), dict[xpathName].ToString(), table.Rows[i]["Steps"].ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + table.TableName);
                }

            }
            else if (table.Rows[i]["Action"].ToString() == "Click")
            {

                //PerformActions(driver, table.Rows[i]["Action"].ToString(), dict[table.Rows[i]["Steps"].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                try
                {
                    string a = table.Rows[i]["Action"].ToString();
                    string b = dict[table.Rows[i]["Steps"].ToString()].ToString();
                    PerformActions(driver, table.Rows[i]["Action"].ToString(), dict[table.Rows[i]["Steps"].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                    return true;
                }
                catch (Exception e)
                {
                    CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun);
                    Console.WriteLine(e);
                }
            }

            else if (table.Rows[i]["Action"].ToString() == "FillSendKeys")
            {
                //Add loop and run by steps which contain , seperated values of columns to add value
                if (ColumnValues.Rows.Count > 0)
                {
                    //for(int j = 0; j<ColumnValues.Rows.Count; j++){
                    Actions actions = new Actions(driver);
                    string xpath = dict[table.Rows[i]["Steps"].ToString()].ToString();
                    try
                    {
                        xpath = xpath.Substring(0, xpath.IndexOf('r') + 1) + (rowcount + 2) + xpath.Substring(xpath.IndexOf('r') + 2);
                        driver.FindElement(By.XPath(xpath));
                    }
                    catch {
                        xpath = xpath.Replace("0", "1");
                    }
                    var ValueToFill = ColumnValues.Rows[0][table.Rows[i]["Steps"].ToString()].ToString().Split(',');
                    IWebElement inputElement = driver.FindElement(By.XPath(xpath));


                    actions.Click(inputElement).Perform();
                    Wait(3000);
                    actions.SendKeys(Keys.Control).SendKeys(Keys.Backspace).SendKeys(Keys.Enter).Click(inputElement).Perform();
                    Wait(2000);
                    actions.SendKeys(ValueToFill[rowcount]).Perform();
                    //driver.FindElement(By.XPath(dict[table.Rows[i]["Steps"].ToString()].ToString())).SendKeys(Keys.Control + Keys.Home);
                    //driver.FindElement(By.XPath(dict[table.Rows[i]["Steps"].ToString()].ToString())).SendKeys(Keys.Control + Keys.Shift + Keys.End);
                    //driver.FindElement(By.XPath(dict[table.Rows[i]["Steps"].ToString()].ToString())).SendKeys(ValueToFill[rowcount] + Keys.Enter);


                    // PerformActions(driver, table.Rows[i]["Action"].ToString(), dict[table.Rows[i]["Steps"].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                }
                rowcount++;
                //}
            }
            else if (table.Rows[i]["Action"].ToString() == "AddSendKeys")
            {
                //Add loop and run by steps which contain , seperated values of columns to add value
                if (ColumnValues.Rows.Count > 0)
                {
                    try
                    {
                        var ValueToFill = ColumnValues.Rows[0][table.Rows[i]["Steps"].ToString()].ToString().Split(',');
                        driver.FindElement(By.XPath(dict[table.Rows[i]["Steps"].ToString()].ToString())).SendKeys(Keys.Control + Keys.Home);
                        driver.FindElement(By.XPath(dict[table.Rows[i]["Steps"].ToString()].ToString())).SendKeys(Keys.Control + Keys.Shift + Keys.End);
                        driver.FindElement(By.XPath(dict[table.Rows[i]["Steps"].ToString()].ToString())).SendKeys(ValueToFill[rowcount] + Keys.Enter);

                        //*[@id="Allocation1undefined"]
                        try
                        {
                            string CheckboxXpath = "//*[@id=\"" + ValueToFill[rowcount] + "undefined\"]";
                            driver.FindElement(By.XPath(CheckboxXpath)).Click();
                        }
                        catch { }
                    }
                    catch
                    {
                        CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, table.Rows[i]["Steps"].ToString() + " at " + table.TableName);
                        throw;
                    }

                }
            }
            else if (table.Rows[i]["Action"].ToString() == "SendKeys")
            {
                driver.FindElement(By.XPath(dict[table.Rows[i]["Steps"].ToString()].ToString())).SendKeys(Keys.Control + Keys.Home);
                driver.FindElement(By.XPath(dict[table.Rows[i]["Steps"].ToString()].ToString())).SendKeys(Keys.Control + Keys.Shift + Keys.End);
                driver.FindElement(By.XPath(dict[table.Rows[i]["Steps"].ToString()].ToString())).SendKeys(row[table.Rows[i]["Steps"].ToString()].ToString() + Keys.Tab);
            }
            else if (table.Rows[i]["Steps"].ToString().Contains("GetColumnValue"))
            {
                try
                {
                    List<string> columns = new List<string>();
                    columns.AddRange(table.Rows[i]["Action"].ToString().Split(','));
                    for (int k = 0; k < columns.Count; k++)
                    {
                        // Wrong code as this key is already present in the list 
                            //columnValues.Add(columns[k], dataRow[columns[k]].ToString());
                        if (!dataRow.Table.Columns.Contains(columns[k]) && !dataRow.Table.TableName.ToLower().Contains("customordertt"))
                            {
                                throw new Exception("Column does not exist in the DataRow.");
                            }
                        columnValues[columns[k]] = dataRow[columns[k]].ToString();

                       
                    }
                }
                catch (Exception ex)
                {
                    if (table.Columns.Contains("ClickOnButton"))
                    {
                        try
                        {
                            PerformActions(driver, "Click", dict[table.Rows[i]["ClickOnButton"].ToString()].ToString(), row, table.Rows[i]["Steps"].ToString());
                            return true;
                        }
                        catch { }
                    }
                    Console.WriteLine(ex.Message);
                }

            }
            else if (table.Rows[i]["Steps"].ToString().Equals("ShortLocate"))
            {
                try
                {
                    IWebElement element = driver.FindElement(By.ClassName(dict["ShorLocateGridClass"].ToString()));
                    string maxRow = element.GetAttribute("aria-rowcount");
                    string id = string.Empty;
                    var allElements = element.FindElements(By.XPath(".//*"));
                    var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                    foreach (var elements in columnHeaderElements)
                    {
                        string elementId = elements.GetAttribute("id");
                        id = elementId.Substring(0, elementId.IndexOf("r") + 1);
                        break;
                    }
                    if (TestCaseSheet_Temp.Tables[0].Columns.Contains("RowNoforEdit") && !string.IsNullOrEmpty(TestCaseSheet_Temp.Tables[0].Rows[0]["RowNoforEdit"].ToString()))
                    {
                        maxRow = (int.Parse(TestCaseSheet_Temp.Tables[0].Rows[0]["RowNoforEdit"].ToString()) + 1).ToString();
                    }
                    int column = 0;
                    while (column < 5) {
                        Wait(1000);
                        PerformActionsThroughID(driver, "Click", id + maxRow + "c" + (column + 1), dataRow, TestCaseSheet_Temp.Tables[0].Columns[column].ToString());
                        PerformActionsThroughID(driver, "Click", id + maxRow + "c" + (column + 1), dataRow, TestCaseSheet_Temp.Tables[0].Columns[column].ToString());
                        Wait(2000);
                        PerformActionsThroughID(driver, "SendKeysWithoutClear", id + maxRow + "c" + (column + 1), dataRow, TestCaseSheet_Temp.Tables[0].Columns[column].ToString());
                        column++;
                    }

                    if (columnValues["Send"].ToUpper().Equals("TRUE")) {
                        driver.FindElement(By.ClassName(dict["SendShortLocate"].ToString())).Click();
                        if (TestCaseSheet_Temp.Tables[0].Columns.Contains("TradeType"))
                        {
                            if (columnValues["TradeType"].ToUpper().Equals("MANUAL"))
                            {
                                Wait(2000);
                                driver.FindElement(By.XPath(dict["ManualPopup_Continue1"].ToString())).Click();
                            }   
                        }
                    }
                    else
                    {
                        Actions action = new Actions(driver);
                        action.SendKeys(Keys.Escape).Perform();
                    }
                    columnValues.Clear();
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            
            }
            

            return false;
        }

        static void PerformUserAction(WebDriver driver, string action, string ButtonXpath, string ButtonXpath1)
        {
            if (action == "MAXIMIZE")
            {
                Console.WriteLine("Performing maximize action...");
                PerformResize(driver, true, ButtonXpath1);
            }
            else if (action == "MINIMIZE")
            {
                Console.WriteLine("Performing minimize action...");
                PerformResize(driver, false, ButtonXpath);
            }
            else
            {
                Console.WriteLine("No action taken.");
            }
        }


        static void EnsureWindowState(WebDriver driver, string ensureResize)
        {
            int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            int windowWidth = Convert.ToInt32(js.ExecuteScript("return window.innerWidth;"));
            int windowHeight = Convert.ToInt32(js.ExecuteScript("return window.innerHeight;"));

            Console.WriteLine("Screen Resolution: " + screenWidth + "x" + screenHeight);
            Console.WriteLine("Browser Window Size: " + windowWidth + "x" + windowHeight);

            int thresholdWidth = (int)(screenWidth * 0.98);
            int thresholdHeight = (int)(screenHeight * 0.92);

            bool isMaximized = windowWidth >= thresholdWidth && windowHeight >= thresholdHeight;
            bool isMinimized = windowWidth < thresholdWidth || windowHeight < thresholdHeight;


            if (ensureResize == "MAXIMIZE" && !isMaximized)
            {
                throw new InvalidOperationException("Window is not maximized as expected.");
            }
            else if (ensureResize == "MINIMIZE" && !isMinimized)
            {
                throw new InvalidOperationException("Window is not minimized as expected.");
            }
            else
            {
                Console.WriteLine(ensureResize == "MAXIMIZE" ? "Window is correctly maximized." : "Window is correctly minimized.");
            }
        }
        static void VerifyPopupAlignment(WebDriver driver, string popupXPath, int tolerance)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {
                IWebElement popup = driver.FindElement(By.XPath(popupXPath));

                int viewportWidth = Convert.ToInt32(js.ExecuteScript("return window.innerWidth"));
                int viewportHeight = Convert.ToInt32(js.ExecuteScript("return window.innerHeight"));

                int popupX = popup.Location.X;
                int popupY = popup.Location.Y;
                int popupWidth = popup.Size.Width;
                int popupHeight = popup.Size.Height;

                int expectedX = (viewportWidth - popupWidth) / 2;
                int expectedY = (viewportHeight - popupHeight) / 2;

                if (Math.Abs(popupX - expectedX) > tolerance || Math.Abs(popupY - expectedY) > tolerance)
                {
                    throw new InvalidOperationException("Popup is NOT centered.");
                }

                Console.WriteLine("Popup is correctly centered.");
            }

            catch (NoSuchElementException)
            {
                Console.WriteLine("Popup '{popupXPath}' not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error verifying popup alignment: {ex.Message}");
            }
        }

        static void PerformResize(WebDriver driver, bool maximize, string ButtonXpath)
        {
            var button = driver.FindElement(By.XPath(ButtonXpath));
            button.Click();
            Console.WriteLine(maximize ? "Maximizing window..." : "Minimizing window...");
        }

        private static void Wait(int p)
        {
            
            Thread.Sleep(p);
        }

        private static void CloseWindow(WebDriver driver, string v1, string v2)
        {
            var window = driver.WindowHandles;
            foreach (string handle in window)
            {
                // Switch to the window
                driver.SwitchTo().Window(handle);

                // Check if the title of the window matches the specified window name
                if (driver.Title.ToString().Contains(v1))
                {
                    try
                    {
                        IWebElement element = driver.FindElement(By.XPath(v2));
                        driver.Close();

                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    // Close the window

                    break;
                }

            }

        }

        private static void PerformActionThroughClass(WebDriver driver, string action, string classOfElement, DataRow row, string Column)
        {
            if (action.Equals("Click"))
            {
                try
                {
                    driver.FindElement(By.ClassName(classOfElement)).Click();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (action.Equals("SendKeys"))
            {
                try
                {
                    if (row.Table.Columns.Contains(Column.ToString()) && !row[Column].ToString().ToUpper().Equals("BLANK"))
                    {
                        driver.FindElement(By.ClassName(classOfElement)).Click();
                        for (int i = 0; i < 10; i++)
                        {
                            Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                            Keyboard.SendKeys(KeyboardConstants.DELETE_KEY);
                        }
                        //clearField(driver, id);
                        driver.FindElement(By.ClassName(classOfElement)).SendKeys(Keys.Control + Keys.Home);
                        driver.FindElement(By.ClassName(classOfElement)).SendKeys(Keys.Control + Keys.Shift + Keys.End);
                        driver.FindElement(By.ClassName(classOfElement)).SendKeys(row[Column].ToString());

                        driver.FindElement(By.ClassName(classOfElement)).SendKeys(Keys.Enter);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }


        private static void PerformActionsThroughID(WebDriver driver, string action, string id, DataRow row, string Column)
        {
            if (action.Equals("Click"))
            {
                try
                {                    
                    var element = driver.FindElement(By.Id(id));
                    Actions actions = new Actions(driver);
                    actions.MoveToElement(element).Click().Perform();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (action.Equals("SendKeys"))
            {
                try
                {
                    if (row.Table.Columns.Contains(Column.ToString()) && !row[Column].ToString().ToUpper().Equals("BLANK"))
                    {
                        if (!string.IsNullOrEmpty(row[Column].ToString()))
                        {
                            var element = driver.FindElement(By.Id(id));
                            Actions actions = new Actions(driver);
                            actions.MoveToElement(element).Click().Perform();
                            for (int i = 0; i < 10; i++)
                            {
                                Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                                Keyboard.SendKeys(KeyboardConstants.DELETE_KEY);
                            }
                            actions.MoveToElement(element)
                            .KeyDown(Keys.Control)
                            .SendKeys("a")
                            .KeyUp(Keys.Control).SendKeys(Keys.Backspace)
                            .Perform();
                            try
                            {
                                driver.FindElement(By.Id(id)).Clear();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                            driver.FindElement(By.Id(id)).SendKeys(row[Column].ToString());
                            driver.FindElement(By.Id(id)).SendKeys(Keys.Enter);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            else if (action.Equals("SendKeysWithoutClear")) {
                try
                {                    
                    driver.FindElement(By.Id(id)).SendKeys(row[Column].ToString());
                    driver.FindElement(By.Id(id)).SendKeys(Keys.Tab);
                    Console.WriteLine(row[Column].ToString());
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
            }
        }

        private static int GTD(DataRow dr)
        {
            var SelectionDate = 0;
            if (dr.Table.Columns.Contains(TestDataConstants.COL_Expiration_Date))
            {
                if (dr[TestDataConstants.COL_Expiration_Date].ToString().ToUpper().Contains("TODAY"))
                {
                    string tempDate = DataUtilities.DateHandler(dr[TestDataConstants.COL_Expiration_Date].ToString());
                    string date = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));
                    string[] split = date.Split('/');
                    SelectionDate = Int32.Parse(split[1]);
                }
                else
                {
                    string str = dr[TestDataConstants.COL_Expiration_Date].ToString();
                    string[] dates = str.Split('/');
                    var nextDate = new DateTime(int.Parse(dates[2]), int.Parse(dates[0]), int.Parse(dates[1]));
                    var today = DateTime.Now;
                    SelectionDate = nextDate.Day - today.Day;
                    SelectionDate += today.Day;
                }
            }
            return SelectionDate;
        }

        private static void PerformActions(WebDriver driver, string action, string xpath, DataRow row, string Column)
        {
            var temp_dict = GetDict(SamsaraMappingTables[temp_ModuleName], 2);
            if (action == "DropDown")
            {
                //Actions actions = new Actions(driver);
                //IWebElement inputElement = driver.FindElement(By.XPath(p2));
                //actions.Click(inputElement).Perform();
                //actions.SendKeys(Keys.Control).SendKeys(Keys.Backspace).SendKeys(Keys.Enter).Click(inputElement).Perform();
                clearField(driver, xpath);

                string value = row[Column].ToString();
                driver.FindElement(By.XPath(xpath)).SendKeys(value);
                driver.FindElement(By.XPath(xpath)).SendKeys(Keys.Enter);
            }
            else if (action == "Click")
            {
                try
                {
                    if (Column.ToUpper().Contains("SEND") || Column.ToUpper().Contains("LIVE"))
                    {
                        SaveData(TestCaseSheet_Temp, row.Table.TableName, row);
                    }
                    if (ConfigurationManager.AppSettings["ToastVerify"] == "false")
                    {
                        Wait(2000);
                    }
                    driver.FindElement(By.XPath(xpath)).Click();
                }
                catch
                {
                    try
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        string script = "document.querySelector(" + xpath + ").click();";
                        js.ExecuteScript(script);
                    }
                    catch
                    {
                        driver.FindElement(By.XPath(temp_dict[Column].ToString())).Click();

                    }
                }
            }
            else if (action == "SendKeys")
            {
                try
                {
                    clearField(driver, xpath);
                    driver.FindElement(By.XPath(xpath)).SendKeys(Keys.Control + Keys.Home);
                    driver.FindElement(By.XPath(xpath)).SendKeys(Keys.Control + Keys.Shift + Keys.End);
                    Wait(2000);
                    driver.FindElement(By.XPath(xpath)).SendKeys(row[Column].ToString());
                    driver.FindElement(By.XPath(xpath)).SendKeys(Keys.Tab);
                }
                catch
                {
                    try
                    {
                        driver.FindElement(By.XPath(temp_dict[Column].ToString())).SendKeys(Keys.Control + Keys.Home);
                        driver.FindElement(By.XPath(temp_dict[Column].ToString())).SendKeys(Keys.Control + Keys.Shift + Keys.End);
                        driver.FindElement(By.XPath(temp_dict[Column].ToString())).SendKeys(row[Column].ToString() + Keys.Tab);
                    }
                    catch
                    {
                        IWebElement element = driver.FindElement(By.XPath(temp_dict[Column].ToString()));
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("arguments[0].value='" + row[Column].ToString() + "';", element);
                    }
                }

            }

            else if (action == "FillValues")
            {
                try
                {
                    if (row[Column].ToString() != "")
                    {
                        driver.FindElement(By.XPath(xpath)).SendKeys(Keys.Control + Keys.Home);
                        driver.FindElement(By.XPath(xpath)).SendKeys(Keys.Control + Keys.Shift + Keys.End);
                        driver.FindElement(By.XPath(xpath)).SendKeys(row[Column].ToString() + Keys.Tab);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
            else if (action == "Enter")
            {
                try
                {
                    driver.FindElement(By.XPath(xpath)).SendKeys(Keys.Enter);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if(action == "WaitForElement")
            {
                Console.WriteLine("Not Implemented yet");
                /*try{
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(p2)));
                }
                catch (WebDriverTimeoutException  ex){
                    Console.WriteLine("Timeout (120s) waiting for element to be visible: "+ex)
                }
                catch (Exception ex){
                    Console.WriteLine(ex);
                }*/
            }


        }

        private static void clearField(WebDriver driver, string p2)
        {
            try
            {
                int maxLength = 26;
                string value = driver.FindElement(By.XPath(p2)).Text;
                int length = value.Length;
                if (length <= 0)
                    length = maxLength;
                for (int i = 0; i < length; i++)
                {
                    try
                    {
                        driver.FindElement(By.XPath(p2)).SendKeys(Keys.Control + Keys.Home);
                        driver.FindElement(By.XPath(p2)).SendKeys(Keys.Delete);
                    }
                    catch
                    {
                        driver.FindElement(By.XPath(p2)).Clear();
                    }
                }
                for (int i = 0; i < length; i++)
                {
                    driver.FindElement(By.XPath(p2)).SendKeys(Keys.Backspace);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

        }


        private static void OpenModule(string moduleName, WebDriver driver)
        {

            SwitchWindow.SwitchToWindow(driver, "Dock");

            if (moduleName.ToLower().Contains("nirvana"))
            {
                string[] arr = moduleName.Split(',');
                int i = 0;
                while (i < 3)
                {
                    driver.FindElement(By.XPath(GetDict(SamsaraMappingTables["HomeDock"])["SearchBar"].ToString())).Click();
                    if (arr.Length > 1)
                    {
                        SwitchWindow.SwitchToWindow(driver, "Home");
                        driver.FindElement(By.XPath(GetDict(SamsaraMappingTables["OpenFin"])["OpenWindow"].ToString())).Click();
                        IWebElement name = driver.FindElement(By.XPath(GetDict(SamsaraMappingTables["OpenFin"])["OpenWindow"].ToString()));
                        Actions action = new Actions(driver);
                        action.MoveToElement(name)
                        .KeyDown(Keys.Control)
                        .SendKeys("a")
                        .KeyUp(Keys.Control).SendKeys(Keys.Backspace).SendKeys(arr[1])
                        .Perform();
                        Wait(2000);
                        action.SendKeys(Keys.Enter).Perform();
                    }
                    else
                    {
                        driver.FindElement(By.XPath(GetDict(SamsaraMappingTables["HomeDock"])["Nirvana"].ToString())).Click();
                        Wait(1000);
                        Keyboard.SendKeys("+");
                    }
                    try
                    {
                        SwitchWindow.SwitchToWindow(driver, "Nirvana", true);

                    }
                    catch (ExceptionHandlingException) { }
                    if (driver.Title == "Nirvana")
                    {
                        break;
                    }
                    else
                    {
                        SwitchWindow.SwitchToWindow(driver, "Dock");
                        i++;
                    }
                    if (i == 3)
                    {
                        Console.WriteLine("RTPNL is not Openend");
                        SamsaraHelperClass.CaptureMyScreen("RTPNL_RelatedStep", ApplicationArguments.TestCaseToBeRun);
                    }
                }
                Wait(10000);
                return;
            }

            driver.FindElement(By.XPath(GetDict(SamsaraMappingTables["HomeDock"])[moduleName].ToString())).Click();

        }

        public static string RemoveTrailingZeroesFromString(string str) {
            int index = str.IndexOf('.');
            string result = (index >= 0) ? str.Substring(0, index) : str;
            return result;
        }

        public static bool checkStepIspresentOrNot(string ModuleName)
        {
            if (DataTables.ContainsKey(ModuleName))
                return true;
            return false;
        }
        public static string SearchFile(string directory, string fileName)
        {
            try
            {
                string[] files = Directory.GetFiles(directory, fileName + ".*", SearchOption.AllDirectories);


                if (files.Length > 0)
                {
                    return files[0];
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static void DeleteFile(string filePath)
        {
            try
            {

                File.Delete(filePath);
            }
            catch (IOException ex)
            {

                Console.WriteLine("Error deleting file: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Unauthorized access to delete file: " + ex.Message);
            }
        }

        public static void MoveWindow(ref WebDriver driver, ref string action)
        {
            try
            {
                string script;

                WindowDirection direction;
                {
                    if (action == "RightTop")
                        direction = WindowDirection.RightTop;
                    else if (action == "RightBottom")
                        direction = WindowDirection.RightBottom;
                    else if (action == "LeftTop")
                        direction = WindowDirection.LeftTop;
                    else if (action == "LeftBottom")
                        direction = WindowDirection.LeftBottom;
                    else
                    {
                        Console.WriteLine("Invalid action: " + action + ". Please provide correct actions");
                        return;
                    }
                }
                switch (direction)
                {
                    case WindowDirection.RightTop:
                        script = "window.moveTo(window.screen.availWidth - window.outerWidth + 120, 50);";
                        break;
                    case WindowDirection.RightBottom:
                        script = "window.moveTo(window.screen.availWidth - window.outerWidth, window.screen.availHeight - window.outerHeight);";
                        break;
                    case WindowDirection.LeftTop:
                        script = "window.moveTo(60, 60);";
                        break;
                    case WindowDirection.LeftBottom:
                        script = "window.moveTo(60, window.screen.availHeight - window.outerHeight);";
                        break;
                    default:
                        throw new ArgumentException("Invalid direction specified.");
                }

                ((IJavaScriptExecutor)driver).ExecuteScript(script);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while moving the window: " + ex.Message);
            }
        }

        public static void ClickMenuItemByText(IWebDriver driver, string menuItemText, int maxAttempts)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    string xpath = "//span[contains(text(), '" + menuItemText + "')]";
                    IWebElement menuItem = driver.FindElement(By.XPath(xpath));

                    menuItem.Click();
                    break;
                }
                catch (NoSuchElementException)
                {

                    if (attempt == maxAttempts)
                    {
                        Console.WriteLine("Menu item '" + menuItemText + "' not found after " + maxAttempts + " attempts.");
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        Console.WriteLine("Attempt " + attempt + ": Menu item '" + menuItemText + "' not found. Retrying...");
                    }
                }
                catch (Exception ex)
                {
                    CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun);
                    Console.WriteLine("An error occurred while clicking the menu item '" + menuItemText + "': " + ex.Message);
                }
            }
        }

        public static void SetDefaultSetValue(string action)
        {
            try { DefaultCellValue = action; }

            catch { }
        }


        public static void UpdateRowIndex(int index)
        {
            try
            {
                RowIndex = index;//int.Parse(table.Rows[i]["Action"].ToString());
            }
            catch { }
        }

        public static void CloseRTPNL(WebDriver driver, ref Dictionary<string, object> dict, DataTable excelData)
        {

           try
    {
        foreach (DataRow dr in excelData.Rows)
        {

            List<string> DashBoardList = dr[TestDataConstants.COL_DASHBOARDPARENTCHILD].ToString().Split(',').ToList();

            if (DashBoardList.Count > 0)
            {
                int retryCount = 0;
                bool success = false;

                while (retryCount < 3 && !success)
                {
                    try
                    {
                            SwitchWindow.SwitchToWindow(driver, DashBoardList[0], true);
                            SwitchWindow.SwitchToChildWindow(driver, DashBoardList[0], dict[DashBoardList[1] + "Dashboard"].ToString());
                            driver.FindElement(By.XPath(dict["CloseModule"].ToString())).Click();
                            if (!excelData.Columns.Contains("ActionOnPopUp")) 
                            {
                                Wait(2000);
                                bool flag = SwitchWindow.SwitchToWindow(driver,"Unsaved changes popup");
                                if(flag)
                                    driver.FindElement(By.Id(dict["DiscardRTPNLclose"].ToString())).Click();
                                Wait(2000);
                            }
                            success = true; 
                    }
                    catch (Exception ex)
                    {
                        retryCount++;
                        if (retryCount >= 3)
                        {
                            Console.WriteLine("Failed to close the module. Attempt: " + retryCount + ". Error: " + ex.Message);
                        }
                       
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("An unexpected error occurred: " + ex.Message);
        throw;
    }
        }

        public static void VerifyExportedData(string input, ref DataTable Data)
        {

            try
            {
                Thread.Sleep(6000);
                int indexOfDash = input.IndexOf("-");
                string fileName = input.Substring(0, indexOfDash);
                string sheetName = input.Substring(indexOfDash + 1);

                string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string filePath2 = SearchFile(downloadsPath, fileName);
                if (!string.IsNullOrEmpty(filePath2))
                {
                    List<string> list = new List<string>();
                    DataSet ds = DataUtilities.GetTestCaseTestData(filePath2, 1, 1, list);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables.Contains(sheetName))
                            Data = ds.Tables[sheetName];
                        else
                            Data = ds.Tables[0];
                    }
                    else
                    {
                        Data = null;
                        Console.WriteLine("VerifyExportedData teststep requires correction...Data not found");
                    }



                }


            }
            catch (Exception ex)
            {
                CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun);
                Console.WriteLine("");
            }
        }

        private static void SaveData(DataSet LiveData, string tablename, DataRow row)
        {
            try
            {
                string newTableName = TestStatusLog.removeDigitsFromModule(tablename);
                DataTable singleRowTable = LiveData.Tables[0].Clone();
                singleRowTable.ImportRow(row);

                singleRowTable.TableName = newTableName;

                DataSet singleRowDataSet = new DataSet();
                singleRowDataSet.Tables.Add(singleRowTable);

                if (!Directory.Exists("SimulatorData"))
                    Directory.CreateDirectory("SimulatorData");
                if (File.Exists(@"SimulatorData/LiveTrades.xml"))
                {
                    try
                    {
                        XmlDocument xml1 = new XmlDocument();
                        XmlDocument xml2 = new XmlDocument();
                        xml1.Load(@"SimulatorData/LiveTrades.xml");

                        if (xml1.SelectSingleNode("NewDataSet/CreateNewSubLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateReplaceLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/SendOrderUsingMTT") != null || xml1.SelectSingleNode("NewDataSet/" + newTableName) != null)
                        {
                            xml2.LoadXml(singleRowDataSet.GetXml());
                            foreach (XmlNode list in xml2.SelectSingleNode("NewDataSet").ChildNodes)
                                xml1.SelectSingleNode("NewDataSet").AppendChild(xml1.ImportNode(list, true));
                            xml1.Save(@"SimulatorData/LiveTrades.xml");
                        }
                        else
                        {
                            singleRowDataSet.WriteXml(@"SimulatorData/LiveTrades.xml");
                        }
                    }
                    catch { singleRowDataSet.WriteXml(@"SimulatorData/LiveTrades.xml"); }
                }
                else
                    singleRowDataSet.WriteXml(@"SimulatorData/LiveTrades.xml");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public static DataTable getGroupedData(string filePath)
        {
            DataTable groupedDataTable = new DataTable();
            List<string> GroupedColumnList = new List<string>();
            bool isItFirstOutline = true;
            bool isItAColumnRow = true;

            try
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["Orders"];

                    for (int row = worksheet.Dimension.Start.Row; row <= worksheet.Dimension.End.Row; row++)
                    {
                        int outlineLevel = worksheet.Row(row).OutlineLevel;

                        if (outlineLevel == 0)
                        {
                            if (!isItFirstOutline)
                            {
                                isItFirstOutline = true;
                            }
                        }
                        else if (outlineLevel > 0)
                        {
                            if (GroupedColumnList.Count > 0)
                            {
                                isItAColumnRow = AreAllRowDataColumns(GroupedColumnList, worksheet, row);
                            }

                            if (GroupedColumnList.Count == 0)
                            {
                                CreateListOfColumns(worksheet, row, GroupedColumnList);
                            }

                            if (isItFirstOutline)
                            {
                                if (groupedDataTable.Columns.Count == 0)
                                    AddColumnsToDataTable(groupedDataTable, GroupedColumnList);

                                isItFirstOutline = false;
                            }

                            if (!isItAColumnRow && !isItFirstOutline)
                            {
                                AddRowToDataTable(groupedDataTable, GroupedColumnList, worksheet, row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return groupedDataTable;
        }

        static bool AreAllRowDataColumns(List<string> groupedColumnList, ExcelWorksheet worksheet, int row)
        {

            bool allRowDataColumns = true;
            try
            {
                for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                {
                    string cellValue = worksheet.Cells[row, col].Text.Trim();
                    if (!string.IsNullOrEmpty(cellValue) && !groupedColumnList.Contains(cellValue))
                    {
                        allRowDataColumns = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return allRowDataColumns;
        }

        static void CreateListOfColumns(ExcelWorksheet worksheet, int row, List<string> groupedColumnList)
        {

            try
            {
                for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                {

                    string columnName = worksheet.Cells[row, col].Text.Trim();
                    if (!string.IsNullOrEmpty(columnName))
                        groupedColumnList.Add(columnName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void AddColumnsToDataTable(DataTable table, List<string> columns)
        {
            try
            {
                foreach (string columnName in columns)
                {
                    table.Columns.Add(columnName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void AddRowToDataTable(DataTable table, List<string> columns, ExcelWorksheet worksheet, int row)
        {
            DataRow newRow = table.NewRow();
            try
            {
                for (int col = worksheet.Dimension.Start.Column; col < worksheet.Dimension.End.Column; col++)
                {
                    string val = table.Columns[col - 1].ColumnName.ToString();
                    string cellValue = worksheet.Cells[row, col + 1].Text.Trim();
                    newRow[val] = cellValue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            table.Rows.Add(newRow);
        }

        private static WebDriver GetSamsaraDriver(bool allowMaxRetries = false)
        {
            TimeSpan timeout = TimeSpan.FromSeconds(10);
            int maxTries = 3;

            for (int attempt = 0; attempt < maxTries; attempt++)
            {
                try
                {
                    if (GlobalDriver == null || !CheckDriverStatus(GlobalDriver))
                    {
                        ChromeOptions options = new ChromeOptions();
                        options.DebuggerAddress = "localhost:8084";
                        GlobalDriver = CreateRemoteWebDriver(options, timeout);
                    }
                    return GlobalDriver;
                }

                catch (Exception ex)
                {


                    if (!allowMaxRetries)
                    {
                        Console.WriteLine(ex.Message + ". Failed to connect to debugger address within the specified timeout.. returning null");
                        return null;
                    }

                    if (attempt + 1 == maxTries)
                        throw new Exception("Failed while calling GetSamsaraDriver: " + ex.Message);

                    CheckAndStartChromeDriver();
                    Console.WriteLine("Attempt " + (attempt + 1) + " failed while calling GetSamsaraDriver: " + ex.Message);
                }
            }
            return GlobalDriver;
        }

        private static WebDriver CreateRemoteWebDriver(ChromeOptions options, TimeSpan timeout)
        {
            try
            {
                return new RemoteWebDriver(new Uri("http://localhost:9515"), options.ToCapabilities(), timeout);
            }
            catch (WebDriverException ex)
            {
                throw ex;
            }
        }


        public static bool LogOutUser()
        {
            bool flag = false;
            if (ConfigurationManager.AppSettings["-ProductDependency"].ToLower() == "samsara")
            {
                WebDriver driver = null;
                try
                {
                    driver = GetSamsaraDriver();

                }
                catch { }


                string name = string.Empty;
                try
                {
                    foreach (var handle in driver.WindowHandles)
                    {
                        name = handle;
                        Console.WriteLine("Try to switch window to " + "Dock");

                        driver.SwitchTo().Window(name);
                        Console.WriteLine(driver.Title);
                        string a = driver.Title;
                        if (driver.Title.Contains("Dock"))
                        {

                            driver.FindElement(By.XPath(SamsaraXpath("LogOut", "HomeDock"))).Click();
                            System.Threading.Thread.Sleep(2000);
                            SwitchWindow.SwitchToWindow(driver, "Popup");
                            driver.FindElement(By.XPath(SamsaraXpath("LogOutFinalButton", "HomeDock"))).Click();
                            System.Threading.Thread.Sleep(8000);
                            flag = true;
                            break;
                        }
                    }

                }

                catch (Exception e)
                {
                    Console.WriteLine("Ignoring NoSuchWindowException " + name + e);
                }

            }
            return flag;
        }
        public static void CaptureMyScreen(string folder = "", string CaseName = "", string action = "", string classType = "")
        {
            if (!ConfigurationManager.AppSettings["-AllowCopyLogFileToMaster"].Equals("false"))
            {
                try
                {
                    //Creating a new Bitmap object
                    Bitmap captureBitmap = new Bitmap(1600, 900, PixelFormat.Format32bppArgb);
                    //Bitmap captureBitmap = new Bitmap(int width, int height, PixelFormat);
                    //Creating a Rectangle object which will
                    //capture our Current Screen
                    Rectangle captureRectangle = System.Windows.Forms.Screen.AllScreens[0].Bounds;
                    //Creating a New Graphics Object
                    Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                    //Copying Image from The Screen
                    captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
                    //Saving the Image File (I am here Saving it in My E drive).
                    long time = DateTime.UtcNow.Ticks;
                    int ip = TestStatusLog.GetSytemIP().ToString().LastIndexOf(".");
                    string slaveip = TestStatusLog.GetSytemIP().ToString().Substring(ip + 1);
                    string masterpath = ConfigurationManager.AppSettings["-MasterReportPath"].ToString().Replace("?", ConfigurationManager.AppSettings["-runDescription"].ToString().Substring(0, ConfigurationManager.AppSettings["-runDescription"].ToString().IndexOf(" ")));
                    string dir = masterpath + "2_" + slaveip + "\\Screenshot\\LoginIssue\\";
                    string dir2 = @"E:\SamsaraScreenshot\LoginIssue\";
                    if (CaseName != "")
                    {
                        dir = masterpath + "2_" + slaveip + "\\Screenshot\\" + CaseName + "\\";
                        dir2 = @"E:\SamsaraScreenshot\" + CaseName + "\\";
                    }
                    else if (folder != "")
                    {
                        dir = masterpath + "2_" + slaveip + "\\Screenshot\\" + folder + "\\";
                        dir2 = @"E:\SamsaraScreenshot\" + folder + "\\";
                    }
                    string path = dir + time + ".jpg";
                    string path2 = dir2 + time + ".jpg";
                    if (!string.IsNullOrEmpty(action))
                    {
                        if (ApplicationArguments.ProductDependency.ToLower().Equals("samsara"))
                        {
                            path = dir + action + ".jpg";
                            path2 = dir2 + action + ".jpg";
                        }
                        else
                        {
                            path = dir + "Issue at " + action + " step or class" + ".jpg";
                        }
                    }
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    if (!Directory.Exists(dir2) && ApplicationArguments.ProductDependency.ToLower().Equals("samsara"))
                    {
                        Directory.CreateDirectory(dir2);
                    }

                    captureBitmap.Save(path, ImageFormat.Jpeg);
                    if (ApplicationArguments.ProductDependency.ToLower().Equals("samsara"))
                    {
                        captureBitmap.Save(path2, ImageFormat.Jpeg);
                        Console.WriteLine("ScreenShot saved at path: " + path2);
                    }
                    Console.WriteLine("ScreenShot saved at path: " + path);
                    string[] processesName = ConfigurationManager.AppSettings["ServicesName"].Split(',');
                    if (ApplicationArguments.ProductDependency.ToLower().Equals("samsara") && string.IsNullOrEmpty(classType))
                    {
                        foreach (string processName in processesName)
                        {
                            if (IsProcessRunning(processName))
                            {
                                Console.WriteLine(processName + " is running.");
                            }
                            else
                            {
                                Console.WriteLine(processName + " is not running.");
                                if (processName.Equals("chromedriver"))
                                {
                                    CheckAndStartChromeDriver();
                                }
                                else
                                {
                                    throw new Exception(processName + " is not running.");
                                }
                            }
                        }
                    }


                    //Displaying the Successfull Result
                    //MessageBox.Show("Screen Captured");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("is not running."))
                    {
                        throw ex;
                    }
                }
            }
        }



        static bool IsProcessRunning(string ProcessName)
        {
            Process[] processes = Process.GetProcessesByName(ProcessName);
            return processes.Length > 0;
        }


        public static bool WaitForWindowToQuit(string windowName, TimeSpan maxTime)
        {

            try
            {
                WebDriver driver = GetSamsaraDriver();
                return WaitForWindow(windowName, driver, maxTime);
            }

            catch (Exception ex)
            {
                Console.WriteLine("WaitForWindow is over");
            }
            return true;
        }

        public static bool WaitForWindow(string windowName, WebDriver driver, TimeSpan maxTime)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var endTime = DateTime.Now.Add(maxTime);
                int maxTries = 3;
                int tries = 0;

                while (DateTime.Now < endTime && tries < maxTries)
                {
                    try
                    {
                        var availableWindows = driver.WindowHandles;
                        bool found = false;

                        Console.WriteLine("Available windows:");

                        foreach (var windowHandle in availableWindows)
                        {
                            if (driver.SwitchTo().Window(windowHandle).Title.Contains(windowName))
                            {
                                Console.WriteLine(windowName + " Found");
                                found = true;
                            }
                            else
                                Console.WriteLine("Window Title: " + driver.Title);
                        }
                        Thread.Sleep(10000);

                        if (!found)
                            tries++;
                    }
                    catch (NoSuchWindowException)
                    {
                        tries++;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("WaitForWindow :" + ex.Message);

            }
            return false;
        }

        public static bool IsWindowOpen(string windowName)
        {
            WebDriver driver = null;
            try
            {
                driver = GetSamsaraDriver();
                var availableWindows2 = driver.WindowHandles;

                foreach (var windowHandle in availableWindows2)
                {
                    if (driver.SwitchTo().Window(windowHandle).Title.Contains(windowName))
                    {
                        Console.WriteLine(windowName + " Found");
                        return true;
                    }
                    else
                        Console.WriteLine("Window Title: " + driver.Title);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }


        public static bool VerifyIndexWise(IWebDriver driver, DataTable dtable, string stepName, ref Dictionary<string, object> dict)
        {
            bool success = false;
            try
            {
                IWebElement ExportButton = driver.FindElement(By.XPath(dict[dtable.Rows[0]["GridType"].ToString() + "ExportButton"].ToString()));
                ExportButton.Click();
                DataTable mainTable = null;
                DataSet Ds = null;
                string fileName = string.Empty;

                if (string.Equals("ORDER", dtable.Rows[0]["GridType"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                {
                    fileName = ConfigurationManager.AppSettings["-BlotterExportDefaultFileName"].ToString();
                    string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    string filePath2 = SamsaraHelperClass.SearchFile(downloadsPath, fileName);

                    if (!string.IsNullOrEmpty(filePath2))
                    {
                        if (string.Equals("SUBORDER", dtable.Rows[0]["GridType"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                        {
                            mainTable = SamsaraHelperClass.getGroupedData(filePath2);
                        }
                        else
                        {
                            Ds = SamsaraGridOperationHelper.ExportedData(ConfigurationManager.AppSettings["-BlotterExportDefaultFileName"].ToString());
                        }
                    }


                    if (mainTable == null)
                    {
                        if (string.Equals("ORDER", dtable.Rows[0]["GridType"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                        {
                            mainTable = Ds.Tables.Contains("Orders") ? Ds.Tables["Orders"] : (Ds.Tables.Count > 0 ? Ds.Tables[0] : null);
                        }
                        else if (string.Equals("WORKING", dtable.Rows[0]["GridType"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                        {
                            mainTable = Ds.Tables.Contains("Working") ? Ds.Tables["Working"] : (Ds.Tables.Count > 0 ? Ds.Tables[1] : null);
                        }

                    }
                }
                for (int i = 0; i < dtable.Rows.Count; i++)
                {
                    DataRow dr = dtable.Rows[i];
                    DataRow drMain = mainTable.Rows[i];
                    success = AreDataRowsEqual(dr, drMain);
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return success;
        }

        public static bool AreDataRowsEqual(DataRow row1, DataRow row2)
        {
            try
            {
                for (int i = 0; i < row1.Table.Columns.Count; i++)
                {
                    var column1 = row1.Table.Columns[i];


                    if (!row2.Table.Columns.Contains(column1.ColumnName))
                        continue;

                    var column2 = row2.Table.Columns[column1.ColumnName];

                    if (!string.IsNullOrEmpty(row1[column1].ToString()))
                    {
                        if (!Equals(row1[column1], row2[column2]))
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return true;
        }



        public static Dictionary<string, string> ConvertDataTableToDictionary(DataTable table)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (DataRow row in table.Rows)
            {
                string key = row[0].ToString();
                string value = row[1].ToString();
                dict.Add(key, value);
            }

            return dict;
        }
        private static void CheckAndStartChromeDriver()
        {

            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                bool isChromeDriverRunning = IsProcessRunning("chromedriver");

                if (isChromeDriverRunning)
                { Console.WriteLine("ChromeDriver is running"); }

                if (!isChromeDriverRunning)
                {
                    try
                    {
                        Wait(5000);
                        ProcessStartInfo StartChromeExe = new ProcessStartInfo();
                        StartChromeExe.FileName = "chromedriver.exe";
                        StartChromeExe.WorkingDirectory = ConfigurationManager.AppSettings["ChromeDriverExePath"];
                        StartChromeExe.WindowStyle = ProcessWindowStyle.Minimized;
                        StartChromeExe.Arguments = "--port=9515";
                        Process ChromeProcess = new Process();
                        ChromeProcess.StartInfo = StartChromeExe;
                        ChromeProcess.Start();
                        Wait(5000);
                    }

                    catch (Exception ex)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                        if (rethrow)
                            throw;
                    }
                }
            }
        }


        private static bool CheckDriverStatus(WebDriver driver)
        {
            try
            {
                foreach (String name in driver.WindowHandles)
                {
                    driver.SwitchTo().Window(name);
                    Console.WriteLine(driver.Title);
                    Console.WriteLine("driver is working fine...");
                    break;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred while checking driver status: " + ex.Message);
                return false;
            }
        }
    }

}

