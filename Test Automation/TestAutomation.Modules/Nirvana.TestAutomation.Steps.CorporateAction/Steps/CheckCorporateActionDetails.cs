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
    public class CheckCorporateActionDetails : CorporateActionUIMap, ITestStep
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
        /// Corporates the action step.
        /// </summary>
        /// <param name="dictColumnToIndexMap">The dictionary column to index map.</param>
        /// <param name="gridMssa">The grid mssa.</param>
        /// <param name="dtRow">The dt row.</param>
        private List<String> CheckCorporateActionStep(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            List<String> errors = new List<String>();
            try
            {
                    DataTable subset = testData.Tables[sheetIndexToName[0]];
                    GrdCorporateActionEntry.Click(MouseButtons.Left);
                    DataTable dtGridData = CSVHelper.CSVAsDataTable(GrdCorporateActionEntry.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                    DataRow dtRow2 = dtGridData.Rows[0];
                    List<String> columns = new List<String>();
                    foreach (DataColumn c in dtRow2.Table.Columns)
                    {
                        columns.Add(c.ColumnName);
                    }
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