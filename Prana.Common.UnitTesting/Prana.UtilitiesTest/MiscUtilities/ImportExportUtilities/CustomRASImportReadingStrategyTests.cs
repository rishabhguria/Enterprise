using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Prana.UnitTesting.MockDataCreation;
using Prana.Utilities.MiscUtilities.ImportExportUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities.ImportExportUtilities
{
    public class CustomRASImportReadingStrategyTests
    {
        [Fact]
        [Trait("Prana.Utilities", "CustomRASImportReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_ValidFile_ReturnsDataTable()
        {
            // Arrange
            string fileName = MockDataPath.GetTestingFolderPath() + "custom_ras.xlsx";
            string fileFormat = "xlsx";
            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");

            // Create a header row
            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Column1");
            headerRow.CreateCell(1).SetCellValue("Column2");
            headerRow.CreateCell(2).SetCellValue("Column3");

            // Create some data rows
            for (int i = 1; i <= 10; i++)
            {
                IRow row = sheet.CreateRow(i);
                row.CreateCell(0).SetCellValue("Data" + i);
                row.CreateCell(1).SetCellValue(i * 10);
                row.CreateCell(2).SetCellValue("More Data" + i);
            }

            // Save the file
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }

            workbook.Close();

            // Act
            var result = CustomRASImportReadingStrategy.GetDataTableFromUploadedDataFile(fileName, fileFormat);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Columns.Count > 0);
            Assert.True(result.Rows.Count > 1);
        }

        [Fact]
        [Trait("Prana.Utilities", "CustomRASImportReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_EmptyFile_ReturnsEmptyDataTable()
        {
            // Arrange
            string fileName = MockDataPath.GetTestingFolderPath() + "custom_ras_empty_file.xlsx";
            string fileFormat = "xlsx";
            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");

            // Save the file
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }

            workbook.Close();

            // Act
            var result = CustomRASImportReadingStrategy.GetDataTableFromUploadedDataFile(fileName, fileFormat);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Rows.Count);
        }


        [Fact]
        [Trait("Prana.Utilities", "CustomRASImportReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_ExceptionOccurs_LogsAndThrows()
        {
            // Arrange
            string fileName = MockDataPath.GetTestingFolderPath() + "nonExistent_file.xlsx";
            string fileFormat = "xlsx";

            // Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                CustomRASImportReadingStrategy.GetDataTableFromUploadedDataFile(fileName, fileFormat);
            });

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}
