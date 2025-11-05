using Infragistics.Win.UltraWinCalcManager;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Rebalancer.PercentTradingTool.Preferences
{
    public partial class ctrlPTTPreferences : UserControl, IPreferences
    {
        public ctrlPTTPreferences()
        {
            InitializeComponent();
        }

        private CompanyUser _user;

        public CompanyUser User
        {
            get { return _user; }
            set { _user = value; }
        }

        public void SetUp(CompanyUser user)
        {
            try
            {
                _user = user;
                BindGrid();
                PTTPrefDataManager.GetInstance.GetPTTDollorAmountPermission();
                FillPTTcombos();
                FillAccountCombo();
                PTTPreferences preferences = PTTPrefDataManager.GetInstance.GetPTTPreferences(user.CompanyUserID);
                if (preferences != null)
                {
                    SetPTTInputParameters(preferences);
                }
                if (chkboxShortLongPref.Checked)
                {
                    grpBxMasterFundAccount.Visible = false;
                    ultraTabShortLong.Visible = true;
                }
                else
                {
                    ultraTabShortLong.Visible = false;
                    grpBxMasterFundAccount.Visible = true;
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

        private void SetPTTInputParameters(PTTPreferences preferences)
        {
            try
            {
                cmbAddSet.Value = (int)preferences.ChangeType;
                cmbCalculationType.Value = (int)preferences.CalculationType;
                cmbCombineAccountTotal.Value = (int)preferences.CombineAccountsTotal;
                cmbMasterFundOrAccountValue.Value = (int)preferences.MasterFundOrAccount;
                chkRemoveAccountsWithZeroNAV.Checked = preferences.RemoveAccountsWithZeroNAV;
                numIncreaseDecimalPrecision.Value = preferences.IncreaseDecimalPrecision;
                chkboxShortLongPref.Checked = preferences.UseShortLongPref;
                if (preferences.CommaSeparatedAccounts != string.Empty)
                {
                    var accountIds = preferences.CommaSeparatedAccounts.Split(Seperators.SEPERATOR_14).ToList();
                    Dictionary<int, string> dictAccounts = new Dictionary<int, string>();
                    if (accountIds != null || accountIds.Count() != 0)
                    {
                        foreach (var accountId in accountIds)
                        {
                            int accID;
                            int.TryParse(accountId, out accID);
                            string accountText = CachedDataManager.GetInstance.GetAccountText(accID);
                            if (!dictAccounts.ContainsKey(accID))
                            {
                                dictAccounts.Add(accID, accountText);
                            }
                        }
                    }
                    multiSelectDropDownAccount.SelectUnselectItems(dictAccounts, CheckState.Checked);
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

        private void FillPTTcombos()
        {
            try
            {
                cmbMasterFundOrAccountValue.DataSource = Enum.GetValues(typeof(PTTMasterFundOrAccount));
                cmbAddSet.DataSource = Enum.GetValues(typeof(PTTChangeType));
                if (!Boolean.Parse(PTTPrefDataManager.PTTDollarAmountPermission.ToString()))
                    cmbCalculationType.DataSource = Enum.GetValues(typeof(PTTType)).Cast<PTTType>().Where(item => item != PTTType.DollarAmount).ToArray();
                else
                    cmbCalculationType.DataSource = Enum.GetValues(typeof(PTTType));

                cmbCombineAccountTotal.DataSource = Enum.GetValues(typeof(PTTCombineAccountTotalValue));
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
        /// Fill Account Combo
        /// </summary>
        private void FillAccountCombo()
        {
            try
            {
                Dictionary<int, string> dictAccounts = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                if (dictAccounts != null)
                {
                    multiSelectDropDownAccount.AddItemsToTheCheckList(dictAccounts, CheckState.Unchecked);
                }
                multiSelectDropDownAccount.AdjustCheckListBoxWidth();
                multiSelectDropDownAccount.TitleText = PTTConstants.CAP_ACCOUNTTEXT;
                multiSelectDropDownAccount.SetTextEditorText("No Account(s) Selected");
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

        private void FillMasterFundCombo()
        {
            try
            {
                Dictionary<int, string> dictUserPermittedMasterFund = CachedDataManager.GetInstance.GetUserMasterFunds();
                if (dictUserPermittedMasterFund != null)
                {
                    multiSelectDropDownAccount.AddItemsToTheCheckList(dictUserPermittedMasterFund, CheckState.Unchecked);
                }
                multiSelectDropDownAccount.AdjustCheckListBoxWidth();
                multiSelectDropDownAccount.TitleText = PTTConstants.CAP_MASTERFUNDTEXT;
                multiSelectDropDownAccount.SetTextEditorText("No Master Fund(s) Selected");
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


        private void BindGrid()
        {
            try
            {
                List<BindingList<PTTMFAccountPref>> mfAccountPref = DeepCopyHelper.Clone(PTTPrefDataManager.GetInstance.GetMasterFundAccountPreferences());
                masterFundAccountGridControlForGroupBox.BindGrid(mfAccountPref[(int)PTTPreferenceType.Global]);
                masterFundAccountGridControlForLongTab.BindGrid(mfAccountPref[(int)PTTPreferenceType.Long]);
                masterFundAccountGridControlForShortTab.BindGrid(mfAccountPref[(int)PTTPreferenceType.Short]);

                if (mfAccountPref[0] != null)
                {
                    grdAccountFactor.DataSource = DeepCopyHelper.Clone(PTTPrefDataManager.PttAccountFactorBindingList);
                    SetGridLayout();
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


        private void SetGridLayout()
        {
            try
            {
                grdAccountFactor.DisplayLayout.Bands[0].Columns[PTTConstants.COL_ACCOUNTNAME].Header.Caption = PTTConstants.CAP_ACCOUNT;
                grdAccountFactor.DisplayLayout.Bands[0].Columns[PTTConstants.COL_ACCOUNTNAME].CellActivation = Activation.NoEdit;
                grdAccountFactor.DisplayLayout.Bands[0].Columns[PTTConstants.COL_ACCOUNTFACTOR].Header.Caption = PTTConstants.CAP_ACCOUNT_FACTOR;
                grdAccountFactor.DisplayLayout.Bands[0].Columns[PTTConstants.COL_ACCOUNTFACTOR].CellActivation = Activation.AllowEdit;
                grdAccountFactor.DisplayLayout.Bands[0].Columns[PTTConstants.COL_ACCOUNTFACTOR].MaxValue = 10000;
                grdAccountFactor.DisplayLayout.Bands[0].Columns[PTTConstants.COL_ACCOUNTFACTOR].MinValue = 0;
                grdAccountFactor.DisplayLayout.Bands[0].Columns[PTTConstants.COL_PERCENTAGE].Hidden = true;
                grdAccountFactor.DisplayLayout.Bands[0].Columns[PTTConstants.COL_ACCOUNTFACTOR].Format = "#,##,###0.00";
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

        public UserControl Reference()
        {
            return this;
        }

        public bool Save()
        {
            try
            {
                List<BindingList<PTTMFAccountPref>> mfAccPrefBindingList = new List<BindingList<PTTMFAccountPref>>();

                BindingList<PTTMFAccountPref> mfAccPrefGlobalBindingList = masterFundAccountGridControlForGroupBox.Save(PTTPreferenceType.Global);
                BindingList<PTTMFAccountPref> mfAccPrefLongBindingList = masterFundAccountGridControlForLongTab.Save(PTTPreferenceType.Long);
                BindingList<PTTMFAccountPref> mfAccPrefShortBindingList = masterFundAccountGridControlForShortTab.Save(PTTPreferenceType.Short);
                if (mfAccPrefGlobalBindingList != null && mfAccPrefLongBindingList != null && mfAccPrefShortBindingList != null)
                {
                    mfAccPrefBindingList.Add(mfAccPrefGlobalBindingList);
                    mfAccPrefBindingList.Add(mfAccPrefLongBindingList);
                    mfAccPrefBindingList.Add(mfAccPrefShortBindingList);

                    grdAccountFactor.UpdateData();
                    BindingList<PTTAccountPreference> accountPrefList = (BindingList<PTTAccountPreference>)grdAccountFactor.DataSource;
                    Dictionary<int, float> dictAccountIDAccFactor = accountPrefList.ToDictionary(acc => acc.AccountId, acc => acc.AccountFactor);
                    PTTPrefDataManager.SavePTTMFAccountPrefDB(mfAccPrefBindingList, dictAccountIDAccFactor);

                    PTTPrefDataManager.PttAccountFactorBindingList = DeepCopyHelper.Clone(accountPrefList);
                    SavePTTInputParameters();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return false;
        }

        private void SavePTTInputParameters()
        {
            try
            {
                PTTPreferences preferences = new PTTPreferences();
                PTTMasterFundOrAccount masterFundOrAccountValue;
                PTTCombineAccountTotalValue combineAccountTotal;
                PTTChangeType addOrSet;
                PTTType type;
                Enum.TryParse<PTTChangeType>(cmbAddSet.SelectedItem.ToString(), out addOrSet);
                Enum.TryParse<PTTType>(cmbCalculationType.SelectedItem.ToString(), out type);
                Enum.TryParse<PTTMasterFundOrAccount>(cmbMasterFundOrAccountValue.SelectedItem.ToString(), out masterFundOrAccountValue);
                Enum.TryParse<PTTCombineAccountTotalValue>(cmbCombineAccountTotal.SelectedItem.ToString(), out combineAccountTotal);
                preferences.CombineAccountsTotal = combineAccountTotal;
                preferences.ChangeType = addOrSet;
                preferences.CalculationType = type;
                preferences.MasterFundOrAccount = masterFundOrAccountValue;
                preferences.IncreaseDecimalPrecision = Int32.Parse(numIncreaseDecimalPrecision.Value.ToString());
                preferences.RemoveAccountsWithZeroNAV = chkRemoveAccountsWithZeroNAV.Checked;
                preferences.UseShortLongPref = chkboxShortLongPref.Checked;
                Dictionary<int, string> dictSelectedAccounts = multiSelectDropDownAccount.GetSelectedItemsInDictionary();
                string commaSeparatedAccountIDs = String.Join(Seperators.SEPERATOR_8, dictSelectedAccounts.Select(x => x.Key));

                if (!string.IsNullOrEmpty(commaSeparatedAccountIDs))
                {
                    preferences.CommaSeparatedAccounts = commaSeparatedAccountIDs;
                }
                else
                {
                    preferences.CommaSeparatedAccounts = string.Empty;
                }

                PTTPrefDataManager.SavePTTInputParametersPrefs(preferences, User.CompanyUserID);
                PTTPrefDataManager.GetInstance.PTTPreferences = preferences;
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



        public void RestoreDefault()
        {
        }

        public IPreferenceData GetPrefs()
        {
            return PTTPrefDataManager.GetInstance.PTTPreferences;
        }


        private string _modulename = string.Empty;

        public string SetModuleActive
        {
            set
            {
                _modulename = value;
            }
        }

        private void grdMasterFundAccountPercentage_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraCalcManager calcManager;
                calcManager = new UltraCalcManager(this.Container);
                e.Layout.Grid.CalcManager = calcManager;
                e.Layout.Bands[0].Columns[PTTConstants.COL_TOTALPERCENTAGE].Formula = "Sum([AccountWisePercentage/Percentage])";
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

        private void cmbMasterFundOrAccountValue_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                multiSelectDropDownAccount.ClearAll();
                if ((PTTMasterFundOrAccount)cmbMasterFundOrAccountValue.Value == PTTMasterFundOrAccount.MasterFund)
                {
                    lblAccount.Text = PTTConstants.CAP_MASTERFUNDTEXT;
                    FillMasterFundCombo();
                }
                else
                {
                    lblAccount.Text = PTTConstants.CAP_ACCOUNTTEXT;
                    FillAccountCombo();
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

        private void grdAccountFactor_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            try
            {
                (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
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

        private void grdMasterFundAccountPercentage_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            try
            {
                (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
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

        private void grdAccountFactor_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {
            try
            {
                UltraGrid grid = (UltraGrid)sender;
                UltraGridCell activeCell = grid.ActiveCell;
                float cellText = 0;
                if (!activeCell.IsFilterRowCell)
                {
                    if (float.TryParse(Convert.ToString(activeCell.Text), out cellText))
                    {
                        if (cellText > 1000)
                        {
                            activeCell.Value = 1000;
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        activeCell.Value = 0;
                        e.Cancel = true;
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

        }



        private void grdMasterFundAccountPercentage_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {
            try
            {
                UltraGrid grid = (UltraGrid)sender;
                UltraGridCell activeCell = grid.ActiveCell;
                float cellText = 0;
                if (!grid.ActiveCell.Column.Key.Equals(PTTConstants.COL_IS_PRORATA_PERCENTAGE))
                {
                    if (float.TryParse(Convert.ToString(activeCell.Text), out cellText))
                    {
                        if (cellText > 100)
                        {
                            activeCell.Value = 100;
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        activeCell.Value = 0;
                        e.Cancel = true;
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

        }
        #region click shortlongPreference check box

        private void chkboxShortLongPref_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (chkboxShortLongPref.Checked)
                {
                    grpBxMasterFundAccount.Visible = false;
                    ultraTabShortLong.Visible = true;
                }
                else
                {
                    ultraTabShortLong.Visible = false;
                    grpBxMasterFundAccount.Visible = true;
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

        #endregion
    }

}
