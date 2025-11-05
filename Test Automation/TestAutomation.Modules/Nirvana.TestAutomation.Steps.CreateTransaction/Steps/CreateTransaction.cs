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

namespace Nirvana.TestAutomation.Steps.CreateTransaction
{
    public class CreateTransaction : CreateTransactionUIMap, ITestStep
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
                OpenCreateTransaction();
                CreateNewTransaction(testData, sheetIndexToName);
                BtnSave.Click(MouseButtons.Left);
                
                if (CreateTransactionsSave.IsVisible)
                {
                    if (testData.Tables[sheetIndexToName[0]].Columns.Contains(TestDataConstants.ColSaveTransaction))
                    {
                        if(string.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.ColSaveTransaction].ToString()))
                        {
                            ButtonSaveYes.Click(MouseButtons.Left);
                        }
                        else if (string.Equals(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.ColSaveTransaction].ToString(), "Yes", StringComparison.OrdinalIgnoreCase))
                        {
                            ButtonSaveYes.Click(MouseButtons.Left);
                        }
                        else
                            ButtonNo.Click(MouseButtons.Left);

                    }
                    else
                        ButtonSaveYes.Click(MouseButtons.Left);
                }

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CreateTransaction");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                if (!NAVLock.IsVisible)
                {
                KeyboardUtilities.MinimizeWindow(ref CreatePosition_UltraFormManager_Dock_Area_Top);
                }
            }
            return _result;
        }

        /// <summary>
        /// Creates the new transaction.
        /// </summary>
        /// <param name="testData">The test data.</param>
        private void CreateNewTransaction(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                BtnAddToCloseTrade.Click(MouseButtons.Left);
                var gridRow = GrdCreatePosition.MsaaObject.CachedChildren[0];
                Dictionary<string, int> indexToColumnMapDictionary = new Dictionary<string, int>();
                for (int colIndex = 0; colIndex < gridRow.CachedChildren[1].ChildCount; colIndex++)
                {
                    if(indexToColumnMapDictionary.ContainsKey(gridRow.CachedChildren[1].CachedChildren[colIndex].Name))
                    {
                        indexToColumnMapDictionary.Add(gridRow.CachedChildren[1].CachedChildren[colIndex].Name + '2', colIndex);
                    }
                    //if (colIndex == 62 || colIndex == 70)
                    //{
                    //    if (colIndex == 62)
                    //        indexToColumnMapDictionary.Add(TestDataConstants.COL_SIDE2, colIndex);
                    //    if (colIndex == 70)
                    //        indexToColumnMapDictionary.Add(TestDataConstants.COL_STRATEGY2, colIndex);
                    //}
                    else
                    {
                        indexToColumnMapDictionary.Add(gridRow.CachedChildren[1].CachedChildren[colIndex].Name, colIndex);
                    }
                }


                int recordsCounter = 0;
                foreach (DataRow dtrow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    var row = GrdCreatePosition.MsaaObject.CachedChildren[0].CachedChildren[1];
                    row.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_SYMBOL]].Click(MouseButtons.Left);
                    Keyboard.SendKeys(dtrow[TestDataConstants.COL_SYMBOL].ToString());
                    Wait(2000);
                    KeyboardUtilities.PressKey(3, KeyboardConstants.TABKEY);
                    if (!dtrow[TestDataConstants.COL_SIDE].Equals("Buy"))
                    {
                        Keyboard.SendKeys(dtrow[TestDataConstants.COL_SIDE].ToString());
                    }   
                    row.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_ACCOUNT]].Click(MouseButtons.Left);
                    if (dtrow[TestDataConstants.COL_ACCOUNT].ToString() != string.Empty)
                    { Keyboard.SendKeys(dtrow[TestDataConstants.COL_ACCOUNT].ToString());
                    }
                    row.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_QUANTITY]].Click(MouseButtons.Left);
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    Keyboard.SendKeys(dtrow[TestDataConstants.COL_QUANTITY].ToString());
                    row.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_COMMISSION]].Click(MouseButtons.Left);
                    Keyboard.SendKeys(dtrow[TestDataConstants.COL_COMMISSION].ToString());
                    row.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_AVERAGE_PRICE]].Click(MouseButtons.Left);
                    Keyboard.SendKeys(dtrow[TestDataConstants.COL_AVERAGE_PRICE].ToString());

                   //
                    //GrdCreatePosition.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, indexToColumnMapDictionary[TestDataConstants.COL_BROKER]);

                    if (dtrow.Table.Columns.Contains(TestDataConstants.COL_BROKER))
                    {
                        if (!string.IsNullOrEmpty(dtrow[TestDataConstants.COL_BROKER].ToString()))
                        {
                            string colName = TestDataConstants.COL_BROKER;
                            GrdCreatePosition.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                            Wait(3000);
                            row.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_BROKER]].Click(MouseButtons.Left);
                            Keyboard.SendKeys(dtrow[TestDataConstants.COL_BROKER].ToString());
                        }
                    }
                    
                   // Wait(3000);
                    if (dtrow.Table.Columns.Contains(TestDataConstants.COL_TRADE_DATE))
                    {
                        if (!string.IsNullOrEmpty(dtrow[TestDataConstants.COL_TRADE_DATE].ToString()))
                        {
                            string colName = TestDataConstants.COL_TRADE_DATE;
                            GrdCreatePosition.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                            String fromdate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(dtrow[TestDataConstants.COL_TRADE_DATE].ToString()));
                            row.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_TRADE_DATE]].Click(MouseButtons.Left);
                            Keyboard.SendKeys(fromdate);
                        }
                    }

                     //Wait(3000);
                     if (dtrow.Table.Columns.Contains(TestDataConstants.COL_ORIGINALDATE))
                     {
                         if (!string.IsNullOrEmpty(dtrow[TestDataConstants.COL_ORIGINALDATE].ToString()))
                         {
                             string colName = TestDataConstants.COL_ORIGINALDATE;
                             GrdCreatePosition.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                             String fromdate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(dtrow[TestDataConstants.COL_ORIGINALDATE].ToString()));
                             row.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_ORIGINALDATE]].Click(MouseButtons.Left);
                             Keyboard.SendKeys(fromdate);
                         }
                     }

                  /*  GrdCreatePosition.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, indexToColumnMapDictionary[TestDataConstants.COL_TRADE_DATE]);
                    row.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_TRADE_DATE]].Click(MouseButtons.Left);
                    String fromdate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(dtrow[TestDataConstants.COL_TRADE_DATE].ToString()));
                    Keyboard.SendKeys(fromdate);
                    Wait(3000);
                    GrdCreatePosition.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, indexToColumnMapDictionary[TestDataConstants.COL_ORIGINALDATE]);
                    row.CachedChildren[indexToColumnMapDictionary[TestDataConstants.COL_ORIGINALDATE]].Click(MouseButtons.Left);
                    string originalDateString = dtrow[TestDataConstants.COL_ORIGINALDATE].ToString();
               //     DateTime originalDate;
                  //  if (DateTime.TryParseExact(originalDateString, ExcelStructureConstants.COMMON_DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out originalDate))
                    {
                        String timestamp = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(dtrow[TestDataConstants.COL_ORIGINALDATE].ToString()));
                        Keyboard.SendKeys(timestamp);
                    }
                   * */



                    recordsCounter++;
                    if (recordsCounter != testData.Tables[sheetIndexToName[0]].Rows.Count)
                    {
                        BtnAddToCloseTrade.WaitForResponding();
                        BtnAddToCloseTrade.Click(MouseButtons.Left);
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