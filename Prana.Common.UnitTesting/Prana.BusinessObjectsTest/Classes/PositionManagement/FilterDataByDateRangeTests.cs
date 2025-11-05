using Prana.BusinessObjects;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PositionManagement
{
    public class FilterDataByDateRangeTests
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
        [Trait("Prana.BusinessObjects", "FilterDataByDateRange")]
        public void Filterdata_DatesWithinRange_ReturnsFilteredData()
        {
            // Arrange
            var filter = new FilterDataByDateRange
            {
                FromDate = new DateTime(2023, 10, 1),
                ToDate = new DateTime(2023, 10, 31)
            };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 10, 15)),
                new MockFilterable(new DateTime(2023, 10, 30)),
                new MockFilterable(new DateTime(2023, 11, 1))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicA", 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(new DateTime(2023, 10, 15), result[0].GetDate());
            Assert.Equal(new DateTime(2023, 10, 30), result[1].GetDate());
            Assert.Equal(2, dataToFilter.Length);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByDateRange")]
        public void Filterdata_DatesOutsideRange_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByDateRange
            {
                FromDate = new DateTime(2023, 10, 1),
                ToDate = new DateTime(2023, 10, 31)
            };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 9, 30)),
                new MockFilterable(new DateTime(2023, 11, 1))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicB", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByDateRange")]
        public void Filterdata_DateRangeInclusive_ReturnsEdgeData()
        {
            // Arrange
            var filter = new FilterDataByDateRange
            {
                FromDate = new DateTime(2023, 10, 1),
                ToDate = new DateTime(2023, 10, 31)
            };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 10, 1)),
                new MockFilterable(new DateTime(2023, 10, 31)),
                new MockFilterable(new DateTime(2023, 10, 15))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicC", 1);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(3, dataToFilter.Length);
            Assert.Equal(new DateTime(2023, 10, 1), result[0].GetDate());
            Assert.Equal(new DateTime(2023, 10, 31), result[1].GetDate());
            Assert.Equal(new DateTime(2023, 10, 15), result[2].GetDate());
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByDateRange")]
        public void Filterdata_EmptyDataToFilter_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByDateRange
            {
                FromDate = new DateTime(2023, 10, 1),
                ToDate = new DateTime(2023, 10, 31)
            };
            object[] dataToFilter = Array.Empty<object>();

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicD", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByDateRange")]
        public void Filterdata_NoDateRangeSet_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByDateRange(); // No FromDate and ToDate set
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 10, 15)),
                new MockFilterable(new DateTime(2023, 10, 30))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicE", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }
    }
}
