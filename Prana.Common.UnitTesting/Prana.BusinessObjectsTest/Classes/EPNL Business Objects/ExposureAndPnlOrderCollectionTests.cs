using Prana.BusinessObjects;
using System.Collections.Generic;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.EPNL_Business_Objects
{
    public class ExposureAndPnlOrderCollectionTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "ExposureAndPnlOrderCollection")]
        public void RemoveOrders_RemovesMatchingOrdersAndUpdatesState()
        {
            // Arrange
            var collection = new ExposureAndPnlOrderCollection();

            var order1 = new EPnlOrder
            {
                ID = "Order1",
                MasterFundID = 100,
                Level1ID = 200,
                Symbol = "AAPL"
            };

            var order2 = new EPnlOrder
            {
                ID = "Order2",
                MasterFundID = 101,
                Level1ID = 201,
                Symbol = "GOOGL"
            };

            var order3 = new EPnlOrder
            {
                ID = "Order3",
                MasterFundID = 100,
                Level1ID = 200,
                Symbol = "AAPL"
            };

            collection.Add(order1);
            collection.Add(order2);
            collection.Add(order3);

            Dictionary<string, EPnlOrder> removedOrders;

            // Act
            collection.RemoveOrders("AAPL", 100, 1, out removedOrders);

            // Assert
            Assert.DoesNotContain(order1, collection);
            Assert.DoesNotContain(order3, collection);
            Assert.Contains(order2, collection);  
            Assert.Equal(2, removedOrders.Count);
            Assert.True(removedOrders.ContainsKey("Order1"));
            Assert.True(removedOrders.ContainsKey("Order3"));
            Assert.Equal(Global.ApplicationConstants.TaxLotState.Deleted, removedOrders["Order1"].EpnlOrderState);
            Assert.Equal(Global.ApplicationConstants.TaxLotState.Deleted, removedOrders["Order3"].EpnlOrderState);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ExposureAndPnlOrderCollection")]
        public void RemoveOrders_DoesNotRemoveNonMatchingOrders()
        {
            // Arrange
            var collection = new ExposureAndPnlOrderCollection();

            var order1 = new EPnlOrder
            {
                ID = "Order1",
                MasterFundID = 100,
                Level1ID = 200,
                Symbol = "AAPL"
            };

            var order2 = new EPnlOrder
            {
                ID = "Order2",
                MasterFundID = 101,
                Level1ID = 201,
                Symbol = "GOOGL"
            };

            collection.Add(order1);
            collection.Add(order2);

            Dictionary<string, EPnlOrder> removedOrders;

            // Act
            collection.RemoveOrders("MSFT", 100, 1, out removedOrders);

            // Assert
            // Check that no orders were removed
            Assert.Equal(2, collection.Count);
            Assert.Empty(removedOrders);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ExposureAndPnlOrderCollection")]
        public void RemoveOrders_RemovesOrdersBasedOnLevel1IDWhenSplittedTaxlotsCacheBasisIsZero()
        {
            // Arrange
            var collection = new ExposureAndPnlOrderCollection();

            var order1 = new EPnlOrder
            {
                ID = "Order1",
                MasterFundID = 100,
                Level1ID = 200,
                Symbol = "AAPL"
            };

            var order2 = new EPnlOrder
            {
                ID = "Order2",
                MasterFundID = 101,
                Level1ID = 200,
                Symbol = "AAPL"
            };

            var order3 = new EPnlOrder
            {
                ID = "Order3",
                MasterFundID = 100,
                Level1ID = 201,
                Symbol = "GOOGL"
            };

            collection.Add(order1);
            collection.Add(order2);
            collection.Add(order3);

            Dictionary<string, EPnlOrder> removedOrders;

            // Act
            collection.RemoveOrders("AAPL", 200, 0, out removedOrders);

            // Assert
            // Only orders with Level1ID = 200 should be removed
            Assert.DoesNotContain(order1, collection);
            Assert.DoesNotContain(order2, collection);
            Assert.Contains(order3, collection);
            Assert.Equal(2, removedOrders.Count);
            Assert.Contains("Order1", removedOrders.Keys);
            Assert.Contains("Order2", removedOrders.Keys);
        }
    }
}
