// ***********************************************************************
// Assembly         : Prana.BusinessObjects
// Author           : Shagoon.Gurtata
// Created          : 11-13-2014
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 12-12-2014
// ***********************************************************************
// <copyright file="CostAdjustmentResult.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Prana.LogManager;
using System;
using System.Collections.Generic;

/// <summary>
/// The Definitions namespace.
/// </summary>
namespace Prana.BusinessObjects.CostAdjustment.Definitions
{
    /// <summary>
    /// Class CostAdjustmentResult.
    /// </summary>
    [Serializable]
    public class CostAdjustmentResult
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the adjusted addition taxlot list.
        /// </summary>
        /// <value>The adjusted addition taxlot list.</value>
        public List<TaxLot> AdjustedAdditionTaxlotList { get; set; }

        /// <summary>
        /// Gets or sets the adjusted taxlots.
        /// </summary>
        /// <value>The adjusted taxlots.</value>
        public List<CostAdjustmentTaxlot> AdjustedTaxlots { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public String Error { get; set; }

        /// <summary>
        /// Gets or sets the adjusted withdrawl taxlot list.
        /// </summary>
        /// <value>The adjusted withdrawl taxlot list.</value>
        public List<TaxLot> AdjustedWithdrawlTaxlotList { get; set; }

        /// <summary>
        /// Gets or sets the original taxlot list.
        /// </summary>
        /// <value>The original taxlot list.</value>
        public List<TaxLot> OriginalTaxlotList { get; set; }

        /// <summary>
        /// Gets or sets the CostAdjustmentTaxlotsForSave taxlot list.
        /// </summary>
        /// <value>The CostAdjustmentTaxlotsForSave taxlot list.</value>
        public List<CostAdjustmentTaxlotsForSave> SavedTaxlotsData { get; set; }

        /// <summary>
        /// Adds cost adjustment result
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public void Merge(CostAdjustmentResult result)
        {
            try
            {
                if (result != null)
                {
                    this.AdjustedAdditionTaxlotList.AddRange(result.AdjustedAdditionTaxlotList);
                    this.AdjustedTaxlots.AddRange(result.AdjustedTaxlots);
                    this.AdjustedWithdrawlTaxlotList.AddRange(result.AdjustedWithdrawlTaxlotList);
                    this.Error += result.Error;
                    this.OriginalTaxlotList.AddRange(result.OriginalTaxlotList);
                    this.UserId = result.UserId;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Constructor of class
        /// </summary>
        public CostAdjustmentResult()
        {
            try
            {
                this.AdjustedAdditionTaxlotList = new List<TaxLot>();
                this.AdjustedTaxlots = new List<CostAdjustmentTaxlot>();
                this.AdjustedWithdrawlTaxlotList = new List<TaxLot>();
                this.Error = string.Empty;
                this.OriginalTaxlotList = new List<TaxLot>();
                this.UserId = int.MinValue;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

    }
}