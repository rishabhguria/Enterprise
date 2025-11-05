#region Using
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace Prana.Admin.RiskManagement.Controls
{
    public delegate void CurrencyChangedHandler(System.Object sender, PassValueEventArgs e);

    /// <summary>
    /// Summary description for Company_OverallLimits.
    /// </summary>
    public class uctCompanyOverallLimits : System.Windows.Forms.UserControl
    {

        #region Wizard Stuff
        const string C_COMBO_SELECT = "- Select -";

        #region private and protected members

        private int _companyID = int.MinValue;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbRMBaseCurrency;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Infragistics.Win.Misc.UltraLabel lbl1;
        private Infragistics.Win.Misc.UltraGroupBox grpBxOverallLimits;
        private Infragistics.Win.Misc.UltraLabel lblNegativePNLLimit;
        private Infragistics.Win.Misc.UltraLabel lblPositivePNLLimit;
        private Infragistics.Win.Misc.UltraLabel lblExposureLimit;
        private Infragistics.Win.Misc.UltraLabel lblCalculateRiskLimit;
        private Infragistics.Win.Misc.UltraLabel lblRMBaseCurrency;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtExposurelimit;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtnegativePNLLimit;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtpositivePNLLimit;
        private Infragistics.Win.Misc.UltraLabel UneditExpLt;
        private Infragistics.Win.Misc.UltraLabel UneditnegPNL;
        private Infragistics.Win.Misc.UltraLabel UneditposPNL;
        private Infragistics.Win.Misc.UltraGroupBox grpCurrencyRates;
        private UltraGrid grdCurrency;
        private Prana.Utilities.UI.UIUtilities.Spinner spnCalRiskLimit;
        private NumericUpDown spnCalRefreshRate;
        private IContainer components;

        public event CurrencyChangedHandler CurrencyChanged;
        #endregion

        public int CompanyID
        {
            set { _companyID = value; }
        }

        public uctCompanyOverallLimits()
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
                if (cmbRMBaseCurrency != null)
                {
                    cmbRMBaseCurrency.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (lbl1 != null)
                {
                    lbl1.Dispose();
                }
                if (grpBxOverallLimits != null)
                {
                    grpBxOverallLimits.Dispose();
                }
                if (lblNegativePNLLimit != null)
                {
                    lblNegativePNLLimit.Dispose();
                }
                if (lblPositivePNLLimit != null)
                {
                    lblPositivePNLLimit.Dispose();
                }
                if (lblExposureLimit != null)
                {
                    lblExposureLimit.Dispose();
                }
                if (lblCalculateRiskLimit != null)
                {
                    lblCalculateRiskLimit.Dispose();
                }
                if (lblRMBaseCurrency != null)
                {
                    lblRMBaseCurrency.Dispose();
                }
                if (ultraLabel2 != null)
                {
                    ultraLabel2.Dispose();
                }
                if (ultraLabel1 != null)
                {
                    ultraLabel1.Dispose();
                }
                if (txtExposurelimit != null)
                {
                    txtExposurelimit.Dispose();
                }
                if (txtnegativePNLLimit != null)
                {
                    txtnegativePNLLimit.Dispose();
                }
                if (txtpositivePNLLimit != null)
                {
                    txtpositivePNLLimit.Dispose();
                }
                if (UneditExpLt != null)
                {
                    UneditExpLt.Dispose();
                }
                if (UneditnegPNL != null)
                {
                    UneditnegPNL.Dispose();
                }
                if (UneditposPNL != null)
                {
                    UneditposPNL.Dispose();
                }
                if (grpCurrencyRates != null)
                {
                    grpCurrencyRates.Dispose();
                }
                if (grdCurrency != null)
                {
                    grdCurrency.Dispose();
                }
                if (spnCalRiskLimit != null)
                {
                    spnCalRiskLimit.Dispose();
                }
                if (spnCalRefreshRate != null)
                {
                    spnCalRefreshRate.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion Wizard Stuff

        #region Component Designer generated code
        /// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurencyID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencySymbol", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyTypeID", 4);
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RMBaseCurrency", 0);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AllOtherCurrencies", 1);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Conversion", 2);
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyRates", 3, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FromCurrencyID", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ToCurrency", 5);
            this.cmbRMBaseCurrency = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpBxOverallLimits = new Infragistics.Win.Misc.UltraGroupBox();
            this.spnCalRefreshRate = new System.Windows.Forms.NumericUpDown();
            this.spnCalRiskLimit = new Spinner();
            this.UneditnegPNL = new Infragistics.Win.Misc.UltraLabel();
            this.UneditposPNL = new Infragistics.Win.Misc.UltraLabel();
            this.UneditExpLt = new Infragistics.Win.Misc.UltraLabel();
            this.txtnegativePNLLimit = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtpositivePNLLimit = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtExposurelimit = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblNegativePNLLimit = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.lblPositivePNLLimit = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblExposureLimit = new Infragistics.Win.Misc.UltraLabel();
            this.lbl1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCalculateRiskLimit = new Infragistics.Win.Misc.UltraLabel();
            this.lblRMBaseCurrency = new Infragistics.Win.Misc.UltraLabel();
            this.grpCurrencyRates = new Infragistics.Win.Misc.UltraGroupBox();
            this.grdCurrency = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRMBaseCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxOverallLimits)).BeginInit();
            this.grpBxOverallLimits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnCalRefreshRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtnegativePNLLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtpositivePNLLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExposurelimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCurrencyRates)).BeginInit();
            this.grpCurrencyRates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCurrency)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbRMBaseCurrency
            // 
            this.cmbRMBaseCurrency.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbRMBaseCurrency.Appearance = appearance1;
            this.cmbRMBaseCurrency.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance2.BorderColor = System.Drawing.Color.Black;
            this.cmbRMBaseCurrency.ButtonAppearance = appearance2;
            this.cmbRMBaseCurrency.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbRMBaseCurrency.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            ultraGridBand1.Hidden = true;
            this.cmbRMBaseCurrency.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbRMBaseCurrency.DisplayMember = "";
            this.cmbRMBaseCurrency.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbRMBaseCurrency.DropDownWidth = 0;
            this.cmbRMBaseCurrency.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbRMBaseCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbRMBaseCurrency.Location = new System.Drawing.Point(278, 6);
            this.cmbRMBaseCurrency.Name = "cmbRMBaseCurrency";
            this.cmbRMBaseCurrency.Size = new System.Drawing.Size(104, 21);
            this.cmbRMBaseCurrency.TabIndex = 5;
            this.cmbRMBaseCurrency.ValueMember = "";
            this.cmbRMBaseCurrency.Leave += new System.EventHandler(this.cmbRMBaseCurrency_Leave);
            this.cmbRMBaseCurrency.ValueChanged += new System.EventHandler(this.cmbRMBaseCurrency_ValueChanged);
            this.cmbRMBaseCurrency.Enter += new System.EventHandler(this.cmbRMBaseCurrency_Enter);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grpBxOverallLimits
            // 
            this.grpBxOverallLimits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grpBxOverallLimits.Controls.Add(this.spnCalRefreshRate);
            this.grpBxOverallLimits.Controls.Add(this.spnCalRiskLimit);
            this.grpBxOverallLimits.Controls.Add(this.UneditnegPNL);
            this.grpBxOverallLimits.Controls.Add(this.UneditposPNL);
            this.grpBxOverallLimits.Controls.Add(this.UneditExpLt);
            this.grpBxOverallLimits.Controls.Add(this.txtnegativePNLLimit);
            this.grpBxOverallLimits.Controls.Add(this.txtpositivePNLLimit);
            this.grpBxOverallLimits.Controls.Add(this.txtExposurelimit);
            this.grpBxOverallLimits.Controls.Add(this.lblNegativePNLLimit);
            this.grpBxOverallLimits.Controls.Add(this.ultraLabel2);
            this.grpBxOverallLimits.Controls.Add(this.lblPositivePNLLimit);
            this.grpBxOverallLimits.Controls.Add(this.ultraLabel1);
            this.grpBxOverallLimits.Controls.Add(this.lblExposureLimit);
            this.grpBxOverallLimits.Controls.Add(this.lbl1);
            this.grpBxOverallLimits.Controls.Add(this.lblCalculateRiskLimit);
            this.grpBxOverallLimits.Controls.Add(this.lblRMBaseCurrency);
            this.grpBxOverallLimits.Controls.Add(this.cmbRMBaseCurrency);
            this.grpBxOverallLimits.Location = new System.Drawing.Point(5, 8);
            this.grpBxOverallLimits.Name = "grpBxOverallLimits";
            this.grpBxOverallLimits.Size = new System.Drawing.Size(500, 131);
            this.grpBxOverallLimits.SupportThemes = false;
            this.grpBxOverallLimits.TabIndex = 27;
            // 
            // spnCalRefreshRate
            // 
            this.spnCalRefreshRate.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.spnCalRefreshRate.Location = new System.Drawing.Point(388, 29);
            this.spnCalRefreshRate.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.spnCalRefreshRate.Name = "spnCalRefreshRate";
            this.spnCalRefreshRate.Size = new System.Drawing.Size(87, 21);
            this.spnCalRefreshRate.TabIndex = 40;
            // 
            // spnCalRiskLimit
            // 
            this.spnCalRiskLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.spnCalRiskLimit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnCalRiskLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spnCalRiskLimit.DataType = DataTypes.PositiveInteger;
            this.spnCalRiskLimit.Increment = 5;
            this.spnCalRiskLimit.Location = new System.Drawing.Point(278, 31);
            this.spnCalRiskLimit.MaxValue = 360;
            this.spnCalRiskLimit.MinValue = 0;
            this.spnCalRiskLimit.Name = "spnCalRiskLimit";
            this.spnCalRiskLimit.Size = new System.Drawing.Size(104, 21);
            this.spnCalRiskLimit.TabIndex = 38;
            this.spnCalRiskLimit.Value = 0;
            this.spnCalRiskLimit.Enter += new System.EventHandler(this.spnCalRiskLimit_Enter);
            this.spnCalRiskLimit.Leave += new System.EventHandler(this.spnCalRiskLimit_Leave);
            // 
            // UneditnegPNL
            // 
            this.UneditnegPNL.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance11.TextHAlign = Infragistics.Win.HAlign.Center;
            this.UneditnegPNL.Appearance = appearance11;
            this.UneditnegPNL.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.UneditnegPNL.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.UneditnegPNL.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.UneditnegPNL.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.UneditnegPNL.Location = new System.Drawing.Point(388, 104);
            this.UneditnegPNL.Name = "UneditnegPNL";
            this.UneditnegPNL.Size = new System.Drawing.Size(2, 2);
            this.UneditnegPNL.TabIndex = 37;
            // 
            // UneditposPNL
            // 
            this.UneditposPNL.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance12.TextHAlign = Infragistics.Win.HAlign.Center;
            this.UneditposPNL.Appearance = appearance12;
            this.UneditposPNL.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.UneditposPNL.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.UneditposPNL.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.UneditposPNL.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.UneditposPNL.Location = new System.Drawing.Point(388, 80);
            this.UneditposPNL.Name = "UneditposPNL";
            this.UneditposPNL.Size = new System.Drawing.Size(2, 2);
            this.UneditposPNL.TabIndex = 36;
            // 
            // UneditExpLt
            // 
            this.UneditExpLt.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance13.TextHAlign = Infragistics.Win.HAlign.Center;
            this.UneditExpLt.Appearance = appearance13;
            this.UneditExpLt.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.UneditExpLt.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.UneditExpLt.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.UneditExpLt.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.UneditExpLt.Location = new System.Drawing.Point(388, 57);
            this.UneditExpLt.Name = "UneditExpLt";
            this.UneditExpLt.Size = new System.Drawing.Size(2, 2);
            this.UneditExpLt.TabIndex = 35;
            // 
            // txtnegativePNLLimit
            // 
            this.txtnegativePNLLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance14.BorderColor = System.Drawing.Color.Black;
            appearance14.TextHAlign = Infragistics.Win.HAlign.Left;
            this.txtnegativePNLLimit.Appearance = appearance14;
            this.txtnegativePNLLimit.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtnegativePNLLimit.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtnegativePNLLimit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtnegativePNLLimit.Location = new System.Drawing.Point(278, 103);
            this.txtnegativePNLLimit.MaxLength = 19;
            this.txtnegativePNLLimit.Name = "txtnegativePNLLimit";
            this.txtnegativePNLLimit.Size = new System.Drawing.Size(104, 20);
            this.txtnegativePNLLimit.TabIndex = 34;
            this.txtnegativePNLLimit.Enter += new System.EventHandler(this.txtnegativePNLLimit_Enter);
            this.txtnegativePNLLimit.TextChanged += new System.EventHandler(this.txtnegativePNLLimit_TextChanged);
            this.txtnegativePNLLimit.Leave += new System.EventHandler(this.txtnegativePNLLimit_Leave);
            // 
            // txtpositivePNLLimit
            // 
            this.txtpositivePNLLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance15.BorderColor = System.Drawing.Color.Black;
            appearance15.TextHAlign = Infragistics.Win.HAlign.Left;
            this.txtpositivePNLLimit.Appearance = appearance15;
            this.txtpositivePNLLimit.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtpositivePNLLimit.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtpositivePNLLimit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtpositivePNLLimit.Location = new System.Drawing.Point(278, 79);
            this.txtpositivePNLLimit.MaxLength = 19;
            this.txtpositivePNLLimit.Name = "txtpositivePNLLimit";
            this.txtpositivePNLLimit.Size = new System.Drawing.Size(104, 20);
            this.txtpositivePNLLimit.TabIndex = 33;
            this.txtpositivePNLLimit.Enter += new System.EventHandler(this.txtpositivePNLLimit_Enter);
            this.txtpositivePNLLimit.TextChanged += new System.EventHandler(this.txtpositivePNLLimit_TextChanged);
            this.txtpositivePNLLimit.Leave += new System.EventHandler(this.txtpositivePNLLimit_Leave);
            // 
            // txtExposurelimit
            // 
            this.txtExposurelimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance16.BorderColor = System.Drawing.Color.Black;
            appearance16.TextHAlign = Infragistics.Win.HAlign.Left;
            this.txtExposurelimit.Appearance = appearance16;
            this.txtExposurelimit.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtExposurelimit.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtExposurelimit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtExposurelimit.Location = new System.Drawing.Point(278, 55);
            this.txtExposurelimit.MaxLength = 19;
            this.txtExposurelimit.Name = "txtExposurelimit";
            this.txtExposurelimit.Size = new System.Drawing.Size(104, 20);
            this.txtExposurelimit.TabIndex = 32;
            this.txtExposurelimit.Enter += new System.EventHandler(this.txtExposurelimit_Enter);
            this.txtExposurelimit.TextChanged += new System.EventHandler(this.txtExposurelimit_TextChanged);
            this.txtExposurelimit.Leave += new System.EventHandler(this.txtExposurelimit_Leave);
            // 
            // lblNegativePNLLimit
            // 
            this.lblNegativePNLLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance17.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblNegativePNLLimit.Appearance = appearance17;
            this.lblNegativePNLLimit.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.lblNegativePNLLimit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblNegativePNLLimit.Location = new System.Drawing.Point(85, 102);
            this.lblNegativePNLLimit.Name = "lblNegativePNLLimit";
            this.lblNegativePNLLimit.Size = new System.Drawing.Size(100, 21);
            this.lblNegativePNLLimit.TabIndex = 31;
            this.lblNegativePNLLimit.Text = "- PNL Limit";
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance18.ForeColor = System.Drawing.Color.Red;
            appearance18.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance18.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraLabel2.Appearance = appearance18;
            this.ultraLabel2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel2.Location = new System.Drawing.Point(262, 61);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(10, 15);
            this.ultraLabel2.TabIndex = 30;
            this.ultraLabel2.Text = "*";
            // 
            // lblPositivePNLLimit
            // 
            this.lblPositivePNLLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance19.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblPositivePNLLimit.Appearance = appearance19;
            this.lblPositivePNLLimit.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.lblPositivePNLLimit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPositivePNLLimit.Location = new System.Drawing.Point(85, 79);
            this.lblPositivePNLLimit.Name = "lblPositivePNLLimit";
            this.lblPositivePNLLimit.Size = new System.Drawing.Size(138, 21);
            this.lblPositivePNLLimit.TabIndex = 30;
            this.lblPositivePNLLimit.Text = "+ PNL Limit ( Optional)";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance20.ForeColor = System.Drawing.Color.Red;
            appearance20.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance20.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraLabel1.Appearance = appearance20;
            this.ultraLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel1.Location = new System.Drawing.Point(262, 35);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(10, 15);
            this.ultraLabel1.TabIndex = 29;
            this.ultraLabel1.Text = "*";
            // 
            // lblExposureLimit
            // 
            this.lblExposureLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance21.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblExposureLimit.Appearance = appearance21;
            this.lblExposureLimit.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.lblExposureLimit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblExposureLimit.Location = new System.Drawing.Point(85, 55);
            this.lblExposureLimit.Name = "lblExposureLimit";
            this.lblExposureLimit.Size = new System.Drawing.Size(187, 21);
            this.lblExposureLimit.TabIndex = 29;
            this.lblExposureLimit.Text = "Exposure Limit ";
            // 
            // lbl1
            // 
            this.lbl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance22.ForeColor = System.Drawing.Color.Red;
            appearance22.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance22.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lbl1.Appearance = appearance22;
            this.lbl1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbl1.Location = new System.Drawing.Point(262, 9);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(10, 15);
            this.lbl1.TabIndex = 28;
            this.lbl1.Text = "*";
            // 
            // lblCalculateRiskLimit
            // 
            this.lblCalculateRiskLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance23.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblCalculateRiskLimit.Appearance = appearance23;
            this.lblCalculateRiskLimit.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.lblCalculateRiskLimit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCalculateRiskLimit.Location = new System.Drawing.Point(85, 32);
            this.lblCalculateRiskLimit.Name = "lblCalculateRiskLimit";
            this.lblCalculateRiskLimit.Size = new System.Drawing.Size(141, 21);
            this.lblCalculateRiskLimit.TabIndex = 28;
            this.lblCalculateRiskLimit.Text = "Calculate Risk Limit ( Sec)";
            // 
            // lblRMBaseCurrency
            // 
            this.lblRMBaseCurrency.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance24.TextHAlign = Infragistics.Win.HAlign.Left;
            appearance24.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblRMBaseCurrency.Appearance = appearance24;
            this.lblRMBaseCurrency.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.lblRMBaseCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblRMBaseCurrency.Location = new System.Drawing.Point(85, 6);
            this.lblRMBaseCurrency.Name = "lblRMBaseCurrency";
            this.lblRMBaseCurrency.Size = new System.Drawing.Size(105, 21);
            this.lblRMBaseCurrency.TabIndex = 27;
            this.lblRMBaseCurrency.Text = "Base Currency";
            // 
            // grpCurrencyRates
            // 
            this.grpCurrencyRates.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grpCurrencyRates.Controls.Add(this.grdCurrency);
            this.grpCurrencyRates.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpCurrencyRates.ForeColor = System.Drawing.Color.Black;
            this.grpCurrencyRates.Location = new System.Drawing.Point(5, 145);
            this.grpCurrencyRates.Name = "grpCurrencyRates";
            this.grpCurrencyRates.Size = new System.Drawing.Size(500, 100);
            this.grpCurrencyRates.SupportThemes = false;
            this.grpCurrencyRates.TabIndex = 28;
            this.grpCurrencyRates.Text = "Currency Conversion Rates";
            // 
            // grdCurrency
            // 
            this.grdCurrency.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn6.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn6.Header.Appearance = appearance4;
            ultraGridColumn6.Header.Caption = "Base Currency";
            ultraGridColumn6.Header.VisiblePosition = 0;
            ultraGridColumn6.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn6.Width = 153;
            appearance5.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn7.CellAppearance = appearance5;
            appearance6.FontData.BoldAsString = "True";
            appearance6.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn7.Header.Appearance = appearance6;
            ultraGridColumn7.Header.Caption = "Complaince Currencies";
            ultraGridColumn7.Header.VisiblePosition = 1;
            ultraGridColumn7.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn7.Width = 174;
            appearance7.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn8.CellAppearance = appearance7;
            appearance8.FontData.BoldAsString = "True";
            appearance8.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn8.Header.Appearance = appearance8;
            ultraGridColumn8.Header.VisiblePosition = 2;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn8.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn8.Width = 108;
            appearance9.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn9.CellAppearance = appearance9;
            appearance10.FontData.BoldAsString = "True";
            appearance10.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn9.Header.Appearance = appearance10;
            ultraGridColumn9.Header.Caption = "Currency Rates";
            ultraGridColumn9.Header.VisiblePosition = 3;
            ultraGridColumn9.Width = 129;
            ultraGridColumn10.Header.VisiblePosition = 4;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn10.Width = 82;
            ultraGridColumn11.Header.VisiblePosition = 5;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn11.Width = 82;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11});
            ultraGridBand2.Header.Enabled = false;
            ultraGridBand2.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            ultraGridBand2.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdCurrency.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdCurrency.DisplayLayout.GroupByBox.Hidden = true;
            this.grdCurrency.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCurrency.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdCurrency.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdCurrency.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCurrency.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCurrency.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCurrency.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCurrency.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdCurrency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCurrency.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdCurrency.Location = new System.Drawing.Point(3, 17);
            this.grdCurrency.Name = "grdCurrency";
            this.grdCurrency.Size = new System.Drawing.Size(494, 80);
            this.grdCurrency.TabIndex = 43;
            // 
            // uctCompanyOverallLimits
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpCurrencyRates);
            this.Controls.Add(this.grpBxOverallLimits);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "uctCompanyOverallLimits";
            this.Size = new System.Drawing.Size(511, 251);
            ((System.ComponentModel.ISupportInitialize)(this.cmbRMBaseCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxOverallLimits)).EndInit();
            this.grpBxOverallLimits.ResumeLayout(false);
            this.grpBxOverallLimits.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnCalRefreshRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtnegativePNLLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtpositivePNLLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExposurelimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCurrencyRates)).EndInit();
            this.grpCurrencyRates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCurrency)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Currency Combo Binding
        /// <summary>
        /// This method binds the existing <see cref="Currency"/> in the ComboBox control by assigning the 
        /// companyTypes object to its datasource property.
        /// </summary>
        private void BindCurrency()
        {
            try
            {
                //GetCurrencies method fetches the existing currencies from the database.
                Prana.Admin.BLL.Currencies currencies = RMAdminBusinessLogic.GetCurrencies();

                Company company = CompanyManager.GetCompany(_companyID);
                int companyBaseCurrencyID = company.BaseCurrencyID;

                //Inserting the - Select - option in the Combo Box at the top.
                currencies.Insert(0, new Currency(int.MinValue, C_COMBO_SELECT, C_COMBO_SELECT));
                this.cmbRMBaseCurrency.DataSource = currencies;
                this.cmbRMBaseCurrency.DisplayMember = "CurrencySymbol";
                this.cmbRMBaseCurrency.ValueMember = "CurencyID";
                //this.cmbRMBaseCurrency.Value = int.MinValue;
                if (companyBaseCurrencyID != int.MinValue)
                {
                    this.cmbRMBaseCurrency.Value = companyBaseCurrencyID;
                }
                else
                {
                    this.cmbRMBaseCurrency.Text = C_COMBO_SELECT;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        #endregion

        #region Validation Check 
        /// <summary>
        /// To check the validation for the controls used in the usercontrol CompanyOverallLimits
        /// </summary>
        /// <returns></returns>
        public bool ValidateControl()
        {
            bool validationSuccess = true;

            errorProvider1.SetError(cmbRMBaseCurrency, "");
            errorProvider1.SetError(spnCalRiskLimit, "");
            errorProvider1.SetError(txtExposurelimit, "");
            errorProvider1.SetError(txtpositivePNLLimit, "");
            errorProvider1.SetError(txtnegativePNLLimit, "");

            if (int.Parse(cmbRMBaseCurrency.Value.ToString()) == int.MinValue)
            {
                cmbRMBaseCurrency.Text = C_COMBO_SELECT;
                errorProvider1.SetError(cmbRMBaseCurrency, "Please select Currency Type!");
                validationSuccess = false;
                cmbRMBaseCurrency.Focus();
            }
            else if (int.Parse(spnCalRiskLimit.Value.ToString()) == 0)
            {
                spnCalRiskLimit.Value = 0;
                errorProvider1.SetError(spnCalRiskLimit, "Please select RiskLimit!");
                validationSuccess = false;
                spnCalRiskLimit.Focus();
            }
            else if (!DataTypeValidation.ValidateNumeric(txtExposurelimit.Text.Trim()))
            {
                txtExposurelimit.Text = "";
                errorProvider1.SetError(txtExposurelimit, "Please enter numeric values for Exposure Limit!");
                validationSuccess = false;
                txtExposurelimit.Focus();
            }
            else if (txtpositivePNLLimit.Text != "")
            {
                if (!DataTypeValidation.ValidateNumeric(txtpositivePNLLimit.Text.Trim()))
                {
                    txtpositivePNLLimit.Text = "";
                    errorProvider1.SetError(txtpositivePNLLimit, "Please enter numeric values for Positive PNL Limit!");
                    txtpositivePNLLimit.Focus();
                    validationSuccess = false;
                }
            }
            if (txtnegativePNLLimit.Text != "")
            {
                if (!DataTypeValidation.ValidateNumeric(txtnegativePNLLimit.Text.Trim()))
                {
                    txtnegativePNLLimit.Text = "";
                    errorProvider1.SetError(txtnegativePNLLimit, "Please enter numeric values for Negative PNL Limit!");
                    //validationSuccess = false;
                    txtnegativePNLLimit.Focus();
                    validationSuccess = false;
                }
            }

            return validationSuccess;
        }

        #endregion Validation Check

        #region Save Method 

        /// <summary>
        /// This method saves the CompanyOverallLimit detail in the database.
        /// </summary>
        /// <param name="companyOverallLimit"></param>
        /// <returns>Returns 1 if saved successfully.</returns>
        public void SaveCompanyOverallLimit(CompanyOverallLimit companyOverallLimit, int _companyID)
        {
            bool IsValid = ValidateControl();

            errorProvider1.SetError(txtpositivePNLLimit, "");
            errorProvider1.SetError(txtnegativePNLLimit, "");

            if (IsValid)
            {
                // Data as input by user is assigned to the respective fields for saving to DB.
                companyOverallLimit.RMBaseCurrencyID = int.Parse(cmbRMBaseCurrency.Value.ToString());
                companyOverallLimit.CalculateRiskLimit = int.Parse(spnCalRiskLimit.Value.ToString());
                companyOverallLimit.ExposureLimit = Convert.ToInt64(txtExposurelimit.Text.Trim().ToString());

                // As the Positive PNL is an optional input for user, so the validation for same is checked at 
                //the save level.
                // So, if the user has entered some data, we check for validation , otherwise skip it,
                //so, no else statement required.
                if (txtpositivePNLLimit.Text != "")
                {
                    companyOverallLimit.PositivePNL = int.Parse(txtpositivePNLLimit.Text.Trim().ToString());
                }
                if (txtnegativePNLLimit.Text != "")
                {
                    companyOverallLimit.NegativePNL = int.Parse(txtnegativePNLLimit.Text.Trim().ToString());
                }

                // Save method is called from the BLL i.e RMAdminBusinessLogic which inturn calls it from DAL.
                RMAdminBusinessLogic.SaveCompanyOverallLimit(companyOverallLimit, _companyID);
                RMAdminBusinessLogic.VerifySubLevelExpLtValidity(_companyID);
            }
            else
            {
                // since the data did not pass the validation it did not get saved.
            }
        }

        #endregion Save Method

        #region ChangeInExpLt

        private void DecreasingCompanyExpLt(int companyID, int newExpLt)
        {
            CompanyOverallLimit companyOverallLimit = RMAdminBusinessLogic.GetCompanyOverallLimit(companyID);
            if (companyOverallLimit != null)
            {
                if (companyOverallLimit.ExposureLimit > newExpLt)
                {
                    if (MessageBox.Show(this, "Reducing the Exposure limit will affect the sub levels also.The Exposure Limit at any sub level,exceeding the new exposure limit, will be made equal to the company Exposure Limit. Do you still want to continue?", "RM ADMIN Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //continue with new ExpLt.
                    }
                    else
                    {
                        txtExposurelimit.Text = companyOverallLimit.ExposureLimit.ToString();
                    }
                }
            }
        }

        #endregion ChangeInExpLt

        #region Refresh Method

        /// <summary>
		/// Blanks all the textboxes in the form. 
		/// </summary>		
		private void RefreshCompanyOverallLimitDetail()
        {
            try
            {
                //cmbRMBaseCurrency.Text =C_COMBO_SELECT;
                txtExposurelimit.Text = "";
                txtpositivePNLLimit.Text = "";
                txtnegativePNLLimit.Text = "";
                spnCalRiskLimit.Value = 0;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion Refresh Method

        #region SetMethod

        //CompanyOverallLimit property sets the CompanyOverallLimit form by displaying the data for the 
        //selected company in the controls on the CompanyOverallLimits Tab page.	
        public CompanyOverallLimit SetCompanyOverallLimit
        {
            set { SettingCompanyOverallLimit(value); }
        }

        /// <summary>
        /// Shows all the details in the respective controls pertaining to that paricular company.
        /// </summary>
        /// <param name="companyID"></param>
        private void SettingCompanyOverallLimit(CompanyOverallLimit companyOverallLimit)
        {
            try
            {
                BindCurrency();

                SetCurrencyLabels();
                this.spnCalRefreshRate.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
                //spnCalRefreshRate. ------Commented by Bhupesh
                /* Before setting the data, we check whether the Object is not null as well as 
                   whether the selected company ID not the Root node of the RMTree and that it shud be valid 
                   company from amongst the list in tree
                   otherwise set the controls as blank for entering fresh data0-0 */
                if (companyOverallLimit != null && companyOverallLimit.CompanyID != int.MinValue)
                {
                    cmbRMBaseCurrency.Value = int.Parse(companyOverallLimit.RMBaseCurrencyID.ToString());
                    spnCalRiskLimit.Value = int.Parse(companyOverallLimit.CalculateRiskLimit.ToString());
                    txtExposurelimit.Text = companyOverallLimit.ExposureLimit.ToString();

                    txtnegativePNLLimit.Text = companyOverallLimit.NegativePNL.ToString();

                    /* Again as the Positive PNL is optional data...so, we first check before setting, 
                     whether there is a valid data for the Positive PNL and according set the data in textbox
                     or set it as empty */
                    if (companyOverallLimit.PositivePNL != int.MinValue)
                    {
                        txtpositivePNLLimit.Text = companyOverallLimit.PositivePNL.ToString();
                    }
                    else
                    {
                        txtpositivePNLLimit.Text = "";
                    }
                }
                else
                {
                    //cmbRMBaseCurrency.Value = C_COMBO_SELECT;
                    RefreshCompanyOverallLimitDetail();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }




        #endregion SetMethod

        #region RMCurrencyLabels

        /// <summary>
        /// The method sets the RMCurrency Labels default properties. 
        /// </summary>
        private void SetCurrencyLabels()
        {
            try
            {
                if (this.cmbRMBaseCurrency.Text.ToString() != C_COMBO_SELECT)
                {
                    this.UneditExpLt.Text = cmbRMBaseCurrency.Text;
                    this.UneditExpLt.AutoSize = true;
                    this.UneditnegPNL.Text = cmbRMBaseCurrency.Text;
                    this.UneditnegPNL.AutoSize = true;
                    this.UneditposPNL.Text = cmbRMBaseCurrency.Text;
                    this.UneditposPNL.AutoSize = true;
                }
                else
                {
                    this.UneditExpLt.Text = "";
                    this.UneditExpLt.AutoSize = true;
                    this.UneditnegPNL.Text = "";
                    this.UneditnegPNL.AutoSize = true;
                    this.UneditposPNL.Text = "";
                    this.UneditposPNL.AutoSize = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion RMCurrencyLabels

        #region Focus Color

        private void cmbRMBaseCurrency_Enter(object sender, EventArgs e)
        {
            cmbRMBaseCurrency.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void cmbRMBaseCurrency_Leave(object sender, EventArgs e)
        {
            cmbRMBaseCurrency.Appearance.BackColor = Color.White;
        }

        private void spnCalRiskLimit_Enter(object sender, EventArgs e)
        {
            spnCalRiskLimit.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnCalRiskLimit_Leave(object sender, EventArgs e)
        {
            spnCalRiskLimit.BackColor = Color.White;
        }

        private void txtExposurelimit_Enter(object sender, EventArgs e)
        {
            txtExposurelimit.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtExposurelimit_Leave(object sender, EventArgs e)
        {
            int txt = 0;
            try
            {
                txtExposurelimit.BackColor = Color.White;
                if (!string.IsNullOrEmpty(txtExposurelimit.Text))
                {
                    if (int.TryParse(txtExposurelimit.Text, out txt))
                    {
                        txt = Convert.ToInt32(txtExposurelimit.Text);
                    }
                }
                DecreasingCompanyExpLt(_companyID, txt);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtpositivePNLLimit_Leave(object sender, EventArgs e)
        {
            txtpositivePNLLimit.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtpositivePNLLimit_Enter(object sender, EventArgs e)
        {
            txtpositivePNLLimit.BackColor = Color.White;
        }

        private void txtnegativePNLLimit_Enter(object sender, EventArgs e)
        {
            txtnegativePNLLimit.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtnegativePNLLimit_Leave(object sender, EventArgs e)
        {
            txtnegativePNLLimit.BackColor = Color.White;
        }


        #endregion

        #region Passing the value from CurrencyCombo to Labels next to textboxes

        /// <summary>
        /*  This event causes :
          1)the selected currency in combo box to be displayed in the labels 
          next to textboxes on the same control as well as other controls used in the main form.
          2)the same currency is to be displayed on other usercontrols also, so, the same is 
          passed to other usercontrols using events and delegates.
          3)depending upon the selected currency symbol, the Currency Rates Grid is also populated. */
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbRMBaseCurrency_ValueChanged(object sender, System.EventArgs e)
        {
            string str;
            try
            {
                // Assign the text i.e currency symbol selected in the currency drop down to the string "str".
                str = " " + this.cmbRMBaseCurrency.Text + " ";


                if (cmbRMBaseCurrency.Value != null)
                {
                    // Check that the selected text should not be the "-select-", 
                    //it should be a valid currency symbol listed  in dropdown.
                    if (cmbRMBaseCurrency.Text != C_COMBO_SELECT)
                    {
                        // Assign the Currency ID of the selected currency Symbol to the "currencyID"
                        int currencyID = int.Parse(cmbRMBaseCurrency.Value.ToString());

                        // The BindCurrencyGrid method is called , and the selected currencyId is passed as
                        // parameter for the grid to be populated.
                        BindCurrencyGrid(currencyID);

                        // Also, the "str" is further assigned to the labels next to Textboxes to display the RM Base Currency.
                        UneditExpLt.Text = str;
                        UneditnegPNL.Text = str;
                        UneditposPNL.Text = str;

                        // Through this eventhandler, we pass the selected RM base Currency symbol to other
                        // usercontrols also.
                        PassValueEventArgs passValueEventArgs = new PassValueEventArgs();
                        passValueEventArgs.rMCurrencySymbol = this.cmbRMBaseCurrency.Text;
                        passValueEventArgs.rMCurrencyID = int.Parse(this.cmbRMBaseCurrency.Value.ToString());

                        if (CurrencyChanged != null)
                        {
                            CurrencyChanged(this, passValueEventArgs);
                        }
                    }
                    // else none of the labels is visible until a currency is selected in the dropdown
                    // as the labels property Autosize is set as true.
                    else
                    {
                        str = "";
                        UneditExpLt.Text = str;
                        UneditnegPNL.Text = str;
                        UneditposPNL.Text = str;

                        // this is to show the Currency Rates grid as blank until a RM Base currency is selected.
                        RMCurrencyRates rmCurrencyRates = new RMCurrencyRates();

                        // here a blank row is entered.
                        rmCurrencyRates.Add(new RMCurrencyRate("", "", "", ""));
                        // and the datasource is assigned to the grid.
                        grdCurrency.DataSource = rmCurrencyRates;

                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region BindCurrencyGrid

        /// <summary>
        /// This method is used to bind The curency grid on the usercontrol as per the selected currency in the 
        /// combo for currency.
        /// </summary>
        /// <param name="currencyID"></param>
        private void BindCurrencyGrid(int currencyID)
        {
            // GetRMCurrencyConversion method is called from BLL of RM.
            RMCurrencyRates rMCurrencyRates = RMAdminBusinessLogic.GetRMCurrencyConversion(currencyID);
            try
            {
                if (rMCurrencyRates.Count > 0)
                {

                    RMCurrencyRates newRMCurrencyRates = new RMCurrencyRates();

                    // The loop iterates through the rows in data collection retrieved from the database
                    // and obtains the Text Name/ Symbol of the CurrencyID to be displayed in the datagrid. 
                    foreach (RMCurrencyRate rMCurrencyRate in rMCurrencyRates)
                    {
                        // Assign the CurrencyID of the FromCurrencyID to "fromCurrency"
                        int fromCurrency = int.Parse(rMCurrencyRate.FromCurrencyID.ToString());

                        // GetCurrency method is called from BLL to get the details regarding the assigned 
                        // CurrencyID above as "fromCurrency".
                        Currency currency = RMAdminBusinessLogic.GetCurrency(fromCurrency);

                        // Above instance now gives the name of the currency and is assigned to the string "rMBaseCurrency".
                        string rMBaseCurrency = currency.CurrencyName.ToString();

                        // the value of string "rMBaseCurrency" is assigned RMBaseCurrency Field.
                        rMCurrencyRate.RMBaseCurrency = rMBaseCurrency.ToString();

                        // Similarly, again, the ID is assigned to "toCurrency" which is used as parameter 
                        // while calling the GetCurrency method from BLL.
                        int toCurrency = int.Parse(rMCurrencyRate.ToCurrency.ToString());
                        Currency newcurrency = RMAdminBusinessLogic.GetCurrency(toCurrency);

                        // Name of ToCurrency assigned to "allOtherCurrencies".
                        string allOtherCurrencies = newcurrency.CurrencyName.ToString();

                        // the value of string "allOtherCurrencies" is assigned to AllOtherCurrencies 
                        rMCurrencyRate.AllOtherCurrencies = allOtherCurrencies.ToString();

                        // this adds the object rows in the collection.
                        newRMCurrencyRates.Add(rMCurrencyRate);
                    }

                    //Assigning DataSource to Grid.
                    grdCurrency.DataSource = rMCurrencyRates;
                }
                else
                {

                    // this is to show the Currency Rates grid as blank when such an RM Base currency is selected
                    // that does not have data in table T_RMCurrencyRates.
                    RMCurrencyRates rmCurrencyRates = new RMCurrencyRates();

                    // here a blank row is entered.
                    rmCurrencyRates.Add(new RMCurrencyRate("", "", "", ""));
                    // and the datasource is assigned to the grid.
                    grdCurrency.DataSource = rmCurrencyRates;
                }

                // This sets the selected columns as hidden in the grid.
                ColumnsCollection columns5 = grdCurrency.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns5)
                {
                    if (column.Key == "FromCurrencyID" || column.Key == "ToCurrency")
                    {
                        column.Hidden = true;
                    }

                }

                // This method is called to ensure that header positon in grid is fixed as per set format.
                FixHeaderPosition();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method is used to set the properties such as Visisble postion and appearance of the headers in the grid as fixed.
        /// </summary>
        private void FixHeaderPosition()
        {
            try
            {
                grdCurrency.DisplayLayout.Bands[0].Columns["RMBaseCurrency"].Header.VisiblePosition = 0;
                grdCurrency.DisplayLayout.Bands[0].Columns["AllOtherCurrencies"].Header.VisiblePosition = 1;
                grdCurrency.DisplayLayout.Bands[0].Columns["Conversion"].Header.VisiblePosition = 2;
                grdCurrency.DisplayLayout.Bands[0].Columns["CurrencyRates"].Header.VisiblePosition = 3;

                grdCurrency.DisplayLayout.Bands[0].Columns["RMBaseCurrency"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                grdCurrency.DisplayLayout.Bands[0].Columns["AllOtherCurrencies"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                grdCurrency.DisplayLayout.Bands[0].Columns["Conversion"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                grdCurrency.DisplayLayout.Bands[0].Columns["CurrencyRates"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion BindCurrencyGrid

        #region To check the Valid Maximum permitted numeric value.
        /// <summary>
        /// the event and the following events are raised to ensure that the entered text is only numeric type and within the permitted values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExposurelimit_TextChanged(object sender, EventArgs e)
        {
            try
            {
                errorProvider1.SetError(txtExposurelimit, "");

                if (txtExposurelimit.Text != "")
                {
                    if (!DataTypeValidation.ValidateNumeric(txtExposurelimit.Text.Trim()))
                    {
                        errorProvider1.SetError(txtExposurelimit, "Please enter numeric values for Exposure Limit!");
                    }
                    else if (!DataTypeValidation.ValidateMaxPermiitedNumeric(txtExposurelimit.Text))
                    {
                        errorProvider1.SetError(txtExposurelimit, "You cannot enter a value greater than 9223372036854775807!");
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtpositivePNLLimit_TextChanged(object sender, EventArgs e)
        {
            try
            {
                errorProvider1.SetError(txtpositivePNLLimit, "");
                if (txtpositivePNLLimit.Text != "")
                {
                    if (!DataTypeValidation.ValidateNumeric(txtpositivePNLLimit.Text.Trim()))
                    {
                        errorProvider1.SetError(txtnegativePNLLimit, "Please enter numeric values for positive PNL Limit!");
                    }
                    else if (!DataTypeValidation.ValidateMaxPermiitedNumeric(txtpositivePNLLimit.Text))
                    {
                        errorProvider1.SetError(txtpositivePNLLimit, " You cannot enter a value greater than 9223372036854775807!");
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtnegativePNLLimit_TextChanged(object sender, EventArgs e)
        {
            try
            {
                errorProvider1.SetError(txtnegativePNLLimit, "");
                if (txtnegativePNLLimit.Text != "")
                {
                    if (!DataTypeValidation.ValidateNumeric(txtnegativePNLLimit.Text.Trim()))
                    {
                        errorProvider1.SetError(txtnegativePNLLimit, "Please enter numeric values for Negative PNL Limit!");
                    }
                    else if (!DataTypeValidation.ValidateMaxPermiitedNumeric(txtnegativePNLLimit.Text))
                    {
                        errorProvider1.SetError(txtnegativePNLLimit, " You cannot enter a value greater than 9223372036854775807!");
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion To check the Valid Maximum permitted numeric value.

    }

    #region PassValueEventArgs class
    /// <summary>
    /// This class is created for the event used to pass the selected RM Base currency on other Controls on Main Form.
    /// </summary>
    public class PassValueEventArgs : System.EventArgs
    {
        private String str;
        private int _value = int.MinValue;
        public String rMCurrencySymbol
        {
            get
            {
                return (str);
            }
            set
            {
                str = value;
            }
        }  // currencySymbol

        public int rMCurrencyID
        {
            get
            {
                return (_value);
            }
            set
            {
                _value = value;
            }
        }
    }  // PassValueEventArgs
    #endregion

}

