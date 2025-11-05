using Prana.BusinessObjects;
using System.Windows;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class TradingRulesBase : BindableBase
    {
        private bool isCheckAllow = true;

        public TradingRulesBase()
        {
        }

        public TradingRulesBase(bool isUICall)
        {
            isCheckAllow = isUICall;
        }

        private bool isReInvestCash = false;
        public bool IsReInvestCash
        {
            get { return isReInvestCash; }
            set
            {
                isReInvestCash = value;
                OnPropertyChanged();
            }
        }

        private bool isSellToRaiseCash = false;
        public bool IsSellToRaiseCash
        {
            get { return isSellToRaiseCash; }
            set
            {
                if (isCheckAllow && IsNegativeCashAllowed && value)
                {
                    ShowAlert();
                }
                else
                {
                    isSellToRaiseCash = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isNoShorting = false;
        public bool IsNoShorting
        {
            get { return isNoShorting; }
            set
            {
                isNoShorting = value;
                OnPropertyChanged();
            }
        }

        private bool isNegativeCashAllowed = false;
        public bool IsNegativeCashAllowed
        {
            get { return isNegativeCashAllowed; }
            set
            {
                if (isCheckAllow && (IsSellToRaiseCash || IsSetCashTarget) && value)
                {
                    ShowAlert();
                }
                else
                {
                    isNegativeCashAllowed = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isSetCashTarget = false;
        public bool IsSetCashTarget
        {
            get { return isSetCashTarget; }
            set
            {
                if (isCheckAllow && IsNegativeCashAllowed && value)
                {
                    ShowAlert();
                }
                else
                {
                    isSetCashTarget = value;
                    if (!isSetCashTarget)    CashTarget = null;
                    OnPropertyChanged();
                }
            }
        }

        private decimal? cashTarget = null;
        public decimal? CashTarget
        {
            get { return cashTarget; }
            set
            {
                cashTarget = value;
                OnPropertyChanged();
            }
        }

        private void ShowAlert()
        {
            MessageBox.Show(RebalancerConstants.MSG_TRADING_RULES_VALIDATION, RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
