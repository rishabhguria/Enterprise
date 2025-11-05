using Csla.Validation;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class ForexPriceImportTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "ForexPriceImport")]
        public void CheckValidationOnAllFileds_ShouldSetValidatedToInvalid_WhenInvalidData()
        {
            // Arrange
            var forexPriceImport = new ForexPriceImport
            {
                BaseCurrencyID = 0,
                SettlementCurrencyID = 0,
                ForexPrice = 0,
                Date = ""
            };

            // Act
            ForexPriceImport.CustomRules.CheckValidationOnAllFileds(forexPriceImport);

            // Assert
            Assert.Equal(ForexPriceImport.INVALID, forexPriceImport.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ForexPriceImport")]
        public void CheckValidationOnAllFileds_ShouldSetValidatedToValid_WhenValidData()
        {
            // Arrange
            var forexPriceImport = new ForexPriceImport
            {
                BaseCurrencyID = 1,
                SettlementCurrencyID = 2,
                ForexPrice = 100,
                Date = DateTime.Now.ToString()
            };
            NAVLockDateRule.NAVLockDate = null;

            // Act
            ForexPriceImport.CustomRules.CheckValidationOnAllFileds(forexPriceImport);

            // Assert
            Assert.Equal(ForexPriceImport.VALID, forexPriceImport.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ForexPriceImport")]
        public void BaseCurrencyCheck_ShouldReturnFalse_WhenBaseCurrencyIDInvalid()
        {
            // Arrange
            var forexPriceImport = new ForexPriceImport { BaseCurrencyID = 0 };
            var ruleArgs = new RuleArgs("BaseCurrencyCheck");

            // Act
            var result = ForexPriceImport.CustomRules.BaseCurrencyCheck(forexPriceImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("From Currency Not Validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ForexPriceImport")]
        public void BaseCurrencyCheck_ShouldReturnTrue_WhenBaseCurrencyIDValid()
        {
            // Arrange
            var forexPriceImport = new ForexPriceImport { BaseCurrencyID = 1 };
            var ruleArgs = new RuleArgs("BaseCurrencyCheck");

            // Act
            var result = ForexPriceImport.CustomRules.BaseCurrencyCheck(forexPriceImport, ruleArgs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ForexPriceImport")]
        public void SettlementCurrencyCheck_ShouldReturnFalse_WhenSettlementCurrencyIDInvalid()
        {
            // Arrange
            var forexPriceImport = new ForexPriceImport { SettlementCurrencyID = 0 };
            var ruleArgs = new RuleArgs("SettlementCurrencyCheck");

            // Act
            var result = ForexPriceImport.CustomRules.SettlementCurrencyCheck(forexPriceImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("To Currency Not Validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ForexPriceImport")]
        public void SettlementCurrencyCheck_ShouldReturnTrue_WhenSettlementCurrencyIDValid()
        {
            // Arrange
            var forexPriceImport = new ForexPriceImport { SettlementCurrencyID = 1 };
            var ruleArgs = new RuleArgs("SettlementCurrencyCheck");

            // Act
            var result = ForexPriceImport.CustomRules.SettlementCurrencyCheck(forexPriceImport, ruleArgs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ForexPriceImport")]
        public void ForexPriceCheck_ShouldReturnFalse_WhenForexPriceInvalid()
        {
            // Arrange
            var forexPriceImport = new ForexPriceImport { ForexPrice = 0 };
            var ruleArgs = new RuleArgs("ForexPriceCheck");

            // Act
            var result = ForexPriceImport.CustomRules.ForexPriceCheck(forexPriceImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Invalid Forex Price", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ForexPriceImport")]
        public void ForexPriceCheck_ShouldReturnTrue_WhenForexPriceValid()
        {
            // Arrange
            var forexPriceImport = new ForexPriceImport { ForexPrice = 100 };
            var ruleArgs = new RuleArgs("ForexPriceCheck");

            // Act
            var result = ForexPriceImport.CustomRules.ForexPriceCheck(forexPriceImport, ruleArgs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ForexPriceImport")]
        public void DateCheck_ShouldReturnFalse_WhenDateInvalid()
        {
            // Arrange
            var forexPriceImport = new ForexPriceImport { Date = "" };
            var ruleArgs = new RuleArgs("DateCheck");

            // Act
            var result = ForexPriceImport.CustomRules.DateCheck(forexPriceImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Date required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ForexPriceImport")]
        public void DateCheck_ShouldReturnTrue_WhenDateValid()
        {
            // Arrange
            var forexPriceImport = new ForexPriceImport { Date = DateTime.Now.ToString() };
            var ruleArgs = new RuleArgs("DateCheck");

            // Act
            var result = ForexPriceImport.CustomRules.DateCheck(forexPriceImport, ruleArgs);

            // Assert
            Assert.True(result);
        }
    }
}
