using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.PricingService2UI.APIDataViewer
{
    partial class LiveFeedDataViewer
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
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
            this.btnFetchLiveFeed = new Infragistics.Win.Misc.UltraButton();
            this.grdAppData = new PranaUltraGrid();
            
            this.grdExclExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.btnExportData = new Infragistics.Win.Misc.UltraButton();
            this.btnSaveLayout = new Infragistics.Win.Misc.UltraButton();
            this.LiveFeedDataViewer_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.btnGetActualFeed = new Infragistics.Win.Misc.UltraButton();
            this.tabControlFeed = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.pageApplicationData = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pageLiveFeed = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.panelLiveFeed = new Infragistics.Win.Misc.UltraPanel();
            this.grdLiveFeedData = new PranaUltraGrid();
            this.btnExportFeedData = new Infragistics.Win.Misc.UltraButton();
            this.btnFeedSaveLayout = new Infragistics.Win.Misc.UltraButton();
            this.chkDebugMode = new System.Windows.Forms.CheckBox();
            this.textBxTolerance = new System.Windows.Forms.TextBox();
            
            ((System.ComponentModel.ISupportInitialize)(this.grdAppData)).BeginInit();
            this.toolStripStatusLabel.SuspendLayout();
            this.LiveFeedDataViewer_Fill_Panel.ClientArea.SuspendLayout();
            this.LiveFeedDataViewer_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlFeed)).BeginInit();
            this.tabControlFeed.SuspendLayout();
            this.pageApplicationData.SuspendLayout();
            this.pageLiveFeed.SuspendLayout();
            this.panelLiveFeed.ClientArea.SuspendLayout();
            this.panelLiveFeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLiveFeedData)).BeginInit();
            this.SuspendLayout();
            this.Load += new System.EventHandler(this.LiveFeedDataViewer_Load);
            // 
            // btnFetchLiveFeed
            // 
            this.btnFetchLiveFeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFetchLiveFeed.Location = new System.Drawing.Point(13, 13);
            this.btnFetchLiveFeed.Name = "btnFetchLiveFeed";
            this.btnFetchLiveFeed.Size = new System.Drawing.Size(163, 23);
            this.btnFetchLiveFeed.TabIndex = 0;
            this.btnFetchLiveFeed.Text = "GET Application Price Data";
            this.btnFetchLiveFeed.Click += new System.EventHandler(this.btnFetchLiveFeed_Click);
            // 
            // grdAppData
            // 
            this.grdAppData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdAppData.DisplayLayout.Appearance = appearance1;
            this.grdAppData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdAppData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdAppData.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdAppData.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdAppData.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdAppData.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdAppData.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAppData.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdAppData.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdAppData.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdAppData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdAppData.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdAppData.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdAppData.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdAppData.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdAppData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdAppData.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdAppData.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdAppData.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdAppData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdAppData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdAppData.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdAppData.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdAppData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdAppData.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdData_BeforeColumnChooserDisplayed);
            this.grdAppData.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdData_BeforeCustomRowFilterDialog);
            this.grdAppData.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdAppData.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdAppData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAppData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAppData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAppData.Location = new System.Drawing.Point(0, 42);
            this.grdAppData.Name = "grdAppData";
            this.grdAppData.Size = new System.Drawing.Size(1157, 468);
            this.grdAppData.TabIndex = 1;
            this.grdAppData.Text = "grdLiveFeedData";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripStatusLabel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
            this.toolStripStatusLabel.Location = new System.Drawing.Point(0, 534);
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(1159, 25);
            this.toolStripStatusLabel.TabIndex = 2;
            this.toolStripStatusLabel.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(0, 22);
            // 
            // btnExportData
            // 
            this.btnExportData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportData.Location = new System.Drawing.Point(182, 13);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(122, 23);
            this.btnExportData.TabIndex = 3;
            this.btnExportData.Text = "Export Data";
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            // 
            // btnSaveLayout
            // 
            this.btnSaveLayout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveLayout.Location = new System.Drawing.Point(310, 13);
            this.btnSaveLayout.Name = "btnSaveLayout";
            this.btnSaveLayout.Size = new System.Drawing.Size(122, 23);
            this.btnSaveLayout.TabIndex = 4;
            this.btnSaveLayout.Text = "Save Layout";
            this.btnSaveLayout.Click += new System.EventHandler(this.btnSaveLayout_Click);
            // 
            // LiveFeedDataViewer_Fill_Panel
            // 
            // 
            // LiveFeedDataViewer_Fill_Panel.ClientArea
            // 
            this.LiveFeedDataViewer_Fill_Panel.ClientArea.Controls.Add(this.btnFetchLiveFeed);
            this.LiveFeedDataViewer_Fill_Panel.ClientArea.Controls.Add(this.btnSaveLayout);
            this.LiveFeedDataViewer_Fill_Panel.ClientArea.Controls.Add(this.btnExportData);
            this.LiveFeedDataViewer_Fill_Panel.ClientArea.Controls.Add(this.grdAppData);
            this.LiveFeedDataViewer_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.LiveFeedDataViewer_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LiveFeedDataViewer_Fill_Panel.Location = new System.Drawing.Point(0, 0);
            this.LiveFeedDataViewer_Fill_Panel.Name = "LiveFeedDataViewer_Fill_Panel";
            this.LiveFeedDataViewer_Fill_Panel.Size = new System.Drawing.Size(1157, 513);
            this.LiveFeedDataViewer_Fill_Panel.TabIndex = 3;
            // 
            // btnGetActualFeed
            // 
            this.btnGetActualFeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetActualFeed.Location = new System.Drawing.Point(11, 13);
            this.btnGetActualFeed.Name = "btnGetActualFeed";
            this.btnGetActualFeed.Size = new System.Drawing.Size(143, 23);
            this.btnGetActualFeed.TabIndex = 5;
            this.btnGetActualFeed.Text = "Get API Data";
            this.btnGetActualFeed.Click += new System.EventHandler(this.btnGetActualFeed_Click);
            // 
            // tabControlFeed
            // 
            this.tabControlFeed.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabControlFeed.Controls.Add(this.pageApplicationData);
            this.tabControlFeed.Controls.Add(this.pageLiveFeed);
            this.tabControlFeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlFeed.Location = new System.Drawing.Point(0, 0);
            this.tabControlFeed.Name = "tabControlFeed";
            this.tabControlFeed.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabControlFeed.Size = new System.Drawing.Size(1159, 534);
            this.tabControlFeed.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabControlFeed.TabIndex = 6;
            ultraTab1.Key = "applicationData";
            ultraTab1.TabPage = this.pageApplicationData;
            ultraTab1.Text = "Application Data";
            ultraTab2.Key = "feedData";
            ultraTab2.TabPage = this.pageLiveFeed;
            ultraTab2.Text = "Feed Data";
            this.tabControlFeed.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1157, 513);
            // 
            // pageApplicationData
            // 
            this.pageApplicationData.Controls.Add(this.LiveFeedDataViewer_Fill_Panel);
            this.pageApplicationData.Location = new System.Drawing.Point(-10000, -10000);
            this.pageApplicationData.Name = "pageApplicationData";
            this.pageApplicationData.Size = new System.Drawing.Size(1157, 513);
            // 
            // pageLiveFeed
            // 
            this.pageLiveFeed.Controls.Add(this.panelLiveFeed);
            this.pageLiveFeed.Location = new System.Drawing.Point(1, 20);
            this.pageLiveFeed.Name = "pageLiveFeed";
            this.pageLiveFeed.Size = new System.Drawing.Size(1157, 513);
            // 
            // panelLiveFeed
            // 
            // 
            // panelLiveFeed.ClientArea
            // 
            this.panelLiveFeed.ClientArea.Controls.Add(this.btnGetActualFeed);
            this.panelLiveFeed.ClientArea.Controls.Add(this.btnFeedSaveLayout);
            this.panelLiveFeed.ClientArea.Controls.Add(this.btnExportFeedData);
            this.panelLiveFeed.ClientArea.Controls.Add(this.grdLiveFeedData);
            
            this.panelLiveFeed.ClientArea.Controls.Add(this.chkDebugMode);
            this.panelLiveFeed.ClientArea.Controls.Add(this.textBxTolerance);
            
            this.panelLiveFeed.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelLiveFeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLiveFeed.Location = new System.Drawing.Point(0, 0);
            this.panelLiveFeed.Name = "panelLiveFeed";
            this.panelLiveFeed.Size = new System.Drawing.Size(1157, 513);
            this.panelLiveFeed.TabIndex = 4;
            // 
            // grdLiveFeedData
            // 
            this.grdLiveFeedData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdLiveFeedData.DisplayLayout.Appearance = appearance13;
            this.grdLiveFeedData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdLiveFeedData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.grdLiveFeedData.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdLiveFeedData.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.grdLiveFeedData.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdLiveFeedData.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.grdLiveFeedData.DisplayLayout.MaxColScrollRegions = 1;
            this.grdLiveFeedData.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdLiveFeedData.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdLiveFeedData.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.grdLiveFeedData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdLiveFeedData.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdLiveFeedData.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.grdLiveFeedData.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdLiveFeedData.DisplayLayout.Override.CellAppearance = appearance20;
            this.grdLiveFeedData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdLiveFeedData.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.grdLiveFeedData.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.grdLiveFeedData.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.grdLiveFeedData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdLiveFeedData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.grdLiveFeedData.DisplayLayout.Override.RowAppearance = appearance23;
            this.grdLiveFeedData.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdLiveFeedData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdLiveFeedData.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdData_BeforeColumnChooserDisplayed);
            this.grdLiveFeedData.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdData_BeforeCustomRowFilterDialog);
            this.grdLiveFeedData.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdLiveFeedData.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.grdLiveFeedData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdLiveFeedData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdLiveFeedData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdLiveFeedData.Location = new System.Drawing.Point(0, 42);
            this.grdLiveFeedData.Name = "grdLiveFeedData";
            this.grdLiveFeedData.Size = new System.Drawing.Size(1157, 468);
            this.grdLiveFeedData.TabIndex = 1;
            this.grdLiveFeedData.Text = "ultraGrid1";
            // 
            // btnExportFeedData
            // 
            this.btnExportFeedData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportFeedData.Location = new System.Drawing.Point(173, 13);
            this.btnExportFeedData.Name = "btnExportFeedData";
            this.btnExportFeedData.Size = new System.Drawing.Size(145, 23);
            this.btnExportFeedData.TabIndex = 6;
            this.btnExportFeedData.Text = "Export Feed Data";
            this.btnExportFeedData.Click += new System.EventHandler(this.btnExportFeedData_Click);
            // 
            // btnFeedSaveLayout
            // 
            this.btnFeedSaveLayout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFeedSaveLayout.Location = new System.Drawing.Point(335, 13);
            this.btnFeedSaveLayout.Name = "btnFeedSaveLayout";
            this.btnFeedSaveLayout.Size = new System.Drawing.Size(122, 23);
            this.btnFeedSaveLayout.TabIndex = 7;
            this.btnFeedSaveLayout.Text = "Save Layout";
            this.btnFeedSaveLayout.Click += new System.EventHandler(this.btnFeedSaveLayout_Click);


            this.textBxTolerance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBxTolerance.Location = new System.Drawing.Point(795, 13);
            this.textBxTolerance.Name = "textBxTolerance";
            this.textBxTolerance.Size = new System.Drawing.Size(82, 23);
            this.textBxTolerance.TabIndex = 7;
            this.textBxTolerance.Text = "50";
            
            this.chkDebugMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDebugMode.Location = new System.Drawing.Point(495, 13);
            this.chkDebugMode.Name = "btnEnableDebug";
            this.chkDebugMode.Size = new System.Drawing.Size(298, 23);
            this.chkDebugMode.TabIndex = 7;
            this.chkDebugMode.Text = "Enable Debug mode with price tolerance (times)";
            this.chkDebugMode.CheckedChanged += chkDebugMode_CheckedChanged;
            // 
            // LiveFeedDataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 559);
            this.Controls.Add(this.tabControlFeed);
            this.Controls.Add(this.toolStripStatusLabel);
            this.Name = "LiveFeedDataViewer";
            this.ShowInTaskbar = false;
            this.Text = "Live Feed Data Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.grdAppData)).EndInit();
            this.toolStripStatusLabel.ResumeLayout(false);
            this.toolStripStatusLabel.PerformLayout();
            this.LiveFeedDataViewer_Fill_Panel.ClientArea.ResumeLayout(false);
            this.LiveFeedDataViewer_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlFeed)).EndInit();
            this.tabControlFeed.ResumeLayout(false);
            this.pageApplicationData.ResumeLayout(false);
            this.pageLiveFeed.ResumeLayout(false);
            this.panelLiveFeed.ClientArea.ResumeLayout(false);
            this.panelLiveFeed.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdLiveFeedData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

       

        

        #endregion

        private Infragistics.Win.Misc.UltraButton btnFetchLiveFeed;
        private PranaUltraGrid grdAppData;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter grdExclExporter;
        private System.Windows.Forms.ToolStrip toolStripStatusLabel;
        private Infragistics.Win.Misc.UltraButton btnExportData;
        private Infragistics.Win.Misc.UltraButton btnSaveLayout;
        private Infragistics.Win.Misc.UltraPanel LiveFeedDataViewer_Fill_Panel;
      
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private Infragistics.Win.Misc.UltraButton btnGetActualFeed;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabControlFeed;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl pageApplicationData;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl pageLiveFeed;
        private Infragistics.Win.Misc.UltraPanel panelLiveFeed;
        private PranaUltraGrid grdLiveFeedData;
        private Infragistics.Win.Misc.UltraButton btnExportFeedData;
        private Infragistics.Win.Misc.UltraButton btnFeedSaveLayout;
        private System.Windows.Forms.CheckBox chkDebugMode;
        private System.Windows.Forms.TextBox textBxTolerance;
    }
}