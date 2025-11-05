using Moq;
using Prana.Utilities.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.IO
{
    public class DataInputStreamTests
    {
        [Fact]
        [Trait("Prana.Utilities", "DataInputStream")]
        public void Read__PassedByte_ReturnCorrectByte()
        {
            // Arrange
            var mockStream = new Mock<IInputStream>();
            var inputStream = new MemoryStream(new byte[] { 0x41 }); // 'A' in ASCII
            mockStream.Setup(x => x.getBaseStream()).Returns(inputStream);

            var dataInputStream = new DataInputStream(mockStream.Object);

            //Act
            int result = dataInputStream.Read();

            // Assert
            Assert.Equal(65, result); // ASCII value of 'A'
        }

        [Fact]
        [Trait("Prana.Utilities", "DataInputStream")]
        public void Read_PassedBytes_ReturnCorrectBytesAndSize()
        {
            // Arrange
            var mockStream = new Mock<IInputStream>();
            var inputBytes = new byte[] { 0x41, 0x42, 0x43 }; // 'ABC' in ASCII
            var inputStream = new MemoryStream(inputBytes);
            mockStream.Setup(x => x.getBaseStream()).Returns(inputStream);

            var dataInputStream = new DataInputStream(mockStream.Object);
            var buffer = new byte[3];

            // Act
            int bytesRead = dataInputStream.Read(buffer);

            // Assert
            // Checking Bytes Size
            Assert.Equal(3, bytesRead);
            // Checking Buffer with filled items
            Assert.Equal(new byte[] { 0x41, 0x42, 0x43 }, buffer);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataInputStream")]
        public void Read_PassedBytesWithOffsetAndLength_ReturnCorrectBytes()
        {
            // Arrange
            var mockStream = new Mock<IInputStream>();
            var inputBytes = new byte[] { 0x41, 0x42, 0x43 }; // 'ABC' in ASCII
            var inputStream = new MemoryStream(inputBytes);
            mockStream.Setup(x => x.getBaseStream()).Returns(inputStream);

            var dataInputStream = new DataInputStream(mockStream.Object);
            var buffer = new byte[3];

            // Act
            int bytesRead = dataInputStream.Read(buffer, 0, 3);

            // Assert
            // Checking Bytes Size
            Assert.Equal(3, bytesRead);
            // Checking Buffer with filled items
            Assert.Equal(new byte[] { 0x41, 0x42, 0x43 }, buffer);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataInputStream")]
        public void Close_AfterCloseTryingToRead_ReturnException()
        {
            // Arrange
            var mockStream = new Mock<IInputStream>();
            var inputStream = new MemoryStream();
            mockStream.Setup(x => x.getBaseStream()).Returns(inputStream);

            var dataInputStream = new DataInputStream(mockStream.Object);

            // Act
            dataInputStream.Close();

            // Assert
            // Reading after closing should throw ObjectDisposedException
            Assert.Throws<ObjectDisposedException>(() => dataInputStream.Read()); 
        }

        [Fact]
        [Trait("Prana.Utilities", "DataInputStream")]
        public void ReadInt_PassedBytes_ReturnCorrectValue()
        {
            // Arrange
            var mockStream = new Mock<IInputStream>();
            var inputStream = new MemoryStream(new byte[] { 0x01, 0x02, 0x03, 0x04 }); // 67305985
            mockStream.Setup(x => x.getBaseStream()).Returns(inputStream);

            var dataInputStream = new DataInputStream(mockStream.Object);

            // Act
            int result = dataInputStream.readInt();

            // Assert
            Assert.Equal(67305985, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataInputStream")]
        public void ReadLong_PassedBytes_ReturnCorrectValue()
        {
            // Arrange
            var mockStream = new Mock<IInputStream>();
            var inputStream = new MemoryStream(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 }); // Little-endian 578437695752307201
            mockStream.Setup(x => x.getBaseStream()).Returns(inputStream);

            var dataInputStream = new DataInputStream(mockStream.Object);

            // Act
            long result = dataInputStream.readLong();

            // Assert
            Assert.Equal(578437695752307201, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataInputStream")]
        public void Available_PassedBytes_ReturnCorrectRemainingBytes()
        {
            // Arrange
            var mockStream = new Mock<IInputStream>();
            var inputStream = new MemoryStream(new byte[10]); // Assuming the stream has 10 bytes
            mockStream.Setup(x => x.getBaseStream()).Returns(inputStream);
            inputStream.Position = 5; // Setting the position to 5 bytes

            var dataInputStream = new DataInputStream(mockStream.Object);

            // Act
            long result = dataInputStream.available();

            // Assert
            // 10 bytes total, 5 bytes already read
            Assert.Equal(5, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataInputStream")]
        public void GetBaseStream_ReturnCorrectStream()
        {
            // Arrange
            var mockStream = new Mock<IInputStream>();
            var inputStream = new MemoryStream();
            mockStream.Setup(x => x.getBaseStream()).Returns(inputStream);

            var dataInputStream = new DataInputStream(mockStream.Object);

            // Act
            Stream result = dataInputStream.getBaseStream();

            // Assert
            Assert.Same(inputStream, result);
        }
    }
}
