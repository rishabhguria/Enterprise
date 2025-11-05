using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class VerifyRollover: BlotterUIMap, ITestStep
    {
        /// <summary>
        /// For verify rollover status on blotter 
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                List<String> errors = InputEnter(testData.Tables[0]);
                if (errors.Count > 0)
                    _res.ErrorMessage = String.Join("\n\r", errors);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyRollover");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseBlotter();
            }
            return _res;
        }

        /// <summary>
        /// To enter details of first table of sheet
        /// </summary>
        /// <param name="dTable"></param>
        /// <returns></returns>
        private List<String> InputEnter(DataTable dTable)
        {
            try
            {
                ViewAllColumnsOnGrid(dTable);
                //Added 2 min wait for rollover happening on blotter before verifying.
                Wait(120000);
                DataTable dtBlotter = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.DgBlotter2.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                dtBlotter = DataUtilities.RemoveCommas(dtBlotter);
                List<String> columns = new List<string>();
                DataRow drow = null;
                if (dTable.Rows.Count > 0)
                {
                    drow = dTable.Rows[0];
                }
                // If Table contains mandatory columns then verify mandatory columns
                if (dTable.Columns.Contains("MandatoryColumn"))
                {

                    if (!String.IsNullOrEmpty(drow["MandatoryColumn"].ToString()))
                    {
                        columns = MandatoryColumns.SubOrder();
                    }
                    else
                    {
                        columns.Add("Symbol");
                    }
                    dTable.Columns.Remove("MandatoryColumn");
                }
                else
                {
                    columns.Add("Symbol");
                }
                
                List<String> errors = Recon.RunRecon(dtBlotter, dTable, columns, 0.01);
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
