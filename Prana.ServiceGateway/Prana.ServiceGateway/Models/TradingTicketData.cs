namespace Prana.ServiceGateway.Models
{
    public class TradingTicketData
    {
        public string UserAllocationData { get; set; }
        public string TradingTicketUIPrefs { get; set; }
        public string UserTIF { get; set; }
        public string UserOrderSide { get; set; }
        public string UserOrderType { get; set; }
        public string UserBroker { get; set; }
        public string UserBrokerStatus { get; set; }
        //   public string CompanyTradingPreferences { get; set; }
    }

    public class TradingTicketInfo
    {
        public List<dynamic> StageTradeHandler { get; set; }
        public string TradingTicketId { get; set; }
    }
}
