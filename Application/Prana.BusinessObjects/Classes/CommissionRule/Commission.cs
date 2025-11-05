using Prana.BusinessObjects.AppConstants;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{

    public class Commission
    {
        // commissioncalculationtype are Shares,Notional and Contracts
        private CalculationBasis _ruleAppliedOn;
        public CalculationBasis RuleAppliedOn
        {
            get { return _ruleAppliedOn; }
            set { _ruleAppliedOn = value; }
        }

        private double _commissionRate = 0;
        public double CommissionRate
        {
            get { return _commissionRate; }
            set { _commissionRate = value; }
        }

        private double _minCommission = 0;
        public double MinCommission
        {
            get { return _minCommission; }
            set { _minCommission = value; }
        }

        private double _maxCommission = 0;
        public double MaxCommission
        {
            get { return _maxCommission; }
            set { _maxCommission = value; }
        }

        //get commission rule criteria for a commission rule if exists...
        private List<CommissionRuleCriteria> _commissionRuleCriteriaList;
        public List<CommissionRuleCriteria> CommissionRuleCriteiaList
        {
            get { return _commissionRuleCriteriaList; }
            set { _commissionRuleCriteriaList = value; }

        }

        private bool _isCriteriaApplied = false;
        public bool IsCriteriaApplied
        {
            get { return _isCriteriaApplied; }
            set { _isCriteriaApplied = value; }
        }

        private bool _isRoundOff = false;
        public bool IsRoundOff
        {
            get { return _isRoundOff; }
            set { _isRoundOff = value; }
        }

        private int _roundOffValue = 0;
        public int RoundOffValue
        {
            get { return _roundOffValue; }
            set { _roundOffValue = value; }
        }
    }
}