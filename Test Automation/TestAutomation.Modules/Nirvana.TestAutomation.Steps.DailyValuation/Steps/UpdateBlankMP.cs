using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;


namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    public class UpdateBlankMP : BlankMarkPriceUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
               OpenMarkPriceTab();
                UpdateNewMP(testData, sheetIndexToName);
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

        private void UpdateNewMP(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {

                DataTable dtDailyVolatility = CSVHelper.CSVAsDataTable(GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                 var msaaObj = GrdPivotDisplay.MsaaObject;
                //Dictionary that stores mapping of column names with it's index
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();
                int Children = 0;

                //if( (gridMssaObject.CachedChildren[0].CachedChildren[1].Accessible).Equals( true))
                // Children = gridMssaObject.CachedChildren[0].CachedChildren[1].ChildCount;
                // else
                Children = msaaObj.CachedChildren[0].CachedChildren[1].ChildCount;


               
                GrdPivotDisplay.Click(MouseButtons.Left);
                BtnSave.Click(MouseButtons.Left);
                GrdPivotDisplay.Click(MouseButtons.Left);
                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    DataRow[] foundRow;
                    if (testData.Tables[sheetIndexToName[0]].Columns.Contains(TestDataConstants.COL_ACCOUNT_DATA))
                        foundRow = dtDailyVolatility.Select(String.Format(@"[" + TestDataConstants.COL_Symbol_DATA + "]='{0}' AND [" + TestDataConstants.COL_ACCOUNT_DATA + "] ='{1}'", dataRow[TestDataConstants.COL_SYMBOL], dataRow[TestDataConstants.COL_ACCOUNT_DATA]));
                    else
                        foundRow = dtDailyVolatility.Select(String.Format(@"[" + TestDataConstants.COL_Symbol_DATA + "]='{0}'", dataRow[TestDataConstants.COL_Symbol_DATA]));
                    if (foundRow.Length > 0)
                    {
                        int index = dtDailyVolatility.Rows.IndexOf(foundRow[0]);
                        var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                        if (columToMSAAIndexMapping.Count == 0)
                        {
                            //TODO: GetColumnIndexMaping() call this method on grid and out of this for loop
                            columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtDailyVolatility);
                            SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, DtDateMonth.Properties["Text"].ToString());

                            // Changes done for adding mark prices in case of single date and multiple symbols.
                            columToMSAAIndexMapping.Clear();
                        }
                    }
                }

            }

            catch (Exception)
            {
                throw;
            }
        }
        private void SetValueInGrid(Dictionary<string, int> columToIndexMapping, DataRow dataRow, MsaaObject gridRow, string columnName)
        {

            try
            {
                if (!String.IsNullOrEmpty(dataRow["MarkPrice"].ToString()))
                {
                    if (columToIndexMapping.ContainsKey(columnName))
                    {
                        int columnIndex = columToIndexMapping[columnName];
                        GrdPivotDisplay.Click(MouseButtons.Left);
                        GrdPivotDisplay.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, columnName);
                        Wait(1000);
                        gridRow.CachedChildren[columnIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(dataRow["MarkPrice"].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
