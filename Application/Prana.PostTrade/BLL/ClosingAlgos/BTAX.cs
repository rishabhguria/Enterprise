using Prana.BusinessObjects;
using System;
using System.Collections.Generic;

namespace Prana.PostTrade
{
    class BTAX : AlgoBase
    {

        private DateTime oneyearbackdate = DateTime.MinValue;

        private List<TaxLot> SortSellList(List<TaxLot> taxlots, PostTradeEnums.ClosingField closingField)
        {
            //sort on the basis of FIFO
            taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { return (t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date)); });
            //secondary sort Avg Price Ascending for same closing date
            switch (closingField)
            {
                case PostTradeEnums.ClosingField.AvgPrice:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    break;
                case PostTradeEnums.ClosingField.UnitCost:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.UnitCost.CompareTo(t2.UnitCost)); });
                    break;
                case PostTradeEnums.ClosingField.AvgPriceBase:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPriceBase.CompareTo(t2.AvgPriceBase)); });
                    break;
                case PostTradeEnums.ClosingField.UnitCostBase:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.UnitCostBase.CompareTo(t2.UnitCostBase)); });
                    break;
                case PostTradeEnums.ClosingField.Default:
                default:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    break;
            }
            //third sort LIFO for same avg price
            //taxlots.Sort(delegate(TaxLot t1, TaxLot t2) { int result = (t1.AvgPrice.CompareTo(t2.AvgPrice)); return (result != 0) ? result : (t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date))*(-1); });
            return taxlots;
        }


        private List<TaxLot> SortBuyList(List<TaxLot> taxlots, PostTradeEnums.ClosingField closingField)
        {
            //sort on the basis of FIFO
            taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { return (t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date)); });
            //secondary sort Avg Price Descending for same closing date
            switch (closingField)
            {
                case PostTradeEnums.ClosingField.AvgPrice:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)) * (-1); });
                    break;
                case PostTradeEnums.ClosingField.UnitCost:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.UnitCost.CompareTo(t2.UnitCost)) * (-1); });
                    break;
                case PostTradeEnums.ClosingField.AvgPriceBase:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPriceBase.CompareTo(t2.AvgPriceBase)) * (-1); });
                    break;
                case PostTradeEnums.ClosingField.UnitCostBase:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.UnitCostBase.CompareTo(t2.UnitCostBase)) * (-1); });
                    break;
                case PostTradeEnums.ClosingField.Default:
                default:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)) * (-1); });
                    break;
            }
            //third sort LIFO for same avg price
            //taxlots.Sort(delegate(TaxLot t1, TaxLot t2) { int result = (t1.AvgPrice.CompareTo(t2.AvgPrice)); return (result != 0) ? result : (t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date))*(-1); });
            return taxlots;

            #region commented
            //List<TaxLot> ListSortedTrades = new List<TaxLot>();

            //List<TaxLot> ListShortTermTrades = new List<TaxLot>();
            //List<TaxLot> ListLongTermTrades = new List<TaxLot>();

            //TaxLot[] TaxlotsClone = new TaxLot[taxlots.Count];
            //taxlots.CopyTo(TaxlotsClone, 0);

            //// Here we are adding those taxlots which satisfy the Long Term citerion in a new list
            //foreach (TaxLot sortedItem in TaxlotsClone)
            //{
            //    if (sortedItem.ClosingDate.Date <= oneyearbackdate)
            //    {
            //        ListLongTermTrades.Add(sortedItem);
            //        taxlots.Remove(sortedItem);
            //    }
            //    else
            //    {
            //        ListShortTermTrades.Add(sortedItem);
            //        taxlots.Remove(sortedItem);
            //    }
            //}
            ////For long term taxlots 
            ////Primary Sort: Descending Avg Price
            //ListLongTermTrades.Sort(delegate(TaxLot t1, TaxLot t2) { return (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
            ////Secondary Sort: Descending Closing Date
            //ListLongTermTrades.Sort(delegate(TaxLot t1, TaxLot t2) { int result = (t1.AvgPrice.CompareTo(t2.AvgPrice)); return (result != 0) ? result : (t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date)) * (-1); });


            ////For Short Term Taxlots 
            ////Primary Sort: Descending Avg Price
            //ListShortTermTrades.Sort(delegate(TaxLot t1, TaxLot t2) { return (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
            ////Secondary Sort: Descending Closing Date
            //ListLongTermTrades.Sort(delegate(TaxLot t1, TaxLot t2) { int result = (t1.AvgPrice.CompareTo(t2.AvgPrice)); return (result != 0) ? result : (t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date)) * (-1); });

            //ListSortedTrades.AddRange(ListShortTermTrades);
            //ListSortedTrades.AddRange(ListLongTermTrades);

            ////Descending sort on the basis of Avg Price
            //ListSortedTrades.Sort(delegate(TaxLot t1, TaxLot t2) { return (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });

            //return ListSortedTrades;
            #endregion

        }


        public override List<TaxLot> GetTaxLotsByDate(DateTime date, List<TaxLot> listOftaxlots)
        {
            oneyearbackdate = date.AddDays(-1).AddYears(-1);
            return base.GetTaxLotsByDate(date, listOftaxlots);
        }


        public override void Sort(ref List<TaxLot> BuyTaxlots, ref List<TaxLot> sellTaxlots, PostTradeEnums.SecondarySortCriteria sortCriteria, PostTradeEnums.ClosingField closingField)
        {
            sellTaxlots = SortSellList(sellTaxlots, closingField);
            BuyTaxlots = SortBuyList(BuyTaxlots, closingField);
        }

        //public override void DynamicSort(ref List<TaxLot> BuyTaxlots, ref List<TaxLot> sellTaxlots, PostTradeEnums.SecondarySortCriteria sortCriteria)
        //{
        //    sellTaxlots = SortSellList(sellTaxlots);
        //    BuyTaxlots = SortBuyList(BuyTaxlots, sellTaxlots);
        //}

        /// <summary>
        /// Narendra Kumar Jangir June 20, 2013
        /// Best TaxLot calculation steps
        /// 
        /// For Long Security
        /// 1. Short Term Loss (High loss to Low loss)
        /// 2. Long Term Loss (High loss to Low loss)
        /// 3. Short Term No Gain or Loss
        /// 4. Long Term No Gain or Loss
        /// 5. Long Term Gain (Low gain to High gain)
        /// 6. Short Term Gain (Low gain to High gain)
        /// 
        /// For Short Security
        /// 1. Short Term Loss (High loss to Low loss)
        /// 2. Short Term No Gain or Loss
        /// 3. Short Term Gain (Low gain to High gain)
        /// 
        /// for same gain/loss criteria follow LIFO
        /// </summary>
        /// <param name="closeTaxLot"></param>
        /// <param name="openTaxLotsAndPositions"></param>
        /// <returns></returns>
        public override void SecondarySort(TaxLot closeTaxLot, ref List<TaxLot> openTaxLotsAndPositions, ClosingPreferences closingPreferences, PostTradeEnums.SecondarySortCriteria secondarySortCriteria, PostTradeEnums.ClosingField closingField)
        {

            //calculate one year back date to identify short term and long term taxlots
            DateTime oneyearbackdate = closeTaxLot.ClosingDate.Date.AddYears(-1);
            //TaxLot positionalTaxlot1 = null;
            //TaxLot closingTaxlot1 = null;
            //TaxLot positionalTaxlot2 = null;
            //TaxLot closingTaxlot2 = null;
            //PositionTag PositionTag1 = PositionTag.Long;
            //PositionTag PositionTag2 = PositionTag.Long;
            //int sideMultiplier1 = 1;
            //int sideMultiplier2 = 1;
            openTaxLotsAndPositions.Sort(delegate (TaxLot t1, TaxLot t2)
            {
                //string positionType = string.Empty;

                //DeterminePositionalandClosingTaxlots(closeTaxLot, t1, out positionalTaxlot1, out closingTaxlot1);
                //DeterminePositionalandClosingTaxlots(closeTaxLot, t2, out positionalTaxlot2, out closingTaxlot2);

                //PositionTag1 = DeterminePositionTag(positionalTaxlot1);
                //PositionTag2 = DeterminePositionTag(positionalTaxlot2);

                //if (PositionTag1.Equals(PositionTag.Short))
                //    sideMultiplier1 = -1;
                //if (PositionTag2.Equals(PositionTag.Short))
                //    sideMultiplier2 = -1;

                //double UnitPNL1 = (closingTaxlot1.UnitCost - positionalTaxlot1.UnitCost) * sideMultiplier1;
                //double UnitPNL2 = (closingTaxlot2.UnitCost - positionalTaxlot2.UnitCost) * sideMultiplier2;
                double UnitPNL1 = (closeTaxLot.UnitCost - t1.UnitCost);
                double UnitPNL2 = (closeTaxLot.UnitCost - t2.UnitCost);
                switch (closingField)
                {
                    case PostTradeEnums.ClosingField.AvgPrice:
                        UnitPNL1 = (closeTaxLot.AvgPrice - t1.AvgPrice);
                        UnitPNL2 = (closeTaxLot.AvgPrice - t2.AvgPrice);
                        break;
                    case PostTradeEnums.ClosingField.UnitCost:
                        UnitPNL1 = (closeTaxLot.UnitCost - t1.UnitCost);
                        UnitPNL2 = (closeTaxLot.UnitCost - t2.UnitCost);
                        break;
                    case PostTradeEnums.ClosingField.AvgPriceBase:
                        UnitPNL1 = (closeTaxLot.AvgPriceBase - t1.AvgPriceBase);
                        UnitPNL2 = (closeTaxLot.AvgPriceBase - t2.AvgPriceBase);
                        break;
                    case PostTradeEnums.ClosingField.UnitCostBase:
                        UnitPNL1 = (closeTaxLot.UnitCostBase - t1.UnitCostBase);
                        UnitPNL2 = (closeTaxLot.UnitCostBase - t2.UnitCostBase);
                        break;
                    case PostTradeEnums.ClosingField.Default:
                    default:
                        UnitPNL1 = (closeTaxLot.UnitCost - t1.UnitCost);
                        UnitPNL2 = (closeTaxLot.UnitCost - t2.UnitCost);
                        break;
                }


                int result = UnitPNL1.CompareTo(UnitPNL2);
                //for handling of short trades
                if (closeTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover) || closeTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || closeTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy))
                {
                    return result * (-1);
                }
                //loss for one taxlot and gain/no gain no loss for another taxlot than taxlot with loss will be given priority
                if (!Math.Sign(UnitPNL1).Equals(Math.Sign(UnitPNL2)))
                {
                    if (UnitPNL1 < 0)
                        return -1;
                    if (UnitPNL2 < 0)
                        return 1;
                    else
                        return result;
                }
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3416
                //taxlot t1 is long term and taxlot t2 is short term
                if (t1.ClosingDate.Date < oneyearbackdate && t2.ClosingDate.Date >= oneyearbackdate)
                {
                    //loss for both taxlot, short term taxlot will be given priority, return t2
                    if (UnitPNL2 < 0 && UnitPNL1 < 0)
                        return 1;
                    //gain for both taxlot, long term taxlot will be given priority, return t1
                    else if (UnitPNL2 > 0 && UnitPNL1 > 0)
                        return -1;
                    //no gain no loss for both taxlot, short term taxlot will be given priority
                    else if (UnitPNL2 == 0 && UnitPNL1 == 0)
                        return 1;
                    //loss for one taxlot and gain for another taxlot, taxlot having loss will be given priority
                    else
                        return result;
                }
                //taxlot t1 is short term and taxlot t2 is long term
                else if (t1.ClosingDate.Date >= oneyearbackdate && t2.ClosingDate.Date < oneyearbackdate)
                {
                    //loss for both taxlot, short term taxlot will be given priority, return t2
                    if (UnitPNL2 < 0 && UnitPNL1 < 0)
                        return -1;
                    //gain for both taxlot, long term taxlot will be given priority, return t1
                    else if (UnitPNL2 > 0 && UnitPNL1 > 0)
                        return 1;
                    //no gain no loss for both taxlot, short term taxlot will be given priority
                    else if (UnitPNL2 == 0 && UnitPNL1 == 0)
                        return -1;
                    //loss for one taxlot and gain for another taxlot, taxlot having loss will be given priority
                    else
                        return result;
                }
                //either both taxlot are long term or both taxlot are short term
                //same gain/loss for both taxlots, t1 and t2 have diffrent closing date follow LIFO
                else if (result == 0 && !(t1.ClosingDate.Date.Equals(t2.ClosingDate.Date)))
                {
                    return t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date) * (-1);
                }
                //either both taxlot are long term or both taxlot are short term
                //same gain/loss for both taxlots, t1 and t2 have same closing date follow secondary sort criteria descending order sequence
                else if (result == 0 && t1.ClosingDate.Date.Equals(t2.ClosingDate.Date))
                {
                    return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID) * (-1));
                }
                else
                    return result;
            });
        }

        //private void DeterminePositionalandClosingTaxlots(TaxLot targetTaxLot, TaxLot draggedTaxLot, out TaxLot positionalTaxlot, out TaxLot closingTaxlot)
        //{
        //    DateTime targetTaxlotDate = targetTaxLot.ClosingDate.Date;
        //    DateTime draggedTaxLotDate = draggedTaxLot.ClosingDate.Date;

        //    positionalTaxlot = targetTaxLot;
        //    closingTaxlot = draggedTaxLot;

        //    if (targetTaxlotDate.Equals(draggedTaxLotDate))
        //    {
        //        // if the dates are equal then we need to consider the orderSide for determining the positional and closing Taxlots...
        //        switch (targetTaxLot.OrderSideTagValue)
        //        {


        //            case FIXConstants.SIDE_Buy_Closed:
        //            case FIXConstants.SIDE_Buy_Cover:
        //            case FIXConstants.SIDE_Sell:
        //            case FIXConstants.SIDE_Sell_Closed:

        //                switch (draggedTaxLot.OrderSideTagValue)
        //                {
        //                    case FIXConstants.SIDE_Buy:
        //                    case FIXConstants.SIDE_Buy_Open:
        //                    case FIXConstants.SIDE_SellShort:
        //                    case FIXConstants.SIDE_Sell_Open:


        //                        positionalTaxlot = draggedTaxLot;
        //                        closingTaxlot = targetTaxLot;
        //                        break;


        //                    default:
        //                        break;
        //                }
        //                break;

        //            case FIXConstants.SIDE_Buy:
        //            case FIXConstants.SIDE_Buy_Open:
        //            case FIXConstants.SIDE_SellShort:
        //            case FIXConstants.SIDE_Sell_Open:

        //                switch (draggedTaxLot.OrderSideTagValue)
        //                {
        //                    case FIXConstants.SIDE_Buy_Closed:
        //                    case FIXConstants.SIDE_Buy_Cover:
        //                    case FIXConstants.SIDE_Sell:
        //                    case FIXConstants.SIDE_Sell_Closed:


        //                        positionalTaxlot = targetTaxLot;
        //                        closingTaxlot = draggedTaxLot;
        //                        break;

        //                    default:
        //                        break;
        //                }
        //                break;

        //        }
        //    }
        //    else
        //    {
        //        if (targetTaxlotDate > draggedTaxLotDate)
        //        {
        //            positionalTaxlot = draggedTaxLot;
        //            closingTaxlot = targetTaxLot;
        //        }
        //    }

        //}

        //private PositionTag DeterminePositionTag(TaxLot positionalTaxlot)
        //{
        //    switch (positionalTaxlot.OrderSideTagValue)
        //    {
        //        case FIXConstants.SIDE_Buy:
        //        case FIXConstants.SIDE_Buy_Open:
        //        case FIXConstants.SIDE_Buy_Closed:
        //        case FIXConstants.SIDE_Buy_Cover:
        //            return PositionTag.Long;
        //            break;

        //        case FIXConstants.SIDE_SellShort:
        //        case FIXConstants.SIDE_Sell_Open:
        //        case FIXConstants.SIDE_Sell:
        //        case FIXConstants.SIDE_Sell_Closed:
        //            return PositionTag.Short;
        //            break;
        //    }
        //    return positionalTaxlot.PositionTag;
        //}
    }
}