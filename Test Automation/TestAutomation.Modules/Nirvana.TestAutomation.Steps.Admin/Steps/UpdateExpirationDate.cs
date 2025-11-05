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

namespace Nirvana.TestAutomation.Steps.Admin
{
    public class UpdateExpirationDate : ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                DataTable dt = testData.Tables[sheetIndexToName[0]];
                foreach (DataRow dr in dt.Rows)
                {
                    string tableName = "T_SMOptionData";
                    DateTime expirationDate = Convert.ToDateTime(dr[TestDataConstants.COL_Expiration_Date]);
                    if (dr.Table.Columns.Contains(TestDataConstants.COL_ASSET_CLASS) && dr[TestDataConstants.COL_ASSET_CLASS].ToString() != String.Empty)
                    {
                        //If need to handle more asset class than add those here
                        if (dr[TestDataConstants.COL_ASSET_CLASS].ToString().Equals("Future"))
                            tableName = "T_SMFutureData";
                    }
                    string symbol = dr[TestDataConstants.COL_Option].ToString();
                    SQLQueriesConstants.UpdateExpirationDate(expirationDate, symbol, tableName);
                    SqlUtilities.ExecuteQuerySM(SQLQueriesConstants.updateExpirationDate);
                }
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
