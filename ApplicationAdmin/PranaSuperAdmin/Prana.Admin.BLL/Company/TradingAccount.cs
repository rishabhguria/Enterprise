namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for TradingAccount.
    /// </summary>
    public class TradingAccount
    {
        #region Private members

        private int _tradingAccountsID = int.MinValue;
        private string _tradingAccountName = string.Empty;
        private string _tradingShortName = string.Empty;
        private int _companyID = int.MinValue;

        #endregion

        #region Constructors
        public TradingAccount()
        {
        }
        public TradingAccount(int tradingAccountsID, string tradingAccountName)
        {
            _tradingAccountsID = tradingAccountsID;
            _tradingAccountName = tradingAccountName;
        }
        #endregion

        #region Properties

        public int TradingAccountsID
        {
            get { return _tradingAccountsID; }
            set { _tradingAccountsID = value; }
        }

        public string TradingAccountName
        {
            get { return _tradingAccountName; }
            set { _tradingAccountName = value; }
        }

        public string TradingShortName
        {
            get { return _tradingShortName; }
            set { _tradingShortName = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        #endregion
    }
}
