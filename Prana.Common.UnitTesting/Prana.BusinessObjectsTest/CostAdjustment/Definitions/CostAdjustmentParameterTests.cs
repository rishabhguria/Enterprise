using Prana.BusinessObjects.CostAdjustment.Definitions;
using System.Collections.Generic;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.CostAdjustment.Definitions
{
    public class CostAdjustmentParameterTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "CostAdjustmentParameter")]
        public void IsValid_ShouldReturnFalse_WhenAdjustCostIsZero()
        {
            // Arrange
            var parameter = new CostAdjustmentParameter
            {
                AdjustCost = 0,
                AdjustQty = 10,
                TotalQuantity = 20,
                TotalCost = 50,
                Taxlots = new List<CostAdjustmentTaxlot> { new CostAdjustmentTaxlot { Symbol = "AAPL" } }
            };

            // Act
            var result = parameter.IsValid(out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal("Adjust Cost cannot be 0.", errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CostAdjustmentParameter")]
        public void IsValid_ShouldReturnFalse_WhenTotalCostPlusAdjustCostIsLessThanZero()
        {
            // Arrange
            var parameter = new CostAdjustmentParameter
            {
                AdjustCost = -60,
                TotalCost = 50,
                AdjustQty = 10,
                TotalQuantity = 20,
                Taxlots = new List<CostAdjustmentTaxlot> { new CostAdjustmentTaxlot { Symbol = "AAPL" } }
            };

            // Act
            var result = parameter.IsValid(out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal("Adjust Cost cannot make total cost less than zero.", errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CostAdjustmentParameter")]
        public void IsValid_ShouldReturnFalse_WhenAdjustQtyIsZero()
        {
            // Arrange
            var parameter = new CostAdjustmentParameter
            {
                AdjustCost = 10,
                AdjustQty = 0,
                TotalQuantity = 20,
                TotalCost = 50,
                Taxlots = new List<CostAdjustmentTaxlot> { new CostAdjustmentTaxlot { Symbol = "AAPL" } }
            };

            // Act
            var result = parameter.IsValid(out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal("Adjust Quantity must be greater than 0.", errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CostAdjustmentParameter")]
        public void IsValid_ShouldReturnFalse_WhenAdjustQtyIsGreaterThanTotalQuantity()
        {
            // Arrange
            var parameter = new CostAdjustmentParameter
            {
                AdjustCost = 10,
                AdjustQty = 30,
                TotalQuantity = 20,
                TotalCost = 50,
                Taxlots = new List<CostAdjustmentTaxlot> { new CostAdjustmentTaxlot { Symbol = "AAPL" } }
            };

            // Act
            var result = parameter.IsValid(out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal("Adjust Quantity should be less than or equal to Total Quantity.", errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CostAdjustmentParameter")]
        public void IsValid_ShouldReturnFalse_WhenNoTaxlotsSelected()
        {
            // Arrange
            var parameter = new CostAdjustmentParameter
            {
                AdjustCost = 10,
                AdjustQty = 5,
                TotalQuantity = 20,
                TotalCost = 50,
                Taxlots = new List<CostAdjustmentTaxlot>()
            };

            // Act
            var result = parameter.IsValid(out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal("Please select a taxlot for cost adjustment preview/save.", errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CostAdjustmentParameter")]
        public void IsValid_ShouldReturnFalse_WhenTaxlotsHaveDifferentSymbols()
        {
            // Arrange
            var parameter = new CostAdjustmentParameter
            {
                AdjustCost = 10,
                AdjustQty = 5,
                TotalQuantity = 20,
                TotalCost = 50,
                Taxlots = new List<CostAdjustmentTaxlot>
                {
                    new CostAdjustmentTaxlot { Symbol = "AAPL" },
                    new CostAdjustmentTaxlot { Symbol = "MSFT" }
                }
            };

            // Act
            var result = parameter.IsValid(out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal("Please select taxlots with same Symbols.", errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CostAdjustmentParameter")]
        public void IsValid_ShouldReturnTrue_WhenAllConditionsAreMet()
        {
            // Arrange
            var parameter = new CostAdjustmentParameter
            {
                AdjustCost = 10,
                AdjustQty = 5,
                TotalQuantity = 20,
                TotalCost = 50,
                Taxlots = new List<CostAdjustmentTaxlot>
                {
                    new CostAdjustmentTaxlot { Symbol = "AAPL" }
                }
            };

            // Act
            var result = parameter.IsValid(out string errorMessage);

            // Assert
            Assert.True(result);
            Assert.Equal(string.Empty, errorMessage);
        }
    }
}
