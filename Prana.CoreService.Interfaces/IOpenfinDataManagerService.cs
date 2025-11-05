using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Prana.CoreService.Interfaces
{

    [ServiceContract(CallbackContract = typeof(IServiceStatusCallback))]
    public interface IOpenfinDataManagerService : IServiceStatus, IServiceOnDemandStatus, IContainerService, IPranaServiceCommon
    {

    }
}
