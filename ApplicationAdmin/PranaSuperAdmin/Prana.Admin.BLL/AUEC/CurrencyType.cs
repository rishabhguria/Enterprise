namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CurrencyType.
    /// </summary>
    public class CurrencyType
    {
        #region Private members

        private int _currencyTypeID = int.MinValue;
        private string _type = string.Empty;

        #endregion

        #region Constructors
        public CurrencyType()
        {
        }

        public CurrencyType(int currencyTypeID, string type)
        {
            _currencyTypeID = currencyTypeID;
            _type = type;
        }
        #endregion

        #region Properties

        public int CurrencyTypeID
        {
            get { return _currencyTypeID; }
            set { _currencyTypeID = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        #endregion
    }
}
