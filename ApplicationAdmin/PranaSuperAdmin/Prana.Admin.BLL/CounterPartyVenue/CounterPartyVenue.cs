namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CounterPartyVenue.
    /// </summary>
    public class CounterPartyVenue
    {
        #region Private and protected members.

        private int _counterPartyVenueDetailsID = int.MinValue;
        private int _counterPartyVenueID = int.MinValue;
        private int _counterPartyID = int.MinValue;
        private int _venueID = int.MinValue;
        private string _displayName = string.Empty;
        private int _isElectronic = int.MinValue;

        private int _auecID = int.MinValue;
        private int _symbolConventionID = int.MinValue;

        //New enterants:
        private string _oatsIdentifier = string.Empty;
        private int _baseCurrencyID = int.MinValue;
        private int _otherCurrencyID = int.MinValue;
        private int _currencyTypeID = int.MinValue;

        private int _cvAUECComplianceID = int.MinValue;
        private int _cvAUECID = int.MinValue;
        private int _followCompliance = int.MinValue;
        private int _shortSellConfirmation = int.MinValue;
        private int _identifierID = int.MinValue;
        private string _foreignID = string.Empty;

        private int _cvFIXID = int.MinValue;
        private string _acronym = string.Empty;
        private int _fixVersionID = int.MinValue;
        private string _targetCompID = string.Empty;
        private string _deliverToCompID = string.Empty;
        private string _deliverToSubID = string.Empty;

        private int _companyCounterPartyCVID = int.MinValue;


        //TODO: Remove the following members as they are no longer needed. 
        private string _fixIdentifier = string.Empty;
        private string _sideID = string.Empty;
        private string _orderTypesID = string.Empty;
        private string _timeInForceID = string.Empty;
        private string _handlingInstructionsID = string.Empty;
        private string _executionInstructionsID = string.Empty;
        private int _advancedOrdersID = int.MinValue;


        #endregion

        public CounterPartyVenue()
        {
        }
        public CounterPartyVenue(int counterPartyVenueID, string counterPartyVenueName)
        {
            _counterPartyVenueID = counterPartyVenueID;
            _displayName = counterPartyVenueName;
        }



        #region Properties

        public int CounterPartyVenueDetailsID
        {
            get { return _counterPartyVenueDetailsID; }
            set { _counterPartyVenueDetailsID = value; }
        }

        public int CounterPartyVenueID
        {
            get { return _counterPartyVenueID; }
            set { _counterPartyVenueID = value; }
        }

        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }

        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public int IsElectronic
        {
            get { return _isElectronic; }
            set { _isElectronic = value; }
        }

        public string FixIdentifier
        {
            get { return _fixIdentifier; }
            set { _fixIdentifier = value; }
        }
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }
        public int SymbolConventionID
        {
            get { return _symbolConventionID; }
            set { _symbolConventionID = value; }
        }

        public string SideID
        {
            get { return _sideID; }
            set { _sideID = value; }
        }
        public string OrderTypesID
        {
            get { return _orderTypesID; }
            set { _orderTypesID = value; }
        }
        public string TimeInForceID
        {
            get { return _timeInForceID; }
            set { _timeInForceID = value; }
        }
        public string HandlingInstructionsID
        {
            get { return _handlingInstructionsID; }
            set { _handlingInstructionsID = value; }
        }
        public string ExecutionInstructionsID
        {
            get { return _executionInstructionsID; }
            set { _executionInstructionsID = value; }
        }
        public int AdvancedOrdersID
        {
            get { return _advancedOrdersID; }
            set { _advancedOrdersID = value; }
        }

        public string OatsIdentifier
        {
            get { return _oatsIdentifier; }
            set { _oatsIdentifier = value; }
        }
        public int BaseCurrencyID
        {
            get { return _baseCurrencyID; }
            set { _baseCurrencyID = value; }
        }
        public int OtherCurrencyID
        {
            get { return _otherCurrencyID; }
            set { _otherCurrencyID = value; }
        }
        public int CurrencyTypeID
        {
            get { return _currencyTypeID; }
            set { _currencyTypeID = value; }
        }

        public int CVAUECComplianceID
        {
            get { return _cvAUECComplianceID; }
            set { _cvAUECComplianceID = value; }
        }

        public int CVAUECID
        {
            get { return _cvAUECID; }
            set { _cvAUECID = value; }
        }

        public int FollowCompliance
        {
            get { return _followCompliance; }
            set { _followCompliance = value; }
        }

        public int ShortSellConfirmation
        {
            get { return _shortSellConfirmation; }
            set { _shortSellConfirmation = value; }
        }

        public int IdentifierID
        {
            get { return _identifierID; }
            set { _identifierID = value; }
        }

        public string ForeignID
        {
            get { return _foreignID; }
            set { _foreignID = value; }
        }

        public int CVFIXID
        {
            get { return _cvFIXID; }
            set { _cvFIXID = value; }
        }

        public string Acronym
        {
            get { return _acronym; }
            set { _acronym = value; }
        }

        public int FixVersionID
        {
            get { return _fixVersionID; }
            set { _fixVersionID = value; }
        }

        public string TargetCompID
        {
            get { return _targetCompID; }
            set { _targetCompID = value; }
        }

        public string DeliverToCompID
        {
            get { return _deliverToCompID; }
            set { _deliverToCompID = value; }
        }

        public string DeliverToSubID
        {
            get { return _deliverToSubID; }
            set { _deliverToSubID = value; }
        }

        public int CompanyCounterPartyCVID
        {
            get { return _companyCounterPartyCVID; }
            set { _companyCounterPartyCVID = value; }
        }
        #endregion

        private string _counterPartyName = string.Empty;
        public string CounterPartyName
        {
            get
            {
                if (_counterPartyName == string.Empty)
                {
                    CounterParty counterParty = CounterPartyManager.GetCounterParty(_counterPartyID);
                    if (counterParty != null)
                    {
                        _counterPartyName = counterParty.CounterPartyFullName;
                    }
                }
                return _counterPartyName;
            }
        }

        private string _venueName = string.Empty;
        public string VenueName
        {
            get
            {
                if (_venueName == string.Empty)
                {
                    Venue venue = VenueManager.GetVenue(_venueID);
                    if (venue != null)
                    {
                        _venueName = venue.VenueName;
                    }
                }
                return _venueName;
            }
        }
    }
}
