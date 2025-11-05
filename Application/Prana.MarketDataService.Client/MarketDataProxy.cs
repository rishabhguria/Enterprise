using Prana.LogManager;
using Prana.MarketDataAdapter.Common;
using Prana.MarketDataService.Common;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;

namespace Prana.MarketDataService.Client
{
    public class MarketDataProxy : IMarketDataCallback
    {
        DuplexProxyBase<IMarketDataService> _serviceProxy = null;

        public MarketDataProxy()
        {
            MarketDataAdapterExtension.CreateSecMasterServicesProxy();
            CreateMarketDataServicesProxy();
        }

        private void CreateMarketDataServicesProxy()
        {
            try
            {
                _serviceProxy = new DuplexProxyBase<IMarketDataService>("MarketDataServiceEndpointAddress", this);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information//MarketDataServiceAddress
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SendResult(Dictionary<string, Dictionary<string, string>> result)
        {

        }

        public void AdviseSymbols(MDServiceReqObject smObject)
        {
            _serviceProxy.InnerChannel.AdviseSymbol(smObject);
        }

        public void AdviseSymbolList(List<MDServiceReqObject> smObjects)
        {
            _serviceProxy.InnerChannel.AdviseSymbolList(smObjects);
        }

        public Dictionary<string, string> SnapshotSymbol(MDServiceReqObject smObject)
        {
            if (_serviceProxy.IsContainerServiceConnected())
            {
                return _serviceProxy.InnerChannel.SnapshotSymbol(smObject);
            }
            else
                return null;
        }

        public Dictionary<string, string> GetSMData(MDServiceReqObject symbol)
        {
            if (_serviceProxy.IsContainerServiceConnected())
            {
                return _serviceProxy.InnerChannel.GetSMData(symbol);
            }
            else
                return null;
        }

        public void DeleteSymbol(MDServiceReqObject smObject)
        {
            _serviceProxy.InnerChannel.DeleteSymbol(smObject);
        }
    }
}
