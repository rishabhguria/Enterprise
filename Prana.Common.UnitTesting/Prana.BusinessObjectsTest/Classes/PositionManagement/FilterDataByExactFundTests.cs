using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PositionManagement
{
    public class FilterDataByExactFundTests
    {
        private class MockFilterable : IFilterable
        {
            private int _accountId;
            public MockFilterable(int accountId)
            {
                _accountId = accountId;
            }

            public int GetAccountID() => _accountId;

            DateTime IFilterable.GetDate()
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
        [Trait("Prana.BusinessObjects", "FilterDataByExactAccount")]
        public void Filterdata_MatchingAccountIDs_ReturnsFilteredData()
        {
            // Arrange
            var filter = new FilterDataByExactAccount
            {
                GivenAccountID = new List<int> { 1001, 1002 }
            };
            object[] dataToFilter = {
                new MockFilterable(1001),
                new MockFilterable(1002),
                new MockFilterable(1003)
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicA", 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(2, dataToFilter.Length);
            Assert.Contains(result, item => item.GetAccountID() == 1001);
            Assert.Contains(result, item => item.GetAccountID() == 1002);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactAccount")]
        public void Filterdata_NonMatchingAccountIDs_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByExactAccount
            {
                GivenAccountID = new List<int> { 2001, 2002 }
            };
            object[] dataToFilter = {
                new MockFilterable(1001),
                new MockFilterable(1002),
                new MockFilterable(1003)
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicB", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactAccount")]
        public void Filterdata_EmptyGivenAccountIDList_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByExactAccount
            {
                GivenAccountID = new List<int>()
            };
            object[] dataToFilter = {
                new MockFilterable(1001),
                new MockFilterable(1002)
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicC", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactAccount")]
        public void Filterdata_EmptyDataToFilter_ReturnsEmptyList()
        {
            // Arrange
            var filter = new FilterDataByExactAccount
            {
                GivenAccountID = new List<int> { 1001, 1002 }
            };
            object[] dataToFilter = Array.Empty<object>();

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicD", 1);

            // Assert
            Assert.Empty(result);
            Assert.Empty(dataToFilter);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FilterDataByExactAccount")]
        public void Filterdata_AllAccountsMatch_ReturnsAllData()
        {
            // Arrange
            var filter = new FilterDataByExactAccount
            {
                GivenAccountID = new List<int> { 1001, 1002, 1003 }
            };
            object[] dataToFilter = {
                new MockFilterable(1001),
                new MockFilterable(1002),
                new MockFilterable(1003)
            };

            // Act
            var result = filter.Filterdata(ref dataToFilter, "TopicE", 1);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(3, dataToFilter.Length);
            Assert.Contains(result, item => item.GetAccountID() == 1001);
            Assert.Contains(result, item => item.GetAccountID() == 1002);
            Assert.Contains(result, item => item.GetAccountID() == 1003);
        }
    }
}
