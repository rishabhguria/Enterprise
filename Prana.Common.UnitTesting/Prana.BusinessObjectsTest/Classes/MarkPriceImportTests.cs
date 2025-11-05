using Csla.Validation;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using Prana.Global;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class MarkPriceImportTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "MarkPriceImport")]
        public void CheckValidationOnAllFileds_Should_Set_ValidationStatus_To_NotExists_When_AUECID_Is_LessThanOrEqualTo_Zero()
        {
            // Arrange
            var markPriceImport = new MarkPriceImport { AUECID = 0, IsSecApproved = true, Symbol = "AAPL", Date = "2024-10-10" };

            // Act
            MarkPriceImport.CustomRules.CheckValidationOnAllFileds(markPriceImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.NotExists.ToString(), markPriceImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MarkPriceImport")]
        public void CheckValidationOnAllFileds_Should_Set_ValidationStatus_To_UnApproved_When_Security_Is_Not_Approved()
        {
            // Arrange
            var markPriceImport = new MarkPriceImport { AUECID = 100, IsSecApproved = false, Symbol = "AAPL", Date = "2024-10-10" };

            // Act
            MarkPriceImport.CustomRules.CheckValidationOnAllFileds(markPriceImport);

            // Assert
            Assert.Equal(ApplicationConstants.ValidationStatus.UnApproved.ToString(), markPriceImport.ValidationStatus);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MarkPriceImport")]
        public void SymbolCheck_Should_Return_False_If_Symbol_Is_NullOrEmpty()
        {
            // Arrange
            var markPriceImport = new MarkPriceImport { Symbol = "" };
            var ruleArgs = new RuleArgs("Test");

            // Act
            var result = MarkPriceImport.CustomRules.SymbolCheck(markPriceImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Symbol not validated", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MarkPriceImport")]
        public void SymbolCheck_Should_Return_True_If_Symbol_Is_Valid()
        {
            // Arrange
            var markPriceImport = new MarkPriceImport { Symbol = "AAPL" };
            var ruleArgs = new RuleArgs("Test");

            // Act
            var result = MarkPriceImport.CustomRules.SymbolCheck(markPriceImport, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MarkPriceImport")]
        public void DateCheck_Should_Return_False_If_Date_Is_NullOrEmpty()
        {
            // Arrange
            var markPriceImport = new MarkPriceImport { Date = "" };
            var ruleArgs = new RuleArgs("Test");

            // Act
            var result = MarkPriceImport.CustomRules.DateCheck(markPriceImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Date required", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MarkPriceImport")]
        public void DateCheck_Should_Return_False_When_Date_Is_Before_NAVLockDate()
        {
            // Arrange
            var navLockDate = new DateTime(2024, 10, 10); // Set a NAV lock date in the past
            NAVLockDateRule.NAVLockDate = navLockDate;

            var markPriceImport = new MarkPriceImport { Date = "2024-10-09" };
            var ruleArgs = new RuleArgs("Test");

            // Act
            var result = MarkPriceImport.CustomRules.DateCheck(markPriceImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("The date you’ve chosen for this action precedes your NAV Lock date (" + navLockDate.ToShortDateString() + "). Please reach out to your Support Team for further assistance.", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MarkPriceImport")]
        public void DateCheck_Should_Return_True_When_Date_Is_After_NAVLockDate()
        {
            // Arrange
            var navLockDate = new DateTime(2024, 10, 10); // Set a NAV lock date in the past
            NAVLockDateRule.NAVLockDate = navLockDate;

            var markPriceImport = new MarkPriceImport { Date = "2024-10-11" };
            var ruleArgs = new RuleArgs("Test");

            // Act
            var result = MarkPriceImport.CustomRules.DateCheck(markPriceImport, ruleArgs);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MarkPriceImport")]
        public void AUECIDCheck_Should_Return_False_When_AUECID_Is_LessThanOrEqualTo_Zero()
        {
            // Arrange
            var markPriceImport = new MarkPriceImport { AUECID = 0 };
            var ruleArgs = new RuleArgs("Test");

            // Act
            var result = MarkPriceImport.CustomRules.AUECIDCheck(markPriceImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Invalid AUECID", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MarkPriceImport")]
        public void AUECIDCheck_Should_Return_True_When_AUECID_Is_GreaterThanOrEqualThan_Zero()
        {
            // Arrange
            var markPriceImport = new MarkPriceImport { AUECID = 1 };
            var ruleArgs = new RuleArgs("Test");

            // Act
            var result = MarkPriceImport.CustomRules.AUECIDCheck(markPriceImport, ruleArgs);

            // Assert
            Assert.True(result);
            Assert.Null(ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MarkPriceImport")]
        public void IsSecurityApprovedCheck_Should_Return_False_If_Security_Is_Not_Approved()
        {
            // Arrange
            var markPriceImport = new MarkPriceImport { AUECID = 100, IsSecApproved = false };
            var ruleArgs = new RuleArgs("Test");

            // Act
            var result = MarkPriceImport.CustomRules.IsSecurityApprovedCheck(markPriceImport, ruleArgs);

            // Assert
            Assert.False(result);
            Assert.Equal("Security not Approved", ruleArgs.Description);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MarkPriceImport")]
        public void IsSecurityApprovedCheck_Should_Return_True_If_Security_Is_Approved()
        {
            // Arrange
            var markPriceImport = new MarkPriceImport { AUECID = 100, IsSecApproved = true };
            var ruleArgs = new RuleArgs("Test");

            // Act
            var result = MarkPriceImport.CustomRules.IsSecurityApprovedCheck(markPriceImport, ruleArgs);

            // Assert
            Assert.True(result);
        }
    }
}
