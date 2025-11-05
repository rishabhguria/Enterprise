using Xunit;
using Prana.BusinessObjects;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.NewLiveFeed
{
    public class EquitySymbolDataTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "EquitySymbolData")]
        public void UpdateContinuousData_ShouldUpdateSharesOutstanding()
        {
            // Arrange
            var initialData = new EquitySymbolData
            {
                SharesOutstanding = 1000
            };

            var updatedData = new EquitySymbolData
            {
                SharesOutstanding = 2000
            };

            // Act
            initialData.UpdateContinuousData(updatedData);

            // Assert
            Assert.Equal(2000, initialData.SharesOutstanding);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "EquitySymbolData")]
        public void UpdateContinuousData_ShouldNotUpdateIfSharesOutstandingIsZero()
        {
            // Arrange
            var initialData = new EquitySymbolData
            {
                SharesOutstanding = 1000
            };

            var updatedData = new EquitySymbolData
            {
                SharesOutstanding = 0
            };

            // Act
            initialData.UpdateContinuousData(updatedData);

            // Assert
            Assert.Equal(1000, initialData.SharesOutstanding); 

        }
    }
}
