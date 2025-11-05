using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects.LiveFeed
{
    /// <summary>
    /// Class to store advice details
    /// </summary>
    public class AdviceSymbolInfo
    {
        public AdviceSymbolInfo()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AdviceSymbolInfo" /> class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public AdviceSymbolInfo(string symbol)
        {
            this._symbol = symbol;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdviceSymbolInfo" /> class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="fromCurrency">From currency.</param>
        /// <param name="toCurrency">To currency.</param>
        /// <param name="assetId">The asset identifier.</param>
        public AdviceSymbolInfo(string symbol, int fromCurrency, int toCurrency, AssetCategory assetId)
        {
            this._symbol = symbol;
            this._fromCurrency = fromCurrency;
            this._toCurrency = toCurrency;
            this._assetId = assetId;
        }

        /// <summary>
        /// The symbol
        /// </summary>
        private String _symbol;
        /// <summary>
        /// From currency
        /// </summary>
        private int _fromCurrency;
        /// <summary>
        /// To currency
        /// </summary>
        private int _toCurrency;
        /// <summary>
        /// The asset identifier
        /// </summary>
        private AssetCategory _assetId;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public String Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        /// <summary>
        /// Gets or sets from currency identifier.
        /// </summary>
        /// <value>
        /// From currency identifier.
        /// </value>
        public int FromCurrencyId
        {
            get { return _fromCurrency; }
            set { _fromCurrency = value; }
        }

        /// <summary>
        /// Gets or sets to currency identifier.
        /// </summary>
        /// <value>
        /// To currency identifier.
        /// </value>
        public int ToCurrencyId
        {
            get { return _toCurrency; }
            set { _toCurrency = value; }
        }

        /// <summary>
        /// Gets or sets the asset identifier.
        /// </summary>
        /// <value>
        /// The asset identifier.
        /// </value>
        public AssetCategory AssetId
        {
            get { return _assetId; }
            set { _assetId = value; }
        }
    }
}
