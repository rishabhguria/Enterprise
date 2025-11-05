using Prana.BusinessObjects.AppConstants;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class ClearingFee
    {
        public ClearingFee()
        {
        }
        // clearingFeecalculationtype are Shares,Notional and Contracts
        private CalculationBasis _ruleAppliedOn;
        public CalculationBasis RuleAppliedOn
        {
            get { return _ruleAppliedOn; }
            set { _ruleAppliedOn = value; }
        }

        private double _clearingFeeRate = 0;
        public double ClearingFeeRate
        {
            get { return _clearingFeeRate; }
            set { _clearingFeeRate = value; }
        }

        private double _minclearingFee = 0;
        public double MinClearingFee
        {
            get { return _minclearingFee; }
            set { _minclearingFee = value; }
        }

        //get clearingFee rule criteria for a clearingFee rule if exists...
        private List<ClearingFeeCriteria> _clearingFeeRuleCriteriaList;
        public List<ClearingFeeCriteria> ClearingFeeRuleCriteiaList
        {
            get { return _clearingFeeRuleCriteriaList; }
            set { _clearingFeeRuleCriteriaList = value; }

        }

        private bool _isCriteriaApplied = false;
        public bool IsCriteriaApplied
        {
            get { return _isCriteriaApplied; }
            set { _isCriteriaApplied = value; }
        }
    }
    public class ClearingFeeCriteria
    {
        public ClearingFeeCriteria()
        {

        }

        private int _clearingFeeCriteriaId = int.MinValue;
        public int ClearingFeeCriteriaId
        {
            get { return _clearingFeeCriteriaId; }
            set { _clearingFeeCriteriaId = value; }
        }

        private double _valueGreaterThan = double.MinValue;
        public double ValueGreaterThan
        {
            get { return _valueGreaterThan; }
            set { _valueGreaterThan = value; }
        }

        private double _valueLessThanOrEqual = double.MaxValue;
        public double ValueLessThanOrEqual
        {
            get { return _valueLessThanOrEqual; }
            set { _valueLessThanOrEqual = value; }
        }

        private string _clearingFeeCriteriaUnit;
        /// <summary>
        /// Whether it is per share, per contract, basis point
        /// </summary>
        public string ClearingFeeCriteriaUnit
        {
            get { return _clearingFeeCriteriaUnit; }
            set { _clearingFeeCriteriaUnit = value; }
        }

        private double _clearingFeeRate = double.MinValue;
        public double ClearingFeeRate
        {
            get { return _clearingFeeRate; }
            set { _clearingFeeRate = value; }
        }

        private ClearingFeeType _clearingFeeType;
        public ClearingFeeType ClearingFeeType
        {
            get { return _clearingFeeType; }
            set { _clearingFeeType = value; }
        }
    }
}
