using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.UIAutomation
{
    public class TestCaseCreation
    {
        public void GenerateXlsxTestCaseSheetSyncDateTimeManager(DataSet GlobalDataSet, string basefolder, string TestCase, int dataStartingRow, bool fileReplace)
        {
            try
            {
                string testCaseName = Path.GetFileNameWithoutExtension(TestCase);
                string testCaseFolder = Path.Combine(basefolder, testCaseName);
                if (!Directory.Exists(testCaseFolder))
                {
                    Directory.CreateDirectory(testCaseFolder);
                }

                string filePath = Path.Combine(testCaseFolder, testCaseName + ".xlsx");

                Console.WriteLine("Final Excel File Path: " + filePath);
                if (fileReplace && File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine("Existing file deleted: " + filePath);
                }

                using (ExcelPackage package = new ExcelPackage())
                {
                    foreach (DataTable table in GlobalDataSet.Tables)
                    {
                        if (table.TableName.IndexOf("Sheet", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            continue;
                        }

                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(table.TableName);

                        int startRow = dataStartingRow > 0 ? dataStartingRow : 5;
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            var headerCell = worksheet.Cells[startRow, i + 2];
                            headerCell.Value = table.Columns[i].ColumnName;
                            headerCell.Style.Font.Bold = true;
                            headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        }


                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            for (int j = 0; j < table.Columns.Count; j++)
                            {
                                string cellValue = table.Rows[i][j].ToString();
                                double tempDouble;
                                DateTime cellDate;

                                if (double.TryParse(cellValue, out tempDouble))
                                {
                                    worksheet.Cells[i + startRow + 1, j + 2].Value = cellValue;
                                    continue;
                                }

                                if (DateTime.TryParse(cellValue, out cellDate))
                                {
                                    string formula = DateDataAdjuster.GetExcelDateFormulaAsString(cellDate);
                                    worksheet.Cells[i + startRow + 1, j + 2].Value = formula;
                                }
                                else
                                {
                                    try
                                    {
                                        string cellText = table.Rows[i][j].ToString();
                                        Match dateMatch = Regex.Match(cellText, @"\d{1,2}/\d{1,2}/\d{4}");
                                        DateTime embeddedDate;

                                        if (dateMatch.Success && DateTime.TryParse(dateMatch.Value, out embeddedDate))
                                        {
                                            string todayFormula = DateDataAdjuster.GetExcelDateFormulaAsString(embeddedDate);
                                            string updatedText = cellText.Replace(dateMatch.Value, todayFormula);
                                            worksheet.Cells[i + startRow + 1, j + 2].Value = updatedText;
                                        }
                                        else
                                        {
                                            worksheet.Cells[i + startRow + 1, j + 2].Value = cellText;
                                        }
                                    }
                                    catch (Exception exInner)
                                    {
                                        Console.WriteLine("Error: " + exInner.Message);
                                        worksheet.Cells[i + startRow + 1, j + 2].Value = table.Rows[i][j];
                                    }
                                }
                                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                                for (int col = 1; col <= table.Columns.Count + 1; col++)
                                {
                                    if (worksheet.Column(col).Width < 15)
                                        worksheet.Column(col).Width = 15;
                                }
                            }
                        }
                    }

                    FileInfo file = new FileInfo(filePath);
                    package.SaveAs(file);
                    Console.WriteLine("File saved at: " + filePath);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }




    }
}
