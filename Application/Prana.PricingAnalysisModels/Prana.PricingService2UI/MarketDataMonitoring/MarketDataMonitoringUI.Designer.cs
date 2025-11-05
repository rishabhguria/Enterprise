namespace Prana.PricingService2UI.MarketDataMonitoring
{
    partial class MarketDataMonitoringUI
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
                if(_marketDataProperties != null)
                {
                    _marketDataProperties.Dispose();
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
            this.ultraPanelHeader = new Infragistics.Win.Misc.UltraPanel();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.labelMandatoryFields = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGridSAPISubscription = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraGridSAPISnapshot = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControlSnapshot = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControlSubscription = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraButtonSAPIUpdate = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonSAPIRequestFields = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonUserQuotaPermissions = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonViewSubscribedSymbols = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonConnectionProperties = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanelSubscribedSymbols = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanelSAPITabsContainer = new Infragistics.Win.Misc.UltraPanel();
            this.textBoxUserQuotaAndPermissions = new System.Windows.Forms.TextBox();
            this.ultraGridMarketDataSymbolSubscription = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStripSubscribedSymbols = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshMarketDataSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshAllSymbolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraGridExcelSubscribedSymbolsExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.clearFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraPanelHeader.ClientArea.SuspendLayout();
            this.ultraPanelHeader.SuspendLayout();
            this.ultraPanelSubscribedSymbols.ClientArea.SuspendLayout();
            this.ultraPanelSubscribedSymbols.SuspendLayout();
            this.ultraPanelSAPITabsContainer.ClientArea.SuspendLayout();
            this.ultraPanelSAPITabsContainer.SuspendLayout();
            this.ultraTabPageControlSnapshot.SuspendLayout();
            this.ultraTabPageControlSubscription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridMarketDataSymbolSubscription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridSAPISubscription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridSAPISnapshot)).BeginInit();
            this.contextMenuStripSubscribedSymbols.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabControl1
            // 
            appearance8.FontData.SizeInPoints = 9F;
            this.ultraTabControl1.Appearance = appearance8;
            appearance9.FontData.SizeInPoints = 9F;
            this.ultraTabControl1.ClientAreaAppearance = appearance9;
            appearance10.FontData.SizeInPoints = 9F;
            this.ultraTabControl1.CloseButtonAppearance = appearance10;
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControlSnapshot);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControlSubscription);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl1.Name = "ultraTabControl1";
            appearance11.FontData.SizeInPoints = 9F;
            this.ultraTabControl1.SelectedTabAppearance = appearance11;
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(855, 126);
            this.ultraTabControl1.TabIndex = 0;
            ultraTab1.Key = "Snapshot";
            ultraTab1.TabPage = this.ultraTabPageControlSnapshot;
            ultraTab1.Text = "Snapshot";
            ultraTab2.Key = "Subscription";
            ultraTab2.TabPage = this.ultraTabPageControlSubscription;
            ultraTab2.Text = "Subscription";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2 });
            //
            //labelMandatoryFields
            //
            this.labelMandatoryFields.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMandatoryFields.ForeColor = System.Drawing.Color.Red;
            this.labelMandatoryFields.Location = new System.Drawing.Point(510, 12);
            this.labelMandatoryFields.Name = "labelMandatoryFields";
            this.labelMandatoryFields.Size = new System.Drawing.Size(340, 23);
            this.labelMandatoryFields.Visible = false;
            this.labelMandatoryFields.Text = "Disabled Checked cells are Mandatory fields for the system";
            //
            //ultraTabSharedControlsPage1
            //
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(0, 0);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(850, 121);
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControlSnapshot.Controls.Add(this.ultraGridSAPISnapshot);
            this.ultraTabPageControlSnapshot.Location = new System.Drawing.Point(1, 2);
            this.ultraTabPageControlSnapshot.Name = "ultraTabPageControl1";
            this.ultraTabPageControlSnapshot.Size = new System.Drawing.Size(850, 121);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControlSubscription.Controls.Add(this.ultraGridSAPISubscription);
            this.ultraTabPageControlSubscription.Location = new System.Drawing.Point(1, 2);
            this.ultraTabPageControlSubscription.Name = "ultraTabPageControl1";
            this.ultraTabPageControlSubscription.Size = new System.Drawing.Size(850, 121);
            //
            //ultraGridSAPISubscription
            //
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGridSAPISubscription.DisplayLayout.Appearance = appearance1;
            this.ultraGridSAPISubscription.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraGridSAPISubscription.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridSAPISubscription.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridSAPISubscription.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridSAPISubscription.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ultraGridSAPISubscription.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridSAPISubscription.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ultraGridSAPISubscription.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridSAPISubscription.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGridSAPISubscription.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraGridSAPISubscription.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ultraGridSAPISubscription.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.ultraGridSAPISubscription.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridSAPISubscription.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGridSAPISubscription.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGridSAPISubscription.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridSAPISubscription.DisplayLayout.Override.CellAppearance = appearance8;
            this.ultraGridSAPISubscription.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraGridSAPISubscription.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridSAPISubscription.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.ultraGridSAPISubscription.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.ultraGridSAPISubscription.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.ultraGridSAPISubscription.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.ultraGridSAPISubscription.DisplayLayout.Override.RowAppearance = appearance11;
            this.ultraGridSAPISubscription.DisplayLayout.Override.RowSelectorNumberStyle = Infragistics.Win.UltraWinGrid.RowSelectorNumberStyle.RowIndex;
            this.ultraGridSAPISubscription.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.ultraGridSAPISubscription.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGridSAPISubscription.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.ultraGridSAPISubscription.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridSAPISubscription.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridSAPISubscription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridSAPISubscription.Location = new System.Drawing.Point(0, 0);
            this.ultraGridSAPISubscription.Name = "ultraGridSAPISubscription";
            this.ultraGridSAPISubscription.Size = new System.Drawing.Size(845, 100);
            this.ultraGridSAPISubscription.TabIndex = 0;
            this.ultraGridSAPISubscription.Text = "ultraGrid1";
            this.ultraGridSAPISubscription.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ultraGridSAPISubscription_InitializeRow);
            this.ultraGridSAPISubscription.AfterHeaderCheckStateChanged += UltraGridSAPISubscription_AfterHeaderCheckStateChanged;
            //
            //ultraGridSAPISnapshot
            //
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGridSAPISnapshot.DisplayLayout.Appearance = appearance1;
            this.ultraGridSAPISnapshot.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraGridSAPISnapshot.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridSAPISnapshot.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridSAPISnapshot.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridSAPISnapshot.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ultraGridSAPISnapshot.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridSAPISnapshot.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ultraGridSAPISnapshot.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridSAPISnapshot.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.CellAppearance = appearance8;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.ultraGridSAPISnapshot.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.RowAppearance = appearance11;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.RowSelectorNumberStyle = Infragistics.Win.UltraWinGrid.RowSelectorNumberStyle.RowIndex;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGridSAPISnapshot.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.ultraGridSAPISnapshot.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridSAPISnapshot.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridSAPISnapshot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridSAPISnapshot.Location = new System.Drawing.Point(0, 0);
            this.ultraGridSAPISnapshot.Name = "ultraGridSAPISnapshot";
            this.ultraGridSAPISnapshot.Size = new System.Drawing.Size(845, 100);
            this.ultraGridSAPISnapshot.TabIndex = 0;
            this.ultraGridSAPISnapshot.Text = "ultraGrid2";
            this.ultraGridSAPISnapshot.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ultraGridSAPISnapshot_InitializeRow);
            this.ultraGridSAPISnapshot.AfterHeaderCheckStateChanged += UltraGridSAPISnapshot_AfterHeaderCheckStateChanged;
            // 
            // ultraPanelHeader
            // 
            // 
            // ultraPanelHeader.ClientArea
            // 
            this.ultraPanelHeader.ClientArea.Controls.Add(this.ultraButtonSAPIRequestFields);
            this.ultraPanelHeader.ClientArea.Controls.Add(this.ultraButtonUserQuotaPermissions);
            this.ultraPanelHeader.ClientArea.Controls.Add(this.ultraButtonViewSubscribedSymbols);
            this.ultraPanelHeader.ClientArea.Controls.Add(this.ultraButtonConnectionProperties);
            this.ultraPanelHeader.ClientArea.Controls.Add(this.labelMandatoryFields);
            this.ultraPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanelHeader.Location = new System.Drawing.Point(0, 0);
            this.ultraPanelHeader.Name = "ultraPanelHeader";
            this.ultraPanelHeader.Size = new System.Drawing.Size(858, 47);
            this.ultraPanelHeader.TabIndex = 0;
            this.ultraPanelHeader.Resize += new System.EventHandler(ultraPanelHeader_Resize);
            //
            //ultraButtonSAPIUpgrade
            //
            this.ultraButtonSAPIUpdate.Location = new System.Drawing.Point(780, 0);
            this.ultraButtonSAPIUpdate.Name = "ultraButtonSAPIUpdate";
            this.ultraButtonSAPIUpdate.Size = new System.Drawing.Size(70, 22);
            this.ultraButtonSAPIUpdate.TabIndex = 3;
            this.ultraButtonSAPIUpdate.Text = "Update";
            this.ultraButtonSAPIUpdate.Visible = true;
            this.ultraButtonSAPIUpdate.Click += new System.EventHandler(this.ultraButtonSAPIRequestFieldUpdate_Click);
            // 
            // ultraButtonSAPIRequestFields
            // 
            this.ultraButtonSAPIRequestFields.Location = new System.Drawing.Point(305, 12);
            this.ultraButtonSAPIRequestFields.Name = "ultraButtonSAPIRequestFields";
            this.ultraButtonSAPIRequestFields.Size = new System.Drawing.Size(153, 23);
            this.ultraButtonSAPIRequestFields.TabIndex = 3;
            this.ultraButtonSAPIRequestFields.Text = "SAPI Request Fields";
            this.ultraButtonSAPIRequestFields.Visible = false;
            this.ultraButtonSAPIRequestFields.Click += new System.EventHandler(this.ultraButtonSAPIRequestFields_Click);
            // 
            // ultraButtonUserQuotaPermissions
            // 
            this.ultraButtonUserQuotaPermissions.Location = new System.Drawing.Point(305, 12);
            this.ultraButtonUserQuotaPermissions.Name = "ultraButtonUserQuotaPermissions";
            this.ultraButtonUserQuotaPermissions.Size = new System.Drawing.Size(153, 23);
            this.ultraButtonUserQuotaPermissions.TabIndex = 3;
            this.ultraButtonUserQuotaPermissions.Text = "User Quota && Permissions";
            this.ultraButtonUserQuotaPermissions.Visible = false;
            this.ultraButtonUserQuotaPermissions.Click += new System.EventHandler(this.ultraButtonUserQuotaPermissions_Click);
            // 
            // ultraButtonViewSubscribedSymbols
            // 
            this.ultraButtonViewSubscribedSymbols.Location = new System.Drawing.Point(146, 12);
            this.ultraButtonViewSubscribedSymbols.Name = "ultraButtonViewSubscribedSymbols";
            this.ultraButtonViewSubscribedSymbols.Size = new System.Drawing.Size(153, 23);
            this.ultraButtonViewSubscribedSymbols.TabIndex = 2;
            this.ultraButtonViewSubscribedSymbols.Text = "View Subscribed Symbols";
            this.ultraButtonViewSubscribedSymbols.Click += new System.EventHandler(this.ultraButtonViewSubscribedSymbols_Click);
            // 
            // ultraButtonConnectionProperties
            // 
            this.ultraButtonConnectionProperties.Location = new System.Drawing.Point(13, 12);
            this.ultraButtonConnectionProperties.Name = "ultraButtonConnectionProperties";
            this.ultraButtonConnectionProperties.Size = new System.Drawing.Size(126, 23);
            this.ultraButtonConnectionProperties.TabIndex = 0;
            this.ultraButtonConnectionProperties.Text = "Connection Properties";
            this.ultraButtonConnectionProperties.Click += new System.EventHandler(this.ultraButtonConnectionProperties_Click);
            // 
            // ultraPanelSubscribedSymbols
            // 
            // 
            // ultraPanelSubscribedSymbols.ClientArea
            // 
            this.ultraPanelSubscribedSymbols.ClientArea.Controls.Add(this.textBoxUserQuotaAndPermissions);
            this.ultraPanelSubscribedSymbols.ClientArea.Controls.Add(this.ultraGridMarketDataSymbolSubscription);
            this.ultraPanelSubscribedSymbols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelSubscribedSymbols.Location = new System.Drawing.Point(0, 47);
            this.ultraPanelSubscribedSymbols.Name = "ultraPanelSubscribedSymbols";
            this.ultraPanelSubscribedSymbols.Size = new System.Drawing.Size(858, 204);
            this.ultraPanelSubscribedSymbols.TabIndex = 1;
            //
            // ultraPanelSubscribedSymbols.ClientArea
            // 
            this.ultraPanelSAPITabsContainer.ClientArea.Controls.Add(this.ultraButtonSAPIUpdate); 
            this.ultraPanelSAPITabsContainer.ClientArea.Controls.Add(this.ultraTabControl1);
            this.ultraPanelSAPITabsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelSAPITabsContainer.Location = new System.Drawing.Point(0, 47);
            this.ultraPanelSAPITabsContainer.Name = "ultraPanel2";
            this.ultraPanelSAPITabsContainer.Size = new System.Drawing.Size(858, 204);
            this.ultraPanelSAPITabsContainer.TabIndex = 1;
            this.ultraPanelSAPITabsContainer.Visible = false;
            this.ultraPanelSAPITabsContainer.Resize += new System.EventHandler(ultraPanel2_Resize);
            this.ultraTabPageControlSnapshot.ResumeLayout(false);
            this.ultraTabPageControlSnapshot.PerformLayout();
            this.ultraTabPageControlSubscription.ResumeLayout(false);
            this.ultraTabPageControlSubscription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            // 
            // textBoxUserQuotaAndPermissions
            // 
            this.textBoxUserQuotaAndPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUserQuotaAndPermissions.Location = new System.Drawing.Point(0, 0);
            this.textBoxUserQuotaAndPermissions.Multiline = true;
            this.textBoxUserQuotaAndPermissions.Name = "textBoxUserQuotaAndPermissions";
            this.textBoxUserQuotaAndPermissions.ReadOnly = true;
            this.textBoxUserQuotaAndPermissions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxUserQuotaAndPermissions.Size = new System.Drawing.Size(858, 204);
            this.textBoxUserQuotaAndPermissions.TabIndex = 0;
            this.textBoxUserQuotaAndPermissions.Visible = false;
            // 
            // ultraGridMarketDataSymbolSubscription
            // 
            this.ultraGridMarketDataSymbolSubscription.ContextMenuStrip = this.contextMenuStripSubscribedSymbols;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Appearance = appearance1;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.CellAppearance = appearance8;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.RowAppearance = appearance11;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.RowSelectorNumberStyle = Infragistics.Win.UltraWinGrid.RowSelectorNumberStyle.RowIndex;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridMarketDataSymbolSubscription.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridMarketDataSymbolSubscription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridMarketDataSymbolSubscription.Location = new System.Drawing.Point(0, 0);
            this.ultraGridMarketDataSymbolSubscription.Name = "ultraGridMarketDataSymbolSubscription";
            this.ultraGridMarketDataSymbolSubscription.Size = new System.Drawing.Size(858, 204);
            this.ultraGridMarketDataSymbolSubscription.TabIndex = 0;
            this.ultraGridMarketDataSymbolSubscription.Text = "ultraGrid1";
            this.ultraGridMarketDataSymbolSubscription.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ultraGridMarketDataSymbolSubscription_MouseDown);
            // 
            // contextMenuStripSubscribedSymbols
            // 
            this.contextMenuStripSubscribedSymbols.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshMarketDataSymbolToolStripMenuItem,
            this.refreshAllSymbolsToolStripMenuItem,
            this.copyToClipboardToolStripMenuItem,
            this.exportDataToolStripMenuItem,
            this.clearFiltersToolStripMenuItem});
            this.contextMenuStripSubscribedSymbols.Name = "contextMenuStripSubscribedSymbols";
            this.contextMenuStripSubscribedSymbols.Size = new System.Drawing.Size(204, 136);
            // 
            // refreshMarketDataSymbolToolStripMenuItem
            // 
            this.refreshMarketDataSymbolToolStripMenuItem.Name = "refreshMarketDataSymbolToolStripMenuItem";
            this.refreshMarketDataSymbolToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.refreshMarketDataSymbolToolStripMenuItem.Text = "Refresh Selected Symbol";
            this.refreshMarketDataSymbolToolStripMenuItem.Click += new System.EventHandler(this.refreshMarketDataSymbolToolStripMenuItem_Click);
            // 
            // refreshAllSymbolsToolStripMenuItem
            // 
            this.refreshAllSymbolsToolStripMenuItem.Name = "refreshAllSymbolsToolStripMenuItem";
            this.refreshAllSymbolsToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.refreshAllSymbolsToolStripMenuItem.Text = "Refresh All Symbol(s)";
            this.refreshAllSymbolsToolStripMenuItem.Click += new System.EventHandler(this.refreshAllSymbolsToolStripMenuItem_Click);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.copyToClipboardToolStripMenuItem.Text = "Copy to Clipboard";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // exportDataToolStripMenuItem
            // 
            this.exportDataToolStripMenuItem.Name = "exportDataToolStripMenuItem";
            this.exportDataToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.exportDataToolStripMenuItem.Text = "Export Data";
            this.exportDataToolStripMenuItem.Click += new System.EventHandler(this.exportDataToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 251);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(858, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // clearFiltersToolStripMenuItem
            // 
            this.clearFiltersToolStripMenuItem.Name = "clearFiltersToolStripMenuItem";
            this.clearFiltersToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.clearFiltersToolStripMenuItem.Text = "Clear Filter(s)";
            this.clearFiltersToolStripMenuItem.Click += new System.EventHandler(this.clearFiltersToolStripMenuItem_Click);
            // 
            // MarketDataMonitoringUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 273);
            this.Controls.Add(this.ultraPanelSubscribedSymbols);
            this.Controls.Add(this.ultraPanelSAPITabsContainer);
            this.Controls.Add(this.ultraPanelHeader);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MarketDataMonitoringUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MarketData Monitoring UI";
            this.ultraPanelHeader.ClientArea.ResumeLayout(false);
            this.ultraPanelHeader.ResumeLayout(false);
            this.ultraPanelSubscribedSymbols.ClientArea.ResumeLayout(false);
            this.ultraPanelSubscribedSymbols.ClientArea.PerformLayout();
            this.ultraPanelSubscribedSymbols.ResumeLayout(false);
            this.ultraPanelSAPITabsContainer.ClientArea.ResumeLayout(false);
            this.ultraPanelSAPITabsContainer.ClientArea.PerformLayout();
            this.ultraPanelSAPITabsContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridMarketDataSymbolSubscription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridSAPISubscription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridSAPISnapshot)).EndInit();
            this.contextMenuStripSubscribedSymbols.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanelHeader;
        private Infragistics.Win.Misc.UltraButton ultraButtonConnectionProperties;
        private Infragistics.Win.Misc.UltraPanel ultraPanelSubscribedSymbols;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridMarketDataSymbolSubscription;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSubscribedSymbols;
        private System.Windows.Forms.ToolStripMenuItem refreshMarketDataSymbolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem refreshAllSymbolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportDataToolStripMenuItem;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelSubscribedSymbolsExporter;
        private Infragistics.Win.Misc.UltraButton ultraButtonViewSubscribedSymbols;
        private Infragistics.Win.Misc.UltraButton ultraButtonUserQuotaPermissions;
        private Infragistics.Win.Misc.UltraButton ultraButtonSAPIRequestFields;
        private System.Windows.Forms.TextBox textBoxUserQuotaAndPermissions;
        private System.Windows.Forms.ToolStripMenuItem clearFiltersToolStripMenuItem;
        private Infragistics.Win.Misc.UltraPanel ultraPanelSAPITabsContainer;
        private Infragistics.Win.Misc.UltraButton ultraButtonSAPIUpdate;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControlSubscription;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControlSnapshot;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridSAPISubscription;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridSAPISnapshot;
        private Infragistics.Win.Misc.UltraLabel labelMandatoryFields;
    }
}