// ***********************************************************************
// Assembly         : Prana.BusinessObjects
// Author           : Shagoon.Gurtata
// Created          : 11-12-2014
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 12-11-2014
// ***********************************************************************
// <copyright file="CostAdjustmentTaxlot.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;

/// <summary>
/// The Definitions namespace.
/// </summary>
namespace Prana.BusinessObjects.CostAdjustment.Definitions
{
    /// <summary>
    /// Class CostAdjustmentTaxlot.
    /// </summary>
    [Serializable]
    public class CostAdjustmentTaxlot
    {

        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>The group identifier.</value>
        public String GroupId { get; set; }
        /// <summary>
        /// Gets or sets the taxlot identifier.
        /// </summary>
        /// <value>The taxlot identifier.</value>
        public String TaxlotId { get; set; }
        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        /// <value>The transaction date.</value>
        public DateTime TransactionDate { get; set; }
        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public String Symbol { get; set; }
        /// <summary>
        /// Gets or sets the order side.
        /// </summary>
        /// <value>The order side.</value>
        public String OrderSide { get; set; }
        /// <summary>
        /// Gets or sets the order quantity.
        /// </summary>
        /// <value>The order quantity.</value>
        public decimal OrderQuantity { get; set; }
        /// <summary>
        /// Gets or sets the remaining open quantity.
        /// </summary>
        /// <value>The remaining open quantity.</value>
        public decimal RemainingOpenQuantity { get; set; }
        // public double Price { get; set; }
        /// <summary>
        /// Gets or sets the total cost.
        /// </summary>
        /// <value>The total cost.</value>
        public decimal TotalCost { get; set; }
        /// <summary>
        /// Gets or sets the unit cost.
        /// </summary>
        /// <value>The unit cost.</value>
        public decimal UnitCost { get; set; }
        /// <summary>
        /// Gets or sets the new total cost.
        /// </summary>
        /// <value>The new total cost.</value>
        public decimal NewTotalCost { get; set; }
        /// <summary>
        /// Gets or sets the new unit cost.
        /// </summary>
        /// <value>The new unit cost.</value>
        public decimal NewUnitCost { get; set; }
        /// <summary>
        /// Gets or sets the cash impact of adjustment.
        /// </summary>
        /// <value>The cash impact of adjustment.</value>
        public decimal CashImpactOfAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the position tag.
        /// </summary>
        /// <value>The position tag.</value>
        public PositionTag PositionTag { get; set; }
        /// <summary>
        /// Gets or sets the order side tag value.
        /// </summary>
        /// <value>The order side tag value.</value>
        public string OrderSideTagValue { get; set; }
        /// <summary>
        /// Gets or sets the type of the transaction.
        /// </summary>
        /// <value>The type of the transaction.</value>
        public string TransactionType { get; set; }
        /// <summary>
        /// Gets or sets the Account Name.
        /// </summary>
        /// <value>The type of the transaction.</value>
        public string AccountName { get; set; }
        /// <summary>
        /// Gets the taxlot.
        /// </summary>
        /// <param name="taxlot">The taxlot.</param>
        /// <returns>CostAdjustmentTaxlot.</returns>
        public static CostAdjustmentTaxlot GetTaxlot(TaxLot taxlot)
        {
            try
            {
                return new CostAdjustmentTaxlot()
                {
                    GroupId = taxlot.GroupID,
                    TaxlotId = taxlot.TaxLotID,
                    TransactionDate = taxlot.AUECLocalDate,
                    Symbol = taxlot.Symbol,
                    OrderSide = taxlot.OrderSide,
                    OrderQuantity = Convert.ToDecimal(taxlot.ExecutedQty),
                    RemainingOpenQuantity = Convert.ToDecimal(taxlot.TaxLotQty),
                    UnitCost = Convert.ToDecimal(taxlot.AvgPrice),
                    //Modifying condition because taxlot.TotalCommissionandFees already subtracted and in case of getdata TotalCommissionandFees was coming 0. 
                    TotalCost = Convert.ToDecimal(taxlot.AvgPrice * taxlot.TaxLotQty * taxlot.ContractMultiplier),
                    NewUnitCost = 0,
                    NewTotalCost = 0,
                    CashImpactOfAdjustment = 0,
                    PositionTag = taxlot.PositionTag,
                    OrderSideTagValue = taxlot.OrderSideTagValue,
                    TransactionType = taxlot.TransactionType,
                    AccountName = taxlot.Level1Name
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

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CostAdjustmentTaxlot() { }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="taxlot">The CostAdjustmentTaxlot</param>
        public CostAdjustmentTaxlot(CostAdjustmentTaxlot taxlot)
        {
            try
            {
                this.GroupId = taxlot.GroupId;
                this.TaxlotId = taxlot.TaxlotId;
                this.TransactionDate = taxlot.TransactionDate;
                this.Symbol = taxlot.Symbol;
                this.OrderSide = taxlot.OrderSide;
                this.OrderQuantity = taxlot.OrderQuantity;
                this.RemainingOpenQuantity = taxlot.RemainingOpenQuantity;
                this.UnitCost = taxlot.UnitCost;
                this.TotalCost = taxlot.TotalCost;
                this.NewUnitCost = taxlot.NewUnitCost;
                this.NewTotalCost = taxlot.NewTotalCost;
                this.CashImpactOfAdjustment = taxlot.CashImpactOfAdjustment;
                this.PositionTag = taxlot.PositionTag;
                this.OrderSideTagValue = taxlot.OrderSideTagValue;
                this.TransactionType = taxlot.TransactionType;
                this.AccountName = taxlot.AccountName;
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
