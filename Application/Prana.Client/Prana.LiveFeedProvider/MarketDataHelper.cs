using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Prana.LiveFeedProvider
{
    public class MarketDataHelper : ILiveFeedCallback, IDisposable
    {
        private static MarketDataHelper _instance = null;
        private DuplexProxyBase<IPricingService> _pricingServicesProxy;
        //private bool _connectionStatus = false;

        public bool IsDataManagerConnected()
        {
            CheckAndSetPricingServiceProxy();
            return _pricingServicesProxy.InnerChannel.IsLiveFeedActive;
        }

        private MarketDataHelper()
        {
            CheckAndSetPricingServiceProxy();
        }

        private static object _lockerObject = new object();

        public static MarketDataHelper GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock (_lockerObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new MarketDataHelper();
                        }
                    }
                }

                return _instance;
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
        public event EventHandler<EventArgs<SymbolData>> OnResponse;
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            if (OnResponse != null)
                OnResponse(this, new EventArgs<SymbolData>(data));
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

        public void RemoveSingleSymbol(string symbol)
        {
            CheckAndSetPricingServiceProxy();
            _pricingServicesProxy.InnerChannel.RemoveSymbol_TTandPTT(symbol);
        }

        /// <summary>
        /// This function is used to remove multiple symbols from the pricing server
        /// </summary>
        /// <param name="symbolSet">HashSet<string></param>
        public void RemoveMultipleSymbols(List<string> symbolSet)
        {
            try
            {
                CheckAndSetPricingServiceProxy();
                _pricingServicesProxy.InnerChannel.RemoveMultipleSymbols(symbolSet);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void RequestSingleSymbol(string symbol, bool isSnapShot)
        {
            CheckAndSetPricingServiceProxy();
            _pricingServicesProxy.InnerChannel.RequestSymbol_TTandPTT(symbol, null, isSnapShot);
        }

        /// <summary>
        /// This function is used to remove symbol from the pricing server
        /// </summary>
        /// <param name="symbolsToBeRemoved">string</param>
        public void RemoveSnapshotForCompliance(List<string> symbolsToBeRemoved)
        {
            try
            {
                CheckAndSetPricingServiceProxy();
                _pricingServicesProxy.InnerChannel.RemoveSnapshotForCompliance(symbolsToBeRemoved);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This function is used to request symbol from the pricing server
        /// </summary>
        /// <param name="symbol">string</param>
        public void RequestSnapshotForCompliance(List<string> requestedSymbols)
        {
            try
            {
                CheckAndSetPricingServiceProxy();
                _pricingServicesProxy.InnerChannel.RequestSnapshotForCompliance(requestedSymbols);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This function is used to add multiple symbols from the pricing server
        /// </summary>
        /// <param name="symbolSet">HashSet<string></param>
        /// <param name="isSnapShot">bool</param>
        public void RequestMultipleSymbols(List<string> symbols, bool isSnapshot)
        {
            try
            {
                CheckAndSetPricingServiceProxy();
                _pricingServicesProxy.InnerChannel.RequestMultipleSymbols(symbols, null, isSnapshot);
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if(_pricingServicesProxy != null)
            {
                _pricingServicesProxy.Dispose();
                _pricingServicesProxy = null;
            }
            _instance = null;
        }

        private void CheckAndSetPricingServiceProxy()
        {
            if(_pricingServicesProxy == null)
                _pricingServicesProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
        }
    }
}
