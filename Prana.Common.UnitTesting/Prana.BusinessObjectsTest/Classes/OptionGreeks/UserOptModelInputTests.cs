using Csla.Validation;
using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.OptionGreeks
{
    public class UserOptModelInputTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "UserOptModelInput")]
        public void SymbolCheck_ShouldReturnFalse_WhenSymbolIsNullOrEmpty()
        {
            // Arrange
            var input = new UserOptModelInput { Symbol = "", AuecID = 1, Volatility = 0.1, Dividend = 0.05, StockBorrowCost = 0.02, IntRate = 0.01, LastPrice = 100 };
            var ruleArgs = new RuleArgs("SymbolVAlidation");

            // Act
            var result = UserOptModelInput.CustomRules.SymbolCheck(input, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Symbol not validated", ruleArgs.Description);
            Assert.Equal(UserOptModelInput.INVALID, input.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "UserOptModelInput")]
        public void SymbolCheck_ValidSymbol_ShouldReturnTrue()
        {
            // Arrange
            var target = new UserOptModelInput { Symbol = "AAPL", AuecID = 1 };
            var ruleArgs = new RuleArgs("SymbolVAlidation");

            // Act
            var result = UserOptModelInput.CustomRules.SymbolCheck(target, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
            Assert.Equal(UserOptModelInput.VALID, target.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "UserOptModelInput")]
        public void InterestRateCheck_NegativeInterestRate_ShouldSetDescriptionAndReturnFalse()
        {
            // Arrange
            var target = new UserOptModelInput { IntRate = -1, AuecID = 1, Symbol = "AAPL" };
            var ruleArgs = new RuleArgs("SymbolVAlidation");

            // Act
            var result = UserOptModelInput.CustomRules.InterestRateCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Negative Value not allowed", ruleArgs.Description);
            Assert.Equal(UserOptModelInput.INVALID, target.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "UserOptModelInput")]
        public void StockBorrowCostCheck_NegativeStockBorrowCost_ShouldSetDescriptionAndReturnFalse()
        {
            // Arrange
            var target = new UserOptModelInput { StockBorrowCost = -1, AuecID = 1, Symbol = "AAPL" };
            var ruleArgs = new RuleArgs("SymbolVAlidation");

            // Act
            var result = UserOptModelInput.CustomRules.StockBorrowCostCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Negative Value not allowed", ruleArgs.Description);
            Assert.Equal(UserOptModelInput.INVALID, target.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "UserOptModelInput")]
        public void DividendCheck_NegativeDividend_ShouldSetDescriptionAndReturnFalse()
        {
            // Arrange
            var target = new UserOptModelInput { Dividend = -1, AuecID = 1, Symbol = "AAPL" };
            var ruleArgs = new RuleArgs("SymbolVAlidation");

            // Act
            var result = UserOptModelInput.CustomRules.DividendCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Negative Value not allowed", ruleArgs.Description);
            Assert.Equal(UserOptModelInput.INVALID, target.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "UserOptModelInput")]
        public void AUECIDCheck_InvalidAuecID_ShouldSetDescriptionAndReturnFalse()
        {
            // Arrange
            var target = new UserOptModelInput { AuecID = 0, Symbol = "AAPL" };
            var ruleArgs = new RuleArgs("SymbolVAlidation");

            // Act
            var result = UserOptModelInput.CustomRules.AUECIDCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Symbol not validated", ruleArgs.Description);
            Assert.Equal(UserOptModelInput.INVALID, target.Validated);
        }
    }
}
