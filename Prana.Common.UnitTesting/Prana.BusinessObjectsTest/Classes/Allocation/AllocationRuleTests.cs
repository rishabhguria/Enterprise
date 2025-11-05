using Prana.BusinessObjects.Classes.Allocation;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.Allocation
{
    public class AllocationRuleTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationRule")]
        public void IsValid_ShouldReturnFalse_WhenNotValid()
        {
            // Arrange
            var defaultRule = new AllocationRule();
            defaultRule.RuleType = MatchingRuleType.Leveling;  
          
            // Act
            string errorMessage;
            var result = defaultRule.IsValid(out errorMessage);

            // Assert
            Assert.False(result);
            Assert.Contains(AllocationStringConstants.ACCOUNT_LIST_NOT_EMPTY, errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationRule")]
        public void IsValid_ShouldReturnTrue_WhenValid()
        {
            // Arrange
            var defaultRule = new AllocationRule();
            defaultRule.RuleType = MatchingRuleType.None;

            // Act
            string errorMessage;
            var result = defaultRule.IsValid(out errorMessage);

            // Assert
            Assert.True(result);
            Assert.Empty(errorMessage);
        }
    }
}
