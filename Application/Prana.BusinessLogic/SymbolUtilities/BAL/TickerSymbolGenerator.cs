using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.LiveFeed;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Prana.BusinessLogic.Symbol
{
    public static class TickerSymbolGenerator
    {
        private static Dictionary<string, MarketDataSymbolMapper> _dictMarketDataSymbolMapper;
        private static readonly object _dictMarketDataSymbolMapperLock = new object();
        private static Dictionary<string, string> _monthCodeWiseOptionMonthNumbers;
        private static Dictionary<string, string> _monthCodeWiseFutureMonthNumbers;
        private static Dictionary<string, MarketDataSymbolMapper> _dictMarketDataSymbolMapperExchangeIdentifier;
        private static readonly object _dictMarketDataSymbolMapperExchangeIdentifierLock = new object();
        private static bool isDictionaryInitilised = false;
        static TickerSymbolGenerator()
        {
            _monthCodeWiseOptionMonthNumbers = new Dictionary<string, string>();
            _monthCodeWiseOptionMonthNumbers.Add("C1", "A");
            _monthCodeWiseOptionMonthNumbers.Add("C2", "B");
            _monthCodeWiseOptionMonthNumbers.Add("C3", "C");
            _monthCodeWiseOptionMonthNumbers.Add("C4", "D");
            _monthCodeWiseOptionMonthNumbers.Add("C5", "E");
            _monthCodeWiseOptionMonthNumbers.Add("C6", "F");
            _monthCodeWiseOptionMonthNumbers.Add("C7", "G");
            _monthCodeWiseOptionMonthNumbers.Add("C8", "H");
            _monthCodeWiseOptionMonthNumbers.Add("C9", "I");
            _monthCodeWiseOptionMonthNumbers.Add("C10", "J");
            _monthCodeWiseOptionMonthNumbers.Add("C11", "K");
            _monthCodeWiseOptionMonthNumbers.Add("C12", "L");

            _monthCodeWiseOptionMonthNumbers.Add("P1", "M");
            _monthCodeWiseOptionMonthNumbers.Add("P2", "N");
            _monthCodeWiseOptionMonthNumbers.Add("P3", "O");
            _monthCodeWiseOptionMonthNumbers.Add("P4", "P");
            _monthCodeWiseOptionMonthNumbers.Add("P5", "Q");
            _monthCodeWiseOptionMonthNumbers.Add("P6", "R");
            _monthCodeWiseOptionMonthNumbers.Add("P7", "S");
            _monthCodeWiseOptionMonthNumbers.Add("P8", "T");
            _monthCodeWiseOptionMonthNumbers.Add("P9", "U");
            _monthCodeWiseOptionMonthNumbers.Add("P10", "V");
            _monthCodeWiseOptionMonthNumbers.Add("P11", "W");
            _monthCodeWiseOptionMonthNumbers.Add("P12", "X");

            _monthCodeWiseFutureMonthNumbers = new Dictionary<string, string>();
            _monthCodeWiseFutureMonthNumbers.Add("1", "F");
            _monthCodeWiseFutureMonthNumbers.Add("2", "G");
            _monthCodeWiseFutureMonthNumbers.Add("3", "H");
            _monthCodeWiseFutureMonthNumbers.Add("4", "J");
            _monthCodeWiseFutureMonthNumbers.Add("5", "K");
            _monthCodeWiseFutureMonthNumbers.Add("6", "M");
            _monthCodeWiseFutureMonthNumbers.Add("7", "N");
            _monthCodeWiseFutureMonthNumbers.Add("8", "Q");
            _monthCodeWiseFutureMonthNumbers.Add("9", "U");
            _monthCodeWiseFutureMonthNumbers.Add("10", "V");
            _monthCodeWiseFutureMonthNumbers.Add("11", "X");
            _monthCodeWiseFutureMonthNumbers.Add("12", "Z");

            _dictMarketDataSymbolMapperExchangeIdentifier = SymbolDataManager.GetTickerSymbolMappingExchangeIdentifier();
        }

        #region Public Methods
        public static MarketDataSymbolResponse GetTickerSymbolFromMarketData(SymbolData marketData,string bloombergFutureRoot = null)
        {
            try
            {  
                InitiliseMarketDataSymbolMapperDictionary(marketData.MarketDataProvider);
              
                switch (marketData.CategoryCode)
                {
                    case AssetCategory.Indices:
                        return IndexTickerSymbolTransformation(marketData);
                    case AssetCategory.Equity:
                        return EquityTickerSymbolTransformation(marketData);
                    case AssetCategory.EquityOption:
                        return EquityOptionTickerSymbolTransformation(marketData);
                    case AssetCategory.Future:
                        return FutureTickerSymbolTransformation(marketData, bloombergFutureRoot);
                    case AssetCategory.FutureOption:
                        return FutureOptionTickerSymbolTransformation(marketData);
                    case AssetCategory.FixedIncome:
                        return FixedIncomeTickerSymbolTransformation(marketData);
                    default:
                        if (marketData.MarketDataProvider == MarketDataProvider.FactSet)
                        {
                            string[] symbolPart = marketData.FactSetSymbol.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                            return new MarketDataSymbolResponse()
                            {
                                TickerSymbol = symbolPart[0],
                                FactSetSymbol = marketData.FactSetSymbol,
                                AUECID = 0
                            };
                        }
                        else if (marketData.MarketDataProvider == MarketDataProvider.SAPI)
                        {
                            string[] symbolPart = marketData.BloombergSymbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            return new MarketDataSymbolResponse()
                            {
                                TickerSymbol = symbolPart[0],
                                BloombergSymbol = marketData.BloombergSymbol,
                                AUECID = 0
                            };
                        }
                        else
                        {
                            string[] symbolPart = marketData.ActivSymbol.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                            return new MarketDataSymbolResponse()
                            {
                                TickerSymbol = symbolPart[0],
                                ActivSymbol = marketData.ActivSymbol,
                                AUECID = 0
                            };
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
            return null;
        }

        public static void UpdateDictMarketDataSymbolMapper(DataSet dsMarketDataSymbolMapper,MarketDataProvider marketDataProvider)
        {
            try
            {
                InitiliseMarketDataSymbolMapperDictionary(marketDataProvider);
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

        /// <summary>
        /// Updates DictMarketDataSymbolMapperExchangeIdentifier.
        /// </summary>
        /// <param name="dsMarketDataSymbolMapper"></param>
        public static void UpdateDictMarketDataSymbolMapperExchangeIdentifier(DataSet dsMarketDataSymbolMapper)
        {
            try
            {
                if (dsMarketDataSymbolMapper != null && dsMarketDataSymbolMapper.Tables[0].Rows.Count > 0)
                {
                    DataTable dictDataTable = dsMarketDataSymbolMapper.Tables[0];
                    for (int counter = 0; counter < dictDataTable.Rows.Count; counter++)
                    {
                        if (_dictMarketDataSymbolMapperExchangeIdentifier != null)
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

                            lock (_dictMarketDataSymbolMapperExchangeIdentifierLock)
                            {
                                if (_dictMarketDataSymbolMapperExchangeIdentifier.ContainsKey(marketDataSymbolMapper.ExchangeIdentifier))
                                    _dictMarketDataSymbolMapperExchangeIdentifier[marketDataSymbolMapper.ExchangeIdentifier] = marketDataSymbolMapper;
                                else
                                    _dictMarketDataSymbolMapperExchangeIdentifier.Add(marketDataSymbolMapper.ExchangeIdentifier, marketDataSymbolMapper);
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
        #endregion

        #region Private Methods
        /// <summary>
        /// to intilise dictionary _dictMarketDataSymbolMapper according to MarketDataProvider
        /// </summary>
        /// <param name="isMarketDataProviderSapi"></param>
        private static void InitiliseMarketDataSymbolMapperDictionary(MarketDataProvider marketDataProvider)
        {
            try
            {  
                bool addEsignalExchangeCode = false;
                if (marketDataProvider == MarketDataProvider.SAPI)
                { 
                    addEsignalExchangeCode = true;
                }

                if (!isDictionaryInitilised)
                {
                    _dictMarketDataSymbolMapper = SymbolDataManager.GetTickerSymbolMapping(addEsignalExchangeCode);
                    isDictionaryInitilised = true;
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
        private static MarketDataSymbolResponse IndexTickerSymbolTransformation(SymbolData marketData)
        {
            MarketDataSymbolResponse marketDataSymbolResponse = null;

            try
            {
                string[] symbolPart = null;

                if (marketData.MarketDataProvider == MarketDataProvider.FactSet && !string.IsNullOrEmpty(marketData.FactSetSymbol))
                {
                    int idx = marketData.FactSetSymbol.LastIndexOf('-');
                    if (idx != -1)
                    {
                        marketDataSymbolResponse = new MarketDataSymbolResponse();
                        marketDataSymbolResponse.FactSetSymbol = marketData.FactSetSymbol;

                        symbolPart = marketData.FactSetSymbol.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
                else if (marketData.MarketDataProvider == MarketDataProvider.ACTIV && !string.IsNullOrEmpty(marketData.ActivSymbol))
                {
                    int idx = marketData.ActivSymbol.LastIndexOf('.');
                    if (idx != -1)
                    {
                        marketDataSymbolResponse = new MarketDataSymbolResponse();
                        marketDataSymbolResponse.ActivSymbol = marketData.ActivSymbol;

                        symbolPart = marketData.ActivSymbol.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }

                if (symbolPart != null)
                {
                    string key = (int)marketData.CategoryCode + "-" + (symbolPart.Length > 1 && symbolPart[1].ToUpper() != "USA" ? symbolPart[1] : string.Empty);

                    if (_dictMarketDataSymbolMapper.ContainsKey(key))
                    {
                        marketDataSymbolResponse.TickerSymbol = _dictMarketDataSymbolMapper[key].EsignalFormatString;
                        //marketDataSymbolResponse.TickerSymbol.Replace("{Root}", symbolPart[0]);
                        marketDataSymbolResponse.AUECID = _dictMarketDataSymbolMapper[key].AUECID;
                    }
                    else
                    {
                        marketDataSymbolResponse.TickerSymbol = "$" + symbolPart[0];
                    }
                }

                if (marketDataSymbolResponse != null)
                    marketDataSymbolResponse.AssetCategory = AssetCategory.Indices;
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

            return marketDataSymbolResponse;
        }

        private static MarketDataSymbolResponse EquityTickerSymbolTransformation(SymbolData marketData)
        {
            MarketDataSymbolResponse marketDataSymbolResponse = null;

            try
            {
                string[] symbolPart = null;

                if (marketData.MarketDataProvider == MarketDataProvider.FactSet && !string.IsNullOrEmpty(marketData.FactSetSymbol))
                {
                    int idx = marketData.FactSetSymbol.LastIndexOf('-');
                    if (idx != -1)
                    {
                        marketDataSymbolResponse = new MarketDataSymbolResponse();
                        marketDataSymbolResponse.FactSetSymbol = marketData.FactSetSymbol;

                        symbolPart = marketData.FactSetSymbol.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
                else if (marketData.MarketDataProvider == MarketDataProvider.ACTIV && !string.IsNullOrEmpty(marketData.ActivSymbol))
                {
                    if (marketData.ActivSymbol.Contains(".MER-T/"))
                        return null;

                    int idx = marketData.ActivSymbol.LastIndexOf('.');
                    if (idx != -1)
                    {
                        marketDataSymbolResponse = new MarketDataSymbolResponse();
                        marketDataSymbolResponse.ActivSymbol = marketData.ActivSymbol;

                        symbolPart = marketData.ActivSymbol.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
                else if (marketData.MarketDataProvider == MarketDataProvider.SAPI && !string.IsNullOrEmpty(marketData.BloombergSymbol))
                {
                    marketDataSymbolResponse = new MarketDataSymbolResponse();
                    marketDataSymbolResponse.BloombergSymbol = marketData.BloombergSymbol;
                    string[] bloombergSymbolPart = marketData.BloombergSymbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if(bloombergSymbolPart.Length == 3)
                    {
                        string bloombergRoot = bloombergSymbolPart[0];
                        string bloombergCode = bloombergSymbolPart[1];
                        MarketDataSymbolMapper marketDataSymbolMapper = null;
                        marketDataSymbolMapper = _dictMarketDataSymbolMapperExchangeIdentifier.Values.Where(x => x.BloombergCompositeCode == bloombergCode).Select(y => y).FirstOrDefault();
                        if(marketDataSymbolMapper == null)
                            marketDataSymbolMapper = _dictMarketDataSymbolMapperExchangeIdentifier.Values.Where(x => x.BloombergExchangeCode == bloombergCode).Select(y => y).FirstOrDefault();
                        if (marketDataSymbolMapper!=null)
                            symbolPart = new string[] { bloombergRoot, marketDataSymbolMapper.EsignalExchangeCode };
                    }
                }

                if (symbolPart != null)
                {
                    string key = (int)marketData.CategoryCode + "-" + (symbolPart.Length > 1 && symbolPart[1].ToUpper() != "USA" ? symbolPart[1] : string.Empty);

                    if (_dictMarketDataSymbolMapper.ContainsKey(key))
                    {
                        marketDataSymbolResponse.TickerSymbol = _dictMarketDataSymbolMapper[key].EsignalFormatString;

                        marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{Root}", symbolPart[0]);

                        if (_dictMarketDataSymbolMapper[key].EsignalExchangeCode != null && !string.IsNullOrEmpty(_dictMarketDataSymbolMapper[key].EsignalExchangeCode))
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{EsignalExchangeCode}", _dictMarketDataSymbolMapper[key].EsignalExchangeCode);
                        else
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("-{EsignalExchangeCode}", string.Empty);

                        marketDataSymbolResponse.AUECID = _dictMarketDataSymbolMapper[key].AUECID;
                    }
                    else
                    {
                        marketDataSymbolResponse.TickerSymbol = symbolPart[0];
                        marketDataSymbolResponse.AUECID = 0;
                    }
                }

                if (marketDataSymbolResponse != null)
                    marketDataSymbolResponse.AssetCategory = AssetCategory.Equity;
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

            return marketDataSymbolResponse;
        }

        private static MarketDataSymbolResponse EquityOptionTickerSymbolTransformation(SymbolData marketData)
        {
            MarketDataSymbolResponse marketDataSymbolResponse = null;

            try
            {
                string[] symbolPart = null;

                if (marketData.MarketDataProvider == MarketDataProvider.FactSet && !string.IsNullOrEmpty(marketData.FactSetSymbol))
                {
                    int idx = marketData.FactSetSymbol.LastIndexOf('-');
                    if (idx != -1)
                    {
                        marketDataSymbolResponse = new MarketDataSymbolResponse();
                        marketDataSymbolResponse.FactSetSymbol = marketData.FactSetSymbol;

                        symbolPart = marketData.FactSetSymbol.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        symbolPart[0] = symbolPart[0].Split(new char[] { '#', '.' })[0];
                    }
                }
                else if (marketData.MarketDataProvider == MarketDataProvider.ACTIV && !string.IsNullOrEmpty(marketData.ActivSymbol))
                {
                    int idx = marketData.ActivSymbol.LastIndexOf('.');
                    if (idx != -1)
                    {
                        marketDataSymbolResponse = new MarketDataSymbolResponse();
                        marketDataSymbolResponse.ActivSymbol = marketData.ActivSymbol;

                        symbolPart = new string[] { marketData.ActivSymbol.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[0] };
                    }
                }
                else if (marketData.MarketDataProvider == MarketDataProvider.SAPI && !string.IsNullOrEmpty(marketData.BloombergSymbol))
                {
                    marketDataSymbolResponse = new MarketDataSymbolResponse();
                    marketDataSymbolResponse.BloombergSymbol = marketData.BloombergSymbol;
                    string[] bloombergSymbolPart = marketData.BloombergSymbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if(bloombergSymbolPart.Length == 5)
                    {
                        string bloombergRoot = bloombergSymbolPart[0];
                        string bloombergCode = bloombergSymbolPart[1];
                        string expiryDateStr = bloombergSymbolPart[2];
                        string putOrCall = bloombergSymbolPart[3][0].ToString();
                        string strikePrice = bloombergSymbolPart[3].Substring(1);
                        string dateFormat = "MM/dd/yy";
                        DateTime expiryDate = DateTime.MinValue;
                        if (DateTime.TryParseExact(expiryDateStr, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out expiryDate))
                        {
                            marketData.ExpirationDate = expiryDate;
                            if (putOrCall == "P")
                                marketData.PutOrCall = OptionType.PUT;
                            else
                                marketData.PutOrCall = OptionType.CALL;
                            marketData.StrikePrice = double.Parse(strikePrice);
                            MarketDataSymbolMapper marketDataSymbolMapper = null;
                            marketDataSymbolMapper = _dictMarketDataSymbolMapperExchangeIdentifier.Values.Where(x => x.BloombergCompositeCode == bloombergCode).Select(y => y).FirstOrDefault();
                            if (marketDataSymbolMapper == null)
                                marketDataSymbolMapper = _dictMarketDataSymbolMapperExchangeIdentifier.Values.Where(x => x.BloombergExchangeCode == bloombergCode).Select(y => y).FirstOrDefault();
                            if (marketDataSymbolMapper != null)
                                symbolPart = new string[] { bloombergRoot, marketDataSymbolMapper.EsignalExchangeCode };
                        }
                    }
                }

                if (symbolPart != null)
                {
                    string key = (int)marketData.CategoryCode + "-" + (symbolPart.Length > 1 && symbolPart[1].ToUpper() != "USA" ? symbolPart[1] : string.Empty);

                    if (_dictMarketDataSymbolMapper.ContainsKey(key))
                    {
                        marketDataSymbolResponse.TickerSymbol = _dictMarketDataSymbolMapper[key].EsignalFormatString;

                        marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{Root}", symbolPart[0]);
                        marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{Year}", marketData.ExpirationDate.ToString("yy"));

                        if (marketData.PutOrCall == OptionType.CALL)
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{Month}", _monthCodeWiseOptionMonthNumbers["C" + marketData.ExpirationDate.Month]);
                        else
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{Month}", _monthCodeWiseOptionMonthNumbers["P" + marketData.ExpirationDate.Month]);

                        marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{StrikePrice}", marketData.StrikePrice.ToString());
                        marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{Day}", marketData.ExpirationDate.Day.ToString());

                        if (_dictMarketDataSymbolMapper[key].EsignalExchangeCode != null && !string.IsNullOrEmpty(_dictMarketDataSymbolMapper[key].EsignalExchangeCode))
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{EsignalExchangeCode}", _dictMarketDataSymbolMapper[key].EsignalExchangeCode);
                        else
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("-{EsignalExchangeCode}", string.Empty);

                        marketDataSymbolResponse.AUECID = _dictMarketDataSymbolMapper[key].AUECID;
                    }
                    else
                    {
                        marketDataSymbolResponse.TickerSymbol = string.Format("O:{0} {1}{2}{3}D{4}{5}", symbolPart[0], marketData.ExpirationDate.ToString("yy"),
                            (marketData.PutOrCall == OptionType.CALL) ? _monthCodeWiseOptionMonthNumbers["C" + marketData.ExpirationDate.Month] : _monthCodeWiseOptionMonthNumbers["P" + marketData.ExpirationDate.Month],
                            marketData.StrikePrice.ToString("F2"), marketData.ExpirationDate.Day.ToString(), (symbolPart.Length > 1 && symbolPart[1].ToUpper() != "USA") ? '-' + symbolPart[1] : string.Empty);

                        marketDataSymbolResponse.AUECID = 1;
                    }
                }

                if (marketDataSymbolResponse != null)
                    marketDataSymbolResponse.AssetCategory = AssetCategory.EquityOption;
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

            return marketDataSymbolResponse;
        }

        private static MarketDataSymbolResponse FutureTickerSymbolTransformation(SymbolData marketData,string bloombergFutureRoot)
        {
            MarketDataSymbolResponse marketDataSymbolResponse = null;

            try
            {
                string[] symbolPart = null;
                string bloombergMonthYear = string.Empty;

                if (marketData.MarketDataProvider == MarketDataProvider.FactSet && !string.IsNullOrEmpty(marketData.FactSetSymbol))
                {
                    int idx = marketData.FactSetSymbol.LastIndexOf('-');
                    if (idx != -1)
                    {
                        marketDataSymbolResponse = new MarketDataSymbolResponse();
                        marketDataSymbolResponse.FactSetSymbol = marketData.FactSetSymbol;

                        symbolPart = marketData.FactSetSymbol.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
                else if (marketData.MarketDataProvider == MarketDataProvider.ACTIV && !string.IsNullOrEmpty(marketData.ActivSymbol))
                {
                    int idx = marketData.ActivSymbol.LastIndexOf('.');
                    if (idx != -1)
                    {
                        marketDataSymbolResponse = new MarketDataSymbolResponse();
                        marketDataSymbolResponse.ActivSymbol = marketData.ActivSymbol;

                        symbolPart = marketData.ActivSymbol.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
                else if(marketData.MarketDataProvider == MarketDataProvider.SAPI && !string.IsNullOrEmpty(marketData.BloombergSymbol) && !string.IsNullOrWhiteSpace(bloombergFutureRoot))
                {
                    marketDataSymbolResponse = new MarketDataSymbolResponse();
                    marketDataSymbolResponse.BloombergSymbol = marketData.BloombergSymbol;
                    string[] bloombergSymbolPart = marketData.BloombergSymbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if(bloombergSymbolPart.Length == 2)
                    {
                        if (bloombergSymbolPart[0].Length > 3)
                        {
                            bloombergMonthYear = bloombergSymbolPart[0].Substring(bloombergSymbolPart[0].Length - 3);
                            symbolPart = new string[] { bloombergFutureRoot };
                        }
                    }
                }

                if (symbolPart != null)
                {
                    string key = (int)marketData.CategoryCode + "-" + (symbolPart.Length > 1 && symbolPart[1].ToUpper() != "USA" ? symbolPart[1] : string.Empty);

                    if (_dictMarketDataSymbolMapper.ContainsKey(key))
                    {
                        marketDataSymbolResponse.TickerSymbol = _dictMarketDataSymbolMapper[key].EsignalFormatString;

                        marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{Root}", symbolPart[0]);

                        string monthYear = string.Empty;
                        if (!string.IsNullOrEmpty(bloombergMonthYear))
                            monthYear = bloombergMonthYear;
                        else
                        {
                            monthYear = _monthCodeWiseFutureMonthNumbers[marketData.ExpirationDate.Month.ToString()];
                            monthYear += marketData.ExpirationDate.ToString("yy");
                        }

                        if (marketDataSymbolResponse.TickerSymbol.Contains(monthYear))
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace(monthYear, string.Empty);

                        marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{Month}{Year}", monthYear);

                        if (_dictMarketDataSymbolMapper[key].EsignalExchangeCode != null && !string.IsNullOrEmpty(_dictMarketDataSymbolMapper[key].EsignalExchangeCode))
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{EsignalExchangeCode}", _dictMarketDataSymbolMapper[key].EsignalExchangeCode);
                        else
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("-{EsignalExchangeCode}", string.Empty);

                        marketDataSymbolResponse.AUECID = _dictMarketDataSymbolMapper[key].AUECID;
                    }
                    else
                    {
                        marketDataSymbolResponse.TickerSymbol = symbolPart[0];

                        if (marketDataSymbolResponse.TickerSymbol.Contains(_monthCodeWiseFutureMonthNumbers[marketData.ExpirationDate.Month.ToString()] + marketData.ExpirationDate.ToString("yy")))
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace(_monthCodeWiseFutureMonthNumbers[marketData.ExpirationDate.Month.ToString()] + marketData.ExpirationDate.ToString("yy"), string.Empty);

                        marketDataSymbolResponse.TickerSymbol += string.Format(" {0}{1}{2}", _monthCodeWiseFutureMonthNumbers[marketData.ExpirationDate.Month.ToString()],
                            marketData.ExpirationDate.ToString("yy"), (symbolPart.Length > 1 && symbolPart[1].ToUpper() != "USA") ? '-' + symbolPart[1] : string.Empty);

                        marketDataSymbolResponse.AUECID = 1;
                    }
                }

                if (marketDataSymbolResponse != null)
                    marketDataSymbolResponse.AssetCategory = AssetCategory.Future;
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

            return marketDataSymbolResponse;
        }

        private static MarketDataSymbolResponse FutureOptionTickerSymbolTransformation(SymbolData marketData)
        {
            MarketDataSymbolResponse marketDataSymbolResponse = null;

            try
            {
                string[] symbolPart = null;

                if (marketData.MarketDataProvider == MarketDataProvider.FactSet && !string.IsNullOrEmpty(marketData.FactSetSymbol))
                {
                    int idx = marketData.FactSetSymbol.LastIndexOf('-');
                    if (idx != -1)
                    {
                        marketDataSymbolResponse = new MarketDataSymbolResponse();
                        marketDataSymbolResponse.FactSetSymbol = marketData.FactSetSymbol;

                        symbolPart = marketData.FactSetSymbol.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        symbolPart[0] = symbolPart[0].Split(new char[] { '#', '.' })[0];
                    }
                }
                else if (marketData.MarketDataProvider == MarketDataProvider.ACTIV && !string.IsNullOrEmpty(marketData.ActivSymbol))
                {
                    int idx = marketData.ActivSymbol.LastIndexOf('.');
                    if (idx != -1)
                    {
                        marketDataSymbolResponse = new MarketDataSymbolResponse();
                        marketDataSymbolResponse.ActivSymbol = marketData.ActivSymbol;

                        symbolPart = marketData.ActivSymbol.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }

                if (symbolPart != null)
                {
                    string key = (int)marketData.CategoryCode + "-" + (symbolPart.Length > 1 && symbolPart[1].ToUpper() != "USA" ? symbolPart[1] : string.Empty);

                    if (_dictMarketDataSymbolMapper.ContainsKey(key))
                    {
                        marketDataSymbolResponse.TickerSymbol = _dictMarketDataSymbolMapper[key].EsignalFormatString;

                        marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{Root}", symbolPart[0]);

                        string monthYear = string.Empty;
                        if (marketData.PutOrCall == OptionType.CALL)
                        {
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{OptionType}", "C");
                            monthYear = _monthCodeWiseFutureMonthNumbers[marketData.ExpirationDate.Month.ToString()];
                        }
                        else
                        {
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{OptionType}", "P");
                            monthYear = _monthCodeWiseFutureMonthNumbers[marketData.ExpirationDate.Month.ToString()];
                        }

                        monthYear += marketData.ExpirationDate.ToString("yy");

                        if (marketDataSymbolResponse.TickerSymbol.Contains(monthYear))
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace(monthYear, string.Empty);

                        marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{Month}{Year}", monthYear);
                        marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{StrikePrice}", marketData.StrikePrice.ToString());
                        marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{Day}", marketData.ExpirationDate.Day.ToString());

                        if (_dictMarketDataSymbolMapper[key].EsignalExchangeCode != null && !string.IsNullOrEmpty(_dictMarketDataSymbolMapper[key].EsignalExchangeCode))
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("{EsignalExchangeCode}", _dictMarketDataSymbolMapper[key].EsignalExchangeCode);
                        else
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace("-{EsignalExchangeCode}", string.Empty);

                        marketDataSymbolResponse.AUECID = _dictMarketDataSymbolMapper[key].AUECID;
                    }
                    else
                    {
                        marketDataSymbolResponse.TickerSymbol = symbolPart[0];

                        if (marketDataSymbolResponse.TickerSymbol.Contains(_monthCodeWiseFutureMonthNumbers[marketData.ExpirationDate.Month.ToString()] + marketData.ExpirationDate.ToString("yy")))
                            marketDataSymbolResponse.TickerSymbol = marketDataSymbolResponse.TickerSymbol.Replace(_monthCodeWiseFutureMonthNumbers[marketData.ExpirationDate.Month.ToString()] + marketData.ExpirationDate.ToString("yy"), string.Empty);

                        marketDataSymbolResponse.TickerSymbol += string.Format(" {0}{1}{2}{3}D{4}{5}", _monthCodeWiseFutureMonthNumbers[marketData.ExpirationDate.Month.ToString()],
                            marketData.ExpirationDate.ToString("yy"), (marketData.PutOrCall == OptionType.CALL) ? 'C' : 'P', marketData.StrikePrice.ToString(), marketData.ExpirationDate.Day.ToString(),
                            (symbolPart.Length > 1 && symbolPart[1].ToUpper() != "USA") ? '-' + symbolPart[1] : string.Empty);

                        marketDataSymbolResponse.AUECID = 1;
                    }
                }

                if (marketDataSymbolResponse != null)
                    marketDataSymbolResponse.AssetCategory = AssetCategory.FutureOption;
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

            return marketDataSymbolResponse;
        }

        private static MarketDataSymbolResponse FixedIncomeTickerSymbolTransformation(SymbolData marketData)
        {
            try
            {
                MarketDataSymbolResponse marketDataSymbolResponse = new MarketDataSymbolResponse();
                marketDataSymbolResponse.TickerSymbol = marketData.CusipNo;

                string key = (int)marketData.CategoryCode + "-OTC";
                if (_dictMarketDataSymbolMapper.ContainsKey(key))
                {
                    marketDataSymbolResponse.AUECID = _dictMarketDataSymbolMapper[key].AUECID;
                }
                else
                {
                    marketDataSymbolResponse.AUECID = 0;
                }

                if (marketData.MarketDataProvider == MarketDataProvider.FactSet)
                {
                    marketDataSymbolResponse.FactSetSymbol = marketData.FactSetSymbol;
                }
                else
                {
                    marketDataSymbolResponse.ActivSymbol = marketData.ActivSymbol;
                }

                if (marketDataSymbolResponse != null)
                    marketDataSymbolResponse.AssetCategory = AssetCategory.FixedIncome;

                return marketDataSymbolResponse;
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
            return null;
        }
        #endregion
    }
}
