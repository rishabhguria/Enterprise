using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.Analytics
{
    public class PranaRequestCarrierTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "PranaRequestCarrier")]
        public void CreateRequest_BetaRequest_True_ShouldAddRiskObjectsToIndividualAndGroupLists()
        {
            // Arrange
            var carrier = new PranaRequestCarrier();
            var riskObjCollection = new PranaRiskObjColl
            {
                new PranaRiskObj { UnderlyingSymbol = "AAPL", Symbol = "AAPL", PositionType = "LONG" },
                new PranaRiskObj { UnderlyingSymbol = "MSFT", Symbol = "MSFT", PositionType = "SHORT" }
            };

            var riskCalculationBasedOn = RiskConstants.RiskCalculationBasedOn.Quantity;
            bool isBetaRequest = true;

            // Act
            carrier.CreateRequest(riskObjCollection, riskCalculationBasedOn, isBetaRequest);

            // Assert
            Assert.Equal(2, carrier.IndividualSymbolList.Count);
            Assert.Equal(2, carrier.GroupSymbolList.Count);
            Assert.Contains("AAPL", carrier.IndividualSymbolList.Keys);
            Assert.Contains("MSFT", carrier.IndividualSymbolList.Keys);
            Assert.Contains("AAPL", carrier.GroupSymbolList.Keys);
            Assert.Contains("MSFT", carrier.GroupSymbolList.Keys);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PranaRequestCarrier")]
        public void CreateRequest_BetaRequest_False_ShouldAddRiskObjectsWithPositionTypeToIndividualAndGroupLists()
        {
            // Arrange
            var carrier = new PranaRequestCarrier();
            var riskObjCollection = new PranaRiskObjColl
            {
                new PranaRiskObj { UnderlyingSymbol = "AAPL", Symbol = "AAPL", PositionType = "LONG" },
                new PranaRiskObj { UnderlyingSymbol = "MSFT", Symbol = "MSFT", PositionType = "SHORT" }
            };

            var riskCalculationBasedOn = RiskConstants.RiskCalculationBasedOn.Quantity;
            bool isBetaRequest = false;

            // Act
            carrier.CreateRequest(riskObjCollection, riskCalculationBasedOn, isBetaRequest);

            // Assert
            string expectedKeyForAAPL = "AAPLLONG";
            string expectedKeyForMSFT = "MSFTSHORT";
            Assert.Equal(2, carrier.IndividualSymbolList.Count);
            Assert.Equal(2, carrier.GroupSymbolList.Count);
            Assert.Contains(expectedKeyForAAPL, carrier.IndividualSymbolList.Keys);
            Assert.Contains(expectedKeyForMSFT, carrier.IndividualSymbolList.Keys);
            Assert.Contains("AAPL", carrier.GroupSymbolList.Keys);
            Assert.Contains("MSFT", carrier.GroupSymbolList.Keys);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PranaRequestCarrier")]
        public void CreateRequest_DuplicateSymbols_ShouldAggregateRiskObjects()
        {
            // Arrange
            var carrier = new PranaRequestCarrier();
            var riskObjCollection = new PranaRiskObjColl
            {
                new PranaRiskObj { UnderlyingSymbol = "AAPL", Symbol = "AAPL", PositionType = "LONG" },
                new PranaRiskObj { UnderlyingSymbol = "AAPL", Symbol = "AAPL", PositionType = "SHORT" }
            };

            var riskCalculationBasedOn = RiskConstants.RiskCalculationBasedOn.Quantity;
            bool isBetaRequest = false;

            // Act
            carrier.CreateRequest(riskObjCollection, riskCalculationBasedOn, isBetaRequest);

            // Assert
            string expectedKeyForAAPL = "AAPLLONG";
            string expectedKeyForAAPLShort = "AAPLSHORT";
            Assert.Equal(2, carrier.IndividualSymbolList.Count);
            Assert.Contains(expectedKeyForAAPL, carrier.IndividualSymbolList.Keys);
            Assert.Contains(expectedKeyForAAPLShort, carrier.IndividualSymbolList.Keys);
            Assert.Single(carrier.GroupSymbolList);
            Assert.Contains("AAPL", carrier.GroupSymbolList.Keys);
        }
    }
}
