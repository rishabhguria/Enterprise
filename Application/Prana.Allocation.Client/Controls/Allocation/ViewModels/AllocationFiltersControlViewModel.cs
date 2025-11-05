using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Controls.Common.ViewModels;
using Prana.Allocation.Common.Definitions;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Prana.Allocation.Client.ViewModelBase" />
    public class AllocationFiltersControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [apply filter event].
        /// </summary>
        public event EventHandler ApplyFilterEvent;

        #endregion Events

        #region Members

        /// <summary>
        /// The _selected all tab
        /// </summary>
        private bool _selectedAllTab;

        /// <summary>
        /// AllFilterControl
        /// </summary>
        private AllocationFilterCommonControlViewModel _allFilterControl;

        /// <summary>
        /// UnallocatedFilterControl
        /// </summary>
        private AllocationFilterCommonControlViewModel _unallocatedFilterControl;

        /// <summary>
        /// AllocatedFilterControl
        /// </summary>
        private AllocationFilterCommonControlViewModel _allocatedFilterControl;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether AllFilterControl
        /// </summary>
        public AllocationFilterCommonControlViewModel AllFilterControl
        {
            get { return _allFilterControl; }
            set
            {
                _allFilterControl = value;
                RaisePropertyChangedEvent("AllFilterControl");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether AllocatedFilterControl
        /// </summary>
        public AllocationFilterCommonControlViewModel AllocatedFilterControl
        {
            get { return _allocatedFilterControl; }
            set
            {
                _allocatedFilterControl = value;
                RaisePropertyChangedEvent("AllocatedFilterControl");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [selected all tab].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [selected all tab]; otherwise, <c>false</c>.
        /// </value>
        public bool SelectedAllTab
        {
            get { return _selectedAllTab; }
            set
            {
                _selectedAllTab = value;
                RaisePropertyChangedEvent("SelectedAllTab");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether UnallocatedFilterControl
        /// </summary>
        public AllocationFilterCommonControlViewModel UnallocatedFilterControl
        {
            get { return _unallocatedFilterControl; }
            set
            {
                _unallocatedFilterControl = value;
                RaisePropertyChangedEvent("UnallocatedFilterControl");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the apply allocation filter.
        /// </summary>
        /// <value>
        /// The apply allocation filter.
        /// </value>
        public RelayCommand<object> ApplyAllocationFilter { get; set; }

        /// <summary>
        /// Gets or sets the allocation filters control view loaded.
        /// </summary>
        /// <value>
        /// The allocation filters control view loaded.
        /// </value>
        public RelayCommand<object> ClearAllocationFilter { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationFiltersControlViewModel"/> class.
        /// </summary>
        public AllocationFiltersControlViewModel()
        {
            try
            {
                _allFilterControl = new AllocationFilterCommonControlViewModel();
                _allocatedFilterControl = new AllocationFilterCommonControlViewModel();
                _unallocatedFilterControl = new AllocationFilterCommonControlViewModel();
                ClearAllocationFilter = new RelayCommand<object>((parameter) => ClearFilter(parameter));
                ApplyAllocationFilter = new RelayCommand<object>((parameter) => ApplyFilter(parameter));
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
        /// Applies the filter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private object ApplyFilter(object parameter)
        {
            try
            {
                if (ApplyFilterEvent != null)
                    ApplyFilterEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Clears the allocation filter.
        /// //TODO:remove parameter as not required
        /// </summary>
        public object ClearFilter(object parameter = null)
        {
            try
            {
                _unallocatedFilterControl.ClearFilter();
                _allFilterControl.ClearFilter();
                _allocatedFilterControl.ClearFilter();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// GetFilterList
        /// </summary>
        /// <returns></returns>
        internal AllocationPrefetchFilter GetFilterList()
        {
            AllocationPrefetchFilter allocationGroupFilter = new AllocationPrefetchFilter();
            try
            {
                if (SelectedAllTab)
                {
                    Dictionary<string, string> filterList = AllFilterControl.GetFilterList();
                    allocationGroupFilter.Allocated = DeepCopyHelper.Clone(filterList);
                    allocationGroupFilter.Unallocated = DeepCopyHelper.Clone(filterList);



                    if (allocationGroupFilter.Unallocated.ContainsKey(AllocationUIConstants.STRATEGY_ID))
                        allocationGroupFilter.Unallocated.Remove(AllocationUIConstants.STRATEGY_ID);
                }
                else
                {
                    allocationGroupFilter.Allocated = AllocatedFilterControl.GetFilterList();
                    allocationGroupFilter.Unallocated = UnallocatedFilterControl.GetFilterList();
                }

                if (allocationGroupFilter.Unallocated.ContainsKey(AllocationUIConstants.ACCOUNT_ID_COLUMN))
                    allocationGroupFilter.Unallocated.Remove(AllocationUIConstants.ACCOUNT_ID_COLUMN);

                if (!allocationGroupFilter.Allocated.ContainsKey(AllocationUIConstants.ACCOUNT_ID_COLUMN))
                    allocationGroupFilter.Allocated.Add(AllocationUIConstants.ACCOUNT_ID_COLUMN, string.Join(",", CachedDataManager.GetInstance.GetUserAccountsAsDict().Keys.ToArray()));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationGroupFilter;
        }

        /// <summary>
        /// Loads the allocation filter data.
        /// </summary>
        internal void LoadAllocationFilterData()
        {
            try
            {
                SelectedAllTab = true;

                List<string> boolList = new List<string>();
                boolList.Add("True");
                boolList.Add("False");

                Dictionary<int, string> dictStrategies = CachedDataManager.GetInstance.GetUserStrategiesDictionary();
                if (dictStrategies != null && dictStrategies.Count > 0 && dictStrategies.Keys.Max() <= 0)
                    dictStrategies = new Dictionary<int, string>();

                AllocationFilterFields allocationFilter = new AllocationFilterFields();
                allocationFilter.Fund = CachedDataManager.GetInstance.GetUserMasterFunds();
                allocationFilter.Account = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                allocationFilter.Side = TagDatabase.GetInstance().OrderSide;
                allocationFilter.Asset = CachedDataManager.GetInstance.GetAllAssets();
                allocationFilter.Strategy = dictStrategies;
                allocationFilter.Exchange = CachedDataManager.GetInstance.GetAllExchanges();
                allocationFilter.Currency = CachedDataManager.GetInstance.GetAllCurrencies();
                allocationFilter.Underlying = CachedDataManager.GetInstance.GetAllUnderlyings();
                allocationFilter.Broker = CachedDataManager.GetInstance.GetAllCounterParties();
                allocationFilter.Venue = CachedDataManager.GetInstance.GetAllVenues();
                allocationFilter.TradingAccount = CachedDataManager.GetInstance.GetUserTradingAccountsAsDict();
                allocationFilter.BoolList = boolList;

                _unallocatedFilterControl.LoadAllocationFilterControlData(allocationFilter);
                _allFilterControl.LoadAllocationFilterControlData(allocationFilter);
                _allocatedFilterControl.LoadAllocationFilterControlData(allocationFilter);

                _unallocatedFilterControl.AccountComboVisibility = Visibility.Collapsed;

                if (!CachedDataManager.GetInstance.IsShowMasterFundonTT())
                    _unallocatedFilterControl.FundsComboVisibility = Visibility.Collapsed;
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
