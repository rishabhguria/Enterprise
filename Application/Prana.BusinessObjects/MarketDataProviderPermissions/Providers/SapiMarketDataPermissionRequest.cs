using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class SapiMarketDataPermissionRequest : MarketDataPermissionRequest
    {
        public string IpAddress { get; set; }
        public string SapiUsername { get; set; }
    }
}
