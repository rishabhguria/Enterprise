// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 07-24-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-09-2014
// ***********************************************************************
// <copyright file="StateCacheStore.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Common.Helper;
using Prana.Allocation.Core.DataAccess;
using Prana.Allocation.Core.Enums;
using Prana.Allocation.Core.FormulaStore;
using Prana.Allocation.Core.Helper;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// The CacheStore namespace.
/// </summary>
namespace Prana.Allocation.Core.CacheStore
{
    /// <summary>
    /// This singleton cache class stores state for different allocation preferences
    /// </summary>
    internal sealed class StateCacheStore
    {
        bool _matchClosingTransactionAtPortfolioOnly = true;

        public bool MatchClosingTransactionAtPortfolioOnly
        {
            get { return _matchClosingTransactionAtPortfolioOnly; }
        }

        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static readonly StateCacheStore _singletonInstance = new StateCacheStore();

        /// <summary>
        /// Instance method to return the singleton instance of the object in the memory
        /// </summary>
        /// <value>The instance.</value>
        internal static StateCacheStore Instance
        {
            get
            {
                return _singletonInstance;
            }
        }

        /// <summary>
        /// Private constructor to restrict object creation
        /// </summary>
        private StateCacheStore()
        {
            // Initialize base objects which does not require database connection
        }

        /// <summary>
        /// Private locker object for State cache
        /// </summary>
        private readonly object _lockerObject = new object();

        /// <summary>
        /// StateCache for notional
        /// </summary>
        private SerializableDictionary<DateTime, SerializableDictionary<string, List<AccountValue>>> _stateCacheNotional;

        /// <summary>
        /// StateCache for cumQuantity
        /// </summary>
        private SerializableDictionary<DateTime, SerializableDictionary<string, List<AccountValue>>> _stateCacheCumQuantity;

        /// <summary>
        /// StateCache for notional
        /// </summary>
        private SerializableDictionary<DateTime, SerializableDictionary<string, List<AllocationState>>> _allocationStateCacheNotional;

        /// <summary>
        /// StateCache for cumQuantity
        /// </summary>
        private SerializableDictionary<DateTime, SerializableDictionary<string, List<AllocationState>>> _allocationStateCacheCumQuantity;


        /// <summary>
        /// PreferenceId wise date cache
        /// </summary>
        private SerializableDictionary<int, DateTime> _preferenceWiseDateTime;


        /// <summary>
        /// Returns current state for given preferenceId, Use -1 as keyToFindState for MatchingRuleType = Inception
        /// </summary>
        /// <param name="keyToFindState">Id for which data will be fetched</param>
        /// <param name="baseType">Allocation base type</param>
        /// <param name="symbol">Symbol for which data is required</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Dictionary of account value which consists the state for given parameters</returns>
        internal Dictionary<int, AccountValue> GetCurrentState(int keyToFindState, AllocationBaseType baseType, string symbol, int userId)
        {
            try
            {
                if (keyToFindState == int.MinValue)
                    return null;

                lock (_lockerObject)
                {
                    DateTime timeToCheck = DateTimeConstants.MinValue.Date;

                    if (_preferenceWiseDateTime.ContainsKey(keyToFindState))
                    {
                        timeToCheck = _preferenceWiseDateTime[keyToFindState];
                    }


                    //if (userId != -1)
                    //{
                    //    Dictionary<int, AccountValue> fromUserState = UserWiseStateCache.Instance.GetStateForUser(userId, timeToCheck, symbol, baseType);
                    //    if (fromUserState != null)
                    //        return fromUserState;
                    //}


                    switch (baseType)
                    {
                        case AllocationBaseType.CumQuantity:
                            {
                                if (_stateCacheCumQuantity.ContainsKey(timeToCheck) && _stateCacheCumQuantity[timeToCheck].ContainsKey(symbol))
                                    return _stateCacheCumQuantity[timeToCheck][symbol].ToDictionary(i => i.AccountId, i => i.Clone());
                                else
                                    return null;
                            }
                        case AllocationBaseType.Notional:
                            {
                                if (_stateCacheNotional.ContainsKey(timeToCheck) && _stateCacheNotional[timeToCheck].ContainsKey(symbol))
                                    return _stateCacheNotional[timeToCheck][symbol].ToDictionary(i => i.AccountId, i => i.Clone());
                                else
                                    return null;
                            }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Initialize the cache.
        /// Loads data from database
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool Initialize()
        {
            try
            {
                lock (_lockerObject)
                {
                    _stateCacheCumQuantity = new SerializableDictionary<DateTime, SerializableDictionary<string, List<AccountValue>>>();
                    _stateCacheNotional = new SerializableDictionary<DateTime, SerializableDictionary<string, List<AccountValue>>>();
                    _allocationStateCacheCumQuantity = new SerializableDictionary<DateTime, SerializableDictionary<string, List<AllocationState>>>();
                    _allocationStateCacheNotional = new SerializableDictionary<DateTime, SerializableDictionary<string, List<AllocationState>>>();

                    _matchClosingTransactionAtPortfolioOnly = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("MatchClosingTransactionAtPortfolioOnly"));
                    _preferenceWiseDateTime = new SerializableDictionary<int, DateTime>();
                    _preferenceWiseDateTime.Add(-1, DateTimeConstants.MinValue.Date);
                    List<int> preferenceIdCollection = CalculatedPreferenceCache.Instance.GetAllVisiblePreferenceId();
                    foreach (int id in preferenceIdCollection)
                    {
                        DateTime updateDateTime = CalculatedPreferenceCache.Instance.GetPreferenceById(id).UpdateDateTime.Date;
                        _preferenceWiseDateTime.Add(id, updateDateTime);
                    }

                    foreach (int preferenceId in _preferenceWiseDateTime.Keys)
                    {
                        DateTime updateDateTime = _preferenceWiseDateTime[preferenceId];
                        GetStateForDate(updateDateTime);

                        if (!_matchClosingTransactionAtPortfolioOnly)
                            GetAllocationStateForDate(updateDateTime);
                        //if (!_stateCacheCumQuantity.ContainsKey(updateDateTime) || !_stateCacheNotional.ContainsKey(updateDateTime))
                        //{
                        //    SerializableDictionary<string, List<AccountValue>> stateNotional;
                        //    SerializableDictionary<string, List<AccountValue>> stateCumQuantity;

                        //    AllocationPrefDataManager.GetState(updateDateTime, out stateNotional, out stateCumQuantity);
                        //    // Adding state for all preferenceId
                        //    _stateCacheNotional.Add(updateDateTime, stateNotional);
                        //    _stateCacheCumQuantity.Add(updateDateTime, stateCumQuantity);
                        //}
                    }

                    // Adding today date so that if any preference is applied today should have any state for it
                    DateTime updateDateTimeToday = DateTime.UtcNow.Date;
                    GetStateForDate(updateDateTimeToday);
                    if (!_matchClosingTransactionAtPortfolioOnly)
                        GetAllocationStateForDate(updateDateTimeToday);
                    //if (!_stateCacheCumQuantity.ContainsKey(updateDateTimeToday) || !_stateCacheNotional.ContainsKey(updateDateTimeToday))
                    //{
                    //    SerializableDictionary<string, List<AccountValue>> stateNotional;
                    //    SerializableDictionary<string, List<AccountValue>> stateCumQuantity;

                    //    AllocationPrefDataManager.GetState(updateDateTimeToday, out stateNotional, out stateCumQuantity);
                    //    // Adding state for all preferenceId
                    //    _stateCacheNotional.Add(updateDateTimeToday, stateNotional);
                    //    _stateCacheCumQuantity.Add(updateDateTimeToday, stateCumQuantity);
                    //}

                    //List<int> prorataDaysBack = PreferenceCacheStore.Instance.GetAllDaysForProrataPref();
                    //foreach (int days in prorataDaysBack)
                    //{
                    //    if (days != 0)
                    //        GetStateForDate(DateTime.UtcNow.Date.AddDays(-1 * days));
                    //}

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Update state in state cache.
        /// </summary>
        /// <param name="updateDateTime">Time for which date to be updated</param>
        private void GetStateForDate(DateTime updateDateTime)
        {
            try
            {
                if (!_stateCacheCumQuantity.ContainsKey(updateDateTime) || !_stateCacheNotional.ContainsKey(updateDateTime))
                {
                    SerializableDictionary<string, List<AccountValue>> stateNotional;
                    SerializableDictionary<string, List<AccountValue>> stateCumQuantity;

                    AllocationPrefDataManager.GetState(updateDateTime, out stateNotional, out stateCumQuantity);

                    _stateCacheNotional.Add(updateDateTime, stateNotional);
                    _stateCacheCumQuantity.Add(updateDateTime, stateCumQuantity);
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
        /// Update state in state cache.
        /// </summary>
        /// <param name="updateDateTime">Time for which date to be updated</param>
        private void GetAllocationStateForDate(DateTime updateDateTime)
        {
            try
            {
                if (!_allocationStateCacheCumQuantity.ContainsKey(updateDateTime) || !_allocationStateCacheNotional.ContainsKey(updateDateTime))
                {
                    SerializableDictionary<string, List<AllocationState>> stateNotional;
                    SerializableDictionary<string, List<AllocationState>> stateCumQuantity;

                    AllocationPrefDataManager.GetAllocationStateWithAccountStrategy(updateDateTime, out stateNotional, out stateCumQuantity);

                    _allocationStateCacheNotional.Add(updateDateTime, stateNotional);
                    _allocationStateCacheCumQuantity.Add(updateDateTime, stateCumQuantity);
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
        /// Updates the stateCache. If notional and cumqty does not exist then it also updates from database
        /// </summary>
        /// <param name="preferenceId">Id of the preference which is updated</param>
        /// <param name="timeToUpdate">Time at which it is updated</param>
        /// <returns>True if update successfully otherwise false</returns>
        internal bool UpdatePreferenceWiseDate(int preferenceId, DateTime timeToUpdate)
        {
            try
            {
                lock (_lockerObject)
                {
                    timeToUpdate = timeToUpdate.Date;
                    if (_preferenceWiseDateTime.ContainsKey(preferenceId))
                        _preferenceWiseDateTime.Remove(preferenceId);
                    _preferenceWiseDateTime.Add(preferenceId, timeToUpdate);

                    // Adding state for all preferenceId
                    GetStateForDate(timeToUpdate);
                    if (!_matchClosingTransactionAtPortfolioOnly)
                        GetAllocationStateForDate(timeToUpdate);

                    //if (!_stateCacheCumQuantity.ContainsKey(timeToUpdate) || !_stateCacheNotional.ContainsKey(timeToUpdate))
                    //{
                    //    SerializableDictionary<string, List<AccountValue>> stateNotional;
                    //    SerializableDictionary<string, List<AccountValue>> stateCumQuantity;

                    //    AllocationPrefDataManager.GetState(timeToUpdate, out stateNotional, out stateCumQuantity);
                    //    // Adding state for all preferenceId
                    //    _stateCacheNotional.Add(timeToUpdate, stateNotional);
                    //    _stateCacheCumQuantity.Add(timeToUpdate, stateCumQuantity);
                    //}
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }

        }

        /// <summary>
        /// Updates the cache.
        /// </summary>
        /// <param name="groups">The groups.</param>        
        /// <param name="sytemStateUpdateParameter">-1 if updating before operation, 1 if updating after operation</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool UpdateCache(List<AllocationGroup> groups, int sytemStateUpdateParameter)
        {
            try
            {
                lock (_lockerObject)
                {
                    foreach (AllocationGroup group in groups)
                    {
                        String symbol = CommonHelper.GetSwapSymbol(group.Symbol, group.IsSwapped);//Updating state based Swap parameters
                        foreach (DateTime keyCumQty in _stateCacheCumQuantity.Keys)
                        {
                            if (keyCumQty <= group.AUECLocalDate)
                            {
                                foreach (TaxLot taxlot in group.TaxLots)
                                {
                                    if (taxlot.Level1ID != 0)
                                    {
                                        decimal value = sytemStateUpdateParameter * (decimal)taxlot.TaxLotQty * Calculations.GetSideMultilpier(group.OrderSideTagValue);
                                        // if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                                        //    value = -1 * value;

                                        if (!_stateCacheCumQuantity[keyCumQty].ContainsKey(symbol))
                                            _stateCacheCumQuantity[keyCumQty].Add(symbol, new List<AccountValue>());

                                        AccountValue val = _stateCacheCumQuantity[keyCumQty][symbol].Find(f => f.AccountId == taxlot.Level1ID);
                                        if (val == null)
                                        {
                                            val = new AccountValue(taxlot.Level1ID, value);
                                            _stateCacheCumQuantity[keyCumQty][symbol].Add(val.Clone());
                                        }
                                        else
                                        {
                                            if (value != 0)
                                                if (taxlot.TaxLotState != ApplicationConstants.TaxLotState.Deleted && taxlot.TaxLotState != ApplicationConstants.TaxLotState.NotChanged)
                                                    val.AddValue(value);
                                        }

                                        //Update the account,strategy, symbol,side cache
                                        if (!_matchClosingTransactionAtPortfolioOnly)
                                            UpdateAllocationStateCumQuantity(symbol, keyCumQty, taxlot, value);
                                    }
                                }
                            }
                        }

                        foreach (DateTime keyNotional in _stateCacheNotional.Keys)
                        {
                            if (keyNotional <= group.AUECLocalDate)
                            {
                                foreach (TaxLot taxlot in group.TaxLots)
                                {

                                    if (taxlot.Level1ID != 0)
                                    {
                                        decimal value = sytemStateUpdateParameter * NotionalCalculator.GetNotional(group, (decimal)taxlot.TaxLotQty);
                                        //  if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                                        //    value = -1 * value;

                                        if (!_stateCacheNotional[keyNotional].ContainsKey(symbol))
                                            _stateCacheNotional[keyNotional].Add(symbol, new List<AccountValue>());

                                        AccountValue val = _stateCacheNotional[keyNotional][symbol].Find(f => f.AccountId == taxlot.Level1ID);
                                        if (val == null)
                                        {
                                            val = new AccountValue(taxlot.Level1ID, value);
                                            _stateCacheNotional[keyNotional][symbol].Add(val.Clone());
                                        }
                                        else
                                        {
                                            if (taxlot.TaxLotState != ApplicationConstants.TaxLotState.Deleted && taxlot.TaxLotState != ApplicationConstants.TaxLotState.NotChanged)
                                                val.AddValue(value);
                                        }

                                        //Update the account,strategy, symbol,side cache
                                        if (!_matchClosingTransactionAtPortfolioOnly)
                                            UpdateAllocationStateNotional(symbol, keyNotional, taxlot, value);
                                    }
                                }
                            }
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Update Allocation State Notional
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="keyNotional"></param>
        /// <param name="taxlot"></param>
        /// <param name="value"></param>
        private void UpdateAllocationStateNotional(string symbol, DateTime keyNotional, TaxLot taxlot, decimal value)
        {
            try
            {
                AllocationState notionalAccountValue = new AllocationState()
                {
                    AccountId = taxlot.Level1ID,
                    Level2ID = taxlot.Level2ID,
                    OrderSideTagValue = taxlot.OrderSideTagValue,
                    Notional = value
                };

                if (!_allocationStateCacheNotional[keyNotional].ContainsKey(symbol))
                {
                    _allocationStateCacheNotional[keyNotional].Add(symbol, new List<AllocationState>());
                    _allocationStateCacheNotional[keyNotional][symbol].Add(notionalAccountValue.Clone());
                }
                else
                {
                    AllocationState val = _allocationStateCacheNotional[keyNotional][symbol].Find(f => f.AccountId == taxlot.Level1ID && f.Level2ID == taxlot.Level2ID && f.OrderSideTagValue == taxlot.OrderSideTagValue);
                    if (val == null)
                    {
                        _allocationStateCacheNotional[keyNotional][symbol].Add(notionalAccountValue.Clone());
                    }
                    else
                    {
                        val.AddValueNotional(value);
                    }

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
        }

        /// <summary>
        /// Update Allocation State CumQuantity
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="keyCumQty"></param>
        /// <param name="taxlot"></param>
        /// <param name="value"></param>
        private void UpdateAllocationStateCumQuantity(String symbol, DateTime keyCumQty, TaxLot taxlot, decimal value)
        {
            try
            {
                AllocationState cumQuantityAccountValue = new AllocationState()
                {
                    AccountId = taxlot.Level1ID,
                    Level2ID = taxlot.Level2ID,
                    OrderSideTagValue = taxlot.OrderSideTagValue,
                    cumQuantity = value
                };

                if (!_allocationStateCacheCumQuantity[keyCumQty].ContainsKey(symbol))
                {
                    _allocationStateCacheCumQuantity[keyCumQty].Add(symbol, new List<AllocationState>());
                    _allocationStateCacheCumQuantity[keyCumQty][symbol].Add(cumQuantityAccountValue.Clone());
                }
                else
                {
                    var orderSideTagValue = taxlot.OrderSideTagValue;

                    if (CheckSideHelper.GetPositionKey(taxlot.OrderSideTagValue).Equals(GroupPositionType.LongClosing) ||
                       CheckSideHelper.GetPositionKey(taxlot.OrderSideTagValue).Equals(GroupPositionType.ShortClosing))
                    {
                        orderSideTagValue = CheckSideHelper.GetOpeningOrderSideTagValue(taxlot.OrderSideTagValue);
                        cumQuantityAccountValue.OrderSideTagValue = orderSideTagValue;
                    }
                    AllocationState val = _allocationStateCacheCumQuantity[keyCumQty][symbol].Find(f => f.AccountId == taxlot.Level1ID && f.Level2ID == taxlot.Level2ID && f.OrderSideTagValue == orderSideTagValue);
                    if (val == null)
                    {
                        _allocationStateCacheCumQuantity[keyCumQty][symbol].Add(cumQuantityAccountValue.Clone());
                    }
                    else
                    {
                        val.AddValueCumQuantity(value);
                    }

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
        }

        /// <summary>
        /// Returns time to check for key received.
        /// </summary>
        /// <param name="keyToFindState"></param>
        /// <returns></returns>
        internal DateTime GetTimeToCheckForKey(int keyToFindState)
        {
            try
            {
                DateTime timeToCheck = DateTimeConstants.MinValue.Date;

                if (_preferenceWiseDateTime.ContainsKey(keyToFindState))
                {
                    timeToCheck = _preferenceWiseDateTime[keyToFindState];
                }
                return timeToCheck;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return DateTimeConstants.MinValue.Date;
            }
        }

        /// <summary>
        /// Gets Current state for symbol for updating allocation user state
        /// </summary>
        /// <param name="dateTime">from date</param>
        /// <param name="baseType">Allocation base type</param>
        /// <param name="symbol">allcoation symbol</param>
        /// <returns>state for keys</returns>
        internal Dictionary<int, AccountValue> GetCurrentState(DateTime dateTime, AllocationBaseType baseType, string symbol)
        {
            try
            {
                lock (_lockerObject)
                {
                    DateTime timeToCheck = dateTime;


                    switch (baseType)
                    {
                        case AllocationBaseType.CumQuantity:
                            {
                                if (_stateCacheCumQuantity.ContainsKey(timeToCheck) && _stateCacheCumQuantity[timeToCheck].ContainsKey(symbol))
                                    return _stateCacheCumQuantity[timeToCheck][symbol].ToDictionary(i => i.AccountId, i => i.Clone());
                                else
                                    return null;
                            }
                        case AllocationBaseType.Notional:
                            {
                                if (_stateCacheNotional.ContainsKey(timeToCheck) && _stateCacheNotional[timeToCheck].ContainsKey(symbol))
                                    return _stateCacheNotional[timeToCheck][symbol].ToDictionary(i => i.AccountId, i => i.Clone());
                                else
                                    return null;
                            }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets state for symbol from DB
        /// </summary>
        /// <param name="updateDateTime">time</param>
        /// <param name="allocationBaseType">Type</param>
        /// <param name="symbol">symbol</param>
        /// <returns></returns>
        internal Dictionary<int, AccountValue> GetStateFromDB(DateTime updateDateTime, AllocationBaseType allocationBaseType, string symbol, StringBuilder groupIds)
        {
            try
            {
                SerializableDictionary<int, AccountValue> stateNotional;
                SerializableDictionary<int, AccountValue> stateCumQuantity;

                AllocationPrefDataManager.GetStateForSymbol(updateDateTime, out stateNotional, out stateCumQuantity, symbol, groupIds);

                switch (allocationBaseType)
                {
                    case AllocationBaseType.CumQuantity:
                        return stateCumQuantity;
                    case AllocationBaseType.Notional:
                        return stateNotional;
                }
                return null;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets state for symbol with Account Strategy
        /// </summary>
        /// <param name="updateDateTime">time</param>
        /// <param name="allocationBaseType">Type</param>
        /// <param name="symbol">symbol</param>
        /// <returns></returns>
        internal List<AllocationState> GetAllocationStateWithAccountStrategy(DateTime updateDateTime, AllocationBaseType baseType, string symbol)
        {
            try
            {

                List<AllocationState> allocationState = null;
                lock (_lockerObject)
                {
                    DateTime timeToCheck = updateDateTime;

                    switch (baseType)
                    {
                        case AllocationBaseType.CumQuantity:

                            if (_allocationStateCacheCumQuantity.ContainsKey(timeToCheck) && _allocationStateCacheCumQuantity[timeToCheck].ContainsKey(symbol))
                            {
                                allocationState = _allocationStateCacheCumQuantity[timeToCheck][symbol].Select(x => x.Clone()).ToList();
                            }

                            break;
                        case AllocationBaseType.Notional:

                            if (_allocationStateCacheNotional.ContainsKey(timeToCheck) && _allocationStateCacheNotional[timeToCheck].ContainsKey(symbol))
                            {
                                allocationState = _allocationStateCacheNotional[timeToCheck][symbol].Select(x => x.Clone()).ToList();
                            }
                            break;

                    }
                }

                return allocationState;

            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }


    }
}
