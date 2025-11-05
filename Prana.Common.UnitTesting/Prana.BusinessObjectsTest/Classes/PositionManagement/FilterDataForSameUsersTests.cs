using Prana.BusinessObjects.Classes.PositionManagement;
using Prana.BusinessObjects;
using Xunit;
using System;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PositionManagement
{
    public class FilterDataForSameUsersTests
    {
        private class TestFilterable : IFilterable
        {
            public DateTime GetDate() => DateTime.Now;
            public DateTime GetDateModified() => DateTime.Now;
            public int GetAccountID() => 1;
            public string GetSymbol() => "TEST";
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataForSameUsers")]
        public void Filterdata_WhenUserIdMatches_DoesNotAddAnyData()
        {
            // Arrange
            var filter = new FilterDataForSameUsers { UserId = 1 };
            object[] dataToFilter = { new TestFilterable(), new TestFilterable() };
            int userId = 1; // Same as filter's UserId

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TestTopic", userId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataForSameUsers")]
        public void Filterdata_WhenUserIdDoesNotMatch_AddsAllData()
        {
            // Arrange
            var filter = new FilterDataForSameUsers { UserId = 1 };
            object[] dataToFilter = { new TestFilterable(), new TestFilterable() };
            int userId = 2; // Different from filter's UserId

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TestTopic", userId);

            // Assert
            Assert.Equal(dataToFilter.Length, result.Count);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataForSameUsers")]
        public void Filterdata_EmptyCollection_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataForSameUsers { UserId = 1 };
            object[] dataToFilter = Array.Empty<object>();
            int userId = 2;

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TestTopic", userId);

            // Assert
            Assert.Empty(result);
        }
    }
}
