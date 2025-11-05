using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Act40OrderGeneratorTool
{
    class PricingServiceConnector : IDisposable, ILiveFeedCallback
    {

        /// <summary>
        /// proxy for the allocation service, used for pre allocation before sending order to compliance
        /// </summary>
        private DuplexProxyBase<IPricingService> _pricingService = null;

        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static PricingServiceConnector _pricingServiceConnector = null;

        /// <summary>
        /// private cunstructor, Initialises the proxy
        /// </summary>
        private PricingServiceConnector()
        {
            try
            {
                _pricingService = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
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
        internal static PricingServiceConnector GetInstance()
        {
            try
            {
                lock (_lock)
                {
                    if (_pricingServiceConnector == null)
                        _pricingServiceConnector = new PricingServiceConnector();
                    return _pricingServiceConnector;
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

        internal Double GetPrice(String symbol)
        {
            try
            {
                return _pricingService.InnerChannel.GetDynamicSymbolData(symbol).SelectedFeedPrice;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return 0.0;
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
                    if (_pricingService != null)
                        _pricingService.Dispose();
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

        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
        }

        public void LiveFeedDisConnected()
        {
        }
    }
}
