using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace DataProcessor
{
   
    class MarkPriceHelper
    {
        static Dictionary<Int32, Dictionary<string, MarkPriceatDate>> _dictMarkPricesWithDates = new Dictionary<Int32, Dictionary<string, MarkPriceatDate>>();
        public static void CreateDictionary(DataTable dtMarkPrice)
        {
            Int32 minDateKey = 0;            
            Int32 dateKey = 0;
            string symbol = String.Empty;
            _dictMarkPricesWithDates.Clear();

            foreach(DataRow row in dtMarkPrice.Rows)
            {
                MarkPriceatDate mpdate = new MarkPriceatDate();
                symbol = row["Symbol"].ToString();
                dateKey = Int32.Parse(row["TimeKey"].ToString());
                mpdate.MarkPrice = Convert.ToSingle(row["MarkPrice"].ToString());
                mpdate.IsHoliday = Convert.ToBoolean(row["IsHoliday"].ToString());
                minDateKey = dateKey;                

                if (_dictMarkPricesWithDates.ContainsKey(dateKey))
                {
                    if (_dictMarkPricesWithDates[dateKey].ContainsKey(symbol))
                    {
                        _dictMarkPricesWithDates[dateKey][symbol] = mpdate;
                    }
                    else
                    {
                        _dictMarkPricesWithDates[dateKey].Add(symbol, mpdate);
                    }    
                }
                else 
                {
                    Dictionary<string, MarkPriceatDate> dictMPrices = new Dictionary<string, MarkPriceatDate>();
                    dictMPrices.Add(symbol, mpdate);
                    _dictMarkPricesWithDates.Add(dateKey, dictMPrices);
                }
            }            
        }
        public static float GetSymbolMarkPrice(Int32 dateKey, string symbol)
        {
            if (_dictMarkPricesWithDates.ContainsKey(dateKey) && _dictMarkPricesWithDates[dateKey].ContainsKey(symbol))
            {
                return _dictMarkPricesWithDates[dateKey][symbol].MarkPrice;
            }
            else
                return 0;
        }
    }
    class MarkPriceatDate
    {
        public float MarkPrice = 0;
        public bool IsHoliday=false;
    }
}
