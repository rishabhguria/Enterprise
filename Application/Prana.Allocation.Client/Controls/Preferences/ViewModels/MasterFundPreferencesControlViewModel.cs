using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Controls.Common.ViewModels;
using Prana.Allocation.Client.EventArguments;
using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class MasterFundPreferencesControlViewModel : ViewModelBase
    {
        #region Members

        /// <summary>
        /// The _selected master fund
        /// </summary>
        private string _mFCalulatedPrefLabel;

        /// <summary>
        /// The _calculated preferences list control view model
        /// </summary>
        private CalculatedPreferencesListControlViewModel _calculatedPreferencesListControlViewModel;

        /// <summary>
        /// The _calculated preference default rule control view model
        /// </summary>
        private CalculatedPreferenceDefaultRuleControlViewModel _calculatedPreferenceDefaultRuleControlViewModel;

        /// <summary>
        /// The _calculated preference general rule control view model
        /// </summary>
        private CalculatedPreferenceGeneralRuleControlViewModel _calculatedPreferenceGeneralRuleControlViewModel;

        /// <summary>
        /// The _preference Account strategy control view model
        /// </summary>
        private PreferenceAccountStrategyControlViewModel _preferenceAccountStrategyControlViewModel;

        /// <summary>
        /// The _master fund ratio control view model
        /// </summary>
        private MasterFundRatioControlViewModel _masterFundRatioControlViewModel;

        /// <summary>
        /// The master fund default rule view model
        /// </summary>
        private DefaultRuleControlViewModel _masterFundDefaultRuleViewModel;

        /// <summary>
        /// Occurs when [preview preference event].
        /// </summary>
        public event EventHandler<EventArgs<KeyValuePair<int, string>>> PreviewPreferenceEvent;

        /// <summary>
        /// Occurs when [update preference cache event].
        /// </summary>
        public event EventHandler<EventArgs<KeyValuePair<int, string>>> UpdateMFPreferenceCacheEvent;

        /// <summary>
        /// Occurs when [preference event].
        /// </summary>
        public event EventHandler<EventArgs<AllocationPrefOperationEventArgs>> PreferenceEvent;

        /// <summary>
        /// UpdateCalculatedPrefCacheEvent
        /// </summary>
        public event EventHandler<EventArgs<KeyValuePair<int, string>>> UpdateCalculatedPrefCacheEvent;

        /// <summary>
        /// PreviewPreferenceSelectedMF
        /// </summary>
        public event EventHandler<EventArgs<KeyValuePair<int, string>>> PreviewPreferenceSelectedMF;

        /// <summary>
        /// The _allocation master fundref
        /// </summary>
        internal AllocationMasterFundPreference _allocationMasterFundPref = null;

        /// <summary>
        /// The original allocation master fund preference
        /// </summary>
        private AllocationMasterFundPreference _originalAllocationMasterFundPref = null;

        /// <summary>
        /// The account list master fund associated
        /// </summary>
        List<int> _accountListMasterFundAssociated = new List<int>();

        /// <summary>
        /// The selected master funds list
        /// </summary>
        ObservableDictionary<int, string> _selectedMasterFundsList = new ObservableDictionary<int, string>();

        /// <summary>
        /// The selected m ffor preview calculated preference
        /// </summary>
        KeyValuePair<int, string> _selectedMFforPreviewCalculatedPref = new KeyValuePair<int, string>();

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the selected master funds list.
        /// </summary>
        /// <value>
        /// The selected master funds list.
        /// </value>
        public ObservableDictionary<int, string> SelectedMasterFundsList
        {
            get { return _selectedMasterFundsList; }
            set
            {
                _selectedMasterFundsList = value;
                if (_selectedMasterFundsList.Count == 0)
                    DisableControlsAndClearGrid("*No master fund selected for allocation.");
                RaisePropertyChangedEvent("SelectedMasterFundsList");
            }
        }

        /// <summary>
        /// Gets or sets the selected m ffor preview calculated preference.
        /// </summary>
        /// <value>
        /// The selected m ffor preview calculated preference.
        /// </value>
        public KeyValuePair<int, string> SelectedMFforPreviewCalculatedPref
        {
            get { return _selectedMFforPreviewCalculatedPref; }
            set
            {
                UpdateCalculatedPreference();

                _selectedMFforPreviewCalculatedPref = value;
                RaisePropertyChangedEvent("SelectedMFforPreviewCalculatedPref");

                PreviewCalculatedPreference();

            }
        }

        /// <summary>
        /// Gets or sets the selected master fund.
        /// </summary>
        /// <value>
        /// The selected master fund.
        /// </value>
        public string MFCalulatedPrefLabel
        {
            get { return _mFCalulatedPrefLabel; }
            set
            {
                _mFCalulatedPrefLabel = value;
                RaisePropertyChangedEvent("MFCalulatedPrefLabel");
            }
        }

        /// <summary>
        /// Gets or sets the calculated preference default rule control view model.
        /// </summary>
        /// <value>
        /// The calculated preference default rule control view model.
        /// </value>
        public CalculatedPreferenceDefaultRuleControlViewModel CalculatedPreferenceDefaultRuleControlViewModel
        {
            get { return _calculatedPreferenceDefaultRuleControlViewModel; }
            set
            {
                _calculatedPreferenceDefaultRuleControlViewModel = value;
                RaisePropertyChangedEvent("CalculatedPreferenceDefaultRuleControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the calculated preference general rule control view model.
        /// </summary>
        /// <value>
        /// The calculated preference general rule control view model.
        /// </value>
        public CalculatedPreferenceGeneralRuleControlViewModel CalculatedPreferenceGeneralRuleControlViewModel
        {
            get { return _calculatedPreferenceGeneralRuleControlViewModel; }
            set
            {
                _calculatedPreferenceGeneralRuleControlViewModel = value;
                RaisePropertyChangedEvent("CalculatedPreferenceGeneralRuleControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the calculated preferences list control view model.
        /// </summary>
        /// <value>
        /// The calculated preferences list control view model.
        /// </value>
        public CalculatedPreferencesListControlViewModel CalculatedPreferencesListControlViewModel
        {
            get { return _calculatedPreferencesListControlViewModel; }
            set
            {
                _calculatedPreferencesListControlViewModel = value;
                RaisePropertyChangedEvent("CalculatedPreferencesListControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the preference Account strategy control view model.
        /// </summary>
        /// <value>
        /// The preference Account strategy control view model.
        /// </value>
        public PreferenceAccountStrategyControlViewModel PreferenceAccountStrategyControlViewModel
        {
            get { return _preferenceAccountStrategyControlViewModel; }
            set
            {
                _preferenceAccountStrategyControlViewModel = value;
                RaisePropertyChangedEvent("PreferenceAccountStrategyControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the master fund ratio control view model.
        /// </summary>
        /// <value>
        /// The master fund ratio control view model.
        /// </value>
        public MasterFundRatioControlViewModel MasterFundRatioControlViewModel
        {
            get { return _masterFundRatioControlViewModel; }
            set
            {
                _masterFundRatioControlViewModel = value;
                RaisePropertyChangedEvent("MasterFundRatioControlViewModel");
            }
        }

        /// <summary>
        /// Gets or sets the master fund default rule view model.
        /// </summary>
        /// <value>
        /// The master fund default rule view model.
        /// </value>
        public DefaultRuleControlViewModel MasterFundDefaultRuleViewModel
        {
            get { return _masterFundDefaultRuleViewModel; }
            set
            {
                _masterFundDefaultRuleViewModel = value;
                RaisePropertyChangedEvent("MasterFundDefaultRuleViewModel");
            }
        }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterFundPreferencesControlViewModel"/> class.
        /// </summary>
        public MasterFundPreferencesControlViewModel()
        {
            try
            {
                _calculatedPreferencesListControlViewModel = new CalculatedPreferencesListControlViewModel();
                _calculatedPreferenceDefaultRuleControlViewModel = new CalculatedPreferenceDefaultRuleControlViewModel();
                _calculatedPreferenceGeneralRuleControlViewModel = new CalculatedPreferenceGeneralRuleControlViewModel();
                _preferenceAccountStrategyControlViewModel = new PreferenceAccountStrategyControlViewModel();
                _masterFundRatioControlViewModel = new MasterFundRatioControlViewModel();
                _masterFundDefaultRuleViewModel = new DefaultRuleControlViewModel();
                _calculatedPreferencesListControlViewModel.PreviewPreferenceEvent += _masterFundPreferencesListControlViewModel_PreviewPreference;
                _calculatedPreferencesListControlViewModel.UpdatePreferenceCacheEvent += _masterFundPreferencesListControlViewModel_UpdatePreferenceCacheEvent;
                _calculatedPreferencesListControlViewModel.PreferenceEvent += _masterFundPreferencesListControlViewModel_PreferenceEvent;
                _calculatedPreferenceGeneralRuleControlViewModel.AddNewPreference += _masterFundPreferenceGeneralRuleControlViewModel_AddNewPreference;
                _masterFundDefaultRuleViewModel.SelectedProrataAccountListChanged += _masterFundRatioControlViewModel_SelectedMasterFundListChanged;
                _masterFundRatioControlViewModel.SelectedMasterFundListChanged += _masterFundRatioControlViewModel_SelectedMasterFundListChanged;
                _masterFundDefaultRuleViewModel.SelectedMatchingRuleChanged += _masterFundDefaultRuleViewModel_SelectedMatchingRuleChanged;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

        }
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Called when [load master fund preference control].
        /// </summary>
        internal void OnLoadMasterFundPreferenceControl()
        {
            try
            {
                //Load Master fund preferences
                LoadMasterFundPreferencesListControl();

                _calculatedPreferencesListControlViewModel.IsEnabledMenuForMF = false;
                _calculatedPreferencesListControlViewModel.IsVisibleButtonForMF = Visibility.Collapsed;
                _calculatedPreferenceDefaultRuleControlViewModel.IsVisibleButtonForMF = Visibility.Collapsed;
                _calculatedPreferenceGeneralRuleControlViewModel.IsVisibleButtonForMF = Visibility.Collapsed;
                // load master fund ratio control
                Dictionary<int, string> masterFunds = CachedDataManager.GetInstance.GetUserMasterFunds();
                DataTable masterFundRatioSet = new DataTable();
                masterFundRatioSet.Columns.Add(AllocationUIConstants.MASTER_FUND_NAME, typeof(string));
                masterFundRatioSet.Columns.Add(AllocationUIConstants.TARGET_RATIO_PCT, typeof(double));
                masterFundRatioSet.Columns.Add(AllocationUIConstants.COMPANY_MASTER_FUND_ID, typeof(int));

                foreach (int masterFundId in masterFunds.Keys)
                    masterFundRatioSet.Rows.Add(masterFunds[masterFundId], 0, masterFundId);

                _masterFundRatioControlViewModel.OnLoadMasterFundRatioControl(masterFundRatioSet, false, false);
                _masterFundDefaultRuleViewModel.OnLoadDefaultRuleControl(true);
                _masterFundDefaultRuleViewModel.IsDefaultRuleControlEnabled = false;

                _calculatedPreferenceDefaultRuleControlViewModel.OnLoadCalculatedPreferenceDefaultRuleControl(CachedDataManager.GetInstance.GetUserAccountsAsDict());
                _calculatedPreferenceGeneralRuleControlViewModel.OnLoadCalculatedPreferenceGeneralRuleControl();
                EnableDisablePrefControl(false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Loads the master fund preferences list control.
        /// </summary>
        internal void LoadMasterFundPreferencesListControl()
        {
            try
            {
                Dictionary<int, string> temp = new Dictionary<int, string>();
                temp.Add(int.MinValue, "-Select-");
                Dictionary<int, string> preferenceList = AllocationClientPreferenceManager.GetInstance.GetMasterFundPreferenceList();
                foreach (var item in preferenceList)
                {
                    temp.Add(item.Key, item.Value);
                }
                _calculatedPreferencesListControlViewModel.OnLoadCalculatedPreferencesListControl(preferenceList);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the PreviewPreference event of the _calculatedPreferencesListControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Global.EventArgs{KeyValuePair{System.Int32, System.String}}"/> instance containing the event data.</param>
        private void _masterFundPreferencesListControlViewModel_PreviewPreference(object sender, Global.EventArgs<KeyValuePair<int, string>> e)
        {
            try
            {
                ClearSelectedItemAndDisableControl();

                if (PreviewPreferenceEvent != null)
                    PreviewPreferenceEvent(this, e);
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
        /// Handles the UpdatePreferenceCacheEvent event of the _calculatedPreferencesListControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{KeyValuePair{System.Int32, System.String}}"/> instance containing the event data.</param>
        private void _masterFundPreferencesListControlViewModel_UpdatePreferenceCacheEvent(object sender, EventArgs<KeyValuePair<int, string>> e)
        {
            try
            {
                if (UpdateMFPreferenceCacheEvent != null)
                    UpdateMFPreferenceCacheEvent(this, e);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Shows the mf allocation preference preview.
        /// </summary>
        /// <param name="allocationMasterFundref">The allocation master fundref.</param>
        internal void ShowMFAllocationPrefPreview(AllocationMasterFundPreference allocationMasterFundref)
        {
            try
            {
                _masterFundDefaultRuleViewModel.SelectedProrataAccountListChanged -= _masterFundRatioControlViewModel_SelectedMasterFundListChanged;
                _masterFundRatioControlViewModel.SelectedMasterFundListChanged -= _masterFundRatioControlViewModel_SelectedMasterFundListChanged;
                _masterFundDefaultRuleViewModel.SelectedMatchingRuleChanged -= _masterFundDefaultRuleViewModel_SelectedMatchingRuleChanged;

                UpdateAllocationMFPreference(allocationMasterFundref);
                _masterFundDefaultRuleViewModel.SetDefaultRule(allocationMasterFundref.DefaultRule, true);
                _masterFundRatioControlViewModel.SetValuesInMasterFundGrid(allocationMasterFundref.MasterFundTargetPercentage);
                _masterFundDefaultRuleViewModel_SelectedMatchingRuleChanged(new object(), new EventArgs<MatchingRuleType>(allocationMasterFundref.DefaultRule.RuleType));

                _masterFundDefaultRuleViewModel.SelectedProrataAccountListChanged += _masterFundRatioControlViewModel_SelectedMasterFundListChanged;
                _masterFundRatioControlViewModel.SelectedMasterFundListChanged += _masterFundRatioControlViewModel_SelectedMasterFundListChanged;
                _masterFundDefaultRuleViewModel.SelectedMatchingRuleChanged += _masterFundDefaultRuleViewModel_SelectedMatchingRuleChanged;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the AddNewPreference event of the _masterFundPreferenceGeneralRuleControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _masterFundPreferenceGeneralRuleControlViewModel_AddNewPreference(object sender, EventArgs e)
        {
            try
            {
                _calculatedPreferenceGeneralRuleControlViewModel.AddNewCheckListWisePreference(_calculatedPreferenceDefaultRuleControlViewModel.DefaultRule, _preferenceAccountStrategyControlViewModel.AccountValueDictionary);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the PreferenceEvent event of the _masterFundPreferencesListControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{AllocationPrefOperationEventArgs}"/> instance containing the event data.</param>
        private void _masterFundPreferencesListControlViewModel_PreferenceEvent(object sender, EventArgs<AllocationPrefOperationEventArgs> e)
        {
            try
            {
                if (PreferenceEvent != null)
                    PreferenceEvent(this, new EventArgs<AllocationPrefOperationEventArgs>(e.Value));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Enable Disable Pref Control
        /// </summary>
        /// <param name="isEnable"></param>
        internal void EnableDisablePrefControl(bool isEnable)
        {
            try
            {
                _preferenceAccountStrategyControlViewModel.IsPrefAccountStrategyEnabled = isEnable;
                _calculatedPreferenceDefaultRuleControlViewModel.IsPrefDefaultRuleControlEnabled = isEnable;
                _calculatedPreferenceGeneralRuleControlViewModel.IsPrefGeneralRuleControlEnabled = isEnable;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the allocationpreference with UI values.
        /// </summary>
        /// <param name="allocationOperationPref">The allocation operation preference.</param>
        internal void UpdateAllocationpreferenceWithUIValues(AllocationOperationPreference allocationOperationPref)
        {
            try
            {
                allocationOperationPref.TryUpdateDefaultRule(CalculatedPreferenceDefaultRuleControlViewModel.DefaultRule);
                SerializableDictionary<int, CheckListWisePreference> checkListWisePrefrences = CalculatedPreferenceGeneralRuleControlViewModel.GetCheckListWisePreferencesDictionary();
                if (checkListWisePrefrences != null)
                    allocationOperationPref.UpdateCheckList(checkListWisePrefrences);
                allocationOperationPref.TryUpdateTargetPercentage(PreferenceAccountStrategyControlViewModel.GetAccountStrategyValues());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Shows the allocation preference.
        /// </summary>
        /// <param name="allocationOperationPref">The allocation operation preference.</param>
        internal void ShowAllocationPreference(AllocationOperationPreference allocationOperationPref)
        {
            try
            {
                if (allocationOperationPref.TargetPercentage != null)
                    PreferenceAccountStrategyControlViewModel.SetAccountStrategyGrid(allocationOperationPref.TargetPercentage, _accountListMasterFundAssociated);
                else
                    PreferenceAccountStrategyControlViewModel.ClearDataAccountStrategyGrid();
                CalculatedPreferenceDefaultRuleControlViewModel.SetCalculatedDefaultRuleControl(allocationOperationPref.DefaultRule);
                CalculatedPreferenceGeneralRuleControlViewModel.CheckListWisePreferences = new ObservableCollection<CheckListWisePreference>(allocationOperationPref.CheckListWisePreference.Values);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the remove update preference collection.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <param name="preferenceName">Name of the preference.</param>
        /// <param name="oldPrefKey">The old preference key.</param>
        internal void AddRemoveUpdatePrefCollection(int preferenceId, string preferenceName, int oldPrefKey)
        {
            try
            {
                _calculatedPreferencesListControlViewModel.AddRemoveUpdatePrefCollection(preferenceId, preferenceName, oldPrefKey);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Deletes the preference from collection.
        /// </summary>
        /// <param name="prefID">The preference identifier.</param>
        internal void DeletePrefFromCollection(int prefID)
        {
            try
            {
                _calculatedPreferencesListControlViewModel.DeletePrefFromCollection(prefID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the calculated preference name and identifier
        /// </summary>
        /// <param name="selectedMasterFundPrefId">The selected master fund preference identifier.</param>
        /// <param name="masterFundId">The master fund identifier.</param>
        /// <param name="masterFundName">Name of the master fund.</param>
        /// <returns></returns>
        internal KeyValuePair<int, string> GetCalculatedPrefIdAndName(int selectedMasterFundPrefId, int masterFundId, string masterFundName)
        {
            KeyValuePair<int, string> selectedCalculatedPrefIdAndName = new KeyValuePair<int, string>();
            try
            {
                if (masterFundId != int.MinValue)
                {
                    int mfCalculatedPrefId = Convert.ToInt32(string.Empty + selectedMasterFundPrefId + masterFundId);

                    if (_allocationMasterFundPref != null && _allocationMasterFundPref.MasterFundPreference.Count > 0 && _allocationMasterFundPref.MasterFundPreference.ContainsKey(masterFundId))
                        mfCalculatedPrefId = _allocationMasterFundPref.MasterFundPreference[masterFundId];

                    //Creating a unique master fund calculated preference ID
                    selectedCalculatedPrefIdAndName = new KeyValuePair<int, string>(mfCalculatedPrefId, AllocationStringConstants.MF_CALCULATED_PREF_PREFIX + mfCalculatedPrefId + "_" + masterFundName);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return selectedCalculatedPrefIdAndName;
        }

        /// <summary>
        /// Disables the controls and clear grid.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs{System.Int32, System.Double}"/> instance containing the event data.</param>
        private void DisableControlsAndClearGrid(string msg)
        {
            try
            {
                MFCalulatedPrefLabel = msg;
                _preferenceAccountStrategyControlViewModel.ClearDataAccountStrategyGrid();
                EnableDisablePrefControl(false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the allocation mf preference.
        /// </summary>
        /// <param name="allocationMasterFundPreference">The allocation master fund preference.</param>
        internal void UpdateAllocationMFPreference(AllocationMasterFundPreference allocationMasterFundPreference)
        {
            try
            {
                if (allocationMasterFundPreference != null)
                {
                    SelectedMasterFundsList = new ObservableDictionary<int, string>();
                    _allocationMasterFundPref = allocationMasterFundPreference;
                    _originalAllocationMasterFundPref = allocationMasterFundPreference.Clone();
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
        /// Sets up mf calculated preference control.
        /// </summary>
        /// <param name="accountList">The account list.</param>
        private void SetUpMFCalculatedPrefControl(Dictionary<int, string> accountList)
        {
            try
            {
                AllocationCompanyWisePref pref = AllocationClientPreferenceManager.GetInstance.GetAllocationCompanyWisePreferences();
                _preferenceAccountStrategyControlViewModel.OnLoadPreferenceAccountStrategyControl(accountList, pref);
                _calculatedPreferenceDefaultRuleControlViewModel.OnLoadCalculatedPreferenceDefaultRuleControl(accountList);
                _calculatedPreferenceGeneralRuleControlViewModel.ResetGeneralRuleControlForMF(accountList);
                _accountListMasterFundAssociated = accountList.Keys.ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the calculated preference.
        /// </summary>
        private void UpdateCalculatedPreference()
        {
            try
            {
                if (_allocationMasterFundPref != null && SelectedMFforPreviewCalculatedPref.Key != 0)
                {
                    KeyValuePair<int, string> calculatedPrefIdAndName = GetCalculatedPrefIdAndName(_allocationMasterFundPref.MasterFundPreferenceId, SelectedMFforPreviewCalculatedPref.Key, SelectedMFforPreviewCalculatedPref.Value);
                    if (!calculatedPrefIdAndName.Equals(default(KeyValuePair<int, string>)))
                    {
                        int mfCalculatedPrefId = calculatedPrefIdAndName.Key;
                        string mfCalculatedPrefName = calculatedPrefIdAndName.Value;
                        if (UpdateCalculatedPrefCacheEvent != null)
                            UpdateCalculatedPrefCacheEvent(this, new EventArgs<KeyValuePair<int, string>>(new KeyValuePair<int, string>(mfCalculatedPrefId, mfCalculatedPrefName)));
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
        /// Previews the calculated preference.
        /// </summary>
        private void PreviewCalculatedPreference()
        {
            try
            {
                Dictionary<int, string> accountList = new Dictionary<int, string>();
                if (_allocationMasterFundPref != null && SelectedMFforPreviewCalculatedPref.Key != 0)
                {
                    int masterFundId = SelectedMFforPreviewCalculatedPref.Key;
                    string masterFundName = SelectedMFforPreviewCalculatedPref.Value;
                    accountList = CommonAllocationMethods.GetMFAssociatedAccounts(masterFundId);
                    MFCalulatedPrefLabel = masterFundName;
                    if (accountList.Count > 0)
                    {
                        EnableDisablePrefControl(true);
                        SetUpMFCalculatedPrefControl(accountList);
                    }
                    else
                    {
                        EnableDisablePrefControl(false);
                        return;
                    }
                    if (_allocationMasterFundPref.MasterFundPreference.ContainsKey(masterFundId))
                    {
                        int calculatedPrefId = _allocationMasterFundPref.MasterFundPreference[masterFundId];
                        if (PreviewPreferenceSelectedMF != null)
                            PreviewPreferenceSelectedMF(this, new EventArgs<KeyValuePair<int, string>>(new KeyValuePair<int, string>(calculatedPrefId, string.Empty)));
                    }
                    else
                    {
                        if (SelectedMFforPreviewCalculatedPref.Key != 0)
                        {
                            KeyValuePair<int, string> calculatedPrefIdAndName = GetCalculatedPrefIdAndName(_allocationMasterFundPref.MasterFundPreferenceId, SelectedMFforPreviewCalculatedPref.Key, SelectedMFforPreviewCalculatedPref.Value);
                            _allocationMasterFundPref.AddUpdateMasterFundPreference(SelectedMFforPreviewCalculatedPref.Key, calculatedPrefIdAndName.Key);
                        }
                    }
                }
                else
                {
                    EnableDisablePrefControl(false);
                    DisableControlsAndClearGrid("*No master fund selected for allocation.");
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
        /// Handles the SelectedMasterFundListChanged event of the _masterFundRatioControlViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{Dictionary{System.Int32, System.String}}"/> instance containing the event data.</param>
        void _masterFundRatioControlViewModel_SelectedMasterFundListChanged(object sender, EventArgs<Dictionary<int, string>> e)
        {
            try
            {
                if (_allocationMasterFundPref != null)
                {
                    ObservableDictionary<int, string> oldMFList = SelectedMasterFundsList;
                    Dictionary<int, string> newMFList = e.Value;
                    List<int> removedMFIds = oldMFList.Keys.Except(newMFList.Keys).ToList();
                    List<int> addedMFIds = newMFList.Keys.Except(oldMFList.Keys).ToList();

                    if (removedMFIds.Count > 0)
                    {
                        foreach (int masterFundId in removedMFIds)
                        {
                            if (SelectedMasterFundsList.ContainsKey(masterFundId))
                                SelectedMasterFundsList.Remove(masterFundId);
                            if (_allocationMasterFundPref.MasterFundPreference.ContainsKey(masterFundId))
                                _allocationMasterFundPref.MasterFundPreference.Remove(masterFundId);
                        }
                        if (SelectedMasterFundsList.Count == 0)
                            SelectedMFforPreviewCalculatedPref = new KeyValuePair<int, string>();
                        else
                            SelectedMFforPreviewCalculatedPref = new KeyValuePair<int, string>(SelectedMasterFundsList.First().Key, SelectedMasterFundsList.First().Value);
                    }
                    if (addedMFIds.Count > 0)
                    {
                        foreach (int masterFundId in addedMFIds)
                        {
                            if (_originalAllocationMasterFundPref.MasterFundPreference.ContainsKey(masterFundId))
                            {
                                int calculatedPrefId = _originalAllocationMasterFundPref.MasterFundPreference[masterFundId];
                                _allocationMasterFundPref.AddUpdateMasterFundPreference(masterFundId, calculatedPrefId);
                            }
                            else
                            {
                                KeyValuePair<int, string> selectedCalculatedPrefdAndName = GetCalculatedPrefIdAndName(_allocationMasterFundPref.MasterFundPreferenceId, masterFundId, newMFList[masterFundId]);
                                if (!selectedCalculatedPrefdAndName.Equals(default(KeyValuePair<int, string>)))
                                {
                                    if (!_allocationMasterFundPref.MasterFundPreference.ContainsKey(masterFundId))
                                        _allocationMasterFundPref.AddUpdateMasterFundPreference(masterFundId, selectedCalculatedPrefdAndName.Key);
                                }
                            }
                        }
                    }
                    SelectedMasterFundsList = new ObservableDictionary<int, string>(e.Value);
                    if (SelectedMasterFundsList.Count > 0 && SelectedMFforPreviewCalculatedPref.Key == 0)
                        SelectedMFforPreviewCalculatedPref = new KeyValuePair<int, string>(SelectedMasterFundsList.First().Key, SelectedMasterFundsList.First().Value);

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
        /// Gets the name of the selected calculated preference identifier and.
        /// </summary>
        /// <param name="masterFundPrefId">The master fund preference identifier.</param>
        /// <returns></returns>
        internal KeyValuePair<int, string> GetSelectedCalculatedPrefIdAndName(int masterFundPrefId)
        {
            KeyValuePair<int, string> selectedCalcPref = new KeyValuePair<int, string>();
            try
            {
                if (SelectedMFforPreviewCalculatedPref.Key != 0)
                    selectedCalcPref = GetCalculatedPrefIdAndName(masterFundPrefId, SelectedMFforPreviewCalculatedPref.Key, SelectedMFforPreviewCalculatedPref.Value);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return selectedCalcPref;
        }

        /// <summary>
        /// Handles the SelectedMatchingRuleChanged event of the _masterFundDefaultRuleViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{Prana.Allocation.Common.Enums.MatchingRuleType}"/> instance containing the event data.</param>
        void _masterFundDefaultRuleViewModel_SelectedMatchingRuleChanged(object sender, EventArgs<MatchingRuleType> e)
        {
            try
            {
                Dictionary<int, string> selectedMFList = new Dictionary<int, string>();
                switch (e.Value)
                {
                    case MatchingRuleType.Leveling:
                    case MatchingRuleType.Prorata:
                    case MatchingRuleType.ProrataByNAV:
                        selectedMFList = (from KeyValuePair<int, string> kvp in _masterFundDefaultRuleViewModel.SelectedProrataAccount select new KeyValuePair<int, string>(kvp.Key, kvp.Value)).ToDictionary(t => t.Key, t => t.Value);
                        _masterFundRatioControlViewModel.IsMasterFundAllocationEnable = false;
                        break;

                    case MatchingRuleType.None:
                    case MatchingRuleType.SinceInception:
                    case MatchingRuleType.SinceLastChange:
                        selectedMFList = DeepCopyHelper.Clone(_masterFundRatioControlViewModel.SelectedMFList);
                        _masterFundRatioControlViewModel.IsMasterFundAllocationEnable = true;
                        break;
                }
                _masterFundRatioControlViewModel_SelectedMasterFundListChanged(sender, new EventArgs<Dictionary<int, string>>(selectedMFList));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        #region Dispose Methods and Unwire Events

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        internal void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    UnwireEvents();
                    _allocationMasterFundPref = null;
                    _originalAllocationMasterFundPref = null;
                    _accountListMasterFundAssociated = null;
                    if (_calculatedPreferencesListControlViewModel != null)
                    {
                        _calculatedPreferencesListControlViewModel.Dispose();
                        _calculatedPreferencesListControlViewModel = null;
                    }

                    if (_calculatedPreferenceDefaultRuleControlViewModel != null)
                    {
                        _calculatedPreferenceDefaultRuleControlViewModel.Dispose();
                        _calculatedPreferenceDefaultRuleControlViewModel = null;
                    }

                    if (_calculatedPreferenceGeneralRuleControlViewModel != null)
                    {
                        _calculatedPreferenceGeneralRuleControlViewModel.Dispose();
                        _calculatedPreferenceGeneralRuleControlViewModel = null;
                    }

                    if (_masterFundRatioControlViewModel != null)
                    {
                        _masterFundRatioControlViewModel.Dispose();
                        _masterFundRatioControlViewModel = null;
                    }

                    if (_masterFundDefaultRuleViewModel != null)
                    {
                        _masterFundDefaultRuleViewModel.Dispose();
                        _masterFundDefaultRuleViewModel = null;
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
        /// Unwires the events.
        /// </summary>
        void UnwireEvents()
        {
            try
            {
                _calculatedPreferencesListControlViewModel.PreviewPreferenceEvent -= _masterFundPreferencesListControlViewModel_PreviewPreference;
                _calculatedPreferencesListControlViewModel.UpdatePreferenceCacheEvent -= _masterFundPreferencesListControlViewModel_UpdatePreferenceCacheEvent;
                _calculatedPreferencesListControlViewModel.PreferenceEvent -= _masterFundPreferencesListControlViewModel_PreferenceEvent;
                _calculatedPreferenceGeneralRuleControlViewModel.AddNewPreference -= _masterFundPreferenceGeneralRuleControlViewModel_AddNewPreference;
                _masterFundDefaultRuleViewModel.SelectedProrataAccountListChanged -= _masterFundRatioControlViewModel_SelectedMasterFundListChanged;
                _masterFundRatioControlViewModel.SelectedMasterFundListChanged -= _masterFundRatioControlViewModel_SelectedMasterFundListChanged;
                _masterFundDefaultRuleViewModel.SelectedMatchingRuleChanged -= _masterFundDefaultRuleViewModel_SelectedMatchingRuleChanged;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clear Selected Item And Disable Control
        /// </summary>
        internal void ClearSelectedItemAndDisableControl()
        {
            try
            {
                _masterFundRatioControlViewModel.SelectedMasterFundItem = null;
                _allocationMasterFundPref = null;
                SelectedMasterFundsList = new ObservableDictionary<int, string>();
                SelectedMFforPreviewCalculatedPref = new KeyValuePair<int, string>();
                DisableControlsAndClearGrid(string.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion
    }
}
