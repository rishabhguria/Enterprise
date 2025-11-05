using Prana.BusinessObjects;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class CAHelperClassTests
    {
        [Fact]
        [Trait("Prana.Utilities", "CAHelperClass")]
        public void GetClosingTaxlotFormatedID_ValidInput_FormatsCorrectly()
        {
            // Arrange
            var helper = CAHelperClass.GetInstance();
            var closingInfoList = new List<ClosingInfo>
            {
                new ClosingInfo
                {
                    ClosingID = "1",
                    PositionalTaxlotID = "Taxlot1",
                    ClosingTaxlotID = "Taxlot2",
                    ClosingTradeDate = new DateTime(2020, 1, 1).ToString()
                }
            };

            var sbClosingID = new StringBuilder();
            var sbPositionalandClosingTaxlotID = new StringBuilder();
            var taxlotClosingIDWithClosingDate = new StringBuilder();

            // Expected results based on the mock data
            var expectedSbClosingID = "1,";
            var expectedSbPositionalandClosingTaxlotID = "Taxlot1,Taxlot2,";
            var expectedTaxlotClosingIDWithClosingDate = "1_1/1/2020 00:00:00,";

            // Act
            helper.GetClosingTaxlotFormatedID(closingInfoList, ref sbClosingID, ref sbPositionalandClosingTaxlotID, ref taxlotClosingIDWithClosingDate);

            // Assert
            Assert.Equal(expectedSbClosingID, sbClosingID.ToString());
            Assert.Equal(expectedSbPositionalandClosingTaxlotID, sbPositionalandClosingTaxlotID.ToString());
            Assert.Equal(expectedTaxlotClosingIDWithClosingDate, taxlotClosingIDWithClosingDate.ToString());
        }
    }
}
