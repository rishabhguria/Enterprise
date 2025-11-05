using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Utilities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Utilities
{
    public class IEnumerableExtensionTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "IEnumerableExtension")]
        public void DistinctBy_ReturnsDistinctRecords()
        {
            // Arrange
            var source = new List<string> { "a", "b", "a", "c" };

            // Act
            var result = source.DistinctBy(s => s).ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("a", result);
            Assert.Contains("b", result);
            Assert.Contains("c", result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "IEnumerableExtension")]
        public void ToHashSet_ConvertsToHashSet()
        {
            // Arrange
            var source = new List<int> { 1, 2, 3, 4 };

            // Act
            var result = BusinessObjects.Classes.Utilities.IEnumerableExtension.ToHashSet(source);

            // Assert
            Assert.IsType<HashSet<int>>(result);
            Assert.Equal(4, result.Count);
        }


        [Fact]
        [Trait("Prana.BusinessObjects", "IEnumerableExtension")]
        public void GetMatchingIEnumerableValues_ReturnsMatchingValues()
        {
            // Arrange
            var source = new List<int> { 1, 2, 3, 4 };

            // Act
            var result = source.GetMatchingIEnumerableValues(2);

            // Assert
            Assert.Single(result);
            Assert.Contains(2, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "IEnumerableExtension")]
        public void ToSerializableDictionary_ConvertsToSerializableDictionary()
        {
            // Arrange
            var source = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(1, "one"),
            new KeyValuePair<int, string>(2, "two")
        };

            // Act
            var result = source.ToSerializableDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Assert
            Assert.IsType<SerializableDictionary<int, string>>(result);
            Assert.Equal("one", result[1]);
            Assert.Equal("two", result[2]);
        }
    }
}