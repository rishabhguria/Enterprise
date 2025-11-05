using Prana.BusinessObjects;
using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.Interfaces
{
    // NOTE: The use of one way service operations allows the callback to occur while
    //       still using the single concurrency mode rather than Reentrant or Multiple.
    [ServiceKnownType(typeof(string))]
    [ServiceKnownType(typeof(int))]
    [ServiceKnownType(typeof(ResponseObj))]
    [ServiceKnownType(typeof(List<StepAnalysisResponse>))]
    [ServiceKnownType(typeof(List<object>))]
    public interface IGreekAnalysisCallback
    {
        [OperationContract(IsOneWay = true)]
        void ProcessSnapshotData(object state);

        [OperationContract(IsOneWay = true)]
        void ProcessStepAnalysisData(object state);
    }
}
