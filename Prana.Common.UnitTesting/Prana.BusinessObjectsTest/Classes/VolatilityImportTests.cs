using Csla.Validation;
using Prana.BusinessObjects;
using Prana.Global;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class VolatilityImportTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void CheckValidationOnAllFields_ShouldSetNotExists_WhenAUECIDIsLessThanOrEqualToZero()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                Symbol = "AAPL",
                Date = "2024-10-25",
                AUECID = 0 // Invalid AUECID
            };

            // Act
            VolatilityImport.CustomRules.CheckValidationOnAllFileds(volatilityImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.NotExists.ToString(), volatilityImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void CheckValidationOnAllFields_ShouldSetMissingData_WhenSymbolOrDateIsMissing()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                Symbol = null, // Missing
                Date = "2024-10-25",
                AUECID = 1 // Valid AUECID
            };

            // Act
            VolatilityImport.CustomRules.CheckValidationOnAllFileds(volatilityImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), volatilityImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void CheckValidationOnAllFields_ShouldSetValidated_WhenAllFieldsAreValid()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                Symbol = "AAPL",
                Date = "2024-10-25",
                AUECID = 1 // Valid AUECID
            };

            // Act
            VolatilityImport.CustomRules.CheckValidationOnAllFileds(volatilityImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.Validated.ToString(), volatilityImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void CheckValidationOnAllFields_ShouldSetMissingData_WhenSymbolIsEmpty()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                Symbol = string.Empty, // Empty Symbol
                Date = "2024-10-25",
                AUECID = 1 // Valid AUECID
            };

            // Act
            VolatilityImport.CustomRules.CheckValidationOnAllFileds(volatilityImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), volatilityImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void CheckValidationOnAllFields_ShouldSetMissingData_WhenDateIsEmpty()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                Symbol = "AAPL",
                Date = string.Empty, // Empty Date
                AUECID = 1 // Valid AUECID
            };

            // Act
            VolatilityImport.CustomRules.CheckValidationOnAllFileds(volatilityImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), volatilityImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void SymbolCheck_ShouldReturnTrue_WhenSymbolIsValid()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                Symbol = "AAPL", // Valid Symbol
                AUECID = 1, // Valid AUECID
                Date = "2024-10-25"
            };
            var ruleArgs = new RuleArgs("SymbolValidation");

            // Act
            var result = VolatilityImport.CustomRules.SymbolCheck(volatilityImport, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void SymbolCheck_ShouldReturnFalse_WhenSymbolIsNull()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                Symbol = null, // Null Symbol
                AUECID = 1, // Valid AUECID
                Date = "2024-10-25"
            };
            var ruleArgs = new RuleArgs("SymbolValidation");

            // Act
            var result = VolatilityImport.CustomRules.SymbolCheck(volatilityImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Symbol not validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void SymbolCheck_ShouldReturnFalse_WhenSymbolIsEmpty()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                Symbol = string.Empty, // Empty Symbol
                AUECID = 1, // Valid AUECID
                Date = "2024-10-25"
            };
            var ruleArgs = new RuleArgs("SymbolValidation");

            // Act
            var result = VolatilityImport.CustomRules.SymbolCheck(volatilityImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Symbol not validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void SymbolCheck_ShouldReturnFalse_WhenTargetIsNull()
        {
            // Arrange
            VolatilityImport volatilityImport = null;
            var ruleArgs = new RuleArgs("SymbolValidation");

            // Act
            var result = VolatilityImport.CustomRules.SymbolCheck(volatilityImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void DateCheck_ShouldReturnTrue_WhenDateIsValid()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                Date = "2024-10-25", // Valid Date
                Symbol = "AAPL", // Valid Symbol
                AUECID = 1 // Valid AUECID
            };
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            var result = VolatilityImport.CustomRules.DateCheck(volatilityImport, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void DateCheck_ShouldReturnFalse_WhenDateIsNull()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                Date = null, // Null Date
                Symbol = "AAPL", // Valid Symbol
                AUECID = 1 // Valid AUECID
            };
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            var result = VolatilityImport.CustomRules.DateCheck(volatilityImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Date required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void DateCheck_ShouldReturnFalse_WhenDateIsEmpty()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                Date = string.Empty, // Empty Date
                Symbol = "AAPL", // Valid Symbol
                AUECID = 1 // Valid AUECID
            };
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            var result = VolatilityImport.CustomRules.DateCheck(volatilityImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Date required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void DateCheck_ShouldReturnFalse_WhenTargetIsNull()
        {
            // Arrange
            VolatilityImport volatilityImport = null;
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            var result = VolatilityImport.CustomRules.DateCheck(volatilityImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void AUECIDCheck_ShouldReturnTrue_WhenAUECIDIsValid()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                AUECID = 1, // Valid AUECID
                Symbol = "AAPL", // Valid Symbol
                Date = "2024-10-25" // Valid Date
            };
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = VolatilityImport.CustomRules.AUECIDCheck(volatilityImport, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void AUECIDCheck_ShouldReturnFalse_WhenAUECIDIsZero()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                AUECID = 0, // Invalid AUECID
                Symbol = "AAPL", // Valid Symbol
                Date = "2024-10-25" // Valid Date
            };
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = VolatilityImport.CustomRules.AUECIDCheck(volatilityImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Invalid AUECID", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void AUECIDCheck_ShouldReturnFalse_WhenAUECIDIsNegative()
        {
            // Arrange
            var volatilityImport = new VolatilityImport
            {
                AUECID = -1, // Invalid AUECID
                Symbol = "AAPL", // Valid Symbol
                Date = "2024-10-25" // Valid Date
            };
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = VolatilityImport.CustomRules.AUECIDCheck(volatilityImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Invalid AUECID", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilityImport")]
        public void AUECIDCheck_ShouldReturnFalse_WhenTargetIsNull()
        {
            // Arrange
            VolatilityImport volatilityImport = null;
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = VolatilityImport.CustomRules.AUECIDCheck(volatilityImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Null(ruleArgs.Description);
        }
    }
}
