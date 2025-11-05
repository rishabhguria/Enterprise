using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Prana.SecurityMasterNew
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class SecMasterOTCManager : ISecMasterOTCService
    {
        private static readonly object _locker = new object();
        static ProxyBase<IPublishing> _proxy;

        public SecMasterOTCManager()
        {
            try
            {
                var isOTCWorkflowEnabledConfig = ConfigurationManager.AppSettings["IsOTCWorkflowEnabled"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["IsOTCWorkflowEnabled"].ToString()) : false;
                CachedDataManager.GetInstance.SetOTCWorkflowPreference(isOTCWorkflowEnabledConfig);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
            }
        }

        /// <summary>
        /// Get OTC Templates
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public async Task<List<SecMasterOTCData>> GetOTCTemplatesAsync(int instrumentTypeId = 0)
        {
            Logger.LoggerWrite("Getting OTC Templates", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_SECMASTER_SERVICE);
            var OTCTemplates = new List<SecMasterOTCData>();
            try
            {
                OTCTemplates = await System.Threading.Tasks.Task.Run(() => SecMasterOTCDataManager.GetOTCTemplates(instrumentTypeId));
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return OTCTemplates;

        }


        /// <summary>
        /// delete OTC Templates
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public async Task<int> DeleteOTCTemplatesAsync(int templateID = -1)
        {
            Logger.LoggerWrite("Deleted OTC Template" + templateID, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_SECMASTER_SERVICE);
            int result = 0;
            try
            {
                result = await System.Threading.Tasks.Task.Run(() => SecMasterOTCDataManager.DeleteOTCTemplates(templateID));
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return result;

        }

        /// <summary>
        /// Get OTC Templates
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public async Task<SecMasterOTCData> GetOTCTemplatesDetailsAsync(int templateID)
        {
            Logger.LoggerWrite("Getting OTC Templates details for " + templateID, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_SECMASTER_SERVICE);
            var OTCTemplate = new SecMasterOTCData();
            try
            {
                OTCTemplate = await System.Threading.Tasks.Task.Run(() => SecMasterOTCDataManager.GetOTCTemplatesDetails(templateID));
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return OTCTemplate;
        }


        /// <summary>
        /// Get OTC Templates
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public async Task<SecMasterCFDData> GetCFDOTCTemplatesDetailsAsync(int templateID)
        {
            Logger.LoggerWrite("Getting OTC Templates details for " + templateID, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_SECMASTER_SERVICE);
            var OTCTemplate = new SecMasterCFDData();
            try
            {
                var OTCTemplates = await System.Threading.Tasks.Task.Run(() => SecMasterOTCDataManager.GetOTCTemplatesDetails(templateID));
                OTCTemplate = OTCTemplates as SecMasterCFDData;
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return OTCTemplate;
        }

        /// <summary>
        /// Get OTC temp data Templates
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public async Task<SecMasterConvertibleBondData> GetOTCTempDataTemplatesDetailsAsync(int templateID)
        {
            Logger.LoggerWrite("Getting OTC Templates details for " + templateID, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_SECMASTER_SERVICE);
            var OTCTemplate = new SecMasterConvertibleBondData();
            try
            {
                var OTCtempTemplates = await System.Threading.Tasks.Task.Run(() => SecMasterOTCDataManager.GetOTCTemplatesDetails(templateID));
                OTCTemplate = OTCtempTemplates as SecMasterConvertibleBondData;
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return OTCTemplate;
        }

        /// <summary>
        /// Get OTC Custom Fields
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public async Task<List<OTCCustomFields>> GetOTCCustomFieldsAsync(int instrumentTypeId = 0, int customFieldId = -1)
        {
            Logger.LoggerWrite("Getting OTC Custom Fields", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
            var customFields = new List<OTCCustomFields>();
            try
            {
                customFields = await System.Threading.Tasks.Task.Run(() => SecMasterOTCDataManager.GetOTCCustomFields(instrumentTypeId, customFieldId));
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return customFields;
        }

        /// <summary>
        /// Save OTC Templates
        /// </summary>
        /// <param name="OTCTemplates"></param>
        /// <returns></returns>
        public async Task<int> SaveOTCTemplatesAsync(SecMasterEquitySwap OTCTemplates)
        {
            Logger.LoggerWrite("Saving OTC Templates", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_SECMASTER_SERVICE);
            int resultCode = 0;
            try
            {
                resultCode = await System.Threading.Tasks.Task.Run(() => SecMasterOTCDataManager.SaveOTCTemplates(OTCTemplates));
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return resultCode;
        }



        /// <summary>
        /// Save OTC Templates
        /// </summary>
        /// <param name="OTCTemplates"></param>
        /// <returns></returns>
        public async Task<int> SaveOTCDataTemplatesAsync(SecMasterConvertibleBondData OTCTemplates)
        {
            Logger.LoggerWrite("Saving OTC Templates", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_SECMASTER_SERVICE);
            int resultCode = 0;
            try
            {
                resultCode = await System.Threading.Tasks.Task.Run(() => SecMasterOTCDataManager.SaveOTCTemplates(OTCTemplates));
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return resultCode;
        }

        /// <summary>
        /// Save CFD templates
        /// </summary>
        /// <param name="OTCTemplates"></param>
        /// <returns></returns>
        public async Task<int> SaveCFDOTCTemplatesAsync(SecMasterCFDData OTCTemplates)
        {
            Logger.LoggerWrite("Saving OTC Templates", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_SECMASTER_SERVICE);
            int resultCode = 0;
            try
            {
                resultCode = await System.Threading.Tasks.Task.Run(() => SecMasterOTCDataManager.SaveOTCTemplates(OTCTemplates));
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return resultCode;
        }
        /// <summary>
        /// SAVE OTC Custom Fields
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public async Task<int> SaveOTCCustomFieldsAsync(OTCCustomFields customField)
        {
            Logger.LoggerWrite("Saving OTC Custom Fields", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_SECMASTER_SERVICE);
            int resultCode = 0;
            try
            {
                resultCode = await System.Threading.Tasks.Task.Run(() => SecMasterOTCDataManager.SaveOTCCustomFields(customField));
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return resultCode;
        }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="customField"></param>
        /// <returns></returns>
        public async Task<int> DeleteOTCCustomFieldsAsync(int customField)
        {
            Logger.LoggerWrite("Saving OTC Custom Fields", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_SECMASTER_SERVICE);
            int resultCode = 0;
            try
            {
                resultCode = await System.Threading.Tasks.Task.Run(() => SecMasterOTCDataManager.DeleteOTCCustomFields(customField));
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return resultCode;
        }

        /// <summary>
        /// Create Publishing Proxy
        /// </summary>
        private void CreatePublishingProxy()
        {
            try
            {
                _proxy = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
            }
        }

        private void PublishData(MessageData msgData)
        {
            try
            {
                lock (_locker)
                {
                    _proxy.InnerChannel.Publish(msgData, msgData.TopicName);
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
            }
        }


        /// <summary>
        /// Get OTC Trade Data Async
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public List<OTCTradeData> GetOTCTradeDataAsync(List<string> groupIds)
        {
            Logger.LoggerWrite("Getting OTC Trade Data", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_SECMASTER_SERVICE);
            List<OTCTradeData> tradeData = new List<OTCTradeData>();
            try
            {
                tradeData = SecMasterOTCDataManager.GetOTCTradeData(groupIds);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return tradeData;
        }

        /// <summary>
        /// Get OTC Workflow Preference
        /// </summary>
        /// <returns></returns>
        public bool GetOTCWorkflowPreference()
        {
            Logger.LoggerWrite("Getting OTC Workflow Preference", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_SECMASTER_SERVICE);

            try
            {
                return CachedDataManager.GetInstance.IsNewOTCWorkflow;
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_SECMASTER_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion
    }
}
