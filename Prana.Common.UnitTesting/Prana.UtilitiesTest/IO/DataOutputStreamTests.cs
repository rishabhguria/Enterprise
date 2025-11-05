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
    public class DataOutputStreamTests
    {
        private readonly ITestOutputHelper outputHelper;
        public DataOutputStreamTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;

            //To check or write in test explorer you can use below line
            //outputHelper.WriteLine("Test case for DataOutputStream Started");
        }

        [Fact]
        [Trait("Prana.Utilities", "DataOutputStream")]
        public void Write_PassedIntegerToWrite_ReturnsCorrectInteger()
        {
            // Arrange
            var mockStream = new Mock<IOutputStream>();
            var memoryStream = new MemoryStream();
            mockStream.Setup(x => x.getBaseStream()).Returns(memoryStream);

            var dataOutputStream = new DataOutputStream(mockStream.Object);
            int expected = 42;

            // Act
            dataOutputStream.Write(expected);
            memoryStream.Position = 0;
            var actual = new BinaryReader(memoryStream).ReadInt32();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataOutputStream")]
        public void Write_PassedBytesToWrite_ShouldWriteCorrectBytes()
        {
            // Arrange
            var mockStream = new Mock<IOutputStream>();
            var memoryStream = new MemoryStream();
            mockStream.Setup(x => x.getBaseStream()).Returns(memoryStream);

            var dataOutputStream = new DataOutputStream(mockStream.Object);
            byte[] expected = new byte[] { 0x41, 0x42, 0x43 };

            // Act
            dataOutputStream.Write(expected);
            memoryStream.Position = 0;
            var actual = new BinaryReader(memoryStream).ReadBytes(expected.Length);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataOutputStream")]
        public void Write_PassedBytesWithOffsetAndLength_ShouldWriteCorrectBytes()
        {
            // Arrange
            var mockStream = new Mock<IOutputStream>();
            var memoryStream = new MemoryStream();
            mockStream.Setup(x => x.getBaseStream()).Returns(memoryStream);

            var dataOutputStream = new DataOutputStream(mockStream.Object);
            byte[] input = new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45 };
            int offset = 1;
            int length = 3;
            byte[] expected = new byte[] { 0x42, 0x43, 0x44 };

            // Act
            dataOutputStream.Write(input, offset, length);
            memoryStream.Position = 0;
            var actual = new BinaryReader(memoryStream).ReadBytes(length);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataOutputStream")]
        public void Flush_ShouldFlushBinaryWriter()
        {
            // Arrange
            var mockStream = new Mock<IOutputStream>();
            var memoryStream = new MemoryStream();
            mockStream.Setup(x => x.getBaseStream()).Returns(memoryStream);

            var dataOutputStream = new DataOutputStream(mockStream.Object);

            // Act
            dataOutputStream.Flush();
            memoryStream.Position = 0;
            var actual = memoryStream.ToArray();

            // Assert
            // Memory stream should contain No data after flush
            Assert.Empty(actual); 
        }

        [Fact]
        [Trait("Prana.Utilities", "DataOutputStream")]
        public void Close_ShouldCloseBinaryWriter()
        {
            // Arrange
            var mockStream = new Mock<IOutputStream>();
            var memoryStream = new MemoryStream();
            mockStream.Setup(x => x.getBaseStream()).Returns(memoryStream);

            var dataOutputStream = new DataOutputStream(mockStream.Object);

            // Act
            dataOutputStream.Close();

            // Assert
            // Writing after closing should throw ObjectDisposedException
            Assert.Throws<ObjectDisposedException>(() => dataOutputStream.Write(42)); 
        }

        [Fact]
        [Trait("Prana.Utilities", "DataOutputStream")]
        public void WriteInt64_ShouldWriteCorrectLong()
        {
            // Arrange
            var mockStream = new Mock<IOutputStream>();
            var memoryStream = new MemoryStream();
            mockStream.Setup(x => x.getBaseStream()).Returns(memoryStream);

            var dataOutputStream = new DataOutputStream(mockStream.Object);
            long expected = 123456789012345;

            // Act
            dataOutputStream.writeInt64(expected);
            memoryStream.Position = 0;
            var actual = new BinaryReader(memoryStream).ReadInt64();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataOutputStream")]
        public void WriteInt_ShouldWriteCorrectInt()
        {
            // Arrange
            var mockStream = new Mock<IOutputStream>();
            var memoryStream = new MemoryStream();
            mockStream.Setup(x => x.getBaseStream()).Returns(memoryStream);

            var dataOutputStream = new DataOutputStream(mockStream.Object);
            int expected = 42;

            // Act
            dataOutputStream.writeInt(expected);
            memoryStream.Position = 0;
            var actual = new BinaryReader(memoryStream).ReadInt32();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataOutputStream")]
        public void WriteLong_ShouldWriteCorrectLong()
        {
            // Arrange
            var mockStream = new Mock<IOutputStream>();
            var memoryStream = new MemoryStream();
            mockStream.Setup(x => x.getBaseStream()).Returns(memoryStream);

            var dataOutputStream = new DataOutputStream(mockStream.Object);
            long expected = 123456789012345;

            // Act
            dataOutputStream.writeLong(expected);
            memoryStream.Position = 0;
            var actual = new BinaryReader(memoryStream).ReadInt64();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
