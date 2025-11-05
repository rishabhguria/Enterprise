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
    public partial class CashJournalUIMap : UIMap
    {
        public CashJournalUIMap()
        {
            InitializeComponent();
        }

        public CashJournalUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Opens the cash journal window
        /// </summary>
        protected void OpenCashJournal()
        {
            try
            {
                OpenGeneralLedger();
                CashJournal.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        protected void OpenGeneralLedger()
        {
            try
            {
                //Shortcut to open general ledger module(CTRL + SHIFT + G)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_GL"]);
                ExtentionMethods.WaitForVisible(ref FrmCashManagementMain, 15);
               // GeneralLedger1.Click(MouseButtons.Left);
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
               // Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Close General Ledger 
        /// </summary>
        protected void CloseGeneralLedgerTab()
        {
            try
            {
                KeyboardUtilities.CloseWindow(ref FrmCashManagementMain_UltraFormManager_Dock_Area_Top);
                FrmCashManagementMain.IsOptional = true;
                if (TransactionValueSave.IsVisible)
                {
                    ButtonNo1.Click(MouseButtons.Left);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Open opening Balance tab.
        /// </summary>
        public void OpenOpeningBalance()
        {
            try
            {
                OpenCashJournal();
                OpeningBalance.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// opens dividend tab
        /// </summary>
        public void OpenDividendTab()
        {
            try
            {
                OpenCashJournal();
                Dividend.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// get key columns for comparing rows in cash journal
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeyColumnsForCashJournal()
        {
            List<string> columnList = new List<string>();
            try
            {
                columnList.Add(TestDataConstants.COL_ACCOUNT);
                columnList.Add(TestDataConstants.COL_SYMBOL);
                columnList.Add(TestDataConstants.COL_CURRENCY);
                columnList.Add(TestDataConstants.COL_CASH_SUBACCOUNT);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return columnList;
        }

        /// <summary>
        /// Selecting matched rows
        /// </summary>
        /// <param name="matchedRows">The matched rows.</param>
        /// <param name="dtGridData">The grid table.</param>
        public MsaaObject SelectTransaction(DataRow[] matchedRows, DataTable dtGridData, UIUltraGrid grid)
        {
            try
            {
                MsaaObject msaaObject = grid.MsaaObject;
                bool singleTranMatched = true;
                string transID = "";
                int index = int.MinValue;
                foreach (DataRow dt in matchedRows)
                {
                    if (transID == "" && singleTranMatched)
                    {
                        transID = dt["TransactionID"].ToString();
                    }
                    else if (transID != dt["TransactionID"].ToString())
                    {
                        singleTranMatched = false; break;
                    }
                    index = dtGridData.Rows.IndexOf(dt);
                }
                if (singleTranMatched && index >= 0)
                {
                    var Row = msaaObject.CachedChildren[1].CachedChildren[index];
                    grid.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                    grid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 0);
                    Row.CachedChildren[0].Click(MouseButtons.Left);
                    return Row;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// selecting the rows.
        /// </summary>
        /// <param name="matchedRow">The matched rows.</param>
        /// <param name="dtGridData">The grid table.</param>
        public MsaaObject SelectTransactionItem(DataRow matchedRow, DataTable dtGridData, UIUltraGrid grid)
        {
            try
            {
                MsaaObject msaaObject = grid.MsaaObject;
                int rowIndex = dtGridData.Rows.IndexOf(matchedRow);
                var Row = msaaObject.CachedChildren[1].CachedChildren[rowIndex + 1];
                grid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 0);
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
        /// To add the details of the newly added transaction
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <param name="DataGrid"></param>
        public void AddTransaction(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName, UIUltraGrid DataGrid)
        {
            try
            {
                DataTable newData = testData.Tables[sheetIndexToName[0]];
                List<String> colList = new List<String>();
                for (int i = 0; i < newData.Columns.Count; i++)
                {
                    colList.Add(newData.Columns[i].ColumnName);
                }
                DataGrid.InvokeMethod("AddColumnsToGrid", colList);
                DataGrid.InvokeMethod("RemoveGrouping", null);
                Wait(200);
                GrdNonTradingTransactions.Click(MouseButtons.Left);
                MsaaObject obj = DataGrid.MsaaObject.CachedChildren.First(x => x.Name.Equals("GenericBindingList`1"));
                Dictionary<string, int> columnToIndexMapping = obj.CachedChildren[1].GetColumnIndexMaping(newData);
                DataTable dtGridData = CSVHelper.CSVAsDataTable(DataGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                int count = dtGridData.Rows.Count;
                int rowCounter = 1;
                foreach (DataRow dtRow in newData.Rows)
                {
                    var Row = obj.CachedChildren[count - 2 + rowCounter];
                    DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, count - 3 + rowCounter);
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
                        if (columnValue.Contains('['))
                        {
                            columnValue = columnValue.Replace("[", "[[");
                        }
                        DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                        Wait(500);
                        Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                        Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(columnValue);
                    }
                    DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 0);
                    rowCounter++;
                }
                BtnSave.Click(MouseButtons.Left);
                if (Information.IsVisible)
                {
                    ButtonOK1.Click(MouseButtons.Left);
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
        /// To edit the details of the selected transaction
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <param name="DataGrid"></param>
        public void EditTransaction(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName, UIUltraGrid DataGrid, int rowIndex)
        {
            try
            {
                DataTable newData = testData.Tables[sheetIndexToName[0]];
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
                            if (columnValue.Contains('['))
                            {
                                columnValue = columnValue.Replace("[", "[[");
                            }
                            DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                            Wait(500);
                            Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                            Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                            Keyboard.SendKeys(columnValue);
                        }
                        DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 0);
                        rowCounter++;
                    }
                    BtnSave.Click(MouseButtons.Left);
                    if (Information.IsVisible)
                    {
                        ButtonOK1.Click(MouseButtons.Left);
                    }
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
        /// To edit the details of the selected transaction
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <param name="DataGrid"></param>
        public void EditTransactionItem(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName, UIUltraGrid DataGrid, int rowIndex)
        {
            try
            {
                DataTable newData = testData.Tables[sheetIndexToName[0]];
                DataTable dtGridData = CSVHelper.CSVAsDataTable(DataGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                MsaaObject obj = DataGrid.MsaaObject.CachedChildren.First(x => x.Name.Equals("GenericBindingList`1"));
                Dictionary<string, int> columnToIndexMapping = obj.CachedChildren[1].GetColumnIndexMaping(newData);
                DataRow dtRow = newData.Rows[0];
                if (rowIndex >= 0)
                {
                    var Row = obj.CachedChildren[rowIndex + 1];
                    DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, rowIndex);
                    DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 0);
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
                        if (columnValue.Contains('['))
                        {
                            columnValue = columnValue.Replace("[", "[[");
                        }
                        DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                        Wait(500);
                        Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                        Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(columnValue);
                    }
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
        /// To add the details of the newly added transaction
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <param name="DataGrid"></param>
        public void AddTransactionItem(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName, UIUltraGrid DataGrid)
        {
            try
            {
                    DataTable newData = testData.Tables[sheetIndexToName[0]];
                    List<String> colList = new List<String>();
                    for (int i = 0; i < newData.Columns.Count; i++)
                    {
                        colList.Add(newData.Columns[i].ColumnName);
                    }
                    DataGrid.InvokeMethod("AddColumnsToGrid", colList);
                    DataGrid.InvokeMethod("RemoveGrouping", null);
                DataTable dtGridData = CSVHelper.CSVAsDataTable(DataGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                int count = dtGridData.Rows.Count;
                count += 1;
                    MsaaObject obj = DataGrid.MsaaObject.CachedChildren.First(x => x.Name.Equals("GenericBindingList`1"));
                    Dictionary<string, int> columnToIndexMapping = obj.CachedChildren[1].GetColumnIndexMaping(newData);
                DataRow dtRow = newData.Rows[0];
                var Row = obj.CachedChildren[count - 1];
                DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, count - 2);
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
                            if (columnValue.Contains('['))
                            {
                                columnValue = columnValue.Replace("[", "[[");
                            }
                            DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                            Wait(500);
                            Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                            Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                            Keyboard.SendKeys(columnValue);
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
        /// To add the details of the newly added transaction
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <param name="DataGrid"></param>
        public void AddMultipleTransactionItem(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName, UIUltraGrid DataGrid)
        {
            try
            {
                DataTable newData = testData.Tables[sheetIndexToName[0]];
                for (int i = 0; i < newData.Rows.Count; i++)
                {
                    GrdOpeningBalance.Click(MouseButtons.Left);
                    GrdOpeningBalance.Click(MouseButtons.Right);
                    if (!PopupMenuDropDown.IsVisible)
                    {
                        GrdOpeningBalance.Click(MouseButtons.Right);
                    }
                    AddTransactionItem1.Click(MouseButtons.Left);
                }
                List<String> colList = new List<String>();
                for (int i = 0; i < newData.Columns.Count; i++)
                {
                    colList.Add(newData.Columns[i].ColumnName);
                }
                DataGrid.InvokeMethod("AddColumnsToGrid", colList);
                DataGrid.InvokeMethod("RemoveGrouping", null);
                Wait(2000);
                GrdNonTradingTransactions.Click(MouseButtons.Left);
                Wait(2000);
                 MsaaObject obj = DataGrid.MsaaObject.CachedChildren[2];
                Wait(10000);
                obj = DataGrid.MsaaObject.CachedChildren[2];
              //  MsaaObject obj = DataGrid.MsaaObject.CachedChildren.First(x => x.Name.Equals("GenericBindingList`1"));
                Dictionary<string, int> columnToIndexMapping = obj.CachedChildren[1].GetColumnIndexMaping(newData);
                DataTable dtGridData = CSVHelper.CSVAsDataTable(DataGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                //int count = dtGridData.Rows.Count;
                var rowCounter = 3;
                //DataRow dtRow = newData.Rows[0];
                foreach (DataRow dtRow in newData.Rows)
                {
                    var Row = obj.CachedChildren[rowCounter];
                    DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, rowCounter - 1);
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
                        if (columnValue.Contains('['))
                        {
                            columnValue = columnValue.Replace("[", "[[");
                        }
                        DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                        Wait(500);
                        Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                        Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(columnValue);
                    }
                    DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 0);
                    rowCounter++;
                }
                BtnSave.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}

    

