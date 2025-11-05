namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Symbol.
    /// </summary>
    public class SymbolConversion
    {
        #region Private and protected members.

        private int _symbolID = int.MinValue;
        private string _symbolName = string.Empty;


        #endregion

        public SymbolConversion()
        {
        }

        public SymbolConversion(int symbolID, string SymbolName)
        {
            _symbolID = symbolID;
            _symbolName = SymbolName;
        }

        #region Properties

        public int SymbolID
        {
            get { return _symbolID; }
            set { _symbolID = value; }
        }

        public string SymbolName
        {
            get { return _symbolName; }
            set { _symbolName = value; }
        }



        #endregion
    }
}
