using Prana.BusinessObjects;
using System;
using System.Collections.Generic;

namespace Prana.PostTrade
{
    class MFIFO : AlgoBase
    {

        private DateTime sameTradeDate = DateTime.MinValue;

        private List<TaxLot> SortBuyList(List<TaxLot> taxlots, PostTradeEnums.SecondarySortCriteria sortCriteria)
        {

            List<TaxLot> listsortedTaxlots = new List<TaxLot>();

            TaxLot[] TaxlotsClone = new TaxLot[taxlots.Count];
            taxlots.CopyTo(TaxlotsClone, 0);


            foreach (TaxLot sortedItem in TaxlotsClone)
            {
                //closing date and trade date are same
                if (sortedItem.ClosingDate.Date == sameTradeDate)
                {
                    listsortedTaxlots.Add(sortedItem);
                    taxlots.Remove(sortedItem);
                }
                //if closing date is gerater than trade date
                if (sortedItem.ClosingDate.Date > sameTradeDate)
                {
                    break;
                }
            }
            //taxlots which have close date and trade date equal should be closed first in the MFIFO
            if (listsortedTaxlots.Count > 0)
                listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { return t1.ClosingDate.CompareTo(t2.ClosingDate); });

            if (taxlots.Count > 0)
            {
                taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { return t1.ClosingDate.CompareTo(t2.ClosingDate); });
            }
            switch (sortCriteria)
            {
                case PostTradeEnums.SecondarySortCriteria.AvgPxASC:
                    listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    break;
                case PostTradeEnums.SecondarySortCriteria.SamePxAvgPxASC:
                    listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    break;
                case PostTradeEnums.SecondarySortCriteria.AvgPxDESC:
                    listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
                    break;
                case PostTradeEnums.SecondarySortCriteria.SamePxAvgPxDESC:
                    listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
                    break;
                case PostTradeEnums.SecondarySortCriteria.OrderSequenceASC:
                    listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID)); });
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID)); });
                    break;
                case PostTradeEnums.SecondarySortCriteria.OrderSequenceDESC:
                    listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID) * (-1)); });
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID) * (-1)); });
                    break;

                case PostTradeEnums.SecondarySortCriteria.None:
                default:
                    break;
            }
            foreach (TaxLot sortedItem in taxlots)
            {
                listsortedTaxlots.Add(sortedItem);
            }
            //now list of taxlot type contains taxlots in the sorted order but the taxlots which have same trade date and no closing taxlot than there order in the list will be at top.

            return listsortedTaxlots;
        }

        private List<TaxLot> SortSellList(List<TaxLot> taxlots, PostTradeEnums.SecondarySortCriteria sortCriteria)
        {

            List<TaxLot> listsortedTaxlots = new List<TaxLot>();

            TaxLot[] TaxlotsClone = new TaxLot[taxlots.Count];
            taxlots.CopyTo(TaxlotsClone, 0);

            foreach (TaxLot sortedItem in TaxlotsClone)
            {
                if (sortedItem.ClosingDate.Date == sameTradeDate)
                {
                    listsortedTaxlots.Add(sortedItem);
                    taxlots.Remove(sortedItem);
                }
                if (sortedItem.ClosingDate.Date > sameTradeDate)
                {
                    break;
                }
            }

            if (listsortedTaxlots.Count > 0)
                listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { return t1.ClosingDate.CompareTo(t2.ClosingDate); });

            if (taxlots.Count > 0)
            {
                taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { return t1.ClosingDate.CompareTo(t2.ClosingDate); });
            }
            switch (sortCriteria)
            {
                case PostTradeEnums.SecondarySortCriteria.AvgPxASC:
                    listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    break;
                case PostTradeEnums.SecondarySortCriteria.SamePxAvgPxASC:
                    listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    break;
                case PostTradeEnums.SecondarySortCriteria.AvgPxDESC:
                    listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
                    break;
                case PostTradeEnums.SecondarySortCriteria.SamePxAvgPxDESC:
                    listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
                    break;
                case PostTradeEnums.SecondarySortCriteria.OrderSequenceASC:
                    listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID)); });
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID)); });
                    break;
                case PostTradeEnums.SecondarySortCriteria.OrderSequenceDESC:
                    listsortedTaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID) * (-1)); });
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID) * (-1)); });
                    break;
                case PostTradeEnums.SecondarySortCriteria.None:
                default:
                    break;
            }
            foreach (TaxLot sortedItem in taxlots)
            {
                listsortedTaxlots.Add(sortedItem);
            }

            return listsortedTaxlots;
        }

        public override List<TaxLot> GetTaxLotsByDate(DateTime date, List<TaxLot> listOftaxlots)
        {
            sameTradeDate = date;
            return base.GetTaxLotsByDate(date, listOftaxlots);
        }

        public override void Sort(ref List<TaxLot> BuyTaxlots, ref List<TaxLot> sellTaxlots, PostTradeEnums.SecondarySortCriteria sortCriteria, PostTradeEnums.ClosingField closingField)
        {
            BuyTaxlots = SortBuyList(BuyTaxlots, sortCriteria);
            sellTaxlots = SortSellList(sellTaxlots, sortCriteria);
        }

        public override void SecondarySort(TaxLot closeTaxLot, ref List<TaxLot> openTaxLotsAndPositions, ClosingPreferences closingPreferences, PostTradeEnums.SecondarySortCriteria secondarySortCriteria, PostTradeEnums.ClosingField closingField)
        {
            //if (secondarySortCriteria.Equals(PostTradeEnums.SecondarySortCriteria.SamePxAvgPxASC))
            //{
            //    openTaxLotsAndPositions.Sort(delegate(TaxLot t1, TaxLot t2)
            //    {
            //        //for MFIFO same day closing will be followed by seconday sory criteria SamePxAvgPxASC
            //        //closing for other dates will be followed by AVGPXASC
            //        if ((t1.ClosingDate.Date.Equals(t2.ClosingDate.Date)) && (t1.ClosingDate.Date.Equals(closeTaxLot.ClosingDate.Date)))
            //        {
            //            if (t1.AvgPrice.Equals(closeTaxLot.AvgPrice))
            //                return -1;
            //            else if (t2.AvgPrice.Equals(closeTaxLot.AvgPrice))
            //                return 1;
            //            else
            //                return t1.AvgPrice.CompareTo(t2.AvgPrice);
            //        }
            //        else
            //            return t1.AvgPrice.CompareTo(t2.AvgPrice);
            //    });
            //}
            //else if (secondarySortCriteria.Equals(PostTradeEnums.SecondarySortCriteria.SamePxAvgPxDESC))
            //{
            //    openTaxLotsAndPositions.Sort(delegate(TaxLot t1, TaxLot t2)
            //    {
            //        //for MFIFO same day closing will be followed by seconday sory criteria SamePxAvgPxDESC
            //        //closing for other dates will be followed by AVGPXDESC
            //        if ((t1.ClosingDate.Date.Equals(t2.ClosingDate.Date)) && (t1.ClosingDate.Date.Equals(closeTaxLot.ClosingDate.Date)))
            //        {
            //            if (t1.AvgPrice.Equals(closeTaxLot.AvgPrice))
            //                return -1;
            //            else if (t2.AvgPrice.Equals(closeTaxLot.AvgPrice))
            //                return 1;
            //            else
            //                return t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1);
            //        }
            //        else
            //            return t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1);
            //    });
            //}
        }
    }
}
