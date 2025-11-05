using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.CommonDataCache;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class AdjustedAccountLevelNAV : BindableBase, IAccountLevelNAV
    {
        #region IAccouneLevelNAV properties
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
        /// Total current market value of assets Equity, Private equity and equity swap.
        /// </summary>
        public decimal SecuritiesMarketValue
        {
            get { return securitiesMarketValue; }
            set
            {
                securitiesMarketValue = value;
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
            set { accrualsInBaseCurrency = value; }
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

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjustedAccountLevelNAV"/> class.
        /// </summary>
        /// <param name="accountLevelNAV">The rebal dto.</param>
        public AdjustedAccountLevelNAV(AccountLevelNAV accountLevelNAV)
        {
            MarketValueForCalculation = TargetSecuritiesMarketValue = CurrentSecuritiesMarketValue = accountLevelNAV.SecuritiesMarketValue;
            OtherAssetsMarketValue = accountLevelNAV.OtherAssetsMarketValue;
            CashInBaseCurrency = accountLevelNAV.CashInBaseCurrency;
            SwapNavAdjustment = accountLevelNAV.SwapNavAdjustment;
            AccrualsInBaseCurrency = accountLevelNAV.AccrualsInBaseCurrency;
            AccountId = accountLevelNAV.AccountId;
        }

        private decimal unRealizedPnlOfSwaps;

        public decimal UnRealizedPnlOfSwaps
        {
            get { return unRealizedPnlOfSwaps; }
            set
            {
                unRealizedPnlOfSwaps = value;
            }
        }

        private decimal cashFlow;
        /// <summary>
        /// While performing rebal operation free mid calculation cash will be stored to this field.
        /// </summary>
        public decimal CashFlow
        {
            get { return cashFlow; }
            set
            {
                cashFlow = value;
                OnPropertyChanged();
            }
        }

        private decimal customCashFlow;
        public decimal CustomCashFlow
        {
            get { return customCashFlow; }
            set
            {
                customCashFlow = value;
                OnPropertyChanged();
            }
        }

        private bool isCustomCashFlow;
        public bool IsCustomCashFlow
        {
            get { return isCustomCashFlow; }
            set
            {
                isCustomCashFlow = value;
                OnPropertyChanged();
            }
        }

        private decimal currentSecuritiesMarketValue;
        /// <summary>
        /// Total current market value of assets Equity, Private equity and equity swap and nav impacting components.
        /// </summary>
        public decimal CurrentSecuritiesMarketValue
        {
            get { return currentSecuritiesMarketValue; }
            set
            {
                currentSecuritiesMarketValue = value;
            }
        }

        private decimal targetSecuritiesMarketValue;
        /// <summary>
        /// Total target market value of assets Equity, Private equity and equity swap and nav impacting components
        /// </summary>
        public decimal TargetSecuritiesMarketValue
        {
            get { return targetSecuritiesMarketValue; }
            set
            {
                targetSecuritiesMarketValue = value;
            }
        }

        private decimal marketValueForCalculation;
        /// <summary>
        /// Market value of all unlocked securities used for calculations. 
        /// </summary>
        public decimal MarketValueForCalculation
        {
            get { return marketValueForCalculation; }
            set
            {
                marketValueForCalculation = value;
            }
        }

        public string AccountName
        {
            get { return CachedDataManager.GetInstance.GetAccount(AccountId); }
        }

        private bool isIncludeOtherAssetsNAV = true;

        public bool IsIncludeOtherAssetsNAV
        {
            get { return isIncludeOtherAssetsNAV; }
            set
            {
                isIncludeOtherAssetsNAV = value;
                OnPropertyChanged();
            }
        }

        private bool isIncludeCashInBaseCurrency = true;

        public bool IsIncludeCashInBaseCurrency
        {
            get { return isIncludeCashInBaseCurrency; }
            set
            {
                isIncludeCashInBaseCurrency = value;
                OnPropertyChanged();
            }
        }

        private bool isIncludeAccrualsInBaseCurrency = true;

        public bool IsIncludeAccrualsInBaseCurrency
        {
            get { return isIncludeAccrualsInBaseCurrency; }
            set
            {
                isIncludeAccrualsInBaseCurrency = value;
                OnPropertyChanged();
            }
        }

        private bool isIncludeSwapNavAdjustement = true;

        public bool IsIncludeSwapNavAdjustement
        {
            get { return isIncludeSwapNavAdjustement; }
            set
            {
                isIncludeSwapNavAdjustement = value;
                OnPropertyChanged();
            }
        }

        private bool isIncludeUnrealizedPNLOfSwaps = true;

        public bool IsIncludeUnrealizedPNLOfSwaps
        {
            get { return isIncludeUnrealizedPNLOfSwaps; }
            set
            {
                isIncludeUnrealizedPNLOfSwaps = value;
                OnPropertyChanged();
            }
        }

        private bool _isIncludeUserAdjustedNAV;

        public bool IsIncludeUserAdjustedNAV
        {
            get { return _isIncludeUserAdjustedNAV; }
            set
            {
                _isIncludeUserAdjustedNAV = value;
                OnPropertyChanged();
            }
        }

        private decimal userAdjustedNAV;

        public decimal UserAdjustedNAV
        {
            get { return userAdjustedNAV; }
            set
            {
                userAdjustedNAV = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// SystemGeneratedNAV
        /// </summary>
        public decimal SystemGeneratedNAV
        {
            get
            {
                decimal totalMV = 0;
                totalMV += SecuritiesMarketValue;
                totalMV += OtherAssetsMarketValue;
                totalMV += CashInBaseCurrency;
                totalMV += AccrualsInBaseCurrency;
                totalMV += SwapNavAdjustment;
                totalMV += UnRealizedPnlOfSwaps;
                return totalMV;
            }
        }

        /// <summary>
        /// CurrentTotalNAV will be used as a denominator to calculate current %
        /// </summary>
        public decimal CurrentTotalNAV
        {
            get
            {
                return CurrentSecuritiesMarketValue;
            }
        }

        /// <summary>
        /// TargetTotalNAV will be used as a denominator to calculate target %
        /// </summary>
        public decimal TargetTotalNAV
        {
            get
            {
                return MarketValueForCalculation + CashFlow;
            }
        }

        private decimal cashFlowNeeded;
        /// <summary>
        /// This field will be used to calculate calculate cash flow needed for the Rebalance across securities action prior to Run Rebalance.
        /// </summary>
        public decimal CashFlowNeeded
        {
            get { return cashFlowNeeded; }
            set
            {
                cashFlowNeeded = value;
            }
        }
    }
}
