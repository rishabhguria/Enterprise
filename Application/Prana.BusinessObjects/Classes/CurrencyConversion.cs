using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Rajat changed 25 Sep 2006 = Made serializable.
    /// Summary description for CurrencyConversion.
    /// </summary>
    [Serializable]
    public class CurrencyConversion
    {
        #region private variables
        private int _fromCurrencyID = Int32.MinValue;
        private string _fromCurrencyName = string.Empty;
        private int _toCurrencyID = Int32.MinValue;
        private string _toCurrencyName = string.Empty;
        private Double _currencyConversionFactor = float.MinValue;

        #endregion

        #region Constructor
        public CurrencyConversion()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Properties
        public int FromCurrencyID
        {
            get
            {
                return this._fromCurrencyID;
            }

            set
            {
                this._fromCurrencyID = value;
            }
        }

        public string FromCurrencyName
        {
            get
            {
                return this._fromCurrencyName;
            }

            set
            {
                this._fromCurrencyName = value.Trim().ToUpper();

            }
        }

        public int ToCurrencyID
        {
            get
            {
                return this._toCurrencyID;
            }

            set
            {
                this._toCurrencyID = value;
            }
        }

        public string ToCurrencyName
        {
            get
            {
                return this._toCurrencyName;
            }

            set
            {
                this._toCurrencyName = value.Trim().ToUpper();

            }
        }

        public Double CurrencyConversionFactor
        {
            get
            {
                return this._currencyConversionFactor;
            }

            set
            {
                this._currencyConversionFactor = value;
            }
        }

        private bool _isDirectConversion;

        /// <summary>
        /// Rajat modified 25 Sep 2006
        /// Gets or sets a value indicating whether this instance is direct conversion.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is direct conversion; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirectConversion
        {
            get { return _isDirectConversion; }
            set { _isDirectConversion = value; }
        }

        private string _currencyPairSymbol;

        /// <summary>
        /// Rajat modified 25 Sep 2006
        /// Gets or sets the symbol of currency pair .
        /// e.g. JPY-USD (Must include the dash between the currency pair)
        /// </summary>
        /// <value>The currency pair symbol.</value>
        public string CurrencyPairSymbol
        {
            get { return _currencyPairSymbol; }
            set { _currencyPairSymbol = value; }
        }

        #endregion

    }
}
