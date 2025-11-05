using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutomationFX.UI;
using OfficeOpenXml;
using ExcelDataReader;
using System.IO;
using System.Text.RegularExpressions;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using Newtonsoft.Json.Linq;


namespace Nirvana.TestAutomation.Utilities
{
    public static class DataUtilities
    {
        private static List<string> _dateFormats = new List<string>() 
        {
            "mm-dd-yy",
            "m/d/yyyy",
            "M/d/yyyy"
        };

        public static void CleanColumnValues(DataTable table, string columnName)
        {
            if (table.Columns.Contains(columnName))
            {
                foreach (DataRow row in table.Rows)
                {
                    if (row[columnName] != DBNull.Value)
                    {
                        string valueStr = row[columnName].ToString();

                        // Remove commas (like in "20,000.00")
                        valueStr = valueStr.Replace(",", "");

                        decimal number;
                        if (decimal.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
                        {
                            // Check if number has no decimal part
                            if (number == Math.Truncate(number))
                            {
                                row[columnName] = ((int)number).ToString();
                            }
                            else
                            {
                                row[columnName] = number.ToString("0.##", CultureInfo.InvariantCulture);
                            }
                        }
                    }
                }
            }
        }

        public static void VerifyDate(DataTable excel1, DataTable ui)
        {
            try
            {
                DataTable excel = excel1.Copy();
                if (ConfigurationManager.AppSettings["ACCenvironment"].ToString().Equals("true"))
                {
                    DataUtilities.RemoveCommas(DataUtilities.RemoveTrailingZeroes(excel1));
                    DataUtilities.RemoveCommas(DataUtilities.RemoveTrailingZeroes(excel));
                }
                if (excel.Columns.Contains("MandatoryColumn"))
                    excel.Columns.Remove("MandatoryColumn");
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (DataColumn dc in excel.Columns)
                {
                    if (dc.ToString().Equals("IsAnotherTaxlotAttributesUpdated"))
                        continue;
                    if (dc.ToString().ToLower().Contains("time") || dc.ToString().ToLower().Contains("date"))
                    {
                        dict.Add(dc.ToString(), "");
                    }
                }
                for (int i = excel.Columns.Count - 1; i >= 0; i--)
                {
                    if (excel.Columns[i].ToString().ToLower().Contains("time") || excel.Columns[i].ToString().ToLower().Contains("date"))
                        continue;
                    if (excel.Columns[i].ToString().Contains("Account") || excel.Columns[i].ToString().ToLower().Contains("symbol") || excel.Columns[i].ToString().ToLower().Contains("qty") || excel.Columns[i].ToString().ToLower().Contains("quantity") || excel.Columns[i].ToString().ToLower().Contains("side") || excel.Columns[i].ToString().ToLower().Contains("price")
                        || excel.Columns[i].ToString().Contains("Broker"))
                        continue;
                    excel.Columns.Remove(excel.Columns[i]);
                }
                bool flag1 = true;
                foreach (DataRow dr in excel.Rows)
                {
                    List<string> keys = new List<string>(dict.Keys);
                    foreach (string key in keys)
                    {
                        if (!string.IsNullOrEmpty(dr[key].ToString()))
                        {
                            dict[key] = dr[key].ToString();
                            dr[key] = string.Empty;
                            flag1 = false;
                        }
                    }
                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(ui), dr);
                    int index = ui.Rows.IndexOf(dtRow);
                    if (index < 0 && !flag1)
                    {
                        throw new Exception("Date verification fail");
                    }
                    foreach (string key in keys)
                    {
                        if (!string.IsNullOrEmpty(dict[key].ToString()))
                        {
                            string formattedDate = "";
                            if (dict[key].ToString() == "N/A")
                            {
                                formattedDate = "NA";
                            }
                            else
                            {
                                string tempDate = DataUtilities.DateHandler(dict[key].ToString());
                                string date = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));
                                DateTime date1 = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                formattedDate = date1.ToString("M/d/yyyy");
                                dict[key] = date;
                            }
                            bool flag = false;
                            if (ui.Rows[index][key].ToString().Contains(dict[key].ToString()))
                            {
                                flag = true;
                            }
                            else if (ui.Rows[index][key].ToString().Contains(formattedDate))
                            {
                                flag = true;
                            }
                            if (!flag)
                            {
                                throw new Exception(key + " column verification error");
                            }
                            else
                            {
                                Console.WriteLine(key + " with value " + ui.Rows[index][key].ToString() + " is verified");
                            }
                        }
                    }
                    foreach (string key in keys)
                    {
                        dict[key] = "";
                    }

                }
                List<string> keys1 = new List<string>(dict.Keys);

                foreach (DataRow dr in excel1.Rows)
                {
                    foreach (string key in keys1)
                    {
                        dr[key] = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Stack Trace:" + ex.StackTrace);
                throw;
            }
        }

        public static void UpdateLiveExcel(string filePath, string newMappings)
        {
            try
            {
                File.WriteAllText(filePath, newMappings);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void UpdateJsonFileKeys(string filePath, string key, int value)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                JObject jsonObj = JObject.Parse(json);
                jsonObj[key] = value;
                File.WriteAllText(filePath, jsonObj.ToString(Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public static void UpdateJson(string filePath, string key, bool value)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(json);
                jsonObject[key] = value;
                File.WriteAllText(filePath, jsonObject.ToString(Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }


        // Check For Non Verification for input steps
        public static bool checkList = false;

        /// <summary>
        /// Gets the matching data rows.
        /// </summary>
        /// <param name="masterDataTable">The master data table.</param>
        /// <param name="rowToMatch">The row to match.</param>
        /// <returns></returns>
        public static DataRow[] GetMatchingDataRows(DataTable masterDataTable1, DataRow rowToMatch, bool dateTimeFlag = false)
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
                for (int i = 0; i < rowToMatch.Table.Columns.Count; i++)
                {
                    string colValue = rowToMatch.ItemArray[i].ToString();
                    if (!string.IsNullOrWhiteSpace(colValue))
                    {
                        if (colValue.Equals(ExcelStructureConstants.BLANK_CONST))
                            colValue = string.Empty;

                        if (DateTime.TryParseExact(rowToMatch[rowToMatch.Table.Columns[i].Caption].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue) && !dateTimeFlag)
                        {
                            dateExpression = string.IsNullOrWhiteSpace(dateExpression) ? dateExpression : dateExpression + " AND ";
                            dateColumn.Add(rowToMatch.Table.Columns[i].Caption);
                            dateExpression = dateExpression + "[" + rowToMatch.Table.Columns[i] + "] = '" + dateValue.Date.ToString("MM/dd/yyyy") + "' ";
                        }
                        else
                        {
                            expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                            expression = expression + "[" + rowToMatch.Table.Columns[i].Caption + "] = '" + colValue.Trim() + "' ";
                        }
                    }
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
                return null;
            }

        }

        /// <summary>
        /// Gets the matching data row.
        /// </summary>
        /// <param name="masterDataTable">The master data table.</param>
        /// <param name="rowToMatch">The row to match.</param>
        /// <returns></returns>
        public static DataRow GetMatchingDataRow(DataTable masterDataTable, DataRow rowToMatch, bool checkFlag = false)
        {
            try
            {
                return GetMatchingDataRows(masterDataTable, rowToMatch, checkFlag).FirstOrDefault();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    return null;
                throw;
            }

        }

        public static DataTable MergeCellLines(DataTable table)
        {
            try
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        object cellValue = table.Rows[i][j];
                        if (cellValue != null)
                        {
                            string cellText = cellValue.ToString();
                            table.Rows[i][j] = cellText.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

            return table;
        }

        public static DataTable RemoveStrings(DataTable table, List<string> stringsToRemove, string replacement)
        {
            try
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        string cellValue = table.Rows[i][j].ToString();
                        foreach (string str in stringsToRemove)
                        {
                            cellValue = cellValue.Replace(str, replacement);
                        }

                        table.Rows[i][j] = cellValue;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

            return table;
        }


        /// <summary>
        /// Gets the matching single data rows.
        /// </summary>
        /// <param name="masterDataTable">The master data table.</param>
        /// <param name="dataTableToMatch">The data table to match.</param>
        /// <returns></returns>
        public static DataRow[] GetMatchingSingleDataRows(DataTable masterDataTable, DataTable dataTableToMatch1, bool checkFlag = false)
        {
            List<DataRow> matchedRows = new List<DataRow>();
            DataTable dataTableToMatch = DataUtilities.RemoveTrailingZeroes(dataTableToMatch1);
            try
            {
                foreach (DataRow dataRow in dataTableToMatch.Rows)
                {
                    DataRow row = GetMatchingDataRow(masterDataTable, dataRow, checkFlag);
                    if (row != null)
                        matchedRows.Add(row);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return matchedRows.ToArray();
        }


        public static DataTable RemovePercent(DataTable table)
        {
            try
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        table.Rows[i][j] = table.Rows[i][j].ToString().Replace("%", "");
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return table;
        }


        /// <summary>
        /// Gets the matching single data rows.
        /// </summary>
        /// <param name="masterDataTable">The master data table.</param>
        /// <param name="dataRowsToMatch">The data rows to match.</param>
        /// <returns></returns>
        public static DataRow[] GetMatchingSingleDataRows(DataTable masterDataTable, DataRow[] dataRowsToMatch, bool checkFlag = false)
        {
            List<DataRow> matchedRows = new List<DataRow>();
            try
            {
                foreach (DataRow dataRow in dataRowsToMatch)
                {
                    DataRow row = GetMatchingDataRow(masterDataTable, dataRow, checkFlag);
                    if (row != null)
                        matchedRows.Add(row);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return matchedRows.ToArray();
        }

        /// <summary>
        /// Gets the matching multiple data rows.
        /// </summary>
        /// <param name="masterDataTable">The master data table.</param>
        /// <param name="dataTableToMatch">The data table to match.</param>
        /// <returns></returns>
        public static DataRow[] GetMatchingMultipleDataRows(DataTable masterDataTable, DataTable dataTableToMatch, string errors, bool checkFlag = false)
        {
            List<DataRow> matchedRows = new List<DataRow>();
            try
            {
                foreach (DataRow dataRow in dataTableToMatch.Rows)
                {
                    List<DataRow> rows = GetMatchingDataRows(masterDataTable, dataRow, checkFlag).ToList();
                    if (rows.Count > 0)
                        matchedRows.AddRange(rows);
                }
            }
            catch (Exception ex)
            {
                errors = ex.Message;
                return null;
            }
            return matchedRows.ToArray();
        }

        /// <summary>
        /// Removes the trailing zeroes.
        /// Optimised version, Karan
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static DataTable RemoveTrailingZeroes(DataTable table)
        {
            SortedSet<int> columnsHavingNoNumber = new SortedSet<int>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (table.Rows[i][j].ToString() == "" || columnsHavingNoNumber.Contains(j))
                    {
                        continue;
                    }
                    Decimal result;
                    if (Decimal.TryParse(table.Rows[i][j].ToString(), out result))
                    {
                        table.Rows[i][j] = result.ToString("0.#############");
                    }
                    else
                    {
                        columnsHavingNoNumber.Add(j);
                    }
                }
            }
            return table;
        }

        /// <summary>
        /// Remove precision and keep up to n number.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="precision"></param>
        /// <returns></returns>

        public static DataTable RemovePrecision(DataTable table, int precision)
        {
            SortedSet<int> columnsHavingNoNumber = new SortedSet<int>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (table.Rows[i][j].ToString() == "" || columnsHavingNoNumber.Contains(j))
                    {
                        continue;
                    }
                    Decimal result;
                    StringBuilder str = new StringBuilder("0.");
                    for (int counter = 1; counter <= precision; counter++)
                    {
                        str.Append("#");
                    }
                    if (Decimal.TryParse(table.Rows[i][j].ToString(), out result))
                    {
                        table.Rows[i][j] = result.ToString(str.ToString());
                    }
                    else
                    {
                        columnsHavingNoNumber.Add(j);
                    }
                }
            }
            return table;
        }


        /// <summary>
        /// Removes the commas.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static DataTable RemoveCommas(DataTable table)
        {
            try
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        table.Rows[i][j] = table.Rows[i][j].ToString().Replace(",", "");
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return table;
        }

        /// <summary>
        /// Converts Dictionary To DataTable
        /// </summary>
        /// <param name="UImap">The dictionary.</param>
        /// <returns></returns>
        public static DataTable ConvertDictionaryToDataTable(Dictionary<string, UIWindow> UImap)
        {
            DataTable UIdata = new DataTable();
            try
            {
                foreach (var col in UImap.Keys)
                {
                    UIdata.Columns.Add(col);
                }
                DataRow dr = UIdata.NewRow();
                foreach (var item in UImap.Keys)
                {
                    string role = UImap[item].MsaaRole.ToString();
                    switch (role)
                    {
                        case "ComboBox":
                        case "Text":
                            dr[item] = UImap[item].Text;
                            break;
                        case "RadioButton":
                        case "CheckButton":
                            dr[item] = UImap[item].Value;
                            break;
                        default: dr[item] = "";
                            break;
                    }
                }
                UIdata.Rows.Add(dr);
            }
            catch (Exception)
            {

                throw;
            }
            return UIdata;
        }
        public static void DeleteRowsIfAllEmpty(DataTable dataTable)
        {
            try
            {
                var rowsToDelete = new System.Collections.Generic.List<DataRow>();

                foreach (DataRow row in dataTable.Rows)
                {
                    bool allEmpty = true;

                    foreach (var column in dataTable.Columns)
                    {
                        var columnName = column.ToString();
                        var cellValue = row[columnName];

                        if (cellValue != DBNull.Value && !string.IsNullOrEmpty(cellValue.ToString()))
                        {
                            allEmpty = false;
                            break;
                        }
                    }

                    if (allEmpty)
                    {
                        rowsToDelete.Add(row);
                    }
                }

                foreach (var rowToDelete in rowsToDelete)
                {
                    dataTable.Rows.Remove(rowToDelete);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting rows: " + ex.Message);
            }
        }
        public static int GetLastUsedRow(ExcelWorksheet sheet)
        {
            if (sheet.Dimension == null) { return 0; } // In case of a blank sheet
            var lastRow = sheet.Dimension.End.Row;

            // Loop through the rows from the last row towards the first row
            for (int row = lastRow; row >= 1; row--)
            {
                bool isBlankRow = true;

                // Loop through the columns of the current row
                for (int column = sheet.Dimension.Start.Column; column <= sheet.Dimension.End.Column; column++)
                {
                    // Check if any cell in the current row is not blank
                    if (sheet.Cells[row, column].Value != null)
                    {
                        isBlankRow = false;
                        break;
                    }
                }

                // If the current row is blank, exclude it and continue searching for the next non-blank row
                if (isBlankRow)
                {
                    lastRow--;
                    continue;
                }
            }

            /*while (row >= 1)
            {
                /*var range = sheet.Cells[row, 1, row, sheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {

                    break;
                }
                row--;
            }*/
            return lastRow;
        }


        public static int GetLastUsedColumn(ExcelWorksheet sheet)
        {
            if (sheet.Dimension == null) { return 0; } // In case of a blank sheet
            var column = sheet.Dimension.End.Column;
            while (column >= 1)
            {
                var range = sheet.Cells[1, column, sheet.Dimension.End.Row, column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                column--;
            }
            return column;
        }


        public static Dictionary<string, int> ColToIndexMapping(ExcelWorksheet sheet, int totalColCount)
        {
            Dictionary<string, int> colToIndexMappingDictionary = new Dictionary<string, int>();

            for (int colCount = 1; colCount <= totalColCount; colCount++)
            {
                colToIndexMappingDictionary.Add(sheet.Cells[1, colCount].Value.ToString(), colCount);
            }

            return colToIndexMappingDictionary;
        }

        public static Dictionary<string, string> GetTestCaseStepMapping(string dataSheetPath, DataTable tableName, string TestCaseToBeRun)
        {
            Dictionary<string, string> StepValueMapping = new Dictionary<string, string>();


            string caseNumber = TestCaseToBeRun.Substring(TestCaseToBeRun.IndexOf("-") + 1);


            int lineNumber = tableName.AsEnumerable().Where(row => row.Field<string>("TestCaseID") == TestCaseToBeRun).Select(row => tableName.Rows.IndexOf(row)).FirstOrDefault();
            try
            {


                int startRow = lineNumber;
                int nextcaselineNumber = (tableName.Rows.Count > lineNumber + 1) ? lineNumber + 1 : tableName.Rows.Count - 1;
                while (nextcaselineNumber < tableName.Rows.Count - 1)
                {
                    if (string.IsNullOrEmpty(tableName.Rows[nextcaselineNumber]["TestCaseID"].ToString()))
                        nextcaselineNumber++;
                    else
                        break;
                }

                if (nextcaselineNumber < tableName.Rows.Count - 1)
                {
                    for (int i = lineNumber; i <= nextcaselineNumber && i < tableName.Rows.Count; i++)
                    {
                        if (!StepValueMapping.ContainsKey(tableName.Rows[i]["Input Sheet 1"].ToString()))
                            StepValueMapping.Add(tableName.Rows[i]["Input Sheet 1"].ToString(), tableName.Rows[i]["Sheet link"].ToString());

                    }
                }
                else if (nextcaselineNumber == tableName.Rows.Count - 1)
                {
                    for (int i = lineNumber; i < tableName.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(tableName.Rows[i]["Input Sheet 1"].ToString()) && !StepValueMapping.ContainsKey(tableName.Rows[i]["Input Sheet 1"].ToString()))
                            StepValueMapping.Add(tableName.Rows[i]["Input Sheet 1"].ToString(), tableName.Rows[i]["Sheet link"].ToString());

                    }

                }

                Dictionary<string, string> StepValueMappingCopy = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> data in StepValueMapping)
                {
                    StepValueMappingCopy.Add(data.Key, data.Value);

                }



                foreach (KeyValuePair<string, string> data in StepValueMappingCopy)
                {
                    if (string.IsNullOrEmpty(data.Key) && string.IsNullOrEmpty(data.Value))
                    {
                        StepValueMapping.Remove(data.Key);
                    }
                    else if (string.IsNullOrEmpty(data.Key) && !string.IsNullOrEmpty(data.Value))
                    {
                        throw new NullReferenceException("INPUT SHEET 1 COLUMN VALUE IS EMPTY BUT SHEET LINK EXIST");
                    }
                }






            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return StepValueMapping;
        }
        public static void DeleteUneccessarySheetsFromWorkbook(string dataSheetPath, Dictionary<string, string> StepValueMapping, List<string> UndefinedModuleSteps)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(dataSheetPath)))
            {
                List<ExcelWorksheet> sheetsToRemove = new List<ExcelWorksheet>();

                //if (StepValueMapping.Count + 3 < package.Workbook.Worksheets.Count)
                //{
                foreach (ExcelWorksheet sheet in package.Workbook.Worksheets)
                {
                    string sheetName = sheet.Name;
                    if (sheetName.Equals("_Modules") || sheetName.Equals("_Settings") || sheetName.Equals("_MasterData"))
                    {
                        //necessarysheets
                    }
                    else
                    {
                        if (!StepValueMapping.ContainsKey(sheetName))
                        {
                            sheetsToRemove.Add(sheet);
                        }
                        else if (UndefinedModuleSteps.Contains(sheetName))
                        {
                            sheetsToRemove.Add(sheet);
                        }

                    }
                }


                foreach (ExcelWorksheet sheet in sheetsToRemove)
                {
                    if (package.Workbook.Worksheets.Contains(sheet))
                    {
                        package.Workbook.Worksheets.Delete(sheet);
                        Console.WriteLine(sheet + " is deleted");
                    }
                }

                package.Save();
            }
            //}
        }
        public static DataSet GetTestCaseTestData(string filePath, int rowHeaderIndex, int startColumnFrom, List<string> UndefinedModuleSteps)
        {
            return getExcelFileData(filePath, rowHeaderIndex, startColumnFrom, UndefinedModuleSteps);
        }


        private static DataSet getExcelFileData(string filePath, int headerIndex, int columnFrom, List<string> UndefinedModuleStep)
        {
            DataSet ds = new DataSet();
            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(filePath)))
                {
                    foreach (var sheet in xlPackage.Workbook.Worksheets)
                    {
                        int rowHeaderIndex = headerIndex;
                        int startColumnFrom = columnFrom;
                        if (!sheet.Name.StartsWith("_") && !UndefinedModuleStep.Contains(sheet.Name))
                        {
                            DataTable dt = new DataTable(sheet.Name);

                            var totalRows = sheet.Dimension.End.Row;
                            var totalColumns = sheet.Dimension.End.Column;

                            //handling merged cells
                            List<string> mergedCellsRange = new List<string>();
                            sheet.MergedCells.ToList().ForEach(x => { mergedCellsRange.Add(x.ToString()); });
                            mergedCellsRange.ForEach(y => { sheet.Cells[y].Merge = false; });

                            //adding columns in data table 
                            List<string> rowHeader = sheet.Cells[rowHeaderIndex, startColumnFrom, rowHeaderIndex, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).ToList();
                            rowHeader.ForEach(colName =>
                            {
                                if (!string.IsNullOrWhiteSpace(colName) && !dt.Columns.Contains(colName.Trim()))
                                    dt.Columns.Add(colName.Trim());
                            });
                            totalColumns = dt.Columns.Count + (startColumnFrom - 1);

                            //adding rows in data table
                            if (totalColumns > 0)
                            {
                                sheet.Calculate();
                                //Get column with date format
                                List<string> columnWithDateFormat = new List<string>();
                                sheet.Cells[rowHeaderIndex + 1, startColumnFrom, rowHeaderIndex + 1, totalColumns].Cast<OfficeOpenXml.ExcelRangeBase>().ToList().ForEach(cell =>
                                {
                                    if (cell != null && _dateFormats.Contains(cell.Style.Numberformat.Format.ToString()))
                                    {
                                        columnWithDateFormat.Add(Regex.Replace(cell.ToString(), @"[\d-]", string.Empty));
                                    }
                                });

                                for (int rowNum = rowHeaderIndex + 1; rowNum <= totalRows; rowNum++) //select starting row here
                                {
                                    //Update date format column value
                                    columnWithDateFormat.ForEach(column =>
                                    {
                                        var cellValue = sheet.Cells[column + rowNum].Value;
                                        if (cellValue != null)
                                        {
                                            long dateNum;
                                            if (long.TryParse(cellValue.ToString().Trim(), out dateNum))
                                            {
                                                DateTime result = DateTime.FromOADate(dateNum);
                                                sheet.Cells[column + rowNum].Value = result.ToString("MM/dd/yyyy");
                                            }
                                        }
                                    });

                                    var cellValues = sheet.Cells[rowNum, startColumnFrom, rowNum, totalColumns].Value;
                                    var values = cellValues as Object[,];
                                    IEnumerable<string> row = null;
                                    if (values != null)
                                        row = (cellValues as Object[,]).Cast<object>().Select(x => x).Select(y => y == null ? string.Empty : y.ToString().Trim());
                                    else
                                    {
                                        string rowValue = cellValues == null ? string.Empty : cellValues.ToString();
                                        row = new List<string> { rowValue };
                                    }
                                    if (row != null && !string.IsNullOrWhiteSpace(string.Join("", row).Trim()))
                                        dt.Rows.Add(row.ToArray());

                                }
                            }

                            ds.Tables.Add(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
            return ds;
        }

        public static Dictionary<string, int> MapColumnsToIndices(DataTable dataTable)
        {
            Dictionary<string, int> columnIndices = new Dictionary<string, int>();

            if (dataTable != null)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    DataColumn column = dataTable.Columns[i];
                    columnIndices[column.ColumnName] = i;
                }
            }

            return columnIndices;
        }

        public static int GetColumnIndex(DataTable dataTable, string columnName)
        {
            if (dataTable != null && dataTable.Columns.Contains(columnName))
            {
                return dataTable.Columns.IndexOf(columnName);
            }
            else
            {

                throw new ArgumentException("Column " + columnName + " not found in the DataTable.");
            }
        }

        public static void IUIAutomationFileLoader()
        {
            try
            {
                if (ApplicationArguments.IUIAutomationDataTables == null)
                {
                    ApplicationArguments.IUIAutomationDataTables = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["IUIAutomationDataFile"]);
                }
                if (ApplicationArguments.IUIAutomationMappingTables == null)
                {
                    ApplicationArguments.IUIAutomationMappingTables = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["IUIAutomationMappingFile"]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public static DataTable RemoveColumnsAndRows(string columnList, DataTable dataTable)
        {

            DataTable filteredDataTable = dataTable.Clone();


            string[] columnsToRemove = columnList.Split(',');


            foreach (string columnName in columnsToRemove)
            {
                if (filteredDataTable.Columns.Contains(columnName.Trim()))
                    filteredDataTable.Columns.Remove(columnName.Trim());
            }


            foreach (DataRow row in dataTable.Rows)
            {

                DataRow newRow = filteredDataTable.NewRow();


                foreach (DataColumn column in filteredDataTable.Columns)
                {
                    newRow[column.ColumnName] = row[column.ColumnName];
                }


                filteredDataTable.Rows.Add(newRow);
            }

            return filteredDataTable;
        }
        public static void RemoveColumnsAndRows(ref string columnList, ref DataTable dataTable)
        {

            string[] columnsToRemove = columnList.Split(',');

            foreach (string columnName in columnsToRemove)
            {
                string trimmedColumnName = columnName.Trim();

                if (dataTable.Columns.Contains(trimmedColumnName))
                {
                    dataTable.Columns.Remove(trimmedColumnName);
                }
            }

            foreach (DataRow row in dataTable.Rows)
            {
                foreach (string columnName in columnsToRemove)
                {
                    string trimmedColumnName = columnName.Trim();

                    if (dataTable.Columns.Contains(trimmedColumnName))
                    {
                        row[trimmedColumnName] = DBNull.Value;
                    }
                }
            }

            dataTable.AcceptChanges();
        }

        /*
                public static void Sorting(UIUltraGrid Main, string sortingASCorDSC, string sortingColumnName, DataTable superset)
                {
                    var gridMssaObject = Main.MsaaObject;
                    var columnNameMsaa = gridMssaObject.FindDescendantByName("Column Headers", 3000);
                    var columnNameMsaa2 = columnNameMsaa.FindDescendantByName(sortingColumnName, 3000);
                    var index = superset.Columns[sortingColumnName];
                    Dictionary<string, int> columnToIndexMapping = columnNameMsaa.FindDescendantByName(sortingColumnName, 3000).GetColumnIndexMaping(superset);
                    Wait(6000);

                    if (index != null)
                    {
                        int colIndex = columnToIndexMapping["sortingColumnName"];
                        gridMssaObject.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, sortingColumnName);
                        Wait(5000);
                    }

                    if (sortingASCorDSC.Equals("ASC"))
                    columnNameMsaa2.Click();

                    else
                    {
                        columnNameMsaa2.Click();
                       columnNameMsaa2.Click();
                }
                }*/

        public static DataTable CopyDataTable(DataTable originalTable, List<string> excludedColumns)
        {
            DataTable newTable = originalTable.Clone();
            foreach (DataRow row in originalTable.Rows)
            {
                DataRow newRow = newTable.Rows.Add();

                foreach (DataColumn column in originalTable.Columns)
                {
                    if (!excludedColumns.Contains(column.ColumnName))
                    {
                        newRow[column.ColumnName] = row[column.ColumnName];
                    }
                }
            }

            return newTable;
        }

        public static string AddSpaceBetweenUppercase(string inputString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(inputString[0]);

            for (int i = 1; i < inputString.Length; i++)
            {
                if (char.IsUpper(inputString[i]) && inputString[i - 1] != ' ')
                {
                    stringBuilder.Append(' ');
                }

                stringBuilder.Append(inputString[i]);
            }

            return stringBuilder.ToString();
        }

        public static void clearTextData(int count = 10, bool useSelectAll = false)
        {
            PressRightArrow(count);
            if (useSelectAll == true)
            {
                PressSelectAll();
            }
            PressBackspace(count);

        }
        static void PressRightArrow(int remainingCount)
        {
            if (remainingCount <= 0)
                return;
            SendKeys.SendWait("{END}");
            SendKeys.SendWait("{Right}");
            Thread.Sleep(100);

            PressRightArrow(remainingCount - 1);

        }
        static void PressBackspace(int remainingCount)
        {
            if (remainingCount <= 0)
                return;

            SendKeys.SendWait("{Backspace}");
            Thread.Sleep(100);

            PressBackspace(remainingCount - 1);
        }
        static void PressSelectAll()
        {
            SendKeys.SendWait("^a");
            Thread.Sleep(100);

        }


        public static bool pickFromMenuItem(UIWindow PopupMenuContext, string itemToSelect, UIWindow futureExpectedWindow = null, string typeOfWindow = null)
        {
            bool isExpectedResultAchieved = false;
            try
            {
                // List<string> li = new List<string>();
                if (PopupMenuContext.IsVisible)
                {
                    var popUpMenuContext = PopupMenuContext.MsaaObject;
                    int itemsCount = (typeOfWindow == "RB") ? popUpMenuContext.CachedChildren[0].ChildCount : popUpMenuContext.ChildCount;
                    for (int i = 0; i < itemsCount; i++)
                    {
                        var childrenContext = (typeOfWindow == "RB") ? popUpMenuContext.CachedChildren[0] : popUpMenuContext;
                        var menuItem = childrenContext.CachedChildren[i];
                        //  
                        //string itemText = menuItem.Value;

                        if (!string.IsNullOrEmpty(menuItem.Name))
                        {
                            //li.Add(menuItem.Name.ToString());
                            string itemName = menuItem.Name.ToString();
                            if (string.Equals(itemName, itemToSelect, StringComparison.OrdinalIgnoreCase))
                            {

                                menuItem.Click();

                                try
                                {
                                    if (futureExpectedWindow != null)
                                    {
                                        if (futureExpectedWindow.IsAttached)
                                        {
                                            isExpectedResultAchieved = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        isExpectedResultAchieved = true;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message + "Expected window not achieved after selecting menu item");
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
            return isExpectedResultAchieved;
        }
        public static void KeyboardInputWithVerification(UIWindow window, string input)
        {
            try
            {
                int maxRetries = 3;
                bool inputSentSuccessfully = false;
                for (int tryCount = 0; tryCount <= maxRetries; tryCount++)
                {
                    try
                    {

                        window.Click(MouseButtons.Left);
                        clearTextData(10, true);
                        window.Click(MouseButtons.Left);
                        Keyboard.SendKeys(input);

                        string actualValue = window.Text.ToString();
                        if (actualValue.Equals(input))
                        {
                            inputSentSuccessfully = true;
                            break;
                        }



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Attempt" + tryCount + " :" + ex.Message);

                    }
                    //Thread.Sleep(1000);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Keyboard.SendKeys(input);
            }
        }
        public static void waitForGridDataToGetVisible(UIUltraGrid grd, int time, int cacheIndex1 = int.MinValue, int cacheIndex2 = int.MinValue, int cacheIndex3 = int.MinValue)
        {
            try
            {
                Stopwatch tmr = new Stopwatch();
                tmr.Start();
                int timer = 0;
                while (!AreGridRowsAccessible(grd, time, cacheIndex1, cacheIndex2, cacheIndex3) && tmr.ElapsedMilliseconds <= time * 15000)
                {
                    ++timer;
                    Console.WriteLine(" Grid data not visible , accessed it " + timer + "times" + "Timer - > " + tmr.ElapsedMilliseconds);
                    Thread.Sleep(1000);

                }
                tmr.Stop();

            }
            catch (Exception ex)
            {
                Log.Error("waitForGridDataToGetVisible failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;

            }
        }
        private static bool AreGridRowsAccessible(UIUltraGrid grd, int time, int cacheIndex1 = int.MinValue, int cacheIndex2 = int.MinValue, int cacheIndex3 = int.MinValue)
        {

            var grdMSAA = grd.MsaaObject;
            var grdDataAccesser = grd.MsaaObject;
            int gridChildCount = 0;
            try
            {
                if (cacheIndex1 > int.MinValue)
                {
                    if (cacheIndex2 > int.MinValue)
                    {
                        if (cacheIndex3 > int.MinValue)
                        {
                            gridChildCount = grdMSAA.CachedChildren[cacheIndex1].CachedChildren[cacheIndex2].CachedChildren[cacheIndex3].CachedChildren.Count;
                            grdDataAccesser = grdMSAA.CachedChildren[cacheIndex1].CachedChildren[cacheIndex2].CachedChildren[cacheIndex3];
                        }

                        else
                        {
                            gridChildCount = grdMSAA.CachedChildren[cacheIndex1].CachedChildren[cacheIndex2].CachedChildren.Count;
                            grdDataAccesser = grdMSAA.CachedChildren[cacheIndex1].CachedChildren[cacheIndex2];
                        }
                    }
                    else
                    {

                        gridChildCount = grdMSAA.CachedChildren[cacheIndex1].CachedChildren.Count;
                        grdDataAccesser = grdMSAA.CachedChildren[cacheIndex1];
                    }

                }

                if (gridChildCount > 0)
                {
                    for (int i = 0; i < gridChildCount; i++)
                    {
                        if (grdDataAccesser.CachedChildren[i].Role == System.Windows.Forms.AccessibleRole.Row)
                        {
                            return true;
                        }

                    }

                    return false;
                }

                else
                    return gridChildCount > 0;
            }

            catch (Exception ex)
            {
                Log.Error("waitForGridDataToGetVisible failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;

            }
            return false;
        }
        public static void CreateOrUpdateExcelFile(ref string fileName, ref  DataTable dataTable, ref string filePath, int columnIndex = 1)
        {
            try
            {

                fileName = Path.ChangeExtension(fileName, ".xlsx");
                string fullFilePath = Path.Combine(filePath, fileName);
                FileInfo file = new FileInfo(fullFilePath);
                using (var package = new ExcelPackage())
                {
                    if (file.Exists)
                    {
                        // If the file exists, load it to check for common columns
                        using (var existingPackage = new ExcelPackage(file))
                        {
                            ExcelWorksheet existingWorksheet = existingPackage.Workbook.Worksheets.FirstOrDefault();
                            if (existingWorksheet != null)
                            {

                                bool columnsMatch = true;
                                for (int i = 0; i < dataTable.Columns.Count; i++)
                                {
                                    if (existingWorksheet.Cells[1, columnIndex + i].Text != dataTable.Columns[i].ColumnName)
                                    {
                                        columnsMatch = false;
                                        break;
                                    }
                                }

                                if (columnsMatch)
                                {
                                    int existingRows = DataUtilities.GetLastUsedRow(existingWorksheet);
                                    for (int row = 0; row < dataTable.Rows.Count; row++)
                                    {
                                        for (int col = 0; col < dataTable.Columns.Count; col++)
                                        {
                                            existingWorksheet.Cells[existingRows + row + 1, columnIndex + col].Value = dataTable.Rows[row][col];
                                        }
                                    }
                                    existingPackage.Save();
                                    Console.WriteLine("Rows added to existing Excel file " + fileName + " at " + filePath);
                                    return;
                                }
                                else
                                {

                                    int lastUsedRow = DataUtilities.GetLastUsedRow(existingWorksheet);
                                    int leastUsedRow = lastUsedRow + 1;
                                    for (int i = 0; i < dataTable.Columns.Count; i++)
                                    {
                                        existingWorksheet.Cells[leastUsedRow, columnIndex + i].Value = dataTable.Columns[i].ColumnName;
                                    }

                                    for (int row = 0; row < dataTable.Rows.Count; row++)
                                    {
                                        for (int col = 0; col < dataTable.Columns.Count; col++)
                                        {
                                            existingWorksheet.Cells[leastUsedRow + 1, columnIndex + col].Value = dataTable.Rows[row][col];
                                        }
                                        leastUsedRow++;
                                    }

                                    existingPackage.Save();
                                    Console.WriteLine("Columns and data added to existing Excel file " + fileName + " at " + filePath);
                                    return;
                                }
                            }
                        }
                    }

                    // If the file doesn't exist or columns don't match, create a new file
                    using (var newPackage = new ExcelPackage(file))
                    {
                        ExcelWorksheet worksheet = newPackage.Workbook.Worksheets.Add(fileName);

                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            worksheet.Cells[1, columnIndex + i].Value = dataTable.Columns[i].ColumnName;
                        }

                        for (int row = 0; row < dataTable.Rows.Count; row++)
                        {
                            for (int col = 0; col < dataTable.Columns.Count; col++)
                            {
                                worksheet.Cells[row + 2, columnIndex + col].Value = dataTable.Rows[row][col];
                            }
                        }

                        newPackage.Save();
                    }
                    Console.WriteLine("Excel file " + fileName + " created or updated successfully at " + filePath);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while generating file" + ex.Message);

            }
        }

        public static void UpdateColumnsInDataTable(ref List<string> columns, ref DataTable dataTable, bool removePreviousColumns = true)
        {
            if (removePreviousColumns == true)
                dataTable.Columns.Clear();

            foreach (string columnName in columns)
            {
                dataTable.Columns.Add(columnName, typeof(string));
            }
        }

        public static void MappingsReader(ref string SheetPath, ref Dictionary<string, StepWiseController> StepProductTypeControlHandler)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(SheetPath)))
                {
                    foreach (ExcelWorksheet sheet in package.Workbook.Worksheets)
                    {
                        if (!sheet.Name.StartsWith("_"))
                        {
                            int rowCount = sheet.Dimension.Rows;

                            for (int row = 2; row <= rowCount; row++)
                            {
                                string testCaseID = sheet.Cells[row, 1].Text;

                                if (!string.IsNullOrEmpty(testCaseID))
                                {
                                    if (!StepProductTypeControlHandler.ContainsKey(testCaseID))
                                    {
                                        StepProductTypeControlHandler[testCaseID] = new StepWiseController();
                                    }
                                    StepProductTypeControlHandler[testCaseID].StepsToRunOnEnterprise = sheet.Cells[row, 2].Text.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList();


                                    StepProductTypeControlHandler[testCaseID].StepsToRunOnSamsara = sheet.Cells[row, 3].Text.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList();




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


        public static string GetUniqueFileName(string baseFileName, string extension)
        {
            string uniqueFileName = "output.txt";
            try
            {
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                uniqueFileName = string.Format("{0}_{1}.{2}", baseFileName, timestamp, extension);
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return uniqueFileName;
        }
        public static List<int> FindIndicesInDataTable(ref DataTable dataTable, ref  string searchString)
        {
            List<int> foundIndices = new List<int>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (dataTable.Rows[i]["Alert Type"].ToString() == searchString)
                {
                    foundIndices.Add(i);
                }
            }

            return foundIndices;
        }

        public static DataSet SetDate(DataSet testdata, string from, string to)
        {
            try
            {
                foreach (DataRow dr in testdata.Tables[0].Rows)
                {
                    var today = DateTime.Now;
                    if (string.IsNullOrEmpty(dr[from].ToString()))
                    {
                        DateTime oneYearAgo = today.AddYears(-1);
                        string formattedDate = oneYearAgo.ToString("MM/dd/yyyy");
                        dr[from] = formattedDate;
                    }
                    if (string.IsNullOrEmpty(dr[to].ToString()))
                    {
                        dr[to] = today.ToString("MM/dd/yyyy");
                    }
                    testdata.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return testdata;

        }
        public static Dictionary<string, AutomationUniqueControlType> CreateDictionaryFromDataTable(DataTable table)
        {
            var dictionary = new Dictionary<string, AutomationUniqueControlType>();

            foreach (DataRow row in table.Rows)
            {
                try
                {
                    if (row["DataName"] != DBNull.Value && !string.IsNullOrEmpty(row["DataName"].ToString()) &&
                        row["AutomationUniqueValue"] != DBNull.Value && !string.IsNullOrEmpty(row["AutomationUniqueValue"].ToString()))
                    {
                        string dataName = row["DataName"].ToString();
                        string automationUniqueValue = row["AutomationUniqueValue"].ToString();
                        string uniquePropertyType = row["UniquePropertyType"] != DBNull.Value && row["UniquePropertyType"] != null
                              ? row["UniquePropertyType"].ToString()
                              : string.Empty;

                        string controlType = row["ControlType"] != DBNull.Value && row["ControlType"] != null
                                             ? row["ControlType"].ToString()
                                             : string.Empty;

                        dictionary[dataName] = new AutomationUniqueControlType
                        {
                            AutomationUniqueValue = automationUniqueValue,
                            UniquePropertyType = uniquePropertyType,
                            ControlType = controlType
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error processing row with DataName: " + row["DataName"] + ". Exception: " + ex.Message);

                }
            }

            return dictionary;
        }

        public static string DateHandler(string input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    if (input.ToUpper().Contains("TODAY()"))
                    {
                        try
                        {
                            var regex = new Regex(@"TODAY\(\)([-+]\d+)?");
                            var match = regex.Match(input.ToUpper());

                            if (match.Success)
                            {
                                string offsetString = match.Groups[1].Value;
                                int offset = string.IsNullOrEmpty(offsetString) ? 0 : int.Parse(offsetString);
                                DateTime resultDate = DateTime.Today.AddDays(offset);
                                return resultDate.ToString("MM/dd/yyyy");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return input;
                        }
                    }
                }
                return input;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return input;
            }
        }


        public static bool VerifyNotAvailableMenuItem(UIWindow PopupMenuContext, string itemToVerify)
        {
            bool isItemNotAvailable = true;

            try
            {
                if (PopupMenuContext.IsVisible)
                {
                    var popUpMenuContext = PopupMenuContext.MsaaObject;
                    int itemsCount = popUpMenuContext.ChildCount;

                    for (int i = 0; i < itemsCount; i++)
                    {
                        var menuItem = popUpMenuContext.CachedChildren[i];

                        if (!string.IsNullOrEmpty(menuItem.Name))
                        {
                            string itemName = menuItem.Name.ToString();
                            if (string.Equals(itemName, itemToVerify, StringComparison.OrdinalIgnoreCase))
                            {
                                isItemNotAvailable = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Popup menu context is not visible.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in verifying menu item: " + ex.Message);
            }

            return isItemNotAvailable;
        }
        public static DataTable MergeDataTablesWithMatchingColumns(DataTable[] tableList, bool removeDuplicateRows)
        {
            DataTable finalTable = new DataTable();

            try
            {
                foreach (DataTable table in tableList)
                {
                    try
                    {
                        if (finalTable.Columns.Count == 0)
                        {
                            finalTable = table.Clone();
                        }

                        foreach (DataRow row in table.Rows)
                        {
                            try
                            {
                                DataRow newRow = finalTable.NewRow();
                                foreach (DataColumn column in table.Columns)
                                {
                                    if (finalTable.Columns.Contains(column.ColumnName))
                                    {
                                        newRow[column.ColumnName] = row[column];
                                    }
                                }

                                if (removeDuplicateRows)
                                {
                                    bool isDuplicate = false;

                                    if (finalTable.Rows.Count > 0)
                                    {
                                        DataRow lastRow = finalTable.Rows[finalTable.Rows.Count - 1];
                                        isDuplicate = AreRowsIdentical(lastRow, newRow);
                                    }

                                    if (!isDuplicate)
                                    {
                                        finalTable.Rows.Add(newRow);
                                    }
                                }
                                else
                                {
                                    finalTable.Rows.Add(newRow);
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }

            return finalTable;
        }

        private static bool AreRowsIdentical(DataRow row1, DataRow row2)
        {
            foreach (DataColumn column in row1.Table.Columns)
            {
                if (!row1[column].Equals(row2[column]))
                {
                    return false;
                }
            }
            return true;
        }

        public static DataTable DeleteDuplicateData(DataTable data, bool removeDuplicateRows)
        {
            try
            {
                if (!removeDuplicateRows || data == null || data.Rows.Count == 0)
                    return data;

                DataTable uniqueData = data.Clone();
                HashSet<string> rowSet = new HashSet<string>();

                foreach (DataRow row in data.Rows)
                {
                    string rowKey = string.Join("|", row.ItemArray);
                    if (!rowSet.Contains(rowKey))
                    {
                        rowSet.Add(rowKey);
                        uniqueData.Rows.Add(row.ItemArray);
                    }
                }

                return uniqueData;
            }
            catch
            {
                return data;
            }
        }

        public static void RemoveEmptyRows(DataSet dataSet)
        {
            try
            {
                if (dataSet == null || dataSet.Tables.Count == 0)
                {
                    return;
                }

                foreach (DataTable table in dataSet.Tables)
                {
                    if (table.Rows.Count == 0)
                        continue;

                    var emptyRows = table.AsEnumerable()
                        .Where(row => row.ItemArray.All(field => field == null || field == DBNull.Value || string.IsNullOrWhiteSpace(field.ToString())))
                        .ToList();

                    foreach (var emptyRow in emptyRows)
                    {
                        table.Rows.Remove(emptyRow);
                    }

                    table.AcceptChanges();
                }
            }
            catch
            {
                return;
            }
        }

        public static void UpdateCheck(string _listNonVerificationOnBlotter)
        {
            try
            {
                if (_listNonVerificationOnBlotter.Contains(ApplicationArguments.TestCaseToBeRun))
                {
                    checkList = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
        public static string RowExpressionIfNotExists(DataRow rowA, DataTable tableB)
        {
            try
            {
                List<DataColumn> matchingColumns = new List<DataColumn>();
                foreach (DataColumn col in rowA.Table.Columns)
                {
                    if (tableB.Columns.Contains(col.ColumnName) &&
                        rowA[col] != null &&
                        rowA[col].ToString() != string.Empty)
                    {
                        matchingColumns.Add(col);
                    }
                }

                List<string> expressionParts = new List<string>();
                foreach (DataColumn col in matchingColumns)
                {
                    string safeValue = rowA[col].ToString().Replace("'", "''");
                    expressionParts.Add(col.ColumnName + "='" + safeValue + "'");
                }

                bool foundMatch = false;
                foreach (DataRow rowB in tableB.Rows)
                {
                    bool allMatch = true;
                    foreach (DataColumn col in matchingColumns)
                    {
                        if (rowB[col.ColumnName] == null ||
                            rowB[col.ColumnName].ToString() != rowA[col.ColumnName].ToString())
                        {
                            allMatch = false;
                            break;
                        }
                    }

                    if (allMatch)
                    {
                        foundMatch = true;
                        break;
                    }
                }

                if (foundMatch)
                    return string.Empty;
                else
                    return string.Join(" AND ", expressionParts.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in RowExpressionIfNotExists: " + ex.Message);
                return string.Empty;
            }
        }

        public static bool RowsMatchInAnyOrder(DataTable tableA, DataTable tableB)
        {
            try
            {
                foreach (DataRow rowA in tableA.Rows)
                {
                    var matchingColumns = tableA.Columns.Cast<DataColumn>()
                        .Where(c => tableB.Columns.Contains(c.ColumnName) &&
                                    rowA[c] != null &&
                                    !string.IsNullOrEmpty(rowA[c].ToString()))
                        .ToList();

                    bool foundMatch = tableB.AsEnumerable().Any(rowB =>
                        matchingColumns.All(c =>
                            rowB[c.ColumnName] != null &&
                            rowB[c.ColumnName].ToString() == rowA[c.ColumnName].ToString()
                        )
                    );

                    if (!foundMatch)
                        return false; 
                }

                return true; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in RowsMatchInAnyOrder: " + ex.Message);
                return false; 
            }
        }

        /// <summary>
        /// Replaces any cell value containing "TODAY" in the given DataTable with a formatted date.
        /// If conversion fails, keeps the original value without throwing.
        /// </summary>
        public static void ReplaceTodayPlaceholders(DataTable inputData)
        {
            if (inputData == null || inputData.Rows.Count == 0)
                return;

            foreach (DataRow dr in inputData.Rows)
            {
                foreach (DataColumn col in inputData.Columns)
                {
                    string cellValue = string.Empty;

                    if (dr[col] != null)
                        cellValue = dr[col].ToString().Trim();

                    if (!string.IsNullOrEmpty(cellValue) && cellValue.ToUpper().Contains("TODAY"))
                    {
                        try
                        {
                            string tempDate = DataUtilities.DateHandler(cellValue);
                            string formattedDate = String.Format(
                                ExcelStructureConstants.TT_DATE_FORMAT,
                                DateTime.Parse(tempDate)
                            );
                            dr[col] = formattedDate; // replace value
                        }
                        catch
                        {
                            // If conversion fails, keep original value
                            dr[col] = cellValue;
                        }
                    }
                }
            }
        }
    }
}
