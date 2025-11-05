using Prana.BusinessObjects;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.NewLiveFeed
{
    public class FutureSymbolDataTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "FutureSymbolData")]
        public void UpdateContinuousData_ShouldUpdateFieldsCorrectly()
        {
            // Arrange
            var originalData = new FutureSymbolData
            {
                ExpirationDate = new DateTime(2025, 1, 1),
                DaysToExpiration = 10,
                OpenInterest = 5000.0
            };

            var newData = new FutureSymbolData
            {
                ExpirationDate = new DateTime(2025, 6, 1),
                DaysToExpiration = 30,
                OpenInterest = 6000.0
            };

            // Act
            originalData.UpdateContinuousData(newData);

            // Assert
            Assert.Equal(new DateTime(2025, 6, 1), originalData.ExpirationDate);
            Assert.Equal(30, originalData.DaysToExpiration);
            Assert.Equal(6000.0, originalData.OpenInterest);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FutureSymbolData")]
        public void UpdateContinuousData_ShouldNotUpdateIfValuesAreDefaults()
        {
            //Arrange
            var originalData = new FutureSymbolData
            {
                ExpirationDate = new DateTime(2025, 1, 1),
                DaysToExpiration = 10,
                OpenInterest = 5000.0
            };


            var newData = new FutureSymbolData
            {
                ExpirationDate = DateTimeConstants.MinValue,
                DaysToExpiration = 0,
                OpenInterest = 0.0
            };

            //Act
            originalData.UpdateContinuousData(newData);

            //Assert
            Assert.Equal(new DateTime(2025, 1, 1), originalData.ExpirationDate);
            Assert.Equal(10, originalData.DaysToExpiration);
            Assert.Equal(5000.0, originalData.OpenInterest);
        }
    }
}
