using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    public class LockandUnlockGridData : RebalancerUIMap, ITestStep
    {
        /// <summary>
        /// Begins the test execution
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenRebalancer();
                if (testData.Tables[0].Columns.Contains("CheckBox")) {
                    for (int i = testData.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        if (testData.Tables[0].Rows[i]["CheckBox"].ToString().ToLower().Equals("false"))
                        {
                            testData.Tables[0].Rows[i].Delete();
                        }
                    }
                    testData.Tables[0].Columns.Remove("CheckBox"); 
                    DataUtilities.RemoveTrailingZeroes(testData.Tables[0]);
                }
                LockSecurities(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeRebalancer();
            }
            return _res;
        }
    }
}
