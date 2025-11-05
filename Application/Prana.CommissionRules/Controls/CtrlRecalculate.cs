using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.BusinessObjects;
using Infragistics.Win.UltraWinGrid;
using Prana.Utilities.UIUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.BusinessObjects.AppConstants;
using Prana.PM;
using Nirvana.CommissionRules;
using Prana.Allocation.BLL;



namespace Prana.CommissionRules.Controls
{
    public partial class CtrlRecalculate : UserControl
    {
        public event EventHandler RecalculateCommission;
        //public event EventHandler ViewRule;
        public CtrlRecalculate()
        {
            InitializeComponent();
            BindCombos();
            txtCommissionRate.Enabled = false;
            txtFeesRate.Enabled = false;
            cmbCommissionBasedOn.Enabled = false;
            cmbFeesBasedOn.Enabled = false;
            cbCommission.Enabled = false;
            cbFees.Enabled = false;
        }



        private void BindCombos()
        {
            CommissionDBManager.GetAllSavedCommissionRules();
            List<CommissionRule> getAllCommissionRules = new List<CommissionRule>();
            getAllCommissionRules = CommissionRulesCacheManager.GetInstance().GetAllCommissionRules();
            //string C_COMBO_SELECT = "-Select-";
            //getAllCommissionRules.Insert(0, new CommissionRule(Guid.Empty, C_COMBO_SELECT));
            
            cmBoxRules.DataSource = null;
            cmBoxRules.DataSource = getAllCommissionRules;
            cmBoxRules.ValueMember = "RuleID";
            cmBoxRules.DisplayMember = "RuleName";
        
            ColumnsCollection columns = cmBoxRules.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                
                if (column.Key != "RuleName")
                {
                    column.Hidden = true;
                }
                else
                {
                    // The column headers are set as invisible.
                    cmBoxRules.DisplayLayout.Bands[0].ColHeadersVisible = false;
                }
            }

            EnumerationValueList lstBasedOn = new EnumerationValueList();

            // bind Based on Parameter combo

            lstBasedOn = EnumHelper.ConvertEnumForBindingWithSelect(typeof(CommissionCalculationBasis));
            cmbCommissionBasedOn.DataSource = null;
            cmbCommissionBasedOn.DataSource = lstBasedOn;
            cmbCommissionBasedOn.DisplayMember = "DisplayText";
            cmbCommissionBasedOn.ValueMember = "Value";
            if(cmbCommissionBasedOn.Value!=null)
            {
            if (cmbCommissionBasedOn.Value.ToString() == "Shares")
            {
                lblCommissionRate.Text = "Per Share";
            }
            else if (cmbCommissionBasedOn.Value.ToString() == "Notional")
            {
                lblCommissionRate.Text = "Basis Point";
            }
            else
            {
                lblCommissionRate.Text = "Per Contract";
            }
            }
            Utils.UltraComboFilter(cmbCommissionBasedOn, "DisplayText");
            cmbCommissionBasedOn.Value = int.MinValue;

            // bind Based on Criteria combo

            lstBasedOn = EnumHelper.ConvertEnumForBindingWithSelect(typeof(CommissionCalculationBasis));
            cmbFeesBasedOn.DataSource = null;
            cmbFeesBasedOn.DataSource = lstBasedOn;
            cmbFeesBasedOn.DisplayMember = "DisplayText";
            cmbFeesBasedOn.ValueMember = "Value";
            Utils.UltraComboFilter(cmbFeesBasedOn, "DisplayText");
            cmbFeesBasedOn.Value = int.MinValue;
            

        }



       

        private void btnRecalculateCommission_Click(object sender, EventArgs e)
        {

            CommissionRule commissionRule = new CommissionRule();
            CommissionRuleEvents commissionRuleEvent = new CommissionRuleEvents();
            if (rdbtnGroup.Checked == true)
            {   
                commissionRuleEvent.GroupWise = true;
              
            }
            else if(rdbtnTaxlot.Checked == true)
            {
                commissionRuleEvent.GroupWise = false;
            }
            if (rdBtnSelectCommissionRule.Checked == true)
            {
                if (cmBoxRules.Text != string.Empty)
                {
                    commissionRule = (CommissionRule)cmBoxRules.SelectedRow.ListObject;
                }
                else
                {
                    MessageBox.Show("Please select Commission Rule for calculation", "Prana Alert", MessageBoxButtons.OK);
                }
            }
            else if (rdBtnSpecifyCommissionRule.Checked == true)
            {
                if (cbCommission.Checked.Equals(true))
                {
                    if (cmbCommissionBasedOn.Text.ToString() != "-Select-")
                    {
                        string commissionRate = txtCommissionRate.Text;
                        float commission = float.MinValue;
                        float.TryParse(commissionRate, out commission);
                        commissionRule.CommissionRate = commission;
                        commissionRule.RuleAppliedOn = (CommissionCalculationBasis)cmbCommissionBasedOn.Value;
                    }
                    else
                    {
                        MessageBox.Show("Please Select Commission Calculation Basis", "Prana Alert", MessageBoxButtons.OK);
                        return;
                    }
                }
                else if (cbCommission.Checked.Equals(false))
                {
                    commissionRule.CommissionRate = float.MinValue;
                }
                if (cbFees.Checked.Equals(true))
                {
                    if (cmbFeesBasedOn.Text.ToString() != "-Select-")
                    {
                        string clearingFee = txtFeesRate.Text;
                        float fee = float.MinValue;
                        float.TryParse(clearingFee, out fee);
                        commissionRule.ClearingFeeRate = fee;
                        commissionRule.ClearingFeeCalculationBasedOn = (CommissionCalculationBasis)cmbFeesBasedOn.Value;
                        if (fee != float.MinValue)
                        {
                            commissionRule.IsClearingFeeApplied = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select Fees Calculation Basis", "Prana Alert", MessageBoxButtons.OK);
                        return;
                    }

                }
            }

            if (RecalculateCommission != null)
            {
                RecalculateCommission(commissionRule, commissionRuleEvent);
            }
        }

        private void cmbCommissionBasedOn_ValueChanged(object sender, EventArgs e)
        {
            if (cmbCommissionBasedOn.Value != null)
            {
                if (cmbCommissionBasedOn.Value.ToString() == "Shares")
                {
                    lblCommissionRate.Text = "Per Share";
                }
                else if (cmbCommissionBasedOn.Value.ToString() == "Notional")
                {
                    lblCommissionRate.Text = "bps";
                }
                else if (cmbCommissionBasedOn.Value.ToString() == "Contracts")
                {
                    lblCommissionRate.Text = "Per Contract";
                }
                else
                {
                    lblCommissionRate.Text = "";
                }
            }
            if (cmbFeesBasedOn.Value != null)
            {
                if (cmbFeesBasedOn.Value.ToString() == "Shares")
                {
                    lblFeesRate.Text = "Per Share";
                }
                else if (cmbFeesBasedOn.Value.ToString() == "Notional")
                {
                    lblFeesRate.Text = "bps";
                }
                else if (cmbFeesBasedOn.Value.ToString() == "Contracts")
                {
                    lblFeesRate.Text = "Per Contract";
                }
                else
                {
                    lblFeesRate.Text = "";
                }
            }

        }

        private void btnViewRule_Click(object sender, EventArgs e)
        {
            CommissionRule commissionRule = new CommissionRule();
            if (rdBtnSelectCommissionRule.Checked == true && cmBoxRules.Value != null)
            {
                commissionRule = (CommissionRule)cmBoxRules.SelectedRow.ListObject;
                CommissionRuleDisplay commissionRuleDisplay = new CommissionRuleDisplay();
                commissionRuleDisplay.Text = commissionRule.RuleName;
                commissionRuleDisplay.GetCommisionRule(commissionRule);
                commissionRuleDisplay.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a Commission Rule","Prana Alert", MessageBoxButtons.OK);
            }
        }

       

        private void rdBtnSpecifyCommissionRule_CheckedChanged(object sender, EventArgs e)
        {
            if(rdBtnSpecifyCommissionRule.Checked.Equals(true))
            {
                txtCommissionRate.Enabled=true;
                txtFeesRate.Enabled=true;
                cmBoxRules.Enabled=false;
                cmbCommissionBasedOn.Enabled = true;
                cmbFeesBasedOn.Enabled = true;
                btnViewRule.Enabled = false;
                cbCommission.Enabled = true;
                cbFees.Enabled = true;
                cbCommission.Checked=true;
                cbFees.Checked=true;
            }
            else if (rdBtnSpecifyCommissionRule.Checked.Equals(false))
            {
                txtCommissionRate.Enabled = false;
                txtFeesRate.Enabled = false;
                cmBoxRules.Enabled=true;
                cmbCommissionBasedOn.Enabled = false;
                cmbFeesBasedOn.Enabled = false;
                btnViewRule.Enabled = true;
                cbCommission.Enabled = false;
                cbFees.Enabled = false;
            }
        }

        private void cbCommission_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCommission.Checked.Equals(true))
            {
                txtCommissionRate.Enabled = true;
                cmbCommissionBasedOn.Enabled = true;
            }
            else if (cbCommission.Checked.Equals(false))
            {
                txtCommissionRate.Enabled=false;
                cmbCommissionBasedOn.Enabled = false;
            }
        }

        private void cbFees_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFees.Checked.Equals(true))
            {
                txtFeesRate.Enabled = true;
                cmbFeesBasedOn.Enabled = true;
            }
            else if (cbFees.Checked.Equals(false))
            {
                txtFeesRate.Enabled = false;
                cmbFeesBasedOn.Enabled = false;
            }
        }



       

       

       
    }
}
