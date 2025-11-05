using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    public class AccountAndStrategyGridControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [preference CMB value change].
        /// </summary>
        public event EventHandler PrefCmbValueChange;

        #endregion Events

        #region Members

        /// <summary>
        /// The Account startegy table
        /// </summary>
        private DataTable _accountStartegyTable;

        /// <summary>
        /// The updated accounts
        /// </summary>
        private HashSet<int> _updatedAccounts = new HashSet<int>();

        /// <summary>
        /// The _startegy list
        /// </summary>
        private Dictionary<int, string> _startegyList = new Dictionary<int, string>();

        /// <summary>
        /// The _selected strategy
        /// </summary>
        private KeyValuePair<int, string> _selectedStrategy = new KeyValuePair<int, string>();

        /// <summary>
        /// The _selected strategy name
        /// </summary>
        private int _selectedStrategyName;

        /// <summary>
        /// The _search strategy visible
        /// </summary>
        private Visibility _searchStrategyVisible;

        /// <summary>
        /// Gets or sets the cum qauntity.
        /// </summary>
        /// <value>
        /// The cum qauntity.
        /// </value>
        private double _cumQuantity;

        /// <summary>
        /// The _is refresh summary
        /// </summary>
        private bool _isRefreshSummary;

        /// <summary>
        /// The _is grid enable
        /// </summary>
        private bool _isGridEnable;

        /// <summary>
        /// The _precision digit
        /// </summary>
        private int _precisionDigit;

        /// <summary>
        /// The is save layout account and startegy grid
        /// </summary>
        private bool _isSaveLayoutAccountAndStartegyGrid;

        /// <summary>
        /// The _selected trades
        /// </summary>
        private int _selectedTrades;

        /// <summary>
        /// The _total trades
        /// </summary>
        private int _totalTrades;

        /// <summary>
        /// The _is preference CMB enabled
        /// </summary>
        private bool _isPrefCmbEnabled;

        /// <summary>
        /// The _is preference selected
        /// </summary>
        private bool _isPrefSelected;

        /// <summary>
        /// The _is preference changed
        /// </summary>
        private bool _isPrefChanged;

        /// <summary>
        /// The _precision format
        /// </summary>
        private string _precisionFormat;

        /// <summary>
        /// The _is sort by percentage
        /// </summary>
        private bool _isSortByPercentage;

        /// <summary>
        /// The _set active cell
        /// </summary>
        private bool _setActiveCell;

        /// <summary>
        /// The _target dic
        /// </summary>
        private SerializableDictionary<int, AccountValue> _targetDic;

        /// <summary>
        /// The original allocation dic
        /// </summary>
        private SerializableDictionary<int, AccountValue> _originalAllocationDic = new SerializableDictionary<int, AccountValue>();

        /// <summary>
        /// The end edit mode grid
        /// </summary>
        private bool _endEditModeGrid;

        private bool _exportAccountAndStrategyGrid;

        private string _exportPathForAutomation;

        #endregion Members

        #region Properties

        public bool ExportAccountAndStrategyGrid
        {
            get { return _exportAccountAndStrategyGrid; }
            set
            {
                _exportAccountAndStrategyGrid = value;
                RaisePropertyChangedEvent("ExportAccountAndStrategyGrid");
            }
        }

        public string ExportPathForAutomation
        {
            get { return _exportPathForAutomation; }
            set { _exportPathForAutomation = value; RaisePropertyChangedEvent("ExportPathForAutomation"); }
        }
        /// <summary>
        /// Gets or sets the Account startegy table.
        /// </summary>
        /// <value>
        /// The Account startegy table.
        /// </value>
        public DataTable AccountStartegyTable
        {
            get { return _accountStartegyTable; }
            set
            {
                _accountStartegyTable = value;
                RaisePropertyChangedEvent("AccountStartegyTable");
            }
        }

        /// <summary>
        /// Gets or sets the updated accounts.
        /// </summary>
        /// <value>
        /// The updated accounts.
        /// </value>
        public HashSet<int> UpdatedAccounts
        {
            get { return _updatedAccounts; }
            set
            {
                _updatedAccounts = value;
                RaisePropertyChangedEvent("UpdatedAccounts");
            }
        }

        /// <summary>
        /// Gets or sets the cum qauntity.
        /// </summary>
        /// <value>
        /// The cum qauntity.
        /// </value>
        public double CumQauntity
        {
            get { return _cumQuantity; }
            set
            {
                _cumQuantity = value;
                RaisePropertyChangedEvent("CumQauntity");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is refresh summary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is refresh summary; otherwise, <c>false</c>.
        /// </value>
        public bool IsRefreshSummary
        {
            get { return _isRefreshSummary; }
            set
            {
                _isRefreshSummary = value;
                RaisePropertyChangedEvent("IsRefreshSummary");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is grid enable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is grid enable; otherwise, <c>false</c>.
        /// </value>
        public bool IsGridEnable
        {
            get { return _isGridEnable; }
            set
            {
                _isGridEnable = value;
                RaisePropertyChangedEvent("IsGridEnable");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is preference changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is preference changed; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrefChanged
        {
            get { return _isPrefChanged; }
            set
            {
                _isPrefChanged = value;
                RaisePropertyChangedEvent("IsPrefChanged");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is preference CMB enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is preference CMB enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrefCmbEnabled
        {
            get { return _isPrefCmbEnabled; }
            set
            {
                _isPrefCmbEnabled = value;
                if (value)
                {
                    if (PrefCmbValueChange != null)
                        PrefCmbValueChange(this, EventArgs.Empty);
                }
                RaisePropertyChangedEvent("IsPrefCmbEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is preference selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is preference selected; otherwise, <c>false</c>.
        /// </value>
        public bool ISPrefSelected
        {
            get { return _isPrefSelected; }
            set
            {
                _isPrefSelected = value;
                RaisePropertyChangedEvent("ISPrefSelected");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is sort by percentage.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is sort by percentage; otherwise, <c>false</c>.
        /// </value>
        public bool IsSortByPercentage
        {
            get { return _isSortByPercentage; }
            set
            {
                _isSortByPercentage = value;
                RaisePropertyChangedEvent("IsSortByPercentage");
            }
        }

        /// <summary>
        /// Gets or sets the precision digit.
        /// </summary>
        /// <value>
        /// The precision digit.
        /// </value>
        public int PrecisionDigit
        {
            get { return _precisionDigit; }
            set
            {
                _precisionDigit = value;
                RaisePropertyChangedEvent("PrecisionDigit");
            }
        }

        /// <summary>
        /// Gets or sets the precision format.
        /// </summary>
        /// <value>
        /// The precision format.
        /// </value>
        public string PrecisionFormat
        {
            get { return _precisionFormat; }
            set
            {
                _precisionFormat = value;
                RaisePropertyChangedEvent("PrecisionFormat");
            }
        }

        /// <summary>
        /// Gets or sets the search strategy visible.
        /// </summary>
        /// <value>
        /// The search strategy visible.
        /// </value>
        public Visibility SearchStrategyVisible
        {
            get { return _searchStrategyVisible; }
            set
            {
                _searchStrategyVisible = value;
                RaisePropertyChangedEvent("SearchStrategyVisible");
            }
        }

        /// <summary>
        /// Gets or sets the selected strategy.
        /// </summary>
        /// <value>
        /// The selected strategy.
        /// </value>
        public KeyValuePair<int, string> SelectedStrategy
        {
            get { return _selectedStrategy; }
            set
            {
                _selectedStrategy = value;
                RaisePropertyChangedEvent("SelectedStrategy");
            }
        }

        /// <summary>
        /// The _selected strategy name
        /// </summary>
        public int SelectedStrategyName
        {
            get { return _selectedStrategyName; }
            set
            {
                _selectedStrategyName = value;
                RaisePropertyChangedEvent("SelectedStrategyName");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is save layout account and startegy grid.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save layout account and startegy grid; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaveLayoutAccountAndStartegyGrid
        {
            get { return _isSaveLayoutAccountAndStartegyGrid; }
            set
            {
                _isSaveLayoutAccountAndStartegyGrid = value;
                RaisePropertyChangedEvent("IsSaveLayoutAccountAndStartegyGrid");
            }
        }

        /// <summary>
        /// Gets or sets the selected trades.
        /// </summary>
        /// <value>
        /// The selected trades.
        /// </value>
        public int SelectedTrades
        {
            get { return _selectedTrades; }
            set
            {
                _selectedTrades = value;
                RaisePropertyChangedEvent("SelectedTrades");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [set active cell].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [set active cell]; otherwise, <c>false</c>.
        /// </value>
        public bool SetActiveCell
        {
            get { return _setActiveCell; }
            set
            {
                _setActiveCell = value;
                RaisePropertyChangedEvent("SetActiveCell");
            }
        }

        /// <summary>
        /// Gets or sets the startegy list.
        /// </summary>
        /// <value>
        /// The startegy list.
        /// </value>
        public Dictionary<int, string> StartegyList
        {
            get { return _startegyList; }
            set
            {
                _startegyList = value;
                RaisePropertyChangedEvent("StartegyList");
            }
        }

        /// <summary>
        /// Gets or sets the target dic1.
        /// </summary>
        /// <value>
        /// The target dic1.
        /// </value>
        public SerializableDictionary<int, AccountValue> TargetDic1
        {
            get { return _targetDic; }
            set
            {
                _targetDic = value;
                RaisePropertyChangedEvent("TargetDic1");
            }
        }

        /// <summary>
        /// Gets or sets the OriginalAllocation dic1.
        /// </summary>
        /// <value>
        /// The OriginalAllocation dic1.
        /// </value>
        public SerializableDictionary<int, AccountValue> OriginalAllocationDic1
        {
            get { return _originalAllocationDic; }
            set
            {
                _originalAllocationDic = value;
                RaisePropertyChangedEvent("OriginalAllocationDic1");
            }
        }

        /// <summary>
        /// Gets or sets the total trades.
        /// </summary>
        /// <value>
        /// The total trades.
        /// </value>
        public int TotalTrades
        {
            get { return _totalTrades; }
            set
            {
                _totalTrades = value;
                RaisePropertyChangedEvent("TotalTrades");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [end edit mode grid].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [end edit mode grid]; otherwise, <c>false</c>.
        /// </value>
        public bool EndEditModeGrid
        {
            get { return _endEditModeGrid; }
            set
            {
                _endEditModeGrid = value;
                RaisePropertyChangedEvent("EndEditModeGrid");
            }
        }

        /// <summary>
        /// Account Filter List
        /// </summary>
        public List<int> AccountFilterList { get; set; }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the search strategy command.
        /// </summary>
        /// <value>
        /// The search strategy command.
        /// </value>
        public RelayCommand<object> SearchStrategyCommand { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountAndStrategyGridControlViewModel"/> class.
        /// </summary>
        public AccountAndStrategyGridControlViewModel()
        {
            try
            {
                SearchStrategyCommand = new RelayCommand<object>((parameter) => SearchStrategy(parameter));
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
        /// Calculates the quantity for percentage.
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="value">The value.</param>
        private void CalculateQuantityForPercentage(string groupKey, int rowIndex, double value)
        {
            try
            {
                if (groupKey.StartsWith(AllocationUIConstants.ACCOUNT))
                {
                    lock (_accountStartegyTable)
                    {
                        _accountStartegyTable.Rows[rowIndex][AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY] = Math.Round(value * this.CumQauntity / 100, 10);
                    }
                }
                else
                {
                    lock (_accountStartegyTable)
                    {
                        double accountValue = Convert.ToDouble(_accountStartegyTable.Rows[rowIndex][AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY]);
                        _accountStartegyTable.Rows[rowIndex][AllocationUIConstants.STRATEGY_PREFIX + groupKey + AllocationUIConstants.QUANTITY] = Math.Round(value * accountValue / 100, 10);
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
        /// Clears the grid.
        /// </summary>
        internal void ClearGrid()
        {
            try
            {
                ClearGridOnly();
                if (TargetDic1 != null)
                    TargetDic1.Clear();
                RefreshSorting();
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Refreshes the sorting.
        /// </summary>
        internal void RefreshSorting()
        {
            try
            {
                IsSortByPercentage = _isSortByPercentage;
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clears the grid new.
        /// </summary>
        internal HashSet<int> ClearGridOnly()
        {
            HashSet<int> accountId = new HashSet<int>();
            try
            {
                StrategyCollection strategyCollection = CommonDataCache.CachedDataManager.GetInstance.GetUserPermittedStrategies();
                if (AccountStartegyTable != null)
                {
                    AccountStartegyTable.AsEnumerable().Where(x => !x[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE].Equals(0.0)).ToList().ForEach(row =>
                    {
                        accountId.Add((int)row[AllocationUIConstants.ACCOUNT_ID]);
                        row[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE] = 0;
                        row[AllocationUIConstants.ACCOUNT + AllocationUIConstants.QUANTITY] = 0;
                        if (strategyCollection != null)
                        {
                            strategyCollection.Cast<Strategy>().Where(strategy => strategy.StrategyID != int.MinValue).ToList().ForEach(strategy =>
                            {
                                row[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.PERCENTAGE] = 0;
                                row[AllocationUIConstants.STRATEGY_PREFIX + strategy.Name + AllocationUIConstants.QUANTITY] = 0;
                            });
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accountId;
        }

        /// <summary>
        /// Gets the target percantage.
        /// </summary>
        /// <returns></returns>
        internal SerializableDictionary<int, AccountValue> GetTargetPercantage()
        {
            try
            {
                SetActiveCell = true;

                if (_targetDic != null)
                    return _targetDic;
                else
                    return new SerializableDictionary<int, AccountValue>();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        internal SerializableDictionary<int, AccountValue> GetOriginalAllocationPercantage()
        {
            try
            {
                if (_originalAllocationDic != null)
                    return _originalAllocationDic;
                else
                    return new SerializableDictionary<int, AccountValue>();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Load AccountStrategy Grid Data
        /// </summary>
        internal void LoadAccountStrategyGridData()
        {
            try
            {
                Dictionary<int, string> accountList = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                //Filter the accounts based on selected masterfunds 
                if (AccountFilterList != null && AccountFilterList.Count > 0)
                {
                    accountList = accountList.Where(x => AccountFilterList.Contains(x.Key)).ToDictionary(x => x.Key, y => y.Value);
                }

                StrategyCollection strategyCollection = CachedDataManager.GetInstance.GetUserPermittedStrategies();
                AccountStartegyTable = CommonAllocationMethods.SetUpDataTable(accountList, strategyCollection);

                StartegyList = CachedDataManager.GetInstance.GetUserStrategiesDictionary();
                if (StartegyList != null && StartegyList.Count > 0 && StartegyList.Keys.Max() <= 0)
                {
                    StartegyList = new Dictionary<int, string>();
                    // Search Strategy Combo & Button should be visible to the users having atleast one strategy permission, PRANA-15413
                    SearchStrategyVisible = Visibility.Hidden;
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
        /// Searches the strategy.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object SearchStrategy(object parameter)
        {
            try
            {
                if (SelectedStrategy.Value != null &&
                    !SelectedStrategy.Value.Equals(AllocationUIConstants.SELECT_STRING))
                    SelectedStrategyName = SelectedStrategy.Key;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Sets the preferences.
        /// </summary>
        /// <param name="precisionDigit">The precision digit.</param>
        internal void SetPreferences(int precisionDigit, bool isSortingDisabled)
        {
            try
            {
                //set precision digits
                PrecisionDigit = precisionDigit;
                PrecisionFormat = CommonAllocationMethods.SetPrecisionFormat(precisionDigit);
                IsSortByPercentage = !isSortingDisabled;

                string sortValue = string.Empty;
                if (_isSortByPercentage)
                {
                    //Default sorting in Data Table
                    sortValue = "[" + AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE + "] desc";
                }

                if (AccountStartegyTable != null)
                {
                    AccountStartegyTable.DefaultView.Sort = sortValue;
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
        /// Sets the values in grid.
        /// </summary>
        /// <param name="targetDictionary">The target dictionary.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        internal void SetValuesInGrid(SerializableDictionary<int, AccountValue> targetDictionary)
        {
            try
            {
                if (AccountStartegyTable != null)
                {
                    lock (AccountStartegyTable)
                    {
                        HashSet<int> updatedAccounts = ClearGridOnly();
                        SerializableDictionary<int, AccountValue> originalAllocationDic = new SerializableDictionary<int, AccountValue>();

                        //Get the user permitted strategies Dictionary
                        Dictionary<int, string> startegyList = CachedDataManager.GetInstance.GetUserStrategiesDictionary();

                        //If user has no strategy permission, then unallocated and select is coming by default. So reinitializing it.
                        if (startegyList != null && startegyList.Count > 0 && startegyList.Keys.Max() <= 0)
                            startegyList = new Dictionary<int, string>();

                        foreach (int accountId in targetDictionary.Keys)
                        {
                            updatedAccounts.Add(accountId);
                            if (AccountStartegyTable.Select(AllocationUIConstants.ACCOUNT_ID + " = " + accountId).Count() > 0)
                            {
                                DataRow dr = AccountStartegyTable.Select(AllocationUIConstants.ACCOUNT_ID + " = " + accountId)[0];
                                dr[AllocationUIConstants.ACCOUNT + AllocationUIConstants.PERCENTAGE] = targetDictionary[accountId].Value;
                                CalculateQuantityForPercentage(AllocationUIConstants.ACCOUNT, AccountStartegyTable.Rows.IndexOf(dr), Convert.ToDouble(targetDictionary[accountId].Value));
                                if (startegyList.Count > 0)
                                {
                                    foreach (StrategyValue strategy in targetDictionary[accountId].StrategyValueList)
                                    {
                                        if (startegyList.ContainsKey(strategy.StrategyId))
                                        {
                                            string strategyName = CommonDataCache.CachedDataManager.GetInstance.GetStrategyText(strategy.StrategyId);
                                            dr[AllocationUIConstants.STRATEGY_PREFIX + strategyName + AllocationUIConstants.PERCENTAGE] = strategy.Value;
                                            CalculateQuantityForPercentage(strategyName, AccountStartegyTable.Rows.IndexOf(dr), Convert.ToDouble(strategy.Value));
                                        }
                                    }
                                }

                                if (!originalAllocationDic.ContainsKey(accountId))
                                {
                                    originalAllocationDic.Add(accountId, targetDictionary[accountId]);
                                }
                                else
                                {
                                    originalAllocationDic[accountId] = targetDictionary[accountId];
                                }
                            }

                        }
                        OriginalAllocationDic1 = originalAllocationDic;
                        UpdatedAccounts = updatedAccounts;
                    }
                    IsRefreshSummary = true;
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
        /// Updates the quantity.
        /// </summary>
        /// <param name="cumQauntity">The cum qauntity.</param>
        internal void UpdateQuantity(double cumQauntity)
        {
            try
            {
                this.CumQauntity = cumQauntity;
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves the account strategy grid layout.
        /// </summary>
        internal void SaveAccountStrategyGridLayout()
        {
            try
            {
                IsSaveLayoutAccountAndStartegyGrid = true;
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the total no of trades.
        /// </summary>
        /// <param name="totalTrades">The total trades.</param>
        /// <param name="selectedTrades">The selected trades.</param>
        internal void UpdateTotalNoOfTrades(int selectedTrades, int totalTrades)
        {
            try
            {
                if (SelectedTrades != selectedTrades)
                    SelectedTrades = selectedTrades;
                if (TotalTrades != totalTrades)
                    TotalTrades = totalTrades;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Methods

        internal void setTargetDictFromOriginalDict(SerializableDictionary<int, AccountValue> targetpercentage)
        {
            try
            {
                if (_targetDic != null)
                {
                    _targetDic.Clear();
                    foreach (KeyValuePair<int, AccountValue> pair in targetpercentage)
                    {
                        if (!_targetDic.ContainsKey(pair.Key))
                        {
                            _targetDic.Add(pair.Key, targetpercentage[pair.Key]);
                        }
                        else
                        {
                            _targetDic[pair.Key] = targetpercentage[pair.Key];
                        }
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
    }
}
