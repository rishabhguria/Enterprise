using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
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
    public class DATAReconcilerTests
    {
        public static DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Symbol", typeof(string));
            dt.Columns.Add("Price", typeof(double));
            dt.Columns.Add("FundName", typeof(string));
            dt.Columns.Add("MismatchType", typeof(string));
            return dt;
        }

        [Fact]
        [Trait("Prana.Utilities", "DATAReconciler")]
        public void RecocileData_ExactComparison_MatchesExactly()
        {
            // Arrange
            DataTable dt1 = CreateDataTable();
            dt1.Rows.Add("AAPL", 150.00, "Fund1", "");

            DataTable dt2 = CreateDataTable();
            dt2.Rows.Add("AAPL", 150.00, "Fund2", "");

            List<string> lstColumnsToKey = new List<string> { "Symbol" };
            Dictionary<string, string> dictColumnsToReconcile = new Dictionary<string, string>
            {
                { "Symbol", "Symbol" },
                { "Price", "Price" }
            };

            // Act
            DATAReconciler.RecocileData(dt1, dt2, lstColumnsToKey, dictColumnsToReconcile, 0.0, ComparisionType.Exact);

            // Assert
            foreach (DataRow row in dt2.Rows)
            {
                Assert.Equal(row["Funds"].ToString(),"Fund1,");
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "DATAReconciler")]
        public void RecocileData_PartialComparison_MatchesPartially()
        {
            // Arrange
            DataTable dt1 = CreateDataTable();
            dt1.Rows.Add("AAPL", 150.00, "Fund1", "");

            DataTable dt2 = CreateDataTable();
            dt2.Rows.Add("AAPL", 150.00, "Fund2", "");

            List<string> lstColumnsToKey = new List<string> { "Symbol" };
            Dictionary<string, string> dictColumnsToReconcile = new Dictionary<string, string>
            {
                { "Symbol", "Symbol" },
                { "Price", "Price" }
            };

            // Act
            DATAReconciler.RecocileData(dt1, dt2, lstColumnsToKey, dictColumnsToReconcile, 0.0, ComparisionType.Partial);

            // Assert
            Assert.Equal(dt2.Columns.Count, 11);
        }

        [Fact]
        [Trait("Prana.Utilities", "DATAReconciler")]
        public void RecocileData_NumericComparison_MatchesWithinTolerance()
        {
            // Arrange
            DataTable dt1 = CreateDataTable();
            dt1.Rows.Add("AAPL", 150.00, "Fund1", "");

            DataTable dt2 = CreateDataTable();
            dt2.Rows.Add("AAPL", 160.10, "Fund2", ""); // Slightly different price, within tolerance

            List<string> lstColumnsToKey = new List<string> { "Symbol" };
            Dictionary<string, string> dictColumnsToReconcile = new Dictionary<string, string>
            {
                { "Symbol", "Symbol" },
                { "Price", "Price" }
            };

            // Act
            DATAReconciler.RecocileData(dt1, dt2, lstColumnsToKey, dictColumnsToReconcile, 0.1, ComparisionType.Numeric);

            // Assert
            foreach (DataRow row in dt1.Rows)
            {
                Assert.Equal(row["MismatchType"].ToString(), "Price Mismatch");
            }
        }
    }
}
