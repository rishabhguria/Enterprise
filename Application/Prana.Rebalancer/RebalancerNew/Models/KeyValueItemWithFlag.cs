namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class KeyValueItemWithFlag : KeyValueItem
    {
        private bool flag;
        public bool Flag
        {
            get { return flag; }
            set { flag = value; OnPropertyChanged("Flag"); }
        }
    }
}
