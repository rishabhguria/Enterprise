using Moq;
using Prana.Utilities.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.IO
{
    public class InputStreamTests
    {
        [Fact]
        [Trait("Prana.Utilities", "InputStream")]
        public void Read_ReturnsByteFromStream()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            var inputStream = new InputStream(mockStream.Object);
            mockStream.Setup(s => s.ReadByte()).Returns(65); // ASCII code 'A'

            // Act
            var result = inputStream.Read();

            // Assert
            Assert.Equal(65, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "InputStream")]
        public void Read_PassedBytes_ReturnsBytesFromStream()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            var inputStream = new InputStream(mockStream.Object);
            var bytesToRead = new byte[] { 65, 66, 67 }; // 'A', 'B', 'C'
            mockStream.Setup(s => s.Read(It.IsAny<byte[]>(), 0, 3)).Returns(bytesToRead.Length)
                      .Callback((byte[] buffer, int offset, int count) => bytesToRead.CopyTo(buffer, offset));

            // Act
            var result = new byte[3];
            inputStream.Read(result);

            // Assert
            Assert.Equal(bytesToRead, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "InputStream")]
        public void Available_ReturnsRemainingBytesInStream()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.SetupGet(s => s.Length).Returns(10);
            mockStream.SetupGet(s => s.Position).Returns(5);
            var inputStream = new InputStream(mockStream.Object);

            // Act
            var result = inputStream.available();

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "InputStream")]
        public void GetBaseStream_ReturnsBaseStream()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            var inputStream = new InputStream(mockStream.Object);

            // Act
            var result = inputStream.getBaseStream();

            // Assert
            Assert.Equal(mockStream.Object, result);
        }
    }
}
