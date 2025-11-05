using Prana.BusinessObjects.Classes.Allocation;
using System;
using System.Collections.Generic;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.Allocation
{
    public class AllocationOperationPreferenceTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationOperationPreference")]
        public void TryUpdateCheckList_ShouldAddChecklist_WhenValid()
        {
            // Arrange
            var preference = new AllocationOperationPreference();
            var checklist = new CheckListWisePreference { ChecklistId = 1 };

            // Act
            var result = preference.TryUpdateCheckList(checklist);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationOperationPreference")]
        public void TryUpdateCheckList_ShouldThrowException_WhenDuplicateAndNotOverwrite()
        {
            // Arrange
            var preference = new AllocationOperationPreference();
            var checklist = new CheckListWisePreference { ChecklistId = 1 };
            preference.TryUpdateCheckList(checklist); // Add first checklist

            // Act & Assert
            var exception = Assert.Throws<NullReferenceException>(() =>
                preference.TryUpdateCheckList(checklist) // Try to add the same checklist again
            );
            Assert.Equal("Object reference not set to an instance of an object.", exception.Message);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationOperationPreference")]
        public void IsValid_ShouldReturnTrue_WhenValid()
        {
            // Arrange
            var defaultRule = new AllocationRule
            {
                RuleType = MatchingRuleType.Prorata,
                ProrataAccountList = new List<int> { 1, 2 }
            };

            var preference = new AllocationOperationPreference(1, 1, 1,"Test", defaultRule, DateTime.Now,true);
            
            preference.TargetPercentage.Add(1, new AccountValue { Value = 50 });
            preference.TargetPercentage.Add(2, new AccountValue { Value = 50 });

            // Act
            string errorMessage;
            var result = preference.IsValid(out errorMessage);

            // Assert
            Assert.True(result);
            Assert.Equal(string.Empty, errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationOperationPreference")]
        public void IsValid_ShouldReturnFalse_WhenNegativeAccountValue()
        {
            // Arrange
            var defaultRule = new AllocationRule
            {
                RuleType = MatchingRuleType.SinceInception,
                ProrataAccountList = new List<int> { 1, 2 }
            };

            var preference = new AllocationOperationPreference(1, 1, 1, "Test", defaultRule, DateTime.Now, true);
            AccountValue accountValue = new AccountValue();
            accountValue.AccountId = 1;
            accountValue.Value = -10;

            preference.TryUpdateTargetPercentage(accountValue);
            
            // Act
            string errorMessage;
            var result = preference.IsValid(out errorMessage);

            // Assert
            Assert.False(result);
            Assert.Contains("negative", errorMessage); 
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationOperationPreference")]
        public void GetSelectedAccountsList_ShouldReturnCorrectAccounts_WhenCalled()
        {
            // Arrange
            var defaultRule = new AllocationRule
            {
                RuleType = MatchingRuleType.Prorata,
                ProrataAccountList = new List<int> { 1, 3 }
            };

            var preference = new AllocationOperationPreference(1, 1, 1, "Test", defaultRule, DateTime.Now, true);

            // Act
            var selectedAccounts = preference.GetSelectedAccountsList();

            // Assert
            Assert.Contains(1, selectedAccounts);
            Assert.Contains(3, selectedAccounts);
        }
    }
}
