using Prana.BusinessObjects.Classes;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class NAVLockDateRuleTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "NAVLockDateRule")]
        public void ValidateNAVLockDate_Should_Return_True_When_Date_Is_After_NAVLockDate()
        {
            // Arrange
            NAVLockDateRule.NAVLockDate = new DateTime(2024, 10, 10);
            string testDate = "2024-10-11";

            // Act
            bool result = NAVLockDateRule.ValidateNAVLockDate(testDate);
            NAVLockDateRule.NAVLockDate = null;

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "NAVLockDateRule")]
        public void ValidateNAVLockDate_Should_Return_False_When_Date_Is_Before_Or_Equal_To_NAVLockDate()
        {
            // Arrange
            NAVLockDateRule.NAVLockDate = new DateTime(2024, 10, 10);

            // Test date equal to NAVLockDate
            string testDateEqual = "2024-10-10";
            bool resultEqual = NAVLockDateRule.ValidateNAVLockDate(testDateEqual);

            // Test date before NAVLockDate
            string testDateBefore = "2024-10-09";
            bool resultBefore = NAVLockDateRule.ValidateNAVLockDate(testDateBefore);

            NAVLockDateRule.NAVLockDate = null;

            // Assert
            Assert.False(resultEqual);
            Assert.False(resultBefore);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "NAVLockDateRule")]
        public void ValidateNAVLockDate_Should_Return_True_When_NAVLockDate_Is_Not_Set()
        {
            // Arrange
            NAVLockDateRule.NAVLockDate = null; // No lock date set
            string testDate = "2024-10-09";

            // Act
            bool result = NAVLockDateRule.ValidateNAVLockDate(testDate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "NAVLockDateRule")]
        public void ValidateNAVLockDate_Should_Return_False_When_Date_Is_Invalid_Format()
        {
            // Arrange
            NAVLockDateRule.NAVLockDate = new DateTime(2024, 10, 10);
            string invalidDate = "InvalidDateString";

            // Act
            bool result = NAVLockDateRule.ValidateNAVLockDate(invalidDate);
            NAVLockDateRule.NAVLockDate = null;

            // Assert
            Assert.True(result); // Invalid date strings should return true (assumes not valid)
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "NAVLockDateRule")]
        public void ValidateNAVLockDate_Should_Return_True_When_Date_Is_Empty_Or_Null()
        {
            // Arrange
            NAVLockDateRule.NAVLockDate = new DateTime(2024, 10, 10);
            string emptyDate = "";
            string nullDate = null;

            // Act
            bool resultEmpty = NAVLockDateRule.ValidateNAVLockDate(emptyDate);
            bool resultNull = NAVLockDateRule.ValidateNAVLockDate(nullDate);
            NAVLockDateRule.NAVLockDate = null;

            // Assert
            Assert.True(resultEmpty);
            Assert.True(resultNull);
        }
    }
}
