using System;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AllCommissionRule.
    /// </summary>
    public class AllCommissionRule
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
        private float _commission = 0;
        private int _applyCriteria = int.MinValue;
        private int _commCriteriaID = int.MinValue;
        private int _ruleID_FK = int.MinValue;
        private int _commissionCalculationID = int.MinValue;
        private float _minCommissionRate = 0;
        private int _commRulecriteriaID = int.MinValue;
        private int _commCriteriaID_FK = int.MinValue;
        private int _operatorID1 = int.MinValue;
        private int _value1 = 0;
        private int _commissionRateID1 = int.MinValue;
        private float _commissionRate1 = 2;
        private int _operatorID2 = int.MinValue;
        private int _value2 = 0;
        private int _commissionRateID2 = int.MinValue;
        private float _commissionRate2 = 2;
        private int _operatorID3 = int.MinValue;
        private int _value3 = 0;
        private int _commissionRateID3 = int.MinValue;
        private float _commissionRate3 = 2;

        private string _auecName = string.Empty;

        private string _commissionRateType = string.Empty;
        private string _operatorName = string.Empty;

        private string _calculationType = string.Empty;
        private string _currencySymbol = string.Empty;

        private Int64 _valueFrom = 0;
        private Int64 _valueTo = 0;

        #endregion
        public AllCommissionRule()
        {

        }
        #region Public Properties

        //updated values

        public Int64 ValueFrom
        {
            get { return _valueFrom; }
            set { _valueFrom = value; }
        }

        public Int64 ValueTo
        {
            get { return _valueTo; }
            set { _valueTo = value; }
        }

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
        public float Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }
        public int ApplyCriteria
        {
            get { return _applyCriteria; }
            set { _applyCriteria = value; }
        }
        public int CommissionCriteriaID
        {
            get { return _commCriteriaID; }
            set { _commCriteriaID = value; }
        }

        public int RuleID_FK
        {
            get { return _ruleID_FK; }
            set { _ruleID_FK = value; }
        }
        public int CommissionCalculationID_FK
        {
            get { return _commissionCalculationID; }
            set { _commissionCalculationID = value; }
        }

        public float MinimumCommissionRate
        {
            get { return _minCommissionRate; }
            set { _minCommissionRate = value; }
        }
        public int CommissionRuleCriteriaID
        {
            get { return _commRulecriteriaID; }
            set { _commRulecriteriaID = value; }
        }
        public int CommissionCriteriaID_FK
        {
            get { return _commCriteriaID_FK; }
            set { _commCriteriaID_FK = value; }
        }

        public int OperatorID_FK1
        {
            get { return _operatorID1; }
            set { _operatorID1 = value; }
        }
        public int Value1
        {
            get { return _value1; }
            set { _value1 = value; }
        }
        public int CommissionRateID_FK1
        {
            get { return _commissionRateID1; }
            set { _commissionRateID1 = value; }
        }
        public float CommisionRate1
        {
            get { return _commissionRate1; }
            set { _commissionRate1 = value; }
        }
        public int OperatorID_FK2
        {
            get { return _operatorID2; }
            set { _operatorID2 = value; }
        }
        public int Value2
        {
            get { return _value2; }
            set { _value2 = value; }
        }
        public int CommissionRateID_FK2
        {
            get { return _commissionRateID2; }
            set { _commissionRateID2 = value; }
        }
        public float CommisionRate2
        {
            get { return _commissionRate2; }
            set { _commissionRate2 = value; }
        }
        public int OperatorID_FK3
        {
            get { return _operatorID3; }
            set { _operatorID3 = value; }
        }
        public int Value3
        {
            get { return _value3; }
            set { _value3 = value; }
        }
        public int CommissionRateID_FK3
        {
            get { return _commissionRateID3; }
            set { _commissionRateID3 = value; }
        }
        public float CommisionRate3
        {
            get { return _commissionRate3; }
            set { _commissionRate3 = value; }
        }

        public string AUECName
        {
            get { return _auecName; }
            set { _auecName = value; }
        }

        public string CommissionRateType
        {
            get { return _commissionRateType; }
            set { _commissionRateType = value; }
        }
        public string OperatorName
        {
            get { return _operatorName; }
            set { _operatorName = value; }
        }

        public string CaluculationType
        {
            get { return _calculationType; }
            set { _calculationType = value; }
        }

        public string CurrencySymbol
        {
            get { return _currencySymbol; }
            set { _currencySymbol = value; }
        }
        #endregion
    }
}
