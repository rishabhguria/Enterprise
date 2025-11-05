namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyClient.
    /// </summary>
    public class CompanyClient
    {
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
        private int _companyClientID = int.MinValue;

        private int _countryID = int.MinValue;
        private int _stateID = int.MinValue;
        private string _zip = string.Empty;
        private string _shortName = string.Empty;

        #endregion

        #region Constructors
        public CompanyClient()
        {
        }
        public CompanyClient(int companyClientID, string name)
        {
            _companyClientID = companyClientID;
            _name = name;
        }
        #endregion

        #region Properties

        #region Details Properties
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

        public int CompanyClientID
        {
            get { return _companyClientID; }
            set { _companyClientID = value; }
        }

        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
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
        #endregion

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

        #endregion

    }
}
