using Prana.BusinessObjects.AppConstants;
using System;


namespace Prana.Admin.BLL
{
    public class CommissionRuleCriteria
    {
        private CommissionRuleCriteria()
        {

        }

        private int _commissionCriteriaId = int.MinValue;

        public int CommissionCriteriaId
        {
            get { return _commissionCriteriaId; }
            set { _commissionCriteriaId = value; }
        }

        private Guid _ruleId = Guid.Empty;

        public Guid RuleId
        {
            get { return _ruleId; }
            set { _ruleId = value; }
        }

        private double _valueGreaterThan = double.MinValue;

        public double ValueGreaterThan
        {
            get { return _valueGreaterThan; }
            set { _valueGreaterThan = value; }
        }

        private double _valueLessThanOrEqual = double.MinValue;

        public double ValueLessThanOrEqual
        {
            get { return _valueLessThanOrEqual; }
            set { _valueLessThanOrEqual = value; }
        }

        private CalculationBasis _commisionCalculationBasedOn = CalculationBasis.Shares;

        public CalculationBasis CommisionCalculationBasedOn
        {
            get { return _commisionCalculationBasedOn; }
            set { _commisionCalculationBasedOn = value; }
        }

        private double _commissionRate = double.MinValue;

        public double CommissionRate
        {
            get { return _commissionRate; }
            set { _commissionRate = value; }
        }


    }
}
