using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PositionManagement
{
    public class FilterDataByAUECDateWiseTests
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
        [Trait("Prana.BusinessObjects", "FilterDataByAUECDateWise")]
        public void Filterdata_MatchingDates_ReturnsFilteredData()
        {
            // Arrange
            var filter = new FilterDataByAUECDateWise
            {
                ListOfAUECDates = new Dictionary<int, DateTime>
                {
                    { 1, new DateTime(2023, 10, 29) },
                    { 2, new DateTime(2023, 10, 30) }
                }
            };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 10, 29)),
                new MockFilterable(new DateTime(2023, 10, 30)),
                new MockFilterable(new DateTime(2023, 10, 31))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicA", 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(new DateTime(2023, 10, 29), result[0].GetDate());
            Assert.Equal(new DateTime(2023, 10, 30), result[1].GetDate());
            Assert.Equal(2, dataToFilter.Length);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByAUECDateWise")]
        public void Filterdata_NoMatchingDates_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByAUECDateWise
            {
                ListOfAUECDates = new Dictionary<int, DateTime>
                {
                    { 1, new DateTime(2023, 10, 29) },
                    { 2, new DateTime(2023, 10, 30) }
                }
            };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 10, 31)),
                new MockFilterable(new DateTime(2023, 11, 01))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicB", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByAUECDateWise")]
        public void Filterdata_EmptyListOfAUECDates_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByAUECDateWise
            {
                ListOfAUECDates = new Dictionary<int, DateTime>()
            };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 10, 29)),
                new MockFilterable(new DateTime(2023, 10, 30))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicC", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByAUECDateWise")]
        public void Filterdata_MultipleMatchingDates_ReturnsAllMatchingData()
        {
            // Arrange
            var filter = new FilterDataByAUECDateWise
            {
                ListOfAUECDates = new Dictionary<int, DateTime>
                {
                    { 1, new DateTime(2023, 10, 29) },
                    { 2, new DateTime(2023, 10, 29) },
                    { 3, new DateTime(2023, 10, 30) }
                }
            };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 10, 29)),
                new MockFilterable(new DateTime(2023, 10, 29)),
                new MockFilterable(new DateTime(2023, 10, 30)),
                new MockFilterable(new DateTime(2023, 10, 31))
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicD", 1);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(3, dataToFilter.Length);
            Assert.All(result, item => Assert.Contains(item.GetDate(), filter.ListOfAUECDates.Values));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByAUECDateWise")]
        public void Filterdata_EmptyDataToFilter_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByAUECDateWise
            {
                ListOfAUECDates = new Dictionary<int, DateTime>
                {
                    { 1, new DateTime(2023, 10, 29) }
                }
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
