using System;
using Xunit;
using Prana.BusinessObjects;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.TTPrefs
{
    public class OrderDetailsTests
    {    
        [Fact]
        [Trait("Prana.BusinessObjects", "OrderDetails")]
        public void CheckForDuplicateOrder_ReturnsTrue_WhenDuplicate()
        {
            // Arrange
            var order1 = new OrderDetails { Symbol = "AAPL", Quantity = 100, OrderSideTagValue = "Buy", CounterPartyID = 1, MasterFund = "FundA", TradeTime = DateTime.Now };
            var order2 = new OrderDetails { Symbol = "AAPL", Quantity = 100, OrderSideTagValue = "Buy", CounterPartyID = 1, MasterFund = "FundA", TradeTime = DateTime.Now.AddSeconds(-5) };

            // Act
            var isDuplicate = order1.CheckForDuplicateOrder(order2, 10);

            // Assert
            Assert.True(isDuplicate);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "OrderDetails")]
        public void CheckForDuplicateOrder_ReturnsFalse_WhenDifferentOrder()
        {
            // Arrange
            var order1 = new OrderDetails { Symbol = "AAPL", Quantity = 100, OrderSideTagValue = "Buy", CounterPartyID = 1, MasterFund = "FundA", TradeTime = DateTime.Now };
            var order2 = new OrderDetails { Symbol = "GOOG", Quantity = 50, OrderSideTagValue = "Sell", CounterPartyID = 2, MasterFund = "FundB", TradeTime = DateTime.Now.AddSeconds(-5) };

            // Act
            var isDuplicate = order1.CheckForDuplicateOrder(order2, 10);

            // Assert
            Assert.False(isDuplicate);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "OrderDetails")]
        public void GetComplianceRejectOrder_ReturnsTrue_WhenSameOrder()
        {
            // Arrange
            var order1 = new OrderDetails { Symbol = "AAPL", Quantity = 100, OrderSideTagValue = "Buy", CounterPartyID = 1, MasterFund = "FundA", TradeTime = DateTime.Now, TransactionTime = DateTime.Now.ToString(DateTimeConstants.NirvanaDateTimeFormat) };
            var order2 = new OrderDetails { Symbol = "AAPL", Quantity = 100, OrderSideTagValue = "Buy", CounterPartyID = 1, MasterFund = "FundA", TradeTime = DateTime.Now.AddSeconds(-5), TransactionTime = DateTime.Now.ToString(DateTimeConstants.NirvanaDateTimeFormat) };

            // Act
            var isSameOrder = order1.GetComplianceRejectOrder(order2);

            // Assert
            Assert.True(isSameOrder);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "OrderDetails")]
        public void GetComplianceRejectOrder_ReturnsFalse_WhenDifferentTransactionTime()
        {
            // Arrange
            var order1 = new OrderDetails { Symbol = "AAPL", Quantity = 100, OrderSideTagValue = "Buy", CounterPartyID = 1, MasterFund = "FundA", TradeTime = DateTime.Now, TransactionTime = DateTime.Now.ToString(DateTimeConstants.NirvanaDateTimeFormat) };
            var order2 = new OrderDetails { Symbol = "AAPL", Quantity = 100, OrderSideTagValue = "Buy", CounterPartyID = 1, MasterFund = "FundA", TradeTime = DateTime.Now.AddSeconds(-5), TransactionTime = DateTime.Now.AddSeconds(-30).ToString(DateTimeConstants.NirvanaDateTimeFormat) };

            // Act
            var isSameOrder = order1.GetComplianceRejectOrder(order2);

            // Assert
            Assert.False(isSameOrder);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "OrderDetails")]
        public void GetUniqueKey_GeneratesCorrectKey()
        {
            // Arrange
            var tradeTime = DateTime.Now;
            var order = new OrderDetails { TradeTime = tradeTime, Symbol = "AAPL", Quantity = 100, OrderSideTagValue = "Buy", CounterPartyID = 1 };

            // Act
            var uniqueKey = order.GetUniqueKey();

            // Assert
            var expectedKey = tradeTime.ToString("mmddyyyy_hh:mm:ss") + "AAPL" + "100" + "Buy" + "1";
            Assert.False(string.IsNullOrEmpty(uniqueKey));
            Assert.Equal(expectedKey, uniqueKey);
        }
    }
}