using Prana.Allocation.Client.Constants;
using Prana.Allocation.ClientLibrary.DataAccess;
using Prana.Allocation.Client.Definitions;
using Prana.Allocation.Common.Helper;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Prana.Allocation.Client.Helper
{
    internal class AllocationManualGroupingHelper
    {
        #region Group Trades

        /// <summary>
        /// Automatics the group.
        /// </summary>
        /// <param name="allocationPreferences">The allocation preferences.</param>
        /// <param name="allocationGroups">The temporary unallocated groups</param>
        /// <returns></returns>
        internal static Object[] AutoGroup(AllocationPreferences allocationPreferences, List<AllocationGroup> allocationGroups, int userId)
        {
            Object[] response = new object[3];
            try
            {
                int initialCount = 0;
                int finalCount = 0;
                Dictionary<String, List<AllocationGroup>> tempDictionary = new Dictionary<string, List<AllocationGroup>>();

                tempDictionary = AllocationGroupingHelper.GetGroupsToGroup(allocationPreferences.AutoGroupingRules, allocationGroups);
                List<AllocationGroup> newGroups = new List<AllocationGroup>();
                foreach (string groupId in tempDictionary.Keys)
                {
                    if (tempDictionary[groupId].Count <= 1) continue;
                    initialCount += tempDictionary[groupId].Count;
                    finalCount++;
                    newGroups.Add(BundleGroups(allocationPreferences, tempDictionary[groupId], userId));
                }
                if (initialCount > 0)
                {
                    response[0] = "Out of " + allocationGroups.Count + " trade(s), " + initialCount + " is/are grouped into " + finalCount + " group(s)";
                    response[1] = tempDictionary.SelectMany(d => d.Value).ToList();
                    response[2] = newGroups;
                    return response;
                }
                response[0] = "Nothing to group.";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                response[0] = "Some Error has occurred";
            }
            return response;
        }

        /// <summary>
        /// Bundles the groups.
        /// </summary>
        /// <param name="allocationPreferences">The allocation preferences.</param>
        /// <param name="groups">The groups.</param>
        private static AllocationGroup BundleGroups(AllocationPreferences allocationPreferences, List<AllocationGroup> groups, int userId)
        {
            try
            {
                bool bTradeDate = allocationPreferences.AutoGroupingRules.TradeDate;
                bool bProcessDate = allocationPreferences.AutoGroupingRules.ProcessDate;
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
                    AuditManager.Instance.AddGroupToAuditEntry(group, true, TradeAuditActionType.ActionType.GROUP, group.Quantity.ToString(), "0", TradeAuditActionType.AllocationAuditComments.TradesGroupedDeleted.ToString(), userId);
                    i++;
                }

                //check and update order fields
                string userName = CachedDataManager.GetInstance.GetUserText(newGroup.UserID);
                AllocationGroupingHelper.UpdateOrderDetails(userId, userName, bTradeDate, bProcessDate, newGroup);

                //update taxlot details
                AllocationGroupingHelper.UpdateTaxlotDetails(newGroup);

                AuditManager.Instance.AddGroupToAuditEntry(newGroup, false, TradeAuditActionType.ActionType.GROUP, "0", newGroup.Quantity.ToString(), TradeAuditActionType.AllocationAuditComments.TradesGroupedCreated.ToString(), userId);
                //AUECRoundingRulesHelper.ApplyRounding(newGroup);

                if (newGroup.TransactionSourceTag == (int)TransactionSource.FIX || newGroup.Orders.All(ord => ord.TransactionSourceTag == (int)TransactionSource.FIX) || (newGroup.TransactionSourceTag == (int)TransactionSource.TradingTicket && newGroup.Orders.All(ord => ord.TransactionSourceTag == (int)TransactionSource.TradingTicket) && newGroup.IsManualGroup == false))
                {
                    int avgRounding = CachedDataManager.GetInstance.GetAvgPriceRounding();
                    if (avgRounding >= 0)
                    {
                        newGroup.AvgPrice = Math.Round(newGroup.AvgPrice, avgRounding, MidpointRounding.AwayFromZero);
                        newGroup.UpdateTaxlotAvgPrice();
                    }
                }

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

        #endregion

        #region Ungroup Trades

        /// <summary>
        /// Ungroup data.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        internal static Object[] UngroupData(List<AllocationGroup> groups, int userId)
        {
            Object[] response = new object[3];
            try
            {
                List<AllocationGroup> validGroups = GetGroupsToUngroup(groups);

                if (validGroups.Count > 0)
                    response = UnbundleGroups(validGroups, userId);
                else
                {
                    response[0] = "Nothing to Group";
                    response[1] = null;
                    response[2] = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                response[0] = "Error";
            }
            return response;
        }

        /// <summary>
        /// Gets the groups to ungroup.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        private static List<AllocationGroup> GetGroupsToUngroup(List<AllocationGroup> groups)
        {
            List<AllocationGroup> allocationGroups = new List<AllocationGroup>();
            try
            {
                StringBuilder modifiedTrades = new StringBuilder();
                groups.ForEach(group =>
                {
                    if (group.IsModified && group.Orders.Count > 1)
                    {
                        modifiedTrades.Append("GroupID : " + group.GroupID);
                        modifiedTrades.Append(", Symbol : " + group.Symbol);
                        modifiedTrades.Append(", Qty : " + group.CumQty);
                        modifiedTrades.Append(", AvgPrice : " + group.AvgPrice);
                        modifiedTrades.Append(" changes are done at group level,");
                        modifiedTrades.Append(Environment.NewLine);
                    }
                    else
                        allocationGroups.Add(group);
                });
                if (modifiedTrades.Length > 0)
                {
                    modifiedTrades.Append("that will be lost in this process.");
                    modifiedTrades.Append(Environment.NewLine);
                    modifiedTrades.Append(" Do you want to ungroup modified groups also?");
                    MessageBoxResult userChoice = MessageBoxResult.No;
                    userChoice = MessageBox.Show(modifiedTrades.ToString(), AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.YesNoCancel, MessageBoxImage.Information);

                    if (userChoice.Equals(MessageBoxResult.Yes))
                        allocationGroups = groups;
                    else if (userChoice.Equals(MessageBoxResult.Cancel))
                        allocationGroups = new List<AllocationGroup>();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationGroups;
        }

        /// <summary>
        /// Unbundle groups.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        private static Object[] UnbundleGroups(List<AllocationGroup> groups, int userId)
        {
            Object[] response = new object[3];
            try
            {
                int initialGroups = 0;
                int finalGroups = 0;
                int totalGroups = groups.Count;
                Dictionary<string, PostTradeEnums.Status> statusDictionary = new Dictionary<string, PostTradeEnums.Status>();
                bool isAllocated = groups[0].State.Equals(PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED);
                if (isAllocated)
                {
                    statusDictionary = AllocationClientManager.GetInstance().GetGroupStatus(groups);
                }
                List<AllocationGroup> newGroups = new List<AllocationGroup>();
                List<AllocationGroup> deletedGroups = new List<AllocationGroup>();
                if (statusDictionary.Any(val => !val.Value.Equals(PostTradeEnums.Status.None)))
                    MessageBox.Show("Some groups could not be ungrouped because they were fully or partially closed.", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                foreach (AllocationGroup group in groups)
                {
                    if (group.Orders.Count > 1)
                    {
                        SerializableDictionary<int, AccountValue> targetpercentage = new SerializableDictionary<int, AccountValue>();
                        AllocationGroup unAllocatedGroup = null;
                        bool isPref = false; int level1ID = int.MinValue;
                        if (isAllocated)
                        {
                            if (!statusDictionary[group.GroupID].Equals(PostTradeEnums.Status.None))
                                continue;
                            else
                            {
                                if (group.AllocationSchemeName != "Manual")
                                {
                                    isPref = true;
                                    level1ID = group.AllocationSchemeID;
                                }
                                else
                                {
                                    targetpercentage = CommonAllocationMethods.GetAllocationDistributionDict(group);
                                }
                                AuditManager.Instance.AddGroupToAuditEntry(group, false, TradeAuditActionType.ActionType.UNALLOCATE, "", "", TradeAuditActionType.AllocationAuditComments.UnallocatedAutomated.ToString(), userId);
                                if (group.TaxLots.Count > 0)
                                {
                                    AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(group, true, TradeAuditActionType.ActionType.UNALLOCATE, group.Quantity.ToString(), "0", TradeAuditActionType.AllocationAuditComments.TaxlotDeleted.ToString(), userId);
                                }
                                List<AllocationGroup> unAllocatedGroups = AllocationClientDataManager.GetInstance.UnAllocateGroups(new List<AllocationGroup>() { group }, userId);
                                if (unAllocatedGroups != null && unAllocatedGroups.Count == 1)
                                {
                                    unAllocatedGroup = unAllocatedGroups[0];
                                }
                            }
                        }
                        else
                            unAllocatedGroup = group;
                        deletedGroups.Add(group);
                        initialGroups++;
                        AuditManager.Instance.AddGroupToAuditEntry(unAllocatedGroup, true, TradeAuditActionType.ActionType.UNGROUP, unAllocatedGroup.Quantity.ToString(), "0", TradeAuditActionType.AllocationAuditComments.GroupsUngroupedDeleted.ToString(), userId);

                        AllocationGroup clonableGroup = GetAllocationGroupClone(unAllocatedGroup);
                        foreach (AllocationOrder order in unAllocatedGroup.Orders)
                        {
                            finalGroups++;
                            AllocationGroup newGroup = UpdateGroupFromOrder(userId, clonableGroup, order);
                            if (isAllocated)
                            {
                                AllocationResponse resp = null;
                                if (isPref)
                                {
                                    resp = AllocationClientDataManager.GetInstance.AllocateByPreference(new List<AllocationGroup>() { newGroup }, level1ID, userId, false, false, false);
                                }
                                else
                                {
                                    AllocationRule rule = new AllocationRule() { BaseType = AllocationBaseType.CumQuantity, RuleType = MatchingRuleType.None, PreferenceAccountId = -1, MatchClosingTransaction = MatchClosingTransactionType.None };
                                    AllocationParameter param = new AllocationParameter(rule, targetpercentage, -1, userId, true);
                                    resp = AllocationClientDataManager.GetInstance.AllocateByParameter(new List<AllocationGroup>() { newGroup }, param, false, false);
                                }
                                if (resp != null && resp.GroupList != null && resp.GroupList.Count == 1)
                                {
                                    newGroup = resp.GroupList[0];
                                    AuditManager.Instance.AddGroupToAuditEntry(newGroup, false, TradeAuditActionType.ActionType.REALLOCATE, "", "", TradeAuditActionType.AllocationAuditComments.GroupCreated.ToString(), userId);
                                    AuditManager.Instance.AddTaxlotsFromGroupToAuditEntry(newGroup, false, TradeAuditActionType.ActionType.REALLOCATE, "0", group.Quantity.ToString(), TradeAuditActionType.AllocationAuditComments.TaxlotCreated.ToString(), userId);
                                }
                            }
                            if (newGroup != null)
                            {
                                newGroup.IsManuallyModified = true;
                                newGroups.Add(newGroup);
                            }
                        }

                    }
                }

                if (initialGroups != finalGroups)
                {
                    response[0] = "Out of " + totalGroups.ToString() + " group(s), " + initialGroups.ToString() + " is/are ungrouped to " + finalGroups.ToString() + " trade(s)";
                    response[1] = deletedGroups;
                    response[2] = newGroups;
                }
                else
                    response[0] = "Nothing to ungroup.";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                response[0] = "Error";
            }
            return response;
        }

        /// <summary>
        /// Gets the allocation group clone.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        private static AllocationGroup GetAllocationGroupClone(AllocationGroup group)
        {
            AllocationGroup clonableGroup = (AllocationGroup)group.Clone();
            try
            {
                clonableGroup.ClearTaxlotDictionary();
                clonableGroup.Orders.Clear();
                clonableGroup.Orders = null;
                clonableGroup.OrdersH = null;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return clonableGroup;
        }

        /// <summary>
        /// Updates the group from order.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="clonableGroup">The clonable group.</param>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        private static AllocationGroup UpdateGroupFromOrder(int userId, AllocationGroup clonableGroup, AllocationOrder order)
        {
            AllocationGroup newGroup = null;
            try
            {
                newGroup = GetNewAllocationGroup(clonableGroup);
                newGroup.Orders = new List<AllocationOrder>();
                newGroup.SetGroupDetailsFromOrder(order);
                newGroup.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(newGroup.OrderSideTagValue);
                newGroup.Venue = CachedDataManager.GetInstance.GetVenueText(newGroup.VenueID);
                newGroup.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(newGroup.CounterPartyID);
                newGroup.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(newGroup.TradingAccountID);
                var nameOfSource = Enum.GetName(typeof(TransactionSource), newGroup.TransactionSourceTag);
                newGroup.TransactionSource = EnumHelper.GetValueFromEnumDescription<TransactionSource>(nameOfSource);
                newGroup.CompanyUserName = CachedDataManager.GetInstance.GetUserText(newGroup.CompanyUserID);

                AllocationGroupingHelper.UpdateTaxlotDetails(newGroup);
                AuditManager.Instance.AddGroupToAuditEntry(newGroup, false, TradeAuditActionType.ActionType.UNGROUP, "0", newGroup.Quantity.ToString(), TradeAuditActionType.AllocationAuditComments.UngroupedGroupsCreated.ToString(), userId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return newGroup;
        }

        #endregion

        #region Common Methods

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
                newGroup.GroupID = AllocationClientDataManager.GetInstance.GenerateGroupID();
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

        #endregion

    }
}
