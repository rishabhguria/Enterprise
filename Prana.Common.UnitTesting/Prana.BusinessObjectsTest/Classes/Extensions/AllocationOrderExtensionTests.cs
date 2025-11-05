using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.Extensions
{
    public class AllocationOrderExtensionTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationOrderExtension")]
        public void UpdateNonEmptyTradeAttributes_ShouldUpdateNonEmptyAttributes()
        {
            // Arrange
            var tradeAttributes = new TradeAttributes
            {
                TradeAttribute1 = "NewValue1",
                TradeAttribute2 = "NewValue2",
                TradeAttribute3 = "",
                TradeAttribute4 = "NewValue4",
                TradeAttribute5 = "NewValue5",
                TradeAttribute6 = "NewValue6"
            };

            var allocationOrder = new AllocationOrder
            {
                TradeAttribute1 = "OldValue1",
                TradeAttribute2 = "OldValue2",
                TradeAttribute3 = "OldValue3",
                TradeAttribute4 = "OldValue4",
                TradeAttribute5 = "OldValue5",
                TradeAttribute6 = "OldValue6"
            };

            // Act
            allocationOrder.UpdateNonEmptyTradeAttributes(tradeAttributes);

            // Assert
            Assert.Equal("NewValue1", allocationOrder.TradeAttribute1);
            Assert.Equal("NewValue2", allocationOrder.TradeAttribute2);
            Assert.Equal("OldValue3", allocationOrder.TradeAttribute3);
            Assert.Equal("NewValue4", allocationOrder.TradeAttribute4);
            Assert.Equal("NewValue5", allocationOrder.TradeAttribute5);
            Assert.Equal("NewValue6", allocationOrder.TradeAttribute6);
        }
    }
}
