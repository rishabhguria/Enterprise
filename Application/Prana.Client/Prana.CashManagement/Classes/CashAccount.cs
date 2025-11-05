using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.CashManagement
{
    class CashAccount
    {
        private int _id;
        private string _name;
        private string _acronym;
        private Dictionary<string, CashSubAccount> _subAccounts;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Acronym
        {
            get { return _acronym; }
            set { _acronym = value; }
        }
        public Dictionary<string, CashSubAccount> SubAccounts
        {
            get { return _subAccounts; }
            set { _subAccounts = value; }
        }

    }
}
