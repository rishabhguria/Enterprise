using Prana.Utilities;
using System.Collections.Generic;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class DataTableExpressionHelperTests
    {
        [Fact]
        [Trait("Prana.Utilities", "DataTableExpressionHelper")]
        public void ConvertUGExpressionToDTExpression_IfClause_ReplacedWithIif()
        {
            // Arrange
            Dictionary<string, string> customColumns = new Dictionary<string, string>
            {
                { "Column1", "if(condition, trueValue, falseValue)" },
                { "Column2", "NoChangeExpected" }
            };
            Dictionary<string, string> expected = new Dictionary<string, string>
            {
                { "Column1", "iif(condition, truevalue, falsevalue)" }, // Note: The replacement makes the entire string lowercase
                { "Column2", "NoChangeExpected" }
            };

            // Act
            DataTableExpressionHelper.ConvertUGExpressionToDTExpression(customColumns);

            // Assert
            foreach (var key in expected.Keys)
            {
                Assert.True(customColumns.ContainsKey(key), $"Key '{key}' not found in the result dictionary.");
                Assert.Equal(expected[key], customColumns[key], ignoreCase: true);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "DataTableExpressionHelper")]
        public void ConvertUGExpressionToDTExpression_PassedDictionaryAsEmpty_ShouldNotUpdatecustomColumns()
        {
            // Arrange
            Dictionary<string, string> customColumns = new Dictionary<string, string>();
            Dictionary<string, string> expected = new Dictionary<string, string>();

            // Act
            DataTableExpressionHelper.ConvertUGExpressionToDTExpression(customColumns);

            // Assert
            Assert.Equal(expected, customColumns);
        }

        [Fact]
        [Trait("Prana.Utilities", "DataTableExpressionHelper")]
        public void ConvertUGExpressionToDTExpression_PassedDictionaryAsNull_ShouldNotUpdatecustomColumns()
        {
            // Arrange
            Dictionary<string, string> customColumns = null;
            Dictionary<string, string> expected = null;

            // Act
            DataTableExpressionHelper.ConvertUGExpressionToDTExpression(customColumns);

            // Assert
            Assert.Equal(expected, customColumns);
        }
    }
}
