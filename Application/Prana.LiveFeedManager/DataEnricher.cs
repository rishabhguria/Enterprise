using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.LiveFeed;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.LiveFeedManager
{
    /// <summary>
    /// Enriches the data received from LiveFeed.
    /// </summary>
    internal class DataEnricher
    {
        static Dictionary<string, string> _expirationMonthCode = new Dictionary<string, string>();
        static DataEnricher()
        {
            FillFutureExpirationMonthCodes();
        }

        internal void ProcessValue(ref SymbolData data)
        {
            //data.PreferencedPrice = data.FinalLastPrice;
            if (data.CategoryCode == AssetCategory.Equity || data.CategoryCode == AssetCategory.Future || data.CategoryCode == AssetCategory.Indices || data.CategoryCode == AssetCategory.FXForward || data.CategoryCode == AssetCategory.FX || data.CategoryCode == AssetCategory.PrivateEquity || data.CategoryCode == AssetCategory.CreditDefaultSwap)
            {
                data.UnderlyingSymbol = data.Symbol;
            }
            //http://jira.nirvanasolutions.com:8080/browse/ENG-111
            if (data.CategoryCode == AssetCategory.EquityOption || data.CategoryCode == AssetCategory.FutureOption || data.CategoryCode == AssetCategory.Future)
            {
                data.AverageVolume20Day = ((SymbolData)data).OpenInterest;
            }

            /// Dividend yield converted into absolute terms. Have to multiply for 100 while displaying on the UI.
            /// TODO: SK 20131008. Data enricher needs to move to post processor. The post processor should work on top of centralized pricing cache 
            /// instead of directly on symbol data. THE GBP?GBX pricing adjustments also need s to be moved in post processor.
            if (data.CategoryCode == AssetCategory.Equity && data.MarketDataProvider == MarketDataProvider.Esignal)
            {
                data.DividendYield = data.DividendYield / 100;
            }
            //Commented as currency code is null in case of ContinuosDataResponse. So moved this check in SymbolData.UpdateContinuousData
            //as we maintain the static data in _liveFeedCache.
            //// in some cases -LON symbols were coming with EURO currency and due to this they were falsely coverted to higher currency. So added this check.
            //if ((!string.IsNullOrEmpty(data.CurencyCode) && (data.CurencyCode == "GBX" || data.CurencyCode == "ZAC")) 
            //    || ( data.Symbol.Contains("-LON") || data.Symbol.Contains("-JSE") || data.UnderlyingSymbol.Contains("-LON") || data.UnderlyingSymbol.Contains("-JSE")))            
            //{                
            //ConvertToHigherCurrency(ref data);
            //        data.IsChangedToHigherCurrency = true;              
            //}


            if (data.Ask != double.MinValue && data.Bid != double.MinValue)
            {
                data.Spread = data.Ask - data.Bid;
            }

            if (data.Ask != double.MinValue && data.Bid != double.MinValue && data.LastPrice != double.MinValue)
            {
                double tempMid = (data.Ask + data.Bid) / 2;
                data.Mid = tempMid==0 ? data.Mid : tempMid;

                double minVal = (data.Bid < data.Ask ? data.Bid : data.Ask);
                double maxVal = (data.Bid > data.Ask ? data.Bid : data.Ask);
                if (minVal <= data.LastPrice && data.LastPrice <= maxVal)
                {

                    data.iMid = data.LastPrice;
                }
                else
                {
                    data.iMid = (data.Ask + data.Bid) / 2;
                }
            }

            if (data.ListedExchange != string.Empty && data.CategoryCode != AssetCategory.None)
            {
                data.AUECID = CachedDataManager.GetInstance.GetAUECIdByExchangeIdentifier(data.ListedExchange + "-" + data.CategoryCode.ToString());
            }
            if (data.CategoryCode == AssetCategory.Indices)
            {
                data.AUECID = CachedDataManager.GetInstance.GetAUECIdByExchangeIdentifier(data.CategoryCode.ToString() + "-" + data.CategoryCode.ToString());
            }
            if (String.IsNullOrEmpty(data.CurencyCode) && data.AUECID != int.MinValue && data.AUECID != 0)
            {
                int currencyID = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(data.AUECID);
                string currencyText = CachedDataManager.GetInstance.GetCurrencyText(currencyID);
                data.CurencyCode = currencyText;
            }
            if (data.AUECID != int.MinValue)
            {
                int exchangeID = int.MinValue;
                Underlying underlying = Underlying.None;
                CachedDataManager.GetInstance.GetUnderlyingExchangeIDFromAUECID(data.AUECID, ref underlying, ref exchangeID);
                data.UnderlyingCategory = underlying;
                data.ExchangeID = exchangeID;

            }

            if (data.Change != double.MinValue && data.Previous != double.MinValue && data.Previous != 0.0)
            {
                data.PctChange = (data.Change / data.Previous) * 100;

            }

            if (data.Open != double.MinValue && data.Previous != double.MinValue)
            {
                data.GapOpen = (data.Open - data.Previous);
            }

            if (data.CategoryCode == AssetCategory.Equity)
            {
                EquitySymbolData eqData = data as EquitySymbolData;
                if (eqData != null)
                {
                    if (eqData.LastPrice != double.MinValue && eqData.SharesOutstanding != double.MinValue)
                    {
                        eqData.MarketCapitalization = eqData.SharesOutstanding * data.LastPrice;
                    }
                }
            }

            if (data.CategoryCode == AssetCategory.FutureOption)
            {
                OptionSymbolData opdata = data as OptionSymbolData;
                if (opdata != null)
                {
                    if (!String.IsNullOrEmpty(opdata.Symbol))
                    {
                        string[] symbolArr = opdata.Symbol.Split(' ');
                        if (symbolArr.Length > 1)
                        {
                            // eg: ES G1C375 & ES G1P375 is future option
                            //  ES G1 is future 
                            if (symbolArr[1].Length > 2)
                            {
                                if (symbolArr[1].Contains("P"))
                                {
                                    opdata.PutOrCall = OptionType.PUT;
                                }
                                else if (symbolArr[1].Contains("C"))
                                {
                                    opdata.PutOrCall = OptionType.CALL;
                                }
                                else
                                {
                                    opdata.PutOrCall = OptionType.NONE;
                                }


                            }
                        }
                    }
                }
            }

            if (data.CategoryCode == AssetCategory.EquityOption)
            {
                OptionSymbolData opData = data as OptionSymbolData;
                if (opData != null)
                {
                    if (opData.ExpirationDate != DateTimeConstants.MinValue)
                    {
                        TimeSpan ts1 = opData.ExpirationDate.Date.Subtract(DateTime.Now.Date);
                        int noOfDaysToExpiration = ts1.Days;
                        opData.DaysToExpiration = noOfDaysToExpiration;
                    }


                    if (opData.UnderlyingSymbol != String.Empty && opData.PutOrCall != OptionType.NONE && opData.StrikePrice != double.MinValue)
                    {
                        string monthName = GetMonthFullName(opData.ExpirationDate.Month, opData.ExpirationDate.Year);
                        string optionType = string.Empty;
                        if (opData.PutOrCall == OptionType.CALL)
                        {
                            optionType = " " + "CALL";
                        }
                        else
                        {
                            optionType = " " + "PUT";
                        }
                        opData.FullCompanyName = opData.UnderlyingSymbol + " " + monthName + " " + opData.StrikePrice.ToString() + optionType;
                    }
                }
            }

            if (data.CategoryCode == AssetCategory.Future)
            {
                FutureSymbolData futData = data as FutureSymbolData;
                if (futData != null)
                {
                    if (futData.ExpirationDate != DateTimeConstants.MinValue)
                    {
                        TimeSpan ts1 = futData.ExpirationDate.Date.Subtract(DateTime.Now.Date);
                        int noOfDaysToExpiration = ts1.Days;
                        futData.DaysToExpiration = noOfDaysToExpiration;
                    }
                    GetFutureExpirationInformation(futData);
                }

            }

            if (data.CategoryCode == AssetCategory.FutureOption)
            {
                OptionSymbolData opData = data as OptionSymbolData;
                if (opData != null)
                {
                    //special case for LME Future Options as for them the expiration date is coming wrong..                  
                    string exchangeIdentifier = opData.ListedExchange + "-" + opData.CategoryCode.ToString();
                    if (string.Compare(exchangeIdentifier, "LME-FutureOption", true) == 0)
                    {
                        opData.ExpirationDate = opData.ExpirationDate.AddDays(1);
                    }
                    if (opData.ExpirationDate != DateTimeConstants.MinValue)
                    {
                        TimeSpan ts1 = opData.ExpirationDate.Date.Subtract(DateTime.Now.Date);
                        int noOfDaysToExpiration = ts1.Days;
                        opData.DaysToExpiration = noOfDaysToExpiration;
                    }

                    if (opData.UnderlyingSymbol != String.Empty && opData.PutOrCall != OptionType.NONE && opData.StrikePrice != double.MinValue)
                    {
                        string monthName = GetMonthFullName(opData.ExpirationDate.Month, opData.ExpirationDate.Year);
                        string optionType = string.Empty;
                        if (opData.PutOrCall == OptionType.CALL)
                        {
                            optionType = " " + "CALL";
                        }
                        else
                        {
                            optionType = " " + "PUT";
                        }
                        opData.FullCompanyName = opData.UnderlyingSymbol + " " + monthName + " " + opData.StrikePrice.ToString() + optionType;
                    }
                }

            }



        }

        public static void GetFutureExpirationInformation(FutureSymbolData futureSnapShotData)
        {
            try
            {
                string symbol = futureSnapShotData.Symbol;
                string futureRootSymbol = string.Empty;
                if (String.IsNullOrEmpty(symbol))
                {
                    return;
                }
                string[] symbolInfoArr = symbol.Split(' ');
                if (symbolInfoArr != null && symbolInfoArr.Length == 2)
                {
                    if (String.IsNullOrEmpty(symbolInfoArr[0]) || String.IsNullOrEmpty(symbolInfoArr[1]))
                    {
                        return;
                    }
                    futureRootSymbol = symbolInfoArr[0];

                    string monthYearStr = symbolInfoArr[1].Substring(0, 2);
                    int result = 0;
                    //MJC: Make sure its numeric
                    if (Int32.TryParse(monthYearStr, out result) == false) return;

                    string monthStr = string.Empty;
                    int expirationYearMonth = 0;
                    string monthName = string.Empty;
                    string year2digit = string.Empty;
                    GetExpiryMonth(monthYearStr, ref expirationYearMonth, ref monthName, ref year2digit);
                    if (String.IsNullOrEmpty(futureSnapShotData.FullCompanyName))
                    {
                        futureSnapShotData.FullCompanyName = futureRootSymbol + " " + monthName + " " + "Future";
                    }
                    else
                    {
                        futureSnapShotData.FullCompanyName = monthName + " " + year2digit + " " + futureSnapShotData.FullCompanyName;
                    }
                    //futureSnapShotData.ExpirationMonth = expirationYearMonth;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        public static void GetExpiryMonth(string monthYearStr, ref int expirationYearMonth, ref string monthName, ref string year2digit)
        {
            //int computedMonthYear = 0;
            try
            {
                //Actual Code
                //string monthCode = monthYearStr.Substring(0, 1);
                //string yearCode = monthYearStr.Substring(1, 1);
                string monthCode;
                string yearCode;
                // Here Month and Year code come as for Symbol - 'L M4-EIR' or 'BRN 2H-ICE'
                // So I parse Month code, based on Parsing, month and year code values are splited
                // Chnaged by Sandeep Singh on Feb 7,2012    
                //updated code
                string tempVar = monthYearStr.Substring(0, 1);
                int result = 0;

                bool isNumber = int.TryParse(tempVar, out result);
                if (isNumber)
                {
                    monthCode = monthYearStr.Substring(1, 1);
                    yearCode = monthYearStr.Substring(0, 1);
                }
                else
                {
                    monthCode = monthYearStr.Substring(0, 1);
                    yearCode = monthYearStr.Substring(1, 1);
                }

                string monthNo = string.Empty;
                string year = DateTime.Now.Year.ToString();
                //bool isCoversionOk = false;
                //TODO : need to change for next Century
                // if year code is like 7,8,9 then check for lenght =1 else for length =2
                year = year.Substring(3, 1).Equals(yearCode) ? year : string.Empty;
                if (String.IsNullOrEmpty(year))
                {
                    year = GetFutureYear(yearCode);
                    year2digit = year.Substring(2);
                }

                if (_expirationMonthCode.ContainsKey(monthCode))
                {
                    monthNo = _expirationMonthCode[monthCode];
                    monthName = Enum.GetName(typeof(MonthEnum), Convert.ToInt32(monthNo));
                    year = year.Substring(3, 1).Equals(yearCode) ? year : string.Empty;
                    int.TryParse(year + monthNo, out expirationYearMonth);
                }
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


        private static string GetFutureYear(string singleDigitYearCode)
        {
            int tempYear = DateTime.Now.Year;
            do
            {
                ++tempYear;
            } while (!tempYear.ToString().Substring(3, 1).Equals(singleDigitYearCode));

            return tempYear.ToString();
        }


        private void ConvertToHigherCurrency(ref SymbolData data)
        {
            if (data.CurencyCode == "GBX")
            {
                data.CurencyCode = "GBP";
            }
            if (data.CurencyCode == "ZAC")
            {
                data.CurencyCode = "ZAR";
            }
            data.Ask = data.Ask / 100;
            data.Bid = data.Bid / 100;
            data.Change = data.Change / 100;
            data.Dividend = data.Dividend / 100;
            data.DividendAmtRate = data.DividendAmtRate / 100;
            data.High = data.High / 100;
            data.LastPrice = data.LastPrice / 100;
            data.Low = data.Low / 100;
            data.Open = data.Open / 100;
            data.High52W = data.High52W / 100;
            data.Low52W = data.Low52W / 100;
            data.Previous = data.Previous / 100;

            data.GapOpen = data.GapOpen / 100;
            data.Spread = data.Spread / 100;
        }

        const int MAX_EXPIRATION_MONTH_LENGTH = 6;
        string[] monthNameArr = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

        private string GetMonthFullName(int expirationMonth, int year)
        {
            string strExpirationMonth = expirationMonth.ToString();
            string strFullMonthName = string.Empty;
            if (expirationMonth <= 12)
            {
                strFullMonthName = monthNameArr[expirationMonth - 1] + " " + year.ToString();
            }

            //if (strExpirationMonth.Length == MAX_EXPIRATION_MONTH_LENGTH)
            //{
            //    string year = expirationMonth.ToString().Substring(0, 4);
            //    int month = Convert.ToInt32(expirationMonth.ToString().Substring(4, 2));
            //    if (month <= 12)
            //    {
            //        strFullMonthName = monthNameArr[month - 1] + " " + year.ToString();
            //    }
            //}

            if (strFullMonthName != string.Empty)
            {
                strFullMonthName = " " + strFullMonthName;
            }
            return strFullMonthName;

        }

        private static void FillFutureExpirationMonthCodes()
        {
            _expirationMonthCode.Add("F", "01"); //Jan
            _expirationMonthCode.Add("G", "02"); //FEB
            _expirationMonthCode.Add("H", "03"); //MAR
            _expirationMonthCode.Add("J", "04"); //APR
            _expirationMonthCode.Add("K", "05"); //MAY
            _expirationMonthCode.Add("M", "06"); //JUN
            _expirationMonthCode.Add("N", "07"); //JUL
            _expirationMonthCode.Add("Q", "08"); //AUG
            _expirationMonthCode.Add("U", "09"); //SEP
            _expirationMonthCode.Add("V", "10"); //OCT
            _expirationMonthCode.Add("X", "11"); //NOV
            _expirationMonthCode.Add("Z", "12"); //DEC
        }
    }
}
