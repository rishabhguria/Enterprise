using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Core.CacheStore;
using Prana.Allocation.Core.Comparers;
using Prana.Allocation.Core.Enums;
using Prana.Allocation.Core.Factories;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Constants;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Prana.Allocation.Core.Helper
{
    /// <summary>
    /// 
    /// </summary>
    internal static class AllocationProcessHelper
    {

        /// <summary>
        /// Is Portfolio Position Perfect Close Possible
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="userId"></param>
        /// <param name="inputToBeVerified"></param>
        /// <returns></returns>
        internal static bool IsPortfolioPositionPerfectClosePossible(string symbol, int userId, List<AllocationGroup> inputToBeVerified, IList<int> accounts)
        {
            try
            {

                Dictionary<int, AccountValue> state = UserWiseStateCache.Instance.GetCurrentState(-1, AllocationBaseType.CumQuantity, symbol, userId);
                decimal totalQuantity = 0.0M;
                if (state != null)
                {
                    if (accounts != null && accounts.Count > 0)
                        state = state.Where(x => accounts.Contains(x.Key)).ToDictionary(val => val.Key, val => val.Value);
                    foreach (int accountId in state.Keys)
                    {
                        totalQuantity += state[accountId].Value;
                    }
                }


                foreach (AllocationGroup group in inputToBeVerified)
                {
                    bool isSellTrade = Calculations.GetSideMultilpier(group.OrderSideTagValue) == -1 ? true : false;
                    if (isSellTrade)
                        totalQuantity += (decimal)group.CumQty * decimal.MinusOne;
                    else
                        totalQuantity += (decimal)group.CumQty;
                }
                return totalQuantity == 0;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Is Portfolio Position Perfect Close Possible
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="userId"></param>
        /// <param name="inputToBeVerified"></param>
        /// <returns></returns>
        internal static bool GetAllocationState(string symbol, int userId, List<AllocationGroup> inputToBeVerified, IList<int> accounts, SerializableDictionary<int, AccountValue> targetPercentage, out Dictionary<int, AccountValue> state)
        {
            state = new Dictionary<int, AccountValue>();
            try
            {
                bool isClosable = false;

                //Checking Match closing transaction at Side level

                //Getting order side of transaction
                var orderSideTagValues = inputToBeVerified.Where(x => CheckSideHelper.GetPositionKey(x.OrderSideTagValue).Equals(GroupPositionType.LongClosing)
                    || CheckSideHelper.GetPositionKey(x.OrderSideTagValue).Equals(GroupPositionType.ShortClosing)).Select(x => x.OrderSideTagValue).Distinct().ToList();

                if (orderSideTagValues != null && orderSideTagValues.Count == 1)
                {
                    //Get Portfolio level allocation state
                    List<AllocationState> portfolioAllocationState = UserWiseStateCache.Instance.GetCurrentStateWithAccountStrategy(-1, AllocationBaseType.CumQuantity, symbol, userId);

                    if (portfolioAllocationState != null)
                    {
                        decimal totalQuantityInput = 0.0M;
                        foreach (AllocationGroup group in inputToBeVerified)
                        {
                            bool isSellTrade = Calculations.GetSideMultilpier(group.OrderSideTagValue) == -1 ? true : false;
                            if (isSellTrade)
                                totalQuantityInput += (decimal)group.CumQty * decimal.MinusOne;
                            else
                                totalQuantityInput += (decimal)group.CumQty;
                        }

                        // Get opening orderSideTagValue for closing trade (like for sell =>open, buytocover => sellshort)
                        string orderSideTagValue = CheckSideHelper.GetOpeningOrderSideTagValue(orderSideTagValues.FirstOrDefault());

                        //Checking Match closing transaction at portfolio-Side level
                        state = GetAllocationsStateFromCollection(AllocationBaseType.CumQuantity, orderSideTagValue, targetPercentage, portfolioAllocationState, true);
                        isClosable = CheckIsClosable(totalQuantityInput, accounts, state);
                        if (isClosable)
                            return isClosable;

                        //Checking Match closing transaction at Accounts-Side level ((accounts specified in allocation scheme))
                        state = GetAllocationsStateFromCollection(AllocationBaseType.CumQuantity, orderSideTagValue, targetPercentage, portfolioAllocationState, false);
                        isClosable = CheckIsClosable(totalQuantityInput, accounts, state);
                        if (isClosable)
                            return isClosable;


                        //validate strategy allocation preference for MCT
                        var isStrategyPrefValid = ValidateStrategyPref(targetPercentage);
                        if (isStrategyPrefValid)
                        {
                            #region Commented
                            ////Checking Match closing transaction at strategy level
                            //state = GetAllocationsStateFromCollection(AllocationBaseType.CumQuantity, string.Empty, targetPercentage, portfolioAllocationState, true);
                            //isClosable = CheckIsClosable(totalQuantityInput, accounts, state);
                            //if (isClosable)
                            //    return isClosable;

                            ////Checking Match closing transaction at strategy level
                            //state = GetAllocationsStateFromCollection(AllocationBaseType.CumQuantity, string.Empty, targetPercentage, portfolioAllocationState, false);
                            //isClosable = CheckIsClosable(totalQuantityInput, accounts, state);
                            //if (isClosable)
                            //    return isClosable; 
                            #endregion

                            //Checking Match closing transaction at portfolio-Side-Strategy level
                            state = GetAllocationsStateFromCollection(AllocationBaseType.CumQuantity, orderSideTagValue, targetPercentage, portfolioAllocationState, true, true);
                            isClosable = CheckIsClosable(totalQuantityInput, accounts, state);
                            if (isClosable)
                                return isClosable;

                            //Checking Match closing transaction at Accounts-Side-Strategy  level (accounts and strategies specified in allocation scheme)
                            state = GetAllocationsStateFromCollection(AllocationBaseType.CumQuantity, orderSideTagValue, targetPercentage, portfolioAllocationState, false, true);
                            isClosable = CheckIsClosable(totalQuantityInput, accounts, state);
                            if (isClosable)
                                return isClosable;

                        }
                    }
                }

                return false;


            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Get Allocations State From Collection
        /// </summary>
        /// <param name="orderSideTagValue"></param>
        /// <param name="strategyId"></param>
        /// <param name="portAllocationState"></param>
        /// <returns></returns>
        private static Dictionary<int, AccountValue> GetAllocationsStateFromCollection(AllocationBaseType baseType, string orderSideTagValue, SerializableDictionary<int, AccountValue> targetPercentage, List<AllocationState> portAllocationState, bool isPortfolioLevel, bool checkAtAccountSideStrategyLevel = false)
        {

            Dictionary<int, AccountValue> state = null;
            try
            {
                var filteredState = new List<AllocationState>();

                if (isPortfolioLevel)
                {
                    //Getting allocation state at portfolio level with filter of strategy and/or side
                    filteredState = GetAllocationStateAtPortfolioLevel(orderSideTagValue, targetPercentage, portAllocationState, checkAtAccountSideStrategyLevel);
                }
                else
                {
                    //Getting allocation state  with filter of account defined in allocation scheme and strategy and/or side
                    filteredState = GetAllocationStateAtAccountsLevel(orderSideTagValue, targetPercentage, portAllocationState, checkAtAccountSideStrategyLevel);
                }

                if (filteredState != null)
                {
                    switch (baseType)
                    {
                        case AllocationBaseType.CumQuantity:

                            state = filteredState.GroupBy(x => x.AccountId).ToDictionary(i => i.Key, x => new AccountValue(x.Key, x.Sum(y => y.cumQuantity)));
                            break;
                        case AllocationBaseType.Notional:
                            state = filteredState.GroupBy(x => x.AccountId).ToDictionary(i => i.Key, x => new AccountValue(x.Key, x.Sum(y => y.Notional)));
                            break;
                    }
                }

                return state;
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
        /// Get Allocation State At Portfolio Level (for all accounts)
        /// </summary>
        /// <param name="orderSideTagValue"></param>
        /// <param name="portAllocationState"></param>
        /// <param name="checkAtAccountSideStrategyLevel"></param>
        /// <returns></returns>
        private static List<AllocationState> GetAllocationStateAtPortfolioLevel(string orderSideTagValue, SerializableDictionary<int, AccountValue> targetPercentage, List<AllocationState> portAllocationState, bool checkAtAccountSideStrategyLevel)
        {
            List<AllocationState> filteredState = null;
            try
            {
                List<int> strategyIds = targetPercentage.Values.SelectMany(x => x.StrategyValueList.Where(y => y.Value == 100).Select(y => y.StrategyId)).Distinct().ToList();

                if (checkAtAccountSideStrategyLevel && !string.IsNullOrWhiteSpace(orderSideTagValue) && strategyIds != null)
                {
                    filteredState = portAllocationState
                        .Where(x => strategyIds.Contains(x.Level2ID) && x.OrderSideTagValue.Equals(orderSideTagValue, StringComparison.OrdinalIgnoreCase)).Select(x => x).ToList();
                }
                else if (!string.IsNullOrWhiteSpace(orderSideTagValue))
                {
                    filteredState = portAllocationState
                         .Where(x => x.OrderSideTagValue.Equals(orderSideTagValue, StringComparison.OrdinalIgnoreCase)).Select(x => x).ToList();
                }
                else if (strategyIds != null)
                {
                    filteredState = portAllocationState
                        .Where(x => strategyIds.Contains(x.Level2ID)).Select(x => x).ToList();
                }

                return filteredState;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get Allocation State At allocation scheme Level
        /// </summary>
        /// <param name="orderSideTagValue"></param>
        /// <param name="targetPercentage"></param>
        /// <param name="portAllocationState"></param>
        /// <param name="checkAtAccountSideStrategyLevel"></param>
        /// <param name="filteredState"></param>
        /// <returns></returns>
        private static List<AllocationState> GetAllocationStateAtAccountsLevel(string orderSideTagValue, SerializableDictionary<int, AccountValue> targetPercentage, List<AllocationState> portAllocationState, bool checkAtAccountSideStrategyLevel)
        {
            List<AllocationState> filteredState = null;
            try
            {
                //Getting allocation state at Account-Strategy level, only if 100% allocation in same strategy in account
                var accountStrategyDetails = targetPercentage.Values.ToDictionary(y => y.AccountId, x => x.StrategyValueList.Where(y => y.Value == 100).Select(y => y.StrategyId).FirstOrDefault());

                if (checkAtAccountSideStrategyLevel && !string.IsNullOrWhiteSpace(orderSideTagValue) && accountStrategyDetails != null)
                {
                    filteredState = portAllocationState.Join(accountStrategyDetails, x => x.AccountId, y => y.Key, (x, y) => new { X = x, Y = y })
                        .Where(x => x.X.Level2ID == x.Y.Value && x.X.OrderSideTagValue.Equals(orderSideTagValue, StringComparison.OrdinalIgnoreCase)).Select(x => x.X).ToList();

                }
                else if (!string.IsNullOrWhiteSpace(orderSideTagValue))
                {
                    filteredState = portAllocationState.Join(targetPercentage, x => x.AccountId, y => y.Key, (x, y) => new { X = x, Y = y })
                        .Where(x => x.X.OrderSideTagValue.Equals(orderSideTagValue, StringComparison.OrdinalIgnoreCase)).Select(x => x.X).ToList();
                }
                else if (accountStrategyDetails != null)
                {
                    filteredState = portAllocationState.Join(accountStrategyDetails, x => x.AccountId, y => y.Key, (x, y) => new { X = x, Y = y }).Where(x => x.X.Level2ID == x.Y.Value).Select(x => x.X).ToList();
                }

                return filteredState;
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Check Is Closable
        /// </summary>
        /// <param name="totalQuantityInput"></param>
        /// <param name="accounts"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private static bool CheckIsClosable(decimal totalQuantityInput, IList<int> accounts, Dictionary<int, AccountValue> state)
        {
            decimal totalQuantity = 0.0M;
            if (state != null)
            {
                if (accounts != null && accounts.Count > 0)
                    state = state.Where(x => accounts.Contains(x.Key)).ToDictionary(val => val.Key, val => val.Value);
                foreach (int accountId in state.Keys)
                {
                    totalQuantity += state[accountId].Value;
                }
            }
            return (totalQuantityInput + totalQuantity) == 0;

        }


        /// <summary>
        /// Validate Strategy allocation scheme Pref
        /// </summary>
        /// <param name="targetPercentage"></param>
        /// <returns></returns>
        internal static bool ValidateStrategyPref(SerializableDictionary<int, AccountValue> targetPercentage)
        {
            try
            {
                //not handled used case => If any scheme has preference of multiple strategy allocation in same account like F1 - S1:60% S2: 40%, then return
                if (targetPercentage.Values.Any(x => x.StrategyValueList.Where(y => y.Value < 100).Count() > 0))
                    return false;
                else return true;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }



        /// <summary>
        /// Allocates for making portfolio position to 0
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="parameter"></param>
        /// <param name="sortedDictionary"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        internal static string TryMatchPortfolioPosition(string symbol, AllocationParameter parameter, SortedDictionary<DateTime, List<AllocationGroup>> sortedDictionary, out SerializableDictionary<string, AllocationOutput> output, string stateNavErrorString, ref Dictionary<int, AccountValue> marketValueStateForSymbol, Dictionary<int, AccountValue> state, ImmutableSortedDictionary<int, decimal> accountWiseNav = null, ImmutableSortedDictionary<string, double> groupWiseMarketValue = null)
        {
            output = null;
            try
            {

                // TODO: Code and code and complete code
                output = new SerializableDictionary<string, AllocationOutput>();
                List<AccountValue> sortedState = new List<AccountValue>();


                foreach (DateTime dt in sortedDictionary.Keys)
                {
                    List<AllocationGroup> list = sortedDictionary[dt];
                    list.Sort(ComparerFactory.Instance.GetComparerFor(AllocationBaseType.CumQuantity, false));
                    //list.Reverse();

                    if (state != null)
                    {
                        IList<int> accounts = parameter.MatchClosingTransactionAccounts;
                        if (accounts != null && accounts.Count > 0)
                            state = state.Where(x => accounts.Contains(x.Key)).ToDictionary(val => val.Key, val => val.Value);
                        sortedState = state.Values.ToList();
                    }



                    List<AllocationGroup> holdState = new List<AllocationGroup>();
                    //List<AllocationGroup> partialAllocated = new List<AllocationGroup>();
                    foreach (AllocationGroup group in list)
                    {
                        int multiplier = Calculations.GetSideMultilpier(group.OrderSideTagValue);
                        AllocationOutput allocationOutput = new AllocationOutput(group.GroupID);

                        /*
                         * if state is null and group is of opening side then allocate with percentage.
                         * and continue
                         */
                        if ((state == null || state.Where(x => x.Value.Value != 0).Count() == 0))
                        {
                            if (
                                (group.OrderSideTagValue == FIXConstants.SIDE_Buy
                                || group.OrderSideTagValue == FIXConstants.SIDE_Buy_Open
                                || group.OrderSideTagValue == FIXConstants.SIDE_SellShort
                                || group.OrderSideTagValue == FIXConstants.SIDE_Sell_Open))
                            {
                                if (!(string.IsNullOrWhiteSpace(stateNavErrorString) && parameter.CheckListWisePreference.RuleType == MatchingRuleType.Leveling)
                                    && (parameter.TargetPercentage == null || parameter.TargetPercentage.Count <= 0))
                                {
                                    return stateNavErrorString;
                                }
                                else
                                {
                                    List<AllocationGroup> groupList = new List<AllocationGroup>();
                                    groupList.Add(group);
                                    GetAllocationOutput(groupList, parameter, ref state, ref output, accountWiseNav, groupWiseMarketValue, ref marketValueStateForSymbol);
                                    if (state != null)
                                        sortedState = state.Values.ToList();
                                    continue;
                                }
                            }
                            else
                            {
                                holdState.Add(group);
                                continue;
                            }
                        }


                        if (multiplier > 0)
                            sortedState = sortedState.OrderBy(i => i, new AccountValueComparer()).ToList();
                        else
                            sortedState = sortedState.OrderByDescending(i => i, new AccountValueComparer(false)).ToList();

                        // Keeping track of assigned quantity
                        decimal assignedQuantity = decimal.Zero;

                        foreach (AccountValue sortedVal in sortedState)
                        {
                            // Breaking loop if all assignment is done for this allocation group
                            if (assignedQuantity == (decimal)group.CumQty)
                                break;

                            // Continuing loop if following condition comes across
                            if ((sortedVal.Value >= 0 && multiplier > 0) || (sortedVal.Value <= 0 && multiplier < 0))
                                continue;
                            /*
                             * if state is 0 and group is of opening side then allocate with percentage.
                             * assign group.CumQty to assigned qty
                             * and continue
                             */
                            if (sortedVal.Value == 0 &&
                                (group.OrderSideTagValue == FIXConstants.SIDE_Buy
                                || group.OrderSideTagValue == FIXConstants.SIDE_Buy_Open
                                || group.OrderSideTagValue == FIXConstants.SIDE_SellShort
                                || group.OrderSideTagValue == FIXConstants.SIDE_Sell_Open))
                            {

                                if (!(string.IsNullOrWhiteSpace(stateNavErrorString) && parameter.CheckListWisePreference.RuleType == MatchingRuleType.Leveling)
                                    && (parameter.TargetPercentage == null || parameter.TargetPercentage.Count <= 0))
                                {
                                    return stateNavErrorString;
                                }
                                else
                                {
                                    List<AllocationGroup> groupList = new List<AllocationGroup>();
                                    groupList.Add(group);
                                    GetAllocationOutput(groupList, parameter, ref state, ref output, accountWiseNav, groupWiseMarketValue, ref marketValueStateForSymbol);
                                    assignedQuantity = (decimal)group.CumQty;
                                    continue;
                                }
                            }


                            AccountValue fvResult;

                            // If required
                            if (Math.Abs(sortedVal.Value) > (decimal)group.CumQty - assignedQuantity)
                                fvResult = new AccountValue(sortedVal.AccountId, (decimal)group.CumQty - assignedQuantity);
                            else
                                fvResult = new AccountValue(sortedVal.AccountId, Math.Abs(sortedVal.Value));

                            if (!parameter.DoCheckSide || CheckSideHelper.ValidateAllocationOutput(fvResult, group, sortedState.ToDictionary(x => x.AccountId, x => x)))
                            {
                                assignedQuantity += fvResult.Value;

                                allocationOutput.Add(fvResult);

                                // Updating state for next loop
                                sortedVal.AddValue(fvResult.Value * multiplier);
                                state[fvResult.AccountId] = sortedVal;

                                if (parameter.CheckListWisePreference.RuleType == MatchingRuleType.Leveling)
                                {
                                    if (marketValueStateForSymbol.ContainsKey(fvResult.AccountId))
                                        marketValueStateForSymbol[fvResult.AccountId].AddValue((fvResult.Value * Convert.ToDecimal(groupWiseMarketValue[group.GroupID])) / Convert.ToDecimal(group.CumQty));
                                    else
                                        marketValueStateForSymbol.Add(fvResult.AccountId, new AccountValue(fvResult.AccountId, (fvResult.Value * Convert.ToDecimal(groupWiseMarketValue[group.GroupID])) / Convert.ToDecimal(group.CumQty)));
                                }

                                if (!output.ContainsKey(group.GroupID))
                                    output.Add(allocationOutput.GroupId, DeepCopyHelper.Clone(allocationOutput));
                                else
                                {
                                    output[group.GroupID].Merge(fvResult);
                                }
                            }
                        }
                        if (assignedQuantity != (decimal)group.CumQty)
                        {
                            if (assignedQuantity == decimal.Zero)
                            {
                                holdState.Add(group);
                            }
                            else
                            {
                                if (assignedQuantity != (decimal)group.CumQty)
                                {
                                    AllocationGroup partialGroup = DeepCopyHelper.Clone(group);
                                    partialGroup.CumQty = group.CumQty - NumberPrecisionConstants.ToDoublePrecise(assignedQuantity);
                                    holdState.Add(partialGroup);
                                }

                            }
                        }

                    }


                    if (holdState.Count > 0)
                    {
                        if (!(string.IsNullOrWhiteSpace(stateNavErrorString) && parameter.CheckListWisePreference.RuleType == MatchingRuleType.Leveling)
                                    && (parameter.TargetPercentage == null || parameter.TargetPercentage.Count <= 0))
                            return stateNavErrorString;

                        if (!GetAllocationOutput(holdState, parameter, ref state, ref output, accountWiseNav, groupWiseMarketValue, ref marketValueStateForSymbol))
                            return symbol + ": Check Side failed for date: " + String.Format("{0:MM/dd/yyyy}", dt);
                    }

                }
                //decimal quantity = decimal.Zero;
                foreach (AccountValue account in sortedState)
                {
                    if (account.Value != 0)
                        return symbol + ": Match closing transaction allocation failed.";
                }
                return "";
                //if (quantity == decimal.Zero)
                //    return "";
                //else
                //    return "Check Side Failed for Symbol: " + symbol + "."; ;

            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return "Error occured while trying to match portfolio position";
            }
        }
        /// <summary>
        /// Get allocation output for hold and partial allocated.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parameter"></param>
        /// <param name="currentStateForSymbol"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static bool GetAllocationOutput(List<AllocationGroup> list, AllocationParameter parameter, ref Dictionary<int, AccountValue> currentStateForSymbol, ref SerializableDictionary<string, AllocationOutput> result, ImmutableSortedDictionary<int, decimal> accountWiseNav, ImmutableSortedDictionary<string, double> groupWiseMarketValue, ref Dictionary<int, AccountValue> marketValueStateForSymbol)
        {
            try
            {
                //hold state groups list for all groups for which check side fails.
                List<AllocationGroup> holdState = AllocateGroups(list, parameter, ref result, ref currentStateForSymbol, accountWiseNav, groupWiseMarketValue, ref marketValueStateForSymbol);

                // if hold state count is greater than 0 then try to allocate groups again.
                bool stopAllocate = holdState.Count <= 0 ? true : false;
                List<AllocationGroup> newHoldState = new List<AllocationGroup>();
                while (!stopAllocate)
                {
                    int startCount = holdState.Count;
                    newHoldState = AllocateGroups(holdState, parameter, ref result, ref currentStateForSymbol, accountWiseNav, groupWiseMarketValue, ref marketValueStateForSymbol);

                    int endCount = newHoldState.Count;
                    //if starting count and ending count are equal or end count is 0 then break else try again.
                    stopAllocate = endCount == 0 || startCount == endCount ? true : false;

                }
                //if hold state count is greater than depending on check side setting allocates.
                //if check side is enable then stop allocation else allocate forcefully.
                if (newHoldState.Count > 0)
                {
                    if (parameter.DoCheckSide)
                        return false;
                    else
                    {
                        newHoldState = AllocateGroups(holdState, parameter, ref result, ref currentStateForSymbol, accountWiseNav, groupWiseMarketValue, ref marketValueStateForSymbol, parameter.DoCheckSide);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }


        /// <summary>
        /// Allocates the groups.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="expnlData">The leveling items.</param>
        /// <param name="result">The result.</param>
        /// <param name="currentStateForSymbol">The current state for symbol.</param>
        private static List<AllocationGroup> AllocateGroups(List<AllocationGroup> groups, AllocationParameter parameter, ref SerializableDictionary<string, AllocationOutput> result, ref Dictionary<int, AccountValue> currentStateForSymbol, ImmutableSortedDictionary<int, decimal> accountWiseNav, ImmutableSortedDictionary<string, double> groupWiseMarketValue, ref Dictionary<int, AccountValue> marketValueStateForSymbol, bool checkside = true)
        {
            List<AllocationGroup> holdstate = new List<AllocationGroup>();
            try
            {
                foreach (AllocationGroup group in groups)
                {
                    if (parameter.CheckListWisePreference.RuleType == MatchingRuleType.Leveling)
                    {
                        parameter.UpdatePercentage(LevelingHelper.GetPercentageForLeveling(accountWiseNav, marketValueStateForSymbol, Convert.ToDecimal(groupWiseMarketValue[group.GroupID]), accountWiseNav.Keys.ToList()));
                    }
                    AllocationOutput outputForThisAllocationGroup = GetAllocationOutput(group, ref currentStateForSymbol, parameter, checkside);

                    //if check side fails for allocation group then add to hold state else add in result.
                    if (!outputForThisAllocationGroup.CheckSideViolated)
                    {
                        if (!result.ContainsKey(group.GroupID))
                            result.Add(group.GroupID, outputForThisAllocationGroup);
                        else
                        {
                            foreach (AccountValue fv in outputForThisAllocationGroup.AccountValueCollection)
                            {
                                result[group.GroupID].Merge(fv);
                            }
                        }
                        if (parameter.CheckListWisePreference.RuleType == MatchingRuleType.Leveling)
                        {
                            foreach (AccountValue accountVal in outputForThisAllocationGroup.AccountValueCollection)
                            {
                                if (marketValueStateForSymbol.ContainsKey(accountVal.AccountId))
                                    marketValueStateForSymbol[accountVal.AccountId].AddValue((accountVal.Value * Convert.ToDecimal(groupWiseMarketValue[group.GroupID])) / Convert.ToDecimal(group.CumQty));
                                else
                                    marketValueStateForSymbol.Add(accountVal.AccountId, new AccountValue(accountVal.AccountId, (accountVal.Value * Convert.ToDecimal(groupWiseMarketValue[group.GroupID])) / Convert.ToDecimal(group.CumQty)));
                            }
                        }
                    }
                    else
                        holdstate.Add(group);
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
            return holdstate;
        }

        /// <summary>
        /// Allocates group to account adjust remaining quantity.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="targetPercentage"></param>
        /// <param name="currentStateForSymbol"></param>
        /// <param name="doCheckSide"></param>
        /// <returns></returns>
        private static AllocationOutput GetAllocationOutput(AllocationGroup group, ref Dictionary<int, AccountValue> currentStateForSymbol, AllocationParameter parameters, bool checkside = true)
        {
            try
            {
                var targetPercentage = parameters.TargetPercentage;
                int preferencedAccountId = parameters.PreferenceId;

                int sideMultiplier = Calculations.GetSideMultilpier(group.OrderSideTagValue);
                // Deciding based on side multiplier whether it is buy trade or sell
                bool isSellTrade = sideMultiplier == -1 ? true : false;

                AllocationOutput result = new AllocationOutput(group.GroupID);
                // Based on it is sell trade getting value in descending or ascending
                List<AccountValue> toBeAssigend = GetNewAssignable((decimal)group.CumQty * sideMultiplier, targetPercentage, currentStateForSymbol, isSellTrade);

                if (currentStateForSymbol == null)
                    currentStateForSymbol = new Dictionary<int, AccountValue>();

                decimal assignedQuantity = 0.0M;
                foreach (AccountValue val in toBeAssigend)
                {
                    if ((isSellTrade && val.Value < 0.0M) || (!isSellTrade && val.Value > 0.0M))
                    {
                        // Getting new assignable quantity
                        decimal quantityInThisLoop = Math.Floor(Math.Abs(val.Value / group.RoundLot)) * group.RoundLot;
                        if (quantityInThisLoop > (decimal)group.CumQty - assignedQuantity)
                            quantityInThisLoop = (decimal)group.CumQty - assignedQuantity;

                        //skipping account-value object if quantityInThisLoop <= 0
                        if (quantityInThisLoop > 0)
                        {
                            //Adding to result
                            AccountValue fvCurrent = new AccountValue(val.AccountId, quantityInThisLoop);
                            result.Add(fvCurrent);

                            //Keeping track of assigned quantity
                            assignedQuantity += quantityInThisLoop;
                            if (!UpdateCurrentState(AllocationBaseType.CumQuantity, fvCurrent, group, ref currentStateForSymbol, sideMultiplier, quantityInThisLoop, parameters, checkside))
                            {
                                result.CheckSideViolated = true;
                                return result;
                            }
                        }
                    }

                }

                // Now logic for remaining quantity for allocation
                if (assignedQuantity < (decimal)group.CumQty)
                {
                    decimal remainingQuantityToBeAssigned = (decimal)group.CumQty - assignedQuantity;

                    // Assigning all remaining quantity to preference account in case if it is assigned and in target percentage cache
                    if (preferencedAccountId != -1 && targetPercentage.ContainsKey(preferencedAccountId))
                    {
                        AccountValue existing = result.GetAccountValueFor(preferencedAccountId);
                        if (existing == null)
                            existing = new AccountValue(preferencedAccountId, remainingQuantityToBeAssigned);
                        else
                            existing.AddValue(remainingQuantityToBeAssigned);

                        result.Add(existing);
                        //Evaluating check side on remaining quantity.
                        AccountValue fvRemaining = new AccountValue(existing.AccountId, remainingQuantityToBeAssigned);
                        result.CheckSideViolated = !UpdateCurrentState(AllocationBaseType.CumQuantity, fvRemaining, group, ref currentStateForSymbol, sideMultiplier, remainingQuantityToBeAssigned, parameters, checkside);
                        return result;


                    }
                    else
                    {
                        // If no preference account is set then will try to minimize the difference by allocating through quantity
                        decimal minimumAssignableQuantity = group.RoundLot;

                        //Getting maximum no of loops required to allocate all remaining quantity
                        int maxLoop = Convert.ToInt32(Math.Ceiling(remainingQuantityToBeAssigned / group.RoundLot));

                        for (int i = 0; i < maxLoop; i++)
                        {
                            List<AccountValue> toBeAssigendRemaining = GetNewAssignable(remainingQuantityToBeAssigned * sideMultiplier, targetPercentage, currentStateForSymbol, isSellTrade);
                            if (toBeAssigendRemaining != null && toBeAssigend.Count > 0)
                            {
                                AccountValue valRemaining = toBeAssigendRemaining[0];
                                decimal quantityToBeInThisLoop = minimumAssignableQuantity;

                                if (remainingQuantityToBeAssigned < quantityToBeInThisLoop)
                                    quantityToBeInThisLoop = remainingQuantityToBeAssigned;


                                //Adding to result
                                AccountValue fvCurrent = result.GetAccountValueFor(valRemaining.AccountId);
                                if (fvCurrent == null)
                                    fvCurrent = new AccountValue(valRemaining.AccountId, quantityToBeInThisLoop);
                                else
                                    fvCurrent.AddValue(quantityToBeInThisLoop);

                                result.Add(fvCurrent);

                                AccountValue fvRemaining = new AccountValue(valRemaining.AccountId, quantityToBeInThisLoop);
                                if (!UpdateCurrentState(AllocationBaseType.CumQuantity, fvRemaining, group, ref currentStateForSymbol, sideMultiplier, quantityToBeInThisLoop, parameters, checkside))
                                {
                                    result.CheckSideViolated = true;
                                    return result;
                                }
                                //Keeping track of assigned quantity
                                remainingQuantityToBeAssigned -= quantityToBeInThisLoop;

                            }
                        }
                    }
                }

                return result;
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
        /// Returns account value list for groups.
        /// </summary>
        /// <param name="valueToBeAssigend"></param>
        /// <param name="targetPercentage"></param>
        /// <param name="currentState"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        internal static List<AccountValue> GetNewAssignable(decimal valueToBeAssigend, Dictionary<int, AccountValue> targetPercentage, Dictionary<int, AccountValue> currentState, bool isAscending)
        {
            try
            {
                List<AccountValue> result = new List<AccountValue>();
                if (currentState != null && currentState.Count > 0 && currentState.Any(x => (targetPercentage.ContainsKey(x.Key) && x.Value.Value != 0m)))
                {
                    result = CalculateAccountInceptionAssignable(valueToBeAssigend, targetPercentage, currentState);
                }
                if (result.Count == 0)
                {
                    foreach (int fv in targetPercentage.Keys)
                    {
                        decimal totalShouldbeValue = (valueToBeAssigend * targetPercentage[fv].Value) / 100;
                        if (totalShouldbeValue != 0)
                        {
                            AccountValue fvData = new AccountValue(fv, totalShouldbeValue);
                            result.Add(fvData);
                        }
                    }
                }

                List<AccountValue> resultList = new List<AccountValue>();

                if (isAscending)
                    resultList = result.OrderBy(i => i, new AccountValueComparer()).ToList();
                else
                    resultList = result.OrderByDescending(i => i, new AccountValueComparer(false)).ToList();
                return resultList;
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
        /// Calculates the account inception assignable.
        /// </summary>
        /// <param name="valueToBeAssigend">The value to be assigend.</param>
        /// <param name="targetPercentage">The target percentage.</param>
        /// <param name="currentState">State of the current.</param>
        /// <returns></returns>
        private static List<AccountValue> CalculateAccountInceptionAssignable(decimal valueToBeAssigend, Dictionary<int, AccountValue> targetPercentage, Dictionary<int, AccountValue> currentState)
        {
            List<AccountValue> result = new List<AccountValue>();
            try
            {
                List<AccountPercent> intermediateQtyDict = new List<AccountPercent>();
                Dictionary<int, decimal> finalQtyDict = new Dictionary<int, decimal>();

                decimal originalTotalQty = 0;
                foreach (int accountID in targetPercentage.Keys)
                {
                    if (currentState.ContainsKey(accountID))
                    {
                        originalTotalQty += currentState[accountID].Value;
                    }
                }
                if (originalTotalQty != 0)
                {
                    decimal finalTotalQty = valueToBeAssigend + originalTotalQty;
                    decimal availableValue = finalTotalQty;

                    if (finalTotalQty != 0)
                    {
                        //Initially taking all accounts to original percent
                        foreach (int accountID in targetPercentage.Keys)
                        {
                            if (currentState.ContainsKey(accountID))
                            {
                                decimal originalPercent = (currentState[accountID].Value * 100m) / originalTotalQty;

                                //Directly go for the target percentage if it is closer than original percentage
                                if ((((valueToBeAssigend < 0) == (finalTotalQty < 0)) && targetPercentage[accountID].Value < originalPercent) ||
                                    (((valueToBeAssigend < 0) != (finalTotalQty < 0)) && targetPercentage[accountID].Value > originalPercent))
                                {
                                    originalPercent = targetPercentage[accountID].Value;
                                }
                                decimal totalShouldbeValue = (originalPercent * finalTotalQty) / 100m;
                                if ((valueToBeAssigend > 0 && totalShouldbeValue > currentState[accountID].Value) || (valueToBeAssigend < 0 && totalShouldbeValue < currentState[accountID].Value))
                                {
                                    AccountPercent value = new AccountPercent(accountID, originalPercent, targetPercentage[accountID].Value, totalShouldbeValue);
                                    intermediateQtyDict.Add(value);
                                    availableValue -= totalShouldbeValue;
                                }
                                else
                                {
                                    availableValue -= currentState[accountID].Value;
                                }
                            }
                            else
                            {
                                AccountPercent value = new AccountPercent(accountID, 0, targetPercentage[accountID].Value, 0);
                                intermediateQtyDict.Add(value);
                            }
                        }

                        //Ignore the accounts in which allocation will increase deviation
                        //Example -> Target greater than current with sell order side
                        for (int i = intermediateQtyDict.Count - 1; i >= 0; i--)
                        {
                            AccountPercent accountStatus = intermediateQtyDict[i];
                            decimal initialState = (accountStatus.OriginalPercent * originalTotalQty) / 100m;
                            decimal totalShouldbeValue = (accountStatus.TargetPercent * finalTotalQty) / 100m;

                            //Current quantity not between initial state and target state
                            if ((accountStatus.CurrentQty < initialState && accountStatus.CurrentQty < totalShouldbeValue) //Lower than both
                                || (accountStatus.CurrentQty > initialState && accountStatus.CurrentQty > totalShouldbeValue)) //greater than both
                            {
                                availableValue -= (initialState - accountStatus.CurrentQty);
                                intermediateQtyDict.RemoveAt(i);
                            }
                        }

                        decimal currentTotalDeviation = intermediateQtyDict.Sum(x => x.initialDeviation);
                        decimal percentRemainingToAllocate = Math.Abs((availableValue * 100m) / finalTotalQty);

                        //order by deviation decreasing
                        intermediateQtyDict = intermediateQtyDict.OrderByDescending(x => x.initialDeviation).ToList();

                        //Minimizing deviation in each account
                        for (int i = intermediateQtyDict.Count - 1; i >= 0; i--)
                        {
                            AccountPercent accountStatus = intermediateQtyDict[i];
                            //Check if possible to minimize all accounts deviation to this level
                            if (percentRemainingToAllocate >= (currentTotalDeviation - (accountStatus.initialDeviation * (i + 1))))
                            {
                                //if yes, then optimize all remaining accounts to same level
                                decimal minDeviation = (currentTotalDeviation - percentRemainingToAllocate) / (i + 1);
                                foreach (AccountPercent account in intermediateQtyDict)
                                {
                                    decimal newTargetPercent = account.TargetPercent > account.OriginalPercent ?
                                        account.TargetPercent - minDeviation : account.TargetPercent + minDeviation;

                                    decimal totalShouldbeValue = (finalTotalQty * newTargetPercent) / 100m;
                                    finalQtyDict.Add(account.AccountID, totalShouldbeValue);
                                }
                                break;
                            }
                            else
                            {
                                //If not, then this account can't be optimized further
                                decimal totalShouldbeValue = (finalTotalQty * accountStatus.OriginalPercent) / 100m;
                                finalQtyDict.Add(accountStatus.AccountID, totalShouldbeValue);
                                currentTotalDeviation -= accountStatus.initialDeviation;
                                intermediateQtyDict.RemoveAt(i);
                            }
                        }

                        foreach (int fv in finalQtyDict.Keys)
                        {
                            decimal totalShouldbeValue = finalQtyDict[fv];
                            if (currentState != null && currentState.ContainsKey(fv))
                                totalShouldbeValue -= currentState[fv].Value;

                            if (totalShouldbeValue != 0)
                            {
                                AccountValue fvData = new AccountValue(fv, totalShouldbeValue);
                                result.Add(fvData);
                            }
                        }
                    }
                    else
                    {
                        foreach (int fv in targetPercentage.Keys)
                        {
                            if (currentState != null && currentState.ContainsKey(fv))
                            {
                                AccountValue fvData = new AccountValue(fv, -currentState[fv].Value);
                                result.Add(fvData);
                            }
                        }
                    }
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
            return result;
        }
        /// <summary>
        /// Updates state after allocation of group.
        /// </summary>
        /// <param name="fvCurrent">account value</param>
        /// <param name="group">group to be allocated</param>
        /// <param name="currentStateForSymbol"></param>
        /// <param name="doCheckSide">checks check side on state and quantity</param>
        /// <param name="sideMultiplier"></param>
        /// <param name="quantityToBeInThisLoop">quantity to be adjusted</param>
        /// <returns>true if state is updated false if not</returns>
        public static bool UpdateCurrentState(AllocationBaseType baseType, AccountValue fvCurrent, AllocationGroup group, ref Dictionary<int, AccountValue> currentStateForSymbol, int sideMultiplier, decimal quantityToBeInThisLoop, AllocationParameter parameters, bool doForceCheckSide, decimal notionalToBeInThisLoop = 0.0M, Dictionary<int, AccountValue> currentQuantityStateForSymbol = null)
        {
            try
            {
                var doCheckSide = doForceCheckSide;

                if (!doForceCheckSide)
                {
                    //Checks for check side is enabled or not
                    doCheckSide = CheckSideHelper.DoCheckSide(group, fvCurrent.AccountId, parameters);

                }
                if (doCheckSide && !group.IsOverbuyOversellAccepted)
                {
                    //Validate check side and update accordingly.
                    if (CheckSideHelper.ValidateAllocationOutput(fvCurrent, group, (currentQuantityStateForSymbol ?? currentStateForSymbol)))
                    {
                        //Updating state
                        decimal quantityModification = quantityToBeInThisLoop * sideMultiplier;
                        UpdateStateDictionary(currentStateForSymbol, (baseType == AllocationBaseType.CumQuantity) ? quantityModification : notionalToBeInThisLoop, fvCurrent.AccountId);
                        if (currentQuantityStateForSymbol != null)
                            UpdateStateDictionary(currentQuantityStateForSymbol, quantityModification, fvCurrent.AccountId);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    decimal quantityModification = fvCurrent.Value * sideMultiplier;
                    UpdateStateDictionary(currentStateForSymbol, (baseType == AllocationBaseType.CumQuantity) ? quantityModification : notionalToBeInThisLoop, fvCurrent.AccountId);
                    return true;
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        private static void UpdateStateDictionary(Dictionary<int, AccountValue> stateDictionary, decimal value, int accountId)
        {
            try
            {
                if (!stateDictionary.ContainsKey(accountId))
                    stateDictionary.Add(accountId, new AccountValue(accountId, value));
                else
                    stateDictionary[accountId].AddValue(value);
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
