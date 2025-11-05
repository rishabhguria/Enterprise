using Prana.BusinessObjects;
using System;
using System.Collections.Generic;

namespace Prana.PostTrade
{
    class ETM : AlgoBase
    {

        private DateTime oneyearbackdate = DateTime.MinValue;

        private List<TaxLot> SortSellList(List<TaxLot> taxlots, PostTradeEnums.ClosingField closingField)
        {


            List<TaxLot> ListSortedTrades = new List<TaxLot>();

            TaxLot[] TaxlotsClone = new TaxLot[taxlots.Count];
            taxlots.CopyTo(TaxlotsClone, 0);

            // Here we are adding those taxlots which satisfy the Long Term citerion in a new list
            foreach (TaxLot sortedItem in TaxlotsClone)
            {
                if (sortedItem.ClosingDate.Date <= oneyearbackdate)
                {
                    ListSortedTrades.Add(sortedItem);
                    taxlots.Remove(sortedItem);
                }
                else
                {
                    break;
                }
            }
            if (ListSortedTrades.Count > 0) //we have to sort all the taxlots by HIFO 
            {
                //sort in Ascending Order based on AveragePrice 
                switch (closingField)
                {
                    case PostTradeEnums.ClosingField.AvgPrice:
                        ListSortedTrades.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.AvgPrice.CompareTo(t2.AvgPrice)); return (result != 0) ? result : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.UnitCost:
                        ListSortedTrades.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.UnitCost.CompareTo(t2.UnitCost)); return (result != 0) ? result : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.AvgPriceBase:
                        ListSortedTrades.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.AvgPriceBase.CompareTo(t2.AvgPriceBase)); return (result != 0) ? result : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.UnitCostBase:
                        ListSortedTrades.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.UnitCostBase.CompareTo(t2.UnitCostBase)); return (result != 0) ? result : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.Default:
                    default:
                        ListSortedTrades.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.AvgPrice.CompareTo(t2.AvgPrice)); return (result != 0) ? result : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                }
            }

            if (taxlots.Count > 0) // sorting by HIFO
            {
                switch (closingField)
                {
                    case PostTradeEnums.ClosingField.AvgPrice:
                        taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.AvgPrice.CompareTo(t2.AvgPrice)); return (result != 0) ? result : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.UnitCost:
                        taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.UnitCost.CompareTo(t2.UnitCost)); return (result != 0) ? result : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.AvgPriceBase:
                        taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.AvgPriceBase.CompareTo(t2.AvgPriceBase)); return (result != 0) ? result : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.UnitCostBase:
                        taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.UnitCostBase.CompareTo(t2.UnitCostBase)); return (result != 0) ? result : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.Default:
                    default:
                        taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.AvgPrice.CompareTo(t2.AvgPrice)); return (result != 0) ? result : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                }
                //Now merging both the lists (long Term as well as normal taxlots) to get the final sorted list
                foreach (TaxLot sortedItem in taxlots)
                {
                    ListSortedTrades.Add(sortedItem);
                }
            }

            return ListSortedTrades;
        }


        private List<TaxLot> SortBuyList(List<TaxLot> taxlots, PostTradeEnums.ClosingField closingField)
        {
            List<TaxLot> ListSortedTrades = new List<TaxLot>();

            TaxLot[] TaxlotsClone = new TaxLot[taxlots.Count];
            taxlots.CopyTo(TaxlotsClone, 0);

            // Here we are adding those taxlots which satisfy the Long Term citerion in a new list
            foreach (TaxLot sortedItem in TaxlotsClone)
            {
                if (sortedItem.ClosingDate.Date <= oneyearbackdate)
                {
                    ListSortedTrades.Add(sortedItem);
                    taxlots.Remove(sortedItem);
                }
                else
                {
                    break;
                }
            }
            if (ListSortedTrades.Count > 0)
            {
                //sort in descending Order based on AveragePrice
                switch (closingField)
                {
                    case PostTradeEnums.ClosingField.AvgPrice:
                        ListSortedTrades.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.AvgPrice.CompareTo(t2.AvgPrice)); return (result != 0) ? result * (-1) : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.UnitCost:
                        ListSortedTrades.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.UnitCost.CompareTo(t2.UnitCost)); return (result != 0) ? result * (-1) : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.AvgPriceBase:
                        ListSortedTrades.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.AvgPriceBase.CompareTo(t2.AvgPriceBase)); return (result != 0) ? result * (-1) : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.UnitCostBase:
                        ListSortedTrades.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.UnitCostBase.CompareTo(t2.UnitCostBase)); return (result != 0) ? result * (-1) : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.Default:
                    default:
                        ListSortedTrades.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.AvgPrice.CompareTo(t2.AvgPrice)); return (result != 0) ? result * (-1) : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                }
            }
            if (taxlots.Count > 0)
            {
                switch (closingField)
                {
                    case PostTradeEnums.ClosingField.AvgPrice:
                        taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.AvgPrice.CompareTo(t2.AvgPrice)); return (result != 0) ? result * (-1) : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.UnitCost:
                        taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.UnitCost.CompareTo(t2.UnitCost)); return (result != 0) ? result * (-1) : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.AvgPriceBase:
                        taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.AvgPriceBase.CompareTo(t2.AvgPriceBase)); return (result != 0) ? result * (-1) : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.UnitCostBase:
                        taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.UnitCostBase.CompareTo(t2.UnitCostBase)); return (result != 0) ? result * (-1) : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                    case PostTradeEnums.ClosingField.Default:
                    default:
                        taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = (t1.AvgPrice.CompareTo(t2.AvgPrice)); return (result != 0) ? result * (-1) : (t1.ClosingDate.CompareTo(t2.ClosingDate)); });
                        break;
                }

                //Now merging both the lists (long Term as well as normal taxlots) to get the final sorted list
                foreach (TaxLot sortedItem in taxlots)
                {
                    ListSortedTrades.Add(sortedItem);
                }
            }
            return ListSortedTrades;
        }


        public override List<TaxLot> GetTaxLotsByDate(DateTime date, List<TaxLot> listOftaxlots)
        {
            oneyearbackdate = date.AddDays(-1).AddYears(-1);
            return base.GetTaxLotsByDate(date, listOftaxlots);
        }


        public override void Sort(ref List<TaxLot> BuyTaxlots, ref List<TaxLot> sellTaxlots, PostTradeEnums.SecondarySortCriteria sortCriteria, PostTradeEnums.ClosingField closingField)
        {
            BuyTaxlots = SortBuyList(BuyTaxlots, closingField);
            sellTaxlots = SortSellList(sellTaxlots, closingField);
        }

        public override void SecondarySort(TaxLot closeTaxLot, ref List<TaxLot> openTaxLotsAndPositions, ClosingPreferences closingPreferences, PostTradeEnums.SecondarySortCriteria secondarySortCriteria, PostTradeEnums.ClosingField closingField)
        {
        }
    }


}