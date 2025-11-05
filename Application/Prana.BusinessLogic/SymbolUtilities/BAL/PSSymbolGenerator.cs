#region Author Summary
///////////////////////////////////////////////////////////////////////////////
// AUTHOR   		 : Bharat Jangir
// CREATION DATE	 : 11 July 2014
// PURPOSE	    	 : PS Symbol Auto Generation
///////////////////////////////////////////////////////////////////////////////
#endregion

#region NameSpaces
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#endregion

namespace Prana.BusinessLogic.Symbol
{
    public static class PSSymbolGenerator
    {
        static Dictionary<int, PSSymbolMapper> _dictPSSymbolMapper;
        static List<int> _nationalCurrencies;
        static readonly object _locker = new object();

        public static string GetPSSymbol(PSSymbolRequestObject psSymbolRequest, ISecMasterServices secMasterServices)
        {
            lock (_locker)
            {
                if (_dictPSSymbolMapper == null)
                    _dictPSSymbolMapper = SymbolDataManager.GetPSSymbolMapping();

                if (_nationalCurrencies == null && !string.IsNullOrEmpty(ConfigurationHelper.Instance.GetAppSettingValueByKey("NationalCurrencyIdsForPSSymbol").ToString().Trim()))
                    _nationalCurrencies = (ConfigurationHelper.Instance.GetAppSettingValueByKey("NationalCurrencyIdsForPSSymbol")).Split(',').Select(int.Parse).ToList();

                PSSymbolRoot psSymbolRoot = null;
                FutureRootData futureRootData = secMasterServices.GetFutureRootData(psSymbolRequest.Symbol);
                if (futureRootData != null)
                {
                    psSymbolRoot = new PSSymbolRoot();
                    psSymbolRoot.ESRoot = futureRootData.RootSymbol.Trim();
                    psSymbolRoot.PSRoot = futureRootData.PSRootSymbol.Trim();
                }

                SecMasterBaseObj secMasterBaseObj = null;
                SecMasterBaseObj underlyingSecMasterBaseObj = null;
                secMasterBaseObj = secMasterServices.GetSecMasterDataForSymbol(psSymbolRequest.Symbol);
                if (((AssetCategory)psSymbolRequest.AssetID) == AssetCategory.EquityOption || ((AssetCategory)psSymbolRequest.AssetID) == AssetCategory.FutureOption)
                {
                    underlyingSecMasterBaseObj = secMasterServices.GetSecMasterDataForSymbol(psSymbolRequest.UnderlyingSymbol);
                }

                return GeneratePSSymbol(psSymbolRequest, psSymbolRoot, secMasterBaseObj, underlyingSecMasterBaseObj);
            }
        }

        //Update psSymbolMapper dictionary after update in AUEC mapping
        public static void UpdateDictPSSymbolMapper(DataSet dictDataSet)
        {
            try
            {
                if (dictDataSet != null && dictDataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable dictDataTable = dictDataSet.Tables[0];
                    for (int counter = 0; counter < dictDataTable.Rows.Count; counter++)
                    {
                        if (_dictPSSymbolMapper != null && _dictPSSymbolMapper.ContainsKey(Convert.ToInt32(dictDataTable.Rows[counter]["AUECID"])))
                        {
                            #region assign_values
                            PSSymbolMapper psSymbolMapper = new PSSymbolMapper();
                            psSymbolMapper.AUECID = Convert.ToInt32(dictDataTable.Rows[counter]["AUECID"]);
                            psSymbolMapper.ExchangeIdentifier = (dictDataTable.Rows[counter]["ExchangeIdentifier"]).ToString().Trim();
                            psSymbolMapper.Year = (dictDataTable.Rows[counter]["Year"]).ToString().Trim();
                            psSymbolMapper.Month = (dictDataTable.Rows[counter]["Month"]).ToString().Trim();
                            psSymbolMapper.Day = (dictDataTable.Rows[counter]["Day"]).ToString().Trim();
                            psSymbolMapper.Type = (dictDataTable.Rows[counter]["Type"]).ToString().Trim();
                            psSymbolMapper.Strike = (dictDataTable.Rows[counter]["Strike"]).ToString().Trim();
                            psSymbolMapper.ExchangeToken = (dictDataTable.Rows[counter]["ExchangeToken"]).ToString().Trim();
                            psSymbolMapper.PSRootToken = (dictDataTable.Rows[counter]["PSRootToken"]).ToString().Trim();
                            psSymbolMapper.PSFormatString = (dictDataTable.Rows[counter]["PSFormatString"]).ToString().Trim();
                            psSymbolMapper.TranslateRoot = Convert.ToBoolean(dictDataTable.Rows[counter]["TranslateRoot"]);
                            psSymbolMapper.TranslateType = Convert.ToBoolean(dictDataTable.Rows[counter]["TranslateType"]);
                            psSymbolMapper.ExerciseStyle = (dictDataTable.Rows[counter]["ExerciseStyle"]).ToString().Trim();
                            #endregion

                            _dictPSSymbolMapper[psSymbolMapper.AUECID] = psSymbolMapper;
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

        private static string GeneratePSSymbol(PSSymbolRequestObject psSymbolRequest, PSSymbolRoot psSymbolRoot, SecMasterBaseObj secMasterBaseObj, SecMasterBaseObj underlyingSecMasterBaseObj)
        {
            try
            {
                PSSymbolMapper psSymbolMapper = null;
                if (_dictPSSymbolMapper.ContainsKey(psSymbolRequest.AUECID))
                    psSymbolMapper = _dictPSSymbolMapper[psSymbolRequest.AUECID];

                if (psSymbolMapper != null)
                {
                    string symbol = GetBody(psSymbolRequest.Symbol, psSymbolMapper);
                    string rootSymbol = GetRoot(psSymbolRequest.Symbol, psSymbolMapper);

                    if (psSymbolRoot != null)
                        rootSymbol = psSymbolMapper.TranslateRoot == true ? psSymbolRoot.PSRoot : psSymbolRoot.ESRoot;

                    string psSymbol = psSymbolMapper.PSFormatString;
                    //Special handling for national and international bonds
                    if (psSymbol.Equals("{BOND}") && (((AssetCategory)psSymbolRequest.AssetID) == AssetCategory.FixedIncome || ((AssetCategory)psSymbolRequest.AssetID) == AssetCategory.ConvertibleBond))
                    {
                        //National Bonds
                        if (_nationalCurrencies != null && _nationalCurrencies.Contains(secMasterBaseObj.CurrencyID))
                        {
                            psSymbol = "{Cusip}.B";
                        }
                        //International Bonds
                        else
                        {
                            psSymbol = "{Sedol}OR{ISIN}.GB";
                        }
                    }

                    #region PSSymbol Generation
                    psSymbol = psSymbol.Replace("{Root}", rootSymbol);

                    if (psSymbol.Contains("{Symbol}"))
                    {
                        psSymbol = psSymbol.Replace("{Symbol}", psSymbolRequest.Symbol);
                    }

                    if (psSymbol.Contains("{UnderlyingSymbol}"))
                    {
                        if (psSymbolRequest.UnderlyingSymbol.Trim().StartsWith("$"))
                        {
                            psSymbol = psSymbol.Replace("{UnderlyingSymbol}", psSymbolRequest.UnderlyingSymbol.Remove(0, 1));
                        }
                        else
                        {
                            psSymbol = psSymbol.Replace("{UnderlyingSymbol}", psSymbolRequest.UnderlyingSymbol);
                        }
                    }

                    if (psSymbol.Contains("{UnderlyingSedol}") && underlyingSecMasterBaseObj != null)
                    {
                        if (!string.IsNullOrEmpty(underlyingSecMasterBaseObj.SedolSymbol))
                        {
                            psSymbol = psSymbol.Replace("{UnderlyingSedol}", (underlyingSecMasterBaseObj.SedolSymbol.Substring(0, 6)).PadRight(6));
                        }
                        else
                        {
                            return psSymbolRequest.Symbol;
                        }
                    }

                    //The {SEDOL}OR{ISIN} check must be written before the {SEDOL} and {ISIN} check otherwise PS Symbol format string replaced wrongly.
                    if ((psSymbol.Contains("{Sedol}OR{ISIN}") || psSymbol.Contains("{ISIN}OR{Sedol}")) && secMasterBaseObj != null)
                    {
                        if (!string.IsNullOrEmpty(secMasterBaseObj.SedolSymbol))
                        {
                            psSymbol = psSymbol.Replace("{ISIN}", string.Empty);
                            psSymbol = psSymbol.Replace("OR", string.Empty);
                            psSymbol = psSymbol.Replace("{Sedol}", secMasterBaseObj.SedolSymbol);
                        }
                        else if (!string.IsNullOrEmpty(secMasterBaseObj.ISINSymbol))
                        {
                            psSymbol = psSymbol.Replace("{Sedol}", string.Empty);
                            psSymbol = psSymbol.Replace("OR", string.Empty);
                            psSymbol = psSymbol.Replace("{ISIN}", secMasterBaseObj.ISINSymbol);
                        }
                        else
                        {
                            return psSymbolRequest.Symbol;
                        }
                    }

                    if (psSymbol.Contains("{Sedol}") && secMasterBaseObj != null)
                    {
                        if (!string.IsNullOrEmpty(secMasterBaseObj.SedolSymbol))
                        {
                            psSymbol = psSymbol.Replace("{Sedol}", secMasterBaseObj.SedolSymbol);
                        }
                        else
                        {
                            return psSymbolRequest.Symbol;
                        }
                    }

                    if (psSymbol.Contains("{ISIN}") && secMasterBaseObj != null)
                    {
                        if (!string.IsNullOrEmpty(secMasterBaseObj.ISINSymbol))
                        {
                            psSymbol = psSymbol.Replace("{ISIN}", secMasterBaseObj.ISINSymbol);
                        }
                        else
                        {
                            return psSymbolRequest.Symbol;
                        }
                    }

                    if (psSymbol.Contains("{Cusip}") && secMasterBaseObj != null)
                    {
                        if (!string.IsNullOrEmpty(secMasterBaseObj.CusipSymbol))
                        {
                            psSymbol = psSymbol.Replace("{Cusip}", secMasterBaseObj.CusipSymbol);
                        }
                        else
                        {
                            return psSymbolRequest.Symbol;
                        }
                    }

                    //Extracts Day Code from Symbol
                    if (psSymbol.Contains("{DayCode-FromSymbol}"))
                    {
                        string day = GetDay(symbol, psSymbolMapper);
                        psSymbol = psSymbol.Replace("{DayCode-FromSymbol}", day);
                    }

                    //2-Digit Day Code from Database
                    if (psSymbol.Contains("{2D-DayCode-FromDB}") && secMasterBaseObj != null)
                    {
                        psSymbol = psSymbol.Replace("{2D-DayCode-FromDB}", ((SecMasterOptObj)secMasterBaseObj).ExpirationDate.Day.ToString().Trim().PadLeft(2, '0'));
                    }

                    //Extracts Month Code from Symbol
                    if (psSymbol.Contains("{MonthCode-FromSymbol}"))
                    {
                        string month = GetMonth(symbol, psSymbolMapper);
                        psSymbol = psSymbol.Replace("{MonthCode-FromSymbol}", month);
                    }

                    //1-Digit Month Code >> A for JAN, B for FEB, X for DEC from Database
                    if (psSymbol.Contains("{1D-MonthCode}"))
                    {
                        string monthCode = string.Empty;
                        if (psSymbolRequest.AssetID == (int)AssetCategory.Equity || psSymbolRequest.AssetID == (int)AssetCategory.Indices)
                        {
                            string monthCodeNumeric = (((SecMasterOptObj)secMasterBaseObj).PutOrCall).ToString().Trim() + psSymbolRequest.ExpirationDate.Month.ToString().Trim().PadLeft(2, '0');
                            monthCode = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_OptionCallPutMonthCodes, monthCodeNumeric);
                        }
                        else if (psSymbolRequest.AssetID == (int)AssetCategory.Future)
                        {
                            string monthCodeNumeric = psSymbolRequest.ExpirationDate.Month.ToString().Trim().PadLeft(2, '0');
                            monthCode = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_FutureExpirationMonthCodes, monthCodeNumeric);
                        }
                        psSymbol = psSymbol.Replace("{1D-MonthCode}", monthCode.Trim());
                    }

                    //2-Digit Month Code >> 01 for JAN, 02 for FEB, 12 for DEC from Database
                    if (psSymbol.Contains("{2D-MonthCode-FromDB}") && secMasterBaseObj != null)
                    {
                        psSymbol = psSymbol.Replace("{2D-MonthCode-FromDB}", ((SecMasterOptObj)secMasterBaseObj).ExpirationDate.Month.ToString().Trim().PadLeft(2, '0'));
                    }

                    //Extracts Year Code from Symbol
                    if (psSymbol.Contains("{YearCode-FromSymbol}"))
                    {
                        string year = GetYear(symbol, psSymbolMapper);
                        if (year == Convert.ToString(int.MinValue))
                        {
                            psSymbol = symbol;
                        }
                        psSymbol = psSymbol.Replace("{YearCode-FromSymbol}", year);
                    }

                    //1-Digit Year Code >> 4 for 2014, 5 for 2015 etc from Database
                    if (psSymbol.Contains("{1D-YearCode-FromDB}") && secMasterBaseObj != null)
                    {
                        psSymbol = psSymbol.Replace("{1D-YearCode-FromDB}", ((SecMasterOptObj)secMasterBaseObj).ExpirationDate.Year.ToString().Trim().Remove(0, 3));
                    }

                    //2-Digit Year Code >> 14 for 2014, 15 for 2015 etc from Database
                    if (psSymbol.Contains("{2D-YearCode-FromDB}") && secMasterBaseObj != null)
                    {
                        psSymbol = psSymbol.Replace("{2D-YearCode-FromDB}", ((SecMasterOptObj)secMasterBaseObj).ExpirationDate.Year.ToString().Trim().Remove(0, 2));
                    }

                    //4-Digit Year Code >> 2014, 2015 etc from Database
                    if (psSymbol.Contains("{4D-YearCode-FromDB}") && secMasterBaseObj != null)
                    {
                        psSymbol = psSymbol.Replace("{4D-YearCode-FromDB}", ((SecMasterOptObj)secMasterBaseObj).ExpirationDate.Year.ToString().Trim());
                    }

                    //Extracts Option Type from Symbol
                    if (psSymbol.Contains("{OptionType-FromSymbol}"))
                    {
                        string type = GetType(symbol, psSymbolMapper);
                        if (psSymbolMapper.TranslateType)
                        {
                            int yeardiff = Convert.ToInt16(GetYear(symbol, psSymbolMapper)) - (DateTime.Today.Year - 2000);
                            char translatedTypeCode = Convert.ToChar(type[0] + yeardiff);
                            type = translatedTypeCode.ToString().Trim();
                        }
                        psSymbol = psSymbol.Replace("{OptionType-FromSymbol}", type);
                    }

                    //Option Type from Database
                    if (psSymbol.Contains("{OptionType-FromDB}") && secMasterBaseObj != null)
                    {
                        char putOrCall = 'P';
                        if (((SecMasterOptObj)secMasterBaseObj).PutOrCall == 1)
                            putOrCall = 'C';

                        psSymbol = psSymbol.Replace("{OptionType-FromDB}", putOrCall.ToString().Trim());
                    }

                    //Extracts Strike Price from Symbol
                    if (psSymbol.Contains("{StrikePrice-FromSymbol}"))
                    {
                        string strike = GetStrike(symbol, psSymbolMapper);
                        psSymbol = psSymbol.Replace("{StrikePrice-FromSymbol}", strike);
                    }

                    //Strike Price from Database
                    if (psSymbol.Contains("{StrikePrice-FromDB}"))
                    {
                        psSymbol = psSymbol.Replace("{StrikePrice-FromDB}", ((SecMasterOptObj)secMasterBaseObj).StrikePrice.ToString().Trim());
                    }

                    //Extracts Strike Price from Symbol then Format applied
                    if (psSymbol.Contains("{StrikePriceFormated-FromSymbol}"))
                    {
                        string strike = GetStrike(symbol, psSymbolMapper);
                        psSymbol = psSymbol.Replace("{StrikePriceFormated-FromSymbol}", Convert.ToDouble(strike).ToString("00000.000").Replace(".", ""));
                    }

                    //Strike Price from Database then Format applied
                    if (psSymbol.Contains("{StrikePriceFormated-FromDB}"))
                    {
                        psSymbol = psSymbol.Replace("{StrikePriceFormated-FromDB}", ((SecMasterOptObj)secMasterBaseObj).StrikePrice.ToString("00000.000").Replace(".", ""));
                    }

                    //Excercise Sytle >> A For American and E for Europian options
                    if (psSymbol.Contains("{ExerciseStyle}"))
                    {
                        psSymbol = psSymbol.Replace("{ExerciseStyle}", psSymbolMapper.ExerciseStyle);
                    }

                    if (psSymbol.Contains("{ImpliedVol}"))
                    {
                        psSymbol = psSymbol.Replace("{ImpliedVol}", psSymbolRequest.Volatility.ToString().Trim());
                    }
                    #endregion

                    return psSymbol;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                Logger.HandleException(new Exception("Invalid PS mapping for Symbol : " + psSymbolRequest.Symbol + "\t AUECID :" + psSymbolRequest.AUECID + "\t Exception Message : " + ex.Message), LoggingConstants.POLICY_LOGANDSHOW);
            }
            return psSymbolRequest.Symbol;
        }

        private static string GetSubString(StringOffset offsets, string symbol)
        {
            string subString = string.Empty;
            try
            {
                if (offsets == null)
                    return string.Empty;

                if (offsets.Count == 1)
                {
                    subString = symbol.Substring(offsets.Start);
                }
                else if (offsets.Count == 2)
                {
                    string buffer = symbol.Substring(offsets.Start);
                    subString = symbol.Substring(offsets.Start, Math.Min(buffer.Length, offsets.Length));
                }
                else
                {
                    throw new Exception("Invalid PS mapping for Symbol: " + symbol);
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
            return subString;
        }

        private static StringOffset GetOffsets(string value)
        {
            StringOffset offset = new StringOffset();
            try
            {
                if (value == null)
                    return null;

                string[] values = value.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                offset.Start = Convert.ToInt16(values[0]);
                if (values.Count() == 2)
                {
                    offset.Length = Convert.ToInt16(values[1]);
                    offset.Count = 2;
                }
                else
                {
                    offset.Count = 1;
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
            return offset;
        }

        private static string GetBody(string symbol, PSSymbolMapper psSymbolMapper)
        {
            string[] values = symbol.Split(new string[] { psSymbolMapper.PSRootToken.Trim().Replace("{space}", " "), psSymbolMapper.ExchangeToken.Trim() }, StringSplitOptions.RemoveEmptyEntries);
            return values.Count() == 1 ? values[0] : values[1];
        }

        private static string GetRoot(string symbol, PSSymbolMapper psSymbolMapper)
        {
            string[] values = symbol.Trim().Split(new string[] { psSymbolMapper.PSRootToken.Trim().Replace("{space}", " ") }, StringSplitOptions.RemoveEmptyEntries);
            return values[0];
        }

        private static string GetYear(string symbol, PSSymbolMapper psSymbolMapper)
        {
            int year = int.MinValue;
            int number;
            try
            {
                bool success = int.TryParse(GetSubString(GetOffsets(psSymbolMapper.Year.Trim()), symbol), out number);
                if (success)
                {
                    year = Convert.ToInt16(number);
                    return (10 + year).ToString().Trim();
                }
                else
                {
                    throw new Exception("Invalid PS mapping for Symbol: " + symbol);
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
            return year.ToString();
        }

        private static string GetMonth(string symbol, PSSymbolMapper psSymbolMapper)
        {
            return GetSubString(GetOffsets(psSymbolMapper.Month.Trim()), symbol);
        }

        private static string GetDay(string symbol, PSSymbolMapper psSymbolMapper)
        {
            return GetSubString(GetOffsets(psSymbolMapper.Day), symbol);
        }

        private static string GetType(string symbol, PSSymbolMapper psSymbolMapper)
        {
            return GetSubString(GetOffsets(psSymbolMapper.Type), symbol);
        }

        private static string GetStrike(string symbol, PSSymbolMapper psSymbolMapper)
        {
            string buffer = string.Empty;
            try
            {
                StringOffset offsets = GetOffsets(psSymbolMapper.Strike);
                buffer = GetSubString(offsets, symbol);
                if (offsets != null && offsets.Count == 2 && buffer.Length != offsets.Length)
                {
                    buffer = buffer.PadRight(offsets.Length, '0');
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
            return buffer;
        }
    }
}