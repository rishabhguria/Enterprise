using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class TimeFormatterTests
    {
        [Fact]
        [Trait("Prana.Utilities", "TimeFormatter")]
        public void Format_WithSecondsLessThanZero_ReturnsEmptyString()
        {
            // Arrange
            int seconds = -1;

            // Act
            var result = TimeFormatter.Format(seconds);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "TimeFormatter")]
        public void Format_WithOnlySeconds_ReturnsCorrectFormat()
        {
            // Arrange
            int seconds = 45; // Less than a minute

            // Act
            var result = TimeFormatter.Format(seconds);

            // Assert
            Assert.Equal("45 Seconds", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "TimeFormatter")]
        public void Format_WithMinutesAndSeconds_ReturnsCorrectFormat()
        {
            // Arrange
            int seconds = 150; // 2 minutes and 30 seconds

            // Act
            var result = TimeFormatter.Format(seconds);

            // Assert
            Assert.Equal("2 Minutes 30 Seconds", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "TimeFormatter")]
        public void Format_WithHoursMinutesAndSeconds_ReturnsCorrectFormat()
        {
            // Arrange
            int seconds = 3665; // 1 hour, 1 minute, and 5 seconds

            // Act
            var result = TimeFormatter.Format(seconds);

            // Assert
            Assert.Equal("1 Hours 1 Minutes 5 Seconds", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "TimeFormatter")]
        public void Format_WithDaysHoursMinutesAndSeconds_ReturnsCorrectFormat()
        {
            // Arrange
            int seconds = 90065; // 1 day, 1 hour, 1 minute, and 5 seconds

            // Act
            var result = TimeFormatter.Format(seconds);

            // Assert
            Assert.Equal("1 Days 1 Hours 1 Minutes 5 Seconds", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "TimeFormatter")]
        public void Format_WithZeroSeconds_ReturnsEmptyString()
        {
            // Arrange
            int seconds = 0;

            // Act
            var result = TimeFormatter.Format(seconds);

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}
