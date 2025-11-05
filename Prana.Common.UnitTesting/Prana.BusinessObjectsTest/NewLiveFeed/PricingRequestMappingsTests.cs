using Prana.BusinessObjects.NewLiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.NewLiveFeed
{
    public class PricingRequestMappingsTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "PricingRequestMappings")]
        public void CreateDataTableAndKeyMappingFromReq_CreatesDataTableWithCorrectStructure()
        {
            // Arrange
            var fields = new List<string> { "Field1", "Field2" };
            var secMasterRequest = new SecMasterRequestObj();
            string requestId = "123";
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(1);
            var pricingRequest = new PricingRequestMappings(requestId, fields, secMasterRequest, startDate, endDate, null, "SecondarySource", true);

            // Act
            DataTable resultTable = pricingRequest.CreateDataTableAndKeyMappingFromReq();

            // Assert
            Assert.NotNull(resultTable);
            Assert.True(resultTable.Columns.Contains("Field1"));
            Assert.True(resultTable.Columns.Contains("Field2"));
            Assert.True(resultTable.Columns.Contains("Symbol"));
            Assert.True(resultTable.Columns.Contains("Date"));
            Assert.True(resultTable.Columns.Contains("Symbology"));
            Assert.True(resultTable.Columns.Contains("SymbolPK"));
            Assert.True(resultTable.Columns.Contains("DataSource"));
            Assert.True(resultTable.Columns.Contains("SecondarySource"));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PricingRequestMappings")]
        public void CreateDataTableAndKeyMappingFromReq_CreatesDataTableWithEmptyStructure()
        {
            // Arrange
            var fields = new List<string>();
            var secMasterRequest = new SecMasterRequestObj();
            string requestId = String.Empty;
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(1);
            var pricingRequest = new PricingRequestMappings(requestId, fields, secMasterRequest, startDate, endDate, null, "SecondarySource", true);

            // Act
            DataTable resultTable = pricingRequest.CreateDataTableAndKeyMappingFromReq();

            // Assert
            Assert.NotNull(resultTable);
            Assert.True(resultTable.Columns.Contains("Symbol"));
            Assert.True(resultTable.Columns.Contains("Date"));
            Assert.True(resultTable.Columns.Contains("Symbology"));
            Assert.True(resultTable.Columns.Contains("SymbolPK"));
            Assert.True(resultTable.Columns.Contains("DataSource"));
            Assert.True(resultTable.Columns.Contains("SecondarySource"));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "PricingRequestMappings")]
        public void MergeBloombergRequestOnFields_MergesCorrectly()
        {
            // Arrange
            string requestId1 = "1";
            string requestId2 = "2";
            var fields1 = new List<string> { "Field1", "Field2" };
            var fields2 = new List<string> { "Field3", "Field4" };
            var secMasterRequest = new SecMasterRequestObj();

            var pricingRequest1 = new PricingRequestMappings(requestId1, fields1, secMasterRequest, DateTime.Today, DateTime.Today.AddDays(1), null, "Source1", true);
            var pricingRequest2 = new PricingRequestMappings(requestId2, fields2, secMasterRequest, DateTime.Today, DateTime.Today.AddDays(1), null, "Source1", true);

            pricingRequest1.BBRequestIds.TryAdd(requestId1, pricingRequest1);
            pricingRequest1.BBRequestIds.TryAdd(requestId2, pricingRequest2);

            // Act
            pricingRequest1.MergeBloombergRequestOnFields();

            // Assert
            Assert.Equal(4, pricingRequest2.FieldNames.Count);
            Assert.Empty(pricingRequest1.BBRequestIdsInProcess);
        }
    }
}

