using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.BusinessObjects;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Utilities.UIUtilities;

namespace Prana.AllocationNew
{
    public partial class CommissionRuleDisplay : Form
    {
        public CommissionRuleDisplay()
        {
            InitializeComponent();
            // ShowCommissionRule(_commisionRule);
        }
        private CommissionRule _commisionRule = new CommissionRule();
        const string CAP_PerShare ="Per Share";
        const string CAP_BasisPoints = "Basis Points";
        const string CAP_PerContract = "Per Contract";
        const string CAP_FlatAmount = "Per Trade/Taxlot";
        public void GetCommisionRule(CommissionRule commisisonRule)
        {
            _commisionRule = commisisonRule;
        }
        private void ShowCommissionRule(CommissionRule commissionRule)
        {
            try
            {
                if (!commissionRule.RuleName.Equals(Prana.Global.ApplicationConstants.C_COMBO_SELECT))
                {
                    string s = string.Empty;
                    string s1 = string.Empty;

                    s = GetRuleAppliedString(commissionRule.Commission.RuleAppliedOn);
                    s1 = GetRuleAppliedString(commissionRule.SoftCommission.RuleAppliedOn);

                    string a = string.Empty;
                    foreach (Prana.BusinessObjects.AppConstants.AssetCategory asset in commissionRule.AssetIdList)
                    {
                        a += asset.ToString() + "," + "\n";
                    }
                    lblAssets.Text = "The Permitted Assets : " + "\n" + a.Substring(0, a.Length - 2);
                    if (commissionRule.RuleDescription != string.Empty)
                    {
                        lblDescription.Text = "Rule Description is " + commissionRule.RuleDescription;
                    }
                    string b = string.Empty;
                    if (commissionRule.ApplyRuleForTrade == Prana.BusinessObjects.AppConstants.TradeType.Both)
                    {
                        b = " Single and Basket Trade";
                    }
                    lblAppliedRule.Text = "Rule Applied to " + commissionRule.ApplyRuleForTrade + b;
                    
                    if (commissionRule.Commission.IsCriteriaApplied == false)
                    {
                        lblCommissionCriteria.Text = "Commission Rate : " + "\n" + commissionRule.Commission.CommissionRate.ToString() + " " + s;
                    }
                    
                    string criteria = string.Empty;
                    if (commissionRule.Commission.IsCriteriaApplied == true)
                    {
                        foreach (CommissionRuleCriteria commissionRuleCriteria in commissionRule.Commission.CommissionRuleCriteiaList)
                        {
                            criteria += commissionRuleCriteria.CommissionRate.ToString() + "\t" + s + " for < " + commissionRuleCriteria.ValueGreaterThan.ToString() + " and <" + commissionRuleCriteria.ValueLessThanOrEqual.ToString() + "\n";
                        }
                        int len = criteria.Length;
                        lblCommissionCriteria.Text = "Commission rate : " + "\n" + criteria.Substring(0, (len - 8));
                    }

                    if (commissionRule.SoftCommission.IsCriteriaApplied == false)
                    {
                        lblSoftCommissionCriteria.Text = "Soft Commission Rate : " + "\n" + commissionRule.SoftCommission.CommissionRate.ToString() + " " + s1;
                    }

                    string softCriteria = string.Empty;
                    if (commissionRule.SoftCommission.IsCriteriaApplied == true)
                    {
                        foreach (CommissionRuleCriteria commissionRuleCriteria in commissionRule.SoftCommission.CommissionRuleCriteiaList)
                        {
                            softCriteria += commissionRuleCriteria.CommissionRate.ToString() + "\t" + s1 + " for < " + commissionRuleCriteria.ValueGreaterThan.ToString() + " and <" + commissionRuleCriteria.ValueLessThanOrEqual.ToString() + "\n";
                        }
                        int len = softCriteria.Length;
                        lblSoftCommissionCriteria.Text = "Soft Commission rate : " + "\n" + softCriteria.Substring(0, (len - 8));
                    }

                    string f = string.Empty;
                    f = GetRuleAppliedString(commissionRule.ClearingFeeCalculationBasedOn);
                    if (commissionRule.IsClearingFeeApplied == true)
                    {
                        lblFeesRate.Text = "OtherBrokerFees Rate : " + "\n" + commissionRule.ClearingFeeRate.ToString() + " " + f;
                    }

                    string f1 = string.Empty;
                    f1 = GetRuleAppliedString(commissionRule.ClearingBrokerFeeCalculationBasedOn);
                    if (commissionRule.IsClearingBrokerFeeApplied == true)
                    {
                        lblClearingBrokerFeeRate.Text = "ClearingBrokerFee Rate : " + "\n" + commissionRule.ClearingBrokerFeeRate.ToString() + " " + f1;
                    }
                    //if (commissionRule.OtherFeesRuleList.Count > 0)
                    //{
                    //    string otherFeeDes = string.Empty;
                    //    foreach (OtherFeeRule oFR in commissionRule.OtherFeesRuleList)
                    //    {
                    //         otherFeeDes+= oFR.OtherFeeType.ToString() + "Long Fee Rate :" + oFR.LongRate.ToString() + " " + GetRuleAppliedString(oFR.LongCalculationBasis) + " " + "Short Fee Rate:" + oFR.ShortRate + " " + GetRuleAppliedString(oFR.ShortCalculationBasis)+"\n";
                    //    }
                    //    lblOtherFees.Text = otherFeeDes;
                    //}
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);


                if (rethrow)
                {
                    throw;
                }
            }
        }

        private string GetRuleAppliedString(Prana.BusinessObjects.AppConstants.CalculationBasis calculationBasis)
        {
            try
            {
                switch (calculationBasis)
                {
                    case Prana.BusinessObjects.AppConstants.CalculationBasis.Shares:
                        return CAP_PerShare;

                    case Prana.BusinessObjects.AppConstants.CalculationBasis.Notional:
                        return CAP_BasisPoints;

                    case Prana.BusinessObjects.AppConstants.CalculationBasis.Contracts:
                        return CAP_PerContract;

                    case Prana.BusinessObjects.AppConstants.CalculationBasis.NotionalPlusCommission:
                        return CAP_BasisPoints;

                    case Prana.BusinessObjects.AppConstants.CalculationBasis.Commission:
                        return CAP_BasisPoints;

                    case Prana.BusinessObjects.AppConstants.CalculationBasis.AvgPrice:
                        return CAP_PerShare;

                    case Prana.BusinessObjects.AppConstants.CalculationBasis.FlatAmount:
                        return CAP_FlatAmount;
                    default:
                        throw new Exception("Commission rule basis not set. It should depend either of Shares,Contracts , Notional values,Commissionor NotionalPlusCommission.");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        private void CommissionRuleDisplay_Load(object sender, EventArgs e)
        {
            try
            {
                ShowCommissionRule(_commisionRule);
                if (CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnClose.BackColor = System.Drawing.Color.FromArgb(55,67,85);
                btnClose.ForeColor = System.Drawing.Color.White;
                btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClose.UseAppStyling = false;
                btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
                               
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Form.ActiveForm.Close();
        }

    }
}

