using Prana.ServiceCommon.Interfaces;
using System.ServiceModel;

namespace Prana.CoreService.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IServiceStatusCallback))]
    public interface IComplianceAlertsService : IServiceStatus, IServiceOnDemandStatus, IContainerService, IPranaServiceCommon
    {
    }
}
