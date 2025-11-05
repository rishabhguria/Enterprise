using Infragistics.Win.UltraWinListView;
using Prana.Admin.BLL;
using Prana.AuditManager.Definitions.Constants;
using Prana.AuditManager.Definitions.Interface;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    /// <summary>
    /// Summary description for AccountGroup.
    /// </summary>
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.AccountGroupCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.AccountGroupUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.AccountGroupDeleted, ShowAuditUI = true)]
    public partial class AccountGroupSetup : UserControl, IAuditSource
    {
        #region Declaration

        /// <summary>
        /// ID of selected group
        /// </summary>
        public int _groupID = int.MinValue;
        /// <summary>
        /// ID of client
        /// </summary>
        public int _clientID = int.MinValue;

        #endregion

        [AuditManager.Attributes.AuditSourceConstAttri]
        public AccountGroupSetup()
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
                this.UBtnUnSelectAvailableClients.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.UBtnAllUnSelectAvailableClients.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.uBtnUnSelectGroupToClients.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.uBtnAllUnSelectGroupToClients.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));

                this.UBtnUnSelectAvailableAccounts.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.uBtnAllUnSelectAvailableAccounts.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.uBtnUnSelectGroupToAccounts.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.uBtnAllUnSelectGroupToAccounts.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));

                this.grpGroupSetup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                this.grpGroupSetup.ForeColor = System.Drawing.Color.White;

                this.grpAccountsGroups.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.grpAccountsGroups.ForeColor = System.Drawing.Color.White;

                this.grpClientsGroups.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.grpClientsGroups.ForeColor = System.Drawing.Color.White;

                this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
                this.groupBox3.ForeColor = System.Drawing.Color.White;

                this.groupBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
                this.groupBox6.ForeColor = System.Drawing.Color.White;

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

        #region Functions

        /// <summary>
        /// initialise data on page load
        /// </summary>
        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void InitializeControl(int groupID, String GroupName)
        {
            try
            {
                txtGroupName.Text = GroupName;
                _groupID = groupID;
                //List<string> mappedClients = ulstClientToGroup.Items.Cast<ListViewItem>()
                //                            .Select(item => item.Text)
                //                            .ToList();

                AccountGroupSetupManager.InitialiseData(groupID);
                BindUnMappedAccounts();
                BindUnMappedClients();
                if (_groupID != int.MinValue)
                {
                    BindMappedAccounts(AccountGroupSetupManager.GetAccountNamesForGroup(_groupID));
                }
                else
                {
                    BindMappedAccounts(null);
                }
                BindMappedClients(AccountGroupSetupManager.GetClientNamesForGroup(_groupID));
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
        /// Show unmapped accounts in ultra list view 
        /// </summary>
        public void BindUnMappedAccounts()
        {

            try
            {
                List<String> unMappedAccountNames = new List<string>();
                //if (_groupID != int.MinValue)
                //{
                unMappedAccountNames = AccountGroupSetupManager.GetUnmappedAccounts(_groupID);
                ulstAvailableAccounts.Items.Clear();
                foreach (String accountName in unMappedAccountNames)
                {
                    UltraListViewItem item = new UltraListViewItem();
                    item.Tag = item.Key = accountName;
                    item.Value = accountName;
                    ulstAvailableAccounts.Items.Add(item);
                }
                // }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Show mapped accounts in ultra list view 
        /// </summary>
        /// <param name="accountNames">List of mappedAccount Names for selected group </param>
        public void BindMappedAccounts(List<String> accountNames)
        {
            try
            {
                if (accountNames == null)
                {
                    //ulstAccountsToGroup.Items.Clear();
                }
                else
                {

                    ulstAccountsToGroup.Items.Clear();
                    foreach (String accountName in accountNames)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Tag = item.Key = accountName;
                        item.Value = accountName;
                        ulstAccountsToGroup.Items.Add(item);
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
        /// Show only Searched item in Available account view for a seleccted group
        /// </summary>
        public void AddSearchedItemAvailableAccounts()
        {
            try
            {
                //if (_groupID != int.MinValue)
                //{
                List<String> searchingList = AccountGroupSetupManager.GetUnmappedAccounts(_groupID);
                List<String> result = AccountGroupSetupManager.SerachForKeyword(uTxtSearchAvailableAccounts.Text, searchingList);
                ulstAvailableAccounts.Items.Clear();
                if (result.Count > 0)
                {
                    foreach (String foundItem in result)
                    {
                        UltraListViewItem item = new UltraListViewItem(foundItem);
                        item.Key = foundItem;
                        item.Tag = foundItem;
                        item.Value = foundItem;
                        ulstAvailableAccounts.Items.Add(item);
                    }
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
        /// Show only Searched item in Mapped accounts view for a selected group
        /// </summary>
        public void AddSearchedItemMappedAccounts()
        {
            try
            {
                //if (_groupID != int.MinValue)
                //{
                List<String> searchingList = AccountGroupSetupManager.GetAccountNamesForGroup(_groupID);
                List<String> result = AccountGroupSetupManager.SerachForKeyword(uTxtSearchMappedAccounts.Text, searchingList);
                ulstAccountsToGroup.Items.Clear();
                if (result.Count > 0)
                {
                    foreach (String foundItem in result)
                    {
                        UltraListViewItem item = new UltraListViewItem(foundItem);
                        item.Key = foundItem;
                        item.Tag = foundItem;
                        item.Value = foundItem;
                        ulstAccountsToGroup.Items.Add(item);
                    }
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
        /// Show only Searched item in Available clients view for a seleccted group
        /// </summary>
        public void AddSearchedItemAvailableClients()
        {
            try
            {
                //if (_groupID != int.MinValue)
                //{
                List<String> searchingList = AccountGroupSetupManager.GetUnmappedClients(_groupID);
                List<String> result = AccountGroupSetupManager.SerachForKeyword(uTxtSearchAvailableClients.Text, searchingList);
                ulstAvailableClients.Items.Clear();
                if (result.Count > 0)
                {
                    foreach (String foundItem in result)
                    {
                        UltraListViewItem item = new UltraListViewItem(foundItem);
                        item.Key = foundItem;
                        item.Tag = foundItem;
                        item.Value = foundItem;
                        ulstAvailableClients.Items.Add(item);
                    }
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
        /// Show only Searched item in Mapped clients view for a selected group
        /// </summary>
        public void AddSearchedItemMappedClients()
        {
            try
            {
                //if (_groupID != int.MinValue)
                //{
                List<String> searchingList = AccountGroupSetupManager.GetClientNamesForGroup(_groupID);
                List<String> result = AccountGroupSetupManager.SerachForKeyword(uTxtSearchMappedClients.Text, searchingList);
                ulstClientToGroup.Items.Clear();
                if (result.Count > 0)
                {
                    foreach (String foundItem in result)
                    {
                        UltraListViewItem item = new UltraListViewItem(foundItem);
                        item.Key = foundItem;
                        item.Tag = foundItem;
                        item.Value = foundItem;
                        ulstClientToGroup.Items.Add(item);
                    }
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
        /// save all changes in data base and clean back up of changes
        /// </summary>
        /// <param name="sender">button save</param>
        /// <param name="e">e</param>
        public int uBtnSaveAccountGroupSetup_Click()
        {
            bool isSaved = true;
            try
            {
                if (txtGroupName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Group Name.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }
                if (ulstClientToGroup.Items.Count == 0)
                {
                    MessageBox.Show("Please add atleast one client to group.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 0;
                }
                if (ulstAccountsToGroup.Items.Count == 0)
                {
                    DialogResult dialogResult = MessageBox.Show("You have not selected any account to this group. Do you want to add all accounts of selected Client(s)?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        uBtnAllUnSelectAvailableAccounts_Click(this, null);
                    }
                }

                if (ulstAccountsToGroup.Items.Count > 0)
                {
                    AccountGroupSetupManager.CleanBackUp();
                    isSaved = AccountGroupSetupManager.SaveMapping(_groupID, txtGroupName.Text.Trim());
                    if (!isSaved)
                    {
                        MessageBox.Show("Duplicate group name cannot be saved.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                    //Save data for audit
                    if (_groupID == int.MinValue)
                    {
                        int maxGroupID = AccountGroupSetupManager.GetMaxAccountGroupID();
                        if (maxGroupID > 0)
                        {
                            AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditAccountSetupData(maxGroupID), AuditManager.Definitions.Enum.AuditAction.AccountGroupCreated);
                        }
                    }
                    else
                        AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditAccountSetupData(_groupID), AuditManager.Definitions.Enum.AuditAction.AccountGroupUpdated);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot insert duplicate key in object 'dbo.T_AccountGroups'"))
                {
                    MessageBox.Show("Duplicate group names cannot be inserted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Function to Get dictionary for details of AccountGroups
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        private Dictionary<String, List<String>> AuditAccountSetupData(int groupID)
        {
            Dictionary<String, List<String>> auditDataForAccountGroup = new Dictionary<string, List<string>>();
            try
            {
                auditDataForAccountGroup.Add(CustomAuditSourceConstants.AuditSourceTypeAccountGroup, new List<string>());
                auditDataForAccountGroup[CustomAuditSourceConstants.AuditSourceTypeAccountGroup].Add(groupID.ToString());
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

            return auditDataForAccountGroup;
        }

        /// <summary>
        /// Show unmapped clients in ultra list view 
        /// </summary>
        public void BindUnMappedClients()
        {
            try
            {
                List<String> unMappedClientNames = new List<string>();
                //if (_clientID != int.MinValue)
                //{
                unMappedClientNames = AccountGroupSetupManager.GetUnmappedClients(_groupID);
                ulstAvailableClients.Items.Clear();
                foreach (String clientName in unMappedClientNames)
                {
                    UltraListViewItem item = new UltraListViewItem();
                    item.Tag = item.Key = clientName;
                    item.Value = clientName;
                    ulstAvailableClients.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Show mapped clients in ultra list view 
        /// </summary>
        public void BindMappedClients(List<String> clientNames)
        {
            try
            {
                if (clientNames == null)
                    ulstClientToGroup.Items.Clear();
                else
                {
                    ulstClientToGroup.Items.Clear();
                    foreach (String clientName in clientNames)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Tag = item.Key = clientName;
                        item.Value = clientName;
                        ulstClientToGroup.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Show mapped clients in ultra list view 
        /// </summary>
        /// <param name="accountNames">List of mappedclient Names for selected group </param>
        public void BindMappedclients(List<String> clientNames)
        {
            try
            {
                if (clientNames == null)
                    ulstClientToGroup.Items.Clear();
                else
                {

                    ulstClientToGroup.Items.Clear();
                    foreach (String clientName in clientNames)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Tag = item.Key = clientName;
                        item.Value = clientName;
                        ulstClientToGroup.Items.Add(item);
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

        public void InitializeAllLists(int groupID, List<String> clientNames)
        {
            BindMappedclients(clientNames);
            BindUnMappedClients();
            AccountGroupSetupManager.BindAccountByClient(BindClientDictionary(clientNames, groupID));
            BindUnMappedAccounts();
            if (_groupID != int.MinValue)
            {
                BindMappedAccounts(AccountGroupSetupManager.GetAccountNamesForGroup(groupID));
            }
            else
            {
                BindMappedAccounts(null);
            }
        }

        #endregion

        #region CommonFunctions

        /// <summary>
        /// add account names in list to be mapped 
        /// </summary>
        /// <param name="isOnlySelectedRequired">true of false only selected Require </param>
        /// <returns>List of toBeUnMappedAccounts</returns>
        private List<String> GetToBeUnMappedAccounts(bool isOnlySelectedRequired)
        {
            int count;
            List<string> toBeUnMappedAccounts = new List<string>();
            try
            {
                if (isOnlySelectedRequired)
                {
                    count = ulstAccountsToGroup.SelectedItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeUnMappedAccounts.Add(ulstAccountsToGroup.SelectedItems[i].Text);
                    }
                }
                else
                {
                    count = ulstAccountsToGroup.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeUnMappedAccounts.Add(ulstAccountsToGroup.Items[i].Text);
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
            return toBeUnMappedAccounts;
        }

        /// <summary>
        /// add account names in list to be Unmapped 
        /// </summary>
        /// <param name="isOnlySelectedRequired">true of false only selected Require </param>
        /// <returns>List of toBeMappedAccounts</returns>
        private List<string> GetToBeMappedAccounts(bool isOnlySelectedRequired)
        {
            int count;
            List<string> toBeMappedAccounts = new List<string>();
            try
            {
                if (isOnlySelectedRequired)
                {
                    count = ulstAvailableAccounts.SelectedItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeMappedAccounts.Add(ulstAvailableAccounts.SelectedItems[i].Text);
                    }
                }
                else
                {
                    count = ulstAvailableAccounts.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeMappedAccounts.Add(ulstAvailableAccounts.Items[i].Text);
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
            return toBeMappedAccounts;
        }

        /// <summary>
        /// add client names in list to be mapped 
        /// </summary>
        /// <param name="isOnlySelectedRequired">true of false only selected Require </param>
        /// <returns>List of toBeUnMappedclients</returns>
        private List<String> GetToBeUnMappedCients(bool isOnlySelectedRequired)
        {
            int count;
            List<string> toBeUnMappedclients = new List<string>();
            try
            {
                if (isOnlySelectedRequired)
                {
                    count = ulstClientToGroup.SelectedItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeUnMappedclients.Add(ulstClientToGroup.SelectedItems[i].Text);
                    }
                }
                else
                {
                    count = ulstClientToGroup.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeUnMappedclients.Add(ulstClientToGroup.Items[i].Text);
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
            return toBeUnMappedclients;
        }

        /// <summary>
        /// add client names in list to be Unmapped 
        /// </summary>
        /// <param name="isOnlySelectedRequired">true of false only selected Require </param>
        /// <returns>List of toBeMappedClients</returns>
        private List<string> GetToBeMappedClients(bool isOnlySelectedRequired)
        {
            int count;
            List<string> toBeMappedClients = new List<string>();
            try
            {
                if (isOnlySelectedRequired)
                {
                    count = ulstAvailableClients.SelectedItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeMappedClients.Add(ulstAvailableClients.SelectedItems[i].Text);
                    }
                }
                else
                {
                    count = ulstAvailableClients.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeMappedClients.Add(ulstAvailableClients.Items[i].Text);
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
            return toBeMappedClients;
        }

        #endregion

        #region Events

        /// <summary>
        /// Unselect selected accounts from available accounts to mapped accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UBtnUnSelectAvailableAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_groupID != int.MinValue)
                //{
                int groupID = _groupID;
                List<string> unSelectAccounts = GetToBeMappedAccounts(true);
                AccountGroupSetupManager.MapAccountsToGroup(groupID, unSelectAccounts);
                List<String> accountNames = AccountGroupSetupManager.GetAccountNamesForGroup(groupID);
                ulstAvailableAccounts.Items.Clear();
                BindMappedAccounts(accountNames);
                BindUnMappedAccounts();
                //Contain search to assigned key in AvailableAccounts list view
                uTxtSearchAvailableAccounts_TextChanged(uTxtSearchAvailableAccounts, null);
                //Contain search to assigned key in Mapped Accounts List view
                uTxtSearchMappedAccounts_TextChanged(uTxtSearchMappedAccounts, null);
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
        ///  Unselect all accounts from available accounts to available accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnAllUnSelectAvailableAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_groupID != int.MinValue)
                //{
                int groupID = _groupID;
                List<string> unSelectAccounts = GetToBeMappedAccounts(false);
                AccountGroupSetupManager.MapAccountsToGroup(groupID, unSelectAccounts);
                List<String> accountNames = AccountGroupSetupManager.GetAccountNamesForGroup(groupID);
                ulstAvailableAccounts.Items.Clear();
                BindMappedAccounts(accountNames);
                BindUnMappedAccounts();
                //Contain search to assigned key in AvailableAccounts list view
                uTxtSearchAvailableAccounts_TextChanged(uTxtSearchAvailableAccounts, null);
                //Contain search to assigned key in Mapped Accounts List view
                uTxtSearchMappedAccounts_TextChanged(uTxtSearchMappedAccounts, null);
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
        /// Unselect accounts from mapped accounts to available accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnSelectAccountAddtoGroup_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_groupID != int.MinValue)
                //{
                int groupID = _groupID;
                List<string> unSelectAccounts = GetToBeUnMappedAccounts(true);
                AccountGroupSetupManager.UnMapAccountsFromGroup(groupID, unSelectAccounts);
                List<String> accountNames = AccountGroupSetupManager.GetAccountNamesForGroup(groupID);
                ulstAccountsToGroup.Items.Clear();
                BindMappedAccounts(accountNames);
                BindUnMappedAccounts();
                //Contain search to assigned key in AvailableAccounts list view
                uTxtSearchAvailableAccounts_TextChanged(uTxtSearchAvailableAccounts, null);
                //Contain search to assigned key in Mapped Accounts List view
                uTxtSearchMappedAccounts_TextChanged(uTxtSearchMappedAccounts, null);
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
        /// Unselect all accounts from mapped accounts to available accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnAllUnSelectAccountAddtoGroup_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_groupID != int.MinValue)
                //{
                int groupID = _groupID;
                List<string> unSelectAccounts = GetToBeUnMappedAccounts(false);
                AccountGroupSetupManager.UnMapAccountsFromGroup(groupID, unSelectAccounts);
                List<String> accountNames = AccountGroupSetupManager.GetAccountNamesForGroup(groupID);
                ulstAccountsToGroup.Items.Clear();
                BindMappedAccounts(accountNames);
                BindUnMappedAccounts();
                //Contain search to assigned key in AvailableAccounts list view
                uTxtSearchAvailableAccounts_TextChanged(uTxtSearchAvailableAccounts, null);
                //Contain search to assigned key in Mapped Accounts List view
                uTxtSearchMappedAccounts_TextChanged(uTxtSearchMappedAccounts, null);
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
        /// searching in avaialble accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uTxtSearchAvailableAccounts_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(uTxtSearchAvailableAccounts.Text.ToUpper()) && uTxtSearchAvailableAccounts.Text.ToUpper() != "SEARCH")
                    AddSearchedItemAvailableAccounts();
                else
                {
                    ulstAvailableAccounts.Items.Clear();
                    BindUnMappedAccounts();
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
        /// searching in mapped accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uTxtSearchMappedAccounts_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(uTxtSearchMappedAccounts.Text.ToUpper()) && uTxtSearchMappedAccounts.Text.ToUpper() != "SEARCH")
                    AddSearchedItemMappedAccounts();
                else
                {
                    ulstAccountsToGroup.Items.Clear();
                    //if (_groupID != int.MinValue)
                    //{
                    List<String> accountNames = AccountGroupSetupManager.GetAccountNamesForGroup(_groupID);
                    if (accountNames != null)
                    {
                        foreach (String foundItem in accountNames)
                        {
                            UltraListViewItem item = new UltraListViewItem();
                            item.Key = foundItem;
                            item.Tag = foundItem;
                            item.Value = foundItem;
                            ulstAccountsToGroup.Items.Add(item);
                        }
                    }
                    else
                    {
                        ulstAccountsToGroup.Items.Clear();
                    }
                    //}
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
        /// Function to unselect available clients selected in list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UBtnUnSelectAvailableClients_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_groupID != int.MinValue)
                //{
                int groupID = _groupID;
                List<string> unSelectClients = GetToBeMappedClients(true);
                AccountGroupSetupManager.MapClientstoGroup(groupID, unSelectClients);
                List<String> clientNames = AccountGroupSetupManager.GetClientNamesForGroup(groupID);
                ulstAvailableClients.Items.Clear();
                InitializeAllLists(groupID, clientNames);
                //Contain search to assigned key in AvailableAccounts list view
                uTxtSearchAvailableClients_TextChanged(uTxtSearchAvailableClients, null);
                //Contain search to assigned key in Mapped Accounts List view
                uTxtSearchMappedClients_TextChanged(uTxtSearchMappedClients, null);
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
        /// Bind client dictionary from clientList and groupID
        /// </summary>
        /// <param name="clientList"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public Dictionary<int, List<int>> BindClientDictionary(List<String> clientList, int groupID)
        {
            Dictionary<int, List<int>> clientDictionary = new Dictionary<int, List<int>>();
            List<int> clientIDCollection = new List<int>();
            foreach (string item in clientList)
            {
                clientIDCollection.Add(AccountGroupSetupManager.GetClientIdByName(item));
            }
            clientDictionary.Add(groupID, clientIDCollection);
            return clientDictionary;
        }

        /// <summary>
        /// Function to unselect all available clients from view list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UBtnAllUnSelectAvailableClients_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_groupID != int.MinValue)
                //{
                int groupID = _groupID;
                List<string> unSelectClients = GetToBeMappedClients(false);
                AccountGroupSetupManager.MapClientstoGroup(groupID, unSelectClients);
                List<String> clientNames = AccountGroupSetupManager.GetClientNamesForGroup(groupID);
                ulstAvailableClients.Items.Clear();
                InitializeAllLists(groupID, clientNames);
                //Contain search to assigned key in AvailableAccounts list view
                uTxtSearchAvailableClients_TextChanged(uTxtSearchAvailableClients, null);
                //Contain search to assigned key in Mapped Accounts List view
                uTxtSearchMappedClients_TextChanged(uTxtSearchMappedClients, null);
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
        /// Function to unselect mapped clients selected in list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnUnSelectGroupToClients_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_groupID != int.MinValue)
                //{
                int groupID = _groupID;
                List<string> unSelectClients = GetToBeUnMappedCients(true);
                AccountGroupSetupManager.UnMapClientsFromGroup(groupID, unSelectClients);
                List<String> clientNames = AccountGroupSetupManager.GetClientNamesForGroup(groupID);
                ulstClientToGroup.Items.Clear();
                InitializeAllLists(groupID, clientNames);
                //Contain search to assigned key in AvailableAccounts list view
                uTxtSearchAvailableClients_TextChanged(uTxtSearchAvailableClients, null);
                //Contain search to assigned key in Mapped Accounts List view
                uTxtSearchMappedClients_TextChanged(uTxtSearchMappedClients, null);
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
        /// Function to unselect all mapped clients from in list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnAllUnSelectGroupToClients_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_groupID != int.MinValue)
                // {
                int groupID = _groupID;
                List<string> unSelectClients = GetToBeUnMappedCients(false);
                AccountGroupSetupManager.UnMapClientsFromGroup(groupID, unSelectClients);
                List<String> clientNames = AccountGroupSetupManager.GetClientNamesForGroup(groupID);
                ulstClientToGroup.Items.Clear();
                InitializeAllLists(groupID, clientNames);
                //Contain search to assigned key in AvailableAccounts list view
                uTxtSearchAvailableClients_TextChanged(uTxtSearchAvailableClients, null);
                //Contain search to assigned key in Mapped Accounts List view
                uTxtSearchMappedClients_TextChanged(uTxtSearchMappedClients, null);
                // }
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
        /// searching in available clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uTxtSearchAvailableClients_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(uTxtSearchAvailableClients.Text.ToUpper()) && uTxtSearchAvailableClients.Text.ToUpper() != "SEARCH")
                    AddSearchedItemAvailableClients();
                else
                {
                    ulstAvailableClients.Items.Clear();
                    BindUnMappedClients();
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
        /// searching in mapped clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uTxtSearchMappedClients_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(uTxtSearchMappedClients.Text.ToUpper()) && uTxtSearchMappedClients.Text.ToUpper() != "SEARCH")
                    AddSearchedItemMappedClients();
                else
                {
                    ulstClientToGroup.Items.Clear();
                    //if (_groupID != int.MinValue)
                    //{
                    List<String> clientNames = AccountGroupSetupManager.GetClientNamesForGroup(_groupID);
                    if (clientNames != null)
                    {
                        foreach (String foundItem in clientNames)
                        {
                            UltraListViewItem item = new UltraListViewItem();
                            item.Key = foundItem;
                            item.Tag = foundItem;
                            item.Value = foundItem;
                            ulstClientToGroup.Items.Add(item);
                        }
                    }
                    else
                    {
                        ulstClientToGroup.Items.Clear();
                    }
                    // }
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
        #endregion

        /// <summary>
        /// To unselect available Clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulstAvailableClients_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count > 0)
                {
                    ulstClientToGroup.SelectedItems.Clear();
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
        /// To unselect assigned Clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulstClientToGroup_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count > 0)
                {
                    ulstAvailableClients.SelectedItems.Clear();
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
        /// To unselect Assigned accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulstAvailableAccounts_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count > 0)
                {
                    ulstAccountsToGroup.SelectedItems.Clear();
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
        /// To unselect Available Accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulstAccountsToGroup_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count > 0)
                {
                    ulstAvailableAccounts.SelectedItems.Clear();
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
        /// Audit account group deletion
        /// </summary>
        /// <param name="groupID"></param>
        public void AuditAccountGroupDeletion(int groupID)
        {
            try
            {
                if (groupID != int.MinValue)
                {
                    AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditAccountSetupData(groupID), AuditManager.Definitions.Enum.AuditAction.AccountGroupDeleted);
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
}

