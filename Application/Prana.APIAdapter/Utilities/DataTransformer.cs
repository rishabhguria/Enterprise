
using Newtonsoft.Json;
using Prana.APIAdapter.Interfaces;
using Prana.APIAdapter.Models;
using Prana.APIAdapter.Sessions;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Prana.APIAdapter.Utilities
{
    ///<inheritdoc cref="IDataTransformer"/>
    internal class DataTransformer : IDataTransformer
    {

        ///<inheritdoc/>
        public async Task<List<SymbolData>> JsonStringToSymbolData(string data)
        {
            try
            {
                var dictSymbologySymbols = new ConcurrentDictionary<ApplicationConstants.SymbologyCodes, List<string>>();

                Dictionary<string, SymbolData> pricesDict = new Dictionary<string, SymbolData>();

                var obj = JsonConvert.DeserializeObject<Prices>(data);

                //TOD0 Implement the config mapping
                // var config = SessionManager.Instance.PricingColumnMappingConfig;
                //var pricesList = config.CreateMapper().Map<List<InstrumentPrice>, List<SymbolData>>(obj.prices);

                ConcurrentDictionary<string, string> _dictSymbolsReverse = SessionManager.Instance.GetSymbolTickerDictionary();
                ConcurrentDictionary<string, SecMasterBaseObj> _dictSymbolsUnderlyingSymbol = SessionManager.Instance.GetSymbolsUnderlyingSymbolDictionary();

                List<SymbolData> pricesList = obj.prices.Select(p =>
                {
                    var securityType = !string.IsNullOrEmpty(p.SecurityType) ? p.SecurityType : "EQT";
                    var underlyingSymbol = string.Empty;

                    SymbolData symbolData = new SymbolData();
                    AssetCategory assetCategoryCode = AssetCategory.Equity;
                    switch (securityType.ToUpper())
                    {
                        case "EQT":
                            assetCategoryCode = AssetCategory.Equity;
                            underlyingSymbol = !string.IsNullOrEmpty(p.Symbol) ? p.Symbol.Trim() : string.Empty;
                            symbolData = new EquitySymbolData();
                            break;

                        case "BOND":
                            assetCategoryCode = AssetCategory.FixedIncome;
                            underlyingSymbol = !string.IsNullOrEmpty(p.Symbol) ? p.Symbol.Trim() : string.Empty;
                            symbolData = new FixedIncomeSymbolData();
                            break;

                        case "OPT":
                            assetCategoryCode = AssetCategory.EquityOption;
                            symbolData = new OptionSymbolData();
                            symbolData.OSIOptionSymbol = !string.IsNullOrEmpty(p.Symbol) ? p.Symbol.Trim() : string.Empty;
                            break;

                        case "FUT":
                            assetCategoryCode = AssetCategory.Future;
                            underlyingSymbol = !string.IsNullOrEmpty(p.Symbol) ? GetSymbolForFuture(p.Symbol) : string.Empty;
                            symbolData = new FutureSymbolData();
                            break;

                        case "OPTFUT":
                            assetCategoryCode = AssetCategory.FutureOption;
                            underlyingSymbol = !string.IsNullOrEmpty(p.Symbol) ? GetUnderlyingSymbolForFutureOption(p.Symbol) : string.Empty;
                            symbolData = new OptionSymbolData();
                            break;

                        case "CASH":
                            assetCategoryCode = AssetCategory.FX;
                            underlyingSymbol = !string.IsNullOrEmpty(p.Symbol) ? p.Symbol.Trim() : string.Empty;
                            symbolData = new FxSymbolData();
                            break;

                        default:
                            assetCategoryCode = AssetCategory.Equity;
                            underlyingSymbol = !string.IsNullOrEmpty(p.Symbol) ? p.Symbol.Trim() : string.Empty;
                            symbolData = new EquitySymbolData();
                            break;

                    }

                    symbolData.Symbol = GetFormattedSymbol(p, assetCategoryCode);
                    symbolData.UnderlyingSymbol = underlyingSymbol;
                    symbolData.CusipNo = !string.IsNullOrEmpty(p.Cusip) ? p.Cusip.Trim() : string.Empty;
                    symbolData.SedolSymbol = !string.IsNullOrEmpty(p.Sedol) ? p.Sedol.Trim() : string.Empty;
                    symbolData.ISIN = !string.IsNullOrEmpty(p.Isin) ? p.Isin.Trim() : string.Empty;
                    symbolData.Bid = p.Bid > 0 ? p.Bid : p.Close;
                    symbolData.Ask = p.Ask > 0 ? p.Ask : p.Close;
                    symbolData.LastPrice = p.Last > 0 ? p.Last : p.Close;
                    symbolData.SelectedFeedPrice = p.Last > 0 ? p.Last : p.Close;
                    symbolData.High = p.Best;
                    symbolData.Previous = p.Close;
                    symbolData.VWAP = p.Vwap;
                    symbolData.CurencyCode = !string.IsNullOrEmpty(p.Currency) ? p.Currency : "USD";
                    symbolData.FullCompanyName = p.Name;
                    symbolData.PricingSource = BusinessObjects.AppConstants.PricingSource.LiveFeed;
                    symbolData.CategoryCode = assetCategoryCode;
                    symbolData.OpraSymbol = !string.IsNullOrEmpty(p.SecurityType) && p.SecurityType.Equals("OPT", StringComparison.OrdinalIgnoreCase) ? p.Symbol : string.Empty;
                    symbolData.RequestedSymbology = GetRequestedSymbology(p, _dictSymbolsReverse, dictSymbologySymbols);
                    symbolData.UpdateTime = DateTime.UtcNow;
                    symbolData.ExpirationDate = DateTime.MinValue;
                    symbolData.MarketDataProvider = MarketDataProvider.API;
                    return symbolData;
                }).ToList();

                if (dictSymbologySymbols.Count > 0)
                {

                    var isUpdated = await SessionManager.Instance.UpdateSymbolTickerDictionary(dictSymbologySymbols);
                }

                var updatedPricesData = pricesList.Select(x =>
                {
                    GetSymbolBySymbology(x, _dictSymbolsReverse);

                    //update option related Details
                    if (x.CategoryCode.Equals(AssetCategory.EquityOption) || x.CategoryCode.Equals(AssetCategory.FutureOption))
                        UpdateSymbolsDetails(x, _dictSymbolsUnderlyingSymbol);
                    return x;
                }).ToList();


                return updatedPricesData;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Get Formatted Symbol
        /// </summary>
        /// <param name="p"></param>
        /// <param name="assetCategoryCode"></param>
        /// <returns></returns>
        private string GetFormattedSymbol(InstrumentPrice priceData, AssetCategory assetCategoryCode)
        {
            var symbol = priceData.Symbol.Trim();
            try
            {

                switch (assetCategoryCode)
                {
                    case AssetCategory.Future:
                        symbol = GetSymbolForFuture(priceData.Symbol);
                        break;
                    case AssetCategory.FutureOption:
                        symbol = GetSymbolForFutureOption(priceData.Symbol);
                        break;
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
            return symbol;
        }

        /// <summary>
        /// Get Symbol For Future Option
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetUnderlyingSymbolForFutureOption(string symbol)
        {
            var futureOptionSymbol = symbol;
            try
            {
                // SCK0 P2835 ->SC K20P2740
                if (symbol.Length >= 3)
                {
                    if (symbol.IndexOf(" ") > 0)
                    {
                        int currentYearTenthDigit = Convert.ToInt32(DateTime.Now.ToString("yy")) / 10;
                        var indexOfSpace = symbol.IndexOf(" ");
                        var callPut = symbol.Substring(indexOfSpace + 1, 1);
                        var strickPrice = symbol.Substring(indexOfSpace + 2);
                        var year = symbol.Substring(indexOfSpace - 1, 1);
                        var root = symbol.Substring(0, indexOfSpace - 2);
                        var month = symbol.Substring(indexOfSpace - 2, 1);

                        futureOptionSymbol = root + " " + month + currentYearTenthDigit + year;
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
            return futureOptionSymbol;
        }

        /// <summary>
        /// Get Symbol For Future Option
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetSymbolForFutureOption(string symbol)
        {
            var futureOptionSymbol = symbol;
            try
            {
                // SCK0 P2835 ->SC K20P2740
                if (symbol.Length >= 3)
                {
                    if (symbol.IndexOf(" ") > 0)
                    {
                        int currentYearTenthDigit = Convert.ToInt32(DateTime.Now.ToString("yy")) / 10;
                        var indexOfSpace = symbol.IndexOf(" ");
                        var callPut = symbol.Substring(indexOfSpace + 1, 1);
                        var strickPrice = symbol.Substring(indexOfSpace + 2);
                        var year = symbol.Substring(indexOfSpace - 1, 1);
                        var root = symbol.Substring(0, indexOfSpace - 2);
                        var month = symbol.Substring(indexOfSpace - 2, 1);

                        futureOptionSymbol = root + " " + month + currentYearTenthDigit + year + callPut + strickPrice;
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
            return futureOptionSymbol;
        }

        /// <summary>
        /// Get Symbol For Future 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private string GetSymbolForFuture(string symbol)
        {
            var futureSymbol = symbol;
            try
            {

                if (symbol.Length >= 3)
                {

                    int currentYearTenthDigit = Convert.ToInt32(DateTime.Now.ToString("yy")) / 10;
                    var root = symbol.Substring(0, symbol.Length - 2);
                    var month = symbol.Substring(symbol.Length - 2, 1);
                    var year = symbol.Substring(symbol.Length - 1);

                    futureSymbol = root + " " + month + currentYearTenthDigit + year;
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
            return futureSymbol;
        }

        /// <summary>
        /// Update Symbols Details
        /// </summary>
        /// <param name="symbolData"></param>
        /// <param name="_dictSymbolsUnderlyingSymbol"></param>
        private void UpdateSymbolsDetails(SymbolData symbolData, ConcurrentDictionary<string, SecMasterBaseObj> _dictSymbolsUnderlyingSymbol)
        {
            try
            {
                if (_dictSymbolsUnderlyingSymbol.Count > 0)
                {
                    if (!string.IsNullOrEmpty(symbolData.OpraSymbol) && _dictSymbolsUnderlyingSymbol.ContainsKey(symbolData.OpraSymbol) && _dictSymbolsUnderlyingSymbol[symbolData.OpraSymbol] != null)
                    {
                        var symbolDetails = _dictSymbolsUnderlyingSymbol[symbolData.OpraSymbol] as SecMasterOptObj;
                        if (symbolDetails != null)
                        {
                            symbolData.UnderlyingSymbol = symbolDetails.UnderLyingSymbol;
                            symbolData.StrikePrice = symbolDetails.StrikePrice;
                            symbolData.ExpirationDate = symbolDetails.ExpirationDate;
                            symbolData.PutOrCall = (OptionType)symbolDetails.PutOrCall;
                        }

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
        }

        /// <summary>
        /// Get Symbol By Symbology
        /// </summary>
        /// <param name="symbolData"></param>
        /// <param name="dictSymbolsReverse"></param>
        private void GetSymbolBySymbology(SymbolData symbolData, ConcurrentDictionary<string, string> dictSymbolsReverse)
        {

            try
            {
                if (dictSymbolsReverse.Count > 0)
                {
                    var cusipKey = !string.IsNullOrEmpty(symbolData.CusipNo) ? symbolData.CusipNo + Seperators.SEPERATOR_5 + symbolData.CurencyCode.ToUpper() : string.Empty;
                    var isinKey = !string.IsNullOrEmpty(symbolData.ISIN) ? symbolData.ISIN + Seperators.SEPERATOR_5 + symbolData.CurencyCode.ToUpper() : string.Empty;
                    var sedolKey = !string.IsNullOrEmpty(symbolData.SedolSymbol) ? symbolData.SedolSymbol + Seperators.SEPERATOR_5 + symbolData.CurencyCode.ToUpper() : string.Empty;
                    var symbolKey = !string.IsNullOrEmpty(symbolData.Symbol) ? symbolData.Symbol + Seperators.SEPERATOR_5 + symbolData.CurencyCode.ToUpper() : string.Empty;

                    if (!string.IsNullOrEmpty(symbolKey) && dictSymbolsReverse.ContainsKey(symbolKey) && !string.IsNullOrEmpty(dictSymbolsReverse[symbolKey]))
                    {
                        symbolData.Symbol = dictSymbolsReverse[symbolKey];
                        symbolData.RequestedSymbology = ApplicationConstants.SymbologyCodes.TickerSymbol;

                    }
                    else if (!string.IsNullOrEmpty(sedolKey) && dictSymbolsReverse.ContainsKey(sedolKey) && !string.IsNullOrEmpty(dictSymbolsReverse[sedolKey]))
                    {
                        symbolData.Symbol = dictSymbolsReverse[sedolKey];
                        symbolData.RequestedSymbology = ApplicationConstants.SymbologyCodes.SEDOLSymbol;
                    }
                    else if (!string.IsNullOrEmpty(cusipKey) && dictSymbolsReverse.ContainsKey(cusipKey) && !string.IsNullOrEmpty(dictSymbolsReverse[cusipKey]))
                    {
                        symbolData.Symbol = dictSymbolsReverse[cusipKey];
                        symbolData.RequestedSymbology = ApplicationConstants.SymbologyCodes.CUSIPSymbol;
                    }
                    else if (!string.IsNullOrEmpty(isinKey) && dictSymbolsReverse.ContainsKey(isinKey) && !string.IsNullOrEmpty(dictSymbolsReverse[isinKey]))
                    {
                        symbolData.Symbol = dictSymbolsReverse[isinKey];
                        symbolData.RequestedSymbology = ApplicationConstants.SymbologyCodes.ISINSymbol;
                    }
                    else if (!string.IsNullOrEmpty(symbolData.Symbol) && dictSymbolsReverse.ContainsKey(symbolData.Symbol) && !string.IsNullOrEmpty(dictSymbolsReverse[symbolData.Symbol]))
                    {
                        symbolData.Symbol = dictSymbolsReverse[symbolData.Symbol];

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
        }


        /// <summary>
        /// Get Requested Symbology
        /// </summary>
        /// <param name="intrument"></param>
        /// <param name="_dictSymbolsReverse"></param>
        /// <returns></returns>
        private Global.ApplicationConstants.SymbologyCodes GetRequestedSymbology(InstrumentPrice intrument, ConcurrentDictionary<string, string> dictSymbolsReverse, ConcurrentDictionary<ApplicationConstants.SymbologyCodes, List<string>> dictSymbologySymbols)
        {
            Prana.Global.ApplicationConstants.SymbologyCodes symbology = Prana.Global.ApplicationConstants.SymbologyCodes.TickerSymbol;
            try
            {

                var symbol = string.Empty;

                if (!string.IsNullOrEmpty(intrument.SecurityType) && intrument.SecurityType.Equals("OPT", StringComparison.OrdinalIgnoreCase))
                {
                    symbology = Global.ApplicationConstants.SymbologyCodes.OSIOptionSymbol;
                    symbol = intrument.Symbol;
                    if (!dictSymbolsReverse.ContainsKey(symbol))
                        GetSymbologyWiseNewSymbols(symbology, symbol, intrument.Currency, dictSymbologySymbols);

                }
                else
                {
                    if (!string.IsNullOrEmpty(intrument.Symbol))
                    {
                        symbology = Global.ApplicationConstants.SymbologyCodes.TickerSymbol;
                        symbol = intrument.Symbol + Seperators.SEPERATOR_5 + intrument.Currency.ToUpper();
                        if (!dictSymbolsReverse.ContainsKey(symbol))
                            GetSymbologyWiseNewSymbols(symbology, symbol, intrument.Currency, dictSymbologySymbols);

                    }
                    if (!string.IsNullOrEmpty(intrument.Sedol))
                    {
                        symbology = Global.ApplicationConstants.SymbologyCodes.SEDOLSymbol;
                        symbol = intrument.Sedol + Seperators.SEPERATOR_5 + intrument.Currency.ToUpper();
                        if (!dictSymbolsReverse.ContainsKey(symbol))
                            GetSymbologyWiseNewSymbols(symbology, symbol, intrument.Currency, dictSymbologySymbols);

                    }
                    if (!string.IsNullOrEmpty(intrument.Cusip))
                    {
                        symbology = Global.ApplicationConstants.SymbologyCodes.CUSIPSymbol;
                        symbol = intrument.Cusip.Trim() + Seperators.SEPERATOR_5 + intrument.Currency.Trim().ToUpper();
                        if (!dictSymbolsReverse.ContainsKey(symbol))
                            GetSymbologyWiseNewSymbols(symbology, symbol, intrument.Currency, dictSymbologySymbols);

                    }
                    if (!string.IsNullOrEmpty(intrument.Isin))
                    {
                        symbology = Global.ApplicationConstants.SymbologyCodes.ISINSymbol;
                        symbol = intrument.Isin + Seperators.SEPERATOR_5 + intrument.Currency.ToUpper();
                        if (!dictSymbolsReverse.ContainsKey(symbol))
                            GetSymbologyWiseNewSymbols(symbology, symbol, intrument.Currency, dictSymbologySymbols);

                    }
                }
                //if (!symbology.Equals(Prana.Global.ApplicationConstants.SymbologyCodes.TickerSymbol) && !dictSymbolsReverse.ContainsKey(symbol))
                //    GetSymbologyWiseNewSymbols(symbology, symbol, intrument.Currency, dictSymbologySymbols);


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
            return symbology;
        }

        /// <summary>
        /// Get Symbology Wise New Symbols
        /// </summary>
        /// <param name="symbology"></param>
        /// <param name="symbol"></param>
        private void GetSymbologyWiseNewSymbols(Global.ApplicationConstants.SymbologyCodes symbology, string symbol, string currency, ConcurrentDictionary<ApplicationConstants.SymbologyCodes, List<string>> dictSymbologySymbols)
        {
            try
            {
                var currencyNew = !string.IsNullOrEmpty(currency) ? currency.ToUpper() : "USD";
                if (!dictSymbologySymbols.ContainsKey(symbology))
                {

                    dictSymbologySymbols.TryAdd(symbology, new List<string> { symbol });
                }
                else
                {
                    if (!dictSymbologySymbols[symbology].Contains(symbol))
                    {
                        dictSymbologySymbols[symbology].Add(symbol);
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
        }

        /// <summary>
        /// Get Normalised Price 
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="price"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="ExchangeRateDisplayMultiplier"></param>
        /// <returns></returns>
        private double GetNormalisedPrice(string currency, double price, double exchangeRate, int ExchangeRateDisplayMultiplier)
        {
            double priceAdjusted = price;
            try
            {

                if (!string.IsNullOrEmpty(currency) && currency != "USD")
                {
                    //check currency last char is lower
                    if (currency.Substring(currency.Length - 1).Any(char.IsLower) && ExchangeRateDisplayMultiplier > 0)
                    {
                        price = price * ExchangeRateDisplayMultiplier;
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
            return priceAdjusted;

        }



        ///<inheritdoc/>
        public Task<List<SymbolData>> XMLStringToSymbolData(string data)
        {
            throw new NotImplementedException();
        }
    }
}
