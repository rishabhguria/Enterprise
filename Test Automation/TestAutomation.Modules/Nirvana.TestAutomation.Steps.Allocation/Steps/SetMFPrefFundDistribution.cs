using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class SetMFPrefFundDistribution : MasterFundPreferencesUIMap, ITestStep
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
                OpenMFPreferences();
                SetMasterFundRatio(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetMFPrefFundDistribution");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeEditAllocationPreference();
            }
            return _res;
        }

        /// <summary>
        /// Sets the master fund ratio
        /// </summary>
        private void SetMasterFundRatio(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                string preferenceName = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_PREFERENCE_NAME].ToString();
                Records.Click(MouseButtons.Left);
                Records.AutomationElementWrapper.FindDescendantByName(preferenceName).WpfClick();
                if (!preferenceName.Equals(string.Empty))
                {
                    Dictionary<string, int> MasterFundToIndex = new Dictionary<string, int>();
                    int count = MasterFundRatioDataGrid.AutomationElementWrapper.Children[0].Children[0].Children.Count();
                    for (int i = 1; i <count; i++)
                    {
                        string FundName = MasterFundRatioDataGrid.AutomationElementWrapper.Children[0].Children[0].Children[i].Children[1].Name;
                        int FundIndex = Convert.ToInt32(MasterFundRatioDataGrid.AutomationElementWrapper.Children[0].Children[0].Children[i].Index);
                        MasterFundToIndex.Add(FundName, FundIndex);

                    }

                    foreach (DataRow dtRow in testData.Tables[sheetIndexToName[0]].Rows)
                    {
                        string FundName = dtRow[TestDataConstants.COL_MASTER_FUND_NAME].ToString();
                        MasterFundRatioDataGrid.AutomationElementWrapper.Children[0].Children[0].Children[MasterFundToIndex[FundName]].Children[1].WpfClick();
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        ExtentionMethods.CheckCellValueConditions(dtRow[TestDataConstants.COL_TARGET_PERCENTAGE].ToString(), string.Empty, true);
                      //  Wait(2000);
                    }
                }

            }
            catch (Exception)
            {
                throw new Exception(MessageConstants.MSG_MASTER_FUND_NOT_ADDED); ;
            }
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
