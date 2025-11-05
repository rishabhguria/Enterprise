using Prana.Utilities.MiscUtilities;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    [Serializable]
    public class TestObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class PranaBinaryFormatterTests
    {
        private readonly PranaBinaryFormatter _formatter = new PranaBinaryFormatter();

        [Fact]
        [Trait("Prana.Utilities", "PranaBinaryFormatter")]
        public void SerializeAndDeserialize_ReturnsOriginalObject()
        {
            var originalObject = new TestObject { Id = 1, Name = "Test" };
            var serializedString = _formatter.Serialize(originalObject);
            var deserializedObject = (TestObject)_formatter.DeSerialize(serializedString);

            Assert.NotNull(deserializedObject);
            Assert.Equal(originalObject.Id, deserializedObject.Id);
            Assert.Equal(originalObject.Name, deserializedObject.Name);
        }

        [Fact]
        [Trait("Prana.Utilities", "PranaBinaryFormatter")]
        public void SerializeAndDeserializeParams_ReturnsOriginalObjects()
        {
            var originalObject1 = new TestObject { Id = 1, Name = "Test1" };
            var originalObject2 = new TestObject { Id = 2, Name = "Test2" };
            var serializedString = _formatter.Serialize(originalObject1, originalObject2);
            var deserializedObjects = _formatter.DeSerializeParams(serializedString);

            Assert.NotNull(deserializedObjects);
            Assert.Equal(2, deserializedObjects.Count);

            var deserializedObject1 = (TestObject)deserializedObjects[0];
            var deserializedObject2 = (TestObject)deserializedObjects[1];

            Assert.Equal(originalObject1.Id, deserializedObject1.Id);
            Assert.Equal(originalObject1.Name, deserializedObject1.Name);
            Assert.Equal(originalObject2.Id, deserializedObject2.Id);
            Assert.Equal(originalObject2.Name, deserializedObject2.Name);
        }

        [Fact]
        [Trait("Prana.Utilities", "PranaBinaryFormatter")]
        public void Serialize_WithNullObject_ReturnsNull()
        {
            // Act
            string serializedString = _formatter.Serialize(null);

            // Assert
            Assert.Null(serializedString);
        }

        [Fact]
        [Trait("Prana.Utilities", "PranaBinaryFormatter")]
        public void DeSerialize_WithEmptyString_ReturnsNull()
        {
            // Act
            var deserializedObjects = _formatter.DeSerializeParams(string.Empty);

            // Assert
            Assert.Null(deserializedObjects);
        }
    }
}
