using Prana.Utilities.DateTimeUtilities;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.DateTimeUtilities
{
    public class CentralTimerTests
    {
        [Fact]
        [Trait("Prana.Utilities", "CentralTimer")]
        public void GetInstance_ReturnsSameInstance()
        {
            // Arrange
            var instance1 = CentralTimer.GetInstance();
            var instance2 = CentralTimer.GetInstance();

            // Act & Assert
            Assert.Same(instance1, instance2);
        }

        [Fact]
        [Trait("Prana.Utilities", "CentralTimer")]
        public void TimerInterval_SetTimerInterval_CheckTimerInterval()
        {
            // Arrange
            var centralTimer = CentralTimer.GetInstance();
            var newInterval = 500; // milliseconds

            // Act
            centralTimer.TimerInterval = newInterval;

            // Assert
            Assert.Equal(newInterval, centralTimer.TimerInterval);
        }
    }
}
