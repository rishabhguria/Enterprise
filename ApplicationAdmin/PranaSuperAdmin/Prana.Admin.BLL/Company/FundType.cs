namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AccountType.
    /// </summary>
    public class AccountType
    {
        #region private members
        private int _accountTypeID = int.MinValue;
        private string _accountTypeName = string.Empty;
        #endregion

        public AccountType()
        {
        }
        public AccountType(int accountTypeID, string accountTypeName)
        {
            _accountTypeID = accountTypeID;
            _accountTypeName = accountTypeName;
        }

        #region properties
        public int AccountTypeID
        {
            get { return _accountTypeID; }
            set { _accountTypeID = value; }
        }
        public string AccountTypeName
        {
            get { return _accountTypeName; }
            set { _accountTypeName = value; }
        }
        #endregion
    }
}
