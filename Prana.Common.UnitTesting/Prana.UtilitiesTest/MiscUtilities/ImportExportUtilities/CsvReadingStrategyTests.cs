using Prana.Common.UnitTesting.MockDataCreation;
using Prana.Utilities.ImportExportUtilities;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities.ImportExportUtilities
{
    public class CsvReadingStrategyTests
    {
        [Fact]
        [Trait("Prana.Utilities", "CsvReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_ShouldReturnEmptyTable_WhenFileIsEmpty()
        {
            // Arrange
            var helper = new CsvReadingStrategy();
            string filePath = CreateCsvFile.CreateCsv("csv_empty.csv", null, new List<string> {});

            // Act
            DataTable result = helper.GetDataTableFromUploadedDataFile(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Rows);
            Assert.Empty(result.Columns);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "CsvReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_ShouldReturnTableWithColumnsAndRows_WhenFileHasData()
        {
            // Arrange
            var helper = new CsvReadingStrategy();
            string filePath = CreateCsvFile.CreateCsv("csv_with_rows1.csv", "COL1,COL2,COL3", new List<string> { "John,30,New York" });

            // Act
            DataTable result = helper.GetDataTableFromUploadedDataFile(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Columns.Count);
            Assert.Equal("COL1", result.Columns[0].ColumnName);
            Assert.Equal("COL2", result.Columns[1].ColumnName);
            Assert.Equal("COL3", result.Columns[2].ColumnName);

            Assert.Equal("John", result.Rows[1][0]);
            Assert.Equal("30", result.Rows[1][1]);
            Assert.Equal("New York", result.Rows[1][2]);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "CsvReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_ShouldHandleQuotedCommasProperly()
        {
            // Arrange
            var helper = new CsvReadingStrategy();
            string filePath = CreateCsvFile.CreateCsv("csv_with_rows2.csv", "COL1,COL2,COL3", new List<string> { "\"John, Doe\",30,\"New York, NY\"" });

            // Act
            DataTable result = helper.GetDataTableFromUploadedDataFile(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John, Doe", result.Rows[1][0]);
            Assert.Equal("30", result.Rows[1][1]);
            Assert.Equal("New York, NY", result.Rows[1][2]);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "CsvReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_ShouldAddColumnsDynamically_WhenDataHasMoreColumnsThanFirstLine()
        {
            // Arrange
            var helper = new CsvReadingStrategy();
            string filePath = CreateCsvFile.CreateCsv("csv_with_rows3.csv", "COL1,COL2,COL3", new List<string> { "Jack,25\nJohn,30,New York" });

            //Act
            DataTable result = helper.GetDataTableFromUploadedDataFile(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Rows.Count);// 1 header row and 2 other rows
            Assert.Equal(3, result.Columns.Count);
            Assert.Equal("John", result.Rows[2][0]);
            Assert.Equal("30", result.Rows[2][1]);
            Assert.Equal("New York", result.Rows[2][2]);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "CsvReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_ShouldRemoveEmptyRows()
        {
            // Arrange
            var helper = new CsvReadingStrategy();
            string filePath = CreateCsvFile.CreateCsv("csv_with_rows4.csv", "COL1,COL2,COL3", new List<string> { "Jack,25,California","John,30,New York\n\n" });

            // Act
            DataTable result = helper.GetDataTableFromUploadedDataFile(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Rows.Count); // 1 header row and 2 other rows

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "CsvReadingStrategy")]
        public void GetDataTableFromUploadedDataFile_ShouldHandleEmptyCells()
        {
            // Arrange
            var helper = new CsvReadingStrategy();
            string filePath = CreateCsvFile.CreateCsv("csv_with_rows5.csv", "COL1,COL2,COL3", new List<string> { "Jack,25,California","\nJohn,,New York" });

            // Act
            DataTable result = helper.GetDataTableFromUploadedDataFile(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Rows.Count); // 1 header row and 2 other rows
            Assert.Equal("John", result.Rows[2][0]);
            Assert.Equal(string.Empty, result.Rows[2][1]);
            Assert.Equal("New York", result.Rows[2][2]);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
