// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : Disha Sharma
// Created          : 11-11-2016
//
// ***********************************************************************
// <copyright file="LevelingHelper.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Enums;
using Prana.Allocation.Core.CacheStore;
using Prana.Allocation.Core.Extensions;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.ServiceConnector;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Prana.Allocation.Core.Helper
{
    internal static class LevelingHelper
    {
        /// <summary>
        /// Calculate percenatge for selected accounts.
        /// </summary>
        /// <param name="marketValueState">State of symbol</param>
        /// <param name="levelingAccountList">List of accounts</param>
        /// <returns>percentage</returns>
        internal static SerializableDictionary<int, AccountValue> GetPercentageForLeveling(ImmutableSortedDictionary<int, decimal> accountWiseNav, Dictionary<int, AccountValue> marketValueState, decimal targetMarketValue, List<int> levelingAccountList)
        {
            SerializableDictionary<int, AccountValue> percentage = new SerializableDictionary<int, AccountValue>();
            try
            {
                /* Algo Steps for calculating leveling percentage
                 * 1. Calculate symbol contribution in each account
                 * 2. Get target percentage, if target notional is long then get minimum percentage for symbol contributuion else get maximum percentage
                 * 3. Now calculate notional distribution for each account to achieve the target percentage
                 * 4. If required notional distribution is greater than target notional then remove account with target percentage and calculate notional distribution for other accounts
                 * 5. Otherwise, calculate the remaining notional (target notional - notional distributed)
                 * 6. Divide remaining notional in all accounts based on their nav ratio
                 */
                SerializableDictionary<int, decimal> symbolContributionToFund = new SerializableDictionary<int, decimal>();
                foreach (int id in levelingAccountList)
                {
                    if (marketValueState.ContainsKey(id) && accountWiseNav.ContainsKey(id))
                        symbolContributionToFund.Add(id, (accountWiseNav[id] == 0.0M) ? 0.0M : marketValueState[id].Value / accountWiseNav[id]);
                    else
                        symbolContributionToFund.Add(id, 0.0M);
                }
                decimal targetPercentage = (targetMarketValue < 0) ? symbolContributionToFund.Values.Min() : symbolContributionToFund.Values.Max();

                Dictionary<int, AccountValue> marketValueDistribution = new Dictionary<int, AccountValue>();
                foreach (int id in levelingAccountList)
                {
                    AccountValue val = new AccountValue(id, Math.Abs(targetPercentage - symbolContributionToFund[id]) * accountWiseNav[id]);
                    marketValueDistribution.Add(id, val);
                }

                if (Math.Abs(targetMarketValue) < Math.Abs(marketValueDistribution.Values.Sum(x => x.Value)))
                {
                    levelingAccountList.Remove(symbolContributionToFund.FirstOrDefault(x => x.Value == targetPercentage).Key);
                    percentage = GetPercentageForLeveling(accountWiseNav, marketValueState, targetMarketValue, levelingAccountList);
                }
                else
                {
                    decimal remainingNotional = Math.Abs(targetMarketValue) - marketValueDistribution.Values.Sum(x => x.Value);
                    decimal totalNAV = accountWiseNav.Where(x => levelingAccountList.Contains(x.Key)).Sum(y => y.Value);
                    levelingAccountList.ForEach(id => { marketValueDistribution[id].AddValue(remainingNotional * accountWiseNav[id] / totalNAV); });
                    percentage = GetPercentageDistribution(marketValueDistribution);
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
            return percentage;
        }

        /// <summary>
        /// Gets the percentage distribution.
        /// </summary>
        /// <param name="marketValueDistribution">The notional distribution.</param>
        /// <returns></returns>
        private static SerializableDictionary<int, AccountValue> GetPercentageDistribution(Dictionary<int, AccountValue> marketValueDistribution)
        {
            SerializableDictionary<int, AccountValue> percentage = new SerializableDictionary<int, AccountValue>();
            try
            {
                decimal totalvalue = marketValueDistribution.Select(s => Math.Abs(s.Value.Value)).Sum();
                foreach (int id in marketValueDistribution.Keys)
                {
                    decimal per = (Math.Abs(marketValueDistribution[id].Value) * 100) / totalvalue;
                    percentage.Add(id, new AccountValue(id, per));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return percentage;
        }

        /// <summary>
        /// Validates the pre conditions.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="accountList">The account list.</param>
        /// <param name="currentStateForSymbol">The current state for symbol.</param>
        /// <returns></returns>
        internal static string ValidatePreConditions(string symbol, List<int> accountList, Dictionary<int, AccountValue> currentStateForSymbol)
        {
            string errorMessage = string.Empty;
            try
            {
                if (IsMarketValueWithLongShort(accountList, currentStateForSymbol))
                    errorMessage = symbol + ": Leveling allocation requires that symbols may not be both long and short.";
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
        /// Determines whether [is notional with long short] [the specified account list].
        /// </summary>
        /// <param name="accountList">The account list.</param>
        /// <param name="currentStateForSymbol">The current state for symbol.</param>
        /// <returns>
        ///   <c>true</c> if [is notional with long short] [the specified account list]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsMarketValueWithLongShort(List<int> accountList, Dictionary<int, AccountValue> currentStateForSymbol)
        {
            try
            {
                return !currentStateForSymbol.Values.Where(y => accountList.Contains(y.AccountId)).All(x => x.Value <= 0.0M)
                            && !currentStateForSymbol.Values.Where(y => accountList.Contains(y.AccountId)).All(x => x.Value >= 0.0M)
                            && !currentStateForSymbol.Values.Where(y => accountList.Contains(y.AccountId)).All(x => x.Value == 0.0M);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return false;
            }
        }

        /// <summary>
        /// Gets the values from expnl.
        /// </summary>
        /// <param name="fundList">The fund list.</param>
        /// <param name="groupList">The group list.</param>
        /// <param name="accountWiseNav">The account wise nav.</param>
        /// <param name="groupWiseMarketValue">The group wise market value.</param>
        /// <param name="symbolWiseMarketValueState">The symbol account wise market value.</param>
        /// <returns></returns>
        internal static string GetValuesFromExpnl(AllocationLevel allocLevel, List<int> fundList, List<AllocationGroup> groupList, out ImmutableSortedDictionary<int, decimal> accountWiseNav, out ImmutableSortedDictionary<string, double> groupWiseMarketValue, out Dictionary<string, Dictionary<int, double>> symbolWiseMarketValueState)
        {
            StringBuilder errorMessage = new StringBuilder();
            var marketValueBuilder = ImmutableSortedDictionary.CreateBuilder<string, double>();
            var navBuilder = ImmutableSortedDictionary.CreateBuilder<int, decimal>();
            symbolWiseMarketValueState = new Dictionary<string, Dictionary<int, double>>();
            try
            {
                if (!ExpnlServiceConnector.GetInstance().IsExpnlServiceConnected)
                {
                    errorMessage.Append("Calculation Service disconnected, so leveling cannot be done");
                    groupWiseMarketValue = marketValueBuilder.ToImmutable();
                    accountWiseNav = navBuilder.ToImmutable();
                    return errorMessage.ToString();
                }

                Dictionary<int, decimal> currentNavDictionary = new Dictionary<int, decimal>();
                Dictionary<string, double> groupMarketValue = new Dictionary<string, double>();

                //create unallocated taxlots for groups to get simulated market value, market value is being calculated for taxlots only, PRANA-26540
                List<AllocationGroup> groups = DeepCopyHelper.Clone(groupList);
                groups.ForEach(x => x.CreateUnAllocatedTaxLot());
                switch (allocLevel)
                {
                    case AllocationLevel.Account:
                        errorMessage = ExpnlServiceConnector.GetInstance().GetValuesForLeveling(groups.SelectMany(x => x.TaxLots).ToList(), fundList, ref currentNavDictionary, ref groupMarketValue, ref symbolWiseMarketValueState);
                        break;

                    case AllocationLevel.MasterFund:
                        errorMessage = ExpnlServiceConnector.GetInstance().GetMFValuesForLeveling(groups.SelectMany(x => x.TaxLots).ToList(), fundList, ref currentNavDictionary, ref groupMarketValue, ref symbolWiseMarketValueState);
                        break;
                }

                //Update symbolWiseMarketValueState before allocation already allocated groups and incoming fills for existing groups 

                List<string> groupIdList = groupList.SelectMany(g => g.OriginalGroupIDs).ToList();
                Expression<Func<AllocationGroup, bool>> predicate = un => groupIdList.Contains(un.GroupID);
                List<AllocationGroup> oldAllocationGroups = AllocationGroupCache.Instance.GetGroups(predicate);
                if (oldAllocationGroups != null && oldAllocationGroups.Count > 0)
                {
                    foreach (AllocationGroup group in oldAllocationGroups)
                    {
                        foreach (TaxLot taxlot in group.TaxLots)
                        {
                            int fundID = int.MinValue;
                            switch (allocLevel)
                            {
                                case AllocationLevel.Account:
                                    fundID = taxlot.Level1ID;
                                    break;

                                case AllocationLevel.MasterFund:
                                    fundID = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(taxlot.Level1ID);
                                    break;
                            }
                            if (fundID > 0)
                            {
                                AllocationGroup grp = groupList.FirstOrDefault(g => g.OriginalGroupIDs.Contains(group.GroupID));
                                double value = -(groupMarketValue[grp.GroupID] * taxlot.TaxLotQty) / grp.CumQty;
                                if (symbolWiseMarketValueState[grp.Symbol].ContainsKey(fundID))
                                    symbolWiseMarketValueState[grp.Symbol][fundID] += value;
                                else
                                    symbolWiseMarketValueState[grp.Symbol].Add(fundID, value);
                            }
                        }
                    }
                }
                // if account nav is negative then set nav as 0
                foreach (int key in currentNavDictionary.Keys)
                {
                    if (currentNavDictionary[key] > 0.0M)
                        navBuilder.Add(key, currentNavDictionary[key]);
                }

                foreach (string key in groupMarketValue.Keys)
                {
                    marketValueBuilder.Add(key, groupMarketValue[key]);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            groupWiseMarketValue = marketValueBuilder.ToImmutable();
            accountWiseNav = navBuilder.ToImmutable();
            if (accountWiseNav.Count == 0)
                errorMessage.AppendLine("As all selected accounts have zero NAV, so leveling cannot be done");
            return errorMessage.ToString();
        }

        /// <summary>
        /// Gets the market value state for symbol.
        /// </summary>
        /// <param name="marketValues">The market values.</param>
        /// <returns></returns>
        internal static Dictionary<int, AccountValue> GetMarketValueStateForSymbol(Dictionary<int, double> marketValues)
        {
            Dictionary<int, AccountValue> accountMarketValue = new Dictionary<int, AccountValue>();
            try
            {
                foreach (int accountId in marketValues.Keys)
                {
                    accountMarketValue.Add(accountId, new AccountValue(accountId, Convert.ToDecimal(marketValues[accountId])));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accountMarketValue;
        }

        /// <summary>
        /// Gets the market value state for symbol.
        /// </summary>
        /// <param name="marketValues">The market values.</param>
        /// <returns></returns>
        internal static Dictionary<int, double> GetMarketValueStateForSymbol(Dictionary<int, AccountValue> marketValues)
        {
            Dictionary<int, double> accountMarketValue = new Dictionary<int, double>();
            try
            {
                foreach (int accountId in marketValues.Keys)
                {
                    accountMarketValue.Add(accountId, Convert.ToDouble(marketValues[accountId].Value));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accountMarketValue;
        }
    }
}

