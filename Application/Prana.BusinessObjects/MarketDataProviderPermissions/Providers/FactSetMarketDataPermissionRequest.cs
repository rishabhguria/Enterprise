using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class FactSetMarketDataPermissionRequest : MarketDataPermissionRequest
    {
        public string IpAddress { get; set; }

        public string FactSetSerialNumber { get; set; }

        public bool IsFactSetSupportUser { get; set; }
    }
}
