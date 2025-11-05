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
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
     class VerifyActivity : ActivityUIMap , ITestStep
    {
       /// <summary>
       /// Run The test.
       /// </summary>
       /// <param name="testData"> The test data</param>
        /// <param name="sheetIndexToName">The sheet no.</param>
       /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
         {
             TestResult _res = new TestResult();
            try
            {
                OpenActivityTab();
                BtnGetActivities.Click(MouseButtons.Left);
                ExtentionMethods.WaitForEnabled(ref BtnGetActivities, TestDataConstants.GL_DATA_FETCHING_TIME);
                if (Revaluation.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                    BtnGetActivities.Click(MouseButtons.Left);
                }
                
                StringBuilder activityError = new StringBuilder(String.Empty);
                List<String> error = VerifyActivityTab(testData, sheetIndexToName[0], GrdActivity);
                if (error.Count > 0)
                    activityError.Append("Errors:-" + String.Join("\n\r", error));
                if (!string.IsNullOrEmpty(activityError.ToString()))
                    _res.ErrorMessage = activityError.ToString();
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
                MinimizeGeneralLedger();
            }
            return _res;
        }

        /// <summary>
        /// Verify the activity data tab
        /// </summary>
        /// <param name="testData">The Test Data.</param>
        /// <param name="sheetName">The Sheet No.</param>
        /// <param name="ActivityGrid">The Activity Grid</param>
        /// <returns></returns>
        public List<String> VerifyActivityTab(DataSet testData, string sheetName, UIUltraGrid ActivityGrid)
        {
            try
            {
                DataTable subset = testData.Tables[sheetName];
                List<String> colList = new List<String>();
                for (int i = 0; i < subset.Columns.Count; i++)
                {
                    colList.Add(subset.Columns[i].ColumnName);
                }
                ActivityGrid.Click(MouseButtons.Left);
                ActivityGrid.InvokeMethod("AddColumnsToGrid", colList);
                ActivityGrid.InvokeMethod("RemoveGrouping", null);               
                DataTable dtCashGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(ActivityGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));                
                List<String> columns = new List<String>();
                columns = GetKeyColumnsForActivity();
                List<String> errors = new List<String>();
                errors = Recon.RunRecon(dtCashGrid, subset, columns, 0.1, true, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
                return errors;
            }
            catch (Exception)
            {
                throw;
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
