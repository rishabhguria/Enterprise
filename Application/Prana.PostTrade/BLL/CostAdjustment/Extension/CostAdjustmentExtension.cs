// ***********************************************************************
// Assembly         : Prana.PostTrade
// Author           : Shagoon.Gurtata
// Created          : 11-14-2014
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 12-11-2014
// ***********************************************************************
// <copyright file="CostAdjustmentExtension.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.PostTrade.BusinessObjects.CostAdjustment;
using System;

/// <summary>
/// The CostAdjustment namespace.
/// </summary>
namespace Prana.PostTrade.BLL.CostAdjustment
{
    /// <summary>
    /// Class CostAdjustmentExtension.
    /// </summary>
    internal static class CostAdjustmentExtension
    {
        /// <summary>
        /// Generates the new taxlot base and allocation group.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="taxlot">The taxlot.</param>
        /// <param name="isWithDrawal">if set to <c>true</c> [is with drawal].</param>
        /// <returns>TaxLot.</returns>
        internal static TaxLot GenerateNewTaxlotBaseAndAllocationGroup(this ICostAdjustmentGenerator generator, TaxLot taxlot, bool isWithDrawal)
        {
            TaxLot taxlotBaseNew = (TaxLot)taxlot.Clone();
            try
            {
                //CARulesHelper.FillDateInfo(taxlotBaseNew, corporateActionRow);

                taxlotBaseNew.PositionType = GetPositionKey(taxlotBaseNew.OrderSideTagValue).ToString();
                if (isWithDrawal)
                {
                    if (taxlotBaseNew.PositionType == PositionType.Short.ToString())
                    {
                        taxlotBaseNew.PositionTag = PositionTag.ShortCostAdj;// "8"; //short cost adjustment
                        taxlotBaseNew.OrderSideTagValue = FIXConstants.SIDE_Buy_Closed;
                        taxlotBaseNew.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotBaseNew.OrderSideTagValue.ToString());
                        taxlotBaseNew.TransactionType = TradingTransactionType.ShortCostAdj.ToString();
                    }
                    else
                    {
                        taxlotBaseNew.PositionTag = PositionTag.LongCostAdj;//"7"; //long cost adjustment
                        taxlotBaseNew.OrderSideTagValue = FIXConstants.SIDE_Sell;
                        taxlotBaseNew.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotBaseNew.OrderSideTagValue.ToString());
                        taxlotBaseNew.TransactionType = TradingTransactionType.LongCostAdj.ToString();
                    }
                }
                else
                {
                    if (taxlotBaseNew.PositionType == PositionType.Short.ToString())
                    {
                        taxlotBaseNew.PositionTag = PositionTag.ShortAddition;
                        taxlotBaseNew.TransactionType = TradingTransactionType.ShortAddition.ToString();
                    }
                    else
                    {
                        taxlotBaseNew.PositionTag = PositionTag.LongAddition;
                        taxlotBaseNew.TransactionType = TradingTransactionType.LongAddition.ToString();
                    }
                }

                //CARulesHelper.FillText(taxlotBaseNew);

                if (isWithDrawal)
                {
                    taxlotBaseNew.OriginalPurchaseDate = taxlot.OriginalPurchaseDate;
                    taxlotBaseNew.ProcessDate = taxlotBaseNew.OriginalPurchaseDate;
                }
                else
                {
                    taxlotBaseNew.OriginalPurchaseDate = taxlot.OriginalPurchaseDate;
                    taxlotBaseNew.ProcessDate = taxlotBaseNew.AUECLocalDate;
                }
                taxlotBaseNew.TransactionSource = TransactionSource.CostAdjustment;
                taxlotBaseNew.TransactionSourceTag = (int)TransactionSource.CostAdjustment;
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
            }

            return taxlotBaseNew;
        }

        /// <summary>
        /// Gets the position key.
        /// </summary>
        /// <param name="sideTagValue">The side tag value.</param>
        /// <returns>PositionType.</returns>
        private static PositionType GetPositionKey(string sideTagValue)
        {
            try
            {
                switch (sideTagValue)
                {
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Buy_Closed:
                    case FIXConstants.SIDE_Buy_Open:
                        return PositionType.Long;

                    case FIXConstants.SIDE_SellShort:
                    case FIXConstants.SIDE_Sell_Open:
                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_Sell_Closed:
                        return PositionType.Short;
                    default:
                        return PositionType.Long;
                }
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
                return PositionType.Long;
            }
        }

        /// <summary>
        /// Gets the quantity adjusted for taxlot.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="taxlot">The taxlot.</param>
        /// <returns>Adjusted quantity</returns>
        internal static decimal GetQuantityAdjustedForTaxlot(this ICostAdjustmentGenerator generator, decimal quantity, CostAdjustmentTaxlot taxlot)
        {
            try
            {
                // Removed condition because now we are getting absolute data for Total Open Quantity to Adjust and Quantity to Adjust
                // Now OrderSide Tag value is not required here
                //if (quantity >= taxlot.RemainingOpenQuantity * Calculations.GetSideMultilpier(taxlot.OrderSideTagValue))
                if (quantity >= taxlot.RemainingOpenQuantity)
                    return taxlot.RemainingOpenQuantity;
                else
                    return quantity;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return 0.0M;
            }
        }
    }
}
