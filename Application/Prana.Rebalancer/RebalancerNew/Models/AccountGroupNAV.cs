using Prana.BusinessObjects;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class AccountGroupNAV : BindableBase
    {

        private List<AdjustedAccountLevelNAV> accountsNAV = new List<AdjustedAccountLevelNAV>();

        public List<AdjustedAccountLevelNAV> AccountsNAV
        {
            get { return accountsNAV; }
            set
            {
                accountsNAV = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// CurrentTotalNAV will be used as a denominator to calculate current %
        /// </summary>
        public decimal CurrentTotalNAV
        {
            get
            {
                return accountsNAV.Sum(x => x.CurrentTotalNAV);
            }
        }

        /// <summary>
        /// TargetTotalNAV will be used as a denominator to calculate target %
        /// </summary>
        public decimal TargetTotalNAV
        {
            get
            {
                return accountsNAV.Sum(x => x.TargetTotalNAV);
            }
        }
    }
}
