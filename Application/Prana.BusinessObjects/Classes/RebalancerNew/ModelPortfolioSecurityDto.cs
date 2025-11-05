namespace Prana.BusinessObjects.Classes.RebalancerNew
{
    public class ModelPortfolioSecurityDto
    {
        public string Asset { get; set; }
        public decimal Delta { get; set; }
        public decimal FXRate { get; set; }
        public decimal LeveragedFactor { get; set; }
        public decimal Multiplier { get; set; }
        public decimal RoundLot { get; set; }
        public decimal Price { get; set; }
        public string Sector { get; set; }
        public string Symbol { get; set; }
        public string BloombergSymbol { get; set; }
        public string FactSetSymbol { get; set; }
        public string ActivSymbol { get; set; }
        public decimal TargetPercentage { get; set; }
        public int AUECID { get; set; }
        public int ModelType { get; set; }
        public string BloombergSymbolWithExchangeCode { get; set; }
        public decimal TolerancePercentage { get; set; }
    }
}
