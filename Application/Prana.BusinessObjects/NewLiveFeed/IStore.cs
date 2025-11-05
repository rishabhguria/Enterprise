using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public interface IStore
    {
        List<SymbolData> GetData();
        void AddOrUpdateData(SymbolData data);
        void DeleteData(string symbol);
        void ClearData();


    }
}
