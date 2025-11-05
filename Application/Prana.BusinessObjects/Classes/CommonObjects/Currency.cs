using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for Currency.
    /// </summary>
    [Serializable]
    public class Currency
    {

        #region private members

        private int _currencyID;
        private string _currencyName;

        #endregion

        public Currency()
        {
            _currencyID = Int32.MinValue;
            _currencyName = string.Empty;
            _symbol = string.Empty;
        }

        public Currency(int id, string name, string symbol)
        {
            _currencyID = id;
            _currencyName = name;
            _symbol = symbol;
        }


        #region

        public int CurrencyID
        {
            get
            {
                return this._currencyID;
            }

            set
            {
                this._currencyID = value;
            }
        }

        public string CurrencyName
        {
            get
            {
                return this._currencyName;
            }

            set
            {
                this._currencyName = value;
            }
        }

        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        public override string ToString()
        {
            return _currencyName;
        }

        #endregion
    }
}
