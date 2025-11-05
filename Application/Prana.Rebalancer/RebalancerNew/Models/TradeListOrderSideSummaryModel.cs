using Prana.BusinessObjects;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class TradeListOrderSideSummaryModel : BindableBase
    {
        public string Side { get; set; }

        public decimal Quantity { get; set; }

        public decimal BuySellValue { get; set; }
    }
}
