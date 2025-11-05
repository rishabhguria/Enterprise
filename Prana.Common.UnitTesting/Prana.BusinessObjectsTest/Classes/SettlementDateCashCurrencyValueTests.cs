using Csla.Validation;
using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class SettlementDateCashCurrencyValueTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void BaseCurrencyCheck_ShouldReturnFalse_WhenTargetIsNotSettlementDateCashCurrencyValue()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("BaseCurrencyValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.BaseCurrencyCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void BaseCurrencyCheck_ShouldReturnFalse_WhenSettlementDateBaseCurrencyIDIsInvalid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 0, // Invalid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = "Account1", // Valid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("BaseCurrencyValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.BaseCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Settlement Date Base Currency Not Validated", ruleArgs.Description);
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void BaseCurrencyCheck_ShouldReturnTrueButInvalid_WhenSettlementDateLocalCurrencyIDIsInvalid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 0, // Invalid
                AccountID = 1, // Valid
                AccountName = "Account1", // Valid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("BaseCurrencyValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.BaseCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void BaseCurrencyCheck_ShouldReturnTrueButInvalid_WhenAccountIDIsInvalid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 0, // Invalid
                AccountName = "Account1", // Valid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("BaseCurrencyValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.BaseCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void BaseCurrencyCheck_ShouldReturnTrueButInvalid_WhenAccountNameIsEmpty()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = string.Empty, // Invalid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("BaseCurrencyValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.BaseCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void BaseCurrencyCheck_ShouldReturnTrueButInvalid_WhenSettlementDateIsNullOrEmpty()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = "Account1", // Valid
                SettlementDate = null // Invalid
            };
            var ruleArgs = new RuleArgs("BaseCurrencyValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.BaseCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void BaseCurrencyCheck_ShouldReturnTrue_WhenAllPropertiesAreValid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = "Account1", // Valid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("BaseCurrencyValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.BaseCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SettlementDateCashCurrencyValue.VALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void LocalCurrencyCheck_ShouldReturnFalse_WhenTargetIsNotSettlementDateCashCurrencyValue()
        {
            // Arrange
            var invalidTarget = new object(); // Not a SettlementDateCashCurrencyValue instance
            var ruleArgs = new RuleArgs("LocalCurrencyValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.LocalCurrencyCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void LocalCurrencyCheck_ShouldReturnFalse_WhenSettlementDateLocalCurrencyIDIsInvalid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 0, // Invalid
                AccountID = 1, // Valid
                AccountName = "Account1", // Valid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("LocalCurrencyValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.LocalCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Local Currency Not Validated", ruleArgs.Description);
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void LocalCurrencyCheck_ShouldReturnTrue_WhenAllPropertiesAreValid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = "Account1", // Valid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("LocalCurrencyValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.LocalCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SettlementDateCashCurrencyValue.VALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void LocalCurrencyCheck_ShouldSetValidatedToInvalid_WhenPropertiesAreNotValid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 0, // Invalid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = "Account1", // Valid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("LocalCurrencyValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.LocalCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result); // The method returns true, despite invalid state
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated); // Invalid should be set
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void AccountIDCheck_ShouldReturnFalse_WhenTargetIsNotSettlementDateCashCurrencyValue()
        {
            // Arrange
            var invalidTarget = new object(); // Not a SettlementDateCashCurrencyValue instance
            var ruleArgs = new RuleArgs("AccountIDValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.AccountIDCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void AccountIDCheck_ShouldReturnFalse_WhenAccountIDIsInvalid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 0, // Invalid
                AccountName = "Account1", // Valid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("AccountIDValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.AccountIDCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Account Name Not Validated", ruleArgs.Description);
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void AccountIDCheck_ShouldSetValidatedToInvalid_WhenOtherPropertiesAreNotValid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = string.Empty, // Invalid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("AccountIDValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.AccountIDCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result); // The method returns true, despite invalid state
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated); // Invalid should be set
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void AccountIDCheck_ShouldReturnTrue_WhenAllPropertiesAreValid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = "Account1", // Valid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("AccountIDValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.AccountIDCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SettlementDateCashCurrencyValue.VALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void DateCheck_ShouldReturnFalse_WhenTargetIsNotSettlementDateCashCurrencyValue()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.DateCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void DateCheck_ShouldReturnFalse_WhenSettlementDateIsEmpty()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = "Account1", // Valid
                SettlementDate = string.Empty // Invalid
            };
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.DateCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Settlement Date required", ruleArgs.Description);
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void DateCheck_ShouldSetValidatedToInvalid_WhenOtherPropertiesAreNotValid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = string.Empty, // Invalid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.DateCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result); // The method returns true, despite invalid state
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated); // Invalid should be set
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void DateCheck_ShouldReturnTrue_WhenAllPropertiesAreValid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = "Account1", // Valid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.DateCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SettlementDateCashCurrencyValue.VALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void AccountCheck_ShouldReturnFalse_WhenTargetIsNotSettlementDateCashCurrencyValue()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("AccountValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.AccountCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void AccountCheck_ShouldReturnFalse_WhenAccountNameIsEmpty()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = string.Empty, // Invalid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("AccountValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.AccountCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Account Name required", ruleArgs.Description);
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void AccountCheck_ShouldSetValidatedToInvalid_WhenOtherPropertiesAreNotValid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = -1, // Invalid
                AccountName = "Account1", // Valid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("AccountValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.AccountCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void AccountCheck_ShouldReturnTrue_WhenAllPropertiesAreValid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                SettlementDateBaseCurrencyID = 1, // Valid
                SettlementDateLocalCurrencyID = 1, // Valid
                AccountID = 1, // Valid
                AccountName = "Account1", // Valid
                SettlementDate = "2024-01-01" // Valid
            };
            var ruleArgs = new RuleArgs("AccountValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.AccountCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SettlementDateCashCurrencyValue.VALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void ForexConversionRateCheck_ShouldReturnFalse_WhenTargetIsNotSettlementDateCashCurrencyValue()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("ForexConversionRateValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.ForexConversionRateCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void ForexConversionRateCheck_ShouldReturnFalse_WhenForexConversionRateIsLessThanOrEqualToZero()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                ForexConversionRate = 0 // Invalid
            };
            var ruleArgs = new RuleArgs("ForexConversionRateValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.ForexConversionRateCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Forex Conversion Rate required for the date", ruleArgs.Description);
            Assert.Equal(SettlementDateCashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SettlementDateCashCurrencyValue")]
        public void ForexConversionRateCheck_ShouldReturnTrue_WhenForexConversionRateIsValid()
        {
            // Arrange
            var cashCurrencyValue = new SettlementDateCashCurrencyValue
            {
                ForexConversionRate = (double)1.5m // Valid
            };
            var ruleArgs = new RuleArgs("ForexConversionRateValidation");

            // Act
            bool result = SettlementDateCashCurrencyValue.CustomRules.ForexConversionRateCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SettlementDateCashCurrencyValue.VALID, cashCurrencyValue.Validated);
        }
    }
}
