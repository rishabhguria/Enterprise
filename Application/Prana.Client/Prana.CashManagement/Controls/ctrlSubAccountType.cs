using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.CashManagement.Controls
{
    public partial class ctrlSubAccountType : UserControl
    {
        public bool _isUnsavedChanges;
        DataSet _dsActivities = new DataSet();
        private DataSet _dataSetMasterCategory = new DataSet();
        DataTable _dtSubAccounts = new DataTable();
        DataTable _dtSubAccountType = new DataTable();
        DataTable _dtTransactionSource = new DataTable();
        ValueList _vlTransactionSource = new ValueList();
        Dictionary<int, string> _dictSubAccountType = new Dictionary<int, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ctrlSubAccountType()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set theme on load of UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtrlSubAccountType_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.ApplyTheme)
                {
                    grdSubAccountDetails.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
                }

                if (!CustomThemeHelper.IsDesignMode())
                {
                    InitializeDataSets();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the data set.
        /// </summary>
        /// <param name="dsActivity">The ds activity.</param>
        /// <returns></returns>
        internal DataSet UpdateDataSet(DataSet dsActivity)
        {
            DataSet tempDataSet = new DataSet();
            try
            {
                foreach (DataTable table in dsActivity.Tables)
                {
                    if (!table.TableName.Equals(CashManagementConstants.TABLE_SUBACCOUNTSTYPE))
                        tempDataSet.Tables.Add(table.Copy());
                }
                tempDataSet.Tables.Add(_dtSubAccountType.Copy());

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return tempDataSet;
        }

        /// <summary>
        /// Initialize dataset for grid
        /// </summary>
        internal void InitializeDataSets()
        {
            try
            {
                _dataSetMasterCategory = WindsorContainerManager.GetAllAccountTablesFromDB();
                _dtSubAccounts = _dataSetMasterCategory.Tables[CashManagementConstants.TABLE_SUBACCOUNTS];
                _dsActivities = CachedDataManager.GetInstance.GetAllActivityTables();
                _dtSubAccountType = _dsActivities.Tables[CashManagementConstants.TABLE_SUBACCOUNTSTYPE];
                _dtTransactionSource = _dsActivities.Tables[CashManagementConstants.TABLE_TRANSACTIONSOURCE];
                _vlTransactionSource = GetValueListFromDataTable(_dtTransactionSource, CashManagementConstants.COLUMN_ACCOUNTTYPE);
                this.grdSubAccountDetails.DataSource = _dtSubAccountType;
                _dictSubAccountType = FillSubAccountDictionary();
                foreach (UltraGridRow row in grdSubAccountDetails.Rows)
                {
                    ((System.Data.DataRowView)(row.ListObject)).Row.SetColumnError(CashManagementConstants.COLUMN_SUBACCOUNTTYPE, string.Empty);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Fill the details in the dictionary(dictSubAccountType) for fast working fetching
        /// </summary>
        private Dictionary<int, string> FillSubAccountDictionary()
        {
            Dictionary<int, string> dictSubAccountType = new Dictionary<int, string>();
            try
            {
                foreach (DataRow item in _dtSubAccountType.Rows)
                {
                    dictSubAccountType.Add(int.Parse(item[CashManagementConstants.COLUMN_SUBACCOUNTTYPEID].ToString()), item[CashManagementConstants.COLUMN_SUBACCOUNTTYPE].ToString().ToUpper().Trim());
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
            return dictSubAccountType;
        }

        /// <summary>
        /// Initialize the content of grid grdSubAccountDetails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSubAccountDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                List<string> lsColumnsToDisplay = new List<string>(new string[] { CashManagementConstants.COLUMN_SUBACCOUNTTYPE });
                UltraWinGridUtils.HideColumns(grdSubAccountDetails.DisplayLayout.Bands[0]);
                UltraWinGridUtils.SetBand(lsColumnsToDisplay, grdSubAccountDetails.DisplayLayout.Bands[0]);

                grdSubAccountDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_SUBACCOUNTTYPE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                grdSubAccountDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_SUBACCOUNTTYPE].Width = 180;
                grdSubAccountDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_SUBACCOUNTTYPE].Header.Caption = CashManagementConstants.CAPTION_SUBACCOUNTTYPE;

                e.Layout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                grdSubAccountDetails.DisplayLayout.Override.RowAppearance.BackColor = Color.Black;
                grdSubAccountDetails.DisplayLayout.Override.RowAppearance.ForeColor = Color.White;
                grdSubAccountDetails.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Gray;
                grdSubAccountDetails.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.Black;
                grdSubAccountDetails.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Cell changed listener for grdSubAccountDetails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSubAccountDetails_CellValueChanged(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {

                DataRow dr = ((System.Data.DataRowView)((sender as UltraGrid).ActiveRow.ListObject)).Row;
                _isUnsavedChanges = true;
                int subAccountId;
                int.TryParse(dr[CashManagementConstants.COLUMN_SUBACCOUNTTYPEID].ToString(), out subAccountId);
                switch (e.Cell.Column.Key)
                {
                    case CashManagementConstants.COLUMN_SUBACCOUNTTYPE:
                        if (string.IsNullOrEmpty(e.Cell.Text.ToString()) || string.IsNullOrWhiteSpace(e.Cell.Text.ToString()))
                        {
                            dr.SetColumnError(CashManagementConstants.COLUMN_SUBACCOUNTTYPE, "Please enter valid value in sub account type...");
                        }
                        else if (_dictSubAccountType.ContainsKey(subAccountId) && _dictSubAccountType[subAccountId].Equals(e.Cell.Text.ToUpper().Trim()))
                        {
                            dr.SetColumnError(CashManagementConstants.COLUMN_SUBACCOUNTTYPE, string.Empty);
                        }
                        else if (_dictSubAccountType.ContainsValue(e.Cell.Text.ToUpper().Trim().ToString()))
                        {
                            dr.SetColumnError(CashManagementConstants.COLUMN_SUBACCOUNTTYPE, "SubAccount already exists...");
                        }
                        else
                        {
                            dr.SetColumnError(CashManagementConstants.COLUMN_SUBACCOUNTTYPE, string.Empty);
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// get the value list from its respective data table
        /// </summary>
        /// <param name="tableToConvertWithSingleColumn">Required data table</param>
        /// <param name="columnName">Column name to be fetched</param>
        /// <returns>Value list</returns>
        private ValueList GetValueListFromDataTable(DataTable tableToConvertWithSingleColumn, string columnName)
        {

            ValueList _vlTransactionSourceTemp = new ValueList();
            try
            {
                foreach (DataRow item in tableToConvertWithSingleColumn.Rows)
                {
                    _vlTransactionSourceTemp.ValueListItems.Add(item[columnName]);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return _vlTransactionSourceTemp;
        }

        /// <summary>
        /// Add new subaccount mapping
        /// </summary>
        internal void AddNewSubAccountMapping()
        {
            try
            {
                _dsActivities = (grdSubAccountDetails.DataSource as DataTable).DataSet;
                DataRow row = _dsActivities.Tables[CashManagementConstants.TABLE_SUBACCOUNTSTYPE].NewRow();
                row.SetColumnError(CashManagementConstants.COLUMN_SUBACCOUNTTYPE, "Please enter valid value in Sub Account Type");
                _dsActivities.Tables[CashManagementConstants.TABLE_SUBACCOUNTSTYPE].Rows.Add(row);
                grdSubAccountDetails.Rows[grdSubAccountDetails.Rows.Count - 1].Selected = true;
                grdSubAccountDetails.Rows[grdSubAccountDetails.Rows.Count - 1].Activate();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Delete the existing sub account type mapping
        /// </summary>
        internal void DeleteSubAccountType()
        {
            try
            {
                if (this.grdSubAccountDetails.ActiveRow != null && this.grdSubAccountDetails.ActiveRow.Index >= 0)
                {
                    object subAccountIDObject = grdSubAccountDetails.ActiveRow.Cells[CashManagementConstants.COLUMN_SUBACCOUNTTYPEID].Value;
                    int subAccountId;
                    int.TryParse(subAccountIDObject.ToString(), out subAccountId);
                    bool isSubAccountInUse = false;
                    if (subAccountId != 0)
                    {
                        foreach (DataRow row in _dtSubAccounts.Rows)
                        {
                            int subAccountIdInTable;
                            int.TryParse(row[CashManagementConstants.COLUMN_SUBACCOUNTTYPEID].ToString(), out subAccountIdInTable);
                            if (subAccountIdInTable == subAccountId)
                            {
                                isSubAccountInUse = true;
                                break;
                            }
                        }
                        if (subAccountId != 0 && isSubAccountInUse)
                        {
                            MessageBox.Show("Sub Account Type already in use.", "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (MessageBox.Show("Do you want to delete the selected Sub Account Type?", "Delete SubAccount Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                grdSubAccountDetails.Rows[this.grdSubAccountDetails.ActiveRow.Index].Delete(false);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Select a mapping row to delete.", "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Set the datasource get called when user clicks on save or refresh all accounts
        /// </summary>
        internal void SetDataSource()
        {
            InitializeDataSets();
        }

        /// <summary>
        /// Export the data to excel sheet
        /// </summary>
        /// <returns></returns>
        internal Infragistics.Documents.Excel.Workbook GetGridDataToExportToExcel()
        {
            Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
            try
            {

                string workbookName = "ActivityJournalMapping_" + DateTime.Now.Date.ToString("yyyyMMdd");
                workBook.Worksheets.Add(workbookName);

                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];

                workBook = this.ultraGridExcelExporter1.Export(this.grdSubAccountDetails, workBook.Worksheets[workbookName]);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return workBook;
        }
    }
}
