using Prana.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest
{
    public class ExtensionMethodTests
    {
        [Theory]
        [InlineData(1.000001)]
        [InlineData(01001.000001)]
        [InlineData(999.000000001)]
        [Trait("Prana.Utilities", "ExtensionMethod")]
        public void ToDecimal_ReturnsDecimalValue(double value)
        {
            //Act
            var result = value.ToDecimal();

            //Assert
            Assert.True(result.GetType() == typeof(Decimal));
        }

        [Fact]
        [Trait("Prana.Utilities", "ExtensionMethod")]
        public void ToDecimal_ReturnsException()
        {
            // Arrange
            double? value = null;

            //Act
            Action act = () => value.ToDecimal();

            //Assert
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
