using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Enums;
using Prana.Allocation.Client.EventArguments;
using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Preferences.ViewModels
{
    public class BulkChangeControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [apply preference].
        /// </summary>
        public event ApplyPreHandler ApplyPref;

        /// <summary>
        /// Occurs when [close bulk change form].
        /// </summary>
        public event EventHandler CloseBulkChangeForm;

        #endregion Events

        #region Delegates

        /// <summary>
        /// ApplyPreHandler
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ApplyBulkChangeEventArgs"/> instance containing the event data.</param>
        public delegate void ApplyPreHandler(Object sender, ApplyBulkChangeEventArgs e);

        #endregion Delegates

        #region Members

        /// <summary>
        /// The _singleton
        /// </summary>
        private static BulkChangeControlViewModel _singleton;

        /// <summary>
        /// The _locker
        /// </summary>
        private static readonly object _locker = new object();

        /// <summary>
        /// The _allocation preference collection
        /// </summary>
        private ObservableDictionary<int, string> _allocationPreferenceCollection;

        /// <summary>
        /// The _selected allocation preferences
        /// </summary>
        private ObservableCollection<object> _selectedAllocationPreferences;

        /// <summary>
        /// The _allocation type collection
        /// </summary>
        private ObservableCollection<EnumerationValue> _allocationTypeCollection;

        /// <summary>
        /// The _selected allocation type
        /// </summary>
        private EnumerationValue _selectedAllocationType;

        /// <summary>
        /// The _matching rule type collection
        /// </summary>
        private ObservableCollection<EnumerationValue> _matchingRuleTypeCollection;

        /// <summary>
        /// The _selected matching rule type
        /// </summary>
        private EnumerationValue _selectedMatchingRuleType;

        /// <summary>
        /// The _selected Account
        /// </summary>
        private ObservableCollection<object> _selectedAccount;

        /// <summary>
        /// The _selected remainder allocation Account
        /// </summary>
        private KeyValuePair<int, string> _selectedRemainderAllocationAccount;

        /// <summary>
        /// The _accounts
        /// </summary>
        private ObservableDictionary<int, string> _accounts;

        /// <summary>
        /// The _match closing collection
        /// </summary>
        private ObservableCollection<EnumerationValue> _matchClosingCollection;

        /// <summary>
        /// The _selected match closing
        /// </summary>
        private EnumerationValue _selectedMatchClosing;

        /// <summary>
        /// The _date upto days
        /// </summary>
        private int _dateUptoDays;

        /// <summary>
        /// The _is selected preference
        /// </summary>
        private bool _isSelectedPreference;

        /// <summary>
        /// The _is default rule
        /// </summary>
        private bool _isDefaultRule;

        /// <summary>
        /// The _is allocation method
        /// </summary>
        private bool _isAllocationMethod;

        /// <summary>
        /// The _is remainder allocation
        /// </summary>
        private bool _isRemainderAllocation;

        /// <summary>
        /// The _is target percent
        /// </summary>
        private bool _isTargetPercent;

        /// <summary>
        /// The _is match closing transaction
        /// </summary>
        private bool _isMatchClosingTransaction;

        /// <summary>
        /// The bring to front
        /// </summary>
        private WindowState _bringToFront;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the Accounts.
        /// </summary>
        /// <value>
        /// The Accounts.
        /// </value>
        public ObservableDictionary<int, string> Accounts
        {
            get { return _accounts; }
            set
            {
                _accounts = value;
                RaisePropertyChangedEvent("Accounts");
            }
        }

        /// <summary>
        /// Gets or sets the allocation preference collection.
        /// </summary>
        /// <value>
        /// The allocation preference collection.
        /// </value>
        public ObservableDictionary<int, string> AllocationPreferenceCollection
        {
            get { return _allocationPreferenceCollection; }
            set
            {
                _allocationPreferenceCollection = value;
                RaisePropertyChangedEvent("AllocationPreferenceCollection");
            }
        }

        /// <summary>
        /// Gets or sets the allocation type collection.
        /// </summary>
        /// <value>
        /// The allocation type collection.
        /// </value>
        public ObservableCollection<EnumerationValue> AllocationTypeCollection
        {
            get { return _allocationTypeCollection; }
            set
            {
                _allocationTypeCollection = value;
                RaisePropertyChangedEvent("AllocationTypeCollection");
            }
        }

        /// <summary>
        /// Gets or sets the date upto days.
        /// </summary>
        /// <value>
        /// The date upto days.
        /// </value>
        public int DateUptoDays
        {
            get { return _dateUptoDays; }
            set
            {
                _dateUptoDays = value;
                RaisePropertyChangedEvent("DateUptoDays");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is allocation method.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is allocation method; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllocationMethod
        {
            get { return _isAllocationMethod; }
            set
            {
                _isAllocationMethod = value;
                RaisePropertyChangedEvent("IsAllocationMethod");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default rule.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is default rule; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefaultRule
        {
            get { return _isDefaultRule; }
            set
            {
                _isDefaultRule = value;
                RaisePropertyChangedEvent("IsDefaultRule");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is match closing transaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is match closing transaction; otherwise, <c>false</c>.
        /// </value>
        public bool IsMatchClosingTransaction
        {
            get { return _isMatchClosingTransaction; }
            set
            {
                _isMatchClosingTransaction = value;
                RaisePropertyChangedEvent("IsMatchClosingTransaction");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is remainder allocation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is remainder allocation; otherwise, <c>false</c>.
        /// </value>
        public bool IsRemainderAllocation
        {
            get { return _isRemainderAllocation; }
            set
            {
                _isRemainderAllocation = value;
                RaisePropertyChangedEvent("IsRemainderAllocation");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected preference.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is selected preference; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelectedPreference
        {
            get { return _isSelectedPreference; }
            set
            {
                _isSelectedPreference = value;
                RaisePropertyChangedEvent("IsSelectedPreference");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is target percent.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is target percent; otherwise, <c>false</c>.
        /// </value>
        public bool IsTargetPercent
        {
            get { return _isTargetPercent; }
            set
            {
                _isTargetPercent = value;
                RaisePropertyChangedEvent("IsTargetPercent");
            }
        }

        /// <summary>
        /// Gets or sets the match closing collection.
        /// </summary>
        /// <value>
        /// The match closing collection.
        /// </value>
        public ObservableCollection<EnumerationValue> MatchClosingCollection
        {
            get { return _matchClosingCollection; }
            set
            {
                _matchClosingCollection = value;
                RaisePropertyChangedEvent("MatchClosingCollection");
            }
        }

        /// <summary>
        /// Gets or sets the matching rule type collection.
        /// </summary>
        /// <value>
        /// The matching rule type collection.
        /// </value>
        public ObservableCollection<EnumerationValue> MatchingRuleTypeCollection
        {
            get { return _matchingRuleTypeCollection; }
            set
            {
                _matchingRuleTypeCollection = value;
                RaisePropertyChangedEvent("MatchingRuleTypeCollection");
            }
        }

        /// <summary>
        /// Gets or sets the selected Account.
        /// </summary>
        /// <value>
        /// The selected Account.
        /// </value>
        public ObservableCollection<object> SelectedAccount
        {
            get { return ((_selectedAccount != null) ? _selectedAccount : new ObservableCollection<object>()); }
            set
            {
                _selectedAccount = value;
                RaisePropertyChangedEvent("SelectedAccount");
            }
        }

        /// <summary>
        /// Gets or sets the selected allocation preferences.
        /// </summary>
        /// <value>
        /// The selected allocation preferences.
        /// </value>
        public ObservableCollection<object> SelectedAllocationPreferences
        {
            get { return ((_selectedAllocationPreferences != null) ? _selectedAllocationPreferences : new ObservableCollection<object>()); }
            set
            {
                _selectedAllocationPreferences = value;
                RaisePropertyChangedEvent("SelectedAllocationPreferences");
            }
        }

        /// <summary>
        /// Gets or sets the type of the selected allocation.
        /// </summary>
        /// <value>
        /// The type of the selected allocation.
        /// </value>
        public EnumerationValue SelectedAllocationType
        {
            get { return _selectedAllocationType; }
            set
            {
                _selectedAllocationType = value;
                RaisePropertyChangedEvent("SelectedAllocationType");
            }
        }

        /// <summary>
        /// Gets or sets the selected match closing.
        /// </summary>
        /// <value>
        /// The selected match closing.
        /// </value>
        public EnumerationValue SelectedMatchClosing
        {
            get { return _selectedMatchClosing; }
            set
            {
                _selectedMatchClosing = value;
                RaisePropertyChangedEvent("SelectedMatchClosing");
            }
        }

        /// <summary>
        /// Gets or sets the type of the selected matching rule.
        /// </summary>
        /// <value>
        /// The type of the selected matching rule.
        /// </value>
        public EnumerationValue SelectedMatchingRuleType
        {
            get { return _selectedMatchingRuleType; }
            set
            {
                _selectedMatchingRuleType = value;

                if (_selectedMatchingRuleType.DisplayText == MatchingRuleType.Prorata.ToString() || _selectedMatchingRuleType.DisplayText == EnumHelper.GetDescription(MatchingRuleType.ProrataByNAV))
                {
                    SelectedAllocationType = AllocationTypeCollection.FirstOrDefault(x => (AllocationBaseType)x.Value == AllocationBaseType.CumQuantity);
                }
                else if (_selectedMatchingRuleType.DisplayText == MatchingRuleType.Leveling.ToString())
                {
                    SelectedAllocationType = AllocationTypeCollection.FirstOrDefault(x => (AllocationBaseType)x.Value == AllocationBaseType.Notional);
                }

                RaisePropertyChangedEvent("SelectedMatchingRuleType");
            }
        }

        /// <summary>
        /// Gets or sets the selected remainder allocation Account.
        /// </summary>
        /// <value>
        /// The selected remainder allocation Account.
        /// </value>
        public KeyValuePair<int, string> SelectedRemainderAllocationAccount
        {
            get { return _selectedRemainderAllocationAccount; }
            set
            {
                _selectedRemainderAllocationAccount = value;
                RaisePropertyChangedEvent("SelectedRemainderAllocationAccount");
            }
        }

        /// <summary>
        /// Gets or sets the bring to front.
        /// </summary>
        /// <value>
        /// The bring to front.
        /// </value>
        public WindowState BringToFront
        {
            get { return _bringToFront; }
            set
            {
                if (_bringToFront == WindowState.Minimized)
                    _bringToFront = value;
                else
                {
                    if (value == WindowState.Minimized)
                        _bringToFront = value;
                    else
                    {
                        WindowState currentState = _bringToFront;
                        _bringToFront = WindowState.Minimized;
                        RaisePropertyChangedEvent("BringToFront");
                        _bringToFront = currentState;
                    }
                }
                RaisePropertyChangedEvent("BringToFront");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the apply preference bulk change.
        /// </summary>
        /// <value>
        /// The apply preference bulk change.
        /// </value>
        public RelayCommand<object> ApplyPreferenceBulkChangeCommand { get; set; }

        /// <summary>
        /// Gets or sets the bulk change control view loaded.
        /// </summary>
        /// <value>
        /// The bulk change control view loaded.
        /// </value>
        public RelayCommand<object> BulkChangeControlViewLoadedCommand { get; set; }

        /// <summary>
        /// Gets or sets the close bulk change UI.
        /// </summary>
        /// <value>
        /// The close bulk change UI.
        /// </value>
        public RelayCommand<object> CloseBulkChangeUI { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkChangeControlViewModel"/> class.
        /// </summary>
        public BulkChangeControlViewModel()
        {
            try
            {
                BulkChangeControlViewLoadedCommand = new RelayCommand<object>((parameter) => BulkChangeControlViewLoaded(parameter));
                ApplyPreferenceBulkChangeCommand = new RelayCommand<object>((parameter) => ApplyPreferenceBulkChange(parameter));
                CloseBulkChangeUI = new RelayCommand<object>((parameter) => CloseBulkChangeUIClick(parameter));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Singleton

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static BulkChangeControlViewModel GetInstance()
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new BulkChangeControlViewModel();
                    }
                }
            }
            return _singleton;
        }

        #endregion Singleton

        #region Methods

        /// <summary>
        /// Applies the preference bulk change.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object ApplyPreferenceBulkChange(object parameter)
        {
            try
            {
                Window bulkChangeWindow = parameter as Window;

                bool isValid = false;
                string errorMsg = string.Empty;
                ApplyBulkChangeEventArgs e = new ApplyBulkChangeEventArgs();
                e.ApplyOnDefaultRule = IsDefaultRule;
                e.ApplyOnSelectedPref = IsSelectedPreference;
                e.allocationBaseChecked = IsAllocationMethod;
                e.matchingRuleChecked = IsTargetPercent;
                e.preferencedAccountChecked = IsRemainderAllocation;
                e.matchPortfolioPostionChecked = IsMatchClosingTransaction;

                if (e.Rule == null)
                    e.Rule = new AllocationRule();
                e.Rule.MatchClosingTransaction = SelectedMatchClosing.DisplayText.ToLower().Equals("true") ? MatchClosingTransactionType.CompletePortfolio : MatchClosingTransactionType.None;
                e.Rule.PreferenceAccountId = SelectedRemainderAllocationAccount.Key;
                e.Rule.BaseType = (AllocationBaseType)SelectedAllocationType.Value;
                e.Rule.RuleType = (MatchingRuleType)SelectedMatchingRuleType.Value;
                if (e.Rule.RuleType == MatchingRuleType.Prorata)
                {
                    e.Rule.ProrataAccountList = CommonAllocationMethods.GetList(SelectedAccount);
                    e.Rule.ProrataDaysBack = DateUptoDays;
                }
                else if (e.Rule.RuleType == MatchingRuleType.Leveling || e.Rule.RuleType == MatchingRuleType.ProrataByNAV)
                {
                    e.Rule.ProrataAccountList = CommonAllocationMethods.GetList(SelectedAccount);
                    e.Rule.ProrataDaysBack = 0;
                }
                else
                {
                    e.Rule.ProrataAccountList = null;
                    e.Rule.ProrataDaysBack = 0;
                }
                e.PreferenceList = CommonAllocationMethods.GetList(SelectedAllocationPreferences);

                //if (IsTargetPercent)
                // {
                isValid = e.Rule.IsValid(out errorMsg);
                //   if (!isValid)
                // MessageBox.Show(errorMsg, AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                // }
                if (isValid)
                {
                    if (ApplyPref != null)
                        ApplyPref(this, e);
                }
                else
                    MessageBox.Show(errorMsg, AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                bulkChangeWindow.Close();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Bulks the change control view loaded.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object BulkChangeControlViewLoaded(object parameter)
        {
            try
            {
                Dictionary<int, string> accountCollection = new Dictionary<int, string>();
                accountCollection.Add(-1, "Select");
                accountCollection.AddRangeThreadSafely(CachedDataManager.GetInstance.GetUserAccountsAsDict().OrderBy(x => x.Value));
                Dictionary<int, string> preferenceCollection = AllocationClientPreferenceManager.GetInstance.GetPreferencesList();
                if (preferenceCollection.ContainsKey(int.MinValue))
                    preferenceCollection.Remove(int.MinValue);
                AllocationTypeCollection = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(AllocationBaseType)));
                MatchingRuleTypeCollection = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(MatchingRuleType)));
                Accounts = new ObservableDictionary<int, string>(accountCollection);
                MatchClosingCollection = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(DefaultBoolean)));
                AllocationPreferenceCollection = new ObservableDictionary<int, string>(preferenceCollection);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Close BulkChangeUI Click
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private object CloseBulkChangeUIClick(object parameter)
        {
            try
            {
                if (CloseBulkChangeForm != null)
                    CloseBulkChangeForm(this, null);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        #endregion Methods

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
                {
                    throw;
                }
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
                    _singleton = null;
                    _allocationPreferenceCollection = null;
                    _selectedAllocationPreferences = null;
                    _allocationTypeCollection = null;
                    _selectedAllocationType = null;
                    _matchingRuleTypeCollection = null;
                    _selectedMatchingRuleType = null;
                    _selectedAccount = null;
                    _accounts = null;
                    _matchClosingCollection = null;
                    _selectedMatchClosing = null;
                }
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