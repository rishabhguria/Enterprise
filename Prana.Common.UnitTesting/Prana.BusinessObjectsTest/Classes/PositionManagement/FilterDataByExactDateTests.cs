using System;
using Xunit;
using Prana.BusinessObjects;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PositionManagement
{
    public class FilterDataByExactDateTests
    {
        private class MockFilterable : IFilterable
        {
            private DateTime _date;
            public MockFilterable(DateTime date)
            {
                _date = date;
            }

            public DateTime GetDate() => _date;

            int IFilterable.GetAccountID()
            {
                throw new NotImplementedException();
            }

            DateTime IFilterable.GetDateModified()
            {
                throw new NotImplementedException();
            }

            string IFilterable.GetSymbol()
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactDate")]
        public void Filterdata_GivenMatchingDate_ReturnsFilteredData()
        {
            // Arrange
            var filter = new FilterDataByExactDate { GivenDate = new DateTime(2023, 10, 29) };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 10, 29)),
                new MockFilterable(new DateTime(2023, 10, 30))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicA", 1);

            // Assert
            Assert.Single(result);
            Assert.Equal(new DateTime(2023, 10, 29), result[0].GetDate());
            Assert.Equal(1, dataToFilter.Length);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactDate")]
        public void Filterdata_GivenNonMatchingDate_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByExactDate { GivenDate = new DateTime(2023, 10, 31) };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 10, 29)),
                new MockFilterable(new DateTime(2023, 10, 30))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicB", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactDate")]
        public void Filterdata_EmptyData_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByExactDate { GivenDate = new DateTime(2023, 10, 29) };
            object[] dataToFilter = Array.Empty<object>();

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicC", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactDate")]
        public void Filterdata_MultipleMatchingDates_ReturnsAllMatchingData()
        {
            // Arrange
            var filter = new FilterDataByExactDate { GivenDate = new DateTime(2023, 10, 29) };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 10, 29)),
                new MockFilterable(new DateTime(2023, 10, 29)),
                new MockFilterable(new DateTime(2023, 10, 30))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicD", 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.Equal(new DateTime(2023, 10, 29), item.GetDate()));
            Assert.Equal(2, dataToFilter.Length);
        }
    }
}
