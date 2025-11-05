using Csla.Validation;
using Prana.BusinessObjects;
using Prana.Global;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class VWAPImportTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "VWAPImport")]
        public void CheckValidationOnAllFields_ShouldSetValidationStatusToValidated_WhenFieldsAreValid()
        {
            // Arrange
            var vwapImport = new VWAPImport
            {
                Symbol = "AAPL",  // Valid symbol
                Date = "2024-10-25",  // Valid date
                AUECID = 1  // Valid AUECID
            };

            // Act
            VWAPImport.CustomRules.CheckValidationOnAllFileds(vwapImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.Validated.ToString(), vwapImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VWAPImport")]
        public void CheckValidationOnAllFields_ShouldSetValidationStatusToMissingData_WhenSymbolIsEmpty()
        {
            // Arrange
            var vwapImport = new VWAPImport
            {
                Symbol = "",  // Missing symbol
                Date = "2024-10-25",  // Valid date
                AUECID = 1  // Valid AUECID
            };

            // Act
            VWAPImport.CustomRules.CheckValidationOnAllFileds(vwapImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), vwapImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VWAPImport")]
        public void CheckValidationOnAllFields_ShouldSetValidationStatusToMissingData_WhenDateIsEmpty()
        {
            // Arrange
            var vwapImport = new VWAPImport
            {
                Symbol = "AAPL",  // Valid symbol
                Date = "",  // Missing date
                AUECID = 1  // Valid AUECID
            };

            // Act
            VWAPImport.CustomRules.CheckValidationOnAllFileds(vwapImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.MissingData.ToString(), vwapImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VWAPImport")]
        public void CheckValidationOnAllFields_ShouldSetValidationStatusToNotExists_WhenAUECIDIsZero()
        {
            // Arrange
            var vwapImport = new VWAPImport
            {
                Symbol = "AAPL",  // Valid symbol
                Date = "2024-10-25",  // Valid date
                AUECID = 0  // Invalid AUECID
            };

            // Act
            VWAPImport.CustomRules.CheckValidationOnAllFileds(vwapImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.NotExists.ToString(), vwapImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VWAPImport")]
        public void CheckValidationOnAllFields_ShouldSetValidationStatusToNotExists_WhenAUECIDIsNegative()
        {
            // Arrange
            var vwapImport = new VWAPImport
            {
                Symbol = "AAPL",  // Valid symbol
                Date = "2024-10-25",  // Valid date
                AUECID = -1  // Invalid AUECID
            };

            // Act
            VWAPImport.CustomRules.CheckValidationOnAllFileds(vwapImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.NotExists.ToString(), vwapImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VWAPImport")]
        public void DateCheck_ShouldReturnTrue_WhenDateIsValid()
        {
            // Arrange
            var vwapImport = new VWAPImport
            {
                Date = "2024-10-25",  // Valid date
                Symbol = "AAPL",  // Valid symbol
                AUECID = 1  // Valid AUECID
            };
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            var result = VWAPImport.CustomRules.DateCheck(vwapImport, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VWAPImport")]

        public void DateCheck_ShouldReturnFalse_WhenDateIsEmpty()
        {
            // Arrange
            var vwapImport = new VWAPImport
            {
                Date = "",  // Missing date
                Symbol = "AAPL",  // Valid symbol
                AUECID = 1  // Valid AUECID
            };
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            var result = VWAPImport.CustomRules.DateCheck(vwapImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Date required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VWAPImport")]
        public void DateCheck_ShouldReturnFalse_WhenTargetIsNull()
        {
            // Arrange
            VWAPImport vwapImport = null;  // Null target
            var ruleArgs = new RuleArgs("DateValidation");

            // Act
            var result = VWAPImport.CustomRules.DateCheck(vwapImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VWAPImport")]
        public void AUECIDCheck_ShouldReturnTrue_WhenAUECIDIsValid()
        {
            // Arrange
            var vwapImport = new VWAPImport
            {
                AUECID = 1,  // Valid AUECID
                Date = "2024-10-25",  // Valid date
                Symbol = "AAPL"  // Valid symbol
            };
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = VWAPImport.CustomRules.AUECIDCheck(vwapImport, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VWAPImport")]
        public void AUECIDCheck_ShouldReturnFalse_WhenAUECIDIsInvalid()
        {
            // Arrange
            var vwapImport = new VWAPImport
            {
                AUECID = 0,  // Invalid AUECID
                Date = "2024-10-25",  // Valid date
                Symbol = "AAPL"  // Valid symbol
            };
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = VWAPImport.CustomRules.AUECIDCheck(vwapImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Invalid AUECID", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VWAPImport")]
        public void AUECIDCheck_ShouldReturnFalse_WhenTargetIsNull()
        {
            // Arrange
            VWAPImport vwapImport = null;  // Null target
            var ruleArgs = new RuleArgs("AUECIDValidation");

            // Act
            var result = VWAPImport.CustomRules.AUECIDCheck(vwapImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Null(ruleArgs.Description);
        }
    }
}
