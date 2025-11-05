#region Using
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
    /// Summary description for CreateCommissionRules.
    /// </summary>
    public class CreateCommissionRules : System.Windows.Forms.Form
    {
        private const string FORM_NAME = " CreateCommissionRules : ";

        const int ZERO = 0;
        const int PER_UNIT_COMMISSIONRATE = 0;
        const int BASIS_POINT_COMMISSIONRATE = 0;
        private IContainer components;
        private System.Windows.Forms.ErrorProvider errorProvider1;

        private bool openView = false;
        public bool OpenView
        {
            set
            {
                openView = value;

                btnSave.Hide();
            }

        }


        private bool disableView = false;
        public bool DisableView
        {
            set
            {
                disableView = value;
                if (disableView == true)
                {
                    cmbAUEC.Enabled = false;
                    cmbApplyRuleto.Enabled = false;
                    cmbCalculation.Enabled = false;
                    cmbCommissionRate.Enabled = false;
                    cmbCommissionRate1.Enabled = false;
                    cmbCommissionRate2.Enabled = false;
                    cmbCommissionRate3.Enabled = false;
                    cmbCriteria.Enabled = false;
                    cmbCurrencyUsed.Enabled = false;
                    cmbLOperator.Enabled = false;
                    cmbLOperator1.Enabled = false;
                    cmbLOperator2.Enabled = false;
                    txtDescription.Enabled = false;
                    txtNamefRule.Enabled = false;
                    nudCommissionRate.Enabled = false;
                    nudCommissionRate1.Enabled = false;
                    nudCommissionRate2.Enabled = false;
                    nudCommissionRate3.Enabled = false;
                    nudCommissionRate4.Enabled = false;

                    numConstant.Enabled = false;
                    numConstant1.Enabled = false;
                    numConstant2.Enabled = false;
                    chkACofRule.Enabled = false;
                    nudCommissionRate4.Enabled = false;

                    btnResetRule1.Enabled = false;
                    btnResetRule2.Enabled = false;
                    btnResetRule3.Enabled = false;

                }


            }

        }


        Prana.Admin.BLL.AllCommissionRule _ruleEdit = null;
        public Prana.Admin.BLL.AllCommissionRule allCommissionRule
        {
            set
            {
                _ruleEdit = value;

            }

        }
        private int _ruleID = int.MinValue;
        public int RuleID
        {
            set { _ruleID = value; }
            get { return _ruleID; }
        }

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;


        Prana.Admin.BLL.AllCommissionRule _ruleView = null;
        private System.Windows.Forms.GroupBox grpCriteria;
        private System.Windows.Forms.NumericUpDown numConstant2;
        private System.Windows.Forms.NumericUpDown numConstant1;
        private System.Windows.Forms.NumericUpDown numConstant;
        private System.Windows.Forms.NumericUpDown nudCommissionRate4;
        private System.Windows.Forms.NumericUpDown nudCommissionRate3;
        private System.Windows.Forms.NumericUpDown nudCommissionRate2;
        private System.Windows.Forms.NumericUpDown nudCommissionRate1;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCriteria;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCommissionRate1;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCommissionRate2;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCommissionRate3;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbLOperator;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbLOperator1;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbLOperator2;
        private System.Windows.Forms.Label lblCommissionRate3;
        private System.Windows.Forms.Label lblCommissionRate1;
        private System.Windows.Forms.Label lblconstant;
        private System.Windows.Forms.Label lblLOperator;
        private System.Windows.Forms.Label lblCriteria;
        private System.Windows.Forms.Label lblACofRule;
        private System.Windows.Forms.CheckBox chkACofRule;
        private System.Windows.Forms.GroupBox grpParameters;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudCommissionRate;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCommissionRate;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCalculation;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCurrencyUsed;
        private System.Windows.Forms.Label lblCommissionRate;
        private System.Windows.Forms.Label lblCurrencyUsed;
        private System.Windows.Forms.Label lblCalculation;
        private System.Windows.Forms.GroupBox grpRule;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbApplyRuleto;
        private System.Windows.Forms.TextBox txtNamefRule;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblApplyRuleTo;
        private System.Windows.Forms.Label lblNameofRule;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblAUEC;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAUEC;
        private System.Windows.Forms.Button btnResetRule3;
        private System.Windows.Forms.Button btnResetRule2;
        private System.Windows.Forms.Button btnResetRule1;
        private Label lbldisplay;
        private Label lbldisplay3;
        private Label lbldisplay2;
        private Label lbldisplay1;

        public Prana.Admin.BLL.AllCommissionRule allCommissionRuleView
        {
            set
            {
                _ruleView = value;

            }


        }

        const string C_COMBO_SELECT = "- Select -";

        public CreateCommissionRules()
        {

            InitializeComponent();
            try
            {
                if (_ruleEdit != null)
                {
                    BindForEdit();
                }
                if (_ruleView != null)
                {
                    BindForView();
                }
                else
                {
                    RefreshAllCommissionRuleDetails();




                    //BindAUECS();
                    BindCommissionRate();
                    BindCalculation();
                    BindApplyRuleTo();
                    BindCurrencyUsed();
                    BindOperator();
                    BindCriteria();
                    BindALLAUEC();
                    BindDetails();

                }
            }

            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            # endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("ExchangeForm_Load",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "CreateCommissionRules_Load", null);
                #endregion
            }

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (grpCriteria != null)
                {
                    grpCriteria.Dispose();
                }
                if (numConstant2 != null)
                {
                    numConstant2.Dispose();
                }
                if (numConstant1 != null)
                {
                    numConstant1.Dispose();
                }
                if (numConstant != null)
                {
                    numConstant.Dispose();
                }
                if (nudCommissionRate4 != null)
                {
                    nudCommissionRate4.Dispose();
                }
                if (nudCommissionRate3 != null)
                {
                    nudCommissionRate3.Dispose();
                }
                if (nudCommissionRate2 != null)
                {
                    nudCommissionRate2.Dispose();
                }
                if (nudCommissionRate1 != null)
                {
                    nudCommissionRate1.Dispose();
                }
                if (cmbCriteria != null)
                {
                    cmbCriteria.Dispose();
                }
                if (cmbCommissionRate1 != null)
                {
                    cmbCommissionRate1.Dispose();
                }
                if (cmbCommissionRate2 != null)
                {
                    cmbCommissionRate2.Dispose();
                }
                if (cmbCommissionRate3 != null)
                {
                    cmbCommissionRate3.Dispose();
                }
                if (cmbLOperator != null)
                {
                    cmbLOperator.Dispose();
                }
                if (cmbLOperator1 != null)
                {
                    cmbLOperator1.Dispose();
                }
                if (cmbLOperator2 != null)
                {
                    cmbLOperator2.Dispose();
                }
                if (lblCommissionRate3 != null)
                {
                    lblCommissionRate3.Dispose();
                }
                if (lblCommissionRate1 != null)
                {
                    lblCommissionRate1.Dispose();
                }
                if (lblconstant != null)
                {
                    lblconstant.Dispose();
                }
                if (lblLOperator != null)
                {
                    lblLOperator.Dispose();
                }
                if (lblCriteria != null)
                {
                    lblCriteria.Dispose();
                }
                if (lblACofRule != null)
                {
                    lblACofRule.Dispose();
                }
                if (chkACofRule != null)
                {
                    chkACofRule.Dispose();
                }
                if (grpParameters != null)
                {
                    grpParameters.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (nudCommissionRate != null)
                {
                    nudCommissionRate.Dispose();
                }
                if (cmbCommissionRate != null)
                {
                    cmbCommissionRate.Dispose();
                }
                if (cmbCalculation != null)
                {
                    cmbCalculation.Dispose();
                }
                if (cmbCurrencyUsed != null)
                {
                    cmbCurrencyUsed.Dispose();
                }
                if (lblCommissionRate != null)
                {
                    lblCommissionRate.Dispose();
                }
                if (lblCurrencyUsed != null)
                {
                    lblCurrencyUsed.Dispose();
                }
                if (lblCalculation != null)
                {
                    lblCalculation.Dispose();
                }
                if (grpRule != null)
                {
                    grpRule.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (cmbApplyRuleto != null)
                {
                    cmbApplyRuleto.Dispose();
                }
                if (txtNamefRule != null)
                {
                    txtNamefRule.Dispose();
                }
                if (lblDescription != null)
                {
                    lblDescription.Dispose();
                }
                if (lblApplyRuleTo != null)
                {
                    lblApplyRuleTo.Dispose();
                }
                if (lblNameofRule != null)
                {
                    lblNameofRule.Dispose();
                }
                if (txtDescription != null)
                {
                    txtDescription.Dispose();
                }
                if (lblAUEC != null)
                {
                    lblAUEC.Dispose();
                }
                if (cmbAUEC != null)
                {
                    cmbAUEC.Dispose();
                }
                if (btnResetRule3 != null)
                {
                    btnResetRule3.Dispose();
                }
                if (btnResetRule2 != null)
                {
                    btnResetRule2.Dispose();
                }
                if (btnResetRule1 != null)
                {
                    btnResetRule1.Dispose();
                }
                if (lbldisplay != null)
                {
                    lbldisplay.Dispose();
                }
                if (lbldisplay3 != null)
                {
                    lbldisplay3.Dispose();
                }
                if (lbldisplay2 != null)
                {
                    lbldisplay2.Dispose();
                }
                if (lbldisplay1 != null)
                {
                    lbldisplay1.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateCommissionRules));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand13 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Unit Name", 1);
            Infragistics.Win.Appearance appearance133 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand14 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn29 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateTypeName", 1);
            Infragistics.Win.Appearance appearance134 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance135 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance136 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance137 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance138 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance139 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance140 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance141 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance142 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance143 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance144 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance145 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand15 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn30 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn31 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateTypeName", 1);
            Infragistics.Win.Appearance appearance146 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance147 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance148 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance149 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance150 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance151 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance152 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance153 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance154 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance155 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance156 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance157 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand16 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn32 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn33 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateTypeName", 1);
            Infragistics.Win.Appearance appearance158 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance159 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance160 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance161 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance162 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance163 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance164 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance165 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance166 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance167 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance168 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance169 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand17 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn34 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OperatorID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn35 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
            Infragistics.Win.Appearance appearance170 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance171 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance172 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance173 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance174 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance175 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance176 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance177 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance178 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance179 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance180 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance181 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand18 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn36 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OperatorID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn37 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Operator Name", 1);
            Infragistics.Win.Appearance appearance182 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance183 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance184 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance185 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance186 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance187 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance188 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance189 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance190 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance191 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance192 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance193 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand19 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn38 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OperatorID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn39 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Operator Name", 1);
            Infragistics.Win.Appearance appearance194 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance195 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance196 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance197 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance198 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance199 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance200 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance201 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance202 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance203 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance204 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance205 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand20 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn40 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn41 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionRateTypeName", 1);
            Infragistics.Win.Appearance appearance206 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance207 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance208 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance209 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance210 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance211 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance212 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance213 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance214 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance215 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance216 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance217 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand21 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn42 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CommissionCalculationID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn43 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CalculationType", 1);
            Infragistics.Win.Appearance appearance218 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance219 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance220 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance221 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance222 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance223 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance224 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance225 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance226 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance227 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance228 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance229 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand22 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn44 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurencyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn45 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn46 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencySymbol", 2);
            Infragistics.Win.Appearance appearance230 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance231 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance232 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance233 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance234 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance235 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance236 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance237 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance238 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance239 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance240 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance241 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand23 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn47 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ApplyRuleID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn48 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradeType", 1);
            Infragistics.Win.Appearance appearance242 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance243 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance244 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance245 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance246 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance247 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance248 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance249 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance250 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance251 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance252 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance253 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand24 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn49 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn50 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 1);
            Infragistics.Win.Appearance appearance254 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance255 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance256 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance257 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance258 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance259 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance260 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance261 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance262 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance263 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance264 = new Infragistics.Win.Appearance();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpCriteria = new System.Windows.Forms.GroupBox();
            this.lbldisplay3 = new System.Windows.Forms.Label();
            this.lbldisplay2 = new System.Windows.Forms.Label();
            this.lbldisplay1 = new System.Windows.Forms.Label();
            this.btnResetRule3 = new System.Windows.Forms.Button();
            this.btnResetRule2 = new System.Windows.Forms.Button();
            this.btnResetRule1 = new System.Windows.Forms.Button();
            this.numConstant2 = new System.Windows.Forms.NumericUpDown();
            this.numConstant1 = new System.Windows.Forms.NumericUpDown();
            this.numConstant = new System.Windows.Forms.NumericUpDown();
            this.nudCommissionRate4 = new System.Windows.Forms.NumericUpDown();
            this.nudCommissionRate3 = new System.Windows.Forms.NumericUpDown();
            this.nudCommissionRate2 = new System.Windows.Forms.NumericUpDown();
            this.nudCommissionRate1 = new System.Windows.Forms.NumericUpDown();
            this.cmbCriteria = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCommissionRate1 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCommissionRate2 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCommissionRate3 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbLOperator = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbLOperator1 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbLOperator2 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblCommissionRate3 = new System.Windows.Forms.Label();
            this.lblCommissionRate1 = new System.Windows.Forms.Label();
            this.lblconstant = new System.Windows.Forms.Label();
            this.lblLOperator = new System.Windows.Forms.Label();
            this.lblCriteria = new System.Windows.Forms.Label();
            this.lblACofRule = new System.Windows.Forms.Label();
            this.chkACofRule = new System.Windows.Forms.CheckBox();
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
            this.grpRule = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbApplyRuleto = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.txtNamefRule = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblApplyRuleTo = new System.Windows.Forms.Label();
            this.lblNameofRule = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblAUEC = new System.Windows.Forms.Label();
            this.cmbAUEC = new Infragistics.Win.UltraWinGrid.UltraCombo();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.grpCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numConstant2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConstant1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConstant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCriteria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommissionRate1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommissionRate2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommissionRate3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLOperator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLOperator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLOperator2)).BeginInit();
            this.grpParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommissionRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCalculation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrencyUsed)).BeginInit();
            this.grpRule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbApplyRuleto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAUEC)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Location = new System.Drawing.Point(234, 345);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 24;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Location = new System.Drawing.Point(312, 345);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 25;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grpCriteria
            // 
            this.grpCriteria.Controls.Add(this.lbldisplay3);
            this.grpCriteria.Controls.Add(this.lbldisplay2);
            this.grpCriteria.Controls.Add(this.lbldisplay1);
            this.grpCriteria.Controls.Add(this.btnResetRule3);
            this.grpCriteria.Controls.Add(this.btnResetRule2);
            this.grpCriteria.Controls.Add(this.btnResetRule1);
            this.grpCriteria.Controls.Add(this.numConstant2);
            this.grpCriteria.Controls.Add(this.numConstant1);
            this.grpCriteria.Controls.Add(this.numConstant);
            this.grpCriteria.Controls.Add(this.nudCommissionRate4);
            this.grpCriteria.Controls.Add(this.nudCommissionRate3);
            this.grpCriteria.Controls.Add(this.nudCommissionRate2);
            this.grpCriteria.Controls.Add(this.nudCommissionRate1);
            this.grpCriteria.Controls.Add(this.cmbCriteria);
            this.grpCriteria.Controls.Add(this.cmbCommissionRate1);
            this.grpCriteria.Controls.Add(this.cmbCommissionRate2);
            this.grpCriteria.Controls.Add(this.cmbCommissionRate3);
            this.grpCriteria.Controls.Add(this.cmbLOperator);
            this.grpCriteria.Controls.Add(this.cmbLOperator1);
            this.grpCriteria.Controls.Add(this.cmbLOperator2);
            this.grpCriteria.Controls.Add(this.lblCommissionRate3);
            this.grpCriteria.Controls.Add(this.lblCommissionRate1);
            this.grpCriteria.Controls.Add(this.lblconstant);
            this.grpCriteria.Controls.Add(this.lblLOperator);
            this.grpCriteria.Controls.Add(this.lblCriteria);
            this.grpCriteria.Controls.Add(this.lblACofRule);
            this.grpCriteria.Controls.Add(this.chkACofRule);
            this.grpCriteria.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpCriteria.Location = new System.Drawing.Point(3, 180);
            this.grpCriteria.Name = "grpCriteria";
            this.grpCriteria.Size = new System.Drawing.Size(668, 158);
            this.grpCriteria.TabIndex = 29;
            this.grpCriteria.TabStop = false;
            this.grpCriteria.Text = "Criteria";
            // 
            // lbldisplay3
            // 
            this.lbldisplay3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbldisplay3.Location = new System.Drawing.Point(494, 107);
            this.lbldisplay3.Name = "lbldisplay3";
            this.lbldisplay3.Size = new System.Drawing.Size(87, 18);
            this.lbldisplay3.TabIndex = 45;
            this.lbldisplay3.Text = "lbldisplay3";
            // 
            // lbldisplay2
            // 
            this.lbldisplay2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbldisplay2.Location = new System.Drawing.Point(494, 86);
            this.lbldisplay2.Name = "lbldisplay2";
            this.lbldisplay2.Size = new System.Drawing.Size(87, 18);
            this.lbldisplay2.TabIndex = 44;
            this.lbldisplay2.Text = "lbldisplay2";
            // 
            // lbldisplay1
            // 
            this.lbldisplay1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbldisplay1.Location = new System.Drawing.Point(494, 63);
            this.lbldisplay1.Name = "lbldisplay1";
            this.lbldisplay1.Size = new System.Drawing.Size(87, 18);
            this.lbldisplay1.TabIndex = 43;
            this.lbldisplay1.Text = "lbldisplay1";
            // 
            // btnResetRule3
            // 
            this.btnResetRule3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnResetRule3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(150)))));
            this.btnResetRule3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnResetRule3.BackgroundImage")));
            this.btnResetRule3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnResetRule3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnResetRule3.Location = new System.Drawing.Point(587, 104);
            this.btnResetRule3.Name = "btnResetRule3";
            this.btnResetRule3.Size = new System.Drawing.Size(75, 23);
            this.btnResetRule3.TabIndex = 42;
            this.btnResetRule3.UseVisualStyleBackColor = false;
            this.btnResetRule3.Click += new System.EventHandler(this.btnResetRule3_Click);
            // 
            // btnResetRule2
            // 
            this.btnResetRule2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnResetRule2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(150)))));
            this.btnResetRule2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnResetRule2.BackgroundImage")));
            this.btnResetRule2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnResetRule2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnResetRule2.Location = new System.Drawing.Point(587, 80);
            this.btnResetRule2.Name = "btnResetRule2";
            this.btnResetRule2.Size = new System.Drawing.Size(75, 23);
            this.btnResetRule2.TabIndex = 41;
            this.btnResetRule2.UseVisualStyleBackColor = false;
            this.btnResetRule2.Click += new System.EventHandler(this.btnResetRule2_Click);
            // 
            // btnResetRule1
            // 
            this.btnResetRule1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnResetRule1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(150)))));
            this.btnResetRule1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnResetRule1.BackgroundImage")));
            this.btnResetRule1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnResetRule1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnResetRule1.Location = new System.Drawing.Point(587, 58);
            this.btnResetRule1.Name = "btnResetRule1";
            this.btnResetRule1.Size = new System.Drawing.Size(75, 23);
            this.btnResetRule1.TabIndex = 40;
            this.btnResetRule1.UseVisualStyleBackColor = false;
            this.btnResetRule1.Click += new System.EventHandler(this.btnResetRule1_Click);
            // 
            // numConstant2
            // 
            this.numConstant2.BackColor = System.Drawing.Color.White;
            this.numConstant2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.numConstant2.Location = new System.Drawing.Point(194, 104);
            this.numConstant2.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numConstant2.Name = "numConstant2";
            this.numConstant2.Size = new System.Drawing.Size(104, 21);
            this.numConstant2.TabIndex = 20;
            this.numConstant2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numConstant2_KeyUp);
            // 
            // numConstant1
            // 
            this.numConstant1.BackColor = System.Drawing.Color.White;
            this.numConstant1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.numConstant1.Location = new System.Drawing.Point(194, 82);
            this.numConstant1.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numConstant1.Name = "numConstant1";
            this.numConstant1.Size = new System.Drawing.Size(104, 21);
            this.numConstant1.TabIndex = 16;
            this.numConstant1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numConstant1_KeyUp);
            // 
            // numConstant
            // 
            this.numConstant.BackColor = System.Drawing.Color.White;
            this.numConstant.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.numConstant.Location = new System.Drawing.Point(194, 60);
            this.numConstant.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numConstant.Name = "numConstant";
            this.numConstant.Size = new System.Drawing.Size(104, 21);
            this.numConstant.TabIndex = 12;
            this.numConstant.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numConstant_KeyUp);
            // 
            // nudCommissionRate4
            // 
            this.nudCommissionRate4.BackColor = System.Drawing.Color.White;
            this.nudCommissionRate4.DecimalPlaces = 2;
            this.nudCommissionRate4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudCommissionRate4.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nudCommissionRate4.Location = new System.Drawing.Point(194, 131);
            this.nudCommissionRate4.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudCommissionRate4.Name = "nudCommissionRate4";
            this.nudCommissionRate4.Size = new System.Drawing.Size(104, 21);
            this.nudCommissionRate4.TabIndex = 23;
            // 
            // nudCommissionRate3
            // 
            this.nudCommissionRate3.BackColor = System.Drawing.Color.White;
            this.nudCommissionRate3.DecimalPlaces = 1;
            this.nudCommissionRate3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudCommissionRate3.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudCommissionRate3.Location = new System.Drawing.Point(413, 104);
            this.nudCommissionRate3.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudCommissionRate3.Name = "nudCommissionRate3";
            this.nudCommissionRate3.Size = new System.Drawing.Size(76, 21);
            this.nudCommissionRate3.TabIndex = 22;
            // 
            // nudCommissionRate2
            // 
            this.nudCommissionRate2.BackColor = System.Drawing.Color.White;
            this.nudCommissionRate2.DecimalPlaces = 1;
            this.nudCommissionRate2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudCommissionRate2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudCommissionRate2.Location = new System.Drawing.Point(413, 82);
            this.nudCommissionRate2.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudCommissionRate2.Name = "nudCommissionRate2";
            this.nudCommissionRate2.Size = new System.Drawing.Size(76, 21);
            this.nudCommissionRate2.TabIndex = 18;
            // 
            // nudCommissionRate1
            // 
            this.nudCommissionRate1.BackColor = System.Drawing.Color.White;
            this.nudCommissionRate1.DecimalPlaces = 1;
            this.nudCommissionRate1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudCommissionRate1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudCommissionRate1.Location = new System.Drawing.Point(413, 60);
            this.nudCommissionRate1.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudCommissionRate1.Name = "nudCommissionRate1";
            this.nudCommissionRate1.Size = new System.Drawing.Size(76, 21);
            this.nudCommissionRate1.TabIndex = 14;
            // 
            // cmbCriteria
            // 
            this.cmbCriteria.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbCriteria.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand13.ColHeadersVisible = false;
            ultraGridColumn26.Header.VisiblePosition = 0;
            ultraGridColumn26.Hidden = true;
            ultraGridColumn27.Header.VisiblePosition = 1;
            ultraGridBand13.Columns.AddRange(new object[] {
            ultraGridColumn26,
            ultraGridColumn27});
            this.cmbCriteria.DisplayLayout.BandsSerializer.Add(ultraGridBand13);
            this.cmbCriteria.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCriteria.DisplayMember = "";
            this.cmbCriteria.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCriteria.DropDownWidth = 0;
            this.cmbCriteria.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCriteria.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCriteria.Location = new System.Drawing.Point(6, 60);
            this.cmbCriteria.Name = "cmbCriteria";
            this.cmbCriteria.Size = new System.Drawing.Size(94, 21);
            this.cmbCriteria.TabIndex = 10;
            this.cmbCriteria.ValueMember = "";
            // 
            // cmbCommissionRate1
            // 
            this.cmbCommissionRate1.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance133.BackColor = System.Drawing.SystemColors.Window;
            appearance133.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCommissionRate1.DisplayLayout.Appearance = appearance133;
            this.cmbCommissionRate1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand14.ColHeadersVisible = false;
            ultraGridColumn28.Header.VisiblePosition = 0;
            ultraGridColumn28.Hidden = true;
            ultraGridColumn29.Header.VisiblePosition = 1;
            ultraGridBand14.Columns.AddRange(new object[] {
            ultraGridColumn28,
            ultraGridColumn29});
            this.cmbCommissionRate1.DisplayLayout.BandsSerializer.Add(ultraGridBand14);
            this.cmbCommissionRate1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCommissionRate1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance134.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance134.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance134.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance134.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate1.DisplayLayout.GroupByBox.Appearance = appearance134;
            appearance135.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCommissionRate1.DisplayLayout.GroupByBox.BandLabelAppearance = appearance135;
            this.cmbCommissionRate1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance136.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance136.BackColor2 = System.Drawing.SystemColors.Control;
            appearance136.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance136.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCommissionRate1.DisplayLayout.GroupByBox.PromptAppearance = appearance136;
            this.cmbCommissionRate1.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCommissionRate1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance137.BackColor = System.Drawing.SystemColors.Window;
            appearance137.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCommissionRate1.DisplayLayout.Override.ActiveCellAppearance = appearance137;
            appearance138.BackColor = System.Drawing.SystemColors.Highlight;
            appearance138.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCommissionRate1.DisplayLayout.Override.ActiveRowAppearance = appearance138;
            this.cmbCommissionRate1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCommissionRate1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance139.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate1.DisplayLayout.Override.CardAreaAppearance = appearance139;
            appearance140.BorderColor = System.Drawing.Color.Silver;
            appearance140.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCommissionRate1.DisplayLayout.Override.CellAppearance = appearance140;
            this.cmbCommissionRate1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCommissionRate1.DisplayLayout.Override.CellPadding = 0;
            appearance141.BackColor = System.Drawing.SystemColors.Control;
            appearance141.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance141.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance141.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance141.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate1.DisplayLayout.Override.GroupByRowAppearance = appearance141;
            appearance142.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCommissionRate1.DisplayLayout.Override.HeaderAppearance = appearance142;
            this.cmbCommissionRate1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCommissionRate1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance143.BackColor = System.Drawing.SystemColors.Window;
            appearance143.BorderColor = System.Drawing.Color.Silver;
            this.cmbCommissionRate1.DisplayLayout.Override.RowAppearance = appearance143;
            this.cmbCommissionRate1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance144.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCommissionRate1.DisplayLayout.Override.TemplateAddRowAppearance = appearance144;
            this.cmbCommissionRate1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCommissionRate1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCommissionRate1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCommissionRate1.DisplayMember = "";
            this.cmbCommissionRate1.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCommissionRate1.DropDownWidth = 0;
            this.cmbCommissionRate1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCommissionRate1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCommissionRate1.Location = new System.Drawing.Point(316, 60);
            this.cmbCommissionRate1.Name = "cmbCommissionRate1";
            this.cmbCommissionRate1.Size = new System.Drawing.Size(79, 21);
            this.cmbCommissionRate1.TabIndex = 13;
            this.cmbCommissionRate1.ValueMember = "";
            this.cmbCommissionRate1.ValueChanged += new System.EventHandler(this.cmbCommissionRate1_ValueChanged);
            // 
            // cmbCommissionRate2
            // 
            this.cmbCommissionRate2.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance145.BackColor = System.Drawing.SystemColors.Window;
            appearance145.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCommissionRate2.DisplayLayout.Appearance = appearance145;
            this.cmbCommissionRate2.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand15.ColHeadersVisible = false;
            ultraGridColumn30.Header.VisiblePosition = 0;
            ultraGridColumn30.Hidden = true;
            ultraGridColumn31.Header.VisiblePosition = 1;
            ultraGridBand15.Columns.AddRange(new object[] {
            ultraGridColumn30,
            ultraGridColumn31});
            this.cmbCommissionRate2.DisplayLayout.BandsSerializer.Add(ultraGridBand15);
            this.cmbCommissionRate2.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCommissionRate2.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance146.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance146.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance146.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance146.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate2.DisplayLayout.GroupByBox.Appearance = appearance146;
            appearance147.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCommissionRate2.DisplayLayout.GroupByBox.BandLabelAppearance = appearance147;
            this.cmbCommissionRate2.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance148.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance148.BackColor2 = System.Drawing.SystemColors.Control;
            appearance148.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance148.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCommissionRate2.DisplayLayout.GroupByBox.PromptAppearance = appearance148;
            this.cmbCommissionRate2.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCommissionRate2.DisplayLayout.MaxRowScrollRegions = 1;
            appearance149.BackColor = System.Drawing.SystemColors.Window;
            appearance149.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCommissionRate2.DisplayLayout.Override.ActiveCellAppearance = appearance149;
            appearance150.BackColor = System.Drawing.SystemColors.Highlight;
            appearance150.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCommissionRate2.DisplayLayout.Override.ActiveRowAppearance = appearance150;
            this.cmbCommissionRate2.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCommissionRate2.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance151.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate2.DisplayLayout.Override.CardAreaAppearance = appearance151;
            appearance152.BorderColor = System.Drawing.Color.Silver;
            appearance152.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCommissionRate2.DisplayLayout.Override.CellAppearance = appearance152;
            this.cmbCommissionRate2.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCommissionRate2.DisplayLayout.Override.CellPadding = 0;
            appearance153.BackColor = System.Drawing.SystemColors.Control;
            appearance153.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance153.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance153.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance153.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate2.DisplayLayout.Override.GroupByRowAppearance = appearance153;
            appearance154.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCommissionRate2.DisplayLayout.Override.HeaderAppearance = appearance154;
            this.cmbCommissionRate2.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCommissionRate2.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance155.BackColor = System.Drawing.SystemColors.Window;
            appearance155.BorderColor = System.Drawing.Color.Silver;
            this.cmbCommissionRate2.DisplayLayout.Override.RowAppearance = appearance155;
            this.cmbCommissionRate2.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance156.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCommissionRate2.DisplayLayout.Override.TemplateAddRowAppearance = appearance156;
            this.cmbCommissionRate2.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCommissionRate2.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCommissionRate2.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCommissionRate2.DisplayMember = "";
            this.cmbCommissionRate2.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCommissionRate2.DropDownWidth = 0;
            this.cmbCommissionRate2.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCommissionRate2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCommissionRate2.Location = new System.Drawing.Point(316, 82);
            this.cmbCommissionRate2.Name = "cmbCommissionRate2";
            this.cmbCommissionRate2.Size = new System.Drawing.Size(79, 21);
            this.cmbCommissionRate2.TabIndex = 17;
            this.cmbCommissionRate2.ValueMember = "";
            this.cmbCommissionRate2.ValueChanged += new System.EventHandler(this.cmbCommissionRate2_ValueChanged);
            // 
            // cmbCommissionRate3
            // 
            this.cmbCommissionRate3.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance157.BackColor = System.Drawing.SystemColors.Window;
            appearance157.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCommissionRate3.DisplayLayout.Appearance = appearance157;
            this.cmbCommissionRate3.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand16.ColHeadersVisible = false;
            ultraGridColumn32.Header.VisiblePosition = 0;
            ultraGridColumn32.Hidden = true;
            ultraGridColumn33.Header.VisiblePosition = 1;
            ultraGridBand16.Columns.AddRange(new object[] {
            ultraGridColumn32,
            ultraGridColumn33});
            this.cmbCommissionRate3.DisplayLayout.BandsSerializer.Add(ultraGridBand16);
            this.cmbCommissionRate3.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCommissionRate3.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance158.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance158.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance158.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance158.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate3.DisplayLayout.GroupByBox.Appearance = appearance158;
            appearance159.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCommissionRate3.DisplayLayout.GroupByBox.BandLabelAppearance = appearance159;
            this.cmbCommissionRate3.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance160.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance160.BackColor2 = System.Drawing.SystemColors.Control;
            appearance160.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance160.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCommissionRate3.DisplayLayout.GroupByBox.PromptAppearance = appearance160;
            this.cmbCommissionRate3.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCommissionRate3.DisplayLayout.MaxRowScrollRegions = 1;
            appearance161.BackColor = System.Drawing.SystemColors.Window;
            appearance161.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCommissionRate3.DisplayLayout.Override.ActiveCellAppearance = appearance161;
            appearance162.BackColor = System.Drawing.SystemColors.Highlight;
            appearance162.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCommissionRate3.DisplayLayout.Override.ActiveRowAppearance = appearance162;
            this.cmbCommissionRate3.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCommissionRate3.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance163.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate3.DisplayLayout.Override.CardAreaAppearance = appearance163;
            appearance164.BorderColor = System.Drawing.Color.Silver;
            appearance164.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCommissionRate3.DisplayLayout.Override.CellAppearance = appearance164;
            this.cmbCommissionRate3.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCommissionRate3.DisplayLayout.Override.CellPadding = 0;
            appearance165.BackColor = System.Drawing.SystemColors.Control;
            appearance165.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance165.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance165.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance165.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate3.DisplayLayout.Override.GroupByRowAppearance = appearance165;
            appearance166.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCommissionRate3.DisplayLayout.Override.HeaderAppearance = appearance166;
            this.cmbCommissionRate3.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCommissionRate3.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance167.BackColor = System.Drawing.SystemColors.Window;
            appearance167.BorderColor = System.Drawing.Color.Silver;
            this.cmbCommissionRate3.DisplayLayout.Override.RowAppearance = appearance167;
            this.cmbCommissionRate3.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance168.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCommissionRate3.DisplayLayout.Override.TemplateAddRowAppearance = appearance168;
            this.cmbCommissionRate3.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCommissionRate3.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCommissionRate3.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCommissionRate3.DisplayMember = "";
            this.cmbCommissionRate3.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCommissionRate3.DropDownWidth = 0;
            this.cmbCommissionRate3.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCommissionRate3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCommissionRate3.Location = new System.Drawing.Point(316, 104);
            this.cmbCommissionRate3.Name = "cmbCommissionRate3";
            this.cmbCommissionRate3.Size = new System.Drawing.Size(79, 21);
            this.cmbCommissionRate3.TabIndex = 21;
            this.cmbCommissionRate3.ValueMember = "";
            this.cmbCommissionRate3.ValueChanged += new System.EventHandler(this.cmbCommissionRate3_ValueChanged);
            // 
            // cmbLOperator
            // 
            this.cmbLOperator.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance169.BackColor = System.Drawing.SystemColors.Window;
            appearance169.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbLOperator.DisplayLayout.Appearance = appearance169;
            this.cmbLOperator.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand17.ColHeadersVisible = false;
            ultraGridColumn34.Header.VisiblePosition = 0;
            ultraGridColumn34.Hidden = true;
            ultraGridColumn35.Header.VisiblePosition = 1;
            ultraGridBand17.Columns.AddRange(new object[] {
            ultraGridColumn34,
            ultraGridColumn35});
            this.cmbLOperator.DisplayLayout.BandsSerializer.Add(ultraGridBand17);
            this.cmbLOperator.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbLOperator.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance170.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance170.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance170.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance170.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbLOperator.DisplayLayout.GroupByBox.Appearance = appearance170;
            appearance171.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbLOperator.DisplayLayout.GroupByBox.BandLabelAppearance = appearance171;
            this.cmbLOperator.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance172.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance172.BackColor2 = System.Drawing.SystemColors.Control;
            appearance172.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance172.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbLOperator.DisplayLayout.GroupByBox.PromptAppearance = appearance172;
            this.cmbLOperator.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbLOperator.DisplayLayout.MaxRowScrollRegions = 1;
            appearance173.BackColor = System.Drawing.SystemColors.Window;
            appearance173.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbLOperator.DisplayLayout.Override.ActiveCellAppearance = appearance173;
            appearance174.BackColor = System.Drawing.SystemColors.Highlight;
            appearance174.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbLOperator.DisplayLayout.Override.ActiveRowAppearance = appearance174;
            this.cmbLOperator.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbLOperator.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance175.BackColor = System.Drawing.SystemColors.Window;
            this.cmbLOperator.DisplayLayout.Override.CardAreaAppearance = appearance175;
            appearance176.BorderColor = System.Drawing.Color.Silver;
            appearance176.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbLOperator.DisplayLayout.Override.CellAppearance = appearance176;
            this.cmbLOperator.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbLOperator.DisplayLayout.Override.CellPadding = 0;
            appearance177.BackColor = System.Drawing.SystemColors.Control;
            appearance177.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance177.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance177.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance177.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbLOperator.DisplayLayout.Override.GroupByRowAppearance = appearance177;
            appearance178.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbLOperator.DisplayLayout.Override.HeaderAppearance = appearance178;
            this.cmbLOperator.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbLOperator.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance179.BackColor = System.Drawing.SystemColors.Window;
            appearance179.BorderColor = System.Drawing.Color.Silver;
            this.cmbLOperator.DisplayLayout.Override.RowAppearance = appearance179;
            this.cmbLOperator.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance180.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbLOperator.DisplayLayout.Override.TemplateAddRowAppearance = appearance180;
            this.cmbLOperator.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbLOperator.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbLOperator.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbLOperator.DisplayMember = "";
            this.cmbLOperator.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbLOperator.DropDownWidth = 0;
            this.cmbLOperator.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbLOperator.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbLOperator.Location = new System.Drawing.Point(112, 60);
            this.cmbLOperator.Name = "cmbLOperator";
            this.cmbLOperator.Size = new System.Drawing.Size(66, 21);
            this.cmbLOperator.TabIndex = 11;
            this.cmbLOperator.ValueMember = "";
            // 
            // cmbLOperator1
            // 
            this.cmbLOperator1.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance181.BackColor = System.Drawing.SystemColors.Window;
            appearance181.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbLOperator1.DisplayLayout.Appearance = appearance181;
            this.cmbLOperator1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand18.ColHeadersVisible = false;
            ultraGridColumn36.Header.VisiblePosition = 0;
            ultraGridColumn36.Hidden = true;
            ultraGridColumn37.Header.VisiblePosition = 1;
            ultraGridBand18.Columns.AddRange(new object[] {
            ultraGridColumn36,
            ultraGridColumn37});
            this.cmbLOperator1.DisplayLayout.BandsSerializer.Add(ultraGridBand18);
            this.cmbLOperator1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbLOperator1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance182.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance182.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance182.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance182.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbLOperator1.DisplayLayout.GroupByBox.Appearance = appearance182;
            appearance183.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbLOperator1.DisplayLayout.GroupByBox.BandLabelAppearance = appearance183;
            this.cmbLOperator1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance184.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance184.BackColor2 = System.Drawing.SystemColors.Control;
            appearance184.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance184.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbLOperator1.DisplayLayout.GroupByBox.PromptAppearance = appearance184;
            this.cmbLOperator1.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbLOperator1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance185.BackColor = System.Drawing.SystemColors.Window;
            appearance185.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbLOperator1.DisplayLayout.Override.ActiveCellAppearance = appearance185;
            appearance186.BackColor = System.Drawing.SystemColors.Highlight;
            appearance186.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbLOperator1.DisplayLayout.Override.ActiveRowAppearance = appearance186;
            this.cmbLOperator1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbLOperator1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance187.BackColor = System.Drawing.SystemColors.Window;
            this.cmbLOperator1.DisplayLayout.Override.CardAreaAppearance = appearance187;
            appearance188.BorderColor = System.Drawing.Color.Silver;
            appearance188.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbLOperator1.DisplayLayout.Override.CellAppearance = appearance188;
            this.cmbLOperator1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbLOperator1.DisplayLayout.Override.CellPadding = 0;
            appearance189.BackColor = System.Drawing.SystemColors.Control;
            appearance189.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance189.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance189.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance189.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbLOperator1.DisplayLayout.Override.GroupByRowAppearance = appearance189;
            appearance190.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbLOperator1.DisplayLayout.Override.HeaderAppearance = appearance190;
            this.cmbLOperator1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbLOperator1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance191.BackColor = System.Drawing.SystemColors.Window;
            appearance191.BorderColor = System.Drawing.Color.Silver;
            this.cmbLOperator1.DisplayLayout.Override.RowAppearance = appearance191;
            this.cmbLOperator1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance192.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbLOperator1.DisplayLayout.Override.TemplateAddRowAppearance = appearance192;
            this.cmbLOperator1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbLOperator1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbLOperator1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbLOperator1.DisplayMember = "";
            this.cmbLOperator1.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbLOperator1.DropDownWidth = 0;
            this.cmbLOperator1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbLOperator1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbLOperator1.Location = new System.Drawing.Point(112, 82);
            this.cmbLOperator1.Name = "cmbLOperator1";
            this.cmbLOperator1.Size = new System.Drawing.Size(66, 21);
            this.cmbLOperator1.TabIndex = 15;
            this.cmbLOperator1.ValueMember = "";
            // 
            // cmbLOperator2
            // 
            this.cmbLOperator2.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance193.BackColor = System.Drawing.SystemColors.Window;
            appearance193.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbLOperator2.DisplayLayout.Appearance = appearance193;
            this.cmbLOperator2.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand19.ColHeadersVisible = false;
            ultraGridColumn38.Header.VisiblePosition = 0;
            ultraGridColumn38.Hidden = true;
            ultraGridColumn39.Header.VisiblePosition = 1;
            ultraGridBand19.Columns.AddRange(new object[] {
            ultraGridColumn38,
            ultraGridColumn39});
            this.cmbLOperator2.DisplayLayout.BandsSerializer.Add(ultraGridBand19);
            this.cmbLOperator2.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbLOperator2.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance194.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance194.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance194.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance194.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbLOperator2.DisplayLayout.GroupByBox.Appearance = appearance194;
            appearance195.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbLOperator2.DisplayLayout.GroupByBox.BandLabelAppearance = appearance195;
            this.cmbLOperator2.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance196.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance196.BackColor2 = System.Drawing.SystemColors.Control;
            appearance196.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance196.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbLOperator2.DisplayLayout.GroupByBox.PromptAppearance = appearance196;
            this.cmbLOperator2.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbLOperator2.DisplayLayout.MaxRowScrollRegions = 1;
            appearance197.BackColor = System.Drawing.SystemColors.Window;
            appearance197.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbLOperator2.DisplayLayout.Override.ActiveCellAppearance = appearance197;
            appearance198.BackColor = System.Drawing.SystemColors.Highlight;
            appearance198.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbLOperator2.DisplayLayout.Override.ActiveRowAppearance = appearance198;
            this.cmbLOperator2.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbLOperator2.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance199.BackColor = System.Drawing.SystemColors.Window;
            this.cmbLOperator2.DisplayLayout.Override.CardAreaAppearance = appearance199;
            appearance200.BorderColor = System.Drawing.Color.Silver;
            appearance200.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbLOperator2.DisplayLayout.Override.CellAppearance = appearance200;
            this.cmbLOperator2.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbLOperator2.DisplayLayout.Override.CellPadding = 0;
            appearance201.BackColor = System.Drawing.SystemColors.Control;
            appearance201.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance201.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance201.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance201.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbLOperator2.DisplayLayout.Override.GroupByRowAppearance = appearance201;
            appearance202.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbLOperator2.DisplayLayout.Override.HeaderAppearance = appearance202;
            this.cmbLOperator2.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbLOperator2.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance203.BackColor = System.Drawing.SystemColors.Window;
            appearance203.BorderColor = System.Drawing.Color.Silver;
            this.cmbLOperator2.DisplayLayout.Override.RowAppearance = appearance203;
            this.cmbLOperator2.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance204.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbLOperator2.DisplayLayout.Override.TemplateAddRowAppearance = appearance204;
            this.cmbLOperator2.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbLOperator2.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbLOperator2.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbLOperator2.DisplayMember = "";
            this.cmbLOperator2.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbLOperator2.DropDownWidth = 0;
            this.cmbLOperator2.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbLOperator2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbLOperator2.Location = new System.Drawing.Point(112, 104);
            this.cmbLOperator2.Name = "cmbLOperator2";
            this.cmbLOperator2.Size = new System.Drawing.Size(66, 21);
            this.cmbLOperator2.TabIndex = 19;
            this.cmbLOperator2.ValueMember = "";
            // 
            // lblCommissionRate3
            // 
            this.lblCommissionRate3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCommissionRate3.Location = new System.Drawing.Point(313, 38);
            this.lblCommissionRate3.Name = "lblCommissionRate3";
            this.lblCommissionRate3.Size = new System.Drawing.Size(90, 14);
            this.lblCommissionRate3.TabIndex = 14;
            this.lblCommissionRate3.Text = "Commission Rate";
            // 
            // lblCommissionRate1
            // 
            this.lblCommissionRate1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCommissionRate1.Location = new System.Drawing.Point(8, 138);
            this.lblCommissionRate1.Name = "lblCommissionRate1";
            this.lblCommissionRate1.Size = new System.Drawing.Size(140, 14);
            this.lblCommissionRate1.TabIndex = 10;
            this.lblCommissionRate1.Text = "Minimum Commission Rate";
            // 
            // lblconstant
            // 
            this.lblconstant.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblconstant.Location = new System.Drawing.Point(206, 38);
            this.lblconstant.Name = "lblconstant";
            this.lblconstant.Size = new System.Drawing.Size(59, 14);
            this.lblconstant.TabIndex = 7;
            this.lblconstant.Text = "Constant";
            // 
            // lblLOperator
            // 
            this.lblLOperator.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLOperator.Location = new System.Drawing.Point(112, 38);
            this.lblLOperator.Name = "lblLOperator";
            this.lblLOperator.Size = new System.Drawing.Size(88, 14);
            this.lblLOperator.TabIndex = 4;
            this.lblLOperator.Text = "Logical Operator";
            // 
            // lblCriteria
            // 
            this.lblCriteria.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCriteria.Location = new System.Drawing.Point(8, 38);
            this.lblCriteria.Name = "lblCriteria";
            this.lblCriteria.Size = new System.Drawing.Size(42, 14);
            this.lblCriteria.TabIndex = 3;
            this.lblCriteria.Text = "Criteria";
            // 
            // lblACofRule
            // 
            this.lblACofRule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblACofRule.Location = new System.Drawing.Point(32, 20);
            this.lblACofRule.Name = "lblACofRule";
            this.lblACofRule.Size = new System.Drawing.Size(128, 16);
            this.lblACofRule.TabIndex = 1;
            this.lblACofRule.Text = "Apply Criteria using Rule";
            // 
            // chkACofRule
            // 
            this.chkACofRule.Location = new System.Drawing.Point(12, 20);
            this.chkACofRule.Name = "chkACofRule";
            this.chkACofRule.Size = new System.Drawing.Size(16, 14);
            this.chkACofRule.TabIndex = 9;
            this.chkACofRule.CheckedChanged += new System.EventHandler(this.chkACofRule_CheckedChanged);
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
            this.grpParameters.Location = new System.Drawing.Point(3, 113);
            this.grpParameters.Name = "grpParameters";
            this.grpParameters.Size = new System.Drawing.Size(668, 64);
            this.grpParameters.TabIndex = 28;
            this.grpParameters.TabStop = false;
            this.grpParameters.Text = "Parameters";
            // 
            // lbldisplay
            // 
            this.lbldisplay.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbldisplay.Location = new System.Drawing.Point(362, 38);
            this.lbldisplay.Name = "lbldisplay";
            this.lbldisplay.Size = new System.Drawing.Size(217, 16);
            this.lbldisplay.TabIndex = 39;
            this.lbldisplay.Text = "lbldisplay";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(459, 11);
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
            this.label5.Location = new System.Drawing.Point(211, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(69, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "*";
            // 
            // nudCommissionRate
            // 
            this.nudCommissionRate.BackColor = System.Drawing.Color.White;
            this.nudCommissionRate.DecimalPlaces = 2;
            this.nudCommissionRate.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudCommissionRate.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nudCommissionRate.Location = new System.Drawing.Point(256, 36);
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
            appearance205.BackColor = System.Drawing.SystemColors.Window;
            appearance205.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCommissionRate.DisplayLayout.Appearance = appearance205;
            this.cmbCommissionRate.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand20.ColHeadersVisible = false;
            ultraGridColumn40.Header.VisiblePosition = 0;
            ultraGridColumn40.Hidden = true;
            ultraGridColumn41.Header.VisiblePosition = 1;
            ultraGridBand20.Columns.AddRange(new object[] {
            ultraGridColumn40,
            ultraGridColumn41});
            this.cmbCommissionRate.DisplayLayout.BandsSerializer.Add(ultraGridBand20);
            this.cmbCommissionRate.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCommissionRate.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance206.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance206.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance206.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance206.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate.DisplayLayout.GroupByBox.Appearance = appearance206;
            appearance207.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCommissionRate.DisplayLayout.GroupByBox.BandLabelAppearance = appearance207;
            this.cmbCommissionRate.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance208.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance208.BackColor2 = System.Drawing.SystemColors.Control;
            appearance208.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance208.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCommissionRate.DisplayLayout.GroupByBox.PromptAppearance = appearance208;
            this.cmbCommissionRate.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCommissionRate.DisplayLayout.MaxRowScrollRegions = 1;
            appearance209.BackColor = System.Drawing.SystemColors.Window;
            appearance209.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCommissionRate.DisplayLayout.Override.ActiveCellAppearance = appearance209;
            appearance210.BackColor = System.Drawing.SystemColors.Highlight;
            appearance210.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCommissionRate.DisplayLayout.Override.ActiveRowAppearance = appearance210;
            this.cmbCommissionRate.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCommissionRate.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance211.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate.DisplayLayout.Override.CardAreaAppearance = appearance211;
            appearance212.BorderColor = System.Drawing.Color.Silver;
            appearance212.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCommissionRate.DisplayLayout.Override.CellAppearance = appearance212;
            this.cmbCommissionRate.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCommissionRate.DisplayLayout.Override.CellPadding = 0;
            appearance213.BackColor = System.Drawing.SystemColors.Control;
            appearance213.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance213.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance213.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance213.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCommissionRate.DisplayLayout.Override.GroupByRowAppearance = appearance213;
            appearance214.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCommissionRate.DisplayLayout.Override.HeaderAppearance = appearance214;
            this.cmbCommissionRate.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCommissionRate.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance215.BackColor = System.Drawing.SystemColors.Window;
            appearance215.BorderColor = System.Drawing.Color.Silver;
            this.cmbCommissionRate.DisplayLayout.Override.RowAppearance = appearance215;
            this.cmbCommissionRate.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance216.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCommissionRate.DisplayLayout.Override.TemplateAddRowAppearance = appearance216;
            this.cmbCommissionRate.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCommissionRate.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCommissionRate.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCommissionRate.DisplayMember = "";
            this.cmbCommissionRate.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCommissionRate.DropDownWidth = 0;
            this.cmbCommissionRate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCommissionRate.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCommissionRate.Location = new System.Drawing.Point(486, 11);
            this.cmbCommissionRate.Name = "cmbCommissionRate";
            this.cmbCommissionRate.Size = new System.Drawing.Size(124, 21);
            this.cmbCommissionRate.TabIndex = 7;
            this.cmbCommissionRate.ValueMember = "";
            this.cmbCommissionRate.Visible = false;
            // 
            // cmbCalculation
            // 
            this.cmbCalculation.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance217.BackColor = System.Drawing.SystemColors.Window;
            appearance217.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCalculation.DisplayLayout.Appearance = appearance217;
            this.cmbCalculation.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand21.ColHeadersVisible = false;
            ultraGridColumn42.Header.VisiblePosition = 0;
            ultraGridColumn42.Hidden = true;
            ultraGridColumn43.Header.VisiblePosition = 1;
            ultraGridBand21.Columns.AddRange(new object[] {
            ultraGridColumn42,
            ultraGridColumn43});
            this.cmbCalculation.DisplayLayout.BandsSerializer.Add(ultraGridBand21);
            this.cmbCalculation.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCalculation.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance218.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance218.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance218.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance218.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCalculation.DisplayLayout.GroupByBox.Appearance = appearance218;
            appearance219.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCalculation.DisplayLayout.GroupByBox.BandLabelAppearance = appearance219;
            this.cmbCalculation.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance220.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance220.BackColor2 = System.Drawing.SystemColors.Control;
            appearance220.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance220.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCalculation.DisplayLayout.GroupByBox.PromptAppearance = appearance220;
            this.cmbCalculation.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCalculation.DisplayLayout.MaxRowScrollRegions = 1;
            appearance221.BackColor = System.Drawing.SystemColors.Window;
            appearance221.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCalculation.DisplayLayout.Override.ActiveCellAppearance = appearance221;
            appearance222.BackColor = System.Drawing.SystemColors.Highlight;
            appearance222.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCalculation.DisplayLayout.Override.ActiveRowAppearance = appearance222;
            this.cmbCalculation.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCalculation.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance223.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCalculation.DisplayLayout.Override.CardAreaAppearance = appearance223;
            appearance224.BorderColor = System.Drawing.Color.Silver;
            appearance224.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCalculation.DisplayLayout.Override.CellAppearance = appearance224;
            this.cmbCalculation.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCalculation.DisplayLayout.Override.CellPadding = 0;
            appearance225.BackColor = System.Drawing.SystemColors.Control;
            appearance225.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance225.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance225.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance225.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCalculation.DisplayLayout.Override.GroupByRowAppearance = appearance225;
            appearance226.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCalculation.DisplayLayout.Override.HeaderAppearance = appearance226;
            this.cmbCalculation.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCalculation.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance227.BackColor = System.Drawing.SystemColors.Window;
            appearance227.BorderColor = System.Drawing.Color.Silver;
            this.cmbCalculation.DisplayLayout.Override.RowAppearance = appearance227;
            this.cmbCalculation.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance228.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCalculation.DisplayLayout.Override.TemplateAddRowAppearance = appearance228;
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
            this.cmbCalculation.ValueChanged += new System.EventHandler(this.cmbCalculation_ValueChanged);
            // 
            // cmbCurrencyUsed
            // 
            this.cmbCurrencyUsed.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance229.BackColor = System.Drawing.SystemColors.Window;
            appearance229.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCurrencyUsed.DisplayLayout.Appearance = appearance229;
            this.cmbCurrencyUsed.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand22.ColHeadersVisible = false;
            ultraGridColumn44.Header.VisiblePosition = 0;
            ultraGridColumn44.Hidden = true;
            ultraGridColumn45.Header.VisiblePosition = 1;
            ultraGridColumn45.Hidden = true;
            ultraGridColumn46.Header.VisiblePosition = 2;
            ultraGridBand22.Columns.AddRange(new object[] {
            ultraGridColumn44,
            ultraGridColumn45,
            ultraGridColumn46});
            this.cmbCurrencyUsed.DisplayLayout.BandsSerializer.Add(ultraGridBand22);
            this.cmbCurrencyUsed.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCurrencyUsed.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance230.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance230.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance230.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance230.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCurrencyUsed.DisplayLayout.GroupByBox.Appearance = appearance230;
            appearance231.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCurrencyUsed.DisplayLayout.GroupByBox.BandLabelAppearance = appearance231;
            this.cmbCurrencyUsed.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance232.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance232.BackColor2 = System.Drawing.SystemColors.Control;
            appearance232.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance232.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCurrencyUsed.DisplayLayout.GroupByBox.PromptAppearance = appearance232;
            this.cmbCurrencyUsed.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCurrencyUsed.DisplayLayout.MaxRowScrollRegions = 1;
            appearance233.BackColor = System.Drawing.SystemColors.Window;
            appearance233.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCurrencyUsed.DisplayLayout.Override.ActiveCellAppearance = appearance233;
            appearance234.BackColor = System.Drawing.SystemColors.Highlight;
            appearance234.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCurrencyUsed.DisplayLayout.Override.ActiveRowAppearance = appearance234;
            this.cmbCurrencyUsed.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCurrencyUsed.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance235.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCurrencyUsed.DisplayLayout.Override.CardAreaAppearance = appearance235;
            appearance236.BorderColor = System.Drawing.Color.Silver;
            appearance236.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCurrencyUsed.DisplayLayout.Override.CellAppearance = appearance236;
            this.cmbCurrencyUsed.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCurrencyUsed.DisplayLayout.Override.CellPadding = 0;
            appearance237.BackColor = System.Drawing.SystemColors.Control;
            appearance237.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance237.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance237.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance237.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCurrencyUsed.DisplayLayout.Override.GroupByRowAppearance = appearance237;
            appearance238.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCurrencyUsed.DisplayLayout.Override.HeaderAppearance = appearance238;
            this.cmbCurrencyUsed.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCurrencyUsed.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance239.BackColor = System.Drawing.SystemColors.Window;
            appearance239.BorderColor = System.Drawing.Color.Silver;
            this.cmbCurrencyUsed.DisplayLayout.Override.RowAppearance = appearance239;
            this.cmbCurrencyUsed.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance240.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCurrencyUsed.DisplayLayout.Override.TemplateAddRowAppearance = appearance240;
            this.cmbCurrencyUsed.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCurrencyUsed.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCurrencyUsed.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCurrencyUsed.DisplayMember = "";
            this.cmbCurrencyUsed.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCurrencyUsed.DropDownWidth = 0;
            this.cmbCurrencyUsed.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCurrencyUsed.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbCurrencyUsed.Location = new System.Drawing.Point(131, 36);
            this.cmbCurrencyUsed.Name = "cmbCurrencyUsed";
            this.cmbCurrencyUsed.Size = new System.Drawing.Size(108, 21);
            this.cmbCurrencyUsed.TabIndex = 6;
            this.cmbCurrencyUsed.ValueMember = "";
            // 
            // lblCommissionRate
            // 
            this.lblCommissionRate.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCommissionRate.Location = new System.Drawing.Point(363, 11);
            this.lblCommissionRate.Name = "lblCommissionRate";
            this.lblCommissionRate.Size = new System.Drawing.Size(90, 16);
            this.lblCommissionRate.TabIndex = 2;
            this.lblCommissionRate.Text = "Commission Rate";
            this.lblCommissionRate.Visible = false;
            // 
            // lblCurrencyUsed
            // 
            this.lblCurrencyUsed.AutoSize = true;
            this.lblCurrencyUsed.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCurrencyUsed.Location = new System.Drawing.Point(133, 18);
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
            // grpRule
            // 
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
            this.grpRule.Location = new System.Drawing.Point(3, 2);
            this.grpRule.Name = "grpRule";
            this.grpRule.Size = new System.Drawing.Size(668, 110);
            this.grpRule.TabIndex = 27;
            this.grpRule.TabStop = false;
            this.grpRule.Text = "Rule";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(315, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(80, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 9);
            this.label1.TabIndex = 36;
            this.label1.Text = "*";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(40, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 9);
            this.label3.TabIndex = 35;
            this.label3.Text = "*";
            // 
            // cmbApplyRuleto
            // 
            this.cmbApplyRuleto.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance241.BackColor = System.Drawing.SystemColors.Window;
            appearance241.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbApplyRuleto.DisplayLayout.Appearance = appearance241;
            this.cmbApplyRuleto.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand23.ColHeadersVisible = false;
            ultraGridColumn47.Header.VisiblePosition = 0;
            ultraGridColumn47.Hidden = true;
            ultraGridColumn48.Header.VisiblePosition = 1;
            ultraGridBand23.Columns.AddRange(new object[] {
            ultraGridColumn47,
            ultraGridColumn48});
            this.cmbApplyRuleto.DisplayLayout.BandsSerializer.Add(ultraGridBand23);
            this.cmbApplyRuleto.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbApplyRuleto.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance242.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance242.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance242.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance242.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbApplyRuleto.DisplayLayout.GroupByBox.Appearance = appearance242;
            appearance243.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbApplyRuleto.DisplayLayout.GroupByBox.BandLabelAppearance = appearance243;
            this.cmbApplyRuleto.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance244.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance244.BackColor2 = System.Drawing.SystemColors.Control;
            appearance244.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance244.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbApplyRuleto.DisplayLayout.GroupByBox.PromptAppearance = appearance244;
            this.cmbApplyRuleto.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbApplyRuleto.DisplayLayout.MaxRowScrollRegions = 1;
            appearance245.BackColor = System.Drawing.SystemColors.Window;
            appearance245.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbApplyRuleto.DisplayLayout.Override.ActiveCellAppearance = appearance245;
            appearance246.BackColor = System.Drawing.SystemColors.Highlight;
            appearance246.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbApplyRuleto.DisplayLayout.Override.ActiveRowAppearance = appearance246;
            this.cmbApplyRuleto.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbApplyRuleto.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance247.BackColor = System.Drawing.SystemColors.Window;
            this.cmbApplyRuleto.DisplayLayout.Override.CardAreaAppearance = appearance247;
            appearance248.BorderColor = System.Drawing.Color.Silver;
            appearance248.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbApplyRuleto.DisplayLayout.Override.CellAppearance = appearance248;
            this.cmbApplyRuleto.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbApplyRuleto.DisplayLayout.Override.CellPadding = 0;
            appearance249.BackColor = System.Drawing.SystemColors.Control;
            appearance249.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance249.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance249.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance249.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbApplyRuleto.DisplayLayout.Override.GroupByRowAppearance = appearance249;
            appearance250.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbApplyRuleto.DisplayLayout.Override.HeaderAppearance = appearance250;
            this.cmbApplyRuleto.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbApplyRuleto.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance251.BackColor = System.Drawing.SystemColors.Window;
            appearance251.BorderColor = System.Drawing.Color.Silver;
            this.cmbApplyRuleto.DisplayLayout.Override.RowAppearance = appearance251;
            this.cmbApplyRuleto.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance252.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbApplyRuleto.DisplayLayout.Override.TemplateAddRowAppearance = appearance252;
            this.cmbApplyRuleto.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbApplyRuleto.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbApplyRuleto.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbApplyRuleto.DisplayMember = "";
            this.cmbApplyRuleto.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbApplyRuleto.DropDownWidth = 0;
            this.cmbApplyRuleto.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbApplyRuleto.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbApplyRuleto.Location = new System.Drawing.Point(329, 39);
            this.cmbApplyRuleto.Name = "cmbApplyRuleto";
            this.cmbApplyRuleto.Size = new System.Drawing.Size(138, 21);
            this.cmbApplyRuleto.TabIndex = 3;
            this.cmbApplyRuleto.ValueMember = "";
            // 
            // txtNamefRule
            // 
            this.txtNamefRule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtNamefRule.Location = new System.Drawing.Point(109, 39);
            this.txtNamefRule.MaxLength = 15;
            this.txtNamefRule.Name = "txtNamefRule";
            this.txtNamefRule.Size = new System.Drawing.Size(132, 21);
            this.txtNamefRule.TabIndex = 2;
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
            this.lblApplyRuleTo.AutoSize = true;
            this.lblApplyRuleTo.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblApplyRuleTo.Location = new System.Drawing.Point(245, 41);
            this.lblApplyRuleTo.Name = "lblApplyRuleTo";
            this.lblApplyRuleTo.Size = new System.Drawing.Size(73, 13);
            this.lblApplyRuleTo.TabIndex = 1;
            this.lblApplyRuleTo.Text = "Apply Rule To";
            // 
            // lblNameofRule
            // 
            this.lblNameofRule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblNameofRule.Location = new System.Drawing.Point(10, 41);
            this.lblNameofRule.Name = "lblNameofRule";
            this.lblNameofRule.Size = new System.Drawing.Size(72, 16);
            this.lblNameofRule.TabIndex = 0;
            this.lblNameofRule.Text = "Name of Rule";
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDescription.Location = new System.Drawing.Point(109, 64);
            this.txtDescription.MaxLength = 50;
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(360, 40);
            this.txtDescription.TabIndex = 4;
            // 
            // lblAUEC
            // 
            this.lblAUEC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAUEC.Location = new System.Drawing.Point(10, 16);
            this.lblAUEC.Name = "lblAUEC";
            this.lblAUEC.Size = new System.Drawing.Size(36, 14);
            this.lblAUEC.TabIndex = 9;
            this.lblAUEC.Text = "AUEC";
            // 
            // cmbAUEC
            // 
            this.cmbAUEC.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance253.BackColor = System.Drawing.SystemColors.Window;
            appearance253.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAUEC.DisplayLayout.Appearance = appearance253;
            this.cmbAUEC.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand24.ColHeadersVisible = false;
            ultraGridColumn49.Header.VisiblePosition = 0;
            ultraGridColumn49.Hidden = true;
            ultraGridColumn50.Header.VisiblePosition = 1;
            ultraGridBand24.Columns.AddRange(new object[] {
            ultraGridColumn49,
            ultraGridColumn50});
            this.cmbAUEC.DisplayLayout.BandsSerializer.Add(ultraGridBand24);
            this.cmbAUEC.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAUEC.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance254.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance254.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance254.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance254.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.GroupByBox.Appearance = appearance254;
            appearance255.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAUEC.DisplayLayout.GroupByBox.BandLabelAppearance = appearance255;
            this.cmbAUEC.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance256.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance256.BackColor2 = System.Drawing.SystemColors.Control;
            appearance256.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance256.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAUEC.DisplayLayout.GroupByBox.PromptAppearance = appearance256;
            this.cmbAUEC.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAUEC.DisplayLayout.MaxRowScrollRegions = 1;
            appearance257.BackColor = System.Drawing.SystemColors.Window;
            appearance257.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAUEC.DisplayLayout.Override.ActiveCellAppearance = appearance257;
            appearance258.BackColor = System.Drawing.SystemColors.Highlight;
            appearance258.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAUEC.DisplayLayout.Override.ActiveRowAppearance = appearance258;
            this.cmbAUEC.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAUEC.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance259.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.Override.CardAreaAppearance = appearance259;
            appearance260.BorderColor = System.Drawing.Color.Silver;
            appearance260.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAUEC.DisplayLayout.Override.CellAppearance = appearance260;
            this.cmbAUEC.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAUEC.DisplayLayout.Override.CellPadding = 0;
            appearance261.BackColor = System.Drawing.SystemColors.Control;
            appearance261.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance261.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance261.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance261.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.Override.GroupByRowAppearance = appearance261;
            appearance262.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbAUEC.DisplayLayout.Override.HeaderAppearance = appearance262;
            this.cmbAUEC.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAUEC.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance263.BackColor = System.Drawing.SystemColors.Window;
            appearance263.BorderColor = System.Drawing.Color.Silver;
            this.cmbAUEC.DisplayLayout.Override.RowAppearance = appearance263;
            this.cmbAUEC.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance264.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAUEC.DisplayLayout.Override.TemplateAddRowAppearance = appearance264;
            this.cmbAUEC.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAUEC.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAUEC.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAUEC.DisplayMember = "";
            this.cmbAUEC.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAUEC.DropDownWidth = 0;
            this.cmbAUEC.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbAUEC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbAUEC.Location = new System.Drawing.Point(67, 14);
            this.cmbAUEC.Name = "cmbAUEC";
            this.cmbAUEC.Size = new System.Drawing.Size(174, 21);
            this.cmbAUEC.TabIndex = 1;
            this.cmbAUEC.ValueMember = "";
            // 
            // CreateCommissionRules
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(675, 373);
            this.Controls.Add(this.grpCriteria);
            this.Controls.Add(this.grpParameters);
            this.Controls.Add(this.grpRule);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateCommissionRules";
            this.Text = "Create Commission Rules";
            this.Load += new System.EventHandler(this.CreateCommissionRules_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.grpCriteria.ResumeLayout(false);
            this.grpCriteria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numConstant2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConstant1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConstant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCriteria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommissionRate1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommissionRate2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommissionRate3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLOperator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLOperator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLOperator2)).EndInit();
            this.grpParameters.ResumeLayout(false);
            this.grpParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCommissionRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommissionRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCalculation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrencyUsed)).EndInit();
            this.grpRule.ResumeLayout(false);
            this.grpRule.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbApplyRuleto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAUEC)).EndInit();
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

        private void cmbLOperator2_GotFocus(object sender, System.EventArgs e)
        {
            cmbLOperator2.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbLOperator2_LostFocus(object sender, System.EventArgs e)
        {
            cmbLOperator2.Appearance.BackColor = Color.White;
        }
        private void cmbLOperator1_GotFocus(object sender, System.EventArgs e)
        {
            cmbLOperator1.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbLOperator1_LostFocus(object sender, System.EventArgs e)
        {
            cmbLOperator1.Appearance.BackColor = Color.White;
        }
        private void cmbLOperator_GotFocus(object sender, System.EventArgs e)
        {
            cmbLOperator.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbLOperator_LostFocus(object sender, System.EventArgs e)
        {
            cmbLOperator.Appearance.BackColor = Color.White;
        }

        private void cmbCommissionRate2_GotFocus(object sender, System.EventArgs e)
        {
            cmbCommissionRate2.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbCommissionRate2_LostFocus(object sender, System.EventArgs e)
        {
            cmbCommissionRate2.Appearance.BackColor = Color.White;
        }
        private void cmbCommissionRate1_GotFocus(object sender, System.EventArgs e)
        {
            cmbCommissionRate1.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbCommissionRate1_LostFocus(object sender, System.EventArgs e)
        {
            cmbCommissionRate1.Appearance.BackColor = Color.White;
        }
        private void cmbCriteria_GotFocus(object sender, System.EventArgs e)
        {
            cmbCriteria.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbCriteria_LostFocus(object sender, System.EventArgs e)
        {
            cmbCriteria.Appearance.BackColor = Color.White;
        }
        private void cmbCommissionRate3_GotFocus(object sender, System.EventArgs e)
        {
            cmbCommissionRate3.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void cmbCommissionRate3_LostFocus(object sender, System.EventArgs e)
        {
            cmbCommissionRate3.Appearance.BackColor = Color.White;
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
        #endregion

        public AUECCommissionRule auecCommissionRuleproperty
        {

            get
            {
                AUECCommissionRule auecCommissionRule = new AUECCommissionRule();
                auecCommissionRule = GetCommissionRuleDetails(auecCommissionRule);
                return auecCommissionRule;
            }
            set
            {
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


        public CommissionRuleCriteriaold commissionRuleCriteriaproperty
        {
            get
            {
                CommissionRuleCriteriaold commissionRuleCriteria = new CommissionRuleCriteriaold();

                commissionRuleCriteria = GetCommissionRuleCriteriaDetails(commissionRuleCriteria);
                return commissionRuleCriteria;
            }
            set { SetCommissionRuleCriteriaDetails(value); }
        }
        #region properties
        private void SetCommissionRuleDetails(AUECCommissionRule auecCommissionRule)
        {
            cmbAUEC.Value = int.Parse(auecCommissionRule.AUECID.ToString());
            txtNamefRule.Text = auecCommissionRule.RuleName;
            cmbApplyRuleto.Value = int.Parse(auecCommissionRule.ApplyRuleID.ToString());
            txtDescription.Text = auecCommissionRule.RuleDescription;
            cmbCalculation.Value = int.Parse(auecCommissionRule.CalculationID.ToString());
            cmbCurrencyUsed.Value = int.Parse(auecCommissionRule.CurrencyID.ToString());
            cmbCommissionRate.Value = int.Parse(auecCommissionRule.CommissionRateID.ToString());
            nudCommissionRate.Value = decimal.Parse(auecCommissionRule.Commission.ToString());
            chkACofRule.Checked = (int.Parse(auecCommissionRule.ApplyCriteria.ToString()) == 1 ? true : false);
        }

        private bool _errorStatus = false;
        private AUECCommissionRule GetCommissionRuleDetails(AUECCommissionRule aueccommissionRule)
        {
            //AUECCommissionRule aueccommissionRule = null;

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
                _errorStatus = true;
                aueccommissionRule = null;
                return aueccommissionRule;
            }
            else if (txtNamefRule.Text.Trim() == "")
            {
                errorProvider1.SetError(txtNamefRule, "Please enter Name of Rule !");
                txtNamefRule.Focus();
                _errorStatus = true;
                aueccommissionRule = null;
                return aueccommissionRule;
            }
            else if (int.Parse(cmbApplyRuleto.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbApplyRuleto, "Please select Rule applied");
                cmbApplyRuleto.Focus();
                _errorStatus = true;
                aueccommissionRule = null;
                return aueccommissionRule;
            }
            else if (txtDescription.Text.Trim() == " ")
            {
                errorProvider1.SetError(txtDescription, "Please enter Description");
                txtDescription.Focus();
                _errorStatus = true;
                aueccommissionRule = null;
                return aueccommissionRule;
            }
            else if (int.Parse(cmbCalculation.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbCalculation, "Please select Value");
                cmbCalculation.Focus();
                _errorStatus = true;
                aueccommissionRule = null;
                return aueccommissionRule;
            }
            else if (int.Parse(cmbCurrencyUsed.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbCurrencyUsed, "Please enter Currency");
                cmbCurrencyUsed.Focus();
                _errorStatus = true;
                aueccommissionRule = null;
                return aueccommissionRule;
            }
            //else if(int.Parse(cmbCommissionRate.Value.ToString())==int.MinValue)
            //{
            //    errorProvider1.SetError(cmbCommissionRate,"Please enter Value of CommissionRate");
            //    cmbCommissionRate.Focus();
            //    _errorStatus = true;
            //    aueccommissionRule = null;
            //    return aueccommissionRule;
            //}
            else if (decimal.Parse(nudCommissionRate.Value.ToString()) == decimal.MinValue)
            {
                errorProvider1.SetError(nudCommissionRate, "Enter Value");
                nudCommissionRate.Focus();
                _errorStatus = true;
                aueccommissionRule = null;
                return aueccommissionRule;
            }

            else
            {
                _errorStatus = false;
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
            }
            return aueccommissionRule;

        }

        private void SetCommissionCriteriaDetails(CommissionCriteria commissionCriteria)
        {


            cmbCriteria.Value = int.Parse(commissionCriteria.CommissionCalculationID_FK.ToString());

            nudCommissionRate4.Value = decimal.Parse(commissionCriteria.MinimumCommissionRate.ToString());
        }


        private CommissionCriteria GetCommissionCriteriaDetails(CommissionCriteria commissioncriteria)
        {
            commissioncriteria = null;
            errorProvider1.SetError(cmbCriteria, "");
            errorProvider1.SetError(nudCommissionRate4, "");

            if (chkACofRule.Checked == true)
            {
                if (int.Parse(cmbCriteria.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbCriteria, "Please select Criteria!");
                    cmbCriteria.Focus();
                    _errorStatus = true;
                    commissioncriteria = null;
                    return commissioncriteria;
                }
                else if (decimal.Parse(nudCommissionRate.Value.ToString()) == decimal.MinValue)
                {
                    errorProvider1.SetError(nudCommissionRate, "Please select Value");
                    nudCommissionRate.Focus();
                    _errorStatus = true;
                    commissioncriteria = null;
                    return commissioncriteria;
                }
                else
                {
                    _errorStatus = false;
                    commissioncriteria = new CommissionCriteria();
                    commissioncriteria.CommissionCalculationID_FK = int.Parse(cmbCriteria.Value.ToString());
                    commissioncriteria.MinimumCommissionRate = float.Parse(nudCommissionRate4.Value.ToString());
                }
            }
            return commissioncriteria;
        }
        private void SetCommissionRuleCriteriaDetails(CommissionRuleCriteriaold commissionRuleCriteria)
        {

            cmbLOperator.Value = int.Parse(commissionRuleCriteria.OperatorID_FK1.ToString());
            cmbLOperator1.Value = int.Parse(commissionRuleCriteria.OperatorID_FK2.ToString());
            cmbLOperator2.Value = int.Parse(commissionRuleCriteria.OperatorID_FK3.ToString());
            numConstant.Value = int.Parse(commissionRuleCriteria.Value1.ToString());
            numConstant1.Value = int.Parse(commissionRuleCriteria.Value2.ToString());
            numConstant2.Value = int.Parse(commissionRuleCriteria.Value3.ToString());
            cmbCommissionRate1.Value = int.Parse(commissionRuleCriteria.CommissionRateID_FK1.ToString());
            cmbCommissionRate2.Value = int.Parse(commissionRuleCriteria.CommissionRateID_FK2.ToString());
            cmbCommissionRate3.Value = int.Parse(commissionRuleCriteria.CommissionRateID_FK3.ToString());
            nudCommissionRate1.Value = decimal.Parse(commissionRuleCriteria.CommisionRate1.ToString());
            nudCommissionRate2.Value = decimal.Parse(commissionRuleCriteria.CommisionRate2.ToString());
            nudCommissionRate3.Value = decimal.Parse(commissionRuleCriteria.CommisionRate3.ToString());
        }

        private CommissionRuleCriteriaold GetCommissionRuleCriteriaDetails(CommissionRuleCriteriaold commissionRuleCriteria)
        {
            System.Text.RegularExpressions.Regex rgnumber = new System.Text.RegularExpressions.Regex(@"^\d+$");
            errorProvider1.SetError(cmbLOperator, "");
            errorProvider1.SetError(cmbLOperator1, "");
            errorProvider1.SetError(cmbLOperator2, "");
            errorProvider1.SetError(numConstant, "");
            errorProvider1.SetError(numConstant1, "");
            errorProvider1.SetError(numConstant2, "");
            errorProvider1.SetError(cmbCommissionRate1, "");
            errorProvider1.SetError(cmbCommissionRate2, "");
            errorProvider1.SetError(cmbCommissionRate3, "");
            errorProvider1.SetError(nudCommissionRate1, "");
            errorProvider1.SetError(nudCommissionRate2, "");
            errorProvider1.SetError(nudCommissionRate3, "");

            if (chkACofRule.Checked == true)
            {
                if (int.Parse(cmbCriteria.Value.ToString()) != int.MinValue && int.Parse(cmbLOperator.Value.ToString()) == int.MinValue && int.Parse(cmbLOperator1.Value.ToString()) == int.MinValue && int.Parse(cmbLOperator2.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbLOperator, "Please select atleast one Operator");
                    cmbLOperator.Focus();
                    commissionRuleCriteria = null;
                    return commissionRuleCriteria;
                }
                if (int.Parse(cmbLOperator.Value.ToString()) != int.MinValue || int.Parse(numConstant.Value.ToString()) > ZERO || int.Parse(cmbCommissionRate1.Value.ToString()) != int.MinValue /*||	int.Parse(nudCommissionRate1.Value.ToString()) > ZERO*/)
                {
                    if (int.Parse(cmbLOperator.Value.ToString()) == int.MinValue)
                    {
                        errorProvider1.SetError(cmbLOperator, "Please select Operator");
                        cmbLOperator.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else if (!rgnumber.IsMatch(numConstant.Value.ToString()))
                    {
                        errorProvider1.SetError(numConstant, "Please Enter integer value");
                        numConstant.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else if (int.Parse(numConstant.Value.ToString()) == ZERO)
                    {
                        errorProvider1.SetError(numConstant, "Please select value");
                        numConstant.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else if (int.Parse(cmbCommissionRate1.Value.ToString()) == int.MinValue)
                    {
                        errorProvider1.SetError(cmbCommissionRate1, "Please select Value");
                        cmbCommissionRate1.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else if (double.Parse(nudCommissionRate1.Value.ToString()) == ZERO)
                    {
                        errorProvider1.SetError(nudCommissionRate1, "Please select Value");
                        nudCommissionRate1.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else
                    {
                        _errorStatus = false;
                        commissionRuleCriteria = new CommissionRuleCriteriaold();
                        commissionRuleCriteria.OperatorID_FK1 = int.Parse(cmbLOperator.Value.ToString());
                        commissionRuleCriteria.Value1 = int.Parse(numConstant.Value.ToString());
                        commissionRuleCriteria.CommissionRateID_FK1 = int.Parse(cmbCommissionRate1.Value.ToString());
                        commissionRuleCriteria.CommisionRate1 = float.Parse(nudCommissionRate1.Value.ToString());
                    }
                }

                if (int.Parse(cmbLOperator1.Value.ToString()) != int.MinValue || int.Parse(numConstant1.Value.ToString()) > ZERO || int.Parse(cmbCommissionRate2.Value.ToString()) != int.MinValue /*|| int.Parse(nudCommissionRate2.Value.ToString()) > ZERO*/)
                {
                    if (int.Parse(cmbLOperator1.Value.ToString()) == int.MinValue)
                    {
                        errorProvider1.SetError(cmbLOperator1, "Please select Operator");
                        cmbLOperator1.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else if (!rgnumber.IsMatch(numConstant1.Value.ToString()))
                    {
                        errorProvider1.SetError(numConstant, "Please Enter integer value");
                        numConstant1.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else if (int.Parse(numConstant1.Value.ToString()) == ZERO)
                    {
                        errorProvider1.SetError(numConstant1, "Enter Value");
                        numConstant1.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else if (int.Parse(cmbCommissionRate2.Value.ToString()) == int.MinValue)
                    {
                        errorProvider1.SetError(cmbCommissionRate2, "Please select Value");
                        cmbCommissionRate2.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else if (double.Parse(nudCommissionRate2.Value.ToString()) == ZERO)
                    {
                        errorProvider1.SetError(nudCommissionRate2, "Please select Value");
                        nudCommissionRate2.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else
                    {
                        _errorStatus = false;
                        if (commissionRuleCriteria == null)
                        {
                            commissionRuleCriteria = new CommissionRuleCriteriaold();
                        }
                        commissionRuleCriteria.OperatorID_FK2 = int.Parse(cmbLOperator1.Value.ToString());
                        commissionRuleCriteria.Value2 = int.Parse(numConstant1.Value.ToString());
                        commissionRuleCriteria.CommissionRateID_FK2 = int.Parse(cmbCommissionRate2.Value.ToString());
                        commissionRuleCriteria.CommisionRate2 = float.Parse(nudCommissionRate2.Value.ToString());
                    }
                }
                if (int.Parse(cmbLOperator2.Value.ToString()) != int.MinValue || int.Parse(numConstant2.Value.ToString()) > 0 || int.Parse(cmbCommissionRate3.Value.ToString()) != int.MinValue /*|| int.Parse(nudCommissionRate3.Value.ToString()) > 0*/)
                {
                    if (int.Parse(cmbLOperator2.Value.ToString()) == int.MinValue)
                    {
                        errorProvider1.SetError(cmbLOperator2, "Please select Operator");
                        cmbLOperator2.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else if (!rgnumber.IsMatch(numConstant2.Value.ToString()))
                    {
                        errorProvider1.SetError(numConstant2, "Please Enter integer value");
                        numConstant2.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else if (int.Parse(numConstant2.Value.ToString()) == ZERO)
                    {
                        errorProvider1.SetError(numConstant2, "Enter Value");
                        numConstant2.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else if (int.Parse(cmbCommissionRate3.Value.ToString()) == int.MinValue)
                    {
                        errorProvider1.SetError(cmbCommissionRate3, "Please select Value");
                        cmbCommissionRate3.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else if (double.Parse(nudCommissionRate3.Value.ToString()) == ZERO)
                    {
                        errorProvider1.SetError(nudCommissionRate3, "Please select Value");
                        nudCommissionRate3.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                    else
                    {
                        _errorStatus = false;
                        if (commissionRuleCriteria == null)
                        {
                            commissionRuleCriteria = new CommissionRuleCriteriaold();
                        }
                        commissionRuleCriteria.OperatorID_FK3 = int.Parse(cmbLOperator2.Value.ToString());
                        commissionRuleCriteria.Value3 = int.Parse(numConstant2.Value.ToString());
                        commissionRuleCriteria.CommissionRateID_FK3 = int.Parse(cmbCommissionRate3.Value.ToString());
                        commissionRuleCriteria.CommisionRate3 = float.Parse(nudCommissionRate3.Value.ToString());
                    }
                }
                if (int.Parse(cmbLOperator.Value.ToString()) != int.MinValue && (int.Parse(cmbLOperator1.Value.ToString()) != int.MinValue) && int.Parse(cmbLOperator2.Value.ToString()) != int.MinValue)
                {
                    if (cmbLOperator.Text != cmbLOperator1.Text || cmbLOperator1.Text != cmbLOperator2.Text || cmbLOperator.Text != cmbLOperator2.Text)
                    {
                        MessageBox.Show("All Logical Operator should be of same type !", "Nervana Information");
                        errorProvider1.SetError(cmbLOperator, "Operator Should be same type");
                        cmbLOperator.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                }
                else if (int.Parse(cmbLOperator.Value.ToString()) != int.MinValue && (int.Parse(cmbLOperator1.Value.ToString()) != int.MinValue))
                {
                    if (cmbLOperator.Text != cmbLOperator1.Text)
                    {
                        MessageBox.Show("All Logical Operator should be of same type !", "Nervana Information");
                        errorProvider1.SetError(cmbLOperator, " Operator Should be of same type");
                        cmbLOperator.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                }
                else if (int.Parse(cmbLOperator.Value.ToString()) != int.MinValue && (int.Parse(cmbLOperator2.Value.ToString()) != int.MinValue))
                {
                    if (cmbLOperator.Text != cmbLOperator2.Text)
                    {
                        MessageBox.Show("All Logical Operator should be of same type !", "Nervana Information");
                        errorProvider1.SetError(cmbLOperator, " Operator Should be of same type");
                        cmbLOperator.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                }
                else if (int.Parse(cmbLOperator1.Value.ToString()) != int.MinValue && (int.Parse(cmbLOperator2.Value.ToString()) != int.MinValue))
                {
                    if (cmbLOperator1.Text != cmbLOperator2.Text)
                    {
                        MessageBox.Show("All Logical Operator should be of same type !", "Nervana Information");
                        errorProvider1.SetError(cmbLOperator, "Operator Should be same type");
                        cmbLOperator.Focus();
                        commissionRuleCriteria = null;
                        _errorStatus = true;
                        return commissionRuleCriteria;
                    }
                }
            }
            return commissionRuleCriteria;
        }


        #endregion

        private Prana.Admin.BLL.AllCommissionRules allCommissionRules = new AllCommissionRules();
        public AllCommissionRules CurrentAllCommissionRules
        {
            get
            {
                return allCommissionRules;
            }
            set
            {
                if (value != null)
                {
                    allCommissionRules = value;
                }
            }
        }

        #region BindForEdit
        public void BindForEdit()
        {
            if (_ruleEdit != null)
            {
                cmbAUEC.Value = int.Parse(_ruleEdit.AUECID.ToString());
                txtNamefRule.Text = _ruleEdit.RuleName;
                cmbApplyRuleto.Value = int.Parse(_ruleEdit.ApplyRuleID.ToString());
                txtDescription.Text = _ruleEdit.RuleDescription;
                cmbCalculation.Value = int.Parse(_ruleEdit.CalculationID.ToString());
                cmbCurrencyUsed.Value = int.Parse(_ruleEdit.CurrencyID.ToString());
                cmbCommissionRate.Value = int.Parse(_ruleEdit.CommissionRateID.ToString());
                nudCommissionRate.Value = (decimal.Parse(_ruleEdit.Commission.ToString()));
                chkACofRule.Checked = (int.Parse(_ruleEdit.ApplyCriteria.ToString()) == 1 ? true : false);
                cmbCriteria.Value = int.Parse(_ruleEdit.CommissionCalculationID_FK.ToString());
                nudCommissionRate4.Value = decimal.Parse(_ruleEdit.MinimumCommissionRate.ToString());
                cmbLOperator.Value = int.Parse(_ruleEdit.OperatorID_FK1.ToString());
                cmbLOperator1.Value = int.Parse(_ruleEdit.OperatorID_FK2.ToString());
                cmbLOperator2.Value = int.Parse(_ruleEdit.OperatorID_FK3.ToString());
                numConstant.Value = int.Parse(_ruleEdit.Value1.ToString());
                numConstant1.Value = int.Parse(_ruleEdit.Value2.ToString());
                numConstant2.Value = int.Parse(_ruleEdit.Value3.ToString());
                cmbCommissionRate1.Value = int.Parse(_ruleEdit.CommissionRateID_FK1.ToString());
                cmbCommissionRate2.Value = int.Parse(_ruleEdit.CommissionRateID_FK2.ToString());
                cmbCommissionRate3.Value = int.Parse(_ruleEdit.CommissionRateID_FK3.ToString());
                nudCommissionRate1.Value = (decimal.Parse(_ruleEdit.CommisionRate1.ToString()));
                nudCommissionRate2.Value = (decimal.Parse(_ruleEdit.CommisionRate2.ToString()));
                nudCommissionRate3.Value = (decimal.Parse(_ruleEdit.CommisionRate3.ToString()));
                ResetControls();

            }
        }
        #endregion
        #region BindForView
        public void BindForView()
        {

            if (_ruleView != null)
            {
                btnSave.Hide();
                cmbAUEC.Enabled = false;
                cmbApplyRuleto.Enabled = false;
                cmbCalculation.Enabled = false;
                cmbCommissionRate.Enabled = false;
                cmbCommissionRate1.Enabled = false;
                cmbCommissionRate2.Enabled = false;
                cmbCommissionRate3.Enabled = false;
                cmbCriteria.Enabled = false;
                cmbCurrencyUsed.Enabled = false;
                cmbLOperator.Enabled = false;
                cmbLOperator1.Enabled = false;
                cmbLOperator2.Enabled = false;
                txtDescription.Enabled = false;
                txtNamefRule.Enabled = false;
                nudCommissionRate.Enabled = false;
                nudCommissionRate1.Enabled = false;
                nudCommissionRate2.Enabled = false;
                nudCommissionRate3.Enabled = false;
                nudCommissionRate4.Enabled = false;

                numConstant.Enabled = false;
                numConstant1.Enabled = false;
                numConstant2.Enabled = false;
                chkACofRule.Enabled = false;
                nudCommissionRate4.Enabled = false;




                cmbAUEC.Value = int.Parse(_ruleView.AUECID.ToString());
                txtNamefRule.Text = _ruleView.RuleName;
                cmbApplyRuleto.Value = int.Parse(_ruleView.ApplyRuleID.ToString());
                txtDescription.Text = _ruleView.RuleDescription;
                cmbCalculation.Value = int.Parse(_ruleView.CalculationID.ToString());
                cmbCurrencyUsed.Value = int.Parse(_ruleView.CurrencyID.ToString());
                cmbCommissionRate.Value = int.Parse(_ruleView.CommissionRateID.ToString());
                nudCommissionRate.Value = (int.Parse(_ruleView.Commission.ToString()));
                chkACofRule.Checked = (int.Parse(_ruleView.ApplyCriteria.ToString()) == 1 ? true : false);


                cmbCriteria.Value = int.Parse(_ruleView.CommissionCalculationID_FK.ToString());

                nudCommissionRate4.Value = int.Parse(_ruleView.MinimumCommissionRate.ToString());


                cmbLOperator.Value = int.Parse(_ruleView.OperatorID_FK1.ToString());
                cmbLOperator1.Value = int.Parse(_ruleView.OperatorID_FK2.ToString());
                cmbLOperator2.Value = int.Parse(_ruleView.OperatorID_FK3.ToString());
                numConstant.Value = int.Parse(_ruleView.Value1.ToString());
                numConstant1.Value = int.Parse(_ruleView.Value2.ToString());
                numConstant2.Value = int.Parse(_ruleView.Value3.ToString());
                cmbCommissionRate1.Value = int.Parse(_ruleView.CommissionRateID_FK1.ToString());
                cmbCommissionRate2.Value = int.Parse(_ruleView.CommissionRateID_FK2.ToString());
                cmbCommissionRate3.Value = int.Parse(_ruleView.CommissionRateID_FK3.ToString());
                nudCommissionRate1.Value = decimal.Parse(_ruleView.CommisionRate1.ToString());
                nudCommissionRate2.Value = decimal.Parse(_ruleView.CommisionRate2.ToString());
                nudCommissionRate3.Value = decimal.Parse(_ruleView.CommisionRate3.ToString());





            }

        }
        #endregion

        private void ResetControls()
        {
            bool chkACofRuleState = false;
            chkACofRuleState = chkACofRule.Checked;
            if (chkACofRuleState == false && disableView == false)
            {
                cmbCriteria.Enabled = false;
                cmbLOperator.Enabled = false;
                cmbLOperator1.Enabled = false;
                cmbLOperator2.Enabled = false;

                numConstant.Enabled = false;
                numConstant1.Enabled = false;
                numConstant2.Enabled = false;

                cmbCommissionRate1.Enabled = false;
                cmbCommissionRate2.Enabled = false;
                cmbCommissionRate3.Enabled = false;

                nudCommissionRate1.Enabled = false;
                nudCommissionRate2.Enabled = false;
                nudCommissionRate3.Enabled = false;

                btnResetRule1.Enabled = false;
                btnResetRule2.Enabled = false;
                btnResetRule3.Enabled = false;

                nudCommissionRate4.Enabled = false;
            }
        }

        public void ResetErrorMsg()
        {
            errorProvider1.SetError(cmbAUEC, "");
            errorProvider1.SetError(txtNamefRule, "");
            errorProvider1.SetError(cmbApplyRuleto, "");
            errorProvider1.SetError(txtDescription, "");
            errorProvider1.SetError(cmbCalculation, "");
            errorProvider1.SetError(cmbCurrencyUsed, "");
            errorProvider1.SetError(cmbCommissionRate, "");
            errorProvider1.SetError(nudCommissionRate, "");
            errorProvider1.SetError(cmbCriteria, "");

            errorProvider1.SetError(cmbCalculation, "");
            errorProvider1.SetError(nudCommissionRate4, "");
            errorProvider1.SetError(cmbCriteria, "");
            errorProvider1.SetError(cmbLOperator, "");
            errorProvider1.SetError(cmbLOperator1, "");
            errorProvider1.SetError(cmbLOperator2, "");
            errorProvider1.SetError(numConstant, "");
            errorProvider1.SetError(numConstant1, "");
            errorProvider1.SetError(numConstant2, "");
            errorProvider1.SetError(cmbCommissionRate1, "");
            errorProvider1.SetError(cmbCommissionRate2, "");
            errorProvider1.SetError(cmbCommissionRate3, "");
            errorProvider1.SetError(nudCommissionRate1, "");
            errorProvider1.SetError(nudCommissionRate2, "");
            errorProvider1.SetError(nudCommissionRate3, "");

        }

        public void RefreshAllCommissionRuleDetails()
        {

            cmbAUEC.Text = C_COMBO_SELECT;
            txtNamefRule.Text = "";
            cmbApplyRuleto.Text = C_COMBO_SELECT;
            txtDescription.Text = "";
            cmbCalculation.Text = C_COMBO_SELECT;
            cmbCurrencyUsed.Text = C_COMBO_SELECT;
            cmbCommissionRate.Text = C_COMBO_SELECT;
            cmbCommissionRate.Value = 1;
            cmbCriteria.Text = C_COMBO_SELECT;
            cmbCriteria.Text = C_COMBO_SELECT;
            cmbLOperator.Text = C_COMBO_SELECT;
            cmbLOperator1.Text = C_COMBO_SELECT;
            cmbLOperator2.Text = C_COMBO_SELECT;

            cmbCommissionRate1.Text = C_COMBO_SELECT;
            cmbCommissionRate2.Text = C_COMBO_SELECT;
            cmbCommissionRate3.Text = C_COMBO_SELECT;


        }



        private int _noData = int.MinValue;
        public int NoData
        {
            set
            {
                _noData = value;
            }
        }


        private void CreateCommissionRules_Load(object sender, System.EventArgs e)
        {
            if (_ruleEdit != null)
            {
                BindForEdit();
            }
            else if (_ruleView != null)
            {
                BindForView();
            }


            else
            {

                RefreshAllCommissionRuleDetails();
            }



        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        //		#region SaveALLCommissionRules
        //		private void SaveALLCommissionRules()
        //		{
        //			errorProvider1.SetError(cmbAUEC, "");
        //			errorProvider1.SetError(txtNamefRule, "");
        //			errorProvider1.SetError(cmbApplyRuleto, "");
        //			errorProvider1.SetError(txtDescription, "");
        //			errorProvider1.SetError(cmbCalculation, "");
        //			errorProvider1.SetError(cmbCurrencyUsed, "");
        //			errorProvider1.SetError(cmbCommissionRate, "");
        //			errorProvider1.SetError(nudCommissionRate, "");
        //			errorProvider1.SetError(cmbCriteria, "");
        //			
        //		
        //			errorProvider1.SetError(nudCommissionRate4, "");
        //			errorProvider1.SetError(cmbCriteria, "");
        //			errorProvider1.SetError(cmbLOperator, "");
        //			errorProvider1.SetError(cmbLOperator1, "");
        //			errorProvider1.SetError(cmbLOperator2, "");
        //			errorProvider1.SetError(numConstant, "");
        //			errorProvider1.SetError(numConstant1, "");
        //			errorProvider1.SetError(numConstant2, "");
        //			errorProvider1.SetError(cmbCommissionRate1, "");
        //			errorProvider1.SetError(cmbCommissionRate2, "");
        //			errorProvider1.SetError(cmbCommissionRate3, "");
        //			errorProvider1.SetError(nudCommissionRate1, "");
        //			errorProvider1.SetError(nudCommissionRate2, "");
        //			errorProvider1.SetError(nudCommissionRate3, "");
        //			
        //
        //
        //			
        //			if(int.Parse(cmbAUEC.Value.ToString()) == int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbAUEC, "Please select AUEC!");
        //				cmbAUEC.Focus();
        //				
        //			}
        //			else if(txtNamefRule.Text.Trim() == "")
        //			{
        //				errorProvider1.SetError(txtNamefRule, "Please enter Name of Rule !");
        //				txtNamefRule.Focus();
        //	
        //			} 
        //			else if(int.Parse(cmbApplyRuleto.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbApplyRuleto,"Please select Rule applied");
        //				cmbApplyRuleto.Focus();
        //					
        //			}
        //			else if(txtDescription.Text.Trim() ==" ")
        //			{
        //				errorProvider1.SetError(txtDescription,"Please enter Description");
        //				txtDescription.Focus();
        //					
        //			}
        //			else if(int.Parse(cmbCalculation.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbCalculation,"Please select Value");
        //				cmbCalculation.Focus();
        //					
        //			}
        //			else if(int.Parse(cmbCurrencyUsed.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbCurrencyUsed,"Please enter Currency");
        //				cmbCurrencyUsed.Focus();
        //				
        //			}
        //			else if(int.Parse(cmbCommissionRate.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbCommissionRate,"Please enter Value of CommissionRate");
        //				cmbCommissionRate.Focus();
        //				
        //			}
        //			else if(int.Parse(nudCommissionRate.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(nudCommissionRate,"Enter Value");
        //				nudCommissionRate.Focus();
        //					
        //			}
        //			else if(int.Parse(cmbCriteria.Value.ToString()) == int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbCriteria, "Please select Criteria!");
        //				cmbCriteria.Focus();
        //			}
        //			else if(int.Parse(nudCommissionRate.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(nudCommissionRate,"Please select Value");
        //				nudCommissionRate.Focus();
        //			}
        //			else if(int.Parse(cmbCriteria.Value.ToString()) == int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbCriteria, "Please select Criteria!");
        //				cmbCriteria.Focus();
        //			}
        //			
        //			else if(int.Parse(cmbLOperator.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbLOperator,"Please select Operator");
        //				cmbLOperator.Focus();
        //			}
        //			
        //			else if(int.Parse(cmbLOperator1.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbLOperator1,"Please select Operator");
        //				cmbLOperator1.Focus();
        //			}
        //			else if(int.Parse(cmbLOperator2.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbLOperator2,"Please select Operator");
        //				cmbLOperator2.Focus();
        //			}
        //			else if(int.Parse(numConstant.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(numConstant,"Please select value");
        //				numConstant.Focus();
        //			}
        //			else if(int.Parse(numConstant1.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(numConstant1,"Enter Value");
        //				numConstant1.Focus();
        //			}
        //			else if(int.Parse(numConstant2.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(numConstant2,"Enter Value");
        //				numConstant2.Focus();
        //			}
        //			else if(int.Parse(cmbCommissionRate1.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbCommissionRate1,"Please select Value");
        //				cmbCommissionRate1.Focus();
        //			}
        //			else if(int.Parse(cmbCommissionRate2.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbCommissionRate2,"Please select Value");
        //				cmbCommissionRate2.Focus();
        //			}
        //			else if(int.Parse(cmbCommissionRate3.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(cmbCommissionRate3,"Please select Value");
        //				cmbCommissionRate3.Focus();
        //			}
        //			else if(int.Parse(nudCommissionRate1.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(nudCommissionRate1,"Please select Value");
        //				nudCommissionRate1.Focus();
        //			}
        //			else if(int.Parse(nudCommissionRate2.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(nudCommissionRate2,"Please select Value");
        //				nudCommissionRate2.Focus();
        //			}
        //			else if(int.Parse(nudCommissionRate3.Value.ToString())==int.MinValue)
        //			{
        //				errorProvider1.SetError(nudCommissionRate3,"Please select Value");
        //				nudCommissionRate3.Focus();
        //			}
        //
        //			else
        //			{
        //					
        //				_ruleEdit.AUECID=int.Parse(cmbAUEC.Value.ToString());
        //				_ruleEdit.RuleName = txtNamefRule.Text.Trim().ToString();
        //				_ruleEdit.ApplyRuleID = int.Parse(cmbApplyRuleto.Value.ToString());
        //				_ruleEdit.RuleDescription= txtDescription.Text.Trim().ToString();
        //				_ruleEdit.CalculationID=int.Parse(cmbCalculation.Value.ToString());
        //				_ruleEdit.CurrencyID=int.Parse(cmbCurrencyUsed.Value.ToString());
        //				_ruleEdit.CommissionRateID=int.Parse(cmbCommissionRate.Value.ToString());
        //				_ruleEdit.Commission= float.Parse(nudCommissionRate.Value.ToString());
        //					
        //				_ruleEdit.ApplyCriteria = (chkACofRule.Checked==true?1:0);
        //				_ruleEdit.CommissionCalculationID_FK=int.Parse(cmbCriteria.Value.ToString());
        //				_ruleEdit.MinimumCommissionRate=int.Parse(nudCommissionRate4.Value.ToString());
        //				_ruleEdit.OperatorID_FK1=int.Parse(cmbLOperator.Value.ToString());
        //				_ruleEdit.OperatorID_FK2=int.Parse(cmbLOperator1.Value.ToString());
        //				_ruleEdit.OperatorID_FK3=int.Parse(cmbLOperator2.Value.ToString());
        //				_ruleEdit.Value1=int.Parse(numConstant.Value.ToString());
        //				_ruleEdit.Value2=int.Parse(numConstant1.Value.ToString());
        //				_ruleEdit.Value3=int.Parse(numConstant2.Value.ToString());
        //				_ruleEdit.CommissionRateID_FK1=int.Parse(cmbCommissionRate1.Value.ToString());
        //				_ruleEdit.CommissionRateID_FK2=int.Parse(cmbCommissionRate2.Value.ToString());
        //				_ruleEdit.CommissionRateID_FK3=int.Parse(cmbCommissionRate3.Value.ToString());
        //				_ruleEdit.CommisionRate1=int.Parse(nudCommissionRate1.Value.ToString());
        //				_ruleEdit.CommisionRate2=int.Parse(nudCommissionRate2.Value.ToString());
        //				_ruleEdit.CommisionRate3=int.Parse(nudCommissionRate3.Value.ToString());
        //				
        //				this.Hide();
        //			}
        //		}
        //		#endregion
        #region btnSave_Click
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (_ruleEdit != null)
            {
                AUECCommissionRule auecCommissionRule = auecCommissionRuleproperty;

                if (auecCommissionRule != null)
                {

                    RuleID = int.Parse(CommissionRuleManager.SaveAUECCommissionRule(auecCommissionRule, RuleID).ToString());
                    if (RuleID < 0)
                    {
                        MessageBox.Show("Commission Rule with the same name already exists.", "Prana Alert", MessageBoxButtons.OK);
                        _errorStatus = true;
                    }
                    else
                    {
                        CommissionCriteria commissionCriteria = commissionCriteriaproperty;
                        if (commissionCriteria != null)
                        {
                            int CommissionCriteriaID = int.Parse(CommissionRuleManager.SaveCommissionCriteria(commissionCriteria, RuleID).ToString());

                            CommissionRuleCriteriaold commissionRuleCriteria = commissionRuleCriteriaproperty;

                            if (commissionRuleCriteria != null)
                            {
                                int CommissionRuleCriteriaID = int.Parse(CommissionRuleManager.SaveCommissionRuleCriteria(commissionRuleCriteria, CommissionCriteriaID).ToString());
                            }
                        }
                        else
                        {
                            CommissionRuleManager.DeleteCommissionCrietariaDetails(RuleID);
                        }
                    }
                }
            }
            if (_errorStatus == false)
            {
                this.Close();
            }
        }



        #endregion

        /*	#region BindAUECS
            private void BindAUECS()
            {
                AUECs aAUECs=AUECManager.GetAUEC();
                Assets assets=new Assets();
                UnderLyings underlyings= new UnderLyings();
                Exchanges exchanges= new Exchanges();
                foreach(AUEC auec in aAUECs)
                {
                    assets.Add(new Asset(auec.AssetID, auec.Asset.Name));
                    underlyings.Add(new UnderLying(auec.UnderlyingID, auec.UnderLying.Name));
                    exchanges.Add(new Exchange(auec.ExchangeID, auec.Exchange.DisplayName));

                }
                assets.Insert(0, new Asset(int.MinValue, C_COMBO_SELECT));
                cmbAssetClass.DataSource = assets;
                cmbAssetClass.DisplayMember = "Name";
                cmbAssetClass.ValueMember = "AssetID";
                cmbAssetClass.Value = int.MinValue;


                underlyings.Insert(0, new UnderLying(int.MinValue, C_COMBO_SELECT));
                cmbUnderlying.DataSource=underlyings;
                cmbUnderlying.DisplayMember="Name";
                cmbUnderlying.ValueMember="UnderlyingID";
                cmbUnderlying.Value=int.MinValue;

                exchanges.Insert(0, new Exchange(int.MinValue, C_COMBO_SELECT));
                cmbExchange.DataSource=exchanges;
                cmbExchange.DisplayMember = "DisplayName";
                cmbExchange.ValueMember = "ExchangeID";
                cmbExchange.Value = int.MinValue;

                ColumnsCollection columns = cmbUnderlying.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if(column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }
                ColumnsCollection columns1 = cmbAssetClass.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns1)
                {
                    if(column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }


                ColumnsCollection columns2 = cmbExchange.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns2)
                {
                    if(column.Key != "DisplayName")
                    {
                        column.Hidden = true;
                    }
                }

            }
            #endregion*/

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

            cmbCommissionRate1.DataSource = null;
            cmbCommissionRate1.DataSource = commissionratetypes;
            cmbCommissionRate1.DisplayMember = "CommissionRateTypeName";
            cmbCommissionRate1.ValueMember = "CommissionRateID";
            cmbCommissionRate1.Value = int.MinValue;
            cmbCommissionRate2.DataSource = null;
            cmbCommissionRate2.DataSource = commissionratetypes;
            cmbCommissionRate2.DisplayMember = "CommissionRateTypeName";
            cmbCommissionRate2.ValueMember = "CommissionRateID";
            cmbCommissionRate2.Value = int.MinValue;
            cmbCommissionRate3.DataSource = null;
            cmbCommissionRate3.DataSource = commissionratetypes;
            cmbCommissionRate3.DisplayMember = "CommissionRateTypeName";
            cmbCommissionRate3.ValueMember = "CommissionRateID";
            cmbCommissionRate3.Value = int.MinValue;


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

        #region BindOperator
        private void BindOperator()
        {
            Operaters operaters = CommissionRuleManager.GetOperaters();
            operaters = CommissionRuleManager.GetOperaters();
            operaters.Insert(0, new Prana.Admin.BLL.Operater(int.MinValue, C_COMBO_SELECT));
            cmbLOperator.DataSource = null;
            cmbLOperator.DataSource = operaters;
            cmbLOperator.DisplayMember = "Name";
            cmbLOperator.ValueMember = "OperatorID";
            cmbLOperator.Value = int.MinValue;
            cmbLOperator1.DataSource = null;
            cmbLOperator1.DataSource = operaters;
            cmbLOperator1.DisplayMember = "Name";
            cmbLOperator1.ValueMember = "OperatorID";
            cmbLOperator1.Value = int.MinValue;
            cmbLOperator2.DataSource = null;
            cmbLOperator2.DataSource = operaters;
            cmbLOperator2.DisplayMember = "Name";
            cmbLOperator2.ValueMember = "OperatorID";
            cmbLOperator2.Value = int.MinValue;

        }
        #endregion

        #region BindCriteria

        private void BindCriteria()
        {

            CommissionCalculations commissioncalculations = CommissionRuleManager.GetCalculation();
            commissioncalculations = CommissionRuleManager.GetCalculation();
            commissioncalculations.Insert(0, new Prana.Admin.BLL.CommissionCalculation(int.MinValue, C_COMBO_SELECT));
            cmbCriteria.DataSource = commissioncalculations;
            cmbCriteria.DisplayMember = "CalculationType";
            cmbCriteria.ValueMember = "CommissionCalculationID";
            cmbCriteria.Value = int.MinValue;
            ColumnsCollection columns9 = cmbCriteria.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns9)
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
                    //Currency currency = new Currency();
                    //currency = AUECManager.GetCurrency(auec.Compliance.BaseCurrencyID);

                    //SK 2061009 removed Compliance class
                    //string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.Exchange.DisplayName.ToString() + "/" + auec.Currency.CurrencySymbol.ToString();
                    string Data = auec.AUECString;
                    int Value = auec.AUECID;
                    //

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

        private void chkACofRule_CheckedChanged(object sender, System.EventArgs e)
        {
            bool chkACofRuleState = false;
            chkACofRuleState = chkACofRule.Checked;
            if (chkACofRuleState == false)
            {
                cmbCriteria.Enabled = false;
                cmbLOperator.Enabled = false;
                cmbLOperator1.Enabled = false;
                cmbLOperator2.Enabled = false;

                numConstant.Enabled = false;
                numConstant1.Enabled = false;
                numConstant2.Enabled = false;

                cmbCommissionRate1.Enabled = false;
                cmbCommissionRate2.Enabled = false;
                cmbCommissionRate3.Enabled = false;

                nudCommissionRate1.Enabled = false;
                nudCommissionRate2.Enabled = false;
                nudCommissionRate3.Enabled = false;

                btnResetRule1.Enabled = false;
                btnResetRule2.Enabled = false;
                btnResetRule3.Enabled = false;

                nudCommissionRate4.Enabled = false;
            }
            else
            {
                if (disableView == false)
                {
                    cmbCriteria.Enabled = true;
                    cmbLOperator.Enabled = true;
                    cmbLOperator1.Enabled = true;
                    cmbLOperator2.Enabled = true;

                    numConstant.Enabled = true;
                    numConstant1.Enabled = true;
                    numConstant2.Enabled = true;

                    cmbCommissionRate1.Enabled = true;
                    cmbCommissionRate2.Enabled = true;
                    cmbCommissionRate3.Enabled = true;

                    nudCommissionRate1.Enabled = true;
                    nudCommissionRate2.Enabled = true;
                    nudCommissionRate3.Enabled = true;

                    btnResetRule1.Enabled = true;
                    btnResetRule2.Enabled = true;
                    btnResetRule3.Enabled = true;

                    nudCommissionRate4.Enabled = true;
                }
            }

        }
        private void BindDetails()
        {
            AUEC auec = AUECManager.GetAUEC(_auecID);

            if (auec != null)
            {
                //				cmbAssetClass.Value = int.Parse(auec.AssetID.ToString());
                //				cmbUnderlying.Value=int.Parse(auec.UnderlyingID.ToString());

                //SK 2061009 removed Compliance class				
                //Currency currency = new Currency();
                //currency = AUECManager.GetCurrency(auec.Compliance.BaseCurrencyID);
                //
                cmbCurrencyUsed.Value = int.Parse(auec.Currency.CurencyID.ToString());
            }
        }

        public int _auecID = int.MinValue;

        private void cmbCommissionRate_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void cmbCriteria_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void btnResetRule1_Click(object sender, System.EventArgs e)
        {
            cmbLOperator.Value = int.MinValue;
            numConstant.Value = PER_UNIT_COMMISSIONRATE;
            cmbCommissionRate1.Value = int.MinValue;
            nudCommissionRate1.Value = BASIS_POINT_COMMISSIONRATE;
        }

        private void btnResetRule2_Click(object sender, System.EventArgs e)
        {
            cmbLOperator1.Value = int.MinValue;
            numConstant1.Value = PER_UNIT_COMMISSIONRATE;
            cmbCommissionRate2.Value = int.MinValue;
            nudCommissionRate2.Value = BASIS_POINT_COMMISSIONRATE;
        }

        private void btnResetRule3_Click(object sender, System.EventArgs e)
        {
            cmbLOperator2.Value = int.MinValue;
            numConstant2.Value = PER_UNIT_COMMISSIONRATE;
            cmbCommissionRate3.Value = int.MinValue;
            nudCommissionRate3.Value = BASIS_POINT_COMMISSIONRATE;
        }

        private void numConstant_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string numConstant2Value = numConstant2.Value.ToString();
        }

        private void numConstant1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string numConstant1Value = numConstant1.Value.ToString();
        }

        private void numConstant2_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string numConstantValueTemp = numConstant.Value.ToString();
        }

        private void cmbCommissionRate1_ValueChanged(object sender, System.EventArgs e)
        {
            if (cmbCommissionRate1.Value != null)
            {
                if (cmbCommissionRate1.Text == "Shares")
                {
                    lbldisplay1.Text = "Cents Per Share";
                    nudCommissionRate1.Minimum = PER_UNIT_COMMISSIONRATE;
                    nudCommissionRate1.Value = PER_UNIT_COMMISSIONRATE;
                }
                else if (cmbCommissionRate1.Text == "Contracts")
                {
                    lbldisplay1.Text = "per contract";
                    nudCommissionRate1.Minimum = PER_UNIT_COMMISSIONRATE;
                    nudCommissionRate1.Value = PER_UNIT_COMMISSIONRATE;
                }
                else if (cmbCommissionRate1.Text == "Notional")
                {
                    lbldisplay1.Text = "Basis Points";
                    nudCommissionRate1.Minimum = PER_UNIT_COMMISSIONRATE;
                    nudCommissionRate1.Value = PER_UNIT_COMMISSIONRATE;
                }
                else
                {
                    lbldisplay1.Text = "";
                    nudCommissionRate1.Minimum = PER_UNIT_COMMISSIONRATE;
                    nudCommissionRate1.Value = PER_UNIT_COMMISSIONRATE;
                }
            }
        }

        private void cmbCommissionRate2_ValueChanged(object sender, System.EventArgs e)
        {
            if (cmbCommissionRate2.Value != null)
            {
                if (cmbCommissionRate2.Text == "Shares")
                {
                    lbldisplay2.Text = "Cents Per Share";
                    nudCommissionRate2.Minimum = PER_UNIT_COMMISSIONRATE;
                    nudCommissionRate2.Value = PER_UNIT_COMMISSIONRATE;
                }
                else if (cmbCommissionRate2.Text == "Contracts")
                {
                    lbldisplay2.Text = "per contract";
                    nudCommissionRate2.Minimum = PER_UNIT_COMMISSIONRATE;
                    nudCommissionRate2.Value = PER_UNIT_COMMISSIONRATE;
                }
                else if (cmbCommissionRate2.Text == "Notional")
                {
                    lbldisplay2.Text = "Basis Points";
                    nudCommissionRate2.Minimum = PER_UNIT_COMMISSIONRATE;
                    nudCommissionRate2.Value = PER_UNIT_COMMISSIONRATE;
                }
                else
                {
                    lbldisplay2.Text = "";
                    nudCommissionRate2.Minimum = PER_UNIT_COMMISSIONRATE;
                    nudCommissionRate2.Value = PER_UNIT_COMMISSIONRATE;
                }
            }
        }

        private void cmbCommissionRate3_ValueChanged(object sender, System.EventArgs e)
        {
            if (cmbCommissionRate3.Value != null)
            {
                if (cmbCommissionRate3.Text == "Shares")
                {
                    lbldisplay3.Text = "Cents Per Share";
                    nudCommissionRate3.Minimum = PER_UNIT_COMMISSIONRATE;
                    nudCommissionRate3.Value = PER_UNIT_COMMISSIONRATE;
                }
                else if (cmbCommissionRate3.Text == "Contracts")
                {
                    lbldisplay3.Text = "per contract";
                    nudCommissionRate3.Minimum = PER_UNIT_COMMISSIONRATE;
                    nudCommissionRate3.Value = PER_UNIT_COMMISSIONRATE;
                }
                else if (cmbCommissionRate3.Text == "Notional")
                {
                    lbldisplay3.Text = "Basis Points";
                    nudCommissionRate3.Minimum = PER_UNIT_COMMISSIONRATE;
                    nudCommissionRate3.Value = PER_UNIT_COMMISSIONRATE;
                }
                else
                {
                    lbldisplay3.Text = "";
                    nudCommissionRate3.Minimum = PER_UNIT_COMMISSIONRATE;
                    nudCommissionRate3.Value = PER_UNIT_COMMISSIONRATE;
                }
            }
        }

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
        //		private void cmbAUEC_ValueChanged(object sender, System.EventArgs e)
        //		{
        //			_auecID =int.Parse(cmbAUEC.Value.ToString());
        //			BindDetails();
        //		}
    }
}


