using Infragistics.Windows.DataPresenter;
using Prana.Allocation.Client.Controls.Preferences.Views;
using Prana.Allocation.Client.EventArguments;
using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class CalculatedPreferenceGeneralRuleControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [add new preference].
        /// </summary>
        public event EventHandler AddNewPreference;

        #endregion Events

        #region Members

        /// <summary>
        /// The _is disable cell
        /// </summary>
        private bool _isDisableCell;

        /// <summary>
        /// The accounts list.
        /// </summary>
        /// <value>
        /// The account list.
        /// </value>
        public Dictionary<int, string> _accountList;

        /// <summary>
        /// The _check list wise preferences
        /// </summary>
        private ObservableCollection<CheckListWisePreference> _checkListWisePreferences;

        /// <summary>
        /// The _exchange operator
        /// </summary>
        private ObservableCollection<EnumerationValue> _exchangeOperator;

        /// <summary>
        /// The _orderside operator
        /// </summary>
        private ObservableCollection<EnumerationValue> _orderSideOperator;

        /// <summary>
        /// The _exchange list
        /// </summary>
        private ObservableDictionary<int, string> _exchangeList;

        /// <summary>
        /// The order side list
        /// </summary>
        private ObservableDictionary<string, string> _orderSideList;

        /// <summary>
        /// The _asset operator
        /// </summary>
        private ObservableCollection<EnumerationValue> _assetOperator;

        /// <summary>
        /// The _asset list
        /// </summary>
        private ObservableDictionary<int, string> _assetList;

        /// <summary>
        /// The _PR operator
        /// </summary>
        private ObservableCollection<EnumerationValue> _prOperator;

        /// <summary>
        /// The _PR list
        /// </summary>
        private string _prList;

        /// <summary>
        /// The _num
        /// </summary>
        Random _num = new Random(1000);

        /// <summary>
        /// The _is preference general rule control enabled
        /// </summary>
        private bool _isPrefGeneralRuleControlEnabled = true;

        /// <summary>
        /// updates general rule preference UI
        /// </summary>
        private static UpdatePreferenceUIViewModel _updatePreferenceUI = null;

        /// <summary>
        /// Check Visibility of Master Fund
        /// </summary>
        private Visibility _isVisibleButtonForMF = Visibility.Visible;

        public bool _exportGrid;

        public bool ExportGrid
        {
            get { return _exportGrid; }
            set
            {
                _exportGrid = value;
                RaisePropertyChangedEvent("ExportGrid");
            }
        }

        private string _exportFilePathForAutomation;
        public string ExportFilePathForAutomation
        {
            get { return _exportFilePathForAutomation; }
            set { _exportFilePathForAutomation = value; RaisePropertyChangedEvent("ExportFilePathForAutomation"); }
        }
        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the asset list.
        /// </summary>
        /// <value>
        /// The asset list.
        /// </value>
        public ObservableDictionary<int, string> AssetList
        {
            get { return _assetList; }
            set
            {
                _assetList = value;
                RaisePropertyChangedEvent("AssetList");
            }
        }

        /// <summary>
        /// Gets or sets the asset operator.
        /// </summary>
        /// <value>
        /// The asset operator.
        /// </value>
        public ObservableCollection<EnumerationValue> AssetOperator
        {
            get { return _assetOperator; }
            set
            {
                _assetOperator = value;
                RaisePropertyChangedEvent("AssetOperator");
            }
        }

        /// <summary>
        /// Gets or sets the check list wise preferences.
        /// </summary>
        /// <value>
        /// The check list wise preferences.
        /// </value>
        public ObservableCollection<CheckListWisePreference> CheckListWisePreferences
        {
            get { return _checkListWisePreferences; }
            set
            {
                _checkListWisePreferences = value;
                RaisePropertyChangedEvent("CheckListWisePreferences");
            }
        }

        /// <summary>
        /// Gets or sets the exchange list.
        /// </summary>
        /// <value>
        /// The exchange list.
        /// </value>
        public ObservableDictionary<int, string> ExchangeList
        {
            get { return _exchangeList; }
            set
            {
                _exchangeList = value;
                RaisePropertyChangedEvent("ExchangeList");
            }
        }

        /// <summary>
        /// Gets or sets the exchange operator.
        /// </summary>
        /// <value>
        /// The exchange operator.
        /// </value>
        public ObservableCollection<EnumerationValue> ExchangeOperator
        {
            get { return _exchangeOperator; }
            set
            {
                _exchangeOperator = value;
                RaisePropertyChangedEvent("ExchangeOperator");
            }
        }

        /// <summary>
        /// Gets or sets the order side operator.
        /// </summary>
        /// <value>
        /// The order side operator.
        /// </value>
        public ObservableCollection<EnumerationValue> OrderSideOperator
        {
            get { return _orderSideOperator; }
            set
            {
                _orderSideOperator = value;
                RaisePropertyChangedEvent("OrderSideOperator");
            }
        }

        /// <summary>
        /// Gets or sets the order side list.
        /// </summary>
        /// <value>
        /// The order side list.
        /// </value>
        public ObservableDictionary<string, string> OrderSideList
        {
            get { return _orderSideList; }
            set
            {
                _orderSideList = value;
                RaisePropertyChangedEvent("OrderSideList");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is disable cell.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disable cell; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisableCell
        {
            get { return _isDisableCell; }
            set
            {
                _isDisableCell = value;
                RaisePropertyChangedEvent("IsDisableCell");
            }
        }

        /// <summary>
        /// Gets or sets the pr list.
        /// </summary>
        /// <value>
        /// The pr list.
        /// </value>
        public string PrList
        {
            get { return _prList; }
            set
            {
                _prList = value;
                RaisePropertyChangedEvent("PrList");
            }
        }

        /// <summary>
        /// Gets or sets the pr operator.
        /// </summary>
        /// <value>
        /// The pr operator.
        /// </value>
        public ObservableCollection<EnumerationValue> PrOperator
        {
            get { return _prOperator; }
            set
            {
                _prOperator = value;
                RaisePropertyChangedEvent("PrOperator");
            }
        }

        /// <summary>
        /// Gets or sets the update general rule preference.
        /// </summary>
        /// <value>
        /// The update general rule preference.
        /// </value>
        public RelayCommand<object> UpdateGeneralRulePreference { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is preference general rule control enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is preference general rule control enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrefGeneralRuleControlEnabled
        {
            get { return _isPrefGeneralRuleControlEnabled; }
            set
            {
                _isPrefGeneralRuleControlEnabled = value;
                RaisePropertyChangedEvent("IsPrefGeneralRuleControlEnabled");
            }
        }



        /// <summary>
        /// Gets or sets the is visible button for mf.
        /// </summary>
        /// <value>
        /// The is visible button for mf.
        /// </value>
        public Visibility IsVisibleButtonForMF
        {
            get { return _isVisibleButtonForMF; }
            set
            {
                _isVisibleButtonForMF = value;
                RaisePropertyChangedEvent("IsVisibleButtonForMF");
            }
        }


        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the add check list wise preference.
        /// </summary>
        /// <value>
        /// The add check list wise preference.
        /// </value>
        public RelayCommand<object> AddCheckListWisePreference { get; set; }

        /// <summary>
        /// Gets or sets the remove check list wise preference.
        /// </summary>
        /// <value>
        /// The remove check list wise preference.
        /// </value>
        public RelayCommand<object> RemoveCheckListWisePreference { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedPreferenceGeneralRuleControlViewModel"/> class.
        /// </summary>
        public CalculatedPreferenceGeneralRuleControlViewModel()
        {
            try
            {
                AddCheckListWisePreference = new RelayCommand<object>((parameter) => AddDataRecord(parameter));
                RemoveCheckListWisePreference = new RelayCommand<object>((parameter) => RemoveDataRecord(parameter));
                UpdateGeneralRulePreference = new RelayCommand<object>((parameter) => UpdatePreferenceRule(parameter, _accountList, IsVisibleButtonForMF));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Updates the preference rule.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object UpdatePreferenceRule(object parameter, Dictionary<int, string> accountsList, Visibility IsVisibleButtonForMF)
        {
            try
            {
                if (_updatePreferenceUI == null)
                {
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    if (parameter != null)
                    {
                        ShowUpdatePreferenceUI(viewModel => dialogService.ShowDialog<UpdatePreferenceUI>(this, viewModel), parameter as CheckListWisePreference, accountsList, IsVisibleButtonForMF);
                    }
                }
                else
                    _updatePreferenceUI.BringToFront = WindowState.Normal;
                //MessageBox.Show("Add/Update Allocation Preference Form is Already Opened", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Shows the update preference UI.
        /// </summary>
        /// <param name="showUpdatePreferenceUI">The show update preference UI.</param>
        /// <param name="selectedCheckList">The selected check list.</param>
        private static void ShowUpdatePreferenceUI(Action<UpdatePreferenceUIViewModel> showUpdatePreferenceUI, CheckListWisePreference selectedCheckList, Dictionary<int, string> accountsList, Visibility IsVisibleButtonForMF)
        {
            try
            {
                _updatePreferenceUI = new UpdatePreferenceUIViewModel(selectedCheckList, accountsList, IsVisibleButtonForMF);
                _updatePreferenceUI.OnFormCloseButtonEvent += updateAllocPrefUI_OnFormCloseButtonEvent;
                showUpdatePreferenceUI(_updatePreferenceUI);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the OnFormCloseButtonEvent event of the addorupdateAllocPrefUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        static void updateAllocPrefUI_OnFormCloseButtonEvent(object sender, EventArgs e)
        {
            try
            {
                if (_updatePreferenceUI != null)
                    _updatePreferenceUI = null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Adds the data record.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object AddDataRecord(object parameter)
        {
            try
            {
                if (AddNewPreference != null)
                    AddNewPreference(this, null);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Adds the new check list wise preference.
        /// </summary>
        /// <param name="defaultRule">The allocation rule.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        internal void AddNewCheckListWisePreference(AllocationRule defaultRule, SerializableDictionary<int, AccountValue> targetPercentage)
        {
            try
            {
                CheckListWisePreference preference = new CheckListWisePreference();
                int newID = _num.Next() * (-1);
                preference.ChecklistId = newID;
                if (defaultRule != null)
                    preference.Rule = defaultRule;
                preference.TargetPercentage = targetPercentage;
                CheckListWisePreferences.Add(preference);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Applies the bulk changes.
        /// </summary>
        /// <param name="e">The <see cref="ApplyBulkChangeEventArgs"/> instance containing the event data.</param>
        internal void ApplyBulkChanges(ApplyBulkChangeEventArgs e)
        {
            try
            {
                if (CheckListWisePreferences != null)
                {
                    foreach (CheckListWisePreference chkListWisePref in CheckListWisePreferences)
                    {
                        if (e.allocationBaseChecked)
                            chkListWisePref.Rule.BaseType = e.Rule.BaseType;
                        if (e.matchingRuleChecked)
                            chkListWisePref.Rule.RuleType = e.Rule.RuleType;
                        if (e.preferencedAccountChecked)
                            chkListWisePref.Rule.PreferenceAccountId = e.Rule.PreferenceAccountId;
                        if (e.Rule.MatchClosingTransaction != MatchClosingTransactionType.None)
                            chkListWisePref.Rule.MatchClosingTransaction = e.Rule.MatchClosingTransaction;
                        if (e.Rule.RuleType == MatchingRuleType.Prorata)
                            chkListWisePref.Rule.ProrataDaysBack = e.Rule.ProrataDaysBack;
                        if (e.matchingRuleChecked && (e.Rule.RuleType == MatchingRuleType.Prorata || e.Rule.RuleType == MatchingRuleType.Leveling || e.Rule.RuleType == MatchingRuleType.ProrataByNAV))
                            chkListWisePref.Rule.ProrataAccountList = e.Rule.ProrataAccountList;
                    }
                    CheckListWisePreferences = new ObservableCollection<CheckListWisePreference>(CheckListWisePreferences);
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

        /// <summary>
        /// Gets the check list wise preferences dictionary.
        /// </summary>
        /// <returns></returns>
        internal SerializableDictionary<int, CheckListWisePreference> GetCheckListWisePreferencesDictionary()
        {
            SerializableDictionary<int, CheckListWisePreference> preferencesDict = new SerializableDictionary<int, CheckListWisePreference>();
            try
            {
                foreach (CheckListWisePreference preference in CheckListWisePreferences)
                {
                    if (preference.Rule.ProrataAccountList != null && preference.Rule.ProrataAccountList.Count == 0)
                        preference.Rule.ProrataAccountList = null;
                    preferencesDict.Add(preference.ChecklistId, preference);
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
            return preferencesDict;
        }

        /// <summary>
        /// Called when [load calculated preference general rule control].
        /// </summary>
        internal void OnLoadCalculatedPreferenceGeneralRuleControl()
        {
            try
            {
                _accountList = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                CheckListWisePreferences = new ObservableCollection<CheckListWisePreference>();
                ExchangeOperator = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(CustomOperator)));
                AssetOperator = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(CustomOperator)));
                PrOperator = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(CustomOperator)));
                OrderSideOperator = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(CustomOperator)));
                ExchangeList = new ObservableDictionary<int, string>(CommonDataCache.CachedDataManager.GetInstance.GetAllExchanges());
                OrderSideList = new ObservableDictionary<string, string>(CommonAllocationMethods.GetOrderSides());
                AssetList = new ObservableDictionary<int, string>(CommonDataCache.CachedDataManager.GetInstance.GetAllAssets());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// set account list to be used in update preference UI and reset checkList pref
        /// </summary>
        /// <param name="accountList"></param>
        internal void ResetGeneralRuleControlForMF(Dictionary<int, string> accountList)
        {
            try
            {
                _accountList = new Dictionary<int, string>(accountList);
                CheckListWisePreferences = new ObservableCollection<CheckListWisePreference>();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Removes the data record.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object RemoveDataRecord(object parameter)
        {
            try
            {
                CheckListWisePreference pref = ((parameter as DataRecord).DataItem) as CheckListWisePreference;
                if (pref != null)
                    CheckListWisePreferences.Remove(pref);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

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
                    _checkListWisePreferences = null;
                    _exchangeOperator = null;
                    _orderSideOperator = null;
                    _exchangeList = null;
                    _orderSideList = null;
                    _assetOperator = null;
                    _assetList = null;
                    _prOperator = null;

                    AddCheckListWisePreference = null;
                    RemoveCheckListWisePreference = null;

                    if (_updatePreferenceUI != null)
                    {
                        _updatePreferenceUI.OnFormCloseButtonEvent -= updateAllocPrefUI_OnFormCloseButtonEvent;
                        _updatePreferenceUI.Dispose();
                        _updatePreferenceUI = null;
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

        #endregion Methods
    }
}
