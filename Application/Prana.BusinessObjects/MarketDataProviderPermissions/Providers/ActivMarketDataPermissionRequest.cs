using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ActivMarketDataPermissionRequest : MarketDataPermissionRequest
    {
        public string ActivUsername { get; set; }

        public string ActivPassword { get; set; }
    }
}
