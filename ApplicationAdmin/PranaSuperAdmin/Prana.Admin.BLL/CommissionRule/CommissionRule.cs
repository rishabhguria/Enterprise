using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System;
using System.Collections.Generic;

namespace Prana.Admin.BLL
{
    public class CommissionRule
    {

        public CommissionRule()
        {
        }

        #region Public properties
        private Guid _ruleId = Guid.Empty;

        public Guid RuleID
        {
            get { return _ruleId; }
            set { _ruleId = value; }
        }

        // asset Category can be Equity or EquityOption or Future or FutureOption etc....
        private List<AssetCategory> _assetIdList;

        public List<AssetCategory> AssetIdList
        {
            get { return _assetIdList; }
            set { _assetIdList = value; }
        }

        private string _ruleName = string.Empty;

        public string RuleName
        {
            get { return _ruleName; }
            set { _ruleName = value; }
        }

        private string _ruleDescription = string.Empty;

        public string RuleDescription
        {
            get { return _ruleDescription; }
            set { _ruleDescription = value; }
        }

        // TradeType can be Single Trade or Basket Trade or Both option
        private TradeType _applyRuleForTrade;

        public TradeType ApplyRuleForTrade
        {
            get { return _applyRuleForTrade; }
            set { _applyRuleForTrade = value; }
        }

        // commissioncalculationtype are Shares,Notional and Contracts
        private CalculationBasis _ruleAppliedOn;

        public CalculationBasis RuleAppliedOn
        {
            get { return _ruleAppliedOn; }
            set { _ruleAppliedOn = value; }
        }

        private double _commissionRate = double.MinValue;

        public double CommissionRate
        {
            get { return _commissionRate; }
            set { _commissionRate = value; }
        }

        private double _minCommission = double.MinValue;

        public double MinCommission
        {
            get { return _minCommission; }
            set { _minCommission = value; }
        }

        // commissioncalculationtype is Shares,Notional and Contracts, it is for Clearing fee
        private CalculationBasis _clearingFeeCalculationBasedOn;

        public CalculationBasis ClearingFeeCalculationBasedOn
        {
            get { return _clearingFeeCalculationBasedOn; }
            set { _clearingFeeCalculationBasedOn = value; }
        }

        private double _clearingFeeRate;

        public double ClearingFeeRate
        {
            get { return _clearingFeeRate; }
            set { _clearingFeeRate = value; }
        }

        private double _minClearingFee;

        public double MinClearingFee
        {
            get { return _minClearingFee; }
            set { _minClearingFee = value; }
        }

        private bool _isCriteriaApplied = false;

        public bool IsCriteriaApplied
        {
            get { return _isCriteriaApplied; }
            set { _isCriteriaApplied = value; }
        }

        private bool _isClearingFeeApplied = false;

        public bool IsClearingFeeApplied
        {
            get { return _isClearingFeeApplied; }
            set { _isClearingFeeApplied = value; }
        }


        //get commission rule criteria for a commission rule if exists...
        private List<CommissionRuleCriteria> _commissionRuleCriteriaList;

        public List<CommissionRuleCriteria> CommissionRuleCriteiaList
        {
            get { return _commissionRuleCriteriaList; }
            set { _commissionRuleCriteriaList = value; }

        }

        private bool _isModified = false;


        // public CommissionRuleChangeHandler CommissionRuleChanged;
        public event EventHandler<EventArgs<CommissionRule>> CommissionRuleChanged;

        public bool IsModified
        {
            get { return _isModified; }
            set
            {
                _isModified = value;
                if (_isModified)
                {
                    if (CommissionRuleChanged != null)
                    {
                        CommissionRuleChanged(this, null);
                    }
                }
            }
        }
        #endregion
    }

    //public delegate void CommissionRuleChangeHandler(CommissionRule changedRule);
}
