using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using System.Data;
using System.ServiceModel;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface ICentralSMSecurityCallback
    {
        [OperationContract(IsOneWay = true)]
        void SecurityValidationResp(SecMasterBaseObj securityData);

        [OperationContract(IsOneWay = true)]
        void GenricPricingResp(string requestID, DataTable pricingTable, bool pricingSuccess, string comment);

        [OperationContract(IsOneWay = true)]
        void SymbolLookUpSearchDataResp(DataSet SymbolsData, SymbolLookupRequestObject symbolLookupRequestObject);

        [OperationContract(IsOneWay = true)]
        void SaveFutureRootDataLocal(DataTable FutureRootData);

        [OperationContract(IsOneWay = true)]
        void FutureRootDataResp(DataSet futureData);

        [OperationContract(IsOneWay = true)]
        void IsAliveResp();

        /// <summary>
        /// Created by omshiv, send any type of response to subscriber
        /// </summary>
        /// <param name="responseType"></param>
        /// <param name="message"></param>
        [OperationContract(IsOneWay = true)]
        void GenericCentralSMResponse(string responseType, object message);
    }
}
