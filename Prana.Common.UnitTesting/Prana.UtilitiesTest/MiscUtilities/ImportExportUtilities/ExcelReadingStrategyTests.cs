using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using Prana.UnitTesting.MockDataCreation;
using Prana.Utilities.ImportExportUtilities;
using System;
using System.Data;
using System.IO;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities.ImportExportUtilities
{
    public class ExcelReadingStrategyTests
    {
        [Fact]
        [Trait("Prana.Utilities", "ExcelReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_ValidExcelFile_ReturnsNonEmptyDataTable()
        {
            // Arrange
            var fileName = MockDataPath.GetTestingFolderPath() + "xls_excel_valid.xls";

            try
            {
                // Create a new .xls file using NPOI
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Sheet1");
                IRow headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Column1");
                headerRow.CreateCell(1).SetCellValue("Column2");
                IRow dataRow = sheet.CreateRow(1);
                dataRow.CreateCell(0).SetCellValue("Value1");
                dataRow.CreateCell(1).SetCellValue("Value2");
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fileStream);
                }
                workbook.Close();

                var readingStrategy = new ExcelReadingStrategy();
                // Act
                DataTable result = readingStrategy.GetDataTableFromUploadedDataFile(fileName);
                // Assert
                Assert.NotNull(result);
                Assert.True(result.Rows.Count > 0);
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "ExcelReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_FileNotFound_ThrowsFileNotFoundException()
        {
            // Arrange
            var fileName = "nonExistentFile.xlsx";
            var readingStrategy = new ExcelReadingStrategy();
            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => readingStrategy.GetDataTableFromUploadedDataFile(fileName));
        }

        [Fact]
        [Trait("Prana.Utilities", "ExcelReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_NullFileName_ThrowsArgumentNullException()
        {
            // Arrange
            string fileName = null;
            var readingStrategy = new ExcelReadingStrategy();
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => readingStrategy.GetDataTableFromUploadedDataFile(fileName));
        }

        [Fact]
        [Trait("Prana.Utilities", "ExcelReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_EmptyFileName_ThrowsArgumentException()
        {
            // Arrange
            string fileName = "";
            var readingStrategy = new ExcelReadingStrategy();
            // Act & Assert
            Assert.Throws<ArgumentException>(() => readingStrategy.GetDataTableFromUploadedDataFile(fileName));
        }

        [Fact]
        [Trait("Prana.Utilities", "ExcelReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_EmptyExcelFile_ReturnsEmptyDataTable()
        {
            // Arrange
            var fileName = MockDataPath.GetTestingFolderPath() + "xlsx_empty_file.xlsx";
            var defaultReadingStrategy = new DefaultReadingStrategy();
            // Act
            DataTable result = defaultReadingStrategy.GetDataTableFromUploadedDataFile(fileName);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Rows.Count); // Count 1, becuase cloumn of row

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}

