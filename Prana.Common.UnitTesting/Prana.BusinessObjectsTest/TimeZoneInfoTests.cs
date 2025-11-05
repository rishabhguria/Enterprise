using Prana.BusinessObjects;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest
{
    public class TimeZoneInfoTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZoneByString_ValidDaylightName_ReturnsTimeZone()
        {
            // Arrange
            string validDaylightName = "Pacific Daylight Time";

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZoneByString(validDaylightName);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZoneByString_ValidDisplayName_ReturnsTimeZone()
        {
            // Arrange
            string validDisplayName = "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi";

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZoneByString(validDisplayName);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZoneByString_ValidStandardName_ReturnsTimeZone()
        {
            // Arrange
            string validStandardName = "Pacific Standard Time";

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZoneByString(validStandardName);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZoneByString_InvalidTimeZoneName_ReturnsNull()
        {
            // Arrange
            string invalidTimeZoneName = "Invalid Time Zone";

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZoneByString(invalidTimeZoneName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZoneByString_EmptyString_ReturnsNull()
        {
            // Arrange
            string emptyTimeZoneName = "";

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZoneByString(emptyTimeZoneName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZoneByString_NullInput_ReturnsNull()
        {
            // Arrange
            string nullTimeZoneName = null;

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZoneByString(nullTimeZoneName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZone_ValidDisplayName_ReturnsCorrectTimeZone()
        {
            // Arrange
            string validTimeZoneName = "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi";

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZone(validTimeZoneName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(validTimeZoneName, result.DisplayName);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZone_InvalidDisplayName_ReturnsNull()
        {
            // Arrange
            string invalidTimeZoneName = "Invalid Display Name";

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZone(invalidTimeZoneName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZone_EmptyString_ReturnsNull()
        {
            // Arrange
            string emptyTimeZoneName = "";

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZone(emptyTimeZoneName);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void ConvertLocalTimeToUTC_NoDaylightSavings_ReturnsCorrectUtcTime()
        {
            // Arrange
            System.DateTime localTime = new System.DateTime(2024, 1, 1, 1, 0, 0); // A date during standard time
            TimeZone zone = TimeZoneInfo.EasternTimeZone; // UTC-5

            // Act
            System.DateTime utcTime = TimeZoneInfo.ConvertLocalTimeToUTC(localTime, zone);

            // Assert
            System.DateTime expectedUtcTime = new System.DateTime(2024, 1, 1, 6, 0, 0); // UTC time is 5 hours ahead of local time
            Assert.Equal(expectedUtcTime, utcTime);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void ConvertLocalTimeToUTC_DaylightSavingsTime_ReturnsCorrectUtcTime()
        {
            // Arrange
            System.DateTime localTime = new System.DateTime(2024, 7, 1, 12, 0, 0); // A date during daylight savings
            TimeZone zone = TimeZoneInfo.FindTimeZoneByDayLightName("Pacific Daylight Time"); // UTC-8

            // Act
            System.DateTime utcTime = TimeZoneInfo.ConvertLocalTimeToUTC(localTime, zone);

            // Assert
            // Since the logic subtracts the daylight bias, the result will be ahead by 7 hours (1 hour is daylight bias  for PDT)
            System.DateTime expectedUtcTime = new System.DateTime(2024, 7, 1, 19, 0, 0); // UTC equivalent of PDT
            Assert.Equal(expectedUtcTime, utcTime);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void ConvertUtcTimeToLocalTime_StandardTime_ReturnsCorrectLocalTime()
        {
            // Arrange
            System.DateTime utcTime = new System.DateTime(2024, 1, 1, 17, 0, 0); // 5:00 PM UTC
            TimeZone zone = TimeZoneInfo.EasternTimeZone;

            // Act
            System.DateTime localTime = TimeZoneInfo.ConvertUtcTimeToLocalTime(utcTime, zone);

            // Assert
            System.DateTime expectedLocalTime = new System.DateTime(2024, 1, 1, 12, 0, 0); // 12:00 PM EST
            Assert.Equal(expectedLocalTime, localTime);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void ConvertUtcTimeToLocalTime_DaylightSavings_ReturnsCorrectLocalTime()
        {
            // Arrange
            System.DateTime utcTime = new System.DateTime(2024, 7, 1, 19, 0, 0);
            TimeZone zone = TimeZoneInfo.FindTimeZoneByDayLightName("Pacific Daylight Time");

            // Mock or simulate that it's daylight savings time
            System.TimeSpan baseOffset = zone.Bias + zone.DaylightBias;

            // Act
            System.DateTime localTime = TimeZoneInfo.ConvertUtcTimeToLocalTime(utcTime, zone);

            // Assert
            System.DateTime expectedLocalTime = new System.DateTime(2024, 7, 1, 12, 0, 0); // 12:00 PM EDT (UTC-4)
            Assert.Equal(expectedLocalTime, localTime);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void ConvertUtcTimeToLocalTime_NullTimeZone_ThrowsNullReferenceException()
        {
            // Arrange
            System.DateTime utcTime = new System.DateTime(2024, 1, 1, 17, 0, 0);
            TimeZone invalidZone = null;

            // Act & Assert
            Assert.Throws<System.NullReferenceException>(() => TimeZoneInfo.ConvertUtcTimeToLocalTime(utcTime, invalidZone));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void ConvertUtcTimeToLocalTime_FutureUtcTime_ReturnsCorrectLocalTime()
        {
            // Arrange
            System.DateTime utcTime = new System.DateTime(2030, 6, 15, 12, 0, 0); // 12:00 PM UTC
            TimeZone zone = TimeZoneInfo.EasternTimeZone;

            // Act
            System.DateTime localTime = TimeZoneInfo.ConvertUtcTimeToLocalTime(utcTime, zone);

            // Assert
            System.DateTime expectedLocalTime = new System.DateTime(2030, 6, 15, 8, 0, 0); // 8:00 AM EDT (UTC-4)
            Assert.Equal(expectedLocalTime, localTime);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void GetBaseOffset_StandardTime_ReturnsBias()
        {
            // Arrange
            System.DateTime standardTime = new System.DateTime(2024, 1, 1, 12, 0, 0);
            TimeZone zone = TimeZoneInfo.EasternTimeZone;

            // Act
            System.TimeSpan baseOffset = TimeZoneInfo.GetBaseOffset(standardTime, zone);

            // Assert
            Assert.Equal(System.TimeSpan.FromHours(5), baseOffset); // Standard time bias only
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void GetBaseOffset_DaylightSavingsTime_ReturnsBiasPlusDaylightBias()
        {
            // Arrange
            System.DateTime daylightTime = new System.DateTime(2024, 7, 1, 12, 0, 0);
            TimeZone zone = TimeZoneInfo.FindTimeZoneByDayLightName("Pacific Daylight Time");

            // Act
            System.TimeSpan baseOffset = TimeZoneInfo.GetBaseOffset(daylightTime, zone);

            // Assert
            Assert.Equal(System.TimeSpan.FromHours(7), baseOffset); // UTC-6 due to daylight savings
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZoneByStandardName_ValidStandardName_ReturnsTimeZone()
        {
            // Arrange
            string timeZoneName = "Eastern Standard Time";

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZoneByStandardName(timeZoneName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Eastern Standard Time", result.StandardName);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZoneByStandardName_InvalidStandardName_ReturnsNull()
        {
            // Arrange
            string timeZoneName = "Invalid TimeZone Name";

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZoneByStandardName(timeZoneName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZoneByStandardName_EmptyStringInput_ReturnsNull()
        {
            // Arrange
            string timeZoneName = string.Empty;

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZoneByStandardName(timeZoneName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZoneByDayLightName_ValidDaylightName_ReturnsTimeZone()
        {
            // Arrange
            string timeZoneName = "Eastern Daylight Time";

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZoneByDayLightName(timeZoneName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Eastern Daylight Time", result.DaylightName);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZoneByDayLightName_InvalidDaylightName_ReturnsNull()
        {
            // Arrange
            string timeZoneName = "Invalid Daylight Time";

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZoneByDayLightName(timeZoneName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "TimeZoneInfo")]
        public void FindTimeZoneByDayLightName_EmptyStringInput_ReturnsNull()
        {
            // Arrange
            string timeZoneName = string.Empty;

            // Act
            TimeZone result = TimeZoneInfo.FindTimeZoneByDayLightName(timeZoneName);

            // Assert
            Assert.Null(result);
        }
    }
}

