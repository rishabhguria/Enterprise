using Csla.Validation;
using Prana.BusinessObjects;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest
{
    public class BusinessRulesTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "BusinessRules")]
        public void GreaterThanZeroCheck_ValidPositiveValue_ReturnsTrue()
        {
            // Arrange
            var target = new PositionMaster
            {
                StrikePrice = 1.0
            };
            var ruleArgs = new RuleArgs("StrikePrice");

            // Act
            bool result = BusinessRules.GreaterThanZeroCheck(target, ruleArgs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BusinessRules")]
        public void GreaterThanZeroCheck_ValidNegativeValue_ReturnsFalse()
        {
            // Arrange
            var target = new PositionMaster
            {
                StrikePrice = -1.0
            };
            var ruleArgs = new RuleArgs("StrikePrice");

            // Act
            bool result = BusinessRules.GreaterThanZeroCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("InValid StrikePrice", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BusinessRules")]
        public void GreaterThanZeroCheck_ZeroValue_ReturnsFalse()
        {
            // Arrange
            var target = new PositionMaster
            {
                StrikePrice = 0.0
            };
            var ruleArgs = new RuleArgs("StrikePrice");

            // Act
            bool result = BusinessRules.GreaterThanZeroCheck(target, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("InValid StrikePrice", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BusinessRules")]
        public void GreaterThanZeroCheck_InvalidPropertyType_ThrowsException()
        {
            // Arrange
            var target = new PositionMaster
            {
                ImportTag = "Not a double"
            };
            var ruleArgs = new RuleArgs("ImportTag");

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => BusinessRules.GreaterThanZeroCheck(target, ruleArgs));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BusinessRules")]
        public void GreaterThanZeroCheck_NonExistentProperty_ThrowsException()
        {
            // Arrange
            var target = new PositionMaster();
            var ruleArgs = new RuleArgs("NonExistentProperty");

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => BusinessRules.GreaterThanZeroCheck(target, ruleArgs));
        }
    }
}
