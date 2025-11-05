#region Using
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;
#endregion

namespace Prana.Admin.RiskManagement.Controls
{
    public delegate void UserChangedHandler(System.Object sender, ValueEventArgs e);
    /// <summary>
    /// Summary description for UserLevel_OverallLimits.
    /// </summary>
    public class uctUserLevelOverallLimits : System.Windows.Forms.UserControl
    {

        #region Wizard Stuff

        const string C_COMBO_SELECT = "- Select -";

        private int _userIDSelected = int.MinValue;
        #region private and protected members

        private int _companyID = int.MinValue;
        private int _companyUserID = int.MinValue;
        private System.Windows.Forms.ErrorProvider errorProvider1;

        public uctCompanyOverallLimits uctcompanyOverall;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdUserOverall;
        private System.Windows.Forms.GroupBox grpBxUserOverall;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        //		private System.Windows.Forms.Label uneditExpLt;
        //		private System.Windows.Forms.Label uneditMaxPNL;
        //		private System.Windows.Forms.Label uneditMaxbasket;
        //		private System.Windows.Forms.Label uneditMaxOrder;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbUser;
        //		private System.Windows.Forms.Label uneditExpLt;
        //		private System.Windows.Forms.Label uneditMaxPNL;
        //		private System.Windows.Forms.Label uneditMaxbasket;
        //		private System.Windows.Forms.Label uneditMaxOrder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtMaxSizeperOrder;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtExposureLimitUserLevel;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtMaxSizeperBasket;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtMaxPNLLoss;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtRMUserOverallID;
        private Infragistics.Win.Misc.UltraLabel lblCurr3;
        private Infragistics.Win.Misc.UltraLabel lblCurr2;
        private Infragistics.Win.Misc.UltraLabel lblCurr1;
        private Infragistics.Win.Misc.UltraLabel lblCurr;
        private IContainer components;


        #endregion
        public event UserChangedHandler UserChanged;
        public int CompanyID
        {
            set { _companyID = value; }
        }
        public int CompanyUserID
        {
            set
            {
                _companyUserID = value;
            }
        }

        public uctUserLevelOverallLimits()
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
                if (grdUserOverall != null)
                {
                    grdUserOverall.Dispose();
                }
                if (grpBxUserOverall != null)
                {
                    grpBxUserOverall.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (cmbUser != null)
                {
                    cmbUser.Dispose();
                }
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if (label11 != null)
                {
                    label11.Dispose();
                }
                if (label13 != null)
                {
                    label13.Dispose();
                }
                if (label14 != null)
                {
                    label14.Dispose();
                }
                if (txtMaxSizeperOrder != null)
                {
                    txtMaxSizeperOrder.Dispose();
                }
                if (txtExposureLimitUserLevel != null)
                {
                    txtExposureLimitUserLevel.Dispose();
                }
                if (txtMaxSizeperBasket != null)
                {
                    txtMaxSizeperBasket.Dispose();
                }
                if (txtMaxPNLLoss != null)
                {
                    txtMaxPNLLoss.Dispose();
                }
                if (txtRMUserOverallID != null)
                {
                    txtRMUserOverallID.Dispose();
                }
                if (lblCurr3 != null)
                {
                    lblCurr3.Dispose();
                }
                if (lblCurr2 != null)
                {
                    lblCurr2.Dispose();
                }
                if (lblCurr1 != null)
                {
                    lblCurr1.Dispose();
                }
                if (lblCurr != null)
                {
                    lblCurr.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RMCompanyUserID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyUserID", 1);
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 2);
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserExposureLimit", 3);
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MaximumPNLLoss", 4);
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MaximumSizePerOrder", 5);
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MaximumSizePerBasket", 6);
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn29 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 7);
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephonePager", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FirstName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingPermission", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Password", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneWork", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address1", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address2", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("EMail", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateID", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LastName", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Title", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsActive", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneHome", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneMobile", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LoginName", 17);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Zip", 18);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID", 19);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 20);
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
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grdUserOverall = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grpBxUserOverall = new System.Windows.Forms.GroupBox();
            this.lblCurr3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCurr2 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCurr1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCurr = new Infragistics.Win.Misc.UltraLabel();
            this.txtMaxSizeperOrder = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtExposureLimitUserLevel = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtMaxSizeperBasket = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtMaxPNLLoss = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbUser = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRMUserOverallID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdUserOverall)).BeginInit();
            this.grpBxUserOverall.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxSizeperOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExposureLimitUserLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxSizeperBasket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxPNLLoss)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRMUserOverallID)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grdUserOverall
            // 
            this.grdUserOverall.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grdUserOverall.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn22.Header.VisiblePosition = 0;
            ultraGridColumn22.Hidden = true;
            ultraGridColumn22.Width = 195;
            appearance14.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn23.CellAppearance = appearance14;
            appearance15.FontData.BoldAsString = "True";
            appearance15.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn23.Header.Appearance = appearance15;
            ultraGridColumn23.Header.VisiblePosition = 1;
            ultraGridColumn23.Hidden = true;
            ultraGridColumn23.Width = 112;
            appearance16.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn24.CellAppearance = appearance16;
            ultraGridColumn24.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
            ultraGridColumn24.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
            ultraGridColumn24.Format = "";
            appearance17.FontData.BoldAsString = "True";
            appearance17.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn24.Header.Appearance = appearance17;
            ultraGridColumn24.Header.Caption = "User ";
            ultraGridColumn24.Header.VisiblePosition = 2;
            ultraGridColumn24.Width = 103;
            appearance18.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn25.CellAppearance = appearance18;
            appearance19.FontData.BoldAsString = "True";
            ultraGridColumn25.Header.Appearance = appearance19;
            ultraGridColumn25.Header.Caption = "User\'s Exposure Limit";
            ultraGridColumn25.Header.VisiblePosition = 3;
            ultraGridColumn25.Width = 125;
            appearance20.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn26.CellAppearance = appearance20;
            appearance21.FontData.BoldAsString = "True";
            appearance21.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn26.Header.Appearance = appearance21;
            ultraGridColumn26.Header.Caption = "Maximum PNL Loss";
            ultraGridColumn26.Header.VisiblePosition = 4;
            ultraGridColumn26.Width = 128;
            appearance22.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn27.CellAppearance = appearance22;
            appearance23.FontData.BoldAsString = "True";
            appearance23.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn27.Header.Appearance = appearance23;
            ultraGridColumn27.Header.Caption = "Maximum Size Per Order";
            ultraGridColumn27.Header.VisiblePosition = 5;
            ultraGridColumn27.Width = 146;
            appearance24.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn28.CellAppearance = appearance24;
            appearance25.FontData.BoldAsString = "True";
            appearance25.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn28.Header.Appearance = appearance25;
            ultraGridColumn28.Header.Caption = "Maximum Size Per Basket";
            ultraGridColumn28.Header.VisiblePosition = 6;
            ultraGridColumn28.Width = 130;
            appearance26.FontData.BoldAsString = "True";
            appearance26.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn29.Header.Appearance = appearance26;
            ultraGridColumn29.Header.VisiblePosition = 7;
            ultraGridColumn29.Hidden = true;
            ultraGridColumn29.Width = 85;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn22,
            ultraGridColumn23,
            ultraGridColumn24,
            ultraGridColumn25,
            ultraGridColumn26,
            ultraGridColumn27,
            ultraGridColumn28,
            ultraGridColumn29});
            ultraGridBand2.Header.Enabled = false;
            ultraGridBand2.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            ultraGridBand2.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdUserOverall.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdUserOverall.DisplayLayout.GroupByBox.Hidden = true;
            this.grdUserOverall.DisplayLayout.MaxColScrollRegions = 1;
            this.grdUserOverall.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdUserOverall.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdUserOverall.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdUserOverall.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdUserOverall.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            this.grdUserOverall.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdUserOverall.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdUserOverall.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdUserOverall.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdUserOverall.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdUserOverall.Location = new System.Drawing.Point(6, 6);
            this.grdUserOverall.Name = "grdUserOverall";
            this.grdUserOverall.Size = new System.Drawing.Size(653, 108);
            this.grdUserOverall.TabIndex = 40;
            this.grdUserOverall.AfterRowActivate += new System.EventHandler(this.grdUserOverall_AfterRowActivate);
            // 
            // grpBxUserOverall
            // 
            this.grpBxUserOverall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBxUserOverall.Controls.Add(this.lblCurr3);
            this.grpBxUserOverall.Controls.Add(this.lblCurr2);
            this.grpBxUserOverall.Controls.Add(this.lblCurr1);
            this.grpBxUserOverall.Controls.Add(this.lblCurr);
            this.grpBxUserOverall.Controls.Add(this.txtMaxSizeperOrder);
            this.grpBxUserOverall.Controls.Add(this.txtExposureLimitUserLevel);
            this.grpBxUserOverall.Controls.Add(this.txtMaxSizeperBasket);
            this.grpBxUserOverall.Controls.Add(this.txtMaxPNLLoss);
            this.grpBxUserOverall.Controls.Add(this.label14);
            this.grpBxUserOverall.Controls.Add(this.label13);
            this.grpBxUserOverall.Controls.Add(this.label11);
            this.grpBxUserOverall.Controls.Add(this.label9);
            this.grpBxUserOverall.Controls.Add(this.cmbUser);
            this.grpBxUserOverall.Controls.Add(this.label1);
            this.grpBxUserOverall.Controls.Add(this.label2);
            this.grpBxUserOverall.Controls.Add(this.label3);
            this.grpBxUserOverall.Controls.Add(this.label4);
            this.grpBxUserOverall.Controls.Add(this.label5);
            this.grpBxUserOverall.Location = new System.Drawing.Point(97, 117);
            this.grpBxUserOverall.Name = "grpBxUserOverall";
            this.grpBxUserOverall.Size = new System.Drawing.Size(472, 130);
            this.grpBxUserOverall.TabIndex = 41;
            this.grpBxUserOverall.TabStop = false;
            // 
            // lblCurr3
            // 
            this.lblCurr3.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblCurr3.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblCurr3.Location = new System.Drawing.Point(399, 108);
            this.lblCurr3.Name = "lblCurr3";
            this.lblCurr3.Size = new System.Drawing.Size(24, 17);
            this.lblCurr3.TabIndex = 74;
            this.lblCurr3.Text = "usd";
            // 
            // lblCurr2
            // 
            this.lblCurr2.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblCurr2.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblCurr2.Location = new System.Drawing.Point(399, 84);
            this.lblCurr2.Name = "lblCurr2";
            this.lblCurr2.Size = new System.Drawing.Size(24, 17);
            this.lblCurr2.TabIndex = 73;
            this.lblCurr2.Text = "usd";
            // 
            // lblCurr1
            // 
            this.lblCurr1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblCurr1.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblCurr1.Location = new System.Drawing.Point(399, 61);
            this.lblCurr1.Name = "lblCurr1";
            this.lblCurr1.Size = new System.Drawing.Size(24, 17);
            this.lblCurr1.TabIndex = 72;
            this.lblCurr1.Text = "usd";
            // 
            // lblCurr
            // 
            this.lblCurr.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblCurr.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblCurr.Location = new System.Drawing.Point(399, 39);
            this.lblCurr.Name = "lblCurr";
            this.lblCurr.Size = new System.Drawing.Size(24, 17);
            this.lblCurr.TabIndex = 43;
            this.lblCurr.Text = "usd";
            // 
            // txtMaxSizeperOrder
            // 
            this.txtMaxSizeperOrder.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtMaxSizeperOrder.Location = new System.Drawing.Point(238, 82);
            this.txtMaxSizeperOrder.Name = "txtMaxSizeperOrder";
            this.txtMaxSizeperOrder.Size = new System.Drawing.Size(155, 20);
            this.txtMaxSizeperOrder.TabIndex = 71;
            this.txtMaxSizeperOrder.Enter += new System.EventHandler(this.txtMaxSizeperOrder_Enter);
            this.txtMaxSizeperOrder.TextChanged += new System.EventHandler(this.txtMaxSizeperOrder_TextChanged);
            this.txtMaxSizeperOrder.Leave += new System.EventHandler(this.txtMaxSizeperOrder_Leave);
            // 
            // txtExposureLimitUserLevel
            // 
            this.txtExposureLimitUserLevel.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtExposureLimitUserLevel.Location = new System.Drawing.Point(238, 36);
            this.txtExposureLimitUserLevel.Name = "txtExposureLimitUserLevel";
            this.txtExposureLimitUserLevel.Size = new System.Drawing.Size(155, 20);
            this.txtExposureLimitUserLevel.TabIndex = 69;
            this.txtExposureLimitUserLevel.Enter += new System.EventHandler(this.txtExposureLimitUserLevel_Enter);
            this.txtExposureLimitUserLevel.TextChanged += new System.EventHandler(this.txtExposureLimitUserLevel_TextChanged);
            this.txtExposureLimitUserLevel.Leave += new System.EventHandler(this.txtExposureLimitUserLevel_Leave);
            // 
            // txtMaxSizeperBasket
            // 
            this.txtMaxSizeperBasket.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtMaxSizeperBasket.Location = new System.Drawing.Point(238, 105);
            this.txtMaxSizeperBasket.Name = "txtMaxSizeperBasket";
            this.txtMaxSizeperBasket.Size = new System.Drawing.Size(155, 20);
            this.txtMaxSizeperBasket.TabIndex = 68;
            this.txtMaxSizeperBasket.Enter += new System.EventHandler(this.txtMaxSizeperBasket_Enter);
            this.txtMaxSizeperBasket.TextChanged += new System.EventHandler(this.txtMaxSizeperBasket_TextChanged);
            this.txtMaxSizeperBasket.Leave += new System.EventHandler(this.txtMaxSizeperBasket_Leave);
            // 
            // txtMaxPNLLoss
            // 
            this.txtMaxPNLLoss.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtMaxPNLLoss.Location = new System.Drawing.Point(238, 59);
            this.txtMaxPNLLoss.Name = "txtMaxPNLLoss";
            this.txtMaxPNLLoss.Size = new System.Drawing.Size(155, 20);
            this.txtMaxPNLLoss.TabIndex = 70;
            this.txtMaxPNLLoss.Enter += new System.EventHandler(this.txtMaxPNLLoss_Enter);
            this.txtMaxPNLLoss.TextChanged += new System.EventHandler(this.txtMaxPNLLoss_TextChanged);
            this.txtMaxPNLLoss.Leave += new System.EventHandler(this.txtMaxPNLLoss_Leave);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(224, 108);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 13);
            this.label14.TabIndex = 67;
            this.label14.Text = "*";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(224, 84);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(13, 13);
            this.label13.TabIndex = 66;
            this.label13.Text = "*";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(224, 36);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 13);
            this.label11.TabIndex = 64;
            this.label11.Text = "*";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(224, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 13);
            this.label9.TabIndex = 63;
            this.label9.Text = "*";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbUser
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbUser.ButtonAppearance = appearance1;
            this.cmbUser.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbUser.DisplayLayout.Appearance = appearance2;
            this.cmbUser.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Hidden = true;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn7.Hidden = true;
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn9.Hidden = true;
            ultraGridColumn10.Header.VisiblePosition = 9;
            ultraGridColumn11.Header.VisiblePosition = 10;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn12.Header.VisiblePosition = 11;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn13.Header.VisiblePosition = 12;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn14.Header.VisiblePosition = 13;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn15.Header.VisiblePosition = 14;
            ultraGridColumn15.Hidden = true;
            ultraGridColumn16.Header.VisiblePosition = 15;
            ultraGridColumn16.Hidden = true;
            ultraGridColumn17.Header.VisiblePosition = 16;
            ultraGridColumn17.Hidden = true;
            ultraGridColumn18.Header.VisiblePosition = 17;
            ultraGridColumn18.Hidden = true;
            ultraGridColumn19.Header.VisiblePosition = 18;
            ultraGridColumn19.Hidden = true;
            ultraGridColumn20.Header.VisiblePosition = 19;
            ultraGridColumn20.Hidden = true;
            ultraGridColumn21.Header.VisiblePosition = 20;
            ultraGridColumn21.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13,
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn16,
            ultraGridColumn17,
            ultraGridColumn18,
            ultraGridColumn19,
            ultraGridColumn20,
            ultraGridColumn21});
            ultraGridBand1.Hidden = true;
            this.cmbUser.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbUser.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbUser.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUser.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUser.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.cmbUser.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUser.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.cmbUser.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbUser.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbUser.DisplayLayout.Override.ActiveCellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbUser.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.cmbUser.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbUser.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            this.cmbUser.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Silver;
            appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbUser.DisplayLayout.Override.CellAppearance = appearance9;
            this.cmbUser.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbUser.DisplayLayout.Override.CellPadding = 0;
            appearance10.BackColor = System.Drawing.SystemColors.Control;
            appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUser.DisplayLayout.Override.GroupByRowAppearance = appearance10;
            appearance11.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbUser.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.cmbUser.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbUser.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            this.cmbUser.DisplayLayout.Override.RowAppearance = appearance12;
            this.cmbUser.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbUser.DisplayLayout.Override.TemplateAddRowAppearance = appearance13;
            this.cmbUser.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Both;
            this.cmbUser.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbUser.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbUser.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbUser.DisplayMember = "";
            this.cmbUser.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbUser.DropDownWidth = 0;
            this.cmbUser.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbUser.Location = new System.Drawing.Point(238, 12);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(155, 21);
            this.cmbUser.TabIndex = 22;
            this.cmbUser.ValueMember = "";
            this.cmbUser.Leave += new System.EventHandler(this.cmbUser_Leave);
            this.cmbUser.ValueChanged += new System.EventHandler(this.cmbUser_ValueChanged);
            this.cmbUser.Enter += new System.EventHandler(this.cmbUser_Enter);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "Maximum size per basket";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Maximum size per order";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(206, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Maximum PNL Loss ";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(181, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Exposure Limit";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "User";
            // 
            // txtRMUserOverallID
            // 
            this.txtRMUserOverallID.Location = new System.Drawing.Point(588, 130);
            this.txtRMUserOverallID.Name = "txtRMUserOverallID";
            this.txtRMUserOverallID.Size = new System.Drawing.Size(36, 22);
            this.txtRMUserOverallID.TabIndex = 42;
            this.txtRMUserOverallID.Text = "-1";
            this.txtRMUserOverallID.Visible = false;
            // 
            // uctUserLevelOverallLimits
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.txtRMUserOverallID);
            this.Controls.Add(this.grpBxUserOverall);
            this.Controls.Add(this.grdUserOverall);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "uctUserLevelOverallLimits";
            this.Size = new System.Drawing.Size(666, 252);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdUserOverall)).EndInit();
            this.grpBxUserOverall.ResumeLayout(false);
            this.grpBxUserOverall.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxSizeperOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExposureLimitUserLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxSizeperBasket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxPNLLoss)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRMUserOverallID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Bind Users Combo and Grid Binding

        /// <summary>
        /// This method binds the existing <see cref="Currency"/> in the ComboBox control by assigning the 
        /// companyTypes object to its datasource property.
        /// </summary>
        private void BindUser()
        {
            try
            {
                if (_companyID != int.MinValue)
                {
                    //Prana.Admin.BLL.Users users = UserManager.GetCompanyUsers(_companyID);
                    Prana.Admin.BLL.Users users = RMAdminBusinessLogic.GetCompanyUsers(_companyID);

                    //Inserting the - Select - option in the Combo Box at the top.
                    users.Insert(0, new User(int.MinValue, "", "", C_COMBO_SELECT, "", "", "", "", "", ""));

                    //					if(users != null)
                    if (users.Count > 1)
                    {

                        this.cmbUser.ValueChanged -= new System.EventHandler(this.cmbUser_ValueChanged);
                        this.cmbUser.DataSource = null;
                        this.cmbUser.DataSource = users;
                        this.cmbUser.DisplayMember = "ShortName";
                        this.cmbUser.ValueMember = "UserID";
                        this.cmbUser.Text = C_COMBO_SELECT;
                        this.cmbUser.Value = int.MinValue;

                        this.cmbUser.ValueChanged += new System.EventHandler(this.cmbUser_ValueChanged);

                        ColumnsCollection columns = cmbUser.DisplayLayout.Bands[0].Columns;
                        foreach (UltraGridColumn column in columns)
                        {
                            //Columns other than "TradingAccountName" are set as hidden.
                            if (column.Key != "ShortName")
                            {
                                column.Hidden = true;
                            }
                            else
                            {
                                // The headers are set to invisible.
                                cmbUser.DisplayLayout.Bands[0].ColHeadersVisible = false;
                            }
                        }

                    }

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
            #endregion Catch
        }


        /// <summary>
        /// binding userOverallLimits grid
        /// </summary>
        private void BindUserOverallGrid()
        {
            //Fetching the existing data from the database and binding it to the grid.
            UserLevelOverallLimits userLevelOverallLimits = RMAdminBusinessLogic.GetUserLevelOverallLimits(_companyID);

            //Assigning the userlevel Overall limit  grid's datasource property to userLevelOverallLimits object if it has some values.
            if (userLevelOverallLimits.Count != 0)
            {
                foreach (UserLevelOverallLimit userLevelOverallLimit in userLevelOverallLimits)
                {
                    User user = RMAdminBusinessLogic.GetCompanyUser(userLevelOverallLimit.CompanyUserID);
                    string userName = user.ShortName;
                    userLevelOverallLimit.ShortName = userName;

                }
                //assigning the grid's datasource to the userLevelOverallLimits object.
                grdUserOverall.DataSource = userLevelOverallLimits;

            }
            else
            {
                userLevelOverallLimits = new UserLevelOverallLimits();
                userLevelOverallLimits.Add(new Prana.Admin.BLL.UserLevelOverallLimit());
                grdUserOverall.DataSource = userLevelOverallLimits;

                grdUserOverall.DisplayLayout.Rows[0].Delete(false);

                RefreshUserLevelOverallLimitDetail();
            }

        }

        #endregion

        #region Validation Method

        /// <summary>
        /// This is to check the validation of the controls used on the usercontrol
        /// </summary>
        /// <returns></returns>
        public bool ValidateControl()
        {
            bool validationSuccess = true;

            errorProvider1.SetError(cmbUser, "");
            errorProvider1.SetError(txtExposureLimitUserLevel, "");

            errorProvider1.SetError(txtMaxSizeperOrder, "");
            errorProvider1.SetError(txtMaxSizeperBasket, "");

            if (int.Parse(cmbUser.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbUser, "Please select User!");
                validationSuccess = false;
                cmbUser.Focus();
                return validationSuccess;
            }
            else if (!DataTypeValidation.ValidateNumeric(txtExposureLimitUserLevel.Text.Trim()))
            {
                errorProvider1.SetError(txtExposureLimitUserLevel, "Please enter numeric value for Exposure Limit!");
                validationSuccess = false;
                txtExposureLimitUserLevel.Focus();
                return validationSuccess;
            }
            else if (txtMaxPNLLoss.Text != "")
            {
                if (!DataTypeValidation.ValidateNumeric(txtMaxPNLLoss.Text.Trim()))
                {
                    errorProvider1.SetError(txtMaxPNLLoss, "Please enter Numeric value for Maximum PNL Loss!");
                    validationSuccess = false;
                    txtMaxPNLLoss.Focus();
                    return validationSuccess;
                }
            }
            else if (!DataTypeValidation.ValidateNumeric(txtMaxSizeperOrder.Text.Trim()))
            {
                errorProvider1.SetError(txtMaxSizeperOrder, "Please enter numeric value for Maximum size per Order!");
                validationSuccess = false;
                txtMaxSizeperOrder.Focus();
                return validationSuccess;
            }
            else if (!DataTypeValidation.ValidateNumeric(txtMaxSizeperBasket.Text.Trim()))
            {
                errorProvider1.SetError(txtMaxSizeperBasket, "Please enter numeric value for Maximum Size per Basket!");
                validationSuccess = false;
                txtMaxSizeperBasket.Focus();
                return validationSuccess;
            }
            return validationSuccess;

        }

        #endregion Validation Method

        #region  Set Methods

        //UserLevelOverallLimit property sets the UserLevelOverallLimit form and get the UserLevelOverallLimit details from it. 
        public UserLevelOverallLimit UserLevelOverallLimit
        {
            set
            { SetUserLevelOverallLimit(value); }
        }

        /// <summary>
        /// Shows all the details in the respective controls pertaining to that paricular User.
        /// </summary>
        /// <param name="User"></param>
        private void SetUserLevelOverallLimit(UserLevelOverallLimit userLevelOverallLimit)
        {
            BindUser();
            BindUserOverallGrid();
            if (userLevelOverallLimit != null && userLevelOverallLimit.CompanyUserID != int.MinValue)
            {
                cmbUser.Value = userLevelOverallLimit.CompanyUserID;
                txtExposureLimitUserLevel.Text = userLevelOverallLimit.UserExposureLimit.ToString();
                txtMaxPNLLoss.Text = userLevelOverallLimit.MaximumPNLLoss.ToString();
                txtMaxSizeperOrder.Text = userLevelOverallLimit.MaximumSizePerOrder.ToString();
                txtMaxSizeperBasket.Text = userLevelOverallLimit.MaximumSizePerBasket.ToString();
            }
            else
            {
                if (_companyUserID != int.MinValue)
                {
                    cmbUser.Value = _companyUserID;
                    RefreshUserLevelOverallLimitDetail();
                }
                else
                {
                    cmbUser.Text = C_COMBO_SELECT;
                    RefreshUserLevelOverallLimitDetail();
                }
            }

        }



        #endregion Public Methods and Private Methods 

        #region Refresh Method
        /// <summary>
        /// Blanks all the textboxes in the form. 
        /// </summary>
        private void RefreshUserLevelOverallLimitDetail()
        {

            txtExposureLimitUserLevel.Text = "";
            txtMaxPNLLoss.Text = "";
            txtMaxSizeperOrder.Text = "";
            txtMaxSizeperBasket.Text = "";
            txtRMUserOverallID.Text = "-1";
        }

        #endregion Refresh Method

        #region Passing Value of selected UserID to Tree on combo value change

        /// <summary>
        /// This event Initiates the eventhandler for passing the UserID as selected in UserCombo to Main Form 
        /// and the UserUIControl .Besides, it also, sets the corresponding row in grid to be selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbUser_ValueChanged(object sender, System.EventArgs e)
        {
            int userID = int.Parse(cmbUser.Value.ToString());
            if (userID != _userIDSelected)
            {
                if (userID > 0)
                {
                    _userIDSelected = int.Parse(cmbUser.Value.ToString());

                    ValueEventArgs valueEventArgs = new ValueEventArgs();
                    valueEventArgs.companyUserID = this.cmbUser.Value.ToString();

                    if (UserChanged != null)
                    {
                        UserChanged(this, valueEventArgs);
                    }

                    bool IsNew = true;
                    Infragistics.Win.UltraWinGrid.RowsCollection _rwclc = grdUserOverall.Rows;
                    for (int i = 0; i < _rwclc.Count; i++)
                    {
                        if (_userIDSelected == Convert.ToInt32(_rwclc[i].Cells["CompanyUserID"].Value.ToString()))
                        {
                            (grdUserOverall.Rows)[i].Selected = true;
                            (grdUserOverall.Rows)[i].Activate();

                            SetForm();

                            IsNew = false;
                            break;

                        }

                        grdUserOverall.Rows[i].Selected = false;

                    }
                    if (IsNew)
                    {

                        RefreshUserLevelOverallLimitDetail();

                    }
                }

            }

        }


        #endregion Passing Value of selected UserID to Tree

        #region SetForm

        private void SetForm()
        {

            txtRMUserOverallID.Text = grdUserOverall.Selected.Rows[0].Cells["RMCompanyUserID"].Text;
            //this.cmbUser.ValueChanged -= new System.EventHandler(this.cmbUser_ValueChanged);
            //cmbUser.Text = grdUserOverall.Selected.Rows[0].Cells["ShortName"].Text;
            ////this.cmbUser.ValueChanged += new System.EventHandler(this.cmbUser_ValueChanged);
            txtExposureLimitUserLevel.Text = grdUserOverall.Selected.Rows[0].Cells["UserExposureLimit"].Text;
            txtMaxPNLLoss.Text = grdUserOverall.Selected.Rows[0].Cells["MaximumPNLLoss"].Text;
            txtMaxSizeperOrder.Text = grdUserOverall.Selected.Rows[0].Cells["MaximumSizePerOrder"].Text;
            txtMaxSizeperBasket.Text = grdUserOverall.Selected.Rows[0].Cells["MaximumSizePerBasket"].Text;
        }
        #endregion SetForm

        #region Save Method

        /// <summary>
        /// This method saves the UserLevelOverallLimit detail in the database.
        /// </summary>
        /// <param name="UserLevelOverallLimit"></param>
        /// <returns>Returns 1 if saved successfully.</returns>
        public int SaveUserLevelOverallLimit(UserLevelOverallLimit userLevelOverallLimit, int _companyID)
        {

            int result = int.MinValue;
            bool IsValidated = ValidateControl();

            if (IsValidated)
            {
                errorProvider1.SetError(cmbUser, "");

                UserLevelOverallLimit userOverallLimit = new UserLevelOverallLimit();
                userOverallLimit.RMCompanyUserID = Convert.ToInt32(this.txtRMUserOverallID.Text);
                userOverallLimit.CompanyUserID = int.Parse(cmbUser.Value.ToString());
                userOverallLimit.UserExposureLimit = int.Parse(txtExposureLimitUserLevel.Text.Trim().ToString());
                if (txtMaxPNLLoss.Text != "")
                {
                    userOverallLimit.MaximumPNLLoss = int.Parse(txtMaxPNLLoss.Text.Trim().ToString());

                }
                userOverallLimit.MaximumSizePerOrder = int.Parse(txtMaxSizeperOrder.Text.Trim().ToString());
                userOverallLimit.MaximumSizePerBasket = int.Parse(txtMaxSizeperBasket.Text.Trim().ToString());

                //Saving the data and retrieving the RmUserOverallId for the newly added UserDetails
                int RMUserOverallID = Prana.Admin.BLL.RMAdminBusinessLogic.SaveUserLevelOverallLimit(userOverallLimit, _companyID);
                //Showing the messages : data already exists by checking the RMUserID value to -1
                if (RMUserOverallID == -1)
                {
                    //						errorProvider1.SetError(cmbUser, "User Details for RM Overall already exists !");
                    // this means the user details have been updated.
                }
                else //data is saved
                {


                }
                result = userOverallLimit.CompanyUserID;
                BindUserOverallGrid();
            }
            return result;

        }

        #endregion Save Method		

        #region TextChange Validation

        private void txtExposureLimitUserLevel_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(cmbUser, "");
            errorProvider1.SetError(txtExposureLimitUserLevel, "");
            if (txtExposureLimitUserLevel.Text != "")
            {

                if (cmbUser.Text == C_COMBO_SELECT)
                {
                    txtExposureLimitUserLevel.Text = "";
                    errorProvider1.SetError(cmbUser, "Please select a user before entering the Exposure Limit !");
                    cmbUser.Focus();
                }
                else
                {
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtExposureLimitUserLevel.Text.Trim());
                    if (!IsValid)
                    {
                        //txtMaxPNLLossBaseCurrency.Text = "";
                        errorProvider1.SetError(txtExposureLimitUserLevel, "Please enter only numeric values for Exposure Limit!");
                        txtExposureLimitUserLevel.Focus();
                        return;
                    }
                    else
                    {
                        Int64 chkvalue = 0;
                        Int64.TryParse(txtExposureLimitUserLevel.Text, out chkvalue);
                        if (txtExposureLimitUserLevel.Text != "0" && chkvalue == 0)
                        {
                            txtExposureLimitUserLevel.Text = "";
                            errorProvider1.SetError(txtExposureLimitUserLevel, "You cannot enter a value greater than 9223372036854775807!");
                            txtExposureLimitUserLevel.Focus();
                        }
                        else
                        {
                            Int64 maxPermittedExpLt = RMAdminBusinessLogic.ValidRMAUECExpLt(_companyID, chkvalue);
                            if (maxPermittedExpLt > 0)
                            {
                                txtExposureLimitUserLevel.Text = "";
                                errorProvider1.SetError(txtExposureLimitUserLevel, "You cannot enter a value greater than Company Exposure Limit" + maxPermittedExpLt + "!");
                                txtExposureLimitUserLevel.Focus();
                            }

                        }
                    }
                }
            }
        }

        private void txtMaxPNLLoss_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(cmbUser, "");
            errorProvider1.SetError(txtMaxPNLLoss, "");
            if (txtMaxPNLLoss.Text != "")
            {

                if (cmbUser.Text == C_COMBO_SELECT)
                {
                    txtMaxPNLLoss.Text = "";
                    errorProvider1.SetError(cmbUser, "Please select a User before entering the Maximum PNL Loss !");
                    cmbUser.Focus();
                }
                else
                {
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtMaxPNLLoss.Text.Trim());
                    if (!IsValid)
                    {
                        //txtMaxPNLLossBaseCurrency.Text = "";
                        errorProvider1.SetError(txtMaxPNLLoss, "Please enter only numeric values for Maximum PNL Loss!");
                        txtMaxPNLLoss.Focus();
                        return;
                    }
                    else
                    {
                        Int64 chkvalue = 0;
                        Int64.TryParse(txtMaxPNLLoss.Text, out chkvalue);
                        if (txtMaxPNLLoss.Text != "0" && chkvalue == 0)
                        {
                            txtMaxPNLLoss.Text = "";
                            errorProvider1.SetError(txtMaxPNLLoss, "You cannot enter a value greater than 9223372036854775807!");
                            txtMaxPNLLoss.Focus();
                        }
                    }
                }
            }
        }

        private void txtMaxSizeperOrder_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(cmbUser, "");
            errorProvider1.SetError(txtMaxSizeperOrder, "");
            if (txtMaxSizeperOrder.Text != "")
            {

                if (cmbUser.Text == C_COMBO_SELECT)
                {
                    txtMaxSizeperOrder.Text = "";
                    errorProvider1.SetError(cmbUser, "Please select a User before entering the Maximum Size per Order !");
                    cmbUser.Focus();
                }
                else
                {
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtMaxSizeperOrder.Text.Trim());
                    if (!IsValid)
                    {
                        //txtMaxPNLLossBaseCurrency.Text = "";
                        errorProvider1.SetError(txtMaxSizeperOrder, "Please enter only numeric values for Maximum Size per Order!");
                        txtMaxSizeperOrder.Focus();
                        return;
                    }
                    else
                    {
                        Int64 chkvalue = 0;
                        Int64.TryParse(txtMaxSizeperOrder.Text, out chkvalue);
                        if (txtMaxSizeperOrder.Text != "0" && chkvalue == 0)
                        {
                            txtMaxSizeperOrder.Text = "";
                            errorProvider1.SetError(txtMaxSizeperOrder, "You cannot enter a value greater than 9223372036854775807!");
                            txtMaxSizeperOrder.Focus();
                        }
                    }
                }
            }
        }

        private void txtMaxSizeperBasket_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(cmbUser, "");
            errorProvider1.SetError(txtMaxSizeperBasket, "");
            if (txtMaxSizeperBasket.Text != "")
            {

                if (cmbUser.Text == C_COMBO_SELECT)
                {
                    txtMaxSizeperBasket.Text = "";
                    errorProvider1.SetError(cmbUser, "Please select a User before entering the Maximum Size per Basket !");
                    cmbUser.Focus();
                }
                else
                {
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtMaxSizeperBasket.Text.Trim());
                    if (!IsValid)
                    {
                        //txtMaxPNLLossBaseCurrency.Text = "";
                        errorProvider1.SetError(txtMaxSizeperBasket, "Please enter only numeric values for Maximum Size per Basket!");
                        txtMaxSizeperBasket.Focus();
                        return;
                    }
                    else
                    {
                        Int64 chkvalue = 0;
                        Int64.TryParse(txtMaxSizeperBasket.Text, out chkvalue);
                        if (txtMaxSizeperBasket.Text != "0" && chkvalue == 0)
                        {
                            txtMaxSizeperBasket.Text = "";
                            errorProvider1.SetError(txtMaxSizeperBasket, "You cannot enter a value greater than 9223372036854775807!");
                            txtMaxSizeperBasket.Focus();
                        }
                    }
                }
            }
        }

        #endregion TextChange Validation

        #region After Row Activate

        /// <summary>
        /// grid row activate event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdUserOverall_AfterRowActivate(object sender, System.EventArgs e)
        {
            if (this.grdUserOverall.Selected.Rows.Count > 0)
            {
                int userID = Convert.ToInt16(grdUserOverall.ActiveRow.Cells["CompanyUserID"].Value.ToString());
                if (userID != _userIDSelected)
                {
                    _userIDSelected = userID;
                    cmbUser.Value = _userIDSelected;
                    SetForm();

                    ValueEventArgs valueEventArgs = new ValueEventArgs();
                    valueEventArgs.companyUserID = this.cmbUser.Value.ToString();

                    if (UserChanged != null)
                    {
                        UserChanged(this, valueEventArgs);
                    }

                }
            }
        }

        #endregion After Row Activate

        #region Passing value to the text for currency

        /// <summary>
        /// This method is used to Update the currency as passed on from source usercontrol.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateCurrencyText(System.Object sender, PassValueEventArgs e)
        {

            lblCurr.Text = e.rMCurrencySymbol;
            lblCurr1.Text = e.rMCurrencySymbol;
            lblCurr2.Text = e.rMCurrencySymbol;
            lblCurr3.Text = e.rMCurrencySymbol;

        }

        #endregion

        #region Focus property

        private void cmbUser_Enter(object sender, EventArgs e)
        {
            cmbUser.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void cmbUser_Leave(object sender, EventArgs e)
        {
            cmbUser.Appearance.BackColor = Color.White;
        }

        private void txtExposureLimitUserLevel_Enter(object sender, EventArgs e)
        {
            txtExposureLimitUserLevel.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtExposureLimitUserLevel_Leave(object sender, EventArgs e)
        {
            txtExposureLimitUserLevel.BackColor = Color.White;
        }

        private void txtMaxPNLLoss_Enter(object sender, EventArgs e)
        {
            txtMaxPNLLoss.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtMaxPNLLoss_Leave(object sender, EventArgs e)
        {
            txtMaxPNLLoss.BackColor = Color.White;
        }

        private void txtMaxSizeperOrder_Enter(object sender, EventArgs e)
        {
            txtMaxSizeperOrder.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtMaxSizeperOrder_Leave(object sender, EventArgs e)
        {
            txtMaxSizeperOrder.BackColor = Color.White;
        }

        private void txtMaxSizeperBasket_Enter(object sender, EventArgs e)
        {
            txtMaxSizeperBasket.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtMaxSizeperBasket_Leave(object sender, EventArgs e)
        {
            txtMaxSizeperBasket.BackColor = Color.White;
        }

        #endregion Focus property

        #region DataEntry Check
        /// <summary>
        /// The method is made to check whether user has entered some data in the controls.
        /// </summary>
        /// <returns></returns>
        public bool CheckforInputData()
        {
            bool IsDataInput = false;

            if (int.Parse(cmbUser.Value.ToString()) != int.MinValue)
            {
                IsDataInput = true;
            }
            else if (txtExposureLimitUserLevel.Text != "")
            {
                IsDataInput = true;
            }
            else if (txtMaxPNLLoss.Text != "")
            {
                IsDataInput = true;
            }
            else if (txtMaxSizeperBasket.Text != "")
            {
                IsDataInput = true;
            }
            else if (txtMaxSizeperOrder.Text != "")
            {
                IsDataInput = true;
            }
            return IsDataInput;
        }

        #endregion DataEntry Check
    }

    #region class ValueEventArgs

    /// <summary>
    /// This Class is used for the event to pass the UserId of the selected user in combo to other controls
    /// </summary>
    public class ValueEventArgs : System.EventArgs
    {
        private String str;

        public String companyUserID
        {
            get
            {
                return (str);
            }
            set
            {
                str = value;
            }
        }  // userShortName

    }  // ValueEventArgs
    #endregion class ValueEventArgs


}
