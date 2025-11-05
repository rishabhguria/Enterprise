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
using Prana.LogManager;
using System;

/// <summary>
/// The FormulaStore namespace.
/// </summary>
namespace Prana.Allocation.Core.FormulaStore
{
    /// <summary>
    /// Formula store for notional of allocation group
    /// </summary>
    internal static class NotionalCalculator
    {
        /// <summary>
        /// Returns the notional for given allocation group
        /// </summary>
        /// <param name="group">AllocationGroup for which notional is required</param>
        /// <param name="quantity">Quantity for which notional is required</param>
        /// <returns>Notional of the given allocation group</returns>
        internal static decimal GetNotional(AllocationGroup group, decimal quantity)
        {
            try
            {
                AssetCategory cat = (AssetCategory)Enum.Parse(typeof(AssetCategory), group.AssetID.ToString());
                decimal contractMultiplier = Double.IsNaN(group.ContractMultiplier) ? decimal.Zero : (decimal)group.ContractMultiplier;
                switch (cat)
                {
                    //Using multiplier for calculating notional
                    case AssetCategory.Equity:
                    case AssetCategory.EquityOption:
                    case AssetCategory.Future:
                    case AssetCategory.FutureOption:
                    case AssetCategory.CreditDefaultSwap:
                    case AssetCategory.PrivateEquity:
                        return (decimal)group.AvgPrice * quantity * Calculations.GetSideMultilpier(group.OrderSideTagValue) * contractMultiplier;
                    case AssetCategory.FixedIncome:
                    case AssetCategory.ConvertibleBond:
                        return (decimal)group.AvgPrice * quantity * Calculations.GetSideMultilpier(group.OrderSideTagValue) * contractMultiplier / 100;
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
        /// <param name="group">AllocationGroup for which reverse notional calculation is required</param>
        /// <param name="notionalTobeReversed">Notional which to reversed to quantity</param>
        /// <returns>Quantity corresponding to notional given</returns>
        internal static decimal ReverseCalculateQuantity(AllocationGroup group, decimal notionalTobeReversed)
        {
            try
            {
                AssetCategory cat = (AssetCategory)Enum.Parse(typeof(AssetCategory), group.AssetID.ToString());
                decimal lotNotional = (decimal)group.AvgPrice;
                switch (cat)
                {
                    //Using multiplier for calculating reverse quantity
                    case AssetCategory.Equity:
                    case AssetCategory.EquityOption:
                    case AssetCategory.Future:
                    case AssetCategory.FutureOption:
                    case AssetCategory.CreditDefaultSwap:
                    case AssetCategory.PrivateEquity:
                        return (notionalTobeReversed * Calculations.GetSideMultilpier(group.OrderSideTagValue)) / (lotNotional * (decimal)group.ContractMultiplier);
                    case AssetCategory.FixedIncome:
                    case AssetCategory.ConvertibleBond:
                        return (notionalTobeReversed * Calculations.GetSideMultilpier(group.OrderSideTagValue) * 100) / (lotNotional * (decimal)group.ContractMultiplier);
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



    }
}
