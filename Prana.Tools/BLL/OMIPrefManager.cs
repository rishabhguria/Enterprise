using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;

namespace Prana.Tools
{
    public class OMIPrefManager
    {
        private DuplexProxyBase<IPricingService> _pricingServiceProxy;
        public DuplexProxyBase<IPricingService> PricingServiceProxy
        {
            get { return _pricingServiceProxy; }
            set { _pricingServiceProxy = value; }
        }
        /// <summary>
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <returns></returns>
        public LiveFeedPreferences GetPreferences()
        {
            LiveFeedPreferences omiPreferences = new LiveFeedPreferences();
            try
            {
                omiPreferences = _pricingServiceProxy.InnerChannel.GetOMILiveFeedPreferences();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return omiPreferences;
        }
        /// <summary>
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void SaveOMIPreferences()
        {
            try
            {
                _pricingServiceProxy.InnerChannel.SaveOMILiveFeedPreferences(_omiPreferences);
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

        private LiveFeedPreferences _omiPreferences;
        public LiveFeedPreferences OMIPreferences
        {
            get
            {
                if (_omiPreferences == null)
                    _omiPreferences = GetPreferences();

                return _omiPreferences;
            }
            set
            {
                _omiPreferences = value;
            }
        }
    }
}
