using Csla.Validation;
using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class DailyCreditLimitTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "DailyCreditLimit")]
        public void AccountCheck_ShouldReturnFalse_WhenAccountNameIsEmpty()
        {
            // Arrange
            var dailyCreditLimit = new DailyCreditLimit
            {
                AccountID = 1,
                AccountName = ""  // Empty AccountName
            };
            var ruleArgs = new RuleArgs("Account Validation");

            // Act
            bool result = DailyCreditLimit.CustomRules.AccountCheck(dailyCreditLimit, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Account Name required", ruleArgs.Description);
            Assert.Equal(DailyCreditLimit.INVALID, dailyCreditLimit.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DailyCreditLimit")]
        public void AccountCheck_ShouldReturnTrueButInvalid_WhenAccountIDIsZero()
        {
            // Arrange
            var dailyCreditLimit = new DailyCreditLimit
            {
                AccountID = 0,
                AccountName = "ValidAccount"
            };
            var ruleArgs = new RuleArgs("Account Validation");

            // Act
            bool result = DailyCreditLimit.CustomRules.AccountCheck(dailyCreditLimit, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
            Assert.Equal(DailyCreditLimit.INVALID, dailyCreditLimit.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DailyCreditLimit")]
        public void AccountCheck_ShouldReturnTrue_WhenAccountIsValid()
        {
            // Arrange
            var dailyCreditLimit = new DailyCreditLimit
            {
                AccountID = 1,
                AccountName = "ValidAccount"
            };
            var ruleArgs = new RuleArgs("Account Validation");

            // Act
            bool result = DailyCreditLimit.CustomRules.AccountCheck(dailyCreditLimit, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
            Assert.Equal(DailyCreditLimit.VALID, dailyCreditLimit.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DailyCreditLimit")]
        public void AccountCheck_ShouldReturnFalse_WhenTargetIsNotDailyCreditLimit()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("Account Validation");

            // Act
            bool result = DailyCreditLimit.CustomRules.AccountCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DailyCreditLimit")]
        public void AccountIDCheck_ShouldReturnFalse_WhenAccountIDIsZero()
        {
            // Arrange
            var dailyCreditLimit = new DailyCreditLimit
            {
                AccountID = 0,
                AccountName = "ValidAccount"
            };
            var ruleArgs = new RuleArgs("Account Validation");

            // Act
            bool result = DailyCreditLimit.CustomRules.AccountIDCheck(dailyCreditLimit, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Account Name Not Validated", ruleArgs.Description);
            Assert.Equal(DailyCreditLimit.INVALID, dailyCreditLimit.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DailyCreditLimit")]
        public void AccountIDCheck_ShouldReturnFalse_WhenAccountIDIsNegative()
        {
            // Arrange
            var dailyCreditLimit = new DailyCreditLimit
            {
                AccountID = -1,
                AccountName = "ValidAccount"
            };
            var ruleArgs = new RuleArgs("Account Validation");

            // Act
            bool result = DailyCreditLimit.CustomRules.AccountIDCheck(dailyCreditLimit, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Account Name Not Validated", ruleArgs.Description);
            Assert.Equal(DailyCreditLimit.INVALID, dailyCreditLimit.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DailyCreditLimit")]
        public void AccountIDCheck_ShouldReturnTrueButInvalid_WhenAccountNameIsEmpty()
        {
            // Arrange
            var dailyCreditLimit = new DailyCreditLimit
            {
                AccountID = 1,
                AccountName = ""
            };
            var ruleArgs = new RuleArgs("Account Validation");

            // Act
            bool result = DailyCreditLimit.CustomRules.AccountIDCheck(dailyCreditLimit, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(DailyCreditLimit.INVALID, dailyCreditLimit.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DailyCreditLimit")]
        public void AccountIDCheck_ShouldReturnTrue_WhenAccountIsValid()
        {
            // Arrange
            var dailyCreditLimit = new DailyCreditLimit
            {
                AccountID = 1,
                AccountName = "ValidAccount"
            };
            var ruleArgs = new RuleArgs("Account Validation");

            // Act
            bool result = DailyCreditLimit.CustomRules.AccountIDCheck(dailyCreditLimit, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
            Assert.Equal(DailyCreditLimit.VALID, dailyCreditLimit.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DailyCreditLimit")]
        public void AccountIDCheck_ShouldReturnFalse_WhenTargetIsNotDailyCreditLimit()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("Account Validation");

            // Act
            bool result = DailyCreditLimit.CustomRules.AccountIDCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Null(ruleArgs.Description);
        }
    }
}
