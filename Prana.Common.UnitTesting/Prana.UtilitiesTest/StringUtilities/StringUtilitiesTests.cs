using PU = Prana.Utilities.StringUtilities;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.StringUtilities
{
    public class StringUtilitiesTests
    {
        [Theory]
        [InlineData("Net Amount(Local)", "Net Amount( Local)")]
        [InlineData("  !#TEST", "!# T E S T")]
        [InlineData(" itestTEST", "itest T E S T")]
        [InlineData("2_Test_test  ", "2_ Test_test")]
        [InlineData("allocationUi", "allocation Ui")]
        [Trait("Prana.Utilities", "StringUtilities")]
        public void SplitCamelCase_ReturnsCorrectString(string inputstring, string expectedstring)
        {
            string actualsting = PU.StringUtilities.SplitCamelCase(inputstring);
            Assert.Equal(expectedstring, actualsting);
        }
    }
}
