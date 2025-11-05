using Prana.BusinessObjects;
using System.Data;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.Extensions
{
    public class DataTableExtensionTests
    {
        private DataTable CreateSampleDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ZColumn", typeof(string));
            dt.Columns.Add("AColumn", typeof(string));
            dt.Columns.Add("MColumn", typeof(string));
            return dt;
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DataTableExtension")]
        public void AlphabeticColumnSort_SortsColumnsAlphabetically()
        {
            // Arrange
            DataTable dt = CreateSampleDataTable();

            // Act
            dt.AlphabeticColumnSort();

            // Assert
            Assert.Equal("AColumn", dt.Columns[0].ColumnName);
            Assert.Equal("MColumn", dt.Columns[1].ColumnName);
            Assert.Equal("ZColumn", dt.Columns[2].ColumnName);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DataTableExtension")]
        public void AlphabeticColumnSort_EmptyDataTable_DoesNotThrow()
        {
            // Arrange
            DataTable dt = new DataTable();

            // Act & Assert
            dt.AlphabeticColumnSort(); // Should not throw
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DataTableExtension")]
        public void SortColumns_SortsBySortingString()
        {
            // Arrange
            DataTable dt = CreateSampleDataTable();
            dt.Rows.Add("YValue", "BValue", "NValue");
            dt.Rows.Add("ZValue", "AValue", "MValue");
            

            // Act
            DataTable sortedTable = dt.SortColumns("AColumn");

            // Assert
            Assert.Equal("AValue", sortedTable.Rows[0]["AColumn"]);
            Assert.Equal("BValue", sortedTable.Rows[1]["AColumn"]);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "DataTableExtension")]
        public void SortColumns_EmptyDataTable_ReturnsEmptyDataTable()
        {
            // Arrange
            DataTable dt = new DataTable();

            // Act
            DataTable sortedTable = dt.SortColumns("");

            // Assert
            Assert.Empty(sortedTable.Columns);
        }
    }
}
