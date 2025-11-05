namespace Prana.PricingService2UI
{
    partial class PricingService2UI
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
                if (_pricingService2ClientHeartbeatManager != null)
                {
                    _pricingService2ClientHeartbeatManager.Dispose();
                }

                if (_subscriptionProxy != null)
                {
                    _subscriptionProxy.Dispose();
                }
                if(dm != null)
                {
                    dm.Dispose();
                }
                if(_marketDataMonitoringUI != null)
                {
                    _marketDataMonitoringUI.Dispose();
                }
                if (dataViwer != null)
                {
                    dataViwer.Dispose();
                }
                if (_bloombergPropertiesUI != null)
                {
                    _bloombergPropertiesUI.Dispose();
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
            this.ultraPanelHeaderOptions = new Infragistics.Win.Misc.UltraPanel();
            this.ultraLabelMarketDataProviderValue = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelPricingServiceConnectionStatus = new Infragistics.Win.Misc.UltraLabel();
            this.comboBoxPricingModels = new System.Windows.Forms.ComboBox();
            this.ultraLabelMarketDataProvider = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelLivefeedStatus = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButtonUpdateValues = new Infragistics.Win.Misc.UltraButton();
            this.numericUpDownVolatilityIterations = new System.Windows.Forms.NumericUpDown();
            this.ultraLabelVolatilityIterations = new Infragistics.Win.Misc.UltraLabel();
            this.numericUpDownBinomialSteps = new System.Windows.Forms.NumericUpDown();
            this.ultraLabelBinomialSteps = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelPricingModels = new Infragistics.Win.Misc.UltraLabel();
            this.btnMarketDataMonitoring = new Infragistics.Win.Misc.UltraButton();
            this.listBoxErrorMessages = new System.Windows.Forms.ListBox();
            this.listBoxClientsConnected = new System.Windows.Forms.ListBox();
            this.ultraButtonDisconnectUser = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonClearLog = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonDataManager = new Infragistics.Win.Misc.UltraButton();
            this.ultraCheckEditorDebugMode = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraCheckEditorRiskLogging = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabelClientsConnected = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButtonOpenLog = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonStopServiceAndUI = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonLoadLog = new Infragistics.Win.Misc.UltraButton();
            this.ultraDropDownButtonClientServicesStatus = new Infragistics.Win.Misc.UltraDropDownButton();
            this.ultraPopupControlContainerClientServicesStatus = new Infragistics.Win.Misc.UltraPopupControlContainer(this.components);
            this.panelClientsConnected = new Infragistics.Win.Misc.UltraPanel();
            this.pictureBoxLoading = new System.Windows.Forms.PictureBox();
            this.listBoxClientServicesStatus = new System.Windows.Forms.ListBox();
            this.btnSecondaryMarketData = new Infragistics.Win.Misc.UltraButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ultraLabelLicenseType = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanelHeaderOptions.ClientArea.SuspendLayout();
            this.ultraPanelHeaderOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVolatilityIterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBinomialSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditorDebugMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditorRiskLogging)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog)).BeginInit();
            this.panelClientsConnected.ClientArea.SuspendLayout();
            this.panelClientsConnected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPanelHeaderOptions
            // 
            // 
            // ultraPanelHeaderOptions.ClientArea
            // 
            this.ultraPanelHeaderOptions.ClientArea.Controls.Add(this.ultraLabelLicenseType);
            this.ultraPanelHeaderOptions.ClientArea.Controls.Add(this.ultraLabelMarketDataProviderValue);
            this.ultraPanelHeaderOptions.ClientArea.Controls.Add(this.ultraLabelPricingServiceConnectionStatus);
            this.ultraPanelHeaderOptions.ClientArea.Controls.Add(this.comboBoxPricingModels);
            this.ultraPanelHeaderOptions.ClientArea.Controls.Add(this.ultraLabelMarketDataProvider);
            this.ultraPanelHeaderOptions.ClientArea.Controls.Add(this.ultraLabelLivefeedStatus);
            this.ultraPanelHeaderOptions.ClientArea.Controls.Add(this.ultraButtonUpdateValues);
            this.ultraPanelHeaderOptions.ClientArea.Controls.Add(this.numericUpDownVolatilityIterations);
            this.ultraPanelHeaderOptions.ClientArea.Controls.Add(this.ultraLabelVolatilityIterations);
            this.ultraPanelHeaderOptions.ClientArea.Controls.Add(this.numericUpDownBinomialSteps);
            this.ultraPanelHeaderOptions.ClientArea.Controls.Add(this.ultraLabelBinomialSteps);
            this.ultraPanelHeaderOptions.ClientArea.Controls.Add(this.ultraLabelPricingModels);
            this.ultraPanelHeaderOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanelHeaderOptions.Location = new System.Drawing.Point(0, 0);
            this.ultraPanelHeaderOptions.Name = "ultraPanelHeaderOptions";
            this.ultraPanelHeaderOptions.Size = new System.Drawing.Size(1158, 53);
            this.ultraPanelHeaderOptions.TabIndex = 5;
            // 
            // ultraButtonFactSetMonitoring
            // 
            this.btnMarketDataMonitoring.Enabled = false;
            this.btnMarketDataMonitoring.Location = new System.Drawing.Point(356, 394);
            this.btnMarketDataMonitoring.Name = "ultraButtonMarketDataMonitoring";
            this.btnMarketDataMonitoring.Size = new System.Drawing.Size(113, 23);
            this.btnMarketDataMonitoring.TabIndex = 5;
            this.btnMarketDataMonitoring.Text = "MarketData Details";
            this.btnMarketDataMonitoring.Visible = false;
            this.btnMarketDataMonitoring.Click += new System.EventHandler(this.ultraButtonDataManager_Click);
            // 
            // ultraLabelMarketDataProviderValue
            // 
            this.ultraLabelMarketDataProviderValue.Location = new System.Drawing.Point(1066, 12);
            this.ultraLabelMarketDataProviderValue.Name = "ultraLabelMarketDataProviderValue";
            this.ultraLabelMarketDataProviderValue.Size = new System.Drawing.Size(83, 14);
            this.ultraLabelMarketDataProviderValue.TabIndex = 10;
            this.ultraLabelMarketDataProviderValue.Visible = false;
            // 
            // ultraLabelPricingServiceConnectionStatus
            // 
            this.ultraLabelPricingServiceConnectionStatus.Location = new System.Drawing.Point(766, 12);
            this.ultraLabelPricingServiceConnectionStatus.Name = "ultraLabelPricingServiceConnectionStatus";
            this.ultraLabelPricingServiceConnectionStatus.Size = new System.Drawing.Size(179, 14);
            this.ultraLabelPricingServiceConnectionStatus.TabIndex = 7;
            this.ultraLabelPricingServiceConnectionStatus.Text = "PricingService2 Disconnected";
            // 
            // comboBoxPricingModels
            // 
            this.comboBoxPricingModels.Enabled = false;
            this.comboBoxPricingModels.FormattingEnabled = true;
            this.comboBoxPricingModels.Location = new System.Drawing.Point(7, 26);
            this.comboBoxPricingModels.Name = "comboBoxPricingModels";
            this.comboBoxPricingModels.Size = new System.Drawing.Size(273, 21);
            this.comboBoxPricingModels.TabIndex = 1;
            // 
            // ultraLabelMarketDataProvider
            // 
            this.ultraLabelMarketDataProvider.Location = new System.Drawing.Point(951, 12);
            this.ultraLabelMarketDataProvider.Name = "ultraLabelMarketDataProvider";
            this.ultraLabelMarketDataProvider.Size = new System.Drawing.Size(116, 14);
            this.ultraLabelMarketDataProvider.TabIndex = 9;
            this.ultraLabelMarketDataProvider.Text = "Market Data Provider:";
            this.ultraLabelMarketDataProvider.Visible = false;
            // 
            // ultraLabelLivefeedStatus
            // 
            this.ultraLabelLivefeedStatus.Location = new System.Drawing.Point(766, 33);
            this.ultraLabelLivefeedStatus.Name = "ultraLabelLivefeedStatus";
            this.ultraLabelLivefeedStatus.Size = new System.Drawing.Size(179, 14);
            this.ultraLabelLivefeedStatus.TabIndex = 8;
            this.ultraLabelLivefeedStatus.Text = "Livefeed Disconnected";
            // 
            // ultraButtonUpdateValues
            // 
            this.ultraButtonUpdateValues.Enabled = false;
            this.ultraButtonUpdateValues.Location = new System.Drawing.Point(497, 24);
            this.ultraButtonUpdateValues.Name = "ultraButtonUpdateValues";
            this.ultraButtonUpdateValues.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonUpdateValues.TabIndex = 6;
            this.ultraButtonUpdateValues.Text = "Update Values";
            this.ultraButtonUpdateValues.Click += new System.EventHandler(this.ultraButtonUpdateValues_Click);
            // 
            // numericUpDownVolatilityIterations
            // 
            this.numericUpDownVolatilityIterations.Enabled = false;
            this.numericUpDownVolatilityIterations.Location = new System.Drawing.Point(391, 26);
            this.numericUpDownVolatilityIterations.Name = "numericUpDownVolatilityIterations";
            this.numericUpDownVolatilityIterations.Size = new System.Drawing.Size(102, 20);
            this.numericUpDownVolatilityIterations.TabIndex = 5;
            // 
            // ultraLabelVolatilityIterations
            // 
            this.ultraLabelVolatilityIterations.Location = new System.Drawing.Point(391, 8);
            this.ultraLabelVolatilityIterations.Name = "ultraLabelVolatilityIterations";
            this.ultraLabelVolatilityIterations.Size = new System.Drawing.Size(102, 14);
            this.ultraLabelVolatilityIterations.TabIndex = 4;
            this.ultraLabelVolatilityIterations.Text = "Volatility Iterations";
            // 
            // numericUpDownBinomialSteps
            // 
            this.numericUpDownBinomialSteps.Enabled = false;
            this.numericUpDownBinomialSteps.Location = new System.Drawing.Point(284, 26);
            this.numericUpDownBinomialSteps.Name = "numericUpDownBinomialSteps";
            this.numericUpDownBinomialSteps.Size = new System.Drawing.Size(102, 20);
            this.numericUpDownBinomialSteps.TabIndex = 3;
            // 
            // ultraLabelBinomialSteps
            // 
            this.ultraLabelBinomialSteps.Location = new System.Drawing.Point(284, 8);
            this.ultraLabelBinomialSteps.Name = "ultraLabelBinomialSteps";
            this.ultraLabelBinomialSteps.Size = new System.Drawing.Size(102, 14);
            this.ultraLabelBinomialSteps.TabIndex = 2;
            this.ultraLabelBinomialSteps.Text = "Binomial Steps";
            // 
            // ultraLabelPricingModels
            // 
            this.ultraLabelPricingModels.Location = new System.Drawing.Point(7, 8);
            this.ultraLabelPricingModels.Name = "ultraLabelPricingModels";
            this.ultraLabelPricingModels.Size = new System.Drawing.Size(273, 14);
            this.ultraLabelPricingModels.TabIndex = 0;
            this.ultraLabelPricingModels.Text = "Pricing Models";
            // 
            // listBoxErrorMessages
            // 
            this.listBoxErrorMessages.FormattingEnabled = true;
            this.listBoxErrorMessages.HorizontalScrollbar = true;
            this.listBoxErrorMessages.Location = new System.Drawing.Point(7, 58);
            this.listBoxErrorMessages.Name = "listBoxErrorMessages";
            this.listBoxErrorMessages.Size = new System.Drawing.Size(938, 329);
            this.listBoxErrorMessages.TabIndex = 6;
            // 
            // listBoxClientsConnected
            // 
            this.listBoxClientsConnected.FormattingEnabled = true;
            this.listBoxClientsConnected.Location = new System.Drawing.Point(951, 110);
            this.listBoxClientsConnected.Name = "listBoxClientsConnected";
            this.listBoxClientsConnected.Size = new System.Drawing.Size(197, 277);
            this.listBoxClientsConnected.TabIndex = 8;
            // 
            // ultraButtonDisconnectUser
            // 
            this.ultraButtonDisconnectUser.Enabled = false;
            this.ultraButtonDisconnectUser.Location = new System.Drawing.Point(1033, 394);
            this.ultraButtonDisconnectUser.Name = "ultraButtonDisconnectUser";
            this.ultraButtonDisconnectUser.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonDisconnectUser.TabIndex = 9;
            this.ultraButtonDisconnectUser.Text = "Disconnect User";
            this.ultraButtonDisconnectUser.Click += new System.EventHandler(this.ultraButtonDisconnectUser_Click);
            // 
            // ultraButtonClearLog
            // 
            this.ultraButtonClearLog.Enabled = false;
            this.ultraButtonClearLog.Location = new System.Drawing.Point(215, 3);
            this.ultraButtonClearLog.Name = "ultraButtonClearLog";
            this.ultraButtonClearLog.Size = new System.Drawing.Size(100, 23);
            this.ultraButtonClearLog.TabIndex = 2;
            this.ultraButtonClearLog.Text = "Clear Log";
            this.ultraButtonClearLog.Click += new System.EventHandler(this.ultraButtonClearLog_Click);
            // 
            // ultraButtonDataManager
            // 
            this.ultraButtonDataManager.Enabled = false;
            this.ultraButtonDataManager.Location = new System.Drawing.Point(321, 3);
            this.ultraButtonDataManager.Name = "ultraButtonDataManager";
            this.ultraButtonDataManager.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonDataManager.TabIndex = 3;
            this.ultraButtonDataManager.Text = "Data Manager";
            this.ultraButtonDataManager.Visible = false;
            this.ultraButtonDataManager.Click += new System.EventHandler(this.ultraButtonDataManager_Click);
            // 
            // ultraCheckEditorDebugMode
            // 
            this.ultraCheckEditorDebugMode.Enabled = false;
            this.ultraCheckEditorDebugMode.Location = new System.Drawing.Point(760, 397);
            this.ultraCheckEditorDebugMode.Name = "ultraCheckEditorDebugMode";
            this.ultraCheckEditorDebugMode.Size = new System.Drawing.Size(92, 20);
            this.ultraCheckEditorDebugMode.TabIndex = 10;
            this.ultraCheckEditorDebugMode.Text = "Testing Mode";
            this.ultraCheckEditorDebugMode.CheckedChanged += new System.EventHandler(this.ultraCheckEditorDebugMode_CheckedChanged);
            // 
            // ultraCheckEditorRiskLogging
            // 
            this.ultraCheckEditorRiskLogging.Enabled = false;
            this.ultraCheckEditorRiskLogging.Location = new System.Drawing.Point(858, 397);
            this.ultraCheckEditorRiskLogging.Name = "ultraCheckEditorRiskLogging";
            this.ultraCheckEditorRiskLogging.Size = new System.Drawing.Size(87, 20);
            this.ultraCheckEditorRiskLogging.TabIndex = 11;
            this.ultraCheckEditorRiskLogging.Text = "Risk Logging";
            this.ultraCheckEditorRiskLogging.CheckedChanged += new System.EventHandler(this.ultraCheckEditorRiskLogging_CheckedChanged);
            // 
            // ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog
            // 
            this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog.Enabled = false;
            this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog.Location = new System.Drawing.Point(951, 395);
            this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog.Name = "ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog";
            this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog.Size = new System.Drawing.Size(72, 21);
            this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog.TabIndex = 12;
            this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog.ValueChanged += new System.EventHandler(this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog_ValueChanged);
            // 
            // ultraLabelClientsConnected
            // 
            this.ultraLabelClientsConnected.Location = new System.Drawing.Point(951, 90);
            this.ultraLabelClientsConnected.Name = "ultraLabelClientsConnected";
            this.ultraLabelClientsConnected.Size = new System.Drawing.Size(197, 14);
            this.ultraLabelClientsConnected.TabIndex = 7;
            this.ultraLabelClientsConnected.Text = "Clients Connected to PricingService2";
            // 
            // ultraButtonOpenLog
            // 
            this.ultraButtonOpenLog.Enabled = false;
            this.ultraButtonOpenLog.Location = new System.Drawing.Point(3, 3);
            this.ultraButtonOpenLog.Name = "ultraButtonOpenLog";
            this.ultraButtonOpenLog.Size = new System.Drawing.Size(100, 23);
            this.ultraButtonOpenLog.TabIndex = 1;
            this.ultraButtonOpenLog.Text = "Open Log";
            this.ultraButtonOpenLog.Click += new System.EventHandler(this.ultraButtonOpenLog_Click);
            // 
            // ultraButtonStopServiceAndUI
            // 
            this.ultraButtonStopServiceAndUI.Enabled = false;
            this.ultraButtonStopServiceAndUI.Location = new System.Drawing.Point(718, 3);
            this.ultraButtonStopServiceAndUI.Name = "ultraButtonStopServiceAndUI";
            this.ultraButtonStopServiceAndUI.Size = new System.Drawing.Size(115, 23);
            this.ultraButtonStopServiceAndUI.TabIndex = 4;
            this.ultraButtonStopServiceAndUI.Text = "Close Service && UI";
            this.ultraButtonStopServiceAndUI.Click += new System.EventHandler(this.ultraButtonStopServiceAndUI_Click);
            // 
            // ultraButtonLoadLog
            // 
            this.ultraButtonLoadLog.Enabled = false;
            this.ultraButtonLoadLog.Location = new System.Drawing.Point(109, 3);
            this.ultraButtonLoadLog.Name = "ultraButtonLoadLog";
            this.ultraButtonLoadLog.Size = new System.Drawing.Size(100, 23);
            this.ultraButtonLoadLog.TabIndex = 13;
            this.ultraButtonLoadLog.Text = "Load Log";
            this.ultraButtonLoadLog.Click += new System.EventHandler(this.ultraButtonLoadLog_Click);
            // 
            // ultraDropDownButtonClientServicesStatus
            // 
            this.ultraDropDownButtonClientServicesStatus.Location = new System.Drawing.Point(951, 58);
            this.ultraDropDownButtonClientServicesStatus.Name = "ultraDropDownButtonClientServicesStatus";
            this.ultraDropDownButtonClientServicesStatus.PopupItemKey = "panelClientsConnected";
            this.ultraDropDownButtonClientServicesStatus.PopupItemProvider = this.ultraPopupControlContainerClientServicesStatus;
            this.ultraDropDownButtonClientServicesStatus.Size = new System.Drawing.Size(197, 24);
            this.ultraDropDownButtonClientServicesStatus.Style = Infragistics.Win.Misc.SplitButtonDisplayStyle.DropDownButtonOnly;
            this.ultraDropDownButtonClientServicesStatus.TabIndex = 7;
            this.ultraDropDownButtonClientServicesStatus.Text = "Connected Services Status";
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
            this.panelClientsConnected.Location = new System.Drawing.Point(951, 83);
            this.panelClientsConnected.Name = "panelClientsConnected";
            this.panelClientsConnected.Size = new System.Drawing.Size(197, 304);
            this.panelClientsConnected.TabIndex = 16;
            this.panelClientsConnected.Visible = false;
            // 
            // pictureBoxLoading
            // 
            this.pictureBoxLoading.BackColor = System.Drawing.Color.White;
            this.pictureBoxLoading.Location = new System.Drawing.Point(61, 109);
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
            this.listBoxClientServicesStatus.ItemHeight = 15;
            this.listBoxClientServicesStatus.Location = new System.Drawing.Point(0, 0);
            this.listBoxClientServicesStatus.Name = "listBoxClientServicesStatus";
            this.listBoxClientServicesStatus.Size = new System.Drawing.Size(197, 304);
            this.listBoxClientServicesStatus.TabIndex = 14;
            this.listBoxClientServicesStatus.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxClientServicesStatus_DrawItem);
            // 
            // btnSecondaryMarketData
            // 
            this.btnSecondaryMarketData.Enabled = false;
            this.btnSecondaryMarketData.Location = new System.Drawing.Point(561, 3);
            this.btnSecondaryMarketData.Name = "btnSecondaryMarketData";
            this.btnSecondaryMarketData.Size = new System.Drawing.Size(151, 23);
            this.btnSecondaryMarketData.TabIndex = 17;
            this.btnSecondaryMarketData.Text = "Secondary Provider Details";
            this.btnSecondaryMarketData.Visible = false;
            this.btnSecondaryMarketData.Click += new System.EventHandler(this.ultraButtonSecondaryMarketDataDetail_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.ultraButtonOpenLog);
            this.flowLayoutPanel1.Controls.Add(this.ultraButtonLoadLog);
            this.flowLayoutPanel1.Controls.Add(this.ultraButtonClearLog);
            this.flowLayoutPanel1.Controls.Add(this.ultraButtonDataManager);
            this.flowLayoutPanel1.Controls.Add(this.btnMarketDataMonitoring);
            this.flowLayoutPanel1.Controls.Add(this.btnSecondaryMarketData);
            this.flowLayoutPanel1.Controls.Add(this.ultraButtonStopServiceAndUI);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 390);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(848, 33);
            this.flowLayoutPanel1.TabIndex = 18;
            // 
            // ultraLabelLicenseType
            // 
            this.ultraLabelLicenseType.Location = new System.Drawing.Point(951, 32);
            this.ultraLabelLicenseType.Name = "ultraLabelLicenseType";
            this.ultraLabelLicenseType.Size = new System.Drawing.Size(195, 14);
            this.ultraLabelLicenseType.TabIndex = 11;
            this.ultraLabelLicenseType.Text = "License type: Channel Partner";
            this.ultraLabelLicenseType.Visible = false;
            // 
            // PricingService2UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 423);
            this.Controls.Add(this.panelClientsConnected);
            this.Controls.Add(this.ultraDropDownButtonClientServicesStatus);
            this.Controls.Add(this.ultraLabelClientsConnected);
            this.Controls.Add(this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog);
            this.Controls.Add(this.ultraCheckEditorRiskLogging);
            this.Controls.Add(this.ultraCheckEditorDebugMode);
            this.Controls.Add(this.ultraButtonDisconnectUser);
            this.Controls.Add(this.listBoxClientsConnected);
            this.Controls.Add(this.listBoxErrorMessages);
            this.Controls.Add(this.ultraPanelHeaderOptions);
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1174, 462);
            this.MinimumSize = new System.Drawing.Size(1174, 462);
            this.Name = "PricingService2UI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prana PricingService2 UI";
            this.Load += new System.EventHandler(this.PricingService2UI_Load);
            this.ultraPanelHeaderOptions.ClientArea.ResumeLayout(false);
            this.ultraPanelHeaderOptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVolatilityIterations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBinomialSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditorDebugMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditorRiskLogging)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog)).EndInit();
            this.panelClientsConnected.ClientArea.ResumeLayout(false);
            this.panelClientsConnected.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanelHeaderOptions;
        private Infragistics.Win.Misc.UltraButton ultraButtonUpdateValues;
        private System.Windows.Forms.NumericUpDown numericUpDownVolatilityIterations;
        private Infragistics.Win.Misc.UltraLabel ultraLabelVolatilityIterations;
        private System.Windows.Forms.NumericUpDown numericUpDownBinomialSteps;
        private Infragistics.Win.Misc.UltraLabel ultraLabelBinomialSteps;
        private Infragistics.Win.Misc.UltraLabel ultraLabelPricingModels;
        private Infragistics.Win.Misc.UltraLabel ultraLabelMarketDataProvider;
        private Infragistics.Win.Misc.UltraLabel ultraLabelLivefeedStatus;
        private System.Windows.Forms.ListBox listBoxErrorMessages;
        private System.Windows.Forms.ListBox listBoxClientsConnected;
        private Infragistics.Win.Misc.UltraButton ultraButtonDisconnectUser;
        private Infragistics.Win.Misc.UltraButton ultraButtonClearLog;
        private Infragistics.Win.Misc.UltraButton ultraButtonDataManager;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraCheckEditorDebugMode;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraCheckEditorRiskLogging;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor ultraNumericEditorRiskAPIEachCalDurationThreasholdToLog;
        private Infragistics.Win.Misc.UltraLabel ultraLabelClientsConnected;
        private System.Windows.Forms.ComboBox comboBoxPricingModels;
        private Infragistics.Win.Misc.UltraLabel ultraLabelPricingServiceConnectionStatus;
        private Infragistics.Win.Misc.UltraButton ultraButtonOpenLog;
        private Infragistics.Win.Misc.UltraButton ultraButtonStopServiceAndUI;
        private Infragistics.Win.Misc.UltraButton ultraButtonLoadLog;
        private Infragistics.Win.Misc.UltraDropDownButton ultraDropDownButtonClientServicesStatus;
        private Infragistics.Win.Misc.UltraPopupControlContainer ultraPopupControlContainerClientServicesStatus;
        private System.Windows.Forms.ListBox listBoxClientServicesStatus;
        private System.Windows.Forms.PictureBox pictureBoxLoading;
        private Infragistics.Win.Misc.UltraPanel panelClientsConnected;
        private Infragistics.Win.Misc.UltraLabel ultraLabelMarketDataProviderValue;
        private Infragistics.Win.Misc.UltraButton btnMarketDataMonitoring;
        private Infragistics.Win.Misc.UltraButton btnSecondaryMarketData;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabelLicenseType;
    }
}

