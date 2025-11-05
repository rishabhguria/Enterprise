using System.Collections.Generic;

namespace Prana.Import
{
    class ImportSettings
    {
        bool _symbolValidation = false;

        public bool SymbolValidation
        {
            get { return _symbolValidation; }
            set { _symbolValidation = value; }
        }

        List<string> _listSymbologyPrecedence = new List<string>();

        public List<string> ListSymbologyPrecedence
        {
            get { return _listSymbologyPrecedence; }
            set { _listSymbologyPrecedence = value; }
        }
    }
}
