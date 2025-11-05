using Xunit;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.SecurityMasterBusinessObjects
{
    public class DynamicUDATests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "DynamicUDA")]
        public void DynamicUDA_Constructor_InitializesProperties()
        {
            // Arrange
            string tag = "TestTag";
            string caption = "TestCaption";
            string defaultValue = "DefaultValue";
            string xml = "<MasterUDAValue><Value key=\"Key1\">Value1</Value><Value key=\"Key2\">Value2</Value></MasterUDAValue>";         
            SerializableDictionary<string, string> masterValues = new SerializableDictionary<string, string>
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" }
        };

            // Act
            var uda = new DynamicUDA(tag, caption, defaultValue, xml);

            // Assert
            Assert.Equal(tag, uda.Tag);
            Assert.Equal(caption, uda.HeaderCaption);
            Assert.Equal(defaultValue, uda.DefaultValue);
            Assert.Equal(masterValues, uda.MasterValues);
        }

       
        [Fact]
        [Trait("Prana.BusinessObjects", "DynamicUDA")]
        public void SerializeToXML_ConvertsDictionaryToXML()
        {
            // Arrange
            var masterValues = new SerializableDictionary<string, string>
    {
        { "Key1", "Value1" },
        { "Key2", "Value2" }
    };
            var uda = new DynamicUDA();

            // Act
            var xml = uda.SerializeToXML(masterValues);

            // Assert
            string expected = "<MasterUDAValue>\r\n  <Value key=\"Key1\">Value1</Value>\r\n  <Value key=\"Key2\">Value2</Value>\r\n</MasterUDAValue>";
            Assert.Equal(expected, xml);
        }


        [Fact]
        [Trait("Prana.BusinessObjects", "DynamicUDA")]
        public void DynamicUDA_DefaultConstructor_InitializesWithoutException()
        {
            // Arrange & Act
            var uda = new DynamicUDA();

            // Assert
            Assert.NotNull(uda);
            Assert.NotNull(uda.MasterValues);
        }
    }
}




