
using Prana.BusinessObjects;
namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class KeyValueItem : BindableBase
    {
        private int key;

        public int Key
        {
            get { return key; }
            set
            {
                key = value;
                OnPropertyChanged("Key");
            }
        }

        private string itemValue;

        public string ItemValue
        {
            get { return itemValue; }
            set
            {
                itemValue = value;
                OnPropertyChanged("ItemValue");
            }
        }


    }
}
