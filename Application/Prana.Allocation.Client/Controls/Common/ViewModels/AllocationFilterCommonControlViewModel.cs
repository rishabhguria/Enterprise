using Prana.Allocation.Client.Constants;
using Prana.Allocation.Common.Definitions;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Common.ViewModels
{
    public class AllocationFilterCommonControlViewModel : ViewModelBase
    {
        #region Members

        /// <summary>
        /// The Account
        /// </summary>
        private ObservableDictionary<int, string> _account;

        /// <summary>
        /// The Account
        /// </summary>
        private ObservableCollection<Account> _funds;



        /// <summary>
        /// The _selected filter Account
        /// </summary>
        private ObservableCollection<object> _selectedFilterAccount = new ObservableCollection<object>();

        /// <summary>
        /// The _selected filter Account
        /// </summary>
        private ObservableCollection<object> _selectedFilterFund = new ObservableCollection<object>();


        /// <summary>
        /// The _asset
        /// </summary>
        private ObservableDictionary<int, string> _asset;

        /// <summary>
        /// The _selected filter asset
        /// </summary>
        private ObservableCollection<object> _selectedFilterAsset = new ObservableCollection<object>();

        /// <summary>
        /// The _side
        /// </summary>
        private ObservableDictionary<string, string> _side;

        /// <summary>
        /// The _selected  filter side
        /// </summary>
        private ObservableCollection<object> _selectedFilterSide = new ObservableCollection<object>();

        /// <summary>
        /// The _selected  symbol
        /// </summary>
        private object[] _selectedSymbol;

        /// <summary>
        /// The _strategy
        /// </summary>
        private ObservableDictionary<int, string> _strategy;

        /// <summary>
        /// The _venue
        /// </summary>
        private ObservableDictionary<int, string> _venue;

        /// <summary>
        /// The _broker
        /// </summary>
        private ObservableDictionary<int, string> _broker;

        /// <summary>
        /// The _exchange
        /// </summary>
        private ObservableDictionary<int, string> _exchange;

        /// <summary>
        /// The _trading account
        /// </summary>
        private ObservableDictionary<int, string> _tradingAccount;

        /// <summary>
        /// The _underlying
        /// </summary>
        private ObservableDictionary<int, string> _underlying;

        /// <summary>
        /// The _currency
        /// </summary>
        private ObservableDictionary<int, string> _currency;

        /// <summary>
        /// The _pre allocated
        /// </summary>
        private ObservableCollection<string> _preAllocated;

        /// <summary>
        /// The _manual group
        /// </summary>
        private ObservableCollection<string> _manualGroup;

        /// <summary>
        /// The _selected strategy
        /// </summary>
        private ObservableCollection<object> _selectedStrategy = new ObservableCollection<object>();

        /// <summary>
        /// The _selected broker
        /// </summary>
        private ObservableCollection<object> _selectedBroker = new ObservableCollection<object>();

        /// <summary>
        /// The _selected trading account
        /// </summary>
        private ObservableCollection<object> _selectedTradingAccount = new ObservableCollection<object>();

        /// <summary>
        /// The _selected currency
        /// </summary>
        private ObservableCollection<object> _selectedCurrency = new ObservableCollection<object>();

        /// <summary>
        /// The _selected exchange
        /// </summary>
        private ObservableCollection<object> _selectedExchange = new ObservableCollection<object>();

        /// <summary>
        /// The _selected underlying
        /// </summary>
        private ObservableCollection<object> _selectedUnderlying = new ObservableCollection<object>();

        /// <summary>
        /// The _selected venue
        /// </summary>
        private ObservableCollection<object> _selectedVenue = new ObservableCollection<object>();

        /// <summary>
        /// The _is pre allocated
        /// </summary>
        private string _isPreAllocated;

        /// <summary>
        /// The _is manual group
        /// </summary>
        private string _isManualGroup;

        /// <summary>
        /// The _account combo visibility
        /// </summary>
        private Visibility _accountComboVisibility;

        /// <summary>
        /// The _fund combo visibility
        /// </summary>
        private Visibility _fundsComboVisibility;

        /// <summary>
        /// The _fund combo Name
        /// </summary>
        private string _fundsComboHeading;


        /// <summary>
        /// The _filter list
        /// </summary>
        private Dictionary<string, string> _filterList;

        /// <summary>
        /// The _group identifier
        /// </summary>
        private string _groupId;

        /// <summary>
        /// The _selected list of symbols
        /// </summary>
        private ObservableCollection<string> _symbolList = new ObservableCollection<string>();

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>
        /// The group identifier.
        /// </value>
        public string GroupId
        {
            get { return _groupId; }
            set
            {
                _groupId = value;
                UpdateFilterList(AllocationUIConstants.GROUP_ID, _groupId);
            }
        }


        /// <summary>
        /// Gets or sets the Account.
        /// </summary>
        /// <value>
        /// The Account.
        /// </value>
        public ObservableDictionary<int, string> Account
        {
            get { return _account; }
            set
            {
                _account = value;
                RaisePropertyChangedEvent("Account");
            }
        }

        /// <summary>
        /// Gets or sets the Account.
        /// </summary>
        /// <value>
        /// The Account.
        /// </value>
        public ObservableCollection<Account> Funds
        {
            get { return _funds; }
            set
            {
                _funds = value;
                RaisePropertyChangedEvent("Funds");
            }
        }

        /// <summary>
        /// Gets or sets the account combo visibility.
        /// </summary>
        /// <value>
        /// The account combo visibility.
        /// </value>
        public Visibility AccountComboVisibility
        {
            get { return _accountComboVisibility; }
            set
            {
                _accountComboVisibility = value;
                RaisePropertyChangedEvent("AccountComboVisibility");
            }
        }

        /// <summary>
        /// Gets or sets the fund combo visibility.
        /// </summary>
        /// <value>
        /// The fund combo visibility.
        /// </value>
        public Visibility FundsComboVisibility
        {
            get { return _fundsComboVisibility; }
            set
            {
                _fundsComboVisibility = value;
                RaisePropertyChangedEvent("FundsComboVisibility");
            }
        }

        /// <summary>
        /// Gets or sets the fund combo Name.
        /// </summary>
        /// <value>
        /// The fund combo Name.
        /// </value>
        public String FundsComboHeading
        {
            get { return _fundsComboHeading; }
            set
            {
                _fundsComboHeading = value;
                RaisePropertyChangedEvent("FundsComboHeading");
            }
        }

        /// <summary>
        /// Gets or sets the asset.
        /// </summary>
        /// <value>
        /// The asset.
        /// </value>
        public ObservableDictionary<int, string> Asset
        {
            get { return _asset; }
            set
            {
                _asset = value;
                RaisePropertyChangedEvent("Asset");
            }
        }

        /// <summary>
        /// Gets or sets the broker.
        /// </summary>
        /// <value>
        /// The broker.
        /// </value>
        public ObservableDictionary<int, string> Broker
        {
            get { return _broker; }
            set
            {
                _broker = value;
                RaisePropertyChangedEvent("Broker");
            }
        }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public ObservableDictionary<int, string> Currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
                RaisePropertyChangedEvent("Currency");
            }
        }

        /// <summary>
        /// Gets or sets the exchange.
        /// </summary>
        /// <value>
        /// The exchange.
        /// </value>
        public ObservableDictionary<int, string> Exchange
        {
            get { return _exchange; }
            set
            {
                _exchange = value;
                RaisePropertyChangedEvent("Exchange");
            }
        }

        /// <summary>
        /// Gets or sets the filter list.
        /// </summary>
        /// <value>
        /// The filter list.
        /// </value>
        internal Dictionary<string, string> FilterList
        {
            set { _filterList = value; }
        }

        /// <summary>
        /// Gets or sets the is manual group.
        /// </summary>
        /// <value>
        /// The is manual group.
        /// </value>
        public string IsManualGroup
        {
            get { return _isManualGroup; }
            set
            {
                _isManualGroup = value;
                UpdateFilterList(AllocationUIConstants.IS_MANUAL_GROUP, _isManualGroup);
                RaisePropertyChangedEvent("IsManualGroup");
            }
        }

        /// <summary>
        /// Gets or sets the is pre allocated.
        /// </summary>
        /// <value>
        /// The is pre allocated.
        /// </value>
        public string IsPreAllocated
        {
            get { return _isPreAllocated; }
            set
            {
                _isPreAllocated = value;
                UpdateFilterList(AllocationUIConstants.IS_PREALLOCATED, _isPreAllocated);
                RaisePropertyChangedEvent("IsPreAllocated");
            }
        }

        /// <summary>
        /// Gets or sets the manual group.
        /// </summary>
        /// <value>
        /// The manual group.
        /// </value>
        public ObservableCollection<string> ManualGroup
        {
            get { return _manualGroup; }
            set
            {
                _manualGroup = value;
                RaisePropertyChangedEvent("ManualGroup");
            }
        }

        /// <summary>
        /// Gets or sets the pre allocated.
        /// </summary>
        /// <value>
        /// The pre allocated.
        /// </value>
        public ObservableCollection<string> PreAllocated
        {
            get { return _preAllocated; }
            set
            {
                _preAllocated = value;
                RaisePropertyChangedEvent("PreAllocated");
            }
        }

        /// <summary>
        /// Gets or sets the selected broker.
        /// </summary>
        /// <value>
        /// The selected broker.
        /// </value>
        public ObservableCollection<object> SelectedBroker
        {
            get { return _selectedBroker; }
            set
            {
                _selectedBroker = value;
                UpdateFilterList(AllocationClientConstants.BROKER_ID, GetStringFromCollection(_selectedBroker));
                RaisePropertyChangedEvent("SelectedBroker");
            }
        }

        /// <summary>
        /// Gets or sets the selected currency.
        /// </summary>
        /// <value>
        /// The selected currency.
        /// </value>
        public ObservableCollection<object> SelectedCurrency
        {
            get { return _selectedCurrency; }
            set
            {
                _selectedCurrency = value;
                UpdateFilterList(AllocationUIConstants.CURRENCY_ID, GetStringFromCollection(_selectedCurrency));
                RaisePropertyChangedEvent("SelectedCurrency");
            }
        }

        /// <summary>
        /// Gets or sets the selected exchange.
        /// </summary>
        /// <value>
        /// The selected exchange.
        /// </value>
        public ObservableCollection<object> SelectedExchange
        {
            get { return _selectedExchange; }
            set
            {
                _selectedExchange = value;
                UpdateFilterList(AllocationUIConstants.EXCHANGE_ID, GetStringFromCollection(_selectedExchange));
                RaisePropertyChangedEvent("SelectedExchange");
            }
        }



        /// <summary>
        /// Gets or sets the selected all filter Account.
        /// </summary>
        /// <value>
        /// The selected all filter Account.
        /// </value>
        public ObservableCollection<object> SelectedFilterAccount
        {
            get { return _selectedFilterAccount; }
            set
            {
                _selectedFilterAccount = value;
                UpdateFilterList(AllocationUIConstants.ACCOUNT_ID_COLUMN, GetStringFromCollection(_selectedFilterAccount));
                RaisePropertyChangedEvent("SelectedFilterAccount");
            }
        }

        /// <summary>
        /// Update Funds List
        /// </summary>
        /// <param name="selectedFilterAccount"></param>
        private void UpdateFundsList(ObservableCollection<object> selectedFilterAccount)
        {
            try
            {
                SelectedFilterFund = new ObservableCollection<object>();
                if (selectedFilterAccount.Count() > 0)
                {
                    var selectedFundsNew = new ObservableCollection<object>();
                    foreach (KeyValuePair<int, string> id in selectedFilterAccount)
                    {
                        var fundId = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(id.Key);
                        var fundName = CachedDataManager.GetInstance.GetMasterFund(fundId);
                        var item = new KeyValuePair<int, string>(fundId, fundName);
                        if (!selectedFundsNew.Contains(item))
                            selectedFundsNew.Add(item);
                    }
                    SelectedFilterFund = selectedFundsNew;

                }

            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Gets or sets the selected all filter Account.
        /// </summary>
        /// <value>
        /// The selected all filter Account.
        /// </value>
        public ObservableCollection<object> SelectedFilterFund
        {
            get { return _selectedFilterFund; }
            set
            {
                _selectedFilterFund = value;
                UpdateAccountsList(_selectedFilterFund);

                UpdateFilterList(AllocationUIConstants.COMPANY_MASTER_FUND_ID, GetStringFromCollectionFromList(_selectedFilterFund));
                RaisePropertyChangedEvent("SelectedFilterFund");
            }
        }

        private string GetStringFromCollectionFromList(ObservableCollection<object> _selectedFilterFund)
        {
            List<int> list = new List<int>();
            foreach (var fund in _selectedFilterFund)
            {
                list.Add(((Account)fund).AccountID);
            }
            return string.Join(",", list.ToArray());
        }

        /// <summary>
        /// Update Accounts List
        /// </summary>
        /// <param name="selectedFilterFund"></param>
        private void UpdateAccountsList(ObservableCollection<object> selectedFilterFund)
        {
            try
            {
                SelectedFilterAccount = new ObservableCollection<object>();
                if (selectedFilterFund.Count() > 0)
                {
                    var selectedAccountsNew = new ObservableCollection<object>();
                    foreach (Account accountDetails in selectedFilterFund)
                    {
                        if (CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation().ContainsKey(accountDetails.AccountID))
                        {
                            var accountIds = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation()[accountDetails.AccountID];
                            accountIds.ForEach(accountId =>
                            {
                                var accountName = CachedDataManager.GetInstance.GetAccount(accountId);
                                var item = new KeyValuePair<int, string>(accountId, accountName);
                                if (!selectedAccountsNew.Contains(item))
                                    selectedAccountsNew.Add(item);
                            });
                        }
                    }
                    SelectedFilterAccount = selectedAccountsNew;

                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }





        /// <summary>
        /// Gets or sets the selected all filter asset.
        /// </summary>
        /// <value>
        /// The selected all filter asset.
        /// </value>
        public ObservableCollection<object> SelectedFilterAsset
        {
            get { return _selectedFilterAsset; }
            set
            {
                _selectedFilterAsset = value;
                UpdateFilterList(AllocationUIConstants.ASSETID, GetStringFromCollection(_selectedFilterAsset));
                RaisePropertyChangedEvent("SelectedFilterAsset");
            }
        }

        /// <summary>
        /// Gets or sets the selected all filter side.
        /// </summary>
        /// <value>
        /// The selected all filter side.
        /// </value>
        public ObservableCollection<object> SelectedFilterSide
        {
            get { return _selectedFilterSide; }
            set
            {
                _selectedFilterSide = value;
                UpdateFilterList(AllocationUIConstants.ORDERSIDE_TAGVALUE, GetStringFromCollection(_selectedFilterSide));
                RaisePropertyChangedEvent("SelectedFilterSide");
            }
        }

        /// <summary>
        /// Gets or sets the selected strategy.
        /// </summary>
        /// <value>
        /// The selected strategy.
        /// </value>
        public ObservableCollection<object> SelectedStrategy
        {
            get { return _selectedStrategy; }
            set
            {
                _selectedStrategy = value;
                UpdateFilterList(AllocationUIConstants.STRATEGY_ID, GetStringFromCollection(_selectedStrategy));
                RaisePropertyChangedEvent("SelectedStrategy");
            }
        }

        /// <summary>
        /// Gets or sets the selected all symbol.
        /// </summary>
        /// <value>
        /// The selected all symbol.
        /// </value>
        public object[] SelectedSymbol
        {
            get { return _selectedSymbol; }
            set
            {
                _selectedSymbol = value;
                string selectedSymbols = _selectedSymbol == null || _selectedSymbol.Count() == 0 ? string.Empty : string.Join(",", _selectedSymbol);
                UpdateFilterList(AllocationUIConstants.SYMBOL, selectedSymbols);
                RaisePropertyChangedEvent("SelectedSymbol");
            }
        }

        /// <summary>
        /// Gets or sets the selected trading account.
        /// </summary>
        /// <value>
        /// The selected trading account.
        /// </value>
        public ObservableCollection<object> SelectedTradingAccount
        {
            get { return _selectedTradingAccount; }
            set
            {
                _selectedTradingAccount = value;
                UpdateFilterList(AllocationUIConstants.TRADING_ACCOUNT_ID, GetStringFromCollection(_selectedTradingAccount));
                RaisePropertyChangedEvent("SelectedTradingAccount");
            }
        }

        /// <summary>
        /// Gets or sets the selected underlying.
        /// </summary>
        /// <value>
        /// The selected underlying.
        /// </value>
        public ObservableCollection<object> SelectedUnderlying
        {
            get { return _selectedUnderlying; }
            set
            {
                _selectedUnderlying = value;
                UpdateFilterList(AllocationUIConstants.UNDERLYING_ID, GetStringFromCollection(_selectedUnderlying));
                RaisePropertyChangedEvent("SelectedUnderlying");
            }
        }

        /// <summary>
        /// Gets or sets the selected venue.
        /// </summary>
        /// <value>
        /// The selected venue.
        /// </value>
        public ObservableCollection<object> SelectedVenue
        {
            get { return _selectedVenue; }
            set
            {
                _selectedVenue = value;
                UpdateFilterList(AllocationUIConstants.VENUE_ID, GetStringFromCollection(_selectedVenue));
                RaisePropertyChangedEvent("SelectedVenue");
            }
        }

        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        /// <value>
        /// The side.
        /// </value>
        public ObservableDictionary<string, string> Side
        {
            get { return _side; }
            set
            {
                _side = value;
                RaisePropertyChangedEvent("Side");
            }
        }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>
        /// The strategy.
        /// </value>
        public ObservableDictionary<int, string> Strategy
        {
            get { return _strategy; }
            set
            {
                _strategy = value;
                RaisePropertyChangedEvent("Strategy");
            }
        }

        /// <summary>
        /// Gets or sets the trading account.
        /// </summary>
        /// <value>
        /// The trading account.
        /// </value>
        public ObservableDictionary<int, string> TradingAccount
        {
            get { return _tradingAccount; }
            set
            {
                _tradingAccount = value;
                RaisePropertyChangedEvent("TradingAccount");
            }
        }

        /// <summary>
        /// Gets or sets the underlying.
        /// </summary>
        /// <value>
        /// The underlying.
        /// </value>
        public ObservableDictionary<int, string> Underlying
        {
            get { return _underlying; }
            set
            {
                _underlying = value;
                RaisePropertyChangedEvent("Underlying");
            }
        }

        /// <summary>
        /// Gets or sets the venue.
        /// </summary>
        /// <value>
        /// The venue.
        /// </value>
        public ObservableDictionary<int, string> Venue
        {
            get { return _venue; }
            set
            {
                _venue = value;
                RaisePropertyChangedEvent("Venue");
            }
        }

        /// <summary>
        /// gets or set the selected symbols
        /// </summary>
        ///   /// <value>
        /// The symbol.
        /// </value>
        public ObservableCollection<string> SymbolList
        {
            get { return _symbolList; }
            set
            {
                _symbolList = value;
                RaisePropertyChangedEvent("SymbolList");
            }
        }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationFilterCommonControlViewModel"/> class.
        /// </summary>
        public AllocationFilterCommonControlViewModel()
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Clears the filter.
        /// </summary>
        internal void ClearFilter()
        {
            try
            {
                SelectedSymbol = null;
                SelectedFilterFund = new ObservableCollection<object>();
                SelectedFilterAsset = new ObservableCollection<object>();
                SelectedFilterAccount = new ObservableCollection<object>();
                SelectedFilterSide = new ObservableCollection<object>();
                SelectedVenue = new ObservableCollection<object>();
                SelectedUnderlying = new ObservableCollection<object>();
                SelectedStrategy = new ObservableCollection<object>();
                SelectedExchange = new ObservableCollection<object>();
                SelectedCurrency = new ObservableCollection<object>();
                SelectedBroker = new ObservableCollection<object>();
                SelectedTradingAccount = new ObservableCollection<object>();

                IsPreAllocated = null;
                IsManualGroup = null;
                _filterList.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the filter list.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, string> GetFilterList()
        {
            try
            {
                if (_filterList != null)
                    return _filterList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the string from collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static string GetStringFromCollection(ObservableCollection<object> selectedValue)
        {
            try
            {
                if (selectedValue.Count > 0)
                {
                    List<string> values = new List<string>();

                    if (selectedValue[0] is KeyValuePair<int, string>)
                    {
                        foreach (KeyValuePair<int, string> kvp in selectedValue)
                            values.Add(kvp.Key.ToString());
                    }
                    else if (selectedValue[0] is KeyValuePair<string, string>)
                    {
                        foreach (KeyValuePair<string, string> kvp in selectedValue)
                            values.Add(kvp.Key.ToString());
                    }
                    return string.Join(",", values.ToArray());
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return string.Empty;
        }

        /// <summary>
        /// Loads the allocation filter data.
        /// </summary>
        internal void LoadAllocationFilterControlData(AllocationFilterFields allocationFilter)
        {
            try
            {
                if (_filterList == null)
                    _filterList = new Dictionary<string, string>();

                FundsComboHeading = CachedDataManager.GetInstance.IsShowmasterFundAsClient() ? "Client" : "Master Fund";

                Account = new ObservableDictionary<int, string>(allocationFilter.Account);
                Side = new ObservableDictionary<string, string>(allocationFilter.Side);
                Asset = new ObservableDictionary<int, string>(allocationFilter.Asset);
                Strategy = new ObservableDictionary<int, string>(allocationFilter.Strategy);
                Exchange = new ObservableDictionary<int, string>(allocationFilter.Exchange);
                Currency = new ObservableDictionary<int, string>(allocationFilter.Currency);
                Underlying = new ObservableDictionary<int, string>(allocationFilter.Underlying);
                Broker = new ObservableDictionary<int, string>(allocationFilter.Broker);
                Venue = new ObservableDictionary<int, string>(allocationFilter.Venue);
                TradingAccount = new ObservableDictionary<int, string>(allocationFilter.TradingAccount);
                ManualGroup = new ObservableCollection<string>(allocationFilter.BoolList);
                PreAllocated = new ObservableCollection<string>(allocationFilter.BoolList);


                AccountCollection masterFundsCollection = new AccountCollection();

                foreach (KeyValuePair<int, string> currMasterFund in allocationFilter.Fund)
                {
                    masterFundsCollection.Add(new Account(currMasterFund.Key, currMasterFund.Value, currMasterFund.Value));
                }
                Funds = new ObservableCollection<Account>(masterFundsCollection.Cast<Account>().OrderBy(account => account.FullName));

                Account accountToRemove = Funds.FirstOrDefault(item => item.AccountID == int.MinValue);
                Funds.Remove(accountToRemove);
                Funds.Insert(0, new Account(Int32.MinValue, AllocationClientConstants.LIT_SELECT_ALL));

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the filter list.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="selectedValue">The selectedValue</param>
        private void UpdateFilterList(string key, string value)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    if (_filterList.ContainsKey(key))
                        _filterList[key] = value;
                    else
                        _filterList.Add(key, value);
                }
                else
                {
                    if (_filterList != null)
                        _filterList.Remove(key);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Methods


    }
}
