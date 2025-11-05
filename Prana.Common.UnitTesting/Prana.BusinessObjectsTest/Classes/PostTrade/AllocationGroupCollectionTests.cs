using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PostTrade
{
    public class AllocationGroupCollectionTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationGroupCollection")]
        public void SumOfExeQuantity_ReturnsCorrectSum()
        {
            // Arrange
            var allocationGroupCollection = new AllocationGroupCollection
            {
                new AllocationGroup { CumQty = 100.0 },
                new AllocationGroup { CumQty = 200.0 },
                new AllocationGroup { CumQty = 300.0 }
            };

            double expectedSum = 600.0;

            // Act
            double result = allocationGroupCollection.SumOfExeQuantity();

            // Assert
            Assert.Equal(expectedSum, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationGroupCollection")]
        public void SumOfExeQuantity_ReturnsZero_WhenCollectionIsEmpty()
        {
            // Arrange
            var allocationGroupCollection = new AllocationGroupCollection();
            double expectedSum = 0.0;

            // Act
            double result = allocationGroupCollection.SumOfExeQuantity();

            // Assert
            Assert.Equal(expectedSum, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationGroupCollection")]
        public void SumOfTotalQuantity_ReturnsCorrectSum()
        {
            // Arrange
            var allocationGroupCollection = new AllocationGroupCollection
            {
                new AllocationGroup { Quantity = 50.0 },
                new AllocationGroup { Quantity = 150.0 },
                new AllocationGroup { Quantity = 250.0 }
            };

            double expectedSum = 450.0;

            // Act
            double result = allocationGroupCollection.SumOfTotalQuantity();

            // Assert
            Assert.Equal(expectedSum, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationGroupCollection")]
        public void SumOfTotalQuantity_ReturnsZero_WhenCollectionIsEmpty()
        {
            // Arrange
            var allocationGroupCollection = new AllocationGroupCollection();
            double expectedSum = 0.0;

            // Act
            double result = allocationGroupCollection.SumOfTotalQuantity();

            // Assert
            Assert.Equal(expectedSum, result);
        }
    }
}
