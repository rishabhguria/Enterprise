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

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class SetMasterFundRatio : MasterFundPreferencesUIMap, ITestStep
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
                OpenAllocation();
                EditAllocationPreferences3.Click(MouseButtons.Left);
                MasterFundPreferences.Click(MouseButtons.Left);
                SetMasterFund(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetMasterFundRatio");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            //When No Master fund present then close it
            finally
            {
                CloseMasterFundRationAllocation();
                MinimizeAllocation();
            }
            return _res;
        }

        /// <summary>
        /// Closes the Master Fund Ration Allocation.
        /// </summary>
        private void CloseMasterFundRationAllocation()
        {
            try
            {
                Save.Click(MouseButtons.Left);
                if (NirvanaPreferences.IsVisible)
                    ButtonOK.Click(MouseButtons.Left);
                Close.Click(MouseButtons.Left);
            }
            catch (Exception) { throw; }
        }



        /// <summary>
        /// Sets the master fund ratio
        /// </summary>
        private void SetMasterFund(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {

                Dictionary<string, int> MasterFundToIndex = new Dictionary<string, int>();
                int count = MasterFundRatioDataGrid1.AutomationElementWrapper.CachedChildren[0].CachedChildren[0].CachedChildren.Count();
                for (int i = 1; i < count; i++)
                {
                    string FundName = MasterFundRatioDataGrid1.AutomationElementWrapper.CachedChildren[0].CachedChildren[0].CachedChildren[i].CachedChildren[1].Name;
                    int FundIndex = Convert.ToInt32(MasterFundRatioDataGrid1.AutomationElementWrapper.CachedChildren[0].CachedChildren[0].CachedChildren[i].Index);
                    MasterFundToIndex.Add(FundName, FundIndex);

                }

                foreach (DataRow dtRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    string FundName = dtRow[TestDataConstants.COL_MASTER_FUND_NAME].ToString();
                    MasterFundRatioDataGrid1.AutomationElementWrapper.CachedChildren[0].CachedChildren[0].CachedChildren[MasterFundToIndex[FundName]].CachedChildren[1].WpfClick();
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    ExtentionMethods.CheckCellValueConditions(dtRow[TestDataConstants.COL_TARGET_PERCENTAGE].ToString(), string.Empty, true);
                   // Wait(2000);
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
