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


namespace Nirvana.TestAutomation.Steps.ShortLocate
{
    public class OpenTTFromShortLocate : ShortLocateUIMap, ITestStep
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
                OpenShortLocateUI();
                ShortLocate_Fill_Panel1.Click();
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
                ShortLocate.BringToFront();
                KeyboardUtilities.CloseWindow(ref ShortLocate_UltraFormManager_Dock_Area_Top1);
            }
            return _result;
        }
        private string GetGridData(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            string errorMessage = string.Empty;
            try
            {
                string ShortLocategrid = modifyCSV();
                DataTable dtShortLocateGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(ShortLocategrid));  
                DataTable excelFileData = testData.Tables[sheetIndexToName[0]];
                DataRow[] Result = DataUtilities.GetMatchingSingleDataRows(dtShortLocateGrid, excelFileData);
                SelectRows(Result, dtShortLocateGrid);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }
        private string SelectRows(DataRow[] Result, DataTable currentDataGrid)
        {
            string errorMessage = string.Empty;
            try
            {
                var mssaobject = GrdShortLocate.MsaaObject;
                foreach (DataRow dt in Result)
                {
                    int index = currentDataGrid.Rows.IndexOf(dt);
                    try
                    {
                        var row = mssaobject.CachedChildren[1].CachedChildren[index + 1];
                        GrdShortLocate1.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                        for (int i = 0; i < 2; i++)
                        {
                            row.CachedChildren[1].Click(MouseButtons.Left);
                        }
                    }
                    catch
                    {
                        var row = mssaobject.CachedChildren[0].CachedChildren[index + 1];
                        row.CachedChildren[1].DoubleClick(MouseButtons.Left);
                    }
                    Wait(10000);
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
