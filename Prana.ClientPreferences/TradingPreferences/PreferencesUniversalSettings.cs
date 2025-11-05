using Prana.BusinessObjects;
using System;
namespace Prana.ClientPreferences
{
    /// <summary>
    /// Summary description for PreferencesUniversalSettings.
    /// </summary>
    [Serializable]
    public class PreferencesUniversalSettings : IPreferenceData
    {
        public PreferencesUniversalSettings()
        {

        }
        public PreferencesUniversalSettings(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            _venueID = venueID.ToString();
            _counterPartyID = counterPartyID.ToString();
            _assetID = assetID.ToString();
            _underlyingID = underlyingID.ToString();
        }

        #region Private Variables

        private string _assetID = string.Empty;
        private string _underlyingID = string.Empty;
        private string _tradingAccountID = string.Empty;
        private string _accountID = string.Empty;
        private string _strategyID = string.Empty;
        private string _borrowerFirmID = string.Empty;

        private string _quantity = string.Empty;
        private string _displayQuantity = string.Empty;
        private string _quantityIncrement = string.Empty;
        private string _priceLimitIncrement = string.Empty;
        private string _stopPriceIncrement = string.Empty;
        private string _pegOffset = string.Empty;
        private string _discrOffset = string.Empty;
        private string _counterPartyID = string.Empty;
        private string _venueID = string.Empty;
        private string _orderTypeID = string.Empty;
        private string _executionInstructionID = string.Empty;
        private string _handlingInstructionID = string.Empty;
        private string _TIF = string.Empty;
        private bool _isDefaultCV = false;

        private int _companyUserID = int.MinValue;
        private int _cmtaID = int.MinValue;
        private string _cmta = string.Empty;
        private int _giveUpID = int.MinValue;
        private string _giveUp = string.Empty;
        private string _orderSide = string.Empty;
        private string _settlCurrency = string.Empty;

        /// <summary>
        /// The _is quantity default value checked
        /// </summary>
        private bool _isQuantityDefaultValueChecked = false;

        #endregion


        #region Properties
        public string AssetID
        {
            get { return this._assetID; }
            set { this._assetID = value; }
        }
        public string UnderlyingID
        {
            get { return this._underlyingID; }
            set { this._underlyingID = value; }
        }
        public string TradingAccountID
        {
            get { return _tradingAccountID; }
            set { _tradingAccountID = value; }
        }
        public string AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
        public string StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }
        public string BorrowerFirmID
        {
            get { return _borrowerFirmID; }
            set { _borrowerFirmID = value; }
        }

        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public string DisplayQuantity
        {
            get { return _displayQuantity; }
            set { _displayQuantity = value; }
        }
        public string QuantityIncrement
        {
            get { return _quantityIncrement; }
            set { _quantityIncrement = value; }
        }
        public string PriceLimitIncrement
        {
            get { return _priceLimitIncrement; }
            set { _priceLimitIncrement = value; }
        }
        public string StopPriceIncrement
        {
            get { return _stopPriceIncrement; }
            set { _stopPriceIncrement = value; }
        }
        public string PegOffset
        {
            get { return _pegOffset; }
            set { _pegOffset = value; }
        }
        public string DiscrOffset
        {
            get { return _discrOffset; }
            set { _discrOffset = value; }
        }
        public string CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }
        public string VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }
        public string OrderTypeID
        {
            get { return _orderTypeID; }
            set { _orderTypeID = value; }
        }
        public string ExecutionInstructionID
        {
            get { return _executionInstructionID; }
            set { _executionInstructionID = value; }
        }
        public string HandlingInstructionID
        {
            get { return _handlingInstructionID; }
            set { _handlingInstructionID = value; }
        }
        public string TIF
        {
            get { return _TIF; }
            set { _TIF = value; }
        }

        public bool IsDefaultCV
        {
            get { return _isDefaultCV; }
            set { _isDefaultCV = value; }
        }
        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }
        public int CMTAID
        {
            get { return _cmtaID; }
            set { _cmtaID = value; }
        }
        public string CMTA
        {
            get { return _cmta; }
            set { _cmta = value; }
        }
        public int GiveUpID
        {
            get { return _giveUpID; }
            set { _giveUpID = value; }
        }
        public string GiveUp
        {
            get { return _giveUp; }
            set { _giveUp = value; }
        }

        public string PrefID
        {
            get
            {
                return IDGenerator.GetAUCVID(int.Parse(_assetID), int.Parse(_underlyingID), int.Parse(_counterPartyID), int.Parse(_venueID));
            }

        }

        public string OrderSide
        {

            get
            {
                return _orderSide;
            }
            set
            {
                _orderSide = value;
            }
        }
        public string SettlCurrency
        {

            get
            {
                return _settlCurrency;
            }
            set
            {
                _settlCurrency = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is quantity default value checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is quantity default value checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsQuantityDefaultValueChecked
        {
            get { return _isQuantityDefaultValueChecked; }
            set { _isQuantityDefaultValueChecked = value; }
        }

        #endregion
    }
}
