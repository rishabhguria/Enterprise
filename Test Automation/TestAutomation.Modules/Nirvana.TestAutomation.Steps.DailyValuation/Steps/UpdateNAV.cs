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

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
   public class UpdateNAV : NAVUIMap, ITestStep
    {
       /// <summary>
       /// Run the test.
       /// </summary>
       /// <param name="testData">The test data.</param>
       /// <param name="sheetIndexToName">The sheet no.</param>
       /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenNAVTab();
                UpdateNAVData(testData, sheetIndexToName);
                BtnSave.Click(MouseButtons.Left);
                Wait(1000);
                GrdPivotDisplay.Click(MouseButtons.Left);
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
       /// Update NAV
       /// </summary>
       /// <param name="testData">The test data.</param>
       /// <param name="sheetIndexToName">The sheet no.</param>
        private void UpdateNAVData(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {

                DataTable dNAV = CSVHelper.CSVAsDataTable(GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();
                var msaaObj = GrdPivotDisplay.MsaaObject;
                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    DataRow[] foundRow = dNAV.Select(String.Format(@"[" + TestDataConstants.COL_ACCOUNT + "] ='{0}'", dataRow[TestDataConstants.COL_ACCOUNT]));
                    if (foundRow.Length > 0)
                    {
                        int index = dNAV.Rows.IndexOf(foundRow[0]);
                        var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                        if (columToMSAAIndexMapping.Count == 0)
                        {
                            columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dNAV);
                            SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, DtDateMonth.Properties["Text"].ToString());
                        }
                    }
                    else 
                    {
                        AddNAVData(testData, sheetIndexToName);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

       /// <summary>
       /// Set value in grid.
       /// </summary>
       /// <param name="columToIndexMapping"></param>
       /// <param name="dataRow"></param>
       /// <param name="gridRow"></param>
       /// <param name="columnName"></param>
        private void SetValueInGrid(Dictionary<string, int> columToIndexMapping, DataRow dataRow, MsaaObject gridRow, string columnName)
        {

            try
            {
                if (!String.IsNullOrEmpty(dataRow["Price"].ToString()))
                {
                    if (columToIndexMapping.ContainsKey(columnName))
                    {
                        int columnIndex = columToIndexMapping[columnName];
                        GrdPivotDisplay.Click(MouseButtons.Left);
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, columnName);
                        Wait(1000);
                        gridRow.CachedChildren[columnIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(dataRow["Price"].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add NAV data, if not found.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet no.</param>
        private void AddNAVData(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                BtnAdd.Click(MouseButtons.Left);
                DataTable dtAddNAV = CSVHelper.CSVAsDataTable(GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();

                var msaaObj = GrdPivotDisplay.MsaaObject;

                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {

                    int index = dtAddNAV.Rows.Count;
                    var gridRow = msaaObj.CachedChildren[0].CachedChildren[index - 1];
                    GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);

                    if (columToMSAAIndexMapping.Count == 0)
                    {
                        columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtAddNAV);
                        AddNAVDataAccount(columToMSAAIndexMapping, dataRow, gridRow);
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add account
        /// </summary>
        /// <param name="columToIndexMapping"></param>
        /// <param name="dataRow"></param>
        /// <param name="gridRow"></param>
        private void AddNAVDataAccount(Dictionary<string, int> columToIndexMapping, DataRow dataRow, MsaaObject gridRow)
        {
            try
            {
                if (!String.IsNullOrEmpty(dataRow["Account"].ToString()))
                {
                    if (columToIndexMapping.ContainsKey("Account"))
                    {
                        int columnIndex = columToIndexMapping["Account"];
                        GrdPivotDisplay.Click(MouseButtons.Left);
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, "Account");
                        Wait(1000);
                        gridRow.CachedChildren[columnIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(dataRow["Account"].ToString());
                    }
                    SetValueInGrid(columToIndexMapping, dataRow, gridRow, DtDateMonth.Properties["Text"].ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}
