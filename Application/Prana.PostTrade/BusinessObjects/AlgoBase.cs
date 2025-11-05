using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public abstract class AlgoBase
    {
        //public abstract List<TaxLot> SortBuyList(List<TaxLot> taxlots);

        //public abstract List<TaxLot> SortSellList(List<TaxLot> taxlots);

        public abstract void Sort(ref List<TaxLot> BuyTaxlots, ref List<TaxLot> sellTaxlots, PostTradeEnums.SecondarySortCriteria secondarySortCriteria, PostTradeEnums.ClosingField closingField);

        public abstract void SecondarySort(TaxLot closingTaxLot, ref List<TaxLot> positionalTaxlots, ClosingPreferences closingPreferences, PostTradeEnums.SecondarySortCriteria secondarySortCriteria, PostTradeEnums.ClosingField closingField);

        public virtual List<TaxLot> GetTaxLotsByDate(DateTime date, List<TaxLot> listOftaxlots)
        {
            List<TaxLot> taxlotList = new List<TaxLot>();
            listOftaxlots.Sort(delegate (TaxLot t1, TaxLot t2) { return t1.ClosingDate.CompareTo(t2.ClosingDate); });

            foreach (TaxLot sortedItem in listOftaxlots)
            {
                if (sortedItem.ClosingDate.Date <= date)
                {
                    taxlotList.Add(sortedItem);
                }
                else
                {
                    break;
                }
            }
            return taxlotList;
        }

        public virtual List<List<TaxLot>> GetSellClosingTaxLots(List<TaxLot> listOftaxlots)
        {
            List<List<TaxLot>> taxlotList = new List<List<TaxLot>>();

            List<TaxLot> longTaxLotList = new List<TaxLot>();
            List<TaxLot> shortTaxLotList = new List<TaxLot>();

            foreach (TaxLot taxLot in listOftaxlots)
            {
                //taxlots with side sell, SellToClose are long taxlots
                if (taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                {
                    longTaxLotList.Add(taxLot);
                }
                //taxlots with remaining sides are short taxlots
                else
                {
                    shortTaxLotList.Add(taxLot);
                }
            }
            taxlotList.Add(longTaxLotList);
            taxlotList.Add(shortTaxLotList);
            return taxlotList;
        }

        public virtual List<List<TaxLot>> GetBuyClosingTaxLots(List<TaxLot> listOftaxlots)
        {
            List<List<TaxLot>> taxlotList = new List<List<TaxLot>>();

            List<TaxLot> longTaxLotList = new List<TaxLot>();
            List<TaxLot> shortTaxLotList = new List<TaxLot>();

            foreach (TaxLot taxLot in listOftaxlots)
            {
                //taxlots with side buy, BuyToOpen are long taxlots
                if (taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open))
                {
                    longTaxLotList.Add(taxLot);
                }
                //taxlots with remaining sides are short taxlots
                else
                {
                    shortTaxLotList.Add(taxLot);
                }
            }
            taxlotList.Add(longTaxLotList);
            taxlotList.Add(shortTaxLotList);
            return taxlotList;
        }
    }
}
