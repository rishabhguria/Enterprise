using Prana.UnitTesting.MockDataCreation;
using Prana.Utilities.IO;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.IO
{
    public class RandomAccessFileTests : IDisposable
    {
        private RandomAccessFile randomAccessFile;
        private readonly ITestOutputHelper outputHelper;
        private string file;
        public RandomAccessFileTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;

            initRandomAccessFile();

            //To check or write in test explorer you can use below line
            //outputHelper.WriteLine("Test case for RandomAccessFile Started");
        }

        /// <summary>
        /// Create the Mock data Text File
        /// </summary>
        public void initRandomAccessFile()
        {
            file = MockDataPath.GetFilePath();

            //filemode : "rwd","r","w"
            string fileMode = "rwd";
            randomAccessFile = new RandomAccessFile(file, fileMode);
        }

        [Fact]
        [Trait("Prana.Utilities", "RandomAccessFile")]
        public void Length_PassedTestingFile_ReturnsCorrectLength()
        {
            // Arrange & Act
            long ExpectedValue = 0;
            long ActualValue = randomAccessFile.Length();

            //outputHelper.WriteLine($"Actual ({ActualValue}) === Expected ({ExpectedValue})");
            // Act
            Assert.Equal(ExpectedValue, ActualValue);
        }

        [Theory]
        [InlineData("Hello, world!")]
        [InlineData("-")]
        [Trait("Prana.Utilities", "RandomAccessFile")]
        public void WriteAndReadUTF(string ExpectedText)
        {
            // Arrange
            randomAccessFile.writeUTF(ExpectedText);

            // Act
            randomAccessFile.Seek(0);
            string ActualText = randomAccessFile.readUTF();

            //outputHelper.WriteLine($"Actual ({ActualText}) === Expected ({ExpectedText})");
            // Assert 
            Assert.Equal(ExpectedText, ActualText);
        }

        [Fact]
        [Trait("Prana.Utilities", "RandomAccessFile")]
        public void Read_WithOffsetAndLength_ReturnsCorrectBytes()
        {
            // Arrange 
            byte[] expectedBytes = { 72, 101, 108, 108, 111, 44 }; // "Hello," in ASCII
            byte[] actualBytes = new byte[6];

            // Act
            randomAccessFile.Write(expectedBytes);
            randomAccessFile.Seek(0);
            randomAccessFile.Read(actualBytes, 0, 6);

            //outputHelper.WriteLine($"Actual ({actualBytes.Length}) === Expected ({expectedBytes.Length})");
            // Assert 
            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        [Trait("Prana.Utilities", "RandomAccessFile")]
        public void Read_WithoutOffsetAndLength_ReturnsCorrectBytes()
        {
            // Arrange 
            byte[] expectedBytes = { 119, 111, 114, 108, 100, 33 }; // "world!" in ASCII
            byte[] actualBytes = new byte[6];

            // Act
            randomAccessFile.Write(expectedBytes);
            randomAccessFile.Seek(0);
            randomAccessFile.Read(actualBytes);

            //outputHelper.WriteLine($"Actual ({actualBytes.Length}) === Expected ({expectedBytes.Length})");
            // Assert 
            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        [Trait("Prana.Utilities", "RandomAccessFile")]
        public void Seek_PositionsStreamCorrectly()
        {
            // Arrange 
            byte[] WriteBytes = { 72, 101, 108, 108, 111 }; // "Hello" in ASCII
            byte[] expectedBytes = { 108, 108, 111, 0, 0 }; 
            byte[] actualBytes = new byte[5];

            // Act
            randomAccessFile.Write(WriteBytes);
            //starting from 2nd postion
            randomAccessFile.Seek(2);
            randomAccessFile.Read(actualBytes, 0, 5);

            //outputHelper.WriteLine($"Actual ({actualBytes.Length}) === Expected ({expectedBytes.Length})");
            // Assert 
            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        [Trait("Prana.Utilities", "RandomAccessFile")]
        public void Sync_NoErrorThrown_ReturnsTestCasePassed()
        {
            try
            {
                // Act
                randomAccessFile.Sync();
                // Assert
                Assert.True(true);
            }
            catch (Exception)
            {
                // Assert
                Assert.True(false);
            }

        }

        [Fact]
        [Trait("Prana.Utilities", "RandomAccessFile")]
        public void GetFD_ReturnsSameInstance()
        {
            // Act
            RandomAccessFile returnedInstance = randomAccessFile.GetFD();

            // Assert 
            Assert.Same(randomAccessFile, returnedInstance);
        }

        [Fact]
        [Trait("Prana.Utilities", "RandomAccessFile")]
        public void getFilePointer_ReturnsCorrectPosition()
        {
            // Arrange 
            long expectedPosition = 0;

            // Act
            long actualPosition = randomAccessFile.getFilePointer();

            // Assert 
            Assert.Equal(expectedPosition, actualPosition);
        }

        public void Dispose()
        {
            randomAccessFile.Close();
            // Check if file exists with its full path
            if (System.IO.File.Exists(file))
            {
                // If file found, delete it
                System.IO.File.Delete(file);
            }
        }
    }
}
