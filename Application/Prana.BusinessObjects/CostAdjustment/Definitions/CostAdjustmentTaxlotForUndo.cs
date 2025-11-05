// ***********************************************************************
// Assembly         : Prana.BusinessObjects
// Author           : Disha.Sharma
// Created          : 04-30-2015
//
// Last Modified By : Disha.Sharma
// Last Modified On : 04-30-2015
// ***********************************************************************
// <copyright file="CostAdjustmentTaxlot.cs" company="Microsoft">
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
    /// Class CostAdjustmentTaxlotForUndo
    /// </summary>
    [Serializable]
    public class CostAdjustmentTaxlotForUndo : CostAdjustmentTaxlot
    {
        /// <summary>
        /// Gets or sets the CAID
        /// </summary>
        public String CAID { get; set; }

        /// <summary>
        /// Cost Adjustment Taxlot List
        /// </summary>
        public List<CostAdjustmentTaxlot> CATaxlotList { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CostAdjustmentTaxlotForUndo() : base() { }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="caId"></param>
        /// <param name="taxlot"></param>
        /// <param name="taxlots"></param>
        public CostAdjustmentTaxlotForUndo(string caId, CostAdjustmentTaxlot taxlot, List<CostAdjustmentTaxlot> taxlots)
            : base(taxlot)
        {
            try
            {
                this.CAID = caId;
                this.CATaxlotList = taxlots;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

    }
}
