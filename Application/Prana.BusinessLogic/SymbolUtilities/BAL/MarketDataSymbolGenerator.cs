using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.LiveFeed;
using Prana.LogManager;
using Prana.Utilities.DateTimeUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace Prana.BusinessLogic.Symbol
{
    public static class MarketDataSymbolGenerator
    {
        private static Dictionary<string, MarketDataSymbolMapper> _dictMarketDataSymbolMapper;
        private static readonly object _dictMarketDataSymbolMapperLock = new object();
        private static Dictionary<string, string> _monthCodeWiseOptionType;
        private static Dictionary<string, string> _monthCodeWiseOptionMonthNumbers;
        private static Dictionary<string, string> _monthCodeWiseFutureMonthNumbers;

        private static string _regexOfEquityAsset = ConfigurationManager.AppSettings["RegexOfEquityAsset"];
        private static string _regexOfEquityOptionAsset = ConfigurationManager.AppSettings["RegexOfEquityOptionAsset"];
        private static string _regexOfFutureAsset = ConfigurationManager.AppSettings["RegexOfFutureAsset"];
        private static string _regexOfFutureOptionAsset = ConfigurationManager.AppSettings["RegexOfFutureOptionAsset"];
        private static string _regexOfFXAsset = ConfigurationManager.AppSettings["RegexOfFXAsset"];
        private static string _regexOfFXForwardAsset = ConfigurationManager.AppSettings["RegexOfFXForwardAsset"];

        public static MarketDataSymbolResponse GetMarketDataSymbolFromTickerSymbol(MarketDataSymbolResponse marketDataSymbolResponse, MarketDataProvider marketDataProvider, FutureRootData futureRootData)
        {
            try
            {
                Setup();

                if (marketDataSymbolResponse.AUECID == 0)
                {
                    marketDataSymbolResponse.AssetCategory = GetAssetCategoryUsingRegex(marketDataSymbolResponse.TickerSymbol);
                }

                MarketDataSymbolMapperHelper marketDataSymbolMapperHelper = null;
                switch (marketDataSymbolResponse.AssetCategory)
                {
                    case AssetCategory.Indices:
                        marketDataSymbolMapperHelper = IndexTickerSymbolTransformation(marketDataSymbolResponse.TickerSymbol);
                        break;
                    case AssetCategory.Equity:
                        marketDataSymbolMapperHelper = EquityTickerSymbolTransformation(marketDataSymbolResponse.TickerSymbol);
                        break;
                    case AssetCategory.EquityOption:
                        marketDataSymbolMapperHelper = EquityOptionTickerSymbolTransformation(marketDataSymbolResponse.TickerSymbol, marketDataProvider);
                        break;
                    case AssetCategory.Future:
                        marketDataSymbolMapperHelper = FutureTickerSymbolTransformation(marketDataSymbolResponse.TickerSymbol);
                        break;
                    case AssetCategory.FutureOption:
                        marketDataSymbolMapperHelper = FutureOptionTickerSymbolTransformation(marketDataSymbolResponse.TickerSymbol);
                        break;
                    case AssetCategory.FX:
                        marketDataSymbolMapperHelper = FXTickerSymbolTransformation(marketDataSymbolResponse.TickerSymbol);
                        break;
                    case AssetCategory.FXForward:
                        marketDataSymbolMapperHelper = FXForwardTickerSymbolTransformation(marketDataSymbolResponse.TickerSymbol);
                        break;
                }

                if (marketDataSymbolMapperHelper != null && _dictMarketDataSymbolMapper.ContainsKey(marketDataSymbolMapperHelper.Key))
                {
                    MarketDataSymbolMapper marketDataSymbolMapper = _dictMarketDataSymbolMapper[marketDataSymbolMapperHelper.Key];

                    if (marketDataSymbolResponse.AUECID == 0)
                    {
                        marketDataSymbolResponse.AUECID = marketDataSymbolMapper.AUECID;
                    }

                    if (marketDataProvider == MarketDataProvider.FactSet)
                        marketDataSymbolResponse.FactSetSymbol = ParseSymbol(marketDataSymbolMapper, marketDataSymbolMapperHelper, marketDataProvider);
                    else if (marketDataProvider == MarketDataProvider.ACTIV)
                        marketDataSymbolResponse.ActivSymbol = ParseSymbol(marketDataSymbolMapper, marketDataSymbolMapperHelper, marketDataProvider);
                    else if (marketDataProvider == MarketDataProvider.SAPI)
                        marketDataSymbolResponse.BloombergSymbol = ParseSymbol(marketDataSymbolMapper, marketDataSymbolMapperHelper, marketDataProvider, futureRootData);
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
            return marketDataSymbolResponse;
        }

        //Update marketDataSymbolMapper dictionary after update in AUEC mapping
        public static void UpdateDictMarketDataSymbolMapper(DataSet dsMarketDataSymbolMapper)
        {
            try
            {
                if (dsMarketDataSymbolMapper != null && dsMarketDataSymbolMapper.Tables[0].Rows.Count > 0)
                {
                    DataTable dictDataTable = dsMarketDataSymbolMapper.Tables[0];
                    for (int counter = 0; counter < dictDataTable.Rows.Count; counter++)
                    {
                        if (_dictMarketDataSymbolMapper != null)
                        {
                            #region assign_values
                            MarketDataSymbolMapper marketDataSymbolMapper = new MarketDataSymbolMapper();

                            marketDataSymbolMapper.AUECID = Convert.ToInt32(dsMarketDataSymbolMapper.Tables[0].Rows[counter]["AUECID"]);
                            marketDataSymbolMapper.ExchangeIdentifier = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["ExchangeIdentifier"]).ToString().Trim();
                            marketDataSymbolMapper.ExchangeToken = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["ExchangeToken"]).ToString().Trim();
                            marketDataSymbolMapper.AssetID = Convert.ToInt32(dsMarketDataSymbolMapper.Tables[0].Rows[counter]["AssetID"]);
                            marketDataSymbolMapper.EsignalExchangeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["EsignalExchangeCode"]).ToString().Trim();
                            marketDataSymbolMapper.FactSetExchangeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["FactSetExchangeCode"]).ToString().Trim();
                            marketDataSymbolMapper.FactSetRegionCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["FactSetRegionCode"]).ToString().Trim();
                            marketDataSymbolMapper.EsignalFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["EsignalFormatString"]).ToString().Trim();
                            marketDataSymbolMapper.FactSetFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["FactSetFormatString"]).ToString().Trim();
                            marketDataSymbolMapper.ActivFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter]["ActivFormatString"]).ToString().Trim();
                            marketDataSymbolMapper.BloombergCompositeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter][BloombergSapiConstants.CONST_BLOOMBERG_COMPOSITE_CODE]).ToString().Trim();
                            marketDataSymbolMapper.BloombergExchangeCode = (dsMarketDataSymbolMapper.Tables[0].Rows[counter][BloombergSapiConstants.CONST_BLOOMBERG_EXCHANGE_CODE]).ToString().Trim();
                            marketDataSymbolMapper.BloombergFormatString = (dsMarketDataSymbolMapper.Tables[0].Rows[counter][BloombergSapiConstants.CONST_BLOOMBERG_FORMAT_STRING]).ToString().Trim();
                            #endregion

                            lock (_dictMarketDataSymbolMapperLock)
                            {
                                if (_dictMarketDataSymbolMapper.ContainsKey(marketDataSymbolMapper.AssetID + "-" + marketDataSymbolMapper.EsignalExchangeCode))
                                    _dictMarketDataSymbolMapper[marketDataSymbolMapper.AssetID + "-" + marketDataSymbolMapper.EsignalExchangeCode] = marketDataSymbolMapper;
                                else
                                    _dictMarketDataSymbolMapper.Add(marketDataSymbolMapper.AssetID + "-" + marketDataSymbolMapper.EsignalExchangeCode, marketDataSymbolMapper);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static void Setup()
        {
            try
            {
                if (_dictMarketDataSymbolMapper == null)
                {
                    _dictMarketDataSymbolMapper = SymbolDataManager.GetMarketDataSymbolMapping();
                    _monthCodeWiseOptionType = new Dictionary<string, string>();
                    _monthCodeWiseOptionType.Add("A", "C");
                    _monthCodeWiseOptionType.Add("B", "C");
                    _monthCodeWiseOptionType.Add("C", "C");
                    _monthCodeWiseOptionType.Add("D", "C");
                    _monthCodeWiseOptionType.Add("E", "C");
                    _monthCodeWiseOptionType.Add("F", "C");
                    _monthCodeWiseOptionType.Add("G", "C");
                    _monthCodeWiseOptionType.Add("H", "C");
                    _monthCodeWiseOptionType.Add("I", "C");
                    _monthCodeWiseOptionType.Add("J", "C");
                    _monthCodeWiseOptionType.Add("K", "C");
                    _monthCodeWiseOptionType.Add("L", "C");

                    _monthCodeWiseOptionType.Add("M", "P");
                    _monthCodeWiseOptionType.Add("N", "P");
                    _monthCodeWiseOptionType.Add("O", "P");
                    _monthCodeWiseOptionType.Add("P", "P");
                    _monthCodeWiseOptionType.Add("Q", "P");
                    _monthCodeWiseOptionType.Add("R", "P");
                    _monthCodeWiseOptionType.Add("S", "P");
                    _monthCodeWiseOptionType.Add("T", "P");
                    _monthCodeWiseOptionType.Add("U", "P");
                    _monthCodeWiseOptionType.Add("V", "P");
                    _monthCodeWiseOptionType.Add("W", "P");
                    _monthCodeWiseOptionType.Add("X", "P");

                    _monthCodeWiseOptionMonthNumbers = new Dictionary<string, string>();
                    _monthCodeWiseOptionMonthNumbers.Add("A", "01");
                    _monthCodeWiseOptionMonthNumbers.Add("B", "02");
                    _monthCodeWiseOptionMonthNumbers.Add("C", "03");
                    _monthCodeWiseOptionMonthNumbers.Add("D", "04");
                    _monthCodeWiseOptionMonthNumbers.Add("E", "05");
                    _monthCodeWiseOptionMonthNumbers.Add("F", "06");
                    _monthCodeWiseOptionMonthNumbers.Add("G", "07");
                    _monthCodeWiseOptionMonthNumbers.Add("H", "08");
                    _monthCodeWiseOptionMonthNumbers.Add("I", "09");
                    _monthCodeWiseOptionMonthNumbers.Add("J", "10");
                    _monthCodeWiseOptionMonthNumbers.Add("K", "11");
                    _monthCodeWiseOptionMonthNumbers.Add("L", "12");

                    _monthCodeWiseOptionMonthNumbers.Add("M", "01");
                    _monthCodeWiseOptionMonthNumbers.Add("N", "02");
                    _monthCodeWiseOptionMonthNumbers.Add("O", "03");
                    _monthCodeWiseOptionMonthNumbers.Add("P", "04");
                    _monthCodeWiseOptionMonthNumbers.Add("Q", "05");
                    _monthCodeWiseOptionMonthNumbers.Add("R", "06");
                    _monthCodeWiseOptionMonthNumbers.Add("S", "07");
                    _monthCodeWiseOptionMonthNumbers.Add("T", "08");
                    _monthCodeWiseOptionMonthNumbers.Add("U", "09");
                    _monthCodeWiseOptionMonthNumbers.Add("V", "10");
                    _monthCodeWiseOptionMonthNumbers.Add("W", "11");
                    _monthCodeWiseOptionMonthNumbers.Add("X", "12");

                    _monthCodeWiseFutureMonthNumbers = new Dictionary<string, string>();
                    _monthCodeWiseFutureMonthNumbers.Add("F", "01");
                    _monthCodeWiseFutureMonthNumbers.Add("G", "02");
                    _monthCodeWiseFutureMonthNumbers.Add("H", "03");
                    _monthCodeWiseFutureMonthNumbers.Add("J", "04");
                    _monthCodeWiseFutureMonthNumbers.Add("K", "05");
                    _monthCodeWiseFutureMonthNumbers.Add("M", "06");
                    _monthCodeWiseFutureMonthNumbers.Add("N", "07");
                    _monthCodeWiseFutureMonthNumbers.Add("Q", "08");
                    _monthCodeWiseFutureMonthNumbers.Add("U", "09");
                    _monthCodeWiseFutureMonthNumbers.Add("V", "10");
                    _monthCodeWiseFutureMonthNumbers.Add("X", "11");
                    _monthCodeWiseFutureMonthNumbers.Add("Z", "12");
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

        private static AssetCategory GetAssetCategoryUsingRegex(string symbol)
        {
            try
            {
                if (symbol.StartsWith("$"))
                {
                    return AssetCategory.Indices;
                }

                Regex regexPattern = new Regex(_regexOfFXAsset);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.FX;
                }

                regexPattern = new Regex(_regexOfEquityAsset);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.Equity;
                }

                regexPattern = new Regex(_regexOfEquityOptionAsset);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.EquityOption;
                }

                regexPattern = new Regex(_regexOfFutureAsset);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.Future;
                }

                regexPattern = new Regex(_regexOfFutureOptionAsset);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.FutureOption;
                }

                regexPattern = new Regex(_regexOfFXForwardAsset);
                if (regexPattern.IsMatch(symbol))
                {
                    return AssetCategory.FXForward;
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
            return AssetCategory.None;
        }

        private static MarketDataSymbolMapperHelper IndexTickerSymbolTransformation(string tickerSymbol)
        {
            MarketDataSymbolMapperHelper marketDataSymbolMapperHelper = new MarketDataSymbolMapperHelper();
            try
            {
                string[] symbolPart;

                int idx = tickerSymbol.LastIndexOf('-');
                if (idx != -1)
                {
                    symbolPart = tickerSymbol.Substring(0, idx).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    marketDataSymbolMapperHelper.Key = (int)AssetCategory.Indices + "-" + tickerSymbol.Substring(idx + 1);
                }
                else
                {
                    symbolPart = tickerSymbol.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    marketDataSymbolMapperHelper.Key = (int)AssetCategory.Indices + "-";
                }
                marketDataSymbolMapperHelper.Root = symbolPart[0].Replace("$", "");
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
            return marketDataSymbolMapperHelper;
        }

        private static MarketDataSymbolMapperHelper EquityTickerSymbolTransformation(string tickerSymbol)
        {
            MarketDataSymbolMapperHelper marketDataSymbolMapperHelper = new MarketDataSymbolMapperHelper();
            try
            {
                string[] symbolPart;

                int idx = tickerSymbol.LastIndexOf('-');
                if (idx != -1)
                {
                    symbolPart = tickerSymbol.Substring(0, idx).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    marketDataSymbolMapperHelper.Key = (int)AssetCategory.Equity + "-" + tickerSymbol.Substring(idx + 1);
                }
                else
                {
                    symbolPart = tickerSymbol.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    marketDataSymbolMapperHelper.Key = (int)AssetCategory.Equity + "-";
                }

                if (symbolPart.Length > 1 && !string.IsNullOrWhiteSpace(symbolPart[1]) && symbolPart[1].Equals("U"))
                {
                    marketDataSymbolMapperHelper.Root = symbolPart[0] + "/U";
                }
                else
                {
                    marketDataSymbolMapperHelper.Root = symbolPart[0];
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
            return marketDataSymbolMapperHelper;
        }

        private static MarketDataSymbolMapperHelper EquityOptionTickerSymbolTransformation(string tickerSymbol, MarketDataProvider marketDataProvider)
        {
            MarketDataSymbolMapperHelper marketDataSymbolMapperHelper = new MarketDataSymbolMapperHelper();
            try
            {
                string symbolPart;

                int idx = tickerSymbol.LastIndexOf('-');
                if (idx != -1)
                {
                    symbolPart = tickerSymbol.Substring(0, idx);
                    marketDataSymbolMapperHelper.Key = (int)AssetCategory.EquityOption + "-" + tickerSymbol.Substring(idx + 1);
                }
                else
                {
                    symbolPart = tickerSymbol;
                    marketDataSymbolMapperHelper.Key = (int)AssetCategory.EquityOption + "-";
                }

                string[] splittedData2 = symbolPart.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                string[] splittedData3 = splittedData2[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                marketDataSymbolMapperHelper.Root = splittedData3[0];
                marketDataSymbolMapperHelper.YearCode2D = splittedData3[1].Substring(0, 2);
                marketDataSymbolMapperHelper.MonthCode1D = splittedData3[1].Substring(2, 1);
                marketDataSymbolMapperHelper.MonthCode2D = _monthCodeWiseOptionMonthNumbers[marketDataSymbolMapperHelper.MonthCode1D];

                marketDataSymbolMapperHelper.OptionType = _monthCodeWiseOptionType[marketDataSymbolMapperHelper.MonthCode1D];

                string[] splittedData4 = splittedData3[1].Substring(3, splittedData3[1].Length - 3).Split(new char[] { 'D' }, StringSplitOptions.RemoveEmptyEntries);

                if (splittedData4.Length == 2)
                {
                    if (splittedData4[1].Length == 1)
                    {
                        marketDataSymbolMapperHelper.Day = "0" + splittedData4[1];
                    }
                    else
                    {
                        marketDataSymbolMapperHelper.Day = splittedData4[1];
                    }
                }
                else
                {
                    if (marketDataProvider == MarketDataProvider.SAPI)
                    {
                        int year = int.MinValue;
                        int month = int.MinValue;
                        if (int.TryParse("20" + marketDataSymbolMapperHelper.YearCode2D, out year) && int.TryParse(marketDataSymbolMapperHelper.MonthCode2D, out month))
                        {
                            DateTime expireDate = DateTimeHelper.GetNthWeekDay(3, DayOfWeek.Friday, year, month).Add(TimeSpan.FromDays(1));
                            marketDataSymbolMapperHelper.Day = expireDate.Day.ToString();
                        }
                    }
                }
                marketDataSymbolMapperHelper.StrikePrice = splittedData4[0];

                marketDataSymbolMapperHelper.StrikePrice6D = marketDataSymbolMapperHelper.StrikePrice.Replace(".", "");

                if (marketDataSymbolMapperHelper.StrikePrice6D.Length < 7)
                    marketDataSymbolMapperHelper.StrikePrice6D += "000000".Substring(0, 6 - marketDataSymbolMapperHelper.StrikePrice6D.Length);

                string[] strikePricePart = marketDataSymbolMapperHelper.StrikePrice.Split(new char[] { '.' });

                if (strikePricePart.Length == 2)
                {

                    string prefix = "00000";
                    string suffix = "000";
                    marketDataSymbolMapperHelper.StrikePrice8D = prefix.Substring(0, 5 - strikePricePart[0].Length) + strikePricePart[0] + strikePricePart[1] + suffix.Substring(0, 3 - strikePricePart[1].Length);
                }
                else
                {
                    string prefix = "00000";
                    string suffix = "000";
                    marketDataSymbolMapperHelper.StrikePrice8D = prefix.Substring(0, 5 - strikePricePart[0].Length) + strikePricePart[0] + suffix;
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
            return marketDataSymbolMapperHelper;
        }

        private static MarketDataSymbolMapperHelper FutureTickerSymbolTransformation(string tickerSymbol)
        {
            MarketDataSymbolMapperHelper marketDataSymbolMapperHelper = new MarketDataSymbolMapperHelper();
            try
            {
                string[] symbolPart;

                int idx = tickerSymbol.LastIndexOf('-');
                if (idx != -1)
                {
                    symbolPart = tickerSymbol.Substring(0, idx).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    marketDataSymbolMapperHelper.Key = (int)AssetCategory.Future + "-" + tickerSymbol.Substring(idx + 1);
                }
                else
                {
                    symbolPart = tickerSymbol.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    marketDataSymbolMapperHelper.Key = (int)AssetCategory.Future + "-";
                }

                string[] splittedData2 = symbolPart[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                marketDataSymbolMapperHelper.Root = splittedData2[0];
                marketDataSymbolMapperHelper.MonthCode1D = splittedData2[1].Substring(0, 1);
                marketDataSymbolMapperHelper.MonthCode2D = _monthCodeWiseFutureMonthNumbers[marketDataSymbolMapperHelper.MonthCode1D];

                if (splittedData2[1].Substring(1, splittedData2[1].Length - 1).Length == 2)
                {
                    marketDataSymbolMapperHelper.YearCode2D = splittedData2[1].Substring(1, splittedData2[1].Length - 1);
                }
                else
                {
                    marketDataSymbolMapperHelper.YearCode1D = splittedData2[1].Substring(1, splittedData2[1].Length - 1);
                    marketDataSymbolMapperHelper.YearCode2D = "2" + marketDataSymbolMapperHelper.YearCode1D;
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
            return marketDataSymbolMapperHelper;
        }

        private static MarketDataSymbolMapperHelper FutureOptionTickerSymbolTransformation(string tickerSymbol)
        {
            MarketDataSymbolMapperHelper marketDataSymbolMapperHelper = new MarketDataSymbolMapperHelper();
            try
            {
                string symbolPart;

                int idx = tickerSymbol.LastIndexOf('-');
                if (idx != -1)
                {
                    symbolPart = tickerSymbol.Substring(0, idx);
                    marketDataSymbolMapperHelper.Key = (int)AssetCategory.FutureOption + "-" + tickerSymbol.Substring(idx + 1);
                }
                else
                {
                    symbolPart = tickerSymbol;
                    marketDataSymbolMapperHelper.Key = (int)AssetCategory.FutureOption + "-";
                }

                string[] splittedData2 = symbolPart.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                marketDataSymbolMapperHelper.Root = splittedData2[0];
                marketDataSymbolMapperHelper.MonthCode1D = splittedData2[1].Substring(0, 1);
                marketDataSymbolMapperHelper.YearCode2D = splittedData2[1].Substring(1, 2);
                marketDataSymbolMapperHelper.OptionType = splittedData2[1].Substring(3, 1);

                int month = Convert.ToInt32(_monthCodeWiseFutureMonthNumbers[marketDataSymbolMapperHelper.MonthCode1D]) - 1;

                if (month > 0)
                {
                    marketDataSymbolMapperHelper.MonthCode2D = "00".Substring(0, 2 - month.ToString().Length) + month;
                }
                else
                {
                    marketDataSymbolMapperHelper.MonthCode2D = "12";
                    marketDataSymbolMapperHelper.YearCode2D = (Convert.ToInt32(marketDataSymbolMapperHelper.YearCode2D) - 1).ToString();
                }

                string[] splittedData3 = splittedData2[1].Substring(4, splittedData2[1].Length - 4).Split(new char[] { 'D' }, StringSplitOptions.RemoveEmptyEntries);

                if (splittedData3.Length == 2)
                {
                    marketDataSymbolMapperHelper.Day = splittedData3[1];
                }
                marketDataSymbolMapperHelper.StrikePrice = splittedData3[0];

                string[] strikePricePart = marketDataSymbolMapperHelper.StrikePrice.Split(new char[] { '.' });

                if (strikePricePart.Length == 2)
                {

                    string prefix = "00000";
                    string suffix = "000000";
                    marketDataSymbolMapperHelper.StrikePrice11D = prefix.Substring(0, 5 - strikePricePart[0].Length) + strikePricePart[0] + strikePricePart[1] + suffix.Substring(0, 6 - strikePricePart[1].Length);
                }
                else
                {
                    string prefix = "00000";
                    string suffix = "000000";
                    marketDataSymbolMapperHelper.StrikePrice11D = prefix.Substring(0, 5 - strikePricePart[0].Length) + strikePricePart[0] + suffix;
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
            return marketDataSymbolMapperHelper;
        }

        /// <summary>
        /// Creates marketDataSymbolMapperHelper from FX Ticker.
        /// </summary>
        /// <param name="tickerSymbol"></param>
        /// <returns></returns>
        private static MarketDataSymbolMapperHelper FXTickerSymbolTransformation(string tickerSymbol)
        {
            MarketDataSymbolMapperHelper marketDataSymbolMapperHelper = new MarketDataSymbolMapperHelper();
            try
            {
                marketDataSymbolMapperHelper.Key = (int)AssetCategory.FX + "-";
                string[] splittedData = tickerSymbol.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (splittedData.Length == 2)
                {
                    marketDataSymbolMapperHelper.FromCountry = splittedData[0];
                    marketDataSymbolMapperHelper.ToCountry = splittedData[1];
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
            return marketDataSymbolMapperHelper;
        }

        /// <summary>
        /// Creates marketDataSymbolMapperHelper from FX Forward Ticker.
        /// </summary>
        /// <param name="tickerSymbol"></param>
        /// <returns></returns>
        private static MarketDataSymbolMapperHelper FXForwardTickerSymbolTransformation(string tickerSymbol)
        {
            MarketDataSymbolMapperHelper marketDataSymbolMapperHelper = new MarketDataSymbolMapperHelper();
            try
            {
                marketDataSymbolMapperHelper.Key = (int)AssetCategory.FXForward + "-";
                string[] splittedData = tickerSymbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if(splittedData.Length == 2)
                {
                    string[] splittedDataCountries = splittedData[0].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if(splittedDataCountries.Length == 2)
                    {
                        marketDataSymbolMapperHelper.FromCountry = splittedDataCountries[0];
                        marketDataSymbolMapperHelper.ToCountry = splittedDataCountries[1];
                    }

                    string[] splittedDataDates = splittedData[1].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if(splittedDataDates.Length == 3)
                    {
                        marketDataSymbolMapperHelper.MonthCode2D = splittedDataDates[0];
                        marketDataSymbolMapperHelper.Day = splittedDataDates[1];
                        if(splittedDataDates[2].ToString().Length == 2)
                            marketDataSymbolMapperHelper.YearCode2D = splittedDataDates[2];
                        else if(splittedDataDates[2].ToString().Length == 4)
                            marketDataSymbolMapperHelper.YearCode2D = splittedDataDates[2].ToString().Substring(2, 2);
                    }
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
            return marketDataSymbolMapperHelper;
        }

        private static string ParseSymbol(MarketDataSymbolMapper marketDataSymbolMapper, MarketDataSymbolMapperHelper marketDataSymbolMapperHelper, MarketDataProvider marketDataProvider, FutureRootData futureRootData = null)
        {
            string marketDataSymbol = string.Empty;
            try
            {
                if (marketDataProvider == MarketDataProvider.FactSet)
                    marketDataSymbol = marketDataSymbolMapper.FactSetFormatString;
                else if (marketDataProvider == MarketDataProvider.ACTIV)
                    marketDataSymbol = marketDataSymbolMapper.ActivFormatString;
                else if (marketDataProvider == MarketDataProvider.SAPI)
                    marketDataSymbol = marketDataSymbolMapper.BloombergFormatString;

                if (!string.IsNullOrEmpty(marketDataSymbol))
                {
                    if (marketDataSymbol.Contains("{Root}"))
                    {
                        if (marketDataProvider == MarketDataProvider.SAPI && futureRootData != null)
                            marketDataSymbol = marketDataSymbol.Replace("{Root}", futureRootData.BBGRoot);
                        else
                            marketDataSymbol = marketDataSymbol.Replace("{Root}", marketDataSymbolMapperHelper.Root);
                    }

                    if (marketDataSymbol.Contains("{OCC-Symbology}"))
                    {
                        OptionDetail optionDetail = new OptionDetail();
                        optionDetail.UnderlyingSymbol = marketDataSymbolMapperHelper.Root;

                        int monthcode = 0;
                        if (_monthCodeWiseOptionMonthNumbers.ContainsKey(marketDataSymbolMapperHelper.MonthCode1D))
                        {
                            monthcode = Convert.ToInt32(_monthCodeWiseOptionMonthNumbers[marketDataSymbolMapperHelper.MonthCode1D]);
                        }

                        optionDetail.ExpirationDate = new DateTime(2000 + Convert.ToInt32(marketDataSymbolMapperHelper.YearCode2D), monthcode, Convert.ToInt32(marketDataSymbolMapperHelper.Day));
                        optionDetail.StrikePrice = Convert.ToDouble(marketDataSymbolMapperHelper.StrikePrice);

                        if (marketDataSymbolMapperHelper.OptionType.Equals("C"))
                            optionDetail.OptionType = OptionType.CALL;
                        else
                            optionDetail.OptionType = OptionType.PUT;

                        string osiSymbol = OptionSymbolGenerator.GenerateOSISymbol(optionDetail);

                        if (!string.IsNullOrWhiteSpace(osiSymbol) && osiSymbol.Length > 6)
                        {
                            marketDataSymbol = marketDataSymbol.Replace("{OCC-Symbology}", osiSymbol.Substring(0, 6).Trim() + "#" + osiSymbol.Substring(6, osiSymbol.Length - 6));
                        }
                    }

                    if (marketDataSymbol.Contains("{1D-YearCode}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{1D-YearCode}", marketDataSymbolMapperHelper.YearCode1D);
                    }

                    if (marketDataSymbol.Contains("{2D-YearCode}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{2D-YearCode}", marketDataSymbolMapperHelper.YearCode2D);
                    }

                    if (marketDataSymbol.Contains("{1D-MonthCode}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{1D-MonthCode}", marketDataSymbolMapperHelper.MonthCode1D);
                    }

                    if (marketDataSymbol.Contains("{2D-MonthCode}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{2D-MonthCode}", marketDataSymbolMapperHelper.MonthCode2D);
                    }

                    if (marketDataSymbol.Contains("{Day}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{Day}", marketDataSymbolMapperHelper.Day);
                    }

                    if (marketDataSymbol.Contains("{YYMMDD-ExpirationDate}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{YYMMDD-ExpirationDate}", marketDataSymbolMapperHelper.YearCode2D + marketDataSymbolMapperHelper.MonthCode2D + "00".Substring(0, 2 - marketDataSymbolMapperHelper.Day.Length) + marketDataSymbolMapperHelper.Day);
                    }

                    if (marketDataSymbol.Contains("{StrikePrice}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{StrikePrice}", marketDataSymbolMapperHelper.StrikePrice);
                    }

                    if (marketDataSymbol.Contains("{6D-StrikePrice}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{6D-StrikePrice}", marketDataSymbolMapperHelper.StrikePrice6D);
                    }

                    if (marketDataSymbol.Contains("{8D-StrikePrice}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{8D-StrikePrice}", marketDataSymbolMapperHelper.StrikePrice8D);
                    }

                    if (marketDataSymbol.Contains("{11D-StrikePrice}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{11D-StrikePrice}", marketDataSymbolMapperHelper.StrikePrice11D);
                    }

                    if (marketDataSymbol.Contains("{OptionType}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{OptionType}", marketDataSymbolMapperHelper.OptionType);
                    }

                    if (marketDataSymbol.Contains("{FactSetExchangeCode}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{FactSetExchangeCode}", marketDataSymbolMapper.FactSetExchangeCode);
                    }

                    if (marketDataSymbol.Contains("{FactSetRegionCode}"))
                    {
                        marketDataSymbol = marketDataSymbol.Replace("{FactSetRegionCode}", marketDataSymbolMapper.FactSetRegionCode);
                    }

                    if (marketDataSymbol.Contains(BloombergSapiConstants.CONST_SYMBOL_MAPPER_BLOOMBERG_CODE))
                    {
                        if (!string.IsNullOrWhiteSpace(marketDataSymbolMapper.BloombergCompositeCode))
                            marketDataSymbol = marketDataSymbol.Replace(BloombergSapiConstants.CONST_SYMBOL_MAPPER_BLOOMBERG_CODE, marketDataSymbolMapper.BloombergCompositeCode);

                        else if (!string.IsNullOrWhiteSpace(marketDataSymbolMapper.BloombergExchangeCode))
                            marketDataSymbol = marketDataSymbol.Replace(BloombergSapiConstants.CONST_SYMBOL_MAPPER_BLOOMBERG_CODE, marketDataSymbolMapper.BloombergExchangeCode);
                    }

                    if (marketDataSymbol.Contains(BloombergSapiConstants.CONST_SYMBOL_MAPPER_BBG_YELLOW_KEY) && futureRootData != null)
                    {
                        marketDataSymbol = marketDataSymbol.Replace(BloombergSapiConstants.CONST_SYMBOL_MAPPER_BBG_YELLOW_KEY, futureRootData.BBGYellowKey);
                    }

                    if (marketDataSymbol.Contains(BloombergSapiConstants.CONST_BLOOMBERG_FROM_COUNTRY))
                    {
                        marketDataSymbol = marketDataSymbol.Replace(BloombergSapiConstants.CONST_BLOOMBERG_FROM_COUNTRY, marketDataSymbolMapperHelper.FromCountry);
                    }

                    if (marketDataSymbol.Contains(BloombergSapiConstants.CONST_BLOOMBERG_TO_COUNTRY))
                    {
                        marketDataSymbol = marketDataSymbol.Replace(BloombergSapiConstants.CONST_BLOOMBERG_TO_COUNTRY, marketDataSymbolMapperHelper.ToCountry);
                    }

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
            return marketDataSymbol;
        }
    }
}
