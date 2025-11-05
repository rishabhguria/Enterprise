namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Currency.
    /// </summary>
    public class Currency
    {
        #region Private members

        private int _currencyID = int.MinValue;
        private string _currencyName = string.Empty;
        private string _currencySymbol = string.Empty;
        private int _currencyTypeID = int.MinValue;

        private int _counterPartyVenueID = int.MinValue;

        #endregion


        #region Constructors

        public Currency()
        {
        }

        public Currency(int currencyID, int currencyTypeID)
        {
            _currencyID = currencyID;
            _currencyTypeID = currencyTypeID;
        }

        public Currency(int currencyID, string currencyName, string currencySymbol)
        {
            _currencyID = currencyID;
            _currencyName = currencyName;
            _currencySymbol = currencySymbol;
        }

        #endregion

        #region Properties

        public int CurencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }

        public string CurrencyName
        {
            get { return _currencyName; }
            set { _currencyName = value; }
        }
        public string CurrencySymbol
        {
            get { return _currencySymbol; }
            set { _currencySymbol = value; }
        }

        public int CurrencyTypeID
        {
            get { return _currencyTypeID; }
            set { _currencyTypeID = value; }
        }

        public int CounterPartyVenueID
        {
            get { return _counterPartyVenueID; }
            set { _counterPartyVenueID = value; }
        }

        #endregion

    }
}
