using Prana.ServiceGateway.Models;

namespace Prana.ServiceGateway.Contracts
{
    public interface IReportsPortalService
    {
        Task<string> GetApi(string url);
        Task<object> PostApi(string url, object BodyJSON);
        Task<string> StoreLayout(SaveDefaultLayoutDto layout); 
        string FetchDefaultLayout(SaveDefaultLayoutDto layout);
    }
}
