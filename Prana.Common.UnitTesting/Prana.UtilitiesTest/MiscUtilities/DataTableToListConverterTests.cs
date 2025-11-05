using Prana.BusinessObjects;
using Prana.UnitTesting.MockDataCreation;
using Prana.Utilities.MiscUtilities;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class DataTableToListConverterTests
    {
        [Fact]
        [Trait("Prana.Utilities", "DataTableToListConverter")]
        public void GetListFromDataTable_WithValidDataTable_ReturnsCorrectList()
        {
            // Arrange
            DataTable customDataTable = CreateDataTable.GetTable();
            List<string> expectedlistofDataTable = CreateDataTable.GetTableDataAsListOfString();

            // Act
            List<string> result = DataTableToListConverter.GetListFromDataTable(customDataTable);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count); // Includes header
            Assert.Equal(expectedlistofDataTable, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataTableToListConverter")]
        public void GetListFromDataTable_WithNullDataTable_ReturnsNull()
        {
            // Act
            List<string> result = DataTableToListConverter.GetListFromDataTable(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataTableToListConverter")]
        public void GetDataTableFromList_WithValidList_CreatesCorrectDataTable()
        {
            // Arrange
            List<string> list = new List<string>
            {
                "Header" + Seperators.SEPERATOR_5 + "Id" + Seperators.SEPERATOR_12 + "System.Int32" + Seperators.SEPERATOR_5 + "Name" + Seperators.SEPERATOR_12 + "System.String",
                "Id" + Seperators.SEPERATOR_12 + "1" + Seperators.SEPERATOR_5 + "Name" + Seperators.SEPERATOR_12 + "John Doe",
                "Id" + Seperators.SEPERATOR_12 + "2" + Seperators.SEPERATOR_5 + "Name" + Seperators.SEPERATOR_12 + "Jane Doe"
            };

            // Act
            DataTable dataTable = DataTableToListConverter.GetDataTableFromList(list);

            // Assert
            Assert.NotNull(dataTable);
            Assert.Equal(2, dataTable.Columns.Count);
            Assert.Equal("Id", dataTable.Columns[0].ColumnName);
            Assert.Equal(typeof(int), dataTable.Columns[0].DataType);
            Assert.Equal("Name", dataTable.Columns[1].ColumnName);
            Assert.Equal(typeof(string), dataTable.Columns[1].DataType);
            Assert.Equal(2, dataTable.Rows.Count);
            Assert.Equal(1, dataTable.Rows[0]["Id"]);
            Assert.Equal("John Doe", dataTable.Rows[0]["Name"]);
            Assert.Equal(2, dataTable.Rows[1]["Id"]);
            Assert.Equal("Jane Doe", dataTable.Rows[1]["Name"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataTableToListConverter")]
        public void GetDataTableFromList_WithNullList_ReturnsNull()
        {
            // Act
            DataTable result = DataTableToListConverter.GetDataTableFromList(null);

            // Assert
            Assert.Null(result);
        }
    }
}
