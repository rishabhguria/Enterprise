using Prana.BusinessObjects.TextSearch.SymbolSearch;
using Prana.Interfaces.TextSearch.Common;
using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.Interfaces.TextSearch.SymbolSearch
{
    public interface ISymbolSearch : ITextSearch
    {
        [OperationContract]
        void addSymbolList(List<SymbolSearchDataModel> dataList);

        [OperationContract]
        void addSymbol(SymbolSearchDataModel dataObject);

        [OperationContract]
        IEnumerable<SymbolSearchDataModel> searchSymbol(string searchText, int limit, string fieldName);
    }
}
