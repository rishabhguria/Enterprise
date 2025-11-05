namespace Prana.CentralSM
{
    partial class CentralSmHost
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CentralSmHost));
            this.btStart = new Infragistics.Win.Misc.UltraButton();
            this.btStopServer = new Infragistics.Win.Misc.UltraButton();
            this.btReload = new Infragistics.Win.Misc.UltraButton();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.btOpenErrolLog = new Infragistics.Win.Misc.UltraButton();
            this.btnOpenTrace = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.listClientConnected = new System.Windows.Forms.ListBox();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.listErrorLog = new Infragistics.Win.UltraWinListView.UltraListView();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listErrorLog)).BeginInit();
            this.SuspendLayout();
            // 
            // btStart
            // 
            this.btStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btStart.Location = new System.Drawing.Point(3, 227);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 0;
            this.btStart.Text = "Start Server";
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btStopServer
            // 
            this.btStopServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btStopServer.Location = new System.Drawing.Point(84, 227);
            this.btStopServer.Name = "btStopServer";
            this.btStopServer.Size = new System.Drawing.Size(75, 23);
            this.btStopServer.TabIndex = 0;
            this.btStopServer.Text = "Stop Server";
            this.btStopServer.Click += new System.EventHandler(this.btStopServer_Click);
            // 
            // btReload
            // 
            this.btReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btReload.Location = new System.Drawing.Point(165, 227);
            this.btReload.Name = "btReload";
            this.btReload.Size = new System.Drawing.Size(75, 23);
            this.btReload.TabIndex = 2;
            this.btReload.Text = "Reload";
            this.btReload.Click += new System.EventHandler(this.btReload_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Location = new System.Drawing.Point(246, 227);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear Log";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btOpenErrolLog
            // 
            this.btOpenErrolLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btOpenErrolLog.Location = new System.Drawing.Point(327, 227);
            this.btOpenErrolLog.Name = "btOpenErrolLog";
            this.btOpenErrolLog.Size = new System.Drawing.Size(103, 23);
            this.btOpenErrolLog.TabIndex = 8;
            this.btOpenErrolLog.Text = "Open Error Log";
            this.btOpenErrolLog.Click += new System.EventHandler(this.btOpenErrolLog_Click);
            // 
            // btnOpenTrace
            // 
            this.btnOpenTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenTrace.Location = new System.Drawing.Point(436, 227);
            this.btnOpenTrace.Name = "btnOpenTrace";
            this.btnOpenTrace.Size = new System.Drawing.Size(75, 23);
            this.btnOpenTrace.TabIndex = 8;
            this.btnOpenTrace.Text = "Open Trace";
            this.btnOpenTrace.Click += new System.EventHandler(this.btnOpenTrace_Click);
            // 
            // ultraPanel1
            // 
            this.ultraPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.listClientConnected);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraSplitter1);
            this.ultraPanel1.ClientArea.Controls.Add(this.listErrorLog);
            this.ultraPanel1.Location = new System.Drawing.Point(3, -1);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(677, 222);
            this.ultraPanel1.TabIndex = 9;
            // 
            // listClientConnected
            // 
            this.listClientConnected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listClientConnected.FormattingEnabled = true;
            this.listClientConnected.Location = new System.Drawing.Point(561, 0);
            this.listClientConnected.Name = "listClientConnected";
            this.listClientConnected.Size = new System.Drawing.Size(116, 222);
            this.listClientConnected.TabIndex = 16;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.Location = new System.Drawing.Point(556, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 0;
            this.ultraSplitter1.Size = new System.Drawing.Size(5, 222);
            this.ultraSplitter1.TabIndex = 0;
            // 
            // listErrorLog
            // 
            this.listErrorLog.Dock = System.Windows.Forms.DockStyle.Left;
            this.listErrorLog.GroupHeadersVisible = Infragistics.Win.DefaultableBoolean.False;
            this.listErrorLog.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.listErrorLog.Location = new System.Drawing.Point(0, 0);
            this.listErrorLog.Name = "listErrorLog";
            this.listErrorLog.Size = new System.Drawing.Size(556, 222);
            this.listErrorLog.TabIndex = 17;
            this.listErrorLog.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.listErrorLog.ViewSettingsDetails.FullRowSelect = true;
            this.listErrorLog.ViewSettingsList.MultiColumn = false;
            // 
            // CentralSmHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 253);
            this.Controls.Add(this.btnOpenTrace);
            this.Controls.Add(this.btOpenErrolLog);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btReload);
            this.Controls.Add(this.btStopServer);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.ultraPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "CentralSmHost";
            this.Text = "Central SM Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CentralSmHost_FormClosing);
            this.Load += new System.EventHandler(this.CentralSmHost_Load);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listErrorLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btStart;
        private Infragistics.Win.Misc.UltraButton btStopServer;
        private Infragistics.Win.Misc.UltraButton btReload;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraButton btOpenErrolLog;
        private Infragistics.Win.Misc.UltraButton btnOpenTrace;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private System.Windows.Forms.ListBox listClientConnected;
        private Infragistics.Win.UltraWinListView.UltraListView listErrorLog;
    }
}

