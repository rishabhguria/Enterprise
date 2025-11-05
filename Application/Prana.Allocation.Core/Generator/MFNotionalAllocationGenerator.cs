// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : Disha Sharma
// Created          : 05-04-2017
// ***********************************************************************
// <copyright file="MFNotionalAllocationGenerator.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Enums;
using Prana.Allocation.Core.Allocator;
using Prana.Allocation.Core.Extensions;
using Prana.Allocation.Core.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Classes.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Prana.Allocation.Core.Generator
{
    internal class MFNotionalAllocationGenerator : MasterFundAllocator
    {
        /// <summary>
        /// The master fund wise start of day nav
        /// </summary>
        ImmutableSortedDictionary<int, decimal> _masterFundWiseNav;

        /// <summary>
        /// The group wise market value
        /// </summary>
        ImmutableSortedDictionary<string, double> _groupWiseMarketValue;

        /// <summary>
        /// The symbol account wise market value
        /// </summary>
        Dictionary<string, Dictionary<int, double>> _symbolMFWiseMarketValue;

        /// <summary>
        /// The symbol market value locker
        /// </summary>
        private readonly object _symbolMarketValueLocker = new object();

        /// <summary>
        /// Updates the allocation base type to Notional.
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
        /// Updates the master fund wise current nav which will be used for allocation Leveling methodology
        /// </summary>
        /// <param name="prorataList">The prorata list.</param>
        /// <returns>Error Message</returns>
        public override string UpdateMasterFundWiseNAV(List<int> prorataList, List<AllocationGroup> groups)
        {
            string errorMessage = string.Empty;
            try
            {
                //get masterfund nav, group and master fund wise market value state and market value of selected groups, PRANA-26601
                errorMessage = LevelingHelper.GetValuesFromExpnl(AllocationLevel.MasterFund, prorataList, groups, out _masterFundWiseNav, out _groupWiseMarketValue, out _symbolMFWiseMarketValue);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }

        /// <summary>
        /// Gets the master fund percentage for allocation rule.
        /// </summary>
        /// <param name="masterFundPref">The master fund preference.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="groupIds">The group ids.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public override SerializableDictionary<int, AccountValue> GetMasterFundPercentageForAllocationRule(AllocationMasterFundPreference masterFundPref, string symbol, int userId, List<string> groupIds, out string errorMessage)
        {
            SerializableDictionary<int, AccountValue> mfTargetPercentage = new SerializableDictionary<int, AccountValue>();
            errorMessage = string.Empty;
            try
            {
                switch (masterFundPref.DefaultRule.RuleType)
                {
                    case MatchingRuleType.None:
                        if (masterFundPref.MasterFundTargetPercentage != null && masterFundPref.MasterFundTargetPercentage.Count > 0)
                            mfTargetPercentage = masterFundPref.MasterFundTargetPercentage.ToSerializableDictionary(t => t.Key, t => new AccountValue(t.Key, t.Value));
                        else
                            errorMessage = "Master fund target percentage is not defined.";
                        break;

                    case MatchingRuleType.Leveling:
                        //get target market value of groups being allocated, this is used for leveling process, PRANA-26601
                        decimal targetMarketValue = Convert.ToDecimal(_groupWiseMarketValue.Where(x => groupIds.Contains(x.Key)).Sum(y => y.Value));

                        if (targetMarketValue == 0.0M)
                            errorMessage = symbol + ": Market value is 0 for some selected groups of symbol.";

                        if (string.IsNullOrWhiteSpace(errorMessage))
                        {
                            int keyToFind = this.GetKeyToFindForGettingState(masterFundPref);
                            Dictionary<int, AccountValue> currentMarketValueStateForSymbol = null;
                            lock (_symbolMarketValueLocker)
                            {
                                currentMarketValueStateForSymbol = LevelingHelper.GetMarketValueStateForSymbol(_symbolMFWiseMarketValue[symbol]);
                            }
                            errorMessage = LevelingHelper.ValidatePreConditions(symbol, masterFundPref.DefaultRule.ProrataAccountList, currentMarketValueStateForSymbol);

                            //get target percentage on basis of current market value state and target market value of groups being allocated, PRANA-26601
                            if (string.IsNullOrWhiteSpace(errorMessage))
                                mfTargetPercentage = LevelingHelper.GetPercentageForLeveling(_masterFundWiseNav, currentMarketValueStateForSymbol, targetMarketValue, _masterFundWiseNav.Keys.ToList());

                            //update market value state for symbol assuming trades are allocated successfully, PRANA-26601
                            lock (_symbolMarketValueLocker)
                            {
                                foreach (AccountValue accountVal in mfTargetPercentage.Values)
                                {
                                    if (_symbolMFWiseMarketValue[symbol].ContainsKey(accountVal.AccountId))
                                        _symbolMFWiseMarketValue[symbol][accountVal.AccountId] += Convert.ToDouble(accountVal.Value * 0.01M * targetMarketValue);
                                    else
                                        _symbolMFWiseMarketValue[symbol].Add(accountVal.AccountId, Convert.ToDouble(accountVal.Value * 0.01M * targetMarketValue));
                                }

                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return mfTargetPercentage;
        }
    }
}
