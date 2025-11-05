using System;
using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.FixClasses
{
    public class FixPartyDetailsTests
    {
        private readonly FixPartyDetails _fixPartyDetails;
        public FixPartyDetailsTests()
        {
            _fixPartyDetails = new FixPartyDetails(
                partyID: 1,
                partyName: "TestParty",
                SenderCompID: "123",
                TargetCompID: "456",
                Port: 8080,
                targetSubID: "ID123",
                hostName: "localhost",
                originatorType: 1,
                brokerConnectionType: 1,
                fixDllAdapterName: "TestDll",
                resetTime: DateTime.Now
            );
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FixPartyDetails")]
        public void GetDestinationAndDeliverToCompID_ShouldReturnCorrectValues()
        {
            // Arrange
            var assetDictionary = _fixPartyDetails.GetAssetDictionary("TestAsset");
            assetDictionary.Add("Venue123", "Destination1,DeliverToCompID1");

            // Act
            _fixPartyDetails.GetDestinationAndDeliverToCompID("Venue123", "TestAsset", out string deliverToCompID, out string exDestination);

            // Assert
            Assert.Equal("DeliverToCompID1", deliverToCompID);
            Assert.Equal("Destination1", exDestination);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FixPartyDetails")]
        public void GetDeliverToCompID_ShouldReturnCorrectDeliverToCompID()
        {
            // Arrange
            var assetDictionary = _fixPartyDetails.GetAssetDictionary("TestAsset");
            assetDictionary.Add("Venue123", "Destination1,DeliverToCompID1");

            // Act
            string result = _fixPartyDetails.GetDeliverToCompID("Venue123", "TestAsset");

            // Assert
            Assert.Equal("DeliverToCompID1", result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FixPartyDetails")]
        public void GetDeliverToCompID_ShouldReturnEmptyStringIfVenueIDNotFound()
        {
            // Act
            string result = _fixPartyDetails.GetDeliverToCompID("Venue123", "TestAsset");

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}
