using Prana.BusinessObjects;
using System.Collections.Generic;

namespace Prana.PostTrade
{
    class FIFO : AlgoBase
    {
        private List<TaxLot> SortBuyList(List<TaxLot> taxlots, PostTradeEnums.SecondarySortCriteria sortCriteria)
        {
            //primary sorting is done based on the core logic of the Algo for eg for FIFO taxlots will first be sorted(ASC) on closing date basis..

            taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { return t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); });

            //Once the taxlots are sorted in ASC order on closing date then secondary sorting will be applied on the same date taxlots by secondary criteria either AveragePrice or TaxlotID....
            switch (sortCriteria)
            {
                case PostTradeEnums.SecondarySortCriteria.AvgPxASC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    break;

                case PostTradeEnums.SecondarySortCriteria.AvgPxDESC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
                    break;

                case PostTradeEnums.SecondarySortCriteria.SamePxAvgPxASC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    break;

                case PostTradeEnums.SecondarySortCriteria.SamePxAvgPxDESC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
                    break;

                case PostTradeEnums.SecondarySortCriteria.OrderSequenceASC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID)); });
                    break;

                case PostTradeEnums.SecondarySortCriteria.OrderSequenceDESC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID) * (-1)); });
                    break;

                case PostTradeEnums.SecondarySortCriteria.None:
                default:
                    break;
            }


            return taxlots;
        }

        private List<TaxLot> SortSellList(List<TaxLot> taxlots, PostTradeEnums.SecondarySortCriteria sortCriteria)
        {
            //primary sorting is done based on the core logic of the Algo for eg for FIFO taxlots will first be sorted(ASC) on closing date basis..
            taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { return t1.ClosingDate.CompareTo(t2.ClosingDate); });

            //Once the taxlots are sorted in ASC order on closing date then secondary sorting will be applied on the same date taxlots by secondary criteria either AveragePrice or TaxlotID....
            switch (sortCriteria)
            {
                case PostTradeEnums.SecondarySortCriteria.AvgPxASC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    break;

                case PostTradeEnums.SecondarySortCriteria.AvgPxDESC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
                    break;

                case PostTradeEnums.SecondarySortCriteria.SamePxAvgPxASC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice)); });
                    break;

                case PostTradeEnums.SecondarySortCriteria.SamePxAvgPxDESC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.AvgPrice.CompareTo(t2.AvgPrice) * (-1)); });
                    break;

                case PostTradeEnums.SecondarySortCriteria.OrderSequenceASC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID)); });
                    break;

                case PostTradeEnums.SecondarySortCriteria.OrderSequenceDESC:
                    taxlots.Sort(delegate (TaxLot t1, TaxLot t2) { int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date); return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID) * (-1)); });
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
        }

        public override void SecondarySort(TaxLot closeTaxLot, ref List<TaxLot> openTaxLotsAndPositions, ClosingPreferences closingPreferences, PostTradeEnums.SecondarySortCriteria secondarySortCriteria, PostTradeEnums.ClosingField closingField)
        {
            //if (secondarySortCriteria.Equals(PostTradeEnums.SecondarySortCriteria.SamePxAvgPxASC))
            //{
            //    openTaxLotsAndPositions.Sort(delegate(TaxLot t1, TaxLot t2)
            //    {
            //        int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date);
            //        if (result == 0)
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
            //        int result = t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date);
            //        if (result == 0)
            //        {
            //            if (t1.AvgPrice.Equals(closeTaxLot.AvgPrice))
            //                return -1;
            //            else if (t2.AvgPrice.Equals(closeTaxLot.AvgPrice))
            //                return 1;
            //            else
            //                return t1.AvgPrice.CompareTo(t2.AvgPrice)*(-1);
            //        }
            //        else
            //            return t1.AvgPrice.CompareTo(t2.AvgPrice)*(-1);
            //    });
            //}
        }
    }
}
