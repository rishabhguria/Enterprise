using Prana.ServiceCommon.Interfaces;
using System.ServiceModel;

namespace Prana.CoreService.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IServiceStatusCallback))]
    public interface ILayoutService : IServiceStatus , IServiceOnDemandStatus , IContainerService, IPranaServiceCommon
    {
    }
}
