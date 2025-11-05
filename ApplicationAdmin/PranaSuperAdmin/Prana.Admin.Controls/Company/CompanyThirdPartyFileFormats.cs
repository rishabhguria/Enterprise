using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.ThirdPartyManager.DataAccess;
using System;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CompanyThirdPartyFileFormats.
    /// </summary>
    public class CompanyThirdPartyFileFormats : System.Windows.Forms.UserControl
    {
        private const string FORM_NAME = "CompanyThirdPartyFileFormats : ";

        const string C_COMBO_SELECT = "- Select -";
        const int MIN_VALUE = int.MinValue;

        const int C_TYPE_PRIMEBROKERCLEARER = 1;
        const int C_TYPE_VENDOR = 2;
        const int C_TYPE_CUSTODIAN = 3;
        const int C_TYPE_ADMINISTRATOR = 4;

        private System.Windows.Forms.GroupBox grpFileFormats;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdFileFormats;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownAccountName;
        private System.Windows.Forms.Button btnGetAccountID;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownPrimeBrokerClearer;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownCustodian;
        private Infragistics.Win.UltraWinGrid.UltraDropDown ultraDropDownAdministrator;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public CompanyThirdPartyFileFormats()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
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
                if (grpFileFormats != null)
                {
                    grpFileFormats.Dispose();
                }
                if (grdFileFormats != null)
                {
                    grdFileFormats.Dispose();
                }
                if (ultraDropDownAccountName != null)
                {
                    ultraDropDownAccountName.Dispose();
                }
                if (btnGetAccountID != null)
                {
                    btnGetAccountID.Dispose();
                }
                if (ultraDropDownPrimeBrokerClearer != null)
                {
                    ultraDropDownPrimeBrokerClearer.Dispose();
                }
                if (ultraDropDownCustodian != null)
                {
                    ultraDropDownCustodian.Dispose();
                }
                if (ultraDropDownAdministrator != null)
                {
                    ultraDropDownAdministrator.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountShortName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyPrimeBrokerClearerID", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyCustodianID", 5, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyAdministratorID", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyAccountID", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyUserAccountID", 8);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountShortName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ContactPerson", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeID", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address1", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address2", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CellPhone", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("WorkTelephone", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Email", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeName", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyThirdPartyID", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn29 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn30 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateID", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn31 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Zip", 17);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn32 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn33 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn34 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn35 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn36 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn37 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ContactPerson", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn38 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeID", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn39 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address1", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn40 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address2", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn41 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CellPhone", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn42 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("WorkTelephone", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn43 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn44 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Email", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn45 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeName", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn46 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyThirdPartyID", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn47 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CountryID", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn48 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StateID", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn49 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Zip", 17);
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
            this.grpFileFormats = new System.Windows.Forms.GroupBox();
            this.btnGetAccountID = new System.Windows.Forms.Button();
            this.grdFileFormats = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDropDownAccountName = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ultraDropDownPrimeBrokerClearer = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ultraDropDownCustodian = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ultraDropDownAdministrator = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.grpFileFormats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFileFormats)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownAccountName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownPrimeBrokerClearer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCustodian)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownAdministrator)).BeginInit();
            this.SuspendLayout();
            // 
            // grpFileFormats
            // 
            this.grpFileFormats.Controls.Add(this.btnGetAccountID);
            this.grpFileFormats.Controls.Add(this.grdFileFormats);
            this.grpFileFormats.Location = new System.Drawing.Point(0, 4);
            this.grpFileFormats.Name = "grpFileFormats";
            this.grpFileFormats.Size = new System.Drawing.Size(612, 216);
            this.grpFileFormats.TabIndex = 0;
            this.grpFileFormats.TabStop = false;
            this.grpFileFormats.Text = "File Format";
            // 
            // btnGetAccountID
            // 
            this.btnGetAccountID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnGetAccountID.Location = new System.Drawing.Point(314, 180);
            this.btnGetAccountID.Name = "btnGetAccountID";
            this.btnGetAccountID.Size = new System.Drawing.Size(90, 20);
            this.btnGetAccountID.TabIndex = 68;
            this.btnGetAccountID.Text = "Click";
            this.btnGetAccountID.UseVisualStyleBackColor = false;
            this.btnGetAccountID.Visible = false;
            this.btnGetAccountID.Click += new System.EventHandler(this.btnGetAccountID_Click);
            // 
            // grdFileFormats
            // 
            this.grdFileFormats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdFileFormats.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 433;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 150;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn3.Width = 81;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 80;
            ultraGridColumn5.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn5.Header.Caption = "PrimeBroker/Clearer";
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridColumn5.Width = 135;
            ultraGridColumn6.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn6.Header.Caption = "Custodian";
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridColumn6.Width = 135;
            ultraGridColumn7.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn7.Header.Caption = "Administrator";
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn7.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridColumn7.Width = 155;
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn8.Width = 81;
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn9.Hidden = true;
            ultraGridColumn9.Width = 81;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdFileFormats.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdFileFormats.DisplayLayout.GroupByBox.Hidden = true;
            this.grdFileFormats.DisplayLayout.MaxColScrollRegions = 1;
            this.grdFileFormats.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdFileFormats.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdFileFormats.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdFileFormats.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdFileFormats.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdFileFormats.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdFileFormats.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdFileFormats.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdFileFormats.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdFileFormats.Location = new System.Drawing.Point(8, 39);
            this.grdFileFormats.Name = "grdFileFormats";
            this.grdFileFormats.Size = new System.Drawing.Size(596, 117);
            this.grdFileFormats.TabIndex = 67;
            this.grdFileFormats.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdFileFormats_InitializeLayout);
            // 
            // ultraDropDownAccountName
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraDropDownAccountName.DisplayLayout.Appearance = appearance1;
            ultraGridColumn10.Header.VisiblePosition = 0;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn11.Header.VisiblePosition = 1;
            ultraGridColumn12.Header.VisiblePosition = 2;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn13.Header.VisiblePosition = 3;
            ultraGridColumn13.Hidden = true;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13});
            this.ultraDropDownAccountName.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.ultraDropDownAccountName.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraDropDownAccountName.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownAccountName.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraDropDownAccountName.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ultraDropDownAccountName.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraDropDownAccountName.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ultraDropDownAccountName.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraDropDownAccountName.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraDropDownAccountName.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraDropDownAccountName.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ultraDropDownAccountName.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraDropDownAccountName.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownAccountName.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraDropDownAccountName.DisplayLayout.Override.CellAppearance = appearance8;
            this.ultraDropDownAccountName.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraDropDownAccountName.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownAccountName.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ultraDropDownAccountName.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.ultraDropDownAccountName.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraDropDownAccountName.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.ultraDropDownAccountName.DisplayLayout.Override.RowAppearance = appearance11;
            this.ultraDropDownAccountName.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraDropDownAccountName.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.ultraDropDownAccountName.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownAccountName.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraDropDownAccountName.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraDropDownAccountName.DisplayMember = "";
            this.ultraDropDownAccountName.Location = new System.Drawing.Point(0, 222);
            this.ultraDropDownAccountName.Name = "ultraDropDownAccountName";
            this.ultraDropDownAccountName.Size = new System.Drawing.Size(112, 36);
            this.ultraDropDownAccountName.TabIndex = 2;
            this.ultraDropDownAccountName.Text = "ultraDropDown1";
            this.ultraDropDownAccountName.ValueMember = "";
            this.ultraDropDownAccountName.Visible = false;
            // 
            // ultraDropDownPrimeBrokerClearer
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Appearance = appearance13;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.CellAppearance = appearance20;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.RowAppearance = appearance23;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraDropDownPrimeBrokerClearer.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraDropDownPrimeBrokerClearer.DisplayMember = "";
            this.ultraDropDownPrimeBrokerClearer.Location = new System.Drawing.Point(120, 222);
            this.ultraDropDownPrimeBrokerClearer.Name = "ultraDropDownPrimeBrokerClearer";
            this.ultraDropDownPrimeBrokerClearer.Size = new System.Drawing.Size(112, 36);
            this.ultraDropDownPrimeBrokerClearer.TabIndex = 3;
            this.ultraDropDownPrimeBrokerClearer.Text = "ultraDropDown1";
            this.ultraDropDownPrimeBrokerClearer.ValueMember = "";
            this.ultraDropDownPrimeBrokerClearer.Visible = false;
            this.ultraDropDownPrimeBrokerClearer.AfterCloseUp += new Infragistics.Win.UltraWinGrid.DropDownEventHandler(this.ultraDropDownPrimeBrokerClearer_AfterCloseUp);
            this.ultraDropDownPrimeBrokerClearer.AfterDropDown += new Infragistics.Win.UltraWinGrid.DropDownEventHandler(this.ultraDropDownPrimeBrokerClearer_AfterDropDown);
            // 
            // ultraDropDownCustodian
            // 
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraDropDownCustodian.DisplayLayout.Appearance = appearance25;
            ultraGridColumn14.Header.VisiblePosition = 0;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn15.Header.VisiblePosition = 1;
            ultraGridColumn16.Header.VisiblePosition = 2;
            ultraGridColumn16.Hidden = true;
            ultraGridColumn17.Header.VisiblePosition = 3;
            ultraGridColumn17.Hidden = true;
            ultraGridColumn18.Header.VisiblePosition = 4;
            ultraGridColumn18.Hidden = true;
            ultraGridColumn19.Header.VisiblePosition = 5;
            ultraGridColumn19.Hidden = true;
            ultraGridColumn20.Header.VisiblePosition = 6;
            ultraGridColumn20.Hidden = true;
            ultraGridColumn21.Header.VisiblePosition = 7;
            ultraGridColumn21.Hidden = true;
            ultraGridColumn22.Header.VisiblePosition = 8;
            ultraGridColumn22.Hidden = true;
            ultraGridColumn23.Header.VisiblePosition = 9;
            ultraGridColumn23.Hidden = true;
            ultraGridColumn24.Header.VisiblePosition = 10;
            ultraGridColumn24.Hidden = true;
            ultraGridColumn25.Header.VisiblePosition = 11;
            ultraGridColumn25.Hidden = true;
            ultraGridColumn26.Header.VisiblePosition = 12;
            ultraGridColumn26.Hidden = true;
            ultraGridColumn27.Header.VisiblePosition = 13;
            ultraGridColumn27.Hidden = true;
            ultraGridColumn28.Header.VisiblePosition = 14;
            ultraGridColumn28.Hidden = true;
            ultraGridColumn29.Header.VisiblePosition = 15;
            ultraGridColumn29.Hidden = true;
            ultraGridColumn30.Header.VisiblePosition = 16;
            ultraGridColumn30.Hidden = true;
            ultraGridColumn31.Header.VisiblePosition = 17;
            ultraGridColumn31.Hidden = true;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn16,
            ultraGridColumn17,
            ultraGridColumn18,
            ultraGridColumn19,
            ultraGridColumn20,
            ultraGridColumn21,
            ultraGridColumn22,
            ultraGridColumn23,
            ultraGridColumn24,
            ultraGridColumn25,
            ultraGridColumn26,
            ultraGridColumn27,
            ultraGridColumn28,
            ultraGridColumn29,
            ultraGridColumn30,
            ultraGridColumn31});
            this.ultraDropDownCustodian.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.ultraDropDownCustodian.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraDropDownCustodian.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownCustodian.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraDropDownCustodian.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.ultraDropDownCustodian.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraDropDownCustodian.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.ultraDropDownCustodian.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraDropDownCustodian.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraDropDownCustodian.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraDropDownCustodian.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.ultraDropDownCustodian.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraDropDownCustodian.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownCustodian.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraDropDownCustodian.DisplayLayout.Override.CellAppearance = appearance32;
            this.ultraDropDownCustodian.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraDropDownCustodian.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownCustodian.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ultraDropDownCustodian.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.ultraDropDownCustodian.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraDropDownCustodian.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.ultraDropDownCustodian.DisplayLayout.Override.RowAppearance = appearance35;
            this.ultraDropDownCustodian.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraDropDownCustodian.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.ultraDropDownCustodian.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownCustodian.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraDropDownCustodian.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraDropDownCustodian.DisplayMember = "";
            this.ultraDropDownCustodian.Location = new System.Drawing.Point(242, 224);
            this.ultraDropDownCustodian.Name = "ultraDropDownCustodian";
            this.ultraDropDownCustodian.Size = new System.Drawing.Size(106, 36);
            this.ultraDropDownCustodian.TabIndex = 4;
            this.ultraDropDownCustodian.Text = "ultraDropDown1";
            this.ultraDropDownCustodian.ValueMember = "";
            this.ultraDropDownCustodian.Visible = false;
            this.ultraDropDownCustodian.AfterCloseUp += new Infragistics.Win.UltraWinGrid.DropDownEventHandler(this.ultraDropDownCustodian_AfterCloseUp);
            // 
            // ultraDropDownAdministrator
            // 
            appearance37.BackColor = System.Drawing.SystemColors.Window;
            appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraDropDownAdministrator.DisplayLayout.Appearance = appearance37;
            ultraGridColumn32.Header.VisiblePosition = 0;
            ultraGridColumn32.Hidden = true;
            ultraGridColumn33.Header.VisiblePosition = 1;
            ultraGridColumn34.Header.VisiblePosition = 2;
            ultraGridColumn34.Hidden = true;
            ultraGridColumn35.Header.VisiblePosition = 3;
            ultraGridColumn35.Hidden = true;
            ultraGridColumn36.Header.VisiblePosition = 4;
            ultraGridColumn36.Hidden = true;
            ultraGridColumn37.Header.VisiblePosition = 5;
            ultraGridColumn37.Hidden = true;
            ultraGridColumn38.Header.VisiblePosition = 6;
            ultraGridColumn38.Hidden = true;
            ultraGridColumn39.Header.VisiblePosition = 7;
            ultraGridColumn39.Hidden = true;
            ultraGridColumn40.Header.VisiblePosition = 8;
            ultraGridColumn40.Hidden = true;
            ultraGridColumn41.Header.VisiblePosition = 9;
            ultraGridColumn41.Hidden = true;
            ultraGridColumn42.Header.VisiblePosition = 10;
            ultraGridColumn42.Hidden = true;
            ultraGridColumn43.Header.VisiblePosition = 11;
            ultraGridColumn43.Hidden = true;
            ultraGridColumn44.Header.VisiblePosition = 12;
            ultraGridColumn44.Hidden = true;
            ultraGridColumn45.Header.VisiblePosition = 13;
            ultraGridColumn45.Hidden = true;
            ultraGridColumn46.Header.VisiblePosition = 14;
            ultraGridColumn46.Hidden = true;
            ultraGridColumn47.Header.VisiblePosition = 15;
            ultraGridColumn47.Hidden = true;
            ultraGridColumn48.Header.VisiblePosition = 16;
            ultraGridColumn48.Hidden = true;
            ultraGridColumn49.Header.VisiblePosition = 17;
            ultraGridColumn49.Hidden = true;
            ultraGridBand4.Columns.AddRange(new object[] {
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
            ultraGridColumn47,
            ultraGridColumn48,
            ultraGridColumn49});
            this.ultraDropDownAdministrator.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.ultraDropDownAdministrator.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraDropDownAdministrator.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance38.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance38.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownAdministrator.DisplayLayout.GroupByBox.Appearance = appearance38;
            appearance39.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraDropDownAdministrator.DisplayLayout.GroupByBox.BandLabelAppearance = appearance39;
            this.ultraDropDownAdministrator.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance40.BackColor2 = System.Drawing.SystemColors.Control;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance40.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraDropDownAdministrator.DisplayLayout.GroupByBox.PromptAppearance = appearance40;
            this.ultraDropDownAdministrator.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraDropDownAdministrator.DisplayLayout.MaxRowScrollRegions = 1;
            appearance41.BackColor = System.Drawing.SystemColors.Window;
            appearance41.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraDropDownAdministrator.DisplayLayout.Override.ActiveCellAppearance = appearance41;
            appearance42.BackColor = System.Drawing.SystemColors.Highlight;
            appearance42.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraDropDownAdministrator.DisplayLayout.Override.ActiveRowAppearance = appearance42;
            this.ultraDropDownAdministrator.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraDropDownAdministrator.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance43.BackColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownAdministrator.DisplayLayout.Override.CardAreaAppearance = appearance43;
            appearance44.BorderColor = System.Drawing.Color.Silver;
            appearance44.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraDropDownAdministrator.DisplayLayout.Override.CellAppearance = appearance44;
            this.ultraDropDownAdministrator.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraDropDownAdministrator.DisplayLayout.Override.CellPadding = 0;
            appearance45.BackColor = System.Drawing.SystemColors.Control;
            appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance45.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance45.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraDropDownAdministrator.DisplayLayout.Override.GroupByRowAppearance = appearance45;
            appearance46.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ultraDropDownAdministrator.DisplayLayout.Override.HeaderAppearance = appearance46;
            this.ultraDropDownAdministrator.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraDropDownAdministrator.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            appearance47.BorderColor = System.Drawing.Color.Silver;
            this.ultraDropDownAdministrator.DisplayLayout.Override.RowAppearance = appearance47;
            this.ultraDropDownAdministrator.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance48.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraDropDownAdministrator.DisplayLayout.Override.TemplateAddRowAppearance = appearance48;
            this.ultraDropDownAdministrator.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraDropDownAdministrator.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraDropDownAdministrator.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraDropDownAdministrator.DisplayMember = "";
            this.ultraDropDownAdministrator.Location = new System.Drawing.Point(354, 224);
            this.ultraDropDownAdministrator.Name = "ultraDropDownAdministrator";
            this.ultraDropDownAdministrator.Size = new System.Drawing.Size(112, 36);
            this.ultraDropDownAdministrator.TabIndex = 5;
            this.ultraDropDownAdministrator.Text = "ultraDropDown1";
            this.ultraDropDownAdministrator.ValueMember = "";
            this.ultraDropDownAdministrator.Visible = false;
            this.ultraDropDownAdministrator.AfterCloseUp += new Infragistics.Win.UltraWinGrid.DropDownEventHandler(this.ultraDropDownAdministrator_AfterCloseUp);
            // 
            // CompanyThirdPartyFileFormats
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.ultraDropDownCustodian);
            this.Controls.Add(this.ultraDropDownPrimeBrokerClearer);
            this.Controls.Add(this.ultraDropDownAccountName);
            this.Controls.Add(this.grpFileFormats);
            this.Controls.Add(this.ultraDropDownAdministrator);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CompanyThirdPartyFileFormats";
            this.Size = new System.Drawing.Size(620, 262);
            this.grpFileFormats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdFileFormats)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownAccountName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownPrimeBrokerClearer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownCustodian)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDropDownAdministrator)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        Accounts _fileFormatAccounts = new Accounts();
        public Prana.Admin.BLL.Accounts FileFormatsAccounts
        {
            get
            {
                _fileFormatAccounts = (Prana.Admin.BLL.Accounts)grdFileFormats.DataSource;
                Prana.Admin.BLL.Accounts validFileFormatAccounts = new Accounts();
                int index = 1;
                foreach (BLL.Account fileFormatAccount in _fileFormatAccounts)
                {
                    if (fileFormatAccount.AccountID == MIN_VALUE)
                    {
                        MessageBox.Show("No account is available to save against the third party so the information can not be saved.");
                        validFileFormatAccounts = null;
                        return validFileFormatAccounts;
                    }
                    if (fileFormatAccount.CompanyPrimeBrokerClearerID == MIN_VALUE)
                    {
                        MessageBox.Show("Please select the PrimeBroker/Clearer in the row: " + index);
                        validFileFormatAccounts = null;
                        return validFileFormatAccounts;
                    }
                    if (fileFormatAccount.CompanyCustodianID == MIN_VALUE)
                    {
                        MessageBox.Show("Please select the Custodian in the row: " + index);
                        validFileFormatAccounts = null;
                        return validFileFormatAccounts;
                    }
                    if (fileFormatAccount.CompanyAdministratorID == MIN_VALUE)
                    {
                        MessageBox.Show("Please select the Administrator in the row: " + index);
                        validFileFormatAccounts = null;
                        return validFileFormatAccounts;
                    }
                    validFileFormatAccounts.Add(fileFormatAccount);
                    index++;

                }


                //return (Prana.Admin.BLL.Accounts) grdFileFormats.DataSource;
                return validFileFormatAccounts;

            }
            set
            {
                //				Accounts fileFormatAccounts = CompanyManager.GetCompanyThirdPartyFileFormats(_companyID);
                _fileFormatAccounts = value;
                if (_fileFormatAccounts.Count > 0)
                {
                    grdFileFormats.DataSource = _fileFormatAccounts;
                }
            }
        }

        public void BindDataGrid()
        {
            try
            {
                //				ClientAccount nullClientAccount = new ClientAccount(int.MinValue, "");
                //				Prana.Admin.BLL.ClientAccounts clientAccounts = ClientAccountManager.GetCompanyClientAccounts(32);
                //				if(clientAccounts.Count <= 0)
                //				{
                //					clientAccounts.Add(nullClientAccount);
                //				}
                //				else
                //				{
                //					//_nullRow = false;
                //				}
                Accounts companyAccounts = CompanyManager.GetAccount(_companyID);
                BLL.Account nullAccount = new BLL.Account();
                if (companyAccounts.Count <= 0)
                {
                    companyAccounts.Add(nullAccount);
                }
                grdFileFormats.DataSource = companyAccounts;
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnLogin_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnLogin_Click", null);
                #endregion
            }
        }

        private int _companyID = int.MinValue;
        public int CompanyID
        {
            get { return _companyID; }
            //			set
            //			{
            //				_companyID = value;
            //				BindDataGrid();
            //			}
        }

        public void SetupControl(int companyID)
        {
            _companyID = companyID;
            BindDataGrid();
        }

        private void grdFileFormats_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            Prana.Admin.BLL.Accounts accounts = CompanyManager.GetAccount(_companyID);


            //			cmbAccountName.DataSource = accounts;
            //			cmbAccountName.ValueMember = "AccountID";
            //			cmbAccountName.DisplayMember = "AccountName";

            //			ultraDropDownAccountName.DataSource = accounts;
            //			ultraDropDownAccountName.ValueMember = "AccountID";
            //			ultraDropDownAccountName.DisplayMember = "AccountName";
            //			grdFileFormats.DisplayLayout.Bands[0].Columns["AccountName"].ValueList = ultraDropDownAccountName;

            ThirdParties companyPBCThirdParties = new ThirdParties();
            ThirdParties companyCustThirdParties = new ThirdParties();
            ThirdParties companyAdmThirdParties = new ThirdParties();
            ThirdParties companyThirdParties = ThirdPartyDataManager.GetCompanyThirdParties(_companyID);
            int thirdPartyTypeID = int.MinValue;
            foreach (Prana.BusinessObjects.ThirdParty thirdParty in companyThirdParties)
            {
                thirdPartyTypeID = int.Parse(ThirdPartyDataManager.GetThirdPartyTypeId(thirdParty).ToString());
                switch (thirdPartyTypeID)
                {
                    case C_TYPE_PRIMEBROKERCLEARER:
                        companyPBCThirdParties.Add(thirdParty);
                        break;
                    case C_TYPE_CUSTODIAN:
                        companyCustThirdParties.Add(thirdParty);
                        break;
                    case C_TYPE_ADMINISTRATOR:
                        companyAdmThirdParties.Add(thirdParty);
                        break;

                }
            }
            companyPBCThirdParties.Insert(0, new Prana.BusinessObjects.ThirdParty(int.MinValue, C_COMBO_SELECT));
            ultraDropDownPrimeBrokerClearer.DataSource = null;
            ultraDropDownPrimeBrokerClearer.DataSource = companyPBCThirdParties;
            ultraDropDownPrimeBrokerClearer.ValueMember = "CompanyThirdPartyID";
            ultraDropDownPrimeBrokerClearer.DisplayMember = "ThirdPartyName";
            ultraDropDownPrimeBrokerClearer.Text = C_COMBO_SELECT;

            grdFileFormats.DisplayLayout.Bands[0].Columns["CompanyPrimeBrokerClearerID"].ValueList = ultraDropDownPrimeBrokerClearer;
            ColumnsCollection columnsPBC = ultraDropDownPrimeBrokerClearer.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsPBC)
            {
                if (column.Key != "ThirdPartyName")
                {
                    column.Hidden = true;
                }
            }

            companyCustThirdParties.Insert(0, new Prana.BusinessObjects.ThirdParty(int.MinValue, C_COMBO_SELECT));
            ultraDropDownCustodian.DataSource = null;
            ultraDropDownCustodian.DataSource = companyCustThirdParties;
            ultraDropDownCustodian.ValueMember = "CompanyThirdPartyID";
            ultraDropDownCustodian.DisplayMember = "ThirdPartyName";
            grdFileFormats.DisplayLayout.Bands[0].Columns["CompanyCustodianID"].ValueList = ultraDropDownCustodian;
            ultraDropDownCustodian.Text = C_COMBO_SELECT;

            ColumnsCollection columnsCustodian = ultraDropDownCustodian.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsCustodian)
            {
                if (column.Key != "ThirdPartyName")
                {
                    column.Hidden = true;
                }
            }

            companyAdmThirdParties.Insert(0, new Prana.BusinessObjects.ThirdParty(int.MinValue, C_COMBO_SELECT));
            ultraDropDownAdministrator.DataSource = null;
            ultraDropDownAdministrator.DataSource = companyAdmThirdParties;
            ultraDropDownAdministrator.ValueMember = "CompanyThirdPartyID";
            ultraDropDownAdministrator.DisplayMember = "ThirdPartyName";
            grdFileFormats.DisplayLayout.Bands[0].Columns["CompanyAdministratorID"].ValueList = ultraDropDownAdministrator;
            ultraDropDownAdministrator.Text = C_COMBO_SELECT;

            ColumnsCollection columnsAdministrator = ultraDropDownAdministrator.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columnsAdministrator)
            {
                if (column.Key != "ThirdPartyName")
                {
                    column.Hidden = true;
                }
            }

        }

        private void btnGetAccountID_Click(object sender, System.EventArgs e)
        {
            //			int accountID = int.Parse(grdFileFormats.ActiveRow.Cells["AccountID"].Value.ToString());
            //			string accountName = grdFileFormats.DisplayLayout.ActiveRow.Cells["AccountName"].Text.ToString();
            //			//MessageBox.Show("AccountName = " + accountName);
            //			MessageBox.Show("AccountID = " + accountID + "AccountName = " + accountName);

            int companyThirdPartyID = int.Parse(grdFileFormats.ActiveRow.Cells["CompanyAdministratorID"].Value.ToString());
            string thirdPartyName = grdFileFormats.DisplayLayout.ActiveRow.Cells["CompanyAdministratorID"].Text.ToString();
            MessageBox.Show("CompanyThirdPartyID = " + companyThirdPartyID + "ThirdPartyName = " + thirdPartyName);
        }

        private void ultraDropDownPrimeBrokerClearer_AfterDropDown(object sender, Infragistics.Win.UltraWinGrid.DropDownEventArgs e)
        {
            //MessageBox.Show("Hi");
        }

        private void ultraDropDownPrimeBrokerClearer_AfterCloseUp(object sender, Infragistics.Win.UltraWinGrid.DropDownEventArgs e)
        {
            //MessageBox.Show("Hi");
            int selectedIndex = int.Parse(grdFileFormats.ActiveRow.Index.ToString());
            this.grdFileFormats.Rows[selectedIndex].Selected = true;
            this.grdFileFormats.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
        }

        private void ultraDropDownCustodian_AfterCloseUp(object sender, Infragistics.Win.UltraWinGrid.DropDownEventArgs e)
        {
            int selectedIndex = int.Parse(grdFileFormats.ActiveRow.Index.ToString());
            this.grdFileFormats.Rows[selectedIndex].Selected = true;
            this.grdFileFormats.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
        }

        private void ultraDropDownAdministrator_AfterCloseUp(object sender, Infragistics.Win.UltraWinGrid.DropDownEventArgs e)
        {
            int selectedIndex = int.Parse(grdFileFormats.ActiveRow.Index.ToString());
            this.grdFileFormats.Rows[selectedIndex].Selected = true;
            this.grdFileFormats.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
        }

    }
}
