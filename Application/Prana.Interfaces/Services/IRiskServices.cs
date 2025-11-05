using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.ServiceCommon.Interfaces;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface IRiskServices : IServiceOnDemandStatus
    {
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void UpdateRiskPreferences(RiskPrefernece Preference);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Task<PranaRequestCarrier> CalculateRiskRelatedData(PranaRequestCarrier pranaReqCarrier, bool isOnlyRiskCorrelationStdDevRequired);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Task<PranaRequestCarrier> CalculateStressTestData(PranaRequestCarrier pranaRequestCarrier, PranaRequestCarrier pranaRequestCarrierForBeta);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        PranaRequestCarrier CalculateHistoricalVol(PranaRequestCarrier pranaReqCarrier);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        bool CheckRiskServiceConnected();
    }
}
