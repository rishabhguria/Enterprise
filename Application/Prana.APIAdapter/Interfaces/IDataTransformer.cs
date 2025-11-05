using Prana.BusinessObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prana.APIAdapter.Interfaces
{
    public interface IDataTransformer
    {
        /// <summary>
        ///  Transform Json String To Symbol Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<List<SymbolData>> JsonStringToSymbolData(string data);

        /// <summary>
        /// Transform XML String To Symbol Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<List<SymbolData>> XMLStringToSymbolData(string data);

    }
}
