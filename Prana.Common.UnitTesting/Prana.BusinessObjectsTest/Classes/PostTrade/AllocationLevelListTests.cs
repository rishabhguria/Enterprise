using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.PostTrade
{
    public class AllocationLevelListTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationLevelList")]
        public void GetSumOfPercentageLevel1_ReturnsCorrectSum()
        {
           // Arrange
            var allocationLevelList = new AllocationLevelList();

            AllocationLevelClass AllocationLevelClass1 = new AllocationLevelClass("") { Percentage = 25.0f };
            AllocationLevelClass AllocationLevelClass2 = new AllocationLevelClass("") { Percentage = 25.0f };
           
            float expectedSum = 50.0f;
            allocationLevelList.Add(AllocationLevelClass1);
            allocationLevelList.Add(AllocationLevelClass2);

            // Act
            float result = allocationLevelList.GetSumOfPercentageLevel1();

            // Assert
            Assert.Equal(expectedSum, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationLevelList")]
        public void GetSumOfPercentageLevel1_ReturnsZero_WhenCollectionIsEmpty()
        {
            // Arrange
            var allocationLevelList = new AllocationLevelList();
            float expectedSum = 0.0f;

            // Act
            float result = allocationLevelList.GetSumOfPercentageLevel1();

            // Assert
            Assert.Equal(expectedSum, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationLevelList")]
        public void CheckSumOfPercentageLevel2_ReturnsLevelId_WhenChildListIsNotEmpty()
        {
            // Arrange
            var allocationLevelList = new AllocationLevelList();
            
            AllocationLevelClass AllocationLevel1 = new AllocationLevelClass("") ;
            AllocationLevel1.LevelnID = 1;

            var allocationLevelChild = new AllocationLevelClass("");
            allocationLevelChild.Percentage = 90;

            AllocationLevel1.AddChilds(allocationLevelChild);
            
            allocationLevelList.Add(AllocationLevel1);
            int expectedLevelnID = 1;

            // Act
            int result = allocationLevelList.CheckSumOfPercentageLevel2();

            // Assert
            Assert.Equal(expectedLevelnID, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationLevelList")]
        public void CheckSumOfPercentageLevel2_ReturnsZero_WhenSumIs100()
        {
            // Arrange
            var allocationLevelList = new AllocationLevelList();

            AllocationLevelClass AllocationLevel1 = new AllocationLevelClass("");
            AllocationLevel1.LevelnID = 1;

            var allocationLevelChild1 = new AllocationLevelClass("");
            allocationLevelChild1.Percentage = 90;

            var allocationLevelChild2 = new AllocationLevelClass("");
            allocationLevelChild2.Percentage = 10;


            AllocationLevel1.AddChilds(allocationLevelChild1);
            AllocationLevel1.AddChilds(allocationLevelChild2 );

            allocationLevelList.Add(AllocationLevel1);
            int expectedLevelnID = 0;

            // Act
            int result = allocationLevelList.CheckSumOfPercentageLevel2();

            // Assert
            Assert.Equal(expectedLevelnID, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "AllocationLevelList")]
        public void CheckSumOfPercentageLevel2_ReturnsZero_WhenChildListIsEmpty()
        {
            // Arrange
            var allocationLevelList = new AllocationLevelList();

            AllocationLevelClass AllocationLevel1 = new AllocationLevelClass("");
            AllocationLevel1.LevelnID = 1;
            
            allocationLevelList.Add(AllocationLevel1);
            int expectedLevelnID = 0;

            // Act
            int result = allocationLevelList.CheckSumOfPercentageLevel2();

            // Assert
            Assert.Equal(expectedLevelnID, result);
        }
    }
}
