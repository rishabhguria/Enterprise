using Nirvana.TestAutomation.Utilities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using OfficeOpenXml.Style;
using System.Linq;

namespace Nirvana.TestAutomation.TestExecutor
{
    public static class CoreModeHelper
    {
        public static Dictionary<string, List<string>> StepMainColumns = new Dictionary<string, List<string>>();

        /// <summary>
        /// Populates StepMainColumns dictionary from the provided DataTable.
        /// Expected columns: "Steps" and "MainColumns"
        /// Handles merging duplicate step names without adding duplicates in columns list.
        /// </summary>
        public static void PopulateStepMainColumns(DataTable sourceTable)
        {
            try
            {
                if (sourceTable == null)
                    throw new ArgumentNullException("sourceTable", "Source table cannot be null.");

                if (!sourceTable.Columns.Contains("Steps") || !sourceTable.Columns.Contains("MainColumns"))
                    throw new ArgumentException("Source table must have 'Steps' and 'MainColumns' columns.");

                for (int i = 0; i < sourceTable.Rows.Count; i++)
                {
                    DataRow row = sourceTable.Rows[i];
                    if (row == null)
                        continue;

                    string stepName = row["Steps"] != null ? row["Steps"].ToString().Trim() : string.Empty;
                    string mainColumnsStr = row["MainColumns"] != null ? row["MainColumns"].ToString().Trim() : string.Empty;

                    if (stepName.Length == 0 || mainColumnsStr.Length == 0)
                        continue;

                    string[] columnsArray = mainColumnsStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (!StepMainColumns.ContainsKey(stepName))
                    {
                        StepMainColumns[stepName] = new List<string>();
                    }

                    // Add columns ensuring no duplicates
                    for (int c = 0; c < columnsArray.Length; c++)
                    {
                        string colName = columnsArray[c].Trim();
                        if (colName.Length > 0 && !StepMainColumns[stepName].Contains(colName))
                        {
                            StepMainColumns[stepName].Add(colName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while populating StepMainColumns: " + ex.Message);
            }
        }

        //public static void UpdateFixingReport()
        //{
        //    try
        //    {
        //        GenerateFixingReport(ApplicationArguments.TestCaseToBeRun);
        //        Console.WriteLine("TestCaseFixingMode Report created successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("An error occurred while creating TestCaseFixingMode Report: " + ex.Message, ex);
        //    }
        //}

        public static void GenerateReport(string testCaseId, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(testCaseId))
                {
                    Console.WriteLine("TestCaseID is null or empty. Report not generated.");
                    return;
                }

                if (!ApplicationArguments.TestCaseFixingDic.ContainsKey(testCaseId))
                {
                    Console.WriteLine("No data found for TestCaseID: " + testCaseId);
                    return;
                }

                string baseFolderPath = ConfigurationManager.AppSettings["TestCaseFixingModeBaseFolder"];
                string newApproach = "DefaultApproach";

                if (ConfigurationManager.AppSettings["UseCustomApproach"] != null &&
                    ConfigurationManager.AppSettings["UseCustomApproach"].ToLower() == "true")
                {
                    newApproach = "CustomApproach";
                    baseFolderPath = baseFolderPath.Replace("DefaultApproach", newApproach);
                }

                if (!Directory.Exists(baseFolderPath))
                {
                    Directory.CreateDirectory(baseFolderPath);
                }

                string reportPath = Path.Combine(baseFolderPath, "Report.xlsx");
                FileInfo fileInfo = new FileInfo(reportPath);
                ExcelPackage package = null;

                try
                {
                    if (fileInfo.Exists)
                        package = new ExcelPackage(fileInfo);
                    else
                        package = new ExcelPackage(new FileInfo(reportPath));

                    ExcelWorksheet ws;

                    if (fileInfo.Exists && package.Workbook.Worksheets.Count > 0)
                        ws = package.Workbook.Worksheets.FirstOrDefault();
                    else
                    {
                        ws = package.Workbook.Worksheets.Add("Report");
                        ws.Cells[1, 1].Value = "TestCaseID";
                        ws.Cells[1, 2].Value = "Approach";
                        ws.Cells[1, 3].Value = "Step";
                        ws.Cells[1, 4].Value = "Updated Columns";
                        ws.Cells[1, 5].Value = "Details";
                        ws.Cells[1, 6].Value = "Status";

                        using (ExcelRange range = ws.Cells[1, 1, 1, 6])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        }
                    }

                    Utilities.TestCaseFixingMode testCaseData = ApplicationArguments.TestCaseFixingDic[testCaseId];
                    int nextRow = (ws.Dimension != null) ? ws.Dimension.End.Row + 1 : 2;

                    // Loop through each step once
                    foreach (string step in testCaseData.StepxDetails.Keys)
                    {
                        List<string> detailsList = testCaseData.StepxDetails[step] ?? new List<string>();
                        List<string> updatedColsList = testCaseData.StepxUpdatedColumns.ContainsKey(step)
                            ? (testCaseData.StepxUpdatedColumns[step] ?? new List<string>())
                            : new List<string>();

                        string combinedDetails = string.Join(Environment.NewLine,
                            detailsList.Where(d => !string.IsNullOrWhiteSpace(d)).Select(d => d.Trim()));

                        string combinedUpdatedCols = string.Join(Environment.NewLine,
                            updatedColsList.Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => c.Trim()));

                        if (string.IsNullOrWhiteSpace(combinedDetails) && string.IsNullOrWhiteSpace(combinedUpdatedCols))
                        {
                            ws.Cells[nextRow, 1].Value = !string.IsNullOrEmpty(testCaseId) ? testCaseId : "N/A";
                            ws.Cells[nextRow, 2].Value = !string.IsNullOrEmpty(testCaseData.Approach) ? testCaseData.Approach : "N/A";
                            ws.Cells[nextRow, 3].Value = !string.IsNullOrEmpty(step) ? step : "N/A";
                            ws.Cells[nextRow, 4].Value = string.Empty;
                            ws.Cells[nextRow, 5].Value = "No Changes";
                            ws.Cells[nextRow, 6].Value = !string.IsNullOrEmpty(status) ? status : "N/A";
                        }
                        else
                        {
                            ws.Cells[nextRow, 1].Value = !string.IsNullOrEmpty(testCaseId) ? testCaseId : "N/A";
                            ws.Cells[nextRow, 2].Value = !string.IsNullOrEmpty(testCaseData.Approach) ? testCaseData.Approach : "N/A";
                            ws.Cells[nextRow, 3].Value = !string.IsNullOrEmpty(step) ? step : "N/A";
                            ws.Cells[nextRow, 4].Value = combinedUpdatedCols;
                            ws.Cells[nextRow, 5].Value = combinedDetails;
                            ws.Cells[nextRow, 6].Value = !string.IsNullOrEmpty(status) ? status : "N/A";
                        }
                        nextRow++;
                    }

                    ws.Cells.AutoFitColumns();
                    package.Save();
                }
                finally
                {
                    if (package != null)
                        package.Dispose();
                }

                Console.WriteLine("Report generated for TestCaseID: " + testCaseId + " with status: " + status);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating report for TestCaseID: " + testCaseId);
                Console.WriteLine(ex.Message);
            }
        }
        public static void DeleteOldFixingReport()
        {
            try
            {
                string baseFolderPath = ConfigurationManager.AppSettings["TestCaseFixingModeBaseFolder"];
                string newApproach = "DefaultApproach";

                if (ConfigurationManager.AppSettings["UseCustomApproach"] != null &&
                    ConfigurationManager.AppSettings["UseCustomApproach"].ToLower() == "true")
                {
                    newApproach = "CustomApproach";
                    baseFolderPath = baseFolderPath.Replace("DefaultApproach", newApproach);
                }

                string reportPath = Path.Combine(baseFolderPath, "Report.xlsx");

                if (File.Exists(reportPath))
                {
                    File.Delete(reportPath);
                    Console.WriteLine("Old fixing report deleted: " + reportPath);
                }
                else
                {
                    Console.WriteLine("No old fixing report found to delete at: " + reportPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DeleteOldFixingReport");
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public static void InitializeFixingReport()
        {
            try
            {
                string baseFolderPath = ConfigurationManager.AppSettings["TestCaseFixingModeBaseFolder"];
                string newApproach = "DefaultApproach";

                if (ConfigurationManager.AppSettings["UseCustomApproach"] != null &&
                    ConfigurationManager.AppSettings["UseCustomApproach"].ToLower() == "true")
                {
                    newApproach = "CustomApproach";
                    baseFolderPath = baseFolderPath.Replace("DefaultApproach", newApproach);
                }

                if (!Directory.Exists(baseFolderPath))
                {
                    Directory.CreateDirectory(baseFolderPath);
                }

                string reportPath = Path.Combine(baseFolderPath, "Report.xlsx");
                FileInfo fileInfo = new FileInfo(reportPath);

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws;

                    string columnConfig = ConfigurationManager.AppSettings["TestCaseFixingReportColumns"];
                    string[] columns = columnConfig.Split(',').Select(c => c.Trim()).ToArray();

                    if (fileInfo.Exists && package.Workbook.Worksheets.Count > 0)
                    {
                        ws = package.Workbook.Worksheets.FirstOrDefault();
                        bool allMatch = true;
                        for (int i = 0; i < columns.Length; i++)
                        {
                            string cellValue = ws.Cells[1, i + 1].Text.Trim();
                            if (!string.Equals(cellValue, columns[i], StringComparison.OrdinalIgnoreCase))
                            {
                                allMatch = false;
                                break;
                            }
                        }

                        if (!allMatch)
                        {
                            ws.Cells.Clear();
                            for (int i = 0; i < columns.Length; i++)
                            {
                                ws.Cells[1, i + 1].Value = columns[i];
                            }
                        }
                    }
                    else
                    {
                        ws = package.Workbook.Worksheets.Add("Report");
                        for (int i = 0; i < columns.Length; i++)
                        {
                            ws.Cells[1, i + 1].Value = columns[i];
                        }

                        using (ExcelRange range = ws.Cells[1, 1, 1, columns.Length])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        }
                    }
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    package.Save();
                }

                Console.WriteLine("InitializeFixingReport generated at: " + reportPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error InitializeFixingReport");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static void WriteFixingRow(TestCaseFixingRow row, string baseFolderPath)
        {
            try
            {
                string reportPath = Path.Combine(baseFolderPath, "Report.xlsx");
                FileInfo fileInfo = new FileInfo(reportPath);

                if (!Directory.Exists(baseFolderPath))
                {
                    Directory.CreateDirectory(baseFolderPath);
                }

                if (!fileInfo.Exists)
                {
                    InitializeFixingReport();
                }

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws;
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        ws = package.Workbook.Worksheets.Add("Report");
                    }
                    else
                    {
                        ws = package.Workbook.Worksheets.FirstOrDefault();
                    }

                    int rowIndex = ws.Dimension == null ? 2 : ws.Dimension.End.Row + 1;

                    ws.Cells[rowIndex, 1].Value = row.TestCaseID;                               // TestCaseID
                    ws.Cells[rowIndex, 2].Value = row.Step;                                     // Step/InputSheet
                    ws.Cells[rowIndex, 3].Value = row.ExcelRow;                                 // Excel Row
                    ws.Cells[rowIndex, 4].Value = string.Join(Environment.NewLine, row.UpdatedColumns.ToArray()); // Updated Columns
                    ws.Cells[rowIndex, 5].Value = string.Join(Environment.NewLine, row.OlderValues.ToArray());    // OlderValue
                    ws.Cells[rowIndex, 6].Value = string.Join(Environment.NewLine, row.NewerValues.ToArray());    // Newer Value
                    ws.Cells[rowIndex, 7].Value = string.Join(Environment.NewLine, row.Details.ToArray());        // Details
                    ws.Cells[rowIndex, 8].Value = row.Approach;
                    ws.Cells[rowIndex, 4, rowIndex, 7].Style.WrapText = false;
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    package.Save();
                }

                Console.WriteLine("Row written to Report.xlsx: " + reportPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in WriteFixingRow");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static void GenerateFixingReport(string testCaseId, TestCaseFixingRow row)
        {
            try
            {
                if (string.IsNullOrEmpty(testCaseId))
                {
                    Console.WriteLine("TestCaseID is null or empty. Report not generated.");
                    return;
                }

                if (!ApplicationArguments.TestCaseFixingDic.ContainsKey(testCaseId))
                {
                    Console.WriteLine("No data found for TestCaseID: " + testCaseId);
                    return;
                }

                string baseFolderPath = ConfigurationManager.AppSettings["TestCaseFixingModeBaseFolder"];
                string newApproach = "DefaultApproach";

                if (ConfigurationManager.AppSettings["UseCustomApproach"] != null &&
                    ConfigurationManager.AppSettings["UseCustomApproach"].ToLower() == "true")
                {
                    newApproach = "CustomApproach";
                    baseFolderPath = baseFolderPath.Replace("DefaultApproach", newApproach);
                }

                if (!Directory.Exists(baseFolderPath))
                {
                    Directory.CreateDirectory(baseFolderPath);
                }

                if (ApplicationArguments.TestCaseFixingRowDictionary == null ||
      !ApplicationArguments.TestCaseFixingRowDictionary.ContainsKey(testCaseId))
                {
                    Console.WriteLine(string.Format("[Warning] TestCaseFixingRowDictionary is null or does not contain TestCaseID: {0}", testCaseId));
                    return;
                }
                WriteFixingRow(row, baseFolderPath);
                //TestCaseFixingRow testcaseRow

                Console.WriteLine("Report generated for TestCaseID: " + testCaseId);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating report for TestCaseID: " + testCaseId);
                Console.WriteLine(ex.Message);
            }
        }

        public static void ResetTestCaseRows()
        {
            ApplicationArguments.TestCaseFixingRowDictionary.Clear();
        }

        public static void DeleteCasesifCaseNotExistinReport()
        {
            try
            {
               
                string baseFolderPath = ConfigurationManager.AppSettings["TestCaseFixingModeBaseFolder"];
                string newApproach = "DefaultApproach";

                if (ConfigurationManager.AppSettings["UseCustomApproach"] != null &&
                    ConfigurationManager.AppSettings["UseCustomApproach"].ToLower() == "true")
                {
                    newApproach = "CustomApproach";
                    baseFolderPath = baseFolderPath.Replace("DefaultApproach", newApproach);
                }

                if (!Directory.Exists(baseFolderPath))
                {
                    return;
                }
                
                string reportPath = Path.Combine(baseFolderPath, "Report.xlsx");
                FileInfo fileInfo = new FileInfo(reportPath);
                ExcelPackage package = null;

                try
                {
                    //access sheet..
                    //find indexes of "TestCaseID" column present at the first row..
                    //
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error generating report for TestCaseID: ");
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("Report generated for TestCaseID: ");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating report for TestCaseID: ");
                Console.WriteLine(ex.Message);
            }
        }

        public static void BackupOldFixingReport()
        {
            try
            {
                string baseFolderPath = ConfigurationManager.AppSettings["TestCaseFixingModeBaseFolder"];
                string newApproach = "DefaultApproach";

                if (ConfigurationManager.AppSettings["UseCustomApproach"] != null &&
                    ConfigurationManager.AppSettings["UseCustomApproach"].ToLower() == "true")
                {
                    newApproach = "CustomApproach";
                    baseFolderPath = baseFolderPath.Replace("DefaultApproach", newApproach);
                }

                string reportPath = Path.Combine(baseFolderPath, "Report.xlsx");

                if (File.Exists(reportPath))
                {
                    DateTime creationDate = File.GetCreationTime(reportPath);
                    DateTime today = DateTime.Today;

                    if (creationDate.Date == today)
                    {
                        Console.WriteLine("Report.xlsx was created today. Skipping backup.");
                        return;
                    }

                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string backupFileName = string.Format("Report_{0}.xlsx", timestamp);
                    string backupPath = Path.Combine(baseFolderPath, backupFileName);

                    if (File.Exists(backupPath))
                    {
                        File.Delete(backupPath);
                    }

                    File.Copy(reportPath, backupPath);
                    File.Delete(reportPath);

                    Console.WriteLine("Old fixing report backed up as: " + backupPath);
                }
                else
                {
                    Console.WriteLine("No old fixing report found to backup at: " + reportPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in BackupOldFixingReport");
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public static void DeleteCaseifCaseNotExistinReport()
        {
            try
            {
                string baseFolderPath = ConfigurationManager.AppSettings["TestCaseFixingModeBaseFolder"];
                string newApproach = "DefaultApproach";

                if (ConfigurationManager.AppSettings["UseCustomApproach"] != null &&
                    ConfigurationManager.AppSettings["UseCustomApproach"].ToLower() == "true")
                {
                    newApproach = "CustomApproach";
                    baseFolderPath = baseFolderPath.Replace("DefaultApproach", newApproach);
                }

                string reportPath = Path.Combine(baseFolderPath, "Report.xlsx");
                if (!File.Exists(reportPath))
                {
                    Console.WriteLine("Report file not found at: " + reportPath);
                    return;
                }

                HashSet<string> validTestCaseIDs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                using (FileStream stream = new FileStream(reportPath, FileMode.Open, FileAccess.Read))
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["Report"];
                    if (worksheet == null)
                    {
                        Console.WriteLine("Sheet 'Report' not found in Report.xlsx");
                        return;
                    }

                    int testCaseIDColumnIndex = -1;
                    int totalColumns = worksheet.Dimension.End.Column;

                    for (int col = 1; col <= totalColumns; col++)
                    {
                        object headerValue = worksheet.Cells[1, col].Value;
                        if (headerValue != null && headerValue.ToString().Trim().Equals("TestCaseID", StringComparison.OrdinalIgnoreCase))
                        {
                            testCaseIDColumnIndex = col;
                            break;
                        }
                    }

                    if (testCaseIDColumnIndex == -1)
                    {
                        Console.WriteLine("'TestCaseID' column not found in Report sheet.");
                        return;
                    }

                    int totalRows = worksheet.Dimension.End.Row;

                    for (int row = 2; row <= totalRows; row++) 
                    {
                        object cellValue = worksheet.Cells[row, testCaseIDColumnIndex].Value;
                        if (cellValue != null)
                        {
                            string testCaseID = cellValue.ToString().Trim();
                            if (!string.IsNullOrEmpty(testCaseID))
                            {
                                validTestCaseIDs.Add(testCaseID);
                            }
                        }
                    }
                }

                string[] subDirectories = Directory.GetDirectories(baseFolderPath);
                foreach (string dir in subDirectories)
                {
                    try
                    {
                        string folderName = Path.GetFileName(dir);
                        if (!validTestCaseIDs.Contains(folderName))
                        {
                            Directory.Delete(dir, true); 
                            Console.WriteLine("[DELETE] Removed directory: " + dir);
                        }
                        else
                        {
                            Console.WriteLine("[KEEP] Directory matched with report: " + dir);
                        }
                    }
                    catch (Exception innerEx)
                    {
                        Console.WriteLine("[ERROR] Failed to delete directory: " + dir);
                        Console.WriteLine(innerEx.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Exception in DeleteCaseifCaseNotExistinReport");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
