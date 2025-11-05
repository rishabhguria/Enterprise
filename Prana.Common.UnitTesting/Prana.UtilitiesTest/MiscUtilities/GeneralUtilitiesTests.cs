using Prana.BusinessObjects;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest
{
    public class GeneralUtilitiesTests
    {
        #region GetListFromString
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetListFromString_NullInput_ReturnsNull()
        {
            // Arrange
            string input = null;
            char separator = ',';

            // Act
            List<string> result = GeneralUtilities.GetListFromString(input, separator);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetListFromString_EmptyStringInput_ReturnsEmptyList()
        {
            // Arrange
            string input = string.Empty;
            char separator = ',';

            // Act
            List<string> result = GeneralUtilities.GetListFromString(input, separator);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetListFromString_SingleValueInput_ReturnsSingleItemList()
        {
            // Arrange
            string input = "John";
            char separator = ',';

            // Act
            List<string> result = GeneralUtilities.GetListFromString(input, separator);

            // Assert
            Assert.Single(result);
            Assert.Equal("John", result[0]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetListFromString_MultipleValuesInput_ReturnsCorrectList()
        {
            // Arrange
            string input = "John,Paul,George,Ringo";
            char separator = ',';

            // Act
            List<string> result = GeneralUtilities.GetListFromString(input, separator);

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Equal("John", result[0]);
            Assert.Equal("Paul", result[1]);
            Assert.Equal("George", result[2]);
            Assert.Equal("Ringo", result[3]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetListFromString_InputWithEmptyValues_ReturnsCorrectList()
        {
            // Arrange
            string input = "John,,Paul,,George,Ringo,";
            char separator = ',';

            // Act
            List<string> result = GeneralUtilities.GetListFromString(input, separator);

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Equal("John", result[0]);
            Assert.Equal("Paul", result[1]);
            Assert.Equal("George", result[2]);
            Assert.Equal("Ringo", result[3]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetListFromString_CustomSeparator_ReturnsCorrectList()
        {
            // Arrange
            string input = "John|Paul|George|Ringo";
            char separator = '|';

            // Act
            List<string> result = GeneralUtilities.GetListFromString(input, separator);

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Equal("John", result[0]);
            Assert.Equal("Paul", result[1]);
            Assert.Equal("George", result[2]);
            Assert.Equal("Ringo", result[3]);
        }
        #endregion

        #region GetStringFromList
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetStringFromList_EmptyList_ReturnsEmptyString()
        {
            // Arrange
            List<string> input = new List<string>();
            char separator = ',';

            // Act
            string result = GeneralUtilities.GetStringFromList(input, separator);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetStringFromList_SingleItemList_ReturnsSingleValueString()
        {
            // Arrange
            List<string> input = new List<string> { "John" };
            char separator = ',';

            // Act
            string result = GeneralUtilities.GetStringFromList(input, separator);

            // Assert
            Assert.Equal("John", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetStringFromList_MultipleItemList_ReturnsCorrectString()
        {
            // Arrange
            List<string> input = new List<string> { "John", "Paul", "George", "Ringo" };
            char separator = ',';

            // Act
            string result = GeneralUtilities.GetStringFromList(input, separator);

            // Assert
            Assert.Equal("John,Paul,George,Ringo", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetStringFromList_ListWithEmptyValues_ReturnsCorrectString()
        {
            // Arrange
            List<string> input = new List<string> { "John", "", "Paul", "", "George", "Ringo" };
            char separator = ',';

            // Act
            string result = GeneralUtilities.GetStringFromList(input, separator);

            // Assert
            Assert.Equal("John,,Paul,,George,Ringo", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetStringFromList_CustomSeparator_ReturnsCorrectString()
        {
            // Arrange
            List<string> input = new List<string> { "John", "Paul", "George", "Ringo" };
            char separator = '|';

            // Act
            string result = GeneralUtilities.GetStringFromList(input, separator);

            // Assert
            Assert.Equal("John|Paul|George|Ringo", result);
        }
        #endregion

        #region GetListFromArrayList
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetListFromArrayList_EmptyArrayList_ReturnsEmptyList()
        {
            // Arrange
            ArrayList input = new ArrayList();

            // Act
            List<string> result = GeneralUtilities.GetListFromArrayList(input);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetListFromArrayList_ArrayListWithStrings_ReturnsCorrectList()
        {
            // Arrange
            ArrayList input = new ArrayList { "John", "Paul", "George", "Ringo" };

            // Act
            List<string> result = GeneralUtilities.GetListFromArrayList(input);

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Equal("John", result[0]);
            Assert.Equal("Paul", result[1]);
            Assert.Equal("George", result[2]);
            Assert.Equal("Ringo", result[3]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetListFromArrayList_ArrayListWithMixedTypes_ReturnsStringList()
        {
            // Arrange
            ArrayList input = new ArrayList { "John", 123, true, 45.67 };

            // Act
            List<string> result = GeneralUtilities.GetListFromArrayList(input);

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Equal("John", result[0]);
            Assert.Equal("123", result[1]);
            Assert.Equal("True", result[2]);
            Assert.Equal("45.67", result[3]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetListFromArrayList_ArrayListWithNullValues_ReturnsStringListWithNulls()
        {
            // Arrange
            ArrayList input = new ArrayList { "John", null, "Paul" };

            // Act
            List<string> result = GeneralUtilities.GetListFromArrayList(input);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("John", result[0]);
            Assert.Null(result[1]);
            Assert.Equal("Paul", result[2]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetListFromArrayList_ArrayListWithEmptyStrings_ReturnsCorrectList()
        {
            // Arrange
            ArrayList input = new ArrayList { "John", "", "Paul" };

            // Act
            List<string> result = GeneralUtilities.GetListFromArrayList(input);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("John", result[0]);
            Assert.Equal(string.Empty, result[1]);
            Assert.Equal("Paul", result[2]);
        }
        #endregion

        #region GetStringFromArrayList
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetStringFromArrayList_EmptyArrayList_ReturnsEmptyString()
        {
            // Arrange
            ArrayList input = new ArrayList();

            // Act
            string result = GeneralUtilities.GetStringFromArrayList(input);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetStringFromArrayList_ArrayListWithStrings_ReturnsCorrectString()
        {
            // Arrange
            ArrayList input = new ArrayList { "John", "Paul", "George", "Ringo" };

            // Act
            string result = GeneralUtilities.GetStringFromArrayList(input);

            // Assert
            Assert.Equal("John,Paul,George,Ringo", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetStringFromArrayList_ArrayListWithMixedTypes_ReturnsCorrectString()
        {
            // Arrange
            ArrayList input = new ArrayList { "John", 123, true, 45.67 };

            // Act
            string result = GeneralUtilities.GetStringFromArrayList(input);

            // Assert
            Assert.Equal("John,123,True,45.67", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetStringFromArrayList_ArrayListWithNullValues_ReturnsStringWithEmptyEntries()
        {
            // Arrange
            ArrayList input = new ArrayList { "John", null, "Paul" };

            // Act
            string result = GeneralUtilities.GetStringFromArrayList(input);

            // Assert
            Assert.Equal("John,,Paul", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetStringFromArrayList_ArrayListWithEmptyStrings_ReturnsCorrectString()
        {
            // Arrange
            ArrayList input = new ArrayList { "John", "", "Paul" };

            // Act
            string result = GeneralUtilities.GetStringFromArrayList(input);

            // Assert
            Assert.Equal("John,,Paul", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetStringFromArrayList_ArrayListWithSpecialCharacters_ReturnsCorrectString()
        {
            // Arrange
            ArrayList input = new ArrayList { "John", "Paul", "Geor,ge", "Ring;o" };

            // Act
            string result = GeneralUtilities.GetStringFromArrayList(input);

            // Assert
            Assert.Equal("John,Paul,Geor,ge,Ring;o", result);
        }
        #endregion

        #region CloneList
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CloneList_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            List<string> input = new List<string>();

            // Act
            List<string> result = GeneralUtilities.CloneList(input);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CloneList_SingleItemList_ReturnsClonedList()
        {
            // Arrange
            List<string> input = new List<string> { "John" };

            // Act
            List<string> result = GeneralUtilities.CloneList(input);

            // Assert
            Assert.Single(result);
            Assert.Equal("John", result[0]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CloneList_MultipleItemList_ReturnsClonedList()
        {
            // Arrange
            List<string> input = new List<string> { "John", "Paul", "George", "Ringo" };

            // Act
            List<string> result = GeneralUtilities.CloneList(input);

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Equal("John", result[0]);
            Assert.Equal("Paul", result[1]);
            Assert.Equal("George", result[2]);
            Assert.Equal("Ringo", result[3]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CloneList_ModifyingOriginalList_DoesNotAffectClonedList()
        {
            // Arrange
            List<string> input = new List<string> { "John", "Paul" };

            // Act
            List<string> result = GeneralUtilities.CloneList(input);
            input.Add("George");

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("John", result[0]);
            Assert.Equal("Paul", result[1]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CloneList_OriginalListWithEmptyStrings_ReturnsClonedListWithEmptyStrings()
        {
            // Arrange
            List<string> input = new List<string> { "John", "", "Paul" };

            // Act
            List<string> result = GeneralUtilities.CloneList(input);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("John", result[0]);
            Assert.Equal(string.Empty, result[1]);
            Assert.Equal("Paul", result[2]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CloneList_NullValuesInOriginalList_ReturnsClonedListWithNullValues()
        {
            // Arrange
            List<string> input = new List<string> { "John", null, "Paul" };

            // Act
            List<string> result = GeneralUtilities.CloneList(input);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("John", result[0]);
            Assert.Null(result[1]);
            Assert.Equal("Paul", result[2]);
        }
        #endregion

        #region Copy
        public class TestClass
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public bool IsActive { get; set; }
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void Copy_SameClassProperties_CopiesValuesCorrectly()
        {
            // Arrange
            var src = new TestClass { Name = "John", Age = 30, IsActive = true };
            var dest = new TestClass();

            // Act
            GeneralUtilities.Copy(src, dest);

            // Assert
            Assert.Equal(src.Name, dest.Name);
            Assert.Equal(src.Age, dest.Age);
            Assert.Equal(src.IsActive, dest.IsActive);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void Copy_SourceWithDefaultValues_DestinationHasDefaultValues()
        {
            // Arrange
            var src = new TestClass();
            var dest = new TestClass { Name = "John", Age = 30, IsActive = true };

            // Act
            GeneralUtilities.Copy(src, dest);

            // Assert
            Assert.Equal(src.Name, dest.Name);
            Assert.Equal(src.Age, dest.Age);
            Assert.Equal(src.IsActive, dest.IsActive);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void Copy_PartialSourceProperties_DestinationUpdatesOnlyThoseProperties()
        {
            // Arrange
            var src = new TestClass { Name = "John" };
            var dest = new TestClass { Name = "Jane", Age = 25, IsActive = false };

            // Act
            GeneralUtilities.Copy(src, dest);

            // Assert
            Assert.Equal(src.Name, dest.Name);
            Assert.NotEqual(25, dest.Age);  // Age should remain unchanged
            Assert.False(dest.IsActive); // IsActive should remain unchanged
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void Copy_SourceWithNullProperties_DestinationPropertiesBecomeNull()
        {
            // Arrange
            var src = new TestClass { Name = null, Age = 0, IsActive = false };
            var dest = new TestClass { Name = "John", Age = 30, IsActive = true };

            // Act
            GeneralUtilities.Copy(src, dest);

            // Assert
            Assert.Null(dest.Name);
            Assert.Equal(0, dest.Age);
            Assert.False(dest.IsActive);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void Copy_DestinationWithNoMatchingProperties_Throws()
        {
            // Arrange
            var src = new TestClass { Name = "John", Age = 30, IsActive = true };
            var dest = new { UnrelatedProperty = "Unrelated" };

            // Act & Assert
            var exception = Record.Exception(() => GeneralUtilities.Copy(src, dest));
            Assert.NotNull(exception);
        }
        #endregion

        #region InsertDataIntoOneTableFromAnotherTable
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void InsertDataIntoOneTableFromAnotherTable_NormalData_InsertsCorrectly()
        {
            // Arrange
            DataTable sourceTable = new DataTable();
            sourceTable.Columns.Add("SourceColumn1");
            sourceTable.Columns.Add("SourceColumn2");
            sourceTable.Columns.Add("SourceTickerSymbol"); // Add SourceTickerSymbol

            sourceTable.Rows.Add("Value1", "Value2", "Ticker1"); // Add Ticker1 value

            DataTable targetTable = new DataTable();
            targetTable.Columns.Add("TargetColumn1");
            targetTable.Columns.Add("TargetColumn2");
            targetTable.Columns.Add("TickerSymbol");

            // Define a primary key for the target table to avoid the "Missing Primary Key" exception
            targetTable.PrimaryKey = new DataColumn[] { targetTable.Columns["TargetColumn1"] };

            Dictionary<string, XmlNode> smMappingCOLList = new Dictionary<string, XmlNode>();
            XmlDocument doc = new XmlDocument();
            XmlNode node1 = doc.CreateElement("Mapping");
            XmlAttribute attr1 = doc.CreateAttribute("SMCOLName");
            attr1.Value = "TargetColumn1";
            node1.Attributes.Append(attr1);
            smMappingCOLList.Add("SourceColumn1", node1);

            XmlNode node2 = doc.CreateElement("Mapping");
            XmlAttribute attr2 = doc.CreateAttribute("SMCOLName");
            attr2.Value = "TargetColumn2";
            node2.Attributes.Append(attr2);
            smMappingCOLList.Add("SourceColumn2", node2);

            XmlNode node3 = doc.CreateElement("Mapping");
            XmlAttribute attr3 = doc.CreateAttribute("SMCOLName");
            attr3.Value = "TickerSymbol";
            node3.Attributes.Append(attr3);
            smMappingCOLList.Add("SourceTickerSymbol", node3);

            // Act
            GeneralUtilities.InsertDataIntoOneTableFromAnotherTable(sourceTable, targetTable, smMappingCOLList);

            // Assert
            Assert.Single(targetTable.Rows);
            Assert.Equal("Value1", targetTable.Rows[0]["TargetColumn1"]);
            Assert.Equal("Value2", targetTable.Rows[0]["TargetColumn2"]);
            Assert.Equal("Ticker1", targetTable.Rows[0]["TickerSymbol"]); // Check TickerSymbol value
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void InsertDataIntoOneTableFromAnotherTable_EmptySourceTable_NoRowsInserted()
        {
            // Arrange
            DataTable sourceTable = new DataTable();
            sourceTable.Columns.Add("SourceColumn1");
            sourceTable.Columns.Add("SourceColumn2");

            DataTable targetTable = new DataTable();
            targetTable.Columns.Add("TargetColumn1");
            targetTable.Columns.Add("TargetColumn2");
            targetTable.Columns.Add("TickerSymbol");

            Dictionary<string, XmlNode> smMappingCOLList = new Dictionary<string, XmlNode>();

            // Act
            GeneralUtilities.InsertDataIntoOneTableFromAnotherTable(sourceTable, targetTable, smMappingCOLList);

            // Assert
            Assert.Empty(targetTable.Rows);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void InsertDataIntoOneTableFromAnotherTable_MissingMapping_ColumnsNotInserted()
        {
            // Arrange
            DataTable sourceTable = new DataTable();
            sourceTable.Columns.Add("SourceColumn1");
            sourceTable.Columns.Add("SourceColumn2");
            sourceTable.Rows.Add("Value1", "Value2");

            DataTable targetTable = new DataTable();
            targetTable.Columns.Add("TargetColumn1");
            targetTable.Columns.Add("TargetColumn2");
            targetTable.Columns.Add("TickerSymbol");

            Dictionary<string, XmlNode> smMappingCOLList = new Dictionary<string, XmlNode>();

            // Act
            GeneralUtilities.InsertDataIntoOneTableFromAnotherTable(sourceTable, targetTable, smMappingCOLList);

            // Assert
            Assert.Empty(targetTable.Rows);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void InsertDataIntoOneTableFromAnotherTable_EmptyTickerSymbol_RowNotInserted()
        {
            // Arrange
            DataTable sourceTable = new DataTable();
            sourceTable.Columns.Add("SourceColumn1");
            sourceTable.Columns.Add("SourceColumn2");
            sourceTable.Rows.Add("Value1", "Value2");

            DataTable targetTable = new DataTable();
            targetTable.Columns.Add("TargetColumn1");
            targetTable.Columns.Add("TargetColumn2");
            targetTable.Columns.Add("TickerSymbol");

            // Define a primary key for the target table to avoid the "Missing Primary Key" exception
            targetTable.PrimaryKey = new DataColumn[] { targetTable.Columns["TargetColumn1"] };

            Dictionary<string, XmlNode> smMappingCOLList = new Dictionary<string, XmlNode>();
            XmlDocument doc = new XmlDocument();
            XmlNode node1 = doc.CreateElement("Mapping");
            XmlAttribute attr1 = doc.CreateAttribute("SMCOLName");
            attr1.Value = "TargetColumn1";
            node1.Attributes.Append(attr1);
            smMappingCOLList.Add("SourceColumn1", node1);

            XmlNode node2 = doc.CreateElement("Mapping");
            XmlAttribute attr2 = doc.CreateAttribute("SMCOLName");
            attr2.Value = "TargetColumn2";
            node2.Attributes.Append(attr2);
            smMappingCOLList.Add("SourceColumn2", node2);

            // Act
            GeneralUtilities.InsertDataIntoOneTableFromAnotherTable(sourceTable, targetTable, smMappingCOLList);

            // Assert
            Assert.Empty(targetTable.Rows);
        }
        #endregion

        #region CreateDataRowFromObject
        public class SampleObject
        {
            public long Symbol_PK { get; set; }
            public string Property1 { get; set; }
            public int Property2 { get; set; }
            public DateTime Property3 { get; set; }
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateDataRowFromObject_AllProperties()
        {
            // Arrange
            SampleObject obj = new SampleObject
            {
                Property1 = "Value1",
                Property2 = 123,
                Property3 = DateTime.Now
            };

            List<string> listOfProperties = null; // All properties of SampleObject

            // Act
            DataRow result = GeneralUtilities.CreateDataRowFromObject(obj, listOfProperties);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(obj.Property1, result["Property1"]);
            Assert.Equal(obj.Property2, int.Parse(result["Property2"].ToString()));
            Assert.Equal(DateTime.Parse(obj.Property3.ToString()), DateTime.Parse(result["Property3"].ToString()));
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateDataRowFromObject_SelectedProperties()
        {
            // Arrange
            SampleObject obj = new SampleObject
            {
                Property1 = "Value1",
                Property2 = 123,
                Property3 = DateTime.Now
            };

            List<string> listOfProperties = new List<string> { "Property1", "Property3" }; // Selected properties

            // Act
            DataRow result = GeneralUtilities.CreateDataRowFromObject(obj, listOfProperties);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(obj.Property1, result["Property1"]);
            Assert.Equal(DateTime.Parse(obj.Property3.ToString()), DateTime.Parse(result["Property3"].ToString()));

            // Ensure Property2 is not in the DataRow
            Assert.False(result.Table.Columns.Contains("Property2"));
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateDataRowFromObject_NullObject()
        {
            // Arrange
            SampleObject obj = null;
            List<string> listOfProperties = null; // All properties

            // Act
            DataRow result = GeneralUtilities.CreateDataRowFromObject(obj, listOfProperties);

            // Assert
            Assert.NotNull(result);
        }
        #endregion

        #region CreateTableStructureFromObject
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateTableStructureFromObject_ValidList()
        {
            // Arrange
            List<SampleObject> list = new List<SampleObject>
            {
                new SampleObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now },
                new SampleObject { Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1) }
            };

            // Act
            DataSet result = GeneralUtilities.CreateTableStructureFromObject(list);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Tables); // Ensure there is exactly one table in the DataSet
            DataTable dataTable = result.Tables[0];

            // Check column names and types
            Assert.Equal("Sheet1", dataTable.TableName);
            Assert.Equal(4, dataTable.Columns.Count); // Assuming SampleObject has 4 properties

            Assert.Contains("Property1", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            Assert.Contains("Property2", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            Assert.Contains("Property3", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateTableStructureFromObject_NullList()
        {
            // Arrange
            IList list = null;

            // Act
            DataSet result = GeneralUtilities.CreateTableStructureFromObject(list);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Tables); // Ensure no tables are added to the DataSet
        }
        #endregion

        #region FillDataSetFromCollection
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void FillDataSetFromCollection_ValidList()
        {
            // Arrange
            List<SampleObject> list = new List<SampleObject>
            {
                new SampleObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now },
                new SampleObject { Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1) }
            };

            DataSet ds = new DataSet();
            DataTable dataTable = new DataTable("Sheet1");
            dataTable.Columns.Add("Symbol_PK", typeof(int));
            dataTable.Columns.Add("Property1", typeof(string));
            dataTable.Columns.Add("Property2", typeof(int));
            dataTable.Columns.Add("Property3", typeof(DateTime));
            ds.Tables.Add(dataTable);

            bool nullForEmptyString = true; // Example: Treat empty strings as null
            bool stringValueForEnums = false; // Example: Convert enums to int values

            // Act
            GeneralUtilities.FillDataSetFromCollection(list, ref ds, nullForEmptyString, stringValueForEnums);

            // Assert
            Assert.NotNull(ds);
            Assert.Single(ds.Tables); // Ensure there is exactly one table in the DataSet
            DataTable resultTable = ds.Tables["Sheet1"]; // Access DataTable by name

            // Check data rows and columns
            Assert.Equal(2, resultTable.Rows.Count); // Check number of rows inserted
            Assert.Equal("Value1", resultTable.Rows[0]["Property1"]);
            Assert.Equal(1, resultTable.Rows[0]["Property2"]);
            Assert.Equal("Value2", resultTable.Rows[1]["Property1"]);
            Assert.Equal(2, resultTable.Rows[1]["Property2"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void FillDataSetFromCollection_NullList()
        {
            // Arrange
            List<SampleObject> list = null;
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());

            bool nullForEmptyString = true;
            bool stringValueForEnums = false;

            // Act
            GeneralUtilities.FillDataSetFromCollection(list, ref ds, nullForEmptyString, stringValueForEnums);

            // Assert
            Assert.NotNull(ds);
            Assert.Single(ds.Tables); // Ensure no new tables are added to the DataSet
            DataTable dataTable = ds.Tables[0];
            Assert.Empty(dataTable.Rows); // Ensure no rows are added
        }
        #endregion

        #region CreateDataSetFromCollection
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateDataSetFromCollection_ValidList_AllProperties()
        {
            // Arrange
            List<SampleObject> list = new List<SampleObject>
            {
                new SampleObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now },
                new SampleObject { Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1) }
            };

            // Act
            DataSet result = GeneralUtilities.CreateDataSetFromCollection(list, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Tables);
            DataTable dt = result.Tables[0];
            Assert.Equal(4, dt.Columns.Count); // Assuming SampleObject has 4 properties
            Assert.Equal(2, dt.Rows.Count); // Two objects in the list
                                            // Additional assertions on specific data values if needed
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateDataSetFromCollection_ValidList_SpecificColumns()
        {
            // Arrange
            List<SampleObject> list = new List<SampleObject>
            {
                new SampleObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now },
                new SampleObject { Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1) }
            };
            List<string> columns = new List<string> { "Property1", "Property3" };

            // Act
            DataSet result = GeneralUtilities.CreateDataSetFromCollection(list, columns);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Tables);
            DataTable dt = result.Tables[0];
            Assert.Equal(2, dt.Columns.Count); // Only 2 specified columns
            Assert.Equal(2, dt.Rows.Count); // Two objects in the list
                                            // Additional assertions on specific data values if needed
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateDataSetFromCollection_NullList()
        {
            // Act
            DataSet result = GeneralUtilities.CreateDataSetFromCollection(null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Tables);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateDataSetFromCollection_EmptyList()
        {
            // Arrange
            List<SampleObject> emptyList = new List<SampleObject>();

            // Act
            DataSet result = GeneralUtilities.CreateDataSetFromCollection(emptyList, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Tables);
            DataTable dt = result.Tables[0];
            Assert.Equal(4, dt.Columns.Count); // Assuming SampleObject has 4 properties
            Assert.Empty(dt.Rows); // No rows in the empty list
        }
        #endregion

        #region CreateDataSetFromCollectionWithHeaders
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateDataSetFromCollectionWithHeaders_ValidList_AllProperties()
        {
            // Arrange
            List<SampleObject> list = new List<SampleObject>
            {
                new SampleObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now },
                new SampleObject { Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1) }
            };

            // Act
            DataSet result = GeneralUtilities.CreateDataSetFromCollectionWithHeaders(list, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Tables);
            DataTable dt = result.Tables[0];
            Assert.Equal(4, dt.Columns.Count); // Assuming SampleObject has 4 properties
            Assert.Equal(2, dt.Rows.Count); // Two objects in the list
                                            // Additional assertions on specific data values if needed
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateDataSetFromCollectionWithHeaders_ValidList_SpecificColumnsWithSpecifiedNames()
        {
            // Arrange
            List<SampleObject> list = new List<SampleObject>
            {
                new SampleObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now },
                new SampleObject { Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1) }
            };
            Dictionary<string, string> columnsWithSpecifiedNames = new Dictionary<string, string>
            {
                { "Property1", "CustomName1" },
                { "Property3", "CustomName3" }
            };

            // Act
            DataSet result = GeneralUtilities.CreateDataSetFromCollectionWithHeaders(list, columnsWithSpecifiedNames);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Tables);
            DataTable dt = result.Tables[0];
            Assert.Equal(2, dt.Columns.Count); // Only 2 specified columns
            Assert.Equal(2, dt.Rows.Count); // Two objects in the list
            Assert.True(dt.Columns.Contains("CustomName1"));
            Assert.True(dt.Columns.Contains("CustomName3"));
            // Additional assertions on specific data values if needed
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateDataSetFromCollectionWithHeaders_NullList()
        {
            // Act
            DataSet result = GeneralUtilities.CreateDataSetFromCollectionWithHeaders(null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Tables);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateDataSetFromCollectionWithHeaders_EmptyList()
        {
            // Arrange
            List<SampleObject> emptyList = new List<SampleObject>();

            // Act
            DataSet result = GeneralUtilities.CreateDataSetFromCollectionWithHeaders(emptyList, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Tables);
            DataTable dt = result.Tables[0];
            Assert.Equal(4, dt.Columns.Count); // Assuming SampleObject has 4 properties
            Assert.Empty(dt.Rows); // No rows in the empty list
        }
        #endregion

        #region ArrangeTable
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ArrangeTable_EmptyCells_ReplacedWithAsterisk()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Column1");
            dt.Columns.Add("Column2");
            dt.Rows.Add("Value1", "");
            dt.Rows.Add(" ", "Value2");
            dt.Rows.Add("Value3", null);

            string expectedTableName = "TestTable";

            // Act
            DataTable result = GeneralUtilities.ArrangeTable(dt, expectedTableName);

            // Assert
            Assert.Equal(expectedTableName, result.TableName);
            Assert.Equal("*", result.Rows[0]["Column2"]);
            Assert.Equal("*", result.Rows[1]["Column1"]);
            Assert.Equal("*", result.Rows[2]["Column2"]);
            Assert.Equal("Value1", result.Rows[0]["Column1"]);
            Assert.Equal("Value2", result.Rows[1]["Column2"]);
            Assert.Equal("Value3", result.Rows[2]["Column1"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ArrangeTable_NoEmptyCells_NoReplacement()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Column1");
            dt.Columns.Add("Column2");
            dt.Rows.Add("Value1", "Value2");
            dt.Rows.Add("Value3", "Value4");

            string expectedTableName = "NonEmptyTable";

            // Act
            DataTable result = GeneralUtilities.ArrangeTable(dt, expectedTableName);

            // Assert
            Assert.Equal(expectedTableName, result.TableName);
            Assert.Equal("Value1", result.Rows[0]["Column1"]);
            Assert.Equal("Value2", result.Rows[0]["Column2"]);
            Assert.Equal("Value3", result.Rows[1]["Column1"]);
            Assert.Equal("Value4", result.Rows[1]["Column2"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ArrangeTable_MixedCells_ReplacementForEmptyCells()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Column1");
            dt.Columns.Add("Column2");
            dt.Rows.Add("Value1", " ");
            dt.Rows.Add("", "Value2");
            dt.Rows.Add("Value3", null);
            dt.Rows.Add(" ", "Value4");

            string expectedTableName = "MixedTable";

            // Act
            DataTable result = GeneralUtilities.ArrangeTable(dt, expectedTableName);

            // Assert
            Assert.Equal(expectedTableName, result.TableName);
            Assert.Equal("Value1", result.Rows[0]["Column1"]);
            Assert.Equal("*", result.Rows[0]["Column2"]);
            Assert.Equal("*", result.Rows[1]["Column1"]);
            Assert.Equal("Value2", result.Rows[1]["Column2"]);
            Assert.Equal("Value3", result.Rows[2]["Column1"]);
            Assert.Equal("*", result.Rows[2]["Column2"]);
            Assert.Equal("*", result.Rows[3]["Column1"]);
            Assert.Equal("Value4", result.Rows[3]["Column2"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ArrangeTable_AllEmptyCells_AllReplacedWithAsterisk()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Column1");
            dt.Columns.Add("Column2");
            dt.Rows.Add("", "");
            dt.Rows.Add(" ", " ");
            dt.Rows.Add(null, null);

            string expectedTableName = "EmptyTable";

            // Act
            DataTable result = GeneralUtilities.ArrangeTable(dt, expectedTableName);

            // Assert
            Assert.Equal(expectedTableName, result.TableName);
            Assert.Equal("*", result.Rows[0]["Column1"]);
            Assert.Equal("*", result.Rows[0]["Column2"]);
            Assert.Equal("*", result.Rows[1]["Column1"]);
            Assert.Equal("*", result.Rows[1]["Column2"]);
            Assert.Equal("*", result.Rows[2]["Column1"]);
            Assert.Equal("*", result.Rows[2]["Column2"]);
        }
        #endregion

        #region ReArrangeTable
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ReArrangeTable_AsteriskCells_ReplacedWithEmptyString()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Column1");
            dt.Columns.Add("Column2");
            dt.Rows.Add("*", "Value1");
            dt.Rows.Add("Value2", "*");

            // Act
            DataTable result = GeneralUtilities.ReArrangeTable(dt);

            // Assert
            Assert.Equal(string.Empty, result.Rows[0]["Column1"]);
            Assert.Equal("Value1", result.Rows[0]["Column2"]);
            Assert.Equal("Value2", result.Rows[1]["Column1"]);
            Assert.Equal(string.Empty, result.Rows[1]["Column2"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ReArrangeTable_NoAsteriskCells_NoReplacement()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Column1");
            dt.Columns.Add("Column2");
            dt.Rows.Add("Value1", "Value2");
            dt.Rows.Add("Value3", "Value4");

            // Act
            DataTable result = GeneralUtilities.ReArrangeTable(dt);

            // Assert
            Assert.Equal("Value1", result.Rows[0]["Column1"]);
            Assert.Equal("Value2", result.Rows[0]["Column2"]);
            Assert.Equal("Value3", result.Rows[1]["Column1"]);
            Assert.Equal("Value4", result.Rows[1]["Column2"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ReArrangeTable_MixedCells_ReplacementForAsteriskCells()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Column1");
            dt.Columns.Add("Column2");
            dt.Rows.Add("*", "Value1");
            dt.Rows.Add("Value2", "*");
            dt.Rows.Add("*", "*");

            // Act
            DataTable result = GeneralUtilities.ReArrangeTable(dt);

            // Assert
            Assert.Equal(string.Empty, result.Rows[0]["Column1"]);
            Assert.Equal("Value1", result.Rows[0]["Column2"]);
            Assert.Equal("Value2", result.Rows[1]["Column1"]);
            Assert.Equal(string.Empty, result.Rows[1]["Column2"]);
            Assert.Equal(string.Empty, result.Rows[2]["Column1"]);
            Assert.Equal(string.Empty, result.Rows[2]["Column2"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ReArrangeTable_AllAsteriskCells_AllReplacedWithEmptyString()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Column1");
            dt.Columns.Add("Column2");
            dt.Rows.Add("*", "*");
            dt.Rows.Add("*", "*");

            // Act
            DataTable result = GeneralUtilities.ReArrangeTable(dt);

            // Assert
            Assert.Equal(string.Empty, result.Rows[0]["Column1"]);
            Assert.Equal(string.Empty, result.Rows[0]["Column2"]);
            Assert.Equal(string.Empty, result.Rows[1]["Column1"]);
            Assert.Equal(string.Empty, result.Rows[1]["Column2"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ReArrangeTable_EmptyTable_NoExceptionThrown()
        {
            // Arrange
            DataTable dt = new DataTable();

            // Act
            DataTable result = GeneralUtilities.ReArrangeTable(dt);

            // Assert
            Assert.Empty(result.Columns);
            Assert.Empty(result.Rows);
        }
        #endregion

        #region CreateCollectionFromDataTable
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateCollectionFromDataTable_ValidDataTable_ReturnsCorrectCollection()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));
            dt.Columns.Add("Property3", typeof(DateTime));

            dt.Rows.Add("Value1", 1, DateTime.Now);
            dt.Rows.Add("Value2", 2, DateTime.Now.AddDays(1));

            // Act
            IList collection = GeneralUtilities.CreateCollectionFromDataTable(dt, typeof(SampleObject));

            // Assert
            Assert.NotNull(collection);
            Assert.Equal(2, collection.Count);

            var firstObject = collection[0] as SampleObject;
            Assert.NotNull(firstObject);
            Assert.Equal("Value1", firstObject.Property1);
            Assert.Equal(1, firstObject.Property2);
            Assert.Equal(dt.Rows[0]["Property3"], firstObject.Property3);

            var secondObject = collection[1] as SampleObject;
            Assert.NotNull(secondObject);
            Assert.Equal("Value2", secondObject.Property1);
            Assert.Equal(2, secondObject.Property2);
            Assert.Equal(dt.Rows[1]["Property3"], secondObject.Property3);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateCollectionFromDataTable_WithDBNullValues_ReturnsCorrectCollection()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));
            dt.Columns.Add("Property3", typeof(DateTime));

            dt.Rows.Add(DBNull.Value, 1, DateTime.Now);
            dt.Rows.Add("Value2", DBNull.Value, DateTime.Now.AddDays(1));

            // Act
            IList collection = GeneralUtilities.CreateCollectionFromDataTable(dt, typeof(SampleObject));

            // Assert
            Assert.NotNull(collection);
            Assert.Equal(2, collection.Count);

            var firstObject = collection[0] as SampleObject;
            Assert.NotNull(firstObject);
            Assert.Null(firstObject.Property1);
            Assert.Equal(1, firstObject.Property2);
            Assert.Equal(dt.Rows[0]["Property3"], firstObject.Property3);

            var secondObject = collection[1] as SampleObject;
            Assert.NotNull(secondObject);
            Assert.Equal("Value2", secondObject.Property1);
            Assert.Equal(0, secondObject.Property2); // default value for int
            Assert.Equal(dt.Rows[1]["Property3"], secondObject.Property3);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateCollectionFromDataTable_EnumProperty_CorrectlyParsed()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("EnumProperty", typeof(string));

            dt.Rows.Add("Value1");
            dt.Rows.Add("Value2");

            // Act
            IList collection = GeneralUtilities.CreateCollectionFromDataTable(dt, typeof(SampleObjectWithEnum));

            // Assert
            Assert.NotNull(collection);
            Assert.Equal(2, collection.Count);

            var firstObject = collection[0] as SampleObjectWithEnum;
            Assert.NotNull(firstObject);
            Assert.Equal(SampleEnum.Value1, firstObject.EnumProperty);

            var secondObject = collection[1] as SampleObjectWithEnum;
            Assert.NotNull(secondObject);
            Assert.Equal(SampleEnum.Value2, secondObject.EnumProperty);
        }

        public class SampleObjectWithEnum
        {
            public SampleEnum EnumProperty { get; set; }
        }

        public enum SampleEnum
        {
            Value1,
            Value2
        }
        #endregion

        #region CreateTableFromCollection
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateTableFromCollection_ValidList_AddsDataToTable()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));
            dt.Columns.Add("Property3", typeof(DateTime));

            List<SampleObject> list = new List<SampleObject>
            {
                new SampleObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now },
                new SampleObject { Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1) }
            };

            // Act
            DataTable result = GeneralUtilities.CreateTableFromCollection(dt, list);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Rows.Count); // Two objects in the list
            Assert.Equal("Value1", result.Rows[0]["Property1"]);
            Assert.Equal(1, result.Rows[0]["Property2"]);
            Assert.Equal(DateTime.Parse(list[0].Property3.ToString()), DateTime.Parse(result.Rows[0]["Property3"].ToString()));
            Assert.Equal("Value2", result.Rows[1]["Property1"]);
            Assert.Equal(2, result.Rows[1]["Property2"]);
            Assert.Equal(DateTime.Parse(list[1].Property3.ToString()), DateTime.Parse(result.Rows[1]["Property3"].ToString()));
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateTableFromCollection_EmptyList_ReturnsEmptyTable()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));
            dt.Columns.Add("Property3", typeof(DateTime));

            List<SampleObject> list = new List<SampleObject>();

            // Act
            DataTable result = GeneralUtilities.CreateTableFromCollection(dt, list);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Rows); // No rows should be added
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateTableFromCollection_NullList_ReturnsOriginalTable()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));
            dt.Columns.Add("Property3", typeof(DateTime));

            List<SampleObject> list = null;

            // Act
            DataTable result = GeneralUtilities.CreateTableFromCollection(dt, list);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Rows); // No rows should be added
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateTableFromCollection_IncompatibleTypes_ThrowsException()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));
            dt.Columns.Add("Property3", typeof(DateTime));

            // Creating a list with incompatible property types
            List<object> list = new List<object>
            {
                new { Property1 = "Value1", Property2 = "string_instead_of_int", Property3 = DateTime.Now },
                new { Property1 = "Value2", Property2 = 2, Property3 = "string_instead_of_DateTime" }
            };

            DataTable result = GeneralUtilities.CreateTableFromCollection(dt, list);
            // Act & Assert
            Assert.Empty(result.Rows);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateTableFromCollection_MissingProperties_HandlesGracefully()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));

            List<SampleObject> list = new List<SampleObject>
        {
            new SampleObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now },
            new SampleObject { Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1) }
        };

            // Act
            DataTable result = GeneralUtilities.CreateTableFromCollection(dt, list);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Rows.Count); // Two objects in the list
            Assert.Equal("Value1", result.Rows[0]["Property1"]);
            Assert.Equal(1, result.Rows[0]["Property2"]);
            Assert.Equal("Value2", result.Rows[1]["Property1"]);
            Assert.Equal(2, result.Rows[1]["Property2"]);
        }
        #endregion

        #region CreateCollectionFromDataTableForSecMaster
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateCollectionFromDataTableForSecMaster_EmptyDataTable_ReturnsEmptyCollection()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));
            dt.Columns.Add("Property3", typeof(DateTime));

            // Act
            IList result = GeneralUtilities.CreateCollectionFromDataTableForSecMaster(dt, typeof(SampleObject));

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // No rows should be added
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateCollectionFromDataTableForSecMaster_IncompatibleTypes_ThrowsException()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(string)); // Incompatible type
            dt.Columns.Add("Property3", typeof(DateTime));
            dt.Rows.Add("Value1", "string_instead_of_int", DateTime.Now);

            // Act & Assert
            Assert.Throws<FormatException>(() => GeneralUtilities.CreateCollectionFromDataTableForSecMaster(dt, typeof(SampleObject)));
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateCollectionFromDataTableForSecMaster_NullValues_SetsPropertiesToNull()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));
            dt.Columns.Add("Property3", typeof(DateTime));
            dt.Rows.Add(DBNull.Value, 1, DBNull.Value);

            // Act
            IList result = GeneralUtilities.CreateCollectionFromDataTableForSecMaster(dt, typeof(SampleObject));

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // One row in the DataTable

            var obj = result[0] as SampleObject;
            Assert.NotNull(obj);
            Assert.Null(obj.Property1);
            Assert.Equal(1, obj.Property2);
            Assert.Equal(DateTime.MinValue, obj.Property3);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateCollectionFromDataTableForSecMaster_ValidDateTimeParsing_ParsesCorrectly()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));
            dt.Columns.Add("Property3", typeof(string)); // Date as string
            dt.Rows.Add("Value1", 1, "2022-03-04");
            dt.Rows.Add("Value2", 2, "invalid_date");

            // Act
            IList result = GeneralUtilities.CreateCollectionFromDataTableForSecMaster(dt, typeof(SampleObject));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Two rows in the DataTable

            var obj1 = result[0] as SampleObject;
            Assert.NotNull(obj1);
            Assert.Equal(new DateTime(2022, 3, 4), obj1.Property3);

            var obj2 = result[1] as SampleObject;
            Assert.NotNull(obj2);
            Assert.Equal(DateTimeConstants.MinValue, obj2.Property3);
        }
        #endregion

        #region FindKeyByValue
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void FindKeyByValue_ValidMatch_ReturnsCorrectKey()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "Value1" },
                { 2, "Value2" },
                { 3, "Value3" }
            };

            // Act
            int resultKey = GeneralUtilities.FindKeyByValue(dictionary, "Value2");

            // Assert
            Assert.Equal(2, resultKey);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void FindKeyByValue_NoMatch_ThrowsException()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "Value1" },
                { 2, "Value2" },
                { 3, "Value3" }
            };

            // Act & Assert
            Assert.Throws<Exception>(() => GeneralUtilities.FindKeyByValue(dictionary, "Value4"));
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void FindKeyByValue_NullDictionary_ThrowsException()
        {
            // Arrange
            Dictionary<int, string> dictionary = null;

            // Act & Assert
            Assert.Throws<Exception>(() => GeneralUtilities.FindKeyByValue(dictionary, "Value1"));
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void FindKeyByValue_EmptyDictionary_ThrowsException()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            // Act & Assert
            Assert.Throws<Exception>(() => GeneralUtilities.FindKeyByValue(dictionary, "Value1"));
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void FindKeyByValue_NullValue_ThrowsArgumentNullException()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                { 1, "Value1" },
                { 2, "Value2" },
                { 3, "Value3" }
            };

            // Act & Assert
            Assert.Throws<Exception>(() => GeneralUtilities.FindKeyByValue(dictionary, null));
        }
        #endregion

        #region GetDataTableFromList
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetDataTableFromList_ValidList_ReturnsCorrectDataTable()
        {
            // Arrange
            List<SampleObject> data = new List<SampleObject>
            {
                new SampleObject { Symbol_PK = 0, Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now },
                new SampleObject { Symbol_PK = 0, Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1) }
            };

            // Act
            DataTable result = data.GetDataTableFromList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Columns.Count); // 3 properties in SampleObject

            Assert.Equal("Property1", result.Columns[1].ColumnName);
            Assert.Equal(typeof(string), result.Columns[1].DataType);
            Assert.Equal("Property2", result.Columns[2].ColumnName);
            Assert.Equal(typeof(int), result.Columns[2].DataType);
            Assert.Equal("Property3", result.Columns[3].ColumnName);
            Assert.Equal(typeof(DateTime), result.Columns[3].DataType);

            Assert.Equal(2, result.Rows.Count); // 2 items in the list

            Assert.Equal("Value1", result.Rows[0]["Property1"]);
            Assert.Equal(1, result.Rows[0]["Property2"]);
            Assert.Equal(data[0].Property3, result.Rows[0]["Property3"]);

            Assert.Equal("Value2", result.Rows[1]["Property1"]);
            Assert.Equal(2, result.Rows[1]["Property2"]);
            Assert.Equal(data[1].Property3, result.Rows[1]["Property3"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetDataTableFromList_EmptyList_ReturnsEmptyDataTable()
        {
            // Arrange
            List<SampleObject> data = new List<SampleObject>();

            // Act
            DataTable result = data.GetDataTableFromList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Columns.Count); // 3 properties in SampleObject
            Assert.Empty(result.Rows);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetDataTableFromList_NullList_ThrowsArgumentNullException()
        {
            // Arrange
            List<SampleObject> data = null;

            // Act & Assert
            DataTable result = GeneralUtilities.GetDataTableFromList(data);
            Assert.Equal(result.Rows.Count, 0);
        }
        #endregion

        #region GetDataTableFromList
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetDataTableFromList_ValidData_ReturnsCorrectDataTable()
        {
            // Arrange
            var data = new List<Tuple<string, int, DateTime>>
            {
                Tuple.Create("Value1", 1, DateTime.Now),
                Tuple.Create("Value2", 2, DateTime.Now.AddDays(1))
            };
            string t2Name = "CustomName2";
            string t3Name = "CustomName3";

            // Act
            DataTable result = data.GetDataTableFromList(t2Name, t3Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Columns.Count); // 3 properties in Tuple

            Assert.Equal("Length", result.Columns[0].ColumnName); // Default names for Tuple items
            Assert.Equal(typeof(int), result.Columns[0].DataType);
            Assert.Equal(t2Name, result.Columns[1].ColumnName);
            Assert.Equal(typeof(int), result.Columns[1].DataType);
            Assert.Equal(t3Name, result.Columns[2].ColumnName);
            Assert.Equal(typeof(DateTime), result.Columns[2].DataType);

            Assert.Equal(2, result.Rows.Count); // 2 items in the list

            Assert.Equal(6, int.Parse(result.Rows[0]["Length"].ToString()));
            Assert.Equal(1, result.Rows[0][t2Name]);
            Assert.Equal(data[0].Item3, result.Rows[0][t3Name]);

            Assert.Equal(6, int.Parse(result.Rows[1]["Length"].ToString()));
            Assert.Equal(2, result.Rows[1][t2Name]);
            Assert.Equal(data[1].Item3, result.Rows[1][t3Name]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetDataTableFromList_EmptyData_ReturnsEmptyDataTable()
        {
            // Arrange
            var data = new List<Tuple<string, int, DateTime>>();
            string t2Name = "CustomName2";
            string t3Name = "CustomName3";

            // Act
            DataTable result = data.GetDataTableFromList(t2Name, t3Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Columns.Count); // 3 properties in Tuple
            Assert.Empty(result.Rows);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void GetDataTableFromList_NullData_ReturnsEmptyDataTable()
        {
            // Arrange
            List<Tuple<string, int, DateTime>> data = null;
            string t2Name = "CustomName2";
            string t3Name = "CustomName3";

            // Act
            DataTable result = data.GetDataTableFromList(t2Name, t3Name);
            // Assert
            Assert.Empty(result.Rows);
        }
        #endregion

        #region CreateTableStructureFromObject
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateTableStructureFromObject_ValidList_AllColumns()
        {
            // Arrange
            IList list = new List<SampleObject>
            {
                new SampleObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now },
                new SampleObject { Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1) }
            };
            List<string> requiredColumns = null;

            // Act
            DataSet result = GeneralUtilities.CreateTableStructureFromObject(list, requiredColumns);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Tables);
            DataTable dt = result.Tables[0];
            Assert.Equal(4, dt.Columns.Count);
            Assert.Contains("Property1", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            Assert.Contains("Property2", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            Assert.Contains("Property3", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateTableStructureFromObject_ValidList_SpecifiedColumns()
        {
            // Arrange
            IList list = new List<SampleObject>
            {
                new SampleObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now },
                new SampleObject { Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1) }
            };
            List<string> requiredColumns = new List<string> { "Property1", "Property3" };

            // Act
            DataSet result = GeneralUtilities.CreateTableStructureFromObject(list, requiredColumns);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Tables);
            DataTable dt = result.Tables[0];
            Assert.Equal(2, dt.Columns.Count);
            Assert.Contains("Property1", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            Assert.Contains("Property3", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateTableStructureFromObject_NullList_ReturnsEmptyDataSet()
        {
            // Arrange
            IList list = null;
            List<string> requiredColumns = new List<string> { "Property1", "Property3" };

            // Act
            DataSet result = GeneralUtilities.CreateTableStructureFromObject(list, requiredColumns);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Tables);
            DataTable dt = result.Tables[0];
            Assert.Empty(dt.Columns);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void CreateTableStructureFromObject_EmptyRequiredColumns_ReturnsAllColumns()
        {
            // Arrange
            IList list = new List<SampleObject>
            {
                new SampleObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now },
                new SampleObject { Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1) }
            };
            List<string> requiredColumns = new List<string>();

            // Act
            DataSet result = GeneralUtilities.CreateTableStructureFromObject(list, requiredColumns);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Tables);
            DataTable dt = result.Tables[0];
            Assert.Equal(4, dt.Columns.Count);
            Assert.Contains("Property1", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            Assert.Contains("Property2", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            Assert.Contains("Property3", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
        }
        #endregion

        #region FillDataSetFromCollection
        public class TestObject
        {
            public string Property1 { get; set; }
            public int Property2 { get; set; }
            public DateTime Property3 { get; set; }
            public SampleEnum Property4 { get; set; }
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void FillDataSetFromCollection_ValidList_AllColumns_NoNullForEmptyString_NoStringValueForEnums()
        {
            // Arrange
            IList list = new List<TestObject>
            {
                new TestObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now, Property4 = SampleEnum.Value1 },
                new TestObject { Property1 = "", Property2 = 2, Property3 = DateTime.Now.AddDays(1), Property4 = SampleEnum.Value2 }
            };
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));
            dt.Columns.Add("Property3", typeof(DateTime));
            dt.Columns.Add("Property4", typeof(int)); // Enum as int
            ds.Tables.Add(dt);
            List<string> requiredColumns = new List<string> { "Property1", "Property2", "Property3", "Property4" };

            // Act
            GeneralUtilities.FillDataSetFromCollection(list, ref ds, false, false, requiredColumns);

            // Assert
            Assert.NotNull(ds);
            Assert.Single(ds.Tables);
            DataTable resultTable = ds.Tables[0];
            Assert.Equal(2, resultTable.Rows.Count);
            Assert.Equal("Value1", resultTable.Rows[0]["Property1"]);
            Assert.Equal(1, resultTable.Rows[0]["Property2"]);
            Assert.Equal((int)SampleEnum.Value1, resultTable.Rows[0]["Property4"]);
            Assert.Equal("", resultTable.Rows[1]["Property1"]);
            Assert.Equal(2, resultTable.Rows[1]["Property2"]);
            Assert.Equal((int)SampleEnum.Value2, resultTable.Rows[1]["Property4"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void FillDataSetFromCollection_ValidList_SpecifiedColumns_NullForEmptyString_StringValueForEnums()
        {
            // Arrange
            IList list = new List<TestObject>
            {
                new TestObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now, Property4 = SampleEnum.Value1 },
                new TestObject { Property1 = "", Property2 = 2, Property3 = DateTime.Now.AddDays(1), Property4 = SampleEnum.Value2 }
            };
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property3", typeof(DateTime));
            dt.Columns.Add("Property4", typeof(string)); // Enum as string
            ds.Tables.Add(dt);
            List<string> requiredColumns = new List<string> { "Property1", "Property3", "Property4" };

            // Act
            GeneralUtilities.FillDataSetFromCollection(list, ref ds, true, true, requiredColumns);

            // Assert
            Assert.NotNull(ds);
            Assert.Single(ds.Tables);
            DataTable resultTable = ds.Tables[0];
            Assert.Equal(2, resultTable.Rows.Count);
            Assert.Equal("Value1", resultTable.Rows[0]["Property1"]);
            Assert.Equal(SampleEnum.Value1.ToString(), resultTable.Rows[0]["Property4"]);
            Assert.Equal(DBNull.Value, resultTable.Rows[1]["Property1"]);
            Assert.Equal(SampleEnum.Value2.ToString(), resultTable.Rows[1]["Property4"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void FillDataSetFromCollection_NullList_ReturnsEmptyDataSet()
        {
            // Arrange
            IList list = null;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));
            dt.Columns.Add("Property3", typeof(DateTime));
            ds.Tables.Add(dt);
            List<string> requiredColumns = new List<string> { "Property1", "Property2", "Property3" };

            // Act
            GeneralUtilities.FillDataSetFromCollection(list, ref ds, false, false, requiredColumns);

            // Assert
            Assert.NotNull(ds);
            Assert.Single(ds.Tables);
            DataTable resultTable = ds.Tables[0];
            Assert.Empty(resultTable.Rows);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void FillDataSetFromCollection_EmptyRequiredColumns_ReturnsAllColumns()
        {
            // Arrange
            IList list = new List<TestObject>
            {
                new TestObject { Property1 = "Value1", Property2 = 1, Property3 = DateTime.Now, Property4 = SampleEnum.Value1 },
                new TestObject { Property1 = "Value2", Property2 = 2, Property3 = DateTime.Now.AddDays(1), Property4 = SampleEnum.Value2 }
            };
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Property1", typeof(string));
            dt.Columns.Add("Property2", typeof(int));
            dt.Columns.Add("Property3", typeof(DateTime));
            dt.Columns.Add("Property4", typeof(string)); // Enum as string
            ds.Tables.Add(dt);
            List<string> requiredColumns = new List<string>();

            // Act
            GeneralUtilities.FillDataSetFromCollection(list, ref ds, false, true, requiredColumns);

            // Assert
            Assert.NotNull(ds);
            Assert.Single(ds.Tables);
            DataTable resultTable = ds.Tables[0];
            Assert.Equal(2, resultTable.Rows.Count);
            Assert.Equal(string.Empty, resultTable.Rows[0]["Property1"].ToString());
            Assert.Equal(string.Empty, resultTable.Rows[0]["Property2"].ToString());
            Assert.Equal(string.Empty, resultTable.Rows[0]["Property4"].ToString());
            Assert.Equal(string.Empty, resultTable.Rows[1]["Property1"].ToString());
            Assert.Equal(string.Empty, resultTable.Rows[1]["Property2"].ToString());
            Assert.Equal(string.Empty, resultTable.Rows[1]["Property4"].ToString());
        }
        #endregion

        #region ChangeColumnDataType
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ChangeColumnDataType_ValidColumn_ChangeToInt()
        {
            // Arrange
            DataTable table = new DataTable();
            table.Columns.Add("Column1", typeof(string));
            table.Rows.Add("1");
            table.Rows.Add("2");

            // Act
            bool result = GeneralUtilities.ChangeColumnDataType(table, "Column1", typeof(int));

            // Assert
            Assert.True(result);
            Assert.Equal(typeof(int), table.Columns["Column1"].DataType);
            Assert.Equal(1, table.Rows[0]["Column1"]);
            Assert.Equal(2, table.Rows[1]["Column1"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ChangeColumnDataType_ColumnDoesNotExist_ReturnsFalse()
        {
            // Arrange
            DataTable table = new DataTable();
            table.Columns.Add("Column1", typeof(string));
            table.Rows.Add("1");

            // Act
            bool result = GeneralUtilities.ChangeColumnDataType(table, "NonExistentColumn", typeof(int));

            // Assert
            Assert.False(result);
            Assert.Equal(typeof(string), table.Columns["Column1"].DataType);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ChangeColumnDataType_ColumnAlreadySameType_ReturnsTrue()
        {
            // Arrange
            DataTable table = new DataTable();
            table.Columns.Add("Column1", typeof(int));
            table.Rows.Add(1);

            // Act
            bool result = GeneralUtilities.ChangeColumnDataType(table, "Column1", typeof(int));

            // Assert
            Assert.True(result);
            Assert.Equal(typeof(int), table.Columns["Column1"].DataType);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ChangeColumnDataType_ValidColumn_ChangeToDateTime()
        {
            // Arrange
            DataTable table = new DataTable();
            table.Columns.Add("Column1", typeof(string));
            table.Rows.Add("2022-01-01");
            table.Rows.Add("2023-01-01");

            // Act
            bool result = GeneralUtilities.ChangeColumnDataType(table, "Column1", typeof(DateTime));

            // Assert
            Assert.True(result);
            Assert.Equal(typeof(DateTime), table.Columns["Column1"].DataType);
            Assert.Equal(new DateTime(2022, 1, 1), table.Rows[0]["Column1"]);
            Assert.Equal(new DateTime(2023, 1, 1), table.Rows[1]["Column1"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void ChangeColumnDataType_NullableColumn_ChangeToInt()
        {
            // Arrange
            DataTable table = new DataTable();
            table.Columns.Add("Column1", typeof(string));
            table.Rows.Add("1");
            table.Rows.Add(DBNull.Value);

            // Act
            bool result = GeneralUtilities.ChangeColumnDataType(table, "Column1", typeof(int));

            // Assert
            Assert.True(result);
            Assert.Equal(typeof(int), table.Columns["Column1"].DataType);
            Assert.Equal(1, table.Rows[0]["Column1"]);
            Assert.Equal(DBNull.Value, table.Rows[1]["Column1"]);
        }
        #endregion

        #region SortDictionaryByValues
        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void SortDictionaryByValues_DictionaryIsNull_ReturnsEmptyDictionary()
        {
            // Arrange
            Dictionary<string, int> dictionary = null;

            // Act
            var result = GeneralUtilities.SortDictionaryByValues(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void SortDictionaryByValues_EmptyDictionary_ReturnsEmptyDictionary()
        {
            // Arrange
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            // Act
            var result = GeneralUtilities.SortDictionaryByValues(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void SortDictionaryByValues_SingleElementDictionary_ReturnsSameDictionary()
        {
            // Arrange
            Dictionary<string, int> dictionary = new Dictionary<string, int> { { "a", 1 } };

            // Act
            var result = GeneralUtilities.SortDictionaryByValues(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result["a"]);
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void SortDictionaryByValues_MultipleElementsDictionary_ReturnsSortedDictionary()
        {
            // Arrange
            Dictionary<string, int> dictionary = new Dictionary<string, int>
            {
                { "b", 2 },
                { "a", 1 },
                { "c", 3 }
            };

            // Act
            var result = GeneralUtilities.SortDictionaryByValues(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            var expectedOrder = new List<string> { "a", "b", "c" };
            Assert.Equal(expectedOrder, result.Keys.ToList());
        }

        [Fact]
        [Trait("Prana.Utilities", "GeneralUtilities")]
        public void SortDictionaryByValues_DictionaryWithDuplicateValues_ReturnsSortedDictionary()
        {
            // Arrange
            Dictionary<string, int> dictionary = new Dictionary<string, int>
            {
                { "a", 1 },
                { "b", 2 },
                { "c", 1 }
            };

            // Act
            var result = GeneralUtilities.SortDictionaryByValues(dictionary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            var expectedOrder = new List<string> { "a", "c", "b" };
            Assert.Equal(expectedOrder, result.Keys.ToList());
        }
        #endregion
    }
}
