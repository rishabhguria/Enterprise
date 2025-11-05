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
    public  class SetRollover : ITestStep
    {
        /// <summary>
        /// Set Rollover schema
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                string assetName;
                string underlyingName;
                string exchangeSymbolName;
                string clearanceTime;
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    assetName = dr[TestDataConstants.COL_ASSET_NAME].ToString();
                    underlyingName = dr[TestDataConstants.COL_UNDERLYING_NAME].ToString();
                    exchangeSymbolName = dr[TestDataConstants.COL_EXCHANGE_SYMBOL_NAME].ToString();
                    //If user mention right now in schema then clearance time will be set as current EST time with added two minutes.
                    clearanceTime = dr[TestDataConstants.COL_CLEARANCE_TIME].ToString();
                    if (clearanceTime == "Right Now" && !ApplicationArguments.ProductDependency.ToLower().Equals("samsara"))
                    {
                        clearanceTime = DateTime.Now.AddMinutes(+2).ToString();
                    }
                    else if (clearanceTime == "Right Now" && ApplicationArguments.ProductDependency.ToLower().Equals("samsara")) 
                    {
                        //increasing rollover time for Samsara release
                        clearanceTime = DateTime.Now.AddMinutes(+7).ToString();
                    }
                    else
                    {
                        clearanceTime = DateTime.Now.AddMinutes(+Int32.Parse(clearanceTime)).ToString();
                    }
                    SQLQueriesConstants.passingSetRolloverSchemaData(clearanceTime, exchangeSymbolName, underlyingName, assetName);
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.clearanceTimeUPdateSQLQuery);
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
