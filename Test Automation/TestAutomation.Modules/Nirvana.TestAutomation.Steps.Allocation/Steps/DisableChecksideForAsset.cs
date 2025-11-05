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
    public partial class DisableChecksideForAsset : PreferencesUIMap, ITestStep
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
                DisableChecksideAsset(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "DisableChecksideAsset");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
        /// <summary>
        /// Method to disable checkside for selected assets.
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void DisableChecksideAsset(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            Dictionary<string, int> AssetNameToId = CreateAssetDictionary();
            int resultant_ID;
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
                string assetID = string.Join(",", disableCheckSideAssets);
                SQLQueriesConstants.setAssetCheckSideValue(assetID);
                SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateAssetCheckSideValueQuery);
              //  Wait(5000);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }

        /// <summary>
        /// Dictionary created to identify ids corresponding to Asset Names.
        /// </summary>
       /* private Dictionary<string, int> CreateAssetDictionary()
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
    }
}
