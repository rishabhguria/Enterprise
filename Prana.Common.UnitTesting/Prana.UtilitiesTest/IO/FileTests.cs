using Moq;
using Prana.UnitTesting.MockDataCreation;
using Prana.Utilities.IO;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.IO
{
    public class FileTests
    {
        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void Delete_DeleteFileWhenFileExists_ReturnTrue()
        {
            // Arrange
            var filePath = MockDataPath.GetFilePath("Test1.txt");
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.FileStream fs = System.IO.File.Create(filePath);
                fs.Close();
            }
            var file = new File(filePath);

            // Act
            var result = file.Delete();

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void Delete_DeleteFileWhenNoFileExists_ReturnException()
        {
            // Arrange
            var filePath = MockDataPath.GetFilePath("Test2.txt");
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.FileStream fs = System.IO.File.Create(filePath);
                fs.Close();
            }
            var file = new File(filePath);

            file.Delete();

            // Act
            var result = file.Delete();

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("C:\\", true)]
        [InlineData("F:\\", false)]
        [Trait("Prana.Utilities", "File")]
        public void Exists_PassedPath_ReturnCorrectFileExistence(string path, bool expectedResult)
        {
            // Arrange
            File _file = new File(path);

            // Act
            var result = _file.Exists();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void GetName_ReturnsCorrectName()
        {
            // Arrange
            var _filePath = MockDataPath.GetFilePath("Test3.txt");
            var _file = new File(_filePath);

            // Act
            var result = _file.GetName();

            // Assert
            Assert.Equal("Test3.txt", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void IsDirectory_PassedDirectoryPath_Returntrue()
        {
            // Arrange
            string _path = "C:\\";
            bool ExpectedValue = true;
            var _file = new File(_path);

            // Act
            bool ActualValue = _file.IsDirectory();

            // Assert
            Assert.Equal(ExpectedValue, ActualValue);
        }

        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void IsDirectory_PassedFilePath_ReturnFalse()
        {
            // Arrange
            var filePath = MockDataPath.GetFilePath("test4.txt");
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.FileStream fs = System.IO.File.Create(filePath);
                fs.Close();
            }
            var file = new File(filePath);

            bool ExpectedValue = false;

            // Act
            bool ActualValue = file.IsDirectory();

            // Assert
            Assert.Equal(ExpectedValue, ActualValue);

            //Deleteing the created file
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void GetCanonicalPath_ReturnPath()
        {
            // Arrange
            var ExpectedPath = "C:\\";
            var file = new File(ExpectedPath);

            // Act
            var ActualPath = file.GetCanonicalPath();

            //Assert
            Assert.Equal(ExpectedPath, ActualPath);
        }

        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void GetAbsolutePath_ReturnPath()
        {
            // Arrange
            var ExpectedPath = "C:\\";
            var file = new File(ExpectedPath);

            // Act
            var ActualPath = file.GetAbsolutePath();

            //Assert
            Assert.Equal(ExpectedPath, ActualPath);
        }

        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void GetPath_ReturnPath()
        {
            // Arrange
            var ExpectedPath = "C:\\";
            var file = new File(ExpectedPath);

            // Act
            var ActualPath = file.GetPath();

            //Assert
            Assert.Equal(ExpectedPath, ActualPath);
        }

        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void Mkdir_DirectoryExists_ReturnFalse()
        {
            // Arrange
            var filePath = "C:\\";
            var file = new File(filePath);

            // Act
            var result = file.Mkdir();

            //Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void Mkdirs_DirectoryExists_ReturnFalse()
        {
            // Arrange
            var filePath = "C:\\";
            var file = new File(filePath);

            // Act
            var result = file.Mkdirs();

            //Assert
            Assert.False(result);
        }

        //This test case is for the file moved from a-location to b-location
        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void RenameTo_PassedPath_CheckTheFileExist()
        {
            // Arrange
            var filePath = MockDataPath.GetFilePath("test5.txt");
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.FileStream fs = System.IO.File.Create(filePath);
                fs.Close();
            }
            var file = new File(filePath);

            string newPath = MockDataPath.GetTestingFolderPath() + "test6.txt";
            File newFilePath = new File(newPath);

            //string ExpectedFile = newPath + "NewTestingTextFile.txt";

            // Act
            file.RenameTo(newFilePath);

            //Assert
            Assert.True(System.IO.File.Exists(newPath));

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            if (System.IO.File.Exists(newPath))
            {
                System.IO.File.Delete(newPath);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void IsFileOpen_FileClosed_ReturnFalse()
        {
            // Arrange
            var filePath = MockDataPath.GetFilePath("test7.txt");

            // Act
            bool result = File.IsFileOpen(filePath);

            // Assert
            Assert.True(result);

            //Deleteing the created file
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void Length_ReturnsSizeofCurrentFile()
        {
            // Arrange
            var filePath = MockDataPath.GetFilePath("test8.txt");
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.FileStream fs = System.IO.File.Create(filePath);
                fs.Close();
            }
            var file = new File(filePath);
            long ExpectedValue = 0;

            // Act
            long ActualValue = file.Length();

            // Assert
            Assert.Equal(ExpectedValue,ActualValue);

            //Deleteing the created file
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "File")]
        public void List_ReturnsNoFilesOnPath()
        {
            // Arrange
            string filePath = MockDataPath.GetTestingFolderPath();
            if (!System.IO.Directory.Exists(filePath))
                System.IO.Directory.CreateDirectory(filePath);
            var file = new File(filePath);
            int ExpectedValue = 0;

            // Act
            var ActualValue = file.List().Length;

            // Assert
            Assert.Equal(ExpectedValue, ActualValue);
        }
    }
}
