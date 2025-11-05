// ***********************************************************************
// Assembly         : Prana.BusinessObjects
// Author           : Disha Sharma
// Created          : 08-08-2016
// ***********************************************************************
// <copyright file="TaxlotExtension.cs" company="Nirvana">
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
    /// This is extension for "TaxLot.cs" class
    /// </summary>
    public static class TaxlotExtension
    {
        /// <summary>
        /// Updates the non empty trade attributes in a taxlot.
        /// </summary>
        /// <param name="taxlot">The taxlot.</param>
        /// <param name="tradeAttributes">The trade attributes.</param>
        public static void UpdateNonEmptyTradeAttributes(this TaxLot taxlot, TradeAttributes tradeAttributes)
        {
            try
            {
                List<TradeAuditActionType.ActionType> tradeActions = new List<TradeAuditActionType.ActionType>();
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute1) && !tradeAttributes.TradeAttribute1.Equals(taxlot.TradeAttribute1))
                {
                    taxlot.TradeAttribute1 = tradeAttributes.TradeAttribute1;
                    tradeActions.Add(TradeAuditActionType.ActionType.TradeAttribute1_Changed);
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute2) && !tradeAttributes.TradeAttribute2.Equals(taxlot.TradeAttribute2))
                {
                    taxlot.TradeAttribute2 = tradeAttributes.TradeAttribute2;
                    tradeActions.Add(TradeAuditActionType.ActionType.TradeAttribute2_Changed);
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute3) && !tradeAttributes.TradeAttribute3.Equals(taxlot.TradeAttribute3))
                {
                    taxlot.TradeAttribute3 = tradeAttributes.TradeAttribute3;
                    tradeActions.Add(TradeAuditActionType.ActionType.TradeAttribute3_Changed);
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute4) && !tradeAttributes.TradeAttribute4.Equals(taxlot.TradeAttribute4))
                {
                    taxlot.TradeAttribute4 = tradeAttributes.TradeAttribute4;
                    tradeActions.Add(TradeAuditActionType.ActionType.TradeAttribute4_Changed);
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute5) && !tradeAttributes.TradeAttribute5.Equals(taxlot.TradeAttribute5))
                {
                    taxlot.TradeAttribute5 = tradeAttributes.TradeAttribute5;
                    tradeActions.Add(TradeAuditActionType.ActionType.TradeAttribute5_Changed);
                }
                if (!string.IsNullOrWhiteSpace(tradeAttributes.TradeAttribute6) && !tradeAttributes.TradeAttribute6.Equals(taxlot.TradeAttribute6))
                {
                    taxlot.TradeAttribute6 = tradeAttributes.TradeAttribute6;
                    tradeActions.Add(TradeAuditActionType.ActionType.TradeAttribute6_Changed);
                }

                foreach (var kvp in tradeAttributes.GetTradeAttributesAsDict())
                {
                    if (!string.IsNullOrWhiteSpace(kvp.Value) && !kvp.Value.Equals(taxlot.GetTradeAttributeValue(kvp.Key)))
                    {
                        taxlot.SetTradeAttributeValue(kvp.Key, kvp.Value);
                        string enumName = kvp.Key + "_Changed";
                        if (Enum.TryParse<TradeAuditActionType.ActionType>(enumName, out var actionEnum))
                        {
                            tradeActions.Add(actionEnum);
                        }

                    }                  
                }
                taxlot.AddTradeActions(tradeActions);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the trade actions list.
        /// </summary>
        /// <param name="taxlot">The taxlot.</param>
        /// <param name="tradeActions">The trade actions list.</param>
        public static void AddTradeActions(this TaxLot taxlot, List<TradeAuditActionType.ActionType> tradeActions)
        {
            try
            {
                taxlot.TradeActionsList.AddRange(tradeActions.Where(x => !(taxlot.TradeActionsList.Contains(x))).ToList());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}
