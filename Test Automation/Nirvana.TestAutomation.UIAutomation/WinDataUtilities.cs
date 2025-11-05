using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAutomationClient;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OfficeOpenXml;
using System.IO;
using OpenQA.Selenium.Remote;
using IUIAutomationElement = UIAutomationClient.IUIAutomationElement;
using IUIAutomationCondition = UIAutomationClient.IUIAutomationCondition;
using IUIAutomationElementArray = UIAutomationClient.IUIAutomationElementArray;
using IUIAutomationTreeWalker = UIAutomationClient.IUIAutomationTreeWalker;
using IUIAutomationCacheRequest = UIAutomationClient.IUIAutomationCacheRequest;
using Nirvana.TestAutomation.UIAutomation;



namespace Nirvana.TestAutomation.UIAutomation
{
    public class WinDataUtilities
    {
       
        private enum COINIT : uint
        {
            COINIT_APARTMENTTHREADED = 0x2,
            COINIT_MULTITHREADED = 0x0,
            COINIT_DISABLE_OLE1DDE = 0x4,
            COINIT_SPEED_OVER_MEMORY = 0x8
        }

        [DllImport("ole32.dll")]
        private static extern int CoInitializeEx(IntPtr pvReserved, COINIT dwCoInit);



        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);




        public static DataTable  AddColumnHeaders(DataTable dataTable, string columnName)
        {
            if (dataTable == null)
            {
                dataTable = new DataTable();
            }
            
            if (!dataTable.Columns.Contains(columnName))
            {
                DataColumn column = new DataColumn(columnName, typeof(string));
                dataTable.Columns.Add(column);
            }
            else
            {
                Console.WriteLine("Column "+columnName +" already exists in the DataTable.");
            }
            return dataTable;
        }
        public static void AddDataRow(DataTable dataTable, string[] values)
        {
            DataRow newRow = dataTable.NewRow();
            for (int i = 0; i < values.Length; i++)
            {
                newRow[i] = values[i];
            }
            dataTable.Rows.Add(newRow);
        }
        public static void AddDataRowfromList(DataTable dataTable, List<string> values)
        {
            DataRow newRow = dataTable.NewRow();
            for (int i = 0; i < values.Count; i++)
            {
                newRow[i] = values[i];
            }
            dataTable.Rows.Add(newRow);
        }

        public static void AddDataRowsFromList(DataTable dataTable, List<List<string>> rows)
        {
            foreach (List<string> values in rows)
            {
                DataRow newRow = dataTable.NewRow();
                for (int i = 0; i < values.Count; i++)
                {
                    newRow[i] = values[i];
                }
                dataTable.Rows.Add(newRow);
            }
        }

        private static void filldictmapwithwpfgrid(KeyValuePair<string, string> dictname, IUIAutomationElement pelement)
        {
            try
            {
                var targetDict = new Dictionary<string, List<string>>();
                if (DataContainer.dictmap.TryGetValue(dictname, out targetDict))
                {
                
                    DataContainer.dictmap.Remove(dictname);
                }
                targetDict = new Dictionary<string, List<string>>();
                
                DataContainer.dictmap[dictname] = targetDict;
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
                        //string ll = getnewvalue(cell);
                       // Console.WriteLine(ll);
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
                                Console.WriteLine("COLUMN  NAME   :  " + value);
                                Console.WriteLine("CELL VALUE   :   " + cell.CurrentName);

                                if (DataContainer.dictmap[dictname].ContainsKey(value))
                                {
                                    DataContainer.dictmap[dictname][value].Add(cell.CurrentName);
                                }
                                else
                                {
                                    DataContainer.dictmap[dictname].Add(value, new List<string> { cell.CurrentName });
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
            catch (Exception ex)
            {

            }
        }

        public  static DataTable fillDataTablewpfgrid( IUIAutomationElement pelement, CustomizedDataKeeperClasses.ModuleStepWiseGridStorrer moduleStepWiseGridStorrer)
        {
            
            DataTable dt = new DataTable();
            try
            {
                
                Dictionary<string, List<string>> targetDict = new Dictionary<string, List<string>>();
                // ApplicationArguments.dictmap[dictname] = targetDict;
                IUIAutomation automation = new CUIAutomation8();

                IUIAutomationCondition conditiongrid = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataGridControlTypeId);
                IUIAutomationElement dataGrid = pelement.FindFirst(TreeScope.TreeScope_Descendants, conditiongrid);
              IUIAutomationCondition conditiondataItem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_DataItemControlTypeId);

                var dataItems = dataGrid.FindAll(TreeScope.TreeScope_Descendants, conditiondataItem);

                // Process each row and map cell values to their corresponding column headers
                for (int i = 0; i < dataItems.Length; i++)
                {

                    IUIAutomationElement dataItem = dataItems.GetElement(i);
                    // Find all cells in the current row
                    IUIAutomationCondition conditioncell = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "cell");

                    var cells = dataItem.FindAll(TreeScope.TreeScope_Children, conditioncell);
                    int columnrowcounter = 0;
                    for (int j = 0; j < cells.Length; j++)
                    {

                        IUIAutomationElement cell = cells.GetElement(j);
                        //string ll = getnewvalue(cell);
                        // Console.WriteLine(ll);
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
                                Console.WriteLine("COLUMN  NAME   :  " + value);
                                Console.WriteLine("CELL VALUE   :   " + cell.CurrentName);

                                if (string.Equals(value,moduleStepWiseGridStorrer.duplicateValue))
                                {
                                    
                                    
                                        value = moduleStepWiseGridStorrer.duplicateValueReplacer[columnrowcounter];
                                    columnrowcounter++;                                    
                                }    
                                if (targetDict.ContainsKey(value))
                                {
                                    
                                    targetDict[value].Add(cell.CurrentName);
                                }
                                else
                                {
                                    targetDict[value] = new List<string> { cell.CurrentName };
                                }
                                
                            }
                           
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                dt = ConvertDictionaryToDataTable(targetDict);
                return dt;


            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        public static DataTable fillDataTablewpfListItems(IUIAutomationElement pelement, CustomizedDataKeeperClasses.ModuleStepWiseGridStorrer moduleStepWiseGridStorrer, DataTable dt)
        {
            try
            {
                Dictionary<string, List<string>> targetDict = new Dictionary<string, List<string>>();
                IUIAutomation automation = new CUIAutomation8();

                IUIAutomationCondition conditiongrid = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListControlTypeId);
                IUIAutomationElement dataGrid = pelement.FindFirst(TreeScope.TreeScope_Descendants, conditiongrid);
                IUIAutomationCondition conditiondataItem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
                var dataItems = dataGrid.FindAll(TreeScope.TreeScope_Descendants, conditiondataItem);

                
                for (int i = 0; i < dataItems.Length; i++)
                {
                    IUIAutomationElement dataItem = dataItems.GetElement(i);
                    
                    IUIAutomationCondition conditioncell = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "custom");
                    var cells = dataItem.FindAll(TreeScope.TreeScope_Children, conditioncell);

                    DataRow newRow = dt.NewRow();

                    for (int j = 0; j < cells.Length; j++)
                    {
                        IUIAutomationElement cell = cells.GetElement(j);
                        try
                        {
                            string className = cell.CurrentClassName;
                            string columnName = null;

                            if (!string.IsNullOrEmpty(className))
                            {
                                string[] classNameParts = className.Split(',');
                                foreach (string part in classNameParts)
                                {
                                    if (part.Trim().StartsWith("FieldName="))
                                    {
                                        columnName = part.Trim().Substring("FieldName=".Length);
                                        break;
                                    }
                                }
                            }

                            string value = cell.CurrentName;
                            bool flag = false;
                           
                            if (!string.IsNullOrEmpty(columnName))
                            {

                                foreach (var key in DataContainer.HeadersxListViewClassNameMappings.Keys)
                                {
                                    List<string> listValues = new List<string>();
                                    if (key.Replace(" ", "") == columnName)
                                    {
                                        columnName = key;
                                        flag = true;
                                        break;
                                    }
                                   

                                    else if (DataContainer.HeadersxListViewClassNameMappings.TryGetValue(key, out listValues))
                                    {

                                        foreach (var listValue in listValues)
                                        {
                                            if (listValue == columnName)
                                            {
                                                columnName = key;
                                                flag = true;
                                                break; 
                                            }
                                        }
                                    }

                                    if (flag)
                                    {
                                        break; 
                                    }

                                }

                                /* if (!dt.Columns.Contains(columnName))
                                 {
                                     dt.Columns.Add(columnName);
                                 }*/

                                if (flag == true)
                                {
                                    newRow[columnName] = value;
                                    flag = false;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + "WHILE ADDING LISTVIEW DATAITEMS ON DATATABLE");
                        }
                    }

                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message+"WHILE ADDING LISTVIEW DATAITEMS ON DATATABLE");  
            }
            return dt;
        }

        public static DataTable ConvertDictionaryToDataTable(Dictionary<string, List<string>> dictionary)
        {
            DataTable dataTable = new DataTable();

          
            foreach (string columnName in dictionary.Keys)
            {
                dataTable.Columns.Add(columnName, typeof(string));
            }

           
            int maxRowCount = dictionary.Values.Max(list => list.Count);

            
            for (int rowIndex = 0; rowIndex < maxRowCount; rowIndex++)
            {
                DataRow row = dataTable.NewRow();

                foreach (string columnName in dictionary.Keys)
                {
                    List<string> columnData = dictionary[columnName];
                    if (rowIndex < columnData.Count)
                    {
                        string cellValue = columnData[rowIndex];
                        double numericValue;
                        if (double.TryParse(cellValue, out numericValue))
                        { 
                            numericValue = Math.Round(numericValue);

                            row[columnName] = numericValue.ToString();
                        }
                        else
                        {
                            row[columnName] = cellValue; 
                        }
                    }
                    else
                    {
                        row[columnName] = "";
                    }
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public static async Task<Dictionary<string, List<string>>> ReadExcelFileAsync(string filePath, string keyColumnName, string sheetName)
        {
            var excelData = new Dictionary<string, List<string>>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                try
                {

                    var worksheet = package.Workbook.Worksheets[sheetName];

                    int rowCount = worksheet.Dimension.Rows;
                    int keyColumnIndex = -1;


                    for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                    {
                        if (worksheet.Cells[1, col].Text == keyColumnName)
                        {
                            keyColumnIndex = col;
                            break;
                        }
                    }

                    if (keyColumnIndex == -1)
                    {
                        Console.WriteLine("Key column " + keyColumnName + " not found in the Excel file.");
                        return excelData;
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        string keyValue = worksheet.Cells[row, keyColumnIndex].Text;

                        if (!excelData.ContainsKey(keyValue))
                        {
                            excelData[keyValue] = new List<string>();
                        }

                        for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                        {
                            if (col != keyColumnIndex)
                            {

                                string cellValue = worksheet.Cells[row, col].Text;
                                if (cellValue.Contains(","))
                                {
                                    
                                    string[] values = cellValue.Split(',');
                                    excelData[keyValue].Add(values[0].Trim()); 
                                    excelData[keyValue].Add(values[1].Trim());
                                }
                                else
                                {
                                   
                                    excelData[keyValue].Add(cellValue.Trim());
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

            return excelData;
        }

        public static  Dictionary<string, List<string>>  ReadExcelFile(string filePath, string keyColumnName, string sheetName)
        {
            var excelData = new Dictionary<string, List<string>>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                try
                {

                    var worksheet = package.Workbook.Worksheets[sheetName];

                    int rowCount = worksheet.Dimension.Rows;
                    int keyColumnIndex = -1;


                    for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                    {
                        if (worksheet.Cells[1, col].Text == keyColumnName)
                        {
                            keyColumnIndex = col;
                            break;
                        }
                    }

                    if (keyColumnIndex == -1)
                    {
                        Console.WriteLine("Key column " + keyColumnName + " not found in the Excel file.");
                        return excelData;
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        string keyValue = worksheet.Cells[row, keyColumnIndex].Text;

                        if (!excelData.ContainsKey(keyValue))
                        {
                            excelData[keyValue] = new List<string>();
                        }

                        for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                        {
                            if (col != keyColumnIndex)
                            {

                                string cellValue = worksheet.Cells[row, col].Text;
                                if (cellValue.Contains(","))
                                {

                                    string[] values = cellValue.Split(',');
                                    excelData[keyValue].Add(values[0].Trim());
                                    excelData[keyValue].Add(values[1].Trim());
                                }
                                else
                                {

                                    excelData[keyValue].Add(cellValue.Trim());
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

            return excelData;
        }

        public static HashSet<string> GetCurrentSuggestionsList(string Window)
        {
            
            HashSet<string> suggestions = new HashSet<string>();

            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                Console.WriteLine(rootElement.ToString());
                if (rootElement != null && !string.IsNullOrEmpty(Window))
                {
                    IUIAutomationCondition aimedWindow = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, Window);

                    IUIAutomationElement Windowelement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, aimedWindow);

                    IUIAutomationCondition paneCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_PaneControlTypeId);
                   
                    IUIAutomationElement activePane = Windowelement.FindFirst(TreeScope.TreeScope_Descendants, paneCondition);

                    IUIAutomationElement MainElement = automation.ContentViewWalker.GetFirstChildElement(activePane);


                    Console.WriteLine("MainElement : " + MainElement.ToString());
                    if (activePane == null)
                    {
                        Console.WriteLine("MainElement isnull");
                    }
                    if (activePane != null)
                    {

                        IUIAutomationCacheRequest cacheRequest = automation.CreateCacheRequest();
                        cacheRequest.AddProperty(UIA_PropertyIds.UIA_ControlTypePropertyId);
                        cacheRequest.TreeScope = TreeScope.TreeScope_Subtree;

                        IUIAutomationCondition lstitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);

                        IUIAutomationElementArray lstelement = MainElement.FindAll(TreeScope.TreeScope_Children, lstitem);

                        IUIAutomationCondition listItemCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
                        IUIAutomationElementArray listItemElements = MainElement.FindAllBuildCache(TreeScope.TreeScope_Descendants, listItemCondition, cacheRequest);


                        for (int i = 0; i < lstelement.Length; i++)
                        {
                            IUIAutomationElement listitem = lstelement.GetElement(i);
                            string listitemText = listitem.GetCurrentPropertyValue(UIA_PropertyIds.UIA_NamePropertyId).ToString();
                            Console.WriteLine(listitemText);
                            listitem.CurrentBoundingRectangle.ToString();
                        }

                        for (int i = 0; i < listItemElements.Length; i++)
                        {

                            IUIAutomationElement listItem = listItemElements.GetElement(i);
                           

                            if (IsElementVisible(listItem) && !IsBoundingRectangleEmpty(listItem))
                            {
                                if (!suggestions.Contains(listItem.CurrentName))
                                suggestions.Add(listItem.CurrentName);

                                Console.WriteLine("Visible Suggested Element: " + listItem.CurrentName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION: " + ex.Message);
            }

            return suggestions;
        }


        public static bool IsElementVisible(IUIAutomationElement element)
        {
            return element.CurrentIsOffscreen == 0;
        }

        public static bool IsBoundingRectangleEmpty(IUIAutomationElement element)
        {
            var boundingRect = element.CurrentBoundingRectangle;
            return boundingRect.left == 0 && boundingRect.top == 0 && boundingRect.right == 0 && boundingRect.bottom == 0;
        }

        public static Dictionary<string, int> GetCurrentSuggestionsList(ref string Window, Dictionary<string, int> dict = null)
        {
            try
            {
                IUIAutomation automation = new CUIAutomation();
                IUIAutomationElement rootElement = automation.GetRootElement();
                Console.WriteLine(rootElement.ToString());
                if (rootElement != null && !string.IsNullOrEmpty(Window))
                {
                    IUIAutomationCondition aimedWindow = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, Window);

                    IUIAutomationElement Windowelement = rootElement.FindFirst(TreeScope.TreeScope_Descendants, aimedWindow);

                    IUIAutomationCondition paneCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_PaneControlTypeId);

                    IUIAutomationElement activePane = Windowelement.FindFirst(TreeScope.TreeScope_Descendants, paneCondition);

                    IUIAutomationElement MainElement = automation.ContentViewWalker.GetFirstChildElement(activePane);


                    Console.WriteLine("MainElement : " + MainElement.ToString());
                    if (activePane == null)
                    {
                        Console.WriteLine("MainElement isnull");
                    }
                    if (activePane != null)
                    {

                        IUIAutomationCacheRequest cacheRequest = automation.CreateCacheRequest();
                        cacheRequest.AddProperty(UIA_PropertyIds.UIA_ControlTypePropertyId);
                        cacheRequest.TreeScope = TreeScope.TreeScope_Subtree;

                        IUIAutomationCondition lstitem = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);

                        IUIAutomationElementArray lstelement = MainElement.FindAll(TreeScope.TreeScope_Children, lstitem);

                        IUIAutomationCondition listItemCondition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ListItemControlTypeId);
                        IUIAutomationElementArray listItemElements = MainElement.FindAllBuildCache(TreeScope.TreeScope_Descendants, listItemCondition, cacheRequest);


                        for (int i = 0; i < lstelement.Length; i++)
                        {
                            IUIAutomationElement listitem = lstelement.GetElement(i);
                            string listitemText = listitem.GetCurrentPropertyValue(UIA_PropertyIds.UIA_NamePropertyId).ToString();
                            Console.WriteLine(listitemText);
                            listitem.CurrentBoundingRectangle.ToString();
                        }

                        for (int i = 0; i < listItemElements.Length; i++)
                        {

                            IUIAutomationElement listItem = listItemElements.GetElement(i);


                            if (IsElementVisible(listItem) && !IsBoundingRectangleEmpty(listItem))
                            {
                                
                                    if (!dict.ContainsKey(listItem.CurrentName))
                                    {
                                        dict[listItem.CurrentName] = i;
                                    }
                                

                                Console.WriteLine("Visible Suggested Element: " + listItem.CurrentName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION: " + ex.Message);
            }

            return dict;
        }

        public static void RemoveEmptyRowsFromAllTables(DataSet dataSet)
        {
            foreach (DataTable table in dataSet.Tables)
            {
                List<DataRow> rowsToRemove = new List<DataRow>();

                foreach (DataRow row in table.Rows)
                {
                    bool isEmpty = true;

                    foreach (object field in row.ItemArray)
                    {
                        if (field != null && field.ToString().Trim() != string.Empty)
                        {
                            isEmpty = false;
                            break;
                        }
                    }

                    if (isEmpty)
                    {
                        rowsToRemove.Add(row);
                    }
                }

                foreach (DataRow rowToRemove in rowsToRemove)
                {
                    table.Rows.Remove(rowToRemove);
                }
            }
        }

        public static void ManageExtraColumn(DataSet ds)
        {
            foreach (DataTable table in ds.Tables)
            {
                for (int i = table.Rows.Count - 1; i >= 0; i--)
                {
                    if (table.Rows[i][0].ToString().Equals(table.Columns[0].ToString()))
                        table.Rows.Remove(table.Rows[i]);
                }
            }
        }
        public static DataSet ConvertExcelToDataSet(string filePath, string parameters = "")
        {
            var dataSet = new DataSet();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    int startRow = worksheet.Dimension.Start.Row;
                    int endRow = worksheet.Dimension.End.Row;
                    int startCol = worksheet.Dimension.Start.Column;
                    int endCol = worksheet.Dimension.End.Column;

                    var mainTable = new DataTable(worksheet.Name);
                    DataTable groupedTable = null;

                    var mainHeaders = new string[endCol - startCol + 1];
                    for (int col = startCol; col <= endCol; col++)
                    {
                        string header = worksheet.Cells[startRow, col].Text;
                        if (header != null)
                            header = header.Trim();

                        if (string.IsNullOrEmpty(header))
                        {
                            bool hasData = false;
                            for (int row = startRow + 1; row <= endRow; row++)
                            {
                                if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Text))
                                {
                                    hasData = true;
                                    break;
                                }
                            }
                            header = hasData ? "CheckBox" : "Column" + col;
                        }

                        string finalHeader = header;
                        int dupCount = 1;
                        while (mainHeaders.Contains(finalHeader))
                        {
                            finalHeader = header + "_" + (dupCount++);
                        }

                        mainHeaders[col - startCol] = finalHeader;
                        mainTable.Columns.Add(finalHeader);
                    }

                    bool groupedHeaderCaptured = false;
                    int groupedHeaderRow = -1;

                    for (int row = startRow + 1; row <= endRow; row++)
                    {
                        int outlineLevel = worksheet.Row(row).OutlineLevel;

                        if (outlineLevel > 0)
                        {
                            if (!groupedHeaderCaptured)
                            {
                                int headerRow = row + 1;
                                groupedHeaderRow = headerRow;
                                groupedTable = new DataTable(worksheet.Name + "_Grouped");

                                for (int col = startCol + 1; col <= endCol; col++)
                                {
                                    string header = worksheet.Cells[headerRow, col].Text;
                                    if (header != null)
                                        header = header.Trim();

                                    if (string.IsNullOrEmpty(header))
                                    {
                                        header = "GroupColumn" + col;
                                        continue;
                                    }

                                    string finalHeader = header;
                                    int dup = 1;
                                    while (groupedTable.Columns.Contains(finalHeader))
                                    {
                                        finalHeader = header + "_" + (dup++);
                                    }

                                    groupedTable.Columns.Add(finalHeader);
                                }

                                groupedHeaderCaptured = true;
                                row++; // skip header row
                                continue;
                            }

                            var gRow = groupedTable.NewRow();
                            for (int col = startCol + 1; col <= endCol; col++)
                            {
                                try
                                {
                                    gRow[col - startCol - 1] = worksheet.Cells[row, col].Text;
                                }
                                catch { }
                            }
                            groupedTable.Rows.Add(gRow);
                        }
                        else
                        {
                            var mRow = mainTable.NewRow();
                            for (int col = startCol; col <= endCol; col++)
                            {
                                mRow[col - startCol] = worksheet.Cells[row, col].Text;
                            }
                            mainTable.Rows.Add(mRow);
                        }
                    }

                    dataSet.Tables.Add(mainTable);
                    if (groupedTable != null && groupedTable.Rows.Count > 0)
                        dataSet.Tables.Add(groupedTable);
                }
            }

            if (parameters.Split(',').Length < 2)
            {
                for (int i = dataSet.Tables.Count - 1; i >= 1; i--)
                {
                    dataSet.Tables.RemoveAt(i);
                }
            }

            return dataSet;
        }


    }
}
