using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ClientInfo
    {
        private int _userId;

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private int _tradingAccountID;

        public int TradingAccountID
        {
            get { return _tradingAccountID; }
            set { _tradingAccountID = value; }
        }

        private int _companyID;

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }



    }
}
