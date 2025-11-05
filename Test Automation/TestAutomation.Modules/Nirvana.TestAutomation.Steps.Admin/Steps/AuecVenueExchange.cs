using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.BussinessObjects;
using System.Data;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Steps.Admin.Scripts;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.ComponentModel;
using System.Diagnostics;
namespace Nirvana.TestAutomation.Steps.Admin
{
    class AuecVenueExchange : ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                string VenueID1 = testData.Tables[0].Rows[0]["FromTableVenueID"].ToString();
                string VenueID2 = testData.Tables[0].Rows[0]["ToTableVenueID"].ToString();

                string sql = @"
                BEGIN TRANSACTION;

                WITH Temp AS (
                    SELECT VenueID, VenueName, VenueTypeID, Route, ExchangeID
                    FROM T_Venue
                    WHERE VenueID IN (" + VenueID1 + " , " + VenueID2 + @")
                )
                UPDATE t
                SET VenueName   = CASE WHEN t.VenueID = " + VenueID1 + @" THEN v2.VenueName   ELSE v1.VenueName   END,
                    VenueTypeID = CASE WHEN t.VenueID = " + VenueID1 + @" THEN v2.VenueTypeID ELSE v1.VenueTypeID END,
                    Route       = CASE WHEN t.VenueID = " + VenueID1 + @" THEN v2.Route       ELSE v1.Route       END,
                    ExchangeID  = CASE WHEN t.VenueID = " + VenueID1 + @" THEN v2.ExchangeID  ELSE v1.ExchangeID  END
                FROM T_Venue t
                CROSS JOIN (SELECT VenueName, VenueTypeID, Route, ExchangeID FROM Temp WHERE VenueID = " + VenueID1 + @") v1
                CROSS JOIN (SELECT VenueName, VenueTypeID, Route, ExchangeID FROM Temp WHERE VenueID = " + VenueID2 + @") v2
                WHERE t.VenueID IN (" + VenueID1 + @", " + VenueID2 + @");

                COMMIT;
                ";
                SqlUtilities.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }
    }
}
