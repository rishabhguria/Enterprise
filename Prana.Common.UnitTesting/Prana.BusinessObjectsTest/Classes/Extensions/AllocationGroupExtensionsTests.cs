using Prana.BusinessObjects;
using System.Collections.Generic;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.Extensions
{
    public class AllocationGroupExtensionsTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationGroupExtensions")]
        public void UpdateNonEmptyTradeAttributes_ShouldUpdateAttributesAndAddTradeActions()
        {
            // Arrange
            var tradeAttributes = new TradeAttributes
            {
                TradeAttribute1 = "NewValue1",
                TradeAttribute2 = "NewValue2",
                TradeAttribute3 = ""
            };

            var allocationGroup = new AllocationGroup
            {
                TradeAttribute1 = "OldValue1",
                TradeAttribute2 = "OldValue2",
                TradeAttribute3 = "OldValue3",
                TradeActionsList = new List<TradeAuditActionType.ActionType>()
            };

            // Act
            allocationGroup.UpdateNonEmptyTradeAttributes(tradeAttributes);

            // Assert
            Assert.Equal("NewValue1", allocationGroup.TradeAttribute1);
            Assert.Equal("NewValue2", allocationGroup.TradeAttribute2);
            Assert.Equal("OldValue3", allocationGroup.TradeAttribute3); 
            Assert.Contains(TradeAuditActionType.ActionType.TradeAttribute1_Changed, allocationGroup.TradeActionsList);
            Assert.Contains(TradeAuditActionType.ActionType.TradeAttribute2_Changed, allocationGroup.TradeActionsList);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationGroupExtensions")]
        public void UpdateGroupAccruedInterest_ShouldUpdateAccruedInterestCorrectly()
        {
            // Arrange
            var taxLot1 = new TaxLot { AccruedInterest = 100.5 };
            var taxLot2 = new TaxLot { AccruedInterest = 200.75 };
            var allocationGroup = new AllocationGroup
            {
                TaxLots = new List<TaxLot> { taxLot1, taxLot2 }
            };

            // Act
            allocationGroup.UpdateGroupAccruedInterest();

            // Assert
            Assert.Equal(301.25, allocationGroup.AccruedInterest);
        }
    }
}
