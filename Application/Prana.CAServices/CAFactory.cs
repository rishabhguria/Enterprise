using Prana.BusinessObjects.AppConstants;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


namespace Prana.CAServices
{
    internal class CAFactory
    {
        static int _hashCode = 0;
        static IPostTradeServices _postTradeServicesInstance;
        static IAllocationServices _allocationServices = null;
        static IClosingServices _closingServices = null;
        static ProxyBase<IPublishing> _proxyPublishing = null;
        static IActivityServices _activityService;
        static DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        static ISecMasterServices _secmasterProxy = null;
        static ICashManagementService _cashManagementService = null;

        static Dictionary<CorporateActionType, ICorporateActionBaseRule> _caInstances = new Dictionary<CorporateActionType, ICorporateActionBaseRule>();

        internal static void Initialize(IPostTradeServices postTradeServicesInstance, int hashCode, IAllocationServices allocationServices, IClosingServices closingServices, ProxyBase<IPublishing> proxyPublishing, IActivityServices activityService, DuplexProxyBase<IPricingService> pricingService, ISecMasterServices secmasterProxy, ICashManagementService cashManagementService)
        {
            try
            {
                _postTradeServicesInstance = postTradeServicesInstance;
                _hashCode = hashCode;
                _allocationServices = allocationServices;
                _closingServices = closingServices;
                _proxyPublishing = proxyPublishing;
                _activityService = activityService;
                _pricingServicesProxy = pricingService;
                _secmasterProxy = secmasterProxy;
                _cashManagementService = cashManagementService;
                XMLCacheManager.Instance.LoadXML();
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

        internal static ICorporateActionBaseRule GetCAInstance(CorporateActionType caType)
        {
            try
            {
                if (_caInstances.ContainsKey(caType))
                {
                    return _caInstances[caType];
                }
                else
                {
                    string className;
                    string dllPath;
                    XMLCacheManager.Instance.GetCADllClassPath(caType, out className, out dllPath);
                    if (String.IsNullOrEmpty(className) || String.IsNullOrEmpty(dllPath))
                    {
                        return null;
                    }
                    string dllFullPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + dllPath;

                    Assembly assembly = Assembly.LoadFile(dllFullPath);
                    Type type = assembly.GetType(className);
                    ICorporateActionBaseRule caInstance = (ICorporateActionBaseRule)Activator.CreateInstance(type);
                    caInstance.Initialize(_postTradeServicesInstance, _hashCode, _allocationServices, _closingServices, _proxyPublishing, _activityService, _pricingServicesProxy, _secmasterProxy, _cashManagementService);
                    _caInstances.Add(caType, caInstance);
                    return (ICorporateActionBaseRule)caInstance;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        internal static void ResetCache()
        {
            _caInstances.Clear();
        }


    }
}
