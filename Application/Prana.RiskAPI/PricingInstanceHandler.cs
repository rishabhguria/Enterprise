using Prana.Interfaces;

namespace Prana.RiskServer
{
    public class PricingInstanceHandler
    {
        private static object _initializeLocker = new object();
        private static PricingInstanceHandler _pricingInstanceHandler = null;
        public static PricingInstanceHandler GetInstance
        {
            get
            {
                if (_pricingInstanceHandler == null)
                {
                    lock (_initializeLocker)
                    {
                        if (_pricingInstanceHandler == null)
                        {
                            _pricingInstanceHandler = new PricingInstanceHandler();
                        }
                    }
                }
                return _pricingInstanceHandler;
            }
        }

        private IPricingService _pricingService = null;
        public IPricingService PricingService
        {
            get { return _pricingService; }
            set { _pricingService = value; }
        }
    }
}
