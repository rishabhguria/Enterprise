using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;

namespace Nirvana.TestAutomation.Utilities
{
    public static class WinAppHelper
    {
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

    }
}
