using Prana.BusinessObjects;
using System;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AUEC.
    /// </summary>
    public class AUEC
    {

        #region Private members

        private int _auecID = int.MinValue;

        private int _assetID = int.MinValue;
        private int _underlyingID = int.MinValue;
        private int _exchangeID = int.MinValue;
        private int _currencyID = int.MinValue;

        private int _counterPartyVenueID = int.MinValue;

        private Asset _asset = null;
        private UnderLying _underlying = null;
        private Exchange _exchange = null;
        private Currency _currency = null;
        //SK 20061009 Compliance is removed from AUEC
        //private Compliance _compliance = null;
        private MarketFee _marketFees = null;

        private int _companyAUECID = int.MinValue;
        private int _companyID = int.MinValue;

        private int _companyUserID = int.MinValue;
        private int _companyUserAUECID = int.MinValue;

        private int _cvAUECID = int.MinValue;


        private string _fullName = string.Empty;
        private string _displayName = string.Empty;
        private int _unitID = int.MinValue;
        private string _timeZone = string.Empty;
        double _timeZoneOffSet = double.MinValue;
        private DateTime _regularTradingStartTime = DateTimeConstants.MinValue;
        private DateTime _regularTradingEndTime = DateTimeConstants.MinValue;
        private DateTime _preMarketTradingStartTime = DateTimeConstants.MinValue;
        private DateTime _preMarketTradingEndTime = DateTimeConstants.MinValue;
        private DateTime _lunchTimeStartTime = DateTimeConstants.MinValue;
        private DateTime _lunchTimeEndTime = DateTimeConstants.MinValue;
        private DateTime _postMarketTradingStartTime = DateTimeConstants.MinValue;
        private DateTime _postMarketTradingEndTime = DateTimeConstants.MinValue;

        private int _settlementDaysBuy = int.MinValue;
        private int _settlementDaysSell = int.MinValue;
        private string _dayLightSaving = string.Empty;

        private int _country = int.MinValue;
        private int _stateID = int.MinValue;

        private int _preMarketCheck = int.MinValue;
        private int _postMarketCheck = int.MinValue;

        private int _regularTimeCheck = int.MinValue;
        private int _lunchTimeCheck = int.MinValue;

        private double _multiplier = double.MinValue;

        /// <summary>
        /// declare roundlot, PRANA-11159 
        /// </summary>
        private decimal _roundLot = decimal.MinValue;

        private int _unit = int.MinValue;
        private int _identifierID = int.MinValue;

        private int _symbolConventionID = int.MinValue;
        private string _exchangeIdentifier = string.Empty;
        private string _marketDataProviderExchangeIdentifier = string.Empty;

        private int _currencyConversion = int.MinValue;

        private int _countryFlagID = int.MinValue;
        private int _logoID = int.MinValue;

        //private int _baseCurrencyID = int.MinValue;
        private int _otherCurrencyID = int.MinValue;

        private double _purchaseSecFees = 0;
        private double _saleSecFees = 0;
        private double _purchaseStamp = 0;
        private double _saleStamp = 0;
        private double _purchaseLevy = 0;
        private double _saleLevy = 0;

        //private int _isShortSaleAllowed = int.MinValue;
        private int _isShortSaleConfirmation = int.MinValue;
        private int _provideAccountNameWithTrade = int.MinValue;
        private int _provideIdentifierNameWithTrade = int.MinValue;


        private int _auecExchangeID = int.MinValue;   //TODO: remove this property.

        private string _auecString = string.Empty;
        #endregion

        #region Constructors

        public AUEC()
        {
        }

        public AUEC(int auecID)
        {
            _auecID = auecID;
        }

        public AUEC(int auecID, int assetID, int underlyingID, int exchangeID, int currencyID)
        {
            _auecID = auecID;
            _assetID = assetID;
            _underlyingID = underlyingID;
            _exchangeID = exchangeID;
            _currencyID = currencyID;
        }

        #endregion

        #region Properties

        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        public int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }

        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }

        public int CounterPartyVenueID
        {
            get { return _counterPartyVenueID; }
            set { _counterPartyVenueID = value; }
        }

        public Asset Asset
        {
            get
            {
                if (_asset == null)
                {
                    _asset = AssetManager.GetAssets(_assetID);
                }
                return _asset;
            }
        }

        public UnderLying UnderLying
        {
            get
            {
                if (_underlying == null)
                {
                    _underlying = AssetManager.GetUnderLying(_underlyingID);
                }
                return _underlying;
            }
        }

        public Exchange Exchange
        {
            get
            {
                if (_exchange == null)
                {
                    //_exchange = ExchangeManager.GetExchange(_exchangeID);
                    //_exchange = AUECManager.GetAUECExchange(AUECID, _exchangeID);
                    if (_auecID < 0)
                    {
                        _exchange = ExchangeManager.GetExchange(_exchangeID);
                    }
                    else
                    {
                        _exchange = ExchangeManager.GetExchange(_exchangeID); //Uncommented on 18th Oct, 2007
                                                                              //_exchange = AUECManager.GetAUECExchange(_auecID);
                    }
                }
                return _exchange;
            }
        }

        public Currency Currency
        {
            get
            {
                if (_currency == null)
                {
                    _currency = AUECManager.GetCurrency(_currencyID);
                }
                return _currency;
            }
        }

        public MarketFee MarketFee
        {
            get
            {
                if (_marketFees == null)
                {
                    _marketFees = AUECManager.GetMarketFees(_auecExchangeID);
                }
                return _marketFees;
            }
            set { _marketFees = value; }
        }

        //SK 20061009 Compliance is removed from AUEC
        //public Compliance Compliance
        //{
        //    get
        //    {
        //        if(_compliance == null)
        //        {
        //            _compliance = AUECManager.GetCompliance(_auecID);
        //        }
        //        return _compliance;
        //    }
        //    set{_compliance = value;}
        //}

        public int CompanyAUECID
        {
            get { return _companyAUECID; }
            set { _companyAUECID = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }

        public int CompanyUserAUECID
        {
            get { return _companyUserAUECID; }
            set { _companyUserAUECID = value; }
        }

        public int CVAUECID
        {
            get { return _cvAUECID; }
            set { _cvAUECID = value; }
        }


        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        public int Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        public string TimeZone
        {
            get { return _timeZone; }
            set { _timeZone = value; }
        }

        public double TimeZoneOffSet
        {
            get { return _timeZoneOffSet; }
            set { _timeZoneOffSet = value; }
        }

        public DateTime RegularTradingStartTime
        {
            get { return _regularTradingStartTime; }
            set { _regularTradingStartTime = value; }
        }

        public DateTime RegularTradingEndTime
        {
            get { return _regularTradingEndTime; }
            set { _regularTradingEndTime = value; }
        }

        public DateTime PreMarketTradingStartTime
        {
            get { return _preMarketTradingStartTime; }
            set { _preMarketTradingStartTime = value; }
        }

        public DateTime PreMarketTradingEndTime
        {
            get { return _preMarketTradingEndTime; }
            set { _preMarketTradingEndTime = value; }
        }

        public DateTime LunchTimeStartTime
        {
            get { return _lunchTimeStartTime; }
            set { _lunchTimeStartTime = value; }
        }

        public DateTime LunchTimeEndTime
        {
            get { return _lunchTimeEndTime; }
            set { _lunchTimeEndTime = value; }
        }

        public DateTime PostMarketTradingStartTime
        {
            get { return _postMarketTradingStartTime; }
            set { _postMarketTradingStartTime = value; }
        }

        public DateTime PostMarketTradingEndTime
        {
            get { return _postMarketTradingEndTime; }
            set { _postMarketTradingEndTime = value; }
        }

        public int SettlementDaysBuy
        {
            get { return _settlementDaysBuy; }
            set { _settlementDaysBuy = value; }
        }

        public int SettlementDaysSell
        {
            get { return _settlementDaysSell; }
            set { _settlementDaysSell = value; }
        }

        public string DayLightSaving
        {
            get { return _dayLightSaving; }
            set { _dayLightSaving = value; }
        }

        public int Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public int StateID
        {
            get { return _stateID; }
            set { _stateID = value; }
        }


        public int PreMarketCheck
        {
            get { return _preMarketCheck; }
            set { _preMarketCheck = value; }
        }

        public int PostMarketCheck
        {
            get { return _postMarketCheck; }
            set { _postMarketCheck = value; }
        }

        public int RegularTimeCheck
        {
            get { return _regularTimeCheck; }
            set { _regularTimeCheck = value; }
        }

        public int LunchTimeCheck
        {
            get { return _lunchTimeCheck; }
            set { _lunchTimeCheck = value; }
        }

        public int SymbolConventionID
        {
            get { return _symbolConventionID; }
            set { _symbolConventionID = value; }
        }

        public string ExchangeIdentifier
        {
            get { return _exchangeIdentifier; }
            set { _exchangeIdentifier = value; }
        }

        public string MarketDataProviderExchangeIdentifier
        {
            get { return _marketDataProviderExchangeIdentifier; }
            set { _marketDataProviderExchangeIdentifier = value; }
        }

        public int CurrencyConversion
        {
            get { return _currencyConversion; }
            set { _currencyConversion = value; }
        }

        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }
        /// <summary>
        /// get set for roundlot, PRANA-11159
        /// </summary>
        public decimal RoundLot
        {
            get { return _roundLot; }
            set { _roundLot = value; }
        }

        public int IdentifierID
        {
            get { return _identifierID; }
            set { _identifierID = value; }
        }

        public int CountryFlagID
        {
            get { return _countryFlagID; }
            set { _countryFlagID = value; }
        }

        public int LogoID
        {
            get { return _logoID; }
            set { _logoID = value; }
        }

        public double PurchaseSecFees
        {
            get { return _purchaseSecFees; }
            set { _purchaseSecFees = value; }
        }

        public double SaleSecFees
        {
            get { return _saleSecFees; }
            set { _saleSecFees = value; }
        }

        public double PurchaseStamp
        {
            get { return _purchaseStamp; }
            set { _purchaseStamp = value; }
        }

        public double SaleStamp
        {
            get { return _saleStamp; }
            set { _saleStamp = value; }
        }

        public double PurchaseLevy
        {
            get { return _purchaseLevy; }
            set { _purchaseLevy = value; }
        }

        public double SaleLevy
        {
            get { return _saleLevy; }
            set { _saleLevy = value; }
        }

        public int OtherCurrencyID
        {
            get { return _otherCurrencyID; }
            set { _otherCurrencyID = value; }
        }

        public int IsShortSaleConfirmation
        {
            get { return _isShortSaleConfirmation; }
            set { _isShortSaleConfirmation = value; }
        }

        public int ProvideAccountNameWithTrade
        {
            get { return _provideAccountNameWithTrade; }
            set { _provideAccountNameWithTrade = value; }
        }

        public int ProvideIdentifierNameWithTrade
        {
            get { return _provideIdentifierNameWithTrade; }
            set { _provideIdentifierNameWithTrade = value; }
        }

        public int UnitID
        {
            get { return _unitID; }
            set { _unitID = value; }
        }

        //public int AUECExchangeID
        //{
        //    get { return _auecExchangeID; }
        //    set { _auecExchangeID = value; }
        //}

        public string AUECString
        {
            get
            {
                if (_auecID > 0)
                {
                    _auecString = Asset.Name.ToString() + "/" + UnderLying.Name.ToString() + "/" + DisplayName.ToString() + "/" + Currency.CurrencySymbol.ToString();
                }
                else
                {
                    _auecString = Asset.Name.ToString() + "/" + UnderLying.Name.ToString() + "/" + Exchange.DisplayName.ToString() + "/" + Currency.CurrencySymbol.ToString();
                }
                return _auecString;
            }
            set { _auecString = value; }
        }

        #endregion
    }
}
