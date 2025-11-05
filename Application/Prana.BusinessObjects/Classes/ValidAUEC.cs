using System.ComponentModel;

namespace Prana.BusinessObjects
{
    public class ValidAUEC
    {
        public ValidAUEC()
        {
        }

        private int _auecID;
        public int AuecID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        private string _auecText;
        public string ExchangeIdentifier
        {
            get { return _auecText; }
            set { _auecText = value; }
        }

        private int _assetID = int.MinValue;
        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        private string _asset;
        public string Asset
        {
            get { return _asset; }
            set { _asset = value; }
        }

        private int _underlyingID = int.MinValue;
        public int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }


        private string _underlying;
        public string Underlying
        {
            get { return _underlying; }
            set { _underlying = value; }
        }

        private int _exchangeID = int.MinValue;
        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        private string _exchange;
        public string Exchange
        {
            get { return _exchange; }
            set { _exchange = value; }
        }

        private int _currencyID = int.MinValue;
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }

        private string _currency;
        public string DefaultCurrency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        private int _roundOff = 0;
        public int RoundOff
        {
            get { return _roundOff; }
            set { _roundOff = value; }
        }

        private bool _isApplied = false;
        [Browsable(false)]
        public bool IsApplied
        {
            get { return _isApplied; }
            set { _isApplied = value; }
        }
        /// <summary>
        /// Added to get and set multiplier value, PRANA-10856
        /// </summary>
        private double _multiplier = 1;
        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }
        /// <summary>
        /// Added to get and set roundlot value, PRANA-10856
        /// </summary>
        private decimal _roundLot = 1;
        public decimal RoundLot
        {
            get { return _roundLot; }
            set { _roundLot = value; }
        }
    }
}
