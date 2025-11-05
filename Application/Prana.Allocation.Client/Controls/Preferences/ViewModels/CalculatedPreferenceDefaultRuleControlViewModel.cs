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
    public class CalculatedPreferenceDefaultRuleControlViewModel : ViewModelBase
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
        /// The _selected prorata Account
        /// </summary>
        private ObservableCollection<object> _selectedProrataAccount;

        /// <summary>
        /// The _accounts
        /// </summary>
        private ObservableDictionary<int, string> _accounts;

        /// <summary>
        /// The Accounts for prorata
        /// </summary>
        private ObservableDictionary<int, string> _accountsForProrata;

        /// <summary>
        /// The _date upto days
        /// </summary>
        private int _dateUptoDays;

        /// <summary>
        /// The _is preference default rule control enabled
        /// </summary>
        private bool _isPrefDefaultRuleControlEnabled = true;

        /// <summary>
        /// The _is match closing checked
        /// </summary>
        private bool _isMatchClosingChecked;

        /// <summary>
        /// The _is visible button for mf
        /// </summary>
        private Visibility _isVisibleButtonForMF = Visibility.Visible;

        #endregion Members

        #region Properties

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
        /// Gets the default rule.
        /// </summary>
        /// <value>
        /// The default rule.
        /// </value>
        public AllocationRule DefaultRule
        {
            get { return GetDefaultRule(); }
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
                return ((_selectedProrataAccount != null) ? _selectedProrataAccount : new ObservableCollection<object>());
            }
            set
            {
                _selectedProrataAccount = value;
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
        /// Gets or sets a value indicating whether this instance is preference default rule control enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is preference default rule control enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrefDefaultRuleControlEnabled
        {
            get { return _isPrefDefaultRuleControlEnabled; }
            set
            {
                _isPrefDefaultRuleControlEnabled = value;
                RaisePropertyChangedEvent("IsPrefDefaultRuleControlEnabled");
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedPreferenceDefaultRuleControlViewModel"/> class.
        /// </summary>
        public CalculatedPreferenceDefaultRuleControlViewModel()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Applies the bulk change.
        /// </summary>
        /// <param name="e">The <see cref="ApplyBulkChangeEventArgs"/> instance containing the event data.</param>
        internal void ApplyBulkChange(ApplyBulkChangeEventArgs e)
        {
            try
            {
                Dictionary<int, string> remainderAllocationAccountList = new Dictionary<int, string>();
                remainderAllocationAccountList.Add(-1, "Select");
                foreach (KeyValuePair<int, string> kvp in CachedDataManager.GetInstance.GetUserAccountsAsDict())
                {
                    remainderAllocationAccountList.Add(kvp.Key, kvp.Value);
                }
                if (e.allocationBaseChecked && AllocationTypeCollection != null)
                    SelectedAllocationType = AllocationTypeCollection.FirstOrDefault(x => (AllocationBaseType)x.Value == e.Rule.BaseType);
                if (e.matchingRuleChecked && MatchingRuleTypeCollection != null)
                    SelectedMatchingRuleType = MatchingRuleTypeCollection.FirstOrDefault(x => (MatchingRuleType)x.Value == e.Rule.RuleType);
                if (e.preferencedAccountChecked && remainderAllocationAccountList.ContainsKey(e.Rule.PreferenceAccountId))
                    SelectedRemainderAllocationAccount = new KeyValuePair<int, string>(e.Rule.PreferenceAccountId, remainderAllocationAccountList[e.Rule.PreferenceAccountId]);
                if (e.matchPortfolioPostionChecked && MatchClosingCollection != null)
                {
                    bool MatchClosingTransaction = e.Rule.MatchClosingTransaction != MatchClosingTransactionType.None;
                    SelectedMatchClosing = MatchClosingCollection.FirstOrDefault(x => x.DisplayText.ToLower() == Convert.ToString(MatchClosingTransaction).ToLower());
                }

                if (e.matchingRuleChecked && (e.Rule.RuleType == MatchingRuleType.Prorata || e.Rule.RuleType == MatchingRuleType.Leveling || e.Rule.RuleType == MatchingRuleType.ProrataByNAV))
                    SelectedProrataAccount = CommonAllocationMethods.GetCollection(e.Rule.ProrataAccountList, CachedDataManager.GetInstance.GetUserAccountsAsDict());
                if (e.Rule.RuleType == MatchingRuleType.Prorata)
                    DateUptoDays = e.Rule.ProrataDaysBack;

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
        /// Gets the default rule.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private AllocationRule GetDefaultRule()
        {
            AllocationRule rule = new AllocationRule();
            try
            {
                rule.BaseType = (AllocationBaseType)SelectedAllocationType.Value;
                rule.MatchClosingTransaction = Convert.ToBoolean(SelectedMatchClosing.DisplayText.ToLower()) ? MatchClosingTransactionType.CompletePortfolio : MatchClosingTransactionType.None;
                rule.RuleType = (MatchingRuleType)SelectedMatchingRuleType.Value;
                rule.PreferenceAccountId = SelectedRemainderAllocationAccount.Key;
                rule.ProrataDaysBack = DateUptoDays;
                foreach (KeyValuePair<int, string> selectAccount in SelectedProrataAccount.Cast<KeyValuePair<int, string>>())
                {
                    if (rule.ProrataAccountList == null)
                        rule.ProrataAccountList = new List<int>();
                    rule.ProrataAccountList.Add(selectAccount.Key);
                }
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
        /// Called when [load calculated preference default rule control].
        /// </summary>
        internal void OnLoadCalculatedPreferenceDefaultRuleControl(Dictionary<int, string> accounts)
        {
            try
            {
                Dictionary<int, string> accountCollection = new Dictionary<int, string>();
                accountCollection.Add(-1, "Select");
                accountCollection.AddRangeThreadSafely(accounts.OrderBy(x => x.Value));
                AllocationTypeCollection = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(AllocationBaseType)));
                MatchingRuleTypeCollection = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(MatchingRuleType)));
                Accounts = new ObservableDictionary<int, string>(accountCollection);
                MatchClosingCollection = new ObservableCollection<EnumerationValue>(Prana.ClientCommon.ClientEnumHelper.ConvertEnumForBindingWithAssignedValuesWithCaption(typeof(DefaultBoolean)));
                AccountsForProrata = new ObservableDictionary<int, string>(accounts.OrderBy(x => x.Value));
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
        /// Sets the calculated default rule control.
        /// </summary>
        /// <param name="allocationOperationPref">The allocation operation preference.</param>
        internal void SetCalculatedDefaultRuleControl(AllocationRule defaultRule)
        {
            try
            {
                Dictionary<int, string> remainderAllocationAccountList = new Dictionary<int, string>();
                remainderAllocationAccountList.Add(-1, "Select");
                foreach (KeyValuePair<int, string> kvp in CachedDataManager.GetInstance.GetUserAccountsAsDict())
                {
                    remainderAllocationAccountList.Add(kvp.Key, kvp.Value);
                }
                this.SelectedAllocationType = this.AllocationTypeCollection.FirstOrDefault(x => (AllocationBaseType)x.Value == defaultRule.BaseType);
                bool MatchClosingTransaction = defaultRule.MatchClosingTransaction != MatchClosingTransactionType.None;
                this.SelectedMatchClosing = (this.MatchClosingCollection.FirstOrDefault(x => x.DisplayText.ToLower() == Convert.ToString(MatchClosingTransaction).ToLower()));
                this.DateUptoDays = defaultRule.ProrataDaysBack;
                this.SelectedMatchingRuleType = this.MatchingRuleTypeCollection.FirstOrDefault(x => (MatchingRuleType)x.Value == defaultRule.RuleType);
                this.SelectedRemainderAllocationAccount = new KeyValuePair<int, string>(defaultRule.PreferenceAccountId, remainderAllocationAccountList[defaultRule.PreferenceAccountId]);
                if (defaultRule.ProrataAccountList != null)
                {
                    this.SelectedProrataAccount = CommonAllocationMethods.GetCollection(defaultRule.ProrataAccountList, CachedDataManager.GetInstance.GetUserAccountsAsDict());
                }
                else
                {
                    this.SelectedProrataAccount = new ObservableCollection<object>();
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
                    _matchClosingCollection = null;
                    _selectedMatchClosing = null;
                    _selectedProrataAccount = null;
                    _accounts = null;
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

        #endregion Methods
    }
}
