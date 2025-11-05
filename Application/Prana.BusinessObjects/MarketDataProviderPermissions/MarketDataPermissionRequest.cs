using System;
using System.Runtime.Serialization;

namespace Prana.BusinessObjects
{
    [KnownType(typeof(FactSetMarketDataPermissionRequest))]
    [KnownType(typeof(ActivMarketDataPermissionRequest))]
    [KnownType(typeof(SapiMarketDataPermissionRequest))]
    [Serializable]
    public class MarketDataPermissionRequest
    {
        public int CompanyUserID { get; set; }
    }
}
