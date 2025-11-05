using System;

namespace Prana.BusinessObjects.Compliance.DataSendingObjects
{
    /// <summary>
    /// Definition for auec details
    /// Used to send data to esper
    /// </summary>
    public class AuecDetails
    {
        int auecId;

        public int AuecId
        {
            get { return auecId; }
            set { auecId = value; }
        }
        DateTime yesterDay;

        public DateTime YesterDay
        {
            get { return yesterDay; }
            set { yesterDay = value; }
        }
        DateTime today;

        public DateTime Today
        {
            get { return today; }
            set { today = value; }
        }

        int assetId;

        public int AssetId
        {
            get { return assetId; }
            set { assetId = value; }
        }
        String asset;

        public String Asset
        {
            get { return asset; }
            set { asset = value; }
        }

        int exchangeId;

        public int ExchangeId
        {
            get { return exchangeId; }
            set { exchangeId = value; }
        }

        String exchangeName;

        public String ExchangeName
        {
            get { return exchangeName; }
            set { exchangeName = value; }
        }

        int underlyingId;

        public int UnderlyingId
        {
            get { return underlyingId; }
            set { underlyingId = value; }
        }

        String underlying;

        public String Underlying
        {
            get { return underlying; }
            set { underlying = value; }
        }

        int currencyId;

        public int CurrencyId
        {
            get { return currencyId; }
            set { currencyId = value; }
        }


        String currency;

        public String Currency
        {
            get { return currency; }
            set { currency = value; }
        }
    }
}
