using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class CommissionRuleTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "CommissionRule")]
        public void IsValid_ReturnsTrue_WhenAllConditionsAreMet()
        {
            // Arrange
            var commissionRule = new CommissionRule
            {
                IsCommissionApplied = true,
                Commission = new Commission { CommissionRate = 10 },
                IsSoftCommissionApplied = true,
                SoftCommission = new Commission { CommissionRate = 5 },
                IsClearingBrokerFeeApplied = true,
                ClearingBrokerFeeObj = new ClearingFee { ClearingFeeRate = 3 },
                IsClearingFeeApplied = true,
                ClearingFeeObj = new ClearingFee { ClearingFeeRate = 2 },
                IsTransactionLevyApplied = true,TransactionLevy = 1,
                IsTaxonCommissionsApplied = true,TaxonCommissions = 1,
                IsStampDutyApplied = true,StampDuty = 1,
                IsClearingFee_AApplied = true,ClearingFee_A = 1,
                IsMiscFeesApplied = true,MiscFees = 1,
                IsSecFeeApplied = true,SecFee = 1,
                IsOccFeeApplied = true,OccFee = 1,
               IsOrfFeeApplied = true, OrfFee = 1,
            };

            // Act
            var isValid = commissionRule.IsValid(out var errorMessage);

            // Assert
            Assert.True(isValid);
            Assert.Empty(errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CommissionRule")]
        public void IsValid_ReturnsFalse_WhenCommissionRateIsNegative()
        {
            // Arrange
            var commissionRule = new CommissionRule
            {
                IsCommissionApplied = true,
                Commission = new Commission { CommissionRate = -10 }
            };

            // Act
            var isValid = commissionRule.IsValid(out var errorMessage);

            // Assert
            Assert.False(isValid);
            Assert.Contains("Commission", errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CommissionRule")]
        public void IsValid_ReturnsFalse_WhenCalculationBasisIsAuto()
        {
            // Arrange
            var commissionRule = new CommissionRule
            {
                IsCommissionApplied = true,
                Commission = new Commission { RuleAppliedOn = CalculationBasis.Auto }
            };

            // Act
            var isValid = commissionRule.IsValid(out var errorMessage);

            // Assert
            Assert.False(isValid);
            Assert.Contains("Commission", errorMessage);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "CommissionRule")]
        public void IsValid_ReturnsFalse_WhenZeroRateRequiresFlatAmount()
        {
            // Arrange
            var commissionRule = new CommissionRule
            {
                IsCommissionApplied = true,
                Commission = new Commission { CommissionRate = 0, RuleAppliedOn = CalculationBasis.Auto }
            };

            // Act
            var isValid = commissionRule.IsValid(out var errorMessage);

            // Assert
            Assert.False(isValid);
            Assert.Contains("Commission", errorMessage);
        }
    }
}
