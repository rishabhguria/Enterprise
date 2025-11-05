using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using PU = Prana.Utilities.StringUtilities;
using Xunit.Abstractions;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.StringUtilities
{
    public class TripleDESEncryptDecryptTests
    {
        private readonly ITestOutputHelper outputHelper;
        private readonly string key;

        public TripleDESEncryptDecryptTests(ITestOutputHelper outputHelper)
        {
            // Arrange
            key = @"sblw-3hn8-sqoy19";

            this.outputHelper = outputHelper;
            outputHelper.WriteLine("Test case for ITestOutputHelper Started");
        }

        [Theory]
        [InlineData("hello", "UAyqARR3mf4=")]
        [InlineData("test", "zQSWvRmUlW0=")]
        [Trait("Prana.Utilities", "TripleDESEncryptDecrypt")]
        public void TripleDESEncryption_ValidInput_ReturnsEncryptedString(string input,string expectedEncrypted)
        {
            // Act
            string actualEncrypted = PU.TripleDESEncryptDecrypt.TripleDESEncryption(input, key);

            outputHelper.WriteLine($"Actual ({actualEncrypted}) === Expected ({expectedEncrypted})");
            // Assert
            Assert.Equal(expectedEncrypted, actualEncrypted);
        }

        [Theory]
        [InlineData("UAyqARR3mf4=", "hello")]
        [InlineData("zQSWvRmUlW0=", "test")]
        [Trait("Prana.Utilities", "TripleDESEncryptDecrypt")]
        public void TripleDESDecryption_ValidInput_ReturnsDecryptedString(string input,string expectedDecrypted)
        {
            //Act
            string actualDecrypted = PU.TripleDESEncryptDecrypt.TripleDESDecryption(input, key);

            outputHelper.WriteLine($"Actual ({actualDecrypted}) === Expected ({expectedDecrypted})");
            // Assert
            Assert.Equal(expectedDecrypted, actualDecrypted);
        }
    }
}
