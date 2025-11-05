using Prana.BusinessObjects.Classes.Allocation;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ClientCommon
{
    public class AllocationSchemeDataManager
    {
        static AllocationSchemeDataManager _allocationSchemeDataManager = null;

        static List<string> _currencyListForAlloScheme = null;

        static ProxyBase<IAllocationManager> _proxyAllocationServices = null;

        static AllocationSchemeDataManager()
        {
            _allocationSchemeDataManager = new AllocationSchemeDataManager();
            CreateAllocationServicesProxy();
            _currencyListForAlloScheme = _proxyAllocationServices.InnerChannel.GetCurrencyListForAllocationScheme();
        }

        public static AllocationSchemeDataManager GetInstance
        {
            get
            {
                return _allocationSchemeDataManager;
            }
        }

        private static void CreateAllocationServicesProxy()
        {
            _proxyAllocationServices = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");

        }

        public List<string> GetCurrencyLIstForAllocationScheme()
        {
            return _currencyListForAlloScheme;
        }
        public AllocationFixedPreference GetAllocationSchemeByName(string allocationSchemeName)
        {
            AllocationFixedPreference allocationScheme = null;
            try
            {
                allocationScheme = _proxyAllocationServices.InnerChannel.GetAllocationSchemeByName(allocationSchemeName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return allocationScheme;
        }

        public DataSet GetAllocationSchemeReconReport(string schemeName, DateTime fromDate, DateTime toDate)
        {
            DataSet dsAllocationSchemeReconReport = null;
            try
            {
                dsAllocationSchemeReconReport = _proxyAllocationServices.InnerChannel.GetAllocationSchemeReconReport(schemeName, fromDate, toDate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dsAllocationSchemeReconReport;
        }

        public Dictionary<int, string> GetAllASchemeNames()
        {
            Dictionary<int, string> schemeNames = null;
            try
            {
                schemeNames = _proxyAllocationServices.InnerChannel.GetAllAllocationSchemeNames();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return schemeNames;
        }

        public int SaveAllocationScheme(AllocationFixedPreference fixedPref)
        {
            int resultantID = 0;
            try
            {
                resultantID = _proxyAllocationServices.InnerChannel.SaveAllocationScheme(fixedPref);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return resultantID;
        }

        public void DisposeAllocationServicesProxy()
        {
            try
            {
                _proxyAllocationServices.Dispose();
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

    }
}
