namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyClientTradingAccounts.
    /// </summary>
    public class CompanyClientTradingAccount
    {

        #region Private members
        int _companyClientTradingAccountID;
        string _companyClientTradingAccount;
        int _companyTradingAccountID;
        string _companyTradingAccountName;
        int _companyClientID;
        int _clientTraderID;
        string _clientTraderShortName;
        #endregion

        #region constructor	
        public CompanyClientTradingAccount()
        {

        }

        public CompanyClientTradingAccount(int companyClientTradingAccountID, string CompanyClientTradingAccount, int CompanyTradingAccountID, string CompanyTradingAccountName, int CompanyClientID, int ClientTraderID, string ClientTraderShortName)
        {
            _companyClientTradingAccountID = companyClientTradingAccountID;
            _companyClientTradingAccount = CompanyClientTradingAccount;
            _companyTradingAccountID = CompanyTradingAccountID;
            _companyTradingAccountName = CompanyTradingAccountName;
            _companyClientID = CompanyClientID;
            _clientTraderID = ClientTraderID;
            _clientTraderShortName = ClientTraderShortName;
        }
        public CompanyClientTradingAccount(string CompanyClientTradingAccount, int CompanyTradingAccountID, string CompanyTradingAccountName, int CompanyClientID, int ClientTraderID, string clientTraderShortName)
        {
            _companyClientTradingAccountID = int.MinValue;
            _companyClientTradingAccount = CompanyClientTradingAccount;
            _companyTradingAccountID = CompanyTradingAccountID;
            _companyTradingAccountName = CompanyTradingAccountName;
            _companyClientID = CompanyClientID;
            _clientTraderID = ClientTraderID;
            _clientTraderShortName = clientTraderShortName;

        }
        #endregion

        public bool Equal(CompanyClientTradingAccount companyClientTradingAccount)
        {
            if (companyClientTradingAccount.CompanyTradingAccountID == _companyTradingAccountID
                    && companyClientTradingAccount.CompanyClientID == _companyClientID
                    && companyClientTradingAccount.ClientTraderID == _clientTraderID
                    && companyClientTradingAccount.ClientTraderShortName == _clientTraderShortName
                    && companyClientTradingAccount.CompClientTradingAccount == _companyClientTradingAccount
                    )
                return true;
            else
                return false;
        }

        #region Properties




        public string CompClientTradingAccount
        {
            get
            {
                return _companyClientTradingAccount;
            }

            set
            {
                _companyClientTradingAccount = value;
            }
        }


        public int CompanyTradingAccountID
        {
            get
            {
                return _companyTradingAccountID;
            }

            set
            {
                _companyTradingAccountID = value;
            }
        }


        public int CompanyClientID
        {
            get
            {
                return _companyClientID;
            }

            set
            {
                _companyClientID = value;
            }
        }


        public int ClientTraderID
        {
            get
            {
                return _clientTraderID;
            }

            set
            {
                _clientTraderID = value;
            }
        }


        public string ClientTraderShortName
        {
            get
            {
                return _clientTraderShortName;
            }

            set
            {
                _clientTraderShortName = value;
            }
        }


        public int CompanyClientTradingAccountID
        {
            get { return _companyClientTradingAccountID; }
            set { _companyClientTradingAccountID = value; }
        }

        public string CompanyTradingAccountName
        {
            get { return _companyTradingAccountName; }
            set { _companyTradingAccountName = value; }
        }


        #endregion

    }
}
