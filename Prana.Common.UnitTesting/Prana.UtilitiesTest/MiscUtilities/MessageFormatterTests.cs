using Prana.BusinessObjects.Compliance.Alerting;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class MessageFormatterTests
    {
        [Fact]
        public void FormatToOverrideMessage_DataSetValid_SetsCorrectMessage()
        {
            // Arrange
            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("Summary", typeof(string));
            table.Columns.Add("validationTime", typeof(string));
            table.Columns.Add("compressionLevel", typeof(string));
            table.Columns.Add("parameters", typeof(string));
            table.Columns.Add("name", typeof(string));

            table.Rows.Add("Summary Text", "Validation Time", "Compression Level", "Parameters", "RuleName");

            dataSet.Tables.Add(table);
            string expectedMessage = "This trade has been blocked by pre trade compliance.\nRule Summary: Summary Text\n\nValidation Time: Validation Time\n\nCompression Level: Compression Level\n\nCurrent Parameter: Parameters\n\nRule Violated: RuleName\n\nDo you want to ALLOW this trade?";

            // Act
            MessageFormatter.FormatToOverrideMessage(dataSet, out string actualMessage);

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void FormatToOverrideMessage_DataSetInvalid_SetsErrorMessage()
        {
            // Arrange
            object data = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => MessageFormatter.FormatToOverrideMessage(data, out string actualMessage));
        }

        [Fact]
        public void FormatToOverrrideResponseMessage_ValidDataSet_AddsIsAllowedColumn()
        {
            // Arrange
            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("Summary", typeof(string));
            table.Rows.Add("Summary Text");
            dataSet.Tables.Add(table);
            bool isAllowed = true;

            // Act
            var result = MessageFormatter.FormatToOverrrideResponseMessage(dataSet, isAllowed);

            // Assert
            Assert.True(result.Tables[0].Columns.Contains("IsAllowed"));
            Assert.Equal(isAllowed, (bool)result.Tables[0].Rows[0]["IsAllowed"]);
        }

        [Fact]
        public void FormatNotificationMessage_ValidDataSet_ReturnsFormattedMessage()
        {
            // Arrange
            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("compressionLevel", typeof(string));
            table.Columns.Add("validationTime", typeof(string));
            table.Columns.Add("description", typeof(string));
            table.Columns.Add("parameters", typeof(string));
            table.Columns.Add("Summary", typeof(string));
            table.Columns.Add("Status", typeof(string));

            table.Rows.Add("Rule Name", "Compression Level", "Validation Time", "Description", "Parameters", "Summary", "Status");
            dataSet.Tables.Add(table);

            string expectedMessage = "<br/><b>Rule validated:</b> Rule Name\r\n<b>Compliance Level:</b> Compression Level\r\n<b>Validation Time:</b> Validation Time\r\n<b>Rule Description:</b> Description\r\n<b>Current values:</b> Parameters\r\n<b>Rule Summary:</b> Summary\r\n<b>Action applied:</b> Status\r\n";

            // Act
            var actualMessage = MessageFormatter.FormatNotificationMessage(dataSet);

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void FormatAlertDescription_ValidDataSet_ReturnsFormattedMessage()
        {
            // Arrange
            var dataSet = new DataSet();
            var table = new DataTable();
            table.Columns.Add("compressionLevel", typeof(string));
            table.Columns.Add("validationTime", typeof(string));
            table.Columns.Add("summary", typeof(string));
            table.Columns.Add("parameters", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("complianceOfficerNotes", typeof(string));

            table.Rows.Add("Compression Level", "Validation Time", "Summary", "Parameters", "Status", "Compliance Officer Notes");
            dataSet.Tables.Add(table);

            string expectedMessage = "Compliance Level: Compression Level\r\n\r\nLast Validation Time: Validation Time\r\n\r\nRule Description: Summary\r\n\r\nCurrent values: Parameters\r\n\r\nCompliance Officer Notes: Compliance Officer Notes\r\n";

            // Act
            var actualMessage = MessageFormatter.FormatAlertDescription(dataSet);

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void FormatRuleNameForGuvnor_ValidRuleName_ReturnsFormattedRuleName()
        {
            // Arrange
            string ruleName = "Rule Name with %";
            string expected = "Rule(20)Name(20)with(20)(25)";

            // Act
            var actual = MessageFormatter.FormatRuleNameForGuvnor(ruleName);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormatRuleNameForDisplay_ValidRuleName_ReturnsFormattedRuleName()
        {
            // Arrange
            string ruleName = "Rule(20)Name(20)with(20)(25)";
            string expected = "Rule Name with %";

            // Act
            var actual = MessageFormatter.FormatRuleNameForDisplay(ruleName);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormatToOverrideMessage_ValidAlert_ReturnsFormattedMessage()
        {
            // Arrange
            var alert = new Alert
            {
                Summary = "Summary",
                ValidationTime = DateTime.Now,
                CompressionLevel = "Compression Level",
                Parameters = "Parameters",
                RuleName = "Rule Name"
            };

            string expectedMessage = $"\nRule Summary: {alert.Summary}\n\nValidation Time: {alert.ValidationTime}\n\nCompression Level: {alert.CompressionLevel}\n\nCurrent Parameter: {alert.Parameters}\n\nRule Violated: {alert.RuleName}\n";

            // Act
            var actualMessage = MessageFormatter.FormatToOverrideMessage(alert);

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }
    }
}
