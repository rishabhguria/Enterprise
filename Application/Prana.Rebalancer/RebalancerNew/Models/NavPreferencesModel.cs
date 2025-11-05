using Newtonsoft.Json;
using Prana.BusinessObjects;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class NavPreferencesModel : BindableBase
    {
        private int _accountId;
        [JsonIgnore]
        public int AccountId
        {
            get { return _accountId; }
            set
            {
                _accountId = value;
                OnPropertyChanged();
            }
        }

        private bool _isIncludeOtherAssetsMarketValue;
        public bool IsIncludeOtherAssetsMarketValue
        {
            get { return _isIncludeOtherAssetsMarketValue; }
            set
            {
                _isIncludeOtherAssetsMarketValue = value;
                OnPropertyChanged();
            }
        }

        private bool _isIncludeCash;
        public bool IsIncludeCash
        {
            get { return _isIncludeCash; }
            set
            {
                _isIncludeCash = value;
                OnPropertyChanged();
            }
        }

        private bool _isIncludeAccruals;
        public bool IsIncludeAccruals
        {
            get { return _isIncludeAccruals; }
            set
            {
                _isIncludeAccruals = value;
                OnPropertyChanged();
            }
        }

        private bool _isIncludeSwapNavAdjustment;
        public bool IsIncludeSwapNavAdjustment
        {
            get { return _isIncludeSwapNavAdjustment; }
            set
            {
                _isIncludeSwapNavAdjustment = value;
                OnPropertyChanged();
            }
        }

        private bool _isIncludeUnrealizedPnlOfSwaps;
        public bool IsIncludeUnrealizedPnlOfSwaps
        {
            get { return _isIncludeUnrealizedPnlOfSwaps; }
            set
            {
                _isIncludeUnrealizedPnlOfSwaps = value;
                OnPropertyChanged();
            }
        }

        private bool _isSelected;
        [JsonIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

    }
}
