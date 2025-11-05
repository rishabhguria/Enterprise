using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.ReconciliationNew;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
namespace Prana.Tools
{
    public partial class ctrlPBAccountMapping : UserControl
    {
        //bool changesMade=false;
        Dictionary<int, List<int>> _dictDataSourceSubAccountAssociation = new Dictionary<int, List<int>>();
        Dictionary<int, string> _dictAccounts = new Dictionary<int, string>();
        //List<GenericNameID> _reconDataSourcecol = new List<GenericNameID>();
        Dictionary<int, string> _dictThirdParties = new Dictionary<int, string>();
        int _noOFAccounts = 0;
        ReconTemplate _recontemplate = null;
        /// <summary>
        /// reverse of _pbidData
        /// will raise exception in case Pb name is not unique
        /// </summary>
        Dictionary<string, int> _dictRevPB_IDData;
        Dictionary<string, int> _dictrevAccounts;
        private bool _isUnsavedChanges = false;

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlPBAccountMapping()
        {
            try
            {
                InitializeComponent();
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

        //return true if mouseclick event occurs on the user control

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <returns></returns>
        public bool IsUnsavedChanges()
        {
            try
            {
                if (_isUnsavedChanges)
                {
                    _isUnsavedChanges = false;
                    return true;
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
            return false;
        }
        /// <summary>
        /// Initializes grid layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdPBAccountMapping_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                // Set the HeaderCheckBoxVisibility so it will display the CheckBox whenever a CheckEditor is used within the UltraGridColumn 
                //e.Layout.Override.HeaderCheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                // Set the HeaderCheckBoxAlignment so the CheckBox will appear to the Right of the caption. 
                // e.Layout.Override.HeaderCheckBoxAlignment = HeaderCheckBoxAlignment.Right;
                // Set the HeaderChe.LayouteckBoxSynchronization so all rows within the GridBand will be synchronized with the CheckBox 
                // e.Layout.Override.HeaderCheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;

                e.Layout.Bands[0].Columns[ReconConstants.COLUMN_ThirdParty].CellActivation = Activation.NoEdit;
                //Count no of rows
                int noRows = grdPBAccountMapping.Rows.Count;
                //setHeight of grid
                if (noRows > 5)
                {
                    grdPBAccountMapping.Height = (5 * 20) + grdPBAccountMapping.DisplayLayout.Bands[0].Header.Height;
                }
                else
                {
                    grdPBAccountMapping.Height = (noRows * 20) + grdPBAccountMapping.DisplayLayout.Bands[0].Header.Height;
                }
                //set column to autosize
                grdPBAccountMapping.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;

                UltraGridBand prBand = e.Layout.Bands[0];
                if (!prBand.Columns.Exists(ReconConstants.COLUMN_Checkbox))
                {
                    UltraGridColumn colDelete = prBand.Columns.Add(ReconConstants.COLUMN_Checkbox);
                    colDelete.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    colDelete.Width = 20;
                    colDelete.Header.Caption = string.Empty;
                    colDelete.Header.VisiblePosition = 0;
                    colDelete.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                    colDelete.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                }
                if (!prBand.Columns.Exists(ReconConstants.COLUMN_Accounts))
                {
                    grdPBAccountMapping.DisplayLayout.Bands[0].Columns.Add(ReconConstants.COLUMN_Accounts);
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
        /// <summary>
        /// Initializes data on grid
        /// </summary>
        private void InitializeDataOnGrid()
        {
            try
            {
                // List of Accounts of selected client
                List<int> lstClientwiseAccounts = new List<int>();
                //lstClientwiseAccounts = CachedDataManagerRecon.GetInstance.GetAllCompanyAccounts()[clientID];
                Dictionary<string, RunUpload> runUploadValueList = ReconPreferences.DictRunUpload;
                String formatName = _recontemplate.FormatName;
                if (runUploadValueList != null && runUploadValueList.Keys != null && runUploadValueList.Keys.Contains(formatName))
                {
                    lstClientwiseAccounts.AddRange(runUploadValueList[formatName].LstAccountID);
                }
                _dictRevPB_IDData = new Dictionary<string, int>();

                // dictionary of accountIDs and accountNames
                foreach (int accountID in lstClientwiseAccounts)
                {
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3739
                    if (!_dictAccounts.ContainsKey(accountID) && CachedDataManagerRecon.GetInstance.GetAllPermittedAccounts().ContainsKey(accountID))
                    {
                        _dictAccounts.Add(accountID, CachedDataManagerRecon.GetInstance.GetAllPermittedAccounts()[accountID]);

                    }
                }
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1479
                _dictThirdParties.Clear();
                _dictDataSourceSubAccountAssociation.Clear();
                foreach (KeyValuePair<int, List<int>> thirdparty in CachedDataManagerRecon.GetInstance.GetAllCompanyThirdPartyAccounts())
                {
                    if (CachedDataManager.GetInstance.GetAllThirdParties().ContainsKey(thirdparty.Key))
                    {
                        foreach (int accountID in thirdparty.Value)
                        {
                            if (lstClientwiseAccounts.Contains(accountID))
                            {
                                // dictionary of thirdparties ID and Names of selected client
                                if (!_dictThirdParties.ContainsKey(thirdparty.Key))
                                {
                                    _dictThirdParties.Add(thirdparty.Key, CachedDataManager.GetInstance.GetAllThirdParties()[thirdparty.Key]);
                                }
                                // dictionary containing thirdpartyID and AccountsID of selected client
                                if (!_dictDataSourceSubAccountAssociation.ContainsKey(thirdparty.Key))
                                {
                                    List<int> lstAccountIds = new List<int>();
                                    lstAccountIds.Add(accountID);
                                    _dictDataSourceSubAccountAssociation.Add(thirdparty.Key, lstAccountIds);
                                }
                                else if (!_dictDataSourceSubAccountAssociation[thirdparty.Key].Contains(accountID))
                                {
                                    _dictDataSourceSubAccountAssociation[thirdparty.Key].Add(accountID);
                                }
                            }
                        }
                    }
                }

                _dictrevAccounts = new Dictionary<string, int>();
                foreach (KeyValuePair<int, string> kvp in _dictThirdParties)
                {
                    if (!_dictRevPB_IDData.ContainsKey(kvp.Value))
                    {
                        _dictRevPB_IDData.Add(kvp.Value, kvp.Key);
                    }
                }

                foreach (KeyValuePair<int, string> kvp in _dictAccounts)
                {
                    if (!_dictrevAccounts.ContainsKey(kvp.Value))
                    {
                        _dictrevAccounts.Add(kvp.Value, kvp.Key);
                    }
                }
                DataTable gridTable = new DataTable();
                gridTable.Columns.Add(ReconConstants.COLUMN_ThirdParty, typeof(string));
                foreach (KeyValuePair<string, int> tpName in _dictRevPB_IDData)
                {
                    if (tpName.Key != ReconConstants.SelectDefaultValue)
                    {
                        gridTable.Rows.Add(tpName.Key);
                    }
                }
                grdPBAccountMapping.DataSource = gridTable;
                foreach (UltraGridRow row in grdPBAccountMapping.Rows)
                {
                    bool b = false;

                    //1.if dictionary is empty
                    //2.if PrimeBrokers is present in dictionary
                    // Modified by Ankit gupta on 31 Oct, 2014
                    // When all accounts and Prime brokers were selected, null string was being sent.
                    //if (_recontemplate.ReconFilters.DictPrimeBrokers.Count == 0 || _recontemplate.ReconFilters.DictPrimeBrokers.ContainsValue(row.Cells[ReconConstants.COLUMN_ThirdParty].Text))
                    if (_recontemplate.ReconFilters.DictPrimeBrokers.ContainsValue(row.Cells[ReconConstants.COLUMN_ThirdParty].Text))
                    {
                        b = true;
                    }
                    row.Cells[ReconConstants.COLUMN_Checkbox].Value = b;
                    row.Cells[ReconConstants.COLUMN_Accounts].EditorComponent = GetUltraCombo(row.Cells[ReconConstants.COLUMN_ThirdParty].Text, false);
                }
                SetAccountSelection();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// cell event for CheckBox header connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grdPBAccountMapping_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                //to check if changes are made on the user control
                _isUnsavedChanges = true;
                if (e.Cell.Column.Key == ReconConstants.COLUMN_Checkbox)
                {
                    grdPBAccountMapping.PerformAction(UltraGridAction.ExitEditMode);
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


        private void grdPBAccountMapping_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == ReconConstants.COLUMN_Checkbox)
                {
                    UltraCombo accountUltraCombo = new UltraCombo();
                    //get the value of grid row checkbox
                    bool value = e.Cell.Text == "True";

                    //get the ultracombo
                    accountUltraCombo = (UltraCombo)e.Cell.Row.Cells[ReconConstants.COLUMN_Accounts].EditorComponent;
                    if (accountUltraCombo == null)
                    {
                        return;
                    }

                    //for each row in ultracombo 
                    foreach (UltraGridRow r in accountUltraCombo.Rows)
                    {
                        r.Cells[accountUltraCombo.CheckedListSettings.CheckStateMember].Value = value;
                    }
                    //set textbox value of combobox
                    e.Cell.Row.Cells[ReconConstants.COLUMN_Accounts].Value = accountUltraCombo.CheckedRows.ToString();
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

        /// <summary>
        /// returns ultracombo to bind on every cell of column "Accounts"
        /// </summary>
        /// <param name="pbName"></param>
        /// <param name="checkState"></param>
        /// <returns></returns>
        public UltraCombo GetUltraCombo(string pbName, bool checkState)
        {
            UltraCombo tempUltraCombo = new UltraCombo();
            try
            {
                DataTable comboTable = new DataTable();
                comboTable.Columns.Add(ReconConstants.CAPTION_MasterFund, typeof(string));
                comboTable.Columns.Add(ReconConstants.COLUMN_Selected, typeof(bool));
                if (_dictDataSourceSubAccountAssociation.ContainsKey(_dictRevPB_IDData[pbName]))
                {
                    foreach (int item in _dictDataSourceSubAccountAssociation[_dictRevPB_IDData[pbName]])
                    {
                        _noOFAccounts++;
                        comboTable.Rows.Add(_dictAccounts[item], checkState);
                    }
                }
                tempUltraCombo.DataSource = comboTable;
                tempUltraCombo.CheckedListSettings.CheckStateMember = ReconConstants.COLUMN_Selected;
                tempUltraCombo.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                tempUltraCombo.CheckedListSettings.ListSeparator = Seperators.SEPERATOR_8;
                tempUltraCombo.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                tempUltraCombo.DisplayLayout.Bands[0].Columns[ReconConstants.COLUMN_Selected].Header.VisiblePosition = 0;
                // tempUltraCombo.DropDownStyle = UltraComboStyle.DropDownList;

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return tempUltraCombo;
        }
        private void SetAccountSelection()
        {
            try
            {
                foreach (UltraGridRow row in grdPBAccountMapping.Rows)
                {
                    UltraGridCell cell = row.Cells[ReconConstants.COLUMN_Accounts];
                    UltraCombo ultraComboAccount = (UltraCombo)cell.EditorComponent;
                    foreach (UltraGridRow comboRow in ultraComboAccount.Rows)
                    {
                        //1.if dictionary is empty
                        //2.if account is present in dictionary
                        // if (_recontemplate.ReconFilters.DictAccounts.Count == 0 || _recontemplate.ReconFilters.DictAccounts.ContainsValue(comboRow.Cells[ReconConstants.CAPTION_MasterFund].Text))
                        if (_recontemplate.ReconFilters.DictAccounts.ContainsValue(comboRow.Cells[ReconConstants.CAPTION_MasterFund].Text))
                        {
                            comboRow.Cells[ultraComboAccount.CheckedListSettings.CheckStateMember].Value = true;
                        }
                    }
                    //set textbox value of combobox                   
                    cell.Row.Cells[ReconConstants.COLUMN_Accounts].Value = ultraComboAccount.CheckedRows.ToString();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void setTemplate(ReconTemplate reconTemplate)
        {
            try
            {
                grdPBAccountMapping.DisplayLayout.GroupByBox.Hidden = true;
                _recontemplate = reconTemplate;
                InitializeDataOnGrid();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void UpdateCheckedPB()
        {
            try
            {
                if (_recontemplate != null)
                {
                    _recontemplate.ReconFilters.DictPrimeBrokers.Clear();
                    foreach (UltraGridRow row in grdPBAccountMapping.Rows)
                    {
                        string value = row.Cells[ReconConstants.COLUMN_ThirdParty].Value.ToString();
                        if (row.Cells[ReconConstants.COLUMN_Checkbox].Value.ToString() == "True")
                        {
                            _recontemplate.ReconFilters.DictPrimeBrokers.Add(_dictRevPB_IDData[value], value);
                        }
                    }
                    if (_recontemplate.ReconFilters.DictPrimeBrokers.Count == grdPBAccountMapping.Rows.Count)
                    {
                        //   _recontemplate.ReconFilters.DictPrimeBrokers.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void UpdateCheckedAccounts()
        {
            try
            {
                if (_recontemplate != null)
                {
                    _recontemplate.ReconFilters.DictAccounts.Clear();
                    foreach (UltraGridRow row in grdPBAccountMapping.Rows)
                    {
                        UltraGridCell cell = row.Cells[ReconConstants.COLUMN_Accounts];
                        UltraCombo ultraComboAccount = (UltraCombo)cell.EditorComponent;
                        foreach (UltraGridRow comboRow in ultraComboAccount.Rows)
                        {
                            string checkBoxValue = comboRow.Cells[ultraComboAccount.CheckedListSettings.CheckStateMember].Value.ToString();
                            string accountName = comboRow.Cells[ReconConstants.CAPTION_MasterFund].Value.ToString();
                            if (checkBoxValue == "True")
                            {
                                if (!_recontemplate.ReconFilters.DictAccounts.ContainsKey(_dictrevAccounts[accountName]))
                                    _recontemplate.ReconFilters.DictAccounts.Add(_dictrevAccounts[accountName], accountName);
                            }
                        }
                    }
                    // AccountId is passed even if all Accounts are selected.
                    //if (_recontemplate.ReconFilters.DictAccounts.Count == _dictrevAccounts.Count)
                    //{
                    //_recontemplate.ReconFilters.DictAccounts.Clear();
                    //}

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Event associated with MultiSelectEditor, which is invoked after dropdown has been closed.
        /// Added by Ankit Gupta on 13 Oct, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1565
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiSelectEditor_AfterEditorButtonCloseUp(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                UpdateCheckedAccounts();
                if (grdPBAccountMapping.Rows.Count > 0 && _recontemplate.ReconFilters.DictAccounts.Count == 0)
                {
                    MessageBox.Show("Please select at least one account to run reconciliation.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
    }
}

