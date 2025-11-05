using Prana.BusinessObjects;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.OptionGreeks
{
    public class StepAnalysisResponseTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "StepAnalysisResponse")]
        public void SetXParameters_UpdatesPrice_ForUnderlyingVolatilityInterestRatePriceCode()
        {
            // Arrange
            var inputParams = new InputParametersForGreeks
            {
                SimulatedUnderlyingStockPrice = 100,
                Volatility = 0.2,
                InterestRate = 0.05
            };
            var stepParam = new StepParameter(StepAnalParameterCode.UnderlyingPrice, 1, 0.9, 9.0);
            var response = new StepAnalysisResponse("user1", "SYM", "UNDERLYING", stepParam)
            {
                InputParameters = inputParams
            };

            // Act
            response.SetXParameters(10);
            // Assert
            Assert.Equal(110, response.InputParameters.SimulatedUnderlyingStockPrice, 2);
            Assert.Equal(0.20, response.InputParameters.Volatility, 2);
            Assert.Equal(0.05, response.InputParameters.InterestRate, 2);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "StepAnalysisResponse")]
        public void SetXParameters_UpdatesExpirationDate_ForDaysToExpirationCode()
        {
            // Arrange
            var initialDate = DateTime.Now;
            var inputParams = new InputParametersForGreeks { ExpirationDate = initialDate };
            var stepParam = new StepParameter(StepAnalParameterCode.DaysToExpiration,1,0.8,9.0);
            var response = new StepAnalysisResponse("user1", "SYM", "UNDERLYING", stepParam)
            {
                InputParameters = inputParams
            };

            // Act
            response.SetXParameters(30); // Move expiration back by 30 days

            // Assert
            Assert.Equal(initialDate.AddDays(-30), response.InputParameters.ExpirationDate);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "StepAnalysisResponse")]
        public void SetXParameters_HandlesExtremeValues_ForSpecialCases()
        {
            // Arrange
            var inputParams = new InputParametersForGreeks
            {
                SimulatedUnderlyingStockPrice = 200,
                Volatility = 0.25,
                InterestRate = 0.03
            };
            var stepParam = new StepParameter(StepAnalParameterCode.UnderlyingPrice,1,0.9,9.0);
            var response = new StepAnalysisResponse("user1", "SYM", "UNDERLYING", stepParam)
            {
                InputParameters = inputParams
            };

            // Act
            response.SetXParameters(-100); // Test the special case for -100

            // Assert
            Assert.Equal(0.000099999999999988987d * 200, response.InputParameters.SimulatedUnderlyingStockPrice, 5);
        }
    }
}
