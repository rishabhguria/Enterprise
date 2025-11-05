using Csla.Validation;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using Prana.Global;
using System;
using System.Collections.Generic;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class PositionMasterTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CheckValidationForAllFields_Should_SetNotExists_When_AUECIDIsLessThanZero()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AUECID = -1,
                AssetID = 1,
                NetPosition = 100,
                SideTagValue = "Buy",
                Side = "Buy",
                ExchangeID = 1,
                UnderlyingID = 1,
                UserID = 1,
                TradingAccountID = 1,
                PositionStartDate = DateTime.Now.ToString(),
                IsSecApproved = true,
                AccountName = "Test Account"
            };

            // Act
            PositionMaster.CustomRules.CheckValidationForAllFields(positionMaster);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.NotExists.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CheckValidationForAllFields_Should_SetUnApproved_When_IsSecApprovedIsFalse()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AUECID = 1,
                AssetID = 1,
                NetPosition = 100,
                SideTagValue = "Buy",
                Side = "Buy",
                ExchangeID = 1,
                UnderlyingID = 1,
                UserID = 1,
                TradingAccountID = 1,
                PositionStartDate = DateTime.Now.ToString(),
                IsSecApproved = false,
                AccountName = "Test Account"
            };

            // Act
            PositionMaster.CustomRules.CheckValidationForAllFields(positionMaster);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.UnApproved.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CheckValidationForAllFields_Should_SetNonPermittedAccounts_When_AccountNameNotInAccountsList()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AUECID = 1,
                AssetID = 1,
                NetPosition = 100,
                SideTagValue = "Buy",
                Side = "Buy",
                ExchangeID = 1,
                UnderlyingID = 1,
                UserID = 1,
                TradingAccountID = 1,
                PositionStartDate = DateTime.Now.ToString(),
                IsSecApproved = true,
                AccountName = "UnknownAccount"
            };

            // Mock AccountsList and TotalAccounts
            var account1 = new Account
            {
                AccountID = 1,
                Name = "TestAccount1"
            };
            var account2 = new Account
            {
                AccountID = 1,
                Name = "TestAccount2"
            };
            PositionMaster.CustomRules.AccountsList.Add(account1);
            PositionMaster.CustomRules.AccountsList.Add(account2);
            PositionMaster.CustomRules.TotalAccounts = 3;

            // Act
            PositionMaster.CustomRules.CheckValidationForAllFields(positionMaster);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.NonPermittedAccounts.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CheckValidationForAllFields_Should_SetNonValidated_When_NAVLockDateFails()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AUECID = 1,
                AssetID = 1,
                NetPosition = 100,
                SideTagValue = "Buy",
                Side = "Buy",
                ExchangeID = 1,
                UnderlyingID = 1,
                UserID = 1,
                TradingAccountID = 1,
                PositionStartDate = DateTime.Now.AddDays(-1).ToString(), // Invalid date (before NAVLockDate)
                IsSecApproved = true,
                AccountName = "TestAccount"
            };

            var account = new Account
            {
                AccountID = 1,
                Name = "TestAccount"
            };
            PositionMaster.CustomRules.AccountsList.Add(account);
            // Mock NAVLockDate to be later than PositionStartDate
            NAVLockDateRule.NAVLockDate = DateTime.Now;

            // Act
            PositionMaster.CustomRules.CheckValidationForAllFields(positionMaster);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.NonValidated.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CheckValidationForAllFields_Should_SetValidated_When_AllConditionsAreValid()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AUECID = 1,
                AssetID = 1,
                NetPosition = 100,
                SideTagValue = "Buy",
                Side = "Buy",
                ExchangeID = 1,
                UnderlyingID = 1,
                UserID = 1,
                TradingAccountID = 1,
                PositionStartDate = DateTime.Now.AddDays(-1).ToString(),
                IsSecApproved = true,
                AccountName = "TestAccount"
            };

            // Mock valid AccountsList and TotalAccounts
            var account = new Account
            {
                AccountID = 1,
                Name = "TestAccount"
            };
            PositionMaster.CustomRules.AccountsList.Clear();
            PositionMaster.CustomRules.TotalAccounts = 0;
            PositionMaster.CustomRules.AccountsList.Add(account);
            PositionMaster.CustomRules.TotalAccounts = 1;

            // Act
            PositionMaster.CustomRules.CheckValidationForAllFields(positionMaster);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.Validated.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CheckValidationForAllFields_Should_SetMissingData_When_SideTagValueIsEmpty()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AUECID = 1,
                AssetID = 1,
                NetPosition = 100,
                SideTagValue = string.Empty, // Missing SideTagValue,
                Side = "Buy",
                ExchangeID = 1,
                UnderlyingID = 1,
                UserID = 1,
                TradingAccountID = 1,
                PositionStartDate = DateTime.Now.AddDays(-1).ToString(),
                IsSecApproved = true,
                AccountName = "TestAccount"
            };

            var account = new Account
            {
                AccountID = 1,
                Name = "TestAccount"
            };
            PositionMaster.CustomRules.AccountsList.Add(account);
            // Act
            PositionMaster.CustomRules.CheckValidationForAllFields(positionMaster);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Contains("Side is missing", positionMaster.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AUECIDCheck_AUECIDIsLessThanOrEqualToZero_ShouldReturnFalseAndSetValidationStatus()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AUECID = 0,
                AssetID = 1,
                NetPosition = 1,
                SideTagValue = "Valid",
                Side = "Valid",
                ExchangeID = 1,
                UnderlyingID = 1,
                UserID = 1,
                TradingAccountID = 1,
                PositionStartDate = "2024-01-01",
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = PositionMaster.CustomRules.AUECIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.NotExists.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("AUECID required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AUECIDCheck_AUECIDIsGreaterThanZero_ShouldReturnTrueAndClearValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AUECID = 123,
                AssetID = 1,
                NetPosition = 1,
                SideTagValue = "Valid",
                Side = "Valid",
                ExchangeID = 1,
                UnderlyingID = 1,
                UserID = 1,
                TradingAccountID = 1,
                PositionStartDate = "2024-01-01",
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = PositionMaster.CustomRules.AUECIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AUECIDCheck_NullTarget_ShouldReturnFalse()
        {
            // Arrange
            object target = null;
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = PositionMaster.CustomRules.AUECIDCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AssetIDCheck_AssetIDIsLessThanOrEqualToZeroAndIsSecApproved_ShouldReturnFalseAndSetValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AssetID = 0,
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("AssetIDValidation");

            // Act
            var result = PositionMaster.CustomRules.AssetIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("AssetID required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AssetIDCheck_AssetIDIsGreaterThanZero_ShouldReturnTrueAndClearValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AssetID = 123
            };
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = PositionMaster.CustomRules.AssetIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description); // No validation error
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AssetIDCheck_AssetIDIsLessThanOrEqualToZeroAndNotSecApproved_ShouldReturnTrue()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AssetID = 0,
                IsSecApproved = false
            };
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = PositionMaster.CustomRules.AssetIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result); // Should return true because IsSecApproved is false
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AssetIDCheck_NullTarget_ShouldReturnFalse()
        {
            // Arrange
            object target = null;
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = PositionMaster.CustomRules.AssetIDCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void NetPositionCheck_NetPositionIsLessThanOrEqualToZeroAndIsSecApproved_ShouldReturnFalseAndSetValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                NetPosition = 0,
                IsSecApproved = true,
            };
            var ruleArgs = new RuleArgs("NetPositionValidation");

            // Act
            var result = PositionMaster.CustomRules.NetPositionCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Invalid Quantity", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void NetPositionCheck_NetPositionIsGreaterThanZero_ShouldReturnTrueAndClearValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                NetPosition = 100,
                IsSecApproved = true,
            };
            var ruleArgs = new RuleArgs("NetPositionValidation");

            // Act
            var result = PositionMaster.CustomRules.NetPositionCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void NetPositionCheck_NetPositionIsLessThanOrEqualToZeroAndNotSecApproved_ShouldReturnTrue()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                NetPosition = 0,
                IsSecApproved = false,
            };
            var ruleArgs = new RuleArgs("NetPositionValidation");

            // Act
            var result = PositionMaster.CustomRules.NetPositionCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void NetPositionCheck_NullTarget_ShouldReturnFalse()
        {
            // Arrange
            object target = null;
            var ruleArgs = new RuleArgs("NetPositionValidation");

            // Act
            var result = PositionMaster.CustomRules.NetPositionCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void SideTagValueCheck_SideTagValueIsNullOrEmptyAndIsSecApproved_ShouldReturnFalseAndSetValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                Symbol = "TestSymbol",
                SideTagValue = null,
                IsSecApproved = true,
            };
            var ruleArgs = new RuleArgs("SideTagValidation");

            // Act
            var result = PositionMaster.CustomRules.SideTagValueCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Side required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void SideTagValueCheck_SideTagValueIsNotNullOrEmpty_ShouldReturnTrueAndClearValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                Symbol = "TestSymbol",
                SideTagValue = "ValidSideTag",
                IsSecApproved = true,
            };
            var ruleArgs = new RuleArgs("SideTagValidation");

            // Act
            var result = PositionMaster.CustomRules.SideTagValueCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void SideTagValueCheck_SymbolIsNull_ShouldReturnTrue()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                Symbol = null, // Symbol is null
                SideTagValue = null, // or string.Empty
                IsSecApproved = true,
            };
            var ruleArgs = new RuleArgs("SideTagValidation");

            // Act
            var result = PositionMaster.CustomRules.SideTagValueCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void SideTagValueCheck_NullTarget_ShouldReturnFalse()
        {
            // Arrange
            object target = null;
            var ruleArgs = new RuleArgs("SideTagValidation");

            // Act
            var result = PositionMaster.CustomRules.SideTagValueCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void SideCheck_SideIsNullOrEmptyAndIsSecApproved_ShouldReturnFalseAndSetValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                Symbol = "TestSymbol",
                Side = null, // or string.Empty
                IsSecApproved = true,
            };
            var ruleArgs = new RuleArgs("SideCheckValidation");

            // Act
            var result = PositionMaster.CustomRules.SideCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Side required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void SideCheck_SideIsNotNullOrEmpty_ShouldReturnTrueAndClearValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                Symbol = "TestSymbol",
                Side = "ValidSide", // Valid Side
                IsSecApproved = true,
            };
            var ruleArgs = new RuleArgs("SideCheckValidation");

            // Act
            var result = PositionMaster.CustomRules.SideCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void SideCheck_SymbolIsNull_ShouldReturnTrue()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                Symbol = null, // Symbol is null
                Side = null, // or string.Empty
                IsSecApproved = true,
            };
            var ruleArgs = new RuleArgs("SideCheckValidation");

            // Act
            var result = PositionMaster.CustomRules.SideCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void SideCheck_NullTarget_ShouldReturnFalse()
        {
            // Arrange
            object target = null;
            var ruleArgs = new RuleArgs("SideCheckValidation");

            // Act
            var result = PositionMaster.CustomRules.SideCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AveragePriceCheck_CostBasisLessThanZeroAndIsSecApproved_ShouldReturnFalseAndSetValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                CostBasis = -1, // Invalid CostBasis
                IsSecApproved = true, // Security is approved
            };
            var ruleArgs = new RuleArgs("AvgPriceValidation");

            // Act
            var result = PositionMaster.CustomRules.AveragePriceCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Average price required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AveragePriceCheck_CostBasisGreaterThanOrEqualToZero_ShouldReturnTrueAndClearValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                CostBasis = 0, // Valid CostBasis
                IsSecApproved = true,
            };
            var ruleArgs = new RuleArgs("AvgPriceValidation");

            // Act
            var result = PositionMaster.CustomRules.AveragePriceCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AveragePriceCheck_NullTarget_ShouldReturnFalse()
        {
            // Arrange
            object target = null;
            var ruleArgs = new RuleArgs("AvgPriceValidation");

            // Act
            var result = PositionMaster.CustomRules.AveragePriceCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AccountCheck_AccountNameIsEmptyAndIsSecApproved_ShouldReturnFalseAndSetValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AccountName = string.Empty, // Invalid AccountName
                IsSecApproved = true, // Security is approved
            };
            var ruleArgs = new RuleArgs("AccountValidation");

            // Act
            var result = PositionMaster.CustomRules.AccountCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Account Name required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AccountCheck_AccountNameIsNotEmptyAndIsSecApproved_ShouldReturnTrueAndClearValidationError()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AccountName = "ValidAccount", // Valid AccountName
                IsSecApproved = true,
            };
            var ruleArgs = new RuleArgs("AccountValidation");

            // Act
            var result = PositionMaster.CustomRules.AccountCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AccountCheck_NullTarget_ShouldReturnFalse()
        {
            // Arrange
            object target = null;
            var ruleArgs = new RuleArgs("AvgPriceValidation");

            // Act
            var result = PositionMaster.CustomRules.AccountCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AccountPermissionCheck_ShouldReturnTrue_WhenAccountNameIsNull()
        {
            // Arrange
            var positionMaster = new PositionMaster { AccountName = null };
            var ruleArgs = new RuleArgs("AccountPermissionValidation");

            // Act
            bool result = PositionMaster.CustomRules.AccountPermissionCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AccountPermissionCheck_ShouldReturnFalse_WhenAccountNameNotInAccountsList()
        {
            // Arrange
            var positionMaster = new PositionMaster { AccountName = "Not Permitted Account" };
            var ruleArgs = new RuleArgs("AccountPermissionValidation");

            // Act
            bool result = PositionMaster.CustomRules.AccountPermissionCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.NonPermittedAccounts.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Account Is not Permitted to the User", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AccountPermissionCheck_ShouldReturnTrue_WhenAccountNameInAccountsList()
        {
            // Arrange
            var positionMaster = new PositionMaster { AccountName = "Account1" };
            var ruleArgs = new RuleArgs("AccountPermissionValidation");
            var account1 = new Account
            {
                AccountID = 1,
                Name = "Account1"
            };
            PositionMaster.CustomRules.AccountsList.Add(account1);

            // Act
            bool result = PositionMaster.CustomRules.AccountPermissionCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            // Ensure no validation errors are set
            Assert.NotEqual(ApplicationConstants.ValidationStatus.NonPermittedAccounts.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void ExchangeCheck_ShouldReturnFalse_WhenTargetIsNotPositionMaster()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("ExchangeValidation");

            // Act
            bool result = PositionMaster.CustomRules.ExchangeCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void ExchangeCheck_ShouldReturnFalse_WhenExchangeIDIsZeroAndIsSecApprovedIsTrue()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                ExchangeID = ApplicationConstants.CONST_ZERO,
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("ExchangeValidation");

            // Act
            bool result = PositionMaster.CustomRules.ExchangeCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Exchange required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void ExchangeCheck_ShouldReturnTrue_WhenExchangeIDIsZeroAndIsSecApprovedIsFalse()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                ExchangeID = ApplicationConstants.CONST_ZERO,
                IsSecApproved = false
            };
            var ruleArgs = new RuleArgs("ExchangeValidation");

            // Act
            bool result = PositionMaster.CustomRules.ExchangeCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void ExchangeCheck_ShouldReturnTrue_WhenExchangeIDIsNotZero()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                ExchangeID = 1,
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("ExchangeValidation");

            // Act
            bool result = PositionMaster.CustomRules.ExchangeCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void UnderLyingCheck_ShouldReturnFalse_WhenTargetIsNotPositionMaster()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("UnderlyingValidation");

            // Act
            bool result = PositionMaster.CustomRules.UnderLyingCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void UnderLyingCheck_ShouldReturnFalse_WhenUnderlyingIDIsZeroAndIsSecApprovedIsTrue()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                UnderlyingID = ApplicationConstants.CONST_ZERO,
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("UnderlyingValidation");

            // Act
            bool result = PositionMaster.CustomRules.UnderLyingCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Underlying required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void UnderLyingCheck_ShouldReturnTrue_WhenUnderlyingIDIsZeroAndIsSecApprovedIsFalse()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                UnderlyingID = ApplicationConstants.CONST_ZERO,
                IsSecApproved = false
            };
            var ruleArgs = new RuleArgs("UnderlyingValidation");

            // Act
            bool result = PositionMaster.CustomRules.UnderLyingCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void UnderLyingCheck_ShouldReturnTrue_WhenUnderlyingIDIsNotZero()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                UnderlyingID = 1,
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("UnderlyingValidation");

            // Act
            bool result = PositionMaster.CustomRules.UnderLyingCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void UserIDCheck_ShouldReturnFalse_WhenTargetIsNotPositionMaster()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("UserIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.UserIDCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void UserIDCheck_ShouldReturnFalse_WhenUserIDIsZeroAndIsSecApprovedIsTrue()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                UserID = ApplicationConstants.CONST_ZERO,
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("UserIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.UserIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("User required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void UserIDCheck_ShouldReturnTrue_WhenUserIDIsZeroAndIsSecApprovedIsFalse()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                UserID = ApplicationConstants.CONST_ZERO,
                IsSecApproved = false
            };
            var ruleArgs = new RuleArgs("UserIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.UserIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void UserIDCheck_ShouldReturnTrue_WhenUserIDIsNotZero()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                UserID = 1,
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("UserIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.UserIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CompanyIDCheck_ShouldReturnFalse_WhenTargetIsNotPositionMaster()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("CompanyIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.CompanyIDCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CompanyIDCheck_ShouldReturnFalse_WhenCompanyIDIsNegativeAndIsSecApprovedIsTrue()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                CompanyID = -1,  // Negative CompanyID
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("CompanyIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.CompanyIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("CompanyID required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CompanyIDCheck_ShouldReturnTrue_WhenCompanyIDIsNegativeAndIsSecApprovedIsFalse()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                CompanyID = -1,  // Negative CompanyID
                IsSecApproved = false
            };
            var ruleArgs = new RuleArgs("CompanyIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.CompanyIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CompanyIDCheck_ShouldReturnTrue_WhenCompanyIDIsNonNegative()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                CompanyID = 1,  // Non-negative CompanyID
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("CompanyIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.CompanyIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void TradingAccountIDCheck_ShouldReturnFalse_WhenTargetIsNotPositionMaster()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("TradingAccoundIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.TradingAccountIDCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void TradingAccountIDCheck_ShouldReturnFalse_WhenTradingAccountIDIsZero()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                TradingAccountID = ApplicationConstants.CONST_ZERO
            };
            var ruleArgs = new RuleArgs("TradingAccoundIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.TradingAccountIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Trading Account ID required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void TradingAccountIDCheck_ShouldReturnTrue_WhenTradingAccountIDIsNonZero()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                TradingAccountID = 1
            };
            var ruleArgs = new RuleArgs("TradingAccoundIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.TradingAccountIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void DateCheck_ShouldReturnFalse_WhenTargetIsNotPositionMaster()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            bool result = PositionMaster.CustomRules.DateCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void DateCheck_ShouldReturnFalse_WhenPositionStartDateIsNullOrEmptyAndIsSecApprovedTrue()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                PositionStartDate = null,
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            bool result = PositionMaster.CustomRules.DateCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Date required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void DateCheck_ShouldReturnFalse_WhenPositionStartDateIsDateTimeMinValAndIsSecApprovedTrue()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                PositionStartDate = DateTimeConstants.DateTimeMinVal,
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            bool result = PositionMaster.CustomRules.DateCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Date required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void DateCheck_ShouldReturnFalse_WhenPositionStartDateIsValidButFailsNAVLockDateCheck()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                PositionStartDate = "2024-01-01",
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("DateValidation");

            // Mock the NAVLockDateRule to simulate a failure
            NAVLockDateRule.NAVLockDate = DateTime.Parse("2024-02-01");  // NAV lock date is after PositionStartDate

            // Act
            bool result = PositionMaster.CustomRules.DateCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Contains("The date you’ve chosen for this action precedes your NAV Lock date", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void DateCheck_ShouldReturnTrue_WhenPositionStartDateIsValidAndPassesNAVLockDateCheck()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                PositionStartDate = "2024-03-01",
                IsSecApproved = true
            };
            var ruleArgs = new RuleArgs("DateValidation");

            // Mock the NAVLockDateRule to simulate a successful validation
            NAVLockDateRule.NAVLockDate = DateTime.Parse("2024-02-01");  // NAV lock date is before PositionStartDate

            // Act
            bool result = PositionMaster.CustomRules.DateCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AccountIDCheck_ShouldReturnFalse_WhenTargetIsNotPositionMaster()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("AccountIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.AccountIDCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AccountIDCheck_ShouldReturnFalse_WhenAccountIDIsLessThanOrEqualToZero()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AccountID = 0  // Invalid AccountID
            };
            var ruleArgs = new RuleArgs("AccountIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.AccountIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Account Name not validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void AccountIDCheck_ShouldReturnTrue_WhenAccountIDIsGreaterThanZero()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                AccountID = 1  // Valid AccountID
            };
            var ruleArgs = new RuleArgs("AccountIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.AccountIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void IsSecurityApprovedCheck_ShouldReturnFalse_WhenTargetIsNotPositionMaster()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("SecurityValidation");

            // Act
            bool result = PositionMaster.CustomRules.IsSecurityApprovedCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void IsSecurityApprovedCheck_ShouldReturnFalse_WhenSecApprovalStatusIsUnapprovedAndAUECIDIsGreaterThanZero()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                SecApprovalStatus = PositionMaster.UNAPPROVED,
                AUECID = 1 // Valid AUECID
            };
            var ruleArgs = new RuleArgs("SecurityValidation");

            // Act
            bool result = PositionMaster.CustomRules.IsSecurityApprovedCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.UnApproved.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Security not Approved", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void IsSecurityApprovedCheck_ShouldReturnTrue_WhenSecApprovalStatusIsNotUnapproved()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                SecApprovalStatus = "APPROVED",
                AUECID = 1 // Valid AUECID
            };
            var ruleArgs = new RuleArgs("SecurityValidation");

            // Act
            bool result = PositionMaster.CustomRules.IsSecurityApprovedCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void IsSecurityApprovedCheck_ShouldReturnTrue_WhenSecApprovalStatusIsUnapprovedAndAUECIDIsLessThanOrEqualToZero()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                SecApprovalStatus = PositionMaster.UNAPPROVED,
                AUECID = 0 // Invalid AUECID
            };
            var ruleArgs = new RuleArgs("SecurityValidation");

            // Act
            bool result = PositionMaster.CustomRules.IsSecurityApprovedCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.UnApproved.ToString(), positionMaster.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CounterPartyIDCheck_ShouldReturnFalse_WhenTargetIsNotPositionMaster()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("CounterPartyIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.CounterPartyIDCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CounterPartyIDCheck_ShouldReturnFalse_WhenCounterPartyIDIsLessThanZero()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                CounterPartyID = -1 // Invalid CounterPartyID
            };
            var ruleArgs = new RuleArgs("CounterPartyIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.CounterPartyIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
            Assert.Equal("Broker required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PositionMaster")]
        public void CounterPartyIDCheck_ShouldReturnTrue_WhenCounterPartyIDIsGreaterThanOrEqualToZero()
        {
            // Arrange
            var positionMaster = new PositionMaster
            {
                CounterPartyID = 0 // Valid CounterPartyID
            };
            var ruleArgs = new RuleArgs("CounterPartyIDValidation");

            // Act
            bool result = PositionMaster.CustomRules.CounterPartyIDCheck(positionMaster, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.NotEqual(ApplicationConstants.ValidationStatus.MissingData.ToString(), positionMaster.ValidationStatus);
        }
    }
}
