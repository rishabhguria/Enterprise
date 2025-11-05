using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.Import
{
    public class StageImportDataTests
    {
        private class FakeAccountCollection : AccountCollection
        {
            private readonly HashSet<int> _accounts;

            public FakeAccountCollection(IEnumerable<int> accounts)
            {
                _accounts = new HashSet<int>(accounts);
            }
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "StageImportData")]
        public void AddToList_AddsOrderSingleToEmptyList()
        {
            // Arrange
            var stageImportData = new StageImportData();
            var order = new OrderSingle { OrderSideTagValue = "Buy", Quantity = 10, Level1ID = 1 };

            // Act
            stageImportData.AddToList(order, new FakeAccountCollection(new List<int> { 1 }));

            // Assert
            var orderList = stageImportData.getOrderSingleList();
            Assert.Single(orderList);
            Assert.Equal(order, orderList[0]);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "StageImportData")]
        public void AddToList_GroupsMatchingOrders()
        {
            // Arrange
            var stageImportData = new StageImportData();
            var accountCollection = new FakeAccountCollection(new List<int> { 1 });
            var order1 = new OrderSingle { OrderSideTagValue = "Buy", Quantity = 10, Level1ID = 1, TransactionTime = DateTime.Now, TransactionSource = TransactionSource.Rebalancer };
            var order2 = new OrderSingle { OrderSideTagValue = "Buy", Quantity = 15, Level1ID = 1, TransactionTime = DateTime.Now, TransactionSource = TransactionSource.Rebalancer };

            stageImportData.AddToList(order1, accountCollection);

            // Act
            stageImportData.AddToList(order2, accountCollection);

            // Assert
            var orderList = stageImportData.getOrderSingleList();
            Assert.Single(orderList);
            Assert.Equal(25, orderList[0].Quantity); // Quantity will be summed because of grouping
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "StageImportData")]
        public void AddToList_DoesNotGroupIfConditionsAreNotMet()
        {
            // Arrange
            var stageImportData = new StageImportData();
            var accountCollection = new FakeAccountCollection(new List<int> { 1 });

            var order1 = new OrderSingle { OrderSideTagValue = "Buy", Quantity = 10, Level1ID = 1, TransactionTime = DateTime.Now };
            var order2 = new OrderSingle { OrderSideTagValue = "Sell", Quantity = 15, Level1ID = 1, TransactionTime = DateTime.Now };

            stageImportData.AddToList(order1, accountCollection);

            // Act
            stageImportData.AddToList(order2, accountCollection);

            // Assert
            var orderList = stageImportData.getOrderSingleList();
            Assert.Equal(2, orderList.Count);
        }
    }
}
