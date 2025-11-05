using Prana.LogManager;
// ***********************************************************************
// Assembly         : Prana.BusinessObjects
// Author           : Disha.Sharma
// Created          : 04-09-2015
//
// Last Modified By : Disha.Sharma
// Last Modified On : 04-09-2015
// ***********************************************************************
// <copyright file="CostAdjustmentTaxlot.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

/// <summary>
/// The Definitions namespace.
/// </summary>
namespace Prana.BusinessObjects.CostAdjustment.Definitions
{
    /// <summary>
    /// Class CostAdjustmentTaxlotsForSave
    /// </summary>
    [Serializable]
    public class CostAdjustmentTaxlotsForSave
    {
        /// <summary>
        /// Gets or sets the CAID
        /// </summary>
        public String CAID { get; set; }

        /// <summary>
        /// Gets or sets the TaxlotID
        /// </summary>
        public String TaxlotID { get; set; }

        /// <summary>
        /// Gets or sets the GroupID
        /// </summary>
        public String GroupID { get; set; }

        /// <summary>
        /// Gets or sets the ClosingTaxlotID
        /// </summary>
        public String ClosingTaxlotID { get; set; }

        /// <summary>
        /// Gets or sets the ClosingUniqueID
        /// </summary>
        public String ClosingUniqueID { get; set; }

        /// <summary>
        /// Gets or sets the ParentTaxlot_PK
        /// </summary>
        public long ParentTaxlot_PK { get; set; }

        /// <summary>
        /// Gets or sets the Taxlot_PK
        /// </summary>
        public long Taxlot_PK { get; set; }

        /// <summary>
        /// Gets or sets the ClosingDate
        /// </summary>
        public DateTime ClosingDate { get; set; }

        /// <summary>
        /// Gets CostAdjustmentTaxlotsForSave from taxlot
        /// </summary>
        /// <param name="taxlot">The taxlot</param>
        /// <param name="CostAdjustmentID">The unique Guid</param>
        /// <returns>Return the CostAdjustmentTaxlotsForSave</returns>
        public static CostAdjustmentTaxlotsForSave GetTaxlot(TaxLot taxlot, String CostAdjustmentID)
        {
            try
            {
                return new CostAdjustmentTaxlotsForSave()
                {
                    CAID = CostAdjustmentID,
                    TaxlotID = taxlot.TaxLotID,
                    GroupID = taxlot.GroupID,
                    ClosingTaxlotID = taxlot.ClosingWithTaxlotID,
                    ClosingUniqueID = taxlot.TaxLotClosingId,
                    ParentTaxlot_PK = taxlot.ParentRowPk,
                    Taxlot_PK = taxlot.TaxlotPk,
                    ClosingDate = taxlot.AUECLocalDate
                };
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }
    }
}
