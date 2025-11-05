// ***********************************************************************
// Assembly         : Prana.BusinessObjects
// Author           : Shagoon.Gurtata
// Created          : 11-12-2014
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 02-24-2015
// ***********************************************************************
// <copyright file="CostAdjustmentParameter.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Prana.BusinessObjects.CostAdjustment.Enums;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The Definitions namespace.
/// </summary>
namespace Prana.BusinessObjects.CostAdjustment.Definitions
{
    /// <summary>
    /// Class CostAdjustmentParameter.
    /// </summary>
    [Serializable]
    public class CostAdjustmentParameter
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the taxlots.
        /// </summary>
        /// <value>The taxlots.</value>
        public List<CostAdjustmentTaxlot> Taxlots { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public CostAdjustmentType Type { get; set; }

        /// <summary>
        /// Gets or sets the adjust cost.
        /// </summary>
        /// <value>The adjust cost.</value>
        public decimal AdjustCost { get; set; }

        /// <summary>
        /// Gets or sets the adjust qty.
        /// </summary>
        /// <value>The adjust qty.</value>
        public decimal AdjustQty { get; set; }

        /// <summary>
        /// Gets or sets the total quantity.
        /// </summary>
        /// <value>The total quantity.</value>
        public decimal TotalQuantity { get; set; }

        /// <summary>
        /// Gets or sets the total cost.
        /// </summary>
        /// <value>The total cost.</value>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is preview.
        /// </summary>
        /// <value><c>true</c> if this instance is preview; otherwise, <c>false</c>.</value>
        public bool isPreview { get; set; }

        /// <summary>
        /// Gets or sets the cost adjustment method.
        /// </summary>
        /// <value>The cost adjustment method.</value>
        public CostAdjustmentMethodology CostAdjustmentMethod { get; set; }

        /// <summary>
        /// Checks validity of a CostAdjustmentParameter
        /// </summary>
        /// <param name="errorMessage">returns error message string</param>
        /// <returns>true if valid, false otherwise</returns>
        public bool IsValid(out string errorMessage)
        {
            try
            {
                if (AdjustCost == 0)
                {
                    errorMessage = "Adjust Cost cannot be 0.";
                    return false;
                }
                else if (TotalCost + AdjustCost < 0)
                {
                    errorMessage = "Adjust Cost cannot make total cost less than zero.";
                    return false;
                }
                else if (AdjustQty == 0)
                {
                    errorMessage = "Adjust Quantity must be greater than 0.";
                    return false;
                }
                else if (AdjustQty > TotalQuantity)
                {
                    errorMessage = "Adjust Quantity should be less than or equal to Total Quantity.";
                    return false;
                }
                else if (Taxlots == null || Taxlots.Count <= 0)
                {
                    errorMessage = "Please select a taxlot for cost adjustment preview/save.";
                    return false;
                }
                else if (Taxlots != null && Taxlots.Count > 0)
                {
                    foreach (CostAdjustmentTaxlot taxlot in Taxlots)
                    {
                        if (Taxlots.Count(t => t.Symbol != taxlot.Symbol) > 0)
                        {
                            errorMessage = "Please select taxlots with same Symbols.";
                            return false;
                        }
                    }
                }
                errorMessage = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                errorMessage = string.Empty;
                return false;
            }

        }
    }
}