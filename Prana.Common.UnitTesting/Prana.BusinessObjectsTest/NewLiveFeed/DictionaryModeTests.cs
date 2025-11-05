using Xunit;
using Prana.BusinessObjects;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.NewLiveFeed
{
    public class DictionaryModeTests
    {
        private readonly DictionaryMode _dictionaryMode;

        public DictionaryModeTests()
        {
            // Initialize the class under test before each test
            _dictionaryMode = new DictionaryMode();
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DictionaryMode")]
        public void AddOrUpdateData_AddsNewData_WhenSymbolDoesNotExist()
        {
            // Arrange
            var symbolData = new SymbolData { Symbol = "AAPL" };

            // Act
            _dictionaryMode.AddOrUpdateData(symbolData);

            // Assert
            var result = _dictionaryMode.GetData();
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("AAPL", result[0].Symbol);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DictionaryMode")]
        public void AddOrUpdateData_UpdatesExistingData_WhenSymbolExists()
        {
            // Arrange
            var symbolData = new SymbolData { Symbol = "AAPL" };
            _dictionaryMode.AddOrUpdateData(symbolData);

            var updatedSymbolData = new SymbolData { Symbol = "AAPL" };

            // Act
            _dictionaryMode.AddOrUpdateData(updatedSymbolData);

            // Assert
            var result = _dictionaryMode.GetData();
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("AAPL", result[0].Symbol);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DictionaryMode")]
        public void GetData_ReturnsDataAndClearsDictionary()
        {
            // Arrange
            var symbolData = new SymbolData { Symbol = "AAPL" };
            _dictionaryMode.AddOrUpdateData(symbolData);

            // Act
            var result = _dictionaryMode.GetData();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("AAPL", result[0].Symbol);

            var resultAfterClear = _dictionaryMode.GetData();
            Assert.Null(resultAfterClear);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DictionaryMode")]
        public void GetData_ReturnsNull_WhenNoDataExists()
        {
            // Act
            var result = _dictionaryMode.GetData();

            // Assert
            Assert.Null(result);
        }
    }
}
