namespace Prana.TradeManager
{
    public class TradingRuleViolatedParameter
    {
        private string _Symbol = string.Empty;

        public string Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }

        private string _MasterFund = string.Empty;

        public string MasterFund
        {
            get { return _MasterFund; }
            set { _MasterFund = value; }
        }

        private string _accountname = string.Empty;

        public string AccountName
        {
            get { return _accountname; }
            set { _accountname = value; }
        }

        private double _TradeQuantity = 0.0;
        public double TradeQuantity
        {
            get { return _TradeQuantity; }
            set { _TradeQuantity = value; }
        }
        private double _CurrentPosition;
        public double CurrentPosition
        {
            get { return _CurrentPosition; }
            set { _CurrentPosition = value; }
        }
        public double? NavPercent
        { get; set; }

        public double? SharesOutstandingPercent
        { get; set; }

    }
}
