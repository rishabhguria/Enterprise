using Xunit;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.SecurityMasterBusinessObjects
{
    public class SecMasterRequestObjTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterRequestObj")]
        public void AddNewRow_AddsNewSymbolDataRow()
        {
            // Arrange
            var request = new SecMasterRequestObj();

            // Act
            request.AddNewRow();

            // Assert
            Assert.Single(request.SymbolDataRowCollection);
            Assert.IsType<SymbolDataRow>(request.SymbolDataRowCollection[0]);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterRequestObj")]
        public void AddData_AddsDataToNewRow_WithSymbol()
        {
            // Arrange
            var request = new SecMasterRequestObj();
            string symbol = "AAPL";
            ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;

            // Act
            request.AddData(symbol, symbology);

            // Assert
            Assert.Single(request.SymbolDataRowCollection);
            Assert.Equal(symbol, request.SymbolDataRowCollection[0].SymbolData[(int)symbology]);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterRequestObj")]
        public void AddData_AddsDataToNewRow_WithBBGID()
        {
            // Arrange
            var request = new SecMasterRequestObj();
            string bbgid = "BBG000B9XRY4";

            // Act
            request.AddData(bbgid);

            // Assert
            Assert.Single(request.SymbolDataRowCollection);
            Assert.Equal(bbgid, request.SymbolDataRowCollection[0].BBGID);
        }


        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterRequestObj")]
        public void CreateXml_GeneratesCorrectXml()
        {
            // Arrange
            var request = new SecMasterRequestObj();
            string symbol = "AAPL";
            ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
            request.AddData(symbol, symbology);

            // Act
            string xml = request.CreateXml();

            // Assert
            Assert.Contains("<SecMasterRequest>", xml);
            Assert.Contains("<TickerSymbol>AAPL</TickerSymbol>", xml);
        }


        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterRequestObj")]
        public void IsRequestValid_ReturnsTrue_WhenValid()
        {
            // Arrange
            var request = new SecMasterRequestObj();
            string symbol = "AAPL";
            ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
            request.AddData(symbol, symbology);

            // Act
            bool isValid = request.IsRequestValid();

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterRequestObj")]
        public void IsRequestValid_ReturnsFalse_WhenInvalid()
        {
            // Arrange
            var request = new SecMasterRequestObj();

            // Act
            bool isValid = request.IsRequestValid();

            // Assert
            Assert.False(isValid);
        }     
    }
}

