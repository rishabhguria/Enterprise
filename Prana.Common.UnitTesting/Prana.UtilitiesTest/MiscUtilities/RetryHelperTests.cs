using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class RetryHelperTests
    {
        [Fact]
        [Trait("Prana.Utilities", "RetryHelper")]
        public void Do_WithActionThatSucceeds_DoesNotThrow()
        {
            // Arrange
            int attempts = 0;
            Action action = () => { attempts++; };

            // Act
            RetryHelper.Do(action, TimeSpan.FromMilliseconds(10), 3);

            // Assert
            Assert.Equal(1, attempts);
        }

        [Fact]
        [Trait("Prana.Utilities", "RetryHelper")]
        public void Do_WithActionThatFails_ThrowsAggregateException()
        {
            // Arrange
            int attempts = 0;
            Action action = () =>
            {
                attempts++;
                throw new InvalidOperationException();
            };

            // Act
            var exception = Assert.Throws<AggregateException>(() => RetryHelper.Do(action, TimeSpan.FromMilliseconds(10), 3));

            // Assert
            Assert.Equal(3, attempts);
            Assert.Equal(3, exception.InnerExceptions.Count);
            Assert.IsType<InvalidOperationException>(exception.InnerExceptions[0]);
        }

        [Fact]
        [Trait("Prana.Utilities", "RetryHelper")]
        public void Do_WithFuncThatSucceeds_ReturnsExpectedResult()
        {
            // Arrange
            int attempts = 0;
            Func<int> func = () =>
            {
                attempts++;
                return 42;
            };

            // Act
            int result = RetryHelper.Do(func, TimeSpan.FromMilliseconds(10), 3);

            // Assert
            Assert.Equal(1, attempts);
            Assert.Equal(42, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "RetryHelper")]
        public void Do_WithFuncThatFails_ThrowsAggregateException()
        {
            // Arrange
            int attempts = 0;
            Func<int> func = () =>
            {
                attempts++;
                throw new InvalidOperationException();
            };

            // Act
            var exception = Assert.Throws<AggregateException>(() => RetryHelper.Do(func, TimeSpan.FromMilliseconds(10), 3));

            // Assert
            Assert.Equal(3, attempts);
            Assert.Equal(3, exception.InnerExceptions.Count);
            Assert.IsType<InvalidOperationException>(exception.InnerExceptions[0]);
        }

        [Fact]
        [Trait("Prana.Utilities", "RetryHelper")]
        public void Do_WithFuncAndArgumentThatSucceeds_ReturnsExpectedResult()
        {
            // Arrange
            int attempts = 0;
            Func<int, int> func = (input) =>
            {
                attempts++;
                return input * 2;
            };

            // Act
            int result = RetryHelper.Do(func, TimeSpan.FromMilliseconds(10), 42, 3);

            // Assert
            Assert.Equal(1, attempts);
            Assert.Equal(84, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "RetryHelper")]
        public void Do_WithFuncAndArgumentThatFails_ThrowsAggregateException()
        {
            // Arrange
            int attempts = 0;
            Func<int, int> func = (input) =>
            {
                attempts++;
                throw new InvalidOperationException();
            };

            // Act
            var exception = Assert.Throws<AggregateException>(() => RetryHelper.Do(func, TimeSpan.FromMilliseconds(10), 42, 3));

            // Assert
            Assert.Equal(3, attempts);
            Assert.Equal(3, exception.InnerExceptions.Count);
            Assert.IsType<InvalidOperationException>(exception.InnerExceptions[0]);
        }
    }
}
