#region Author Summary
///////////////////////////////////////////////////////////////////////////////
// AUTHOR   		 : Bharat Jangir
// CREATION DATE	 : 08 October 2014
// PURPOSE	    	 : Option Symbol Auto Generation
///////////////////////////////////////////////////////////////////////////////
#endregion

#region NameSpaces
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
#endregion

namespace Prana.BusinessLogic.Symbol
{
    public static class OptionSymbolGenerator
    {
        static Dictionary<int, OptionSymbolMapper> _dictOptionSymbolMapper;

        public static void GetOptionSymbol(OptionDetail optionDetail)
        {
            try
            {
                if (_dictOptionSymbolMapper == null)
                    _dictOptionSymbolMapper = SymbolDataManager.GetOptionSymbolMapping();

                GenerateOptionSymbol(optionDetail);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Update optionSymbolMapper dictionary after update in AUEC mapping
        public static void UpdateDictOptionSymbolMapper(DataSet dictDataSet)
        {
            try
            {
                if (dictDataSet != null && dictDataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable dictDataTable = dictDataSet.Tables[0];
                    for (int counter = 0; counter < dictDataTable.Rows.Count; counter++)
                    {
                        if (_dictOptionSymbolMapper != null && _dictOptionSymbolMapper.ContainsKey(Convert.ToInt32(dictDataTable.Rows[counter]["AUECID"])))
                        {
                            #region assign_values
                            OptionSymbolMapper optionSymbolMapper = new OptionSymbolMapper();
                            optionSymbolMapper.AUECID = Convert.ToInt32(dictDataTable.Rows[counter]["AUECID"]);
                            optionSymbolMapper.ExchangeIdentifier = (dictDataTable.Rows[counter]["ExchangeIdentifier"]).ToString().Trim();
                            optionSymbolMapper.ExchangeToken = (dictDataTable.Rows[counter]["ExchangeToken"]).ToString().Trim();
                            optionSymbolMapper.EsignalOptionFormatString = (dictDataTable.Rows[counter]["EsignalOptionFormatString"]).ToString().Trim();
                            optionSymbolMapper.BloombergOptionFormatString = (dictDataTable.Rows[counter]["BloombergOptionFormatString"]).ToString().Trim();
                            optionSymbolMapper.EsignalRootToken = (dictDataTable.Rows[counter]["EsignalRootToken"]).ToString().Trim();
                            optionSymbolMapper.BloombergRootToken = (dictDataTable.Rows[counter]["BloombergRootToken"]).ToString().Trim();
                            #endregion

                            _dictOptionSymbolMapper[optionSymbolMapper.AUECID] = optionSymbolMapper;
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

        private static void GenerateOptionSymbol(OptionDetail optionDetail)
        {
            try
            {
                OptionSymbolMapper optionSymbolMapper = null;
                if (_dictOptionSymbolMapper.ContainsKey(optionDetail.AUECID))
                    optionSymbolMapper = _dictOptionSymbolMapper[optionDetail.AUECID];

                if (optionSymbolMapper != null)
                {
                    if (optionDetail.AssetCategory == AssetCategory.Indices)
                    {
                        if (optionDetail.UnderlyingSymbol.Trim().StartsWith("$"))
                        {
                            optionDetail.UnderlyingSymbol = optionDetail.UnderlyingSymbol.Remove(0, 1);
                        }
                    }

                    string rootSymbol = string.Empty;
                    if (optionDetail.Symbology == ApplicationConstants.SymbologyCodes.TickerSymbol)
                    {
                        optionDetail.Symbol = optionSymbolMapper.EsignalOptionFormatString;

                        if (string.IsNullOrEmpty(optionDetail.EsignalOptionRoot))
                            rootSymbol = GetRoot(optionDetail.UnderlyingSymbol, optionSymbolMapper, optionDetail.Symbology);
                        else
                            rootSymbol = optionDetail.EsignalOptionRoot;
                    }
                    else if (optionDetail.Symbology == ApplicationConstants.SymbologyCodes.BloombergSymbol)
                    {
                        optionDetail.Symbol = optionSymbolMapper.BloombergOptionFormatString;

                        if (string.IsNullOrEmpty(optionDetail.BloombergOptionRoot))
                            rootSymbol = GetRoot(optionDetail.UnderlyingSymbol, optionSymbolMapper, optionDetail.Symbology);
                        else
                            rootSymbol = optionDetail.BloombergOptionRoot;
                    }

                    if (!string.IsNullOrEmpty(optionDetail.Symbol))
                    {
                        #region Option Symbol Generation

                        if (optionDetail.Symbol.Contains("{Root}"))
                        {
                            optionDetail.Symbol = optionDetail.Symbol.Replace("{Root}", rootSymbol.Trim());
                        }

                        if (optionDetail.Symbol.Contains("{UnderlyingSymbol}"))
                        {
                            optionDetail.Symbol = optionDetail.Symbol.Replace("{UnderlyingSymbol}", optionDetail.UnderlyingSymbol.Trim());
                        }

                        //2-Digit Day Code
                        if (optionDetail.Symbol.Contains("{2D-DayCode}"))
                        {
                            optionDetail.Symbol = optionDetail.Symbol.Replace("{2D-DayCode}", optionDetail.ExpirationDate.Day.ToString().Trim().PadLeft(2, '0'));
                        }

                        //1-Digit Month Code >> A for JAN, B for FEB, X for DEC
                        if (optionDetail.Symbol.Contains("{1D-MonthCode}"))
                        {
                            string monthCode = string.Empty;
                            if (optionDetail.AssetCategory == AssetCategory.Equity || optionDetail.AssetCategory == AssetCategory.Indices)
                            {
                                string monthCodeNumeric = ((int)optionDetail.OptionType).ToString().Trim() + optionDetail.ExpirationDate.Month.ToString().Trim().PadLeft(2, '0');
                                monthCode = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_OptionCallPutMonthCodes, monthCodeNumeric);
                            }
                            else if (optionDetail.AssetCategory == AssetCategory.Future)
                            {
                                string monthCodeNumeric = optionDetail.ExpirationDate.Month.ToString().Trim().PadLeft(2, '0');
                                monthCode = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_FutureExpirationMonthCodes, monthCodeNumeric);
                            }

                            if (!string.IsNullOrEmpty(monthCode))
                            {
                                optionDetail.Symbol = optionDetail.Symbol.Replace("{1D-MonthCode}", monthCode.Trim());
                            }
                            else
                            {
                                optionDetail.Symbol = optionDetail.Symbol.Replace("{1D-MonthCode}", String.Empty);
                            }
                        }

                        //2-Digit Month Code >> 01 for JAN, 02 for FEB, 12 for DEC
                        if (optionDetail.Symbol.Contains("{2D-MonthCode}"))
                        {
                            optionDetail.Symbol = optionDetail.Symbol.Replace("{2D-MonthCode}", optionDetail.ExpirationDate.Month.ToString().Trim().PadLeft(2, '0'));
                        }

                        //1-Digit Year Code >> 4 for 2014, 5 for 2015 etc.
                        if (optionDetail.Symbol.Contains("{1D-YearCode}"))
                        {
                            optionDetail.Symbol = optionDetail.Symbol.Replace("{1D-YearCode}", optionDetail.ExpirationDate.Year.ToString().Trim().Remove(0, 3));
                        }

                        //2-Digit Year Code >> 14 for 2014, 15 for 2015 etc.
                        if (optionDetail.Symbol.Contains("{2D-YearCode}"))
                        {
                            optionDetail.Symbol = optionDetail.Symbol.Replace("{2D-YearCode}", optionDetail.ExpirationDate.Year.ToString().Trim().Remove(0, 2));
                        }

                        //4-Digit Year Code >> 2014, 2015 etc.
                        if (optionDetail.Symbol.Contains("{4D-YearCode}"))
                        {
                            optionDetail.Symbol = optionDetail.Symbol.Replace("{4D-YearCode}", optionDetail.ExpirationDate.Year.ToString().Trim());
                        }

                        if (optionDetail.Symbol.Contains("{OptionType}"))
                        {
                            string callOrPutStr = "C";
                            if (optionDetail.OptionType == OptionType.PUT)
                                callOrPutStr = "P";

                            optionDetail.Symbol = optionDetail.Symbol.Replace("{OptionType}", callOrPutStr);
                        }

                        //Strike Price without decimal digits
                        if (optionDetail.Symbol.Contains("{StrikePrice}"))
                        {
                            //Negative Strike Price Check
                            if (optionDetail.StrikePrice < 0)
                                optionDetail.Symbol = optionDetail.Symbol.Replace("{StrikePrice}", "N" + (Math.Abs(optionDetail.StrikePrice * optionDetail.StrikePriceMultiplier)).ToString().Trim());
                            else
                                optionDetail.Symbol = optionDetail.Symbol.Replace("{StrikePrice}", (optionDetail.StrikePrice * optionDetail.StrikePriceMultiplier).ToString().Trim());
                        }

                        //2-Decimal Digit in Strike Price.
                        if (optionDetail.Symbol.Contains("{2D-StrikePrice}"))
                        {
                            //Negative Strike Price Check
                            if (optionDetail.StrikePrice < 0)
                                optionDetail.Symbol = optionDetail.Symbol.Replace("{2D-StrikePrice}", "N" + (Math.Abs(optionDetail.StrikePrice * optionDetail.StrikePriceMultiplier)).ToString("f2").Trim());
                            else
                                optionDetail.Symbol = optionDetail.Symbol.Replace("{2D-StrikePrice}", (optionDetail.StrikePrice * optionDetail.StrikePriceMultiplier).ToString("f2").Trim());
                        }

                        if (optionDetail.Symbol.Contains("{FlexOption}"))
                        {
                            //Flex Option Checks
                            DateTime standardExpiry = Prana.Utilities.DateTimeUtilities.DateTimeHelper.GetNthWeekDay(3, DayOfWeek.Friday, optionDetail.ExpirationDate.Year, optionDetail.ExpirationDate.Month).Add(TimeSpan.FromDays(1));
                            if (optionDetail.ExpirationDate.Date != standardExpiry.Date)
                            {
                                optionDetail.Symbol = optionDetail.Symbol.Replace("{FlexOption}", "D" + optionDetail.ExpirationDate.Day.ToString().Trim());
                            }
                            else
                            {
                                optionDetail.Symbol = optionDetail.Symbol.Replace("{FlexOption}", string.Empty);
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static string GetRoot(string symbol, OptionSymbolMapper optionSymbolMapper, ApplicationConstants.SymbologyCodes symbology)
        {
            string rootSymbol = string.Empty;
            // http://jira.nirvanasolutions.com:8080/browse/PRANA-9637
            symbol = symbol.Split(optionSymbolMapper.ExchangeToken.ToCharArray())[0];
            if (symbology == ApplicationConstants.SymbologyCodes.BloombergSymbol)
                rootSymbol = symbol.Trim().Split(new string[] { optionSymbolMapper.BloombergRootToken.Trim().Replace("{space}", " ") }, StringSplitOptions.RemoveEmptyEntries)[0];
            else
                rootSymbol = symbol.Trim().Split(new string[] { optionSymbolMapper.EsignalRootToken.Trim().Replace("{space}", " ") }, StringSplitOptions.RemoveEmptyEntries)[0];

            return rootSymbol;
        }

        private static bool IsEquityOptionTickerSymbol(string symbol)
        {
            try
            {
                //It will work on the basis of the Esignal Format string like O:{Root} {2D-YearCode}{1D-MonthCode}{2D-StrikePrice}{FlexOption}
                //   Format                              Pattern 
                // O:AAPL 20A123.00              ^[O]{1}:[A-Z0-9]{1,10}\s[0-9]{2}[A-X]{1}[0-9]{1,10}.[0-9]{1,10}$
                // O:AAPL 20A123.00D31           ^[O]{1}:[A-Z0-9]{1,10}\s[0-9]{2}[A-X]{1}[0-9]{1,10}.[0-9]{1,10}[D]([1-9]|1[0-9]|2[0-9]|3[0-1])$
                // O:ATH 15A5.00-MXO             ^[O]{1}:[A-Z0-9]{1,10}\s[0-9]{2}[A-X]{1}[0-9]{1,10}.[0-9]{1,10}-[A-Z]{1,10}$
                // O:ATH 15A5.00D30-MXO          ^[O]{1}:[A-Z0-9]{1,10}\s[0-9]{2}[A-X]{1}[0-9]{1,10}.[0-9]{1,10}[D]([1-9]|1[0-9]|2[0-9]|3[0-1])-[A-Z]{1,10}$

                string strRegex = @"(^[O]{1}:[A-Z0-9]{1,10}\s[0-9]{2}[A-X]{1}[0-9]{1,10}.[0-9]{1,10}$)|(^[O]{1}:[A-Z0-9]{1,10}\s[0-9]{2}[A-X]{1}[0-9]{1,10}.[0-9]{1,10}[D]([1-9]|1[0-9]|2[0-9]|3[0-1])$)|(^[O]{1}:[A-Z0-9]{1,10}\s[0-9]{2}[A-X]{1}[0-9]{1,10}.[0-9]{1,10}-[A-Z]{1,10}$)|(^[O]{1}:[A-Z0-9]{1,10}\s[0-9]{2}[A-X]{1}[0-9]{1,10}.[0-9]{1,10}[D]([1-9]|1[0-9]|2[0-9]|3[0-1])-[A-Z]{1,10}$)";


                Regex regexPattern = new Regex(strRegex);
                if (regexPattern.IsMatch(symbol))
                {
                    return true;
                }
                else
                {
                    return false;
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
            return false;
        }

        private static bool IsEquityOptionBloombergSymbol(string symbol)
        {
            try
            {
                //It will work on the basis of the bloomberg format string like {Root} US {2D-MonthCode}/{2D-DayCode}/{2D-YearCode} {OptionType}{2D-StrikePrice} EQUITY
                string strRegex = @"(^[A-Z0-9]{1,10}\s[A-Z]{2}\s(0[1-9]|1[0-2])/(0[1-9]|1[0-9]|2[0-9]|3[0-1])/[0-9]{2}\s(C|P)([0-9]{1,10}|[0-9]{1,10}.[0-9]{1,10})\sEQUITY$)";
                Regex regexPattern = new Regex(strRegex);
                if (regexPattern.IsMatch(symbol))
                {
                    return true;
                }
                else
                {
                    return false;
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
            return false;
        }

        public static OptionDetail GetOptionDetailObj(string optionSymbol, ApplicationConstants.SymbologyCodes Symbology, string underlyingSymbol)
        {
            try
            {
                OptionDetail optionDetail = new OptionDetail();
                if (Symbology == ApplicationConstants.SymbologyCodes.TickerSymbol && IsEquityOptionTickerSymbol(optionSymbol))
                {
                    string[] optionDetailArray = optionSymbol.Trim().Split(new string[] { " ", ":", "-" }, StringSplitOptions.RemoveEmptyEntries);
                    optionDetail.AssetCategory = AssetCategory.EquityOption;
                    optionDetail.Symbol = optionSymbol;
                    optionDetail.Symbology = Symbology;
                    if (!string.IsNullOrEmpty(underlyingSymbol))
                        optionDetail.UnderlyingSymbol = underlyingSymbol;
                    else
                        optionDetail.UnderlyingSymbol = optionDetailArray[1];

                    optionDetail.AUECID = SecMasterConstants.DefaultOptionAUECID;
                    int year = Convert.ToInt32("20" + optionDetailArray[2].Substring(0, 2));
                    string oneDMonthCodeKey = ConfigurationHelper.Instance.GetOptionCallPutMonthCodeKey(optionDetailArray[2].Substring(2, 1).ToString());
                    int month = Convert.ToInt32(oneDMonthCodeKey.Substring(1, 2));
                    optionDetail.OptionType = (OptionType)Convert.ToInt32(oneDMonthCodeKey[0].ToString());
                    int index = optionDetailArray[2].IndexOf('D', 3);
                    if (index != -1)
                    {
                        optionDetail.StrikePrice = Convert.ToDouble(optionDetailArray[2].Substring(3, index - 3));
                        int dayOfMonth = optionDetailArray[2].Length >= index + 3 ? Convert.ToInt16(optionDetailArray[2].Substring(index + 1, 2)) : Convert.ToInt16(optionDetailArray[2][index + 1].ToString());

                        optionDetail.ExpirationDate = new DateTime(year, month, dayOfMonth);
                    }
                    else
                    {
                        optionDetail.StrikePrice = Convert.ToDouble(optionDetailArray[2].Substring(3));
                        DateTime standardExpiry = Prana.Utilities.DateTimeUtilities.DateTimeHelper.GetNthWeekDay(3, DayOfWeek.Friday, optionDetail.ExpirationDate.Year, optionDetail.ExpirationDate.Month).Add(TimeSpan.FromDays(1));
                        optionDetail.ExpirationDate = new DateTime(year, month, standardExpiry.Day);
                    }
                    return optionDetail;
                }
                else if (Symbology == ApplicationConstants.SymbologyCodes.BloombergSymbol && IsEquityOptionBloombergSymbol(optionSymbol))
                {
                    string[] optionDetailArray = optionSymbol.Trim().Split(new string[] { " ", "/" }, StringSplitOptions.RemoveEmptyEntries);
                    optionDetail.AssetCategory = AssetCategory.EquityOption;
                    optionDetail.AUECID = SecMasterConstants.DefaultOptionAUECID;
                    optionDetail.Symbol = optionSymbol;
                    optionDetail.Symbology = Symbology;
                    if (!string.IsNullOrEmpty(underlyingSymbol))
                        optionDetail.UnderlyingSymbol = underlyingSymbol;
                    else
                        optionDetail.UnderlyingSymbol = optionDetailArray[0] + " " + optionDetailArray[1] + " " + optionDetailArray[6];
                    int year = Convert.ToInt32("20" + optionDetailArray[4]);
                    int month = Convert.ToInt32(optionDetailArray[2]);
                    int dayOfMonth = Convert.ToInt32(optionDetailArray[3]);
                    optionDetail.ExpirationDate = new DateTime(year, month, dayOfMonth);
                    string oneDMonthCodeKey = ConfigurationHelper.Instance.GetOptionCallPutMonthCodeKey(optionDetailArray[5].Substring(0, 1).ToString());
                    optionDetail.StrikePrice = Convert.ToDouble(optionDetailArray[5].Substring(1));
                    optionDetail.OptionType = (OptionType)Convert.ToInt32(oneDMonthCodeKey[0].ToString());
                    return optionDetail;
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
            return null;
        }

        public static string GenerateOSISymbol(OptionDetail optionDetail)
        {
            try
            {
                string osiSymbol = string.Empty;
                string symbol = string.Empty;
                //if (optionDetail.UnderlyingSymbol.Length >= 4)
                //    symbol = optionDetail.UnderlyingSymbol.Substring(0, 4).PadRight(6);
                //else
                symbol = optionDetail.UnderlyingSymbol.PadRight(6);
                string datecode = optionDetail.ExpirationDate.Year.ToString().Substring(2) + optionDetail.ExpirationDate.Month.ToString().PadLeft(2, '0') + optionDetail.ExpirationDate.Day.ToString().PadLeft(2, '0');
                string[] arrayPrice = optionDetail.StrikePrice.ToString().Split(new char[] { '.' });
                string strikepriceCode = (arrayPrice.Length.Equals(2)) ? arrayPrice[0].PadLeft(5, '0') + arrayPrice[1].PadRight(3, '0') : arrayPrice[0].PadLeft(5, '0').PadRight(8, '0');
                osiSymbol = symbol + datecode + optionDetail.OptionType.ToString().Substring(0, 1) + strikepriceCode;
                return osiSymbol;
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
    }
}