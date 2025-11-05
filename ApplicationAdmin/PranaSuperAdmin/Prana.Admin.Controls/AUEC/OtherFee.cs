using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    public partial class OtherFee : UserControl
    {
        public OtherFee()
        {
            InitializeComponent();
        }

        private const string BASISPERSHARE = "Per Share";
        private const string BASISPERCONTRACT = "Per Contract";
        private const string BASISPOINT = "Basis Point";
        private const string FLATAMOUNT = "Per Trade/Taxlot";

        public void SetupControl()
        {
            try
            {
                BindFeeBasisCombos();
                grpBoxOtherFee.Text = OrderFields.FeeNamesCollection[OtherFeeType.ToString()];
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void BindCriteriaGrid()
        {
            try
            {
                grdCriteriaBuyTrades.DataSource = AddNewTempRowForBuyGrid();
                grdCriteriaSellTrades.DataSource = AddNewTempRowForSellGrid();
                InitCriteriaGrids(grdCriteriaBuyTrades, true);
                InitCriteriaGrids(grdCriteriaSellTrades, false);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private List<OtherFeesCriteria> AddNewTempRowForBuyGrid()
        {
            List<OtherFeesCriteria> otherFeesCriteriacolls = new List<OtherFeesCriteria>();
            try
            {
                OtherFeesCriteria otherFeescri = new OtherFeesCriteria();
                otherFeescri.OtherFeesCriteriaId = 0;
                otherFeescri.LongFeeRate = 0;
                otherFeescri.LongValueGreaterThan = 0;
                otherFeescri.LongValueLessThanOrEqual = 0;
                otherFeescri.LongFeeCriteriaUnit = GetRateUnitByValue(Convert.ToInt32(cmbBasedOnBuyCriteria.Value));
                otherFeescri.LongCalculationBasis = (int)cmbBasedOnBuyCriteria.Value;
                otherFeesCriteriacolls.Add(otherFeescri);
            }
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
            return otherFeesCriteriacolls;
        }

        private List<OtherFeesCriteria> AddNewTempRowForSellGrid()
        {
            List<OtherFeesCriteria> otherFeesCriteriacolls = new List<OtherFeesCriteria>();
            try
            {
                OtherFeesCriteria otherFeescri = new OtherFeesCriteria();
                otherFeescri.OtherFeesCriteriaId = 0;
                otherFeescri.ShortFeeRate = 0;
                otherFeescri.ShortValueGreaterThan = 0;
                otherFeescri.ShortValueLessThanOrEqual = 0;
                otherFeescri.ShortFeeCriteriaUnit = GetRateUnitByValue(Convert.ToInt32(cmbBasedOnSellCriteria.Value));
                otherFeescri.ShortCalculationBasis = (int)cmbBasedOnSellCriteria.Value;
                otherFeesCriteriacolls.Add(otherFeescri);
            }
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
            return otherFeesCriteriacolls;
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

        private void InitCriteriaGrids(UltraGrid grd, bool isLong)
        {
            try
            {
                string Long = isLong ? "Long" : "Short";
                string Sort = isLong ? "Short" : "Long";
                grd.DisplayLayout.Bands[0].Columns[Long + "ValueGreaterThan"].Header.VisiblePosition = 0;
                grd.DisplayLayout.Bands[0].Columns[Long + "ValueGreaterThan"].Header.Caption = "Value From(>)";
                grd.DisplayLayout.Bands[0].Columns[Long + "ValueGreaterThan"].MaxLength = 14;
                grd.DisplayLayout.Bands[0].Columns[Long + "ValueGreaterThan"].Width = 105;
                grd.DisplayLayout.Bands[0].Columns[Long + "ValueGreaterThan"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grd.DisplayLayout.Bands[0].Columns[Long + "ValueGreaterThan"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                grd.DisplayLayout.Bands[0].Columns[Long + "ValueLessThanOrEqual"].Header.VisiblePosition = 1;
                grd.DisplayLayout.Bands[0].Columns[Long + "ValueLessThanOrEqual"].MaxLength = 14;
                grd.DisplayLayout.Bands[0].Columns[Long + "ValueLessThanOrEqual"].Header.Caption = "Value To(<=)";
                grd.DisplayLayout.Bands[0].Columns[Long + "ValueLessThanOrEqual"].Width = 105;
                grd.DisplayLayout.Bands[0].Columns[Long + "ValueLessThanOrEqual"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grd.DisplayLayout.Bands[0].Columns[Long + "ValueLessThanOrEqual"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                grd.DisplayLayout.Bands[0].Columns[Long + "FeeRate"].Header.VisiblePosition = 2;
                grd.DisplayLayout.Bands[0].Columns[Long + "FeeRate"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
                grd.DisplayLayout.Bands[0].Columns[Long + "FeeRate"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                grd.DisplayLayout.Bands[0].Columns[Long + "FeeRate"].Format = "##,###.0000";
                grd.DisplayLayout.Bands[0].Columns[Long + "FeeRate"].MaxLength = 10;
                grd.DisplayLayout.Bands[0].Columns[Long + "FeeRate"].Width = 105;
                grd.DisplayLayout.Bands[0].Columns[Long + "FeeRate"].Header.Caption = "Fee Rate";
                grd.DisplayLayout.Bands[0].Columns[Long + "FeeRate"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grd.DisplayLayout.Bands[0].Columns[Long + "FeeRate"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                grd.DisplayLayout.Bands[0].Columns[Long + "FeeCriteriaUnit"].Header.VisiblePosition = 3;
                grd.DisplayLayout.Bands[0].Columns[Long + "FeeCriteriaUnit"].Width = 75;
                grd.DisplayLayout.Bands[0].Columns[Long + "FeeCriteriaUnit"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grd.DisplayLayout.Bands[0].Columns[Long + "FeeCriteriaUnit"].Header.Caption = "Rate Unit";
                grd.DisplayLayout.Bands[0].Columns[Long + "FeeCriteriaUnit"].CellActivation = Activation.NoEdit;

                grd.DisplayLayout.Bands[0].Columns["DeleteButton"].Header.VisiblePosition = 4;
                grd.DisplayLayout.Bands[0].Columns["DeleteButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                grd.DisplayLayout.Bands[0].Columns["DeleteButton"].Width = 40;
                grd.DisplayLayout.Bands[0].Columns["DeleteButton"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;

                grd.DisplayLayout.Bands[0].Columns["OtherFeesCriteriaID"].Header.VisiblePosition = 5;
                grd.DisplayLayout.Bands[0].Columns["OtherFeesCriteriaID"].CellActivation = Activation.Disabled;
                grd.DisplayLayout.Bands[0].Columns["OtherFeesCriteriaID"].Hidden = true;

                grd.DisplayLayout.Bands[0].Columns[Sort + "ValueGreaterThan"].Hidden = true;
                grd.DisplayLayout.Bands[0].Columns[Sort + "ValueLessThanOrEqual"].Hidden = true;
                grd.DisplayLayout.Bands[0].Columns[Sort + "FeeRate"].Hidden = true;
                grd.DisplayLayout.Bands[0].Columns[Sort + "FeeCriteriaUnit"].Hidden = true;
                grd.DisplayLayout.Bands[0].Columns["LongCalculationBasis"].Hidden = true;
                grd.DisplayLayout.Bands[0].Columns["ShortCalculationBasis"].Hidden = true;
            }
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
        }

        private void BindFeeBasisCombos()
        {
            try
            {
                rdbtnParameters.Checked = true;
                parameterGroupBox.Enabled = true;
                rdbtnCriteria.Checked = false;
                grpCriteria.Enabled = false;

                chkEditorPositionType.Checked = false;
                chkEditorRoundOff.Checked = true;
                chkEditorMaxValue.Checked = true;
                chkEditorMinValue.Checked = true;

                EnumerationValueList listForCalculationBasis = CommissionEnumHelper.GetNewListForCalculationBasis(OtherFeeType);

                this.cmbBuyBasis.DataSource = null;
                this.cmbBuyBasis.DataSource = listForCalculationBasis;
                this.cmbBuyBasis.DisplayMember = "DisplayText";
                this.cmbBuyBasis.ValueMember = "Value";
                this.cmbBuyBasis.Value = int.MinValue;
                this.cmbBuyBasis.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                this.cmbSellBasis.DataSource = null;
                this.cmbSellBasis.DataSource = listForCalculationBasis;
                this.cmbSellBasis.DisplayMember = "DisplayText";
                this.cmbSellBasis.ValueMember = "Value";
                this.cmbSellBasis.Value = int.MinValue;
                this.cmbSellBasis.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                // bind Based on Criteria combo
                listForCalculationBasis = CommissionEnumHelper.GetCommissionCriterias();
                cmbBasedOnBuyCriteria.DataSource = null;
                cmbBasedOnBuyCriteria.DataSource = listForCalculationBasis;
                cmbBasedOnBuyCriteria.DisplayMember = "DisplayText";
                cmbBasedOnBuyCriteria.ValueMember = "Value";
                Utils.UltraComboFilter(cmbBasedOnBuyCriteria, "DisplayText");
                cmbBasedOnBuyCriteria.Value = int.MinValue;


                cmbBasedOnSellCriteria.DataSource = null;
                cmbBasedOnSellCriteria.DataSource = listForCalculationBasis;
                cmbBasedOnSellCriteria.DisplayMember = "DisplayText";
                cmbBasedOnSellCriteria.ValueMember = "Value";
                Utils.UltraComboFilter(cmbBasedOnSellCriteria, "DisplayText");
                cmbBasedOnSellCriteria.Value = int.MinValue;
            }
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

        }

        public void BindControlWithAUECID(int auecID, OtherFeeRule otherFeeRule)
        {
            numUpDwnRoundOff.Value = otherFeeRule.RoundOffPrecision;
            numUpDwnMaxValue.Value = decimal.Parse(otherFeeRule.MaxValue.ToString());
            numUpDwnMinValue.Value = decimal.Parse(otherFeeRule.MinValue.ToString());
            numUpDwnRoundDown.Value = otherFeeRule.RoundDownPrecision;
            numUpDwnRoundUp.Value = otherFeeRule.RoundUpPrecision;
            rdbtnCriteria.Checked = otherFeeRule.IsCriteriaApplied;
            rdbtnParameters.Checked = !otherFeeRule.IsCriteriaApplied;
            if (otherFeeRule.IsCriteriaApplied)
            {
                BindCriteriaGrid();
                grdCriteriaBuyTrades.DataSource = ValidateCriteriaList(otherFeeRule.LongFeeRuleCriteriaList, true);
                cmbBasedOnBuyCriteria.Value = (CalculationBasis)otherFeeRule.LongFeeRuleCriteriaList[0].LongCalculationBasis;
                grdCriteriaSellTrades.DataSource = ValidateCriteriaList(otherFeeRule.ShortFeeRuleCriteriaList, false);
                cmbBasedOnSellCriteria.Value = (CalculationBasis)otherFeeRule.ShortFeeRuleCriteriaList[0].ShortCalculationBasis;
            }
            else
            {
                cmbBuyBasis.Value = otherFeeRule.LongCalculationBasis;
                numUpDwnBuyCommissionRate.Value = decimal.Parse(otherFeeRule.LongRate.ToString());
                cmbSellBasis.Value = otherFeeRule.ShortCalculationBasis;
                numUpDwnSellCommissionRate.Value = decimal.Parse(otherFeeRule.ShortRate.ToString());
            }
            switch (otherFeeRule.FeePrecisionType)
            {
                case FeePrecisionType.RoundUp:
                    chkEditorRoundUp.Checked = true;
                    chkEditorRoundOff.Checked = false;
                    chkEditorRoundDown.Checked = false;
                    break;
                case FeePrecisionType.RoundOff:
                    chkEditorRoundUp.Checked = false;
                    chkEditorRoundOff.Checked = true;
                    chkEditorRoundDown.Checked = false;
                    break;
                case FeePrecisionType.RoundDown:
                    chkEditorRoundUp.Checked = false;
                    chkEditorRoundOff.Checked = false;
                    chkEditorRoundDown.Checked = true;
                    break;
            }
        }

        private List<OtherFeesCriteria> ValidateCriteriaList(List<OtherFeesCriteria> CriteriaList, bool longorshort)
        {
            List<OtherFeesCriteria> feeCriteriaList = new List<OtherFeesCriteria>();
            try
            {
                if (chkbxBuySell.Checked.Equals(true))
                    return CriteriaList;
                if (longorshort)
                {
                    foreach (OtherFeesCriteria criteria in CriteriaList)
                    {
                        if (!(criteria.LongValueGreaterThan == 0 && criteria.LongValueLessThanOrEqual == 0))
                        {
                            feeCriteriaList.Add(criteria);
                        }
                        else
                            break;
                    }
                }
                else
                {
                    foreach (OtherFeesCriteria criteria in CriteriaList)
                    {
                        if (!(criteria.ShortValueGreaterThan == 0 && criteria.ShortValueLessThanOrEqual == 0))
                        {
                            feeCriteriaList.Add(criteria);
                        }
                        else
                            break;
                    }
                }
            }
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
            return feeCriteriaList;
        }

        public void ResetControl(int auecID)
        {
            cmbBuyBasis.Text = ApplicationConstants.C_COMBO_SELECT;
            numUpDwnBuyCommissionRate.Value = 0;
            cmbSellBasis.Text = ApplicationConstants.C_COMBO_SELECT;
            numUpDwnSellCommissionRate.Value = 0;
            numUpDwnRoundOff.Value = 2;
            numUpDwnMaxValue.Value = 0;
            numUpDwnMinValue.Value = 0;
            rdbtnParameters.Checked = true;
        }

        public OtherFeeRule GetValidatedOtherFeeRule()
        {
            OtherFeeRule otherFeeRule = new OtherFeeRule();
            otherFeeRule.OtherFeeType = (OtherFeeType)_otherFeeType;
            if (rdbtnParameters.Checked.Equals(true))
            {
                otherFeeRule.IsCriteriaApplied = false;
                if (chkEditorPositionType.Checked.Equals(true))
                {
                    cmbSellBasis.Value = cmbBuyBasis.Value;
                    numUpDwnSellCommissionRate.Value = numUpDwnBuyCommissionRate.Value;
                }
                otherFeeRule.LongCalculationBasis = (CalculationFeeBasis)cmbBuyBasis.Value;
                otherFeeRule.ShortCalculationBasis = (CalculationFeeBasis)cmbSellBasis.Value;
                otherFeeRule.LongRate = Convert.ToDouble(numUpDwnBuyCommissionRate.Value);
                otherFeeRule.ShortRate = Convert.ToDouble(numUpDwnSellCommissionRate.Value);
                if (!(cmbBuyBasis.Text.ToString().Equals(ApplicationConstants.C_COMBO_SELECT)) && cmbSellBasis.Text.ToString().Equals(ApplicationConstants.C_COMBO_SELECT))
                {
                    MessageBox.Show("Please give details for sell rate trades.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    otherFeeRule = null;
                    return otherFeeRule;
                }
                if (!(cmbSellBasis.Text.ToString().Equals(ApplicationConstants.C_COMBO_SELECT)) && cmbBuyBasis.Text.ToString().Equals(ApplicationConstants.C_COMBO_SELECT))
                {
                    MessageBox.Show("Please give details for buy rate trades.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    otherFeeRule = null;
                    return otherFeeRule;
                }
            }
            else
            {
                otherFeeRule.IsCriteriaApplied = true;

                if (chkbxBuySell.Checked.Equals(true))
                {
                    cmbBasedOnSellCriteria.Value = cmbBasedOnBuyCriteria.Value;
                    otherFeeRule.LongFeeRuleCriteriaList = (List<OtherFeesCriteria>)grdCriteriaBuyTrades.DataSource;
                    otherFeeRule.ShortFeeRuleCriteriaList = (List<OtherFeesCriteria>)grdCriteriaBuyTrades.DataSource;
                    UpdateShortCriteriaBasedOnLong(otherFeeRule);
                }
                else
                {
                    otherFeeRule.LongFeeRuleCriteriaList = (List<OtherFeesCriteria>)grdCriteriaBuyTrades.DataSource;
                    otherFeeRule.ShortFeeRuleCriteriaList = (List<OtherFeesCriteria>)grdCriteriaSellTrades.DataSource;
                }

                //otherFeeRule.LongCalculationBasis = (CalculationFeeBasis)cmbBasedOnBuyCriteria.Value;
                //otherFeeRule.ShortCalculationBasis = (CalculationFeeBasis)cmbBasedOnSellCriteria.Value;

                if (!(cmbBasedOnBuyCriteria.Text.ToString().Equals(ApplicationConstants.C_COMBO_SELECT)) && cmbBasedOnSellCriteria.Text.ToString().Equals(ApplicationConstants.C_COMBO_SELECT))
                {
                    MessageBox.Show("Please give criteria details for sell trades.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    otherFeeRule = null;
                    return otherFeeRule;
                }
                if (!(cmbBasedOnSellCriteria.Text.ToString().Equals(ApplicationConstants.C_COMBO_SELECT)) && cmbBasedOnBuyCriteria.Text.ToString().Equals(ApplicationConstants.C_COMBO_SELECT))
                {
                    MessageBox.Show("Please give criteria details for buy trades.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    otherFeeRule = null;
                    return otherFeeRule;
                }
                if (chkbxBuySell.Checked.Equals(true) && !validateCriteriaGrids(grdCriteriaBuyTrades, true, false))
                {
                    otherFeeRule = null;
                    return otherFeeRule;
                }
                else if (chkbxBuySell.Checked.Equals(false) && (!validateCriteriaGrids(grdCriteriaBuyTrades, true, true) || !validateCriteriaGrids(grdCriteriaSellTrades, false, true)))
                {
                    otherFeeRule = null;
                    return otherFeeRule;
                }
            }

            otherFeeRule.MaxValue = Convert.ToDouble(numUpDwnMaxValue.Value);
            otherFeeRule.MinValue = Convert.ToDouble(numUpDwnMinValue.Value);
            if (chkEditorRoundDown.Checked.Equals(false) && chkEditorRoundOff.Checked.Equals(false) && chkEditorRoundUp.Checked.Equals(false))
            {
                otherFeeRule.RoundOffPrecision = 2;
            }
            else
            {
                otherFeeRule.RoundOffPrecision = Convert.ToInt16(numUpDwnRoundOff.Value);
            }

            otherFeeRule.RoundDownPrecision = Convert.ToInt32(numUpDwnRoundDown.Value);
            otherFeeRule.RoundUpPrecision = Convert.ToInt32(numUpDwnRoundUp.Value);

            if (chkEditorRoundUp.Checked.Equals(true))
                otherFeeRule.FeePrecisionType = FeePrecisionType.RoundUp;
            else if (chkEditorRoundDown.Checked.Equals(true))
                otherFeeRule.FeePrecisionType = FeePrecisionType.RoundDown;
            else
                otherFeeRule.FeePrecisionType = FeePrecisionType.RoundOff;

            return otherFeeRule;
        }

        private static bool validateCriteriaGrids(UltraGrid grd, bool longOrSort, bool updateId)
        {
            int index = 1;
            string Long = longOrSort ? "Long" : "Short";
            if (grd.Rows.Count <= 1)
            {
                MessageBox.Show("Please enter atleast one Criteria.", "Prana Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                grd.Focus();
                return false;
            }
            else if (grd.Rows.Count > 1)
            {
                int rowcount = grd.Rows.Count;
                // Validation to check whether any field in not blank              
                foreach (UltraGridRow dr in grd.Rows)
                {
                    double valuefrom = Convert.ToDouble(dr.Cells[Long + "ValueGreaterThan"].Value.ToString());
                    double valueto = Convert.ToDouble(dr.Cells[Long + "ValueLessThanOrEqual"].Value.ToString());
                    double commissionrate = double.Parse(dr.Cells[Long + "FeeRate"].Value.ToString());
                    if (updateId)
                    {
                        dr.Cells["OtherFeesCriteriaId"].Value = index - 1;
                    }
                    if (rowcount != index)
                    {
                        if (valueto > 0 || commissionrate <= 0)
                        {
                            if (commissionrate <= 0)
                            {
                                MessageBox.Show("Please enter the fee Rate in the row : " + index, "Nirvane Admin");
                                grd.Focus();
                                return false;
                            }
                        }
                        if (valuefrom >= valueto)
                        {
                            MessageBox.Show("' Value To(<=) ' should be greater than ' Value From(>) ' in the row : " + index, "Nirvane Admin");
                            grd.Focus();
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
                                MessageBox.Show("Please enter the fee Rate in the row : " + index, "Nirvane Admin");
                                grd.Focus();
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private static void UpdateShortCriteriaBasedOnLong(OtherFeeRule otherFeeRule)
        {
            foreach (OtherFeesCriteria criteria in otherFeeRule.ShortFeeRuleCriteriaList)
            {
                criteria.ShortValueGreaterThan = criteria.LongValueGreaterThan;
                criteria.ShortValueLessThanOrEqual = criteria.LongValueLessThanOrEqual;
                criteria.ShortFeeRate = criteria.LongFeeRate;
                criteria.ShortCalculationBasis = criteria.LongCalculationBasis;
                criteria.ShortFeeCriteriaUnit = criteria.LongFeeCriteriaUnit;
            }
        }

        public bool IsOtherFeeRuleEntered()
        {
            bool isOtherFeeRuleEntered = false;
            if (chkEditorPositionType.Checked.Equals(true))
            {
                cmbSellBasis.Value = cmbBuyBasis.Value;
                numUpDwnSellCommissionRate.Value = numUpDwnBuyCommissionRate.Value;
            }
            OtherFeeRule otherFeeRule = new OtherFeeRule();

            if (!(cmbBuyBasis.Text.ToString().Equals(ApplicationConstants.C_COMBO_SELECT)) || !(cmbSellBasis.Text.ToString().Equals(ApplicationConstants.C_COMBO_SELECT)) || !(cmbBasedOnBuyCriteria.Text.ToString().Equals(ApplicationConstants.C_COMBO_SELECT)) || !(cmbBasedOnSellCriteria.Text.ToString().Equals(ApplicationConstants.C_COMBO_SELECT)))
            {
                isOtherFeeRuleEntered = true;
            }
            return isOtherFeeRuleEntered;
        }

        private OtherFeeType _otherFeeType;
        public OtherFeeType OtherFeeType
        {
            set
            {
                _otherFeeType = value;
            }
            get
            {
                return _otherFeeType;
            }
        }

        private void chkEditorPositionType_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEditorPositionType.Checked.Equals(true))
            {
                lblBuyTrades.Visible = false;
                lblSellTrades.Visible = false;
                lblSellBasis.Visible = false;
                cmbSellBasis.Visible = false;
                lblSellRate.Visible = false;
                numUpDwnSellCommissionRate.Visible = false;
                lblSellSideBasisDisplay.Visible = false;
            }
            else
            {
                lblBuyTrades.Visible = true;
                lblSellTrades.Visible = true;
                lblSellBasis.Visible = true;
                cmbSellBasis.Visible = true;
                lblSellRate.Visible = true;
                numUpDwnSellCommissionRate.Visible = true;
                lblSellSideBasisDisplay.Visible = true;
            }
        }

        private void cmbBuyBasis_ValueChanged(object sender, EventArgs e)
        {
            CalculationFeeBasis calBuyBasis = (CalculationFeeBasis)cmbBuyBasis.Value;
            setLebelOnCalculationBasis(calBuyBasis, lblBuySideBasisDisplay);
        }

        private void setLebelOnCalculationBasis(CalculationFeeBasis calBasis, UltraLabel lbl)
        {
            switch (calBasis)
            {
                case CalculationFeeBasis.Shares:
                    lbl.Text = BASISPERSHARE;
                    break;
                case CalculationFeeBasis.Commission:
                    lbl.Text = BASISPOINT;
                    break;
                case CalculationFeeBasis.Contracts:
                    lbl.Text = BASISPERCONTRACT;
                    break;
                case CalculationFeeBasis.Notional:
                    lbl.Text = BASISPOINT;
                    break;
                case CalculationFeeBasis.NotionalPlusCommission:
                    lbl.Text = BASISPOINT;
                    break;
                case CalculationFeeBasis.FlatAmount:
                    lbl.Text = FLATAMOUNT;
                    break;
                case CalculationFeeBasis.ClearingBrokerFee:
                case CalculationFeeBasis.ClearingFee:
                case CalculationFeeBasis.MiscFees:
                case CalculationFeeBasis.OccFee:
                case CalculationFeeBasis.OrfFee:
                case CalculationFeeBasis.OtherBrokerFee:
                case CalculationFeeBasis.SecFee:
                case CalculationFeeBasis.SoftCommission:
                case CalculationFeeBasis.StampDuty:
                case CalculationFeeBasis.TransactionLevy:
                case CalculationFeeBasis.TaxOnCommissions:
                    lbl.Text = BASISPOINT;
                    break;

            }
        }

        private void cmbSellBasis_ValueChanged(object sender, EventArgs e)
        {
            CalculationFeeBasis calSellBasis = (CalculationFeeBasis)cmbSellBasis.Value;
            setLebelOnCalculationBasis(calSellBasis, lblSellSideBasisDisplay);
        }

        private void DisableAllnumUpDwn()
        {
            numUpDwnRoundUp.Enabled = false;
            numUpDwnRoundOff.Enabled = false;
            numUpDwnRoundDown.Enabled = false;
        }

        private void chkEditorRoundOff_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEditorRoundOff.Checked.Equals(true))
            {

                chkEditorRoundUp.Checked = false;
                chkEditorRoundDown.Checked = false;
                DisableAllnumUpDwn();
                numUpDwnRoundOff.Enabled = true;
            }
            else
            {
                numUpDwnRoundOff.Enabled = false;
            }
        }

        private void chkEditorMaxValue_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEditorMaxValue.Checked.Equals(true))
            {
                numUpDwnMaxValue.Enabled = true;
            }
            else
            {
                numUpDwnMaxValue.Value = 0;
                numUpDwnMaxValue.Enabled = false;
            }
        }

        private void chkEditorMinValue_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEditorMinValue.Checked.Equals(true))
            {
                numUpDwnMinValue.Enabled = true;
            }
            else
            {
                numUpDwnMinValue.Value = 0;
                numUpDwnMinValue.Enabled = false;
            }
        }

        private void chkEditorRoundUp_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkEditorRoundUp.Checked.Equals(true))
            {
                chkEditorRoundDown.Checked = false;
                chkEditorRoundOff.Checked = false;
                DisableAllnumUpDwn();
                numUpDwnRoundUp.Enabled = true;
            }
        }

        private void chkEditorRoundDown_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkEditorRoundDown.Checked.Equals(true))
            {
                chkEditorRoundOff.Checked = false;
                chkEditorRoundUp.Checked = false;
                DisableAllnumUpDwn();
                numUpDwnRoundDown.Enabled = true;
            }
        }

        private void rdbtnCriteria_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnCriteria.Checked == true)
            {
                grpCriteria.Enabled = true;
                BindCriteriaGrid();
                parameterGroupBox.Enabled = false;
            }
            else
            {
                grpCriteria.Enabled = false;
                parameterGroupBox.Enabled = true;
            }
        }

        private void rdbtnParameters_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnParameters.Checked == true)
            {
                grpCriteria.Enabled = false;
                parameterGroupBox.Enabled = true;
            }
            else
            {
                grpCriteria.Enabled = true;
                parameterGroupBox.Enabled = false;
            }
        }

        private void chkbxBuySell_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxBuySell.Checked.Equals(true))
            {
                lblRateBuyTrades.Visible = false;
                grpCriteriaSell.Visible = false;
            }
            else
            {
                lblRateBuyTrades.Visible = true;
                grpCriteriaSell.Visible = true;
                //  BindCriteriaGrid();
            }
        }

        private void cmbBasedOnBuyCriteria_ValueChanged(object sender, EventArgs e)
        {
            UpdateFeeRateUnitForCriterias(grdCriteriaBuyTrades, cmbBasedOnBuyCriteria, true);
        }

        private void cmbBasedOnSellCriteria_ValueChanged(object sender, EventArgs e)
        {
            UpdateFeeRateUnitForCriterias(grdCriteriaSellTrades, cmbBasedOnSellCriteria, false);
        }

        private void UpdateFeeRateUnitForCriterias(UltraGrid grd, UltraCombo cmb, bool LongOrShort)
        {
            int selectedValue = Convert.ToInt32(cmb.Value);
            string rateUnit = GetRateUnitByValue(selectedValue);
            List<OtherFeesCriteria> commissionCriterias = (List<OtherFeesCriteria>)grd.DataSource;
            if (commissionCriterias == null)
            {
                return;
            }
            foreach (OtherFeesCriteria criteria in commissionCriterias)
            {
                if (LongOrShort)
                {
                    criteria.LongFeeCriteriaUnit = rateUnit;
                    criteria.LongCalculationBasis = (int)cmb.Value;
                }
                else
                {
                    criteria.ShortFeeCriteriaUnit = rateUnit;
                    criteria.ShortCalculationBasis = (int)cmb.Value;
                }
            }
            grd.Refresh();
        }

        private void grdCriteriaBuyTrades_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                AddNewRow(grdCriteriaBuyTrades, true);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCriteriaBuyTrades_ClickCellButton(object sender, CellEventArgs e)
        {
            grdCriteriaBuyTrades.UpdateData();
            if (grdCriteriaBuyTrades.Rows.Count == 1)
            {
                // MessageBox.Show("Nothing to delete.", "Prana Alter");
                return;
            }
            if (e.Cell.Column.Key.Equals("DeleteButton"))
            {
                if (Convert.ToDouble(grdCriteriaBuyTrades.ActiveRow.Cells["LongValueLessThanOrEqual"].Value.ToString()) > 0 && Convert.ToDouble(grdCriteriaBuyTrades.ActiveRow.Cells["LongValueGreaterThan"].Value.ToString()) > 0)
                {
                    int activeRowIndex = grdCriteriaBuyTrades.ActiveRow.Index;
                    double activeRowValueLessThanOrEqual = Convert.ToDouble(grdCriteriaBuyTrades.ActiveRow.Cells["LongValueLessThanOrEqual"].Value.ToString());
                    bool isDeleted = grdCriteriaBuyTrades.ActiveRow.Delete();
                    if (isDeleted)
                    {
                        UltraGridRow previousDatarow = grdCriteriaBuyTrades.Rows[activeRowIndex - 1];
                        previousDatarow.Cells["LongValueLessThanOrEqual"].Value = activeRowValueLessThanOrEqual;
                    }
                }
            }
        }

        private void grdCriteriaBuyTrades_CellChange(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key.Equals("LongValueLessThanOrEqual") || e.Cell.Column.Key.Equals("LongFeeRate"))
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

        private void grdCriteriaSellTrades_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                AddNewRow(grdCriteriaSellTrades, false);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdCriteriaSellTrades_CellChange(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key.Equals("ShortValueLessThanOrEqual") || e.Cell.Column.Key.Equals("ShortFeeRate"))
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

        private void grdCriteriaSellTrades_ClickCellButton(object sender, CellEventArgs e)
        {
            grdCriteriaSellTrades.UpdateData();
            if (grdCriteriaSellTrades.Rows.Count == 1)
            {
                // MessageBox.Show("Nothing to delete.", "Prana Alter");
                return;
            }
            if (e.Cell.Column.Key.Equals("DeleteButton"))
            {
                if (Convert.ToDouble(grdCriteriaSellTrades.ActiveRow.Cells["ShortValueLessThanOrEqual"].Value.ToString()) > 0 && Convert.ToDouble(grdCriteriaSellTrades.ActiveRow.Cells["ShortValueGreaterThan"].Value.ToString()) > 0)
                {
                    int activeRowIndex = grdCriteriaSellTrades.ActiveRow.Index;
                    double activeRowValueLessThanOrEqual = Convert.ToDouble(grdCriteriaSellTrades.ActiveRow.Cells["ShortValueLessThanOrEqual"].Value.ToString());
                    bool isDeleted = grdCriteriaSellTrades.ActiveRow.Delete();
                    if (isDeleted)
                    {
                        UltraGridRow previousDatarow = grdCriteriaSellTrades.Rows[activeRowIndex - 1];
                        previousDatarow.Cells["ShortValueLessThanOrEqual"].Value = activeRowValueLessThanOrEqual;
                    }
                }
            }
        }


        private void AddNewRow(UltraGrid grd, bool longOrSort)
        {
            try
            {
                string Long = longOrSort ? "Long" : "Short";
                UltraGridCell prevActiveCell = grd.Rows[0].Cells[0];
                string cellText = string.Empty;
                int len = int.MinValue;
                if (grd.ActiveCell != null)
                {
                    prevActiveCell = grd.ActiveCell;
                    cellText = prevActiveCell.Text;
                    len = cellText.Length;
                }

                UltraGridRow activeRow = grd.ActiveRow;
                double valueLessThanOrEqual = 0;
                if (activeRow != null)
                {
                    valueLessThanOrEqual = Convert.ToDouble(activeRow.Cells[Long + "ValueLessThanOrEqual"].Value.ToString());
                }
                int rowsCount = grd.Rows.Count;
                UltraGridRow dr = grd.Rows[rowsCount - 1];

                List<OtherFeesCriteria> commrulecris = (List<OtherFeesCriteria>)grd.DataSource;
                OtherFeesCriteria commrulecri = new OtherFeesCriteria();

                //The below varriables are taken from the last row of the grid before adding the new row.
                double dblValueFrom = Convert.ToDouble(dr.Cells[Long + "ValueGreaterThan"].Value.ToString());
                double dblValueTo = Convert.ToDouble(dr.Cells[Long + "ValueLessThanOrEqual"].Value.ToString());
                double CommRate = Convert.ToDouble(dr.Cells[Long + "FeeRate"].Value.ToString());
                string commissionRateUnit = dr.Cells[Long + "FeeCriteriaUnit"].Value.ToString();
                int calbasis = (int)dr.Cells[Long + "CalculationBasis"].Value;
                int otherFeesCriteriaId = (int)dr.Cells["OtherFeesCriteriaId"].Value;

                // Convert.ToInt32(cmbBasedOnCriteria.Value);

                //Add one more condition to avoid adding 2nd new row when the below condition fullfills.
                if (dblValueTo > 0)
                {
                    commrulecri.OtherFeesCriteriaId = otherFeesCriteriaId + 1;
                    if (longOrSort)
                    {
                        commrulecri.LongFeeRate = 0;
                        commrulecri.LongValueGreaterThan = 0;
                        commrulecri.LongValueLessThanOrEqual = 0;
                        commrulecri.LongFeeCriteriaUnit = commissionRateUnit;
                        commrulecri.LongCalculationBasis = calbasis;
                    }
                    else
                    {
                        commrulecri.ShortFeeRate = 0;
                        commrulecri.ShortValueGreaterThan = 0;
                        commrulecri.ShortValueLessThanOrEqual = 0;
                        commrulecri.ShortFeeCriteriaUnit = commissionRateUnit;
                        commrulecri.ShortCalculationBasis = calbasis;
                    }
                    // commrulecri.CommissionType = commissionType;
                    commrulecris.Add(commrulecri);
                    grd.DataSource = commrulecris;
                    grd.DataBind();
                    grd.ActiveCell = prevActiveCell;
                    grd.Focus();
                    grd.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                }

                grd.UpdateData();
                if (grd.ActiveRow != null)
                {
                    int rowindex = grd.ActiveRow.Index;
                    int rowsC = grd.Rows.Count;
                    // assign value to next cloumn
                    if ((rowindex + 1) != rowsC)
                    {
                        UltraGridRow newDatarow = grd.Rows[rowindex + 1];
                        newDatarow.Cells[Long + "ValueGreaterThan"].Value = valueLessThanOrEqual;
                    }
                }
            }
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
        }

    }
}
