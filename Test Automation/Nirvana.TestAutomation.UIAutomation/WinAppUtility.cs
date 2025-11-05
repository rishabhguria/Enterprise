using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Support.UI;
using OfficeOpenXml;
using System.IO;
using System.Diagnostics;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces;
using System.Data;
using OpenQA.Selenium.Remote;

namespace Nirvana.TestAutomation.UIAutomation
{
    public class WinAPPMappings
    {
        public string SelectorType;
        public string SelectorValue;

    }

   

    public  static class WinAppUtility
    {
       public  static string automationId = "PranaMain"; // ID for PranaMain
        public static string login_id = "txtLoginID_EmbeddableTextBox"; // maintain sheets for id
        public static string password = "txtPassword_EmbeddableTextBox";
        public static string loginButton = "btnLogin";
        
        public static string pttClass = "ViewableRecordCollection";

        public static string pttidforclick = "//Edit[@ClassName=\'XamMaskedEditor']";


        public static string xpath_login_id = "//Edit[@AutomationId='txtLoginID_EmbeddableTextBox']";

        public static string xpath_login2 = "//Edit[@AutomationId=\'txtLoginID']";

        public static string xpath_passwrd = "//Edit[@AutomationId=\'txtPassword']";


        public static string xpath_loginbtn = "//Button[@AutomationId=\'btnLogin']";

        public static string xpath_pttGrid = "//DataGrid[@ClassName=\'RecordListGroup\']";

        public static string xpath_PranaMain = "//Window[@AutomationId=\'PranaMain']";
        public static string openPTT = "//Button[@AutomationId=\'[Toolbar : subToolBar Tools] Tool : % Trading Tool - Index : 3']";

        public static string AutomationId_PranaMain = "_PranaMain_UltraFormManager_Dock_Area_Top";

        public static string xPath_PTT_Symbol = "//Edit[@AutomationId=\'TextBoxPresenter\']";


        public static string xPathPTTAccounts = "//Custom[@ClassName=\'Cell\']/Custom[@ClassName=\'XamComboEditor\']";

        public static string xDataItems = "//DataItem[@ClassName=\'Record\']";

        public static string Open_PTT = "//Button[@AutomationId=\'[Toolbar : subToolBar Tools] Tool : % Trading Tool - Index : 3 ']";

        public static string Open_RB = "//Button[@AutomationId=\'[Toolbar : subToolBar Tools] Tool : Rebalancer - Index : 4 ']";

        public static string xPath_rb_AccountGroups = "//Custom[@Name=\"CmbAccountsAndGroups\"][@AutomationId=\"CmbAccountsAndGroups\"]/Edit[@AutomationId=\"TextBoxPresenter\"]";

        public static string xPath_rb_FetchBtn = "//Custom[@AutomationId=\"RebalancerView\"]/Pane[@ClassName=\"ScrollViewer\"]/Button[@ClassName=\"Button\"][@Name=\"Fetch\"]";

        public static string xPath_rb_AddCash = "//Edit[@AutomationId=\"CashFowEditor\"]";

        public static string xPath_rb_RebalanceBtn = "//Group[@ClassName=\"GroupBox\"][@Name=\"Rebalance\"]/Button[@ClassName=\"Button\"]";

        public static Dictionary<string, Dictionary<string, List<WinAPPMappings>>> MappingsReader(string SheetPath)
        {
            Dictionary<string, Dictionary<string, List<WinAPPMappings>>> sheetMappings = new Dictionary<string, Dictionary<string, List<WinAPPMappings>>>();
            try
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(SheetPath)))
                {
                    foreach (ExcelWorksheet sheet in package.Workbook.Worksheets)
                    {
                        if (!sheet.Name.StartsWith("_"))
                        {
                            Dictionary<string, List<WinAPPMappings>> innerDictionary = new Dictionary<string, List<WinAPPMappings>>();

                            int rowCount = sheet.Dimension.Rows;
                            string keyName = null; // Declare keyName outside the loop

                            for (int row = 2; row <= rowCount; row++)
                            {
                                keyName = sheet.Cells[row, 1].Text;
                                WinAPPMappings mapping = new WinAPPMappings
                                {
                                    SelectorType = sheet.Cells[row, 2].Text,
                                    SelectorValue = sheet.Cells[row, 3].Text
                                };

                                if (!innerDictionary.ContainsKey(keyName))
                                {
                                    innerDictionary[keyName] = new List<WinAPPMappings>();
                                }

                                innerDictionary[keyName].Add(mapping);
                            }

                            sheetMappings[sheet.Name] = innerDictionary;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sheetMappings;
        }



        public static void OpenAndMinimizeApp(string appPath)
        {
            AppiumOptions appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.App, appPath);

            WindowsDriver<WindowsElement> driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptions);

            driver.Manage().Window.Minimize();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(condition =>
            {

                return true; // custom condition
            });
            driver.Quit();
        }

        public static void StartWinAppDriverServer()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C \"C:\\Program Files (x86)\\Windows Application Driver\\WinAppDriver.exe\"";
            process.StartInfo.UseShellExecute = false;
            process.Start();

            System.Threading.Thread.Sleep(5000);
        }
        public static void ApplicationStartup(WinAppDriverProvider driver)
        {
            try
            {
              
                driver.KeyboardInputWithVerification(DataContainer.xpath_login2, DataContainer.Username_Id1);

                driver.KeyboardInputWithVerification(DataContainer.xpath_passwrd, DataContainer.Password_Id);
                driver.LeftClick(DataContainer.xpath_loginbtn);
                driver.SwitchWindow();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void EditTradeQuantityInGrid(
         WindowsDriver<WindowsElement> driver,
         string gridName,
         string accountNameElementId,
         string tradeQuantityElementId,
         string newTradeQuantityValue)
        {
            try
            {
               
                AppiumWebElement grid = driver.FindElementByAccessibilityId(gridName); 

                if (grid != null)
                {
                    AppiumWebElement previousAccountNameElement = null; 

                    foreach (AppiumWebElement row in grid.FindElementsByClassName("DataItem")) 
                    {
                       
                        AppiumWebElement accountNameElement = row.FindElementByAccessibilityId(accountNameElementId); 
                        AppiumWebElement tradeQuantityElement = row.FindElementByAccessibilityId(tradeQuantityElementId); 

                        if (accountNameElement != null && tradeQuantityElement != null)
                        {
                            string accountName = accountNameElement.Text;

                            if (previousAccountNameElement != null && accountName == previousAccountNameElement.Text)
                            {
                               
                                tradeQuantityElement.Click();

                                
                                driver.Keyboard.SendKeys(newTradeQuantityValue);

                               
                                break;
                            }

                            // Store the current account name element for comparison in the next iteration.
                            previousAccountNameElement = accountNameElement;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

           public static Dictionary<string, List<int>> ReadDataFromExcel(string filePath, string sheetName)
        {
            var indexDictionary = new Dictionary<string, List<int>>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];

                if (worksheet != null)
                {
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    var columnNames = new List<string>();

                    // Read column names from the first row
                    for (int colIndex = 1; colIndex <= colCount; colIndex++)
                    {
                        columnNames.Add(worksheet.Cells[1, colIndex].Text);
                    }

                    // Initialize dictionary with empty lists
                    foreach (var columnName in columnNames)
                    {
                        indexDictionary[columnName] = new List<int>();
                    }

                    // Read data rows starting from the second row
                    for (int rowIndex = 2; rowIndex <= rowCount; rowIndex++)
                    {
                        for (int colIndex = 1; colIndex <= colCount; colIndex++)
                        {
                            string columnName = columnNames[colIndex - 1];
                            string cellValue = worksheet.Cells[rowIndex, colIndex].Text;
                            List<int> cellValues = cellValue.Split(',').Select(int.Parse).ToList();
                            indexDictionary[columnName].AddRange(cellValues);
                        }
                    }
                }
            }

            return indexDictionary;
        }
            
    }
}
