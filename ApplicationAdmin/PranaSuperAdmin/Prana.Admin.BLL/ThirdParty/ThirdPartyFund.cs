namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ThirdPartyAccount.
    /// </summary>
    public class ThirdPartyAccount
    {
        #region Private Members
        private int _companyThirdPartyID_PK = int.MinValue;
        private int _companyThirdPartyID_FK = int.MinValue;
        private int _companyID = int.MinValue;
        private int _internalAccountID = int.MinValue;
        private string _mappedAccountName = string.Empty;
        private string _account = string.Empty;
        private int _accountTypeID = int.MinValue;

        private string _accountName = string.Empty;
        #endregion

        #region Constructors
        public ThirdPartyAccount()
        {
        }
        public ThirdPartyAccount(int companyThirdPartyID, string accountName)
        {
            _companyThirdPartyID_PK = companyThirdPartyID;
            _accountName = accountName;
        }

        public ThirdPartyAccount(int companyThirdPartyID_PK, int companyThirdPartyID, int companyID,
            int internalAccountID, string accountName, string account, int accountTypeID)
        {
            _companyThirdPartyID_PK = companyThirdPartyID_PK;
            _companyThirdPartyID_FK = companyThirdPartyID;
            _companyID = companyID;
            _internalAccountID = internalAccountID;
            _mappedAccountName = accountName;
            _account = account;
            _accountTypeID = accountTypeID;
        }

        #endregion

        #region Properties

        public int CompanyID_FK
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        public int CompanyThirdPartyID_PK
        {
            get { return _companyThirdPartyID_PK; }
            set { _companyThirdPartyID_PK = value; }
        }

        public int CompanyThirdPartyID_FK
        {
            get { return _companyThirdPartyID_FK; }
            set { _companyThirdPartyID_FK = value; }
        }
        //public string CompanyIdentifier
        //{
        //    get{return _companyIdentifier;}
        //    set{_companyIdentifier = value;}
        //}
        public int InternalAccountID
        {
            get { return _internalAccountID; }
            set { _internalAccountID = value; }
        }
        //public int AccountSubAccountID
        //{
        //    get{return _accountSubAccountID;}
        //    set{_accountSubAccountID = value;}
        //}
        public string MappedAccountName
        {
            get { return _mappedAccountName; }
            set { _mappedAccountName = value; }
        }
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }
        public int AccountTypeID
        {
            get { return _accountTypeID; }
            set { _accountTypeID = value; }
        }
        //public string Field1
        //{
        //    get{return _field1;}
        //    set{_field1 = value;}
        //}
        //public string Field2
        //{
        //    get{return _field2;}
        //    set{_field2 = value;}
        //}
        //public string Field3
        //{
        //    get{return _field3;}
        //    set{_field3 = value;}
        //}
        //public string NamingConvention
        //{
        //    get{return _namingConvention;}
        //    set{_namingConvention = value;}
        //}
        //public string SaveGeneratedFile
        //{
        //    get{return _saveGeneratedFile;}
        //    set{_saveGeneratedFile = value;}
        //}
        //public string ButtonField
        //{
        //    get{return _buttonField;}
        //    set{_buttonField = value;}
        //}
        //public string ThirdPartyName
        //{
        //    get
        //    {
        //        if(_companyThirdPartyID > int.MinValue)
        //        {
        //            ThirdParty thirdParty =  ThirdPartyManager.GetCompanyThirdParty(_companyThirdPartyID);	
        //            _thirdPartyName = thirdParty.ThirdPartyName.ToString();
        //        }

        //        return _thirdPartyName;
        //    }
        //    set{_thirdPartyName = value;}
        //}
        public string AccountName
        {
            get
            {
                if (_internalAccountID > int.MinValue)
                {
                    Account account = CompanyManager.GetsAccount(_internalAccountID);
                    _accountName = account.AccountName.ToString();
                }

                return _accountName;
            }
            set { _accountName = value; }
        }
        #endregion
    }
}
