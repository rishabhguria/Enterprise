using Csla.Validation;
using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class SecMasterUpdateDataByImportUITests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterUpdateDataByImportUI")]
        public void SymbolCheck_ShouldReturnFalse_WhenTargetIsNotSecMasterUpdateDataByImportUI()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("SymbolValidation");

            // Act
            bool result = SecMasterUpdateDataByImportUI.CustomRules.SymbolCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterUpdateDataByImportUI")]
        public void SymbolCheck_ShouldReturnFalse_WhenTickerSymbolIsNullOrEmpty()
        {
            // Arrange
            var secMasterData = new SecMasterUpdateDataByImportUI
            {
                TickerSymbol = string.Empty, // Invalid TickerSymbol
                AUECID = 1 // Valid AUECID
            };
            var ruleArgs = new RuleArgs("SymbolValidation");

            // Act
            bool result = SecMasterUpdateDataByImportUI.CustomRules.SymbolCheck(secMasterData, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Symbol not found in SM", ruleArgs.Description);
            Assert.Equal(SecMasterUpdateDataByImportUI.INVALID, secMasterData.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterUpdateDataByImportUI")]
        public void SymbolCheck_ShouldReturnTrueButInvalid_WhenAUECIDIsLessThanOrEqualToZeroOrIntMinValue()
        {
            // Arrange
            var secMasterData = new SecMasterUpdateDataByImportUI
            {
                TickerSymbol = "AAPL",
                AUECID = 0
            };
            var ruleArgs = new RuleArgs("SymbolValidation");

            // Act
            bool result = SecMasterUpdateDataByImportUI.CustomRules.SymbolCheck(secMasterData, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SecMasterUpdateDataByImportUI.INVALID, secMasterData.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterUpdateDataByImportUI")]
        public void SymbolCheck_ShouldReturnTrue_WhenTickerSymbolAndAUECIDAreValid()
        {
            // Arrange
            var secMasterData = new SecMasterUpdateDataByImportUI
            {
                TickerSymbol = "AAPL",
                AUECID = 1
            };
            var ruleArgs = new RuleArgs("SymbolValidation");

            // Act
            bool result = SecMasterUpdateDataByImportUI.CustomRules.SymbolCheck(secMasterData, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SecMasterUpdateDataByImportUI.VALID, secMasterData.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterUpdateDataByImportUI")]
        public void AUECIDCheck_ShouldReturnFalse_WhenTargetIsNotSecMasterUpdateDataByImportUI()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            bool result = SecMasterUpdateDataByImportUI.CustomRules.AUECIDCheck(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterUpdateDataByImportUI")]
        public void AUECIDCheck_ShouldReturnFalse_WhenTickerSymbolIsNullOrEmptyOrAUECIDIsInvalid()
        {
            // Arrange
            var secMasterData = new SecMasterUpdateDataByImportUI
            {
                TickerSymbol = string.Empty,
                AUECID = 0
            };
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            bool result = SecMasterUpdateDataByImportUI.CustomRules.AUECIDCheck(secMasterData, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Symbol not found in SM", ruleArgs.Description);
            Assert.Equal(SecMasterUpdateDataByImportUI.INVALID, secMasterData.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterUpdateDataByImportUI")]
        public void AUECIDCheck_ShouldReturnFalse_WhenValidationErrorIsNotEmpty()
        {
            // Arrange
            var secMasterData = new SecMasterUpdateDataByImportUI
            {
                TickerSymbol = "AAPL",
                AUECID = 1,
                ValidationError = "Some validation error"
            };
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            bool result = SecMasterUpdateDataByImportUI.CustomRules.AUECIDCheck(secMasterData, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Some validation error", ruleArgs.Description);
            Assert.Equal(SecMasterUpdateDataByImportUI.INVALID, secMasterData.Validated);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterUpdateDataByImportUI")]
        public void AUECIDCheck_ShouldReturnTrue_WhenTickerSymbolAndAUECIDAreValidAndNoValidationError()
        {
            // Arrange
            var secMasterData = new SecMasterUpdateDataByImportUI
            {
                TickerSymbol = "AAPL",
                AUECID = 1,
                ValidationError = string.Empty
            };
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            bool result = SecMasterUpdateDataByImportUI.CustomRules.AUECIDCheck(secMasterData, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Equal(SecMasterUpdateDataByImportUI.VALID, secMasterData.Validated);
        }
    }
}
