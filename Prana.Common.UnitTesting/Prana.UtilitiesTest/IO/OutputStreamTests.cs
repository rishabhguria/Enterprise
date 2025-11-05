using Moq;
using Prana.Utilities.IO;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.IO
{
    public class OutputStreamTests
    {
        [Fact]
        [Trait("Prana.Utilities", "OutputStream")]
        public void Write_PassedInt_DontThowsExeception()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            var outputStream = new OutputStream(mockStream.Object);
            try
            {
                // Act
                outputStream.Write(65); // Writing the ASCII value of 'A'
                
                // Assert
                Assert.True(true);
            }
            catch (Exception)
            {
                // Assert
                Assert.Fail();
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "OutputStream")]
        public void Write_PassedBytes_DontThowsExeception()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            var outputStream = new OutputStream(mockStream.Object);
            byte[] bytes = { 65, 66, 67 }; // 'A', 'B', 'C'

            try
            {
                // Act
                outputStream.Write(bytes);

                // Assert
                Assert.True(true);
            }
            catch (Exception)
            {
                // Assert
                Assert.Fail();
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "OutputStream")]
        public void Write_PassedBytesWithOffsetAndLength_DontThowsExeception()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            var outputStream = new OutputStream(mockStream.Object);
            byte[] bytes = { 65, 66, 67 }; // 'A', 'B', 'C'

            try
            {
                // Act
                outputStream.Write(bytes, 1, 2); // 'B', 'C'

                // Assert
                Assert.True(true);
            }
            catch (Exception)
            {
                // Assert
                Assert.Fail();
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "OutputStream")]
        public void Flush_CallsFlush_DontThowsExeception()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            var outputStream = new OutputStream(mockStream.Object);

            try
            {
                // Act
                outputStream.Flush();

                // Assert
                Assert.True(true);
            }
            catch (Exception)
            {
                // Assert
                Assert.Fail();
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "OutputStream")]
        public void getBaseStream_ReturnsCorrectStream()
        {
            // Arrange
            var expectedStream = new MemoryStream();
            var outputStream = new OutputStream(expectedStream);

            // Act
            var result = outputStream.getBaseStream();

            // Assert
            Assert.Equal(expectedStream, result);
        }
    }
}
