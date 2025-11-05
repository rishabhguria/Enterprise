using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTree;
using Prana.BusinessObjects.AppConstants;
using Prana.CashManagement.Classes;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prana.CashManagement
{
    public partial class ctrlActivityJournalMapping : UserControl
    {
        DataSet _dsActivities = new DataSet();
        ValueList _vlActivityType = new ValueList();
        ValueList _vlCashActivityType = new ValueList();
        ValueList _vlAmountType = new ValueList();
        ValueList _vlSubAccount = new ValueList();
        //ValueList _vlCashTransactionType = new ValueList();
        ValueList _vlCashValueType = new ValueList();
        ValueList _vlCashTransactionDateType = new ValueList();
        public bool isUnSavedChanges = false;
        Dictionary<int, CashJournalMappingItem> _dictMapping = new Dictionary<int, CashJournalMappingItem>();

        public ctrlActivityJournalMapping()
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

        internal void InitializeDataSets()
        {
            try
            {
                grdJournalData.DataSource = null;
                _dsActivities = CachedDataManager.GetInstance.GetAllActivityTables();
                _vlActivityType = ValueListHelper.GetInstance.GetActivityTypeValueList().Clone();
                _vlAmountType = ValueListHelper.GetInstance.GetAmountTypeValueList().Clone();
                _vlCashActivityType = ValueListHelper.GetInstance.GetCashActivityTypeValueList().Clone();
                //_vlCashTransactionType = CachedDataManager.GetInstance.GetCashTransactionTypeValueList();
                _vlSubAccount = ValueListHelper.GetInstance.GetSubAccountTypeValueList().Clone();
                _vlCashValueType = ValueListHelper.GetInstance.GetCashValueTypeValueList().Clone();
                _vlCashTransactionDateType = ValueListHelper.GetInstance.GetCashTransactionDateTypeValueList().Clone();

                //this.ultraTreeActivityType.SetDataBinding(_dsActivities, CashManagementConstants.TABLE_ACTIVITYTYPE);
                SetActivityTypeTree(_dsActivities);
                this.ultraTreeActivityType.SynchronizeCurrencyManager = true;
                this.ultraTreeActivityType.Override.ActiveNodeAppearance.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(190)), ((System.Byte)(226)), ((System.Byte)(255)));
                this.ultraTreeActivityType.Override.ActiveNodeAppearance.ForeColor = Color.Black;
                this.ultraTreeActivityType.Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.Never;
                this.ultraTreeActivityType.Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnDisplay;
                this.ultraTreeActivityType.ViewStyle = Infragistics.Win.UltraWinTree.ViewStyle.Standard;
                //this.ultraTreeActivityType.SelectedNodes.AddRange(ultraTreeActivityType.Nodes[0]);
                //this.ultraTreeActivityType.ActiveNode = ultraTreeActivityType.Nodes[0];
                if (ultraTreeActivityType.Nodes.Count > 0)
                    ultraTreeActivityType.Nodes[0].Selected = true;
                grdJournalData.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
                string filterName = txtFilterActivity.Text.Trim();
                txtFilterActivity.Text = "";
                txtFilterActivity.Text = filterName;
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

        private void SetActivityTypeTree(DataSet _dsActivities)
        {
            try
            {
                if (ultraTreeActivityType != null && ultraTreeActivityType.Nodes.Count > 0)
                {
                    ultraTreeActivityType.Nodes.Clear();
                }
                foreach (DataRow dr in _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYTYPE].Rows)
                {
                    if (ultraTreeActivityType.Nodes.Exists("Group_" + dr[CashManagementConstants.COLUMN_ACTIVITYSOURCE].ToString()))
                    {
                        UltraTreeNode node = new UltraTreeNode(dr[CashManagementConstants.COLUMN_ACTIVITYTYPEID].ToString(), dr[CashManagementConstants.COLUMN_ACTIVITYTYPE].ToString());
                        if (!ultraTreeActivityType.Nodes["Group_" + dr[CashManagementConstants.COLUMN_ACTIVITYSOURCE].ToString()].Nodes.Exists(dr[CashManagementConstants.COLUMN_ACTIVITYTYPEID].ToString()))
                            ultraTreeActivityType.Nodes["Group_" + dr[CashManagementConstants.COLUMN_ACTIVITYSOURCE].ToString()].Nodes.Add(node);
                    }
                    else
                    {
                        ActivitySource actSource = (ActivitySource)Convert.ToInt32(dr[CashManagementConstants.COLUMN_ACTIVITYSOURCE].ToString());
                        UltraTreeNode parentNode = new UltraTreeNode("Group_" + dr[CashManagementConstants.COLUMN_ACTIVITYSOURCE].ToString(), actSource.ToString());
                        UltraTreeNode childNode = new UltraTreeNode(dr[CashManagementConstants.COLUMN_ACTIVITYTYPEID].ToString(), dr[CashManagementConstants.COLUMN_ACTIVITYTYPE].ToString());
                        ultraTreeActivityType.Nodes.Add(parentNode);
                        ultraTreeActivityType.Nodes["Group_" + dr[CashManagementConstants.COLUMN_ACTIVITYSOURCE].ToString()].Nodes.Add(childNode);
                    }
                }
                ultraTreeActivityType.Override.Sort = SortType.Ascending;
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
        /// added by: Bharat Raturi, 13 Oct 2014
        /// Create the dictionary of mapping so that it can be used later to prevent redundancy 
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-5074
        /// </summary>
        public void CreateActivityJournalMappingDictionary()
        {
            try
            {
                if (_dsActivities.Tables.Contains(CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING) && _dictMapping != null
                    && _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].Columns.Contains("ActivityTypeId_FK")
                    && _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].Columns.Contains("DebitAccount")
                    && _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].Columns.Contains("CreditAccount"))
                {
                    _dictMapping.Clear();
                    foreach (DataRow dr in _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].Rows)
                    {
                        //consider only non-deleted rows
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-5315
                        if (dr.RowState == DataRowState.Deleted)
                            continue;
                        int mappingID = 0;
                        int creditAccount = 0;
                        int debitAccount = 0;
                        if (dr["ActivityTypeId_FK"] != DBNull.Value)
                            mappingID = Convert.ToInt32(dr["ActivityTypeId_FK"]);
                        if (dr["DebitAccount"] != DBNull.Value)
                            creditAccount = Convert.ToInt32(dr["DebitAccount"]);
                        if (dr["CreditAccount"] != DBNull.Value)
                            debitAccount = Convert.ToInt32(dr["CreditAccount"]);

                        CashJournalMappingItem objMap = new CashJournalMappingItem();

                        objMap.MappingID = mappingID;
                        if (!_dictMapping.ContainsKey(mappingID))
                        {
                            List<int> lstDebit = new List<int>();
                            List<int> lstCredit = new List<int>();
                            if (creditAccount != 0)
                                lstCredit.Add(creditAccount);
                            if (debitAccount != 0)
                                lstDebit.Add(debitAccount);
                            objMap.CreditAccount = lstCredit;
                            objMap.DebitAccount = lstDebit;
                            //objMap.ActivityTypeID = activityTypeId;
                            _dictMapping.Add(mappingID, objMap);
                        }
                        else
                        {
                            if (!_dictMapping[mappingID].CreditAccount.Contains(creditAccount) && creditAccount != 0)
                                _dictMapping[mappingID].CreditAccount.Add(creditAccount);
                            if (!_dictMapping[mappingID].DebitAccount.Contains(debitAccount) && debitAccount != 0)
                                _dictMapping[mappingID].DebitAccount.Add(debitAccount);
                        }
                    }

                    //sort the debit and credit accounts inside the dictionary
                    foreach (int mapID in _dictMapping.Keys)
                    {
                        _dictMapping[mapID].CreditAccount.Sort();
                        _dictMapping[mapID].DebitAccount.Sort();
                    }
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

        private void grdJournalData_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                //e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
                grdJournalData.DisplayLayout.Override.RowAppearance.BackColor = Color.Black;
                grdJournalData.DisplayLayout.Override.RowAppearance.ForeColor = Color.White;
                //grdJournalData.DisplayLayout.Override.RowAppearance.BackColor2 = Color.Green;
                grdJournalData.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Gray;
                grdJournalData.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.Black;
                //grdJournalData.DisplayLayout.Appearance.BackColor = Color.Gainsboro;
                //grdJournalData.DisplayLayout.Appearance.BackColor2 = Color.Gainsboro;
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

        private void grdJournalData_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
                switch (e.Cell.Column.Key)
                {
                    case CashManagementConstants.COLUMN_AMOUNTTYPEID_FK:
                        //if(listViewActivityJournalMapping.SelectedItems.Count>0)
                        //{
                        //    DataRow[] rowsHavingActivityTypeAndAmountType = _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].Select(CashManagementConstants.COLUMN_AMOUNTTYPEID_FK+ " = " +e.Cell.Value + " AND " +CashManagementConstants.COLUMN_ACTIVITYTYPEID_FK+ " = " +ultraTreeActivityType.SelectedNodes[0].DataKey);
                        //    if(rowsHavingActivityTypeAndAmountType.Length>0)
                        //    {
                        //        grdJournalData.AfterCellUpdate -=new CellEventHandler(grdJournalData_AfterCellUpdate);
                        //        e.Cell.Value = e.Cell.OriginalValue;
                        //        grdJournalData.AfterCellUpdate += new CellEventHandler(grdJournalData_AfterCellUpdate);
                        //        MessageBox.Show("Cannot Add multiple journal entries for same Amount Type", "Journal Mapping Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //        grdJournalData.ActiveCell = e.Cell;
                        //    }
                        //}
                        break;
                    case CashManagementConstants.COLUMN_DEBITACCOUNT:
                        if (string.IsNullOrEmpty(e.Cell.Text))
                        {
                            row[CashManagementConstants.COLUMN_DEBITACCOUNT] = int.MinValue;
                        }
                        break;
                    case CashManagementConstants.COLUMN_CREDITACCOUNT:
                        if (string.IsNullOrEmpty(e.Cell.Text))
                        {
                            row[CashManagementConstants.COLUMN_CREDITACCOUNT] = int.MinValue;
                        }
                        break;
                    default:
                        break;
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
        /// Narendra Kumar jangir
        /// Date: Apr 2013 25
        /// Desc: Bind Journal Data to the Grid On the basis of JournalCode
        /// </summary>
        /// 
        private void BindDataToJournalGrid(DataRow row)
        {
            try
            {
                //check that modified datatable is matched
                if (row.Table.TableName == CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING)
                {
                    grdJournalData.DataSource = row.Table.DataSet.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING];
                    //list of columns which are visible
                    // Description column should also be displayed PRANA-32636
                    List<string> lsColumnsToDisplay = new List<string>(new string[] { CashManagementConstants.COLUMN_ACTIVITYDATETYPE, CashManagementConstants.COLUMN_AMOUNTTYPEID_FK, CashManagementConstants.COLUMN_DEBITACCOUNT, CashManagementConstants.COLUMN_CREDITACCOUNT, CashManagementConstants.COLUMN_CASHVALUETYPE, CashManagementConstants.CAPTION_DESCRIPTION });
                    UltraWinGridUtils.HideColumns(grdJournalData.DisplayLayout.Bands[0]);
                    UltraWinGridUtils.SetBand(lsColumnsToDisplay, grdJournalData.DisplayLayout.Bands[0]);
                    //define caption, column width and value list for the column
                    //value lists are so that user can select value from drop down
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYDATETYPE].ValueList = _vlCashTransactionDateType;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYDATETYPE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYDATETYPE].Width = 140;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYDATETYPE].Header.Caption = CashManagementConstants.CAPTION_ACTIVITYDATETYPE;

                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_AMOUNTTYPEID_FK].ValueList = _vlAmountType;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_AMOUNTTYPEID_FK].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_AMOUNTTYPEID_FK].Header.Caption = CashManagementConstants.CAPTION_AMOUNTTYPE;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_AMOUNTTYPEID_FK].Width = 120;

                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_DEBITACCOUNT].ValueList = _vlSubAccount;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_DEBITACCOUNT].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_DEBITACCOUNT].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_DEBITACCOUNT].AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_DEBITACCOUNT].Width = 180;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_DEBITACCOUNT].Header.Caption = CashManagementConstants.CAPTION_DEBITACCOUNT;

                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_CREDITACCOUNT].ValueList = _vlSubAccount;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_CREDITACCOUNT].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_CREDITACCOUNT].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_CREDITACCOUNT].AutoSuggestFilterMode = AutoSuggestFilterMode.Contains;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_CREDITACCOUNT].Width = 180;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_CREDITACCOUNT].Header.Caption = CashManagementConstants.CAPTION_CREDITACCOUNT;

                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_CASHVALUETYPE].ValueList = _vlCashValueType;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_CASHVALUETYPE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_CASHVALUETYPE].Width = 120;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_CASHVALUETYPE].Header.Caption = CashManagementConstants.CAPTION_CASHVALUETYPE;

                    // Styling of Description column PRANA-32636
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_DESCRIPTION].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_DESCRIPTION].Width = 140;
                    grdJournalData.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_DESCRIPTION].Header.Caption = CashManagementConstants.COLUMN_DESCRIPTION;

                    foreach (UltraGridRow gr in grdJournalData.Rows)
                    {

                        DataRow gridRow = ((System.Data.DataRowView)(gr.ListObject)).Row;
                        if (gridRow != row)
                        {
                            if (!gridRow[CashManagementConstants.COLUMN_ACTIVITYTYPEID_FK].ToString().Equals(row[CashManagementConstants.COLUMN_ACTIVITYTYPEID_FK].ToString()))
                            {
                                gr.Hidden = true;
                            }
                        }
                        else
                        {
                            // gr.Band.Columns["Acronym"].CellActivation = Activation.AllowEdit;
                            gr.Hidden = false;
                            //ValidateRow(gr.Cells[1]);
                            //ValidateRow(gr.Cells[4]);
                        }
                    }
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
        //private void GenerateCashJournalEntries(string NewJournalCode)
        //{
        //    try
        //    {
        //        DataRow[] rows = _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].Select(CashManagementConstants.COLUMN_ACTIVITYTYPE + " = 'eq.buy'");
        //        foreach (DataRow rowStandardEntry in rows)
        //        {
        //            DataRow rowJournalEntry = _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].NewRow();
        //            //rowJournalEntry[COLUMN_JOURNALCODE] = NewJournalCode;
        //            rowJournalEntry[CashManagementConstants.COLUMN_ACTIVITYTYPEID_FK] = int.MinValue;
        //            rowJournalEntry[CashManagementConstants.COLUMN_AMOUNTTYPEID_FK] = rowStandardEntry[CashManagementConstants.COLUMN_AMOUNTTYPEID_FK];
        //            rowJournalEntry[CashManagementConstants.COLUMN_CREDITACCOUNT] = rowStandardEntry[CashManagementConstants.COLUMN_CREDITACCOUNT];
        //            rowJournalEntry[CashManagementConstants.COLUMN_DEBITACCOUNT] = rowStandardEntry[CashManagementConstants.COLUMN_DEBITACCOUNT];
        //            _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].Rows.Add(rowJournalEntry);
        //            BindDataToJournalGrid(rowJournalEntry);
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

        private void grdJournalData_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                //apply text color setting
                //if (e.Row.Appearance.BackColor.Equals(Color.Black))
                //{
                //    e.Row.Appearance.ForeColor = Color.White;
                //}
                //else if(e.Row.Appearance.BackColor.Equals(Color.White))
                //    e.Row.Appearance.ForeColor = Color.Black;
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
        /// Add journal mapping for the activity type
        /// </summary>
        public void AddNewJournalMapping()
        {
            try
            {
                if (ultraTreeActivityType.SelectedNodes[0].HasNodes)
                {
                    MessageBox.Show("Please select a valid Activity Type", "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (ultraTreeActivityType.SelectedNodes[0].Text.Equals("UnknownActivity"))
                {
                    MessageBox.Show("Cannot add Mapping to this Activity Type", "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DataRow row = _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].NewRow();
                if (ultraTreeActivityType.SelectedNodes.Count > 0)
                {
                    //activity type id will set according to selected item from activity type listview.
                    row[CashManagementConstants.COLUMN_ACTIVITYTYPEID_FK] = ultraTreeActivityType.SelectedNodes[0].Key;
                    row[CashManagementConstants.COLUMN_AMOUNTTYPEID_FK] = int.MinValue;
                    row[CashManagementConstants.COLUMN_CREDITACCOUNT] = int.MinValue;
                    row[CashManagementConstants.COLUMN_DEBITACCOUNT] = int.MinValue;
                    _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].Rows.Add(row);
                    BindDataToJournalGrid(row);
                    grdJournalData.Rows[grdJournalData.Rows.Count - 1].Selected = true;
                    grdJournalData.Rows[grdJournalData.Rows.Count - 1].Activate();
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
        /// Delete selected journal mapping for the activity type
        /// </summary>
        public void DeleteJournalMapping()
        {
            try
            {
                if (ultraTreeActivityType.SelectedNodes[0].Text.Equals("UnknownActivity"))
                {
                    MessageBox.Show("Cannot delete Mapping to this Activity Type", "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (this.grdJournalData.ActiveRow != null && this.grdJournalData.ActiveRow.Index > 0 && ultraTreeActivityType.ActiveNode != null
                    && !string.IsNullOrWhiteSpace(ultraTreeActivityType.ActiveNode.Key.ToString()) && ultraTreeActivityType.ActiveNode.Key == (grdJournalData.ActiveRow.Cells[CashManagementConstants.COLUMN_ACTIVITYTYPEID_FK].Value.ToString()))
                {
                    if ((_dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].Rows[grdJournalData.ActiveRow.Index].RowState) == DataRowState.Added)
                    {
                        DeleteMappingRow();
                    }
                    else
                    {
                        int activityTypeId = Convert.ToInt32(grdJournalData.ActiveRow.Cells[CashManagementConstants.COLUMN_ACTIVITYTYPEID_FK].Value.ToString());
                        if (!CashAccountDataManager.IfJournalMappingIsInUse(activityTypeId))
                        {
                            //remove the row from the grid, changes will be reflected in the underlying data source/data table.
                            DeleteMappingRow();
                        }
                        else
                        {
                            MessageBox.Show("Activity Journal Mapping is in use.", "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //MessageBox.Show("Activity Journal Mapping is in use.", "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);                    
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("Please Select a mapping row to delete.", "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);                    
                    MessageBox.Show("Please Select a mapping row to delete.", "Prana", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void DeleteMappingRow()
        {
            if (MessageBox.Show("Do you want to delete the selected mapping?", "Delete Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                grdJournalData.Rows[this.grdJournalData.ActiveRow.Index].Delete(false);
        }

        //private void ultraTreeActivityType_ColumnSetGenerated(object sender, Infragistics.Win.UltraWinTree.ColumnSetGeneratedEventArgs e)
        //{
        //    try
        //    {
        //        switch (e.ColumnSet.Key)
        //        {
        //            case "ActivityType":
        //                // We want Spaceport nodes to show the SpacePort Name, 
        //                // so set the NodeTextColumn.
        //                e.ColumnSet.NodeTextColumn = e.ColumnSet.Columns[CashManagementConstants.COLUMN_ACTIVITYTYPE];
        //                break;
        //            default:
        //                break;
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

        //private void ultraTreeActivityType_InitializeDataNode(object sender, Infragistics.Win.UltraWinTree.InitializeDataNodeEventArgs e)
        //{
        //    try
        //    {
        //        // Populate the unbound "Full Name" column with a combination
        //        // of the First_Name and Last_Name fields. 
        //        //string fullName = string.Format("{0}",e.Node.Cells["ActivityType"].Value.ToString());

        //        //e.Node.Cells[CashManagementConstants.COLUMN_ACTIVITYTYPE].Key = e.Node.Cells[CashManagementConstants.COLUMN_ACTIVITYTYPEID].Value.ToString();
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

        private void ultraTreeActivityType_AfterSelect(object sender, SelectEventArgs e)
        {
            try
            {
                if (ultraTreeActivityType.SelectedNodes.Count > 0 && ultraTreeActivityType.SelectedNodes[0].HasNodes == false)
                {
                    if (e.NewSelections.Count > 0)
                    {
                        UltraTreeNode node = (UltraTreeNode)e.NewSelections.All[0];
                        if (String.IsNullOrEmpty(node.Key.ToString()))
                        {
                            MessageBox.Show("Please save the activity type first");
                        }
                        else
                        {
                            DataRow[] result = _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].Select(CashManagementConstants.COLUMN_ACTIVITYTYPEID_FK + " = '" + node.Key.ToString() + "'");
                            if (result.Length > 0)
                            {
                                bool isEnabled;
                                bool.TryParse(result[0][CashManagementConstants.RELATION_ISENABLED].ToString(), out isEnabled);
                                CashAccountsUI.Instance.DisablePanel(isEnabled, CashManagementAccountSetup.ActivityJournalMapping);

                                foreach (DataRow row in result)
                                {
                                    grdJournalData.DisplayLayout.Bands[0].Override.AllowUpdate = bool.Parse(row[CashManagementConstants.RELATION_ISENABLED].ToString()) ? DefaultableBoolean.True : DefaultableBoolean.False;
                                    BindDataToJournalGrid(row);
                                }
                            }
                            else
                            {
                                grdJournalData.DataSource = null;
                            }
                        }
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

        internal DataSet GetDataSet()
        {
            if (grdJournalData.Rows.Count > 0)
                grdJournalData.Rows[0].Cells[0].Activated = true;
            grdJournalData.UpdateData();
            return _dsActivities;
        }

        internal string GetSelectedTreeNodeIndex()
        {
            try
            {
                if (ultraTreeActivityType.SelectedNodes.Count > 0 && ultraTreeActivityType.SelectedNodes[0] != null)
                {
                    return ultraTreeActivityType.SelectedNodes[0].Key;
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
            return string.Empty;
        }
        internal void SetDataSources()
        {
            InitializeDataSets();
        }

        //internal void RestoreDefault()
        //{
        //    InitializeDataSets();
        //}

        private void ctrlActivityJournalMapping_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.ApplyTheme)
                {
                    Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
                    appearance16.BackColor = System.Drawing.Color.Black;
                    appearance16.BackColor2 = System.Drawing.Color.White;
                    appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                    appearance16.BorderColor = System.Drawing.Color.Black;
                    appearance16.FontData.BoldAsString = "True";
                    appearance16.FontData.SizeInPoints = 11F;
                    appearance16.ForeColor = System.Drawing.Color.White;
                    appearance16.TextVAlignAsString = "Middle";
                    this.lblActivityJournalMapping.Appearance = appearance16;

                    Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
                    appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                    appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(167)))), ((int)(((byte)(155)))));
                    appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                    appearance15.FontData.BoldAsString = "True";
                    appearance15.FontData.SizeInPoints = 10F;
                    appearance15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(64)))), ((int)(((byte)(52)))));
                    appearance15.TextVAlignAsString = "Middle";
                    //this.ultraLabel3.Appearance = appearance15;

                    Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
                    appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                    appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(167)))), ((int)(((byte)(155)))));
                    appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                    appearance1.FontData.BoldAsString = "True";
                    appearance1.FontData.SizeInPoints = 10F;
                    appearance1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(64)))), ((int)(((byte)(52)))));
                    appearance1.TextVAlignAsString = "Middle";
                    this.ultraLabel2.Appearance = appearance1;
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

        private void grdJournalData_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            isUnSavedChanges = true;
        }

        public Infragistics.Documents.Excel.Workbook GetGridDataToExportToExcel()
        {
            Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
            try
            {

                string workbookName = "ActivityJournalMapping_" + DateTime.Now.Date.ToString("yyyyMMdd");
                workBook.Worksheets.Add(workbookName);

                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];

                workBook = this.ultraGridExcelExporter1.Export(this.grdJournalData, workBook.Worksheets[workbookName]);
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

        internal void SelectDefaultTreeNode()
        {
            try
            {
                if (ultraTreeActivityType.Nodes.Count > 0)
                {
                    ultraTreeActivityType.Nodes[0].Selected = true;
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

        internal void SetSelectedNode(string selectedNodeKey)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(selectedNodeKey) && ultraTreeActivityType.Nodes.Count > 0 && ultraTreeActivityType.ActiveNode != null)
                {
                    ultraTreeActivityType.SelectedNodes.Clear();
                    foreach (UltraTreeNode node in ultraTreeActivityType.Nodes)
                    {
                        if (node.Nodes.Count > 0 && node.Nodes.Exists(selectedNodeKey))
                        {
                            node.Nodes[selectedNodeKey].Selected = true;
                            ultraTreeActivityType.ActiveNode = ultraTreeActivityType.Nodes[node.Key].Nodes[selectedNodeKey];
                            break;
                        }
                    }
                    //ultraTreeActivityType.Nodes[selectedNodeKey].Selected = true;
                    //ultraTreeActivityType.ActiveNode = ultraTreeActivityType.Nodes[selectedNodeKey];
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
        /// Added by: Bharat Raturi, 13 oct 2014
        /// Check if there is redundant mapping
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-5074
        /// </summary>
        /// <returns>True if redundant mapping. false otherwise</returns>
        //internal bool HasDuplicateMapping()
        //{
        //    try
        //    {
        //        List<CashJournalMappingItem> lstSame = new List<CashJournalMappingItem>();
        //        StringBuilder duplicateMap = new StringBuilder();
        //        foreach (int mapID in _dictMapping.Keys)
        //        {
        //            List<int> creditCount = _dictMapping[mapID].CreditAccount;
        //            List<int> debitCount = _dictMapping[mapID].DebitAccount;

        //            var equalCredit = _dictMapping.Values.Where(x => x.CreditAccount.SequenceEqual(creditCount) && x.MappingID != mapID); 
        //            if (equalCredit.ToList() == null || equalCredit.ToList().Count == 0)
        //            {
        //                continue;
        //            }
        //            var equalDebit = equalCredit.Where(x => x.DebitAccount.SequenceEqual(debitCount));


        //            if (equalDebit != null)
        //            {
        //                lstSame.AddRange(equalDebit);
        //                if (lstSame != null && lstSame.Count > 0)
        //                {
        //                    duplicateMap.Append(_vlActivityType.FindByDataValue(mapID).DisplayText + ", ");
        //                    foreach (CashJournalMappingItem item in lstSame)
        //                        duplicateMap.Append(_vlActivityType.FindByDataValue(item.MappingID).DisplayText + ",");
        //                    MessageBox.Show("Following activities have same mapping:\n" + duplicateMap.ToString().TrimEnd(','), "Mapping Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    return true;
        //                }
        //            }
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
        //    return false;
        //}

        private void txtFilterActivity_ValueChanged(object sender, EventArgs e)
        {
            FilterActivityTypeNodes(txtFilterActivity.Text.ToLower().Trim());
        }

        private void FilterActivityTypeNodes(string searchText)
        {
            if (!string.IsNullOrWhiteSpace(searchText) && searchText != CashManagementConstants.DEFAULTACTIVITYSEARCHTEXT.ToLower())
                foreach (UltraTreeNode node in ultraTreeActivityType.Nodes)
                    HideNodes(node, searchText);
            else
                foreach (UltraTreeNode node in ultraTreeActivityType.Nodes)
                    node.Nodes.All.Cast<UltraTreeNode>().ToList().Where(t => t.Visible == false).ToList().ForEach(t => t.Visible = true);
        }

        private void HideNodes(UltraTreeNode node, string searchText)
        {
            node.Nodes.All.Cast<UltraTreeNode>().ToList().ForEach(t => t.Visible = false);
            node.Nodes.All.Cast<UltraTreeNode>().ToList().Where(t => t.Text.ToLower().Contains(searchText)).ToList().ForEach(n => n.Visible = true);
        }

        internal void EmptyFilterTextBox()
        {
            txtFilterActivity.Text = "";
        }

        private void grdJournalData_CellDataError(object sender, CellDataErrorEventArgs e)
        {
            try
            {
                e.RestoreOriginalValue = true;
                e.RaiseErrorEvent = false;
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
