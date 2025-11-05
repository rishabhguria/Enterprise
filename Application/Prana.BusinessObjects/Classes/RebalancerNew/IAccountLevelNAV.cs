namespace Prana.BusinessObjects.Classes.RebalancerNew
{
    public interface IAccountLevelNAV
    {
        int AccountId { get; set; }
        decimal CashInBaseCurrency { get; set; }
        decimal AccrualsInBaseCurrency { get; set; }
        decimal OtherAssetsMarketValue { get; set; }
        decimal SecuritiesMarketValue { get; set; }
        decimal SwapNavAdjustment { get; set; }
    }
}
