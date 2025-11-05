#region Using
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CommissionRule.
    /// </summary>
    public class CommissionRule : System.Windows.Forms.UserControl
    {
        const string C_COMBO_SELECT = "- Select -";
        private const string FORM_NAME = "Commission Rule : ";
        const int ZERO = 0;
        const int PER_UNIT_COMMISSIONRATE = 0;
        const int BASIS_POINT_COMMISSIONRATE = 0;

        //private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownCommissionRate;
        private System.Windows.Forms.GroupBox grpRule;
        private System.Windows.Forms.Label lblNameofRule;
        private System.Windows.Forms.Label lblApplyRuleTo;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtNamefRule;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbApplyRuleto;
        private System.Windows.Forms.GroupBox grpParameters;
        private System.Windows.Forms.Label lblCalculation;
        private System.Windows.Forms.Label lblCurrencyUsed;
        private System.Windows.Forms.Label lblCommissionRate;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCurrencyUsed;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCalculation;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCommissionRate;
        private System.Windows.Forms.NumericUpDown nudCommissionRate;
        private System.Windows.Forms.GroupBox grpCriteria;
        private System.Windows.Forms.CheckBox chkACofRule;
        private System.Windows.Forms.Label lblCriteria;
        private System.Windows.Forms.Label lblCommissionRate1;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCriteria;
        private System.Windows.Forms.NumericUpDown nudCommissionRate4;
        private System.Windows.Forms.TextBox txtDescription;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAUEC;
        private System.Windows.Forms.Label lblAUEC;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Label lbldisplay;
        private UltraGrid grdCommissionRules;
        private UltraDropDown drpDownCommisionRateType;
        private GroupBox groupBox1;
        private Label lbldisplayClear;
        private Label label9;
        private Label label10;
        private NumericUpDown nudCommissionRateClear;
        private UltraCombo cmbCalculationClear;
        private UltraCombo cmbCurrencyUsedClear;
        private Label label12;
        private Label label13;
        private CheckBox chkClearFee;
        private IContainer components;

        public CommissionRule()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (grpRule != null)
                {
                    grpRule.Dispose();
                }
                if (lblApplyRuleTo != null)
                {
                    lblApplyRuleTo.Dispose();
                }
                if (lblDescription != null)
                {
                    lblDescription.Dispose();
                }
                if (txtNamefRule != null)
                {
                    txtNamefRule.Dispose();
                }
                if (cmbApplyRuleto != null)
                {
                    cmbApplyRuleto.Dispose();
                }
                if (grpParameters != null)
                {
                    grpParameters.Dispose();
                }
                if (lblCalculation != null)
                {
                    lblCalculation.Dispose();
                }
                if (lblCurrencyUsed != null)
                {
                    lblCurrencyUsed.Dispose();
                }
                if (lblCommissionRate != null)
                {
                    lblCommissionRate.Dispose();
                }
                if (cmbCurrencyUsed != null)
                {
                    cmbCurrencyUsed.Dispose();
                }
                if (cmbCalculation != null)
                {
                    cmbCalculation.Dispose();
                }
                if (cmbCommissionRate != null)
                {
                    cmbCommissionRate.Dispose();
                }
                if (nudCommissionRate != null)
                {
                    nudCommissionRate.Dispose();
                }
                if (grpCriteria != null)
                {
                    grpCriteria.Dispose();
                }
                if (chkACofRule != null)
                {
                    chkACofRule.Dispose();
                }
                if (lblCriteria != null)
                {
                    lblCriteria.Dispose();
                }
                if (lblCommissionRate1 != null)
                {
                    lblCommissionRate1.Dispose();
                }
                if (cmbCriteria != null)
                {
                    cmbCriteria.Dispose();
                }
                if (nudCommissionRate4 != null)
                {
                    nudCommissionRate4.Dispose();
                }
                if (txtDescription != null)
                {
                    txtDescription.Dispose();
                }
                if (cmbAUEC != null)
                {
                    cmbAUEC.Dispose();
                }
                if (lblAUEC != null)
                {
                    lblAUEC.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (lbldisplay != null)
                {
                    lbldisplay.Dispose();
                }
                if (grdCommissionRules != null)
                {
                    grdCommissionRules.Dispose();
                }
                if (drpDownCommisionRateType != null)
                {
                    drpDownCommisionRateType.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (lbldisplayClear != null)
                {
                    lbldisplayClear.Dispose();
                }
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if (label10 != null)
                {
                    label10.Dispose();
                }
                if (nudCommissionRateClear != null)
                {
                    nudCommissionRateClear.Dispose();
                }
                if (cmbCalculationClear != null)
                {
                    cmbCalculationClear.Dispose();
                }
                if (cmbCurrencyUsedClear != null)
                {
                    cmbCurrencyUsedClear.Dispose();
                }
                if (label12 != null)
                {
                    label12.Dispose();
                }
                if (label13 != null)
                {
                    label13.Dispose();
                }
                if (chkClearFee != null)
                {
                    chkClearFee.Dispose();
                }
                if (lblNameofRule != null)
                {
                    lblNameofRule.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateTypeName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateTypeID", 1);
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ApplyRuleID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradeType", 1);
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateTypeName", 1);
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionCalculationID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CalculationType", 1);
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand6 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurencyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencySymbol", 2);
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand7 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ValueFrom", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ValueTo", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateID_FK", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommisionRate", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TextCaption", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DeleteButton", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionCriteriaID_FK", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRuleCriteriaID", 7);
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand8 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Unit Name", 1);
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand9 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionCalculationID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CalculationType", 1);
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand10 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurencyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencySymbol", 2);
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            this.lblAUEC = new System.Windows.Forms.Label();
            this.cmbAUEC = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.grpRule = new System.Windows.Forms.GroupBox();
            this.drpDownCommisionRateType = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbApplyRuleto = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.txtNamefRule = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblApplyRuleTo = new System.Windows.Forms.Label();
            this.lblNameofRule = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.grpParameters = new System.Windows.Forms.GroupBox();
            this.lbldisplay = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudCommissionRate = new System.Windows.Forms.NumericUpDown();
            this.cmbCommissionRate = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCalculation = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCurrencyUsed = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblCommissionRate = new System.Windows.Forms.Label();
            this.lblCurrencyUsed = new System.Windows.Forms.Label();
            this.lblCalculation = new System.Windows.Forms.Label();
            this.grpCriteria = new System.Windows.Forms.GroupBox();
            this.grdCommissionRules = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.nudCommissionRate4 = new System.Windows.Forms.NumericUpDown();
            this.cmbCriteria = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblCommissionRate1 = new System.Windows.Forms.Label();
            this.lblCriteria = new System.Windows.Forms.Label();
            this.chkACofRule = new System.Windows.Forms.CheckBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkClearFee = new System.Windows.Forms.CheckBox();
            this.lbldisplayClear = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nudCommissionRateClear = new System.Windows.Forms.NumericUpDown();
            this.cmbCalculationClear = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCurrencyUsedClear = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAUEC)).BeginInit();
            this.grpRule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.drpDownCommisionRateType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbApplyRuleto)).BeginInit();
            this.grpParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommissionRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCalculation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrencyUsed)).BeginInit();
            this.grpCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCommissionRules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCriteria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRateClear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCalculationClear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrencyUsedClear)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAUEC
            // 
            this.lblAUEC.AutoSize = true;
            this.lblAUEC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAUEC.Location = new System.Drawing.Point(10, 16);
            this.lblAUEC.Name = "lblAUEC";
            this.lblAUEC.Size = new System.Drawing.Size(34, 13);
            this.lblAUEC.TabIndex = 9;
            this.lblAUEC.Text = "AUEC";
            // 
            // cmbAUEC
            // 
            this.cmbAUEC.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAUEC.DisplayLayout.Appearance = appearance1;
            this.cmbAUEC.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.cmbAUEC.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbAUEC.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAUEC.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAUEC.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbAUEC.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAUEC.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbAUEC.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAUEC.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAUEC.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAUEC.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbAUEC.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAUEC.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAUEC.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbAUEC.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAUEC.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbAUEC.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbAUEC.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAUEC.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbAUEC.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbAUEC.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAUEC.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbAUEC.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAUEC.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAUEC.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAUEC.DisplayMember = "";
            this.cmbAUEC.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAUEC.DropDownWidth = 0;
            this.cmbAUEC.UseFlatMode = DefaultableBoolean.True;
            this.cmbAUEC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbAUEC.Location = new System.Drawing.Point(62, 14);
            this.cmbAUEC.Name = "cmbAUEC";
            this.cmbAUEC.Size = new System.Drawing.Size(174, 21);
            this.cmbAUEC.TabIndex = 1;
            this.cmbAUEC.ValueMember = "";
            this.cmbAUEC.LostFocus += new System.EventHandler(this.cmbAUEC_LostFocus);
            this.cmbAUEC.ValueChanged += new System.EventHandler(this.cmbAUEC_ValueChanged);
            this.cmbAUEC.GotFocus += new System.EventHandler(this.cmbAUEC_GotFocus);
            // 
            // grpRule
            // 
            this.grpRule.Controls.Add(this.drpDownCommisionRateType);
            this.grpRule.Controls.Add(this.label2);
            this.grpRule.Controls.Add(this.label1);
            this.grpRule.Controls.Add(this.label3);
            this.grpRule.Controls.Add(this.cmbApplyRuleto);
            this.grpRule.Controls.Add(this.txtNamefRule);
            this.grpRule.Controls.Add(this.lblDescription);
            this.grpRule.Controls.Add(this.lblApplyRuleTo);
            this.grpRule.Controls.Add(this.lblNameofRule);
            this.grpRule.Controls.Add(this.txtDescription);
            this.grpRule.Controls.Add(this.lblAUEC);
            this.grpRule.Controls.Add(this.cmbAUEC);
            this.grpRule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpRule.Location = new System.Drawing.Point(2, 4);
            this.grpRule.Name = "grpRule";
            this.grpRule.Size = new System.Drawing.Size(609, 110);
            this.grpRule.TabIndex = 1;
            this.grpRule.TabStop = false;
            this.grpRule.Text = "Rule";
            // 
            // drpDownCommisionRateType
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.drpDownCommisionRateType.DisplayLayout.Appearance = appearance13;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn3,
            ultraGridColumn4});
            this.drpDownCommisionRateType.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.drpDownCommisionRateType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.drpDownCommisionRateType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.drpDownCommisionRateType.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.drpDownCommisionRateType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.drpDownCommisionRateType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.drpDownCommisionRateType.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.drpDownCommisionRateType.DisplayLayout.MaxColScrollRegions = 1;
            this.drpDownCommisionRateType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.drpDownCommisionRateType.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.drpDownCommisionRateType.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.drpDownCommisionRateType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.drpDownCommisionRateType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.drpDownCommisionRateType.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.drpDownCommisionRateType.DisplayLayout.Override.CellAppearance = appearance20;
            this.drpDownCommisionRateType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.drpDownCommisionRateType.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.drpDownCommisionRateType.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlign = Infragistics.Win.HAlign.Left;
            this.drpDownCommisionRateType.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.drpDownCommisionRateType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.drpDownCommisionRateType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.drpDownCommisionRateType.DisplayLayout.Override.RowAppearance = appearance23;
            this.drpDownCommisionRateType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.drpDownCommisionRateType.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.drpDownCommisionRateType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.drpDownCommisionRateType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.drpDownCommisionRateType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.drpDownCommisionRateType.DisplayMember = "";
            this.drpDownCommisionRateType.Location = new System.Drawing.Point(483, 16);
            this.drpDownCommisionRateType.Name = "drpDownCommisionRateType";
            this.drpDownCommisionRateType.Size = new System.Drawing.Size(120, 36);
            this.drpDownCommisionRateType.TabIndex = 38;
            this.drpDownCommisionRateType.Text = "ultraDropDown1";
            this.drpDownCommisionRateType.ValueMember = "";
            this.drpDownCommisionRateType.Visible = false;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(310, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(8, 9);
            this.label2.TabIndex = 37;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(74, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 9);
            this.label1.TabIndex = 36;
            this.label1.Text = "*";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(42, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 9);
            this.label3.TabIndex = 35;
            this.label3.Text = "*";
            // 
            // cmbApplyRuleto
            // 
            this.cmbApplyRuleto.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbApplyRuleto.DisplayLayout.Appearance = appearance25;
            this.cmbApplyRuleto.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridColumn5.Header.VisiblePosition = 0;
            ultraGridColumn5.Hidden = true;
            ultraGridColumn6.Header.VisiblePosition = 1;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn5,
            ultraGridColumn6});
            this.cmbApplyRuleto.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbApplyRuleto.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbApplyRuleto.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbApplyRuleto.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbApplyRuleto.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.cmbApplyRuleto.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbApplyRuleto.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.cmbApplyRuleto.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbApplyRuleto.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbApplyRuleto.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbApplyRuleto.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.cmbApplyRuleto.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbApplyRuleto.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.cmbApplyRuleto.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbApplyRuleto.DisplayLayout.Override.CellAppearance = appearance32;
            this.cmbApplyRuleto.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbApplyRuleto.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbApplyRuleto.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbApplyRuleto.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.cmbApplyRuleto.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbApplyRuleto.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.cmbApplyRuleto.DisplayLayout.Override.RowAppearance = appearance35;
            this.cmbApplyRuleto.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbApplyRuleto.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.cmbApplyRuleto.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbApplyRuleto.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbApplyRuleto.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbApplyRuleto.DisplayMember = "";
            this.cmbApplyRuleto.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbApplyRuleto.DropDownWidth = 0;
            this.cmbApplyRuleto.UseFlatMode = DefaultableBoolean.True;
            this.cmbApplyRuleto.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbApplyRuleto.Location = new System.Drawing.Point(328, 38);
            this.cmbApplyRuleto.Name = "cmbApplyRuleto";
            this.cmbApplyRuleto.Size = new System.Drawing.Size(138, 21);
            this.cmbApplyRuleto.TabIndex = 3;
            this.cmbApplyRuleto.ValueMember = "";
            this.cmbApplyRuleto.LostFocus += new System.EventHandler(this.cmbApplyRuleto_LostFocus);
            this.cmbApplyRuleto.GotFocus += new System.EventHandler(this.cmbApplyRuleto_GotFocus);
            // 
            // txtNamefRule
            // 
            this.txtNamefRule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtNamefRule.Location = new System.Drawing.Point(92, 38);
            this.txtNamefRule.MaxLength = 15;
            this.txtNamefRule.Name = "txtNamefRule";
            this.txtNamefRule.Size = new System.Drawing.Size(144, 21);
            this.txtNamefRule.TabIndex = 2;
            this.txtNamefRule.LostFocus += new System.EventHandler(this.txtNamefRule_LostFocus);
            this.txtNamefRule.GotFocus += new System.EventHandler(this.txtNamefRule_GotFocus);
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblDescription.Location = new System.Drawing.Point(8, 76);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(100, 14);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Description of Rule";
            // 
            // lblApplyRuleTo
            // 
            this.lblApplyRuleTo.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblApplyRuleTo.Location = new System.Drawing.Point(242, 40);
            this.lblApplyRuleTo.Name = "lblApplyRuleTo";
            this.lblApplyRuleTo.Size = new System.Drawing.Size(74, 16);
            this.lblApplyRuleTo.TabIndex = 1;
            this.lblApplyRuleTo.Text = "Apply Rule To";
            // 
            // lblNameofRule
            // 
            this.lblNameofRule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblNameofRule.Location = new System.Drawing.Point(8, 40);
            this.lblNameofRule.Name = "lblNameofRule";
            this.lblNameofRule.Size = new System.Drawing.Size(72, 16);
            this.lblNameofRule.TabIndex = 0;
            this.lblNameofRule.Text = "Name of Rule";
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDescription.Location = new System.Drawing.Point(114, 64);
            this.txtDescription.MaxLength = 50;
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(352, 40);
            this.txtDescription.TabIndex = 4;
            this.txtDescription.LostFocus += new System.EventHandler(this.txtDescription_LostFocus);
            this.txtDescription.GotFocus += new System.EventHandler(this.txtDescription_GotFocus);
            // 
            // grpParameters
            // 
            this.grpParameters.Controls.Add(this.lbldisplay);
            this.grpParameters.Controls.Add(this.label6);
            this.grpParameters.Controls.Add(this.label5);
            this.grpParameters.Controls.Add(this.label4);
            this.grpParameters.Controls.Add(this.nudCommissionRate);
            this.grpParameters.Controls.Add(this.cmbCommissionRate);
            this.grpParameters.Controls.Add(this.cmbCalculation);
            this.grpParameters.Controls.Add(this.cmbCurrencyUsed);
            this.grpParameters.Controls.Add(this.lblCommissionRate);
            this.grpParameters.Controls.Add(this.lblCurrencyUsed);
            this.grpParameters.Controls.Add(this.lblCalculation);
            this.grpParameters.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpParameters.Location = new System.Drawing.Point(2, 116);
            this.grpParameters.Name = "grpParameters";
            this.grpParameters.Size = new System.Drawing.Size(609, 64);
            this.grpParameters.TabIndex = 2;
            this.grpParameters.TabStop = false;
            this.grpParameters.Text = "Parameters";
            // 
            // lbldisplay
            // 
            this.lbldisplay.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbldisplay.Location = new System.Drawing.Point(371, 38);
            this.lbldisplay.Name = "lbldisplay";
            this.lbldisplay.Size = new System.Drawing.Size(217, 16);
            this.lbldisplay.TabIndex = 38;
            this.lbldisplay.Text = "lbldisplay";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(336, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(8, 9);
            this.label6.TabIndex = 37;
            this.label6.Text = "*";
            this.label6.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(213, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(66, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "*";
            // 
            // nudCommissionRate
            // 
            this.nudCommissionRate.BackColor = System.Drawing.Color.White;
            this.nudCommissionRate.DecimalPlaces = 4;
            this.nudCommissionRate.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudCommissionRate.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudCommissionRate.Location = new System.Drawing.Point(265, 36);
            this.nudCommissionRate.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudCommissionRate.Name = "nudCommissionRate";
            this.nudCommissionRate.Size = new System.Drawing.Size(100, 21);
            this.nudCommissionRate.TabIndex = 8;
            // 
            // cmbCommissionRate
            // 
            this.cmbCommissionRate.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance37.BackColor = System.Drawing.SystemColors.Window;
            appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCommissionRate.DisplayLayout.Appearance = appearance37;
            this.cmbCommissionRate.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand4.ColHeadersVisible = false;
            ultraGridColumn7.Header.VisiblePosition = 0;
            ultraGridColumn7.Hidden = true;
            ultraGridColumn8.Header.VisiblePosition = 1;
            ultraGridBand4.Columns.AddRange(new object[] {
            ultraGridColumn7,
            ultraGridColumn8});
            this.cmbCommissionRate.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.cmbCommissionRate.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCommissionRate.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance38.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance38.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate.DisplayLayout.GroupByBox.Appearance = appearance38;
            appearance39.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCommissionRate.DisplayLayout.GroupByBox.BandLabelAppearance = appearance39;
            this.cmbCommissionRate.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance40.BackColor2 = System.Drawing.SystemColors.Control;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance40.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCommissionRate.DisplayLayout.GroupByBox.PromptAppearance = appearance40;
            this.cmbCommissionRate.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCommissionRate.DisplayLayout.MaxRowScrollRegions = 1;
            appearance41.BackColor = System.Drawing.SystemColors.Window;
            appearance41.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCommissionRate.DisplayLayout.Override.ActiveCellAppearance = appearance41;
            appearance42.BackColor = System.Drawing.SystemColors.Highlight;
            appearance42.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCommissionRate.DisplayLayout.Override.ActiveRowAppearance = appearance42;
            this.cmbCommissionRate.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCommissionRate.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance43.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate.DisplayLayout.Override.CardAreaAppearance = appearance43;
            appearance44.BorderColor = System.Drawing.Color.Silver;
            appearance44.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCommissionRate.DisplayLayout.Override.CellAppearance = appearance44;
            this.cmbCommissionRate.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCommissionRate.DisplayLayout.Override.CellPadding = 0;
            appearance45.BackColor = System.Drawing.SystemColors.Control;
            appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance45.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance45.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate.DisplayLayout.Override.GroupByRowAppearance = appearance45;
            appearance46.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCommissionRate.DisplayLayout.Override.HeaderAppearance = appearance46;
            this.cmbCommissionRate.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCommissionRate.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            appearance47.BorderColor = System.Drawing.Color.Silver;
            this.cmbCommissionRate.DisplayLayout.Override.RowAppearance = appearance47;
            this.cmbCommissionRate.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance48.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCommissionRate.DisplayLayout.Override.TemplateAddRowAppearance = appearance48;
            this.cmbCommissionRate.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCommissionRate.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCommissionRate.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCommissionRate.DisplayMember = "";
            this.cmbCommissionRate.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCommissionRate.DropDownWidth = 0;
            this.cmbCommissionRate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCommissionRate.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCommissionRate.Location = new System.Drawing.Point(456, 13);
            this.cmbCommissionRate.Name = "cmbCommissionRate";
            this.cmbCommissionRate.Size = new System.Drawing.Size(132, 21);
            this.cmbCommissionRate.TabIndex = 7;
            this.cmbCommissionRate.ValueMember = "";
            this.cmbCommissionRate.Visible = false;
            this.cmbCommissionRate.LostFocus += new System.EventHandler(this.cmbCommissionRate_LostFocus);
            this.cmbCommissionRate.GotFocus += new System.EventHandler(this.cmbCommissionRate_GotFocus);
            // 
            // cmbCalculation
            // 
            this.cmbCalculation.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance49.BackColor = System.Drawing.SystemColors.Window;
            appearance49.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCalculation.DisplayLayout.Appearance = appearance49;
            this.cmbCalculation.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand5.ColHeadersVisible = false;
            ultraGridColumn9.Header.VisiblePosition = 0;
            ultraGridColumn9.Hidden = true;
            ultraGridColumn10.Header.VisiblePosition = 1;
            ultraGridBand5.Columns.AddRange(new object[] {
            ultraGridColumn9,
            ultraGridColumn10});
            this.cmbCalculation.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
            this.cmbCalculation.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCalculation.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance50.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance50.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance50.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance50.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCalculation.DisplayLayout.GroupByBox.Appearance = appearance50;
            appearance51.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCalculation.DisplayLayout.GroupByBox.BandLabelAppearance = appearance51;
            this.cmbCalculation.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance52.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance52.BackColor2 = System.Drawing.SystemColors.Control;
            appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance52.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCalculation.DisplayLayout.GroupByBox.PromptAppearance = appearance52;
            this.cmbCalculation.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCalculation.DisplayLayout.MaxRowScrollRegions = 1;
            appearance53.BackColor = System.Drawing.SystemColors.Window;
            appearance53.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCalculation.DisplayLayout.Override.ActiveCellAppearance = appearance53;
            appearance54.BackColor = System.Drawing.SystemColors.Highlight;
            appearance54.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCalculation.DisplayLayout.Override.ActiveRowAppearance = appearance54;
            this.cmbCalculation.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCalculation.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance55.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCalculation.DisplayLayout.Override.CardAreaAppearance = appearance55;
            appearance56.BorderColor = System.Drawing.Color.Silver;
            appearance56.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCalculation.DisplayLayout.Override.CellAppearance = appearance56;
            this.cmbCalculation.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCalculation.DisplayLayout.Override.CellPadding = 0;
            appearance57.BackColor = System.Drawing.SystemColors.Control;
            appearance57.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance57.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance57.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCalculation.DisplayLayout.Override.GroupByRowAppearance = appearance57;
            appearance58.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCalculation.DisplayLayout.Override.HeaderAppearance = appearance58;
            this.cmbCalculation.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCalculation.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance59.BackColor = System.Drawing.SystemColors.Window;
            appearance59.BorderColor = System.Drawing.Color.Silver;
            this.cmbCalculation.DisplayLayout.Override.RowAppearance = appearance59;
            this.cmbCalculation.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance60.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCalculation.DisplayLayout.Override.TemplateAddRowAppearance = appearance60;
            this.cmbCalculation.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCalculation.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCalculation.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCalculation.DisplayMember = "";
            this.cmbCalculation.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCalculation.DropDownWidth = 0;
            this.cmbCalculation.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCalculation.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCalculation.Location = new System.Drawing.Point(10, 36);
            this.cmbCalculation.Name = "cmbCalculation";
            this.cmbCalculation.Size = new System.Drawing.Size(102, 21);
            this.cmbCalculation.TabIndex = 5;
            this.cmbCalculation.ValueMember = "";
            this.cmbCalculation.LostFocus += new System.EventHandler(this.cmbCalculation_LostFocus);
            this.cmbCalculation.ValueChanged += new System.EventHandler(this.cmbCalculation_ValueChanged);
            this.cmbCalculation.GotFocus += new System.EventHandler(this.cmbCalculation_GotFocus);
            // 
            // cmbCurrencyUsed
            // 
            this.cmbCurrencyUsed.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance61.BackColor = System.Drawing.SystemColors.Window;
            appearance61.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCurrencyUsed.DisplayLayout.Appearance = appearance61;
            this.cmbCurrencyUsed.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand6.ColHeadersVisible = false;
            ultraGridColumn11.Header.VisiblePosition = 0;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn12.Header.VisiblePosition = 1;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn13.Header.VisiblePosition = 2;
            ultraGridBand6.Columns.AddRange(new object[] {
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13});
            this.cmbCurrencyUsed.DisplayLayout.BandsSerializer.Add(ultraGridBand6);
            this.cmbCurrencyUsed.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCurrencyUsed.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance62.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance62.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance62.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance62.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCurrencyUsed.DisplayLayout.GroupByBox.Appearance = appearance62;
            appearance63.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCurrencyUsed.DisplayLayout.GroupByBox.BandLabelAppearance = appearance63;
            this.cmbCurrencyUsed.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance64.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance64.BackColor2 = System.Drawing.SystemColors.Control;
            appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance64.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCurrencyUsed.DisplayLayout.GroupByBox.PromptAppearance = appearance64;
            this.cmbCurrencyUsed.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCurrencyUsed.DisplayLayout.MaxRowScrollRegions = 1;
            appearance65.BackColor = System.Drawing.SystemColors.Window;
            appearance65.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCurrencyUsed.DisplayLayout.Override.ActiveCellAppearance = appearance65;
            appearance66.BackColor = System.Drawing.SystemColors.Highlight;
            appearance66.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCurrencyUsed.DisplayLayout.Override.ActiveRowAppearance = appearance66;
            this.cmbCurrencyUsed.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCurrencyUsed.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance67.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCurrencyUsed.DisplayLayout.Override.CardAreaAppearance = appearance67;
            appearance68.BorderColor = System.Drawing.Color.Silver;
            appearance68.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCurrencyUsed.DisplayLayout.Override.CellAppearance = appearance68;
            this.cmbCurrencyUsed.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCurrencyUsed.DisplayLayout.Override.CellPadding = 0;
            appearance69.BackColor = System.Drawing.SystemColors.Control;
            appearance69.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance69.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance69.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance69.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCurrencyUsed.DisplayLayout.Override.GroupByRowAppearance = appearance69;
            appearance70.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCurrencyUsed.DisplayLayout.Override.HeaderAppearance = appearance70;
            this.cmbCurrencyUsed.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCurrencyUsed.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance71.BackColor = System.Drawing.SystemColors.Window;
            appearance71.BorderColor = System.Drawing.Color.Silver;
            this.cmbCurrencyUsed.DisplayLayout.Override.RowAppearance = appearance71;
            this.cmbCurrencyUsed.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance72.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCurrencyUsed.DisplayLayout.Override.TemplateAddRowAppearance = appearance72;
            this.cmbCurrencyUsed.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCurrencyUsed.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCurrencyUsed.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCurrencyUsed.DisplayMember = "";
            this.cmbCurrencyUsed.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCurrencyUsed.DropDownWidth = 0;
            this.cmbCurrencyUsed.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCurrencyUsed.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCurrencyUsed.Location = new System.Drawing.Point(135, 36);
            this.cmbCurrencyUsed.Name = "cmbCurrencyUsed";
            this.cmbCurrencyUsed.Size = new System.Drawing.Size(108, 21);
            this.cmbCurrencyUsed.TabIndex = 6;
            this.cmbCurrencyUsed.ValueMember = "";
            this.cmbCurrencyUsed.LostFocus += new System.EventHandler(this.cmbCurrencyUsed_LostFocus);
            this.cmbCurrencyUsed.GotFocus += new System.EventHandler(this.cmbCurrencyUsed_GotFocus);
            // 
            // lblCommissionRate
            // 
            this.lblCommissionRate.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCommissionRate.Location = new System.Drawing.Point(350, 11);
            this.lblCommissionRate.Name = "lblCommissionRate";
            this.lblCommissionRate.Size = new System.Drawing.Size(100, 16);
            this.lblCommissionRate.TabIndex = 2;
            this.lblCommissionRate.Text = "Commission Rate";
            this.lblCommissionRate.Visible = false;
            // 
            // lblCurrencyUsed
            // 
            this.lblCurrencyUsed.AutoSize = true;
            this.lblCurrencyUsed.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCurrencyUsed.Location = new System.Drawing.Point(137, 18);
            this.lblCurrencyUsed.Name = "lblCurrencyUsed";
            this.lblCurrencyUsed.Size = new System.Drawing.Size(78, 13);
            this.lblCurrencyUsed.TabIndex = 1;
            this.lblCurrencyUsed.Text = "Currency Used";
            // 
            // lblCalculation
            // 
            this.lblCalculation.AutoSize = true;
            this.lblCalculation.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCalculation.Location = new System.Drawing.Point(10, 18);
            this.lblCalculation.Name = "lblCalculation";
            this.lblCalculation.Size = new System.Drawing.Size(59, 13);
            this.lblCalculation.TabIndex = 0;
            this.lblCalculation.Text = "Calculation";
            // 
            // grpCriteria
            // 
            this.grpCriteria.Controls.Add(this.grdCommissionRules);
            this.grpCriteria.Controls.Add(this.nudCommissionRate4);
            this.grpCriteria.Controls.Add(this.cmbCriteria);
            this.grpCriteria.Controls.Add(this.lblCommissionRate1);
            this.grpCriteria.Controls.Add(this.lblCriteria);
            this.grpCriteria.Controls.Add(this.chkACofRule);
            this.grpCriteria.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpCriteria.Location = new System.Drawing.Point(2, 182);
            this.grpCriteria.Name = "grpCriteria";
            this.grpCriteria.Size = new System.Drawing.Size(609, 286);
            this.grpCriteria.TabIndex = 3;
            this.grpCriteria.TabStop = false;
            this.grpCriteria.Text = "Criteria";
            // 
            // grdCommissionRules
            // 
            ultraGridColumn14.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn14.ColSpan = ((short)(0));
            ultraGridColumn14.DataType = typeof(long);
            ultraGridColumn14.DefaultCellValue = ((long)(0));
            ultraGridColumn14.Header.Caption = "Value From (>=)";
            ultraGridColumn14.Header.ToolTipText = "Enter Range From (numeric)";
            ultraGridColumn14.Header.VisiblePosition = 0;
            ultraGridColumn14.InvalidValueBehavior = Infragistics.Win.UltraWinGrid.InvalidValueBehavior.RevertValue;
            ultraGridColumn14.MaxLength = 14;
            ultraGridColumn14.MinValue = ((short)(0));
            ultraGridColumn14.NullText = "0";
            ultraGridColumn14.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn14.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
            ultraGridColumn14.Width = 100;
            ultraGridColumn15.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn15.DataType = typeof(long);
            ultraGridColumn15.DefaultCellValue = ((long)(0));
            ultraGridColumn15.Header.Caption = "Value To (<=)";
            ultraGridColumn15.Header.ToolTipText = "Enter Range To (numeric)";
            ultraGridColumn15.Header.VisiblePosition = 1;
            ultraGridColumn15.InvalidValueBehavior = Infragistics.Win.UltraWinGrid.InvalidValueBehavior.RevertValue;
            ultraGridColumn15.MaxLength = 14;
            ultraGridColumn15.MinValue = ((short)(0));
            ultraGridColumn15.NullText = "0";
            ultraGridColumn15.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn15.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
            ultraGridColumn15.Width = 100;
            ultraGridColumn16.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn16.Header.Caption = "Commission on";
            ultraGridColumn16.Header.ToolTipText = "Select Commission Type";
            ultraGridColumn16.Header.VisiblePosition = 2;
            ultraGridColumn16.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            ultraGridColumn16.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn16.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridColumn16.Width = 120;
            ultraGridColumn17.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn17.DataType = typeof(double);
            ultraGridColumn17.Format = "##,###.0000";
            ultraGridColumn17.Header.Caption = "Commission Rate";
            ultraGridColumn17.Header.ToolTipText = "Enter Commission Rate";
            ultraGridColumn17.Header.VisiblePosition = 3;
            ultraGridColumn17.InvalidValueBehavior = Infragistics.Win.UltraWinGrid.InvalidValueBehavior.RevertValue;
            ultraGridColumn17.MaxLength = 10;
            ultraGridColumn17.MinValue = ((short)(0));
            ultraGridColumn17.NullText = "0";
            ultraGridColumn17.PromptChar = ' ';
            ultraGridColumn17.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn17.Width = 90;
            ultraGridColumn18.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn18.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn18.Header.Caption = "";
            ultraGridColumn18.Header.VisiblePosition = 4;
            ultraGridColumn18.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn19.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn19.Header.Caption = "Delete";
            ultraGridColumn19.Header.ToolTipText = "Delete";
            ultraGridColumn19.Header.VisiblePosition = 5;
            ultraGridColumn19.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn19.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridColumn19.Width = 65;
            ultraGridColumn20.Header.VisiblePosition = 6;
            ultraGridColumn20.Hidden = true;
            ultraGridColumn21.Header.VisiblePosition = 7;
            ultraGridColumn21.Hidden = true;
            ultraGridBand7.Columns.AddRange(new object[] {
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn16,
            ultraGridColumn17,
            ultraGridColumn18,
            ultraGridColumn19,
            ultraGridColumn20,
            ultraGridColumn21});
            ultraGridBand7.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            ultraGridBand7.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            this.grdCommissionRules.DisplayLayout.BandsSerializer.Add(ultraGridBand7);
            this.grdCommissionRules.DisplayLayout.GroupByBox.Hidden = true;
            this.grdCommissionRules.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCommissionRules.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdCommissionRules.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCommissionRules.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdCommissionRules.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCommissionRules.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCommissionRules.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCommissionRules.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCommissionRules.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCommissionRules.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCommissionRules.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdCommissionRules.Location = new System.Drawing.Point(5, 80);
            this.grdCommissionRules.Name = "grdCommissionRules";
            this.grdCommissionRules.Size = new System.Drawing.Size(598, 167);
            this.grdCommissionRules.TabIndex = 4;
            this.grdCommissionRules.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdCommissionRules_BeforeCellUpdate);
            this.grdCommissionRules.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdCommissionRules_Error);
            this.grdCommissionRules.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCommissionRules_InitializeLayout);
            this.grdCommissionRules.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCommissionRules_ClickCellButton);
            this.grdCommissionRules.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCommissionRules_CellChange);
            this.grdCommissionRules.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCommissionRules_AfterCellUpdate);
            // 
            // nudCommissionRate4
            // 
            this.nudCommissionRate4.BackColor = System.Drawing.Color.White;
            this.nudCommissionRate4.DecimalPlaces = 4;
            this.nudCommissionRate4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudCommissionRate4.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudCommissionRate4.Location = new System.Drawing.Point(135, 253);
            this.nudCommissionRate4.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudCommissionRate4.Name = "nudCommissionRate4";
            this.nudCommissionRate4.Size = new System.Drawing.Size(95, 21);
            this.nudCommissionRate4.TabIndex = 23;
            // 
            // cmbCriteria
            // 
            this.cmbCriteria.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbCriteria.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand8.ColHeadersVisible = false;
            ultraGridColumn22.Header.VisiblePosition = 0;
            ultraGridColumn22.Hidden = true;
            ultraGridColumn23.Header.VisiblePosition = 1;
            ultraGridBand8.Columns.AddRange(new object[] {
            ultraGridColumn22,
            ultraGridColumn23});
            this.cmbCriteria.DisplayLayout.BandsSerializer.Add(ultraGridBand8);
            this.cmbCriteria.DisplayMember = "";
            this.cmbCriteria.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCriteria.DropDownWidth = 0;
            this.cmbCriteria.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCriteria.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCriteria.Location = new System.Drawing.Point(62, 47);
            this.cmbCriteria.Name = "cmbCriteria";
            this.cmbCriteria.Size = new System.Drawing.Size(104, 21);
            this.cmbCriteria.TabIndex = 10;
            this.cmbCriteria.ValueMember = "";
            // 
            // lblCommissionRate1
            // 
            this.lblCommissionRate1.AutoSize = true;
            this.lblCommissionRate1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCommissionRate1.Location = new System.Drawing.Point(9, 255);
            this.lblCommissionRate1.Name = "lblCommissionRate1";
            this.lblCommissionRate1.Size = new System.Drawing.Size(108, 13);
            this.lblCommissionRate1.TabIndex = 10;
            this.lblCommissionRate1.Text = "Minimum Commission ";
            // 
            // lblCriteria
            // 
            this.lblCriteria.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCriteria.Location = new System.Drawing.Point(13, 49);
            this.lblCriteria.Name = "lblCriteria";
            this.lblCriteria.Size = new System.Drawing.Size(42, 14);
            this.lblCriteria.TabIndex = 3;
            this.lblCriteria.Text = "Criteria";
            // 
            // chkACofRule
            // 
            this.chkACofRule.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkACofRule.Location = new System.Drawing.Point(12, 20);
            this.chkACofRule.Name = "chkACofRule";
            this.chkACofRule.Size = new System.Drawing.Size(185, 20);
            this.chkACofRule.TabIndex = 9;
            this.chkACofRule.Text = "Apply Criteria using Rule";
            this.chkACofRule.CheckedChanged += new System.EventHandler(this.chkACofRule_CheckedChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkClearFee);
            this.groupBox1.Controls.Add(this.lbldisplayClear);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.nudCommissionRateClear);
            this.groupBox1.Controls.Add(this.cmbCalculationClear);
            this.groupBox1.Controls.Add(this.cmbCurrencyUsedClear);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(4, 470);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(609, 66);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Clearing Fee";
            // 
            // chkClearFee
            // 
            this.chkClearFee.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkClearFee.Location = new System.Drawing.Point(12, 35);
            this.chkClearFee.Name = "chkClearFee";
            this.chkClearFee.Size = new System.Drawing.Size(61, 20);
            this.chkClearFee.TabIndex = 39;
            this.chkClearFee.Text = "Apply";
            this.chkClearFee.CheckedChanged += new System.EventHandler(this.chkClearFee_CheckedChanged);
            // 
            // lbldisplayClear
            // 
            this.lbldisplayClear.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbldisplayClear.Location = new System.Drawing.Point(420, 37);
            this.lbldisplayClear.Name = "lbldisplayClear";
            this.lbldisplayClear.Size = new System.Drawing.Size(183, 16);
            this.lbldisplayClear.TabIndex = 38;
            this.lbldisplayClear.Text = "lblDisplayCLear";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(274, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "*";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(141, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 35;
            this.label10.Text = "*";
            // 
            // nudCommissionRateClear
            // 
            this.nudCommissionRateClear.BackColor = System.Drawing.Color.White;
            this.nudCommissionRateClear.DecimalPlaces = 4;
            this.nudCommissionRateClear.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudCommissionRateClear.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudCommissionRateClear.Location = new System.Drawing.Point(314, 35);
            this.nudCommissionRateClear.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudCommissionRateClear.Name = "nudCommissionRateClear";
            this.nudCommissionRateClear.Size = new System.Drawing.Size(100, 21);
            this.nudCommissionRateClear.TabIndex = 8;
            // 
            // cmbCalculationClear
            // 
            this.cmbCalculationClear.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance73.BackColor = System.Drawing.SystemColors.Window;
            appearance73.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCalculationClear.DisplayLayout.Appearance = appearance73;
            this.cmbCalculationClear.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand9.ColHeadersVisible = false;
            ultraGridColumn24.Header.VisiblePosition = 0;
            ultraGridColumn24.Hidden = true;
            ultraGridColumn25.Header.VisiblePosition = 1;
            ultraGridBand9.Columns.AddRange(new object[] {
            ultraGridColumn24,
            ultraGridColumn25});
            this.cmbCalculationClear.DisplayLayout.BandsSerializer.Add(ultraGridBand9);
            this.cmbCalculationClear.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCalculationClear.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance74.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance74.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance74.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance74.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCalculationClear.DisplayLayout.GroupByBox.Appearance = appearance74;
            appearance75.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCalculationClear.DisplayLayout.GroupByBox.BandLabelAppearance = appearance75;
            this.cmbCalculationClear.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance76.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance76.BackColor2 = System.Drawing.SystemColors.Control;
            appearance76.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance76.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCalculationClear.DisplayLayout.GroupByBox.PromptAppearance = appearance76;
            this.cmbCalculationClear.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCalculationClear.DisplayLayout.MaxRowScrollRegions = 1;
            appearance77.BackColor = System.Drawing.SystemColors.Window;
            appearance77.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCalculationClear.DisplayLayout.Override.ActiveCellAppearance = appearance77;
            appearance78.BackColor = System.Drawing.SystemColors.Highlight;
            appearance78.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCalculationClear.DisplayLayout.Override.ActiveRowAppearance = appearance78;
            this.cmbCalculationClear.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCalculationClear.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance79.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCalculationClear.DisplayLayout.Override.CardAreaAppearance = appearance79;
            appearance80.BorderColor = System.Drawing.Color.Silver;
            appearance80.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCalculationClear.DisplayLayout.Override.CellAppearance = appearance80;
            this.cmbCalculationClear.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCalculationClear.DisplayLayout.Override.CellPadding = 0;
            appearance81.BackColor = System.Drawing.SystemColors.Control;
            appearance81.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance81.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance81.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance81.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCalculationClear.DisplayLayout.Override.GroupByRowAppearance = appearance81;
            appearance82.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCalculationClear.DisplayLayout.Override.HeaderAppearance = appearance82;
            this.cmbCalculationClear.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCalculationClear.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance83.BackColor = System.Drawing.SystemColors.Window;
            appearance83.BorderColor = System.Drawing.Color.Silver;
            this.cmbCalculationClear.DisplayLayout.Override.RowAppearance = appearance83;
            this.cmbCalculationClear.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance84.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCalculationClear.DisplayLayout.Override.TemplateAddRowAppearance = appearance84;
            this.cmbCalculationClear.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCalculationClear.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCalculationClear.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCalculationClear.DisplayMember = "";
            this.cmbCalculationClear.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCalculationClear.DropDownWidth = 0;
            this.cmbCalculationClear.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCalculationClear.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCalculationClear.Location = new System.Drawing.Point(81, 35);
            this.cmbCalculationClear.Name = "cmbCalculationClear";
            this.cmbCalculationClear.Size = new System.Drawing.Size(102, 21);
            this.cmbCalculationClear.TabIndex = 5;
            this.cmbCalculationClear.ValueMember = "";
            this.cmbCalculationClear.LostFocus += new System.EventHandler(this.cmbCalculationClear_LostFocus);
            this.cmbCalculationClear.ValueChanged += new System.EventHandler(this.cmbCalculationClear_ValueChanged);
            this.cmbCalculationClear.GotFocus += new System.EventHandler(this.cmbCalculationClear_GotFocus);
            // 
            // cmbCurrencyUsedClear
            // 
            this.cmbCurrencyUsedClear.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance85.BackColor = System.Drawing.SystemColors.Window;
            appearance85.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCurrencyUsedClear.DisplayLayout.Appearance = appearance85;
            this.cmbCurrencyUsedClear.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand10.ColHeadersVisible = false;
            ultraGridColumn26.Header.VisiblePosition = 0;
            ultraGridColumn26.Hidden = true;
            ultraGridColumn27.Header.VisiblePosition = 1;
            ultraGridColumn27.Hidden = true;
            ultraGridColumn28.Header.VisiblePosition = 2;
            ultraGridBand10.Columns.AddRange(new object[] {
            ultraGridColumn26,
            ultraGridColumn27,
            ultraGridColumn28});
            this.cmbCurrencyUsedClear.DisplayLayout.BandsSerializer.Add(ultraGridBand10);
            this.cmbCurrencyUsedClear.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCurrencyUsedClear.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance86.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance86.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance86.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance86.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCurrencyUsedClear.DisplayLayout.GroupByBox.Appearance = appearance86;
            appearance87.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCurrencyUsedClear.DisplayLayout.GroupByBox.BandLabelAppearance = appearance87;
            this.cmbCurrencyUsedClear.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance88.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance88.BackColor2 = System.Drawing.SystemColors.Control;
            appearance88.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance88.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCurrencyUsedClear.DisplayLayout.GroupByBox.PromptAppearance = appearance88;
            this.cmbCurrencyUsedClear.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCurrencyUsedClear.DisplayLayout.MaxRowScrollRegions = 1;
            appearance89.BackColor = System.Drawing.SystemColors.Window;
            appearance89.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.ActiveCellAppearance = appearance89;
            appearance90.BackColor = System.Drawing.SystemColors.Highlight;
            appearance90.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.ActiveRowAppearance = appearance90;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance91.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.CardAreaAppearance = appearance91;
            appearance92.BorderColor = System.Drawing.Color.Silver;
            appearance92.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.CellAppearance = appearance92;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.CellPadding = 0;
            appearance93.BackColor = System.Drawing.SystemColors.Control;
            appearance93.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance93.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance93.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance93.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.GroupByRowAppearance = appearance93;
            appearance94.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.HeaderAppearance = appearance94;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance95.BackColor = System.Drawing.SystemColors.Window;
            appearance95.BorderColor = System.Drawing.Color.Silver;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.RowAppearance = appearance95;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance96.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCurrencyUsedClear.DisplayLayout.Override.TemplateAddRowAppearance = appearance96;
            this.cmbCurrencyUsedClear.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCurrencyUsedClear.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCurrencyUsedClear.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCurrencyUsedClear.DisplayMember = "";
            this.cmbCurrencyUsedClear.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCurrencyUsedClear.DropDownWidth = 0;
            this.cmbCurrencyUsedClear.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCurrencyUsedClear.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCurrencyUsedClear.Location = new System.Drawing.Point(194, 35);
            this.cmbCurrencyUsedClear.Name = "cmbCurrencyUsedClear";
            this.cmbCurrencyUsedClear.Size = new System.Drawing.Size(108, 21);
            this.cmbCurrencyUsedClear.TabIndex = 6;
            this.cmbCurrencyUsedClear.ValueMember = "";
            this.cmbCurrencyUsedClear.LostFocus += new System.EventHandler(this.cmbCurrencyUsedClear_LostFocus);
            this.cmbCurrencyUsedClear.GotFocus += new System.EventHandler(this.cmbCurrencyUsedClear_GotFocus);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label12.Location = new System.Drawing.Point(194, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Currency Used";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label13.Location = new System.Drawing.Point(81, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Calculation";
            // 
            // CommissionRule
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpCriteria);
            this.Controls.Add(this.grpParameters);
            this.Controls.Add(this.grpRule);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CommissionRule";
            this.Size = new System.Drawing.Size(616, 537);
            this.Load += new System.EventHandler(this.CommissionRule_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbAUEC)).EndInit();
            this.grpRule.ResumeLayout(false);
            this.grpRule.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.drpDownCommisionRateType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbApplyRuleto)).EndInit();
            this.grpParameters.ResumeLayout(false);
            this.grpParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommissionRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCalculation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrencyUsed)).EndInit();
            this.grpCriteria.ResumeLayout(false);
            this.grpCriteria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCommissionRules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCriteria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRateClear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCalculationClear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrencyUsedClear)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus	
        private void cmbApplyRuleto_GotFocus(object sender, System.EventArgs e)
        {
            cmbApplyRuleto.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbApplyRuleto_LostFocus(object sender, System.EventArgs e)
        {
            cmbApplyRuleto.Appearance.BackColor = Color.White;
        }
        private void cmbCurrencyUsed_GotFocus(object sender, System.EventArgs e)
        {
            cmbCurrencyUsed.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbCurrencyUsed_LostFocus(object sender, System.EventArgs e)
        {
            cmbCurrencyUsed.Appearance.BackColor = Color.White;
        }
        private void cmbCalculation_GotFocus(object sender, System.EventArgs e)
        {
            cmbCalculation.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbCalculation_LostFocus(object sender, System.EventArgs e)
        {
            cmbCalculation.Appearance.BackColor = Color.White;
        }
        private void cmbCommissionRate_GotFocus(object sender, System.EventArgs e)
        {
            cmbCommissionRate.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbCommissionRate_LostFocus(object sender, System.EventArgs e)
        {
            cmbCommissionRate.Appearance.BackColor = Color.White;
        }

        private void txtNamefRule_GotFocus(object sender, System.EventArgs e)
        {
            txtNamefRule.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void txtNamefRule_LostFocus(object sender, System.EventArgs e)
        {
            txtNamefRule.BackColor = Color.White;
        }
        private void txtDescription_GotFocus(object sender, System.EventArgs e)
        {
            txtDescription.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void txtDescription_LostFocus(object sender, System.EventArgs e)
        {
            txtDescription.BackColor = Color.White;
        }

        private void chkACofRule_GotFocus(object sender, System.EventArgs e)
        {
            chkACofRule.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void chkACofRule_LostFocus(object sender, System.EventArgs e)
        {
            chkACofRule.BackColor = Color.White;
        }
        private void cmbAUEC_GotFocus(object sender, System.EventArgs e)
        {
            cmbAUEC.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbAUEC_LostFocus(object sender, System.EventArgs e)
        {
            cmbAUEC.Appearance.BackColor = Color.White;
        }
        private void cmbCalculationClear_GotFocus(object sender, System.EventArgs e)
        {
            cmbCalculationClear.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbCalculationClear_LostFocus(object sender, System.EventArgs e)
        {
            cmbCalculationClear.Appearance.BackColor = Color.White;
        }

        private void cmbCurrencyUsedClear_GotFocus(object sender, System.EventArgs e)
        {
            cmbCurrencyUsedClear.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbCurrencyUsedClear_LostFocus(object sender, System.EventArgs e)
        {
            cmbCurrencyUsedClear.Appearance.BackColor = Color.White;
        }



        #endregion


        private void ultraCombo1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void CommissionRule_Load(object sender, System.EventArgs e)
        {
            try
            {
                BindCommissionRate();
                BindCalculation();
                BindApplyRuleTo();
                BindCurrencyUsed();
                BindCriteria();
                BindALLAUEC();
                BindDetails();
                BindGrid();
                BindCalculationClearCombo();
                BindCurrencyUsedClearCombo();
            }

            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("ExchangeForm_Load",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "CommissionRule_Load", null);
                #endregion
            }
        }


        #region BindApplyRuleTo
        private void BindApplyRuleTo()
        {
            Rules rules = CommissionRuleManager.GetRules();
            rules = CommissionRuleManager.GetRules();
            rules.Insert(0, new Prana.Admin.BLL.Rule(int.MinValue, C_COMBO_SELECT));

            cmbApplyRuleto.DataSource = null;
            cmbApplyRuleto.DataSource = rules;
            cmbApplyRuleto.DisplayMember = "TradeType";
            cmbApplyRuleto.ValueMember = "ApplyRuleID";
            cmbApplyRuleto.Value = int.MinValue;
        }
        #endregion

        #region BindCalculation
        private void BindCalculation()
        {
            CommissionCalculations commissioncalculations = CommissionRuleManager.GetCalculation();
            commissioncalculations = CommissionRuleManager.GetCalculation();
            commissioncalculations.Insert(0, new Prana.Admin.BLL.CommissionCalculation(int.MinValue, C_COMBO_SELECT));
            cmbCalculation.DataSource = null;
            cmbCalculation.DataSource = commissioncalculations;
            cmbCalculation.DisplayMember = "CalculationType";
            cmbCalculation.ValueMember = "CommissionCalculationID";
            cmbCalculation.Value = int.MinValue;
        }
        #endregion

        #region BindCalculationClear Combo
        private void BindCalculationClearCombo()
        {
            CommissionCalculations commissioncalculations = CommissionRuleManager.GetCalculation();
            commissioncalculations = CommissionRuleManager.GetCalculation();
            commissioncalculations.Insert(0, new Prana.Admin.BLL.CommissionCalculation(int.MinValue, C_COMBO_SELECT));
            cmbCalculationClear.DataSource = null;
            cmbCalculationClear.DataSource = commissioncalculations;
            cmbCalculationClear.DisplayMember = "CalculationType";
            cmbCalculationClear.ValueMember = "CommissionCalculationID";
            cmbCalculationClear.Value = int.MinValue;
        }
        #endregion

        #region BindCurrencyUsed Fee Clear Combo
        private void BindCurrencyUsedClearCombo()
        {
            Currencies currencies = new Currencies();
            currencies = AUECManager.GetCurrencies();
            currencies.Insert(0, new Prana.Admin.BLL.Currency(int.MinValue, C_COMBO_SELECT, C_COMBO_SELECT));
            cmbCurrencyUsedClear.DataSource = null;
            cmbCurrencyUsedClear.DataSource = currencies;
            cmbCurrencyUsedClear.DisplayMember = "CurrencySymbol";
            cmbCurrencyUsedClear.ValueMember = "CurencyID";
            cmbCurrencyUsedClear.Value = int.MinValue;

            ColumnsCollection columns3 = cmbCurrencyUsedClear.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns3)
            {
                if (column.Key != "CurrencySymbol")
                {
                    column.Hidden = true;
                }
            }
        }
        #endregion

        #region BindCommissionRate
        private void BindCommissionRate()
        {
            CommissionRateTypes commissionratetypes = new CommissionRateTypes();
            commissionratetypes = CommissionRuleManager.GetCommisionRate();
            commissionratetypes.Insert(0, new Prana.Admin.BLL.CommissionRateType(int.MinValue, C_COMBO_SELECT));
            cmbCommissionRate.DataSource = null;
            cmbCommissionRate.DataSource = commissionratetypes;
            cmbCommissionRate.DisplayMember = "CommissionRateTypeName";
            cmbCommissionRate.ValueMember = "CommissionRateID";
            cmbCommissionRate.Value = int.MinValue;
        }
        #endregion

        #region BindCurrencyUsed
        private void BindCurrencyUsed()
        {
            Currencies currencies = new Currencies();
            currencies = AUECManager.GetCurrencies();
            currencies.Insert(0, new Prana.Admin.BLL.Currency(int.MinValue, C_COMBO_SELECT, C_COMBO_SELECT));
            cmbCurrencyUsed.DataSource = null;
            cmbCurrencyUsed.DataSource = currencies;
            cmbCurrencyUsed.DisplayMember = "CurrencySymbol";
            cmbCurrencyUsed.ValueMember = "CurencyID";
            cmbCurrencyUsed.Value = int.MinValue;

            ColumnsCollection columns3 = cmbCurrencyUsed.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns3)
            {
                if (column.Key != "CurrencySymbol")
                {
                    column.Hidden = true;
                }
            }


            //
            //			cmbCurrency.DataSource=currencies;
            //			cmbCurrency.DisplayMember="CurrencySymbol";
            //			cmbCurrency.ValueMember="CurencyID";
            //			cmbCurrency.Value=int.MinValue;
            //			ColumnsCollection columns4 = cmbCurrency.DisplayLayout.Bands[0].Columns;
            //			foreach (UltraGridColumn column in columns4)
            //			{
            //				if(column.Key != "CurrencySymbol")
            //				{
            //					column.Hidden = true;
            //				}
            //			}

        }
        #endregion



        #region BindCriteria

        private void BindCriteria()
        {
            CommissionCalculations commissioncalculations = CommissionRuleManager.GetCalculation();
            commissioncalculations = CommissionRuleManager.GetCalculation();
            commissioncalculations.Insert(0, new Prana.Admin.BLL.CommissionCalculation(int.MinValue, C_COMBO_SELECT));
            cmbCriteria.DataSource = null;
            cmbCriteria.DataSource = commissioncalculations;
            cmbCriteria.DisplayMember = "CalculationType";
            cmbCriteria.ValueMember = "CommissionCalculationID";
            cmbCriteria.Value = int.MinValue;
            ColumnsCollection columns10 = cmbCriteria.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns10)
            {
                if (column.Key != "CalculationType")
                {
                    column.Hidden = true;
                }
            }

        }
        #endregion

        #region BindALLAUEC
        private void BindALLAUEC()
        {
            AUECs auecs = AUECManager.GetAUEC();

            System.Data.DataTable dtauec = new System.Data.DataTable();
            dtauec.Columns.Add("Data");
            dtauec.Columns.Add("Value");
            object[] row = new object[2];
            row[0] = C_COMBO_SELECT;
            row[1] = int.MinValue;
            dtauec.Rows.Add(row);

            if (auecs.Count > 0)
            {
                foreach (AUEC auec in auecs)
                {
                    //SK 2061009 removed Compliance class
                    //Currency currency = new Currency();
                    //currency = AUECManager.GetCurrency(auec.Compliance.BaseCurrencyID);
                    //
                    //string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.Exchange.DisplayName.ToString() + "/" + auec.Currency.CurrencySymbol.ToString();
                    string Data = auec.AUECString;
                    int Value = auec.AUECID;

                    row[0] = Data;
                    row[1] = Value;
                    dtauec.Rows.Add(row);
                }
                cmbAUEC.DataSource = null;
                cmbAUEC.DataSource = dtauec;
                //auecs.Insert(0, new AUEC(int.MinValue, C_COMBO_SELECT));
                cmbAUEC.DisplayMember = "Data";
                cmbAUEC.ValueMember = "Value";

            }
            ColumnsCollection columns5 = cmbAUEC.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns5)
            {
                if (column.Key != "Data")
                {
                    column.Hidden = true;
                }
            }



        }


        #endregion

        private void BindDetails()
        {
            AUEC auec = AUECManager.GetAUEC(_auecID);

            if (auec != null)
            {
                //				cmbAssetClass.Value = int.Parse(auec.AssetID.ToString());
                //				cmbUnderlying.Value=int.Parse(auec.UnderlyingID.ToString());

                //			Exchange exchange= new Exchange();
                //			exchange=AUECManager.GetAUECExchange(auec.Exchange.AUECExchangeID);
                //
                //			cmbExchange.Value=int.Parse(exchange.AUECExchangeID.ToString());
                //SK 2061009 removed Compliance class			
                //Currency currency = new Currency();
                //currency = AUECManager.GetCurrency(auec.Compliance.BaseCurrencyID);
                //
                cmbCurrencyUsed.Value = int.Parse(auec.Currency.CurencyID.ToString());
            }
        }

        public int _auecID = int.MinValue;
        private void cmbAUEC_ValueChanged(object sender, System.EventArgs e)
        {
            if (cmbAUEC.Value != null)
            {
                _auecID = int.Parse(cmbAUEC.Value.ToString());
                BindDetails();
            }
        }

        AUECCommissionRule auecCommissionRule = new AUECCommissionRule();
        public AUECCommissionRule auecCommissionRuleproperty
        {

            get
            {
                auecCommissionRule = GetCommissionRuleDetails(auecCommissionRule);
                return auecCommissionRule;
            }
            set
            {
                auecCommissionRule = value;
                SetCommissionRuleDetails(value);

            }
        }


        public CommissionCriteria commissionCriteriaproperty
        {
            get
            {
                CommissionCriteria commissionCriteria = new CommissionCriteria();
                commissionCriteria = GetCommissionCriteriaDetails(commissionCriteria);
                return commissionCriteria;
            }
            set { SetCommissionCriteriaDetails(value); }
        }

        #region Commission Clearing Fee

        public Prana.Admin.BLL.CommissionRuleClearingFee commissionRuleClearingFee
        {
            get
            {
                Prana.Admin.BLL.CommissionRuleClearingFee commRuleClrFee = new Prana.Admin.BLL.CommissionRuleClearingFee();
                commRuleClrFee = GetCommissionRuleClreaingFee(commRuleClrFee);
                return commRuleClrFee;
            }
            set { SetCommissionRuleClreaingFee(value); }
        }

        private Prana.Admin.BLL.CommissionRuleClearingFee GetCommissionRuleClreaingFee(Prana.Admin.BLL.CommissionRuleClearingFee commissionClrFee)
        {
            commissionClrFee = null;
            errorProvider1.SetError(cmbCalculationClear, "");
            errorProvider1.SetError(nudCommissionRateClear, "");
            errorProvider1.SetError(cmbCurrencyUsedClear, "");

            if (chkClearFee.Checked == true)
            {
                if (int.Parse(cmbCalculationClear.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbCalculationClear, "Please select Criteria!");
                    cmbCalculationClear.Focus();
                    return commissionClrFee;
                }
                else if (int.Parse(cmbCurrencyUsedClear.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbCurrencyUsedClear, "Please select Value");
                    cmbCurrencyUsedClear.Focus();
                    return commissionClrFee;
                }
                else if (double.Parse(nudCommissionRateClear.Value.ToString()) == 0)
                {
                    errorProvider1.SetError(nudCommissionRateClear, "Please select Value");
                    nudCommissionRateClear.Focus();
                    return commissionClrFee;
                }
                else
                {
                    commissionClrFee = new Prana.Admin.BLL.CommissionRuleClearingFee();
                    commissionClrFee.CalculationId = int.Parse(cmbCalculationClear.Value.ToString());
                    commissionClrFee.CurrencyId = int.Parse(cmbCurrencyUsedClear.Value.ToString());
                    commissionClrFee.CommissionRate = float.Parse(nudCommissionRateClear.Value.ToString());
                }
            }
            return commissionClrFee;
        }


        private void SetCommissionRuleClreaingFee(Prana.Admin.BLL.CommissionRuleClearingFee commissionRuleClrFee)
        {
            if (commissionRuleClrFee != null)
            {
                cmbCalculationClear.Value = int.Parse(commissionRuleClrFee.CalculationId.ToString());
                cmbCurrencyUsedClear.Value = int.Parse(commissionRuleClrFee.CurrencyId.ToString());
                nudCommissionRateClear.Value = decimal.Parse(commissionRuleClrFee.CommissionRate.ToString());
                chkClearFee.Checked = true;
            }
            else
            {
                RefreshClearingFee();
            }
        }

        public void RefreshClearingFee()
        {
            chkClearFee.Checked = false;
            cmbCalculationClear.Text = C_COMBO_SELECT;
            cmbCalculationClear.Enabled = false;
            cmbCurrencyUsedClear.Text = C_COMBO_SELECT;
            cmbCurrencyUsedClear.Enabled = false;
            nudCommissionRateClear.Value = 0;
            nudCommissionRateClear.Enabled = false;

        }


        #endregion Commission Clreaing Fee

        public void setgrid()
        {
            intchk = 1;
            BindGrid();
            cmbCriteria.Text = C_COMBO_SELECT;
        }

        public bool IsCriteriaSelected()
        {
            if (chkACofRule.Checked == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool IsClearingFeeSelected()
        {
            if (chkClearFee.Checked == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public CommissionRuleCriteriaold commissionRuleCriteriaproperty
        {
            get
            {
                CommissionRuleCriteriaold commissionRuleCriteria = new CommissionRuleCriteriaold();

                //commissionRuleCriteria = GetCommissionRuleCriteriaDetails( commissionRuleCriteria);
                return commissionRuleCriteria;
            }
            set
            {
                SetCommissionRuleCriteriaDetails(value);

            }
        }


        public void RefreshCommissionRuleDetails()
        {
            cmbAUEC.Text = C_COMBO_SELECT;
            txtNamefRule.Text = "";
            cmbApplyRuleto.Text = C_COMBO_SELECT;
            txtDescription.Text = "";
            cmbCalculation.Text = C_COMBO_SELECT;
            cmbCurrencyUsed.Text = C_COMBO_SELECT;
            cmbCommissionRate.Text = C_COMBO_SELECT;
            cmbCommissionRate.Value = 1;
            nudCommissionRate.Value = 0;
        }


        private void SetCommissionRuleDetails(AUECCommissionRule auecCommissionRule)
        {
            cmbAUEC.Value = int.Parse(auecCommissionRule.AUECID.ToString());
            txtNamefRule.Text = auecCommissionRule.RuleName;
            cmbApplyRuleto.Value = int.Parse(auecCommissionRule.ApplyRuleID.ToString());
            txtDescription.Text = auecCommissionRule.RuleDescription;
            cmbCalculation.Value = int.Parse(auecCommissionRule.CalculationID.ToString());
            cmbCurrencyUsed.Value = int.Parse(auecCommissionRule.CurrencyID.ToString());
            cmbCommissionRate.Value = int.Parse(auecCommissionRule.CommissionRateID.ToString());
            //nudCommissionRate.Value=int.Parse(auecCommissionRule.Commission.ToString());
            nudCommissionRate.Value = Decimal.Parse(auecCommissionRule.Commission.ToString());
            chkACofRule.Checked = (int.Parse(auecCommissionRule.ApplyCriteria.ToString()) == 1 ? true : false);
            chkClearFee.Checked = (int.Parse(auecCommissionRule.ApplyClearingFee.ToString()) == 1 ? true : false);
            if (chkACofRule.Checked == false)
            {
                RefreshCommissionRuleCriteriaDetails();
                ResetCrieteriaControls();
            }
            if (!chkClearFee.Checked)
            {
                RefreshClearingFee();
            }
        }

        private AUECCommissionRule GetCommissionRuleDetails(AUECCommissionRule aueccommissionRule)
        {
            aueccommissionRule = null;

            errorProvider1.SetError(cmbAUEC, "");
            errorProvider1.SetError(txtNamefRule, "");
            errorProvider1.SetError(cmbApplyRuleto, "");
            errorProvider1.SetError(txtDescription, "");
            errorProvider1.SetError(cmbCalculation, "");
            errorProvider1.SetError(cmbCurrencyUsed, "");
            //errorProvider1.SetError(cmbCommissionRate, "");
            errorProvider1.SetError(nudCommissionRate, "");


            if (int.Parse(cmbAUEC.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbAUEC, "Please select AUEC!");
                cmbAUEC.Focus();
                return aueccommissionRule;
            }
            else if (txtNamefRule.Text.Trim() == "")
            {
                errorProvider1.SetError(txtNamefRule, "Please enter Name of Rule !");
                txtNamefRule.Focus();
                return aueccommissionRule;
            }
            else if (int.Parse(cmbApplyRuleto.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbApplyRuleto, "Please select Rule applied");
                cmbApplyRuleto.Focus();
                return aueccommissionRule;
            }
            else if (txtDescription.Text.Trim() == " ")
            {
                errorProvider1.SetError(txtDescription, "Please enter Description");
                txtDescription.Focus();
                return aueccommissionRule;
            }
            else if (int.Parse(cmbCalculation.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbCalculation, "Please select Value");
                cmbCalculation.Focus();
                return aueccommissionRule;
            }
            else if (int.Parse(cmbCurrencyUsed.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbCurrencyUsed, "Please enter Currency");
                cmbCurrencyUsed.Focus();
                return aueccommissionRule;
            }
            //else if(int.Parse(cmbCommissionRate.Value.ToString())==int.MinValue)
            //{
            //    errorProvider1.SetError(cmbCommissionRate,"Please enter Value of CommissionRate");
            //    cmbCommissionRate.Focus();
            //    return aueccommissionRule;
            //}
            else if (double.Parse(nudCommissionRate.Value.ToString()) == double.MinValue)
            {
                errorProvider1.SetError(nudCommissionRate, "Enter Value");
                nudCommissionRate.Focus();
                return aueccommissionRule;
            }

            else
            {
                aueccommissionRule = new AUECCommissionRule();
                aueccommissionRule.AUECID = int.Parse(cmbAUEC.Value.ToString());
                aueccommissionRule.RuleName = txtNamefRule.Text.Trim().ToString();
                aueccommissionRule.ApplyRuleID = int.Parse(cmbApplyRuleto.Value.ToString());
                aueccommissionRule.RuleDescription = txtDescription.Text.Trim().ToString();
                aueccommissionRule.CalculationID = int.Parse(cmbCalculation.Value.ToString());
                aueccommissionRule.CurrencyID = int.Parse(cmbCurrencyUsed.Value.ToString());
                aueccommissionRule.CommissionRateID = int.Parse(cmbCommissionRate.Value.ToString());
                aueccommissionRule.Commission = float.Parse(nudCommissionRate.Value.ToString());
                //auecCommissionRule.ApplyCriteria = auecCommissionRule.ApplyCriteria+(chkACofRule.Checked.Equals(true)?1:0);
                aueccommissionRule.ApplyCriteria = (chkACofRule.Checked == true ? 1 : 0);
                aueccommissionRule.ApplyClearingFee = (chkClearFee.Checked == true ? 1 : 0);
            }
            return aueccommissionRule;

        }

        public void RefreshCommissionCriteriaDetails()
        {

            cmbCriteria.Text = C_COMBO_SELECT;
            cmbCalculation.Text = C_COMBO_SELECT;

        }

        private void SetCommissionCriteriaDetails(CommissionCriteria commissionCriteria)
        {
            if (commissionCriteria != null)
            {
                cmbCriteria.Value = int.Parse(commissionCriteria.CommissionCalculationID_FK.ToString());
                nudCommissionRate4.Value = decimal.Parse(commissionCriteria.MinimumCommissionRate.ToString());
            }
            else
            {
                RefreshCommissionCriteriaDetails();
                RefreshCommissionRuleCriteriaDetails();
            }
        }


        private CommissionCriteria GetCommissionCriteriaDetails(CommissionCriteria commissioncriteria)
        {
            commissioncriteria = null;
            errorProvider1.SetError(cmbCriteria, "");
            errorProvider1.SetError(nudCommissionRate, "");
            errorProvider1.SetError(grdCommissionRules, "");

            if (chkACofRule.Checked == true)
            {
                if (int.Parse(cmbCriteria.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbCriteria, "Please select Criteria!");
                    cmbCriteria.Focus();
                    return commissioncriteria;
                }
                else if (double.Parse(nudCommissionRate.Value.ToString()) == double.MinValue)
                {
                    errorProvider1.SetError(nudCommissionRate, "Please select Value");
                    nudCommissionRate.Focus();
                    return commissioncriteria;
                }

                else
                {
                    commissioncriteria = new CommissionCriteria();
                    commissioncriteria.CommissionCalculationID_FK = int.Parse(cmbCriteria.Value.ToString());
                    commissioncriteria.MinimumCommissionRate = float.Parse(nudCommissionRate4.Value.ToString());
                }
            }
            return commissioncriteria;
        }

        public void RefreshCommissionRuleCriteriaDetails()
        {
            chkACofRule.Checked = false;
            cmbCriteria.Enabled = false;
            cmbCriteria.Text = C_COMBO_SELECT;
            nudCommissionRate4.Value = 0;
            grdCommissionRules.Enabled = false;

        }

        private void SetCommissionRuleCriteriaDetails(CommissionRuleCriteriaold commissionRuleCriteria)
        {
            if (commissionRuleCriteria != null)
            {

            }
            if (chkACofRule.Checked == false)
            {
                RefreshCommissionRuleCriteriaDetails();
                ResetCrieteriaControls();
            }
        }


        private void ResetCrieteriaControls()
        {
            bool chkACofRuleState = false;
            chkACofRuleState = chkACofRule.Checked;
            if (chkACofRuleState == false)
            {
                cmbCriteria.Enabled = false;

                nudCommissionRate4.Enabled = false;

                RefreshCommissionRuleCriteriaDetails();
            }
            else
            {
                cmbCriteria.Enabled = true;
                nudCommissionRate4.Enabled = true;
            }
        }
        private void chkACofRule_CheckedChanged(object sender, System.EventArgs e)
        {
            bool chkACofRuleState = false;
            chkACofRuleState = chkACofRule.Checked;
            if (chkACofRuleState == false)
            {
                cmbCriteria.Enabled = false;
                nudCommissionRate4.Enabled = false;

                RefreshCommissionRuleCriteriaDetails();

                // Refresh Grid
                intchk = 1;
                BindGrid();
                grdCommissionRules.Enabled = false;

            }
            else
            {
                cmbCriteria.Enabled = true;
                nudCommissionRate4.Enabled = true;

                grdCommissionRules.Enabled = true;
            }
        }

        public string val = string.Empty;
        public bool valChanged = false;

        private void cmbCalculation_ValueChanged(object sender, EventArgs e)
        {
            if (cmbCalculation.Value != null)
            {
                if (cmbCalculation.Text == "Shares")
                {
                    lbldisplay.Text = "Cents Per Share";
                }
                else if (cmbCalculation.Text == "Contracts")
                {
                    lbldisplay.Text = "per contract";
                }
                else if (cmbCalculation.Text == "Notional")
                {
                    lbldisplay.Text = "Basis Points";
                }
                else
                {
                    lbldisplay.Text = "";
                }
            }
        }

        CommissionRuleCriteriasUp _commissionRuleCriteriasUp = new CommissionRuleCriteriasUp();
        public CommissionRuleCriteriasUp commissionRuleCriteriasUpproperties
        {
            get
            {
                _commissionRuleCriteriasUp = (CommissionRuleCriteriasUp)grdCommissionRules.DataSource;

                CommissionRuleCriteriasUp validcommissionrules = new CommissionRuleCriteriasUp();

                if (grdCommissionRules.Rows.Count == 1)
                {
                    Int64 intvalfrom = Int64.Parse(grdCommissionRules.ActiveRow.Cells["ValueFrom"].Value.ToString());
                    Int64 intvalto = Int64.Parse(grdCommissionRules.ActiveRow.Cells["ValueTo"].Value.ToString());
                    int intcommrateID = int.Parse(grdCommissionRules.ActiveRow.Cells["CommissionRateID_FK"].Value.ToString());
                    double dblcommrate = double.Parse(grdCommissionRules.ActiveRow.Cells["CommisionRate"].Value.ToString());
                    if (intvalfrom == 0 && intvalto == 0 && intcommrateID == int.MinValue && dblcommrate == 0)
                    {
                        MessageBox.Show("Please enter the values for, atleast one Commission Rule ", "Nirvane Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        validcommissionrules = null;
                        grdCommissionRules.Focus();
                        return validcommissionrules;
                    }

                }
                int index1 = 1;
                // Validation to check whether any field in not blank              
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCommissionRules.Rows)
                {

                    Int64 valuefrom = Int64.Parse(dr.Cells["ValueFrom"].Value.ToString());
                    Int64 valueto = Int64.Parse(dr.Cells["ValueTo"].Value.ToString());
                    int commissionrateID = int.Parse(dr.Cells["CommissionRateID_FK"].Value.ToString());
                    double commissionrate = double.Parse(dr.Cells["CommisionRate"].Value.ToString());

                    if (valuefrom > 0 || valueto > 0 || commissionrateID != int.MinValue)
                    //if (valuefrom > 0 || valueto > 0 || commissionrateID != int.MinValue || commissionrate > 0)
                    {
                        if (valuefrom <= 0)
                        {
                            MessageBox.Show("Please enter the Range ValueFrom(>=) in the row " + index1, "Nirvane Alert");
                            validcommissionrules = null;
                            grdCommissionRules.Focus();
                            return validcommissionrules;
                        }
                        if (valueto <= 0)
                        {
                            MessageBox.Show("Please enter the Range ValueTo(<=) in the row " + index1, "Nirvane Alert");
                            validcommissionrules = null;
                            grdCommissionRules.Focus();
                            return validcommissionrules;
                        }
                        if (commissionrateID == int.MinValue)
                        {
                            MessageBox.Show("Please select the Commission on type in the row " + index1, "Nirvane Alert");
                            validcommissionrules = null;
                            grdCommissionRules.Focus();
                            return validcommissionrules;
                        }
                        if (commissionrate <= 0)
                        {
                            MessageBox.Show("Please enter the Commission Rate in the row " + index1, "Nirvane Alert");
                            validcommissionrules = null;
                            grdCommissionRules.Focus();
                            return validcommissionrules;
                        }
                        index1 = index1 + 1;
                    }
                }

                // Validation to check whether To Value is greater than From Value
                int index2 = 1;
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCommissionRules.Rows)
                {

                    Int64 intvaluefrom = Int64.Parse(dr.Cells["ValueFrom"].Value.ToString());
                    Int64 intvalueto = Int64.Parse(dr.Cells["ValueTo"].Value.ToString());

                    if (intvaluefrom > 0 && intvalueto > 0)
                    {
                        if (intvaluefrom > intvalueto)
                        {
                            MessageBox.Show("ValueTo(<=) should be greater than or equal to ValueFrom(>=) in the row  " + index2, "Nirvane Alert");
                            validcommissionrules = null;
                            grdCommissionRules.Focus();
                            return validcommissionrules;
                        }
                    }
                    index2 = index2 + 1;
                }

                //Validation for Case of Overlapping               
                int index3 = 0;
                int intTest = 0;
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCommissionRules.Rows)
                {
                    Int64 valfrm = Int64.Parse(dr.Cells["ValueFrom"].Value.ToString());

                    Int64 valuefrom = Int64.Parse(grdCommissionRules.Rows[index3].Cells["ValueFrom"].Value.ToString());
                    Int64 valueto = Int64.Parse(grdCommissionRules.Rows[index3].Cells["ValueTo"].Value.ToString());
                    int commissionrateID = int.Parse(grdCommissionRules.Rows[index3].Cells["CommissionRateID_FK"].Value.ToString());
                    string commissionratetype = grdCommissionRules.Rows[index3].Cells["CommissionRateID_FK"].Text.ToString();
                    double commissionrate = double.Parse(grdCommissionRules.Rows[index3].Cells["CommisionRate"].Value.ToString());

                    intTest = intTest + 1;
                    for (int localindex = intTest; localindex < grdCommissionRules.Rows.Count - 1; localindex++)
                    {
                        if (valueto > 0)
                        {
                            Int64 localvaluefrom = Int64.Parse(grdCommissionRules.Rows[localindex].Cells["ValueFrom"].Value.ToString());
                            Int64 localvalueto = Int64.Parse(grdCommissionRules.Rows[localindex].Cells["ValueTo"].Value.ToString());
                            int localcommissionrateID = int.Parse(grdCommissionRules.Rows[localindex].Cells["CommissionRateID_FK"].Value.ToString());
                            string localcommissionratetype = grdCommissionRules.Rows[localindex].Cells["CommissionRateID_FK"].Text.ToString();
                            double localcommissionrate = double.Parse(grdCommissionRules.Rows[localindex].Cells["CommisionRate"].Value.ToString());

                            if (valfrm <= localvaluefrom)
                            {

                                if (valueto > 0 && localvaluefrom > 0 && localcommissionrateID != int.MinValue && localcommissionrate > 0)
                                {
                                    if (valueto >= localvaluefrom)
                                    {
                                        if (commissionratetype == "Contracts" && localcommissionratetype == "Contracts")
                                        {
                                            MessageBox.Show("Overlapping ranges in row " + (index3 + 1) + " and " + (localindex + 1), "Nirvane Alert");
                                            validcommissionrules = null;
                                            grdCommissionRules.Focus();
                                            return validcommissionrules;
                                        }
                                        else if (commissionratetype == "Contracts" && localcommissionratetype == "Notional")
                                        {
                                            MessageBox.Show("Overlapping ranges in row " + (index3 + 1) + " and " + (localindex + 1), "Nirvane Alert");
                                            validcommissionrules = null;
                                            grdCommissionRules.Focus();
                                            return validcommissionrules;
                                        }
                                        else if (commissionratetype == "Notional" && localcommissionratetype == "Notional")
                                        {
                                            MessageBox.Show("Overlapping ranges in row " + (index3 + 1) + " and " + (localindex + 1), "Nirvane Alert");
                                            validcommissionrules = null;
                                            grdCommissionRules.Focus();
                                            return validcommissionrules;
                                        }
                                        else if (commissionratetype == "Notional" && localcommissionratetype == "Contracts")
                                        {
                                            MessageBox.Show("Overlapping ranges in row " + (index3 + 1) + " and " + (localindex + 1), "Nirvane Alert");
                                            validcommissionrules = null;
                                            grdCommissionRules.Focus();
                                            return validcommissionrules;
                                        }
                                    }
                                }
                            }

                            else
                            {
                                if (valueto > 0 && localvaluefrom > 0 && localcommissionrateID != int.MinValue && localcommissionrate > 0)
                                {
                                    if (valueto >= localvaluefrom && valfrm < localvalueto)
                                    {
                                        if (commissionratetype == "Contracts" && localcommissionratetype == "Contracts")
                                        {
                                            MessageBox.Show("Overlapping ranges in row " + (index3 + 1) + " and " + (localindex + 1), "Nirvane Alert");
                                            validcommissionrules = null;
                                            grdCommissionRules.Focus();
                                            return validcommissionrules;
                                        }
                                        else if (commissionratetype == "Contracts" && localcommissionratetype == "Notional")
                                        {
                                            MessageBox.Show("Overlapping ranges in row " + (index3 + 1) + " and " + (localindex + 1), "Nirvane Alert");
                                            validcommissionrules = null;
                                            grdCommissionRules.Focus();
                                            return validcommissionrules;
                                        }
                                        else if (commissionratetype == "Notional" && localcommissionratetype == "Notional")
                                        {
                                            MessageBox.Show("Overlapping ranges in row " + (index3 + 1) + " and " + (localindex + 1), "Nirvane Alert");
                                            validcommissionrules = null;
                                            grdCommissionRules.Focus();
                                            return validcommissionrules;
                                        }
                                        else if (commissionratetype == "Notional" && localcommissionratetype == "Contracts")
                                        {
                                            MessageBox.Show("Overlapping ranges in row " + (index3 + 1) + " and " + (localindex + 1), "Nirvane Alert");
                                            validcommissionrules = null;
                                            grdCommissionRules.Focus();
                                            return validcommissionrules;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    index3 = index3 + 1;
                }



                foreach (CommissionRuleCriteriaUp commissionrulecritUp in _commissionRuleCriteriasUp)
                {
                    if (commissionrulecritUp.CommissionRateID_FK != int.MinValue && commissionrulecritUp.ValueFrom > 0 && commissionrulecritUp.ValueTo > 0 && commissionrulecritUp.CommisionRate > 0)
                    {
                        validcommissionrules.Add(commissionrulecritUp);
                    }
                }
                return validcommissionrules;
            }
            set
            {
                _commissionRuleCriteriasUp = value;

                grdCommissionRules.DataSource = _commissionRuleCriteriasUp;
                if (_commissionRuleCriteriasUp.Count > 0)
                {
                    AddNewRow();
                    SetCellChangeValues();
                }
                else
                {
                    AddNewTempRow();
                }
                RefreshGrid();
            }
        }

        private void SetCellChangeValues()
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCommissionRules.Rows)
            {

                if (dr.Cells["CommissionRateID_FK"].Text == "Shares")
                {
                    dr.Cells["TextCaption"].Value = "Cents per Shares";
                }
                else if (dr.Cells["CommissionRateID_FK"].Text == "Contracts")
                {
                    dr.Cells["TextCaption"].Value = "Per Contract";
                }
                else if (dr.Cells["CommissionRateID_FK"].Text == "Notional")
                {
                    dr.Cells["TextCaption"].Value = "Basis Points";
                }
                else if (dr.Cells["CommissionRateID_FK"].Text == "- Select -")
                {
                    dr.Cells["TextCaption"].Value = "";
                }
            }

        }

        int intchk = 0;
        private void grdCommissionRules_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            CommissionRateTypes commissionratetypes = new CommissionRateTypes();
            commissionratetypes = CommissionRuleManager.GetCommisionRate();
            commissionratetypes.Insert(0, new Prana.Admin.BLL.CommissionRateType(int.MinValue, C_COMBO_SELECT));

            drpDownCommisionRateType.DisplayMember = "CommissionRateTypeName";
            drpDownCommisionRateType.ValueMember = "CommissionRateID";
            drpDownCommisionRateType.DataSource = null;
            drpDownCommisionRateType.DataSource = commissionratetypes;

            drpDownCommisionRateType.Text = C_COMBO_SELECT;
            if (intchk != 1)
            {
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRateID_FK"].ValueList = drpDownCommisionRateType;
            }
            ColumnsCollection columnsCF = drpDownCommisionRateType.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsCF)
            {
                if (column.Key != "CommissionRateTypeName")
                {
                    column.Hidden = true;
                }
            }
            drpDownCommisionRateType.DisplayLayout.Bands[0].ColHeadersVisible = false;
            intchk = 0;

        }

        private void BindGrid()
        {
            CommissionRuleCriteriasUp commissioncriteriasup = new CommissionRuleCriteriasUp();
            commissioncriteriasup = CommissionRuleManager.GetCommissionRuleCriteriasUp(0);
            grdCommissionRules.DataSource = commissioncriteriasup;
            AddNewTempRow();
            RefreshGrid();
        }

        private void AddNewTempRow()
        {
            CommissionRuleCriteriasUp commrulecrisup = new CommissionRuleCriteriasUp();

            CommissionRuleCriteriaUp commrulecriup = new CommissionRuleCriteriaUp();
            commrulecriup.CommissionRateID_FK = int.MinValue;
            commrulecriup.CommisionRate = 0;
            commrulecriup.CommissionCriteriaID_FK = int.MinValue;
            commrulecriup.CommissionRuleCriteriaID = int.MinValue;
            commrulecriup.ValueFrom = 0;
            commrulecriup.ValueTo = 0;

            commrulecrisup.Add(commrulecriup);
            grdCommissionRules.DataSource = commrulecrisup;
            //grdCommissionRules.DataBind();

        }
        private void RefreshGrid()
        {
            if (grdCommissionRules.Rows.Count > 0)
            {
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueFrom"].Header.VisiblePosition = 0;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueFrom"].Header.Caption = "Value From(>=)";
                // grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueFrom"].DataType = typeof(Int64);
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueFrom"].MaxLength = 14;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueFrom"].Width = 100;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueFrom"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueFrom"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                //grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueFrom"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Automatic;

                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueTo"].Header.VisiblePosition = 1;
                // grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueTo"].DataType = typeof(Int64);
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueTo"].MaxLength = 14;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueTo"].Header.Caption = "Value To(<=)";
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueTo"].Width = 100;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueTo"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["ValueTo"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRateID_FK"].Header.VisiblePosition = 2;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRateID_FK"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRateID_FK"].Width = 120;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRateID_FK"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRateID_FK"].Header.Caption = "Commission On";
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRateID_FK"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;


                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommisionRate"].Header.VisiblePosition = 3;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommisionRate"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommisionRate"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommisionRate"].Format = "##,###.0000";
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommisionRate"].MaxLength = 10;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommisionRate"].Width = 95;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommisionRate"].Header.Caption = "Commission Rate";
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommisionRate"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommisionRate"].InvalidValueBehavior = InvalidValueBehavior.RevertValue;

                grdCommissionRules.DisplayLayout.Bands[0].Columns["TextCaption"].Header.VisiblePosition = 4;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["TextCaption"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["TextCaption"].Width = 99;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["TextCaption"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;

                grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Header.VisiblePosition = 5;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Width = 63;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;

                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionCriteriaID_FK"].Header.VisiblePosition = 6;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionCriteriaID_FK"].Hidden = true;

                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRuleCriteriaID"].Header.VisiblePosition = 7;
                grdCommissionRules.DisplayLayout.Bands[0].Columns["CommissionRuleCriteriaID"].Hidden = true;

            }
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


            int rowsCount = grdCommissionRules.Rows.Count;
            UltraGridRow dr = grdCommissionRules.Rows[rowsCount - 1];

            CommissionRuleCriteriasUp commrulecrisup = (CommissionRuleCriteriasUp)grdCommissionRules.DataSource;
            CommissionRuleCriteriaUp commrulecriup = new CommissionRuleCriteriaUp();

            //The below varriables are taken from the last row of the grid before adding the new row.
            Int64 intValueFrom = Int64.Parse(dr.Cells["ValueFrom"].Value.ToString());
            Int64 intValueTo = Int64.Parse(dr.Cells["ValueTo"].Value.ToString());
            int CommRateId = int.Parse(dr.Cells["CommissionRateID_FK"].Value.ToString());

            //Add one more condition to avoid adding 2nd new row when the below condition fullfills.
            if (CommRateId != int.MinValue && (intValueFrom > 0 && intValueTo > 0))
            {
                commrulecriup.CommisionRate = 0;
                commrulecriup.CommissionCriteriaID_FK = int.MinValue;
                commrulecriup.CommissionRateID_FK = int.MinValue;
                commrulecriup.CommissionRuleCriteriaID = int.MinValue;
                commrulecriup.ValueFrom = 0;
                commrulecriup.ValueTo = 0;
                commrulecrisup.Add(commrulecriup);
                grdCommissionRules.DataSource = commrulecrisup;
                grdCommissionRules.DataBind();
                grdCommissionRules.ActiveCell = prevActiveCell;
                grdCommissionRules.Focus();
                grdCommissionRules.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                if (len != int.MinValue)
                {
                    //prevActiveCell.SelLength = 0;
                    //prevActiveCell.SelStart = len + 1;
                }
            }
        }

        private void grdCommissionRules_CellChange(object sender, CellEventArgs e)
        {

            try
            {
                //if (e.Cell.Column.Key.ToLower() == "valuefrom")
                //{
                //    if (e.Cell.Value == null)
                //    {
                //    }
                //}

                //string ruleDescription = string.Empty;
                //Int64 oldValueFrom = Int64.Parse(grdCommissionRules.ActiveRow.Cells["ValueFrom"].Value.ToString());
                //Int64 oldValueTo = Int64.Parse(grdCommissionRules.ActiveRow.Cells["ValueTo"].Value.ToString());
                //int oldCommissionRateID = int.Parse(grdCommissionRules.ActiveRow.Cells["CommissionRateID_FK"].Value.ToString());
                //double oldCommissionRate = double.Parse(grdCommissionRules.ActiveRow.Cells["CommisionRate"].Value.ToString());
                //grdCommissionRules.UpdateData();
                // // for Value from
                // bool result = false;
                // Int64 updatedValueFrom = Int64.Parse(grdCommissionRules.ActiveRow.Cells["ValueFrom"].Value.ToString());
                // if (oldValueFrom != updatedValueFrom)
                // {
                //     result = ValueFromTest();
                //     if (result == true)
                //     {
                //         grdCommissionRules.ActiveRow.Cells["ValueFrom"].Value = oldValueFrom;
                //         grdCommissionRules.UpdateData();
                //     }
                // }

                //// Value to
                //result = false;
                //Int64 updatedValueTo = Int64.Parse(grdCommissionRules.ActiveRow.Cells["ValueTo"].Value.ToString());
                //if (oldValueTo != updatedValueTo)
                //{
                //    result = ValueFromTest();
                //    if (result == true)
                //    {
                //        grdCommissionRules.ActiveRow.Cells["ValueTo"].Value = oldValueTo;
                //        grdCommissionRules.UpdateData();
                //    }
                //}

                ////Commission Rate Type                
                //result = false;
                //int upCommissionRateID = int.Parse(grdCommissionRules.ActiveRow.Cells["CommissionRateID_FK"].Value.ToString());
                //if (oldCommissionRateID != upCommissionRateID)
                //{
                //    result = ValueFromTest();
                //    if (result == true)
                //    {
                //        grdCommissionRules.ActiveRow.Cells["CommissionRateID_FK"].Value = oldCommissionRateID;
                //        grdCommissionRules.UpdateData();
                //    }
                //}

                ////Commission Rate               
                //result = false;
                //double upCommissionRate = double.Parse(grdCommissionRules.ActiveRow.Cells["CommisionRate"].Value.ToString());
                //if (oldCommissionRate != upCommissionRate)
                //{
                //    result = ValueFromTest();
                //    if (result == true)
                //    {
                //        grdCommissionRules.ActiveRow.Cells["CommisionRate"].Value = oldCommissionRateID;
                //        grdCommissionRules.UpdateData();
                //    }
                //}

                if (grdCommissionRules.ActiveRow.Cells["CommissionRateID_FK"].Text == "Shares")
                {
                    grdCommissionRules.ActiveRow.Cells["TextCaption"].Value = "Cents per Shares";
                }
                else if (grdCommissionRules.ActiveRow.Cells["CommissionRateID_FK"].Text == "Contracts")
                {
                    grdCommissionRules.ActiveRow.Cells["TextCaption"].Value = "Per Contract";
                }
                else if (grdCommissionRules.ActiveRow.Cells["CommissionRateID_FK"].Text == "Notional")
                {
                    grdCommissionRules.ActiveRow.Cells["TextCaption"].Value = "Basis Points";
                }
                else if (grdCommissionRules.ActiveRow.Cells["CommissionRateID_FK"].Text == "- Select -")
                {
                    grdCommissionRules.ActiveRow.Cells["TextCaption"].Value = "";
                }

                AddNewRow();
                this.drpDownCommisionRateType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            }
            catch (Exception)
            {
                //if (ex.Message == "Value could not be converted to System.Int32.")
                //{
                //    MessageBox.Show("Please enter the integer value", "Prana Alert");
                //    grdCommissionRules.Focus();
                //    return;
                //}
                //else
                //{
                //    MessageBox.Show("Value can not be blank", "Prana Alert");
                //    grdCommissionRules.Focus();
                //    return;

                //}
            }

        }


        private bool ValueFromTest()
        {
            bool result = false;
            Int64 Valuefrom = Int64.Parse(grdCommissionRules.ActiveRow.Cells["ValueFrom"].Value.ToString());
            Int64 Valueto = Int64.Parse(grdCommissionRules.ActiveRow.Cells["ValueTo"].Value.ToString());
            int commirateid = int.Parse(grdCommissionRules.ActiveRow.Cells["CommissionRateID_FK"].Value.ToString());
            double CommissionRate = double.Parse(grdCommissionRules.ActiveRow.Cells["CommisionRate"].Value.ToString());

            int currentIndex = grdCommissionRules.ActiveRow.Index;
            int checkIndex = int.MinValue;
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in grdCommissionRules.Rows)
            {
                Int64 dValuefrom = Int64.Parse(dr.Cells["ValueFrom"].Value.ToString());
                Int64 dValueto = Int64.Parse(dr.Cells["ValueTo"].Value.ToString());
                int dcommirateid = int.Parse(dr.Cells["CommissionRateID_FK"].Value.ToString());
                double dCommissionRate = double.Parse(dr.Cells["CommisionRate"].Value.ToString());

                checkIndex = dr.Index;
                if (Valuefrom == dValuefrom && Valueto == dValueto && commirateid == dcommirateid && CommissionRate == dCommissionRate && checkIndex != currentIndex)
                {
                    result = true;
                    MessageBox.Show(this, "The combination of Value From,Value To, Commission on and Commission Rate already exists, you cannot choose this combination again.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
            }
            return result;
        }

        private void grdCommissionRules_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {


        }

        private void grdCommissionRules_AfterCellUpdate(object sender, CellEventArgs e)
        {
            // grdCommissionRules.UpdateData();

            // if ((int.Parse(grdCommissionRules.ActiveRow.Cells["ValueFrom"].Value.ToString())) > 0 && (int.Parse(grdCommissionRules.ActiveRow.Cells["ValueTo"].Value.ToString())) > 0)
            //{
            //    if (int.Parse(grdCommissionRules.ActiveRow.Cells["ValueFrom"].Value.ToString()) > int.Parse(grdCommissionRules.ActiveRow.Cells["ValueTo"].Value.ToString()))
            //    {
            //        MessageBox.Show("Wrong Entry");                   
            //        grdCommissionRules.Focus();
            //        return;
            //    }
            //}

        }

        private void grdCommissionRules_ClickCellButton(object sender, CellEventArgs e)
        {
            grdCommissionRules.UpdateData();

            if (grdCommissionRules.DisplayLayout.Bands[0].Columns["DeleteButton"].Key == "DeleteButton")
            {
                if (int.Parse(grdCommissionRules.ActiveRow.Cells["CommissionRateID_FK"].Value.ToString()) != int.MinValue && Int64.Parse(grdCommissionRules.ActiveRow.Cells["ValueFrom"].Value.ToString()) > 0 && Int64.Parse(grdCommissionRules.ActiveRow.Cells["CommissionRateID_FK"].Value.ToString()) > 0)
                {
                    grdCommissionRules.ActiveRow.Delete();
                }

            }
        }

        private void grdCommissionRules_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            e.Cancel = true;
        }


        private void chkClearFee_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClearFee.Checked == true)
            {
                cmbCalculationClear.Enabled = true;
                cmbCurrencyUsedClear.Enabled = true;
                nudCommissionRateClear.Enabled = true;
            }
            else
            {
                cmbCalculationClear.Enabled = false;
                cmbCurrencyUsedClear.Enabled = false;
                nudCommissionRateClear.Enabled = false;
                RefreshClearingFee();
            }

        }

        private void cmbCalculationClear_ValueChanged(object sender, EventArgs e)
        {
            if (cmbCalculationClear.Value != null)
            {
                if (cmbCalculationClear.Text == "Shares")
                {
                    lbldisplayClear.Text = "Cents Per Share";
                }
                else if (cmbCalculationClear.Text == "Contracts")
                {
                    lbldisplayClear.Text = "per contract";
                }
                else if (cmbCalculationClear.Text == "Notional")
                {
                    lbldisplayClear.Text = "Basis Points";
                }
                else
                {
                    lbldisplayClear.Text = "";
                }
            }
        }







    }

}
