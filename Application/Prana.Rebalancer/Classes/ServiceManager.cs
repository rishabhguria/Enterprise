using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;

namespace Prana.Rebalancer
{
    public class ServiceManager : IDisposable
    {
        #region singleton
        private static volatile ServiceManager instance;
        private static object syncRoot = new Object();

        private ServiceManager() { }

        public static ServiceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ServiceManager();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        static ProxyBase<IPranaPositionServices> _pranaPositionServices = null;
        public ProxyBase<IPranaPositionServices> PranaPositionServices
        {
            set
            {
                _pranaPositionServices = value;
            }
            get
            {
                if (_pranaPositionServices == null)
                {
                    CreatePositionServicesProxy();
                }
                return _pranaPositionServices;
            }
        }


        /// <summary>
        /// Create Position Services Proxyto connect with server
        /// </summary>
        public void CreatePositionServicesProxy()
        {
            try
            {
                if (_pranaPositionServices == null)
                {
                    _pranaPositionServices = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
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

        /// <summary>
        /// Create sec master Services Proxy to connect with server
        /// </summary>
        ProxyBase<ISecMasterSyncServices> _secMasterServices = null;
        public ProxyBase<ISecMasterSyncServices> SecMasterServices
        {
            set
            {
                _secMasterServices = value;
            }
            get
            {
                if (_secMasterServices == null)
                {
                    CreateSecMasterServicesProxy();
                }
                return _secMasterServices;
            }
        }

        public void CreateSecMasterServicesProxy()
        {
            try
            {
                _secMasterServices = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
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

        ProxyBase<IAllocationManager> _allocationManager = null;
        public ProxyBase<IAllocationManager> AllocationManager
        {
            set
            {
                _allocationManager = value;
            }
            get
            {
                if (_allocationManager == null)
                {
                    CreateAllocationManagerProxy();
                }
                return _allocationManager;
            }
        }

        /// <summary>
        /// Create allocation manager Proxy to connect with server
        /// </summary>
        public void CreateAllocationManagerProxy()
        {
            try
            {
                if (_allocationManager == null)
                {
                    _allocationManager = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
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



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _secMasterServices.Dispose();
                    _allocationManager.Dispose();
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
    }
}
