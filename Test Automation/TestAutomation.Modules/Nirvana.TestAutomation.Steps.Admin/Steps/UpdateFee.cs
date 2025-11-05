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
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.Admin
{
    class UpdateFee : ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                DataTable dt = testData.Tables[sheetIndexToName[0]];
                foreach (DataRow dr in dt.Rows)
                {
                    decimal fee = Convert.ToDecimal(dr[TestDataConstants.COL_Fees]);
                    string displayName = dr[TestDataConstants.COL_EXCHANGE_SYMBOL_NAME].ToString();
                    FeeType feetype = (FeeType)Enum.Parse(typeof(FeeType), dr[TestDataConstants.COL_FeeType].ToString());
                    switch(feetype)
                    {
                        case FeeType.SecFee:
                            feetype = FeeType.SecFee;
                            break;
                        case FeeType.OrfFee:
                            feetype = FeeType.OrfFee;
                            break;
                        case FeeType.OccFee:
                            feetype = FeeType.OccFee;
                            break;
                        case FeeType.ClearingFee:
                            feetype = FeeType.ClearingFee;
                            break;
                        case FeeType.MiscFees:
                            feetype = FeeType.MiscFees;
                            break;
                        case FeeType.StampDuty:
                            feetype = FeeType.StampDuty;
                            break;
                        case FeeType.TaxOnCommissions:
                            feetype = FeeType.TaxOnCommissions;
                            break;
                        case FeeType.TransactionLevy:
                            feetype = FeeType.TransactionLevy;
                            break;
                    }
                    SQLQueriesConstants.UpdateFee(fee, feetype, displayName);
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateSecFee);
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
