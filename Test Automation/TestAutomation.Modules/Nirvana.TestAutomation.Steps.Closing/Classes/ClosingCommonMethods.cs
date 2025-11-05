using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces.Enums;
using Nirvana.TestAutomation.Steps.Closing;


namespace Nirvana.TestAutomation.Steps.Closing.Classes
{
    internal static class ClosingCommonMethods 
    {
        /// <summary>
        /// Perform Closing Operation in a Generic Method
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <param name="grid"></param>
        internal static TestResult PerformClosingOperation(DataSet testData, Dictionary<int, string> sheetIndexToName, UIUltraGrid grid)
        {
            TestResult _res = new TestResult();
            try
            {
                DataTable currentDataGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(grid.InvokeMethod(ExcelStructureConstants.COL_GET_ALL_VISIBLE_DATA_FROM_THE_GRID, null).ToString()));
                DataTable excelFileData = testData.Tables[sheetIndexToName[0]];
            
                int index=1;

                foreach (DataRow dr in excelFileData.Rows)
                {
                    string colName = string.Empty;
                    string colValue = string.Empty;

                    MsaaObject msaaObjectGridCT;

                    if (grid.MsaaName.Equals("grdCashandExpire")) //For expire
                    {
                        msaaObjectGridCT = grid.MsaaObject.FindDescendantByName("GenericBindingList`1", 3000).CachedChildren[index];
                    }
                    else
                    {
                        Keyboard.SendKeys(KeyboardConstants.CTRL_HOMEKEY);
                        msaaObjectGridCT = grid.MsaaObject.FindDescendantByName("OTCPositionList", 3000).CachedChildren[index]; 
                    }

                    Dictionary<string, int> indexToColumnMapDictionary = msaaObjectGridCT.GetColumnIndexMaping(currentDataGrid);      
       
                    int length = currentDataGrid.Columns.Count;         
                    for (int i = 0; i < length; i++)
                    {
                        if (excelFileData.Columns.Contains(currentDataGrid.Columns[i].ColumnName))
                        {
                            colName = currentDataGrid.Columns[i].ColumnName;
                            colValue = dr[colName].ToString();

                            if (!string.IsNullOrWhiteSpace(colValue))
                            {                
                                int columnIndex = indexToColumnMapDictionary[colName];
                                grid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                                msaaObjectGridCT.CachedChildren[columnIndex].Click(MouseButtons.Left);
                                msaaObjectGridCT.CachedChildren[columnIndex].Click(MouseButtons.Left);

                                if (colName.Equals("Trade Date") || colName.Equals("Settlement Date"))
                                {
                                    Keyboard.SendKeys(colValue);
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);

                                    ExerciseTradesTab dt = new ExerciseTradesTab();
                                    if (dt.Warning1.IsVisible)
                                    { dt.ButtonOK1.Click(MouseButtons.Left); }
                                    
                                }     
                                else
                                {
                                    DateTime dDate;
                                    string tempColValue = DateTime.TryParse(colValue, out dDate) ? colValue.Replace(@"/", "") : colValue;
                                    Keyboard.SendKeys(tempColValue);

                                   
                                    // For editing close date On Expiring four symbols togetherly
                                    //Keyboard.SendKeys(tempColValue); /*Commenting out the line of code due to "TESTAUTO-2558" & "TESTAUTO-2546".
                                }

                                
                            }        
                        }
                    }
                  index=index+1;             
                }
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Unwind Close
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        internal static TestResult UnwindClosingData(DataSet testData, Dictionary<int, string> sheetIndexToName, UIWindow grid)
        {
            TestResult _result = new TestResult();
            try
            {
                DataTable currentDataGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(grid.InvokeMethod(ExcelStructureConstants.COL_GET_ALL_VISIBLE_DATA_FROM_THE_GRID, null).ToString()));
                DataTable excelFileData = testData.Tables[sheetIndexToName[0]];
                DataRow[] Result = DataUtilities.GetMatchingSingleDataRows(currentDataGrid, excelFileData);
                
                if (Result.Count() > 0)
                {
                    MsaaObject mssaobject = grid.MsaaObject;
                    int index=0;
                    foreach (DataRow dt in Result)
                    {
                        index = currentDataGrid.Rows.IndexOf(dt);
                        grid.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                        mssaobject.CachedChildren[0].CachedChildren[index + 1].CachedChildren[0].Click(MouseButtons.Left);
                    }
                    mssaobject.CachedChildren[0].CachedChildren[index+1].CachedChildren[2].Click(MouseButtons.Left);
                    MouseController.RightClick();
                    MouseController.RightClick();
                }
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

        /// <summary>
        /// Verify Closing Data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetName"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        internal static TestResult VerifyClosingData(DataSet testData, string sheetName, UIWindow grid)
        {
            TestResult _res = new TestResult();
            try
            {
                StringBuilder errorBuilder = new StringBuilder();
                DataTable currentDataGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(grid.InvokeMethod(ExcelStructureConstants.COL_GET_ALL_VISIBLE_DATA_FROM_THE_GRID, null).ToString()));
                DataTable excelFileData = testData.Tables[sheetName];
                string gridName = grid.InstanceName.ToString();
                List<string> keyColumns = new List<string>();
                DataRow drow = null;
                if (excelFileData.Rows.Count > 0)
                {
                    drow = excelFileData.Rows[0];
                }
                // If Table contains mandatory columns then verify mandatory columns
                if (excelFileData.Columns.Contains("MandatoryColumn"))
                {

                    if (!String.IsNullOrEmpty(drow["MandatoryColumn"].ToString()))
                    {
                        if (gridName.ToString() == "grdLong" || gridName.ToString() == "grdShort")
                            keyColumns = MandatoryColumns.LongShortGrid();
                        else if (gridName.ToString() == "grdNetPosition")
                            keyColumns = MandatoryColumns.Closing();
                        else if (gridName.ToString() == "grdAccountExpired")
                            keyColumns = MandatoryColumns.ExpiredSettleGrid();
                    }
                    else
                    {
                        keyColumns.Add(TestDataConstants.COL_SYMBOL);
                        keyColumns.Add(TestDataConstants.COL_ACCOUNT);
                    }
                    excelFileData.Columns.Remove("MandatoryColumn");
                }
                else
                {
                    keyColumns.Add(TestDataConstants.COL_SYMBOL);
                    keyColumns.Add(TestDataConstants.COL_ACCOUNT);
                }
                
                //{ TestDataConstants.COL_SYMBOL, TestDataConstants.COL_ACCOUNT};
                List<string> errors = Recon.RunRecon(currentDataGrid, excelFileData, keyColumns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
                if (errors.Count > 0)
                    errorBuilder.Append("Errors:-" + String.Join("\n\r", errors));

                if (!String.IsNullOrEmpty(errorBuilder.ToString()))
                   _res.AddResult(false,errorBuilder.ToString());
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
           return _res;
        }


        internal static TestResult VerifyCreateTransactionData(DataSet testData, string sheetName, UIWindow grid)
        {
            TestResult _res = new TestResult();
            try
            {
                StringBuilder errorBuilder = new StringBuilder();
                DataTable currentDataGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(grid.InvokeMethod(ExcelStructureConstants.COL_GET_ALL_VISIBLE_DATA_FROM_THE_GRID, null).ToString()));
                DataTable excelFileData = testData.Tables[sheetName];
                List<string> keyColumns = new List<string>() { TestDataConstants.COL_SYMBOL, TestDataConstants.COL_ACCOUNT };
                List<string> errors = Recon.RunRecon(currentDataGrid, excelFileData, keyColumns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
                if (errors.Count > 0)
                    errorBuilder.Append("Errors:-" + String.Join("\n\r", errors));

                if (!String.IsNullOrEmpty(errorBuilder.ToString()))
                    _res.AddResult(false, errorBuilder.ToString());
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
 
        }
    }
   
}
