using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;

namespace Prana.ClientCommon
{
    public class ClientLevelConstants
    {
        #region MarketData Constants
        public static string HEADER_MARKET_DATA_ALERT
        {
            get
            {
                switch (CachedDataManager.CompanyMarketDataProvider)
                {
                    case MarketDataProvider.FactSet:
                        return "Nirvana Market Data Provider: FactSet";
                    case MarketDataProvider.ACTIV:
                        return "Nirvana Market Data Provider: ACTIV";
                    case MarketDataProvider.SAPI:
                        return "Nirvana Market Data Provider: Bloomberg SAPI";
                    default:
                        return "Nirvana Market Data Alert";
                }
            }
        }

        public static string MSG_MARKET_DATA_NOT_AVAILABLE
        {
            get
            {
                switch (CachedDataManager.CompanyMarketDataProvider)
                {
                    case MarketDataProvider.FactSet:
                        if(CachedDataManager.CompanyFactSetContractType == FactSetContractType.ChannelPartner)
                            return "Please log into FactSet workstation or FactSet launch (launch.factset.com) on this computer, to use market data.";
                        else
                            return "Please click on Connect Live Feed to connect with FactSet.";
                    case MarketDataProvider.ACTIV:
                        return "Please use valid ACTIV credentials to use market data.";
                    case MarketDataProvider.SAPI:
                        return "Please log into Bloomberg terminal on this computer to use market data.";
                    default:
                        return "Please connect Pricing Server, to use market data.";
                }
            }
        }

        public static string MSG_MARKET_DATA_NOT_AVAILABLE_COMPLIANCE
        {
            get
            {
                switch (CachedDataManager.CompanyMarketDataProvider)
                {
                    case MarketDataProvider.FactSet:
                        if (CachedDataManager.CompanyFactSetContractType == FactSetContractType.ChannelPartner)
                            return "Your FactSet workstation or FactSet launch is not connected on this computer, please back log to use compliance.";
                        else
                            return "Please click on Connect Live Feed to connect with FactSet.";
                    case MarketDataProvider.ACTIV:
                        return "Please use valid ACTIV credentials to use market data.";
                    case MarketDataProvider.SAPI:
                        return "Your Bloomberg terminal is not connected on this computer, please login to use compliance.";
                    default:
                        return "Please connect Pricing Server, to use compliance.";
                }
            }
        }

        public static string MSG_MARKET_DATA_AVAILABLE
        {
            get
            {
                switch (CachedDataManager.CompanyMarketDataProvider)
                {
                    case MarketDataProvider.FactSet:
                        if(CachedDataManager.CompanyFactSetContractType == FactSetContractType.ChannelPartner)
                            return "Your FactSet workstation or FactSet launch (launch.factset.com) is connected. Please reopen dependent modules to use market data";
                        else
                            return "Successfully connected to FactSet. Please reopen dependent modules to use market data.";
                    case MarketDataProvider.ACTIV:
                        return "Please use valid ACTIV credentials to use market data.";
                    case MarketDataProvider.SAPI:
                        return "Your Bloomberg terminal is connected. Please reopen dependent modules to use market data";
                    default:
                        return "Pricing Server is connected. Please reopen dependent modules to use market data";
                }
            }
        }

        public static string MSG_MARKET_DATA_NOT_AVAILABLE_MODULE_CLOSE
        {
            get
            {
                switch (CachedDataManager.CompanyMarketDataProvider)
                {
                    case MarketDataProvider.FactSet:
                        if (CachedDataManager.CompanyFactSetContractType == FactSetContractType.ChannelPartner)
                            return "Your FactSet workstation or FactSet launch (launch.factset.com) is not connected on this computer, please back log. The modules dependent on FactSet market data will be closed in {0}";
                        else
                            return "Please click on Connect Live Feed to connect with FactSet. The modules dependent on FactSet market data will be closed in {0}";
                    case MarketDataProvider.ACTIV:
                        return "Please use valid ACTIV credentials to use market data.";
                    case MarketDataProvider.SAPI:
                        return "Your Bloomberg terminal is not connected on this computer, please back log. The modules dependent on SAPI market data will be closed in {0}";
                    default:
                        return "Please connect Pricing Server, to use market data. The modules dependent on market data will be closed in {0}";
                }
            }
        }
        #endregion
    }
}
