// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : Disha Sharma
// Created          : 05-05-2017
// ***********************************************************************
// <copyright file="MasterFundAllocator.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Common.Interfaces;
using Prana.Allocation.Core.Extensions;
using Prana.Allocation.Core.Factories;
using Prana.Allocation.Core.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prana.Allocation.Core.Allocator
{
    public abstract class MasterFundAllocator : IMasterFundAllocationGenerator
    {
        /// <summary>
        /// The base type
        /// </summary>
        private AllocationBaseType _baseType = AllocationBaseType.CumQuantity;

        /// <summary>
        /// This returns the AllocationBaseType for current implementation
        /// </summary>
        /// <value>
        /// The type of the base.
        /// </value>
        public AllocationBaseType BaseType
        {
            get { return _baseType; }
            set { _baseType = value; }
        }

        /// <summary>
        /// Generates the specified group list.
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <param name="masterFundPref">The master fund preference.</param>
        /// <returns></returns>
        public MasterFundAllocationOutputResult Generate(List<AllocationGroup> groupList, AllocationMasterFundPreference masterFundPref, int userId)
        {
            MasterFundAllocationOutputResult mfAllocationResult = new MasterFundAllocationOutputResult();
            try
            {
                /*
                 * 1. Get comparer for base type
                 * 2. Validate allocation Input : MasterFund rule validation and group validation for cumQty and notional
                 * 3. If MasterFundRule is
                 *      Leveling : Get masterFund Nav for leveling, also get group and master fund wise market value state and market value of selected groups, PRANA-26601
                 *      Prorata by NAV : Get masterFund wise start of day Nav for Prorata by NAV
                 * 4. Get Symbol sorted data
                 * 5. Get MasterFundAllocationOutput for each symbol (Repeat Steps 6 to 11)
                 * 6. get master fund target percentage if allocation rule is not leveling, because leveling percentage is calculated for next group after each group is allocated 
                 * 7. Get symbol wise orderside sorted data
                 * 8. Get orderside wise virtual groups
                 * 9. For each order side virtual group: create virtual groups for each master fund and update mf virtual group list
                 * 10. get master fund quantity distribution and total quantity
                 * 11. Add MasterFundAllocationOutput to MasterFundAllocationOutputResult
                 */

                object mfResultLocker = new object();
                UpdateBaseType();
                IComparer<AllocationGroup> comparer = ComparerFactory.Instance.GetComparerFor(this.BaseType, false);
                Dictionary<string, List<AllocationGroup>> sortedData = this.SortAllocationGroups(groupList, comparer);
                List<string> groupIds = new List<string>();

                //validate masterfund allocation input
                mfAllocationResult.ErrorMessage = this.ValidateMasterFundAllocationInputs(groupList, masterFundPref);
                if (!String.IsNullOrWhiteSpace(mfAllocationResult.ErrorMessage))
                {
                    return mfAllocationResult;
                }

                string navError = string.Empty;

                //Update master fund wise NAV from expnl in case of Leveling and ProrataByNAV
                switch (masterFundPref.DefaultRule.RuleType)
                {
                    case MatchingRuleType.ProrataByNAV:
                    case MatchingRuleType.Leveling:
                        navError = UpdateMasterFundWiseNAV(masterFundPref.DefaultRule.ProrataAccountList, groupList);
                        break;
                }

                if (string.IsNullOrWhiteSpace(mfAllocationResult.ErrorMessage))
                {
                    Parallel.ForEach(sortedData.Keys, symbol =>
                    {
                        string errorMessage = string.Empty;
                        SerializableDictionary<int, AccountValue> masterFundPercentage = new SerializableDictionary<int, AccountValue>();
                        List<AllocationGroup> symbolWiseGroupList = sortedData[symbol];
                        MatchClosingTransactionType matchClosingTransaction = MatchClosingTransactionType.None;

                        if (masterFundPref.DefaultRule.MatchClosingTransaction != MatchClosingTransactionType.None)
                        {
                            List<int> accounts = new List<int>();
                            if (masterFundPref.DefaultRule.MatchClosingTransaction == MatchClosingTransactionType.SelectedAccounts)
                                accounts = MasterFundAllocationHelper.GetSelectedAccountsList(masterFundPref.MasterFundPreference.Keys.ToList());
                            bool isMatchClosingPossible = AllocationProcessHelper.IsPortfolioPositionPerfectClosePossible(symbol, userId, symbolWiseGroupList, accounts);
                            if (isMatchClosingPossible)
                            {
                                matchClosingTransaction = masterFundPref.DefaultRule.MatchClosingTransaction;
                            }
                        }
                        switch (masterFundPref.DefaultRule.RuleType)
                        {
                            case MatchingRuleType.Prorata:
                                groupIds = symbolWiseGroupList.Select(group => group.GroupID).Distinct().ToList();
                                break;

                            case MatchingRuleType.Leveling:
                            case MatchingRuleType.ProrataByNAV:
                                errorMessage = navError;
                                break;
                        }

                        //in case of leveling get percentage after each group is leveled
                        if (string.IsNullOrWhiteSpace(errorMessage) && masterFundPref.DefaultRule.RuleType != MatchingRuleType.Leveling)
                        {
                            masterFundPercentage = GetMasterFundPercentageForAllocationRule(masterFundPref, symbol, userId, groupIds, out errorMessage);
                        }

                        if (string.IsNullOrWhiteSpace(errorMessage))
                        {
                            Dictionary<string, List<AllocationGroup>> orderSideSortedData = this.SortAllocationGroupsOrderSideWise(symbolWiseGroupList, comparer);
                            List<AllocationGroup> orderSideVirtualGroups = MasterFundAllocationHelper.GetOrderSideWiseVirtualGroups(orderSideSortedData);

                            SerializableDictionary<int, List<AllocationGroup>> mfVirtualGroupList = new SerializableDictionary<int, List<AllocationGroup>>();
                            object virtualGrpListLocker = new object();

                            Parallel.ForEach(orderSideVirtualGroups, (group, state) =>
                            {
                                //in case of leveling get percentage on basis of each orderside
                                if (masterFundPref.DefaultRule.RuleType == MatchingRuleType.Leveling)
                                {
                                    if (orderSideSortedData.ContainsKey(group.OrderSide))
                                        groupIds = orderSideSortedData[group.OrderSide].Select(x => x.GroupID).ToList();
                                    masterFundPercentage = GetMasterFundPercentageForAllocationRule(masterFundPref, symbol, userId, groupIds, out errorMessage);
                                }
                                if (string.IsNullOrEmpty(errorMessage))
                                {
                                    Dictionary<int, AllocationGroup> mfVirtualGroups = MasterFundAllocationHelper.GetMasterFundWiseVirtualGroups(masterFundPercentage, group, masterFundPref.DefaultRule.PreferenceAccountId);
                                    foreach (int mfId in mfVirtualGroups.Keys)
                                    {
                                        lock (virtualGrpListLocker)
                                        {
                                            if (!mfVirtualGroupList.ContainsKey(mfId))
                                                mfVirtualGroupList.Add(mfId, new List<AllocationGroup>());

                                            mfVirtualGroupList[mfId].Add(mfVirtualGroups[mfId]);
                                        }
                                    }
                                }
                                else
                                    state.Break();
                            });

                            Dictionary<int, decimal> masterFundQuantity = mfVirtualGroupList.ToDictionary(t => t.Key, t => Convert.ToDecimal(t.Value.Sum(x => x.CumQty)));
                            decimal totalCumQty = masterFundQuantity.Sum(x => x.Value);

                            lock (mfResultLocker)
                            {
                                if (string.IsNullOrWhiteSpace(errorMessage))
                                    mfAllocationResult.OutputCollection.Add(symbol, new MasterFundAllocationOutput(symbol, totalCumQty, masterFundQuantity, mfVirtualGroupList, symbolWiseGroupList, matchClosingTransaction));
                                else
                                {
                                    if (MasterFundAllocationHelper.IsLongPositionInSelectedGroups(symbolWiseGroupList))
                                        matchClosingTransaction = MatchClosingTransactionType.None;
                                    mfAllocationResult.OutputCollection.Add(symbol, new MasterFundAllocationOutput(symbol, errorMessage, matchClosingTransaction, symbolWiseGroupList));
                                }
                            }
                        }
                        else
                        {
                            lock (mfResultLocker)
                            {
                                if (MasterFundAllocationHelper.IsLongPositionInSelectedGroups(symbolWiseGroupList))
                                    matchClosingTransaction = MatchClosingTransactionType.None;
                                mfAllocationResult.OutputCollection.Add(symbol, new MasterFundAllocationOutput(symbol, errorMessage, matchClosingTransaction, symbolWiseGroupList));
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return mfAllocationResult;
        }

        /// <summary>
        /// Updates the type of the base.
        /// </summary>
        public abstract void UpdateBaseType();

        /// <summary>
        /// Updates the master fund wise nav.
        /// </summary>
        /// <param name="prorataList">The prorata list.</param>
        /// <returns>Error Message</returns>
        public abstract string UpdateMasterFundWiseNAV(List<int> prorataList, List<AllocationGroup> groups);

        /// <summary>
        /// Gets the master fund percentage for allocation rule.
        /// </summary>
        /// <param name="masterFundPref">The master fund preference.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="groupIds">The group ids.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public abstract SerializableDictionary<int, AccountValue> GetMasterFundPercentageForAllocationRule(AllocationMasterFundPreference masterFundPref, string symbol, int userId, List<string> groupIds, out string errorMessage);
    }
}
