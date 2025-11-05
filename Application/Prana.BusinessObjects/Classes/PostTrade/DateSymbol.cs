using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class DateSymbol
    {
        private DateTime _Fromdate;

        public DateTime FromDate
        {
            get { return _Fromdate; }
            set { _Fromdate = value; }
        }

        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private DateTime _todate;

        public DateTime ToDate
        {
            get { return _todate; }
            set { _todate = value; }
        }
    }
}
