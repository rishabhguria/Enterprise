using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
namespace Prana.PostTradeServices
{
    class MarkPriceHelper
    {
        static Dictionary<Int32, Dictionary<string, double>> _dictMarkPricesWithDates = new Dictionary<Int32, Dictionary<string, double>>();
        public static void CreateDictionary()
        {
            try
            {
                DataTable dtMarkPrice = DataBaseManager.GetMarkPriceData();
                Int32 dateKey = 0;
                string symbol = String.Empty;
                _dictMarkPricesWithDates.Clear();

                foreach (DataRow row in dtMarkPrice.Rows)
                {
                    double markPrice;
                    symbol = row["Symbol"].ToString();
                    dateKey = Int32.Parse(row["TimeKey"].ToString());
                    markPrice = Convert.ToDouble(row["MarkPrice"].ToString());

                    if (_dictMarkPricesWithDates.ContainsKey(dateKey))
                    {
                        if (_dictMarkPricesWithDates[dateKey].ContainsKey(symbol))
                        {
                            _dictMarkPricesWithDates[dateKey][symbol] = markPrice;
                        }
                        else
                        {
                            _dictMarkPricesWithDates[dateKey].Add(symbol, markPrice);
                        }
                    }
                    else
                    {
                        Dictionary<string, double> dictMPrices = new Dictionary<string, double>();
                        dictMPrices.Add(symbol, markPrice);
                        _dictMarkPricesWithDates.Add(dateKey, dictMPrices);
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
        }
        public static double GetSymbolMarkPrice(Int32 dateKey, string symbol)
        {
            int auecID = SMHelper.GetAUECIDForTickerSymbol(symbol);
            while (HolidayHelper.IsHoliday(auecID, dateKey))
            {
                dateKey = dateKey - 1;
            }

            if (_dictMarkPricesWithDates.ContainsKey(dateKey) && _dictMarkPricesWithDates[dateKey].ContainsKey(symbol))
            {
                return _dictMarkPricesWithDates[dateKey][symbol];
            }
            else
                return 0;
        }
    }
}
