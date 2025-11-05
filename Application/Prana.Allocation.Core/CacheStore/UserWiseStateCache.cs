// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-09-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-09-2014
// ***********************************************************************
// <copyright file="User wiseStateCache.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Common.Helper;
using Prana.Allocation.Core.Enums;
using Prana.Allocation.Core.FormulaStore;
using Prana.Allocation.Core.Helper;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
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
    /// Class UserWiseStateCache.
    /// </summary>
    internal class UserWiseStateCache
    {
        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static readonly UserWiseStateCache _singletonInstance = new UserWiseStateCache();

        /// <summary>
        /// Instance method to return the singleton instance of the object in the memory
        /// </summary>
        /// <value>The instance.</value>
        internal static UserWiseStateCache Instance
        {
            get
            {
                return _singletonInstance;
            }
        }

        /// <summary>
        /// Private constructor to restrict object creation
        /// </summary>
        private UserWiseStateCache()
        {
            // Initialize base objects which does not require database connection
            try
            {
                lock (_lockerObject)
                {
                    _userWiseCacheCumQuantity = new Dictionary<int, Dictionary<DateTime, Dictionary<string, List<AccountValue>>>>();
                    _userWiseCacheNotional = new Dictionary<int, Dictionary<DateTime, Dictionary<string, List<AccountValue>>>>();

                    _userWiseAllocationCacheCumQuantity = new Dictionary<int, Dictionary<DateTime, Dictionary<string, List<AllocationState>>>>();
                    _userWiseAllocationCacheNotional = new Dictionary<int, Dictionary<DateTime, Dictionary<string, List<AllocationState>>>>();
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
        /// Private locker object for State cache
        /// </summary>
        private readonly object _lockerObject = new object();


        /// <summary>
        /// The _user wise cache cum quantity
        /// </summary>
        Dictionary<int, Dictionary<DateTime, Dictionary<string, List<AccountValue>>>> _userWiseCacheCumQuantity;
        /// <summary>
        /// The _user wise cache notional
        /// </summary>
        Dictionary<int, Dictionary<DateTime, Dictionary<string, List<AccountValue>>>> _userWiseCacheNotional;

        /// <summary>
        /// The _user wise cache cum quantity
        /// </summary>
        Dictionary<int, Dictionary<DateTime, Dictionary<string, List<AllocationState>>>> _userWiseAllocationCacheCumQuantity;
        /// <summary>
        /// The _user wise cache notional
        /// </summary>
        Dictionary<int, Dictionary<DateTime, Dictionary<string, List<AllocationState>>>> _userWiseAllocationCacheNotional;

        /// <summary>
        /// Gets the state for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="dateTime">The date time.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="baseType">Type of the base.</param>
        /// <returns>Dictionary&lt;System.Int32, AccountValue&gt;.</returns>
        private Dictionary<int, AccountValue> GetStateForUser(int userId, DateTime dateTime, string symbol, AllocationBaseType baseType)
        {
            try
            {
                lock (_lockerObject)
                {
                    dateTime = dateTime.Date;
                    switch (baseType)
                    {
                        case AllocationBaseType.CumQuantity:
                            if (!_userWiseCacheCumQuantity.ContainsKey(userId))
                                return null;
                            if (!_userWiseCacheCumQuantity[userId].ContainsKey(dateTime))
                                return null;
                            if (!_userWiseCacheCumQuantity[userId][dateTime].ContainsKey(symbol))
                                return null;

                            return _userWiseCacheCumQuantity[userId][dateTime][symbol].ToDictionary(i => i.AccountId, i => i.Clone());

                        case AllocationBaseType.Notional:
                            if (!_userWiseCacheNotional.ContainsKey(userId))
                                return null;
                            if (!_userWiseCacheNotional[userId].ContainsKey(dateTime))
                                return null;
                            if (!_userWiseCacheNotional[userId][dateTime].ContainsKey(symbol))
                                return null;

                            return _userWiseCacheNotional[userId][dateTime][symbol].ToDictionary(i => i.AccountId, i => i.Clone());
                        default:
                            return null;
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
                return null;
            }
        }

        /// <summary>
        /// Gets the allocation state for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="dateTime">The date time.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="baseType">Type of the base.</param>
        /// <returns>Dictionary&lt;System.Int32, AccountValue&gt;.</returns>
        private List<AllocationState> GetAllocationStateForUser(int userId, DateTime dateTime, string symbol, AllocationBaseType baseType)
        {
            try
            {
                lock (_lockerObject)
                {
                    dateTime = dateTime.Date;
                    switch (baseType)
                    {
                        case AllocationBaseType.CumQuantity:
                            if (!_userWiseAllocationCacheCumQuantity.ContainsKey(userId))
                                return null;
                            if (!_userWiseAllocationCacheCumQuantity[userId].ContainsKey(dateTime))
                                return null;
                            if (!_userWiseAllocationCacheCumQuantity[userId][dateTime].ContainsKey(symbol))
                                return null;

                            return _userWiseAllocationCacheCumQuantity[userId][dateTime][symbol];

                        case AllocationBaseType.Notional:
                            if (!_userWiseAllocationCacheNotional.ContainsKey(userId))
                                return null;
                            if (!_userWiseAllocationCacheNotional[userId].ContainsKey(dateTime))
                                return null;
                            if (!_userWiseAllocationCacheNotional[userId][dateTime].ContainsKey(symbol))
                                return null;

                            return _userWiseAllocationCacheNotional[userId][dateTime][symbol];
                        default:
                            return null;
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
                return null;
            }
        }


        /// <summary>
        /// Gets the initial state for any user.
        /// </summary>
        /// <returns>Dictionary&lt;DateTime, Dictionary&lt;System.String, List&lt;AccountValue&gt;&gt;&gt;.</returns>
        private Dictionary<DateTime, Dictionary<string, List<AccountValue>>> GetInitialStateForAnyUser()
        {
            try
            {
                Dictionary<DateTime, Dictionary<string, List<AccountValue>>> result = new Dictionary<DateTime, Dictionary<string, List<AccountValue>>>();
                List<int> preferenceIdCollection = CalculatedPreferenceCache.Instance.GetAllPreferenceId();
                foreach (int id in preferenceIdCollection)
                {
                    DateTime updateDateTime = CalculatedPreferenceCache.Instance.GetPreferenceById(id).UpdateDateTime.Date;
                    if (!result.ContainsKey(updateDateTime))
                        result.Add(updateDateTime, new Dictionary<string, List<AccountValue>>());
                }
                if (!result.ContainsKey(DateTimeConstants.MinValue.Date))
                    result.Add(DateTimeConstants.MinValue.Date, new Dictionary<string, List<AccountValue>>());
                if (!result.ContainsKey(DateTime.UtcNow.Date))
                    result.Add(DateTime.UtcNow.Date, new Dictionary<string, List<AccountValue>>());
                return result;
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
        /// Gets the initial state for any user.
        /// </summary>
        /// <returns>Dictionary&lt;DateTime, Dictionary&lt;System.String, List&lt;AccountValue&gt;&gt;&gt;.</returns>
        private Dictionary<DateTime, Dictionary<string, List<AllocationState>>> GetInitialAllocationStateForAnyUser()
        {
            try
            {
                Dictionary<DateTime, Dictionary<string, List<AllocationState>>> result = new Dictionary<DateTime, Dictionary<string, List<AllocationState>>>();
                List<int> preferenceIdCollection = CalculatedPreferenceCache.Instance.GetAllPreferenceId();
                foreach (int id in preferenceIdCollection)
                {
                    DateTime updateDateTime = CalculatedPreferenceCache.Instance.GetPreferenceById(id).UpdateDateTime.Date;
                    if (!result.ContainsKey(updateDateTime))
                        result.Add(updateDateTime, new Dictionary<string, List<AllocationState>>());
                }
                if (!result.ContainsKey(DateTimeConstants.MinValue.Date))
                    result.Add(DateTimeConstants.MinValue.Date, new Dictionary<string, List<AllocationState>>());
                if (!result.ContainsKey(DateTime.UtcNow.Date))
                    result.Add(DateTime.UtcNow.Date, new Dictionary<string, List<AllocationState>>());
                return result;
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
        /// Updates the state for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="modifiedDate">The modified date.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="newCumQuantity">The new cum quantity.</param>
        /// <param name="newNotional">The new notional.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool UpdateStateForUser(int userId, DateTime modifiedDate, string symbol, List<AccountValue> newCumQuantity, List<AccountValue> newNotional, string orderSideTagValue)
        {
            try
            {
                lock (_lockerObject)
                {
                    modifiedDate = modifiedDate.Date;
                    if (!_userWiseCacheCumQuantity.ContainsKey(userId))
                        _userWiseCacheCumQuantity.Add(userId, GetInitialStateForAnyUser());

                    if (!_userWiseAllocationCacheCumQuantity.ContainsKey(userId))
                        _userWiseAllocationCacheCumQuantity.Add(userId, GetInitialAllocationStateForAnyUser());

                    var resultCumQty = from key in _userWiseCacheCumQuantity[userId]
                                       where key.Key <= modifiedDate
                                       select key.Key;

                    foreach (DateTime dateTime in resultCumQty)
                    {

                        if (!_userWiseCacheCumQuantity[userId][dateTime].ContainsKey(symbol))
                        {
                            Dictionary<int, AccountValue> originalAccountVal = StateCacheStore.Instance.GetCurrentState(dateTime, AllocationBaseType.CumQuantity, symbol);
                            _userWiseCacheCumQuantity[userId][dateTime].Add(symbol, new List<AccountValue>());
                            if (originalAccountVal != null && originalAccountVal.Count > 0)
                                _userWiseCacheCumQuantity[userId][dateTime][symbol].AddRange(originalAccountVal.Values);
                        }

                        if (!_userWiseAllocationCacheCumQuantity[userId][dateTime].ContainsKey(symbol))
                        {
                            List<AllocationState> originalAllocationState = StateCacheStore.Instance.GetAllocationStateWithAccountStrategy(dateTime, AllocationBaseType.CumQuantity, symbol);
                            _userWiseAllocationCacheCumQuantity[userId][dateTime].Add(symbol, new List<AllocationState>());
                            if (originalAllocationState != null && originalAllocationState.Count > 0)
                                _userWiseAllocationCacheCumQuantity[userId][dateTime][symbol].AddRange(originalAllocationState);
                        }

                        foreach (AccountValue fvCumQuantity in newCumQuantity)
                        {
                            if (fvCumQuantity.AccountId != 0)
                            {
                                AccountValue existingAccountVal = _userWiseCacheCumQuantity[userId][dateTime][symbol].Find(fv => fv.AccountId == fvCumQuantity.AccountId);
                                if (existingAccountVal == null)
                                {
                                    _userWiseCacheCumQuantity[userId][dateTime][symbol].Add(fvCumQuantity.Clone());
                                    //if (originalAccountVal == null || !originalAccountVal.ContainsKey(fvCumQuantity.AccountId))

                                    //else
                                    //{
                                    //    originalAccountVal[fvCumQuantity.AccountId].AddValue(fvCumQuantity.Value);
                                    //    _userWiseCacheCumQuantity[userId][dateTime][symbol].Add(originalAccountVal[fvCumQuantity.AccountId].Clone());
                                    //}
                                }
                                else
                                    existingAccountVal.AddValue(fvCumQuantity.Value);

                                //for Allocation state at account orderSide and strategy
                                foreach (var strategyValue in fvCumQuantity.StrategyValueList)
                                {
                                    UpdateAllocationStateCumQuantity(symbol, userId, dateTime, fvCumQuantity.AccountId, strategyValue.StrategyId, orderSideTagValue, fvCumQuantity.Value);
                                }
                            }
                        }
                    }



                    if (!_userWiseCacheNotional.ContainsKey(userId))
                        _userWiseCacheNotional.Add(userId, GetInitialStateForAnyUser());

                    if (!_userWiseAllocationCacheNotional.ContainsKey(userId))
                        _userWiseAllocationCacheNotional.Add(userId, GetInitialAllocationStateForAnyUser());

                    var resultNotional = from key in _userWiseCacheNotional[userId]
                                         where key.Key <= modifiedDate
                                         select key.Key;

                    foreach (DateTime dateTime in resultNotional)
                    {

                        if (!_userWiseCacheNotional[userId][dateTime].ContainsKey(symbol))
                        {
                            Dictionary<int, AccountValue> originalAccountVal = StateCacheStore.Instance.GetCurrentState(dateTime, AllocationBaseType.Notional, symbol);
                            _userWiseCacheNotional[userId][dateTime].Add(symbol, new List<AccountValue>());
                            if (originalAccountVal != null && originalAccountVal.Count > 0)
                                _userWiseCacheNotional[userId][dateTime][symbol].AddRange(originalAccountVal.Values);
                        }

                        if (!_userWiseAllocationCacheNotional[userId][dateTime].ContainsKey(symbol))
                        {
                            List<AllocationState> originalAllocationStateNotional = StateCacheStore.Instance.GetAllocationStateWithAccountStrategy(dateTime, AllocationBaseType.Notional, symbol);
                            _userWiseAllocationCacheNotional[userId][dateTime].Add(symbol, new List<AllocationState>());
                            if (originalAllocationStateNotional != null && originalAllocationStateNotional.Count > 0)
                                _userWiseAllocationCacheNotional[userId][dateTime][symbol].AddRange(originalAllocationStateNotional);
                        }

                        foreach (AccountValue fvNotional in newNotional)
                        {
                            if (fvNotional.AccountId != 0)
                            {
                                AccountValue existingAccountVal = _userWiseCacheNotional[userId][dateTime][symbol].Find(fv => fv.AccountId == fvNotional.AccountId);

                                if (existingAccountVal == null)
                                {
                                    _userWiseCacheNotional[userId][dateTime][symbol].Add(fvNotional.Clone());
                                    //if (originalAccountVal == null || !originalAccountVal.ContainsKey(fvNotional.AccountId))

                                    //else
                                    //{
                                    //    originalAccountVal[fvNotional.AccountId].AddValue(fvNotional.Value);
                                    //    _userWiseCacheNotional[userId][dateTime][symbol].Add(originalAccountVal[fvNotional.AccountId].Clone());
                                    //}
                                }
                                else
                                    existingAccountVal.AddValue(fvNotional.Value);

                                //for Allocation state at account orderSide and strategy
                                foreach (var strategyValue in fvNotional.StrategyValueList)
                                {
                                    UpdateAllocationStateNotional(symbol, userId, dateTime, fvNotional.AccountId, strategyValue.StrategyId, orderSideTagValue, fvNotional.Value);
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
        /// Clears the state for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool ClearStateForUser(int userId)
        {
            try
            {
                lock (_lockerObject)
                {
                    if (_userWiseCacheCumQuantity.ContainsKey(userId))
                        _userWiseCacheCumQuantity.Remove(userId);
                    if (_userWiseCacheNotional.ContainsKey(userId))
                        _userWiseCacheNotional.Remove(userId);

                    if (_userWiseAllocationCacheCumQuantity.ContainsKey(userId))
                        _userWiseAllocationCacheCumQuantity.Remove(userId);
                    if (_userWiseAllocationCacheNotional.ContainsKey(userId))
                        _userWiseAllocationCacheNotional.Remove(userId);
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
        /// Updates the state for all user according to group.
        /// </summary>
        /// <param name="groups">List of AllocationGroup object</param>
        /// <param name="sytemStateUpdateParameter">Tells state to be add or remove</param>
        /// <returns><c>true</c> if successfull, <c>false</c> otherwise.</returns>
        internal bool UpdateStateForUser(List<AllocationGroup> groups, int sytemStateUpdateParameter)
        {
            try
            {
                lock (_lockerObject)
                {
                    foreach (AllocationGroup group in groups)
                    {
                        String symbol = CommonHelper.GetSwapSymbol(group.Symbol, group.IsSwapped);//Updating state based Swap parameters
                        foreach (int userId in _userWiseCacheCumQuantity.Keys)
                        {
                            foreach (DateTime keyCumQty in _userWiseCacheCumQuantity[userId].Keys)
                            {
                                if (keyCumQty <= group.AUECLocalDate)
                                {
                                    foreach (TaxLot taxlot in group.GetAllTaxlots())
                                    {
                                        if (taxlot.Level1ID != 0)
                                        {
                                            decimal value = sytemStateUpdateParameter * (decimal)taxlot.TaxLotQty * Calculations.GetSideMultilpier(group.OrderSideTagValue);
                                            if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                                                value = -1 * value;

                                            if (_userWiseCacheCumQuantity[userId][keyCumQty].ContainsKey(symbol))
                                            {
                                                //_userWiseCacheCumQuantity[userId][keyCumQty].Add(group.Symbol, new List<AccountValue>());

                                                AccountValue val = _userWiseCacheCumQuantity[userId][keyCumQty][symbol].Find(f => f.AccountId == taxlot.Level1ID);
                                                if (val == null)
                                                {
                                                    val = new AccountValue(taxlot.Level1ID, value);
                                                    _userWiseCacheCumQuantity[userId][keyCumQty][symbol].Add(val.Clone());
                                                }
                                                else
                                                    val.AddValue(value);
                                            }

                                            //for Allocation state at account orderSide and strategy
                                            UpdateAllocationStateCumQuantity(symbol, userId, keyCumQty, taxlot.Level1ID, taxlot.Level2ID, taxlot.OrderSideTagValue, value);
                                        }
                                    }
                                }
                            }


                            foreach (DateTime keyNotional in _userWiseCacheNotional[userId].Keys)
                            {
                                if (keyNotional <= group.AUECLocalDate)
                                {
                                    foreach (TaxLot taxlot in group.GetAllTaxlots())
                                    {
                                        if (taxlot.Level1ID != 0)
                                        {

                                            decimal value = sytemStateUpdateParameter * NotionalCalculator.GetNotional(group, (decimal)taxlot.TaxLotQty);
                                            if (taxlot.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                                                value = -1 * value;

                                            if (_userWiseCacheNotional[userId][keyNotional].ContainsKey(symbol))
                                            {
                                                //_userWiseCacheNotional[userId][keyNotional].Add(group.Symbol, new List<AccountValue>());

                                                AccountValue val = _userWiseCacheNotional[userId][keyNotional][symbol].Find(f => f.AccountId == taxlot.Level1ID);
                                                if (val == null)
                                                {
                                                    val = new AccountValue(taxlot.Level1ID, value);
                                                    _userWiseCacheNotional[userId][keyNotional][symbol].Add(val.Clone());
                                                }
                                                else
                                                    val.AddValue(value);
                                            }

                                            //for Allocation state at account orderSide and strategy
                                            UpdateAllocationStateNotional(symbol, userId, keyNotional, taxlot.Level1ID, taxlot.Level2ID, taxlot.OrderSideTagValue, value);
                                        }
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
        /// Update user wise Allocation State Cum Quantity
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="userId"></param>
        /// <param name="keyCumQty"></param>
        /// <param name="taxlot"></param>
        /// <param name="value"></param>
        private void UpdateAllocationStateCumQuantity(String symbol, int userId, DateTime keyCumQty, int level1ID, int level2ID, string orderSideTagValue, decimal value)
        {
            try
            {

                if (_userWiseAllocationCacheCumQuantity[userId][keyCumQty].ContainsKey(symbol))
                {
                    if (CheckSideHelper.GetPositionKey(orderSideTagValue).Equals(GroupPositionType.LongClosing) ||
                       CheckSideHelper.GetPositionKey(orderSideTagValue).Equals(GroupPositionType.ShortClosing))
                    {
                        orderSideTagValue = CheckSideHelper.GetOpeningOrderSideTagValue(orderSideTagValue);
                    }
                    AllocationState val = _userWiseAllocationCacheCumQuantity[userId][keyCumQty][symbol].Find(f => f.AccountId == level1ID && f.Level2ID == level2ID && f.OrderSideTagValue == orderSideTagValue);
                    if (val == null)
                    {
                        AllocationState cumQuantityAccountValue = new AllocationState()
                        {
                            AccountId = level1ID,
                            Level2ID = level2ID,
                            OrderSideTagValue = orderSideTagValue,
                            cumQuantity = value
                        };
                        _userWiseAllocationCacheCumQuantity[userId][keyCumQty][symbol].Add(cumQuantityAccountValue.Clone());
                    }
                    else
                    {
                        val.AddValueCumQuantity(value);
                    }


                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Update User wise Allocation State Notional
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="userId"></param>
        /// <param name="keyCumQty"></param>
        /// <param name="taxlot"></param>
        /// <param name="value"></param>
        private void UpdateAllocationStateNotional(String symbol, int userId, DateTime keyCumQty, int level1ID, int level2ID, string orderSideTagValue, decimal value)
        {
            try
            {
                if (_userWiseAllocationCacheNotional[userId][keyCumQty].ContainsKey(symbol))
                {
                    if (CheckSideHelper.GetPositionKey(orderSideTagValue).Equals(GroupPositionType.LongClosing) ||
                       CheckSideHelper.GetPositionKey(orderSideTagValue).Equals(GroupPositionType.ShortClosing))
                    {
                        orderSideTagValue = CheckSideHelper.GetOpeningOrderSideTagValue(orderSideTagValue);

                    }
                    AllocationState val = _userWiseAllocationCacheNotional[userId][keyCumQty][symbol].Find(f => f.AccountId == level1ID && f.Level2ID == level2ID && f.OrderSideTagValue == orderSideTagValue);
                    if (val == null)
                    {
                        AllocationState cumQuantityAccountValue = new AllocationState()
                        {
                            AccountId = level1ID,
                            Level2ID = level2ID,
                            OrderSideTagValue = orderSideTagValue,
                            Notional = value
                        };
                        _userWiseAllocationCacheNotional[userId][keyCumQty][symbol].Add(cumQuantityAccountValue.Clone());
                    }
                    else
                    {
                        val.AddValueNotional(value);
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }
        }

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

                    timeToCheck = StateCacheStore.Instance.GetTimeToCheckForKey(keyToFindState);
                    //if (_preferenceWiseDateTime.ContainsKey(keyToFindState))
                    //{
                    //    timeToCheck = _preferenceWiseDateTime[keyToFindState];
                    //}

                    Dictionary<int, AccountValue> userState = null;
                    if (userId != -1)
                    {
                        userState = GetStateForUser(userId, timeToCheck, symbol, baseType);
                    }

                    if (userState == null)
                    {
                        userState = StateCacheStore.Instance.GetCurrentState(keyToFindState, baseType, symbol, userId);
                        // if (userState != null)

                    }
                    return userState;

                    //switch (baseType)
                    //{
                    //    case AllocationBaseType.CumQuantity:
                    //        {
                    //            if (_userWiseCacheCumQuantity[userId].ContainsKey(timeToCheck) && _userWiseCacheCumQuantity[userId][timeToCheck].ContainsKey(symbol))
                    //                return _userWiseCacheCumQuantity[userId][timeToCheck][symbol].ToDictionary(i => i.AccountId, i => i.Clone());
                    //            else
                    //                return null;
                    //        }
                    //    case AllocationBaseType.Notional:
                    //        {
                    //            if (_userWiseCacheNotional[userId].ContainsKey(timeToCheck) && _userWiseCacheNotional[userId][timeToCheck].ContainsKey(symbol))
                    //                return _userWiseCacheNotional[userId][timeToCheck][symbol].ToDictionary(i => i.AccountId, i => i.Clone());
                    //            else
                    //                return null;
                    //        }
                    //}
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
                return null;
            }
        }

        /// <summary>
        /// Gets state for Prorata allocation.
        /// </summary>
        /// <param name="noOfDays">days to subtracted</param>
        /// <param name="allocationBaseType">type</param>
        /// <param name="symbol"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal Dictionary<int, AccountValue> GetCurrentStateForDays(int noOfDays, AllocationBaseType allocationBaseType, string symbol, int userId, StringBuilder groupIds)
        {
            try
            {
                lock (_lockerObject)
                {
                    DateTime timeToCheck = DateTime.UtcNow.Date.AddDays(-1 * noOfDays).Date;

                    Dictionary<int, AccountValue> userState = null;
                    //if (userId != -1)
                    //    userState = GetStateForUser(userId, timeToCheck, symbol, allocationBaseType);
                    //if (userState == null)
                    // userState = StateCacheStore.Instance.GetCurrentState(timeToCheck, allocationBaseType, symbol);
                    //if (userState == null)
                    userState = StateCacheStore.Instance.GetStateFromDB(timeToCheck, allocationBaseType, symbol, groupIds);
                    return userState;
                }
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
        /// Gets state for Prorata allocation.
        /// </summary>
        /// <param name="noOfDays">days to subtracted</param>
        /// <param name="allocationBaseType">type</param>
        /// <param name="symbol"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal List<AllocationState> GetCurrentStateWithAccountStrategy(int keyToFindState, AllocationBaseType baseType, string symbol, int userId)
        {
            try
            {
                if (keyToFindState == int.MinValue)
                    return null;

                lock (_lockerObject)
                {
                    DateTime timeToCheck = DateTimeConstants.MinValue.Date;

                    timeToCheck = StateCacheStore.Instance.GetTimeToCheckForKey(keyToFindState);

                    List<AllocationState> portfolioAllocationState = null;


                    if (userId != -1)
                    {
                        portfolioAllocationState = GetAllocationStateForUser(userId, timeToCheck, symbol, baseType);
                    }

                    if (portfolioAllocationState == null)
                    {
                        portfolioAllocationState = StateCacheStore.Instance.GetAllocationStateWithAccountStrategy(timeToCheck, baseType, symbol);

                    }
                    // List<AllocationState> portfolioAllocationState = StateCacheStore.Instance.GetAllocationStateWithAccountStrategy(keyToFindState, timeToCheck, baseType, symbol);

                    return portfolioAllocationState;
                }

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
        /// Gets the master fund current state for days.
        /// </summary>
        /// <param name="noOfDays">The no of days.</param>
        /// <param name="allocationBaseType">Type of the allocation base.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="groupIds">The group ids.</param>
        /// <returns></returns>
        internal Dictionary<int, AccountValue> GetMasterFundCurrentStateForDays(int noOfDays, AllocationBaseType allocationBaseType, string symbol, int userId, StringBuilder groupIds)
        {
            Dictionary<int, AccountValue> userMasterFundState = new Dictionary<int, AccountValue>();
            try
            {
                Dictionary<int, AccountValue> userAccountState = GetCurrentStateForDays(noOfDays, allocationBaseType, symbol, userId, groupIds);
                if (userAccountState != null)
                {
                    userMasterFundState = GetMasterFundStateFromAccountState(userAccountState);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return userMasterFundState;
        }

        /// <summary>
        /// Gets the state of the current master fund.
        /// </summary>
        /// <param name="keyToFindState">State of the key to find.</param>
        /// <param name="baseType">Type of the base.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        internal Dictionary<int, AccountValue> GetCurrentMasterFundState(int keyToFindState, AllocationBaseType baseType, string symbol, int userId)
        {
            Dictionary<int, AccountValue> masterFundState = new Dictionary<int, AccountValue>();
            try
            {
                Dictionary<int, AccountValue> userState = GetCurrentState(keyToFindState, baseType, symbol, userId);
                if (userState != null)
                {
                    masterFundState = GetMasterFundStateFromAccountState(userState);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return masterFundState;
        }

        /// <summary>
        /// Gets the state of the master fund state from account.
        /// </summary>
        /// <param name="userAccountState">State of the user account.</param>
        /// <returns></returns>
        private Dictionary<int, AccountValue> GetMasterFundStateFromAccountState(Dictionary<int, AccountValue> userAccountState)
        {
            Dictionary<int, AccountValue> userMasterFundState = new Dictionary<int, AccountValue>();
            try
            {
                Dictionary<int, List<int>> mfAssociatedAccounts = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                foreach (int mfId in mfAssociatedAccounts.Keys)
                {
                    decimal mfQty = userAccountState.Where(x => mfAssociatedAccounts[mfId].Contains(x.Key)).Sum(y => y.Value.Value);
                    userMasterFundState.Add(mfId, new AccountValue(mfId, mfQty));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return userMasterFundState;
        }
    }
}
