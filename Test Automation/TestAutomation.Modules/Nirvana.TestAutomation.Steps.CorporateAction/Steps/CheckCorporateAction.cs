using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Text;

namespace Nirvana.TestAutomation.Steps.CorporateAction
{
    public class CheckCorporateAction : CorporateActionUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        /// 
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();

            try
            {
                OpenCorporateActionsUI();
                StringBuilder verifyError = new StringBuilder(String.Empty);
                List<String> error = CheckCorporateActionStep(testData, sheetIndexToName);
                if (error.Count > 0)
                    verifyError.Append("Errors:-" + String.Join("\n\r", error));
                if (!string.IsNullOrEmpty(verifyError.ToString()))
                    _result.ErrorMessage = verifyError.ToString();
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
                MinimizeCorporateActionUI();
            }
            return _result;
        }

        /// <summary>
        /// Check Corporate Action Trade
        /// </summary>
        private List<String> CheckCorporateActionStep(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            List<String> errors = new List<String>();
            try
            {
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                DataTable dtGridData = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(GrdPositions.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                dtGridData = DataUtilities.RemoveCommas(dtGridData);
                List<String> columns = new List<String>();
                errors = Recon.RunRecon(dtGridData, subset, columns, 0.01);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errors;
        }
    }
}