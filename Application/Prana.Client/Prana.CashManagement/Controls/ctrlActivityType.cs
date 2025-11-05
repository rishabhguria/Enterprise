using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
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
    public partial class ctrlActivityType : UserControl
    {
        DataSet _dsActivities = new DataSet();
        ValueList _vlBalanceType = new ValueList();
        ValueList _vlActivitySourceType = new ValueList();
        public bool _isUnSavedChanges = false;


        //bool _isNewRow = false;
        public ctrlActivityType()
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
                _dsActivities = CachedDataManager.GetInstance.GetAllActivityTables();
                _vlActivitySourceType = ValueListHelper.GetInstance.GetActivitySourceTypeValueList().Clone();
                _vlBalanceType.Reset();
                _vlBalanceType.ValueListItems.Add("1", "Cash");
                _vlBalanceType.ValueListItems.Add("2", "Accrual");
                grdActivityTypeDetails.DataSource = _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYTYPE];
                SetReadOnlyRows();
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
        void ctrlActivityType_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.ApplyTheme)
                {
                    Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
                    appearance14.BackColor = System.Drawing.Color.Black;
                    appearance14.BackColor2 = System.Drawing.Color.White;
                    appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                    appearance14.BorderColor = System.Drawing.Color.Black;
                    appearance14.FontData.BoldAsString = "True";
                    appearance14.FontData.SizeInPoints = 11F;
                    appearance14.ForeColor = System.Drawing.Color.White;
                    appearance14.TextVAlignAsString = "Middle";
                    this.lblActivityJournalMapping.Appearance = appearance14;

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
        private void grdActivityTypeDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                //Group columns on the basis of activity source
                //grdActivityTypeDetails.DisplayLayout.Bands[0].SortedColumns.Add(CashManagementConstants.COLUMN_ACTIVITYSOURCE, false, true);
                //grdActivityTypeDetails.Rows.ExpandAll(true);
                //list of columns which are visible
                List<string> lsColumnsToDisplay = new List<string>(new string[] { CashManagementConstants.COLUMN_ACTIVITYACRONYM, CashManagementConstants.COLUMN_ACTIVITYTYPE, CashManagementConstants.COLUMN_ACTIVITYSOURCE, CashManagementConstants.COLUMN_DESCRIPTION, CashManagementConstants.COLUMN_BALANCETYPE });
                UltraWinGridUtils.HideColumns(grdActivityTypeDetails.DisplayLayout.Bands[0]);
                UltraWinGridUtils.SetBand(lsColumnsToDisplay, grdActivityTypeDetails.DisplayLayout.Bands[0]);
                //define caption, column width and value list for the column
                //value lists are so that user can select value from drop down
                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYACRONYM].Header.Caption = CashManagementConstants.CAPTION_ACTIVITYACRONYM;
                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYACRONYM].Width = 150;


                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYTYPE].Header.Caption = CashManagementConstants.CAPTION_ACTIVITYTYPE;
                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYTYPE].Width = 180;

                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYSOURCE].ValueList = _vlActivitySourceType;
                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYSOURCE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYSOURCE].Width = 180;
                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYSOURCE].Header.Caption = CashManagementConstants.CAPTION_ACTIVITYSOURCE;

                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_DESCRIPTION].Width = 180;
                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_DESCRIPTION].Header.Caption = CashManagementConstants.CAPTION_DESCRIPTION;

                //show column balance type 
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-5419
                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_BALANCETYPE].ValueList = _vlBalanceType;
                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_BALANCETYPE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_BALANCETYPE].Width = 180;
                grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_BALANCETYPE].Header.Caption = CashManagementConstants.COLUMN_BALANCETYPE;
                e.Layout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;

                grdActivityTypeDetails.DisplayLayout.Override.RowAppearance.BackColor = Color.Black;
                grdActivityTypeDetails.DisplayLayout.Override.RowAppearance.ForeColor = Color.White;
                grdActivityTypeDetails.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.Gray;
                grdActivityTypeDetails.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.Black;
                grdActivityTypeDetails.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;

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

        private void grdActivityTypeDetails_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                switch (e.Cell.Column.Key)
                {
                    case CashManagementConstants.COLUMN_AMOUNTTYPEID_FK:
                        //if(listViewActivityJournalMapping.SelectedItems.Count>0)
                        //{
                        //    DataRow[] rowsHavingActivityTypeAndAmountType = _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYJOURNALMAPPING].Select(CashManagementConstants.COLUMN_AMOUNTTYPEID_FK+ " = " +e.Cell.Value + " AND " +CashManagementConstants.COLUMN_ACTIVITYTYPEID_FK+ " = " +ultraTreeActivityType.SelectedNodes[0].DataKey);
                        //    if(rowsHavingActivityTypeAndAmountType.Length>0)
                        //    {
                        //        grdActivityTypeDetails.AfterCellUpdate -=new CellEventHandler(grdActivityTypeDetails_AfterCellUpdate);
                        //        e.Cell.Value = e.Cell.OriginalValue;
                        //        grdActivityTypeDetails.AfterCellUpdate += new CellEventHandler(grdActivityTypeDetails_AfterCellUpdate);
                        //        MessageBox.Show("Cannot Add multiple journal entries for same Amount Type", "Journal Mapping Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //        grdActivityTypeDetails.ActiveCell = e.Cell;
                        //    }
                        //}
                        break;
                    case CashManagementConstants.COLUMN_DEBITACCOUNT:
                        break;
                    case CashManagementConstants.COLUMN_CREDITACCOUNT:
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

        #region commented
        /// <summary>
        /// Narendra Kumar jangir
        /// Date: Apr 2013 25
        /// Desc: Bind Journal Data to the Grid On the basis of JournalCode
        /// </summary>
        /// 
        //private void BindDataToActivityTypeGrid(DataRow row)
        //{
        //    try
        //    {
        //        //check that modified datatable is matched
        //        if (row.Table.TableName == CashManagementConstants.TABLE_ACTIVITYTYPE)
        //        {
        //            grdActivityTypeDetails.DataSource = row.Table.DataSet.Tables[CashManagementConstants.TABLE_ACTIVITYTYPE];
        //            //list of columns which are visible
        //            List<string> lsColumnsToDisplay = new List<string>(new string[] { CashManagementConstants.COLUMN_ACTIVITYTYPE, "Description", "BalanceType" });
        //            UltraWinGridUtils.HideColumns(grdActivityTypeDetails.DisplayLayout.Bands[0]);
        //            UltraWinGridUtils.SetBand(lsColumnsToDisplay, grdActivityTypeDetails.DisplayLayout.Bands[0]);
        //            //define caption, column width and value list for the column
        //            //value lists are so that user can select value from drop down
        //            //grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_AMOUNTTYPE].ValueList = _vlActiType;
        //            //grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_AMOUNTTYPE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
        //            grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYTYPE].Header.Caption = "Activity Type";
        //            grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_ACTIVITYTYPE].Width = 180;

        //            //grdActivityTypeDetails.DisplayLayout.Bands[0].Columns[CashManagementConstants.COLUMN_DEBITACCOUNT].ValueList = _vlSubAccount;
        //            //grdActivityTypeDetails.DisplayLayout.Bands[0].Columns["Description"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
        //            grdActivityTypeDetails.DisplayLayout.Bands[0].Columns["Description"].Width = 180;
        //            grdActivityTypeDetails.DisplayLayout.Bands[0].Columns["Description"].Header.Caption = "Description";

        //            grdActivityTypeDetails.DisplayLayout.Bands[0].Columns["BalanceType"].ValueList = _vlIsDividendType;
        //            grdActivityTypeDetails.DisplayLayout.Bands[0].Columns["BalanceType"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
        //            grdActivityTypeDetails.DisplayLayout.Bands[0].Columns["BalanceType"].Width = 180;
        //            grdActivityTypeDetails.DisplayLayout.Bands[0].Columns["BalanceType"].Header.Caption = "BalanceType";


        //            foreach (UltraGridRow gr in grdActivityTypeDetails.Rows)
        //            {

        //                DataRow gridRow = ((System.Data.DataRowView)(gr.ListObject)).Row;
        //                if (gridRow != row)
        //                {
        //                    if (!gridRow[CashManagementConstants.COLUMN_ACTIVITYTYPE].ToString().Equals(row[CashManagementConstants.COLUMN_ACTIVITYTYPE].ToString()))
        //                    {
        //                        gr.Hidden = true;
        //                    }
        //                }
        //                else
        //                {
        //                    // gr.Band.Columns["Acronym"].CellActivation = Activation.AllowEdit;
        //                    gr.Hidden = false;
        //                    //ValidateRow(gr.Cells[1]);
        //                    //ValidateRow(gr.Cells[4]);
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
        //}
        //private void GenerateActivityType(string NewJournalCode)
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
        //            BindDataToActivityTypeGrid(rowJournalEntry);
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
        #endregion

        //private void grdActivityTypeDetails_InitializeRow(object sender, InitializeRowEventArgs e)
        //{
        //try
        //{
        //        if ((e.Row.Cells[CashManagementConstants.COLUMN_ACTIVITYACRONYM].Value.ToString()).Equals("UnknownActivity"))
        //        {
        //            e.Row.Activation = Activation.NoEdit;
        //            return;
        //        }
        //    if (!_isNewRow)
        //{
        //        _isNewRow = false;
        //        e.Row.Cells[CashManagementConstants.COLUMN_ACTIVITYACRONYM].Activation = Activation.NoEdit;
        //}
        //    //apply text color setting
        //    //if (e.Row.Appearance.BackColor.Equals(Color.Black))
        //    //{
        //    //    e.Row.Appearance.ForeColor = Color.White;
        //    //}
        //    //else if(e.Row.Appearance.BackColor.Equals(Color.White))
        //    //    e.Row.Appearance.ForeColor = Color.Black;
        //}
        //catch (Exception ex)
        //{
        //    // Invoke our policy that is responsible for making sure no secure information
        //    // gets out of our layer.
        //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //    if (rethrow)
        //    {
        //        throw;
        //    }
        //}
        //}
        /// <summary>
        /// Add journal mapping for the activity type
        /// </summary>
        public void AddNewActivityType()
        {
            try
            {
                //_isNewRow = true;
                _dsActivities = (grdActivityTypeDetails.DataSource as DataTable).DataSet;
                DataRow row = _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYTYPE].NewRow();

                //activity type id will set according to selected item from activity type listview.
                //row[CashManagementConstants.COLUMN_ACTIVITYTYPEID] = int.MinValue;
                //row[CashManagementConstants.COLUMN_ACTIVITYTYPE] = "select";
                row[CashManagementConstants.COLUMN_ACTIVITYTYPE] = string.Empty;
                row["Description"] = string.Empty;
                row["BalanceType"] = 1;
                row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYSOURCE, "Activity Source cannot be empty ! ");
                row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYACRONYM, "Acronym cannot be empty ! ");
                row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYTYPE, "Activity Type cannot be empty !");

                _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYTYPE].Rows.Add(row);
                //grdActivityTypeDetails.DataSource = null;
                //grdActivityTypeDetails.DataSource = _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYTYPE];
                //grdActivityTypeDetails.Update();
                grdActivityTypeDetails.Rows[grdActivityTypeDetails.Rows.Count - 1].Selected = true;
                grdActivityTypeDetails.Rows[grdActivityTypeDetails.Rows.Count - 1].Activate();
                //BindDataToActivityTypeGrid(row);
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
        public void DeleteActivityType()
        {
            try
            {
                if (this.grdActivityTypeDetails.ActiveRow != null && this.grdActivityTypeDetails.ActiveRow.Index >= 0)
                {
                    object activityIDValue = grdActivityTypeDetails.ActiveRow.Cells[CashManagementConstants.COLUMN_ACTIVITYTYPEID].Value;
                    if (String.IsNullOrEmpty(activityIDValue.ToString()))
                    {
                        if (MessageBox.Show("Do you want to delete the selected activity?", "Delete Activity", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            grdActivityTypeDetails.Rows[this.grdActivityTypeDetails.ActiveRow.Index].Delete(false);
                    }
                    else if (this.grdActivityTypeDetails.ActiveRow.Cells[CashManagementConstants.CAPTION_ACTIVITYACRONYM].Value.ToString().Equals("UnknownActivity"))
                    {
                        MessageBox.Show("Cannot delete this Activity Type", "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        int activityTypeId = Convert.ToInt32(grdActivityTypeDetails.ActiveRow.Cells[CashManagementConstants.COLUMN_ACTIVITYTYPEID].Value.ToString());
                        if (!CashAccountDataManager.IfActivityTypeIsInUse(activityTypeId))
                        {
                            //remove the row from the grid, changes will be reflected in the underlying datasource/datatable.
                            if (MessageBox.Show("Do you want to delete the selected activity?", "Delete Activity", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                grdActivityTypeDetails.Rows[this.grdActivityTypeDetails.ActiveRow.Index].Delete(false);
                        }
                        else
                        {
                            MessageBox.Show("Cash Transaction-Activity Mapping is in use.", "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //MessageBox.Show("Cash Transaction-Activity Mapping is in use.", "Cash Management", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
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

        //private void ultraTreeActivityType_AfterSelect(object sender, SelectEventArgs e)
        //{
        //    try
        //    {
        //        if (ultraTreeActivityType.SelectedNodes.Count > 0)
        //        {
        //            if (e.NewSelections.Count > 0)
        //            {
        //                UltraTreeNode node = (UltraTreeNode)e.NewSelections.All[0];
        //                DataRow[] result = _dsActivities.Tables[CashManagementConstants.TABLE_ACTIVITYTYPE].Select(CashManagementConstants.COLUMN_ACTIVITYTYPEID + " = '" + node.Cells[CashManagementConstants.COLUMN_ACTIVITYTYPEID].Text + "'");
        //                if (result.Length > 0)
        //                {
        //                    foreach (DataRow row in result)
        //                    {
        //                        BindDataToActivityTypeGrid(row);
        //                    }
        //                }
        //                else
        //                    grdActivityTypeDetails.DataSource = null;
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
        //}
        //internal DataSet GetDataSet()
        //{
        //    grdActivityTypeDetails.UpdateData();
        //    return _dsActivities;
        //}
        internal void SetDataSources()
        {
            InitializeDataSets();
        }

        //internal void RestoreDefault()
        //{
        //    InitializeDataSets();
        //}

        private void grdActivityTypeDetails_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            _isUnSavedChanges = true;
        }

        private void grdActivityTypeDetails_BeforeRowDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (sender as UltraGrid != null && (sender as UltraGrid).ActiveRow != null)
                {
                    if (!IsValidRow((sender as UltraGrid).ActiveRow))
                        return;
                    IsValidActivityNameAndAcronym((sender as UltraGrid).ActiveRow);// || ((sender as UltraGrid).ActiveRow.ListObject as DataRowView).Row.RowState==DataRowState.Deleted)
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
        /// Added by: Bharat raturi, Nov 04 2014
        /// Validate the acronym and activity name for duplicates
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-5310
        /// </summary>
        /// <param name="columnKey"></param>
        private bool IsValidActivityNameAndAcronym(UltraGridRow grdRow)
        {
            try
            {
                bool isdupAcronym = false;
                bool isdupActivity = false;
                DataRow row = ((System.Data.DataRowView)(grdRow.ListObject)).Row;
                row.ClearErrors();
                foreach (DataColumn column in row.Table.Columns)
                {
                    if (column.ColumnName != CashManagementConstants.COLUMN_ACTIVITYACRONYM
                        && column.ColumnName != CashManagementConstants.COLUMN_ACTIVITYTYPE)
                    {
                        continue;
                    }
                    string columnKey = column.ColumnName.ToString();
                    if (columnKey == CashManagementConstants.COLUMN_ACTIVITYACRONYM)
                    {
                        var acronymColl = (from dtRow in grdActivityTypeDetails.Rows.AsEnumerable() where dtRow.Cells[CashManagementConstants.COLUMN_ACTIVITYACRONYM].Text.ToString().ToLower() == grdRow.Cells[columnKey].Text.ToString().ToLower() select new { grdRow.Cells[columnKey].Text }).ToList();
                        if (acronymColl != null && acronymColl.ToList().Count > 1)
                        {
                            row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYACRONYM, "Duplicate Acronym");
                            isdupAcronym = true;
                        }
                    }
                    if (columnKey == CashManagementConstants.COLUMN_ACTIVITYTYPE)
                    {
                        var acronymColl = (from dtRow in grdActivityTypeDetails.Rows.AsEnumerable() where dtRow.Cells[CashManagementConstants.COLUMN_ACTIVITYTYPE].Text.ToString().ToLower() == grdRow.Cells[columnKey].Text.ToString().ToLower() select new { grdRow.Cells[columnKey].Text }).ToList();
                        if (acronymColl != null && acronymColl.ToList().Count > 1)
                        {
                            row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYTYPE, "Duplicate ActivityType");
                            isdupActivity = true; ;
                        }
                    }
                    if (isdupAcronym || isdupActivity)
                    {
                        return false;
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
            return true;
        }

        private bool IsValidRow(UltraGridRow grdRow)
        {
            try
            {
                DataRow row = ((System.Data.DataRowView)(grdRow.ListObject)).Row;
                row.ClearErrors();
                foreach (DataColumn column in row.Table.Columns)
                {
                    if (column.ColumnName != CashManagementConstants.COLUMN_ACTIVITYACRONYM
                        && column.ColumnName != CashManagementConstants.COLUMN_ACTIVITYTYPE
                        && column.ColumnName != CashManagementConstants.COLUMN_ACTIVITYSOURCE)
                    {
                        continue;
                    }
                    string columnModified = column.ColumnName.ToString();
                    //if (!_isAcronymError && !_isTypeError)
                    //    row.ClearErrors();
                    switch (columnModified)
                    {
                        case CashManagementConstants.COLUMN_ACTIVITYACRONYM:
                            if (string.IsNullOrWhiteSpace(row[column].ToString()))
                            {
                                row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYACRONYM, "Acronym can not be Empty ! ");
                                return false;
                            }
                            break;

                        case CashManagementConstants.COLUMN_ACTIVITYTYPE:
                            if (string.IsNullOrWhiteSpace(row[column].ToString()))
                            {
                                row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYTYPE, "activity Type can not be Empty !");
                                return false;
                            }
                            break;

                        case CashManagementConstants.COLUMN_ACTIVITYSOURCE:
                            if (string.IsNullOrWhiteSpace(row[column].ToString()))
                            {
                                row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYSOURCE, "activity Source can not be Empty ! ");
                                return false;
                            }
                            break;
                        default:
                            if (String.IsNullOrEmpty(row[CashManagementConstants.COLUMN_ACTIVITYACRONYM].ToString()))
                            {
                                row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYACRONYM, "Acronym cannot be empty ! ");
                                return false;
                            }
                            else if (String.IsNullOrEmpty(row[CashManagementConstants.COLUMN_ACTIVITYTYPE].ToString()))
                            {
                                row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYTYPE, "Activity Type cannot be empty !");
                                return false;
                            }
                            else if (String.IsNullOrEmpty(row[CashManagementConstants.COLUMN_ACTIVITYSOURCE].ToString()))
                            {
                                row.SetColumnError(CashManagementConstants.COLUMN_ACTIVITYSOURCE, "Activity Source cannot be empty ! ");
                                return false;
                            }
                            break;
                    }
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
            return true;
        }

        public Infragistics.Documents.Excel.Workbook GetGridDataToExportToExcel()
        {
            Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
            try
            {

                string workbookName = "Activity Type" + DateTime.Now.Date.ToString("yyyyMMdd");
                workBook.Worksheets.Add(workbookName);

                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];

                workBook = this.ultraGridExcelExporter1.Export(this.grdActivityTypeDetails, workBook.Worksheets[workbookName]);
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

        /// <summary>
        /// added by: bharat Raturi, 06 Nov 2014
        /// make acronym read only
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-5310
        /// </summary>
        private void SetReadOnlyRows()
        {
            grdActivityTypeDetails.Rows.AsEnumerable().First(row => row.Cells[CashManagementConstants.COLUMN_ACTIVITYACRONYM].Value.ToString() == "UnknownActivity").Activation = Activation.NoEdit;
            grdActivityTypeDetails.Rows.AsEnumerable().ToList().ForEach(r => r.Cells[CashManagementConstants.COLUMN_ACTIVITYACRONYM].Activation = Activation.NoEdit);
        }

        /// <summary>
        /// Added by: Bharat Raturi, 05 nov 2014
        /// Clean up the errors of the current row before saving the changes
        /// </summary>
        internal void CleanTableErrors()
        {
            try
            {
                (grdActivityTypeDetails.DataSource as DataTable).AsEnumerable().Where(row => row.RowState == DataRowState.Deleted).ToList().ForEach(r => r.ClearErrors());
                foreach (UltraGridRow grdRow in grdActivityTypeDetails.Rows)
                {
                    grdActivityTypeDetails.ActiveRow = grdRow;
                    grdActivityTypeDetails_BeforeRowDeactivate(grdActivityTypeDetails, null);
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
    }
}
