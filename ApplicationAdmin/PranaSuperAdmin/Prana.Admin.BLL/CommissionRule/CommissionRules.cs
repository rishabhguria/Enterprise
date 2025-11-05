using Prana.Global;
using System;
using System.Collections.Generic;

namespace Prana.Admin.BLL
{
    public class CommissionRules
    {
        public List<CommissionRule> _addCommissionRule = new List<CommissionRule>();
        public Dictionary<Guid, CommissionRule> _commissionRuleCollection = new Dictionary<Guid, CommissionRule>();
        public List<CommissionRule> _modifiedCommissionRule = new List<CommissionRule>();


        #region Constructor Region

        public CommissionRules()
        {

        }

        #endregion Constructor Region


        public CommissionRule GetCommissionRuleByRuleId(Guid ruleId)
        {
            CommissionRule commissionRuleobj = null;

            if (_commissionRuleCollection.ContainsKey(ruleId))
            {
                commissionRuleobj = _commissionRuleCollection[ruleId];
            }

            return commissionRuleobj;

        }

        public List<CommissionRule> AddCommissionRule(CommissionRule commissionRule)
        {
            // add commission rule into the main collection, that is type of List<CommissionRule>
            if (!_addCommissionRule.Contains(commissionRule))
            {
                _addCommissionRule.Add(commissionRule);
                commissionRule.CommissionRuleChanged += commissionRule_CommissionRuleChanged;
            }
            // add commission rule into the collection which is require for searching purpose
            // that is type of Dictionary<RuleId,CommissionRule>
            if (!_commissionRuleCollection.ContainsKey(commissionRule.RuleID))
            {
                _commissionRuleCollection.Add(commissionRule.RuleID, commissionRule);
            }
            return _addCommissionRule;

        }

        void commissionRule_CommissionRuleChanged(object sender, EventArgs<CommissionRule> args)
        {
            CommissionRule changedRule = sender as CommissionRule;
            if (changedRule != null)
            {
                if (!_modifiedCommissionRule.Contains(changedRule))
                {
                    _modifiedCommissionRule.Add(changedRule);
                }
            }
        }

    }
}
