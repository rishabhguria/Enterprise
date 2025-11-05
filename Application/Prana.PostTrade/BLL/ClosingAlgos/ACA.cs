using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.PostTrade
{
    class ACA : AlgoBase
    {
        /// <summary>
        /// Sort Buy List
        /// </summary>
        /// <param name="taxlots"></param>
        /// <param name="sortCriteria"></param>
        /// <returns></returns>
        private List<TaxLot> SortBuyList(List<TaxLot> taxlots, PostTradeEnums.SecondarySortCriteria sortCriteria)
        {
            taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { return t1.ClosingDate.CompareTo(t2.ClosingDate); });

            //Once the taxlots are sorted in ASC order on closing date then secondary sorting will be applied on the same date taxlots by secondary criteria either AveragePrice or TaxlotID....
            switch (sortCriteria)
            {

                case PostTradeEnums.SecondarySortCriteria.OrderSequenceASC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID)); });
                    break;



                case PostTradeEnums.SecondarySortCriteria.None:
                default:
                    break;
            }
            return taxlots;
        }

        /// <summary>
        ///  Sort Sell List
        /// </summary>
        /// <param name="taxlots"></param>
        /// <param name="sortCriteria"></param>
        /// <returns></returns>
        private List<TaxLot> SortSellList(List<TaxLot> taxlots, PostTradeEnums.SecondarySortCriteria sortCriteria)
        {
            taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { return t1.ClosingDate.CompareTo(t2.ClosingDate); });
            //Once the taxlots are sorted in ASC order on closing date then secondary sorting will be applied on the same date taxlots by secondary criteria either AveragePrice or TaxlotID....
            switch (sortCriteria)
            {

                case PostTradeEnums.SecondarySortCriteria.OrderSequenceASC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID)); });
                    break;



                case PostTradeEnums.SecondarySortCriteria.None:
                default:
                    break;
            }
            return taxlots;
        }

        public override void Sort(ref List<TaxLot> BuyTaxlots, ref List<TaxLot> sellTaxlots, PostTradeEnums.SecondarySortCriteria sortCriteria, PostTradeEnums.ClosingField closingField)
        {
            BuyTaxlots = SortBuyList(BuyTaxlots, sortCriteria);
            sellTaxlots = SortSellList(sellTaxlots, sortCriteria);

            // Here we are copying the ACA Price in the opening Taxlots based on the date of the closing transaction.

            //if (BuyTaxlots[0].ClosingDate < sellTaxlots[0].ClosingDate)
            //{
            //    foreach (TaxLot taxlot in sellTaxlots)
            //    {
            //        if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell))
            //        {
            //            ACAManager.ApplyACA(BuyTaxlots, taxlot.ClosingDate.Date);
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (TaxLot taxlot in BuyTaxlots)
            //    {
            //        if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed))
            //        {
            //            ACAManager.ApplyACA(sellTaxlots, taxlot.ClosingDate.Date);
            //            break;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Apply secondary sort on open taxlots for closing 
        /// </summary>
        /// <param name="closeTaxLot"></param>
        /// <param name="openTaxLotsAndPositions"></param>
        /// <param name="closingPreferences"></param>
        /// <param name="secondarySortCriteria"></param>
        public override void SecondarySort(TaxLot closeTaxLot, ref List<TaxLot> openTaxLotsAndPositions, ClosingPreferences closingPreferences, PostTradeEnums.SecondarySortCriteria secondarySortCriteria, PostTradeEnums.ClosingField closingField)
        {
            try
            {
                //if (closingPreferences.AcaMode.Equals(PostTradeEnums.ACAMode.FractionalClosing))
                //{
                List<TaxLot> EligibleTaxlots = new List<TaxLot>();

                if (secondarySortCriteria.Equals(PostTradeEnums.SecondarySortCriteria.OrderSequenceASC))
                {
                    //if secondarySortCriteria is OrderSequenceASC then get taxlots with sequence of  TaxLotID
                    long taxlotIdCloseTrade = Convert.ToInt64(closeTaxLot.TaxLotID);
                    foreach (TaxLot tax in openTaxLotsAndPositions)
                    {
                        long taxlotId = Convert.ToInt64(tax.TaxLotID);
                        if ((tax.ClosingDate.Date < closeTaxLot.ClosingDate.Date) || ((tax.ClosingDate.Date == closeTaxLot.ClosingDate.Date) && taxlotId < taxlotIdCloseTrade))
                            EligibleTaxlots.Add(tax);
                    }

                    if (EligibleTaxlots.Count == 0)
                    {
                        EligibleTaxlots = openTaxLotsAndPositions;
                    }
                }
                else if (secondarySortCriteria.Equals(PostTradeEnums.SecondarySortCriteria.None))
                {
                    EligibleTaxlots = openTaxLotsAndPositions;
                }

                ApplyProportionalClosing(closeTaxLot, EligibleTaxlots);
                //}
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
        }


        /// <summary>
        /// Apply Proportional Closing
        /// </summary>
        /// <param name="closeTaxLot">closeTaxLot</param>
        /// <param name="openTaxLotsAndPositions">openTaxLotsAndPositions</param>
        private static void ApplyProportionalClosing(TaxLot closeTaxLot, List<TaxLot> openTaxLotsAndPositions)
        {
            try
            {
                //calculate total open positional quantity
                double OpenPosition = openTaxLotsAndPositions.Sum(d => d.TaxLotQty);
                double taxLotQtyToCloseModifiedClosingTaxlot = 0;
                //Calculate tax lot qty 
                foreach (TaxLot taxLot in openTaxLotsAndPositions)
                {
                    //TODO - Need to clean up when we convert data type from double to decimal in taxlot- omshiv
                    // while calculating proportional closing quantity for taxlot, there was a precision loss on working with double
                    //so here we are using decimal data type.
                    Decimal taxLotQty = (decimal)taxLot.TaxLotQty;
                    Decimal openPositions = (decimal)OpenPosition;
                    Decimal closeQty = (decimal)closeTaxLot.TaxLotQty;
                    Decimal taxlotPct = (taxLotQty * 100 / openPositions);
                    Decimal taxLotQtyToClose = (taxlotPct * closeQty) / 100;

                    //help link- http://stackoverflow.com/questions/1584314/conversion-of-a-decimal-to-double-number-in-c-sharp-results-in-a-difference
                    const decimal PreciseOne = 1.000000000000000000000000000000000000000000000000M;
                    double taxLotQtyToCloseModifiedOpeningTaxlot = (double)((taxLotQtyToClose / PreciseOne));
                    taxLot.TaxLotQtyToClose = taxLotQtyToCloseModifiedOpeningTaxlot;
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-5299
                    if (taxLotQtyToCloseModifiedOpeningTaxlot > taxLot.TaxLotQty)
                    {
                        taxLot.TaxLotQtyToClose = taxLot.TaxLotQty;
                    }
                    taxLotQtyToCloseModifiedClosingTaxlot += taxLot.TaxLotQtyToClose;
                }
                closeTaxLot.TaxLotQtyToClose = taxLotQtyToCloseModifiedClosingTaxlot;
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
        }
    }
}
