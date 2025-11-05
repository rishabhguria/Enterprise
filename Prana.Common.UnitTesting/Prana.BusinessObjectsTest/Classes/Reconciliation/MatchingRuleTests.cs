using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using System.Data;
using System.Text;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.Reconciliation
{
    public class MatchingRuleTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "MatchingRule")]
        public void Add_ShouldAddRuleParameterToCollections()
        {
            // Arrange
            var matchingRule = MatchingRule.GetInstance();
            var ruleParameter = new RuleParameters
            {
                FieldName = "TestField",
                Type = ComparisionType.Numeric,
                IsIncluded = true
            };

            // Act
            matchingRule.Add("TestField", ruleParameter);

            // Assert
            var ruleFieldCollection = matchingRule.GetRuleFieldCollection();
            var includedRuleFieldCollection = matchingRule.GetInculdedRuleFieldCollection();

            Assert.True(ruleFieldCollection.ContainsKey("TestField"));
            Assert.Equal(ruleParameter, ruleFieldCollection["TestField"]);
            Assert.True(includedRuleFieldCollection.ContainsKey("TestField"));
            Assert.Equal(ruleParameter, includedRuleFieldCollection["TestField"]);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MatchingRule")]
        public void ValidateMatchingRules_ShouldReturnExpectedMessage_WhenColumnsMissing()
        {
            // Arrange
            var dtAppData = new DataTable();
            var dtPBData = new DataTable();
            dtAppData.Columns.Add("ExistingColumn");
            dtPBData.Columns.Add("DifferentColumn");

            var matchingRule = MatchingRule.GetInstance();
            var ruleParameter = new RuleParameters
            {
                FieldName = "TestField",
                Type = ComparisionType.Numeric,
                IsIncluded = true
            };
            matchingRule.Add("TestField", ruleParameter);

            // Act
            StringBuilder validationMessage = matchingRule.ValidateMatchingRules(dtAppData, dtPBData);

            // Assert
            Assert.Contains("Matching Rule TestField not available in Nirvana Data", validationMessage.ToString());
            Assert.Contains("Matching Rule TestField not available in PB Data", validationMessage.ToString());
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "MatchingRule")]
        public void ValidateMatchingRules_ShouldReturnEmptyMessage_WhenColumnsPresent()
        {
            // Arrange
            var dtAppData = new DataTable();
            var dtPBData = new DataTable();
            dtAppData.Columns.Add("TestField");
            dtPBData.Columns.Add("TestField");

            var matchingRule = MatchingRule.GetInstance();
            var ruleParameter = new RuleParameters
            {
                FieldName = "TestField",
                Type = ComparisionType.Numeric,
                IsIncluded = true
            };
            matchingRule.Add("TestField", ruleParameter);

            // Act
            StringBuilder validationMessage = matchingRule.ValidateMatchingRules(dtAppData, dtPBData);

            // Assert
            Assert.Equal(string.Empty, validationMessage.ToString());
        }
    }
}
