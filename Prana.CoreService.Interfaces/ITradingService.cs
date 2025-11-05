using Prana.ServiceCommon.Interfaces;
using System.ServiceModel;

namespace Prana.CoreService.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IServiceStatusCallback))]
    public interface ITradingService : IServiceStatus, IServiceOnDemandStatus, IContainerService, IPranaServiceCommon
    {
    }
}
