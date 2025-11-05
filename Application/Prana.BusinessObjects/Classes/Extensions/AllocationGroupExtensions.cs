// ***********************************************************************
// Assembly         : Prana.BusinessObjects
// Author           : Disha Sharma
// Created          : 08-08-2016
// ***********************************************************************
// <copyright file="AllocationGroupExtensions.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// This is extension class for "AllocationGroup.cs"
    /// TODO: There should be only one extension class for allocation group. Merge AllocationGroupExtension.cs from Prana.Allocation.Core to this class   
    /// </summary>
    public static class AllocationGroupExtensions
    {

        /// <summary>
        /// Updates the non empty trade attributes for group and generate audit trail.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="tradeAttributes">The trade attributes.</param>
        public static void UpdateNonEmptyTradeAttributes(this AllocationGroup group, TradeAttributes tradeAttributes)
        {
            try
            {
                List<TradeAuditActionType.ActionType> tradeActions = new List<TradeAuditActionType.ActionType>();
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute1) && !tradeAttributes.TradeAttribute1.Equals(group.TradeAttribute1))
                {
                    group.TradeAttribute1 = tradeAttributes.TradeAttribute1;
                    tradeActions.Add(TradeAuditActionType.ActionType.TradeAttribute1_Changed);
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute2) && !tradeAttributes.TradeAttribute2.Equals(group.TradeAttribute2))
                {
                    group.TradeAttribute2 = tradeAttributes.TradeAttribute2;
                    tradeActions.Add(TradeAuditActionType.ActionType.TradeAttribute2_Changed);
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute3) && !tradeAttributes.TradeAttribute3.Equals(group.TradeAttribute3))
                {
                    group.TradeAttribute3 = tradeAttributes.TradeAttribute3;
                    tradeActions.Add(TradeAuditActionType.ActionType.TradeAttribute3_Changed);
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute4) && !tradeAttributes.TradeAttribute4.Equals(group.TradeAttribute4))
                {
                    group.TradeAttribute4 = tradeAttributes.TradeAttribute4;
                    tradeActions.Add(TradeAuditActionType.ActionType.TradeAttribute4_Changed);
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute5) && !tradeAttributes.TradeAttribute5.Equals(group.TradeAttribute5))
                {
                    group.TradeAttribute5 = tradeAttributes.TradeAttribute5;
                    tradeActions.Add(TradeAuditActionType.ActionType.TradeAttribute5_Changed);
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute6) && !tradeAttributes.TradeAttribute6.Equals(group.TradeAttribute6))
                {
                    group.TradeAttribute6 = tradeAttributes.TradeAttribute6;
                    tradeActions.Add(TradeAuditActionType.ActionType.TradeAttribute6_Changed);
                }
                foreach (var kvp in tradeAttributes.GetTradeAttributesAsDict())
                {
                    if (!string.IsNullOrWhiteSpace(kvp.Value) && !kvp.Value.Equals(group.GetTradeAttributeValue(kvp.Key)))
                    {
                        group.SetTradeAttributeValue(kvp.Key, kvp.Value);
                        string enumName = kvp.Key + "_Changed";
                        if (Enum.TryParse<TradeAuditActionType.ActionType>(enumName, out var actionEnum))
                        {
                            tradeActions.Add(actionEnum);
                        }

                    }
                }
                UpdateTaxlotNonEmptyTradeAttributes(group, tradeAttributes);
                group.AddTradeActionsToGroup(tradeActions);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the non empty trade attributes for taxlots
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="tradeAttributes">The trade attributes.</param>
        public static void UpdateTaxlotNonEmptyTradeAttributes(this AllocationGroup group, TradeAttributes tradeAttributes)
        {
            try
            {
                foreach (TaxLot taxlot in group.TaxLots)
                {
                    taxlot.UpdateNonEmptyTradeAttributes(tradeAttributes);
                    group.UpdateTaxlotState(taxlot);
                }
                if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                {
                    TaxLot updatedTaxlot = new TaxLot();
                    updatedTaxlot.TaxLotQty = group.CumQty;
                    updatedTaxlot.TaxLotID = group.GroupID;
                    updatedTaxlot.GroupID = group.GroupID;
                    updatedTaxlot.SideMultiplier = GetSideMultilpier(group.OrderSideTagValue);
                    updatedTaxlot.CopyBasicDetails((PranaBasicMessage)group);
                    group.UpdateTaxlotState(updatedTaxlot);
                    group.UpdateGroupOrder(group);
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
        /// Gets the side multiplier based on the side tag value.
        /// </summary>
        /// <param name="sidetagvalue"></param>
        /// <returns></returns>
        private static int GetSideMultilpier(string sidetagvalue)
        {
            int sideMul = 1;
            try
            {
                switch (sidetagvalue)
                {
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Buy_Open:
                    case FIXConstants.SIDE_Buy_Closed:
                        sideMul = 1;
                        break;

                    case FIXConstants.SIDE_Sell_Open:
                    case FIXConstants.SIDE_SellShort:
                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_Sell_Closed:
                        sideMul = -1;
                        break;

                    default:
                        sideMul = 1;
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return sideMul;
        }

        /// <summary>
        /// Updates the order non empty trade attributes for ungrouped trades.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="tradeAttributes">The trade attributes.</param>
        public static void UpdateOrderNonEmptyTradeAttributes(this AllocationGroup group, TradeAttributes tradeAttributes)
        {
            try
            {
                if (group.Orders.Count == 1)
                    group.Orders[0].UpdateNonEmptyTradeAttributes(tradeAttributes);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the trade actions to group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="tradeActions">The trade actions.</param>
        public static void AddTradeActionsToGroup(this AllocationGroup group, List<TradeAuditActionType.ActionType> tradeActions)
        {
            try
            {
                group.TradeActionsList.AddRange(tradeActions.Where(x => !(group.TradeActionsList.Contains(x))).ToList());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the trade actions to taxlots.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="tradeActions">The trade actions.</param>
        public static void AddTradeActionsToTaxlots(this AllocationGroup group, List<TradeAuditActionType.ActionType> tradeActions)
        {
            try
            {
                group.TaxLots.ForEach(taxlot =>
                {
                    taxlot.AddTradeActions(tradeActions);
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
        /// Update Group Accrued Interest
        /// </summary>
        /// <param name="group"></param>
        public static void UpdateGroupAccruedInterest(this AllocationGroup group)
        {
            try
            {
                double accruedInterest = 0.0;
                group.TaxLots.ForEach(childTaxlot =>
                {
                    accruedInterest += childTaxlot.AccruedInterest;
                });

                group.AccruedInterest = accruedInterest;
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
