using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Prana.UnitTesting.MockDataCreation;
using Prana.Utilities.MiscUtilities.ImportExportUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities.ImportExportUtilities
{
    public class NpoiXlsxReadingStrategyTests
    {
        [Fact]
        [Trait("Prana.Utilities", "NpoiXlsxReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_ValidXlsFile_ReturnsDataTable()
        {
            // Arrange
            var xlsxReadingStrategy = new NpoiXlsxReadingStrategy();
            var fileName = MockDataPath.GetTestingFolderPath() + "npoi.xlsx";
            var dataTable = CreateTestXlsxFile(fileName);

            // Act
            DataTable result = xlsxReadingStrategy.GetDataTableFromUploadedDataFile(fileName);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dataTable.Rows.Count + 1, result.Rows.Count); // as it will create header with COL1, COL2 and so on
            Assert.Equal(dataTable.Columns.Count, result.Columns.Count);

            for (int i = 1; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    Assert.Equal(dataTable.Rows[i - 1][j], result.Rows[i][j]);
                }
            }
        }

        private DataTable CreateTestXlsxFile(string fileName)
        {
            // Create a new DataTable for comparison
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Age");
            DataRow row = dataTable.NewRow();
            row["ID"] = "1";
            row["Name"] = "John";
            row["Age"] = "20";
            dataTable.Rows.Add(row);

            DataRow row2 = dataTable.NewRow();
            row2["ID"] = "2";
            row2["Name"] = "Jane";
            row2["Age"] = "25";
            dataTable.Rows.Add(row2);

            DataRow row3 = dataTable.NewRow();
            row3["ID"] = "3";
            row3["Name"] = "Sam";
            row3["Age"] = "30";
            dataTable.Rows.Add(row3);

            // Create a new .xls file using NPOI
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Sheet1");

                IRow headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("ID");
                headerRow.CreateCell(1).SetCellValue("Name");
                headerRow.CreateCell(2).SetCellValue("Age");

                IRow dataRow = sheet.CreateRow(1);
                dataRow.CreateCell(0).SetCellValue("1");
                dataRow.CreateCell(1).SetCellValue("John");
                dataRow.CreateCell(2).SetCellValue("20");

                IRow dataRow2 = sheet.CreateRow(2);
                dataRow2.CreateCell(0).SetCellValue("2");
                dataRow2.CreateCell(1).SetCellValue("Jane");
                dataRow2.CreateCell(2).SetCellValue("25");

                IRow dataRow3 = sheet.CreateRow(3);
                dataRow3.CreateCell(0).SetCellValue("3");
                dataRow3.CreateCell(1).SetCellValue("Sam");
                dataRow3.CreateCell(2).SetCellValue("30");

                workbook.Write(fileStream);
            }

            return dataTable;
        }
    }
}
