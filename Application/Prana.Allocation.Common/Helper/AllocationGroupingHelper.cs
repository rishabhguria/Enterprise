using Prana.Allocation.Common.Definitions;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Allocation.Common.Helper
{
    public class AllocationGroupingHelper
    {
        /// <summary>
        /// Gets the groups to group.
        /// </summary>
        /// <param name="autoGroupingrules">The auto-grouping rules.</param>
        /// <param name="allocationGroups">The allocation groups.</param>
        /// <returns></returns>
        public static Dictionary<String, List<AllocationGroup>> GetGroupsToGroup(AutoGroupingRules autoGroupingrules, List<AllocationGroup> allocationGroups)
        {
            Dictionary<String, List<AllocationGroup>> tempDictionary = new Dictionary<string, List<AllocationGroup>>();
            try
            {
                int totalCount = allocationGroups.Count;
                List<String> alreadyAddedGroup = new List<string>();
                for (int outerLoop = 0; outerLoop < totalCount; outerLoop++)
                {
                    AllocationGroup outerAllocationGroup = allocationGroups[outerLoop];
                    String currentGroupId = outerAllocationGroup.GroupID;
                    List<AllocationGroup> tempList = new List<AllocationGroup>();

                    int indexOuter = alreadyAddedGroup.BinarySearch(currentGroupId);
                    if (indexOuter >= 0)
                    {
                        continue;
                    }
                    tempList.Add(outerAllocationGroup);
                    alreadyAddedGroup.Insert(~indexOuter, currentGroupId);

                    for (int innerLoop = outerLoop + 1; innerLoop < totalCount; innerLoop++)
                    {
                        AllocationGroup innerAllocationGroup = allocationGroups[innerLoop];
                        int indexInner = alreadyAddedGroup.BinarySearch(innerAllocationGroup.GroupID);

                        if (indexInner >= 0 || !AreGroupsGroupable(innerAllocationGroup, outerAllocationGroup, autoGroupingrules))
                            continue;
                        alreadyAddedGroup.Insert(~indexInner, innerAllocationGroup.GroupID);
                        tempList.Add(innerAllocationGroup);
                    }

                    if (tempList.Count > 1)
                        tempDictionary.Add(currentGroupId, tempList);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return tempDictionary;
        }

        /// <summary>
        /// Ares the groups groupable.
        /// </summary>
        /// <param name="group1">The group1.</param>
        /// <param name="group2">The group2.</param>
        /// <param name="_autoGroupingrules">The _allocation preferences.</param>
        /// <returns></returns>
        public static bool AreGroupsGroupable(AllocationGroup group1, AllocationGroup group2, AutoGroupingRules _autoGroupingrules)
        {
            bool result = false;
            try
            {
                if (group1.IsSwapped || group2.IsSwapped)
                    return result;

                bool bCounterParty = _autoGroupingrules.Broker;
                bool bVenue = _autoGroupingrules.Venue;
                bool bTradingAccount = _autoGroupingrules.TradingAccount;
                bool bBuyAndBCV = _autoGroupingrules.BuyAndBCV;
                bool bProcessDate = _autoGroupingrules.ProcessDate;
                bool bTradeDate = _autoGroupingrules.TradeDate;

                if ((group1.Symbol.Equals(group2.Symbol))
                    && (group1.OrderSideTagValue.Equals(group2.OrderSideTagValue) || ((bBuyAndBCV) && ((group2.OrderSide.Equals("Buy") && group1.OrderSide.Equals("BCV")) || (group1.OrderSide.Equals("Buy") && group2.OrderSide.Equals("BCV")))))
                    && (group1.SettlementCurrencyID.Equals(group2.SettlementCurrencyID))
                    && ((!bCounterParty) || (bCounterParty && group1.CounterPartyID.Equals(group2.CounterPartyID)))
                    && ((!bVenue) || (bVenue && group1.VenueID.Equals(group2.VenueID)))
                    && ((!bTradingAccount) || (bTradingAccount && group1.TradingAccountID.Equals(group2.TradingAccountID)))
                    && (AreTradeAttributesGroupable(group1, group2, _autoGroupingrules)))
                {
                    result = AreDateFieldGroupable(group1, group2, result, bProcessDate, bTradeDate);
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
        /// Ares the date field groupable.
        /// </summary>
        /// <param name="group1">The group1.</param>
        /// <param name="group2">The group2.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <param name="bProcessDate">if set to <c>true</c> [b process date].</param>
        /// <param name="bTradeDate">if set to <c>true</c> [b trade date].</param>
        /// <returns></returns>
        private static bool AreDateFieldGroupable(AllocationGroup group1, AllocationGroup group2, bool result, bool bProcessDate, bool bTradeDate)
        {
            try
            {
                if (((bTradeDate && bProcessDate) && ((group1.AUECLocalDate.Date.Equals(group2.AUECLocalDate.Date))
                    && (group1.ProcessDate.Date.Equals(group2.ProcessDate.Date))))
                    || (bTradeDate && !bProcessDate && group1.AUECLocalDate.Date.Equals(group2.AUECLocalDate.Date))
                    || (bProcessDate && !bTradeDate && group1.ProcessDate.Date.Equals(group2.ProcessDate.Date)))
                    result = true;
                else
                    result = false;
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
        /// Checks if attributes for group are equal or not for auto grouping.
        /// </summary>
        /// <param name="group1">The group1.</param>
        /// <param name="group2">The group2.</param>
        /// <param name="autoGroupingrules">The allocation preferences.</param>
        /// <returns></returns>
        private static bool AreTradeAttributesGroupable(AllocationGroup group1, AllocationGroup group2, AutoGroupingRules autoGroupingrules)
        {
            try
            {
                bool bTradeAttributes1 = autoGroupingrules.TradeAttributes1;
                bool bTradeAttributes2 = autoGroupingrules.TradeAttributes2;
                bool bTradeAttributes3 = autoGroupingrules.TradeAttributes3;
                bool bTradeAttributes4 = autoGroupingrules.TradeAttributes4;
                bool bTradeAttributes5 = autoGroupingrules.TradeAttributes5;
                bool bTradeAttributes6 = autoGroupingrules.TradeAttributes6;

                if (
                       (!bTradeAttributes1 || IsTradeAttributeEqual(group1.TradeAttribute1, group2.TradeAttribute1))
                    && (!bTradeAttributes2 || IsTradeAttributeEqual(group1.TradeAttribute2, group2.TradeAttribute2))
                    && (!bTradeAttributes3 || IsTradeAttributeEqual(group1.TradeAttribute3, group2.TradeAttribute3))
                    && (!bTradeAttributes4 || IsTradeAttributeEqual(group1.TradeAttribute4, group2.TradeAttribute4))
                    && (!bTradeAttributes5 || IsTradeAttributeEqual(group1.TradeAttribute5, group2.TradeAttribute5))
                    && (!bTradeAttributes6 || IsTradeAttributeEqual(group1.TradeAttribute6, group2.TradeAttribute6))
                    && (CompareAdditionalTradeAttributes(group1, group2, autoGroupingrules))
                   )
                    return true;
                else
                    return false;
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
        /// Compares trade attribute values between two groups based on the rules defined in AdditionalTradeAttributes.
        /// Only attributes with a 'true' flag are compared; attributes with 'false' are ignored.
        /// </summary>
        /// <param name="group1">The first group object to compare.</param>
        /// <param name="group2">The second group object to compare.</param>
        /// <param name="autoGroupingRules">The auto grouping rules containing which attributes to compare.</param>
        /// <returns>True if all applicable attributes are equal; otherwise, false.</returns>
        private static bool CompareAdditionalTradeAttributes(AllocationGroup group1, AllocationGroup group2, AutoGroupingRules autoGroupingRules)
        {
            foreach (var kvp in autoGroupingRules.GetTradeAttributesAsDict())
            {
                string attributeName = kvp.Key;
                bool shouldCompare = kvp.Value;

                if (!shouldCompare)
                {
                    continue; // Skip this attribute
                }

                if (!IsTradeAttributeEqual(group1.GetTradeAttributeValue(attributeName), group2.GetTradeAttributeValue(attributeName)))
                {
                    return false;
                }
            }
            return true; // All compared attributes matched
        }

        /// <summary>
        /// Checks if two strings are equal or not.
        /// </summary>
        /// <param name="tradeAttribute1">string1</param>
        /// <param name="tradeAttribute2">string2</param>
        /// <returns></returns>
        private static bool IsTradeAttributeEqual(string tradeAttribute1, string tradeAttribute2)
        {
            try
            {
                if (string.IsNullOrEmpty(tradeAttribute1))
                    tradeAttribute1 = string.Empty;

                if (string.IsNullOrEmpty(tradeAttribute2))
                    tradeAttribute2 = string.Empty;

                return tradeAttribute1.Trim().Equals(tradeAttribute2.Trim());
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
        /// Check Order Fields
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bTradeDate"></param>
        /// <param name="bProcessDate"></param>
        /// <param name="newGroup"></param>
        public static void UpdateOrderDetails(int userId, string userName, bool bTradeDate, bool bProcessDate, AllocationGroup newGroup)
        {
            try
            {
                foreach (AllocationOrder order in newGroup.Orders)
                {
                    UpdateOrderBasicDetails(userId, userName, bTradeDate, bProcessDate, newGroup, order);
                    UpdateOrderTradeAttributes(newGroup, order);
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
        /// Updates the order basic details.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="bTradeDate">if set to <c>true</c> [b trade date].</param>
        /// <param name="bProcessDate">if set to <c>true</c> [b process date].</param>
        /// <param name="newGroup">The new group.</param>
        /// <param name="order">The order.</param>
        private static void UpdateOrderBasicDetails(int userId, string userName, bool bTradeDate, bool bProcessDate, AllocationGroup newGroup, AllocationOrder order)
        {
            try
            {
                if (newGroup.OrderSideTagValue != String.Empty && newGroup.OrderSideTagValue != order.OrderSideTagValue)
                {
                    newGroup.OrderSideTagValue = String.Empty;
                    newGroup.OrderSide = String.Empty;
                }
                if (newGroup.CounterPartyID != 0 && newGroup.CounterPartyID != order.CounterPartyID)
                {
                    newGroup.CounterPartyID = 0;
                    newGroup.CounterPartyName = String.Empty;
                }
                if (newGroup.UserID != order.CompanyUserID)
                {
                    newGroup.UserID = userId;
                    newGroup.CompanyUserName = userName;
                    newGroup.CompanyUserID = userId;
                }
                if (newGroup.VenueID != 0 && newGroup.VenueID != order.VenueID)
                {
                    newGroup.VenueID = 0;
                    newGroup.Venue = String.Empty;
                }
                if (newGroup.TradingAccountID != 0 && newGroup.TradingAccountID != order.TradingAccountID)
                {
                    newGroup.TradingAccountID = 0;
                    newGroup.TradingAccountName = String.Empty;
                }

                newGroup.SettlementCurrencyID = (newGroup.SettlementCurrencyID != order.SettlementCurrencyID) ? 0 : newGroup.SettlementCurrencyID;

                newGroup.ProcessDate = (bTradeDate && newGroup.ProcessDate < order.ProcessDate) ? order.ProcessDate : newGroup.ProcessDate;

                newGroup.AUECLocalDate = (bProcessDate && newGroup.AUECLocalDate < order.AUECLocalDate) ? order.AUECLocalDate : newGroup.AUECLocalDate;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the order trade attributes.
        /// </summary>
        /// <param name="newGroup">The new group.</param>
        /// <param name="order">The order.</param>
        private static void UpdateOrderTradeAttributes(AllocationGroup newGroup, AllocationOrder order)
        {
            try
            {
                //Set Trade Attribute values
                newGroup.TradeAttribute1 = GetTradeAttributeValue(newGroup.TradeAttribute1, order.TradeAttribute1) ? string.Empty : newGroup.TradeAttribute1;
                newGroup.TradeAttribute2 = GetTradeAttributeValue(newGroup.TradeAttribute2, order.TradeAttribute2) ? string.Empty : newGroup.TradeAttribute2;
                newGroup.TradeAttribute3 = GetTradeAttributeValue(newGroup.TradeAttribute3, order.TradeAttribute3) ? string.Empty : newGroup.TradeAttribute3;
                newGroup.TradeAttribute4 = GetTradeAttributeValue(newGroup.TradeAttribute4, order.TradeAttribute4) ? string.Empty : newGroup.TradeAttribute4;
                newGroup.TradeAttribute5 = GetTradeAttributeValue(newGroup.TradeAttribute5, order.TradeAttribute5) ? string.Empty : newGroup.TradeAttribute5;
                newGroup.TradeAttribute6 = GetTradeAttributeValue(newGroup.TradeAttribute6, order.TradeAttribute6) ? string.Empty : newGroup.TradeAttribute6;

                for (int attributeIndex = 7; attributeIndex <= 45; attributeIndex++)
                {
                    string attributeName = $"TradeAttribute{attributeIndex}";

                    // Get the current value of the attribute from the group and the order
                    string groupValue = newGroup.GetTradeAttributeValue(attributeName);
                    string orderValue = order.GetTradeAttributeValue(attributeName);

                    // Determine if the values differ or are missing
                    bool valuesDiffer = GetTradeAttributeValue(groupValue, orderValue);

                    // If values differ, clear the group's attribute; otherwise, retain the original
                    newGroup.SetTradeAttributeValue(attributeName, valuesDiffer ? string.Empty : groupValue);
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
        /// Get Trade Attribute Value
        /// </summary>
        /// <param name="groupVal"></param>
        /// <param name="orderVal"></param>
        /// <returns></returns>
        private static bool GetTradeAttributeValue(string groupVal, string orderVal)
        {
            return (string.IsNullOrWhiteSpace(groupVal) || string.IsNullOrWhiteSpace(orderVal) || groupVal.Trim() != orderVal.Trim());
        }

        /// <summary>
        /// Updates the taxlot details.
        /// </summary>
        /// <param name="group">The allocation group.</param>
        public static void UpdateTaxlotDetails(AllocationGroup groupObj, ApplicationConstants.TaxLotState TaxLotState = ApplicationConstants.TaxLotState.New)
        {
            try
            {
                TaxLot unallocatedtaxlot = CommonHelper.CreateUnAllocatedTaxLot(groupObj, groupObj.GroupID);
                unallocatedtaxlot.TaxLotState = TaxLotState;
                List<TaxLot> newtaxlots = new List<TaxLot>();
                newtaxlots.Add(unallocatedtaxlot);
                groupObj.ResetTaxlotDictionary(newtaxlots);
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
