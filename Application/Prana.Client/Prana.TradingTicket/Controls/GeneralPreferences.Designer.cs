namespace Prana.TradingTicket.Controls
{
    partial class TTGeneralPreferencesControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.label1 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbSymbology = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblOptionType = new Infragistics.Win.Misc.UltraLabel();
            this.chkShowOptionDetails = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.cmbOptionType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.chkKeepTTOpen = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkPopulatelastPriceInPriceWhenAskORBidIsZero = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtDefaultICs = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtBrokerComments = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.chkUseCustodianAsExecutingBroker = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSymbology)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowOptionDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOptionType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkKeepTTOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPopulatelastPriceInPriceWhenAskORBidIsZero)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDefaultICs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBrokerComments)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseCustodianAsExecutingBroker)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Default Symbology:";
            // 
            // cmbSymbology
            // 
            this.cmbSymbology.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbSymbology.LimitToList = true;
            this.cmbSymbology.Location = new System.Drawing.Point(13, 40);
            this.cmbSymbology.Name = "cmbSymbology";
            this.cmbSymbology.Nullable = false;
            this.cmbSymbology.Size = new System.Drawing.Size(122, 21);
            this.cmbSymbology.TabIndex = 1;
            // 
            // lblOptionType
            // 
            this.lblOptionType.AutoSize = true;
            this.lblOptionType.Location = new System.Drawing.Point(162, 15);
            this.lblOptionType.Name = "lblOptionType";
            this.lblOptionType.Size = new System.Drawing.Size(108, 14);
            this.lblOptionType.TabIndex = 2;
            this.lblOptionType.Text = "Default Option Type:";
            // 
            // chkShowOptionDetails
            // 
            this.chkShowOptionDetails.AutoSize = true;
            this.chkShowOptionDetails.Location = new System.Drawing.Point(298, 44);
            this.chkShowOptionDetails.Name = "chkShowOptionDetails";
            this.chkShowOptionDetails.Size = new System.Drawing.Size(124, 17);
            this.chkShowOptionDetails.TabIndex = 6;
            this.chkShowOptionDetails.Text = "Show Option Details";
            // 
            // chkUseCustodianAsExecutingBroker
            // 
            this.chkUseCustodianAsExecutingBroker.AutoSize = true;
            this.chkUseCustodianAsExecutingBroker.Location = new System.Drawing.Point(298, 68);
            this.chkUseCustodianAsExecutingBroker.Name = "chkUseCustodianAsExecutingBroker";
            this.chkUseCustodianAsExecutingBroker.Size = new System.Drawing.Size(200, 20);
            this.chkUseCustodianAsExecutingBroker.TabIndex = 22;
            this.chkUseCustodianAsExecutingBroker.Text = "Use custodian as Executing Broker";
            // 
            // cmbOptionType
            // 
            this.cmbOptionType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbOptionType.LimitToList = true;
            this.cmbOptionType.Location = new System.Drawing.Point(161, 40);
            this.cmbOptionType.Name = "cmbOptionType";
            this.cmbOptionType.Nullable = false;
            this.cmbOptionType.Size = new System.Drawing.Size(110, 21);
            this.cmbOptionType.TabIndex = 7;
            // 
            // chkKeepTTOpen
            // 
            this.chkKeepTTOpen.AutoSize = true;
            this.chkKeepTTOpen.Location = new System.Drawing.Point(298, 19);
            this.chkKeepTTOpen.Name = "chkKeepTTOpen";
            this.chkKeepTTOpen.Size = new System.Drawing.Size(211, 17);
            this.chkKeepTTOpen.TabIndex = 11;
            this.chkKeepTTOpen.Text = "Keep Trading Ticket Open after Trade";


            // 
            // chkPopulatelastPriceInPriceWhenAskORBidIsZero
            // 
            this.chkPopulatelastPriceInPriceWhenAskORBidIsZero.AutoSize = true;
            this.chkPopulatelastPriceInPriceWhenAskORBidIsZero.Location = new System.Drawing.Point(10, 140);
            this.chkPopulatelastPriceInPriceWhenAskORBidIsZero.Name = "chkPopulatelastPriceInPriceWhenAskORBidIsZero";
            this.chkPopulatelastPriceInPriceWhenAskORBidIsZero.Size = new System.Drawing.Size(211, 17);
            this.chkPopulatelastPriceInPriceWhenAskORBidIsZero.TabIndex = 20;
            this.chkPopulatelastPriceInPriceWhenAskORBidIsZero.Text = "Populate last price in Price/Limit when Ask/Bid is 0";


            // 
            // txtDefaultICs
            // 
            this.txtDefaultICs.Location = new System.Drawing.Point(10, 100);
            this.txtDefaultICs.Name = "txtDefaultICs";
            this.txtDefaultICs.NullText = "Default Notes";
            appearance1.ForeColor = System.Drawing.Color.DarkGray;
            this.txtDefaultICs.NullTextAppearance = appearance1;
            this.txtDefaultICs.Size = new System.Drawing.Size(260, 21);
            this.txtDefaultICs.TabIndex = 19;
            // 
            // txtBrokerComments
            // 
            this.txtBrokerComments.Location = new System.Drawing.Point(298, 100);
            this.txtBrokerComments.Name = "txtBrokerComments";
            this.txtBrokerComments.NullText = "Default Broker Notes";
            appearance2.ForeColor = System.Drawing.Color.DarkGray;
            this.txtBrokerComments.NullTextAppearance = appearance2;
            this.txtBrokerComments.Size = new System.Drawing.Size(260, 21);
            this.txtBrokerComments.TabIndex = 21;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.chkUseCustodianAsExecutingBroker);
            this.ultraPanel1.ClientArea.Controls.Add(this.txtDefaultICs);
            this.ultraPanel1.ClientArea.Controls.Add(this.txtBrokerComments); 
            this.ultraPanel1.ClientArea.Controls.Add(this.chkPopulatelastPriceInPriceWhenAskORBidIsZero); 
            this.ultraPanel1.ClientArea.Controls.Add(this.chkShowOptionDetails);
            this.ultraPanel1.ClientArea.Controls.Add(this.chkKeepTTOpen);
            this.ultraPanel1.ClientArea.Controls.Add(this.cmbOptionType);
            this.ultraPanel1.ClientArea.Controls.Add(this.label1);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblOptionType);
            this.ultraPanel1.ClientArea.Controls.Add(this.cmbSymbology);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(600, 442);
            this.ultraPanel1.TabIndex = 22;
            // 
            // TTGeneralPreferencesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "TTGeneralPreferencesControl";
            this.Size = new System.Drawing.Size(600, 442);
            ((System.ComponentModel.ISupportInitialize)(this.cmbSymbology)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowOptionDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOptionType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkKeepTTOpen)).EndInit(); 
            ((System.ComponentModel.ISupportInitialize)(this.chkPopulatelastPriceInPriceWhenAskORBidIsZero)).EndInit(); 
            ((System.ComponentModel.ISupportInitialize)(this.txtDefaultICs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBrokerComments)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkUseCustodianAsExecutingBroker)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel label1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbSymbology;
        private Infragistics.Win.Misc.UltraLabel lblOptionType;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkShowOptionDetails;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbOptionType;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkKeepTTOpen;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkPopulatelastPriceInPriceWhenAskORBidIsZero;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDefaultICs;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtBrokerComments;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkUseCustodianAsExecutingBroker;
    }
}
