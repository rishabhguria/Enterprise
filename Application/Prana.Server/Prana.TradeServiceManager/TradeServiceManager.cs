using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.CoreService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;

namespace Prana.TradeServiceManager
{
    public class TradeServiceManager : IDisposable, IServiceStatusCallback
    {
        private static TradeServiceManager _tradeServiceManager = null;
        private DuplexProxyBase<ITradeService> _tradeServiceProxy = null;

        private TradeServiceManager()
        {
            try
            {
                _tradeServiceProxy = new DuplexProxyBase<ITradeService>("TradeServiceEndpointAddress", this);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static TradeServiceManager GetInstance
        {
            get
            {
                if (_tradeServiceManager == null)
                {
                    _tradeServiceManager = new TradeServiceManager();
                }
                return _tradeServiceManager;
            }
        }

        #region IServiceStatusCallback Methods
        public void HeartbeatReceived()
        {
        }

        public void ServiceClosed()
        {
        }
        #endregion

        #region IContainerService Methods
        public async System.Threading.Tasks.Task RequestStartupData()
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.RequestStartupData();
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

        public async System.Threading.Tasks.Task<byte[]> OpenLog()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.OpenLog();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<byte[]> LoadLog()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.LoadLog();
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

            return null;
        }

        public async System.Threading.Tasks.Task StopService()
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.StopService();
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

        public async System.Threading.Tasks.Task<List<HostedService>> GetClientServicesStatus()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.GetClientServicesStatus();
            }
            catch
            {
                return new List<HostedService>();
            }
        }

        public async System.Threading.Tasks.Task SetDebugModeStatus(bool isDebugModeEnabled)
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.SetDebugModeStatus(isDebugModeEnabled);
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

        public async System.Threading.Tasks.Task<bool> GetDebugModeStatus()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.GetDebugModeStatus();
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
            return false;
        }
        #endregion

        #region ITradeService Methods
        public async System.Threading.Tasks.Task ReloadRules()
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.ReloadRules();
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

        public async System.Threading.Tasks.Task ReloadXslt()
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.ReloadXslt();
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

        public async System.Threading.Tasks.Task<bool> IsTradeServiceReadyForClose()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.IsTradeServiceReadyForClose();
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

                return false;
            }
        }

        public async System.Threading.Tasks.Task GetMessageStatus(Order order)
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.GetMessageStatus(order);
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

        public async System.Threading.Tasks.Task MoveOldTrade()
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.MoveOldTrade();
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

        public async System.Threading.Tasks.Task RefreshCacheClosing()
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.RefreshCacheClosing();
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

        public async System.Threading.Tasks.Task RefreshPreferenceCache()
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.RefreshPreferenceCache();
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

        public async System.Threading.Tasks.Task SendManualDropsOnFix()
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.SendManualDropsOnFix();
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

        public async System.Threading.Tasks.Task ClearFixTradeOrderCache()
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.ClearFixTradeOrderCache();
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

        public async System.Threading.Tasks.Task ClearFixTradeOrder(string orderID)
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.ClearFixTradeOrder(orderID);
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

        public async System.Threading.Tasks.Task<List<string>> FetchProcessorNames()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.FetchProcessorNames();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<OrderCollection> ShowMessagesForProcessor(string processorName)
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.ShowMessagesForProcessor(processorName);
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<OrderCollection> ShowErrorMessagesForProcessor(string processorName)
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.ShowErrorMessagesForProcessor(processorName);
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<List<string>> PersistedMessagesReceivedFromClient()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.PersistedMessagesReceivedFromClient();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<List<string>> PersistedMessagesSentToBroker()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.PersistedMessagesSentToBroker();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<List<string>> PersistedMessagesReceivedFromBroker()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.PersistedMessagesReceivedFromBroker();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<OrderCollection> PendingPreTradeCompliance()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.PendingPreTradeCompliance();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<OrderCollection> PendingApprovalTrades()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.PendingApprovalTrades();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task<Dictionary<int, FixPartyDetails>> GetFixAllPartyDetails()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.GetFixAllPartyDetails();
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

                return null;
            }
        }

        public async System.Threading.Tasks.Task OverideTrade(bool isAllowed, String orderId)
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.OverideTrade(isAllowed, orderId);
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

        public async System.Threading.Tasks.Task ReProcessMsg(string jsonDataRow)
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.ReProcessMsg(jsonDataRow);
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

        public async System.Threading.Tasks.Task ReProcessMsg2(Order pranaOrder)
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.ReProcessMsg2(pranaOrder);
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

        public async System.Threading.Tasks.Task FixEngineConnectBuySide(int connectionID)
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.FixEngineConnectBuySide(connectionID);
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

        public async System.Threading.Tasks.Task FixEngineDisconnectBuySide(int connectionID)
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.FixEngineDisconnectBuySide(connectionID);
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

        public async System.Threading.Tasks.Task SetFixConnectionsAutoReconnectStatus(bool autoConnectStatus)
        {
            try
            {
                await _tradeServiceProxy.InnerChannel.SetFixConnectionsAutoReconnectStatus(autoConnectStatus);
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

        public async System.Threading.Tasks.Task<bool> IsComplianceModulePermitted()
        {
            try
            {
                return await _tradeServiceProxy.InnerChannel.IsComplianceModulePermitted();
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

                return false;
            }
        }
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tradeServiceProxy.Dispose();
            }
        }
        #endregion
    }
}