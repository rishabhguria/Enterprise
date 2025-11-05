using Csla.Validation;
using Prana.BusinessObjects;
using Prana.Global;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class CollateralImportTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "CollateralImport")]
        public void CheckValidationOnAllFields_Should_SetValidationStatus_To_NotExists_When_AUECID_Is_Less_Than_Or_Equal_To_Zero()
        {
            // Arrange
            var collateralImport = new CollateralImport
            {
                Symbol = "ABC",
                Date = "2024-10-24",
                AUECID = 0
            };

            // Act
            CollateralImport.CustomRules.CheckValidationOnAllFileds(collateralImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.NotExists.ToString(), collateralImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CollateralImport")]
        public void CheckValidationOnAllFields_Should_SetValidationStatus_To_MissingData_When_Symbol_Or_Date_Is_Null_Or_Empty()
        {
            // Arrange
            var collateralImport = new CollateralImport
            {
                Symbol = "",
                Date = "2024-10-24",
                AUECID = 100
            };

            // Act
            CollateralImport.CustomRules.CheckValidationOnAllFileds(collateralImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), collateralImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CollateralImport")]
        public void CheckValidationOnAllFields_Should_SetValidationStatus_To_Validated_When_All_Fields_Are_Valid()
        {
            // Arrange
            var collateralImport = new CollateralImport
            {
                Symbol = "ABC",
                Date = "2024-10-24",
                AUECID = 100
            };

            // Act
            CollateralImport.CustomRules.CheckValidationOnAllFileds(collateralImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.Validated.ToString(), collateralImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CollateralImport")]
        public void SymbolCheck_WithNullCollateralImport_ReturnsFalse()
        {
            // Arrange
            object target = null;
            var ruleArgs = new RuleArgs("Symbol Validation");

            // Act
            bool result = CollateralImport.CustomRules.SymbolCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CollateralImport")]
        public void SymbolCheck_WithValidSymbol_ReturnsTrue()
        {
            // Arrange
            var finalTarget = new CollateralImport { Symbol = "ValidSymbol", AUECID = 1, Date = "2023-10-24" };
            var ruleArgs = new RuleArgs("Symbol Validation");

            // Act
            bool result = CollateralImport.CustomRules.SymbolCheck(finalTarget, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CollateralImport")]
        public void SymbolCheck_WithEmptySymbol_ReturnsFalse()
        {
            // Arrange
            var finalTarget = new CollateralImport { Symbol = "", AUECID = 1, Date = "2023-10-24" };
            var ruleArgs = new RuleArgs("Symbol Validation");

            // Act
            bool result = CollateralImport.CustomRules.SymbolCheck(finalTarget, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Symbol not validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CollateralImport")]
        public void DateCheck_WithNullCollateralImport_ReturnsFalse()
        {
            // Arrange
            object target = null;
            var ruleArgs = new RuleArgs("Symbol Validation");

            // Act
            bool result = CollateralImport.CustomRules.DateCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CollateralImport")]
        public void DateCheck_WithValidDate_ReturnsTrue()
        {
            // Arrange
            var finalTarget = new CollateralImport { Symbol = "ValidSymbol", AUECID = 1, Date = "2023-10-24" };
            var ruleArgs = new RuleArgs("Date Validation");

            // Act
            bool result = CollateralImport.CustomRules.DateCheck(finalTarget, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CollateralImport")]
        public void DateCheck_WithEmptyDate_ReturnsFalse()
        {
            // Arrange
            var finalTarget = new CollateralImport { Symbol = "ValidSymbol", AUECID = 1, Date = "" };
            var ruleArgs = new RuleArgs("Date Validation");

            // Act
            bool result = CollateralImport.CustomRules.DateCheck(finalTarget, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Date required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CollateralImport")]
        public void AUECIDCheck_WithNullCollateralImport_ReturnsFalse()
        {
            // Arrange
            object target = null;
            var ruleArgs = new RuleArgs("AUECID Validation");

            // Act
            bool result = CollateralImport.CustomRules.AUECIDCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CollateralImport")]
        public void AUECIDCheck_WithValidAUECID_ReturnsTrue()
        {
            // Arrange
            var finalTarget = new CollateralImport { Symbol = "ValidSymbol", AUECID = 1, Date = "2023-10-24" };
            var ruleArgs = new RuleArgs("AUECID Validation");

            // Act
            bool result = CollateralImport.CustomRules.AUECIDCheck(finalTarget, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CollateralImport")]
        public void AUECIDCheck_WithInvalidAUECID_ReturnsFalse()
        {
            // Arrange
            var finalTarget = new CollateralImport { Symbol = "ValidSymbol", AUECID = 0, Date = "2023-10-24" };
            var ruleArgs = new RuleArgs("AUECID Validation");

            // Act
            bool result = CollateralImport.CustomRules.AUECIDCheck(finalTarget, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Invalid AUECID", ruleArgs.Description);
        }
    }
}
