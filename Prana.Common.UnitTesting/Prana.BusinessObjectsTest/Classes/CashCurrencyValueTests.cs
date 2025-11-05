using Csla.Validation;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class CashCurrencyValueTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void CheckValidationOnAllFields_ShouldSetValidatedToInvalid_WhenBaseCurrencyIDIsLessThanOrEqualToZero()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 0,
                LocalCurrencyID = 1,
                AccountID = 1,
                AccountName = "Account",
                Date = "2024-10-24"
            };

            // Act
            CashCurrencyValue.CustomRules.CheckValidationOnAllFileds(cashCurrencyValue);

            // Assert
            Assert.Equal(CashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void CheckValidationOnAllFields_ShouldSetValidatedToInvalid_WhenLocalCurrencyIDIsLessThanOrEqualToZero()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 0,
                AccountID = 1,
                AccountName = "Account",
                Date = "2024-10-24"
            };

            // Act
            CashCurrencyValue.CustomRules.CheckValidationOnAllFileds(cashCurrencyValue);

            // Assert
            Assert.Equal(CashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void CheckValidationOnAllFields_ShouldSetValidatedToInvalid_WhenAccountIDIsLessThanOrEqualToZero()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 1,
                AccountID = 0,
                AccountName = "Account",
                Date = "2024-10-24"
            };

            // Act
            CashCurrencyValue.CustomRules.CheckValidationOnAllFileds(cashCurrencyValue);

            // Assert
            Assert.Equal(CashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void CheckValidationOnAllFields_ShouldSetValidatedToInvalid_WhenAccountNameIsEmpty()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 1,
                AccountID = 1,
                AccountName = string.Empty,
                Date = "2024-10-24"
            };

            // Act
            CashCurrencyValue.CustomRules.CheckValidationOnAllFileds(cashCurrencyValue);

            // Assert
            Assert.Equal(CashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void CheckValidationOnAllFields_ShouldSetValidatedToInvalid_WhenDateIsNullOrEmpty()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 1,
                AccountID = 1,
                AccountName = "Account",
                Date = string.Empty
            };

            // Act
            CashCurrencyValue.CustomRules.CheckValidationOnAllFileds(cashCurrencyValue);

            // Assert
            Assert.Equal(CashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void CheckValidationOnAllFields_ShouldSetValidatedToInvalid_WhenNAVLockDateIsInTheFuture()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 1,
                AccountID = 1,
                AccountName = "Account",
                Date = "2024-10-24"
            };

            // Set NAVLockDate to a date after the provided cashCurrencyValue.Date
            NAVLockDateRule.NAVLockDate = DateTime.Parse("2024-10-25");

            // Act
            CashCurrencyValue.CustomRules.CheckValidationOnAllFileds(cashCurrencyValue);
            NAVLockDateRule.NAVLockDate = null;

            // Assert
            Assert.Equal(CashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void CheckValidationOnAllFields_ShouldSetValidatedToValid_WhenNAVLockDateIsInThePast()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 1,
                AccountID = 1,
                AccountName = "Account",
                Date = "2024-10-24"
            };

            // Set NAVLockDate to a date before the provided cashCurrencyValue.Date
            NAVLockDateRule.NAVLockDate = DateTime.Parse("2024-10-23");

            // Act
            CashCurrencyValue.CustomRules.CheckValidationOnAllFileds(cashCurrencyValue);
            NAVLockDateRule.NAVLockDate = null;

            // Assert
            Assert.Equal(CashCurrencyValue.VALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void BaseCurrencyCheck_ShouldReturnFalse_WhenBaseCurrencyIDIsLessThanOrEqualToZero()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 0
            };
            var ruleArgs = new RuleArgs("Base Currency Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.BaseCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Base Currency Not Validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void BaseCurrencyCheck_ShouldReturnTrue_WhenBaseCurrencyIDIsGreaterThanZero()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1
            };
            var ruleArgs = new RuleArgs("Base Currency Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.BaseCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void LocalCurrencyCheck_ShouldReturnFalse_WhenBaseCurrencyIDIsLessThanOrEqualToZero()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                LocalCurrencyID = 0
            };
            var ruleArgs = new RuleArgs("Local Currency Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.LocalCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Local Currency Not Validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void LocalCurrencyCheck_ShouldReturnTrue_WhenBaseCurrencyIDIsGreaterThanZero()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                LocalCurrencyID = 1
            };
            var ruleArgs = new RuleArgs("Local Currency Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.LocalCurrencyCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void AccountIDCheck_ShouldReturnFalse_WhenBaseCurrencyIDIsLessThanOrEqualToZero()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                AccountID = 0
            };
            var ruleArgs = new RuleArgs("AccountID Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.AccountIDCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Account Name Not Validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void AccountID_ShouldReturnTrue_WhenBaseCurrencyIDIsGreaterThanZero()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                AccountID = 1
            };
            var ruleArgs = new RuleArgs("AccountID Currency Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.AccountIDCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void DateCheck_ShouldReturnFalse_WhenDateIsNullOrEmpty()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 1,
                AccountID = 1,
                AccountName = "Test Account",
                Date = ""
            };
            var ruleArgs = new RuleArgs("Date Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.DateCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Date required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void DateCheck_ShouldReturnFalse_WhenDatePrecedesNAVLockDate()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 1,
                AccountID = 1,
                AccountName = "Test Account",
                Date = "2024-10-24"
            };

            // Set NAVLockDate to a date after the provided cashCurrencyValue.Date
            NAVLockDateRule.NAVLockDate = DateTime.Parse("2024-10-25");
            var ruleArgs = new RuleArgs("Date Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.DateCheck(cashCurrencyValue, ruleArgs);
            NAVLockDateRule.NAVLockDate = null;

            // Assert
            Assert.False(result);
            Assert.Equal("The date you’ve chosen for this action precedes your NAV Lock date (10/25/2024). Please reach out to your Support Team for further assistance.", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void DateCheck_ShouldReturnTrue_WhenDatePassesNAVLockDateCheck()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 1,
                AccountID = 1,
                AccountName = "Test Account",
                Date = "2024-10-24"
            };

            // Set NAVLockDate to a date before the provided cashCurrencyValue.Date
            NAVLockDateRule.NAVLockDate = DateTime.Parse("2024-10-23");
            var ruleArgs = new RuleArgs("Date Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.DateCheck(cashCurrencyValue, ruleArgs);
            NAVLockDateRule.NAVLockDate = null;

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void DateCheck_ShouldReturnFalse_WhenTargetIsNotCashCurrencyValue()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("Date Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.DateCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void AccountCheck_ShouldReturnFalse_WhenAccountNameIsEmpty()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 1,
                AccountID = 1,
                AccountName = "",
                Date = "2024-10-24"
            };
            var ruleArgs = new RuleArgs("Account Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.AccountCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Account Name required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void AccountCheck_ShouldReturnTrue_WhenAccountNameIsValid()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 1,
                AccountID = 1,
                AccountName = "ValidAccount",
                Date = "2024-10-24"
            };
            var ruleArgs = new RuleArgs("Account Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.AccountCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void ForexConversionRateCheck_ShouldReturnFalse_WhenForexConversionRateIsLessThanOrEqualToZero()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 1,
                AccountID = 1,
                AccountName = "Test Account",
                Date = "2024-10-24",
                ForexConversionRate = 0
            };
            var ruleArgs = new RuleArgs("Forex Conversion Rate Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.ForexConversionRateCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Forex Conversion Rate required for the date", ruleArgs.Description);
            Assert.Equal(CashCurrencyValue.INVALID, cashCurrencyValue.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CashCurrencyValue")]
        public void ForexConversionRateCheck_ShouldReturnTrue_WhenForexConversionRateIsGreaterThanZero()
        {
            // Arrange
            var cashCurrencyValue = new CashCurrencyValue
            {
                BaseCurrencyID = 1,
                LocalCurrencyID = 1,
                AccountID = 1,
                AccountName = "Test Account",
                Date = "2024-10-24",
                ForexConversionRate = 1.5
            };
            var ruleArgs = new RuleArgs("Forex Conversion Rate Validation");

            // Act
            bool result = CashCurrencyValue.CustomRules.ForexConversionRateCheck(cashCurrencyValue, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
            Assert.Equal(CashCurrencyValue.VALID, cashCurrencyValue.Validated);
        }
    }
}
