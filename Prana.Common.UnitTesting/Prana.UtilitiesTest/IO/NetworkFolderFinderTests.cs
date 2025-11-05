using Moq;
using Prana.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.IO
{
    public class NetworkFolderFinderTests
    {

        [Fact]
        [Trait("Prana.Utilities", "NetworkFolderFinder")]
        public void MachineExist_UncPathExists_ReturnsTrue()
        {
            // Arrange & Act
            var result = NetworkFolderFinder.MachineExist(@"\\localhost\", 1000);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "NetworkFolderFinder")]
        public void MachineExist_UncPathNotExists_ReturnsFalse()
        {
            // Arrange & Act
            var result = NetworkFolderFinder.MachineExist(@"\\196.0.0.168\Test", 1000);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "NetworkFolderFinder")]
        public void DirectoryExist_UncPathExists_ReturnsTrue()
        {
            // Arrange & Act
            var result = NetworkFolderFinder.DirectoryExist(@"C:\", 1000);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "NetworkFolderFinder")]
        public void DirectoryExist_UncPathNotExists_ReturnsFalse()
        {
            // Arrange & Act
            var result = NetworkFolderFinder.DirectoryExist(@"\\196.0.0.168\Test", 1000);

            // Assert
            Assert.False(result);
        }
    }
}
