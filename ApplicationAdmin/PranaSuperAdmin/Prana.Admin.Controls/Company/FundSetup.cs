using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.AuditManager.Definitions.Constants;
using Prana.AuditManager.Definitions.Interface;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.AccountCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.AccountUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.AccountApproved, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.AccountDeleted, ShowAuditUI = true)]
    public partial class AccountSetup : UserControl, IAuditSource
    {
        #region GlobalVariables
        /// <summary>
        /// ID of the company
        /// </summary>
        public int _companyID = 0;

        /// <summary>
        /// Flag variable to check if the data is to be saved
        /// </summary>
        public bool _isSaveRequired = false;

        public bool _isDataChanged = false;

        public bool _isMappingChanged = false;

        /// <summary>
        /// Flag to see if there is invalid data
        /// this will be set when the data is to be saved
        /// </summary>
        public bool _isInvalidData = false;

        /// <summary>
        /// Temporary variable to generate IDs for the new accounts
        /// </summary>
        public int accountID = -1;

        public int tempAccountID = -1;

        /// <summary>
        /// Flag variable to indicate whether the feeder Account setup enabled or not
        /// </summary>
        private static bool _isFeederAccountEnabled = false;

        /// <summary>
        /// ValueList for the Schedule Type
        /// </summary>
        ValueList _vlSchedule = new ValueList();
        ValueList _currencyList = new ValueList();

        // creating instance for tooptip

        Infragistics.Win.UltraWinToolTip.UltraToolTipInfo tipInfo = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("-Select-", Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);
        //Added by Faisal Shah 24/07/14
        //Purpose To Track Index on AccountName
        private string _accountName = String.Empty;

        #endregion

        [AuditManager.Attributes.AuditSourceConstAttri]
        public AccountSetup()
        {
            InitializeComponent();
        }

        // Added By : Manvendra P.
        // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3519 
        int _cmpyBaseCurrencyID;
        public void InitializeControl(int companyID, bool isFeederAccountEnabled, int companyBaseCurrencyID, bool isSendAllocations)
        {
            try
            {
                _cmpyBaseCurrencyID = companyBaseCurrencyID;
                cmbCurrency.Enabled = false;
                errorProvider1.Clear();
                this._companyID = companyID;
                _isFeederAccountEnabled = isFeederAccountEnabled;
                grdAccounts.DataSource = AccountSetupManager.InitializeAccountDetails(_companyID);
                cmbCurrency.DataSource = AccountSetupManager.GetCurrencyDetails();
                cmbCurrency.DropDownStyle = UltraComboStyle.DropDownList;
                cmbCurrency.DisplayLayout.Bands[0].Columns["CurrencyID"].Hidden = true;
                cmbCurrency.ValueMember = "CurrencyID";
                cmbCurrency.DisplayMember = "Name";
                cmbCurrency.DisplayLayout.Bands[0].ColHeadersVisible = false;

                cmbClosing.DataSource = AccountSetupManager.GetClosingMethods();
                cmbClosing.DropDownStyle = UltraComboStyle.DropDownList;
                cmbClosing.DisplayLayout.Bands[0].Columns["MethodID"].Hidden = true;
                cmbClosing.ValueMember = "MethodID";
                cmbClosing.DisplayMember = "MethodName";
                cmbClosing.DisplayLayout.Bands[0].ColHeadersVisible = false;

                cmbPrimeBroker.DataSource = AccountSetupManager.GetThirdParty();
                cmbPrimeBroker.DisplayLayout.Bands[0].Columns["ThirdPartyID"].Hidden = true;
                cmbPrimeBroker.DropDownStyle = UltraComboStyle.DropDownList;
                cmbPrimeBroker.ValueMember = "ThirdPartyID";
                cmbPrimeBroker.DisplayMember = "ThirdPartyName";
                cmbPrimeBroker.DisplayLayout.Bands[0].ColHeadersVisible = false;

                cmbSort.DataSource = AccountSetupManager.GetSecondarySort();
                cmbSort.DisplayLayout.Bands[0].Columns["SecondarySortID"].Hidden = true;
                cmbSort.DropDownStyle = UltraComboStyle.DropDownList;
                cmbSort.ValueMember = "SecondarySortID";
                cmbSort.DisplayMember = "SecondarySortName";
                cmbSort.DisplayLayout.Bands[0].ColHeadersVisible = false;

                cmbLockSchedule.DataSource = AccountSetupManager.GetScheduleTypes();
                cmbLockSchedule.DisplayLayout.Bands[0].Columns["ScheduleID"].Hidden = true;
                cmbLockSchedule.DropDownStyle = UltraComboStyle.DropDownList;
                cmbLockSchedule.ValueMember = "ScheduleID";
                cmbLockSchedule.DisplayMember = "ScheduleName";
                cmbLockSchedule.DisplayLayout.Bands[0].ColHeadersVisible = false;

                swapAccountDropDown.ClearAll();
                swapAccountDropDown.SetTextEditorText("No  Account(s) Selected");
                swapAccountDropDown.AddItemsToTheCheckList(AccountSetupManager.GetAccountList(), CheckState.Checked, AccountSetupManager.GetSwapAccounts());
                allocationCheckBox.Checked = isSendAllocations;
                swapAccountDropDown.Enabled = allocationCheckBox.Checked;
                swapAccountDropDown.CheckStateChanged += swapAccountDropDown_CheckStateChanged;

                SetActiveRow();
                LoadFeederGridData();

                // Purpose : To bind currency column in feeder account setup.
                Dictionary<int, string> currencyCollection = AccountFeederAccountMappingManager.GetCurrencies();
                _currencyList.ValueListItems.Clear();
                foreach (int key in currencyCollection.Keys)
                    _currencyList.ValueListItems.Add(key, currencyCollection[key]);

                if (_isFeederAccountEnabled)
                {
                    grdFeederAccounts.Enabled = true;
                    btnAddFeederAccount.Enabled = true;
                    btnDeleteFeederAccount.Enabled = true;
                    btnEditFeederAccount.Enabled = true;
                }
                else
                {
                    grdFeederAccounts.Enabled = false;
                    btnAddFeederAccount.Enabled = false;
                    btnDeleteFeederAccount.Enabled = false;
                    btnEditFeederAccount.Enabled = false;
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

        #region Valiadate Closing Criteria
        /// <summary>
        ///Added BY Faisal Shah
        /// 
        /// for all algos corresponding valid secondary sort criteria are following
        /// 
        /// FIFO: AvgPxASC,AvgPxDESC,SamePxAvgPxASC,SamePxAvgPxDESC,OrderSequenceASC,OrderSequenceDESC and None
        /// LIFO: AvgPxASC,AvgPxDESC,SamePxAvgPxASC,SamePxAvgPxDESC,OrderSequenceASC,OrderSequenceDESC and None
        /// MIFO: AvgPxASC,AvgPxDESC,SamePxAvgPxASC,SamePxAvgPxDESC,OrderSequenceASC,OrderSequenceDESC and None
        /// HIFO: None
        /// HIHO: None
        /// LOWCOST: None
        /// ETM: None
        /// BTAX: None
        /// TAXADV: None
        /// ACA: None
        /// </summary>
        /// <param name="algorithm">Selected algorithm for the closing</param>
        ///  <param name="secondarySort">Secondary sort criteria for the closing</param>
        private bool ValiadateClosingCriteria(ref string errorMessage)
        {
            PostTradeEnums.CloseTradeAlogrithm algorithm = PostTradeEnums.CloseTradeAlogrithm.NONE;
            PostTradeEnums.SecondarySortCriteria secondarySort = PostTradeEnums.SecondarySortCriteria.None;
            if (!string.IsNullOrWhiteSpace(cmbClosing.Text.ToString()))
                if ((int)cmbClosing.Value != -1)
                    algorithm = (PostTradeEnums.CloseTradeAlogrithm)Enum.ToObject(typeof(PostTradeEnums.CloseTradeAlogrithm), (int)cmbClosing.Value);
                else
                    algorithm = PostTradeEnums.CloseTradeAlogrithm.NONE;
            if (!string.IsNullOrWhiteSpace(cmbSort.Text.ToString()) && cmbSort.Text.ToString() != "-Select-")
                secondarySort = (PostTradeEnums.SecondarySortCriteria)Enum.ToObject(typeof(PostTradeEnums.SecondarySortCriteria), (int)cmbSort.Value);
            else
                secondarySort = PostTradeEnums.SecondarySortCriteria.None;
            bool isValidate = true;
            try
            {
                switch (algorithm)
                {
                    case PostTradeEnums.CloseTradeAlogrithm.NONE:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                            isValidate = false;

                        break;
                    //for FIFO and LOFO, valid secondary sort criteria are AvgPxASC,AvgPxDESC,OrderSequenceASC,OrderSequenceDESC and None
                    case PostTradeEnums.CloseTradeAlogrithm.FIFO:
                    case PostTradeEnums.CloseTradeAlogrithm.LIFO:

                        if (secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.SamePxAvgPxASC) || secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.SamePxAvgPxDESC))
                        {
                            errorMessage = "For selected algo SamePxAvgPxASC /SamePxAvgPxDESC is not a valid secondary sort criteria.";
                            isValidate = false;
                        }
                        break;

                    //for MFIFO valid secondary sort criteria are AvgPxASC,AvgPxDESC,SamePxAvgPxASC,SamePxAvgPxDESC,OrderSequenceASC,OrderSequenceDESC and None
                    case PostTradeEnums.CloseTradeAlogrithm.MFIFO:

                        break;
                    //for HIFO valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.HIFO:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "For HIFO valid secondary sort criteria is None";
                            isValidate = false;
                        }

                        break;
                    //for PRESET algo secondary sort criteria would be validated during saving secondary sort for an algo.
                    case PostTradeEnums.CloseTradeAlogrithm.PRESET:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "For PRESET valid secondary sort criteria is None";
                            isValidate = false;
                        }
                        break;
                    //for ETM valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.ETM:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "For ETM valid secondary sort criteria is None";
                            isValidate = false;
                        }
                        // isValidate = false;
                        break;
                    //for LOWCOST valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.LOWCOST:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "For LOWCOST valid secondary sort criteria is None";
                            isValidate = false;
                        }
                        break;
                    //for ACA valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.ACA:
                        if (!(secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None) || secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.OrderSequenceASC)))
                        {
                            errorMessage = "For ACA valid secondary sort criteria are OrderSequenceASC and None";
                            isValidate = false;
                        }
                        break;
                    //for HIHO valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.HIHO:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "For HIHO valid secondary sort criteria is None";
                            isValidate = false;
                        }
                        break;
                    //for BTAX valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.BTAX:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "For BTAX valid secondary sort criteria is None";
                            isValidate = false;
                        }
                        break;
                    //for TAXADV valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.TAXADV:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "For TAXADV valid secondary sort criteria is None";
                            isValidate = false;
                        }
                        break;
                    case PostTradeEnums.CloseTradeAlogrithm.MANUAL:
                        //whenever user changes closed quantity from close order UI than closing algorithm will be MANUAL.
                        break;
                    default:
                        break;
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
            return isValidate;
        }
        #endregion

        /// <summary>
        /// Fill the grid with data
        /// </summary>
        public void LoadFeederGridData()
        {
            try
            {
                int accountID = int.MinValue;

                if (grdAccounts.ActiveRow != null)
                {
                    accountID = int.Parse(grdAccounts.ActiveRow.Cells["FundID"].Text);
                }

                grdFeederAccounts.DataSource = AccountSetupManager.GetFeedersForCurrentAccount(accountID);
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
        /// Get the Value list for the feeder accounts 
        /// </summary>
        /// <returns>Value list of the feeder accounts</returns>
        public ValueList GetFeederValueList()
        {
            try
            {
                ValueList vlFeeder = new ValueList();
                Dictionary<int, string> dictFeederIDName = AccountSetupManager.GetFeederIDNames();
                vlFeeder.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int feederID in dictFeederIDName.Keys)
                {
                    vlFeeder.ValueListItems.Add(feederID, dictFeederIDName[feederID]);
                }
                return vlFeeder;
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
        /// Initialize the layout of the Accounts grid for display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdAccounts_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand bndAccount = e.Layout.Bands[0];
                bndAccount.Override.AllowRowFiltering = DefaultableBoolean.True;
                UltraGridColumn colAccountName = bndAccount.Columns["FundName"];
                colAccountName.Header.Caption = "Account Name";

                UltraGridColumn colAccountShortName = bndAccount.Columns["FundShortName"];
                colAccountShortName.Header.Caption = "Account Short Name";

                bndAccount.Columns["FundID"].Hidden = true;

                foreach (UltraGridRow ugRow in grdAccounts.Rows)
                {
                    ugRow.Activation = Activation.NoEdit;
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
        /// Fill the Details of the account in the respective fields after the row is activated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdAccounts_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                if (grdAccounts.Rows.Count > 0)
                {
                    if (grdAccounts.ActiveRow != null && !string.IsNullOrEmpty(grdAccounts.ActiveRow.Cells["FundID"].Text))
                    {
                        //_isRowChanged = true;
                        int accountID = (int)grdAccounts.ActiveRow.Cells["FundID"].Value;
                        GetSelectedAccountID(accountID);

                        // to check if selected account is associated with batch.
                        checkAccountBatchAssociation(accountID);

                        AccountDetails account = AccountSetupManager.GetCurrentAccount(accountID);
                        if (account != null)
                        {
                            cmbCurrency.Value = account.Currency;
                            dtInceptionDate.Value = account.InceptionDate;
                            dtOnboardDate.Value = account.OnBoardDate;
                            cmbClosing.Value = account.ClosingMethodology;
                            if (account.LockDate == SqlDateTime.MinValue.ToString())
                            {
                                dtLockDate.Text = "N/A";
                            }
                            else
                            {
                                dtLockDate.Text = account.LockDate;//.ToShortDateString();
                            }
                            //dtLockDate.AllowDrop = false;
                            cmbLockSchedule.Value = account.LockSchedule;
                            cmbPrimeBroker.Value = account.CompanyPrimeBrokerClearerID;
                            cmbSort.Value = account.SecondarySortCriteria;
                        }
                        LoadFeederGridData();
                    }
                }
                //if (_isDataChanged)
                //{
                //    _isDataChanged = false;
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
        ///Added by Faisal Shah 24/07/14
        ///Purpose To Set Active User as same as was last Modified by User
        /// </summary>
        private void SetActiveRow()
        {
            try
            {
                if (!string.IsNullOrEmpty(_accountName))
                {
                    int index = GetIndexForAccount();
                    if (index >= 0)
                    {
                        grdAccounts.ActiveRow = grdAccounts.Rows[index];
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
        /// <summary>
        ///  //Added by Faisal Shah 24/07/14
        ///Purpose To Get Index on the basis of Accountname
        /// </summary>
        /// <returns></returns>
        private int GetIndexForAccount()
        {
            try
            {
                foreach (UltraGridRow dRow in grdAccounts.Rows)
                {
                    if (dRow.Cells["FundName"].Text.ToString() == _accountName)
                    {
                        return dRow.Index;
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
            return -1;
        }

        /// <summary>
        /// To pass selected accountID in Audit trail
        /// </summary>
        /// <param name="accountID"></param>
        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void GetSelectedAccountID(int accountID) { }

        /// <summary>
        /// To show prompt for prime broker updation, if account is associated with batch.
        /// </summary>
        /// <param name="accountID"></param>
        public void checkAccountBatchAssociation(int accountID)
        {
            bool isAccountBatchAssociated = false;
            try
            {
                isAccountBatchAssociated = AccountSetupManager.isAccountAssociatedWithBatch(accountID);
                if (isAccountBatchAssociated)
                {
                    cmbPrimeBroker.ReadOnly = true;
                }
                else
                {
                    cmbPrimeBroker.ReadOnly = false;
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
        /// Button to Edit the account details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_isSaveRequired)
                {
                    _isSaveRequired = true;
                }
                if (grdAccounts.Rows.Count > 0 && grdAccounts.ActiveRow != null)
                {
                    foreach (UltraGridRow ugRow in grdAccounts.Rows)
                        ugRow.Activation = Activation.NoEdit;
                    grdAccounts.ActiveRow.Activation = Activation.AllowEdit;
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
        /// Button to add the new account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                GetSelectedAccountID(int.MinValue);
                cmbPrimeBroker.ReadOnly = false;
                if (!ValidateData())
                {
                    return;
                }

                UltraGridRow row = grdAccounts.DisplayLayout.Bands[0].AddNew();
                row.Cells["IsActive"].Value = false;
                foreach (UltraGridRow ugRow in grdAccounts.Rows)
                {
                    ugRow.Activation = Activation.NoEdit;
                }
                grdAccounts.ActiveRow.Activation = Activation.AllowEdit;
                ResetAccountDetailControls();

                cmbCurrency.Value = _cmpyBaseCurrencyID;

                int count = grdFeederAccounts.Rows.Count - 1;
                for (int i = count; i >= 0; i--)
                {
                    grdFeederAccounts.Rows[i].Delete(false);
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
        /// reset the controls thatg give the details of the account to their default values 
        /// </summary>
        private void ResetAccountDetailControls()
        {
            cmbCurrency.Value = -1;
            dtInceptionDate.Value = DateTime.UtcNow;
            dtOnboardDate.Value = DateTime.UtcNow;
            cmbClosing.Value = -1;
            dtLockDate.Text = "";
            cmbPrimeBroker.Value = -1;
            cmbLockSchedule.Value = -1;
            cmbSort.Value = -1;
        }

        /// <summary>
        /// Button to delete the existing account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!_isSaveRequired)
                //{
                //    _isSaveRequired = true;
                //}
                if (!_isDataChanged)
                {
                    _isSaveRequired = true;
                    _isDataChanged = true;
                }
                if (grdAccounts.Rows.Count > 0 && grdAccounts.ActiveRow != null)
                {
                    if (!string.IsNullOrEmpty(grdAccounts.ActiveRow.Cells["FundID"].Text) && AccountSetupManager.dictAccounts.ContainsKey(Convert.ToInt32(grdAccounts.ActiveRow.Cells["FundID"].Text)))
                    {
                        if (grdFeederAccounts.Rows.Count != 0)
                        {
                            MessageBox.Show("The Account has associated Feeder accounts. Unmap the feeder accounts first", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        // Added by Bhavana to check account association
                        string hasAccountAssociation = AccountSetupManager.GetAccountAssociation(Convert.ToInt32(grdAccounts.ActiveRow.Cells["FundID"].Text));
                        if (hasAccountAssociation != string.Empty)
                        {
                            MessageBox.Show(hasAccountAssociation, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        DateTime dateValue;
                        String lockDate = String.Empty;
                        Dictionary<int, string> dictLocks = NAVLockManager.GetLockDatesForAccounts();
                        if (dictLocks.ContainsKey(Convert.ToInt32(grdAccounts.ActiveRow.Cells["FundID"].Text)))
                            lockDate = dictLocks[Convert.ToInt32(grdAccounts.ActiveRow.Cells["FundID"].Text)];
                        if (DateTime.TryParse(lockDate, out dateValue))
                        {
                            MessageBox.Show("Account cannot be deleted as it has NAV lock(s). Please release all lock(s) for account first.", "Account Setup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        DialogResult drDelete = MessageBox.Show("Do you want to delete the selected Account?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (drDelete == DialogResult.No)
                        {
                            return;
                        }
                        int accountID = int.Parse(grdAccounts.ActiveRow.Cells["FundID"].Text);
                        if (AccountSetupManager.mappedAccount.Contains(accountID))
                        {
                            MessageBox.Show("The account is associated with other master funds. Cannot be deleted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        //else 
                        //if (grdFeederAccounts.Rows.Count != 0)
                        //{
                        //    DialogResult dr = MessageBox.Show("The Account has associated Feeder accounts. Unmap the feeder accounts first", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        //    if (dr == DialogResult.No)
                        //    {
                        //        return;
                        //    }
                        //}
                        AccountSetupManager.dictAccounts[accountID].modifiedType = AccountDetails.AccountModifiedType.Deleted;
                    }
                    else if ((grdAccounts.ActiveRow.Cells["FundID"].Text) != "")
                    {
                        if (Convert.ToInt32(grdAccounts.ActiveRow.Cells["FundID"].Text) < 0)
                        {
                            int accountKey = int.MinValue;
                            foreach (KeyValuePair<int, AccountDetails> kvp in AccountSetupManager.dictAccounts)
                            {
                                if (kvp.Value.AccountShortName == grdAccounts.ActiveRow.Cells["FundShortName"].Text
                                    && kvp.Value.CompanyAccountID == int.MinValue)
                                {
                                    accountKey = kvp.Key;
                                }
                            }
                            if (accountKey != int.MinValue)
                            {
                                AccountSetupManager.dictAccounts.Remove(accountKey);
                            }
                        }
                    }
                    grdAccounts.ActiveRow.Delete(false);
                    ResetAccountDetailControls();
                    grdFeederAccounts.Selected.Rows.AddRange((UltraGridRow[])grdFeederAccounts.Rows.All);
                    grdFeederAccounts.DeleteSelectedRows(false);
                }
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
        /// Button to map the new account with currently selected account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFeederAccount_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!_isSaveRequired)
                //{
                //    _isSaveRequired = true;
                //}
                grdFeederAccounts.DisplayLayout.Bands[0].AddNew();
                //Modified By faisal Shah 07/08/14
                //Making all Columns Editable except ShortName in the Feeder Account Grid.
                grdFeederAccounts.ActiveRow.Activation = Activation.AllowEdit;
                foreach (UltraGridColumn gridCell in grdFeederAccounts.DisplayLayout.Bands[0].Columns)
                {
                    if (!gridCell.Key.Equals("FeederAccountShortName"))
                    {
                        gridCell.CellActivation = Activation.AllowEdit;
                    }

                }
                grdFeederAccounts.ActiveRow.Cells["FeederAccountID"].Value = int.MinValue;
                //grdFeederAccounts.ActiveRow.Cells["AllocatedAmount"].Value = Convert.ToDecimal("0.00");
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
        /// button to edit the mapped feeder details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditFeederAccount_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_isSaveRequired)
                {
                    _isSaveRequired = true;
                }
                if (grdFeederAccounts.Rows.Count > 0 && grdAccounts.ActiveRow != null)
                {
                    //Modified By faisal Shah 07/08/14
                    //Making all Columns Editable except ShortName in the Feeder Account Grid.
                    foreach (UltraGridColumn gridCell in grdFeederAccounts.DisplayLayout.Bands[0].Columns)
                    {
                        if (!gridCell.Key.Equals("FeederAccountShortName"))
                        {
                            gridCell.CellActivation = Activation.AllowEdit;
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

        /// <summary>
        /// button to delete the currently selected mapped account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteFeederAccount_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_isSaveRequired)
                {
                    _isSaveRequired = true;
                }
                if (grdFeederAccounts.Rows.Count > 0 && grdFeederAccounts.ActiveRow != null)
                {
                    if (!grdFeederAccounts.ActiveRow.Cells["FeederAccountID"].Text.Equals("-Select-"))
                    {
                        DialogResult dr = MessageBox.Show("Do you want to unmap the feeder?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                        {
                            return;
                        }
                    }
                    grdFeederAccounts.ActiveRow.Delete(false);
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
        /// Check if grid has empty rows or empty cells
        /// </summary>
        /// <returns>True if there are empty cells</returns>
        public bool HasEmpty()
        {
            try
            {
                for (int i = grdAccounts.Rows.Count - 1; i >= 0; i--)
                {
                    UltraGridRow grdRow = grdAccounts.Rows[i];
                    if (grdRow.Cells["FundName"].Text == "" && grdRow.Cells["FundShortName"].Text == "")
                    {
                        grdRow.Delete(false);
                    }
                    if (grdRow.Cells["FundName"].Text == "" || grdRow.Cells["FundShortName"].Text == "")
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
        }

        /// <summary>
        /// Save the account details in the Database
        /// </summary>
        /// <returns>Number of affected rows in the db</returns>
        private int SaveAccountSetupDetails()
        {
            try
            {
                if (HasEmpty())
                {
                    MessageBox.Show("Blank Accounts cannot be inserted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                //Modified By faisal Shah 07/08/14
                //Giving User a prompt for zero Amount Feeders and saving data on users response.
                string duplicateAccount = string.Empty;
                bool isProceedToSave = true;

                foreach (UltraGridRow row in grdFeederAccounts.Rows)
                {
                    if (Convert.ToDouble(row.Cells["AllocatedAmount"].Text.ToString()) == 0.0)
                    {
                        DialogResult dlgResult = new DialogResult();
                        dlgResult = ConfirmationMessageBox.DisplayYesNo("Feeder Accounts mapping with amount zero cannot be saved.\nDo you want to continue?", "Alert");
                        if (dlgResult == DialogResult.No)
                        {
                            isProceedToSave = false;
                            _isMappingChanged = false;
                        }
                        break;
                    }

                }

                if (isProceedToSave)
                {
                    duplicateAccount = AccountSetupManager.SaveAccountSetup(_companyID);

                    if (!string.IsNullOrEmpty(duplicateAccount) && !(duplicateAccount.Equals("NotFound")) && !(duplicateAccount.Equals("Failure")))
                    {
                        //purpose : for showing the accountname and companyname on messagebox CHMW-2703
                        MessageBox.Show(duplicateAccount.ToString(), "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                    else if (duplicateAccount.Equals("Failure"))
                    {
                        MessageBox.Show("Account already exists please change name and short name", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Failure"))
                {
                    MessageBox.Show("Problem in saving account details.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                else if (ex.Message.Contains("The DELETE statement conflicted"))
                {
                    MessageBox.Show("Some of removed accounts that are associated with other entities cannot be deleted. ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Problem in saving account details.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                //if (rethrow)
                //{
                //    throw;
                //}
            }
            return 0;
        }

        /// <summary>
        /// Initialize the layout of the accounts grid to display the details of the accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdFeederAccounts_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                ValueList vlFeederIDName = GetFeederValueList();

                UltraGridBand band = e.Layout.Bands[0];
                //band.PerformAutoResizeColumns(false, PerformAutoSizeType.VisibleRows);
                band.Override.AllowRowFiltering = DefaultableBoolean.True;
                UltraGridColumn fAccountNameCol = band.Columns["FeederAccountID"];
                fAccountNameCol.Header.Caption = "Name";
                fAccountNameCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                fAccountNameCol.ValueList = vlFeederIDName;
                fAccountNameCol.CellActivation = Activation.NoEdit;

                UltraGridColumn fAccountShortNameCol = band.Columns["FeederAccountShortName"];
                fAccountShortNameCol.Header.Caption = "Short Name";
                fAccountShortNameCol.CellActivation = Activation.NoEdit;

                UltraGridColumn fAccountAllocatedAmountCol = band.Columns["AllocatedAmount"];
                fAccountAllocatedAmountCol.Header.Caption = "Amount";
                fAccountAllocatedAmountCol.NullText = "-Fill Amount-";
                fAccountAllocatedAmountCol.CellActivation = Activation.NoEdit;

                UltraGridColumn CurrencyIdCol = band.Columns["CurrencyID"];
                CurrencyIdCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                CurrencyIdCol.ValueList = _currencyList;
                CurrencyIdCol.Header.Caption = "Currency";
                CurrencyIdCol.CellActivation = Activation.NoEdit;
                //CurrencyIdCol.NullText = "-Select-";
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
        /// Update the details of the account before the row deactivates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdAccounts_BeforeRowDeactivate(object sender, CancelEventArgs e)
        {
            //if (grdAccounts.ActiveRow.Cells["AccountName"].Text == "" || grdAccounts.ActiveRow.Cells["AccountShortName"].Text == "")
            //{
            //    MessageBox.Show("Provide the Name and the short name for the account.","Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}           
            if (!UpdateAccountDetails())
            {
                e.Cancel = true;
                grdAccounts.ActiveRow = ((UltraGrid)sender).ActiveRow;
            }
            else
            {
                ResetAccountDetailControls();
            }
        }

        /// <summary>
        /// Update the details of the currently selected account
        /// </summary>
        private bool UpdateAccountDetails()
        {
            try
            {
                if (grdAccounts.ActiveRow == null)
                {
                    return true;
                }
                //if (!_isRowChanged && !_isUpdateRequired)
                //{
                //    return true;
                //}
                if (!ValidateData())
                {
                    return false;
                }
                //Modified by Faisal Shah
                //Dated 03/07/14
                // Assigning Temp Account ID and adding the same to Feeder Accounts
                //if (grdAccounts.ActiveRow != null && string.IsNullOrEmpty(grdAccounts.ActiveRow.Cells["AccountID"].Text))
                //{
                //    grdAccounts.ActiveRow.Cells["AccountID"].Value = _tempAccountID;
                //    _tempAccountID += 1;
                //}
                else if (!_isSaveRequired)
                {
                    return false;
                }
                AccountDetails account = new AccountDetails();

                if (!String.IsNullOrEmpty(grdAccounts.ActiveRow.Cells["FundID"].Text.ToString()))
                {
                    accountID = int.Parse(grdAccounts.ActiveRow.Cells["FundID"].Text);
                }
                else
                {
                    accountID = tempAccountID;
                    tempAccountID--;
                }

                account.AccountID = accountID;
                if (grdAccounts.ActiveRow != null && string.IsNullOrEmpty(grdAccounts.ActiveRow.Cells["FundID"].Text))
                {
                    grdAccounts.ActiveRow.Cells["FundID"].Value = accountID;
                }
                account.AccountName = grdAccounts.ActiveRow.Cells["FundName"].Text.Trim();
                account.AccountShortName = grdAccounts.ActiveRow.Cells["FundShortName"].Text.Trim();
                account.CompanyID = _companyID;
                account.Currency = Convert.ToInt32(cmbCurrency.Value);
                account.InceptionDate = (DateTime)dtInceptionDate.Value;
                account.OnBoardDate = (DateTime)dtOnboardDate.Value;
                account.ClosingMethodology = Convert.ToInt32(cmbClosing.Value);
                account.IsSwapAccount = swapAccountDropDown.GetSelectedItemsInDictionary().ContainsKey(account.AccountID);
                account.IsActive = Convert.ToBoolean(grdAccounts.ActiveRow.Cells["IsActive"].Text.Trim());
                if (string.IsNullOrEmpty(dtLockDate.Text) || dtLockDate.Text == "N/A")
                {
                    account.LockDate = SqlDateTime.MinValue.ToString(); //(DateTime)dtLockDate.Text;
                }
                else
                {
                    account.LockDate = dtLockDate.Text;
                }
                account.CompanyPrimeBrokerClearerID = Convert.ToInt32(cmbPrimeBroker.Value);
                account.SecondarySortCriteria = Convert.ToInt32(cmbSort.Value);
                account.LockSchedule = Convert.ToInt32(cmbLockSchedule.Value);
                //if (!_isDataChanged)
                //{
                DataTable dtFeeder = (DataTable)grdFeederAccounts.DataSource;
                _isDataChanged = AccountSetupManager.UpdateAccount(account, dtFeeder);
                //}
                //_isUpdateRequired = false;
                if (accountID > 0)
                {
                    AccountSetupManager.SetMappedFeeder(accountID, dtFeeder);
                }
                if (!_isMappingChanged)
                {
                    _isMappingChanged = AccountSetupManager._isMappingChanged;
                }
                return true;
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
        }

        /// <summary>
        /// Check if the details for the account are valid
        /// </summary>
        /// <returns>True If the data is valid</returns>
        public bool ValidateData()
        {
            try
            {
                if (grdAccounts.ActiveRow != null)
                {
                    errorProvider1.SetError(cmbCurrency, "");
                    errorProvider1.SetError(dtInceptionDate, "");
                    errorProvider1.SetError(dtOnboardDate, "");
                    errorProvider1.SetError(cmbClosing, "");
                    errorProvider1.SetError(cmbSort, "");
                    errorProvider1.SetError(cmbPrimeBroker, "");

                    if (string.IsNullOrEmpty(grdAccounts.ActiveRow.Cells["FundName"].Text.Trim()) || string.IsNullOrEmpty(grdAccounts.ActiveRow.Cells["FundShortName"].Text.Trim()))
                    {
                        MessageBox.Show("Provide the name and the short name for the account.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _isInvalidData = true;
                        return false;
                    }
                    else if (grdAccounts.ActiveRow.Cells["FundName"].Text.Length > 100)
                    {
                        MessageBox.Show("Account Name can contain maximum 100 characters.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _isInvalidData = true;
                        return false;
                    }
                    else if (grdAccounts.ActiveRow.Cells["FundShortName"].Text.Length > 100)
                    {
                        MessageBox.Show("Account Short Name can contain maximum 100 characters.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _isInvalidData = true;
                        return false;
                    }
                    else if (string.IsNullOrEmpty(cmbCurrency.Text.ToString()) || (!string.IsNullOrEmpty(cmbCurrency.Value.ToString()) && int.Parse(cmbCurrency.Value.ToString()) == -1))
                    {
                        errorProvider1.SetError(cmbCurrency, "Choose a currency for the account");
                        cmbCurrency.Focus();
                        _isInvalidData = true;
                        return false;
                    }
                    else if (dtInceptionDate.Value == null)
                    {
                        errorProvider1.SetError(dtInceptionDate, "Choose an inception date for the account");
                        dtInceptionDate.Focus();
                        _isInvalidData = true;
                        return false;
                    }
                    else if (dtOnboardDate.Value == null)
                    {
                        errorProvider1.SetError(dtOnboardDate, "Choose an onboard date for the account");
                        _isInvalidData = true;
                        dtOnboardDate.Focus();
                        return false;
                    }
                    else if (Convert.ToDateTime(dtInceptionDate.Value).Date > Convert.ToDateTime(dtOnboardDate.Value).Date)
                    {
                        errorProvider1.SetError(dtOnboardDate, "Onboard date should be greater or equal to inception date for the account");
                        _isInvalidData = true;
                        dtOnboardDate.Focus();
                        return false;
                    }
                    else if (string.IsNullOrEmpty(cmbClosing.Text.ToString()) || (!string.IsNullOrEmpty(cmbClosing.Value.ToString()) && int.Parse(cmbClosing.Value.ToString()) == -1))
                    {
                        errorProvider1.SetError(cmbClosing, "Choose a closing methodology for the account");
                        _isInvalidData = true;
                        cmbClosing.Focus();
                        return false;
                    }
                    else if (!string.IsNullOrEmpty(cmbClosing.Value.ToString()) && cmbClosing.Text.ToString().Equals("NONE", StringComparison.InvariantCultureIgnoreCase))
                    {
                        errorProvider1.SetError(cmbClosing, "Closing methodology cannot be NONE for the account");
                        _isInvalidData = true;
                        cmbClosing.Focus();
                        return false;
                    }
                    else if (string.IsNullOrEmpty(cmbSort.Text.ToString()) || (!string.IsNullOrEmpty(cmbSort.Value.ToString()) && int.Parse(cmbSort.Value.ToString()) == -1))
                    {
                        errorProvider1.SetError(cmbSort, "Choose a secondary sort criteria for the account");
                        _isInvalidData = true;
                        cmbSort.Focus();
                        return false;
                    }
                    else if (string.IsNullOrEmpty(cmbPrimeBroker.Text.ToString()) || (!string.IsNullOrEmpty(cmbPrimeBroker.Value.ToString()) && int.Parse(cmbPrimeBroker.Value.ToString()) == -1))
                    {
                        errorProvider1.SetError(cmbPrimeBroker, "Choose a prime broker for the account");
                        _isInvalidData = true;
                        cmbPrimeBroker.Focus();
                        return false;
                    }
                    _isInvalidData = false;
                    return true;
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
            return true;
        }
        /// <summary>
        /// Fill the feeder details when the feeder is selected from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdFeederAccounts_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (!_isSaveRequired)
                {
                    _isSaveRequired = true;
                }
                if (e.Cell.Column.Header.Caption == "Name")
                {
                    UltraGridColumn gridColumn = e.Cell.Column;
                    String ColumnText = e.Cell.Text;
                    EmbeddableEditorBase editor = e.Cell.EditorResolved;
                    object changedValue = editor.IsValid ? editor.Value : editor.CurrentEditText;
                    //int feederID = Convert.ToInt32(changedValue);
                    UltraGridRow actRow = grdFeederAccounts.ActiveRow;
                    if (ColumnText.Equals("-Select-"))
                    {
                        //actRow.Cells["FeederAccountShortName"].Value="";
                        //actRow.Cells["AllocatedAmount"].Value = Convert.ToDecimal("0.00");
                        e.Cell.CancelUpdate();
                        return;
                    }

                    //int i = (int)actRow.Cells["FormatName"].Value;
                    int i = Convert.ToInt32(changedValue);
                    foreach (UltraGridRow ugRow in grdFeederAccounts.Rows)
                    {
                        if (ugRow != actRow)
                        {
                            if (Convert.ToInt32(ugRow.Cells["FeederAccountID"].Value) == i)
                            {
                                MessageBox.Show("The feeder is already mapped with this account.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                e.Cell.CancelUpdate();
                                return;
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

            try
            {
                if (e.Cell.Column.Header.Caption == "Name")
                {
                    UltraGridColumn gridColumn = e.Cell.Column;
                    String ColumnText = e.Cell.Text;
                    if (ColumnText.Equals("-Select-"))
                    {
                        return;
                    }
                    EmbeddableEditorBase editor = e.Cell.EditorResolved;
                    object changedValue = editor.IsValid ? editor.Value : editor.CurrentEditText;
                    int feederID = Convert.ToInt32(changedValue);
                    FeederAccountItem feederAccount = AccountFeederAccountMappingManager.GetSingleFeeder(feederID);
                    UltraGridRow ugRow = grdFeederAccounts.ActiveRow;
                    ugRow.Cells["FeederAccountID"].Value = feederAccount.FeederAccountID;
                    ugRow.Cells["FeederAccountShortName"].Value = feederAccount.FeederShortName;
                    ugRow.Cells["AllocatedAmount"].Value = Convert.ToDecimal("0.00");
                    ugRow.Cells["CurrencyID"].Value = feederAccount.FeederCurrency;
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
        /// Save the Details to the Database
        /// </summary>
        public int SaveDetails()
        {
            bool isUpdated = false;
            //bool isSaved = false;
            try
            {
                if (!_isSaveRequired)
                {
                    return 0;
                }
                isUpdated = UpdateAccountDetails();
                if (!isUpdated)
                {
                    return 0;
                }
                if (grdAccounts.ActiveRow != null)
                {
                    _accountName = grdAccounts.ActiveRow.Cells["FundName"].Text.ToString();
                }
                int i = SaveAccountSetupDetails();
                if (i > 0)
                {
                    _isSaveRequired = false;
                    _isDataChanged = false;
                    _isMappingChanged = false;
                }
                else
                {
                    _isInvalidData = true;
                    return i;
                }


                if (isUpdated == true)
                {
                    DataSet dsAccount = AccountSetupManager.CreateAccountDataSet();
                    if (dsAccount.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsAccount.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(dr["ModifiedType"].ToString()) == 1)
                                AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditAccountData(_companyID, dr), AuditManager.Definitions.Enum.AuditAction.AccountCreated);
                            else if (Convert.ToInt32(dr["ModifiedType"].ToString()) == 2)
                                AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditAccountData(_companyID, dr), AuditManager.Definitions.Enum.AuditAction.AccountDeleted);
                            else if (Convert.ToInt32(dr["ModifiedType"].ToString()) == 3)
                                AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditAccountData(_companyID, dr), AuditManager.Definitions.Enum.AuditAction.AccountUpdated);
                            // For deleted item Modified Type = 2
                        }
                    }
                }
                return i;
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
        /// Function to approve accounts changes
        /// </summary>
        /// <param name="sender">button approve</param>
        /// <param name="e">e</param>
        public int uBtnApprove_Click()
        {
            int i = 0;
            try
            {
                if (_companyID != int.MinValue)
                {
                    if (grdAccounts.Rows.Count > 0 && grdAccounts.ActiveRow != null)
                    {
                        if (!String.IsNullOrEmpty(grdAccounts.ActiveRow.Cells["FundID"].Text))
                        {
                            int accountID = int.Parse(grdAccounts.ActiveRow.Cells["FundID"].Text);
                            DataSet dsAccount = AccountSetupManager.CreateDataSetForSelectedAccount(accountID);  // should be modified using Linq
                            if (dsAccount.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr in dsAccount.Tables[0].Rows)
                                {
                                    AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditAccountData(_companyID, dr), AuditManager.Definitions.Enum.AuditAction.AccountApproved);
                                    GetSelectedAccountID(accountID);
                                }
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
            return i;
        }

        /// <summary>
        /// Prepare the data for saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdAccounts_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                PrepareForSave();
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
        /// Prepare the data for saving
        /// </summary>
        private void PrepareForSave()
        {
            try
            {
                if (!_isSaveRequired)// && !_isDataChanged)
                {
                    _isSaveRequired = true;
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
        /// handle the value changed event of the comboboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCurrency_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                UltraCombo cmb = sender as UltraCombo;
                if (!cmb.Text.Equals("-Select-"))
                {
                    errorProvider1.SetError(cmb, "");
                    //_isUpdateRequired = true;
                }
                PrepareForSave();
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
        /// Handle the value change event of the date fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtInceptionDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //_isUpdateRequired = true;
                PrepareForSave();
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
        /// Function to Get dictionary for details of Accounts
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        private Dictionary<String, List<String>> AuditAccountData(int _companyID, DataRow dsAccount)
        {
            Dictionary<String, List<String>> auditDataForAccount = new Dictionary<string, List<string>>();
            try
            {
                auditDataForAccount.Add(CustomAuditSourceConstants.AuditSourceTypeAccount, new List<string>());
                auditDataForAccount[CustomAuditSourceConstants.AuditSourceTypeAccount].Add(_companyID.ToString());
                auditDataForAccount[CustomAuditSourceConstants.AuditSourceTypeAccount].Add(dsAccount[0].ToString());
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

            return auditDataForAccount;
        }

        /// <summary>
        /// Update the details of the account if user changes the tab before changing the row in the grid 
        /// </summary>
        public void UpdateAccountBeforeTabChange()
        {
            try
            {
                UpdateAccountDetails();
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
        /// Update the lock dates when the account setup tab is selected
        /// </summary>
        public void UpdateLockDateAfterAccountTabSelected()
        {
            try
            {
                AccountSetupManager.UpdateAccountLockDates();
                if (grdAccounts.ActiveRow != null && !string.IsNullOrEmpty(grdAccounts.ActiveRow.Cells["FundID"].Value.ToString()))
                {
                    int accountID = Convert.ToInt32(grdAccounts.ActiveRow.Cells["FundID"].Value.ToString());
                    dtLockDate.Text = AccountSetupManager.GetCurrentAccountLockDate(accountID);
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
        /// Added By Faisal Shah
        /// Need to check the Secondary Sort Criteria as per the Closing Algo Selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSort_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                string errorMessage = string.Empty;
                if (ValiadateClosingCriteria(ref errorMessage))
                {
                    PrepareForSave();
                }
                else
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        MessageBox.Show(errorMessage, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    cmbSort.Value = -1;// PostTradeEnums.SecondarySortCriteria.None.ToString();
                    return;
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
            //_isUpdateRequired = true;

        }

        /// <summary>
        /// check if the data is to be saved
        /// </summary>
        public bool SaveBeforeClose()
        {
            bool r = false;
            try
            {
                UpdateAccountDetails();
                // Modified by Ankit Gupta on 8th Oct, 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1550
                if (this._isSaveRequired && (this._isDataChanged || this._isMappingChanged))
                {
                    DialogResult dr = MessageBox.Show("Account details Not Saved. Save the details now?", "Alert", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        this.SaveDetails();
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        r = true;

                    }
                    else if (dr == DialogResult.No)
                    {
                        _isSaveRequired = false;
                        _isDataChanged = false;
                        _isMappingChanged = false;
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
            return r;

        }

        /// <summary>
        /// to show user if account has association with batch.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPrimeBroker_MouseHover(object sender, EventArgs e)
        {
            try
            {
                //tipInfo.ToolTipTextStyle = ToolTipTextStyle.Formatted;
                //ultraToolTipManager1.DisplayStyle = ToolTipDisplayStyle.Office2007;
                if (cmbPrimeBroker.ReadOnly == true)
                {
                    ultraToolTipManager1.SetUltraToolTip(cmbPrimeBroker, tipInfo);
                    tipInfo.ToolTipTextFormatted = "Prime broker can not be updated since account has association with batch.";
                    ultraToolTipManager1.ShowToolTip(cmbPrimeBroker, true);
                }
                // Modified by Ankit Gupta on 20 Oct, 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1641
                else
                {
                    tipInfo.ToolTipTextFormatted = cmbPrimeBroker.Text;
                    ultraToolTipManager1.SetUltraToolTip(cmbPrimeBroker, tipInfo);
                    ultraToolTipManager1.ShowToolTip(cmbPrimeBroker, true);
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
        /// Display tool tip for each row of ultra combo.
        /// Modified by Ankit Gupta on 20 Oct, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1641
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPrimeBroker_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                e.Row.ToolTipText = e.Row.Cells["ThirdPartyName"].Text;
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
        /// Handles the CheckedChanged event of the allocationCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void allocationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                swapAccountDropDown.Enabled = allocationCheckBox.Checked;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the send allocations via fix status.
        /// </summary>
        /// <returns></returns>
        public bool GetSendAllocationsViaFixStatus()
        {
            bool isSendAllocations = false;
            try
            {
                isSendAllocations = allocationCheckBox.Checked;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isSendAllocations;
        }

        /// <summary>
        /// Handles the CheckStateChanged event of the swapAccountDropDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ItemCheckEventArgs"/> instance containing the event data.</param>
        private void swapAccountDropDown_CheckStateChanged(object sender, ItemCheckEventArgs e)
        {
            try
            {
                AccountDetails account = new AccountDetails();
                account.AccountID = swapAccountDropDown.GetKeyValueFromIndex(e.Index);
                AccountDetails existingAccount = AccountSetupManager.GetCurrentAccount(account.AccountID);
                account.AccountName = existingAccount.AccountName;
                account.AccountShortName = existingAccount.AccountShortName;
                account.CompanyID = _companyID;
                account.Currency = existingAccount.Currency;
                account.InceptionDate = existingAccount.InceptionDate;
                account.OnBoardDate = existingAccount.OnBoardDate;
                account.ClosingMethodology = existingAccount.ClosingMethodology;
                account.LockDate = existingAccount.LockDate;
                account.CompanyPrimeBrokerClearerID = existingAccount.CompanyPrimeBrokerClearerID;
                account.SecondarySortCriteria = existingAccount.SecondarySortCriteria;
                account.LockSchedule = existingAccount.LockSchedule;
                account.IsSwapAccount = e.NewValue == CheckState.Checked;
                account.IsActive = existingAccount.IsActive;
                DataTable dtFeeder = (DataTable)grdFeederAccounts.DataSource;
                _isDataChanged = AccountSetupManager.UpdateAccount(account, dtFeeder);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}
