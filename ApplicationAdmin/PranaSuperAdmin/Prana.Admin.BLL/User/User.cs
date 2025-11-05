namespace Prana.Admin.BLL
{
    /// <summary>
    /// User is a ORM mapping of T_User table in Database.
    /// </summary>
    public class User
    {
        #region Private members

        private int _userID = int.MinValue;
        private string _lastName = string.Empty;
        private string _firstName = string.Empty;
        private string _shortName = string.Empty;
        private string _factSetUsernameAndSerialNumber = string.Empty;
        private bool _isFactSetSupportUser = false;
        private string _activUsername = string.Empty;
        private string _activPassword = string.Empty;
        private string _marketDataAccessIPAddresses = string.Empty;
        private string _title = string.Empty;
        private string _eMail = string.Empty;
        private string _telephoneWork = string.Empty;
        private string _telephoneHome = string.Empty;
        private string _telephoneMobile = string.Empty;
        private string _telephonePager = string.Empty;
        private string _fax = string.Empty;
        private string _loginname = string.Empty;
        private string _password = string.Empty;
        private string _address1 = string.Empty;
        private string _address2 = string.Empty;
        private int _countryID = int.MinValue;
        private int _stateID = int.MinValue;
        private string _zip = string.Empty;
        private int _companyID = int.MinValue;
        private int _isActive = int.MinValue;

        private int _tradingPermission = int.MinValue;
        private int _superUser = int.MinValue;

        private string _city = string.Empty;
        private string _reion = string.Empty;
        private int _roleID = int.MinValue;
        private bool _isAllGroupsAccess = false;
        private string _samsaraAzureId = string.Empty;
        private string _sapiUsername = string.Empty;
        #endregion

        #region Constructors

        public User()
        {
        }

        public User(int userID, string firstName)
        {
            _userID = userID;
            _firstName = firstName;
        }
        public User(int userID, string lastName, string firstName,
            string shortName, string title, string eMail,
            string telephoneWork, string telephoneHome,
            string telephoneMobile, string fax)
        {
            _userID = userID;
            _lastName = lastName;
            _firstName = firstName;
            _shortName = shortName;
            _title = title;
            _eMail = eMail;
            _telephoneWork = telephoneWork;
            _telephoneHome = telephoneHome;
            _telephoneMobile = telephoneMobile;
            _fax = fax;
        }

        #endregion

        #region Properties

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }

        public string FactSetUsernameAndSerialNumber
        {
            get { return _factSetUsernameAndSerialNumber; }
            set { _factSetUsernameAndSerialNumber = value; }
        }

        public bool IsFactSetSupportUser
        {
            get { return _isFactSetSupportUser; }
            set { _isFactSetSupportUser = value; }
        }

        public string ActivUsername
        {
            get { return _activUsername; }
            set { _activUsername = value; }
        }

        public string ActivPassword
        {
            get { return _activPassword; }
            set { _activPassword = value; }
        }

        public string MarketDataAccessIPAddresses
        {
            get { return _marketDataAccessIPAddresses; }
            set { _marketDataAccessIPAddresses = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string EMail
        {
            get { return _eMail; }
            set { _eMail = value; }
        }

        public string TelephoneWork
        {
            get { return _telephoneWork; }
            set { _telephoneWork = value; }
        }

        public string TelephoneHome
        {
            get { return _telephoneHome; }
            set { _telephoneHome = value; }
        }

        public string TelephoneMobile
        {
            get { return _telephoneMobile; }
            set { _telephoneMobile = value; }
        }

        public string TelephonePager
        {
            get { return _telephonePager; }
            set { _telephonePager = value; }
        }

        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        public string LoginName
        {
            get { return _loginname; }
            set { _loginname = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
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

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        public int IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public int TradingPermission
        {
            get { return _tradingPermission; }
            set { _tradingPermission = value; }
        }

        public int SuperUser
        {
            get { return _superUser; }
            set { _superUser = value; }
        }

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string Region
        {
            get { return _reion; }
            set { _reion = value; }
        }

        public int RoleID
        {
            get { return _roleID; }
            set { _roleID = value; }
        }

        public bool IsAllGroupsAccess
        {
            get { return _isAllGroupsAccess; }
            set { _isAllGroupsAccess = value; }
        }

        public string SamsaraAzureId
        {
            get { return _samsaraAzureId; }
            set { _samsaraAzureId = value; }
        }

        public string SapiUsername
        {
            get { return _sapiUsername; }
            set { _sapiUsername = value; }
        }

        #endregion

        #region Permissions
        private Permissions _permissions = null;
        public Permissions Permissions
        {
            get
            {
                if (_permissions != null)
                {
                    _permissions = PermissionManager.GetPermissions(_userID);
                }
                return _permissions;
            }
        }
        #endregion
    }
}
