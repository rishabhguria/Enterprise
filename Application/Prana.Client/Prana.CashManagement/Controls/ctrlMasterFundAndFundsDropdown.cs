using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Prana.CashManagement.Controls
{
    public partial class ctrlMasterFundAndAccountsDropdown : UserControl
    {
        public ctrlMasterFundAndAccountsDropdown()
        {
            InitializeComponent();
        }

        private Dictionary<int, List<int>> masterFundSubAccountAssociation;
        private Dictionary<int, List<int>> masterFundSubAccountAssociationPermitted = new Dictionary<int, List<int>>();
        private Dictionary<int, string> userAccountsDict;
        public event EventHandler CheckStateChanged;

        public MultiSelectDropDown MultiAccounts
        {
            get { return cmbMultiAccounts; }
        }

        public void UpdateFundDropDown(Dictionary<int, RevaluationUpdateDetail> dictRevalDates)
        {
            try
            {
                Dictionary<int, string> updatedFundDict = new Dictionary<int, string>();
                if (cmbMultiAccounts.GetNoOfTotalItems() > 0)
                {
                    foreach (int accountId in cmbMultiAccounts.GetSelectedItemsInDictionary().Keys)
                    {
                        if (dictRevalDates.ContainsKey(accountId))
                        {
                            if (!updatedFundDict.ContainsKey(accountId))
                            {
                                updatedFundDict.Add(accountId, cmbMultiAccounts.GetSelectedItemsInDictionary()[accountId] + " ( Last Modified On:- " + dictRevalDates[accountId].LastRevaluationDate.ToString() + ")");
                            }
                            else
                            {
                                updatedFundDict[accountId] = cmbMultiAccounts.GetSelectedItemsInDictionary()[accountId] + " ( Last Modified On:- " + dictRevalDates[accountId].LastRevaluationDate.ToString() + ")";
                            }
                        }
                        else
                        {
                            if (!updatedFundDict.ContainsKey(accountId))
                            {
                                updatedFundDict.Add(accountId, cmbMultiAccounts.GetSelectedItemsInDictionary()[accountId]);
                            }
                            else
                            {
                                updatedFundDict[accountId] = cmbMultiAccounts.GetSelectedItemsInDictionary()[accountId];
                            }
                        }
                    }
                }
                cmbMultiAccounts.ClearAll();
                cmbMultiAccounts.AddItemsToTheCheckList(updatedFundDict, CheckState.Checked);
                cmbMultiAccounts.AdjustCheckListBoxWidth();
                cmbMultiAccounts.TitleText = "Account";
                cmbMultiAccounts.SelectUnselectAll(CheckState.Checked);
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
        /// Last Modified Date' should be auto publish (not on restarting CM module)
        /// </summary>
        Dictionary<int, string> updatedFundsDict = new Dictionary<int, string>();
        public void UpdateLastFundDropDown(DateTime date, List<int> accountId)
        {
            try
            {
                if (cmbMultiAccounts.GetNoOfTotalItems() > 0)
                {
                    foreach (int account in cmbMultiAccounts.GetAllItemsInDictionary().Keys)
                    {
                        if (accountId.Contains(account))
                        {
                            if (!updatedFundsDict.ContainsKey(account))
                            {
                                updatedFundsDict.Add(account, CachedDataManager.GetCashAccountName(account) + " ( Last Modified On:- " + date + ")");
                            }
                            else
                            {
                                updatedFundsDict[account] = CachedDataManager.GetCashAccountName(account) + " ( Last Modified On:- " + date + ")";
                            }
                        }
                        else
                        {
                            if (!updatedFundsDict.ContainsKey(account))
                            {
                                updatedFundsDict.Add(account, cmbMultiAccounts.GetAllItemsInDictionary()[account]);
                            }
                            else
                            {
                                updatedFundsDict[account] = cmbMultiAccounts.GetAllItemsInDictionary()[account];
                            }
                        }
                    }
                }
                Dictionary<int, string> dictSelectedAccounts = cmbMultiAccounts.GetSelectedItemsInDictionary();
                cmbMultiAccounts.ClearAll();
                cmbMultiAccounts.AddItemsToTheCheckList(updatedFundsDict, CheckState.Checked, dictSelectedAccounts);
                cmbMultiAccounts.AdjustCheckListBoxWidth();
                cmbMultiAccounts.TitleText = "Account";
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

        public void Setup()
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    masterFundSubAccountAssociation = new Dictionary<int, List<int>>(CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation());
                    SetAccountMultiComboBox();
                    SetMasterFundComboBox();
                    cmbMasterFund.CheckStateChanged += new EventHandler<ItemCheckEventArgs>(cmbMasterFund_StateChanged);
                    cmbMultiAccounts.CheckStateChanged += new EventHandler<ItemCheckEventArgs>(cmbMultiAccounts_StateChanged);
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

        public void SetBackColor()
        {
            try
            {
                this.BackColor = System.Drawing.Color.LightGray;
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

        #region Setting Both Dropdown Controls
        private void SetMasterFundComboBox()
        {
            try
            {
                if (cmbMasterFund.GetNoOfTotalItems() <= 0)
                {
                    Dictionary<int, string> dictMasterFunds = new Dictionary<int, string>();
                    if (cmbMasterFund.GetNoOfTotalItems() <= 0)
                    {
                        dictMasterFunds = CachedDataManager.GetInstance.GetAllMasterFunds();
                        //Pick MFs on the basis of their respective account's permissions[Userwise]
                        for (int counter = 0; counter < masterFundSubAccountAssociation.Keys.Count; counter++)
                        {
                            int key = masterFundSubAccountAssociation.Keys.ElementAt(counter);
                            List<int> accountIDs = masterFundSubAccountAssociation[key];
                            bool shouldRemove = true;
                            for (int ctr = 0; ctr < accountIDs.Count; ctr++)
                            {
                                if (userAccountsDict.ContainsKey(accountIDs[ctr]))
                                {
                                    shouldRemove = false;
                                    if (!masterFundSubAccountAssociationPermitted.ContainsKey(key))
                                    {
                                        List<int> lstActIds = new List<int>();
                                        lstActIds.Add(accountIDs[ctr]);
                                        masterFundSubAccountAssociationPermitted.Add(key, lstActIds);
                                    }
                                    else
                                    {
                                        masterFundSubAccountAssociationPermitted[key].Add(accountIDs[ctr]);
                                    }
                                }
                            }
                            if (shouldRemove)
                                dictMasterFunds.Remove(key);
                        }
                        //add Assets to the check list default value will be unchecked
                        cmbMasterFund.AddItemsToTheCheckList(dictMasterFunds, CheckState.Unchecked);
                        //adjust checklistbox width according to the longest Asset Name
                        cmbMasterFund.AdjustCheckListBoxWidth();
                        cmbMasterFund.TitleText = "MasterFund";
                        cmbMasterFund.SetTitleText(0);
                        cmbMasterFund.SelectUnselectAll(CheckState.Checked);
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

        private void SetAccountMultiComboBox()
        {
            try
            {
                Dictionary<int, string> dictAccounts = new Dictionary<int, string>();
                if (cmbMultiAccounts.GetNoOfTotalItems() <= 0)
                {
                    dictAccounts = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                    userAccountsDict = dictAccounts;
                    //add Assets to the check list default value will be unchecked
                    cmbMultiAccounts.AddItemsToTheCheckList(dictAccounts, CheckState.Checked);
                    //adjust checklistbox width according to the longest Asset Name
                    cmbMultiAccounts.AdjustCheckListBoxWidth();
                    cmbMultiAccounts.TitleText = "Account";
                    cmbMultiAccounts.SelectUnselectAll(CheckState.Checked);
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
        #endregion

        private void cmbMasterFund_StateChanged(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (CheckStateChanged != null)
                    CheckStateChanged(this, null);

                if (e.Index.Equals(0))
                {
                    if (e.NewValue.Equals(CheckState.Unchecked))
                    {
                        cmbMultiAccounts.SelectUnselectAllForMasterFund(CheckState.Unchecked);
                    }
                    else
                    {
                        cmbMultiAccounts.SelectUnselectAllForMasterFund(CheckState.Checked);
                    }
                }
                else
                {
                    KeyValuePair<int, string> keyValue = (KeyValuePair<int, string>)cmbMasterFund.CheckedMultipleItems.Items[e.Index];
                    List<int> accountIDs = masterFundSubAccountAssociationPermitted[keyValue.Key];

                    //Changing the check state of respective accounts
                    foreach (int accountID in accountIDs)
                    {
                        int accountIndex = GetIndexByAccountID(accountID);
                        CheckState state = cmbMultiAccounts.CheckedMultipleItems.GetItemCheckState(accountIndex);
                        if (e.CurrentValue == CheckState.Indeterminate)
                        {
                            cmbMultiAccounts.AssignNewCheckState(accountIndex, CheckState.Unchecked);
                        }
                        else if (state == e.CurrentValue)
                        {
                            cmbMultiAccounts.AssignNewCheckState(accountIndex, e.NewValue);
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

        private void cmbMultiAccounts_StateChanged(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (CheckStateChanged != null)
                    CheckStateChanged(this, null);

                if (e.Index.Equals(0))
                {
                    if (e.NewValue.Equals(CheckState.Unchecked))
                    {
                        cmbMasterFund.SelectUnselectAllForMasterFund(CheckState.Unchecked);
                    }
                    else
                    {
                        cmbMasterFund.SelectUnselectAllForMasterFund(CheckState.Checked);
                    }
                }
                else
                {
                    int masterFundID = 1, indexCount = 1, accountsCountForMasterFund = 1;
                    bool isfound = false;
                    KeyValuePair<int, string> keyValue = (KeyValuePair<int, string>)cmbMultiAccounts.CheckedMultipleItems.Items[e.Index];
                    foreach (KeyValuePair<int, List<int>> masterFund_accountIds in masterFundSubAccountAssociationPermitted)
                    {
                        masterFundID = masterFund_accountIds.Key;
                        accountsCountForMasterFund = masterFund_accountIds.Value.Count;
                        foreach (int accountID in masterFund_accountIds.Value)
                        {
                            if (accountID.Equals(keyValue.Key))
                            {
                                isfound = true;
                                break;
                            }
                        }
                        if (isfound)
                            break;

                        indexCount++;
                    }
                    if (indexCount != cmbMasterFund.CheckedMultipleItems.Items.Count)
                    {
                        int masterFundIndex = GetIndexByMasterFundName(CachedDataManager.GetInstance.GetMasterFund(masterFundID));
                        if (accountsCountForMasterFund == 1)
                        {
                            if (cmbMasterFund.GetNoOfTotalItems() >= masterFundIndex)
                                cmbMasterFund.AssignNewCheckState(masterFundIndex, e.NewValue);
                        }
                        else
                        {
                            List<int> accountIDs;
                            if (masterFundSubAccountAssociationPermitted.TryGetValue(masterFundID, out accountIDs))
                            {
                                int accountsCheckCount = 0, accountsUncheckCount = 0;
                                foreach (int accountID in accountIDs)
                                {
                                    foreach (KeyValuePair<int, string> kvp in cmbMultiAccounts.CheckedMultipleItems.Items)
                                    {
                                        if (kvp.Key.Equals(accountID))
                                        {
                                            int accountIndex = GetIndexByAccountID(accountID);
                                            CheckState accountState = cmbMultiAccounts.CheckedMultipleItems.GetItemCheckState(accountIndex);
                                            if (accountState == CheckState.Unchecked)
                                                accountsUncheckCount++;
                                            else
                                                accountsCheckCount++;
                                            break;
                                        }
                                    }
                                }
                                if (accountIDs.Count.Equals(accountsCheckCount + 1) && e.NewValue == CheckState.Checked)
                                {
                                    if (cmbMasterFund.GetNoOfTotalItems() >= masterFundIndex)
                                        cmbMasterFund.AssignNewCheckState(masterFundIndex, CheckState.Checked);
                                }
                                else if (accountIDs.Count.Equals(accountsUncheckCount + 1) && e.NewValue == CheckState.Unchecked)
                                {
                                    if (cmbMasterFund.GetNoOfTotalItems() >= masterFundIndex)
                                        cmbMasterFund.AssignNewCheckState(masterFundIndex, CheckState.Unchecked);
                                }
                                else
                                {
                                    if (cmbMasterFund.GetNoOfTotalItems() >= masterFundIndex)
                                        cmbMasterFund.AssignNewCheckState(masterFundIndex, CheckState.Indeterminate);
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
        }

        private int GetIndexByMasterFundName(string masterFundName)
        {
            int masterFundIndex = 0;
            try
            {
                foreach (KeyValuePair<int, string> masterFund in cmbMasterFund.CheckedMultipleItems.Items)
                {
                    if (masterFund.Value.Equals(masterFundName))
                    {
                        break;
                    }
                    masterFundIndex++;
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
            return masterFundIndex;
        }

        private int GetIndexByAccountID(int accountID)
        {
            int accountIndex = 0;
            try
            {
                foreach (KeyValuePair<int, string> account in cmbMultiAccounts.CheckedMultipleItems.Items)
                {
                    if (account.Key.Equals(accountID))
                    {
                        break;
                    }
                    accountIndex++;
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
            return accountIndex;
        }

        public void UnWireEvents()
        {
            try
            {
                cmbMasterFund.CheckStateChanged -= new EventHandler<ItemCheckEventArgs>(cmbMasterFund_StateChanged);
                cmbMultiAccounts.CheckStateChanged -= new EventHandler<ItemCheckEventArgs>(cmbMultiAccounts_StateChanged);
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
        public void WireEvents()
        {
            try
            {
                cmbMasterFund.CheckStateChanged += new EventHandler<ItemCheckEventArgs>(cmbMasterFund_StateChanged);
                cmbMultiAccounts.CheckStateChanged += new EventHandler<ItemCheckEventArgs>(cmbMultiAccounts_StateChanged);
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
