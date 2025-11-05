// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-09-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-09-2014
// ***********************************************************************
// <copyright file="AllocationGeneratorExtension.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Common.Helper;
using Prana.Allocation.Common.Interfaces;
using Prana.Allocation.Core.FormulaStore;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Constants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The Extensions namespace.
/// </summary>
namespace Prana.Allocation.Core.Extensions
{
    /// <summary>
    /// Extension methods for IAllocationGenerator instances
    /// </summary>
    internal static class AllocationGeneratorExtension
    {
        #region IAllocationGenerator methods
        /// <summary>
        /// This method sort the allocation group based on provided comparer and do ascending if required
        /// </summary>
        /// <param name="generator">Parent instance of this extension method</param>
        /// <param name="groupList">List of group which to be sorted</param>
        /// <param name="comparer">Comparer which will be used while sorting the groups</param>
        /// <param name="isAscending">If true data will sorted in ascending order otherwise in descending order</param>
        /// <returns>Dictionary&lt;System.String, List&lt;AllocationGroup&gt;&gt;.</returns>
        internal static Dictionary<string, List<AllocationGroup>> SortAllocationGroups(this IAllocationGenerator generator, List<AllocationGroup> groupList, IComparer<AllocationGroup> comparer)
        {
            Dictionary<string, List<AllocationGroup>> result = new Dictionary<string, List<AllocationGroup>>();
            try
            {
                result = SortAllocationGroupsSymbolWise(groupList, comparer);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// chunk groups symbol and dayewise
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="groupList"></param>
        /// <param name="comparer"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        internal static Dictionary<string, SortedDictionary<DateTime, List<AllocationGroup>>> DateWiseSortAllocationGroups(this IAllocationGenerator generator, List<AllocationGroup> groupList, IComparer<AllocationGroup> comparer)
        {
            try
            {
                Dictionary<string, SortedDictionary<DateTime, List<AllocationGroup>>> result = new Dictionary<string, SortedDictionary<DateTime, List<AllocationGroup>>>();

                foreach (AllocationGroup group in groupList)
                {
                    String symbol = CommonHelper.GetSwapSymbol(group.Symbol, group.IsSwapped);//Dividing Swap and non-Swap groups
                    if (!result.ContainsKey(symbol))
                    {
                        result.Add(symbol, new SortedDictionary<DateTime, List<AllocationGroup>>());
                        result[symbol].Add(group.AUECLocalDate.Date, new List<AllocationGroup>());
                    }
                    else if (!result[symbol].ContainsKey(group.AUECLocalDate.Date))
                    {
                        result[symbol].Add(group.AUECLocalDate.Date, new List<AllocationGroup>());
                    }


                    result[symbol][group.AUECLocalDate.Date].Add(group);
                }

                foreach (string symbol in result.Keys)
                {
                    foreach (DateTime dt in result[symbol].Keys)
                    {
                        result[symbol][dt].Sort(comparer);
                    }
                }

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
        /// Sorts the allocation groups order side wise.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="groupList">The group list.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        internal static Dictionary<string, List<AllocationGroup>> SortAllocationGroupsOrderSideWise(this IAllocationGenerator generator, List<AllocationGroup> groupList, IComparer<AllocationGroup> comparer)
        {
            Dictionary<string, List<AllocationGroup>> result = new Dictionary<string, List<AllocationGroup>>();
            try
            {
                result = SortAllocationGroupsOrderSideWise(groupList, comparer);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Validates inputs for allocation to avoid divide by zero while allocating
        /// </summary>
        /// <param name="generator">Defines genereator for allocation</param>
        /// <returns>Error message</returns>
        internal static String ValidateAllocationParameters(this IAllocationGenerator generator, AllocationParameter parameter)
        {
            try
            {
                //Checking sum of percentage is 100 or not if rule is not Prorata
                if (parameter.CheckListWisePreference.RuleType != MatchingRuleType.Prorata && parameter.CheckListWisePreference.RuleType != MatchingRuleType.Leveling && parameter.CheckListWisePreference.RuleType != MatchingRuleType.ProrataByNAV)
                {
                    decimal sumPercentage = 0.0M;
                    foreach (int accountId in parameter.TargetPercentage.Keys)
                    {
                        //if value is negative invalid input
                        if (parameter.TargetPercentage[accountId].Value < 0)
                            return "Account Percentage can not be negative.";

                        decimal strategyPercentage = 0.0M;
                        sumPercentage += parameter.TargetPercentage[accountId].Value;
                        if (parameter.DoStrategyAllocation)
                        {
                            foreach (StrategyValue strategy in parameter.TargetPercentage[accountId].StrategyValueList)
                            {
                                //if value is negative invalid input
                                if (strategy.Value < 0)
                                    return "Strategy Percentage can not be negative.";

                                strategyPercentage += strategy.Value;
                            }
                            if (strategyPercentage != 0 && !strategyPercentage.EqualsPrecise(100M))
                            {
                                StringBuilder message = new StringBuilder();
                                message.AppendLine("Sum of strategy percentage is not 100!");
                                message.AppendLine("Strategy % Entered: " + strategyPercentage);
                                message.AppendLine("Remaining %: " + (100 - strategyPercentage));
                                return message.ToString();
                            }
                        }
                    }
                    if (!sumPercentage.EqualsPrecise(100M))
                    {
                        StringBuilder message = new StringBuilder();
                        message.AppendLine("Sum of target percentage is not 100!");
                        message.AppendLine("Allocation % Entered: " + sumPercentage);
                        message.AppendLine("Remaining %: " + (100 - sumPercentage));
                        return message.ToString();
                    }
                }

                return String.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return "Error in Validating";
            }
        }

        /// <summary>
        /// Validates inputs for allocation to avoid divide by zero while allocating
        /// </summary>
        /// <param name="generator">Defines genereator for allocation</param>
        /// <param name="groupList">List of allocation groups</param>
        /// <returns>Error message</returns>
        internal static String ValidateAllocationGroups(this IAllocationGenerator generator, List<AllocationGroup> groupList)
        {
            try
            {
                return ValidateGroupList(generator.BaseType, groupList);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return "Error in Validating";
            }
        }

        /// <summary>
        /// Gets the state of the key to find for getting.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="clonedParameter">The cloned parameter.</param>
        /// <returns></returns>
        internal static int GetKeyToFindForGettingState(this IAllocationGenerator generator, AllocationParameter clonedParameter)
        {
            int keyToFind = int.MinValue;
            try
            {
                keyToFind = GetKeyToFindForGettingState(clonedParameter.CheckListWisePreference.RuleType, clonedParameter.PreferenceId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return keyToFind;
        }

        /// <summary>
        /// Gets the group identifier list from sorted data
        /// PRANA-21444 
        /// Step1: Extracting current trade groupId to exclude it from prorata State
        /// Step2: Passing it in GetPercentageForProrata and excludind it from P_AL_GetAllocationStateBySymbol sp
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="dateSortedData">The date sorted data.</param>
        /// <returns></returns>
        internal static StringBuilder GetGroupIdList(this IAllocationGenerator generator, SortedDictionary<DateTime, List<AllocationGroup>> dateSortedData)
        {
            StringBuilder groupIds = new StringBuilder();
            try
            {
                Parallel.ForEach(dateSortedData, datewiseGroupDictionary =>
                    {
                        Parallel.ForEach(datewiseGroupDictionary.Value, groups =>
                        {
                            groupIds = groupIds.Append(groups.GroupID + ",");
                        });
                    });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return groupIds;
        }

        /// <summary>
        /// Sets the error message.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="result">The result.</param>
        /// <param name="errorBuilder">The error builder.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="errorMessage">The error message.</param>
        internal static void SetErrorMessage(this IAllocationGenerator generator, AllocationOutputResult result, StringBuilder errorBuilder, string symbol, string errorMessage)
        {
            try
            {
                result.AddAllocationFailedSymbols(new List<string> { symbol });
                if (!errorBuilder.ToString().Contains(errorMessage))
                    errorBuilder.AppendLine(errorMessage);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion

        #region IMasterFundAllocationGenerator methods

        internal static Dictionary<string, List<AllocationGroup>> SortAllocationGroupsOrderSideWise(this IMasterFundAllocationGenerator generator, List<AllocationGroup> groupList, IComparer<AllocationGroup> comparer)
        {
            Dictionary<string, List<AllocationGroup>> result = new Dictionary<string, List<AllocationGroup>>();
            try
            {
                result = SortAllocationGroupsOrderSideWise(groupList, comparer);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Sorts the allocation groups.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="groupList">The group list.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        internal static Dictionary<string, List<AllocationGroup>> SortAllocationGroups(this IMasterFundAllocationGenerator generator, List<AllocationGroup> groupList, IComparer<AllocationGroup> comparer)
        {
            Dictionary<string, List<AllocationGroup>> result = new Dictionary<string, List<AllocationGroup>>();
            try
            {
                result = SortAllocationGroupsSymbolWise(groupList, comparer);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Validates the master fund allocation inputs.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="groupList">The group list.</param>
        /// <param name="masterFundPref">The master fund preference.</param>
        /// <returns></returns>
        internal static string ValidateMasterFundAllocationInputs(this IMasterFundAllocationGenerator generator, List<AllocationGroup> groupList, AllocationMasterFundPreference masterFundPref)
        {
            string errorMessage = string.Empty;
            try
            {
                //validate masterFundPref 
                masterFundPref.IsValid(out errorMessage);
                //validate groupList
                if (string.IsNullOrWhiteSpace(errorMessage))
                    errorMessage = ValidateGroupList(generator.BaseType, groupList);
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
        /// Gets the state of the key to find for getting.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="masterFundPref">The master fund preference.</param>
        /// <returns></returns>
        internal static int GetKeyToFindForGettingState(this IMasterFundAllocationGenerator generator, AllocationMasterFundPreference masterFundPref)
        {
            int keyToFind = int.MinValue;
            try
            {
                keyToFind = GetKeyToFindForGettingState(masterFundPref.DefaultRule.RuleType, masterFundPref.MasterFundPreferenceId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return keyToFind;
        }

        #endregion

        #region Private methods  

        /// <summary>
        /// Gets the state of the key to find for getting.
        /// </summary>
        /// <param name="clonedParameter">The cloned parameter.</param>
        /// <returns></returns>
        private static int GetKeyToFindForGettingState(MatchingRuleType ruleType, int preferenceId)
        {
            int keyToFind = int.MinValue;
            try
            {
                switch (ruleType)
                {
                    case MatchingRuleType.None:
                        keyToFind = int.MinValue;
                        break;
                    case MatchingRuleType.SinceInception:
                    case MatchingRuleType.Leveling:
                        keyToFind = -1;
                        break;
                    case MatchingRuleType.SinceLastChange:
                        keyToFind = preferenceId == -1 ? int.MinValue : preferenceId;
                        break;
                    default:
                        keyToFind = int.MinValue;
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return keyToFind;
        }

        /// <summary>
        /// Sorts the allocation groups symbol wise.
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        private static Dictionary<string, List<AllocationGroup>> SortAllocationGroupsSymbolWise(List<AllocationGroup> groupList, IComparer<AllocationGroup> comparer)
        {
            Dictionary<string, List<AllocationGroup>> result = new Dictionary<string, List<AllocationGroup>>();
            try
            {
                foreach (AllocationGroup group in groupList)
                {
                    String symbol = CommonHelper.GetSwapSymbol(group.Symbol, group.IsSwapped); //Dividing Swap and non-Swap groups                 
                    if (!result.ContainsKey(symbol))
                        result.Add(symbol, new List<AllocationGroup>());

                    result[symbol].Add(group);
                }
                foreach (string symbol in result.Keys)
                {
                    result[symbol].Sort(comparer);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Sorts the allocation groups order side wise.
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        private static Dictionary<string, List<AllocationGroup>> SortAllocationGroupsOrderSideWise(List<AllocationGroup> groupList, IComparer<AllocationGroup> comparer)
        {
            Dictionary<string, List<AllocationGroup>> result = new Dictionary<string, List<AllocationGroup>>();
            try
            {
                foreach (AllocationGroup group in groupList)
                {
                    String orderSide = group.OrderSide;
                    if (!result.ContainsKey(orderSide))
                        result.Add(orderSide, new List<AllocationGroup>());
                    result[orderSide].Add(group);
                }
                foreach (string orderSide in result.Keys)
                {
                    result[orderSide].Sort(comparer);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// Validates the group list.
        /// </summary>
        /// <param name="baseType">Type of the base.</param>
        /// <param name="groupList">The group list.</param>
        /// <returns></returns>
        private static string ValidateGroupList(AllocationBaseType baseType, List<AllocationGroup> groupList)
        {
            try
            {
                foreach (AllocationGroup group in groupList)
                {
                    if (group.RoundLot <= 0)
                        return string.Format("{0}: Invalid roundlot", group.Symbol);

                    switch (baseType)
                    {
                        case AllocationBaseType.CumQuantity:
                            if (group.CumQty == 0)
                                return string.Format("{0}: Quantity is zero for this symbol, Avg Price: {1}", group.Symbol, group.AvgPrice);
                            break;
                        case AllocationBaseType.Notional:
                            decimal notional = NotionalCalculator.GetNotional(group, (decimal)group.CumQty);
                            if (notional == 0.0M)
                                return string.Format("{0}: Notional is zero for this symbol, Quantity: {1}, Avg Price: {2}\nPlease change your allocation method for this trade", group.Symbol, group.CumQty, group.AvgPrice);
                            break;
                    }

                    //Return error message for closed excercised groups. As these can not be re-allocated.
                    //For Unallocated status is none
                    PostTradeEnums.Status st = Prana.Allocation.Core.DataAccess.ServiceProxyConnector.ClosingProxy.CheckGroupStatus(group);

                    switch (st)
                    {
                        case PostTradeEnums.Status.Closed:
                            return group.Symbol + ": Group is partially or fully closed so can't be allocated again";
                        case PostTradeEnums.Status.CorporateAction:
                            return group.Symbol + ": First undo the applied corporate action to make any changes.";
                        case PostTradeEnums.Status.Exercise:
                        case PostTradeEnums.Status.ExerciseAssignManually:
                            return group.Symbol + ": Group is generated by exercise";
                        case PostTradeEnums.Status.CostBasisAdjustment:     //Don't allow to reallocate for group generated by cost adjustment: http://jira.nirvanasolutions.com:8080/browse/PRANA-6806
                            return group.Symbol + ": Group is generated by cost adjustment, so it can't be allocated again.";
                            //case PostTradeEnums.Status.FutureDateClosed:
                            //case PostTradeEnums.Status.IsExercised:
                            //case PostTradeEnums.Status.None
                            //default:
                            // TODO: complete impelementaiton of remaining enum constants

                    }
                }

                return String.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return "Error in Validating";
            }
        }

        #endregion


    }
}