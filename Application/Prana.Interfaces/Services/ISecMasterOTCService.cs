using Prana.BusinessObjects;
using Prana.ServiceCommon.Interfaces;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface ISecMasterOTCService : IServiceOnDemandStatus
    {
        /// <summary>
        /// Get OTC Templates Async
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Task<List<SecMasterOTCData>> GetOTCTemplatesAsync(int instrumentTypeId = 0);

        /// <summary>
        /// Delete OTC Templates Async
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Task<int> DeleteOTCTemplatesAsync(int templateID = -1);

        /// <summary>
        /// Get OTC Templates Details Async
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        [OperationContract]
        Task<SecMasterOTCData> GetOTCTemplatesDetailsAsync(int templateID);

        /// <summary>
        /// Get OTC Templates Details Async
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        [OperationContract]
        Task<SecMasterCFDData> GetCFDOTCTemplatesDetailsAsync(int templateID);

        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        [OperationContract]
        Task<SecMasterConvertibleBondData> GetOTCTempDataTemplatesDetailsAsync(int templateID);

        /// <summary>
        /// Get OTC Custom Fields Async
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Task<List<OTCCustomFields>> GetOTCCustomFieldsAsync(int instrumentTypeId = 0, int customTempId = -1);

        /// <summary>
        /// Save OTC Templates Async
        /// </summary>
        /// <param name="OTCTemplates"></param>
        /// <returns></returns>
        [OperationContract]
        Task<int> SaveOTCTemplatesAsync(SecMasterEquitySwap OTCTemplates);


        /// <summary>
        /// Save OTC Templates Async
        /// </summary>
        /// <param name="OTCTemplates"></param>
        /// <returns></returns>
        [OperationContract]
        Task<int> SaveCFDOTCTemplatesAsync(SecMasterCFDData OTCTemplates);



        /// <summary>
        /// Save OTC Templates Async
        /// </summary>
        /// <param name="OTCTemplates"></param>
        /// <returns></returns>
        [OperationContract]

        Task<int> SaveOTCDataTemplatesAsync(SecMasterConvertibleBondData OTCTemplates);
        /// <summary>
        /// Save OTC Custom Fields Async
        /// </summary>
        /// <param name="customField"></param>
        /// <returns></returns>
        [OperationContract]
        Task<int> SaveOTCCustomFieldsAsync(OTCCustomFields customField);


        /// <summary>
        /// Delete OTC Custom FieldsAsync
        /// </summary>
        /// <param name="customField"></param>
        /// <returns></returns>
        [OperationContract]
        Task<int> DeleteOTCCustomFieldsAsync(int customField);




        /// <summary>
        /// Get OTC Trade data for Group (ids) 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        List<OTCTradeData> GetOTCTradeDataAsync(List<string> groupIds);

        /// <summary>
        /// Get OTC Workflow Preference
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool GetOTCWorkflowPreference();
    }
}
