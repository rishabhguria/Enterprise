using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Steps.Allocation.Scripts;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public partial class SetAvgPriceRounding : PreferencesUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                SQLQueriesConstants.setAvgPriceRoundingValueQuery(testData.Tables[sheetIndexToName[0]].Rows[0]["AvgPriceRounding"].ToString());
                SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateAvgPriceRoundingValueQuery);

                SQLQueriesConstants.setRoundingDigitQuery(testData.Tables[sheetIndexToName[0]]);
                if (SQLQueriesConstants.updateRoundingDigitQuery != null)
                {
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateRoundingDigitQuery);
                }
                    
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetAvgPriceRounding");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
    }
}
