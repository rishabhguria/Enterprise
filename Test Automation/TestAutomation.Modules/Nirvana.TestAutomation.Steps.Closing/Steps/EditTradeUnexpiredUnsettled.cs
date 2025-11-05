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
    class EditTradeUnexpiredUnsettled : ExerciseTradesTab, ITestStep
    {
        public TestResult RunTest(DataSet testdata, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenClosingUI();
                ExpirationDivideSettlement.Click(MouseButtons.Left);
                int rowIndex = (int)GrdAccountUnexpired.InvokeMethod("GetActiveRow", null);
                EditUnexpiredUnsettledGrid(testdata, sheetIndexToName, GrdAccountUnexpired, rowIndex);
                Keyboard.SendKeys(KeyboardConstants.HOMEKEY);
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeClosing();
            }
            return _res;
        }


        public void EditUnexpiredUnsettledGrid(System.Data.DataSet testdata, Dictionary<int, string> sheetIndexToName, UIUltraGrid DataGrid, int rowIndex)
        {

            try
            {
                DataTable newData = testdata.Tables[sheetIndexToName[0]];
                DataTable dtGridData = CSVHelper.CSVAsDataTable(DataGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                MsaaObject obj = DataGrid.MsaaObject.CachedChildren.First(x => x.Name.Equals("GenericBindingList`1"));
                Dictionary<string, int> columnToIndexMapping = obj.CachedChildren[1].GetColumnIndexMaping(newData);
                if (rowIndex >= 0)
                {
                    int rowCounter = 0;
                    foreach (DataRow dtRow in newData.Rows)
                    {
                        var Row = obj.CachedChildren[rowIndex + 1 + rowCounter];
                        DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, rowIndex + rowCounter);
                        Row.CachedChildren[1].Click(MouseButtons.Left);
                        for (int i = 0; i < newData.Columns.Count; i++)
                        {
                            string colName = newData.Columns[i].ColumnName.ToString();
                            string columnValue = dtRow[colName].ToString();
                            int colIndex = columnToIndexMapping[colName];
                            if (columnValue == "$#$")
                            {
                                continue;
                            }
                            DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                            Wait(500);
                            Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                            Keyboard.SendKeys(columnValue);
                        }
                        DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 0);
                        rowCounter++;
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
