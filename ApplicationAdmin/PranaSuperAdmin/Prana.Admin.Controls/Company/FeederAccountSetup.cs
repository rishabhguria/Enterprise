using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    public partial class FeederAccountSetup : UserControl
    {
        /// <summary>
        /// company id
        /// </summary>
        public int _companyID = -1;

        /// <summary>
        /// falg variable to check if the feeder account is enabled
        /// </summary>
        bool _isFeederAccountEnabled = false;

        /// <summary>
        /// IDs of feeder accounts
        /// </summary>
        List<int> feederAccountsIDCollection;

        public bool _isSaveRequired = false;
        public bool _isValidData = true;
        public bool _isAllocationError = false;

        public FeederAccountSetup()
        {
            InitializeComponent();
        }
        private ValueList _currencyList = new ValueList();

        /// <summary>
        /// initialized data at load time
        /// </summary>
        public void InitializeControl()
        {
            try
            {
                //uceFeederAccount.Checked = true;
                //feederAccountsIDCollection = new List<int>();
                LoadFeederGridData(_companyID, _isFeederAccountEnabled);
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
        /// When the control loads, load the records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeederAccountSetup_Load(object sender, EventArgs e)
        {
            try
            {
                uceFeederAccount_CheckedChanged(sender, e);
                feederAccountsIDCollection = new List<int>();
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
        /// Fill the grid with the data
        /// </summary>
        /// <param name="companyID">ID of the current client</param>
        /// <param name="isFeederAccountEnabled">true if the feeders are enabled</param>
        public void LoadFeederGridData(int companyID, bool isFeederAccountEnabled)
        {
            try
            {
                this._companyID = companyID;
                uceFeederAccount.Checked = isFeederAccountEnabled;
                _isFeederAccountEnabled = isFeederAccountEnabled;
                ugrFeederAccounts.DataSource = FeederAccountManager.GetSelectedFeederDetails(_companyID);
                //if (isFeederAccountEnabled)
                //{
                //    this.Enabled = true;
                //}
                //else
                //{
                //    this.Enabled = false;
                //}
                ActivateFeederAccounts();
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
        /// Show or hide the data based on whether the checkbox is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uceFeederAccount_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ActivateFeederAccounts();
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
        /// activate or deactivate the control according to whether the feeder accounts are enabled or not 
        /// </summary>
        private void ActivateFeederAccounts()
        {
            if (uceFeederAccount.Checked)
            {
                _isFeederAccountEnabled = true;
                //LoadFeederGridData(_companyID, _isFeederAccountEnabled);
                ugrFeederAccounts.Enabled = true;
                btnAdd.Enabled = btnDelete.Enabled = btnEdit.Enabled = true;
                uceFeederAccount.Text = "Disable Feeder Accounts";
            }
            else
            {
                _isFeederAccountEnabled = false;
                //ugrFeederAccounts.DataSource = null;
                btnAdd.Enabled = btnDelete.Enabled = btnEdit.Enabled = false;
                ugrFeederAccounts.Enabled = false;
                uceFeederAccount.Text = "Enable Feeder Accounts";
            }
        }

        /// <summary>
        /// Customize the UltraGrid Columns
        /// </summary>
        /// <param name="sender">UltraGrid Control</param>
        /// <param name="e"></param>
        private void ugrFeederAccounts_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                Dictionary<int, string> currencyCollection = FeederAccountManager.GetCurrencies();
                _currencyList.ValueListItems.Clear();
                foreach (int key in currencyCollection.Keys)
                    _currencyList.ValueListItems.Add(key, currencyCollection[key]);
                UltraGridBand band = e.Layout.Bands[0];
                band.Override.AllowRowFiltering = DefaultableBoolean.True;
                UltraGridColumn fAccountNameCol = band.Columns["FeederFundName"];
                fAccountNameCol.Header.Caption = "Name";
                //fAccountNameCol.CellActivation = Activation.NoEdit;

                UltraGridColumn fAccountShortNameCol = band.Columns["FeederFundShortName"];
                fAccountShortNameCol.Header.Caption = "Short Name";
                //fAccountShortNameCol.CellActivation = Activation.NoEdit;


                // JIRA : CHMW-679
                //UltraGridColumn fAccountAmountCol = band.Columns["Amount"];
                //fAccountAmountCol.Header.Caption = "Total Amount";
                //fAccountCapitalCol.CellActivation = Activation.NoEdit;

                //UltraGridColumn CurrencyCol = band.Columns["Currency"];
                //CurrencyCol.Header.Caption = "Currency";
                //CurrencyCol.CellActivation = Activation.NoEdit;
                //CurrencyCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                //CurrencyCol.ValueList = _currencyList;
                e.Layout.Bands[0].Columns["FeederFundID"].Hidden = true;
                e.Layout.Bands[0].Columns["AllocatedAmount"].Hidden = true;
                e.Layout.Bands[0].Columns["Amount"].Hidden = true;
                e.Layout.Bands[0].Columns["Currency"].Hidden = true;
                foreach (UltraGridRow ugRow in ugrFeederAccounts.Rows)
                {
                    ugRow.Activation = Activation.NoEdit;
                }
                ugrFeederAccounts.DisplayLayout.ViewStyleBand = ViewStyleBand.Horizontal;
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
        /// Create the List of account names from the grid
        /// </summary>
        /// <param name="ugGrid">the grid</param>
        /// <returns>the lits of feeder account names</returns>
        List<string> MakeFeederList(UltraGrid ugGrid)
        {
            List<string> makeFeederList = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(HasDuplicates()) || HasEmpty())
                    return null;

                foreach (UltraGridRow ugRow in ugGrid.Rows)
                {
                    if (ugRow.Cells["FeederFundName"].Text.Trim() != "" && ugRow.Cells["FeederFundID"].Text == "")
                        makeFeederList.Add(ugRow.Cells["FeederFundName"].Text.Trim());
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
            return makeFeederList;
        }

        /// <summary>
        /// check for duplicate feeder account names in the grid
        /// </summary>
        /// <returns>true if any name is duplicated</returns>
        public string HasDuplicates()
        {
            try
            {
                List<string> feederNameCollection = new List<string>();
                foreach (UltraGridRow ugRow in ugrFeederAccounts.Rows)
                {
                    if (ugRow.Cells["FeederFundName"].Text.Trim() != "")
                    {
                        if (feederNameCollection.Contains(ugRow.Cells["FeederFundName"].Text.Trim().ToLower()))
                        {
                            return ugRow.Cells["FeederFundName"].Text;
                        }
                        feederNameCollection.Add(ugRow.Cells["FeederFundName"].Text.Trim().ToLower());
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// check for empty details in the grid
        /// </summary>
        /// <returns>true if any cell is empty</returns>
        public bool HasEmpty()
        {
            try
            {
                for (int i = ugrFeederAccounts.Rows.Count - 1; i >= 0; i--)
                {
                    UltraGridRow ugRow = ugrFeederAccounts.Rows[i];
                    if (string.IsNullOrEmpty(ugRow.Cells[0].Text) && string.IsNullOrEmpty(ugRow.Cells[1].Text) && string.IsNullOrEmpty(ugRow.Cells[2].Text)
                        && string.IsNullOrEmpty(ugRow.Cells[3].Text))
                    {
                        ugRow.Delete(false);
                        continue;
                    }
                    if (string.IsNullOrEmpty(ugRow.Cells[1].Text) || string.IsNullOrEmpty(ugRow.Cells[2].Text) || string.IsNullOrEmpty(ugRow.Cells[3].Text))
                    {
                        return true;
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
            return false;
            //bool isBlank = false;
            //int id = 0;
            //try
            //{
            //    foreach (UltraGridRow ugRow in ugrFeederAccounts.Rows)
            //    {
            //        try
            //        {
            //            id = Convert.ToInt32(ugRow.Cells[0].Text.Trim());
            //        }
            //        catch (Exception)
            //        {
            //            id = 0;
            //        }
            //        if (id == 0)
            //        {
            //            //check for empty cells
            //            if (ugRow.Cells[1].Text.Trim() == "" && ugRow.Cells[2].Text.Trim() == "" && ugRow.Cells[3].Text.Trim() == "")
            //                continue;
            //            else if (ugRow.Cells[1].Text.Trim() == "" || ugRow.Cells[2].Text.Trim() == "" || ugRow.Cells[3].Text.Trim() == "")
            //            {
            //                isBlank = true;
            //                break;
            //            }
            //        }
            //        else
            //        {
            //            if (ugRow.Cells[1].Text.Trim() == "" || ugRow.Cells[2].Text.Trim() == "" || ugRow.Cells[3].Text.Trim() == "")
            //            {
            //                isBlank = true;
            //                break;
            //            }

            //        }
            //    }
            //    if (isBlank)
            //    {
            //        //MessageBox.Show("Blank details cannot be inserted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return true;
            //    }
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
            //return false;
        }

        /// <summary>
        /// Save the last unsaved row of the grid
        /// </summary>
        /// <returns>True if the data is saved</returns>
        public bool SaveData()
        {
            try
            {
                //ValidateAllocatedAmount();
                if (!_isSaveRequired)
                    return true;
                if (HasEmpty())
                {
                    MessageBox.Show("Blank details for feeder accounts cannot be inserted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _isValidData = false;
                    return false;
                }
                _isValidData = true;
                if (FeederAccountManager.HasDuplicateFeeders(MakeFeederList(ugrFeederAccounts)) == true)
                {
                    MessageBox.Show("Feeder name already exists in the database", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _isValidData = false;
                    return false;
                }
                if (!UpdateData())
                    return false;
                int count = ugrFeederAccounts.Rows.Count;
                bool isBlank = false;

                List<FeederAccountItem> allAccountsDetails = new List<FeederAccountItem>();

                try
                {
                    foreach (UltraGridRow ugRow in ugrFeederAccounts.Rows)
                    {
                        if (ugRow.Cells["FeederFundID"].Text == "")
                        {
                            string[] accountValues = new string[5];
                            accountValues[0] = ugRow.Cells["FeederFundName"].Text;
                            accountValues[1] = ugRow.Cells["FeederFundShortName"].Text;
                            accountValues[2] = ugRow.Cells["Amount"].Text;
                            //accountValues[3] = ugRow.Cells["Currency"].Value.ToString();
                            //accountValues[4] = (Convert.ToDecimal(ugRow.Cells["Amount"].Text) - Convert.ToDecimal(ugRow.Cells["AllocatedAmount"].Text)).ToString();

                            foreach (string value in accountValues)
                            {
                                if (value == "")
                                {
                                    isBlank = true;
                                    break;
                                }
                            }
                            if (isBlank)
                                break;
                            FeederAccountItem accountDetails = new FeederAccountItem();
                            //put the values of the last row cells in the array
                            accountDetails.FeederAccountName = accountValues[0];
                            accountDetails.FeederShortName = accountValues[1];
                            accountDetails.FeederAmount = Convert.ToDecimal(accountValues[2]);
                            //accountDetails.FeederCurrency = Convert.ToInt32(accountValues[3]);
                            //accountDetails.FeederRemainingAmount = Convert.ToDecimal(accountValues[4]);
                            accountDetails.FeederCompanyId = _companyID;
                            allAccountsDetails.Add(accountDetails);
                        }
                    }
                    FeederAccountManager.SaveFeederAccounts(allAccountsDetails);
                    _isSaveRequired = false;
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
            finally
            {
                //LoadFeederGridData(_companyID);
            }
            return true;
        }

        /// <summary>
        /// Update the existing Feeder account details
        /// </summary>
        /// <returns>true if successfully updated</returns>
        private bool UpdateData()
        {
            if (feederAccountsIDCollection.Count != 0)
                if (!DeleteData())
                    return false;
            if (!string.IsNullOrEmpty(HasDuplicates()))
            {
                MessageBox.Show("'" + HasDuplicates() + "' already exists. Choose a different name", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _isValidData = false;
                return false;
            }
            else if (HasEmpty())
            {
                //MessageBox.Show("Empty Details cannot be inserted", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //int i = 0;
            bool isBlank = false;
            List<FeederAccountItem> allAccountsDetails = new List<FeederAccountItem>();
            try
            {
                foreach (UltraGridRow ugRow in ugrFeederAccounts.Rows)
                {
                    if (ugRow.Cells["FeederFundID"].Text != "")
                    {
                        string[] accountValues = new string[5];
                        accountValues[0] = ugRow.Cells["FeederFundID"].Text;
                        accountValues[1] = ugRow.Cells["FeederFundName"].Text;
                        accountValues[2] = ugRow.Cells["FeederFundShortName"].Text;
                        accountValues[3] = ugRow.Cells["Amount"].Text;
                        accountValues[4] = ugRow.Cells["Currency"].Value.ToString();
                        //accountValues[5] = (Convert.ToDecimal(ugRow.Cells["Amount"].Text) - Convert.ToDecimal(ugRow.Cells["AllocatedAmount"].Text)).ToString();

                        foreach (string value in accountValues)
                        {
                            if (value == "")
                            {
                                isBlank = true;
                                break;
                            }
                        }
                        if (isBlank)
                        {
                            //MessageBox.Show("Blank values cannot be saved", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        FeederAccountItem accountDetails = new FeederAccountItem();

                        //put the values of the active row cells in the array
                        accountDetails.FeederAccountID = Convert.ToInt32(accountValues[0]);
                        accountDetails.FeederAccountName = accountValues[1];
                        accountDetails.FeederShortName = accountValues[2];
                        accountDetails.FeederAmount = Convert.ToDecimal(accountValues[3]);
                        accountDetails.FeederCurrency = Convert.ToInt32(accountValues[4]);
                        accountDetails.FeederRemainingAmount = 0.0M;
                        accountDetails.FeederCompanyId = _companyID;

                        allAccountsDetails.Add(accountDetails);
                    }
                }
                FeederAccountManager.UpdateFeederAccounts(allAccountsDetails);
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
            return true;
        }

        /// <summary>
        /// delete the feeder details
        /// </summary>
        /// <returns>true if all the specified feeders were deleted</returns>
        public bool DeleteData()
        {
            try
            {
                //check for empty or duplicate records
                if (!string.IsNullOrEmpty(HasDuplicates()) || HasEmpty())
                    return false;
                int total = feederAccountsIDCollection.Count;
                int deleted = FeederAccountManager.DeleteSelectedFeeder(feederAccountsIDCollection);
                feederAccountsIDCollection.Clear();
                if (deleted < total)
                    MessageBox.Show("Some Feeder Accounts cannot be deleted because of association with other accounts", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            return true;
        }

        /// <summary>
        /// Add new row to the grid to add new record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isAllocationError)
                {
                    return;
                }
                ugrFeederAccounts.DisplayLayout.Bands[0].AddNew();
                _isSaveRequired = true;
                foreach (UltraGridRow ugRow in ugrFeederAccounts.Rows)
                    ugRow.Activation = Activation.NoEdit;
                ugrFeederAccounts.ActiveRow.Activation = Activation.AllowEdit;
                ugrFeederAccounts.ActiveRow.Cells["Amount"].Activation = Activation.NoEdit;
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
        /// activate the row for editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                _isSaveRequired = true;
                foreach (UltraGridRow ugRow in ugrFeederAccounts.Rows)
                    ugRow.Activation = Activation.NoEdit;
                ugrFeederAccounts.ActiveRow.Activation = Activation.AllowEdit;
                ugrFeederAccounts.ActiveRow.Cells["Amount"].Activation = Activation.NoEdit;
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
        /// remove the row from the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ugrFeederAccounts.ActiveRow != null)
                {
                    int feederAccountId = 0;
                    if (!string.IsNullOrEmpty(ugrFeederAccounts.ActiveRow.Cells["FeederFundID"].Value.ToString()) || !string.IsNullOrEmpty(ugrFeederAccounts.ActiveRow.Cells["FeederFundName"].Value.ToString()))
                    {
                        if (!string.IsNullOrEmpty(ugrFeederAccounts.ActiveRow.Cells["Amount"].Value.ToString()) && Convert.ToDecimal(ugrFeederAccounts.ActiveRow.Cells["Amount"].Value) > 0.0m)
                        {
                            MessageBox.Show("The feeder is mapped with accounts. Unmap the feeder to delete it.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (!string.IsNullOrEmpty(ugrFeederAccounts.ActiveRow.Cells["FeederFundID"].Value.ToString()))
                        {
                            feederAccountId = Convert.ToInt32(ugrFeederAccounts.ActiveRow.Cells["FeederFundID"].Text);
                        }
                        DialogResult dr = MessageBox.Show("Do you want to delete the selected feeder? '" + ugrFeederAccounts.ActiveRow.Cells["FeederFundName"].Text, "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                        {
                            return;
                        }
                        if (feederAccountId > 0)
                        {
                            feederAccountsIDCollection.Add(feederAccountId);
                            _isSaveRequired = true;
                        }
                    }
                    ugrFeederAccounts.ActiveRow.Delete(false);
                }
                else
                {
                    MessageBox.Show("Select a row to delete feeder account.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                return;
            }
        }

        /// <summary>
        /// make the new added row editable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ugrFeederAccounts_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                if (ugrFeederAccounts.ActiveRow.Cells["FeederFundID"].Text == "")
                {
                    foreach (UltraGridRow ugRow in ugrFeederAccounts.Rows)
                        ugRow.Activation = Activation.NoEdit;
                    ugrFeederAccounts.ActiveRow.Activation = Activation.AllowEdit;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                return;
            }
        }

        /// <summary>
        /// Validate the account amount. THe total amount cannot be less than the allocated amount
        /// </summary>
        //private void ValidateAllocatedAmount()
        //{
        //    foreach (UltraGridRow ugRow in ugrFeederAccounts.Rows)
        //    {
        //        if (ugRow.Cells["Amount"].Text != "" && ugRow.Cells["AllocatedAmount"].Text != "")
        //        {
        //            try
        //            {
        //                if (Convert.ToDecimal(ugRow.Cells["Amount"].Text) < Convert.ToDecimal(ugRow.Cells["AllocatedAmount"].Text))
        //                {
        //                    MessageBox.Show("The total amount cannot be less then the allocated Amount.\nUnallocate sufficient amount first.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    ugRow.Cells["Amount"].Value = ugRow.Cells["AllocatedAmount"].Text;
        //                    return;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                // Invoke our policy that is responsible for making sure no secure information
        //                // gets out of our layer.
        //                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //                if (rethrow)
        //                {
        //                    throw;
        //                }
        //            }
        //        }
        //    }
        //}


        /// <summary>
        /// Do not show the row deletion confirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void ugrFeederAccounts_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        //{
        //    try
        //    {
        //        e.DisplayPromptMsg = false;
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

        /// <summary>
        /// Validate the amount
        /// Amount cannot be less than the amount that is already assigned to other accounts of the company
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ugrFeederAccounts_BeforeCellDeactivate(object sender, CancelEventArgs e)
        {
            UltraGridRow ugRow = ugrFeederAccounts.ActiveRow;
            if (ugRow.Cells["FeederFundName"].Text != "" && ugRow.Cells["Amount"].Text == "")
            {
                ugRow.Cells["Amount"].Value = Convert.ToDecimal("0.00");
            }
            //ValidateAllocatedAmount();
        }

        /// <summary>
        /// Gets the value whether the feedre account is enabled
        /// </summary>
        /// <returns>true if the feeder accounts are enabled</returns>
        public bool GetIsFeederAccountEnabled()
        {
            return _isFeederAccountEnabled;
        }
    }
}
