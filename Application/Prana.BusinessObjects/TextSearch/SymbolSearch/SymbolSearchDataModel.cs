using Prana.BusinessObjects.TextSearch.Common;

namespace Prana.BusinessObjects.TextSearch.SymbolSearch
{
    public class SymbolSearchDataModel : TextSearchDataModel
    {
        public string TickerSymbol { get; set; }
        public string BloombergSymbol { get; set; }
        public string FactSetSymbol { get; set; }
        public string ActivSymbol { get; set; }
    }
}
