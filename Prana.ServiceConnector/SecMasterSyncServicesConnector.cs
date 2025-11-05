using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.UIEventAggregator;
using Prana.UIEventAggregator.Events;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;

namespace Prana.ServiceConnector
{
    /// <summary>
    /// SecMasterSyncServicesConnector
    /// </summary>
    public class SecMasterSyncServicesConnector : IDisposable
    {
        /// <summary>
        /// The _expnl connector service
        /// </summary>
        private ProxyBase<ISecMasterSyncServices> _secMasterSyncServices = null;

        #region SingletonInstance
        /// <summary>
        /// The _lock
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// The _expnl service connector
        /// </summary>
        private static SecMasterSyncServicesConnector _secMasterSyncServicesConnector = null;

        /// <summary>
        /// Prevents a default instance of the <see cref="ExpnlServiceConnector"/> class from being created.
        /// </summary>
        private SecMasterSyncServicesConnector()
        {
            try
            {
                _secMasterSyncServices = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
                _secMasterSyncServices.ConnectedEvent += SecMasterSyncServiceOnConnectedEvent;
                _secMasterSyncServices.DisconnectedEvent += SecMasterSyncService_DisconnectedEvent;
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
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static SecMasterSyncServicesConnector GetInstance()
        {
            try
            {
                lock (_lock)
                {
                    if (_secMasterSyncServicesConnector == null)
                        _secMasterSyncServicesConnector = new SecMasterSyncServicesConnector();
                    return _secMasterSyncServicesConnector;
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

        #region proxy
        /// <summary>
        /// Tries the get channel.
        /// </summary>
        /// <returns></returns>
        public string TryGetChannel()
        {
            try
            {
                var expnlConnectorServiceChannel = _secMasterSyncServices.InnerChannel;

                if (expnlConnectorServiceChannel != null)
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

        /// <summary>
        /// SecMasterSyncService
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SecMasterSyncServiceOnConnectedEvent(object sender, EventArgs eventArgs)
        {
            try
            {
                EventAggregator.GetInstance.PublishEvent(new PTTExpnlStatus
                {
                    IsExpnlServiceConnected = true
                });
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the DisconnectedEvent event of the _expnlConnectorService control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SecMasterSyncService_DisconnectedEvent(object sender, EventArgs e)
        {

        }
        #endregion

        public Dictionary<string, SecMasterBaseObj> GetSecMasterSymbolData(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCode)
        {
            return _secMasterSyncServices.InnerChannel.GetSecMasterSymbolData(symbolList, symbologyCode);
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
                    if (_secMasterSyncServices != null)
                        _secMasterSyncServices.Dispose();
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
    }
}