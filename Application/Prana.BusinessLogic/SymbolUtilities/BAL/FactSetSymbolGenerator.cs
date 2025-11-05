using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace Prana.BusinessLogic.Symbol
{
    public static class FactSetSymbolGenerator
    {
        private static Dictionary<string, FactSetSymbolMapper> _dictFactSetSymbolMapper;
        private static Dictionary<string, string> _monthCodeWiseOptionType;
        private static Dictionary<string, string> _monthCodeWiseOptionMonthNumbers;
        private static Dictionary<string, string> _monthCodeWiseFutureMonthNumbers;

        private static string _regexOfEquityAsset = ConfigurationManager.AppSettings["RegexOfEquityAsset"];
        private static string _regexOfEquityOptionAsset = ConfigurationManager.AppSettings["RegexOfEquityOptionAsset"];
        private static string _regexOfFutureAsset = ConfigurationManager.AppSettings["RegexOfFutureAsset"];
        private static string _regexOfFutureOptionAsset = ConfigurationManager.AppSettings["RegexOfFutureOptionAsset"];

        public static FactSetSymbolResponse GetFactSetSymbolFromTickerSymbol(FactSetSymbolResponse factSetSymbolResponse)
        {
            try
            {
                Setup();

                if (factSetSymbolResponse.AUECID == 0)
                {
                    factSetSymbolResponse.AssetCategory = GetAssetCategoryUsingRegex(factSetSymbolResponse.TickerSymbol);
                }

                FactSetSymbolMapperHelper factSetSymbolMapperHelper = null;
                switch (factSetSymbolResponse.AssetCategory)
                {
                    case AssetCategory.Indices:
                        factSetSymbolMapperHelper = IndexTickerSymbolTransformation(factSetSymbolResponse.TickerSymbol);
                        break;
                    case AssetCategory.Equity:
                        factSetSymbolMapperHelper = EquityTickerSymbolTransformation(factSetSymbolResponse.TickerSymbol);
                        break;
                    case AssetCategory.EquityOption:
                        factSetSymbolMapperHelper = EquityOptionTickerSymbolTransformation(factSetSymbolResponse.TickerSymbol);
                        break;
                    case AssetCategory.Future:
                        factSetSymbolMapperHelper = FutureTickerSymbolTransformation(factSetSymbolResponse.TickerSymbol);
                        break;
                    case AssetCategory.FutureOption:
                        factSetSymbolMapperHelper = FutureOptionTickerSymbolTransformation(factSetSymbolResponse.TickerSymbol);
                        break;
                }

                if (factSetSymbolMapperHelper != null && _dictFactSetSymbolMapper.ContainsKey(factSetSymbolMapperHelper.Key))
                {
                    FactSetSymbolMapper factSetSymbolMapper = _dictFactSetSymbolMapper[factSetSymbolMapperHelper.Key];

                    if (factSetSymbolResponse.AUECID == 0)
                    {
                        factSetSymbolResponse.AUECID = factSetSymbolMapper.AUECID;
                    }

                    factSetSymbolResponse.FactSetSymbol = ParseSymbol(factSetSymbolMapper, factSetSymbolMapperHelper);
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
            return factSetSymbolResponse;
        }

        //Update factSetSymbolMapper dictionary after update in AUEC mapping
        public static void UpdateDictFactSetSymbolMapper(DataSet dsFactSetSymbolMapper)
        {
            try
            {
                if (dsFactSetSymbolMapper != null && dsFactSetSymbolMapper.Tables[0].Rows.Count > 0)
                {
                    DataTable dictDataTable = dsFactSetSymbolMapper.Tables[0];
                    for (int counter = 0; counter < dictDataTable.Rows.Count; counter++)
                    {
                        if (_dictFactSetSymbolMapper != null)
                        {
                            #region assign_values
                            FactSetSymbolMapper factSetSymbolMapper = new FactSetSymbolMapper();

                            factSetSymbolMapper.AUECID = Convert.ToInt32(dsFactSetSymbolMapper.Tables[0].Rows[counter]["AUECID"]);
                            factSetSymbolMapper.ExchangeIdentifier = (dsFactSetSymbolMapper.Tables[0].Rows[counter]["ExchangeIdentifier"]).ToString().Trim();
                            factSetSymbolMapper.ExchangeToken = (dsFactSetSymbolMapper.Tables[0].Rows[counter]["ExchangeToken"]).ToString().Trim();
                            factSetSymbolMapper.AssetID = Convert.ToInt32(dsFactSetSymbolMapper.Tables[0].Rows[counter]["AssetID"]);
                            factSetSymbolMapper.FactSetFormatString = (dsFactSetSymbolMapper.Tables[0].Rows[counter]["FactSetFormatString"]).ToString().Trim();
                            factSetSymbolMapper.FactSetRegionCode = (dsFactSetSymbolMapper.Tables[0].Rows[counter]["FactSetRegionCode"]).ToString().Trim();
                            factSetSymbolMapper.EsignalExchangeCode = (dsFactSetSymbolMapper.Tables[0].Rows[counter]["EsignalExchangeCode"]).ToString().Trim();
                            factSetSymbolMapper.FactSetExchangeCode = (dsFactSetSymbolMapper.Tables[0].Rows[counter]["FactSetExchangeCode"]).ToString().Trim();
                            #endregion

                            lock (_dictFactSetSymbolMapper)
                            {
                                if (_dictFactSetSymbolMapper.ContainsKey(factSetSymbolMapper.AssetID + "-" + factSetSymbolMapper.EsignalExchangeCode))
                                    _dictFactSetSymbolMapper[factSetSymbolMapper.AssetID + "-" + factSetSymbolMapper.EsignalExchangeCode] = factSetSymbolMapper;
                                else
                                    _dictFactSetSymbolMapper.Add(factSetSymbolMapper.AssetID + "-" + factSetSymbolMapper.EsignalExchangeCode, factSetSymbolMapper);
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
                if (_dictFactSetSymbolMapper == null)
                {
                    _dictFactSetSymbolMapper = SymbolDataManager.GetFactSetSymbolMapping();
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

                Regex regexPattern = new Regex(_regexOfEquityAsset);
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

        private static FactSetSymbolMapperHelper IndexTickerSymbolTransformation(string tickerSymbol)
        {
            FactSetSymbolMapperHelper factSetSymbolMapperHelper = new FactSetSymbolMapperHelper();
            try
            {
                string[] symbolPart;

                int idx = tickerSymbol.LastIndexOf('-');
                if (idx != -1)
                {
                    symbolPart = tickerSymbol.Substring(0, idx).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    factSetSymbolMapperHelper.Key = (int)AssetCategory.Indices + "-" + tickerSymbol.Substring(idx + 1);
                }
                else
                {
                    symbolPart = tickerSymbol.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    factSetSymbolMapperHelper.Key = (int)AssetCategory.Indices + "-";
                }
                factSetSymbolMapperHelper.Root = symbolPart[0].Replace("$", "");
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
            return factSetSymbolMapperHelper;
        }

        private static FactSetSymbolMapperHelper EquityTickerSymbolTransformation(string tickerSymbol)
        {
            FactSetSymbolMapperHelper factSetSymbolMapperHelper = new FactSetSymbolMapperHelper();
            try
            {
                string[] symbolPart;

                int idx = tickerSymbol.LastIndexOf('-');
                if (idx != -1)
                {
                    symbolPart = tickerSymbol.Substring(0, idx).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    factSetSymbolMapperHelper.Key = (int)AssetCategory.Equity + "-" + tickerSymbol.Substring(idx + 1);
                }
                else
                {
                    symbolPart = tickerSymbol.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    factSetSymbolMapperHelper.Key = (int)AssetCategory.Equity + "-";
                }
                factSetSymbolMapperHelper.Root = symbolPart[0];
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
            return factSetSymbolMapperHelper;
        }

        private static FactSetSymbolMapperHelper EquityOptionTickerSymbolTransformation(string tickerSymbol)
        {
            FactSetSymbolMapperHelper factSetSymbolMapperHelper = new FactSetSymbolMapperHelper();
            try
            {
                string symbolPart;

                int idx = tickerSymbol.LastIndexOf('-');
                if (idx != -1)
                {
                    symbolPart = tickerSymbol.Substring(0, idx);
                    factSetSymbolMapperHelper.Key = (int)AssetCategory.EquityOption + "-" + tickerSymbol.Substring(idx + 1);
                }
                else
                {
                    symbolPart = tickerSymbol;
                    factSetSymbolMapperHelper.Key = (int)AssetCategory.EquityOption + "-";
                }

                string[] splittedData2 = symbolPart.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                string[] splittedData3 = splittedData2[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                factSetSymbolMapperHelper.Root = splittedData3[0];
                factSetSymbolMapperHelper.YearCode2D = splittedData3[1].Substring(0, 2);
                factSetSymbolMapperHelper.MonthCode1D = splittedData3[1].Substring(2, 1);

                factSetSymbolMapperHelper.OptionType = _monthCodeWiseOptionType[factSetSymbolMapperHelper.MonthCode1D];

                string[] splittedData4 = splittedData3[1].Substring(3, splittedData3[1].Length - 3).Split(new char[] { 'D' }, StringSplitOptions.RemoveEmptyEntries);

                if (splittedData4.Length == 2)
                {
                    if (splittedData4[1].Length == 1)
                    {
                        factSetSymbolMapperHelper.Day = "0" + splittedData4[1];
                    }
                    else
                    {
                        factSetSymbolMapperHelper.Day = splittedData4[1];
                    }
                }
                factSetSymbolMapperHelper.StrikePrice = splittedData4[0];

                factSetSymbolMapperHelper.StrikePrice6D = factSetSymbolMapperHelper.StrikePrice.Replace(".", "");
                factSetSymbolMapperHelper.StrikePrice6D += "000000".Substring(0, 6 - factSetSymbolMapperHelper.StrikePrice6D.Length);
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
            return factSetSymbolMapperHelper;
        }

        private static FactSetSymbolMapperHelper FutureTickerSymbolTransformation(string tickerSymbol)
        {
            FactSetSymbolMapperHelper factSetSymbolMapperHelper = new FactSetSymbolMapperHelper();
            try
            {
                string[] symbolPart;

                int idx = tickerSymbol.LastIndexOf('-');
                if (idx != -1)
                {
                    symbolPart = tickerSymbol.Substring(0, idx).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    factSetSymbolMapperHelper.Key = (int)AssetCategory.Future + "-" + tickerSymbol.Substring(idx + 1);
                }
                else
                {
                    symbolPart = tickerSymbol.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    factSetSymbolMapperHelper.Key = (int)AssetCategory.Future + "-";
                }

                string[] splittedData2 = symbolPart[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                factSetSymbolMapperHelper.Root = splittedData2[0];
                factSetSymbolMapperHelper.MonthCode1D = splittedData2[1].Substring(0, 1);
                factSetSymbolMapperHelper.MonthCode2D = _monthCodeWiseFutureMonthNumbers[factSetSymbolMapperHelper.MonthCode1D];

                if (splittedData2[1].Substring(1, splittedData2[1].Length - 1).Length == 2)
                {
                    factSetSymbolMapperHelper.YearCode2D = splittedData2[1].Substring(1, splittedData2[1].Length - 1);
                }
                else
                {
                    factSetSymbolMapperHelper.YearCode1D = splittedData2[1].Substring(1, splittedData2[1].Length - 1);
                    factSetSymbolMapperHelper.YearCode2D = "2" + factSetSymbolMapperHelper.YearCode1D;
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
            return factSetSymbolMapperHelper;
        }

        private static FactSetSymbolMapperHelper FutureOptionTickerSymbolTransformation(string tickerSymbol)
        {
            FactSetSymbolMapperHelper factSetSymbolMapperHelper = new FactSetSymbolMapperHelper();
            try
            {
                string symbolPart;

                int idx = tickerSymbol.LastIndexOf('-');
                if (idx != -1)
                {
                    symbolPart = tickerSymbol.Substring(0, idx);
                    factSetSymbolMapperHelper.Key = (int)AssetCategory.FutureOption + "-" + tickerSymbol.Substring(idx + 1);
                }
                else
                {
                    symbolPart = tickerSymbol;
                    factSetSymbolMapperHelper.Key = (int)AssetCategory.FutureOption + "-";
                }

                string[] splittedData2 = symbolPart.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                factSetSymbolMapperHelper.Root = splittedData2[0];
                factSetSymbolMapperHelper.MonthCode1D = splittedData2[1].Substring(0, 1);
                factSetSymbolMapperHelper.YearCode2D = splittedData2[1].Substring(1, 2);
                factSetSymbolMapperHelper.OptionType = splittedData2[1].Substring(3, 1);

                int month = Convert.ToInt32(_monthCodeWiseFutureMonthNumbers[factSetSymbolMapperHelper.MonthCode1D]) - 1;

                if (month > 0)
                {
                    factSetSymbolMapperHelper.MonthCode2D = "00".Substring(0, 2 - month.ToString().Length) + month;
                }
                else
                {
                    factSetSymbolMapperHelper.MonthCode2D = "12";
                    factSetSymbolMapperHelper.YearCode2D = (Convert.ToInt32(factSetSymbolMapperHelper.YearCode2D) - 1).ToString();
                }

                string[] splittedData3 = splittedData2[1].Substring(4, splittedData2[1].Length - 4).Split(new char[] { 'D' }, StringSplitOptions.RemoveEmptyEntries);

                if (splittedData3.Length == 2)
                {
                    factSetSymbolMapperHelper.Day = splittedData3[1];
                }
                factSetSymbolMapperHelper.StrikePrice = splittedData3[0];

                string[] strikePricePart = factSetSymbolMapperHelper.StrikePrice.Split(new char[] { '.' });

                if (strikePricePart.Length == 2)
                {

                    string prefix = "00000";
                    string suffix = "000000";
                    factSetSymbolMapperHelper.StrikePrice11D = prefix.Substring(0, 5 - strikePricePart[0].Length) + strikePricePart[0] + strikePricePart[1] + suffix.Substring(0, 6 - strikePricePart[1].Length);
                }
                else
                {
                    string prefix = "00000";
                    string suffix = "000000";
                    factSetSymbolMapperHelper.StrikePrice11D = prefix.Substring(0, 5 - strikePricePart[0].Length) + strikePricePart[0] + suffix;
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
            return factSetSymbolMapperHelper;
        }

        private static string ParseSymbol(FactSetSymbolMapper factSetSymbolMapper, FactSetSymbolMapperHelper factSetSymbolMapperHelper)
        {
            string factSetSymbol = factSetSymbolMapper.FactSetFormatString;

            if (factSetSymbol.Contains("{Root}"))
            {
                factSetSymbol = factSetSymbol.Replace("{Root}", factSetSymbolMapperHelper.Root);
            }

            if (factSetSymbol.Contains("{OCC-Symbology}"))
            {
                OptionDetail optionDetail = new OptionDetail();
                optionDetail.UnderlyingSymbol = factSetSymbolMapperHelper.Root;

                int monthcode = 0;
                if (_monthCodeWiseOptionMonthNumbers.ContainsKey(factSetSymbolMapperHelper.MonthCode1D))
                {
                    monthcode = Convert.ToInt32(_monthCodeWiseOptionMonthNumbers[factSetSymbolMapperHelper.MonthCode1D]);
                }

                optionDetail.ExpirationDate = new DateTime(2000 + Convert.ToInt32(factSetSymbolMapperHelper.YearCode2D), monthcode, Convert.ToInt32(factSetSymbolMapperHelper.Day));
                optionDetail.StrikePrice = Convert.ToDouble(factSetSymbolMapperHelper.StrikePrice);

                if (factSetSymbolMapperHelper.OptionType.Equals("C"))
                    optionDetail.OptionType = OptionType.CALL;
                else
                    optionDetail.OptionType = OptionType.PUT;

                string osiSymbol = OptionSymbolGenerator.GenerateOSISymbol(optionDetail);

                if (!string.IsNullOrWhiteSpace(osiSymbol) && osiSymbol.Length > 6)
                {
                    factSetSymbol = factSetSymbol.Replace("{OCC-Symbology}", osiSymbol.Substring(0, 6).Trim() + "#" + osiSymbol.Substring(6, osiSymbol.Length - 6));
                }
            }

            if (factSetSymbol.Contains("{1D-YearCode}"))
            {
                factSetSymbol = factSetSymbol.Replace("{1D-YearCode}", factSetSymbolMapperHelper.YearCode1D);
            }

            if (factSetSymbol.Contains("{2D-YearCode}"))
            {
                factSetSymbol = factSetSymbol.Replace("{2D-YearCode}", factSetSymbolMapperHelper.YearCode2D);
            }

            if (factSetSymbol.Contains("{1D-MonthCode}"))
            {
                factSetSymbol = factSetSymbol.Replace("{1D-MonthCode}", factSetSymbolMapperHelper.MonthCode1D);
            }

            if (factSetSymbol.Contains("{2D-MonthCode}"))
            {
                factSetSymbol = factSetSymbol.Replace("{2D-MonthCode}", factSetSymbolMapperHelper.MonthCode2D);
            }

            if (factSetSymbol.Contains("{Day}"))
            {
                factSetSymbol = factSetSymbol.Replace("{Day}", factSetSymbolMapperHelper.Day);
            }

            if (factSetSymbol.Contains("{YYMMDD-ExpirationDate}"))
            {
                factSetSymbol = factSetSymbol.Replace("{YYMMDD-ExpirationDate}", factSetSymbolMapperHelper.YearCode2D + factSetSymbolMapperHelper.MonthCode2D + "00".Substring(0, 2 - factSetSymbolMapperHelper.Day.Length) + factSetSymbolMapperHelper.Day);
            }

            if (factSetSymbol.Contains("{StrikePrice}"))
            {
                factSetSymbol = factSetSymbol.Replace("{StrikePrice}", factSetSymbolMapperHelper.StrikePrice);
            }

            if (factSetSymbol.Contains("{6D-StrikePrice}"))
            {
                factSetSymbol = factSetSymbol.Replace("{6D-StrikePrice}", factSetSymbolMapperHelper.StrikePrice6D);
            }

            if (factSetSymbol.Contains("{11D-StrikePrice}"))
            {
                factSetSymbol = factSetSymbol.Replace("{11D-StrikePrice}", factSetSymbolMapperHelper.StrikePrice11D);
            }

            if (factSetSymbol.Contains("{OptionType}"))
            {
                factSetSymbol = factSetSymbol.Replace("{OptionType}", factSetSymbolMapperHelper.OptionType);
            }

            if (factSetSymbol.Contains("{FactSetExchangeCode}"))
            {
                factSetSymbol = factSetSymbol.Replace("{FactSetExchangeCode}", factSetSymbolMapper.FactSetExchangeCode);
            }

            if (factSetSymbol.Contains("{FactSetRegionCode}"))
            {
                factSetSymbol = factSetSymbol.Replace("{FactSetRegionCode}", factSetSymbolMapper.FactSetRegionCode);
            }
            return factSetSymbol;
        }
    }
}
