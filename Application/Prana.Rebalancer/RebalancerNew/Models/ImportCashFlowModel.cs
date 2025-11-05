namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class ImportCashFlowModel
    {
        public bool IsValid { get; set; }

        public string AccountName { get; set; }

        public decimal Cash { get; set; }

        public string Comment { get; set; }
    }
}
