namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Symbol.
    /// </summary>
    public class Symbol
    {
        #region Private members

        private int _symbolID = int.MinValue;
        private string _symbol = string.Empty;
        private string _symbolCompany = string.Empty;

        #endregion

        #region Constructors

        public Symbol()
        {
        }

        public Symbol(int symbolID, string symbol)
        {
            _symbolID = symbolID;
            _symbol = symbol;
        }

        public Symbol(int symbolID, string symbol, string symbolCompany)
        {
            _symbolID = symbolID;
            _symbol = symbol;
            _symbolCompany = symbolCompany;
        }

        #endregion

        #region Properties

        public int SymbolID
        {
            get { return _symbolID; }
            set { _symbolID = value; }
        }

        public string CompanySymbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        public string Company
        {
            get { return _symbolCompany; }
            set { _symbolCompany = value; }
        }
        #endregion
    }
}
