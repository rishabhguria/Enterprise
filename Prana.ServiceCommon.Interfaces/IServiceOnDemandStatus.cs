using System.ServiceModel;
using System.Threading.Tasks;

namespace Prana.ServiceCommon.Interfaces
{
    [ServiceContract]
    public interface IServiceOnDemandStatus
    {
        [OperationContract]
        Task<bool> HealthCheck();
    }
}
