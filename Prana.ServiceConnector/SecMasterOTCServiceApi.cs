using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Prana.ServiceConnector
{
    public class SecMasterOTCServiceApi : ISecMasterOTCService, IDisposable
    {

        /// <summary>
        /// The OTC service
        /// </summary>
        private ProxyBase<ISecMasterOTCService> _secMasterOTCService = null;

        #region SingletonInstance
        /// <summary>
        /// The _lock
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// The _expnl service connector
        /// </summary>
        private static SecMasterOTCServiceApi _secMasterOTCServiceApi = null;


        /// <summary>
        /// Prevents a default instance of the <see cref="RebalancerServiceApi"/> class from being created.
        /// </summary>
        private SecMasterOTCServiceApi()
        {
            try
            {
                _secMasterOTCService = new ProxyBase<ISecMasterOTCService>("TradeOTCServiceEndpointAddress");
                _secMasterOTCService.ConnectedEvent += SecMasterOTCServiceOnConnectedEvent;
                _secMasterOTCService.DisconnectedEvent += SecMasterOTCServiceOnDisConnectedEvent;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sec Master OTC Service On DisConnected Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecMasterOTCServiceOnDisConnectedEvent(object sender, EventArgs e)
        {
            _isOTCServiceConnected = false;
        }

        /// <summary>
        /// SecMaster OTC Service On  Connected Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecMasterOTCServiceOnConnectedEvent(object sender, EventArgs e)
        {
            _isOTCServiceConnected = true;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static SecMasterOTCServiceApi GetInstance()
        {
            try
            {
                lock (_lock)
                {
                    if (_secMasterOTCServiceApi == null)
                        _secMasterOTCServiceApi = new SecMasterOTCServiceApi();
                    return _secMasterOTCServiceApi;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }
        #endregion

        private bool _isOTCServiceConnected;
        public bool IsOTClServiceConnected
        {
            get { return _isOTCServiceConnected; }
            set { _isOTCServiceConnected = value; }
        }

        /// <summary>
        /// Tries the get channel.
        /// </summary>
        /// <returns></returns>
        public string TryGetChannel()
        {
            try
            {
                var rebalancerServiceChannel = _secMasterOTCService.InnerChannel;

                if (rebalancerServiceChannel != null)
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        #region IDisposable

        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_secMasterOTCServiceApi != null)
                        _secMasterOTCServiceApi = null;
                    if (_secMasterOTCService != null)
                    {
                        _secMasterOTCService.ConnectedEvent -= SecMasterOTCServiceOnConnectedEvent;
                        _secMasterOTCService.DisconnectedEvent -= SecMasterOTCServiceOnDisConnectedEvent;
                        _secMasterOTCService.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        /// <summary>
        /// Get OTC Templates Async
        /// </summary>
        /// <returns></returns>
        public Task<List<SecMasterOTCData>> GetOTCTemplatesAsync(int instrumentTypeId = 0)
        {
            return _secMasterOTCService.InnerChannel.GetOTCTemplatesAsync(instrumentTypeId);
        }

        /// <summary>
        /// Get OTC Templates Async
        /// </summary>
        /// <returns></returns>
        public Task<int> DeleteOTCTemplatesAsync(int templateID = -1)
        {
            return _secMasterOTCService.InnerChannel.DeleteOTCTemplatesAsync(templateID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public Task<SecMasterOTCData> GetOTCTemplatesDetailsAsync(int templateID)
        {
            return _secMasterOTCService.InnerChannel.GetOTCTemplatesDetailsAsync(templateID);
        }


        public Task<SecMasterCFDData> GetCFDOTCTemplatesDetailsAsync(int templateID)
        {
            return _secMasterOTCService.InnerChannel.GetCFDOTCTemplatesDetailsAsync(templateID);
        }

        public Task<SecMasterConvertibleBondData> GetOTCTempDataTemplatesDetailsAsync(int templateID)
        {
            return _secMasterOTCService.InnerChannel.GetOTCTempDataTemplatesDetailsAsync(templateID);
        }

        /// <summary>
        /// Get OTC Custom Fields Async
        /// </summary>
        /// <returns></returns>
        public Task<List<OTCCustomFields>> GetOTCCustomFieldsAsync(int instrumentTypeId = 0, int customTempId = -1)
        {
            return _secMasterOTCService.InnerChannel.GetOTCCustomFieldsAsync(instrumentTypeId, customTempId);
        }

        /// <summary>
        /// Save OTC Templates Async
        /// </summary>
        /// <param name="OTCTemplates"></param>
        /// <returns></returns>
        public Task<int> SaveOTCTemplatesAsync(SecMasterEquitySwap OTCTemplates)
        {
            try
            {
                return _secMasterOTCService.InnerChannel.SaveOTCTemplatesAsync(OTCTemplates);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<int> SaveCFDOTCTemplatesAsync(SecMasterCFDData CFDDataFinal)
        {
            try
            {
                return _secMasterOTCService.InnerChannel.SaveCFDOTCTemplatesAsync(CFDDataFinal);
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CFDDataFinal"></param>
        /// <returns></returns>
        /// 
        ///todo: SecMasterOTCData create OTCviewData
        public Task<int> SaveOTCDataTemplatesAsync(SecMasterConvertibleBondData OTCDataFinal)
        {
            try
            {
                return _secMasterOTCService.InnerChannel.SaveOTCDataTemplatesAsync(OTCDataFinal);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Save OTC Custom Fields Async
        /// </summary>
        /// <param name="customField"></param>
        /// <returns></returns>
        public Task<int> SaveOTCCustomFieldsAsync(OTCCustomFields customField)
        {
            return _secMasterOTCService.InnerChannel.SaveOTCCustomFieldsAsync(customField);
        }

        /// <summary>
        /// Delete OTC Custom Fields Async
        /// </summary>
        /// <param name="customField"></param>
        /// <returns></returns>
        public Task<int> DeleteOTCCustomFieldsAsync(int customFieldId)
        {
            return _secMasterOTCService.InnerChannel.DeleteOTCCustomFieldsAsync(customFieldId);
        }


        public List<OTCTradeData> GetOTCTradeDataAsync(List<string> groupIds)
        {
            return _secMasterOTCService.InnerChannel.GetOTCTradeDataAsync(groupIds);
        }


        public bool GetOTCWorkflowPreference()
        {
            var isOTCWorkflow = _secMasterOTCService.InnerChannel.GetOTCWorkflowPreference();
            CachedDataManager.GetInstance.SetOTCWorkflowPreference(isOTCWorkflow);
            return isOTCWorkflow;
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
