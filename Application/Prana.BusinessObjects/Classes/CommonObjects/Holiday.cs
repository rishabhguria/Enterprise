using System;



namespace Prana.BusinessObjects.CommonObjects
{
    /// <summary>
    /// Summary description for Holiday.
    /// </summary>
    public class Holiday
    {
        #region Private members
        private bool _select = false;
        private int _holidayID = int.MinValue;
        private int _auecExchangeID = int.MinValue;
        private string _description = string.Empty;
        private bool _marketOff = false;                            // MarketOfff and SettlementOff fieds are added by bhavana on March 28, 2014
        private bool _settlementOff = false;
        private DateTime _date = DateTimeConstants.MinValue;
        //private DateTime? _date ;
        private string _dateString = string.Empty;

        private int _auecID = int.MinValue;

        //Now this is declared so as to save holidays as corresponding to the exchange. Also to remove _auecID abpve.
        private int _exchangeID = int.MinValue;

        #endregion

        #region Constructors

        public Holiday()
        {
        }

        //Constructor modified by bhavana on March 28, 2014 for additional columns MarketOff and SettlementOff
        public Holiday(DateTime date, string decsription, bool marketOff, bool settlementOff)
        {
            _date = date;
            _description = decsription;
            _marketOff = marketOff;
            _settlementOff = settlementOff;
        }

        public Holiday(int holidayID, int auecExchangeID, string description, DateTime date)
        {
            _holidayID = holidayID;
            _auecExchangeID = auecExchangeID;
            _description = description;
            _date = date;
        }

        public Holiday(int holidayID, string description, DateTime date, int exchangeID)
        {
            _holidayID = holidayID;
            _exchangeID = exchangeID;
            _description = description;
            _date = date;
        }

        public Holiday(int holidayID, int auecExchangeID, string description, string dateString)
        {
            _holidayID = holidayID;
            _auecExchangeID = auecExchangeID;
            _description = description;
            _dateString = dateString;
        }

        #endregion

        #region Properties
        public int HolidayID
        {
            get { return _holidayID; }
            set { _holidayID = value; }
        }

        public int AUECExchangeID
        {
            get { return _auecExchangeID; }
            set { _auecExchangeID = value; }
        }

        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public bool MarketOff                          //properties Added by bhavana on March 28, 2014
        {
            get { return _marketOff; }
            set { _marketOff = value; }
        }

        public bool SettlementOff
        {
            get { return _settlementOff; }
            set { _settlementOff = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public string DateString
        {
            get { return _dateString; }
            set { _dateString = value; }
        }

        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        public bool Select
        {
            get { return _select; }
            set { _select = value; }
        }
        #endregion
    }
}
