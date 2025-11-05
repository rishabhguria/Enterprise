// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-10-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-10-2014
// ***********************************************************************
// <copyright file="AllocationGroupPredicates.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Core.Extensions;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using Prana.Utilities.PredicateHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// The Comparers namespace.
/// </summary>
namespace Prana.Allocation.Core.Comparers
{
    /// <summary>
    /// Class AllocationGroupPredicates.
    /// </summary>
    internal static class AllocationGroupPredicates
    {
        /// <summary>
        /// Returns the predicate for which data is required
        /// </summary>
        /// <param name="toDate">Date up to which filter will be applied</param>
        /// <param name="fromDate">Date from which filter will be applied</param>
        /// <param name="filterList">List of filters on other properties</param>
        /// <returns>Predicate which will be used to filter the list</returns>
        internal static Expression<Func<AllocationGroup, bool>> GetPredicateFromFilter(DateTime toDate, DateTime fromDate, AllocationPrefetchFilter filterList, int userId)
        {

            try
            {
                Dictionary<String, String> filterAllocated = new Dictionary<string, string>();
                Dictionary<String, String> filterUnAllocated = new Dictionary<string, string>();

                if (filterList != null)
                {
                    if (filterList.Allocated.Count > 0)
                        filterAllocated = filterList.Allocated;
                    if (filterList.Unallocated.Count > 0)
                        filterUnAllocated = filterList.Unallocated;
                }


                Expression<Func<AllocationGroup, bool>> predicateAllocated = un => un.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED && un.AllocationDate >= DateTime.Parse(filterAllocated["FromDate"]) && un.AllocationDate <= toDate;
                predicateAllocated = GetAppliedFiltersPredicate(filterAllocated, predicateAllocated);

                TradingAccountCollection tradingAccounts = Prana.CommonDataCache.WindsorContainerManager.GetTradingAccounts(userId);
                var accountIdList = new List<int>();
                foreach (TradingAccount acc in tradingAccounts)
                {
                    if (!accountIdList.Contains(acc.TradingAccountID))
                        accountIdList.Add(acc.TradingAccountID);
                }

                Expression<Func<AllocationGroup, bool>> predicateUnallocated = un => un.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED
                    && un.AUECLocalDate >= fromDate && un.AUECLocalDate <= toDate && (accountIdList.Contains(un.TradingAccountID) || un.TradingAccountID == 0 || un.TradingAccountID == int.MinValue);
                predicateUnallocated = GetAppliedFiltersPredicate(filterUnAllocated, predicateUnallocated);

                Expression<Func<AllocationGroup, bool>> final = predicateAllocated.Or(predicateUnallocated);
                return final.And(un => un.CumQty > 0);

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
        /// Gets the applied filters predicate.
        /// </summary>
        /// <param name="appliedFilters">The applied filters.</param>
        /// <param name="filterPredicate">The filter predicate.</param>
        /// <returns></returns>
        private static Expression<Func<AllocationGroup, bool>> GetAppliedFiltersPredicate(Dictionary<String, String> appliedFilters, Expression<Func<AllocationGroup, bool>> filterPredicate)
        {
            try
            {
                foreach (string key in appliedFilters.Keys)
                {
                    switch (key)
                    {
                        case "Symbol":
                            UpdatePredicateForSymbol(ref filterPredicate, appliedFilters[key]);
                            break;
                        case "IsPreAllocated":
                            UpdatePredicateForPreAllocated(ref filterPredicate, Convert.ToBoolean(appliedFilters[key]));
                            break;
                        case "IsManualGroup":
                            UpdatePredicateForManualGroup(ref filterPredicate, Convert.ToBoolean(appliedFilters[key]));
                            break;
                        case "OrderSideTagValue":
                            UpdatePredicateForOrderSideTagValue(ref filterPredicate, appliedFilters[key]);
                            break;
                        case "BrokerID":
                            UpdatePredicateForCounterPartyID(ref filterPredicate, appliedFilters[key]);
                            break;
                        case "TradingAccountID":
                            UpdatePredicateForTradingAccountID(ref filterPredicate, appliedFilters[key]);
                            break;
                        case "VenueID":
                            UpdatePredicateForVenueID(ref filterPredicate, appliedFilters[key]);
                            break;
                        case "CurrencyID":
                            UpdatePredicateForCurrencyID(ref filterPredicate, appliedFilters[key]);
                            break;
                        case "ExchangeID":
                            UpdatePredicateForExchangeID(ref filterPredicate, appliedFilters[key]);
                            break;
                        case "AssetID":
                            UpdatePredicateForAssetID(ref filterPredicate, appliedFilters[key]);
                            break;
                        case "UnderlyingID":
                            UpdatePredicateForUnderlyingID(ref filterPredicate, appliedFilters[key]);
                            break;
                        case "AccountID":
                            UpdatePredicateForAccountId(ref filterPredicate, appliedFilters[key]);
                            break;
                        case "StrategyID":
                            UpdatePredicateForStrategyId(ref filterPredicate, appliedFilters[key]);
                            break;
                        case "GroupID":
                            UpdatePredicateForGroupID(ref filterPredicate, appliedFilters[key]);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return filterPredicate;
        }

        /// <summary>
        /// Updates the predicate for strategy identifier.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForStrategyId(ref Expression<Func<AllocationGroup, bool>> predicate, string value)
        {
            try
            {
                string[] strategyArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Expression<Func<AllocationGroup, bool>> predicateLoop = null;
                foreach (string strategyId in strategyArray)
                {
                    int strategy = Convert.ToInt32(strategyId);
                    Expression<Func<AllocationGroup, bool>> predicateTemp = un => un.ContainsStrategy(strategy);
                    if (predicateLoop == null)
                        predicateLoop = predicateTemp;
                    else
                        predicateLoop = predicateLoop.Or(predicateTemp);
                }

                if (predicateLoop != null)
                    predicate = predicate.And(predicateLoop);
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
        /// Updates the predicate for account identifier.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForAccountId(ref Expression<Func<AllocationGroup, bool>> predicate, string value)
        {
            try
            {
                string[] accountArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Expression<Func<AllocationGroup, bool>> predicateLoop = null;
                foreach (string accountId in accountArray)
                {
                    int account = Convert.ToInt32(accountId);
                    Expression<Func<AllocationGroup, bool>> predicateTemp = un => un.ContainsAccount(account);
                    if (predicateLoop == null)
                        predicateLoop = predicateTemp;
                    else
                        predicateLoop = predicateLoop.Or(predicateTemp);
                }

                if (predicateLoop != null)
                    predicate = predicate.And(predicateLoop);
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
        /// Updates the predicate for underlying identifier.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForUnderlyingID(ref Expression<Func<AllocationGroup, bool>> predicate, string value)
        {
            try
            {
                string[] symbolArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Expression<Func<AllocationGroup, bool>> predicateLoop = null;
                foreach (string symbol in symbolArray)
                {
                    Expression<Func<AllocationGroup, bool>> predicateTemp = un => un.UnderlyingID == Convert.ToInt32(symbol);
                    if (predicateLoop == null)
                        predicateLoop = predicateTemp;
                    else
                        predicateLoop = predicateLoop.Or(predicateTemp);
                }

                if (predicateLoop != null)
                    predicate = predicate.And(predicateLoop);
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
        /// Updates the predicate for asset identifier.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForAssetID(ref Expression<Func<AllocationGroup, bool>> predicate, string value)
        {
            try
            {
                string[] symbolArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Expression<Func<AllocationGroup, bool>> predicateLoop = null;
                foreach (string symbol in symbolArray)
                {
                    int assetId = Convert.ToInt32(symbol);
                    Expression<Func<AllocationGroup, bool>> predicateTemp = un => un.AssetID == assetId;
                    if (predicateLoop == null)
                        predicateLoop = predicateTemp;
                    else
                        predicateLoop = predicateLoop.Or(predicateTemp);
                }

                if (predicateLoop != null)
                    predicate = predicate.And(predicateLoop);
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
        /// Updates the predicate for exchange identifier.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForExchangeID(ref Expression<Func<AllocationGroup, bool>> predicate, string value)
        {
            try
            {
                string[] symbolArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Expression<Func<AllocationGroup, bool>> predicateLoop = null;
                foreach (string symbol in symbolArray)
                {
                    Expression<Func<AllocationGroup, bool>> predicateTemp = un => un.ExchangeID == Convert.ToInt32(symbol);
                    if (predicateLoop == null)
                        predicateLoop = predicateTemp;
                    else
                        predicateLoop = predicateLoop.Or(predicateTemp);
                }
                if (predicateLoop != null)
                    predicate = predicate.And(predicateLoop);
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
        /// Updates the predicate for currency identifier.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForCurrencyID(ref Expression<Func<AllocationGroup, bool>> predicate, string value)
        {
            try
            {
                string[] symbolArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Expression<Func<AllocationGroup, bool>> predicateLoop = null;
                foreach (string symbol in symbolArray)
                {
                    Expression<Func<AllocationGroup, bool>> predicateTemp = un => un.CurrencyID == Convert.ToInt32(symbol);
                    if (predicateLoop == null)
                        predicateLoop = predicateTemp;
                    else
                        predicateLoop = predicateLoop.Or(predicateTemp);
                }
                if (predicateLoop != null)
                    predicate = predicate.And(predicateLoop);
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
        /// Updates the predicate for venue identifier.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForVenueID(ref Expression<Func<AllocationGroup, bool>> predicate, string value)
        {
            try
            {
                string[] symbolArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Expression<Func<AllocationGroup, bool>> predicateLoop = null;
                foreach (string symbol in symbolArray)
                {
                    Expression<Func<AllocationGroup, bool>> predicateTemp = un => un.VenueID == Convert.ToInt32(symbol);
                    if (predicateLoop == null)
                        predicateLoop = predicateTemp;
                    else
                        predicateLoop = predicateLoop.Or(predicateTemp);
                }

                if (predicateLoop != null)
                    predicate = predicate.And(predicateLoop);
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
        /// Updates the predicate for trading account identifier.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForTradingAccountID(ref Expression<Func<AllocationGroup, bool>> predicate, string value)
        {
            try
            {
                string[] symbolArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Expression<Func<AllocationGroup, bool>> predicateLoop = null;
                foreach (string symbol in symbolArray)
                {
                    Expression<Func<AllocationGroup, bool>> predicateTemp = un => un.TradingAccountID == Convert.ToInt32(symbol);
                    if (predicateLoop == null)
                        predicateLoop = predicateTemp;
                    else
                        predicateLoop = predicateLoop.Or(predicateTemp);
                }

                if (predicateLoop != null)
                    predicate = predicate.And(predicateLoop);
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
        /// Updates the predicate for counter party identifier.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForCounterPartyID(ref Expression<Func<AllocationGroup, bool>> predicate, string value)
        {
            try
            {
                string[] symbolArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Expression<Func<AllocationGroup, bool>> predicateLoop = null;
                foreach (string symbol in symbolArray)
                {
                    Expression<Func<AllocationGroup, bool>> predicateTemp = un => un.CounterPartyID == Convert.ToInt32(symbol);
                    if (predicateLoop == null)
                        predicateLoop = predicateTemp;
                    else
                        predicateLoop = predicateLoop.Or(predicateTemp);
                }

                if (predicateLoop != null)
                    predicate = predicate.And(predicateLoop);
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
        /// Updates the predicate for order side tag value.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForOrderSideTagValue(ref Expression<Func<AllocationGroup, bool>> predicate, string value)
        {
            try
            {
                string[] symbolArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Expression<Func<AllocationGroup, bool>> predicateLoop = null;
                foreach (string symbol in symbolArray)
                {
                    Expression<Func<AllocationGroup, bool>> predicateTemp = un => un.OrderSideTagValue.ToUpper().Equals(symbol.ToUpper());
                    if (predicateLoop == null)
                        predicateLoop = predicateTemp;
                    else
                        predicateLoop = predicateLoop.Or(predicateTemp);
                }
                if (predicateLoop != null)
                    predicate = predicate.And(predicateLoop);
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
        /// Updates the predicate for manual group.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForManualGroup(ref Expression<Func<AllocationGroup, bool>> predicate, bool value)
        {
            try
            {
                predicate = predicate.And(un => un.IsManualGroup == value);
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
        /// Updates the predicate for pre allocated.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForPreAllocated(ref Expression<Func<AllocationGroup, bool>> predicate, bool value)
        {
            try
            {
                predicate = predicate.And(un => un.IsPreAllocated == value);
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
        /// Updates the predicate for symbol.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForSymbol(ref Expression<Func<AllocationGroup, bool>> predicate, string value)
        {
            try
            {
                string[] symbolArray = Array.ConvertAll(value.Split(','), p => p.Trim());
                symbolArray = symbolArray.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                Expression<Func<AllocationGroup, bool>> predicateLoop = null;
                foreach (string symbol in symbolArray)
                {
                    Expression<Func<AllocationGroup, bool>> predicateTemp = un => un.Symbol.ToUpper().Contains(symbol.ToUpper());
                    if (predicateLoop == null)
                        predicateLoop = predicateTemp;
                    else
                        predicateLoop = predicateLoop.Or(predicateTemp);
                }

                if (predicateLoop != null)
                    predicate = predicate.And(predicateLoop);
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
        /// Updates the predicate for Group identifier.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        private static void UpdatePredicateForGroupID(ref Expression<Func<AllocationGroup, bool>> predicate, string value)
        {
            try
            {
                string[] groupIdArray = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Expression<Func<AllocationGroup, bool>> predicateLoop = null;
                foreach (string groupId in groupIdArray)
                {
                    Expression<Func<AllocationGroup, bool>> predicateTemp = un => un.GroupID.Equals(groupId);
                    if (predicateLoop == null)
                        predicateLoop = predicateTemp;
                    else
                        predicateLoop = predicateLoop.Or(predicateTemp);
                }

                if (predicateLoop != null)
                    predicate = predicate.And(predicateLoop);
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
