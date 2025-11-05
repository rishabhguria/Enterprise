using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.PM.Client.UI
{
    partial class CtrlRunDownload
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_secMasterSyncService != null)
                {
                    _secMasterSyncService.Dispose();
                }
                if (_gridBandRunUpload != null)
                {
                    _gridBandRunUpload.Dispose();
                }
                if (_importHelper != null)
                {
                    _importHelper.Dispose();
                }
                if (_pricingServicesProxy != null)
                {
                    _pricingServicesProxy.Dispose();
                }
                if (_displayForm != null)
                {
                    _displayForm.Dispose();
                }
                if (_dsSecMasterInsert != null)
                {
                    _dsSecMasterInsert.Dispose();
                }
                if (_dsCollIntMasterInsert != null)
                {
                    _dsCollIntMasterInsert.Dispose();
                }
                if (_dsAllocationScheme != null)
                {
                    _dsAllocationScheme.Dispose();
                }
                if (_dtSMMapping != null)
                {
                    _dtSMMapping.Dispose();
                }
                if (_schemeForm != null)
                {
                    _schemeForm.Dispose();
                }
                if (_ctrlImportPreferences != null)
                {
                    _ctrlImportPreferences.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SelectButton", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("XSLTFile", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("XSLTSelectButton", 2);
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
            this.gridRunUpload = new PranaUltraGrid();
            this.cmbDataSources = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.cmbCompanies = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.cmbStatus = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.cmbTableTypes = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.btnUpload = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridRunUpload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDataSources)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTableTypes)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridRunUpload
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.gridRunUpload.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn1.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridColumn1.Width = 60;
            ultraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn2.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn2.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn2.Width = 70;
            ultraGridColumn3.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn3.SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
            ultraGridColumn3.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridColumn3.Width = 60;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            this.gridRunUpload.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.gridRunUpload.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridRunUpload.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.gridRunUpload.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridRunUpload.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.gridRunUpload.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.gridRunUpload.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridRunUpload.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.gridRunUpload.DisplayLayout.MaxColScrollRegions = 1;
            this.gridRunUpload.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gridRunUpload.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.LightSlateGray;
            appearance6.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance6.BorderColor = System.Drawing.Color.DimGray;
            appearance6.ForeColor = System.Drawing.Color.White;
            this.gridRunUpload.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.gridRunUpload.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.gridRunUpload.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.gridRunUpload.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.gridRunUpload.DisplayLayout.Override.CellAppearance = appearance8;
            this.gridRunUpload.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.gridRunUpload.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.gridRunUpload.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.gridRunUpload.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.gridRunUpload.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.gridRunUpload.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.gridRunUpload.DisplayLayout.Override.RowAppearance = appearance11;
            this.gridRunUpload.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.gridRunUpload.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.gridRunUpload.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridRunUpload.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridRunUpload.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridRunUpload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRunUpload.Location = new System.Drawing.Point(0, 0);
            this.gridRunUpload.Name = "gridRunUpload";
            this.gridRunUpload.Size = new System.Drawing.Size(706, 292);
            this.gridRunUpload.TabIndex = 40;
            this.gridRunUpload.Text = "ultraGrid1";
            this.gridRunUpload.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridRunUpload_AfterCellUpdate);
            this.gridRunUpload.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.gridRunUpload_InitializeLayout);
            this.gridRunUpload.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridRunUpload_CellChange);
            this.gridRunUpload.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.gridRunUpload_ClickCellButton);
            this.gridRunUpload.BeforeColumnChooserDisplayed += gridRunUpload_BeforeColumnChooserDisplayed;
            this.gridRunUpload.BeforeCustomRowFilterDialog += gridRunUpload_BeforeCustomRowFilterDialog;
            // 
            // cmbDataSources
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbDataSources.DisplayLayout.Appearance = appearance13;
            this.cmbDataSources.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbDataSources.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbDataSources.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbDataSources.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbDataSources.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbDataSources.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbDataSources.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbDataSources.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbDataSources.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbDataSources.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbDataSources.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbDataSources.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbDataSources.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbDataSources.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbDataSources.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbDataSources.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbDataSources.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.cmbDataSources.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbDataSources.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbDataSources.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmbDataSources.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbDataSources.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbDataSources.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbDataSources.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbDataSources.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbDataSources.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbDataSources.Location = new System.Drawing.Point(543, 160);
            this.cmbDataSources.Name = "cmbDataSources";
            this.cmbDataSources.Size = new System.Drawing.Size(106, 31);
            this.cmbDataSources.TabIndex = 42;
            this.cmbDataSources.Visible = false;
            // 
            // cmbCompanies
            // 
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCompanies.DisplayLayout.Appearance = appearance25;
            this.cmbCompanies.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCompanies.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCompanies.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCompanies.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.cmbCompanies.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCompanies.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.cmbCompanies.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCompanies.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCompanies.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCompanies.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.cmbCompanies.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCompanies.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCompanies.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCompanies.DisplayLayout.Override.CellAppearance = appearance32;
            this.cmbCompanies.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCompanies.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCompanies.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlignAsString = "Left";
            this.cmbCompanies.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.cmbCompanies.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCompanies.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.cmbCompanies.DisplayLayout.Override.RowAppearance = appearance35;
            this.cmbCompanies.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCompanies.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.cmbCompanies.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCompanies.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCompanies.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCompanies.Location = new System.Drawing.Point(540, 227);
            this.cmbCompanies.Name = "cmbCompanies";
            this.cmbCompanies.Size = new System.Drawing.Size(106, 31);
            this.cmbCompanies.TabIndex = 41;
            this.cmbCompanies.Visible = false;
            // 
            // cmbStatus
            // 
            appearance37.BackColor = System.Drawing.SystemColors.Window;
            appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbStatus.DisplayLayout.Appearance = appearance37;
            this.cmbStatus.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbStatus.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance38.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance38.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbStatus.DisplayLayout.GroupByBox.Appearance = appearance38;
            appearance39.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbStatus.DisplayLayout.GroupByBox.BandLabelAppearance = appearance39;
            this.cmbStatus.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance40.BackColor2 = System.Drawing.SystemColors.Control;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance40.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbStatus.DisplayLayout.GroupByBox.PromptAppearance = appearance40;
            this.cmbStatus.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbStatus.DisplayLayout.MaxRowScrollRegions = 1;
            appearance41.BackColor = System.Drawing.SystemColors.Window;
            appearance41.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbStatus.DisplayLayout.Override.ActiveCellAppearance = appearance41;
            appearance42.BackColor = System.Drawing.SystemColors.Highlight;
            appearance42.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbStatus.DisplayLayout.Override.ActiveRowAppearance = appearance42;
            this.cmbStatus.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbStatus.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance43.BackColor = System.Drawing.SystemColors.Window;
            this.cmbStatus.DisplayLayout.Override.CardAreaAppearance = appearance43;
            appearance44.BorderColor = System.Drawing.Color.Silver;
            appearance44.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbStatus.DisplayLayout.Override.CellAppearance = appearance44;
            this.cmbStatus.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbStatus.DisplayLayout.Override.CellPadding = 0;
            appearance45.BackColor = System.Drawing.SystemColors.Control;
            appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance45.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance45.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbStatus.DisplayLayout.Override.GroupByRowAppearance = appearance45;
            appearance46.TextHAlignAsString = "Left";
            this.cmbStatus.DisplayLayout.Override.HeaderAppearance = appearance46;
            this.cmbStatus.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbStatus.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            appearance47.BorderColor = System.Drawing.Color.Silver;
            this.cmbStatus.DisplayLayout.Override.RowAppearance = appearance47;
            this.cmbStatus.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance48.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbStatus.DisplayLayout.Override.TemplateAddRowAppearance = appearance48;
            this.cmbStatus.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbStatus.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbStatus.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbStatus.Location = new System.Drawing.Point(3, 227);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(106, 31);
            this.cmbStatus.TabIndex = 43;
            this.cmbStatus.Visible = false;
            // 
            // cmbTableTypes
            // 
            appearance49.BackColor = System.Drawing.SystemColors.Window;
            appearance49.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbTableTypes.DisplayLayout.Appearance = appearance49;
            this.cmbTableTypes.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTableTypes.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance50.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance50.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance50.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance50.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTableTypes.DisplayLayout.GroupByBox.Appearance = appearance50;
            appearance51.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTableTypes.DisplayLayout.GroupByBox.BandLabelAppearance = appearance51;
            this.cmbTableTypes.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance52.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance52.BackColor2 = System.Drawing.SystemColors.Control;
            appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance52.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTableTypes.DisplayLayout.GroupByBox.PromptAppearance = appearance52;
            this.cmbTableTypes.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbTableTypes.DisplayLayout.MaxRowScrollRegions = 1;
            appearance53.BackColor = System.Drawing.SystemColors.Window;
            appearance53.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbTableTypes.DisplayLayout.Override.ActiveCellAppearance = appearance53;
            appearance54.BackColor = System.Drawing.SystemColors.Highlight;
            appearance54.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbTableTypes.DisplayLayout.Override.ActiveRowAppearance = appearance54;
            this.cmbTableTypes.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbTableTypes.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance55.BackColor = System.Drawing.SystemColors.Window;
            this.cmbTableTypes.DisplayLayout.Override.CardAreaAppearance = appearance55;
            appearance56.BorderColor = System.Drawing.Color.Silver;
            appearance56.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbTableTypes.DisplayLayout.Override.CellAppearance = appearance56;
            this.cmbTableTypes.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbTableTypes.DisplayLayout.Override.CellPadding = 0;
            appearance57.BackColor = System.Drawing.SystemColors.Control;
            appearance57.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance57.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance57.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTableTypes.DisplayLayout.Override.GroupByRowAppearance = appearance57;
            appearance58.TextHAlignAsString = "Left";
            this.cmbTableTypes.DisplayLayout.Override.HeaderAppearance = appearance58;
            this.cmbTableTypes.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbTableTypes.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance59.BackColor = System.Drawing.SystemColors.Window;
            appearance59.BorderColor = System.Drawing.Color.Silver;
            this.cmbTableTypes.DisplayLayout.Override.RowAppearance = appearance59;
            this.cmbTableTypes.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance60.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbTableTypes.DisplayLayout.Override.TemplateAddRowAppearance = appearance60;
            this.cmbTableTypes.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbTableTypes.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbTableTypes.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbTableTypes.Location = new System.Drawing.Point(540, 197);
            this.cmbTableTypes.Name = "cmbTableTypes";
            this.cmbTableTypes.Size = new System.Drawing.Size(106, 31);
            this.cmbTableTypes.TabIndex = 46;
            this.cmbTableTypes.Visible = false;
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnUpload.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnUpload.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.Location = new System.Drawing.Point(244, 9);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(100, 23);
            this.btnUpload.TabIndex = 48;
            this.btnUpload.Text = "Upload Data";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnView.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnView.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView.Location = new System.Drawing.Point(362, 9);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(100, 23);
            this.btnView.TabIndex = 48;
            this.btnView.Text = "View File";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "Nirvana Help.chm";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 332);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(706, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 86;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.cmbCompanies);
            this.ultraPanel1.ClientArea.Controls.Add(this.cmbDataSources);
            this.ultraPanel1.ClientArea.Controls.Add(this.cmbStatus);
            this.ultraPanel1.ClientArea.Controls.Add(this.cmbTableTypes);
            this.ultraPanel1.ClientArea.Controls.Add(this.gridRunUpload);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraPanel2);
            this.ultraPanel1.ClientArea.Controls.Add(this.statusStrip1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(706, 354);
            this.ultraPanel1.TabIndex = 87;
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.btnUpload);
            this.ultraPanel2.ClientArea.Controls.Add(this.btnView);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 292);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(706, 40);
            this.ultraPanel2.TabIndex = 87;
            // 
            // CtrlRunDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.ultraPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.helpProvider1.SetHelpKeyword(this, "ImportingPositions.html");
            this.helpProvider1.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "CtrlRunDownload";
            this.helpProvider1.SetShowHelp(this, true);
            this.Size = new System.Drawing.Size(706, 354);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.CtrlRunDownload_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridRunUpload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDataSources)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCompanies)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTableTypes)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PranaUltraGrid gridRunUpload;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbDataSources;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbCompanies;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbStatus;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbTableTypes;
        private Infragistics.Win.Misc.UltraButton btnUpload;
        private Infragistics.Win.Misc.UltraButton btnView;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;


    }
}
