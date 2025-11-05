using Castle.Core.Logging;
using Moq;
using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PositionManagement
{
    public class FilterDataForLastDateModifiedTests
    {
        private class MockFilterable : IFilterable, IKeyable
        {
            private DateTime _dateModified;
            private string _key;

            public MockFilterable(DateTime dateModified, string key)
            {
                _dateModified = dateModified;
                _key = key;
            }

            public DateTime GetDateModified() => _dateModified;

            public string GetKey() => _key;

            int IFilterable.GetAccountID()
            {
                throw new NotImplementedException();
            }

            DateTime IFilterable.GetDate()
            {
                throw new NotImplementedException();
            }

            string IFilterable.GetSymbol()
            {
                throw new NotImplementedException();
            }

            void IKeyable.Update(IKeyable item)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataForLastDateModified")]
        public void Filterdata_ValidDatesAndUniqueKeys_ReturnsFilteredData()
        {
            // Arrange
            var filter = new FilterDataForLastDateModified { TillDate = new DateTime(2023, 1, 1) };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2022, 12, 31), "A"),
                new MockFilterable(new DateTime(2023, 1, 1), "B"),
                new MockFilterable(new DateTime(2023, 1, 2), "C")
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicA", 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, item => item.GetDateModified() == new DateTime(2022, 12, 31) && ((IKeyable)item).GetKey() == "A");
            Assert.Contains(result, item => item.GetDateModified() == new DateTime(2023, 1, 1) && ((IKeyable)item).GetKey() == "B");
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataForLastDateModified")]
        public void Filterdata_DuplicateKeysWithDifferentDates_ReturnsMostRecentDateForEachKey()
        {
            // Arrange
            var filter = new FilterDataForLastDateModified { TillDate = new DateTime(2023, 1, 1) };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2022, 12, 31), "A"),
                new MockFilterable(new DateTime(2023, 1, 1), "A"),  // More recent date for key "A"
                new MockFilterable(new DateTime(2022, 12, 30), "B")
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicB", 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, item => item.GetDateModified() == new DateTime(2023, 1, 1) && ((IKeyable)item).GetKey() == "A");
            Assert.Contains(result, item => item.GetDateModified() == new DateTime(2022, 12, 30) && ((IKeyable)item).GetKey() == "B");
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataForLastDateModified")]
        public void Filterdata_NoMatchingDates_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataForLastDateModified { TillDate = new DateTime(2022, 12, 31) };
            object[] dataToFilter = {
                new MockFilterable(new DateTime(2023, 1, 1), "A"),
                new MockFilterable(new DateTime(2023, 1, 2), "B")
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicC", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataForLastDateModified")]
        public void Filterdata_EmptyDataToFilter_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataForLastDateModified();
            filter.TillDate = DateTime.Today;
            object[] dataToFilter = Array.Empty<object>();

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TestTopic", 123);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }
    }
}
