namespace Prana.TradeServiceUI
{
    partial class TradeServiceUI
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
            _tradeServiceClientHeartbeatManager.Dispose();

            if (_subscriptionProxy != null)
                _subscriptionProxy.Dispose();

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
            this.ultraPanelMessageGrid = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGridMessages = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.messagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showErrorMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.persistedMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.receivedFromClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sentToBrokerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.receivedFromBrokerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pendingPreTradeComplianceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pendingApprovalTradesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualDropsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshCacheClosingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixTradesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshCacheAllocationPreference = new System.Windows.Forms.ToolStripMenuItem();
            this.clearOrderCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ultraButtonGetMessageStatus = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonMoveOldTrade = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonExcel = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonClearLog = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanelButtons = new System.Windows.Forms.Panel();
            this.ultraButtonLoadLog = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonStopServiceAndUI = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonOpenLog = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonReloadXslt = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonReloadRules = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabelTradeServiceConnectionStatus = new Infragistics.Win.Misc.UltraLabel();
            this.ultraCheckEditorDebugMode = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraButtonDisconnectUser = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabelClientsConnected = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanelFixConnections = new Infragistics.Win.Misc.UltraPanel();
            this.ultraLabelFixConnections = new Infragistics.Win.Misc.UltraLabel();
            this.ultraCheckEditorFixConnectionsAutoReconnect = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dropCopyPostTradeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processAgainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.allowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxClientsConnected = new System.Windows.Forms.ListBox();
            this.listBoxErrorMessages = new System.Windows.Forms.ListBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ultraLabelPricingService2ConnectionStatus = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPopupControlContainerClientServicesStatus = new Infragistics.Win.Misc.UltraPopupControlContainer(this.components);
            this.panelClientsConnected = new Infragistics.Win.Misc.UltraPanel();
            this.pictureBoxLoading = new System.Windows.Forms.PictureBox();
            this.listBoxClientServicesStatus = new System.Windows.Forms.ListBox();
            this.ultraDropDownButtonClientServicesStatus = new Infragistics.Win.Misc.UltraDropDownButton();
            this.ultraPanelMessageGrid.ClientArea.SuspendLayout();
            this.ultraPanelMessageGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridMessages)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.ultraPanelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditorDebugMode)).BeginInit();
            this.ultraPanelFixConnections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditorFixConnectionsAutoReconnect)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            this.panelClientsConnected.ClientArea.SuspendLayout();
            this.panelClientsConnected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanelMessageGrid
            // 
            // 
            // ultraPanelMessageGrid.ClientArea
            // 
            this.ultraPanelMessageGrid.ClientArea.Controls.Add(this.ultraGridMessages);
            this.ultraPanelMessageGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanelMessageGrid.Location = new System.Drawing.Point(0, 255);
            this.ultraPanelMessageGrid.Name = "ultraPanelMessageGrid";
            this.ultraPanelMessageGrid.Size = new System.Drawing.Size(1158, 201);
            this.ultraPanelMessageGrid.TabIndex = 12;
            // 
            // ultraGridMessages
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGridMessages.DisplayLayout.Appearance = appearance1;
            this.ultraGridMessages.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridMessages.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridMessages.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridMessages.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ultraGridMessages.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridMessages.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ultraGridMessages.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridMessages.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGridMessages.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraGridMessages.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ultraGridMessages.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGridMessages.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGridMessages.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridMessages.DisplayLayout.Override.CellAppearance = appearance8;
            this.ultraGridMessages.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.ultraGridMessages.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridMessages.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.ultraGridMessages.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.ultraGridMessages.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGridMessages.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.ultraGridMessages.DisplayLayout.Override.RowAppearance = appearance11;
            this.ultraGridMessages.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGridMessages.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.ultraGridMessages.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridMessages.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridMessages.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.ultraGridMessages.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraGridMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridMessages.Enabled = false;
            this.ultraGridMessages.Location = new System.Drawing.Point(0, 0);
            this.ultraGridMessages.Name = "ultraGridMessages";
            this.ultraGridMessages.Size = new System.Drawing.Size(1158, 201);
            this.ultraGridMessages.TabIndex = 0;
            this.ultraGridMessages.Text = "ultraGrid1";
            this.ultraGridMessages.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ultraGridMessages_InitializeRow);
            this.ultraGridMessages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ultraGridMessages_MouseDown);
            // 
            // messagesToolStripMenuItem
            // 
            this.messagesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMessagesToolStripMenuItem,
            this.showErrorMessagesToolStripMenuItem,
            this.persistedMessagesToolStripMenuItem,
            this.pendingPreTradeComplianceToolStripMenuItem,
            this.pendingApprovalTradesToolStripMenuItem});
            this.messagesToolStripMenuItem.Name = "messagesToolStripMenuItem";
            this.messagesToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.messagesToolStripMenuItem.Text = "Messages";
            // 
            // showMessagesToolStripMenuItem
            // 
            this.showMessagesToolStripMenuItem.Name = "showMessagesToolStripMenuItem";
            this.showMessagesToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.showMessagesToolStripMenuItem.Text = "Show Messages";
            // 
            // showErrorMessagesToolStripMenuItem
            // 
            this.showErrorMessagesToolStripMenuItem.Name = "showErrorMessagesToolStripMenuItem";
            this.showErrorMessagesToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.showErrorMessagesToolStripMenuItem.Text = "Show Error Messages";
            // 
            // persistedMessagesToolStripMenuItem
            // 
            this.persistedMessagesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.receivedFromClientToolStripMenuItem,
            this.sentToBrokerToolStripMenuItem,
            this.receivedFromBrokerToolStripMenuItem});
            this.persistedMessagesToolStripMenuItem.Name = "persistedMessagesToolStripMenuItem";
            this.persistedMessagesToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.persistedMessagesToolStripMenuItem.Text = "Persisted Messages";
            // 
            // receivedFromClientToolStripMenuItem
            // 
            this.receivedFromClientToolStripMenuItem.Name = "receivedFromClientToolStripMenuItem";
            this.receivedFromClientToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.receivedFromClientToolStripMenuItem.Text = "Received From Client";
            this.receivedFromClientToolStripMenuItem.Click += new System.EventHandler(this.receivedFromClientToolStripMenuItem_Click);
            // 
            // sentToBrokerToolStripMenuItem
            // 
            this.sentToBrokerToolStripMenuItem.Name = "sentToBrokerToolStripMenuItem";
            this.sentToBrokerToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.sentToBrokerToolStripMenuItem.Text = "Sent To Broker";
            this.sentToBrokerToolStripMenuItem.Click += new System.EventHandler(this.sentToBrokerToolStripMenuItem_Click);
            // 
            // receivedFromBrokerToolStripMenuItem
            // 
            this.receivedFromBrokerToolStripMenuItem.Name = "receivedFromBrokerToolStripMenuItem";
            this.receivedFromBrokerToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.receivedFromBrokerToolStripMenuItem.Text = "Received From Broker";
            this.receivedFromBrokerToolStripMenuItem.Click += new System.EventHandler(this.receivedFromBrokerToolStripMenuItem_Click);
            // 
            // pendingPreTradeComplianceToolStripMenuItem
            // 
            this.pendingPreTradeComplianceToolStripMenuItem.Name = "pendingPreTradeComplianceToolStripMenuItem";
            this.pendingPreTradeComplianceToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.pendingPreTradeComplianceToolStripMenuItem.Text = "Pending Pre Trade Compliance";
            this.pendingPreTradeComplianceToolStripMenuItem.Click += new System.EventHandler(this.pendingPreTradeComplianceToolStripMenuItem_Click);
            // 
            // pendingApprovalTradesToolStripMenuItem
            // 
            this.pendingApprovalTradesToolStripMenuItem.Name = "pendingApprovalTradesToolStripMenuItem";
            this.pendingApprovalTradesToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.pendingApprovalTradesToolStripMenuItem.Text = "Pending Approval Trades";
            this.pendingApprovalTradesToolStripMenuItem.Click += new System.EventHandler(this.pendingApprovalTradesToolStripMenuItem_Click);
            // 
            // refreshCacheToolStripMenuItem
            // 
            this.refreshCacheToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshCacheClosingToolStripMenuItem,
            this.fixTradesToolStripMenuItem,
            this.refreshCacheAllocationPreference});
            this.refreshCacheToolStripMenuItem.Name = "refreshCacheToolStripMenuItem";
            this.refreshCacheToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.refreshCacheToolStripMenuItem.Text = "Refresh Cache";
            // 
            // refreshCacheToolStripMenuItem
            // 
            this.manualDropsToolStripMenuItem.Name = "manualDropsToolStripMenuItem";
            this.manualDropsToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.manualDropsToolStripMenuItem.Text = "Manual Drops";
            this.manualDropsToolStripMenuItem.Click += manualDropsToolStripMenuItem_Click;
            // 
            // refreshCacheClosingToolStripMenuItem
            // 
            this.refreshCacheClosingToolStripMenuItem.Name = "refreshCacheClosingToolStripMenuItem";
            this.refreshCacheClosingToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.refreshCacheClosingToolStripMenuItem.Text = "Closing";
            this.refreshCacheClosingToolStripMenuItem.Click += new System.EventHandler(this.refreshCacheClosingToolStripMenuItem_Click);
            // 
            // refreshCacheAllocationPreference
            // 
            this.refreshCacheAllocationPreference.Name = "refreshCacheAllocationPreference";
            this.refreshCacheAllocationPreference.Size = new System.Drawing.Size(125, 22);
            this.refreshCacheAllocationPreference.Text = "Allocation Preference";
            this.refreshCacheAllocationPreference.Click += new System.EventHandler(this.refreshCacheAllocationPreferenceToolStripMenuItem_Click);
            // 
            // fixTradesToolStripMenuItem
            // 
            this.fixTradesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearOrderCacheToolStripMenuItem,
            this.clearOrderToolStripMenuItem});
            this.fixTradesToolStripMenuItem.Name = "fixTradesToolStripMenuItem";
            this.fixTradesToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.fixTradesToolStripMenuItem.Text = "Fix Trades";
            // 
            // clearOrderCacheToolStripMenuItem
            // 
            this.clearOrderCacheToolStripMenuItem.Name = "clearOrderCacheToolStripMenuItem";
            this.clearOrderCacheToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.clearOrderCacheToolStripMenuItem.Text = "Clear Order Cache";
            this.clearOrderCacheToolStripMenuItem.Click += new System.EventHandler(this.clearOrderCacheToolStripMenuItem_Click);
            // 
            // clearOrderToolStripMenuItem
            // 
            this.clearOrderToolStripMenuItem.Name = "clearOrderToolStripMenuItem";
            this.clearOrderToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.clearOrderToolStripMenuItem.Text = "Clear Order";
            this.clearOrderToolStripMenuItem.Click += new System.EventHandler(this.clearOrderToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Enabled = false;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.messagesToolStripMenuItem,
            this.refreshCacheToolStripMenuItem,
            this.manualDropsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1158, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ultraButtonGetMessageStatus
            // 
            this.ultraButtonGetMessageStatus.Enabled = false;
            this.ultraButtonGetMessageStatus.Location = new System.Drawing.Point(590, 7);
            this.ultraButtonGetMessageStatus.Name = "ultraButtonGetMessageStatus";
            this.ultraButtonGetMessageStatus.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonGetMessageStatus.TabIndex = 6;
            this.ultraButtonGetMessageStatus.Text = "Get Msg Status";
            this.ultraButtonGetMessageStatus.Click += new System.EventHandler(this.ultraButtonGetMessageStatus_Click);
            // 
            // ultraButtonMoveOldTrade
            // 
            this.ultraButtonMoveOldTrade.Enabled = false;
            this.ultraButtonMoveOldTrade.Location = new System.Drawing.Point(706, 7);
            this.ultraButtonMoveOldTrade.Name = "ultraButtonMoveOldTrade";
            this.ultraButtonMoveOldTrade.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonMoveOldTrade.TabIndex = 7;
            this.ultraButtonMoveOldTrade.Text = "Move Old Trade";
            this.ultraButtonMoveOldTrade.Click += new System.EventHandler(this.ultraButtonMoveOldTrade_Click);
            // 
            // ultraButtonExcel
            // 
            this.ultraButtonExcel.Enabled = false;
            this.ultraButtonExcel.Location = new System.Drawing.Point(822, 7);
            this.ultraButtonExcel.Name = "ultraButtonExcel";
            this.ultraButtonExcel.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonExcel.TabIndex = 8;
            this.ultraButtonExcel.Text = "Excel";
            this.ultraButtonExcel.Click += new System.EventHandler(this.ultraButtonExcel_Click);
            // 
            // ultraButtonClearLog
            // 
            this.ultraButtonClearLog.Enabled = false;
            this.ultraButtonClearLog.Location = new System.Drawing.Point(242, 7);
            this.ultraButtonClearLog.Name = "ultraButtonClearLog";
            this.ultraButtonClearLog.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonClearLog.TabIndex = 2;
            this.ultraButtonClearLog.Text = "Clear Log";
            this.ultraButtonClearLog.Click += new System.EventHandler(this.ultraButtonClearLog_Click);
            // 
            // ultraPanelButtons
            // 
            this.ultraPanelButtons.Controls.Add(this.ultraButtonLoadLog);
            this.ultraPanelButtons.Controls.Add(this.ultraButtonStopServiceAndUI);
            this.ultraPanelButtons.Controls.Add(this.ultraButtonOpenLog);
            this.ultraPanelButtons.Controls.Add(this.ultraButtonClearLog);
            this.ultraPanelButtons.Controls.Add(this.ultraButtonExcel);
            this.ultraPanelButtons.Controls.Add(this.ultraButtonMoveOldTrade);
            this.ultraPanelButtons.Controls.Add(this.ultraButtonGetMessageStatus);
            this.ultraPanelButtons.Controls.Add(this.ultraButtonReloadXslt);
            this.ultraPanelButtons.Controls.Add(this.ultraButtonReloadRules);
            this.ultraPanelButtons.Cursor = System.Windows.Forms.Cursors.Default;
            this.ultraPanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanelButtons.Location = new System.Drawing.Point(0, 456);
            this.ultraPanelButtons.Name = "ultraPanelButtons";
            this.ultraPanelButtons.Size = new System.Drawing.Size(1158, 37);
            this.ultraPanelButtons.TabIndex = 0;
            // 
            // ultraButtonLoadLog
            // 
            this.ultraButtonLoadLog.Enabled = false;
            this.ultraButtonLoadLog.Location = new System.Drawing.Point(126, 7);
            this.ultraButtonLoadLog.Name = "ultraButtonLoadLog";
            this.ultraButtonLoadLog.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonLoadLog.TabIndex = 14;
            this.ultraButtonLoadLog.Text = "Load Log";
            this.ultraButtonLoadLog.Click += new System.EventHandler(this.ultraButtonLoadLog_Click);
            // 
            // ultraButtonStopServiceAndUI
            // 
            this.ultraButtonStopServiceAndUI.Enabled = false;
            this.ultraButtonStopServiceAndUI.Location = new System.Drawing.Point(938, 7);
            this.ultraButtonStopServiceAndUI.Name = "ultraButtonStopServiceAndUI";
            this.ultraButtonStopServiceAndUI.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonStopServiceAndUI.TabIndex = 9;
            this.ultraButtonStopServiceAndUI.Text = "Close Service && UI";
            this.ultraButtonStopServiceAndUI.Click += new System.EventHandler(this.ultraButtonStopServiceAndUI_Click);
            // 
            // ultraButtonOpenLog
            // 
            this.ultraButtonOpenLog.Enabled = false;
            this.ultraButtonOpenLog.Location = new System.Drawing.Point(10, 7);
            this.ultraButtonOpenLog.Name = "ultraButtonOpenLog";
            this.ultraButtonOpenLog.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonOpenLog.TabIndex = 1;
            this.ultraButtonOpenLog.Text = "Open Log";
            this.ultraButtonOpenLog.Click += new System.EventHandler(this.ultraButtonOpenLog_Click);
            // 
            // ultraButtonReloadXslt
            // 
            this.ultraButtonReloadXslt.Enabled = false;
            this.ultraButtonReloadXslt.Location = new System.Drawing.Point(474, 7);
            this.ultraButtonReloadXslt.Name = "ultraButtonReloadXslt";
            this.ultraButtonReloadXslt.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonReloadXslt.TabIndex = 4;
            this.ultraButtonReloadXslt.Text = "Reload Xslt";
            this.ultraButtonReloadXslt.Click += new System.EventHandler(this.ultraButtonReloadXslt_Click);
            this.ultraButtonReloadXslt.MouseHover += new System.EventHandler(this.ultraButtonReloadXslt_MouseHover);
            // 
            // ultraButtonReloadRules
            // 
            this.ultraButtonReloadRules.Enabled = false;
            this.ultraButtonReloadRules.Location = new System.Drawing.Point(358, 7);
            this.ultraButtonReloadRules.Name = "ultraButtonReloadRules";
            this.ultraButtonReloadRules.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonReloadRules.TabIndex = 3;
            this.ultraButtonReloadRules.Text = "Reload Rules";
            this.ultraButtonReloadRules.Click += new System.EventHandler(this.ultraButtonReloadRules_Click);
            // 
            // ultraLabelTradeServiceConnectionStatus
            // 
            this.ultraLabelTradeServiceConnectionStatus.Location = new System.Drawing.Point(312, 37);
            this.ultraLabelTradeServiceConnectionStatus.Name = "ultraLabelTradeServiceConnectionStatus";
            this.ultraLabelTradeServiceConnectionStatus.Size = new System.Drawing.Size(294, 13);
            this.ultraLabelTradeServiceConnectionStatus.TabIndex = 5;
            this.ultraLabelTradeServiceConnectionStatus.Text = "TradeService Disconnected";
            // 
            // ultraCheckEditorDebugMode
            // 
            this.ultraCheckEditorDebugMode.Enabled = false;
            this.ultraCheckEditorDebugMode.Location = new System.Drawing.Point(961, 236);
            this.ultraCheckEditorDebugMode.Name = "ultraCheckEditorDebugMode";
            this.ultraCheckEditorDebugMode.Size = new System.Drawing.Size(86, 13);
            this.ultraCheckEditorDebugMode.TabIndex = 10;
            this.ultraCheckEditorDebugMode.Text = "Debug Mode";
            this.ultraCheckEditorDebugMode.CheckedChanged += new System.EventHandler(this.ultraCheckEditorDebugMode_CheckedChanged);
            // 
            // ultraButtonDisconnectUser
            // 
            this.ultraButtonDisconnectUser.Enabled = false;
            this.ultraButtonDisconnectUser.Location = new System.Drawing.Point(1053, 230);
            this.ultraButtonDisconnectUser.Name = "ultraButtonDisconnectUser";
            this.ultraButtonDisconnectUser.Size = new System.Drawing.Size(98, 23);
            this.ultraButtonDisconnectUser.TabIndex = 11;
            this.ultraButtonDisconnectUser.Text = "Disconnect User";
            this.ultraButtonDisconnectUser.Click += new System.EventHandler(this.ultraButtonDisconnectUser_Click);
            // 
            // ultraLabelClientsConnected
            // 
            this.ultraLabelClientsConnected.Location = new System.Drawing.Point(961, 91);
            this.ultraLabelClientsConnected.Name = "ultraLabelClientsConnected";
            this.ultraLabelClientsConnected.Size = new System.Drawing.Size(182, 13);
            this.ultraLabelClientsConnected.TabIndex = 8;
            this.ultraLabelClientsConnected.Text = "Clients Connected to TradeService";
            // 
            // ultraPanelFixConnections
            // 
            this.ultraPanelFixConnections.AutoScroll = true;
            this.ultraPanelFixConnections.Enabled = false;
            this.ultraPanelFixConnections.Location = new System.Drawing.Point(0, 56);
            this.ultraPanelFixConnections.Name = "ultraPanelFixConnections";
            this.ultraPanelFixConnections.Size = new System.Drawing.Size(294, 199);
            this.ultraPanelFixConnections.TabIndex = 4;
            // 
            // ultraLabelFixConnections
            // 
            this.ultraLabelFixConnections.Location = new System.Drawing.Point(0, 37);
            this.ultraLabelFixConnections.Name = "ultraLabelFixConnections";
            this.ultraLabelFixConnections.Size = new System.Drawing.Size(93, 13);
            this.ultraLabelFixConnections.TabIndex = 2;
            this.ultraLabelFixConnections.Text = "Fix Connections";
            // 
            // ultraCheckEditorFixConnectionsAutoReconnect
            // 
            this.ultraCheckEditorFixConnectionsAutoReconnect.Enabled = false;
            this.ultraCheckEditorFixConnectionsAutoReconnect.Location = new System.Drawing.Point(192, 37);
            this.ultraCheckEditorFixConnectionsAutoReconnect.Name = "ultraCheckEditorFixConnectionsAutoReconnect";
            this.ultraCheckEditorFixConnectionsAutoReconnect.Size = new System.Drawing.Size(102, 13);
            this.ultraCheckEditorFixConnectionsAutoReconnect.TabIndex = 3;
            this.ultraCheckEditorFixConnectionsAutoReconnect.Text = "Auto Reconnect";
            this.ultraCheckEditorFixConnectionsAutoReconnect.CheckedChanged += new System.EventHandler(this.ultraCheckEditorFixConnectionsAutoReconnect_CheckedChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshDataToolStripMenuItem,
            this.saveLayoutToolStripMenuItem,
            this.processAgainToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 70);
            // 
            // refreshDataToolStripMenuItem
            // 
            this.refreshDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dropCopyPostTradeToolStripMenuItem});
            this.refreshDataToolStripMenuItem.Name = "refreshDataToolStripMenuItem";
            this.refreshDataToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.refreshDataToolStripMenuItem.Text = "Refresh Data";
            // 
            // dropCopyPostTradeToolStripMenuItem
            // 
            this.dropCopyPostTradeToolStripMenuItem.Name = "dropCopyPostTradeToolStripMenuItem";
            this.dropCopyPostTradeToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.dropCopyPostTradeToolStripMenuItem.Text = "DropCopy_PostTrade";
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // processAgainToolStripMenuItem
            // 
            this.processAgainToolStripMenuItem.Name = "processAgainToolStripMenuItem";
            this.processAgainToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.processAgainToolStripMenuItem.Text = "Process Again";
            this.processAgainToolStripMenuItem.Click += new System.EventHandler(this.processAgainToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allowToolStripMenuItem,
            this.blockToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(105, 48);
            // 
            // allowToolStripMenuItem
            // 
            this.allowToolStripMenuItem.Name = "allowToolStripMenuItem";
            this.allowToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
            this.allowToolStripMenuItem.Text = "Allow";
            this.allowToolStripMenuItem.Click += new System.EventHandler(this.allowToolStripMenuItem_Click);
            // 
            // blockToolStripMenuItem
            // 
            this.blockToolStripMenuItem.Name = "blockToolStripMenuItem";
            this.blockToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
            this.blockToolStripMenuItem.Text = "Block";
            this.blockToolStripMenuItem.Click += new System.EventHandler(this.blockToolStripMenuItem_Click);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(103, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // listBoxClientsConnected
            // 
            this.listBoxClientsConnected.FormattingEnabled = true;
            this.listBoxClientsConnected.Location = new System.Drawing.Point(961, 108);
            this.listBoxClientsConnected.Name = "listBoxClientsConnected";
            this.listBoxClientsConnected.Size = new System.Drawing.Size(190, 121);
            this.listBoxClientsConnected.TabIndex = 9;
            // 
            // listBoxErrorMessages
            // 
            this.listBoxErrorMessages.FormattingEnabled = true;
            this.listBoxErrorMessages.HorizontalScrollbar = true;
            this.listBoxErrorMessages.Location = new System.Drawing.Point(300, 56);
            this.listBoxErrorMessages.Name = "listBoxErrorMessages";
            this.listBoxErrorMessages.Size = new System.Drawing.Size(654, 199);
            this.listBoxErrorMessages.TabIndex = 7;
            // 
            // ultraLabelPricingService2ConnectionStatus
            // 
            this.ultraLabelPricingService2ConnectionStatus.Location = new System.Drawing.Point(702, 37);
            this.ultraLabelPricingService2ConnectionStatus.Name = "ultraLabelPricingService2ConnectionStatus";
            this.ultraLabelPricingService2ConnectionStatus.Size = new System.Drawing.Size(187, 13);
            this.ultraLabelPricingService2ConnectionStatus.TabIndex = 6;
            this.ultraLabelPricingService2ConnectionStatus.Text = "PricingService2 Disconnected";
            // 
            // ultraPopupControlContainerClientServicesStatus
            // 
            this.ultraPopupControlContainerClientServicesStatus.PopupControl = this.panelClientsConnected;
            this.ultraPopupControlContainerClientServicesStatus.Opened += new System.EventHandler(this.ultraPopupControlContainerClientServicesStatus_Opened);
            // 
            // panelClientsConnected
            // 
            this.panelClientsConnected.BackColorInternal = System.Drawing.Color.Transparent;
            // 
            // panelClientsConnected.ClientArea
            // 
            this.panelClientsConnected.ClientArea.Controls.Add(this.pictureBoxLoading);
            this.panelClientsConnected.ClientArea.Controls.Add(this.listBoxClientServicesStatus);
            this.panelClientsConnected.Location = new System.Drawing.Point(961, 82);
            this.panelClientsConnected.Name = "panelClientsConnected";
            this.panelClientsConnected.Size = new System.Drawing.Size(190, 173);
            this.panelClientsConnected.TabIndex = 17;
            this.panelClientsConnected.Visible = false;
            // 
            // pictureBoxLoading
            // 
            this.pictureBoxLoading.BackColor = System.Drawing.Color.White;
            this.pictureBoxLoading.Location = new System.Drawing.Point(59, 52);
            this.pictureBoxLoading.Name = "pictureBoxLoading";
            this.pictureBoxLoading.Size = new System.Drawing.Size(75, 75);
            this.pictureBoxLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLoading.TabIndex = 15;
            this.pictureBoxLoading.TabStop = false;
            this.pictureBoxLoading.Visible = false;
            // 
            // listBoxClientServicesStatus
            // 
            this.listBoxClientServicesStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxClientServicesStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxClientServicesStatus.ItemHeight = 14;
            this.listBoxClientServicesStatus.Location = new System.Drawing.Point(0, 0);
            this.listBoxClientServicesStatus.Name = "listBoxClientServicesStatus";
            this.listBoxClientServicesStatus.Size = new System.Drawing.Size(190, 173);
            this.listBoxClientServicesStatus.TabIndex = 14;
            this.listBoxClientServicesStatus.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxClientServicesStatus_DrawItem);
            // 
            // ultraDropDownButtonClientServicesStatus
            // 
            this.ultraDropDownButtonClientServicesStatus.Location = new System.Drawing.Point(961, 56);
            this.ultraDropDownButtonClientServicesStatus.Name = "ultraDropDownButtonClientServicesStatus";
            this.ultraDropDownButtonClientServicesStatus.PopupItemKey = "listBoxClientServicesStatus";
            this.ultraDropDownButtonClientServicesStatus.PopupItemProvider = this.ultraPopupControlContainerClientServicesStatus;
            this.ultraDropDownButtonClientServicesStatus.Size = new System.Drawing.Size(190, 24);
            this.ultraDropDownButtonClientServicesStatus.Style = Infragistics.Win.Misc.SplitButtonDisplayStyle.DropDownButtonOnly;
            this.ultraDropDownButtonClientServicesStatus.TabIndex = 15;
            this.ultraDropDownButtonClientServicesStatus.Text = "Connected Services Status";
            // 
            // TradeServiceUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 493);
            this.Controls.Add(this.panelClientsConnected);
            this.Controls.Add(this.ultraDropDownButtonClientServicesStatus);
            this.Controls.Add(this.ultraLabelPricingService2ConnectionStatus);
            this.Controls.Add(this.listBoxErrorMessages);
            this.Controls.Add(this.listBoxClientsConnected);
            this.Controls.Add(this.ultraCheckEditorFixConnectionsAutoReconnect);
            this.Controls.Add(this.ultraLabelFixConnections);
            this.Controls.Add(this.ultraLabelClientsConnected);
            this.Controls.Add(this.ultraButtonDisconnectUser);
            this.Controls.Add(this.ultraCheckEditorDebugMode);
            this.Controls.Add(this.ultraPanelFixConnections);
            this.Controls.Add(this.ultraPanelMessageGrid);
            this.Controls.Add(this.ultraPanelButtons);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.ultraLabelTradeServiceConnectionStatus);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1174, 532);
            this.MinimumSize = new System.Drawing.Size(1174, 532);
            this.Name = "TradeServiceUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prana TradeService UI";
            this.Load += new System.EventHandler(this.TradeServiceUI_Load);
            this.ultraPanelMessageGrid.ClientArea.ResumeLayout(false);
            this.ultraPanelMessageGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridMessages)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ultraPanelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditorDebugMode)).EndInit();
            this.ultraPanelFixConnections.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditorFixConnectionsAutoReconnect)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            this.panelClientsConnected.ClientArea.ResumeLayout(false);
            this.panelClientsConnected.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanelMessageGrid;
        private System.Windows.Forms.ToolStripMenuItem messagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMessagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showErrorMessagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem persistedMessagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem receivedFromClientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sentToBrokerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem receivedFromBrokerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pendingPreTradeComplianceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pendingApprovalTradesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshCacheToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualDropsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshCacheClosingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixTradesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshCacheAllocationPreference;
        private System.Windows.Forms.ToolStripMenuItem clearOrderCacheToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearOrderToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private Infragistics.Win.Misc.UltraButton ultraButtonGetMessageStatus;
        private Infragistics.Win.Misc.UltraButton ultraButtonMoveOldTrade;
        private Infragistics.Win.Misc.UltraButton ultraButtonExcel;
        private Infragistics.Win.Misc.UltraButton ultraButtonClearLog;
        private System.Windows.Forms.Panel ultraPanelButtons;
        private Infragistics.Win.Misc.UltraButton ultraButtonReloadXslt;
        private Infragistics.Win.Misc.UltraButton ultraButtonReloadRules;
        private Infragistics.Win.Misc.UltraLabel ultraLabelTradeServiceConnectionStatus;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraCheckEditorDebugMode;
        private Infragistics.Win.Misc.UltraButton ultraButtonDisconnectUser;
        private Infragistics.Win.Misc.UltraLabel ultraLabelClientsConnected;
        private Infragistics.Win.Misc.UltraPanel ultraPanelFixConnections;
        private Infragistics.Win.Misc.UltraLabel ultraLabelFixConnections;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraCheckEditorFixConnectionsAutoReconnect;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridMessages;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dropCopyPostTradeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processAgainToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem allowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blockToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ListBox listBoxClientsConnected;
        private System.Windows.Forms.ListBox listBoxErrorMessages;
        private System.Windows.Forms.ToolTip toolTip1;
        private Infragistics.Win.Misc.UltraLabel ultraLabelPricingService2ConnectionStatus;
        private Infragistics.Win.Misc.UltraButton ultraButtonOpenLog;
        private Infragistics.Win.Misc.UltraButton ultraButtonStopServiceAndUI;
        private Infragistics.Win.Misc.UltraButton ultraButtonLoadLog;
        private Infragistics.Win.Misc.UltraPopupControlContainer ultraPopupControlContainerClientServicesStatus;
        private Infragistics.Win.Misc.UltraDropDownButton ultraDropDownButtonClientServicesStatus;
        private Infragistics.Win.Misc.UltraPanel panelClientsConnected;
        private System.Windows.Forms.PictureBox pictureBoxLoading;
        private System.Windows.Forms.ListBox listBoxClientServicesStatus;

    }
}

