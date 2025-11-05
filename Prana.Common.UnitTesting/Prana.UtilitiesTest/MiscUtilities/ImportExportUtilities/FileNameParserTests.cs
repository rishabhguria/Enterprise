using Castle.Core.Logging;
using Moq;
using Prana.BusinessObjects;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities.ImportExportUtilities
{
    public class FileNameParserTests
    {
        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameFromNamingConvention_ShouldParseValidFileName()
        {
            // Arrange
            string nameBeforeParsing = "a{M/d/yyyy}a{M/d/yy}a{MM/dd/yy}a{MM/dd/yy}a{yy/MM/dd}a{yyyy-MM-dd}a{dd-MMM-yy}a{dd}a{MMM}a{yy}a{yyyy}a{MM}a{}.csv";
            DateTime date = new DateTime(2014, 4, 3);

            // Act
            string result = FileNameParser.GetFileNameFromNamingConvention(nameBeforeParsing, date);

            // Assert
            Assert.Equal("a4/3/2014a4/3/14a04/03/14a04/03/14a14/04/03a2014-04-03a03-Apr-14a03aApra14a2014a04a.csv", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameFromNamingConvention_ShouldHandleEmptyBraces()
        {
            // Arrange
            string nameBeforeParsing = "file_{}.txt";
            DateTime date = new DateTime(2023, 8, 14);

            // Act
            string result = FileNameParser.GetFileNameFromNamingConvention(nameBeforeParsing, date);

            // Assert
            Assert.Equal("file_.txt", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameFromNamingConvention_ShouldReturnUnchanged_WhenNoBraces()
        {
            // Arrange
            string nameBeforeParsing = "filename.csv";
            DateTime date = DateTime.Now;

            // Act
            string result = FileNameParser.GetFileNameFromNamingConvention(nameBeforeParsing, date);

            // Assert
            Assert.Equal("filename.csv", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameFromNamingConvention_ShouldHandleMismatchedBraces()
        {
            // Arrange
            string nameBeforeParsing = "file_{yyyy-MM-dd.txt";
            DateTime date = DateTime.Now;

            // Act
            string result = FileNameParser.GetFileNameFromNamingConvention(nameBeforeParsing, date);

            // Assert
            Assert.Equal("file_{yyyy-MM-dd.txt", result); // Since braces don't match, should return the original string.
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameFromNamingConvention_ShouldHandleMultipleEmptyBraces()
        {
            // Arrange
            string nameBeforeParsing = "file_{}_{}_.txt";
            DateTime date = new DateTime(2024, 1, 1);

            // Act
            string result = FileNameParser.GetFileNameFromNamingConvention(nameBeforeParsing, date);

            // Assert
            Assert.Equal("file___.txt", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameFromNamingConvention_ShouldHandleSingleBrace()
        {
            // Arrange
            string nameBeforeParsing = "file_{yyyy-MM-dd}.txt";
            DateTime date = new DateTime(2023, 8, 14);

            // Act
            string result = FileNameParser.GetFileNameFromNamingConvention(nameBeforeParsing, date);

            // Assert
            Assert.Equal("file_2023-08-14.txt", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameUsingExtension_ShouldReturnCorrectFileName_WhenCsvExtension()
        {
            // Arrange
            string originalFilePath = "ENLANDER_trades_20140409.CSV.asc.14.04.04_01_2";

            // Act
            string result = FileNameParser.GetFileNameUsingExtension(originalFilePath);

            // Assert
            Assert.Equal("ENLANDER_trades_20140409.CSV", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameUsingExtension_ShouldReturnCorrectFileName_WhenXlsExtension()
        {
            // Arrange
            string originalFilePath = "ENLANDER_trades_20140409.XLS.asc.14.04.04_01_2";

            // Act
            string result = FileNameParser.GetFileNameUsingExtension(originalFilePath);

            // Assert
            Assert.Equal("ENLANDER_trades_20140409.XLS", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameUsingExtension_ShouldReturnCorrectFileName_WhenTxtExtension()
        {
            // Arrange
            string originalFilePath = "ENLANDER_trades_20140409.TXT.asc.14.04.04_01_2";

            // Act
            string result = FileNameParser.GetFileNameUsingExtension(originalFilePath);

            // Assert
            Assert.Equal("ENLANDER_trades_20140409.TXT", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameUsingExtension_ShouldReturnEmpty_WhenNoValidExtensionFound()
        {
            // Arrange
            string originalFilePath = "ENLANDER_trades_20140409.asc.14.04.04_01_2";

            // Act
            string result = FileNameParser.GetFileNameUsingExtension(originalFilePath);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameUsingExtension_ShouldHandleMixedCaseExtensions()
        {
            // Arrange
            string originalFilePath = "ENLANDER_trades_20140409.cSv.asc.14.04.04_01_2";

            // Act
            string result = FileNameParser.GetFileNameUsingExtension(originalFilePath);

            // Assert
            Assert.Equal("ENLANDER_trades_20140409.cSv", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameUsingExtension_ShouldHandleNoExtension()
        {
            // Arrange
            string originalFilePath = "ENLANDER_trades_20140409";

            // Act
            string result =  FileNameParser.GetFileNameUsingExtension(originalFilePath);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameUsingExtension_ShouldHandleMultipleExtensions()
        {
            // Arrange
            string originalFilePath = "ENLANDER_trades_20140409.CSV.asc.14.04.04.CSV_01_2";

            // Act
            string result = FileNameParser.GetFileNameUsingExtension(originalFilePath);

            // Assert
            Assert.Equal("ENLANDER_trades_20140409.CSV.asc.14.04.04.CSV", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameUsingExtension_ShouldHandleNullInput()
        {
            // Arrange
            string originalFilePath = null;

            // Act & Assert
            Assert.Throws<System.NullReferenceException>(() => FileNameParser.GetFileNameUsingExtension(originalFilePath));
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetFileNameUsingExtension_ShouldHandleEmptyStringInput()
        {
            // Arrange
            string originalFilePath = "";

            // Act
            string result = FileNameParser.GetFileNameUsingExtension(originalFilePath);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetDateStringFromFileName_ValidFormat_ReturnsCorrectDate()
        {
            // Arrange
            string fileNameSyntax = "report_{yyyyMMdd}.txt";
            string fileName = "report_20230814.txt";
            string expectedDate = "20230814";

            // Act
            string actualDate = FileNameParser.GetDateStringFromFileName(fileNameSyntax, fileName);

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetDateStringFromFileName_InvalidFormat_ReturnsEmptyString()
        {
            // Arrange
            string fileNameSyntax = "report_{yyyyMMdd.txt";
            string fileName = "report_20230814.txt";

            // Act
            string actualDate = FileNameParser.GetDateStringFromFileName(fileNameSyntax, fileName);

            // Assert
            Assert.Equal(string.Empty, actualDate);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetDateStringFromFileName_DifferentDateFormat_ReturnsCorrectDate()
        {
            // Arrange
            string fileNameSyntax = "log_{ddMMyyyy}.log";
            string fileName = "log_14082023.log";
            string expectedDate = "14082023";

            // Act
            string actualDate = FileNameParser.GetDateStringFromFileName(fileNameSyntax, fileName);

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetDateStringFromFileName_EmptyBracesInSyntax_ReturnsEmptyString()
        {
            // Arrange
            string fileNameSyntax = "file_{}.txt";
            string fileName = "file_12345.txt";

            // Act
            string actualDate = FileNameParser.GetDateStringFromFileName(fileNameSyntax, fileName);

            // Assert
            Assert.Equal(string.Empty, actualDate);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetDateStringFromFileName_NoBracesInSyntax_ReturnsEmptyString()
        {
            // Arrange
            string fileNameSyntax = "report.txt";
            string fileName = "report.txt";

            // Act
            string actualDate = FileNameParser.GetDateStringFromFileName(fileNameSyntax, fileName);

            // Assert
            Assert.Equal(string.Empty, actualDate);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetDateStringFromFileName_DifferentLengths_ReturnsCorrectDate()
        {
            // Arrange
            string fileNameSyntax = "log_{yyyy_MM_dd_HH_mm_ss}.log";
            string fileName = "log_2023_08_14_12_00_00.log";
            string expectedDate = "2023_08_14_12_00_00";

            // Act
            string actualDate = FileNameParser.GetDateStringFromFileName(fileNameSyntax, fileName);

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [InlineData("Report_{yyyyMMdd}.txt", "yyyyMMdd")]
        [InlineData("Data_{dd_MM_yyyy}_v1.csv", "dd_MM_yyyy")]
        [InlineData("Export_{yyyy-MM-dd}_final.xlsx", "yyyy-MM-dd")]
        [InlineData("Log_{yyyyMMdd_HHmmss}.log", "yyyyMMdd_HHmmss")]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetDateFormatFromFileName_ValidFileName_ReturnsDateFormat(string fileName, string expectedDateFormat)
        {
            // Act
            string result = FileNameParser.GetDateFormatFromFileName(fileName);

            // Assert
            Assert.Equal(expectedDateFormat, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetDateFormatFromFileName_EmptyBraces_ReturnsEmptyString()
        {
            // Arrange
            string fileName = "Report_{}.txt";

            // Act
            string result = FileNameParser.GetDateFormatFromFileName(fileName);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetDateFormatFromFileName_NoBraces_ReturnsEmptyString()
        {
            // Arrange
            string fileName = "Report_20240814.txt";

            // Act
            string result = FileNameParser.GetDateFormatFromFileName(fileName);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "FileNameParser")]
        public void GetDateFormatFromFileName_NoDateFormat_ReturnsEmptyString()
        {
            // Arrange
            string fileName = "Report_{}.txt"; // Braces with no format inside

            // Act
            string result = FileNameParser.GetDateFormatFromFileName(fileName);

            // Assert
            Assert.Empty(result);
        }
    }
}
