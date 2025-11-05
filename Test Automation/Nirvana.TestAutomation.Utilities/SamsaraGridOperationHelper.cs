using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using Nirvana.TestAutomation.Utilities;
using System.Threading;
using System.Configuration;
using System.Collections.ObjectModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Drawing;

namespace Nirvana.TestAutomation.Utilities
{
    public class GridRow
    {
        public int RowIndex { get; set; }
        public Dictionary<int, string> ColumnValues { get; set; }

        public GridRow(int rowIndex)
        {
            RowIndex = rowIndex;
            ColumnValues = new Dictionary<int, string>();
        }
        

    }

    class SamsaraGridOperationHelper
    {
        public static Dictionary<string, int> columnGridIndexMapping = new Dictionary<string, int>();
        private static int maxRowIndex = 1;
        private static List<GridRow> gridData = new List<GridRow>();
        private static Dictionary<string, string> prevGridCellElements = new Dictionary<string, string>();
        private static Dictionary<int, string> columnIndexMapping = new Dictionary<int, string>();
        public static int CheckRowCountOnUIGrid(IWebDriver driver, string id)
        {
            int rowCount = 0;
            string xpath = string.Empty;
            if (id.Contains("cellid1r")) 
            {
                xpath = SamsaraHelperClass.SamsaraXpath("SubOrderGridAGgridID", "Blotter");
            }
            for (int i = 2; i < int.MaxValue; i++)
            {
                 //tempCellId = id;
                string tempCellId = string.Format(xpath, i, 2);
                Console.WriteLine(tempCellId);

                try
                {
                    //Actions actions = new Actions(driver);
                    IWebElement element4 = driver.FindElement(By.XPath(tempCellId));
                    rowCount++;
                }
                catch (NoSuchElementException)
                {

                    return rowCount;
                }
            }

            return rowCount;
        }

        public static void PerformActionsOnGrid(IWebDriver driver, DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                //GenericMthodperformAnyActionOnGrid
            }
        }

        public static void EditColumnChooser(WebDriver driver, IWebElement columnChooserWindow, IWebElement inputColumn, string columnName, Actions actions)
        {
            actions.MoveToElement(inputColumn).DoubleClick().Perform();
            for(int j = 0; j < 20; j++)
            {
                actions.MoveToElement(inputColumn).SendKeys(Keys.Backspace).Perform();
            }
            DataUtilities.clearTextData();
            actions.MoveToElement(inputColumn).SendKeys(columnName).SendKeys(Keys.Tab).Perform();
            Thread.Sleep(2000);
            IList<IWebElement> divElements = columnChooserWindow.FindElements(By.TagName("span"));
            foreach (var ele in divElements) 
            {
                if (ele.Text.ToString().ToLower().Equals(columnName.ToLower()))
                {
                    ele.Click();
                    break;
                }
            }
        }

        public static DataTable GetAGgridData(WebDriver driver) 
        {
            DataTable agGridDataTable = new DataTable();
            var headerCells = driver.FindElements(By.XPath("//div[@class='ag-header-cell-label']//span[@class='ag-header-cell-text']"));
            foreach (var headerCell in headerCells)
            {
                agGridDataTable.Columns.Add(headerCell.Text);
            }

            var rows = driver.FindElements(By.XPath("//div[@class='ag-center-cols-container']//div[@role='row']"));

            // Populate DataTable with Row Data
            foreach (var row in rows)
            {
                var cells = row.FindElements(By.XPath(".//div[@role='gridcell']"));
                cells[0].Click();
                DataRow dataRow = agGridDataTable.NewRow();
                for (int j = 0; j < cells.Count; j++)
                {
                    dataRow[j] = cells[j].Text;
                }
                agGridDataTable.Rows.Add(dataRow);
            }
            return agGridDataTable;
        }

        public static Dictionary<string, int> GetAllCellValues(IWebDriver driver, string cellID)
        {
            Dictionary<string, int> cellValues = new Dictionary<string, int>();
            try
            {
                IReadOnlyCollection<IWebElement> cellElements = driver.FindElements(By.CssSelector("[id^=" + cellID + "]"));
                int cellNum = 0;
                foreach (var cellElement in cellElements)
                {
                    string cellId = cellElement.GetAttribute("id");
                    string cellValue = cellElement.Text;
                    cellValues.Add(cellValue, cellNum);
                    cellNum++;
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            return cellValues;
        }

        public static void EditAddFillsDetails(IWebDriver driver, DataTable dt, string stepName, string DefaultCellValue, Dictionary<string, object> dict)
        {
            try
            {
                IWebElement element = driver.FindElement(By.Id(dict["Add/ModifyFillsID"].ToString()));
                string id1 = string.Empty;
                var allElements = element.FindElements(By.XPath(".//*"));
                var columnHeaderElements = allElements.Where(e => e.GetAttribute("role") == "columnheader").ToList();
                foreach (var elements in columnHeaderElements)
                {
                    string elementId = elements.GetAttribute("id");
                    id1 = elementId.Substring(0, elementId.IndexOf("r") + 1);
                    break;
                }
                DefaultCellValue = "#" + id1;
                string cell = string.Equals(DefaultCellValue.Substring(0, 1), "#", StringComparison.OrdinalIgnoreCase) ? DefaultCellValue.Substring(1) : DefaultCellValue;
                int i = CheckRowCountOnUIGrid(driver, "#" + cell);
                Dictionary<string, int> cellColumnValues = GetAllCellValues(driver, cell + "1c");

                foreach (DataRow dr in dt.Rows)
                {
                    bool newLine = true;
                    if (!string.IsNullOrEmpty(dr["New"].ToString()))
                    {
                        if (string.Equals(dr["New"].ToString().ToLower(), "false", StringComparison.OrdinalIgnoreCase))
                        {
                            newLine = false;
                        }
                    }
                    if (!string.IsNullOrEmpty(dr["Fill"].ToString()))
                    {
                        try
                        {
                            string id = DefaultCellValue + ((newLine == false) ? (i = 2).ToString() : (i + 1).ToString()) + "c" + cellColumnValues["Fill"];

                            int maxTry = 20;
                            // string id = DefaultCellValue + ((newLine == false) ? i.ToString() : (i + 1).ToString()) + "c" + cellColumnValues["Fill"];

                            Actions actions = new Actions(driver);
                            IList<IWebElement> divElements = driver.FindElements(By.CssSelector(id));
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
                                            spanElement.Click();
                                            Thread.Sleep(1000);
                                            actions.MoveToElement(spanElement)
                                            .KeyDown(Keys.Control)
                                            .SendKeys("a")
                                            .KeyUp(Keys.Control).SendKeys(Keys.Backspace)
                                            .Perform();

                                            actions.SendKeys(dr["Fill"].ToString())
                                                   .SendKeys(Keys.Enter)
                                                   .Build()
                                                   .Perform();

                                            Thread.Sleep(1000);
                                            Console.WriteLine(spanElement.Text);

                                            string value = spanElement.Text.ToString();
                                            string cleanedString = value.Replace(",", "");
                                            int idx = cleanedString.IndexOf(".");
                                            cleanedString = idx < 0 ? cleanedString : cleanedString.Substring(0, idx);
                                            if (cleanedString.Contains(dr["Fill"].ToString()))
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
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());

                        }
                    }



                    if (!string.IsNullOrEmpty(dr["Last Fill Price (Local)"].ToString()))
                    {
                        try
                        {
                            string id = DefaultCellValue + ((newLine == false) ? (i = 2).ToString() : (i + 1).ToString()) + "c" + cellColumnValues["Last Fill Price (Local)"];

                            int maxTry = 20;
                            // string id = DefaultCellValue + ((newLine == false) ? i.ToString() : (i + 1).ToString()) + "c" + cellColumnValues["Fill"];

                            Actions actions = new Actions(driver);
                            IList<IWebElement> divElements = driver.FindElements(By.CssSelector(id));
                            foreach (var divElement in divElements)
                            {
                                string existingValue = divElement.Text;
                                Console.WriteLine("Existing value in cell " + id + " : " + existingValue);


                                IList<IWebElement> spanElements = divElement.FindElements(By.TagName("span"));
                                bool rightValue = false;

                                foreach (var spanElement in spanElements)
                                {
                                    while (!rightValue && maxTry > 0)
                                    {
                                        try
                                        {
                                            spanElement.Click();
                                            Thread.Sleep(1000);
                                            actions.MoveToElement(spanElement)
                                            .KeyDown(Keys.Control)
                                            .SendKeys("a")
                                            .KeyUp(Keys.Control).SendKeys(Keys.Backspace)
                                            .Perform();

                                            actions.SendKeys(dr["Last Fill Price (Local)"].ToString())
                                                   .SendKeys(Keys.Enter)
                                                   .Build()
                                                   .Perform();

                                            Thread.Sleep(1000);
                                            Console.WriteLine(spanElement.Text);
                                            string value = spanElement.Text.ToString();
                                            string cleanedString = value.Replace(",", "");
                                            if (cleanedString.Contains(dr["Last Fill Price (Local)"].ToString()))
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
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());

                        }
                    }
                    if (newLine)
                        i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void AllocateFromBlotterGridFill(IWebDriver driver, DataTable dt, string stepName, string cell, ref Dictionary<string, object> dict)
        {
            try
            {
                DataTable data = dt.Copy();
                string cellId = cell + "1c";
                string customCellID = cell.Substring(1);

                DataTable dt2 = dt.Clone();

                string filepath = ConfigurationManager.AppSettings["-columnMappingFile"].ToString();
                string sheetName = stepName;
                DataSet columMapDs = DataUtilities.GetTestCaseTestData(filepath, 1, 1, new List<string>());

                SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref sheetName, columMapDs.Tables["VerificationHandler"], ref data);

                Dictionary<string, int> cellColumnValues = GetAllCellValues(driver, cellId.Substring(1));

                int rowcount = dt.Rows.Count;


                int i = 2;
                try
                {
                    foreach (DataRow dr in data.Rows)
                    {
                        int maxTry = 20;

                        if (dr.Table.Columns.Contains("PreferenceName"))
                        {
                            if (!string.IsNullOrEmpty(dr["PreferenceName"].ToString()))
                            {
                                try
                                {

                                    string newValues = dr["PreferenceName"].ToString();

                                    IWebElement dropdownElement = driver.FindElement(By.XPath(dict["AllocationPrefButton"].ToString()));
                                    if (dropdownElement.Enabled)
                                    {
                                        bool valueInputted = false;
                                        int attempts = 0;
                                        const int maxAttempts = 1;

                                        while (!valueInputted && attempts < maxAttempts)
                                        {
                                            try
                                            {

                                                dropdownElement.Click();
                                                Actions actions2 = new Actions(driver);
                                                actions2.DoubleClick(dropdownElement).Perform();
                                                actions2.SendKeys(newValues).SendKeys(Keys.Tab).Perform();
                                                Console.WriteLine("Value: " + newValues);
                                                if (dropdownElement.GetAttribute("value") == newValues)
                                                {
                                                    valueInputted = true;
                                                    Console.WriteLine("Value inputted successfully.");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Value not inputted. Retrying...");
                                                    attempts++;
                                                }
                                            }
                                            catch (ElementNotInteractableException ex)
                                            {
                                                Console.WriteLine("Element is not interactable: " + ex.Message);
                                                break;
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("Element is not interactable: " + ex.Message);
                                                break;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        Console.WriteLine("Dropdown element is not typable.");
                                    }
                                }

                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                            }
                        }


                        if (dr.Table.Columns.Contains("Account"))
                        {

                            if (!string.IsNullOrEmpty(dr["Account"].ToString()))
                            {
                                int maxRetryCount = 2;
                                int currentRetry = 0;
                                bool success = false;


                                while (!success && currentRetry < maxRetryCount)
                                {
                                    try
                                    {
                                        string newValue = dr["Account"].ToString();
                                        Thread.Sleep(3000);
                                        IWebElement searchBarOpen = driver.FindElement(By.XPath(dict["SearchAccountSelector"].ToString()));
                                        //IReadOnlyCollection<IWebElement> searchBar = driver.FindElements(By.XPath(dict["SearchAccountSelector"].ToString()));
                                        searchBarOpen.Click();
                                        IWebElement selectAllAcc = driver.FindElement(By.XPath(dict["SelectAll_Filter"].ToString()));
                                        selectAllAcc.Click();
                                        IWebElement searchBarElement = driver.FindElement(By.XPath(dict["SearchAccount"].ToString()));
                                        searchBarElement.Click();
                                        searchBarElement.Clear();
                                        searchBarElement.SendKeys(newValue);
                                        searchBarElement.SendKeys(Keys.Tab);
                                        IWebElement selectAccCheckBox = driver.FindElement(By.XPath(dict["AccountSelector"].ToString()));
                                        selectAccCheckBox.Click();
                                        IWebElement ApplyBtn = driver.FindElement(By.XPath(dict["Filter_ApplyButton"].ToString()));
                                        ApplyBtn.Click();
                                        success = true;//proper verification left


                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                    currentRetry++;
                                    Thread.Sleep(1000); // Wait before retrying
                                }

                                if (success)
                                {
                                    Console.WriteLine("Adding Account on to Allocate Operation Succeeded.");
                                }
                                else
                                {
                                    try
                                    {
                                        string newValue = dr["Account"].ToString();
                                        Thread.Sleep(3000);
                                        IWebElement searchBarElement = driver.FindElement(By.XPath(dict["SearchAccount"].ToString()));
                                        searchBarElement.Click();
                                        searchBarElement.Clear();
                                        searchBarElement.SendKeys(newValue);
                                        searchBarElement.SendKeys(Keys.Tab);
                                        Actions actions = new Actions(driver);
                                        actions.SendKeys(Keys.Space).Perform();
                                    }
                                    catch (Exception) { }
                                }


                            }
                        }
                        if (dr.Table.Columns.Contains("Target Alloc. %"))
                        {
                            if (!string.IsNullOrEmpty(dr["Target Alloc. %"].ToString()))
                            {
                                try
                                {
                                    string id = cell + i + "c" + cellColumnValues["Target Alloc. %"];
                                    Actions actions = new Actions(driver);
                                    IWebElement divElement = driver.FindElement(By.CssSelector(id));

                                    IList<IWebElement> spanElements = divElement.FindElements(By.TagName("span"));
                                    foreach (var spanElement in spanElements)
                                    {
                                        try
                                        {
                                            bool rightValue = false;
                                            while (!rightValue && maxTry > 0)
                                            {
                                                //actions.DoubleClick(spanElement).Build()
                                                      // .Perform();
                                                spanElement.Click();
                                                Thread.Sleep(1000);
                                                actions.SendKeys(dr["Target Alloc. %"].ToString())
                                                       .SendKeys(Keys.Enter)
                                                       .Build()
                                                       .Perform();
                                                Thread.Sleep(1000);
                                                Console.WriteLine(spanElement.Text);

                                                string value = spanElement.Text.ToString();
                                                string cleanedString = value.Replace(",", "");
                                                if (cleanedString.Contains(dr["Target Alloc. %"].ToString()))
                                                {

                                                    rightValue = true;

                                                }
                                                maxTry--;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }

                                    }
                                }

                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        if (dr.Table.Columns.Contains("Target Alloc. Qty"))
                        {
                            if (!string.IsNullOrEmpty(dr["Target Alloc. Qty"].ToString()))
                            {
                                try
                                {
                                    string id = cell + i + "c" + cellColumnValues["Target Alloc. Qty"];
                                    Actions actions = new Actions(driver);
                                    IWebElement divElement = driver.FindElement(By.CssSelector(id));

                                    IList<IWebElement> spanElements = divElement.FindElements(By.TagName("span"));
                                    foreach (var spanElement in spanElements)
                                    {
                                        bool rightValue = false;
                                        try
                                        {
                                            while (!rightValue && maxTry > 0)
                                            {
                                                //actions.DoubleClick(spanElement).Build()
                                                // .Perform();
                                                spanElement.Click();
                                                Thread.Sleep(1000);
                                                actions.SendKeys(dr["Target Alloc. Qty"].ToString())
                                                       .SendKeys(Keys.Enter)
                                                       .Build()
                                                       .Perform();

                                                Thread.Sleep(1000);                                                
                                                Console.WriteLine(spanElement.Text);

                                                string value = spanElement.Text.ToString();
                                                string cleanedString = value.Replace(",", "");
                                                if (cleanedString.Contains(dr["Target Alloc. Qty"].ToString()))
                                                {

                                                    rightValue = true;

                                                }
                                                maxTry--;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }


                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                            }
                        }

                        //  i++;
                        if (dr.Table.Columns.Contains(TestDataConstants.RESET_COL))
                        {
                            if (dr[TestDataConstants.RESET_COL].ToString() != String.Empty)
                            {
                                IWebElement resetElement = driver.FindElement(By.XPath(dict["Reset_Allocation"].ToString()));
                                resetElement.Click();
                            }
                        }
                    }
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



        public static DataRow DeleteNonMatchingColumnsValues(DataRow row, DataTable dataTable)
        {
            DataRow newRow = dataTable.NewRow();
            try
            {

                foreach (DataColumn column in dataTable.Columns)
                {
                    string columnName = column.ColumnName;

                    if (row.Table.Columns.Contains(columnName))
                    {
                        newRow[columnName] = row[columnName];
                    }
                    else
                    {
                        newRow[columnName] = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return newRow;
        }

        public static void AddOrRemoveColumnWithVerification(IWebDriver driver, DataTable dt, string stepName, string cell, ref Dictionary<string, object> dict)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //IWebElement dropdownElement = driver.FindElement(By.XPath(dict["AllocationPrefButton"].ToString()));

                    IWebElement AddColumn = driver.FindElement(By.XPath(dict[dr["GridType"].ToString() + "AddColumn"].ToString()));
                    IWebElement AddColumnInputText = driver.FindElement(By.XPath(dict[dr["GridType"].ToString() + "AddColumnInputText"].ToString()));
                    // AddColumn.Click();
                    Actions actions24 = new Actions(driver);
                    Thread.Sleep(4000);
                    IWebElement SelectAll = driver.FindElement(By.XPath(dict["SelectAll"].ToString()));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", SelectAll);
                    //ReadOnlyCollection<IWebElement> checkboxes2 = driver.FindElements(By.XPath("//label[text()='(Select All)']/preceding-sibling::div/input[@type='checkbox']"));

                    string addOrRemoveColumnList = dr["AddOrRemoveColumnList"].ToString();

                    List<string> columnsList = addOrRemoveColumnList.Split(',').ToList();
                    foreach (string val in columnsList)
                    {
                        AddColumn.Click();
                        try
                        {
                            AddColumnInputText.Clear();
                            Thread.Sleep(3000);
                            AddColumnInputText.SendKeys(val);
                            Thread.Sleep(3000);
                        }
                        catch
                        {
                            AddColumn.Click();
                            Thread.Sleep(3000);
                            AddColumnInputText.Clear();
                            Thread.Sleep(3000);
                            AddColumnInputText.SendKeys(val);
                            Thread.Sleep(3000);
                        }

                        string valCheckBox = dict["AddRemoveColumnCheckBox"].ToString();
                        valCheckBox = valCheckBox.Replace("Symbol", val);




                        try
                        {
                            if (string.Equals("Add", dr["Action"].ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    IWebElement box = driver.FindElement(By.XPath(valCheckBox));
                                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", box);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    IWebElement box = driver.FindElement(By.XPath(valCheckBox));
                                    IWebElement checkbox = driver.FindElement(By.XPath(valCheckBox));
                                    bool isChecked = checkbox.Selected;
                                    if (isChecked)
                                    {
                                        while (isChecked)
                                        {
                                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);
                                            Console.WriteLine(val + "is unchecked");
                                            isChecked = checkbox.Selected;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(val + "is unchecked as action is Remove");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            if (string.Equals("TRUE", dr["Verify"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    IWebElement ExportButton = driver.FindElement(By.XPath(dict[dr["GridType"].ToString() + "ExportButton"].ToString()));
                                    ExportButton.Click();
                                    Thread.Sleep(6000);
                                    DataTable mainTable = null;
                                    DataSet Ds = null;
                                    string fileName = ConfigurationManager.AppSettings["-BlotterExportDefaultFileName"].ToString();
                                    string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                                    string filePath2 = SamsaraHelperClass.SearchFile(downloadsPath, fileName);

                                    if (!string.IsNullOrEmpty(filePath2))
                                    {
                                        if (string.Equals("SUBORDER", dr["GridType"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                                        {
                                            mainTable = SamsaraHelperClass.getGroupedData(filePath2);
                                        }
                                        else
                                        {
                                            Ds = ExportedData(ConfigurationManager.AppSettings["-BlotterExportDefaultFileName"].ToString());
                                        }
                                    }


                                    if (mainTable == null)
                                    {
                                        if (string.Equals("ORDER", dr["GridType"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                                        {
                                            mainTable = Ds.Tables.Contains("Orders") ? Ds.Tables["Orders"] : (Ds.Tables.Count > 0 ? Ds.Tables[0] : null);
                                        }
                                        else if (string.Equals("WORKING", dr["GridType"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                                        {
                                            mainTable = Ds.Tables.Contains("Working") ? Ds.Tables["Working"] : (Ds.Tables.Count > 0 ? Ds.Tables[1] : null);
                                        }

                                    }

                                    bool allColumnsExist = true;

                                    if (columnsList.Count > 0)
                                    {
                                        foreach (string columnName in columnsList)
                                        {
                                            if (!mainTable.Columns.Contains(columnName))
                                            {
                                                if (string.Equals("REMOVE", dr["Action"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                                                    allColumnsExist = false;
                                                break;
                                            }
                                        }

                                        if (!allColumnsExist)
                                        {

                                            Console.WriteLine("Column Verification Failed.");
                                        }
                                        else
                                        { Console.WriteLine("Column Verification Passed."); }
                                    }

                                    else if (columnsList.Count == 0)
                                    {
                                        if (mainTable.Rows.Count == 0)
                                        {
                                            Console.WriteLine("Column Verification Passed.");
                                        }
                                    }




                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }


                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        public static DataSet ExportedData(string input)
        {

            try
            {
                string fileName = input;
                string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string filePath2 = SamsaraHelperClass.SearchFile(downloadsPath, fileName);
                if (!string.IsNullOrEmpty(filePath2))
                {
                    List<string> list = new List<string>();
                    DataSet ds = DataUtilities.GetTestCaseTestData(filePath2, 1, 1, list);
                    return ds;
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Exporting");
            }
            return null;
        }

        public static bool SortGridOnBlotter(IWebDriver driver, DataTable dtable, string stepName, ref Dictionary<string, object> dict)
        {
            bool success = false;
            try
            {
                foreach (DataRow dr in dtable.Rows)
                {

                    IWebElement AddColumn = driver.FindElement(By.XPath(dict[dr["GridType"].ToString() + "AddColumn"].ToString()));
                    IWebElement AddColumnInputText = driver.FindElement(By.XPath(dict[dr["GridType"].ToString() + "AddColumnInputText"].ToString()));
                    //  AddColumn.Click();
                    Actions actions24 = new Actions(driver);
                    Thread.Sleep(4000);
                    IWebElement SelectAll = driver.FindElement(By.XPath(dict["SelectAll"].ToString()));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", SelectAll);

                    string addOrRemoveColumnList = dr["ColumnsForSorting"].ToString();

                    List<string> columnsList = addOrRemoveColumnList.Split(',').ToList();
                    foreach (string val in columnsList)
                    {
                        AddColumn.Click();
                        try
                        {
                            AddColumnInputText.Clear();
                            Thread.Sleep(3000);
                            AddColumnInputText.SendKeys(val);
                            Thread.Sleep(3000);
                        }
                        catch
                        {
                            AddColumn.Click();
                            Thread.Sleep(3000);
                            AddColumnInputText.Clear();
                            Thread.Sleep(3000);
                            AddColumnInputText.SendKeys(val);
                            Thread.Sleep(3000);
                        }
                        string valCheckBox = dict["AddRemoveColumnCheckBox"].ToString();


                        valCheckBox = valCheckBox.Replace("Symbol", val);
                        try
                        {

                            IWebElement box = driver.FindElement(By.XPath(valCheckBox));
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", box);


                        }
                        catch (Exception ex)
                        {
                            success = false;
                            Console.WriteLine(ex.Message);
                        }

                    }


                    //sort
                    if (string.Equals("THREEDOTS", dr["Action"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            Dictionary<string, string> Iddict = SamsaraHelperClass.ConvertDataTableToDictionary(SamsaraHelperClass.DataTables["GridIDMappings"]);

                            string idToReplace = Iddict[dr["GridType"].ToString()];

                            foreach (string val in columnsList)
                            {

                                IList<IWebElement> foundElements = GetElementsWithCustomXPath(driver, val, ref idToReplace, ref dict, "DefaultBlotterCustomExpressionSortGridThreeDots");
                                foreach (IWebElement element in foundElements)
                                {
                                    try
                                    {
                                        element.Click();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                    Thread.Sleep(6000);
                                    Console.WriteLine("Text inside the element: " + element.Text);

                                    if (string.Equals("ASC", dr["SortType"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                                    {
                                        string ascXPath = dict["SortAsc"].ToString();
                                        driver.FindElement(By.XPath(ascXPath)).Click();
                                        Thread.Sleep(6000);
                                        element.Click();
                                    }

                                    else
                                    {
                                        string descXPath = dict["SortDesc"].ToString();
                                        driver.FindElement(By.XPath(descXPath)).Click();
                                        Thread.Sleep(6000);
                                        element.Click();

                                    }
                                }


                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else if (string.Equals("SORTBYCLICK", dr["Action"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {

                            Dictionary<string, string> Iddict = SamsaraHelperClass.ConvertDataTableToDictionary(SamsaraHelperClass.DataTables["GridIDMappings"]);

                            string idToReplace = Iddict[dr["GridType"].ToString()];

                            foreach (string val in columnsList)
                            {

                                IList<IWebElement> foundElements = GetElementsWithCustomXPath(driver, val, ref idToReplace, ref dict);
                                foreach (IWebElement element in foundElements)
                                {
                                    Console.WriteLine("Text inside the element: " + element.Text);

                                    if (string.Equals("ASC", dr["SortType"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                                        element.Click();

                                    else
                                    {
                                        element.Click();
                                        element.Click();

                                    }
                                }


                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    //verify
                    /*
                    if (string.Equals("TRUE", dr["Verify"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {



                            IWebElement ExportButton = driver.FindElement(By.XPath(dict[dr["GridType"].ToString() + "ExportButton"].ToString()));
                            ExportButton.Click();
                            DataTable mainTable = null;
                            DataSet Ds = null;
                            string fileName = ConfigurationManager.AppSettings["-BlotterExportDefaultFileName"].ToString();
                            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                            string filePath2 = SamsaraHelperClass.SearchFile(downloadsPath, fileName);

                            if (!string.IsNullOrEmpty(filePath2))
                            {
                                if (string.Equals("SUBORDER", dr["GridType"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                                {
                                    mainTable = SamsaraHelperClass.getGroupedData(filePath2);
                                }
                            }
                            else
                            {
                                Ds = ExportedData(ConfigurationManager.AppSettings["-BlotterExportDefaultFileName"].ToString());
                            }

                            if (mainTable == null)
                            {
                                if (string.Equals("ORDER", dr["GridType"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                                {
                                    mainTable = Ds.Tables.Contains("Orders") ? Ds.Tables["Orders"] : (Ds.Tables.Count > 0 ? Ds.Tables[0] : null);
                                }
                                else if (string.Equals("WORKING", dr["GridType"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                                {
                                    mainTable = Ds.Tables.Contains("Working") ? Ds.Tables["Working"] : (Ds.Tables.Count > 0 ? Ds.Tables[1] : null);
                                }

                            }


                            if (!string.IsNullOrEmpty(filePath2))
                            {
                                SamsaraHelperClass.DeleteFile(filePath2);
                                Console.WriteLine("File " + fileName + " found and deleted successfully.");

                            }


                            //SamsaraHelperClass.DataTables

                            success = true;
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            Console.WriteLine(ex.Message);
                        }
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                success = false;
                Console.WriteLine(ex.Message);
            }


            return success;


        }

        public static IList<IWebElement> GetElementsWithCustomXPath(IWebDriver driver, string columnsForSorting, ref string idToReplace, ref Dictionary<string, object> dict, string xpath = "DefaultBlotterCustomExpressionSortGrid")
        {
            IList<IWebElement> elements = null;
            try
            {
                string customXPath = dict[xpath].ToString();
                customXPath = customXPath.Replace("Symbol", columnsForSorting);
                customXPath = customXPath.Replace("cellid0r", idToReplace);
                elements = driver.FindElements(By.XPath(customXPath));

                return elements;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return elements;
        }

        public static bool EnsureGridWindowActionAndSwitchToChildWindow(WebDriver driver, string parentWindowName, string DashBoardXpath, string ButtonXpath,bool maximizeCompulsory= false)
        {
            string originalWindow = driver.CurrentWindowHandle;
            IList<string> windowHandles = driver.WindowHandles;

            foreach (string handle in windowHandles)
            {
                driver.SwitchTo().Window(handle);
                if (driver.Title.Contains(parentWindowName))
                {
                    Console.WriteLine("Switched to window with title:" + driver.Title);
                    int retries = 0;
                    while (retries < 2)
                    {
                        try
                        {
                            var element = driver.FindElements(By.XPath(DashBoardXpath));
                            if (element.Count > 0)
                            {
                                Console.WriteLine(element.Count);
                                Console.WriteLine("Element found in the window.");
                                int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                                int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                                int windowWidth = Convert.ToInt32(js.ExecuteScript("return window.innerWidth;"));
                                int windowHeight = Convert.ToInt32(js.ExecuteScript("return window.innerHeight;"));
                                Console.WriteLine("Screen Resolution: " + screenWidth + "x" + screenHeight);
                                Console.WriteLine("Browser Window Size: " + windowWidth + "x" + windowHeight);
                                int thresholdWidth = (int)(screenWidth * 0.75);
                                int thresholdHeight = (int)(screenHeight * 0.75);
                                if (windowWidth < thresholdWidth || windowHeight < thresholdHeight || maximizeCompulsory)
                                {
                                    Console.WriteLine("Window is less than three-fourths of the screen. Clciking on Button now...");
                                    var mButton = driver.FindElement(By.XPath(ButtonXpath));
                                    mButton.Click();
                                }
                                else
                                {
                                    Console.WriteLine("Window is already more than three-fourths of the screen.");
                                }
                                return true;


                            }
                            else
                            {
                                ++retries;
                            }

                        }
                        catch (WebDriverTimeoutException)
                        {
                            Console.WriteLine("Element not found in this window. Retrying...");
                            retries++;
                            Thread.Sleep(500);
                        }
                        catch (WebDriverException e)
                        {
                            Console.WriteLine("WebDriverException: " + e.Message);
                            retries++;
                            Thread.Sleep(500);
                        }

                    }
                }
            }
            driver.SwitchTo().Window(originalWindow);
            Console.WriteLine("Element not found in any window. Switched back to the original window.");
            return false;
        }
        public static void IncreaseOrUnwindPositionRTPNL(WebDriver driver, ref Dictionary<string, object> dict, string switchWindowTo, DataTable excelData )
        {
            try
            {
                SwitchWindow.SwitchToWindow(driver,switchWindowTo);

                List<string> OptionLists = new List<string>();

                if (!string.IsNullOrEmpty(excelData.Rows[0][TestDataConstants.COL_VERIFYOPTIONLIST].ToString()))
                {
                    OptionLists = excelData.Rows[0][TestDataConstants.COL_VERIFYOPTIONLIST].ToString().Split(',').ToList();
                    foreach (string name in OptionLists)
                    {
                        if (name.ToLower().Contains("increase"))
                        {
                            try
                            {
                                var element = driver.FindElements(By.XPath(dict["IncreasePositionButton"].ToString()));
                                if (element.Count > 0)
                                {
                                    Console.WriteLine("IncreasePositionButton Exist");
                                }
                                else
                                      throw new Exception("IncreasePositionButton doesnot Exist");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("IncreaseOrUnwindPositionRTPNL VerifyOptionsList operation failed: " + ex.Message);
                            }
                        }
                         if(name.ToLower().Contains("unwind"))
                         {
                             try
                             {
                                 var element2 = driver.FindElements(By.XPath(dict["UnwindPositionButton"].ToString()));
                                 if (element2.Count > 0)
                                 {
                                     Console.WriteLine("UnwindPositionButton Exist");
                                 }
                                 else
                                     throw new Exception("UnwindPositionButton doesnot Exist");
                             }
                             catch (Exception ex)
                             {
                                 Console.WriteLine("IncreaseOrUnwindPositionRTPNL VerifyOptionsList operation failed: " + ex.Message);
                             }

                         }
                         else if (name.ToLower().Contains("adjust")) {
                             var element = driver.FindElements(By.XPath(dict["AdjustPositionButton"].ToString()));
                             if (element.Count > 0)
                             {
                                 Console.WriteLine("AdjustPositionButton Exist");
                             }
                             else
                                 throw new Exception("AdjustPositionButton doesnot Exist");
                         
                         }

                    }
                    
                }

                if (!string.IsNullOrEmpty(excelData.Rows[0][TestDataConstants.COL_POSITION_ACTION].ToString()))
                {
                    string sol = excelData.Rows[0][TestDataConstants.COL_POSITION_ACTION].ToString().ToLower();
                   
                    if (sol.Contains("adjust"))
                    {
                        driver.FindElement(By.XPath(dict["AdjustPositionButton"].ToString())).Click();
                        Thread.Sleep(2000);
                        string secondaction = sol.Split(',')[1];
                        if (!string.IsNullOrEmpty(secondaction))
                        {
                            IWebElement ele = driver.FindElement(By.ClassName(dict["AdjustActionButtons"].ToString()));
                            IList<IWebElement> divElements = ele.FindElements(By.TagName("button"));
                            foreach (var e in divElements) {
                                if (e.Text.ToLower().Contains(secondaction.ToLower())) 
                                {
                                    e.Click();
                                    Console.WriteLine("Clicking on " + e.Text.ToString());
                                }
                            }
                        }
                    }
					// Previous code click on unwind button even if we are passing Increase position value
                    else if (sol.Contains("increase"))
                    {
                        driver.FindElement(By.XPath(dict["IncreasePositionButton"].ToString())).Click();
                    }
                    else
                    {
                        driver.FindElement(By.XPath(dict["UnwindPositionButton"].ToString())).Click();
                    }
                }

                if(excelData.Columns.Contains("Disable/Grey buttons") && !string.IsNullOrEmpty(excelData.Rows[0]["Disable/Grey buttons"].ToString()))
                {
                    string buttonName = excelData.Rows[0]["Disable/Grey buttons"].ToString().ToLower();
                    string xpath = string.Empty;
                    if (buttonName.Contains("increase")) 
                    {
                        xpath = dict["IncreasePositionButton"].ToString();
                    }
                    else if (buttonName.Contains("adjust"))
                    {
                        xpath = dict["AdjustPositionButton"].ToString();
                    }
                    else {
                        xpath = dict["UnwindPositionButton"].ToString();
                    }
                    IWebElement button = driver.FindElement(By.XPath(xpath));

                    bool isDisabled = button.GetAttribute("disabled") != null;
                    if (isDisabled)
                    {
                        Console.WriteLine(buttonName + " is disabled");
                    }
                    else {
                        throw new Exception(buttonName + " is not disabled");
                    }
                }

            }

           catch (Exception ex)
           {
             Console.WriteLine(" IncreaseOrUnwindPositionRTPNL operation failed: " + ex.Message);
             bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
               if (rethrow)
               throw;
           }
        }

        public static void ClickElement(WebDriver driver, string action, ref Dictionary<string, object> dict, string tabTitle = null, DataTable UIData = null, List<string> ActionList = null)
        {
            switch (action)
            {
                case "ClickElementOnTabPM":
                    ClickOnTabPM(driver, tabTitle, ref dict);
                    break;
                case "ExtractTabOutsideAndVerify":
                    ExtractTabOutsideAndVerify(driver, tabTitle, ref dict, ref UIData, ActionList);
                    break;

                case "HoldAndExtractElement":
                    HoldAndExtractElement(driver, ref dict, tabTitle);
                    break;
                    
                case "IncreaseOrUnwindPositionRTPNL":
                    IncreaseOrUnwindPositionRTPNL(driver, ref dict, tabTitle,UIData);
                    break;

                default:
                    if (action.Contains("Ensure"))
                    {
                        List<string> DList = tabTitle.Split(',').ToList();
                        if (DList.Count > 1)
                        {
                            try
                            {
                                string xbutton = action.Substring("Ensure".Length);
                                EnsureGridWindowActionAndSwitchToChildWindow(driver, DList[0], dict[DList[1] + "Dashboard"].ToString(), dict[xbutton].ToString());
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Exception occurred on EnsureGridWindowActionAndSwitchToChildWindow: " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Unsupported action: " + action);
                    }
                    break;

            }
        }

        public static void HoldAndExtractElement(WebDriver driver, ref Dictionary<string, object> dict, string tabTitle)
        {
            try
            {
                string xpath = dict["ClickAndOpenTab"].ToString();
                xpath = xpath.Replace("YOUR_TAB_TITLE", tabTitle);
                IWebElement maximizeButton;
                try
                {
                    maximizeButton = driver.FindElement(By.XPath(dict["MaximizeWindow"].ToString()));
                }
                catch 
                {
                    maximizeButton = driver.FindElement(By.XPath("//button[@title='Restore Window']"));
                }
                Console.WriteLine("Without Maximized window needed");

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                int windowWidth = Convert.ToInt32(js.ExecuteScript("return window.innerWidth;"));
                int windowHeight = Convert.ToInt32(js.ExecuteScript("return window.innerHeight;"));


                IWebElement tabElement = driver.FindElement(By.XPath(xpath));

                int targetX = windowWidth - 200;
                Point tabLocation = tabElement.Location;
                int targetY = -tabLocation.Y;
                Actions actions = new Actions(driver);

                bool isTab = tabElement.Enabled;
                actions.MoveToElement(tabElement).Perform();
                Console.WriteLine("Cursor Moved to Element");

                actions.ClickAndHold(tabElement);

                // actions.MoveByOffset(0, targetY);//- tabElement.Location.Y)
                //.Release();
                //.Perform();
                IWebElement targetElement = driver.FindElement(By.XPath(dict["TargetElement"].ToString()));
                Console.WriteLine(tabElement.Location.X + "," + tabElement.Location.Y);
                Console.WriteLine(targetElement.Location.X + "," + targetElement.Location.Y);
                Point startLocation = tabElement.Location;
                Point endLocation = targetElement.Location;
                actions.MoveToElement(targetElement).Perform();
                Thread.Sleep(5000);
                maximizeButton.Click();
                Console.WriteLine("Element Click)");
                actions.Release().Perform();
                Thread.Sleep(5000);
                Console.WriteLine("Element moved to target position successfully.");
                maximizeButton.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }



        public static void ExtractTabOutsideAndVerify(WebDriver driver, string tabTitle, ref Dictionary<string, object> dict, ref DataTable dtable, List<string> actionList)
        {
            const int maxRetries = 1;
            const int delayBetweenRetries = 1000;

            try
            {
                foreach (DataRow dr in dtable.Rows)
                {
                    bool success = false;
                    int attempt = 0;

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;
                        Console.WriteLine("Attempting again ....");

                        try
                        {
                            List<string> DashBoardList = dr[TestDataConstants.COL_DASHBOARDPARENTCHILD].ToString().Split(',').ToList();
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

                            EnsureGridWindowActionAndSwitchToChildWindow(driver, DashBoardList[0], dict[DashBoardList[1].ToString() + "Dashboard"].ToString(), dict["MaximizeWindow"].ToString());

                            string ExtractTabName = dr[TestDataConstants.COL_EXTRACTTABNAME].ToString();

                            string expectedColor = "";

                            if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_COLOR].ToString()))
                            {
                                expectedColor = dr[TestDataConstants.COL_COLOR].ToString();
                            }

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
                                ClickElement(driver, "ClickElementOnTabPM", ref dict, ExtractTabName);
                                SwitchWindow.SwitchToWindow(driver, ExtractTabName);
                                Thread.Sleep(2000);
                                DataTable Data = null; //before extract data
                                try
                                {
                                    Data = SamsaraGridOperationHelper.ExtractGridData(driver, ref dict, ExtractTabName + "Headers", ExtractTabName + "DataItems");
                                    if (Data != null)
                                    {
                                        SamsaraGridOperationHelper.CreateExcelFileFromDataTable(Data, ExtractTabName);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                                    if (rethrow)
                                        throw;
                                }
                                ////////////////////////////////////////////////////////////////////////////////////
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

                                EnsureGridWindowActionAndSwitchToChildWindow(driver, DashBoardList[0], dict[DashBoardList[1].ToString() + "Dashboard"].ToString(), dict["MaximizeWindow"].ToString());
                                Console.WriteLine("HoldAndExtract needs mini window hence minimizing it");
                                EnsureGridWindowActionAndSwitchToChildWindow(driver, DashBoardList[0], dict[DashBoardList[1].ToString() + "Dashboard"].ToString(), dict["MaximizeWindow"].ToString(),true);
                                
                                ClickElement(driver, "HoldAndExtractElement", ref dict, ExtractTabName);

                                Thread.Sleep(10000);
                               
                                List<string> afterSplitDashBoardList = dr[TestDataConstants.COL_AFTEREXTRACTDN].ToString().Split(',').ToList();
                                if (afterSplitDashBoardList.Count > 0)
                                {
                                    if (afterSplitDashBoardList.Count == 1)
                                    {
                                        SwitchWindow.SwitchToWindow(driver, afterSplitDashBoardList[0].ToString(), true);
                                    }
                                    else
                                    {
                                        SwitchWindow.SwitchToChildWindow(driver, afterSplitDashBoardList[0].ToString(), dict[afterSplitDashBoardList[1].ToString() + "Dashboard"].ToString());
                                    }
                                }
                                var maximizeButton = driver.FindElement(By.XPath(dict["MaximizeWindow"].ToString()));
                                maximizeButton.Click();
                                EnsureGridWindowActionAndSwitchToChildWindow(driver, afterSplitDashBoardList[0], dict[afterSplitDashBoardList[1].ToString() + "Dashboard"].ToString(), dict["MaximizeWindow"].ToString());
                                
                                ClickElement(driver, "ClickElementOnTabPM", ref dict, ExtractTabName);
                                SwitchWindow.SwitchToWindow(driver, ExtractTabName);
                                Thread.Sleep(2000);


                                if (string.IsNullOrEmpty(dr[TestDataConstants.COL_UNLINKBEFOREVERIFY].ToString()))
                                {
                                    driver.FindElement(By.XPath(dict[actionList[0]].ToString())).Click();
                                    string baseXPath = dict[actionList[1]].ToString();
                                    string colorXPath = baseXPath.Replace("ColorName", expectedColor);
                                    Thread.Sleep(2000);
                                    try
                                    {
                                        IWebElement colorElement = driver.FindElement(By.XPath(colorXPath));
                                        if (colorElement.Displayed)
                                            colorElement.Click();
                                    }
                                    catch
                                    { }
                                }
                                else
                                {
                                    driver.FindElement(By.XPath(dict[actionList[0]].ToString())).Click();
                                    string baseXPath = dict[actionList[1]].ToString();
                                    string colorXPath = baseXPath.Replace("ColorName", "");
                                    Thread.Sleep(2000);
                                    try
                                    {
                                        IWebElement colorElement = driver.FindElement(By.XPath(colorXPath));
                                        if (colorElement.Displayed)
                                            colorElement.Click();
                                    }
                                    catch
                                    { }
                                }
                                
                

                                DataTable DataNew = null;
                                try
                                {
                                    DataNew = SamsaraGridOperationHelper.ExtractGridData(driver, ref dict, ExtractTabName + "Headers", ExtractTabName + "DataItems");
                                    if (DataNew != null)
                                    {
                                        SamsaraGridOperationHelper.CreateExcelFileFromDataTable(Data, ExtractTabName);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                                    if (rethrow)
                                        throw;
                                }


                                bool areTwoTableSame = DataTableComparer.AreTablesEqual(DataNew, Data);
                                if (!areTwoTableSame)
                                {
                                    throw new Exception("DataChanged");
                                }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        




        }
        static void ClickOnTabPM(IWebDriver driver, string tabTitle, ref Dictionary<string, object> dict)
        {

            string xpath = dict["ClickAndOpenTab"].ToString();

            xpath = xpath.Replace("YOUR_TAB_TITLE", tabTitle);

            IWebElement tabElement = driver.FindElement(By.XPath(xpath));
            tabElement.Click();
        }
        //can perform all right click actions
        public static List<string> GetAllRightClick(WebDriver driver)
        {
            List<string> list = new List<string>();
            IReadOnlyCollection<IWebElement> allGrids = driver.FindElements(By.ClassName("contexify_itemContent"));

            List<IWebElement> visibleGrids = allGrids
                .Where(grid => grid.Displayed && grid.Enabled)
                .ToList();
            foreach (var ele in visibleGrids) 
            {
                list.Add(ele.Text.ToString());
            }
            return list;
        }

        public static void PerformRightClickActions(WebDriver driver, String Action, String ContextMenuXpath, string ContextMenuXpath1 = "")
        {
            string path = string.Empty;
            try
            {
                path = ContextMenuXpath.Replace("str", Action.ToString());
                IWebElement childElement = driver.FindElement(By.XPath(path));
                childElement.Click();
            }
            catch
            {
                IWebElement menuItem = driver.FindElement(By.XPath(path));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", menuItem);
            }
             /*       
            int str = 1;
            while (true)
            {
                try
                {
                    string path = ContextMenuXpath.Replace("str", str.ToString());
                    IWebElement parentDiv = null;
                    try
                    {
                        parentDiv = driver.FindElement(By.XPath(path));
                    }
                    catch {
                        string path1 = ContextMenuXpath1.Replace("str", str.ToString());
                        parentDiv = driver.FindElement(By.XPath(path1));
                    }
                    IWebElement childElement = parentDiv.FindElement(By.TagName("span"));
                    string text = childElement.Text.ToString();
                    if (text.ToLower().Contains(Action.ToLower()))
                    {
                        parentDiv.Click();
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine(Action + " not found");
                    break;
                }
                str++;
            }
              * */

        }

        public static bool SelectColumn(WebDriver driver,ref Dictionary<string, object> dict, DataTable table)
        {
            bool result = true;
            string columnType = "Columns_" + table.Rows[0]["DashBoardType"].ToString();
            string columnChooser = dict[columnType].ToString();
            string[] columns = table.Rows[0]["ColumnsToAdd"].ToString().Split(',');

            // Convert the array to a List
            List<string> splitList = new List<string>(columns);

            IWebElement element = driver.FindElement(By.XPath(columnChooser));
            try {
                if (table.Rows[0]["ColumnsToAdd"].ToString() == "All" || table.Rows[0]["ColumnsToAdd"].ToString().Equals(""))
                {
                    Console.WriteLine("Do Nothing");
                }
                else if (table.Rows[0]["ColumnsToAdd"].ToString() == "AllColumns")// handling in case user wants to select all columns ,Modified by yash
                {
                    element.Click();
                    IWebElement SelectAll = driver.FindElement(By.XPath(dict["SelectAllColumn"].ToString()));
                    SelectAll.Click();
                    Thread.Sleep(2000);
                }
                else
                {
                    element.Click();
                    Thread.Sleep(1000);
                    while (true)
                    {
                        IWebElement SelectAll = driver.FindElement(By.ClassName("MuiButtonBase-root")); 
                        string isChecked = SelectAll.GetAttribute("class").ToString();
                        if (isChecked.Contains("Mui-checked") || isChecked.Contains("MuiCheckbox-indeterminate"))
                        {
                            SelectAll.Click();
                        }
                        else
                        {
                            break;
                        }
                        Thread.Sleep(1500);
                    }
                    
                    
                    foreach (string col in splitList)
                    {
                        string valCheckBox = dict["SelectColumnValue"].ToString();
                        valCheckBox = valCheckBox.Replace("Exposure", col);
                        IWebElement checkBox = driver.FindElement(By.XPath(valCheckBox));
                        checkBox.Click();
                        Thread.Sleep(2000);
                    }
                    element.Click();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }
            return result;
        }

        public static bool FilterGrid(WebDriver driver, ref Dictionary<string, object> dict, string headerPath1, DataTable table)
        {
            bool result = true;
            string headerPath = dict[headerPath1].ToString();
            IList<IWebElement> columnHeaderElements = driver.FindElements(By.XPath(headerPath));
            List<string> columnHeaders = new List<string>();
            Dictionary<string, string> columnIndexMapping = new Dictionary<string, string>();
            
                foreach (DataRow dr in table.Rows)
                {
                    foreach (IWebElement columnHeaderElement in columnHeaderElements)
                    {
                        try
                        {
                            IWebElement parentHeader = columnHeaderElement.FindElement(By.XPath("ancestor::div[contains(@class, 'ag-header-cell')][@col-id]"));
                            string columnName = columnHeaderElement.Text;
                            if (!string.IsNullOrEmpty(columnName) && columnName.ToString().Equals(dr["Column"].ToString()))
                            {
                                IWebElement element = parentHeader.FindElement(By.XPath(".//span[@data-ref='eFilterButton']"));
                                try
                                {
                                    element.Click();
                                    Actions actions24 = new Actions(driver);
                                    Thread.Sleep(4000);

                                    IList<IWebElement> filterCheckBox = driver.FindElements(By.XPath(dict["SelectAll"].ToString()));
                                    //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", SelectAll);
                                    foreach (IWebElement elm in filterCheckBox)
                                    {
                                        try
                                        {
                                            if (elm.Text == "(Select All)")
                                            {
                                                actions24.Click(elm).Build().Perform();
                                            }
                                            else if (elm.Text == dr["Value"].ToString())
                                            {
                                                actions24.Click(elm).Build().Perform();
                                            }
                                            //string valueToChange = dr["Value"].ToString();
                                            //valCheckBox = valCheckBox.Replace("AAPL", valueToChange);
                                            /* IWebElement box = driver.FindElement(By.XPath(valCheckBox));
                                             ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", box);
                                             Thread.Sleep(2000);
                                             IWebElement apply = driver.FindElement(By.XPath(dict["Apply"].ToString()));
                                             apply.Click();*/
                                        }
                                        catch (Exception ex)
                                        {
                                            result = false;
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }

                                catch (Exception ex)
                                {
                                    result = false;
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error while processing column headers: " + ex.Message);
                        }
                    }
            }
            return result;
        }

        public static bool SortGridOnRtpnl(WebDriver driver, ref Dictionary<string, object> dict, string headerPath1, DataTable table)
        {
            bool result = true;
            string headerPath = dict[headerPath1].ToString();
            IList<IWebElement> columnHeaderElements = driver.FindElements(By.XPath(headerPath));
            List<string> columnHeaders = new List<string>();
            Dictionary<string, string> columnIndexMapping = new Dictionary<string, string>();
            foreach (DataRow dr in table.Rows)
            {
                foreach (IWebElement columnHeaderElement in columnHeaderElements)
                {
                    try
                    {
                        if (string.Equals("THREEDOTS", dr["Action"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                        {
                            IWebElement parentHeader = columnHeaderElement.FindElement(By.XPath("ancestor::div[contains(@class, 'ag-header-cell')][@col-id]"));
                            string isAsc = parentHeader.GetAttribute("aria-sort");
                            string columnName = columnHeaderElement.Text;
                            if (!string.IsNullOrEmpty(columnName) && columnName.ToString().Equals(dr["Column"].ToString()))
                            {
                                IWebElement clickDots = parentHeader.FindElement(By.XPath(".//span[@data-ref='eMenu']"));
                                //IWebElement clickDots = driver.FindElement(By.XPath("//*[@id= '" + id + "']/div[2]"));
                                clickDots.Click();
                                IWebElement element = driver.FindElement(By.XPath(dict["Apply"].ToString()));
                                if (isAsc.ToString() == "ascending")
                                {
                                    try
                                    {
                                        if (dr["Value"].ToString() == "Asc")
                                        {
                                            element.Click();
                                        }
                                        else
                                        {
                                            string baseXpath = dict["Dsc"].ToString();
                                            IWebElement dscXPath = driver.FindElement(By.XPath(baseXpath + "[normalize-space(text())= 'Sort Descending']"));
                                            dscXPath.Click();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        result = false;
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                else if (isAsc.ToString() == "descending")
                                {
                                    try
                                    {
                                        if (dr["Value"].ToString() == "Asc")
                                        {
                                            string baseXpath = dict["Asc"].ToString();
                                            IWebElement ascXPath = driver.FindElement(By.XPath(baseXpath + "[normalize-space(text())= 'Sort Ascending']"));
                                            ascXPath.Click();
                                        }
                                        else
                                        {
                                            element.Click();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        result = false;
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if (dr["Value"].ToString() == "Asc")
                                        {
                                            string baseXpath = dict["Asc"].ToString();
                                            IWebElement ascXPath = driver.FindElement(By.XPath(baseXpath + "[normalize-space(text())= 'Sort Ascending']"));
                                            ascXPath.Click();
                                        }
                                        else
                                        {
                                            string baseXpath = dict["Dsc"].ToString();
                                            IWebElement dscXPath = driver.FindElement(By.XPath(baseXpath + "[normalize-space(text())= 'Sort Descending']"));
                                            dscXPath.Click();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        result = false;
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                        }
                        else if (string.Equals("SORTBYCLICK", dr["Action"].ToString().ToUpper(), StringComparison.OrdinalIgnoreCase))
                        {
                            IWebElement parentHeader = columnHeaderElement.FindElement(By.XPath("ancestor::div[contains(@class, 'ag-header-cell')][@col-id]"));
                            string isAsc = parentHeader.GetAttribute("aria-sort");
                            string columnName = columnHeaderElement.Text;

                            if (!string.IsNullOrEmpty(columnName) && columnName.ToString().Equals(dr["Column"].ToString()))
                            {
                                    
                                IWebElement element = parentHeader.FindElement(By.XPath(".//span[@data-ref ='eText']"));
                                if (isAsc.ToString() == "ascending")
                                {
                                    try
                                    {
                                        if (dr["Value"].ToString() == "Asc")
                                        {
                                            // do nothing
                                            Console.WriteLine("Column is already sorted for asc");
                                        }
                                        else
                                        {
                                            element.Click();
                                            element.Click();
                                        }
                                    }

                                    catch (Exception ex)
                                    {
                                        result = false;
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                else if (isAsc.ToString() == "descending")
                                {
                                    try
                                    {
                                        if (dr["Value"].ToString() == "Asc")
                                        {
                                            element.Click();
                                        }
                                        else
                                        {
                                            // do nothing
                                            Console.WriteLine("Column is already sorted for asc");
                                        }
                                    }

                                    catch (Exception ex)
                                    {
                                        result = false;
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if (dr["Value"].ToString() == "Asc")
                                        {
                                            element.Click();
                                        }
                                        else
                                        {
                                            element.Click();
                                            element.Click();
                                        }
                                    }

                                    catch (Exception ex)
                                    {
                                        result = false;
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return result;
        }

        private static bool RTPNLVerticalData(IWebDriver driver, string horizontalScrollerPath, string verticalScrollerpath, string gridItemsPath)
        {
            try
            {
                driver.FindElement(By.XPath(verticalScrollerpath));
                MoveVerticalSlider(driver, verticalScrollerpath, 50);
                bool canSlide2 = true;
                int maxrowidx = maxRowIndex;
                while (canSlide2)
                {
                    bool allColumnsExist2 = true;
                    Dictionary<string, string> currGridCellElements = new Dictionary<string, string>();

                    IList<IWebElement> gridCellElements = driver.FindElements(By.XPath(gridItemsPath));

                    foreach (IWebElement gridCellElement in gridCellElements)
                    {
                        try
                        {
                            string cellId = gridCellElement.GetAttribute("id");
                            string cellValue = gridCellElement.Text;
                            currGridCellElements[cellId] = cellValue;

                            int rowIndex = int.Parse(cellId.Substring(cellId.LastIndexOf("r") + 1, cellId.LastIndexOf("c") - cellId.LastIndexOf("r") - 1));
                            int colIndex = int.Parse(cellId.Substring(cellId.LastIndexOf("c") + 1));
                            if (rowIndex < maxRowIndex)
                            {
                                continue;
                            }
                            maxrowidx = rowIndex;
                            if (columnIndexMapping.ContainsKey(colIndex))
                            {
                                string columnName = columnIndexMapping[colIndex];

                                GridRow gridRow = gridData.FirstOrDefault(row => row.RowIndex == rowIndex);
                                if (gridRow == null)
                                {
                                    gridRow = new GridRow(rowIndex);
                                    gridData.Add(gridRow);
                                }
                                if (!string.IsNullOrEmpty(cellValue) &&
     (!gridRow.ColumnValues.ContainsKey(colIndex) || !string.IsNullOrEmpty(gridRow.ColumnValues[colIndex])))
                                {
                                    gridRow.ColumnValues[colIndex] = cellValue;
                                }
                                allColumnsExist2 = false;
                            }
                        }
                        catch
                        {
                        }
                    }

                    if (prevGridCellElements.Count > 0)
                    {
                        allColumnsExist2 = true;
                        foreach (var cell in prevGridCellElements)
                        {
                            if (!currGridCellElements.ContainsKey(cell.Key) || currGridCellElements[cell.Key] != cell.Value)
                            {
                                allColumnsExist2 = false;
                                break;
                            }
                        }
                    }

                    prevGridCellElements = new Dictionary<string, string>(currGridCellElements);

                    if (allColumnsExist2)
                    {
                        canSlide2 = false;
                    }
                    else
                    {
                        MoveSlider(driver, horizontalScrollerPath);
                    }
                }
                if (maxrowidx > maxRowIndex)
                {
                    maxRowIndex = maxrowidx;
                    canSlide2 = false;
                }
                else {
                    canSlide2 = true;
                }
                if (!string.IsNullOrEmpty(horizontalScrollerPath))
                {
                    ResetSlider(driver, horizontalScrollerPath);
                }

                return canSlide2;
            }
            catch { }
            return false;
        }

        public static void SetAccoutOrMFForPST(IWebDriver driver, string grid, string account, string scroller) 
        {
            IWebElement divElement = driver.FindElement(By.ClassName(grid));
            var allElements = divElement.FindElements(By.XPath(".//*"));
            var columnHeaderElements = allElements.Where(e => e.GetAttribute("class") == "rct-text").ToList();
            for (int i = 0; i < columnHeaderElements.Count; i++)
            {
                if (columnHeaderElements[i].Text.ToUpper().Equals(account.ToUpper()))
                {
                    try
                    {
                        IWebElement checkboxChild = columnHeaderElements[i].FindElement(By.CssSelector("span.rct-checkbox"));
                        string ariaChecked = checkboxChild.GetAttribute("aria-checked");
                        if (ariaChecked.Equals("false"))
                            columnHeaderElements[i].Click();
                    }
                    catch
                    {
                        MoveVerticalSlider(driver, scroller, 20);
                        i--;
                    }
                }
            }
            MoveVerticalSlider(driver, scroller, -80);
        }

        public static DataTable ExtractGridData(IWebDriver driver, ref Dictionary<string, object> dict, string headerPath1, string gridItemsPath1, string verticalScroller = "", string horizontalScroller = "")
        {
            DataTable dataTable = new DataTable();
            try
            {
                if (!string.Equals(headerPath1, "SummaryDashboardHeaders", StringComparison.OrdinalIgnoreCase))
                {
                    string headerPath = dict[headerPath1].ToString();
                    string gridItemsPath = dict[gridItemsPath1].ToString();
                    string horizontalScrollerPath = "";
                    string verticalScrollerpath = "";
                    if (!string.IsNullOrEmpty(horizontalScroller) && dict.ContainsKey(horizontalScroller))
                    {
                        horizontalScrollerPath = dict[horizontalScroller].ToString();
                    }
                    if (!string.IsNullOrEmpty(verticalScroller) && dict.ContainsKey(verticalScroller))
                    {
                        verticalScrollerpath = dict[verticalScroller].ToString();
                    }
                    if (!string.IsNullOrEmpty(horizontalScrollerPath))
                    {
                        ResetSlider(driver, horizontalScrollerPath);
                    }

                    List<string> existingColumns = new List<string>();
                    bool canSlide = true;
                    while (canSlide)
                    {
                        IList<IWebElement> columnHeaderElements = driver.FindElements(By.XPath(headerPath));
                        bool allColumnsExist = true;

                        foreach (IWebElement columnHeaderElement in columnHeaderElements)
                        {
                            try
                            {
                                string id = columnHeaderElement.GetAttribute("id");
                                int cIndex = id.LastIndexOf('c');
                                int columnIndex = int.Parse(id.Substring(cIndex + 1));
                                string columnName = columnHeaderElement.FindElement(By.XPath("span")).Text;
                                if (string.IsNullOrEmpty(columnName))
                                {
                                    Console.WriteLine("ColumnName is empty hence marking it on index 0");
                                    try
                                    {
                                        columnGridIndexMapping.Add("CheckBox", 0);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                if (!string.IsNullOrEmpty(columnName) && !existingColumns.Contains(columnName))
                                {
                                    dataTable.Columns.Add(columnName);
                                    existingColumns.Add(columnName);
                                    allColumnsExist = false;
                                    columnIndexMapping.Add(columnIndex, columnName);
                                    try
                                    {
                                        columnGridIndexMapping.Add(columnName, columnIndex);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error while processing column headers: " + ex.Message);
                            }
                        }

                        if (allColumnsExist)
                        {
                            canSlide = false;
                        }
                        else
                        {
                            MoveSlider(driver, horizontalScrollerPath);
                        }
                    }
                    if (!string.IsNullOrEmpty(horizontalScrollerPath))
                    {
                        ResetSlider(driver, horizontalScrollerPath);
                    }

                    bool canSlide2 = true;


                    while (canSlide2)
                    {
                        bool allColumnsExist2 = true;
                        Dictionary<string, string> currGridCellElements = new Dictionary<string, string>();

                        IList<IWebElement> gridCellElements = driver.FindElements(By.XPath(gridItemsPath));

                        foreach (IWebElement gridCellElement in gridCellElements)
                        {
                            try
                            {
                                string cellId = gridCellElement.GetAttribute("id");
                                string cellValue = gridCellElement.Text;
                                currGridCellElements[cellId] = cellValue;

                                int rowIndex = int.Parse(cellId.Substring(cellId.LastIndexOf("r") + 1, cellId.LastIndexOf("c") - cellId.LastIndexOf("r") - 1));
                                int colIndex = int.Parse(cellId.Substring(cellId.LastIndexOf("c") + 1));
                                if (rowIndex > maxRowIndex) {
                                    maxRowIndex = rowIndex;
                                }
                                if (columnIndexMapping.ContainsKey(colIndex))
                                {
                                    string columnName = columnIndexMapping[colIndex];

                                    GridRow gridRow = gridData.FirstOrDefault(row => row.RowIndex == rowIndex);
                                    if (gridRow == null)
                                    {
                                        gridRow = new GridRow(rowIndex);
                                        gridData.Add(gridRow);
                                    }
                                    if (!string.IsNullOrEmpty(cellValue) &&
     (!gridRow.ColumnValues.ContainsKey(colIndex) || !string.IsNullOrEmpty(gridRow.ColumnValues[colIndex])))
                                    {
                                        gridRow.ColumnValues[colIndex] = cellValue;
                                    }
                                    allColumnsExist2 = false;
                                }
                            }
                            catch 
                            {
                            }
                        }

                        if (prevGridCellElements.Count > 0)
                        {
                            allColumnsExist2 = true;
                            foreach (var cell in prevGridCellElements)
                            {
                                if (!currGridCellElements.ContainsKey(cell.Key) || currGridCellElements[cell.Key] != cell.Value)
                                {
                                    allColumnsExist2 = false;
                                    break;
                                }
                            }
                        }

                        prevGridCellElements = new Dictionary<string, string>(currGridCellElements);

                        if (allColumnsExist2)
                        {
                            canSlide2 = false;
                        }
                        else
                        {
                            MoveSlider(driver, horizontalScrollerPath);
                        }
                    }

                    if (!string.IsNullOrEmpty(horizontalScrollerPath))
                    {
                        ResetSlider(driver, horizontalScrollerPath);
                    }


                    if (maxRowIndex > 28)
                    {
                        bool scroller = false;
                        int rowIDX = maxRowIndex;
                        while (true)
                        {
                            while (!scroller)
                            {
                                scroller = RTPNLVerticalData(driver, horizontalScrollerPath, verticalScrollerpath, gridItemsPath);
                            }
                            scroller = false;
                            if (rowIDX != maxRowIndex)
                            {
                                rowIDX = maxRowIndex;
                            }
                            else
                            {
                                break;
                            }
                        }

                        ResetVerticalSlider(driver, verticalScrollerpath);
                    }

                    foreach (GridRow row in gridData)
                    {
                        DataRow newRow = dataTable.NewRow();
                        try
                        {
                            foreach (KeyValuePair<int, string> columnValue in row.ColumnValues)
                            {
                                string columnName;
                                if (columnIndexMapping.TryGetValue(columnValue.Key, out columnName))
                                {
                                    newRow[columnName] = columnValue.Value;
                                }
                                else
                                {
                                    Console.WriteLine("Column index: " + columnValue.Key + " not found in columnIndexMapping.");
                                }
                            }
                            dataTable.Rows.Add(newRow);
                        }
                        catch (Exception ex)
                        {
                            SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " RTPNL Grid Error");
                            Console.WriteLine("Error while extracting grid data: " + ex.ToString());
                        }
                    }
                    try
                    {
                        DataView dv = dataTable.DefaultView;
                        if (gridItemsPath1.Contains("SymAccount"))
                        {
                            dv.Sort = "Symbol ASC, Account ASC";
                        }
                        else if (gridItemsPath1.Contains("SymbolFund"))
                        {
                            dv.Sort = "Symbol ASC, Fund ASC";
                        }
                        else if (gridItemsPath1.Contains("Account"))
                        {
                            dv.Sort = "Account ASC";
                        }
                        else if (gridItemsPath1.Contains("Fund"))
                        {
                            dv.Sort = "Fund ASC";
                        }
                        else if (gridItemsPath1.Contains("Symbol"))
                        {
                            dv.Sort = "Symbol ASC";
                        }
                        dataTable = dv.ToTable();
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                    maxRowIndex = 1;
                    gridData = new List<GridRow>();
                    prevGridCellElements = new Dictionary<string, string>();
                    columnIndexMapping = new Dictionary<int, string>();

                }
                else
                {

                    string parentPath = dict[headerPath1].ToString();
                    var parentElements = driver.FindElements(By.XPath(parentPath));
                    var headers = new List<string>();
                    var values = new List<string>();
                    var percentageValues = new List<string>();
                    foreach (var parentElement in parentElements)
                    {
                        var headerElement = parentElement.FindElement(By.XPath(".//p"));
                        if (headers.Contains(headerElement.Text.Trim()))
                        {
                            continue;
                        }
                        headers.Add(headerElement.Text.Trim());
                        var valueElement = parentElement.FindElement(By.XPath(".//h4"));
                        values.Add(valueElement.Text.Trim());
                        try
                        {
                            var percentageElement = parentElement.FindElement(By.XPath(".//span"));
                            percentageValues.Add(percentageElement.Text.Trim());
                        }
                        catch (Exception)
                        {
                            percentageValues.Add(String.Empty);
                        }
                    }

                    foreach (var header in headers)
                    {
                        dataTable.Columns.Add(header, typeof(string));
                    }

                    DataRow valuesRow = dataTable.NewRow();
                    for (int i = 0; i < headers.Count; i++)
                    {
                        if (i < values.Count)
                        {
                            valuesRow[i] = values[i];
                        }
                        else
                        {
                            valuesRow[i] = DBNull.Value;
                        }
                    }
                    dataTable.Rows.Add(valuesRow);
                    DataRow percentageValuesRow = dataTable.NewRow();
                    for (int i = 0; i < headers.Count; i++)
                    {
                        if (i < percentageValues.Count)
                        {
                            percentageValuesRow[i] = percentageValues[i];
                        }
                        else
                        {
                            percentageValuesRow[i] = DBNull.Value;
                        }
                    }
                    dataTable.Rows.Add(percentageValuesRow);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return dataTable;
        }

        public static void ResetVerticalSlider(IWebDriver driver, string sliderXPath)
        {
            MoveSlider(driver, sliderXPath);
            Actions actions = null;
            try
            {
                IWebElement slider = driver.FindElement(By.XPath(sliderXPath));
                actions = new Actions(driver);
                actions.MoveToElement(slider).Perform();
                int maxOffset = -slider.Size.Height;
                int step = 50;
                int isSuccessful = 0;

                while (isSuccessful < 10 && maxOffset < 0)
                {
                    try
                    {
                        actions.ClickAndHold(slider)
                               .MoveByOffset(0, maxOffset)
                               .Release()
                               .Perform();
                        isSuccessful++;
                    }
                    catch (MoveTargetOutOfBoundsException)
                    {
                        maxOffset += step;
                        isSuccessful++;
                    }
                }

                if (isSuccessful > 1)
                {
                    Console.WriteLine("Slider moved to the far left with maximum safe offset.");
                }
                else
                {
                    Console.WriteLine("Unable to move the slider to the far left within bounds.");
                }

                slider = driver.FindElement(By.XPath(sliderXPath));
                if (slider.Location.Y > 0)
                {

                    int remainingOffset = -slider.Location.Y;

                    try
                    {
                        actions.ClickAndHold(slider)
                               .MoveByOffset(0, remainingOffset)
                               .Release()
                               .Perform();

                        Console.WriteLine("Slider reset to the leftmost position.");
                    }
                    catch (MoveTargetOutOfBoundsException ex)
                    {
                        Console.WriteLine("Final move failed due to move target out of bounds: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Slider is already at the leftmost position. No need to reset.");
                }
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("Slider element not found: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while resetting the slider: " + ex.Message);
            }
            finally
            {
                actions = null;
            }
        }

        public static void MoveVerticalSlider(IWebDriver driver, string sliderXPath, int? moveByWidth = null)
        {
            try
            {
                Thread.Sleep(3000);

                IWebElement slider = driver.FindElement(By.XPath(sliderXPath));
                Actions actions = new Actions(driver);

                actions.MoveToElement(slider).Perform();
                int width = moveByWidth ?? slider.Size.Width;
                actions.ClickAndHold(slider)
                       .MoveByOffset(0, width)
                       .Release()
                       .Perform();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public static void MoveSlider(IWebDriver driver, string sliderXPath, int? moveByWidth = null)
        {
            try
            {
                Thread.Sleep(3000);

                IWebElement slider = driver.FindElement(By.XPath(sliderXPath));
                Actions actions = new Actions(driver);

                actions.MoveToElement(slider).Perform();
                int width = moveByWidth ?? slider.Size.Width;
                actions.ClickAndHold(slider)
                       .MoveByOffset(width, 0)
                       .Release()
                       .Perform();
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("Slider element not found: " + ex.Message);
            }
            catch
            {

            }
        }
        public static void ResetSlider(IWebDriver driver, string sliderXPath)
        {
            MoveSlider(driver, sliderXPath);
            Actions actions = null;
            try
            {
                IWebElement slider = driver.FindElement(By.XPath(sliderXPath));
                actions = new Actions(driver);
                actions.MoveToElement(slider).Perform();
                int maxOffset = -slider.Size.Width;
                int step = 10;
                int isSuccessful = 0;

                while (isSuccessful < 5 && maxOffset < 0)
                {
                    try
                    {
                        actions.ClickAndHold(slider)
                               .MoveByOffset(maxOffset, 0)
                               .Release()
                               .Perform();
                        isSuccessful++;
                        break;
                    }
                    catch (MoveTargetOutOfBoundsException)
                    {
                        maxOffset += step;
                        isSuccessful++;
                    }
                }

                if (isSuccessful > 1)
                {
                    Console.WriteLine("Slider moved to the far left with maximum safe offset.");
                }
                else
                {
                    Console.WriteLine("Unable to move the slider to the far left within bounds.");
                }

                slider = driver.FindElement(By.XPath(sliderXPath));
                if (slider.Location.X > 0)
                {

                    int remainingOffset = -slider.Location.X;

                    try
                    {
                        actions.ClickAndHold(slider)
                               .MoveByOffset(remainingOffset, 0)
                               .Release()
                               .Perform();

                        Console.WriteLine("Slider reset to the leftmost position.");
                    }
                    catch (MoveTargetOutOfBoundsException ex)
                    {
                        Console.WriteLine("Final move failed due to move target out of bounds: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Slider is already at the leftmost position. No need to reset.");
                }
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("Slider element not found: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while resetting the slider: " + ex.Message);
            }
            finally
            {
                actions = null;
            }
        }
        public static void SelectRTPNLRow(WebDriver driver, DataTable dtable, ref Dictionary<string, object> dict, DataTable UIData, string DefaultCellValue, int rowIndex)
        {
            string columnName = "CheckBox";
            if (dtable.Columns.Contains(columnName))
            {
                try
                {
                    foreach (DataRow dr in dtable.Rows)
                    {
                        // int RowIndex =  DataUtilities.get                   //UIData.Rows.IndexOf(dr) + rowIndex+1;
                        DataRow newRow = SamsaraGridOperationHelper.DeleteNonMatchingColumnsValues(dr, UIData);
                        DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(UIData), newRow);
                        int RowIndex = UIData.Rows.IndexOf(dtRow) + 2;


                        if (RowIndex < 2)
                        {
                            Console.WriteLine("Row not found in the UIData table.");
                            continue;
                        }
                        int columnGridIndex = 0;
                        try
                        {
                            columnGridIndex = columnGridIndexMapping[columnName];
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        string cellId = DefaultCellValue.Substring(1) + RowIndex.ToString() + "c" + columnGridIndex;

                        string xpath = dict["CheckBox"].ToString().Replace("cellID", cellId);
                        try
                        {

                            IWebElement checkbox = driver.FindElement(By.XPath(xpath));
                            if (!checkbox.Selected)
                            {
                                checkbox.Click();
                                Console.WriteLine("Checkbox with ID " + cellId + " selected successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Checkbox with ID " + cellId + " is already selected.");
                            }
                        }
                        catch (NoSuchElementException)
                        {
                            Console.WriteLine("Element with XPath " + xpath + " not found.");
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine("Error occurred while interacting with the checkbox: " + ex.Message);
                        }

                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }
            else
            {
                try
                {
                    foreach (DataRow dr in dtable.Rows)
                    {
                     
                        DataRow newRow = SamsaraGridOperationHelper.DeleteNonMatchingColumnsValues(dr, UIData);
                        DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(UIData), newRow);
                        int RowIndex = UIData.Rows.IndexOf(dtRow);
                            string xpath = dict["CheckBox_Ag"].ToString();
                            IList<IWebElement> list = driver.FindElements(By.XPath(xpath));
                            IWebElement checkBox = list[RowIndex];
                                checkBox.Click();
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        public static void LinkDashBoard(WebDriver driver, DataTable dtable, ref Dictionary<string, object> dict, List<string> actionList)
        {
            const int maxRetries = 3;
            const int delayBetweenRetries = 1000;

            try
            {
                foreach (DataRow dr in dtable.Rows)
                {
                    bool success = false;
                    int attempt = 0;

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;
                        Console.WriteLine("Attempting again ....");

                        try
                        {
                            List<string> DashBoardList = dr[TestDataConstants.COL_DASHBOARDPARENTCHILD].ToString().Split(',').ToList();
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

                            driver.FindElement(By.XPath(dict["MaximizeWindow"].ToString())).Click();

                            List<string> LinkingTabNameList = dr[TestDataConstants.COL_LINKINGTABNAME].ToString().Split(',').ToList();
                            string expectedColor = dr[TestDataConstants.COL_COLOR].ToString();


                            foreach (string LinkingTabName in LinkingTabNameList)
                            {
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
                                ClickElement(driver, "ClickElementOnTabPM", ref dict, LinkingTabName);
                                SwitchWindow.SwitchToWindow(driver, LinkingTabName);
                                Thread.Sleep(2000);
                                driver.FindElement(By.XPath(dict[actionList[0]].ToString())).Click();
                                string baseXPath = dict[actionList[1]].ToString();
                                string colorXPath = baseXPath.Replace("ColorName", expectedColor);
                                Thread.Sleep(2000);
                                IWebElement colorElement = driver.FindElement(By.XPath(colorXPath));
                                if (colorElement.Displayed)
                                    colorElement.Click();

                                IWebElement colorSpan = driver.FindElement(By.XPath(dict[actionList[2]].ToString()));
                                string colorClass = colorSpan.GetAttribute("class");
                                success = colorClass.Contains(expectedColor);

                                if (success)
                                {
                                    Console.WriteLine("Color " + expectedColor + " verified successfully.");

                                }
                                else
                                {
                                    Console.WriteLine("Failed to verify Linking color " + expectedColor + " on attempt " + attempt + ".");
                                }
                            }

                            if (!success)
                            {
                                Console.WriteLine("Retrying operation due to failure in color verification for attempt " + attempt + ".");
                                Thread.Sleep(delayBetweenRetries);
                            }
                        }
                        catch (Exception ex)
                        {

                            if (attempt < maxRetries)
                            {
                                Thread.Sleep(delayBetweenRetries);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                    if (!success)
                    {
                        Console.WriteLine("Failed to verify Linking  color after all retries.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void CreateExcelFileFromDataTable(DataTable dataTable, string fileName)
        {
            try
            {

                string downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
                string filePath = Path.Combine(downloadPath, "RTPNLData" + fileName + ".xlsx");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                    package.SaveAs(new FileInfo(filePath));
                }

                Console.WriteLine("Excel file created successfully at " + filePath);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Issue arises while creating ExcelFileFromDataTable");
            }
        }

        public static DataTable ExtractPSTGridData(IWebDriver driver, ref Dictionary<string, object> dict, string headerPath1, string gridItemsPath1, string verticalScroller = "", string horizontalScroller = "")
        {
            DataTable dataTable = new DataTable();
            try
            {

                string headerPath = dict[headerPath1].ToString();
                string gridItemsPath = dict[gridItemsPath1].ToString();
                string horizontalScrollerPath = "";
                string verticalScrollerpath = "";
                if (!string.IsNullOrEmpty(horizontalScroller) && dict.ContainsKey(horizontalScroller))
                {
                    horizontalScrollerPath = dict[horizontalScroller].ToString();
                }
                if (!string.IsNullOrEmpty(verticalScroller) && dict.ContainsKey(verticalScroller))
                {
                    verticalScrollerpath = dict[verticalScroller].ToString();
                }
                if (!string.IsNullOrEmpty(horizontalScrollerPath))
                {
                    ResetSlider2(driver, horizontalScrollerPath, ref dict);
                }

                List<string> existingColumns = new List<string>();
                bool canSlide = true;
                while (canSlide)
                {
                    IList<IWebElement> columnHeaderElements = driver.FindElements(By.XPath(headerPath));
                    bool allColumnsExist = true;

                    foreach (IWebElement columnHeaderElement in columnHeaderElements)
                    {
                        try
                        {
                            string id = columnHeaderElement.GetAttribute("id");
                            int cIndex = id.LastIndexOf('c');
                            int columnIndex = int.Parse(id.Substring(cIndex + 1));
                            string columnName = columnHeaderElement.FindElement(By.XPath("span")).Text;
                            if (string.IsNullOrEmpty(columnName))
                            {
                                Console.WriteLine("ColumnName is empty hence marking it on index 0");
                                try
                                {
                                    columnGridIndexMapping.Add("CheckBox", 0);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (!string.IsNullOrEmpty(columnName) && !existingColumns.Contains(columnName))
                            {
                                dataTable.Columns.Add(columnName);
                                existingColumns.Add(columnName);
                                allColumnsExist = false;
                                columnIndexMapping.Add(columnIndex, columnName);
                                try
                                {
                                    columnGridIndexMapping.Add(columnName, columnIndex);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error while processing column headers: " + ex.Message);
                        }
                    }

                    if (allColumnsExist)
                    {
                        canSlide = false;
                    }
                    else
                    {
                        IWebElement slider = driver.FindElement(By.XPath(horizontalScrollerPath));
                        if (slider != null)
                        {
                            int ScrollerValue = (int)Math.Ceiling(Convert.ToDouble(dict[horizontalScroller + "MoveSize"]));
                            MoveSlider(driver, horizontalScrollerPath, slider.Size.Width / ScrollerValue);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(horizontalScrollerPath))
                {
                    ResetSlider2(driver, horizontalScrollerPath, ref dict);
                }

                bool canSlide2 = true;


                while (canSlide2)
                {
                    bool allColumnsExist2 = true;
                    Dictionary<string, string> currGridCellElements = new Dictionary<string, string>();

                    IList<IWebElement> gridCellElements = driver.FindElements(By.XPath(gridItemsPath));

                    foreach (IWebElement gridCellElement in gridCellElements)
                    {
                        try
                        {
                            string cellId = gridCellElement.GetAttribute("id");
                            string cellValue = gridCellElement.Text;
                            currGridCellElements[cellId] = cellValue;

                            int rowIndex = int.Parse(cellId.Substring(cellId.LastIndexOf("r") + 1, cellId.LastIndexOf("c") - cellId.LastIndexOf("r") - 1));
                            int colIndex = int.Parse(cellId.Substring(cellId.LastIndexOf("c") + 1));
                            if (rowIndex > maxRowIndex)
                            {
                                maxRowIndex = rowIndex;
                            }
                            if (columnIndexMapping.ContainsKey(colIndex))
                            {
                                string columnName = columnIndexMapping[colIndex];

                                GridRow gridRow = gridData.FirstOrDefault(row => row.RowIndex == rowIndex);
                                if (gridRow == null)
                                {
                                    gridRow = new GridRow(rowIndex);
                                    gridData.Add(gridRow);
                                }
                                if (!string.IsNullOrEmpty(cellValue) &&
     (!gridRow.ColumnValues.ContainsKey(colIndex) || !string.IsNullOrEmpty(gridRow.ColumnValues[colIndex])))
                                {
                                    gridRow.ColumnValues[colIndex] = cellValue;
                                }
                                allColumnsExist2 = false;
                            }
                        }
                        catch
                        {
                        }
                    }

                    if (prevGridCellElements.Count > 0)
                    {
                        allColumnsExist2 = true;
                        foreach (var cell in prevGridCellElements)
                        {
                            if (!currGridCellElements.ContainsKey(cell.Key) || currGridCellElements[cell.Key] != cell.Value)
                            {
                                allColumnsExist2 = false;
                                break;
                            }
                        }
                    }

                    prevGridCellElements = new Dictionary<string, string>(currGridCellElements);

                    if (allColumnsExist2)
                    {
                        canSlide2 = false;
                    }
                    else
                    {
                        IWebElement slider = driver.FindElement(By.XPath(horizontalScrollerPath));
                        if (slider != null)
                        {
                            int ScrollerValue = (int)Math.Ceiling(Convert.ToDouble(dict[horizontalScroller + "MoveSize"]));
                            MoveSlider(driver, horizontalScrollerPath, slider.Size.Width / ScrollerValue);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(horizontalScrollerPath))
                {
                    ResetSlider2(driver, horizontalScrollerPath, ref dict);
                }



                foreach (GridRow row in gridData)
                {
                    DataRow newRow = dataTable.NewRow();
                    try
                    {
                        foreach (KeyValuePair<int, string> columnValue in row.ColumnValues)
                        {
                            string columnName;
                            if (columnIndexMapping.TryGetValue(columnValue.Key, out columnName))
                            {
                                newRow[columnName] = columnValue.Value;
                            }
                            else
                            {
                                Console.WriteLine("Column index: " + columnValue.Key + " not found in columnIndexMapping.");
                            }
                        }
                        dataTable.Rows.Add(newRow);
                    }
                    catch (Exception ex)
                    {
                        SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun + " RTPNL Grid Error");
                        Console.WriteLine("Error while extracting grid data: " + ex.ToString());
                    }
                }

                maxRowIndex = 1;
                gridData = new List<GridRow>();
                prevGridCellElements = new Dictionary<string, string>();
                columnIndexMapping = new Dictionary<int, string>();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return dataTable;
        }

        public static void MoveVerticalSliderPST(IWebDriver driver, string sliderXPath, int? moveByWidth = null)
        {
            try
            {
                Thread.Sleep(3000);

                IWebElement slider = driver.FindElement(By.XPath(sliderXPath));
                Actions actions = new Actions(driver);

                actions.MoveToElement(slider).Perform();
                int width = moveByWidth ?? slider.Size.Width;
                actions.ClickAndHold(slider)
                       .MoveByOffset(0, width)
                       .Release()
                       .Perform();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public static void ResetSlider2(IWebDriver driver, string sliderXPath, ref Dictionary<string, object> dict)
        {
            try
            {
                IWebElement slider = driver.FindElement(By.XPath(sliderXPath));

                MoveSlider(driver, sliderXPath, slider.Size.Width / 4);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Actions actions = null;
            try
            {
                IWebElement slider = driver.FindElement(By.XPath(sliderXPath));
                actions = new Actions(driver);
                actions.MoveToElement(slider).Perform();
                int ScrollerValue = (int)Math.Ceiling(Convert.ToDouble(dict["PTTGridHorizontalScrollerMoveSizeReset"]));
                int maxOffset = -slider.Size.Width / ScrollerValue;
                int step = 10;
                int isSuccessful = 0;

                while (isSuccessful < 5 && maxOffset < 0)
                {
                    try
                    {
                        actions.ClickAndHold(slider)
                               .MoveByOffset(maxOffset, 0)
                               .Release()
                               .Perform();
                        isSuccessful++;
                        break;
                    }
                    catch (MoveTargetOutOfBoundsException)
                    {
                        maxOffset += step;
                        isSuccessful++;
                    }
                }

                if (isSuccessful > 1)
                {
                    Console.WriteLine("Slider moved to the far left with maximum safe offset.");
                }
                else
                {
                    Console.WriteLine("Unable to move the slider to the far left within bounds.");
                }

                slider = driver.FindElement(By.XPath(sliderXPath));
                if (slider.Location.X > 0)
                {

                    int remainingOffset = -slider.Location.X;

                    try
                    {
                        actions.ClickAndHold(slider)
                               .MoveByOffset(remainingOffset, 0)
                               .Release()
                               .Perform();

                        Console.WriteLine("Slider reset to the leftmost position.");
                    }
                    catch (MoveTargetOutOfBoundsException ex)
                    {
                        Console.WriteLine("Final move failed due to move target out of bounds: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Slider is already at the leftmost position. No need to reset.");
                }
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("Slider element not found: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while resetting the slider: " + ex.Message);
            }
            finally
            {
                actions = null;
            }
        }
        public static bool MoveSliderToTopMost(IWebDriver driver, string sliderXPath, Dictionary<string, object> dict)
        {
            try
            {
                Thread.Sleep(3000);
                IWebElement slider = driver.FindElement(By.XPath(sliderXPath));
                Actions actions = new Actions(driver);
                int initialY = slider.Location.Y;
                actions.MoveToElement(slider).Perform();
                int ScrollerValue = (int)Math.Ceiling(Convert.ToDouble(dict["PTTGridVerticalScrollerMaximumScrollCount"]));
                int height = slider.Size.Height / ScrollerValue;
                actions.ClickAndHold(slider)
                       .MoveByOffset(0, -height)
                       .Release()
                       .Perform();
                Thread.Sleep(1000);
                int newY = slider.Location.Y;
                return initialY != newY;
            }
            catch
            {
                return false;
            }
        }

        public static bool MoveSliderToBottomMost(IWebDriver driver, string sliderXPath, Dictionary<string, object> dict)
        {
            try
            {
                Thread.Sleep(3000);
                IWebElement slider = driver.FindElement(By.XPath(sliderXPath));
                Actions actions = new Actions(driver);
                int initialY = slider.Location.Y;
                actions.MoveToElement(slider).Perform();

                int ScrollerValue = (int)Math.Ceiling(Convert.ToDouble(dict["PTTGridVerticalScrollerMaximumScrollCount"]));
                int height = slider.Size.Height / ScrollerValue;
                actions.ClickAndHold(slider)
                       .MoveByOffset(0, height)
                       .Release()
                       .Perform();
                Thread.Sleep(1000);
                int newY = slider.Location.Y;
                return initialY != newY;
            }
            catch
            {
                return false;
            }
        }




        internal static DataTable GetGridData(IWebDriver driver, ref Dictionary<string, object> dict, string headerPath1, string gridItemsPath1, string verticalScroller = "", string horizontalScroller = "")
        {
            DataTable dataTable = new DataTable();
            string headerPath = dict[headerPath1].ToString();
            string gridItemsPath = dict[gridItemsPath1].ToString();
            string horizontalScrollerPath = "";
            string verticalScrollerpath = "";
            Dictionary<string, string> columnMap = new Dictionary<string, string>();
            Dictionary<string, string> columnIndexMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(horizontalScroller) && dict.ContainsKey(horizontalScroller))
            {
                horizontalScrollerPath = dict[horizontalScroller].ToString();
                
            }
            if (!string.IsNullOrEmpty(verticalScroller) && dict.ContainsKey(verticalScroller))
            {
                verticalScrollerpath = dict[verticalScroller].ToString();
            }

            if (!string.IsNullOrEmpty(horizontalScrollerPath))
            {
                LeftSlider(driver, horizontalScroller);

            }

            // Fetch Rows indexes
            var rows = driver.FindElements(By.XPath("//*[@role='row'][@row-index]"));

            //  Extract all unique row-index values dynamically
            List<int> rowIndexes = new List<int>();
            foreach (var row in rows)
            {
                string rowIndexStr = row.GetAttribute("row-index");
                int rowIndex = 0;
                int.TryParse(rowIndexStr, out rowIndex);
                if (!rowIndexes.Contains(rowIndex))
                {
                    rowIndexes.Add(rowIndex);
                }
            }

            List<string> existingColumns = new List<string>();
            for (int i = 0; i < rowIndexes.Count; i++)
            {
                
                string xpath = string.Format(gridItemsPath, i);
                int count = 0;
                bool canSlide = true;
                while (canSlide)
                {

                    IList<IWebElement> columnHeaderElements = driver.FindElements(By.XPath(headerPath));
                    bool allColumnsExist = true;
                    foreach (IWebElement columnHeaderElement in columnHeaderElements)
                    {
                        try
                        {
                            string columnName = columnHeaderElement.Text;
                            IWebElement parentElement = columnHeaderElement.FindElement(By.XPath("ancestor::div[contains(@class, 'ag-header-cell')][@col-id]"));
                            string colId = parentElement.GetAttribute("col-id");
                            if (string.IsNullOrEmpty(columnName))
                            {
                                continue;
                            }
                            else if (!string.IsNullOrEmpty(columnName) && !existingColumns.Contains(columnName))
                            {
                                try
                                {
                                    dataTable.Columns.Add(columnName);
                                    existingColumns.Add(columnName);
                                    allColumnsExist = false;
                                    columnMap.Add(columnName, colId);
                                    try
                                    {
                                        columnIndexMap.Add(colId, columnName);
                                    }
                                    catch (Exception ex) { }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                            }

                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine("Error while processing column headers: " + ex.Message);
                        }
                    }
                    count++;
                    if (allColumnsExist)
                    {
                        canSlide = false;
                    }
                    else
                    {
                        int steps = 7;
                        if (count == 1)
                        {
                            steps = 16;
                        }
                        RightSlider(driver, horizontalScroller,steps);
                    }
                }
                if (!string.IsNullOrEmpty(horizontalScrollerPath))
                {
                    LeftSlider(driver, horizontalScrollerPath);
                }

                count = 0;
                bool canSlide2 = true;
                bool allColumnsExist2 = true;
                DataRow dataRow = dataTable.NewRow();
                List<string> fetchedColumnsList = new List<string>();
                while (canSlide2)
                {
                    IList<IWebElement> columnHeaderElements = driver.FindElements(By.XPath(xpath));
                    foreach (IWebElement columnHeaderElement in columnHeaderElements)
                    {
                        try
                        {
                            string colId = columnHeaderElement.GetAttribute("col-id");
                            string cellValue = columnHeaderElement.Text;

                            if (columnIndexMap.ContainsKey(colId))
                            {
                                string columnName = columnIndexMap[colId];
                                fetchedColumnsList.Add(columnName);
                                dataRow[columnName] = cellValue;
                            }

                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }

                    }
                    count++;
                    try
                    {
                        foreach (var colId in columnMap.Keys)
                        {
                            if (fetchedColumnsList.Contains(colId))
                            {
                                allColumnsExist2 = true;
                            }
                            else
                            {
                                allColumnsExist2 = false;
                            }
                        }
                        if (allColumnsExist2)
                        {
                            dataTable.Rows.Add(dataRow);
                            canSlide2 = false;
                        }
                        else
                        {
                            int steps = 7;
                            if (count == 1)
                            {
                                steps = 16;
                            }
                            RightSlider(driver, horizontalScroller,steps);
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
                if (!string.IsNullOrEmpty(horizontalScrollerPath))
                {
                    LeftSlider(driver, horizontalScroller);
                }
                
            }
            return dataTable;
        
            }

        public static void RightSlider(IWebDriver driver, string horizontalRight, int steps)
        {
            try
            {
                Actions actions = new Actions(driver);
                string xpath = horizontalRight;
                IWebElement startingCell = driver.FindElement(By.XPath(xpath));
                startingCell.Click();
                   for (int i = 0; i < steps; i++)
                   {
                       try
                       {
                           actions.SendKeys(Keys.ArrowRight).Perform();
                           Thread.Sleep(100);
                       }
                       catch (Exception ex)
                       {
                       }
               }
            }
            catch (Exception)
            {
                Actions actions = new Actions(driver);
                for (int i = 0; i < steps; i++)
                {
                    actions.SendKeys(Keys.ArrowRight).Perform();
                    Thread.Sleep(200);

                }   
            }
        }

        private static void LeftSlider(IWebDriver driver, string horizontalLeft)
        {
            try
            {
                Actions actions = new Actions(driver);
                string xpath = horizontalLeft;
                IWebElement startingCell = driver.FindElement(By.XPath(xpath));
                if (startingCell.Displayed && startingCell.Enabled)
                {
                    startingCell.Click();
                    Console.WriteLine("Initial cell clicked.");
                }
                else
                {
                    for (int i = 0; i < 56; i++)
                    {
                        actions.SendKeys(Keys.ArrowLeft).Perform();
                        Thread.Sleep(200);

                    }
                }
            }
            catch (Exception)
            {
                Actions actions = new Actions(driver);
                for (int i = 0; i < 56; i++)
                {
                    actions.SendKeys(Keys.ArrowLeft).Perform();
                    Thread.Sleep(200);

                }
            }
        }
    }
}

