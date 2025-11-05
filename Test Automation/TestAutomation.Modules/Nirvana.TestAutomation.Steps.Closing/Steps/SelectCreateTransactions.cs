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
    class SelectCreateTransactions : ExerciseTradesTab, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenClosingUI();
                ExpirationDivideSettlement.Click(MouseButtons.Left);
                SelectGrid(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }

        private string SelectGrid(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            string errorMessage = string.Empty;

            try
            {
                DataTable currentDataGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.GrdCreatePosition.InvokeMethod(ExcelStructureConstants.COL_GET_ALL_VISIBLE_DATA_FROM_THE_GRID, null).ToString()));
                DataTable excelFileData = testData.Tables[sheetIndexToName[0]];
                string msg = "";
                DataRow[] Result = DataUtilities.GetMatchingMultipleDataRows(currentDataGrid, excelFileData, msg );
                SelectRows(Result, currentDataGrid, excelFileData);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }

        private string SelectRows(DataRow[] Result, DataTable currentDataGrid, DataTable excelData)
        {
            string errorMessage = string.Empty;
            try
            {
                var mssaobject = GrdCreatePosition.MsaaObject;
                foreach (DataRow dt in Result)
                {
                    
                    int index = currentDataGrid.Rows.IndexOf(dt);
                    var row = mssaobject.CachedChildren[0].CachedChildren[index + 1];
                    GrdCreatePosition.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
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
    }
}
