// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-09-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-09-2014
// ***********************************************************************
// <copyright file="NotionalAllocationGenerator.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Common.Enums;
using Prana.Allocation.Core.Allocator;
using Prana.Allocation.Core.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

/// <summary>
/// The Generator namespace.
/// </summary>
namespace Prana.Allocation.Core.Generator
{
    /// <summary>
    /// Notional allocation generator uses Notional as AllocationBaseType to allocate groups
    /// </summary>
    internal class NotionalAllocationGenerator : AccountAllocator
    {
        /// <summary>
        /// The _account wise nav used in leveling process
        /// </summary>
        private ImmutableSortedDictionary<int, decimal> _accountWiseNav;

        /// <summary>
        /// The group wise market value
        /// </summary>
        private ImmutableSortedDictionary<string, double> _groupWiseMarketValue;

        /// <summary>
        /// The symbol account wise market value
        /// </summary>
        Dictionary<string, Dictionary<int, double>> _symbolAccountWiseMarketValue;

        /// <summary>
        /// The symbol market value locker
        /// </summary>
        private readonly object _symbolMarketValueLocker = new object();

        /// <summary>
        /// Updates the type of the base.
        /// </summary>
        public override void UpdateBaseType()
        {
            try
            {
                BaseType = AllocationBaseType.Notional;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the account nav.
        /// </summary>
        /// <param name="allocationLevel">The allocation level.</param>
        /// <param name="rule">The rule.</param>
        /// <param name="groupList">The group list.</param>
        /// <returns></returns>
        public override string UpdateAccountNAV(AllocationLevel allocationLevel, AllocationRule rule, List<AllocationGroup> groupList)
        {
            string errorGettingNAV = string.Empty;
            try
            {
                //Update account wise NAV from expnl in case of Leveling
                //get account nav, account wise market value for symbols and simulated market value for group from expnl server, PRANA-26333
                errorGettingNAV = LevelingHelper.GetValuesFromExpnl(allocationLevel, rule.ProrataAccountList, groupList, out _accountWiseNav, out _groupWiseMarketValue, out _symbolAccountWiseMarketValue);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errorGettingNAV;
        }

        /// <summary>
        /// Tries the match portfolio position.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="clonedParameter">The cloned parameter.</param>
        /// <param name="dateSortedData">The date sorted data.</param>
        /// <param name="allocationOutputForCurrentSymbol">The allocation output for current symbol.</param>
        /// <param name="stateNavErrorString">The state nav error string.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        /// <returns></returns>
        public override string TryMatchPortfolioPosition(string symbol, AllocationParameter clonedParameter, SortedDictionary<DateTime, List<AllocationGroup>> dateSortedData, ref SerializableDictionary<string, AllocationOutput> allocationOutputForCurrentSymbol, string stateNavErrorString, ref Dictionary<int, AccountValue> marketValueStateForSymbol, Dictionary<int, AccountValue> currentAllocationState)
        {
            string matchPositionError = string.Empty;
            try
            {
                matchPositionError = AllocationProcessHelper.TryMatchPortfolioPosition(symbol, clonedParameter, dateSortedData, out allocationOutputForCurrentSymbol, stateNavErrorString, ref marketValueStateForSymbol, currentAllocationState, _accountWiseNav, _groupWiseMarketValue);
                if (string.IsNullOrWhiteSpace(matchPositionError) && clonedParameter.CheckListWisePreference.RuleType == MatchingRuleType.Leveling)
                {
                    UpdateLevelingState(symbol, marketValueStateForSymbol);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return matchPositionError;
        }

        /// <summary>
        /// Gets the target percentage.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="clonedParameter">The cloned parameter.</param>
        /// <param name="dateSortedData">The date sorted data.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        /// <returns></returns>
        public override string GetTargetPercentage(string symbol, AllocationParameter clonedParameter, SortedDictionary<DateTime, List<AllocationGroup>> dateSortedData, ref Dictionary<int, AccountValue> marketValueStateForSymbol)
        {
            string stateNavErrorString = string.Empty;
            try
            {
                if (dateSortedData.SelectMany(x => x.Value).Any(y => _groupWiseMarketValue[y.GroupID] == 0.0))
                    stateNavErrorString = symbol + ": Market value is 0 for some selected groups of symbol";
                else
                {
                    lock (_symbolMarketValueLocker)
                    {
                        //get current account wise market value state for symbol
                        if (_symbolAccountWiseMarketValue != null && _symbolAccountWiseMarketValue.ContainsKey(symbol))
                            marketValueStateForSymbol = LevelingHelper.GetMarketValueStateForSymbol(_symbolAccountWiseMarketValue[symbol]);
                    }
                    stateNavErrorString = LevelingHelper.ValidatePreConditions(symbol, clonedParameter.CheckListWisePreference.ProrataAccountList, marketValueStateForSymbol);
                    clonedParameter.DoCheckSide = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return stateNavErrorString;
        }

        /// <summary>
        /// Updates the state of the leveling.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        public override void UpdateLevelingState(string symbol, Dictionary<int, AccountValue> marketValueStateForSymbol)
        {
            try
            {
                lock (_symbolMarketValueLocker)
                {
                    if (_symbolAccountWiseMarketValue.ContainsKey(symbol))
                        _symbolAccountWiseMarketValue[symbol] = LevelingHelper.GetMarketValueStateForSymbol(marketValueStateForSymbol);
                    else
                        _symbolAccountWiseMarketValue.Add(symbol, LevelingHelper.GetMarketValueStateForSymbol(marketValueStateForSymbol));
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
        /// Gets the percentage for leveling.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        /// <returns></returns>
        public override SerializableDictionary<int, AccountValue> GetPercentageForLeveling(AllocationGroup group, Dictionary<int, AccountValue> marketValueStateForSymbol)
        {
            SerializableDictionary<int, AccountValue> dict = new SerializableDictionary<int, AccountValue>();
            try
            {
                dict = LevelingHelper.GetPercentageForLeveling(_accountWiseNav, marketValueStateForSymbol, Convert.ToDecimal(_groupWiseMarketValue[group.GroupID]), _accountWiseNav.Keys.ToList());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dict;
        }

        /// <summary>
        /// Updates the account value state for symbol.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        /// <param name="result">The result.</param>
        public override void UpdateAccountValueStateForSymbol(AllocationGroup group, Dictionary<int, AccountValue> marketValueStateForSymbol, AllocationOutput result)
        {
            try
            {
                if (_groupWiseMarketValue != null && marketValueStateForSymbol != null)
                {
                    foreach (AccountValue accountVal in result.AccountValueCollection)
                    {
                        if (marketValueStateForSymbol.ContainsKey(accountVal.AccountId))
                            marketValueStateForSymbol[accountVal.AccountId].AddValue((accountVal.Value * Convert.ToDecimal(_groupWiseMarketValue[group.GroupID])) / Convert.ToDecimal(group.CumQty));
                        else
                            marketValueStateForSymbol.Add(accountVal.AccountId, new AccountValue(accountVal.AccountId, (accountVal.Value * Convert.ToDecimal(_groupWiseMarketValue[group.GroupID])) / Convert.ToDecimal(group.CumQty)));
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