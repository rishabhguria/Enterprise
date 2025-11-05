using Prana.BusinessObjects.Enumerators.RebalancerNew;
using System.Text;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class AccountsModel : KeyValueItem
    {
        private string itemValue;
        public new string ItemValue
        {
            get
            {
                if (type == RebalancerEnums.AccountTypes.Account && IsSpaceRequired)
                    return new StringBuilder("    ").Append(itemValue).ToString();
                return itemValue;
            }
            set
            {
                itemValue = value;
                OnPropertyChanged("ItemValue");
            }
        }

        private RebalancerEnums.AccountTypes type;
        public RebalancerEnums.AccountTypes Type
        {
            get { return type; }
            set { type = value; }
        }

        private bool isSpaceRequired = true;

        public bool IsSpaceRequired
        {
            get { return isSpaceRequired; }
            set
            {
                isSpaceRequired = value;
                OnPropertyChanged("IsSpaceRequired");
            }
        }

    }
}
