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
    public class ShowMasterFundAsClient : PreferencesUIMap, ITestStep
    {

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
               
                SQLQueriesConstants.setCheckMasterFundAsClient(testData.Tables[sheetIndexToName[0]].Rows[0]["ShowMasterFundAsClient"].ToString());
                SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateClientCheckBoxValue);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ShowMasterFundAsClient");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;

        }
    }
}
