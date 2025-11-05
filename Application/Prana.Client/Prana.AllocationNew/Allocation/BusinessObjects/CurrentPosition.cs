using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.AllocationNew
{
    class CurrentPosition
    {
        private int _accountID;

        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
        private double  _qty;

        public double  Qty
        {
            get { return _qty; }
            set { _qty = value; }
        }
        private string  _symbol;

        public string  Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
	
	
    }
}
