using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class CompressionHelperTests
    {
        [Fact]
        [Trait("Prana.Utilities", "CompressionHelper")]
        public void ZipAndUnZip_ReturnsOriginalString_WhenGivenNormalString()
        {
            // Arrange
            string originalText = "This is a test string to be compressed and then decompressed.";
            byte[] byteArray = null; // This parameter is not used in the current implementation

            // Act
            string compressedText = CompressionHelper.Zip(originalText, byteArray);
            byte[] gzBuffer = null; // Adjusted to fit the UnZip method signature
            string decompressedText = CompressionHelper.UnZip(compressedText, gzBuffer);

            // Assert
            Assert.Equal(originalText, decompressedText);
        }

        [Fact]
        [Trait("Prana.Utilities", "CompressionHelper")]
        public void Zip_ReturnsNonNull_WhenGivenEmptyString()
        {
            // Arrange
            string originalText = "";
            byte[] byteArray = null;

            // Act
            string compressedText = CompressionHelper.Zip(originalText, byteArray);

            // Assert
            Assert.NotNull(compressedText);
        }

        [Fact]
        [Trait("Prana.Utilities", "CompressionHelper")]
        public void UnZip_ReturnsEmptyString_WhenGivenCompressedEmptyString()
        {
            // Arrange
            string originalText = "";
            byte[] byteArray = null;
            string compressedText = CompressionHelper.Zip(originalText, byteArray);

            // Act
            byte[] gzBuffer = null;
            string decompressedText = CompressionHelper.UnZip(compressedText, gzBuffer);

            // Assert
            Assert.Equal(originalText, decompressedText);
        }

        [Fact]
        [Trait("Prana.Utilities", "CompressionHelper")]
        public void ZipAndUnZip_ReturnsOriginalString_WhenGivenLargeString()
        {
            // Arrange
            string originalText = new string('a', 5000); // Creating a large string
            byte[] byteArray = null;

            // Act
            string compressedText = CompressionHelper.Zip(originalText, byteArray);
            byte[] gzBuffer = null;
            string decompressedText = CompressionHelper.UnZip(compressedText, gzBuffer);

            // Assert
            Assert.Equal(originalText, decompressedText);
        }
    }
}
