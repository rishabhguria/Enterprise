using Prana.BusinessObjects;
using System;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Company.
    /// </summary>
    public class Company
    {
        //TODO: To allocate regions for the respective tabs.

        #region Private members

        private int _companyID = int.MinValue;
        private string _name = string.Empty;
        private string _address1 = string.Empty;
        private string _address2 = string.Empty;
        private int _companyTypeID = int.MinValue;
        private string _telephone = string.Empty;
        private string _fax = string.Empty;
        private string _primaryContactFirstName = string.Empty;
        private string _primaryContactLastName = string.Empty;
        private string _primaryContactTitle = string.Empty;
        private string _primaryContactEMail = string.Empty;
        private string _primaryContactTelephone = string.Empty;
        private string _primaryContactCell = string.Empty;
        private string _secondaryContactFirstName = string.Empty;
        private string _secondaryContactLastName = string.Empty;
        private string _secondaryContactTitle = string.Empty;
        private string _secondaryContactEMail = string.Empty;
        private string _secondaryContactTelephone = string.Empty;
        private string _secondaryContactCell = string.Empty;
        private string _technologyContactFirstName = string.Empty;
        private string _technologyContactLastName = string.Empty;
        private string _technologyContactTitle = string.Empty;
        private string _technologyContactEMail = string.Empty;
        private string _technologyContactTelephone = string.Empty;
        private string _technologyContactCell = string.Empty;
        private int _mpid = int.MinValue; //Changed from Compliance
                                          //New enterants
        private int _companyMPID = int.MinValue;

        private int _companyComplianceID = int.MinValue;
        private int _baseCurrencyID = int.MinValue;
        private int _supportsMultipleCurrency = int.MinValue;
        private int _fixVersionID = int.MinValue;
        private int _fixCapabilityID = int.MinValue;
        private int _multipleCurrencyID = int.MinValue;
        private int _companyAllCurrencyID = int.MinValue;

        private int _companyBorrowerID = int.MinValue;
        private string _borrowerName = string.Empty;
        private string _borrowerShortName = string.Empty;
        private string _firmID = string.Empty;

        private string _shortName = string.Empty;
        private string _loginName = string.Empty;
        private string _password = string.Empty;
        private int _countryID = int.MinValue;
        private int _stateID = int.MinValue;
        private string _zip = string.Empty;
        private string _city = string.Empty;

        //Modified By: Bharat Raturi, Date: 05/March/2014
        //purpose: Variable for adding one more detail of company
        private string _region = string.Empty;
        private bool _sendAllocationsViaFix = false;

        #region Venue Members
        private int _companyVenueID = int.MinValue;
        private string _venueName = string.Empty;
        private string _venueShortName = string.Empty;
        private int _venueType = int.MinValue;

        private int _timeZone = int.MinValue;
        private string _emailAlert = string.Empty;
        private DateTime _regularTradingStartTime = DateTimeConstants.MinValue;
        private DateTime _regularTradingEndTime = DateTimeConstants.MinValue;
        private DateTime _preMarketTradingStartTime = DateTimeConstants.MinValue;
        private DateTime _preMarketTradingEndTime = DateTimeConstants.MinValue;
        private DateTime _lunchTimeStartTime = DateTimeConstants.MinValue;
        private DateTime _lunchTimeEndTime = DateTimeConstants.MinValue;
        private DateTime _postMarketTradingStartTime = DateTimeConstants.MinValue;
        private DateTime _postMarketTradingEndTime = DateTimeConstants.MinValue;

        private int _preMarketCheck = int.MinValue;
        private int _postMarketCheck = int.MinValue;

        private int _regularTimeCheck = int.MinValue;
        private int _lunchTimeCheck = int.MinValue;
        #endregion

        #endregion

        #region Constructors

        public Company()

        {
        }

        public Company(int companyTypeID, string name)
        {
            _companyTypeID = companyTypeID;
            _name = name;
        }
        public Company(int companyTypeID, string name, string borrowerName)
        {
            _companyTypeID = companyTypeID;
            _name = name;
            _borrowerName = borrowerName;
        }
        public Company(string borrowerName, string borrowerShortName, string firmID)
        {
            _borrowerName = borrowerName;
            _borrowerShortName = borrowerShortName;
            _firmID = firmID;
        }
        //public Company(int baseCurrencyID, int supportsMultipleCurrency, int multipleCurrencyID)
        //{
        //    _baseCurrencyID = baseCurrencyID;
        //    _supportsMultipleCurrency = supportsMultipleCurrency;
        //    _multipleCurrencyID = multipleCurrencyID;
        //}
        public Company(int currencyID)
        {
            _baseCurrencyID = currencyID; //Here _basecurrencyID refers to just any currencyID, not any specific base or multiple currency.
        }

        #endregion

        #region Properties

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Address1
        {
            get { return _address1; }
            set { _address1 = value; }
        }

        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; }
        }

        public int CompanyTypeID
        {
            get { return _companyTypeID; }
            set { _companyTypeID = value; }
        }

        public string Telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }

        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public int CountryID
        {
            get { return _countryID; }
            set { _countryID = value; }
        }
        public int StateID
        {
            get { return _stateID; }
            set { _stateID = value; }
        }
        public string Zip
        {
            get { return _zip; }
            set { _zip = value; }
        }
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string Region
        {
            get { return _region; }
            set { _region = value; }
        }

        #region Primary Contact information

        public string PrimaryContactFirstName
        {
            get { return _primaryContactFirstName; }
            set { _primaryContactFirstName = value; }
        }

        public string PrimaryContactLastName
        {
            get { return _primaryContactLastName; }
            set { _primaryContactLastName = value; }
        }

        public string PrimaryContactTitle
        {
            get { return _primaryContactTitle; }
            set { _primaryContactTitle = value; }
        }

        public string PrimaryContactEMail
        {
            get { return _primaryContactEMail; }
            set { _primaryContactEMail = value; }
        }

        public string PrimaryContactTelephone
        {
            get { return _primaryContactTelephone; }
            set { _primaryContactTelephone = value; }
        }

        public string PrimaryContactCell
        {
            get { return _primaryContactCell; }
            set { _primaryContactCell = value; }
        }

        #endregion


        #region Secondary Contact information

        public string SecondaryContactFirstName
        {
            get { return _secondaryContactFirstName; }
            set { _secondaryContactFirstName = value; }
        }

        public string SecondaryContactLastName
        {
            get { return _secondaryContactLastName; }
            set { _secondaryContactLastName = value; }
        }

        public string SecondaryContactTitle
        {
            get { return _secondaryContactTitle; }
            set { _secondaryContactTitle = value; }
        }

        public string SecondaryContactEMail
        {
            get { return _secondaryContactEMail; }
            set { _secondaryContactEMail = value; }
        }

        public string SecondaryContactTelephone
        {
            get { return _secondaryContactTelephone; }
            set { _secondaryContactTelephone = value; }
        }

        public string SecondaryContactCell
        {
            get { return _secondaryContactCell; }
            set { _secondaryContactCell = value; }
        }

        #endregion

        #region Technology contact information

        public string TechnologyContactFirstName
        {
            get { return _technologyContactFirstName; }
            set { _technologyContactFirstName = value; }
        }

        public string TechnologyContactLastName
        {
            get { return _technologyContactLastName; }
            set { _technologyContactLastName = value; }
        }

        public string TechnologyContactTitle
        {
            get { return _technologyContactTitle; }
            set { _technologyContactTitle = value; }
        }

        public string TechnologyContactEMail
        {
            get { return _technologyContactEMail; }
            set { _technologyContactEMail = value; }
        }

        public string TechnologyContactTelephone
        {
            get { return _technologyContactTelephone; }
            set { _technologyContactTelephone = value; }
        }

        public string TechnologyContactCell
        {
            get { return _technologyContactCell; }
            set { _technologyContactCell = value; }
        }

        public int MPID
        {
            get { return _mpid; }
            set { _mpid = value; }
        }
        public int CompanyMPID
        {
            get { return _companyMPID; }
            set { _companyMPID = value; }
        }
        #endregion

        #region Compliance information
        public int BaseCurrencyID
        {
            get { return _baseCurrencyID; }
            set { _baseCurrencyID = value; }
        }
        public int SupportsMultipleCurrency
        {
            get { return _supportsMultipleCurrency; }
            set { _supportsMultipleCurrency = value; }
        }
        public int FixVersionID
        {
            get { return _fixVersionID; }
            set { _fixVersionID = value; }
        }
        public int FixCapabilityID
        {
            get { return _fixCapabilityID; }
            set { _fixCapabilityID = value; }
        }

        public string BorrowerName
        {
            get { return _borrowerName; }
            set { _borrowerName = value; }
        }

        public string BorrowerShortName
        {
            get { return _borrowerShortName; }
            set { _borrowerShortName = value; }
        }
        public string FirmID
        {
            get { return _firmID; }
            set { _firmID = value; }
        }

        public int CompanyComplianceID
        {
            get { return _companyComplianceID; }
            set { _companyComplianceID = value; }
        }

        public int CompanyBorrowerID
        {
            get { return _companyBorrowerID; }
            set { _companyBorrowerID = value; }
        }

        public int MultipleCurrencyID
        {
            get { return _multipleCurrencyID; }
            set { _multipleCurrencyID = value; }
        }

        public int CompanyAllCurrencyID
        {
            get { return _companyAllCurrencyID; }
            set { _companyAllCurrencyID = value; }
        }
        #endregion

        #region Company Venue Information
        public int CompanyVenueID
        {
            get { return _companyVenueID; }
            set { _companyVenueID = value; }
        }

        public string VenueName
        {
            get { return _venueName; }
            set { _venueName = value; }
        }
        public string VenueShortName
        {
            get { return _venueShortName; }
            set { _venueShortName = value; }
        }

        public int VenueType
        {
            get { return _venueType; }
            set { _venueType = value; }
        }

        public int TimeZone
        {
            get { return _timeZone; }
            set { _timeZone = value; }
        }

        public string EmailAlert
        {
            get { return _emailAlert; }
            set { _emailAlert = value; }
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
        #endregion

        public bool SendAllocationsViaFix
        {
            get { return _sendAllocationsViaFix; }
            set { _sendAllocationsViaFix = value; }
        }
        #endregion
    }
}
