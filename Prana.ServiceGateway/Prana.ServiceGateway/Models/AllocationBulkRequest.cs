namespace Prana.ServiceGateway.Models
{
    public class AllocationBulkRequest
    {
        public List<int> PreferenceIds { get; set; }
        public string TradingTicketId { get; set; }
    }
}
