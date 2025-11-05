using Prana.Interfaces;
using Prana.WCFConnectionMgr;
using System;

namespace Prana.Import
{
    public class ServiceManager
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

        static ProxyBase<IAllocationManager> _allocationServices = null;
        public ProxyBase<IAllocationManager> AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
            get
            {
                return _allocationServices;
            }
        }

        static DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        public DuplexProxyBase<IPricingService> PricingServices
        {
            set
            {
                _pricingServicesProxy = value;
            }
            get
            {
                return _pricingServicesProxy;
            }
        }

        static ProxyBase<ICashManagementService> _CashManagementServices = null;
        public ProxyBase<ICashManagementService> CashManagementServices
        {
            set
            {
                _CashManagementServices = value;

            }
            get { return _CashManagementServices; }
        }


    }
}