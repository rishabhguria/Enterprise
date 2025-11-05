using Csla.Validation;
using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class BetaImportTests
    {

        [Fact]
        [Trait("Prana.BusinessObjects", "BetaImport")]
        public void PropertySetters_ShouldUpdateProperties()
        {
            // Arrange
            var betaImport = new BetaImport();
            string newSymbol = "TestSymbol";

            // Act
            betaImport.Symbol = newSymbol;

            // Assert
            Assert.Equal(newSymbol, betaImport.Symbol);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BetaImport")]
        public void SymbolCheck_ShouldFail_WhenSymbolIsEmpty()
        {
            // Arrange
            var betaImport = new BetaImport
            {
                Date = "2024-01-01",
                AUECID = 1
            };

            var ruleArgs = new RuleArgs("Symbol");

            // Act
            var result = BetaImport.CustomRules.SymbolCheck(betaImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Symbol not validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BetaImport")]
        public void DateCheck_ShouldFail_WhenDateIsEmpty()
        {
            // Arrange
            var betaImport = new BetaImport
            {
                Symbol = "TestSymbol",
                AUECID = 1
            };

            var ruleArgs = new RuleArgs("Date");

            // Act
            var result = BetaImport.CustomRules.DateCheck(betaImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Date required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BetaImport")]
        public void AUECIDCheck_ShouldFail_WhenAUECIDIsZero()
        {
            // Arrange
            var betaImport = new BetaImport
            {
                Symbol = "TestSymbol",
                Date = "2024-01-01",
                AUECID = 0 // Invalid AUECID
            };

            var ruleArgs = new RuleArgs("AUECID");

            // Act
            var result = BetaImport.CustomRules.AUECIDCheck(betaImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Invalid AUECID", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BetaImport")]
        public void Validation_ShouldPass_WhenAllRequiredFieldsAreSet()
        {
            // Arrange
            var betaImport = new BetaImport
            {
                Symbol = "TestSymbol",
                Date = "2024-01-01",
                AUECID = 1
            };

            var ruleArgsSymbol = new RuleArgs("Symbol");
            var ruleArgsDate = new RuleArgs("Date");
            var ruleArgsAUECID = new RuleArgs("AUECID");

            // Act
            var symbolResult = BetaImport.CustomRules.SymbolCheck(betaImport, ruleArgsSymbol);
            var dateResult = BetaImport.CustomRules.DateCheck(betaImport, ruleArgsDate);
            var auecidResult = BetaImport.CustomRules.AUECIDCheck(betaImport, ruleArgsAUECID);

            // Assert
            Assert.True(symbolResult);
            Assert.True(dateResult);
            Assert.True(auecidResult);
        }
    }
}
