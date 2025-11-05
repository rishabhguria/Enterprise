using Prana.BusinessObjects;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.OptionGreeks
{
    public class VolatilitySkewRequestObjectTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilitySkewRequestObject")]
        public void GetDaysToExpiration_ValidFutureDate_ReturnsCorrectDays()
        {
            // Arrange
            var volSkewObject = new VolSkewObject();
            DateTime futureDate = DateTime.Now.AddDays(10);

            // Act
            int daysToExpiration = volSkewObject.GetDaysToExpiration(futureDate);

            // Assert
            Assert.Equal(11, daysToExpiration); 
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilitySkewRequestObject")]
        public void GetDaysToExpiration_PastDate_ReturnsZero()
        {
            // Arrange
            var volSkewObject = new VolSkewObject();
            DateTime pastDate = DateTime.Now.AddDays(-5);

            // Act
            int daysToExpiration = volSkewObject.GetDaysToExpiration(pastDate);

            // Assert
            Assert.Equal(0, daysToExpiration);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilitySkewRequestObject")]
        public void UpdateProxySymbolDictionary_AddsStepValueForUnderlyingPriceParameter()
        {
            // Arrange
            var volSkewObject = new VolSkewObject
            {
                ParameterCode = StepAnalParameterCode.UnderlyingPrice
            };
            string stepValue = "Step1";
            string proxySymbol = "Proxy1";

            // Act
            volSkewObject.UpdateProxySymbolDictionary(stepValue, proxySymbol, 0);

            // Assert
            Assert.True(volSkewObject.GetStepkeyValuesForProxySymbol(proxySymbol).Contains(stepValue));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "VolatilitySkewRequestObject")]
        public void UpdateProxySymbolDictionary_AddsExpirationDateForDaysToExpirationParameter()
        {
            // Arrange
            var volSkewObject = new VolSkewObject
            {
                ParameterCode = StepAnalParameterCode.DaysToExpiration
            };
            string stepValue = "Step1";
            string proxySymbol = "Proxy1";
            int proxyExpirationMonth = DateTime.Now.Month;
            DateTime expirationDate = DateTime.Now;

            volSkewObject.DictProxyExpirationDates.Add(stepValue, expirationDate);

            // Act
            volSkewObject.UpdateProxySymbolDictionary(stepValue, proxySymbol, proxyExpirationMonth);

            // Assert
            var result = volSkewObject.GetStepkeyValuesForProxySymbol(proxySymbol);
            Assert.NotNull(result);
            Assert.Contains(stepValue, result);
        }
    }
}
