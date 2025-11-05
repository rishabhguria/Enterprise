namespace Prana.ServiceGateway.Models
{
    public class ServiceStatusDto
    {
        public string ServiceName { get; set; }

        public string ServiceDisplayName { get; set; }

        public DateTime TimeStamp { get; set; }

        public bool IsLive { get; set; }

        // Each service has its own time interval after which it sends its status
        public int TimeInterval { get; set; } 

    }
}
