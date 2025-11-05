
namespace Prana.BusinessObjects
{
    public class MarketDataPermissionRequest
    {
        public int CompanyUserID { get; set; }

        public string IpAddress { get; set; }

        public string FactSetSerialNumber { get; set; }

        public bool IsFactSetSupportUser { get; set; }
    }
}
