using Prana.BusinessObjects.AppConstants;
using Prana.Utilities;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest
{
    public class CommissionEnumHelperTests
    {
        [Fact]
        [Trait("Prana.Utilities", "CommissionEnumHelper")]
        public void GetCommissionCriterias_ReturnsValidEnumerationValueList()
        {
            // Act
            var result = CommissionEnumHelper.GetCommissionCriterias();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count == 8);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Prana.Utilities", "CommissionEnumHelper")]
        public void GetOldListForCalculationBasis_ReturnsValidEnumerationValueList(bool isClearingFee)
        {
            // Act
            var result = CommissionEnumHelper.GetOldListForCalculationBasis(isClearingFee);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(isClearingFee ? result.Count == 7 : result.Count == 5);
        }

        [Theory]
        [InlineData(OtherFeeType.MiscFees)]
        [InlineData(OtherFeeType.OrfFee)]
        [Trait("Prana.Utilities", "CommissionEnumHelper")]
        public void GetNewListForCalculationBasis_ReturnsValidEnumerationValueList(OtherFeeType feeType)
        {
            // Act
            var result = CommissionEnumHelper.GetNewListForCalculationBasis(feeType);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count == 19);
        }

        [Fact]
        [Trait("Prana.Utilities", "CommissionEnumHelper")]
        public void GetOtherFeeList_ReturnsValidEnumerationValueList()
        {
            // Act
            var result = CommissionEnumHelper.GetOtherFeeList();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count == 8);
        }

        [Fact]
        [Trait("Prana.Utilities", "CommissionEnumHelper")]
        public void GetOldListForCalculationBasisAsDic_ReturnsValidDictionary()
        {
            // Act
            var result = CommissionEnumHelper.GetOldListForCalculationBasisAsDic();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count == 4);
        }
    }
}
