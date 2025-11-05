using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.BusinessObjects;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Utilities.UIUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessLogic;
//using Prana.PostTrade;
//using Prana.PostTrade.Commission;
using Prana.Interfaces;
using Prana.WCFConnectionMgr;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.CommonDataCache;


namespace Prana.AllocationNew
{
    public partial class CtrlRecalculate : UserControl
    {
        #region Constants
        const string CalculationBasis_Shares = "Shares";
        const string CalculationBasis_Shares_Caption = "Per Share";
        const string CalculationBasis_Contacts = "Contracts";
        const string CalculationBasis_Contacts_Caption = "Per Contract";
        const string CalculationBasis_Notional = "Notional";
        const string CalculationBasis_Notional_Caption = "bps";
        const string CalculationBasis_FlatAmount = "FlatAmount";
        const string CalculationBasis_FlatAmount_Caption = "Per Trade/Taxlot";
        #endregion

        #region Properties
        private int _companyUserID = int.MinValue;
        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }

        ProxyBase<IAllocationServices> _allocationServices = null;
        public ProxyBase<IAllocationServices> AllocationServices
        {
            set
            {
                _allocationServices = value;

            }

        }

        private Dictionary<int, string> _accountPBDetails;
        public Dictionary<int, string> AccountPBDetails
        {
            get { return _accountPBDetails; }
            set { _accountPBDetails = value; }
        }


        private Dictionary<int, List<int>> _masterFundAssociation;
        public Dictionary<int, List<int>> MasterFundAssociation
        {
            get { return _masterFundAssociation; }
            set { _masterFundAssociation = value; }
        }
        #endregion

        #region Events
        public event EventHandler RecalculateCommission;
        public event EventHandler BulkChangeOnGroupLevel;
        public event EventHandler DisplayMessage;
        #endregion

        public CtrlRecalculate()
        {
            InitializeComponent();
            if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnViewRule.BackColor = System.Drawing.Color.FromArgb(55,67,85);
                btnViewRule.ForeColor = System.Drawing.Color.White;
                btnViewRule.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnViewRule.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnViewRule.UseAppStyling = false;
                btnViewRule.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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
        public void InitControl(int companyUser)
        {
            try
            {
                _companyUserID = companyUser;
                BindCombos();
                BindCounterParties();
                BindThirdParties();
                BindFXConvertor();
                BindMasterFunds();
                _masterFundAssociation = Prana.CommonDataCache.CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                grpSpecifyCommissionRule.Enabled = false;
                grpSelectCommissionRule.Enabled = false;
                if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER);
                }
                else
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_ALLOCATION_MAIN);
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

        #region Binding Data to ComboBoxes
        private void BindFXConvertor()
        {
            try
            {
                DataTable dtFXOperators = new DataTable();
                DataColumn colOperatorID = new DataColumn("OperatorID");
                colOperatorID.DataType = typeof(int);

                DataColumn colOperatorName = new DataColumn("OperatorName");
                colOperatorName.DataType = typeof(string);

                dtFXOperators.Columns.Add(colOperatorID);
                dtFXOperators.Columns.Add(colOperatorName);

                DataRow row = dtFXOperators.NewRow();
                row[colOperatorID] = int.MinValue;
                row[colOperatorName] = ApplicationConstants.C_COMBO_SELECT;

                dtFXOperators.Rows.Add(row);

                List<EnumerationValue> fxConversionMethodOperator = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.Operator));
                foreach (EnumerationValue var in fxConversionMethodOperator)
                {
                    if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                    {

                        DataRow dRow = dtFXOperators.NewRow();
                        dRow[colOperatorID] = var.Value;
                        dRow[colOperatorName] = var.DisplayText;
                        dtFXOperators.Rows.Add(dRow);
                    }
                }

                BindCombo(cmbFXConversionOperator, dtFXOperators, "OperatorName", "OperatorID");
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

        private void BindMasterFunds()
        {
            try
            {
                DataTable dtMasterFunds = new DataTable();
                DataColumn colMasterFundID = new DataColumn("MasterFundID");
                colMasterFundID.DataType = typeof(int);

                DataColumn colMasterFundName = new DataColumn("MasterFundName");
                colMasterFundName.DataType = typeof(string);

                dtMasterFunds.Columns.Add(colMasterFundID);
                dtMasterFunds.Columns.Add(colMasterFundName);

                DataRow row = dtMasterFunds.NewRow();
                row[colMasterFundID] = int.MinValue;
                row[colMasterFundName] = ApplicationConstants.C_COMBO_SELECT;

                dtMasterFunds.Rows.Add(row);

                Dictionary<int, string> userMasterFunds = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAllMasterFunds();
                foreach (KeyValuePair<int, string> kvp in userMasterFunds)
                {
                    DataRow dRow = dtMasterFunds.NewRow();
                    dRow[colMasterFundID] = kvp.Key;
                    dRow[colMasterFundName] = kvp.Value;
                    dtMasterFunds.Rows.Add(dRow);
                }

                BindCombo(cmbMasterFund, dtMasterFunds, "MasterFundName", "MasterFundID");
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

        private void BindCounterParties()
        {
            try
            {
                DataTable dtCounterParties = new DataTable();
                DataColumn colCounterPartyID = new DataColumn("CounterPartyID");
                colCounterPartyID.DataType = typeof(int);

                DataColumn colCounterPartyName = new DataColumn("CounterPartyName");
                colCounterPartyName.DataType = typeof(string);

                dtCounterParties.Columns.Add(colCounterPartyID);
                dtCounterParties.Columns.Add(colCounterPartyName);

                DataRow row = dtCounterParties.NewRow();
                row[colCounterPartyID] = int.MinValue;
                row[colCounterPartyName] = ApplicationConstants.C_COMBO_SELECT;

                dtCounterParties.Rows.Add(row);

                Dictionary<int, string> userCounterParties = Prana.CommonDataCache.CachedDataManager.GetInstance.GetUserCounterParties();
                foreach (KeyValuePair<int, string> kvp in userCounterParties)
                {
                    DataRow dRow = dtCounterParties.NewRow();
                    dRow[colCounterPartyID] = kvp.Key;
                    dRow[colCounterPartyName] = kvp.Value;
                    dtCounterParties.Rows.Add(dRow);
                }

                BindCombo(cmbCounterParty, dtCounterParties, "CounterPartyName", "CounterPartyID");
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

        private void BindThirdParties()
        {
            try
            {
                DataTable dtThirdParties = new DataTable();
                DataColumn colThirdPartyID = new DataColumn("ThirdPartyID");
                colThirdPartyID.DataType = typeof(int);

                DataColumn colThirdPartyName = new DataColumn("ThirdPartyName");
                colThirdPartyName.DataType = typeof(string);

                dtThirdParties.Columns.Add(colThirdPartyID);
                dtThirdParties.Columns.Add(colThirdPartyName);

                DataRow row = dtThirdParties.NewRow();
                row[colThirdPartyID] = int.MinValue;
                row[colThirdPartyName] = ApplicationConstants.C_COMBO_SELECT;

                dtThirdParties.Rows.Add(row);

                int companyID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCompanyID();

                _accountPBDetails = _allocationServices.InnerChannel.GetAccountPBDetails();
                Dictionary<int, string> thirdPartyDetails = _allocationServices.InnerChannel.GetThirdPartyDetails(companyID);

                foreach (KeyValuePair<int, string> kvp in thirdPartyDetails)
                {
                    DataRow dRow = dtThirdParties.NewRow();
                    dRow[colThirdPartyID] = kvp.Key;
                    dRow[colThirdPartyName] = kvp.Value;
                    dtThirdParties.Rows.Add(dRow);
                }

                BindCombo(cmbFXPrimeBroker, dtThirdParties, "ThirdPartyName", "ThirdPartyID");
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

        private void BindCombo(UltraCombo comboBox, DataTable dt, string displayMember, string valueMember)
        {
            try
            {
                comboBox.DataSource = null;
                comboBox.DataSource = dt;

                comboBox.DisplayMember = displayMember;
                comboBox.ValueMember = valueMember;

                foreach (UltraGridColumn column in comboBox.DisplayLayout.Bands[0].Columns)
                {
                    column.Hidden = true;
                    if (column.Key.Equals(displayMember))
                    {
                        column.Hidden = false;
                    }
                }
                comboBox.Text = ApplicationConstants.C_COMBO_SELECT;
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

        private void BindCombos()
        {
            try
            {
                //PostTradeCacheManager.LoadCommisionRules(_companyUserID);
                //_allocationServices.LoadCommisionRules(_companyUserID);
                List<CommissionRule> getAllCommissionRules = new List<CommissionRule>();
                //getAllCommissionRules = CommissionRulesCacheManager.GetInstance().GetAllCommissionRules();
                getAllCommissionRules = _allocationServices.InnerChannel.GetAllCommissionRules();

                CommissionRule defaultRule = new CommissionRule();
                defaultRule.RuleID = Guid.Empty;
                defaultRule.RuleName = ApplicationConstants.C_COMBO_SELECT;

                getAllCommissionRules.Add(defaultRule);

                if (getAllCommissionRules != null)
                {
                    cmBoxRules.DataSource = null;
                    cmBoxRules.DataSource = getAllCommissionRules;
                    cmBoxRules.ValueMember = "RuleID";
                    cmBoxRules.DisplayMember = "RuleName";

                    ColumnsCollection columns = cmBoxRules.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns)
                    {
                        if (!column.Key.Equals("RuleName"))
                        {
                            column.Hidden = true;
                        }
                    }
                    foreach (UltraGridBand var in cmBoxRules.DisplayLayout.Bands)
                    {
                        var.ColHeadersVisible = false;
                    }
                    cmBoxRules.Text = ApplicationConstants.C_COMBO_SELECT;
                    //cmBoxRules.DisplayLayout.Bands[0].ColHeadersVisible = false;

                    BindHardSoftCommissionCombos(cmbCommissionBasedOn, lblCommissionRate);
                    BindHardSoftCommissionCombos(cmbSoftCommissionBasedOn, lblSoftCommissionRate);

                    EnumerationValueList lstBasedOn = new EnumerationValueList();
                    lstBasedOn = CommissionEnumHelper.GetOldListForCalculationBasis();

                    BindCommissionCombos(cmbFeesBasedOn, lstBasedOn);
                    BindCommissionCombos(cmbClearingBrokerFeeBasedOn, lstBasedOn);
                    BindCommissionCombos(cmbStampDuty, lstBasedOn);
                    BindCommissionCombos(cmbClearingFee, lstBasedOn);
                    BindCommissionCombos(cmbTransactionLevy, lstBasedOn);
                    BindCommissionCombos(cmbMiscFees, lstBasedOn);
                    BindCommissionCombos(cmbMiscFees, lstBasedOn);
                    BindCommissionCombos(cmbTaxonCommission, lstBasedOn);
                    BindCommissionCombos(cmbSecFee, lstBasedOn);
                    BindCommissionCombos(cmbOccFee, lstBasedOn);
                    BindCommissionCombos(cmbOrfFee, lstBasedOn);


                }
                else
                {
                    MessageBox.Show("No commission rule found.", "Commission Rule", MessageBoxButtons.OK);
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

        private void BindHardSoftCommissionCombos(UltraCombo cmbCommBasedOn, Infragistics.Win.Misc.UltraLabel lblCommRate)
        {
            EnumerationValueList lstBasedOn = new EnumerationValueList();

            // bind Based on Parameter combo
            lstBasedOn = CommissionEnumHelper.GetOldListForCalculationBasis();
            cmbCommBasedOn.DataSource = null;
            cmbCommBasedOn.DataSource = lstBasedOn;
            cmbCommBasedOn.DisplayMember = "DisplayText";
            cmbCommBasedOn.ValueMember = "Value";
            if (cmbCommBasedOn.Value != null)
            {
                if (cmbCommBasedOn.Value.ToString().Equals(CalculationBasis_Shares))
                {
                    lblCommRate.Text = CalculationBasis_Shares_Caption;
                }
                else if (cmbCommBasedOn.Value.ToString().Equals(CalculationBasis_Notional))
                {
                    lblCommRate.Text = CalculationBasis_Notional_Caption;
                }
                else if (cmbCommBasedOn.Value.ToString().Equals(CalculationBasis_Contacts))
                {
                    lblCommRate.Text = CalculationBasis_Contacts_Caption;
                }
                else
                {
                    lblCommRate.Text = CalculationBasis_FlatAmount_Caption;
                }
            }
            Utils.UltraComboFilter(cmbCommBasedOn, "DisplayText");
            cmbCommBasedOn.Value = int.MinValue;
        }

        private void BindCommissionCombos(UltraCombo comboBox, EnumerationValueList lstBasedOn)
        {
            try
            {
                comboBox.DataSource = null;
                comboBox.DataSource = lstBasedOn;
                comboBox.DisplayMember = "DisplayText";
                comboBox.ValueMember = "Value";
                Utils.UltraComboFilter(comboBox, "DisplayText");
                comboBox.Value = int.MinValue;
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
        #endregion

        #region CommissionRules
        private void btnRecalculateCommission_Click(object sender, EventArgs e)
        {
            try
            {
                CommissionRule commissionRule = new CommissionRule();
                CommissionRuleEvents commissionRuleEvent = new CommissionRuleEvents();
                bool isAnyCheckboxSelected = false;

                if (rdbtnGroup.Checked)
                {
                    commissionRuleEvent.GroupWise = true;
                }
                else if (rdbtnTaxlot.Checked)
                {
                    commissionRuleEvent.GroupWise = false;
                }
                if (rdBtnSelectCommissionRule.Checked)
                {
                    if (!cmBoxRules.Text.Equals(string.Empty) && !cmBoxRules.Text.Equals(ApplicationConstants.C_COMBO_SELECT))
                    {
                        commissionRule = (CommissionRule)cmBoxRules.SelectedRow.ListObject;
                        isAnyCheckboxSelected = true;
                        commissionRule.IsCommissionRuleSelected = true;
                        if (chkLstPrimeBrokerAccounts.CheckedItems.Count > 0)
                        {
                            commissionRule.AccountIDs = GetSelectedAccountIDs();
                        }
                        else
                        {
                            commissionRule.AccountIDs = null;
                        }

                        if (commissionRule.Commission.CommissionRate == 0)
                            commissionRule.Commission.CommissionRate = double.MinValue;
                        if (commissionRule.SoftCommission.CommissionRate == 0)
                            commissionRule.SoftCommission.CommissionRate = double.MinValue;
                        if (commissionRule.ClearingFeeRate == 0)
                            commissionRule.ClearingFeeRate = double.MinValue;
                        if (commissionRule.ClearingBrokerFeeRate == 0)
                            commissionRule.ClearingBrokerFeeRate = double.MinValue;

                        commissionRule.StampDuty = double.MinValue;
                        commissionRule.TaxonCommissions = double.MinValue;
                        commissionRule.TransactionLevy = double.MinValue;
                        commissionRule.ClearingFee_A = double.MinValue;
                        commissionRule.MiscFees = double.MinValue;
                        commissionRule.SecFee = double.MinValue;
                        commissionRule.OccFee = double.MinValue;
                        commissionRule.OrfFee = double.MinValue;
                    }
                    else
                    {
                        statusProvider.SetError(btnRecalculateCommission, "Please select Commission Rule for calculation.");
                        return;
                    }
                }
                else if (rdBtnSpecifyCommissionRule.Checked)
                {
                    commissionRule.IsCommissionRuleSelected = false;
                    if (cbCommission.Checked)
                    {
                        if (cmbCommissionBasedOn.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT && txtCommissionRate.Value > 0)
                        {
                            commissionRule.Commission.CommissionRate = (double)txtCommissionRate.Value;
                            commissionRule.Commission.RuleAppliedOn = (CalculationBasis)cmbCommissionBasedOn.Value;
                            isAnyCheckboxSelected = true;
                        }
                        else
                        {
                            statusProvider.SetError(btnRecalculateCommission, "Either Commission Calculation Basis is not selected or Commission rate is less than or equal to 0.");
                            return;
                        }
                    }
                    else if (!cbCommission.Checked)
                    {
                        commissionRule.Commission.CommissionRate = double.MinValue;
                    }

                    if (cbSoftCommission.Checked)
                    {
                        if (cmbSoftCommissionBasedOn.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT && txtSoftCommissionRate.Value > 0)
                        {
                            commissionRule.SoftCommission.CommissionRate = (double)txtSoftCommissionRate.Value;
                            commissionRule.SoftCommission.RuleAppliedOn = (CalculationBasis)cmbSoftCommissionBasedOn.Value;
                            isAnyCheckboxSelected = true;
                        }
                        else
                        {
                            statusProvider.SetError(btnRecalculateCommission, "Either Soft Commission Calculation Basis is not selected or Soft Commission rate is less than or equal to 0.");
                            return;
                        }
                    }
                    else if (!cbSoftCommission.Checked)
                    {
                        commissionRule.SoftCommission.CommissionRate = double.MinValue;
                    }

                    if (cbFees.Checked)
                    {
                        if (cmbFeesBasedOn.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT && txtFeesRate.Value > 0)
                        {
                            commissionRule.ClearingFeeRate = (double)txtFeesRate.Value;
                            commissionRule.ClearingFeeCalculationBasedOn = (CalculationBasis)cmbFeesBasedOn.Value;
                            if (txtFeesRate.Value != 0)
                            {
                                commissionRule.IsClearingFeeApplied = true;
                            }
                            isAnyCheckboxSelected = true;
                        }
                        else
                        {
                            statusProvider.SetError(btnRecalculateCommission, "Either Other Broker Fee Calculation Basis is not selected or Other Broker Fee rate is less than or equal to 0.");
                            return;
                        }
                    }
                    else if(!cbFees.Checked)
                    {
                        commissionRule.ClearingFeeRate = double.MinValue;
                    }

                    if (cbClearingBrokerFee.Checked)
                    {
                        if (cmbClearingBrokerFeeBasedOn.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT && txtClearingBrokerFeeRate.Value > 0)
                        {
                            commissionRule.ClearingBrokerFeeRate = (double)txtClearingBrokerFeeRate.Value;
                            commissionRule.ClearingBrokerFeeCalculationBasedOn = (CalculationBasis)cmbClearingBrokerFeeBasedOn.Value;
                            if (txtClearingBrokerFeeRate.Value != 0)
                            {
                                commissionRule.IsClearingBrokerFeeApplied = true;
                            }
                            isAnyCheckboxSelected = true;
                        }
                        else
                        {
                            statusProvider.SetError(btnRecalculateCommission, "Either Other Broker Fee Calculation Basis is not selected or Other Broker Fee rate is less than or equal to 0.");
                            return;
                        }
                    }
                    else if (!cbClearingBrokerFee.Checked)
                    {
                        commissionRule.ClearingBrokerFeeRate = double.MinValue;
                    }

                    if (chkStampDuty.Checked)
                    {
                        if (cmbStampDuty.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT && txtStampDuty.Value > 0)
                        {
                            commissionRule.StampDuty = (double)txtStampDuty.Value;
                            commissionRule.StampDutyCalculationBasedOn = (CalculationBasis)cmbStampDuty.Value;
                            isAnyCheckboxSelected = true;
                        }
                        else
                        {
                            statusProvider.SetError(btnRecalculateCommission, "Either Stamp Duty Calculation Basis is not selected or Stamp Duty rate is less than or equal to 0.");
                            return;
                        }
                    }
                    else
                    {
                        commissionRule.StampDuty = double.MinValue;
                    }

                    if (chkClearingFee.Checked)
                    {
                        if (cmbClearingFee.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT && txtClearingFee.Value > 0)
                        {
                            commissionRule.ClearingFee_A = (double)txtClearingFee.Value;
                            commissionRule.ClearingFeeCalculationBasedOn_A = (CalculationBasis)cmbClearingFee.Value;
                            isAnyCheckboxSelected = true;
                        }
                        else
                        {
                            statusProvider.SetError(btnRecalculateCommission, "Either Clearing Fee Calculation Basis is not selected or Clearing Fee rate is less than or equal to 0.");
                            return;
                        }
                    }
                    else
                    {
                        commissionRule.ClearingFee_A = double.MinValue;
                    }

                    if (chkTaxOnCommission.Checked)
                    {
                        if (cmbTaxonCommission.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT && txtTaxonCommission.Value > 0)
                        {
                            commissionRule.TaxonCommissions = (double)txtTaxonCommission.Value;
                            commissionRule.TaxonCommissionsCalculationBasedOn = (CalculationBasis)cmbTaxonCommission.Value;
                            isAnyCheckboxSelected = true;
                        }
                        else
                        {
                            statusProvider.SetError(btnRecalculateCommission, "Either Tax on Commissions Calculation Basis is not selected or Tax on Commissions rate is less than or equal to 0.");
                            return;
                        }
                    }
                    else
                    {
                        commissionRule.TaxonCommissions = double.MinValue;
                    }

                    if (chkTransactionLevy.Checked)
                    {
                        if (cmbTransactionLevy.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT && txtTransactionLevy.Value > 0)
                        {
                            commissionRule.TransactionLevy = (double)txtTransactionLevy.Value;
                            commissionRule.TransactionLevyCalculationBasedOn = (CalculationBasis)cmbTransactionLevy.Value;
                            isAnyCheckboxSelected = true;
                        }
                        else
                        {
                            statusProvider.SetError(btnRecalculateCommission, "Either Transaction Levy Calculation Basis is not selected or Transaction Levy rate is less than or equal to 0.");
                            return;
                        }
                    }
                    else
                    {
                        commissionRule.TransactionLevy = double.MinValue;
                    }

                    if (chkMiscFees.Checked)
                    {
                        if (cmbMiscFees.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT && txtMiscFees.Value > 0)
                        {
                            commissionRule.MiscFees = (double)txtMiscFees.Value;
                            commissionRule.MiscFeesCalculationBasedOn = (CalculationBasis)cmbMiscFees.Value;
                            isAnyCheckboxSelected = true;
                        }
                        else
                        {
                            statusProvider.SetError(btnRecalculateCommission, "Either Misc Fees Calculation Basis is not selected or Misc Fees rate is less than or equal to 0.");
                            return;
                        }
                    }
                    else
                    {
                        commissionRule.MiscFees = double.MinValue;
                    }

                    if (chkSecFee.Checked)
                    {
                        if (cmbSecFee.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT && txtSecFee.Value > 0)
                        {
                            commissionRule.SecFee = (double)txtSecFee.Value;
                            commissionRule.SecFeeCalculationBasedOn = (CalculationBasis)cmbSecFee.Value;
                            isAnyCheckboxSelected = true;
                        }
                        else
                        {
                            statusProvider.SetError(btnRecalculateCommission, "Either SEC Fee Calculation Basis is not selected or SEC Fee rate is less than or equal to 0.");
                            return;
                        }
                    }
                    else
                    {
                        commissionRule.SecFee = double.MinValue;
                    }

                    if (chkOccFee.Checked)
                    {
                        if (cmbOccFee.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT && txtOccFee.Value > 0)
                        {
                            commissionRule.OccFee = (double)txtOccFee.Value;
                            commissionRule.OccFeeCalculationBasedOn = (CalculationBasis)cmbOccFee.Value;
                            isAnyCheckboxSelected = true;
                        }
                        else
                        {
                            statusProvider.SetError(btnRecalculateCommission, "Either OCC Fee Calculation Basis is not selected or OCC Fee rate is less than or equal to 0.");
                            return;
                        }
                    }
                    else
                    {
                        commissionRule.OccFee = double.MinValue;
                    }

                    if (chkOrfFee.Checked)
                    {
                        if (cmbOrfFee.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT && txtOrfFee.Value > 0)
                        {
                            commissionRule.OrfFee = (double)txtOrfFee.Value;
                            commissionRule.OrfFeeCalculationBasedOn = (CalculationBasis)cmbOrfFee.Value;
                            isAnyCheckboxSelected = true;
                        }
                        else
                        {
                            statusProvider.SetError(btnRecalculateCommission, "Either ORF Fee Calculation Basis is not selected or ORF Fee rate is less than or equal to 0.");
                            return;
                        }
                    }
                    else
                    {
                        commissionRule.OrfFee = double.MinValue;
                    }

                    if (chkLstPrimeBrokerAccounts.CheckedItems.Count > 0)
                    {
                        commissionRule.AccountIDs = GetSelectedAccountIDs();                      
                    }
                    else
                    {
                        commissionRule.AccountIDs = null;
                    }
                }
                else if (rdBtnDefaultRules.Checked)
                {
                    commissionRule = null;
                    isAnyCheckboxSelected = true;
                }

                if (isAnyCheckboxSelected)
                {
                    statusProvider.Clear();

                    if (RecalculateCommission != null)
                    {
                        RecalculateCommission(commissionRule, commissionRuleEvent);
                    }
                }
                else
                {
                    statusProvider.SetError(btnRecalculateCommission, "Please specify atleast one Commission/Fees Rule.");
                    return;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private List<int> GetSelectedAccountIDs()
        {
            List<int> accountIDs = new List<int>();
            try
            {
                foreach (Object item in chkLstPrimeBrokerAccounts.CheckedItems)
                {
                    int accountID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountID(item.ToString());
                    accountIDs.Add(accountID);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return accountIDs;
        }

        private void btnViewRule_Click(object sender, EventArgs e)
        {
            try
            {
                CommissionRule commissionRule = new CommissionRule();
                if (rdBtnSelectCommissionRule.Checked == true && cmBoxRules.Value != null && !cmBoxRules.Text.Equals(Prana.Global.ApplicationConstants.C_COMBO_SELECT))
                {
                    commissionRule = (CommissionRule)cmBoxRules.SelectedRow.ListObject;
                    CommissionRuleDisplay commissionruledisplay = new CommissionRuleDisplay();
                    commissionruledisplay.Text = commissionRule.RuleName;
                    commissionruledisplay.GetCommisionRule(commissionRule);
                    commissionruledisplay.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Please select a Commission Rule", "Alert", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void rdBtnSpecifyCommissionRule_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSpecifyCommissionRule.Checked)
                {
                    grpSpecifyCommissionRule.Enabled = true;
                    grpSelectCommissionRule.Enabled = false;

                    //select commission rule
                    cmBoxRules.Enabled = false;
                    btnViewRule.Enabled = false;

                    // specify commission rule
                    //commission                
                    cbCommission.Enabled = true;
                    cbCommission.Checked = false;
                    txtCommissionRate.Enabled = false;
                    cmbCommissionBasedOn.Enabled = false;
                    //soft commission
                    cbSoftCommission.Enabled = true;
                    cbSoftCommission.Checked = false;
                    txtSoftCommissionRate.Enabled = false;
                    cmbSoftCommissionBasedOn.Enabled = false;
                    // other broker fees                
                    cbFees.Enabled = true;
                    cbFees.Checked = false;
                    txtFeesRate.Enabled = false;
                    cmbFeesBasedOn.Enabled = false;
                    // clearing broker fees
                    cbClearingBrokerFee.Enabled = true;
                    cbClearingBrokerFee.Checked = false;
                    txtClearingBrokerFeeRate.Enabled = false;
                    cmbClearingBrokerFeeBasedOn.Enabled = false;
                    //clreaing fee              
                    chkClearingFee.Enabled = true;
                    chkClearingFee.Checked = false;
                    txtClearingFee.Enabled = false;
                    cmbClearingFee.Enabled = false;
                    //taxon commissions               
                    chkTaxOnCommission.Enabled = true;
                    chkTaxOnCommission.Checked = false;
                    txtTaxonCommission.Enabled = false;
                    cmbTaxonCommission.Enabled = false;
                    //stamp duty                
                    chkStampDuty.Enabled = true;
                    chkStampDuty.Checked = false;
                    txtStampDuty.Enabled = false;
                    cmbStampDuty.Enabled = false;
                    //Transaction Levy
                    chkTransactionLevy.Enabled = true;
                    chkTransactionLevy.Checked = false;
                    txtTransactionLevy.Enabled = false;
                    cmbTransactionLevy.Enabled = false;
                    //Misc Fees
                    chkMiscFees.Enabled = true;
                    chkMiscFees.Checked = false;
                    txtMiscFees.Enabled = false;
                    cmbMiscFees.Enabled = false;
                    //Sec Fee
                    chkSecFee.Enabled = true;
                    chkSecFee.Checked = false;
                    txtSecFee.Enabled = false;
                    cmbSecFee.Enabled = false;
                    //Occ Fee
                    chkOccFee.Enabled = true;
                    chkOccFee.Checked = false;
                    txtOccFee.Enabled = false;
                    cmbOccFee.Enabled = false;
                    //Orf Fee
                    chkOrfFee.Enabled = true;
                    chkOrfFee.Checked = false;
                    txtOrfFee.Enabled = false;
                    cmbOrfFee.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void rdBtnSelectCommissionRule_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSelectCommissionRule.Checked)
                {
                    grpSpecifyCommissionRule.Enabled = false;
                    grpSelectCommissionRule.Enabled = true;
                    txtCommissionRate.Enabled = false;
                    txtSoftCommissionRate.Enabled = false;
                    txtFeesRate.Enabled = false;
                    txtClearingBrokerFeeRate.Enabled = false;
                    cmBoxRules.Enabled = true;
                    btnViewRule.Enabled = true;
                    cmbCommissionBasedOn.Enabled = false;
                    cmbSoftCommissionBasedOn.Enabled = false;
                    cmbFeesBasedOn.Enabled = false;
                    cmbClearingBrokerFeeBasedOn.Enabled = false;
                    cbCommission.Enabled = false;
                    cbSoftCommission.Enabled = false;
                    cbFees.Enabled = false;
                    cbClearingBrokerFee.Enabled = false;

                    //rdbtnTaxlot.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void rdBtnDefaultRules_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnDefaultRules.Checked)
                {
                    grpSpecifyCommissionRule.Enabled = false;
                    grpSelectCommissionRule.Enabled = false;
                    txtCommissionRate.Enabled = false;
                txtSoftCommissionRate.Enabled = false;
                    txtFeesRate.Enabled = false;
                txtClearingBrokerFeeRate.Enabled = false;
                    btnViewRule.Enabled = true;
                    cmBoxRules.Enabled = true;
                    cmbCommissionBasedOn.Enabled = false;
                cmbSoftCommissionBasedOn.Enabled = false;
                    cmbFeesBasedOn.Enabled = false;
                cmbClearingBrokerFeeBasedOn.Enabled = false;
                    cbCommission.Enabled = false;
                cbSoftCommission.Enabled = false;
                    cbFees.Enabled = false;
                cbClearingBrokerFee.Enabled = false;

                    //rdbtnTaxlot.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Commission CheckBox State Changed
        private void cbCommission_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToggleFeesControlsState(cbCommission, txtCommissionRate, cmbCommissionBasedOn);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cbFees_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToggleFeesControlsState(cbFees, txtFeesRate, cmbFeesBasedOn);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkClearingFee_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToggleFeesControlsState(chkClearingFee, txtClearingFee, cmbClearingFee);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkTaxOnCommission_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToggleFeesControlsState(chkTaxOnCommission, txtTaxonCommission, cmbTaxonCommission);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkStampDuty_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToggleFeesControlsState(chkStampDuty, txtStampDuty, cmbStampDuty);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkTransactionLevy_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToggleFeesControlsState(chkTransactionLevy, txtTransactionLevy, cmbTransactionLevy);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkMiscFees_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToggleFeesControlsState(chkMiscFees, txtMiscFees, cmbMiscFees);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkSecFee_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToggleFeesControlsState(chkSecFee, txtSecFee, cmbSecFee);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkOccFee_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToggleFeesControlsState(chkOccFee, txtOccFee, cmbOccFee);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkOrfFee_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToggleFeesControlsState(chkOrfFee, txtOrfFee, cmbOrfFee);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ToggleFeesControlsState(CheckBox checkBox, NumericUpDown textBox, UltraCombo comboBox)
        {
            try
            {
                bool isEnabled = checkBox.Checked ? true : false;
                textBox.Enabled = isEnabled;
                comboBox.Enabled = isEnabled;
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
        #endregion

        #region Commission ComboBox Value Changed
        private void cmbCommissionBasedOn_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionRateBasedOn(cmbCommissionBasedOn, lblCommissionRate);

                CommissionRateBasedOn(cmbSoftCommissionBasedOn, lblSoftCommissionRate);

                CommissionRateBasedOn(cmbFeesBasedOn, lblFeesRate);

                CommissionRateBasedOn(cmbClearingBrokerFeeBasedOn, lblClearingBrokerFeeRate);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbClearingFee_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionRateBasedOn(cmbClearingFee, lblClearingFee);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbTaxonCommission_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionRateBasedOn(cmbTaxonCommission, lblTaxonCommission);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbStampDuty_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionRateBasedOn(cmbStampDuty, lblStampDuty);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbTransactionLevy_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionRateBasedOn(cmbTransactionLevy, lblTransaction);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbMiscFees_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionRateBasedOn(cmbMiscFees, lblMiscFees);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbSecFee_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionRateBasedOn(cmbSecFee, lblSecFee);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbOccFee_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionRateBasedOn(cmbOccFee, lblOccFee);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbOrfFee_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CommissionRateBasedOn(cmbOrfFee, lblOrfFee);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CommissionRateBasedOn(UltraCombo comboBox, Infragistics.Win.Misc.UltraLabel label)
        {
            try
            {
                if (comboBox.Value != null)
                {
                    if (comboBox.Value.ToString().Equals(CalculationBasis_Shares))
                    {
                        label.Text = CalculationBasis_Shares_Caption;
                    }
                    else if (comboBox.Value.ToString().Equals(CalculationBasis_Notional))
                    {
                        label.Text = CalculationBasis_Notional_Caption;
                    }
                    else if (comboBox.Value.ToString().Equals(CalculationBasis_Contacts))
                    {
                        label.Text = CalculationBasis_Contacts_Caption;
                    }
                    else if (comboBox.Value.ToString().Equals(CalculationBasis_FlatAmount))
                    {
                        label.Text = CalculationBasis_FlatAmount_Caption;
                    }
                    else
                    {
                        label.Text = "";
                    }
                }
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

        private void ToggleUIItemsForCommissionrule(bool status)
        {
            cbCommission.Checked = status;
            cbSoftCommission.Checked = status;
            cbFees.Checked = status;
            cbClearingBrokerFee.Checked = status;
            chkClearingFee.Checked = status;
            chkTaxOnCommission.Checked = status;
            chkStampDuty.Checked = status;
            chkTransactionLevy.Checked = status;
            chkMiscFees.Checked = status;
            chkSecFee.Checked = status;
            chkOccFee.Checked = status;
            chkOrfFee.Checked = status;
        }
        #endregion

        #region BulkUpdates
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkAll.Checked)
                {
                    ToggleUIItemsForCommissionrule(true);
                }

                else
                {
                    ToggleUIItemsForCommissionrule(false);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void btnUpdateBulkChanges_Click(object sender, EventArgs e)
        {
            try
            {
                BulkChangesAtGroupLevel bulkChanges = new BulkChangesAtGroupLevel();
                bool isAnyCheckboxSelected = false;

                if (chkAVGPrice.Checked)
                {
                    if (txtAvgPrice.Value > 0)
                    {
                        bulkChanges.AvgPrice = txtAvgPrice.Value;
                        isAnyCheckboxSelected = true;
                        statusProvider.SetError(txtAvgPrice, "");
                    }
                    else
                    {
                        statusProvider.SetError(txtAvgPrice, "Please check Avg Price, it is less than or equal to 0.");
                        return;
                    }
                }

                // Checking Round Average Price Checked or not, If Checked then set AvgPxUpto to given decimal places by user
                if (chckRoundAvgPx.Checked)
                {
                    if (numericRoundAvgPx.Value > 0)
                    {
                        statusProvider.SetError(numericRoundAvgPx, System.String.Empty);
                        bulkChanges.AvgPxUpto = int.Parse(numericRoundAvgPx.Value.ToString());
                        isAnyCheckboxSelected = true;
                        statusProvider.SetError(numericRoundAvgPx, "");
                    }
                    else
                    {
                        statusProvider.SetError(numericRoundAvgPx, "Please check Round Avg Price Upto, it is less than or equal to 0");
                        return;
                    }
                }

                if (chkAccruedInterest.Checked)
                {
                    if (txtAccruedInterest.Value > 0)
                    {
                        bulkChanges.AccruedInterest = txtAccruedInterest.Value;
                        isAnyCheckboxSelected = true;
                        statusProvider.SetError(txtAccruedInterest, "");
                    }
                    else
                    {
                        statusProvider.SetError(txtAccruedInterest, "Please check Accrued Interest, it is less than or equal to 0.");
                        return;
                    }
                }
                if (chkCounterParty.Checked)
                {
                    if (cmbCounterParty.Text.ToString() != string.Empty && cmbCounterParty.Text.ToString() != Prana.Global.ApplicationConstants.C_COMBO_SELECT)
                    {
                        bulkChanges.CounterPartyID = Convert.ToInt32(cmbCounterParty.Value.ToString());
                        isAnyCheckboxSelected = true;
                        statusProvider.SetError(cmbCounterParty, "");
                    }
                    else
                    {
                        statusProvider.SetError(cmbCounterParty, "Please select " + ApplicationConstants.CONST_BROKER);
                        return;
                    }
                }
                if (chkDescription.Checked)
                {
                    if (!string.IsNullOrEmpty(txtDescription.Text))
                    {
                        bulkChanges.Description = txtDescription.Text;
                        isAnyCheckboxSelected = true;
                        statusProvider.SetError(txtDescription, "");
                    }
                    else
                    {
                        statusProvider.SetError(txtDescription, "Please check Description, it should not be blank.");
                        return;
                    }
                }
                if (chkInternalComments.Checked)
                {
                    if (!string.IsNullOrEmpty(txtInternaComments.Text))
                    {
                        bulkChanges.InternalComments = txtInternaComments.Text;
                        isAnyCheckboxSelected = true;
                        statusProvider.SetError(txtInternaComments, "");
                    }
                    else
                    {
                        statusProvider.SetError(txtDescription, "Please check Internal Comments, it should not be blank.");
                        return;
                    }
                }
                if (chkFXRate.Checked)
                {
                    string fxConversionMethodOperator = cmbFXConversionOperator.Text;
                    if (txtFXRate.Value > 0)
                    {
                        bulkChanges.FXRate = txtFXRate.Value;
                        isAnyCheckboxSelected = true;
                        statusProvider.SetError(txtFXRate, "");
                    }
                    else
                    {
                        statusProvider.SetError(txtFXRate, "Please check FX Rate, it is less than or equal to 0.");
                        return;
                    }
                    if (!fxConversionMethodOperator.Equals(string.Empty) && !fxConversionMethodOperator.Equals(ApplicationConstants.C_COMBO_SELECT))
                    {
                        bulkChanges.FXConversionOperator = fxConversionMethodOperator;
                        isAnyCheckboxSelected = true;
                        statusProvider.SetError(cmbFXConversionOperator, "");
                    }
                    else
                    {
                        statusProvider.SetError(cmbFXConversionOperator, "Please select FX Conversion Operator");
                        return;
                    }
                    if (!rdbtnGroup.Checked)
                    {
                        bulkChanges.GroupWise = false;
                        string pbName = cmbFXPrimeBroker.Text;
                        string mfName = cmbMasterFund.Text;
                        if (mfName.Equals(string.Empty) || mfName.Equals(ApplicationConstants.C_COMBO_SELECT))
                        {
                            if (pbName.Equals(string.Empty) || pbName.Equals(ApplicationConstants.C_COMBO_SELECT))
                            {
                                statusProvider.SetError(cmbMasterFund, "Please select MasterFund to update FX Rate");
                                return;
                            }
                        }
                        if (chkLstPrimeBrokerAccounts.Items.Count > 0)
                        {
                            if (chkLstPrimeBrokerAccounts.CheckedItems.Count > 0)
                            {
                                bulkChanges.AccountIDs = GetSelectedAccountIDs();
                                statusProvider.SetError(chkLstPrimeBrokerAccounts, "");
                            }
                            else
                            {
                                statusProvider.SetError(chkLstPrimeBrokerAccounts, "Please select atleast one account");
                                return;
                            }
                        }
                        else
                        {
                            statusProvider.SetError(chkLstPrimeBrokerAccounts, "Please select atleast one account");
                            return;
                        }
                    }
                }

                if (isAnyCheckboxSelected)
                {
                    statusProvider.Clear();
                    if (BulkChangeOnGroupLevel != null)
                    {
                        BulkChangeOnGroupLevel(bulkChanges, null);
                    }
                }
                else
                {
                    statusProvider.Clear();
                    //statusProvider.SetError(btnUpdateBulkChanges, "Please enter/select atleast one value.");
                    if (DisplayMessage != null)
                    {
                        DisplayMessage(null, EventArgs.Empty);
                    }

                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkSelectAllBulkChanges_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSelectAllBulkChanges.Checked)
                {
                    ToggleUIItemsForBulkChanges(true);
                }

                else
                {
                    ToggleUIItemsForBulkChanges(false);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ToggleUIItemsForBulkChanges(bool status)
        {
            try
            {
                chkAVGPrice.Checked = status;
                txtAvgPrice.Enabled = status;

                //Enabling numericRoundAvgPx/chckRoundAvgPx 
                numericRoundAvgPx.Enabled = status;
                chckRoundAvgPx.Checked = status;  

                chkFXRate.Checked = status;
                txtFXRate.Enabled = status;

                chkAccruedInterest.Checked = status;
                txtAccruedInterest.Enabled = status;

                chkCounterParty.Checked = status;
                cmbCounterParty.Enabled = status;

                chkDescription.Checked = status;
                txtDescription.Enabled = status;

                chkInternalComments.Checked = status;
                txtInternaComments.Enabled = status;
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

        private void chkAVGPrice_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                EnableUI(chkAVGPrice, txtAvgPrice);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkFXRate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkFXRate.Checked)
                {
                    txtFXRate.Enabled = true;
                    cmbFXConversionOperator.Enabled = true;
                    //if (rdbtnTaxlot.Checked)
                    //{
                    //ToggleFXPanelControlsState(true);
                    //ApplyAccountsCheckBoxState(CheckState.Checked);
                    //}
                    //else
                    //{
                    //    ToggleFXPanelControlsState(false);
                    //    ApplyAccountsCheckBoxState(CheckState.Indeterminate);
                    //}
                }
                else
                {
                    txtFXRate.Enabled = false;
                    cmbFXConversionOperator.Enabled = false;
                    //ToggleFXPanelControlsState(false);
                    //ApplyAccountsCheckBoxState(CheckState.Indeterminate);
                    //SelectAllCheckBoxState(false);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ApplyAccountsCheckBoxState(CheckState state)
        {
            try
            {
                if (chkLstPrimeBrokerAccounts.Items.Count > 0 && chkLstPrimeBrokerAccounts.CheckedItems.Count > 0)
                {
                    for (int i = 0; i < chkLstPrimeBrokerAccounts.CheckedItems.Count; i++)
                    {
                        chkLstPrimeBrokerAccounts.SetItemCheckState(i, state);
                    }
                }
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

        private void chkAccruedInterest_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                EnableUI(chkAccruedInterest, txtAccruedInterest);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkDescription_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtDescription.Enabled = chkDescription.Checked ? true : false;
                SelectAllCheckBoxState(chkDescription.Checked);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void EnableUI(CheckBox check, NumericUpDown textbox)
        {
            try
            {
                textbox.Enabled = check.Checked ? true : false;
                SelectAllCheckBoxState(check.Checked);
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

        private void SelectAllCheckBoxState(bool checkState)
        {
            try
            {
                if (!checkState && chkSelectAllBulkChanges.Checked)
                {
                    chkSelectAllBulkChanges.CheckedChanged -= new EventHandler(chkSelectAllBulkChanges_CheckedChanged);
                    chkSelectAllBulkChanges.Checked = false;
                    chkSelectAllBulkChanges.CheckedChanged += new EventHandler(chkSelectAllBulkChanges_CheckedChanged);
                }
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

        private void chkCounterParty_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkCounterParty.Checked)
                {
                    cmbCounterParty.Enabled = true;
                }
                else
                {
                    cmbCounterParty.Enabled = false;
                    SelectAllCheckBoxState(false);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbFXPrimeBroker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMasterFund.Value != null)
                {
                    int masterFundID = int.Parse(cmbMasterFund.Value.ToString());
                    string thirdPartyName = string.Empty;

                    if (!masterFundID.Equals(int.MinValue))
                    {
                        List<int> accountIDs = _masterFundAssociation[masterFundID];
                        List<int> pbAccountIDs = GetThirdPartyName();

                        if (pbAccountIDs != null)
                        {
                            chkLstPrimeBrokerAccounts.Items.Clear();
                            foreach (int accountID in accountIDs)
                            {
                                foreach (int pbAccountID in pbAccountIDs)
                                {
                                    if (accountID.Equals(pbAccountID))
                                    {
                                        BindAccount(pbAccountID);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        chkLstPrimeBrokerAccounts.Items.Clear();
                        List<int> accountIDs = GetThirdPartyName();
                        if (accountIDs != null)
                        {
                            foreach (int accountID in accountIDs)
                            {
                                BindAccount(accountID);
                            }
                        }

                    }
                }
                if (chkLstPrimeBrokerAccounts.Items.Count > 0)
                {
                    SetAllAccountsCheckState(true);
                }
                else
                {
                    SetAllAccountsCheckState(false);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetAllAccountsCheckState(bool check)
        {
            chkSelectAllAccounts.CheckedChanged -= new EventHandler(chkSelectAllAccounts_CheckedChanged);
            chkSelectAllAccounts.Checked = check;
            chkSelectAllAccounts.CheckedChanged += new EventHandler(chkSelectAllAccounts_CheckedChanged);
        }

        private List<int> GetThirdPartyName()
        {
            List<int> accountIDs = null;
            try
            {
                if (cmbFXPrimeBroker.Value != null)
                {
                    string thirdPartyName = cmbFXPrimeBroker.Text;
                    if (!thirdPartyName.Equals(ApplicationConstants.C_COMBO_SELECT) && !thirdPartyName.Equals(string.Empty))
                    {
                        accountIDs = GetAccountIDByPBName(thirdPartyName);
                    }
                }
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
            return accountIDs;
        }

        private void BindAccount(int accountID)
        {
            try
            {
                string accountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(accountID);
                chkLstPrimeBrokerAccounts.Items.Add(accountName);
                for (int j = 0; j < chkLstPrimeBrokerAccounts.Items.Count; j++)
                {
                    chkLstPrimeBrokerAccounts.SetItemCheckState(j, CheckState.Checked);
                }
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

        private void cmbMasterFund_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMasterFund.Value != null)
                {
                    int masterFundID = int.Parse(cmbMasterFund.Value.ToString());

                    chkLstPrimeBrokerAccounts.Items.Clear();

                    cmbFXPrimeBroker.ValueChanged -= new EventHandler(cmbFXPrimeBroker_ValueChanged);
                    cmbFXPrimeBroker.Text = ApplicationConstants.C_COMBO_SELECT;
                    cmbFXPrimeBroker.ValueChanged += new EventHandler(cmbFXPrimeBroker_ValueChanged);

                    if (!masterFundID.Equals(int.MinValue))
                    {
                        List<int> accountIDs = _masterFundAssociation[masterFundID];
                        if (accountIDs != null)
                        {
                            foreach (int accountID in accountIDs)
                            {
                                BindAccount(accountID);
                            }
                        }
                    }
                }
                if (chkLstPrimeBrokerAccounts.Items.Count > 0)
                {
                    SetAllAccountsCheckState(true);
                }
                else
                {
                    SetAllAccountsCheckState(false);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void rdbtnGroup_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chkFXRate.Enabled = true;
                if (rdbtnGroup.Checked)
                {
                    ToggleCheckBoxesState(true);
                    ToggleFXPanelControlsState(false);
                    ApplyAccountsCheckBoxState(CheckState.Indeterminate);
                    rdBtnDefaultRules.Checked = true;
                }
                else if (rdbtnTaxlot.Checked)
                {
                    chkAccruedInterest.Checked = false;
                    chkAVGPrice.Checked = false;
                    chkCounterParty.Checked = false;
                    chkDescription.Checked = false;
                    rdBtnSelectCommissionRule.Checked = true;
                    chkFXRate.Checked = true;
                    
                    //Disable chckRoundAvgPx
                    chckRoundAvgPx.Checked = false;
                    ToggleCheckBoxesState(false);

                    //if (chkFXRate.Checked)
                    //{
                    ToggleFXPanelControlsState(true);
                    ApplyAccountsCheckBoxState(CheckState.Checked);
                    //}
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ToggleCheckBoxesState(bool shouldCheck)
        {
            try
            {
                chkAccruedInterest.Enabled = shouldCheck;
                chkAVGPrice.Enabled = shouldCheck;
                chkCounterParty.Enabled = shouldCheck;
                chkDescription.Enabled = shouldCheck;
                rdBtnDefaultRules.Enabled = shouldCheck;
                chkSelectAllBulkChanges.Enabled = shouldCheck;
                chckRoundAvgPx.Enabled = shouldCheck; // Enabling CheckBox for Round Avg Price
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Toggles the FX Panel in the Update group based on selection of Group and taxlot level
        /// </summary>
        /// <param name="state"></param>
        private void ToggleFXPanelControlsState(bool state)
        {
            try
            {
                chkSelectAllAccounts.Enabled = state;
                cmbMasterFund.Enabled = state;
                cmbFXPrimeBroker.Enabled = state;
                chkLstPrimeBrokerAccounts.Enabled = state;
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

        /// <summary>
        /// return the list of account ids based on selected prime broker.
        /// </summary>
        /// <param name="pbName"></param>
        /// <returns></returns>
        public List<int> GetAccountIDByPBName(string pbName)
        {
            List<int> accountIDs = new List<int>();
            try
            {
                foreach (KeyValuePair<int, string> kvp in _accountPBDetails)
                {
                    if (string.Compare(kvp.Value, pbName, true) == 0)
                    {
                        accountIDs.Add(kvp.Key);
                    }
                }
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
            return accountIDs;
        }

        private void chkSelectAllAccounts_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSelectAllAccounts.Checked)
                {
                    for (int j = 0; j < chkLstPrimeBrokerAccounts.Items.Count; j++)
                    {
                        chkLstPrimeBrokerAccounts.SetItemChecked(j, true);
                    }
                }
                else
                {
                    for (int j = 0; j < chkLstPrimeBrokerAccounts.Items.Count; j++)
                    {
                        chkLstPrimeBrokerAccounts.SetItemChecked(j, false);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkLstPrimeBrokerAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkLstPrimeBrokerAccounts.CheckedItems.Count < chkLstPrimeBrokerAccounts.Items.Count)
                {
                    SetAllAccountsCheckState(false);
                }
                else if (chkLstPrimeBrokerAccounts.CheckedItems.Count.Equals(chkLstPrimeBrokerAccounts.Items.Count))
                {
                    SetAllAccountsCheckState(true);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        private void cbSoftCommission_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToggleFeesControlsState(cbSoftCommission, txtSoftCommissionRate, cmbSoftCommissionBasedOn);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cbClearingBrokerFee_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ToggleFeesControlsState(cbClearingBrokerFee, txtClearingBrokerFeeRate, cmbClearingBrokerFeeBasedOn);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chkInternalComments_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtInternaComments.Enabled = chkInternalComments.Checked ? true : false;
                SelectAllCheckBoxState(chkInternalComments.Checked);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Calling method EnableUI() for enabling CheckBox and Numeric Editor
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">Eventargs</param>
        private void chckRoundAvgPx_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                EnableUI(chckRoundAvgPx, numericRoundAvgPx);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Checking Numeric Editor Value, If Grater than 15 then it will show message to user
        /// that Round avg px cannot be grater than 15 Because Math.Round() round the number upto 15 decimal places
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericRoundAvgPx_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                var txt = (TextBox)numericRoundAvgPx.Controls[1];
                int val = Convert.ToInt32(txt.Text);
                if (val > 15 || val < 0)
                {
                    statusProvider.SetError(numericRoundAvgPx, "Round Avg Px can not be grater than 15 or less than 0");
                }
                else
                {
                    statusProvider.SetError(numericRoundAvgPx, System.String.Empty);
                }
            }
            catch
            {
                statusProvider.SetError(numericRoundAvgPx, "Round Avg Px is invalid");
            }
        }

    }
}
