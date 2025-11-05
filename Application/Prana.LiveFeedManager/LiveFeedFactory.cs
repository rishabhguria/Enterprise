using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using System;

namespace Prana.LiveFeedManager
{
    internal class LiveFeedFactory
    {
        private static LiveFeedFactory _liveFeedFactoryInstance;

        static LiveFeedFactory()
        {
            _liveFeedFactoryInstance = new LiveFeedFactory();
        }

        public static LiveFeedFactory GetInstance()
        {
            return _liveFeedFactoryInstance;
        }

        public ILiveFeedAdapter LiveFeedProviderInstance()
        {
            ILiveFeedAdapter liveFeedInstance = null;
            try
            {
                switch (CachedDataManager.CompanyMarketDataProvider)
                {
                    case MarketDataProvider.Esignal:
                        liveFeedInstance = ESignalAdapter.ESignalAdapter.GetInstance();
                        break;
                    case MarketDataProvider.SAPI:
                        liveFeedInstance = Prana.SAPIAdapter.SAPIManager.GetInstance();
                        break;

                    case MarketDataProvider.API:
                        liveFeedInstance = PricingApiServiceInstance;
                        break;
                    case MarketDataProvider.None:
                        liveFeedInstance = new NonePricingManager();
                        break;
                    case MarketDataProvider.Google:
                    case MarketDataProvider.Yahoo:
                    case MarketDataProvider.BlpDLWS:
                        break;
                    case MarketDataProvider.FactSet:
                        liveFeedInstance = Prana.FactSetAdapter.FactSetManager.GetInstance();
                        break;
                    case MarketDataProvider.ACTIV:
                        liveFeedInstance = Prana.ActivAdapter.ActivManager.GetInstance();
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return liveFeedInstance;
        }

        public ILiveFeedAdapter SecondaryProviderInstance()
        {
            ILiveFeedAdapter liveFeedInstance = null;
            try
            {
                if (CachedDataManager.SecondaryCompanyMarketDataProvider == SecondaryMarketDataProvider.BloombergDLWS)
                {
                    liveFeedInstance = Prana.BloombergAdapter.BloombergManager.GetInstance();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return liveFeedInstance;
        }
        /// <summary>
        /// Pricing Api Service Instance
        /// </summary>
        public ILiveFeedAdapter PricingApiServiceInstance { get; set; }
    }
}
