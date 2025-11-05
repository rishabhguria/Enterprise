using Nirvana.TestAutomation.Interfaces;
using System;
using System.Data;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using TestAutomationFX.Core;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Nirvana.TestAutomation.TestDataProvider
{
    public class OpenXmlDataProvider : ITestDataProvider
    {
        public const string TT_DATE_FORMAT = "{0:MM/dd/yyyy}";
        /// <summary>
        /// Gets the test data.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="rowHeaderIndex">Index of the row header.</param>
        /// <param name="startColumnFrom">The start column from.</param>
        /// <returns></returns>
        public DataSet GetTestData(string filePath, int rowHeaderIndex = 1, int startColumnFrom = 1, string fileType = "")
        {
            return LoadExcelFile(filePath, rowHeaderIndex, startColumnFrom, fileType);
        }

        private static List<string> _dateFormats = new List<string>() 
        {
            "mm-dd-yy",
            "m/d/yyyy",
            "M/d/yyyy"
        };

        /// <summary>
        /// Loads the excel file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="headerIndex">Index of the header.</param>
        /// <param name="columnFrom">The column from.</param>
        /// <returns></returns>
        private DataSet LoadExcelFile(string filePath, int headerIndex, int columnFrom, string fileType = "")
        {
            bool flag = false;
            DataSet ds = new DataSet();
            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(filePath)))
                {
                    foreach (var sheet in xlPackage.Workbook.Worksheets)
                    {
                        int rowHeaderIndex = headerIndex;
                        int startColumnFrom = columnFrom;
                        if (!sheet.Name.StartsWith("_"))
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
                        else if (sheet.Name.StartsWith("_") && fileType.Equals("testCase")) {
                            flag = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Information("Error: " + ex.Message);
            }
            if (!flag && fileType.Equals("testCase"))
            {
                ConfigurationManager.AppSettings["ACCenvironment"] = "true";
                Console.WriteLine("Case is created by ACC tool, Hence enabling ACC environment");
               
                //change TODAY() format on all sheets
                foreach (DataTable table in ds.Tables)
                {
                    try
                    {
                        HashSet<string> AllowedStepsforReplaceTodayPlaceholders = new HashSet<string>(
                                                                                                        (ConfigurationManager.AppSettings["-AllowedStepsforReplaceTodayPlaceholders"] ?? "")
                                                                                                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                                                                            .Select(step => step.Trim()),
                                                                                                        StringComparer.OrdinalIgnoreCase
                                                      
                                                                                                        );
                        string rawTableName = table.TableName;
                        string cleanedTableName = System.Text.RegularExpressions.Regex.Replace(rawTableName, @"\d", "");

                        if (AllowedStepsforReplaceTodayPlaceholders.Contains(cleanedTableName))
                        {
                            ReplaceTodayPlaceholders(table);
                        }
                        else
                        {
                            Console.WriteLine("[SKIP] TODAY() replacement skipped for table: " + rawTableName + " (cleaned: " + cleanedTableName + ")");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to replace TODAY() in sheet: " + table.TableName);
                        Console.WriteLine("Reason: " + ex.Message);
                    }
                }
            }
            else if (fileType.Equals("testCase"))
            {
                ConfigurationManager.AppSettings["ACCenvironment"] = "false";                
            }
            return ds;
        }
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
                            string tempDate = DateHandler(cellValue);
                            string formattedDate = String.Format(
                                TT_DATE_FORMAT,
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
        private static string DateHandler(string input)
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

        
    }
}
