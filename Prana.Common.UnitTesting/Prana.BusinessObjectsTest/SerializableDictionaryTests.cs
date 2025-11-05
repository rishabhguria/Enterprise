using Prana.BusinessObjects;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest
{
    public class SerializableDictionaryTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "SerializableDictionary")]
        public void AddAndRetrieveItems_ShouldWork()
        {
            // Arrange
            var dictionary = new SerializableDictionary<int, string>();

            // Act
            dictionary.Add(1, "one");
            dictionary.Add(2, "two");

            // Assert
            Assert.Equal("one", dictionary[1]);
            Assert.Equal("two", dictionary[2]);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SerializableDictionary")]
        public void SerializationAndDeserialization_ShouldWork()
        {
            // Arrange
            var dictionary = new SerializableDictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" }
            };

            var serializer = new XmlSerializer(typeof(SerializableDictionary<int, string>));
            string xml;

            // Act - Serialize to XML
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    serializer.Serialize(xmlWriter, dictionary);
                    xml = stringWriter.ToString();
                }
            }

            // Assert - Check if XML string is not empty
            Assert.False(string.IsNullOrEmpty(xml));

            // Act - Deserialize from XML
            SerializableDictionary<int, string> deserializedDictionary;
            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    deserializedDictionary = (SerializableDictionary<int, string>)serializer.Deserialize(xmlReader);
                }
            }

            // Assert - Check if deserialized dictionary contains the same data
            Assert.Equal("one", deserializedDictionary[1]);
            Assert.Equal("two", deserializedDictionary[2]);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SerializableDictionary")]
        public void EmptyDictionarySerialization_ShouldWork()
        {
            // Arrange
            var dictionary = new SerializableDictionary<int, string>();

            var serializer = new XmlSerializer(typeof(SerializableDictionary<int, string>));
            string xml;

            // Act - Serialize an empty dictionary
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    serializer.Serialize(xmlWriter, dictionary);
                    xml = stringWriter.ToString();
                }
            }

            // Assert - Check if XML string is not null
            Assert.False(string.IsNullOrEmpty(xml));

            // Act - Deserialize from XML
            SerializableDictionary<int, string> deserializedDictionary;
            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    deserializedDictionary = (SerializableDictionary<int, string>)serializer.Deserialize(xmlReader);
                }
            }

            // Assert - Check if deserialized dictionary is empty
            Assert.Empty(deserializedDictionary);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SerializableDictionary")]
        public void WriteXml_ShouldGenerateCorrectXml()
        {
            // Arrange
            var dictionary = new SerializableDictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" }
            };

            string xml;
            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                ConformanceLevel = ConformanceLevel.Fragment // Allow writing XML fragments
            };

            // Act - Serialize the dictionary using WriteXml
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    dictionary.WriteXml(xmlWriter);
                }
                xml = stringWriter.ToString();
            }

            // Assert - Check if XML contains expected elements
            Assert.Contains("<item>", xml);
            Assert.Contains("<key>", xml);
            Assert.Contains("<value>", xml);
            Assert.Contains("one", xml);
            Assert.Contains("two", xml);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "SerializableDictionary")]
        public void WriteXml_EmptyDictionary_ShouldProduceEmptyXml()
        {
            // Arrange
            var dictionary = new SerializableDictionary<int, string>();

            string xml;
            var xmlWriterSettings = new XmlWriterSettings { Indent = true };

            // Act - Serialize an empty dictionary using WriteXml
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    dictionary.WriteXml(xmlWriter);
                }
                xml = stringWriter.ToString();
            }

            // Assert - Check if XML does not contain any <item> tags
            Assert.DoesNotContain("<item>", xml);
        }
    }
}
