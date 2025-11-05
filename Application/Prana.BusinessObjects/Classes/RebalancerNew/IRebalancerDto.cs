namespace Prana.BusinessObjects.Classes.RebalancerNew
{
    public interface IRebalancerDto
    {
        int AUECID { get; set; }
        int AccountId { get; set; }
        string Asset { get; set; }
        decimal Delta { get; set; }
        decimal FXRate { get; set; }
        decimal LeveragedFactor { get; set; }
        decimal Multiplier { get; set; }
        decimal Price { get; set; }
        decimal Quantity { get; set; }
        string Sector { get; set; }
        Prana.BusinessObjects.AppConstants.PositionType Side { get; set; }
        string Symbol { get; set; }
        bool IsStaleClosingMark { get; set; }
        bool IsStaleFxRate { get; set; }
    }
}
