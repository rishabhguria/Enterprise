using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Prana.CentralSM
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICentralSMService" in both code and config file together.
    [ServiceContract(CallbackContract=typeof(ICentralSMSecurityCallback))]
    public interface ICentralSMService
    { 
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void GetSecMasterDataCentralSM(SecMasterRequestObj secMasterReqObj,string clientName, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
         //modified by omshiv, added secondary pricingSource 
        void GetGenericSMPrice(string requestID, String PricingSource, List<string> fields, SecMasterRequestObj secMasterReqObj, DateTime startDate, DateTime endDate, string clientName, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback, bool isGetDataFromCacheOrDB);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void SaveSecMasterDataCentralSM(SecMasterbaseList secMasterData, String ResquestID, String UserID, string clientName, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback);
        
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void SearchSecurity(SymbolLookupRequestObject symbolLookupReqObj, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback);
        
        
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void SaveFutureRootData(DataTable dt, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void GetFutureRootData();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        bool IsAlive(string clientName, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void Disconnect(object sender, EventArgs<string> e);


        /// <summary>
        /// Created by omshiv, Sending any type of data to CSM with callback
        /// </summary>
        /// <param name="RequestKey">RequestKey</param>
        /// <param name="Data">Data</param>
        /// <param name="securityCallback">SecurityCallback</param>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void GenericeSendDataToCSM(String RequestKey, object Data, String ClientName, [Optional, DefaultParameterValue(null)] ICentralSMSecurityCallback securityCallback);
       


        //[OperationContract]
        //[FaultContract(typeof(PranaAppException))]
        //void Subscribe();

        //[OperationContract]
        //[FaultContract(typeof(PranaAppException))]
        //void UnSubscribe();
    }
}