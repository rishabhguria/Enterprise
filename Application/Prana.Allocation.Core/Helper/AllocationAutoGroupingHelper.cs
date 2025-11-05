using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Common.Helper;
using Prana.Allocation.Core.CacheStore;
using Prana.Allocation.Core.DataAccess;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Allocation.Core.Helper
{
    internal class AllocationAutoGroupingHelper
    {
        /// <summary>
        /// Groups the with existing.
        /// </summary>
        /// <param name="autoGroupingrules">The automatic groupingrules.</param>
        /// <param name="allocationGroups">The allocation groups.</param>
        /// <returns></returns>
        internal static Object[] GroupWithExisting(AutoGroupingRules autoGroupingrules, AllocationGroup incomingGroup)
        {
            Object[] response = new object[2];
            try
            {
                AllocationPrefetchFilter filter = new AllocationPrefetchFilter();
                filter.Allocated.Add("AccountID", string.Join(",", Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccounts().Keys));
                filter.Allocated.Add("FromDate", DateTime.UtcNow.Date.ToString());
                List<AllocationGroup> existingGroups = GroupCache.GetInstance().AutoGroupingCache;
                Dictionary<string, PostTradeEnums.Status> groupStatus = ServiceProxyConnector.ClosingProxy.GetGroupStatus(existingGroups);
                existingGroups = existingGroups.Where(grp =>
                   ((grp.Orders.All(ord => ((ord.TransactionSourceTag == (int)TransactionSource.TradingTicket) ||(ord.TransactionSourceTag == (int)TransactionSource.HotButton))) && grp.IsManualGroup == false) || (grp.Orders.All(ord => ord.TransactionSourceTag == (int)TransactionSource.FIX))) && (groupStatus[grp.GroupID].Equals(PostTradeEnums.Status.None)) && !grp.IsManuallyModified
                    && (grp.State.Equals(PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED) || !grp.AllocationSchemeName.Equals("Manual") || grp.TaxLots.Count == 1)).ToList();

                List<AllocationGroup> deletedGroups = new List<AllocationGroup>();
                List<AllocationGroup> newGroups = new List<AllocationGroup>();
                bool isGrouped = false;
                for (int i = 0; i < existingGroups.Count; i++)
                {
                    AllocationGroup existingGroup = (AllocationGroup)existingGroups[i].Clone();
                    if (existingGroup.State.Equals(PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED) && existingGroup.AllocationSchemeName.Equals("Manual") && existingGroup.TaxLots.Count == 1)
                    {
                        if (existingGroup.AllocationSchemeName.Equals("Manual"))
                        {
                            if (existingGroup.TaxLots.Count == 1)
                            {
                                existingGroup.AllocationSchemeID = existingGroup.TaxLots[0].Level1ID;
                                existingGroup.StrategyID = existingGroup.TaxLots[0].Level2ID;
                            }
                        }
                    }
                    if (AllocationGroupingHelper.AreGroupsGroupable(incomingGroup, existingGroup, autoGroupingrules) && IsSameAllocation(incomingGroup, existingGroup))
                    {
                        AuditManager.Instance.AddGroupToAuditEntry(existingGroup, false, TradeAuditActionType.ActionType.UNALLOCATE, existingGroup.Quantity.ToString(), "0", TradeAuditActionType.AllocationAuditComments.GroupDeleted.ToString(), -1);
                        if (existingGroup.TaxLots.Count > 0)
                        {
                            AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(existingGroup, true, TradeAuditActionType.ActionType.UNALLOCATE, existingGroup.Quantity.ToString(), "0", TradeAuditActionType.AllocationAuditComments.TaxlotDeleted.ToString(), -1);
                        }
                        AllocationGroup newGroup = BundleGroups(autoGroupingrules, new List<AllocationGroup>() { existingGroup, incomingGroup }, -1);
                        newGroups.Add(newGroup);
                        AuditManager.Instance.AddGroupToAuditEntry(existingGroup, true, TradeAuditActionType.ActionType.GROUP, existingGroup.Quantity.ToString(), "0", TradeAuditActionType.AllocationAuditComments.TradesGroupedDeleted.ToString(), -1);
                        AuditManager.Instance.AddGroupToAuditEntry(newGroup, false, TradeAuditActionType.ActionType.GROUP, "0", newGroup.Quantity.ToString(), TradeAuditActionType.AllocationAuditComments.TradesGroupedCreated.ToString(), -1);
                        existingGroup.ResetTaxlotDictionaryState(ApplicationConstants.TaxLotState.Deleted);
                        existingGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                        existingGroup.TaxLots = new List<TaxLot>();
                        deletedGroups.Add(existingGroup);
                        isGrouped = true;
                        break;
                    }
                }
                if (!isGrouped)
                    newGroups.Add(incomingGroup);
                response[0] = deletedGroups;
                response[1] = newGroups;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return response;
        }

        /// <summary>
        /// Determines whether [is same allocation] [the specified GRP1].
        /// </summary>
        /// <param name="grp1">The GRP1.</param>
        /// <param name="grp2">The GRP2.</param>
        /// <returns>
        ///   <c>true</c> if [is same allocation] [the specified GRP1]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsSameAllocation(AllocationGroup grp1, AllocationGroup grp2)
        {
            try
            {
                return grp1.State.Equals(grp2.State) &&
                    (grp1.State.Equals(PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED) ||
                        (grp1.AllocationSchemeID.Equals(grp2.AllocationSchemeID) &&
                        (!grp2.AllocationSchemeName.Equals("Manual") || grp1.StrategyID.Equals(grp2.StrategyID))));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// Automatics the group.
        /// </summary>
        /// <param name="allocationPreferences">The allocation preferences.</param>
        /// <param name="allocationGroups">The temporary unallocated groups</param>
        /// <returns></returns>
        internal static Dictionary<string, List<AllocationGroup>> AutoGroupImportedTrades(AutoGroupingRules autoGroupingrules, Dictionary<string, List<AllocationGroup>> allocationGroupsDict, int userId)
        {

            Dictionary<string, List<AllocationGroup>> newGroupsDict = new Dictionary<string, List<AllocationGroup>>();
            try
            {
                foreach (var key in allocationGroupsDict.Keys)
                {
                    List<AllocationGroup> allocationGroups = allocationGroupsDict[key];
                    if (allocationGroups != null && allocationGroups.Count > 0)
                    {
                        List<AllocationGroup> newGroupedList = new List<AllocationGroup>();
                        Dictionary<String, List<AllocationGroup>> tempDictionary = AllocationGroupingHelper.GetGroupsToGroup(autoGroupingrules, allocationGroups);
                        List<AllocationGroup> newGroups = new List<AllocationGroup>();
                        foreach (string groupId in tempDictionary.Keys)
                        {
                            newGroups.Add(BundleGroups(autoGroupingrules, tempDictionary[groupId], userId));
                        }
                        if (tempDictionary.Count > 0)
                        {
                            Dictionary<String, AllocationGroup> groupedGroupDict = tempDictionary.SelectMany(d => d.Value).ToDictionary(group => group.GroupID, group => group);
                            newGroups.ForEach(groupedGroup => newGroupedList.Add(groupedGroup));
                            allocationGroups.ForEach(group =>
                            {
                                if (!groupedGroupDict.ContainsKey(group.GroupID))
                                    newGroupedList.Add(group);
                            });
                        }
                        else
                        {
                            allocationGroups.ForEach(group => newGroupedList.Add(group));
                        }
                        newGroupsDict.Add(key, newGroupedList);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return newGroupsDict;
        }

        /// <summary>
        /// Bundles the groups.
        /// </summary>
        /// <param name="allocationPreferences">The allocation preferences.</param>
        /// <param name="groups">The groups.</param>
        private static AllocationGroup BundleGroups(AutoGroupingRules autoGroupingrules, List<AllocationGroup> groups, int userId)
        {
            try
            {
                bool bTradeDate = autoGroupingrules.TradeDate;
                bool bProcessDate = autoGroupingrules.ProcessDate;
                int i = 0;
                AllocationGroup newGroup = GetNewAllocationGroup(groups[0]);

                //update basic details
                newGroup.IsModified = false;
                newGroup.OriginalAllocationPreferenceID = 0;
                //We are assigning None when two Allocation groups are Grouped (regardless of their current TransactionSource), Current TransactionSource may be same for those groups
                newGroup.TransactionSource = TransactionSource.None;
                newGroup.TransactionSourceTag = (int)TransactionSource.None;

                // add other groups to new groups
                foreach (AllocationGroup group in groups)
                {
                    if (i != 0)
                        newGroup.AddGroup(group);
                    //AuditManager.Instance.AddGroupToAuditEntry(group, true, DateTime.UtcNow, TradeAuditActionType.ActionType.GROUP, "", "Trades Grouped(Deleted)", userId);
                    i++;
                }

                //check and update order fields
                string userName = CachedDataManager.GetInstance.GetUserText(newGroup.UserID);
                AllocationGroupingHelper.UpdateOrderDetails(userId, userName, bTradeDate, bProcessDate, newGroup);

                //update taxlot details
                AllocationGroupingHelper.UpdateTaxlotDetails(newGroup);

                //AuditManager.Instance.AddGroupToAuditEntry(newGroup, false, DateTime.UtcNow, TradeAuditActionType.ActionType.GROUP, "", "Trades Grouped(Created)", userId);

                //AUECRoundingRulesHelper.ApplyRounding(newGroup);
                return newGroup;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Gets the new allocation group.
        /// </summary>
        /// <param name="group">The groups.</param>
        /// <returns></returns>
        private static AllocationGroup GetNewAllocationGroup(AllocationGroup group)
        {
            AllocationGroup newGroup = (AllocationGroup)group.Clone();
            try
            {
                newGroup.PersistenceStatus = ApplicationConstants.PersistenceStatus.New;
                newGroup.GroupID = AllocationIDGenerator.GenerateGroupID();
                newGroup.ClearTaxlotDictionary();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return newGroup;
        }


    }
}
