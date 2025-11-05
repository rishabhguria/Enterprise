using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects
{
    public class TradeParametersArgs : EventArgs
    {
        private string _symbol;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        //Uncommented on 03 Aug 2007 - Rajat
        private string _OpenClose;
        //Uncommented on 03 Aug 2007 - Rajat
        /// <summary>
        /// Gets or sets the OpenClose.
        /// </summary>
        /// <value>The OpenClose.</value>
        public string OpenClose
        {
            get { return _OpenClose; }
            set { _OpenClose = value; }
        }

        private string _underlyingSymbol;

        /// <summary>
        /// Gets or sets the underlying symbol.
        /// </summary>
        /// <value>The underlying symbol.</value>
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        private string _orderSide;
        /// <summary>
        /// Gets or sets the order side.
        /// </summary>
        /// <value>The order side.</value>
        public string OrderSide
        {
            get { return _orderSide; }
            set { _orderSide = value; }
        }

        private double _price;

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        #region AUEC information

        private AssetCategory _assetId;

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        /// <value>The asset id.</value>
        public AssetCategory AssetId
        {
            get { return _assetId; }
            set { _assetId = value; }
        }

        private Underlying _underlyingId;

        /// <summary>
        /// Gets or sets the underlying id.
        /// </summary>
        /// <value>The underlying id.</value>
        public Underlying UnderlyingId
        {
            get { return _underlyingId; }
            set { _underlyingId = value; }
        }

        private int _listedExchangeId;

        /// <summary>
        /// Gets or sets the exchange id.
        /// </summary>
        /// <value>The exchange id.</value>
        public int ListedExchangeId
        {
            get { return _listedExchangeId; }
            set { _listedExchangeId = value; }
        }

        private int _currencyId;

        /// <summary>
        /// Gets or sets the currency id.
        /// </summary>
        /// <value>The currency id.</value>
        public int CurrencyId
        {
            get { return _currencyId; }
            set { _currencyId = value; }
        }

        private int _auecId;

        /// <summary>
        /// Gets or sets the AUEC id.
        /// </summary>
        /// <value>The AUEC id.</value>
        public int AUECId
        {
            get { return _auecId; }
            set { _auecId = value; }
        }

        private double _quantity = double.Epsilon;

        /// <summary>
        /// Gets or sets the Quantity id.
        /// </summary>
        /// <value>The Quantity id.</value>
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        #endregion

        #region Options/Futures specific information

        private double _strikePrice;

        /// <summary>
        /// Gets or sets the strike price.
        /// </summary>
        /// <value>The strike price.</value>
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        private int _expirationMonth;

        /// <summary>
        /// Gets or sets the expiration month.
        /// </summary>
        /// <value>The expiration month.</value>
        public int ExpirationMonth
        {
            get { return _expirationMonth; }
            set { _expirationMonth = value; }
        }

        private int _expirationDay;

        public int ExpirationDay
        {
            get { return _expirationDay; }
            set { _expirationDay = value; }
        }

        private string _underlyingPutOrCall;

        /// <summary>
        /// Gets or sets the underlying's put or call.
        /// </summary>
        /// <value>The underlying put or call.</value>
        public string UnderlyingPutOrCall
        {
            get { return _underlyingPutOrCall; }
            set { _underlyingPutOrCall = value; }
        }

        #endregion

        #region Allocation Type Information

        private int _accountID = int.MinValue;

        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }


        private int _strategyID = int.MinValue;

        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        #endregion Allocation Type Information
    }
}
