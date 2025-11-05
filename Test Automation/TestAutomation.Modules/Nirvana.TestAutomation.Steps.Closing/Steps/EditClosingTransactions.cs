using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Closing
{
    class EditClosingTransactions : ExerciseTradesTab, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                int rowIndex = (int)GrdCreatePosition.InvokeMethod("GetActiveRow", null);
                EditCreateTransactions(testData, sheetIndexToName, GrdCreatePosition, rowIndex);
                /*BtnSave.Click(MouseButtons.Left);
                if (ButtonYes.IsVisible)
                    ButtonYes.Click(MouseButtons.Left);*/

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

        private void EditCreateTransactions(DataSet testdata, Dictionary<int, string> sheetIndexToName, UIUltraGrid DataGrid, int rowIndex)
        {
            try
            {
                
                DataTable newData = testdata.Tables[sheetIndexToName[0]];
                //GetAllColumnsOnGrid(newData);
                DataTable dtGridData = CSVHelper.CSVAsDataTable(DataGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());

                MsaaObject obj;

                if (DataGrid.MsaaName.Equals("grdCashandExpire"))
                {
                    obj = DataGrid.MsaaObject.CachedChildren.First(x => x.Name.Equals("GenericBindingList`1"));
                }

                else
                {
                    Keyboard.SendKeys(KeyboardConstants.CTRL_HOMEKEY);
                    obj = DataGrid.MsaaObject.CachedChildren.First(x => x.Name.Equals("OTCPositionList"));
                }

                Dictionary<string, int> columnToIndexMapping = obj.CachedChildren[1].GetColumnIndexMaping(newData);
                if (rowIndex >= 0)
                {
                    int rowCounter = 0;
                    foreach (DataRow dtRow in newData.Rows)
                    {
                        var Row = obj.CachedChildren[rowIndex + 1 + rowCounter];
                        DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, rowIndex + rowCounter);
                        Row.CachedChildren[0].Click(MouseButtons.Left);
                        for (int i = 0; i < newData.Columns.Count; i++)
                        {
                            if (dtGridData.Columns.Contains(newData.Columns[i].ColumnName))
                            {
                                string colName = newData.Columns[i].ColumnName.ToString();
                                string columnValue = dtRow[colName].ToString();
                                int colIndex = columnToIndexMapping[colName];
                                if (columnValue == "$#$")
                                {
                                    continue;
                                }
                                else if (colName.Equals("Trade Date") || colName.Equals("Settlement Date"))
                                {
                                    Keyboard.SendKeys(columnValue);
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);

                                    ExerciseTradesTab dt = new ExerciseTradesTab();
                                    if (dt.Warning1.IsVisible)
                                    { dt.ButtonOK1.Click(MouseButtons.Left); }

                                }
                                else if (!columnValue.Equals(""))
                                {
                                    DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                                    Wait(500);
                                    Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                                    Keyboard.SendKeys(columnValue);
                                    DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 0);
                                }
                            }
                        }
                        DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, "Symbol");
                        Row.CachedChildren[0].Click(MouseButtons.Left);
                        
                        rowCounter++;
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /*protected void GetAllColumnsOnGrid(DataTable newData)
        {
            try
            {
                List<string> columns = new List<string>();
                foreach (DataColumn item in newData.Columns)
                {
                    columns.Add(item.ColumnName);
                }
                this.GrdCreatePosition.InvokeMethod("AddColumnsToGrid", columns);

            }
            catch (Exception)
            {
                throw;
            }
        }*/
    }
}
