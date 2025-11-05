using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Prana.CommonDataCache
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public partial class LiveFeedConnectionStatus : ILiveFeedCallback, IDisposable
    {
        private DuplexProxyBase<IPricingService> _pricingServiceProxy = null;
        private bool _isAlreadySubscribed;

        public event EventHandler<EventArgs<bool>> LiveFeedConnectionStatusChanged;

        private void CreatePricingServiceProxy()
        {
            _pricingServiceProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
        }

        public LiveFeedConnectionStatus()
        {
        }

        public void Subscribe()
        {
            try
            {
                if (_pricingServiceProxy == null)
                {
                    CreatePricingServiceProxy();
                }

                try
                {
                    if (_pricingServiceProxy.InnerChannel != null && !_isAlreadySubscribed)
                    {
                        _pricingServiceProxy.InnerChannel.Subscribe();
                        _isAlreadySubscribed = true;
                    }
                }
                catch
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("PricingService2 not connected", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
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

        private void Unsubscribe()
        {
            try
            {
                if (_pricingServiceProxy != null)
                {
                    try
                    {
                        if (_pricingServiceProxy.InnerChannel != null && _isAlreadySubscribed)
                        {
                            _pricingServiceProxy.InnerChannel.UnSubscribe();
                            _isAlreadySubscribed = false;
                        }
                    }
                    catch
                    {
                    }
                    _pricingServiceProxy.Dispose();
                }
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

        #region ILiveFeedCallback Members
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            try
            {
                if (LiveFeedConnectionStatusChanged != null)
                {
                    LiveFeedConnectionStatusChanged(this, new EventArgs<bool>(true));
                }
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

        public void LiveFeedDisConnected()
        {
            try
            {
                _isAlreadySubscribed = false;

                if (LiveFeedConnectionStatusChanged != null)
                {
                    LiveFeedConnectionStatusChanged(this, new EventArgs<bool>(false));
                }
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
                Unsubscribe();
                if (_pricingServiceProxy != null)
                    _pricingServiceProxy.Dispose();
            }
        }
        #endregion
    }
}
