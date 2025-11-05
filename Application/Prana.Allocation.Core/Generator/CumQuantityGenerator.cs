
// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-09-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-09-2014
// ***********************************************************************
// <copyright file="CumQuantityGenerator.cs" company="Nirvana">
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


/// <summary>
/// The Generator namespace.
/// </summary>
namespace Prana.Allocation.Core.Generator
{
    /// <summary>
    /// IAllocationGeneratorInstance for cumQuantityAllocation
    /// </summary>
    public class CumQuantityGenerator : AccountAllocator
    {
        /// <summary>
        /// The _account wise start of day nav used in Pro rata by NAV process
        /// </summary>
        ImmutableSortedDictionary<int, decimal> _accountWiseStartOfDayNav;

        /// <summary>
        /// Updates the type of the base.
        /// </summary>
        public override void UpdateBaseType()
        {
            try
            {
                BaseType = AllocationBaseType.CumQuantity;
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
                //Update account wise start of Day NAV from expnl in case of Pro rata By NAV
                errorGettingNAV = ProrataByNAVHelper.GetStartOfDayNAV(allocationLevel, rule.ProrataAccountList, out _accountWiseStartOfDayNav);
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
                matchPositionError = AllocationProcessHelper.TryMatchPortfolioPosition(symbol, clonedParameter, dateSortedData, out allocationOutputForCurrentSymbol, stateNavErrorString, ref marketValueStateForSymbol, currentAllocationState);
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
        /// <param name="sortedDictionary">The sorted dictionary.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        /// <returns></returns>
        public override string GetTargetPercentage(string symbol, AllocationParameter clonedParameter, SortedDictionary<DateTime, List<AllocationGroup>> sortedDictionary, ref Dictionary<int, AccountValue> marketValueStateForSymbol)
        {
            string stateNavErrorString = string.Empty;
            try
            {
                SerializableDictionary<int, AccountValue> percentage1 = null;
                stateNavErrorString = ProrataByNAVHelper.GetPercentageForProrataByNAV(clonedParameter.CheckListWisePreference.ProrataAccountList, _accountWiseStartOfDayNav, out percentage1);
                if (string.IsNullOrWhiteSpace(stateNavErrorString))
                {
                    clonedParameter.UpdatePercentage(percentage1);
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
        }

        /// <summary>
        /// Gets the percentage for leveling.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        /// <returns></returns>
        public override SerializableDictionary<int, AccountValue> GetPercentageForLeveling(AllocationGroup group, Dictionary<int, AccountValue> marketValueStateForSymbol)
        {
            return new SerializableDictionary<int, AccountValue>();
        }

        /// <summary>
        /// Updates the account value state for symbol.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="marketValueStateForSymbol">The market value state for symbol.</param>
        /// <param name="result">The result.</param>
        public override void UpdateAccountValueStateForSymbol(AllocationGroup group, Dictionary<int, AccountValue> marketValueStateForSymbol, AllocationOutput result)
        {
        }
    }
}
