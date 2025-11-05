// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 08-13-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-01-2014
// ***********************************************************************
// <copyright file="NotionalCalculator.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.LogManager;
using System;

/// <summary>
/// The FormulaStore namespace.
/// </summary>
namespace Prana.PostTrade.BLL.CostAdjustment
{
    /// <summary>
    /// Formula store for notional of allocation group
    /// </summary>
    internal static class NotionalCalculator
    {
        /// <summary>
        /// Returns the notional for given allocation group
        /// </summary>
        /// <param name="taxlot">AllocationGroup for which notional is required</param>
        /// <param name="quantity">Quantity for which notional is required</param>
        /// <returns>Notional of the given allocation group</returns>
        internal static decimal GetNotional(TaxLot taxlot, decimal quantity)
        {
            try
            {
                AssetCategory cat = (AssetCategory)Enum.Parse(typeof(AssetCategory), taxlot.AssetID.ToString());

                switch (cat)
                {
                    case AssetCategory.Equity:
                    case AssetCategory.EquityOption:
                    case AssetCategory.Future:
                    case AssetCategory.FutureOption:
                    case AssetCategory.CreditDefaultSwap:
                    case AssetCategory.PrivateEquity:
                        return (decimal)taxlot.AvgPrice * quantity * (decimal)taxlot.ContractMultiplier;
                    case AssetCategory.FixedIncome:
                    case AssetCategory.ConvertibleBond:
                        return (decimal)taxlot.AvgPrice * quantity * (decimal)taxlot.ContractMultiplier / 100;
                    default:
                        return decimal.Zero;

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
                return decimal.Zero;
            }
        }

        /// <summary>
        /// Returns the quantity by reverse calculating from given notional
        /// </summary>
        /// <param name="taxlot">AllocationGroup for which reverse notional calculation is required</param>
        /// <param name="notionalTobeReversed">Notional which to reversed to quantity</param>
        /// <returns>Quantity corresponding to notional given</returns>
        internal static decimal ReverseCalculateQuantity(TaxLot taxlot, decimal notionalTobeReversed)
        {
            try
            {
                AssetCategory cat = (AssetCategory)Enum.Parse(typeof(AssetCategory), taxlot.AssetID.ToString());
                decimal lotNotional = (decimal)taxlot.AvgPrice * taxlot.RoundLot;
                switch (cat)
                {
                    case AssetCategory.Equity:
                    case AssetCategory.EquityOption:
                    case AssetCategory.Future:
                    case AssetCategory.FutureOption:
                    case AssetCategory.CreditDefaultSwap:
                    case AssetCategory.PrivateEquity:
                        return (notionalTobeReversed * Calculations.GetSideMultilpier(taxlot.OrderSideTagValue)) / (lotNotional * (decimal)taxlot.ContractMultiplier);
                    case AssetCategory.FixedIncome:
                    case AssetCategory.ConvertibleBond:
                        return (notionalTobeReversed * Calculations.GetSideMultilpier(taxlot.OrderSideTagValue) * 100) / (lotNotional * (decimal)taxlot.ContractMultiplier);
                    default:
                        return decimal.Zero;

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
                return 0.0M;
            }
        }

        /// <summary>
        /// Returns the cash impact for given allocation group
        /// </summary>
        /// <param name="taxlot">AllocationGroup for which cash impact is required</param>
        /// <param name="quantity">Quantity for which cash impact is required</param>
        /// <returns>Cash impact of the given allocation group</returns>
        internal static decimal GetCashImpact(CostAdjustmentTaxlot taxlot)
        {
            try
            {
                return (taxlot.TotalCost - taxlot.NewTotalCost) * Calculations.GetSideMultilpier(taxlot.OrderSideTagValue);
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
                return decimal.Zero;
            }
        }

    }
}
