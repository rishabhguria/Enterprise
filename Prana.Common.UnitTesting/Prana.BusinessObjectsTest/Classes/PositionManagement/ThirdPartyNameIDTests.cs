using Csla.Validation;
using Prana.BusinessObjects.PositionManagement;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PositionManagement
{
    public class ThirdPartyNameIDTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "ThirdPartyNameID")]
        public void DataSourceRequired_WithValidId_ReturnsTrue()
        {
            // Arrange
            var thirdParty = new ThirdPartyNameID { ID = 1 };
            var ruleArgs = new RuleArgs("DataSourceValidation");

            // Act
            bool result = ThirdPartyNameID.CustomClass.DataSourceRequired(thirdParty, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ThirdPartyNameID")]
        public void DataSourceRequired_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            var thirdParty = new ThirdPartyNameID { ID = 0 };
            var ruleArgs = new RuleArgs("DataSourceValidation");

            // Act
            bool result = ThirdPartyNameID.CustomClass.DataSourceRequired(thirdParty, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Data Source required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ThirdPartyNameID")]
        public void DataSourceRequired_WithNegativeId_ReturnsFalse()
        {
            // Arrange
            var thirdParty = new ThirdPartyNameID { ID = -1 };
            var ruleArgs = new RuleArgs("DataSourceValidation");

            // Act
            bool result = ThirdPartyNameID.CustomClass.DataSourceRequired(thirdParty, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Data Source required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ThirdPartyNameID")]
        public void DataSourceRequired_WithNullTarget_ReturnsFalse()
        {
            // Arrange
            ThirdPartyNameID thirdParty = null;
            var ruleArgs = new RuleArgs("DataSourceValidation");

            // Act
            bool result = ThirdPartyNameID.CustomClass.DataSourceRequired(thirdParty, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "ThirdPartyNameID")]
        public void DataSourceRequired_WithDifferentType_ReturnsFalse()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("DataSourceValidation");

            // Act
            bool result = ThirdPartyNameID.CustomClass.DataSourceRequired(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Null(ruleArgs.Description);
        }
    }
}
