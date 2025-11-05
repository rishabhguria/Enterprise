using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Prana.BusinessObjects;
using Prana.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class SqlParserTests
    {
        [Fact]
        [Trait("Prana.Utilities", "SqlParser")]
        public void GetDynamicConditionQuery_WithEmptyConditions_ReturnsEmptyString()
        {
            // Arrange
            var dictCustomConditions = new Dictionary<string, List<CustomCondition>>();

            // Act
            var result = SqlParser.GetDynamicConditionQuerry(dictCustomConditions);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        [Trait("Prana.Utilities", "SqlParser")]
        public void GetDynamicConditionQuery_WithSingleEqualsCondition_ReturnsCorrectQuery()
        {
            // Arrange
            var conditions = new List<CustomCondition>
            {
                new CustomCondition
                {
                    CompareValue = "TestValue",
                    ConditionOperatorType = BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.Equals
                }
            };
            var dictCustomConditions = new Dictionary<string, List<CustomCondition>>
            {
                { "ColumnName", conditions }
            };

            // Act
            var result = SqlParser.GetDynamicConditionQuerry(dictCustomConditions);

            // Assert
            Assert.Equal("AND(ColumnName LIKE 'TestValue')", result);
        }

        [Fact]
        [Trait("Prana.Utilities", "SqlParser")]
        public void GetDynamicConditionQuery_WithMultipleConditions_ReturnsCorrectQuery()
        {
            // Arrange
            var conditions = new List<CustomCondition>
            {
                new CustomCondition
                {
                    CompareValue = "Value1",
                    ConditionOperatorType = BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.Contains
                },
                new CustomCondition
                {
                    CompareValue = "Value2",
                    ConditionOperatorType = BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.Like
                },
                new CustomCondition
                {
                    CompareValue = "a",
                    ConditionOperatorType = BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.StartsWith
                },
                new CustomCondition
                {
                    CompareValue = "z",
                    ConditionOperatorType = BusinessObjects.AppConstants.EnumDescriptionAttribute.ConditionOperator.DoesNotEndWith
                }
            };
            var dictCustomConditions = new Dictionary<string, List<CustomCondition>>
            {
                { "ColumnName", conditions }
            };

            // Act
            var result = SqlParser.GetDynamicConditionQuerry(dictCustomConditions);

            // Assert
            Assert.Equal("AND(ColumnName LIKE '%Value1%' OR ColumnName LIKE 'Value2%' OR ColumnName LIKE 'a%' AND ColumnName NOT LIKE '%z')", result);
        }
    }
}
