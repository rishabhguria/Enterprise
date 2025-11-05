#region Using
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
#endregion;

namespace Prana.Admin.RiskManagement.Controls
{
    public delegate void ClientChangedHandler(System.Object sender, ClientIDValueEventArgs e);
    /// <summary>
    /// Summary description for Client_OverallLimits.
    /// </summary>
    public class uctClientOverallLimits : System.Windows.Forms.UserControl
    {
        #region Wizard Stuff
        const string C_COMBO_SELECT = "- Select -";
        private const string FORM_NAME = "CLIENTOVERALL : ";

        #region private and protected members

        private int _companyID = int.MinValue;
        private int _companyClientID = int.MinValue;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.Label lblExposureLimitClient;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbClient;
        private System.Windows.Forms.ErrorProvider errorProvider1;

        private Infragistics.Win.UltraWinGrid.UltraGrid grdClientOverall;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtRMClientID;
        private System.Windows.Forms.Label uneditExpo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtExposureLimitClient;
        private IContainer components;
        //		private System.Windows.Forms.ErrorProvider errorProvider1;
        public event ClientChangedHandler ClientChanged;

        #endregion

        public int CompanyID
        {
            set { _companyID = value; }
        }
        public int CompanyClientID
        {
            set { _companyClientID = value; }
        }
        public uctClientOverallLimits()
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
                if (lblClient != null)
                {
                    lblClient.Dispose();
                }
                if (lblExposureLimitClient != null)
                {
                    lblExposureLimitClient.Dispose();
                }
                if (cmbClient != null)
                {
                    cmbClient.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (grdClientOverall != null)
                {
                    grdClientOverall.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (txtRMClientID != null)
                {
                    txtRMClientID.Dispose();
                }
                if (uneditExpo != null)
                {
                    uneditExpo.Dispose();
                }
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (txtExposureLimitClient != null)
                {
                    txtExposureLimitClient.Dispose();
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
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactCell", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactTitle", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn29 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactFirstName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn30 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactCell", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn31 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 4);
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn32 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactTelephone", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn33 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactTitle", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn34 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyType", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn35 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactEmail", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn36 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Telephone", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn37 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactTelephone", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn38 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MailingAddress1", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn39 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactFirstName", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn40 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MailingAddress2", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn41 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClientID", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn42 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClientName", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn43 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactLastName", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn44 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactEmail", 17);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn45 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 18);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn46 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactLastName", 19);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn47 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 20);
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn48 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyClientRMID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn49 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClientID", 1);
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn50 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClientName", 2, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn51 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClientExposureLimit", 3);
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn52 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 4);
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            this.lblClient = new System.Windows.Forms.Label();
            this.lblExposureLimitClient = new System.Windows.Forms.Label();
            this.cmbClient = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grdClientOverall = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtExposureLimitClient = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.uneditExpo = new System.Windows.Forms.Label();
            this.txtRMClientID = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdClientOverall)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExposureLimitClient)).BeginInit();
            this.SuspendLayout();
            // 
            // lblClient
            // 
            this.lblClient.AutoSize = true;
            this.lblClient.Location = new System.Drawing.Point(20, 30);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(34, 13);
            this.lblClient.TabIndex = 0;
            this.lblClient.Text = "Client";
            // 
            // lblExposureLimitClient
            // 
            this.lblExposureLimitClient.AutoSize = true;
            this.lblExposureLimitClient.Location = new System.Drawing.Point(20, 50);
            this.lblExposureLimitClient.Name = "lblExposureLimitClient";
            this.lblExposureLimitClient.Size = new System.Drawing.Size(76, 13);
            this.lblExposureLimitClient.TabIndex = 1;
            this.lblExposureLimitClient.Text = "Exposure limit ";
            // 
            // cmbClient
            // 
            appearance22.BackColor = System.Drawing.Color.White;
            this.cmbClient.Appearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbClient.ButtonAppearance = appearance23;
            this.cmbClient.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance24.BackColor = System.Drawing.SystemColors.Window;
            appearance24.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbClient.DisplayLayout.Appearance = appearance24;
            this.cmbClient.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridColumn27.Header.VisiblePosition = 0;
            ultraGridColumn27.Hidden = true;
            ultraGridColumn28.Header.VisiblePosition = 1;
            ultraGridColumn28.Hidden = true;
            ultraGridColumn29.Header.VisiblePosition = 2;
            ultraGridColumn29.Hidden = true;
            ultraGridColumn30.Header.VisiblePosition = 3;
            ultraGridColumn30.Hidden = true;
            appearance25.ForeColor = System.Drawing.Color.Black;
            ultraGridColumn31.CellAppearance = appearance25;
            ultraGridColumn31.Header.VisiblePosition = 4;
            ultraGridColumn31.Hidden = true;
            ultraGridColumn32.Header.VisiblePosition = 5;
            ultraGridColumn32.Hidden = true;
            ultraGridColumn33.Header.VisiblePosition = 6;
            ultraGridColumn33.Hidden = true;
            ultraGridColumn34.Header.VisiblePosition = 7;
            ultraGridColumn34.Hidden = true;
            ultraGridColumn35.Header.VisiblePosition = 8;
            ultraGridColumn35.Hidden = true;
            ultraGridColumn36.Header.VisiblePosition = 9;
            ultraGridColumn36.Hidden = true;
            ultraGridColumn37.Header.VisiblePosition = 10;
            ultraGridColumn37.Hidden = true;
            ultraGridColumn38.Header.VisiblePosition = 11;
            ultraGridColumn38.Hidden = true;
            ultraGridColumn39.Header.VisiblePosition = 12;
            ultraGridColumn39.Hidden = true;
            ultraGridColumn40.Header.VisiblePosition = 13;
            ultraGridColumn40.Hidden = true;
            ultraGridColumn41.Header.VisiblePosition = 14;
            ultraGridColumn41.Hidden = true;
            ultraGridColumn42.Header.VisiblePosition = 15;
            ultraGridColumn42.Hidden = true;
            ultraGridColumn43.Header.VisiblePosition = 16;
            ultraGridColumn43.Hidden = true;
            ultraGridColumn44.Header.VisiblePosition = 17;
            ultraGridColumn44.Hidden = true;
            ultraGridColumn45.Header.VisiblePosition = 18;
            ultraGridColumn45.Hidden = true;
            ultraGridColumn46.Header.VisiblePosition = 19;
            ultraGridColumn46.Hidden = true;
            ultraGridColumn47.Header.VisiblePosition = 20;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn27,
            ultraGridColumn28,
            ultraGridColumn29,
            ultraGridColumn30,
            ultraGridColumn31,
            ultraGridColumn32,
            ultraGridColumn33,
            ultraGridColumn34,
            ultraGridColumn35,
            ultraGridColumn36,
            ultraGridColumn37,
            ultraGridColumn38,
            ultraGridColumn39,
            ultraGridColumn40,
            ultraGridColumn41,
            ultraGridColumn42,
            ultraGridColumn43,
            ultraGridColumn44,
            ultraGridColumn45,
            ultraGridColumn46,
            ultraGridColumn47});
            ultraGridBand3.Hidden = true;
            this.cmbClient.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbClient.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClient.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClient.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClient.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.cmbClient.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClient.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.cmbClient.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbClient.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbClient.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbClient.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.cmbClient.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbClient.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.cmbClient.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbClient.DisplayLayout.Override.CellAppearance = appearance32;
            this.cmbClient.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbClient.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClient.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbClient.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.cmbClient.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbClient.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.cmbClient.DisplayLayout.Override.RowAppearance = appearance35;
            this.cmbClient.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbClient.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.cmbClient.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Both;
            this.cmbClient.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbClient.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbClient.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbClient.DisplayMember = "";
            this.cmbClient.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbClient.DropDownWidth = 0;
            this.cmbClient.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbClient.Location = new System.Drawing.Point(202, 28);
            this.cmbClient.Name = "cmbClient";
            this.cmbClient.Size = new System.Drawing.Size(123, 21);
            this.cmbClient.TabIndex = 4;
            this.cmbClient.ValueMember = "";
            this.cmbClient.Leave += new System.EventHandler(this.cmbClient_Leave);
            this.cmbClient.ValueChanged += new System.EventHandler(this.cmbClient_ValueChanged);
            this.cmbClient.Enter += new System.EventHandler(this.cmbClient_Enter);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grdClientOverall
            // 
            this.grdClientOverall.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grdClientOverall.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn48.Header.VisiblePosition = 0;
            ultraGridColumn48.Hidden = true;
            ultraGridColumn48.Width = 195;
            appearance37.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn49.CellAppearance = appearance37;
            appearance38.FontData.BoldAsString = "True";
            appearance38.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn49.Header.Appearance = appearance38;
            ultraGridColumn49.Header.VisiblePosition = 1;
            ultraGridColumn49.Hidden = true;
            ultraGridColumn49.Width = 183;
            appearance39.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn50.CellAppearance = appearance39;
            ultraGridColumn50.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
            ultraGridColumn50.CellMultiLine = Infragistics.Win.DefaultableBoolean.True;
            ultraGridColumn50.Format = "";
            appearance40.FontData.BoldAsString = "True";
            appearance40.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn50.Header.Appearance = appearance40;
            ultraGridColumn50.Header.Caption = "Client Name";
            ultraGridColumn50.Header.VisiblePosition = 2;
            ultraGridColumn50.Width = 188;
            appearance41.FontData.BoldAsString = "True";
            appearance41.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn51.Header.Appearance = appearance41;
            ultraGridColumn51.Header.Caption = "Client Exposure Limit";
            ultraGridColumn51.Header.VisiblePosition = 3;
            ultraGridColumn51.Width = 178;
            appearance42.FontData.BoldAsString = "True";
            appearance42.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn52.Header.Appearance = appearance42;
            ultraGridColumn52.Header.VisiblePosition = 4;
            ultraGridColumn52.Hidden = true;
            ultraGridColumn52.Width = 123;
            ultraGridBand4.Columns.AddRange(new object[] {
            ultraGridColumn48,
            ultraGridColumn49,
            ultraGridColumn50,
            ultraGridColumn51,
            ultraGridColumn52});
            ultraGridBand4.Header.Enabled = false;
            ultraGridBand4.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            ultraGridBand4.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdClientOverall.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.grdClientOverall.DisplayLayout.GroupByBox.Hidden = true;
            this.grdClientOverall.DisplayLayout.MaxColScrollRegions = 1;
            this.grdClientOverall.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdClientOverall.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdClientOverall.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdClientOverall.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdClientOverall.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdClientOverall.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdClientOverall.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdClientOverall.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdClientOverall.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdClientOverall.Location = new System.Drawing.Point(8, 10);
            this.grdClientOverall.Name = "grdClientOverall";
            this.grdClientOverall.Size = new System.Drawing.Size(387, 130);
            this.grdClientOverall.TabIndex = 41;
            this.grdClientOverall.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdClientOverall_AfterSelectChange);
            this.grdClientOverall.AfterRowActivate += new System.EventHandler(this.grdClientOverall_AfterRowActivate);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.txtExposureLimitClient);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.uneditExpo);
            this.groupBox1.Controls.Add(this.txtRMClientID);
            this.groupBox1.Controls.Add(this.lblClient);
            this.groupBox1.Controls.Add(this.cmbClient);
            this.groupBox1.Controls.Add(this.lblExposureLimitClient);
            this.groupBox1.Location = new System.Drawing.Point(8, 150);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(387, 78);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            // 
            // txtExposureLimitClient
            // 
            this.txtExposureLimitClient.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtExposureLimitClient.Location = new System.Drawing.Point(202, 51);
            this.txtExposureLimitClient.MaxLength = 19;
            this.txtExposureLimitClient.Name = "txtExposureLimitClient";
            this.txtExposureLimitClient.Size = new System.Drawing.Size(123, 20);
            this.txtExposureLimitClient.TabIndex = 67;
            this.txtExposureLimitClient.Enter += new System.EventHandler(this.txtExposureLimitClient_Enter);
            this.txtExposureLimitClient.TextChanged += new System.EventHandler(this.txtExposureLimitClient_TextChanged);
            this.txtExposureLimitClient.Leave += new System.EventHandler(this.txtExposureLimitClient_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(189, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 66;
            this.label1.Text = "*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(189, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 13);
            this.label9.TabIndex = 65;
            this.label9.Text = "*";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uneditExpo
            // 
            this.uneditExpo.AutoSize = true;
            this.uneditExpo.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.uneditExpo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uneditExpo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uneditExpo.Location = new System.Drawing.Point(331, 54);
            this.uneditExpo.Name = "uneditExpo";
            this.uneditExpo.Size = new System.Drawing.Size(37, 15);
            this.uneditExpo.TabIndex = 7;
            this.uneditExpo.Text = "ioiouo";
            // 
            // txtRMClientID
            // 
            this.txtRMClientID.Location = new System.Drawing.Point(331, 25);
            this.txtRMClientID.Name = "txtRMClientID";
            this.txtRMClientID.Size = new System.Drawing.Size(50, 21);
            this.txtRMClientID.TabIndex = 6;
            this.txtRMClientID.Text = "-1";
            this.txtRMClientID.Visible = false;
            // 
            // uctClientOverallLimits
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grdClientOverall);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "uctClientOverallLimits";
            this.Size = new System.Drawing.Size(406, 236);
            ((System.ComponentModel.ISupportInitialize)(this.cmbClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdClientOverall)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExposureLimitClient)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Validation Check

        /// <summary>
        /// this method is to check the validation of data entered.
        /// </summary>
        /// <returns></returns>
        private bool ValidationControl()
        {
            bool validationSuccess = true;

            if (cmbClient.Enabled == true)
            {

                errorProvider1.SetError(cmbClient, "");
                errorProvider1.SetError(txtExposureLimitClient, "");

                if (int.Parse(cmbClient.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbClient, "Please select the Client !");
                    validationSuccess = false;
                    cmbClient.Focus();
                    return validationSuccess;
                }
                else if (!DataTypeValidation.ValidateNumeric(txtExposureLimitClient.Text.Trim()))
                {
                    errorProvider1.SetError(txtExposureLimitClient, "Please enter the numeric value for Exposure Limit !");
                    validationSuccess = false;
                    txtExposureLimitClient.Focus();
                    return validationSuccess;
                }
                return validationSuccess;
            }
            else
            {
                validationSuccess = false;
                return validationSuccess;
            }

        }
        #endregion Validation Check

        #region Bind Clients and Bindgrid

        /// <summary>
        /// This method binds the existing <see cref="Client"/> in the ComboBox control by assigning the 
        /// client object to its datasource property.
        /// </summary>
        private void BindClient()
        {
            if (_companyID != int.MinValue)
            {
                //GetClients method fetches the existing currencies from the database.
                Prana.Admin.BLL.CompanyClients companyClients = RMAdminBusinessLogic.GetCompanyClients(_companyID);
                //CompanyManager.GetCompanyClientsRM(_companyID);
                //Inserting the - Select - option in the Combo Box at the top.
                companyClients.Insert(0, new CompanyClient(int.MinValue, C_COMBO_SELECT));
                if (companyClients.Count > 1)
                {
                    EnableTxtBxs();
                    this.cmbClient.ValueChanged -= new System.EventHandler(this.cmbClient_ValueChanged);
                    //					companyClients.Add(new CompanyClient(222, "ClientName"));
                    this.cmbClient.DataSource = null;
                    this.cmbClient.DataSource = companyClients;
                    this.cmbClient.DisplayMember = "Name";
                    this.cmbClient.ValueMember = "CompanyClientID";
                    this.cmbClient.Value = int.MinValue;
                    this.cmbClient.ValueChanged += new System.EventHandler(this.cmbClient_ValueChanged);

                    ColumnsCollection columns1 = cmbClient.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns1)
                    {
                        if (column.Key != "Name")
                        {
                            column.Hidden = true;
                        }
                    }
                }
                else
                {
                    MessageBox.Show(" There are no users for this company !");
                    RefreshClientDetail();
                    cmbClient.Text = C_COMBO_SELECT;
                    DisableTxtBxs();
                }
            }
        }

        private void BindClientGrid()
        {
            //Fetching the existing data from the database and binding it to the grid.
            ClientOverallLimits clientOverallLimits = RMAdminBusinessLogic.GetClientOverallLimits(_companyID);

            //Assigning the ClientOverallLimits  grid's datasource property to ClientOverallLimits object if it has some values.
            if (clientOverallLimits.Count != 0)
            {


                //assigning the grid's datasource to the ClientOverallLimits object.
                grdClientOverall.DataSource = clientOverallLimits;

            }
            else
            {
                clientOverallLimits = new ClientOverallLimits();
                clientOverallLimits.Add(new Prana.Admin.BLL.ClientOverallLimit());
                grdClientOverall.DataSource = clientOverallLimits;

                grdClientOverall.DisplayLayout.Rows[0].Delete(false);

                RefreshClientDetail();
            }
        }
        #endregion Bind Clients and Bindgrid

        #region Enable and Disable TxtBxs

        private void DisableTxtBxs()
        {
            cmbClient.Enabled = false;
            txtExposureLimitClient.Enabled = false;
        }

        private void EnableTxtBxs()
        {
            cmbClient.Enabled = true;
            txtExposureLimitClient.Enabled = true;

        }

        #endregion Enable and Disable TxtBxs

        #region Refresh Method
        /// <summary>
        /// Blanks all the textboxes in the form. 
        /// </summary>
        private void RefreshClientDetail()
        {
            txtExposureLimitClient.Text = "";
            txtRMClientID.Text = "-1";
        }

        #endregion Refresh Method

        #region Set Method


        public ClientOverallLimit SetClientOverallLimit
        {
            set { SettingClientOverallLimit(value); }
        }

        /// <summary>
        /// Shows all the details in the respective controls pertaining to that paricular company.
        /// </summary>
        /// <param name="Client"></param>
        private void SettingClientOverallLimit(ClientOverallLimit clientOverallLimit)
        {
            BindClient();
            BindClientGrid();
            if (clientOverallLimit != null && clientOverallLimit.ClientID != int.MinValue)
            {
                //				txtRMClientID.Text = clientOverallLimit.CompanyClientRMID.ToString();
                cmbClient.Value = int.Parse(clientOverallLimit.ClientID.ToString());
                txtExposureLimitClient.Text = clientOverallLimit.ClientExposureLimit.ToString();
            }
            else
            {
                if (_companyClientID != int.MinValue)
                {
                    cmbClient.Value = _companyClientID;
                    RefreshClientDetail();
                }
                else
                {
                    cmbClient.Value = int.MinValue;
                    RefreshClientDetail();
                }
            }
        }


        #endregion

        #region Save Method

        /// <summary>
        /// This method saves the ClientOverallLimit detail in the database.
        /// </summary>
        /// <param name="clientOverallLimit"></param>
        /// <returns>Returns 1 if saved successfully.</returns>
        public int SaveClientOverallLimit(ClientOverallLimit clientOverallLimit, int _companyID)
        {
            int result = int.MinValue;
            bool IsValidated = ValidationControl();
            if (IsValidated)
            {

                clientOverallLimit.CompanyClientRMID = Convert.ToInt32(this.txtRMClientID.Text);
                clientOverallLimit.ClientID = int.Parse(cmbClient.Value.ToString());
                clientOverallLimit.ClientExposureLimit = int.Parse(txtExposureLimitClient.Text.Trim().ToString());

                //Saving the data and retrieving the RmClientId for the newly added ClientDetails
                int RMClientID = Prana.Admin.BLL.RMAdminBusinessLogic.SaveClientOverallLimit(clientOverallLimit, _companyID);
                //Showing the messages : data already exists by checking the RMClientID value to -1
                if (RMClientID == -1)
                {
                    //					errorProvider1.SetError(cmbClient, "ClientDetails for RM Overall already exists !");
                    //details have been updated
                }
                else //data is saved
                {
                    //cmbClient.Tag = null;
                    //RefreshClientDetails();
                }
                result = clientOverallLimit.ClientID;
                BindClientGrid();
                //				cmbClient.Text = C_COMBO_SELECT;
                //				RefreshClientDetail();
            }
            return result;

        }

        #endregion Save Method		

        #region Focus Color

        private void cmbClient_Enter(object sender, EventArgs e)
        {
            cmbClient.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void cmbClient_Leave(object sender, EventArgs e)
        {
            cmbClient.Appearance.BackColor = Color.White;
        }

        private void txtExposureLimitClient_Leave(object sender, EventArgs e)
        {
            txtExposureLimitClient.BackColor = Color.White;
        }

        private void txtExposureLimitClient_Enter(object sender, EventArgs e)
        {
            txtExposureLimitClient.BackColor = Color.FromArgb(255, 250, 205);
        }
        #endregion

        #region Passing The Currency Symbol

        /// <summary>
        /// for displaying the selected RM Base currency symbol on Client Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateCurrencyText(System.Object sender, PassValueEventArgs e)
        {
            uneditExpo.Text = e.rMCurrencySymbol;
        }
        #endregion Passing The Currency Symbol

        #region Passing the value of the clientID to tree

        /// <summary>
        /// This event is used to Allow the selection of corresponding selected Client in Grid as well as Tree
        /// also , it displays the content of the selected client in textboxes.. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbClient_ValueChanged(object sender, System.EventArgs e)
        {
            if (this.cmbClient.Value.Equals(null))
            {
                DisableTxtBxs();
                return;
            }


            EnableTxtBxs();
            int clientId = int.Parse(cmbClient.Value.ToString());

            ClientIDValueEventArgs clientValueEventArgs = new ClientIDValueEventArgs();
            clientValueEventArgs.companyClientID = this.cmbClient.Value.ToString();

            if (ClientChanged != null)
            {
                ClientChanged(this, clientValueEventArgs);
            }

            bool IsNew = true;
            Infragistics.Win.UltraWinGrid.RowsCollection _rwclc = grdClientOverall.Rows;
            for (int i = 0; i < _rwclc.Count; i++)
            {
                if (clientId == Convert.ToInt32(_rwclc[i].Cells["ClientID"].Value.ToString()))
                {
                    (grdClientOverall.Rows)[i].Selected = true;
                    (grdClientOverall.Rows)[i].Activate();
                    SetValueInText();
                    IsNew = false;
                    break;

                }

                grdClientOverall.Rows[i].Selected = false;

            }
            if (IsNew)
            {
                RefreshClientDetail();
            }
        }

        #endregion  Passing the value of the clientID to tree

        #region Grid Row Activate nad selectrow method

        /// <summary>
        /// method to set the data of the selected row in the grid in the form 
        /// </summary>
        private void SetValueInText()
        {
            //			txtRMClientID.Text= "";
            //			txtRMClientID.Text = grdClientOverall.ActiveRow.Cells["CompanyClientRMID"].Text;
            this.cmbClient.ValueChanged -= new System.EventHandler(this.cmbClient_ValueChanged);
            cmbClient.Text = grdClientOverall.Selected.Rows[0].Cells["ClientName"].Text;
            this.cmbClient.ValueChanged += new System.EventHandler(this.cmbClient_ValueChanged);
            txtExposureLimitClient.Text = grdClientOverall.Selected.Rows[0].Cells["ClientExposureLimit"].Text;
            txtRMClientID.Text = grdClientOverall.Selected.Rows[0].Cells["CompanyClientRMID"].Text;
        }

        /// <summary>
        /// grid row activate event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdClientOverall_AfterRowActivate(object sender, System.EventArgs e)
        {
            if (this.grdClientOverall.Selected.Rows.Count > 0)
            {
                int ClientIdonRowSelect = Convert.ToInt32(this.grdClientOverall.Selected.Rows[0].Cells["ClientID"].Value.ToString());

                if (ClientIdonRowSelect != int.MinValue && _companyClientID == int.MinValue || ClientIdonRowSelect == int.MinValue && _companyClientID != int.MinValue
                    || ClientIdonRowSelect != int.MinValue && _companyClientID != int.MinValue)
                {


                    ClientIDValueEventArgs clientValueEventArgs = new ClientIDValueEventArgs();
                    this._companyClientID = ClientIdonRowSelect;
                    clientValueEventArgs.companyClientID = this._companyClientID.ToString();

                    if (ClientChanged != null)
                    {
                        ClientChanged(this, clientValueEventArgs);
                    }

                    SetValueInText();


                }
                else
                {

                    RefreshClientDetail();
                }
            }
        }

        /// <summary>
        /// select row event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdClientOverall_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            grdClientOverall_AfterRowActivate(sender, e);

        }

        #endregion Grid Row Activate and selectrow method

        #region Validation Check on Text_changed event

        /// <summary>
        /// The event is raised to check the validity of the entered data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExposureLimitClient_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtExposureLimitClient, "");
            errorProvider1.SetError(cmbClient, "");
            if (txtExposureLimitClient.Text != "")
            {
                if (int.Parse(cmbClient.Value.ToString()) == int.MinValue)
                {
                    errorProvider1.SetError(cmbClient, "Please select a client before entering Exposure Limit !");
                    txtExposureLimitClient.Text = "";
                    cmbClient.Focus();
                    return;
                }
                else
                {
                    if (!DataTypeValidation.ValidateNumeric(txtExposureLimitClient.Text.Trim()))
                    {
                        errorProvider1.SetError(txtExposureLimitClient, "Please enter only numeric data!");
                        txtExposureLimitClient.Focus();
                        return;
                    }
                    else if (!DataTypeValidation.ValidateMaxPermiitedNumeric(txtExposureLimitClient.Text.Trim()))
                    {
                        errorProvider1.SetError(txtExposureLimitClient, "You cannot enter a value greater than 9223372036854775807 !");
                        txtExposureLimitClient.Text = "";
                        txtExposureLimitClient.Focus();
                        return;

                    }
                    else
                    {
                        Int64 chkvalue = Int64.MinValue;
                        Int64.TryParse(txtExposureLimitClient.Text, out chkvalue);
                        Int64 maxPermittedExpLt = RMAdminBusinessLogic.ValidRMAUECExpLt(_companyID, chkvalue);
                        if (maxPermittedExpLt > 0)
                        {
                            txtExposureLimitClient.Text = "";
                            errorProvider1.SetError(txtExposureLimitClient, "You cannot enter a value greater than Company Exposure Limit i.e." + maxPermittedExpLt + "!");
                            txtExposureLimitClient.Focus();
                        }

                    }



                }
            }
        }

        #endregion Validation Check on Text_changed event

    }

    #region ClientIDValueEventArgs Class
    public class ClientIDValueEventArgs : System.EventArgs
    {
        private String str;
        public String companyClientID
        {
            get
            {
                return (str);
            }
            set
            {
                str = value;
            }
        }  // companyClientID
    }  // ValueEventArgs

    #endregion ClientIDValueEventArgs Class
}

