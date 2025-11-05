namespace Prana.Admin.BLL
{
    public class CommissionRuleClearingFee
    {

        public CommissionRuleClearingFee()
        {

        }

        private int _clearingFeeId;

        public int ClearingFeeId
        {
            get { return _clearingFeeId; }
            set { _clearingFeeId = value; }
        }

        private int _ruleId;

        public int RuleId
        {
            get { return _ruleId; }
            set { _ruleId = value; }
        }

        private int _calculationId;

        public int CalculationId
        {
            get { return _calculationId; }
            set { _calculationId = value; }
        }
        private string _calculationType = string.Empty;

        public string CalculationType
        {
            get { return _calculationType; }
            set { _calculationType = value; }
        }

        private int _currencyId;

        public int CurrencyId
        {
            get { return _currencyId; }
            set { _currencyId = value; }
        }
        private double _commissionRate;

        public double CommissionRate
        {
            get { return _commissionRate; }
            set { _commissionRate = value; }
        }

    }
}
