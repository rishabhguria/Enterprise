using Prana.BusinessObjects;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PositionManagement
{
    public class FilterDataByToDateTests
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
        [Trait("Prana.BusinessObjects", "FilterDataByToDate")]
        public void Filterdata_DatesBeforeOrEqualToToDate_ReturnsFilteredData()
        {
            // Arrange
            var filter = new FilterDataByToDate
            {
                ToDate = new DateTime(2023, 1, 1)
            };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2022, 12, 31)),
                new MockFilterable(new DateTime(2023, 1, 1)),
                new MockFilterable(new DateTime(2023, 1, 2))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicA", 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(2, dataToFilter.Length);
            Assert.Contains(result, item => item.GetDate() == new DateTime(2022, 12, 31));
            Assert.Contains(result, item => item.GetDate() == new DateTime(2023, 1, 1));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByToDate")]
        public void Filterdata_DatesAfterToDate_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByToDate
            {
                ToDate = new DateTime(2022, 12, 31)
            };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 1, 1)),
                new MockFilterable(new DateTime(2023, 1, 2))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicB", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByToDate")]
        public void Filterdata_ExactToDate_ReturnsMatchingData()
        {
            // Arrange
            var filter = new FilterDataByToDate
            {
                ToDate = new DateTime(2023, 1, 1)
            };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 1, 1)),
                new MockFilterable(new DateTime(2022, 12, 31))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicC", 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(2, dataToFilter.Length);
            Assert.Contains(result, item => item.GetDate() == new DateTime(2023, 1, 1));
            Assert.Contains(result, item => item.GetDate() == new DateTime(2022, 12, 31));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByToDate")]
        public void Filterdata_EmptyDataToFilter_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByToDate
            {
                ToDate = new DateTime(2023, 1, 1)
            };
            object[] dataToFilter = Array.Empty<object>();

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicD", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }
    }
}
