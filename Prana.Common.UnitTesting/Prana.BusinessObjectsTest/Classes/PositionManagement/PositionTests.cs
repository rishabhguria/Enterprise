using Csla.Validation;
using Prana.BusinessObjects.PositionManagement;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PositionManagement
{
    public class PositionTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "Position")]
        public void AUECRequired_WithValidAUECID_ReturnsTrue()
        {
            // Arrange
            var position = new Position { AUECID = 1 };
            var ruleArgs = new RuleArgs("AUECValidation");

            // Act
            var result = Position.AddNewPositionRules.AUECRequired(position, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Position")]
        public void AUECRequired_WithInvalidAUECID_ReturnsFalse()
        {
            // Arrange
            var position = new Position { AUECID = 0 };
            var ruleArgs = new RuleArgs("AUECValidation");

            // Act
            var result = Position.AddNewPositionRules.AUECRequired(position, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("AUEC required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Position")]
        public void AUECRequired_WithNullPosition_ReturnsFalse()
        {
            // Arrange
            Position position = null;
            var ruleArgs = new RuleArgs("AUECValidation");

            // Act
            var result = Position.AddNewPositionRules.AUECRequired(position, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "Position")]
        public void AUECRequired_WithInvalidType_ReturnsFalse()
        {
            // Arrange
            var invalidTarget = new object();
            var ruleArgs = new RuleArgs("AUECValidation");

            // Act
            var result = Position.AddNewPositionRules.AUECRequired(invalidTarget, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Null(ruleArgs.Description);
        }
    }
}
