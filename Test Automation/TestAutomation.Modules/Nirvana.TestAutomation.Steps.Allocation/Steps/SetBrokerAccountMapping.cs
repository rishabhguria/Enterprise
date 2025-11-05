using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.TestDataProvider;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Data.SqlClient;
using System.Configuration;
using Nirvana.TestAutomation.Steps.Allocation.Scripts;
using Nirvana.TestAutomation.Interfaces;
using System.Data;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Nirvana.TestAutomation.Steps.Allocation.PreferencesUIMap" />
    /// <seealso cref="Nirvana.TestAutomation.Interfaces.ITestStep" />
    public class SetBrokerAccountMapping : PreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
          public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                SQLQueriesConstants.deleteCVAccount(testData.Tables[sheetIndexToName[0]]);
                SQLQueriesConstants.deleteCVAUEC(testData.Tables[sheetIndexToName[0]]);
                SQLQueriesConstants.insertBrokerAccountAuecValues(testData.Tables[sheetIndexToName[0]]);
               

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetBrokerAccountMapping");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;

        }
    }
}
