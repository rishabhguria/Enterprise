using Prana.Utilities.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.IO
{
    public class StringByteConversionTests
    {
        private readonly ITestOutputHelper outputHelper;
        public StringByteConversionTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;

            //To check or write in test explorer you can use below line
            //outputHelper.WriteLine("Test case for StringByteConversion Started");
        }

        [Fact]
        [Trait("Prana.Utilities", "StringByteConversion")]
        public void GetBytes_PassString_ReturnByteTypeOfData()
        {
            // Arrange & Act
            string msg = "Hello";
            var datatype = StringByteConversion.GetBytes(msg).GetType();

            //outputHelper.WriteLine($"Actual ({typeof(byte[])}) === Expected ({datatype})");
            // Act
            Assert.True(datatype.Equals(typeof(byte[])));
        }

        [Fact]
        [Trait("Prana.Utilities", "StringByteConversion")]
        public void GetBytes_PassNull_ReturnArgumentNullException()
        {
            // Arrange & Act
            string msg = null;
            Action action = () => StringByteConversion.GetBytes(msg).GetType();

            // Assert
            Assert.Throws<ArgumentNullException>(action);
            //outputHelper.WriteLine($"By Passing Null values Returned ArgumentNullException ");
        }

        [Fact]
        [Trait("Prana.Utilities", "StringByteConversion")]
        public void GetString_PassByte_ReturnStringTypeOfData()
        {
            // Arrange
            byte[] data = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            var datatype = StringByteConversion.GetString(data).GetType();

            //outputHelper.WriteLine($"Actual ({typeof(string)}) === Expected ({datatype})");
            // Act
            Assert.True(datatype.Equals(typeof(string)));
        }

        [Fact]
        [Trait("Prana.Utilities", "StringByteConversion")]
        public void GetString_PassNull_ReturnStringTypeOfData()
        {
            // Arrange & Act
            byte[] data = null; ;
            Action action = () => StringByteConversion.GetString(data).GetType();

            // Assert
            Assert.Throws<NullReferenceException>(action);
            //outputHelper.WriteLine($"By Passing Null values Returned NullReferenceException");
        }
    }
}
