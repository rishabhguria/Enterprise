using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class TradingAccount
    {
        int _tradingAccountID = int.MinValue;
        string _name = string.Empty;

        public TradingAccount()
        {
        }

        public TradingAccount(int tradingAccountID, string name)
        {
            _tradingAccountID = tradingAccountID;
            _name = name;
        }

        public int TradingAccountID
        {
            get
            {
                return _tradingAccountID;
            }

            set
            {
                _tradingAccountID = value;
            }
        }


        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

    }


}
