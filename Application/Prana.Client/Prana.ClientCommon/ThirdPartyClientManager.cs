using Prana.Interfaces;
using Prana.WCFConnectionMgr;

namespace Prana.ClientCommon
{
    public class ThirdPartyClientManager
    {
        private ProxyBase<IThirdPartyService> _thirdPartyService;
        private static ThirdPartyClientManager _singletonInstance;

        /// <summary>
        /// Gets the InnerChannel of IThirdPartyService proxy
        /// </summary>
        public static IThirdPartyService ServiceInnerChannel
        {
            get
            {
                if (_singletonInstance == null)
                {
                    _singletonInstance = new ThirdPartyClientManager();
                }
                if (_singletonInstance._thirdPartyService == null)
                {
                    _singletonInstance._thirdPartyService = new ProxyBase<IThirdPartyService>("TradeThirdPartyServiceEndpointAddress");
                }
                return _singletonInstance._thirdPartyService.InnerChannel;
            }
        }

        /// <summary>
        /// Disposes the proxy
        /// </summary>
        public static void Dispose()
        {
            if (_singletonInstance._thirdPartyService != null)
            {
                _singletonInstance._thirdPartyService.Dispose();
            }
            _singletonInstance._thirdPartyService = null;
        }
    }
}
