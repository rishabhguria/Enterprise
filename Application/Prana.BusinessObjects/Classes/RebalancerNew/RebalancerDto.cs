namespace Prana.BusinessObjects.Classes.RebalancerNew
{
    /// <summary>
    /// Contains fields required by Rebalancer UI for calculations
    /// </summary>
    public class RebalancerDto : BindableBase, IRebalancerDto
    {
        public int AUECID { get; set; }
        public int AccountId { get; set; }
        public string Asset { get; set; }
        public decimal Delta { get; set; }
        public decimal FXRate { get; set; }
        public decimal LeveragedFactor { get; set; }
        public decimal RoundLot { get; set; }
        public decimal Multiplier { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public string Sector { get; set; }
        public AppConstants.PositionType Side { get; set; }
        public string Symbol { get; set; }
        public string BloombergSymbol { get; set; }
        public string FactSetSymbol { get; set; }
        public string ActivSymbol { get; set; }
        public bool IsStaleClosingMark { get; set; }
        public bool IsStaleFxRate { get; set; }
        public string BloombergSymbolWithExchangeCode { get; set; }
    }
}
