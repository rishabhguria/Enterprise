#region Using
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace Prana.Admin.RiskManagement.Controls
{
    public delegate void RMUserAUECChangedHandler(System.Object sender, UserAUECValueEventArgs e);
    /// <summary>
    /// Summary description for UserLevel_UIControls.
    /// </summary>
    public class uctUserLevelUIControls : System.Windows.Forms.UserControl
    {

        #region Wizard Stuff
        const string C_COMBO_SELECT = "- Select -";
        private const string FORM_NAME = "USERLEVELUI : ";
        private int _userAUECID = int.MinValue;
        private int _companyID = int.MinValue;
        private int _companyUserID = int.MinValue;
        private int _auecIDSelected = int.MinValue;

        #region private and protected members

        //private  AUECs _usersAUECS;

        private System.Windows.Forms.Label lblTicketSize;
        private System.Windows.Forms.Label lbldeviationfromcurrentprice;
        private System.Windows.Forms.Label lblAllowusertooverwrite;
        private System.Windows.Forms.Label lblNotifyUser;
        private System.Windows.Forms.Label lblplusminus;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.CheckedListBox checkedLstAUEC;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdUserUI;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAUEC;
        private System.Windows.Forms.GroupBox grpBxUserUI;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkNotifyUser;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkAllowUser;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDeviation;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtTicketSize;
        private Label lblAUEC;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtRMUserUIID;
        private Infragistics.Win.Misc.UltraLabel lblCurr;
        private Infragistics.Win.Misc.UltraLabel lblCurr1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel lblRMCurr;
        private IContainer components;


        #endregion
        public event RMUserAUECChangedHandler RMUserAUECChanged;
        public int CompanyID
        {
            set { _companyID = value; }
        }

        public int UserAUECID
        {
            set { _userAUECID = value; }
        }

        public int CompanyUserID
        {
            set
            {
                _companyUserID = value;
                //if(_companyUserID == int.MinValue)
                //{
                //    lblUser.Text = "";
                //}

            }

        }

        #endregion Wizard Stuff

        public uctUserLevelUIControls()
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
                if (lblTicketSize != null)
                {
                    lblTicketSize.Dispose();
                }
                if (lbldeviationfromcurrentprice != null)
                {
                    lbldeviationfromcurrentprice.Dispose();
                }
                if (lblAllowusertooverwrite != null)
                {
                    lblAllowusertooverwrite.Dispose();
                }
                if (lblNotifyUser != null)
                {
                    lblNotifyUser.Dispose();
                }
                if (lblplusminus != null)
                {
                    lblplusminus.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (checkedLstAUEC != null)
                {
                    checkedLstAUEC.Dispose();
                }
                if (grdUserUI != null)
                {
                    grdUserUI.Dispose();
                }
                if (cmbAUEC != null)
                {
                    cmbAUEC.Dispose();
                }
                if (grpBxUserUI != null)
                {
                    grpBxUserUI.Dispose();
                }
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (chkNotifyUser != null)
                {
                    chkNotifyUser.Dispose();
                }
                if (chkAllowUser != null)
                {
                    chkAllowUser.Dispose();
                }
                if (txtDeviation != null)
                {
                    txtDeviation.Dispose();
                }
                if (txtTicketSize != null)
                {
                    txtTicketSize.Dispose();
                }
                if (lblAUEC != null)
                {
                    lblAUEC.Dispose();
                }
                if (txtRMUserUIID != null)
                {
                    txtRMUserUIID.Dispose();
                }
                if (lblCurr != null)
                {
                    lblCurr.Dispose();
                }
                if (lblCurr1 != null)
                {
                    lblCurr1.Dispose();
                }
                if (ultraLabel1 != null)
                {
                    ultraLabel1.Dispose();
                }
                if (lblRMCurr != null)
                {
                    lblRMCurr.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RMCompanyUserUIID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyUserID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyUserAUECID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AUEC", 4);
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TicketSize", 5);
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceDeviation", 6, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AllowUsertoOverwrite", 7);
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NotifyUserWhenLiveFeedsAreDown", 8);
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AllowUsertoOverWriteTrueFalse", 9);
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NotifyUserTrueFalse", 10);
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 11);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 1);
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.lblTicketSize = new System.Windows.Forms.Label();
            this.lbldeviationfromcurrentprice = new System.Windows.Forms.Label();
            this.lblAllowusertooverwrite = new System.Windows.Forms.Label();
            this.lblNotifyUser = new System.Windows.Forms.Label();
            this.lblplusminus = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.checkedLstAUEC = new System.Windows.Forms.CheckedListBox();
            this.grdUserUI = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cmbAUEC = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.grpBxUserUI = new System.Windows.Forms.GroupBox();
            this.lblCurr = new Infragistics.Win.Misc.UltraLabel();
            this.lblCurr1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblRMCurr = new Infragistics.Win.Misc.UltraLabel();
            this.chkNotifyUser = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkAllowUser = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtDeviation = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtTicketSize = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblAUEC = new System.Windows.Forms.Label();
            this.txtRMUserUIID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdUserUI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAUEC)).BeginInit();
            this.grpBxUserUI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeviation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTicketSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRMUserUIID)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTicketSize
            // 
            this.lblTicketSize.AutoSize = true;
            this.lblTicketSize.Location = new System.Drawing.Point(23, 40);
            this.lblTicketSize.Name = "lblTicketSize";
            this.lblTicketSize.Size = new System.Drawing.Size(60, 13);
            this.lblTicketSize.TabIndex = 1;
            this.lblTicketSize.Text = "Ticket Size ";
            // 
            // lbldeviationfromcurrentprice
            // 
            this.lbldeviationfromcurrentprice.AutoSize = true;
            this.lbldeviationfromcurrentprice.Location = new System.Drawing.Point(23, 61);
            this.lbldeviationfromcurrentprice.Name = "lbldeviationfromcurrentprice";
            this.lbldeviationfromcurrentprice.Size = new System.Drawing.Size(156, 13);
            this.lbldeviationfromcurrentprice.TabIndex = 2;
            this.lbldeviationfromcurrentprice.Text = "% deviation from Current Price";
            // 
            // lblAllowusertooverwrite
            // 
            this.lblAllowusertooverwrite.AutoSize = true;
            this.lblAllowusertooverwrite.Location = new System.Drawing.Point(23, 80);
            this.lblAllowusertooverwrite.Name = "lblAllowusertooverwrite";
            this.lblAllowusertooverwrite.Size = new System.Drawing.Size(118, 13);
            this.lblAllowusertooverwrite.TabIndex = 3;
            this.lblAllowusertooverwrite.Text = "Allow user to overwrite";
            // 
            // lblNotifyUser
            // 
            this.lblNotifyUser.AutoSize = true;
            this.lblNotifyUser.Location = new System.Drawing.Point(23, 99);
            this.lblNotifyUser.Name = "lblNotifyUser";
            this.lblNotifyUser.Size = new System.Drawing.Size(187, 13);
            this.lblNotifyUser.TabIndex = 4;
            this.lblNotifyUser.Text = "Notify User when live feeds are down";
            // 
            // lblplusminus
            // 
            this.lblplusminus.AutoSize = true;
            this.lblplusminus.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Pixel);
            this.lblplusminus.Location = new System.Drawing.Point(199, 61);
            this.lblplusminus.Name = "lblplusminus";
            this.lblplusminus.Size = new System.Drawing.Size(15, 13);
            this.lblplusminus.TabIndex = 11;
            this.lblplusminus.Text = "+";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // checkedLstAUEC
            // 
            this.checkedLstAUEC.CheckOnClick = true;
            this.checkedLstAUEC.HorizontalScrollbar = true;
            this.checkedLstAUEC.Location = new System.Drawing.Point(1048, 434);
            this.checkedLstAUEC.Name = "checkedLstAUEC";
            this.checkedLstAUEC.ScrollAlwaysVisible = true;
            this.checkedLstAUEC.Size = new System.Drawing.Size(272, 100);
            this.checkedLstAUEC.TabIndex = 12;
            // 
            // grdUserUI
            // 
            this.grdUserUI.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn3.Width = 195;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 655;
            ultraGridColumn5.Header.VisiblePosition = 2;
            ultraGridColumn5.Hidden = true;
            ultraGridColumn5.Width = 74;
            ultraGridColumn6.Header.VisiblePosition = 3;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn6.Width = 99;
            appearance16.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn7.CellAppearance = appearance16;
            appearance17.FontData.BoldAsString = "True";
            ultraGridColumn7.Header.Appearance = appearance17;
            ultraGridColumn7.Header.VisiblePosition = 4;
            ultraGridColumn7.Width = 183;
            appearance18.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn8.CellAppearance = appearance18;
            appearance19.FontData.BoldAsString = "True";
            ultraGridColumn8.Header.Appearance = appearance19;
            ultraGridColumn8.Header.Caption = "Ticket Size";
            ultraGridColumn8.Header.VisiblePosition = 5;
            ultraGridColumn8.Width = 115;
            appearance20.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn9.CellAppearance = appearance20;
            appearance21.FontData.BoldAsString = "True";
            ultraGridColumn9.Header.Appearance = appearance21;
            ultraGridColumn9.Header.Caption = "Price Deviation";
            ultraGridColumn9.Header.VisiblePosition = 6;
            ultraGridColumn9.Width = 103;
            appearance22.FontData.BoldAsString = "True";
            ultraGridColumn10.Header.Appearance = appearance22;
            ultraGridColumn10.Header.Caption = "";
            ultraGridColumn10.Header.VisiblePosition = 7;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn10.Width = 76;
            appearance23.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn11.CellAppearance = appearance23;
            appearance24.FontData.BoldAsString = "True";
            ultraGridColumn11.Header.Appearance = appearance24;
            ultraGridColumn11.Header.Caption = "";
            ultraGridColumn11.Header.VisiblePosition = 8;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn11.Width = 73;
            appearance25.FontData.BoldAsString = "True";
            ultraGridColumn12.Header.Appearance = appearance25;
            ultraGridColumn12.Header.Caption = "Allow User to Overwrite";
            ultraGridColumn12.Header.VisiblePosition = 9;
            ultraGridColumn12.Width = 115;
            appearance26.FontData.BoldAsString = "True";
            ultraGridColumn13.Header.Appearance = appearance26;
            ultraGridColumn13.Header.Caption = "Notify When Live Feeds Down";
            ultraGridColumn13.Header.VisiblePosition = 10;
            ultraGridColumn13.Width = 125;
            ultraGridColumn14.Header.VisiblePosition = 11;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn14.Width = 86;
            ultraGridBand2.Columns.AddRange(new object[] {
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
            ultraGridColumn14});
            ultraGridBand2.Header.Enabled = false;
            ultraGridBand2.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            ultraGridBand2.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridBand2.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            this.grdUserUI.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdUserUI.DisplayLayout.GroupByBox.Hidden = true;
            this.grdUserUI.DisplayLayout.MaxColScrollRegions = 1;
            this.grdUserUI.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdUserUI.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdUserUI.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdUserUI.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdUserUI.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdUserUI.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdUserUI.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdUserUI.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdUserUI.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdUserUI.Location = new System.Drawing.Point(3, 4);
            this.grdUserUI.Name = "grdUserUI";
            this.grdUserUI.Size = new System.Drawing.Size(662, 114);
            this.grdUserUI.TabIndex = 42;
            this.grdUserUI.AfterRowActivate += new System.EventHandler(this.grdUserUI_AfterRowActivate);
            // 
            // cmbAUEC
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.cmbAUEC.ButtonAppearance = appearance3;
            this.cmbAUEC.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance4.BackColor = System.Drawing.SystemColors.Window;
            appearance4.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAUEC.DisplayLayout.Appearance = appearance4;
            this.cmbAUEC.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            ultraGridBand1.Hidden = true;
            this.cmbAUEC.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbAUEC.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAUEC.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance5.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance5.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance5.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.GroupByBox.Appearance = appearance5;
            appearance6.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAUEC.DisplayLayout.GroupByBox.BandLabelAppearance = appearance6;
            this.cmbAUEC.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance7.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance7.BackColor2 = System.Drawing.SystemColors.Control;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAUEC.DisplayLayout.GroupByBox.PromptAppearance = appearance7;
            this.cmbAUEC.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAUEC.DisplayLayout.MaxRowScrollRegions = 1;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            appearance8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAUEC.DisplayLayout.Override.ActiveCellAppearance = appearance8;
            appearance9.BackColor = System.Drawing.SystemColors.Highlight;
            appearance9.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAUEC.DisplayLayout.Override.ActiveRowAppearance = appearance9;
            this.cmbAUEC.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAUEC.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.Override.CardAreaAppearance = appearance10;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            appearance11.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAUEC.DisplayLayout.Override.CellAppearance = appearance11;
            this.cmbAUEC.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAUEC.DisplayLayout.Override.CellPadding = 0;
            appearance12.BackColor = System.Drawing.SystemColors.Control;
            appearance12.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance12.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance12.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAUEC.DisplayLayout.Override.GroupByRowAppearance = appearance12;
            appearance13.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbAUEC.DisplayLayout.Override.HeaderAppearance = appearance13;
            this.cmbAUEC.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAUEC.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.BorderColor = System.Drawing.Color.Silver;
            this.cmbAUEC.DisplayLayout.Override.RowAppearance = appearance14;
            this.cmbAUEC.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance15.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAUEC.DisplayLayout.Override.TemplateAddRowAppearance = appearance15;
            this.cmbAUEC.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Both;
            this.cmbAUEC.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAUEC.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAUEC.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAUEC.DisplayMember = "";
            this.cmbAUEC.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAUEC.DropDownWidth = 0;
            this.cmbAUEC.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbAUEC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbAUEC.Location = new System.Drawing.Point(216, 14);
            this.cmbAUEC.Name = "cmbAUEC";
            this.cmbAUEC.Size = new System.Drawing.Size(265, 21);
            this.cmbAUEC.TabIndex = 43;
            this.cmbAUEC.ValueMember = "";
            this.cmbAUEC.Leave += new System.EventHandler(this.cmbAUEC_Leave);
            this.cmbAUEC.ValueChanged += new System.EventHandler(this.cmbAUEC_ValueChanged);
            this.cmbAUEC.Enter += new System.EventHandler(this.cmbAUEC_Enter);
            // 
            // grpBxUserUI
            // 
            this.grpBxUserUI.Controls.Add(this.lblCurr);
            this.grpBxUserUI.Controls.Add(this.lblCurr1);
            this.grpBxUserUI.Controls.Add(this.ultraLabel1);
            this.grpBxUserUI.Controls.Add(this.lblRMCurr);
            this.grpBxUserUI.Controls.Add(this.chkNotifyUser);
            this.grpBxUserUI.Controls.Add(this.chkAllowUser);
            this.grpBxUserUI.Controls.Add(this.txtDeviation);
            this.grpBxUserUI.Controls.Add(this.txtTicketSize);
            this.grpBxUserUI.Controls.Add(this.label2);
            this.grpBxUserUI.Controls.Add(this.label1);
            this.grpBxUserUI.Controls.Add(this.label9);
            this.grpBxUserUI.Controls.Add(this.lblAUEC);
            this.grpBxUserUI.Controls.Add(this.lblNotifyUser);
            this.grpBxUserUI.Controls.Add(this.lblAllowusertooverwrite);
            this.grpBxUserUI.Controls.Add(this.lbldeviationfromcurrentprice);
            this.grpBxUserUI.Controls.Add(this.cmbAUEC);
            this.grpBxUserUI.Controls.Add(this.lblplusminus);
            this.grpBxUserUI.Controls.Add(this.lblTicketSize);
            this.grpBxUserUI.Location = new System.Drawing.Point(52, 128);
            this.grpBxUserUI.Name = "grpBxUserUI";
            this.grpBxUserUI.Size = new System.Drawing.Size(527, 120);
            this.grpBxUserUI.TabIndex = 44;
            this.grpBxUserUI.TabStop = false;
            // 
            // lblCurr
            // 
            this.lblCurr.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblCurr.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblCurr.Location = new System.Drawing.Point(453, 36);
            this.lblCurr.Name = "lblCurr";
            this.lblCurr.Size = new System.Drawing.Size(24, 17);
            this.lblCurr.TabIndex = 75;
            this.lblCurr.Text = "usd";
            // 
            // lblCurr1
            // 
            this.lblCurr1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblCurr1.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblCurr1.Location = new System.Drawing.Point(453, 57);
            this.lblCurr1.Name = "lblCurr1";
            this.lblCurr1.Size = new System.Drawing.Size(24, 17);
            this.lblCurr1.TabIndex = 74;
            this.lblCurr1.Text = "usd";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(244, 50);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(0, 0);
            this.ultraLabel1.TabIndex = 73;
            // 
            // lblRMCurr
            // 
            this.lblRMCurr.Location = new System.Drawing.Point(453, 36);
            this.lblRMCurr.Name = "lblRMCurr";
            this.lblRMCurr.Size = new System.Drawing.Size(0, 0);
            this.lblRMCurr.TabIndex = 72;
            // 
            // chkNotifyUser
            // 
            appearance1.BorderColor = System.Drawing.Color.Black;
            this.chkNotifyUser.Appearance = appearance1;
            this.chkNotifyUser.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkNotifyUser.Location = new System.Drawing.Point(216, 95);
            this.chkNotifyUser.Name = "chkNotifyUser";
            this.chkNotifyUser.Size = new System.Drawing.Size(18, 20);
            this.chkNotifyUser.TabIndex = 71;
            // 
            // chkAllowUser
            // 
            appearance2.BorderColor = System.Drawing.Color.Black;
            this.chkAllowUser.Appearance = appearance2;
            this.chkAllowUser.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkAllowUser.Location = new System.Drawing.Point(216, 78);
            this.chkAllowUser.Name = "chkAllowUser";
            this.chkAllowUser.Size = new System.Drawing.Size(18, 16);
            this.chkAllowUser.TabIndex = 70;
            // 
            // txtDeviation
            // 
            this.txtDeviation.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtDeviation.Location = new System.Drawing.Point(216, 57);
            this.txtDeviation.MaxLength = 19;
            this.txtDeviation.Name = "txtDeviation";
            this.txtDeviation.Size = new System.Drawing.Size(231, 20);
            this.txtDeviation.TabIndex = 69;
            this.txtDeviation.Enter += new System.EventHandler(this.txtDeviation_Enter);
            this.txtDeviation.TextChanged += new System.EventHandler(this.txtDeviation_TextChanged);
            this.txtDeviation.Leave += new System.EventHandler(this.txtDeviation_Leave);
            // 
            // txtTicketSize
            // 
            this.txtTicketSize.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtTicketSize.Location = new System.Drawing.Point(216, 36);
            this.txtTicketSize.MaxLength = 19;
            this.txtTicketSize.Name = "txtTicketSize";
            this.txtTicketSize.Size = new System.Drawing.Size(231, 20);
            this.txtTicketSize.TabIndex = 68;
            this.txtTicketSize.Enter += new System.EventHandler(this.txtTicketSize_Enter);
            this.txtTicketSize.TextChanged += new System.EventHandler(this.txtTicketSize_TextChanged);
            this.txtTicketSize.Leave += new System.EventHandler(this.txtTicketSize_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(204, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 66;
            this.label2.Text = "*";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(187, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 65;
            this.label1.Text = "*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(203, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 13);
            this.label9.TabIndex = 64;
            this.label9.Text = "*";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAUEC
            // 
            this.lblAUEC.AutoSize = true;
            this.lblAUEC.Location = new System.Drawing.Point(23, 18);
            this.lblAUEC.Name = "lblAUEC";
            this.lblAUEC.Size = new System.Drawing.Size(83, 13);
            this.lblAUEC.TabIndex = 0;
            this.lblAUEC.Text = "AUEC for User: ";
            // 
            // txtRMUserUIID
            // 
            this.txtRMUserUIID.Location = new System.Drawing.Point(585, 146);
            this.txtRMUserUIID.Name = "txtRMUserUIID";
            this.txtRMUserUIID.Size = new System.Drawing.Size(53, 22);
            this.txtRMUserUIID.TabIndex = 45;
            this.txtRMUserUIID.Text = "-1";
            this.txtRMUserUIID.Visible = false;
            // 
            // uctUserLevelUIControls
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.txtRMUserUIID);
            this.Controls.Add(this.grpBxUserUI);
            this.Controls.Add(this.grdUserUI);
            this.Controls.Add(this.checkedLstAUEC);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "uctUserLevelUIControls";
            this.Size = new System.Drawing.Size(669, 256);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdUserUI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAUEC)).EndInit();
            this.grpBxUserUI.ResumeLayout(false);
            this.grpBxUserUI.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeviation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTicketSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRMUserUIID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Refresh Method

        /// <summary>
        /// Blanks all the textboxes in the form. 
        /// </summary>
        public void RefreshUserUIControlDetail()
        {
            txtRMUserUIID.Text = "-1";
            txtTicketSize.Text = "";
            txtDeviation.Text = "";
            chkAllowUser.CheckState = CheckState.Unchecked;
            chkNotifyUser.CheckState = CheckState.Unchecked;
        }

        #endregion Refresh Method

        #region Validation Method

        public bool ValidateControl()
        {
            bool validationSuccess = true;

            errorProvider1.SetError(cmbAUEC, "");
            errorProvider1.SetError(txtTicketSize, "");
            errorProvider1.SetError(txtDeviation, "");

            if (int.Parse(cmbAUEC.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbAUEC, "Please select an AUEC !");
                validationSuccess = false;
                cmbAUEC.Focus();
                return validationSuccess;
            }
            if (!DataTypeValidation.ValidateNumeric(txtTicketSize.Text.Trim()))
            {
                errorProvider1.SetError(txtTicketSize, "Please enter numeric value for Ticket Size!");
                validationSuccess = false;
                txtTicketSize.Focus();
                return validationSuccess;
            }
            else if (!DataTypeValidation.ValidateNumeric(txtDeviation.Text.Trim()))
            {
                errorProvider1.SetError(txtDeviation, "Please enter numeric value for Deviation !");
                validationSuccess = false;
                txtDeviation.Focus();
                return validationSuccess;
            }
            return validationSuccess;

        }
        #endregion Validation Method

        #region SetMethod

        public UserUIControl SetUserUIControl
        {
            set { SettingUserUIControl(value); }
        }

        private void SettingUserUIControl(UserUIControl userUIControl)
        {
            BindAUEC(_companyUserID);
            BindGrid();
            if (_companyID != int.MinValue && userUIControl.RMCompanyUserUIID != int.MinValue)
            {
                txtRMUserUIID.Text = userUIControl.RMCompanyUserUIID.ToString();
                cmbAUEC.Value = int.Parse(userUIControl.CompanyUserAUECID.ToString());

                txtDeviation.Text = userUIControl.PriceDeviation.ToString();
                txtTicketSize.Text = userUIControl.TicketSize.ToString();
                chkAllowUser.Checked = ((userUIControl.AllowUsertoOverWriteTrueFalse.ToString()) == "Yes" ? true : false);
                chkNotifyUser.Checked = ((userUIControl.NotifyUserTrueFalse.ToString()) == "Yes" ? true : false);

            }
            else
            {
                if (_userAUECID != int.MinValue)
                {
                    cmbAUEC.Value = _userAUECID;
                    RefreshUserUIControlDetail();
                }
                else
                {
                    cmbAUEC.Text = C_COMBO_SELECT;
                    RefreshUserUIControlDetail();
                }

            }
        }

        #endregion SetMethod

        #region Save Method

        /// <summary>
        /// This method saves the CompanyOverallLimit detail in the database.
        /// </summary>
        /// <param name="companyOverallLimit"></param>
        /// <returns>Returns 1 if saved successfully.</returns>
        public int SaveUserUIControl(UserUIControl userUIControl, int _companyID)
        {
            int result = int.MinValue;
            bool IsValidated = ValidateControl();
            if (IsValidated)
            {
                userUIControl.RMCompanyUserUIID = Convert.ToInt32(this.txtRMUserUIID.Text);
                userUIControl.CompanyUserAUECID = int.Parse(this.cmbAUEC.Value.ToString());
                userUIControl.TicketSize = int.Parse(txtTicketSize.Text.Trim().ToString());
                userUIControl.PriceDeviation = int.Parse(txtDeviation.Text.Trim().ToString());
                userUIControl.AllowUsertoOverwrite = chkAllowUser.Checked.Equals(true) ? 1 : 0;
                userUIControl.NotifyUserWhenLiveFeedsAreDown = chkNotifyUser.Checked.Equals(true) ? 1 : 0;


                //Saving the data and retrieving the RmUserUIlId for the newly added UserUIDetails
                int RMUserUIID = RMAdminBusinessLogic.SaveUserUIControls(userUIControl, _companyID, _companyUserID);

                //Showing the messages : data already exists by checking the RMUserID value to -1
                if (RMUserUIID == -1)
                {
                    // this means that the details have been updated.
                }
                else
                {
                    //new data is saved
                }
                result = userUIControl.CompanyUserAUECID;
                BindGrid();
            }
            return result;

        }
        #endregion SaveMethod

        #region Bind AUEC 

        /// <summary>
        /// This is the method to BindGrid For User Auec Details
        /// </summary>
        private void BindGrid()
        {
            //			grdUserUI.DataSource = null;

            UserUIControls userUIControls = RMAdminBusinessLogic.GetUserUIControls(_companyID, _companyUserID);

            UserUIControls newUserUIControls = new UserUIControls();
            foreach (UserUIControl userUIControl in userUIControls)
            {
                int auecID = int.Parse(userUIControl.CompanyUserAUECID.ToString());
                AUEC auec = AUECManager.GetAUEC(auecID);

                //SK 20061009 Compliance is removed from AUEC
                //Currency currency = new Currency();
                //currency = AUECManager.GetCurrency(auec.Compliance.BaseCurrencyID);
                //

                string auecName = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.DisplayName.ToString() + "/" + auec.Currency.CurrencyName.ToString();
                //string auecName = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.Exchange.DisplayName.ToString() + "/" + currency.CurrencySymbol.ToString();

                //AllCommissionRule allCommissonRule = new AllCommissionRule();
                userUIControl.AUEC = auecName.ToString();
                newUserUIControls.Add(userUIControl);
            }

            grdUserUI.DataSource = userUIControls;
            if (userUIControls.Count <= 0)
            {
                userUIControls = new UserUIControls();
                userUIControls.Add(new Prana.Admin.BLL.UserUIControl());

                grdUserUI.DisplayLayout.Rows[0].Delete(false);

                RefreshUserUIControlDetail();
            }
            FixHeadersPostions();
        }

        /// <summary>
        /// The method to obtain fixed headers postions in grid.
        /// </summary>
        private void FixHeadersPostions()
        {
            grdUserUI.DisplayLayout.Bands[0].Columns["AUEC"].Header.VisiblePosition = 0;
            grdUserUI.DisplayLayout.Bands[0].Columns["TicketSize"].Header.VisiblePosition = 1;
            grdUserUI.DisplayLayout.Bands[0].Columns["PriceDeviation"].Header.VisiblePosition = 2;
            grdUserUI.DisplayLayout.Bands[0].Columns["AllowUsertoOverwrite"].Header.VisiblePosition = 3;
            grdUserUI.DisplayLayout.Bands[0].Columns["NotifyUserWhenLiveFeedsAreDown"].Header.VisiblePosition = 4;

        }

        /// <summary>
        /// This method binds the existing <see cref="AUEC"/> in the ComboBox control by assigning the 
        /// companyTypes object to its datasource property.
        /// </summary>

        private void BindAUEC(int companyUserID)
        {
            //int result = int.MinValue;
            if (companyUserID != int.MinValue)
            {
                AUECs auecs = AUECManager.GetUserAUEC(companyUserID);

                if (auecs.Count > 0)
                {
                    System.Data.DataTable dtauec = new System.Data.DataTable();
                    dtauec.Columns.Add("Data");
                    dtauec.Columns.Add("Value");
                    object[] row = new object[2];
                    row[0] = C_COMBO_SELECT;
                    row[1] = int.MinValue;
                    dtauec.Rows.Add(row);

                    foreach (AUEC auec in auecs)
                    {
                        //SK 20061009 Compliance is removed from AUEC
                        //Currency currency = new Currency();
                        //currency = AUECManager.GetCurrency(auec.Compliance.BaseCurrencyID);
                        //

                        string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.DisplayName.ToString() + "/" + auec.Currency.CurrencyName.ToString();
                        //string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.Exchange.DisplayName.ToString() + "/" + currency.CurrencySymbol.ToString();
                        int Value = auec.AUECID;

                        row[0] = Data;
                        row[1] = Value;
                        dtauec.Rows.Add(row);
                    }

                    this.cmbAUEC.ValueChanged -= new System.EventHandler(this.cmbAUEC_ValueChanged);
                    cmbAUEC.DataSource = null;
                    cmbAUEC.DataSource = dtauec;
                    cmbAUEC.DisplayMember = "Data";
                    cmbAUEC.ValueMember = "Value";
                    cmbAUEC.Text = C_COMBO_SELECT;
                    this.cmbAUEC.ValueChanged += new System.EventHandler(this.cmbAUEC_ValueChanged);

                    ColumnsCollection columns5 = cmbAUEC.DisplayLayout.Bands[0].Columns;
                    foreach (UltraGridColumn column in columns5)
                    {
                        if (column.Key != "Data")
                        {
                            column.Hidden = true;
                        }
                        else
                        {
                            cmbAUEC.DisplayLayout.Bands[0].ColHeadersVisible = false;
                        }
                    }

                    //result = 1;

                }
                else
                {
                    this.cmbAUEC.ValueChanged -= new System.EventHandler(this.cmbAUEC_ValueChanged);
                    cmbAUEC.DataSource = null;
                    this.cmbAUEC.ValueChanged += new System.EventHandler(this.cmbAUEC_ValueChanged);
                    cmbAUEC.Text = C_COMBO_SELECT;
                    RefreshUserUIControlDetail();
                }
            }

        }


        #endregion

        #region Focus Color

        private void cmbAUEC_Enter(object sender, EventArgs e)
        {
            cmbAUEC.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void cmbAUEC_Leave(object sender, EventArgs e)
        {
            cmbAUEC.Appearance.BackColor = Color.White;
        }

        private void txtTicketSize_Enter(object sender, EventArgs e)
        {
            txtTicketSize.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtTicketSize_Leave(object sender, EventArgs e)
        {
            txtTicketSize.BackColor = Color.White;
        }

        private void txtDeviation_Enter(object sender, EventArgs e)
        {
            txtDeviation.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtDeviation_Leave(object sender, EventArgs e)
        {
            txtDeviation.BackColor = Color.White;
        }




        #endregion

        #region Passing the Currency Symbol
        /// <summary>
        /// The method is used to display the selected Currency as passed on from the source userControl 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateCurrencyText(System.Object sender, PassValueEventArgs e)
        {
            lblCurr.Text = e.rMCurrencySymbol;
            lblCurr1.Text = e.rMCurrencySymbol;
        }
        #endregion Passing the Currency Symbol

        #region Combo Value changed 
        /// <summary>
        ///This event sets the corresponding row in grid to be selected for the selected AUEC in combo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAUEC_ValueChanged(object sender, System.EventArgs e)
        {
            int auecID = int.Parse(cmbAUEC.Value.ToString());
            if (auecID != _auecIDSelected)
            {
                if (auecID != int.MinValue)
                {
                    _auecIDSelected = int.Parse(cmbAUEC.Value.ToString());

                    UserAUECValueEventArgs userAUECValueEventArgs = new UserAUECValueEventArgs();
                    userAUECValueEventArgs.companyUserAUECID = this.cmbAUEC.Value.ToString();

                    if (RMUserAUECChanged != null)
                    {
                        RMUserAUECChanged(this, userAUECValueEventArgs);
                    }

                    bool IsNew = true;
                    Infragistics.Win.UltraWinGrid.RowsCollection _rwclc = grdUserUI.Rows;
                    for (int i = 0; i < _rwclc.Count; i++)
                    {
                        if (_auecIDSelected == Convert.ToInt32(_rwclc[i].Cells["CompanyUserAUECID"].Value.ToString()))
                        {
                            (grdUserUI.Rows)[i].Selected = true;
                            (grdUserUI.Rows)[i].Activate();

                            SetForm();

                            IsNew = false;
                            break;

                        }

                        grdUserUI.Rows[i].Selected = false;

                    }
                    if (IsNew)
                    {

                        RefreshUserUIControlDetail();

                    }
                }
                else
                {
                    RefreshUserUIControlDetail();
                    errorProvider1.SetError(txtDeviation, "");
                    errorProvider1.SetError(txtTicketSize, "");
                }
            }

        }



        #endregion Combo Value changed 

        #region SetForm
        private void SetForm()
        {
            txtRMUserUIID.Text = grdUserUI.Selected.Rows[0].Cells["RMCompanyUserUIID"].Text;
            txtTicketSize.Text = grdUserUI.Selected.Rows[0].Cells["TicketSize"].Text;
            txtDeviation.Text = grdUserUI.Selected.Rows[0].Cells["PriceDeviation"].Text;
            chkAllowUser.Checked = (grdUserUI.Selected.Rows[0].Cells["AllowUsertoOverWriteTrueFalse"].Text) == "Yes" ? true : false;
            chkNotifyUser.Checked = (grdUserUI.Selected.Rows[0].Cells["NotifyUserTrueFalse"].Text) == "Yes" ? true : false;
        }
        #endregion SetForm

        #region After Row Activate in Grid

        private void grdUserUI_AfterRowActivate(object sender, System.EventArgs e)
        {
            if (this.grdUserUI.Selected.Rows.Count > 0)
            {
                int auecID = Convert.ToInt16(grdUserUI.ActiveRow.Cells["CompanyUserAUECID"].Value.ToString());
                if (auecID != _auecIDSelected)
                {
                    _auecIDSelected = auecID;
                    cmbAUEC.Value = _auecIDSelected;
                    SetForm();

                    UserAUECValueEventArgs userAUECValueEventArgs = new UserAUECValueEventArgs();
                    userAUECValueEventArgs.companyUserAUECID = this.cmbAUEC.Value.ToString();

                    if (RMUserAUECChanged != null)
                    {
                        RMUserAUECChanged(this, userAUECValueEventArgs);
                    }

                }
            }

        }

        #endregion After Row Activate in Grid 

        #region TextChange Validation

        private void txtTicketSize_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(cmbAUEC, "");
            errorProvider1.SetError(txtTicketSize, "");
            if (txtTicketSize.Text != "")
            {

                if (cmbAUEC.Text == C_COMBO_SELECT)
                {
                    txtTicketSize.Text = "";
                    errorProvider1.SetError(cmbAUEC, "Please select an AUEC before entering the Ticket size !");
                    cmbAUEC.Focus();
                }
                else
                {
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtTicketSize.Text.Trim());
                    if (!IsValid)
                    {

                        errorProvider1.SetError(txtTicketSize, "Please enter only numeric values for Ticket Size!");
                        txtTicketSize.Focus();
                        return;
                    }
                    else
                    {
                        Int64 chkvalue = 0;
                        Int64.TryParse(txtTicketSize.Text, out chkvalue);
                        if (txtTicketSize.Text != "0" && chkvalue == 0)
                        {
                            txtTicketSize.Text = "";
                            errorProvider1.SetError(txtTicketSize, "You cannot enter a value greater than 9223372036854775807!");
                            txtTicketSize.Focus();
                        }
                    }
                }
            }
        }

        private void txtDeviation_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(cmbAUEC, "");
            errorProvider1.SetError(txtDeviation, "");
            if (txtDeviation.Text != "")
            {

                if (cmbAUEC.Text == C_COMBO_SELECT)
                {
                    txtDeviation.Text = "";
                    errorProvider1.SetError(cmbAUEC, "Please select an AUEC before entering the Deviation !");
                    cmbAUEC.Focus();
                }
                else
                {
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtDeviation.Text.Trim());
                    if (!IsValid)
                    {

                        errorProvider1.SetError(txtDeviation, "Please enter only numeric values for Deviation!");
                        txtDeviation.Focus();
                        return;
                    }
                    else
                    {
                        Int64 chkvalue = 0;
                        Int64.TryParse(txtDeviation.Text, out chkvalue);
                        if (txtDeviation.Text != "0" && chkvalue == 0)
                        {
                            txtDeviation.Text = "";
                            errorProvider1.SetError(txtDeviation, "You cannot enter a value greater than 9223372036854775807!");
                            txtDeviation.Focus();
                        }
                    }
                }
            }
        }

        #endregion TextChange Validation

        #region DataEntry Check
        /// <summary>
        /// The method is used to check whether user has entered some data in the controls .
        /// </summary>
        /// <returns></returns>
        public bool CheckforInputData()
        {
            bool IsDataEntered = false;


            if (txtDeviation.Text != "")
            {
                IsDataEntered = true;
            }
            else if (txtTicketSize.Text != "")
            {
                IsDataEntered = true;
            }
            else if (int.Parse(cmbAUEC.Value.ToString()) != int.MinValue)
            {
                IsDataEntered = true;
            }
            else if (chkAllowUser.CheckState == CheckState.Checked)
            {
                IsDataEntered = true;
            }
            else if (chkNotifyUser.CheckState == CheckState.Checked)
            {
                IsDataEntered = true;
            }
            return IsDataEntered;
        }

        #endregion DataEntry Check
    }

    #region Class UserValueEventArgs
    public class UserAUECValueEventArgs : System.EventArgs
    {
        private String str;
        public String companyUserAUECID
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
    #endregion Class UserValueEventArgs

}
