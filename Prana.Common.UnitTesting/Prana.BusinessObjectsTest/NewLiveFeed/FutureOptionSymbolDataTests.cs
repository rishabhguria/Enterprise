using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.NewLiveFeed
{
    public class FutureOptionSymbolDataTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "FutureOptionSymbolData")]
        public void UpdateContinuousData_ShouldUpdateFieldsCorrectly()
        {
            // Arrange
            var originalData = new FutureOptionSymbolData
            {
                PutOrCall = OptionType.CALL,
                StrikePrice = 100.0,
                ExpirationDate = new DateTime(2025, 1, 1),
                DaysToExpiration = 10,
                OpenInterest = 5000.0,
                InterestRate = 1.5
            };

            var newData = new FutureOptionSymbolData
            {
                PutOrCall = OptionType.PUT,
                StrikePrice = 105.0,
                ExpirationDate = new DateTime(2025, 6, 1),
                DaysToExpiration = 30,
                OpenInterest = 6000.0,
                InterestRate = 2.0
            };

            // Act
            originalData.UpdateContinuousData(newData);

            // Assert
            Assert.Equal(OptionType.PUT, originalData.PutOrCall);
            Assert.Equal(105.0, originalData.StrikePrice);
            Assert.Equal(new DateTime(2025, 6, 1), originalData.ExpirationDate);
            Assert.Equal(30, originalData.DaysToExpiration);
            Assert.Equal(6000.0, originalData.OpenInterest);
            Assert.Equal(2.0, originalData.InterestRate);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FutureOptionSymbolData")]
        public void UpdateContinuousData_ShouldNotUpdateIfValuesAreDefaults()
        {
            // Arrange
            var originalData = new FutureOptionSymbolData
            {
                PutOrCall = OptionType.CALL,
                StrikePrice = 100.0,
                ExpirationDate = new DateTime(2025, 1, 1),
                DaysToExpiration = 10,
                OpenInterest = 5000.0,
                InterestRate = 1.5
            };

            var newData = new FutureOptionSymbolData
            {
                PutOrCall = OptionType.NONE,
                StrikePrice = 0.0,
                ExpirationDate = DateTimeConstants.MinValue,
                DaysToExpiration = 0,
                OpenInterest = 0.0,
                InterestRate = 0.0
            };

            // Act
            originalData.UpdateContinuousData(newData);

            // Assert
            Assert.Equal(OptionType.CALL, originalData.PutOrCall);
            Assert.Equal(100.0, originalData.StrikePrice);
            Assert.Equal(new DateTime(2025, 1, 1), originalData.ExpirationDate);
            Assert.Equal(10, originalData.DaysToExpiration);
            Assert.Equal(5000.0, originalData.OpenInterest);
            Assert.Equal(1.5, originalData.InterestRate);
        }
    }
}
