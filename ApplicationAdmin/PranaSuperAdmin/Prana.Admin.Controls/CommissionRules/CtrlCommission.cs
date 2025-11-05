#region Summary
///////////////////////////////////////////////////////////////////////////////
// AUTHOR		 : Bharat Kumar Jangir
// CREATED ON	 : 13 March, 2014
///////////////////////////////////////////////////////////////////////////////
#endregion

#region NameSpaces
#region .Net Base Class Namespace Imports
using System;
using System.Collections.Generic;
using System.Windows.Forms;
#endregion
#region Custom Namespaces
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommissionRules;
using Prana.Utilities.UI.UIUtilities;
using Prana.Utilities;
#endregion
#endregion

namespace Prana.Admin.Controls.CommissionRules
{
    public partial class CtrlCommission : UserControl
    {
        #region Variables
        const string C_COMBO_SELECT = "-Select-";
        private CommissionType commissionType;
        //TO DO: Set this variable to Parent control
        #endregion

        #region Public Methods
        public CtrlCommission()
        {
            InitializeComponent();
        }

        public void InitControls()
        {
            cmbBasedOnCriteria.Text = C_COMBO_SELECT;

            // parameter box
            rdbtnParameters.Checked = true;
            grpParameters.Enabled = true;
            cmbBasedOnParameter.Text = C_COMBO_SELECT;
            nudCommissionRateParameter.Value = 0;
            nudMiniCommParameter.Value = 0;
            nudMaxCommParameter.Value = 0;

            // Criteria box
            rdbtnCriteria.Checked = false;
            grpCriteria.Enabled = false;
            nudMiniCommCriteria.Value = 0;
            cmbBasedOnCriteria.Text = C_COMBO_SELECT;
        }

        public void CommissionRuleLoad(CommissionType commissionType)
        {
            rdbtnParameters.Checked = true;
            RoundOffCheckBox.Checked = true;
            this.commissionType = commissionType;
            BindCriteriaGrid(commissionType);
        }

        public void AddNewCommissionRuleOnUI(Commission commission)
        {
            commission.IsCriteriaApplied = false;
            commission.IsRoundOff = false;
            commission.RoundOffValue = 2;
            commission.RuleAppliedOn = (CalculationBasis)cmbBasedOnParameter.Value;
            commission.CommissionRate = double.Parse(nudCommissionRateParameter.Value.ToString());
            commission.MinCommission = double.Parse(nudMiniCommParameter.Value.ToString());
            commission.MaxCommission = double.Parse(nudMaxCommParameter.Value.ToString());
        }

        public Boolean CheckValidation(ErrorProvider errorProvider1)
        {
            errorProvider1.SetError(cmbBasedOnParameter, "");
            errorProvider1.SetError(nudCommissionRateParameter, "");

            errorProvider1.SetError(cmbBasedOnCriteria, "");

            if (rdbtnParameters.Checked == true)
            {
                if ((int)cmbBasedOnParameter.Value == int.MinValue)
                {
                    errorProvider1.SetError(cmbBasedOnParameter, "Please select Commission Based on Parameter.");
                    cmbBasedOnParameter.Focus();
                    return false;
                }
                else if (nudCommissionRateParameter.Value < 0)
                {
                    errorProvider1.SetError(nudCommissionRateParameter, "Please select Commission Based on Parameter.");
                    nudCommissionRateParameter.Focus();
                    return false;
                }
                if ((decimal.Parse(nudMaxCommParameter.Value.ToString())) < (decimal.Parse(nudMiniCommParameter.Value.ToString())))
                {
                    errorProvider1.SetError(nudMaxCommParameter, "Maximum commission can not be lower than minimum commission");
                    nudMaxCommParameter.Focus();
                    return false;
                }
            }
            else if (rdbtnCriteria.Checked == true)
            {
                int index = 1;

                if ((int)cmbBasedOnCriteria.Value == int.MinValue)
                {
                    errorProvider1.SetError(cmbBasedOnCriteria, "Please select Commission Based on Criteria.");
                    cmbBasedOnCriteria.Focus();
                    return false;
                }
                else if (grdCommissionRules.Rows.Count <= 1)
                {
                    MessageBox.Show("Please enter atleast one Commission Rule Criteria.", "Prana Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    grdCommissionRules.Focus();
                    return false;
                }
                else if (grdCommissionRules.Rows.Count > 1)
                {
                    int rowcount = grdCommissionRules.Rows.Count;
                    // Validation to check whether any field in not blank              
                    foreach (UltraGridRow dr in grdCommissionRules.Rows)
                    {
                        double valuefrom = Convert.ToDouble(dr.Cells["ValueGreaterThan"].Value.ToString());
                        double valueto = Convert.ToDouble(dr.Cells["ValueLessThanOrEqual"].Value.ToString());
                        double commissionrate = double.Parse(dr.Cells["CommissionRate"].Value.ToString());

                        if (rowcount != index)
                        {
                            if (valueto > 0 || commissionrate <= 0)
                            {
                                if (commissionrate <= 0)
                                {
                                    MessageBox.Show("Please enter the Commission Rate in the row : " + index, "Nirvane Admin");
                                    grdCommissionRules.Focus();
                                    return false;
                                }
                            }
                            if (valuefrom >= valueto)
                            {
                                MessageBox.Show("' Value To(<=) ' should be greater than ' Value From(>) ' in the row : " + index, "Nirvane Admin");
                                grdCommissionRules.Focus();
                                return false;

                            }
                            index = index + 1;
                        }
                        // check validation of commission rate for the last row
                        if (rowcount == index)
                        {
                            if (valuefrom > 0)
                            {
                                if (commissionrate <= 0)
                                {
                                    MessageBox.Show("Please enter the Commission Rate in the row : " + index, "Nirvane Admin");
                                    grdCommissionRules.Focus();
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void SetCommissionRuleDetails(Commission commission, CommissionType commissionType)
        {
            this.commissionType = commissionType;
            if (commission.IsCriteriaApplied)
            {
                rdbtnCriteria.Checked = true;
                cmbBasedOnCriteria.Value = commission.RuleAppliedOn;
                nudMiniCommCriteria.Value = decimal.Parse(commission.MinCommission.ToString());
                grdCommissionRules.DataSource = commission.CommissionRuleCriteiaList;
                UpdateCommissionRateUnitForCriterias();

                // make Parameter section default
                rdbtnParameters.Checked = false;
                cmbBasedOnParameter.Text = C_COMBO_SELECT;
                nudCommissionRateParameter.Value = 0;
                nudMiniCommParameter.Value = 0;
                nudMaxCommParameter.Value = 0;
            }
            else
            {
                rdbtnParameters.Checked = true;
                cmbBasedOnParameter.Value = commission.RuleAppliedOn;
                nudCommissionRateParameter.Value = decimal.Parse(commission.CommissionRate.ToString());
                nudMiniCommParameter.Value = decimal.Parse(commission.MinCommission.ToString());
                nudMaxCommParameter.Value = decimal.Parse(commission.MaxCommission.ToString());
                RoundOffCheckBox.Checked = commission.IsRoundOff;
                RoundOff.Value = decimal.Parse(commission.RoundOffValue.ToString());
                //make criteria section default
                rdbtnCriteria.Checked = false;
                cmbBasedOnCriteria.Text = C_COMBO_SELECT;
                nudMiniCommCriteria.Value = 0;
                BindCriteriaGrid(commissionType);
            }
        }

        public void BindAllBasedOnCombos()
        {
            EnumerationValueList lstBasedOn = new EnumerationValueList();

            // bind Based on Parameter combo
            lstBasedOn = CommissionEnumHelper.GetOldListForCalculationBasis(false);
            cmbBasedOnParameter.DataSource = lstBasedOn;
            cmbBasedOnParameter.DisplayMember = "DisplayText";
            cmbBasedOnParameter.ValueMember = "Value";
            Utils.UltraComboFilter(cmbBasedOnParameter, "DisplayText");
            cmbBasedOnParameter.Value = int.MinValue;

            // bind Based on Criteria combo
            lstBasedOn = CommissionEnumHelper.GetCommissionCriterias();
            cmbBasedOnCriteria.DataSource = null;
            cmbBasedOnCriteria.DataSource = lstBasedOn;
            cmbBasedOnCriteria.DisplayMember = "DisplayText";
            cmbBasedOnCriteria.ValueMember = "Value";
            Utils.UltraComboFilter(cmbBasedOnCriteria, "DisplayText");
            cmbBasedOnCriteria.Value = int.MinValue;
        }

        public void SetValues(Commission commission)
        {
            if (rdbtnParameters.Checked == true)
            {
                commission.IsCriteriaApplied = false;
                commission.RuleAppliedOn = (CalculationBasis)cmbBasedOnParameter.Value;
                commission.CommissionRate = double.Parse(nudCommissionRateParameter.Value.ToString());
                commission.MinCommission = double.Parse(nudMiniCommParameter.Value.ToString());
                commission.MaxCommission = double.Parse(nudMaxCommParameter.Value.ToString());
                commission.IsRoundOff = bool.Parse(RoundOffCheckBox.Checked.ToString());
                commission.RoundOffValue = int.Parse(RoundOff.Value.ToString());
            }
            if (rdbtnCriteria.Checked == true)
            {
                commission.RuleAppliedOn = (CalculationBasis)cmbBasedOnCriteria.Value;
                commission.IsCriteriaApplied = true;
                commission.MinCommission = double.Parse(nudMiniCommCriteria.Value.ToString());
                commission.CommissionRuleCriteiaList = (List<CommissionRuleCriteria>)grdCommissionRules.DataSource;
            }
        }
        #endregion

        #region Private Methods
        private void UpdateCommissionRateUnitForCriterias()
        {
            int selectedValue = Convert.ToInt32(cmbBasedOnCriteria.Value);
            string rateUnit = GetRateUnitByValue(selectedValue);
            List<CommissionRuleCriteria> commissionCriterias = (List<CommissionRuleCriteria>)grdCommissionRules.DataSource;
            if (commissionCriterias == null)
            {
                return;
            }
            foreach (CommissionRuleCriteria criteria in commissionCriterias)
            {
                criteria.CommissionCriteriaUnit = rateUnit;
            }
            grdCommissionRules.Refresh();
        }

        private void BindCriteriaGrid(CommissionType commissionType)
        {
            List<CommissionRuleCriteria> commissioncriterias = new List<CommissionRuleCriteria>();
            Guid dummyRuleID = System.Guid.NewGuid();
            commissioncriterias = CommissionDBManager.GetCommissionRuleCriterias(dummyRuleID);
            grdCommissionRules.DataSource = commissioncriterias;
            AddNewTempRow(commissionType);
            InitCriteriaGrid();
        }

        private void AddNewTempRow(CommissionType commissionType)
        {
            List<CommissionRuleCriteria> commissionRuleCriteriacolls = new List<CommissionRuleCriteria>();

            CommissionRuleCriteria commrulecri = new CommissionRuleCriteria();
            commrulecri.CommissionCriteriaId = int.MinValue;
            commrulecri.CommissionRate = 0;
            commrulecri.ValueGreaterThan = 0;
            commrulecri.ValueLessThanOrEqual = 0;
            commrulecri.CommissionType = commissionType;
            commrulecri.CommissionCriteriaUnit = GetRateUnitByValue(Convert.ToInt32(cmbBasedOnCriteria.Value));
            commissionRuleCriteriacolls.Add(commrulecri);
            grdCommissionRules.DataSource = commissionRuleCriteriacolls;
        }

        private void AddNewRow()
        {
            UltraGridCell prevActiveCell = grdCommissionRules.Rows[0].Cells[0];
            string cellText = string.Empty;
            int len = int.MinValue;
            if (grdCommissionRules.ActiveCell != null)
            {
                prevActiveCell = grdCommissionRules.ActiveCell;
                cellText = prevActiveCell.Text;
                len = cellText.Length;
            }

            UltraGridRow activeRow = grdCommissionRules.ActiveRow;
            double valueLessThanOrEqual = 0;
            if (activeRow != null)
            {
                valueLessThanOrEqual = Convert.ToDouble(activeRow.Cells["ValueLessThanOrEqual"].Value.ToString());
            }
            int rowsCount = grdCommissionRules.Rows.Count;
            UltraGridRow dr = grdCommissionRules.Rows[rowsCount - 1];

            List<CommissionRuleCriteria> commrulecris = (List<CommissionRuleCriteria>)grdCommissionRules.DataSource;
            CommissionRuleCriteria commrulecri = new CommissionRuleCriteria();

            //The below varriables are taken from the last row of the grid before adding the new row.
            double dblValueFrom = Convert.ToDouble(dr.Cells["ValueGreaterThan"].Value.ToString());
            double dblValueTo = Convert.ToDouble(dr.Cells["ValueLessThanOrEqual"].Value.ToString());
            double CommRate = Convert.ToDouble(dr.Cells["CommissionRate"].Value.ToString());
            string commissionRateUnit = dr.Cells["CommissionCriteriaUnit"].Value.ToString();

            Convert.ToInt32(cmbBasedOnCriteria.Value);

            //Add one more condition to avoid adding 2nd new row when the below condition fullfills.
            if (dblValueTo > 0)
            {
                commrulecri.CommissionCriteriaId = int.MinValue;
                commrulecri.CommissionRate = 0;
                commrulecri.ValueGreaterThan = 0;
                commrulecri.ValueLessThanOrEqual = 0;
                commrulecri.CommissionCriteriaUnit = commissionRateUnit;
                commrulecri.CommissionType = commissionType;
                commrulecris.Add(commrulecri);
                grdCommissionRules.DataSource = commrulecris;
                grdCommissionRules.DataBind();
                grdCommissionRules.ActiveCell = prevActiveCell;
                grdCommissionRules.Focus();
                grdCommissionRules.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
            }

            grdCommissionRules.UpdateData();
            if (grdCommissionRules.ActiveRow != null)
            {
                int rowindex = grdCommissionRules.ActiveRow.Index;
                int rowsC = grdCommissionRules.Rows.Count;
                // assign value to next cloumn
                if ((rowindex + 1) != rowsC)
                {
                    UltraGridRow newDatarow = grdCommissionRules.Rows[rowindex + 1];
                    newDatarow.Cells["ValueGreaterThan"].Value = valueLessThanOrEqual;
                }
            }
        }

        private void InitCriteriaGrid()
        {
            grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].Header.VisiblePosition = 0;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].Header.Caption = "Value From(>)";
            grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].MaxLength = 14;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].Width = 105;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

            grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].Header.VisiblePosition = 1;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].MaxLength = 14;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].Header.Caption = "Value To(<=)";
            grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].Width = 105;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].Header.VisiblePosition = 2;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].Format = "##,###.0000";
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].MaxLength = 10;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].Width = 105;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].Header.Caption = "Commission Rate";
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRate"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionCriteriaUnit"].Header.VisiblePosition = 4;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionCriteriaUnit"].Width = 75;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionCriteriaUnit"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionCriteriaUnit"].Header.Caption = "Rate Unit";
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionCriteriaUnit"].CellActivation = Activation.NoEdit;

            grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Header.VisiblePosition = 4;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Width = 40;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;

            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionCriteriaID"].Header.VisiblePosition = 5;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionCriteriaID"].Hidden = true;

            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionType"].Header.VisiblePosition = 6;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionType"].Header.Caption = "Commission Type";
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionType"].CellActivation = Activation.Disabled;
            grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionType"].Hidden = true;
        }

        private string GetRateUnitByValue(int selectedValue)
        {
            CalculationBasis criteria = (CalculationBasis)selectedValue;
            switch (criteria)
            {
                case CalculationBasis.Shares:
                    return "Per Share";
                case CalculationBasis.Notional:
                    return "Basis Points";
                case CalculationBasis.Contracts:
                    return "Per Contract";
                case CalculationBasis.AvgPrice:
                    return "Per Share/Contract";
                case CalculationBasis.FlatAmount:
                    return "Per Trade/Taxlot";
                default:
                    return string.Empty;
            }
        }
        #endregion

        #region  Grid Events Region
        private void grdCommissionRules_ClickCellButton(object sender, CellEventArgs e)
        {
            grdCommissionRules.UpdateData();
            if (grdCommissionRules.Rows.Count == 1)
            {
                // MessageBox.Show("Nothing to delete.", "Prana Alter");
                return;
            }
            if (e.Cell.Column.Key.Equals("DeleteButton"))
            {
                if (Convert.ToDouble(grdCommissionRules.ActiveRow.Cells["ValueLessThanOrEqual"].Value.ToString()) > 0 && Convert.ToDouble(grdCommissionRules.ActiveRow.Cells["ValueGreaterThan"].Value.ToString()) > 0)
                {
                    int activeRowIndex = grdCommissionRules.ActiveRow.Index;
                    double activeRowValueLessThanOrEqual = Convert.ToDouble(grdCommissionRules.ActiveRow.Cells["ValueLessThanOrEqual"].Value.ToString());
                    bool isDeleted = grdCommissionRules.ActiveRow.Delete();
                    if (isDeleted)
                    {
                        UltraGridRow previousDatarow = grdCommissionRules.Rows[activeRowIndex - 1];
                        previousDatarow.Cells["ValueLessThanOrEqual"].Value = activeRowValueLessThanOrEqual;
                    }
                }
            }
        }

        private void grdCommissionRules_Error(object sender, ErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void grdCommissionRules_CellChange(object sender, CellEventArgs e)
        {
            ((ctrlCommissionRule)(this.Parent.Parent.Parent)).IsUnSavedChanges = true;
            if (e.Cell.Column.Key.Equals("ValueLessThanOrEqual") || e.Cell.Column.Key.Equals("CommissionRate"))
            {
                double result;
                bool isdblParsed = double.TryParse(e.Cell.Text.ToString(), out result);
                if (isdblParsed)
                {
                    double dblValueTo = Convert.ToDouble(e.Cell.Text.ToString());
                    e.Cell.Refresh();
                }
            }
        }

        private void grdCommissionRules_AfterCellUpdate(object sender, CellEventArgs e)
        {
            AddNewRow();
        }
        #endregion  Grid Events Region

        private void RoundOffCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ((ctrlCommissionRule)(this.Parent.Parent.Parent)).IsUnSavedChanges = true;
            if (RoundOffCheckBox.Checked.Equals(true))
            {
                RoundOff.Enabled = true;
            }
            else
            {
                RoundOff.Value = 2;
                RoundOff.Enabled = false;
            }
        }

        private void RoundOff_ValueChanged(object sender, EventArgs e)
        {
            ((ctrlCommissionRule)(this.Parent.Parent.Parent)).IsUnSavedChanges = true;
            nudMaxCommParameter.Value = Math.Round(nudMaxCommParameter.Value, int.Parse(RoundOff.Value.ToString()));
            nudMiniCommParameter.Value = Math.Round(nudMiniCommParameter.Value, int.Parse(RoundOff.Value.ToString()));
        }

        private void cmbBasedOnCriteria_ValueChanged(object sender, System.EventArgs e)
        {
            ((ctrlCommissionRule)(this.Parent.Parent.Parent)).IsUnSavedChanges = true;
            UpdateCommissionRateUnitForCriterias();
        }

        private void cmbBasedOnParameter_ValueChanged(object sender, EventArgs e)
        {
            ((ctrlCommissionRule)(this.Parent.Parent.Parent)).IsUnSavedChanges = true;
            if (cmbBasedOnParameter.Value != null)
            {
                int selectedValue = Convert.ToInt32(cmbBasedOnParameter.Value);
                lbldisplayParameter.Text = GetRateUnitByValue(selectedValue);
                CalculationBasis criteria = (CalculationBasis)selectedValue;
                if (criteria.Equals(CalculationBasis.FlatAmount))
                {
                    lblCommissionRateParameter.Text = "Commission";
                }
                else
                {
                    lblCommissionRateParameter.Text = "Commission Rate";
                }
            }

        }

        private void rdbtnParameters_CheckedChanged(object sender, EventArgs e)
        {
            ((ctrlCommissionRule)(this.Parent.Parent.Parent)).IsUnSavedChanges = true;
            if (rdbtnParameters.Checked == false)
            {
                grpParameters.Enabled = false;
                cmbBasedOnParameter.Text = C_COMBO_SELECT;
                nudCommissionRateParameter.Value = 0;
                nudMiniCommParameter.Value = 0;
                nudMaxCommParameter.Value = 0;
            }
            else if (rdbtnParameters.Checked == true)
            {
                grpParameters.Enabled = true;
                // make criteria group enable = false;
                grpCriteria.Enabled = false;
                nudMiniCommCriteria.Value = 0;
                nudMaxCommParameter.Value = 0;
                cmbBasedOnCriteria.Text = C_COMBO_SELECT;
            }
        }

        private void rdbtnCriteria_CheckedChanged(object sender, EventArgs e)
        {
            ((ctrlCommissionRule)(this.Parent.Parent.Parent)).IsUnSavedChanges = true;
            if (rdbtnCriteria.Checked == false)
            {
                grpCriteria.Enabled = false;
                nudMiniCommCriteria.Value = 0;
                cmbBasedOnCriteria.Text = C_COMBO_SELECT;
                BindCriteriaGrid(commissionType);
            }
            else if (rdbtnCriteria.Checked == true)
            {
                grpCriteria.Enabled = true;
                // make Parameter group box enable=false and reset all the control of that group box
                grpParameters.Enabled = false;
                cmbBasedOnParameter.Text = C_COMBO_SELECT;
                nudCommissionRateParameter.Value = 0;
                nudMiniCommParameter.Value = 0;
                nudMaxCommParameter.Value = 0;
            }
        }
    }
}
