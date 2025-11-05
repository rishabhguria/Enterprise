using Prana.BusinessObjects;
using System;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Exchange.
    /// </summary>
    public class Exchange
    {
        //TODO: Remove unwanted field n members from the class.


        #region Private members

        private int _auecID = int.MinValue;
        private int _exchangeID = int.MinValue;
        private int _auecExchangeID = int.MinValue;

        private string _fullName = string.Empty;
        private string _displayName = null;

        private int _currency = int.MinValue;
        private int _unit = int.MinValue;

        private string _timeZone = string.Empty;
        private double _timeZoneOffSet = double.MinValue;
        private DateTime _regularTradingStartTime = DateTimeConstants.MinValue;
        private DateTime _regularTradingEndTime = DateTimeConstants.MinValue;
        private DateTime _preMarketTradingStartTime = DateTimeConstants.MinValue;
        private DateTime _preMarketTradingEndTime = DateTimeConstants.MinValue;
        private DateTime _lunchTimeStartTime = DateTimeConstants.MinValue;
        private DateTime _lunchTimeEndTime = DateTimeConstants.MinValue;
        private DateTime _postMarketTradingStartTime = DateTimeConstants.MinValue;
        private DateTime _postMarketTradingEndTime = DateTimeConstants.MinValue;

        private int _settlementDays = int.MinValue;
        private string _dayLightSaving = string.Empty;

        private int _country = int.MinValue;
        private int _stateID = int.MinValue;

        private int _preMarketCheck = int.MinValue;
        private int _postMarketCheck = int.MinValue;

        private int _regularTimeCheck = int.MinValue;
        private int _lunchTimeCheck = int.MinValue;

        //New enterants:
        private int _symbolConventionID = int.MinValue;
        private string _exchangeIdentifier = string.Empty;

        private int _currencyConversion = int.MinValue;

        private int _countryFlagID = int.MinValue;
        private int _logoID = int.MinValue;

        #endregion

        #region Constructors

        public Exchange()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public Exchange(int exchangeID, string name)
        {
            _exchangeID = exchangeID;
            _displayName = name;
        }

        #endregion

        #region Properties

        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        public int AUECExchangeID
        {
            get { return _auecExchangeID; }
            set { _auecExchangeID = value; }
        }

        public string Name
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public int Currency
        {
            get { return _currency; }
            set { _currency = value; }
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

        public int SettlementDays
        {
            get { return _settlementDays; }
            set { _settlementDays = value; }
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

        public int CurrencyConversion
        {
            get { return _currencyConversion; }
            set { _currencyConversion = value; }
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
        #endregion
    }
}
