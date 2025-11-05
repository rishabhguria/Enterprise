using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Enums;
using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    public class EditPreferenceControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [close edit preference UI].
        /// </summary>
        public event EventHandler CloseEditPrefUI;

        /// <summary>
        /// Occurs when [preview allocation data].
        /// </summary>
        public event EventHandler PreviewAllocationData;

        /// <summary>
        /// Occurs when [update status allocation].
        /// </summary>
        public event EventHandler<EventArgs<string>> UpdateStatusAllocation;

        #endregion Events

        #region Members

        /// <summary>
        /// The _singleton
        /// </summary>
        private static EditPreferenceControlViewModel _singleton;

        /// <summary>
        /// The _locker
        /// </summary>
        private static readonly object _locker = new object();

        /// <summary>
        /// The _allocation type collection
        /// </summary>
        private ObservableCollection<EnumerationValue> _allocationTypeCollection;

        /// <summary>
        /// The _selected remainder allocation Account
        /// </summary>
        private KeyValuePair<int, string> _selectedRemainderAllocationAccount;

        /// <summary>
        /// The _matching rule type collection
        /// </summary>
        private ObservableCollection<EnumerationValue> _matchingRuleTypeCollection;

        /// <summary>
        /// The Accounts
        /// </summary>
        private ObservableDictionary<int, string> _accounts;

        /// <summary>
        /// The _accounts for prorata
        /// </summary>
        private ObservableDictionary<int, string> _accountsForProrata;

        /// <summary>
        /// The _is match closing checked
        /// </summary>
        private bool _isMatchClosingChecked;

        /// <summary>
        /// The _selected allocation type
        /// </summary>
        private EnumerationValue _selectedAllocationType;

        /// <summary>
        /// The _selected matching rule type
        /// </summary>
        private EnumerationValue _selectedMatchingRuleType;

        /// <summary>
        /// The _selected prorata Account
        /// </summary>
        private ObservableCollection<object> _selectedProrataAccount;

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
        /// The matching rule type
        /// </summary>
        EnumerationValue matchingRuleType;

        /// <summary>
        /// The _selected prorata date
        /// </summary>
        private DateTime _selectedProrataDate;

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
        /// Gets or sets the accounts for prorata.
        /// </summary>
        /// <value>
        /// The accounts for prorata.
        /// </value>
        public ObservableDictionary<int, string> AccountsForProrata
        {
            get { return _accountsForProrata; }
            set
            {
                _accountsForProrata = value;
                RaisePropertyChangedEvent("AccountsForProrata");
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
        /// Gets or sets a value indicating whether this instance is match closing checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is match closing checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsMatchClosingChecked
        {
            get { return _isMatchClosingChecked; }
            set
            {
                _isMatchClosingChecked = value;
                RaisePropertyChangedEvent("IsMatchClosingChecked");
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
                else
                {
                    matchingRuleType = _selectedMatchingRuleType;
                }

                RaisePropertyChangedEvent("SelectedMatchingRuleType");
            }
        }

        /// <summary>
        /// Gets or sets the selected prorata Account.
        /// </summary>
        /// <value>
        /// The selected prorata Account.
        /// </value>
        public ObservableCollection<object> SelectedProrataAccount
        {
            get { return _selectedProrataAccount; }
            set
            {
                _selectedProrataAccount = value;
                RaisePropertyChangedEvent("SelectedProrataAccount");
            }
        }

        /// <summary>
        /// Gets or sets the selected prorata date.
        /// </summary>
        /// <value>
        /// The selected prorata date.
        /// </value>
        public DateTime SelectedProrataDate
        {
            get { return _selectedProrataDate; }
            set
            {
                _selectedProrataDate = value;
                RaisePropertyChangedEvent("SelectedProrataDate");
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
        /// Gets or sets the apply button.
        /// </summary>
        /// <value>
        /// The apply button.
        /// </value>
        public RelayCommand<object> ApplyButton { get; set; }

        /// <summary>
        /// Gets or sets the close edit preference UI.
        /// </summary>
        /// <value>
        /// The close edit preference UI.
        /// </value>
        public RelayCommand<object> CloseEditPreferenceUI { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EditPreferenceControlViewModel"/> class.
        /// </summary>
        public EditPreferenceControlViewModel()
        {
            try
            {
                ApplyButton = new RelayCommand<object>((parameter) => OnApplyButton(parameter));
                CloseEditPreferenceUI = new RelayCommand<object>((parameter) => OnCloseEditPreferenceUI(parameter));

            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors

        #region Methods

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
                    if (_singleton != null)
                        _singleton = null;
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
        /// Ges the allocation rule.
        /// </summary>
        /// <returns></returns>
        internal AllocationRule GetAllocationRule()
        {
            AllocationRule rule = new AllocationRule();
            try
            {
                rule.BaseType = (AllocationBaseType)Enum.Parse(typeof(AllocationBaseType), SelectedAllocationType.Value.ToString());
                rule.RuleType = (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), SelectedMatchingRuleType.Value.ToString());
                rule.PreferenceAccountId = _selectedRemainderAllocationAccount.Key;
                rule.ProrataAccountList = CommonAllocationMethods.GetList(_selectedProrataAccount);
                rule.MatchClosingTransaction = IsMatchClosingChecked ? MatchClosingTransactionType.CompletePortfolio : MatchClosingTransactionType.None;
                rule.ProrataDaysBack = (DateTime.Now.Date - SelectedProrataDate).Days;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return rule;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static EditPreferenceControlViewModel GetInstance()
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new EditPreferenceControlViewModel();
                    }
                }
            }
            return _singleton;
        }

        /// <summary>
        /// Called when [apply button].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnApplyButton(object parameter)
        {
            try
            {
                bool isValid = false;
                string errorMsg = string.Empty;
                AllocationRule rule = GetAllocationRule();
                isValid = rule.IsValid(out errorMsg);
                if (isValid)
                {
                    Window editPrefWindow = parameter as Window;
                    if (UpdateStatusAllocation != null)
                        UpdateStatusAllocation(this, new EventArgs<string>("Applied allocation rule for current allocation"));
                    if (PreviewAllocationData != null)
                        PreviewAllocationData(this, EventArgs.Empty);
                    editPrefWindow.Close();
                }
                else
                {
                    MessageBox.Show(errorMsg, AllocationClientConstants.PREFERENCES_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Called when [close edit preference UI].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnCloseEditPreferenceUI(object parameter)
        {
            try
            {
                if (SelectedMatchingRuleType.DisplayText.Equals(MatchingRuleType.Prorata.ToString()) && SelectedProrataAccount.Count == 0)
                {
                    MessageBox.Show("Accounts for Prorata are no selected. Your changes will be reverted.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                    SelectedMatchingRuleType = MatchingRuleTypeCollection.FirstOrDefault(x => (MatchingRuleType)x.Value == (MatchingRuleType)(int)matchingRuleType.Value);
                }
                if (CloseEditPrefUI != null)
                    CloseEditPrefUI(this, null);

            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Sets the default rules.
        /// </summary>
        /// <param name="defaultRule">The default rules.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        internal void SetDefaultRules(AllocationRule defaultRules)
        {
            try
            {
                Dictionary<int, string> remainderAllocationAccountList = new Dictionary<int, string>();
                remainderAllocationAccountList.Add(-1, "Select");
                foreach (KeyValuePair<int, string> kvp in CachedDataManager.GetInstance.GetUserAccountsAsDict().OrderBy(x => x.Value))
                {
                    remainderAllocationAccountList.Add(kvp.Key, kvp.Value);
                }

                AllocationTypeCollection = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(AllocationBaseType)));
                MatchingRuleTypeCollection = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(MatchingRuleType)));
                Accounts = new ObservableDictionary<int, string>(remainderAllocationAccountList);
                AccountsForProrata = new ObservableDictionary<int, string>(CachedDataManager.GetInstance.GetUserAccountsAsDict().OrderBy(x => x.Value));
                MatchClosingCollection = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(DefaultBoolean)));
                if (defaultRules != null)
                {
                    IsMatchClosingChecked = defaultRules.MatchClosingTransaction != MatchClosingTransactionType.None;
                    DateUptoDays = defaultRules.ProrataDaysBack;
                    SelectedAllocationType = AllocationTypeCollection.FirstOrDefault(x => (AllocationBaseType)x.Value == defaultRules.BaseType);
                    SelectedMatchingRuleType = MatchingRuleTypeCollection.FirstOrDefault(x => (MatchingRuleType)x.Value == defaultRules.RuleType);
                    if (remainderAllocationAccountList.ContainsKey(defaultRules.PreferenceAccountId))
                        SelectedRemainderAllocationAccount = new KeyValuePair<int, string>(defaultRules.PreferenceAccountId, remainderAllocationAccountList[defaultRules.PreferenceAccountId]);
                    if (defaultRules.ProrataAccountList != null)
                        SelectedProrataAccount = CommonAllocationMethods.GetCollection(defaultRules.ProrataAccountList, CachedDataManager.GetInstance.GetUserAccountsAsDict());
                    else
                        SelectedProrataAccount = new ObservableCollection<object>();

                    SelectedProrataDate = System.DateTime.Now.AddDays(-DateUptoDays);
                }

                ApplyPermissionFilters();
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal void ApplyPermissionFilters()
        {
            if (!AllocationSubModulePermission.IsLevelingPermitted)
                MatchingRuleTypeCollection.Remove(MatchingRuleTypeCollection.FirstOrDefault(item => (int)item.Value == (int)MatchingRuleType.Leveling));
            if (!AllocationSubModulePermission.IsProrataByNavPermitted)
                MatchingRuleTypeCollection.Remove(MatchingRuleTypeCollection.FirstOrDefault(item => (int)item.Value == (int)MatchingRuleType.ProrataByNAV));
        }

        #endregion Methods
    }
}
