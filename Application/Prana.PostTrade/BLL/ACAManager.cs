using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Data;
//using Prana.PositionServices;
using Prana.Interfaces;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.CommonDataCache;
using Prana.Utilities.DateTimeUtilities;
using Prana.BusinessObjects.AppConstants;

namespace Prana.PostTrade
{
    public class ACAManager
    {

        static IPranaPositionServices _positionServices = null;
        public static IPranaPositionServices positionServices
        {
            set
            {
                _positionServices = value;
            }
        }

       static  Dictionary<DateTime, Dictionary<string, ACAData>> _dictDateWiseACA = new Dictionary<DateTime, Dictionary<string, ACAData>>();

        private static Dictionary<DateTime, List<TaxLot>> GetACATaxlots(List<DateSymbol> lstSymbols, DateTime fromdate, DateTime toDate, List<string> eligibleFundlist, bool isForSymbol)
        {
            Dictionary<DateTime, List<TaxLot>> dictTransactions = new Dictionary<DateTime, List<TaxLot>>();
            try
            {
                dictTransactions = _positionServices.GetAllACATransactions(lstSymbols, fromdate, toDate, isForSymbol, eligibleFundlist);

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                //return false;
            }

            return dictTransactions;
        }

        public static List<TaxLot> GetACATaxlotsForClosing(List<TaxLot> taxlots, List<string> eligibleFundlist)
        {
            List<TaxLot> EligibleTaxlots = new List<TaxLot>();

            try
            {
                EligibleTaxlots = taxlots.FindAll(
             delegate(TaxLot taxlot)
             {
                 if (eligibleFundlist.Contains(taxlot.Level1ID.ToString()))
                 {
                     return true;
                 }
                 else
                 {
                     return false;
                 }
             }
         );

                taxlots.RemoveAll(delegate(TaxLot taxlot)
                    {
                        if (eligibleFundlist.Contains(taxlot.Level1ID.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                );
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                //return false;
            }


            return EligibleTaxlots;
        }

        public static void CreateACADateWiseDictionary(List<DateSymbol> listSymbols)
        {
            try
            {
                _dictDateWiseACA.Clear();
                DateTime fromDate = DateTime.UtcNow.Date;
                DateTime toDate = DateTime.UtcNow.Date;
                Dictionary<DateTime, List<ACAData>> dictACA = ACADataManager.GetACADataFromDB(listSymbols, fromDate, toDate, true);
                foreach (KeyValuePair<DateTime, List<ACAData>> kp in dictACA)
                {
                    List<ACAData> acaList = kp.Value;
                    Dictionary<string, ACAData> dictACAData = new Dictionary<string, ACAData>();
                    foreach (ACAData acaData in acaList)
                    {
                        string key = acaData.Symbol.ToString().ToLower() + acaData.FundID.ToString() + acaData.PositionType;
                        dictACAData.Add(key, acaData);
                    }
                    _dictDateWiseACA.Add(kp.Key, dictACAData);
                }
            }

            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                //return false;
            }
        }
        public static void ApplyACA(List<TaxLot> taxlots, DateTime ACAdate)
        {
            try
            {
                foreach (TaxLot taxlot in taxlots)
                {
                    if (_dictDateWiseACA.ContainsKey(ACAdate.Date))
                    {
                        Dictionary<string, ACAData> dictACA = _dictDateWiseACA[ACAdate.Date];
                        string key = string.Empty;
                        switch (taxlot.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Open:
                            case FIXConstants.SIDE_Sell:
                            case FIXConstants.SIDE_Sell_Closed:
                              
                                key = taxlot.Symbol.ToString().ToLower() + taxlot.Level1ID.ToString() + PositionType.Long.ToString();
                                break;
                            
                            case FIXConstants.SIDE_SellShort:
                            case FIXConstants.SIDE_Sell_Open:
                            case FIXConstants.SIDE_Buy_Closed:
                          
                               key = taxlot.Symbol.ToString().ToLower() + taxlot.Level1ID.ToString() + PositionType.Short.ToString();
                                break;
                    
                            default:
                                break;
                        }
                        if (dictACA.ContainsKey(key))
                        {
                            ACAData acaData = dictACA[key];
                            taxlot.ACAData.ACAAvgPrice = acaData.ACAAvgPrice;
                            taxlot.ACAData.ACAUnitCost = acaData.ACAUnitCost;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;

                }
            }
        }
        private static List<ACAData> GetACA(Dictionary<DateTime, List<TaxLot>> dictTransactions, Dictionary<DateTime, List<ACAData>> dictACAData, DateTime fromDate, DateTime toDate)
        {
            List<ACAData> listACADataTotal = new List<ACAData>();
            List<ACAData> listACADataPreviousDate = new List<ACAData>();
            Dictionary<string, List<TaxLot>> dictOpenTransactions = null;
            Dictionary<string, List<TaxLot>> dictClosingTransactions = null;
            Dictionary<string, ACAData> dictACADataPreviousDate = null;
            Dictionary<DateTime,Dictionary<string,string>> dictAppliedCAs = GetCAsForDateRange(CorporateActionType.NameChange, fromDate, toDate, true);
            try
            {
                while (fromDate <= toDate)
                {
                    DateTime previousDate = GetPreviousBusinessDay(fromDate);

                    if (dictACAData.ContainsKey(previousDate))
                    {
                        List<ACAData> lstACAData = dictACAData[previousDate];
                        listACADataPreviousDate.AddRange(lstACAData);
                    }

                    List<TaxLot> dateTaxlots = new List<TaxLot>();
                    if (dictTransactions.ContainsKey(fromDate))
                    {
                        dateTaxlots = dictTransactions[fromDate];
                    }
                    else
                    {
                        foreach (ACAData acaData in listACADataPreviousDate)
                        {
                            // acaData.Date = fromDate;
                            ACAData acaDataNew = (ACAData)acaData.Clone();
                            acaDataNew.Date = fromDate;
                            listACADataTotal.Add(acaDataNew);
                        }
                        fromDate = GetNextBusinessDay(fromDate);
                        continue;
                    }
                    if (dateTaxlots.Count > 0)
                    {
                        dictOpenTransactions = new Dictionary<string, List<TaxLot>>();
                        dictClosingTransactions = new Dictionary<string, List<TaxLot>>();
                        dictACADataPreviousDate = new Dictionary<string, ACAData>();
                        foreach (TaxLot taxlot in dateTaxlots)
                        {
                            string positionType = string.Empty;
                            string key = string.Empty;
                            switch (taxlot.OrderSideTagValue)
                            {
                                //All opening transactions
                                case FIXConstants.SIDE_Buy:
                                case FIXConstants.SIDE_Buy_Open:
                                case FIXConstants.SIDE_SellShort:
                                case FIXConstants.SIDE_Sell_Open:

                                    key = taxlot.Symbol.ToString().ToLower() + taxlot.Level1ID.ToString() + taxlot.PositionType;
                                    if (dictOpenTransactions.ContainsKey(key))
                                    {
                                        dictOpenTransactions[key].Add(taxlot);
                                    }
                                    else
                                    {
                                        List<TaxLot> listtaxlots = new List<TaxLot>();
                                        listtaxlots.Add(taxlot);
                                        dictOpenTransactions.Add(key, listtaxlots);
                                    }
                                    break;

                                // All closing transactions
                                case FIXConstants.SIDE_Sell:
                                case FIXConstants.SIDE_Sell_Closed:

                                    positionType = PositionType.Long.ToString();
                                    key = taxlot.Symbol.ToString().ToLower() + taxlot.Level1ID.ToString() + positionType;
                                    if (dictClosingTransactions.ContainsKey(key))
                                    {
                                        dictClosingTransactions[key].Add(taxlot);
                                    }
                                    else
                                    {
                                        List<TaxLot> listtaxlots = new List<TaxLot>();
                                        listtaxlots.Add(taxlot);
                                        dictClosingTransactions.Add(key, listtaxlots);
                                    }
                                    break;
                                case FIXConstants.SIDE_Buy_Closed:
                                    positionType = PositionType.Short.ToString();
                                    key = taxlot.Symbol.ToString().ToLower() + taxlot.Level1ID.ToString() + positionType;
                                    if (dictClosingTransactions.ContainsKey(key))
                                    {
                                        dictClosingTransactions[key].Add(taxlot);
                                    }
                                    else
                                    {
                                        List<TaxLot> listtaxlots = new List<TaxLot>();
                                        listtaxlots.Add(taxlot);
                                        dictClosingTransactions.Add(key, listtaxlots);
                                    }
                                    break;
                            }
                        }

                        if (dictAppliedCAs.ContainsKey(fromDate))
                        {
                            AdjustACADataForCA(fromDate, listACADataPreviousDate, dictAppliedCAs);
                        }

                        foreach (ACAData acaData in listACADataPreviousDate)
                        {
                            ACAData acaDataNew = (ACAData)acaData.Clone();
                            acaDataNew.Date = fromDate;
                            string key = acaDataNew.Symbol.ToString().ToLower() + acaDataNew.FundID.ToString() + acaDataNew.PositionType;
                            dictACADataPreviousDate.Add(key, acaDataNew);
                        }
                        listACADataPreviousDate = CalculateACAAveragePrice(dictACADataPreviousDate, dictOpenTransactions, dictClosingTransactions, fromDate);
                        listACADataTotal.AddRange(listACADataPreviousDate);
                    }

                    fromDate = GetNextBusinessDay(fromDate);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }

            return listACADataTotal;
        }

        // should have been called from CAServices but due to cross referencing (caservices using closing) it cannot be called. 
        private static Dictionary<DateTime, Dictionary<string, string>> GetCAsForDateRange(CorporateActionType caType, DateTime fromDate, DateTime toDate, bool isApplied)
        {

            DataTable dtNamechange = new DataTable();
            CAOnProcessObjects requestObject = null;
            try
            {
                requestObject = new CAOnProcessObjects();
                requestObject.CAType = caType;
                requestObject.FromDate = fromDate;
                requestObject.ToDate = toDate;
                requestObject.IsApplied = isApplied;

                return ACADataManager.GetAllCAs(requestObject);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return new Dictionary<DateTime, Dictionary<string, string>>();
        }

        private static void AdjustACADataForCA(DateTime Date, List<ACAData> lstACAData, Dictionary<DateTime, Dictionary<string, string>> dictAppliedNameChanges)
        {
            if (dictAppliedNameChanges.ContainsKey(Date))
            {
                Dictionary<string, string> dictNameChanges = dictAppliedNameChanges[Date];
                List<ACAData> lstACADataCopy = new List<ACAData>();
                lstACADataCopy.AddRange(lstACAData);

                foreach (ACAData data in lstACADataCopy)
                {
                    if (dictNameChanges.ContainsKey(data.Symbol))
                    {
                        string NewSymbol = dictNameChanges[data.Symbol];
                        ACAData acaDataNew = (ACAData)data.Clone();
                        acaDataNew.Symbol = NewSymbol;
                        lstACAData.Add(acaDataNew);
                    }
                }
            }
        }

        private static List<ACAData> CalculateACAAveragePrice(Dictionary<string, ACAData> dictACAData, Dictionary<string, List<TaxLot>> dictOpenTransactions, Dictionary<string, List<TaxLot>> dictCloseTransactions, DateTime date)
        {
            List<ACAData> ListACAData = new List<ACAData>();
            bool isTransitionTaxlot = false;

            foreach (KeyValuePair<string, List<TaxLot>> kp in dictOpenTransactions)
            {
                ACAData acaData = null;
                if (dictACAData.ContainsKey(kp.Key))
                {
                    acaData = dictACAData[kp.Key];
                }
                else
                {
                    TaxLot taxlotobj = kp.Value[0];
                    acaData = new ACAData(taxlotobj.Symbol, taxlotobj.Level1ID, taxlotobj.OrderSideTagValue);
                    dictACAData.Add(kp.Key, acaData);
                }
                CalculateAndSetValues(kp.Value, acaData);
                acaData.Date = date;
               // dictOpenTransactions.Remove(kp.Key);
            }

            foreach (string key in dictCloseTransactions.Keys)
            {
                if (dictACAData.ContainsKey(key))
                {
                    isTransitionTaxlot = false;
                    ACAData acaData = dictACAData[key];
                    List<TaxLot> lstTaxlots = new List<TaxLot>();

                    foreach (TaxLot taxlot in dictCloseTransactions[key])
                    {
                        acaData.PositionQty = (acaData.PositionQty)-(taxlot.TaxLotQty);
                        if (acaData.PositionQty < 0 && !isTransitionTaxlot)
                        {
                            isTransitionTaxlot = true;
                            taxlot.TaxLotQty = (acaData.PositionQty) * (-1);
                            lstTaxlots.Add(taxlot);
                        }
                        else if (acaData.PositionQty < 0)
                        {
                            lstTaxlots.Add(taxlot);
                        }
                    }
                    // case where the position is changing from long to short or vice versa.
                    if (lstTaxlots.Count > 0 && acaData.PositionQty < 0)
                    {
                        ACAData acaDataNew = null;
                        string positionTypeNew = string.Empty;
                        if (acaData.PositionType.Equals(PositionType.Long.ToString()))
                        {
                            positionTypeNew = PositionType.Short.ToString();
                        }
                        else
                        {
                            positionTypeNew = PositionType.Long.ToString();
                        }
                        string keyNew = acaData.FundID.ToString() + acaData.Symbol + positionTypeNew;

                        if (dictACAData.ContainsKey(keyNew))
                        {
                            acaDataNew = dictACAData[keyNew];
                        }
                        else
                        {
                            acaDataNew = new ACAData();
                            acaDataNew.FundID = acaData.FundID;
                            acaData.Symbol = acaData.Symbol;
                            acaData.PositionType = positionTypeNew;
                            dictACAData.Add(keyNew, acaDataNew);
                        }
                        CalculateAndSetValues(lstTaxlots, acaDataNew);
                        acaDataNew.Date = date;
                    }
                    if (acaData.PositionQty <= 0)
                    {
                        dictACAData.Remove(key);
                    }
                }
            }
            ListACAData.AddRange(dictACAData.Values);
            return ListACAData;
        }

        private static void CalculateAndSetValues(List<TaxLot> taxlots, ACAData acaData)
        {
            double ACAAveragePrice = 0;
            double ACAUnitCost = 0;
            double totalNotional = 0;
            double totalNetNotional = 0;
            double totalQuantity = 0;
          
            foreach (TaxLot taxlot in taxlots)
            {
                totalNetNotional += (taxlot.AvgPrice * taxlot.TaxLotQty) + (taxlot.TotalCommissionandFees);
                totalNotional += (taxlot.AvgPrice * taxlot.TaxLotQty);
                totalQuantity += taxlot.TaxLotQty;

            }
            if (totalQuantity != 0)
            {
                ACAAveragePrice = ((acaData.ACAAvgPrice * acaData.PositionQty) + totalNotional) / (totalQuantity + acaData.PositionQty);
                ACAUnitCost = ((acaData.ACAUnitCost * acaData.PositionQty) + totalNetNotional) / (totalQuantity + acaData.PositionQty);
            }
            acaData.ACAAvgPrice = ACAAveragePrice;
            acaData.ACAUnitCost = ACAUnitCost;
            acaData.PositionQty += totalQuantity;
        }

        public static void CalculateACADataForSymbol(Dictionary<string, DateTime> dictSymbols, List<string> eligibleFundlist, DateTime toDate)
        {
            try
            {
                List<ACAData> lstACAData = null;
                int rowsAffected = 0;
                if (eligibleFundlist.Count > 0)
                {
                    DateTime fromDate = DateTime.UtcNow.Date;
                    List<DateTime> dates = new List<DateTime>();
                    dates.AddRange(dictSymbols.Values);
                    dates.Sort(delegate(DateTime d1, DateTime d2) { return d1.Date.CompareTo(d2.Date); });
                    if (dates.Count > 0)
                    {
                        fromDate = dates[0];
                    }
                    if (fromDate.Date > toDate.Date)
                    {
                        return;
                    }
                    List<DateSymbol> lstSymbols = new List<DateSymbol>();

                    List<DateSymbol> lstSymbolsNew = new List<DateSymbol>();
                    foreach (KeyValuePair<string, DateTime> kp in dictSymbols)
                    {
                        DateSymbol dateSymbol = new DateSymbol();
                        dateSymbol.Symbol = kp.Key;
                        dateSymbol.FromDate = kp.Value.Date;
                        dateSymbol.ToDate = toDate.Date;
                        lstSymbols.Add(dateSymbol);
                    }

                    Dictionary<DateTime, List<TaxLot>> dictTransactions = GetACATaxlots(lstSymbols, fromDate.Date, toDate.Date, eligibleFundlist, true);
                  foreach (DateSymbol dateSymbol in lstSymbols)
                    {
                        DateSymbol dateSymbolNew = new DateSymbol();
                        dateSymbolNew.Symbol = dateSymbol.Symbol;
                        dateSymbolNew.FromDate = dateSymbol.FromDate;
                        dateSymbolNew.ToDate = dateSymbol.ToDate;
                        dateSymbolNew.FromDate = GetPreviousBusinessDay(dateSymbol.FromDate);
                        lstSymbolsNew.Add(dateSymbolNew);
                    }
                    Dictionary<DateTime, List<ACAData>> dictACAData = ACADataManager.GetACADataFromDB(lstSymbolsNew, fromDate.Date, toDate.Date, true);
                    lstACAData = GetACA(dictTransactions, dictACAData, fromDate.Date, toDate.Date);

                    if (lstACAData.Count > 0 || lstSymbols.Count > 0)
                    {
                        rowsAffected = ACADataManager.SaveACAData(lstACAData, lstSymbols, true);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

            }
        }
        
        private static DateTime GetPreviousBusinessDay(DateTime Date)
        {
            DateTime previousDate = Date.AddDays(-1);
            bool isHoliday = BusinessDayCalculator.CheckForHoliday(previousDate);
            while (isHoliday)
            {
                previousDate = previousDate.AddDays(-1);
                isHoliday = BusinessDayCalculator.CheckForHoliday(previousDate);
            }
            return previousDate;
        }

        private static DateTime GetNextBusinessDay(DateTime date)
        {
            DateTime nextDate = date.AddDays(1);
            bool isHoliday = BusinessDayCalculator.CheckForHoliday(nextDate);
            while (isHoliday)
            {
                nextDate = nextDate.AddDays(1);
                isHoliday = BusinessDayCalculator.CheckForHoliday(nextDate);
            }
            return nextDate;
        }

        public static int CalculateAndSaveACA(DateTime fromDate, List<string> eligibleFundlist)
        {
            List<ACAData> lstACAData= null;
            int rowsAffected = 0;
           
            if (eligibleFundlist.Count > 0)
            {
                if (fromDate.Equals(DateTime.MinValue))
                {
                    fromDate = DateTime.UtcNow.Date;
                }
                DateTime toDate = DateTime.UtcNow.Date;
                DateTime previousDate = GetPreviousBusinessDay(fromDate);
                Dictionary<DateTime,List<TaxLot>> dictTransactions = GetACATaxlots(null, fromDate.Date, toDate.Date, eligibleFundlist,false);
                Dictionary<DateTime, List<ACAData>> dictACAData = ACADataManager.GetACADataFromDB(null, previousDate.Date, previousDate.Date, false);
                lstACAData = GetACA(dictTransactions, dictACAData, fromDate.Date, toDate.Date);

               if (lstACAData.Count > 0)
               {
                   rowsAffected = ACADataManager.SaveACAData(lstACAData, null, false);
               }
            }
            return rowsAffected;
        }
    }
}
