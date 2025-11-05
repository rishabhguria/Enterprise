using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    [UITestFixture]
    public partial class CashTransactionsUIMap : UIMap
    {
        public CashTransactionsUIMap()
        {
            InitializeComponent();
        }

        public CashTransactionsUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// To open cash transaction tab
        /// </summary>
        public void OpenCashTransactions()
        {
            try
            {
                //Shortcut to open general ledger module(CTRL + SHIFT + G)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_GL"]);
                ExtentionMethods.WaitForVisible(ref FrmCashManagementMain, 15);
               // GeneralLedger1.Click(MouseButtons.Left);
                CashTransactions.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Minimize General Ledger 
        /// </summary>
        public void MinimizeGeneralLedger()
        {
            try
            {
                KeyboardUtilities.MinimizeWindow(ref FrmCashManagementMain_UltraFormManager_Dock_Area_Top);
                //Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// To get the matching data item row
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetName"></param>
        /// <param name="DataGrid"></param>
        public void GetMatchingData(System.Data.DataSet testData, string sheetName, UIUltraGrid GrdNonTradingTransactions)
        {
            try
            {
                DataTable tableData = testData.Tables[sheetName];
                List<String> colList = new List<String>();
                for (int i = 0; i < tableData.Columns.Count; i++)
                {
                    colList.Add(tableData.Columns[i].ColumnName);
                }
                GrdCashDividends.InvokeMethod("AddColumnsToGrid", colList);
                DataTable dtGridData = CSVHelper.CSVAsDataTable(GrdCashDividends.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] matchedRow = DataUtilities.GetMatchingSingleDataRows(dtGridData, tableData);
                if (matchedRow.Length > 0)
                {
                    SelectCashTransactionMatched(matchedRow[0], dtGridData, GrdNonTradingTransactions);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// selecting the row matched.
        /// </summary>
        /// <param name="matchedRow">The matched rows.</param>
        /// <param name="dtGridData">The grid table.</param>
        public MsaaObject SelectCashTransactionMatched(DataRow matchedRow, DataTable dtGridData, UIUltraGrid grid)
        {
            try
            {
                grid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 0);
                MsaaObject msaaObject = grid.MsaaObject;
                int rowIndex = dtGridData.Rows.IndexOf(matchedRow);
                var Row = msaaObject.CachedChildren[0].CachedChildren[rowIndex + 1];
                grid.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, rowIndex);
                Row.CachedChildren[1].Click(MouseButtons.Left);
                return Row;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// To add the details of the newly added cash transaction
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <param name="GrdCashTransaction"></param>
        public void AddCashTransactionData(System.Data.DataSet testData, string sheetName, UIUltraGrid GrdCashTransaction)
        {
            try
            {
                DataTable newData = testData.Tables[sheetName];
                List<String> colList = new List<String>();
                for (int i = 0; i < newData.Columns.Count; i++)
                {
                    colList.Add(newData.Columns[i].ColumnName);
                }
                GrdCashTransaction.InvokeMethod("AddColumnsToGrid", colList);
                GrdCashTransaction.InvokeMethod("RemoveGrouping", null);
                DataTable dtGridData = CSVHelper.CSVAsDataTable(GrdCashTransaction.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                int count = dtGridData.Rows.Count;
                MsaaObject msaaObject = GrdCashTransaction.MsaaObject;
                Dictionary<string, int> columnToIndexMapping = msaaObject.CachedChildren[1].CachedChildren[1].GetColumnIndexMaping(newData);
                DataRow dtRow = newData.Rows[0];
                var Row = msaaObject.CachedChildren[1].CachedChildren[count];
                GrdCashTransaction.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, count - 2);
                Row.CachedChildren[1].Click(MouseButtons.Left);
                GrdCashTransaction.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 1);
                for (int i = 0; i < newData.Columns.Count; i++)
                {
                    string colName = newData.Columns[i].ColumnName.ToString();
                    string columnValue = dtRow[colName].ToString();
                    int colIndex = columnToIndexMapping[colName];
                    if (columnValue == "$#$")
                    {
                        continue;
                    }
                    if (columnValue.Contains('['))
                    {
                        columnValue = columnValue.Replace("[", "[[");
                    }
                    GrdCashTransaction.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                    Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                    Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                    Keyboard.SendKeys(columnValue);
                    Wait(500);
                }
                GrdCashTransaction.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 1);
                BtnSave.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public void EditCashTransactionData(System.Data.DataSet testData, string sheetName, UIUltraGrid GrdCashDividends)
        {
            try
            {
                DataTable newData = testData.Tables[sheetName];
                int rowIndex = new int();
                rowIndex = (int)GrdCashDividends.InvokeMethod("GetActiveRow", null);
                DataTable dtGridData = CSVHelper.CSVAsDataTable(GrdCashDividends.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                Dictionary<string, int> columnToIndexMapping = GrdCashDividends.MsaaObject.CachedChildren[0].CachedChildren[1].GetColumnIndexMaping(newData);
                if (rowIndex != -1)
                {
                    var Row = GrdCashDividends.MsaaObject.CachedChildren[0].CachedChildren[rowIndex + 1];
                    GrdCashDividends.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, rowIndex);
                    GrdCashDividends.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 0);
                    Row.CachedChildren[2].Click(MouseButtons.Left);
                    DataRow dtRow = newData.Rows[0];
                    for (int i = 0; i < newData.Columns.Count; i++)
                    {
                        string colName = newData.Columns[i].ColumnName.ToString();
                        string columnValue = dtRow[colName].ToString();
                        int colIndex = columnToIndexMapping[colName];
                        if (columnValue == "$#$")
                        {
                            continue;
                        }
                        if (columnValue.Contains('['))
                        {
                            columnValue = columnValue.Replace("[", "[[");
                        }
                        GrdCashDividends.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                        Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                        Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(columnValue);
                        Wait(500);
                    }
                    BtnSave.Click(MouseButtons.Left);
                }   
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public List<string> GetKeyColumnsForCashTransaction()
        {
            List<string> columnList = new List<string>();
            try
            {
                columnList.Add(TestDataConstants.COL_ACCOUNT);
                columnList.Add(TestDataConstants.COL_SYMBOL);
                columnList.Add(TestDataConstants.COL_CURRENCY);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return columnList;
        }
    }
}
