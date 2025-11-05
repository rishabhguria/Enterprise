using Prana.BusinessObjects;
using Prana.CommonDataCache;
using System;
using System.Collections.Generic;

namespace Prana.PostTrade
{
    class TAXADV : AlgoBase
    {

        private DateTime oneyearbackdate = DateTime.MinValue;
        private int LeadCurrencyID = int.MinValue;
        private static int VsCurrencyID = int.MinValue;

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

            foreach (TaxLot taxLot in taxlots)
            {
                //There will be equity and equity options for close amend UI so currencyID will be LeadCurrencyID and VsCurrencyID CompanyBaseCurrencyID will be VsCurrencyID;
                //CurrencyID is LeadCurrencyID
                LeadCurrencyID = taxLot.CurrencyID;
                //CompanyBaseCurrencyID is VsCurrencyID
                VsCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                if (taxLot.FXRate == 0)
                {
                    //CHMW-3132	Account wise fx rate handling for expiration settlement
                    ConversionRate conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetConversionRateFromCurrencies(LeadCurrencyID, VsCurrencyID, taxLot.AUECLocalDate, taxLot.Level1ID);
                    taxLot.FXConversionMethodOperator = conversionRate.ConversionMethod.ToString();
                    taxLot.FXRate = conversionRate.RateValue;
                }
            }
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

            foreach (TaxLot taxLot in taxlots)
            {
                //There will be equity and equity options for close amend UI so currencyID will be LeadCurrencyID and VsCurrencyID CompanyBaseCurrencyID will be VsCurrencyID;
                //CurrencyID is LeadCurrencyID
                LeadCurrencyID = taxLot.CurrencyID;
                //CompanyBaseCurrencyID is VsCurrencyID
                VsCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                if (taxLot.FXRate == 0)
                {
                    //CHMW-3132	Account wise fx rate handling for expiration settlement
                    ConversionRate conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetConversionRateFromCurrencies(LeadCurrencyID, VsCurrencyID, taxLot.AUECLocalDate, taxLot.Level1ID);
                    taxLot.FXConversionMethodOperator = conversionRate.ConversionMethod.ToString();
                    taxLot.FXRate = conversionRate.RateValue;
                }
            }

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


        /// <summary>
        /// Narendra Kumar Jangir June 27, 2013
        /// TAXADV(Tax Advantage) Lot Selection Method
        /// 
        /// The tax advantage lot selection method, applies weighted tax liability to each open lot to determine the order for lot selection. 
        /// The method is aimed at minimizing tax liability on a account/entity when securities, are disposed of. 
        /// The method will dispose of lots with the lowest tax liability first. 
        /// Gains and losses are based off of the amortized cost per unit of each lot and are weighted with long and short term tax rates.
        /// If multiple lots have the same weighted tax liability, they are selected on a FIFO basis. Tax advantage cannot be used with forwards, swaps,or physical exercises of options.   
        /// </summary>
        /// <param name="closeTaxLot"></param>
        /// <param name="openTaxLotsAndPositions"></param>
        /// <returns></returns>
        public override void SecondarySort(TaxLot closeTaxLot, ref List<TaxLot> openTaxLotsAndPositions, ClosingPreferences closingPreferences, PostTradeEnums.SecondarySortCriteria secondarySortCriteria, PostTradeEnums.ClosingField closingField)
        {
            //calculate one year back date to identify short term and long term taxlots
            DateTime oneyearbackdate = closeTaxLot.ClosingDate.AddYears(-1);
            //TODO: short term and long term tax rates will be set at closing preferences ui
            double shortTermTaxRate = closingPreferences.ClosingMethodology.ShortTermTaxRate;
            double longTermTaxRate = closingPreferences.ClosingMethodology.LongTermTaxRate;

            openTaxLotsAndPositions.Sort(delegate (TaxLot t1, TaxLot t2)
            {
                double UnitPNL1 = (closeTaxLot.UnitCost * closeTaxLot.FXRate - t1.UnitCost * t1.FXRate);
                double UnitPNL2 = (closeTaxLot.UnitCost * closeTaxLot.FXRate - t2.UnitCost * t2.FXRate);

                switch (closingField)
                {
                    case PostTradeEnums.ClosingField.AvgPrice:
                        UnitPNL1 = (closeTaxLot.AvgPrice * closeTaxLot.FXRate - t1.AvgPrice * t1.FXRate);
                        UnitPNL2 = (closeTaxLot.AvgPrice * closeTaxLot.FXRate - t2.AvgPrice * t2.FXRate);
                        break;
                    case PostTradeEnums.ClosingField.UnitCost:
                        UnitPNL1 = (closeTaxLot.UnitCost * closeTaxLot.FXRate - t1.UnitCost * t1.FXRate);
                        UnitPNL2 = (closeTaxLot.UnitCost * closeTaxLot.FXRate - t2.UnitCost * t2.FXRate);
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
                        UnitPNL1 = (closeTaxLot.UnitCost * closeTaxLot.FXRate - t1.UnitCost * t1.FXRate);
                        UnitPNL2 = (closeTaxLot.UnitCost * closeTaxLot.FXRate - t2.UnitCost * t2.FXRate);
                        break;
                }

                double UnitTaxLiability1 = 0;
                double UnitTaxLiability2 = 0;

                //t1 is long term
                if (t1.ClosingDate.Date < oneyearbackdate.Date)
                {
                    //for short trades UnitTaxLiability will be calculated using short term tax rate irrespective of opening trade is long or short
                    if (closeTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover) || closeTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || closeTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy))
                    {
                        UnitTaxLiability1 = UnitPNL1 * shortTermTaxRate;
                    }
                    else
                        UnitTaxLiability1 = UnitPNL1 * longTermTaxRate;
                }
                //t1 is short term
                else
                {
                    UnitTaxLiability1 = UnitPNL1 * shortTermTaxRate;
                }

                //t2 is long term
                if (t2.ClosingDate.Date < oneyearbackdate.Date)
                {
                    //for short trades UnitTaxLiability will be calculated using short term tax rate irrespective of opening trade is long or short
                    if (closeTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover) || closeTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || closeTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy))
                    {
                        UnitTaxLiability2 = UnitPNL2 * shortTermTaxRate;
                    }
                    else
                        UnitTaxLiability2 = UnitPNL2 * longTermTaxRate;
                }
                //t2 is short term
                else
                {
                    UnitTaxLiability2 = UnitPNL2 * shortTermTaxRate;
                }

                int result = UnitTaxLiability1.CompareTo(UnitTaxLiability2);

                //same tax liability for both taxlots, t1 and t2 have diffrent closing date follow FIFO
                if (result == 0 && !(t1.ClosingDate.Date.Equals(t2.ClosingDate.Date)))
                {
                    return t1.ClosingDate.Date.CompareTo(t2.ClosingDate.Date);
                }
                //same tax liability for both taxlots, t1 and t2 have same closing date follow secondary sort criteria ascending order sequence
                else if (result == 0 && t1.ClosingDate.Date.Equals(t2.ClosingDate.Date))
                {
                    return (result != 0) ? result : (t1.TaxLotID.CompareTo(t2.TaxLotID));
                }
                //for handling of short trades
                else if (closeTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover) || closeTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || closeTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy))
                {
                    return result * (-1);
                }
                else
                    return result;
            });
        }
    }
}