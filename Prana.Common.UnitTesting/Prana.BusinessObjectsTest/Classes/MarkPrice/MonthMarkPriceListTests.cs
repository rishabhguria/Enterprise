using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.MarkPrice
{
    public class MonthMarkPriceListTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "MonthMarkPriceList")]
        public void GetMarkPriceForSymbolAndMonth_ReturnsCorrectPrice_WhenSymbolAndMonthMatch()
        {
            // Arrange
            var monthMarkPriceList = new MonthMarkPriceList
            {
                new MonthMarkPrice { Symbol = "AAPL", Month = "2024-11", FinalMarkPrice = 150.50 },
                new MonthMarkPrice { Symbol = "AAPL", Month = "2024-12", FinalMarkPrice = 155.75 },
                new MonthMarkPrice { Symbol = "GOOGL", Month = "2024-11", FinalMarkPrice = 2800.00 }
            };

            string symbol = "AAPL";
            string month = "2024-12";
            double expectedPrice = 155.75;

            // Act
            double result = monthMarkPriceList.GetMarkPriceForSymbolAndMonth(symbol, month);

            // Assert
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MonthMarkPriceList")]
        public void GetMarkPriceForSymbolAndMonth_ReturnsZero_WhenNoMatchFound()
        {
            // Arrange
            var monthMarkPriceList = new MonthMarkPriceList
            {
                new MonthMarkPrice { Symbol = "AAPL", Month = "2024-11", FinalMarkPrice = 150.50 },
                new MonthMarkPrice { Symbol = "GOOGL", Month = "2024-12", FinalMarkPrice = 2800.00 }
            };

            string symbol = "MSFT";
            string month = "2024-11";

            // Act
            double result = monthMarkPriceList.GetMarkPriceForSymbolAndMonth(symbol, month);

            // Assert
            Assert.Equal(0.0, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MonthMarkPriceList")]
        public void GetMarkPriceForSymbolAndMonth_ReturnsCorrectPrice_WhenMultipleMatchesExist()
        {
            // Arrange
            var monthMarkPriceList = new MonthMarkPriceList
            {
                new MonthMarkPrice { Symbol = "AAPL", Month = "2024-11", FinalMarkPrice = 150.50 },
                new MonthMarkPrice { Symbol = "AAPL", Month = "2024-11", FinalMarkPrice = 152.75 } 
            };

            string symbol = "AAPL";
            string month = "2024-11";
            double expectedPrice = 150.50; 

            // Act
            double result = monthMarkPriceList.GetMarkPriceForSymbolAndMonth(symbol, month);

            // Assert
            Assert.Equal(expectedPrice, result);
        }
    }
}
