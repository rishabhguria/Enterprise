namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class TradingRules : TradingRulesBase
    {
        public TradingRules(TradingRulesBase tradingRulesBase, bool isUICall)
            : base(isUICall)
        {
            IsReInvestCash = tradingRulesBase.IsReInvestCash;
            IsNoShorting = tradingRulesBase.IsNoShorting;
            IsSellToRaiseCash = tradingRulesBase.IsSellToRaiseCash;
            IsNegativeCashAllowed = tradingRulesBase.IsNegativeCashAllowed;
            IsSetCashTarget = tradingRulesBase.IsSetCashTarget;
            CashTarget = tradingRulesBase.CashTarget;
        }
        private bool isIncreaseCashPosition = false;
        public bool IsIncreaseCashPosition
        {
            get { return isIncreaseCashPosition; }
            set
            {
                isIncreaseCashPosition = value;
                OnPropertyChanged();
            }
        }
    }
}
