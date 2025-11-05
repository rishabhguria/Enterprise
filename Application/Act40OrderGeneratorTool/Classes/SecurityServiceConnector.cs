using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;

namespace Act40OrderGeneratorTool
{
    class SecurityServiceConnector : IDisposable
    {
        /// <summary>
        /// proxy for the allocation service, used for pre allocation before sending order to compliance
        /// </summary>
        private ProxyBase<ISecMasterSyncServices> _securityMasterService = null;

        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static SecurityServiceConnector _securityServiceConnector = null;

        /// <summary>
        /// private cunstructor, Initialises the proxy
        /// </summary>
        private SecurityServiceConnector()
        {
            try
            {
                _securityMasterService = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
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

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static SecurityServiceConnector GetInstance()
        {
            try
            {
                lock (_lock)
                {
                    if (_securityServiceConnector == null)
                        _securityServiceConnector = new SecurityServiceConnector();
                    return _securityServiceConnector;
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
            return null;
        }
        #endregion

        internal String GetSector(String symbol)
        {
            try
            {
                var security = _securityMasterService.InnerChannel.GetSecMasterSymbolData(new List<String>() { symbol }, ApplicationConstants.SymbologyCodes.TickerSymbol);
                return security[symbol].SymbolUDAData.UDASector;
            }
            catch (Exception)
            {
                return "";
            }
        }

        #region IDisposable
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
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

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_securityMasterService != null)
                        _securityMasterService.Dispose();
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
