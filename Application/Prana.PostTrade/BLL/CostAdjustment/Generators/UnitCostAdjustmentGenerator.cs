// ***********************************************************************
// Assembly         : Prana.PostTrade
// Author           : Shagoon.Gurtata
// Created          : 11-14-2014
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 12-12-2014
// ***********************************************************************
// <copyright file="UnitCostAdjustmentGenerator.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.BusinessObjects.CostAdjustment.Enums;
using Prana.LogManager;
using Prana.PostTrade.BusinessObjects.CostAdjustment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The CostAdjustment namespace.
/// </summary>
namespace Prana.PostTrade.BLL.CostAdjustment
{
    /// <summary>
    /// Class UnitCostAdjustmentGenerator.
    /// </summary>
    internal class UnitCostAdjustmentGenerator : ICostAdjustmentGenerator
    {
        #region ICostAdjutmentGenerator Members

        /// <summary>
        /// Adjusts the cost.
        /// </summary>
        /// <param name="taxlotList">The taxlot list.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>CostAdjustmentResult.</returns>
        public CostAdjustmentResult AdjustCost(List<TaxLot> taxlotList, List<CostAdjustmentParameter> parameterList)
        {
            try
            {
                // Done changes for changing CostAdjustmentParameter to list of CostAdjustmentParameter in input parameter
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7227
                CostAdjustmentResult result = new CostAdjustmentResult();

                //  Locker object
                object resultLockerObject = new object();

                //Added Parallel.ForEach to do cost adjustment on each parameter in parallel manner
                Parallel.ForEach(parameterList, parameter =>
                {
                    decimal averagePrice = parameter.AdjustCost;
                    decimal quantityToAdjust = parameter.AdjustQty;

                    List<CostAdjustmentTaxlot> adjustedTaxlotsList = new List<CostAdjustmentTaxlot>();
                    List<TaxLot> newWithdrawlTaxlotsList = new List<TaxLot>();
                    List<TaxLot> newAdditionTaxlotsList = new List<TaxLot>();

                    foreach (CostAdjustmentTaxlot taxlot in parameter.Taxlots)
                    {
                        //Adjust cost till quantity to adjust is not zero.
                        if (quantityToAdjust != 0)
                        {
                            //Getting quantity of taxlot that need to be adjusted.
                            decimal quantityAdjusted = this.GetQuantityAdjustedForTaxlot(quantityToAdjust, taxlot);

                            //Getting taxlot for costAdjustmentTaxlot
                            TaxLot selectedTaxlot = (taxlotList.Single(a => a.TaxLotID == taxlot.TaxlotId));

                            //Generating withdrawl taxlot for taxlot
                            TaxLot newWithdrawlTaxlot = this.GenerateNewTaxlotBaseAndAllocationGroup(selectedTaxlot, true);

                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7378
                            // Updated ParentRowPK of taxlot
                            newWithdrawlTaxlot.ParentRowPk = selectedTaxlot.TaxlotPk;

                            // Updated Executed Quantity and Quantity of adjusted taxlot equal to open quantity of original taxlot
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7239
                            newWithdrawlTaxlot.ExecutedQty = Convert.ToDouble(taxlot.RemainingOpenQuantity);
                            newWithdrawlTaxlot.Quantity = Convert.ToDouble(taxlot.RemainingOpenQuantity);

                            //Getting costAdjustmentTaxlot for TAxlot
                            CostAdjustmentTaxlot withdrawlTaxlot = CostAdjustmentTaxlot.GetTaxlot(newWithdrawlTaxlot);
                            withdrawlTaxlot.TotalCost = NotionalCalculator.GetNotional(selectedTaxlot, taxlot.RemainingOpenQuantity);

                            //Setting closing taxlot Id to close original with close
                            selectedTaxlot.ClosingWithTaxlotID = newWithdrawlTaxlot.TaxLotID;
                            newWithdrawlTaxlot.ClosingWithTaxlotID = selectedTaxlot.TaxLotID;

                            adjustedTaxlotsList.Add(withdrawlTaxlot);
                            newWithdrawlTaxlotsList.Add(newWithdrawlTaxlot);

                            //Calculating new avg price based on quantity adjusted (weighted price)
                            decimal newAvgPrice = ((quantityAdjusted * averagePrice) + ((taxlot.RemainingOpenQuantity - quantityAdjusted) * Convert.ToDecimal(selectedTaxlot.AvgPrice))) / taxlot.RemainingOpenQuantity;
                            // this was updating avg price of original taxlot so commented it
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7375
                            //selectedTaxlot.AvgPrice = NumberPrecisionConstants.ToDoublePrecise(newAvgPrice);

                            //new taxlot with adjusted price.
                            TaxLot newAdditionTaxlot = this.GenerateNewTaxlotBaseAndAllocationGroup(selectedTaxlot, false);
                            // Updated average price of new taxlot
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7375
                            newAdditionTaxlot.AvgPrice = NumberPrecisionConstants.ToDoublePrecise(newAvgPrice);

                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7378
                            // Updated ParentRowPK of taxlot
                            newAdditionTaxlot.ParentRowPk = selectedTaxlot.TaxlotPk;

                            // Updated Executed Quantity and Quantity of adjusted taxlot equal to open quantity of original taxlot
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7239
                            newAdditionTaxlot.ExecutedQty = Convert.ToDouble(taxlot.RemainingOpenQuantity);
                            newAdditionTaxlot.Quantity = Convert.ToDouble(taxlot.RemainingOpenQuantity);

                            CostAdjustmentTaxlot additionTaxlot = CostAdjustmentTaxlot.GetTaxlot(newAdditionTaxlot);
                            additionTaxlot.NewTotalCost = NotionalCalculator.GetNotional(newAdditionTaxlot, taxlot.RemainingOpenQuantity);
                            additionTaxlot.NewUnitCost = newAvgPrice;
                            additionTaxlot.UnitCost = withdrawlTaxlot.UnitCost;
                            additionTaxlot.TotalCost = withdrawlTaxlot.TotalCost;
                            additionTaxlot.CashImpactOfAdjustment = NotionalCalculator.GetCashImpact(additionTaxlot);
                            adjustedTaxlotsList.Add(additionTaxlot);

                            newAdditionTaxlotsList.Add(newAdditionTaxlot);

                            //deducting quantity adjusted from quantity to adjust
                            // Removed condition because now we are getting absolute data for Total Open Quantity to Adjust and Quantity to Adjust
                            // Now OrderSide Tag value is not required here
                            //quantityToAdjust-= quantityAdjusted  * Calculations.GetSideMultilpier(taxlot.OrderSideTagValue));
                            quantityToAdjust -= quantityAdjusted;

                        }
                    }
                    lock (resultLockerObject)
                    {
                        //Added Adjusted and Original taxlots lists to final result
                        result.AdjustedTaxlots.AddRange(adjustedTaxlotsList);
                        result.AdjustedAdditionTaxlotList.AddRange(newAdditionTaxlotsList);
                        result.AdjustedWithdrawlTaxlotList.AddRange(newWithdrawlTaxlotsList);
                        result.OriginalTaxlotList.AddRange(taxlotList);
                    }
                });
                //CostAdjustmentResult result = new CostAdjustmentResult() 
                //{ 
                //    AdjustedTaxlots = adjustedTaxlotsList,
                //    AdjustedAdditionTaxlotList = newAdditionTaxlotsList,
                //    AdjustedWithdrawlTaxlotList = newWithdrawlTaxlotsList,
                //    OriginalTaxlotList = taxlotList 
                //};
                return result;
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
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public CostAdjustmentType Type
        {
            get { return CostAdjustmentType.Unit; }
        }

        #endregion
    }
}
