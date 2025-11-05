using Newtonsoft.Json;
using Prana.BusinessObjects;
using System.ComponentModel.DataAnnotations;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class PortfolioDto : BindableBase
    {
        #region Symbol
        private string _symbol;

        [Required(ErrorMessage = "Symbol is Required")]
        [JsonProperty(PropertyName = "Symbol")]
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
                if (!string.IsNullOrWhiteSpace(_symbol))
                {
                    _symbol = _symbol.ToUpper();
                }
                OnPropertyChanged("Symbol");
            }
        }
        #endregion

        #region Target Percentage
        private decimal _targetPercentage;

        [Required(ErrorMessage = "Target% is required")]
        [JsonProperty(PropertyName = "Target")]
        public decimal TargetPercentage
        {
            get { return _targetPercentage; }
            set
            {
                _targetPercentage = value;
                OnPropertyChanged("TargetPercentage");
            }
        }

        #endregion

        #region Is Symbol Valid
        private bool _isSymbolValid;

        [JsonIgnore]
        public bool IsSymbolValid
        {
            get { return _isSymbolValid; }
            set
            {
                _isSymbolValid = value;
                OnPropertyChanged("IsSymbolValid");
            }
        }
        #endregion

        #region Tolerance Percentage
        private decimal _tolerancePercentage = 0;

        [Required(ErrorMessage = "TolerancePercentage is required")]
        [JsonProperty(PropertyName = "TolerancePercentage")]
        public decimal TolerancePercentage
        {
            get { return _tolerancePercentage; }
            set
            {
                _tolerancePercentage = value;
                OnPropertyChanged("TolerancePercentage");

                _toleranceBPS = (int)(_tolerancePercentage * 100);
                OnPropertyChanged("ToleranceBPS");
            }
        }

        #endregion

        #region ToleranceBPS
        private int _toleranceBPS = 0;

        [Required(ErrorMessage = "ToleranceBPS is required")]
        [JsonProperty(PropertyName = "ToleranceBPS")]
        public int ToleranceBPS
        {
            get { return _toleranceBPS; }
            set
            {
                _toleranceBPS = value;
                OnPropertyChanged("ToleranceBPS");

                _tolerancePercentage = (decimal)_toleranceBPS / 100;
                OnPropertyChanged("TolerancePercentage");
            }
        }

        #endregion

        #region Chcked
        private bool _isChecked;

        [Required(ErrorMessage = "IsChecked is required")]
        [JsonProperty(PropertyName = "IsChecked")]
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        #endregion    
    }
}
