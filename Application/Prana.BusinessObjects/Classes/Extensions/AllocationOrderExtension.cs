// ***********************************************************************
// Assembly         : Prana.BusinessObjects
// Author           : Disha Sharma
// Created          : 08-08-2016
// ***********************************************************************
// <copyright file="AllocationOrderExtension.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.LogManager;
using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// This is extension for "AllocationOrder.cs" class
    /// </summary>
    public static class AllocationOrderExtension
    {
        /// <summary>
        /// Updates the non empty trade attributes in order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="tradeAttributes">The trade attributes.</param>
        public static void UpdateNonEmptyTradeAttributes(this AllocationOrder order, TradeAttributes tradeAttributes)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute1))
                {
                    order.TradeAttribute1 = tradeAttributes.TradeAttribute1;
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute2))
                {
                    order.TradeAttribute2 = tradeAttributes.TradeAttribute2;
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute3))
                {
                    order.TradeAttribute3 = tradeAttributes.TradeAttribute3;
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute4))
                {
                    order.TradeAttribute4 = tradeAttributes.TradeAttribute4;
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute5))
                {
                    order.TradeAttribute5 = tradeAttributes.TradeAttribute5;
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute6))
                {
                    order.TradeAttribute6 = tradeAttributes.TradeAttribute6;
                }
                order.SetTradeAttribute(tradeAttributes.GetTradeAttributesAsJson());
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}
