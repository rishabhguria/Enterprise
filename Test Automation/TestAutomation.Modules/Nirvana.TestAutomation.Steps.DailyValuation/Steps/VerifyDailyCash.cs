using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    class VerifyDailyCash : DailyCashUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenDailyCashTab();
                List<String> errors = VerifyData(testData.Tables[0]);
                if (errors.Count > 0)
                    _result.ErrorMessage = String.Join("\n\r", errors);
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
                KeyboardUtilities.CloseWindow(ref MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        /// <summary>
        /// To Verify the data.
        /// </summary>
        /// <param name="dTable">The test data.</param>
        private List<String> VerifyData(DataTable dTable)
        {
            List<String> errors = new List<string>();
            try
            {
                GetAllColumnsOnGrid(dTable);
                DataTable dtDailyCash = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                dtDailyCash = DataUtilities.RemoveCommas(dtDailyCash);
                List<String> columns = new List<string>();
                columns.Add("Account");
                errors = Recon.RunRecon(dtDailyCash, dTable, columns, 0.01);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errors;
        }

        /// <summary>
        /// To get columns from Grid.
        /// </summary>
        /// <param name="dTable">The test data.</param>
        private void GetAllColumnsOnGrid(DataTable dTable)
        {
            try
            {
                List<string> columns = new List<string>();
                foreach (DataColumn item in dTable.Columns)
                {
                    columns.Add(item.ColumnName);
                }
                this.GrdPivotDisplay.InvokeMethod("AddColumnsToGrid", columns);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch 
            {
                throw;
            }
            
        }
    }
}
