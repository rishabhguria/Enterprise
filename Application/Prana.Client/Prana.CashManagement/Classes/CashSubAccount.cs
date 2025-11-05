using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.CashManagement
{
    class CashSubAccount
    {
        private int _id;
        private int _typeID;
        private string _name;
        private string _acronym;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public int TypeID
        {
            get { return _typeID; }
            set { _typeID = value; }
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

    }
}
