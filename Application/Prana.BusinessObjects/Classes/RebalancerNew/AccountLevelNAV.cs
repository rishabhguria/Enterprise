namespace Prana.BusinessObjects.Classes.RebalancerNew
{
    public class AccountLevelNAV : BindableBase, IAccountLevelNAV
    {
        private int accountId;

        public int AccountId
        {
            get { return accountId; }
            set
            {
                accountId = value;
            }
        }

        private decimal securitiesMarketValue;
        /// <summary>
        /// Total market value of assets Equity, Private equity and equity swap.
        /// </summary>
        public decimal SecuritiesMarketValue
        {
            get { return securitiesMarketValue; }
            set
            {
                securitiesMarketValue = value;
            }
        }

        private decimal cashInBaseCurrency;

        public decimal CashInBaseCurrency
        {
            get { return cashInBaseCurrency; }
            set
            {
                cashInBaseCurrency = value;
            }
        }

        private decimal accrualsInBaseCurrency;

        public decimal AccrualsInBaseCurrency
        {
            get { return accrualsInBaseCurrency; }
            set
            {
                accrualsInBaseCurrency = value;
            }
        }

        private decimal otherAssetsMarketValue;

        public decimal OtherAssetsMarketValue
        {
            get { return otherAssetsMarketValue; }
            set
            {
                otherAssetsMarketValue = value;
            }
        }

        private decimal swapNavAdjustment;

        public decimal SwapNavAdjustment
        {
            get { return swapNavAdjustment; }
            set
            {
                swapNavAdjustment = value;
            }
        }
    }
}
