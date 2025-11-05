using Csla.Validation;
using NPOI.SS.Formula.Functions;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class DividendImportTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void CheckValidationOnAllFields_ShouldSetValidatedToINVALID_WhenFieldsAreInvalid()
        {
            // Arrange
            var dividendImport = new DividendImport
            {
                ExDate = null, // Invalid ExDate
                FundID = 0, // Invalid FundID
                ActivityTypeId = 0, // Invalid ActivityTypeId
                CurrencyID = 0 // Invalid CurrencyID
            };

            // Act
            DividendImport.CustomRules.CheckValidationOnAllFileds(dividendImport);

            // Assert
            Assert.Equal(DividendImport.INVALID, dividendImport.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void CheckValidationOnAllFields_ShouldSetValidatedToVALID_WhenFieldsAreValid()
        {
            // Arrange
            var dividendImport = new DividendImport
            {
                ExDate = new DateTime(2024, 1, 1).ToString(),
                FundID = 1,
                ActivityTypeId = 1,
                CurrencyID = 1
            };
            NAVLockDateRule.NAVLockDate = null;

            // Act
            DividendImport.CustomRules.CheckValidationOnAllFileds(dividendImport);

            // Assert
            Assert.Equal(DividendImport.VALID, dividendImport.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void AUECIDCheck_ShouldReturnFalse_WhenAUECIDIsInvalid()
        {
            // Arrange
            var dividendImport = new DividendImport { AUECID = 0 };
            var ruleArgs = new RuleArgs("AUECID");

            // Act
            var result = DividendImport.CustomRules.AUECIDCheck(dividendImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("AUECID required", ruleArgs.Description);
            Assert.Equal(DividendImport.INVALID, dividendImport.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void AUECIDCheck_ShouldReturnTrue_WhenAUECIDIsValid()
        {
            // Arrange
            var dividendImport = new DividendImport { AUECID = 10 };
            var ruleArgs = new RuleArgs("AUECID");

            // Act
            var result = DividendImport.CustomRules.AUECIDCheck(dividendImport, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(DividendImport.VALID, dividendImport.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void FXRateCheck_ShouldReturnFalse_WhenFXRateIsInvalid()
        {
            // Arrange
            var dividendImport = new DividendImport { FXRate = 0 };
            var ruleArgs = new RuleArgs("FXRate");

            // Act
            var result = DividendImport.CustomRules.FXRateCheck(dividendImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("FXRate required", ruleArgs.Description);
            Assert.Equal(DividendImport.INVALID, dividendImport.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void FXRateCheck_ShouldReturnTrue_WhenFXRateIsValid()
        {
            // Arrange
            var dividendImport = new DividendImport { FXRate = 1.5 };
            var ruleArgs = new RuleArgs("FXRate");

            // Act
            var result = DividendImport.CustomRules.FXRateCheck(dividendImport, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(DividendImport.VALID, dividendImport.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void SymbolCheck_ShouldReturnFalse_WhenSymbolIsInvalid()
        {
            // Arrange
            var dividendImport = new DividendImport { Symbol = "" };
            var ruleArgs = new RuleArgs("Symbol");

            // Act
            var result = DividendImport.CustomRules.SymbolCheck(dividendImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Symbol not validated", ruleArgs.Description);
            Assert.Equal(DividendImport.INVALID, dividendImport.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void SymbolCheck_ShouldReturnTrue_WhenSymbolIsValid()
        {
            // Arrange
            var dividendImport = new DividendImport { Symbol = "AAPL" };
            var ruleArgs = new RuleArgs("Symbol");

            // Act
            var result = DividendImport.CustomRules.SymbolCheck(dividendImport, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(DividendImport.VALID, dividendImport.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void ExDateCheck_ShouldReturnFalse_WhenExDateIsInvalid()
        {
            // Arrange
            var dividendImport = new DividendImport { ExDate = "" };
            var ruleArgs = new RuleArgs("ExDate");

            // Act
            var result = DividendImport.CustomRules.ExDateCheck(dividendImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Ex Date required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void ExDateCheck_ShouldReturnTrue_WhenExDateIsValid()
        {
            // Arrange
            var dividendImport = new DividendImport { ExDate = new DateTime(2024, 1, 1).ToString() };
            var ruleArgs = new RuleArgs("ExDate");
            NAVLockDateRule.NAVLockDate = null;

            // Act
            var result = DividendImport.CustomRules.ExDateCheck(dividendImport, ruleArgs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void AccountIDCheck_ShouldReturnFalse_WhenFundIDIsInvalid()
        {
            // Arrange
            var dividendImport = new DividendImport
            {
                FundID = 0,
            };
            var ruleArgs = new RuleArgs("AccountID");

            // Act
            var result = DividendImport.CustomRules.AccountIDCheck(dividendImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Account Name not validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void AccountIDCheck_ShouldReturnTrue_WhenFundIDIsValid()
        {
            // Arrange
            var dividendImport = new DividendImport
            {
                FundID = 1,
            };
            var ruleArgs = new RuleArgs("AccountID");

            // Act
            var result = DividendImport.CustomRules.AccountIDCheck(dividendImport, ruleArgs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void ActivityTypeIDCheck_ShouldReturnFalse_WhenActivityTypeIdIsInvalid()
        {
            // Arrange
            var dividendImport = new DividendImport
            {
                ActivityTypeId = 0
            };
            var ruleArgs = new RuleArgs("ActivityTypeID");

            // Act
            var result = DividendImport.CustomRules.ActivityTypeIDCheck(dividendImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("ActivityType not validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void ActivityTypeIDCheck_ShouldReturnTrue_WhenActivityTypeIdIsValid()
        {
            // Arrange
            var dividendImport = new DividendImport
            {
                ActivityTypeId = 1
            };
            var ruleArgs = new RuleArgs("ActivityTypeID");

            // Act
            var result = DividendImport.CustomRules.ActivityTypeIDCheck(dividendImport, ruleArgs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void CurrencyCheck_ShouldReturnFalse_WhenCurrencyIDIsInvalid()
        {
            // Arrange
            var dividendImport = new DividendImport
            {
                CurrencyID = 0
            };
            var ruleArgs = new RuleArgs("CurrencyID");

            // Act
            var result = DividendImport.CustomRules.CurrencyCheck(dividendImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Currency not validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendImport")]
        public void CurrencyCheck_ShouldReturnTrue_WhenCurrencyIDIsValid()
        {
            // Arrange
            var dividendImport = new DividendImport
            {
                CurrencyID = 1
            };
            var ruleArgs = new RuleArgs("CurrencyID");

            // Act
            var result = DividendImport.CustomRules.CurrencyCheck(dividendImport, ruleArgs);

            // Assert
            Assert.True(result);
        }
    }
}
