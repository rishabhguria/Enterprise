using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Closing
{
      
    class SelectTradeUnexpiredUnsettled : ExerciseTradesTab , ITestStep
    {
        /// <summary>
        /// Run Test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenClosingUI();
                ExpirationDivideSettlement.Click(MouseButtons.Left);
                ChkCopyOpeningTradeAttributes.Click(MouseButtons.Left);
                GetGridData(testData, sheetIndexToName);
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
                MinimizeClosing();
            }
            return _result;
        }

        /// <summary>
        /// Get Grid Data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private string GetGridData(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            string errorMessage = string.Empty;

            try
            {
                DataTable currentDataGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.GrdAccountUnexpired.InvokeMethod(ExcelStructureConstants.COL_GET_ALL_VISIBLE_DATA_FROM_THE_GRID, null).ToString()));
                DataTable excelFileData = testData.Tables[sheetIndexToName[0]];
                DataRow[] Result = DataUtilities.GetMatchingSingleDataRows(currentDataGrid, excelFileData);
                SelectRows(Result,currentDataGrid);
            }
            catch (Exception ex)
            {
				bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }

        /// <summary>
        /// Select Rows
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="currentDataGrid"></param>
        private string SelectRows(DataRow[] Result, DataTable currentDataGrid)
        {
            string errorMessage = string.Empty;
            try
            {
                var mssaobject = GrdAccountUnexpired.MsaaObject;
                foreach (DataRow dt in Result)
                {
                    int index = currentDataGrid.Rows.IndexOf(dt);
                    var row = mssaobject.CachedChildren[0].CachedChildren[index + 1];
                    GrdAccountUnexpired.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                    row.CachedChildren[0].Click(MouseButtons.Left);
                }
            }
            catch (Exception ex)
            {
				bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMessage;
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
