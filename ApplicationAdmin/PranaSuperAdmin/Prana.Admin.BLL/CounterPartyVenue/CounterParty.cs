namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CounterParty.
    /// </summary>
    public class CounterParty
    {

        #region Private members
        //TODO: Remove the unused members from here.

        private int _counterPartyID = int.MinValue;
        private string _counterPartyFullName = string.Empty;
        private string _shortName = string.Empty;
        private string _address = string.Empty;
        private string _phone = string.Empty;
        private string _fax = string.Empty;
        private string _contactName1 = string.Empty;
        private string _title1 = string.Empty;
        private string _email1 = string.Empty;
        private string _contactName2 = string.Empty;
        private string _title2 = string.Empty;
        private string _email2 = string.Empty;
        private string _acronym = string.Empty;
        private int _baseCurrency = int.MinValue;
        private int _companyID = int.MinValue;

        private int _electronicID = int.MinValue;
        private int _fixVersionID = int.MinValue;
        private int _counterPartyTypeID = int.MinValue;
        private string _description = string.Empty;
        private int _fixCapabilitiesID = int.MinValue;
        private string _companyOutgoingID = string.Empty;
        private string _companyIncomingID = string.Empty;

        private Venues _venues = null;

        private string _address2 = string.Empty;
        private int _countryID = int.MinValue;
        private int _stateID = int.MinValue;
        private string _zip = string.Empty;
        private string _contactName1LastName = string.Empty; //A primary contact last name. Used the convention so as to comply with the earlier convention and to do lot of changes in already defined properties, code and Database.
        private string _contactName1WorkPhone = string.Empty; //A primary contact work telephone. Used the convention so as to comply with the earlier convention and to do lot of changes in already defined properties, code and Database.
        private string _contactName1CellPhone = string.Empty; //A primary contact cell phone. Used the convention so as to comply with the earlier convention and to do lot of changes in already defined properties, code and Database.
        private string _contactName2LastName = string.Empty; //A secondary contact last name. Used the convention so as to comply with the earlier convention and to do lot of changes in already defined properties, code and Database.
        private string _contactName2WorkPhone = string.Empty; //A secondary contact work telephone. Used the convention so as to comply with the earlier convention and to do lot of changes in already defined properties, code and Database.
        private string _contactName2CellPhone = string.Empty; //A secondary contact cell phone. Used the convention so as to comply with the earlier convention and to do lot of changes in already defined properties, code and Database.

        private string _city = string.Empty;
        private bool _isAlgoBroker;
        private bool _isOTDorEMS;
        #endregion

        #region Constructors

        public CounterParty()
        {

        }

        public CounterParty(int counterPartyID, string fullNameCounterParty)
        {
            _counterPartyID = counterPartyID;
            _counterPartyFullName = fullNameCounterParty;
        }
        public CounterParty(int counterPartyID, string fullNameCounterParty, string shortNameCounterParty)
        {
            _counterPartyID = counterPartyID;
            _counterPartyFullName = fullNameCounterParty;
            _shortName = shortNameCounterParty;
        }

        public CounterParty(int counterPartyID, string fullNameCounterParty, string shortName,
                            string address, string phone, string fax, string contactName1,
                            string title1, string email1, string contactName2, string title2,
                            string email2, string acronym, int baseCurrency)
        {
            _counterPartyID = counterPartyID;
            _counterPartyFullName = fullNameCounterParty;
            _shortName = shortName;
            _address = address;
            _phone = phone;
            _fax = fax;
            _contactName1 = contactName1;
            _title1 = title1;
            _email1 = email1;
            _contactName2 = contactName2;
            _title2 = title2;
            _email2 = email2;
            _acronym = acronym;
            _baseCurrency = baseCurrency;
        }

        #endregion

        #region Properties

        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }

        public string CounterPartyFullName
        {
            get { return _counterPartyFullName; }
            set { _counterPartyFullName = value; }
        }

        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        public string ContactName1
        {
            get { return _contactName1; }
            set { _contactName1 = value; }
        }

        public string Title1
        {
            get { return _title1; }
            set { _title1 = value; }
        }

        public string Email1
        {
            get { return _email1; }
            set { _email1 = value; }
        }

        public string contactName2
        {
            get { return _contactName2; }
            set { _contactName2 = value; }
        }

        public string Title2
        {
            get { return _title2; }
            set { _title2 = value; }
        }

        public string Email2
        {
            get { return _email2; }
            set { _email2 = value; }
        }

        public string Acronym
        {
            get { return _acronym; }
            set { _acronym = value; }
        }

        public int BaseCurrency
        {
            get { return _baseCurrency; }
            set { _baseCurrency = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }



        public int ElectronicID
        {
            get { return _electronicID; }
            set { _electronicID = value; }
        }
        public int FixVersionID
        {
            get { return _fixVersionID; }
            set { _fixVersionID = value; }
        }
        public int CounterPartyTypeID
        {
            get { return _counterPartyTypeID; }
            set { _counterPartyTypeID = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public int FixCapabilitiesID
        {
            get { return _fixCapabilitiesID; }
            set { _fixCapabilitiesID = value; }

        }
        public string CompanyOutgoingID
        {
            get { return _companyOutgoingID; }
            set { _companyOutgoingID = value; }
        }
        public string CompanyIncomingID
        {
            get { return _companyIncomingID; }
            set { _companyIncomingID = value; }
        }

        public Venues Venues
        {
            get
            {
                if (_venues == null)
                {
                    _venues = VenueManager.GetCounterPartyVenues(_counterPartyID);
                }
                return _venues;
            }
        }

        public Venues CompanyUserVenues
        {
            get
            {
                if (_venues == null)
                {
                    _venues = VenueManager.GetCounterPartyVenues(_counterPartyID);
                }
                return _venues;
            }
        }

        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; }
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

        public string ContactName1LastName
        {
            get { return _contactName1LastName; }
            set { _contactName1LastName = value; }
        }

        public string ContactName1WorkPhone
        {
            get { return _contactName1WorkPhone; }
            set { _contactName1WorkPhone = value; }
        }

        public string ContactName1CellPhone
        {
            get { return _contactName1CellPhone; }
            set { _contactName1CellPhone = value; }
        }

        public string ContactName2LastName
        {
            get { return _contactName2LastName; }
            set { _contactName2LastName = value; }
        }

        public string ContactName2WorkPhone
        {
            get { return _contactName2WorkPhone; }
            set { _contactName2WorkPhone = value; }
        }

        public string ContactName2CellPhone
        {
            get { return _contactName2CellPhone; }
            set { _contactName2CellPhone = value; }
        }

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public bool IsAlgoBroker
        {
            get { return _isAlgoBroker; }
            set { _isAlgoBroker = value; }
        }

        public bool IsOTDorEMS
        {
            get { return _isOTDorEMS; }
            set { _isOTDorEMS = value; }
        }

        #endregion
    }
}
