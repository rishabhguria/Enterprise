namespace Prana.BusinessObjects
{
    public class CommissionFields
    {
        private double _commission = double.MinValue;
        public double Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }

        private double _softCommission = double.MinValue;
        public double SoftCommission
        {
            get { return _softCommission; }
            set { _softCommission = value; }
        }

        private double _otherBrokerfees = double.MinValue;
        public double OtherBrokerFees
        {
            get { return _otherBrokerfees; }
            set { _otherBrokerfees = value; }
        }

        private double _clearingBrokerFee = double.MinValue;
        public double ClearingBrokerFee
        {
            get { return _clearingBrokerFee; }
            set { _clearingBrokerFee = value; }
        }

        private double _stampDuty = double.MinValue;
        public double StampDuty
        {
            get { return _stampDuty; }
            set { _stampDuty = value; }
        }

        private double _transactionLevy = double.MinValue;
        public double TransactionLevy
        {
            get { return _transactionLevy; }
            set { _transactionLevy = value; }
        }

        private double _clearingFee = double.MinValue;
        public double ClearingFee
        {
            get { return _clearingFee; }
            set { _clearingFee = value; }
        }

        private double _taxOnCommissions = double.MinValue;
        public double TaxOnCommissions
        {
            get { return _taxOnCommissions; }
            set { _taxOnCommissions = value; }
        }

        private double _miscFees = double.MinValue;
        public double MiscFees
        {
            get { return _miscFees; }
            set { _miscFees = value; }
        }

        private double _secFee = double.MinValue;
        public double SecFee
        {
            get { return _secFee; }
            set { _secFee = value; }
        }

        private double _occFee = double.MinValue;
        public double OccFee
        {
            get { return _occFee; }
            set { _occFee = value; }
        }

        private double _orfFee = double.MinValue;
        public double OrfFee
        {
            get { return _orfFee; }
            set { _orfFee = value; }
        }

        private double _optionPremiumAdjustment;
        public double OptionPremiumAdjustment
        {
            get { return _optionPremiumAdjustment; }
            set { _optionPremiumAdjustment = value; }
        }
    }
}
