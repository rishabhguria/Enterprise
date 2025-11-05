namespace Prana.LiveFeedEngine
{
    partial class frmLiveFeedEngineMain
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
            this.btnRestartLiveFeeds = new System.Windows.Forms.Button();
            this.btnClearLoggingWindow = new System.Windows.Forms.Button();
            this.btnOpenLogFile = new System.Windows.Forms.Button();
            this.btnStopEngine = new System.Windows.Forms.Button();
            this.btnStartEngine = new System.Windows.Forms.Button();
            this.btnDisconnectUser = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lstExpnlUsers = new System.Windows.Forms.ListBox();
            this.lblErrorLog = new System.Windows.Forms.Label();
            this.lstbxErrorLog = new System.Windows.Forms.ListBox();
            this.lbllivefeedstatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRestartLiveFeeds
            // 
            this.btnRestartLiveFeeds.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRestartLiveFeeds.Location = new System.Drawing.Point(403, 258);
            this.btnRestartLiveFeeds.Name = "btnRestartLiveFeeds";
            this.btnRestartLiveFeeds.Size = new System.Drawing.Size(103, 23);
            this.btnRestartLiveFeeds.TabIndex = 97;
            this.btnRestartLiveFeeds.Text = "Restart Livefeeds";
            this.btnRestartLiveFeeds.UseVisualStyleBackColor = true;
            this.btnRestartLiveFeeds.Click += new System.EventHandler(this.btnRestartLiveFeeds_Click);
            // 
            // btnClearLoggingWindow
            // 
            this.btnClearLoggingWindow.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClearLoggingWindow.Location = new System.Drawing.Point(254, 287);
            this.btnClearLoggingWindow.Name = "btnClearLoggingWindow";
            this.btnClearLoggingWindow.Size = new System.Drawing.Size(103, 23);
            this.btnClearLoggingWindow.TabIndex = 96;
            this.btnClearLoggingWindow.Text = "Clear Logging Window";
            this.btnClearLoggingWindow.UseVisualStyleBackColor = true;
            this.btnClearLoggingWindow.Click += new System.EventHandler(this.btnClearLoggingWindow_Click);
            // 
            // btnOpenLogFile
            // 
            this.btnOpenLogFile.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOpenLogFile.Location = new System.Drawing.Point(254, 258);
            this.btnOpenLogFile.Name = "btnOpenLogFile";
            this.btnOpenLogFile.Size = new System.Drawing.Size(103, 23);
            this.btnOpenLogFile.TabIndex = 95;
            this.btnOpenLogFile.Text = "Open Log File";
            this.btnOpenLogFile.UseVisualStyleBackColor = true;
            this.btnOpenLogFile.Click += new System.EventHandler(this.btnOpenLogFile_Click);
            // 
            // btnStopEngine
            // 
            this.btnStopEngine.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnStopEngine.Location = new System.Drawing.Point(99, 287);
            this.btnStopEngine.Name = "btnStopEngine";
            this.btnStopEngine.Size = new System.Drawing.Size(103, 23);
            this.btnStopEngine.TabIndex = 94;
            this.btnStopEngine.Text = "Stop Engine";
            this.btnStopEngine.UseVisualStyleBackColor = true;
            this.btnStopEngine.Click += new System.EventHandler(this.btnStopEngine_Click);
            // 
            // btnStartEngine
            // 
            this.btnStartEngine.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnStartEngine.Location = new System.Drawing.Point(99, 258);
            this.btnStartEngine.Name = "btnStartEngine";
            this.btnStartEngine.Size = new System.Drawing.Size(103, 23);
            this.btnStartEngine.TabIndex = 93;
            this.btnStartEngine.Text = "Start Engine";
            this.btnStartEngine.UseVisualStyleBackColor = true;
            this.btnStartEngine.Click += new System.EventHandler(this.btnStartEngine_Click);
            // 
            // btnDisconnectUser
            // 
            this.btnDisconnectUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisconnectUser.Location = new System.Drawing.Point(582, 258);
            this.btnDisconnectUser.Name = "btnDisconnectUser";
            this.btnDisconnectUser.Size = new System.Drawing.Size(103, 23);
            this.btnDisconnectUser.TabIndex = 102;
            this.btnDisconnectUser.Text = "Disconnect User";
            this.btnDisconnectUser.UseVisualStyleBackColor = true;
            this.btnDisconnectUser.Click += new System.EventHandler(this.btnDisconnectUser_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(579, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 101;
            this.label1.Text = "Connected Users";
            // 
            // lstExpnlUsers
            // 
            this.lstExpnlUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstExpnlUsers.FormattingEnabled = true;
            this.lstExpnlUsers.HorizontalScrollbar = true;
            this.lstExpnlUsers.Location = new System.Drawing.Point(566, 24);
            this.lstExpnlUsers.Name = "lstExpnlUsers";
            this.lstExpnlUsers.Size = new System.Drawing.Size(130, 225);
            this.lstExpnlUsers.TabIndex = 100;
            // 
            // lblErrorLog
            // 
            this.lblErrorLog.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblErrorLog.AutoSize = true;
            this.lblErrorLog.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblErrorLog.Location = new System.Drawing.Point(249, 8);
            this.lblErrorLog.Name = "lblErrorLog";
            this.lblErrorLog.Size = new System.Drawing.Size(58, 13);
            this.lblErrorLog.TabIndex = 99;
            this.lblErrorLog.Text = "Error Log";
            // 
            // lstbxErrorLog
            // 
            this.lstbxErrorLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstbxErrorLog.ForeColor = System.Drawing.Color.Red;
            this.lstbxErrorLog.HorizontalScrollbar = true;
            this.lstbxErrorLog.Items.AddRange(new object[] {
            ""});
            this.lstbxErrorLog.Location = new System.Drawing.Point(12, 24);
            this.lstbxErrorLog.Name = "lstbxErrorLog";
            this.lstbxErrorLog.Size = new System.Drawing.Size(548, 225);
            this.lstbxErrorLog.TabIndex = 98;
            // 
            // lbllivefeedstatus
            // 
            this.lbllivefeedstatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbllivefeedstatus.AutoSize = true;
            this.lbllivefeedstatus.Location = new System.Drawing.Point(560, 297);
            this.lbllivefeedstatus.Name = "lbllivefeedstatus";
            this.lbllivefeedstatus.Size = new System.Drawing.Size(117, 13);
            this.lbllivefeedstatus.TabIndex = 103;
            this.lbllivefeedstatus.Text = "Live Feed Connection :";
            // 
            // frmLiveFeedEngineMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 323);
            this.Controls.Add(this.lbllivefeedstatus);
            this.Controls.Add(this.btnDisconnectUser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstExpnlUsers);
            this.Controls.Add(this.lblErrorLog);
            this.Controls.Add(this.lstbxErrorLog);
            this.Controls.Add(this.btnRestartLiveFeeds);
            this.Controls.Add(this.btnClearLoggingWindow);
            this.Controls.Add(this.btnOpenLogFile);
            this.Controls.Add(this.btnStopEngine);
            this.Controls.Add(this.btnStartEngine);
            this.MinimumSize = new System.Drawing.Size(609, 272);
            this.Name = "frmLiveFeedEngineMain";
            this.Text = "LiveFeed Engine";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLiveFeedEngineMain_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLiveFeedEngineMain_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRestartLiveFeeds;
        private System.Windows.Forms.Button btnClearLoggingWindow;
        private System.Windows.Forms.Button btnOpenLogFile;
        private System.Windows.Forms.Button btnStopEngine;
        private System.Windows.Forms.Button btnStartEngine;
        private System.Windows.Forms.Button btnDisconnectUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstExpnlUsers;
        private System.Windows.Forms.Label lblErrorLog;
        private System.Windows.Forms.ListBox lstbxErrorLog;
        private System.Windows.Forms.Label lbllivefeedstatus;
    }
}

