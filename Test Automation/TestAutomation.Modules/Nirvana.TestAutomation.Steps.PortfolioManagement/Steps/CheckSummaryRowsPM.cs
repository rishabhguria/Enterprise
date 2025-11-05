using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    public partial class CheckSummaryRowsPM : PortfolioManagementUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Failed to check summary rows on PM. Reason : \n(" + ex.Message + ")
        /// </exception>

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenConsolidationView();
               // Wait(9000);
                DataTable superset = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.Main.InvokeMethod("GetAllVisibleGroupedDataFromTheGrid", null).ToString()));
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                List<String> columns = new List<String>();
                columns.Add("Symbol");
                columns.Add("Order Side");
                List<String> errors =new List<String>();
                errors = Recon.RunRecon(superset,subset,columns,.0000000001);
                if (errors.Count > 0)
                {
                    _result.ErrorMessage=String.Join("\n\r", errors);
                }
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                PMclose();
            }
            return _result;
        }
    }
}
