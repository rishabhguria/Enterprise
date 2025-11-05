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
    public class SetMarketDataProvider : ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                DataTable dt = testData.Tables[sheetIndexToName[0]];
                foreach (DataRow dr in dt.Rows)
                {
                    int val = 0;
                    string marketDataType = dr[TestDataConstants.COL_MARKETTYPE].ToString();
                    if (dr.Table.Columns.Contains(TestDataConstants.COL_BLOCKDATA))
                    {
                        string IsMarketDataBlocked = dr[TestDataConstants.COL_BLOCKDATA].ToString();
                        int B_data = IsMarketDataBlocked.ToUpper().Equals("TRUE") ? 1 : 0;

                        switch (marketDataType)
                        {
                            case "ACTIV":
                                val = 9;
                                break;
                            case "FactSet":
                                val = 8;
                                break;
                            case "SAPI":
                                val = 1;
                                break;
                            default:
                                val = 0;
                                break;
                        }
                        SQLQueriesConstants.UpdateMarketType(val, B_data);
                    }
                    else
                    {
                        switch (marketDataType)
                        {
                            case "ACTIV":
                                val = 9;
                                break;
                            case "FactSet":
                                val = 8;
                                break;
                            default:
                                val = 0;
                                break;
                        }
                    }
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
