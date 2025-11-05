using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prana.Allocation.Client.Controls.Common.ViewModels
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Prana.Allocation.Client.ViewModelBase" />
    public class DefaultRuleControlViewModel : ViewModelBase
    {
        #region Members

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
        /// The _accounts
        /// </summary>
        private ObservableDictionary<int, string> _accounts;

        /// <summary>
        /// The _selected prorata Account
        /// </summary>
        private ObservableCollection<object> _selectedProrataAccount;

        /// <summary>
        /// The _selected remainder allocation Account
        /// </summary>
        private KeyValuePair<int, string> _selectedRemainderAllocationAccount;

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
        /// The _is match closing checked
        /// </summary>
        private bool _isMatchClosingChecked;

        /// <summary>
        /// The _accounts for prorata
        /// </summary>
        private ObservableDictionary<int, string> _accountsForProrata;

        /// <summary>
        /// The is master fund rule
        /// </summary>
        private bool _isMasterFundRule;

        /// <summary>
        /// The is default rule control enabled
        /// </summary>
        private bool _isDefaultRuleControlEnabled;

        #endregion Members

        #region Events

        /// <summary>
        /// Occurs when [preview preference event].
        /// </summary>
        public event EventHandler<EventArgs<Dictionary<int, string>>> SelectedProrataAccountListChanged;

        /// <summary>
        /// Occurs when [selected matching rule changed].
        /// </summary>
        public event EventHandler<EventArgs<MatchingRuleType>> SelectedMatchingRuleChanged;

        #endregion

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
                else if (_selectedMatchingRuleType.DisplayText == EnumHelper.GetDescription(MatchingRuleType.None) && IsMasterFundRule)
                {
                    SelectedAllocationType = AllocationTypeCollection.FirstOrDefault(x => (AllocationBaseType)x.Value == AllocationBaseType.CumQuantity);
                }
                if (SelectedMatchingRuleChanged != null)
                    SelectedMatchingRuleChanged(this, new EventArgs<MatchingRuleType>(EnumHelper.GetValueFromEnumDescription<MatchingRuleType>(_selectedMatchingRuleType.DisplayText)));
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
            get
            {
                return _selectedProrataAccount ?? new ObservableCollection<object>();
            }
            set
            {
                _selectedProrataAccount = value;

                if (SelectedMatchingRuleType != null && (SelectedMatchingRuleType.DisplayText.Equals(EnumHelper.GetDescription(MatchingRuleType.Leveling))
                    || SelectedMatchingRuleType.DisplayText.Equals(EnumHelper.GetDescription(MatchingRuleType.Prorata))
                    || SelectedMatchingRuleType.DisplayText.Equals(EnumHelper.GetDescription(MatchingRuleType.ProrataByNAV))))
                {
                    RaiseProrataAccountListEvent(_selectedProrataAccount);
                }
                RaisePropertyChangedEvent("SelectedProrataAccount");
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
        /// Gets or sets a value indicating whether this instance is master fund rule.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is master fund rule; otherwise, <c>false</c>.
        /// </value>
        public bool IsMasterFundRule
        {
            get { return _isMasterFundRule; }
            set
            {
                _isMasterFundRule = value;
                RaisePropertyChangedEvent("IsMasterFundRule");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default rule control enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is default rule control enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefaultRuleControlEnabled
        {
            get { return _isDefaultRuleControlEnabled; }
            set
            {
                _isDefaultRuleControlEnabled = value;
                RaisePropertyChangedEvent("IsDefaultRuleControlEnabled");
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRuleControlViewModel"/> class.
        /// </summary>
        public DefaultRuleControlViewModel()
        {
            try
            {
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
        /// Gets the default rule.
        /// </summary>
        /// <returns>the default rule</returns>
        internal AllocationRule GetDefaultRule()
        {
            AllocationRule defaultRule = new AllocationRule();
            try
            {
                List<int> prorataAccountList = (from KeyValuePair<int, string> kvp in SelectedProrataAccount select kvp.Key).ToList();
                defaultRule = new AllocationRule()
                {
                    BaseType = (AllocationBaseType)Enum.Parse(typeof(AllocationBaseType), SelectedAllocationType.Value.ToString()),
                    MatchClosingTransaction = IsMasterFundRule ? (MatchClosingTransactionType)Enum.Parse(typeof(MatchClosingTransactionType), SelectedMatchClosing.Value.ToString()) :
                    (Convert.ToBoolean(IsMatchClosingChecked) ? MatchClosingTransactionType.CompletePortfolio : MatchClosingTransactionType.None),
                    RuleType = (MatchingRuleType)Enum.Parse(typeof(MatchingRuleType), SelectedMatchingRuleType.Value.ToString()),
                    PreferenceAccountId = SelectedRemainderAllocationAccount.Key,
                    ProrataDaysBack = DateUptoDays,
                    ProrataAccountList = prorataAccountList
                };
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return defaultRule;
        }

        /// <summary>
        /// Sets the default rule.
        /// </summary>
        /// <param name="defaultRule">The default rule.</param>
        /// <param name="isMasterFundRule">if set to <c>true</c> [is master fund rule].</param>
        internal void SetDefaultRule(AllocationRule defaultRule, bool isMasterFundRule)
        {
            try
            {
                if (defaultRule != null)
                {
                    IsDefaultRuleControlEnabled = true;
                    //SelectedMatchClosing used for dropdown on in masterFund Rule
                    SelectedMatchClosing = MatchClosingCollection.FirstOrDefault(x => (MatchClosingTransactionType)x.Value == defaultRule.MatchClosingTransaction);
                    //IsMatchClosingChecked used for Default Rule in Preferences Window
                    IsMatchClosingChecked = defaultRule.MatchClosingTransaction != MatchClosingTransactionType.None;
                    DateUptoDays = defaultRule.ProrataDaysBack;
                    SelectedAllocationType = AllocationTypeCollection.FirstOrDefault(x => (AllocationBaseType)x.Value == defaultRule.BaseType);
                    SelectedMatchingRuleType = MatchingRuleTypeCollection.FirstOrDefault(x => (MatchingRuleType)x.Value == defaultRule.RuleType);
                    if (Accounts.ContainsKey(defaultRule.PreferenceAccountId))
                        SelectedRemainderAllocationAccount = new KeyValuePair<int, string>(defaultRule.PreferenceAccountId, Accounts[defaultRule.PreferenceAccountId]);
                    if (defaultRule.ProrataAccountList != null)
                    {
                        if (isMasterFundRule)
                            SelectedProrataAccount = CommonAllocationMethods.GetCollection(defaultRule.ProrataAccountList, CachedDataManager.GetInstance.GetAllMasterFunds());
                        else
                            SelectedProrataAccount = CommonAllocationMethods.GetCollection(defaultRule.ProrataAccountList, CachedDataManager.GetInstance.GetUserAccountsAsDict());
                    }
                    else
                        SelectedProrataAccount = new ObservableCollection<object>();
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Called when [load default rule control].
        /// </summary>
        /// <param name="isMasterFundRule">if set to <c>true</c> [is master fund rule].</param>
        internal void OnLoadDefaultRuleControl(bool isMasterFundRule)
        {
            try
            {
                IsMasterFundRule = isMasterFundRule;
                List<EnumerationValue> matchingRuleTypeCollection = Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(MatchingRuleType));
                ObservableDictionary<int, string> fundList = new ObservableDictionary<int, string>();

                //update fund list for rule
                if (IsMasterFundRule)
                {
                    fundList = new ObservableDictionary<int, string>(CachedDataManager.GetInstance.GetUserMasterFunds().OrderBy(x => x.Value));
                    matchingRuleTypeCollection.RemoveAll(x => (x.Value.Equals((int)MatchingRuleType.SinceInception) || x.Value.Equals((int)MatchingRuleType.SinceLastChange)));
                }
                else
                {
                    fundList = new ObservableDictionary<int, string>(CachedDataManager.GetInstance.GetAccounts().OrderBy(x => x.Value));
                }

                //add select for remainder allocation list
                Dictionary<int, string> remainderAllocationList = new Dictionary<int, string>();
                remainderAllocationList.Add(-1, "Select");
                foreach (int key in fundList.Keys)
                    remainderAllocationList.Add(key, fundList[key]);

                //add values to dropdowns
                Accounts = new ObservableDictionary<int, string>(remainderAllocationList);
                AccountsForProrata = new ObservableDictionary<int, string>(fundList);
                AllocationTypeCollection = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(AllocationBaseType)));
                MatchingRuleTypeCollection = new ObservableCollection<EnumerationValue>(matchingRuleTypeCollection);
                MatchClosingCollection = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(MatchClosingTransactionType)));

                //set defaults
                SelectedRemainderAllocationAccount = new KeyValuePair<int, string>(-1, "Select");
                SelectedProrataAccount = new ObservableCollection<object>();
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
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
                    _allocationTypeCollection = null;
                    _selectedAllocationType = null;
                    _matchingRuleTypeCollection = null;
                    _selectedMatchingRuleType = null;
                    _accounts = null;
                    _selectedProrataAccount = null;
                    _matchClosingCollection = null;
                    _selectedMatchClosing = null;
                    _accountsForProrata = null;
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
        /// Raises the prorata account list event.
        /// </summary>
        /// <param name="selectedProrataAccount">The selected prorata account.</param>
        private void RaiseProrataAccountListEvent(ObservableCollection<object> selectedProrataAccount)
        {
            try
            {
                Dictionary<int, string> selectedProrataList = (from KeyValuePair<int, string> kvp in selectedProrataAccount select new KeyValuePair<int, string>(kvp.Key, kvp.Value)).ToDictionary(t => t.Key, t => t.Value);
                if (SelectedProrataAccountListChanged != null)
                    SelectedProrataAccountListChanged(this, new EventArgs<Dictionary<int, string>>(selectedProrataList));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Methods
    }
}
