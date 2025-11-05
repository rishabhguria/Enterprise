using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public partial class DisableChecksideForCounterParty : PreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                DisableChecksideCounterParty(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "DisableChecksideCounterParty");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Method to disable checkside for selected counter parties or brokers.
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void DisableChecksideCounterParty(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            Dictionary<string, int> CounterPartyNameToId = CreateCounterPartyDictionary();
            try
            {
                IList<string> disableCheckSideBroker = new List<string>();
                int resultant_ID;
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    string counterPartyName = dr[TestDataConstants.COL_BROKER].ToString();
                    if (CounterPartyNameToId.TryGetValue(counterPartyName, out resultant_ID))
                    {
                        disableCheckSideBroker.Add(resultant_ID.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Could not find the specified key.");
                    }
                }

                string CounterPartyID = string.Join(",", disableCheckSideBroker);
                SQLQueriesConstants.setCounterPartyCheckSideValue(CounterPartyID);
                SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateCounterPartyCheckSideValueQuery);
               // Wait(5000);

            }

            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Dictionary created to identify ids corresponding to Counter Party Names.
        /// </summary>
        /// <returns></returns>
       
    }
}
