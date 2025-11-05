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
    public partial class DisableChecksideForAccount : PreferencesUIMap, ITestStep
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
                DisableChecksideAccount(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "DisableChecksideAccounts");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Method to disable checkside for selected accounts.
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void DisableChecksideAccount(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            Dictionary<string, int> AccountNameToId = CreateAccountDictionary();
            int resultant_ID;
            try
            {
                IList<string> disableCheckSideAccounts = new List<string>();

                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    string accountName = dr[TestDataConstants.COL_ACCOUNT_NAME].ToString();
                    if (AccountNameToId.TryGetValue(accountName, out resultant_ID))
                    {
                        disableCheckSideAccounts.Add(resultant_ID.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Could not find the specified key.");
                    }

                }

                string accountID = string.Join(",", disableCheckSideAccounts);
                SQLQueriesConstants.setAccountCheckSideValue(accountID);
                SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateAccountCheckSideValueQuery);
                //Wait(5000);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }


        /// <summary>
        /// Dictionary created to identify ids corresponding to Account Names.
        /// </summary>
       /* private Dictionary<string, int> CreateAccountDictionary()
        {
            Dictionary<string, int> AccountNameToId = new Dictionary<string, int>();
            AccountNameToId.Add("OFFSHORE", 1182);
            AccountNameToId.Add("LP C/O", 1183);
            AccountNameToId.Add("Allocation2", 1184);
            AccountNameToId.Add("Allocation3", 1185);
            AccountNameToId.Add("Allocation1", 1186);
            AccountNameToId.Add("rt", 1189);
            AccountNameToId.Add("Allocation4", 1190);

            return AccountNameToId;
        }*/
    }
}
