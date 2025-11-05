using Prana.ServiceCommon.Interfaces;
using System.ServiceModel;

namespace Prana.CoreService.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IServiceStatusCallback))]
    public interface ILiveFeedService : IServiceStatus, IServiceOnDemandStatus, IContainerService, IPranaServiceCommon
    {
    }
}
