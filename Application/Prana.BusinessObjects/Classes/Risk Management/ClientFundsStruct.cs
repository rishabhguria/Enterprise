namespace Prana.BusinessObjects
{
    public struct ClientAccountsStruct
    {
        private int _accountID;

        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        private int _clientID;

        public int ClientID
        {
            get { return _clientID; }
            set { _clientID = value; }
        }

        private string _clientName;

        public string ClientName
        {
            get { return _clientName; }
            set { _clientName = value; }
        }


        private string _accountName;

        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }


    }

}
