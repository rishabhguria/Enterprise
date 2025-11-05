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
    public class ShowMasterFundOnTT : PreferencesUIMap, ITestStep
    {

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                //string CheckButtonMasterFundonTTValue;
                SQLQueriesConstants.setCheckButtonMasterFundonTTValue(testData.Tables[sheetIndexToName[0]].Rows[0]["ShowMasterFundOnTT"].ToString());
                SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateCheckBoxValueQuery);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ShowMasterFundOnTT");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
            
        }
    }
}
