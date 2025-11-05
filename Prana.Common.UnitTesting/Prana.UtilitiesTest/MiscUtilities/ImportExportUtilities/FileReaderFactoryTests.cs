using System;
using System.Data;
using Xunit;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.MiscUtilities.ImportExportUtilities;
using Prana.UnitTesting.MockDataCreation;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Text;
using NPOI.XSSF.UserModel;
using Prana.Common.UnitTesting.MockDataCreation;
using System.Collections.Generic;
namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities.ImportExportUtilities

{
    public class FileReaderFactoryTests
    {
        [Fact]
        [Trait("Prana.Utilities", "FileReaderFactory")]
        public void Create_ShouldReturnCorrectStrategyForCsvFormat()
        {
            // Act
            var strategy = FileReaderFactory.Create(DataSourceFileFormat.Csv);

            // Assert
            Assert.NotNull(strategy);
            Assert.IsType<CsvReadingStrategy>(strategy);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileReaderFactory")]
        public void Create_ShouldReturnCorrectStrategyForXlsxFormat()
        {
            // Act
            var strategy = FileReaderFactory.Create(DataSourceFileFormat.Xlsx);

            // Assert
            Assert.NotNull(strategy);
            Assert.IsType<NpoiXlsxReadingStrategy>(strategy);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileReaderFactory")]
        public void Create_ShouldThrowExceptionForUnknownFormat()
        {
            // Arrange
            var unknownFormat = (DataSourceFileFormat)999; // An invalid format

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => FileReaderFactory.Create(unknownFormat));
            Assert.Equal("Could not find a FileFormatStrategy implementation for this fileFormatType", exception.Message);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileReaderFactory")]
        public void GetDataTableFromDifferentFileFormats_ShouldReturnDataTableForCsvFile()
        {
            // Arrange
            // Add some data rows
            List<string> rows = new List<string>();
            for (int i = 1; i <= 10; i++)
            {
                rows.Add($"Data{i},{i * 10},More Data{i}");
            }
            string filePath = CreateCsvFile.CreateCsv("csv_empty.csv", "Column1,Column2,Column3", rows);

            // Act
            var result = FileReaderFactory.GetDataTableFromDifferentFileFormats(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DataTable>(result);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "FileReaderFactory")]
        public void GetDataTableFromDifferentFileFormats_ShouldReturnDataTableForXlsFile()
        {
            var fileName = MockDataPath.GetTestingFolderPath() + "file_reader_factory.xls";
            try
            {
                // Arrange
                HSSFWorkbook workbook = new HSSFWorkbook();
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
                var result = FileReaderFactory.GetDataTableFromDifferentFileFormats(fileName);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<DataTable>(result);

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
        [Trait("Prana.Utilities", "FileReaderFactory")]
        public void GetDataTableFromDifferentFileFormatsNew_ShouldReturnDataTableForXlsxFile()
        {
            // Arrange
            var fileName = MockDataPath.GetTestingFolderPath() + "file_reader_factory.xlsx";
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
            var result = FileReaderFactory.GetDataTableFromDifferentFileFormatsNew(fileName);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DataTable>(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileReaderFactory")]
        public void GetDataTableFromDifferentFileFormatsNew_ShouldReturnDataTableForTextFile()
        {
            // Arrange
            var fileName = MockDataPath.GetTestingFolderPath() + "text_file.txt";

            // Act
            var result = FileReaderFactory.GetDataTableFromDifferentFileFormatsNew(fileName);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DataTable>(result);
        }
    }
}