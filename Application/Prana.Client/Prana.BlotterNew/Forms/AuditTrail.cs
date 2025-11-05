using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.Global;
using Prana.LogManager;
using Prana.ShortLocate.Preferences;
using Prana.Utilities.UI.UIUtilities;
using System;

namespace Prana.Blotter
{
    public class AuditTrail : System.Windows.Forms.Form
    {
        private ShortLocateUIPreferences _shortLocatePreferences = null;
        private ctrlShortLocatePrefDataManager Dataobj = new ctrlShortLocatePrefDataManager();
        private System.Windows.Forms.ImageList imlToolBaar;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdTicketDetails;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdAuditTrail;
        private Infragistics.Win.Misc.UltraGroupBox ugbxAuditHeader;
        private Infragistics.Win.Misc.UltraGroupBox ugbxBody;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AuditTrail_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AuditTrail_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AuditTrail_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AuditTrail_UltraFormManager_Dock_Area_Bottom;
        private System.ComponentModel.IContainer components;

        public AuditTrail(string _clorderID)
        {
            try
            {
                InitializeComponent();
                this.ClOrderId = _clorderID;
                if (_clorderID != string.Empty)
                {
                    SetAuditTrail(_clorderID);
                }
                if (CustomThemeHelper.ApplyTheme && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_AUDIT_TRAIL);
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        private string _clOrderId = string.Empty;
        public string ClOrderId
        {
            get { return _clOrderId; }
            set { _clOrderId = value; }
        }

        private string _clientId = string.Empty;
        public string ClientId
        {
            get { return _clientId; }
            set { _clientId = value; }
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
                if (imlToolBaar != null)
                {
                    imlToolBaar.Dispose();
                }
                if (grdTicketDetails != null)
                {
                    grdTicketDetails.Dispose();
                }
                if (grdAuditTrail != null)
                {
                    grdAuditTrail.Dispose();
                }
                if (ugbxAuditHeader != null)
                {
                    ugbxAuditHeader.Dispose();
                }
                if (ugbxBody != null)
                {
                    ugbxBody.Dispose();
                }
                if (ultraPanel1 != null)
                {
                    ultraPanel1.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (_AuditTrail_UltraFormManager_Dock_Area_Left != null)
                {
                    _AuditTrail_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (_AuditTrail_UltraFormManager_Dock_Area_Right != null)
                {
                    _AuditTrail_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (_AuditTrail_UltraFormManager_Dock_Area_Top != null)
                {
                    _AuditTrail_UltraFormManager_Dock_Area_Top.Dispose();
                }
                if (_AuditTrail_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _AuditTrail_UltraFormManager_Dock_Area_Bottom.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuditTrail));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("EntryTime");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterParty");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Venue");
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingAccount");
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Side");
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Quantity");
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecurityID");
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderType");
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TIF");
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExecutionInstruction");
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("HandlingInstruction");
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderSide", 0, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Symbol", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TransactionTime", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClOrderID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Price", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingAccountName", 5);
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridLayout ultraGridLayout1 = new Infragistics.Win.UltraWinGrid.UltraGridLayout();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Time");
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Event", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, false);
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook2 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridLayout ultraGridLayout2 = new Infragistics.Win.UltraWinGrid.UltraGridLayout();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            this.imlToolBaar = new System.Windows.Forms.ImageList(this.components);
            this.grdTicketDetails = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grdAuditTrail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ugbxAuditHeader = new Infragistics.Win.Misc.UltraGroupBox();
            this.ugbxBody = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._AuditTrail_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AuditTrail_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AuditTrail_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AuditTrail_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.grdTicketDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAuditTrail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxAuditHeader)).BeginInit();
            this.ugbxAuditHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxBody)).BeginInit();
            this.ugbxBody.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // imlToolBaar
            // 
            this.imlToolBaar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlToolBaar.ImageStream")));
            this.imlToolBaar.TransparentColor = System.Drawing.Color.Red;
            this.imlToolBaar.Images.SetKeyName(0, "");
            // 
            // grdTicketDetails
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdTicketDetails.DisplayLayout.Appearance = appearance1;
            appearance2.TextHAlignAsString = "Center";
            ultraGridColumn1.CellAppearance = appearance2;
            appearance3.TextHAlignAsString = "Center";
            ultraGridColumn1.Header.Appearance = appearance3;
            ultraGridColumn1.Header.VisiblePosition = 4;
            ultraGridColumn1.Hidden = true;
            appearance4.TextHAlignAsString = "Center";
            ultraGridColumn2.CellAppearance = appearance4;
            appearance5.TextHAlignAsString = "Center";
            ultraGridColumn2.Header.Appearance = appearance5;
            ultraGridColumn2.Header.VisiblePosition = 12;
            ultraGridColumn2.Hidden = true;
            appearance6.TextHAlignAsString = "Center";
            ultraGridColumn3.CellAppearance = appearance6;
            appearance7.TextHAlignAsString = "Center";
            ultraGridColumn3.Header.Appearance = appearance7;
            ultraGridColumn3.Header.VisiblePosition = 10;
            ultraGridColumn3.Hidden = true;
            appearance8.TextHAlignAsString = "Center";
            ultraGridColumn4.CellAppearance = appearance8;
            appearance9.TextHAlignAsString = "Center";
            ultraGridColumn4.Header.Appearance = appearance9;
            ultraGridColumn4.Header.VisiblePosition = 9;
            ultraGridColumn4.Hidden = true;
            appearance10.TextHAlignAsString = "Center";
            ultraGridColumn5.CellAppearance = appearance10;
            appearance11.TextHAlignAsString = "Center";
            ultraGridColumn5.Header.Appearance = appearance11;
            ultraGridColumn5.Header.VisiblePosition = 8;
            ultraGridColumn5.Hidden = true;
            appearance12.TextHAlignAsString = "Center";
            ultraGridColumn6.CellAppearance = appearance12;
            appearance13.TextHAlignAsString = "Center";
            ultraGridColumn6.Header.Appearance = appearance13;
            ultraGridColumn6.Header.VisiblePosition = 11;
            ultraGridColumn6.Hidden = true;
            appearance14.TextHAlignAsString = "Center";
            ultraGridColumn7.CellAppearance = appearance14;
            ultraGridColumn7.Header.VisiblePosition = 5;
            ultraGridColumn7.Width = 93;
            appearance15.TextHAlignAsString = "Center";
            ultraGridColumn8.CellAppearance = appearance15;
            appearance16.TextHAlignAsString = "Center";
            ultraGridColumn8.Header.Appearance = appearance16;
            ultraGridColumn8.Header.VisiblePosition = 13;
            ultraGridColumn8.Hidden = true;
            appearance17.TextHAlignAsString = "Center";
            ultraGridColumn9.CellAppearance = appearance17;
            appearance18.TextHAlignAsString = "Center";
            ultraGridColumn9.Header.Appearance = appearance18;
            ultraGridColumn9.Header.VisiblePosition = 3;
            ultraGridColumn9.Width = 85;
            appearance19.TextHAlignAsString = "Center";
            ultraGridColumn10.CellAppearance = appearance19;
            appearance20.TextHAlignAsString = "Center";
            ultraGridColumn10.Header.Appearance = appearance20;
            ultraGridColumn10.Header.VisiblePosition = 14;
            ultraGridColumn10.Width = 62;
            appearance21.TextHAlignAsString = "Center";
            ultraGridColumn11.CellAppearance = appearance21;
            appearance22.TextHAlignAsString = "Center";
            ultraGridColumn11.Header.Appearance = appearance22;
            ultraGridColumn11.Header.VisiblePosition = 15;
            ultraGridColumn11.Width = 55;
            appearance23.TextHAlignAsString = "Center";
            ultraGridColumn12.CellAppearance = appearance23;
            appearance24.TextHAlignAsString = "Center";
            ultraGridColumn12.Header.Appearance = appearance24;
            ultraGridColumn12.Header.VisiblePosition = 16;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn13.Header.VisiblePosition = 1;
            ultraGridColumn13.Width = 55;
            ultraGridColumn14.Header.VisiblePosition = 2;
            ultraGridColumn14.Width = 48;
            ultraGridColumn15.Header.VisiblePosition = 17;
            ultraGridColumn15.Width = 33;
            ultraGridColumn16.Header.VisiblePosition = 0;
            ultraGridColumn16.Width = 81;
            ultraGridColumn17.Header.VisiblePosition = 6;
            ultraGridColumn17.Width = 55;
            ultraGridColumn18.Header.VisiblePosition = 7;
            ultraGridColumn18.Width = 59;
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
            ultraGridColumn18});
            ultraGridBand1.Header.Enabled = false;
            appearance25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            ultraGridBand1.Override.RowAlternateAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.Black;
            appearance26.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            ultraGridBand1.Override.RowAppearance = appearance26;
            this.grdTicketDetails.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdTicketDetails.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdTicketDetails.DisplayLayout.GroupByBox.Hidden = true;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.ForeColor = System.Drawing.Color.Black;
            this.grdTicketDetails.DisplayLayout.Override.ActiveRowAppearance = appearance27;
            this.grdTicketDetails.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdTicketDetails.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdTicketDetails.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdTicketDetails.DisplayLayout.Override.CellPadding = 0;
            this.grdTicketDetails.DisplayLayout.Override.DefaultColWidth = 50;
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdTicketDetails.DisplayLayout.Override.HeaderAppearance = appearance28;
            this.grdTicketDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdTicketDetails.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance29.ForeColor = System.Drawing.Color.Lime;
            this.grdTicketDetails.DisplayLayout.Override.RowAlternateAppearance = appearance29;
            appearance30.BackColor = System.Drawing.Color.Black;
            appearance30.ForeColor = System.Drawing.Color.Lime;
            this.grdTicketDetails.DisplayLayout.Override.RowAppearance = appearance30;
            appearance31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdTicketDetails.DisplayLayout.Override.RowSelectorAppearance = appearance31;
            this.grdTicketDetails.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.grdTicketDetails.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdTicketDetails.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdTicketDetails.DisplayLayout.Override.SpecialRowSeparator = ((Infragistics.Win.UltraWinGrid.SpecialRowSeparator)(((Infragistics.Win.UltraWinGrid.SpecialRowSeparator.TemplateAddRow | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FixedRows)
            | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.SummaryRow)));
            appearance32.BackColor = System.Drawing.SystemColors.Info;
            this.grdTicketDetails.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            scrollBarLook1.Appearance = appearance33;
            this.grdTicketDetails.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdTicketDetails.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdTicketDetails.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdTicketDetails.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdTicketDetails.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdTicketDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTicketDetails.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridLayout1.BandsSerializer.Add(ultraGridBand2);
            this.grdTicketDetails.Layouts.Add(ultraGridLayout1);
            this.grdTicketDetails.Location = new System.Drawing.Point(3, 20);
            this.grdTicketDetails.Name = "grdTicketDetails";
            this.grdTicketDetails.Size = new System.Drawing.Size(632, 97);
            this.grdTicketDetails.TabIndex = 1;
            this.grdTicketDetails.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // grdAuditTrail
            // 
            appearance34.BackColor = System.Drawing.Color.Black;
            this.grdAuditTrail.DisplayLayout.Appearance = appearance34;
            appearance35.TextHAlignAsString = "Center";
            ultraGridColumn19.CellAppearance = appearance35;
            ultraGridColumn19.Header.VisiblePosition = 0;
            ultraGridColumn19.Width = 100;
            appearance36.TextHAlignAsString = "Center";
            ultraGridColumn20.CellAppearance = appearance36;
            ultraGridColumn20.Header.VisiblePosition = 1;
            ultraGridColumn20.Width = 100;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn19,
            ultraGridColumn20});
            this.grdAuditTrail.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.grdAuditTrail.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdAuditTrail.DisplayLayout.GroupByBox.Hidden = true;
            appearance37.BackColor = System.Drawing.Color.Gold;
            appearance37.BorderColor = System.Drawing.Color.Black;
            appearance37.ForeColor = System.Drawing.Color.Black;
            this.grdAuditTrail.DisplayLayout.Override.ActiveRowAppearance = appearance37;
            this.grdAuditTrail.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdAuditTrail.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdAuditTrail.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdAuditTrail.DisplayLayout.Override.CellPadding = 0;
            this.grdAuditTrail.DisplayLayout.Override.DefaultColWidth = 50;
            appearance38.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdAuditTrail.DisplayLayout.Override.HeaderAppearance = appearance38;
            this.grdAuditTrail.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdAuditTrail.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance39.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance39.ForeColor = System.Drawing.Color.Lime;
            this.grdAuditTrail.DisplayLayout.Override.RowAlternateAppearance = appearance39;
            appearance40.BackColor = System.Drawing.Color.Black;
            appearance40.ForeColor = System.Drawing.Color.Lime;
            this.grdAuditTrail.DisplayLayout.Override.RowAppearance = appearance40;
            appearance41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdAuditTrail.DisplayLayout.Override.RowSelectorAppearance = appearance41;
            this.grdAuditTrail.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.grdAuditTrail.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdAuditTrail.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdAuditTrail.DisplayLayout.Override.SpecialRowSeparator = ((Infragistics.Win.UltraWinGrid.SpecialRowSeparator)(((Infragistics.Win.UltraWinGrid.SpecialRowSeparator.TemplateAddRow | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FixedRows)
            | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.SummaryRow)));
            appearance42.BackColor = System.Drawing.SystemColors.Info;
            this.grdAuditTrail.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance42;
            appearance43.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            scrollBarLook2.Appearance = appearance43;
            this.grdAuditTrail.DisplayLayout.ScrollBarLook = scrollBarLook2;
            this.grdAuditTrail.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAuditTrail.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAuditTrail.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAuditTrail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAuditTrail.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdAuditTrail.Layouts.Add(ultraGridLayout2);
            this.grdAuditTrail.Location = new System.Drawing.Point(3, 3);
            this.grdAuditTrail.Name = "grdAuditTrail";
            this.grdAuditTrail.Size = new System.Drawing.Size(632, 359);
            this.grdAuditTrail.TabIndex = 0;
            this.grdAuditTrail.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ugbxAuditHeader
            // 
            appearance44.FontData.SizeInPoints = 10F;
            this.ugbxAuditHeader.Appearance = appearance44;
            this.ugbxAuditHeader.Controls.Add(this.grdTicketDetails);
            this.ugbxAuditHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.ugbxAuditHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ugbxAuditHeader.Location = new System.Drawing.Point(0, 0);
            this.ugbxAuditHeader.Name = "ugbxAuditHeader";
            this.ugbxAuditHeader.Size = new System.Drawing.Size(638, 120);
            this.ugbxAuditHeader.TabIndex = 4;
            this.ugbxAuditHeader.Text = "Ticket Details";
            // 
            // ugbxBody
            // 
            appearance45.FontData.SizeInPoints = 11F;
            this.ugbxBody.Appearance = appearance45;
            this.ugbxBody.Controls.Add(this.grdAuditTrail);
            this.ugbxBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugbxBody.Location = new System.Drawing.Point(0, 120);
            this.ugbxBody.Name = "ugbxBody";
            this.ugbxBody.Size = new System.Drawing.Size(638, 365);
            this.ugbxBody.TabIndex = 5;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxBody);
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxAuditHeader);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(4, 27);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(638, 485);
            this.ultraPanel1.TabIndex = 2;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _AuditTrail_UltraFormManager_Dock_Area_Left
            // 
            this._AuditTrail_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AuditTrail_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AuditTrail_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AuditTrail_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AuditTrail_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AuditTrail_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._AuditTrail_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._AuditTrail_UltraFormManager_Dock_Area_Left.Name = "_AuditTrail_UltraFormManager_Dock_Area_Left";
            this._AuditTrail_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 485);
            // 
            // _AuditTrail_UltraFormManager_Dock_Area_Right
            // 
            this._AuditTrail_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AuditTrail_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AuditTrail_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AuditTrail_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AuditTrail_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AuditTrail_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._AuditTrail_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(642, 27);
            this._AuditTrail_UltraFormManager_Dock_Area_Right.Name = "_AuditTrail_UltraFormManager_Dock_Area_Right";
            this._AuditTrail_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 485);
            // 
            // _AuditTrail_UltraFormManager_Dock_Area_Top
            // 
            this._AuditTrail_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AuditTrail_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AuditTrail_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AuditTrail_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AuditTrail_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AuditTrail_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AuditTrail_UltraFormManager_Dock_Area_Top.Name = "_AuditTrail_UltraFormManager_Dock_Area_Top";
            this._AuditTrail_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(646, 27);
            // 
            // _AuditTrail_UltraFormManager_Dock_Area_Bottom
            // 
            this._AuditTrail_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AuditTrail_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AuditTrail_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AuditTrail_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AuditTrail_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AuditTrail_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._AuditTrail_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 512);
            this._AuditTrail_UltraFormManager_Dock_Area_Bottom.Name = "_AuditTrail_UltraFormManager_Dock_Area_Bottom";
            this._AuditTrail_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(646, 4);
            // 
            // AuditTrail
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(646, 516);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this._AuditTrail_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AuditTrail_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AuditTrail_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AuditTrail_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "AuditTrail";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Audit Trail";
            ((System.ComponentModel.ISupportInitialize)(this.grdTicketDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAuditTrail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxAuditHeader)).EndInit();
            this.ugbxAuditHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxBody)).EndInit();
            this.ugbxBody.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        OrderCollection _orderColl = null;

        /// <summary>
        /// The aim here is to Bind the data corresponding to a particular order in different Grids.
        /// Main order in the top most grid, Requests in 2nd grid and all fills in the third grid
        /// </summary>
        /// <param name="_clOrderID"></param>
        private void SetAuditTrail(string _clOrderID)
        {
            try
            {
                Order orderDetails = new Order();
                orderDetails = AuditTrailManager.GetInstance().GetSubOrderDetails(_clOrderID);
                OrderCollection orderTrail = new OrderCollection();
                orderTrail = AuditTrailManager.GetInstance().GetTrailByOrderID(_clOrderID);//added order type,UserID
                OrderCollection fills = new OrderCollection();
                OrderCollection totalFills = new OrderCollection();
                foreach (Order order in orderTrail)
                {
                    fills = AuditTrailManager.GetInstance().GetFillsByOrderID(order.ClOrderID, order); //added exec type
                    foreach (Order fill in fills)
                    {
                        totalFills.Add(fill);
                    }
                    order.SubOrders = fills;

                }
                _orderColl = new OrderCollection();
                _orderColl.Add(orderDetails);
                grdTicketDetails.DataSource = null;
                grdTicketDetails.DataSource = _orderColl;
                grdAuditTrail.DataSource = orderTrail;
                HideColumns();
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


        private void HideColumns()
        {
            HideColumnsGridOrderDetails();
            HideColumnsGridAuditTrail();

            if (grdAuditTrail.DisplayLayout.Bands.Count > 1)
            {
                foreach (UltraGridRow existingRow in grdAuditTrail.Rows)
                {
                    if (existingRow.ChildBands.Count > 0)
                    {
                        existingRow.ChildBands[0].Rows.ExpandAll(false);
                    }
                }
            }
        }

        private void HideColumnsGridOrderDetails()
        {
            foreach (UltraGridBand band in grdTicketDetails.DisplayLayout.Bands)
            {
                ColumnsCollection columns = band.Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Header.Caption.ToString().Trim() != "ClOrderID"
                        && column.Header.Caption.ToString().Trim() != "OrderSide"
                        && column.Header.Caption.ToString().Trim() != "OrderType"
                        && column.Header.Caption.ToString().Trim() != "Symbol"
                        && column.Header.Caption.ToString().Trim() != "Quantity"
                        && column.Header.Caption.ToString().Trim() != "ClientTime"
                        && column.Header.Caption.ToString().Trim() != "TIF"
                        && column.Header.Caption.ToString().Trim() != "Price"
                        && column.Header.Caption.ToString().Trim() != "TradingAccountName"
                        && column.Header.Caption.ToString().Trim() != "ExecInstruction"
                        && column.Header.Caption.ToString().Trim() != "TransactionTime"
                        )
                    {
                        column.Hidden = true;
                    }
                    else
                    {
                        if (column.Header.Caption.ToString().Trim() == "ClientTime")
                        {
                            column.Header.Caption = "Time";
                        }
                        if (column.Header.Caption.Trim().Equals("OrderSide"))
                        {
                            column.Header.Caption = "Order Side";
                        }
                        if (column.Header.Caption.Trim().Equals("OrderType"))
                        {
                            column.Header.Caption = "Order Type";
                        }
                        if (column.Header.Caption.Trim().Equals("TradingAccountName"))
                        {
                            column.Header.Caption = "Trader";
                        }
                        if (column.Header.Caption.Trim().Equals("ExecInstruction"))
                        {
                            column.Header.Caption = "Execution Instruction";
                        }
                        if (column.Header.Caption.Trim().Equals("TransactionTime"))
                        {
                            column.Header.Caption = "Transaction Time";
                        }
                    }
                }
            }
            grdTicketDetails.DisplayLayout.Bands[0].Columns["Quantity"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grdTicketDetails.DisplayLayout.Bands[0].Columns["Price"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grdTicketDetails.DisplayLayout.Bands[0].Columns["Price"].Format = "F4";
        }

        private void HideColumnsGridAuditTrail()
        {
            _shortLocatePreferences = Dataobj.GetShortLocatePreferences(Prana.CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
            UltraGridBand grdAuditTrailBand1 = grdAuditTrail.DisplayLayout.Bands[0];
            ColumnsCollection columnsAuditTrailBand1 = grdAuditTrailBand1.Columns;
            foreach (UltraGridColumn column in columnsAuditTrailBand1)
            {
                if (column.Header.Caption.ToString().Trim() != "Quantity"
                    && column.Header.Caption.ToString().Trim() != "Price"
                    && column.Header.Caption.ToString().Trim() != "ClientTime"
                    && column.Header.Caption.ToString().Trim() != "CompanyUserName"
                    && column.Header.Caption.ToString().Trim() != "ClOrderID"
                    && column.Header.Caption.ToString().Trim() != "Time"
                   && column.Header.Caption.ToString().Trim() != "Total Qty"
                    && column.Header.Caption.ToString().Trim() != "OrderType"
                    && column.Header.Caption.ToString().Trim() != "TransactionTime"
                    && column.Header.Caption.ToString().Trim() != "BorrowerID"
                     && column.Header.Caption.ToString().Trim() != "BorrowerBroker"
                     && column.Header.Caption.ToString().Trim() != "ShortRebate"
                    )
                {
                    column.Hidden = true;

                }
                else
                {
                    if (column.Header.Caption.ToString().Trim() == "TransactionTime")
                    {
                        column.Header.Caption = "Transaction Time";
                    }
                    if (column.Header.Caption.ToString().Trim() == "Quantity")
                    {
                        column.Header.Caption = "Total Qty";
                    }
                    if (column.Header.Caption.ToString().Trim() == "CompanyUserName")
                    {
                        column.Header.Caption = "User";
                    }
                    if (column.Header.Caption.ToString().Trim() == "OrderType")
                    {
                        column.Header.Caption = "Order Type";
                    }
                    column.Width = 65;
                }
            }
            if (_shortLocatePreferences.Rebatefees == ShortLocateRebateFee.BPS.ToString())
                columnsAuditTrailBand1["ShortRebate"].Header.Caption = OrderFields.CAPTION_BORROWERRATEBPS;
            else
                columnsAuditTrailBand1["ShortRebate"].Header.Caption = OrderFields.CAPTION_BORROWERRATECENT;
            columnsAuditTrailBand1["BorrowerID"].Header.Caption = OrderFields.CAPTION_BORROWERID;
            columnsAuditTrailBand1["BorrowerBroker"].Header.Caption = OrderFields.CAPTION_BORROWERBROKER;
            columnsAuditTrailBand1["Quantity"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columnsAuditTrailBand1["Price"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columnsAuditTrailBand1["Price"].Format = "F4";

            UltraGridBand grdAuditTrailBand2 = grdAuditTrail.DisplayLayout.Bands[1];
            ColumnsCollection columnsAuditTrail = grdAuditTrailBand2.Columns;
            foreach (UltraGridColumn columnAuditTrail in columnsAuditTrail)
            {
                if (columnAuditTrail.Header.Caption.ToString().Trim() != "LastShares"
                   && columnAuditTrail.Header.Caption.ToString().Trim() != "LastPrice"
                   && columnAuditTrail.Header.Caption.ToString().Trim() != "TransactionTime"
                   && columnAuditTrail.Header.Caption.ToString().Trim() != "Fill"
                   && columnAuditTrail.Header.Caption.ToString().Trim() != "CumQty"
                   && columnAuditTrail.Header.Caption.ToString().Trim() != "OrderStatus"
                   && columnAuditTrail.Header.Caption.ToString().Trim() != "AvgPrice"
                    && columnAuditTrail.Header.Caption.ToString().Trim() != "Fill Price"
                   && columnAuditTrail.Header.Caption.ToString().Trim() != "Time"
                   )
                {
                    columnAuditTrail.Hidden = true;
                }
                else
                {
                    if (columnAuditTrail.Header.Caption.ToString().Trim() == "LastShares")
                    {
                        columnAuditTrail.Header.Caption = "Fill";
                    }
                    if (columnAuditTrail.Header.Caption.ToString().Trim() == "LastPrice")
                    {
                        columnAuditTrail.Header.Caption = "Fill Price";
                    }
                    if (columnAuditTrail.Header.Caption.ToString().Trim() == "TransactionTime")
                    {
                        columnAuditTrail.Header.Caption = "Transaction Time";
                    }
                    if (columnAuditTrail.Header.Caption.ToString().Trim() == "CumQty")
                    {
                        columnAuditTrail.Header.Caption = "Cum Quantity";
                    }
                    if (columnAuditTrail.Header.Caption.ToString().Trim() == "CumQty")
                    {
                        columnAuditTrail.Header.Caption = "Cum Quantity";
                    }
                    if (columnAuditTrail.Header.Caption.ToString().Trim() == "OrderStatus")
                    {
                        columnAuditTrail.Header.Caption = "Order Status";
                    }
                    if (columnAuditTrail.Header.Caption.ToString().Trim() == "AvgPrice")
                    {
                        columnAuditTrail.Header.Caption = "Average Price";
                    }
                    columnAuditTrail.Width = 65;
                }

            }
            grdAuditTrailBand1.Override.RowAppearance.BackColor = System.Drawing.Color.Black;
            grdAuditTrailBand1.Override.RowAlternateAppearance.ForeColor = System.Drawing.Color.FromArgb(255, 128, 0); ;
            grdAuditTrailBand1.Override.RowAppearance.ForeColor = System.Drawing.Color.FromArgb(255, 128, 0); ;

            grdAuditTrailBand2.Override.RowAlternateAppearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            grdAuditTrailBand2.Override.RowAppearance.ForeColor = System.Drawing.Color.DeepSkyBlue;
            grdAuditTrailBand2.Override.RowAlternateAppearance.ForeColor = System.Drawing.Color.DeepSkyBlue;

            columnsAuditTrail["LastShares"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columnsAuditTrail["LastPrice"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columnsAuditTrail["LastShares"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columnsAuditTrail["LastPrice"].Format = "F4";
            columnsAuditTrail["CumQty"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columnsAuditTrail["AvgPrice"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            columnsAuditTrail["CumQty"].Format = "F4";
        }
    }
}