using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    public partial class Release : UserControl
    {
        public bool _isSaveRequired = false;
        public bool _isValidData = false;

        public Release()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                this.ugReleaseDetails.DisplayLayout.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
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
        /// Initialize the control data
        /// </summary>
        public void InitializeData()
        {
            try
            {
                DataSet dsClient = new DataSet();
                dsClient = ReleaseSetupManager.GetClientFromDB();
                ucbClient.DataSource = dsClient;
                ucbClient.DisplayLayout.Bands[0].Columns["CompanyID"].Hidden = true;
                ucbClientAccounts.DataSource = ReleaseSetupManager.GetAccounts(dsClient);
                ucbClientAccounts.DisplayLayout.Bands[0].Columns["FundID"].Hidden = true;
                ugReleaseDetails.DataSource = ReleaseSetupManager.GetReleaseDetails();
                SetAccountEditorControl();
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
        /// To set accounts for saved releases.
        /// </summary>
        private void SetAccountEditorControl()
        {
            try
            {
                foreach (UltraGridRow row in ugReleaseDetails.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(row.Cells["Company"].Value.ToString()))
                    {
                        DataSet dsClient = new DataSet("dsClient");
                        DataTable dtClient = new DataTable("dtClient");
                        dtClient.Columns.Add("CompanyID", typeof(int));

                        List<object> listClient = (List<object>)row.Cells["Company"].Value;
                        foreach (int clientID in listClient)
                        {
                            dtClient.Rows.Add(clientID);
                        }
                        dsClient.Tables.Add(dtClient);
                        row.Cells["Account"].EditorComponent = EditorComponentForCell(dsClient);
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

        /// <summary>
        /// Event to initialize Release details grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ugReleaseDetails_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = e.Layout.Bands[0];
                band.Override.AllowRowFiltering = DefaultableBoolean.True;
                if (!band.Columns.Exists("DeleteButton"))
                {
                    UltraGridColumn colDelete = band.Columns.Add("DeleteButton");
                    colDelete.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colDelete.Width = 20;
                    colDelete.Header.Caption = "";
                    colDelete.Header.VisiblePosition = 0;
                    colDelete.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }

                if (ucbClient.DataSource != null)
                {
                    if (!ucbClient.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn cbClient = ucbClient.DisplayLayout.Bands[0].Columns.Add();
                        cbClient.Key = "Selected";
                        cbClient.Header.Caption = string.Empty;
                        cbClient.Width = 25;
                        cbClient.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        cbClient.DataType = typeof(bool);
                        cbClient.Header.VisiblePosition = 1;
                    }
                    ucbClient.CheckedListSettings.CheckStateMember = "Selected";
                    ucbClient.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    ucbClient.CheckedListSettings.ListSeparator = " , ";
                    ucbClient.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    ucbClient.DisplayMember = "CompanyName";
                    ucbClient.ValueMember = "CompanyID";
                    ucbClient.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
                }

                UltraGridColumn colClient = band.Columns["Company"];
                colClient.Header.Caption = "Company";
                ucbClient.NullText = "-Select-";
                colClient.EditorComponent = ucbClient;

                if (!ucbClientAccounts.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                {
                    UltraGridColumn cbAccounts = ucbClientAccounts.DisplayLayout.Bands[0].Columns.Add();
                    cbAccounts.Key = "Selected";
                    cbAccounts.Header.Caption = string.Empty;
                    cbAccounts.Width = 25;
                    cbAccounts.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    cbAccounts.DataType = typeof(bool);
                    cbAccounts.Header.VisiblePosition = 1;
                }
                ucbClientAccounts.CheckedListSettings.CheckStateMember = "Selected";
                ucbClientAccounts.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                ucbClientAccounts.CheckedListSettings.ListSeparator = " , ";
                ucbClientAccounts.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                ucbClientAccounts.DisplayMember = "AccountName";
                ucbClientAccounts.ValueMember = "AccountID";
                ucbClientAccounts.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";

                UltraGridColumn colAccounts = band.Columns["Account"];
                colAccounts.Header.Caption = "Account";
                ucbClientAccounts.NullText = "-Select-";
                colAccounts.EditorComponent = ucbClientAccounts;

                ugReleaseDetails.DisplayLayout.Bands[0].Columns["ReleaseID"].Hidden = true;
                ugReleaseDetails.DisplayLayout.Bands[0].Columns["InUse"].Hidden = true;
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
        /// Add new rows to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ugReleaseDetails.DisplayLayout.Bands[0].AddNew();
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
        /// Handle the click of the delete button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ugReleaseDetails_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                UltraGridRow row = e.Cell.Row;
                if (e.Cell.Column.Key == "DeleteButton")
                {
                    if (!string.IsNullOrEmpty(row.Cells["InUse"].Value.ToString()))
                    {
                        if (Convert.ToInt32(row.Cells["InUse"].Value.ToString()) == 0)
                        {
                            if (!string.IsNullOrEmpty(row.Cells["Company"].Value.ToString()) && !string.IsNullOrEmpty(row.Cells["Account"].Value.ToString()))
                            {
                                DialogResult dr = MessageBox.Show("Do you want to delete the selected release?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (dr == DialogResult.No)
                                {
                                    return;
                                }
                                if (!_isSaveRequired)
                                {
                                    _isSaveRequired = true;
                                }
                            }
                            e.Cell.Row.Delete(false);
                        }
                        else
                        {
                            MessageBox.Show("Cannot delete the release details as the release is in use.\n Please remove association with batches to delete the release details.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        e.Cell.Row.Delete(false);
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

        /// <summary>
        /// Create the List of release Details from grid
        /// </summary>
        /// <returns>The list of lists with each element list having the details of one release</returns>
        public List<List<string>> CreateReleaseList()
        {
            try
            {
                List<List<string>> releaseDetails = new List<List<string>>();
                DataTable dt = (DataTable)ugReleaseDetails.DataSource;
                foreach (DataRow dRow in dt.Rows)
                {
                    if (dRow[0] == DBNull.Value)
                    {
                        try
                        {
                            // For new rows Release id set to 0. since in database auto id is assigned to newly created release.
                            dRow[0] = 0;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            dRow[0] = 1;
                        }
                    }
                    List<object> listClient = (List<object>)dRow["Company"];
                    foreach (int clientID in listClient)
                    {
                        List<object> listAccount = (List<object>)dRow["Account"];
                        foreach (int accountID in listAccount)
                        {
                            string[] values = { dRow[0].ToString(), dRow[1].ToString(), clientID.ToString(), accountID.ToString(), dRow[4].ToString(), dRow[5].ToString(), dRow[6].ToString(), dRow[7].ToString() };
                            List<string> releaseData = new List<string>(values);
                            releaseDetails.Add(releaseData);
                        }
                    }
                }
                return releaseDetails;
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
            return null;
        }

        /// <summary>
        /// Check if grid has empty rows or empty cells
        /// </summary>
        /// <returns>True if there are empty cells</returns>
        public bool HasEmpty()
        {
            Dictionary<int, string> dicaccounts = new Dictionary<int, string>();
            List<string> listRelease = new List<string>();
            for (int i = ugReleaseDetails.Rows.Count - 1; i >= 0; i--)
            {
                UltraGridRow ugRow = ugReleaseDetails.Rows[i];

                if (string.IsNullOrEmpty(ugRow.Cells["Company"].Text) && string.IsNullOrEmpty(ugRow.Cells["Account"].Text)
                    && ugRow.Cells["ReleaseName"].Text == string.Empty && ugRow.Cells["IP"].Text == string.Empty && ugRow.Cells["ReleasePath"].Text == string.Empty
                    && ugRow.Cells["ClientDB_Name"].Text == string.Empty && ugRow.Cells["SMDB_Name"].Text == string.Empty)
                {
                    ugRow.Delete(false);
                    continue;
                }
                if (string.IsNullOrEmpty(ugRow.Cells["Company"].Text) || string.IsNullOrEmpty(ugRow.Cells["Account"].Text)
                    || ugRow.Cells["ReleaseName"].Text == string.Empty || ugRow.Cells["IP"].Text == string.Empty && ugRow.Cells["ReleasePath"].Text == string.Empty
                    || ugRow.Cells["ClientDB_Name"].Text == string.Empty || ugRow.Cells["SMDB_Name"].Text == string.Empty)
                {
                    MessageBox.Show("Blank release cannot be inserted. \nFill in all the details", "Release", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }

                // Validation for account release association
                List<object> listAccounts = (List<object>)ugRow.Cells["Account"].Value;
                foreach (int accountName in listAccounts)
                {
                    if (!dicaccounts.ContainsKey(accountName))
                    {
                        dicaccounts.Add(accountName, "account");
                    }
                    else
                    {
                        MessageBox.Show("A account can be associated with single release.", "Release", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _isValidData = false;
                        return true;
                    }
                }

                // Validation for unique release name
                if (!listRelease.Contains(ugRow.Cells["ReleaseName"].Text.Trim()))
                {
                    listRelease.Add(ugRow.Cells["ReleaseName"].Text.Trim());
                }
                else
                {
                    MessageBox.Show("Duplicate release cannot be inserted. Details could not be saved.", "Release", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _isValidData = false;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Save the release details in the Database
        /// </summary>
        public int SaveReleaseDetails()
        {
            try
            {
                if (ugReleaseDetails.DisplayLayout.Bands[0].Override.AllowUpdate == DefaultableBoolean.False)
                {
                    return 1;
                }
                if (!_isSaveRequired)
                {
                    return 1;
                }
                if (HasEmpty())
                {
                    _isValidData = false;
                    return 0;
                }
                List<List<string>> releaseList = CreateReleaseList();
                if (releaseList != null)
                {
                    bool isSaved = ReleaseSetupManager.SaveReleaseData(releaseList);
                    if (!isSaved)
                    {
                        _isValidData = false;
                        MessageBox.Show("Duplicate release cannot be inserted. Details could not be saved.", "Release", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        ugReleaseDetails.UpdateData();
                        return 1;
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
            return 0;
        }

        /// <summary>
        /// Called when cell value changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ugReleaseDetails_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (!_isSaveRequired)
                {
                    _isSaveRequired = true;
                }
                // Modified by Ankit Gupta on 2nd Jan, 2015
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2200
                //if (e.Cell.Column.Key == "Company")
                //{
                //    // To invoke AfterCellUpdate event of ultragrid.
                //    ugReleaseDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode);
                //}
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
        /// Event fired when cell value change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ugReleaseDetails_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "Company")
                {
                    DataSet dsClientList = new DataSet("dsClient");
                    DataTable dtClientList = new DataTable("dtClient");
                    dtClientList.Columns.Add("CompanyID", typeof(int));

                    UltraGridRow row = e.Cell.Row;
                    List<object> listClient = (List<object>)row.Cells["Company"].Value;
                    foreach (int clientID in listClient)
                    {
                        dtClientList.Rows.Add(clientID);
                    }
                    dsClientList.Tables.Add(dtClientList);

                    ugReleaseDetails.ActiveRow.Cells["Account"].EditorComponent = EditorComponentForCell(dsClientList);
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
        /// Function to bind client wise accounts in dropdown.
        /// </summary>
        /// <param name="dsClientList"></param>
        /// <returns></returns>
        private UltraCombo EditorComponentForCell(DataSet dsClientList)
        {
            UltraCombo cmbCell = new UltraCombo();
            try
            {
                cmbCell.DataSource = ReleaseSetupManager.GetAccounts(dsClientList);
                if (cmbCell.DataSource != null)
                {
                    if (!cmbCell.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn colSelectAccount = cmbCell.DisplayLayout.Bands[0].Columns.Add();
                        colSelectAccount.Key = "Selected";
                        colSelectAccount.Header.Caption = string.Empty;
                        colSelectAccount.Width = 25;
                        colSelectAccount.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        colSelectAccount.DataType = typeof(bool);
                        colSelectAccount.Header.VisiblePosition = 1;
                    }
                    cmbCell.CheckedListSettings.CheckStateMember = "Selected";
                    cmbCell.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    cmbCell.CheckedListSettings.ListSeparator = " , ";
                    cmbCell.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    cmbCell.DisplayMember = "AccountName";
                    cmbCell.ValueMember = "AccountID";
                    cmbCell.NullText = "-Select-";
                    cmbCell.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
                    cmbCell.DisplayLayout.Bands[0].Columns[0].Hidden = true;
                }
                if (cmbCell.Rows.Count == 0)
                {
                    cmbCell.SelectedText = string.Empty;
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
            return cmbCell;
        }

        /// <summary>
        /// added by: Bharat Raturi, 13 jun 2014
        /// make the controls read only if the user does not have write permission
        /// </summary>
        /// <param name="isActive"></param>
        public void SetGridAccess(bool isActive)
        {
            try
            {
                if (!isActive)
                {
                    ugReleaseDetails.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                    //grdPermissions.DisplayLayout.Bands[0].Override.AllowAddNew = DefaultableBoolean.False;
                    //grdPermissions.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                    btnAdd.Enabled = false;
                }
                else
                {
                    ugReleaseDetails.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                    btnAdd.Enabled = true;
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
        /// Adding tool tip for each row of company name drop down.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-2201
        /// [Admin] Tool tip should be there for company name and account name in release setup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucbClient_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists("CompanyName"))
                {
                    e.Row.ToolTipText = e.Row.Cells["CompanyName"].Text;
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
        /// Adding tool tip for each row of accounts name drop down.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-2201
        /// [Admin] Tool tip should be there for company name and account name in release setup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucbClientAccounts_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists("AccountName"))
                {
                    e.Row.ToolTipText = e.Row.Cells["AccountName"].Text;
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
