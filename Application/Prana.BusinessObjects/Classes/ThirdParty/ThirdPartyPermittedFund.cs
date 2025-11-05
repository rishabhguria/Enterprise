namespace Prana.BusinessObjects
{
    public class ThirdPartyPermittedAccount
    {
        #region Private Members

        private int _thirdPartyAccountID_PK = int.MinValue;
        private int _thirdPartyID = int.MinValue;
        private int _companyAccountID = int.MinValue;
        private string _accountName = string.Empty;
        private int _accountTypeID = int.MinValue;

        #endregion Private Members

        #region Constructors
        public ThirdPartyPermittedAccount()
        {

        }

        public ThirdPartyPermittedAccount(int thirdPartyID, int companyAccountID)
        {
            _thirdPartyID = thirdPartyID;
            _companyAccountID = companyAccountID;
        }

        public ThirdPartyPermittedAccount(int thirdPartyAccountID_PK, int thirdPartyID, int companyAccountID,
                                string accountName, int accountTypeID)
        {
            _thirdPartyAccountID_PK = thirdPartyAccountID_PK;
            _thirdPartyID = thirdPartyID;
            _companyAccountID = companyAccountID;
            _accountName = accountName;
            _accountTypeID = accountTypeID;
        }
        #endregion Constructors

        #region Properties

        public int ThirdPartyAccountID_PK
        {
            get { return _thirdPartyAccountID_PK; }
            set { _thirdPartyAccountID_PK = value; }
        }

        public int ThirdPartyID
        {
            get { return _thirdPartyID; }
            set { _thirdPartyID = value; }
        }

        public int CompanyAccountID
        {
            get { return _companyAccountID; }
            set { _companyAccountID = value; }
        }

        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }

        public int AccountTypeID
        {
            get { return _accountTypeID; }
            set { _accountTypeID = value; }
        }

        #endregion Properties
    }
}
