using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PostTrade
{
    public class TaxLotTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "TaxLot")]
        public void SetAndCalculateValues_SetsExpectedValues()
        {
            // Arrange
            TaxLot taxLot = new TaxLot();
           
            var level1 = new AllocationLevelClass("Group1")
            {
                LevelnID = 1,
                GroupID = "G1",
                Percentage = 50.0f
            };

            var level2 = new AllocationLevelClass("Group2")
            {
                LevelnID = 2,
                GroupID = "G2",
                Percentage = 20.0f,
                AllocatedQty = 100
            };

            float expectedPercentage;
            if (level2.Percentage == 0)
            {
                expectedPercentage = level1.Percentage;
            }
            else
            {
                expectedPercentage = level2.Percentage * level1.Percentage / 100;
            }

            // Act
            taxLot.SetAndCalculateValues(level1, level2, isFractionalAllowed: true);

            // Assert
            Assert.Equal(expectedPercentage, taxLot.Percentage);
            Assert.Equal(level2.AllocatedQty, taxLot.ExecutedQty);
            Assert.Equal(level1.GroupID, taxLot.GroupID);
        }
    }
}
