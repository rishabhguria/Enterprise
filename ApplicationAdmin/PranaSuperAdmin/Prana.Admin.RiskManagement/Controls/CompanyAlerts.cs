#region Using
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;
using Prana.Admin.BLL;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
#endregion

namespace Prana.Admin.RiskManagement.Controls
{
    /// <summary>
    /// Summary description for CompanyAlerts.
    /// </summary>
    public class Company_Alerts : System.Windows.Forms.UserControl
    {
        #region Wizard stuff
        const string C_COMBO_SELECT = "- Select -";
        private const string FORM_NAME = "COMPANYALERTS : ";

        //private bool _isSecondRowEnable = false;
        //private bool _isThirdRowEnable = false;
        //private bool _isFourthRowEnable = false;

        #region private and protected members

        private int _companyID = int.MinValue;
        private System.Windows.Forms.ErrorProvider errorProvider1;

        #endregion

        #region private members
        private Infragistics.Win.Misc.UltraGroupBox grpCompanyAlerts;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel lblCalculationRefreshRate;
        private Infragistics.Win.Misc.UltraLabel lblTo;
        private Infragistics.Win.Misc.UltraLabel lblFrom;
        private Infragistics.Win.Misc.UltraLabel lblBlkTrading;
        private Infragistics.Win.Misc.UltraLabel lblEmail;
        private Infragistics.Win.Misc.UltraLabel lblAlertMessage;
        private Infragistics.Win.Misc.UltraLabel lblNotify;
        private Infragistics.Win.Misc.UltraLabel lblExposureLimitsAlerts;
        private Infragistics.Win.Misc.UltraGroupBox grpBxDefault;
        private Infragistics.Win.Misc.UltraLabel lblCalRefreshRateGrp;
        private Infragistics.Win.Misc.UltraLabel lblBeforeEachTradeGrp;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet chkDefaultAlertType;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBlockTrade4;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBlockTrade3;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBlockTrade2;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBlockTrade1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtEmailAlert4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtEmailAlert3;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtEmailAlert2;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtEmailAlert1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAlertMsg4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAlertMsg3;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAlertMsg2;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAlertMsg1;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet chkAlertType3;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet chkAlertType4;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet chkAlertType2;
        private Infragistics.Win.Misc.UltraLabel lblAfterEvrySecGrp;
        private Spinner spnExpoLimit1;
        private Spinner spnExpLimitto4;
        private Spinner spnExpoLimit4;
        private Spinner spnExpoLimit3;
        private Spinner spnExpoLimit2;
        private Spinner spnExpLimitto1;
        private Spinner spnExpLimitto3;
        private Spinner spnExpLimitto2;
        private UltraLabel ultraLabel8;
        private Spinner spnCalRefRate1;
        private Spinner spnCalRefRate2;
        private Spinner spnCalRefRate3;
        private Spinner spnCalRefRate4;
        private Spinner spnCalRefRateGrpbx;
        private UltraLabel ultraLabel9;
        private UltraOptionSet chkAlertType1;
        private UltraLabel ultraLabel1;
        private UltraLabel ultraLabel10;

        private IContainer components;

        #endregion private members

        public int CompanyID
        {
            set { _companyID = value; }
        }


        public Company_Alerts()
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
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (grpCompanyAlerts != null)
                {
                    grpCompanyAlerts.Dispose();
                }
                if (ultraLabel7 != null)
                {
                    ultraLabel7.Dispose();
                }
                if (ultraLabel6 != null)
                {
                    ultraLabel6.Dispose();
                }
                if (ultraLabel5 != null)
                {
                    ultraLabel5.Dispose();
                }
                if (ultraLabel4 != null)
                {
                    ultraLabel4.Dispose();
                }
                if (lblCalculationRefreshRate != null)
                {
                    lblCalculationRefreshRate.Dispose();
                }
                if (lblTo != null)
                {
                    lblTo.Dispose();
                }
                if (lblFrom != null)
                {
                    lblFrom.Dispose();
                }
                if (lblBlkTrading != null)
                {
                    lblBlkTrading.Dispose();
                }
                if (lblEmail != null)
                {
                    lblEmail.Dispose();
                }
                if (lblAlertMessage != null)
                {
                    lblAlertMessage.Dispose();
                }
                if (lblNotify != null)
                {
                    lblNotify.Dispose();
                }
                if (lblExposureLimitsAlerts != null)
                {
                    lblExposureLimitsAlerts.Dispose();
                }
                if (grpBxDefault != null)
                {
                    grpBxDefault.Dispose();
                }
                if (lblCalRefreshRateGrp != null)
                {
                    lblCalRefreshRateGrp.Dispose();
                }
                if (lblBeforeEachTradeGrp != null)
                {
                    lblBeforeEachTradeGrp.Dispose();
                }
                if (ultraLabel2 != null)
                {
                    ultraLabel2.Dispose();
                }
                if (chkDefaultAlertType != null)
                {
                    chkDefaultAlertType.Dispose();
                }
                if (chkBlockTrade4 != null)
                {
                    chkBlockTrade4.Dispose();
                }
                if (chkBlockTrade3 != null)
                {
                    chkBlockTrade3.Dispose();
                }
                if (chkBlockTrade2 != null)
                {
                    chkBlockTrade2.Dispose();
                }
                if (chkBlockTrade1 != null)
                {
                    chkBlockTrade1.Dispose();
                }
                if (txtEmailAlert4 != null)
                {
                    txtEmailAlert4.Dispose();
                }
                if (txtEmailAlert3 != null)
                {
                    txtEmailAlert3.Dispose();
                }
                if (txtEmailAlert2 != null)
                {
                    txtEmailAlert2.Dispose();
                }
                if (txtEmailAlert1 != null)
                {
                    txtEmailAlert1.Dispose();
                }
                if (txtAlertMsg4 != null)
                {
                    txtAlertMsg4.Dispose();
                }
                if (txtAlertMsg3 != null)
                {
                    txtAlertMsg3.Dispose();
                }
                if (txtAlertMsg2 != null)
                {
                    txtAlertMsg2.Dispose();
                }
                if (txtAlertMsg1 != null)
                {
                    txtAlertMsg1.Dispose();
                }
                if (chkAlertType3 != null)
                {
                    chkAlertType3.Dispose();
                }
                if (chkAlertType4 != null)
                {
                    chkAlertType4.Dispose();
                }
                if (chkAlertType2 != null)
                {
                    chkAlertType2.Dispose();
                }
                if (lblAfterEvrySecGrp != null)
                {
                    lblAfterEvrySecGrp.Dispose();
                }
                if (spnExpoLimit1 != null)
                {
                    spnExpoLimit1.Dispose();
                }
                if (spnExpLimitto4 != null)
                {
                    spnExpLimitto4.Dispose();
                }
                if (spnExpoLimit4 != null)
                {
                    spnExpoLimit4.Dispose();
                }
                if (spnExpoLimit3 != null)
                {
                    spnExpoLimit3.Dispose();
                }
                if (spnExpoLimit2 != null)
                {
                    spnExpoLimit2.Dispose();
                }
                if (spnExpLimitto1 != null)
                {
                    spnExpLimitto1.Dispose();
                }
                if (spnExpLimitto3 != null)
                {
                    spnExpLimitto3.Dispose();
                }
                if (spnExpLimitto2 != null)
                {
                    spnExpLimitto2.Dispose();
                }
                if (ultraLabel8 != null)
                {
                    ultraLabel8.Dispose();
                }
                if (spnCalRefRate1 != null)
                {
                    spnCalRefRate1.Dispose();
                }
                if (spnCalRefRate2 != null)
                {
                    spnCalRefRate2.Dispose();
                }
                if (spnCalRefRate3 != null)
                {
                    spnCalRefRate3.Dispose();
                }
                if (spnCalRefRate4 != null)
                {
                    spnCalRefRate4.Dispose();
                }
                if (spnCalRefRateGrpbx != null)
                {
                    spnCalRefRateGrpbx.Dispose();
                }
                if (ultraLabel9 != null)
                {
                    ultraLabel9.Dispose();
                }
                if (chkAlertType1 != null)
                {
                    chkAlertType1.Dispose();
                }
                if (ultraLabel1 != null)
                {
                    ultraLabel1.Dispose();
                }
                if (ultraLabel10 != null)
                {
                    ultraLabel10.Dispose();
                }

            }
            base.Dispose(disposing);
        }
        #endregion Wizard stuff

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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
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
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpCompanyAlerts = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
            this.chkAlertType1 = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.spnCalRefRate1 = new Spinner();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.spnCalRefRate2 = new Spinner();
            this.spnCalRefRate3 = new Spinner();
            this.spnExpoLimit4 = new Spinner();
            this.spnCalRefRate4 = new Spinner();
            this.spnExpLimitto4 = new Spinner();
            this.spnExpoLimit3 = new Spinner();
            this.spnExpLimitto1 = new Spinner();
            this.spnExpoLimit2 = new Spinner();
            this.spnExpLimitto3 = new Spinner();
            this.spnExpoLimit1 = new Spinner();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.spnExpLimitto2 = new Spinner();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCalculationRefreshRate = new Infragistics.Win.Misc.UltraLabel();
            this.lblTo = new Infragistics.Win.Misc.UltraLabel();
            this.lblFrom = new Infragistics.Win.Misc.UltraLabel();
            this.lblBlkTrading = new Infragistics.Win.Misc.UltraLabel();
            this.lblEmail = new Infragistics.Win.Misc.UltraLabel();
            this.lblAlertMessage = new Infragistics.Win.Misc.UltraLabel();
            this.lblNotify = new Infragistics.Win.Misc.UltraLabel();
            this.lblExposureLimitsAlerts = new Infragistics.Win.Misc.UltraLabel();
            this.grpBxDefault = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel();
            this.spnCalRefRateGrpbx = new Spinner();
            this.lblAfterEvrySecGrp = new Infragistics.Win.Misc.UltraLabel();
            this.lblCalRefreshRateGrp = new Infragistics.Win.Misc.UltraLabel();
            this.lblBeforeEachTradeGrp = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.chkDefaultAlertType = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.chkBlockTrade4 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkBlockTrade3 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkBlockTrade2 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkBlockTrade1 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtEmailAlert4 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtEmailAlert3 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtEmailAlert2 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtEmailAlert1 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtAlertMsg4 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtAlertMsg3 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtAlertMsg2 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtAlertMsg1 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.chkAlertType3 = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.chkAlertType4 = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.chkAlertType2 = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCompanyAlerts)).BeginInit();
            this.grpCompanyAlerts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkAlertType1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxDefault)).BeginInit();
            this.grpBxDefault.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkDefaultAlertType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailAlert4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailAlert3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailAlert2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailAlert1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlertMsg4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlertMsg3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlertMsg2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlertMsg1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAlertType3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAlertType4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAlertType2)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grpCompanyAlerts
            // 
            this.grpCompanyAlerts.Controls.Add(this.ultraLabel1);
            this.grpCompanyAlerts.Controls.Add(this.ultraLabel9);
            this.grpCompanyAlerts.Controls.Add(this.chkAlertType1);
            this.grpCompanyAlerts.Controls.Add(this.spnCalRefRate1);
            this.grpCompanyAlerts.Controls.Add(this.ultraLabel8);
            this.grpCompanyAlerts.Controls.Add(this.spnCalRefRate2);
            this.grpCompanyAlerts.Controls.Add(this.spnCalRefRate3);
            this.grpCompanyAlerts.Controls.Add(this.spnExpoLimit4);
            this.grpCompanyAlerts.Controls.Add(this.spnCalRefRate4);
            this.grpCompanyAlerts.Controls.Add(this.spnExpLimitto4);
            this.grpCompanyAlerts.Controls.Add(this.spnExpoLimit3);
            this.grpCompanyAlerts.Controls.Add(this.spnExpLimitto1);
            this.grpCompanyAlerts.Controls.Add(this.spnExpoLimit2);
            this.grpCompanyAlerts.Controls.Add(this.spnExpLimitto3);
            this.grpCompanyAlerts.Controls.Add(this.spnExpoLimit1);
            this.grpCompanyAlerts.Controls.Add(this.ultraLabel7);
            this.grpCompanyAlerts.Controls.Add(this.spnExpLimitto2);
            this.grpCompanyAlerts.Controls.Add(this.ultraLabel6);
            this.grpCompanyAlerts.Controls.Add(this.ultraLabel5);
            this.grpCompanyAlerts.Controls.Add(this.ultraLabel4);
            this.grpCompanyAlerts.Controls.Add(this.lblCalculationRefreshRate);
            this.grpCompanyAlerts.Controls.Add(this.lblTo);
            this.grpCompanyAlerts.Controls.Add(this.lblFrom);
            this.grpCompanyAlerts.Controls.Add(this.lblBlkTrading);
            this.grpCompanyAlerts.Controls.Add(this.lblEmail);
            this.grpCompanyAlerts.Controls.Add(this.lblAlertMessage);
            this.grpCompanyAlerts.Controls.Add(this.lblNotify);
            this.grpCompanyAlerts.Controls.Add(this.lblExposureLimitsAlerts);
            this.grpCompanyAlerts.Controls.Add(this.grpBxDefault);
            this.grpCompanyAlerts.Controls.Add(this.chkBlockTrade4);
            this.grpCompanyAlerts.Controls.Add(this.chkBlockTrade3);
            this.grpCompanyAlerts.Controls.Add(this.chkBlockTrade2);
            this.grpCompanyAlerts.Controls.Add(this.chkBlockTrade1);
            this.grpCompanyAlerts.Controls.Add(this.txtEmailAlert4);
            this.grpCompanyAlerts.Controls.Add(this.txtEmailAlert3);
            this.grpCompanyAlerts.Controls.Add(this.txtEmailAlert2);
            this.grpCompanyAlerts.Controls.Add(this.txtEmailAlert1);
            this.grpCompanyAlerts.Controls.Add(this.txtAlertMsg4);
            this.grpCompanyAlerts.Controls.Add(this.txtAlertMsg3);
            this.grpCompanyAlerts.Controls.Add(this.txtAlertMsg2);
            this.grpCompanyAlerts.Controls.Add(this.txtAlertMsg1);
            this.grpCompanyAlerts.Controls.Add(this.chkAlertType3);
            this.grpCompanyAlerts.Controls.Add(this.chkAlertType4);
            this.grpCompanyAlerts.Controls.Add(this.chkAlertType2);
            this.grpCompanyAlerts.Location = new System.Drawing.Point(4, 3);
            this.grpCompanyAlerts.Name = "grpCompanyAlerts";
            this.grpCompanyAlerts.Size = new System.Drawing.Size(677, 226);
            this.grpCompanyAlerts.SupportThemes = false;
            this.grpCompanyAlerts.TabIndex = 116;
            // 
            // ultraLabel1
            // 
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance1.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraLabel1.Appearance = appearance1;
            this.ultraLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel1.Location = new System.Drawing.Point(494, 43);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(79, 15);
            this.ultraLabel1.TabIndex = 177;
            this.ultraLabel1.Text = "Send E-Mail To";
            // 
            // ultraLabel9
            // 
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance2.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraLabel9.Appearance = appearance2;
            this.ultraLabel9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel9.Location = new System.Drawing.Point(179, 43);
            this.ultraLabel9.Name = "ultraLabel9";
            this.ultraLabel9.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel9.TabIndex = 176;
            this.ultraLabel9.Text = "/";
            // 
            // chkAlertType1
            // 
            this.chkAlertType1.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.chkAlertType1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkAlertType1.ItemAppearance = appearance3;
            valueListItem1.DataValue = "";
            valueListItem1.DisplayText = "";
            valueListItem2.DataValue = "";
            this.chkAlertType1.Items.Add(valueListItem1);
            this.chkAlertType1.Items.Add(valueListItem2);
            this.chkAlertType1.ItemSpacingHorizontal = 40;
            this.chkAlertType1.Location = new System.Drawing.Point(138, 69);
            this.chkAlertType1.Name = "chkAlertType1";
            this.chkAlertType1.Size = new System.Drawing.Size(76, 20);
            this.chkAlertType1.TabIndex = 175;
            this.chkAlertType1.Click += new System.EventHandler(this.chkAlertType1_Click);
            this.chkAlertType1.ValueChanged += new System.EventHandler(this.chkAlertType1_ValueChanged);
            this.chkAlertType1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkAlertType1_MouseUp);
            // 
            // spnCalRefRate1
            // 
            this.spnCalRefRate1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnCalRefRate1.DataType = DataTypes.Numeric;
            this.spnCalRefRate1.Increment = 1;
            this.spnCalRefRate1.Location = new System.Drawing.Point(231, 69);
            this.spnCalRefRate1.MaxValue = 60;
            this.spnCalRefRate1.MinValue = 0;
            this.spnCalRefRate1.Name = "spnCalRefRate1";
            this.spnCalRefRate1.Size = new System.Drawing.Size(45, 20);
            this.spnCalRefRate1.TabIndex = 166;
            this.spnCalRefRate1.Value = 0;
            this.spnCalRefRate1.Enter += new System.EventHandler(this.spnCalRefRate1_Enter);
            this.spnCalRefRate1.Leave += new System.EventHandler(this.spnCalRefRate1_Leave);
            // 
            // ultraLabel8
            // 
            appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance4.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraLabel8.Appearance = appearance4;
            this.ultraLabel8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel8.Location = new System.Drawing.Point(119, 35);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(71, 31);
            this.ultraLabel8.TabIndex = 165;
            this.ultraLabel8.Text = "Before EachTrade";
            // 
            // spnCalRefRate2
            // 
            this.spnCalRefRate2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnCalRefRate2.DataType = DataTypes.Numeric;
            this.spnCalRefRate2.Increment = 1;
            this.spnCalRefRate2.Location = new System.Drawing.Point(231, 94);
            this.spnCalRefRate2.MaxValue = 60;
            this.spnCalRefRate2.MinValue = 0;
            this.spnCalRefRate2.Name = "spnCalRefRate2";
            this.spnCalRefRate2.Size = new System.Drawing.Size(45, 20);
            this.spnCalRefRate2.TabIndex = 167;
            this.spnCalRefRate2.Value = 0;
            this.spnCalRefRate2.Enter += new System.EventHandler(this.spnCalRefRate2_Enter);
            this.spnCalRefRate2.Leave += new System.EventHandler(this.spnCalRefRate2_Leave);
            // 
            // spnCalRefRate3
            // 
            this.spnCalRefRate3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnCalRefRate3.DataType = DataTypes.Numeric;
            this.spnCalRefRate3.Increment = 1;
            this.spnCalRefRate3.Location = new System.Drawing.Point(231, 119);
            this.spnCalRefRate3.MaxValue = 60;
            this.spnCalRefRate3.MinValue = 0;
            this.spnCalRefRate3.Name = "spnCalRefRate3";
            this.spnCalRefRate3.Size = new System.Drawing.Size(45, 20);
            this.spnCalRefRate3.TabIndex = 168;
            this.spnCalRefRate3.Value = 0;
            this.spnCalRefRate3.Enter += new System.EventHandler(this.spnCalRefRate3_Enter);
            this.spnCalRefRate3.Leave += new System.EventHandler(this.spnCalRefRate3_Leave);
            // 
            // spnExpoLimit4
            // 
            this.spnExpoLimit4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnExpoLimit4.DataType = DataTypes.Numeric;
            this.spnExpoLimit4.Increment = 1;
            this.spnExpoLimit4.Location = new System.Drawing.Point(9, 144);
            this.spnExpoLimit4.MaxValue = 99;
            this.spnExpoLimit4.MinValue = 0;
            this.spnExpoLimit4.Name = "spnExpoLimit4";
            this.spnExpoLimit4.Size = new System.Drawing.Size(45, 20);
            this.spnExpoLimit4.TabIndex = 119;
            this.spnExpoLimit4.Value = 0;
            this.spnExpoLimit4.Enter += new System.EventHandler(this.spnExpoLimit4_Enter);
            this.spnExpoLimit4.Leave += new System.EventHandler(this.spnExpoLimit4_Leave);
            // 
            // spnCalRefRate4
            // 
            this.spnCalRefRate4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnCalRefRate4.DataType = DataTypes.Numeric;
            this.spnCalRefRate4.Increment = 1;
            this.spnCalRefRate4.Location = new System.Drawing.Point(231, 144);
            this.spnCalRefRate4.MaxValue = 60;
            this.spnCalRefRate4.MinValue = 0;
            this.spnCalRefRate4.Name = "spnCalRefRate4";
            this.spnCalRefRate4.Size = new System.Drawing.Size(45, 20);
            this.spnCalRefRate4.TabIndex = 169;
            this.spnCalRefRate4.Value = 0;
            this.spnCalRefRate4.Enter += new System.EventHandler(this.spnCalRefRate4_Enter);
            this.spnCalRefRate4.Leave += new System.EventHandler(this.spnCalRefRate4_Leave);
            // 
            // spnExpLimitto4
            // 
            this.spnExpLimitto4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnExpLimitto4.DataType = DataTypes.Numeric;
            this.spnExpLimitto4.Increment = 1;
            this.spnExpLimitto4.Location = new System.Drawing.Point(58, 144);
            this.spnExpLimitto4.MaxValue = 100;
            this.spnExpLimitto4.MinValue = 0;
            this.spnExpLimitto4.Name = "spnExpLimitto4";
            this.spnExpLimitto4.Size = new System.Drawing.Size(45, 20);
            this.spnExpLimitto4.TabIndex = 122;
            this.spnExpLimitto4.Value = 0;
            this.spnExpLimitto4.Enter += new System.EventHandler(this.spnExpLimitto4_Enter);
            this.spnExpLimitto4.Leave += new System.EventHandler(this.spnExpLimitto4_Leave);
            // 
            // spnExpoLimit3
            // 
            this.spnExpoLimit3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnExpoLimit3.DataType = DataTypes.Numeric;
            this.spnExpoLimit3.Increment = 1;
            this.spnExpoLimit3.Location = new System.Drawing.Point(9, 119);
            this.spnExpoLimit3.MaxValue = 99;
            this.spnExpoLimit3.MinValue = 0;
            this.spnExpoLimit3.Name = "spnExpoLimit3";
            this.spnExpoLimit3.Size = new System.Drawing.Size(45, 20);
            this.spnExpoLimit3.TabIndex = 118;
            this.spnExpoLimit3.Value = 0;
            this.spnExpoLimit3.Enter += new System.EventHandler(this.spnExpoLimit3_Enter);
            this.spnExpoLimit3.Leave += new System.EventHandler(this.spnExpoLimit3_Leave);
            // 
            // spnExpLimitto1
            // 
            this.spnExpLimitto1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnExpLimitto1.DataType = DataTypes.Numeric;
            this.spnExpLimitto1.Increment = 1;
            this.spnExpLimitto1.Location = new System.Drawing.Point(58, 69);
            this.spnExpLimitto1.MaxValue = 100;
            this.spnExpLimitto1.MinValue = 1;
            this.spnExpLimitto1.Name = "spnExpLimitto1";
            this.spnExpLimitto1.Size = new System.Drawing.Size(45, 20);
            this.spnExpLimitto1.TabIndex = 123;
            this.spnExpLimitto1.Value = 100;
            this.spnExpLimitto1.Enter += new System.EventHandler(this.spnExpLimitto1_Enter);
            this.spnExpLimitto1.Leave += new System.EventHandler(this.spnExpLimitto1_Leave);
            // 
            // spnExpoLimit2
            // 
            this.spnExpoLimit2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnExpoLimit2.DataType = DataTypes.Numeric;
            this.spnExpoLimit2.Increment = 1;
            this.spnExpoLimit2.Location = new System.Drawing.Point(9, 94);
            this.spnExpoLimit2.MaxValue = 99;
            this.spnExpoLimit2.MinValue = 0;
            this.spnExpoLimit2.Name = "spnExpoLimit2";
            this.spnExpoLimit2.Size = new System.Drawing.Size(45, 20);
            this.spnExpoLimit2.TabIndex = 117;
            this.spnExpoLimit2.Value = 0;
            this.spnExpoLimit2.Enter += new System.EventHandler(this.spnExpoLimit2_Enter);
            this.spnExpoLimit2.Leave += new System.EventHandler(this.spnExpoLimit2_Leave);
            // 
            // spnExpLimitto3
            // 
            this.spnExpLimitto3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnExpLimitto3.DataType = DataTypes.Numeric;
            this.spnExpLimitto3.Increment = 1;
            this.spnExpLimitto3.Location = new System.Drawing.Point(58, 119);
            this.spnExpLimitto3.MaxValue = 100;
            this.spnExpLimitto3.MinValue = 0;
            this.spnExpLimitto3.Name = "spnExpLimitto3";
            this.spnExpLimitto3.Size = new System.Drawing.Size(45, 20);
            this.spnExpLimitto3.TabIndex = 121;
            this.spnExpLimitto3.Value = 0;
            this.spnExpLimitto3.Enter += new System.EventHandler(this.spnExpLimitto3_Enter);
            this.spnExpLimitto3.Leave += new System.EventHandler(this.spnExpLimitto3_Leave);
            // 
            // spnExpoLimit1
            // 
            this.spnExpoLimit1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnExpoLimit1.DataType = DataTypes.Numeric;
            this.spnExpoLimit1.Increment = 1;
            this.spnExpoLimit1.Location = new System.Drawing.Point(9, 69);
            this.spnExpoLimit1.MaxValue = 99;
            this.spnExpoLimit1.MinValue = 0;
            this.spnExpoLimit1.Name = "spnExpoLimit1";
            this.spnExpoLimit1.Size = new System.Drawing.Size(45, 20);
            this.spnExpoLimit1.TabIndex = 114;
            this.spnExpoLimit1.Value = 0;
            this.spnExpoLimit1.Enter += new System.EventHandler(this.spnExpoLimit1_Enter);
            this.spnExpoLimit1.Leave += new System.EventHandler(this.spnExpoLimit1_Leave);
            // 
            // ultraLabel7
            // 
            appearance5.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance5.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraLabel7.Appearance = appearance5;
            this.ultraLabel7.Location = new System.Drawing.Point(232, 43);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(43, 15);
            this.ultraLabel7.TabIndex = 164;
            this.ultraLabel7.Text = "Minutes";
            // 
            // spnExpLimitto2
            // 
            this.spnExpLimitto2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnExpLimitto2.DataType = DataTypes.Numeric;
            this.spnExpLimitto2.Increment = 1;
            this.spnExpLimitto2.Location = new System.Drawing.Point(58, 94);
            this.spnExpLimitto2.MaxValue = 100;
            this.spnExpLimitto2.MinValue = 0;
            this.spnExpLimitto2.Name = "spnExpLimitto2";
            this.spnExpLimitto2.Size = new System.Drawing.Size(45, 20);
            this.spnExpLimitto2.TabIndex = 120;
            this.spnExpLimitto2.Value = 0;
            this.spnExpLimitto2.Enter += new System.EventHandler(this.spnExpLimitto2_Enter);
            this.spnExpLimitto2.Leave += new System.EventHandler(this.spnExpLimitto2_Leave);
            // 
            // ultraLabel6
            // 
            appearance6.ForeColor = System.Drawing.Color.Red;
            appearance6.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance6.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraLabel6.Appearance = appearance6;
            this.ultraLabel6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel6.Location = new System.Drawing.Point(280, 8);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(10, 15);
            this.ultraLabel6.TabIndex = 163;
            this.ultraLabel6.Text = "*";
            // 
            // ultraLabel5
            // 
            appearance7.ForeColor = System.Drawing.Color.Red;
            appearance7.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance7.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraLabel5.Appearance = appearance7;
            this.ultraLabel5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel5.Location = new System.Drawing.Point(498, 7);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(10, 15);
            this.ultraLabel5.TabIndex = 162;
            this.ultraLabel5.Text = "*";
            // 
            // ultraLabel4
            // 
            appearance8.ForeColor = System.Drawing.Color.Red;
            appearance8.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance8.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraLabel4.Appearance = appearance8;
            this.ultraLabel4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel4.Location = new System.Drawing.Point(100, 8);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(10, 15);
            this.ultraLabel4.TabIndex = 161;
            this.ultraLabel4.Text = "*";
            // 
            // lblCalculationRefreshRate
            // 
            appearance9.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance9.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblCalculationRefreshRate.Appearance = appearance9;
            this.lblCalculationRefreshRate.Location = new System.Drawing.Point(196, 43);
            this.lblCalculationRefreshRate.Name = "lblCalculationRefreshRate";
            this.lblCalculationRefreshRate.Size = new System.Drawing.Size(32, 15);
            this.lblCalculationRefreshRate.TabIndex = 157;
            this.lblCalculationRefreshRate.Text = "Every ";
            // 
            // lblTo
            // 
            appearance10.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance10.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblTo.Appearance = appearance10;
            this.lblTo.Location = new System.Drawing.Point(73, 43);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(14, 15);
            this.lblTo.TabIndex = 156;
            this.lblTo.Text = "to";
            // 
            // lblFrom
            // 
            appearance11.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance11.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblFrom.Appearance = appearance11;
            this.lblFrom.Location = new System.Drawing.Point(13, 43);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(30, 15);
            this.lblFrom.TabIndex = 155;
            this.lblFrom.Text = "From";
            // 
            // lblBlkTrading
            // 
            appearance12.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance12.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblBlkTrading.Appearance = appearance12;
            this.lblBlkTrading.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblBlkTrading.Location = new System.Drawing.Point(620, 11);
            this.lblBlkTrading.Name = "lblBlkTrading";
            this.lblBlkTrading.Size = new System.Drawing.Size(53, 37);
            this.lblBlkTrading.TabIndex = 154;
            this.lblBlkTrading.Text = "Block Trading";
            // 
            // lblEmail
            // 
            appearance13.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance13.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblEmail.Appearance = appearance13;
            this.lblEmail.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblEmail.Location = new System.Drawing.Point(420, 12);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(77, 15);
            this.lblEmail.TabIndex = 152;
            this.lblEmail.Text = "E-mail Alerts";
            // 
            // lblAlertMessage
            // 
            this.lblAlertMessage.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAlertMessage.Location = new System.Drawing.Point(339, 43);
            this.lblAlertMessage.Name = "lblAlertMessage";
            this.lblAlertMessage.Size = new System.Drawing.Size(74, 15);
            this.lblAlertMessage.TabIndex = 151;
            this.lblAlertMessage.Text = "Alert Message";
            // 
            // lblNotify
            // 
            appearance14.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance14.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblNotify.Appearance = appearance14;
            this.lblNotify.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblNotify.Location = new System.Drawing.Point(138, 12);
            this.lblNotify.Name = "lblNotify";
            this.lblNotify.Size = new System.Drawing.Size(145, 15);
            this.lblNotify.TabIndex = 150;
            this.lblNotify.Text = "Calculation Refresh Rate";
            // 
            // lblExposureLimitsAlerts
            // 
            appearance15.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance15.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblExposureLimitsAlerts.Appearance = appearance15;
            this.lblExposureLimitsAlerts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblExposureLimitsAlerts.Location = new System.Drawing.Point(8, 12);
            this.lblExposureLimitsAlerts.Name = "lblExposureLimitsAlerts";
            this.lblExposureLimitsAlerts.Size = new System.Drawing.Size(103, 29);
            this.lblExposureLimitsAlerts.TabIndex = 149;
            this.lblExposureLimitsAlerts.Text = "Exposure Limit %";
            // 
            // grpBxDefault
            // 
            this.grpBxDefault.Controls.Add(this.ultraLabel10);
            this.grpBxDefault.Controls.Add(this.spnCalRefRateGrpbx);
            this.grpBxDefault.Controls.Add(this.lblAfterEvrySecGrp);
            this.grpBxDefault.Controls.Add(this.lblCalRefreshRateGrp);
            this.grpBxDefault.Controls.Add(this.lblBeforeEachTradeGrp);
            this.grpBxDefault.Controls.Add(this.ultraLabel2);
            this.grpBxDefault.Controls.Add(this.chkDefaultAlertType);
            this.grpBxDefault.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpBxDefault.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.TopInsideBorder;
            this.grpBxDefault.Location = new System.Drawing.Point(7, 169);
            this.grpBxDefault.Name = "grpBxDefault";
            this.grpBxDefault.Size = new System.Drawing.Size(666, 52);
            this.grpBxDefault.SupportThemes = false;
            this.grpBxDefault.TabIndex = 148;
            this.grpBxDefault.Text = "Default Refresh Rate";
            // 
            // ultraLabel10
            // 
            appearance16.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance16.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraLabel10.Appearance = appearance16;
            this.ultraLabel10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel10.Location = new System.Drawing.Point(300, 6);
            this.ultraLabel10.Name = "ultraLabel10";
            this.ultraLabel10.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel10.TabIndex = 171;
            this.ultraLabel10.Text = "/";
            // 
            // spnCalRefRateGrpbx
            // 
            this.spnCalRefRateGrpbx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.spnCalRefRateGrpbx.DataType = DataTypes.Numeric;
            this.spnCalRefRateGrpbx.Increment = 1;
            this.spnCalRefRateGrpbx.Location = new System.Drawing.Point(363, 25);
            this.spnCalRefRateGrpbx.MaxValue = 60;
            this.spnCalRefRateGrpbx.MinValue = 0;
            this.spnCalRefRateGrpbx.Name = "spnCalRefRateGrpbx";
            this.spnCalRefRateGrpbx.Size = new System.Drawing.Size(45, 20);
            this.spnCalRefRateGrpbx.TabIndex = 170;
            this.spnCalRefRateGrpbx.Value = 0;
            this.spnCalRefRateGrpbx.Enter += new System.EventHandler(this.spnCalRefRateGrpbx_Enter);
            this.spnCalRefRateGrpbx.Leave += new System.EventHandler(this.spnCalRefRateGrpbx_Leave);
            // 
            // lblAfterEvrySecGrp
            // 
            appearance17.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance17.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblAfterEvrySecGrp.Appearance = appearance17;
            this.lblAfterEvrySecGrp.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAfterEvrySecGrp.Location = new System.Drawing.Point(317, 6);
            this.lblAfterEvrySecGrp.Name = "lblAfterEvrySecGrp";
            this.lblAfterEvrySecGrp.Size = new System.Drawing.Size(32, 15);
            this.lblAfterEvrySecGrp.TabIndex = 113;
            this.lblAfterEvrySecGrp.Text = "Every ";
            // 
            // lblCalRefreshRateGrp
            // 
            appearance18.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance18.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblCalRefreshRateGrp.Appearance = appearance18;
            this.lblCalRefreshRateGrp.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCalRefreshRateGrp.Location = new System.Drawing.Point(363, 6);
            this.lblCalRefreshRateGrp.Name = "lblCalRefreshRateGrp";
            this.lblCalRefreshRateGrp.Size = new System.Drawing.Size(43, 15);
            this.lblCalRefreshRateGrp.TabIndex = 83;
            this.lblCalRefreshRateGrp.Text = "Minutes";
            // 
            // lblBeforeEachTradeGrp
            // 
            appearance19.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance19.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.lblBeforeEachTradeGrp.Appearance = appearance19;
            this.lblBeforeEachTradeGrp.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblBeforeEachTradeGrp.Location = new System.Drawing.Point(207, 6);
            this.lblBeforeEachTradeGrp.Name = "lblBeforeEachTradeGrp";
            this.lblBeforeEachTradeGrp.Size = new System.Drawing.Size(93, 15);
            this.lblBeforeEachTradeGrp.TabIndex = 82;
            this.lblBeforeEachTradeGrp.Text = "Before EachTrade";
            // 
            // ultraLabel2
            // 
            appearance20.ForeColor = System.Drawing.Color.Red;
            appearance20.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance20.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraLabel2.Appearance = appearance20;
            this.ultraLabel2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel2.Location = new System.Drawing.Point(128, 2);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(10, 15);
            this.ultraLabel2.TabIndex = 112;
            this.ultraLabel2.Text = "*";
            // 
            // chkDefaultAlertType
            // 
            this.chkDefaultAlertType.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.chkDefaultAlertType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkDefaultAlertType.ItemAppearance = appearance21;
            valueListItem3.DataValue = "";
            valueListItem3.DisplayText = "";
            valueListItem4.DataValue = "";
            this.chkDefaultAlertType.Items.Add(valueListItem3);
            this.chkDefaultAlertType.Items.Add(valueListItem4);
            this.chkDefaultAlertType.ItemSpacingHorizontal = 40;
            this.chkDefaultAlertType.Location = new System.Drawing.Point(263, 25);
            this.chkDefaultAlertType.Name = "chkDefaultAlertType";
            this.chkDefaultAlertType.Size = new System.Drawing.Size(76, 20);
            this.chkDefaultAlertType.TabIndex = 80;
            this.chkDefaultAlertType.Click += new System.EventHandler(this.chkDefaultAlertType_Click);
            this.chkDefaultAlertType.ValueChanged += new System.EventHandler(this.chkDefaultAlertType_ValueChanged);
            this.chkDefaultAlertType.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkDefaultAlertType_MouseUp);
            // 
            // chkBlockTrade4
            // 
            this.chkBlockTrade4.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkBlockTrade4.Location = new System.Drawing.Point(640, 144);
            this.chkBlockTrade4.Name = "chkBlockTrade4";
            this.chkBlockTrade4.Size = new System.Drawing.Size(20, 20);
            this.chkBlockTrade4.TabIndex = 147;
            this.chkBlockTrade4.CheckStateChanged += new System.EventHandler(this.chkBlockTrade4_CheckStateChanged);
            // 
            // chkBlockTrade3
            // 
            this.chkBlockTrade3.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkBlockTrade3.Location = new System.Drawing.Point(640, 119);
            this.chkBlockTrade3.Name = "chkBlockTrade3";
            this.chkBlockTrade3.Size = new System.Drawing.Size(20, 20);
            this.chkBlockTrade3.TabIndex = 146;
            this.chkBlockTrade3.CheckStateChanged += new System.EventHandler(this.chkBlockTrade3_CheckStateChanged);
            // 
            // chkBlockTrade2
            // 
            this.chkBlockTrade2.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkBlockTrade2.Location = new System.Drawing.Point(640, 94);
            this.chkBlockTrade2.Name = "chkBlockTrade2";
            this.chkBlockTrade2.Size = new System.Drawing.Size(20, 20);
            this.chkBlockTrade2.TabIndex = 145;
            this.chkBlockTrade2.CheckStateChanged += new System.EventHandler(this.chkBlockTrade2_CheckStateChanged);
            // 
            // chkBlockTrade1
            // 
            this.chkBlockTrade1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkBlockTrade1.Location = new System.Drawing.Point(640, 69);
            this.chkBlockTrade1.Name = "chkBlockTrade1";
            this.chkBlockTrade1.Size = new System.Drawing.Size(20, 20);
            this.chkBlockTrade1.TabIndex = 144;
            this.chkBlockTrade1.CheckStateChanged += new System.EventHandler(this.chkBlockTrade1_CheckStateChanged);
            // 
            // txtEmailAlert4
            // 
            this.txtEmailAlert4.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtEmailAlert4.Location = new System.Drawing.Point(464, 145);
            this.txtEmailAlert4.Name = "txtEmailAlert4";
            this.txtEmailAlert4.Size = new System.Drawing.Size(153, 20);
            this.txtEmailAlert4.TabIndex = 143;
            this.txtEmailAlert4.Enter += new System.EventHandler(this.txtEmailAlert4_Enter);
            this.txtEmailAlert4.Leave += new System.EventHandler(this.txtEmailAlert4_Leave);
            // 
            // txtEmailAlert3
            // 
            this.txtEmailAlert3.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtEmailAlert3.Location = new System.Drawing.Point(464, 119);
            this.txtEmailAlert3.Name = "txtEmailAlert3";
            this.txtEmailAlert3.Size = new System.Drawing.Size(153, 20);
            this.txtEmailAlert3.TabIndex = 142;
            this.txtEmailAlert3.Enter += new System.EventHandler(this.txtEmailAlert3_Enter);
            this.txtEmailAlert3.Leave += new System.EventHandler(this.txtEmailAlert3_Leave);
            // 
            // txtEmailAlert2
            // 
            this.txtEmailAlert2.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtEmailAlert2.Location = new System.Drawing.Point(464, 95);
            this.txtEmailAlert2.Name = "txtEmailAlert2";
            this.txtEmailAlert2.Size = new System.Drawing.Size(153, 20);
            this.txtEmailAlert2.TabIndex = 141;
            this.txtEmailAlert2.Enter += new System.EventHandler(this.txtEmailAlert2_Enter);
            this.txtEmailAlert2.Leave += new System.EventHandler(this.txtEmailAlert2_Leave);
            // 
            // txtEmailAlert1
            // 
            this.txtEmailAlert1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtEmailAlert1.Location = new System.Drawing.Point(464, 69);
            this.txtEmailAlert1.Name = "txtEmailAlert1";
            this.txtEmailAlert1.Size = new System.Drawing.Size(153, 20);
            this.txtEmailAlert1.TabIndex = 140;
            this.txtEmailAlert1.Enter += new System.EventHandler(this.txtEmailAlert1_Enter);
            this.txtEmailAlert1.Leave += new System.EventHandler(this.txtEmailAlert1_Leave);
            // 
            // txtAlertMsg4
            // 
            this.txtAlertMsg4.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtAlertMsg4.Location = new System.Drawing.Point(300, 144);
            this.txtAlertMsg4.Name = "txtAlertMsg4";
            this.txtAlertMsg4.Size = new System.Drawing.Size(158, 20);
            this.txtAlertMsg4.TabIndex = 135;
            this.txtAlertMsg4.Enter += new System.EventHandler(this.txtAlertMsg4_Enter);
            this.txtAlertMsg4.Leave += new System.EventHandler(this.txtAlertMsg4_Leave);
            // 
            // txtAlertMsg3
            // 
            this.txtAlertMsg3.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtAlertMsg3.Location = new System.Drawing.Point(300, 119);
            this.txtAlertMsg3.Name = "txtAlertMsg3";
            this.txtAlertMsg3.Size = new System.Drawing.Size(158, 20);
            this.txtAlertMsg3.TabIndex = 134;
            this.txtAlertMsg3.Enter += new System.EventHandler(this.txtAlertMsg3_Enter);
            this.txtAlertMsg3.Leave += new System.EventHandler(this.txtAlertMsg3_Leave);
            // 
            // txtAlertMsg2
            // 
            this.txtAlertMsg2.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtAlertMsg2.Location = new System.Drawing.Point(300, 94);
            this.txtAlertMsg2.Name = "txtAlertMsg2";
            this.txtAlertMsg2.Size = new System.Drawing.Size(158, 20);
            this.txtAlertMsg2.TabIndex = 133;
            this.txtAlertMsg2.Enter += new System.EventHandler(this.txtAlertMsg2_Enter);
            this.txtAlertMsg2.Leave += new System.EventHandler(this.txtAlertMsg2_Leave);
            // 
            // txtAlertMsg1
            // 
            this.txtAlertMsg1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtAlertMsg1.Location = new System.Drawing.Point(300, 69);
            this.txtAlertMsg1.Name = "txtAlertMsg1";
            this.txtAlertMsg1.Size = new System.Drawing.Size(158, 20);
            this.txtAlertMsg1.TabIndex = 132;
            this.txtAlertMsg1.Enter += new System.EventHandler(this.txtAlertMsg1_Enter);
            this.txtAlertMsg1.Leave += new System.EventHandler(this.txtAlertMsg1_Leave);
            // 
            // chkAlertType3
            // 
            this.chkAlertType3.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.chkAlertType3.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkAlertType3.ItemAppearance = appearance22;
            valueListItem5.DataValue = "";
            valueListItem5.DisplayText = "";
            valueListItem6.DataValue = "";
            this.chkAlertType3.Items.Add(valueListItem5);
            this.chkAlertType3.Items.Add(valueListItem6);
            this.chkAlertType3.ItemSpacingHorizontal = 40;
            this.chkAlertType3.Location = new System.Drawing.Point(138, 119);
            this.chkAlertType3.Name = "chkAlertType3";
            this.chkAlertType3.Size = new System.Drawing.Size(76, 20);
            this.chkAlertType3.TabIndex = 127;
            this.chkAlertType3.Click += new System.EventHandler(this.chkAlertType3_Click);
            this.chkAlertType3.ValueChanged += new System.EventHandler(this.chkAlertType3_ValueChanged);
            this.chkAlertType3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkAlertType3_MouseUp);
            // 
            // chkAlertType4
            // 
            this.chkAlertType4.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.chkAlertType4.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkAlertType4.ItemAppearance = appearance23;
            valueListItem7.DataValue = "";
            valueListItem7.DisplayText = "";
            valueListItem8.DataValue = "";
            this.chkAlertType4.Items.Add(valueListItem7);
            this.chkAlertType4.Items.Add(valueListItem8);
            this.chkAlertType4.ItemSpacingHorizontal = 40;
            this.chkAlertType4.Location = new System.Drawing.Point(138, 144);
            this.chkAlertType4.Name = "chkAlertType4";
            this.chkAlertType4.Size = new System.Drawing.Size(76, 20);
            this.chkAlertType4.TabIndex = 126;
            this.chkAlertType4.Click += new System.EventHandler(this.chkAlertType4_Click);
            this.chkAlertType4.ValueChanged += new System.EventHandler(this.chkAlertType4_ValueChanged);
            this.chkAlertType4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkAlertType4_MouseUp);
            // 
            // chkAlertType2
            // 
            this.chkAlertType2.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.chkAlertType2.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkAlertType2.ItemAppearance = appearance24;
            valueListItem9.DataValue = "";
            valueListItem9.DisplayText = "";
            valueListItem10.DataValue = "";
            this.chkAlertType2.Items.Add(valueListItem9);
            this.chkAlertType2.Items.Add(valueListItem10);
            this.chkAlertType2.ItemSpacingHorizontal = 40;
            this.chkAlertType2.Location = new System.Drawing.Point(138, 93);
            this.chkAlertType2.Name = "chkAlertType2";
            this.chkAlertType2.Size = new System.Drawing.Size(76, 20);
            this.chkAlertType2.TabIndex = 125;
            this.chkAlertType2.Click += new System.EventHandler(this.chkAlertType2_Click);
            this.chkAlertType2.ValueChanged += new System.EventHandler(this.chkAlertType2_ValueChanged);
            this.chkAlertType2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkAlertType2_MouseUp);
            // 
            // Company_Alerts
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpCompanyAlerts);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "Company_Alerts";
            this.Size = new System.Drawing.Size(686, 234);
            this.Load += new System.EventHandler(this.Company_Alerts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCompanyAlerts)).EndInit();
            this.grpCompanyAlerts.ResumeLayout(false);
            this.grpCompanyAlerts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkAlertType1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxDefault)).EndInit();
            this.grpBxDefault.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkDefaultAlertType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailAlert4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailAlert3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailAlert2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailAlert1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlertMsg4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlertMsg3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlertMsg2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlertMsg1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAlertType3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAlertType4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAlertType2)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Set Method

        /// <summary>
        /// The set property is to set the data fetched from database into the display controls.
        /// </summary>
        public CompanyAlerts SetCompanyAlerts
        {
            set { SettingCompanyAlerts(value); }
        }

        /// <summary>
        /// The method is used to set each data item in its specific control.
        /// </summary>
        /// <param name="companyAlerts"></param>
        private void SettingCompanyAlerts(CompanyAlerts companyAlerts)
        {
            try
            {
                chkBlockTrade1.CheckStateChanged -= new EventHandler(chkBlockTrade1_CheckStateChanged);
                chkBlockTrade2.CheckStateChanged -= new EventHandler(chkBlockTrade2_CheckStateChanged);
                chkBlockTrade3.CheckStateChanged -= new EventHandler(chkBlockTrade3_CheckStateChanged);
                chkBlockTrade4.CheckStateChanged -= new EventHandler(chkBlockTrade4_CheckStateChanged);
                //To check that the valid companyID is selected.
                if (_companyID != int.MinValue)
                {
                    //The companyAlerts is a collection instance..so we check the count.
                    if (companyAlerts.Count > 0)
                    {
                        //While traversing through each object in the collection, we set them according to the data and there respective controls.
                        foreach (CompanyAlert companyAlert in companyAlerts)
                        {
                            // to set the data for the 1st row.
                            if (companyAlert.Rank == 1 && companyAlert.AlertMessage != "")
                            {
                                spnExpoLimit1.Value = int.Parse(companyAlert.CompanyExposureLower.ToString());
                                spnExpLimitto1.Value = int.Parse(companyAlert.CompanyExposureUpper.ToString());

                                //if (companyAlert.RefreshRateCalculation > 0)
                                //{
                                //    chkAlertType1.indeCheckedIndex = 1;
                                //}
                                //else
                                //{
                                //    chkAlertType1.CheckedIndex = 0;
                                //}
                                if (companyAlert.AlertTypeID == 0)
                                {
                                    chkAlertType1.CheckedIndex = 0;
                                }
                                else if (companyAlert.AlertTypeID == 1)
                                {
                                    chkAlertType1.CheckedIndex = 1;
                                    //chkAlertType1. CheckedIndex = ;
                                }
                                //chkAlertType1.CheckedIndex = companyAlert.AlertTypeID;
                                //if the notification for refreshing calculation is "Every min " type, then 
                                if (companyAlert.AlertTypeID != 0)
                                {
                                    //the refresh rate spinner is enabled as well as set with the value.
                                    spnCalRefRate1.Enabled = true;
                                    spnCalRefRate1.Value = companyAlert.RefreshRateCalculation;
                                }
                                else
                                {
                                    //else, it is disabled.
                                    spnCalRefRate1.Value = 0;
                                    spnCalRefRate1.Enabled = false;
                                }
                                txtAlertMsg1.Text = companyAlert.AlertMessage.ToString();
                                txtEmailAlert1.Text = companyAlert.EmailAddress.ToString();
                                chkBlockTrade1.Checked = (int.Parse(companyAlert.BlockTrading.ToString()) == 1 ? true : false);
                            }
                            // to set the data for 2nd row.
                            else if (companyAlert.Rank == 2 && companyAlert.AlertMessage != "")
                            {
                                spnExpoLimit2.Value = int.Parse(companyAlert.CompanyExposureLower.ToString());
                                spnExpLimitto2.Value = int.Parse(companyAlert.CompanyExposureUpper.ToString());
                                chkAlertType2.CheckedIndex = companyAlert.AlertTypeID;
                                if (companyAlert.AlertTypeID != 0)
                                {
                                    spnCalRefRate2.Enabled = true;
                                    spnCalRefRate2.Value = companyAlert.RefreshRateCalculation;
                                }
                                else
                                {
                                    spnCalRefRate2.Value = 0;
                                    spnCalRefRate2.Enabled = false;
                                }
                                txtAlertMsg2.Text = companyAlert.AlertMessage.ToString();
                                txtEmailAlert2.Text = companyAlert.EmailAddress.ToString();
                                chkBlockTrade2.Checked = (int.Parse(companyAlert.BlockTrading.ToString()) == 1 ? true : false);
                            }
                            // to set the data for 3rd row.
                            else if (companyAlert.Rank == 3 && companyAlert.AlertMessage != "")
                            {
                                spnExpoLimit3.Value = int.Parse(companyAlert.CompanyExposureLower.ToString());
                                spnExpLimitto3.Value = int.Parse(companyAlert.CompanyExposureUpper.ToString());
                                chkAlertType3.CheckedIndex = companyAlert.AlertTypeID;
                                if (companyAlert.AlertTypeID != 0)
                                {
                                    spnCalRefRate3.Enabled = true;
                                    spnCalRefRate3.Value = companyAlert.RefreshRateCalculation;
                                }
                                else
                                {
                                    spnCalRefRate3.Value = 0;
                                    spnCalRefRate3.Enabled = false;
                                }
                                txtAlertMsg3.Text = companyAlert.AlertMessage.ToString();
                                txtEmailAlert3.Text = companyAlert.EmailAddress.ToString();
                                chkBlockTrade3.Checked = (int.Parse(companyAlert.BlockTrading.ToString()) == 1 ? true : false);
                            }
                            // to set the data for 4rth row.
                            else if (companyAlert.Rank == 4 && companyAlert.AlertMessage != "")
                            {
                                spnExpoLimit4.Value = int.Parse(companyAlert.CompanyExposureLower.ToString());
                                spnExpLimitto4.Value = int.Parse(companyAlert.CompanyExposureUpper.ToString());
                                chkAlertType4.CheckedIndex = companyAlert.AlertTypeID;
                                if (companyAlert.AlertTypeID != 0)
                                {
                                    spnCalRefRate4.Enabled = true;
                                    spnCalRefRate4.Value = companyAlert.RefreshRateCalculation;
                                }
                                else
                                {
                                    spnCalRefRate4.Value = 0;
                                    spnCalRefRate4.Enabled = false;
                                }
                                txtAlertMsg4.Text = companyAlert.AlertMessage.ToString();
                                txtEmailAlert4.Text = companyAlert.EmailAddress.ToString();
                                chkBlockTrade4.Checked = (int.Parse(companyAlert.BlockTrading.ToString()) == 1 ? true : false);
                            }
                        }

                    }
                    //If the count of collection is zero, than the companyalerts form is set to refresh.
                    else
                    {
                        RefreshFirstRow();
                        if (spnExpLimitto1.Value == 100)
                        {
                            RefreshSecondRow();
                            DisableSecondRow();
                            RefreshThirdRow();
                            DisableThirdRow();
                            RefreshFourthRow();
                            DisableFourthRow();
                        }

                    }

                }
                chkBlockTrade1.CheckStateChanged += new EventHandler(chkBlockTrade1_CheckStateChanged);
                chkBlockTrade2.CheckStateChanged += new EventHandler(chkBlockTrade2_CheckStateChanged);
                chkBlockTrade3.CheckStateChanged += new EventHandler(chkBlockTrade3_CheckStateChanged);
                chkBlockTrade4.CheckStateChanged += new EventHandler(chkBlockTrade4_CheckStateChanged);
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

        public DefaultAlert SetDefaultAlert
        {
            set { SettingDefaultAlert(value); }
        }

        private void SettingDefaultAlert(DefaultAlert defaultAlert)
        {
            try
            {
                if (_companyID != int.MinValue && defaultAlert.RMDefaultID > 0)
                {
                    chkDefaultAlertType.CheckedIndex = defaultAlert.AlertTypeID;
                    if (chkDefaultAlertType.CheckedIndex != 0)
                    {
                        spnCalRefRateGrpbx.Value = defaultAlert.RefreshRateCalculation;
                    }
                }
                else
                {
                    RefreshDefaultAlert();
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

        #endregion Set Method

        #region Row Enable/Disable/Refresh methods

        private void RefreshAllRows()
        {
            RefreshFirstRow();
            RefreshSecondRow();
            RefreshThirdRow();
            RefreshFourthRow();
        }

        /// <summary>
        /// This refreshes the controls in 1st row.
        /// </summary>
        private void RefreshFirstRow()
        {

            spnExpoLimit1.Value = 0;
            spnExpLimitto1.Value = 100;
            chkAlertType1.CheckedIndex = -1;
            spnCalRefRate1.Enabled = true;
            spnCalRefRate1.Value = 0;
            txtAlertMsg1.Text = "";
            txtEmailAlert1.Text = "";
            chkBlockTrade1.CheckState = CheckState.Unchecked;

        }

        /// <summary>
        /// This refreshes the controls in 2nd row.
        /// </summary>
        private void RefreshSecondRow()
        {

            if (spnExpLimitto1.Value != 100)
            {
                int b = Convert.ToInt32(spnExpLimitto1.Value);
                b = b++;
                spnExpoLimit2.Value = b;
                spnExpLimitto2.Value = 100;

            }
            else
            {
                spnExpoLimit2.Value = 0;
                spnExpLimitto2.Value = 0;
            }
            chkAlertType2.CheckedIndex = -1;
            spnCalRefRate2.Enabled = true;
            spnCalRefRate2.Value = 0;
            txtAlertMsg2.Text = "";
            txtEmailAlert2.Text = "";
            chkBlockTrade2.CheckState = CheckState.Unchecked;
        }

        /// <summary>
        /// This refreshes the controls in 3rd row.
        /// </summary>
        private void RefreshThirdRow()
        {

            if (spnExpLimitto2.Value != 100 && spnExpLimitto2.Value != 0)
            {
                int b = Convert.ToInt32(spnExpLimitto2.Value);
                b++;
                spnExpoLimit3.Value = b;
                spnExpLimitto3.Value = 100;
            }
            else
            {
                spnExpoLimit3.Value = 0;
                spnExpLimitto3.Value = 0;
            }
            chkAlertType3.CheckedIndex = -1;
            spnCalRefRate3.Enabled = true;
            spnCalRefRate3.Value = 0;
            txtAlertMsg3.Text = "";
            txtEmailAlert3.Text = "";
            chkBlockTrade3.CheckState = CheckState.Unchecked;
        }

        /// <summary>
        /// this refreshes controls in 4rth row.
        /// </summary>
        private void RefreshFourthRow()
        {
            if (spnExpLimitto3.Value != 100 && spnExpLimitto3.Value != 0)
            {
                int b = Convert.ToInt32(spnExpLimitto3.Value);
                b++;
                spnExpoLimit4.Value = b;
                spnExpLimitto4.Value = 100;
            }
            else
            {
                spnExpoLimit4.Value = 0;
                spnExpLimitto4.Value = 0;
            }
            chkAlertType4.CheckedIndex = -1;
            spnCalRefRate4.Enabled = true;
            spnCalRefRate4.Value = 0;
            txtAlertMsg4.Text = "";
            txtEmailAlert4.Text = "";
            chkBlockTrade4.CheckState = CheckState.Unchecked;
        }

        /// <summary>
        /// This method is ued to refresh the Controls used for Default Alerts settings
        /// </summary>
        private void RefreshDefaultAlert()
        {
            try
            {
                chkDefaultAlertType.CheckedIndex = -1;
                spnCalRefRateGrpbx.Enabled = true;
                spnCalRefRateGrpbx.Value = 0;
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
        /// for enabling or disabling second row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableSecondRow()
        {
            try
            {
                if (spnExpLimitto1.Value != 100)
                {
                    int a = Convert.ToInt32(spnExpLimitto1.Value);
                    a++;
                    spnExpoLimit2.Value = a;
                    spnExpoLimit2.Enabled = false;
                    //spnExpoLimit2.BackColor = Color.White;
                    spnExpLimitto2.Enabled = true;
                    chkAlertType2.Enabled = true;
                    spnCalRefRate2.Enabled = true;
                    txtAlertMsg2.Enabled = true;
                    txtEmailAlert2.Enabled = true;
                    chkBlockTrade2.Enabled = true;
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

        private void DisableSecondRow()
        {
            try
            {
                spnExpoLimit2.Enabled = false;
                spnExpLimitto2.Enabled = false;
                chkAlertType2.Enabled = false;
                spnCalRefRate2.Enabled = false;
                txtAlertMsg2.Enabled = false;
                txtEmailAlert2.Enabled = false;
                chkBlockTrade2.Enabled = false;
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
        /// For enabling and disabling third row
        /// </summary>
        private void EnableThirdRow()
        {
            try
            {
                if (spnExpLimitto2.Value != 100)
                {
                    int a = Convert.ToInt32(spnExpLimitto2.Value);
                    a++;
                    spnExpoLimit3.Value = a;
                    spnExpoLimit3.Enabled = false;

                    spnExpLimitto3.Enabled = true;
                    chkAlertType3.Enabled = true;
                    spnCalRefRate3.Enabled = true;
                    txtAlertMsg3.Enabled = true;
                    txtEmailAlert3.Enabled = true;
                    chkBlockTrade3.Enabled = true;
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

        private void DisableThirdRow()
        {
            try
            {
                spnExpoLimit3.Enabled = false;
                spnExpLimitto3.Enabled = false;
                chkAlertType3.Enabled = false;
                spnCalRefRate3.Enabled = false;
                txtAlertMsg3.Enabled = false;
                txtEmailAlert3.Enabled = false;
                chkBlockTrade3.Enabled = false;
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
        /// For enabling and disabling fourth row.
        /// </summary>
        private void EnableFourthRow()
        {
            try
            {
                if (spnExpLimitto3.Value != 100)
                {
                    int a = Convert.ToInt32(spnExpLimitto3.Value);
                    a++;
                    spnExpoLimit4.Value = a;
                    spnExpoLimit4.Enabled = false;
                    spnExpLimitto4.Enabled = true;
                    chkAlertType4.Enabled = true;
                    spnCalRefRate4.Enabled = true;
                    txtAlertMsg4.Enabled = true;
                    txtEmailAlert4.Enabled = true;
                    chkBlockTrade4.Enabled = true;
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

        private void DisableFourthRow()
        {
            try
            {
                spnExpoLimit4.Enabled = false;
                spnExpLimitto4.Enabled = false;
                chkAlertType4.Enabled = false;
                spnCalRefRate4.Enabled = false;
                txtAlertMsg4.Enabled = false;
                txtEmailAlert4.Enabled = false;
                chkBlockTrade4.Enabled = false;
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

        #endregion Row Enable/Disable/Refresh methods

        #region Selected Notification type

        private void chkAlertType1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkAlertType1.CheckedIndex == 0)
                {
                    spnCalRefRate1.Value = 0;
                    spnCalRefRate1.Enabled = false;
                }
                else if (chkAlertType1.CheckedIndex == 1)
                {
                    spnCalRefRate1.Enabled = true;
                    spnCalRefRate1.Value = 0;
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

        private void chkAlertType2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkAlertType2.CheckedIndex == 0)
                {
                    spnCalRefRate2.Value = 0;
                    spnCalRefRate2.Enabled = false;
                }
                else if (chkAlertType2.CheckedIndex == 1)
                {
                    spnCalRefRate2.Enabled = true;
                    spnCalRefRate2.Value = 0;
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

        private void chkAlertType3_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkAlertType3.CheckedIndex == 0)
                {
                    spnCalRefRate3.Value = 0;
                    spnCalRefRate3.Enabled = false;
                }
                else if (chkAlertType3.CheckedIndex == 1)
                {
                    spnCalRefRate3.Enabled = true;
                    spnCalRefRate3.Value = 0;
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

        private void chkAlertType4_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkAlertType4.CheckedIndex == 0)
                {
                    spnCalRefRate4.Value = 0;
                    spnCalRefRate4.Enabled = false;
                }
                else if (chkAlertType4.CheckedIndex == 1)
                {
                    spnCalRefRate4.Enabled = true;
                    spnCalRefRate4.Value = 0;
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

        private void chkAlertType1_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkAlertType1.CheckedIndex == 0)
                {
                    spnCalRefRate1.Value = 0;
                    spnCalRefRate1.Enabled = false;
                }
                else if (chkAlertType1.CheckedIndex == 1)
                {
                    spnCalRefRate1.Enabled = true;
                    spnCalRefRate1.Value = 0;
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

        private void chkAlertType2_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkAlertType2.CheckedIndex == 0)
                {
                    spnCalRefRate2.Value = 0;
                    spnCalRefRate2.Enabled = false;
                }
                else if (chkAlertType2.CheckedIndex == 1)
                {
                    spnCalRefRate2.Enabled = true;
                    spnCalRefRate2.Value = 0;
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

        private void chkAlertType3_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkAlertType3.CheckedIndex == 0)
                {
                    spnCalRefRate3.Value = 0;
                    spnCalRefRate3.Enabled = false;
                }
                else if (chkAlertType3.CheckedIndex == 1)
                {
                    spnCalRefRate3.Enabled = true;
                    spnCalRefRate3.Value = 0;
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

        private void chkAlertType4_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkAlertType4.CheckedIndex == 0)
                {
                    spnCalRefRate4.Value = 0;
                    spnCalRefRate4.Enabled = false;
                }
                else if (chkAlertType4.CheckedIndex == 1)
                {
                    spnCalRefRate4.Enabled = true;
                    spnCalRefRate4.Value = 0;
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

        private void chkDefaultAlertType_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkDefaultAlertType.CheckedIndex == 0)
                {
                    spnCalRefRateGrpbx.Value = 0;
                    spnCalRefRateGrpbx.Enabled = false;
                }
                else if (chkDefaultAlertType.CheckedIndex == 1)
                {
                    spnCalRefRateGrpbx.Enabled = true;
                    spnCalRefRateGrpbx.Value = 0;
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

        private void chkAlertType1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (chkAlertType1.CheckedIndex == 0)
                {
                    spnCalRefRate1.Value = 0;
                    spnCalRefRate1.Enabled = false;
                }
                else if (chkAlertType1.CheckedIndex == 1)
                {
                    spnCalRefRate1.Enabled = true;
                    spnCalRefRate1.Value = 0;
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

        private void chkAlertType2_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (chkAlertType2.CheckedIndex == 0)
                {
                    spnCalRefRate2.Value = 0;
                    spnCalRefRate2.Enabled = false;
                }
                else if (chkAlertType2.CheckedIndex == 1)
                {
                    spnCalRefRate2.Enabled = true;
                    spnCalRefRate2.Value = 0;
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

        private void chkAlertType3_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (chkAlertType3.CheckedIndex == 0)
                {
                    spnCalRefRate3.Value = 0;
                    spnCalRefRate3.Enabled = false;
                }
                else if (chkAlertType3.CheckedIndex == 1)
                {
                    spnCalRefRate3.Enabled = true;
                    spnCalRefRate3.Value = 0;
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

        private void chkAlertType4_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (chkAlertType4.CheckedIndex == 0)
                {
                    spnCalRefRate4.Value = 0;
                    spnCalRefRate4.Enabled = false;
                }
                else if (chkAlertType4.CheckedIndex == 1)
                {
                    spnCalRefRate4.Enabled = true;
                    spnCalRefRate4.Value = 0;
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

        private void chkDefaultAlertType_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (chkDefaultAlertType.CheckedIndex == 0)
                {
                    chkDefaultAlertType.CheckedIndex = 0;
                    spnCalRefRateGrpbx.Enabled = false;
                }
                else if (chkDefaultAlertType.CheckedIndex == 1)
                {
                    spnCalRefRateGrpbx.Enabled = true;
                    spnCalRefRateGrpbx.Value = 0;
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

        private void chkDefaultAlertType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkDefaultAlertType.CheckedIndex == 0)
                {
                    chkDefaultAlertType.CheckedIndex = 0;
                    spnCalRefRateGrpbx.Enabled = false;
                }
                else if (chkDefaultAlertType.CheckedIndex == 1)
                {
                    spnCalRefRateGrpbx.Enabled = true;
                    spnCalRefRateGrpbx.Value = 0;
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
        #endregion Selected Notification type

        #region Validation Check

        /// <summary>
        /// This is to check the validation of the controls. 
        /// </summary>
        public bool ValidateControl()
        {
            bool validationSuccess = true;
            // The regular expressions are used to Check that the entered email Id is valid.
            Regex emailRegex = new Regex("(?<user>[^@]+)@(?<host>.+),");

            // Here the email address entered in txtEmailAlert1 is checked for validity and correctness.
            Match emailMatch = emailRegex.Match(txtEmailAlert1.Text.ToString());

            errorProvider1.SetError(spnExpoLimit1, "");
            errorProvider1.SetError(spnExpLimitto1, "");
            errorProvider1.SetError(spnCalRefRate1, "");
            errorProvider1.SetError(chkAlertType1, "");
            errorProvider1.SetError(txtAlertMsg1, "");
            errorProvider1.SetError(txtEmailAlert1, "");

            // It checks that the Upper Exposure Limit is greater than the lower exposure limit.
            if (int.Parse(spnExpLimitto1.Value.ToString()) < int.Parse(spnExpoLimit1.Value.ToString()))
            {
                // an error message to be displayed to the user is set here.
                errorProvider1.SetError(this.spnExpLimitto1, "Please set a correct Upper Exposure Limit !");
                // since the validation failed , so the bool is set to false.
                validationSuccess = false;
                // the control for which the validation fails is set to focus.
                spnExpLimitto1.Focus();
                // the bool value is returned.
                return validationSuccess;
            }
            else if (chkAlertType1.CheckedIndex == -1)
            {
                errorProvider1.SetError(chkAlertType1, "Please select the alert type for calculation refresh rate!");
                validationSuccess = false;
                chkAlertType1.Focus();
                return validationSuccess;
            }
            else if (chkAlertType1.CheckedIndex == 1 && spnCalRefRate1.Value == 0)
            {
                errorProvider1.SetError(this.spnCalRefRate1, "Please set the Refresh Rate !");
                validationSuccess = false;
                spnCalRefRate1.Focus();
                return validationSuccess;
            }
            else if (txtAlertMsg1.Text.Equals(""))
            {
                errorProvider1.SetError(this.txtAlertMsg1, "Please enter Alert Message!");
                txtAlertMsg1.Focus();
                validationSuccess = false;
                return validationSuccess;
            }
            else if (txtEmailAlert1.Text.Equals(""))
            {
                errorProvider1.SetError(this.txtEmailAlert1, "Please enter Email address(es)!");
                txtEmailAlert1.Focus();
                validationSuccess = false;
                return validationSuccess;
            }
            else if (!emailMatch.Success)
            {
                errorProvider1.SetError(txtEmailAlert1, "Please enter valid Email address!");
                txtEmailAlert1.Focus();
                validationSuccess = false;
                return validationSuccess;
            }

            //It checks the validation for 2nd row of controls, if it is in enabled state.
            else if (spnExpLimitto1.Value != 100)
            {
                //// If the Row is enabled, then, the particular control on it is also enabled. 
                //if (txtAlertMsg2.Enabled == true)
                //{
                //The Textbox for email in 2nd row is checked for validation. 
                Match emailMatch2 = emailRegex.Match(txtEmailAlert2.Text.ToString());

                errorProvider1.SetError(spnExpoLimit2, "");
                errorProvider1.SetError(spnExpLimitto2, "");
                errorProvider1.SetError(chkAlertType2, "");
                errorProvider1.SetError(spnCalRefRate2, "");
                errorProvider1.SetError(txtAlertMsg2, "");
                errorProvider1.SetError(txtEmailAlert2, "");


                if (int.Parse(spnExpLimitto2.Value.ToString()) < int.Parse(spnExpoLimit2.Value.ToString()))
                {
                    errorProvider1.SetError(this.spnExpLimitto2, "Please set a correct Upper Exposure Limit!");
                    validationSuccess = false;
                    spnExpLimitto2.Focus();
                    return validationSuccess;
                }
                else if (chkAlertType2.CheckedIndex == -1)
                {
                    errorProvider1.SetError(chkAlertType2, "Please select the alert type!");
                    validationSuccess = false;
                    chkAlertType2.Focus();
                    return validationSuccess;
                }
                else if ((chkAlertType2.CheckedIndex == 1) && (spnCalRefRate2.Value == 0))
                {
                    errorProvider1.SetError(this.spnCalRefRate2, "Please set a Refresh Rate !");
                    validationSuccess = false;
                    spnCalRefRate2.Focus();
                    return validationSuccess;
                }
                else if (txtAlertMsg2.Text.Equals(""))
                {
                    errorProvider1.SetError(this.txtAlertMsg2, "Please enter AlertMessage!");
                    validationSuccess = false;
                    txtAlertMsg2.Focus();
                    return validationSuccess;
                }
                else if (txtEmailAlert2.Text.Equals(""))
                {
                    errorProvider1.SetError(txtEmailAlert2, "Please enter Email address!");
                    txtEmailAlert2.Focus();
                    validationSuccess = false;
                    return validationSuccess;
                }
                else if (!emailMatch2.Success)
                {
                    errorProvider1.SetError(txtEmailAlert2, "Please enter valid Email address!");
                    txtEmailAlert2.Focus();
                    validationSuccess = false;
                    return validationSuccess;
                }

                // checking validation for 3rd row ,if it is enabled. 
                else if (spnExpLimitto2.Value != 100)
                {
                    //if (txtAlertMsg3.Enabled == true)
                    //{
                    Match emailMatch3 = emailRegex.Match(txtEmailAlert3.Text.ToString());

                    errorProvider1.SetError(spnExpoLimit3, "");
                    errorProvider1.SetError(spnExpLimitto3, "");
                    errorProvider1.SetError(spnCalRefRate3, "");
                    errorProvider1.SetError(chkAlertType3, "");
                    errorProvider1.SetError(txtAlertMsg3, "");
                    errorProvider1.SetError(txtEmailAlert3, "");


                    if (int.Parse(spnExpLimitto3.Value.ToString()) < int.Parse(spnExpoLimit3.Value.ToString()))
                    {
                        errorProvider1.SetError(this.spnExpLimitto3, "Please set a correct Upper Exposure Limit!");
                        validationSuccess = false;
                        spnExpLimitto2.Focus();
                        return validationSuccess;
                    }
                    else if (chkAlertType3.CheckedIndex == -1)
                    {
                        errorProvider1.SetError(chkAlertType3, "Please select the alert type!");
                        validationSuccess = false;
                        chkAlertType3.Focus();
                        return validationSuccess;
                    }
                    else if ((chkAlertType3.CheckedIndex == 1) && (spnCalRefRate3.Value == 0))
                    {
                        errorProvider1.SetError(this.spnCalRefRate3, "Please set a Refresh Rate !");
                        validationSuccess = false;
                        spnCalRefRate3.Focus();
                        return validationSuccess;
                    }
                    else if (txtAlertMsg3.Text.Equals(""))
                    {
                        errorProvider1.SetError(this.txtAlertMsg3, "Please enter AlertMessage!");
                        validationSuccess = false;
                        txtAlertMsg3.Focus();
                        return validationSuccess;
                    }
                    else if (txtEmailAlert3.Text.Equals(""))
                    {
                        errorProvider1.SetError(this.txtEmailAlert3, "Please enter Email Address!");
                        validationSuccess = false;
                        txtEmailAlert3.Focus();
                        return validationSuccess;
                    }
                    else if (!emailMatch3.Success)
                    {
                        errorProvider1.SetError(txtEmailAlert3, "Please enter valid Email address!");
                        txtEmailAlert3.Focus();
                        validationSuccess = false;
                        return validationSuccess;
                    }

                    // Checking for validation of 4rth Row , if  it is in enabled state.
                    else if (spnExpLimitto3.Value != 100)
                    {
                        //if (txtAlertMsg4.Enabled == true)
                        //{
                        Match emailMatch4 = emailRegex.Match(txtEmailAlert4.Text.ToString());

                        errorProvider1.SetError(spnExpoLimit4, "");
                        errorProvider1.SetError(spnExpLimitto4, "");
                        errorProvider1.SetError(chkAlertType3, "");
                        errorProvider1.SetError(spnCalRefRate4, "");
                        errorProvider1.SetError(txtAlertMsg4, "");
                        errorProvider1.SetError(txtEmailAlert4, "");

                        if (int.Parse(spnExpLimitto4.Value.ToString()) < int.Parse(spnExpoLimit4.Value.ToString()))
                        {
                            errorProvider1.SetError(this.spnExpLimitto4, "Please set a correct Upper Exposure Limit!");
                            validationSuccess = false;
                            spnExpLimitto4.Focus();
                            return validationSuccess;
                        }
                        else if (chkAlertType4.CheckedIndex == -1)
                        {
                            errorProvider1.SetError(chkAlertType4, "Please select the alert type !");
                            validationSuccess = false;
                            chkAlertType4.Focus();
                            return validationSuccess;
                        }
                        else if ((chkAlertType4.CheckedIndex == 1) && (spnCalRefRate4.Value == 0))
                        {
                            errorProvider1.SetError(this.spnCalRefRate4, "Please set a Refresh Rate !");
                            validationSuccess = false;
                            spnCalRefRate4.Focus();
                            return validationSuccess;
                        }
                        else if (txtAlertMsg4.Text.Equals(""))
                        {
                            errorProvider1.SetError(this.txtAlertMsg4, "Please enter AlertMessage!");
                            validationSuccess = false;
                            txtAlertMsg4.Focus();
                            return validationSuccess;
                        }
                        else if (txtEmailAlert4.Text.Equals(""))
                        {
                            errorProvider1.SetError(this.txtEmailAlert4, "Please enter Email address!");
                            validationSuccess = false;
                            txtEmailAlert4.Focus();
                            return validationSuccess;
                        }
                        else if (!emailMatch4.Success)
                        {
                            errorProvider1.SetError(txtEmailAlert4, "Please enter valid Email address!");
                            txtEmailAlert4.Focus();
                            validationSuccess = false;
                            return validationSuccess;
                        }
                    }
                }
            }
            return validationSuccess = true;
        }
        #endregion Validation Check

        #region DefaultAlert Validation Check

        /// <summary>
        /// The method is used to check the validation for the Default Alerts in the CompanyAlerts Usercontrol
        /// </summary>
        /// <returns></returns>
        public bool DefaultAlertsValidationControl()
        {
            bool validationSuccess = true;

            errorProvider1.SetError(chkDefaultAlertType, "");
            errorProvider1.SetError(spnCalRefRateGrpbx, "");

            if (chkDefaultAlertType.CheckedIndex == -1)
            {
                errorProvider1.SetError(chkDefaultAlertType, "Please select an alert type!");
                chkDefaultAlertType.Focus();
                validationSuccess = false;
                return validationSuccess;
            }
            else if ((chkDefaultAlertType.CheckedIndex == 1) && (spnCalRefRateGrpbx.Value == 0))
            {
                errorProvider1.SetError(this.spnCalRefRateGrpbx, "Please set the Refresh Rate !");
                spnCalRefRateGrpbx.Focus();
                validationSuccess = false;
                return validationSuccess;
            }
            return validationSuccess = true;
        }
        #endregion DefaultAlert Validation Check

        #region InsertRows

        /// <summary>
        /// The method is to conditionally insert the input data from the controls
        ///  to the database by calling the BLL save method
        /// </summary>
        private void InsertRows()
        {
            //Insert query for first row
            CompanyAlert companyAlert = new CompanyAlert();
            companyAlert.CompanyExposureLower = int.Parse(spnExpoLimit1.Value.ToString());
            companyAlert.CompanyExposureUpper = int.Parse(spnExpLimitto1.Value.ToString());
            companyAlert.AlertTypeID = int.Parse(chkAlertType1.CheckedIndex.ToString());
            // If the Alert type is "After Trade", the Refresh Rate is also, saved.
            if (chkAlertType1.CheckedIndex == 1)
            {
                companyAlert.RefreshRateCalculation = int.Parse(spnCalRefRate1.Value.ToString());
            }
            companyAlert.AlertMessage = txtAlertMsg1.Text.ToString();
            companyAlert.EmailAddress = txtEmailAlert1.Text.ToString();
            companyAlert.BlockTrading = chkBlockTrade1.Checked.Equals(true) ? 1 : 0;
            companyAlert.Rank = 1;
            // this calls the SaveCompanyAlerts method from BLL.
            RMAdminBusinessLogic.SaveCompanyAlerts(companyAlert, _companyID);

            //Second row insertion 
            if ((spnExpLimitto1.Value != 100) && (txtAlertMsg2.Enabled == true))
            {

                CompanyAlert companyAlert1 = new CompanyAlert();

                companyAlert1.CompanyExposureLower = int.Parse(spnExpoLimit2.Value.ToString());
                companyAlert1.CompanyExposureUpper = int.Parse(spnExpLimitto2.Value.ToString());
                companyAlert1.AlertTypeID = int.Parse(chkAlertType2.CheckedIndex.ToString());
                // The Refresh Rate is saved, if, the selected Alert is "After Trade". 
                if (chkAlertType2.CheckedIndex == 1)
                {
                    companyAlert1.RefreshRateCalculation = int.Parse(spnCalRefRate2.Value.ToString());
                }
                companyAlert1.AlertMessage = txtAlertMsg2.Text.ToString();
                companyAlert1.EmailAddress = txtEmailAlert2.Text.ToString();
                companyAlert1.BlockTrading = chkBlockTrade1.Checked.Equals(true) ? 1 : 0;
                companyAlert1.Rank = 2;
                // this calls the SaveCompanyAlerts method from BLL.
                RMAdminBusinessLogic.SaveCompanyAlerts(companyAlert1, _companyID);

                ///Third row insertion 
                if ((spnExpLimitto2.Value != 100) && (txtAlertMsg3.Enabled == true))
                {

                    CompanyAlert companyAlert2 = new CompanyAlert();

                    companyAlert2.CompanyExposureLower = int.Parse(spnExpoLimit3.Value.ToString());
                    companyAlert2.CompanyExposureUpper = int.Parse(spnExpLimitto3.Value.ToString());
                    companyAlert2.AlertTypeID = int.Parse(chkAlertType3.CheckedIndex.ToString());
                    // The Refresh Rate is saved, if, the selected Alert is "After Trade". 
                    if (chkAlertType3.CheckedIndex == 1)
                    {
                        companyAlert2.RefreshRateCalculation = int.Parse(spnCalRefRate3.Value.ToString());
                    }
                    companyAlert2.AlertMessage = txtAlertMsg3.Text.ToString();
                    companyAlert2.EmailAddress = txtEmailAlert3.Text.ToString();
                    companyAlert2.BlockTrading = chkBlockTrade3.Checked.Equals(true) ? 1 : 0;
                    companyAlert2.Rank = 3;
                    // this calls the SaveCompanyAlerts method from BLL.
                    RMAdminBusinessLogic.SaveCompanyAlerts(companyAlert2, _companyID);

                    ///Fourth row insertion 
                    if ((spnExpLimitto3.Value != 100) && (txtAlertMsg4.Enabled == true))
                    {

                        CompanyAlert companyAlert3 = new CompanyAlert();

                        companyAlert3.CompanyExposureLower = int.Parse(spnExpoLimit4.Value.ToString());
                        companyAlert3.CompanyExposureUpper = int.Parse(spnExpLimitto4.Value.ToString());
                        companyAlert3.AlertTypeID = int.Parse(chkAlertType4.CheckedIndex.ToString());
                        // The Refresh Rate is saved, if, the selected Alert is "After Trade". 
                        if (chkAlertType4.CheckedIndex == 1)
                        {
                            companyAlert3.RefreshRateCalculation = int.Parse(spnCalRefRate4.Value.ToString());
                        }
                        companyAlert3.AlertMessage = txtAlertMsg4.Text.ToString();
                        companyAlert3.EmailAddress = txtEmailAlert4.Text.ToString();
                        companyAlert3.BlockTrading = chkBlockTrade4.Checked.Equals(true) ? 1 : 0;
                        companyAlert3.Rank = 4;
                        // this calls the SaveCompanyAlerts method from BLL.
                        RMAdminBusinessLogic.SaveCompanyAlerts(companyAlert3, _companyID);
                    }
                }

            }
        }

        #endregion InsertRows

        #region Save method

        public void SaveCompanyAlerts(CompanyAlert companyAlert, int companyID)
        {
            try
            {
                bool IsValid = ValidateControl();
                if (IsValid)
                {
                    RMAdminBusinessLogic.DeleteRMCompanyAlerts(_companyID);
                    InsertRows();
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

        public void SaveDefaultAlert(DefaultAlert defaultAlert, int companyID)
        {
            try
            {
                bool IsDefaultValid = DefaultAlertsValidationControl();
                if (IsDefaultValid)
                {
                    defaultAlert.AlertTypeID = int.Parse(chkDefaultAlertType.CheckedIndex.ToString());
                    defaultAlert.RefreshRateCalculation = int.Parse(spnCalRefRateGrpbx.Value.ToString());
                    RMAdminBusinessLogic.SaveDefaultAlert(defaultAlert, _companyID);
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

        #endregion Save method

        #region ExpLtIntervals Focus Property
        private void spnExpoLimit1_Enter(object sender, EventArgs e)
        {
            spnExpoLimit1.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnExpoLimit1_Leave(object sender, EventArgs e)
        {
            spnExpoLimit1.BackColor = Color.White;
        }

        private void spnExpLimitto1_Enter(object sender, EventArgs e)
        {
            spnExpLimitto1.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnExpLimitto1_Leave(object sender, EventArgs e)
        {
            spnExpLimitto1.BackColor = Color.White;
        }

        private void spnExpoLimit2_Enter(object sender, EventArgs e)
        {
            spnExpoLimit2.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnExpoLimit2_Leave(object sender, EventArgs e)
        {
            spnExpoLimit2.BackColor = Color.White;
        }

        private void spnExpLimitto2_Enter(object sender, EventArgs e)
        {
            spnExpLimitto2.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnExpLimitto2_Leave(object sender, EventArgs e)
        {
            spnExpLimitto2.BackColor = Color.White;
        }

        private void spnExpoLimit3_Enter(object sender, EventArgs e)
        {
            spnExpoLimit3.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnExpoLimit3_Leave(object sender, EventArgs e)
        {
            spnExpoLimit3.BackColor = Color.White;
        }

        private void spnExpLimitto3_Enter(object sender, EventArgs e)
        {
            spnExpLimitto3.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnExpLimitto3_Leave(object sender, EventArgs e)
        {
            spnExpLimitto3.BackColor = Color.White;
        }

        private void spnExpoLimit4_Enter(object sender, EventArgs e)
        {
            spnExpoLimit4.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnExpoLimit4_Leave(object sender, EventArgs e)
        {
            spnExpoLimit4.BackColor = Color.White;
        }

        private void spnExpLimitto4_Enter(object sender, EventArgs e)
        {
            spnExpLimitto4.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnExpLimitto4_Leave(object sender, EventArgs e)
        {
            spnExpLimitto3.BackColor = Color.White;
        }

        #endregion ExpLtIntervals Focus Property

        #region RefreshRate Focus Property

        //private void chkAlertType1_Enter(object sender, EventArgs e)
        //{
        //    chkAlertType1.BackColor = Color.FromArgb(255, 250, 205);
        //}

        //private void chkAlertType1_Leave(object sender, EventArgs e)
        //{
        //    chkAlertType1.BackColor = Color.White;
        //}

        //private void chkAlertType2_Enter(object sender, EventArgs e)
        //{
        //    chkAlertType2.BackColor = Color.FromArgb(255, 250, 205);
        //}

        //private void chkAlertType2_Leave(object sender, EventArgs e)
        //{
        //    chkAlertType2.BackColor = Color.White;
        //}

        //private void chkAlertType3_Enter(object sender, EventArgs e)
        //{
        //    chkAlertType3.BackColor = Color.FromArgb(255, 250, 205);
        //}

        //private void chkAlertType3_Leave(object sender, EventArgs e)
        //{
        //    chkAlertType3.BackColor = Color.White;
        //}

        //private void chkAlertType4_Enter(object sender, EventArgs e)
        //{
        //    chkAlertType4.BackColor = Color.FromArgb(255, 250, 205);
        //}

        //private void chkAlertType4_Leave(object sender, EventArgs e)
        //{
        //    chkAlertType4.BackColor = Color.White;
        //}

        private void spnCalRefRate1_Enter(object sender, EventArgs e)
        {
            spnCalRefRate1.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnCalRefRate1_Leave(object sender, EventArgs e)
        {
            spnCalRefRate1.BackColor = Color.White;
        }

        private void spnCalRefRate2_Enter(object sender, EventArgs e)
        {
            spnCalRefRate2.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnCalRefRate2_Leave(object sender, EventArgs e)
        {
            spnCalRefRate2.BackColor = Color.White;
        }

        private void spnCalRefRate3_Enter(object sender, EventArgs e)
        {
            spnCalRefRate3.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnCalRefRate3_Leave(object sender, EventArgs e)
        {
            spnCalRefRate3.BackColor = Color.White;
        }

        private void spnCalRefRate4_Enter(object sender, EventArgs e)
        {
            spnCalRefRate4.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnCalRefRate4_Leave(object sender, EventArgs e)
        {
            spnCalRefRate4.BackColor = Color.White;
        }


        #endregion RefreshRate Focus Property

        #region Focus Property Of Alertmsg, email, default
        private void txtAlertMsg1_Enter(object sender, EventArgs e)
        {
            txtAlertMsg1.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtAlertMsg1_Leave(object sender, EventArgs e)
        {
            txtAlertMsg1.Appearance.BackColor = Color.White;
        }

        private void txtAlertMsg2_Enter(object sender, EventArgs e)
        {
            txtAlertMsg2.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtAlertMsg2_Leave(object sender, EventArgs e)
        {
            txtAlertMsg2.Appearance.BackColor = Color.White;
        }

        private void txtAlertMsg3_Enter(object sender, EventArgs e)
        {
            txtAlertMsg3.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtAlertMsg3_Leave(object sender, EventArgs e)
        {
            txtAlertMsg3.Appearance.BackColor = Color.White;
        }

        private void txtAlertMsg4_Enter(object sender, EventArgs e)
        {
            txtAlertMsg4.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtAlertMsg4_Leave(object sender, EventArgs e)
        {
            txtAlertMsg4.Appearance.BackColor = Color.White;
        }

        private void txtEmailAlert1_Enter(object sender, EventArgs e)
        {
            txtEmailAlert1.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtEmailAlert1_Leave(object sender, EventArgs e)
        {
            txtEmailAlert1.Appearance.BackColor = Color.White;
        }

        private void txtEmailAlert2_Enter(object sender, EventArgs e)
        {
            txtEmailAlert2.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtEmailAlert2_Leave(object sender, EventArgs e)
        {
            txtEmailAlert2.Appearance.BackColor = Color.White;
        }

        private void txtEmailAlert3_Enter(object sender, EventArgs e)
        {
            txtEmailAlert3.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtEmailAlert3_Leave(object sender, EventArgs e)
        {
            txtEmailAlert3.Appearance.BackColor = Color.White;
        }

        private void txtEmailAlert4_Enter(object sender, EventArgs e)
        {
            txtEmailAlert4.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtEmailAlert4_Leave(object sender, EventArgs e)
        {
            txtAlertMsg1.Appearance.BackColor = Color.White;
        }
        private void spnCalRefRateGrpbx_Enter(object sender, EventArgs e)
        {
            spnCalRefRateGrpbx.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void spnCalRefRateGrpbx_Leave(object sender, EventArgs e)
        {
            spnCalRefRateGrpbx.BackColor = Color.White;
        }

        //private void chkDefaultAlertType_Enter(object sender, EventArgs e)
        //{
        //    chkDefaultAlertType.BackColor = Color.FromArgb(255, 250, 205);
        //}

        //private void chkDefaultAlertType_Leave(object sender, EventArgs e)
        //{
        //    chkDefaultAlertType.BackColor = Color.White;
        //}
        #endregion Focus Property Of Alertmsg, email, default

        #region Form Load
        private void Company_Alerts_Load(object sender, EventArgs e)
        {
            try
            {
                //spnExpLimitto1.ValueChanged += new EventHandler(spnExpLimitto1_ValueChanged);
                spnExpLimitto1.ValueChanged += new EventHandler(spnExpLimitto1_ValueChanged);
                spnExpLimitto2.ValueChanged += new EventHandler(spnExpLimitto2_ValueChanged);
                spnExpLimitto3.ValueChanged += new EventHandler(spnExpLimitto3_ValueChanged);
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
        #endregion Form load

        #region Spinners Value Changed event
        private void spnExpLimitto3_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (spnExpLimitto3.Value != 100 && spnExpLimitto3.Value != 0)
                {
                    EnableFourthRow();
                    RefreshFourthRow();
                    int expLt = Convert.ToInt32(spnExpLimitto3.Value);
                    int a = expLt++;
                    spnExpoLimit4.Value = a;
                    spnExpoLimit4.BackColor = Color.White;
                    spnExpLimitto4.Value = 100;
                    spnExpLimitto4.BackColor = Color.White;
                }
                else
                {
                    RefreshFourthRow();
                    DisableFourthRow();
                    spnExpoLimit4.BackColor = Color.Gray;
                    spnExpLimitto4.BackColor = Color.Gray;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void spnExpLimitto2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (spnExpLimitto2.Value != 100 && spnExpLimitto2.Value != 0)
                {
                    EnableThirdRow();
                    RefreshThirdRow();
                    int expLt = Convert.ToInt32(spnExpLimitto2.Value);
                    int a = expLt++;
                    spnExpoLimit3.Value = a;
                    spnExpoLimit3.BackColor = Color.White;
                    spnExpLimitto3.Value = 100;
                    spnExpLimitto3.BackColor = Color.White;
                }
                else
                {
                    RefreshThirdRow();
                    DisableThirdRow();
                    spnExpoLimit3.BackColor = Color.Gray;
                    spnExpLimitto3.BackColor = Color.Gray;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

        }

        private void spnExpLimitto1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                /* 1. if 100, disable next row
                 2. else enable n refresh
                 3. nd pass value to next level
                 4. if 100, bkcolor of next level
                 nd else also*/

                if (spnExpLimitto1.Value < 100)
                {
                    EnableSecondRow();
                    RefreshSecondRow();
                    int expLt = Convert.ToInt32(spnExpLimitto1.Value);
                    int a = expLt++;
                    spnExpoLimit2.Value = a;
                    spnExpoLimit2.BackColor = Color.White;
                    spnExpLimitto2.Value = 100;
                    spnExpLimitto2.BackColor = Color.White;

                }
                else
                {
                    RefreshSecondRow();
                    DisableSecondRow();
                    spnExpoLimit2.BackColor = Color.Gray;
                    spnExpLimitto2.BackColor = Color.Gray;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }
        #endregion Spinners Value Changed event

        #region BlockTrade Alert

        private void chkBlockTrade1_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkBlockTrade1.CheckState == CheckState.Checked)
            {
                if (MessageBox.Show(this, "This is a critical functionality. Are you sure you want to select it?", "RM ADMIN ALERT", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    chkBlockTrade1.CheckState = CheckState.Checked;
                }
                else
                {
                    chkBlockTrade1.CheckState = CheckState.Unchecked;
                }
            }
        }

        private void chkBlockTrade2_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkBlockTrade2.CheckState == CheckState.Checked)
            {
                if (MessageBox.Show(this, "This is a critical functionality. Are you sure you want to select it?", "RM ADMIN ALERT", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    chkBlockTrade2.CheckState = CheckState.Checked;
                }
                else
                {
                    chkBlockTrade2.CheckState = CheckState.Unchecked;
                }
            }
        }

        private void chkBlockTrade3_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkBlockTrade3.CheckState == CheckState.Checked)
            {
                if (MessageBox.Show(this, "This is a critical functionality. Are you sure you want to select it?", "RM ADMIN ALERT", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    chkBlockTrade3.CheckState = CheckState.Checked;
                }
                else
                {
                    chkBlockTrade3.CheckState = CheckState.Unchecked;
                }
            }
        }

        private void chkBlockTrade4_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkBlockTrade4.CheckState == CheckState.Checked)
            {
                if (MessageBox.Show(this, "This is a critical functionality. Are you sure you want to select it?", "RM ADMIN ALERT", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    chkBlockTrade4.CheckState = CheckState.Checked;
                }
                else
                {
                    chkBlockTrade4.CheckState = CheckState.Unchecked;
                }
            }
        }

        #endregion BlockTrade Alert







    }
}



