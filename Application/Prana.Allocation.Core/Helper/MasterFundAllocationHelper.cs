using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Allocation.Core.Helper
{
    internal static class MasterFundAllocationHelper
    {
        /// <summary>
        /// Gets the master fund wise quantity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="totalCumQty">The total cum qty.</param>
        /// <param name="serializableDictionary">The serializable dictionary.</param>
        /// <param name="remainderQuantityId">The remainder quantity identifier.</param>
        /// <returns></returns>
        internal static Dictionary<T, decimal> GetQuantityForPercentage<T>(decimal totalCumQty, Dictionary<T, decimal> serializableDictionary, T remainderQuantityId)
        {
            Dictionary<T, decimal> masterFundQuantity = new Dictionary<T, decimal>();
            try
            {
                foreach (T mfId in serializableDictionary.Keys)
                {
                    decimal quantity = Math.Floor(serializableDictionary[mfId] * Math.Abs(totalCumQty) / 100.00M);
                    masterFundQuantity.Add(mfId, quantity);
                }
                decimal assignedQty = masterFundQuantity.Sum(x => x.Value);

                //assign remaining qty
                if (assignedQty < Math.Abs(totalCumQty))
                {
                    decimal remainingQty = Math.Abs(totalCumQty) - assignedQty;
                    int remainderId = int.MinValue;
                    if (int.TryParse(remainderQuantityId.ToString(), out remainderId) && remainderId != -1 && serializableDictionary.ContainsKey(remainderQuantityId))
                    {
                        if (masterFundQuantity.ContainsKey(remainderQuantityId))
                            masterFundQuantity[remainderQuantityId] += remainingQty;
                        else
                            masterFundQuantity.Add(remainderQuantityId, remainingQty);
                    }
                    else
                    {
                        decimal minAssignableQty = 1;
                        Dictionary<T, decimal> masterFundQtyRequired = GetNewAssignable<T>(totalCumQty, serializableDictionary, masterFundQuantity);
                        foreach (T mfId in masterFundQtyRequired.Keys)
                        {
                            if (remainingQty > 0)
                            {
                                decimal assignQtyToMF = masterFundQtyRequired[mfId];
                                if (assignQtyToMF < minAssignableQty)
                                    assignQtyToMF = minAssignableQty;
                                if (remainingQty < assignQtyToMF)
                                    assignQtyToMF = remainingQty;
                                masterFundQuantity[mfId] += assignQtyToMF;
                                remainingQty -= assignQtyToMF;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return masterFundQuantity;
        }

        /// <summary>
        /// Gets the new assignable.
        /// </summary>
        /// <param name="totalCumQty">The total cum qty.</param>
        /// <param name="pref">The preference.</param>
        /// <param name="masterFundQuantity">The master fund quantity.</param>
        /// <returns></returns>
        private static Dictionary<T, decimal> GetNewAssignable<T>(decimal totalCumQty, Dictionary<T, decimal> serializableDictionary, Dictionary<T, decimal> masterFundQuantity)
        {
            Dictionary<T, decimal> masterFundQtyRequired = new Dictionary<T, decimal>();
            try
            {
                foreach (T mfId in serializableDictionary.Keys)
                {
                    decimal totalValRequired = (Math.Abs(Convert.ToDecimal(totalCumQty)) * serializableDictionary[mfId]) / 100.00M;
                    decimal newRequired = totalValRequired - masterFundQuantity[mfId];
                    masterFundQtyRequired.Add(mfId, newRequired);
                }
                if (totalCumQty < 0)
                    masterFundQtyRequired = masterFundQtyRequired.OrderBy(x => x.Value).ToDictionary(t => t.Key, t => t.Value);
                else
                    masterFundQtyRequired = masterFundQtyRequired.OrderByDescending(x => x.Value).ToDictionary(t => t.Key, t => t.Value);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return masterFundQtyRequired;
        }

        /// <summary>
        /// Gets the order side wise virtual groups.
        /// </summary>
        /// <param name="orderSideSortedData">The order side sorted data.</param>
        /// <returns></returns>
        internal static List<AllocationGroup> GetOrderSideWiseVirtualGroups(Dictionary<string, List<AllocationGroup>> orderSideSortedData)
        {
            List<AllocationGroup> orderSideGroups = new List<AllocationGroup>();
            try
            {
                foreach (string orderSide in orderSideSortedData.Keys)
                {
                    AllocationGroup grp = orderSideSortedData[orderSide][0];
                    grp.OriginalGroupIDs = orderSideSortedData[orderSide].Select(g => g.GroupID).ToList();
                    double totalCumQty = orderSideSortedData[orderSide].Sum(x => x.CumQty);
                    double averagePrice = orderSideSortedData[orderSide].Average(x => x.AvgPrice);

                    AllocationGroup group = DeepCopyHelper.Clone(grp);
                    group.CumQty = Math.Abs(totalCumQty);
                    group.Quantity = Math.Abs(totalCumQty);
                    group.AvgPrice = Convert.ToDouble(averagePrice);
                    group.GroupID += Convert.ToInt32(group.OrderSideTagValue.ToCharArray()[0]);

                    AllocationOrder order = (AllocationOrder)group.Orders[0].Clone();
                    order.CumQty = Math.Abs(totalCumQty);
                    order.Quantity = Math.Abs(totalCumQty);
                    order.AvgPrice = Convert.ToDouble(averagePrice);
                    order.ClOrderID += Convert.ToInt32(order.OrderSideTagValue.ToCharArray()[0]);
                    order.ParentClOrderID += Convert.ToInt32(order.OrderSideTagValue.ToCharArray()[0]);
                    group.Orders.Clear();
                    group.AddOrder(order);

                    orderSideGroups.Add(group);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return orderSideGroups;
        }

        /// <summary>
        /// Gets the master fund wise virtual groups.
        /// </summary>
        /// <param name="masterFundPercentage">The master fund percentage.</param>
        /// <param name="grp">The group.</param>
        /// <param name="preferenceAccountId">The preference account identifier.</param>
        /// <returns></returns>
        internal static Dictionary<int, AllocationGroup> GetMasterFundWiseVirtualGroups(SerializableDictionary<int, AccountValue> masterFundPercentage, AllocationGroup grp, int preferenceAccountId)
        {
            Dictionary<int, AllocationGroup> mfVirtualGroup = new Dictionary<int, AllocationGroup>();
            try
            {
                Dictionary<int, decimal> masterFundWiseQuantity = GetQuantityForPercentage<int>(Convert.ToDecimal(grp.CumQty), masterFundPercentage.ToDictionary(t => t.Key, t => t.Value.Value), preferenceAccountId);
                foreach (int mfId in masterFundWiseQuantity.Keys)
                {
                    AllocationGroup group = DeepCopyHelper.Clone(grp);
                    group.CumQty = Math.Abs(Convert.ToDouble(masterFundWiseQuantity[mfId]));
                    group.Quantity = Math.Abs(Convert.ToDouble(masterFundWiseQuantity[mfId]));
                    group.GroupID += Convert.ToInt32(mfId);

                    AllocationOrder order = (AllocationOrder)group.Orders[0].Clone();
                    order.CumQty = Math.Abs(Convert.ToDouble(masterFundWiseQuantity[mfId]));
                    order.Quantity = Math.Abs(Convert.ToDouble(masterFundWiseQuantity[mfId]));
                    order.ClOrderID += Convert.ToInt32(mfId);
                    order.ParentClOrderID += Convert.ToInt32(mfId);
                    group.Orders.Clear();
                    group.AddOrder(order);

                    mfVirtualGroup.Add(mfId, group);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return mfVirtualGroup;
        }

        /// <summary>
        /// Updates the general rules in master fund preferences.
        /// </summary>
        /// <param name="masterFundPreferences">The master fund preferences dictionary.</param>
        internal static void UpdateMasterFundPreferences(ref Dictionary<int, AllocationOperationPreference> masterFundPreferences)
        {
            try
            {
                List<CheckListWisePreference> generalRulesList = masterFundPreferences.Values.ToList().SelectMany(x => x.CheckListWisePreference.Values.ToList()).Distinct().ToList();
                generalRulesList.ForEach(generalRule => generalRule.Rule.MatchClosingTransaction = MatchClosingTransactionType.None);
                masterFundPreferences.Values.ToList().ForEach(pref =>
                    {
                        AllocationRule rule = pref.DefaultRule;
                        rule.MatchClosingTransaction = MatchClosingTransactionType.None;
                        pref.TryUpdateDefaultRule(rule);
                        //add general rule with default rule and percentage if preference doesnt contain the general rule
                        generalRulesList.ForEach(generalRule =>
                        {
                            CheckListWisePreference checkPref = generalRule.Clone();
                            if (!pref.CheckListWisePreference.ContainsValue(generalRule))
                            {
                                checkPref.TryUpdateDefaultRule(pref.DefaultRule);
                                checkPref.TryUpdateTargetPercentage(pref.TargetPercentage);
                            }
                            pref.TryUpdateCheckList(checkPref, true);
                        });
                    });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the selected accounts list.
        /// </summary>
        /// <param name="masterFundPreferences">The master fund preferences.</param>
        /// <returns></returns>
        internal static List<int> GetSelectedAccountsList(List<int> masterFunds)
        {
            List<int> accounts = new List<int>();
            try
            {
                Dictionary<int, List<int>> masterFundAccountsDic = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                if (masterFundAccountsDic != null && masterFundAccountsDic.Count > 0 && masterFunds != null && masterFunds.Count > 0)
                {
                    masterFunds.ForEach(x =>
                        {
                            if (masterFundAccountsDic.ContainsKey(x) && masterFundAccountsDic[x] != null)
                                accounts.AddRange(masterFundAccountsDic[x]);
                        });
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return accounts;

        }

        /// <summary>
        /// Determines whether [is long position in selected groups] [the specified symbol wise group list].
        /// </summary>
        /// <param name="symbolWiseGroupList">The symbol wise group list.</param>
        /// <returns></returns>
        internal static bool IsLongPositionInSelectedGroups(List<AllocationGroup> symbolWiseGroupList)
        {
            try
            {
                return symbolWiseGroupList.Any(group => (group.OrderSideTagValue == FIXConstants.SIDE_Buy
                                        || group.OrderSideTagValue == FIXConstants.SIDE_Buy_Open
                                        || group.OrderSideTagValue == FIXConstants.SIDE_SellShort
                                        || group.OrderSideTagValue == FIXConstants.SIDE_Sell_Open));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Updates the preference for match closing.
        /// </summary>
        /// <param name="calculatedPref">The calculated preference.</param>
        /// <param name="remainingGroups">The remaining groups.</param>
        /// <returns></returns>
        internal static bool UpdatePreferenceForMatchClosing(ref AllocationOperationPreference calculatedPref, List<AllocationGroup> remainingGroups)
        {
            try
            {
                bool isLongPosition = remainingGroups.Any(group => (group.OrderSideTagValue == FIXConstants.SIDE_Buy || group.OrderSideTagValue == FIXConstants.SIDE_Buy_Open
                                                                 || group.OrderSideTagValue == FIXConstants.SIDE_SellShort || group.OrderSideTagValue == FIXConstants.SIDE_Sell_Open));
                CheckListWisePreference checkListPref = calculatedPref.CheckListWisePreference.Values.FirstOrDefault();
                if (!isLongPosition && checkListPref != null)
                {
                    //If preference does not have a default rule, use one of the general rules as default rule
                    if (calculatedPref.DefaultRule.BaseType != AllocationBaseType.CumQuantity && calculatedPref.DefaultRule.BaseType != AllocationBaseType.Notional)
                    {
                        calculatedPref.TryUpdateDefaultRule(checkListPref.Rule);
                        calculatedPref.TryUpdateTargetPercentage(checkListPref.TargetPercentage);
                        calculatedPref.TryUpdateAccountsList(checkListPref.GetAllocationAccountsList());
                    }
                    calculatedPref.UpdateCheckList(new SerializableDictionary<int, CheckListWisePreference>());
                    return true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }
    }
}
