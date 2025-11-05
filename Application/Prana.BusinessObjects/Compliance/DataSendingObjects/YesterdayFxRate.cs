using System;

namespace Prana.BusinessObjects.Compliance.DataSendingObjects
{
    /// <summary>
    /// Definition of YesterdayFxRate
    /// Used to send data to esper
    /// </summary>
    public class YesterdayFxRate
    {
        String currencySymbol;

        public String CurrencySymbol
        {
            get { return currencySymbol; }
            set { currencySymbol = value; }
        }

        double conversionRate;

        public double ConversionRate
        {
            get { return conversionRate; }
            set { conversionRate = value; }
        }

        String conversionMethodOperator;

        public String ConversionMethodOperator
        {
            get { return conversionMethodOperator; }
            set { conversionMethodOperator = value; }
        }

        DateTime rateTime;

        public DateTime RateTime
        {
            get { return rateTime; }
            set { rateTime = value; }
        }

    }
}
