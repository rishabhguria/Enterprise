using Infragistics.Win.UltraWinEditors;
using Prana.Admin.BLL;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Admin.Controls.AutoGrouping
{
    /// <summary>
    /// This class is contains all the funtions which is required to performe from AutoGroup tab UI
    /// </summary>
    public partial class CtrlAllocationPref : UserControl
    {
        public CtrlAllocationPref()
        {
            InitializeComponent();

            account_selectallbox.Click += delegate (object sender, EventArgs e) { selectallbox_Click(sender, e, checkedlstAccounts); };
            checkedlstAccounts.SelectedValueChanged += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, account_selectallbox); };
            checkedlstAccounts.DoubleClick += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, account_selectallbox); };

        }

        /// <summary>
        /// Selectallboxes the click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <param name="selectedcheckedlistbox">The selectedcheckedlistbox.</param>
        private void selectallbox_Click(object sender, EventArgs e, CheckedListBox selectedcheckedlistbox)
        {
            CheckBox selectedcheckbox = sender as CheckBox;
            for (int i = 0; i < selectedcheckedlistbox.Items.Count; i++)
                selectedcheckedlistbox.SetItemChecked(i, selectedcheckbox.Checked);
        }

        /// <summary>
        /// Checkedlistboxes the selected value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <param name="currentselectallbox">The currentselectallbox.</param>
        private void checkedlistbox_SelectedValueChanged(object sender, EventArgs e, CheckBox currentselectallbox)
        {
            CheckedListBox selectedcheckedlistbox = sender as CheckedListBox;
            if (selectedcheckedlistbox.CheckedItems.Count == 0)
            {
                currentselectallbox.Checked = false;
                currentselectallbox.CheckState = CheckState.Unchecked;
            }
            else if (selectedcheckedlistbox.CheckedItems.Count < selectedcheckedlistbox.Items.Count)
                currentselectallbox.CheckState = CheckState.Indeterminate;
            else
            {
                currentselectallbox.Checked = true;
                currentselectallbox.CheckState = CheckState.Checked;
            }
        }

        #region Accounts Get Set
        /// <summary>
        /// Sets the checked accounts.
        /// </summary>
        /// <param name="companyID">The company identifier.</param>
        /// <param name="companyUserID">The company user identifier.</param>
        public void SetCheckedAccounts(string funds, int companyID)
        {
            Accounts companyAccounts = new Accounts();
            companyAccounts = CompanyManager.GetAccount(companyID);
            companyAccounts.Add(new Prana.Admin.BLL.Account(0, "Unallocated") { CompanyAccountID = 0 });
            checkedlstAccounts.DataSource = companyAccounts;
            checkedlstAccounts.DisplayMember = "AccountName";
            checkedlstAccounts.ValueMember = "CompanyAccountID";
            checkedlstAccounts.SelectedIndex = -1;
            List<int> fundIDs = new List<int>();
            if (!string.IsNullOrWhiteSpace(funds))
                fundIDs = funds.Split(',').Select(int.Parse).ToList();
            int i = 0;
            foreach (Prana.Admin.BLL.Account companyAccount in companyAccounts)
            {
                if (fundIDs.Contains(companyAccount.CompanyAccountID))
                {
                    checkedlstAccounts.SelectedValue = companyAccount.CompanyAccountID;
                    checkedlstAccounts.SetSelected(i, true);
                    checkedlstAccounts.SetItemChecked(i, true);
                }
                else
                    checkedlstAccounts.SetItemChecked(i, false);
                i++;
            }
            checkedlistbox_SelectedValueChanged(this.checkedlstAccounts, null, this.account_selectallbox);
        }

        /// <summary>
        /// Gets the checked accounts.
        /// </summary>
        /// <returns></returns>
        public List<int> GetCheckedAccounts()
        {
            List<int> accountsIDs = new List<int>();
            for (int i = 0, count = checkedlstAccounts.Items.Count; i < count; i++)
            {
                if (checkedlstAccounts.GetItemChecked(i))
                {
                    checkedlstAccounts.SetSelected(i, true);
                    accountsIDs.Add(((Prana.Admin.BLL.Account)checkedlstAccounts.SelectedItem).CompanyAccountID);
                }
            }
            return accountsIDs;
        }
        #endregion

        /// <summary>
        /// This class contains all the functions which are required to perform from AutoGroup tab UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enableAutoGroup_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var checked_boxes = filedChkLayout.Controls.OfType<UltraCheckEditor>();
                if (this.enableAutoGroup.Checked)
                {
                    grpAccounts.Enabled = true;
                    foreach (var chk in checked_boxes)
                    {
                        chk.Enabled = true;
                    }
                }
                else
                {
                    grpAccounts.Enabled = false;
                    foreach (var chk in checked_boxes)
                    {
                        chk.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This function is use to take all the values of check box and save it into dictionary.
        /// </summary>
        /// <returns>Dictionary<string, bool></returns>
        public Dictionary<string, bool> GetPrefs(out string funds)
        {
            Dictionary<string, bool> prefDict = new Dictionary<string, bool>();
            funds = string.Empty;
            try
            {
                prefDict.Add(enableAutoGroup.Name, enableAutoGroup.Checked);
                var ctrlList = filedChkLayout.Controls.OfType<UltraCheckEditor>();
                foreach (var chk in ctrlList)
                {
                    prefDict.Add(chk.Name, chk.Checked);
                }
                funds = string.Join(",", GetCheckedAccounts());
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
            return prefDict;
        }

        /// <summary>
        /// this function is to set the preference check box values.
        /// </summary>
        /// <param name="autoGroupPrefs">DataTable</param>
        private void UpdatePrefsUI(DataTable autoGroupPrefs)
        {
            try
            {
                DataRow row = autoGroupPrefs.Rows[0];
                chkTradeAttribute1.Checked = Convert.ToBoolean(row["TradeAttribute1"]);
                chkTradeAttribute2.Checked = Convert.ToBoolean(row["TradeAttribute2"]);
                chkTradeAttribute3.Checked = Convert.ToBoolean(row["TradeAttribute3"]);
                chkTradeAttribute4.Checked = Convert.ToBoolean(row["TradeAttribute4"]);
                chkTradeAttribute5.Checked = Convert.ToBoolean(row["TradeAttribute5"]);
                chkTradeAttribute6.Checked = Convert.ToBoolean(row["TradeAttribute6"]);
                chkBroker.Checked = Convert.ToBoolean(row["Broker"]);
                chkVenue.Checked = Convert.ToBoolean(row["Venue"]);
                chkProcessDate.Checked = Convert.ToBoolean(row["ProcessDate"]);
                chkTradingAC.Checked = Convert.ToBoolean(autoGroupPrefs.Rows[0]["TradingAC"]);
                chkTradeDate.Checked = Convert.ToBoolean(autoGroupPrefs.Rows[0]["TradeDate"]);
                SetCheckedAccounts(row["FundList"].ToString(), Convert.ToInt32(row["CompanyId"]));
                enableAutoGroup.Checked = Convert.ToBoolean(row["AutoGroup"]);
                enableAutoGroup_CheckedChanged(null, null);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the controls.
        /// </summary>
        public void SetControls()
        {
            try
            {
                UpdatePrefsUI(CompanyManager.GetAutoGroupingPreferences());
                SetAvgPriceRounding(CachedDataManager.GetInstance.GetAvgPriceRounding());
                // SetAllocationCheckSidePreference();
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

        ///// <summary>
        ///// Set Allocation Check Side Preference
        ///// </summary>
        //private void SetAllocationCheckSidePreference()
        //{
        //    try
        //    {
        //        AllocationCheckSidePref checkSidePref = CompanyManager.GetAllocationCheckSidePref(CachedDataManager.GetInstance.GetCompanyID());

        //        if (checkSidePref != null)
        //        {
        //            ChkValidateCheckSide.Checked = checkSidePref.DoCheckSideSystem;

        //            SetAccountMultiComboBox(checkSidePref);
        //            SetAssetMultiComboBox(checkSidePref);
        //            SetCounterPartyMultiComboBox(checkSidePref);
        //            // SetMasterFundsMultiComboBox();
        //            // SetAUECMultiComboBox();
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Set Master Funds Multi Combo Box
        ///// </summary>
        //private void SetAUECMultiComboBox(AllocationCheckSidePref checkSidePref)
        //{
        //    try
        //    {
        //        Dictionary<int, string> dictAUEC = new Dictionary<int, string>();

        //        if (cmbMultiAUEC.GetNoOfTotalItems() <= 0)
        //        {
        //            dictAUEC = CachedDataManager.GetInstance.GetAllAuecs();
        //            cmbMultiAUEC.SetManualTheme(false);
        //            //add Assets to the check list default value will be unchecked
        //            cmbMultiAUEC.AddItemsToTheCheckList(dictAUEC, CheckState.Unchecked);
        //            //adjust checklistbox width according to the longest Asset Name
        //            cmbMultiAUEC.AdjustCheckListBoxWidth();
        //            cmbMultiAUEC.TitleText = "AUEC";
        //            cmbMultiAUEC.SetTitleText(0);

        //        }
        //        if (checkSidePref.DisableCheckSidePref.ContainsKey(OrderFilterLevels.AUEC))
        //        {
        //            cmbMultiAUEC.SelectUnselectItems(checkSidePref.DisableCheckSidePref[OrderFilterLevels.AUEC].ToDictionary(x => x, y => y.ToString()), CheckState.Checked);
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

        ///// <summary>
        ///// Set Master Funds Multi Combo Box
        ///// </summary>
        //private void SetMasterFundsMultiComboBox(AllocationCheckSidePref checkSidePref)
        //{
        //    try
        //    {
        //        Dictionary<int, string> dictMasterFunds = new Dictionary<int, string>();

        //        if (cmbMultiMasterFund.GetNoOfTotalItems() <= 0)
        //        {
        //            dictMasterFunds = CachedDataManager.GetInstance.GetAllMasterFunds();
        //            cmbMultiMasterFund.SetManualTheme(false);
        //            //add Assets to the check list default value will be unchecked
        //            cmbMultiMasterFund.AddItemsToTheCheckList(dictMasterFunds, CheckState.Unchecked);
        //            //adjust checklistbox width according to the longest Asset Name
        //            cmbMultiMasterFund.AdjustCheckListBoxWidth();
        //            cmbMultiMasterFund.TitleText = "Master Fund";
        //            cmbMultiMasterFund.SetTitleText(0);

        //        }
        //        if (checkSidePref.DisableCheckSidePref.ContainsKey(OrderFilterLevels.MasterFund))
        //        {
        //            cmbMultiMasterFund.SelectUnselectItems(checkSidePref.DisableCheckSidePref[OrderFilterLevels.MasterFund].ToDictionary(x => x, y => y.ToString()),CheckState.Checked);
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

        ///// <summary>
        ///// Set Counter Party Multi Combo Box
        ///// </summary>
        //private void SetCounterPartyMultiComboBox(AllocationCheckSidePref checkSidePref)
        //{
        //    try
        //    {
        //        Dictionary<int, string> dictCounterParty = new Dictionary<int, string>();

        //        if (cmbMultiCounterParty.GetNoOfTotalItems() <= 0)
        //        {
        //            dictCounterParty = CachedDataManager.GetInstance.GetAllCounterParties();
        //            cmbMultiCounterParty.SetManualTheme(false);
        //            //add Assets to the check list default value will be unchecked
        //            cmbMultiCounterParty.AddItemsToTheCheckList(dictCounterParty, CheckState.Unchecked);
        //            //adjust checklistbox width according to the longest Asset Name
        //            cmbMultiCounterParty.AdjustCheckListBoxWidth();
        //            cmbMultiCounterParty.TitleText = "Counter Party";
        //            cmbMultiCounterParty.SetTitleText(0);
        //        }
        //        if (checkSidePref.DisableCheckSidePref.ContainsKey(OrderFilterLevels.CounterParty))
        //        {
        //            cmbMultiCounterParty.SelectUnselectItems(checkSidePref.DisableCheckSidePref[OrderFilterLevels.CounterParty].ToDictionary(x => x, y => y.ToString()), CheckState.Checked);

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

        ///// <summary>
        ///// Set Asset Multi Combo Box
        ///// </summary>
        //private void SetAssetMultiComboBox(AllocationCheckSidePref checkSidePref)
        //{
        //    try
        //    {
        //        Dictionary<int, string> dictAsset = new Dictionary<int, string>();

        //        if (cmbMultiAssets.GetNoOfTotalItems() <= 0)
        //        {
        //            dictAsset = CachedDataManager.GetInstance.GetAllAssets();
        //            cmbMultiAssets.SetManualTheme(false);
        //            //add Assets to the check list default value will be unchecked
        //            cmbMultiAssets.AddItemsToTheCheckList(dictAsset, CheckState.Unchecked);
        //            //adjust checklistbox width according to the longest Asset Name
        //            cmbMultiAssets.AdjustCheckListBoxWidth();
        //            cmbMultiAssets.TitleText = "Asset";
        //            cmbMultiAssets.SetTitleText(0);
        //        }
        //        if (checkSidePref.DisableCheckSidePref.ContainsKey(OrderFilterLevels.Asset))
        //        {
        //            cmbMultiAssets.SelectUnselectItems(checkSidePref.DisableCheckSidePref[OrderFilterLevels.Asset].ToDictionary(x => x, y => y.ToString()), CheckState.Checked);

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

        ///// <summary>
        ///// Set Account Multi Combo Box
        ///// </summary>
        //private void SetAccountMultiComboBox(AllocationCheckSidePref checkSidePref)
        //{
        //    try
        //    {
        //        Dictionary<int, string> dictAccounts = new Dictionary<int, string>();

        //        if (cmbMultiAccounts.GetNoOfTotalItems() <= 0)
        //        {
        //            dictAccounts = CachedDataManager.GetInstance.GetAccounts();
        //            cmbMultiAccounts.SetManualTheme(false);
        //            //add Assets to the check list default value will be unchecked
        //            cmbMultiAccounts.AddItemsToTheCheckList(dictAccounts, CheckState.Unchecked);
        //            //adjust checklistbox width according to the longest Asset Name
        //            cmbMultiAccounts.AdjustCheckListBoxWidth();
        //            cmbMultiAccounts.TitleText = "Account";
        //            cmbMultiAccounts.SetTitleText(0);
        //        }
        //        if (checkSidePref.DisableCheckSidePref.ContainsKey(OrderFilterLevels.Account))
        //        {
        //            cmbMultiAccounts.SelectUnselectItems(checkSidePref.DisableCheckSidePref[OrderFilterLevels.Account].ToDictionary(x => x, y => y.ToString()), CheckState.Checked);

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

        /// <summary>
        /// this funtion check alleast one check box should be selected process date or trade date.
        /// </summary>
        private void chkTradeDate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!chkProcessDate.Checked && !chkTradeDate.Checked)
                {
                    MessageBox.Show("Select at least one field from Process Date or Trade Date", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    chkTradeDate.Checked = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// this funtion check alleast one check box should be selected process date or trade date.
        /// </summary>
        private void chkProcessDate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!chkProcessDate.Checked && !chkTradeDate.Checked)
                {
                    MessageBox.Show("Select at least one field from Process Date or Trade Date", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    chkProcessDate.Checked = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the ChkAvgRounding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void chkAvgRounding_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (chkAvgRounding.Checked)
                    numAvgRounding.Enabled = true;
                else
                    numAvgRounding.Enabled = false;
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
        /// Gets the average price roundnig.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int? GetAvgPriceRoundnig()
        {
            try
            {
                if (chkAvgRounding.Checked)
                    return (int)Math.Round(numAvgRounding.Value, 0);
                return -1;
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
        /// Sets the average price roundnig.
        /// </summary>
        /// <param name="avgRounding">The average rounding.</param>
        public void SetAvgPriceRounding(int avgRounding)
        {
            try
            {
                if (avgRounding.Equals(-1))
                    chkAvgRounding.Checked = false;
                else
                {
                    chkAvgRounding.Checked = true;
                    numAvgRounding.Value = avgRounding;
                }
                chkAvgRounding_CheckedChanged(null, null);
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

        ///// <summary>
        ///// handle ChkValidateCheckSide
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ChkValidateCheckSide_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ChkValidateCheckSide.Checked)
        //        {
        //            cmbMultiAUEC.Enabled = true;
        //            cmbMultiAccounts.Enabled = true;
        //            cmbMultiAssets.Enabled = true;
        //            cmbMultiCounterParty.Enabled = true;
        //            cmbMultiMasterFund.Enabled = true;

        //        }
        //        else
        //        {
        //            cmbMultiAUEC.Enabled = false;
        //            cmbMultiAccounts.Enabled = false;
        //            cmbMultiAssets.Enabled = false;
        //            cmbMultiCounterParty.Enabled = false;
        //            cmbMultiMasterFund.Enabled = false;
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

        ///// <summary>
        ///// Get Allocation CheckSide Preferences
        ///// </summary>
        ///// <returns></returns>
        //public AllocationCheckSidePref SaveAllocationCheckSidePref(int companyId)
        //{
        //    AllocationCheckSidePref checkSidePref = new AllocationCheckSidePref();
        //    try
        //    {
        //        if (ChkValidateCheckSide.Checked)
        //        {
        //            checkSidePref.DoCheckSideSystem = true;
        //            Dictionary<OrderFilterLevels, List<int>> disableCheckSidePref = new Dictionary<OrderFilterLevels, List<int>>();

        //            if (cmbMultiAUEC.GetNoOfCheckedItems() > 0)
        //                disableCheckSidePref.Add(OrderFilterLevels.AUEC, cmbMultiAUEC.GetSelectedItemsInDictionary().Select(x => x.Key).ToList());

        //            if (cmbMultiAccounts.GetNoOfCheckedItems() > 0)
        //                disableCheckSidePref.Add(OrderFilterLevels.Account, cmbMultiAccounts.GetSelectedItemsInDictionary().Select(x => x.Key).ToList());

        //            if (cmbMultiAssets.GetNoOfCheckedItems() > 0)
        //                disableCheckSidePref.Add(OrderFilterLevels.Asset, cmbMultiAssets.GetSelectedItemsInDictionary().Select(x => x.Key).ToList());

        //            if (cmbMultiCounterParty.GetNoOfCheckedItems() > 0)
        //                disableCheckSidePref.Add(OrderFilterLevels.CounterParty, cmbMultiCounterParty.GetSelectedItemsInDictionary().Select(x => x.Key).ToList());

        //            if (cmbMultiMasterFund.GetNoOfCheckedItems() > 0)
        //                disableCheckSidePref.Add(OrderFilterLevels.MasterFund, cmbMultiMasterFund.GetSelectedItemsInDictionary().Select(x => x.Key).ToList());


        //            checkSidePref.DisableCheckSidePref = disableCheckSidePref;
        //        }
        //        else
        //        {
        //            checkSidePref.DoCheckSideSystem = false;

        //        }

        //        CompanyManager.SaveCompanyAllocationCheckSidePref(companyId,checkSidePref);
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
        //    return checkSidePref;
        //}
    }
}
