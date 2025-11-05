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
    public class SetShortLocateStructures : ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                
                DataRow dr = testData.Tables[0].Rows[0];
                if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_isShowmasterFundOnShortLocate].ToString()))
                {
                    string IsShowmasterFundOnShortLocate = dr[TestDataConstants.COL_isShowmasterFundOnShortLocate].ToString();
                    SQLQueriesConstants.IsShowmasterFundOnShortLocate(IsShowmasterFundOnShortLocate);
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.ShortLocateStructures);
                }

                if (testData.Tables[0].Columns.Contains(TestDataConstants.IsImportOverrideOnShortLocate) && !string.IsNullOrEmpty(dr[TestDataConstants.IsImportOverrideOnShortLocate].ToString()))
                {
                    string IsImportOverrideOnShortLocate = dr[TestDataConstants.IsImportOverrideOnShortLocate].ToString();
                    SQLQueriesConstants.IsImportOverrideOnShortLocate(IsImportOverrideOnShortLocate);
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.ShortLocateStructures);
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
