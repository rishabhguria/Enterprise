using Prana.BusinessObjects;
using System;
using System.Data;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.Extensions
{
    public class DataRowExtensionTests
    {
        private DataRow CreateDataRow()
        {
            DataTable table = new DataTable();
            table.Columns.Add("DoubleColumn", typeof(string));
            table.Columns.Add("IntColumn", typeof(string));
            table.Columns.Add("StringColumn", typeof(string));
            table.Columns.Add("BoolColumn", typeof(Boolean));
            table.Columns.Add("DateColumn", typeof(string));
            table.Columns.Add("EnumColumn", typeof(string));
            table.Columns.Add("IsZero",typeof(Boolean));

            DataRow row = table.NewRow();
            row["DoubleColumn"] = "123.45";
            row["IntColumn"] = "42";
            row["StringColumn"] = "TestString";
            row["BoolColumn"] = true;
            row["DateColumn"] = "2023-10-15";
            row["EnumColumn"] = "FirstValue";
            row["IsZero"] = true;
            return row;
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DataRowExtension")]
        public void GetDouble_ValidValue_ReturnsDouble()
        {
            // Arrange
            var row = CreateDataRow();

            // Act
            double result = row.GetDouble("DoubleColumn", 0);

            // Assert
            Assert.Equal(123.45, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DataRowExtension")]
        public void GetDouble_InvalidValue_ReturnsDefault()
        {
            // Arrange
            var row = CreateDataRow();
            row["DoubleColumn"] = "invalid";

            // Act
            double result = row.GetDouble("DoubleColumn", 0);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DataRowExtension")]
        public void GetInteger_ValidValue_ReturnsInt()
        {
            // Arrange
            var row = CreateDataRow();

            // Act
            int result = row.GetInteger("IntColumn", 0);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DataRowExtension")]
        public void GetString_ValidValue_ReturnsString()
        {
            // Arrange
            var row = CreateDataRow();

            // Act
            string result = row.GetString("StringColumn", "Default");

            // Assert
            Assert.Equal("TestString", result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DataRowExtension")]
        public void GetBool_ValidValue_ReturnsBoolean()
        {
            // Arrange
            var row = CreateDataRow();

            // Act
            bool result = row.GetBool("BoolColumn", false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DataRowExtension")]
        public void GetEnum_ValidValue_ReturnsEnum()
        {
            // Arrange
            var row = CreateDataRow();

            // Act
            TestEnum result = row.GetEnum<TestEnum>("EnumColumn", TestEnum.DefaultValue);

            // Assert
            Assert.Equal(TestEnum.FirstValue, result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DataRowExtension")]
        public void GetDate_ValidValue_ReturnsDate()
        {
            // Arrange
            var row = CreateDataRow();

            // Act
            DateTime result = row.GetDate("DateColumn", DateTime.MinValue);

            // Assert
            Assert.Equal(new DateTime(2023, 10, 15), result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DataRowExtension")]
        public void GetDate_InvalidValue_ReturnsDefault()
        {
            // Arrange
            var row = CreateDataRow();
            row["DateColumn"] = "invalid";

            // Act
            DateTime result = row.GetDate("DateColumn", DateTime.MinValue);

            // Assert
            Assert.Equal(DateTime.MinValue, result);
        }
    }

    public enum TestEnum
    {
        DefaultValue,
        FirstValue,
        SecondValue
    }
}
