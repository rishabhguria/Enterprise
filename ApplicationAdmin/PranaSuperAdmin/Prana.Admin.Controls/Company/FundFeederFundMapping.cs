using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinListView;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    public partial class AccountFeederAccountMapping : UserControl
    {
        /// <summary>
        /// Variable to hold company ID
        /// </summary>
        public int _companyID = int.MinValue;

        /// <summary>
        /// flag variable to check if the records have to be saved 
        /// </summary>
        static bool _isSaveRequired = false;
        int _accountID = -1;
        //int _prevAccountID = -1;
        ValueList _currencyList = new ValueList();
        ValueList _feederList = new ValueList();

        public AccountFeederAccountMapping()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the control with data
        /// </summary>
        /// <param name="comp_ID">Company ID</param>
        public void InitializeControl(int comp_ID)
        {
            try
            {
                _companyID = comp_ID;
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

            try
            {
                AccountFeederAccountMappingManager.companyID = _companyID;
                Dictionary<int, string> mFeeders = AccountFeederAccountMappingManager.GetAllAccounts(_companyID);
                if (ulvAccounts.Items.Count != 0)
                {
                    ulvAccounts.Items.Clear();
                }
                foreach (int feederID in mFeeders.Keys)
                {
                    UltraListViewItem ulItem = new UltraListViewItem();
                    ulItem.Tag = mFeeders[feederID];
                    ulItem.Key = feederID.ToString();
                    ulItem.Value = mFeeders[feederID];
                    ulvAccounts.Items.Add(ulItem);
                }

                Dictionary<int, string> currencyCollection = AccountFeederAccountMappingManager.GetCurrencies();
                _currencyList.ValueListItems.Clear();
                foreach (int key in currencyCollection.Keys)
                    _currencyList.ValueListItems.Add(key, currencyCollection[key]);

                Dictionary<int, string> feederCollection = AccountFeederAccountMappingManager.GetFeederIDNames();

                _feederList.ValueListItems.Clear();
                foreach (int key in feederCollection.Keys)
                {
                    _feederList.ValueListItems.Add(key, feederCollection[key]);
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
        /// Fill the grid with the data
        /// </summary>
        public void LoadFeederGridData()
        {
            try
            {
                int accountID = 0;
                accountID = int.Parse(ulvAccounts.SelectedItems[0].Key);
                if (!_isSaveRequired)
                {
                    ugvFeederAccounts.DataSource = AccountFeederAccountMappingManager.GetFeedersForCurrentAccount(accountID);
                }
                else
                {
                    ugvFeederAccounts.DataSource = AccountFeederAccountMappingManager.LoadNewMapping(accountID);
                    if (ugvFeederAccounts.Rows.Count != 0)
                    {
                        List<AccountFeederMapItem> accountFeederMap = GetMappedFeeder(ugvFeederAccounts);
                        //AccountFeederAccountMappingManager.RemoveAllocationDetails(accountFeederMap);
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
        /// Create list of feedrs objects for mapping that are in the ultragrid
        /// </summary>
        /// <param name="ugFeeders">UltraGrid control</param>
        /// <returns>Collection of Feeder IDs</returns>
        List<AccountFeederMapItem> GetMappedFeeder(UltraGrid ugFeeders)
        {
            List<AccountFeederMapItem> mappedFeederCollection = new List<AccountFeederMapItem>();
            try
            {
                foreach (UltraGridRow ugRow in ugFeeders.Rows)
                {
                    if (ugRow.Cells["FeederAccountID"].Text != "" && ugRow.Cells["Amount"].Text != "")
                    {
                        AccountFeederMapItem mappedFeeder = new AccountFeederMapItem();
                        mappedFeeder.AccountID = _accountID;
                        mappedFeeder.FeederAccountID = Convert.ToInt32(ugRow.Cells["FeederAccountID"].Value);
                        mappedFeeder.AllocatedAmount = Convert.ToDecimal(ugRow.Cells["AllocatedAmount"].Value);
                        mappedFeeder.CompanyID = _companyID;
                        mappedFeederCollection.Add(mappedFeeder);
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
            return mappedFeederCollection;
        }

        /// <summary>
        /// Customize the grid for display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ugvFeederAccounts_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = e.Layout.Bands[0];
                UltraGridColumn fAccountNameCol = band.Columns["FeederAccountID"];
                fAccountNameCol.Header.Caption = "Name";
                fAccountNameCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                fAccountNameCol.ValueList = _feederList;

                UltraGridColumn fAccountShortNameCol = band.Columns["FeederAccountShortName"];
                fAccountShortNameCol.Header.Caption = "Short Name";
                fAccountShortNameCol.CellActivation = Activation.NoEdit;

                UltraGridColumn CurrencyCol = band.Columns["Currency"];
                CurrencyCol.Header.Caption = "Currency";
                CurrencyCol.ValueList = _currencyList;
                CurrencyCol.CellActivation = Activation.NoEdit;

                UltraGridColumn fAccountAmountCol = band.Columns["Amount"];
                fAccountAmountCol.Header.Caption = "Total Amount";
                fAccountAmountCol.Hidden = true;
                e.Layout.Bands[0].Columns["RemainingAmount"].Hidden = true;
                //e.Layout.Bands[0].Columns["AllocatedAmount"].Hidden = true;
                UltraGridColumn fAccountAllocatedAmountCol = band.Columns["AllocatedAmount"];
                fAccountAllocatedAmountCol.Header.Caption = "Amount";
                ugvFeederAccounts.DisplayLayout.ViewStyleBand = ViewStyleBand.Horizontal;
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
                ugvFeederAccounts.DisplayLayout.Bands[0].AddNew();
                if (!_isSaveRequired)
                    _isSaveRequired = true;
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
        /// Save the mapping and load the feeders accounts for the selected account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulvAccounts_ItemSelectionChanged(object sender, Infragistics.Win.UltraWinListView.ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (_isSaveRequired)
                {
                    if (_accountID != -1)
                    {
                        AccountFeederAccountMappingManager objMapping = new AccountFeederAccountMappingManager();
                        List<AccountFeederMapItem> accountFeederMap = GetMappedFeeder(ugvFeederAccounts);
                        objMapping.SetNewMapping(_accountID, accountFeederMap);
                        //AccountFeederAccountMappingManager.SetAllocationDetails(accountFeederMap);
                    }
                }
                if (ulvAccounts.SelectedItems.Count == 1)
                {
                    _accountID = Convert.ToInt32(ulvAccounts.SelectedItems[0].Key);
                    LoadFeederGridData();
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
        /// Remove the active row from the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (ugvFeederAccounts.ActiveRow.Cells["FeederAccountShortName"].Text != "")
                {
                    if (!_isSaveRequired)
                        _isSaveRequired = true;
                }
                ugvFeederAccounts.ActiveRow.Delete();
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ugvFeederAccounts_CellChange(object sender, CellEventArgs e)
        {
            if (!_isSaveRequired)
            {
                _isSaveRequired = true;
            }
            try
            {
                if (e.Cell.Column.Header.Caption == "Name")
                {
                    UltraGridColumn gridColumn = e.Cell.Column;
                    String ColumnText = e.Cell.Text;
                    EmbeddableEditorBase editor = e.Cell.EditorResolved;
                    object changedValue = editor.IsValid ? editor.Value : editor.CurrentEditText;
                    int feederID = Convert.ToInt32(changedValue);
                    FeederAccountItem feederAccount = AccountFeederAccountMappingManager.GetSingleFeeder(feederID);
                    UltraGridRow ugRow = ugvFeederAccounts.ActiveRow;
                    ugRow.Cells["FeederAccountID"].Value = feederAccount.FeederAccountID;
                    ugRow.Cells["FeederAccountShortName"].Value = feederAccount.FeederShortName;
                    ugRow.Cells["Amount"].Value = feederAccount.FeederAmount;
                    ugRow.Cells["Currency"].Value = feederAccount.FeederCurrency;
                    ugRow.Cells["RemainingAmount"].Value = feederAccount.FeederRemainingAmount;
                    ugRow.Cells["AllocatedAmount"].Value = feederAccount.FeederRemainingAmount;
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

        private void ulvAccounts_ItemActivated(object sender, ItemActivatedEventArgs e)
        {
            //try
            //{
            //    _prevAccountID = _accountID;
            //    _accountID = Convert.ToInt32(ulvAccounts.SelectedItems[0].Key);
            //}
            //catch (ArgumentOutOfRangeException)
            //{
            //    MessageBox.Show("Select a account from the list to proceed", "Prana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        /// <summary>
        /// Create the feeder objects from the data in the grid
        /// </summary>
        /// <param name="grid">The required grid</param>
        /// <returns>The list of feeder objects</returns>
        private List<AccountFeederMapItem> CreateFeederItems(UltraGrid grid)
        {
            List<AccountFeederMapItem> feeders = new List<AccountFeederMapItem>();
            try
            {
                foreach (UltraGridRow ugRow in grid.Rows)
                {
                    if (ugRow.Cells["FeederAccountShortName"].Text != "")
                    {
                        AccountFeederMapItem mappedFeeder = new AccountFeederMapItem();
                        mappedFeeder.FeederAccountID = Convert.ToInt32(ugRow.Cells["FeederAccountID"].Value);
                        mappedFeeder.AllocatedAmount = Convert.ToDecimal(ugRow.Cells["AllocatedAmount"].Text);
                        feeders.Add(mappedFeeder);
                    }
                }
                return feeders;
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

        private void ugvFeederAccounts_BeforeCellListDropDown(object sender, CancelableCellEventArgs e)
        {

        }

        /// <summary>
        /// Validate the allocated amount for the feeder account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ugvFeederAccounts_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Header.Caption == "Amount")
            {
                try
                {
                    UltraGridRow ugRow = e.Cell.Row;
                    decimal all_Amount = Convert.ToDecimal(e.Cell.Value);
                    decimal rem_Amount = Convert.ToDecimal(ugRow.Cells["RemainingAmount"].Value);
                    if (all_Amount > rem_Amount)
                    {
                        MessageBox.Show("Insufficient amount for allocation", "Prana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        e.Cell.Value = Convert.ToDecimal(ugRow.Cells["RemainingAmount"].Value);
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
        }

        /// <summary>
        /// save the data in the database
        /// </summary>
        public void SaveData()
        {
            try
            {
                AccountFeederAccountMappingManager objMapping = new AccountFeederAccountMappingManager();
                List<AccountFeederMapItem> accountFeederMap = GetMappedFeeder(ugvFeederAccounts);
                objMapping.SetNewMapping(_accountID, accountFeederMap);
                int i = AccountFeederAccountMappingManager.SaveMapping();
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

        private void AccountFeederAccountMapping_Load(object sender, EventArgs e)
        {

        }
    }
}