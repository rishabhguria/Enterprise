using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommissionRules;
using Prana.LogManager;
using Prana.Utilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Admin.Controls.CommissionRules
{
    public partial class CtrlClearingBrokerFee : UserControl
    {
        const string C_COMBO_SELECT = "-Select-";
        ClearingFeeType _clearingFeeType;

        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlClearingBrokerFee"/> class.
        /// </summary>
        /// <param name="clearingFeeType">Type of the clearing fee.</param>
        public CtrlClearingBrokerFee(ClearingFeeType clearingFeeType)
        {
            InitializeComponent();
            BindAllBasedOnCombos();
            _clearingFeeType = clearingFeeType;
            BindCriteriaGrid();
            this.nudMiniCommParameter.ValueChanged += ValueChanged_Event;
            this.nudCommissionRateParameter.ValueChanged += ValueChanged_Event;
            this.nudMiniCommCriteria.ValueChanged += ValueChanged_Event;
        }

        /// <summary>
        /// Binds all based on combos.
        /// </summary>
        public void BindAllBasedOnCombos()
        {
            try
            {
                EnumerationValueList lstBasedOn = new EnumerationValueList();

                // bind Based on Parameter combo
                lstBasedOn = CommissionEnumHelper.GetOldListForCalculationBasis(true);
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
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Initializes the controls.
        /// </summary>
        public void InitControls()
        {
            try
            {
                cmbBasedOnCriteria.Text = C_COMBO_SELECT;

                // parameter box
                rdbtnParameters.Checked = true;
                grpParameters.Enabled = true;
                cmbBasedOnParameter.Text = C_COMBO_SELECT;
                nudCommissionRateParameter.Value = 0;
                nudMiniCommParameter.Value = 0;

                // Criteria box
                rdbtnCriteria.Checked = false;
                grpCriteria.Enabled = false;
                nudMiniCommCriteria.Value = 0;
                cmbBasedOnCriteria.Text = C_COMBO_SELECT;

                rdbtnParameters.Enabled = false;
                rdbtnCriteria.Enabled = false;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Enables the disable control.
        /// </summary>
        /// <param name="isEnable">if set to <c>true</c> [is enable].</param>
        public void EnableDisableCtrl(bool isEnable)
        {
            try
            {
                ((ctrlCommissionRule)(this.Parent)).IsUnSavedChanges = true;
                if (isEnable)
                {
                    grpParameters.Enabled = true;
                    // make criteria group enable = false;
                    grpCriteria.Enabled = false;
                    nudMiniCommCriteria.Value = 0;
                    cmbBasedOnCriteria.Text = C_COMBO_SELECT;

                    grpCriteria.Enabled = false;
                    nudMiniCommCriteria.Value = 0;
                    cmbBasedOnCriteria.Text = C_COMBO_SELECT;
                    //BindCriteriaGrid();

                    rdbtnParameters.Enabled = true;
                    rdbtnCriteria.Enabled = true;

                }
                else
                {
                    rdbtnParameters.Checked = true;
                    grpParameters.Enabled = false;
                    grpCriteria.Enabled = false;
                    rdbtnParameters.Enabled = false;
                    rdbtnCriteria.Enabled = false;

                    nudCommissionRateParameter.Value = 0;
                    nudMiniCommParameter.Value = 0;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Handles the ValueChanged event of the cmbBasedOnCriteria control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cmbBasedOnCriteria_ValueChanged(object sender, System.EventArgs e)
        {
            UpdateCommissionRateUnitForCriterias();
        }

        /// <summary>
        /// Updates the commission rate unit for criterias.
        /// </summary>
        private void UpdateCommissionRateUnitForCriterias()
        {
            try
            {
                int selectedValue = Convert.ToInt32(cmbBasedOnCriteria.Value);
                string rateUnit = GetRateUnitByValue(selectedValue);
                List<ClearingFeeCriteria> commissionCriterias = (List<ClearingFeeCriteria>)grdClearingFeeRules.DataSource;
                if (commissionCriterias == null)
                {
                    return;
                }
                foreach (ClearingFeeCriteria criteria in commissionCriterias)
                {
                    criteria.ClearingFeeCriteriaUnit = rateUnit;
                }
                grdClearingFeeRules.Refresh();
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Sets the details.
        /// </summary>
        /// <param name="clearingFee">The clearing fee.</param>
        public void SetDetails(Prana.BusinessObjects.ClearingFee clearingFee)
        {
            try
            {
                ((ctrlCommissionRule)(this.Parent)).IsUnSavedChanges = true;
                if (clearingFee.IsCriteriaApplied)
                {
                    rdbtnCriteria.Checked = true;
                    cmbBasedOnCriteria.Value = clearingFee.RuleAppliedOn;
                    nudMiniCommCriteria.Value = decimal.Parse(clearingFee.MinClearingFee.ToString());
                    grdClearingFeeRules.DataSource = clearingFee.ClearingFeeRuleCriteiaList;
                    UpdateCommissionRateUnitForCriterias();

                    // make Parameter section default
                    rdbtnParameters.Checked = false;
                    cmbBasedOnParameter.Text = C_COMBO_SELECT;
                    nudCommissionRateParameter.Value = 0;
                    nudMiniCommParameter.Value = 0;
                }
                else
                {
                    rdbtnParameters.Checked = true;
                    cmbBasedOnParameter.Value = clearingFee.RuleAppliedOn;
                    nudCommissionRateParameter.Value = decimal.Parse(clearingFee.ClearingFeeRate.ToString());
                    nudMiniCommParameter.Value = decimal.Parse(clearingFee.MinClearingFee.ToString());

                    //make criteria section default
                    rdbtnCriteria.Checked = false;
                    cmbBasedOnCriteria.Text = C_COMBO_SELECT;
                    nudMiniCommCriteria.Value = 0;
                    BindCriteriaGrid();
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

        }

        /// <summary>
        /// Handles the Event event of the ValueChanged control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ValueChanged_Event(object sender, System.EventArgs e)
        {
            ((ctrlCommissionRule)(this.Parent)).IsUnSavedChanges = true;
        }

        /// <summary>
        /// Adds the new clearing fee on UI.
        /// </summary>
        /// <param name="clearingFee">The clearing fee.</param>
        public void AddNewClearingFeeOnUI(ClearingFee clearingFee)
        {
            try
            {
                clearingFee.IsCriteriaApplied = false;
                clearingFee.RuleAppliedOn = (CalculationBasis)cmbBasedOnParameter.Value;
                clearingFee.ClearingFeeRate = double.Parse(nudCommissionRateParameter.Value.ToString());
                clearingFee.MinClearingFee = double.Parse(nudMiniCommParameter.Value.ToString());
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <param name="clearingFee">The clearing fee.</param>
        /// <param name="IsClearingFeeApplied">if set to <c>true</c> [is clearing fee applied].</param>
        public void GetDetails(Prana.BusinessObjects.ClearingFee clearingFee, bool IsClearingFeeApplied)
        {
            try
            {
                if (IsClearingFeeApplied)
                {
                    if (rdbtnCriteria.Checked)
                    {
                        clearingFee.RuleAppliedOn = (CalculationBasis)cmbBasedOnCriteria.Value;
                        clearingFee.IsCriteriaApplied = true;
                        clearingFee.MinClearingFee = double.Parse(nudMiniCommCriteria.Value.ToString());
                        clearingFee.ClearingFeeRuleCriteiaList = (List<ClearingFeeCriteria>)grdClearingFeeRules.DataSource;

                        clearingFee.ClearingFeeRate = 0;
                    }
                    else if (rdbtnParameters.Checked)
                    {
                        clearingFee.IsCriteriaApplied = false;
                        clearingFee.RuleAppliedOn = (CalculationBasis)cmbBasedOnParameter.Value;
                        clearingFee.ClearingFeeRate = double.Parse(nudCommissionRateParameter.Value.ToString());
                        clearingFee.MinClearingFee = double.Parse(nudMiniCommParameter.Value.ToString());
                        if (clearingFee.ClearingFeeRuleCriteiaList != null)
                            clearingFee.ClearingFeeRuleCriteiaList.Clear();
                    }
                }
                else
                {
                    clearingFee.RuleAppliedOn = CalculationBasis.Shares;
                    clearingFee.ClearingFeeRate = double.Parse(nudCommissionRateParameter.Value.ToString());
                    clearingFee.MinClearingFee = double.Parse(nudMiniCommParameter.Value.ToString());
                    if (clearingFee.ClearingFeeRuleCriteiaList != null)
                        clearingFee.ClearingFeeRuleCriteiaList.Clear();
                    clearingFee.IsCriteriaApplied = false;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Handles the ValueChanged event of the cmbBasedOnClearingFeeParameter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void cmbBasedOnClearingFeeParameter_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBasedOnParameter.Value != null)
                {
                    if (cmbBasedOnParameter.Text.Trim().Equals(CalculationBasis.Shares.ToString()))
                    {
                        lbldisplayClearingFee.Text = "Per Share";
                    }
                    else if (cmbBasedOnParameter.Text.Trim().Equals(CalculationBasis.Contracts.ToString()))
                    {
                        lbldisplayClearingFee.Text = "Per contract";
                    }
                    else if (cmbBasedOnParameter.Text.Trim().Equals(CalculationBasis.Notional.ToString()))
                    {
                        lbldisplayClearingFee.Text = "Basis Points";
                    }
                    else if (cmbBasedOnParameter.Text.Trim().Equals(CalculationBasis.Commission.ToString()))
                    {
                        lbldisplayClearingFee.Text = "Basis Points";
                    }
                    else if (cmbBasedOnParameter.Text.Trim().Equals(CalculationBasis.SoftCommission.ToString()))
                    {
                        lbldisplayClearingFee.Text = "Basis Points";
                    }
                    else if (cmbBasedOnParameter.Text.Trim().Equals(CalculationBasis.FlatAmount.ToString()))
                    {
                        lbldisplayClearingFee.Text = "Per Trade/Taxlot";
                    }
                    else
                    {
                        lbldisplayClearingFee.Text = "";
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Checks the validation.
        /// </summary>
        /// <param name="errorProvider1">The error provider1.</param>
        /// <returns></returns>
        public Boolean CheckValidation(ErrorProvider errorProvider1)
        {
            try
            {
                errorProvider1.SetError(cmbBasedOnParameter, "");
                errorProvider1.SetError(nudCommissionRateParameter, "");

                errorProvider1.SetError(cmbBasedOnCriteria, "");

                if (rdbtnParameters.Checked == true)
                {
                    if ((int)cmbBasedOnParameter.Value == int.MinValue)
                    {
                        errorProvider1.SetError(cmbBasedOnParameter, "Please select Fee Based on Parameter.");
                        cmbBasedOnParameter.Focus();
                        return false;
                    }
                    else if (nudCommissionRateParameter.Value < 0)
                    {
                        errorProvider1.SetError(nudCommissionRateParameter, "Please select Commission Based on Parameter.");
                        nudCommissionRateParameter.Focus();
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
                    else if (grdClearingFeeRules.Rows.Count <= 1)
                    {
                        MessageBox.Show("Please enter atleast one Fee Rule Criteria.", "Prana Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        grdClearingFeeRules.Focus();
                        return false;
                    }
                    else if (grdClearingFeeRules.Rows.Count > 1)
                    {
                        int rowcount = grdClearingFeeRules.Rows.Count;
                        // Validation to check whether any field in not blank              
                        foreach (UltraGridRow dr in grdClearingFeeRules.Rows)
                        {
                            double valuefrom = Convert.ToDouble(dr.Cells["ValueGreaterThan"].Value.ToString());
                            double valueto = Convert.ToDouble(dr.Cells["ValueLessThanOrEqual"].Value.ToString());
                            double commissionrate = double.Parse(dr.Cells["ClearingFeeRate"].Value.ToString());

                            if (rowcount != index)
                            {
                                if (valueto > 0 || commissionrate <= 0)
                                {
                                    if (commissionrate <= 0)
                                    {
                                        MessageBox.Show("Please enter the Fee Rate in the row : " + index, "Nirvane Admin");
                                        grdClearingFeeRules.Focus();
                                        return false;
                                    }
                                }
                                if (valuefrom >= valueto)
                                {
                                    MessageBox.Show("' Value To(<=) ' should be greater than ' Value From(>) ' in the row : " + index, "Nirvane Admin");
                                    grdClearingFeeRules.Focus();
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
                                        grdClearingFeeRules.Focus();
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return false;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rdbtnParameters control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void rdbtnParameters_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ((ctrlCommissionRule)(this.Parent)).IsUnSavedChanges = true;
                if (rdbtnParameters.Checked == false)
                {
                    grpParameters.Enabled = false;
                    cmbBasedOnParameter.Text = C_COMBO_SELECT;
                    nudCommissionRateParameter.Value = 0;
                    nudMiniCommParameter.Value = 0;
                }
                else if (rdbtnParameters.Checked == true)
                {
                    grpParameters.Enabled = true;
                    // make criteria group enable = false;
                    grpCriteria.Enabled = false;
                    nudMiniCommCriteria.Value = 0;
                    cmbBasedOnCriteria.Text = C_COMBO_SELECT;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rdbtnCriteria control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void rdbtnCriteria_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ((ctrlCommissionRule)(this.Parent)).IsUnSavedChanges = true;
                if (rdbtnCriteria.Checked == false)
                {
                    grpCriteria.Enabled = false;
                    nudMiniCommCriteria.Value = 0;
                    cmbBasedOnCriteria.Text = C_COMBO_SELECT;
                    BindCriteriaGrid();
                }
                else if (rdbtnCriteria.Checked == true)
                {
                    grpCriteria.Enabled = true;
                    // make Parameter group box enable=false and reset all the control of that group box
                    grpParameters.Enabled = false;
                    cmbBasedOnParameter.Text = C_COMBO_SELECT;
                    nudCommissionRateParameter.Value = 0;
                    nudMiniCommParameter.Value = 0;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Binds the criteria grid.
        /// </summary>
        private void BindCriteriaGrid()
        {
            try
            {
                List<ClearingFeeCriteria> clearingFeeCriterias = new List<ClearingFeeCriteria>();
                Guid dummyRuleID = System.Guid.NewGuid();
                clearingFeeCriterias = CommissionDBManager.GetClearingCriterias(dummyRuleID);
                grdClearingFeeRules.DataSource = clearingFeeCriterias;
                AddNewTempRow(_clearingFeeType);
                InitCriteriaGrid();
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Adds the new temporary row.
        /// </summary>
        /// <param name="clearingFeeType">Type of the clearing fee.</param>
        private void AddNewTempRow(ClearingFeeType clearingFeeType)
        {
            try
            {
                List<ClearingFeeCriteria> clearingFeeCriteriaList = new List<ClearingFeeCriteria>();

                ClearingFeeCriteria commrulecri = new ClearingFeeCriteria();
                commrulecri.ClearingFeeCriteriaId = int.MinValue;
                commrulecri.ClearingFeeRate = 0;
                commrulecri.ValueGreaterThan = 0;
                commrulecri.ValueLessThanOrEqual = 0;
                commrulecri.ClearingFeeType = clearingFeeType;
                commrulecri.ClearingFeeCriteriaUnit = GetRateUnitByValue(Convert.ToInt32(cmbBasedOnCriteria.Value));
                clearingFeeCriteriaList.Add(commrulecri);
                grdClearingFeeRules.DataSource = clearingFeeCriteriaList;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Gets the rate unit by value.
        /// </summary>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns></returns>
        private string GetRateUnitByValue(int selectedValue)
        {
            try
            {
                CalculationBasis criteria = (CalculationBasis)selectedValue;
                switch (criteria)
                {
                    case CalculationBasis.Shares:
                        return "Per Share";
                    case CalculationBasis.Notional:
                    case CalculationBasis.Commission:
                    case CalculationBasis.SoftCommission:
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
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return string.Empty;
        }

        /// <summary>
        /// Initializes the criteria grid.
        /// </summary>
        private void InitCriteriaGrid()
        {
            try
            {
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].Header.VisiblePosition = 0;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].Header.Caption = "Value From(>)";
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].MaxLength = 14;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].Width = 105;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ValueGreaterThan"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].Header.VisiblePosition = 1;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].MaxLength = 14;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].Header.Caption = "Value To(<=)";
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].Width = 105;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ValueLessThanOrEqual"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeRate"].Header.VisiblePosition = 2;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeRate"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeRate"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeRate"].Format = "##,###.0000";
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeRate"].MaxLength = 10;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeRate"].Width = 105;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeRate"].Header.Caption = "Commission Rate";
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeRate"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeRate"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeCriteriaUnit"].Header.VisiblePosition = 4;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeCriteriaUnit"].Width = 75;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeCriteriaUnit"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeCriteriaUnit"].Header.Caption = "Rate Unit";
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeCriteriaUnit"].CellActivation = Activation.NoEdit;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Header.VisiblePosition = 4;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Width = 40;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["DeleteButton"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeCriteriaID"].Header.VisiblePosition = 5;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeCriteriaID"].Hidden = true;

                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeType"].Header.VisiblePosition = 6;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeType"].Header.Caption = "Commission Type";
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeType"].CellActivation = Activation.Disabled;
                grdClearingFeeRules.DisplayLayout.Bands[0].Columns["ClearingFeeType"].Hidden = true;
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Handles the ClickCellButton event of the grdCommissionRules control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void grdCommissionRules_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                grdClearingFeeRules.UpdateData();
                if (grdClearingFeeRules.Rows.Count == 1)
                {
                    // MessageBox.Show("Nothing to delete.", "Prana Alter");
                    return;
                }
                if (e.Cell.Column.Key.Equals("DeleteButton"))
                {
                    if (Convert.ToDouble(grdClearingFeeRules.ActiveRow.Cells["ValueLessThanOrEqual"].Value.ToString()) > 0 && Convert.ToDouble(grdClearingFeeRules.ActiveRow.Cells["ValueGreaterThan"].Value.ToString()) > 0)
                    {
                        int activeRowIndex = grdClearingFeeRules.ActiveRow.Index;
                        double activeRowValueLessThanOrEqual = Convert.ToDouble(grdClearingFeeRules.ActiveRow.Cells["ValueLessThanOrEqual"].Value.ToString());
                        bool isDeleted = grdClearingFeeRules.ActiveRow.Delete();
                        if (isDeleted)
                        {
                            UltraGridRow previousDatarow = grdClearingFeeRules.Rows[activeRowIndex - 1];
                            previousDatarow.Cells["ValueLessThanOrEqual"].Value = activeRowValueLessThanOrEqual;
                        }
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Handles the Error event of the grdCommissionRules control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ErrorEventArgs"/> instance containing the event data.</param>
        private void grdCommissionRules_Error(object sender, ErrorEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// Handles the CellChange event of the grdCommissionRules control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void grdCommissionRules_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                ((ctrlCommissionRule)(this.Parent)).IsUnSavedChanges = true;
                if (e.Cell.Column.Key.Equals("ValueLessThanOrEqual") || e.Cell.Column.Key.Equals("ClearingFeeRate"))
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
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Handles the AfterCellUpdate event of the grdCommissionRules control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellEventArgs"/> instance containing the event data.</param>
        private void grdCommissionRules_AfterCellUpdate(object sender, CellEventArgs e)
        {
            AddNewRow();
        }

        /// <summary>
        /// Adds the new row.
        /// </summary>
        private void AddNewRow()
        {
            try
            {
                UltraGridCell prevActiveCell = grdClearingFeeRules.Rows[0].Cells[0];
                string cellText = string.Empty;
                int len = int.MinValue;
                if (grdClearingFeeRules.ActiveCell != null)
                {
                    prevActiveCell = grdClearingFeeRules.ActiveCell;
                    cellText = prevActiveCell.Text;
                    len = cellText.Length;
                }

                UltraGridRow activeRow = grdClearingFeeRules.ActiveRow;
                double valueLessThanOrEqual = 0;
                if (activeRow != null)
                {
                    valueLessThanOrEqual = Convert.ToDouble(activeRow.Cells["ValueLessThanOrEqual"].Value.ToString());
                }
                int rowsCount = grdClearingFeeRules.Rows.Count;
                UltraGridRow dr = grdClearingFeeRules.Rows[rowsCount - 1];

                List<ClearingFeeCriteria> commrulecris = (List<ClearingFeeCriteria>)grdClearingFeeRules.DataSource;
                ClearingFeeCriteria commrulecri = new ClearingFeeCriteria();

                //The below varriables are taken from the last row of the grid before adding the new row.
                double dblValueFrom = Convert.ToDouble(dr.Cells["ValueGreaterThan"].Value.ToString());
                double dblValueTo = Convert.ToDouble(dr.Cells["ValueLessThanOrEqual"].Value.ToString());
                double CommRate = Convert.ToDouble(dr.Cells["ClearingFeeRate"].Value.ToString());
                string commissionRateUnit = dr.Cells["ClearingFeeCriteriaUnit"].Value.ToString();

                Convert.ToInt32(cmbBasedOnCriteria.Value);

                //Add one more condition to avoid adding 2nd new row when the below condition fullfills.
                if (dblValueTo > 0)
                {
                    commrulecri.ClearingFeeCriteriaId = int.MinValue;
                    commrulecri.ClearingFeeRate = 0;
                    commrulecri.ValueGreaterThan = 0;
                    commrulecri.ValueLessThanOrEqual = 0;
                    commrulecri.ClearingFeeCriteriaUnit = commissionRateUnit;
                    commrulecri.ClearingFeeType = _clearingFeeType;
                    commrulecris.Add(commrulecri);
                    grdClearingFeeRules.DataSource = commrulecris;
                    grdClearingFeeRules.DataBind();
                    grdClearingFeeRules.ActiveCell = prevActiveCell;
                    grdClearingFeeRules.Focus();
                    grdClearingFeeRules.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                }

                grdClearingFeeRules.UpdateData();
                if (grdClearingFeeRules.ActiveRow != null)
                {
                    int rowindex = grdClearingFeeRules.ActiveRow.Index;
                    int rowsC = grdClearingFeeRules.Rows.Count;
                    // assign value to next cloumn
                    if ((rowindex + 1) != rowsC)
                    {
                        UltraGridRow newDatarow = grdClearingFeeRules.Rows[rowindex + 1];
                        newDatarow.Cells["ValueGreaterThan"].Value = valueLessThanOrEqual;
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }
    }
}
