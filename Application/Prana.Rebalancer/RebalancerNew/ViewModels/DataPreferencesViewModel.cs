using ExportGridsData;
using Infragistics.Windows.DataPresenter;
using Newtonsoft.Json;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.MvvmDialogs;
using Prana.Rebalancer.RebalancerNew.BussinessLogic;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.Rebalancer.RebalancerNew.Views;
using Prana.ServiceConnector;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Prana.Rebalancer.RebalancerNew.ViewModels
{
    public class DataPreferencesViewModel : RebalancerBase, IExportGridData
    {
        #region Properties

        /// <summary>
        /// Feed price list
        /// </summary>
        private ObservableCollection<KeyValueItem> _feedPriceList;
        public ObservableCollection<KeyValueItem> FeedPriceList
        {
            get { return _feedPriceList; }
            set
            {
                _feedPriceList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<KeyValueItem> _rasIncreaseDecreaseOrSetList = null;
        public ObservableCollection<KeyValueItem> RASIncreaseDecreaseOrSetList
        {
            get { return _rasIncreaseDecreaseOrSetList; }
            set
            {
                _rasIncreaseDecreaseOrSetList = value;
                OnPropertyChanged();
            }
        }

        private KeyValueItem _selectedRASIncreaseDecreaseOrSet;
        public KeyValueItem SelectedRASIncreaseDecreaseOrSet
        {
            get { return _selectedRASIncreaseDecreaseOrSet; }
            set
            {
                _selectedRASIncreaseDecreaseOrSet = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<KeyValueItemWithFlag> _assetClassCollection;
        public ObservableCollection<KeyValueItemWithFlag> AssetClassCollection
        {
            get { return _assetClassCollection; }
            set
            {
                _assetClassCollection = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<KeyValueItemWithFlag> _otherNavImpactingCollection;
        public ObservableCollection<KeyValueItemWithFlag> OtherNavImpactingCollection
        {
            get { return _otherNavImpactingCollection; }
            set
            {
                _otherNavImpactingCollection = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<KeyValueItem> _roundingTypeList;
        public ObservableCollection<KeyValueItem> RoundingTypeList
        {
            get { return _roundingTypeList; }
            set
            {
                _roundingTypeList = value;
                OnPropertyChanged();
            }
        }

        private KeyValueItem _selectedRoundingType;
        public KeyValueItem SelectedRoundingType
        {
            get { return _selectedRoundingType; }
            set
            {
                _selectedRoundingType = value;
                OnPropertyChanged();
            }
        }

        private AccountGroupDropdownPrefModel _accountGroupDropdownPref = new AccountGroupDropdownPrefModel();

        public AccountGroupDropdownPrefModel AccountGroupDropdownPref
        {
            get { return _accountGroupDropdownPref; }
            set
            {
                _accountGroupDropdownPref = value;
                OnPropertyChanged();
            }
        }

        public TradingRulesBase _tradingRulesInstance = new TradingRulesBase();
        public TradingRulesBase TradingRulesInstance
        {
            get { return _tradingRulesInstance; }
            set
            {
                _tradingRulesInstance = value;
                OnPropertyChanged();
            }
        }

        private bool _isExpanded = false;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        private bool _showSaveTradePopup = false;
        public bool ShowSaveTradePopup
        {
            get { return _showSaveTradePopup; }
            set
            {
                _showSaveTradePopup = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Nav Preference List
        /// </summary>
        private Dictionary<int, bool> _navPreferencesListFilteredDict;
        private ObservableCollection<NavPreferencesModel> _navPreferencesList;
        public ObservableCollection<NavPreferencesModel> NavPreferencesList
        {
            get { return _navPreferencesList; }
            set
            {
                _navPreferencesList = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Are all accounts selected
        /// </summary>
        public bool? AreAllAccountsSelected
        {
            get
            {

                bool? value = null;
                if (NavPreferencesList != null)
                {
                    List<NavPreferencesModel> navPreferencesModels = new List<NavPreferencesModel>();

                    foreach (var navPreferences in NavPreferencesList)
                    {
                        if (_navPreferencesListFilteredDict[navPreferences.AccountId])
                            navPreferencesModels.Add(navPreferences);
                    }

                    for (int i = 0; i < navPreferencesModels.Count; ++i)
                    {
                        if (i == 0)
                        {
                            value = navPreferencesModels[0].IsSelected;
                        }
                        else if (value != navPreferencesModels[i].IsSelected)
                        {
                            value = null;
                            break;
                        }
                    }
                }
                return value;
            }
            set
            {
                if (value == null)
                    return;
                foreach (NavPreferencesModel member in NavPreferencesList)
                    member.IsSelected = _navPreferencesListFilteredDict[member.AccountId] && value.Value;
            }
        }

        /// <summary>
        /// Selected Account
        /// </summary>
        private KeyValueItem _selectedAccount;
        public KeyValueItem SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                if (value != null)
                    UpdateNavCalculationsPreference(value.Key);
                OnPropertyChanged("SelectedAccount");
            }
        }

        private void UpdateNavCalculationsPreference(int accountId)
        {
            try
            {
                UpdateNavCalculationsPreferenceOnUI(RebalancerConstants.CONST_AssetClass, accountId);
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
        /// Accounts List
        /// </summary>
        private ObservableCollection<KeyValueItem> _accounts;
        public ObservableCollection<KeyValueItem> Accounts
        {
            get { return _accounts; }
            set
            {
                _accounts = value;
                OnPropertyChanged("Accounts");
            }
        }

        /// <summary>
        /// Selected feed price
        /// </summary>
        private KeyValueItem _selectedFeedPriceInstance;
        public KeyValueItem SelectedFeedPriceInstance
        {
            get { return _selectedFeedPriceInstance; }
            set
            {
                _selectedFeedPriceInstance = value;
                OnPropertyChanged();
            }
        }

        private StatusAndErrorMessageModel _statusAndErrorMessages;
        public StatusAndErrorMessageModel StatusAndErrorMessages
        {
            get { return _statusAndErrorMessages; }
            set
            {
                _statusAndErrorMessages = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// To set NavCalculationPreference enablity and disability
        /// </summary>
        private bool _isNavCalculationPreferenceAllowed = true;
        public bool IsNavCalculationPreferenceAllowed
        {
            get { return _isNavCalculationPreferenceAllowed; }
            set { SetProperty(ref _isNavCalculationPreferenceAllowed, value); }
        }

        /// <summary>
        /// NavCalculationBoxOpacity
        /// to set opacity level.
        /// </summary>
        private double navCalculationBoxOpacity = 1.0;
        public double NavCalculationBoxOpacity
        {
            get { return navCalculationBoxOpacity; }
            set { SetProperty(ref navCalculationBoxOpacity, value); }
        }

        /// <summary>
        /// Login user
        /// </summary>
        private CompanyUser _loginUser;
        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set { SetProperty(ref _loginUser, value); }
        }

        private ObservableCollection<KeyValueItem> _importSymbologyList;
        /// <summary>
        /// To set Import Symbology Preference DD
        /// </summary>
        public ObservableCollection<KeyValueItem> ImportSymbologyList
        {
            get { return _importSymbologyList; }
            set
            {
                _importSymbologyList = value;
                OnPropertyChanged();
            }
        }

        private KeyValueItem _selectedImportSymbology;
        /// <summary>
        /// To set Selected Import Symbology
        /// </summary>
        public KeyValueItem SelectedImportSymbology
        {
            get { return _selectedImportSymbology; }
            set
            {
                _selectedImportSymbology = value;
                OnPropertyChanged();
            }
        }

        private bool _exportDataGridForAutomation;
        public bool ExportDataGridForAutomation
        {
            get { return _exportDataGridForAutomation; }
            set { _exportDataGridForAutomation = value; OnPropertyChanged("ExportDataGridForAutomation"); }
        }

        private string _exportFilePathForAutomation;
        public string ExportFilePathForAutomation
        {
            get { return _exportFilePathForAutomation; }
            set { _exportFilePathForAutomation = value; OnPropertyChanged("ExportFilePathForAutomation"); }
        }


        #endregion

        #region Command

        /// <summary>
        /// Save pricing preference command
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// Save pricing preference command
        /// </summary>
        public DelegateCommand SelectionCheckboxClickedCommand { get; set; }

        public DelegateCommand ApplyPreferenceToAccountsCommand { get; set; }

        public DelegateCommand<object> NavPreferenceFilterChangeCommand { get; set; }

        #endregion

        public DataPreferencesViewModel(ISecurityMasterServices securityMasterInstance, IRebalancerHelper rebalancerHelperInstance)
            : base(securityMasterInstance, rebalancerHelperInstance)
        {
            try
            {
                SaveCommand = new DelegateCommand(UpdateNavCalculationPreference);
                SelectionCheckboxClickedCommand = new DelegateCommand(() => { OnPropertyChanged("AreAllAccountsSelected"); });
                ApplyPreferenceToAccountsCommand = new DelegateCommand(ApplyPreferenceToAccounts);
                NavPreferenceFilterChangeCommand =
                    new DelegateCommand<object>(UpdateFilteredPreferenceDictionary);
                StatusAndErrorMessages = new StatusAndErrorMessageModel();
                InitializeViewModel();
                InstanceManager.RegisterInstance(this);
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

        private void UpdateFilteredPreferenceDictionary(object obj)
        {
            try
            {
                ViewableRecordCollection viewableRecord = obj as ViewableRecordCollection;
                _navPreferencesListFilteredDict.Keys.ToList().ForEach(key => _navPreferencesListFilteredDict[key] = false);
                if (viewableRecord != null)
                    foreach (var item in viewableRecord)
                    {
                        NavPreferencesModel navPreference = (item as DataRecord).DataItem as NavPreferencesModel;
                        if (navPreference != null) _navPreferencesListFilteredDict[navPreference.AccountId] = true;
                    }
                OnPropertyChanged("AreAllAccountsSelected");
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

        private void ApplyPreferenceToAccounts()
        {
            try
            {
                Dictionary<int, bool> checkedNavImpactedPreferences = new Dictionary<int, bool>();
                foreach (var navImpactComponent in OtherNavImpactingCollection)
                {
                    checkedNavImpactedPreferences.Add(navImpactComponent.Key, navImpactComponent.Flag);
                }

                foreach (NavPreferencesModel member in NavPreferencesList)
                {
                    if (member.IsSelected)
                    {
                        member.IsIncludeCash = checkedNavImpactedPreferences[(int)RebalancerEnums.ItemsImpactingNav.Cash];
                        member.IsIncludeAccruals = checkedNavImpactedPreferences[(int)RebalancerEnums.ItemsImpactingNav.Accruals];
                        member.IsIncludeOtherAssetsMarketValue = checkedNavImpactedPreferences[(int)RebalancerEnums.ItemsImpactingNav.OtherAssetsMarketValue];
                        member.IsIncludeSwapNavAdjustment = checkedNavImpactedPreferences[(int)RebalancerEnums.ItemsImpactingNav.SwapNavAdjustment];
                        member.IsIncludeUnrealizedPnlOfSwaps = checkedNavImpactedPreferences[(int)RebalancerEnums.ItemsImpactingNav.UnrealizedPandLofSwaps];
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

        /// <summary>
        /// UpdateNav Calculations Preference On UI
        /// </summary>
        /// <param name="preferenceKey"></param>
        /// <param name="accountId"></param>
        private void UpdateNavCalculationsPreferenceOnUI(string preferenceKey, int accountId)
        {
            string navPreference =
                RebalancerCache.Instance.GetRebalPreference(preferenceKey, accountId);
            if (string.IsNullOrEmpty(navPreference)) return;
            int temp = 0;
            List<int> navPreferenceList = navPreference.Split(',')
                .Where(m => int.TryParse(m, out temp))
                .Select(int.Parse)
                .ToList();
            switch (preferenceKey)
            {
                case RebalancerConstants.CONST_AssetClass:
                    if (AssetClassCollection != null)
                        foreach (var prop in AssetClassCollection)
                            prop.Flag = navPreferenceList.Contains(prop.Key);
                    break;
            }
        }

        /// <summary>
        /// Update Nav Calculation Preference
        /// </summary>
        private void UpdateNavCalculationPreference()
        {
            try
            {
                if (ValidateNavCalculationPreference() && ValidateAccountGroupVisibilityPreferences())
                {
                    Dictionary<int, string> navImpactingPreferenceDictionary = new Dictionary<int, string>();

                    foreach (NavPreferencesModel memberModel in NavPreferencesList)
                    {
                        navImpactingPreferenceDictionary.Add(memberModel.AccountId, JsonConvert.SerializeObject(memberModel));
                    }

                    bool otherNavImpactsSaveResult = false, updatePricingFieldPreference = false, updateAccountGroupVisibility = false
                        , updateRASPrefrence = false, updateTradingRulesPreference = false, updateIsExpanded = false, updateShowSaveTradePopup = false
                        , updateRoundingType = false, updateImportSymbology = false;

                    Parallel.Invoke(
                        () =>
                        {
                            otherNavImpactsSaveResult = RebalancerServiceApi.GetInstance()
                                .UpdateRebalPreferencesForAllAccounts(RebalancerConstants.CONST_OtherItemsImpactingNAV, navImpactingPreferenceDictionary);
                        },
                        () =>
                        {
                            updatePricingFieldPreference = UpdatePricingFieldPreference();
                        },
                        () =>
                        {
                            updateAccountGroupVisibility = UpdateAccountGroupVisibilityPreference();
                        },
                        () =>
                        {
                            updateRASPrefrence = UpdateRASPrefrence();
                        },
                        () =>
                        {
                            updateTradingRulesPreference = UpdateTradingRulesPreference();
                        },
                        () =>
                        {
                            updateIsExpanded = UpdateIsExpanded();
                        },
                        () =>
                        {
                            updateShowSaveTradePopup = UpdateShowSaveTradePopup();
                        },
                        () =>
                        {
                            updateRoundingType = UpdateRoundingType();
                        },
                        () =>
                        {
                            updateImportSymbology = UpdateImportSymbologyPreference();
                        }
                        );

                    if (otherNavImpactsSaveResult && updatePricingFieldPreference && updateAccountGroupVisibility && updateRASPrefrence
                        && updateTradingRulesPreference && updateRoundingType && updateIsExpanded && updateShowSaveTradePopup && updateImportSymbology)
                    {
                        RebalancerCache.Instance.AddOrUpdateRebalPreference(0, RebalancerConstants.CONST_RebalPricingFeld, SelectedFeedPriceInstance.Key.ToString());
                        RebalancerCache.Instance.AddOrUpdateRebalPreference(0, RebalancerConstants.CONST_RebalRASPrefrence, SelectedRASIncreaseDecreaseOrSet.Key.ToString());
                        RebalancerCache.Instance.AddOrUpdateRebalPreference(0, RebalancerConstants.CONST_RebalAccountGroupVisibilityPref, JsonConvert.SerializeObject(AccountGroupDropdownPref));
                        RebalancerCache.Instance.AddOrUpdateRebalPreference(0, RebalancerConstants.CONST_RebalTradingRulesPref, JsonConvert.SerializeObject(TradingRulesInstance));
                        RebalancerCache.Instance.AddOrUpdateRebalPreference(0, RebalancerConstants.CONST_RebalExpandAcrossSecurities, IsExpanded.ToString());
                        RebalancerCache.Instance.AddOrUpdateRebalPreference(0, RebalancerConstants.CONST_RebalShowSaveTradePopup, ShowSaveTradePopup.ToString());
                        RebalancerCache.Instance.AddOrUpdateRebalPreference(0, RebalancerConstants.CONST_RebalRoundingType, SelectedRoundingType.Key.ToString());
                        RebalancerCache.Instance.AddOrUpdateRebalPreference(0, RebalancerConstants.CONST_RebalImportSymbologyPreference, SelectedImportSymbology.Key.ToString());

                        RebalancerCache.Instance.AddOrUpdateRebalPreferenceForAllAccounts(RebalancerConstants.CONST_OtherItemsImpactingNAV, navImpactingPreferenceDictionary);
                        DialogService.DialogServiceInstance.ShowMessageBox(this, "Rebalance Preferences Saved Successfully.",
                            RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK,
                            MessageBoxImage.Information,
                            MessageBoxResult.OK, true);
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

        /// <summary>
        /// Validate Nav Calculation Preference
        /// </summary>
        /// <returns></returns>
        private bool ValidateNavCalculationPreference()
        {
            bool isValid = true;
            StringBuilder errors = new StringBuilder();
            if (NavPreferencesList == null)
            {
                isValid = false;
                errors.AppendLine("NavPreferencesList not available");
            }
            if (!isValid)
                DialogService.DialogServiceInstance.ShowMessageBox(this, errors.ToString(),
                    RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning,
                    MessageBoxResult.OK, true);
            return isValid;
        }

        /// <summary>
        /// Validate Nav Calculation Preference
        /// </summary>
        /// <returns></returns>
        private bool ValidateAccountGroupVisibilityPreferences()
        {
            bool isValid = true;
            StringBuilder errors = new StringBuilder();
            if (!AccountGroupDropdownPref.IsAccountIncluded && !AccountGroupDropdownPref.IsMasterFundIncluded && !AccountGroupDropdownPref.IsCustomGroupIncluded)
            {
                isValid = false;
                errors.AppendLine("Please select atleast one Account/Group Visiblity Preferences.");
            }
            if (!isValid)
                DialogService.DialogServiceInstance.ShowMessageBox(this, errors.ToString(),
                    RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning,
                    MessageBoxResult.OK, true);
            return isValid;
        }

        private bool UpdateAccountGroupVisibilityPreference()
        {
            try
            {
                RebalPreferencesDto rebalPreferences = new RebalPreferencesDto
                {
                    AccountId = 0,
                    PreferenceKey = RebalancerConstants.CONST_RebalAccountGroupVisibilityPref,
                    PreferenceValue = JsonConvert.SerializeObject(AccountGroupDropdownPref)
                };
                bool result = RebalancerServiceApi.GetInstance()
                    .UpdateRebalPreferences(rebalPreferences);
                return result;

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        private bool UpdateTradingRulesPreference()
        {
            try
            {
                RebalPreferencesDto rebalPreferences = new RebalPreferencesDto
                {
                    AccountId = 0,
                    PreferenceKey = RebalancerConstants.CONST_RebalTradingRulesPref,
                    PreferenceValue = JsonConvert.SerializeObject(TradingRulesInstance)
                };
                bool result = RebalancerServiceApi.GetInstance()
                    .UpdateRebalPreferences(rebalPreferences);
                return result;

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Update Pricing Field Preference
        /// </summary>
        private bool UpdatePricingFieldPreference()
        {
            try
            {
                if (SelectedFeedPriceInstance != null)
                {
                    RebalPreferencesDto rebalPreferences = new RebalPreferencesDto
                    {
                        AccountId = 0,
                        PreferenceKey = RebalancerConstants.CONST_RebalPricingFeld,
                        PreferenceValue = SelectedFeedPriceInstance.Key.ToString()
                    };
                    bool result = RebalancerServiceApi.GetInstance()
                        .UpdateRebalPreferences(rebalPreferences);
                    return result;
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
            return false;
        }

        private bool UpdateRASPrefrence()
        {
            try
            {
                if (SelectedRASIncreaseDecreaseOrSet != null)
                {
                    RebalPreferencesDto rebalPreferences = new RebalPreferencesDto
                    {
                        AccountId = 0,
                        PreferenceKey = RebalancerConstants.CONST_RebalRASPrefrence,
                        PreferenceValue = SelectedRASIncreaseDecreaseOrSet.Key.ToString()
                    };
                    bool result = RebalancerServiceApi.GetInstance()
                        .UpdateRebalPreferences(rebalPreferences);
                    return result;
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
            return false;
        }

        private bool UpdateIsExpanded()
        {
            try
            {
                RebalPreferencesDto rebalPreferences = new RebalPreferencesDto
                {
                    AccountId = 0,
                    PreferenceKey = RebalancerConstants.CONST_RebalExpandAcrossSecurities,
                    PreferenceValue = IsExpanded.ToString()
                };
                bool result = RebalancerServiceApi.GetInstance()
                    .UpdateRebalPreferences(rebalPreferences);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        private bool UpdateShowSaveTradePopup()
        {
            try
            {
                RebalPreferencesDto rebalPreferences = new RebalPreferencesDto
                {
                    AccountId = 0,
                    PreferenceKey = RebalancerConstants.CONST_RebalShowSaveTradePopup,
                    PreferenceValue = ShowSaveTradePopup.ToString()
                };
                bool result = RebalancerServiceApi.GetInstance()
                    .UpdateRebalPreferences(rebalPreferences);
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        private bool UpdateRoundingType()
        {
            try
            {
                if (SelectedRoundingType != null)
                {
                    RebalPreferencesDto rebalPreferences = new RebalPreferencesDto
                    {
                        AccountId = 0,
                        PreferenceKey = RebalancerConstants.CONST_RebalRoundingType,
                        PreferenceValue = SelectedRoundingType.Key.ToString()
                    };
                    bool result = RebalancerServiceApi.GetInstance()
                        .UpdateRebalPreferences(rebalPreferences);
                    return result;
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
            return false;
        }

        /// <summary>
        /// Initialize View Model
        /// </summary>
        public void InitializeViewModel()
        {
            try
            {
                FeedPriceList = GetFeedPriceList();
                RASIncreaseDecreaseOrSetList = GetRASList();
                ImportSymbologyList = GetImportSymbologyList();
                RoundingTypeList = RebalancerCommon.Instance.GetKeyValueItemFromEnum<RebalancerEnums.RoundingTypes>();
                LoginUser = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser;
                //check if basket compliance is enabled, if enabled then disabled NavCalculationpreference
                if (ComplianceCacheManager.CheckIsBasketComplianceEnabled(_loginUser.CompanyUserID))
                {
                    IsNavCalculationPreferenceAllowed = false;
                    NavCalculationBoxOpacity = 0.7;
                }

                int selectedFeedPrice;
                int selectedRASPref;
                bool _isExpanded;
                bool _showSaveTradePopup;
                int selectedRoundingType;
                int selectedImportSymbology;
                if (int.TryParse(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalPricingFeld, 0),
                    out selectedFeedPrice))
                    SelectedFeedPriceInstance = FeedPriceList.FirstOrDefault(price =>
                        price.Key.Equals(selectedFeedPrice));
                if (int.TryParse(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalRASPrefrence, 0),
                    out selectedRASPref))
                    SelectedRASIncreaseDecreaseOrSet = RASIncreaseDecreaseOrSetList.FirstOrDefault(pref =>
                        pref.Key.Equals(selectedRASPref));
                if (bool.TryParse(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalExpandAcrossSecurities, 0),
                out _isExpanded))
                    IsExpanded = _isExpanded;
                if (bool.TryParse(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalShowSaveTradePopup, 0),
                out _showSaveTradePopup))
                    ShowSaveTradePopup = _showSaveTradePopup;
                if (int.TryParse(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalRoundingType, 0),
                    out selectedRoundingType))
                {
                    SelectedRoundingType = RoundingTypeList.FirstOrDefault(type =>
                       type.Key.Equals(selectedRoundingType));
                }
                else
                {
                    SelectedRoundingType = RoundingTypeList.SingleOrDefault(p => p.Key == 3);
                }
                if (int.TryParse(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalImportSymbologyPreference, 0),
                    out selectedImportSymbology))
                {
                    SelectedImportSymbology = ImportSymbologyList.FirstOrDefault(p => p.Key.Equals(selectedImportSymbology));
                }
                else
                {
                    SelectedImportSymbology = ImportSymbologyList.First();
                }

                OtherNavImpactingCollection = RebalancerCommon.Instance
                    .GetKeyValueItemWithFlagFromEnum<RebalancerEnums.ItemsImpactingNav>();
                AssetClassCollection = RebalancerCommon.Instance
                    .GetKeyValueItemWithFlagFromEnum<RebalancerEnums.AssetClass>();
                Accounts = new ObservableCollection<KeyValueItem>(CachedDataManager.GetInstance.GetUserAccountsAsDict()
                    .Select(p => new KeyValueItem { ItemValue = p.Value, Key = p.Key }).ToList());
                LoadDataInNavPreferenceList();
                UpdateNavCalculationsPreference(0);
                AccountGroupDropdownPref = JsonConvert.DeserializeObject<AccountGroupDropdownPrefModel>(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalAccountGroupVisibilityPref, 0));
                TradingRulesBase tradingRulesPref = JsonConvert.DeserializeObject<TradingRulesBase>(RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_RebalTradingRulesPref, 0));
                if (tradingRulesPref != null)
                {
                    TradingRulesInstance = tradingRulesPref;
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

        private ObservableCollection<KeyValueItem> GetRASList()
        {
            var rasPrefrences = Enum.GetNames(typeof(RebalancerEnums.RASIncreaseDecreaseOrSet));
            ObservableCollection<KeyValueItem> rasPrefrenceList = new ObservableCollection<KeyValueItem>();
            int i = 0;
            foreach (var pref in rasPrefrences)
            {
                rasPrefrenceList.Add(new KeyValueItem { Key = i, ItemValue = pref });
                i++;
            }
            return rasPrefrenceList;
        }


        private void LoadDataInNavPreferenceList()
        {
            NavPreferencesList = new ObservableCollection<NavPreferencesModel>();
            _navPreferencesListFilteredDict = new Dictionary<int, bool>();
            foreach (var account in Accounts)
            {
                string jsonNavPreferenceModel =
                    RebalancerCache.Instance.GetRebalPreference(RebalancerConstants.CONST_OtherItemsImpactingNAV,
                        account.Key);
                NavPreferencesModel navPreferenceInstance = new NavPreferencesModel();
                if (!string.IsNullOrWhiteSpace(jsonNavPreferenceModel))
                {

                    navPreferenceInstance = JsonConvert.DeserializeObject<NavPreferencesModel>(jsonNavPreferenceModel);
                    navPreferenceInstance.AccountId = account.Key;
                }
                else
                {
                    navPreferenceInstance.AccountId = account.Key;
                    navPreferenceInstance.IsIncludeCash = true;
                }
                _navPreferencesListFilteredDict.Add(account.Key, true);
                NavPreferencesList.Add(navPreferenceInstance);
            }

        }

        /// <summary>
        /// Get Feed Price List
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<KeyValueItem> GetFeedPriceList()
        {
            var feedPrices = Enum.GetNames(typeof(SelectedFeedPrice));
            ObservableCollection<KeyValueItem> feedPriceList = new ObservableCollection<KeyValueItem>();
            int i = 0;
            foreach (var feedPrice in feedPrices)
            {
                if (feedPrice != SelectedFeedPrice.None.ToString() && feedPrice != SelectedFeedPrice.AskOrBid.ToString())
                {
                    feedPriceList.Add(new KeyValueItem { Key = i, ItemValue = feedPrice });
                    i++;
                }
            }
            return feedPriceList;
        }

        /// <summary>
        /// This method is to get import symbology list for preference DD
        /// </summary>
        /// <returns>A list of import symbology for preferences</returns>
        private ObservableCollection<KeyValueItem> GetImportSymbologyList()
        {
            try
            {
                var importSymbology = Enum.GetNames(typeof(RebalancerEnums.ImportSymbology));
                ObservableCollection<KeyValueItem> importSymbologyList = new ObservableCollection<KeyValueItem>();
                int i = 0;
                foreach (var symbology in importSymbology)
                {
                    importSymbologyList.Add(new KeyValueItem { Key = i, ItemValue = symbology });
                    i++;
                }
                return importSymbologyList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// This method is to update import symbology preference
        /// </summary>
        /// <returns></returns>
        private bool UpdateImportSymbologyPreference()
        {
            try
            {
                if (SelectedImportSymbology != null)
                {
                    RebalPreferencesDto rebalPreferences = new RebalPreferencesDto
                    {
                        AccountId = 0,
                        PreferenceKey = RebalancerConstants.CONST_RebalImportSymbologyPreference,
                        PreferenceValue = SelectedImportSymbology.Key.ToString()
                    };
                    bool result = RebalancerServiceApi.GetInstance()
                        .UpdateRebalPreferences(rebalPreferences);
                    return result;
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
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_feedPriceList != null)
                    _feedPriceList = null;
                if (_assetClassCollection != null)
                    _assetClassCollection = null;

                _selectedFeedPriceInstance = null;

                if (_otherNavImpactingCollection != null)
                    _otherNavImpactingCollection = null;

                if (_accounts != null)
                    _accounts = null;
            }
            InstanceManager.ReleaseInstance(typeof(DataPreferencesViewModel));
        }

        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            try
            {
                if (ExportDataGridForAutomation == true)
                    ExportDataGridForAutomation = false;
                ExportFilePathForAutomation = filePath;
                ExportDataGridForAutomation = true;
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
    }
}
