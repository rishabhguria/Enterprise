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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.UIAutomation;

namespace Nirvana.TestAutomation.Steps.MultiTradingTicket
{
    class EditTradeUsingMTT : MultiTradingTicketUIMap, ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet name.</param>
        /// <returns></returns>
     public  TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
          try
            {  
                int rowIndex = (int)GrdTrades.InvokeMethod("GetActiveRow", null);
                EditTradesWithQuantity(testData,sheetIndexToName, GrdTrades, rowIndex); 

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
     /// Edit top row on MTT
     /// </summary>
     /// <param name="testData"></param>
     /// <param name="sheetIndexToName"></param>
     /// <param name="DataGrid"></param>
     private void EditTradesWithQuantity(DataSet testData, Dictionary<int, string> sheetIndexToName, UIUltraGrid DataGrid, int rowIndex)
     {
         try
         {
                DataTable newData = testData.Tables[sheetIndexToName[0]];
                List<String> columns = new List<String>();
                try
                {
                    string StepName = "EditTradeUsingMTT";
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref newData);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref newData);
                }
                catch (Exception ex)
                { Console.WriteLine(ex.Message); }
                DataTable dtGridData = CSVHelper.CSVAsDataTable(DataGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                MsaaObject obj = DataGrid.MsaaObject.FindDescendantByName("OrderBindingList", 3000);
                Dictionary<string, int> columnToIndexMapping = obj.CachedChildren[1].GetColumnIndexMaping(newData);
                DataRow dtRow = newData.Rows[0];
                if (rowIndex >= 0)
                {
                    var Row = obj.CachedChildren[rowIndex + 1];
                    DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, rowIndex);
                    DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 0);
                    for (int i = 0; i < newData.Columns.Count; i++)
                    {
                        string colName = newData.Columns[i].ColumnName.ToString();
                       
                            if (colName == "Commission Basis" || colName == "Soft Basis")
                            {
                                string columnValue = dtRow[colName].ToString();

                                if (!string.IsNullOrEmpty(columnValue))
                                {
                                int colIndex = 0;
                                if (columnToIndexMapping.ContainsKey(colName))
                                {
                                    colIndex = columnToIndexMapping[colName];

                                    if (columnValue == "$#$" || columnValue == "")
                                    {
                                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                        continue;
                                    }
                                }
                                else
                                {
                                    continue;
                                }

                                DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                                Wait(500);
                                Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                                Dictionary<string, int> dict = new Dictionary<string, int>();
                                dict = WinDataUtilities.GetCurrentSuggestionsList(ref TestDataConstants.Mtt, dict);
                                int elementIndex = GetElementIndex(dict, columnValue);
                                if (elementIndex <= -1)
                                {
                                    Console.WriteLine(columnValue + " does not exist");
                                    throw new Exception(columnValue + " does not exist");
                                }
                                else
                                {
                                    KeyboardUtilities.PressUpKeyWithWait(2 * dict.Count);
                                    for (int j = 0; j < elementIndex; j++)
                                    {
                                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                                    }

                                }
                            }


                            Wait(500);
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }
 
                        else if (colName != "Settlement Fx Rate" || colName != "Settlement Amount" || colName != "Settlement Fx Operator")
                        {
                            string columnValue = dtRow[colName].ToString();
                            int colIndex = 0;
                            if (columnToIndexMapping.ContainsKey(colName))
                            {
                                colIndex = columnToIndexMapping[colName];

                                if (columnValue == "$#$" || columnValue == "")
                                {
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }

                            DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                            Wait(500);
                            Row.CachedChildren[colIndex].DoubleClick(MouseButtons.Left);
                            UpdateCellValueConditionsMTT(columnValue, "");
                            Wait(500);
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);

                            Wait(5000);
                            if (uiWindow1.IsVisible)
                            {
                                if (ButtonOK.IsVisible)
                                {
                                    ButtonOK.Click(MouseButtons.Left);
                                }
                            }
                        }
                    }
                    DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, "");
                }
         }
         catch
         {
             throw;
         }
     }
    /// <summary>
    /// Function used to clear text of Grid cells
    /// </summary>
     public static void UpdateCellValueConditionsMTT(string cellValue, string appnedValue)
     {
         try
         {
             if (!string.IsNullOrWhiteSpace(cellValue))
             {
                 Stopwatch tmr = new Stopwatch();
                 tmr.Start();
                 //Keyboard.SendKeys("[END]");
                 while (tmr.ElapsedMilliseconds <= 1500)
                 {
                     Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                 }
                 while (tmr.ElapsedMilliseconds <= 3500)
                 {
                     Keyboard.SendKeys(KeyboardConstants.DELETE_KEY);
                 }
                 tmr.Stop();
                 if (!cellValue.Equals(ExcelStructureConstants.BLANK_CONST))
                     Keyboard.SendKeys(cellValue + appnedValue);
             }
         }
         catch (Exception ex)
         {
             Log.Error("Update cell value is failed :" + ex.Message);
             bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
             if (rethrow)
                 throw;
         }
     }
     
    }
}


