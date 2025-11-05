using Prana.BusinessObjects;
using Prana.ServiceCommon.Interfaces;
using System.ServiceModel;

namespace Prana.Interfaces
{
    // NOTE: The use of one way service operations allows the callback to occur while
    //       still using the single concurrency mode rather than Reentrant or Multiple.
    [ServiceContract(
        Name = "PricingGreekAnalysisService",
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(IGreekAnalysisCallback))]
    public interface IGreekAnalysisServices : IServiceOnDemandStatus
    {
        [OperationContract(IsOneWay = true, IsInitiating = true)]
        void SendCallbackDetails();

        [OperationContract(IsOneWay = true)]
        void RequestSnapshotData(InputParametersCollection inputParametersCollection);

        [OperationContract(IsOneWay = true)]
        void RequestStepAnalysisData(InputParametersCollection inputParametersCollection);

        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void RemoveCallbackDetails();
    }
}
