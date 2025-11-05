using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.Allocation
{
    public class AllocationParameterTests
    { 
        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationParameter")]
        public void ContainsValidAllocationRule_ShouldReturnFalse_WhenTargetPercentageIsEmpty()
        {
            // Arrange
            var rule = new AllocationRule(); 
            rule.RuleType = MatchingRuleType.None;
            
            var parameter = new AllocationParameter(rule, new SerializableDictionary<int, AccountValue>(), -1, -1, false);
            
            // Act
            var result = parameter.ContainsValidAllocationRule(out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal("Target percentage is not given for this preference", errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationParameter")]
        public void ContainsValidAllocationRule_ShouldReturnTrue_WhenValidRuleAndTargetPercentage()
        {
            // Arrange
            var rule = new AllocationRule();
            rule.RuleType = MatchingRuleType.None;
            var targetPercentage = new SerializableDictionary<int, AccountValue>
            {
                { 1, new AccountValue { Value = 10 } }
            };
            var parameter = new AllocationParameter(rule, targetPercentage, -1, -1, false);

            // Act
            var result = parameter.ContainsValidAllocationRule(out string errorMessage);

            // Assert
            Assert.True(result);
            Assert.Empty(errorMessage);
        }
    }
}
