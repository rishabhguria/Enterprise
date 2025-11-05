using Prana.APIAdapter.Models;
using System.Threading.Tasks;

namespace Prana.APIAdapter.Interfaces
{
    public interface IPriceAPIService
    {
        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<AuthSession> Authenticate();

        /// <summary>
        /// GetPrice
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<string> GetPrice();

    }
}
