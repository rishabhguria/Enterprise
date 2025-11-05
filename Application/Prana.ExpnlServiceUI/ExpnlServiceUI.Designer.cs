namespace Prana.ExpnlServiceUI
{
    partial class ExpnlServiceUI
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
            _expnlServiceClientHeartbeatManager.Dispose();

            if (_subscriptionProxy != null)
                _subscriptionProxy.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing && (_clearanceForm != null))
            {
                _clearanceForm.Dispose();
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
            this.ultraButtonOpenLog = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonClearLog = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonClearanceSetup = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonUpdateRefreshTimeInterval = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabelPricingService = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelTradeService = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelLivefeed = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelMessage = new Infragistics.Win.Misc.UltraLabel();
            this.listBoxErrorMessages = new System.Windows.Forms.ListBox();
            this.listBoxClientsConnected = new System.Windows.Forms.ListBox();
            this.ultraButtonRefreshData = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonDataDumper = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonDisconnectUser = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabelClientsConnected = new Infragistics.Win.Misc.UltraLabel();
            this.numericUpDownRefreshInterval = new System.Windows.Forms.NumericUpDown();
            this.ultraLabelColon1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelColon2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelColon4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelPricingServiceConnectionStatus = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelTradeServiceConnectionStatus = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelLivefeedStatus = new Infragistics.Win.Misc.UltraLabel();
            this.ultraCheckEditorDebugMode = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraLabelExpnlServiceConnectionStatus = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelColon3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelExpnlService = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButtonStopServiceAndUI = new Infragistics.Win.Misc.UltraButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraLabelCompressionName = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButtonLoadLog = new Infragistics.Win.Misc.UltraButton();
            this.ultraPopupControlContainerClientServicesStatus = new Infragistics.Win.Misc.UltraPopupControlContainer(this.components);
            this.panelClientsConnected = new Infragistics.Win.Misc.UltraPanel();
            this.pictureBoxLoading = new System.Windows.Forms.PictureBox();
            this.listBoxClientServicesStatus = new System.Windows.Forms.ListBox();
            this.ultraDropDownButtonClientServicesStatus = new Infragistics.Win.Misc.UltraDropDownButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRefreshInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditorDebugMode)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panelClientsConnected.ClientArea.SuspendLayout();
            this.panelClientsConnected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraButtonOpenLog
            // 
            this.ultraButtonOpenLog.Enabled = false;
            this.ultraButtonOpenLog.Location = new System.Drawing.Point(9, 9);
            this.ultraButtonOpenLog.Name = "ultraButtonOpenLog";
            this.ultraButtonOpenLog.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonOpenLog.TabIndex = 1;
            this.ultraButtonOpenLog.Text = "Open Log";
            this.ultraButtonOpenLog.Click += new System.EventHandler(this.ultraButtonOpenLog_Click);
            // 
            // ultraButtonClearLog
            // 
            this.ultraButtonClearLog.Enabled = false;
            this.ultraButtonClearLog.Location = new System.Drawing.Point(241, 9);
            this.ultraButtonClearLog.Name = "ultraButtonClearLog";
            this.ultraButtonClearLog.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonClearLog.TabIndex = 2;
            this.ultraButtonClearLog.Text = "Clear Log";
            this.ultraButtonClearLog.Click += new System.EventHandler(this.ultraButtonClearLog_Click);
            // 
            // ultraButtonClearanceSetup
            // 
            this.ultraButtonClearanceSetup.Enabled = false;
            this.ultraButtonClearanceSetup.Location = new System.Drawing.Point(357, 9);
            this.ultraButtonClearanceSetup.Name = "ultraButtonClearanceSetup";
            this.ultraButtonClearanceSetup.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonClearanceSetup.TabIndex = 3;
            this.ultraButtonClearanceSetup.Text = "Clearance Setup";
            this.ultraButtonClearanceSetup.Click += new System.EventHandler(this.ultraButtonClearanceSetup_Click);
            // 
            // ultraButtonUpdateRefreshTimeInterval
            // 
            this.ultraButtonUpdateRefreshTimeInterval.Enabled = false;
            this.ultraButtonUpdateRefreshTimeInterval.Location = new System.Drawing.Point(77, 46);
            this.ultraButtonUpdateRefreshTimeInterval.Name = "ultraButtonUpdateInterval";
            this.ultraButtonUpdateRefreshTimeInterval.Size = new System.Drawing.Size(163, 23);
            this.ultraButtonUpdateRefreshTimeInterval.TabIndex = 10;
            this.ultraButtonUpdateRefreshTimeInterval.Text = "Update Refresh Interval (sec)";
            this.ultraButtonUpdateRefreshTimeInterval.Click += new System.EventHandler(this.ultraButtonUpdateRefreshTimeInterval_Click);
            // 
            // ultraLabelPricingService
            // 
            this.ultraLabelPricingService.Location = new System.Drawing.Point(777, 9);
            this.ultraLabelPricingService.Name = "ultraLabelPricingService";
            this.ultraLabelPricingService.Size = new System.Drawing.Size(84, 17);
            this.ultraLabelPricingService.TabIndex = 15;
            this.ultraLabelPricingService.Text = "PricingService2";
            // 
            // ultraLabelTradeService
            // 
            this.ultraLabelTradeService.Location = new System.Drawing.Point(777, 32);
            this.ultraLabelTradeService.Name = "ultraLabelTradeService";
            this.ultraLabelTradeService.Size = new System.Drawing.Size(84, 17);
            this.ultraLabelTradeService.TabIndex = 18;
            this.ultraLabelTradeService.Text = "TradeService";
            // 
            // ultraLabelLivefeed
            // 
            this.ultraLabelLivefeed.Location = new System.Drawing.Point(777, 78);
            this.ultraLabelLivefeed.Name = "ultraLabelLivefeed";
            this.ultraLabelLivefeed.Size = new System.Drawing.Size(84, 17);
            this.ultraLabelLivefeed.TabIndex = 24;
            this.ultraLabelLivefeed.Text = "Live Feed";
            // 
            // ultraLabelMessage
            // 
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.Red;
            this.ultraLabelMessage.Appearance = appearance1;
            this.ultraLabelMessage.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelMessage.Location = new System.Drawing.Point(9, 80);
            this.ultraLabelMessage.Name = "ultraLabelMessage";
            this.ultraLabelMessage.Size = new System.Drawing.Size(511, 17);
            this.ultraLabelMessage.TabIndex = 11;
            this.ultraLabelMessage.Text = "* User would receive the unallocated data only if he has permission for all the a" +
    "ccounts";
            // 
            // listBoxErrorMessages
            // 
            this.listBoxErrorMessages.FormattingEnabled = true;
            this.listBoxErrorMessages.HorizontalScrollbar = true;
            this.listBoxErrorMessages.Location = new System.Drawing.Point(9, 102);
            this.listBoxErrorMessages.Name = "listBoxErrorMessages";
            this.listBoxErrorMessages.Size = new System.Drawing.Size(762, 290);
            this.listBoxErrorMessages.TabIndex = 12;
            // 
            // listBoxClientsConnected
            // 
            this.listBoxClientsConnected.FormattingEnabled = true;
            this.listBoxClientsConnected.Location = new System.Drawing.Point(777, 154);
            this.listBoxClientsConnected.Name = "listBoxClientsConnected";
            this.listBoxClientsConnected.Size = new System.Drawing.Size(184, 238);
            this.listBoxClientsConnected.TabIndex = 28;
            // 
            // ultraButtonRefreshData
            // 
            this.ultraButtonRefreshData.Enabled = false;
            this.ultraButtonRefreshData.Location = new System.Drawing.Point(9, 395);
            this.ultraButtonRefreshData.Name = "ultraButtonRefreshData";
            this.ultraButtonRefreshData.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonRefreshData.TabIndex = 13;
            this.ultraButtonRefreshData.Text = "Refresh Data";
            this.ultraButtonRefreshData.Click += new System.EventHandler(this.ultraButtonRefreshData_Click);
            // 
            // ultraButtonDataDumper
            // 
            this.ultraButtonDataDumper.Enabled = false;
            this.ultraButtonDataDumper.Location = new System.Drawing.Point(589, 9);
            this.ultraButtonDataDumper.Name = "ultraButtonDataDumper";
            this.ultraButtonDataDumper.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonDataDumper.TabIndex = 4;
            this.ultraButtonDataDumper.Text = "Start Data Dumper";
            this.ultraButtonDataDumper.Visible = false;
            this.ultraButtonDataDumper.Click += new System.EventHandler(this.ultraButtonDataDumper_Click);
            // 
            // ultraButtonDisconnectUser
            // 
            this.ultraButtonDisconnectUser.Enabled = false;
            this.ultraButtonDisconnectUser.Location = new System.Drawing.Point(846, 395);
            this.ultraButtonDisconnectUser.Name = "ultraButtonDisconnectUser";
            this.ultraButtonDisconnectUser.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonDisconnectUser.TabIndex = 29;
            this.ultraButtonDisconnectUser.Text = "Disconnect User";
            this.ultraButtonDisconnectUser.Click += new System.EventHandler(this.ultraButtonDisconnectUser_Click);
            // 
            // ultraLabelClientsConnected
            // 
            this.ultraLabelClientsConnected.Location = new System.Drawing.Point(777, 133);
            this.ultraLabelClientsConnected.Name = "ultraLabelClientsConnected";
            this.ultraLabelClientsConnected.Size = new System.Drawing.Size(199, 15);
            this.ultraLabelClientsConnected.TabIndex = 27;
            this.ultraLabelClientsConnected.Text = "Clients Connected to ExpnlService";
            // 
            // numericUpDownRefreshInterval
            // 
            this.numericUpDownRefreshInterval.Enabled = false;
            this.numericUpDownRefreshInterval.Location = new System.Drawing.Point(9, 47);
            this.numericUpDownRefreshInterval.Maximum = new decimal(new int[] {
            28800,
            0,
            0,
            0});
            this.numericUpDownRefreshInterval.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownRefreshInterval.Name = "numericUpDownRefreshInterval";
            this.numericUpDownRefreshInterval.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownRefreshInterval.TabIndex = 9;
            this.numericUpDownRefreshInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // ultraLabelColon1
            // 
            this.ultraLabelColon1.Location = new System.Drawing.Point(864, 9);
            this.ultraLabelColon1.Name = "ultraLabelColon1";
            this.ultraLabelColon1.Size = new System.Drawing.Size(10, 17);
            this.ultraLabelColon1.TabIndex = 16;
            this.ultraLabelColon1.Text = ":";
            // 
            // ultraLabelColon2
            // 
            this.ultraLabelColon2.Location = new System.Drawing.Point(865, 32);
            this.ultraLabelColon2.Name = "ultraLabelColon2";
            this.ultraLabelColon2.Size = new System.Drawing.Size(10, 17);
            this.ultraLabelColon2.TabIndex = 19;
            this.ultraLabelColon2.Text = ":";
            // 
            // ultraLabelColon4
            // 
            this.ultraLabelColon4.Location = new System.Drawing.Point(865, 78);
            this.ultraLabelColon4.Name = "ultraLabelColon4";
            this.ultraLabelColon4.Size = new System.Drawing.Size(10, 17);
            this.ultraLabelColon4.TabIndex = 25;
            this.ultraLabelColon4.Text = ":";
            // 
            // ultraLabelPricingServiceConnectionStatus
            // 
            this.ultraLabelPricingServiceConnectionStatus.Location = new System.Drawing.Point(877, 9);
            this.ultraLabelPricingServiceConnectionStatus.Name = "ultraLabelPricingServiceConnectionStatus";
            this.ultraLabelPricingServiceConnectionStatus.Size = new System.Drawing.Size(84, 17);
            this.ultraLabelPricingServiceConnectionStatus.TabIndex = 17;
            this.ultraLabelPricingServiceConnectionStatus.Text = "Disconnected";
            // 
            // ultraLabelTradeServiceConnectionStatus
            // 
            this.ultraLabelTradeServiceConnectionStatus.Location = new System.Drawing.Point(877, 32);
            this.ultraLabelTradeServiceConnectionStatus.Name = "ultraLabelTradeServiceConnectionStatus";
            this.ultraLabelTradeServiceConnectionStatus.Size = new System.Drawing.Size(84, 17);
            this.ultraLabelTradeServiceConnectionStatus.TabIndex = 20;
            this.ultraLabelTradeServiceConnectionStatus.Text = "Disconnected";
            // 
            // ultraLabelLivefeedStatus
            // 
            this.ultraLabelLivefeedStatus.Location = new System.Drawing.Point(877, 78);
            this.ultraLabelLivefeedStatus.Name = "ultraLabelLivefeedStatus";
            this.ultraLabelLivefeedStatus.Size = new System.Drawing.Size(84, 17);
            this.ultraLabelLivefeedStatus.TabIndex = 26;
            this.ultraLabelLivefeedStatus.Text = "Disconnected";
            // 
            // ultraCheckEditorDebugMode
            // 
            this.ultraCheckEditorDebugMode.Enabled = false;
            this.ultraCheckEditorDebugMode.Location = new System.Drawing.Point(138, 396);
            this.ultraCheckEditorDebugMode.Name = "ultraCheckEditorDebugMode";
            this.ultraCheckEditorDebugMode.Size = new System.Drawing.Size(120, 20);
            this.ultraCheckEditorDebugMode.TabIndex = 14;
            this.ultraCheckEditorDebugMode.Text = "DEV Testing Mode";
            this.ultraCheckEditorDebugMode.CheckedChanged += new System.EventHandler(this.ultraCheckEditorDebugMode_CheckedChanged);
            // 
            // ultraLabelExpnlServiceConnectionStatus
            // 
            this.ultraLabelExpnlServiceConnectionStatus.Location = new System.Drawing.Point(877, 55);
            this.ultraLabelExpnlServiceConnectionStatus.Name = "ultraLabelExpnlServiceConnectionStatus";
            this.ultraLabelExpnlServiceConnectionStatus.Size = new System.Drawing.Size(84, 17);
            this.ultraLabelExpnlServiceConnectionStatus.TabIndex = 23;
            this.ultraLabelExpnlServiceConnectionStatus.Text = "Disconnected";
            // 
            // ultraLabelColon3
            // 
            this.ultraLabelColon3.Location = new System.Drawing.Point(865, 55);
            this.ultraLabelColon3.Name = "ultraLabelColon3";
            this.ultraLabelColon3.Size = new System.Drawing.Size(10, 17);
            this.ultraLabelColon3.TabIndex = 22;
            this.ultraLabelColon3.Text = ":";
            // 
            // ultraLabelExpnlService
            // 
            this.ultraLabelExpnlService.Location = new System.Drawing.Point(777, 55);
            this.ultraLabelExpnlService.Name = "ultraLabelExpnlService";
            this.ultraLabelExpnlService.Size = new System.Drawing.Size(84, 17);
            this.ultraLabelExpnlService.TabIndex = 21;
            this.ultraLabelExpnlService.Text = "ExpnlService";
            // 
            // ultraButtonStopServiceAndUI
            // 
            this.ultraButtonStopServiceAndUI.Enabled = false;
            this.ultraButtonStopServiceAndUI.Location = new System.Drawing.Point(473, 9);
            this.ultraButtonStopServiceAndUI.Name = "ultraButtonStopServiceAndUI";
            this.ultraButtonStopServiceAndUI.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonStopServiceAndUI.TabIndex = 5;
            this.ultraButtonStopServiceAndUI.Text = "Close Service && UI";
            this.ultraButtonStopServiceAndUI.Click += new System.EventHandler(this.ultraButtonStopServiceAndUI_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(103, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // ultraLabelCompressionName
            // 
            this.ultraLabelCompressionName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelCompressionName.Location = new System.Drawing.Point(473, 52);
            this.ultraLabelCompressionName.Margin = new System.Windows.Forms.Padding(0);
            this.ultraLabelCompressionName.Name = "ultraLabelCompressionName";
            this.ultraLabelCompressionName.Size = new System.Drawing.Size(231, 15);
            this.ultraLabelCompressionName.TabIndex = 31;
            // 
            // ultraButtonLoadLog
            // 
            this.ultraButtonLoadLog.Enabled = false;
            this.ultraButtonLoadLog.Location = new System.Drawing.Point(125, 9);
            this.ultraButtonLoadLog.Name = "ultraButtonLoadLog";
            this.ultraButtonLoadLog.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonLoadLog.TabIndex = 32;
            this.ultraButtonLoadLog.Text = "Load Log";
            this.ultraButtonLoadLog.Click += new System.EventHandler(this.ultraButtonLoadLog_Click);
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
            this.panelClientsConnected.Location = new System.Drawing.Point(777, 127);
            this.panelClientsConnected.Name = "panelClientsConnected";
            this.panelClientsConnected.Size = new System.Drawing.Size(184, 265);
            this.panelClientsConnected.TabIndex = 34;
            this.panelClientsConnected.Visible = false;
            // 
            // pictureBoxLoading
            // 
            this.pictureBoxLoading.BackColor = System.Drawing.Color.White;
            this.pictureBoxLoading.Location = new System.Drawing.Point(56, 89);
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
            this.listBoxClientServicesStatus.Location = new System.Drawing.Point(0, 0);
            this.listBoxClientServicesStatus.Name = "listBoxClientServicesStatus";
            this.listBoxClientServicesStatus.Size = new System.Drawing.Size(184, 265);
            this.listBoxClientServicesStatus.TabIndex = 14;
            this.listBoxClientServicesStatus.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxClientServicesStatus_DrawItem);
            // 
            // ultraDropDownButtonClientServicesStatus
            // 
            this.ultraDropDownButtonClientServicesStatus.Location = new System.Drawing.Point(777, 102);
            this.ultraDropDownButtonClientServicesStatus.Name = "ultraDropDownButtonClientServicesStatus";
            this.ultraDropDownButtonClientServicesStatus.PopupItemKey = "listBoxClientServicesStatus";
            this.ultraDropDownButtonClientServicesStatus.PopupItemProvider = this.ultraPopupControlContainerClientServicesStatus;
            this.ultraDropDownButtonClientServicesStatus.Size = new System.Drawing.Size(184, 24);
            this.ultraDropDownButtonClientServicesStatus.Style = Infragistics.Win.Misc.SplitButtonDisplayStyle.DropDownButtonOnly;
            this.ultraDropDownButtonClientServicesStatus.TabIndex = 33;
            this.ultraDropDownButtonClientServicesStatus.Text = "Connected Services Status";
            // 
            // ExpnlServiceUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 423);
            this.Controls.Add(this.panelClientsConnected);
            this.Controls.Add(this.ultraDropDownButtonClientServicesStatus);
            this.Controls.Add(this.ultraButtonLoadLog);
            this.Controls.Add(this.ultraLabelCompressionName);
            this.Controls.Add(this.ultraButtonStopServiceAndUI);
            this.Controls.Add(this.ultraLabelExpnlServiceConnectionStatus);
            this.Controls.Add(this.ultraLabelColon3);
            this.Controls.Add(this.ultraLabelExpnlService);
            this.Controls.Add(this.ultraCheckEditorDebugMode);
            this.Controls.Add(this.ultraLabelLivefeedStatus);
            this.Controls.Add(this.ultraLabelTradeServiceConnectionStatus);
            this.Controls.Add(this.ultraLabelPricingServiceConnectionStatus);
            this.Controls.Add(this.ultraLabelColon4);
            this.Controls.Add(this.ultraLabelColon2);
            this.Controls.Add(this.ultraLabelColon1);
            this.Controls.Add(this.numericUpDownRefreshInterval);
            this.Controls.Add(this.ultraLabelClientsConnected);
            this.Controls.Add(this.ultraButtonDisconnectUser);
            this.Controls.Add(this.ultraButtonDataDumper);
            this.Controls.Add(this.ultraButtonRefreshData);
            this.Controls.Add(this.listBoxClientsConnected);
            this.Controls.Add(this.listBoxErrorMessages);
            this.Controls.Add(this.ultraLabelMessage);
            this.Controls.Add(this.ultraLabelLivefeed);
            this.Controls.Add(this.ultraLabelTradeService);
            this.Controls.Add(this.ultraLabelPricingService);
            this.Controls.Add(this.ultraButtonUpdateRefreshTimeInterval);
            this.Controls.Add(this.ultraButtonClearanceSetup);
            this.Controls.Add(this.ultraButtonClearLog);
            this.Controls.Add(this.ultraButtonOpenLog);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(986, 462);
            this.MinimumSize = new System.Drawing.Size(986, 462);
            this.Name = "ExpnlServiceUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prana ExpnlService UI";
            this.Load += new System.EventHandler(this.ExpnlServiceUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRefreshInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditorDebugMode)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panelClientsConnected.ClientArea.ResumeLayout(false);
            this.panelClientsConnected.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton ultraButtonOpenLog;
        private Infragistics.Win.Misc.UltraButton ultraButtonClearLog;
        private Infragistics.Win.Misc.UltraButton ultraButtonClearanceSetup;
        private Infragistics.Win.Misc.UltraButton ultraButtonUpdateRefreshTimeInterval;
        private Infragistics.Win.Misc.UltraLabel ultraLabelPricingService;
        private Infragistics.Win.Misc.UltraLabel ultraLabelTradeService;
        private Infragistics.Win.Misc.UltraLabel ultraLabelLivefeed;
        private Infragistics.Win.Misc.UltraLabel ultraLabelMessage;
        private System.Windows.Forms.ListBox listBoxErrorMessages;
        private System.Windows.Forms.ListBox listBoxClientsConnected;
        private Infragistics.Win.Misc.UltraButton ultraButtonRefreshData;
        private Infragistics.Win.Misc.UltraButton ultraButtonDataDumper;
        private Infragistics.Win.Misc.UltraButton ultraButtonDisconnectUser;
        private Infragistics.Win.Misc.UltraLabel ultraLabelClientsConnected;
        private System.Windows.Forms.NumericUpDown numericUpDownRefreshInterval;
        private Infragistics.Win.Misc.UltraLabel ultraLabelColon1;
        private Infragistics.Win.Misc.UltraLabel ultraLabelColon2;
        private Infragistics.Win.Misc.UltraLabel ultraLabelColon4;
        private Infragistics.Win.Misc.UltraLabel ultraLabelPricingServiceConnectionStatus;
        private Infragistics.Win.Misc.UltraLabel ultraLabelTradeServiceConnectionStatus;
        private Infragistics.Win.Misc.UltraLabel ultraLabelLivefeedStatus;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraCheckEditorDebugMode;
        private Infragistics.Win.Misc.UltraLabel ultraLabelExpnlServiceConnectionStatus;
        private Infragistics.Win.Misc.UltraLabel ultraLabelColon3;
        private Infragistics.Win.Misc.UltraLabel ultraLabelExpnlService;
        private Infragistics.Win.Misc.UltraButton ultraButtonStopServiceAndUI;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private Infragistics.Win.Misc.UltraLabel ultraLabelCompressionName;
        private Infragistics.Win.Misc.UltraButton ultraButtonLoadLog;
        private Infragistics.Win.Misc.UltraPopupControlContainer ultraPopupControlContainerClientServicesStatus;
        private Infragistics.Win.Misc.UltraDropDownButton ultraDropDownButtonClientServicesStatus;
        private Infragistics.Win.Misc.UltraPanel panelClientsConnected;
        private System.Windows.Forms.PictureBox pictureBoxLoading;
        private System.Windows.Forms.ListBox listBoxClientServicesStatus;
    }
}

