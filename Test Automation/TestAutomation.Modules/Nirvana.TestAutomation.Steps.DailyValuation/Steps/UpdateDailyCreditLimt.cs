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
   public class UpdateDailyCreditLimt : DailyCreditLimitUIMap, ITestStep
    {
       /// <summary>
       /// Run the test.
       /// </summary>
       /// <param name="testData">The test data.</param>
       /// <param name="sheetIndexToName">The sheet no.</param>
       /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenDailyCreditLimitTab();
                UpdateDailyCreditLimitData(testData, sheetIndexToName);
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
       /// Update Credit limit data.
       /// </summary>
       /// <param name="testData">The test data.</param>
       /// <param name="sheetIndexToName">The sheet no.</param>
        private void UpdateDailyCreditLimitData(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataTable dtDailyVolatility = CSVHelper.CSVAsDataTable(GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();
                var msaaObj = GrdPivotDisplay.MsaaObject;
                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    DataRow[] foundRow = dtDailyVolatility.Select(String.Format(@"[" + TestDataConstants.COL_ACCOUNT + "] ='{0}'", dataRow[TestDataConstants.COL_ACCOUNT]));
                    if (foundRow.Length > 0)
                    {
                        int index = dtDailyVolatility.Rows.IndexOf(foundRow[0]);
                        var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                        if (columToMSAAIndexMapping.Count == 0)
                        {
                            columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtDailyVolatility);
                            SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow);
                        }
                    }
                    else
                    {
                        AddDailyCreditLmitData(testData, sheetIndexToName);                        
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
        private void SetValueInGrid(Dictionary<string, int> columToIndexMapping, DataRow dataRow, MsaaObject gridRow)
        {           
            try
            {
                SetValueInCell(columToIndexMapping, dataRow, gridRow, TestDataConstants.COL_LONG_DEBIT_LIMIT);
                SetValueInCell(columToIndexMapping, dataRow, gridRow, TestDataConstants.COL_SHORT_CREDIT_LIMIT);
                SetValueInCell(columToIndexMapping, dataRow, gridRow, TestDataConstants.COL_LONG_DEBIT_BALANCE);
                SetValueInCell(columToIndexMapping, dataRow, gridRow, TestDataConstants.COL_SHORT_CREDIT_BALANCE);      
             
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add credit limit data.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet no.</param>
        private void AddDailyCreditLmitData(DataSet testData, Dictionary<int, string> sheetIndexToName)
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
                        AddDailyCreditAccount(columToMSAAIndexMapping, dataRow, gridRow);
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add account in credit limit .
        /// </summary>
        /// <param name="columToIndexMapping"></param>
        /// <param name="dataRow"></param>
        /// <param name="gridRow"></param>
        private void AddDailyCreditAccount(Dictionary<string, int> columToIndexMapping, DataRow dataRow, MsaaObject gridRow)
        {
            try
            {
                if (!String.IsNullOrEmpty(dataRow[TestDataConstants.COL_ACCOUNT].ToString()))
                {
                    if (columToIndexMapping.ContainsKey(TestDataConstants.COL_ACCOUNT))
                    {
                        int columnIndex = columToIndexMapping[TestDataConstants.COL_ACCOUNT];
                        GrdPivotDisplay.Click(MouseButtons.Left);
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, TestDataConstants.COL_ACCOUNT);
                        Wait(1000);
                        gridRow.CachedChildren[columnIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(dataRow[TestDataConstants.COL_ACCOUNT].ToString());
                    }

                    SetValueInGrid(columToIndexMapping, dataRow, gridRow);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }    
    }
}

