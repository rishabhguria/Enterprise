namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Accounts.
    /// </summary>
    public class Account
    {
        #region Private members

        private int _accountID = int.MinValue;
        private string _accountName = string.Empty;
        private string _accountShortName = string.Empty;
        private int _companyID = int.MinValue;

        private int _companyPrimeBrokerClearerID = int.MinValue;
        private int _companyCustodianID = int.MinValue;
        private int _companyAdministratorID = int.MinValue;

        private int _companyAccountID = int.MinValue;
        private int _companyUserAccountID = int.MinValue;

        private int _accountTypeID = int.MinValue;
        #endregion

        #region Constructors
        public Account()
        {
        }

        public Account(int accountID, string accountName)
        {
            _accountID = accountID;
            _accountName = accountName;
        }

        #endregion

        #region Properties

        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }

        //		public string AccountName
        //		{
        //			get
        //			{
        //				if(_accountID > int.MinValue)
        //				{
        //					Account account = CompanyManager.GetsAccount(_accountID);
        //					_accountName = account.AccountName.ToString();
        //					return _accountName;
        //				}
        //				else
        //				{
        //					return _accountName;
        //				}
        //			}
        //			set{_accountName = value;}
        //		}


        public string AccountShortName
        {
            get { return _accountShortName; }
            set { _accountShortName = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        public int CompanyPrimeBrokerClearerID
        {
            get { return _companyPrimeBrokerClearerID; }
            set { _companyPrimeBrokerClearerID = value; }
        }

        public int CompanyCustodianID
        {
            get { return _companyCustodianID; }
            set { _companyCustodianID = value; }
        }

        public int CompanyAdministratorID
        {
            get { return _companyAdministratorID; }
            set { _companyAdministratorID = value; }
        }

        public int CompanyAccountID
        {
            get { return _companyAccountID; }
            set { _companyAccountID = value; }
        }

        public int CompanyUserAccountID
        {
            get { return _companyUserAccountID; }
            set { _companyUserAccountID = value; }
        }

        public int AccountTypeID
        {
            get { return _accountTypeID; }
            set { _accountTypeID = value; }
        }

        //		public string CompanyAccountName
        //		{
        //			get
        //			{
        //				if(_accountID > int.MinValue)
        //				{
        //					Account account = CompanyManager.GetsAccount(_accountID);
        //					_accountName = account.AccountName.ToString();
        //					return _companyAccountName;
        //				}
        //				else
        //				{
        //					return _companyAccountName;
        //				}
        //			}
        //		}
        #endregion
    }
}
