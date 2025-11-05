using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeSheetDelegation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> casesList = new List<string>();
            string sheetDelegation = ConfigurationManager.AppSettings["-sheetDelegation"];
            string notRunFailedCases = ConfigurationManager.AppSettings["-NotRunFailedCases"];

            string sheetDelegationArg = GetArgValue(args, "-sheetDelegation");
            string notRunFailedCasesArg = GetArgValue(args, "-NotRunFailedCases");

            if (!string.IsNullOrEmpty(sheetDelegationArg))
                sheetDelegation = sheetDelegationArg;

            if (!string.IsNullOrEmpty(notRunFailedCasesArg))
                notRunFailedCases = notRunFailedCasesArg;


            if (sheetDelegation.ToUpper().Equals("TRUE")) 
            { 
                DataSet dataSet = ConvertExcelToDataSet(ConfigurationManager.AppSettings["MasterFile"].ToString());
                string backupFolderPath = ConfigurationManager.AppSettings["BackupFilePath"].ToString();
                foreach (string path in ConfigurationManager.AppSettings["MasterFile"].ToString().Split(','))
                {
                    CreateUniqueBackup(path, backupFolderPath);
                }
                foreach (string path in ConfigurationManager.AppSettings["SamsaraFile"].ToString().Split(','))
                {
                    CreateUniqueBackup(path, backupFolderPath);
                }
                foreach (string path in ConfigurationManager.AppSettings["SamsaraComplianceFile"].ToString().Split(','))
                {
                    CreateUniqueBackup(path, backupFolderPath);
                }
                foreach (string path in ConfigurationManager.AppSettings["EnterpriseFile"].ToString().Split(','))
                {
                    CreateUniqueBackup(path, backupFolderPath);
                }
                foreach (string path in ConfigurationManager.AppSettings["EnterpriseComplianceFile"].ToString().Split(','))
                {
                    CreateUniqueBackup(path, backupFolderPath);
                }
                if (dataSet.Tables[0].Columns.Contains("Member")) 
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        if (dr["Member"].ToString().ToLower().Contains("samsara") && (dr["Dependency"].ToString().ToLower().Contains("none") || dr["Dependency"].ToString().ToLower().Contains("simulator")))
                        {
                            foreach (string path in ConfigurationManager.AppSettings["SamsaraFile"].ToString().Split(','))
                            {
                                AddRowToSheet(ConfigurationManager.AppSettings["SamsaraFile"].ToString(), dr, "Testcase ID", dr["Testcase ID"].ToString());
                            }
                        }
                        else if (dr["Member"].ToString().ToLower().Contains("samsara") && dr["Dependency"].ToString().ToLower().Contains("compliance"))
                        {
                            foreach (string path in ConfigurationManager.AppSettings["SamsaraComplianceFile"].ToString().Split(','))
                            {
                                AddRowToSheet(ConfigurationManager.AppSettings["SamsaraComplianceFile"].ToString(), dr, "Testcase ID", dr["Testcase ID"].ToString());
                            }
                        }
                        else if (dr["Member"].ToString().ToLower().Contains("enterprise") && dr["Dependency"].ToString().ToLower().Contains("compliance"))
                        {
                            foreach (string path in ConfigurationManager.AppSettings["EnterpriseComplianceFile"].ToString().Split(','))
                            {
                                AddRowToSheet(ConfigurationManager.AppSettings["EnterpriseComplianceFile"].ToString(), dr, "Testcase ID", dr["Testcase ID"].ToString());
                            }
                        }
                        else if(dr["Member"].ToString().ToLower().Contains("enterprise"))
                        {
                            foreach (string path in ConfigurationManager.AppSettings["EnterpriseFile"].ToString().Split(','))
                            {
                                AddRowToSheet(ConfigurationManager.AppSettings["EnterpriseFile"].ToString(), dr, "Testcase ID", dr["Testcase ID"].ToString());
                            }
                        }
                    }
                }
                else {
                    throw new Exception(ConfigurationManager.AppSettings["MasterFile"].ToString() + " doesnot have Member column");
                }
            }

            if (notRunFailedCases.ToUpper().Equals("TRUE"))
            {
                foreach (var sheet in ConfigurationManager.AppSettings["NotRunSheets"].ToString().Split(',')) 
                {
                    CreateUniqueBackup(sheet, ConfigurationManager.AppSettings["BackupFilePath"].ToString());
                    DataSet ds = ConvertExcelToDataSet(sheet);
                    MoveFailRowsToBottom(ds.Tables[0]);
                    ChangeStatusToNotRun(ds.Tables[0]);
                    DataSetToExcel(ds, sheet);
                }
            }
        }

        static void DataSetToExcel(DataSet ds, string filePath)
        {
            using (var package = new ExcelPackage())
            {
                foreach (DataTable table in ds.Tables)
                {
                    var worksheet = package.Workbook.Worksheets.Add(table.TableName);

                    // Add column headers
                    for (int col = 0; col < table.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col + 1].Value = table.Columns[col].ColumnName;
                        worksheet.Cells[1, col + 1].Style.Font.Bold = true;
                    }

                    // Add data rows
                    for (int row = 0; row < table.Rows.Count; row++)
                    {
                        for (int col = 0; col < table.Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = table.Rows[row][col]?.ToString();
                        }
                    }

                    // Auto-fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                // Save to file
                var fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }

        private static void ChangeStatusToNotRun(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if (row["Status"].ToString().Equals("Fail") || row["Status"].ToString().Equals("Invalid_Data"))
                {
                    row["Status"] = "Not_Run";
                    row["Retry Counter"] = "0";
                    row["Current IP Address"] = string.Empty;
                    row["Previous IP Address"] = string.Empty;
                    row["Update Time"] = string.Empty;
                }
            }
        }

        private static void MoveFailRowsToBottom(DataTable dt)
        {
            var failRows = dt.AsEnumerable()
                         .Where(r => r.Field<string>("Status")?.Equals("Fail", StringComparison.OrdinalIgnoreCase) == true)
                         .Select(r => r.ItemArray) // clone data
                         .ToList();

            var blankRows = dt.AsEnumerable()
                              .Where(r => IsRowBlank(r))
                              .ToList();

            foreach (DataRow r in dt.AsEnumerable()
                                    .Where(r => r.Field<string>("Status")?.Equals("Fail", StringComparison.OrdinalIgnoreCase) == true)
                                    .ToList())
            {
                dt.Rows.Remove(r);
            }

            int blankIndex = 0;
            foreach (var failData in failRows)
            {
                if (blankIndex < blankRows.Count)
                {
                    var blank = blankRows[blankIndex];
                    blankIndex++;

                    for (int i = 0; i < dt.Columns.Count; i++)
                        blank[i] = failData[i];
                }
                else
                {
                    dt.Rows.Add(failData);
                }
            }
        }

        public static void AddRowToSheet(string filePath, DataRow rowData, string keyColumnName, string keyValue)
        {
            var fileInfo = new FileInfo(filePath);

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[0];

                if (worksheet == null)
                {
                    throw new Exception("Worksheet not found in the Excel file.");
                }

                // Find the Testcase ID column index
                int totalColumns = worksheet.Dimension.End.Column;
                int keyColumnIndex = -1;

                for (int col = 1; col <= totalColumns; col++)
                {
                    var headerValue = worksheet.Cells[1, col].Text.Trim();
                    if (string.Equals(headerValue, keyColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        keyColumnIndex = col;
                        break;
                    }
                }

                if (keyColumnIndex == -1)
                {
                    throw new Exception($"Column '{keyColumnName}' not found in Excel sheet.");
                }

                // Check if keyValue already exists in that column
                int totalRows = worksheet.Dimension.End.Row;
                for (int row = 2; row <= totalRows; row++) // assuming row 1 is header
                {
                    var cellValue = worksheet.Cells[row, keyColumnIndex].Text.Trim();
                    if (string.Equals(cellValue, keyValue, StringComparison.OrdinalIgnoreCase))
                    {
                        //Console.WriteLine($"Key '{keyValue}' already exists in the sheet. Skipping insertion.");
                        return; // Exit without adding
                    }
                }

                // Find next empty row
                int nextEmptyRow = worksheet.Dimension.End.Row + 1;
                int? rowIndex = GetFirstEmptyRowIndex(filePath);
                if (rowIndex.HasValue)
                {
                    nextEmptyRow = rowIndex.Value;
                }

                // Add new row data
                object[] itemArray = rowData.ItemArray;
                for (int i = 0; i < itemArray.Length; i++)
                {
                    worksheet.Cells[nextEmptyRow, i + 1].Value = rowData[i];
                }
                Console.WriteLine($"Key '{keyValue}' added in the sheet.");
                package.Save();
            }
        }

        public static int? GetFirstEmptyRowIndex(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[0];
                if (worksheet == null)
                {
                    throw new Exception($"Worksheet not found.");
                }

                int startRow = worksheet.Dimension.Start.Row;
                int endRow = worksheet.Dimension.End.Row;

                for (int r = startRow + 1; r <= endRow; r++)
                {
                    string firstCellText = worksheet.Cells[r, 1].Text;

                    if (string.IsNullOrWhiteSpace(firstCellText))
                    {
                        return r;
                    }
                }
            }
            return null;
        }

        public static DataSet ConvertExcelToDataSet(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            DataSet ds = new DataSet();
            var fileInfo = new FileInfo(filePath);

            using (var package = new ExcelPackage(fileInfo))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    if (worksheet.Dimension == null) continue; // Skip empty sheets

                    var dt = new DataTable(worksheet.Name);

                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        string header = worksheet.Cells[1, col].Text;
                        if (string.IsNullOrWhiteSpace(header))
                        {
                            header = $"Column {col}";
                        }
                        dt.Columns.Add(header);
                    }

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            dr[col - 1] = worksheet.Cells[row, col].Text.Trim();
                        }
                        dt.Rows.Add(dr);
                    }

                    ds.Tables.Add(dt);
                }
            }

            return ds;
        }

        public static void CreateUniqueBackup(string originalFilePath, string backupFolderPath)
        {
            if (!File.Exists(originalFilePath))
            {
                Console.WriteLine($"Error: Source file not found at {originalFilePath}");
                return;
            }

            try
            {
                if (!Directory.Exists(backupFolderPath))
                {
                    Directory.CreateDirectory(backupFolderPath);
                }

                string originalFileName = Path.GetFileNameWithoutExtension(originalFilePath);
                string originalExtension = Path.GetExtension(originalFilePath);

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

                string backupFileName = $"{originalFileName}_Backup_{timestamp}{originalExtension}";

                string newBackupFilePath = Path.Combine(backupFolderPath, backupFileName);

                File.Copy(originalFilePath, newBackupFilePath, false);

                Console.WriteLine($"  -> Created: {newBackupFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating backup: {ex.Message}");
            }
        }

        public static void CreateDummySourceFile(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(filePath, "This is the original excel file content.");
        }

        static string GetArgValue(string[] args, string key)
        {
            int index = Array.IndexOf(args, key);
            if (index >= 0 && index < args.Length - 1)
            {
                return args[index + 1];
            }
            return null;
        }

        static bool IsRowBlank(DataRow row)
        {
            foreach (var item in row.ItemArray)
            {
                if (item != null && !string.IsNullOrWhiteSpace(item.ToString()))
                    return false;
            }
            return true;
        }

    }
}
