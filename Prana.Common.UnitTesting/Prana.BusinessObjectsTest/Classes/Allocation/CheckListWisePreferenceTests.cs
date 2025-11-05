using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using System.Collections.Generic;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.Allocation
{
    public class CheckListWisePreferenceTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "CheckListWisePreference")]
        public void IsValid_ShouldReturnFalse_WhenNoExchangeIsSelected()
        {
            // Arrange
            SerializableDictionary<int, AccountValue> TargetPercentage = new SerializableDictionary<int, AccountValue>();
            TargetPercentage.Add(1, new AccountValue { Value = 50 });
            TargetPercentage[1].StrategyValueList = new List<StrategyValue>(); 
            
            var checklistPreference = new CheckListWisePreference(1, AllocationBaseType.Notional, MatchingRuleType.None, 2, MatchClosingTransactionType.None, null, 1, TargetPercentage);
            
            checklistPreference.ExchangeOperator = CustomOperator.Exclude;
            checklistPreference.ExchangeList = new List<int>();
        
            // Act
            var isValid = checklistPreference.IsValid(out string errorMessage);

            // Assert
            Assert.False(isValid);
            Assert.Contains(AllocationStringConstants.NO_EXCHANGE_SELECTED, errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CheckListWisePreference")]
        public void IsValid_ShouldReturnFalse_WhenNoOrderSideIsSelected()
        {
            // Arrange
            SerializableDictionary<int, AccountValue> TargetPercentage = new SerializableDictionary<int, AccountValue>();
            TargetPercentage.Add(1, new AccountValue { Value = 50 });
            TargetPercentage[1].StrategyValueList = new List<StrategyValue>();

            var checklistPreference = new CheckListWisePreference(1, AllocationBaseType.Notional, MatchingRuleType.None, 2, MatchClosingTransactionType.None, null, 1, TargetPercentage);

            checklistPreference.OrderSideOperator = CustomOperator.Exclude;
            checklistPreference.OrderSideList = new List<string>();

            // Act
            var isValid = checklistPreference.IsValid(out string errorMessage);

            // Assert
            Assert.False(isValid);
            Assert.Contains(AllocationStringConstants.NO_ORDERSIDE_SELECTED, errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CheckListWisePreference")]
        public void IsValid_ShouldReturnTrue_WhenValidChecklist()
        {
            // Arrange
            SerializableDictionary<int, AccountValue> TargetPercentage = new SerializableDictionary<int, AccountValue>();
            TargetPercentage.Add(1, new AccountValue { Value = 100 });
            TargetPercentage[1].StrategyValueList = new List<StrategyValue>();

            var checklistPreference = new CheckListWisePreference(1, AllocationBaseType.Notional, MatchingRuleType.None, 2, MatchClosingTransactionType.None, null, 1, TargetPercentage);
            checklistPreference.ExchangeOperator = CustomOperator.All;

            // Act
            var isValid = checklistPreference.IsValid(out string errorMessage);

            // Assert
            Assert.True(isValid);
            Assert.True(string.IsNullOrEmpty(errorMessage));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CheckListWisePreference")]
        public void IsTargetPercentageValid_ShouldReturnFalse_WhenSumIsNot100()
        {
            // Arrange
            var rule = new AllocationRule { RuleType = MatchingRuleType.None };
            SerializableDictionary<int, AccountValue> TargetPercentage = new SerializableDictionary<int, AccountValue>();
            TargetPercentage.Add(1, new AccountValue { Value = 40 });
            TargetPercentage.Add(2, new AccountValue { Value = 50 });
            TargetPercentage[1].StrategyValueList = new List<StrategyValue>();
            TargetPercentage[2].StrategyValueList = new List<StrategyValue>();

            var checklistPreference = new CheckListWisePreference(1, AllocationBaseType.Notional, MatchingRuleType.None, 2, MatchClosingTransactionType.None, null, 1, TargetPercentage);

            // Act
            var isValid = checklistPreference.IsTargetPercentageValid(out string errorMessage, rule, TargetPercentage);

            // Assert
            Assert.False(isValid);
            Assert.Contains(AllocationStringConstants.PERCENTAGE_SUM_NOT_100, errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CheckListWisePreference")]
        public void IsTargetPercentageValid_ShouldReturnFalse_WhenNegativePercentageFound()
        {
            // Arrange
            var rule = new AllocationRule { RuleType = MatchingRuleType.None };
            SerializableDictionary<int, AccountValue> TargetPercentage = new SerializableDictionary<int, AccountValue>();
            TargetPercentage.Add(1, new AccountValue { Value = -40 });
            TargetPercentage[1].StrategyValueList = new List<StrategyValue>();
           
            var checklistPreference = new CheckListWisePreference(1, AllocationBaseType.Notional, MatchingRuleType.None, 2, MatchClosingTransactionType.None, null, 1, TargetPercentage);

            // Act
            var isValid = checklistPreference.IsTargetPercentageValid(out string errorMessage, rule, TargetPercentage);

            // Assert
            Assert.False(isValid);
            Assert.Contains(AllocationStringConstants.PERCENTAGE_NOT_NEGATIVE, errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CheckListWisePreference")]
        public void IsTargetPercentageValid_ShouldReturnTrue_WhenValidPercentage()
        {
            // Arrange
            var rule = new AllocationRule { RuleType = MatchingRuleType.None };
            SerializableDictionary<int, AccountValue> TargetPercentage = new SerializableDictionary<int, AccountValue>();
            TargetPercentage.Add(1, new AccountValue { Value = 50 });//sum is 100
            TargetPercentage.Add(2, new AccountValue { Value = 50 });
            TargetPercentage[1].StrategyValueList = new List<StrategyValue>();
            TargetPercentage[2].StrategyValueList = new List<StrategyValue>();

            var checklistPreference = new CheckListWisePreference(1, AllocationBaseType.Notional, MatchingRuleType.None, 2, MatchClosingTransactionType.None, null, 1, TargetPercentage);

            // Act
            var isValid = checklistPreference.IsTargetPercentageValid(out string errorMessage, rule, TargetPercentage);

            // Assert
            Assert.True(isValid);
            Assert.True(string.IsNullOrEmpty(errorMessage));
        }
    }
}
