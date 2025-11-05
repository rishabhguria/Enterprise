namespace Prana.PM.BLL
{
    public class TradeTransaction
    {
        private string _side;

        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }

        private int _execQty;

        public int ExecQty
        {
            get { return _execQty; }
            set { _execQty = value; }
        }

        private float _avgPrice;

        public float AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }

        private string _cv;

        public string CV
        {
            get { return _cv; }
            set { _cv = value; }
        }

        private string _account;

        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        private double _notional;

        public double Notional
        {
            get { return _notional; }
            set { _notional = value; }
        }

        private float _commissions;

        public float Commissions
        {
            get { return _commissions; }
            set { _commissions = value; }
        }

        private float _fees;

        public float Fees
        {
            get { return _fees; }
            set { _fees = value; }
        }

        private double _netAmount;

        public double NetAmount
        {
            get { return _netAmount; }
            set { _netAmount = value; }
        }

    }
}
