using Prana.BusinessObjects;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PositionManagement
{
    public class FilterDataByExactSymbolTests
    {
        private class MockFilterable : IFilterable
        {
            private string _symbol;
            public MockFilterable(string symbol)
            {
                _symbol = symbol;
            }

            public string GetSymbol() => _symbol;

            int IFilterable.GetAccountID()
            {
                throw new NotImplementedException();
            }

            DateTime IFilterable.GetDate()
            {
                throw new NotImplementedException();
            }

            DateTime IFilterable.GetDateModified()
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactSymbol")]
        public void Filterdata_MatchingSymbol_ReturnsFilteredData()
        {
            // Arrange
            var filter = new FilterDataByExactSymbol
            {
                GivenSymbol = "AAPL"
            };
            object[] dataToFilter = {
                new MockFilterable("AAPL"),
                new MockFilterable("GOOG"),
                new MockFilterable("AAPL")
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicA", 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(2, dataToFilter.Length);
            Assert.All(result, item => Assert.Equal("AAPL", item.GetSymbol()));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactSymbol")]
        public void Filterdata_NonMatchingSymbol_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByExactSymbol
            {
                GivenSymbol = "MSFT"
            };
            object[] dataToFilter = {
                new MockFilterable("AAPL"),
                new MockFilterable("GOOG")
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicB", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactSymbol")]
        public void Filterdata_CaseInsensitiveMatching_ReturnsFilteredData()
        {
            // Arrange
            var filter = new FilterDataByExactSymbol
            {
                GivenSymbol = "aapl"
            };
            object[] dataToFilter = {
                new MockFilterable("AAPL"),
                new MockFilterable("aapl"),
                new MockFilterable("GOOG")
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicC", 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(2, dataToFilter.Length);
            Assert.All(result, item => Assert.Equal("AAPL", item.GetSymbol(), StringComparer.InvariantCultureIgnoreCase));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactSymbol")]
        public void Filterdata_EmptySymbol_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByExactSymbol
            {
                GivenSymbol = string.Empty
            };
            object[] dataToFilter = {
                new MockFilterable("AAPL"),
                new MockFilterable("GOOG")
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicD", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactSymbol")]
        public void Filterdata_EmptyDataToFilter_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByExactSymbol
            {
                GivenSymbol = "AAPL"
            };
            object[] dataToFilter = Array.Empty<object>();

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicE", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }
    }
}
