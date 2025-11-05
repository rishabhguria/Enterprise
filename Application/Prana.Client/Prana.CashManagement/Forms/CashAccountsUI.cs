using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace Prana.CashManagement
{
    public partial class CashAccountsUI : Form, ICashAccounts, IPublishing
    {
        private DataSet _dataSetMasterCategory = new DataSet();
        private bool IsBottomUIEnabled = true;

        #region activities
        //private const string COLUMN_JOURNALCODE = "JournalCode";   
        const string ERROR = "Please correct the activity journal mapping.";

        //string IdColumn = string.Empty;
        DataSet _dsActivities = new DataSet();
        ValueList _vlActivityType = new ValueList();
        ValueList _vlCashActivityType = new ValueList();
        ValueList _vlAmountType = new ValueList();
        ValueList _vlSubAccount = new ValueList();
        //ValueList _vlCreditAccount = new ValueList();
        ValueList _vlCashTransactionType = new ValueList();
        private static CashAccountsUI _cashAccountsUI;
        #endregion

        public static CashAccountsUI Instance
        {
            get
            {
                if (_cashAccountsUI == null)
                    _cashAccountsUI = new CashAccountsUI();
                return _cashAccountsUI;
            }
        }

        public CashAccountsUI()
        {
            _cashAccountsUI = this;
            InitializeComponent();
            CreateSubscriptionServicesProxy();
            this.Disposed += new EventHandler(CashAccountsUI_Disposed);
        }

        DuplexProxyBase<ISubscription> _proxy;

        public void CreateSubscriptionServicesProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                SubscribeForCashTransactions();
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

        void CashAccountsUI_Disposed(object sender, EventArgs e)
        {
            if (FormClosedHandler != null)
            {
                FormClosedHandler(this, e);
            }
        }

        private void CashAccountsUI_Load(object sender, EventArgs e)
        {
            #region activities
            //_vlActivityType = CashAccountCache.GetInstance.GetActivityTypeValueList();
            //_vlAmountType = CashAccountCache.GetInstance.GetAmountTypeValueList();
            //_vlCashActivityType = CashAccountCache.GetInstance.GetCashActivityTypeValueList();
            //_vlCashTransactionType = CashAccountCache.GetInstance.GetCashTransactionTypeValueList();
            //_vlSubAccount = CashAccountCache.GetInstance.GetSubAccountTypeValueList();
            //SetListViewActivityTypes();
            //ctrlActivityJournalMapping1.SetListViewActivityJournals();
            //SetListViewCashTransactionTypes();
            #endregion

            this.Disposed += new EventHandler(CashAccountsUI_Disposed);
            SetButtonsColor();
            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CASH_ACCOUNTS);
            if (CustomThemeHelper.ApplyTheme)
            {
                this.Status.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                this.Status.ForeColor = System.Drawing.Color.WhiteSmoke;
                this.toolStripStatusLabel1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
            }
        }

        /// <summary>
        /// Used for changing the color of buttons. The indices and their colors are as follows:
        /// 0 & 3: For the Green Shade
        /// 1 & 4: For the Neutral Shade
        /// 2 & 5: For the Red Shade 
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                ultraButton1.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                ultraButton1.ForeColor = System.Drawing.Color.White;
                ultraButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraButton1.UseAppStyling = false;
                ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnDelete.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnDelete.ForeColor = System.Drawing.Color.White;
                btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDelete.UseAppStyling = false;
                btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAdd.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnAdd.ForeColor = System.Drawing.Color.White;
                btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAdd.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAdd.UseAppStyling = false;
                btnAdd.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRestoreDefault.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRestoreDefault.ForeColor = System.Drawing.Color.White;
                btnRestoreDefault.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRestoreDefault.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRestoreDefault.UseAppStyling = false;
                btnRestoreDefault.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExport.ForeColor = System.Drawing.Color.White;
                btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExport.UseAppStyling = false;
                btnExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRefresh.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRefresh.ForeColor = System.Drawing.Color.White;
                btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRefresh.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRefresh.UseAppStyling = false;
                btnRefresh.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        //TODO: Make this saving code more clear
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = String.Empty;
                _dsActivities = ctrlActivityJournalMapping1.GetDataSet();
                _dsActivities = ctrlSubAccountType.UpdateDataSet(_dsActivities);
                ctrlActivityJournalMapping1.CreateActivityJournalMappingDictionary();
                _dataSetMasterCategory = ctrlAccounts1.GetDataSet();
                string selectedNodeIndex = ctrlActivityJournalMapping1.GetSelectedTreeNodeIndex();
                ctrlActivityType1.CleanTableErrors();
                if (_dsActivities.HasErrors || _dataSetMasterCategory.HasErrors)
                {
                    // _dataSetMasterCategory.
                    toolStripStatusLabel1.Text = "Please Check for the Errors in Details.";
                }
                else if (_dataSetMasterCategory.GetChanges() != null || _dsActivities.GetChanges() != null)
                {
                    if (_dsActivities.GetChanges() != null && _dsActivities.GetChanges().Tables.Contains(CashManagementConstants.TABLE_SUBACCOUNTSTYPE))
                    {
                        DataTable subAccountType = _dsActivities.GetChanges().Tables[CashManagementConstants.TABLE_SUBACCOUNTSTYPE];
                        foreach (DataRow row in subAccountType.Rows)
                        {
                            if (row.RowState != DataRowState.Deleted)
                            {
                                string subAccountId = row[CashManagementConstants.COLUMN_SUBACCOUNTTYPEID].ToString();
                                foreach (DataRow row2 in _dsActivities.Tables[CashManagementConstants.TABLE_SUBACCOUNTSTYPE].Rows)
                                {
                                    if (row2[CashManagementConstants.COLUMN_SUBACCOUNTTYPEID].ToString() == subAccountId)
                                    {
                                        row2[CashManagementConstants.COLUMN_SUBACCOUNTTYPE] = row2[CashManagementConstants.COLUMN_SUBACCOUNTTYPE].ToString().Trim();
                                    }
                                }
                            }
                        }
                    }
                    if (_dataSetMasterCategory.GetChanges() != null)
                    {
                        CashDataManager.GetInstance().UpdateCashAccountsTablesInDB(_dataSetMasterCategory.GetChanges());
                        _dataSetMasterCategory.AcceptChanges();
                        ctrlAccounts1.isUnSavedChanges = false;
                        CachedDataManager.GetInstance.RefreshAccountData();
                        if (SubAccountUpdated != null)
                            SubAccountUpdated(null, null);
                        toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "] Cash Accounts Saved Successfully.";
                        ctrlAccounts1.SetDataSet(_dataSetMasterCategory);
                        ctrlSubAccountType.SetDataSource();
                    }
                    if (_dsActivities.GetChanges() != null)
                    {
                        string ErrorMessage = CashManagementValidations.GetInstance.validateActivityJournalMapping(_dsActivities);

                        //https://jira.nirvanasolutions.com:8443/browse/PRANA-20534
                        DataTable modifiedRows = _dsActivities.Tables["ActivityJournalMapping"].GetChanges(DataRowState.Modified);
                        DataTable addedRows = _dsActivities.Tables["ActivityJournalMapping"].GetChanges(DataRowState.Added);

                        DataTable modifiedAndAddedRows = new DataTable();
                        if (modifiedRows != null)
                        {
                            modifiedAndAddedRows.Merge(modifiedRows);
                        }
                        if (addedRows != null)
                        {
                            modifiedAndAddedRows.Merge(addedRows);
                        }

                        if (modifiedAndAddedRows.Rows.Count > 0 && ErrorMessage.Equals(String.Empty))
                        {
                            foreach (DataRow row in modifiedAndAddedRows.Rows)
                            {
                                if (int.Parse(row[CashManagementConstants.COLUMN_DEBITACCOUNT].ToString()) == int.MinValue || int.Parse(row[CashManagementConstants.COLUMN_CREDITACCOUNT].ToString()) == int.MinValue)
                                {
                                    ErrorMessage += ERROR;
                                    break;
                                }
                            }
                        }

                        if (ErrorMessage.Length == 0)
                        {
                            CashDataManager.GetInstance().UpdateCashActivityTablesInDB(_dsActivities.GetChanges());

                            _dsActivities.AcceptChanges();
                            ctrlSubAccountType._isUnsavedChanges = false;
                            ctrlActivityJournalMapping1.isUnSavedChanges = false;
                            ctrlActivityType1._isUnSavedChanges = false;
                            CachedDataManager.GetInstance.RefreshAccountData();
                            toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "] Cash Activity Saved Successfully.";
                        }
                        else
                        {
                            _dsActivities.RejectChanges();
                            MessageBox.Show(ErrorMessage, "Mapping Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        ctrlSubAccountType.SetDataSource();
                        ctrlActivityType1.SetDataSources();
                        ctrlAccounts1.InitializeAccountsControl();
                        ctrlActivityJournalMapping1.SetDataSources();
                        ctrlActivityJournalMapping1.SetSelectedNode(selectedNodeIndex);
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "] Nothing to Save.";
                }
                if (!toolStripStatusLabel1.Visible)
                    toolStripStatusLabel1.Visible = true;
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

        //private void btnSaveActivity_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (_dsActivities.HasErrors)
        //        {
        //            // _dsActivities.
        //            MessageBox.Show("Please Check for the Errors in Details.", "Prana", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        else if (_dsActivities.GetChanges() != null)
        //        {
        //            //CashAccountDataManager.UpdateCashAccountsTablesInDB(_dsActivities);
        //            CashDataManager.GetInstance().UpdateCashActivityTablesInDB(_dsActivities);
        //            _dsActivities.AcceptChanges();
        //            CachedDataManager.GetInstance.RefreshAccountData();
        //            MessageBox.Show("Cash Activity Saved Successfully.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        else if (_dsActivities.GetChanges() != null)
        //        {
        //            string ErrorMessage = CashManagementValidations.GetInstance.validateActivityJournalMapping(_dsActivities);
        //            if (ErrorMessage.Length == 0)
        //            {
        //                //CashAccountDataManager.UpdateCashAccountsTablesInDB(_dsActivities);
        //                CashDataManager.GetInstance().UpdateCashActivityTablesInDB(_dsActivities);
        //                _dsActivities.AcceptChanges();
        //                CachedDataManager.GetInstance.RefreshAccountData();
        //                MessageBox.Show("Cash Activity Saved Successfully.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //            else
        //            {
        //                MessageBox.Show(ErrorMessage, "Journal Mapping Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);                          
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Nothing to Save.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //private void btnScreenshot_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.Width = 700;
        //        SnapShotManager.GetInstance().TakeSnapshot(this);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        public static event EventHandler SubAccountUpdated;

        //private int GetNewSubAccountID()
        //{
        //    return ++_maxSubAccountID;
        //}

        //private int GetNewMasterFundID()
        //{
        //    return ++_maxSubCategoryID;
        //}

        #region ICashAccounts Members

        public Form Reference()
        {
            return this;
        }

        public event EventHandler FormClosedHandler;

        #endregion

        //private void grdData_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        //{

        //    ValidateRow(e.Cell);
        //}

        //private void ValidateRow(UltraGridCell cell)
        //{
        //    try
        //    {
        //        DataRow row = ((System.Data.DataRowView)(cell.Row.ListObject)).Row;
        //        string columnModified = cell.Column.Key.ToString();

        //        switch (columnModified)
        //        {
        //            case CashManagementConstants.COLUMN_NAME:
        //                if (cell.Text.Equals(String.Empty))
        //                {
        //                    row.SetColumnError(columnModified, "Name can not be Empty !");
        //                }
        //                else if (IsRepeatedName(cell.Text, columnModified, row))
        //                {
        //                    row.SetColumnError(columnModified, "Name Already Exists !");
        //                }
        //                else
        //                {
        //                    row.SetColumnError(columnModified, "");
        //                }
        //                if (row[CashManagementConstants.COLUMN_ACRONYM].ToString().Equals(String.Empty))
        //                {
        //                    row.SetColumnError(CashManagementConstants.COLUMN_ACRONYM, "Acronym can not be Empty !");
        //                }
        //                if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_TYPEID) && row[CashManagementConstants.COLUMN_TYPEID].ToString().Equals(String.Empty))
        //                {
        //                    row.SetColumnError(CashManagementConstants.COLUMN_TYPEID, "Select a Type !");
        //                }
        //                else if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_TYPEID) && row[CashManagementConstants.COLUMN_TYPEID].ToString() == int.MinValue.ToString())
        //                {
        //                    row.SetColumnError(CashManagementConstants.COLUMN_TYPEID, "Select a valid Type !");
        //                }
        //                break;

        //            case CashManagementConstants.COLUMN_ACRONYM:

        //                if (cell.Text.Equals(String.Empty))
        //                {
        //                    row.SetColumnError(columnModified, "Acronym can not be Empty !");
        //                }
        //                else if (IsRepeatedName(cell.Text, columnModified, row))
        //                {
        //                    row.SetColumnError(columnModified, "Acronym Already Exists !");
        //                }
        //                else
        //                {
        //                    row.SetColumnError(columnModified, "");
        //                }
        //                if (row[CashManagementConstants.COLUMN_NAME].ToString().Equals(String.Empty))
        //                {
        //                    row.SetColumnError(CashManagementConstants.COLUMN_NAME, "Acronym can not be Empty !");
        //                }

        //                //if (row[COLUMN_SUBCATEGORY].ToString().Equals(String.Empty))
        //                //{
        //                //    row.SetColumnError(CashManagementConstants.COLUMN_NAME, "Subcategory can not be Empty !");
        //                //}
        //                if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_TYPEID) && row[CashManagementConstants.COLUMN_TYPEID].ToString().Equals(String.Empty))
        //                {
        //                    row.SetColumnError(CashManagementConstants.COLUMN_TYPEID, "Select a Type !");
        //                }
        //                else if (row.Table.Columns.Contains(CashManagementConstants.COLUMN_TYPEID) && row[CashManagementConstants.COLUMN_TYPEID].ToString() == int.MinValue.ToString())
        //                {
        //                    row.SetColumnError(CashManagementConstants.COLUMN_TYPEID, "Select a valid Type !");
        //                }
        //                break;


        //            case CashManagementConstants.COLUMN_TRANSACTIONTYPEID:
        //                if (cell.Text.Equals(String.Empty))
        //                {
        //                    row.SetColumnError(columnModified, "Select a Type !");
        //                }
        //                else if (cell.Text == ApplicationConstants.C_COMBO_SELECT)
        //                {
        //                    row.SetColumnError(columnModified, "Select a valid Type !");
        //                }
        //                else
        //                {
        //                    row.SetColumnError(columnModified, "");
        //                }
        //                if (row[CashManagementConstants.COLUMN_NAME].ToString().Equals(String.Empty))
        //                {
        //                    row.SetColumnError(CashManagementConstants.COLUMN_NAME, "Acronym can not be Empty !");
        //                }
        //                if (row[CashManagementConstants.COLUMN_ACRONYM].ToString().Equals(String.Empty))
        //                {
        //                    row.SetColumnError(CashManagementConstants.COLUMN_ACRONYM, "Acronym can not be Empty !");
        //                }
        //                break;
        //            case CashManagementConstants.COLUMN_SUBCATEGORYNAME:
        //                if (cell.Text.Equals(String.Empty))
        //                {
        //                    row.SetColumnError(CashManagementConstants.COLUMN_SUBCATEGORYNAME, "Subcategory can not be Empty !");
        //                }
        //                else
        //                {
        //                    row.SetColumnError(CashManagementConstants.COLUMN_SUBCATEGORYNAME, "");
        //                }
        //                break;

        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //private bool IsRepeatedName(string cellText, string column, DataRow currentRow)
        //{
        //    bool isRepeated = false;
        //    try
        //    {


        //        foreach (DataRow row in currentRow.Table.Rows)
        //        {
        //            if (row.RowState != DataRowState.Deleted)
        //            {
        //                if (row == currentRow) // Skip the current row check in other rows
        //                {
        //                    continue;
        //                }

        //                if (string.Compare(row[column].ToString().Trim(), cellText.Trim(), true) == 0)
        //                {
        //                    isRepeated = true;
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return isRepeated;
        //}

        //private bool IfSubAccountIsInMappingFile(string acronym)
        //{
        //    bool isInUse = false;
        //    try
        //    {
        //        if (!System.IO.File.Exists(_mappingDirPath))
        //        {
        //            return false;
        //        }
        //        if (_dtMappingFile == null)
        //        {
        //            _dtMappingFile = CashAccountDataManager.GetSubAccountMappingFileTable(_mappingDirPath);
        //        }
        //        foreach (DataRow row in _dtMappingFile.Rows)
        //        {
        //            if (row["PBSubAccountCode"].ToString().Equals(acronym))
        //            {
        //                isInUse = true;
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return isInUse;
        //}

        private void CashAccountsUI_FormClosing(object sender, FormClosingEventArgs e)
        {

            _dsActivities = ctrlActivityJournalMapping1.GetDataSet();
            _dataSetMasterCategory = ctrlAccounts1.GetDataSet();


            if (_dsActivities.HasChanges() || _dataSetMasterCategory.HasChanges()
                || ctrlAccounts1.isUnSavedChanges || ctrlActivityJournalMapping1.isUnSavedChanges || ctrlActivityType1._isUnSavedChanges || ctrlSubAccountType._isUnsavedChanges)
            {
                DialogResult dlgResult = DialogResult.Yes;
                dlgResult = MessageBox.Show("Do you want to save the changes?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dlgResult.Equals(DialogResult.Yes))
                {
                    try
                    {
                        btnSave_Click(null, null);
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
                else if (dlgResult.Equals(DialogResult.No))
                {
                    _dsActivities.RejectChanges();
                    ctrlActivityJournalMapping1.SetDataSources();

                    _dataSetMasterCategory.RejectChanges();
                    ctrlAccounts1.SetDataSet(_dataSetMasterCategory);
                }
                else if (dlgResult.Equals(DialogResult.Cancel))
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tabCtrlActivities.SelectedTab.Key.Equals(CashManagementConstants.TAB_ACTIVITYJOURNALMAPPING))
                {
                    ctrlActivityJournalMapping1.DeleteJournalMapping();
                }
                else if (this.tabCtrlActivities.SelectedTab.Key.Equals(CashManagementConstants.TAB_ACTIVITYTYPES))
                {
                    ctrlActivityType1.DeleteActivityType();
                }
                else if (this.tabCtrlActivities.SelectedTab.Key.Equals(CashManagementConstants.TAB_SUBACCOUNTTYPE))
                {
                    ctrlSubAccountType.DeleteSubAccountType();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tabCtrlActivities.SelectedTab.Key.Equals(CashManagementConstants.TAB_ACTIVITYJOURNALMAPPING))
                {
                    ctrlActivityJournalMapping1.AddNewJournalMapping();
                }

                else if (this.tabCtrlActivities.SelectedTab.Key.Equals(CashManagementConstants.TAB_ACTIVITYTYPES))
                {
                    ctrlActivityType1.AddNewActivityType();
                }
                else if (this.tabCtrlActivities.SelectedTab.Key.Equals(CashManagementConstants.TAB_SUBACCOUNTTYPE))
                {
                    ctrlSubAccountType.AddNewSubAccountMapping();
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


        void btnRestoreDefault_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tabCtrlActivities.SelectedTab.Key.Equals(CashManagementConstants.TAB_ACTIVITYJOURNALMAPPING))
                {

                    StringBuilder errorMessage = new StringBuilder();
                    foreach (KeyValuePair<string, int> kvp in CachedDataManager.GetActivityType())
                    {
                        if (CashAccountDataManager.IfJournalMappingIsInUse(kvp.Value))
                            errorMessage.Append("Cash Transaction Type " + kvp.Key + " is in use \n");
                    }
                    if (errorMessage.Length == 0)
                    {
                        CashAccountDataManager.RestoreDefaultActivityJournalMapping();
                        CachedDataManager.GetInstance.RefreshAccountData();
                    }
                    else
                    {
                        MessageBox.Show(errorMessage.ToString(), "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //MessageBox.Show(errorMessage.ToString(), "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
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

        private void tabCtrlActivities_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                _dsActivities = ctrlActivityJournalMapping1.GetDataSet();
                _dataSetMasterCategory = ctrlAccounts1.GetDataSet();

                if (_dsActivities.HasChanges() || _dataSetMasterCategory.HasChanges()
                    || ctrlAccounts1.isUnSavedChanges || ctrlActivityJournalMapping1.isUnSavedChanges || ctrlActivityType1._isUnSavedChanges || ctrlSubAccountType._isUnsavedChanges)
                {
                    DialogResult dlgResult = DialogResult.Yes;
                    dlgResult = MessageBox.Show("Do you want to save the changes?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlgResult.Equals(DialogResult.Yes))
                    {
                        try
                        {
                            btnSave_Click(null, null);
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
                    else if (dlgResult.Equals(DialogResult.No))
                    {
                        _dsActivities.RejectChanges();
                        ctrlActivityJournalMapping1.SetDataSources();

                        _dataSetMasterCategory.RejectChanges();
                        ctrlAccounts1.SetDataSet(_dataSetMasterCategory);
                        ctrlAccounts1.isUnSavedChanges = false;
                        ctrlActivityJournalMapping1.isUnSavedChanges = false;
                        ctrlActivityType1._isUnSavedChanges = false;
                        ctrlSubAccountType._isUnsavedChanges = false;
                    }
                }

                if (e.Tab.Key.Equals(CashManagementConstants.TAB_CASHACCOUNTS))
                {
                    btnAdd.Visible = false;
                    btnDelete.Visible = false;
                    btnRestoreDefault.Visible = false;
                    toolStripStatusLabel1.Text = String.Empty;
                    toolStripStatusLabel1.Visible = true;
                    ctrlAccounts1.SelectDefaultTreeNode();
                    this.DisablePanel(true, CashManagementAccountSetup.CashAccounts);
                }
                else if (e.Tab.Key.Equals(CashManagementConstants.TAB_ACTIVITYTYPES))
                {
                    btnAdd.Visible = true;
                    btnDelete.Visible = true;
                    btnRestoreDefault.Visible = false;
                    toolStripStatusLabel1.Visible = false;
                    this.DisablePanel(true, CashManagementAccountSetup.ActivityTypes);
                }
                else if (e.Tab.Key.Equals(CashManagementConstants.TAB_ACTIVITYJOURNALMAPPING))
                {
                    btnAdd.Visible = true;
                    btnDelete.Visible = true;
                    btnRestoreDefault.Visible = true;
                    toolStripStatusLabel1.Visible = false;
                    ctrlActivityJournalMapping1.SelectDefaultTreeNode();
                    this.DisablePanel(IsBottomUIEnabled, CashManagementAccountSetup.ActivityJournalMapping);
                }
                else if (e.Tab.Key.Equals(CashManagementConstants.TAB_SUBACCOUNTTYPE))
                {
                    btnAdd.Visible = true;
                    btnDelete.Visible = true;
                    toolStripStatusLabel1.Visible = false;
                    btnRestoreDefault.Visible = false;
                    this.DisablePanel(true, CashManagementAccountSetup.SubAccountType);
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



        //private void SetGridColumnsLayout(UltraGridBand band)
        //{
        //    try
        //    {
        //        foreach (UltraGridColumn column in band.Columns)
        //        {
        //            column.Width = 100;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        //private void SetListViewActivityTypes()
        //{
        //    try
        //    {
        //        #region tab1
        //        listViewActivities.ViewSettingsDetails.ImageSize = Size.Empty;

        //private void listViewCashTransactionType_ItemActivated(object sender, ItemActivatedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Item != null)
        //        {
        //            DataRow[] result = _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYTRANSACTIONMAPPING].Select("CashTransactionTypeId = '" + int.Parse(e.Item.Key) + "'");
        //            if (result.Length > 0)
        //                BindCashTransactionGrid(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}


        //private void grdTransactionData_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{

        //}

        private bool ExportToExcel()
        {
            bool result = false;
            try
            {
                Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                string pathName = null;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Application.StartupPath;
                saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    pathName = saveFileDialog1.FileName;
                }
                else
                {
                    return result;
                }

                if (this.tabCtrlActivities.SelectedTab.Key.Equals(CashManagementConstants.TAB_CASHACCOUNTS))
                {
                    workBook = ctrlAccounts1.GetGridDataToExportToExcel();
                }
                else if (this.tabCtrlActivities.SelectedTab.Key.Equals(CashManagementConstants.TAB_ACTIVITYTYPES))
                {
                    workBook = ctrlActivityType1.GetGridDataToExportToExcel();
                }
                else if (this.tabCtrlActivities.SelectedTab.Key.Equals(CashManagementConstants.TAB_ACTIVITYJOURNALMAPPING))
                {
                    workBook = ctrlActivityJournalMapping1.GetGridDataToExportToExcel();
                }
                else if (this.tabCtrlActivities.SelectedTab.Key.Equals(CashManagementConstants.TAB_SUBACCOUNTTYPE))
                {
                    workBook = ctrlSubAccountType.GetGridDataToExportToExcel();
                }
                if (workBook != null && workBook.Worksheets.Count > 0)
                    workBook.Save(pathName);

                result = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
            return result;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ExportToExcel())
                {
                    MessageBox.Show("Report Successfully saved.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
        //void CashAccountsUI_Resize(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        if (this.WindowState == FormWindowState.Maximized)
        //        {
        //            this.Height = Screen.PrimaryScreen.Bounds.Height;
        //            this.Width = Screen.PrimaryScreen.Bounds.Width;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void SubscribeForCashTransactions()
        {
            try
            {
                if (_proxy != null)
                {
                    _proxy.UnSubscribe();
                    _proxy.Subscribe(Topics.Topic_SubAccounts, null);
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

        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker del =
                            delegate
                            {
                                Publish(e, topicName);
                            };
                        this.BeginInvoke(del);
                        return;
                    }
                    object[] user = (object[])e.EventData;
                    if (user[0] != null && !string.IsNullOrWhiteSpace(user[0].ToString()))
                    {
                        int userID = Convert.ToInt32(user[0]);
                        if (userID != CachedDataManager.GetInstance.LoggedInUser.CompanyUserID)
                            toolStripStatusLabel1.Text = "Account Setup information has been modified. Click the Refresh button to see the changes.";
                    }
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

        public string getReceiverUniqueName()
        {
            return "CtrlSubAccountSetup";
        }

        /// <summary>
        /// Reload the updated details from the DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ReloadAccountSetupDetails();
                toolStripStatusLabel1.Text = "Updated details have been loaded";
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
        /// Get the updated details from the DB
        /// </summary>
        private void ReloadAccountSetupDetails()
        {
            try
            {
                CachedDataManager.GetInstance.RefreshAccountData();
                ctrlAccounts1.InitializeAccountsControl();
                ctrlActivityType1.InitializeDataSets();
                ctrlSubAccountType.InitializeDataSets();
                ctrlActivityJournalMapping1.InitializeDataSets();
                ctrlActivityJournalMapping1.EmptyFilterTextBox();
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

        public void DisablePanel(bool isEnabled, CashManagementAccountSetup tabName)
        {
            if (tabName == CashManagementAccountSetup.ActivityJournalMapping)
                IsBottomUIEnabled = isEnabled;
            ultraPanel2.Enabled = isEnabled;
        }

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}