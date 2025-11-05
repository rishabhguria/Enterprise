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
    public class IsImportOverrideOnShortLocate : ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                DataRow dr = testData.Tables[0].Rows[0];
                string isImportOverrideOnShortLocate = dr[TestDataConstants.COL_isImportOverrideOnShortLocate].ToString();
                SQLQueriesConstants.isImportOverrideOnShortLocate(isImportOverrideOnShortLocate);
                SqlUtilities.ExecuteQuery(SQLQueriesConstants.importOverrideOnShortLocate);
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
