using Prana.BusinessObjects;
using System;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class StatusAndErrorMessageModel : BindableBase
    {
        private string _statusMessage = String.Empty;

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private string _errorMessage = String.Empty;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }
    }
}
