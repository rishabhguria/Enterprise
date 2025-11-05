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
    class DisableCheckside : PreferencesUIMap, ITestStep
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
                string checkSideValue= ValidateCheckside(testData, sheetIndexToName);
                string assetID=DisableChecksideAsset(testData, sheetIndexToName);
                string accountID=DisableChecksideAccount(testData, sheetIndexToName);
                string counterPartyID=DisableChecksideCounterParty(testData, sheetIndexToName);

                SQLQueriesConstants.setAllocationPreferences(assetID, accountID, counterPartyID, checkSideValue);
                SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateAllocationPreferencesQuery);
              //  Wait(5000);
                
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "DisableCheckside");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }


        /// <summary>
        /// Method to set the value of Validate Checkside Checkbox.
        /// </summary>
        /// <param name="dtRow"></param>
        /// <param name="sheetIndexToName"></param>
        private string ValidateCheckside(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {          
            try
            {
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    string checkSideValue = dr[TestDataConstants.COL_VALIDATE_CHECKSIDE].ToString();
                    if (!string.IsNullOrEmpty(checkSideValue))
                    {
                        return checkSideValue;                                          
                    }                                 
                }                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return "";
        }


        /// <summary>
        /// Method to disable checkside for selected assets.
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private string DisableChecksideAsset(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            Dictionary<string, int> AssetNameToId = CreateAssetDictionary();
            int resultant_ID;
            string assetID=null;
            try
            {
                IList<string> disableCheckSideAssets = new List<string>();
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    string assetName = dr[TestDataConstants.COL_ASSET].ToString();
                    if (AssetNameToId.TryGetValue(assetName, out resultant_ID))
                    {
                        disableCheckSideAssets.Add(resultant_ID.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Could not find the specified key.");
                    }
                }              
                    assetID = string.Join(",", disableCheckSideAssets);
                    return assetID;                                  
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;                
        }

        /// <summary>
        /// Dictionary created to identify ids corresponding to Asset Names.
        /// </summary>
        /*private Dictionary<string, int> CreateAssetDictionary()
        {
            Dictionary<string, int> AssetNameToId = new Dictionary<string, int>();
            AssetNameToId.Add("Equity", 1);
            AssetNameToId.Add("EquityOption", 2);
            AssetNameToId.Add("Future", 3);
            AssetNameToId.Add("FutureOption", 4);
            AssetNameToId.Add("FX", 5);
            AssetNameToId.Add("Cash", 6);
            AssetNameToId.Add("Indices", 7);
            AssetNameToId.Add("FixedIncome", 8);
            AssetNameToId.Add("PrivateEquity", 9);
            AssetNameToId.Add("FXOption", 10);
            AssetNameToId.Add("FXForward", 11);
            AssetNameToId.Add("Forex", 12);
            AssetNameToId.Add("ConvertibleBond", 13);
            AssetNameToId.Add("CreditDefaultSwap", 14);
            return AssetNameToId;
        }*/

        /// <summary>
        /// Method to disable checkside for selected accounts.
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private string DisableChecksideAccount(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            Dictionary<string, int> AccountNameToId = CreateAccountDictionary();
            int resultant_ID;
            string accountID=null;
            try
            {
                IList<string> disableCheckSideAccounts = new List<string>();
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    string accountName = dr[TestDataConstants.COL_ACCOUNT_NAME].ToString();
                    if (accountName.Contains(","))
                    {
                        string[] accountNames = accountName.Split(',');
                        foreach (string accounts in accountNames)
                        {
                            if (AccountNameToId.TryGetValue(accounts, out resultant_ID))
                            {
                                disableCheckSideAccounts.Add(resultant_ID.ToString());
                            }
                            else
                            {
                                Console.WriteLine("Could not find the specified key while  disableCheckSideAccounts.");
                            }
                        }
                        
                    }
                    else
                    {
                    if (AccountNameToId.TryGetValue(accountName, out resultant_ID))
                    {
                        disableCheckSideAccounts.Add(resultant_ID.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Could not find the specified key.");
                    }
                    }
                }

                        accountID = string.Join(",", disableCheckSideAccounts);
                        return accountID;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
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

        /// <summary>
        /// Method to disable checkside for selected counter parties or brokers.
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private string DisableChecksideCounterParty(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            Dictionary<string, int> CounterPartyNameToId = CreateCounterPartyDictionary();
            try
            {
                IList<string> disableCheckSideBroker = new List<string>();
                int resultant_ID;
                string counterPartyID=null;
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
                    counterPartyID = string.Join(",", disableCheckSideBroker);
                    return counterPartyID;             
            }

            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Dictionary created to identify ids corresponding to Counter Party Names.
        /// </summary>
        /// <returns></returns>
       /* private Dictionary<string, int> CreateCounterPartyDictionary()
        {
            Dictionary<string, int> CounterPartyNameToId = new Dictionary<string, int>();
            CounterPartyNameToId.Add("MS", 1);
            CounterPartyNameToId.Add("GS", 2);
            CounterPartyNameToId.Add("CSFB", 5);
            CounterPartyNameToId.Add("PiperJaffray", 6);
            CounterPartyNameToId.Add("Bernstein", 7);
            CounterPartyNameToId.Add("STN", 9);
            CounterPartyNameToId.Add("FIMAT", 10);
            CounterPartyNameToId.Add("Source", 11);
            CounterPartyNameToId.Add("Lakeshore", 12);
            CounterPartyNameToId.Add("Wolverine", 13);
            CounterPartyNameToId.Add("DC", 14);
            CounterPartyNameToId.Add("DB", 15);
            CounterPartyNameToId.Add("UBS", 16);
            CounterPartyNameToId.Add("dik", 17);
            CounterPartyNameToId.Add("NSEW", 98);
            CounterPartyNameToId.Add("MSCOS", 99);
            return CounterPartyNameToId;
        }*/
    }
}
