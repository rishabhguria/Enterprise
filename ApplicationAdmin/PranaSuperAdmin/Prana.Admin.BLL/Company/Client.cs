namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Client.
    /// </summary>
    public class Client
    {
        #region Private members

        private int _clientID = int.MinValue;
        private string _clientName = string.Empty;
        private string _mailingAddress1 = string.Empty;
        private string _mailingAddress2 = string.Empty;
        private int _companyType = int.MinValue;
        private string _telephone = string.Empty;
        private string _fax = string.Empty;
        private string _primaryContactFirstName = string.Empty;
        private string _primaryContactLastName = string.Empty;
        private string _primaryContactTitle = string.Empty;
        private string _primaryContactEmail = string.Empty;
        private string _primaryContactTelephone = string.Empty;
        private string _primaryContactCell = string.Empty;
        private string _secondaryContactFirstName = string.Empty;
        private string _secondaryContactLastName = string.Empty;
        private string _secondaryContactTitle = string.Empty;
        private string _secondaryContactEmail = string.Empty;
        private string _secondaryContactTelephone = string.Empty;
        private string _secondaryContactCell = string.Empty;
        private int _companyID = int.MinValue;

        #endregion

        #region Constructors

        public Client()
        {
        }

        public Client(int clientID, string clientName)
        {
            _clientID = clientID;
            _clientName = clientName;
        }

        #endregion

        #region Properties

        public int ClientID
        {
            get { return _clientID; }
            set { _clientID = value; }
        }

        public string ClientName
        {
            get { return _clientName; }
            set { _clientName = value; }
        }

        public int CompanyType
        {
            get { return _companyType; }
            set { _companyType = value; }
        }

        public string MailingAddress1
        {
            get { return _mailingAddress1; }
            set { _mailingAddress1 = value; }
        }

        public string MailingAddress2
        {
            get { return _mailingAddress2; }
            set { _mailingAddress2 = value; }
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

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        #region Primary Contact
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

        public string PrimaryContactEmail
        {
            get { return _primaryContactEmail; }
            set { _primaryContactEmail = value; }
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

        #region Secondry Contact
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

        public string SecondaryContactEmail
        {
            get { return _secondaryContactEmail; }
            set { _secondaryContactEmail = value; }
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
