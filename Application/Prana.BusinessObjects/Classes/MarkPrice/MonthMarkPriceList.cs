
using Csla;

namespace Prana.BusinessObjects
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class MonthMarkPriceList : BusinessListBase<MonthMarkPriceList, MonthMarkPrice>
    {

        /// <summary>
        /// Gets the mark price for symbol and month.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="month">The month.</param>
        /// <returns></returns>
        public double GetMarkPriceForSymbolAndMonth(string symbol, string month)
        {
            double result = 0d;
            foreach (MonthMarkPrice monthMarkPrice in this)
            {
                if (monthMarkPrice.Symbol.Equals(symbol) && monthMarkPrice.Month.Equals(month))
                {
                    result = monthMarkPrice.FinalMarkPrice;
                    break;
                }
            }
            return result;

        }
    }
}
