using Csla.Validation;
using Prana.BusinessObjects;
using Prana.Global;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class DividendYieldImportTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "DividendYieldImport")]
        public void CheckValidationOnAllFileds_InvalidAUECID_ShouldSetNotExistsStatus()
        {
            // Arrange
            var finalTarget = new DividendYieldImport
            {
                AUECID = 0,
            };

            // Act
            DividendYieldImport.CustomRules.CheckValidationOnAllFileds(finalTarget);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.NotExists.ToString(), finalTarget.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendYieldImport")]
        public void CheckValidationOnAllFileds_MissingData_ShouldSetMissingDataStatus()
        {
            // Arrange
            var finalTarget = new DividendYieldImport
            {
                AUECID = 12345,
            };

            // Act
            DividendYieldImport.CustomRules.CheckValidationOnAllFileds(finalTarget);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), finalTarget.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendYieldImport")]
        public void CheckValidationOnAllFileds_ValidData_ShouldSetValidatedStatus()
        {
            // Arrange
            var finalTarget = new DividendYieldImport
            {
                AUECID = 12345,
                Symbol = "TestSymbol",
                Date = "2024-10-24"
            };

            // Act
            DividendYieldImport.CustomRules.CheckValidationOnAllFileds(finalTarget);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.Validated.ToString(), finalTarget.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendYieldImport")]
        public void SymbolCheck_EmptySymbol_ShouldReturnFalseAndSetDescription()
        {
            // Arrange
            var finalTarget = new DividendYieldImport
            {
                Symbol = ""
            };
            var ruleArgs = new RuleArgs("Symbol");

            // Act
            var result = DividendYieldImport.CustomRules.SymbolCheck(finalTarget, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Symbol not validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendYieldImport")]
        public void SymbolCheck_ValidSymbol_ShouldReturnTrue()
        {
            // Arrange
            var finalTarget = new DividendYieldImport
            {
                Symbol = "TestSymbol"
            };
            var ruleArgs = new RuleArgs("Symbol");

            // Act
            var result = DividendYieldImport.CustomRules.SymbolCheck(finalTarget, ruleArgs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendYieldImport")]
        public void DateCheck_EmptyDate_ShouldReturnFalseAndSetDescription()
        {
            // Arrange
            var finalTarget = new DividendYieldImport
            {
                Date = ""
            };
            var ruleArgs = new RuleArgs("Date");

            // Act
            var result = DividendYieldImport.CustomRules.DateCheck(finalTarget, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Date required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendYieldImport")]
        public void DateCheck_ValidDate_ShouldReturnTrue()
        {
            // Arrange
            var finalTarget = new DividendYieldImport
            {
                Date = "2024-10-24"
            };
            var ruleArgs = new RuleArgs("Date");

            // Act
            var result = DividendYieldImport.CustomRules.DateCheck(finalTarget, ruleArgs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendYieldImport")]
        public void AUECIDCheck_InvalidAUECID_ShouldReturnFalseAndSetDescription()
        {
            // Arrange
            var finalTarget = new DividendYieldImport
            {
                AUECID = 0
            };
            var ruleArgs = new RuleArgs("AUECID");

            // Act
            var result = DividendYieldImport.CustomRules.AUECIDCheck(finalTarget, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Invalid AUECID", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DividendYieldImport")]
        public void AUECIDCheck_ValidAUECID_ShouldReturnTrue()
        {
            // Arrange
            var finalTarget = new DividendYieldImport
            {
                AUECID = 12345
            };
            var ruleArgs = new RuleArgs("AUECID");

            // Act
            var result = DividendYieldImport.CustomRules.AUECIDCheck(finalTarget, ruleArgs);

            // Assert
            Assert.True(result);
        }
    }
}
