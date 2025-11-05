namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AUECCommissionRules.
    /// </summary>
    /// 
    public class AUECCommissionRule
    {
        #region Private Members
        private int _ruleID = int.MinValue;
        private int _auecID = int.MinValue;
        private string _ruleName = string.Empty;
        private int _applyruleID = int.MinValue;
        private string _ruleDescription = string.Empty;
        private int _calculationID = int.MinValue;
        private int _currencyID = int.MinValue;
        private int _commissionRateID = int.MinValue;
        private double _commission = double.MinValue;
        private int _applyCriteria = int.MinValue;
        private int _applyClreaingFee = int.MinValue;


        #endregion

        public AUECCommissionRule()
        {

        }
        public AUECCommissionRule(int ruleID, string ruleName)
        {
            _ruleID = ruleID;
            _ruleName = ruleName;
        }

        #region Public Properties
        public int RuleID
        {
            get { return _ruleID; }
            set { _ruleID = value; }
        }
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }
        public string RuleName
        {
            get { return _ruleName; }
            set { _ruleName = value; }
        }
        public int ApplyRuleID
        {
            get { return _applyruleID; }
            set { _applyruleID = value; }
        }
        public string RuleDescription
        {
            get { return _ruleDescription; }
            set { _ruleDescription = value; }
        }
        public int CalculationID
        {
            get { return _calculationID; }
            set { _calculationID = value; }
        }
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }
        public int CommissionRateID
        {
            get { return _commissionRateID; }
            set { _commissionRateID = value; }
        }
        public double Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }
        public int ApplyCriteria
        {
            get { return _applyCriteria; }
            set { _applyCriteria = value; }
        }
        public int ApplyClearingFee
        {
            get { return _applyClreaingFee; }
            set { _applyClreaingFee = value; }
        }

        #endregion

    }

}
