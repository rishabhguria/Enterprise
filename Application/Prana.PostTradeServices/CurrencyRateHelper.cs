using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PostTradeServices
{
    class CurrencyRateHelper
    {
        private static int _companyBaseCurrency = 1;
        private static Dictionary<int, Dictionary<int, Dictionary<int, double>>> _dictCurrencyConversionRates = new Dictionary<int, Dictionary<int, Dictionary<int, double>>>();
        private static Dictionary<string, double> _dictFxRateOnTradeDate = new Dictionary<string, double>();

        public static void CreateDictionary()
        {
            try
            {
                _dictCurrencyConversionRates.Clear();
                _companyBaseCurrency = DataBaseManager.GetCompanyBaseCurrency();
                DataTable dtMarkPrice = DataBaseManager.GetCurrencyConversionRateData();

                foreach (DataRow row in dtMarkPrice.Rows)
                {
                    int fromCurrencyID = Int32.Parse(row["FromCurrencyID"].ToString());
                    int toCurrencyID = Int32.Parse(row["ToCurrencyID"].ToString());
                    int timeKey = Int32.Parse(row["TimeKey"].ToString());
                    double conversionRate = Double.Parse(row["ConversionRate"].ToString());

                    if (_dictCurrencyConversionRates.ContainsKey(fromCurrencyID))
                    {
                        if (_dictCurrencyConversionRates[fromCurrencyID].ContainsKey(toCurrencyID))
                        {
                            if (_dictCurrencyConversionRates[fromCurrencyID][toCurrencyID].ContainsKey(timeKey))
                            {
                                _dictCurrencyConversionRates[fromCurrencyID][toCurrencyID][timeKey] = conversionRate;
                            }
                            else
                            {
                                _dictCurrencyConversionRates[fromCurrencyID][toCurrencyID].Add(timeKey, conversionRate);
                            }
                        }
                        else
                        {
                            Dictionary<int, double> dictDateConRate = new Dictionary<int, double>();
                            dictDateConRate.Add(timeKey, conversionRate);
                            _dictCurrencyConversionRates[fromCurrencyID].Add(toCurrencyID, dictDateConRate);
                        }
                    }
                    else
                    {
                        Dictionary<int, Dictionary<int, double>> dictToCurrencyDateConRate = new Dictionary<int, Dictionary<int, double>>();
                        Dictionary<int, double> dictDateConRate = new Dictionary<int, double>();
                        dictDateConRate.Add(timeKey, conversionRate);
                        dictToCurrencyDateConRate.Add(toCurrencyID, dictDateConRate);
                        _dictCurrencyConversionRates.Add(fromCurrencyID, dictToCurrencyDateConRate);
                    }
                }


                _dictFxRateOnTradeDate.Clear();
                DataTable dtTradeFxRate = DataBaseManager.GetFxRateOnTradeDateData();
                foreach (DataRow row in dtTradeFxRate.Rows)
                {
                    string taxlotID = row["TaxlotID"].ToString();
                    double tradeFxRate = Double.Parse(row["TradeFxRate"].ToString());
                    if (!_dictFxRateOnTradeDate.ContainsKey(taxlotID))
                    {
                        _dictFxRateOnTradeDate.Add(taxlotID, tradeFxRate);
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

        public static double GetCurrencyConversionRate(SMData smData, int toCurrencyID, int timeKey)
        {
            double rate = 1.0;
            while (HolidayHelper.IsHoliday(smData.AUECID, timeKey))
            {
                timeKey = timeKey - 1; // skip back for holidays
            }

            try
            {
                if (_dictCurrencyConversionRates.ContainsKey(smData.CurrencyID))
                {
                    if (_dictCurrencyConversionRates[smData.CurrencyID].ContainsKey(toCurrencyID))
                    {
                        if (_dictCurrencyConversionRates[smData.CurrencyID][toCurrencyID].ContainsKey(timeKey))
                        {
                            rate = _dictCurrencyConversionRates[smData.CurrencyID][toCurrencyID][timeKey];
                        }
                    }
                }
                else if (_dictCurrencyConversionRates.ContainsKey(toCurrencyID))
                {
                    if (_dictCurrencyConversionRates[toCurrencyID].ContainsKey(smData.CurrencyID))
                    {
                        if (_dictCurrencyConversionRates[toCurrencyID][smData.CurrencyID].ContainsKey(timeKey) && _dictCurrencyConversionRates[toCurrencyID][smData.CurrencyID][timeKey] != 0.0)
                        {
                            rate = 1 / _dictCurrencyConversionRates[toCurrencyID][smData.CurrencyID][timeKey];
                        }
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
            return rate;
        }
        public static double GetToBaseCurrencyConversionRate(SMData smData, int timeKey)
        {
            if (smData.CurrencyID == _companyBaseCurrency)
            {
                return 1.0;
            }
            else
            {
                return GetCurrencyConversionRate(smData, _companyBaseCurrency, timeKey);
            }
        }
        public static double GetTradeFxRate(string taxlotID)
        {
            if (_dictFxRateOnTradeDate.ContainsKey(taxlotID))
            {
                return _dictFxRateOnTradeDate[taxlotID];
            }
            else
            {
                return 1.0;
            }
        }

    }
}
