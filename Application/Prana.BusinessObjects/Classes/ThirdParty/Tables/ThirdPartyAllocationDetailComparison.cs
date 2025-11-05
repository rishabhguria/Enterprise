namespace Prana.BusinessObjects.Classes.ThirdParty.Tables
{
    public class ThirdPartyAllocationDetailComparison
    {
        private string _item;
        private string _nirvana;
        private string _broker;

        public string Item
        {
            get { return _item; }
            set { _item = value; }
        }

        public string Nirvana
        {
            get { return _nirvana; }
            set { _nirvana = value; }
        }

        public string Broker
        {
            get { return _broker; }
            set { _broker = value; }
        }
    }
}
