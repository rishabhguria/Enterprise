using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.BusinessObjects;

namespace Prana.Allocation
{
    public partial class CommissionRuleDisplay : Form
    {
        public CommissionRuleDisplay()
        {
            InitializeComponent();
            // ShowCommissionRule(_commisionRule);
        }
        private CommissionRule _commisionRule = new CommissionRule();
        public void GetCommisionRule(CommissionRule commisisonRule)
        {
            _commisionRule = commisisonRule;
        }
        private void ShowCommissionRule(CommissionRule commissionRule)
        {
            string s = string.Empty;
            
            if (commissionRule.RuleAppliedOn == Prana.BusinessObjects.AppConstants.CommissionCalculationBasis.Shares)
            {
                s="Per Share";
            }
            else if (commissionRule.RuleAppliedOn == Prana.BusinessObjects.AppConstants.CommissionCalculationBasis.Notional)
            {
               s= "Basis Points";
            }
            else if (commissionRule.RuleAppliedOn == Prana.BusinessObjects.AppConstants.CommissionCalculationBasis.Contracts)
            {
               s= "Per Contract";
            };
            string a = string.Empty;
            foreach (Prana.BusinessObjects.AppConstants.AssetCategory asset in commissionRule.AssetIdList)
            {
                a += asset.ToString() + ","+"\n"; 
            }
            lblAssets.Text = "The Permitted Assets : "+ "\n"+a.Substring(0,a.Length-2)   ;
            if (commissionRule.RuleDescription != string.Empty)
            {
                lblDescription.Text = "Rule Description is " + commissionRule.RuleDescription;
            }
            string b= string.Empty;
            if (commissionRule.ApplyRuleForTrade == Prana.BusinessObjects.AppConstants.TradeType.Both)
            {
                b = " Single and Basket Trade";
            }
            lblAppliedRule.Text = "Rule Applied to " + commissionRule.ApplyRuleForTrade + b;
            if (commissionRule.IsCriteriaApplied == false)
            {
                lblCommissionCriteria.Text = "Commission Rate : " +"\n"+ commissionRule.CommissionRate.ToString() + " " + s;
            }
            string criteria = string.Empty;

            if (commissionRule.IsCriteriaApplied == true)       
            {
                foreach (CommissionRuleCriteria commissionRuleCriteria in commissionRule.CommissionRuleCriteiaList)
                {
                    criteria += commissionRuleCriteria.CommissionRate.ToString()+"\t"+s+" for < "+ commissionRuleCriteria .ValueGreaterThan.ToString() + " and <" + commissionRuleCriteria.ValueLessThanOrEqual.ToString() +"\n";               
                }
                int len = criteria.Length;
                lblCommissionCriteria.Text = "Commission rate : " +"\n"+ criteria.Substring(0, (len - 8));
            }
            string f = string.Empty;

            if (commissionRule.ClearingFeeCalculationBasedOn == Prana.BusinessObjects.AppConstants.CommissionCalculationBasis.Shares)
            {
                f = "Per Share";
            }
            else if (commissionRule.ClearingFeeCalculationBasedOn == Prana.BusinessObjects.AppConstants.CommissionCalculationBasis.Notional)
            {
                f = "Basis Points";
            }
            else if (commissionRule.ClearingFeeCalculationBasedOn == Prana.BusinessObjects.AppConstants.CommissionCalculationBasis.Contracts)
            {
                f = "Per Contract";
            };
            if (commissionRule.IsClearingFeeApplied == true)
            {
                lblFeesRate.Text = "Clearing  Fee Rate : "+"\n" + commissionRule.ClearingFeeRate.ToString() + " " + f;
            }
                
               
            
        }

        private void CommissionRuleDisplay_Load(object sender, EventArgs e)
        {
            ShowCommissionRule(_commisionRule);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Form.ActiveForm.Close();
        }

    }
}

