using Prana.BusinessObjects;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class AccountGroupDropdownPrefModel : BindableBase
    {
        private bool isAccountIncluded;

        public bool IsAccountIncluded
        {
            get { return isAccountIncluded; }
            set
            {
                isAccountIncluded = value;
                OnPropertyChanged();
            }
        }

        private bool isMasterFundIncluded;

        public bool IsMasterFundIncluded
        {
            get { return isMasterFundIncluded; }
            set
            {
                isMasterFundIncluded = value;
                OnPropertyChanged();
            }
        }

        private bool isCustomGroupIncluded;

        public bool IsCustomGroupIncluded
        {
            get { return isCustomGroupIncluded; }
            set
            {
                isCustomGroupIncluded = value;
                OnPropertyChanged();
            }
        }

    }
}
