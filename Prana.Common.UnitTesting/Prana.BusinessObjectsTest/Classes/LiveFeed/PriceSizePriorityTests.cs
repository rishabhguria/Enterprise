using Prana.BusinessObjects.LiveFeed;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.LiveFeed
{
    public class PriceSizePriorityTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "PriceSizePriority")]
        public void CompareOrder_PriceComparison_ShouldReturnCorrectOrder()
        {
            // Arrange
            var priceSizePriority = new PriceSizePriority();
            var mmidX = new MarketMaker { Price = 100, Size = 500, MMID = "A", BidAsk = "ASK" };
            var mmidY = new MarketMaker { Price = 200, Size = 300, MMID = "B", BidAsk = "ASK" };

            // Act
            var result = priceSizePriority.CompareOrder(mmidX, mmidY, 1);

            // Assert
            Assert.True(result < 0, "Expected X to come before Y since X's price is lower.");
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PriceSizePriority")]
        public void CompareOrder_SamePrice_DifferentSize_ShouldReturnCorrectOrder()
        {
            // Arrange
            var priceSizePriority = new PriceSizePriority();
            var mmidX = new MarketMaker { Price = 100, Size = 300, MMID = "A", BidAsk = "BID" };
            var mmidY = new MarketMaker { Price = 100, Size = 500, MMID = "B", BidAsk = "BID" };

            // Act
            var result = priceSizePriority.CompareOrder(mmidX, mmidY, -1);

            // Assert
            Assert.True(result > 0, "Expected Y to come before X since Y's size is larger.");
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PriceSizePriority")]
        public void CompareOrder_SamePriceAndSize_DifferentMMID_ShouldReturnCorrectOrder()
        {
            // Arrange
            var priceSizePriority = new PriceSizePriority();
            var mmidX = new MarketMaker { Price = 100, Size = 500, MMID = "A", BidAsk = "ASK" };
            var mmidY = new MarketMaker { Price = 100, Size = 500, MMID = "B", BidAsk = "ASK" };

            // Act
            var result = priceSizePriority.CompareOrder(mmidX, mmidY, 1);

            // Assert
            Assert.True(result < 0, "Expected X to come before Y since MMID X is alphabetically smaller.");
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PriceSizePriority")]
        public void CompareOrder_SortingOrder_ShouldReturnCorrectOrder()
        {
            // Arrange
            var priceSizePriority = new PriceSizePriority();
            var mmidX = new MarketMaker { Price = 100, Size = 500, MMID = "A", BidAsk = "ASK" };
            var mmidY = new MarketMaker { Price = 200, Size = 300, MMID = "B", BidAsk = "ASK" };

            // Act
            var ascendingResult = priceSizePriority.CompareOrder(mmidX, mmidY, 1);
            var descendingResult = priceSizePriority.CompareOrder(mmidX, mmidY, -1);

            // Assert
            Assert.True(ascendingResult < 0, "Expected X to come before Y in ascending order.");
            Assert.True(descendingResult > 0, "Expected Y to come before X in descending order.");
        }
    }
}
