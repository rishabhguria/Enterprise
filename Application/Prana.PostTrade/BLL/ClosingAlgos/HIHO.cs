using Prana.BusinessObjects;
using System.Collections.Generic;

namespace Prana.PostTrade
{
    class HIHO : AlgoBase
    {
        #region IClosingAlgo Members

        public List<TaxLot> SortSellList(List<TaxLot> taxlots, PostTradeEnums.ClosingField closingField)
        {
            //Need to be sorted in Ascending order for HIFO
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
            return taxlots;
        }


        public List<TaxLot> SortBuyList(List<TaxLot> taxlots, PostTradeEnums.ClosingField closingField)
        {
            //Need to be sorted in Descending Order 
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
            return taxlots;
        }


        public override void Sort(ref List<TaxLot> BuyTaxlots, ref List<TaxLot> sellTaxlots, PostTradeEnums.SecondarySortCriteria sortCriteria, PostTradeEnums.ClosingField closingField)
        {
            BuyTaxlots = SortBuyList(BuyTaxlots, closingField);
            sellTaxlots = SortSellList(sellTaxlots, closingField);
        }

        public override void SecondarySort(TaxLot closeTaxLot, ref List<TaxLot> openTaxLotsAndPositions, ClosingPreferences closingPreferences, PostTradeEnums.SecondarySortCriteria secondarySortCriteria, PostTradeEnums.ClosingField closingField)
        {

        }
        #endregion

    }
}