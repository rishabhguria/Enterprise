namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for SymbolIdentifier.
    /// </summary>
    public class SymbolIdentifier
    {
        int _symbolIdentifierID = int.MinValue;
        string _symbolIdentifierName = string.Empty;
        string _shortName = string.Empty;

        public SymbolIdentifier()
        {
        }

        public SymbolIdentifier(int symbolIdentifierID, string symbolIdentifierName, string shortName)
        {
            _symbolIdentifierID = symbolIdentifierID;
            _symbolIdentifierName = symbolIdentifierName;
            _shortName = shortName;
        }

        public int SymbolIdentifierID
        {
            get
            {
                return _symbolIdentifierID;
            }

            set
            {
                _symbolIdentifierID = value;
            }
        }


        public string SymbolIdentifierName
        {
            get
            {
                return _symbolIdentifierName;
            }

            set
            {
                _symbolIdentifierName = value;
            }
        }

        public string ShortName
        {
            get
            {
                return _shortName;
            }

            set
            {
                _shortName = value;
            }
        }
    }
}
