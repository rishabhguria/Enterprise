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
    class SetAuecExecutingBroker : ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                SQLQueriesConstants.SetAuecExecutingBrokerQuery(testData.Tables[sheetIndexToName[0]]);
                SqlUtilities.ExecuteQuery("update T_CompanyTTGeneralPreferences set CounterPartyID = '5'");
                /*This hard code to change Default broker is added so that If AUEC broker is mapped for 'MS' broker we can get
                 that it will be occuring because of AUEC mapping not because of Default broker which is MS broker
                 Refer mail:  Subject: Update on Changes to "SetAuecExecutingBroker" Step
                 */
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
