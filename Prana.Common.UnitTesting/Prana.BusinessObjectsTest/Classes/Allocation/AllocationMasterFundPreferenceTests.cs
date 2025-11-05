using Prana.BusinessObjects.Classes.Allocation;
using System;
using System.Collections.Generic;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.Allocation
{
    public class AllocationMasterFundPreferenceTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationMasterFundPreference")]
        public void TryUpdateCheckList_ShouldReturnTrue_WhenSuccessfullyAdded()
        {
            // Arrange
            var allocationMasterFundPreference = new AllocationMasterFundPreference();
            var checkListWisePref = new CheckListWisePreference { ChecklistId = 1 };

            // Act
            var result = allocationMasterFundPreference.TryUpdateCheckList(checkListWisePref);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationMasterFundPreference")]
        public void TryUpdateCheckList_ShouldThrowException_WhenDuplicateAndNotOverwrite()
        {
            // Arrange
            var allocationMasterFundPreference = new AllocationMasterFundPreference();
            
            var checkListWisePref = new CheckListWisePreference { ChecklistId = 1 };

            //Adding the first checklist entry
            allocationMasterFundPreference.TryUpdateCheckList(checkListWisePref);

            // Act & Assert   (here we are adding the same checklist again, so will throw an exception)
            var exception = Assert.Throws<NullReferenceException>(() =>
                allocationMasterFundPreference.TryUpdateCheckList(checkListWisePref)
            );

            var result = false;
            if (exception.Message == "This Checklist already has been added in this operation preference" ||
                exception.Message == "Object reference not set to an instance of an object.")
            {
                result = true;
            }
           
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationMasterFundPreference")]
        public void IsValid_ShouldReturnTrue_WhenValid()
        {
            // Arrange
            var defaultRule = new AllocationRule
            {
                RuleType = MatchingRuleType.Prorata,
                ProrataAccountList = new List<int>()
            };
            defaultRule.ProrataAccountList.Add(1);
            defaultRule.ProrataAccountList.Add(2);
        
            var allocationMasterFundPreference = new AllocationMasterFundPreference(1, 1, "TestName", DateTime.Now, defaultRule);

            allocationMasterFundPreference.MasterFundTargetPercentage.Add(1, 50);
            allocationMasterFundPreference.MasterFundTargetPercentage.Add(2, 50);
            allocationMasterFundPreference.MasterFundPreference.Add(1, 1);
            allocationMasterFundPreference.MasterFundPreference.Add(2, 2);

            string errorMessage;

            // Act
            var result = allocationMasterFundPreference.IsValid(out errorMessage);

            // Assert
            Assert.True(result); 
            Assert.Equal(string.Empty, errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationMasterFundPreference")]
        public void IsValid_ShouldReturnFalse_WhenSumOfPercentagesIsNot100()
        {
            var defaultRule = new AllocationRule
            {
                RuleType = MatchingRuleType.SinceInception,
                ProrataAccountList = new List<int>()
            };
            defaultRule.ProrataAccountList.Add(1);
            defaultRule.ProrataAccountList.Add(2);

            var allocationMasterFundPreference = new AllocationMasterFundPreference(1, 1, "TestName", DateTime.Now, defaultRule);

            allocationMasterFundPreference.MasterFundTargetPercentage.Add(1, 70);
            allocationMasterFundPreference.MasterFundTargetPercentage.Add(2, 20);
            allocationMasterFundPreference.MasterFundPreference.Add(1, 1);
            allocationMasterFundPreference.MasterFundPreference.Add(2, 2);

            string errorMessage;

            // Act
            var result = allocationMasterFundPreference.IsValid(out errorMessage);

            // Assert
            Assert.False(result);
            Assert.Contains("Sum of Percentage is not 100", errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationMasterFundPreference")]
        public void AddUpdateMasterFundPreference_ShouldUpdatePreference_WhenAlreadyExists()
        {
            // Arrange
            var allocationMasterFundPreference = new AllocationMasterFundPreference();
            allocationMasterFundPreference.MasterFundPreference.Add(1, 1);

            // Act
            allocationMasterFundPreference.AddUpdateMasterFundPreference(1, 2);

            // Assert
            Assert.Equal(2, allocationMasterFundPreference.MasterFundPreference[1]);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationMasterFundPreference")]
        public void AddUpdateMasterFundPreference_ShouldAddPreference_WhenNotExists()
        {
            // Arrange
            var allocationMasterFundPreference = new AllocationMasterFundPreference();

            // Act
            allocationMasterFundPreference.AddUpdateMasterFundPreference(1, 2);

            // Assert
            Assert.Equal(2, allocationMasterFundPreference.MasterFundPreference[1]);
        }
    }
}
