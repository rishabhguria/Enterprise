using Prana.BusinessObjects;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class TradeListSummaryModel : BindableBase
    {
        public long TotalSymbols { get; set; }

        public decimal TotalQuantity { get; set; }
        
        public decimal TotalBuySellValue { get; set; }
    }
}
